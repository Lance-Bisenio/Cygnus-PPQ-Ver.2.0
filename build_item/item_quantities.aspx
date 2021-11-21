<%@ Page Language="VB" AutoEventWireup="false" CodeFile="item_quantities.aspx.vb" Inherits="item_quantities" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="css/BasicContol.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function invoke() {
            <%=vScript %>
        }
    </script>
</head>
<body style="background-color:#e7e7e7" onload="invoke();">
    <form id="form1" runat="server">
    <div class="SmallBox_Frame">
        
        
        <div id="divErrorDis" runat="server" visible="false" style="width:99%; border: solid 2px #dc0606; background:#fb9d9d; color:#000000; padding-left:5px; text-align:left;">
            <table>
                <tr>
                    <td style="width:25px;"><img src="images/alert.png" style="width:19px; height:24px;"></img></td>
                    <td style="text-shadow: 1px 1px 1px #e7e7e7; font-weight:bold;"><%=vErrorMsg %></td>
                </tr>
            </table>
        </div>



        <table id="Standard_Tbl" style="width:100%;" border="0">
            <tr>
                <td style="text-align:left; border-bottom: solid 1px #808080;" colspan="4">
                    <asp:Label ID="txtTitle" CssClass="vModuleTitle" runat="server" Text="Quantities"></asp:Label></td>

            </tr>
           <tr><td>&nbsp;</td><td></td></tr>
            <tr>
                <td style="width:130px;">Ceilling :</td>
                <td style="width:300px;">
                    <asp:TextBox ID="txtCeilling" runat="server" Width="100px" CssClass="label" >0</asp:TextBox>
                </td>
                <td>Current Qty :</td>
                <td>
                    <asp:TextBox ID="txtCurrQty" runat="server" Width="100px" CssClass="label" >0</asp:TextBox>
                &nbsp;/
                    <asp:DropDownList ID="cmbUom" runat="server" Width="100px" CssClass="labelL">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>Floor :</td>
                <td>
                    <asp:TextBox ID="txtFloor" runat="server" Width="100px" CssClass="label" >0</asp:TextBox>
                </td>
                <td>Unserved Qty :</td>
                <td>
                    <asp:TextBox ID="txtUnserved" runat="server" Width="100px" CssClass="label" >0</asp:TextBox>
                </td>
            </tr>
            
            <tr>
                <td>&nbsp;</td>
                <td colspan="2">
                    
                    <asp:Button ID="txtSave" CssClass="Button" runat="server"  Text="Save" />
                    <asp:Button ID="txtClose" CssClass="Button" runat="server"  Text="Close" />     
                </td>
                <td></td>
            </tr>
            <tr><td>&nbsp;</td></tr>
            <tr>
                <td style="text-align:left; border-bottom: solid 1px #808080;" colspan="4">
                    <asp:Label ID="Label1" CssClass="vModuleTitle" runat="server" Text="Item Conversion"></asp:Label></td>

            </tr>
            <tr><td>&nbsp;</td><td></td></tr>
            <tr>
                <td>Convesion Factor :</td>
                <td>
                    <asp:TextBox ID="txtConvFac" runat="server" Width="100px" CssClass="label"  Enabled="false">0</asp:TextBox>
                </td>
                <td> </td>
                <td> </td>
            </tr>
            <tr>
                <td>Unit of Measurement :</td>
                <td>
                    <asp:DropDownList ID="cmbConvUom" runat="server" Width="106px" Enabled="false" CssClass="labelL"></asp:DropDownList>
                </td>
                <td> </td>
                <td> </td>
            </tr>
            <tr>
                <td></td>
                <td colspan="2">
                    <asp:Button ID="txtQtyDetails" CssClass="Button" runat="server"  Text="Qty Details" />
                    <asp:Button ID="txtNewConv" CssClass="Button" runat="server"  Text="Add Conversion" />
                    <asp:Button ID="txtEditConv" CssClass="Button" runat="server"  Text="Edit" />
                    <asp:Button ID="txtDel" CssClass="Button" runat="server"  Text="Delete" />
                    <asp:Button ID="txtConvClose" CssClass="Button" runat="server"  Text="Close" Visible="False" />     
                </td>
            </tr>
            <tr><td>&nbsp;</td></tr>
            <tr>
                <td colspan="4" class="labelC">

                    <asp:GridView ID="tblItemConv" runat="server" AllowPaging="True" BorderColor="#8B8B8A"
                        AutoGenerateColumns="False" Width="100%" BorderStyle="Solid" BorderWidth="1px"
                        CssClass="mainGridView" EnableModelValidation="True">
                        <Columns>
                            <asp:TemplateField HeaderText="#" HeaderStyle-Width="30px" >   
                                <ItemTemplate>
                                    <%# Container.DataItemIndex + 1 %>   
                                </ItemTemplate>

                            <HeaderStyle Width="30px"></HeaderStyle>
                                <ItemStyle CssClass="labelC" />
                            </asp:TemplateField>

                            <asp:BoundField DataField="ToUomCd" HeaderText="UOM Code" >
                                <ItemStyle CssClass="labelC" />
                            </asp:BoundField>

                            <asp:BoundField DataField="FactorName" HeaderText="UOM Description" >
                                <ItemStyle CssClass="labelC" />
                            </asp:BoundField>
                
                            <asp:BoundField DataField="Factor" HeaderText="Multiply By" >
                                <ItemStyle CssClass="labelC" />
                            </asp:BoundField>
                
                            <asp:CommandField ButtonType="Button" ShowSelectButton="True" >
                                <ControlStyle CssClass="Button" />
                                <ItemStyle CssClass="labelC" />
                            </asp:CommandField>
                
                        </Columns>
            
                        <SelectedRowStyle CssClass="activeBar" />
                        <PagerStyle CssClass="label" Font-Size="8pt" BorderStyle="None" />
                        <RowStyle CssClass="odd" />
                        <HeaderStyle CssClass="titleBar" />
                        <AlternatingRowStyle CssClass="even" />
                    </asp:GridView>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
