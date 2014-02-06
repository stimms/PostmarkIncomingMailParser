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
            var mailMessage = parser.Parse(await Request.Content.ReadAsStringAsync());
            WebApiApplication.LastMailMessage = mailMessage;
            return "Messaged parsed";
        }
    }
}
