using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using System.ComponentModel;

namespace Lync_Billing.Libs
{
    public class Enums
    {
        public static enum Gateways
        {
            [Description("gateways")]
            TableName,
            [Description("gateway_id")]
            GatewayID,
            [Description("gateway_name")]
            GatewayName
        }

        public static enum Pools 
        {
            [Description("pools")]
            TableName,
            [Description("pool_id")]
            PoolID,
            [Description("pool_name")]
            PoolName
        }

        public static enum Users 
        {
            [Description("users")]
            TableName,
            [Description("employee_id")]
            employeeID,
            [Description("username")]
            userName,
            [Description("pool_id")]
            PoolID,
            [Description("gateway_id")]
            GatewayID,
            [Description("role_id")]
            RoleID
        }

        public static enum PhoneCalls 
        {
            [Description("phone_calls")]
            TableName,
            [Description("phone_call_id")]
            PhoneCallID,
            [Description("date_of_call")]
            CalledOn,
            [Description("sip_account")]
            SipAccount,
            [Description("src_number")]
            SrcNumber,
            [Description("dst_number")]
            DstNumber,
            [Description("dst_country")]
            DstCountry,
            [Description("type_of_service")]
            TypeOfService,
            [Description("gateway")]
            Gateway,
            [Description("updated_by")]
            UpdatedBy,
            [Description("modified_by")]
            ModifiedBy,
            [Description("duration")]
            Duration,
            [Description("rate")]
            rate,
            [Description("cost")]
            Cost,
            [Description("updated_on")]
            UpdateOn,
            [Description("modified_on")]
            ModifiedOn,
            [Description("is_personal")]
            IsPersonal,
            [Description("dispute")]
            Dispute,
            [Description("payed")]
            Payed,
            [Description("bill_it")]
            BillIt
     }

        public static enum Rates
        {
            [Description("fixed_line_rate")]
            FixedLineRate,
            [Description("mobile_line_rate")]
            MobileLineRate,
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