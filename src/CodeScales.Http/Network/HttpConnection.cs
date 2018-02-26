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
using System.Text.RegularExpressions;
using System.Net;
using System.Net.Sockets;
using System.Threading;

using CodeScales.Http.Methods;
using CodeScales.Http.Entity;
using CodeScales.Http.Protocol;

namespace CodeScales.Http.Network
{
    internal class HttpConnection
    {

        private HttpConnectionFactory m_factory = null;
        private bool m_isBusy = false;
        private Socket m_socket = null;
        private Uri m_proxy = null;
        private Uri m_uri = null;
        private int m_timeout = 60 * 1000;

        internal HttpConnection(HttpConnectionFactory factory, Uri uri, Uri proxy)
        {
            this.m_factory = factory;
            this.m_uri = uri;
            this.m_proxy = proxy;
        }

        internal Uri Uri
        {
            get
            {
                return this.m_uri;
            }
        }

        internal Uri EndPointUri
        {
            get
            {
                if (this.m_proxy != null)
                {
                    return this.m_proxy;
                }
                else
                {
                    return this.m_uri;
                }
            }
        }

        internal bool IsConnected()
        {
            return (m_socket != null
                    && m_socket.Connected);
        }
        
        internal void Connect()
        {
            m_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint remoteEP = new IPEndPoint(Dns.Resolve(this.EndPointUri.Host).AddressList[0], EndPointUri.Port);
            m_socket.Connect(remoteEP);

            m_socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, 60 * 1000);
            m_socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 60 * 1000);
        }

        public void Close()
        {
            if (m_socket != null)
            {
                m_socket.Close();
            }
        }

        public bool IsBusy()
        {
            return this.m_isBusy;
        }

        public void SetBusy(bool busyState)
        {
            this.m_isBusy = busyState;
        }

        public void CheckKeepAlive(HttpResponse response)
        {
            WebHeaderCollection headers = response.Headers;

            if ((headers[HTTP.CONN_DIRECTIVE] != null && headers[HTTP.CONN_DIRECTIVE].ToLower() == HTTP.CONN_KEEP_ALIVE) ||
                (headers[HTTP.PROXY_CONN_DIRECTIVE] != null && headers[HTTP.PROXY_CONN_DIRECTIVE].ToLower() == HTTP.CONN_KEEP_ALIVE))
            {
                return;
            }
            else
            {
                this.Close();
            }
        }
                
        public void SendRequestHeader(HttpRequest request)
        {
            if (!this.IsConnected())
            {
                throw new HttpNetworkException("Socket is closed or not ready");
            }
            this.m_socket.Send(Encoding.ASCII.GetBytes(GetRequestHeader(request).ToString()));
            
            int counter = 0;
            // wait another timeout period for the response to arrive.
            while (!(this.m_socket.Available > 0) && counter < (this.m_timeout / 100))
            {
                counter++;
                Thread.Sleep(100);
            }
        }
                
        public void SendRequestHeaderAndEntity(HttpRequest request, HttpEntity httpEntity, bool expectContinue)
        {
            if (!this.IsConnected())
            {
                throw new HttpNetworkException("Socket is closed or not ready");
            }
            byte[] header = Encoding.ASCII.GetBytes(GetRequestHeader(request).ToString());
            byte[] body = httpEntity.Content;
            
            // first send the headers
            Send(header, 0, header.Length, this.m_timeout);
            
            // then look for 100-continue response for no more than 2 seconds
            if (expectContinue)
            {
                // 2 seconds timeout for 100-continue
                WaitForDataToArriveAtSocket(2 * 1000);
                if (this.m_socket.Available > 0)
                {
                    // now read the 100-continue response
                    HttpResponse response = ReceiveResponseHeaders();
                    if (response.ResponseCode != 100)
                    {
                        throw new HttpNetworkException("reponse returned before entity was sent, but it is not 100-continue");
                    }
                }
            }

            byte[] message = body;   

            int sendBufferSize = this.m_socket.SendBufferSize;
            int size = (message.Length > sendBufferSize) ? sendBufferSize : message.Length;
            for (int i = 0; i < message.Length; i = i + size)
            {
                int remaining = (size < (message.Length - i)) ? size : (message.Length - i);
                Send(message, i, remaining, this.m_timeout);
            }
            
        }

        private void WaitForDataToArriveAtSocket(int timeout)
        {
            int counter = 0;
            // wait another timeout period for the response to arrive.
            while (!(this.m_socket.Available > 0) && counter < (timeout / 100))
            {
                counter++;
                Thread.Sleep(100);
            }
        }

        private void Send(byte[] buffer, int offset, int size, int timeout)
        {
            int startTickCount = Environment.TickCount;
            int sent = 0;  // how many bytes is already sent
            do
            {
                if (Environment.TickCount > startTickCount + timeout)
                    throw new Exception("Timeout.");
                try
                {
                    sent += this.m_socket.Send(buffer, offset + sent, size - sent, SocketFlags.None);
                }
                catch (SocketException ex)
                {
                    if (ex.SocketErrorCode == SocketError.WouldBlock ||
                        ex.SocketErrorCode == SocketError.IOPending ||
                        ex.SocketErrorCode == SocketError.NoBufferSpaceAvailable)
                    {
                        // socket buffer is probably full, wait and try again
                        Thread.Sleep(30);
                    }
                    else
                        throw ex;  // any serious error occurr
                }
            } while (sent < size);
        }

        private StringBuilder GetRequestHeader(HttpRequest request)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(request.GetRequestLine(this.m_proxy != null));
            sb.AppendLine("Host: " + this.m_uri.Host);
            sb.Append(request.Headers);
            return sb;
        }

        public HttpResponse ReceiveResponseHeaders()
        {
            if (!this.IsConnected())
            {
                throw new HttpNetworkException("Socket is closed or not ready");
            }

            WaitForDataToArriveAtSocket(this.m_timeout);

            HttpResponse httpResponse = new HttpResponse();
            httpResponse.RequestUri = this.Uri;
            string Header = "";
            WebHeaderCollection Headers = new WebHeaderCollection();

            byte[] bytes = new byte[10];
            while (this.m_socket.Receive(bytes, 0, 1, SocketFlags.None) > 0)
            {
                Header += Encoding.ASCII.GetString(bytes, 0, 1);
                if (bytes[0] == '\n' && Header.EndsWith("\r\n\r\n"))
                    break;
            }
            MatchCollection matches = new Regex("[^\r\n]+").Matches(Header.TrimEnd('\r', '\n'));
            for (int n = 1; n < matches.Count; n++)
            {
                string[] strItem = matches[n].Value.Split(new char[] { ':' }, 2);
                if (strItem.Length > 1)
                {
                    if (!strItem[0].Trim().ToLower().Equals("set-cookie"))
                    {
                        Headers.Add(strItem[0].Trim(), strItem[1].Trim());
                    }
                    else
                    {
                        Headers.Add(strItem[0].Trim(), strItem[1].Trim());
                        // HTTPProtocol.AddHttpCookie(strItem[1].Trim(), cookieCollection);
                    }
                }
            }

            httpResponse.Headers = Headers;

            // set the response code
            if (matches.Count > 0)
            {
                try
                {
                    string firstLine = matches[0].Value;
                    int index1 = firstLine.IndexOf(" ");
                    int index2 = firstLine.IndexOf(" ", index1 + 1);
                    httpResponse.ResponseCode = Int32.Parse(firstLine.Substring(index1 + 1, index2 - index1 - 1));
                }
                catch (Exception ex)
                {
                    throw new HttpNetworkException("Response Code is missing from the response");
                }
            }          

            return httpResponse;

        }

        public void ReceiveResponseEntity(HttpResponse response)
        {
            if (!this.IsConnected())
            {
                throw new HttpNetworkException("Socket is closed or not ready");
            }

            string chunkedHeader = EntityUtils.GetTransferEncoding(response.Headers);
            if (chunkedHeader != null
                && chunkedHeader.ToLower().Equals(HTTP.CHUNK_CODING))
            {
                List<byte> byteBuffer = new List<byte>();
                BasicHttpEntity httpEntity = new BasicHttpEntity();
                httpEntity.ContentLength = 0;
                httpEntity.ContentType = EntityUtils.GetContentType(response.Headers);
                response.Entity = httpEntity;

                int chunkSize = EntityUtils.ConvertHexToInt(ReceiveLine());
                while (chunkSize > 0)
                {
                    // for each chunk...
                    byteBuffer.AddRange(ReceiveBytes(chunkSize));
                    httpEntity.ContentLength += chunkSize;
                    string test = ReceiveLine();
                    chunkSize = EntityUtils.ConvertHexToInt(ReceiveLine());
                }

                httpEntity.Content = byteBuffer.ToArray();
            }
            else
            {
                // TODO: support "Transfer-Encoding: chunked"
                int length = EntityUtils.GetContentLength(response.Headers);
                if (length > 0)
                {
                    BasicHttpEntity httpEntity = new BasicHttpEntity();
                    httpEntity.ContentLength = length;
                    httpEntity.Content = ReceiveBytes(length).ToArray();
                    httpEntity.ContentType = EntityUtils.GetContentType(response.Headers);
                    response.Entity = httpEntity;
                }
            }
                         
            return;
        }

        private string ReceiveLine()
        {
            WaitForDataToArriveAtSocket(this.m_timeout);

            string line = string.Empty;
            byte[] bytes = new byte[10];
            while (this.m_socket.Receive(bytes, 0, 1, SocketFlags.None) > 0)
            {
                line += Encoding.ASCII.GetString(bytes, 0, 1);
                if (bytes[0] == '\n' && line.EndsWith("\r\n"))
                    break;
            }
            return line.Replace("\r\n","");
        }

        private List<byte> ReceiveBytes(long size)
        {
            WaitForDataToArriveAtSocket(this.m_timeout);

            List<byte> byteBuffer = new List<byte>();
            int minSize = 10240;
            if (size < 10240)
            {
                minSize = (int)size;
            }
            byte[] RecvBuffer = new byte[minSize];
            long nBytes, nTotalBytes = 0;
            int RecvSize = minSize;
            
            // loop to receive response buffer
            while ((nBytes = this.m_socket.Receive(RecvBuffer, 0, RecvSize, SocketFlags.None)) > 0)
            {
                // increment total received bytes
                nTotalBytes += nBytes;

                // add received buffer to response string
                if (nBytes < minSize)
                {
                    byte[] smallByteArray = new byte[nBytes];
                    for (int i = 0; i < nBytes; i++)
                    {
                        smallByteArray[i] = RecvBuffer[i];
                    }
                    byteBuffer.AddRange(smallByteArray);
                }
                else
                {
                    byteBuffer.AddRange(RecvBuffer);
                }

                if (nTotalBytes >= size && size > 0)
                    break;
                else
                {
                    long nBytesLeft = size - nTotalBytes;
                    if (nBytesLeft < RecvSize)
                    {
                        RecvSize = (int)nBytesLeft;
                    }
                }
            }
            return byteBuffer;
        }

    }
}
