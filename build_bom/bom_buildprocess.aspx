<%@ Page Language="VB" AutoEventWireup="false" CodeFile="bom_buildprocess.aspx.vb" Inherits="bom_buildprocess" %>

<!DOCTYPE html>


<html  xmlns="http://www.w3.org/1999/xhtml">
	<head>
		<title></title>
		<%--<link href="../css/BasicContol.css" rel="stylesheet" type="text/css" /> --%>
		<link href="../css/CalendarControl.css" rel="stylesheet" type="text/css"  />
        <link href="../css/jquery-ui.css" rel="stylesheet" />
        <link href="../css/redmond/jquery-ui-1.10.4.custom.css" rel="stylesheet" />
        <link href="../bootstrap/css/bootstrap.css" rel="stylesheet" />
         
	    </head>
	<body onload="invoke();">
        <%--style="background-color:#e2e2e2" class="SmallBox_Frame"--%>
		<form id="Form1" method="post" runat="server">

            <%--<asp:dropdownlist id="cmbFSG" runat="server" Width="455px"  class="form-control input-xs" AutoPostBack="True"></asp:dropdownlist> 
            <asp:textbox id="txtSFG" runat="server" Width="120px" Text="" CssClass="form-control input-xs" ></asp:textbox>  
            <asp:button id="cmdGetItem" runat="server" CssClass="button" Text="Find" EnableTheming="False" ></asp:button>--%>

            <div>
		        <table style="width:98%; margin:auto;" border="0">
                    <tr>
                        <td style="text-align:left; border-bottom: solid 1px #808080;" colspan="6">
                            <h3><asp:Label ID="txtTitle" CssClass="vModuleTitle" runat="server" Text="Process Settings"></asp:Label></h3></td>
                    </tr>
                    <tr><td style="height:8px;"></td></tr> 
				    <tr>
					    <td class="text-right" style="width:150px;">BOM Code :</td>
					    <td style="width:400px;"> 
                            <table>
                                <tr>
                                    <td><asp:textbox id="txtBOM_Cd" runat="server" Text="" CssClass="form-control input-xs" Enabled="false" Width="120px" ></asp:textbox></td>
                                    <td class="text-right" >&nbsp;&nbsp;&nbsp;Revision :</td>
                                    <td><asp:textbox id="txtBOM_Rev" runat="server" Width="120px" Text="" CssClass="form-control input-xs"  Enabled="false"></asp:textbox></td>
                                </tr>
                            </table> 
					    </td>
                        <td style="width:5px; width:200px;"></td>
                        <td style="vertical-align:top;"> 
                            
                        </td>
				    </tr>
                    <tr>
					    <td class="text-right">FG Code :</td>
					    <td> 
				            <asp:textbox id="txtItem_Cd" runat="server" Width="120px" Text="" CssClass="form-control input-xs"  Enabled="false"></asp:textbox>  
					    </td>
                        <td class="text-right">Kilos :</td>
					    <td> 
				            <asp:textbox id="txtKilo" runat="server" Width="120px" Text="" CssClass="form-control input-xs"></asp:textbox>  
					    </td>
				    </tr>
                    <tr>
					    <td class="text-right">FG Description :</td>
					    <td> 
				            <asp:textbox id="txtBOM_Item" runat="server" Text="" CssClass="form-control input-xs"  Enabled="false"></asp:textbox>  
					    </td>
                        <td class="text-right">Meters :</td>
					    <td> 
				            <asp:textbox id="txtMeter" runat="server" Width="120px" Text="" CssClass="form-control input-xs" ></asp:textbox>  
					    </td>
				    </tr>
                    <tr>
					    <td class="text-right">SFG Description :</td>
					    <td> 
				            <asp:textbox id="txtSFG" runat="server" Text="" CssClass="form-control input-xs"  Enabled="true" placeholder="Optional"></asp:textbox>  
					    </td>
				    </tr>
                    <tr>
					    <td class="text-right">Section Name :</td>
					    <td> 
				            <asp:dropdownlist id="cmbSection" runat="server" class="form-control input-xs" AutoPostBack="True"></asp:dropdownlist>
					    </td>
                        <td class="text-right">StartUp Duration :</td>
					    <td>
                            <table>
                                <tr>
                                    <td><asp:TextBox ID="txtSDTime" runat="server" Width="60px" CssClass="form-control input-xs" MaxLength="4" >00</asp:TextBox></td>
                                    <td><asp:dropdownlist id="cmbSRDMins" runat="server" Width="80px" CssClass="form-control input-xs"></asp:dropdownlist></td>
                                    <td>HH:MM </td>
                                </tr>
                            </table>
					    </td>
				    </tr>
				    <tr>
					    <td class="text-right">Process Name :</td>
					    <td>
				            <asp:dropdownlist id="cmbProcess" runat="server" class="form-control input-xs"></asp:dropdownlist>
				        </td> 
                        <td class="text-right">Initial Run Duration :</td>
					    <td>
                            <table>
                                <tr>
                                    <td><asp:TextBox ID="txtIRDTime" runat="server" Width="60px" CssClass="form-control input-xs" MaxLength="4" >00</asp:TextBox></td>
                                    <td><asp:dropdownlist id="cmbIRDMins" runat="server" Width="80px"  class="form-control input-xs"></asp:dropdownlist> </td>
                                    <td>HH:MM</td>
                                </tr>
                            </table>
					    </td>
				    </tr>

				    <tr>
					    <td class="text-right">Operating Order No :</td>
					    <td>
				            <asp:textbox id="txtOperatingOrderNo" runat="server" Width="120px"  CssClass="form-control input-xs">0</asp:textbox>
					    </td>
                        <td class="text-right">Prod Run Duration :</td>
					    <td>
                            <table>
                                <tr>
                                    <td><asp:TextBox ID="txtPRDTime" runat="server" Width="60px" CssClass="form-control input-xs" MaxLength="4" >00</asp:TextBox></td>
                                    <td><asp:dropdownlist id="cmbPRDMins" runat="server" Width="80px"  class="form-control input-xs"></asp:dropdownlist> </td>
                                    <td>HH:MM</td>
                                </tr>
                            </table>
					    </td>
				    </tr> 
                    <tr>
					    <td class="text-right">Qty Output :</td>
					    <td>
				            <asp:textbox id="txtQty" runat="server" Width="120px"  CssClass="form-control input-xs">0</asp:textbox>
                        </td>
                        <td class="text-right">With Container ? :</td>
					    <td>
                            <asp:RadioButtonList ID="rdoWithContainer" runat="server" CssClass="" Width="100px" RepeatDirection="Horizontal">
                                <asp:ListItem Value="0">Yes</asp:ListItem>
                                <asp:ListItem Selected="True"  Value="1">No</asp:ListItem>
                            </asp:RadioButtonList>
					    </td>
				    </tr>  
                    <tr>
					    <td class="text-right">Qty Output UOM :</td>
					    <td>
				            <asp:dropdownlist id="cmbQtyOutUOM" runat="server" Width="120px"  class="form-control input-xs"></asp:dropdownlist> 
                        </td>
                        <td class="text-right">Container UOM :</td>
					    <td>
				            <asp:dropdownlist id="cmbConUOM" runat="server" Width="120px"  class="form-control input-xs"></asp:dropdownlist> 
                        </td>
				    </tr>
                    <tr><td style="height:8px;"></td></tr> 
                    <tr>
                        <td></td>
                        <td>
                            <asp:button id="cmdSave" runat="server" Text="Save and Next" CssClass="btn btn-primary btn-sm"></asp:button>
						    <asp:button id="cmdCancel" runat="server" CssClass="btn btn-default btn-sm" Text="Cancel" OnClientClick="self.close()"></asp:button>

                            <input id="h_QA_list" runat="server" name="h_QA_list" style="width:80px" type="hidden" />  
                            <input id="h_TranId" runat="server" name="h_TranId" style="width:80px" type="hidden" /> 
                            <input id="h_SFGCode" runat="server" name="h_SFGCode" style="width:80px" type="hidden" /> 
                        </td> 
                    </tr>
                    <tr><td>&nbsp;</td></tr>
			    </table>

                <%--<div style="width:95%; height:280px; border-collapse:collapse; border: solid 1px #ccc; overflow:auto; margin:auto;">
                        <table class="table table-bordered" style="width:100%;" border="1" >
                        <thead><tr><th colspan="4">&nbsp;List of Materials</th></tr></thead>
                        <tr> 
                            <td style="width:100px; text-align:center">Mat Code</td>
                            <td style=" text-align:center">Description</td>
                            <td style="width:60px; text-align:center">Grams</td>
                            <td style="width:90px; text-align:center">Percentage</td>
                        </tr>
                        <%=vMat_List%>
                    </table>
				</div>--%>

			</div>
		</form>
	</body>
</html>


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