<%@ Page Language="VB" AutoEventWireup="false" CodeFile="item_general.aspx.vb" Inherits="Item_general" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    
    <%--<link href="../bootstrap/css/bootstrap.css" rel="stylesheet" />--%>
	<link href="../bootstrap/bootstrap/css/bootstrap.css" rel="stylesheet" />
    <style>
    	 
    	div .col-md-5, div .col-md-3 {
    		padding:1px;
		}
    	body {
			font-family:Arial; font-size:12px;
    	}
    </style>
</head>
<body onload="invoke(); ">
    <form id="form1" runat="server">
    <div class="container">
		<h4>Item Settings</h4> 
		<hr />
		<div class="row"> 
			<div class="col-md-2 text-right">Item Code :</div>
			<div class="col-md-5"><asp:TextBox ID="txtItemCd" runat="server" Width="" CssClass="form-control" placeholder="Required" ></asp:TextBox></div>
			<div class="col-md-2 text-right">Barcode :</div>
			<div class="col-md-3"><asp:TextBox ID="txtBarcode" runat="server" Width="150px" CssClass="form-control" ></asp:TextBox></div>
		</div>
        
        <div class="row">
			<div class="col-md-2 text-right">Description 1 :</div>
			<div class="col-md-5"><asp:TextBox ID="txtDescr1" runat="server" Width="" CssClass="form-control" placeholder="Required"></asp:TextBox></div>
			<div class="col-md-2 text-right">Serial No.:</div>
			<div class="col-md-3"><asp:TextBox ID="txtSerialNo" runat="server" Width="150px" CssClass="form-control" ></asp:TextBox></div>
		</div>
        
		<div class="row">
			<div class="col-md-2 text-right">Description 2 :</div>
			<div class="col-md-5"><asp:TextBox ID="txtDescr2" runat="server" Width="" CssClass="form-control" ></asp:TextBox></div>
			<div class="col-md-2 text-right">Cost :</div>
			<div class="col-md-3"><asp:TextBox ID="txtCost" runat="server" Width="150px" CssClass="form-control" >0</asp:TextBox> </div>
		</div>
        
		<div class="row">
			<div class="col-md-2 text-right">Parent Item :</div>
			<div class="col-md-5"><asp:DropDownList ID="cmbParentItem" runat="server" Width="" CssClass="form-control"></asp:DropDownList></div>
			<div class="col-md-2 text-right">UOM :</div>
			<div class="col-md-3"><asp:DropDownList ID="cmbUomQty" runat="server" Width="150px" CssClass="form-control"></asp:DropDownList></div>
		</div>
        
		<div class="row">
			<div class="col-md-2 text-right">Item Type:</div>
			<div class="col-md-5"><asp:DropDownList ID="cmbItemType" runat="server" Width="200px" CssClass="form-control"></asp:DropDownList></div>
			<div class="col-md-2 text-right">Sub Type :</div>
			<div class="col-md-3"><asp:DropDownList ID="cmbTypeClass" runat="server" Width="150px" CssClass="form-control"></asp:DropDownList></div>
		</div>

        <div class="row">
			<div class="col-md-2 text-right">Source :</div>
			<div class="col-md-5"><asp:DropDownList ID="cmbSource" runat="server" Width="200px" CssClass="form-control"></asp:DropDownList></div>
			<div class="col-md-2 text-right">Core Weight :</div>
			<div class="col-md-3"><asp:TextBox ID="txtCoreWeight" runat="server" Width="150px" Text="0" CssClass="form-control" ></asp:TextBox></div>
		</div>
        
        <div class="row">
			<div class="col-md-2 text-right">Min Order Qty :</div>
			<div class="col-md-5"><asp:TextBox ID="txtMinOrderQty" runat="server" Width="200px" CssClass="form-control" >0</asp:TextBox></div>
			<div class="col-md-2 text-right">Status :</div>
			<div class="col-md-3">
				<asp:DropDownList ID="cmbStatus" runat="server" Width="150px" CssClass="form-control">
					<asp:ListItem Value="1" Selected="True">Active</asp:ListItem>
					<asp:ListItem Value="2">Inactive</asp:ListItem>
				</asp:DropDownList>
			</div>
		</div>

		<div class="row">
			<div class="col-md-2 text-right">Min Order UOM :</div>
			<div class="col-md-5">
				<asp:DropDownList ID="cmbMinOrderUom" runat="server" Width="200px" CssClass="form-control" ></asp:DropDownList> 
			</div> 
			<div class="col-md-2 text-right">Material Specs :</div>
			<div class="col-md-3"><asp:TextBox ID="txtMatSpecs" runat="server" Width="150px" Text="0" CssClass="form-control" ></asp:TextBox></div>
		</div>
       
		<div class="row">
			<div class="col-md-2 text-right">Production Lead Time :</div>
			<div class="col-md-5">
				<table>
					<tr>
						<td><asp:TextBox ID="txtProdLeadTimeDays" runat="server" Width="200px" CssClass="form-control" >0</asp:TextBox></td>
						<td><asp:TextBox ID="txtProdLeadTimeHrs" runat="server" Width="100px" CssClass="form-control" >00:00</asp:TextBox></td>
						<td>&nbsp;DD / HH:MM</td>
					</tr>
				</table>
			</div> 
			<div class="col-md-2 text-right">Roll Width :</div>
			<div class="col-md-3"><asp:TextBox ID="txtRollWidth" runat="server" Width="150px" Text="0" CssClass="form-control" ></asp:TextBox></div>
		</div>
        
		<div class="row">
			<div class="col-md-2 text-right">Release Lead Time :</div>
			<div class="col-md-5">
				<table>
					<tr>
						<td><asp:TextBox ID="txtReleaseLeadTimeDay" runat="server" Width="200px" CssClass="form-control" >0</asp:TextBox></td>
						<td><asp:TextBox ID="txtReleaseLeadTimeHrs" runat="server" Width="100px" CssClass="form-control" >00:00</asp:TextBox></td>
						<td>&nbsp;DD / HH:MM</td>
					</tr>
				</table>
			</div> 
			<div class="col-md-2 text-right">Repeat Lenght :</div>
			<div class="col-md-3"><asp:TextBox ID="txtRepeatLenght" runat="server" Width="150px" Text="0" CssClass="form-control" ></asp:TextBox></div>
		</div>
        <div class="row">
			<div class="col-md-2 text-right">Bag Dimension :</div>
			<div class="col-md-5"><asp:TextBox ID="txtBagDimension" runat="server" Width="" CssClass="form-control" Placeholder="Optional" ></asp:TextBox></div>
			<div class="col-md-2 text-right"></div>
			<div class="col-md-3"></div>
		</div>
		<div class="row">
			<div class="col-md-12">&nbsp;</div>
			<div class="col-md-2"></div>
			<div class="col-md-5">
				<asp:Button ID="cmdSave" CssClass="btn btn-primary" runat="server" Text="Save and Next" />
				<asp:Button ID="txtClose" CssClass="btn btn-primary" runat="server" Text="Close" />  
				<asp:Label ID="lblErrorMsg" runat="server" Text="Label" CssClass="labelL" Font-Bold="True" Font-Size="12px" ForeColor="#CC0000" Visible="False"></asp:Label>
			</div>
		</div>
        
		<div class="row">
			<div id="divErrorDis" runat="server" visible="false" style="width:99%; border: solid 2px #dc0606; background:#fb9d9d; color:#000000; padding-left:5px; text-align:left;">
				<table>
					<tr>
						<td style="width:25px;"><img src="images/alert.png" style="width:19px; height:24px;"></td>
						<td style="text-shadow: 1px 1px 1px #e7e7e7; font-weight:bold;"></td>
					</tr> 
				</table>
			</div>
		</div>
        

        
    </div>
    </form>
</body>
</html>


<script type="text/javascript">
        function invoke() {
            <%=vScript %>
        }

        function showSettings(vType) {
            id = vType;
            s = 0;
            linkwin = window.open('../global_forms/maintenance.aspx?pMode=NewWindow&id=' + id + "&s=" + s,
                'viewprop', 'top=80,left=100,scrollbars=yes,resizable=yes,toolbars=no,width=850,height=450');
            linkwin.focus();
        }
        function close_popup() {
            window.close();
        }
    </script>