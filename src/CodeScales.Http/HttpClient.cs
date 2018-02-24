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
using System.IO;

using CodeScales.Http.Cookies;
using CodeScales.Http.Methods;
using CodeScales.Http.Network;
using CodeScales.Http.Entity;

namespace CodeScales.Http
{
    public class HttpClient
    {
        private HttpCookieStore m_cookieStore = new HttpCookieStore();
        private Uri m_proxy = null;
        private int m_maxRedirects = 1;

        public Uri Proxy
        {
            set
            {
                this.m_proxy = value;
            }
        }

        public int MaxRedirects
        {
            set
            {
                this.m_maxRedirects = value;
            }
            get
            {
                return this.MaxRedirects;
            }
        }

        public HttpResponse Execute(HttpRequest request)
        {
            return Navigate(request, null);
        }

        public HttpResponse Execute(HttpRequest request, HttpBehavior httpBehavior)
        {
            return Navigate(request, httpBehavior);
        }

        private HttpResponse Navigate(HttpRequest request, HttpBehavior httpBehavior)
        {
            bool ContinueRedirect = true;
            HttpResponse response = null;

            HttpConnectionFactory connFactory = new HttpConnectionFactory();
            HttpConnection connection = connFactory.GetConnnection(request.Uri, this.m_proxy);

            HttpBehavior.RedirectStep rs = null;
            string redirectUri = null;
            int responseCode = 0;
            int redirectCounter = 0;

            try
            {
                while (ContinueRedirect)
                {
                    try
                    {
                        response = SendRequestAndGetResponse(connection, request);
                        redirectUri = response.Location;
                        responseCode = response.ResponseCode;

                        // response code 100 means that we need to wait for another response
                        // and receive the response from the socket again on the same connection
                        if (responseCode == 100)
                        {
                            response = GetResponse(connection);
                            redirectUri = response.Location;
                            responseCode = response.ResponseCode;
                        }

                        if (httpBehavior != null)
                        {
                            rs = httpBehavior.GetNextStep();
                            rs.Compare(responseCode, redirectUri);
                            ContinueRedirect = !httpBehavior.IsEmpty();
                        }
                        else
                        {
                            ContinueRedirect = (redirectCounter < this.m_maxRedirects && (responseCode == 301 || responseCode == 302));
                            redirectCounter++;
                        }

                        if (ContinueRedirect)
                        {
                            request = new HttpGet(new Uri(redirectUri));
                            // make sure the connection is still open and redirect url is from the same host
                            connection = connFactory.GetConnnection(request.Uri, this.m_proxy, connection);
                        }

                    }
                    catch (Exception ex)
                    {
                        int i = 0;
                        throw ex;
                    }

                }
            }
            finally
            {
                connection.Close();
            }
            return response;      

        }

        private HttpResponse SendRequestAndGetResponse(HttpConnection connection, HttpRequest request)
        {
            m_cookieStore.WriteCookiesToRequest(request);
            
            // if we need to send a body (not only headers)
            if (request.GetType().GetInterface("HttpEntityEnclosingRequest") != null)
            {
                HttpEntityEnclosingRequest heer = (HttpEntityEnclosingRequest)request;
                connection.SendRequestHeaderAndEntity(request, heer.Entity, heer.ExpectContinue);
            }
            else
            {
                connection.SendRequestHeader(request);
            }

            return GetResponse(connection);
            
        }

        private HttpResponse GetResponse(HttpConnection connection)
        {
            HttpResponse response = connection.ReceiveResponseHeaders();
            m_cookieStore.ReadCookiesFromResponse(response);
            connection.ReceiveResponseEntity(response);
            
            // for response code 100 we expect another http message to arrive later
            if (response.ResponseCode != 100)
            {
                connection.CheckKeepAlive(response);
            }
            return response;
        }

        private string GetRedirectUri(Uri uri)
        {
            if (uri != null)
                return uri.AbsoluteUri;
            else
                return "";
        }

        
    }
}
