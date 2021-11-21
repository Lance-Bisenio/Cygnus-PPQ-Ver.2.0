<%@ Page Language="VB" AutoEventWireup="false" CodeFile="production-cost.aspx.vb" Inherits="productioncost"%>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
        <link href="../bootstrap/bootstrap/css/bootstrap.css" rel="stylesheet" />
		<style>			
			body {
				font-family:Arial;
			}
			div,row {
				margin-top:1px;
			}
		</style>
</head>
<body onload="invoke();" >
    <form id="form1" runat="server">
        <div class="container-fluid">
            <input type="hidden" id="h_TranId" runat="server" name="h_TranId" />
            <input type="hidden" id="h_Mode" runat="server" name="h_Mode" />

			<div class="row">
				<div class="col-md-2">Select JONO:</div>
				<div class="col-md-10">
					<div class="col-md-3">
						<asp:DropDownList ID="cmdJOList" runat="server" CssClass="form-control pad" AutoPostBack="True"></asp:DropDownList>
					</div>
				</div>
			</div>
			<div class="row">
				<div class="col-md-2">Select Process:</div>
				<div class="col-md-10">
					<div class="col-md-3">
						<asp:DropDownList ID="cmdProcessList" runat="server" CssClass="form-control pad" AutoPostBack="True"></asp:DropDownList>
					</div>
				</div>
			</div>

			<div class="row">
				<div class="col-md-8">
					<h3>Process Cost Summary</h3>
					<table class="table table-bordered" border="0" style="width:100%; margin:auto; font-size:11px;"  > 
					<thead style="text-align:center;">
						<tr>
							<td>Batch Num</td> 
							<td>Core Weight</td>
							<td>Net Weight</td>
							<td>Gross Weight</td>
							<td>Meter</td> 
							<td>Cost</td> 
							<td>Total Cost</td> 
						</tr> 
					</thead>
					<%=vHeader %>
					<%=vData %>
				</table> 
				</div>
				<div class="col-md-4">
					<h3>Materials Running Cost Summary</h3>

					<table class="table table-bordered" border="0" style="width:100%; margin:auto; font-size:11px;"  > 
					<thead style="text-align:center;">
						<tr> 
							<td>Cost</td> 
							<td>Total Cost</td> 
							<td>Running Cost</td> 
							<td>Date Received</td> 
						</tr> 
					</thead> 
					<%=vMCostSummary %>
				</table> 
				</div>
			</div>
			 
            <div class="row"> 
                
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
                                            <input type="text" runat="server" class="form-control" id="txtPrvBatch" placeholder="Prev Batch">
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

    
</script>