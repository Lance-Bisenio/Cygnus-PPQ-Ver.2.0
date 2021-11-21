<%@ Page Language="VB" AutoEventWireup="false" CodeFile="item_sfg_settings.aspx.vb" Inherits="build_item_item_sfg_settings" %>

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
        
    <script type="text/javascript">
        function invoke() {
            <%=vScript %>
        }
    </script> 
</head>
<body onload="invoke();">

    <form id="form1" runat="server">
    <div>
        <table id="Standard_Tbl" style="width:100%;" border="0"> 
            <tr>
                <td style="text-align:left; border-bottom: solid 1px #808080; padding-top:5px" colspan="4">
                    <asp:Label ID="txtTitle" CssClass="vModuleTitle" runat="server" Text="SFG Item Settings"></asp:Label></td>
            </tr>
            <tr><td>&nbsp;</td></tr>
            <tr>
                <td style="width:150px;">SFG Item Code :</td>
                <td style="width:550px; vertical-align:top;">
                    <asp:TextBox ID="txtSFGItemCd" runat="server" Width="100px" Height="18px" CssClass="labelL" ></asp:TextBox>
                    <asp:Button ID="btnFindSFG" CssClass="Button" runat="server"  Text="Find" />
                </td>
                <td style="width:120px;"> </td>
                <td></td>
            </tr>
            
            <tr> 
                <td>SFG Description :</td>
                <td>
                    <asp:DropDownList ID="cmbSFGDescr" runat="server" Width="485px" CssClass="labelL" AutoPostBack="True"></asp:DropDownList>
                    <img src="../images/settings.png" style="vertical-align:middle; cursor:pointer;" id="Img2" alt="" onclick="showSettings(id);"/></td>

                <td></td>
                <td></td>
            </tr>
            <tr><td colspan="4">&nbsp;</td></tr>
            <tr>
                <td style="width:150px;">Material Item Code :</td>
                <td style="width:550px; vertical-align:top;">
                    <asp:TextBox ID="txtMatItemCd" runat="server" Width="100px" Height="18px" CssClass="labelL" ></asp:TextBox>
                    <asp:Button ID="btnFindMat" CssClass="Button" runat="server"  Text="Find" />
                </td> 
                <td></td>
                <td></td>
            </tr>
            <tr>
                <td>Material Description :</td>
                <td >
                    <asp:DropDownList ID="cmbMatDescr" runat="server" Width="485px" CssClass="labelL" AutoPostBack="True"></asp:DropDownList>
                    <img src="../images/settings.png" style="vertical-align:middle; cursor:pointer;" id="Img1" alt="" onclick="showSettings(id);"/></td>
                <td></td>
                <td></td>
            </tr>
            <tr>
                <td style="width:150px;">Grams :</td>
                <td style="width:550px; vertical-align:top;">
                    <asp:TextBox ID="txtGrams" runat="server" Width="100px" Height="18px" CssClass="labelL" ></asp:TextBox> 
                </td> 
                <td></td>
                <td></td>
            </tr>
            <tr>
                <td style="width:150px;">Percentage :</td>
                <td style="width:550px; vertical-align:top;">
                    <asp:TextBox ID="txtPercentage" runat="server" Width="100px" Height="18px" CssClass="labelL" ></asp:TextBox> 
                </td> 
                <td></td>
                <td></td>
            </tr>
            <tr><td colspan="4">&nbsp;</td></tr>

            <tr>
                <td></td>
                <td colspan="3">
                    <asp:Button ID="cmdSave" CssClass="Button" runat="server"  Text="Save" />
                    <asp:Button ID="txtClose" CssClass="Button" runat="server"  Text="Close" />  
                    <asp:Label ID="lblErrorMsg" runat="server" Text="Label" CssClass="labelL" Font-Bold="True" Font-Size="12px" ForeColor="#CC0000" Visible="False"></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
