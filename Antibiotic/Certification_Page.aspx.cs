using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Configuration;
using System.Text.RegularExpressions;

using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using iTextSharp.text.html;
using iTextSharp.tool.xml;
using System.Text;
using System.Web.UI.HtmlControls;

public partial class Antibiotic_Certification_Page : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(@"");

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            loadGridData();
            PatientinforList();
            AntibioticChecklist();
        }
        //string Patient_Session_Id = Server.UrlDecode(Request.QueryString["Patient_Session_Id"]);
        string Patient_Session_Id = Convert.ToString(Session["Patient_Session_Id"]); //Default查詢頁面的病歷號
        if (Patient_Session_Id != null)
        {
            TextBox1.Text = Patient_Session_Id;
            //TextBox1.Text = Request.QueryString["Patient_Session_Id"].ToString(); //Patient_Session_Id填入病歷號碼欄位
            //TextBox2.Text = Request.QueryString["name"].ToString();
        }
        else
        {
            TextBox1.Text = "NO DATA";
        }
    }
    private void loadGridData()
    {
        string strConnString = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
        string Patient_Session_Id = Convert.ToString(Session["Patient_Session_Id"]);

        // string Patient_Session_Id = Request.QueryString["Patient_Session_Id"].ToString();
        using (SqlConnection con = new SqlConnection(strConnString))
        {
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandText = "SELECT * FROM Drog_info WHERE P_ID='" + Patient_Session_Id + "'";
                cmd.Connection = con;
                cmd.Parameters.AddWithValue("@P_ID", TextBox1.Text.Trim());
                DataTable dt = new DataTable();
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    sda.Fill(dt);
                    Grid_Output.DataSource = dt;
                    Grid_Output.DataBind();
                    GridView1.DataSource = dt;
                    GridView1.DataBind();
                }
            }
        }
    }


    protected void gridView_RowUpdating(object sender, GridViewUpdateEventArgs e) //Update GridView Row Data
    {
        string p_Drg_ID = Grid_Output.DataKeys[e.RowIndex].Values["Drg_ID"].ToString();
        Label p_ID = (Label)Grid_Output.Rows[e.RowIndex].FindControl("txthpid");
        TextBox p_Anti_name = (TextBox)Grid_Output.Rows[e.RowIndex].FindControl("txtAnti_name");
        TextBox p_Dose = (TextBox)Grid_Output.Rows[e.RowIndex].FindControl("txtDose");
        TextBox p_Usage = (TextBox)Grid_Output.Rows[e.RowIndex].FindControl("txtUsage");
        TextBox p_Start_date = (TextBox)Grid_Output.Rows[e.RowIndex].FindControl("txtStart_date");
        TextBox p_Con_usage = (TextBox)Grid_Output.Rows[e.RowIndex].FindControl("txtCon_usage");
        TextBox p_Stop_date = (TextBox)Grid_Output.Rows[e.RowIndex].FindControl("txtStop_date");
        con.Open();
        SqlCommand cmd = new SqlCommand("update [Drog_info] set [Anti_name]='" + p_Anti_name.Text + "', [Dose]='" + p_Dose.Text + "', [Usage]='" + p_Usage.Text + "', [Start_date]='" + p_Start_date.Text + "', [Con_usage]='" + p_Con_usage.Text + "', [Stop_date]='" + p_Stop_date.Text + "' where [Drg_ID]='" + p_Drg_ID + "'", con);
        cmd.ExecuteNonQuery();
        con.Close();
        Grid_Output.EditIndex = -1;
        loadGridData();
    }
    //經由CheckBox去驗證Gridview內的資料並UpDate到DataBase
    public void chkStatus_OnCheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkStatus = (CheckBox)sender;
        Int64 nID = Convert.ToInt64(Grid_Output.DataKeys[((GridViewRow)chkStatus.NamingContainer).RowIndex].Value);
        
        string sQuery = "UPDATE [Drog_info] SET Certification=@Certification, Update_User=@Update_User, Update_Time=@Update_Time WHERE Drg_ID = @Drg_ID";
        string consString = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
        using (SqlConnection conn = new SqlConnection(consString))
        {
            SqlCommand com = new SqlCommand(sQuery, conn);
            com.Parameters.Add("@Certification", SqlDbType.Bit).Value = chkStatus.Checked;
            com.Parameters.Add("@Update_User", SqlDbType.NVarChar, 50).Value = UserLabel.Text;
            com.Parameters.AddWithValue("@Update_Time", DateTime.Now);
            
            com.Parameters.Add("@Drg_ID", SqlDbType.BigInt).Value = nID;
            conn.Open();
            com.ExecuteNonQuery();
        }
        loadGridData();
    }

    protected void gridView_RowEditing(object sender, GridViewEditEventArgs e) // RowEditing
    {
        Grid_Output.EditIndex = e.NewEditIndex;
        loadGridData();
    }
    protected void gridView_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e) //RowCancelingEdit Row Data
    {
        Grid_Output.EditIndex = -1;
        loadGridData();
    }
    /* protected void gridView_RowDeleting(object sender, GridViewDeleteEventArgs e)  //Delete GridView Row Data
     {
         string p_Drg_ID = Grid_Output.DataKeys[e.RowIndex].Values["Drg_ID"].ToString();
         con.Open();
         SqlCommand cmd = new SqlCommand("delete from [Drog_info] where Drg_ID='" + p_Drg_ID + "'", con);
         int result = cmd.ExecuteNonQuery();
         con.Close();
         loadGridData();
     }*/

    protected void gridView_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        string p_Anti_name = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "Anti_name"));
        string Update_User = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "Update_User"));
        string Update_Time = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "Update_Time"));
        bool Row8 = Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "Certification"));
        UserLabel.Text = Convert.ToString(Session["Certification_Session_Id"]);
               

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (p_Anti_name != "" && Row8 == false)
            {
                LinkButton1.Attributes["onclick"] = "if(!confirm('尚有資料未確認，確定要離開??')){ return false; };";
            }

            //System.Web.UI.WebControls.Image img = (System.Web.UI.WebControls.Image)e.Row.FindControl("chkimage");
            /* Button lnkbtnresult = (Button)e.Row.FindControl("ButtonDelete");
             if (lnkbtnresult != null)
             {
                 lnkbtnresult.Attributes["onclick"] = "if(!confirm('確定要刪除 " + p_Anti_name + " ?')){ return false; };";
             }
             */
            /* if (UserLabel.Text == "") //如果沒登入使用者Columns[12]就隱藏
             {
                 Grid_Output.Columns[12].Visible = false;
             }
             else
             {
                 Grid_Output.Columns[12].Visible = true;
             }*/


            if (Row8 == true)
            {
                //e.Row.Cells[8].Text = "已驗證";
                //img.ImageUrl = "images/greencheck.png";
                //e.Row.Cells[8].Controls.Add(img);
                var GridCell = e.Row.Cells[8];
                System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
                img.ImageUrl = "images/greencheck.png";
                img.Height = 16;
                GridCell.Controls.Add(img);

                Label hdr = new Label();
                hdr.Text = "已驗證";
                GridCell.Controls.Add(hdr);

                if (p_Anti_name == "")
                {
                    img.Visible = false;
                    hdr.Visible = false;
                }
                //e.Row.Cells[8].Attributes.Add("style", "background-image: url(images/greencheck.png)");
            }
            else if (Row8 == false)
            {
                var GridCell = e.Row.Cells[8];
                System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
                img.ImageUrl = "images/CrossMark.png";
                img.Height = 16;
                GridCell.Controls.Add(img);

                Label hdr = new Label();
                hdr.Text = "未驗證";
                GridCell.Controls.Add(hdr);

                if (p_Anti_name == "")
                {
                    img.Visible = false;
                    hdr.Visible = false;
                }
            }

            

            if (Update_User != "")  //mouseover顯示已驗證的User訊息於Gridview row 上
            {
                e.Row.ToolTip = Update_User + "已於" + Update_Time + "驗證";
            }

           
            /* foreach (TableCell item in e.Row.Cells)
             {
                 if (item.HasControls())
                 {
                     foreach (Control c in item.Controls)
                     {
                         if (c is CheckBox)
                         {
                             item.Attributes["onclick"] = "event.cancelBubble=true;";
                         }
                     }
                 }
             }*/

            e.Row.Attributes["onmouseover"] = "this.style.cursor='hand';this.style.background='#eeff00';";
            e.Row.Attributes["onmouseout"] = "this.style.textDecoration='none';this.style.background='#ffffff';";
            //e.Row.Attributes["onclick"] = manager.GetPostBackEventReference(sender as GridView, "SELECT$" + e.Row.RowIndex);
            //e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(sender as GridView, "SELECT$" + e.Row.RowIndex);
            //e.Row.ToolTip = "My FooBar tooltip";
            // e.Row.Attributes.Add("title", "My FooBar tooltip");
        }
    }



    private void PatientinforList()  //retrieve Patient_infor data
    {
        using (SqlConnection con1 = new SqlConnection(@"Data Source=3F-54-94\SQLEXPRESS;Initial Catalog=Antibiotic;Persist Security Info=True;User ID=sa;Password=1234"))
        {
            string Patient_Session_Id = Convert.ToString(Session["Patient_Session_Id"]);
            DataTable dt = new DataTable();
            con1.Open();
            SqlDataReader myReader = null;
            // SqlCommand myCommand = new SqlCommand("select * from customer_registration where username='" + Session["username"] + "'", con1);
            SqlCommand myCommand = new SqlCommand("SELECT * FROM [Patient_infor] WHERE P_ID='" + Patient_Session_Id + "'", con1);
            myCommand.Parameters.AddWithValue("@P_ID", TextBox1.Text.Trim());
            myReader = myCommand.ExecuteReader();

            while (myReader.Read())
            {
                bool BSex = myReader.GetBoolean(myReader.GetOrdinal("P_Sex"));
                if (BSex == true)
                {
                    ManCheckBox.Checked = true;
                }
                else
                {
                    GirlCheckBox.Checked = true;
                }
                //TextBox1.Text = (myReader["P_ID"].ToString());
                TextBox2.Text = (myReader["P_Name"].ToString());


                //DropDownListGender.SelectedItem.Text = (myReader["P_Sex"].ToString());
                TextBox30.Text = (myReader["P_Age"].ToString());
                TextBox30.Text = TextBox30.Text.Replace(" ", "");
                TextBox3.Text = (myReader["P_bed"].ToString());
                TextBox3.Text = TextBox3.Text.Replace(" ", "");
                TextBox4.Text = (myReader["P_dr"].ToString());
                TextBox4.Text = TextBox4.Text.Replace(" ", "");
                TextBox6.Text = (myReader["P_ph"].ToString());
                TextBox6.Text = TextBox6.Text.Replace(" ", "");
                TextBox7.Text = (myReader["P_weight"].ToString());
                TextBox7.Text = TextBox7.Text.Replace(" ", "");
                //  DropDownListCountry.SelectedItem.Text = (myReader["country"].ToString());
                TextBox8.Text = (myReader["P_creatin"].ToString());
                TextBox8.Text = TextBox8.Text.Replace(" ", "");
                TextBox9.Text = (myReader["P_bun"].ToString());
                TextBox9.Text = TextBox9.Text.Replace(" ", "");
                TextBox10.Text = (myReader["P_got"].ToString());
                TextBox10.Text = TextBox10.Text.Replace(" ", "");
                TextBox11.Text = (myReader["P_gpt"].ToString());
                TextBox11.Text = TextBox11.Text.Replace(" ", "");
            }
            con1.Close();
        }//end using
    }

    protected void ImageButton1_Click(object sender, ImageClickEventArgs e)  //Calendar1 Visible "true" or "false"
    {
        Calendar1.Visible = true;    //看得見！現形！
    }
    protected void Calendar1_SelectionChanged(object sender, EventArgs e)
    {
        TextBox28.Text = Calendar1.SelectedDate.ToShortDateString();
        Calendar1.Visible = false;    //選完日期後，看不見！隱形！
    }
    private void AntibioticChecklist()   //retrieve Antibiotic data
    {
        //string Patient_Session_Id = Request.QueryString["Patient_Session_Id"].ToString();
        string Patient_Session_Id = Convert.ToString(Session["Patient_Session_Id"]);
        using (SqlConnection con1 = new SqlConnection(@"Data Source=3F-54-94\SQLEXPRESS;Initial Catalog=Antibiotic;Persist Security Info=True;User ID=sa;Password=1234"))
        {
            DataTable dt = new DataTable();
            con1.Open();
            SqlDataReader reader = null;
            // SqlCommand myCommand = new SqlCommand("select * from customer_registration where username='" + Session["username"] + "'", con1);
            SqlCommand myCommand = new SqlCommand("SELECT * FROM [Antibiotic] WHERE Q_P_ID='" + Patient_Session_Id + "'", con1);
            reader = myCommand.ExecuteReader();
            while (reader.Read())
            {
                bool BQ1 = reader.GetBoolean(reader.GetOrdinal("Q1_1"));
                Q1CheckBox.Checked = BQ1;
                bool BQ1_2 = reader.GetBoolean(reader.GetOrdinal("Q1_2"));
                Q1_1CheckBox.Checked = BQ1_2;
                bool Q2 = reader.GetBoolean(reader.GetOrdinal("Q2"));
                Q2CheckBox.Checked = Q2;
                TextBox17.Text = (reader["Q2_1"].ToString());
                TextBox17.Text = TextBox17.Text.Replace(" ", "");
                TextBox18.Text = (reader["Q2_2"].ToString());
                TextBox18.Text = TextBox18.Text.Replace(" ", "");
                bool BQ3 = reader.GetBoolean(reader.GetOrdinal("Q3"));
                Q3CheckBox.Checked = BQ3;
                bool BQ3_1 = reader.GetBoolean(reader.GetOrdinal("Q3_1"));
                Q3_1CheckBox.Checked = BQ3_1;
                bool BQ3_2 = reader.GetBoolean(reader.GetOrdinal("Q3_2"));
                Q3_2CheckBox.Checked = BQ3_2;
                bool BQ3_3 = reader.GetBoolean(reader.GetOrdinal("Q3_3"));
                Q3_3CheckBox.Checked = BQ3_3;
                bool BQ4 = reader.GetBoolean(reader.GetOrdinal("Q4"));
                Q4CheckBox.Checked = BQ4;
                bool BQ4_1 = reader.GetBoolean(reader.GetOrdinal("Q4_1"));
                Q4_1CheckBox.Checked = BQ4_1;
                bool BQ4_2 = reader.GetBoolean(reader.GetOrdinal("Q4_2"));
                Q4_2CheckBox.Checked = BQ4_2;
                bool BQ4_3 = reader.GetBoolean(reader.GetOrdinal("Q4_3"));
                Q4_3CheckBox.Checked = BQ4_3;
                bool BQ5 = reader.GetBoolean(reader.GetOrdinal("Q5"));
                Q5CheckBox.Checked = BQ5;
                bool BQ5_1 = reader.GetBoolean(reader.GetOrdinal("Q5_1"));
                Q5_1CheckBox.Checked = BQ5_1;
                bool BQ5_2 = reader.GetBoolean(reader.GetOrdinal("Q5_2"));
                Q5_2CheckBox.Checked = BQ5_2;
                bool BQ5_3 = reader.GetBoolean(reader.GetOrdinal("Q5_3"));
                Q5_3CheckBox.Checked = BQ5_3;
                bool BQ6 = reader.GetBoolean(reader.GetOrdinal("Q6"));
                Q6CheckBox.Checked = BQ6;
                bool BQ7 = reader.GetBoolean(reader.GetOrdinal("Q7"));
                Q7CheckBox.Checked = BQ7;
                bool BQ8 = reader.GetBoolean(reader.GetOrdinal("Q8"));
                Q8CheckBox.Checked = BQ8;
                bool BQ9 = reader.GetBoolean(reader.GetOrdinal("Q9"));
                Q9CheckBox.Checked = BQ9;
                bool BQ10 = reader.GetBoolean(reader.GetOrdinal("Q10"));
                Q10CheckBox.Checked = BQ10;
                bool BQ11 = reader.GetBoolean(reader.GetOrdinal("Q11"));
                Q11CheckBox.Checked = BQ11;
                TextBox19.Text = (reader["Q2_2"].ToString());
                TextBox19.Text = TextBox19.Text.Replace(" ", "");
                bool BQ12 = reader.GetBoolean(reader.GetOrdinal("Q12"));
                Q12CheckBox.Checked = BQ12;
                TextBox20.Text = (reader["Q12_t"].ToString());
                TextBox20.Text = TextBox20.Text.Replace(" ", "");
                bool BQ12_1 = reader.GetBoolean(reader.GetOrdinal("Q12_1"));
                Q12_1CheckBox.Checked = BQ12_1;
                bool BQ12_2 = reader.GetBoolean(reader.GetOrdinal("Q12_2"));
                Q12_2CheckBox.Checked = BQ12_2;
                bool BQ12_3 = reader.GetBoolean(reader.GetOrdinal("Q12_3"));
                Q12_3CheckBox.Checked = BQ12_3;
                bool BQ12_4 = reader.GetBoolean(reader.GetOrdinal("Q12_4"));
                Q12_4CheckBox.Checked = BQ12_4;
                bool BQ13 = reader.GetBoolean(reader.GetOrdinal("Q13"));
                Q13CheckBox.Checked = BQ13;
                TextBox21.Text = (reader["Q13_t"].ToString());
                TextBox28.Text = (reader["Q13_1"].ToString());
                TextBox28.Text = TextBox28.Text.Replace(" ", "");
                bool BQ14 = reader.GetBoolean(reader.GetOrdinal("Q14"));
                Q14CheckBox.Checked = BQ14;
                bool BQ15 = reader.GetBoolean(reader.GetOrdinal("Q15"));
                Q15CheckBox.Checked = BQ15;
                bool BQ16 = reader.GetBoolean(reader.GetOrdinal("Q16"));
                Q16CheckBox.Checked = BQ16;
                bool BQ16_1 = reader.GetBoolean(reader.GetOrdinal("Q16_1"));
                Q16_1CheckBox.Checked = BQ16_1;
                TextBox22.Text = (reader["Q16_1_t"].ToString());
                bool BQ16_2 = reader.GetBoolean(reader.GetOrdinal("Q16_2"));
                Q16_2CheckBox.Checked = BQ16_2;
                bool BQ16_3 = reader.GetBoolean(reader.GetOrdinal("Q16_3"));
                Q16_3CheckBox.Checked = BQ16_3;
                TextBox23.Text = (reader["Q16_3_t"].ToString());
                bool BQ17 = reader.GetBoolean(reader.GetOrdinal("Q17"));
                Q17CheckBox.Checked = BQ17;
                bool BQ17_1 = reader.GetBoolean(reader.GetOrdinal("Q17_1"));
                Q17_1CheckBox.Checked = BQ17_1;
                bool BQ17_2 = reader.GetBoolean(reader.GetOrdinal("Q17_2"));
                Q17_2CheckBox.Checked = BQ17_2;
                bool BQ17_3 = reader.GetBoolean(reader.GetOrdinal("Q17_3"));
                Q17_3CheckBox.Checked = BQ17_3;
                bool BQ17_4 = reader.GetBoolean(reader.GetOrdinal("Q17_4"));
                Q17_4CheckBox.Checked = BQ17_4;
                bool BQ18 = reader.GetBoolean(reader.GetOrdinal("Q18"));
                Q18CheckBox.Checked = BQ18;
                bool BQ19 = reader.GetBoolean(reader.GetOrdinal("Q19"));
                Q19CheckBox.Checked = BQ19;
                TextBox24.Text = (reader["Q19_1"].ToString());
                TextBox25.Text = (reader["Q19_2"].ToString());
                TextBox26.Text = (reader["Q19_3"].ToString());
                bool BQ20 = reader.GetBoolean(reader.GetOrdinal("Q20"));
                Q20CheckBox.Checked = BQ20;
                TextBox27.Text = (reader["Q20_1"].ToString());
            }
            con1.Close();
        }
    }

    /* protected void btnSave_Click(object sender, EventArgs e)
     {
       //  DataTable dt = (DataTable)ViewState["Table"];

         //Loop through the GridView and get the insert values.
         foreach (GridViewRow row in Grid_Insert.Rows)
         {
             if (row.RowType == DataControlRowType.DataRow)
             {
                 Label lblid = (Label)row.FindControl("lblDrg_ID");
              //   Label p_Drg_ID = (Label)row.FindControl("Drg_ID");
                 TextBox p_Anti_name = (TextBox)row.FindControl("txtAnti_name");
                 TextBox p_Dose = (TextBox)row.FindControl("txtDose");
                 TextBox p_Usage = (TextBox)row.FindControl("txtUsage");
                 TextBox p_Start_date = (TextBox)row.FindControl("txtStart_date");
                 TextBox p_Con_usage = (TextBox)row.FindControl("txtCon_usage");
                 TextBox p_Stop_date = (TextBox)row.FindControl("txtStop_date");
                 //Insert them into the DataTable
                // dt.Rows.Add(lblid.Text,p_Anti_name.Text, p_Dose.Text, p_Usage.Text, p_Start_date.Text, p_Con_usage.Text, p_Stop_date.Text);

                 using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString))
                 {
                     string sql = "insert into [Drog_info] ([Anti_name], [Dose], [Usage], [Start_date], [Con_usage], [Stop_date]) values(@Anti_name,@Dose,@Usage,@Start_date,@Con_usage,@Stop_date)";
                     SqlCommand cmd = new SqlCommand(sql, conn);

                   //  cmd.Parameters.Add("@Drg_ID", SqlDbType.NChar, 10).Value = p_Drg_ID.Text;
                     cmd.Parameters.Add("@Anti_name", SqlDbType.NVarChar, 50).Value = p_Anti_name.Text;
                     cmd.Parameters.Add("@Dose", SqlDbType.NVarChar, 50).Value = p_Dose.Text;
                     cmd.Parameters.Add("@Usage", SqlDbType.NVarChar, 50).Value = p_Usage.Text;
                     cmd.Parameters.Add("@Start_date", SqlDbType.NVarChar, 50).Value = p_Start_date.Text;
                     cmd.Parameters.Add("@Con_usage", SqlDbType.NVarChar, 50).Value = p_Con_usage.Text;
                     cmd.Parameters.Add("@Stop_date", SqlDbType.NVarChar, 50).Value = p_Stop_date.Text;
                     // cmd.Parameters.AddWithValue("@Drg_Cre_Time", DateTime.Now);
                     DataTable dt = new DataTable();
                     using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                     {
                         sda.Fill(dt);
                         Grid_Output.DataSource = dt;
                         Grid_Output.DataBind();

                     }
                 }
             }
         }

         //Using ViewState method to store the Insert values.
         //You could also insert them into database.
       //  ViewState["Table"] = dt;

         //Display the result:
       //  Grid_Output.DataSource = dt;
       //  Grid_Output.DataBind();

     }

     private void getAuditChecklist()
     {
         SqlCommand cmd = null;
         string conn = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
         string queryString = @"SELECT Mount, Braker, Access, Conn_Net, Log_Book, Pictures, Floor, Cb_Lenght, Channel FROM AUDITOR_CHECKLIST " +
             "WHERE SITE_ID = @SiteID";

         using (SqlConnection connection = new SqlConnection(conn))
         {
             SqlCommand command = new SqlCommand(queryString, connection);
             connection.Open();
             cmd = new SqlCommand(queryString);
             cmd.Connection = connection;

             cmd.Parameters.Add(new SqlParameter("@SiteID", //the name of the parameter to map
                   System.Data.SqlDbType.NVarChar, //SqlDbType value
                   20, //The width of the parameter
                   "SITE_ID")); //The name of the column source
                                //Fill the parameter with the value retrieved
                                //from the text field
             cmd.Parameters["@SiteID"].Value = foo.Site_ID;

             SqlDataReader reader = cmd.ExecuteReader();
             while (reader.Read())
             {
                 CheckBox1.Checked = (reader.GetBoolean(reader.GetOrdinal("Mount")));
                 CheckBox2.Checked = (reader.GetBoolean(reader.GetOrdinal("Braker")));
                 CheckBox3.Checked = (reader.GetBoolean(reader.GetOrdinal("Access")));
                 CheckBox4.Checked = (reader.GetBoolean(reader.GetOrdinal("Conn_Net")));
                 CheckBox5.Checked = (reader.GetBoolean(reader.GetOrdinal("Log_Book")));
                 CheckBox6.Checked = (reader.GetBoolean(reader.GetOrdinal("Pictures")));
                 CheckBox8.Checked = (reader.GetBoolean(reader.GetOrdinal("Floor")));
                 CheckBox9.Checked = (reader.GetBoolean(reader.GetOrdinal("Cb_lenght")));
                 CheckBox10.Checked = (reader.GetBoolean(reader.GetOrdinal("Channel")));



             }


             reader.Close();
         }
     }*/


    protected void Button1_Click(object sender, EventArgs e)
    {
        string ID = TextBox1.Text;
        string Name = TextBox2.Text;
        string Sex_Checked = ManCheckBox.Checked ? "1" : "0";
        string Age = TextBox30.Text;
        string Bed = TextBox3.Text;
        string Dr = TextBox4.Text;
        string Ph = TextBox6.Text;
        string Weight = TextBox7.Text;
        string Creatin = TextBox8.Text;
        string Bun = TextBox9.Text;
        string Got = TextBox10.Text;
        string Gpt = TextBox11.Text;


        using (SqlCommand cmd = new SqlCommand("update [Patient_infor] set [P_Name]=@P_Name, [P_Sex]=@P_Sex, [P_Age]=@P_Age, [P_bed]=@P_bed, [P_dr]=@P_dr, [P_ph]=@P_ph, [P_weight]=@P_weight, [P_creatin]=@P_creatin, [P_bun]=@P_bun, [P_got]=@P_got, [P_gpt]=@P_gpt WHERE [P_ID]=@P_ID"))
        {
            cmd.Connection = con;
            cmd.Parameters.AddWithValue("@P_ID", ID);
            cmd.Parameters.AddWithValue("@P_Name", Name);
            if (ManCheckBox.Checked)
            {
                cmd.Parameters.AddWithValue("@P_Sex", Sex_Checked);
            }
            else
            {
                cmd.Parameters.AddWithValue("@P_Sex", 0);
            }
            cmd.Parameters.AddWithValue("@P_Age", Age);
            cmd.Parameters.AddWithValue("@P_bed", Bed);
            cmd.Parameters.AddWithValue("@P_dr", Dr);
            cmd.Parameters.AddWithValue("@P_ph", Ph);
            cmd.Parameters.AddWithValue("@P_weight", Weight);
            cmd.Parameters.AddWithValue("@P_creatin", Creatin);
            cmd.Parameters.AddWithValue("@P_bun", Bun);
            cmd.Parameters.AddWithValue("@P_got", Got);
            cmd.Parameters.AddWithValue("@P_gpt", Gpt);

            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }

        bool relocate1 = Q1CheckBox.Checked ? true : false;
        bool relocate1_1 = Q1_1CheckBox.Checked ? true : false;
        bool relocate2 = Q2CheckBox.Checked ? true : false;
        bool relocate6 = Q6CheckBox.Checked ? true : false;
        bool relocate7 = Q7CheckBox.Checked ? true : false;
        bool relocate8 = Q8CheckBox.Checked ? true : false;
        bool relocate9 = Q9CheckBox.Checked ? true : false;
        bool relocate10 = Q10CheckBox.Checked ? true : false;
        bool relocate11 = Q11CheckBox.Checked ? true : false;
        bool relocate12 = Q12CheckBox.Checked ? true : false;
        bool relocate13 = Q13CheckBox.Checked ? true : false;
        bool relocate14 = Q14CheckBox.Checked ? true : false;
        bool relocate15 = Q15CheckBox.Checked ? true : false;
        bool relocate18 = Q18CheckBox.Checked ? true : false;
        //Muti Q
        bool relocate3 = Q3CheckBox.Checked ? true : false;
        bool relocate3_1 = Q3_1CheckBox.Checked ? true : false;
        bool relocate3_2 = Q3_2CheckBox.Checked ? true : false;
        bool relocate3_3 = Q3_3CheckBox.Checked ? true : false;
        bool relocate4 = Q4CheckBox.Checked ? true : false;
        bool relocate4_1 = Q4_1CheckBox.Checked ? true : false;
        bool relocate4_2 = Q4_2CheckBox.Checked ? true : false;
        bool relocate4_3 = Q4_3CheckBox.Checked ? true : false;
        bool relocate5 = Q5CheckBox.Checked ? true : false;
        bool relocate5_1 = Q5_1CheckBox.Checked ? true : false;
        bool relocate5_2 = Q5_2CheckBox.Checked ? true : false;
        bool relocate5_3 = Q5_3CheckBox.Checked ? true : false;
        bool relocate12_1 = Q12_1CheckBox.Checked ? true : false;
        bool relocate12_2 = Q12_2CheckBox.Checked ? true : false;
        bool relocate12_3 = Q12_3CheckBox.Checked ? true : false;
        bool relocate12_4 = Q12_4CheckBox.Checked ? true : false;
        bool relocate16 = Q16CheckBox.Checked ? true : false;
        bool relocate16_1 = Q16CheckBox.Checked ? true : false;
        bool relocate16_2 = Q16CheckBox.Checked ? true : false;
        bool relocate16_3 = Q16CheckBox.Checked ? true : false;

        bool relocate17 = Q17CheckBox.Checked ? true : false;
        bool relocate17_1 = Q17_1CheckBox.Checked ? true : false;
        bool relocate17_2 = Q17_2CheckBox.Checked ? true : false;
        bool relocate17_3 = Q17_3CheckBox.Checked ? true : false;
        bool relocate17_4 = Q17_4CheckBox.Checked ? true : false;
        bool relocate19 = Q19CheckBox.Checked ? true : false;
        bool relocate20 = Q20CheckBox.Checked ? true : false;

        using (SqlCommand cmd = new SqlCommand("update [Antibiotic] set [Q1_1]=@Q1_1, [Q1_2]=@Q1_2, [Q2]=@Q2, [Q2_1]=@Q2_1, [Q2_2]=@Q2_2, [Q3]=@Q3, [Q3_1]=@Q3_1, [Q3_2]=@Q3_2, [Q3_3]=@Q3_3, [Q4]=@Q4, [Q4_1]=@Q4_1, [Q4_2]=@Q4_2, [Q4_3]=@Q4_3, [Q5]=@Q5, [Q5_1]=@Q5_1, [Q5_2]=@Q5_2, [Q5_3]=@Q5_3, [Q6]=@Q6, [Q7]=@Q7, [Q8]=@Q8, [Q9]=@Q9, [Q10]=@Q10, [Q11]=@Q11, [Q11_t]=@Q11_t, [Q12]=@Q12, [Q12_t]=@Q12_t, [Q12_1]=@Q12_1, [Q12_2]=@Q12_2, [Q12_3]=@Q12_3, [Q12_4]=@Q12_3, [Q13]=@Q13, [Q13_t]=@Q13_t, [Q13_1]=@Q13_1, [Q14]=@Q14, [Q15]=@Q15, [Q16]=@Q16, [Q16_1]=@Q16_1, [Q16_1_t]=@Q16_1_t, [Q16_2]=@Q16_2, [Q16_3]=@Q16_3, [Q16_3_t]=@Q16_3_t, [Q17]=@Q17, [Q17_1]=@Q17_1, [Q17_2]=@Q17_2, [Q17_3]=@Q17_3, [Q17_4]=@Q17_4, [Q18]=@Q18, [Q19]=@Q19, [Q19_1]=@Q19_1, [Q19_2]=@Q19_2, [Q19_3]=@Q19_3, [Q20]=@Q20, [Q20_1]=@Q20_1 WHERE [Q_P_ID]=@Q_P_ID"))
        {
            cmd.Connection = con;
            cmd.Parameters.AddWithValue("@Q_P_ID", ID);
            cmd.Parameters.AddWithValue("@Q1_1", relocate1);
            cmd.Parameters.AddWithValue("@Q1_2", relocate1_1);
            cmd.Parameters.AddWithValue("@Q2", relocate2);
            cmd.Parameters.Add("@Q2_1", SqlDbType.NChar, 10).Value = this.TextBox17.Text;
            cmd.Parameters.Add("@Q2_2", SqlDbType.NChar, 10).Value = this.TextBox18.Text;

            cmd.Parameters.AddWithValue("@Q3", relocate3);
            cmd.Parameters.AddWithValue("@Q3_1", relocate3_1);
            cmd.Parameters.AddWithValue("@Q3_2", relocate3_2);
            cmd.Parameters.AddWithValue("@Q3_3", relocate3_3);

            //cmd.Parameters.Add("@Q3", SqlDbType.NChar, 10).Value = "0";
            //cmd.Parameters.Add("@Q3_1", SqlDbType.NChar, 10).Value = "0";
            //cmd.Parameters.Add("@Q3_2", SqlDbType.NChar, 10).Value = "0";
            //cmd.Parameters.Add("@Q3_3", SqlDbType.NChar, 10).Value = "0";

            cmd.Parameters.AddWithValue("@Q4", relocate4);
            cmd.Parameters.AddWithValue("@Q4_1", relocate4_1);
            cmd.Parameters.AddWithValue("@Q4_2", relocate4_2);
            cmd.Parameters.AddWithValue("@Q4_3", relocate4_3);
            //.Parameters.Add("@Q4", SqlDbType.NChar, 10).Value = "0";
            //cmd.Parameters.Add("@Q4_1", SqlDbType.NChar, 10).Value = "0";
            //cmd.Parameters.Add("@Q4_2", SqlDbType.NChar, 10).Value = "0";
            //cmd.Parameters.Add("@Q4_3", SqlDbType.NChar, 10).Value = "0";

            cmd.Parameters.AddWithValue("@Q5", relocate5);
            cmd.Parameters.AddWithValue("@Q5_1", relocate5_1);
            cmd.Parameters.AddWithValue("@Q5_2", relocate5_2);
            cmd.Parameters.AddWithValue("@Q5_3", relocate5_2);
            //cmd.Parameters.Add("@Q5", SqlDbType.NChar, 10).Value = "0";
            //cmd.Parameters.Add("@Q5_1", SqlDbType.NChar, 10).Value = "0";
            //cmd.Parameters.Add("@Q5_2", SqlDbType.NChar, 10).Value = "0";
            //cmd.Parameters.Add("@Q5_3", SqlDbType.NChar, 10).Value = "0";

            cmd.Parameters.AddWithValue("@Q6", relocate6);
            cmd.Parameters.AddWithValue("@Q7", relocate7);
            cmd.Parameters.AddWithValue("@Q8", relocate8);
            cmd.Parameters.AddWithValue("@Q9", relocate9);
            cmd.Parameters.AddWithValue("@Q10", relocate10);
            cmd.Parameters.AddWithValue("@Q11", relocate11);
            if (Q11CheckBox.Checked)
            {
                cmd.Parameters.Add("@Q11_t", SqlDbType.NVarChar, 50).Value = this.TextBox19.Text;
            }
            else
            {
                cmd.Parameters.Add("@Q11_t", SqlDbType.NVarChar, 50).Value = "";
            }
            cmd.Parameters.AddWithValue("@Q12", relocate12);
            cmd.Parameters.AddWithValue("@Q12_1", relocate12_1);
            cmd.Parameters.AddWithValue("@Q12_2", relocate12_2);
            cmd.Parameters.AddWithValue("@Q12_3", relocate12_3);
            cmd.Parameters.AddWithValue("@Q12_4", relocate12_4);
            if (Q12CheckBox.Checked)
            {
                cmd.Parameters.Add("@Q12_t", SqlDbType.NVarChar, 50).Value = this.TextBox20.Text;
            }
            else
            {
                cmd.Parameters.Add("@Q12_t", SqlDbType.NVarChar, 50).Value = "";
                //cmd.Parameters.Add("@Q12_1", SqlDbType.NChar, 10).Value = "0";
                //cmd.Parameters.Add("@Q12_2", SqlDbType.NChar, 10).Value = "0";
                //cmd.Parameters.Add("@Q12_3", SqlDbType.NChar, 10).Value = "0";
                //cmd.Parameters.Add("@Q12_4", SqlDbType.NChar, 10).Value = "0";

            }
            cmd.Parameters.AddWithValue("@Q13", relocate13);
            if (Q13CheckBox.Checked)
            {
                cmd.Parameters.Add("@Q13_t", SqlDbType.NVarChar, 50).Value = this.TextBox21.Text;
                cmd.Parameters.Add("@Q13_1", SqlDbType.NVarChar, 50).Value = this.TextBox28.Text;
            }
            else
            {
                cmd.Parameters.Add("@Q13_t", SqlDbType.NVarChar, 50).Value = "";
                cmd.Parameters.Add("@Q13_1", SqlDbType.NVarChar, 50).Value = "";
            }
            // cmd.Parameters.AddWithValue("@Q13", relocate29);
            // cmd.Parameters.AddWithValue("@Q13_1", relocate30);
            cmd.Parameters.AddWithValue("@Q14", relocate14);
            cmd.Parameters.AddWithValue("@Q15", relocate15);
            cmd.Parameters.AddWithValue("@Q16", relocate16);
            cmd.Parameters.AddWithValue("@Q16_1", relocate16_1);
            cmd.Parameters.AddWithValue("@Q16_2", relocate16_2);
            cmd.Parameters.AddWithValue("@Q16_3", relocate16_3);
            if (Q16CheckBox.Checked)
            {
                cmd.Parameters.Add("@Q16_1_t", SqlDbType.NVarChar, 50).Value = this.TextBox22.Text;
                cmd.Parameters.Add("@Q16_3_t", SqlDbType.NVarChar, 50).Value = this.TextBox23.Text;
            }
            else
            {
                //cmd.Parameters.Add("@Q16_1", SqlDbType.NVarChar, 50).Value = "0";
                cmd.Parameters.Add("@Q16_1_t", SqlDbType.NVarChar, 50).Value = "";
                //cmd.Parameters.Add("@Q16_2", SqlDbType.NChar, 10).Value = "0";
                //cmd.Parameters.Add("@Q16_3", SqlDbType.NVarChar, 50).Value = "0";
                cmd.Parameters.Add("@Q16_3_t", SqlDbType.NVarChar, 50).Value = "";
            }
            cmd.Parameters.AddWithValue("@Q17", relocate17);
            cmd.Parameters.AddWithValue("@Q17_1", relocate17_1);
            cmd.Parameters.AddWithValue("@Q17_2", relocate17_2);
            cmd.Parameters.AddWithValue("@Q17_3", relocate17_3);
            cmd.Parameters.AddWithValue("@Q17_4", relocate17_4);
            //cmd.Parameters.Add("@Q17", SqlDbType.NChar, 10).Value = "0";
            //cmd.Parameters.Add("@Q17_1", SqlDbType.NChar, 10).Value = "0";
            //cmd.Parameters.Add("@Q17_2", SqlDbType.NChar, 10).Value = "0";
            //cmd.Parameters.Add("@Q17_3", SqlDbType.NChar, 10).Value = "0";
            //cmd.Parameters.Add("@Q17_4", SqlDbType.NChar, 10).Value = "0";

            cmd.Parameters.AddWithValue("@Q18", relocate18);
            cmd.Parameters.AddWithValue("@Q19", relocate19);
            if (Q19CheckBox.Checked)
            {
                cmd.Parameters.Add("@Q19_1", SqlDbType.NVarChar, 50).Value = this.TextBox24.Text;
                cmd.Parameters.Add("@Q19_2", SqlDbType.NVarChar, 50).Value = this.TextBox25.Text;
                cmd.Parameters.Add("@Q19_3", SqlDbType.NVarChar, 50).Value = this.TextBox26.Text;
            }
            else
            {
                //cmd.Parameters.Add("@Q19", SqlDbType.NChar, 10).Value = "0";
                cmd.Parameters.Add("@Q19_1", SqlDbType.NVarChar, 50).Value = "";
                cmd.Parameters.Add("@Q19_2", SqlDbType.NVarChar, 50).Value = "";
                cmd.Parameters.Add("@Q19_3", SqlDbType.NVarChar, 50).Value = "";
            }
            cmd.Parameters.AddWithValue("@Q20", relocate20);
            if (Q20CheckBox.Checked)
            {
                cmd.Parameters.Add("@Q20_1", SqlDbType.NVarChar, 50).Value = this.TextBox27.Text;
            }
            else
            {
                //cmd.Parameters.Add("@Q20", SqlDbType.NChar, 10).Value = "0";
                cmd.Parameters.Add("@Q20_1", SqlDbType.NVarChar, 50).Value = "";
            }
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }


    }

    public void creatPDF()
    {
        using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString))
        {
            string ID = TextBox1.Text;
            string Name = TextBox2.Text;
            string fileName2 = string.Empty;
            string filePath2 = string.Empty;
            string getPath = string.Empty;
            string pathToStore = string.Empty;
            DateTime fileCreationDatetime2 = DateTime.Now;
            string sql = "update [PDF_info] set [P_Name]=P_Name, [PDF_CreTime]=@PDF_CreTime, [PDF_Path]=@PDF_Path WHERE [P_ID]=@P_ID";
            {
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@P_ID", ID);
                cmd.Parameters.AddWithValue("@P_Name", Name);
                cmd.Parameters.AddWithValue("@PDF_CreTime", DateTime.Now);
                fileName2 = string.Format("{0}.pdf", fileCreationDatetime2.ToString(@"yyyyMMdd") + "_" + TextBox1.Text);
                filePath2 = Server.MapPath(@"PDFs/" + fileName2);
                // System.IO.File.Move(ckName, reName); // 更改檔名 
                // System.IO.File.Move(“原始檔名“, “新檔名“); // 更改檔名
                int getPos = filePath2.LastIndexOf("\\");
                int len = filePath2.Length;
                getPath = filePath2.Substring(getPos, len - getPos);
                pathToStore = getPath.Remove(0, 1);
                cmd.Parameters.AddWithValue("@PDF_Path", pathToStore);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }
        string fileName = string.Empty;
        string filePath = string.Empty;
        DateTime fileCreationDatetime = DateTime.Now;
        fileName = string.Format(fileCreationDatetime.ToString(@"yyyyMMdd") + "_" + TextBox1.Text + ".pdf");
        filePath = Server.MapPath(@"~\Antibiotic\PDFs\") + fileName;

        var doc1 = new Document(PageSize.A4, 50, 50, 5, 10);
        Response.ContentType = "application/pdf";
        Response.ContentEncoding = System.Text.Encoding.UTF8;
        Response.AddHeader("content-disposition", "attachment; filename=" + fileName);
        //  Response.AddHeader("content-disposition", "attachment;filename=TestPage.pdf");
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        PdfWriter PdfWriter = PdfWriter.GetInstance(doc1, new FileStream(filePath, FileMode.Create));
        //字型設定
        BaseFont bfChinese = BaseFont.CreateFont(@"C:\WINDOWS\Fonts\kaiu.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
        BaseFont bfEngInt = BaseFont.CreateFont(@"C:\WINDOWS\Fonts\Arial.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
        BaseFont BaseF = BaseFont.CreateFont("C:\\Windows\\Fonts\\kaiu.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
        Font HeaderFont = new Font(bfChinese, 23);
        Font FontSize1 = new Font(bfChinese, 12);
        Font HeaderFontsmall = new Font(bfEngInt, 22);
        Font ChFont = new Font(bfChinese, 12);
        Font ChFont1 = new Font(bfChinese, 11);
        Font fontCh = new Font(BaseF, 14);

        PdfPTable table = new PdfPTable(new float[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 });
        table.TotalWidth = 550f;
        table.LockedWidth = true;
        PdfPCell Header1 = new PdfPCell(new Phrase("林新醫療社團法人 烏日林新醫院\n管 制 性 抗 生 素 申 請 表", HeaderFont));
        Header1.Colspan = 15;
        Header1.MinimumHeight = 20;
        Header1.Border = iTextSharp.text.Rectangle.NO_BORDER;
        Header1.HorizontalAlignment = 1; table.AddCell(Header1);

        PdfPCell space = new PdfPCell(new Phrase(" ", new Font(Font.FontFamily.HELVETICA, 5f, Font.BOLD)));
        space.Colspan = 15;
        space.BorderWidth = 0;
        table.AddCell(space);

        //space.MinimumHeight = 5; //設定間距高度
        PdfPCell row1 = new PdfPCell(new Phrase("姓名:" + TextBox2.Text, fontCh));
        row1.Colspan = 3;
        row1.MinimumHeight = 10;
        row1.BorderWidthTop = 0.1f;
        row1.BorderWidthLeft = 0.1f;
        row1.BorderWidthRight = 0;
        row1.BorderWidthBottom = 0.1f;
        table.AddCell(row1);

        PdfPTable Images2 = new PdfPTable(1);
        Images2.TotalWidth = 550f;
        Images2.LockedWidth = true;
        string imageURL2 = Server.MapPath(".") + "/images/checked16.png";
        iTextSharp.text.Image image2 = iTextSharp.text.Image.GetInstance(imageURL2);

        PdfPCell row12 = new PdfPCell();
        row12.Colspan = 2;
        row12.MinimumHeight = 10;
        row12.BorderWidthTop = 0.1f;
        row12.BorderWidthLeft = 0.1f;
        row12.BorderWidthRight = 0;
        row12.BorderWidthBottom = 0.1f;
        Paragraph T1 = new Paragraph();
        T1.Add(new Phrase("  "));
        if (ManCheckBox.Checked)
        {
            T1.Add(new Chunk(image2, 0, -3));
            T1.Add(new Phrase("男", fontCh));
        }
        else
        {
            T1.Add(new Phrase("□男", fontCh));
        }
        T1.Add(new Phrase("  "));
        if (GirlCheckBox.Checked)
        {
            T1.Add(new Chunk(image2, 0, -3));
            T1.Add(new Phrase("女", fontCh));
        }
        else
        {
            T1.Add(new Phrase("□女", fontCh));
        }
        row12.AddElement(T1);
        table.AddCell(row12);

        PdfPCell row13 = new PdfPCell(new Phrase("年齡:" + TextBox30.Text, fontCh));
        row13.Colspan = 2;
        row13.MinimumHeight = 10;
        row13.BorderWidthTop = 0.1f;
        row13.BorderWidthLeft = 0.1f;
        row13.BorderWidthRight = 0;
        row13.BorderWidthBottom = 0;
        table.AddCell(row13);

        PdfPCell row14 = new PdfPCell(new Phrase("病歷號碼:" + TextBox1.Text, fontCh));
        row14.Colspan = 4;
        row14.MinimumHeight = 10;
        row14.BorderWidthTop = 0.1f;
        row14.BorderWidthLeft = 0.1f;
        row14.BorderWidthRight = 0;
        row14.BorderWidthBottom = 0;
        table.AddCell(row14);

        PdfPCell row15 = new PdfPCell(new Phrase("床號:" + TextBox3.Text, fontCh));
        row15.Colspan = 4;
        row15.MinimumHeight = 10;
        row15.BorderWidthTop = 0.1f;
        row15.BorderWidthLeft = 0.1f;
        row15.BorderWidthRight = 0.1f;
        row15.BorderWidthBottom = 0;
        table.AddCell(row15);


        PdfPTable table2 = new PdfPTable(new float[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 });
        table2.TotalWidth = 550f;
        table2.LockedWidth = true;

        PdfPCell row2 = new PdfPCell(new Phrase("申請醫師簽名、蓋章:" + TextBox5.Text, fontCh));
        row2.Colspan = 5;
        row2.MinimumHeight = 10;
        row2.BorderWidthTop = 0.1f;
        row2.BorderWidthLeft = 0.1f;
        row2.BorderWidthRight = 0;
        row2.BorderWidthBottom = 0.1f;
        row2.HorizontalAlignment = 0; //文字位置 0=Left, 1=Centre, 2=Right
        table2.AddCell(row2);

        PdfPCell row21 = new PdfPCell(new Phrase("主治醫師:" + TextBox4.Text, fontCh));
        row21.Colspan = 4;
        row21.MinimumHeight = 10;
        row21.BorderWidthTop = 0.1f;
        row21.BorderWidthLeft = 0.1f;
        row21.BorderWidthRight = 0;
        row21.BorderWidthBottom = 0.1f;
        row21.HorizontalAlignment = 0;
        table2.AddCell(row21);

        PdfPCell row22 = new PdfPCell(new Phrase("藥師:" + TextBox6.Text, fontCh));
        row22.Colspan = 4;
        row22.MinimumHeight = 10;
        row22.BorderWidthTop = 0.1f;
        row22.BorderWidthLeft = 0.1f;
        row22.BorderWidthRight = 0.1f;
        row22.BorderWidthBottom = 0.1f;
        row22.HorizontalAlignment = 0;
        table2.AddCell(row22);

        //Ootput DataGrideView  
        PdfPTable ptb = new PdfPTable(new float[] { 1, 1, 1, 1, 1, 1 });
        ptb.TotalWidth = 550f;
        ptb.LockedWidth = true;
        //表格標題
        string[] TitleTx = new string[] { "抗生素名稱", "劑量", "用法", "開始使用日", "續用", "預估停止日" };
        for (int h = 0; h < TitleTx.Length; h++)
        {
            PdfPCell ptbhd = new PdfPCell(new Phrase(TitleTx[h], fontCh));
            ptbhd.MinimumHeight = 10;
            ptbhd.BorderWidthTop = 0.1f;
            ptbhd.BorderWidthLeft = 0.1f;
            if (h == 0) { ptbhd.BorderWidthLeft = 0.1f; }
            ptbhd.BorderWidthRight = 0.1f;
            if (h == 6) { ptbhd.BorderWidthRight = 0.1f; }
            ptbhd.BorderWidthBottom = 0.1f;
            ptbhd.HorizontalAlignment = 1;
            ptb.AddCell(ptbhd);
        }

        //讀取GridView Data //表格內文
        for (int j = 0; j < 5; j++)
        {
            for (int k = 0; k < 6; k++)
            {
                GridView1.Rows[j].Cells[k].Text = GridView1.Rows[j].Cells[k].Text.Replace("&nbsp;", " ");
                ptb.AddCell(new Phrase(GridView1.Rows[j].Cells[k].Text, fontCh));
            }
        }

        PdfPTable table3 = new PdfPTable(new float[] { 1 });
        table3.TotalWidth = 550f;
        table3.LockedWidth = true;
        PdfPCell T3 = new PdfPCell();
        T3.MinimumHeight = 10;
        T3.BorderWidthTop = 0;
        T3.BorderWidthLeft = 0.1f;
        T3.BorderWidthRight = 0.1f;
        T3.BorderWidthBottom = 0.1f;
        Paragraph T3_1 = new Paragraph();
        T3_1.Add(new Phrase("相關數值", fontCh));
        T3_1.Add(new Phrase(" ", ChFont));
        T3_1.Add(new Phrase("體重:" + TextBox7.Text + "公斤", ChFont));
        T3_1.Add(new Phrase(" ", ChFont));
        T3_1.Add(new Phrase("Creatinine:" + TextBox8.Text + "mg/dL", ChFont));
        T3_1.Add(new Phrase(" ", ChFont));
        T3_1.Add(new Phrase("BUN:" + TextBox9.Text + "mg/dL", ChFont));
        T3_1.Add(new Phrase(" ", ChFont));
        T3_1.Add(new Phrase("GOT:" + TextBox10.Text + "U/I", ChFont));
        T3_1.Add(new Phrase(" ", ChFont));
        T3_1.Add(new Phrase("GPT:" + TextBox11.Text + "U/L", ChFont));
        T3.AddElement(T3_1);
        table3.AddCell(T3);

        PdfPTable Images = new PdfPTable(1);
        Images.TotalWidth = 550f;
        Images.LockedWidth = true;
        string imageURL = Server.MapPath(".") + "/images/checkbox14.png";
        iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(imageURL);

        PdfPTable table4 = new PdfPTable(new float[] { 1 });
        table4.TotalWidth = 550f;
        table4.LockedWidth = true;
        PdfPCell L1 = new PdfPCell();
        L1.MinimumHeight = 10; L1.BorderWidthTop = 0;
        L1.BorderWidthLeft = 0; L1.BorderWidthRight = 0;
        L1.BorderWidthBottom = 0f;
        Paragraph L1_1 = new Paragraph();
        L1_1.Add(new Phrase("甲、缺乏培養資料(請將檢體送檢!!)", ChFont));
        if (Q1CheckBox.Checked)
        {
            L1_1.Add(new Chunk(image, 0, -4));
            L1_1.Add(new Phrase("已培養但未有結果", ChFont1));
        }
        else
        {
            L1_1.Add(new Phrase("□已培養但未有結果", ChFont1));
        }
        if (Q1_1CheckBox.Checked)
        {
            L1_1.Add(new Chunk(image, 0, -4));
            L1_1.Add(new Phrase("有長但ST未完成", ChFont1));
        }
        else
        {
            L1_1.Add(new Phrase("□有長但ST未完成", ChFont1));
        }
        L1.AddElement(L1_1);
        table4.AddCell(L1);

        PdfPTable table5 = new PdfPTable(new float[] { 1 });
        table5.TotalWidth = 550f;
        table5.LockedWidth = true;
        PdfPCell Q2 = new PdfPCell();
        Q2.MinimumHeight = 10; Q2.BorderWidthTop = 0;
        Q2.BorderWidthLeft = 0; Q2.BorderWidthRight = 0;
        Q2.BorderWidthBottom = 0f;
        Paragraph Q2_1 = new Paragraph();
        if (Q2CheckBox.Checked)
        {
            Q2_1.Add(new Chunk(image, 0, -4));
            Q2_1.Add(new Phrase("依經驗認為治療需要", ChFont1));
        }
        else
        {
            Q2_1.Add(new Phrase("□依經驗認為治療需要", ChFont1));
        }
        Q2_1.Add(new Phrase("    "));
        Q2_1.Add(new Phrase("病患原在性疾病" + TextBox17.Text, ChFont1));
        Q2_1.Add(new Phrase("  "));
        Q2_1.Add(new Phrase("感染性疾病名稱" + TextBox18.Text + "(必填)", ChFont1));
        Q2.AddElement(Q2_1);
        table5.AddCell(Q2);

        PdfPTable table6 = new PdfPTable(new float[] { 1 });
        table6.TotalWidth = 550f;
        table6.LockedWidth = true;
        PdfPCell L2 = new PdfPCell(new Phrase("   適用理由(請詳實勾選):", ChFont1));
        L2.Colspan = 4; L2.MinimumHeight = 10;
        L2.BorderWidthTop = 0; L2.BorderWidthLeft = 0;
        L2.BorderWidthRight = 0; L2.BorderWidthBottom = 0f;
        L2.HorizontalAlignment = 0;
        table6.AddCell(L2);

        PdfPTable table7 = new PdfPTable(new float[] { 1 });
        table7.TotalWidth = 550f;
        table7.LockedWidth = true;
        PdfPCell Q3 = new PdfPCell();
        Q3.MinimumHeight = 10; Q3.BorderWidthTop = 0;
        Q3.BorderWidthLeft = 0; Q3.BorderWidthRight = 0;
        Q3.BorderWidthBottom = 0f;
        Paragraph Q3_1 = new Paragraph();
        Q3_1.Add(new Phrase("    "));
        if (Q3CheckBox.Checked)
        {
            Q3_1.Add(new Chunk(image, -1, -4));
            Q3_1.Add(new Phrase("感染病情嚴重者:", ChFont1));
        }
        else
        {
            Q3_1.Add(new Phrase("□感染病情嚴重者:", ChFont1));
        }
        if (Q3_1CheckBox.Checked)
        {
            Q3_1.Add(new Chunk(image, 0, -4));
            Q3_1.Add(new Phrase("敗血症或敗血性休克", ChFont1));
        }
        else
        {
            Q3_1.Add(new Phrase("□敗血症或敗血性休克", ChFont1));
        }
        if (Q3_2CheckBox.Checked)
        {
            Q3_1.Add(new Chunk(image, 0, -4));
            Q3_1.Add(new Phrase("中樞神經感染", ChFont1));
        }
        else
        {
            Q3_1.Add(new Phrase("□中樞神經感染", ChFont1));
        }
        if (Q3_3CheckBox.Checked)
        {
            Q3_1.Add(new Chunk(image, 0, -4));
            Q3_1.Add(new Phrase("使用呼吸器者", ChFont1));
        }
        else
        {
            Q3_1.Add(new Phrase("□使用呼吸器者", ChFont1));
        }
        Q3.AddElement(Q3_1);
        table7.AddCell(Q3);

        PdfPTable table8 = new PdfPTable(new float[] { 1 });
        table8.TotalWidth = 550f;
        table8.LockedWidth = true;
        PdfPCell Q4 = new PdfPCell();
        Q4.MinimumHeight = 10; Q4.BorderWidthTop = 0;
        Q4.BorderWidthLeft = 0; Q4.BorderWidthRight = 0;
        Q4.BorderWidthBottom = 0f;
        Paragraph Q4_2 = new Paragraph();
        Q4_2.Add(new Phrase("    "));
        if (Q4CheckBox.Checked)
        {
            Q4_2.Add(new Chunk(image, -1, -4));
            Q4_2.Add(new Phrase("免疫狀態不良者:", ChFont1));
        }
        else
        {
            Q4_2.Add(new Phrase("□免疫狀態不良者:", ChFont1));
        }
        if (Q4_1CheckBox.Checked)
        {
            Q4_2.Add(new Chunk(image, 0, -4));
            Q4_2.Add(new Phrase("接受免疫抑制劑", ChFont1));
        }
        else
        {
            Q4_2.Add(new Phrase("□接受免疫抑制劑", ChFont1));
        }
        if (Q4_2CheckBox.Checked)
        {
            Q4_2.Add(new Chunk(image, 0, -4));
            Q4_2.Add(new Phrase("接受抗癌化學療法", ChFont1));
        }
        else
        {
            Q4_2.Add(new Phrase("□接受抗癌化學療法", ChFont1));
        }
        if (Q4_3CheckBox.Checked)
        {
            Q4_2.Add(new Chunk(image, 0, -4));
            Q4_2.Add(new Phrase("白血球≦1000/cumm或多核球≦500/cumm", ChFont1));
        }
        else
        {
            Q4_2.Add(new Phrase("□白血球≦1000/cumm或多核球≦500/cumm", ChFont1));
        }
        Q4.AddElement(Q4_2);
        table8.AddCell(Q4);
        /*
        PdfPTable table9 = new PdfPTable(new float[] { 1 });
        table9.TotalWidth = 550f;
        table9.LockedWidth = true;
        PdfPCell Q4_1 = new PdfPCell();
        Q4_1.MinimumHeight = 10;        Q4_1.BorderWidthTop = 0;
        Q4_1.BorderWidthLeft = 0;        Q4_1.BorderWidthRight = 0;
        Q4_1.BorderWidthBottom = 0f;
        Paragraph Q4_1_1 = new Paragraph();
        Q4_1_1.Add(new Phrase("                                        "));
        if (Q4_3CheckBox.Checked)
        {
            Q4_1_1.Add(new Chunk(image, 0, 0));
            Q4_1_1.Add(new Phrase("白血球≦1000/cumm或多核球≦500/cumm", ChFont1));
        }
        else
        {
            Q4_1_1.Add(new Phrase("□白血球≦1000/cumm或多核球≦500/cumm", ChFont1));
        }
        Q4_1.AddElement(Q4_1_1);
        table9.AddCell(Q4_1);*/

        PdfPTable table10 = new PdfPTable(new float[] { 1 });
        table10.TotalWidth = 550f;
        table10.LockedWidth = true;
        PdfPCell Q5 = new PdfPCell();
        Q5.MinimumHeight = 10; Q5.BorderWidthTop = 0;
        Q5.BorderWidthLeft = 0; Q5.BorderWidthRight = 0;
        Q5.BorderWidthBottom = 0f;
        Paragraph Q5_1 = new Paragraph();
        Q5_1.Add(new Phrase("    "));
        if (Q5CheckBox.Checked)
        {
            Q5_1.Add(new Chunk(image, -1, -4));
            Q5_1.Add(new Phrase("手術中發現有明顯感染病灶者:", ChFont1));
        }
        else
        {
            Q5_1.Add(new Phrase("□手術中發現有明顯感染病灶者:", ChFont1));
        }
        if (Q5_1CheckBox.Checked)
        {
            Q5_1.Add(new Chunk(image, 0, -4));
            Q5_1.Add(new Phrase("臟器穿孔", ChFont1));
        }
        else
        {
            Q5_1.Add(new Phrase("□臟器穿孔", ChFont1));
        }
        if (Q5_2CheckBox.Checked)
        {
            Q5_1.Add(new Chunk(image, 0, -4));
            Q5_1.Add(new Phrase("嚴重汙染傷口", ChFont1));
        }
        else
        {
            Q5_1.Add(new Phrase("□嚴重汙染傷口", ChFont1));
        }
        if (Q5_3CheckBox.Checked)
        {
            Q5_1.Add(new Chunk(image, 0, -4));
            Q5_1.Add(new Phrase("手術發生感染併發症", ChFont1));
        }
        else
        {
            Q5_1.Add(new Phrase("□手術發生感染併發症", ChFont1));
        }
        Q5.AddElement(Q5_1);
        table10.AddCell(Q5);

        PdfPTable table11 = new PdfPTable(new float[] { 1 });
        table11.TotalWidth = 550f;
        table11.LockedWidth = true;
        PdfPCell Q6 = new PdfPCell();
        Q6.MinimumHeight = 10; Q6.BorderWidthTop = 0;
        Q6.BorderWidthLeft = 0; Q6.BorderWidthRight = 0;
        Q6.BorderWidthBottom = 0f;
        Paragraph Q6_1 = new Paragraph();
        Q6_1.Add(new Phrase("    "));
        if (Q6CheckBox.Checked)
        {
            Q6_1.Add(new Chunk(image, -1, -4));
            Q6_1.Add(new Phrase("疑似感染之早產兒及兩個月內之新生兒", ChFont1));
        }
        else
        {
            Q6_1.Add(new Phrase("□疑似感染之早產兒及兩個月內之新生兒:", ChFont1));
        }
        Q6.AddElement(Q6_1);
        table11.AddCell(Q6);

        PdfPTable table12 = new PdfPTable(new float[] { 1 });
        table12.TotalWidth = 550f;
        table12.LockedWidth = true;
        PdfPCell Q7 = new PdfPCell();
        Q7.MinimumHeight = 10; Q7.BorderWidthTop = 0;
        Q7.BorderWidthLeft = 0; Q7.BorderWidthRight = 0;
        Q7.BorderWidthBottom = 0f;
        Paragraph Q7_1 = new Paragraph();
        Q7_1.Add(new Phrase("    "));
        if (Q7CheckBox.Checked)
        {
            Q7_1.Add(new Chunk(image, -1, -4));
            Q7_1.Add(new Phrase("嬰幼兒(2m-5y)疑似感染疾病，在使用第一線抗生素72小時仍無明顯療效者", ChFont1));
        }
        else
        {
            Q7_1.Add(new Phrase("□嬰幼兒(2m-5y)疑似感染疾病，在使用第一線抗生素72小時仍無明顯療效者", ChFont1));
        }
        Q7.AddElement(Q7_1);
        table12.AddCell(Q7);

        PdfPTable table13 = new PdfPTable(new float[] { 1 });
        table13.TotalWidth = 550f;
        table13.LockedWidth = true;
        PdfPCell Q8 = new PdfPCell();
        Q8.MinimumHeight = 10; Q8.BorderWidthTop = 0;
        Q8.BorderWidthLeft = 0; Q8.BorderWidthRight = 0;
        Q8.BorderWidthBottom = 0f;
        Paragraph Q8_1 = new Paragraph();
        Q8_1.Add(new Phrase("    "));
        if (Q8CheckBox.Checked)
        {
            Q8_1.Add(new Chunk(image, -1, -4));
            Q8_1.Add(new Phrase("脾臟切除有不明熱", ChFont1));
        }
        else
        {
            Q8_1.Add(new Phrase("□脾臟切除有不明熱", ChFont1));
        }
        Q8.AddElement(Q8_1);
        table13.AddCell(Q8);

        PdfPTable table14 = new PdfPTable(new float[] { 1 });
        table14.TotalWidth = 550f;
        table14.LockedWidth = true;
        PdfPCell Q9 = new PdfPCell();
        Q9.MinimumHeight = 10; Q9.BorderWidthTop = 0;
        Q9.BorderWidthLeft = 0; Q9.BorderWidthRight = 0;
        Q9.BorderWidthBottom = 0f;
        Paragraph Q9_1 = new Paragraph();
        Q9_1.Add(new Phrase("    "));
        if (Q9CheckBox.Checked)
        {
            Q9_1.Add(new Chunk(image, -1, -4));
            Q9_1.Add(new Phrase("發生明確嚴重院內感染者", ChFont1));
        }
        else
        {
            Q9_1.Add(new Phrase("□發生明確嚴重院內感染者", ChFont1));
        }
        Q9.AddElement(Q9_1);
        table14.AddCell(Q9);

        PdfPTable table15 = new PdfPTable(new float[] { 1 });
        table15.TotalWidth = 550f;
        table15.LockedWidth = true;
        PdfPCell Q10 = new PdfPCell();
        Q10.MinimumHeight = 10; Q10.BorderWidthTop = 0;
        Q10.BorderWidthLeft = 0; Q10.BorderWidthRight = 0;
        Q10.BorderWidthBottom = 0f;
        Paragraph Q10_1 = new Paragraph();
        Q10_1.Add(new Phrase("    "));
        if (Q10CheckBox.Checked)
        {
            Q10_1.Add(new Chunk(image, -1, -4));
            Q10_1.Add(new Phrase("常有厭氧菌與非厭氧菌混和感染之組織部位感染(如糖尿病足部壞疽併感染、骨盆腔內感染)", ChFont1));
        }
        else
        {
            Q10_1.Add(new Phrase("□常有厭氧菌與非厭氧菌混和感染之組織部位感染(如糖尿病足部壞疽併感染、骨盆腔內感染)", ChFont1));
        }
        Q10.AddElement(Q10_1);
        table15.AddCell(Q10);
        /*
        PdfPTable table16 = new PdfPTable(new float[] { 1, 1 });
        table16.TotalWidth = 550f;
        table16.LockedWidth = true;
        PdfPCell Q10_1_1 = new PdfPCell(new Phrase("    內感染)", ChFont1));
        Q10_1_1.Colspan = 4;        Q10_1_1.MinimumHeight = 10;
        Q10_1_1.BorderWidthTop = 0;        Q10_1_1.BorderWidthLeft = 0;
        Q10_1_1.BorderWidthRight = 0;        Q10_1_1.BorderWidthBottom = 0f;
        Q10_1_1.HorizontalAlignment = 0;
        table16.AddCell(Q10_1_1);*/

        PdfPTable table17 = new PdfPTable(new float[] { 1 });
        table17.TotalWidth = 550f;
        table17.LockedWidth = true;
        PdfPCell Q11 = new PdfPCell();
        Q11.MinimumHeight = 10; Q11.BorderWidthTop = 0;
        Q11.BorderWidthLeft = 0; Q11.BorderWidthRight = 0;
        Q11.BorderWidthBottom = 0f;
        Paragraph Q11_1 = new Paragraph();
        Q11_1.Add(new Phrase("    "));
        if (Q11CheckBox.Checked)
        {
            Q11_1.Add(new Chunk(image, -1, -4));
            Q11_1.Add(new Phrase("其他特殊理由，請說明" + TextBox19.Text, ChFont1));
        }
        else
        {
            Q11_1.Add(new Phrase("□其他特殊理由，請說明" + TextBox19.Text, ChFont1));
        }
        Q11.AddElement(Q11_1);
        table17.AddCell(Q11);

        PdfPTable table18 = new PdfPTable(new float[] { 1 });
        table18.TotalWidth = 550f;
        table18.LockedWidth = true;
        PdfPCell Q12 = new PdfPCell();
        Q12.MinimumHeight = 10; Q12.BorderWidthTop = 0;
        Q12.BorderWidthLeft = 0; Q12.BorderWidthRight = 0;
        Q12.BorderWidthBottom = 0f;
        Paragraph Q12_1 = new Paragraph();
        if (Q12CheckBox.Checked)
        {
            Q12_1.Add(new Chunk(image, -1, -4));
            Q12_1.Add(new Phrase("預防性抗生素使用要請說明  侵入性步驟或手術名稱" + TextBox20.Text, ChFont1));
        }
        else
        {
            Q12_1.Add(new Phrase("□預防性抗生素使用要請說明  侵入性步驟或手術名稱" + TextBox20.Text, ChFont1));
        }
        Q12.AddElement(Q12_1);
        table18.AddCell(Q12);

        PdfPTable table19 = new PdfPTable(new float[] { 1 });
        table19.TotalWidth = 550f;
        table19.LockedWidth = true;
        PdfPCell Q12_1_1 = new PdfPCell();
        Q12_1_1.MinimumHeight = 10; Q12_1_1.BorderWidthTop = 0;
        Q12_1_1.BorderWidthLeft = 0; Q12_1_1.BorderWidthRight = 0;
        Q12_1_1.BorderWidthBottom = 0f;
        Paragraph Q12_1_1_1 = new Paragraph();
        Q12_1_1_1.Add(new Phrase("   手術傷口分類:", ChFont1));
        if (Q12_1CheckBox.Checked)
        {
            Q12_1_1_1.Add(new Chunk(image, 0, -4));
            Q12_1_1_1.Add(new Phrase("清潔性", ChFont1));
        }
        else
        {
            Q12_1_1_1.Add(new Phrase("□清潔性", ChFont1));
        }
        if (Q12_2CheckBox.Checked)
        {
            Q12_1_1_1.Add(new Chunk(image, 0, -4));
            Q12_1_1_1.Add(new Phrase("清潔但易受汙染", ChFont1));
        }
        else
        {
            Q12_1_1_1.Add(new Phrase("□清潔但易受汙染", ChFont1));
        }
        if (Q12_3CheckBox.Checked)
        {
            Q12_1_1_1.Add(new Chunk(image, 0, -4));
            Q12_1_1_1.Add(new Phrase("污染性", ChFont1));
        }
        else
        {
            Q12_1_1_1.Add(new Phrase("□污染性", ChFont1));
        }
        if (Q12_4CheckBox.Checked)
        {
            Q12_1_1_1.Add(new Chunk(image, 0, -4));
            Q12_1_1_1.Add(new Phrase("骯髒性", ChFont1));
        }
        else
        {
            Q12_1_1_1.Add(new Phrase("□骯髒性", ChFont1));
        }
        Q12_1_1.AddElement(Q12_1_1_1);
        table19.AddCell(Q12_1_1);

        PdfPTable table20 = new PdfPTable(new float[] { 1 });
        table20.TotalWidth = 550f;
        table20.LockedWidth = true;
        PdfPCell Q13 = new PdfPCell();
        Q13.MinimumHeight = 10; Q13.BorderWidthTop = 0;
        Q13.BorderWidthLeft = 0; Q13.BorderWidthRight = 0;
        Q13.BorderWidthBottom = 0f;
        Paragraph Q13_1 = new Paragraph();
        if (Q13CheckBox.Checked)
        {
            Q13_1.Add(new Chunk(image, -1, -4));
            Q13_1.Add(new Phrase("經感染科專科醫師會診建議:" + TextBox21.Text + "會診日期:" + TextBox28.Text, ChFont1));
        }
        else
        {
            Q13_1.Add(new Phrase("□經感染科專科醫師會診建議:" + TextBox21.Text + "會診日期:" + TextBox28.Text, ChFont1));
        }
        Q13.AddElement(Q13_1);
        table20.AddCell(Q13);

        PdfPTable table21 = new PdfPTable(new float[] { 1, 1 });
        table21.TotalWidth = 550f;
        table21.LockedWidth = true;
        PdfPCell L3 = new PdfPCell(new Phrase("乙、有培養資料證明治療需要", ChFont));
        L3.Colspan = 4; L3.MinimumHeight = 10;
        L3.BorderWidthTop = 0; L3.BorderWidthLeft = 0;
        L3.BorderWidthRight = 0; L3.BorderWidthBottom = 0;
        L3.HorizontalAlignment = 0;
        table21.AddCell(L3);

        PdfPTable table22 = new PdfPTable(new float[] { 1 });
        table22.TotalWidth = 550f;
        table22.LockedWidth = true;
        PdfPCell Q14 = new PdfPCell();
        Q14.MinimumHeight = 10; Q14.BorderWidthTop = 0;
        Q14.BorderWidthLeft = 0; Q14.BorderWidthRight = 0;
        Q14.BorderWidthBottom = 0f;
        Paragraph Q14_1 = new Paragraph();
        if (Q14CheckBox.Checked)
        {
            Q14_1.Add(new Chunk(image, -1, -4));
            Q14_1.Add(new Phrase("已有結果不必換藥", ChFont1));
        }
        else
        {
            Q14_1.Add(new Phrase("□已有結果不必換藥", ChFont1));
        }
        Q14.AddElement(Q14_1);
        table22.AddCell(Q14);

        PdfPTable table23 = new PdfPTable(new float[] { 1 });
        table23.TotalWidth = 550f;
        table23.LockedWidth = true;
        PdfPCell Q15 = new PdfPCell();
        Q15.MinimumHeight = 10; Q15.BorderWidthTop = 0;
        Q15.BorderWidthLeft = 0; Q15.BorderWidthRight = 0;
        Q15.BorderWidthBottom = 0f;
        Paragraph Q15_1 = new Paragraph();
        if (Q15CheckBox.Checked)
        {
            Q15_1.Add(new Chunk(image, -1, -4));
            Q15_1.Add(new Phrase("使用第一線抗生素超過72小時，經微生物培養及藥物敏感試驗證實對第一線抗生素具抗藥性，確實需要使用者", ChFont1));
        }
        else
        {
            Q15_1.Add(new Phrase("□使用第一線抗生素超過72小時，經微生物培養及藥物敏感試驗證實對第一線抗生素具抗藥性，確實需要使用者", ChFont1));
        }
        Q15.AddElement(Q15_1);
        table23.AddCell(Q15);
        /*
        PdfPTable table24 = new PdfPTable(new float[] { 1, 1 });
        table24.TotalWidth = 550f;
        table24.LockedWidth = true;
        PdfPCell Q15_2 = new PdfPCell(new Phrase("  抗藥性，確實需要使用者。", ChFont1));
        Q15_2.Colspan = 4;        Q15_2.MinimumHeight = 10;
        Q15_2.BorderWidthTop = 0;        Q15_2.BorderWidthLeft = 0;
        Q15_2.BorderWidthRight = 0;        Q15_2.BorderWidthBottom = 0f;
        Q15_2.HorizontalAlignment = 0;
        table24.AddCell(Q15_2);*/

        PdfPTable table25 = new PdfPTable(new float[] { 1 });
        table25.TotalWidth = 550f;
        table25.LockedWidth = true;
        PdfPCell Q16 = new PdfPCell();
        Q16.MinimumHeight = 10; Q16.BorderWidthTop = 0;
        Q16.BorderWidthLeft = 0; Q16.BorderWidthRight = 0;
        Q16.BorderWidthBottom = 0f;
        Paragraph Q16_1 = new Paragraph();
        if (Q16CheckBox.Checked)
        {
            Q16_1.Add(new Chunk(image, -1, -4));
            Q16_1.Add(new Phrase("培養所得抗生素敏感性試驗證實欲申請藥及其他藥物均有效，但是", ChFont1));
        }
        else
        {
            Q16_1.Add(new Phrase("□培養所得抗生素敏感性試驗證實欲申請藥及其他藥物均有效，但是", ChFont1));
        }
        Q16.AddElement(Q16_1);
        table25.AddCell(Q16);

        PdfPTable table26 = new PdfPTable(new float[] { 1 });
        table26.TotalWidth = 550f;
        table26.LockedWidth = true;
        PdfPCell Q16_2 = new PdfPCell();
        Q16_2.MinimumHeight = 10; Q16_2.BorderWidthTop = 0;
        Q16_2.BorderWidthLeft = 0; Q16_2.BorderWidthRight = 0;
        Q16_2.BorderWidthBottom = 0f;
        Paragraph Q16_2_1 = new Paragraph();
        Q16_2_1.Add(new Phrase("    "));
        if (Q16_1CheckBox.Checked)
        {
            Q16_2_1.Add(new Chunk(image, -1, -4));
            Q16_2_1.Add(new Phrase("其他藥物臨床不具療效或不適用(請說明陪養菌種與日期)" + TextBox22.Text, ChFont1));
        }
        else
        {
            Q16_2_1.Add(new Phrase("□其他藥物臨床不具療效或不適用(請說明陪養菌種與日期)" + TextBox22.Text, ChFont1));
        }
        Q16_2.AddElement(Q16_2_1);
        table26.AddCell(Q16_2);

        PdfPTable table27 = new PdfPTable(new float[] { 1 });
        table27.TotalWidth = 550f;
        table27.LockedWidth = true;
        PdfPCell Q16_3 = new PdfPCell();
        Q16_3.MinimumHeight = 10; Q16_3.BorderWidthTop = 0;
        Q16_3.BorderWidthLeft = 0; Q16_3.BorderWidthRight = 0;
        Q16_3.BorderWidthBottom = 0f;
        Paragraph Q16_3_1 = new Paragraph();
        Q16_3_1.Add(new Phrase("    "));
        if (Q16_2CheckBox.Checked)
        {
            Q16_3_1.Add(new Chunk(image, -1, -4));
            Q16_3_1.Add(new Phrase("其他藥物造成嚴重副作用需停用，而欲申請之藥為最適當之替代品，請說明藥物使用情形、種類、使用時", ChFont1));
        }
        else
        {
            Q16_3_1.Add(new Phrase("□其他藥物造成嚴重副作用需停用，而欲申請之藥為最適當之替代品，請說明藥物使用情形、種類、使用時", ChFont1));
        }
        Q16_3.AddElement(Q16_3_1);
        table27.AddCell(Q16_3);

        PdfPTable table28 = new PdfPTable(new float[] { 1, 1 });
        table28.TotalWidth = 550f;
        table28.LockedWidth = true;
        PdfPCell Q16_3_2 = new PdfPCell(new Phrase("      間及副作用", ChFont1));
        Q16_3_2.Colspan = 4; Q16_3_2.MinimumHeight = 10;
        Q16_3_2.BorderWidthTop = 0; Q16_3_2.BorderWidthLeft = 0;
        Q16_3_2.BorderWidthRight = 0; Q16_3_2.BorderWidthBottom = 0f;
        Q16_3_2.HorizontalAlignment = 0;
        table28.AddCell(Q16_3_2);

        PdfPTable table29 = new PdfPTable(new float[] { 1 });
        table29.TotalWidth = 550f;
        table29.LockedWidth = true;
        PdfPCell Q16_4 = new PdfPCell();
        Q16_4.MinimumHeight = 10; Q16_4.BorderWidthTop = 0;
        Q16_4.BorderWidthLeft = 0; Q16_4.BorderWidthRight = 0;
        Q16_4.BorderWidthBottom = 0f;
        Paragraph Q16_4_1 = new Paragraph();
        Q16_4_1.Add(new Phrase("    "));
        if (Q16_3CheckBox.Checked)
        {
            Q16_4_1.Add(new Chunk(image, -1, -4));
            Q16_4_1.Add(new Phrase("其他特殊理由，請說明" + TextBox23.Text, ChFont1));
        }
        else
        {
            Q16_4_1.Add(new Phrase("□其他特殊理由，請說明" + TextBox23.Text, ChFont1));
        }
        Q16_4.AddElement(Q16_4_1);
        table29.AddCell(Q16_4);

        PdfPTable table30 = new PdfPTable(new float[] { 1, 1 });
        table30.TotalWidth = 550f;
        table30.LockedWidth = true;
        PdfPCell L4 = new PdfPCell(new Phrase("丙、住院後出現嚴重院內感染者", ChFont));
        L4.Colspan = 4; L4.MinimumHeight = 10; L4.BorderWidthTop = 0;
        L4.BorderWidthLeft = 0; L4.BorderWidthRight = 0; L4.BorderWidthBottom = 0;
        L4.HorizontalAlignment = 0;
        table30.AddCell(L4);

        PdfPTable table31 = new PdfPTable(new float[] { 1 });
        table31.TotalWidth = 550f;
        table31.LockedWidth = true;
        PdfPCell Q17 = new PdfPCell();
        Q17.MinimumHeight = 10; Q17.BorderWidthTop = 0;
        Q17.BorderWidthLeft = 0; Q17.BorderWidthRight = 0;
        Q17.BorderWidthBottom = 0f;
        Paragraph Q17_1 = new Paragraph();
        if (Q17CheckBox.Checked)
        {
            Q17_1.Add(new Chunk(image, -1, -4));
            Q17_1.Add(new Phrase("有敗血症，經血液培養證實者", ChFont1));
        }
        else
        {
            Q17_1.Add(new Phrase("□有敗血症，經血液培養證實者", ChFont1));
        }
        if (Q17_1CheckBox.Checked)
        {
            Q17_1.Add(new Chunk(image, 0, -4));
            Q17_1.Add(new Phrase("有肺炎", ChFont1));
        }
        else
        {
            Q17_1.Add(new Phrase("□有肺炎", ChFont1));
        }
        if (Q17_2CheckBox.Checked)
        {
            Q17_1.Add(new Chunk(image, 0, -4));
            Q17_1.Add(new Phrase("發燒", ChFont1));
        }
        else
        {
            Q17_1.Add(new Phrase("□發燒", ChFont1));
        }
        if (Q17_3CheckBox.Checked)
        {
            Q17_1.Add(new Chunk(image, 0, -4));
            Q17_1.Add(new Phrase("X光有浸潤性病變", ChFont1));
        }
        else
        {
            Q17_1.Add(new Phrase("□X光有浸潤性病變", ChFont1));
        }
        if (Q17_4CheckBox.Checked)
        {
            Q17_1.Add(new Chunk(image, 0, -4));
            Q17_1.Add(new Phrase("全身性症狀", ChFont1));
        }
        else
        {
            Q17_1.Add(new Phrase("□全身性症狀", ChFont1));
        }
        Q17.AddElement(Q17_1);
        table31.AddCell(Q17);

        PdfPTable table33 = new PdfPTable(new float[] { 1 });
        table33.TotalWidth = 550f;
        table33.LockedWidth = true;
        PdfPCell Q18 = new PdfPCell();
        Q18.MinimumHeight = 10; Q18.BorderWidthTop = 0;
        Q18.BorderWidthLeft = 0; Q18.BorderWidthRight = 0;
        Q18.BorderWidthBottom = 0f;
        Paragraph Q18_1 = new Paragraph();
        if (Q18CheckBox.Checked)
        {
            Q18_1.Add(new Chunk(image, -1, -4));
            Q18_1.Add(new Phrase("有尿道感染，經細菌陪養證實，且有臨床症狀", ChFont1));
        }
        else
        {
            Q18_1.Add(new Phrase("□有尿道感染，經細菌陪養證實，且有臨床症狀", ChFont1));
        }
        Q18.AddElement(Q18_1);
        table33.AddCell(Q18);

        PdfPTable table34 = new PdfPTable(new float[] { 1 });
        table34.TotalWidth = 550f;
        table34.LockedWidth = true;
        PdfPCell Q19 = new PdfPCell();
        Q19.MinimumHeight = 10; Q19.BorderWidthTop = 0;
        Q19.BorderWidthLeft = 0; Q19.BorderWidthRight = 0;
        Q19.BorderWidthBottom = 0f;
        Paragraph Q19_1 = new Paragraph();
        if (Q19CheckBox.Checked)
        {
            Q19_1.Add(new Chunk(image, -1, -4));
            Q19_1.Add(new Phrase("有傷口組織感染，經細菌陪有證實，請說明:", ChFont1));
        }
        else
        {
            Q19_1.Add(new Phrase("□有傷口組織感染，經細菌陪有證實，請說明:", ChFont1));
        }
        Q19.AddElement(Q19_1);
        table34.AddCell(Q19);

        PdfPTable table35 = new PdfPTable(new float[] { 1 });
        table35.TotalWidth = 550f;
        table35.LockedWidth = true;
        PdfPCell Q19_4 = new PdfPCell(new Phrase("   原在疾病:" + TextBox24.Text, ChFont1));
        Q19_4.Colspan = 4; Q19_4.MinimumHeight = 10;
        Q19_4.BorderWidthTop = 0; Q19_4.BorderWidthLeft = 0;
        Q19_4.BorderWidthRight = 0; Q19_4.BorderWidthBottom = 0f;
        Q19_4.HorizontalAlignment = 0;
        table35.AddCell(Q19_4);

        PdfPTable table36 = new PdfPTable(new float[] { 1 });
        table36.TotalWidth = 550f;
        table36.LockedWidth = true;
        PdfPCell Q19_2 = new PdfPCell(new Phrase("   手術方法:" + TextBox25.Text, ChFont1));
        Q19_2.Colspan = 4; Q19_2.MinimumHeight = 10;
        Q19_2.BorderWidthTop = 0; Q19_2.BorderWidthLeft = 0;
        Q19_2.BorderWidthRight = 0; Q19_2.BorderWidthBottom = 0f;
        Q19_2.HorizontalAlignment = 0;
        table36.AddCell(Q19_2);

        PdfPTable table37 = new PdfPTable(new float[] { 1 });
        table37.TotalWidth = 550f;
        table37.LockedWidth = true;
        PdfPCell Q19_3 = new PdfPCell(new Phrase("   感染部位:" + TextBox26.Text, ChFont1));
        Q19_3.Colspan = 4; Q19_3.MinimumHeight = 10;
        Q19_3.BorderWidthTop = 0; Q19_3.BorderWidthLeft = 0;
        Q19_3.BorderWidthRight = 0; Q19_3.BorderWidthBottom = 0f;
        Q19_3.HorizontalAlignment = 0;
        table37.AddCell(Q19_3);

        PdfPTable table38 = new PdfPTable(new float[] { 1 });
        table38.TotalWidth = 550f;
        table38.LockedWidth = true;
        PdfPCell Q20 = new PdfPCell();
        Q20.MinimumHeight = 10; Q20.BorderWidthTop = 0;
        Q20.BorderWidthLeft = 0; Q20.BorderWidthRight = 0;
        Q20.BorderWidthBottom = 0f;
        Paragraph Q20_1 = new Paragraph();
        Q20_1.Add(new Phrase("    "));
        if (Q20CheckBox.Checked)
        {
            Q20_1.Add(new Chunk(image, -1, -4));
            Q20_1.Add(new Phrase("其他院內感染，請說明:" + TextBox27.Text, ChFont1));
        }
        else
        {
            Q20_1.Add(new Phrase("□其他院內感染，請說明:" + TextBox27.Text, ChFont1));
        }
        Q20.AddElement(Q20_1);
        table38.AddCell(Q20);

        doc1.Open();
        PdfContentByte cb = PdfWriter.DirectContent;
        cb.BeginText();
        //BaseFont bfont = BaseFont.CreateFont(@"c:\windows\fonts\SIMHEI.TTF", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);//设定字体：黑体 
        cb.SetFontAndSize(bfChinese, 8);
        //cb.SetCharacterSpacing(1); 
        //cb.SetRGBColorFill(200, 200, 200);
        cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "列印時間:" + DateTime.Now.ToString(), 25, 10, 0);
        cb.EndText();
        //cb.SetRGBColorFill(00, 00, 00);

        PdfPTable table39 = new PdfPTable(new float[] { 1 });
        string imageURL3 = Server.MapPath(".") + "/images/Page2.png";
        iTextSharp.text.Image image3 = iTextSharp.text.Image.GetInstance(imageURL3);
        table39.TotalWidth = 550f;
        table39.LockedWidth = true;
        PdfPCell Q21 = new PdfPCell();
        Q21.MinimumHeight = 0;
        Q21.BorderWidthTop = 0;
        Q21.BorderWidthLeft = 0;
        Q21.BorderWidthRight = 0;
        Q21.BorderWidthBottom = 0f;
        Paragraph Q21_1 = new Paragraph();
        Q21_1.Add(new Chunk(image3, -60, -875));
        Q21.AddElement(Q21_1);
        table39.AddCell(Q21);

        doc1.Add(table); doc1.Add(Images2); doc1.Add(table2); doc1.Add(ptb); doc1.Add(table3);
        doc1.Add(table4); doc1.Add(table5); doc1.Add(table6); doc1.Add(table7); doc1.Add(table8);
        /*doc1.Add(table9);*/
        doc1.Add(table10); doc1.Add(table11); doc1.Add(table12); doc1.Add(table13);
        doc1.Add(table14); doc1.Add(table15);        /*doc1.Add(table16);*/        doc1.Add(table17); doc1.Add(table18);
        doc1.Add(table19); doc1.Add(table20); doc1.Add(table21); doc1.Add(table22); doc1.Add(table23);
        /*doc1.Add(table24);*/
        doc1.Add(table25); doc1.Add(table26); doc1.Add(table27); doc1.Add(table28);
        doc1.Add(table29); doc1.Add(table30); doc1.Add(table31); doc1.Add(table33); doc1.Add(table34);
        doc1.Add(table35);
        doc1.Add(table36); doc1.Add(table37); doc1.Add(table38); doc1.Add(table39); doc1.Add(Images);

        doc1.Close();


        Response.WriteFile(filePath);
        Response.Write(doc1);
        Response.End();
        Response.Close();
        Response.Write("<script language='javascript'>window.location='Default.aspx'</script>");
        PdfWriter.Close();
    }

    protected void Button2_Click(object sender, EventArgs e)
    {
        creatPDF();
    }

    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        string BkUser_id = Convert.ToString(Session["Certification_Session_Id"]);
        Session["BkUser_id"] = BkUser_id;
        Response.Redirect("Default.aspx?BkUser_id=" + BkUser_id);
        Session.RemoveAll();

        Boolean Selected = false;
        for (int count = 0; count < Grid_Output.Rows.Count; count++)
        {
            if (((CheckBox)Grid_Output.Rows[count].FindControl("chkStatus")).Checked)
            {
                Selected = true;
            }
        }
        if (Selected == false)
        {
            LinkButton1.Attributes.Add("onclick ", "return confirm( '尚有資料未確認');");
        }

        
    }


    protected void lnkBtn_Click(object sender, EventArgs e)
    {
        lnkBtn.Attributes.Add("onclick ", "return confirm( '尚有資料未確認');");
    }
}
