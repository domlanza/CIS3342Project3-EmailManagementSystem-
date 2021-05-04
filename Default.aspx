<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Project3EmailManagementSystem.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Lemongrass Mail</title>
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css" />
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" integrity="sha384-JcKb8q3iqJ61gNV9KGb8thSsNjpSL0n8PARn9HuZOnIxN0hoP+VmmDGMN5t9UJ0Z" crossorigin="anonymous" />
    <style type="text/css">
        .card {
            margin: 0 auto;
            float: none; 
            margin-bottom: 50px;
        }

        body {
            margin: 0;
        }
        .main-image {
            height: 100vh;
            width: 100vw;
            background-image: url('imgs/BackgroundImage.jpg');
            background-size: cover;
        }
    </style>

</head>
<body>
    <section class="main-image">

        <form id="login" runat="server">
            <div class="container">
                <br />
                <br />
                <div class="card" style="width: 28rem;">
                    <div class="card-body">
                        <h4>Lemongrass Mail</h4>
                        <br />
                        <label for="EmailAddress"><b>Email Address</b></label>
                        <asp:TextBox ID="inputEmail" runat="server" type="email" class="form-control" placeholder="Email Address" autofocus=""></asp:TextBox>
                        <asp:Label ID="lblValidation" runat="server"></asp:Label>
                        <br />
                        <label for="Password"><b>Password</b></label>
                        <asp:TextBox ID="inputPassword" runat="server" type="password" class="form-control" placeholder="Password"></asp:TextBox>
                        <br />
                        <center>
                    <asp:Label id="Label1" runat="server"></asp:Label>

                     <asp:Button type="button" class="btn btn-info" ID="btnLogin" runat="server" Text="Login" OnClick="btnLogin_Click" />
                     <br />
                     <br />
                    <h6>Don't have an account?</h6><br />
                    <asp:Button type="button" class="btn btn-info" ID="btnNewAccount" runat="server" Text="Create New Account" OnClick="btnNewAccount_Click"  />
                    </center>
                    </div>
                </div>
            </div>
        </form>
    </section>
</body>
</html>
