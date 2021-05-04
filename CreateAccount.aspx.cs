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
    public partial class CreateAccount : System.Web.UI.Page
    {

        DBConnect dbConnect = new DBConnect();
        SqlCommand objCommand = new SqlCommand();


        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void validation() {

        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            EmailLibrary.Users objuser = new EmailLibrary.Users();
            objuser.UserName = TxtUserName.Text;
            objuser.PhoneNumber = TxtPhoneNumber.Text;
            // objuser.CreatedEmailAddress = TxtCreateEmail.Text;
            objuser.SecurityEmailAddress = TxtSecurityEmail.Text;
            objuser.Password = txtPassword.Text;
            objuser.CreatedEmailAddress = TxtCreateEmail.Text;
            objuser.Avatar = "imgs/" + ddlAvatarImg.SelectedValue + ".jpg";
            objuser.UserType = ddlAccountType.SelectedValue;


            string EmailAddressSubmitted = TxtCreateEmail.Text.ToString();

            if (CheckUnique(EmailAddressSubmitted) == false)
            {
                lblValidation.Text = "Email Already Exists";
            }

            else if (CheckUnique(EmailAddressSubmitted) == true) { 

                //check is user Email is already there


                //String sql = "Select UserId From Users WHERE EmailAddress = " + objuser.CreatedEmailAddress + "";
                //DataSet mydata = dbConnect.GetDataSet(sql);
                //int size = Int32.Parse(mydata.Tables[0].Rows[0].ToString());
                //if (size == 0)
                //{
                //    //email is good to go
                //    Response.Write("<script>alert('Good to go.')</script>");

                //}
                //else {
                //    Response.Write("<script>alert('Need a new email.')</script>");
                //}



                DBConnect database = new DBConnect();
                //string sqlQuery = " INSERT INTO Users VALUES ('" + TxtUserName.Text + "," + TxtPhoneNumber.Text + "," + TxtCreateEmail.Text + "," + TxtSecurityEmail.Text + "," + "1" + "," + txtPassword.Text  +"," + Convert.ToInt32(ddlAvatarImg.SelectedValue) + "," + ddlAccountType.SelectedValue + ")'";

                SqlCommand objCommand1 = new SqlCommand();

                objCommand1.CommandType = CommandType.StoredProcedure;
                objCommand1.CommandText = "InsertIntoUsersEverything";

                SqlParameter UserName = new SqlParameter("@UserName", objuser.UserName);
                UserName.Direction = ParameterDirection.Input;
                objCommand1.Parameters.Add(UserName);

                SqlParameter Password = new SqlParameter("@Password", objuser.Password);
                Password.Direction = ParameterDirection.Input;
                objCommand1.Parameters.Add(Password);

                SqlParameter Phone = new SqlParameter("@PhoneNumber", objuser.PhoneNumber);
                Phone.Direction = ParameterDirection.Input;
                objCommand1.Parameters.Add(Phone);

                SqlParameter Email = new SqlParameter("@EmailAddress", objuser.CreatedEmailAddress);
                Email.Direction = ParameterDirection.Input;
                objCommand1.Parameters.Add(Email);

                SqlParameter Security = new SqlParameter("@Security", objuser.SecurityEmailAddress);
                Security.Direction = ParameterDirection.Input;
                objCommand1.Parameters.Add(Security);

                SqlParameter Avatar = new SqlParameter("@Avatar", objuser.Avatar);
                Avatar.Direction = ParameterDirection.Input;
                objCommand1.Parameters.Add(Avatar);

                SqlParameter Active = new SqlParameter("@Active", 1);
                Active.Direction = ParameterDirection.Input;
                objCommand1.Parameters.Add(Active);

                SqlParameter Type = new SqlParameter("@Type", objuser.UserType);
                Type.Direction = ParameterDirection.Input;
                objCommand1.Parameters.Add(Type);

                dbConnect.DoUpdateUsingCmdObj(objCommand1);
                //DataSet dataset = database.GetDataSetUsingCmdObj(objCommand1);


                // string strSQL = "INSERT INTO Users (UserName, Passsword, PhoneNumber, EmailAddress, SecurityEmailAddress, AvatarPicture, ActiveAccount, Type) " 
                //     + "VALUES ('" + objuser.UserName + "', '" + objuser.Password + "', '" + objuser.PhoneNumber + "', '" + objuser.CreatedEmailAddress + "', '" + objuser.SecurityEmailAddress
                //    + "', '" + objuser.Avatar + "', '" + "1" + "','" + objuser.UserType + "')";



                // DataSet dataset = database.GetDataSet(strSQL);


                //Obtain the CreatedEmailAddress From the User
                SqlCommand objCommand2 = new SqlCommand();

                objCommand2.CommandType = CommandType.StoredProcedure;
                objCommand2.CommandText = "UserIdFromUsersCreateAccountPage";

                SqlParameter CreatedEmailAddress = new SqlParameter("@CreatedEmailAddress", objuser.CreatedEmailAddress);
                CreatedEmailAddress.Direction = ParameterDirection.Input;
                objCommand2.Parameters.Add(CreatedEmailAddress);

                DataSet UserId = dbConnect.GetDataSetUsingCmdObj(objCommand2);

                //String sql = "SELECT UserId FROM Users " + " " +
                //    "WHERE Users.EmailAddress = '" + objuser.CreatedEmailAddress + "'";
                //DataSet newData = database.GetDataSet(sql);
                //   DataSet UserId = dbConnect.GetDataSet(sql);

                int CurrentUserId = Int32.Parse(UserId.Tables[0].Rows[0]["UserId"].ToString());

                //Create Tags for the user
                String tagNameInbox = "Inbox";
                String tagNameSent = "Sent";
                String tagNameJunk = "Junk";
                String tagNameTrash = "Trash";
                String tagNameFlag = "Flag";

                //Inbox Insert Tag
                SqlCommand objCommand3 = new SqlCommand();

                objCommand3.CommandType = CommandType.StoredProcedure;
                objCommand3.CommandText = "InsertTag";

                SqlParameter TagNameInbox = new SqlParameter("@TagName", tagNameInbox);
                TagNameInbox.Direction = ParameterDirection.Input;
                objCommand3.Parameters.Add(TagNameInbox);

                SqlParameter CurrentUserIdInbox = new SqlParameter("UserId", CurrentUserId);
                CurrentUserIdInbox.Direction = ParameterDirection.Input;
                objCommand3.Parameters.Add(CurrentUserIdInbox);

                dbConnect.DoUpdateUsingCmdObj(objCommand3);


                //string strTagsInboxSql = "INSERT INTO Tags (TagName, UserId) "
                //+ "VALUES ('" + tagNameInbox + "', '" + CurrentUserId + "')";

                //DataSet InboxUpdate = database.GetDataSet(strTagsInboxSql);

                //Sent Insert Tag
                //string strTagsSentSql = "INSERT INTO Tags (TagName, UserId) "
                //+ "VALUES ('" + tagNameSent + "', '" + CurrentUserId + "')";

                //DataSet SentUpdate = database.GetDataSet(strTagsSentSql);

                SqlCommand objCommand4 = new SqlCommand();

                objCommand4.CommandType = CommandType.StoredProcedure;
                objCommand4.CommandText = "InsertTag";

                SqlParameter TagNameSent = new SqlParameter("@TagName", tagNameSent);
                TagNameSent.Direction = ParameterDirection.Input;
                objCommand4.Parameters.Add(TagNameSent);

                SqlParameter CurrentUserIdSent = new SqlParameter("@UserId", CurrentUserId);
                CurrentUserIdSent.Direction = ParameterDirection.Input;
                objCommand4.Parameters.Add(CurrentUserIdSent);

                dbConnect.DoUpdateUsingCmdObj(objCommand4);


                //Junk
                //string strTagsJunkSql = "INSERT INTO Tags (TagName, UserId) "
                //+ "VALUES ('" + tagNameJunk + "', '" + CurrentUserId + "')";

                //DataSet JunkUpdate = database.GetDataSet(strTagsJunkSql);

                SqlCommand objCommand5 = new SqlCommand();

                objCommand5.CommandType = CommandType.StoredProcedure;
                objCommand5.CommandText = "InsertTag";

                SqlParameter TagNameJunk = new SqlParameter("@TagName", tagNameJunk);
                TagNameJunk.Direction = ParameterDirection.Input;
                objCommand5.Parameters.Add(TagNameJunk);

                SqlParameter CurrentUserIdJunk = new SqlParameter("@UserId", CurrentUserId);
                CurrentUserIdJunk.Direction = ParameterDirection.Input;
                objCommand5.Parameters.Add(CurrentUserIdJunk);

                dbConnect.DoUpdateUsingCmdObj(objCommand5);



                ////Trash
                //string strTagsTrashSql = "INSERT INTO Tags (TagName, UserId) "
                //+ "VALUES ('" + tagNameTrash + "', '" + CurrentUserId + "')";

                //DataSet TrashUpdate = database.GetDataSet(strTagsTrashSql);


                SqlCommand objCommand6 = new SqlCommand();

                objCommand6.CommandType = CommandType.StoredProcedure;
                objCommand6.CommandText = "InsertTag";

                SqlParameter TagNameTrash = new SqlParameter("@TagName", tagNameTrash);
                TagNameTrash.Direction = ParameterDirection.Input;
                objCommand6.Parameters.Add(TagNameTrash);

                SqlParameter CurrentUserIdTrash = new SqlParameter("@UserId", CurrentUserId);
                CurrentUserIdTrash.Direction = ParameterDirection.Input;
                objCommand6.Parameters.Add(CurrentUserIdTrash);

                dbConnect.DoUpdateUsingCmdObj(objCommand6);

                //Flag
                //string strTagsFlagSql = "INSERT INTO Tags (TagName, UserId) "
                //+ "VALUES ('" + tagNameFlag + "', '" + CurrentUserId + "')";

                //DataSet FlagUpdate = database.GetDataSet(strTagsFlagSql);

                SqlCommand objCommand7 = new SqlCommand();

                objCommand7.CommandType = CommandType.StoredProcedure;
                objCommand7.CommandText = "InsertTag";

                SqlParameter TagNameFlag = new SqlParameter("@TagName", tagNameFlag);
                TagNameFlag.Direction = ParameterDirection.Input;
                objCommand7.Parameters.Add(TagNameFlag);

                SqlParameter CurrentUserIdFlag = new SqlParameter("@UserId", CurrentUserId);
                CurrentUserIdFlag.Direction = ParameterDirection.Input;
                objCommand7.Parameters.Add(CurrentUserIdFlag);

                dbConnect.DoUpdateUsingCmdObj(objCommand7);

                database.CloseConnection();

                Response.Redirect("Default.aspx");


            }
            else {

                lblValidation.Text = "You shoud not have made it here";

            }
        }

        public bool CheckUnique(string emailaddress)
        {
            //            String sql = "SELECT * FROM Users WHERE EmailAddress = '" + emailaddress + "' ";

            //            CREATE PROCEDURE[dbo].[CreateNewUserValidation]
            //        @emailaddress varchar(max)
            //AS
            //SELECT* FROM Users WHERE EmailAddress = @emailaddress
            //RETURN 0

            SqlCommand objCommand8 = new SqlCommand();

            objCommand8.CommandType = CommandType.StoredProcedure;
            objCommand8.CommandText = "CreateNewUserValidation";

            SqlParameter EmailAddressId8 = new SqlParameter("@emailaddress", emailaddress);
            EmailAddressId8.Direction = ParameterDirection.Input;
            objCommand8.Parameters.Add(EmailAddressId8);

           DataSet mydata = dbConnect.GetDataSetUsingCmdObj(objCommand8);
        //    DataSet mydata = dbConnect.GetDataSet(sql);
            int size = Int32.Parse(mydata.Tables[0].Rows.Count.ToString());

            if (size > 0)
            {
                return false;
            }
            else {
                return true;
            }
        }


        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string newImage = "imgs/" + ddlAvatarImg.SelectedItem.Text + ".jpg";
            AvatarImg.ImageUrl = newImage;
        }

    }
}