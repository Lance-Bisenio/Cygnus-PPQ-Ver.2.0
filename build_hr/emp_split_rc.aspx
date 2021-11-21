<%@ Page Language="VB" AutoEventWireup="false" CodeFile="emp_split_rc.aspx.vb" Inherits="emp_split_rc" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Cost Center Split Page</title> 
		<link href="css/BasicContol.css" rel="stylesheet" type="text/css" /> 
     
    
    <script language="javascript" type="text/javascript">
        function invoke() {
            <%=vScript %>
        }
         
    </script>

</head>
<body style="background-color:#e7e7e7" onload="invoke();">
		<form id="Form1" method="post" runat="server">
            <div class="SmallBox_Frame">
		        <table id="Standard_Tbl" style="width:100%;" border="0">
                    <tr>
                        <td style="text-align:left; border-bottom: solid 1px #808080;" colspan="2">
                            <asp:Label ID="txtTitle" CssClass="vModuleTitle" runat="server" Text="Employee multiple cost center"></asp:Label></td>
                    
                    </tr>
                    <tr><td>&nbsp;</td></tr>
                    <tr>
                        <td>
                            <asp:Panel ID="Panel1" runat="server" Height="400px" ScrollBars="Vertical" CssClass="labelL">
                                <asp:CheckBoxList ID="chkCodes" runat="server" CssClass="labelL" 
                                    RepeatLayout="Flow">
                                </asp:CheckBoxList>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr><td> 
                        <asp:Button ID="cmdSelect" runat="server" CssClass="button" Text="Select All" />
                        <asp:Button ID="cmdDeselect" runat="server" CssClass="button" Text="Deselect All" />
                        <asp:Button ID="cmdSave" runat="server" CssClass="button" Text="Save" />
                        <input id="cmdClose" class="button" type="button" value="Close" onclick="window.close();"/>
                        </td></tr>
			    </table>
			</div>
    
    </form>
</body>
</html>

