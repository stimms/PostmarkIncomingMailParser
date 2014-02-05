using System;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Collections.Generic;
using Microsoft.CSharp.RuntimeBinder;

namespace PostmarkIncomingMailParser
{
    public class Parser
    {
        public PostmarkMailMessage Parse(string toParse)
        {
            var message = new PostmarkMailMessage();
            var parsedJson = System.Web.Helpers.Json.Decode(toParse);

            PraseTo(message, parsedJson);
            ParseFrom(message, parsedJson);
            ParseCC(message, parsedJson);
            ParseSubject(message, parsedJson);
            ParseIsBodyHTML(message, parsedJson);
            ParseHeaders(message, parsedJson);
            ParseBody(message, parsedJson);
            ParseId(message, parsedJson);
            ParseDate(message, parsedJson);
            ParseAttachments(message, parsedJson);

            return message;
        }
        private static void ParseFrom(PostmarkMailMessage message, dynamic parsedJson)
        {
            message.From = new MailAddress(parsedJson.FromFull.Email, parsedJson.FromFull.Name);
        }
        private static void PraseTo(PostmarkMailMessage message, dynamic parsedJson)
        {
            foreach (dynamic to in parsedJson.ToFull)
            {
                message.To.Add(new MailAddress(to.Email, to.Name));
            }
        }

        private static void ParseCC(PostmarkMailMessage message, dynamic parsedJson)
        {
            foreach (dynamic cc in parsedJson.CCFull)
            {
                message.CC.Add(new MailAddress(cc.Email, cc.Name));
            }
        }

        private static void ParseSubject(PostmarkMailMessage message, dynamic parsedJson)
        {
            message.Subject = parsedJson.Subject;
        }

        private static void ParseHeaders(PostmarkMailMessage message, dynamic parsedJson)
        {
            foreach (dynamic header in parsedJson.Headers)
            {
                message.Headers.Add(header.Name, header.Value);
            }
        }
        private static void ParseBody(PostmarkMailMessage message, dynamic parsedJson)
        {
            if (message.IsBodyHtml)
                message.Body = parsedJson.HtmlBody;
            else
                message.Body = parsedJson.TextBody;
        }
        private static void ParseId(PostmarkMailMessage message, dynamic parsedJson)
        {
            message.MessageId = parsedJson.MessageID;
        }
        private void ParseAttachments(PostmarkMailMessage message, dynamic parsedJson)
        {
            foreach(var attachment in parsedJson.Attachments)
                message.Attachments.Add(new Attachment(new MemoryStream(Convert.FromBase64String(attachment.Content)), attachment.Name, attachment.ContentType));
        }
        private static void ParseDate(PostmarkMailMessage message, dynamic parsedJson)
        {
            message.Date = DateTime.Parse(parsedJson.Date);
        }
        private void ParseIsBodyHTML(PostmarkMailMessage message, dynamic parsedJson)
        {
            try
            {
                if (!String.IsNullOrWhiteSpace(parsedJson.HtmlBody))
                    message.IsBodyHtml = true;
                else
                    message.IsBodyHtml = false;
            }
            catch (RuntimeBinderException)
            {
                message.IsBodyHtml = false;
            }
        }
     
    }
}