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

using CodeScales.Http.Network;
using CodeScales.Http.Entity;

namespace CodeScales.Http.Methods
{
    public class HttpResponse : HttpMessageBase
    {
        private int m_responseCode = 0;
        private Uri m_requestUri = null;
        private HttpEntity m_entity = null;
                                        
        public HttpResponse()
        {

        }

        public int ResponseCode
        {
            get { return m_responseCode; }
            set { m_responseCode = value; }
        }

        public Uri RequestUri
        {
            get { return m_requestUri; }
            set { m_requestUri = value; }
        }

        public HttpEntity Entity
        {
            get { return m_entity; }
            set { m_entity = value; }
        }             

        public string Location
        {
            get
            {
                if (this.Headers != null &&
                    this.Headers["Location"] != null)
                {
                    try { return new Uri(this.Headers["Location"]).AbsoluteUri; }
                    catch { return new Uri(this.m_requestUri, this.Headers["Location"]).AbsoluteUri; }
                }
                else
                    return string.Empty;
            }
        }



        
    }
}