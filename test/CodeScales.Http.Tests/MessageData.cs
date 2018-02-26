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
using System.Xml.Serialization;
using System.IO;

namespace CodeScales.Http.Tests
{
    [Serializable]
    public class MessageData
    {
        private NameValueColl m_queryParameters;
        private NameValueColl m_postParameters;
        private NameValueColl m_cookies;
        private NameValueColl m_files;
                
        public static MessageData Empty
        {
            get
            {
                return new MessageData();
            }
        }
        
        public NameValueColl QueryParameters
        {
            get 
            {
                if (m_queryParameters == null)
                {
                    m_queryParameters = new NameValueColl();
                }
                return m_queryParameters; 
            }
            set { m_queryParameters = value; }
        }

        public NameValueColl PostParameters
        {
            get 
            {
                if (m_postParameters == null)
                {
                    m_postParameters = new NameValueColl();
                }
                return m_postParameters; 
            }
            set { m_postParameters = value; }
        }

        public NameValueColl Cookies
        {
            get 
            {
                if (m_cookies == null)
                {
                    m_cookies = new NameValueColl();
                }
                return m_cookies; 
            }
            set { m_cookies = value; }
        }

        public NameValueColl Files
        {
            get 
            {
                if (m_files == null)
                {
                    m_files = new NameValueColl();
                }
                return m_files; 
            }
            set { m_files = value; }
        }

        public override string ToString()
        {
            XmlSerializer xs = new XmlSerializer(typeof(MessageData));
            TextWriter writer = new StringWriter();
            xs.Serialize(writer, this);
            return writer.ToString();
        }

        public MessageData FromString(string xmlString)
        {
            XmlSerializer xs = new XmlSerializer(typeof(MessageData));
            TextReader reader = new StringReader(xmlString);
            return (MessageData)xs.Deserialize(reader);
        }



    }
}
