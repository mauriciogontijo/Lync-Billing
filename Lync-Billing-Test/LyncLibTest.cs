using System;
using System.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lync_Billing.Libs;
using Lync_Billing.DB;
using System.Xml;

namespace Lync_Billing_Test
{
    [TestClass]
    public class LyncLibTest
    {
        [TestMethod]
        public void  DB_SELECT()
        {
            DBLib dbRoutines = new DBLib();
            
            string tableName = string.Empty;
            string whereField = string.Empty;
            string whereValue = string.Empty;


            DataTable dt;

            tableName = Enums.GetDescription(Enums.PhoneCalls.TableName);
            whereField = Enums.GetDescription(Enums.PhoneCalls.SourceUserUri);
            whereValue = @"SGhaida@ccc.gr";

            dt = dbRoutines.SELECT(tableName,whereField,whereValue);

            Assert.IsNotNull(dt);
        }

        [TestMethod]
        public void Authenticate() 
        {
            string username = string.Empty;
            string password = string.Empty;

            username = @"sghaida@ccc.gr";
            password = @"=25_ar;p1";

            AdLib adRoutines = new AdLib();

            bool status = adRoutines.AuthenticateUser(username, password);
            Assert.IsFalse(status);
            
        }
    }
}
