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
    public class HttpCookie
    {
        private string m_name;
        private string m_value;
        private string m_path;
        private DateTime m_expiration;

        public HttpCookie(string name, string value)
        {
            this.m_name = name;
            this.m_value = value;
        }
        
        public string Name
        {
            get { return m_name; }
            set { m_name = value; }
        }

        public string Value
        {
            get { return m_value; }
            set { m_value = value; }
        }
        
        public string Path
        {
            get { return m_path; }
            set { m_path = value; }
        }
        
        public DateTime Expiration
        {
            get { return m_expiration; }
            set { m_expiration = value; }
        }
    }
}
