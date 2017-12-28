using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Arbeitszeiten
{
    public class Tag
    {
        const double Gehalt = 9.45;
        private DateTime tmpTime = DateTime.Now;
        public DateTime Day { get; set; }

        [XmlIgnore]
        public string Dauer { get { return GetFormat(Ts); } }
        
        [XmlIgnore]
        public TimeSpan Ts { get; set; }

        [XmlElement("Dauer")]
        public string XmlDuration
        {
            get { return Ts.ToString(); }
            set { Ts = TimeSpan.Parse(value); }
        }

        public string Log { get; set; }

        [XmlIgnore]
        public double Geldwert { get { return Math.Round(Ts.TotalHours * Gehalt, 2, MidpointRounding.AwayFromZero); } }

        public void An()
        {
            tmpTime = DateTime.Now;
            Log += "\nStart um " + tmpTime.ToShortTimeString();
        }

        public void Aus()
        {
            TimeSpan tsTmp = DateTime.Now - tmpTime;
            Ts = Ts.Add(tsTmp);
            Log += "\nEnde um " + DateTime.Now.ToShortTimeString();
            Log += "\n=> " + GetFormat(tsTmp);
        }

        string GetFormat(TimeSpan ts)
        {
            return ts.Hours + "h " + ts.Minutes + "min " + ts.Seconds + "s";
        }
    }
}
