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
using CodeScales.Http.Protocol;

namespace CodeScales.Http.Entity
{
    public class EntityUtils
    {        
        public static string ToString(HttpEntity entity)
        {
            if (entity != null
                && entity.Content != null)
            {
                return HTTPProtocol.GetEncoding(entity.ContentType).GetString(entity.Content);
            }
            else
                return string.Empty;
        }

        public static string GetContentType(WebHeaderCollection headers)
        {

            if (headers != null &&
                headers[HTTP.CONTENT_TYPE] != null)
                return headers[HTTP.CONTENT_TYPE];
            else
                return null;
        
        }

        public static int GetContentLength(WebHeaderCollection headers)
        {
            if (headers != null &&
                    headers[HTTP.CONTENT_LEN] != null)
            {
                return int.Parse(headers[HTTP.CONTENT_LEN]);
            }
            return 0;
        }

        public static string GetTransferEncoding(WebHeaderCollection headers)
        {
            if (headers != null &&
                headers[HTTP.TRANSFER_ENCODING] != null)
                return headers[HTTP.TRANSFER_ENCODING];
            else
                return null;
        }

        public static int ConvertHexToInt(string hexValue)
        {
            try
            {
                return int.Parse(hexValue, System.Globalization.NumberStyles.HexNumber);
            }
            catch
            {
                return 0;
            }
        }
    }
}
