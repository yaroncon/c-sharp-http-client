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

namespace CodeScales.Http.Cookies
{
    public class HttpCookieCollection
    {

        private Dictionary<string, HttpCookie> m_cookies = new Dictionary<string, HttpCookie>();

        public Dictionary<string, HttpCookie>.KeyCollection Keys
        {
            get
            {
                return this.m_cookies.Keys;
            }
        }

        public int Count
        {
            get
            {
                return this.m_cookies.Count;
            }
        }

        public HttpCookie GetCookie(string key)
        {
            return this.m_cookies[key];
        }

        public void Add(string key, HttpCookie cookie)
        {
            if (cookie.Value != null && cookie.Value != string.Empty)
            {
                if (this.m_cookies.ContainsKey(key))
                {
                    this.m_cookies[key].Value = cookie.Value;
                }
                else
                    this.m_cookies.Add(key, cookie);

                
            }
        }

        
    }
}
