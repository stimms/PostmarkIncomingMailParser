# Postmark Inbound Webhook Mail Parser 
Fork of [PostmarkIncomingMailParser](https://github.com/stimms/PostmarkIncomingMailParser)

[Postmark](https://postmarkapp.com/) provides a rock-solid transactional email service. Additionally, they have an inbound webhook mail service for use on Azure or other times when you don't want to have to deal with mail servers yourself. Or when you have unique requirements such as handling email sent to generated email addresses (see [this for just one example](https://postmarkapp.com/blog/how-to-process-generated-email-addresses-with-google-apps-and-postmark)). For transactional email, use [Postmark's .NET library for their API](https://github.com/wildbit/postmark-dotnet). However, for inbound webhook email processing, receiving mail involves setting up a page on your site or in your API which will take a posted JSON document. When people email your application, Postmark accepts and parses the email into a JSON representation and subsequently POSTs it to the given URL. Of course, parsing this incoming JSON data can be a bit tricky--this library makes the process much easier. 

Note: this is a fork of https://github.com/stimms/PostmarkIncomingMailParser which appears to no longer be maintained. Many thanks to https://github.com/stimms for the original implementation. This implementation contains several breaking changes but deals with a couple of shortcomings including a switch to Newtonsoft.Json.JsonConvert (among many other advantages, it makes JSON payload maximum size much less of an issue), inclusion of ContentID for inline images as attachments, no reliance on System.Net.MailMessage (in case you want to use a non-SMTP third-party mail relay), and more.

# Getting Postmark incoming mail parser

This forked mail parser is now also in nuget so simply run the following: 

    install-package PostmarkInboundWebhookMailParser

If you want the original that does not appear to be maintained anymore, run:

    install-package postmarkincomingmailparser

# Using it

First you should set up a URL in the Postmark settings under the incoming hook. This should be the final URL of your published end point. Obviously there cannot be a password around the API as there is nowhere to set it, but Postmark provides [some security recommendations here](https://postmarkapp.com/blog/putting-webhooks-to-work) and BASIC authentication is always an option as well. The ability to provide an auth token or credentials is something nice Postmark could add.

The simplest way to set up a page to be the endpoint is to set up a ASP.net MVC WebAPI page. It can contain just a single method

    public async void Post()
    {
        var parser = new PostmarkInboundWebhookMailParser.Parser(); // Or via dependency injection
        var model = parser.Parse(await Request.Content.ReadAsStringAsync());
        //do something here with your mail message model such as mapping it to a dynamic template model you can send via https://github.com/wildbit/postmark-dotnet
    }

~~The mail message which is returned is an extension of the standard System.Net.MailMessage. It adds a couple of extra fields which are postmark specific~~

~~-MessageId - the ID of the message from postmark
-Date - the date of the message (see the issues section)~~

## Handling attachments (inline and otherwise)

As to handling attachments to send via Postmark's transactional email service, you can easily add attachments to PostmarkMessage or TemplatedPostmarkMessage by converting the Base64 string into the required byte array: 

     new PostmarkMessageAttachment(Convert.FromBase64String(attachment.Content), attachment.Name, attachment.ContentType));

Here's how to deal with inline images in HtmlBody:

     new PostmarkMessageAttachment(Convert.FromBase64String(attachment.Content), attachment.Name, attachment.ContentType, $"cid:{attachment.ContentId}")                         : 

If `ContentId` is specified you know that it's an inline entity.

# Issues

Everything here is done in memory so if you have a large attachment it is going to gobble up your memory like a donkey eating a waffle. A potential improvement would be to serialize directly from the request stream (e.g. via NewtonSoft.Json.JsonSerializer.Deserialize), but maybe YAGNI...I haven't personally found a need for it yet.

~~Also the timezones are almost certainly wrong because timezones are always wrong. I should probably take a dependency on noda.~~ Fixed.
