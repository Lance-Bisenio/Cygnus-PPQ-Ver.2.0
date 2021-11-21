<%@ Page Language="VB" AutoEventWireup="false" CodeFile="materials-return-form.aspx.vb" Inherits="inventory_materials_request_form" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>

    <link href="../css/BasicContol.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-ui-1.10.4.custom.js"></script>
    <script src="../js/jquery-ui-1.10.4.custom.min.js"></script>
    <script src="../js/jquery-1.10.2.js"></script>
    <script src="../js/jquery-ui.js"></script>
     
    <link href="../css/calendar/jquery-ui-1.10.4.custom.min.css" rel="stylesheet" />
    <link href="../css/calendar/jquery-ui-1.10.4.custom.css" rel="stylesheet" />

    <style type="text/css"> 
        input {
            font-family:Arial;
            height:25px;
            padding:2px;
            font-size: 13px;
            font-weight:100;
            padding-left:8px;
        }

        select { 
            font-size: 14px;
            height:31px !important;
            cursor:pointer; 
            padding-left: 4px;
        }
         
    </style> 
     <script>
        function invoke() {
            <%=vScript %>
        }


         $(document).ready(function () {
             $("#txtQty").bind('keyup change click', function () {

                 if ($.trim($(this).val()) != "") {
                     if ($(this).val().match(/^\d+$/)) {
                     } else {
                         //alert("Not a number");
                         $(this).val($(this).val().replace(/[^0.-9]+/gi, ""));
                     }
                 }
             });


             $("#txtWeight").bind('keyup change click', function () {

                 if ($.trim($(this).val()) != "") {
                     if ($(this).val().match(/^\d+$/)) {
                     } else {
                         //alert("Not a number");
                         $(this).val($(this).val().replace(/[^0.-9]+/gi, ""));
                     }
                 }
             });
         });

    </script>
</head>
<body onload="invoke();" >
    <form id="form1" runat="server">
    <div style="width:100%;"><br />
        <table id="Standard_Tbl" style="width:100%;" border="0">
 
            <tr>
                <td>Material Code :</td>
                <td><asp:TextBox ID="txtItemCd" runat="server" Width="350px" ReadOnly="true"></asp:TextBox></td> 
            </tr>
     
            <tr>
                <td>Description :</td> 
                <td>
                    <asp:dropdownlist id="cmbMaterials" runat="server" Width="90%" Height="35px" CssClass="DropDownList" AutoPostBack="True"></asp:dropdownlist>
                </td> 
            </tr> 
            <tr><td>&nbsp;</td></tr>
            <tr>
                <td>Quantity :</td> 
                <td><asp:TextBox ID="txtQty" runat="server" Width="350px" AutoComplete="off" /> &nbsp; 
                    <b>Quantity Received : <asp:Label ID="lblQtyReceived" runat="server" Font-Size="16px" Text="0.00"></asp:Label></b></td>
            </tr>
            <tr style="visibility:hidden;">
                <td>Net Weight :</td> 
                <td><asp:TextBox ID="txtWeight" runat="server" Width="350px" AutoComplete="off" >0</asp:TextBox>
                </td> 
            </tr>
            <tr><td>&nbsp;</td></tr>
              
             <tr>
                <td></td>
                <td>
                    <asp:Button id="cmdSave" runat="server" Text="Save" Height="30px" ></asp:Button>  
                    <asp:button id="cmdCancel" runat="server" CssClass="button" Text="Cancel" Height="30px" OnClientClick="self.close()"></asp:button> 
                </td> 
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
