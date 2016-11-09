using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using TimeStopper.Properties;

namespace TimeStopper
{
    public sealed class PersistentStopwatch
    {
        private Record runningRecord;

        private static string XmlFilePath
        {
            get
            {
                return string.Format("{0}\\{1}", Application.StartupPath, SetupSettings.CurrentSettings.RecordsFileName);
            }
        }

        private PersistentStopwatch()
            : base()
        {
            this.InitRecentRecords();
        }

        private void InitRecentRecords()
        {
            if (File.Exists(XmlFilePath))
            {
                this.RecentRecords = XmlSerializers.DeserializeFromXmlFile<Records>(XmlFilePath);
            }
            else
            {
                this.RecentRecords = new Records();
            }
        }

        public void ExportRecentRecords(string exportFileName)
        {
            var summary = this.RecentRecords.GetSummary();

            File.WriteAllText(exportFileName, summary);

            MessageBox.Show("Export was successfull!", "Success");
        }

        public long CurrentRecordElapsedTicks
        {
            get
            {
                return this.IsRunning ? this.runningRecord.CurrentElapsed.Ticks : 0;
            }
        }

        public TimeSpan Elapsed
        {
            get
            {
                return TimeSpan.FromTicks(this.ElapsedTicks);
            }
        }

        public long ElapsedTicks
        {
            get
            {
                return this.RecentRecords.ElapsedTicks + this.CurrentRecordElapsedTicks;
            }
        }

        public bool IsRunning
        {
            get
            {
                return this.runningRecord != null;
            }
        }

        public Records RecentRecords
        {
            get;
            set;
        }

        public static PersistentStopwatch LoadFromFile()
        {
            return new PersistentStopwatch();
        }

        public void Reset()
        {
            this.ClearRunningRecord();

            this.RecentRecords.Clear();
            this.RecentRecords.SerializeToXmlFile(XmlFilePath);
        }

        private void ClearRunningRecord()
        {
            this.runningRecord = null;
        }

        public void StartNewRecord()
        {
            this.InitNewRunningRecord();
        }

        private void InitNewRunningRecord()
        {
            this.runningRecord = new Record();
            this.runningRecord.Start = DateTime.Now;
        }

        public void StopRunningRecord()
        {
            this.EndRunningRecordAndAddToRecentRecords();

            this.RecentRecords.SerializeToXmlFile(XmlFilePath);

            this.ClearRunningRecord();
        }

        private void EndRunningRecordAndAddToRecentRecords()
        {
            this.runningRecord.End = DateTime.Now;
            this.RecentRecords.Add(this.runningRecord);
        }
    }
}