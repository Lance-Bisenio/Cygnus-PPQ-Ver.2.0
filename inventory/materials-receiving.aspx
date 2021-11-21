<%@ Page Language="VB" AutoEventWireup="false" CodeFile="materials-receiving.aspx.vb" Inherits="inventory_taskmaterials" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>


    <%-- <link href="../css/bootstrap4/css/bootstrap.css" rel="stylesheet" />
    <script src="../js/jquery-1.10.2.js"></script>
    <script src="../css/bootstrap4/js/bootstrap.js"></script>--%>

    <link href="../bootstrap/bootstrap-4.0.0/css/bootstrap.min.css" rel="stylesheet" />
    <script src="../js/jquery-v3.5.1.js"></script>
    <script src="../bootstrap/bootstrap-4.0.0/js/bootstrap.min.js"></script>

    <style>
        body {
            font-family: Calibri, Arial, 'Trebuchet MS', sans-serif;
            font-size: 12px;
        }
    </style>
</head>
<body onload="invoke();">
    <form id="form1" runat="server">
        <div class="container-fluid">
            <br />
            <div class="row">
                <div class="col-sm-12">
                    <h3>Production Raw Material Receiving</h3>
                </div>
            </div>
            <hr />
            <div class="row">
                <div class="col-sm-6">
                    <div class="form-inline">
                        <label class="sr-only" for="txtCode">Barcode Scanner</label>
                        <input type="text" id="txtCode" runat="server" class="form-control form-control-sm col-6" placeholder="Barcode Scanner" />
                        &nbsp;
						<button type="submit" id="btnScan" runat="server" class="btn btn-sm btn-primary">Scan</button>&nbsp;<br />
                    </div>
                    <h6>
                        <asp:Label ID="lblScanLabel" runat="server" Text="Label"></asp:Label></h6>
                </div>
                <div class="col-sm-6">
                    <div class="form-inline">
                        <asp:DropDownList ID="cmdProcess" runat="server" CssClass="form-control form-control-sm col-6" Height="32px"></asp:DropDownList>&nbsp;
                        <asp:Button ID="cmdViewSFG" runat="server" CssClass="btn btn-sm btn-primary" Text="View all SFG" Visible="true"></asp:Button> 
                    </div>

                </div>
            </div>
            <hr />

            <div class="row">
                <div class="col-sm-6">
                    <div class="btn-group btn-group-sm">
                        <asp:Button ID="cmdSave" runat="server" CssClass="btn btn-primary" Text="Save Qty Receive" Visible="false"></asp:Button>
                        <asp:Button ID="cmdViewPending" runat="server" CssClass="btn btn-primary" Text="View all Pending" Visible="false"></asp:Button>
                        <asp:Button ID="cmdViewAll" runat="server" CssClass="btn btn-primary" Text="View all Materials" Visible="false"></asp:Button>
                        <asp:Button ID="cmdCancel" runat="server" CssClass="btn btn-danger btn-sm" Text="Close" OnClientClick="self.close()" Visible="false"></asp:Button>
                    </div>
                </div>

                <div class="col-sm-6">
                    
                </div>
            </div>
            &nbsp;

            <div class="row">
                <div class="col-sm-12">
                    <table class="table table-bordered table-striped">
                        <%=vHeader %>
                        <%=vData %>
                    </table>
                </div>



            </div>
        </div>


        <input type="hidden" id="h_Action" value="" runat="server" />

        <br />
        <br />

        <div class="modal fade" id="myModal" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title">SFG Raw-materials</h5>
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                    </div>
                    <div class="modal-body">
                        <h4 class="text-info">Use the selected Semi-Finished Good as part of the process? </h4>
                        <br />
                        <h5>If Yes, click the submit button.</h5>
                        <input type="hidden" id="h_TranId" class="form-control" runat="server">
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="cmdGrabRawMats" runat="server" Text="Submit" CssClass="btn btn-primary" />
                        <button type="button" class="btn btn-default btn-danger" data-dismiss="modal">Cancel</button>
                    </div>
                </div>
            </div>
        </div>

        <%--<div id="myModal" class="modal fade" role="dialog">
            <div class="modal-dialog modal-sm">

                <!-- Modal content-->
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h4 class="modal-title">SFG Raw-materials</h4>
                    </div>
                    <div class="modal-body">
                        <h4>Use the selected SFG as part of the process?</h4>
                        <input type="hidden" id="h_TranId" class="form-control" runat="server">
                    </div>
                    <div class="modal-footer">
                        
                    </div>
                </div>
            </div>
        </div>--%>

        <div id="ModelDelete" class="modal fade" role="dialog">
            <div class="modal-dialog">

                <!-- Modal content-->
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title">SFG Raw-materials</h5>
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                    </div>
                    <div class="modal-body">
                        <h4 class="text-danger">Are you sure you want to remove the selected Semi-Finished Good?</h4>
                        <br />
                        <h5>If Yes, click the submit button.</h5>
                        <input type="hidden" id="Hidden1" class="form-control" runat="server">
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="cmdDelRawMats" runat="server" Text="Submit" CssClass="btn btn-primary" />
                        <button type="button" class="btn btn-default btn-danger" data-dismiss="modal">Cancel</button>
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

    });

    function UsedRawmats(pId, pPrevBatch, pCore, pMeter, pQty) {
        //$('#txtPrvBatch').val(pPrevBatch);
        //$('#txtCore').val(pCore);
        //$('#txtMeter').val(pMeter);
        //$('#txtQty').val(pQty);
        $('#h_TranId').val(pId);
    }

    function RemoveRawmats(pBatchNo) {
        $('#h_TranId').val(pBatchNo);
    }


</script>
