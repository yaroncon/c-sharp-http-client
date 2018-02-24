using System;
using System.Collections.Generic;
using System.Text;

using CodeScales.Http;
using CodeScales.Http.Methods;
using CodeScales.Http.Entity;
using CodeScales.Http.Common;

namespace CodeScales.Http.Examples
{
    public class HttpPostExamples
    {
        public static void DoPost()
        {
            HttpClient client = new HttpClient();
            HttpPost postMethod = new HttpPost(new Uri("http://www.w3schools.com/asp/demo_simpleform.asp"));

            List<NameValuePair> nameValuePairList = new List<NameValuePair>();
            nameValuePairList.Add(new NameValuePair("fname", "brian"));
            
            UrlEncodedFormEntity formEntity = new UrlEncodedFormEntity(nameValuePairList, Encoding.UTF8);
            postMethod.Entity = formEntity;
            
            HttpResponse response = client.Execute(postMethod);

            Console.WriteLine("Response Code: " + response.ResponseCode);
            Console.WriteLine("Response Content: " + EntityUtils.ToString(response.Entity));
        }
    }
}
