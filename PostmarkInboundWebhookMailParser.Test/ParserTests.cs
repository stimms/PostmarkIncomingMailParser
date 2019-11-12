using System;
using System.Linq;
using System.Text;
using SharpTestsEx;
using Xunit;

namespace PostmarkInboundWebhookMailParser.Test
{
    public class When_parsing_json
    {
        private readonly string _testJson = @"{
                                      'FromName': 'John Doe',
                                      'MessageStream': 'inbound',
                                      'From': 'myUser@theirDomain.com',
                                      'FromFull': {
                                        'Email': 'myUser@theirDomain.com',
                                        'Name': 'John Doe',
                                        'MailboxHash' : ''

                                      },
                                      'To': '451d9b70cf9364d23ff6f9d51d870251569e+ahoy@inbound.postmarkapp.com',
                                      'ToFull': [
                                        {
                                          'Email': '451d9b70cf9364d23ff6f9d51d870251569e+ahoy@inbound.postmarkapp.com',
                                          'Name': 'Jake Peralta',
                                          'MailboxHash' : '451d9b70cf9364d23ff6f9d51d870251569e'
                                        }
                                      ],
                                      'Cc': '\'Full name\' <sample.cc@emailDomain.com>, \'Another Cc\' <another.cc@emailDomain.com>',
                                      'CcFull': [
                                        {
                                          'Email': 'sample.cc@emailDomain.com',
                                          'Name': 'Full name',
                                          'MailboxHash' : ''
                                        },
                                        {
                                          'Email': 'another.cc@emailDomain.com',
                                          'Name': 'Another Cc',
                                          'MailboxHash' : ''
                                        }
                                      ],
                                      'Bcc': '\'Full name\' <sample.bcc@emailDomain.com>, \'Another Bcc\' <another.bcc@emailDomain.com>',
                                      'BccFull': [
                                        {
                                          'Email': 'sample.bcc@emailDomain.com',
                                          'Name': 'Full name',
                                          'MailboxHash' : ''
                                        },
                                        {
                                          'Email': 'another.bcc@emailDomain.com',
                                          'Name': 'Another Bcc',
                                          'MailboxHash' : ''
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
            _testJson = _testJson.Replace("[BASE64-ENCODED CONTENT]", Convert.ToBase64String(bytes));
        }

        [Fact]
        public void To_is_parsed()
        {
            var parser = new Parser();
            var result = parser.Parse(_testJson);

            var to = result.ToFull.First();

            to.Email.Should().Be("451d9b70cf9364d23ff6f9d51d870251569e+ahoy@inbound.postmarkapp.com");
            to.Name.Should().Be("Jake Peralta");
            to.MailboxHash.Should().Be("451d9b70cf9364d23ff6f9d51d870251569e");
        }

        [Fact]
        public void CC_is_parsed()
        {
            var parser = new Parser();
            var result = parser.Parse(_testJson);

            var to = result.CcFull.First();

            to.Email.Should().Be("sample.cc@emailDomain.com");
            to.Name.Should().Be("Full name");
            to.MailboxHash.Should().Be("");
        }

        [Fact]
        public void BCC_is_parsed()
        {
            var parser = new Parser();
            var result = parser.Parse(_testJson);

            var to = result.BccFull.First();

            to.Email.Should().Be("sample.bcc@emailDomain.com");
            to.Name.Should().Be("Full name");
            to.MailboxHash.Should().Be("");
        }

        [Fact]
        public void MessageStream_is_parsed()
        {
            var parser = new Parser();
            var result = parser.Parse(_testJson);

            result.MessageStream.Should().Be("inbound");
        }

        [Fact]
        public void From_is_parsed()
        {
            var parser = new Parser();
            var result = parser.Parse(_testJson);

            result.FromFull.Email.Should().Be("myUser@theirDomain.com");
            result.FromFull.Name.Should().Be("John Doe");
            result.FromFull.MailboxHash.Should().Be("");
        }

        [Fact]
        public void Subject_is_parsed()
        {
            var parser = new Parser();
            var result = parser.Parse(_testJson);
            result.Subject.Should().Be.EqualTo("This is an inbound message");
        }

        [Fact]
        public void Body_is_html_is_set_when_html_is_populated()
        {
            var parser = new Parser();
            var result = parser.Parse(_testJson);
            result.IsBodyHtml.Should().Be.True();
        }

        [Fact]
        public void Body_is_html_is_not_set_when_html_is_not_populated()
        {
            var parser = new Parser();
            var result = parser.Parse(_testJson.Replace("'HtmlBody': '[HTML(encoded)]',", ""));
            result.IsBodyHtml.Should().Be.False();
        }

        [Fact]
        public void Headers_are_set()
        {
            var parser = new Parser();
            var result = parser.Parse(_testJson);
            result.Headers.Count.Should().Be.EqualTo(8);
        }

        [Fact]
        public void Empty_headers_are_not_set()
        {
            var parser = new Parser();
            var result = parser.Parse(_testJson.Replace("X-Spam-Checker-Version", ""));
            result.Headers.Count.Should().Be.EqualTo(7);
        }

        [Fact]
        public void Empty_headers_values_are_not_set()
        {
            var parser = new Parser();
            var result = parser.Parse(_testJson.Replace("No", ""));
            result.Headers.Count.Should().Be.EqualTo(7);
        }

        [Fact]
        public void Headers_have_values()
        {
            var parser = new Parser();
            var result = parser.Parse(_testJson);
            result.Headers.First(x => x.Name.Equals("MIME-Version", StringComparison.OrdinalIgnoreCase)).Value.Should().Be.EqualTo("1.0");
        }

        [Fact]
        public void HtmlBody_is_parsed_for_html_email()
        {
            var parser = new Parser();
            var result = parser.Parse(_testJson);
            result.HtmlBody.Should().Be.EqualTo("[HTML(encoded)]");
        }

        [Fact]
        public void TextBody_is_parsed_for_text_email()
        {
            var parser = new Parser();
            var result = parser.Parse(_testJson.Replace("'HtmlBody': '[HTML(encoded)]',", ""));
            result.TextBody.Should().Be.EqualTo("[ASCII]");
        }

        [Fact]
        public void Message_id_is_parsed()
        {
            var parser = new Parser();
            var result = parser.Parse(_testJson);
            result.MessageId.Should().Be.EqualTo("22c74902-a0c1-4511-804f2-341342852c90");
        }

        [Fact]
        public void ParsedDate_is_parsed()
        {
            var parser = new Parser();
            var result = parser.Parse(_testJson);
            result.ParsedDate.Should().Be.EqualTo(new DateTime(2012, 4, 5, 14, 59, 1, DateTimeKind.Utc));
        }

        [Fact]
        public void Attachments_are_created()
        {
            var parser = new Parser();
            var result = parser.Parse(_testJson);
            result.Attachments.Count.Should().Be.EqualTo(2);
        }

        [Fact]
        public void Attachment_contentId_is_parsed()
        {
            var parser = new Parser();
            var result = parser.Parse(_testJson);
            var attachment = result.Attachments.First();
            attachment.ContentId.Should().Be.EqualTo("myimage.png@01CE7342.75E71F80");
        }

        //[Fact]
        //public void Attachments_are_correctly_encoded()
        //{
        //    var parser = new Parser();
        //    var result = parser.Parse(_testJson);
        //    var stream = result.Attachments.First().ContentStream;
        //    var bytes = new byte[stream.Length];
        //    stream.Read(bytes, 0, (int)stream.Length);
        //    var fileContent = Encoding.UTF8.GetString(bytes);
        //    fileContent.Should().Be.EqualTo(_fileContent);
        //}
    }
}
