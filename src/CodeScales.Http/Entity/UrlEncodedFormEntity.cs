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

/*
 * ====================================================================
 *
 * This class is inspired by org.apache.http.client.entity.UrlEncodedFormEntity
 * 
 * ====================================================================
 *
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

using CodeScales.Http.Common;
using CodeScales.Http.Network;

namespace CodeScales.Http.Entity
{
    public class UrlEncodedFormEntity : HttpEntity
    {
        private Encoding m_encoding;
        private List<NameValuePair> m_parameters;
        private byte[] m_content;

        public UrlEncodedFormEntity(List<NameValuePair> parameters, Encoding encoding)
        {
            this.m_parameters = parameters;
            this.m_encoding = encoding;

            StringBuilder stringBuilder = new StringBuilder();
            HTTPProtocol.AddPostParameters(this.m_parameters, stringBuilder);
            m_content = this.m_encoding.GetBytes(stringBuilder.ToString());
        }
        
        public string ContentEncoding
        {
            get { return null; }
            set {}
        }
        
        public string ContentType
        {
            get { return "application/x-www-form-urlencoded"; }
            set {}
        }

        public byte[] Content
        {
            get
            {
                return this.m_content;
            }
            set {}
        }

        public long ContentLength
        {
            get
            {
                if (this.m_content != null)
                {
                    return this.m_content.GetLongLength(0);
                }
                else
                    return 0;
            }
            set {}
        }

        public bool IsChunked
        {
            get
            {
                return false;
            }
            set {}
        }
    }
}
