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
            [Description("SiteName")]
            SiteName,
            [Description("CountryName")]
            CountryName,
            [Description("SiteLocation")]
            SiteLocation,
            [Description("Currency")]
            Currency,
            [Description("Domain")]
            Domain
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
            [Description("GatewayName")]
            GatewayName,
            [Description("GatewayLocation")]
            GatewayCountry,
            [Description("GatewayLocation")]
            GatewayLocation,
            [Description("GatewayUPN")]
            GatewayUPN,
            [Description("PoolFQDN")]
            PoolFQDN,
            [Description("RatesTableID")]
            RatesTableID
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
            [Description("PoolID")]
            PoolID,
            [Description("GatewayID")]
            GatewayID,
            [Description("IsNormalUsers")]
            IsNormalUsers,
            [Description("UPN")]
            UPN
        }
        public enum RolesPerUsers 
        {
            [Description("RolesPerUsers")]
            TableName,
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
        public enum RatesPerGateways
        {
            [Description("RatesPerGateways")]
            RatesPerGateways,
            [Description("RateID")]
            RateID,
            [Description("GatewayID")]
            GatewayID,
            [Description("StartingDate")]
            StartingDate,
            [Description("EndingDate")]
            EndingDate,
            [Description("Currency")]
            Currency
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