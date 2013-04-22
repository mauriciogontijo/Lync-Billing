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

        public static string GetEnumDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }

        public static IEnumerable<T> EnumToList<T>()
        {
            Type enumType = typeof(T);

            // Can't use generic type constraints on value types,
            // so have to do check like this
            if (enumType.BaseType != typeof(Enum))
                throw new ArgumentException("T must be of type System.Enum");

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