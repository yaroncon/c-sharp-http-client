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

namespace CodeScales.Http.Methods
{
    public abstract class HttpRequest : HttpMessageBase
    {
        private Uri m_uri = null;

        public abstract string Method { get; }
        public abstract string GetRequestLine(bool useProxy);

        public Uri Uri
        {
            get { return m_uri; }
            set { m_uri = value; }
        }

    }
}
