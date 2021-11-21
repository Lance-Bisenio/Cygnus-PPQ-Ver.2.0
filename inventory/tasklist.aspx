<%@ Page Language="VB" AutoEventWireup="false" CodeFile="tasklist.aspx.vb" Inherits="inventory_tasklist" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    
    
    
    <link href="../css/BasicContol.css" rel="stylesheet" type="text/css" />
    <link href="../css/calendar/jquery-ui-1.10.4.custom.min.css" rel="stylesheet" />
    <%--<link href="../css/calendar/jquery-ui-1.10.4.custom.css" rel="stylesheet" />--%>
    <style>
        .submenu {
            height:25px; width:160px; padding-left:0px; text-align:left; font-size:12px; color:#000; background:#e0e0e0; 
            border:0px solid #000; cursor:pointer; font-weight:bold;
        }

        .submenu:hover {
            color:#0094ff;  
        }

        .submenu_counter {
            width:30px; 
            
            text-align:center; 
            font-size:10px; 
            font-weight:bold; 
            color:#fff; 
            background:#cd3c02; 
            
            border:0px solid #882903; 
            
            /*
                padding:3px; 
                margin-top:0px; 
                -moz-border-radius: 5px 5px 5px 5px;
                -webkit-border-radius: 5px 5px 5px 5px;
            */
        }
    </style>
    

</head>
<body onload="invoke();" >
    <form id="form1" runat="server">
        <input type="hidden" id="hDFrom" value="" runat="server" /> 
        <input type="hidden" id="hDTo" value="" runat="server" /> 


    <div>
        <table id="Standard_Tbl" border="0" style="width:100%;  border-collapse:collapse;"> 
            <tr>
                <td style="width:100px;">Job Order Status :</td>
                <td style="width:300px;">
                    <asp:DropDownList ID="cmbStatus" runat="server" Width="210px" CssClass="labelL">
                    </asp:DropDownList>
                </td>
                <td style="width:120px;">Release Date From :</td>
                <td style="width:300px;"> 
                    <asp:TextBox ID="txtDateFrom" runat="server" CssClass="labelL" Width="100px" Height="18px"></asp:TextBox> 
                    <img src="../images/calendar.png" class="DatePicker" style="vertical-align:middle;" alt="" /></td>
 
                <td style="width:100px;"></td>
                <td></td>
            </tr>
            <tr>
                <td>Customer Name :</td>
                <td>
                    <asp:DropDownList ID="cmbCustomer" runat="server" Width="210px" CssClass="labelL">
                    </asp:DropDownList>
                </td>
                <td>Release Date To :</td>
                <td> 
                    <asp:TextBox ID="txtDateTo" runat="server" CssClass="labelL" Width="100px" Height="18px"></asp:TextBox> 
                    <img src="../images/calendar.png" class="DatePicker" style="vertical-align:middle;" alt="" />
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
                    <asp:Button ID="btnSearch" CssClass="Button" runat="server"  Text="Search" />    
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
                     
                   
                        
                    <asp:GridView ID="tbl_Tasklist" runat="server" AllowPaging="True" BorderColor="#cccccc"
                        AutoGenerateColumns="False" Width="100%" BorderStyle="Solid" BorderWidth="1px"
                        CssClass="mainGridView" PageSize="15">
                        <Columns>
                            <asp:CommandField ButtonType="Button" ShowSelectButton="True" > 
                                <ItemStyle CssClass="labelC" Width="40px" />
                                <ControlStyle CssClass="button" />
                            </asp:CommandField>

                            <asp:TemplateField HeaderText="#" HeaderStyle-Width="30px" >   
                                <ItemTemplate>
                                    <%# Container.DataItemIndex + 1 %> 
                                </ItemTemplate> 
                                <HeaderStyle Width="30px"></HeaderStyle>
                            </asp:TemplateField> 

                            <asp:BoundField DataField="TranId" HeaderText="Tran ID" >  
                                <ItemStyle CssClass="labelC" Width="80px" />
                            </asp:BoundField>
                            
                            <asp:BoundField DataField="BOM_Cd" HeaderText="BOM Code" HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn" ItemStyle-width="80px"/>

                            <asp:BoundField DataField="BOMRev" HeaderText="Revision" HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn" ItemStyle-width="80px"/>
                             
                            <asp:BoundField DataField="JobOrderNo" HeaderText="JO NO." >  
                                <ItemStyle CssClass="labelC" Width="80px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Alt_Cd" HeaderText="GCAS" >
                                <ItemStyle CssClass="labelL" Width="80px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Item_Cd" HeaderText="Item Code" >
                                <ItemStyle CssClass="labelL" Width="80px" />
                            </asp:BoundField>
                                <asp:BoundField DataField="Item_Name" HeaderText="Item Description" >
                                <ItemStyle CssClass="labelL" />
                            </asp:BoundField>  
                            <asp:BoundField DataField="vItemType" HeaderText="Item Type" >
                                <ItemStyle CssClass="labelC" Width="80px" />
                            </asp:BoundField> 
                             <asp:BoundField DataField="OrderQty" HeaderText="QTY Order" >
                                <ItemStyle CssClass="labelC" Width="80px" />
                            </asp:BoundField> 
                            <asp:BoundField DataField="vUom" HeaderText="Item UOM" >
                                <ItemStyle CssClass="labelC" Width="80px" />
                            </asp:BoundField>  
                            <asp:BoundField DataField="vItemRequest" HeaderText="Item Request" >
                                <ItemStyle CssClass="labelC" Width="80px" ForeColor="Red" />
                            </asp:BoundField>  
                            <asp:BoundField DataField="vItemReturn" HeaderText="Item Return" >
                                <ItemStyle CssClass="labelC" Width="80px" ForeColor="Red" />
                            </asp:BoundField>  
                            
                            
                        </Columns> 
                        <SelectedRowStyle CssClass="activeBar" />
                        <PagerStyle Font-Size="8pt" /> 
                        <HeaderStyle CssClass="titleBar" />
                        <RowStyle CssClass="odd" />
                        <AlternatingRowStyle CssClass="even" />
                    </asp:GridView>
                   
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


<%--<script src="../js/jquery-ui-1.10.4.custom.js"></script>
<script src="../js/jquery-ui-1.10.4.custom.min.js"></script>--%>
<script src="../js/jquery-1.10.2.js"></script>
<script src="../js/jquery-ui.js"></script>
<script>
    function invoke() {
        <%=vScript %>
    }
    //function OpenAllMaterials(pTranId, pBOM, pBOMRev, pJO) {
              
    //    var vProperties = "width=1200px, height=700px, top=50px, left=80px, scrollbars=yes";
    //    var JODetails = window.open("../inventory/taskdetails.aspx?pTranId=" +
    //        pTranId + "&pBom=" +
    //        pBOM + "&pBomRev=" +
    //        pBOMRev + "&pJO=" +
    //        pJO + "", "popupWindow-JODetails", vProperties);
    //    JODetails.focus();
    //}

    $(document).ready(function () {
        $("#txtDateFrom").datepicker();
        $("#txtDateTo").datepicker();
    });
</script>