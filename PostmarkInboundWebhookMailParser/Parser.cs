using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace PostmarkInboundWebhookMailParser
{
    public class Parser : IParser
    {
        public PostmarkIncomingMessage Parse(string toParse)
        {
            var message = JsonConvert.DeserializeObject<PostmarkIncomingMessage>(toParse);

            if (message != null)
                message.Headers = CleanUpHeaders(message.Headers);

            return message;
        }

        private IList<Header> CleanUpHeaders(IEnumerable<Header> headers)
        {
            return headers?.Where(header => !string.IsNullOrWhiteSpace(header.Name) && !string.IsNullOrWhiteSpace(header.Value)).ToList();
        }
    }
}