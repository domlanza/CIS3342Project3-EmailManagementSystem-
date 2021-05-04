<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EmailClient.aspx.cs" Inherits="Project3EmailManagementSystem.EmailClient" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Lemongrass Mail</title>
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" integrity="sha384-JcKb8q3iqJ61gNV9KGb8thSsNjpSL0n8PARn9HuZOnIxN0hoP+VmmDGMN5t9UJ0Z" crossorigin="anonymous" />
    <style>
        select {
            text-align-last: center;
            text-align: center;
            -ms-text-align-last: center;
            -moz-text-align-last: center;
        }



        .labels {
            padding-bottom: 30px;
        }

        a {
            padding-left: 10px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <%--        Page Content--%>
        <%--header--%>
        <nav class="navbar navbar-expand-lg navbar-light border-bottom">
            <asp:Image ID="EmailuserAvatar" runat="server" ImageUrl="~/imgs/10.jpg" Width="55" Height="55" class="rounded" />
            <a class="navbar-brand text-dark" href="#">Lemongrass Mail</a>
            <button class="navbar-toggler" type="button" data-toggle="collaspe" data-target="#navbarCollaspe" aria-controls="navbarCollapse" aria-expanded="false" aria-label="Toggle navigation">
                <span class="navbar-toggle-icon"></span>
            </button>

            <div class="collapse navbar-collapse" id="navbarCollapse">
                <ul class="navbar-nav mr-auto">
                    <li class="nav-item">
                        <div class="form-inline">
                            <asp:TextBox ID="SearchEmail" runat="server" class="form-control mx-lg-4" placeholder="Search Email" aria-label="Search Email"></asp:TextBox>
                            <asp:Button ID="btnSearchEmail" runat="server" Text="Search" class="btn btn-outline-success my-2 my-sm-0" Visible="True" OnClick="btnSearchEmail_Click" />
                        </div>
                    </li>
                </ul>
                <div class="form-inline mt-2 mt-md-0">
                    <asp:Button ID="btnLogout" runat="server" Text="Logout" class="btn btn-outline-warning my-2 my-sm-0" OnClick="btnLogout_Click" />
                </div>
            </div>
        </nav>

        <div class="container-fluid">
            <div class="row">
                <nav id="sidebarMenu" class="col-md-3 col-lg-2 d-md-block bg-light sidebar collapse">
                    <asp:Label ID="EmailUserName" runat="server" Text="UserName" CssClass="text-dark"></asp:Label>
                    <br />
                    <div class="position-sticky pt-3">

                        <div class="container-fluid">
                            <div class="bg-light border-right" id="sidebar-wrapper">


                                <!-- Navigation links in sidebar-->
                                <div class="list-group list-group-flush">
                                    <asp:Button ID="btnCompose" runat="server" Text="+Compose" CssClass="btn btn-primary" OnClick="btnCompose_Click" />
                                    <asp:LinkButton ID="LinkButtonInbox" runat="server" class=" text-dark" OnClick="LinkButtonInbox_Click">Inbox</asp:LinkButton>
                                    <asp:LinkButton ID="LinkButtonSent" runat="server" class="text-dark" OnClick="LinkButtonSent_Click">Sent</asp:LinkButton>
                                    <asp:LinkButton ID="LinkButtonFlag" runat="server" class=" text-dark" OnClick="LinkButtonFlag_Click">Flag</asp:LinkButton>
                                    <asp:LinkButton ID="LinkButtonJunk" runat="server" class="  text-dark" OnClick="LinkButtonJunk_Click1">Junk</asp:LinkButton>
                                    <asp:LinkButton ID="LinkButtonTrash" runat="server" class="text-dark" OnClick="LinkButtonTrash_Click">Trash</asp:LinkButton>
                                    <asp:LinkButton ID="LinkButtonAllEmail" runat="server" class=" text-dark" OnClick="LinkButtonAllEmail_Click">All Emails</asp:LinkButton>
                                    <asp:DropDownList ID="ddlCustomFolder" runat="server" class="  text-dark" AutoPostBack="True" OnSelectedIndexChanged="ddlCustomFolder_SelectedIndexChanged"></asp:DropDownList>
                                </div>
                            </div>
                        </div>
                    </div>
                </nav>

                <main class="col-md-9 ms-sm-auto col-lg-10 px-md-4">
                    <div class="table-responsive">
                        <table class="table table-striped table-sm">
                            <div id="content">
                                <%--                Email Content--%>
                                <div class="dropdown">
                                    <asp:DropDownList ID="moveToDropDown" runat="server" class="btn btn-secondary btn-sm dropdown-toggle" AutoPostBack="True" Visible="false" OnSelectedIndexChanged="moveToDropDown_SelectedIndexChanged"></asp:DropDownList>
                                    <asp:Button ID="btnMoveToFolder" runat="server" Text="Move to Folder" class="btn btn-secondary btn-sm" Visible="false" OnClick="btnMoveToFolder_Click" />
                                    <asp:Button ID="btnFlag" runat="server" Text="Flag" class="btn btn-secondary btn-sm" Visible="false" OnClick="btnFlag_Click" />
                                    <asp:Button ID="btnUnFlag" runat="server" Text="unFlag" class="btn btn-secondary btn-sm" Visible="false" OnClick="btnUnFlag_Click" />
                                    <asp:Button ID="btnDelete" runat="server" Text="Delete" class="btn btn-secondary btn-sm" Visible="false" OnClick="btnDelete_Click" />
                                    <asp:TextBox ID="txtAddFolder" runat="server" type="text" class="" placeholder="Add new folder" aria-label="Add new folder" Visible="false" Width="162px"></asp:TextBox>
                                    <asp:Button ID="btnAddFolder" runat="server" Text="Add" class="btn btn-secondary btn-sm" Visible="false" OnClick="btnAddFolder_Click" />
                                </div>
                                <br />
                                <asp:Label ID="LabelEmpty" runat="server" Text="Your Inbox is empty."></asp:Label>
                                <asp:GridView ID="EmailMainView" runat="server" AutoGenerateColumns="False" OnRowCommand="EmailMainView_RowCommand" GridLines="Horizontal" CssClass="table table-hover" BorderStyle="None" ShowHeader="False" OnSelectedIndexChanged="EmailMainView_SelectedIndexChanged">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Select">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkSelectEmail" runat="server" AutoPostBack="true" OnCheckedChanged="chkSelectEmail_CheckedChanged" />
                                            </ItemTemplate>

                                            <ItemStyle Width="20px" HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:TemplateField>
                                        <asp:ButtonField Text="Select" />

                                        <%--                    CHANGED SENDERID AND RECEIVERID TO sendername receivername--%>
                                        <asp:BoundField DataField="SenderName" HeaderText="Sender">
                                            <ItemStyle Width="200px" VerticalAlign="Middle" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ReceiverName" HeaderText="Receiver" />
                                        <asp:BoundField DataField="Subject" HeaderText="Subject">
                                            <ItemStyle Width="300px" VerticalAlign="Middle" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="EmailBody" HeaderText="Content">
                                            <ItemStyle Width="800px" VerticalAlign="Middle" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="CreatedTime" HeaderText="CreatedTime">
                                            <ItemStyle Width="100px" VerticalAlign="Middle" />
                                        </asp:BoundField>
                                    </Columns>
                                    <RowStyle VerticalAlign="Middle" />
                                </asp:GridView>

                                <div class="container-fluid">
                                    <asp:Label ID="ComposeTitle" runat="server"> <h3>New Message</h3></asp:Label>
                                    <br />
                                    <asp:Label ID="FromWho" runat="server" Text="From: " class="mr-sm-2" Font-Bold="True"></asp:Label>
                                    <asp:TextBox ID="EmailFrom" runat="server" class="form-control my-2 my-sm-0" placeholder="Sender Email" aria-label="Sender Email" aria-describedly="basic-addon1" Width="256px"></asp:TextBox>
                                    <br />
                                    <asp:Label ID="ToWhom" runat="server" Text="To: " class="mr-sm-2" Font-Bold="True"></asp:Label>
                                    <asp:TextBox ID="EmailTo" runat="server" class="form-control my-2 my-sm-0" placeholder="Receiver Email" aria-label="Reciever Email" aria-describedly="basic-addon1" Width="256px"></asp:TextBox>
                                    <br/>
                                    <asp:Label ID="LabelValidation" runat="server" Text="" class="mr-sm-2" Font-Bold="True" style="color:red"></asp:Label>

                                    <br />
                                    <asp:Label ID="EmailSubjectTitle" runat="server" Text="Subject: " class="mr-sm-2" Font-Bold="True"></asp:Label>
                                    <asp:TextBox ID="EmailSubject" runat="server" class="form-control my-2 my-sm-0" placeholder="Email Subject" aria-label="Email Subject" aria-describedly="basic-addon1" Width="960px"></asp:TextBox>
                                    <br />
                                    <asp:Label ID="EmailBodyTitle" runat="server" Text="Email Content: " class="mr-sm-2" Font-Bold="True"></asp:Label>
                                    <asp:TextBox ID="EmailBody" runat="server" class="form-control my-2 my-sm-0" placeholder="Email Body" aria-label="Email Body" aria-describedly="basic-addon1" Height="256px" TextMode="MultiLine" Width="960px"></asp:TextBox>
                                    <br />
                                    <asp:Button ID="btnBack" runat="server" Text="Back" class="btn btn-primary" OnClick="btnBack_Click" />
                                    <asp:Button ID="btnSendEmail" runat="server" Text="Send" class="btn btn-primary" OnClick="btnSendEmail_Click" />
                                </div>
                                <div class="container-fluid">
                                    <table class="table table-borderless">
                                        <tbody>

                                            <tr>
                                                <td>
                                                    <asp:Label ID="SubjectT" runat="server" Font-Bold="true" Text="Subject:"></asp:Label></td>
                                                <td>
                                                    <asp:Label ID="SubjectD" runat="server" Text="Subject"></asp:Label></td>
                                            </tr>

                                            <tr>
                                                <td>
                                                    <asp:Label ID="createdTimeT" runat="server" Text="CreatedTime: " class="mr-sm-2" Font-Bold="true"></asp:Label></td>
                                                <td>
                                                    <asp:Label ID="createdTimeD" runat="server" Text="" class="mr-sm-2"></asp:Label></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="fromWhoEmailT" runat="server" Text="From: " class="mr-sm-2" Font-Bold="true"></asp:Label></td>
                                                <td>
                                                    <asp:Label ID="fromEmailD" runat="server" Text="" class="mr-sm-2"></asp:Label></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="toWhoEmailT" runat="server" Text="To: " class="mr-sm-2" Font-Bold="true"></asp:Label></td>
                                                <td>
                                                    <asp:Label ID="toEmailD" runat="server" Text="" class="mr-sm-2"></asp:Label></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="emailBodyT" runat="server" Text="EmailBody: " class="mr-sm-2" Font-Bold="true"></asp:Label></td>
                                                <td colspan="2">
                                                    <asp:Label ID="emailBodyD" runat="server" Text="" class="mr-sm-2"></asp:Label></td>
                                            </tr>
                                        </tbody>
                                    </table>
                                    <asp:Button ID="btnBackD" runat="server" Text="Back" class="btn btn-primary" OnClick="btnBackD_Click" />
                                </div>
                            </div>
                        </table>
                    </div>
                </main>
            </div>
        </div>
    </form>
</body>
</html>
