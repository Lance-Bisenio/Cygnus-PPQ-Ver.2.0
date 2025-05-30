﻿<%@ Page Language="VB" AutoEventWireup="false" CodeFile="warehouse.aspx.vb" Inherits="warehouse" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@4.6.2/dist/css/bootstrap.min.css" />
    <script src="content/jquery/jquery/jquery.3.6.js"></script>
    <script src="content/bootstrap/bootstrap-4.6.0-dist/js/bootstrap.js"></script>

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
    </style>

    <script>
        function DelItem(Itemid) {
            alert("test1-tr" + Itemid);

            $.post("warehouse_ajax.aspx", { ReleasingDelItem: Itemid }, function (result) {
                $("#tr" + Itemid).remove();
            });

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
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">

        <nav class="navbar navbar-expand-md navbar-light">
            <div class="collapse navbar-collapse">
                <div class="container-fluid">
                    <div class="row">
                        <div class="col-lg-3 sidebar fixed-top mb-5">
                            <label class="nav-brand d-block mx-auto text-center text-secondary py-3 bottom-border">
                                <h4>Main Warehouse</h4>
                                <h6 class="text-info">Releasing</h6>
                            </label>

                            <ul class="navbar-nav flex-column mt-2">
                                <%=vPendingItem %>
                            </ul>
                        </div>

                    </div>
                </div>
            </div>
        </nav>

        <section>
            <div class="d-flex">
                <div class="p-2" style="width: 305px"></div>
                <div class="p-2 flex-grow-1">
                    <div class="container-fluid">
                        <div class="d-flex mb-3">
                            <div class="p-2 col-3 flex-fill">
                                <small>Item Type:</small>
                                <asp:DropDownList ID="cmbItemType" runat="server" CssClass="form-control form-control-sm">
                                </asp:DropDownList>

                                <small>Quick Search (Enter Item Code):</small>
                                <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                <small>Lot Number:</small>
                                <asp:TextBox ID="TxtLotno" runat="server" CssClass="form-control form-control-sm" placeholder="Enter lot number and select the item code"></asp:TextBox>


                                <asp:Button ID="btnSearch" CssClass="btn btn-primary btn-sm mt-2" runat="server" Text="Search" />
                                <input type="button" runat="server" id="btnScan" value="Add to my list" class="btn btn-success btn-sm mt-2" data-toggle="modal" data-target="#myModal" />
                            </div>
                            <div class="p-2 col-3 flex-fill">
                                <small>Warehouse List:</small>
                                <asp:DropDownList ID="DDLWarehouseList" runat="server" Width="" CssClass="form-control form-control-sm"></asp:DropDownList>
                                <small>Posted Date From:</small>
                                <asp:TextBox ID="txtDateFrom" runat="server" CssClass="form-control form-control-sm" placeholder="mm / dd / yyyy"></asp:TextBox>
                                <small>Posted Date To:</small>
                                <asp:TextBox ID="TextBox1" runat="server" CssClass="form-control form-control-sm" placeholder="mm / dd / yyyy"></asp:TextBox>
                                <asp:Button ID="BtnViewSummary" CssClass="btn btn-primary btn-sm mt-2" runat="server" Text="View Posted Transaction" />
                            </div>
                            <div class="p-2 flex-fill"></div>
                        </div>
                    </div>
                    <div id="DivItemList" runat="server" class="d-flex mb-3">
                        <div class="p-2 flex-fill grid">
                            <h6>
                                <asp:Label ID="lblTotal" runat="server" CssClass="" Text="Total Item Retrieved : 0"></asp:Label></h6>
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


                                    <asp:BoundField DataField="Item_Cd" HeaderText="Item Code">
                                        <ItemStyle Width="90px" />
                                    </asp:BoundField>

                                    <asp:BoundField DataField="Descr" HeaderText="Item Description">
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
                        <div class="p-2 flex-fill grid">
                            <h6>
                                <asp:Label ID="lblTotalPerItem" runat="server" CssClass="" Text="Total Item Retrieved : 0"></asp:Label></h6>
                            <asp:GridView ID="tblItemOnhandDetails" runat="server" AllowPaging="True"
                                AutoGenerateColumns="False" Font-Size="Small"
                                CssClass="table table-bordered table-sm" PageSize="20">
                                <Columns>

                                    <asp:CommandField ButtonType="Button" ShowSelectButton="True">
                                        <ControlStyle CssClass="btn btn-primary btn-sm" />
                                        <ItemStyle CssClass="" Width="40px" />
                                    </asp:CommandField>

                                    <asp:BoundField DataField="LotNum" HeaderText="Lot Number">
                                        <ItemStyle />
                                    </asp:BoundField>

                                    <asp:BoundField DataField="QTY" HeaderText="QTY">
                                        <ItemStyle CssClass="text-right" />
                                    </asp:BoundField>

                                </Columns>


                                <SelectedRowStyle CssClass="table-info" />
                                <PagerStyle Font-Size="8pt" />
                                <HeaderStyle CssClass="table-primary" />
                                <RowStyle CssClass="" />
                                <AlternatingRowStyle CssClass="" />
                            </asp:GridView>
                        </div>
                        <div class="p-2 flex-fill grid">
                            <h6>
                                <asp:Label ID="lblTotalPerLotnum" runat="server" CssClass="" Text="Total Item Retrieved : 0"></asp:Label></h6>
                            <asp:GridView ID="tblItemTransaction" runat="server" AllowPaging="True"
                                AutoGenerateColumns="False" Font-Size="Small"
                                CssClass="table table-bordered table-sm" PageSize="20">
                                <Columns>

                                    <asp:BoundField DataField="QTY" HeaderText="QTY">
                                        <ItemStyle CssClass="text-right" />
                                    </asp:BoundField>

                                    <asp:BoundField DataField="UOM" HeaderText="UOM">
                                        <ItemStyle CssClass="text-right" />
                                    </asp:BoundField>

                                    <asp:BoundField DataField="Remarks" HeaderText="Remarks">
                                        <ItemStyle CssClass="" />
                                    </asp:BoundField>

                                    <asp:BoundField DataField="DateCreated" HeaderText="Date Created">
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
                </div>
            </div>
        </section>
        <section>
            <div id="DivSummary" runat="server" class="d-flex" visible="false">
                <div class="p-2" style="width: 305px"></div>
                <div class="p-2 flex-grow-1">

                    <div class="d-flex">
                        <div class="p-2 col-3">
                            <asp:GridView ID="TblSummary" runat="server" AllowPaging="True"
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


                                    <asp:BoundField DataField="PostRefNo" HeaderText="BatchNo">
                                        <ItemStyle Width="90px" />
                                    </asp:BoundField>

                                    <asp:BoundField DataField="DatePosted" HeaderText="Date Posted">
                                        <ItemStyle CssClass="" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="SystemRemarks" HeaderText="Remarks">
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
                        <div class="p-2 flex-grow-1">
                            <asp:GridView ID="TblTranDetails" runat="server" AllowPaging="True"
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


                                    <asp:BoundField DataField="PostRefNo" HeaderText="BatchNo">
                                        <ItemStyle Width="90px" />
                                    </asp:BoundField>

                                    <asp:BoundField DataField="DatePosted" HeaderText="Date Posted">
                                        <ItemStyle CssClass="" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="SystemRemarks" HeaderText="Remarks">
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
                </div>

            </div>
        </section>



        <div id="ItemLisModal" class="modal fade" data-backdrop="static" data-keyboard="false" tabindex="-1" aria-labelledby="staticBackdropLabel" aria-hidden="true">
            <div class="modal-dialog modal-size">
                <div class="modal-content">

                    <!-- Modal Header -->
                    <div class="modal-header">
                        <h4 class="modal-title">Item Details</h4>
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                    </div>

                    <!-- Modal body -->
                    <div class="modal-body">
                        <table class="table table-bordered table-sm table-striped small">
                            <thead>
                                <tr>
                                    <th></th>
                                    <th>Item Code</th>
                                    <th>Item Description</th>
                                    <th>LOTNO</th>
                                    <th>QTY</th>
                                    <th>Date Created</th>
                                    <th>Del</th>
                                </tr>
                            </thead>
                            <tbody id="PendingItemList">
                            </tbody>
                        </table>
                    </div>

                    <!-- Modal footer -->
                    <div class="modal-footer">
                        <button type="button" id="btnPost" runat="server" class="btn btn-success" data-dismiss="modal">POST</button>
                        <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                    </div>

                </div>
            </div>
        </div>

        <div class="modal fade" id="myModal" data-backdrop="static" data-keyboard="false" tabindex="-1" aria-labelledby="staticBackdropLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title text-primary">
                            <asp:Label ID="Label16" runat="server" Text="Add to list"></asp:Label></h5>
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                    </div>
                    <div class="modal-body">

                        <div class="row">
                            <div class="col-md-12">
                                <small>Transaction Type:</small>
                                <asp:DropDownList ID="DDLTranType" runat="server" Width="" CssClass="form-control form-control-sm"></asp:DropDownList>
                            </div>
                            <div class="col-md-12">
                                <small>Item QTY:</small>
                                <asp:TextBox ID="TxtItemQty" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
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
                        <button type="button" class="btn btn-sm btn-danger" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>

