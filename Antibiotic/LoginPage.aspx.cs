using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class Antibiotic_LoginPage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(@"Data Source=\SQLEXPRESS;Initial Catalog=;Persist Security Info=True;User ID=;Password=");
        SqlCommand cmd = new SqlCommand("select * from [UserLogin] where UserName=@UserName and PassWord=@PassWord", con);
        cmd.Parameters.AddWithValue("@UserName", TextBox1.Text);
        cmd.Parameters.AddWithValue("@PassWord", TextBox2.Text);
        SqlDataAdapter sda = new SqlDataAdapter(cmd);
        DataTable dt = new DataTable();
        sda.Fill(dt);
        con.Open();
        int i = cmd.ExecuteNonQuery();
        con.Close();
        if (dt.Rows.Count > 0)
        {
            string User_id = TextBox1.Text.Trim();
            Session["User_id"] = User_id;
            Response.Redirect("Default.aspx?User_id=" + User_id);  //傳登入值User_id至Default.aspx頁面
            Session.RemoveAll();
        }
        else
        {
            Label1.Text = "帳號或密碼輸入錯誤";
            Label1.ForeColor = System.Drawing.Color.Red;
        }
    }
}
