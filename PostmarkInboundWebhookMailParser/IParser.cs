namespace PostmarkInboundWebhookMailParser
{
    public interface IParser
    {
        PostmarkIncomingMessage Parse(string toParse);
    }
}