<%@ Page Language="VB" Debug="true" ViewStateEncryptionMode="Always" AutoEventWireup="true" CodeFile="Rewind.aspx.vb" Inherits="Rewind" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>



    <link href="../css/bootstrap4/css/bootstrap.css" rel="stylesheet" />
    <link href="../css/calendar/jquery-ui-1.10.4.custom.min.css" rel="stylesheet" />

    <script src="../js/jquery-ui-1.12.1/external/jquery/jquery.js"></script>
    <%--<script src="../js/jquery-1.10.2.js"></script>--%>
    <script src="../css/bootstrap4/js/bootstrap.js"></script>

    <%--<script src="../js/jquery-ui.js"></script>--%>
    <script src="../js/jquery-ui-1.12.1/jquery-ui.js"></script>
    <style>
        body {
            font-family: Arial;
            font-size: 12px
        }

        div {
            border: 0px solid #000
        }

        h6, h6 small {
            padding-bottom: -2px;
            margin-bottom: 0px;
        }

            h6 small {
                font-size: 11px;
            }

        .divPad {
            margin-bottom: 5px
        }
    </style>
</head>
<body onload="invoke();">
    <form id="form1" runat="server" autocomplete="off">
        <div class="container-fluid">
            <input type="hidden" id="h_TranId" runat="server" name="h_TranId" />
            <input type="hidden" id="h_Mode" runat="server" name="h_Mode" />
            <h3>Rewinding Process</h3>

            <div class="row divPad">
                <div class="col-md-3">
                    <h6><small class="text-muted">Date created from and to:</small></h6>
                    <div class="form-inline">
                        <div class="form-group">
                            <asp:TextBox ID="txtDateFrom" runat="server" CssClass="form-control form-control-sm col-4"></asp:TextBox>&nbsp;To:
                            <asp:TextBox ID="txtDateTo" runat="server" CssClass="form-control form-control-sm pad col-4"></asp:TextBox>&nbsp;
                            <button type="submit" id="btnSearch" runat="server" class="btn btn-sm btn-primary">Search</button>
                        </div>
                    </div>
                </div>

                <%--<div class="col-md-3">
                    <h6><small class="text-muted">Quick search Job Order Number:</small></h6>
                    <div class="form-inline">
                        <div class="form-group">
                            <asp:TextBox ID="TxtJONO" runat="server" CssClass="form-control form-control-sm" placeholder="Enter Job order number"></asp:TextBox>&nbsp;
                            <button type="submit" id="BtnQSearch" runat="server" class="btn btn-sm btn-primary">Search</button>
                        </div>
                    </div>
                </div>--%>
            </div>

            <div class="row">
                <div class="col-sm-12">
                     
                    <table class="table table-bordered" border="0" style="font-size: 11px;">
                        <thead>
                            <tr class="text-info" style="text-align: center;">
                                <td style="width: 10px">#</td>
                                <td>JONO</td>
                                <td>BTH NO</td>
                                <td>Core Weight</td>
                                <td>Net Weight</td>
                                <td>Gross Weight</td>
                                <td>Meter</td>
                                <td>Qty</td>
                                <td>Completion By</td>
                                <td>Created By</td>
                                <td style="width: 180px">Rewined By</td>
                                <td style="width: 40px"></td>
                            </tr>
                        </thead>
                        <%=vHeader %>
                        <%=vData %>
                    </table>
                </div>
            </div>

        </div>

        <span id="txtHint"></span>
    </form>
</body>
</html>


<script>
    function invoke() {
        <%=vScript %>
    }


    $("#txtDateFrom").datepicker();
    $("#txtDateTo").datepicker();

    function CompleteRewind(pId, pLastVal, pJO) {

        var xmlhttp = new XMLHttpRequest();

        xmlhttp.onreadystatechange = function () {
            if (this.readyState == 4 && this.status == 200) {

                if (pLastVal == "Cancel") {
                    document.getElementById(pId + "span").innerHTML = "";
                } else {
                    document.getElementById(pId + "span").innerHTML = this.responseText;
                }
                
                //alert(this.responseText);
            }
        };


        if (pLastVal == "Cancel") {
            $('#' + pId).attr("value", "Complete");
            $('#' + pId).attr("class", "btn btn-sm btn-primary");
            xmlhttp.open("GET", "RewindXML.aspx?pModule=Rewind&pMode=Cancel&pJO=" + pJO + "&pId=" + pId, true);
        } else {
            $('#' + pId).attr("value", "Cancel");
            $('#' + pId).attr("class", "btn btn-sm btn-danger");
            xmlhttp.open("GET", "RewindXML.aspx?pModule=Rewind&pMode=Complete&pJO=" + pJO + "&pId=" + pId, true);
        }

        xmlhttp.send();
    }

</script>
