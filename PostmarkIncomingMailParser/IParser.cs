namespace PostmarkIncomingMailParser
{
    public interface IParser
    {
        PostmarkMailMessage Parse(string toParse);
    }
}