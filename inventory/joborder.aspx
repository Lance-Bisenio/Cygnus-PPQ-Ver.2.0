<%@ Page Language="VB" AutoEventWireup="false" CodeFile="joborder.aspx.vb" Inherits="joborder" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../bootstrap/css/bootstrap.css" rel="stylesheet" /> 
    <script src="../js/jquery-ui-1.10.4.custom.js"></script>
    <script src="../js/jquery-ui-1.10.4.custom.min.js"></script>
    <script src="../js/jquery-1.10.2.js"></script>
    <script src="../js/jquery-ui.js"></script>
     
    <link href="../css/calendar/jquery-ui-1.10.4.custom.min.css" rel="stylesheet" />
    <link href="../css/calendar/jquery-ui-1.10.4.custom.css" rel="stylesheet" />
    <script type="text/javascript"> 
     
        $(document).ready(function () {
            $('#cmbItemName').on('change', function () {
                $('#txtItemCd').val($('#cmbItemName').val());
            });

            $('#cmbCustomerName').on('change', function () {
                $('#txtCustomerCd').val($('#cmbCustomerName').val());
            });

            $("#txtReleaseDate").datepicker();
            $("#txtSDate").datepicker();
            $("#txtDueDate").datepicker();


            //$("").datepicker();
            
        });

        function invoke() {
            <%=vScript %>
        }
    </script>

    <style type="text/css">
        .auto-style1 {
            height: 24px;
        }
        .Mgin {
            margin-bottom: 3px
        }
    </style>

</head>
<body onload="invoke();" >
    <form id="form1" runat="server">

        <div class="container">
            <div class="row">
                <h4><asp:Label ID="Label1" CssClass="vModuleTitle" runat="server" Text="Job Order Settings"></asp:Label></h4>
                <hr />
            </div>
            <div class="row Mgin">
                <div class="col-md-2">Item Code :</div>
                <div class="col-md-4"><asp:TextBox ID="txtItemCd" runat="server" Width="" CssClass="form-control" ReadOnly="true"></asp:TextBox></div>
                <div class="col-md-2">BOM Code :</div>
                <div class="col-md-4"><asp:TextBox ID="txtBOMCd" runat="server" Width="" CssClass="form-control" ReadOnly="true"></asp:TextBox></div>
            </div>
        
            <div class="row Mgin">
                <div class="col-md-2">Item Description :</div>
                <div class="col-md-4"><asp:TextBox ID="txtItemName" runat="server" Width="" CssClass="form-control" ReadOnly="true"></asp:TextBox></div>
                <div class="col-md-2">BOM Revision :</div>
                <div class="col-md-4"><asp:TextBox ID="txtBOMRev" runat="server" Width="" CssClass="form-control" ReadOnly="true" ></asp:TextBox></div>
            </div>        
            <div class="row Mgin">
                <div class="col-md-2">Customer Code :</div>
                <div class="col-md-4"><asp:TextBox ID="txtCustomerCd" runat="server" Width="" CssClass="form-control" ReadOnly="true"></asp:TextBox></div>
                <div class="col-md-2">Job Order No. :</div>
                <div class="col-md-4"><asp:TextBox ID="txtJONo" runat="server" Width="" CssClass="form-control" ></asp:TextBox></div>
            </div>        
            <div class="row Mgin">
                <div class="col-md-2">Customer Name :</div>
                <div class="col-md-4"><asp:dropdownlist id="cmbCustomerName" runat="server" class="form-control" AutoPostBack="True"></asp:dropdownlist></div>
                <div class="col-md-2">Sales Order No :</div>
                <div class="col-md-4"><asp:TextBox ID="txtSONo" runat="server" Width="" CssClass="form-control" ></asp:TextBox></div>
            </div>        
            <div class="row Mgin">
                <div class="col-md-2">GCAS/Item Code :</div>
                <div class="col-md-4"><asp:TextBox ID="txtItemCatalog" runat="server" Width="" CssClass="form-control" ReadOnly="true"></asp:TextBox></div>
                <div class="col-md-2">Purchase Order No :</div>
                <div class="col-md-4"><asp:TextBox ID="txtPONo" runat="server" Width="" CssClass="form-control" ></asp:TextBox></div>
            </div>        
            <div class="row Mgin">
                <div class="col-md-2">Item Type :</div>
                <div class="col-md-4"><asp:TextBox ID="txtItemType" runat="server" Width="" CssClass="form-control" ReadOnly="true"></asp:TextBox></div>
                <div class="col-md-2">Quantity :</div>
                <div class="col-md-4"><asp:TextBox ID="txtQty" runat="server" Width="" CssClass="form-control" ></asp:TextBox></div>
            </div>
            <div class="row Mgin">
                <div class="col-md-2">Min Order QTY :</div>
                <div class="col-md-4"><asp:TextBox ID="txtMinOrder" runat="server" Width="" CssClass="form-control" ReadOnly="true"></asp:TextBox></div>
                <div class="col-md-2">Item UOM :</div>
                <div class="col-md-4"><asp:TextBox ID="txtItemUOM" runat="server" Width="" CssClass="form-control" ReadOnly="true" ></asp:TextBox></div>
            </div>
            <div class="row"><hr /></div>
            <div class="row Mgin">
                <div class="col-md-2">Job Order Status :</div> 
                <div class="col-md-4"><asp:dropdownlist id="cmbStatus" runat="server" class="form-control" AutoPostBack="True"></asp:dropdownlist></div> 
                <div class="col-md-2">Remarks :</div>
                <div class="col-md-4"><asp:TextBox ID="txtReason" runat="server" Width="" CssClass="form-control" ></asp:TextBox></div>
            </div>
            <div class="row Mgin">
                <div class="col-md-2">Due Date :</div> 
                <div class="col-md-4">
                    <asp:TextBox ID="txtDueDate" runat="server" Width="" CssClass="form-control" ></asp:TextBox>
                    <input type="hidden" id="h_DueDate" runat="server" style="width:80px;" />
                </div>
                <%--<div class="col-md-2">Kilos :</div> 
                <div class="col-md-4">
                    <asp:TextBox ID="txtKilos" runat="server" Width="" CssClass="form-control" ></asp:TextBox>
                </div>   --%> 
            </div>
            <div class="row Mgin">
                <div class="col-md-2">Start Date :</div> 
                <div class="col-md-4">
                    <asp:TextBox ID="txtSDate" runat="server" Width="" CssClass="form-control" ></asp:TextBox> 
                    <input type="hidden" id="h_SDate" runat="server" style="width:80px;" />
                </div> 
                <%--<div class="col-md-2">Meters :</div> 
                <div class="col-md-4">
                    <asp:TextBox ID="txtMeter" runat="server" Width="" CssClass="form-control" ></asp:TextBox>
                </div>--%>
            </div>
            <div class="row Mgin">
                <div class="col-md-2">Release Date :</div> 
                <div class="col-md-4">
                    <asp:TextBox ID="txtReleaseDate" runat="server" Width="" CssClass="form-control" ></asp:TextBox> 
                    <input type="hidden" id="h_ReleaseDate" runat="server" style="width:80px;" />
                </div>    
                <div class="col-md-2"></div>
                <div class="col-md-4"><input type="hidden" id="h_ItemType" runat="server" ReadOnly="true" /></div>
            </div>
            <div class="row"><hr /></div>
            <div class="row Mgin">
                <div class="col-md-2">&nbsp;</div>
                <div class="col-md-4">
                    <asp:Button ID="cmdSave" CssClass="btn btn-primary" runat="server"  Text="Save and Next" /> 
                    <asp:button id="cmdCancel" runat="server" CssClass="btn btn-primary" Text="Cancel" OnClientClick="self.close()"></asp:button>
                </div>
            </div>
        </div>

    <div style="width:100%; margin:auto; border:0px solid #000;">

        <%--<table id="Standard_Tbl" style="width:98%; margin:auto;" border="0"> 
            <tr>
                <td style="text-align:left; border-bottom: solid 1px #808080; padding-left:10px" colspan="4">
                    </td>
            </tr>  
        </table>
        <div class="col-md-12">&nbsp;</div>--%>
        
        <div class="col-md-6 text-right">
            
            
        </div> 
        <div class="col-md-6 text-right">
            
            
        </div>
        
        <div style="visibility:hidden;">
            Start Date Time :
            <asp:dropdownlist id="cmbHrs" runat="server" Width="80px"  class="labelL"></asp:dropdownlist> : 
            <asp:dropdownlist id="cmbMin" runat="server" Width="80px"  class="labelL"></asp:dropdownlist> HH:MM
        </div>
          
        <div class="col-md-2 text-right"><input type="hidden" id="Hidden3" runat="server" /></div>
        <div class="col-md-2 text-right"><input type="hidden" id="h_UOMCd" runat="server" /></div>  
        <div class="col-md-4"></div> 
        <div class="col-md-2 text-right">&nbsp;</div>
  
    </div>
    </form>
</body>
</html>
