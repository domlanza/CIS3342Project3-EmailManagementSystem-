<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EmailManager.aspx.cs" Inherits="Project3EmailManagementSystem.EmailManager" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Admin Lemongrass Mail</title>
        <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" integrity="sha384-JcKb8q3iqJ61gNV9KGb8thSsNjpSL0n8PARn9HuZOnIxN0hoP+VmmDGMN5t9UJ0Z" crossorigin="anonymous" />

    <style>
        footer {
        padding-bottom:10px;
        }
        </style>


</head>
<body data-gr-c-s-loaded="true">
    <form id="form1" runat="server">
        <%--Nav--%>
        <header>
            <div class="navbar navbar-expand-md navbar-light bg-light">
                <div class="container d-flex justify-content-between">
                    <a class="navbar-brand" href="#">Lemongrass Mail</a>
                    <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarCollapse" aria-controls="navbarCollapse" aria-expanded="false" aria-label="Toggle navigation">
                        <span class="navbar-toggler-icon"></span>
                    </button>
                    <div class="collapse navbar-collapse" id="navbarCollapse">
                        <ul class="navbar-nav mr-auto"></ul>
                        <div class="form-inline mt-2 mt-md-0">
                            <asp:Button ID="btnLogout" runat="server" Text="Logout" class="btn btn-outline-warning my-2 my-sm-0" OnClick="btnLogout_Click"/>
                            </div>
                    </div>
                </div>
            </div>
        </header>
        <br />





        <main role="main">
            <center>
             <asp:Button ID="ShowCurrentAccounts" runat="server" Text="Show Current Accounts" class="btn btn-primary btn-lg" OnClick="ShowCurrentAccounts_Click" /> &nbsp
             <asp:Button ID="ShowFlaggedEmailInformation" runat="server" Text="Flagged Email Information" class="btn btn-success btn-lg" OnClick="ShowFlaggedEmailInformation_Click"/>
            </center>
            <br />


            <div class="container">
                <div class="row justify-content-center">
                <div class="col">
<%--                    Account infor--%>
                    <div>
                        <center>
                    <asp:Label ID="lbAccount" runat="server"><h3>Current Accounts</h3></asp:Label>
                        <p>
                            <asp:Button ID="btnBan" runat="server" Text="Ban" class="btn btn-danger btn-lg" OnClick="btnBan_Click"/> &nbsp
                            <asp:Button ID="btnUnBan" runat="server" Text="Remove Ban" class="btn btn-danger btn-lg" OnClick="btnUnBan_Click"/> 
                        </p>
                        <asp:GridView ID="gvAccount" runat="server" AutoGenerateColumns="false">
                            <Columns>
                                <asp:TemplateField HeaderText="Select">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkAccount" runat="server"/>
                                    </ItemTemplate>
                                       </asp:TemplateField>

                                    <asp:ImageField DataImageUrlField="Avatar" HeaderText="Avatar" ControlStyle-Width="30" ControlStyle-height="30"></asp:ImageField>
                                    <asp:BoundField DataField="UserName" HeaderText="UserName"/>
                                    <asp:BoundField DataField="PhoneNumber" HeaderText="PhoneNumber"/>
                                    <asp:BoundField DataField="CreatedEmailAddress" HeaderText="Email"/>
                                    <asp:BoundField DataField="Active" HeaderText="Active"/>
                            </Columns>
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"/>
                            <RowStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:GridView>
                          </center>

                    </div>
<%--                    The flag email information gridview--%>
                    <div>
                        <center>
                        <asp:Label ID="lblFlagEmail" runat="server"><h3>Flagged Email Information</h3></asp:Label>
                        <br/>
                        <asp:Label ID="lblEmpty" runat="server" Text="No flag emails found." Visible="false"></asp:Label>
                        <asp:GridView ID="gvFlagEmail" runat="server" AutoGenerateColumns="false" AutoGenerateSelectButton="True" OnSelectedIndexChanged="gvFlagEmail_SelectedIndexChanged">
                            <Columns>
                                <asp:BoundField DataField="SenderName" HeaderText="SenderEmail" />
                                <asp:BoundField DataField="ReceiverName" HeaderText="ReceiverEmail" />
                                <asp:BoundField DataField="Subject" HeaderText="Subject" />
                                <asp:BoundField DataField="EmailBody" HeaderText="Content" />
                                <asp:BoundField DataField="CreatedTime" HeaderText="CreatedTime" />
                                </Columns>
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            <RowStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:GridView>
                            </center>
                </div>  
                    
                    <div class="container-fluid">
                        <table class="table table-borderless">
                          <tbody>
                            <tr>
                                <td><asp:Label ID="lblSubject" runat="server" Text="Subject: " class="mr-sm-2" Font-Bold="true"></asp:Label></td>
                                <td><asp:Label ID="subjectOfPerson" runat="server" Text="Subject"></asp:Label></td>
                            </tr>
                            <tr>
                                <td><asp:Label ID="CreatedTimeFirstPerson" runat="server" Text="CreatedTime: " class="mr-sm-2" Font-Bold="true"></asp:Label></td>
                                <td><asp:Label ID="CreatedTimeSecondPerson" runat="server" Text="" class="mr-sm-2"></asp:Label></td>
                            </tr>
                            <tr>
                               <td><asp:Label ID="FromEmailFirstPerson" runat="server" Text="From: " class="mr-sm-2" Font-Bold="true"></asp:Label></td>
                               <td><asp:Label ID="FromEmailSecondPerson" runat="server" Text="" class="mr-sm-2"></asp:Label></td>
                            </tr>
                            <tr>
                               <td><asp:Label ID="ToEmailFirstPerson" runat="server" Text="To: " class="mr-sm-2" Font-Bold="true"></asp:Label></td>
                               <td><asp:Label ID="ToEmailSecondPerson" runat="server" Text="" class="mr-sm-2"></asp:Label></td>
                            </tr>
                            <tr>
                               <td><asp:Label ID="EmailBodyFirstPerson" runat="server" Text="EmailBody: " class="mr-sm-2" Font-Bold="true"></asp:Label></td>
                                                           <td colspan="2"><asp:Label ID="EmailBodySecondPerson" runat="server" Text="" class="mr-sm-2"></asp:Label></td>

                            </tr>
                        </tbody>
                    </table>
                    <asp:Button ID="btnBackD" runat="server" Text="Back" class="btn btn-primary" OnClick="btnBackD_Click"/>
                            </div>
                </div>
            </div>
        </main>
    </form>
  

</body>
    <footer>
        </footer>
</html>
