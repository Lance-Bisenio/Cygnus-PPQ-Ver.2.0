<%@ Page Language="VB" Debug="true" AutoEventWireup="false" EnableEventValidation="false" CodeFile="taskdetails.aspx.vb" Inherits="inventory_taskdetails" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>

    <link href="../css/bootstrap4/css/bootstrap.css" rel="stylesheet" />
    <script src="../js/jquery-ui-1.12.1/external/jquery/jquery.js"></script>
    <%--<script src="../js/jquery-1.10.2.js"></script>--%>
    <script src="../css/bootstrap4/js/bootstrap.js"></script>

    <script>
        function invoke() {
            <%=vScript %>
        }

        $(document).ready(function () {

            //var vProperties = "width=1200px, height=700px, top=50px, left=80px, scrollbars=yes";
            var vProperties = "width=450px, height=700px, top=10, left=100, scrollbars=yes";
            var vParam = "&pTranId=" + $('#lblJO').text() + "&pBom=" + $('#lblBOM').text() +
                "&pBomRev=" + $('#lblBOMRev').text() + "";

            $('#btnRelease').click(function (event) {
                event.preventDefault();
                winPop = window.open("materials-releasing.aspx?pMode=WarehouseRelease" + vParam + "", "popupWindow", vProperties);
                winPop.focus();
            });


            $('#btnPrint').click(function (event) {
                vParam = "&pTranId=" + $('#h_TranId').val() + "&pBOM=" + $('#lblBOM').text() + "&pBOMRev=" + $('#lblBOMRev').text() + "&pJO=" + $('#lblJO').text() + ""

                event.preventDefault();
                winPop = window.open("taskdetails.aspx?pMode=Print" + vParam + "", "popupWindow", vProperties);
                winPop.focus();
            });
        });

        function EditItem(vTranId, pOrderNo, pItemCode, pQty, pLot, pRollNo, pBatchNo, pIsAltItem) {
            var vProperties = "width=450px, height=700px, top=10, left=100, scrollbars=yes";
            var vParam = "&pOrderNo=" + pOrderNo +
                "&pBom=" + $('#lblBOM').text() +
                "&pBomRev=" + $('#lblBOMRev').text() +
                "&pJO=" + $('#lblJO').text() +
                "&pItemCode=" + pItemCode +
                "&pQty=" + pQty +
                "&pBatchNo=" + pBatchNo +
                "&pLot=" + pLot +
                "&pRollNo=" + pRollNo +
                "&pIsAltItem=" + pIsAltItem + "";

            winPop = window.open("materials-releasing.aspx?pMode=EditItem&pTranId=" + vTranId + vParam, "popupWindow", vProperties);
            winPop.focus();
        }

        function RecRawMats(vTranId, vType) {

            $('#h_TrnIdRecRM').val(vTranId);
            $('#h_TrnIdRecRMType').val(vType);

            if (vType == "Cancel") {
                $('#lblConfirmLabel').text("Are you sure you want to cancel this item?");
            } else {
                $('#lblConfirmLabel').text("Are you sure you want to accept the selected item?");
            }
        }

        function TagAsComplete(vTranId, vType) {
            $('#h_TrnReqID').val(vTranId);
            $('#h_TrnReqType').val(vType);

            if (vType == "Cancel") {
                $('#LblComplete').text("Are you sure you want to cancel this item?");

            } else {
                $('#LblComplete').text("Are you sure you want to tag this item as complete?");
            }
        }

        function PrintLabel(pType, pTranId, pJO, pBatchNo) {

            if (pType == "CancelPerItem") {
                $("#lblRemarks").fadeIn();
            } else {
                $("#txtHint").fadeIn();
            }

            var xmlhttp = new XMLHttpRequest();

            xmlhttp.onreadystatechange = function () {

                if (this.readyState == 4 && this.status == 200) {

                    if (pType == "CancelPerItem") {
                        document.getElementById("lblRemarks").innerHTML = this.responseText;
                        $("#" + pTranId + "x").hide();
                        $("#lblRemarks").fadeOut(800);
                    } else {
                        document.getElementById("txtHint").innerHTML = this.responseText;
                        $("#txtHint").fadeOut(800);
                    }
                }
            };

            xmlhttp.open("POST", "taskdetailsXML.aspx?pType=" + pType + "&pTranId=" + pTranId + "&pJO=" + pJO + "&pBatchNo=" + pBatchNo, false);
            xmlhttp.setRequestHeader("Content-type", "application/x-www-form-urlencoded");
            xmlhttp.send();

        }

    </script>

    <style>
        body {
            font-family: Calibri, Arial, 'Trebuchet MS', sans-serif;
            font-size: 12px;
        }

        h6, h6 small {
            padding-bottom: -5px;
            margin-bottom: -5px
        }
    </style>
</head>
<body onload="invoke();">
    <form id="form1" runat="server">
        <input type="hidden" runat="server" id="h_TranId" name="h_TranId" value="" />
        <input type="hidden" runat="server" id="h_Mode" name="h_Mode" value="" />
        <input type="hidden" runat="server" id="h_TotalBal" name="h_TotalBal" value="" />

        <input type="hidden" runat="server" id="h_TrnIdRecRM" name="h_TrnIdRecRM" value="" />
        <input type="hidden" runat="server" id="h_TrnIdRecRMType" name="h_TrnIdRecRM" value="" />
        <input type="hidden" runat="server" id="h_TrnReqID" name="h_TrnIdRecRM" value="" />
        <input type="hidden" runat="server" id="h_TrnReqType" name="h_TrnReqType" value="" />
        <div class="container-fluid">

            <div class="row">
                <div class="col-md-4">
                    <div class="col-md-12">
                        <h6><small class="text-muted">Item Code | Product Code or GCAS</small></h6>
                        <h5>
                            <small>
                                <asp:Label ID="lblItemCode" runat="server" CssClass="text-primary" Text="Label"></asp:Label>&nbsp;|
                                <asp:Label ID="lblGCAS" runat="server" CssClass="labelL" Text="Label"></asp:Label>
                            </small>
                        </h5>
                    </div>
                    <div class="col-md-12">
                        <h6><small class="text-muted">Item Description</small></h6>
                        <h5>
                            <small>
                                <asp:Label ID="lblItemDescr" runat="server" CssClass="labelL" Text="Label"></asp:Label>
                            </small>
                        </h5>
                    </div>
                    <div class="col-md-12">
                        <h6><small class="text-muted">Customer Details</small></h6>
                        <h5>
                            <small>
                                <asp:Label ID="lblCust_Cd" runat="server" Text="Label"></asp:Label>&nbsp;|
                                <asp:Label ID="lblCustDescr" runat="server" Text="Label"></asp:Label>
                            </small>
                        </h5>
                    </div>
                </div>
                <div class="col-md-2">
                    <div class="col-md-12">
                        <h6><small class="text-muted">Job Order Number</small></h6>
                        <h5>
                            <small>
                                <asp:Label ID="lblJO" runat="server" CssClass="text-primary" Text="Label"></asp:Label>
                            </small>
                        </h5>
                    </div>
                    <div class="col-md-12">
                        <h6><small class="text-muted">Sale Order Number</small></h6>
                        <h5>
                            <small>
                                <asp:Label ID="lblSO" runat="server" CssClass="labelL" Text="Label"></asp:Label>
                            </small>
                        </h5>
                    </div>
                    <div class="col-md-12">
                        <h6><small class="text-muted">Purchase Order Number</small></h6>
                        <h5>
                            <small>
                                <asp:Label ID="lblPO" runat="server" CssClass="labelL" Text="Label"></asp:Label>
                            </small>
                        </h5>
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="col-md-12">
                        <h6><small class="text-muted">BOM Code | Revision</small></h6>
                        <h5>
                            <small>
                                <asp:Label ID="lblBOM" runat="server" CssClass="labelL" Text="Label"></asp:Label>&nbsp;|
                                <asp:Label ID="lblBOMRev" runat="server" CssClass="labelL" Text="Label"></asp:Label>
                            </small>
                        </h5>
                    </div>
                    <div class="col-md-12">
                        <h6><small class="text-muted">Production Start Date</small></h6>
                        <h5>
                            <small>
                                <asp:Label ID="lblProdDate" runat="server" CssClass="labelL" Text="Label"></asp:Label>
                            </small>
                        </h5>
                    </div>
                    <div class="col-md-12">
                        <h6><small class="text-muted">Qty Order</small></h6>
                        <h5>
                            <small>
                                <asp:Label ID="lblQtyOrder" runat="server" CssClass="labelL" Text="Label"></asp:Label>
                            </small>
                        </h5>
                    </div>
                </div>
            </div>

            <br />

            <div class="row">
                <div class="col-sm-3">
                    <div class="col-sm-6">
                        <div class="btn-group btn-group-sm">
                            <asp:Button ID="btnBack" runat="server" Text="Back" CssClass="btn btn-primary" />
                            <input type="button" runat="server" id="btnRelease" name="btnRelease" value="Dispatch Materials" class="btn btn-primary" />
                            <%--<asp:Button ID="btnReload" runat="server" Text="Reload" CssClass="btn btn-primary" />--%>
                            <input type="button" runat="server" id="btnPrint" name="btnPrint" value="Print" class="btn btn-primary" />
                        </div>
                    </div>
                </div>

                <div class="col-sm-5">
                    <div class="col-sm-12">
                        <div class="btn-group btn-group-sm">
                            <asp:Button ID="BtnTabRel" runat="server" Text="JO Raw Material" CssClass="btn btn-success btn-sm"></asp:Button>

                            <button type="button" id="BtnTabReq" runat="server" class="btn btn-primary btn-sm">
                                Request <%--<span class="badge badge-light">4</span>--%>
                            </button>

                            <button type="button" id="BtnTabRet" runat="server" class="btn btn-primary btn-sm">
                                Return 
                                <span class="badge badge-light">
                                    <asp:Label ID="RetCount" runat="server" Text="0"></asp:Label>
                                </span>
                            </button>
                            <asp:Button ID="BtnPrintingLabel" runat="server" Text="Printing Label Details" CssClass="btn btn-primary btn-sm"></asp:Button>
                        </div>
                    </div>
                </div>
                <div class="col-sm-4 text-right">
                    <div class="col-sm-12">
                        <h6 class="text-right"><span id="txtHint" class="text-success text-right"></span></h6>
                    </div>
                </div>
            </div>

            <br />

            <div class="row">
                <div class="col-sm-12">
                    <div class="col-sm-12">
                        <table class="table table-bordered table-striped">
                            <%=vHeader %>
                            <%=vData %>
                        </table>
                    </div>
                </div>
            </div>
        </div>

        <br />
        <br />

        <div class="modal" id="myModal">
            <div class="modal-dialog">
                <div class="modal-content">

                    <!-- Modal Header -->
                    <div class="modal-header">
                        <h4 class="modal-title text-primary">
                            <asp:Label ID="lblConfirmLabel" runat="server" Text=""></asp:Label></h4>
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                    </div>

                    <!-- Modal footer -->
                    <div class="modal-footer">

                        <asp:Button ID="BtnAcceptRawmats" class="btn btn-primary btn-sm" runat="server" Text="Save" />
                        <button type="button" class="btn btn-danger btn-sm" data-dismiss="modal">Close</button>
                    </div>

                </div>
            </div>
        </div>

        <div class="modal" id="ModalTagAsCompete">
            <div class="modal-dialog">
                <div class="modal-content">

                    <!-- Modal Header -->
                    <div class="modal-header">
                        <h4 class="modal-title text-primary">
                            <asp:Label ID="LblComplete" runat="server" Text=""></asp:Label></h4>
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                    </div>


                    <!-- Modal footer -->
                    <div class="modal-footer">
                        <asp:Button ID="BtnSaveItemRequest" class="btn btn-primary btn-sm" runat="server" Text="YES" />
                        <button type="button" class="btn btn-danger btn-sm" data-dismiss="modal">Close</button>
                    </div>

                </div>
            </div>
        </div>

        <div class="modal" id="PrintingListModal">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">

                    <!-- Modal Header -->
                    <div class="modal-header">
                        <h4 class="modal-title text-primary">
                            <asp:Label ID="Label1" runat="server" Text="">Printing Label Details</asp:Label></h4>
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                    </div>
                    <div class="modal-body">
                        <div class="row">
                            <div class="col-12">
                                <h6 class="text-right"><span id="lblRemarks" class="text-success text-right"></span>&nbsp;</h6> 
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-12">
                                <table class="table table-bordered">
                                    <thead>
                                        <tr class="bg-info text-white">
                                            <th>Ref ID</th>
                                            <th>Material Code</th>
                                            <th>Qty Release</th>
                                            <th>LOTNO</th>
                                            <th>Roll No</th>
                                            <th style="width: 50px"></th>
                                        </tr>
                                    </thead>
                                    <%=vDataPrintingLabel %>
                                </table>
                            </div>
                        </div>


                    </div>

                    <!-- Modal footer -->
                    <div class="modal-footer">
                        <asp:Button ID="Button1" class="btn btn-primary btn-sm" runat="server" Text="Cancel All" />
                        <button type="button" class="btn btn-danger btn-sm" data-dismiss="modal">Close</button>
                    </div>

                </div>
            </div>
        </div>
    </form>
</body>
</html>
