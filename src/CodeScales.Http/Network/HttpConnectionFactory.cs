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

namespace CodeScales.Http.Network
{
    internal class HttpConnectionFactory
    {
        public HttpConnection GetConnnection(Uri uri, Uri proxy)
        {
            return GetConnectionPrivate(uri, proxy);
        }
        
        // make sure the connection is still open and from the same host
        public HttpConnection GetConnnection(Uri uri, Uri proxy, HttpConnection liveConnection)
        {
            if (liveConnection != null
                && liveConnection.IsConnected()
                && liveConnection.Uri.Host.ToLower() == uri.Host.ToLower())
            {
                return liveConnection;
            }
            else
            {
                liveConnection.Close();
                return GetConnectionPrivate(uri, proxy);
            }
        }

        private HttpConnection GetConnectionPrivate(Uri uri, Uri proxy)
        {
            HttpConnection conn = new HttpConnection(this, uri, proxy);
            conn.SetBusy(true);
            conn.Connect();
            return conn;
        }
    }
}
