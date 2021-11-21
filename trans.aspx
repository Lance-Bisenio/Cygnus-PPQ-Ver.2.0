<%@ Page Language="VB" AutoEventWireup="false" CodeFile="trans.aspx.vb" Inherits="trans" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Detailed Transactions Page</title>
   
      <link href="css/BasicContol.css" rel="stylesheet" />
    <script language="javascript" type="text/javascript">
        function invoke() {
            <%=vScript %>
        }
    </script>
    <style type="text/css">
    
        .Tbl_ {
            padding: 5px; font-size: 11px; font-family:Arial, Helvetica, sans-serif;
	            color: #000000; border: solid 1px #8B8B8A; border-collapse:collapse; 
        }

        .Tbl_labelL {
	            text-align:left;  border: solid 1px #8B8B8A; padding: 5px; 
        }

        .Tbl_labelR {
	            text-align:right;  border: solid 1px #8B8B8A; padding: 5px; 
        }
        Tbl_labelR:hover {
	        background-color:#d8def7;
        }

        .Tbl_labelC {
	            text-align:center; border: solid 1px #8B8B8A; padding: 5px; 
        }


    </style>
</head>
<body onload="invoke();">
    <form id="form1" runat="server">
    <center>

        <table id="tblSubDetails" class="Tbl_" style="width:100%; " border="0" align="left">
                <%=vBuildTransaction %>
        </table>
    
    </center>
    </form>

</body>
</html>
