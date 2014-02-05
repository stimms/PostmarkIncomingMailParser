using System;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Collections.Generic;
using Microsoft.CSharp.RuntimeBinder;

namespace PostmarkIncomingMailParser
{
    public class PostmarkMailMessage : System.Net.Mail.MailMessage
    {
        public String MessageId { get; set; }
        public DateTime Date { get; set; }
    }
}
