<%@ Page Language="VB" AutoEventWireup="false" CodeFile="joborder-completion.aspx.vb" Inherits="inventory_jocomplition" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <%--<link href="../bootstrap/bootstrap/css/bootstrap.css" rel="stylesheet" />--%>
    <link href="../css/bootstrap4/css/bootstrap.min.css" rel="stylesheet" />
</head>
<body onload="invoke();">
    <form id="form1" runat="server">
        <div class="container-fluid">
            <input type="hidden" id="h_TranId" runat="server" name="h_TranId" />
            <input type="hidden" id="h_Mode" runat="server" name="h_Mode" />

            <div class="row">
                <div class="col-sm-6 text-primary">
                    <h3>Job Order Number:&nbsp;&nbsp;<asp:Label ID="LblJONO" runat="server"></asp:Label></h3>
                </div>
                <div class="col-sm-6">
                    <div class="row">
                        <div class="col-sm-3 text-success">
                            <small>Total Completion:</small>
                            <h5>
                                <asp:Label ID="lblTtlCompCnt" runat="server">0</asp:Label></h5>
                        </div>
                        <div class="col-sm-3 text-danger">
                            <small>Total Void:</small>
                            <h5>
                                <asp:Label ID="lblTtlCompCntVoid" runat="server">0</asp:Label></h5>
                        </div>

                        <div class="col-sm-3 text-success">
                            <small>Total Waste:</small>
                            <h5>
                                <asp:Label ID="lblTtlWasteCnt" runat="server">0</asp:Label></h5>
                        </div>

                        <div class="col-sm-3 text-danger">
                            <small>Total Void:</small>
                            <h5>
                                <asp:Label ID="lblTtlWasteCntVoid" runat="server">0</asp:Label></h5>
                        </div>
                    </div>
                </div>
            </div>
            <%--<hr /> 
			<div class="row">
				<div class="col-sm-2">Total Completion:</div>
				<div class="col-sm-4"></div>
				<div class="col-sm-2">Total Waste:</div>
				<div class="col-sm-4"></div>
			</div>
			<div class="row">
				<div class="col-sm-2"></div>
				<div class="col-sm-4"></div>
				<div class="col-sm-2">Total Void:</div>
				<div class="col-sm-4"></div>
			</div>--%>
            <br />
            <div class="row">
                <div class="col-sm-12">
                    <table class="table table-bordered" border="0" style="font-size: 11px;">
                        <thead>
                            <tr>
                                <td colspan="15">
                                    <h4 class="text-info">COMPLETION</h4>
                                </td>
                            </tr>
                            <tr class="text-info" style="text-align: center;">
                                <td>#</td>
                                <td>JONO</td>
                                <td>BTH NO</td>
                                <td>Prev BTH NO</td>
                                <td>Core Weight</td>
                                <td>Net Weight</td>
                                <td>Gross Weight</td>
                                <td>Meter</td>
                                <td>Qty</td>
                                <td>(Bagmaking)
                                    <br />
                                    TotalPcs/Box </td>
                                <%-- <td>Unit Cost</td>
								<td title="Total Cost=Net Weight*Unit Cost">Total Cost</td>--%>
                                <td style="width: 100px">Created By</td>
                                <td style="width: 100px">Edited By</td>
                                <td style="width: 100px">Void By</td>
                                <td style="width: 100px"></td>
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

                        </div>
                        <div class="modal-body">
                            <div class="form-horizontal">
                                <h4 class="modal-title">Edit Completion</h4>
                                <div class="row">
                                    <div class="col-sm-12">
                                        <small>Previous Batch:</small>
                                        <input type="text" runat="server" class="form-control" id="txtPrvBatch" placeholder="Prev Batch" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-12">
                                        <small>Core:</small>
                                        <input type="text" runat="server" class="form-control col-6" id="txtCore" placeholder="Core">
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-12">
                                        <small>Meter:</small>
                                        <input type="text" runat="server" class="form-control col-6" id="txtMeter" placeholder="Meter">
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-12">
                                        <small>QTY:</small>
                                        <input type="text" runat="server" class="form-control col-6" id="txtQty" placeholder="Enter email">
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <asp:Button ID="btnSubmit" runat="server" CssClass="btn btn-sm btn-primary" Text="Submit" />
                            <button type="button" class="btn btn-sm btn-primary" data-dismiss="modal">Close</button>
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

                        </div>
                        <div class="modal-body">
                            <h4>Void Completion</h4>
                            Are you sure you want to void this transaction?
                        </div>
                        <div class="modal-footer">
                            <asp:Button ID="btnYes" runat="server" CssClass="btn btn-sm btn-primary" Text="YES" />
                            <button type="button" class="btn btn-sm btn-primary" data-dismiss="modal">NO</button>
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
                            <asp:Button ID="btnVoidCancel" runat="server" CssClass="btn btn-sm btn-primary" Text="YES" />
                            <button type="button" class="btn btn-sm btn-primary" data-dismiss="modal">NO</button>
                        </div>
                    </div>

                </div>
            </div>
        </div>
    </form>
</body>
</html>

<script src="../js/jquery-3.1.1.min.js"></script>
<%--<script src="../bootstrap/bootstrap/js/bootstrap.min.js"></script>--%>
<script src="../css/bootstrap4/js/bootstrap.min.js"></script>
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
        //alert(pPrevBatch);

        $('#txtPrvBatch').val("");
        $('#txtCore').val("");
        $('#txtMeter').val("");
        $('#txtQty').val("");

        $('#txtPrvBatch').val(pPrevBatch);
        $('#txtCore').val(pCore);
        $('#txtMeter').val(pMeter);
        $('#txtQty').val(pQty);
        $('#h_TranId').val(pId); 

        //$('#ModalEdit').modal('show');
    }

    function ModelDelete(pId, pPrevBatch, pCore, pMeter, pQty) {
        $('#txtPrvBatch').val("");
        $('#txtCore').val("");
        $('#txtMeter').val("");
        $('#txtQty').val("");

        $('#h_TranId').val(pId);
        $('#ModalDel').modal('show');
    }

	function ModalCancel(pId, pCore, pMeter, pQty) {
        $('#txtPrvBatch').val("");
        $('#txtCore').val("");
        $('#txtMeter').val("");
        $('#txtQty').val("");


		$('#h_TranId').val(pId);
		$('#ModalCancel').modal('show');
	}

    function ForRewind(pId,pLastVal,pJO) {
        var xmlhttp = new XMLHttpRequest();

        if (pLastVal == "Cancel") {
            $('#' + pId).attr("value", "For Rewind");
            $('#' + pId).attr("class", "btn btn-sm btn-primary");
            xmlhttp.open("GET", "RewindXML.aspx?pModule=Completion&pMode=Del&pJO=" + pJO + "&pId=" + pId, true);
        } else {
            $('#' + pId).attr("value", "Cancel");
            $('#' + pId).attr("class", "btn btn-sm btn-danger");
            xmlhttp.open("GET", "RewindXML.aspx?pModule=Completion&pMode=Add&pJO=" + pJO + "&pId=" + pId, true);
        }

        xmlhttp.send();     
    }
    
</script>
