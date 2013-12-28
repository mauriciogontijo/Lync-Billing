using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Lync_Billing.Libs;
using Lync_Billing.ConfigurationSections;
using System.Configuration;

namespace Lync_Billing.Backend.Summaries
{
    public class UserCallsSummary
    {
        private static DBLib DBRoutines = new DBLib();

        public string EmployeeID { get; set; }
        public string FullName { get; set; }
        public string SipAccount { get; set; }
        public string SiteName { get; set; }

        public int BusinessCallsCount { get; set; }
        public decimal BusinessCallsCost { get; set; }
        public int BusinessCallsDuration { get; set; }
        public int PersonalCallsCount { get; set; }
        public int PersonalCallsDuration { get; set; }
        public decimal PersonalCallsCost { get; set; }
        public int UnmarkedCallsCount { get; set; }
        public int UnmarkedCallsDuration { get; set; }
        public decimal UnmarkedCallsCost { get; set; }
        public int NumberOfDisputedCalls { get; set; }

        public DateTime Date { set; get; }
        public decimal Duration { get; set; }

        //This is a total of the costs of phonecalls, used in designing cartesian charts
        public decimal TotalCallsCosts { get; set; }
        public decimal TotalCallsDurations { get; set; }
        public decimal TotalCallsCount { get; set; }

        public int Year { get; set; }
        public int Month { get; set; }


        /// <summary>
        /// Given a SipAccount address, a year's number, a starting month's number, and an ending month's number return sum object of all the phone calls summaries between the months.
        /// </summary>
        /// <param name="siteName">The user's SipAccount address</param>
        /// <param name="year">The year of the phone calls summary</param>
        /// <param name="fromMonth">The starting month of the phone calls summary</param>
        /// <param name="endMonth">The ending month of the phone calls summary</param>
        /// <returns>A UserCallsSummary sum object of all summaries between the months</returns>
        public static UserCallsSummary GetUserCallsSummary(string sipAccount, int Year, int fromMonth, int toMonth)
        {
            DataTable dt = new DataTable();
            string databaseFunction = Enums.GetDescription(Enums.DatabaseFunctionsNames.Get_CallsSummary_ForUser);

            List<object> functionParams = new List<object>();
            Dictionary<string, object> wherePart = new Dictionary<string, object>();

            UserCallsSummary userSummary = new UserCallsSummary();

            //Initialize the function parameters and query the database
            wherePart.Add(Enums.GetDescription(Enums.PhoneCallSummary.Year), Year);
            wherePart.Add(Enums.GetDescription(Enums.PhoneCallSummary.Month), String.Format("BETWEEN '{0}' AND '{1}'", fromMonth, toMonth));
            functionParams.Add(sipAccount);

            dt = DBRoutines.SELECT_FROM_FUNCTION(databaseFunction, functionParams, wherePart);


            foreach (DataRow row in dt.Rows)
            {
                //Start processing the summary
                userSummary = new UserCallsSummary();

                userSummary.Year = Convert.ToInt32(HelperFunctions.ReturnZeroIfNull(row[dt.Columns[Enums.GetDescription(Enums.PhoneCallSummary.Year)]]));
                userSummary.Month = Convert.ToInt32(HelperFunctions.ReturnZeroIfNull(row[dt.Columns[Enums.GetDescription(Enums.PhoneCallSummary.Month)]]));
                userSummary.Date = new DateTime(userSummary.Year, userSummary.Month, DateTime.DaysInMonth(userSummary.Year, userSummary.Month));

                userSummary.BusinessCallsDuration += Convert.ToInt32(HelperFunctions.ReturnZeroIfNull(row[dt.Columns[Enums.GetDescription(Enums.PhoneCallSummary.BusinessCallsDuration)]]));
                userSummary.BusinessCallsCount += Convert.ToInt32(HelperFunctions.ReturnZeroIfNull(row[dt.Columns[Enums.GetDescription(Enums.PhoneCallSummary.BusinessCallsCount)]]));
                userSummary.BusinessCallsCost += Convert.ToDecimal(HelperFunctions.ReturnZeroIfNull(row[dt.Columns[Enums.GetDescription(Enums.PhoneCallSummary.BusinessCallsCost)]]));
                userSummary.PersonalCallsDuration += Convert.ToInt32(HelperFunctions.ReturnZeroIfNull(row[dt.Columns[Enums.GetDescription(Enums.PhoneCallSummary.PersonalCallsDuration)]]));
                userSummary.PersonalCallsCount += Convert.ToInt32(HelperFunctions.ReturnZeroIfNull(row[dt.Columns[Enums.GetDescription(Enums.PhoneCallSummary.PersonalCallsCount)]]));
                userSummary.PersonalCallsCost += Convert.ToDecimal(HelperFunctions.ReturnZeroIfNull(row[dt.Columns[Enums.GetDescription(Enums.PhoneCallSummary.PersonalCallsCost)]]));
                userSummary.UnmarkedCallsDuration += Convert.ToInt32(HelperFunctions.ReturnZeroIfNull(row[dt.Columns[Enums.GetDescription(Enums.PhoneCallSummary.UnmarkedCallsDuration)]]));
                userSummary.UnmarkedCallsCount += Convert.ToInt32(HelperFunctions.ReturnZeroIfNull(row[dt.Columns[Enums.GetDescription(Enums.PhoneCallSummary.UnmarkedCallsCount)]]));
                userSummary.UnmarkedCallsCost += Convert.ToDecimal(HelperFunctions.ReturnZeroIfNull(row[dt.Columns[Enums.GetDescription(Enums.PhoneCallSummary.UnmarkedCallsCost)]]));

                userSummary.Duration += (userSummary.PersonalCallsDuration / 60);
            }

            userSummary.TotalCallsCosts = userSummary.BusinessCallsCost + userSummary.PersonalCallsCost + userSummary.UnmarkedCallsCost;
            userSummary.TotalCallsDurations = userSummary.BusinessCallsDuration + userSummary.PersonalCallsDuration + userSummary.UnmarkedCallsDuration;
            userSummary.TotalCallsCount = userSummary.BusinessCallsCount + userSummary.PersonalCallsCount + userSummary.UnmarkedCallsCount;

            //Get the remaining user information
            Users userInfo = Users.GetUser(sipAccount);

            userSummary.EmployeeID = userInfo.EmployeeID.ToString();
            userSummary.SipAccount = userInfo.SipAccount;
            userSummary.FullName = userInfo.FullName;
            userSummary.SiteName = userInfo.SiteName;

            return userSummary;
        }


        /// <summary>
        /// Given a SipAccount address, a starting date, and an ending date return a UserCallsSummary sum object of all the phone calls summaries between the dates.
        /// </summary>
        /// <param name="siteName">The user's SipAccount address</param>
        /// <param name="year">The year of the phone calls summary</param>
        /// <param name="startingDate">The starting date of phone calls summary</param>
        /// <param name="endingDate">The ending date of the phone calls summary</param>
        /// <returns>A UserCallsSummary sum object of all summaries between the dates</returns>
        public static UserCallsSummary GetUserCallsSummary(string sipAccount, DateTime startDate, DateTime endDate)
        {
            DataTable dt = new DataTable();
            string databaseFunction = Enums.GetDescription(Enums.DatabaseFunctionsNames.Get_CallsSummary_ForUser);

            List<object> functionParams = new List<object>();
            Dictionary<string, object> wherePart = new Dictionary<string, object>();
            
            UserCallsSummary userSummary = new UserCallsSummary();

            //Initialize the function parameters and query the database
            wherePart.Add(Enums.GetDescription(Enums.PhoneCallSummary.Year), String.Format("BETWEEN '{0}' AND '{1}'", startDate.Year, endDate.Year));
            wherePart.Add(Enums.GetDescription(Enums.PhoneCallSummary.Month), String.Format("BETWEEN '{0}' AND '{1}'", startDate.Month, endDate.Month));
            functionParams.Add(sipAccount);

            dt = DBRoutines.SELECT_FROM_FUNCTION(databaseFunction, functionParams, wherePart);

            foreach (DataRow row in dt.Rows)
            {
                //summaryYear = Convert.ToInt32(HelperFunctions.ReturnZeroIfNull(row[dt.Columns[Enums.GetDescription(Enums.PhoneCallSummary.Year)]]));
                //summaryMonth = Convert.ToInt32(HelperFunctions.ReturnZeroIfNull(row[dt.Columns[Enums.GetDescription(Enums.PhoneCallSummary.Month)]]));

                ////Skip this summary-row if it's out of the range of the given date periods
                //if ((summaryYear < startDate.Year && summaryMonth < startDate.Month) ||
                //    (summaryYear > endDate.Year && summaryMonth > endDate.Month))
                //    continue;

                ////Start processing the summary
                //userSummary = new UserCallsSummary();

                //userSummary.Year = summaryYear;
                //userSummary.Month = summaryMonth;
                //userSummary.Date = new DateTime(summaryYear, summaryMonth, DateTime.DaysInMonth(summaryYear, summaryMonth));

                //Start processing the summary
                userSummary = new UserCallsSummary();

                userSummary.Year = Convert.ToInt32(HelperFunctions.ReturnZeroIfNull(row[dt.Columns[Enums.GetDescription(Enums.PhoneCallSummary.Year)]]));
                userSummary.Month = Convert.ToInt32(HelperFunctions.ReturnZeroIfNull(row[dt.Columns[Enums.GetDescription(Enums.PhoneCallSummary.Month)]]));
                userSummary.Date = new DateTime(userSummary.Year, userSummary.Month, DateTime.DaysInMonth(userSummary.Year, userSummary.Month));

                userSummary.BusinessCallsDuration += Convert.ToInt32(HelperFunctions.ReturnZeroIfNull(row[dt.Columns[Enums.GetDescription(Enums.PhoneCallSummary.BusinessCallsDuration)]]));
                userSummary.BusinessCallsCount += Convert.ToInt32(HelperFunctions.ReturnZeroIfNull(row[dt.Columns[Enums.GetDescription(Enums.PhoneCallSummary.BusinessCallsCount)]]));
                userSummary.BusinessCallsCost += Convert.ToDecimal(HelperFunctions.ReturnZeroIfNull(row[dt.Columns[Enums.GetDescription(Enums.PhoneCallSummary.BusinessCallsCost)]]));
                userSummary.PersonalCallsDuration += Convert.ToInt32(HelperFunctions.ReturnZeroIfNull(row[dt.Columns[Enums.GetDescription(Enums.PhoneCallSummary.PersonalCallsDuration)]]));
                userSummary.PersonalCallsCount += Convert.ToInt32(HelperFunctions.ReturnZeroIfNull(row[dt.Columns[Enums.GetDescription(Enums.PhoneCallSummary.PersonalCallsCount)]]));
                userSummary.PersonalCallsCost += Convert.ToDecimal(HelperFunctions.ReturnZeroIfNull(row[dt.Columns[Enums.GetDescription(Enums.PhoneCallSummary.PersonalCallsCost)]]));
                userSummary.UnmarkedCallsDuration += Convert.ToInt32(HelperFunctions.ReturnZeroIfNull(row[dt.Columns[Enums.GetDescription(Enums.PhoneCallSummary.UnmarkedCallsDuration)]]));
                userSummary.UnmarkedCallsCount += Convert.ToInt32(HelperFunctions.ReturnZeroIfNull(row[dt.Columns[Enums.GetDescription(Enums.PhoneCallSummary.UnmarkedCallsCount)]]));
                userSummary.UnmarkedCallsCost += Convert.ToDecimal(HelperFunctions.ReturnZeroIfNull(row[dt.Columns[Enums.GetDescription(Enums.PhoneCallSummary.UnmarkedCallsCost)]]));

                userSummary.Duration += (userSummary.PersonalCallsDuration / 60);
            }

            userSummary.TotalCallsCosts = userSummary.BusinessCallsCost + userSummary.PersonalCallsCost + userSummary.UnmarkedCallsCost;
            userSummary.TotalCallsDurations = userSummary.BusinessCallsDuration + userSummary.PersonalCallsDuration + userSummary.UnmarkedCallsDuration;
            userSummary.TotalCallsCount = userSummary.BusinessCallsCount + userSummary.PersonalCallsCount + userSummary.UnmarkedCallsCount;

            //Get the remaining user information
            Users userInfo = Users.GetUser(sipAccount);

            userSummary.EmployeeID = userInfo.EmployeeID.ToString();
            userSummary.SipAccount = userInfo.SipAccount;
            userSummary.FullName = userInfo.FullName;
            userSummary.SiteName = userInfo.SiteName;

            return userSummary;
        }


        /// <summary>
        /// Given a SipAccount address, a year's number, a starting month's number, and an ending month's number return the list of this user's summaries with respect to months.
        /// </summary>
        /// <param name="siteName">The user's SipAccount address</param>
        /// <param name="Year">The year of the phone calls summary</param>
        /// <param name="fromMonth">The starting month of the phone calls summary</param>
        /// <param name="endMonth">The ending month of the phone calls summary</param>
        /// <returns>A list of UserCallsSummary for that user</returns>
        public static List<UserCallsSummary> GetUsersCallsSummary(string sipAccount, int Year, int fromMonth, int toMonth)
        {
            DataTable dt = new DataTable();
            string databaseFunction = Enums.GetDescription(Enums.DatabaseFunctionsNames.Get_CallsSummary_ForUser);

            List<object> functionParams = new List<object>();
            Dictionary<string, object> wherePart = new Dictionary<string, object>();

            UserCallsSummary userSummary;
            List<UserCallsSummary> chartList = new List<UserCallsSummary>();

            //Get this user's information
            Users userInfo = Users.GetUser(sipAccount);

            //Initialize the function parameters and query the database
            wherePart.Add(Enums.GetDescription(Enums.PhoneCallSummary.Year), Year);
            wherePart.Add(Enums.GetDescription(Enums.PhoneCallSummary.Month), String.Format("BETWEEN '{0}' AND '{1}'", fromMonth, toMonth));
            functionParams.Add(sipAccount);

            dt = DBRoutines.SELECT_FROM_FUNCTION(databaseFunction, functionParams, wherePart);


            foreach (DataRow row in dt.Rows)
            {
                //Start processing the summary
                userSummary = new UserCallsSummary();

                userSummary.Year = Convert.ToInt32(HelperFunctions.ReturnZeroIfNull(row[dt.Columns[Enums.GetDescription(Enums.PhoneCallSummary.Year)]]));
                userSummary.Month = Convert.ToInt32(HelperFunctions.ReturnZeroIfNull(row[dt.Columns[Enums.GetDescription(Enums.PhoneCallSummary.Month)]]));
                userSummary.Date = new DateTime(userSummary.Year, userSummary.Month, DateTime.DaysInMonth(userSummary.Year, userSummary.Month));

                userSummary.EmployeeID = userInfo.EmployeeID.ToString();
                userSummary.SipAccount = userInfo.SipAccount;
                userSummary.FullName = userInfo.FullName;
                userSummary.SiteName = userInfo.SiteName;

                userSummary.BusinessCallsDuration = Convert.ToInt32(HelperFunctions.ReturnZeroIfNull(row[dt.Columns[Enums.GetDescription(Enums.PhoneCallSummary.BusinessCallsDuration)]]));
                userSummary.BusinessCallsCount = Convert.ToInt32(HelperFunctions.ReturnZeroIfNull(row[dt.Columns[Enums.GetDescription(Enums.PhoneCallSummary.BusinessCallsCount)]]));
                userSummary.BusinessCallsCost = Convert.ToDecimal(HelperFunctions.ReturnZeroIfNull(row[dt.Columns[Enums.GetDescription(Enums.PhoneCallSummary.BusinessCallsCost)]]));
                userSummary.PersonalCallsDuration = Convert.ToInt32(HelperFunctions.ReturnZeroIfNull(row[dt.Columns[Enums.GetDescription(Enums.PhoneCallSummary.PersonalCallsDuration)]]));
                userSummary.PersonalCallsCount = Convert.ToInt32(HelperFunctions.ReturnZeroIfNull(row[dt.Columns[Enums.GetDescription(Enums.PhoneCallSummary.PersonalCallsCount)]]));
                userSummary.PersonalCallsCost = Convert.ToDecimal(HelperFunctions.ReturnZeroIfNull(row[dt.Columns[Enums.GetDescription(Enums.PhoneCallSummary.PersonalCallsCost)]]));
                userSummary.UnmarkedCallsDuration = Convert.ToInt32(HelperFunctions.ReturnZeroIfNull(row[dt.Columns[Enums.GetDescription(Enums.PhoneCallSummary.UnmarkedCallsDuration)]]));
                userSummary.UnmarkedCallsCount = Convert.ToInt32(HelperFunctions.ReturnZeroIfNull(row[dt.Columns[Enums.GetDescription(Enums.PhoneCallSummary.UnmarkedCallsCount)]]));
                userSummary.UnmarkedCallsCost = Convert.ToDecimal(HelperFunctions.ReturnZeroIfNull(row[dt.Columns[Enums.GetDescription(Enums.PhoneCallSummary.UnmarkedCallsCost)]]));

                userSummary.Duration = (userSummary.PersonalCallsDuration / 60);

                userSummary.TotalCallsCosts = userSummary.BusinessCallsCost + userSummary.PersonalCallsCost + userSummary.UnmarkedCallsCost;
                userSummary.TotalCallsDurations = userSummary.BusinessCallsDuration + userSummary.PersonalCallsDuration + userSummary.UnmarkedCallsDuration;
                userSummary.TotalCallsCount = userSummary.BusinessCallsCount + userSummary.PersonalCallsCount + userSummary.UnmarkedCallsCount;

                //Add it to the list.
                chartList.Add(userSummary);
            }

            return chartList;
        }


        /// <summary>
        /// Given a Site's name, a starting date, and an end date, return the list of all users' summaries who belong to that Site
        /// </summary>
        /// <param name="siteName">The Site's name</param>
        /// <param name="startingDate">The starting date of phone calls summary</param>
        /// <param name="endingDate">The ending date of the phone calls summary</param>
        /// <param name="asTotals">If this is specified as true the function will return a list of calls summaries (totals) for each employee in that site</param>
        /// <returns>A list of UserCallsSummaries between the dates specified</returns>
        public static List<UserCallsSummary> GetUsersCallsSummaryInSite(string siteName, DateTime startingDate, DateTime endingDate, bool asTotals = false)
        {
            DataTable dt = new DataTable();
            string databaseFunction = Enums.GetDescription(Enums.DatabaseFunctionsNames.Get_CallsSummary_ForUsers_PerSite);

            Users userInfo;
            UserCallsSummary userSummary;
            List<UserCallsSummary> usersSummaryList = new List<UserCallsSummary>();

            List<string> sharedSitesPerGateway = new List<string>();

            List<object> functionParams = new List<object>();
            Dictionary<string, object> wherePart = new Dictionary<string, object>();


            //Add the siteName to the functionParams
            wherePart.Add(Enums.GetDescription(Enums.PhoneCallSummary.Year), String.Format("BETWEEN '{0}' AND '{1}'", startingDate.Year, endingDate.Year));
            wherePart.Add(Enums.GetDescription(Enums.PhoneCallSummary.Month), String.Format("BETWEEN '{0}' AND '{1}'", startingDate.Month, endingDate.Month));
            functionParams.Add(siteName);

            dt = DBRoutines.SELECT_FROM_FUNCTION(databaseFunction, functionParams, wherePart);

            usersSummaryList = (from row in dt.AsEnumerable()
                                select new UserCallsSummary()
                                {
                                    EmployeeID = Convert.ToString(HelperFunctions.ReturnEmptyIfNull(row[Enums.GetDescription(Enums.PhoneCallSummary.EmployeeID)])),
                                    SipAccount = Convert.ToString(HelperFunctions.ReturnEmptyIfNull(row[Enums.GetDescription(Enums.PhoneCallSummary.ChargingParty)])),
                                    FullName = Convert.ToString(HelperFunctions.ReturnEmptyIfNull(row[Enums.GetDescription(Enums.PhoneCallSummary.DisplayName)])),
                                    SiteName = Users.GetUserSite(Convert.ToString(HelperFunctions.ReturnEmptyIfNull(row[Enums.GetDescription(Enums.PhoneCallSummary.ChargingParty)]))),
                                    
                                    BusinessCallsDuration = Convert.ToInt32(HelperFunctions.ReturnZeroIfNull(row[Enums.GetDescription(Enums.PhoneCallSummary.BusinessCallsDuration)])),
                                    BusinessCallsCount = Convert.ToInt32(HelperFunctions.ReturnZeroIfNull(row[Enums.GetDescription(Enums.PhoneCallSummary.BusinessCallsCount)])),
                                    BusinessCallsCost = Convert.ToDecimal(HelperFunctions.ReturnZeroIfNull(row[Enums.GetDescription(Enums.PhoneCallSummary.BusinessCallsCost)])),
                                    
                                    PersonalCallsDuration = Convert.ToInt32(HelperFunctions.ReturnZeroIfNull(row[Enums.GetDescription(Enums.PhoneCallSummary.PersonalCallsDuration)])),
                                    PersonalCallsCount = Convert.ToInt32(HelperFunctions.ReturnZeroIfNull(row[Enums.GetDescription(Enums.PhoneCallSummary.PersonalCallsCount)])),
                                    PersonalCallsCost = Convert.ToDecimal(HelperFunctions.ReturnZeroIfNull(row[Enums.GetDescription(Enums.PhoneCallSummary.PersonalCallsCost)])),
                                    
                                    UnmarkedCallsDuration = Convert.ToInt32(HelperFunctions.ReturnZeroIfNull(row[Enums.GetDescription(Enums.PhoneCallSummary.UnmarkedCallsDuration)])),
                                    UnmarkedCallsCount = Convert.ToInt32(HelperFunctions.ReturnZeroIfNull(row[Enums.GetDescription(Enums.PhoneCallSummary.UnmarkedCallsCount)])),
                                    UnmarkedCallsCost = Convert.ToDecimal(HelperFunctions.ReturnZeroIfNull(row[Enums.GetDescription(Enums.PhoneCallSummary.UnmarkedCallsCost)])),

                                    Duration = (Convert.ToInt32(HelperFunctions.ReturnZeroIfNull(row[Enums.GetDescription(Enums.PhoneCallSummary.PersonalCallsDuration)])) / 60),

                                    TotalCallsCosts = Convert.ToDecimal(HelperFunctions.ReturnZeroIfNull(row[Enums.GetDescription(Enums.PhoneCallSummary.BusinessCallsCost)]))
                                                    + Convert.ToDecimal(HelperFunctions.ReturnZeroIfNull(row[Enums.GetDescription(Enums.PhoneCallSummary.PersonalCallsCost)]))
                                                    + Convert.ToDecimal(HelperFunctions.ReturnZeroIfNull(row[Enums.GetDescription(Enums.PhoneCallSummary.UnmarkedCallsCost)])),

                                    TotalCallsDurations = Convert.ToInt32(HelperFunctions.ReturnZeroIfNull(row[Enums.GetDescription(Enums.PhoneCallSummary.BusinessCallsDuration)]))
                                                    + Convert.ToInt32(HelperFunctions.ReturnZeroIfNull(row[Enums.GetDescription(Enums.PhoneCallSummary.PersonalCallsDuration)]))
                                                    + Convert.ToInt32(HelperFunctions.ReturnZeroIfNull(row[Enums.GetDescription(Enums.PhoneCallSummary.UnmarkedCallsDuration)])),

                                    TotalCallsCount = Convert.ToInt32(HelperFunctions.ReturnZeroIfNull(row[Enums.GetDescription(Enums.PhoneCallSummary.BusinessCallsCount)]))
                                                    + Convert.ToInt32(HelperFunctions.ReturnZeroIfNull(row[Enums.GetDescription(Enums.PhoneCallSummary.PersonalCallsCount)]))
                                                    + Convert.ToInt32(HelperFunctions.ReturnZeroIfNull(row[Enums.GetDescription(Enums.PhoneCallSummary.UnmarkedCallsCount)]))
                                })
                                .Where(summary => summary.SiteName == siteName)
                                .ToList<UserCallsSummary>();

            if (asTotals == true)
            {
                usersSummaryList = (
                    from summary in usersSummaryList.AsEnumerable<UserCallsSummary>()
                    group summary by new { summary.SipAccount, summary.EmployeeID, summary.FullName, summary.SiteName } into result
                    select new UserCallsSummary
                    {
                        EmployeeID = result.Key.EmployeeID,
                        FullName = result.Key.FullName,
                        SipAccount = result.Key.SipAccount,
                        SiteName = result.Key.SiteName,

                        BusinessCallsCost = result.Sum(x => x.BusinessCallsCost),
                        BusinessCallsDuration = result.Sum(x => x.BusinessCallsDuration),
                        BusinessCallsCount = result.Sum(x => x.BusinessCallsCount),

                        PersonalCallsCost = result.Sum(x => x.PersonalCallsCost),
                        PersonalCallsDuration = result.Sum(x => x.PersonalCallsDuration),
                        PersonalCallsCount = result.Sum(x => x.PersonalCallsCount),

                        UnmarkedCallsCost = result.Sum(x => x.UnmarkedCallsCost),
                        UnmarkedCallsDuration = result.Sum(x => x.UnmarkedCallsDuration),
                        UnmarkedCallsCount = result.Sum(x => x.UnmarkedCallsCount),
                    }
                )
                .ToList<UserCallsSummary>();
            }

            return usersSummaryList;
        }


        /// <summary>
        /// Given a Site's name, a list of specific users sipaccounts addresses, a starting date, and an end date, return the list of all of these users summaries - who belong to that same Site.
        /// </summary>
        /// <param name="siteName">The Site's name</param>
        /// <param name="sipAccountsList">The list of users' SipAccount addresses</param>
        /// <param name="startingDate">The starting date of phone calls summary</param>
        /// <param name="endingDate">The ending date of the phone calls summary</param>
        /// <returns>A list of UserCallsSummary</returns>
        public static Dictionary<string, UserCallsSummary> GetUsersCallsSummaryInSite(string siteName, List<string> sipAccountsList, DateTime startingDate, DateTime endingDate)
        {
            DataTable dt = new DataTable();
            string columnName = string.Empty;

            List<UserCallsSummary> ListOfUsersSummaries = GetUsersCallsSummaryInSite(siteName, startingDate, endingDate, asTotals: true);

            Dictionary<string, UserCallsSummary> usersSummaryList = ListOfUsersSummaries
                .Where(summary => sipAccountsList.Contains(summary.SipAccount))
                .ToDictionary(summary => summary.SipAccount);

            return usersSummaryList;
        }


        /// <summary>
        /// Exports the user phonecalls summries for a specific site to a PDF document
        /// </summary>
        /// <param name="siteName">The site's name</param>
        /// <param name="startingDate">Starting date of the report</param>
        /// <param name="endingDate">Ending date of the report</param
        /// <param name="UsersCollection">A dictionary of each user's sipaccount with his/her user information</param>
        /// <param name="response">The response stream on which to write the file</param>
        /// <param name="document">The source pdf document object</param>
        /// <param name="headers">The pdf document headers</param>
        public static void ExportUsersCallsSummaryToPDF(string siteName, DateTime startingDate, DateTime endingDate, Dictionary<string, Dictionary<string, object>> UsersCollection, HttpResponse response, out Document document, Dictionary<string, string> headers)
        {
            //THE PDF REPORT PROPERTIES
            PDFReportsPropertiesSection section = ((PDFReportsPropertiesSection)ConfigurationManager.GetSection(PDFReportsPropertiesSection.ConfigurationSectionName));
            PDFReportPropertiesElement pdfReportProperties = section.GetReportProperties("AccountingSummary");

            //Database related
            DataTable dt = new DataTable();
            string databaseFunction = Enums.GetDescription(Enums.DatabaseFunctionsNames.Get_CallsSummary_ForUsers_PerSite_PDF);

            List<object> functionParams = new List<object>();
            List<string> columnsList = new List<string>();
            Dictionary<string, object> wherePart = new Dictionary<string, object>();

            //These two are passed to the PdfLib
            int[] pdfColumnsWidths = new int[] { };
            List<string> pdfColumnsSchema = new List<string>();
            Dictionary<string, object> callsCostsTotals = new Dictionary<string, object>();

            if (pdfReportProperties != null)
            {
                pdfColumnsWidths = pdfReportProperties.ColumnsWidths();
                pdfColumnsSchema = pdfReportProperties.ColumnsNames();
            }

            headers.Add("comments", "* Please note that the columns headers below - Business, Personal and Unallocated - refer to the Costs of the phonecalls based on their type.");


            //Get the report content from the database with the following function parameters, where statement and group by fields.
            functionParams.Add(siteName);
            functionParams.Add(startingDate);
            functionParams.Add(endingDate);

            wherePart.Add(Enums.GetDescription(Enums.PhoneCallSummary.ChargingParty), UsersCollection.Keys.ToList<string>());

            dt = DBRoutines.SELECT_FROM_FUNCTION(databaseFunction, functionParams, wherePart);


            //Try to compute totals, if an error occurs which is the case of an empty "dt", set the totals dictionary to zeros
            try
            {
                callsCostsTotals.Add("PersonalCallsCost", Decimal.Round(Convert.ToDecimal(HelperFunctions.ReturnZeroIfNull(dt.Compute("Sum(PersonalCallsCost)", "PersonalCallsCost > 0"))), 2));
                callsCostsTotals.Add("BusinessCallsCost", Decimal.Round(Convert.ToDecimal(HelperFunctions.ReturnZeroIfNull(dt.Compute("Sum(BusinessCallsCost)", "BusinessCallsCost > 0"))), 2));
                callsCostsTotals.Add("UnmarkedCallsCost", Decimal.Round(Convert.ToDecimal(HelperFunctions.ReturnZeroIfNull(dt.Compute("Sum(UnmarkedCallsCost)", "UnmarkedCallsCost > 0"))), 2));
            }
            catch (Exception e)
            {
                callsCostsTotals.Add("PersonalCost", 0.00);
                callsCostsTotals.Add("BusinessCost", 0.00);
                callsCostsTotals.Add("UnmarkedCost", 0.00);
            }

            document = PDFLib.CreateAccountingSummaryReport(
                ResponseStream: response,
                SourceDataTable: dt,
                CallsCostsTotals: callsCostsTotals,
                PDFColumnsSchema: pdfColumnsSchema,
                PDFColumnsWidths: pdfColumnsWidths,
                PDFDocumentHeaders: headers
            );
        }


        /// <summary>
        /// Export the phonecalls of all employees who have called from or through a site
        /// </summary>
        /// <param name="siteName">The site's name</param>
        /// <param name="startingDate">Starting date if the report</param>
        /// <param name="endingDate">Ending date of the report</param>
        /// <param name="UsersCollection">The user information collection</param>
        /// <param name="response">The response stream on which to write the document</param>
        /// <param name="document">The source pdf document object</param>
        /// <param name="headers">The pdf document header texts</param>
        public static void ExportUsersCallsDetailedToPDF(string siteName, DateTime startingDate, DateTime endingDate, Dictionary<string, Dictionary<string, object>> UsersCollection, HttpResponse response, out Document document, Dictionary<string, string> headers)
        {
            //THE PDF REPORT PROPERTIES
            PDFReportsPropertiesSection section = ((PDFReportsPropertiesSection)ConfigurationManager.GetSection(PDFReportsPropertiesSection.ConfigurationSectionName));
            PDFReportPropertiesElement pdfReportProperties = section.GetReportProperties("AccountingDetailed");

            //Database query related
            DataTable dt;
            string databaseFunction = Enums.GetDescription(Enums.DatabaseFunctionsNames.Get_ChargeableCalls_ForSite);

            List<object> functionParams = new List<object>();
            List<string> columnsList = new List<string>();
            Dictionary<string, object> wherePart = new Dictionary<string,object>();

            //This is passed to the PdfLib
            List<string> pdfColumnsSchema = new List<string>();
            int[] pdfColumnsWidths = new int[] { };

            if (pdfReportProperties != null)
            {
                pdfColumnsWidths = pdfReportProperties.ColumnsWidths();
                pdfColumnsSchema = pdfReportProperties.ColumnsNames();
            }


            //Database query related
            functionParams.Add(siteName);
            
            columnsList.Add(Enums.GetDescription(Enums.PhoneCalls.ChargingParty));
            columnsList.Add(Enums.GetDescription(Enums.PhoneCalls.ResponseTime));
            columnsList.Add(Enums.GetDescription(Enums.PhoneCalls.Marker_CallToCountry));
            columnsList.Add(Enums.GetDescription(Enums.PhoneCalls.DestinationNumberUri));
            columnsList.Add(Enums.GetDescription(Enums.PhoneCalls.Duration));
            columnsList.Add(Enums.GetDescription(Enums.PhoneCalls.Marker_CallCost));
            columnsList.Add(Enums.GetDescription(Enums.PhoneCalls.UI_CallType));

            wherePart.Add(Enums.GetDescription(Enums.PhoneCallSummary.ChargingParty), UsersCollection.Keys.ToList<string>());
            wherePart.Add(Enums.GetDescription(Enums.PhoneCalls.ResponseTime), String.Format("BETWEEN '{0}' AND '{1}'", startingDate, endingDate));

            //The PDF report body contents
            dt = DBRoutines.SELECT_FROM_FUNCTION(databaseFunction, functionParams, wherePart, selectColumnsList: columnsList);

            //Get the collection of users' summaries.
            Dictionary<string, UserCallsSummary> UsersSummaires = UserCallsSummary.GetUsersCallsSummaryInSite(siteName, UsersCollection.Keys.ToList(), startingDate, endingDate);

            //Get a closed instance of the document which contains all the formatted data
            document = PDFLib.CreateAccountingDetailedReport(
                ResponseStream: response,
                SourceDataTable: dt, 
                PDFColumnsWidths: pdfColumnsWidths, 
                PDFColumnsSchema: pdfColumnsSchema, 
                PDFDocumentHeaders: headers, 
                DataSeparatorName: Enums.GetDescription(Enums.PhoneCalls.ChargingParty), 
                UsersInfoCollections: UsersCollection, 
                UsersSummariesMap: UsersSummaires
            );
        }

    }

}