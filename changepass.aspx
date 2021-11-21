<%@ Page Language="vb" AutoEventWireup="false" Inherits="denaro.ChangePass" CodeFile="changepass.aspx.vb" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
	<head>
		<title>Change User Password</title>
        <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR" />
        <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE" />
        <meta content="JavaScript" name="vs_defaultClientScript" />
        <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />
        <link href="css/BasicContol.css" rel="stylesheet" type="text/css" />

		<script language="javascript" type="text/javascript">

		    function invoke() {
		         <%=vScript%>    
		    }
		    
		    function validate()
		    {
                if (document.getElementById("txtPwd").value == "")
                {
                    alert("Please enter your current password!");
                    document.getElementById("txtPwd").focus()
                    return false
                }
                else if (document.getElementById("txtNew").value == "")
                {
                    alert("Please enter your new password!");
                    document.getElementById("txtNew").focus()
                    return false
                }
                else if (document.getElementById("txtConfirm").value == "")
                {
                    alert("Please reenter your new password!");
                    document.getElementById("txtConfirm").focus()
                    return false
                }
		    }  
		</script>
	</head>
	<body style="background-color:#e7e7e7" onload="invoke();">
	    
	<form id="Form1" method="post" runat="server">
	    <center>
	        <div style="width: 600px;" class="SmallBox">
			    <table id="Standard_Tbl" width="90%" border="0" align="center">
			        <tr>
			            <td></td>
			            <td style="font-weight:bold; color:#ff0000">
			                To change your password, you must first type in your current password.</td>
			        </tr>
			        <tr>
			            <td></td>
			            <td style="font-weight:bold; color:#ff0000; text-decoration:blink;">
			                NOTE: Password field is case-sensitive</td>
			        </tr>
			        <tr><td colspan="2">&nbsp;</td></tr>
				    <tr>
					    <td>Userid :</td>
					    <td>
						    <asp:textbox id="txtUserid" runat="server" Width="95%" ReadOnly="True" ></asp:textbox><font size="2"></font></td>
				    </tr>
				    <tr>
					    <td>Current Password :</td>
					    <td>
						    <asp:textbox id="txtPwd" tabIndex="1" runat="server" 
						    TextMode="Password" CssClass="label" Width="95%" MaxLength="16"></asp:textbox></td>
				    </tr>
				    <tr>
					    <td>New Password :</td>
					    <td>
						    <asp:textbox id="txtNew" tabIndex="1" runat="server"
						    TextMode="Password" CssClass="label" Width="95%" MaxLength="16"></asp:textbox></td>
				    </tr>
				    <tr>
					    <td>Retype Password :</td>
					    <td>
						    <asp:textbox id="txtConfirm" tabIndex="1" runat="server"
						    TextMode="Password" CssClass="label" Width="95%" MaxLength="16"></asp:textbox></td>
				    </tr>
				    <tr>
				        <td></td>
				        <td>
				            <asp:Button id="cmdChange" runat="server" Text="Change Password" OnClientClick="return validate();"></asp:Button>
			                <input type="reset" value="Reset" >
			                <asp:Button id="cmdReturn" runat="server" Text="Logout"></asp:Button> <br />
			                <asp:CustomValidator id="vldCheck" runat="server" Width="400px" Height="88px" CssClass="label"></asp:CustomValidator>
				        </td>
				    </tr>
			    </table>
                <br />
                
			        
                    
            </div>
        </center>
		       
    </form>
	</body>
</html>
