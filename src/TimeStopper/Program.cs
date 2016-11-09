using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using System.Threading;
using TimeStopper.Properties;
using System.IO;

namespace TimeStopper
{
    static class Program
    {
        private static PersistentStopwatch stopwatch;

        private static Gui guiForStopwatch;

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            ShowSetupIfFirstStart();
            InitStopwatchFromFile();
            InitGui();
            StartGui();
        }

        private static void ShowSetupIfFirstStart()
        {
            if (!SetupSettings.SettingsExists)
            {
                Setup.ShowSetup();
            }
        }

        private static void StartGui()
        {
            guiForStopwatch.MakeVisibleToUser();
            Application.Run();
        }

        private static void InitGui()
        {
            guiForStopwatch = Gui.GetForStopwatch(stopwatch);
        }

        private static void InitStopwatchFromFile()
        {
            stopwatch = PersistentStopwatch.LoadFromFile();
        }
    }
}