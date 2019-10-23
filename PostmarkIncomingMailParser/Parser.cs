using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace PostmarkIncomingMailParser
{
    public class Parser : IParser
    {
        public PostmarkMessage Parse(string toParse)
        {
            var message = JsonConvert.DeserializeObject<PostmarkMessage>(toParse);

            message.Headers = CleanUpHeaders(message.Headers);

            return message;
        }

        private IList<Header> CleanUpHeaders(IEnumerable<Header> headers)
        {
            return headers.Where(header => !string.IsNullOrWhiteSpace(header.Name) && !string.IsNullOrWhiteSpace(header.Value)).ToList();
        }
    }
}