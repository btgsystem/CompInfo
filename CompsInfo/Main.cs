using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraBars.Helpers;
using DevExpress.Skins;
using DevExpress.LookAndFeel;
using DevExpress.UserSkins;
using System.IO;
using System.Data.SqlClient;
using DevExpress.XtraEditors;
using System.Text.RegularExpressions;
using DevExpress.XtraEditors.Repository;


namespace CompsInfo
{
    public partial class Main : RibbonForm
    {
        private SqlConnection _connect;
        private DataTable dt,ExplDt;//----------------------------------------------

        private DataSet ds=new DataSet();

        private bool flag1=false, flag2=false;
        private string auds;
        private int InsertCount;
        public Main()
        {
            InitializeComponent();
            InitSkinGallery();
            _connect = Data.Value;
        }

        void InitSkinGallery()
        {
            SkinHelper.InitSkinGallery(rgbiSkins, true);
        }

        private void Open_ItemClick(object sender, ItemClickEventArgs e)
        {
            Stream myStream = null;
            auds = "";
            InsertCount = 0;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((myStream = openFileDialog1.OpenFile()) != null)
                    {
                        using (StreamReader sr = new StreamReader(myStream, System.Text.Encoding.GetEncoding(1251)))
                        {
                            ReadFile(sr, openFileDialog1.FileName); 
                        }                       
                    }
                   /* if(auds!="")
                        XtraMessageBox.Show(String.Format("В справочник добавлены аудитории: {0}", auds));*/
                    if(InsertCount>0)
                        XtraMessageBox.Show("Данные из файлов успешно добавлены в БД", "Статус", MessageBoxButtons.OK, MessageBoxIcon.Information); 
                    else
                        XtraMessageBox.Show("Данные из файлов уже существуют в БД", "Статус", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception msg)
                {
                    XtraMessageBox.Show("Чтение файла невозможно: " + msg.Message);
                }
            }
        }

        //------------------------------Чтение файла-----------------------------------
        private void ReadFile(StreamReader sr,string fName)
        {
            string txt, fileName, dir;
            string[] splitTxt, splitFile;
            txt = sr.ReadToEnd();
            splitTxt = txt.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < splitTxt.Length; i++)
                splitTxt[i] = System.Text.RegularExpressions.Regex.Replace(splitTxt[i], @"\s+", " ").Trim();
            dir = fName;
            fileName = Path.GetFileName(dir);
            splitFile = fileName.Split('-');
            SaveInDB(splitTxt, splitFile[0], ValidationAud(splitFile[1]), ValidationInv(splitFile[2].Replace(".txt", "")));
        }

        //-----------------------Валидация аудитории---(всегда 000х)-------------------
        private string ValidationAud(string aud)
        {
            string rus="абвгде";
            string eng="abvgde";
            int digits = 0,nulls=0,letters=0;
            if (aud.Length < 5)
            {
                for (int i = 0; i < aud.Length; i++)
                {
                    if (Char.IsDigit(aud[i]))
                    {
                        digits++;
                        if (aud[i] == '0')
                            nulls++;
                    }
                    else//letter
                    {
                        letters++;
                        for (int j = 0; j < 6; j++)
                        {
                            if (aud[i] == eng[j])
                                aud = aud.Replace(eng[j], rus[j]);
                        }
                    }
                }
                if (digits < 4)
                {
                    digits = 3 - digits;
                    while (digits != 0)
                    {
                        aud = aud.Insert(0, "0");
                        digits--;
                    }
                }
            }
            else//-------------пока не паримся
            {
                aud = "000x";
            }
            return aud;
        }

        //-----------------------Валидация инвентарника------------------------------
        private string ValidationInv(string invent)
        {
            try
            {
                if (invent.Length == 11 || invent.Length == 12)
                {
                    double num=Double.Parse(invent);
                    return invent;
                }
                else return "00x0000000E";
            }
            catch
            {
                return "00x0000000E";
            }
        }

        //------------------------------Сохранение в бд------------------------------
        private void SaveInDB(string[] splitTxt, string korp, string aud, string invent)
        {
            try
            {
                string query = @"insert into Comp (type,OC,SP_OC,Name,Domen,CPU,Mother,RAM,Video,HardDisk,MAC,ExplId,Invent) 
                                 values(@type,@OC,@SP_OC,@Name,@Domen,@CPU,@Mother,@RAM,@Video,@HardDisk,@MAC,@ExplId,@Invent)";
                using (SqlCommand cmd = new SqlCommand(query, _connect))
                {
                    string s = "", str = "";
                    string[] temp;
                    for (int i = 0; i < splitTxt.Length; i++)
                    {
                        temp = splitTxt[i].Split(' ');
                        if (temp.Length > 1)
                            s = temp[0] + " " + temp[1];
                        else
                            s = temp[0];
                        switch (s)
                        {
                            case "Компьютер:":
                                {
                                    cmd.Parameters.Add(new SqlParameter("@type", splitTxt[i + 1].Replace("Тип компьютера", "").TrimStart()));
                                    cmd.Parameters.Add(new SqlParameter("@OC", splitTxt[i + 2].Replace("Операционная система", "").TrimStart()));
                                    cmd.Parameters.Add(new SqlParameter("@SP_OC", splitTxt[i + 3].Replace("Пакет обновления ОС", "").TrimStart()));
                                    i = i + 3;
                                    break;
                                }
                            case "Имя компьютера":
                                {
                                    cmd.Parameters.Add(new SqlParameter("@Name", splitTxt[i].Replace("Имя компьютера", "").TrimStart()));
                                    break;
                                }
                            case "Вход в":
                                {
                                    cmd.Parameters.Add(new SqlParameter("@Domen", splitTxt[i].Replace("Вход в домен", "").TrimStart()));
                                    break;
                                }
                            case "Тип ЦП":
                                {
                                    cmd.Parameters.Add(new SqlParameter("@CPU", splitTxt[i].Replace("Тип ЦП", "").TrimStart()));
                                    break;
                                }
                            case "Системная плата":
                                {
                                    cmd.Parameters.Add(new SqlParameter("@Mother", splitTxt[i].Replace("Системная плата", "").TrimStart()));
                                    break;
                                }
                            case "Системная память":
                                {
                                    cmd.Parameters.Add(new SqlParameter("@RAM", splitTxt[i].Replace("Системная память", "").TrimStart()));
                                    break;
                                }
                            case "Отображение:":
                                {
                                    i++;
                                    cmd.Parameters.Add(new SqlParameter("@Video", splitTxt[i].Replace("Видеоадаптер", "").TrimStart()));
                                    break;
                                }
                            case "Дисковый накопитель":
                                {
                                    str += splitTxt[i].Replace("Дисковый накопитель", "") + " ";//-----
                                    break;
                                }
                            case "Сеть:":
                                {
                                    cmd.Parameters.Add(new SqlParameter("@MAC", splitTxt[i + 2].Replace("Первичный адрес MAC", "").TrimStart()));
                                    i = i + 2;
                                    break;
                                }
                        }
                    }
                    if (CheckComps(invent, cmd.Parameters["@Mac"].Value.ToString()))
                    {
                        cmd.Parameters.Add(new SqlParameter("@invent", invent));//--------------------может быть пустым
                        cmd.Parameters.Add(new SqlParameter("@ExplId", GetExplId(aud, korp)));
                        cmd.Parameters.Add(new SqlParameter("@HardDisk", str));
                        if (_connect.State != ConnectionState.Open)
                            _connect.Open();
                        int result = (int)cmd.ExecuteNonQuery();
                        if (result != 0)
                        {
                            InsertCount += result;
                            StatusItem.Caption = String.Format("Добавлено записей: {0}", InsertCount);
                        }
                        //_connect.Close();
                    }
                    else
                    {
                        StatusItem.Caption = String.Format("Добавлено записей: {0}", InsertCount);
                        return;
                    }
                }
            }
            catch (Exception msg)
            {
                XtraMessageBox.Show(msg.ToString());
            }
            finally
            {
                _connect.Close();
            }
        }

        //------------------------Проверка на наличие записи в базе------------------
        private bool CheckComps(string invent,string MAC)
        {
            string query = String.Format("select Id from Comp where Invent='{0}' and MAC='{1}'",invent,MAC);
            SqlCommand cmd = new SqlCommand(query, _connect);
            if (_connect.State != ConnectionState.Open)
                _connect.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                dr.Close();
                return false;
            }
            else  
            {
                dr.Close();
                return true;
            }
        }

        private void Main_Load(object sender, EventArgs e)
        {
            try
            {
                //--------
                gridView1.GroupPanelText = "";
                gridControl.Visible = false;
                FillDataSet("Korps",KorpsRepository);
                FillDataSet("Podrs", PodrsRepository);
            }
            catch (Exception msg)
            {
                XtraMessageBox.Show(msg.ToString());
            }
        }

        //-------------------------Получение id из экспликации----------------------
        private int GetExplId(string aud,string korp)
        {
            int result=1;
            string query = String.Format("select Id from Expl where Aud='{0}' and KorpId=(select Id from Korps where socr='{1}')",aud, korp);
            SqlCommand cmd = new SqlCommand(query, _connect);
            if(_connect.State!=ConnectionState.Open)
                _connect.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    result = dr.GetInt32(0);
                }                
            }
            else//добавление аудитории в справочник
            {
                dr.Close();
                string insertQuery = String.Format("insert into Expl (Aud,KorpId,PodrId) values ('{0}',(select Id from Korps where socr='{1}'),'0');SELECT SCOPE_IDENTITY();", aud, korp);
                using (SqlCommand InsCmd = new SqlCommand(insertQuery, _connect))
                {
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.InsertCommand = InsCmd;
                    result=Convert.ToInt32(da.InsertCommand.ExecuteScalar());
                    if (result!= 0)
                    {
                        auds += String.Format("{0} {1};", aud, korp);
                    }
                }
            }
            _connect.Close();
            return result;
        }

        //----------------------------Чтение всех файлов в папке--------------------
        private void iSave_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    InsertCount = 0;
                    auds = "";
                    string folderName = folderBrowserDialog1.SelectedPath; //папка с текстовыми файлами
                    string[] allFiles = Directory.GetFiles(folderName, "*.txt");
                    foreach (string s in allFiles)
                    {
                         using (StreamReader sr = new StreamReader(s, System.Text.Encoding.GetEncoding(1251)))
                         {
                             ReadFile(sr, s);
                         }
                    }
                   /* if (auds != "")
                        XtraMessageBox.Show(String.Format("В справочник добавлены аудитории: {0}", auds));*/
                    //else
                    if (InsertCount > 0)
                        XtraMessageBox.Show("Данные из файлов успешно добавлены в БД", "Статус", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else
                        XtraMessageBox.Show("Данные из файлов уже существуют в БД", "Статус", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception msg)
                {
                    XtraMessageBox.Show("Чтение файла невозможно: " + msg.Message);

                }
            }
        }

        private bool UpdateExpl(int korp,int podr,int id)
        {
            int f = 0;
            string query = "update Expl set KorpId=@KorpId, PodrId=@PodrId where Id=@Id";
            using (SqlCommand cmd = new SqlCommand(query,_connect))
            {
                cmd.Parameters.Add(new SqlParameter("@KorpId",korp));
                cmd.Parameters.Add(new SqlParameter("@PodrId",podr));
                cmd.Parameters.Add(new SqlParameter("@Id", id));
                SqlDataAdapter da=new SqlDataAdapter();
                _connect.Open();
                da.UpdateCommand=cmd;
                f=(int)da.UpdateCommand.ExecuteNonQuery();
                _connect.Close(); 
                if(f!=0)
                    return true;
                else 
                    return false;
            }          
        }

        private void GetInfoExpl()
        {
            int korp, podr, id, count=0;
            string query = "select * from Expl";
            using (SqlCommand cmd = new SqlCommand(query, _connect))
            {
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                if (_connect.State != ConnectionState.Open)
                    _connect.Open();
                ExplDt = new DataTable();
                da.Fill(ExplDt);
                _connect.Close();
            }
            for (int i = 0; i < ExplDt.Rows.Count; i++)
            {
                korp=GetKorpId(ExplDt.Rows[i].ItemArray[1].ToString());//---------------------
                podr =GetPodrId(ExplDt.Rows[i].ItemArray[3].ToString());
                id = Convert.ToInt32(ExplDt.Rows[i].ItemArray[0]);
                if(korp==0 || podr==0)
                    XtraMessageBox.Show(String.Format("В справочниках такого значения нет!{0},{1}",ExplDt.Rows[i].ItemArray[1].ToString(),ExplDt.Rows[i].ItemArray[3].ToString()));//---------------------- 
                if (UpdateExpl(korp, podr, id))
                    count++;
            }
            XtraMessageBox.Show(String.Format("Обновлено {0} строк!",count.ToString()));
        }

        //----------------------------начальное заполнение комбобоксов---------------------
        private void FillDataSet(string tableName, RepositoryItemLookUpEdit repository)
        {
            string query = String.Format("select * from {0}", tableName);
            using (SqlCommand cmd = new SqlCommand(query, _connect))
            {
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                if (_connect.State != ConnectionState.Open)
                    _connect.Open();
                if (ds.Tables[tableName] != null)
                    ds.Tables[tableName].Clear();
                da.Fill(ds,tableName);             
                _connect.Close();
                repository.DataSource = ds.Tables[tableName];
                repository.DisplayMember = "name";
                repository.ValueMember = "Id";  
            }
        }

        private void KorpusComboBox_EditValueChanged(object sender, EventArgs e)
        {
            flag1 = true;
            string KorpId = KorpusComboBox.EditValue.ToString();
            if (CheckFlag())//flag2=false
            {
                FillPodrsOrKorps(KorpId, "PodrId", "KorpId", "Podrs", PodrsRepository);
                FillAudCombo(KorpId, "");
                FillGridBy(String.Format("select Id from Expl where KorpId={0}", KorpId));
            }
            else//оба флага true 
            {
                string PodrId = PodrComboBox.EditValue.ToString();
                FillAudCombo(KorpId,String.Format("and PodrId={0}",PodrId));
                FillGridBy(String.Format("select Id from Expl where KorpId={0} and PodrId={1}",KorpId,PodrId));
                FillDataSet("Korps", KorpsRepository);
                FillDataSet("Podrs", PodrsRepository);
            }
        }

        private void PodrComboBox_EditValueChanged(object sender, EventArgs e)
        {
            flag2 = true;
            string PodrId = PodrComboBox.EditValue.ToString();
            if (CheckFlag())//flag1=false
            {
                FillPodrsOrKorps(PodrId, "KorpId", "PodrId", "Korps", KorpsRepository);
                FillGridBy(String.Format("select Id from Expl where PodrId={0}", PodrId));
            }
            else//оба флага true 
            {
                string KorpId = KorpusComboBox.EditValue.ToString();
                FillAudCombo(KorpId, String.Format("and PodrId={0}", PodrId));
                FillGridBy(String.Format("select Id from Expl where KorpId={0} and PodrId={1}",KorpId,PodrId));
                FillDataSet("Korps", KorpsRepository);
                FillDataSet("Podrs", PodrsRepository);
            }
        }

        //-----------------------заполнение комбобокса по ключу-----------------------------
        private void FillPodrsOrKorps(string korp,string selectKey,string whereKey,string tableName, RepositoryItemLookUpEdit repository)
        {
            string query = String.Format("select distinct {0} from Expl where {1}={2} order by {0} asc",selectKey,whereKey,korp);
            string queryForTemp = "";
            using (SqlCommand cmd = new SqlCommand(query, _connect))
            {
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                if (_connect.State != ConnectionState.Open)
                    _connect.Open();
                if (ds.Tables["Temp"] != null)
                    ds.Tables["Temp"].Reset();
                da.Fill(ds, "Temp");
                _connect.Close();
            }
            if (ds.Tables[tableName] != null)
                ds.Tables[tableName].Clear();
            for (int i = 0; i < ds.Tables["Temp"].Rows.Count; i++)
            {
                queryForTemp = String.Format("select id,name from {0} where Id={1}",tableName,ds.Tables["Temp"].Rows[i].ItemArray[0].ToString());
                using (SqlCommand cmd = new SqlCommand(queryForTemp, _connect))
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    if (_connect.State != ConnectionState.Open)
                        _connect.Open();
                    da.Fill(ds, tableName);
                    _connect.Close();
                }
            }
            DataView dataView = new DataView(ds.Tables[tableName]);
            dataView.Sort = "name asc";
            ds.Tables["Temp"].Clear();
            repository.DataSource = dataView;
            repository.DisplayMember = "name";
            repository.ValueMember = "Id";
        }

        //-----------------------------заполнение комбобокса аудиторий--------------------
        private void FillAudCombo(string korp,string wherePodr)//-----------------по корпусу
        {
            string query = String.Format("select distinct Id,Aud from Expl where KorpId={0} {1} order by Aud asc", korp, wherePodr);
            using (SqlCommand cmd = new SqlCommand(query, _connect))
            {
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                if (_connect.State != ConnectionState.Open)
                    _connect.Open();
                if (ds.Tables["Auds"] != null)
                    ds.Tables["Auds"].Clear();
                da.Fill(ds, "Auds");
                _connect.Close();
                AudsRepository.DataSource = ds.Tables["Auds"];
                AudsRepository.DisplayMember = "Aud";
                AudsRepository.ValueMember = "Id";
            }
        }

        //-----------------------------проверка выбора в комбо---------------------------
        private bool CheckFlag()
        {
            if (flag1 == true && flag2 == true)
            {
                flag1 = false;
                flag2 = false;
                return false;
            }
            else return true;
        }

        //-----------------------------заполнение грида----------------------------------
        private void FillGridBy(string ExplQuery)
        {
            try
            {
                string selectQuery;
                GetExplIds(ExplQuery);             
                if (ds.Tables["Comps"] != null)
                    ds.Tables["Comps"].Clear();
                gridControl.RefreshDataSource();
                for (int i = 0; i < ds.Tables["ExplIds"].Rows.Count; i++)
                {
                    selectQuery = String.Format("select * from Comp where ExplId={0}", ds.Tables["ExplIds"].Rows[i].ItemArray[0].ToString());
                    using (SqlCommand cmd = new SqlCommand(selectQuery, _connect))
                    {
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        if (_connect.State != ConnectionState.Open)
                            _connect.Open();
                        da.Fill(ds, "Comps");
                        _connect.Close();
                    }
                }             
                gridControl.DataSource = ds.Tables["Comps"];
                gridControl.Visible = true;
                gridView1.GroupPanelText = "Выберите компьютер";
            }
            catch (Exception msg)
            {
                XtraMessageBox.Show(msg.ToString());
            }
        }

        //-----------------------получение Id из Expl по корпусу и подразделению---------
        private void GetExplIds(string query)
        {
            //string query=String.Format("select Id from Expl where KorpId={0} and PodrId={1}",KorpId,PodrId);
            using (SqlCommand cmd = new SqlCommand(query, _connect))
            {
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                if (_connect.State != ConnectionState.Open)
                    _connect.Open();
                if (ds.Tables["ExplIds"] != null)
                    ds.Tables["ExplIds"].Reset();
                da.Fill(ds, "ExplIds");
                _connect.Close();
            }
        }     

        //-----------------------вывод всего---------------------------------------------
        private void UpdateGrid()
        {
            string query = "select * from Comp";
            using (SqlCommand cmd = new SqlCommand(query,_connect))
            {
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                if (ds.Tables["Comps"] != null)
                    ds.Tables.Clear();
                if (_connect.State != ConnectionState.Open)
                    _connect.Open();
                da.Fill(ds, "Comps");
                _connect.Close();
                gridControl.DataSource = ds.Tables["Comps"];
                gridControl.Visible = true;
                gridView1.GroupPanelText = "Выберите компьютер";
            }
        }

        //------------------------вывод всех данных--------------------------------------
        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            UpdateGrid();
        }

        //-------------------------------Поиск по инвентернику---------------------------
        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {        
            string query = String.Format("select * from Comp where invent='{0}'",findInventItem.EditValue.ToString());
            using (SqlCommand cmd = new SqlCommand(query, _connect))
            {
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                if (ds.Tables["Comps"] != null)
                    ds.Tables.Clear();
                if (_connect.State != ConnectionState.Open)
                    _connect.Open();
                da.Fill(ds, "Comps");
                _connect.Close();
                gridControl.RefreshDataSource();
                gridControl.DataSource = ds.Tables["Comps"];
                gridControl.Visible = true;
                gridView1.GroupPanelText = "Выберите компьютер";
            }
        }

        private void gridView1_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {

            int ExplId = 0;
            string Korp = "", Podr = "", Aud = "";
            //Korp = KorpsRepository.GetDisplayText(KorpusComboBox.EditValue);
            //Podr = PodrsRepository.GetDisplayText(PodrComboBox.EditValue);
            int[] rows = gridView1.GetSelectedRows();
            if (rows.Length != 0)
            {
                ExplId = (int)gridView1.GetRowCellValue(rows[0], "ExplId");
                string query = String.Format(@"select e.Aud as Aud,k.Name as Korp,p.name as Podr from Expl e 
                                           left outer join Korps k on k.id=e.KorpId
                                           left outer join Podrs p on p.id=e.PodrId     
                                           where e.Id='{0}'", ExplId);
                using (SqlCommand cmd = new SqlCommand(query, _connect))
                {
                    if (_connect.State != ConnectionState.Open)
                        _connect.Open();
                    SqlDataReader drd = cmd.ExecuteReader();
                    if (drd.HasRows)
                    {
                        while (drd.Read())
                        {
                            Aud = drd.GetString(0);
                            Korp = drd.GetString(1);
                            if (!drd.IsDBNull(2))
                            {
                                Podr = drd.GetString(2);
                            }
                        }
                        drd.Close();
                    }
                    _connect.Close();
                    gridView1.GroupPanelText = String.Format(@"Корпус «{0}» \«{1}» \«{2}»", Korp, Podr, Aud);
                }
            }
        }

        #region
        private void iSaveAs_ItemClick(object sender, ItemClickEventArgs e)
        {
            //GetInfoExpl();
        }

        private int GetKorpId(string name)
        {
            int result = 0;
            string query = String.Format("select id from Korps where name='{0}'",name);
            SqlCommand cmd = new SqlCommand(query, _connect);
            _connect.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    result = dr.GetInt32(0);
                }
            }
            _connect.Close();
            return result;
        }

        private int GetPodrId(string name)
        {
            int result = 0;
            string query = String.Format("select id from Podrs where Name='{0}'", name);
            SqlCommand cmd = new SqlCommand(query, _connect);
            _connect.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    result = dr.GetInt32(0);
                }
            }
            _connect.Close();
            return result;
        }
        #endregion//-------------------конвертация в id ------------------------------------             

       
    }
}