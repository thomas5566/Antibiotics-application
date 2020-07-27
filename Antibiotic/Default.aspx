<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" ValidateRequest="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <style type="text/css">
        .auto-style2 {
            width: 50%;
        }
    </style>
</head>
<body>   
       <form id="form2" runat="server" defaultbutton="SearchBt">     
    <div>
        <table border=1 cellpadding=5 cellspacing=0 class="auto-style2">
            <tr>
                <td width=30% align=center><asp:LinkButton ID="LinkButton1" runat="server" PostBackUrl="~/Antibiotic/Default2.aspx">新增管制性抗生素申請表</asp:LinkButton></td>
                <td width=30% align=center><asp:LinkButton ID="LinkButton2" runat="server" ForeColor="Red" OnClick="LinkButton1_Click" PostBackUrl="~/Antibiotic/LoginPage.aspx" Width="43px">登入</asp:LinkButton></td>
            </tr>
        </table>        
         Search:
        <asp:TextBox ID="txtSearch" runat="server" Width="239px" />
        <asp:Button ID="SearchBt" Text="Search" runat="server" OnClick="Search" UseSubmitBehavior="true" />
        <asp:Label ID="Label1" runat="server" ForeColor="Red" Visible="false" ></asp:Label><br />        
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
             <asp:GridView ID="GridView2" runat="server" 
                 AutoGenerateColumns="false" 
                 PageSize="10"
                 DataKeyNames ="P_ID"
                 OnRowCommand="grdAttachment_RowCommand"
                 AllowPaging="True"
                 CallbackMode="true" 
                 Serialize="true"
                 Width="800px" 
                 OnPageIndexChanging="GridView2_PageIndexChanging"
                 OnRowDataBound="OnRowDataBound2"
                 
                 >
                <Columns>
                <asp:BoundField DataField="P_ID" HeaderText="病歷號碼"/>
                <asp:BoundField DataField="P_Name" HeaderText="病患姓名" />
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:Button ID="btnViewDetails" runat="server" OnClick="btnViewDetails_Click" Text="ViewDetails" />
                    </ItemTemplate>
                </asp:TemplateField>
                    <asp:BoundField DataField="PDF_CreTime" HeaderText="上傳時間"  />
                    <asp:TemplateField HeaderText="Document">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkFile" runat="server" CausesValidation="false" CommandArgument='<%# Eval("PDF_Path") %>' CommandName="ViewFile" Text='<%# Eval("PDF_Path") %>'></asp:LinkButton>
                    </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="刪除"  >
                        <ItemTemplate>
                            <asp:Button ID="lnkDelete" runat="server" Text="Delete" OnClick="DeleteFile"
                                CommandArgument='<%# Eval("P_ID") %>'></asp:Button>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="驗證"  >
                        <ItemTemplate>                            
                            <asp:Label ID="Label6" runat="server" Text='<%#Eval("Certification")%>' Visible="false" ></asp:Label >
                         </ItemTemplate>                          
                    </asp:TemplateField>
                </Columns>
                <HeaderStyle BackColor="#4279bd" Font-Bold="True" ForeColor="#E7E7FF" />
            </asp:GridView>
           
             </ContentTemplate>
        </asp:UpdatePanel>
        
        </div>
        </form>
</body>
</html>
