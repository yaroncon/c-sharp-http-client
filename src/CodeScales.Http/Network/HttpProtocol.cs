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
using System.Web;

using CodeScales.Http.Cookies;
using CodeScales.Http.Common;

namespace CodeScales.Http.Network
{
    internal class HTTPProtocol
    {
        private const string VIEWSTATEStart = "<input type=\"hidden\" name=\"__VIEWSTATE\" id=\"__VIEWSTATE\" value=\"";

        public static Dictionary<string, string> ExtractUrlParameters(string url)
        {
            if (url.IndexOf('?') < 0)
                return null;
            string query = url.Substring(url.IndexOf('?') + 1);
            if (query == null)
                return null;
            string[] parameters = query.Split('&');
            if (parameters.Length <= 0)
                return null;
            Dictionary<string, string> dic = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
            foreach (string param in parameters)
            {
                string[] namevalue = param.Split('=');
                if (namevalue != null && namevalue.Length == 2)
                {
                    dic.Add(namevalue[0], namevalue[1]);
                }
            }
            return dic;
        }

        public static string GetViewState(string html)
        {
            string viewState = "";
            if (html.IndexOf("__VIEWSTATE") > 0)
            {
                int start = html.IndexOf(VIEWSTATEStart) + VIEWSTATEStart.Length;
                int end = html.IndexOf("\"", start);
                viewState = html.Substring(start, end - start);
            }
            return viewState;
        }

        internal static void AddPostParameters(List<NameValuePair> parameters, StringBuilder builder)
        {
            int counter = 0;
            foreach (NameValuePair pair in parameters)
            {
                builder.Append(pair.Name + "=" + HttpUtility.UrlEncode(pair.Value));
                if (counter < parameters.Count - 1)
                {
                    builder.Append("&");
                }
                counter++;
            }
        }

        internal static string GetPostParameter(string name, string value, string boundry)
        {
            StringBuilder builder = new StringBuilder();
            string paramBoundry = "--" + boundry + "\r\n";
            string stringParam = "Content-Disposition: form-data; name=\"";
            string paramEnd = "\"\r\n\r\n";
            builder.Append(paramBoundry);
            builder.Append(stringParam + name + paramEnd + value + "\r\n");
            return builder.ToString();
        }

        internal static string AddPostParametersFile(string name, string fileName, string boundry, string contentType)
        {
            if (name == null)
            {
                name = string.Empty;
            }
            if (fileName == null)
            {
                fileName = string.Empty;
            }

            StringBuilder builder = new StringBuilder();
            string paramBoundry = "--" + boundry + "\r\n";
            string stringParam = "Content-Disposition: form-data; name=\"";
            string paramEnd = "\"; filename=\"" + fileName + "\"\r\nContent-Type: " + contentType + "\r\n\r\n";
            builder.Append(paramBoundry);
            builder.Append(stringParam + name + paramEnd);
            return builder.ToString();
        }

        internal static string AddPostParametersEnd(string boundry)
        {
            return "--" + boundry + "--\r\n\r\n";            
        }

        public static Encoding GetEncoding(string contentType)
        {
            Encoding encoding = null;
            string enStr = GetCharacterSet(contentType);
            if (enStr != null && enStr != string.Empty)
            {
                try
                {
                    encoding = Encoding.GetEncoding(enStr);
                }
                catch
                {
                    encoding = Encoding.UTF8; //default
                }
            }
            else
            {
                encoding = Encoding.UTF8; //default
            }
            return encoding;
        }

        private static string GetCharacterSet(string s)
        {
            s = s.ToUpper();
            int start = s.LastIndexOf("CHARSET");
            if (start == -1)
                return "";


            start = s.IndexOf("=", start);
            if (start == -1)
                return "";


            start++;
            s = s.Substring(start).Trim();
            int end = s.Length;


            int i = s.IndexOf(";");
            if (i != -1)
                end = i;
            i = s.IndexOf("\"");
            if (i != -1 && i < end)
                end = i;
            i = s.IndexOf("'");
            if (i != -1 && i < end)
                end = i;
            i = s.IndexOf("/");
            if (i != -1 && i < end)
                end = i;


            return s.Substring(0, end).Trim();
        }

        public static string GetCookiesHeader(CodeScales.Http.Cookies.HttpCookieCollection cookies)
        {
            if (cookies != null && cookies.Count > 0)
            {
                int cookieCounter = 0;
                StringBuilder sb = new StringBuilder();
                // sb.Append("Cookie: ");
                foreach (string key in cookies.Keys)
                {
                    sb.Append(key + "=" + cookies.GetCookie(key).Value);
                    if (cookieCounter < cookies.Count - 1)
                    {
                        sb.Append(';');
                    }
                    cookieCounter++;
                }
                return sb.ToString(); // +"\r\n";
            }
            return null;
        }

        public static void AddHttpCookie(string headerPart, CodeScales.Http.Cookies.HttpCookieCollection cookies)
        {
            if (headerPart == null || headerPart == "" || cookies == null)
                return;

            string[] cookie_components = headerPart.Split(';');

            if (cookie_components != null && cookie_components.Length > 0)
            {
                string kv = cookie_components[0];
                int pos = kv.IndexOf('=');
                if (pos == -1)
                {
                    /* XXX ugh */

                }
                else
                {
                    string key = kv.Substring(0, pos).Trim();
                    string val = kv.Substring(pos + 1).Trim();

                    cookies.Add(key, new CodeScales.Http.Cookies.HttpCookie(key, val));
                }
            }
        }
    }
}

