﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using SharpTestsEx;

namespace PostmarkIncommingMailParser.Test
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
        [Fact]
        public void From_is_parsed()
        {
            var parser = new PostmarkIncommingMailParser.Parser();
            var result = parser.Parse(_testJSON);
            result.To.Should().Contain(new MailAddress("451d9b70cf9364d23ff6f9d51d870251569e+ahoy@inbound.postmarkapp.com", ""));
        }
    }
}
