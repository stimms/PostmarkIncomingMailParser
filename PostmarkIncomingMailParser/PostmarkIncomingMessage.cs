using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;

namespace PostmarkIncomingMailParser
{
    public class PostmarkIncomingMessage
    {
        public string FromName { get; set; }
        public string MessageStream { get; set; }
        //public string From { get; set; }
        public EmailFull FromFull { get; set; }
        //public string To { get; set; }
        public IList<EmailFull> ToFull { get; set; }
        //public string Cc { get; set; }
        public IList<EmailFull> CcFull { get; set; }
        //public string Bcc { get; set; }
        public IList<EmailFull> BccFull { get; set; }
        public string OriginalRecipient { get; set; }
        public string Subject { get; set; }
        [JsonProperty("MessageID")]
        public string MessageId { get; set; }
        public string ReplyTo { get; set; }
        public string MailboxHash { get; set; }
        public string Date { get; set; }
        public string TextBody { get; set; }
        public string HtmlBody { get; set; }
        public string StrippedTextReply { get; set; }
        public string Tag { get; set; }
        public IList<Header> Headers { get; set; }
        public IList<Attachment> Attachments { get; set; }

        private bool? _isBodyHtml;
        public bool IsBodyHtml
        {
            get
            {
                if (_isBodyHtml == null)
                {
                    _isBodyHtml = !string.IsNullOrWhiteSpace(HtmlBody);
                }

                return _isBodyHtml ?? false;
            }
        }

        private DateTime? _parsedDate;
        /// <summary>
        /// Returns the JSON string date properly parsed into <see cref="DateTime"/>. Could potentially adapt via <see cref="JsonSerializerSettings"/> <see cref="DateParseHandling"/>, but that brings in a whole new set of complications.
        /// </summary>
        public DateTime ParsedDate
        {
            get
            {
                if (_parsedDate == null)
                {
                    _parsedDate = DateTime.Parse(this.Date, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal);
                }

                // Can safely assume that date is always set so never null.
                return _parsedDate.Value;
            }
        }

        //public void AttachAttachments()
        //{
        //    // Left here for reference for how best to convert to actual attachment stream.
        //    //foreach (var attachment in parsedJson.Attachments)
        //    //    message.Attachments.Add(new Attachment(new MemoryStream(Convert.FromBase64String(attachment.Content)), attachment.Name, attachment.ContentType));
        //}
    }

    public class EmailFull
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string MailboxHash { get; set; }
    }

    public class Header
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }

    public class Attachment
    {
        public string Name { get; set; }
        public string Content { get; set; }
        public string ContentType { get; set; }
        public int ContentLength { get; set; }
        [JsonProperty("ContentID")]
        public string ContentId { get; set; }
    }
}
