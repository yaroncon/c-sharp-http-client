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
 * This class is inspired by org.apache.http.entity.AbstractHttpEntity
 * 
 * ====================================================================
 *
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace CodeScales.Http.Entity
{
    public abstract class AbstractHttpEntity : HttpEntity 
    {
        // void WriteTo(OutputStream);
        private byte[] m_content;
        private string m_contentEncoding;
        private long m_contentLength = -1;
        private string m_contentType;
        private bool m_isChunked;

        // Replaces the getContent and writeTo methods in java
        public byte[] Content
        {
            get { return m_content; }
            set { m_content = value; }
        }

        // this is not the .net Encoding relevant header
        public string ContentEncoding
        {
            get { return m_contentEncoding; }
            set { m_contentEncoding = value; }
        }

        public long ContentLength
        {
            get { return m_contentLength; }
            set { m_contentLength = value; }
        }

        // this is the .net Encoding relevant header
        public string ContentType
        {
            get { return m_contentType; }
            set { m_contentType = value; }
        }
        
        // for future reference. this method is not used.
        public bool IsChunked
        {
            get { return m_isChunked; }
            set { m_isChunked = value; }
        }

    }
}
