using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;

namespace TimeStopper
{

    public sealed class Records : List<Record>
    {
        Dictionary<DateTime, TimeSpan> elapsedDays;

        private Records(IEnumerable<Record> records)
            : base(records)
        {
            this.elapsedDays = new Dictionary<DateTime, TimeSpan>();
        }

        public Records()
            : base()
        {
            this.elapsedDays = new Dictionary<DateTime, TimeSpan>();
        }

        private static Records FromIEnumerable(IEnumerable<Record> records)
        {
            return new Records(records);
        }

        public long ElapsedTicks
        {
            get
            {
                return this.Sum(s => s.Ticks);
            }
        }

        private static DateTime RoundToDay(DateTime tim)
        {
            return new DateTime(tim.Year, tim.Month, tim.Day);
        }

        public string GetSummary()
        {
            this.elapsedDays.Clear();
            foreach (var item in this)
            {
                var das = RoundToDay(item.Start);
                if (!this.elapsedDays.ContainsKey(das))
                {
                    this.elapsedDays.Add(das, new TimeSpan());
                }

                this.elapsedDays[das] = this.elapsedDays[das].Add(item.Elapsed);
            }


            var res = string.Empty;
            foreach (var item in this.elapsedDays)
            {
                var roundedMins = Math.Round(item.Value.TotalMinutes, 0);
                res += string.Format("{0}: {1} minute(s)\n", item.Key.ToShortDateString(), roundedMins.ToString());
            }

            return res.Trim();
        }
        
        public Records GetTodaysRecords()
        {
            var todaysDayShort = DateTime.Now.ToShortDateString();
            var todaysRecordItems = this.Where(s => s.Start.ToShortDateString() == todaysDayShort);

            return Records.FromIEnumerable(todaysRecordItems);
        }
    }
}