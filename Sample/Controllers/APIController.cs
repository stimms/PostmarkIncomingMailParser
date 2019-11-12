using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace Sample.Controllers
{
    public class APIController : ApiController
    {
        public async Task<string> Post()
        {
            var parser = new PostmarkInboundWebhookMailParser.Parser();
            var json = await Request.Content.ReadAsStringAsync();
            WebApiApplication.LastMailMessageJson = json;

            try
            {
                var mailMessage = parser.Parse(json);
                WebApiApplication.LastMailMessage = mailMessage;
                WebApiApplication.LastMailMessageDate = DateTime.Now;
                return "Messaged parsed";
            }
            catch (Exception ex)
            {
                WebApiApplication.LastError = ex;
                WebApiApplication.LastErrorDate = DateTime.Now;
                return "Unable to parse message";
            }
        }
    }
}
