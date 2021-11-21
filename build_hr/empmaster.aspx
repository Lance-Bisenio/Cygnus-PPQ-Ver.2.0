<%@ Page Language="VB" AutoEventWireup="false" CodeFile="empmaster.aspx.vb" Inherits="empmaster" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    
    <title></title>
    <link href="../css/BasicContol.css" rel="stylesheet" type="text/css" /> 
    <link href="../css/jquery_datatable.css" rel="stylesheet" /> 
    <link href="../css/colorbox.css" rel="stylesheet" />
    <link href="../css/jquery-ui.css" rel="stylesheet" />  

    <script src="../js/jquery-1.10.2.js" type="text/javascript"></script>
    <script src="../media/js/jquery.dataTables.js" type="text/javascript"> </script>
    <script src="../js/jquery.colorbox-min.js" type="text/javascript">  </script>

    <style type="text/css"> 
        .iDataFrame {
            width:99%; border: solid 0px #e2e2e2; height: 96%; margin:0px;
        }
    </style> 
    <script type="text/javascript"> 
         
        $(document).ready(function () {
            var vProperties = "width=1200px, height=700px, top=50px, left=80px, scrollbars=yes";
            var vParam = "&pWinType=Popup&id=206&pItemCd=" + $('#h_EmpCode').val() + "";
            var vDeleteParam = ""

            $('#btnItemAdd').click(function (event) {
                event.preventDefault();
                winPop = window.open("../global_forms/edit.aspx?pMode=new" + vParam + "", "popupWindow", vProperties);
                winPop.focus();
            });

            $('#btnItemEdit').click(function (event) { 
                event.preventDefault();
                winPop = window.open("../global_forms/edit.aspx?pMode=edit&pCode=" + $('#h_EmpCode').val() + vParam + "", "popupWindow", vProperties);
                winPop.focus();
            });

            $('#btnItemDel').click(function (event) {
                vDeleteParam = "&pType=Delete_Emp&pWinType=confirm&pTitle=Delete message alert&pMess=Employee"
                vParam = "&id=206&pTranId=" + $('#h_EmpCode').val() + "";

                event.preventDefault();
                winPop = window.open("../global_forms/confirm.aspx?mode=Delete_Item" + vParam + vDeleteParam + "",
                    "SmallWindow", "width=600px, height=300px, top=50px, left=80px, scrollbars=yes");
                winPop.focus();
            });
        });
    </script>

</head>
<body onload="invoke();">
    <form id="form1" runat="server"> 
    <div>
        <table id="Standard_Tbl" border="0" style="width:98%;">
             
            <tr>
                <td style="width: 100px;">Department :</td>
                <td style="width: 300px"> 
                    <asp:DropDownList ID="cmbDept" runat="server" Width="210px" CssClass="labelL" >
                    </asp:DropDownList>
                </td>
                
                <td style="width: 100px;">Position :</td>
                <td style="width: 300px">
                    <asp:DropDownList ID="cmbPos" runat="server" Width="210px" CssClass="labelL" >
                    </asp:DropDownList>
                    &nbsp;</td>
                
                <td style="width: 100px">&nbsp;</td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td>Section :</td>
                <td>
                    <asp:DropDownList ID="cmbSection" runat="server" Width="210px" CssClass="labelL" >
                    </asp:DropDownList>
                    </td>
                <td>Status :</td>
                <td>
                    <asp:DropDownList ID="cmbStatus" runat="server" Width="210px" CssClass="labelL">
                        <asp:ListItem Value="1" Selected="True">Active</asp:ListItem>
                        <asp:ListItem Value="0">Inactive</asp:ListItem>
                    </asp:DropDownList> 
                </td>
                <td>&nbsp;</td>
                <td><asp:TextBox ID="txtRefreshList" runat="server" Width="112px" BorderColor="White" BorderStyle="Solid" BorderWidth="1px" ReadOnly="True"></asp:TextBox></td>
            </tr>
            <tr><td style="height:10px;" colspan="6"></td></tr> 
            <tr>
                <td>Quick Search :</td>
                <td> 
                    <input type="hidden" id="h_EmpCode" runat="server" />
                    <input type="hidden" id="h_Mode" runat="server" /> 
                    <asp:TextBox ID="txtSearch" runat="server" CssClass="labelL" Width="204px" Height="18px"></asp:TextBox> 
                </td>
                <td>Search By :</td>
                <td colspan="2">
                    <asp:DropDownList ID="cmbSearchBy" runat="server" Width="210px" CssClass="labelL" >
                    </asp:DropDownList>
                    <asp:Button ID="cmdSearch" CssClass="Button" runat="server" Text="Search" />
                </td>
            </tr>
            <tr><td style="height:10px;"></td></tr> 
        </table>

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
                    <b><asp:Label ID="lblTotal" runat="server" CssClass="labelL" Text="Total Employee Retrieved : 0" ForeColor="#ffffff"></asp:Label></b>&nbsp;&nbsp;&nbsp;
                </td>
            </tr>
        </table>
        <asp:GridView ID="tbl_EmpMaster" runat="server" AllowPaging="True" BorderColor="#cccccc"
            AutoGenerateColumns="False" Width="100%" BorderStyle="Solid" BorderWidth="1px"
            CssClass="mainGridView" PageSize="15"> 
            <Columns> 
                <asp:CommandField ButtonType="Button" ShowSelectButton="True" > 
                    <ItemStyle CssClass="labelC" Width="40px" />
                    <ControlStyle CssClass="button" />
                </asp:CommandField>

                <%--<asp:TemplateField HeaderText="#" HeaderStyle-Width="30px" >   
                    <%--<ItemTemplate>
                        <%# Container.DataItemIndex + 1 %>   
                    </ItemTemplate>-- % >
                <HeaderStyle Width="30px"></HeaderStyle>
                </asp:TemplateField>--%>
                
                <asp:BoundField DataField="Emp_Cd" HeaderText="Emp Code" >
                    <ItemStyle CssClass="labelC" Width="90px" />
                </asp:BoundField>
                
                <asp:BoundField DataField="FullName" HeaderText="Employee Name" >
                    <ItemStyle CssClass="" />
                </asp:BoundField>
                              
                <asp:BoundField DataField="vDeptCd" HeaderText="Department" >
                    <ItemStyle CssClass="labelL" Width="180px" />
                </asp:BoundField>
                <asp:BoundField DataField="vSectionCd" HeaderText="Section" >
                    <ItemStyle CssClass="labelL" Width="180px" />
                </asp:BoundField>
                <asp:BoundField DataField="vProcessCd" HeaderText="Process" >
                    <ItemStyle CssClass="labelL" Width="180px" />
                </asp:BoundField>
                <asp:BoundField DataField="vPosCd" HeaderText="Position" >
                    <ItemStyle CssClass="labelL" Width="180px" />
                </asp:BoundField>

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
    </div> 
    </form>
</body>
</html>

    <%--<asp:TemplateField HeaderText="#" HeaderStyle-Width="30px" >   
                    <%--<ItemTemplate>
                        <%# Container.DataItemIndex + 1 %>   
                    </ItemTemplate>-- % >
                <HeaderStyle Width="30px"></HeaderStyle>
                </asp:TemplateField>--%>
