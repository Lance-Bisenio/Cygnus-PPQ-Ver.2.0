<%@ Page Language="VB" AutoEventWireup="false" CodeFile="assignpwd.aspx.vb" Inherits="assignpwd" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
    <title>Assign Password Page</title>
    <link href="css/BasicContol.css" rel="stylesheet" />
    <script language="javascript" type="text/javascript">
        function invoke() {
            <%=vScript %>
        }
        
        function validate() {
            val1 = document.getElementById("txtPwd").value;
            val2 = document.getElementById("txtRetype").value;
            
            //if(val1.length < 6 || val2.length < 6)
            //{
            //    alert('Password should be atleast 6 characters');
            //    return false;
            //}
        }
    </script>
    

    <link href="redtheme/red.css" rel="stylesheet" type="text/css" />
</head>
<body onload="invoke();" style="background-color:#e7e7e7" >
    <form id="form1" runat="server">
     <div style="width: 400px;" class="SmallBox_Frame">
			<table id="Standard_Tbl" width="90%" border="0" align="center">
            <tr>
                <td style="text-align:left; border-bottom: solid 2px #808080;" colspan="2" >
                    <asp:Label ID="txtTitle" CssClass="vModuleTitle" runat="server" Text="..."></asp:Label></td>
            </tr>
            <tr><td colspan="2">&nbsp;</td></tr>

            <tr>
                <td class="labelR">New Password :</td>
                <td class="labelL">
                    <asp:TextBox ID="txtPwd" runat="server" CssClass="label" TextMode="Password" MaxLength="16"></asp:TextBox></td>
            </tr>
            <tr>
                <td class="labelR">Re-type Password :</td>
                <td class="labelL">
                    <asp:TextBox ID="txtRetype" runat="server" CssClass="label" TextMode="Password" MaxLength="16"></asp:TextBox></td>
            </tr>
            <tr>
                <td></td>
                <td></td>
            </tr>
            <tr>
                <td></td>
                <td class="labelR">
                    <asp:Button ID="cmdSave" runat="server" CssClass="button" Text="Set Password" OnClientClick="return validate();" />
                    <asp:Button ID="cmdReturn" runat="server" CssClass="button" Text="Close" /></td>
            </tr>
        </table>
        <asp:CustomValidator ID="vldPwd" runat="server" ErrorMessage="Fields do not match"></asp:CustomValidator>    

    </div>
        
    </form>
</body>
</html>
