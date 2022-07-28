<%@ Page Language="VB" AutoEventWireup="false" CodeFile="processmaterials_receiving.aspx.vb" Inherits="processmaterials_receiving" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <link href="css/bootstrap4/css/bootstrap.css" rel="stylesheet" />
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

            //$(".btn btn-danger btn-sm my-1 py-1").click(function (event) {
            //    alert("test");
            //})
            
           

            $("a").click(function () {
                var txt = this.id;
                $.post("warehouse_ajax.aspx", { ItemToReceive: txt }, function (result) {
                    $("#PendingItemList").html(result);
                });
            });
            // $("a[name=|'Del']").click(function () {
            //    var txt = this.id;
            //    alert("test");
            //});

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
                                <h4>Process Warehouse</h4>
                                <h6 class="text-info">Raw-Materials Receiving</h6>
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
            <div id="panel" class="col-lg-10 ml-auto">
                <div class="container-fluid"> 
                   <div class="row">
                        
                        <div class="col-md-3">
                            <small>Date Received From:</small>
                            <asp:TextBox ID="TxtDatePosterFrom" runat="server" CssClass="form-control form-control-sm" placeholder="mm / dd / yyyy"></asp:TextBox>

                        </div>
                        <div class="col-md-3">
                            <small>Process Warehouse List:</small>
                            <asp:DropDownList ID="DDLWarehouseList" runat="server" Width="" CssClass="form-control form-control-sm"></asp:DropDownList>
                        </div>
                        <div class="col-md-3">
                          
                        </div>
                        <div class="col-md-3"> 
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-3">
                            <small>Date Received To:</small>
                            <asp:TextBox ID="TxtDatePosterTo" runat="server" CssClass="form-control form-control-sm" placeholder="mm / dd / yyyy"></asp:TextBox>
                        </div>
                        <div class="col-md-3">
                            <br />
                            <asp:Button ID="btnSearch" CssClass="btn btn-primary btn-sm" runat="server" Text="Search" />
                             
                        </div>
                        <div class="col-md-3">
                            
                        </div>
                        <div class="col-md-3"></div>
                    </div>  
                    <br /> 
                    <div class="row">
                        <div class="col-md-4">
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
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ItemName" HeaderText="Item Name"> 
                                    </asp:BoundField>
                                    <asp:BoundField DataField="WHName" HeaderText="WHTag"> 
                                    </asp:BoundField>
                                    
                                     
                                </Columns>

                                <SelectedRowStyle CssClass="table-info" />
                                <PagerStyle Font-Size="8pt" />
                                <HeaderStyle CssClass="table-light" />
                                <RowStyle CssClass="" />
                                <AlternatingRowStyle CssClass="" />
                            </asp:GridView>
                        </div>
                        <div class="col-md-3">
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
                                <HeaderStyle CssClass="table-light" />
                                <RowStyle CssClass="" />
                                <AlternatingRowStyle CssClass="" />
                            </asp:GridView>
                        </div>
                        <div class="col-md-5">
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
                                <HeaderStyle CssClass="table-light" />
                                <RowStyle CssClass="" />
                                <AlternatingRowStyle CssClass="" />
                            </asp:GridView>
                        </div>
                    </div>

                </div>
            </div>
        </section>



        <div class="modal fade" id="ItemLisModal" data-backdrop="static" data-keyboard="false" tabindex="-1" aria-labelledby="staticBackdropLabel" aria-hidden="true">
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
                                </tr>
                            </thead>
                            <tbody id="PendingItemList">
                            </tbody>
                        </table>
                    </div>

                    <!-- Modal footer -->
                    <div class="modal-footer">
                        <button type="button" id="btnAccept" runat="server" class="btn btn-success text-uppercase" data-dismiss="modal">Accept</button>
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
