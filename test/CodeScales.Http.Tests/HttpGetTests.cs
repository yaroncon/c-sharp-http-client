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

using CodeScales.Http;
using CodeScales.Http.Common;
using CodeScales.Http.Methods;
using CodeScales.Http.Entity;
using NUnit.Framework;

namespace CodeScales.Http.Tests
{
    [TestFixture] 
    public class HttpGetTests
    {
        [Test]
        public void HttpGet()
        {
            HttpClient client = new HttpClient();
            HttpGet getMethod = new HttpGet(new Uri(Constants.HTTP_GET_200));
            HttpResponse response = client.Execute(getMethod);

            Assert.AreEqual(200, response.ResponseCode);
            string responseString = EntityUtils.ToString(response.Entity);
            Assert.AreEqual(MessageData.Empty.ToString(), responseString);
            Assert.AreEqual(Constants.HTTP_GET_200, response.RequestUri.AbsoluteUri);
            Console.Write(responseString);
            
        }

        [Test]
        public void HttpGetWithParameters()
        {
            HttpClient client = new HttpClient();
            HttpGet getMethod = new HttpGet(new Uri(Constants.HTTP_GET_200));
            getMethod.Parameters.Add("test", "1 and text");
            getMethod.Parameters.Add("test2", "2 and text <> &&");
            HttpResponse response = client.Execute(getMethod);

            Assert.AreEqual(200, response.ResponseCode);
            Assert.AreEqual(Constants.HTTP_GET_200, response.RequestUri.AbsoluteUri);
            // assert the parameters
            MessageData md = new MessageData();
            md.QueryParameters.Add(new NameValuePair("test", "1 and text"));
            md.QueryParameters.Add(new NameValuePair("test2", "2 and text <> &&"));
            string responseString = EntityUtils.ToString(response.Entity);
            Assert.AreEqual(md.ToString(), responseString);
            Console.Write(responseString);
        }

        [Test]
        public void HttpGetWithProxy()
        {
            HttpClient client = new HttpClient();
            client.Proxy = new Uri(Constants.FIDDLER_DEFAULT_ADDRESS);
            HttpGet getMethod = new HttpGet(new Uri(Constants.HTTP_GET_200));
            HttpResponse response = client.Execute(getMethod);

            Assert.AreEqual(200, response.ResponseCode);
            string responseString = EntityUtils.ToString(response.Entity);
            Assert.AreEqual(MessageData.Empty.ToString(), responseString);
            Assert.AreEqual(Constants.HTTP_GET_200, response.RequestUri.AbsoluteUri);
            Console.Write(responseString);
        }

        [Test]
        public void HttpGetWithCookies()
        {
            // with SetMaxRedirects
            // make sure we are not redirected
            HttpClient client = new HttpClient();
            client.MaxRedirects = 0;
            HttpGet getMethod = new HttpGet(new Uri(Constants.HTTP_GET_200_WITH_SET_COOKIES));
            getMethod.Parameters.Add("cookie1", "value1");
            getMethod.Parameters.Add("cookie2", "value2");
            HttpResponse response = client.Execute(getMethod);

            Assert.AreEqual(200, response.ResponseCode);
            Assert.AreEqual(Constants.HTTP_GET_200_WITH_SET_COOKIES, response.RequestUri.AbsoluteUri);

            getMethod = new HttpGet(new Uri(Constants.HTTP_GET_200));
            response = client.Execute(getMethod);

            Assert.AreEqual(200, response.ResponseCode);
            Assert.AreEqual(Constants.HTTP_GET_200, response.RequestUri.AbsoluteUri);

            // assert the cookies
            MessageData md = new MessageData();
            md.Cookies.Add(new NameValuePair("cookie1", "value1"));
            md.Cookies.Add(new NameValuePair("cookie2", "value2"));
            string responseString = EntityUtils.ToString(response.Entity);
            Assert.AreEqual(md.ToString(), responseString);
            Console.Write(responseString);            
        }

        [Test]
        public void HttpGetWithChunkedResponse()
        {
            // we use codescales.com that is hosted by google
            // the google server always uses chunked transfer-encoding

            HttpClient client = new HttpClient();
            HttpGet getMethod = new HttpGet(new Uri(Constants.HTTP_GET_200_CODESCALES_COM));
            HttpResponse response = client.Execute(getMethod);

            Assert.AreEqual(200, response.ResponseCode);
            string responseString = EntityUtils.ToString(response.Entity);
            Assert.AreEqual(Constants.HTTP_GET_200_CODESCALES_COM, response.RequestUri.AbsoluteUri);
            Console.Write(responseString);
        }

        [Test]
        public void HttpGetWithChunkedResponse2()
        {
            // we use codescales.com that is hosted by google
            // the google server always uses chunked transfer-encoding

            HttpClient client = new HttpClient();
            HttpGet getMethod = new HttpGet(new Uri("http://forums.photographyreview.com/showthread.php?t=68825"));
            HttpResponse response = client.Execute(getMethod);

            Assert.AreEqual(200, response.ResponseCode);
            string responseString = EntityUtils.ToString(response.Entity);
            Assert.AreEqual("http://forums.photographyreview.com/showthread.php?t=68825", response.RequestUri.AbsoluteUri);
            Console.Write(responseString);
        }

    }
}
