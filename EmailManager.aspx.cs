using EmailLibrary;
using System;
using System.Collections;
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
    public partial class EmailManager : System.Web.UI.Page
    {
        DBConnect dBConnect = new DBConnect();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //Visibility control
                lbAccount.Visible = false;
                btnBan.Visible = false;
                btnUnBan.Visible = false;

                lblFlagEmail.Visible = false;
                lblEmpty.Visible = false;
                //DisplayCurrentUsers();
                //showFlagEmails();
                DetailViewOfEmail(false);
            }
        }

        public void DisplayCurrentUsers()
        {
            ArrayList users = new ArrayList();
            String UserType = "User";

            //Stored Procedure #1 Select All From Users by the Type of Users
            SqlCommand objCommand1 = new SqlCommand();

            objCommand1.CommandType = CommandType.StoredProcedure;
            objCommand1.CommandText = "SelectAllUsersWhereUsersAreAType";

            SqlParameter UserType1 = new SqlParameter("@UserType", UserType);
            UserType1.Direction = ParameterDirection.Input;
            objCommand1.Parameters.Add(UserType1);

            DataSet mydata = dBConnect.GetDataSetUsingCmdObj(objCommand1);

            int size = mydata.Tables[0].Rows.Count;
            if (size > 0)
            {
                for (int i = 0; i < size; i++)
                {
                    Users user = new Users();
                    user.UserName = mydata.Tables[0].Rows[i]["UserName"].ToString();
                    user.Avatar = mydata.Tables[0].Rows[i]["AvatarPicture"].ToString();
                    user.PhoneNumber = mydata.Tables[0].Rows[i]["PhoneNumber"].ToString();
                    user.CreatedEmailAddress = mydata.Tables[0].Rows[i]["EmailAddress"].ToString();
                    user.SecurityEmailAddress = mydata.Tables[0].Rows[i]["SecurityEmailAddress"].ToString();
                    if (mydata.Tables[0].Rows[i]["ActiveAccount"].ToString().CompareTo("1") == 0)
                    {
                        user.Active = "active";
                    }
                    else
                    {
                        user.Active = "inactive";
                    }
                    users.Add(user);
                }
                gvAccount.DataSource = users;
                gvAccount.DataBind();
            }
            else
            {
                Response.Write("<script>alert('No users found.')</script>");
            }
        }


        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session["UserId"] = null;
            Session["UserName"] = null;
            Session["PhoneNumber"] = null;
            Session["AvatarPicture"] = null;
            Response.Redirect("Default.aspx");
        }


        public void showFlagEmails(String userId = "")
        {
            ArrayList emails = new ArrayList();
            String sql = "";
            SqlCommand objCommand2 = new SqlCommand();
            int FlagNum = 1;
            if (userId.CompareTo("") == 0)
            {
                //Stored Procedure #2 Show Flagged Email Where Flag Eqauls 1

                objCommand2.CommandType = CommandType.StoredProcedure;
                objCommand2.CommandText = "ShowFlaggedEmailEquals1";

                SqlParameter FlagNum2 = new SqlParameter("@FlagNum", FlagNum);
                FlagNum2.Direction = ParameterDirection.Input;
                objCommand2.Parameters.Add(FlagNum2);
            }
            else
            {
                //Stored Procedure #3 Show Flagged Email Where Flag Eqauls 1 and UserId
                objCommand2.CommandType = CommandType.StoredProcedure;
                objCommand2.CommandText = "ShowFlaggedEmailEquals1AndUserId";

                SqlParameter FlagNum3 = new SqlParameter("@FlagNum", FlagNum);
                FlagNum3.Direction = ParameterDirection.Input;
                objCommand2.Parameters.Add(FlagNum3);

                SqlParameter UserId3 = new SqlParameter("@UserId", userId);
                UserId3.Direction = ParameterDirection.Input;
                objCommand2.Parameters.Add(UserId3);

            }
            DataSet mydata = dBConnect.GetDataSetUsingCmdObj(objCommand2);
            int size = mydata.Tables[0].Rows.Count;
            if (size > 0)
            {
                for (int i = 0; i < size; i++)
                {
                    Email email = new Email();
                    String senderId = mydata.Tables[0].Rows[i]["SenderId"].ToString();
                    String receiverId = mydata.Tables[0].Rows[i]["ReceiverId"].ToString();
                    String senderEmail = getUserEmailByUserId(senderId);
                    String receiverEmail = getUserEmailByUserId(receiverId);
                    email.SenderName = senderEmail;
                    email.ReceiverName = receiverEmail;
                    email.Subject = mydata.Tables[0].Rows[i]["EmailSubject"].ToString();
                    email.EmailBody = mydata.Tables[0].Rows[i]["EmailBody"].ToString();
                    String createdTime = mydata.Tables[0].Rows[i]["Timestamp"].ToString();
                    email.CreatedTime = DateTime.Parse(createdTime).ToShortDateString();
                    emails.Add(email);
                }
                gvFlagEmail.DataSource = emails;
                gvFlagEmail.DataBind();

                lblEmpty.Visible = false;
            }
            else
            {
                lblEmpty.Visible = true;
            }
        }

        public String getUserEmailByUserId(String userId)
        {
            int userID = Int32.Parse(userId.ToString());

            //Stored Procedure #4 Get EmailAddress from the UserId

            SqlCommand objCommand4 = new SqlCommand();

            objCommand4.CommandType = CommandType.StoredProcedure;
            objCommand4.CommandText = "SelectEmailAddressFromUserId";

            SqlParameter UserId4 = new SqlParameter("@UserId", userID);
            UserId4.Direction = ParameterDirection.Input;
            objCommand4.Parameters.Add(UserId4);

            DataSet mydata = dBConnect.GetDataSetUsingCmdObj(objCommand4);

            int size = mydata.Tables[0].Rows.Count;
            if (size > 0)
            {
                String email = mydata.Tables[0].Rows[0]["EmailAddress"].ToString();
                return email;
            }
            else {
                return "No email address";
            }
        }
        public void BanCurrentUser(String active)
        {

            int active2 = Int32.Parse(active.ToString());
            //check the gridview
            for (int row = 0; row < gvAccount.Rows.Count; row++)
            {
                CheckBox Cbox;
                //Get the reference for the chkselect Control in the current row
                Cbox = (CheckBox)gvAccount.Rows[row].FindControl("chkAccount");
                if (Cbox.Checked)
                {
                    //get the email id and username
                    String userName = gvAccount.Rows[row].Cells[2].Text;
                    String email = gvAccount.Rows[row].Cells[4].Text;

                    //Stored Procedure #5 Update Users byChanging active
                    SqlCommand objCommand5 = new SqlCommand();

                    objCommand5.CommandType = CommandType.StoredProcedure;
                    objCommand5.CommandText = "UpdateActiveAccount";

                    SqlParameter Active5 = new SqlParameter("@Active", active2);
                    Active5.Direction = ParameterDirection.Input;
                    objCommand5.Parameters.Add(Active5);

                    SqlParameter EmailAddress5 = new SqlParameter("@EmailAddress", email);
                    EmailAddress5.Direction = ParameterDirection.Input;
                    objCommand5.Parameters.Add(EmailAddress5);

                    SqlParameter UserName5 = new SqlParameter("@UserName", userName);
                    UserName5.Direction = ParameterDirection.Input;
                    objCommand5.Parameters.Add(UserName5);

                    int val = dBConnect.DoUpdateUsingCmdObj(objCommand5);

                    if (val >= 0)
                    {

                    }
                    else {
                        Response.Write("<script>alert('Unable to ban user')</script>");
                    }
                }

            }
            DisplayCurrentUsers();
        }

        protected void btnBan_Click(object sender, EventArgs e)
        {
            //Ban the user
            BanCurrentUser("0");
        }

        protected void btnUnBan_Click(object sender, EventArgs e)
        {
            //Unban the User
            BanCurrentUser("1");
        }

        public void DetailViewOfEmail(Boolean flag) {
            lblSubject.Visible = flag;
            subjectOfPerson.Visible = flag;
            CreatedTimeFirstPerson.Visible = flag;
            CreatedTimeSecondPerson.Visible = flag;
            FromEmailFirstPerson.Visible = flag;
            FromEmailSecondPerson.Visible = flag;
            ToEmailFirstPerson.Visible = flag;
            ToEmailSecondPerson.Visible = flag;
            EmailBodySecondPerson.Visible = flag;
            EmailBodyFirstPerson.Visible = flag;
            btnBackD.Visible = flag;
        }

        protected void btnBackD_Click(object sender, EventArgs e)
        {
            showFlagEmails();
            gvFlagEmail.Visible = true;
            DetailViewOfEmail(false);
        }

        protected void gvFlagEmail_SelectedIndexChanged(object sender, EventArgs e)
        {
            Email email = new Email();
            email.SenderName = gvFlagEmail.SelectedRow.Cells[1].Text;
            email.ReceiverName = gvFlagEmail.SelectedRow.Cells[2].Text;
            email.Subject = gvFlagEmail.SelectedRow.Cells[3].Text;
            email.EmailBody = gvFlagEmail.SelectedRow.Cells[4].Text;
            email.CreatedTime = gvFlagEmail.SelectedRow.Cells[5].Text;

            FromEmailSecondPerson.Text = email.SenderName;
            ToEmailSecondPerson.Text = email.ReceiverName;
            subjectOfPerson.Text = email.Subject;
            EmailBodySecondPerson.Text = email.EmailBody;
            CreatedTimeSecondPerson.Text = email.CreatedTime;

            gvFlagEmail.Visible = false;
            DetailViewOfEmail(true);

        }

        protected void ShowCurrentAccounts_Click(object sender, EventArgs e)
        {

            showCurrentAccountsView(true);
            showFlaggedEmailInformation(false);
            DetailViewOfEmail(false);

        }

        protected void ShowFlaggedEmailInformation_Click(object sender, EventArgs e)
        {
            showCurrentAccountsView(false);
            showFlaggedEmailInformation(true);


        }


        public void showCurrentAccountsView (bool tf){
            lbAccount.Visible = tf;
            btnBan.Visible = tf;
            btnUnBan.Visible = tf;
            gvAccount.Visible = tf;
            DisplayCurrentUsers();


        }

        public void showFlaggedEmailInformation(bool tf) {
            lblFlagEmail.Visible = tf;
            lblEmpty.Visible = tf;
            gvFlagEmail.Visible = tf;
            //DetailViewOfEmail(tf);
            showFlagEmails();

        }
    }
}