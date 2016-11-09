using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TimeStopper.Properties;

namespace TimeStopper
{
    public static class TimeSpanHelper
    {
        public static string GetFormattedDuration(this TimeSpan duration)
        {
            var earnedMoneyAmount = duration.GetEarnedMoneyAmount();

            return string.Format("{0}\n{1}", duration.FormatAsDate(), earnedMoneyAmount.FormatAsCurrency());
        }

        public static double GetEarnedMoneyAmount(this TimeSpan duration)
        {
            return duration.TotalHours * SetupSettings.CurrentSettings.HourlyWage;
        }

        private static string FormatAsDate(this TimeSpan duration)
        {
            return duration.ToString(@"h\h\ m\m\i\n\ s\s");
        }

        private static string FormatAsCurrency(this double value)
        {
            return value.ToString("0.00€");
        }
    }
}
