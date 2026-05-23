using System;
using System.Windows.Forms;
using System.IO;

namespace TTG_Tools
{
    static class Program
    {
        public static bool FirstTime = true;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            string xmlPath = Application.StartupPath + "\\config.xml";

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (File.Exists(xmlPath))
            {
                FirstTime = false;
                AppData.LoadConfig();
            }
            else
            {
                // Create default config.xml with ASCII=1251, Normal Unicode
                var defaultSettings = new Settings();
                defaultSettings.ASCII_N = 1251;
                defaultSettings.unicodeSettings = 0;
                defaultSettings.languageIndex = -1;
                Settings.SaveConfig(defaultSettings);
                AppData.settings = defaultSettings;
                FirstTime = false;
            }

            Application.Run(new MainLauncher());
        }
    }
}
