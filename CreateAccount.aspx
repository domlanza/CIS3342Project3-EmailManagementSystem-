<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CreateAccount.aspx.cs" Inherits="Project3EmailManagementSystem.CreateAccount" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Create Account </title>
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" integrity="sha384-JcKb8q3iqJ61gNV9KGb8thSsNjpSL0n8PARn9HuZOnIxN0hoP+VmmDGMN5t9UJ0Z" crossorigin="anonymous" />
    <style type="text/css">
        .card {
            margin: 0 auto;
            float: none;
            margin-bottom: 0px;
        }

        body {
            margin: 0;
            padding: 0;
        }

        .main-image {
            background-image: url('imgs/BackgroundImage.jpg');
            background-size: cover;
            background-repeat: repeat-y;
        }

        #links a {
            color: #000000;
            font-size: 18px;
        }
    </style>
</head>
<body>
    <form id="login" runat="server">
        <section class="main-image">
            <nav class="navbar navbar-dark bg-dark bg-transparent fixed-top navbar-expand-md">
                <div id="navbar" class="navbar-collapse collapse">
                    <ul class="nav navbar-nav" id="links">
                        <li>
                            <a href="Default.aspx" class="nav-link">Back to Login</a>
                        </li>
                    </ul>
                </div>
            </nav>
            <br />
            <div class="container">
                <br />
                <br />
                <div class="card" style="width: 28rem;">
                    <div class="card-body">
                        <h4>Create New Account</h4>
                        <br />
                        <label><b>Select Your Account Type</b></label>
                        <br />
                                                <asp:Label runat="server" ID="lblValidation"></asp:Label>
                                                <br />



                        <asp:DropDownList ID="ddlAccountType" runat="server" Width="200px" CssClass="form-control">
                            <asp:ListItem Text="Administrator" Value="Admin" Selected="True" />
                            <asp:ListItem Text="User" Value="User" />

                        </asp:DropDownList><br />


                        <br />
                        <label for="username"><b>Username</b></label>
                        <asp:TextBox ID="TxtUserName" runat="server" class="form-control" placeholder="Enter a Username" required/>

                        <br />
                        <br />

                        <label for="PhoneNumber"><b>Phone Number</b></label>
                        <asp:TextBox ID="TxtPhoneNumber" runat="server" class="form-control" placeholder="(215) 111-1111" required/>
                        <br />
                        <asp:Label runat="server" ID="Label1"></asp:Label>
                        <br />

                        <div class="d-flex align-items-center">
                            <div class="d-inline-block">

                                <label for="avatar">Select a profile picture:</label>
                                <asp:Image ID="AvatarImg" runat="server" ImageUrl="~/imgs/10.jpg" Width="48" Height="48" class="rounded" />
                                <div class="d-inline-block">

                                    <asp:DropDownList ID="ddlAvatarImg" AutoPostBack="true" runat="server" CssClass="form-control" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged">
                                        <asp:ListItem>1</asp:ListItem>
                                        <asp:ListItem>2</asp:ListItem>
                                        <asp:ListItem>3</asp:ListItem>
                                        <asp:ListItem>4</asp:ListItem>
                                        <asp:ListItem>5</asp:ListItem>
                                        <asp:ListItem>6</asp:ListItem>
                                        <asp:ListItem>7</asp:ListItem>
                                        <asp:ListItem>8</asp:ListItem>
                                        <asp:ListItem>9</asp:ListItem>
                                        <asp:ListItem Selected="True">10</asp:ListItem>
                                    </asp:DropDownList>
                                </div>

                            </div>
                        </div>
                        <br />

                        <label for="Create Email"><b>Create Email</b></label>
                        <asp:TextBox ID="TxtCreateEmail" runat="server" class="form-control" placeholder="JohnDoe@example.com" required/>
                        <br />
                        <asp:Label runat="server" ID="Label2"></asp:Label>
                        <br />

                        <label for="Security Email"><b>Security Email</b></label>
                        <asp:TextBox ID="TxtSecurityEmail" runat="server" class="form-control" placeholder="JohnDoe@example.com" required/>

                        <br />
                        <asp:Label runat="server" ID="Label3"></asp:Label>
                        <br />

                        <label for="Password"><b>Password</b></label>
                        <asp:TextBox ID="txtPassword" type="password" runat="server" class="form-control" placeholder="password" required/>

                        <br />
                        <asp:Label runat="server" ID="Label4"></asp:Label>
                        <br />

                        <asp:Label runat="server" ID="Label5"></asp:Label>
                        <br />
                        <asp:Button type="button" class="btn btn-info" ID="btnCreate" runat="server" Text="Create Account" OnClick="btnCreate_Click" />
                    </div>
                </div>
            </div>
        </section>

    </form>
</body>
</html>
