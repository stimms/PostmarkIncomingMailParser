using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;


namespace Sample.Controllers
{
    public class APIController : ApiController
    {
        public async Task<String> Post()
        {
            var parser = new PostmarkIncomingMailParser.Parser();
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
