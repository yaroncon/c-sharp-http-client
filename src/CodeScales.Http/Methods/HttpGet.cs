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
using System.Web;

namespace CodeScales.Http.Methods
{
    public class HttpGet : HttpRequest
    {
        private static string METHOD = "GET";
        
        public HttpGet()
        {
            this.Headers.Add("UserAgent", @"Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)");
        }

        public HttpGet(Uri uri) : this()
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
            Uri url = AddParametersToPath(this.Uri);
            
            string requestUri = null;
            if (useProxy)
            {
                requestUri = url.AbsoluteUri;
            }
            else
            {
                requestUri = url.PathAndQuery;
            }

            return Method + " " + requestUri + " HTTP/1.1";
        }

        private Uri AddParametersToPath(Uri url)
        {
            if (this.Parameters.Count > 0)
            {
                string absoluteUri = url.AbsoluteUri;
                StringBuilder newQuery = new StringBuilder();
                newQuery.Append(absoluteUri);
                if (absoluteUri.IndexOf('?') < 0)
                {
                    newQuery.Append('?');
                }
                else
                {
                    newQuery.Append('&');
                }
                int counter = 1;
                foreach (string key in this.Parameters.Keys)
                {
                    newQuery.Append(key + '=' + HttpUtility.UrlEncode(this.Parameters[key]));
                    if (counter < this.Parameters.Count)
                    {
                        newQuery.Append('&');
                    }
                    counter++;
                }
                return new Uri(newQuery.ToString());
            }
            else
                return url;
        }

        

    }
}
