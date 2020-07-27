<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LoginPage.aspx.cs" Inherits="Antibiotic_LoginPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <style type="text/css">
        .auto-style1 {
            width: 79px;
        }
        .auto-style2 {
            width: 157px;
        }
        .auto-style3 {
            width: 48%;
        }
        .auto-style4 {
            width: 86px;
        }
    </style>
</head>
<body style="width: 802px">
    <form id="form1" runat="server">  
        <div >  
            <table class="auto-style3">  
                <caption class="style1">  
                    <strong>登入頁面</strong>  
                </caption>  
                <tr>  
                    <td class="auto-style1">  
 </td>  
                    <td class="auto-style2">  
 </td>  
                    <td class="auto-style4">  
 </td>  
                </tr>  
                <tr>  
                    <td class="auto-style1">  
帳號:</td>  
                    <td class="auto-style2">  
                        <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>  
                    </td>  
                    <td class="auto-style4">  
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="TextBox1" ErrorMessage="請輸入帳號" ForeColor="Red"></asp:RequiredFieldValidator>  
                    </td>  
                </tr>  
                <tr>  
                    <td class="auto-style1">  
                        密碼:</td>  
                    <td class="auto-style2">  
                        <asp:TextBox ID="TextBox2" TextMode="Password" runat="server"></asp:TextBox>  
                    </td>  
                    <td class="auto-style4">  
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="TextBox2" ErrorMessage="請輸入密碼" ForeColor="Red"></asp:RequiredFieldValidator>  
                    </td>  
                </tr>  
                <tr>  
                    <td class="auto-style1">  
 </td>  
                    <td class="auto-style2">  
 </td>  
                    <td class="auto-style4">  
 </td>  
                </tr>  
                <tr>  
                    <td class="auto-style1">  
 </td>  
                    <td class="auto-style2">  
                        <asp:Button ID="Button1" runat="server" Text="登入" onclick="Button1_Click" />  
                    </td>  
                    <td class="auto-style4">  
                        <asp:Label ID="Label1" runat="server"></asp:Label>  
                    </td>  
                </tr>  
            </table>  
        </div>  
    </form>  
</body>
</html>
