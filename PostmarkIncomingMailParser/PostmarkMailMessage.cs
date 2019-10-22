using System;

namespace PostmarkIncomingMailParser
{
    public class PostmarkMailMessage : System.Net.Mail.MailMessage
    {
        public string MessageId { get; set; }
        public DateTime Date { get; set; }
    }
}
