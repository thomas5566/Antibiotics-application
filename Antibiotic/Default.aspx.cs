using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using System.Diagnostics;
using System.Data.SqlClient;
using System.IO;
using System.Text.RegularExpressions;
public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            loadGridData2();
        }

        if (Session["User_id"] != null)
        {
            Label1.Text = Convert.ToString(Session["User_id"]);
            LinkButton2.Text = Convert.ToString(Session["User_id"]);
        }

        if (Session["BkUser_id"] != null)
        {
            Label1.Text = Convert.ToString(Session["BkUser_id"]);
            LinkButton2.Text = Convert.ToString(Session["BkUser_id"]);
        }
        //Label1.Text = Session["User_id"].ToString();
    }
    protected void Search(object sender, EventArgs e)
    {
        this.loadGridData2();
    }
    
    private void loadGridData2()
    {
        string constr = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
        using (SqlConnection con = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand())
            {
                // SqlCommand cmd = new SqlCommand("Select DISTINCT * from [Image] ORDER BY [id] Desc", con);
                cmd.CommandText = "SELECT DISTINCT Patient_infor.P_ID, Patient_infor.P_Name, PDF_info.P_ID, PDF_info.PDF_CreTime, PDF_info.PDF_Path, Drog_info.Certification FROM Patient_infor INNER JOIN PDF_info ON Patient_infor.P_ID = PDF_info.P_ID INNER JOIN Drog_info ON Patient_infor.P_ID = Drog_info.P_ID WHERE Patient_infor.P_ID LIKE '%' + @P_ID + '%' ORDER BY PDF_info.PDF_CreTime DESC; ";
                cmd.Connection = con;
                cmd.Parameters.AddWithValue("@P_ID", txtSearch.Text.Trim());
                DataTable dt = new DataTable();
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    sda.Fill(dt);
                    GridView2.DataSource = dt;
                    GridView2.DataBind();
                }
            }
        }
    }
    protected void DeleteFile(object sender, EventArgs e)
    {
        string constr = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
        string P_ID = (sender as Button).CommandArgument;
        using (SqlConnection con = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandText = "Delete PDF_info where P_ID=@P_ID";
                cmd.Parameters.AddWithValue("@P_ID", P_ID);
                cmd.Connection = con;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        Response.Redirect(Request.Url.AbsoluteUri);
    }
    protected void grdAttachment_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "ViewFile")
        {
            string fileName = Server.MapPath("~/Antibiotic/PDFs/" + e.CommandArgument.ToString());
            Process process = new Process();
            process.StartInfo.UseShellExecute = true;
            process.StartInfo.FileName = fileName;
            process.Start();
        }
    }
    protected void OnRowDataBound2(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string p_ID = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "P_ID"));            
            Button lnkbtnresult = (Button)e.Row.FindControl("lnkDelete");
            if (lnkbtnresult != null)
            {
                lnkbtnresult.Attributes["onclick"] = "if(!confirm('確定要刪除 " + p_ID + " ?')){ return false; };";
            }

            bool Row6 = Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "Certification"));
            if (Row6 == true)
            {
                var GridCell = e.Row.Cells[6];
                System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
                img.ImageUrl = "images/greencheck.png";
                img.Height = 16;
                GridCell.Controls.Add(img);

                Label hdr = new Label();
                hdr.Text = "已驗證";
                GridCell.Controls.Add(hdr);
            }
            else if (Row6 == false)
            {
                var GridCell = e.Row.Cells[6];
                System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
                img.ImageUrl = "images/CrossMark.png";
                img.Height = 16;
                GridCell.Controls.Add(img);

                Label hdr = new Label();
                hdr.Text = "未驗證";
                GridCell.Controls.Add(hdr);
            }

            
            /*if (e.Row.RowIndex > 0)
            {
             GridViewRow previousRow = GridView2.Rows[e.Row.RowIndex - 1];
             
                    if (e.Row.Cells[6].Text != previousRow.Cells[6].Text)
                    {
                        if (previousRow.Cells[6].RowSpan == 0)
                        {
                            previousRow.Cells[6].RowSpan += 2;
                            e.Row.Cells[6].Text = "AAAAAA";
                        }
                    }
            }*/
            /*  for (int rowIndex = GridView2.Rows.Count - 2; rowIndex >= 0; rowIndex--)
              {
                  GridViewRow gvRow = GridView2.Rows[rowIndex];
                  GridViewRow gvPreviousRow = GridView2.Rows[rowIndex + 1];
                  for (int cellCount = 0; cellCount < gvRow.Cells.Count; cellCount++)
                  {
                      if (gvRow.Cells[cellCount].Text ==
                                          gvPreviousRow.Cells[cellCount].Text)
                      {
                          if (gvPreviousRow.Cells[cellCount].RowSpan < 2)
                          {
                              gvRow.Cells[cellCount].RowSpan = 2;
                          }
                          else
                          {
                              gvRow.Cells[cellCount].RowSpan =
                                  gvPreviousRow.Cells[cellCount].RowSpan + 1;
                          }
                          gvPreviousRow.Cells[cellCount].Visible = false;
                      }
                  }
              }*/



            /*  if (e.Row.RowIndex > 0)
              {
                  GridViewRow previousRow = GridView2.Rows[e.Row.RowIndex - 1];
                  if (e.Row.Cells[0].Text == previousRow.Cells[0].Text)
                  {
                      if (e.Row.Cells[6].Text != previousRow.Cells[6].Text)
                      {
                          if (previousRow.Cells[0].RowSpan == 0)
                          {
                              previousRow.Cells[0].RowSpan += 2;
                              e.Row.Cells[0].Visible = false;
                          }
                      }
                  }
              }*/


            //For row 1
            /* e.Row.Cells[0].Text = Regex.Replace(e.Row.Cells[0].Text, txtSearch.Text.Trim(), delegate (Match match)
             {
                 return string.Format("<span style = 'Background-color:#FFFFAF'>{0}</span>", match.Value);
             }, RegexOptions.IgnoreCase);
             //For row 2
             e.Row.Cells[1].Text = Regex.Replace(e.Row.Cells[1].Text, txtSearch.Text.Trim(), delegate (Match match)
             {
                 return string.Format("<span style = 'background-color:#FFFFAF'>{0}</span>", match.Value);
             }, RegexOptions.IgnoreCase);*/
        }
    }
    protected void GridView2_PageIndexChanging(object sender, EventArgs e)
    {
        Response.Write("PageIndexChanging");
    }
    protected void GridView2_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        this.GridView2.PageIndex = e.NewPageIndex;
        this.loadGridData2();
    }
    protected void btnViewDetails_Click(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
        //Get current row
        GridViewRow row = (GridViewRow)btn.NamingContainer;
        //Get current row values. if you are using TemplateField, you could also use FindControl method()

        Session.RemoveAll();
       

        if (Label1.Text != "")
        {
            string Certification_Session_Id = Label1.Text;
            Session["Patient_Session_Id"] = row.Cells[0].Text;
            Session["Certification_Session_Id"] = Label1.Text;
            // Response.Redirect("Default3.aspx");
            Response.Redirect("Certification_Page.aspx");
        }
        else
        {
            string Patient_Session_Id = row.Cells[0].Text;  //Patient_Id值丟到Page Default3  
            Session["Patient_Session_Id"] = row.Cells[0].Text;
            Response.Redirect("Default3.aspx");
        }
        
        //Response.Redirect(String.Format("Default3.aspx?Patient_Session_Id={0}", Server.UrlEncode(Patient_Session_Id))));



        //string name = row.Cells[1].Text;
        //pass value to destination page
        //Session["Patient_Session_Id"] = Patient_Session_Id;
        //Session["Certification_Session_Id"] = Certification_Session_Id;


        //Response.Redirect(String.Format("Default3.aspx?Patient_Session_Id={0}&Certification_Session_Id={1}", Server.UrlEncode(Patient_Session_Id), Server.UrlEncode(Certification_Session_Id)));
        //Session.RemoveAll();
    }





    /*protected void btnRetrieve_Click(object sender, EventArgs e)
    {
        string P_ID = (sender as Button).CommandArgument;
        string constr = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
        using (SqlConnection con = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandText = "select * from PDF_info where P_ID=@P_ID";
                cmd.Parameters.AddWithValue("@P_ID", P_ID);
                cmd.Connection = con;
                con.Open();
                SqlDataReader dr = null;
                dr = cmd.ExecuteReader();                
                if (dr.HasRows)
                {
                    dr.Read();
                    if (!string.IsNullOrEmpty(Convert.ToString(dr["PDF_Path"])))
                    {
                        Image1.ImageUrl = "~/PDFs/" + Convert.ToString(dr["PDF_Path"]);
                    }
                }
                con.Close();
                // cmd.ExecuteNonQuery();
            }
            //Response.Redirect(Request.Url.AbsoluteUri);
        }
    }*/

    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        if (Session["User_id"] != null)
        {
            LinkButton1.Text = Convert.ToString(Session["User_id"]);
        }
    }

  /*  public class GridDecorator
    {
        public static void MergeRows(GridView GridView2)
        {
            for (int rowIndex = GridView2.Rows.Count - 2; rowIndex >= 0; rowIndex--)
            {
                GridViewRow row = GridView2.Rows[rowIndex];
                GridViewRow previousRow = GridView2.Rows[rowIndex + 1];

                for (int i = 0; i < row.Cells.Count; i++)
                {
                    if (row.Cells[i].Text == previousRow.Cells[i].Text)
                    {
                        row.Cells[i].RowSpan = previousRow.Cells[i].RowSpan < 2 ? 2 :
                                               previousRow.Cells[i].RowSpan + 1;
                        previousRow.Cells[i].Visible = false;
                    }
                }
            }
        }
    }

    protected void gridView_PreRender(object sender, EventArgs e)
    {
        GridDecorator.MergeRows(GridView2);
    }*/
}