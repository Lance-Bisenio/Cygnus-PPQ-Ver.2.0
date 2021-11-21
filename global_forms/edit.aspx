<%@ Page Language="VB" AutoEventWireup="false" CodeFile="edit.aspx.vb" Inherits="global_forms_edit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../css/BasicContol.css" rel="stylesheet" type="text/css" /> 
	<link href="../css/CalendarControl.css" rel="stylesheet" type="text/css"  />
    <link href="../css/jquery-ui.css" rel="stylesheet" />
    <link href="../css/redmond/jquery-ui-1.10.4.custom.css" rel="stylesheet" />

    <script src="../js/jquery-1.10.2.js"></script>
    <script src="../js/jquery-1.10.1.min.js"></script>
    <script src="../js/jquery-ui.js"></script>

     <script type="text/javascript">
         function invoke() {
            <%=vScript %>
         }
         $(function () {
             $("#txtBOM_DateAvtive").datepicker();
         });

     </script>

</head>
<body onload="invoke();">
    <form id="form1" runat="server">
    <div>
        <table id="Standard_Tbl" style="width:100%;" border="0"> 
            <tr>
                <td style="text-align:left; border-bottom: solid 1px #808080; padding-top:5px" colspan="4">
                    <asp:Label ID="txtTitle" CssClass="vModuleTitle" runat="server" Text="Reference Settings"></asp:Label></td>
            </tr> 
            <tr>
                <td style="width:90px;">&nbsp;</td>
                <td></td> 
            </tr>
            <%=BuildForm %>

            <tr><td>&nbsp;</td></tr>
            <tr>
                <td>&nbsp;</td>
                <td>
                    <asp:Button id="cmdSave" runat="server" Text="Save Changes" CssClass="button"></asp:Button>
                    <asp:button id="cmdCancel" runat="server" CssClass="button" Text="Cancel" OnClientClick="self.close()" Visible="false"></asp:button>
                    <input type="hidden" id="h_mode" runat="server" />
                </td> 
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
