using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using System.ComponentModel;

namespace Lync_Billing.DB
{
    public static class Enums
    {
        public enum Sites
        {
            [Description("Sites")]
            TableName,
            [Description("SiteID")]
            SiteID,
            [Description("SiteName")]
            SiteName,
            [Description("SiteLocation")]
            SiteLocation,
            [Description("SiteUPN")]
            SiteUPN,
            [Description("CountryCode")]
            CountryCode
           
        }
        public enum Pools
        {
            [Description("Pools")]
            TableName,
            [Description("PoolID")]
            PoolID,
            [Description("PoolFQDN")]
            PoolFQDN
        }
        public enum Gateways
        {
            [Description("Gateways")]
            TableName,
            [Description("GatewayID")]
            GatewayID,
            [Description("GatewayFQDN")]
            GatewayFQDN,
            [Description("GatewayLocation")]
            GatewayLocation,
            [Description("CountryCode")]
            CountryCode,
            [Description("PoolFQDN")]
            PoolFQDN
        }
        public enum Rates
        {
            [Description("RateID")]
            RateID,
            [Description("GatewayName")]
            GatewayName,
            [Description("FixedLineRate")]
            FixedLineRate,
            [Description("MobileLineRate")]
            MobileLineRate
        }
        public enum GatewaysRates
        {
            [Description("GatewaysRates")]
            TableName,
            [Description("GatewaysRatesID")]
            RatesPerGatewaysID,
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
        public enum Users 
        {
            [Description("Users")]
            TableName,
            [Description("EmployeeID")]
            EmployeeID,
            [Description("LoginName")]
            LoginName,
            [Description("EmailAddress")]
            EmailAddress,
            [Description("PhoneNumber")]
            PhoneNumber,
            [Description("SiteName")]
            SiteName,
            [Description("PoolFQDN")]
            PoolID,
            [Description("UserUPN")]
            UserUPN,
            [Description("IsNormalUsers")]
            IsNormalUsers
          
        }
        public enum UsersRoles 
        {
            [Description("UsersRoles")]
            TableName,
            [Description("UsersRolesID")]
            UsersRoles,
            [Description("EmployeeID")]
            EmployeeID,
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
            UI_IsInvoiced
        }

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