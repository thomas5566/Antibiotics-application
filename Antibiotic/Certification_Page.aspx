<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Certification_Page.aspx.cs" Inherits="Antibiotic_Certification_Page" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
  <link rel="stylesheet" href="//code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css"/>
  <link rel="stylesheet" href="/resources/demos/style.css"/>
<script src="https://code.jquery.com/jquery-1.12.4.js"></script>
<script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>
<script src="http://code.jquery.com/jquery-1.10.1.min.js"></script>  
<script src="Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
<script src="Scripts/jquery.dynDateTime.min.js" type="text/javascript"></script>
<script src="Scripts/calendar-en.min.js" type="text/javascript"></script>
<link href="Styles/calendar-blue.css" rel="stylesheet" type="text/css" />
<script type = "text/javascript">
    $(document).ready(function () {
        $(".Calender").dynDateTime({
            showsTime: false,
            ifFormat: "%Y/%m/%d",
            daFormat: "%l;%M %p, %e %m,  %Y",
            align: "BR",
            electric: false,
            singleClick: true,
            displayArea: ".siblings('.dtcDisplayArea')",
            button: ".next()"
        });
    });
</script>

    <style type="text/css">
        .auto-style4 {
            height: 26px;
        }
        .auto-style5 {
            height: 26px;
            width: 1018px;
        }
        </style>
</head>
<body>
    <form id="form1" runat="server">

        <div>
            
            <div class="auto-style5">
                <asp:Label ID="UserLabel" runat="server" ForeColor="Red" ></asp:Label>
            </div>
               <div class="clear hideSkiplink" style=" padding: 5px 5px 5px 5px;">
                  <asp:LinkButton ID="LinkButton1" runat="server" OnClick="LinkButton1_Click" >查詢頁面</asp:LinkButton>
                </div>
            <table  class="auto-style20" >
                <tr style="border:3px #cccccc solid;">
                    <td class="auto-style31" bgcolor="#4279bd" style=" color:White;" aria-multiline="True">姓名:</td>
                    <td class="auto-style29" aria-multiline="True">
                        <asp:TextBox ID="TextBox2" runat="server"  ></asp:TextBox>
                    </td>
                    <td class="auto-style30"  bgcolor="#4279bd" style=" color:White;" aria-multiline="True">性別:</td>
                    <td class="auto-style32" aria-multiline="True">
                        男<asp:CheckBox ID="ManCheckBox" runat="server" />
                        女<asp:CheckBox ID="GirlCheckBox" runat="server" />
                    </td>
                    <td class="auto-style47"  bgcolor="#4279bd" style=" color:White;" aria-multiline="True">年齡:</td>
                    <td class="auto-style4" aria-multiline="True">
                        <asp:TextBox ID="TextBox30" runat="server"></asp:TextBox>
                    </td>
                    <td class="auto-style47"  bgcolor="#4279bd" style=" color:White;" aria-multiline="True">病歷號碼:</td>
                    <td class="auto-style4" aria-multiline="True">
                        <asp:Label ID="TextBox1" runat="server" BackColor="#DEDFDE" ForeColor="Red" ></asp:Label>
                    </td>
                    <td class="auto-style41"  bgcolor="#4279bd" style=" color:White;" aria-multiline="True">床號:</td>
                    <td class="auto-style27" aria-multiline="True">
                        <asp:TextBox ID="TextBox3" runat="server"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <table class="auto-style35" >
                <tr>
                    <td class="auto-style36" bgcolor="#4279bd" style=" color:White;">主治醫師:<br />
                    </td>
                    <td class="auto-style33">
                        <asp:TextBox ID="TextBox4" runat="server" Height="19px"></asp:TextBox>
                    </td>
                    <td class="auto-style58"  bgcolor="#4279bd" style=" color:White;">申請醫師<br />
                        簽名、蓋章</td>
                    <td class="auto-style40">
                        <asp:TextBox ID="TextBox5" runat="server"></asp:TextBox>
                    </td>
                    <td class="auto-style42"  bgcolor="#4279bd" style=" color:White;">藥師:</td>
                    <td class="auto-style39">
                        <asp:TextBox ID="TextBox6" runat="server"></asp:TextBox>
                    </td>
                </tr>
              </table>
            <table class="auto-style35" >
                <tr>
                    <td class="auto-style59" bgcolor="#4279bd" style=" color:White;">相關數值:</td>
                    <td class="auto-style45">體重:</td>
                    <td class="auto-style44">
                        <asp:TextBox ID="TextBox7" runat="server"></asp:TextBox>
                        公斤</td>
                     <td class="auto-style45">Creatinine:</td>
                    <td class="auto-style46">
                        <asp:TextBox ID="TextBox8" runat="server"></asp:TextBox>
                        mg/dL</td>
                    <td class="auto-style45">BUN:</td>
                    <td class="auto-style39">
                        <asp:TextBox ID="TextBox9" runat="server"></asp:TextBox>
                        mg/dL</td>
                    <td class="auto-style45">GOT:</td>
                    <td class="auto-style39">
                        <asp:TextBox ID="TextBox10" runat="server"></asp:TextBox>
                        U/I</td>
                    <td class="auto-style45">GPT:</td>
                    <td class="auto-style39">
                        <asp:TextBox ID="TextBox11" runat="server"></asp:TextBox>
                        U/L</td>
                </tr>                
            </table>
            <asp:GridView ID="Grid_Output" 
                runat="server"
                AutoGenerateColumns="false"
                AllowPaging ="False"   
                DataKeyNames="Drg_ID" 
                HeaderStyle-Font-Bold="true"
                PageSize="5"
                Visible="true"
                onrowediting="gridView_RowEditing"
                onrowupdating="gridView_RowUpdating"                 
                onrowcancelingedit="gridView_RowCancelingEdit" 
                OnRowDataBound="gridView_RowDataBound"
                >                
                 <Columns>
                     <asp:TemplateField HeaderText="項目"  Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblDrg_ID" runat="server" Text='<%#Eval("Drg_ID") %>' />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="txtDrg_ID" runat="server" Text='<%#Eval("Drg_ID") %>' width="100px" />
                        </EditItemTemplate>
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="病歷號碼" Visible="False">
                        <ItemTemplate>
                            <asp:Label ID="lblhpid" runat="server" Text='<%#Eval("P_ID") %>' />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="txthpid" runat="server" Text='<%#Eval("P_ID") %>' width="100px" />
                        </EditItemTemplate>
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="抗生素名稱">
                        <ItemTemplate>
                            <asp:Label ID="lblAnti_name" runat="server" Text='<%#Eval("Anti_name") %>' />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtAnti_name" runat="server" Text='<%#Eval("Anti_name") %>' width="100px" />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="劑量">
                        <ItemTemplate>
                            <asp:Label ID="lblDose" runat="server" Text='<%#Eval("Dose") %>' />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtDose" runat="server" Text='<%#Eval("Dose") %>' width="100px" />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="用法">
                        <ItemTemplate>
                            <asp:Label ID="lblUsage" runat="server" Text='<%#Eval("Usage") %>' />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtUsage" runat="server" Text='<%#Eval("Usage") %>' width="100px" />
                        </EditItemTemplate>
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="開始使用日">
                        <ItemTemplate>
                            <asp:Label ID="lblStart_date" runat="server" Text='<%#Eval("Start_date") %>' />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtStart_date" runat="server" Text='<%#Eval("Start_date") %>' width="100px" class = "Calender" />
                            <img src="images/calender.png" />
                        </EditItemTemplate>
                     </asp:TemplateField> 
                     <asp:TemplateField HeaderText="續用">
                        <ItemTemplate>
                            <asp:Label ID="lblCon_usage" runat="server" Text='<%#Eval("Con_usage") %>' />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtCon_usage" runat="server" Text='<%#Eval("Con_usage") %>' width="100px" />
                        </EditItemTemplate>
                    </asp:TemplateField> 
                     <asp:TemplateField HeaderText="預估停止日">
                        <ItemTemplate>
                            <asp:Label ID="lblStop_date" runat="server" Text='<%#Eval("Stop_date") %>' />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtStop_date" runat="server" Text='<%#Eval("Stop_date") %>' width="100px" class = "Calender" />
                            <img src="images/calender.png" />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="編輯">
                        <ItemTemplate>
                            <asp:LinkButton ID="ButtonEdit" runat="server" CommandName="Edit" Text="編輯" />                          
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:LinkButton ID="ButtonUpdate" runat="server" CommandName="Update" Text="更新" />
                            <asp:LinkButton ID="ButtonCancel" runat="server" CommandName="Cancel" Text="取消" />
                        </EditItemTemplate>
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="Certification" Visible="false"  >
                        <ItemTemplate>                            
                            <asp:Label ID="Label5" runat="server" Text='<%#Eval("Certification") %>' ></asp:Label >
                         </ItemTemplate>                          
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="Update_User" Visible="false"  >
                        <ItemTemplate>                            
                            <asp:Label ID="Label6" runat="server" Text='<%#Eval("Update_User") %>'  ></asp:Label >
                         </ItemTemplate>                          
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="Update_Time" Visible="false"  >
                        <ItemTemplate>                            
                            <asp:Label ID="Label7" runat="server" Text='<%#Eval("Update_Time") %>' ></asp:Label >
                         </ItemTemplate>                          
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="認證">
                        <ItemTemplate>
                            <asp:CheckBox ID="chkStatus" runat="server" 
                                AutoPostBack="true" OnCheckedChanged="chkStatus_OnCheckedChanged"
                                Checked='<%# Convert.ToBoolean(Eval("Certification")) %>'
                                Text='<%# Eval("Certification").ToString().Equals("True") ? " Approved " : " Pending " %>' 
                                />
                        </ItemTemplate>                    
                    </asp:TemplateField>
                </Columns>
                <HeaderStyle BackColor="#4279bd" Font-Bold="True" ForeColor="#E7E7FF" />
                <RowStyle BackColor="#DEDFDE" ForeColor="Black" />
            </asp:GridView>
            <br />
              <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" Visible="false" >
                  <Columns>
                <asp:BoundField DataField="Anti_name" HeaderText="抗生素名稱" />
                <asp:BoundField DataField="Dose" HeaderText="劑量" />
                <asp:BoundField DataField="Usage" HeaderText="用法" />
                <asp:BoundField DataField="Start_date" HeaderText="開始日期" />
                <asp:BoundField DataField="Con_usage" HeaderText="續用" />
                <asp:BoundField DataField="Stop_date" HeaderText="預估停止日" />
                </Columns>
              </asp:GridView>
            
            <br />
             甲、缺乏培養資料(請將檢體送檢!!)
            <asp:CheckBox ID="Q1CheckBox" runat="server" />
            <asp:Label ID="Label1" runat="server" Text="已培養但未有結果"></asp:Label>
            <asp:CheckBox ID="Q1_1CheckBox" runat="server" />
            <asp:Label ID="Label2" runat="server" Text="有長但ST未完成"></asp:Label>
            <br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <asp:CheckBox ID="Q2CheckBox" runat="server" />依經驗認為治療需要&nbsp;&nbsp;&nbsp; 病患原在性疾病<asp:TextBox ID="TextBox17" runat="server"></asp:TextBox>
&nbsp;&nbsp;&nbsp; 感染性疾病名稱<asp:TextBox ID="TextBox18" runat="server"></asp:TextBox>
            (必填)<br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 適用理由(請詳實勾選):<br />
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            
            <asp:CheckBox ID="Q3CheckBox" runat="server" />
            <asp:Label ID="Label3" runat="server" Text="感染病情嚴重者:"></asp:Label>
            <asp:CheckBox ID="Q3_1CheckBox" runat="server"/>
            <asp:Label ID="Label5" runat="server" Text="敗血症或敗血性休克"></asp:Label>
            <asp:CheckBox ID="Q3_2CheckBox" runat="server" />
            <asp:Label ID="Label6" runat="server" Text="中樞神經感染"></asp:Label>
            <asp:CheckBox ID="Q3_3CheckBox" runat="server" />
            <asp:Label ID="Label7" runat="server" Text="使用呼吸器者"></asp:Label>
            <br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <asp:CheckBox ID="Q4CheckBox" runat="server" />
            <asp:Label ID="Label8" runat="server" Text="免疫狀態不良者:"></asp:Label>
            <asp:CheckBox ID="Q4_1CheckBox" runat="server" />
            <asp:Label ID="Label9" runat="server" Text="接受免疫抑制劑"></asp:Label>
            <asp:CheckBox ID="Q4_2CheckBox" runat="server" />
            <asp:Label ID="Labe20" runat="server" Text="接受抗癌化學療法"></asp:Label>
            <asp:CheckBox ID="Q4_3CheckBox" runat="server" />
            <asp:Label ID="Labe21" runat="server" Text="白血球≦1000/cumm或多核球≦500/cumm"></asp:Label>
            <br />
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <asp:CheckBox ID="Q5CheckBox" runat="server" />
            <asp:Label ID="Label12" runat="server" Text="手術中發現有明顯感染病灶者:"></asp:Label>
            <asp:CheckBox ID="Q5_1CheckBox" runat="server" />
            <asp:Label ID="Labe22" runat="server" Text="臟器穿孔"></asp:Label>
            <asp:CheckBox ID="Q5_2CheckBox" runat="server" />
            <asp:Label ID="Labe23" runat="server" Text="嚴重汙染傷口"></asp:Label>
            <asp:CheckBox ID="Q5_3CheckBox" runat="server" />
            <asp:Label ID="Labe24" runat="server" Text="手術發生感染併發症"></asp:Label>
            <br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <asp:CheckBox ID="Q6CheckBox" runat="server" />
            <asp:Label ID="Labe25" runat="server" Text="疑似感然之早產兒及兩個月內之新生兒"></asp:Label>
            <br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <asp:CheckBox ID="Q7CheckBox" runat="server" />
            <asp:Label ID="Labe26" runat="server" Text="嬰幼兒(2m-5y)疑似感染疾病，在使用第一線抗生素72小時仍無明顯療效者"></asp:Label>
            <br />
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <asp:CheckBox ID="Q8CheckBox" runat="server" />
            <asp:Label ID="Labe27" runat="server" Text="脾臟切除有不明熱"></asp:Label>
            <br />
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <asp:CheckBox ID="Q9CheckBox" runat="server" />
            <asp:Label ID="Labe28" runat="server" Text="發生明確嚴重院內感染者"></asp:Label>
            <br />
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <asp:CheckBox ID="Q10CheckBox" runat="server" />
            <asp:Label ID="Labe29" runat="server" Text="常有厭氧菌與非厭氧菌混和感染之組織部位感染(如糖尿病足部壞疽併感染、骨盆腔內感染)"></asp:Label>
            <br />
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <asp:CheckBox ID="Q11CheckBox" runat="server" />
            <asp:Label ID="Labe30" runat="server" Text="其他特殊理由，請說明"></asp:Label>

            <asp:TextBox ID="TextBox19" runat="server" Width="465px"></asp:TextBox>
            <br />
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <asp:CheckBox ID="Q12CheckBox" runat="server" />
            <asp:Label ID="Labe31" runat="server" Text="預防性抗生素使用要請說明  侵入性步驟或手術名稱"></asp:Label>
            <asp:TextBox ID="TextBox20" runat="server" Width="292px"></asp:TextBox>
            <br />
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 手術傷口分類:<asp:CheckBox ID="Q12_1CheckBox" runat="server" />
            <asp:Label ID="Labe32" runat="server" Text="清潔性"></asp:Label>
            <asp:CheckBox ID="Q12_2CheckBox" runat="server" />
            <asp:Label ID="Labe33" runat="server" Text="清潔但易受汙染"></asp:Label>
            <asp:CheckBox ID="Q12_3CheckBox" runat="server"  />
            <asp:Label ID="Labe34" runat="server" Text="污染性"></asp:Label>
            <asp:CheckBox ID="Q12_4CheckBox" runat="server" />
            <asp:Label ID="Labe35" runat="server" Text="骯髒性"></asp:Label>
            <br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:CheckBox ID="Q13CheckBox" runat="server" />
            <asp:Label ID="Labe36" runat="server" Text="經感染科專科醫師會診建議:"></asp:Label>
            <asp:TextBox ID="TextBox21" runat="server" Width="284px"></asp:TextBox>
            會診日期:<asp:TextBox ID="TextBox28" runat="server" ReadOnly="True" Width="101px"></asp:TextBox>
            <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/Antibiotic/images/calendar.png" OnClick="ImageButton1_Click"/>
            <div id="div1" style="position:absolute ;z-index:1;top:545px;left:900px"> 
            <asp:Calendar ID="Calendar1" runat="server" Visible="False" OnSelectionChanged="Calendar1_SelectionChanged"></asp:Calendar>
            </div>
            <br />
            乙、有培養資料證明治療需要<br />
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:CheckBox ID="Q14CheckBox" runat="server" />
            <asp:Label ID="Labe38" runat="server" Text="已有結果不必換藥"></asp:Label>
            <br />
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:CheckBox ID="Q15CheckBox" runat="server" />
            <asp:Label ID="Labe39" runat="server" Text="使用第一線抗生素超過72小時，經微生物培養及藥物敏感試驗證實對第一線抗生素具抗藥性，確實需要使用者。"></asp:Label>
            <br />
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:CheckBox ID="Q16CheckBox" runat="server" />
            <asp:Label ID="Labe40" runat="server" Text="培養所得抗生素敏感性試驗證實欲申請藥及其他藥物均有效，但是
                "></asp:Label>

            <br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:CheckBox ID="Q16_1CheckBox" runat="server" />
            <asp:Label ID="Labe41" runat="server" Text="其他藥物臨床不具療效或不適用(請說明陪養菌種與日期)"></asp:Label>
            <asp:TextBox ID="TextBox22" runat="server" Width="320px"></asp:TextBox>
            <br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:CheckBox ID="Q16_2CheckBox" runat="server" />
            <asp:Label ID="Labe42" runat="server" Text="其他藥物造成嚴重副作用需停用，而欲申請知要為最適當之替代品，請說明藥物使用情形、種類、使用時間及副作用" Value="其他藥物造成嚴重副作用需停用，而欲申請之藥為最適當之替代品，請說明藥物使用情形、種類、使用時間及副作用"></asp:Label>
            <br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:CheckBox ID="Q16_3CheckBox" runat="server" />
            <asp:Label ID="Labe43" runat="server" Text="其他特殊理由，請說明"></asp:Label>

            <asp:TextBox ID="TextBox23" runat="server" Width="645px"></asp:TextBox>
            <br />
            丙、住院後出現嚴重院內感染者<br />
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:CheckBox ID="Q17CheckBox" runat="server" />
            <asp:Label ID="Labe44" runat="server" Text="有敗血症，經血液培養證實者"></asp:Label>
            <asp:CheckBox ID="Q17_1CheckBox" runat="server" />
            <asp:Label ID="Labe45" runat="server" Text="有肺炎"></asp:Label>
            <asp:Label ID="Labe46" runat="server" Text="且有"></asp:Label>
            <asp:CheckBox ID="Q17_2CheckBox" runat="server" />
            <asp:Label ID="Labe47" runat="server" Text="發燒"></asp:Label>
            <asp:CheckBox ID="Q17_3CheckBox" runat="server" />
            <asp:Label ID="Labe48" runat="server" Text="X光有浸潤性病變"></asp:Label>
            <asp:CheckBox ID="Q17_4CheckBox" runat="server" />
            <asp:Label ID="Labe49" runat="server" Text="全身性症狀"></asp:Label>

            <br />
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:CheckBox ID="Q18CheckBox" runat="server" />
            <asp:Label ID="Labe50" runat="server" Text="有尿道感染，經細菌陪養證實，且有臨床症狀"></asp:Label>
            <br />
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:CheckBox ID="Q19CheckBox" runat="server" />
            <asp:Label ID="Labe51" runat="server" Text="有傷口組織感染，經細菌陪有證實，請說明:"></asp:Label>

            <br />
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 原在疾病:<asp:TextBox ID="TextBox24" runat="server" Width="353px"></asp:TextBox>

            <br />
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 手術方法:<asp:TextBox ID="TextBox25" runat="server" Width="353px"></asp:TextBox>
            <br />
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 感染部位:<asp:TextBox ID="TextBox26" runat="server" Width="353px"></asp:TextBox>
            <br />
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:CheckBox ID="Q20CheckBox" runat="server" />
            <asp:Label ID="Labe52" runat="server" Text="其他院內感染，請說明"></asp:Label>

            <asp:TextBox ID="TextBox27" runat="server"></asp:TextBox>
            <br />
            <br />
            <asp:Button ID="Button1" runat="server" Text="UpDateButton" OnClick="Button1_Click" />
            <asp:Button ID="Button2" runat="server" Text="CreatPDF" OnClick="Button2_Click" />
                    
        </div>
    </form>
</body>
</html>
