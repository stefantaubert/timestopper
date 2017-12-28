using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using Arbeitszeiten.Properties;
using System.Windows.Forms;

namespace Arbeitszeiten
{
    public class Recorder
    {
        string Speicherpfad = Path.Combine(Application.StartupPath, Settings.Default.Speichername);

        public List<Tag> Tage { get; set; }

        [XmlIgnore]
        public Tag AktuellerTag { get { return (Tage != null && Tage.Count > 0) ? Tage[Tage.Count - 1] : null; } }

        public Recorder()
        {
            Tage = new List<Tag>();
        }

        public Tag GetAlleTage()
        {
            TimeSpan ts = new TimeSpan();
            for (int i = 0; i < Tage.Count; i++)
            {
                ts = ts.Add(Tage[i].Ts);
            }
            return new Tag() { Ts = ts };
        }

        public void AddToday()
        {
            if (AktuellerTag == null || AktuellerTag.Day.ToShortDateString() != DateTime.Now.ToShortDateString())
            {
                Tage.Add(new Tag() { Day = DateTime.Now, Log = "" });
            }
        }

        public void Save()
        {
            Serialisieren<Recorder>(this, Speicherpfad);
        }

        public void Load()
        {
            Recorder rec = Deserialisieren<Recorder>(Speicherpfad);
            if (rec != null)
                this.Tage = rec.Tage;
        }

        private void Serialisieren<T>(T obj, string file)
        {
            using (FileStream fs = new FileStream(file, FileMode.Create))
                new XmlSerializer(typeof(T)).Serialize(fs, obj);
        }

        private T Deserialisieren<T>(string file)
        {
            //  return File.Exists(file) ? ((T)(new XmlSerializer(typeof(T))).Deserialize(new FileStream(file, FileMode.Open))) : default(T);
            if (!File.Exists(file)) return default(T);
            else using (FileStream fs = new FileStream(file, FileMode.Open))
                    return (T)(new XmlSerializer(typeof(T))).Deserialize(fs);
        }
    }
}
