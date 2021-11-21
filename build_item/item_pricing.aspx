<%@ Page Language="VB" AutoEventWireup="false" CodeFile="item_pricing.aspx.vb" Inherits="item_pricing" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="css/BasicContol.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function invoke() {
            <%=vScript %>
        }
        function showSettings(vType) {
            id = 1080;
            s = 0;
            linkwin = window.open('maintenance.aspx?id=' + id + "&s=" + s,
           'viewprop', 'top=80,left=100,scrollbars=yes,resizable=yes,toolbars=no,width=850,height=550');
            linkwin.focus();
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
                    <asp:Label ID="txtTitle" CssClass="vModuleTitle"  runat="server" Text="Pricing"></asp:Label></td>

            </tr>
           <tr><td>&nbsp;</td><td></td></tr>
            <tr>
                <td>Wholesale Price :</td>
                <td>
                    <asp:TextBox ID="txtWSPrice" runat="server" Width="100px" CssClass="label" >0.00</asp:TextBox> / 
                    <asp:DropDownList ID="cmbWholeSale_Uom" runat="server" Width="100px" CssClass="labelL">
                    </asp:DropDownList>
                    <img src="images/settings.png" style="vertical-align:middle; cursor:pointer;" id="1080_1" alt="" onclick="showSettings(id);"/>
                </td>
                <td>Discount Factor :</td>
                <td>
                    <asp:TextBox ID="txtWSDisFac" runat="server" Width="100px" CssClass="label" >0</asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>Retail Price :</td>
                <td>
                    <asp:TextBox ID="txtRetailPrice" runat="server" Width="100px" CssClass="label" >0.00</asp:TextBox> /
                    <asp:DropDownList ID="cmbRetail_Uom" runat="server" Width="100px" CssClass="labelL">
                    </asp:DropDownList>
                    <img src="images/settings.png" style="vertical-align:middle; cursor:pointer;" id="1080_2" alt="" onclick="showSettings(id);"/>
                </td>
                <td>Discount Factor :</td>
                <td>
                    <asp:TextBox ID="txtRetailDisFac" runat="server" Width="100px" CssClass="label" >0</asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>Current Cost :</td>
                <td>
                    <asp:TextBox ID="txtCurrCost" runat="server" Width="100px" CssClass="label" >0.00</asp:TextBox>
                </td>
                <td>Average Cost :</td>
                <td>
                    <asp:TextBox ID="txtAveCost" runat="server" Width="100px" CssClass="label" >0.00</asp:TextBox>
                </td>
            </tr>
            <tr>
                <td></td>
                <td>
                    
                    <asp:Button ID="txtSave" CssClass="Button" runat="server"  Text="Save" />
                    <asp:Button ID="txtClose" CssClass="Button" runat="server"  Text="Close" />     
                </td>
                <td>&nbsp;</td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td colspan="3">
                    
                    &nbsp;</td>
                <td></td>
            </tr>
            <tr><td>&nbsp;</td></tr>
        </table>
    </div>
    </form>
</body>
</html>
