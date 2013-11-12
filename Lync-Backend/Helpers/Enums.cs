using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.ComponentModel;

namespace Lync_Backend.Helpers
{
    public static class Enums
    {
        public enum MonitoringServersInfo
        {
            [Description("MonitoringServersInfo")]
            TableName,
            [Description("id")]
            Id,
            [Description("instanceHostName")]
            InstanceHostName,
            [Description("instanceName")]
            InstanceName,
            [Description("databaseName")]
            DatabaseName,
            [Description("userName")]
            Userame,
            [Description("password")]
            Password,
            [Description("TelephonySolutionName")]
            TelephonySolutionName,
            [Description("phoneCallsTable")]
            PhoneCallsTable,
            [Description("description")]
            Description,
            [Description("created_at")]
            CreatedAt
            
        }

        public enum PhoneCalls
        {
            [Description("PhoneCallsTableName")]
            PhoneCallsTableName,
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
            [Description("DestinationUserUri")]
            DestinationUserUri,
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
            [Description("marker_CallFrom")]
            Marker_CallFrom,
            [Description("marker_CallTo")]
            Marker_CallTo,
            [Description("marker_CallToCountry")]
            Marker_CallToCountry,
            [Description("marker_CallType")]
            Marker_CallType,
            [Description("marker_CallTypeID")]
            Marker_CallTypeID,
            [Description("marker_CallCost")]
            Marker_CallCost,
            [Description("ui_MarkedOn")]
            UI_MarkedOn,
            [Description("ui_UpdatedByUser")]
            UI_UpdatedByUser,
            [Description("ui_CallType")]
            UI_CallType,
            [Description("ac_DisputeStatus")]
            AC_DisputeStatus,
            [Description("ac_DisputeResolvedOn")]
            AC_DisputeResolvedOn,
            [Description("ac_IsInvoiced")]
            AC_IsInvoiced,
            [Description("ac_InvoiceDate")]
            AC_InvoiceDate,
            [Description("Exclude")]
            Exclude

        }

        public enum CallMarkerStatus
        {
            [Description("CallMarkerStatus")]
            TableName,
            [Description("markerId")]
            MarkerId,
            [Description("phoneCallsTable")]
            PhoneCallsTable,
            [Description("type")]
            Type,
            [Description("timestamp")]
            Timestamp
        }

        public enum Gateways
        {
            [Description("Gateways")]
            TableName,
            [Description("GatewayId")]
            GatewayId,
            [Description("Gateway")]
            GatewayName
        }

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

        public enum Pools
        {
            [Description("Pools")]
            TableName,
            [Description("PoolId")]
            PoolId,
            [Description("PoolFQDN")]
            PoolFQDN
        }

        public enum SitesGateways
        {
            [Description("GatewayId")]
            GatewayId,
            [Description("SiteID")]
            SiteID,
            [Description("Gateway")]
            GatewayName,
            [Description("SiteName")]
            SiteName,
            [Description("CountryCode")]
            CountryCode
        }

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

        public enum NumberingPlan
        {
            [Description("NumberingPlan")]
            TableName,
            [Description("Dialing_prefix")]
            DialingPrefix,
            [Description("Country_Name")]
            CountryName,
            [Description("Two_Digits_country_code")]
            TwoDigitsCountryCode,
            [Description("Three_Digits_Country_Code")]
            ThreeDigitsCountryCode,
            [Description("City")]
            City,
            [Description("Provider")]
            Provider,
            [Description("Type_Of_Service")]
            TypeOfService
        }

        public enum Rates
        {
            [Description("RateID")]
            RateID,
            [Description("Three_Digits_Country_Code")]
            CountryCode,
            [Description("Country_Name")]
            CountryName,
            [Description("Fixedline")]
            FixedlineRate,
            [Description("GSM")]
            MobileLineRate
        }

        public enum CallTypes
        {
            [Description("CallTypes")]
            TableName,
            [Description("id")]
            id,
            [Description("CallType")]
            CallType
        }

        public enum DIDs
        {
            [Description("DIDs")]
            TableName,
            [Description("id")]
            id,
            [Description("did")]
            did,
            [Description("description")]
            description
        }

        public enum PhoneCallsExceptions
        {
            [Description("PhoneCallsExceptions")]
            TableName,
            [Description("ID")]
            ID,
            [Description("UserUri")]
            UserUri,
            [Description("Number")]
            Number,
            [Description("Description")]
            Description
        }

        public enum PhoneCallSummary
        {
            [Description("UserPhoneCallsStats")]
            TableName,
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
            [Description("UnmarkedCallsDuration")]
            UnmarkedCallsDuration,
            [Description("UnmarkedCallsCost")]
            UnmarkedCallsCost,
            [Description("NumberOfDisputedCalls")]
            NumberOfDisputedCalls,
        }

        /// <summary>
        /// Users Database table Fields Names
        /// </summary>
        public enum Users
        {
            [Description("ActiveDirectoryUsers")]
            TableName,
            [Description("AD_UserID")]
            AD_UserID,
            [Description("SipAccount")]
            SipAccount,
            [Description("AD_DisplayName")]
            AD_DisplayName,
            [Description("AD_PhysicalDeliveryOfficeName")]
            AD_PhysicalDeliveryOfficeName,
            [Description("AD_Department")]
            AD_Department
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
