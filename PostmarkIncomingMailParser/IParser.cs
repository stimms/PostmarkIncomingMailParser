namespace PostmarkIncomingMailParser
{
    public interface IParser
    {
        PostmarkIncomingMessage Parse(string toParse);
    }
}