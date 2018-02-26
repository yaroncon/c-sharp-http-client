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

namespace CodeScales.Http.Protocol
{
    public class HTTP
    {
         /** HTTP header definitions */ 
        public const string TRANSFER_ENCODING = "Transfer-Encoding";
        public const string CONTENT_LEN  = "Content-Length";
        public const string CONTENT_TYPE = "Content-Type";
        public const string CONTENT_ENCODING = "Content-Encoding";
        public const string EXPECT_DIRECTIVE = "Expect";
        public const string CONN_DIRECTIVE = "Connection";
        public const string PROXY_CONN_DIRECTIVE = "Proxy-Connection";
        public const string TARGET_HOST = "Host";
        public const string USER_AGENT = "User-Agent";
        public const string DATE_HEADER = "Date";
        public const string SERVER_HEADER = "Server";
    
        /** HTTP expectations */
        public  const string EXPECT_CONTINUE = "100-Continue";

        /** HTTP connection control */
        public  const string CONN_CLOSE = "Close";
        public  const string CONN_KEEP_ALIVE = "Keep-Alive";
        
        /** Transfer encoding definitions */
        public  const string CHUNK_CODING = "chunked";
        public  const string IDENTITY_CODING = "identity";
    }
}
