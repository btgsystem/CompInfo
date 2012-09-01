using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace CITreport
{
    public partial class Form1 : Form
    {
        string infostring;
        string corp;
        string strcorp;
        string saveto;

        public Form1()
        {
            InitializeComponent();
        }

        private void generateSaveto()
        {
            corp = corpusBox.Text;
            switch (corp)
            {
                case "Главного учебно-административного корпуса":
                    strcorp = "guk";
                    break;
                case "Факультет зоотехнологии и менеджмента":
                    strcorp = "zoo";
                    break;
                case "Факультета защиты растений":
                    strcorp = "fzr";
                    break;
                case "Факультет ветеринарной медицины":
                    strcorp = "vet";
                    break;
                case "Экономического факультета":
                    strcorp = "eco";
                    break;
                case "Факультета энергетики и эликтрификации":
                    strcorp = "elf";
                    break;
                case "Факультет механизации сельского хозяйства":
                    strcorp = "meh";
                    break;
                case "Факультета водохозяйственного строительства и мелиорации":
                    strcorp = "gid";
                    break;
                case "Факультет заочного обучения":
                    strcorp = "zao4";
                    break;
                case "Учебно–лабораторный корпус":
                    strcorp = "ulk";
                    break;
                case "НИИ «Биотехнологии и сертификации пищевой продукции»":
                    strcorp = "nii";
                    break;
                default:
                    strcorp = "000";
                    break;
            }
            saveto = string.Format("\\out\\{0}-{1}-{2}.txt",strcorp,kabinet.Text,inventory.Text);
        }

        private void closeApp()
        {
            Application.Exit();
        }

        private void startAida()
        {
            Process aidabin = new Process();
            ProcessStartInfo info = new ProcessStartInfo();
            info.FileName = "aida64.exe";
            infostring = string.Format("/R {0} /TEXT /LANGRU /CUSTOM format.rpf /SAFE", saveto);
            info.Arguments = infostring;
            info.UseShellExecute = false;
            aidabin.StartInfo = info;
            button1.Enabled = false;
            button2.Enabled = false;
            aidabin.Start();
            aidabin.WaitForExit();
            closeApp();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            generateSaveto();
            startAida();
        }

        private void button2_Click(object sender, EventArgs e)
        {
           closeApp();
        }

    }
}
