namespace PostmarkIncomingMailParser
{
    public interface IParser
    {
        PostmarkMessage Parse(string toParse);
    }
}