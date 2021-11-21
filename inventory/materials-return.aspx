<%@ Page Language="VB" AutoEventWireup="false" CodeFile="materials-return.aspx.vb" Inherits="materials_return" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <%--<script src="../js/jquery-ui-1.10.4.custom.js"></script>
    <script src="../js/jquery-ui-1.10.4.custom.min.js"></script>
    <script src="../js/jquery-1.10.2.js"></script>
    <script src="../js/jquery-ui.js"></script> 
	<link href="../bootstrap/bootstrap/css/bootstrap.min.css" rel="stylesheet" />--%>

    <link href="../css/bootstrap4/css/bootstrap.css" rel="stylesheet" />
    <script src="../js/jquery-1.10.2.js"></script>
    <script src="../css/bootstrap4/js/bootstrap.js"></script>

    <script>
        function invoke() {
            <%=vScript %>
        }

        function CancelItem(pReturnId) {
            //alert(pReturnId);

            $('#myModal').modal('show');
            $('#h_ReturnId').val(pReturnId); 
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
        <input type="hidden" id="h_Action" runat="server" />
        <input type="hidden" id="h_ReturnId" runat="server" />

        <div class="container-fluid">
            <div class="row">
                <div class="col-sm-12">
                    <h3>Production Raw Material Return</h3>
                    <div class="row">
                        <div class="col-md-4">
                            <h6><small class="text-muted">Job Order Number:</small></h6>
                            <h5>
                                <small>
                                    <h5><asp:Label ID="LblJONO" runat="server" Text="Label"></asp:Label></h5>
                                </small>
                            </h5>
                        </div>
                        <div class="col-md-4">
                            <h6><small class="text-muted">Process:</small></h6>
                            <h5>
                                <small>
                                    <h5><asp:Label ID="LblProcess" runat="server" Text="Label"></asp:Label></h5>
                                </small>
                            </h5>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-12">
                    <div class="btn-group btn-group-sm">
                        <asp:Button ID="cmdSave" runat="server" CssClass="btn btn-primary" Text="Save Return Qty" Visible="false"></asp:Button>
                        <asp:Button ID="cmdCancel" runat="server" CssClass="btn btn-danger" Text="Close" OnClientClick="self.close()" Visible="false"></asp:Button>
                    </div>
                </div>
            </div>
            <br />
            <div class="row">
                <div class="col-sm-12">
                    <table class="table table-bordered">
                        <%=vHeader %>
                        <%=vData %>
                    </table>
                    <br />
                    <table class="table table-bordered table-striped">
                        <%=vSubHeader %>
                    </table>
                </div>
            </div>
        </div>


        <div id="myModal" class="modal fade bd-example-modal-xl" tabindex="-1" role="dialog" aria-labelledby="myExtraLargeModalLabel" aria-hidden="true">
            <div class="modal-dialog modal-sm">
                <div class="modal-content">

                    <!-- Modal Header -->
                    <div class="modal-header">
                        <h5 class="modal-title">Are you sure you want to cancel the selected item?</h5>
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                    </div>
                     

                    <div class="row">
                        <div class="col-md-12">
                            <br />
                            <div class="col-md-12">
                                <asp:Button ID="BtnCancelItemReturn" CssClass="btn btn-primary btn-sm" runat="server" Text="Yes" />
                                <button type="button" class="btn btn-danger btn-sm" data-dismiss="modal">Close</button>
                            </div>
                        </div>
                    </div>
                    <br />
                    <br />
                </div>
            </div>
        </div>
    </form>
</body>
</html>
