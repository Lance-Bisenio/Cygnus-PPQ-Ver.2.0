<%@ Page Language="VB" AutoEventWireup="false" CodeFile="joborder-helper.aspx.vb" Inherits="inventory_joborder_helper" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="../js/jquery-ui-1.10.4.custom.js"></script>
    <script src="../js/jquery-ui-1.10.4.custom.min.js"></script>
    <script src="../js/jquery-1.10.2.js"></script>
    <script src="../js/jquery-ui.js"></script>
    
    <link href="../css/BasicContol.css" rel="stylesheet" type="text/css" />
    <link href="../css/calendar/jquery-ui-1.10.4.custom.min.css" rel="stylesheet" />
    <link href="../css/calendar/jquery-ui-1.10.4.custom.css" rel="stylesheet" />

    <style type="text/css">
        .trLink {
            cursor:pointer;
        }

        .trLink:hover {
            background: #f8f7f7;    
        }
    </style>

    <script type="text/javascript" >

        function SetupHelperList(pJO, pSect, pProc, pSFG, pEmp) {
            
            var xmlhttp = new XMLHttpRequest();
            xmlhttp.open("GET", "production_xml.aspx?pMode=Prod-HelperSetup&pJO=" + pJO + "&pSect=" + pSect + "&pProc=" + pProc + "&pSFG=" + pSFG + "&pEmp=" + pEmp, true);
            xmlhttp.send();


            window.form1.submit();
        }

        function RemoveHelperList(pTranId) {
            var xmlhttp = new XMLHttpRequest();
            xmlhttp.open("GET", "production_xml.aspx?pMode=Prod-HelperRemove&pTranId=" + pTranId, true);
            xmlhttp.send();

            window.form1.submit();
        }

        function invoke() {
            <%=vScript %>
            }
    </script>
    

</head>
<body onload="invoke();" >
    <form id="form1" runat="server">
    <div style="width:100%; margin:auto; border:0px solid #000;">
        <table id="Standard_Tbl" style="width:100%;" border="0">
             <tr>
                <td style="text-align:left; border-bottom: solid 1px #808080;" colspan="6">
                    <asp:Label ID="txtTitle" CssClass="vModuleTitle" runat="server" Text="List of Helpers"></asp:Label> 
                </td>
            </tr>
            <tr><td style="height:5px;"></td></tr>
        </table>

        <table style="width:100%;" border="0">
            <tr>
                <td style="width:35%; vertical-align:top;">
                    <table style="width:100%; border-collapse:collapse; padding-right:0px;" border="0">
                        <tr>
                            <td colspan="2" style="text-align:right;">
                                Section : <asp:dropdownlist id="cmbSection" runat="server" CssClass="DropDownList" AutoPostBack="True" Width="200px"></asp:dropdownlist>
                            </td>
                        </tr>
                        <tr><td colspan="2" style="height:5px;"></td></tr> 
                    </table>

                    <div style="width:99%; height:419px; overflow:auto; border: solid 1px #cccccc; margin:auto; vertical-align:top;">                        
                        <table style="width:95%; margin:auto; border-collapse:collapse; " border="0">
                            <tr><td style="height:3px;"></td></tr>
                            <%=vEmpList %>
                        </table>    
                    </div>
                    

                </td>
                <td style="width:65%; vertical-align:top;">
                    <div style="width:98%; height:450px; overflow:auto; float:right; border: solid 1px #cccccc">
                        <table style="width:95%; margin:auto; border-collapse:collapse; " border="0">
                            <tr><td style="height:10px;"></td></tr>

                            

                            <%=vHelperList %>
                        </table>    
                    </div>

                </td>
            </tr>
        </table>
        
        
    </div>
    </form>
</body>
</html>
