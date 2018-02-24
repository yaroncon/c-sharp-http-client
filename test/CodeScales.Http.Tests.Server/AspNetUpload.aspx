<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AspNetUpload.aspx.cs" Inherits="CodeScales.Http.Tests.Server.AspNetUpload" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server" enctype="multipart/form-data">
    <div>
        <asp:FileUpload ID="FileUpload1" runat="server" /><br />
        <asp:Button ID="Submit1" runat="server"
            Text="Submit" />
    </div>
    </form>
</body>
</html>
