<%@ Page Language="VB" AutoEventWireup="false" CodeFile="modifyuser.aspx.vb" Inherits="modifyuser" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
    <title>Modify User Page</title>
    <link href="css/BasicContol.css" rel="stylesheet" type="text/css" />
     <style type="text/css">
         .vTile { padding-bottom: 9px; font-family: Arial, Century Gothic; font-size: 13px; font-weight:bold; color:#808080;}
    </style>
    
    <script language="javascript" type="text/javascript">
        function invoke() {
            <%=vScript %>
        }
        
        function postvalidate() {
            if(document.getElementById("txtUserId").value=="") {
                alert("Userid field should not be empty.");
                return false;
            }
            if(document.getElementById("txtFullName").value=="") {
                alert("Fullname field should not be empty.");
                return false;
            }
            return true;
        }
    </script>
</head>
<body style="background-color:#e7e7e7" onload ="invoke();">
    <form id="form1" runat="server">
    <center>
    <div style="width: 500px;" class="SmallBox">
    
        <table align="center" border="0" style="width: 100%" id="Standard_Tbl" >
            <tr>
                    <td style="text-align:left; border-bottom: solid 2px #808080;" colspan="2">
                        <asp:Label ID="txtTitle" CssClass="vTile" runat="server" Text="User Maintenance"></asp:Label></td>
            </tr>
            <tr><td colspan="2">&nbsp;</td></tr>
            <tr>
                <td style="width:120px;">Username / Emp Code :</td>
                <td><asp:TextBox ID="txtUserId" runat="server" CssClass="labelL" Width="218px"></asp:TextBox>
                    <asp:Button ID="cmbFind" runat="server" CssClass="button" Text="Find" />
                </td>
            </tr>
            <tr>
                <td>&nbsp;Password :</td>
                <td>
                    <asp:TextBox ID="txtPwd" runat="server" CssClass="labelL" TextMode="Password" MaxLength="16" Width="218px" Font-Size="Medium"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>&nbsp;Re-type Password :</td>
                <td>
                    <asp:TextBox ID="txtRetype" runat="server" CssClass="labelL" TextMode="Password" MaxLength="16" Width="218px" Font-Size="Medium"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td></td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>Fullname :</td>
                <td>
                    <asp:TextBox ID="txtFullName" runat="server" CssClass="labelL" Width="218px"></asp:TextBox></td>
            </tr>
            <tr>
                <td>Position :</td>
                <td>
                    <asp:DropDownList ID="cmbPosition" runat="server" Width="224px" CssClass="labelL"></asp:DropDownList>
                    <%--<asp:DropDownList ID="cmbPosition" runat="server" Width="222px" >
                        <asp:ListItem Selected="True">Team Leader</asp:ListItem>
                        <asp:ListItem>Processor</asp:ListItem>
                        <asp:ListItem>Administrator</asp:ListItem>
                    </asp:DropDownList>--%></td>
            </tr>
            <tr>
                <td>Employee Status : </td>
                <td>
                    <asp:DropDownList ID="cmbEmpStatus" runat="server" Width="224px" CssClass="labelL">
                        <asp:ListItem Selected="True" Value="Active">Active</asp:ListItem>
                        <asp:ListItem Value="InActive">InActive</asp:ListItem>
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td></td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>&nbsp;Email :</td>
                <td>
                    <asp:TextBox ID="txtEmail" runat="server" Width="218px" CssClass="labelL"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>Line Number :</td>
                <td>
                    <asp:TextBox ID="txtLineNum" runat="server" Width="218px" MaxLength="3" CssClass="labelL"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td></td>
                <td>
                    <asp:Button ID="cmdSave" runat="server" CssClass="button" Text="Save" />
                    <asp:Button ID="cmdCancel" runat="server" CssClass="button" Text="Cancel" /></td>
            </tr>
        </table>

    </div>
    </center>
    </form>
</body>
</html>
