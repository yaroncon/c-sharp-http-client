using System;
using System.Collections.Generic;
using System.Text;

namespace CodeScales.Http.Protocol
{
    public class HTTP
    {
         /** HTTP header definitions */ 
    public  const string TRANSFER_ENCODING = "Transfer-Encoding";
    public  const string CONTENT_LEN  = "Content-Length";
    public  const string CONTENT_TYPE = "Content-Type";
    public  const string CONTENT_ENCODING = "Content-Encoding";
    public  const string EXPECT_DIRECTIVE = "Expect";
    public  const string CONN_DIRECTIVE = "Connection";
    public  const string TARGET_HOST = "Host";
    public  const string USER_AGENT = "User-Agent";
    public  const string DATE_HEADER = "Date";
    public  const string SERVER_HEADER = "Server";
    
    /** HTTP expectations */
    public  const string EXPECT_CONTINUE = "100-Continue";

    /** HTTP connection control */
    public  const string CONN_CLOSE = "Close";
    public  const string CONN_KEEP_ALIVE = "Keep-Alive";
    
    /** Transfer encoding definitions */
    public  const string CHUNK_CODING = "chunked";
    public  const string IDENTITY_CODING = "identity";
    }
}
