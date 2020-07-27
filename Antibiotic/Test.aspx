<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Test.aspx.cs" Inherits="Antibiotic_Test" %>

<!DOCTYPE html>

<html  
    xmlns="http://www.w3.org/1999/xhtml">  
    <head runat="server">  
        <title></title>  
    </head>  
    <body bgcolor="#ffff99">  
        <form id="form1" runat="server">  
            <div>  
                <p>  
                    <strong style="font-size: xx-large">Hello Everyone! Welcome to my Page.  
  
</strong>  
                </p>  
            </div>  
            <asp:Image ID="Image1" runat="server" Height="335px"   
ImageUrl="~/2.jpg" Width="817px" />  
            <p>  
 </p>  
            <p>  
                <asp:Label ID="Label1" runat="server"></asp:Label>  
            </p>  
            <p>  
                <asp:Button ID="Button1" runat="server" Height="47px" onclick="Button1_Click"   
Text="Logout" Width="92px" />  
            </p>  
        </form>  
    </body>  
</html>
