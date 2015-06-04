# CSharp HttpClient by `CodeScales.com` #

An **HTTP client library** with a very simple API, written in CSharp (.Net 2.0) for sending HTTP requests and receiving HTTP responses. <br />
This library is much simpler to use than the HttpWebRequest provided with the .Net library. <br />
This library was inspired by the JAVA HttpClient library, and has a very similar API. <br />

## You are encouraged to visit the project's web site at http://www.codescales.com for code examples and documentation. ##

### What can you do with it? ###
  1. Send GET requests.
  1. Send POST requests with parameters, using Url-Encoded Entity.
  1. Upload Files to an HTTP server, using a Multipart Entity.
  1. Send Post request with parameters, using Multipart Entity (which is actually the same as #3).
  1. Control the redirect behavior of the client, using HttpBehavior.
  1. Use as many parallel connections (sockets) as you need, using the default HttpConnectionFactory.
  1. Use a proxy server that does not need a password.

### Current Status ###
This is a preliminary version. Some basic functionality is missing. See the limitations section for details. <br />
It is still work in progress, and you are welcome to write me if you want to help at [yaron@codescales.com](mailto:yaron@codescales.com). <br />

### Limitations ###
This is a list of things that are NOT supported yet:
  * Transfer-Encoding: gzip
  * Requests with keep-alive header.
  * Expiration of cookies.
  * Using proxies that need password.
  * HTTPS and SSL.