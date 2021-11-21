<%@ Page Language="VB" AutoEventWireup="false" CodeFile="curing.aspx.vb" Inherits="inventory_curing" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    
    <title></title> 
	
    <link href="../css/calendar/jquery-ui-1.10.4.custom.min.css" rel="stylesheet" /> 
	<link href="../bootstrap/bootstrap/css/bootstrap.css" rel="stylesheet" />
	
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
    	.ZeroPadleft {
    		padding-left:0px; margin-left:0px;
		}
    	.Tbl {
    		border: solid 1px #CCC;
		}
        small { font-size:12px;
        } 
    </style> 

</head>
<body onload="invoke();">
    <form id="form1" runat="server" autocomplete="off"> 

    

    <div class="container-fluid" >
		<h3>Curing / Oven Process</h3>

        <div class="row" > 
            <div class="col-sm-3">
                <div class="col-md-12">
                    <small>Curing Date From:</small><br /> 
                    <div class="col-md-10 ZeroPadleft">
					    <asp:TextBox ID="txtDateFrom" runat="server" CssClass="form-control" placeholder="mm / dd / yyyy" ></asp:TextBox>  
				    </div>  
				    <img src="../images/calendar.png" class="DatePicker" style="vertical-align:middle;" alt="" />
                    
                </div>
                <div class="col-md-12">
                    <small>Curing Date To:</small><br /> 
                    <div class="col-md-10 ZeroPadleft">
					    <asp:TextBox ID="txtDateTo" runat="server" CssClass="form-control" placeholder="mm / dd / yyyy" ></asp:TextBox>  
				    </div>  
				    <img src="../images/calendar.png" class="DatePicker" style="vertical-align:middle;" alt="" />
                     
                </div>
            </div>
            <div class="col-sm-3">
                <div class="col-md-12">
                    <small>JO Number:</small><br /> 
                    <div class="col-md-10 ZeroPadleft">
					    <asp:TextBox ID="txtJONO" runat="server" CssClass="form-control" placeholder="Enter Batch JO Number" ></asp:TextBox>   
				    </div>   
                </div>
                
                <div class="col-md-12">
                    <div class="col-md-10 ZeroPadleft">
                        <small>Completion Status:</small>
                        <asp:DropDownList ID="DDLCompStatus" runat="server" Width="" CssClass="form-control form-control-sm"></asp:DropDownList>
                    </div>
                </div>
                <div class="col-md-12">
                    <div class="btn-group"> 
					    <input type="button" runat="server" id="btnScan" value="Create Transaction" class="btn btn-primary btn-sm" data-toggle="modal" data-target="#myModal" />
					    <asp:Button ID="btnSearch" CssClass="btn btn-primary btn-sm" runat="server"  Text="Search" />
				    </div>
                </div>
            </div> 

            
            <div class="col-sm-3"></div>
            <div class="col-sm-3"></div>
        </div>

        <input type="hidden" id="TxtItemKey" value="" runat="server" />
		<input type="hidden" id="h_ItemCd" name="h_ItemCd" runat="server" style="width:45px"/>     
		<input type="hidden" id="h_Mode" name="h_Mode" runat="server" style="width:45px"/>     
		<input type="hidden" id="h_Sql" name="h_Mode" runat="server" style="width:45px"/>
	  
		<div class="row Mgin">
			<div class="col-md-2 ZeroPadleft"></div>
			<div class="col-md-3 ZeroPadleft">
				
			</div>
		</div>
		<br /><br />
		<div class="row">
			
			<div class="col-md-12"> 
				<table id="tblCuringReport" class="table table-bordered" style="border:solid 1px #000; font-size:12px" border="1">
					<tr>
						<th colspan="7"></th> 
						<th colspan="2" style="background:#C3FCB8;">Curing IN</th>
						<th colspan="2" style="background:#CAC8FB;">Curing OUT</th> 
						<th colspan="3"></th>
					</tr>
					<tr>
						<th style="width:10px">#</th>
                        <th style="width:100px">JO Number</th>
						<th style="width:120px">Batch No.</th>
						<th style="width:100px">Tran Date</th> 
						<th style="width:120px">Item Code</th>
						<th>Item Name</th>
                        <th style="width:80px">Net WT</th>
						<th style="width:100px; background:#C3FCB8;">Date</th>
						<th style="width:100px; background:#C3FCB8;">Created by</th>
						<th style="width:100px; background:#CAC8FB;">Date</th>
						<th style="width:100px; background:#CAC8FB;">Created by</th>
						<th style="width:100px">Total Hour(s)</th>
						<th style="width:120px">Remarks</th>
                        <th style="width:60px"></th>
					</tr>
					<%= vData %>
				</table><br />
			</div>			
			
		</div>
		  
		</div>
		 
    
	<!-- Modal -->
	<div id="myModal" runat="server"  class="modal fade" role="dialog">
		<div class="modal-dialog modal-sm">

		<!-- Modal content-->
			<div class="modal-content">
				<div class="modal-header">
					<button type="button" class="close" data-dismiss="modal">&times;</button>
					<h3 class="modal-title">SFG Curing Logs</h3>
				</div>
				<div class="modal-body"> 

					<div class="row Mgin">
						<div class="col-md-12">Please scan the sticker’s barcode label :</div>
					</div>
					<div class="row Mgin">
						<div class="col-md-12">
							<asp:TextBox ID="txtSFGCode" runat="server" CssClass="form-control" ></asp:TextBox> 
						</div> 
						<div class="col-md-12" style="color:red; margin-top:10px; margin-bottom: 30px">
							NOTE: All items scanned from this module will be recorded in the monitoring logs as transaction.
						</div>
					</div>

					<div class="row Mgin">
						<div class="col-md-12">Remarks :</div>
					</div>
					<div class="row Mgin">
						<div class="col-md-12">
							<asp:TextBox ID="TxtRemarks" runat="server" CssClass="form-control" Rows="3" TextMode="MultiLine" ></asp:TextBox> 
						</div> 
						 
					</div>
				</div>
				<div class="modal-footer">
					<asp:Button ID="btnSave" CssClass="btn btn-primary btn-sm" runat="server"  Text="Save" /> 
					<button type="button" class="btn btn-primary btn-sm" data-dismiss="modal">Close</button>
				</div>
			</div>

		</div>
	</div>
    
    <div id="ModalComplete" runat="server"  class="modal fade" role="dialog">
		<div class="modal-dialog modal-sm">

		<!-- Modal content-->
			<div class="modal-content">
				<div class="modal-header">
					<button type="button" class="close" data-dismiss="modal">&times;</button>
					<h4 class="modal-title">Curing Complition</h4>
				</div>
				<div class="modal-body"> 

					 
					<div class="row Mgin"> 
						<div class="col-md-12" style="color:red;">
							Are you sure you want to tag this item as completed.
						</div>
					</div>

					 
				</div>
				<div class="modal-footer">
					<asp:Button ID="BtnComplete" CssClass="btn btn-primary btn-sm" runat="server" Text="YES" /> 
					<button type="button" class="btn btn-primary btn-sm" data-dismiss="modal">Cancel</button>
				</div>
			</div>

		</div>
	</div>
    </form>
</body>
</html>
 
<script src="../js/jquery-1.10.2.js"></script>
<script src="../js/jquery-ui.js"></script> 
<script src="../bootstrap/bootstrap/js/bootstrap.js"></script>

<script>
	function invoke() {
		<%=vScript %>
	}
	
	$(document).ready(function () {
		$(document).ready(function () {
			$("#txtDateFrom").datepicker(); 
			$("#txtDateTo").datepicker();
		});  

		$('#btnScan').click(function (event) { 
			$("#txtSFGCode").focus();
		});
		 
	});

	function openModal() {
		alert('test');
		//$('#myModal').modal('show');
	}
	 
    function SndParam(pKey) {
        //alert(pKey);
        document.getElementById("TxtItemKey").value = pKey;
    }

    function TblDeleteRow() { 
        alert("test1");
        //document.getElementById("tblCuringReport").deleteRow(2);
        //alert("test2");
    }

</script>