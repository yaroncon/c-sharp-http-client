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

using CodeScales.Http.Methods;
using CodeScales.Http.Cookies;
using CodeScales.Http.Network;

namespace CodeScales.Http.Cookies
{
    public class HttpCookieStore
    {
        private Dictionary<string, HttpCookieCollection> m_cookieStore = new Dictionary<string, HttpCookieCollection>();

        internal void WriteCookiesToRequest(HttpRequest request)
        {
            string host = request.Uri.Host;
            if (this.m_cookieStore.ContainsKey(host))
            {
                HttpCookieCollection cookieColl = this.m_cookieStore[request.Uri.Host];
                if (cookieColl != null)
                {
                    request.Headers.Add("Cookie", HTTPProtocol.GetCookiesHeader(cookieColl));
                }
            }
        }

        internal void ReadCookiesFromResponse(HttpResponse response)
        {
            lock (this)
            {
                Uri responseUri = response.RequestUri;
                HttpCookieCollection cookieColl = null;
                if (this.m_cookieStore.ContainsKey(responseUri.Host))
                {
                    cookieColl = this.m_cookieStore[responseUri.Host];
                }
                else
                {
                    cookieColl = new HttpCookieCollection();
                    this.m_cookieStore.Add(responseUri.Host, cookieColl);
                }

                string [] cookieHeaders = response.Headers.GetValues("Set-Cookie");
                if (cookieHeaders != null)
                {

                    foreach (string header in cookieHeaders)
                    {
                        HTTPProtocol.AddHttpCookie(header, cookieColl);
                    }
                }
            }
        }
    }
}
