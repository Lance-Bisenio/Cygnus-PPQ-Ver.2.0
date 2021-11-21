<%@ Page Language="VB" AutoEventWireup="false" CodeFile="itemmaster.aspx.vb" Inherits="itemmaster" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    
    <title></title>
    <%--<link href="../css/BasicContol.css" rel="stylesheet" type="text/css" /> 
     <link href="../css/jquery_datatable.css" rel="stylesheet" />
    <link href="../css/colorbox.css" rel="stylesheet" />
    <link href="../css/jquery-ui.css" rel="stylesheet" />  --%>
	
	<link href="../bootstrap/bootstrap/css/bootstrap.css" rel="stylesheet" />

    <script src="../js/jquery-1.10.2.js" type="text/javascript"></script>
    <script src="../media/js/jquery.dataTables.js" type="text/javascript"> </script>
    <script src="../js/jquery.colorbox-min.js" type="text/javascript">  </script>

    <style type="text/css"> 

        .iDataFrame {
            width:99%; border: solid 0px #e2e2e2; height: 96%; margin:0px;
        }
    	div {
			border:0px solid #000; padding-top:1px;
    	}
    	body {
			font-family:Arial;
			font-size:12px;
    	}
    </style> 

    <script type="text/javascript">
		 
        function invoke() {
            <%=vScript %>
        }

        $(document).ready(function () {
            var vProperties = "width=1200px, height=550px, top=50px, left=80px, scrollbars=yes";
            var vParam = "&id=4000&pItemCd=" + $('#h_ItemCd').val() + "";
            var vDeleteParam = ""

            $('#btnItemAdd').click(function (event) {
                event.preventDefault();
                winPop = window.open("item_general.aspx?pMode=new" + vParam + "", "popupWindow", vProperties);
                winPop.focus();
            });

            $('#btnItemEdit').click(function (event) {
                event.preventDefault();
                winPop = window.open("item_general.aspx?pMode=edit" + vParam + "", "popupWindow", vProperties);
                winPop.focus();
            });
           
            $('#btnItemDel').click(function (event) { 
                vDeleteParam = "&pType=Delete_Item&pWinType=confirm&pTitle=Delete message alert&pMess=Item"
                vParam = "&id=4000&pTranId=" + $('#h_ItemCd').val() + "";

                event.preventDefault();
                winPop = window.open("../global_forms/confirm.aspx?mode=Delete_Item" + vParam + vDeleteParam + "",
                    "SmallWindow", "width=600px, height=300px, top=50px, left=80px, scrollbars=yes");
                winPop.focus();
            });
        });
        
    </script>

</head>
<body onload="invoke();">
    <form id="form1" runat="server">
    <%--#include file="filters.aspx"--%>
    <div class="container-fluid">
		<h3>Item Master</h3>

		<div class="row Mgin">
			<div class="col-md-1">Item Type :</div>
			<div class="col-md-3">
				<div class="col-md-10">
					<asp:DropDownList ID="cmbItemType" runat="server" CssClass="form-control"> 
					</asp:DropDownList> 
				</div> 
			</div>
			<div class="col-md-1">Item UOM :</div>
			<div class="col-md-3">
				<div class="col-md-10">
					<asp:DropDownList ID="cmbUOMQ" runat="server" CssClass="form-control"> 
					</asp:DropDownList> 
				</div> 
			</div> 
			<div class="col-md-1">Source :</div>
			<div class="col-md-3">
				<div class="col-md-10">
					<asp:DropDownList ID="cmbSource" runat="server" CssClass="form-control"> 
						<asp:ListItem Value="Buy" Selected="True">Buy</asp:ListItem>
						<asp:ListItem Value="Make">Make</asp:ListItem>
					</asp:DropDownList> 
				</div> 
			</div>
		</div>

		<div class="row Mgin">
			<div class="col-md-1">Sub Type :</div>
			<div class="col-md-3">
				<div class="col-md-10">
					<asp:DropDownList ID="cmbTypeClass" runat="server" CssClass="form-control"> 
					</asp:DropDownList> 
				</div> 
			</div>
			<div class="col-md-1">Status :</div>
			<div class="col-md-3">
				<div class="col-md-10">
					<asp:DropDownList ID="cmbItemStatus" runat="server" CssClass="form-control"> 
						<asp:ListItem Value="1" Selected="True">Active</asp:ListItem>
						<asp:ListItem Value="2">Inactive</asp:ListItem>
					</asp:DropDownList> 
				</div> 
			</div> 
			<div class="col-md-1"></div>
			<div class="col-md-3">
				<div class="col-md-10"> 
				</div> 
			</div>
		</div>

		<div class="row Mgin">
			<div class="col-md-1">Quick Search :</div>
			<div class="col-md-3">
				<div class="col-md-10">
					<asp:TextBox ID="txtSearch" runat="server" CssClass="form-control"></asp:TextBox> 
				</div> 
			</div>
			<div class="col-md-1">Search By :</div>
			<div class="col-md-3">
				<div class="col-md-10">
					<asp:DropDownList ID="cmbSearchBy" runat="server" CssClass="form-control"> 
					</asp:DropDownList> 
				</div> 
			</div> 
			<div class="col-md-1"> </div>
			<div class="col-md-3">
				<div class="col-md-10">
					<input type="hidden" id="h_EmpCode" runat="server" />
					<input type="hidden" id="Hidden1" runat="server" /> 
					 
					<input type="hidden" id="h_ItemCd" name="h_ItemCd" runat="server" style="width:45px"/>     
					<input type="hidden" id="h_Mode" name="h_Mode" runat="server" style="width:45px"/>     
					<input type="hidden" id="h_Sql" name="h_Mode" runat="server" style="width:45px"/>
				</div> 
			</div>
		</div>
		  
		<div class="row Mgin">
			<div class="col-md-1">Sub Type :</div>
			<div class="col-md-3">
				<div class="btn-group" style="margin-left:15px">
					<button type="button" runat="server" id="btnItemAdd" class="btn btn-primary btn-sm">Add</button>
					<button type="button" runat="server" id="btnItemEdit" class="btn btn-primary btn-sm">Edit</button>
					<button type="button" runat="server" id="btnItemDel" class="btn btn-primary btn-sm">Delete</button> 
					<asp:Button ID="txtSearch_1" CssClass="btn btn-primary btn-sm" runat="server"  Text="Search" />
				</div>
			</div>
			<div class="col-md-1">Page No. :</div>
			<div class="col-md-3">
				<div class="col-md-4">
					<asp:DropDownList ID="cmbShow" runat="server" AutoPostBack="True" CssClass="form-control">
					</asp:DropDownList> 
					 
				</div> 
				<asp:Label ID="lblTotal" runat="server" CssClass="labelL" Text="Total Item Retrieved : 0" ForeColor="#ffffff"></asp:Label>
			</div> 
			<div class="col-md-1"></div>
			<div class="col-md-3">
				<div class="col-md-10"> 
				</div> 
			</div>
		</div>
			
		<div class="row Mgin">
			<div class="col-md-12">
				<asp:GridView ID="tbl_ItemMaster" runat="server" AllowPaging="True"  
					AutoGenerateColumns="False" Width="99%"
					CssClass="table table-bordered" PageSize="10"> 
					<Columns>
                            
						<asp:CommandField ButtonType="Button" ShowSelectButton="True" > 
							<ItemStyle CssClass="" Width="40px" />
							<ControlStyle CssClass="btn btn-primary btn-xs" />
						</asp:CommandField>
						<asp:TemplateField HeaderText="#" HeaderStyle-Width="30px" >   
							<ItemTemplate>
								<%# Container.DataItemIndex + 1 %>   
							</ItemTemplate>
						<HeaderStyle Width="30px"></HeaderStyle>
						</asp:TemplateField>
                 
						<asp:TemplateField HeaderText="GCAS">
							<ItemTemplate>
								<asp:Label ID="Label1" runat="server" Text='<%# GetGCAS(Eval("Item_Cd"))%>'></asp:Label>
							</ItemTemplate>
							<ItemStyle CssClass="labelC" Width="90px" />
						</asp:TemplateField>

						<asp:BoundField DataField="Item_Cd" HeaderText="Item Code" >
							<ItemStyle CssClass="labelC" Width="90px" />
						</asp:BoundField>
                
						<asp:BoundField DataField="Descr" HeaderText="Item Description" >
							<ItemStyle CssClass="" />
						</asp:BoundField>
                             
						<%-- <asp:BoundField DataField="vCustomer" HeaderText="Customer" >
							<ItemStyle CssClass="labelL" />
						</asp:BoundField>
                            
						<asp:BoundField DataField="vSupplier" HeaderText="Supplier" >
							<ItemStyle CssClass="labelL" />
						</asp:BoundField>--%>
                            
						<asp:BoundField DataField="vItemType" HeaderText="Item Type" >
							<ItemStyle CssClass="labelL" Width="120px" />
						</asp:BoundField>
						<asp:BoundField DataField="vTypeClass" HeaderText="Type Class" >
							<ItemStyle CssClass="labelL" Width="120px" />
						</asp:BoundField>
						<asp:BoundField DataField="vUomWeight" HeaderText="UOM Qty" >
							<ItemStyle CssClass="labelL" Width="80px" />
						</asp:BoundField>
						<%-- <asp:BoundField DataField="vUomQty" HeaderText="UOM Weight" >
							<ItemStyle CssClass="labelL" Width="80px" />
						</asp:BoundField>--%>

						<asp:TemplateField HeaderText="Unit Cost">
							<ItemTemplate>
								<asp:Label ID="Label1" runat="server" Text='<%#Format(Val(Eval("CurrCost").ToString.Replace(",", "")), "###,###,##0.00") %>'></asp:Label>
							</ItemTemplate>
							<ItemStyle CssClass="labelR" Width="80px" />
						</asp:TemplateField>
						<asp:BoundField DataField="Source" HeaderText="Source" >
							<ItemStyle CssClass="labelL" Width="80px" />
						</asp:BoundField>
					</Columns>
            
					<SelectedRowStyle CssClass="activeBar" />
					<PagerStyle Font-Size="8pt" /> 
					<HeaderStyle CssClass="titleBar" />
					<RowStyle CssClass="odd" />
					<AlternatingRowStyle CssClass="even" />
				</asp:GridView>
			</div>
			
		</div>
		
 
        <%------------------------------------------------------------------------------------------------------------------------------------------------%>
       
        <%------------------------------------------------------------------------------------------------------------------------------------------------%>
    </div> 
        
    </form>
</body>
</html>
