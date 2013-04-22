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
            [Description("gateway_id")]
            GatewayID,
            [Description("gateway_name")]
            GatewayName
        }

        public static enum Pools 
        {
            [Description("pool_id")]
            PoolID,
            [Description("pool_name")]
            PoolName
        }

        public static enum Users 
        {
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

        public static string GetEnumDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }

    }
}