<%@ Page Language="VB" AutoEventWireup="false" CodeFile="item_sfg.aspx.vb" Inherits="item_sfg" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    
    <title></title>
    <link href="../css/BasicContol.css" rel="stylesheet" type="text/css" /> 
    <link href="../css/jquery_datatable.css" rel="stylesheet" /> 
    <link href="../css/colorbox.css" rel="stylesheet" />
    <link href="../css/jquery-ui.css" rel="stylesheet" />  

    <script src="../js/jquery-1.10.2.js" type="text/javascript"></script>
    <script src="../media/js/jquery.dataTables.js" type="text/javascript"> </script>
    <script src="../js/jquery.colorbox-min.js" type="text/javascript">  </script>

    <style type="text/css">
        /*html, body, form {
            overflow: hidden;
            margin: auto;
            height: 100%;
            width: 100%;
        }*/ 

        .iDataFrame {
            width:99%; border: solid 0px #e2e2e2; height: 96%; margin:0px;
        }
    </style> 

    <script type="text/javascript">

        
        //function ShowTabValue(vLink) {
        //    document.getElementById("frmContent").src = vLink; 
        //}
        
        //function OpenPopUp(vMode) {
        //    var vItem = document.getElementById("h_Selected").value;
        //    if (vMode == "btnEdit") {
        //        winpreview = window.open("item_general.aspx?mode=view&itemCd=" + vItem, "Add Item", "toolbar=no,scrollbars=no,top=100,left=100,height=400,width=800");
        //    } else {
        //        winpreview = window.open("item_general.aspx?mode=new", "Add Item", "toolbar=no,scrollbars=no,top=100,left=100,height=400,width=800");
        //    } 
        //    winpreview.focus(); 
        //    //('item_quantities.aspx?mode=view&itemCd=" & tblItemMaster.SelectedRow.Cells(1).Text & "'); "opener.document.form1.submit(); window.close();"
        //}
        //function DeleteItem() {
        //    var vItem = document.getElementById("h_Selected").value;
        //    var x;

        //    if (vItem == "") {
        //        alert("No item seleted");
        //    } else {
        //       var r = confirm("Are you sure you want to delete this item?");
        //        if (r == true) { 
        //            document.getElementById("h_Mode").value = "delete";
        //            form1.submit();
        //        }
        //    } 
        //} 
        //$(document).ready(function () {

        //    $('#table_id').dataTable({
        //        "sDom": '<"H"fr>tC<"F"ip>',
        //        "bJQueryUI": true, 
        //        "oColVis": {
        //            "buttonText": "&nbsp;",
        //            "bRestore": true,
        //            "sAlign": "left"
        //        }, 
        //        "searchable": true,
        //        "search.regex": true,
        //        "bPaginate": true,
        //        "iDisplayLength": 18,
        //        "sPaginationType": "full_numbers", 
        //    });

        //    var table = $('#table_id').DataTable();
        //    $('#table_id tbody').on('click', 'tr', function () {
        //        if ($(this).hasClass('selected')) {
        //            $(this).removeClass('selected');
        //        }
        //        else {
        //            table.$('tr.selected').removeClass('selected');
        //            $(this).addClass('selected');
        //            document.getElementById("h_Selected").value = $(this).find("td").eq(0).html();
        //        }
        //    });
        //    $('#button').click(function () {
        //        table.row('.selected').remove().draw(false);
        //    });

        //    $(document).ready(function () {
        //        $('#btnAdd').colorbox({ 'href': "item_general.aspx?mode=new", 'width': '80%', 'height': '80%;', iframe: true });
        //    });

        //    $('#btnEdit').on('click', function () {  
        //        $('#btnEdit').colorbox({ 'href': "item_general.aspx?mode=view&itemCd=" + $('#h_Selected').val() + "", 'width': '80%', 'height': '80%;', iframe: true });
        //    }); 
        //});

        function invoke() {
            <%=vScript %>
        }

        $(document).ready(function () {
            var vProperties = "width=1200px, height=700px, top=50px, left=80px, scrollbars=yes";
            var vParam = "&id=204&pTranId=" + $('#h_ItemCd').val() + "";
            var vDeleteParam = ""

            $('#btnItemAdd').click(function (event) {
                event.preventDefault();
                winPop = window.open("item_sfg_settings.aspx?pMode=new" + vParam + "", "popupWindow", vProperties);
                winPop.focus();
            });

            $('#btnItemEdit').click(function (event) {
                event.preventDefault();
                winPop = window.open("item_sfg_settings.aspx?pMode=edit" + vParam + "", "popupWindow", vProperties);
                winPop.focus();
            });
           
            $('#btnItemDel').click(function (event) { 
                vDeleteParam = "&pType=Delete_Sfg&pWinType=confirm&pTitle=Delete message alert&pMess=Item"
                vParam = "&id=204&pTranId=" + $('#h_ItemCd').val() + "";

                event.preventDefault();
                winPop = window.open("../global_forms/confirm.aspx?mode=Delete_Sfg" + vParam + vDeleteParam + "",
                    "SmallWindow", "width=600px, height=300px, top=50px, left=80px, scrollbars=yes");
                winPop.focus();
            });
        });
        
    </script>

</head>
<body onload="invoke();">
    <form id="form1" runat="server">
    <%--#include file="filters.aspx"--%>
    <div>
       <table id="Standard_Tbl" border="0" style="width:98%;">
            <%--<tr>
                <td class="labelR">Status :</td>
                <td class="labelL">
                    
                    </td>
                <td class="labelR">&nbsp;</td>
                <td class="labelL">
                    &nbsp;</td> 
                <td class="labelR">&nbsp;</td>
                <td class="labelL"> 
                    &nbsp;</td>
            </tr>--%>
            <tr>
                <td style="width:100px;" class="labelR">Status :</td>
                <td style="width:300px;"  class="labelL"> 
                    <asp:DropDownList ID="cmbItemStatus" runat="server" Width="210px" CssClass="labelL">
                        <asp:ListItem Value="1" Selected="True">Active</asp:ListItem>
                        <asp:ListItem Value="2">Inactive</asp:ListItem>
                    </asp:DropDownList> 
                </td>
                <td style="width:100px;" class="labelR"></td>
                <td style="width:300px;" class="labelL"></td>
                <td style="width:100px;" ></td>
                <td></td>
            </tr>
          <tr><td style="height:10px;" colspan="6"></td></tr> 
            <tr>
                <td class="labelR">Quick Search :</td>
                <td class="labelL"> 
                    <input type="hidden" id="h_EmpCode" runat="server" />
                    <input type="hidden" id="Hidden1" runat="server" /> 
                    <asp:TextBox ID="txtSearch" runat="server" CssClass="labelL" Width="204px" Height="18px"></asp:TextBox> 
                </td>
                <td class="labelR">Search By :</td>
                <td class="labelL" colspan="2">
                    <asp:DropDownList ID="cmbSearchBy" runat="server" Width="210px" CssClass="labelL" >
                    </asp:DropDownList>
                    <asp:Button ID="txtSearch_1" CssClass="Button" runat="server"  Text="Search" /> 
                    <input type="hidden" id="h_ItemCd" name="h_ItemCd" runat="server" style="width:45px"/>     
                    <input type="hidden" id="h_Mode" name="h_Mode" runat="server" style="width:45px"/>     
                    <input type="hidden" id="h_Sql" name="h_Mode" runat="server" style="width:45px"/>
                </td>
            </tr>
            
           <tr><td colspan="4">&nbsp;</td></tr>
        </table>

        <%------------------------------------------------------------------------------------------------------------------------------------------------%>
        <%--<table id="table_id" class="display" >
            <thead>
                <tr>  
                   <%-- <th style="width:40px; padding:0px; margin:0px;"> </th> - - %>
                    <% = vColHeader%> 
                </tr>
            </thead>
            <tbody> 
                <% = vEmpRecords%>
            </tbody>
        </table>--%>
        <%------------------------------------------------------------------------------------------------------------------------------------------------%>
    </div>

        <%--<div style="position:absolute; top: 130px; bottom:15px; left: 10px; right:10px; border:solid 0px #000000;
            overflow:auto; z-index:400;"> --%>
                    <table id="Table3" class="titleBarTop"  border="0" style="width:100%;  border-collapse:collapse;">
                        <tr style="border:solid 0px #cccccc; border-bottom:0px;">
                            <td style="width:320px; text-align:left;">
                                &nbsp;&nbsp;
                                    <input type="button" runat="server" id="btnItemAdd" name="btnItemAdd" value="Add"  class="tblButtonA" />  
                                    <input type="button" runat="server" id="btnItemEdit" name="btnItemEdit" value="Edit"  class="tblButtonE" />  
                                    <input type="button" runat="server" id="btnItemDel" name="" value="Delete" class="tblButtonD" />
                                
                                Show :<asp:DropDownList ID="cmbShow" runat="server" Width="70px" AutoPostBack="True" CssClass="labelL">
                                </asp:DropDownList> 

                            </td>
                            <td style="text-align:left; padding-left :5px;"></td>
                            <td class="labelR" >
                                <b><asp:Label ID="lblTotal" runat="server" CssClass="labelL" Text="Total Item Retrieved : 0" ForeColor="#ffffff"></asp:Label></b>&nbsp;&nbsp;&nbsp;
                            </td>
                        </tr>
                    </table>
                    <asp:GridView ID="tbl_ItemMaster" runat="server" AllowPaging="True" BorderColor="#cccccc"
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
                            <asp:BoundField DataField="TranId" HeaderText="Tran ID" HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn" ItemStyle-width="80px"/>
                            
                            <asp:BoundField DataField="SFG_ItemCd" HeaderText="SFG Item Code" >
                                <ItemStyle CssClass="labelC" Width="90px" />
                            </asp:BoundField>
                
                            <asp:BoundField DataField="SFGName" HeaderText="SFG Name" >
                                <ItemStyle CssClass="" />
                            </asp:BoundField>
                              
                            <asp:BoundField DataField="Mat_ItemCd" HeaderText="Mat Item Type" >
                                <ItemStyle CssClass="labelC" Width="120px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="MatName" HeaderText="Material Name" >
                                <ItemStyle CssClass="labelL" Width="500px" />
                            </asp:BoundField>
                             
                            <asp:TemplateField HeaderText="Grams">
                                <ItemTemplate>
                                    <asp:Label ID="Label1" runat="server" Text='<%# Format(Val(Eval("Grams").ToString.Replace(",", "")), "###,###,##0.00")%>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="labelR" Width="80px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Percentage">
                                <ItemTemplate>
                                    <asp:Label ID="Label1" runat="server" Text='<%# format(val(Eval("Percentage").tostring.replace(",","")),"###,###,##0.00") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="labelR" Width="80px" />
                            </asp:TemplateField>

                            <%--<asp:TemplateField HeaderText="Unit Cost">
                                <ItemTemplate>
                                    <asp:Label ID="Label1" runat="server" Text='<%# format(val(Eval("CurrCost").tostring.replace(",","")),"###,###,##0.00") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="labelR" Width="80px" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="Source" HeaderText="Source" >
                                <ItemStyle CssClass="labelL" Width="80px" />
                            </asp:BoundField>--%>
                        </Columns>
            
                        <SelectedRowStyle CssClass="activeBar" />
                        <PagerStyle Font-Size="8pt" /> 
                        <HeaderStyle CssClass="titleBar" />
                        <RowStyle CssClass="odd" />
                        <AlternatingRowStyle CssClass="even" />
                    </asp:GridView>
        <%--</div>--%>

                 
     <%--<table id="Table2" border="0" style="width:98%;  border-collapse:collapse; margin:auto;">
            <tr>
                <td style="width:70%">
                    
                </td>
                <td style="width:30%; vertical-align:top;">

                    <iframe id="frmContent" class="iDataFrame" src="" style=""></iframe>

                </td>
           </tr>
        </table> --%>

        <table id="Table2" border="0" style="width:98%;  border-collapse:collapse; margin:auto;">
        <%=vData %>
            </table>

    </form>
</body>
</html>
