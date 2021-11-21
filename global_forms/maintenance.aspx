<%@ Page Language="VB" AutoEventWireup="false" CodeFile="maintenance.aspx.vb" Inherits="maintenance" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Codeset Maintenance Page</title> 

    
    <script type="text/javascript" src="../js/jquery-1.10.2.js"></script>
    <script type="text/javascript" src="../media/js/jquery.dataTables.js"></script> 
    <script type="text/javascript" src="../js/jquery.colorbox-min.js"></script> 
     
    <link href="../css/BasicContol.css" rel="stylesheet" type="text/css" />
    <link href="../css/colorbox.css" rel="stylesheet" />

    <script language="javascript" type="text/javascript">
        function invoke() {
            <%=vScript %>
        }
        
        //function modify(m) {
        //    //alert("lance test");
        //    scrid = document.getElementById("lblScrId").value;
        //    tvalue = document.getElementById("txtvalue").value;
        //    vSubFile = document.getElementById("txtSubFile").value;
            
        //    if (tvalue != "" && m == 'e' || m == 'a')
        //    {
        //        if (scrid == 2050 || scrid == 2045 || scrid == 3020) {
        //            winmodify = window.open(vSubFile + "?mode=" + m + "&id=" + scrid + "&value=" + tvalue, "winmodify", "toolbar=no,scrollbars=no,top=80,left=100,height=350,width=650");
        //        } else {
        //            winmodify = window.open(vSubFile + "?mode=" + m + "&id=" + scrid + "&value=" + tvalue, "winmodify", "toolbar=no,scrollbars=no,top=80,left=100,height=270,width=650");
        //        }
               
        //        winmodify.focus();
        //    }else{
        //        alert('Please select item first.');
        //    }
        //}
        
        function print() {
            winpreview = window.open("crystalpreview.aspx?rpt=civilref.rpt","Preview","toolbar=no,scrollbars=no,top=80,left=100,height=600,width=800");
            winpreview.focus();
        }

        //$(document).ready(function () { 
        //    $('#cmdEdit').on('click', function () {
        //        btnHeaderAdd
        //        $('#cmdEdit').colorbox({ 'href': $('#txtSubFile').val() + "?mode=e&id=" + $('#lblScrId').val() + "&value=" + $('#txtvalue').val() + "", 'width': '80%', 'height': '80%;', iframe: true });
        //    });

        //    $('#cmdAdd').colorbox({
        //        href: $('#txtSubFile').val() + "?mode=a&id=" + $('#lblScrId').val() + "&value=" + $('#txtvalue').val() + "", width: "80%", height: "80%", iframe: true
        //    });
        //});

        $(document).ready(function () {
            $('#btnHeaderEdit').on('click', function () {
                $('#btnHeaderEdit').colorbox({ 'href': $('#txtSubFile').val() + "?pMode=edit&id=" + $('#lblScrId').val() + "&pCode=" + $('#txtvalue').val() + "", 'width': '80%', 'height': '80%;', iframe: true });
                //$('#btnHeaderEdit').colorbox({ 'href': $('#txtSubFile').val() + "?mode=e&id=" + $('#lblScrId').val() + "&value=" + $('#txtvalue').val() + "", 'width': '80%', 'height': '80%;', iframe: true });
            });

            $('#btnHeaderAdd').colorbox({
                href: $('#txtSubFile').val() + "?pMode=a&id=" + $('#lblScrId').val() + "&pCode=" + $('#txtvalue').val() + "", width: "80%", height: "80%", iframe: true
                //href: $('#txtSubFile').val() + "?mode=a&id=" + $('#lblScrId').val() + "&value=" + $('#txtvalue').val() + "", width: "80%", height: "80%", iframe: true
            });
            $(document).bind('cbox_closed', function () {
                location.reload();
            });
        });


    </script>
</head>
<body onload="invoke();">
<center>
    <form id="form1" runat="server">
                    <span id="lblCaption"><asp:Label id="lblScrName" runat="server" Font-Size="Medium" Font-Bold="True"></asp:Label></span> 
        
        <div id="mainContent">
                <table style="width: 100%" cellpadding="0" cellspacing="0">
                    <tr id="vTranType" runat="server" visible="false">
                        <td class="labelR" >Tran Type :</td>
                        <td class="labelL">
                            <asp:DropDownList ID="cmbAcctType" runat="server" Width="204px" CssClass="labelL">
                            </asp:DropDownList>
                        </td>
                    </tr>
                     
                    <tr>
                        <td class="labelL" style="width:80px;"><b>Quick Search :</b></td>
                           <%-- <asp:LinkButton ID="cmdA" runat="server" CssClass="label">A</asp:LinkButton>
                            <asp:LinkButton ID="cmdB" runat="server" CssClass="label">B</asp:LinkButton>
                            <asp:LinkButton ID="cmdC" runat="server" CssClass="label">C</asp:LinkButton>
                            <asp:LinkButton ID="cmdD" runat="server" CssClass="label">D</asp:LinkButton>
                            <asp:LinkButton ID="cmdE" runat="server" CssClass="label">E</asp:LinkButton>
                            <asp:LinkButton ID="cmdF" runat="server" CssClass="label">F</asp:LinkButton>
                            <asp:LinkButton ID="cmdG" runat="server" CssClass="label">G</asp:LinkButton>
                            <asp:LinkButton ID="cmdH" runat="server" CssClass="label">H</asp:LinkButton>
                            <asp:LinkButton ID="cmdI" runat="server" CssClass="label">I</asp:LinkButton>
                            <asp:LinkButton ID="cmdJ" runat="server" CssClass="label">J</asp:LinkButton>
                            <asp:LinkButton ID="cmdK" runat="server" CssClass="label">K</asp:LinkButton>
                            <asp:LinkButton ID="cmdL" runat="server" CssClass="label">L</asp:LinkButton>
                            <asp:LinkButton ID="cmdM" runat="server" CssClass="label">M</asp:LinkButton>
                            <asp:LinkButton ID="cmdN" runat="server" CssClass="label">N</asp:LinkButton>
                            <asp:LinkButton ID="cmdO" runat="server" CssClass="label">O</asp:LinkButton>
                            <asp:LinkButton ID="cmdP" runat="server" CssClass="label">P</asp:LinkButton>
                            <asp:LinkButton ID="cmdQ" runat="server" CssClass="label">Q</asp:LinkButton>
                            <asp:LinkButton ID="cmdR" runat="server" CssClass="label">R</asp:LinkButton>
                            <asp:LinkButton ID="cmdS" runat="server" CssClass="label">S</asp:LinkButton>
                            <asp:LinkButton ID="cmdT" runat="server" CssClass="label">T</asp:LinkButton>
                            <asp:LinkButton ID="cmdU" runat="server" CssClass="label">U</asp:LinkButton>
                            <asp:LinkButton ID="cmdV" runat="server" CssClass="label">V</asp:LinkButton>
                            <asp:LinkButton ID="cmdW" runat="server" CssClass="label">W</asp:LinkButton>
                            <asp:LinkButton ID="cmdX" runat="server" CssClass="label">X</asp:LinkButton>
                            <asp:LinkButton ID="cmdY" runat="server" CssClass="label">Y</asp:LinkButton>
                            <asp:LinkButton ID="cmdZ" runat="server" CssClass="label">Z</asp:LinkButton>&nbsp;--%>
                            <td class="labelL">
                            <asp:TextBox ID="txtSearch" runat="server" CssClass="labelL" Width="200px" Height="18px"></asp:TextBox> 
                            <asp:Button ID="cmdSearch" runat="server" CssClass="button" Text="Search" />
                            <asp:Button ID="cmdReturn" runat="server" CssClass="button" Text="Close this screen" />
                            <%--<input id="cmdAdd" class="button" type="button" value="Add"style="width: 60px" /><%-- onclick="modify('a');"  
                            <input id="cmdEdit" class="button" type="button" value="Edit"  style="width: 60px" runat="server" />< % - -onclick="modify('e');"- - %>

                            
                            <input id="cmdPrint" class="button" type="button" value="Print" onclick="print();" style="width: 60px" size="" />--%>
                            

                        <input id="txtvalue" runat="server" name="txtvalue" style="width: 32px" type="hidden" />
                        <input id="lblScrId" runat="server" style="width: 24px" type="hidden" /> 
                        <input id="txtSubFile" runat="server" name="txtvalue" type="hidden" />
                        <input id="h_Mode" runat="server" name="h_Mode" style="width: 32px" type="hidden" />
                        </td>
                    </tr>
                    <tr> 
                        <td colspan="2">
 
                            <table id="Table2" class="titleBarTop"  border="0" style="width:100%;  border-collapse:collapse;">
                                <tr style="border:solid 0px #cccccc; border-bottom:0px;">
                                    <td style="width:320px; text-align:left;">
                                        &nbsp;&nbsp;
                                        <input type="button" runat="server" id="btnHeaderAdd" name="btnProAdd" value="Add"  class="tblButtonA" />  
                                        <input type="button" runat="server" id="btnHeaderEdit" name="btnProEdit" value="Edit"  class="tblButtonE" /> 
                                        <asp:Button ID="cmdDelete" runat="server" CssClass="tblButtonD" Text="Delete" OnClientClick='return confirm("Are you sure you want to delete the selected record?");' Width="60px" /> 
                                        <%--<input type="button" runat="server" id="btnHeaderDel" name="" value="Delete" class="tblButtonD" data-href="divPop" />--%>
                                        Show : <asp:DropDownList ID="cmbShow" runat="server" Width="70px" AutoPostBack="True" CssClass="labelL">
                                        </asp:DropDownList></td>
                                    <td style="text-align:left; padding-left :5px;">
                                        
                                    </td>
                                    <td class="labelR" >
                                        <b><asp:Label ID="lblTotalDocs" runat="server" CssClass="labelL" Text="Total Records Retrieved : 0" ForeColor="#ffffff"></asp:Label></b>&nbsp;&nbsp;&nbsp;
                                    </td>
                                </tr>
                            </table>
                             
                            <asp:GridView ID="tblMaintenance" runat="server" AllowPaging="True" CssClass="mainGridView" BorderColor="#cccccc" BorderStyle="Solid" AutoGenerateColumns="true"
                                PageSize="10" Width="100%">
                                <Columns>
                           
                                    <asp:CommandField ButtonType="Button" ShowSelectButton="True">
                                        <ControlStyle CssClass="button" />
                                        <ItemStyle CssClass="labelC" Width="50px" />
                                     </asp:CommandField>

                                    <asp:TemplateField HeaderText="#" HeaderStyle-Width="30px" >
                                        <ItemTemplate>
                                            <%# Container.DataItemIndex + 1 %>   
                                        </ItemTemplate>
                                        <HeaderStyle Width="30px"></HeaderStyle>
                                    </asp:TemplateField>
                                     
                                </Columns>
                                
                                <PagerSettings PageButtonCount="40" />
                                
                                <SelectedRowStyle CssClass="activeBar" />
                                <PagerStyle  HorizontalAlign="Left" />
                                <RowStyle CssClass="odd" />
                                <HeaderStyle CssClass="titleBar" />
                                <AlternatingRowStyle CssClass="even" />
                            </asp:GridView>
                            </td>
                    </tr>
                </table>
            
            </div>
    </form>
    </center>
</body>
</html>
