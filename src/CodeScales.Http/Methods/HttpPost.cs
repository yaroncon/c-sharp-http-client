/* Copyright (c) 2010 CodeScales.com
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
*/

using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

using CodeScales.Http.Entity;
using CodeScales.Http.Protocol;

namespace CodeScales.Http.Methods
{
    public class HttpPost : HttpRequest, HttpEntityEnclosingRequest
    {
        private static string METHOD = "POST";
        private HttpEntity m_entity = null;
        
        public HttpPost()
        {
        }

        public HttpPost(Uri uri) : this()
        {
            this.Uri = uri;
        }

        public override string Method
        {
            get
            {
                return METHOD;
            }
        }

        public override string GetRequestLine(bool useProxy)
        {
            string requestUri = null;
            if (useProxy)
            {
                requestUri = this.Uri.AbsoluteUri;
            }
            else
            {
                requestUri = this.Uri.PathAndQuery;
            }

            return Method + " " + requestUri + " HTTP/1.1";
        }

        public HttpEntity Entity
        {
            get { return m_entity; }
            set { m_entity = value; }
        }

        public override WebHeaderCollection Headers
        {
            get
            {
                if (base.Headers != null && this.m_entity != null)
                {
                    if (base.Headers[HTTP.CONTENT_LEN] == null
                    || base.Headers[HTTP.CONTENT_LEN] == string.Empty)
                    {
                        base.Headers[HTTP.CONTENT_LEN] = this.m_entity.ContentLength.ToString();
                    }

                    if (base.Headers[HTTP.CONTENT_TYPE] == null
                    || base.Headers[HTTP.CONTENT_TYPE] == string.Empty)
                    {
                        base.Headers[HTTP.CONTENT_TYPE] = this.m_entity.ContentType;
                    }
                    base.Headers.Add(HTTP.USER_AGENT, @"Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)");
                }
                    
                return base.Headers;
            }
            set
            {
                base.Headers = value;
            }
        }
        
        public bool ExpectContinue
        {
            get
            {
                if (base.Headers != null)
                {
                    return (base.Headers[HTTP.EXPECT_DIRECTIVE] != null
                        && base.Headers[HTTP.EXPECT_DIRECTIVE].Equals(HTTP.EXPECT_CONTINUE));
                }
                else return false;
            }
        }
    }
}
