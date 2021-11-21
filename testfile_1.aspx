<%@ Page Language="VB" AutoEventWireup="false" CodeFile="testfile_1.aspx.vb" Inherits="testfile_1" %>

<!DOCTYPE html>

<html lang="en">
<head>
<meta charset="utf-8" />
<title>TEST FILE</title> 
    <link href="css/BasicContol.css" rel="stylesheet" type="text/css" />

</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table id="Standard_Tbl" border="0" style="width:100%;  border-collapse:collapse;"> 
            <tr>
                <td style="width:100px;">Job Order Status :</td>
                <td style="width:300px;">
                    <asp:DropDownList ID="cmbStatus" runat="server" Width="210px" CssClass="labelL">
                    </asp:DropDownList>
                </td>
                <td style="width:100px;">Month / Year :</td>
                <td style="width:300px;"> 
                    <asp:DropDownList ID="cmbMonth" runat="server" Width="120px" CssClass="labelL">
                        <asp:ListItem Value="01">January</asp:ListItem> 
                        <asp:ListItem Value="02">February</asp:ListItem> 
                        <asp:ListItem Value="03">March</asp:ListItem> 
                        <asp:ListItem Value="04">April</asp:ListItem> 
                        <asp:ListItem Value="05">May</asp:ListItem> 
                        <asp:ListItem Value="06">June</asp:ListItem> 
                        <asp:ListItem Value="07">July</asp:ListItem> 
                        <asp:ListItem Value="08">August</asp:ListItem> 
                        <asp:ListItem Value="09">September</asp:ListItem> 
                        <asp:ListItem Value="10">October</asp:ListItem> 
                        <asp:ListItem Value="11">November</asp:ListItem>  
                        <asp:ListItem Value="12">December</asp:ListItem>  
                    </asp:DropDownList>
                    <asp:DropDownList ID="cmbYear" runat="server" Width="87px" CssClass="labelL"> 
                    </asp:DropDownList>
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
                <td>View Type By :</td>
                <td> 
                    <asp:DropDownList ID="cmbViewType" runat="server" Width="210px" CssClass="labelL">
                        <asp:ListItem Value="DueDate">Due Date</asp:ListItem> 
                        <asp:ListItem Value="StartDate">Start Date</asp:ListItem> 
                        <asp:ListItem Value="ReleaseDate">Release Date</asp:ListItem>  
                    </asp:DropDownList>
                </td>
                <td></td>
                <td></td>
            </tr>
            <tr>
                <td>Item Type :</td>
                <td>
                    <asp:DropDownList ID="cmbItemType" runat="server" Width="210px" CssClass="labelL">
                    </asp:DropDownList>
                </td>
                <td></td>
                <td></td>
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
                    <input type="button" runat="server" id="btnDupBOM" name="btnDupBOM" value="Duplicate BOM" class="Button" data-href="divPop" visible="false" />
                    <input type="button" runat="server" id="btnJO" name="btnDupBOM0" value="Create Job Order" class="Button" visible="false" />
                    <input type="hidden" id="h_TranId" />
                    <input type="hidden" id="h_BOM" />
                    <input type="hidden" id="h_BOMRev" />
                </td>
            </tr>
        </table><br />
            <table id="Table1" class="titleBarTop"  border="0" style="width:100%;  border-collapse:collapse;">
                <tr style="border:solid 0px #cccccc; border-bottom:0px;">
                    <td style="width:320px; text-align:left;"></td>
                    <td style="text-align:left; padding-left :5px;"></td>
                    <td class="labelR" >
                        <b><asp:Label ID="lblTotalDocs" runat="server" CssClass="labelL" Text="Record Retrieved : 0" ForeColor="#ffffff"></asp:Label></b>&nbsp;&nbsp;&nbsp;
                    </td>
                </tr>
           </table>
           <table style="width:100%; border-collapse:collapse; border-color:#eeeded; border:solid 1px #eeeded;" border="1"  > 
                <tr class="titleBar" >
                    <td>ID</td>
                    <td>ItemCd</td>
                    <td>Desc1</td>

                    <td>GCAS</td>
                    <td>Mat Specs</td>
                    <td>Dimension</td>
                    <td>MinOrder Qty</td>
					<td>MinOrder UOM</td>
					<td>UOM</td>
					<td>Count</td>
                </tr>
                <%=vData  %>
            </table>
        </div> 
    </form>
</body>
</html>
