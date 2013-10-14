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
            [Description("phoneCallsTable")]
            PhoneCallsTable
        }

        public enum CallsImportStatus 
        {
            [Description("CallsImportStatus")]
            TableName,
            [Description("importId")]
            ImportID,
            [Description("importedTableName")]
            ImportedTableName,
            [Description("timestamp")]
            Timestamp
        }

        public enum CallMarkerStatus
        {
            [Description("CallMarkerStatus")]
            TableName,
            [Description("markerId")]
            MarkerId,
            [Description("phoneCallsTable")]
            PhoneCallsTable,
            [Description("timestamp")]
            Timestamp
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
            Duration
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
