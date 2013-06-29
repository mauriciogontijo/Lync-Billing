﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Configuration;

namespace Lync_Billing.Libs
{
    public class Mailer
    {
        private SmtpClient client = new SmtpClient();
        private string mailhost = ConfigurationManager.AppSettings["MailHost"];
        
        public Mailer(string emailAddress, string templateSubject, string templateBody)
        {
            MailMessage mail = new MailMessage("ebill@ccc.gr", @emailAddress);
            client.Port = 25;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Host = mailhost;
            mail.Subject = templateSubject;
            mail.Body = templateBody;
            client.Send(mail);
        }
        
    }
}