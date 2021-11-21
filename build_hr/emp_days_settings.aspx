<%@ Page Language="VB" AutoEventWireup="false" CodeFile="emp_days_settings.aspx.vb" Inherits="emp_days_settings" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
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

        .txtEntreL {
            width:100%; height:100%; border:0px solid #fff; text-align:right; padding-right:3px; outline: none; text-align:left;
        }

         .txtEntreR {
            width:100%; height:100%; border:0px solid #fff; text-align:right; padding-right:3px; outline: none; text-align:right;
        }

    </style>

</head>
<body style="background-color:#e7e7e7" onload="invoke();">
    <form id="form1" runat="server"> 
    <div class="SmallBox_Frame">
        <table id="Standard_Tbl" style="width:100%; border-collapse:collapse;" border="0" >
            <tr>
                <td style="text-align:left; border-bottom: solid 1px #808080;" colspan="4">
                    <asp:Label ID="txtTitle" CssClass="vModuleTitle" runat="server" Text="Total days work settings"></asp:Label></td>
            </tr>
            <tr><td style="height:10px;"></td></tr>
        </table>
         
        <table border="0" style="width:100%;  border-collapse:collapse;" >
            <%=vData %>
        </table> 
                
         <table id="Table1" style="width:100%; border-collapse:collapse;" border="0" >
            <tr>
                <td>
                    <asp:Button ID="cmdSave" runat="server" CssClass="labelC" text="Save" Width="40px"  /> 
                    <asp:Button ID="cmdCancel" runat="server" CssClass="labelC" text="Cancel" Width="50px"  /> 
                </td>
            </tr>
        </table>
         
    </div>
    </form>
</body>
</html>

