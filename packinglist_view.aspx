<%@ Page Language="VB" AutoEventWireup="false" CodeFile="packinglist_view.aspx.vb" Inherits="packinglist_view" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="content/bootstrap/bootstrap-4.6.0-dist/css/bootstrap.css" rel="stylesheet" />
    <script src="content/jquery/jquery/jquery.3.6.js"></script>
    <script src="content/bootstrap/bootstrap-4.6.0-dist/js/bootstrap.js"></script>
    <link href="content/bootstrap/bootstrap-4.6.0-dist/css/boots-custom.css" rel="stylesheet" />
    <style>
        table {
            width: 100%;
        }

        table, th, td {
            border: solid 1px #808080;
            border-collapse: collapse;
        }

        .lbltitle {
            margin-bottom: 0px;
            font-size: 11px;
        }

        .lblDesc {
            margin-top: -5px;
            margin-bottom: 0px;
            font-size: 14px;
        }
    </style>

    <script> 
        $(document).ready(function () {

        });
    </script>
</head>
<body>
    <form id="form1" runat="server">

        <section>
            <div class="d-flex mt-4">
                <div class="p-2 flex-fill"></div>
                <div class="p-2 flex-fill text-center">
                    <img src="images/logo.jpg" style="width: 300px;" />
                    <br />
                    <h5 class="pl-4 pt-3">PACKING LIST</h5>

                </div>
                <div class="p-2 flex-fill"></div>
            </div>
            <div class="d-flex">
                <div class="d-flex col-sm-12">
                    <div class="p-2 flex-fill" style="width: 600px">
                        <div class="col-sm-12">
                            <label class="text-info text-sm-left lbltitle" for="usr">Customer</label>
                            <div class="form-group lblDesc">
                                <label runat="server" id="lblCust" class="text-info text-sm-left p-0" for="usr">Browse and select your CSV file</label>
                            </div>
                        </div>
                        <div class="col-sm-12">
                            <label class="text-info text-sm-left lbltitle" for="usr">Product Name</label>
                            <div class="form-group lblDesc">
                                <label runat="server" id="lblProd" class="text-info text-sm-left p-0" for="usr">Browse and select your CSV file</label>
                            </div>
                        </div>
                        <div class="col-sm-12">
                            <label class="text-info text-sm-left lbltitle" for="usr">Product Code</label>
                            <div class="form-group lblDesc">
                                <label runat="server" id="lbProdCode" class="text-info text-sm-left p-0" for="usr">Browse and select your CSV file</label>
                            </div>
                        </div>
                        <div class="col-sm-12">
                            <label class="text-info text-sm-left lbltitle" for="usr">Product GCAS</label>
                            <div class="form-group lblDesc">
                                <label runat="server" id="lblGCAS" class="text-info text-sm-left p-0" for="usr">Browse and select your CSV file</label>
                            </div>
                        </div>
                    </div>
                    <div class="p-2 flex-fill">
                        <div class="col-sm-12">
                            <label class="text-info text-sm-left lbltitle" for="usr">Job Order Number</label>
                            <div class="form-group lblDesc">
                                <label runat="server" id="lblJO" class="text-info text-sm-left p-0" for="usr">15-1000001</label>
                            </div>
                        </div>
                        <div class="col-sm-12">
                            <label class="text-info text-sm-left lbltitle" for="usr">Purchase Order Number</label>
                            <div class="form-group lblDesc">
                                <label runat="server" id="lblPO" class="text-info text-sm-left p-0" for="usr">10-1000001</label>
                            </div>
                        </div>
                        <div class="col-sm-12">
                            <label class="text-info text-sm-left lbltitle" for="usr">Sale Order Number</label>
                            <div class="form-group lblDesc">
                                <label runat="server" id="lblSO" class="text-info text-sm-left p-0" for="usr">15-1000001</label>
                            </div>
                        </div>
                    </div>
                    <div class="p-2 flex-fill">
                        <div class="col-sm-12">
                            <label class="text-info text-sm-left lbltitle" for="usr">Job Order Date</label>
                            <div class="form-group lblDesc">
                                <label runat="server" id="lblJODate" class="text-info text-sm-left p-0" for="usr">MM/DD/YYYY</label>
                            </div>
                        </div>
                        <div class="col-sm-12">
                            <label class="text-info text-sm-left lbltitle" for="usr">Purchase Order Date</label>
                            <div class="form-group lblDesc">
                                <label runat="server" id="lblPODate" class="text-info text-sm-left p-0" for="usr">MM/DD/YYYY</label>
                            </div>
                        </div>
                        <div class="col-sm-12">
                            <label class="text-info text-sm-left lbltitle" for="usr">Delivery Date</label>
                            <div class="form-group lblDesc">
                                <label runat="server" id="lblDelDate" class="text-info text-sm-left p-0" for="usr">MM/DD/YYYY</label>
                            </div>
                        </div>
                    </div>
                </div>

            </div>

        </section>
        <section>
            <div class="container-fluid">
                <div class="row">
                    <div class="col-12">
                        <table class="table-sm small mb-5">
                            <thead>
                                <tr>
                                    <th class="bg-primary text-white"></th>
                                    <th class="bg-primary text-white">BatchNo / RollNo</th>
                                    <th class="bg-primary text-white">Gross Wt</th>
                                    <th class="bg-primary text-white">Core Wt</th>
                                    <th class="bg-primary text-white">Net Wt</th>
                                    <th class="bg-primary text-white">Total PCS</th> 
                                    <th class="bg-primary text-white">QTY</th>
                                    <th class="bg-primary text-white">UOM</th> 
                                    <th class="bg-primary text-white">Pallet No</th>
                                </tr>
                            </thead>
                            <tbody>
                                <%=vData %>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </section>
    </form>
</body>
</html>

