<%@ Page Language="VB" AutoEventWireup="false" CodeFile="bom_buildmaterials.aspx.vb" Inherits="bom_buildmaterials" %>

<!DOCTYPE html>

<html  xmlns="http://www.w3.org/1999/xhtml">
	<head>
		<title></title>
		<link href="../css/CalendarControl.css" rel="stylesheet" type="text/css"  /> 
        <link href="../bootstrap/css/bootstrap.css" rel="stylesheet" />
	            
	</head>
	<body  onload="invoke();">
		<form id="Form1" method="post" runat="server">
            <div><%-- class="SmallBox_Frame">style="background-color:#e2e2e2"--%>
                <asp:dropdownlist id="cmbRC" runat="server" Width="280px" class="form-control input-xs" Visible="false"></asp:dropdownlist>
		        <table style="width:98%; margin:auto;" border="0">
                    <tr>
                        <td style="text-align:left; border-bottom: solid 1px #808080;" colspan="4">
                            <h5><asp:Label ID="txtTitle" CssClass="vModuleTitle" runat="server" Text="Raw Materials settings"></asp:Label></h5></td>
                    </tr>
                    <tr><td style="height:10px;"></td></tr>
				    <tr>
					    <td class="text-right" style="width:130px;">BOM Code :</td>
					    <td style="width:400px;"> 
				            
                            <table>
                                <tr>
                                    <td class="text-right"><asp:textbox id="txtBOM_Cd" runat="server" Width="120px" Text="" CssClass="form-control input-xs" Enabled="false"></asp:textbox>  </td>
                                    <td class="text-right" style="Width:100px;" >&nbsp;&nbsp;&nbsp;Revision :</td>
                                    <td><asp:textbox id="txtBOM_Rev" runat="server" Width="120px" Text="" CssClass="form-control input-xs"  Enabled="false"></asp:textbox></td>
                                </tr>
                            </table> 

					    </td>
                        <td style="width:20px;"></td>
                        <td rowspan="14" style="vertical-align:top;">
                            <b>Alternative Materials </b>
                            <div style="width:99%; height:355px; border-collapse:collapse; border: solid 1px #ccc; overflow:auto">
                                 <table style="width:100%;" border="0">
                                     <%=vAlt_Materials %>
                                 </table>
				            </div>

                        </td>
				    </tr>
				     
                     <tr>
					    <td class="text-right">BOM Item Code :</td>
					    <td> 
				            <asp:textbox id="txtBOM_ItemCd" runat="server" Width="120px" Text="" CssClass="form-control input-xs"  Enabled="false"></asp:textbox>  
					    </td>
				    </tr>
                    <tr>
					    <td class="text-right">FG Description :</td>
					    <td> 
				            <asp:textbox id="txtBOM_Item" runat="server"  Text="" CssClass="form-control input-xs"  Enabled="false"></asp:textbox>  
					    </td>
				    </tr>
                    <%--<tr><td style="height:10px;"></td></tr>--%>
                    <tr>
					    <td class="text-right">Section :</td>
					    <td>
				            <asp:textbox id="txtBOM_Section" runat="server"  Text="" CssClass="form-control input-xs"  Enabled="false"></asp:textbox> 
				        </td> 
				    </tr>
				    <tr>
					    <td class="text-right">Process :</td>
					    <td>
				            <asp:textbox id="txtBOM_Process" runat="server"  Text="" CssClass="form-control input-xs"  Enabled="false"></asp:textbox> 
				        </td> 
				    </tr>
                    
                    <tr>
					    <td class="text-right">SFG Code :</td>
					    <td>
				            <asp:textbox id="txtSFGCode" runat="server"  Text="" CssClass="form-control input-xs"  Enabled="false"></asp:textbox> 
				        </td> 
				    </tr>
                    <tr>
					    <td class="text-right">SFG Description :</td>
					    <td>
				            <asp:textbox id="txtSFGDescr" runat="server"  Text="" CssClass="form-control input-xs"  Enabled="false"></asp:textbox> 
				        </td> 
				    </tr>
                    <tr><td style="height:10px;"></td></tr>
				   <tr>
					    <td class="text-right" title="Raw Materials Code">Raw Mats Code :</td>
					    <td>
                            <table>
                                <tr>
                                    <td class="text-right">
                                        <asp:textbox id="txtMat_Cd" runat="server" Width="120px" Text="" CssClass="form-control input-xs"></asp:textbox>  
                                    </td>
                                    <td class="text-right">
                                        <asp:button id="cmdGetItem0" runat="server" CssClass="btn btn-primary btn-xs" Text="Find" EnableTheming="False" ></asp:button>
                                    </td>
                                </tr>
                            </table> 
				            
                            
                        </td>
				    </tr>
				    <tr>
					    <td class="text-right" title="Raw Materials Description">Raw Mats Descr :</td>
					    <td>
				            <asp:dropdownlist id="cmbMat" runat="server"   class="form-control input-xs" AutoPostBack="True"></asp:dropdownlist>
                            
                            <%--<asp:button id="cmdGetItem" runat="server" CssClass="button" Text="..."></asp:button>--%>
                        </td>
				    </tr>
                    <tr>
					    <td class="text-right">Required Qty :</td>
					    <td>				            
                            <table border="0">
                                <tr>
                                    <td class="text-right">
                                        <asp:textbox id="txtOutput_Qty" runat="server" Width="120px"  CssClass="form-control input-xs" Text="0"></asp:textbox>            
                                    </td>
                                    <td class="text-right" style="Width:100px;" >&nbsp;&nbsp;&nbsp;Grams :</td>
                                    <td><asp:textbox id="txtGrams" runat="server" Width="120px" Text="0" CssClass="form-control input-xs" ></asp:textbox></td>
                                </tr>
                            </table> 
                            </td>
				    </tr> 
                    <tr>
					    <td class="text-right">Material UOM :</td>
					    <td>
                            
                            <table border="0">
                                <tr>
                                    <td class="text-right">
                                        <asp:dropdownlist id="cmbMatUOM" runat="server" Width="120px"  class="form-control input-xs"></asp:dropdownlist>
                                    </td>
                                    <td class="text-right" style="Width:100px;">&nbsp;&nbsp;&nbsp;Percentage :</td>
                                    <td><asp:textbox id="txtPerc" runat="server" Width="120px" Text="0" CssClass="form-control input-xs" ></asp:textbox></td>
                                </tr>
                            </table> 
                        </td>
				    </tr> 
				    
                    <tr>
					    <td class="text-right">Drawn From ? :</td>
					    <td>
                            <asp:RadioButtonList ID="rdoDrawnFrom" runat="server" CssClass="" RepeatDirection="Horizontal" Width="220px">
                                <asp:ListItem Selected="True" Value="Warehouse">Warehouse</asp:ListItem>
                                <asp:ListItem Value="NextProcess">Next Process</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
				    </tr>
                    <tr><td>&nbsp;</td></tr> 
                    <tr>
                        <td></td>
                        <td>
                            <asp:button id="cmdSave" runat="server" Text="Save" CssClass="btn btn-primary btn-sm"></asp:button>
						    <asp:button id="cmdCancel" runat="server" CssClass="btn btn-default btn-sm" Text="Cancel" OnClientClick="self.close()"></asp:button>
                            <input type="hidden" id="h_MatList" name="h_MatList" runat="server"  style="width:45px"/>
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
        