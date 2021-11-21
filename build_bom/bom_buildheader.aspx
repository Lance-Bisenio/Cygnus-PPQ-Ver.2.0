<%@ Page Language="VB" AutoEventWireup="false" CodeFile="bom_buildheader.aspx.vb" Inherits="bom_buildheader" %>

<!DOCTYPE html>
<html lang="en">
	<head>
		<title></title>
        <%--<link href="../css/BasicContol.css" rel="stylesheet" type="text/css" /> --%>
		<link href="../css/CalendarControl.css" rel="stylesheet" type="text/css"  />
        <link href="../css/jquery-ui.css" rel="stylesheet" />
        <link href="../css/redmond/jquery-ui-1.10.4.custom.css" rel="stylesheet" />
		<link href="../bootstrap/bootstrap/css/bootstrap.css" rel="stylesheet" />

        <script src="../js/jquery-1.10.2.js"></script>
        <script src="../js/jquery-1.10.1.min.js"></script>
        <script src="../js/jquery-ui.js"></script>

		<style>
			.rpad {
				padding:2px;
			}
			body {
				font-family:Arial;
			}
		</style>

		 
		<script type="text/javascript">
		    function copyval() {
		        document.Form1.txtNick.value = document.Form1.txtFirst.value;
		    } 
		    function invoke() {
			    <%=vScript %>
			    //resize(); 
		    }

		    $(function() {
		        $("#txtBOM_DateAvtive").datepicker();
		    });
             
		</script>
        
	    </head>
	<body  onload="invoke();"><%--style="background-color:#e2e2e2"--%>
		<form id="Form1" method="post" runat="server">
			<div class="container-fluid">
				<div class="row rpad"> 
					<div class="col-lg-12">
						<h3><asp:Label ID="txtTitle" CssClass="vModuleTitle" runat="server" Text="BOM Header Settings"></asp:Label></h3> 
						<hr />
					</div>
					
				</div>
				<div class="row rpad">
					<div class="form-group form-group-sm"> 
						<div class="col-lg-2">BOM Code :</div>
						<div class="col-lg-4">
							<div class="col-xs-6">
								<asp:textbox id="txtBOM_No" runat="server" Text="Auto Generated" CssClass="form-control" ForeColor="Gray" ReadOnly="True"></asp:textbox> 
							</div> 
						</div> 
						<div class="col-lg-2">Net Weight :</div>
						<div class="col-lg-4">
							<div class="col-xs-6">
								<asp:textbox id="txtNetW" runat="server" CssClass="form-control" Text="0"></asp:textbox>
							</div>
						</div>
					</div> 
				</div>
				
				<div class="row rpad">
					<div class="form-group form-group-sm">
						<div class="col-lg-2">Revision :</div>
						<div class="col-lg-4">
							<div class="col-sm-6">
								<asp:textbox id="txtBOM_Rev" runat="server" Text="Auto Generated" CssClass="form-control" ForeColor="Gray" ReadOnly="True"></asp:textbox>
							</div>
						</div>
						<div class="col-lg-2">Weight UOM :</div>
						<div class="col-lg-4">
							<div class="col-sm-6">
								<asp:dropdownlist id="cmbNetWUOM" runat="server" class="form-control" ></asp:dropdownlist> 
							</div>
						</div> 
					</div>
				</div>
				<div class="row rpad">
					<div class="form-group form-group-sm">
						<div class="col-lg-2">Item Code :</div>
						<div class="col-lg-4">
							<div class="col-sm-6">
								<asp:textbox id="txtItemCd" runat="server" Text="" CssClass="form-control" ></asp:textbox> 
							</div>
							<asp:button id="cmdGetItemDetails" runat="server" CssClass="btn btn-primary btn-sm" Text="Find"></asp:button>  
						</div>
						<div class="col-lg-2">Min Order Qty :</div>
						<div class="col-lg-4">
							<div class="col-sm-6">
								<asp:textbox id="txtMOQty" runat="server" CssClass="form-control" Text="0"></asp:textbox> 
							</div>
						</div> 
					</div>
					
				</div>
				<div class="row rpad">
					<div class="form-group form-group-sm">
					<div class="col-lg-2">Item Description :</div>
					<div class="col-lg-4">
						<div class="col-sm-12">
						<asp:dropdownlist id="cmbItem" runat="server" class="form-control" AutoPostBack="True" Width="100%"></asp:dropdownlist>
						</div>
					</div>
					<div class="col-lg-2">Min Order UOM :</div>
					<div class="col-lg-4">
						<div class="col-sm-6">
						<asp:dropdownlist id="cmbMOUOM" runat="server" class="form-control"></asp:dropdownlist>
						</div>
					</div> 
					</div>
				</div>
				<div class="row rpad">
					<div class="form-group form-group-sm">
					<div class="col-lg-2">Item UOM :</div>
					<div class="col-lg-4">
						<div class="col-sm-6">
						<asp:dropdownlist id="cmbItemUom" runat="server" class="form-control" AutoPostBack="false" ></asp:dropdownlist>
						</div>
					</div>
					<div class="col-lg-2">Prod Lead Day/Time :</div>
					<div class="col-lg-4">
						<div class="col-sm-6">
							<asp:TextBox ID="txtSPRunDays" runat="server" CssClass="form-control" >0</asp:TextBox> 
							<asp:TextBox ID="txtSPRunTime" runat="server" CssClass="form-control" >00:00</asp:TextBox> 
						</div>
						&nbsp;DD / HH:MM:SS
					</div> 
					</div>
				</div>
				
				<div class="row rpad">
					<div class="form-group form-group-sm">
					<div class="col-lg-2">Item Type :</div>
					<div class="col-lg-4">
						<div class="col-sm-6">
						<asp:textbox id="txtItemType" runat="server" CssClass="form-control" ReadOnly="true"></asp:textbox>
                        <asp:textbox id="txtItemType_Cd" runat="server" CssClass="form-control" ReadOnly="true" Visible="false"></asp:textbox> 
						</div>
					</div>
					<div class="col-lg-2"></div>
					<div class="col-lg-4"></div> 
					</div>
				</div>
				<div class="row rpad">
					<div class="form-group form-group-sm">
					<div class="col-lg-2">Status :</div>
					<div class="col-lg-4">
						<div class="col-sm-6">
						<asp:dropdownlist id="cmbStatus" runat="server" class="form-control" AutoPostBack="True"></asp:dropdownlist>
						</div>
					</div>
					<div class="col-lg-2"></div>
					<div class="col-lg-4"></div> 
					</div>
				</div>
				<div class="row rpad">
					<div class="form-group form-group-sm">
					<div class="col-lg-2">Date Active :</div>
					<div class="col-lg-4">
						<div class="col-sm-6">
							<asp:textbox id="txtBOM_DateAvtive" runat="server" CssClass="form-control" ReadOnly="true"></asp:textbox> 
						</div>
						<img src="../images/calendar.png" style="vertical-align:top;" alt="" /> 
				        MM / DD / YYYY
					</div>
					<div class="col-lg-2"></div>
					<div class="col-lg-4"></div> 
					</div>
				</div>
				<div class="row rpad">
					<div class="form-group form-group-sm">
					<div class="col-lg-2">Report To :</div>
					<div class="col-lg-4">
						<div class="col-sm-12">
							<div class="checkbox">
								<label>
									<asp:RadioButtonList ID="rdoReportTo" runat="server" RepeatDirection="Horizontal">
										<asp:ListItem Selected="True" Value="Warehouse">Warehouse</asp:ListItem>
										<asp:ListItem Value="NextProcess">Next Process</asp:ListItem>
									</asp:RadioButtonList>
								</label>
							</div>
						
						</div>
					</div>
					<div class="col-lg-2"></div>
					<div class="col-lg-4">
						 
					</div> 
					</div>
				</div>
				<div class="row rpad"> 
					<div class="col-lg-12"> 
						<hr />
					</div> 
				</div>
				<div>
					<div class="col-lg-2"></div>
					<div class="col-lg-4">
						<div class="col-sm-12">
							<asp:button id="cmdSave" runat="server" CssClass="btn bg-primary btn-sm" Text="Save"></asp:button>
							<asp:button id="cmdCancel" runat="server" CssClass="btn btn-primary btn-sm" Text="Cancel" OnClientClick="self.close()"></asp:button>
						</div>
						
					</div>
					<div class="col-lg-2"></div>
					<div class="col-lg-4"></div>
				</div>
			</div>
			 
             
		</form>


	    
	</body>
</html>
