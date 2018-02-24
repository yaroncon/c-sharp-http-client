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
using System.IO;

using CodeScales.Http;
using CodeScales.Http.Common;
using CodeScales.Http.Methods;
using CodeScales.Http.Entity;
using CodeScales.Http.Entity.Mime;
using NUnit.Framework;

using CodeScales.Http.Tests.resources;

namespace CodeScales.Http.Tests
{
    [TestFixture]
    public class HttpPostTests
    {
        [Test]
        public void HttpPost()
        {
            HttpClient client = new HttpClient();
            HttpPost postMethod = new HttpPost(new Uri(Constants.HTTP_POST_200));
            List<NameValuePair> nameValuePairList = new List<NameValuePair>();
            nameValuePairList.Add(new NameValuePair("param1","value1"));
            nameValuePairList.Add(new NameValuePair("param2","!#$^&*((<>"));
            UrlEncodedFormEntity formEntity = new UrlEncodedFormEntity(nameValuePairList, Encoding.UTF8);
            postMethod.Entity = formEntity;
            HttpResponse response = client.Execute(postMethod);

            Assert.AreEqual(200, response.ResponseCode);
            string responseString = EntityUtils.ToString(response.Entity);
            MessageData md = new MessageData();
            md.PostParameters.Add(new NameValuePair("param1", "value1"));
            md.PostParameters.Add(new NameValuePair("param2", "!#$^&*((<>"));
            Assert.AreEqual(md.ToString(), responseString);
            Assert.AreEqual(Constants.HTTP_POST_200, response.RequestUri.AbsoluteUri);
            Console.Write(responseString);

        }

        [Test]
        public void HttpPostWithParameters2()
        {
            HttpClient client = new HttpClient();

            // first request - to get cookies
            HttpGet getMethod = new HttpGet(new Uri(Constants.HTTP_POST_W3SCHOOLS));
            HttpResponse response = client.Execute(getMethod);
            Assert.AreEqual(200, response.ResponseCode);

            // second request - to send form data
            HttpPost postMethod = new HttpPost(new Uri(Constants.HTTP_POST_W3SCHOOLS));
            List<NameValuePair> nameValuePairList = new List<NameValuePair>();
            nameValuePairList.Add(new NameValuePair("fname", "user 1"));
            UrlEncodedFormEntity formEntity = new UrlEncodedFormEntity(nameValuePairList, Encoding.UTF8);
            postMethod.Entity = formEntity;
            response = client.Execute(postMethod);

            Assert.AreEqual(200, response.ResponseCode);
            string responseString = EntityUtils.ToString(response.Entity);
            Assert.IsTrue(responseString.Contains("Hello user 1!"));

        }

        [Test]
        public void HttpPostWithFilesAndParameters()
        {
            // this method sends an empty file (or no file :)
            HttpClient client = new HttpClient();
            HttpPost postMethod = new HttpPost(new Uri(Constants.HTTP_MULTIPART_POST_200));

            MultipartEntity multipartEntity = new MultipartEntity();
            StringBody stringBody1 = new StringBody(Encoding.ASCII, "param1", "value1");
            multipartEntity.AddBody(stringBody1);
            StringBody stringBody2 = new StringBody(Encoding.ASCII, "param2", "!#$^&*((<>");
            multipartEntity.AddBody(stringBody2);
            FileBody fileBody1 = new FileBody("photo", "me.file", null);
            multipartEntity.AddBody(fileBody1);
            postMethod.Entity = multipartEntity;
            HttpResponse response = client.Execute(postMethod);

            Assert.AreEqual(200, response.ResponseCode);
            string responseString = EntityUtils.ToString(response.Entity);
            MessageData md = new MessageData();
            md.PostParameters.Add(new NameValuePair("param1", "value1"));
            md.PostParameters.Add(new NameValuePair("param2", "!#$^&*((<>"));
            md.Files.Add(new NameValuePair("me.file", "0"));
            Assert.AreEqual(md.ToString(), responseString);
            Assert.AreEqual(Constants.HTTP_MULTIPART_POST_200, response.RequestUri.AbsoluteUri);
            Console.Write(responseString);
        }

        [Test]
        public void HttpPostWithFilesAndParameters2()
        {
            // this method sends a file and checks for the number of bytes recieved
            HttpClient client = new HttpClient();
            HttpPost postMethod = new HttpPost(new Uri(Constants.HTTP_MULTIPART_POST_200));

            MultipartEntity multipartEntity = new MultipartEntity();

            string fileName = "big-text.txt";

            FileInfo fi = ResourceManager.GetResourceFileInfo(fileName);
            FileBody fileBody1 = new FileBody("file", fileName, fi, "text/plain");

            multipartEntity.AddBody(fileBody1);
            postMethod.Entity = multipartEntity;

            StringBody stringBody1 = new StringBody(Encoding.ASCII, "param1", "value1");
            multipartEntity.AddBody(stringBody1);
            StringBody stringBody2 = new StringBody(Encoding.ASCII, "param2", "!#$^&*((<>");
            multipartEntity.AddBody(stringBody2);

            HttpResponse response = client.Execute(postMethod);

            Assert.AreEqual(200, response.ResponseCode);
            string responseString = EntityUtils.ToString(response.Entity);
            MessageData md = new MessageData();
            md.PostParameters.Add(new NameValuePair("param1", "value1"));
            md.PostParameters.Add(new NameValuePair("param2", "!#$^&*((<>"));
            md.Files.Add(new NameValuePair(fileName, fi.Length.ToString()));
            Assert.AreEqual(md.ToString(), responseString);
            Assert.AreEqual(Constants.HTTP_MULTIPART_POST_200, response.RequestUri.AbsoluteUri);
            Console.Write(responseString);
        }

        [Test]
        public void HttpPostWithFilesAndParameters3()
        {
            // this method sends a file and checks for the number of bytes recieved
            HttpClient client = new HttpClient();
            string url = "http://www.fiddler2.com/sandbox/FileForm.asp";
            HttpPost postMethod = new HttpPost(new Uri(url));

            MultipartEntity multipartEntity = new MultipartEntity();
            postMethod.Entity = multipartEntity;

            string fileName = "small-text.txt";

            StringBody stringBody1 = new StringBody(Encoding.ASCII, "1", "1_");
            multipartEntity.AddBody(stringBody1);

            FileInfo fi = ResourceManager.GetResourceFileInfo(fileName);
            FileBody fileBody1 = new FileBody("fileentry", fileName, fi, "text/plain");
            multipartEntity.AddBody(fileBody1);

            StringBody stringBody2 = new StringBody(Encoding.ASCII, "_charset_", "windows-1252");
            multipartEntity.AddBody(stringBody2);

            HttpResponse response = client.Execute(postMethod);

            Assert.AreEqual(200, response.ResponseCode);
            string responseString = EntityUtils.ToString(response.Entity);
            Assert.AreEqual(url, response.RequestUri.AbsoluteUri);
            Console.Write(responseString);
        }
    }
}
