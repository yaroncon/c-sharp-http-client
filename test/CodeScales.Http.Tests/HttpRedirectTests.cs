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
using CodeScales.Http.Methods;
using CodeScales.Http.Entity;
using NUnit.Framework;

namespace CodeScales.Http.Tests
{
    [TestFixture]
    public class HttpRedirectTests
    {
        [Test]
        public void HttpGetWithRedirect()
        {
            // with SetMaxRedirects
            // make sure we are not redirected
            HttpClient client = new HttpClient();
            client.MaxRedirects = 0;
            HttpGet getMethod = new HttpGet(new Uri(Constants.HTTP_GET_302));
            HttpResponse response = client.Execute(getMethod);

            Assert.AreEqual(302, response.ResponseCode);
            Console.Write(EntityUtils.ToString(response.Entity));

            getMethod = new HttpGet(new Uri(response.Location));
            response = client.Execute(getMethod);

            Assert.AreEqual(200, response.ResponseCode);
            Assert.AreEqual(Constants.HTTP_REDIRECT_TARGET_1, response.RequestUri.AbsoluteUri);
            string responseString = EntityUtils.ToString(response.Entity);
            Console.Write(responseString);

        }

        [Test]
        public void HttpGetWithRedirect2()
        {
            // with SetMaxRedirects
            // make sure we were redirected
            HttpClient client = new HttpClient();
            client.MaxRedirects = 1;
            HttpGet getMethod = new HttpGet(new Uri(Constants.HTTP_GET_302));
            HttpResponse response = client.Execute(getMethod);

            Assert.AreEqual(200, response.ResponseCode);
            Assert.AreEqual(Constants.HTTP_REDIRECT_TARGET_1, response.RequestUri.AbsoluteUri);
            string responseString = EntityUtils.ToString(response.Entity);
            Console.Write(responseString);

        }

        [Test]
        public void HttpGetWithRedirect3()
        {
            // this time with HTTPBehavior
            // make sure we were redirected and no exception is thrown
            HttpClient client = new HttpClient();
            HttpBehavior httpBehavior = new HttpBehavior();
            httpBehavior.AddStep(302, Constants.HTTP_REDIRECT_TARGET_1);
            httpBehavior.AddStep(200);
            HttpGet getMethod = new HttpGet(new Uri(Constants.HTTP_GET_302));
            HttpResponse response = client.Execute(getMethod, httpBehavior);

            Assert.AreEqual(200, response.ResponseCode);
            Assert.AreEqual(Constants.HTTP_REDIRECT_TARGET_1, response.RequestUri.AbsoluteUri);
            string responseString = EntityUtils.ToString(response.Entity);
            Console.Write(responseString);
        }

        [Test]
        //[ExpectedException(typeof(HttpBehaviorException))]
        public void HttpGetWithRedirect4()
        {
            // this time with HTTPBehavior
            // here both response code and location are wrong
            // make sure an exception is thrown
            Assert.Catch<HttpBehaviorException>(() =>
            {
                HttpClient client = new HttpClient();
                HttpBehavior httpBehavior = new HttpBehavior();
                httpBehavior.AddStep(200);
                HttpGet getMethod = new HttpGet(new Uri(Constants.HTTP_GET_302));
                HttpResponse response = client.Execute(getMethod, httpBehavior);
            });
        }

        [Test]
        //[ExpectedException(typeof(HttpBehaviorException))]
        public void HttpGetWithRedirect5()
        {
            // this time with HTTPBehavior
            // here the response code is wrong
            // make sure an exception is thrown
            Assert.Catch<HttpBehaviorException>(() =>
            {
                HttpClient client = new HttpClient();
                HttpBehavior httpBehavior = new HttpBehavior();
                httpBehavior.AddStep(200, Constants.HTTP_GET_302);
                HttpGet getMethod = new HttpGet(new Uri(Constants.HTTP_GET_302));
                HttpResponse response = client.Execute(getMethod, httpBehavior);
            });
        }

        [Test]
        //[ExpectedException(typeof(HttpBehaviorException))]
        public void HttpGetWithRedirect6()
        {
            // this time with HTTPBehavior
            // here the response location is wrong
            // make sure an exception is thrown
            Assert.Catch<HttpBehaviorException>(() =>
            {
                HttpClient client = new HttpClient();
                HttpBehavior httpBehavior = new HttpBehavior();
                httpBehavior.AddStep(302, Constants.HTTP_GET_200);
                HttpGet getMethod = new HttpGet(new Uri(Constants.HTTP_GET_302));
                HttpResponse response = client.Execute(getMethod, httpBehavior);
            });
        }

        [Test]
        public void HttpGetWithRedirect7()
        {
            // this time with HTTPBehavior
            // make sure we get only the first response
            // and do not make the second call
            HttpClient client = new HttpClient();
            HttpBehavior httpBehavior = new HttpBehavior();
            httpBehavior.AddStep(302, Constants.HTTP_REDIRECT_TARGET_1);
            HttpGet getMethod = new HttpGet(new Uri(Constants.HTTP_GET_302));
            HttpResponse response = client.Execute(getMethod, httpBehavior);

            Assert.AreEqual(302, response.ResponseCode);
            Assert.AreEqual(Constants.HTTP_REDIRECT_TARGET_1, response.Location);
        }

    }
}
