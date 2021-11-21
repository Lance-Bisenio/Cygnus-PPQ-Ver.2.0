<%@ Page Language="VB" AutoEventWireup="false" CodeFile="bom_buildperipherals.aspx.vb" Inherits="bom_buildperipherals" %>

<!DOCTYPE html>

 <html  xmlns="http://www.w3.org/1999/xhtml">
	<head>
		<title></title>
		<link href="../css/CalendarControl.css" rel="stylesheet" type="text/css" />
		<link href="../css/BasicContol.css" rel="stylesheet" type="text/css" /> 
        <script src="../js/CalendarControl.js" type="text/javascript"></script>

        <script src="../js/jquery-1.10.2.js"></script>
        <script src="../js/jquery-1.10.1.min.js"></script>
        <script src="../js/jquery-ui.js"></script>

		<script type="text/javascript">
		    function copyval() {
		        document.Form1.txtNick.value = document.Form1.txtFirst.value;
		    }

		    function invoke() {
			    <%=vScript %> 
		    }

		    function showSettings(vType) {
		        id = vType;
		        s = 0;
		        linkwin = window.open('maintenance.aspx?id=' + id + "&s=" + s,
               'viewprop', 'top=80,left=100,scrollbars=yes,resizable=yes,toolbars=no,width=850,height=550');
		        linkwin.focus();
		    }

		    function showsplit() {
		        rc = document.getElementById("cmbRC").value;

		        winsplit = window.open("splitrc.aspx?rc=" + rc, "winsplit", "top=100,left=100,width=640,height=480,resizable=yes,scrollbars=yes");
		        winsplit.focus();
		    }
		</script>
        
	    <style type="text/css">
            .auto-style1 {
                height: 27px;
            }
        </style>
        
	</head>
	<body onload="invoke();"><%--   class="SmallBox_Frame" style="background-color:#e2e2e2"--%>
		<form id="Form1" method="post" runat="server">
            <div>
		        <table id="Standard_Tbl" style="width:100%;" border="0">
                    <tr>
                        <td style="text-align:left; border-bottom: solid 1px #808080;" colspan="4">
                            <asp:Label ID="txtTitle" CssClass="vModuleTitle" runat="server" Text="BOM Peripherals settings"></asp:Label></td>
                    </tr>
                    <tr><td>&nbsp;</td></tr>
                    <asp:dropdownlist id="cmbRC" runat="server" Width="280px" class="labelL" Visible="false"></asp:dropdownlist>
			 
				    <tr>
					    <td style="width:130px;">BOM Code :</td>
					    <td style="width:500px;"> 
				            <asp:textbox id="txtBOM_Cd" runat="server" Width="120px" Text="" CssClass="labelL" Enabled="false"></asp:textbox>  
					    </td>
                        <td style="width:40px;"></td>
                        <td style="vertical-align:top;" rowspan="14"><b>Alternative Peripherals</b>
                            <div style="width:100%; height:300px; border-collapse:collapse; border: solid 1px #ccc; overflow:auto">
                                 <table style="width:99%;" border="0">
                                     <%=vAlt_Perp %>
                                 </table>
				            </div>

                        </td>
				    </tr>
				    <tr>
					    <td>Revision :</td>
					    <td> 
				            <asp:textbox id="txtBOM_Rev" runat="server" Width="120px" Text="" CssClass="labelL"  Enabled="false"></asp:textbox>  
					    </td>
				    </tr>
                    <tr>
					    <td>Item Code :</td>
					    <td> 
				            <asp:textbox id="txtBOM_ItemCd" runat="server" Width="120px" Text="" CssClass="labelL"  Enabled="false"></asp:textbox>  
					    </td>
				    </tr>
                    <tr>
					    <td>BOM Item Description :</td>
					    <td> 
				            <asp:textbox id="txtBOM_Item" runat="server" Width="450px" Text="" CssClass="labelL"  Enabled="false"></asp:textbox>  
					    </td>
				    </tr>
                    <tr>
					    <td>Section :</td>
					    <td>
				            <asp:textbox id="txtBOM_Section" runat="server" Width="450px" Text="" CssClass="labelL"  Enabled="false"></asp:textbox> 
				        </td> 
				    </tr>
				    <tr>
					    <td>Process :</td>
					    <td>
				            <asp:textbox id="txtBOM_Process" runat="server" Width="450px" Text="" CssClass="labelL"  Enabled="false"></asp:textbox> 
				        </td> 
				    </tr>
                    <tr>
					    <td>Machine Code :</td>
					    <td>
				            <asp:textbox id="txtMachCode" runat="server" Width="120px" Text="" CssClass="labelL"  Enabled="false"></asp:textbox> 
				        </td> 
				    </tr>
                    <tr>
					    <td>Machine Description :</td>
					    <td>
				            <asp:textbox id="txtMachine" runat="server" Width="450px" Text="" CssClass="labelL" Enabled="false"></asp:textbox> 
				        </td> 
				    </tr>
                    <tr><td>&nbsp;</td></tr>
				     <tr>
					    <td style="width:130px;">Peripherals  Code :</td>
					    <td> 
				            <asp:textbox id="txtPerp_Cd" runat="server" Width="120px" Text="" CssClass="labelL" Enabled="false"></asp:textbox>  
					    </td>
				    </tr>
				    <tr>
					    <td class="auto-style1">Peripherals Description :</td>
					    <td class="auto-style1">
				            <asp:dropdownlist id="cmbPerp" runat="server" Width="455"  class="labelL" AutoPostBack="True"></asp:dropdownlist>
                        </td>
				    </tr>
                     
                    <tr>
					    <td>Drawn From ? :</td>
					    <td> 
                            <asp:RadioButtonList ID="rdoDrawnFrom" runat="server" CssClass="labelL" RepeatDirection="Horizontal">
                                <asp:ListItem Selected="True" Value="Warehouse">Warehouse</asp:ListItem>
                                <asp:ListItem Value="NextProcess">Next Process</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
				    </tr>
                    <tr><td>&nbsp;</td></tr>
                    
                    <tr>
                        <td></td>
                        <td>
                            <asp:button id="cmdSave" runat="server" Text="Save"></asp:button>
						    <asp:button id="cmdCancel" runat="server" CssClass="button" Text="Cancel" OnClientClick="self.close()"></asp:button>
                            <input type="hidden" id="h_PerpList" name="h_PerpList" runat="server"  style="width:45px"/> 
                            <input type="hidden" id="h_Parent_TranId" name="TranId" runat="server"  style="width:45px"/>
                            <input type="hidden" id="h_SectCd" name="h_SectCd" runat="server"  style="width:45px"/> 
                            <input type="hidden" id="h_ProcessCd" name="h_ProcessCd" runat="server"  style="width:45px"/>
                            <input type="hidden" id="h_TranId" name="h_TranId" runat="server"  style="width:45px"/>
                        </td> 
                    </tr>
			    </table>
			</div>
		</form>
	</body>
</html>