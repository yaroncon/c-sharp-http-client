using System;
using System.Collections.Generic;
using System.Text;

using CodeScales.Http;
using CodeScales.Http.Entity;
using CodeScales.Http.Methods;

namespace CodeScales.Http.Examples
{
    public class HttpGetExample
    {
        public static void DoGet()
        {
            HttpClient httpClient = new HttpClient();
            HttpGet httpGet = new HttpGet(new Uri("http://www.codescales.com"));
            HttpResponse httpResponse = httpClient.Execute(httpGet);

            Console.WriteLine("Response Code: " + httpResponse.ResponseCode);
            Console.WriteLine("Response Content: " + EntityUtils.ToString(httpResponse.Entity));
        }

        public static void DoGetWithRedirects()
        {
            HttpClient httpClient = new HttpClient();
            httpClient.MaxRedirects = 0;
            HttpGet httpGet = new HttpGet(new Uri("http://www.codescales.com/home"));
            HttpResponse httpResponse = httpClient.Execute(httpGet);

            Console.WriteLine("Response Code: " + httpResponse.ResponseCode);
            Console.WriteLine("Response Code: " + httpResponse.Location);
            Console.WriteLine("Response Content: " + EntityUtils.ToString(httpResponse.Entity));
        }

        public static void DoGetWithRedirects2()
        {
            HttpClient httpClient = new HttpClient();
            HttpGet httpGet = new HttpGet(new Uri("http://www.codescales.com/home"));
            
            HttpBehavior httpBehavior = new HttpBehavior();
            httpBehavior.AddStep(301, "http://www.codescales.com");
            httpBehavior.AddStep(200);
            
            HttpResponse httpResponse = httpClient.Execute(httpGet, httpBehavior);

            Console.WriteLine("Response Code: " + httpResponse.ResponseCode);
            Console.WriteLine("Response Code: " + httpResponse.Location);
            Console.WriteLine("Response Content: " + EntityUtils.ToString(httpResponse.Entity));
        }

        public static void DoGetWithProxy()
        {
            HttpClient httpClient = new HttpClient();
            httpClient.Proxy = new Uri("http://localhost:8888/"); // default address of fiddler
            HttpGet httpGet = new HttpGet(new Uri("http://www.codescales.com"));
            HttpResponse httpResponse = httpClient.Execute(httpGet);

            Console.WriteLine("Response Code: " + httpResponse.ResponseCode);
            Console.WriteLine("Response Content: " + EntityUtils.ToString(httpResponse.Entity));
        }
    }
}
