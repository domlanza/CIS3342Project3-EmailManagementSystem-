using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Utilities;

namespace Project3EmailManagementSystem
{
    public partial class Default : System.Web.UI.Page
    {
        DBConnect dbConnect = new DBConnect();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["EmailAddress"] != null)
                {
                    inputEmail.Text = Session["EmailAddress"].ToString();
                }
            }
        }

        protected void btnNewAccount_Click(object sender, EventArgs e)
        {
            Server.Transfer("CreateAccount.aspx");
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            if (inputEmail.Text != null && inputPassword != null)
            {
                SqlCommand objCommand = new SqlCommand();

                objCommand.CommandType = CommandType.StoredProcedure;
                    objCommand.CommandText = "SelectAllFromUsers";

                 SqlParameter Email = new SqlParameter("@InputEmail", inputEmail.Text);
                Email.Direction = ParameterDirection.Input;
                    objCommand.Parameters.Add(Email);

                   SqlParameter Password = new SqlParameter("@InputPassword", inputPassword.Text);
                Password.Direction = ParameterDirection.Input;
                    objCommand.Parameters.Add(Password);

                DataSet mydata = dbConnect.GetDataSetUsingCmdObj(objCommand);


                // String sql = "SELECT * FROM Users WHERE EmailAddress = '" + inputEmail.Text + "' AND Passsword = '" + inputPassword.Text + "'";
                //  DataSet mydata = dbConnect.GetDataSet(sql);



                int size = mydata.Tables[0].Rows.Count;
                if (size > 0)
                {
                    int userActive = int.Parse(dbConnect.GetField("ActiveAccount", 0).ToString());
                    //Active accounts are 1s
                    if (userActive == 0)
                    {
                        Response.Write("<script>alert('Your email account is banned.Contact the administrator')</script>");
                    }
                    else
                    {
                        String userType = dbConnect.GetField("Type", 0).ToString();
                        if (userType.CompareTo("User") == 0)
                        {
                            Session["UserId"] = dbConnect.GetField("UserId", 0);
                            Session["UserName"] = dbConnect.GetField("UserName", 0);
                            Session["PhoneNumber"] = dbConnect.GetField("PhoneNumber", 0);
                            Session["EmailAddress"] = inputEmail.Text;
                            Session["AvatarPicture"] = dbConnect.GetField("AvatarPicture", 0);
                            Response.Redirect("EmailClient.aspx");
                        }
                        else
                        {
                            Session["UserId"] = dbConnect.GetField("UserId", 0);
                            Session["UserName"] = dbConnect.GetField("UserName", 0);
                            Session["PhoneNumber"] = dbConnect.GetField("PhoneNumber", 0);
                            Session["EmailAddress"] = inputEmail.Text;
                            Session["AvatarPicture"] = dbConnect.GetField("AvatarPicture", 0);
                            Response.Redirect("EmailManager.aspx");
                        }
                    }
                }
                else {
                    Response.Write("<script>alert('Incorrect password')</script>");
                }
            }
        }

        //public DataSet getLoginData(String email, String password)
        //{
        //    objCommand.CommandType = CommandType.StoredProcedure;
        //    objCommand.CommandText = "LoginCheck";

        //    SqlParameter inputEmailAddress = new SqlParameter("@emailAddress", email);
        //    inputEmailAddress.Direction = ParameterDirection.Input;
        //    objCommand.Parameters.Add(inputEmailAddress);

        //    SqlParameter inputPassword = new SqlParameter("@password", password);
        //    inputEmailAddress.Direction = ParameterDirection.Input;
        //    objCommand.Parameters.Add(inputPassword);

        //    DataSet mydata = dbConnect.GetDataSetUsingCmdObj(objCommand);
        //    return mydata;
        //}
    }
}