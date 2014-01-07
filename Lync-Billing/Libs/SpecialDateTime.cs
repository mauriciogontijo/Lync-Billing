﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

using Lync_Billing.Backend;

namespace Lync_Billing.Libs
{
    public class SpecialDateTime
    {
        private DBLib DBRoutines = new DBLib();

        public int YearAsNumber { get; set; }
        public string YearAsText { get; set; }
        public int QuarterAsNumber { get; set; }
        public string QuarterAsText { get; set; }

        
        public static SpecialDateTime Get_OneYearAgoFromToday()
        {
            return new SpecialDateTime()
            { 
                YearAsText = Enums.GetDescription(Enums.SpecialDateTime.OneYearAgoFromToday),
                YearAsNumber = Convert.ToInt32(Enums.GetValue(Enums.SpecialDateTime.OneYearAgoFromToday)),
            };
        }


        public static SpecialDateTime Get_TwoYearsAgoFromToday()
        {
            return new SpecialDateTime()
            {
                YearAsText = Enums.GetDescription(Enums.SpecialDateTime.TwoYearsAgoFromToday),
                YearAsNumber = Convert.ToInt32(Enums.GetValue(Enums.SpecialDateTime.TwoYearsAgoFromToday)),
            };
        }


        public static List<SpecialDateTime> GetQuartersOfTheYear()
        {
            List<SpecialDateTime> quarters = new List<SpecialDateTime>()
            {
                //First Quarter
                new SpecialDateTime {
                    QuarterAsText = Enums.GetDescription(Enums.SpecialDateTime.FirstQuarter),
                    QuarterAsNumber = Convert.ToInt32(Enums.GetValue(Enums.SpecialDateTime.FirstQuarter))
                },
                //Second Quarter
                new SpecialDateTime {
                    QuarterAsText = Enums.GetDescription(Enums.SpecialDateTime.SecondQuarter),
                    QuarterAsNumber = Convert.ToInt32(Enums.GetValue(Enums.SpecialDateTime.SecondQuarter))
                },
                //Third Quarter
                new SpecialDateTime {
                    QuarterAsText = Enums.GetDescription(Enums.SpecialDateTime.ThirdQuarter),
                    QuarterAsNumber = Convert.ToInt32(Enums.GetValue(Enums.SpecialDateTime.ThirdQuarter))
                },
                //Fourth Quarter
                new SpecialDateTime {
                    QuarterAsText = Enums.GetDescription(Enums.SpecialDateTime.FourthQuarter),
                    QuarterAsNumber = Convert.ToInt32(Enums.GetValue(Enums.SpecialDateTime.FourthQuarter))
                },
                //All Quarters
                new SpecialDateTime {
                    QuarterAsText = Enums.GetDescription(Enums.SpecialDateTime.AllQuarters),
                    QuarterAsNumber = Convert.ToInt32(Enums.GetValue(Enums.SpecialDateTime.AllQuarters))
                }
            };

            return quarters;
        }


        public static string ConvertDate(DateTime datetTime, bool excludeHoursAndMinutes = false)
        {
            if (datetTime != DateTime.MinValue || datetTime != null)
            {
                if(excludeHoursAndMinutes == true)
                    return datetTime.ToString("yyyy-MM-dd");
                else
                    return datetTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
            }
            else
                return null;
        }

        public static string ConvertSecondsToReadable(int secondsParam)
        {
            int hours = Convert.ToInt32(Math.Floor((double)(secondsParam / 3600)));
            int minutes = Convert.ToInt32(Math.Floor((double)(secondsParam - (hours * 3600)) / 60));
            int seconds = secondsParam - (hours * 3600) - (minutes * 60);

            string hours_str = hours.ToString();
            string mins_str = minutes.ToString();
            string secs_str = seconds.ToString();

            if (hours < 10)
            {
                hours_str = "0" + hours_str;
            }

            if (minutes < 10)
            {
                mins_str = "0" + mins_str;
            }
            if (seconds < 10)
            {
                secs_str = "0" + secs_str;
            }

            return hours_str + ':' + mins_str + ':' + secs_str;
        }


        public static string ConstructDateRange(int filterYear, int filterQuater, out DateTime startingDate, out DateTime endingDate)
        {
            int quarterStartingMonth, quarterEndingMonth;
            string finalDateRangeTitle = string.Empty;

            SpecialDateTime Quarter;
            List<SpecialDateTime> AllQuarters = GetQuartersOfTheYear();

            SpecialDateTime OneYearAgoFromToday = Get_OneYearAgoFromToday();
            SpecialDateTime TwoYearsAgoFromToday = Get_TwoYearsAgoFromToday();
            

            //Begin
            //First, handle the year
            if (filterYear == OneYearAgoFromToday.YearAsNumber)
            {
                startingDate = new DateTime(DateTime.Now.Year - 1, DateTime.Now.Month, 1);
                endingDate = DateTime.Now;

                finalDateRangeTitle = OneYearAgoFromToday.YearAsText;
            }
            else if (filterYear == TwoYearsAgoFromToday.YearAsNumber)
            {
                startingDate = new DateTime(DateTime.Now.Year - 2, DateTime.Now.Month, 1);
                endingDate = DateTime.Now;

                finalDateRangeTitle = TwoYearsAgoFromToday.YearAsText;
            }
            else
            {
                //Handle the fromMonth and toMonth
                switch (filterQuater)
                {
                    case 1:
                        quarterStartingMonth = 1;
                        quarterEndingMonth = 3;
                        break;

                    case 2:
                        quarterStartingMonth = 4;
                        quarterEndingMonth = 6;
                        break;

                    case 3:
                        quarterStartingMonth = 7;
                        quarterEndingMonth = 9;
                        break;

                    case 4:
                        quarterStartingMonth = 10;
                        quarterEndingMonth = 12;
                        break;

                    case 5:
                        quarterStartingMonth = 1;
                        quarterEndingMonth = 12;
                        break;

                    default:
                        quarterStartingMonth = 1;
                        quarterEndingMonth = 12;
                        break;
                }

                Quarter = AllQuarters.Find(quarter => quarter.QuarterAsNumber == filterQuater) ?? AllQuarters.Find(quarter => quarter.QuarterAsNumber == 5);

                startingDate = new DateTime(Convert.ToInt32(filterYear), quarterStartingMonth, 1);
                endingDate = new DateTime(Convert.ToInt32(filterYear), quarterEndingMonth, 1);

                finalDateRangeTitle = String.Format("{0} ({1})", filterYear, Quarter.QuarterAsText);
            }

            return finalDateRangeTitle;
        }

    }

}