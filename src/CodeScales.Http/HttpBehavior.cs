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

namespace CodeScales.Http
{
    public class HttpBehavior
    {
        private Queue<RedirectStep> m_steps = new Queue<RedirectStep>();

        public void AddStep(int responseCode)
        {
            AddStep(responseCode, null);
        }

        public void AddStep(int responseCode, string url)
        {
            RedirectStep rs = new RedirectStep();
            rs.ResponseCode = responseCode;
            rs.Url = url;
            m_steps.Enqueue(rs);
        }

        public bool IsEmpty()
        {
            return m_steps.Count == 0;
        }

        public RedirectStep GetNextStep()
        {
            if (m_steps.Count > 0)
            {
                return m_steps.Dequeue();
            }
            else
            {
                // if there is no next step
                throw new HttpBehaviorException();
            }
        }

        public class RedirectStep
        {
            private int m_responseCode = 0;
            private string m_url = null;

            public int ResponseCode
            {
                get { return m_responseCode; }
                set { m_responseCode = value; }
            }

            public string Url
            {
                get { return m_url; }
                set { m_url = value; }
            }

            internal void Compare(int responseStatus, string redirectUri)
            {
                if (m_responseCode != responseStatus)
                {
                    throw new HttpBehaviorException(responseStatus, redirectUri);
                }

                if (m_responseCode == 301 || m_responseCode == 302)
                {
                    if (IsEmpty(redirectUri) && !IsEmpty(m_url))
                        throw new HttpBehaviorException();

                    if (!IsEmpty(redirectUri) && IsEmpty(m_url))
                        throw new HttpBehaviorException();

                    if (redirectUri.StartsWith(m_url))
                    {
                        return; //OK
                    }
                    else
                    {
                        throw new HttpBehaviorException();
                    }
                }
            }

            private bool IsEmpty(string uri)
            {
                if (uri == null || uri.Equals(string.Empty))
                    return true;
                else
                    return false;
            }

            internal bool IsRedirect()
            {
                return (m_responseCode == 301 || m_responseCode == 302);
            }
        }
    }

    public class HttpBehaviorException : Exception
    {
        private int m_responseCode = 0;
        private string m_locatioon = null;

        public HttpBehaviorException()
        {
        }

        public HttpBehaviorException(int responseCode, string location)
        {
            this.m_responseCode = responseCode;
            this.m_locatioon = location;
        }
    }

}

