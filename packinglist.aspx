<%@ Page Language="VB" AutoEventWireup="false" CodeFile="packinglist.aspx.vb" Inherits="warehouse" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="content/bootstrap/bootstrap-4.6.0-dist/css/bootstrap.css" rel="stylesheet" />
    <%--<link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@4.6.2/dist/css/bootstrap.min.css" />--%>
    <script src="content/jquery/jquery/jquery.3.6.js"></script>
    <script src="content/bootstrap/bootstrap-4.6.0-dist/js/bootstrap.js"></script>
    <link href="content/bootstrap/bootstrap-4.6.0-dist/css/boots-custom.css" rel="stylesheet" />

    <style>
        .sidebar {
            background-color: #F5F4F4;
            border: solid 0pc #000;
            box-shadow: 5px 7px 25px #999;
            height: 100vh;
            max-width: 300px
        }

        .bottom-border {
            border-bottom: 1px groove #eee;
        }

        .sidebar-link {
            color: #444;
            display: block;
            font-size: 15px;
            padding: 0.5rem 1rem;
            transition: all 1s;
        }

            .sidebar-link:hover {
                background-color: #444;
                border-radius: 5px;
                color: #fff;
                text-decoration: unset;
            }

        .modal-size {
            max-width: 800px;
        }

        .grid {
            max-width: 576px;
        }

        .bg-con {
            background-color: #f7f3f3;
            border-radius: 10px;
            padding: 10px;
            border: solid 1px #cccccc;
        }
    </style>

    <script>
        function PreparedBy() {
            var tdID = $("#txtBthNumKey").val();
            //alert("test1-tr" + tdID);
            $("#" + tdID).html('YES');

            $.post("packinglist_api.aspx",
                {
                    batchno: tdID,
                    jono: '<%= tblGetPackingList.SelectedRow.Cells(2).Text %>',
                    source: '<%= tblGetPackingList.SelectedRow.Cells(18).Text %>' 
                },
                function (data, status) {
                    $("#lblSearchMessage").fadeIn();
                    $("#lblSearchMessage").removeClass();
                    if (data == 0) {
                        $("#lblSearchMessage").html('Item not found');
                        $("#lblSearchMessage").addClass("pt-4 text-danger");
                    }
                    if (data == 1) {
                        $("#lblSearchMessage").html('Success');
                        $("#lblSearchMessage").addClass("pt-4 text-success");

                        $('#btn' + tdID).removeClass('btn btn-info btn-sm');
                        $('#btn' + tdID).addClass('btn btn-danger btn-sm');
                        $('#btn' + tdID).val('Del');
                    }
                    $("#lblSearchMessage").fadeOut();

                    //alert("Data: " + data + "\nStatus: " + status);
                });
        }

        function DelItem(Itemid) {
            alert("test1-tr" + Itemid);

            $.post("warehouse_ajax.aspx", { ReleasingDelItem: Itemid }, function (result) {
                $("#tr" + Itemid).remove();
            });
        }

        function AddItem(pId, pName) { 
            if (pName == 'Add') {
                $('#' + pId).removeClass('btn btn-info btn-sm');
                $('#' + pId).addClass('btn btn-danger btn-sm');
                $('#' + pId).val('Del');
            } else {
                $('#' + pId).removeClass('btn btn-danger btn-sm');
                $('#' + pId).addClass('btn btn-info btn-sm');
                $('#' + pId).val('Add');
            } 

            $.post("packinglist_api.aspx",
                {
                    batchno: pId,
                    trantype: pName
                },
                function (data, status) {                     
                    alert("Data: " + data + "\nParam:" + pName + "\nStatus: " + status);
                });

        }

        function dis_status(Val1) {
            alert('hi ' + Val1);
        }

        function ViewReport() {
            var myWindow = window.open("packinglist_view.aspx", "MsgWindow", "toolbar=yes,scrollbars=yes,resizable=yes,top=50,left=200,width=1100,height=900");
            //myWindow.document.write("<p>This is 'MsgWindow'. I am 200px wide and 100px tall!</p>");
        }

        $(document).ready(function () {
            $("a[title*='Del']").css("background-color", "yellow");

            $("#BtnSave").click(function (event) {
                var qty = $("#TxtItemQty");

                if (qty.val().trim() < 1 || qty.val().trim() == "") {
                    //qty.css("border", "solid 1px #811");
                    event.preventDefault();
                }
            })

            $("a").click(function () {
                var txt = this.id;
                $.post("warehouse_ajax.aspx", { warehouseType: txt }, function (result) {
                    $("#PendingItemList").html(result);
                });
            });

            $("#btnPListView").click(function () {
                var txt = this.id;
                $.post("packinglist_view.aspx", { warehouseType: txt }, function (result) {
                    $("#PendingItemList").html(result);
                });
            });

            $('#txtBthNumKey').keypress(function (e) {
                if (e.which == 13) {
                    e.preventDefault();
                    PreparedBy();
                }
            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <h5 class="pl-4 pt-3">Packing List Management</h5>
        <section>
            <div class="d-flex">
                <div class="pl-0 flex-grow-1">
                    <div class="container-fluid">
                        <div class="d-flex">
                            <div class="p-2 col-3 flex-fill">
                                <small>Quick Search (Enter Job Order Numner):</small>
                                <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                <small>Customer List:</small>
                                <asp:DropDownList ID="DDLCustList" runat="server" Width="" CssClass="form-control form-control-sm"></asp:DropDownList>
                                <asp:Button ID="btnSearch" CssClass="btn btn-primary btn-sm mt-2" runat="server" Text="Search" />
                            </div>
                            <div class="p-2 flex-fill"></div>
                        </div>
                    </div>

                    <div id="DivItemList" runat="server" class="d-flex">

                        <div class="p-2 col-12">
                        </div>

                    </div>
                </div>
            </div>
        </section>
        <section>
            <div class="d-flex container-fluid mb-3">
                <div class="col-12 bg-con">
                    <div class="d-flex pb-1">
                        <div class="mr-auto">
                            <h5>Packing List</h5>
                        </div>
                        <div class="">
                            <asp:Button ID="btnPListEdit" CssClass="btn btn-primary btn-sm" runat="server" Text="Edit" Enabled="false" />
                            <asp:Button ID="btnPListDel" CssClass="btn btn-primary btn-sm" runat="server" Text="Delete" Enabled="false" />
                            <asp:Button ID="btnPListAddItem" CssClass="btn btn-primary btn-sm" runat="server" Text="Add Item" Enabled="false" />
                            <asp:Button ID="btnPListView" CssClass="btn btn-primary btn-sm" runat="server" Text="View Print" Enabled="false" />
                        </div>
                    </div>
                    <asp:GridView ID="tblGetPackingList" runat="server" AllowPaging="True"
                        AutoGenerateColumns="False" Font-Size="Small"
                        CssClass="table table-bordered table-sm" PageSize="5">
                        <Columns>

                            <asp:CommandField ButtonType="Button" ShowSelectButton="True">
                                <ControlStyle CssClass="btn btn-primary btn-sm" />
                                <ItemStyle CssClass="" Width="40px" />

                            </asp:CommandField>

                            <asp:BoundField DataField="BatchNo" HeaderText="Batch No">
                                <ItemStyle />
                            </asp:BoundField>
                            <asp:BoundField DataField="JONO" HeaderText="Job Order No">
                                <ItemStyle />
                            </asp:BoundField>
                            <asp:BoundField DataField="CustName" HeaderText="Customer Name">
                                <ItemStyle />
                            </asp:BoundField>
                            <asp:BoundField DataField="ItemName" HeaderText="Item Name">
                                <ItemStyle />
                            </asp:BoundField>
                            <asp:BoundField DataField="CoreWeight" HeaderText="Core Weight">
                                <ItemStyle />
                            </asp:BoundField>
                            <asp:BoundField DataField="NetWeight" HeaderText="Net Weight">
                                <ItemStyle />
                            </asp:BoundField>
                            <asp:BoundField DataField="GrossWeight" HeaderText="Gross Weight">
                                <ItemStyle />
                            </asp:BoundField>
                            <asp:BoundField DataField="DateCreated" HeaderText="Date Created">
                                <ItemStyle />
                            </asp:BoundField>
                            <asp:BoundField DataField="PONO" HeaderText="PONO">
                                <ItemStyle />
                            </asp:BoundField>
                            <asp:BoundField DataField="PODate" HeaderText="PODate">
                                <ItemStyle />
                            </asp:BoundField>
                            <asp:BoundField DataField="DeliveryDate" HeaderText="DeliveryDate">
                                <ItemStyle />
                            </asp:BoundField>
                            <asp:BoundField DataField="ProdDate" HeaderText="ProdDate">
                                <ItemStyle />
                            </asp:BoundField>
                            <asp:BoundField DataField="Remarks" HeaderText="Remarks">
                                <ItemStyle />
                            </asp:BoundField>

                            <asp:BoundField DataField="PalletCnt" HeaderText="PalletCnt"
                                HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn" ItemStyle-Width="80px" />

                            <asp:BoundField DataField="PalletItemCnt" HeaderText="PalletItemCnt"
                                HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn" ItemStyle-Width="80px" />

                            <asp:BoundField DataField="CustomerId" HeaderText="CustomerId"
                                HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn" ItemStyle-Width="80px" />

                            <asp:BoundField DataField="Pallet" HeaderText="Pallet">
                                <ItemStyle />
                            </asp:BoundField>
                            <asp:BoundField DataField="Source" HeaderText="Source"
                                HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn" ItemStyle-Width="80px" />

                            <%--
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkDelete" OnClientClick=<%#String.Format("return dis_status(" & Eval("BatchNo") & ");") %> runat="server" CommandArgument='<%# Eval("BatchNo")%>' Text='<%# Eval("BatchNo")%>' CommandName="cmdDelete" />
                                </ItemTemplate>
                            </asp:TemplateField>--%>
                        </Columns>


                        <SelectedRowStyle CssClass="table-info" />
                        <PagerStyle Font-Size="8pt" />
                        <HeaderStyle CssClass="table-primary" />
                        <RowStyle CssClass="" />
                        <AlternatingRowStyle CssClass="" />
                    </asp:GridView>
                </div>
            </div>
            <div class="d-flex container-fluid">
                <div class="col-6 bg-con">
                    <div class="d-flex pb-1">
                        <div class="mr-auto">
                            <h5>Job Order List</h5>
                        </div>
                        <div class="">
                            <input type="button" runat="server" id="btnCreate" value="Create New Packing list" class="btn btn-primary btn-sm mt-2" data-toggle="modal" data-target="#myModal" disabled="disabled" />
                        </div>
                    </div>
                    <asp:GridView ID="tblItemMaster" runat="server" AllowPaging="True"
                        AutoGenerateColumns="False" Font-Size="Small"
                        CssClass="table table-bordered table-sm" PageSize="20">
                        <Columns>

                            <asp:CommandField ButtonType="Button" ShowSelectButton="True">
                                <ControlStyle CssClass="btn btn-primary btn-sm" />
                                <ItemStyle CssClass="" Width="40px" />
                            </asp:CommandField>

                            <asp:TemplateField HeaderText="#" HeaderStyle-Width="30px">
                                <ItemTemplate>
                                    <%# Container.DataItemIndex + 1 %>
                                </ItemTemplate>
                                <HeaderStyle Width="30px"></HeaderStyle>
                            </asp:TemplateField>


                            <asp:BoundField DataField="JobOrderNo" HeaderText="JONO">
                                <ItemStyle />
                            </asp:BoundField>
                            <asp:BoundField DataField="SalesOrderNo" HeaderText="SONO">
                                <ItemStyle CssClass="" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Item_Cd" HeaderText="Item Code">
                                <ItemStyle />
                            </asp:BoundField>
                            <asp:BoundField DataField="ItemName" HeaderText="Item Name">
                                <ItemStyle />
                            </asp:BoundField>
                            <asp:BoundField DataField="CustName" HeaderText="Customer">
                                <ItemStyle CssClass="" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Cust_Cd" HeaderText="Cust_Cd" HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn" />


                        </Columns>

                        <SelectedRowStyle CssClass="table-info" />
                        <PagerStyle Font-Size="8pt" />
                        <HeaderStyle CssClass="table-primary" />
                        <RowStyle CssClass="" />
                        <AlternatingRowStyle CssClass="" />
                    </asp:GridView>
                </div>
                <div class="p-2 flex-grow-1">
                    <div class="d-flex pb-1">
                        <div class="mr-auto">
                            <h5>Completion List</h5>
                        </div>
                        <div class="">
                            <button id="btnSlit" runat="server" type="button" class="btn btn-warning btn-sm mt-2">
                                SLITTING <span id="lblCtnComp1" runat="server" class="badge badge-danger">0</span>
                            </button>
                            <button id="btnBag" runat="server" type="button" class="btn btn-warning btn-sm mt-2">
                                BAG MAKING <span id="lblCtnComp2" runat="server" class="badge badge-danger">0</span>
                            </button>
                        </div>
                    </div>
                    <asp:GridView ID="tblItemDetails" runat="server" AllowPaging="True"
                        AutoGenerateColumns="False" Font-Size="Small"
                        CssClass="table table-bordered table-sm" PageSize="20">
                        <Columns>

                            <asp:TemplateField HeaderText="#" HeaderStyle-Width="30px">
                                <ItemTemplate>
                                    <%# Container.DataItemIndex + 1 %>
                                </ItemTemplate>
                                <HeaderStyle Width="30px"></HeaderStyle>
                            </asp:TemplateField>


                            <asp:BoundField DataField="ProcessDescr" HeaderText="Process Descr">
                                <ItemStyle />
                            </asp:BoundField>
                            <asp:BoundField DataField="BatchNo" HeaderText="BatchNo / RollNo">
                                <ItemStyle CssClass="" />
                            </asp:BoundField>
                            <asp:BoundField DataField="GrossWeight" HeaderText="Gross Weight">
                                <ItemStyle CssClass="" />
                            </asp:BoundField>
                            <asp:BoundField DataField="CoreWeight" HeaderText="Core Weight">
                                <ItemStyle />
                            </asp:BoundField>
                            <asp:BoundField DataField="NetWeight" HeaderText="Net Weight">
                                <ItemStyle />
                            </asp:BoundField>

                            <asp:BoundField DataField="QTY" HeaderText="QTY">
                                <ItemStyle CssClass="" />
                            </asp:BoundField>
                            <asp:BoundField DataField="TtlPCS" HeaderText="TtlPCS">
                                <ItemStyle CssClass="" />
                            </asp:BoundField>
                            <asp:BoundField DataField="DateCreated" HeaderText="Completion Date">
                                <ItemStyle CssClass="" />
                            </asp:BoundField>
                        </Columns>

                        <SelectedRowStyle CssClass="table-info" />
                        <PagerStyle Font-Size="8pt" />
                        <HeaderStyle CssClass="table-primary" />
                        <RowStyle CssClass="" />
                        <AlternatingRowStyle CssClass="" />
                    </asp:GridView>
                </div>
            </div>
        </section>


        <div id="AddItemModal" class="modal fade" data-backdrop="static" data-keyboard="false" tabindex="-1" aria-labelledby="staticBackdropLabel" aria-hidden="true">
            <div class="modal-dialog modal-size">
                <div class="modal-content">

                    <!-- Modal Header -->
                    <div class="modal-header">
                        <h4 class="modal-title">Item Details</h4>
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                    </div>

                    <!-- Modal body -->
                    <div class="modal-body">
                        <div class="col-12 bg-con">
                            <div class="d-flex pb-1">

                                <div class="col-7">
                                    <h6>Packing List Preparation:</h6>
                                    <div class="input-group">
                                        <input type="text" id="txtBthNumKey" class="form-control" placeholder="Scan or Enter Batch number" />
                                        <div class="input-group-append">
                                            <button class="btn btn-success" type="button" onclick="PreparedBy()">Search</button>
                                        </div>
                                    </div>
                                </div>
                                <div class="mr-auto col-5">
                                    <h4 id="lblSearchMessage" class="pt-4"></h4>
                                </div>
                            </div>
                        </div>
                        <table class="table table-bordered table-sm table-striped small mt-4">
                            <thead>
                                <tr>
                                    <th></th>
                                    <th>BatchNo / RollNo</th>
                                    <th>Gross W</th>
                                    <th>Core W</th>
                                    <th>Net W</th>
                                    <th>QTY</th>
                                    <th>Ttl PCS</th>
                                    <th>Ttl PCS per Box</th>
                                    <th>Item Found?</th>
                                    <th></th>
                                </tr>
                            </thead>
                            <tbody id="PendingItemList">
                                <%=Complist %>
                            </tbody>
                        </table>
                    </div>

                    <!-- Modal footer -->
                    <div class="modal-footer">
                        <asp:Button ID="BtnSaveSeleteditem" CssClass="btn btn-success btn-sm" runat="server" Text="Submit" />
                        <button type="button" class="btn btn-secondary btn-sm btn-sm" data-dismiss="modal">Close</button>
                    </div>

                </div>
            </div>
        </div>

        <div class="modal fade" id="myModal" data-backdrop="static" data-keyboard="false" tabindex="-1" aria-labelledby="staticBackdropLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title text-primary">Packing List Details</h5>
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                    </div>
                    <div class="modal-body">
                        <h6 class="text-info">Please complete all required fields</h6>
                        <div class="row">
                            <div class="col-md-12">
                                <small>Customer Name:</small>
                                <asp:Label ID="LblCustName" runat="server" CssClass="form-control form-control-sm" Text=""></asp:Label>
                            </div>
                            <div class="col-md-12">
                                <small>Product Name:</small>
                                <asp:Label ID="LblItemName" runat="server" CssClass="form-control form-control-sm" Text="" Height="70px"></asp:Label>
                            </div>
                            <div class="col-md-6">
                                <small>Job Order Number:</small>
                                <asp:TextBox ID="TxtJO" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                            </div>
                            <div class="col-md-6">
                                <small>Job Order Date:</small>
                                <asp:TextBox ID="TxtJODate" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                            </div>
                            <div class="col-md-6">
                                <small>Purchase Order Number:</small>
                                <asp:TextBox ID="TxtPO" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                            </div>
                            <div class="col-md-6">
                                <small>Purchase Order Date:</small>
                                <asp:TextBox ID="TxtPODate" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                            </div>

                            <div class="col-md-6">
                                <small>Production Date:</small>
                                <asp:TextBox ID="TxtProdDate" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                            </div>

                            <div class="col-md-6">
                                <small>Delivery Date:</small>
                                <asp:TextBox ID="TxtDelDate" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                            </div>
                            <div class="col-md-3">
                                <small class="text-info">Pallet Count:</small>
                                <asp:TextBox ID="TxtPalletCnt" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                            </div>
                            <div class="col-md-3">
                                <small class="text-info">Item per Pallet:</small>
                                <asp:TextBox ID="TxtPalletItemCnt" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                            </div>
                            <div class="col-md-6">
                                <small>Completion Source:</small>
                                <asp:DropDownList ID="ddlSource" runat="server" Width="" CssClass="form-control form-control-sm"></asp:DropDownList>
                            </div>
                            <div class="col-md-12">
                                <small>Remarks:</small>
                                <asp:TextBox ID="TxtRemarks" runat="server" CssClass="form-control form-control-sm" Rows="3" TextMode="MultiLine"></asp:TextBox>
                            </div>

                            <div class="col-sm-12">
                                <p id="demo" class="text-info"></p>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="BtnSave" CssClass="btn btn-success btn-sm" runat="server" Text="Submit" />
                        <asp:Button ID="BtnUpdate" CssClass="btn btn-info btn-sm" runat="server" Text="Submit" Visible="false" />
                        <button type="button" class="btn btn-sm btn-danger" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>

