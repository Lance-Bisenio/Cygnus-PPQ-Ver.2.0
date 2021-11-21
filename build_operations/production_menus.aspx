<%@ Page Language="VB" AutoEventWireup="false" CodeFile="production_menus.aspx.vb" Inherits="build_operations_production_menus" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <style>
        .doneProcess {
            background:#57fc57;box-sizing: border-box; font-weight:bold; color:#000;
        }

        .doneProcess:hover {
            background:#57fc57; font-weight:bold;
        }
        .auto-style1 {
            height: 17px;
        }
        .auto-style2 {
            padding-left: 3px;
            font-size: 11px;
            font-family: 'Century Gothic',Arial;
            text-align: left;
            color: #000000;
            padding-right: 1px;
            padding-top: 1px;
            padding-bottom: 1px;
            height: 17px;
        }

        .Compbutton {
            background-color:#bbbbbb; color:#000; cursor:pointer
        }

        .Compbutton:hover {
            background-color:#ff0000; color:#000; cursor:pointer
        }
    </style>

    <link href="../css/menu_v2.css" rel="stylesheet" type="text/css" media="screen" />
    <link rel="stylesheet" type="text/css" href="../css/menu/default.css" />
    <link rel="stylesheet" type="text/css" href="../css/menu/component.css" />

    <link href="../css/BasicContol.css" rel="stylesheet" type="text/css" /> 
    <link href="../css/jquery_datatable.css" rel="stylesheet" /> 
    <link href="../css/colorbox.css" rel="stylesheet" />
    <link href="../css/jquery-ui.css" rel="stylesheet" />
    <link href="../bootstrap/css/bootstrap.css" rel="stylesheet" />
</head>

<body onload="invoke();" >
    <form id="form1" runat="server" style="background-color:#fff;">
          
    <table style="visibility:hidden; position:absolute; font-size: 9px; text-align:right; width:100%; background:#ffd800; color:#000;">
        <tr>
            <td>JONO:<input type="text" id="txtJONO" name="txtJONO" runat="server" readonly="readonly" style="width:50px;"/></td>
            <td>SONO:<input type="text" id="txtSONO" name="txtSONO" runat="server" readonly="readonly" style="width:50px;"/></td>
            <td>PONO:<input type="text" id="txtPONO" name="txtPONO" runat="server" readonly="readonly" style="width:50px;"/></td>
        </tr>
        <tr>
            <td>BOM:<input type="text" id="txtBOM" name="txtBOM" runat="server" readonly="readonly" style="width:50px;"/></td>
            <td>BOMRev:<input type="text" id="txtBomRev" name="txtBomRev" runat="server" readonly="readonly" style="width:50px;"/></td>
            <td>Process TranID:<input type="text" id="txtProcess" name="txtProcess" runat="server" readonly="readonly" style="width:50px;"/></td>
        </tr>
        <tr>
            <td>Section:<input type="text" id="txtSect" name="txtSect" runat="server" readonly="readonly" style="width:50px;"/></td>
            <td>FG:<input type="text" id="txtFG" name="txtFG" runat="server" readonly="readonly" style="width:50px;"/></td>
            <td>SFG:<input type="text" id="txtSFG" name="txtSFG" runat="server" readonly="readonly" style="width:50px;"/></td>
        </tr>
        <tr>
            <td>Qty Required:<input type="text" id="txtQty" name="txtQty" runat="server" readonly="readonly" style="width:50px;"/></td>
            <td>Status<input type="text" id="txtStatus" name="txtStatus" runat="server" readonly="readonly" style="width:50px;"/></td>
            <td>Mode<input type="text" id="txtMode" name="txtMode" runat="server" readonly="readonly" style="width:50px;"/></td>
        </tr>
        <tr>
            <td>StartUpRun:<input type="text" id="txtStartUpRun" name="txtStartUpRun" runat="server" readonly="readonly" style="width:50px;"/></td>
            <td>InitialRun<input type="text" id="txtInitialRun" name="txtInitialRun" runat="server" readonly="readonly" style="width:50px;"/></td>
            <td>ProdRun<input type="text" id="txtProdRun" name="txtProdRun" runat="server" readonly="readonly" style="width:50px;"/></td>
        </tr>
        <tr>
            <td>TranId:<input type="text" id="txtTranId" name="txtTranId" runat="server" readonly="readonly" style="width:50px;"/></td>
            <td>Oper No<input type="text" id="txtOperNo" name="txtOperNo" runat="server" readonly="readonly" style="width:50px;"/></td>
            <td></td>
        </tr>
    </table> 
    <table style="font-size: 11px; text-align:right; width:100%; background:#fff; color:#000; border-collapse:collapse;" border="0"> 
        <tr><td style="height:10px;"></td></tr>
        <tr>
            <td>JO Number :</td>
            <td style="vertical-align:top; font-weight:bold;" 
                class="labelL" colspan="3"><asp:Label ID="lblJONO" runat="server" Text="Label" CssClass=""></asp:Label></td>
        </tr>
        <tr>
            <td>GCAS :</td>
            <td style="vertical-align:top; font-weight:bold;" 
                class="labelL" colspan="3"><asp:Label ID="lblGCAS" runat="server" Text="Label" CssClass=""></asp:Label></td>
        </tr>
        <tr>
            <td style="width:90px; vertical-align:top;">SFG Name :</td>
            <td style="vertical-align:top; font-weight:bold;" colspan="3" class="labelL">
                <asp:Label ID="lblItemName" runat="server" Text="Label"></asp:Label></td>
        </tr>
        <tr><td style="height:10px;" colspan="4"></td></tr>
        <tr>
            <td class="auto-style1">QTY :</td>
            <td class="auto-style2"><asp:Label ID="lblQtyOrder" runat="server" Text="Label"></asp:Label></td>
            <td class="auto-style1">Startup Run :</td>
            <td class="auto-style2"><asp:Label ID="lblRun1" runat="server" Text="00:00"></asp:Label></td>
        </tr>
        <tr>
            <td>Section :</td>
            <td class="labelL"><asp:Label ID="lblSecDescr" runat="server" Text="Label"></asp:Label></td>
            <td class="auto-style1">Initial Run :</td>
            <td class="auto-style2"><asp:Label ID="lblRun2" runat="server" Text="00:00"></asp:Label></td>
        </tr>
        <tr>
            <td>Process :</td>
            <td class="labelL"><asp:Label ID="lblProsDescr" runat="server" Text="Label"></asp:Label></td>
            <td class="auto-style1">Prod Run :</td>
            <td class="auto-style2"><asp:Label ID="lblRun3" runat="server" Text="00:00"></asp:Label></td>
        </tr>
        <tr>
            <td>Opertaion No :</td>
            <td class="labelL" colspan="3"><asp:Label ID="lblOperNo" runat="server" Text="Label" CssClass=""></asp:Label></td>
        </tr>
        <tr>
            <td colspan="4"><hr /></td>
        </tr>
    </table>

    <%--<button type="button" class="btn btn-info btn-lg" 
        data-toggle="modal" data-target="#myModal">Open Modal</button>--%>

    <div style="border:0px solid #000;"> 
        <table class="table-menu" style="width:360px;cursor:pointer;" border="1" > 
            <tr> 
                <td id="tdReceive">
                <img alt="" src="../images/menu/receive.png" style="margin-top:11px" /><br />Receive Mat</td>
                <td id="tdHelper" onclick='showModule(""" & rs("Dependencies") & """, """ & rs("Label_Caption") & """)'>
                <img alt="" src="../images/menu/helper.png" style="margin-top:11px" /><br />Helper</td>
                <td id="tdRequest" onclick='showModule(""" & rs("Dependencies") & """, """ & rs("Label_Caption") & """)'>
                <img alt="" src="../images/menu/request.png" style="margin-top:11px" /><br />Request Mat</td>
            </tr>
            
            <tr>
                <td id="tdCompletion" onclick="showModule(this.id)">
                <img alt="" src="../images/menu/parcomplete.png" style="margin-top:11px" /><br />Completion Report</td>
                <td id="tdComplete" class="Compbutton" data-toggle="modal" data-target="#myModal"><br />JOB ORDER COMPLETE</td>
                <td id="tdReturn" onclick="showModule(this.id)">
                <img alt="" src="../images/menu/return.png" style="margin-top:11px" /><br />Return Mat</td>

                <%--<td id="tdMachine" onclick='showModule(""" & rs("Dependencies") & """, """ & rs("Label_Caption") & """)'>
                <img alt="" src="../images/menu/machine.png" style="margin-top:11px" /><br />Machine</td>--%> 
            </tr>
            <tr> 
                <td id="tdIncident">
                <img alt="" src="../images/menu/incident.png" style="margin-top:11px" /><br />Incident Report</td>
                <td id="" onclick='showModule(""" & rs("Dependencies") & """, """ & rs("Label_Caption") & """)'>
                <img alt="" src="../images/menu/helperx.png" style="margin-top:11px" /><br /></td>
                <td id="" onclick='showModule(""" & rs("Dependencies") & """, """ & rs("Label_Caption") & """)'>
                <img alt="" src="../images/menu/requestx.png" style="margin-top:11px" /><br /></td>
            </tr>
        </table> 
        <br />
        <table class="table-menu" style="width:360px" border="0" > 
            <%=vMenus %>
        </table>

        <%--<table style="width:350px; margin:auto; margin-top:4px;" border="0" > 
            <tr>
                <td>
                    <asp:Button ID="btnComp" runat="server" Text="Complition" 
                        CssClass="btn btn-primary" Height="51px" Width="349px" Visible="false"  /> 
                    <a href="c:/Temp/App/CitiApplication.exe">test</a>
                </td>
            </tr>
        </table>--%>

    </div>
    <div id="myModal" class="modal fade" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Complete job order</h4>
                </div>
                <div class="modal-body">
                    <p>Enter your Employee Code and click Submit.</p>
                    <asp:TextBox ID="txtEmpCode" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
                     
                </div>
                <div class="modal-footer">
                    <asp:Button ID="btnSubmit" runat="server" Text ="Submit" CssClass="btn btn-primary btn-sm" Height="30px" />
                    <asp:Button ID="btnCancel" runat="server" Text ="Cancel" CssClass="btn btn-primary btn-sm" Height="30px" />
                </div>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
 
<script src="../js/jquery-1.10.2.js" type="text/javascript"></script>
<script src="../media/js/jquery.dataTables.js" type="text/javascript"> </script>
<script src="../js/jquery.colorbox-min.js" type="text/javascript">  </script>
<script src="../bootstrap/js/bootstrap.min.js"></script>

<script type="text/javascript">
    //function runWinZip() {
    //    var shell = new ActiveXObject("WScript.shell");
    //    shell.run("C:\\Temp\\App\\CitiApplication.exe");
    //}

    function invoke() {
        <%=vScript %>
    }

    function RunProcess(vParam) {
        $('#txtMode').val(vParam);
        window.form1.submit();
    }

    $(document).ready(function () {
        var vProperties = "top=50px, left=80px, scrollbars=yes";
        var vParam = "&pJO=" + $('#txtJONO').val() + "&pBOM=" + $('#txtBOM').val() + "&pBOMRev=" + $('#txtBomRev').val() +
                        "&pTranId=" + $('#txtTranId').val() + "&pSection=" + $('#txtSect').val() + "&pProcess=" + $('#txtProcess').val() +
                        "&pSFG=" + $('#txtSFG').val() + "&pOperNo=" + $('#txtOperNo').val() + "";

        var vDeleteParam = ""

        $('#tdReceive').click(function (event) { 
            winPop = window.open("../inventory/materials-receiving.aspx?pMode=ReceiveMaterials" + vParam, "ProdWindow", "width=1200px, height=700px," + vProperties);
            winPop.focus();
        });

        $('#tdRequest').click(function (event) {
            winPop = window.open("../inventory/materials-request.aspx?pMode=RequestMaterials" + vParam, "ProdWindow", "width=1000px, height=500px," + vProperties);
            winPop.focus();
        });

        $('#tdReturn').click(function (event) {
            winPop = window.open("../inventory/materials-return.aspx?pMode=RequestMaterials" + vParam, "ProdWindow", "width=1000px, height=500px," + vProperties);
            winPop.focus();
        }); 

        $('#tdReject').click(function (event) {
            winPop = window.open("../inventory/joborder-disposal.aspx?pMode=RequestMaterials" + vParam, "ProdWindow", "width=1000px, height=500px," + vProperties);
            winPop.focus();
        }); 

        $('#tdHelper').click(function (event) {
            winPop = window.open("../inventory/joborder-helper.aspx?pMode=SelectHelper" + vParam, "ProdWindow", "width=800px, height=500px," + vProperties);
            winPop.focus();
        });

        $('#tdMachine').click(function (event) {
            winPop = window.open("../inventory/joborder-machine.aspx?pMode=SelectHelper" + vParam, "ProdWindow", "width=800px, height=500px," + vProperties);
            winPop.focus();
        });

        $('#tdCompletion').click(function (event) {
            winPop = window.open("../inventory/joborder-completion.aspx?pMode=Completion" + vParam, "ProdWindow", "width=1200px, height=500px," + vProperties);
            winPop.focus();
        });

        $('#tdIncident').click(function (event) {
            winPop = window.open("../inventory/joborder-incidentLog.aspx?pMode=SelectHelper" + vParam, "ProdWindow", "width=1024px, height=500px," + vProperties);
            winPop.focus();
        });
    });
</script>