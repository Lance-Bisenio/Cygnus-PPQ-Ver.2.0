<%@ Page Language="VB" AutoEventWireup="false" CodeFile="materials-releasing.aspx.vb" Inherits="inventory_materials_movement" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <title></title>

    <%--<link href="../css/BasicContol.css" rel="stylesheet" type="text/css" />--%>
    <link href="../css/calendar/jquery-ui-1.10.4.custom.min.css" rel="stylesheet" />
    <link href="../css/bootstrap4/css/bootstrap.css" rel="stylesheet" />
    
    <style>
        .form-margin {
            margin-bottom: 4px
        }

        label {
            color: #2c2c2c;
        }
        .drp {
            width:100%
        }

        body {
            font-family: Calibri, Arial, 'Trebuchet MS', sans-serif;
        }
    </style>

</head>
<body onload="invoke();">



    <form id="form1" runat="server">
        <div class="container-fluid">
            <input type="hidden" id="txtProcessCode" value="" runat="server" />
            <input type="hidden" id="txtSectCode" value="" runat="server" />

            <h4>
                <asp:Label ID="txtTitle" CssClass="vModuleTitle" runat="server"
                    Text="Dispatch Materials to Production"></asp:Label></h4>

            <asp:TextBox ID="txtItemCd" runat="server" CssClass="form-control" placeholder="Item Code" Visible="false"></asp:TextBox>


            <div class="row">
                <div class="col-sm-12">
                    <div class="form-inline">
                        <h6><small>Select Process:</small></h6>
                        <asp:DropDownList ID="cmbProcess" runat="server" CssClass="btn btn-secondary dropdown-toggle drp" AutoPostBack="True">
                        </asp:DropDownList>
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col-sm-12">
                    <table class="table table-bordered" style="font-size:12px">
                        <tr>
                            <td style="width: 150px">Job Order No:</td>
                            <td>
                                <asp:Label ID="lblJONO" runat="server" Text="-" CssClass="BiglabelL" ForeColor="Red" /></td>
                        </tr>
                        <tr>
                            <td>BOM | Rev | Order No:</td>
                            <td>
                                <asp:Label ID="lblBOM" runat="server" Text="-" CssClass="BiglabelL" />&nbsp;|
                                <asp:Label ID="lblOrderNo" runat="server" Text="-" CssClass="BiglabelL" />
                            </td>
                        </tr> 
                        <tr>
                            <td>RW Code:</td>
                            <td>
                                <asp:Label ID="lblRWCode" runat="server" Text="-" CssClass="BiglabelL" /></td>
                        </tr>
                        <tr>
                            <td>Machine:</td>
                            <td>
                                <asp:Label ID="LblMachine" runat="server" Text="-" CssClass="BiglabelL" /></td>
                        </tr>
                    </table>
                </div>
            </div>

            <div class="row">
                <div class="col-12">
                    <h6><small>Default BOM raw materials:</small></h6>
                    <asp:DropDownList ID="cmbMaterials" runat="server" CssClass="btn btn-secondary dropdown-toggle drp" AutoPostBack="True"></asp:DropDownList>
                </div>
            </div>

            <div class="row">
                <div class="col-12">
                    <h6><small>Alternative raw materials:</small></h6>
                    <asp:DropDownList ID="cmbAltItem" runat="server" CssClass="btn btn-warning dropdown-toggle drp" AutoPostBack="True"></asp:DropDownList>
                </div>
            </div>


            <div class="row">
                <div class="col-12">
                    <h6><small>Lot Number:</small></h6>
                    <asp:DropDownList ID="cmbItemLotNo" runat="server" CssClass="btn btn-secondary dropdown-toggle drp" AutoPostBack="True"></asp:DropDownList>
                </div>
            </div>

            <div class="row">
                <div class="col-6">
                    <h6><small>Quantity :</small></h6>
                    <div class="form-group">
                        <asp:TextBox ID="txtQty" runat="server" CssClass="form-control border border-danger" Width="200px" placeholder="Enter Quantity"></asp:TextBox>
                    </div>
                    <h6><small>Roll Number :</small></h6>
                    <div class="form-group">
                        <asp:TextBox ID="txtRollNuber" runat="server" CssClass="form-control border border-info" Width="200px" placeholder="Enter Roll Number"></asp:TextBox>
                    </div>  
                </div>
                <div class="col-6">
                    <div class="col-12">
                        <h6><small>Unit Cost :</small></h6>
                        <b><asp:Label ID="lblCost" runat="server" Text="0.00" CssClass="BiglabelL" /></b>
                    </div>
                    
                    <div class="col-12">
                        <h6><small>Onhand :</small></h6>
                        <b><asp:Label ID="lblCurrQty" runat="server" Text="0.00" CssClass="BiglabelL" /></b>
                    </div>
                    
                    <div class="col-12">
                        <h6><small>UOM :</small></h6>
                        <b><asp:Label ID="lblUOM" runat="server" Text="-" CssClass="BiglabelL" /></b>
                    </div>
                    
                </div>
            </div>


            <div class="row">
                <div class="form-group"> 
                    <div class="col-sm-10">
                        <asp:Button ID="cmdSave" runat="server" CssClass="btn btn-primary" Text="Save"></asp:Button>
                        <asp:Button ID="cmdEdit" runat="server" CssClass="btn btn-primary" Text="Update" Visible="False"></asp:Button>
                        <asp:Button ID="cmdDelete" runat="server" CssClass="btn btn-primary" Text="Delete" Visible="False"></asp:Button>
                        <asp:Button ID="cmdCancel" runat="server" CssClass="btn btn-primary" Text="Cancel" OnClientClick="self.close()"></asp:Button>
                    </div>
                </div>
            </div>

        </div>
    </form>
</body>
</html>


<script src="../js/jquery-ui-1.10.4.custom.js"></script>
<script src="../js/jquery-ui-1.10.4.custom.min.js"></script>
<script src="../js/jquery-1.10.2.js"></script>
<script src="../js/jquery-ui.js"></script>

<script>
    function invoke() {
        <%=vScript %>
           }
      
</script>
