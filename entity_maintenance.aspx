<%@ Page Language="VB" AutoEventWireup="false" CodeFile="entity_maintenance.aspx.vb" Inherits="entity_maintenance" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="css/BasicContol.css" rel="stylesheet" type="text/css" /> 


    <script type="text/javascript">
        function ShowTabValue(vLink) {
            document.getElementById("frmContent").src = vLink;

        }
        function invoke() {
            
            <%=vScript %>
        } 
    </script>
</head>
<body class="PopUp" onload="invoke();">
    <form id="form1" runat="server">
    <table style="margin-bottom:10px;">
        <thead>
            <tr><td class="labelTilteL"> <%=vEntityTitle %></td></tr>
        </thead>
    </table>
    <div class="DivBorder">
        <table style="width:100%; margin-top:10px; margin-bottom:10px;" >
            <%=vEntity %>
        </table>
    </div>
    </form>
</body>
</html>
