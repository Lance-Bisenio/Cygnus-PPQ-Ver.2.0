<%@ Page Language="VB" AutoEventWireup="false" CodeFile="bom_buildmachine.aspx.vb" Inherits="bom_buildmachine" %>

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
	<body  onload="invoke();"><%--style="background-color:#e2e2e2" class="SmallBox_Frame"--%>
		<form id="Form1" method="post" runat="server">
            <div>
		        <table id="Standard_Tbl" style="width:100%;" border="0">
                    <tr>
                        <td style="text-align:left; border-bottom: solid 1px #808080;" colspan="4">
                            <asp:Label ID="txtTitle" CssClass="vModuleTitle" runat="server" Text="Machine settings"></asp:Label></td>
                    </tr>
                    <tr><td>&nbsp;</td></tr>
                    <asp:dropdownlist id="cmbRC" runat="server" Width="280px" class="labelL" Visible="false"></asp:dropdownlist>
			 
				    <tr>
					    <td style="width:130px;">BOM Code :</td>
					    <td style="width:500px;"> 
				            <asp:textbox id="txtBOM_Cd" runat="server" Width="120px" Text="" CssClass="labelL" Enabled="false"></asp:textbox>  
					    </td>
                        <td style="width:20px;"></td>
                        <td rowspan="15" style="vertical-align:top;">
                            <b>Alternative Machine </b>
                            <div style="width:99%; height:300px; border-collapse:collapse; border: solid 1px #ccc; overflow:auto">
                                 <table style="width:100%;" border="0">
                                     <%=vAlt_Machine %>
                                 </table>
				            </div>
                            <%--<br />
                            <b>Machine Peripherals </b>
                            <div style="width:99%; height:120px; border-collapse:collapse; border: solid 1px #ccc; overflow:auto">
                                 <table style="width:100%;" border="0">
                                     <%=vMachine_Perp%>
                                 </table>
				            </div>--%>
                        </td>
				    </tr>
				    <tr>
					    <td>Revision :</td>
					    <td> 
				            <asp:textbox id="txtBOM_Rev" runat="server" Width="120px" Text="" CssClass="labelL"  Enabled="false"></asp:textbox>  
					    </td>
				    </tr>
                     <tr>
					    <td>BOM Item Code :</td>
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
                    <tr><td>&nbsp;</td></tr>
                    <tr>
					    <td class="auto-style1">Machine Code :</td>
					    <td class="auto-style1">
				           <asp:textbox id="txtMat_Cd" runat="server" Width="120px" Text="" CssClass="labelL" Enabled="false"></asp:textbox>  
                        </td>
				    </tr>
				    <tr>
					    <td>Machine Description :</td>
					    <td>
				            <asp:dropdownlist id="cmbMachine" runat="server" Width="455px"  class="labelL" AutoPostBack="True"></asp:dropdownlist>
                            <asp:button id="cmdGetItem" runat="server" CssClass="button" Text="..."></asp:button>
					    </td>
				    </tr>
				    <tr>
					    <td class="auto-style1">Cap Unit :</td>
					    <td class="auto-style1">
				            <asp:textbox id="txtCapUnit" runat="server" Width="120px"  CssClass="labelL" >0</asp:textbox> </td>
				    </tr>
                    
                    <tr>
					    <td>Cap Unit UOM :</td>
					    <td>
				            <asp:dropdownlist id="cmbUom" runat="server" Width="126px"  class="labelL" Enabled="false"></asp:dropdownlist>
                            </td>
				    </tr> 
				    <tr>
					    <td>Lead Day / Time :</td>
					    <td>
                            <asp:TextBox ID="txtLeadDays" runat="server" Width="45px" CssClass="labelL" >0</asp:TextBox> /
                            <asp:TextBox ID="txtLeadTime" runat="server" Width="60px" CssClass="labelL" >00:00</asp:TextBox>
                            &nbsp;DD / HH:MM 
				            <%--<asp:textbox id="txtMinutes" runat="server" Width="120px"  CssClass="labelL" ></asp:textbox>
                            <img src="images/clock.png" style="vertical-align:middle; cursor:pointer;" id="Img2" alt="" onclick="showSettings(id);"/> in minutes--%>
                            </td>
				    </tr> 
                    <tr>
					    <td>Default :</td>
					    <td> 
                            <asp:RadioButtonList ID="rdoDefault" runat="server" CssClass="labelL" RepeatDirection="Horizontal">
                                <asp:ListItem Selected="True" Value="1">Warehouse</asp:ListItem>
                                <asp:ListItem Value="2">Next Process</asp:ListItem>
                            </asp:RadioButtonList>  
                        </td>
				    </tr>
                    <tr><td>&nbsp;</td></tr>
                    
                    
                    <tr>
                        <td></td>
                        <td>
                            <asp:button id="cmdSave" runat="server" Text="Save"></asp:button>
						    <asp:button id="cmdCancel" runat="server" CssClass="button" Text="Cancel" OnClientClick="self.close()"></asp:button>
                            <input type="hidden" id="txtSave" name="txtSave" runat="server"  style="width:45px"/>
                            <input type="hidden" id="h_MachList" name="h_MachList" runat="server"  style="width:45px"/>

                            <input type="hidden" id="h_TranId" name="h_TranId" runat="server"  style="width:45px"/>
                            <input type="hidden" id="h_SectCd" name="h_SectCd" runat="server"  style="width:45px"/>
                            <input type="hidden" id="h_ProcessCd" name="h_ProcessCd" runat="server"  style="width:45px"/>
                        </td> 
                    </tr>
			    </table>
			</div>
		</form>
	</body>
</html>
