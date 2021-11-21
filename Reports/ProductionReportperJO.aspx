<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ProductionReportperJO.aspx.vb" Inherits="ProductionReportperJO" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>



    <link href="../css/bootstrap4/css/bootstrap.css" rel="stylesheet" />
    <link href="../css/calendar/jquery-ui-1.10.4.custom.min.css" rel="stylesheet" />

    <script src="../js/jquery-1.10.2.js"></script>
    <script src="../css/bootstrap4/js/bootstrap.js"></script>

    <script src="../js/jquery-ui-1.10.4.custom.min.js"></script>
    <script src="../js/jquery-ui.js"></script>

    <style>
        body {
            font-family: Arial;
            font-size:12px
        }
        div { border:0px solid #000
        }
        h6, h6 small {
            padding-bottom: -2px;
            margin-bottom: 0px;
        }
        h6 small {
            font-size:11px;
        }
        .divPad {
            margin-bottom: 5px
        }
    </style>
</head>
<body onload="invoke();">
    <form id="form1" runat="server" autocomplete="off">
        <div class="container-fluid">
            <input type="hidden" id="h_TranId" runat="server" name="h_TranId" />
            <input type="hidden" id="h_Mode" runat="server" name="h_Mode" />
            <h3>Job Order Summary Report</h3>

            <div class="row divPad">
                <div class="col-md-3"> 
                    <h6><small class="text-muted">Job Order Release Date:</small></h6>
                    <div class="form-inline">
                        <div class="form-group">
                            <asp:TextBox ID="txtDateFrom" runat="server" CssClass="form-control form-control-sm col-4" ></asp:TextBox>&nbsp;To:
                            <asp:TextBox ID="txtDateTo" runat="server" CssClass="form-control form-control-sm pad col-4"></asp:TextBox>&nbsp;
                            <button type="submit" id="btnSearchJoList" runat="server" class="btn btn-sm btn-primary">Search</button>
                        </div>
                    </div>
                </div>

                <div class="col-md-3"> 
                    <h6><small class="text-muted">Quick search Job Order Number:</small></h6>
                    <div class="form-inline">
                        <div class="form-group">
                            <asp:TextBox ID="TxtJONO" runat="server" CssClass="form-control form-control-sm" placeholder="Enter Job order number" ></asp:TextBox>&nbsp;
                            <button type="submit" id="BtnQSearch" runat="server" class="btn btn-sm btn-primary">Search</button>
                        </div>
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col-md-3">
                    <h6><small class="text-muted">Select Job Order Number:</small></h6>
                    <asp:DropDownList ID="cmdJOList" runat="server" AutoPostBack="true" CssClass="form-control form-control-sm pad col-12"></asp:DropDownList>
                </div>
            </div>
            <div class="row">
                <div class="col-md-3">
                    <h6><small class="text-muted">Select Process to view:</small></h6>
                    <asp:DropDownList ID="cmdProcessList" runat="server" AutoPostBack="true" CssClass="form-control form-control-sm pad col-12"></asp:DropDownList>
                </div>
            </div>

            <br />
            <hr />
            <div class="row">
                <div class="col-md-4">
                    <h6><small class="text-muted">Job Order Number :</small></h6>
                    <h6><asp:Label ID="lblJONO" runat="server" Text="-"></asp:Label></h6>
                </div>
                <div class="col-md-4">
                    <h6><small class="text-muted">Item Code / GCAS :</small></h6>
                    <h6><asp:Label ID="lblItemCode" runat="server" Text="-"></asp:Label></h6>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <h6><small class="text-muted">Customer Code:</small></h6>
                    <h6><asp:Label ID="lblCustomerDescr" runat="server" Text="-"></asp:Label></h6>
                </div>
                <div class="col-md-4">
                    <h6><small class="text-muted">Item Description:</small></h6>
                    <h6><asp:Label ID="lblItemDescr" runat="server" Text="-"></asp:Label></h6>
                </div>
            </div>
            
            <div class="row">
                <div class="col-md-4">
                    <h6><small class="text-muted">Customer Name:</small></h6>
                    <h6><asp:Label ID="lblCustomer" runat="server" Text="-"></asp:Label></h6>
                </div>
            </div>
            <hr />
            <br />
            <div class="row">
                <div class="col-md-12"> 
                    <%=vProcess %> 
                </div>
                <%--<div class="col-md-7">
                    <h5><%--Materials Running Cost Summary- -%></h5>

                    <table class="table table-bordered" border="0" style="width: 100%; margin: auto; font-size: 11px;">
                        <thead style="text-align: center;">
                            <tr>
                                <td>OUTPUT</td> 
                            </tr>
                            <tr>
                                <td>Cost</td>
                                <td>Total Cost</td>
                                <td>Running Cost</td>
                                <td>Date Received</td>
                            </tr>
                        </thead>
                        <%=vMCostSummary %>
                    </table>
                </div>--%>
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

<script>
    function invoke() {
        <%=vScript %>
    }

    $(document).ready(function () {
        $("#txtDateFrom").datepicker();
        $("#txtDateTo").datepicker();
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
