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
    public partial class EmailClient : System.Web.UI.Page
    {
        DBConnect dBConnect = new DBConnect();
        SqlCommand objCommand = new SqlCommand();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ComposeButtonView(false);
                showDetailViewOfEmail(false);
                if (Session["UserId"] != null)
                {
                    OpenInboxEmailClient();
                    CreateTags();
                }
            }
        }

        public void OpenInboxEmailClient()
        {
            Session["Tag"] = "Inbox";
            String TagNameInbox = "Inbox";
            String UserId = Session["UserId"].ToString();

            //StoredProcedure #1 Acquire TagId from User
            SqlCommand objCommand1 = new SqlCommand();

            objCommand1.CommandType = CommandType.StoredProcedure;
            objCommand1.CommandText = "OpenInboxEmailClient";

            SqlParameter UserId1 = new SqlParameter("@UserId", UserId);
            UserId1.Direction = ParameterDirection.Input;
            objCommand1.Parameters.Add(UserId1);

            SqlParameter TagNameInbox1 = new SqlParameter("@TagNameInbox", TagNameInbox);
            TagNameInbox1.Direction = ParameterDirection.Input;
            objCommand1.Parameters.Add(TagNameInbox1);

            DataSet mydata = dBConnect.GetDataSetUsingCmdObj(objCommand1);

            String emailTag = mydata.Tables[0].Rows[0]["TagId"].ToString();

            //display the UserName and Avatar picture
            EmailUserName.Text = Session["UserName"].ToString();
            EmailuserAvatar.ImageUrl = Session["AvatarPicture"].ToString();

            //display the email gridview
            //storedProcedure #2 display the email the gridview
            SqlCommand objCommand2 = new SqlCommand();

            objCommand2.CommandType = CommandType.StoredProcedure;
            objCommand2.CommandText = "DisplayEmailGridView";

            SqlParameter UserId2 = new SqlParameter("@UserId", UserId);
            UserId2.Direction = ParameterDirection.Input;
            objCommand2.Parameters.Add(UserId2);

            SqlParameter EmailTag2 = new SqlParameter("@EmailTagId", emailTag);
            EmailTag2.Direction = ParameterDirection.Input;
            objCommand2.Parameters.Add(EmailTag2);

            DataSet mydata2 = dBConnect.GetDataSetUsingCmdObj(objCommand2);

            //DataSet mydata2 = dBConnect.GetDataSet(strSql2);
            int size = mydata2.Tables[0].Rows.Count;
            if (size > 0)
            {
                ArrayList emailList = new ArrayList();
                for (int i = 0; i < size; i++)
                {
                    //storedProcedure #3 obtain UserName and Email Address of the Sender Id
                    String senderId = mydata2.Tables[0].Rows[i]["SenderId"].ToString();

                    SqlCommand objCommand3 = new SqlCommand();

                    objCommand3.CommandType = CommandType.StoredProcedure;
                    objCommand3.CommandText = "UserNameEmailAddressFromUsers";

                    SqlParameter UserId3 = new SqlParameter("@SenderId", senderId);
                    UserId3.Direction = ParameterDirection.Input;
                    objCommand3.Parameters.Add(UserId3);

                    DataSet mydata3 = dBConnect.GetDataSetUsingCmdObj(objCommand3);

                    Email email = new Email();
                    email.SenderName = mydata3.Tables[0].Rows[0]["UserName"].ToString();
                    email.ReceiverName = Session["UserName"].ToString();
                    email.Subject = mydata2.Tables[0].Rows[i]["EmailSubject"].ToString();
                    email.EmailBody = mydata2.Tables[0].Rows[i]["EmailBody"].ToString();
                    String createdTime = mydata2.Tables[0].Rows[i]["Timestamp"].ToString();
                    email.CreatedTime = DateTime.Parse(createdTime).ToShortDateString();

                    emailList.Add(email);
                }

                EmailMainView.DataSource = emailList;
                EmailMainView.DataBind();

                EmailMainView.Visible = true;
                LabelEmpty.Visible = false;
            }
            else
            {
                LabelEmpty.Text = "Your Inbox is empty.";
                LabelEmpty.Visible = true;
                EmailMainView.Visible = false;
            }
            SearchEmail.Text = "";

        }

        public void ViewInbox()
        {
            Session["Tag"] = "Inbox";
            DisplayView("Inbox");
            ComposeButtonView(false);
            buttonGroup(false);
            //   sideBarSelect(LinkButtonInbox);
        }

        public void buttonGroup(Boolean flag)
        {

            moveToDropDown.Visible = flag;
            btnFlag.Visible = flag;
            btnUnFlag.Visible = flag;
            btnDelete.Visible = flag;
            txtAddFolder.Visible = flag;
            btnAddFolder.Visible = flag;
            btnMoveToFolder.Visible = flag;
        }

        protected void LinkButtonInbox_Click(object sender, EventArgs e)
        {
            ViewInbox();
            OpenInboxEmailClient();
        }

        public void ComposeButtonView(bool tf)
        {
            if (tf == true)
            {
                ComposeTitle.Visible = true;
                FromWho.Visible = true;
                EmailFrom.Visible = true;
                ToWhom.Visible = true;
                EmailTo.Visible = true;
                EmailSubjectTitle.Visible = true;
                EmailSubject.Visible = true;
                EmailBodyTitle.Visible = true;
                EmailBody.Visible = true;
                btnBack.Visible = true;
             //   btnSearchEmail.Visible = true;
                btnSendEmail.Visible = true;
                LabelEmpty.Visible = false;
                EmailFrom.Text = Session["EmailAddress"].ToString();
                //have to hide other stuff that could be open
                EmailMainView.Visible = false;
            }
            else
            {
                ComposeTitle.Visible = false;
                FromWho.Visible = false;
                EmailFrom.Visible = false;
                ToWhom.Visible = false;
                EmailTo.Visible = false;
                EmailSubjectTitle.Visible = false;
                EmailSubject.Visible = false;
                EmailBodyTitle.Visible = false;
                EmailBody.Visible = false;
                btnBack.Visible = false;
              //  btnSearchEmail.Visible = false;
                btnSendEmail.Visible = false;
            }
        }
    
        public void showDetailViewOfEmail(bool tf)
        {

            if (tf == true)
            {
                SubjectT.Visible = true;

                SubjectD.Visible = true;
                createdTimeT.Visible = true;
                createdTimeD.Visible = true;
                fromWhoEmailT.Visible = true;
                fromEmailD.Visible = true;
                toWhoEmailT.Visible = true;
                toEmailD.Visible = true;
                emailBodyT.Visible = true;
                emailBodyD.Visible = true;
                btnBackD.Visible = true;
                LabelEmpty.Visible = false;
            }
            else
            {
                SubjectT.Visible = false;
                SubjectD.Visible = false;
                createdTimeT.Visible = false;
                createdTimeD.Visible = false;
                fromWhoEmailT.Visible = false;
                fromEmailD.Visible = false;
                toWhoEmailT.Visible = false;
                toEmailD.Visible = false;
                emailBodyT.Visible = false;
                emailBodyD.Visible = false;
                btnBackD.Visible = false;
            }
        }

        public void DisplayView(String tagsN)
        {
            String tagName = tagsN;
            String UserId = Session["UserId"].ToString();
            //Stored Procedure #4 Obtain TagId Where Tags.UserId = UserId and Tags.TagName = tagName

           // String strSql = "SELECT TagId FROM Tags WHERE Tags.UserId = " + Session["UserId"].ToString() + " AND Tags.TagName = '" + tagName + "'";

            SqlCommand objCommand4 = new SqlCommand();

            objCommand4.CommandType = CommandType.StoredProcedure;
            objCommand4.CommandText = "OpenInboxEmailClient";

            SqlParameter UserId1 = new SqlParameter("@UserId", UserId);
            UserId1.Direction = ParameterDirection.Input;
            objCommand4.Parameters.Add(UserId1);

            SqlParameter TagNameInbox1 = new SqlParameter("@TagNameInbox", tagName);
            TagNameInbox1.Direction = ParameterDirection.Input;
            objCommand4.Parameters.Add(TagNameInbox1);

            DataSet mydata = dBConnect.GetDataSetUsingCmdObj(objCommand4);

            //DataSet mydata = dBConnect.GetDataSet(strSql);
            String emailTag = mydata.Tables[0].Rows[0]["TagId"].ToString();

            //display the email gridview

            //Stored procedure #5 SelectSenderId, EmailSubect, EmailBody, and Timestamp

            //String strSql2 = "SELECT SenderId, EmailSubject, EmailBody, Timestamp " +
            //                    "FROM Emails, EmailRecipient " +
            //                    "WHERE EmailRecipient.UserId = " + Session["UserId"].ToString() + " " +
            //                    "AND EmailRecipient.EmailTag = " + emailTag + " " +
            //                    "AND Emails.EmailId = EmailRecipient.EmailId " +
            //                    "AND Emails.ReceiverId = " + Session["UserId"].ToString() + " ";


            SqlCommand objCommand5 = new SqlCommand();

            objCommand5.CommandType = CommandType.StoredProcedure;
            objCommand5.CommandText = "DisplayViewSenderIdEmailSubjectEmailBodyTimestamp";

            SqlParameter UserId2 = new SqlParameter("@UserId", UserId);
            UserId2.Direction = ParameterDirection.Input;
            objCommand5.Parameters.Add(UserId2);

            SqlParameter TagName5 = new SqlParameter("@EmailTag", emailTag);
            TagName5.Direction = ParameterDirection.Input;
            objCommand5.Parameters.Add(TagName5);

            DataSet mydata2 = dBConnect.GetDataSetUsingCmdObj(objCommand5);

          //  DataSet mydata2 = dBConnect.GetDataSet(strSql2);
            int size = mydata2.Tables[0].Rows.Count;
            if (size > 0)
            {
                ArrayList emailList = new ArrayList();
                for (int i = 0; i < size; i++)
                {
                    int senderId = Int32.Parse(mydata2.Tables[0].Rows[i]["SenderId"].ToString());

                    //StoredProcedure #6 Select the UserName and EmailAddress by SenderId


                    SqlCommand objCommand6 = new SqlCommand();

                    objCommand6.CommandType = CommandType.StoredProcedure;
                    objCommand6.CommandText = "SelectUserNameEmailAddressFromUsersWhereSenderId";

                    SqlParameter SenderId = new SqlParameter("@SenderId", senderId);
                    SenderId.Direction = ParameterDirection.Input;
                    objCommand6.Parameters.Add(SenderId);

                    DataSet mydata3 = dBConnect.GetDataSetUsingCmdObj(objCommand6);

                   // SelectUserNameEmailAddressFromUsersWhereSenderId


                    //String strSql3 = "SELECT UserName, EmailAddress FROM Users WHERE Users.UserId = " + senderId + " ";
                    //DataSet mydata3 = dBConnect.GetDataSet(strSql3);

                    Email email = new Email();
                    email.SenderName = mydata3.Tables[0].Rows[0]["UserName"].ToString();
                    email.ReceiverName = Session["UserName"].ToString();
                    email.Subject = mydata2.Tables[0].Rows[i]["EmailSubject"].ToString();
                    email.EmailBody = mydata2.Tables[0].Rows[i]["EmailBody"].ToString();
                    String createdTime = mydata2.Tables[0].Rows[i]["Timestamp"].ToString();
                    email.CreatedTime = DateTime.Parse(createdTime).ToShortDateString();

                    emailList.Add(email);
                }

            }
        }

        protected void btnAddFolder_Click(object sender, EventArgs e)
        {
            String newFolderName = txtAddFolder.Text;
            int UserId = Int32.Parse(Session["UserId"].ToString());

            //ObjCommand#7
            SqlCommand objCommand7 = new SqlCommand();

            objCommand7.CommandType = CommandType.StoredProcedure;
            objCommand7.CommandText = "InsertIntoTagNameUserid";

            SqlParameter NewFolderName = new SqlParameter("@NewFolderInt", newFolderName);
            NewFolderName.Direction = ParameterDirection.Input;
            objCommand7.Parameters.Add(NewFolderName);

            SqlParameter UserId1 = new SqlParameter("@UserId", UserId);
            UserId1.Direction = ParameterDirection.Input;
            objCommand7.Parameters.Add(UserId1);

            int ret = dBConnect.DoUpdateUsingCmdObj(objCommand7);
           // int ret = Int32.Parse(mydata.Tab);

        //    String sql = "INSERT INTO Tags (TagName, UserId) VALUES ('" + newFolderName + "', " + userId + ") ";
          //  int ret = dBConnect.DoUpdate(sql);
            if (ret > 0)
            {
                ComposeButtonView(false);
                showDetailViewOfEmail(false);
                buttonGroup(false);
                if (Session["UserId"] != null)
                {
                    OpenInboxEmailClient();
                    CreateTags();
                }
            }
            else
            {
                Response.Write("<script>alert('Failed to add new folder due to some reasons.')</script>");
            }
        }

        public void CreateTags()
        {
            //String tagSql = "SELECT TagName FROM Tags WHERE UserId = " + Session["UserId"].ToString();
            int UserId = Int32.Parse(Session["UserId"].ToString());
            //Stored Procedure Number #8: Select TagName From Tags Where UserId

            SqlCommand objCommand8 = new SqlCommand();

            objCommand8.CommandType = CommandType.StoredProcedure;
            objCommand8.CommandText = "PopulateTagsTagName";

            SqlParameter UserId8 = new SqlParameter("@UserId", UserId);
            UserId8.Direction = ParameterDirection.Input;
            objCommand8.Parameters.Add(UserId8);

            DataSet tagData = dBConnect.GetDataSetUsingCmdObj(objCommand8);

         //   DataSet tagData = dBConnect.GetDataSet(tagSql);

            ArrayList tags0 = new ArrayList();
            ArrayList tags = new ArrayList();
            int size = tagData.Tables[0].Rows.Count;
            if (size > 0)
            {
                for (int i = 0; i < size; i++)
                {
                    Tag tag0 = new Tag();
                    Tag tag = new Tag();
                    String tagName = tagData.Tables[0].Rows[i]["TagName"].ToString();

                    if (tagName.CompareTo("Sent") == 0 || tagName.CompareTo("Flag") == 0)
                    {
                        //do nothing
                    }
                    else
                    {
                        tag0.TagName = tagName;
                        tags0.Add(tag0);
                    }
                    if (tagName.CompareTo("Inbox") == 0 || tagName.CompareTo("Sent") == 0 || tagName.CompareTo("Flag") == 0 ||
                        tagName.CompareTo("Junk") == 0 || tagName.CompareTo("Trash") == 0)
                    {
                        //do nothing
                    }
                    else
                    {
                        tag.TagName = tagName;
                        tags.Add(tag);
                    }
                }
                moveToDropDown.DataSource = tags0;
                moveToDropDown.DataTextField = "TagName";
                moveToDropDown.DataValueField = "TagName";
                moveToDropDown.DataBind();
                moveToDropDown.Items.Insert(0, new ListItem("--Move to --", "0"));

                ddlCustomFolder.DataSource = tags;
                ddlCustomFolder.DataTextField = "TagName";
                ddlCustomFolder.DataValueField = "TagName";
                ddlCustomFolder.DataBind();
                ddlCustomFolder.Items.Insert(0, new ListItem("--Other Folders--", "0"));

            }
        }

        public void ViewFlag()
        {
            Session["Tag"] = "Flag";
            String tagName = LinkButtonFlag.Text;
            int UserId = Int32.Parse(Session["UserId"].ToString());
            //display the email gridview
            //String strSql2 = "SELECT SenderId, EmailSubject, EmailBody, Timestamp " +
            //                    "FROM Emails, EmailRecipient " +
            //                    "WHERE EmailRecipient.UserId = " + Session["UserId"].ToString() + " " +
            //                    "AND EmailRecipient.Flag =  1 " +
            //                    "AND Emails.ReceiverId = EmailRecipient.EmailId ";
            //StoredProcedure #9 ViewFlags that have EmailRecip.Flag = 1
            SqlCommand objCommand9 = new SqlCommand();

            objCommand9.CommandType = CommandType.StoredProcedure;
            objCommand9.CommandText = "ViewFlagSenderIdEmailSubjectEmailBody";

            SqlParameter UserId9 = new SqlParameter("@UserId", UserId);
            UserId9.Direction = ParameterDirection.Input;
            objCommand9.Parameters.Add(UserId9);

            DataSet mydata2 = dBConnect.GetDataSetUsingCmdObj(objCommand9);

           // DataSet mydata2 = dBConnect.GetDataSet(strSql2);
            int size = mydata2.Tables[0].Rows.Count;
            if (size > 0)
            {
                ArrayList emailList = new ArrayList();
                for (int i = 0; i < size; i++)
                {
                    int senderId = Int32.Parse(mydata2.Tables[0].Rows[i]["SenderId"].ToString());

                    //Stored Procedure #10 Select UserName, EmailAddress From Users.Users where SenderId
                    SqlCommand objCommand10 = new SqlCommand();

                    objCommand10.CommandType = CommandType.StoredProcedure;
                    objCommand10.CommandText = "ViewFlagUserNameEmailAddress";

                    SqlParameter SenderId = new SqlParameter("@SenderId", senderId);
                    SenderId.Direction = ParameterDirection.Input;
                    objCommand10.Parameters.Add(SenderId);

                    //String strSql3 = "SELECT UserName, EmailAddress FROM Users WHERE Users.UserId = " + senderId + " ";
                    DataSet mydata3 = dBConnect.GetDataSetUsingCmdObj(objCommand10);

                    Email email = new Email();
                    email.SenderName = mydata3.Tables[0].Rows[0]["UserName"].ToString();
                    email.Subject = mydata2.Tables[0].Rows[i]["EmailSubject"].ToString();
                    email.EmailBody = mydata2.Tables[0].Rows[i]["EmailBody"].ToString();
                    String createdTime = mydata2.Tables[0].Rows[i]["Timestamp"].ToString();
                    email.CreatedTime = DateTime.Parse(createdTime).ToShortDateString();
                }
            }
        }

        protected void btnSearchEmail_Click(object sender, EventArgs e)
        {
            int UserId = Int32.Parse(Session["UserId"].ToString());
            String searchContent = SearchEmail.Text;
            //String sql = "SELECT SenderId, EmailSubject, EmailBody, Timestamp " +
            //                "FROM Emails, EmailRecipient " +
            //                "WHERE EmailRecipient.UserId = " + Session["UserId"].ToString() + " " +
            //                "AND EmailRecipient.EmailId = Emails.EmailId " +
            //                "AND Emails.EmailSubject LIKE '%" + searchContent + "%' " +
            //                "AND (EmailSubject LIKE '%" + searchContent + "%' " +
            //                "OR EmailBody LIKE '%" + searchContent + "%') ";

            //StoredProcedure#11 ButtonSearchEmail to find SenderId, EmailSubject, EmailBody, Timestamp
            SqlCommand objCommand11 = new SqlCommand();

            objCommand11.CommandType = CommandType.StoredProcedure;
            objCommand11.CommandText = "btnSearchStatment";

            SqlParameter UserId11 = new SqlParameter("@UserId", UserId);
            UserId11.Direction = ParameterDirection.Input;
            objCommand11.Parameters.Add(UserId11);

            SqlParameter SearchContent11 = new SqlParameter("@searchContent", searchContent);
            SearchContent11.Direction = ParameterDirection.Input;
            objCommand11.Parameters.Add(SearchContent11);

            DataSet mydata = dBConnect.GetDataSetUsingCmdObj(objCommand11);

           // DataSet mydata = dBConnect.GetDataSet(sql);
            int size = mydata.Tables[0].Rows.Count;
            if (size > 0)
            {

                ArrayList emailList = new ArrayList();
                for (int i = 0; i < size; i++)
                {
                    int senderId = Int32.Parse(mydata.Tables[0].Rows[i]["SenderId"].ToString());
                    String strSql3 = "SELECT UserName, EmailAddress FROM Users WHERE Users.UserId = '" + senderId + "' ";
                    //Stored Procedure #12 Already Wrote this stored Procedure
                    //SelectUserNameEmailAddressFromUsersWhereSenderId
                    SqlCommand objCommand12 = new SqlCommand();

                    objCommand12.CommandType = CommandType.StoredProcedure;
                    objCommand12.CommandText = "SelectUserNameEmailAddressFromUsersWhereSenderId";

                    SqlParameter SenderId = new SqlParameter("@SenderId", senderId);
                    SenderId.Direction = ParameterDirection.Input;
                    objCommand12.Parameters.Add(SenderId);

                    DataSet mydata3 = dBConnect.GetDataSetUsingCmdObj(objCommand12);



                    //DataSet mydata3 = dBConnect.GetDataSet(strSql3);

                    Email email = new Email();
                    email.SenderName = mydata3.Tables[0].Rows[0]["UserName"].ToString();
                    //email.ReceiverName = Session["UserName"].ToString();
                    email.Subject = mydata.Tables[0].Rows[i]["EmailSubject"].ToString();
                    email.EmailBody = mydata.Tables[0].Rows[i]["EmailBody"].ToString();
                    String createdTime = mydata.Tables[0].Rows[i]["Timestamp"].ToString();
                    email.CreatedTime = DateTime.Parse(createdTime).ToShortDateString();

                    emailList.Add(email);
                }
                EmailMainView.DataSource = emailList;
                EmailMainView.DataBind();

                EmailMainView.Visible = true;
                LabelEmpty.Visible = false;
            }
            else
            {
                LabelEmpty.Text = "Your search content is empty.";
                LabelEmpty.Visible = true;
                EmailMainView.Visible = false;
            }

        }

        protected void btnCompose_Click(object sender, EventArgs e)
        {
            ComposeButtonView(true);
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            ComposeButtonView(false);
        }

        public bool checkUnique(string emailaddress) {
            SqlCommand objCommand8 = new SqlCommand();

            objCommand8.CommandType = CommandType.StoredProcedure;
            objCommand8.CommandText = "CreateNewUserValidation";

            SqlParameter EmailAddressId8 = new SqlParameter("@emailaddress", emailaddress);
            EmailAddressId8.Direction = ParameterDirection.Input;
            objCommand8.Parameters.Add(EmailAddressId8);

            DataSet mydata = dBConnect.GetDataSetUsingCmdObj(objCommand8);
            //    DataSet mydata = dbConnect.GetDataSet(sql);
            int size = Int32.Parse(mydata.Tables[0].Rows.Count.ToString());

            if (size > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        protected void btnSendEmail_Click(object sender, EventArgs e)
        {
            //capture all data so sender, receiver, email subject, body, time
            ArrayList emailList = new ArrayList();

            String senderName = EmailFrom.Text;
            String receiverName = EmailTo.Text;
            String subject = EmailSubject.Text;
            String body = EmailBody.Text;

            int senderId = Int32.Parse(Session["UserId"].ToString());

            //find if email is valid
            if (checkUnique(receiverName) == true)
            {
                LabelValidation.Text = "Email does not exists please enter a valid email";
            }

            else if (checkUnique(receiverName) == false)
            {






                //Stored Procedure #13 Select UserId From Users Where EmailAddress is receiverName
                SqlCommand objCommand13 = new SqlCommand();

                objCommand13.CommandType = CommandType.StoredProcedure;
                objCommand13.CommandText = "btnSendEmailFindEmail";

                SqlParameter SenderId13 = new SqlParameter("@receiverName", receiverName);
                SenderId13.Direction = ParameterDirection.Input;
                objCommand13.Parameters.Add(SenderId13);

                DataSet data = dBConnect.GetDataSetUsingCmdObj(objCommand13);
                //String sqlReceiver = "SELECT UserId FROM Users WHERE EmailAddress = '" + receiverName + "'";
                //dBConnect = new DBConnect();
                //DataSet data = dBConnect.GetDataSet(sqlReceiver);
                int sqlReceiverId = Int32.Parse(data.Tables[0].Rows[0]["UserId"].ToString());

                //Stored Procedure #14 Insert information into emails
                SqlCommand objCommand14 = new SqlCommand();

                objCommand14.CommandType = CommandType.StoredProcedure;
                objCommand14.CommandText = "btnSendEmailInsertEmail";

                SqlParameter SenderId14 = new SqlParameter("@SenderId", senderId);
                SenderId14.Direction = ParameterDirection.Input;
                objCommand14.Parameters.Add(SenderId14);


                SqlParameter ReceiverId14 = new SqlParameter("@SqlReceiverId", sqlReceiverId);
                ReceiverId14.Direction = ParameterDirection.Input;
                objCommand14.Parameters.Add(ReceiverId14);

                SqlParameter EmailSubject14 = new SqlParameter("@EmailSubject", subject);
                EmailSubject14.Direction = ParameterDirection.Input;
                objCommand14.Parameters.Add(EmailSubject14);

                SqlParameter EmailBody14 = new SqlParameter("@EmailBody", body);
                EmailBody14.Direction = ParameterDirection.Input;
                objCommand14.Parameters.Add(EmailBody14);

                int insertedData = dBConnect.DoUpdateUsingCmdObj(objCommand14);

                //String sql = "INSERT INTO Emails (Senderid, Receiverid, EmailSubject, EmailBody, Timestamp) "
                //        + "VALUES ('" + senderId + "', '" + sqlReceiverId + "', '" + subject + "', '" + body +
                //        "', '" + DateTime.Now + "')";
                // int insertedData = dBConnect.DoUpdate(sql);


                //Stored Procedure #15 Select all from emails where senderId is senderId and orderd by Desc

                SqlCommand objCommand15 = new SqlCommand();

                objCommand15.CommandType = CommandType.StoredProcedure;
                objCommand15.CommandText = "btnSendEmailSelectAllEmailsDESCOrder";

                SqlParameter SenderId15 = new SqlParameter("@SenderId", senderId);
                SenderId15.Direction = ParameterDirection.Input;
                objCommand15.Parameters.Add(SenderId15);
                DataSet mydata2 = dBConnect.GetDataSetUsingCmdObj(objCommand15);

                //  String sqlGetEmail = "SELECT * FROM Emails WHERE Senderid = '" + senderId + "' ORDER BY EmailId DESC";
                //  DataSet mydata2 = dBConnect.GetDataSet(sqlGetEmail);
                int emailId = Int32.Parse(mydata2.Tables[0].Rows[0]["EmailId"].ToString());
                String timestampforObject = mydata2.Tables[0].Rows[0]["Timestamp"].ToString(); ;

                //Stored Procedure 17 Select TagId From Tags where TagName is Sent
                // btnSendEmailWhereTagNameSent;

                SqlCommand objCommand17 = new SqlCommand();

                objCommand17.CommandType = CommandType.StoredProcedure;
                objCommand17.CommandText = "btnSendEmailWhereTagNameSent";

                SqlParameter SenderId17 = new SqlParameter("@SenderId", senderId);
                SenderId17.Direction = ParameterDirection.Input;
                objCommand17.Parameters.Add(SenderId17);
                DataSet getSenderTagId = dBConnect.GetDataSetUsingCmdObj(objCommand17);

                //   String sqlGetSenderTagId = "SELECT TagId FROM Tags WHERE UserId = '" + senderId + "' AND TagName = 'Sent'";
                //   DataSet getSenderTagId = dBConnect.GetDataSet(sqlGetSenderTagId);
                int senderTagId = Int32.Parse(getSenderTagId.Tables[0].Rows[0]["TagId"].ToString());

                //Stored Procedure #18 Insert INTO EmailRecipient
                //BtnSendEmailInsertIntoEmailRecip

                SqlCommand objCommand18 = new SqlCommand();

                objCommand18.CommandType = CommandType.StoredProcedure;
                objCommand18.CommandText = "BtnSendEmailInsertIntoEmailRecip";

                SqlParameter SenderId18 = new SqlParameter("@SenderId", senderId);
                SenderId18.Direction = ParameterDirection.Input;
                objCommand18.Parameters.Add(SenderId18);

                SqlParameter EmailId18 = new SqlParameter("@EmailId", emailId);
                EmailId18.Direction = ParameterDirection.Input;
                objCommand18.Parameters.Add(EmailId18);

                SqlParameter SenderTagId18 = new SqlParameter("@SenderTagId", senderTagId);
                SenderTagId18.Direction = ParameterDirection.Input;
                objCommand18.Parameters.Add(SenderTagId18);

                int emailRecipientSuccess = dBConnect.DoUpdateUsingCmdObj(objCommand18);

                //String sqlInsertTag = "INSERT INTO EmailRecipient (UserId, EmailId, EmailTag, Flag) " +
                //"VALUES ('" + senderId + "', '" + emailId + "', '" + senderTagId + "', '" + 0 + "')";
                //dBConnect = new DBConnect();
                //int emailRecipientSuccess = dBConnect.DoUpdate(sqlInsertTag);

                //StoredProcedure#19 SelectTagId From Tags WHERE UserId = ReceiverId and TagName is inbox
                //[btnSendEmailGetInboxOfReceiver]

                SqlCommand objCommand19 = new SqlCommand();

                objCommand19.CommandType = CommandType.StoredProcedure;
                objCommand19.CommandText = "btnSendEmailGetInboxOfReceiver";

                SqlParameter SqlReceieverId19 = new SqlParameter("@sqlReceiverId", sqlReceiverId);
                SqlReceieverId19.Direction = ParameterDirection.Input;
                objCommand19.Parameters.Add(SqlReceieverId19);


                DataSet receiverTagId1 = dBConnect.GetDataSetUsingCmdObj(objCommand19);
                int receiverTagId = Int32.Parse(receiverTagId1.Tables[0].Rows[0]["TagId"].ToString());

                //String sqlGetReceiverTagId = "SELECT TagId FROM Tags WHERE UserId = '" + sqlReceiverId + "' AND TagName = 'Inbox'";
                //DataSet getReceiverTagId = dBConnect.GetDataSet(sqlGetReceiverTagId);
                //int receiverTagId = Int32.Parse(getReceiverTagId.Tables[0].Rows[0]["TagId"].ToString());

                //StoredProcedure this statement looks the same to statement 18

                //        String sqlIntoRecipient = "INSERT INTO EmailRecipient (UserId, EmailId, EmailTag, Flag) " +
                //"       VALUES ('" + sqlReceiverId + "', '" + emailId + "', '" + receiverTagId + "', '" + 0 + "')";
                //        dBConnect = new DBConnect();
                //        int emailRecipientSuccess2 = dBConnect.DoUpdate(sqlIntoRecipient);


                SqlCommand objCommand20 = new SqlCommand();

                objCommand20.CommandType = CommandType.StoredProcedure;
                objCommand20.CommandText = "BtnSendEmailInsertIntoEmailRecip";

                SqlParameter SenderId20 = new SqlParameter("@SenderId", sqlReceiverId);
                SenderId20.Direction = ParameterDirection.Input;
                objCommand20.Parameters.Add(SenderId20);

                SqlParameter EmailId20 = new SqlParameter("@EmailId", emailId);
                EmailId20.Direction = ParameterDirection.Input;
                objCommand20.Parameters.Add(EmailId20);

                SqlParameter SenderTagId20 = new SqlParameter("@SenderTagId", receiverTagId);
                SenderTagId20.Direction = ParameterDirection.Input;
                objCommand20.Parameters.Add(SenderTagId20);

                int emailRecipientSuccess2 = dBConnect.DoUpdateUsingCmdObj(objCommand20);

                //Stored Procedure #21 Where SELECT all from Emails Where SEnder ID = SenderId

                //   String sqlSelectFromEmailTable = "SELECT * FROM Emails WHERE Senderid = '" + senderId + "'";
                //   DataSet emailData = dBConnect.GetDataSet(sqlSelectFromEmailTable);

                SqlCommand objCommand21 = new SqlCommand();

                objCommand21.CommandType = CommandType.StoredProcedure;
                objCommand21.CommandText = "btnSendEmailSelectAllEmailsWithSenderId";

                SqlParameter SenderId21 = new SqlParameter("@SenderId", senderId);
                SenderId21.Direction = ParameterDirection.Input;
                objCommand21.Parameters.Add(SenderId21);

                DataSet emailData = dBConnect.GetDataSetUsingCmdObj(objCommand21);

                //Stored Procedure #22 FindtheReceiverName
                //[btnSendEmailFindtheRecieverName]

                SqlCommand objCommand22 = new SqlCommand();

                objCommand22.CommandType = CommandType.StoredProcedure;
                objCommand22.CommandText = "btnSendEmailFindtheRecieverName";

                SqlParameter ReceiverId22 = new SqlParameter("@ReceieverId", sqlReceiverId);
                ReceiverId22.Direction = ParameterDirection.Input;
                objCommand22.Parameters.Add(ReceiverId22);

                DataSet emailData2 = dBConnect.GetDataSetUsingCmdObj(objCommand22);


                //    String findtheReceiverUserName = "SELECT UserName FROM Users WHERE UserId = '" + sqlReceiverId + "'";
                //  DataSet emailData2 = dBConnect.GetDataSet(findtheReceiverUserName);
                String receiverNameFinal = emailData2.Tables[0].Rows[0]["UserName"].ToString();

                Email email = new Email();
                //You are sending the email
                email.SenderName = Session["UserName"].ToString();
                email.ReceiverName = receiverNameFinal;
                email.Subject = subject;
                email.EmailBody = body;
                String createdTime = timestampforObject;
                email.CreatedTime = DateTime.Parse(createdTime).ToShortDateString();

                emailList.Add(email);

                EmailMainView.DataSource = emailList;
                EmailMainView.DataBind();
                LabelValidation.Visible = false;
                LabelValidation.Text = "";
                ComposeButtonView(false);
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

        protected void LinkButtonSent_Click(object sender, EventArgs e)
        {
            int senderId = Int32.Parse(Session["UserId"].ToString());

            //find username
            //Stored Procedure #22 Select all from Emails with senderId 

            //[LinkButtonSentFindAllEmailsOfSender]

            SqlCommand objCommand22 = new SqlCommand();

            objCommand22.CommandType = CommandType.StoredProcedure;
            objCommand22.CommandText = "LinkButtonSentFindAllEmailsOfSender";

            SqlParameter SenderId22 = new SqlParameter("@SenderId", senderId);
            SenderId22.Direction = ParameterDirection.Input;
            objCommand22.Parameters.Add(SenderId22);

            DataSet emailData = dBConnect.GetDataSetUsingCmdObj(objCommand22);

            //String sqlSelectFromEmailTable = "SELECT * FROM Emails WHERE SenderId = '" + senderId + "'";
            //DataSet emailData = dBConnect.GetDataSet(sqlSelectFromEmailTable);

            ArrayList emailList = new ArrayList();
            int size = emailData.Tables[0].Rows.Count;
            for (int i = 0; i < size; i++)
            {

                //StoredProcedure#23 Find EmailSubkect, EmailBody, Timestamp, receievegerId FROM Emails
                SqlCommand objCommand23 = new SqlCommand();

                objCommand23.CommandType = CommandType.StoredProcedure;
                objCommand23.CommandText = "LinkSentFindEmailInformationOfReciever";

                SqlParameter SenderId23 = new SqlParameter("@SenderId", senderId);
                SenderId23.Direction = ParameterDirection.Input;
                objCommand23.Parameters.Add(SenderId23);

                //[LinkSentFindEmailInformationOfReciever]

                DataSet emaildata21 = dBConnect.GetDataSetUsingCmdObj(objCommand23);

               // String sqlSelectReceiverId = "SELECT EmailSubject, EmailBody, Timestamp, ReceiverId FROM Emails WHERE SenderId = " + senderId + " ";
               // DataSet emaildata21 = dBConnect.GetDataSet(sqlSelectReceiverId);
                int receiverID = Int32.Parse(emaildata21.Tables[0].Rows[i]["ReceiverID"].ToString());

                //Keep row at one because SQL changes in the loop will not reach past first row

                //Stored Procedure #24 FindUserName of Receiever id
                SqlCommand objCommand24 = new SqlCommand();

                objCommand24.CommandType = CommandType.StoredProcedure;
                objCommand24.CommandText = "LinkBtnSentUserNameFromUsersWithReceieverId";

                SqlParameter ReceieverId24 = new SqlParameter("@ReceiverId", receiverID);
                ReceieverId24.Direction = ParameterDirection.Input;
                objCommand24.Parameters.Add(ReceieverId24);

                //[LinkSentFindEmailInformationOfReciever]

                DataSet emailData2 = dBConnect.GetDataSetUsingCmdObj(objCommand24);

               // String sqlSelectUserNameRecieverId = "SELECT UserName FROM Users WHERE UserId = " + receiverID + " ";
                //DataSet emailData2 = dBConnect.GetDataSet(sqlSelectUserNameRecieverId);
                String receiverUserName = emailData2.Tables[0].Rows[0]["UserName"].ToString();

                //Keep row at one because SQL changes in the loop will not reach past first row

                //StoredProcedure#25 Find the userName from users where UserId is the senderId

                SqlCommand objCommand25 = new SqlCommand();

                objCommand25.CommandType = CommandType.StoredProcedure;
                objCommand25.CommandText = "LinkButtonSentUserNameFROMUsersWHEREUserIdIsSenderId";

                SqlParameter SenderId25 = new SqlParameter("@SenderId", senderId);
                SenderId25.Direction = ParameterDirection.Input;
                objCommand25.Parameters.Add(SenderId25);

                //[LinkSentFindEmailInformationOfReciever]

                DataSet emailData3 = dBConnect.GetDataSetUsingCmdObj(objCommand25);

                //String sqlSelectUserNameSessionUser = "SELECT UserName FROM Users WHERE UserId = " + senderId + " ";
               // DataSet emailData3 = dBConnect.GetDataSet(sqlSelectUserNameSessionUser);
                String UserNameSessionUser = emailData3.Tables[0].Rows[0]["UserName"].ToString();

                String emailsubject = emailData.Tables[0].Rows[i]["EmailSubject"].ToString();
                String emailBody = emailData.Tables[0].Rows[i]["EmailBody"].ToString();
                String CreatedTime = emailData.Tables[0].Rows[i]["Timestamp"].ToString();


                Email email = new Email();
                email.SenderName = UserNameSessionUser;
                email.ReceiverName = receiverUserName;
                email.Subject = emailsubject;
                email.EmailBody = emailBody;
                email.CreatedTime = CreatedTime;

                emailList.Add(email);
            }

            EmailMainView.DataSource = emailList;
            EmailMainView.DataBind();

           // int size = emailData.Tables[0].Rows.Count;
            if (size == 0)
            {
                EmailMainView.Visible = false;
                LabelEmpty.Text = "You Have Not Sent Any Emails.";
            }
            else
            {
                LabelEmpty.Text = "";
                EmailMainView.Visible = true;
            }
            ViewSent();
        }

        public void ViewSent()
        {
            Session["Tag"] = "Sent";
            DisplayView("Sent");
            ComposeButtonView(false);
            buttonGroup(false);
        }

        protected void chkSelectEmail_CheckedChanged(object sender, EventArgs e)
        {
            buttonGroup(true);
        }

        protected void EmailMainView_SelectedIndexChanged(object sender, EventArgs e)
        {
            showDetailViewOfEmail(true);
        }

        protected void EmailMainView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int rowIndex = int.Parse(e.CommandArgument.ToString());

            String senderEmail = EmailMainView.Rows[rowIndex].Cells[2].Text;
            fromEmailD.Text = senderEmail;

            String ReceiverEmail = EmailMainView.Rows[rowIndex].Cells[3].Text;
            toEmailD.Text = ReceiverEmail;

            String subject = EmailMainView.Rows[rowIndex].Cells[4].Text;
            SubjectD.Text = subject;

            String content = EmailMainView.Rows[rowIndex].Cells[5].Text;
            emailBodyD.Text = content;

            String createdTime = EmailMainView.Rows[rowIndex].Cells[6].Text;
            createdTimeD.Text = createdTime;
            showDetailViewOfEmail(true);
            ComposeButtonView(false);
        }

        protected void LinkButtonJunk_Click(object sender, EventArgs e)
        {
            int receiverId = Int32.Parse(Session["UserId"].ToString());
           Session["Tag"] = "Junk";
           String junk = Session["Tag"].ToString();

            //Stored Procedure #26 Select the TagId From Tags Where Tags.UserId gets the receiever Id and the junk
            SqlCommand objCommand26 = new SqlCommand();

            objCommand26.CommandType = CommandType.StoredProcedure;
            objCommand26.CommandText = "SelectTagIdFromTagsForJunk";

            SqlParameter RecieverId26 = new SqlParameter("@ReceiverId", receiverId);
            RecieverId26.Direction = ParameterDirection.Input;
            objCommand26.Parameters.Add(RecieverId26);

            SqlParameter TagNameId26 = new SqlParameter("@TagName", junk);
            TagNameId26.Direction = ParameterDirection.Input;
            objCommand26.Parameters.Add(TagNameId26);

            DataSet dsTagId = dBConnect.GetDataSetUsingCmdObj(objCommand26);

          //  String sqlTagId = "SELECT TagId FROM Tags WHERE Tags.UserId = " + receiverId + " AND Tags.TagName = '" + junk + "'";
         //   DataSet dsTagId = dBConnect.GetDataSet(sqlTagId);
            int tagId = Int32.Parse(dsTagId.Tables[0].Rows[0]["TagId"].ToString());

            //Stored Procedure #27 SELECT all emails from Junk
            //[SelectAllEmailsForJunk]

            SqlCommand objCommand27 = new SqlCommand();

            objCommand27.CommandType = CommandType.StoredProcedure;
            objCommand27.CommandText = "SelectAllEmailsForJunk";

            SqlParameter RecieverId27 = new SqlParameter("@ReceiverId", receiverId);
            RecieverId27.Direction = ParameterDirection.Input;
            objCommand27.Parameters.Add(RecieverId27);

            SqlParameter TagId27 = new SqlParameter("@TagId", tagId);
            TagId27.Direction = ParameterDirection.Input;
            objCommand27.Parameters.Add(TagId27);

            DataSet emailData = dBConnect.GetDataSetUsingCmdObj(objCommand27);

            //String sqlSelectFromEmailTable = "SELECT * FROM Emails, EmailRecipient " +
            //    "WHERE Emails.EmailId = EmailRecipient.EmailId " +
            //    "AND Emails.Receiverid = '" + receiverId + "' " +
            //    "AND EmailRecipient.EmailTag = '" + tagId + "'";
            //DataSet emailData = dBConnect.GetDataSet(sqlSelectFromEmailTable);
            EmailMainView.DataSource = emailData;
            EmailMainView.DataBind();

            int size = emailData.Tables[0].Rows.Count;
            if (size == 0)
            {
                EmailMainView.Visible = false;
                LabelEmpty.Text = "You have no emails in Junk.";
            }
            else
            {
                LabelEmpty.Text = "";
                EmailMainView.Visible = true;
            }

            ViewJunk();
        }

        public void ViewJunk()
        {
            Session["Tag"] = "Junk";
            DisplayView("Junk");
            ComposeButtonView(false);
            buttonGroup(false);
        }

        protected void LinkButtonTrash_Click(object sender, EventArgs e)
        {
            int receiverId = Int32.Parse(Session["UserId"].ToString());
            Session["Tag"] = "Trash";
            String junk = Session["Tag"].ToString();

            //Stored Procedure #28 same format as Junk link
            SqlCommand objCommand28 = new SqlCommand();

            objCommand28.CommandType = CommandType.StoredProcedure;
            objCommand28.CommandText = "SelectTagIdFromTagsForJunk";

            SqlParameter RecieverId28 = new SqlParameter("@ReceiverId", receiverId);
            RecieverId28.Direction = ParameterDirection.Input;
            objCommand28.Parameters.Add(RecieverId28);

            SqlParameter TagNameId28 = new SqlParameter("@TagName", junk);
            TagNameId28.Direction = ParameterDirection.Input;
            objCommand28.Parameters.Add(TagNameId28);

            DataSet dsTagId = dBConnect.GetDataSetUsingCmdObj(objCommand28);



          //  String sqlTagId = "SELECT TagId FROM Tags WHERE Tags.UserId = " + receiverId + " AND Tags.TagName = '" + junk + "'";
          //  DataSet dsTagId = dBConnect.GetDataSet(sqlTagId);
            int tagId = Int32.Parse(dsTagId.Tables[0].Rows[0]["TagId"].ToString());


            //Stored Procedure #29 SELECT all emails from Trash Same as junk Stored Procedure
            //[SelectAllEmailsForJunk]

            SqlCommand objCommand29 = new SqlCommand();

            objCommand29.CommandType = CommandType.StoredProcedure;
            objCommand29.CommandText = "SelectAllEmailsForJunk";

            SqlParameter RecieverId29 = new SqlParameter("@ReceiverId", receiverId);
            RecieverId29.Direction = ParameterDirection.Input;
            objCommand29.Parameters.Add(RecieverId29);

            SqlParameter TagId29 = new SqlParameter("@TagId", tagId);
            TagId29.Direction = ParameterDirection.Input;
            objCommand29.Parameters.Add(TagId29);

            DataSet emailData = dBConnect.GetDataSetUsingCmdObj(objCommand29);


            //String sqlSelectFromEmailTable = "SELECT * FROM Emails, EmailRecipient " +
            //    "WHERE Emails.EmailId = EmailRecipient.EmailId " +
            //    "AND Emails.Receiverid = '" + receiverId + "' " +
            //    "AND EmailRecipient.EmailTag = '" + tagId + "'";

          //  DataSet emailData = dBConnect.GetDataSet(sqlSelectFromEmailTable);

            ArrayList emailList = new ArrayList();
            int size = emailData.Tables[0].Rows.Count;
            for (int i = 0; i < size; i++)
            {
                int senderId = Int32.Parse(emailData.Tables[0].Rows[0]["SenderId"].ToString());
                //Keep row at one because SQL changes in the loop will not reach past first row

                //Stored Procedure #30 this is similar to emails StoredProcedure
                //                CREATE PROCEDURE[dbo].[SelectUserNameEmailAddressFromUsersWhereSenderId]
                //        @SenderId int
                //    AS
                // SELECT UserName, EmailAddress FROM Users WHERE Users.UserId = @SenderId
                //RETURN 0
                SqlCommand objCommand30 = new SqlCommand();

                objCommand30.CommandType = CommandType.StoredProcedure;
                objCommand30.CommandText = "SelectUserNameEmailAddressFromUsersWhereSenderId";

                SqlParameter RecieverId30 = new SqlParameter("@SenderId", senderId);
                RecieverId30.Direction = ParameterDirection.Input;
                objCommand30.Parameters.Add(RecieverId30);


                DataSet emailData3 = dBConnect.GetDataSetUsingCmdObj(objCommand30);

              //  String sqlSelectUserNameSessionUser = "SELECT UserName FROM Users WHERE UserId = " + senderId + " ";
              //  DataSet emailData3 = dBConnect.GetDataSet(sqlSelectUserNameSessionUser);
                String UserNameSessionUser = emailData3.Tables[0].Rows[0]["UserName"].ToString();

                //Stored Procedure #31 Similar to All emails Stored Procedure


                //                CREATE PROCEDURE[dbo].[LinkSentFindEmailInformationOfReciever]
                //        @SenderId int

                //    AS
                // SELECT EmailSubject, EmailBody, "Timestamp", ReceiverId FROM Emails WHERE SenderId = @SenderId
                //RETURN 0

                SqlCommand objCommand31 = new SqlCommand();

                objCommand31.CommandType = CommandType.StoredProcedure;
                objCommand31.CommandText = "LinkSentFindEmailInformationOfReciever";

                SqlParameter SenderId31 = new SqlParameter("@SenderId", senderId);
                SenderId31.Direction = ParameterDirection.Input;
                objCommand31.Parameters.Add(SenderId31);


                DataSet emaildata21 = dBConnect.GetDataSetUsingCmdObj(objCommand31);


         //       String sqlSelectReceiverId = "SELECT EmailSubject, EmailBody, Timestamp, ReceiverId FROM Emails WHERE SenderId = " + senderId + " ";
         //       DataSet emaildata21 = dBConnect.GetDataSet(sqlSelectReceiverId);
                int receiverID = Int32.Parse(emaildata21.Tables[0].Rows[i]["ReceiverID"].ToString());

                //StoredProcedure #32 Similar to All Emails Stored Procedure

                //                CREATE PROCEDURE[dbo].[LinkBtnSentUserNameFromUsersWithReceieverId]
                //        @ReceiverId int
                //    AS
                // SELECT UserName FROM Users WHERE UserId = @ReceiverId
                //RETURN 0

                SqlCommand objCommand32 = new SqlCommand();

                objCommand32.CommandType = CommandType.StoredProcedure;
                objCommand32.CommandText = "LinkBtnSentUserNameFromUsersWithReceieverId";

                SqlParameter RecieverId32 = new SqlParameter("@ReceiverId", receiverID);
                RecieverId32.Direction = ParameterDirection.Input;
                objCommand32.Parameters.Add(RecieverId32);


                DataSet emailData2 = dBConnect.GetDataSetUsingCmdObj(objCommand32);


                //Keep row at one because SQL changes in the loop will not reach past first row
          //      String sqlSelectUserNameRecieverId = "SELECT UserName FROM Users WHERE UserId = " + receiverID + " ";
          //      DataSet emailData2 = dBConnect.GetDataSet(sqlSelectUserNameRecieverId);
                String receiverUserName = emailData2.Tables[0].Rows[0]["UserName"].ToString();


                String emailsubject = emailData.Tables[0].Rows[i]["EmailSubject"].ToString();
                String emailBody = emailData.Tables[0].Rows[i]["EmailBody"].ToString();
                String CreatedTime = emailData.Tables[0].Rows[i]["Timestamp"].ToString();


                Email email = new Email();
                email.SenderName = UserNameSessionUser;
                email.ReceiverName = Session["UserName"].ToString();
                email.Subject = emailsubject;
                email.EmailBody = emailBody;
                email.CreatedTime = CreatedTime;

                emailList.Add(email);
            }

            EmailMainView.DataSource = emailList;
            EmailMainView.DataBind();

            // int size = emailData.Tables[0].Rows.Count;
            if (size == 0)
            {
                EmailMainView.Visible = false;
                LabelEmpty.Text = "You Have No Trash Emails.";
            }
            else
            {
                LabelEmpty.Text = "";
                EmailMainView.Visible = true;
            }
            ViewTrash();

        }

        public void ViewTrash()
        {
            Session["Tag"] = "Trash";
            DisplayView("Trash");
            ComposeButtonView(false);
            buttonGroup(false);
        }

        protected void LinkButtonFlag_Click(object sender, EventArgs e)
        {

            int receiverId = Int32.Parse(Session["UserId"].ToString());
            int flagnum = 1;
            //Stored Procedure #33 Select all emails where the flag num is 1

            //            CREATE PROCEDURE[dbo].[SelectAllFromEmailsAndRecipientWhereFlagNumIs]
            //        @ReceiverId int,
            //	@Flagnum int
            //AS
            //SELECT* FROM Emails, EmailRecipient WHERE Emails.EmailId = EmailRecipient.EmailId
            //AND ReceiverId = @ReceiverId
            //AND EmailRecipient.Flag = @Flagnum
            //RETURN 0

            SqlCommand objCommand33 = new SqlCommand();

            objCommand33.CommandType = CommandType.StoredProcedure;
            objCommand33.CommandText = "SelectAllFromEmailsAndRecipientWhereFlagNumIs";

            SqlParameter RecieverId33 = new SqlParameter("@ReceiverId", receiverId);
            RecieverId33.Direction = ParameterDirection.Input;
            objCommand33.Parameters.Add(RecieverId33);

            SqlParameter FlagNum33 = new SqlParameter("@Flagnum", flagnum);
            FlagNum33.Direction = ParameterDirection.Input;
            objCommand33.Parameters.Add(FlagNum33);

            DataSet emailData = dBConnect.GetDataSetUsingCmdObj(objCommand33);


            //String sqlSelectFromEmailTable = "SELECT * FROM Emails, EmailRecipient WHERE Emails.EmailId = " +
            //    "EmailRecipient.EmailId AND ReceiverId = " + receiverId +
            //    " AND EmailRecipient.Flag = " + flagnum + "";
        //    DataSet emailData = dBConnect.GetDataSet(sqlSelectFromEmailTable);


            ArrayList emailList = new ArrayList();
            int size = emailData.Tables[0].Rows.Count;
            for (int i = 0; i < size; i++)
            {



                int senderId = Int32.Parse(emailData.Tables[0].Rows[0]["SenderId"].ToString());

                //Stored Procedure #34 Where You Select the UserName FROM Users WHEREUserId = sender Id
                //I have used this same stored procedure
                //                CREATE PROCEDURE[dbo].[LinkButtonSentUserNameFROMUsersWHEREUserIdIsSenderId]
                //        @SenderId int
                //        AS
                //SELECT UserName FROM Users WHERE UserId = @SenderId
                //RETURN 0

                SqlCommand objCommand34 = new SqlCommand();

                objCommand34.CommandType = CommandType.StoredProcedure;
                objCommand34.CommandText = "LinkButtonSentUserNameFROMUsersWHEREUserIdIsSenderId";

                SqlParameter SenderId34 = new SqlParameter("@SenderId", senderId);
                SenderId34.Direction = ParameterDirection.Input;
                objCommand34.Parameters.Add(SenderId34);

                DataSet emailData3 = dBConnect.GetDataSetUsingCmdObj(objCommand34);

                //Keep row at one because SQL changes in the loop will not reach past first row
     //           String sqlSelectUserNameSessionUser = "SELECT UserName FROM Users WHERE UserId = " + senderId + " ";
     //           DataSet emailData3 = dBConnect.GetDataSet(sqlSelectUserNameSessionUser);
                String UserNameSessionUser = emailData3.Tables[0].Rows[0]["UserName"].ToString();

                //Stored Procedure #35 Where we select the emaiSubject, EmailBody,TImestamp, and ReceiverId, From Emails Where SenderId = sederId

//                CREATE PROCEDURE[dbo].[SelectEmailInformationFromSenderId]
//        @SenderId int
//        AS
//SELECT EmailSubject, EmailBody, "Timestamp", ReceiverId FROM Emails WHERE SenderId = @SenderId
//RETURN 0



                SqlCommand objCommand35 = new SqlCommand();

                objCommand35.CommandType = CommandType.StoredProcedure;
                objCommand35.CommandText = "SelectEmailInformationFromSenderId";

                SqlParameter SenderId35 = new SqlParameter("@SenderId", senderId);
                SenderId35.Direction = ParameterDirection.Input;
                objCommand35.Parameters.Add(SenderId35);

                DataSet emaildata21 = dBConnect.GetDataSetUsingCmdObj(objCommand35);

     //           String sqlSelectReceiverId = "SELECT EmailSubject, EmailBody, Timestamp, ReceiverId FROM Emails WHERE SenderId = " + senderId + " ";
     //           DataSet emaildata21 = dBConnect.GetDataSet(sqlSelectReceiverId);
                int receiverID = Int32.Parse(emaildata21.Tables[0].Rows[i]["ReceiverID"].ToString());

                //Keep row at one because SQL changes in the loop will not reach past first row

                //Stored Procedure #36 Select userName From users where UserId = specifc id
                //Same as above
                //                CREATE PROCEDURE[dbo].[LinkButtonSentUserNameFROMUsersWHEREUserIdIsSenderId]
                //        @SenderId int
                //        AS
                //SELECT UserName FROM Users WHERE UserId = @SenderId
                //RETURN 0

                SqlCommand objCommand36 = new SqlCommand();

                objCommand36.CommandType = CommandType.StoredProcedure;
                objCommand36.CommandText = "LinkButtonSentUserNameFROMUsersWHEREUserIdIsSenderId";

                SqlParameter ReceieverId36 = new SqlParameter("@SenderId", receiverID);
                ReceieverId36.Direction = ParameterDirection.Input;
                objCommand36.Parameters.Add(ReceieverId36);

                DataSet emailData2 = dBConnect.GetDataSetUsingCmdObj(objCommand36);


        //        String sqlSelectUserNameRecieverId = "SELECT UserName FROM Users WHERE UserId = " + receiverID + " ";
        //        DataSet emailData2 = dBConnect.GetDataSet(sqlSelectUserNameRecieverId);
                String receiverUserName = emailData2.Tables[0].Rows[0]["UserName"].ToString();


                String emailsubject = emailData.Tables[0].Rows[i]["EmailSubject"].ToString();
                String emailBody = emailData.Tables[0].Rows[i]["EmailBody"].ToString();
                String CreatedTime = emailData.Tables[0].Rows[i]["Timestamp"].ToString();


                Email email = new Email();
                email.SenderName = UserNameSessionUser;
                email.ReceiverName = Session["UserName"].ToString();
                email.Subject = emailsubject;
                email.EmailBody = emailBody;
                email.CreatedTime = CreatedTime;

                emailList.Add(email);
            }

            EmailMainView.DataSource = emailList;
            EmailMainView.DataBind();
           // refresh();

            // int size = emailData.Tables[0].Rows.Count;
            if (size == 0)
            {
                EmailMainView.Visible = false;
                LabelEmpty.Text = "You Have Not Flagged Any Emails.";
            }
            else
            {
                LabelEmpty.Text = "";
                EmailMainView.Visible = true;
            }
            ViewFlag();
        }


        public void refresh() {
            Response.Redirect("EmailClient.aspx");
        }

        protected void LinkButtonAllEmail_Click(object sender, EventArgs e)
        {
            int receiverId = Int32.Parse(Session["UserId"].ToString());

            //Stored Procedure #37 
            //Select all from EmailTables WhereandIdisEqualToYours

            //            CREATE PROCEDURE[dbo].[SelectAllEmailsThatYouHaveSentOrReceived]
            //        @ReveiverId int
            //    AS
            //SELECT* FROM Emails WHERE ReceiverId = @ReveiverId
            //OR SenderId = @ReveiverId
            //RETURN 0


            SqlCommand objCommand37 = new SqlCommand();

            objCommand37.CommandType = CommandType.StoredProcedure;
            objCommand37.CommandText = "SelectAllEmailsThatYouHaveSentOrReceived";

            SqlParameter RecieverId37 = new SqlParameter("@ReveiverId", receiverId);
            RecieverId37.Direction = ParameterDirection.Input;
            objCommand37.Parameters.Add(RecieverId37);


            DataSet emailData = dBConnect.GetDataSetUsingCmdObj(objCommand37);


     //       String sqlSelectFromEmailTable = "SELECT * FROM Emails WHERE ReceiverId = '" + receiverId + "'" +
    //            "OR SenderId = '" + receiverId + "'";
    //        DataSet emailData = dBConnect.GetDataSet(sqlSelectFromEmailTable);

            ArrayList emailList = new ArrayList();
            int size = emailData.Tables[0].Rows.Count;
            for (int i = 0; i < size; i++)
            {
                //Stored Procedure #38 Select Email information from correspodning sender or receiever

                //                CREATE PROCEDURE[dbo].[SelectAllEmailInformationFromSenderOrReceiver]
                //        @ReceiverId int
                //    AS
                //SELECT EmailSubject, EmailBody, "Timestamp", ReceiverId, SenderId
                //FROM Emails
                //WHERE ReceiverId = @ReceiverId
                //OR SenderId = @ReceiverId
                //RETURN 0

                SqlCommand objCommand38 = new SqlCommand();

                objCommand38.CommandType = CommandType.StoredProcedure;
                objCommand38.CommandText = "SelectAllEmailsThatYouHaveSentOrReceived";

                SqlParameter RecieverId38 = new SqlParameter("@ReveiverId", receiverId);
                RecieverId38.Direction = ParameterDirection.Input;
                objCommand38.Parameters.Add(RecieverId38);

                DataSet emaildata21 = dBConnect.GetDataSetUsingCmdObj(objCommand38);


       //         String sqlSelectReceiverId = "SELECT EmailSubject, EmailBody, Timestamp, ReceiverId, SenderId FROM Emails WHERE ReceiverId = " + receiverId + " OR SenderId = " + receiverId + "";
      //          DataSet emaildata21 = dBConnect.GetDataSet(sqlSelectReceiverId);
                int receiverID = Int32.Parse(emaildata21.Tables[0].Rows[i]["ReceiverId"].ToString());
                int senderID = Int32.Parse(emaildata21.Tables[0].Rows[i]["SenderId"].ToString());

                //Stored Procedure #39 Select UserName From Users Where UserId equal receiverId
                //similar to junk link

                //                CREATE PROCEDURE[dbo].[LinkButtonSentUserNameFROMUsersWHEREUserIdIsSenderId]
                //        @SenderId int
                //        AS
                //SELECT UserName FROM Users WHERE UserId = @SenderId
                //RETURN 0

                SqlCommand objCommand39 = new SqlCommand();

                objCommand39.CommandType = CommandType.StoredProcedure;
                objCommand39.CommandText = "LinkButtonSentUserNameFROMUsersWHEREUserIdIsSenderId";

                SqlParameter ReceieverId39 = new SqlParameter("@SenderId", receiverID);
                ReceieverId39.Direction = ParameterDirection.Input;
                objCommand39.Parameters.Add(ReceieverId39);

                DataSet emailData2 = dBConnect.GetDataSetUsingCmdObj(objCommand39);


           //     String sqlSelectUserNameReceiverId = "SELECT UserName FROM Users WHERE UserId = " + receiverID + " ";
           //     DataSet emailData2 = dBConnect.GetDataSet(sqlSelectUserNameReceiverId);
                String receiverUsername = emailData2.Tables[0].Rows[0]["UserName"].ToString();

                //Stored Procedure #40 Select UserName From Users Where UserId equal receiverId
                //similar to junk link

                //                CREATE PROCEDURE[dbo].[LinkButtonSentUserNameFROMUsersWHEREUserIdIsSenderId]
                //        @SenderId int
                //        AS
                //SELECT UserName FROM Users WHERE UserId = @SenderId
                //RETURN 0

                SqlCommand objCommand40 = new SqlCommand();

                objCommand40.CommandType = CommandType.StoredProcedure;
                objCommand40.CommandText = "LinkButtonSentUserNameFROMUsersWHEREUserIdIsSenderId";

                SqlParameter SenderId40 = new SqlParameter("@SenderId", senderID);
                SenderId40.Direction = ParameterDirection.Input;
                objCommand40.Parameters.Add(SenderId40);

                DataSet emailData3 = dBConnect.GetDataSetUsingCmdObj(objCommand40);


           //     String sqlSelectUserNameSessionUser = "SELECT UserName FROM Users WHERE UserId = " + senderID + " ";
           //     DataSet emailData3 = dBConnect.GetDataSet(sqlSelectUserNameSessionUser);
                String username = emailData3.Tables[0].Rows[0]["UserName"].ToString();

                String subject = emailData.Tables[0].Rows[i]["EmailSubject"].ToString();
                String body = emailData.Tables[0].Rows[i]["EmailBody"].ToString();
                String timeCreated = emailData.Tables[0].Rows[i]["Timestamp"].ToString();


                Email email = new Email();
                email.SenderName = username;
                email.ReceiverName = receiverUsername;
                email.Subject = subject;
                email.EmailBody = body;
                email.CreatedTime = timeCreated;

                emailList.Add(email);
            }

            EmailMainView.DataSource = emailList;
            EmailMainView.DataBind();

            if (size == 0)
            {
                EmailMainView.Visible = false;
                LabelEmpty.Text = "You have no emails.";
            }
            else
            {
                LabelEmpty.Text = "";
                EmailMainView.Visible = true;
            }
            AllEmailsView();
        }

        public void AllEmailsView()
        {
            ViewInbox();
            ViewFlag();
            ViewJunk();
            ViewSent();
            ViewTrash();
        }

        protected void LinkButtonJunk_Click1(object sender, EventArgs e)
        {
                int receiverId = Int32.Parse(Session["UserId"].ToString());
                Session["Tag"] = "Junk";
                String junk = Session["Tag"].ToString();

            //Stored Procedure #41 Similar to Trash

            //            CREATE PROCEDURE[dbo].[SelectTagIdFromTagsForJunk]
            //        @ReceiverId int,
            //	@TagName varchar(50)
            //AS
            //SELECT TagId FROM Tags WHERE Tags.UserId = @ReceiverId AND Tags.TagName = @TagName
            //RETURN 0

            SqlCommand objCommand41 = new SqlCommand();

            objCommand41.CommandType = CommandType.StoredProcedure;
            objCommand41.CommandText = "SelectTagIdFromTagsForJunk";

            SqlParameter RecieverId41 = new SqlParameter("@ReceiverId", receiverId);
            RecieverId41.Direction = ParameterDirection.Input;
            objCommand41.Parameters.Add(RecieverId41);

            SqlParameter TagName41 = new SqlParameter("@TagName", junk);
            TagName41.Direction = ParameterDirection.Input;
            objCommand41.Parameters.Add(TagName41);

            DataSet dsTagId = dBConnect.GetDataSetUsingCmdObj(objCommand41);


         //   String sqlTagId = "SELECT TagId FROM Tags WHERE Tags.UserId = " + receiverId + " AND Tags.TagName = '" + junk + "'";
         //       DataSet dsTagId = dBConnect.GetDataSet(sqlTagId);
                int tagId = Int32.Parse(dsTagId.Tables[0].Rows[0]["TagId"].ToString());

            //StoredProcedure #42 Select all emails where Receiever Id and TagId are matched

            //            CREATE PROCEDURE[dbo].[SelectAllEmailsForJunk]
            //        @ReceiverId int,
            //	@TagId int
            //AS
            //SELECT* FROM Emails, EmailRecipient
            //WHERE Emails.EmailId = EmailRecipient.EmailId
            //AND Emails.ReceiverId = @ReceiverId
            //AND EmailRecipient.EmailTag = @TagId
            //RETURN 0

            SqlCommand objCommand42 = new SqlCommand();

            objCommand42.CommandType = CommandType.StoredProcedure;
            objCommand42.CommandText = "SelectAllEmailsForJunk";

            SqlParameter RecieverId42 = new SqlParameter("@ReceiverId", receiverId);
            RecieverId42.Direction = ParameterDirection.Input;
            objCommand42.Parameters.Add(RecieverId42);

            SqlParameter TagName42 = new SqlParameter("@TagId", tagId);
            TagName42.Direction = ParameterDirection.Input;
            objCommand42.Parameters.Add(TagName42);

            DataSet emailData = dBConnect.GetDataSetUsingCmdObj(objCommand42);

            //String sqlSelectFromEmailTable = "SELECT * FROM Emails, EmailRecipient " +
            //        "WHERE Emails.EmailId = EmailRecipient.EmailId " +
            //        "AND Emails.Receiverid = '" + receiverId + "' " +
            //        "AND EmailRecipient.EmailTag = '" + tagId + "'";

         //       DataSet emailData = dBConnect.GetDataSet(sqlSelectFromEmailTable);

                ArrayList emailList = new ArrayList();
                int size = emailData.Tables[0].Rows.Count;
                for (int i = 0; i < size; i++)
                {
                    int senderId = Int32.Parse(emailData.Tables[0].Rows[0]["SenderId"].ToString());
                //Keep row at one because SQL changes in the loop will not reach past first row
                //Stored Procedure #43 Select UserName From Users Where UserId
                //Same Procedure

                //                CREATE PROCEDURE[dbo].[LinkBtnSentUserNameFromUsersWithReceieverId]
                //        @ReceiverId int
                //    AS
                // SELECT UserName FROM Users WHERE UserId = @ReceiverId
                //RETURN 0

                SqlCommand objCommand43 = new SqlCommand();

                objCommand43.CommandType = CommandType.StoredProcedure;
                objCommand43.CommandText = "LinkBtnSentUserNameFromUsersWithReceieverId";

                SqlParameter RecieverId43 = new SqlParameter("@ReceiverId", senderId);
                RecieverId43.Direction = ParameterDirection.Input;
                objCommand43.Parameters.Add(RecieverId43);

                DataSet emailData3 = dBConnect.GetDataSetUsingCmdObj(objCommand43);

        //        String sqlSelectUserNameSessionUser = "SELECT UserName FROM Users WHERE UserId = " + senderId + " ";
        //            DataSet emailData3 = dBConnect.GetDataSet(sqlSelectUserNameSessionUser);
                    String UserNameSessionUser = emailData3.Tables[0].Rows[0]["UserName"].ToString();

                //Stored Procedure #44 Select EmailInfromation by senderid
                //Similar Procedure

                //                CREATE PROCEDURE[dbo].[LinkSentFindEmailInformationOfReciever]
                //        @SenderId int

                //    AS
                // SELECT EmailSubject, EmailBody, "Timestamp", ReceiverId FROM Emails WHERE SenderId = @SenderId
                //RETURN 0


                SqlCommand objCommand44 = new SqlCommand();

                objCommand44.CommandType = CommandType.StoredProcedure;
                objCommand44.CommandText = "LinkSentFindEmailInformationOfReciever";

                SqlParameter SenderId44 = new SqlParameter("@SenderId", senderId);
                SenderId44.Direction = ParameterDirection.Input;
                objCommand44.Parameters.Add(SenderId44);

                DataSet emaildata21 = dBConnect.GetDataSetUsingCmdObj(objCommand44);


             //   String sqlSelectReceiverId = "SELECT EmailSubject, EmailBody, Timestamp, ReceiverId FROM Emails WHERE SenderId = " + senderId + " ";
             //       DataSet emaildata21 = dBConnect.GetDataSet(sqlSelectReceiverId);
                    int receiverID = Int32.Parse(emaildata21.Tables[0].Rows[i]["ReceiverID"].ToString());


                //Stored Procedure #45 Select UserName From Users Where UserId
                //Same Procedure

                //                CREATE PROCEDURE[dbo].[LinkBtnSentUserNameFromUsersWithReceieverId]
                //        @ReceiverId int
                //    AS
                // SELECT UserName FROM Users WHERE UserId = @ReceiverId
                //RETURN 0

                SqlCommand objCommand45 = new SqlCommand();

                objCommand45.CommandType = CommandType.StoredProcedure;
                objCommand45.CommandText = "LinkBtnSentUserNameFromUsersWithReceieverId";

                SqlParameter RecieverId45 = new SqlParameter("@ReceiverId", receiverID);
                RecieverId45.Direction = ParameterDirection.Input;
                objCommand45.Parameters.Add(RecieverId45);

                DataSet emailData2 = dBConnect.GetDataSetUsingCmdObj(objCommand45);


            //    String sqlSelectUserNameRecieverId = "SELECT UserName FROM Users WHERE UserId = " + receiverID + " ";
            //        DataSet emailData2 = dBConnect.GetDataSet(sqlSelectUserNameRecieverId);
                    String receiverUserName = emailData2.Tables[0].Rows[0]["UserName"].ToString();


                    String emailsubject = emailData.Tables[0].Rows[i]["EmailSubject"].ToString();
                    String emailBody = emailData.Tables[0].Rows[i]["EmailBody"].ToString();
                    String CreatedTime = emailData.Tables[0].Rows[i]["Timestamp"].ToString();


                    Email email = new Email();
                    email.SenderName = UserNameSessionUser;
                    email.ReceiverName = Session["UserName"].ToString();
                    email.Subject = emailsubject;
                    email.EmailBody = emailBody;
                    email.CreatedTime = CreatedTime;

                    emailList.Add(email);
                }

                EmailMainView.DataSource = emailList;
                EmailMainView.DataBind();

                // int size = emailData.Tables[0].Rows.Count;
                if (size == 0)
                {
                    EmailMainView.Visible = false;
                    LabelEmpty.Text = "You Have No Junk Emails.";
                }
                else
                {
                    LabelEmpty.Text = "";
                    EmailMainView.Visible = true;
                }
                ViewJunk();
            
        }

        protected void btnFlag_Click(object sender, EventArgs e)
        {
            //go through gridview
            //see which are checked
            //depedning that was on checek get Email id 
            for (int row = 0; row < EmailMainView.Rows.Count; row++)
            {
                CheckBox cb;
                cb = (CheckBox)EmailMainView.Rows[row].FindControl("chkSelectEmail");
                if (cb.Checked)
                {
                    String userId = Session["UserId"].ToString();
                    String SenderName = EmailMainView.Rows[row].Cells[2].Text;
                    String ReceiverName = EmailMainView.Rows[row].Cells[3].Text;
                    String EmailSubject = EmailMainView.Rows[row].Cells[4].Text;
                    String EmailBody = EmailMainView.Rows[row].Cells[5].Text;
                    String TimeStamp = EmailMainView.Rows[row].Cells[6].Text;

                    //  string[] info = { SendId, ReceiveID, EmailSubject, EmailBody, TimeStamp };


                    //Find the User with their specific user name
                    //Stored Procedure #47 Select the Userid From the UserName
                    //                    CREATE PROCEDURE[dbo].[FindUserIdByUserName]
                    //        @Sendername varchar(max)
                    //AS
                    //SELECT UserId FROM Users WHERE UserName = @Sendername
                    //RETURN 0

                    SqlCommand objCommand47 = new SqlCommand();

                    objCommand47.CommandType = CommandType.StoredProcedure;
                    objCommand47.CommandText = "FindUserIdByUserName";

                    SqlParameter SenderName47 = new SqlParameter("@Sendername", SenderName);
                    SenderName47.Direction = ParameterDirection.Input;
                    objCommand47.Parameters.Add(SenderName47);

                    DataSet emailData2 = dBConnect.GetDataSetUsingCmdObj(objCommand47);


                    //String sqlFindUserWithSenderNameandfindtheirassociatediD = "SELECT UserId FROM Users WHERE UserName = " +
                    //    "'" + SenderName + "'";
                    //DataSet emailData2 = dBConnect.GetDataSet(sqlFindUserWithSenderNameandfindtheirassociatediD);
                    int actualSender = Int32.Parse(emailData2.Tables[0].Rows[0]["UserId"].ToString());

                    //Stored Procedure #48 Select EmailId From Emails with user Information

                    //                    CREATE PROCEDURE[dbo].[FindEmailIdFromSenderInformation]
                    //        @ActualSender int,
                    //	@UserId int,
                    //	@EmailSubject varchar(max),
                    //	@EmailBody varchar(max)
                    //AS
                    //SELECT EmailId FROM Emails
                    //                       WHERE SenderId = @ActualSender
                    //                        AND ReceiverId = @UserId
                    //                        AND EmailSubject = @EmailSubject
                    //                        AND EmailBody = @EmailBody
                    //                        RETURN 0
                    SqlCommand objCommand48 = new SqlCommand();

                    objCommand48.CommandType = CommandType.StoredProcedure;
                    objCommand48.CommandText = "FindEmailIdFromSenderInformation";

                    SqlParameter SenderId48 = new SqlParameter("@ActualSender", actualSender);
                    SenderId48.Direction = ParameterDirection.Input;
                    objCommand48.Parameters.Add(SenderId48);

                    SqlParameter UserId48 = new SqlParameter("@UserId", userId);
                    UserId48.Direction = ParameterDirection.Input;
                    objCommand48.Parameters.Add(UserId48);

                    SqlParameter EmailSubject48 = new SqlParameter("@EmailSubject", EmailSubject);
                    EmailSubject48.Direction = ParameterDirection.Input;
                    objCommand48.Parameters.Add(EmailSubject48);

                    SqlParameter EmailBody48 = new SqlParameter("@EmailBody", EmailBody);
                    EmailBody48.Direction = ParameterDirection.Input;
                    objCommand48.Parameters.Add(EmailBody48);

                    DataSet emailData = dBConnect.GetDataSetUsingCmdObj(objCommand48);


                    //String sqlSelectFromEmailTable = "SELECT EmailId FROM Emails " +
                    //    "WHERE SenderId = '" + actualSender + "' " +
                    //    "AND ReceiverId = '" + userId + "' " +
                    //    "AND EmailSubject = '" + EmailSubject + "' " +
                    //    "AND EmailBody = '" + EmailBody + "' ";
                    //  //  "AND Timestamp = '" + TimeStamp + "'";

                 //   DataSet emailData = dBConnect.GetDataSet(sqlSelectFromEmailTable);
                    int emailId = Int32.Parse(emailData.Tables[0].Rows[0]["EmailId"].ToString());
                    //NOW INSERT
                    //NOw INSERT

                    //Stored Procedure 49 Update EmailRecipient
                    //

                    //                    CREATE PROCEDURE[dbo].[UpdateFlagNumberInEmailRecip]
                    //        @FlagNum int,
                    //	@EmailId int,
                    //	@UserId int
                    //AS
                    //Update EmailRecipient SET Flag = @FlagNum WHERE EmailId = @EmailId
                    //AND UserId = @UserId
                    //RETURN 0
                    int flagNum = 1;
                    SqlCommand objCommand49 = new SqlCommand();

                    objCommand49.CommandType = CommandType.StoredProcedure;
                    objCommand49.CommandText = "UpdateFlagNumberInEmailRecip";

                    SqlParameter FlagNum49 = new SqlParameter("@FlagNum", flagNum);
                    FlagNum49.Direction = ParameterDirection.Input;
                    objCommand49.Parameters.Add(FlagNum49);

                    SqlParameter EmailId49 = new SqlParameter("@EmailId", emailId);
                    EmailId49.Direction = ParameterDirection.Input;
                    objCommand49.Parameters.Add(EmailId49);

                    SqlParameter UserId49 = new SqlParameter("@UserId", userId);
                    UserId49.Direction = ParameterDirection.Input;
                    objCommand49.Parameters.Add(UserId49);

                    int ret = dBConnect.DoUpdateUsingCmdObj(objCommand49);


                    //String sqlInsertTag = "Update EmailRecipient SET Flag = 1 WHERE EmailId = '" + emailId +
                    //    "'AND UserId = '" + userId + "'";
                    dBConnect = new DBConnect();
                  //  DataSet FlagData = dBConnect.GetDataSet(sqlInsertTag);
                    reloadData();

                }
                else
                {
                    // Response.Write("<script>alert('Checkbox is not checked.')</script>");
                }
            }
        }

        protected void btnUnFlag_Click(object sender, EventArgs e)
        {
            //go through gridview
            //see which are checked
            //depedning that was on checek get Email id 
            for (int row = 0; row < EmailMainView.Rows.Count; row++)
            {
                CheckBox cb;
                cb = (CheckBox)EmailMainView.Rows[row].FindControl("chkSelectEmail");
                if (cb.Checked)
                {
                    String userId = Session["UserId"].ToString();
                    String SenderName = EmailMainView.Rows[row].Cells[2].Text;
                    String ReceiverName = EmailMainView.Rows[row].Cells[3].Text;
                    String EmailSubject = EmailMainView.Rows[row].Cells[4].Text;
                    String EmailBody = EmailMainView.Rows[row].Cells[5].Text;
                    String TimeStamp = EmailMainView.Rows[row].Cells[6].Text;

                    //  string[] info = { SendId, ReceiveID, EmailSubject, EmailBody, TimeStamp };
                    //Stored Procedure #50
                    //Find the User with their specific user name
                    //Stored Procedure #47 Select the Userid From the UserName
                    //                    CREATE PROCEDURE[dbo].[FindUserIdByUserName]
                    //        @Sendername varchar(max)
                    //AS
                    //SELECT UserId FROM Users WHERE UserName = @Sendername
                    //RETURN 0

                    SqlCommand objCommand50 = new SqlCommand();

                    objCommand50.CommandType = CommandType.StoredProcedure;
                    objCommand50.CommandText = "FindUserIdByUserName";

                    SqlParameter SenderName50 = new SqlParameter("@Sendername", SenderName);
                    SenderName50.Direction = ParameterDirection.Input;
                    objCommand50.Parameters.Add(SenderName50);

                    DataSet emailData2 = dBConnect.GetDataSetUsingCmdObj(objCommand50);


                    //Find the User with their specific user name
                    //String sqlFindUserWithSenderNameandfindtheirassociatediD = "SELECT UserId FROM Users WHERE UserName = " +
                    //    "'" + SenderName + "'";
                    //DataSet emailData2 = dBConnect.GetDataSet(sqlFindUserWithSenderNameandfindtheirassociatediD);
                    int actualSender = Int32.Parse(emailData2.Tables[0].Rows[0]["UserId"].ToString());


                    //Stored Procedure #51

                    //Stored Procedure #48 Select EmailId From Emails with user Information

                    //                    CREATE PROCEDURE[dbo].[FindEmailIdFromSenderInformation]
                    //        @ActualSender int,
                    //	@UserId int,
                    //	@EmailSubject varchar(max),
                    //	@EmailBody varchar(max)
                    //AS
                    //SELECT EmailId FROM Emails
                    //                       WHERE SenderId = @ActualSender
                    //                        AND ReceiverId = @UserId
                    //                        AND EmailSubject = @EmailSubject
                    //                        AND EmailBody = @EmailBody
                    //                        RETURN 0
                    SqlCommand objCommand51 = new SqlCommand();

                    objCommand51.CommandType = CommandType.StoredProcedure;
                    objCommand51.CommandText = "FindEmailIdFromSenderInformation";

                    SqlParameter SenderId51 = new SqlParameter("@ActualSender", actualSender);
                    SenderId51.Direction = ParameterDirection.Input;
                    objCommand51.Parameters.Add(SenderId51);

                    SqlParameter UserId51 = new SqlParameter("@UserId", userId);
                    UserId51.Direction = ParameterDirection.Input;
                    objCommand51.Parameters.Add(UserId51);

                    SqlParameter EmailSubject51 = new SqlParameter("@EmailSubject", EmailSubject);
                    EmailSubject51.Direction = ParameterDirection.Input;
                    objCommand51.Parameters.Add(EmailSubject51);

                    SqlParameter EmailBody51 = new SqlParameter("@EmailBody", EmailBody);
                    EmailBody51.Direction = ParameterDirection.Input;
                    objCommand51.Parameters.Add(EmailBody51);

                    DataSet emailData = dBConnect.GetDataSetUsingCmdObj(objCommand51);

                    //String sqlSelectFromEmailTable = "SELECT EmailId FROM Emails " +
                    //    "WHERE SenderId = '" + actualSender + "' " +
                    //    "AND ReceiverId = '" + userId + "' " +
                    //    "AND EmailSubject = '" + EmailSubject + "' " +
                    //    "AND EmailBody = '" + EmailBody + "' ";
                    ////  "AND Timestamp = '" + TimeStamp + "'";

                  //  DataSet emailData = dBConnect.GetDataSet(sqlSelectFromEmailTable);
                    int emailId = Int32.Parse(emailData.Tables[0].Rows[0]["EmailId"].ToString());
                    //NOW INSERT
                    //NOw INSERT

                    //Stored Procedure 52 Update EmailRecipient
                    //

                    //                    CREATE PROCEDURE[dbo].[UpdateFlagNumberInEmailRecip]
                    //        @FlagNum int,
                    //	@EmailId int,
                    //	@UserId int
                    //AS
                    //Update EmailRecipient SET Flag = @FlagNum WHERE EmailId = @EmailId
                    //AND UserId = @UserId
                    //RETURN 0
                    int flagNum = 0;
                    SqlCommand objCommand52 = new SqlCommand();

                    objCommand52.CommandType = CommandType.StoredProcedure;
                    objCommand52.CommandText = "UpdateFlagNumberInEmailRecip";

                    SqlParameter FlagNum52 = new SqlParameter("@FlagNum", flagNum);
                    FlagNum52.Direction = ParameterDirection.Input;
                    objCommand52.Parameters.Add(FlagNum52);

                    SqlParameter EmailId52 = new SqlParameter("@EmailId", emailId);
                    EmailId52.Direction = ParameterDirection.Input;
                    objCommand52.Parameters.Add(EmailId52);

                    SqlParameter UserId52 = new SqlParameter("@UserId", userId);
                    UserId52.Direction = ParameterDirection.Input;
                    objCommand52.Parameters.Add(UserId52);

                    int ret = dBConnect.DoUpdateUsingCmdObj(objCommand52);


                    //String sqlInsertTag = "Update EmailRecipient SET Flag = 0 WHERE EmailId = '" + emailId +
                    //    "'AND UserId = '" + userId + "'";
                    //dBConnect = new DBConnect();
                 //   DataSet FlagData = dBConnect.GetDataSet(sqlInsertTag);
                    reloadData();

                }
                else
                {
                    // Response.Write("<script>alert('Checkbox is not checked.')</script>");
                }
            }
        }

        public void reloadData()
        {
            EmailMainView.DataBind();
            OpenInboxEmailClient();

        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            buttonGroup(false);
            int warning = 0;
            for (int row = 0; row < EmailMainView.Rows.Count; row++)
            {
                CheckBox cb;
                cb = (CheckBox)EmailMainView.Rows[row].FindControl("chkSelectEmail");
                if (cb.Checked)
                {
                    warning++;
                    String userId = Session["UserId"].ToString();
                    String senderName = EmailMainView.Rows[row].Cells[2].Text;
                    String receiverName = EmailMainView.Rows[row].Cells[3].Text;
                    String subject = EmailMainView.Rows[row].Cells[4].Text;
                    String body = EmailMainView.Rows[row].Cells[5].Text;
                    String timeCreated = EmailMainView.Rows[row].Cells[6].Text;

                    //Find the User with their specific user name



                    //Find the User with their specific user name
                    //Stored Procedure #52 Select the Userid From the UserName
                    //                    CREATE PROCEDURE[dbo].[FindUserIdByUserName]
                    //        @Sendername varchar(max)
                    //AS
                    //SELECT UserId FROM Users WHERE UserName = @Sendername
                    //RETURN 0

                    SqlCommand objCommand52 = new SqlCommand();

                    objCommand52.CommandType = CommandType.StoredProcedure;
                    objCommand52.CommandText = "FindUserIdByUserName";

                    SqlParameter SenderName52 = new SqlParameter("@Sendername", senderName);
                    SenderName52.Direction = ParameterDirection.Input;
                    objCommand52.Parameters.Add(SenderName52);

                    DataSet emailData2 = dBConnect.GetDataSetUsingCmdObj(objCommand52);

                    //String sqlFindUserWithSenderNameandfindtheirassociatediD = "SELECT UserId FROM Users WHERE UserName = " +
                    //    "'" + senderName + "'";
                    //DataSet emailData2 = dBConnect.GetDataSet(sqlFindUserWithSenderNameandfindtheirassociatediD);
                    int actualSender = Int32.Parse(emailData2.Tables[0].Rows[0]["UserId"].ToString());

                    //Stored Procedure #53 Select EmailId From Emails with user Information

                    //                    CREATE PROCEDURE[dbo].[FindEmailIdFromSenderInformation]
                    //        @ActualSender int,
                    //	@UserId int,
                    //	@EmailSubject varchar(max),
                    //	@EmailBody varchar(max)
                    //AS
                    //SELECT EmailId FROM Emails
                    //                       WHERE SenderId = @ActualSender
                    //                        AND ReceiverId = @UserId
                    //                        AND EmailSubject = @EmailSubject
                    //                        AND EmailBody = @EmailBody
                    //                        RETURN 0

                    SqlCommand objCommand53 = new SqlCommand();

                    objCommand53.CommandType = CommandType.StoredProcedure;
                    objCommand53.CommandText = "FindEmailIdFromSenderInformation";

                    SqlParameter SenderId53 = new SqlParameter("@ActualSender", actualSender);
                    SenderId53.Direction = ParameterDirection.Input;
                    objCommand53.Parameters.Add(SenderId53);

                    SqlParameter UserId53 = new SqlParameter("@UserId", userId);
                    UserId53.Direction = ParameterDirection.Input;
                    objCommand53.Parameters.Add(UserId53);

                    SqlParameter EmailSubject53 = new SqlParameter("@EmailSubject", subject);
                    EmailSubject53.Direction = ParameterDirection.Input;
                    objCommand53.Parameters.Add(EmailSubject53);

                    SqlParameter EmailBody53 = new SqlParameter("@EmailBody", body);
                    EmailBody53.Direction = ParameterDirection.Input;
                    objCommand53.Parameters.Add(EmailBody53);

                    DataSet dsEmailId = dBConnect.GetDataSetUsingCmdObj(objCommand53);


                    //String sqlFindEmailID = "SELECT EmailId FROM Emails " +
                    //    "WHERE SenderId = '" + actualSender + "' " +
                    //    "AND ReceiverId = '" + userId + "' " +
                    //    "AND EmailSubject = '" + subject + "' " +
                    //    "AND EmailBody = '" + body + "' ";
                    //    //"AND Timestamp = '" + timeCreated + "'";

                    //DataSet dsEmailId = dBConnect.GetDataSet(sqlFindEmailID);

                    int sqlEmailId = Int32.Parse(dsEmailId.Tables[0].Rows[0]["EmailId"].ToString());
                    String trashname = "Trash";

                    //Stored Procedure #54 Select TagId From Tags Where Tags.Userd and Tag.TagName match a user

                    //                    CREATE PROCEDURE[dbo].[SelectTagIdFromUserIdAndTagName]
                    //        @UserId int,
                    //	@TagName varchar(50)
                    //AS
                    //SELECT TagId FROM Tags WHERE Tags.UserId = @UserId
                    //AND Tags.TagName = @TagName
                    //RETURN 0

                    SqlCommand objCommand54 = new SqlCommand();

                    objCommand54.CommandType = CommandType.StoredProcedure;
                    objCommand54.CommandText = "SelectTagIdFromUserIdAndTagName";

                    SqlParameter SenderId54 = new SqlParameter("@UserId", userId);
                    SenderId54.Direction = ParameterDirection.Input;
                    objCommand54.Parameters.Add(SenderId54);

                    SqlParameter TagName54 = new SqlParameter("@TagName", trashname);
                    TagName54.Direction = ParameterDirection.Input;
                    objCommand54.Parameters.Add(TagName54);

                    DataSet dsTagId = dBConnect.GetDataSetUsingCmdObj(objCommand54);


                    //String sqlGetTagId = "SELECT TagId FROM Tags WHERE Tags.UserId = " + userId +
                    //    " AND Tags.TagName = '" + trashname + "'";
                    //DataSet dsTagId = dBConnect.GetDataSet(sqlGetTagId);

                    int tagId = Int32.Parse(dsTagId.Tables[0].Rows[0]["TagId"].ToString());

                    //Stored Procedure #55 Update EmailRecipitent by setting the EmailTag
                    //                    CREATE PROCEDURE[dbo].[UpdateEmailRecipBySettingEmailTag]
                    //        @TagId int,
                    //	@UserId int,
                    //	@SqlEmailId int
                    //AS
                    //UPDATE EmailRecipient SET EmailTag = @TagId
                    //WHERE UserId = @UserId
                    //AND EmailId = @SqlEmailId
                    //RETURN 0

                    SqlCommand objCommand55 = new SqlCommand();

                    objCommand55.CommandType = CommandType.StoredProcedure;
                    objCommand55.CommandText = "UpdateEmailRecipBySettingEmailTag";

                    SqlParameter TagId55 = new SqlParameter("@TagId", tagId);
                    TagId55.Direction = ParameterDirection.Input;
                    objCommand55.Parameters.Add(TagId55);

                    SqlParameter UserId55 = new SqlParameter("@UserId", userId);
                    UserId55.Direction = ParameterDirection.Input;
                    objCommand55.Parameters.Add(UserId55);

                    SqlParameter SqlEmailId55 = new SqlParameter("@SqlEmailId", sqlEmailId);
                    SqlEmailId55.Direction = ParameterDirection.Input;
                    objCommand55.Parameters.Add(SqlEmailId55);

                    int emailUpdateSuccess = dBConnect.DoUpdateUsingCmdObj(objCommand55);


                    //String sqlUpdateEmailTag = "Update EmailRecipient SET EmailTag = '" + tagId + "'" +
                    //    "WHERE UserId = '" + userId + "' AND EmailId = '" + sqlEmailId + "'";
                    //dBConnect = new DBConnect();
                    //int emailUpdateSuccess = dBConnect.DoUpdate(sqlUpdateEmailTag);
                    reloadData();

                }
            }
            if (warning == 0)
            {
                Response.Write("<script>alert('There are no checkboxes selected')</script>");
            }
        }

        protected void btnMoveToFolder_Click(object sender, EventArgs e)
        {
            String ddlValue = moveToDropDown.SelectedValue.ToString();

            buttonGroup(false);
            int count = 0;
            for (int row = 0; row < EmailMainView.Rows.Count; row++)
            {
                CheckBox cb;
                cb = (CheckBox)EmailMainView.Rows[row].FindControl("chkSelectEmail");
                if (cb.Checked)
                {
                    count++;
                    String userId = Session["UserId"].ToString();
                    String senderName = EmailMainView.Rows[row].Cells[2].Text;
                    String receiverName = EmailMainView.Rows[row].Cells[3].Text;
                    String subject = EmailMainView.Rows[row].Cells[4].Text;
                    String body = EmailMainView.Rows[row].Cells[5].Text;
                    String timeCreated = EmailMainView.Rows[row].Cells[6].Text;



                    //Find the User with their specific user name

                    //Stored Procedure #56 FindUserId FROM Users from userName
                    //Same procedure
                    //Find the User with their specific user name
                    //                    CREATE PROCEDURE[dbo].[FindUserIdByUserName]
                    //        @Sendername varchar(max)
                    //AS
                    //SELECT UserId FROM Users WHERE UserName = @Sendername
                    //RETURN 0

                    SqlCommand objCommand56 = new SqlCommand();

                    objCommand56.CommandType = CommandType.StoredProcedure;
                    objCommand56.CommandText = "FindUserIdByUserName";

                    SqlParameter SenderName56 = new SqlParameter("@Sendername", senderName);
                    SenderName56.Direction = ParameterDirection.Input;
                    objCommand56.Parameters.Add(SenderName56);

                    DataSet emailData2 = dBConnect.GetDataSetUsingCmdObj(objCommand56);



                    //String sqlFindUserWithSenderNameandfindtheirassociatediD = "SELECT UserId FROM Users WHERE UserName = " +
                    //    "'" + senderName + "'";
                    //DataSet emailData2 = dBConnect.GetDataSet(sqlFindUserWithSenderNameandfindtheirassociatediD);
                    int actualSender = Int32.Parse(emailData2.Tables[0].Rows[0]["UserId"].ToString());

                    //Stored Procedure #57 Same as Delete Button

                    //Stored Procedure #53 Select EmailId From Emails with user Information

                    //                    CREATE PROCEDURE[dbo].[FindEmailIdFromSenderInformation]
                    //        @ActualSender int,
                    //	@UserId int,
                    //	@EmailSubject varchar(max),
                    //	@EmailBody varchar(max)
                    //AS
                    //SELECT EmailId FROM Emails
                    //                       WHERE SenderId = @ActualSender
                    //                        AND ReceiverId = @UserId
                    //                        AND EmailSubject = @EmailSubject
                    //                        AND EmailBody = @EmailBody
                    //                        RETURN 0

                    SqlCommand objCommand57 = new SqlCommand();

                    objCommand57.CommandType = CommandType.StoredProcedure;
                    objCommand57.CommandText = "FindEmailIdFromSenderInformation";

                    SqlParameter SenderId57 = new SqlParameter("@ActualSender", actualSender);
                    SenderId57.Direction = ParameterDirection.Input;
                    objCommand57.Parameters.Add(SenderId57);

                    SqlParameter UserId57 = new SqlParameter("@UserId", userId);
                    UserId57.Direction = ParameterDirection.Input;
                    objCommand57.Parameters.Add(UserId57);

                    SqlParameter EmailSubject57 = new SqlParameter("@EmailSubject", subject);
                    EmailSubject57.Direction = ParameterDirection.Input;
                    objCommand57.Parameters.Add(EmailSubject57);

                    SqlParameter EmailBody57 = new SqlParameter("@EmailBody", body);
                    EmailBody57.Direction = ParameterDirection.Input;
                    objCommand57.Parameters.Add(EmailBody57);

                    DataSet dsEmailId = dBConnect.GetDataSetUsingCmdObj(objCommand57);


                    //String sqlFindEmailID = "SELECT EmailId FROM Emails " +
                    //    "WHERE SenderId = '" + actualSender + "' " +
                    //    "AND ReceiverId = '" + userId + "' " +
                    //    "AND EmailSubject = '" + subject + "' " +
                    //    "AND EmailBody = '" + body + "' ";
                    //    //"AND Timestamp = '" + timeCreated + "'";

                    //DataSet dsEmailId = dBConnect.GetDataSet(sqlFindEmailID);
                    int sqlEmailId = Int32.Parse(dsEmailId.Tables[0].Rows[0]["EmailId"].ToString());

                    //Stored Procedure #58
                    //Same as delete button

                    //                    CREATE PROCEDURE[dbo].[SelectTagIdFromUserIdAndTagName]
                    //        @UserId int,
                    //	@TagName varchar(50)
                    //AS
                    //SELECT TagId FROM Tags WHERE Tags.UserId = @UserId
                    //AND Tags.TagName = @TagName
                    //RETURN 0

                    SqlCommand objCommand58 = new SqlCommand();

                    objCommand58.CommandType = CommandType.StoredProcedure;
                    objCommand58.CommandText = "SelectTagIdFromUserIdAndTagName";

                    SqlParameter SenderId58 = new SqlParameter("@UserId", userId);
                    SenderId58.Direction = ParameterDirection.Input;
                    objCommand58.Parameters.Add(SenderId58);

                    SqlParameter TagName58 = new SqlParameter("@TagName", ddlValue);
                    TagName58.Direction = ParameterDirection.Input;
                    objCommand58.Parameters.Add(TagName58);

                    DataSet dsTagId = dBConnect.GetDataSetUsingCmdObj(objCommand58);

                    //String sqlGetTagId = "SELECT TagId FROM Tags WHERE Tags.UserId = " + userId +
                    //    " AND Tags.TagName = '" + ddlValue + "'";
                    //DataSet dsTagId = dBConnect.GetDataSet(sqlGetTagId);
                    int tagId = Int32.Parse(dsTagId.Tables[0].Rows[0]["TagId"].ToString());

                    //Stored Procedure #59 Update EmailRecipient 
                    //Same as delete

                    //                    CREATE PROCEDURE[dbo].[UpdateEmailRecipBySettingEmailTag]
                    //        @TagId int,
                    //	@UserId int,
                    //	@SqlEmailId int
                    //AS
                    //UPDATE EmailRecipient SET EmailTag = @TagId
                    //WHERE UserId = @UserId
                    //AND EmailId = @SqlEmailId
                    //RETURN 0

                    SqlCommand objCommand59 = new SqlCommand();

                    objCommand59.CommandType = CommandType.StoredProcedure;
                    objCommand59.CommandText = "UpdateEmailRecipBySettingEmailTag";

                    SqlParameter TagId59 = new SqlParameter("@TagId", tagId);
                    TagId59.Direction = ParameterDirection.Input;
                    objCommand59.Parameters.Add(TagId59);

                    SqlParameter UserId59 = new SqlParameter("@UserId", userId);
                    UserId59.Direction = ParameterDirection.Input;
                    objCommand59.Parameters.Add(UserId59);

                    SqlParameter SqlEmailId59 = new SqlParameter("@SqlEmailId", sqlEmailId);
                    SqlEmailId59.Direction = ParameterDirection.Input;
                    objCommand59.Parameters.Add(SqlEmailId59);

                    int emailUpdateSuccess = dBConnect.DoUpdateUsingCmdObj(objCommand59);

                    //String sqlUpdateEmailTag = "Update EmailRecipient SET EmailTag = '" + tagId + "'" +
                    //    "WHERE UserId = '" + userId + "' AND EmailId = '" + sqlEmailId + "'";
                    //dBConnect = new DBConnect();
                    //int emailUpdateSuccess = dBConnect.DoUpdate(sqlUpdateEmailTag);
                }
            }
            refresh();
            if (count == 0)
            {
                Response.Write("<script>alert('There are no checkboxes selected')</script>");
            }
            //ViewCustomFolder(ddlValue);
        }

        protected void moveToDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ddlCustomFolder_SelectedIndexChanged(object sender, EventArgs e)
        {
            String ddlValueFolderSelect = ddlCustomFolder.SelectedValue.ToString();

            {
                int receiverId = Int32.Parse(Session["UserId"].ToString());
                Session["Tag"] = ddlValueFolderSelect;
                String CUSTOMFOLDER = Session["Tag"].ToString();

                //Stored Procedure #60
                //                CREATE PROCEDURE[dbo].[SelectTagIdFromUserIdAndTagName]
                //        @UserId int,
                //	@TagName varchar(50)
                //AS
                //SELECT TagId FROM Tags WHERE Tags.UserId = @UserId
                //AND Tags.TagName = @TagName
                //RETURN 0

                SqlCommand objCommand60 = new SqlCommand();

                objCommand60.CommandType = CommandType.StoredProcedure;
                objCommand60.CommandText = "SelectTagIdFromUserIdAndTagName";

                SqlParameter TagId60 = new SqlParameter("@UserId", receiverId);
                TagId60.Direction = ParameterDirection.Input;
                objCommand60.Parameters.Add(TagId60);

                SqlParameter TagName60 = new SqlParameter("@TagName", CUSTOMFOLDER);
                TagName60.Direction = ParameterDirection.Input;
                objCommand60.Parameters.Add(TagName60);

                DataSet dsTagId = dBConnect.GetDataSetUsingCmdObj(objCommand60);
                int tagId = Int32.Parse(dsTagId.Tables[0].Rows[0]["TagId"].ToString());


                //String sqlTagId = "SELECT TagId FROM Tags WHERE Tags.UserId = " + receiverId + " AND Tags.TagName = '" + CUSTOMFOLDER + "'";
                //DataSet dsTagId = dBConnect.GetDataSet(sqlTagId);

                //Stored Procedure #61 Same as Delete
                //                CREATE PROCEDURE[dbo].[SelectAllFromEmailsAndRecipientsWhereYouFindReceiverIdandTag]
                //        @ReceiverId int,
                //	@TagId int
                //AS
                //SELECT* FROM Emails, EmailRecipient
                //WHERE Emails.EmailId = EmailRecipient.EmailId
                //AND Emails.ReceiverId = @ReceiverId
                //AND EmailRecipient.EmailTag = @TagId
                //RETURN 0

                SqlCommand objCommand61 = new SqlCommand();

                objCommand61.CommandType = CommandType.StoredProcedure;
                objCommand61.CommandText = "SelectAllFromEmailsAndRecipientsWhereYouFindReceiverIdandTag";

                SqlParameter ReceiverId61 = new SqlParameter("@ReceiverId", receiverId);
                ReceiverId61.Direction = ParameterDirection.Input;
                objCommand61.Parameters.Add(ReceiverId61);

                SqlParameter TagId61 = new SqlParameter("@TagId", tagId);
                TagId61.Direction = ParameterDirection.Input;
                objCommand61.Parameters.Add(TagId61);

                DataSet emailData = dBConnect.GetDataSetUsingCmdObj(objCommand61);


                //String sqlSelectFromEmailTable = "SELECT * FROM Emails, EmailRecipient " +
                //    "WHERE Emails.EmailId = EmailRecipient.EmailId " +
                //    "AND Emails.Receiverid = '" + receiverId + "' " +
                //    "AND EmailRecipient.EmailTag = '" + tagId + "'";

                //DataSet emailData = dBConnect.GetDataSet(sqlSelectFromEmailTable);

                ArrayList emailList = new ArrayList();
                int size = emailData.Tables[0].Rows.Count;
                for (int i = 0; i < size; i++)
                {
                    int senderId = Int32.Parse(emailData.Tables[0].Rows[0]["SenderId"].ToString());
                    //Keep row at one because SQL changes in the loop will not reach past first row

                    //Stored Procedure #62 Finder UserName from UserId
                    //                    CREATE PROCEDURE[dbo].[FindUserNameByUserId]
                    //        @UserId int
                    //    AS
                    //SELECT UserName FROM Users WHERE UserId = @UserId
                    //RETURN 0
                    SqlCommand objCommand62 = new SqlCommand();

                    objCommand62.CommandType = CommandType.StoredProcedure;
                    objCommand62.CommandText = "FindUserNameByUserId";

                    SqlParameter UserId62 = new SqlParameter("@UserId", senderId);
                    UserId62.Direction = ParameterDirection.Input;
                    objCommand62.Parameters.Add(UserId62);

                    DataSet emailData3 = dBConnect.GetDataSetUsingCmdObj(objCommand62);

                 //   String sqlSelectUserNameSessionUser = "SELECT UserName FROM Users WHERE UserId = " + senderId + " ";
                 //   DataSet emailData3 = dBConnect.GetDataSet(sqlSelectUserNameSessionUser);
                    String UserNameSessionUser = emailData3.Tables[0].Rows[0]["UserName"].ToString();

                    //Stored Procedure #63 Select EmailInfo from Id

                    //                    CREATE PROCEDURE[dbo].[SelectEmailInformationFromSenderId]
                    //        @SenderId int
                    //        AS
                    //SELECT EmailSubject, EmailBody, "Timestamp", ReceiverId FROM Emails WHERE SenderId = @SenderId
                    //RETURN 0

                    SqlCommand objCommand63 = new SqlCommand();

                    objCommand63.CommandType = CommandType.StoredProcedure;
                    objCommand63.CommandText = "SelectEmailInformationFromSenderId";

                    SqlParameter SenderId63 = new SqlParameter("@SenderId", senderId);
                    SenderId63.Direction = ParameterDirection.Input;
                    objCommand63.Parameters.Add(SenderId63);

                    DataSet emaildata21 = dBConnect.GetDataSetUsingCmdObj(objCommand63);

                    //String sqlSelectReceiverId = "SELECT EmailSubject, EmailBody, Timestamp, ReceiverId FROM Emails WHERE SenderId = " + senderId + " ";
                    //DataSet emaildata21 = dBConnect.GetDataSet(sqlSelectReceiverId);
                    int receiverID = Int32.Parse(emaildata21.Tables[0].Rows[i]["ReceiverID"].ToString());

                    //Keep row at one because SQL changes in the loop will not reach past first row

                    //Stored Procedure #64 Select UserName from userId
                    //                    CREATE PROCEDURE[dbo].[FindUserNameByUserId]
                    //        @UserId int
                    //    AS
                    //SELECT UserName FROM Users WHERE UserId = @UserId
                    //RETURN 0
                    SqlCommand objCommand64 = new SqlCommand();

                    objCommand64.CommandType = CommandType.StoredProcedure;
                    objCommand64.CommandText = "FindUserNameByUserId";

                    SqlParameter UserId64 = new SqlParameter("@UserId", receiverID);
                    UserId64.Direction = ParameterDirection.Input;
                    objCommand64.Parameters.Add(UserId64);

                    DataSet emailData2 = dBConnect.GetDataSetUsingCmdObj(objCommand64);

                    //String sqlSelectUserNameRecieverId = "SELECT UserName FROM Users WHERE UserId = " + receiverID + " ";
                    //DataSet emailData2 = dBConnect.GetDataSet(sqlSelectUserNameRecieverId);
                    String receiverUserName = emailData2.Tables[0].Rows[0]["UserName"].ToString();


                    String emailsubject = emailData.Tables[0].Rows[i]["EmailSubject"].ToString();
                    String emailBody = emailData.Tables[0].Rows[i]["EmailBody"].ToString();
                    String CreatedTime = emailData.Tables[0].Rows[i]["Timestamp"].ToString();


                    Email email = new Email();
                    email.SenderName = UserNameSessionUser;
                    email.ReceiverName = Session["UserName"].ToString();
                    email.Subject = emailsubject;
                    email.EmailBody = emailBody;
                    email.CreatedTime = CreatedTime;

                    emailList.Add(email);
                }

                EmailMainView.DataSource = emailList;
                EmailMainView.DataBind();

                // int size = emailData.Tables[0].Rows.Count;
                if (size == 0)
                {
                    EmailMainView.Visible = false;
                    LabelEmpty.Text = "You Have No Emails in your " + ddlValueFolderSelect + " folder.";
                }
                else
                {
                    LabelEmpty.Text = "";
                    EmailMainView.Visible = true;
                }
                ViewCustomFolder(ddlValueFolderSelect);

            }
            }
        
        public void ViewCustomFolder(String TagName)
        { 
            Session["Tag"] = TagName;
            DisplayView(TagName);
            ComposeButtonView(false);
            buttonGroup(false);
        }

        protected void btnBackD_Click(object sender, EventArgs e)
        {
            showDetailViewOfEmail(false);
        }
    }
    }

    
    

    
