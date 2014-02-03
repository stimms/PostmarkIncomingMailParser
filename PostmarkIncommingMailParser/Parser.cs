using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace PostmarkIncommingMailParser
{
    public class Parser
    {
        public PostmarkMailMessage Parse(string toParse)
        {
            var message = new PostmarkMailMessage();
            var parsedJson = System.Web.Helpers.Json.Decode(toParse);

            foreach (dynamic to in parsedJson.ToFull)
            {
                message.To.Add(new MailAddress(to.Email, to.Name));
            }
            
            return message;
        }
    }

    public class PostmarkMailMessage: System.Net.Mail.MailMessage
    {

    }
}
