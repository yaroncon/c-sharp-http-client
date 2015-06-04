# c-sharp-http-client
Automatically exported from code.google.com/p/c-sharp-http-client

CSharp HttpClient by CodeScales.com

An HTTP client library with a very simple API, written in CSharp (.Net 2.0) for sending HTTP requests and receiving HTTP responses.
This library is much simpler to use than the HttpWebRequest provided with the .Net library.
This library was inspired by the JAVA HttpClient library, and has a very similar API.
See codescales.com for more details
You are encouraged to visit the project's web site at http://www.codescales.com for code examples and documentation.

What can you do with it?

Send GET requests.
Send POST requests with parameters, using Url-Encoded Entity.
upload Files to an HTTP server, using a Multipart Entity.
Send Post request with parameters, using Multipart Entity (which is actually the same as #3).
Control the redirect behavior of the client, using HttpBehavior.
Use as many parallel connections (sockets) as you need, using the default HttpConnectionFactory.
Use a proxy server that does not need a password.
Current Status

This is a preliminary version. Some basic functionality is missing. See the limitations section for details. It is still work in progress, and you are welcome to write me if you want to help at yaron@codescales.com.

Limitations

This is a list of things that are NOT supported yet:

Transfer-Encoding: gzip
Requests with keep-alive header.
Expiration of cookies.
Using proxies that need password.
HTTPS and SSL.
