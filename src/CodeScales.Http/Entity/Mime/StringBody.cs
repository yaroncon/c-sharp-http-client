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
 * This class is inspired by org.apache.http.entity.mime.content.StringBody
 * 
 * ====================================================================
 *
 */

using System;
using System.Collections.Generic;
using System.Text;

using CodeScales.Http.Network;

namespace CodeScales.Http.Entity.Mime
{
    public class StringBody : Body
    {
        private Encoding m_encoding;
        private string m_name;
        private string m_value;

        public StringBody(Encoding encoding, string name, string value)
        {
            this.m_encoding = encoding;
            this.m_name = name;
            this.m_value = value;
        }

        public byte[] GetContent(string boundry)
        {
            return this.m_encoding.GetBytes(HTTPProtocol.GetPostParameter(this.m_name, this.m_value, boundry));            
        }
    }
}
