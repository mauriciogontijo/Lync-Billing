using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using System.ComponentModel;

namespace Lync_Billing.DB
{
    /// <summary>
    /// A class Holds all Billing Database fields name
    /// </summary>
    public static class Enums
    {
        /// <summary>
        /// Sites Database table fields Names
        /// </summary>
        public enum Sites
        {
            [Description("Sites")]
            TableName,
            [Description("SiteID")]
            SiteID,
            [Description("SiteName")]
            SiteName,
            [Description("CountryCode")]
            CountryCode
           
        }

        /// <summary>
        /// Pools Database table fields Names
        /// </summary>
        public enum Pools
        {
            [Description("Pools")]
            TableName,
            [Description("PoolID")]
            PoolID,
            [Description("PoolFQDN")]
            PoolFQDN
        }

        /// <summary>
        /// GatewaysDetails Database table fields Names
        /// </summary>
        public enum GatewaysDetails
        {
            [Description("GatewaysDetails")]
            TableName,
            [Description("GatewayID")]
            GatewayID,
            [Description("SiteID")]
            SiteID,
            [Description("PoolID")]
            PoolID,
            [Description("Description")]
            Description
        }

        /// <summary>
        /// Rates Database table fields Names
        /// </summary>
        public enum Rates
        {
            [Description("RateID")]
            RateID,
            [Description("CountryCode")]
            CountryCode,
            [Description("FixedLineRate")]
            FixedLineRate,
            [Description("MobileLineRate")]
            MobileLineRate
        }

        /// <summary>
        /// GatewaysRates Database table fields Names
        /// </summary>
        public enum GatewaysRates
        {
            [Description("GatewaysRates")]
            TableName,
            [Description("GatewaysRatesID")]
            GatewaysRatesID,
            [Description("GatewayID")]
            GatewayID,
            [Description("RatesTableName")]
            RatesTableName,
            [Description("StartingDate")]
            StartingDate,
            [Description("EndingDate")]
            EndingDate,
            [Description("ProviderName")]
            ProviderName,
            [Description("CurrencyCode")]
            CurrencyCode
        }

        /// <summary>
        /// Users Database table Fields Names
        /// </summary>
        public enum Users 
        {
            [Description("Users")]
            TableName,
            [Description("UserID")]
            UserID,
            [Description("SipAccount")]
            SipAccount,
            [Description("SiteName")]
            SiteName
        }

        /// <summary>
        /// Roles Database table fields Names
        /// </summary>
        public enum Roles 
        {
            [Description("Roles")]
            TableName,
            [Description("RoleID")]
            RoleID,
            [Description("RoleName")]
            RoleName,
            [Description("RoleDescription")]
            RoleDescription
        }

        /// <summary>
        /// UsersRoles Database table fields Names
        /// </summary>
        public enum UsersRoles 
        {
            [Description("UsersRoles")]
            TableName,
            [Description("UsersRolesID")]
            UsersRolesID,
            [Description("SipAccount")]
            EmailAddress,
            [Description("RoleID")]
            RoleID,
            [Description("SiteID")]
            SiteID,
            [Description("PoolID")]
            PoolID,
            [Description("GatewayID")]
            GatewayID,
            [Description("Notes")]
            Notes
        }

        /// <summary>
        /// PhoneCalls Database table fields Names
        /// </summary>
        public enum PhoneCalls
        {
            [Description("PhoneCalls")]
            TableName,
            [Description("SessionIdTime")]
            SessionIdTime,
            [Description("SessionIdSeq")]
            SessionIdSeq,
            [Description("ResponseTime")]
            ResponseTime,
            [Description("SessionEndTime")]
            SessionEndTime,
            [Description("SourceUserUri")]
            SourceUserUri,
            [Description("SourceNumberUri")]
            SourceNumberUri,
            [Description("DestinationNumberUri")]
            DestinationNumberUri,
            [Description("FromMediationServer")]
            FromMediationServer,
            [Description("ToMediationServer")]
            ToMediationServer,
            [Description("FromGateway")]
            FromGateway,
            [Description("ToGateway")]
            ToGateway,
            [Description("SourceUserEdgeServer")]
            SourceUserEdgeServer,
            [Description("DestinationUserEdgeServer")]
            DestinationUserEdgeServer,
            [Description("ServerFQDN")]
            ServerFQDN,
            [Description("PoolFQDN")]
            PoolFQDN,
            [Description("Duration")]
            Duration,
            [Description("marker_CallToCountry")]
            Marker_CallToCountry,
            [Description("marker_CallType")]
            Marker_CallType,
            [Description("marker_CallCost")]
            Marker_CallCost,
            [Description("ui_MarkedOn")]
            UI_MarkedOn,
            [Description("ui_UpdatedByUser")]
            UI_UpdatedByUser,
            [Description("ui_IsPersonal")]
            UI_IsPersonal,
            [Description("ui_Dispute")]
            UI_Dispute,
            [Description("ui_IsInvoiced")]
            UI_IsInvoiced,
            [Description("ac_DisputeStatus")]
            ac_DisputeStatus
        }

        public enum UsersCallsSummary
        {
            [Description("BusinessCallsCount")]
            BusinessCallsCount,
            [Description("BusinessCallsCost")]
            BusinessCallsCost,
            [Description("BusinessCallsDuration")]
            BusinessCallsDuration,
            [Description("PersonalCallsCount")]
            PersonalCallsCount,
            [Description("PersonalCallsDuration")]
            PersonalCallsDuration,
            [Description("PersonalCallsCost")]
            PersonalCallsCost,
            [Description("UnmarkedCallsCount")]
            UnmarkedCallsCount,
            [Description("UnmarkedCallsDuartion")]
            UnmarkedCallsDuartion,
            [Description("UnmarkedCallsCost")]
            UnmarkedCallsCost,
            [Description("NumberOfDisputedCalls")]
            NumberOfDisputedCalls,
        }

        public enum Delegates 
        {
            [Description("Delegates")]
            TableName,
            [Description("ID")]
            ID,
            [Description("SipAccount")]
            SipAccount,
            [Description("DelegeeAccount")]
            DelegeeAccount,
            [Description("Description")]
            Description
        }

        public enum Announcements 
        {
            [Description("Announcements")]
            TableName,
            [Description("ID")]
            ID,
            [Description("Announcement")]
            Announcement,
            [Description("Role")]
            Role,
            [Description("AnnouncementDate")]
            AnnouncementDate
        }

        /// <summary>
        /// Gets the Name of DB table Field
        /// </summary>
        /// <param name="value">Enum Name</param>
        /// <returns>Field Description</returns>
        public static string GetDescription(Enum value)
        {
            FieldInfo fieldInfo = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] descAttributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (descAttributes != null && descAttributes.Length > 0)
                return descAttributes[0].Description;
            else
                return value.ToString();
        }
        
        public static IEnumerable<T> EnumToList<T>()
        {
            Type enumType = typeof(T);

            if (enumType.BaseType != typeof(Enum))
                throw new ArgumentException("T is not of System.Enum Type");

            Array enumValArray = Enum.GetValues(enumType);
            List<T> enumValList = new List<T>(enumValArray.Length);

            foreach (int val in enumValArray)
            {
                enumValList.Add((T)Enum.Parse(enumType, val.ToString()));
            }

            return enumValList;
        }

    }
}