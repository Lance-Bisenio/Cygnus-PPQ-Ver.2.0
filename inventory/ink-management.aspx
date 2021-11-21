<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ink-management.aspx.vb" Inherits="inkmanagement" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">
     
    <link href="../css/calendar/jquery-ui-1.10.4.custom.css" rel="stylesheet" />
    <link href="../css/bootstrap4/css/bootstrap.css" rel="stylesheet" />  
     <style>
         div { border: solid 0px; }
         body {
			font-family:Arial;
			font-size:12px;
    	 } 
         small { font-size:12px; padding-bottom:0px; margin-bottom:0px }
         #ui-datepicker-div {
             font-size:11px;
         }
     </style>
</head>
<body onload="invoke();" >
    <form id="form1" runat="server">
        <div class="container-fluid">

            <h3>Ink Mixing Management</h3>

            <div class="row">
                <div class="col-md-8">
                    
                    <div class="row"> 
                        <div class="col-md-4">
                            <div class="col-md-12">
                                <h5><small>Completion Date From</small></h5>
                                <asp:TextBox ID="txtDateFrom" runat="server" CssClass="form-control form-control-sm" ></asp:TextBox> 
                            </div>
                            <div class="col-md-12">
                                <h5><small>Completion Date To</small></h5>
                                <asp:TextBox ID="txtDateTo" runat="server" CssClass="form-control form-control-sm" ></asp:TextBox> 
                            </div> 
                        </div>

                        <div class="col-md-4">
                            <div class="col-md-12">
                                <h5><small>Search Item Code</small></h5>
                                <asp:TextBox ID="TxtItemCode" runat="server" CssClass="form-control form-control-sm" placeholder="Enter Item Code or Description" ></asp:TextBox> 
                            </div> 
                        </div>
                        <div class="col-md-4">
                        </div> 
                    </div>
                    
                    <div class="row">
                        <div class="col-md-4"> 
                            <div class="col-md-12"> 
                                <div class="btn-group btn-group-sm"> 
                                    <button type="button" class="btn btn-primary">Create Completion</button>
                                    <button type="button" class="btn btn-primary">Edit</button>
                                    <button type="button" class="btn btn-primary">Delete</button>
                                </div> 
                            </div>
                        </div>

                        <div class="col-md-4"> 
                            <div class="col-md-12"> 
                                <div class="btn-group btn-group-sm">
                                    <asp:Button ID="btnSearch" CssClass="btn btn-success" runat="server"  Text="Search" />     
                                    <button type="button" class="btn btn-primary">Dispatch to Production</button>
                                    <button type="button" class="btn btn-primary">Receive Return</button> 
                                </div> 
                            </div>
                        </div>
                        <div class="col-md-4">
                        </div> 
                    </div>
                    
                    <div class="row">
                        <div class="col-md-12">
                            <asp:GridView ID="tbl_Tasklist" runat="server" AllowPaging="True" BorderColor="#cccccc"
                                AutoGenerateColumns="False" Width="100%" BorderStyle="Solid" BorderWidth="1px"
                                CssClass="table table-bordered" PageSize="15">
                                <Columns>
                                    <asp:CommandField ButtonType="Button" ShowSelectButton="True" > 
                                        <ItemStyle CssClass="labelC" Width="40px" />
                                        <ControlStyle CssClass="btn btn-primary btn-sm" />
                                    </asp:CommandField>

                                    <asp:TemplateField HeaderText="#" HeaderStyle-Width="30px" >   
                                        <ItemTemplate>
                                            <%# Container.DataItemIndex + 1 %> 
                                        </ItemTemplate> 
                                        <HeaderStyle Width="30px"></HeaderStyle>
                                    </asp:TemplateField> 
                                    
                                    <asp:BoundField DataField="Item_Cd" HeaderText="Item Code" >  
                                        <ItemStyle CssClass="labelC" Width="80px" />
                                    </asp:BoundField>
                             
                                    <asp:BoundField DataField="Descr" HeaderText="Description" >  
                                        <ItemStyle CssClass="labelC"/>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Descr1" HeaderText="Other Info" >
                                        <ItemStyle CssClass="labelL"  Width="120px" />
                                    </asp:BoundField> 
                            
                            
                                </Columns> 
                                <SelectedRowStyle CssClass="activeBar" />
                                <PagerStyle Font-Size="8pt" /> 
                                <HeaderStyle CssClass="titleBar" />
                                <RowStyle CssClass="odd" />
                                <AlternatingRowStyle CssClass="even" />
                            </asp:GridView>
                        </div>
                    </div>


                </div>
                <div class="col-md-4">
                    <div class="row"> 
                        <div class="col-md-6">
                            <div class="col-md-12">
                                <h5><small>Dispatch Date From:</small></h5>
                                <asp:TextBox ID="TxtDsipatchFrom" runat="server" CssClass="form-control form-control-sm" ></asp:TextBox> 
                            </div>
                            <div class="col-md-12">
                                <h5><small>Dispatch Date To:</small></h5>
                                <asp:TextBox ID="TxtDsipatchTo" runat="server" CssClass="form-control form-control-sm" ></asp:TextBox> 
                            </div> 
                        </div>

                        <div class="col-md-6">
                            <div class="col-md-12">
                                <h5><small>Search Item Code</small></h5>
                                <asp:TextBox ID="TextBox3" runat="server" CssClass="form-control form-control-sm" placeholder="Enter Item Code or Description" ></asp:TextBox> 
                            </div> 
                        </div> 
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <div class="col-md-12">
                                <div class="btn-group btn-group-sm">
                                    <asp:Button ID="Button1" CssClass="btn btn-success" runat="server"  Text="Search" />     
                                    <button type="button" class="btn btn-primary">Dispatch to Production</button>
                                    <button type="button" class="btn btn-primary">Receive Return</button> 
                                </div> 
                            </div>
                        </div>
                    </div>

                </div>
                 
            </div>
        </div>



            <br />
            <div class="row">
                <div class="col-md-12">
                    
                </div> 
            </div> 
        </div>






    <div>
        <table id="Standard_Tbl" border="0" style="width:100%;  border-collapse:collapse;"> 
            <tr>
                <td style="width:100px;">Job Order Status :</td>
                <td style="width:300px;">
                    <asp:DropDownList ID="cmbStatus" runat="server" Width="210px" CssClass="custom-select-sm">
                    </asp:DropDownList>
                </td>
                <td style="width:120px;">Completion Date From :</td>
                <td style="width:300px;"> 
                    </td>
 
                <td style="width:100px;"></td>
                <td></td>
            </tr>
            <tr>
                <td>Customer Name :</td>
                <td>
                    <asp:DropDownList ID="cmbCustomer" runat="server" Width="210px" CssClass="labelL">
                    </asp:DropDownList>
                </td>
                <td>Completion Date To :</td>
                <td>  
                </td>
                <td>&nbsp;</td>
                <td> 
                    &nbsp;</td>
                <td></td>
                <td></td>
            </tr>
            <tr>
                <td>Item Type :</td>
                <td>
                    <asp:DropDownList ID="cmbItemType" runat="server" Width="210px" CssClass="labelL">
                    </asp:DropDownList>
                </td>
                <td><%--View Type By :--%></td>
                <td> 
                    <asp:DropDownList ID="cmbViewType" runat="server" Width="210px" CssClass="labelL" Visible="false"> 
                    </asp:DropDownList>
                </td>
                <td></td>
            </tr>
            <tr><td style="height:10px;" colspan="6"></td></tr>
                <tr>
                <td>Quick Search :</td>
                <td>  
                    <asp:TextBox ID="txtSearch" runat="server" CssClass="labelL" Width="204px" Height="18px"></asp:TextBox> 
                </td>
                <td>Search By :</td>
                <td colspan="4">
                    <asp:DropDownList ID="cmbSearchBy" runat="server" Width="210px" CssClass="labelL" >
                    </asp:DropDownList> 
                    
                    <asp:Button ID="btnViewDetails" CssClass="Button" runat="server"  Text="View JO Materials" visible="false" />
                    <input type="button" runat="server" id="btnDupBOM" name="btnDupBOM" value="Duplicate BOM" class="Button" data-href="divPop" visible="false" />&nbsp;
                   
                    <input type="hidden" id="h_TranId" runat="server" />
                    <input type="hidden" id="h_BOM" runat="server"/>
                    <input type="hidden" id="h_BOMRev" runat="server"/>
                    <input type="hidden" id="h_JO" runat="server"/>
                    <input type="hidden" id="h_Mode" runat="server"/> 
                    <input type="hidden" id="h_Sql" runat="server" style="width:45px"/>
                </td>
            </tr>
        </table><br />
        <table style="width:100%; margin:auto; border: solid 0px; border-collapse:collapse;" border="0">
            <tr>
                <td>
                    <table id="Table1" class="titleBarTop"  border="0" style="width:100%;  border-collapse:collapse;">
                        <tr style="border:solid 0px #cccccc; border-bottom:0px;">
                            <td style="width:320px; text-align:left;">
                                &nbsp;&nbsp;
                                    <%--<input type="button" runat="server" id="btnHeaderAdd" name="btnProAdd" value="Add"  class="tblButtonA" />  
                                    <input type="button" runat="server" id="btnHeaderEdit" name="btnProEdit" value="Edit"  class="tblButtonE" />  
                                    <input type="button" runat="server" id="btnHeaderDel" name="" value="Delete" class="tblButtonD" data-href="divPop" />--%>Show :<asp:DropDownList ID="cmbShow" runat="server" Width="70px" AutoPostBack="True" CssClass="labelL">
                                </asp:DropDownList> 

                            </td>
                            <td style="text-align:left; padding-left :5px;"></td>
                            <td class="labelR" >
                                <b><asp:Label ID="lblTotalRecords" runat="server" CssClass="labelL" Text="Documents Retrieved : 0" ForeColor="White"></asp:Label></b>&nbsp;&nbsp;&nbsp;
                            </td>
                        </tr>
                    </table>
                     
                   
                        
                    
                   
                    <br />
                </td>
            </tr> 
        </table>
    </div>
	<br />
		<br />
		<br />
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
    function OpenAllMaterials(pTranId, pBOM, pBOMRev, pJO) {
              
        var vProperties = "width=1200px, height=700px, top=50px, left=80px, scrollbars=yes";
        var JODetails = window.open("../inventory/taskdetails.aspx?pTranId=" +
            pTranId + "&pBom=" +
            pBOM + "&pBomRev=" +
            pBOMRev + "&pJO=" +
            pJO + "", "popupWindow-JODetails", vProperties);
        JODetails.focus();
    }

    $(document).ready(function () {
        $("#txtDateFrom").datepicker();
        $("#txtDateTo").datepicker();
        $("#TxtDsipatchFrom").datepicker();
        $("#TxtDsipatchTo").datepicker();
        
    });
</script>