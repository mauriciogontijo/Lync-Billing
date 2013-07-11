﻿ using System;
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
        /// Gateways datadbase table fields names
        /// </summary>
        public enum Gateways 
        {
            [Description("Gateways")]
            TableName,
            [Description("GatewayId")]
            GatewayId,
            [Description("Gateway")]
            GatewayName
        }

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
            [Description("Three_Digits_Country_Code")]
            CountryCode,
            [Description("Country_Name")]
            CountryName,
            [Description("Fixedline")]
            FixedlineRate,
            [Description("GSM")]
            MobileLineRate
        }

        public enum ActualRates
        {
            [Description("GatewaysDetails")]
            TableName,
            [Description("Rate_ID")]
            RateID,
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
            TypeOfService,
            [Description("rate")]
            Rate, 
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

        public enum TmpUsers 
        {
            [Description("TmpUsers")]
            TableName,
            [Description("ID")]
            ID,
            [Description("SipAccount")]
            SipAccount
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
            [Description("ui_CallType")]
            UI_CallType,
            [Description("ac_DisputeStatus")]
            AC_DisputeStatus,
            [Description("ac_DisputeResolvedOn")]
            AC_DisputeResolvedOn,
            [Description("ac_IsInvoiced")]
            AC_IsInvoiced,
            [Description("ac_InvoiceDate")]
            AC_InvoiceDate
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

        public enum PhoneBook
        {
            [Description("PhoneBook")]
            TableName,
            [Description("ID")]
            ID,
            [Description("SipAccount")]
            SipAccount,
            [Description("DestinationNumber")]
            DestinationNumber,
            [Description("DestinationCountry")]
            DestinationCountry,
            [Description("Type")]
            Type,
            [Description("Name")]
            Name
        }

        public enum Currencies
        {
            [Description("Currencies")]
            TableName,
            [Description("CountryName")]
            CountryName,
            [Description("CurrencyName")]
            CurrencyName,
            [Description("CurrencyISOName")]
            CurrencyISOName
        }

        public enum MailTemplates
        {
            [Description("MailTemplates")]
            TableName,
            [Description("TemplateID")]
            TemplateID,
            [Description("TemplateSubject")]
            TemplateSubject,
            [Description("TemplateBody")]
            TemplateBody
        }

        public enum Persistence 
        {
            [Description("Persistence")]
            TableName,
            [Description("ID")]
            ID,
            [Description("Module")]
            Module,
            [Description("Module_Key")]
            ModuleKey,
            [Description("Module_Value")]
            ModuleValue
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