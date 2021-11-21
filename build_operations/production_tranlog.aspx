<%@ Page Language="VB" AutoEventWireup="false" CodeFile="production_tranlog.aspx.vb" Inherits="production_tranlog" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../css/BasicContol.css" rel="stylesheet" type="text/css" /> 
    <link href="../css/jquery_datatable.css" rel="stylesheet" /> 
    <link href="../css/colorbox.css" rel="stylesheet" />
    <link href="../css/jquery-ui.css" rel="stylesheet" />  

    <script src="../js/jquery-1.10.2.js" type="text/javascript"></script>
    <script src="../media/js/jquery.dataTables.js" type="text/javascript"> </script>
    <script src="../js/jquery.colorbox-min.js" type="text/javascript">  </script>
        
    <script type="text/javascript">
        function invoke() {
            <%=vScript %>
        }
    </script> 
</head>
<body onload="invoke();">

    <form id="form1" runat="server">
    <div>
        <table id="Standard_Tbl" style="width:100%;" border="0"> 
            <tr>
                <td style="text-align:left; border-bottom: solid 1px #808080; padding-top:5px" colspan="4">
                    <asp:Label ID="txtTitle" CssClass="vModuleTitle" runat="server" Text="SFG Item Settings"></asp:Label></td>
            </tr>
            <tr><td>&nbsp;</td></tr>
             <tr>
                <td style="width:150px;">Container Type :</td>
                <td style="width:550px; vertical-align:top;">
                    <asp:DropDownList ID="cmbContainerType" runat="server" Width="206px" CssClass="labelL" >
                    </asp:DropDownList>
                </td> 
                <td></td>
                <td></td>
            </tr>
            <tr>
                <td>Weight :</td>
                <td >
                    <asp:TextBox ID="txtConWeight" runat="server" Width="200px" CssClass="labelL" ></asp:TextBox>
                    </td>
                <td></td>
                <td></td>
            </tr>
            
            <tr><td colspan="4">&nbsp;</td></tr>
            <tr>
                <td style="width:150px;">SFG Item Code :</td>
                <td style="width:550px; vertical-align:top;">
                    <asp:TextBox ID="txtSFGItemCd" runat="server" Width="500px" CssClass="labelL" ></asp:TextBox>
                </td>
                <td style="width:120px;"> </td>
                <td></td>
            </tr>
            
            <tr> 
                <td>SFG Description :</td>
                <td>
                    <asp:TextBox ID="txtSFGDescr" runat="server" Width="500px" CssClass="labelL" ></asp:TextBox>
                    </td>

                <td></td>
                <td></td>
            </tr>
            <tr> 
                <td>SFG Weight :</td>
                <td>
                    <asp:TextBox ID="TextBox1" runat="server" Width="200px" CssClass="labelL" ></asp:TextBox>
                    </td>

                <td>&nbsp;</td>
                <td></td>
            </tr>
            <tr><td colspan="4">&nbsp;</td></tr>
           
            <tr>
				<td>Report To :</td>
				<td> 
                    <asp:RadioButtonList ID="rdoReportTo" runat="server" CssClass="labelL" RepeatDirection="Horizontal" AutoPostBack="True">
                        <asp:ListItem Selected="True" Value="Warehouse">Warehouse</asp:ListItem>
                        <asp:ListItem Value="NextProcess">Next Process</asp:ListItem>
                    </asp:RadioButtonList></td>
                <td></td>
				<td></td>
			</tr>
            <tr>
                <td style="width:150px;">Process List :</td>
                <td style="width:550px; vertical-align:top;">
                    <asp:DropDownList ID="cmbOperationNo" runat="server" Width="510px" CssClass="labelL" >
                    </asp:DropDownList>
                </td> 
                <td></td>
                <td></td>
            </tr>
            <tr><td colspan="4">&nbsp;</td></tr> 
            <tr>
                <td></td>
                <td colspan="3">
                    <asp:Button ID="cmdSave" CssClass="Button" runat="server"  Text="Save" />
                    <asp:Button ID="cmdSave0" CssClass="Button" runat="server"  Text="Save and Complete" />
                    <asp:Button ID="txtClose" CssClass="Button" runat="server"  Text="Close" />  
                    <input type="hidden" id="txtOperOrder" name="txtOperOrder" runat="server" readonly="readonly" style="width:50px;"/></td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
