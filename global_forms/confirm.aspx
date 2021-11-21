<%@ Page Language="VB" AutoEventWireup="false" CodeFile="confirm.aspx.vb" Inherits="global_forms_confirm" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../css/BasicContol.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
         .vTile { padding-bottom: 9px; font-family: Arial, Century Gothic; font-size: 13px; font-weight:bold; color:#808080;}
    </style>
    <script type="text/javascript">
        function ShowTabValue(vLink) {
            document.getElementById("frmContent").src = vLink; 
        }
        function invoke() {
            <%=vScript %> 
        }

    </script>
</head>
<body style="background-color:#e7e7e7" onload="invoke();" >
    <form id="form1" runat="server">
    <div class="SmallBox_Frame" style="width: 95%; margin:auto" >
         <table id="Standard_Tbl" style="width:100%;" border="0">
            <tr>
                <td style="text-align:left; border-bottom: solid 1px #808080;" colspan="4">
                    <asp:Label ID="txtTitle" CssClass="vModuleTitle" runat="server" Text="..."></asp:Label></td>
            </tr>
            <tr><td colspan="2">&nbsp;</td></tr>  
			<tr>
                <td style="width:5px;"></td>
                <td class="labelL" ><h2><b><asp:Label ID="lblMess" runat="server" /></b></h2></td></tr>  
			<tr>
				<td></td>
				<td><asp:textbox id="txtReason"  runat="server" Width="98%" Height="100px"  CssClass="labelL" TextMode="MultiLine" ></asp:textbox></td> 
			</tr>
             <tr>
                <td></td> 
				<td>
                    <asp:button ID="cmdSave" runat="server"  Text="Save" /> 
                    <asp:button id="cmdCancel" runat="server" CssClass="button" Text="Cancel" OnClientClick="self.close()"></asp:button>
				</td>
				
			</tr>
             <tr><td colspan="2">&nbsp;</td></tr>  
        </table>
    </div>
    </form>
</body>
</html>
