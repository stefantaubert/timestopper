using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TimeStopper
{
    public class Record
    {
        public TimeSpan CurrentElapsed
        {
            get
            {
                return DateTime.Now - this.Start;
            }
        }

        public TimeSpan Elapsed
        {
            get
            {
                return this.End - this.Start;
            }
        }

        public DateTime End
        {
            get;
            set;
        }

        public DateTime Start
        {
            get;
            set;
        }

        public long Ticks
        {
            get
            {
                return this.Elapsed.Ticks;
            }
        }
    }
}