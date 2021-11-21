<%@ Page Language="VB" AutoEventWireup="false" CodeFile="bom_activation.aspx.vb" Inherits="bom_activation" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
		<title></title>
		 <link rel="stylesheet" type="text/css" href="css/CalendarControl.css">
        <script src="js/CalendarControl.js" language="javascript"></script>
		<link href="css/BasicContol.css" rel="stylesheet" type="text/css" /> 

		<script language="javascript" type="text/javascript">
		    
		    function invoke() {
			    <%=vScript %>
		        resize();

		    }
		    
		</script>
        
	    <style type="text/css">
            .auto-style1 {
                height: 27px;
            }
            .auto-style2 {
                height: 8px;
                width: 130px;
            }
            .auto-style3 {
                width: 130px;
            }
            .auto-style4 {
                height: 27px;
                width: 130px;
            }
        </style>
        
	</head>
	<body style="background-color:#e2e2e2" onload="invoke();">
    <form id="form1" runat="server">
    <div class="SmallBox_Frame">
		<table id="Standard_Tbl" style="width:100%;" border="0">
            <tr>
                <td style="text-align:left; border-bottom: solid 1px #808080;" colspan="2">
                    <asp:Label ID="txtTitle" CssClass="vModuleTitle" runat="server" Text="Activate Bill of Manufacturing (BOM)"></asp:Label></td>
            </tr>
            <tr><td class="auto-style2"></td></tr> 
			<tr>
				<td class="auto-style3">BOM Code :</td>
				<td> 
				    <asp:textbox id="txtBOM_Cd" runat="server" Width="120px" Text="" CssClass="labelL" Enabled="false"></asp:textbox>  
				</td>
			</tr>
			<tr>
				<td class="auto-style3">Last Revision :</td>
				<td> 
				    <asp:textbox id="txtRev" runat="server" Width="120px" Text="" CssClass="labelL" Enabled="false"></asp:textbox>  
				</td>
			</tr>
            <tr>
				<td class="auto-style3">Item Code :</td>
				<td> 
				    <asp:textbox id="txtBOM_Item" runat="server" Width="272px" Text="" CssClass="labelL"  Enabled="false"></asp:textbox>  
				</td>
			</tr>
            <tr>
				<td class="auto-style3">Item Description :</td>
				<td> 
				    <asp:textbox id="txtBOM_ItemDescr" runat="server" Width="272px" Text="" CssClass="labelL"  Enabled="false"></asp:textbox>  
				</td>
			</tr>
            <tr><td class="auto-style3">&nbsp;</td></tr>
            <tr>
				<td class="auto-style3" style="vertical-align:top;">Remarks :</td>
				<td> 
				    <asp:textbox id="txtRemarks" runat="server" Width="274px" Text="" CssClass="labelL" Height="60px" TextMode="MultiLine"></asp:textbox>  
				</td>
			</tr>
            
             
            <tr>
                <td class="auto-style3"></td>
                <td>
                    <asp:button id="cmdSave" runat="server" CssClass="button" Text="Save"></asp:button>
					<asp:button id="cmdCancel" runat="server" CssClass="button" Text="Cancel" OnClientClick="self.close()"></asp:button>
                    <input id="h_QA_list" runat="server" name="h_QA_list" style="width:80px" type="hidden" /> 
                </td> 
            </tr>
		</table>
	</div>
    </form>
</body>
</html>
