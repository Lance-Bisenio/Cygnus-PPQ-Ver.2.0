<%@ Page Language="VB" AutoEventWireup="false" CodeFile="item_inv.aspx.vb" Inherits="item_inv" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <title></title>

    <link href="../css/calendar/jquery-ui-1.10.4.custom.min.css" rel="stylesheet" />
    <link href="../bootstrap/bootstrap-4.0.0/css/bootstrap.min.css" rel="stylesheet" />

    <%--<link href="../css/bootstrap4/css/bootstrap.css" rel="stylesheet" />
    <link href="../css/gridview.css" rel="stylesheet" />--%>



    <style type="text/css">
        .ZeroPadleft {
            padding-left: 0px;
            margin-left: 0px;
        }

        .iDataFrame {
            width: 99%;
            border: solid 0px #e2e2e2;
            height: 96%;
            margin: 0px;
        }

        div {
            border: 0px solid #000;
            padding-top: 1px;
        }

        body {
            font-family: Arial;
            font-size: 12px;
        }

        .RowLink {
            display: table-row;
        }

            .RowLink:hover {
                background-color: #e5e5e5;
                cursor: pointer;
            }
    </style>

</head>
<body onload="invoke();">
    <form id="form1" runat="server" autocomplete="off">

        <div class="container-fluid">
            <h3>Wrapping Inventory</h3>

            <div class="row Mgin">
                <div class="col-md-3">
                    Date Created From:
                    <asp:TextBox ID="txtDateFrom" runat="server" CssClass="form-control form-control-sm col-5" placeholder="mm/dd/yyyy"></asp:TextBox>
                </div>

                <div class="col-md-3">
                </div>
                <div class="col-md-3">
                </div>
                <div class="col-md-3">
                </div>
            </div>

            <div class="row Mgin">
                <div class="col-md-3">
                    Date Created To:
                    <asp:TextBox ID="txtDateTo" runat="server" CssClass="form-control form-control-sm col-5" placeholder="mm/dd/yyyy"></asp:TextBox>
                </div>
                <div class="col-md-3">
                    Quick Search (Enter Customer Name, Product Name or PO Number)
                    <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control form-control-sm" Placeholder="Enter your keyword"></asp:TextBox>
                </div>
                <div class="col-md-3">
                </div>
                <div class="col-md-3">
                </div>
            </div>
            <br />
            <div class="row Mgin">
                <div class="col-md-3">
                </div>
                <div class="col-md-3">
                </div>
                <div class="col-md-3">
                </div>
                <div class="col-md-3">
                    <input type="hidden" id="h_EmpCode" runat="server" />
                    <input type="hidden" id="Hidden1" runat="server" />
                    <input type="hidden" id="h_ItemCd" name="h_ItemCd" runat="server" style="width: 45px" />
                    <input type="hidden" id="h_Mode" name="h_Mode" runat="server" style="width: 45px" />
                    <input type="hidden" id="h_Sql" name="h_Mode" runat="server" style="width: 45px" />
                </div>
            </div>

            <div class="row Mgin">
                <div class="col-md-3">
                    <div class="btn-group">
                        <input type="button" id="BtnPackingList" class="btn btn-primary btn-sm" value="Create Packing List" />
                        <input type="button" id="BtnEditPackingList" class="btn btn-primary btn-sm" value="Edit" />
                        <input type="button" id="BtnScanItem" class="btn btn-primary btn-sm" value="Scan Item" />
                        <asp:Button ID="BtnSearch" CssClass="btn btn-primary btn-sm" runat="server" Text="Search" />
                    </div>
                </div>
                <div class="col-md-1"><%--Page No. :--%></div>
                <div class="col-md-3">
                </div>
                <div class="col-md-1"></div>
                <div class="col-md-1">
                    <div class="col-md-10">
                    </div>
                </div>
            </div><br />

            <div class="row Mgin">
                <div class="col-md-7">  
                    <asp:GridView ID="TblPackingListHeader" runat="server" AllowPaging="True" BorderColor="#CCCCCC" Font-Size="12px"
                        AutoGenerateColumns="False" Width="100%" BorderStyle="Solid" BorderWidth="1px"
                        CssClass="table table-sm table-bordered table-striped" PageSize="20" EnableModelValidation="True"
                        SelectedRowStyle-CssClass="btn btn-info">
                         
                        <Columns> 
                            <asp:CommandField ButtonType="Button" ShowSelectButton="True">
                                <ItemStyle CssClass="" Width="40px" />
                                <ControlStyle CssClass="btn btn-primary btn-sm" />
                            </asp:CommandField>

                            <asp:TemplateField HeaderText="#" HeaderStyle-Width="30px">
                                <ItemTemplate>
                                    <%# Container.DataItemIndex + 1 %>
                                </ItemTemplate>
                                <HeaderStyle Width="30px"></HeaderStyle>
                            </asp:TemplateField>

                            <asp:BoundField DataField="BatchNo" HeaderText="Reference No.">
                                <ItemStyle />
                            </asp:BoundField>

                            <asp:BoundField DataField="Cust_Cd" HeaderText="Cust Code">
                                <ItemStyle />
                            </asp:BoundField>

                            <asp:BoundField DataField="CustomerName" HeaderText="Customer Name">
                                <ItemStyle />
                            </asp:BoundField>

                            <asp:BoundField DataField="Item_Cd" HeaderText="Item Code">
                                <ItemStyle />
                            </asp:BoundField>

                            <asp:BoundField DataField="ItemName" HeaderText="Description">
                                <ItemStyle />
                            </asp:BoundField>

                            <asp:BoundField DataField="IONumber" HeaderText="IONumber">
                                <ItemStyle />
                            </asp:BoundField>

                            <asp:BoundField DataField="PONO" HeaderText="PONO">
                                <ItemStyle />
                            </asp:BoundField>
                            <asp:BoundField DataField="CreatedBy" HeaderText="Created By">
                                <ItemStyle />
                            </asp:BoundField>
                            <asp:BoundField DataField="DateCreated" HeaderText="Date Created">
                                <ItemStyle />
                            </asp:BoundField>
                            
                             
                        </Columns>

                        <SelectedRowStyle CssClass="bg-warning" />
                        <PagerStyle Font-Size="8pt" />
                        <HeaderStyle CssClass="titleBar" />
                        <RowStyle CssClass="odd" />
                        <AlternatingRowStyle CssClass="even" />
                    </asp:GridView>
                </div>
                <div class="col-md-5">
                    <asp:GridView ID="tblItemMaster" runat="server" AllowPaging="True"
                        AutoGenerateColumns="False" Width="100%"
                        CssClass="table table-bordered " PageSize="10">
                        <Columns>

                            <asp:CommandField ButtonType="Button" ShowSelectButton="True" SelectText="Select">
                            <ItemStyle CssClass="labelC" Width="40px" />
                            <ControlStyle CssClass="btn btn-primary btn-sm" />
                        </asp:CommandField>

                            <asp:TemplateField HeaderText="#" HeaderStyle-Width="30px">
                                <ItemTemplate>
                                    <%# Container.DataItemIndex + 1 %>
                                </ItemTemplate>
                                <HeaderStyle Width="30px"></HeaderStyle>
                            </asp:TemplateField>

                            <asp:BoundField DataField="ItemCode" HeaderText="Item Code">
                                <ItemStyle />
                            </asp:BoundField>

                            <asp:BoundField DataField="ItemName" HeaderText="Item Description">
                                <ItemStyle />
                            </asp:BoundField>

                            <asp:BoundField DataField="vRollWidth" HeaderText="Roll Width">
                                <ItemStyle />
                            </asp:BoundField>

                            <asp:BoundField DataField="vRepeatLenght" HeaderText="Repeat Lenght">
                                <ItemStyle />
                            </asp:BoundField>

                            <asp:BoundField DataField="vBagDimension" HeaderText="Bag Dimension">
                                <ItemStyle />
                            </asp:BoundField>

                        </Columns>

                        <SelectedRowStyle CssClass="bg-light" />
                        <PagerStyle Font-Size="8pt" />
                        <HeaderStyle CssClass="titleBar" />
                        <RowStyle CssClass="odd" />
                        <AlternatingRowStyle CssClass="even" />
                    </asp:GridView>
                </div>
                <%--<div class="col-md-4"> 
				
			</div> --%>
            </div>
            <br />

            <div class="row Mgin">
                <div class="col-md-12">
                    <table class="table table-bordered">
                        <tr>
                            <td>#</td>
                            <td>Job Order No#</td>
                            <td>GCAS / Product Code</td>
                            <td>Roll No.</td>
                            <td>Gross Wight	</td>
                            <td>Core Weight</td>
                            <td>Net Weight</td>
                            <td>QTY</td>
                            <td>UOM</td>
                        </tr>
                        <%=vData %>
                    </table>
                </div>
            </div>

            <div class="row Mgin">
                <div class="col-md-8">
                    <asp:Label ID="lblMessageBox" runat="server" Text="..."></asp:Label>
                </div>
            </div>
        </div>

        <!-- Modal -->
        <div id="ModalItemDetails" style="width: 100%" class="modal fade" role="dialog">
            <div class="modal-dialog modal-lg">

                <!-- Modal content-->
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h4 class="modal-title">Item Logs Details</h4>
                    </div>
                    <div class="modal-body">
                        <div class="row Mgin">
                            <div class="col-md-12">
                                <iframe id="frmItemDetails" style="border: solid 1px; width: 100%; height: 600px;" src="../inventory/joborder-completion.aspx"></iframe>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-primary btn-sm" data-dismiss="modal">Save</button>
                        <button type="button" class="btn btn-primary btn-sm" data-dismiss="modal">Close</button>
                    </div>
                </div>

            </div>
        </div>

        <div id="ModalCreateNew" style="width: 100%" class="modal fade" role="dialog">
            <div class="modal-dialog modal-lg">

                <!-- Modal content-->
                <div class="modal-content">
                    <div class="modal-header">
                        <h4 class="modal-title">Create Paking List</h4>
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                    </div>
                    <div class="modal-body">
                        <div class="row Mgin">
                            <div class="col-md-8">
                                Customer Name:
                                <asp:DropDownList ID="CmdCustomerList" runat="server" Width="" CssClass="form-control form-control-sm border border-danger"></asp:DropDownList> 
                            </div>
                            <div class="col-md-4">
                                Date Created:
                                <asp:TextBox ID="TxtDateCreated" runat="server" CssClass="form-control form-control-sm border border-danger"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row Mgin">
                            <div class="col-md-8">
                                Product Name:
                                <asp:DropDownList ID="CmdProductList" runat="server" Width="" CssClass="form-control form-control-sm border border-danger"></asp:DropDownList>  
                            </div>
                            <div class="col-md-4">
                                I.O Number
                                <asp:TextBox ID="TxtIONUmber" runat="server" CssClass="form-control form-control-sm border border-danger"></asp:TextBox>
                            </div>
                        </div>

                        <div class="row Mgin">
                            <div class="col-md-8"></div>
                            <div class="col-md-4">
                                PO Number
                                <asp:TextBox ID="TxtPONO" runat="server" CssClass="form-control form-control-sm border border-danger"></asp:TextBox>
                            </div>
                        </div>
                      
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-primary btn-sm" id="BtnSubmit" runat="server">Submit</button>
                        <button type="button" class="btn btn-danger btn-sm" data-dismiss="modal">Close</button>
                    </div>
                </div>

            </div>
        </div>

        <div id="ModalScan" style="width: 100%" class="modal fade" role="dialog">
            <div class="modal-dialog modal-sm">

                <!-- Modal content-->
                <div class="modal-content">
                    <div class="modal-header">
                        <h4 class="modal-title">Scan Item</h4>
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                    </div>
                    <div class="modal-body">
                        <div class="row Mgin">
                            <div class="col-md-8">
                                Customer Name:
                                <asp:DropDownList ID="DropDownList1" runat="server" Width="" CssClass="form-control form-control-sm border border-danger"></asp:DropDownList> 
                            </div>
                            <div class="col-md-4">
                                Date Created:
                                <asp:TextBox ID="TextBox1" runat="server" CssClass="form-control form-control-sm border border-danger"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row Mgin">
                            <div class="col-md-8">
                                Product Name:
                                <asp:DropDownList ID="DropDownList2" runat="server" Width="" CssClass="form-control form-control-sm border border-danger"></asp:DropDownList>  
                            </div>
                            <div class="col-md-4">
                                I.O Number
                                <asp:TextBox ID="TextBox2" runat="server" CssClass="form-control form-control-sm border border-danger"></asp:TextBox>
                            </div>
                        </div>

                        <div class="row Mgin">
                            <div class="col-md-8"></div>
                            <div class="col-md-4">
                                PO Number
                                <asp:TextBox ID="TextBox3" runat="server" CssClass="form-control form-control-sm border border-danger"></asp:TextBox>
                            </div>
                        </div>
                      
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-primary btn-sm" id="Button1" runat="server">Submit</button>
                        <button type="button" class="btn btn-danger btn-sm" data-dismiss="modal">Close</button>
                    </div>
                </div>

            </div>
        </div>
    </form>
</body>
</html>



<script src="../js/jquery-v3.5.1.js"></script>
<script src="../js/jquery-ui.js"></script>
<script src="../bootstrap/bootstrap-4.0.0/js/bootstrap.min.js"></script>

<%--<script src="../js/jquery-1.10.2.js"></script> 
<script src="../bootstrap/bootstrap/js/bootstrap.js"></script>--%>


<script type="text/javascript">

    function invoke() {
        <%=vScript %>
    }

    function ShowDetails(pJONO) {
        var vProperties = "width=1200px, height=550px, top=50px, left=80px, scrollbars=yes";
        var vParam = "pJO=" + pJONO;
        var vDeleteParam = ""

        winPop = window.open("item_invlogs.aspx?" + vParam + vDeleteParam + "",
            "SmallWindow", vProperties);
        winPop.focus();

        //document.getElementById('frmItemDetails').src = sites[Math.floor(Math.random() * sites.length)];
        //$('#ModalItemDetails').modal('show');
    }

    $(document).ready(function () {
        $(document).ready(function () {
            $("#txtDateFrom").datepicker();
            $("#txtDateTo").datepicker();
        });

        $('ItemHeader').click(function () {
            alert("Data: lance ");
        });

        $('#BtnPackingList').click(function () {
            $('#ModalCreateNew').modal();

        });

        $('#BtnEditPackingList').click(function () {
            var RefNo = "<%=Session("BatchNo") %>";
            $('#ModalCreateNew').modal();
        });

        $('#BtnScanItem').click(function () { 
            $('#ModalScan').modal();
        });

        
    });


</script>
