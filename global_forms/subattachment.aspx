<%@ Page Language="VB" AutoEventWireup="false" CodeFile="subattachment.aspx.vb" Inherits="subattachment" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../css/BasicContol.css" rel="stylesheet" type="text/css" />
    
    <script src="../js/jquery-1.10.2.js" type="text/javascript"></script>
    <script src="../media/js/jquery.dataTables.js" type="text/javascript"> </script>
    <script language="javascript" type="text/javascript">
        function invoke() {
        <%=vscript %>
        }
            function remove(a) {
                document.getElementById("Text1").value = a;
                document.form1.submit();
            }
    </script>
</head>

<body style="background-color:#e7e7e7" onload="invoke();">
    <form id="form1" runat="server">
    <div class="SmallBox_Frame">

        <table style="width:100%" cellpadding="0" cellspacing="0" border="0">
             <tr>
                <td style="text-align:left; border-bottom: solid 1px #808080; padding-top:5px" colspan="4">
                    <asp:Label ID="txtTitle" CssClass="vModuleTitle" runat="server" Text="Upload Item Images"></asp:Label></td>
            </tr> 
            <tr>
                <td style="height: 160px; vertical-align:top;" colspan="2">
                    <%=mysubattachments%>    
                    <input id="Text1" type="hidden" runat="server" /> 
                </td>
            </tr>
            <tr><td colspan="2"><hr /></td></tr>
            <tr> 
                <td class="labelL">
                    <b>Filename :</b>
                    <asp:FileUpload ID="fileuploader" runat="server" Width="260px" CssClass="labelL" />
                    <asp:Button ID="cmdSave" runat="server" Text="Add"  />
                    <asp:Button ID="cmdClose" runat="server" Text="Close" CssClass="button"    />
                </td>
            </tr>
        </table>
    </div>
    
    </form>
</body>
</html>
