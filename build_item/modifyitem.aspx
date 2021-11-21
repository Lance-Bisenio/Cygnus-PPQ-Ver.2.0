<%@ Page Language="VB" AutoEventWireup="false" CodeFile="modifyitem.aspx.vb" Inherits="modifyitem" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>New/Modify Item</title>
    <link href="css/BasicContol.css" rel="stylesheet" />
    <script language="javascript" type="text/javascript">
        function invoke() {
            <%=vScript %>
        }
    </script>
</head>
<body onload="invoke();" style="background-color:#e7e7e7" >
    <form id="form1" runat="server">
    <center>
    
    <div style="width: 600px;" class="SmallBox_Frame">
			<table id="Standard_Tbl" width="90%" border="0" align="center">
            <tr>
                <td style="text-align:left; border-bottom: solid 2px #808080;" colspan="2" >
                    <asp:Label ID="txtTitle" CssClass="vModuleTitle" runat="server" Text="..."></asp:Label></td>
            </tr>
                <tr><td colspan="2">&nbsp;</td></tr>
            <tr>
                <td class="labelR">Budget ID :</td>
                <td class="labelL">
                    <asp:TextBox ID="txtBudgId" runat="server" CssClass="labelL" ReadOnly="True"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="labelR">Item ID :</td>
                <td class="labelL">
                    <asp:TextBox ID="txtParticularId" runat="server" CssClass="labelL" 
                        ReadOnly="True">Auto-generated</asp:TextBox>
                </td>
            </tr>
            <%--<tr>
                <td class="labelR">SAP Code :</td>
                <td class="labelL">
                    <asp:TextBox ID="txtSAPCode" runat="server" CssClass="labelL" ></asp:TextBox>
                </td>
            </tr>--%>
            <tr>
                <td class="labelR">Sub Account :</td>
                <td class="labelL">
                    <asp:RadioButtonList ID="rdoSubAcct" runat="server" RepeatDirection="Horizontal" CssClass="labelL">
                        <asp:ListItem Value="0">Yes</asp:ListItem>
                        <asp:ListItem Value="1" Selected="True">No</asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td class="labelR">&nbsp;View By :</td>
                <td class="labelL">
                    <asp:RadioButtonList ID="rdoViewBy" runat="server" RepeatDirection="Horizontal" CssClass="labelL">
                        <asp:ListItem Selected="True" Value="0">Amount</asp:ListItem>
                        <asp:ListItem Value="1">Quantity</asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
           <tr><td>&nbsp;</td></tr>
            <tr>
                <td class="labelR">Parent Item :</td>
                <td class="labelL">
                    <asp:DropDownList ID="cmbParent" runat="server" CssClass="labelL" Width="322px">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="labelR">Filter Acct Code by :</td>
                <td class="labelL">
                    <asp:DropDownList ID="cmbFilterAcct" runat="server" CssClass="labelL" Width="322px" AutoPostBack="True">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="labelR">Acct Code :</td>
                <td class="labelL">
                    <asp:DropDownList ID="cmbAcctCd" runat="server" CssClass="labelL" Width="322px" AutoPostBack="True">
                    </asp:DropDownList>
                </td>
            </tr>
           
             <tr>
                <td class="labelR">
                    Item Name :</td>
                <td class="labelL">
                    <asp:TextBox ID="txtParticular" runat="server" CssClass="labelL" Width="316px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="labelR">&nbsp;</td>
                <td class="labelL">
                    <asp:Button ID="cmdSave" runat="server" Text="Save" CssClass="button" />
                    <input id="cmdClose" class="button" type="button" value="Cancel" onclick="window.close();" />
                </td>
            </tr>
        </table>
    </div>
    </center>
    </form>
</body>
</html>
