using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using TimeStopper.Properties;

namespace TimeStopper
{
    public class SetupSettings
    {
        private static SetupSettings currentSettings;

        public static SetupSettings CurrentSettings
        {
            get
            {
                if (currentSettings == null)
                {
                    if (SettingsExists)
                    {
                        currentSettings = XmlSerializers.DeserializeFromXmlFile<SetupSettings>(Settings.Default.SettingsFileName);
                    }

                    if (currentSettings == null)
                    {
                        currentSettings = new SetupSettings();
                    }
                }

                return currentSettings;
            }
        }

        public static bool SettingsExists
        {
            get
            {
                return File.Exists(Settings.Default.SettingsFileName);
            }
        }

        public void Save()
        {
            this.SerializeToXmlFile(Settings.Default.SettingsFileName);
        }

        public SetupSettings()
        {
            this.RecordsFileName = Settings.Default.DefaultRecordsFileName;
            this.RunningColorArgb = Settings.Default.DefaultRunningColor.ToArgb();
            this.StopColorArgb = Settings.Default.DefaultStoppingColor.ToArgb();
            this.HourlyWage = Settings.Default.DefaultHourlyWage;
        }

        public string RecordsFileName
        {
            get;
            set;
        }

        public double HourlyWage
        {
            get;
            set;
        }

        public Color StopColor
        {
            get
            {
                return Color.FromArgb(this.StopColorArgb);
            }
        }

        public Color RunningColor
        {
            get
            {
                return Color.FromArgb(this.RunningColorArgb);
            }
        }

        public int RunningColorArgb
        {
            get;
            set;
        }

        public int StopColorArgb
        {
            get;
            set;
        }
    }
}
