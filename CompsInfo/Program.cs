using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows.Forms;
using DevExpress.LookAndFeel;

namespace CompsInfo
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            DevExpress.Skins.SkinManager.EnableFormSkins();
            DevExpress.UserSkins.BonusSkins.Register();
            UserLookAndFeel.Default.SetSkinStyle("DevExpress Style");

            Application.Run(new Main());
        }
    }

    static class Data
    {
        private static string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=CompsBase;Integrated Security=SSPI;";
        private static SqlConnection _connect;
        public static SqlConnection Value { 
            get
            {
                if (_connect == null)
                {
                    _connect = new SqlConnection(connectionString);
                }
                return _connect;
            }
        }
    }
}