<%@ Page Language="VB" AutoEventWireup="false" CodeFile="item_invlogs.aspx.vb" Inherits="item_invlogs"%>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title> 
    <link href="../css/bootstrap4/css/bootstrap.css" rel="stylesheet" />
</head>
<body onload="invoke();" >
    <form id="form1" runat="server">
        <div class="container-fluid">
            <input type="hidden" id="h_TranId" runat="server" name="h_TranId" />
            <input type="hidden" id="h_Mode" runat="server" name="h_Mode" />
			 
            <div class="row">
				<div class="col-sm-12"><h3>Job Order Number:&nbsp;&nbsp;<asp:Label ID="LblJONO" runat="server" ></asp:Label></h3></div>  
			</div>
            <hr /> 
			<div class="row">
				<div class="col-sm-2">Total Completion:</div>
				<div class="col-sm-4"><asp:Label ID="lblTtlCompCnt" runat="server" >0</asp:Label></div>
				<div class="col-sm-2">Total Waste:</div>
				<div class="col-sm-4"><asp:Label ID="lblTtlWasteCnt" runat="server" >0</asp:Label></div>
			</div>
			<div class="row">
				<div class="col-sm-2">Total Void:</div>
				<div class="col-sm-4"><asp:Label ID="lblTtlCompCntVoid" runat="server" >0</asp:Label></div>
				<div class="col-sm-2">Total Void:</div>
				<div class="col-sm-4"><asp:Label ID="lblTtlWasteCntVoid" runat="server" >0</asp:Label></div>
			</div>
			<br />
            <div class="row"> 
				<div class="col-sm-12">
					<table class="table table-bordered" border="0" style="font-size:11px;"  >
						<thead style="text-align:center;">
							<tr><td>#</td>
								
								<td>BTH NO</td>
								<td>Prev BTH NO</td>
								<td>Core Weight</td>
								<td>Net Weight</td>
								<td>Gross Weight</td>
								<td>Meter</td>
								<td>Qty</td>
								<td>Date Received</td>
                                <td>Received By</td>
							</tr> 
						</thead>
						<%=vHeader %>
						<%=vData %>
					</table>
				</div> 
            </div>
   
            <!-- Modal -->
            <div id="ModalEdit" class="modal fade" role="dialog">
                <div class="modal-dialog">
    
                    <!-- Modal content-->
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                            <h4 class="modal-title">Edit Completion</h4>
                        </div>
                        <div class="modal-body">
                            <div class="form-horizontal">
                                <div class="row">
                                    <div class="form-group">
                                        <label class="control-label col-sm-4" for="txtPrvBatch">Previous Batch:</label>
                                        <div class="col-sm-6">
                                            <input type="text" runat="server" class="form-control" id="txtPrvBatch" placeholder="Prev Batch" />
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="form-group">
                                        <label class="control-label col-sm-4" for="txtCore">Core:</label>
                                        <div class="col-sm-6">
                                            <input type="text" runat="server" class="form-control" id="txtCore" placeholder="Core">
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="form-group">
                                        <label class="control-label col-sm-4" for="txtMeter">Meter:</label>
                                        <div class="col-sm-6">
                                            <input type="text" runat="server" class="form-control" id="txtMeter" placeholder="Meter">
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="form-group">
                                        <label class="control-label col-sm-4" for="txtQty">QTY:</label>
                                        <div class="col-sm-6">
                                            <input type="text" runat="server" class="form-control" id="txtQty" placeholder="Enter email">
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <asp:Button ID="btnSubmit" runat="server" CssClass="btn btn-primary" Text="Submit" />
                            <button type="button" class="btn btn-primary" data-dismiss="modal">Close</button>
                        </div>
                    </div>
      
                </div>
            </div>
  
            <div id="ModalDel" class="modal fade bd-example-modal-sm" tabindex="-1" role="dialog" aria-labelledby="mySmallModalLabel" aria-hidden="true">
           <%-- <div class="modal fade bd-example-modal-sm" id="ModalDel" role="dialog">--%>
                <div class="modal-dialog modal-sm">
    
                    <!-- Modal content-->
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                            <h4 class="modal-title">Void Completion</h4>
                        </div>
                        <div class="modal-body">
                            Are you sure you want to void this transaction?
                        </div>
                        <div class="modal-footer">
                            <asp:Button ID="btnYes" runat="server" CssClass="btn btn-primary" Text="YES" />
                            <button type="button" class="btn btn-primary" data-dismiss="modal">NO</button>
                        </div>
                    </div>
      
                </div>
            </div>

			<div id="ModalCancel" class="modal fade bd-example-modal-sm" tabindex="-1" role="dialog" aria-labelledby="mySmallModalLabel" aria-hidden="true">
           <%-- <div class="modal fade bd-example-modal-sm" id="ModalDel" role="dialog">--%>
                <div class="modal-dialog modal-sm">
    
                    <!-- Modal content-->
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                            <h4 class="modal-title">Void Cancellation</h4>
                        </div>
                        <div class="modal-body">
                            Are you sure you want to include this transaction?
                        </div>
                        <div class="modal-footer">
                            <asp:Button ID="btnVoidCancel" runat="server" CssClass="btn btn-primary" Text="YES" />
                            <button type="button" class="btn btn-primary" data-dismiss="modal">NO</button>
                        </div>
                    </div>
      
                </div>
            </div>
        </div>
    </form>
</body>
</html>

<script src="../js/jquery-3.1.1.min.js"></script>
<script src="../bootstrap/bootstrap/js/bootstrap.min.js"></script>
<script>
    function invoke() {
        <%=vScript %>
    }

    $(document).ready(function () {

        //alert("lance");
        //$('#myModal').modal('toggle');
    
        //$('#myModal').modal('hide');
    });

    function ModalEdit(pId, pPrevBatch, pCore, pMeter, pQty) {
        //alert('lance x test' + pId);
        $('#txtPrvBatch').val(pPrevBatch);
        $('#txtCore').val(pCore);
        $('#txtMeter').val(pMeter);
        $('#txtQty').val(pQty);
        $('#h_TranId').val(pId); 
        //$('#ModalEdit').modal('show');
    }

    function ModelDelete(pId, pPrevBatch, pCore, pMeter, pQty) {
        //alert('lance test' + pId);
        $('#h_TranId').val(pId);
        $('#ModalDel').modal('show');
    }

	function ModalCancel(pId, pPrevBatch, pCore, pMeter, pQty) {
		//alert('lance test' + pId);
		$('#h_TranId').val(pId);
		$('#ModalCancel').modal('show');
	}
    
</script>