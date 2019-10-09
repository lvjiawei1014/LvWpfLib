using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LvWpfLib
{
    class Utils
    {
        public static string GetTimeLabel(DateTime dateTime,TimeSpanLevel timeSpanLevel)
        {
            switch (timeSpanLevel)
            {
                case TimeSpanLevel.Minute:
                    if (dateTime.Minute == 0)
                    {
                        return dateTime.Hour + "时";
                    }
                    else
                    {
                        return dateTime.Minute + "分";
                    }
                case TimeSpanLevel.Hour:
                    if (dateTime.Hour == 0)
                    {
                        return dateTime.Day + "日";
                    }
                    return dateTime.Hour + "时";
                case TimeSpanLevel.day:

                    if (dateTime.Day == 0)
                    {
                        return dateTime.Month + "月";
                    }
                    return dateTime.Day + "日";
                case TimeSpanLevel.Month:
                    if (dateTime.Month == 0)
                    {
                        return dateTime.Year + "年";
                    }
                    return dateTime.Month + "月";
                case TimeSpanLevel.Year:
                    return dateTime.Year + "年";
                default:
                    break;
            }
            return "";
        }

        public static DateTime DateTimeRound(DateTime dateTime,TimeSpanLevel timeSpanLevel)
        {
            switch (timeSpanLevel)
            {
                case TimeSpanLevel.Minute:
                    return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, 0, 0);
                case TimeSpanLevel.Hour:
                    return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0);
                case TimeSpanLevel.day:
                    return new DateTime(dateTime.Year, dateTime.Month, 1, 0, 0, 0);
                case TimeSpanLevel.Month:
                    return new DateTime(dateTime.Year, 1, 1, 0, 0, 0);
                case TimeSpanLevel.Year:
                    int y = (dateTime.Year / 10) * 10;
                    return new DateTime(y,0,0,0, 0, 0);
                default:
                    break;
            }
            return dateTime;
        }

    }
}
