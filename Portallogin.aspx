<%@ Page Language="VB" AutoEventWireup="false" CodeFile="portallogin.aspx.vb" Inherits="Portallogin" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="bootstrap/css/bootstrap.min.css" rel="stylesheet" />
</head>
<body style="background: url(images/bg1.jpg) no-repeat center center fixed; background-size: cover;">
    <form id="form1" runat="server">
    <div class="container" style="width:400px; margin:auto; margin-top:100px; ">
        
        <img src="images/logofnl.jpg" style="width:350px; margin-left:20px; padding-bottom:20px; " />

        <div class="well" style="height:170px;border: solid 1px #808080; box-shadow: 5px 5px 15px #888888;">
            
            <div class="form-group">
                <label for="usr">Username :</label>
                <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control input-lg" 
                    placeholder="Enter username" autocomplete="Off" TextMode="Password"></asp:TextBox>
              
            </div>
            
            <div class="span6 pull-right" style="text-align:right"> 
                <asp:Button ID="Button1" runat="server" Text="Login" CssClass="btn btn-primary btn-lg" /> 
            </div>

            <div class="span6 pull-right" style="text-align:right">
                <h5><asp:Label ID="lblError" runat="server" Text="Invalid username, please try again." Font-Bold="False" ForeColor="Red"></asp:Label>&nbsp;&nbsp;&nbsp; </h5> 
            </div>

        </div>
    </div>
    </form>
</body>
</html>
