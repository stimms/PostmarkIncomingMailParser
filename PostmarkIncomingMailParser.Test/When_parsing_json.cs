﻿using Xunit;
using System;
using System.Linq;
using System.Text;
using SharpTestsEx;
using System.Net.Mail;
using System.Collections.Generic;

namespace PostmarkIncomingMailParser.Test
{
    public class When_parsing_json
    {
        private string _testJSON = @"{
                                      'From': 'myUser@theirDomain.com',
                                      'FromFull': {
                                        'Email': 'myUser@theirDomain.com',
                                        'Name': 'John Doe'
                                      },
                                      'To': '451d9b70cf9364d23ff6f9d51d870251569e+ahoy@inbound.postmarkapp.com',
                                      'ToFull': [
                                        {
                                          'Email': '451d9b70cf9364d23ff6f9d51d870251569e+ahoy@inbound.postmarkapp.com',
                                          'Name': ''
                                        }
                                      ],
                                      'Cc': '\'Full name\' <sample.cc@emailDomain.com>, \'Another Cc\' <another.cc@emailDomain.com>',
                                      'CcFull': [
                                        {
                                          'Email': 'sample.cc@emailDomain.com',
                                          'Name': 'Full name'
                                        },
                                        {
                                          'Email': 'another.cc@emailDomain.com',
                                          'Name': 'Another Cc'
                                        }
                                      ],
                                      'ReplyTo': 'myUsersReplyAddress@theirDomain.com',
                                      'Subject': 'This is an inbound message',
                                      'MessageID': '22c74902-a0c1-4511-804f2-341342852c90',
                                      'Date': 'Thu, 5 Apr 2012 16:59:01 +0200',
                                      'MailboxHash': 'ahoy',
                                      'TextBody': '[ASCII]',
                                      'HtmlBody': '[HTML(encoded)]',
                                      'Tag': '',
                                      'Headers': [
                                        {
                                          'Name': 'X-Spam-Checker-Version',
                                          'Value': 'SpamAssassin 3.3.1 (2010-03-16) onrs-ord-pm-inbound1.wildbit.com'
                                        },
                                        {
                                          'Name': 'X-Spam-Status',
                                          'Value': 'No'
                                        },
                                        {
                                          'Name': 'X-Spam-Score',
                                          'Value': '-0.1'
                                        },
                                        {
                                          'Name': 'X-Spam-Tests',
                                          'Value': 'DKIM_SIGNED,DKIM_VALID,DKIM_VALID_AU,SPF_PASS'
                                        },
                                        {
                                          'Name': 'Received-SPF',
                                          'Value': 'Pass (sender SPF authorized) identity=mailfrom; client-ip=209.85.160.180; helo=mail-gy0-f180.google.com; envelope-from=myUser@theirDomain.com; receiver=451d9b70cf9364d23ff6f9d51d870251569e+ahoy@inbound.postmarkapp.com'
                                        },
                                        {
                                          'Name': 'DKIM-Signature',
                                          'Value': 'v=1; a=rsa-sha256; c=relaxed\/relaxed;        d=wildbit.com; s=google;        h=mime-version:reply-to:date:message-id:subject:from:to:cc         :content-type;        bh=cYr\/+oQiklaYbBJOQU3CdAnyhCTuvemrU36WT7cPNt0=;        b=QsegXXbTbC4CMirl7A3VjDHyXbEsbCUTPL5vEHa7hNkkUTxXOK+dQA0JwgBHq5C+1u         iuAJMz+SNBoTqEDqte2ckDvG2SeFR+Edip10p80TFGLp5RucaYvkwJTyuwsA7xd78NKT         Q9ou6L1hgy\/MbKChnp2kxHOtYNOrrszY3JfQM='
                                        },
                                        {
                                          'Name': 'MIME-Version',
                                          'Value': '1.0'
                                        },
                                        {
                                          'Name': 'Message-ID',
                                          'Value': '<CAGXpo2WKfxHWZ5UFYCR3H_J9SNMG+5AXUovfEFL6DjWBJSyZaA@mail.gmail.com>'
                                        }
                                      ],
                                      'Attachments': [
                                        {
                                          'Name': 'myimage.png',
                                          'Content': '[BASE64-ENCODED CONTENT]',
                                          'ContentType': 'image/png',
                                          'ContentLength': 4096,
                                          'ContentID': 'myimage.png@01CE7342.75E71F80'
                                        },
                                        {
                                          'Name': 'mypaper.doc',
                                          'Content': '[BASE64-ENCODED CONTENT]',
                                          'ContentType': 'application/msword',
                                          'ContentLength': 16384,
                                          'ContentID': ''
                                        }
                                      ]
                                    }";
        private string _fileContent = "I like traffic lights";

        public When_parsing_json()
        {
            var bytes = Encoding.UTF8.GetBytes(_fileContent);
            _testJSON = _testJSON.Replace("[BASE64-ENCODED CONTENT]", Convert.ToBase64String(bytes));
        }
        [Fact]
        public void To_is_parsed()
        {
            var parser = new PostmarkIncomingMailParser.Parser();
            var result = parser.Parse(_testJSON);
            result.To.Should().Contain(new MailAddress("451d9b70cf9364d23ff6f9d51d870251569e+ahoy@inbound.postmarkapp.com", ""));
        }

        [Fact]
        public void CC_is_parsed()
        {
            var parser = new PostmarkIncomingMailParser.Parser();
            var result = parser.Parse(_testJSON);
            result.CC.Should().Contain(new MailAddress("sample.cc@emailDomain.com", "Full name"));
        }

        [Fact]
        public void From_is_parsed()
        {
            var parser = new PostmarkIncomingMailParser.Parser();
            var result = parser.Parse(_testJSON);
            result.From.Should().Be.EqualTo(new MailAddress("myUser@theirDomain.com", "John Doe"));
        }

        [Fact]
        public void Subject_is_parsed()
        {
            var parser = new PostmarkIncomingMailParser.Parser();
            var result = parser.Parse(_testJSON);
            result.Subject.Should().Be.EqualTo("This is an inbound message");
        }

        [Fact]
        public void Body_is_html_is_set_when_html_is_populated()
        {
            //any time we see text in the html body we assume the body is HTML... I don't know if that is right
            var parser = new PostmarkIncomingMailParser.Parser();
            var result = parser.Parse(_testJSON);
            result.IsBodyHtml.Should().Be.True();
        }

        [Fact]
        public void Body_is_html_is_not_set_when_html_is_not_populated()
        {
            //any time we see text in the html body we assume the body is HTML... I don't know if that is right
            var parser = new PostmarkIncomingMailParser.Parser();
            var result = parser.Parse(_testJSON.Replace("'HtmlBody': '[HTML(encoded)]',",""));
            result.IsBodyHtml.Should().Be.False();
        }

        [Fact]
        public void Headers_are_set()
        {
            var parser = new PostmarkIncomingMailParser.Parser();
            var result = parser.Parse(_testJSON);
            result.Headers.Count.Should().Be.EqualTo(8);
        }

        [Fact]
        public void Empty_headers_are_not_set()
        {
            var parser = new PostmarkIncomingMailParser.Parser();
            var result = parser.Parse(_testJSON.Replace("X-Spam-Checker-Version", ""));
            result.Headers.Count.Should().Be.EqualTo(7);
        }

        [Fact]
        public void Empty_headers_values_are_not_set()
        {
            var parser = new PostmarkIncomingMailParser.Parser();
            var result = parser.Parse(_testJSON.Replace("No", ""));
            result.Headers.Count.Should().Be.EqualTo(7);
        }
        
        [Fact]
        public void Headers_have_values()
        {
            var parser = new PostmarkIncomingMailParser.Parser();
            var result = parser.Parse(_testJSON);
            result.Headers.Get("MIME-Version").Should().Be.EqualTo("1.0");
        }

        [Fact]
        public void Body_is_parsed_for_html_email()
        {
            var parser = new PostmarkIncomingMailParser.Parser();
            var result = parser.Parse(_testJSON);
            result.Body.Should().Be.EqualTo("[HTML(encoded)]");
        }

        [Fact]
        public void Body_is_parsed_for_text_email()
        {
            var parser = new PostmarkIncomingMailParser.Parser();
            var result = parser.Parse(_testJSON.Replace("'HtmlBody': '[HTML(encoded)]',", ""));
            result.Body.Should().Be.EqualTo("[ASCII]");
        }

        [Fact]
        public void Message_id_is_parsed()
        {
            var parser = new PostmarkIncomingMailParser.Parser();
            var result = parser.Parse(_testJSON);
            result.MessageId.Should().Be.EqualTo("22c74902-a0c1-4511-804f2-341342852c90");
        }

        [Fact]
        public void Date_is_parsed()
        {
            var parser = new PostmarkIncomingMailParser.Parser();
            var result = parser.Parse(_testJSON);
            result.Date.Should().Be.EqualTo(new DateTime(2012, 4, 5, 8, 59, 1, DateTimeKind.Utc));
        }

        [Fact]
        public void Attachments_are_created()
        {
            var parser = new PostmarkIncomingMailParser.Parser();
            var result = parser.Parse(_testJSON);
            result.Attachments.Count.Should().Be.EqualTo(2);
        }

        [Fact]
        public void Attachments_are_correctly_encoded()
        {
            var parser = new PostmarkIncomingMailParser.Parser();
            var result = parser.Parse(_testJSON);
            var stream =result.Attachments.First().ContentStream;
            var bytes = new byte[stream.Length];
            stream.Read(bytes, 0, (int)stream.Length);
            var fileContent = Encoding.UTF8.GetString(bytes);
            fileContent.Should().Be.EqualTo(_fileContent);
        }

        //Thu, 5 Apr 2012 16:59:01 +0200
    }
}
