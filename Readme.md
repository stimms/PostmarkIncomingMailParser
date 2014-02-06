Postmark Mail Incoming Parser
=============================

Postmark is a mail service for use on Azure or other times when you don't want to have to deal with mail servers yourself. Sending mail is easy. Receiving mail involves setting up a page on your site which will take a posted JSON document. When people e-mail your application the e-mail will be converted into a JSON representation and posted to the given URL. Of course parsing this document can be a bit tricky. This library makes the process much easier. 

#Getting postmark incoming mail parser

The mail parser is in nuget so all you need to do is run 

    install-package postmarkincomingmailparser

Thank goodness for tab completion, huh? 

#Using it

The simplest way is to set up a ASP.net MVC WebAPI page. 

    public async void Post()
    {
        var parser = new PostmarkIncomingMailParser.Parser();
        var mailMessage = parser.Parse(await Request.Content.ReadAsStringAsync());
        //do something here with your mail message
    }

The mail message which is returned is an extension of the standard System.Net.MailMessage. It adds a couple of extra fields which are postmark specific

-MessageId - the ID of the message from postmark
-Date - the date of the message (see the issues section)

#Issues

Everything here is done in memory so if you have a large attachment it is going to gobble up your memory like a donkey eating a waffle. There are some possible efficiencies which will help with that but I haven't implemented them. More work needs to be done with streams instead of converting attachments into strings.

Also the timezones are almost certainly wrong because timezones are always wrong. I should probably take a dependency on noda.