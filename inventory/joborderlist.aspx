<%@ Page Language="VB" AutoEventWireup="false" CodeFile="joborderlist.aspx.vb" Inherits="joborderlist" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../bootstrap/css/bootstrap.css" rel="stylesheet" />
    <link href="../css/calendar/jquery-ui-1.10.4.custom.min.css" rel="stylesheet" />
    <link href="../css/calendar/jquery-ui-1.10.4.custom.css" rel="stylesheet" />
    <link href="../css/BasicContol.css" rel="stylesheet" type="text/css" />
     
    <style>
        .cDueDate { 
            border:5px solid; color:#d90202;
            margin-left:-1px; margin-right: -1px; 
        }
        .cStartDate { 
            border:5px solid; color:#268ffb;
            margin-left:-1px; margin-right: -1px; 
        }
        .cWorkDays { 
            border:5px solid; color:#93c8ff;
            margin-left:-1px; margin-right: -1px; 
        }
        .cReleaseDate { 
            border:5px solid; color:#a40dd5; 
            margin-left:-1px; margin-right: -1px; 
        }

        .vColorB { color:blue; }
        .vColorG { color:#ff6a00; }
        .vColorR { color:red; }
    </style>
</head>
<body onload="invoke();" >
    <form id="form1" runat="server">
    <input type="hidden" id="h_LastSelected" />
    <input type="hidden" id="h_oldClass" />
    <input type="hidden" id="h_PrevTranId" />

    <input type="hidden" id="h_BOM" runat="server"  />
    <input type="hidden" id="h_BOMRev" runat="server"  />
    <input type="hidden" id="h_TranId" runat="server"  />

    <div class="container-fluid" style="margin-bottom:50px;">
        <div class="row" style="width:98%; margin:auto;">
            <table id="" border="0" style="width:100%; margin:5px;  border-collapse:collapse;"> 
                <tr>
                    <td style="width:100px;">Job Order Status :</td>
                    <td style="width:300px;">
                        <asp:DropDownList ID="cmbStatus" runat="server" Width="210px" CssClass="labelL">
                        </asp:DropDownList>
                    </td>
                    <td style="width:100px;">Due Date From :</td>
                    <td style="width:300px;"> 
                        <asp:TextBox ID="txtDateFrom" runat="server" CssClass="labelL" Width="100px" Height="18px"></asp:TextBox> 
                        <img src="../images/calendar.png" class="DatePicker" style="vertical-align:middle;" alt="" /></td>
                    <td style="width:100px;">  
                        Section :</td>
                    <td> 
                        <asp:DropDownList ID="cmbSection" runat="server" Width="210px" CssClass="labelL" AutoPostBack="True">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>Customer Name :</td>
                    <td>
                        <asp:DropDownList ID="cmbCustomer" runat="server" Width="210px" CssClass="labelL">
                        </asp:DropDownList>
                    </td>
                    <td>Due Date To :</td>
                    <td> 
                        <asp:TextBox ID="txtDateTo" runat="server" CssClass="labelL" Width="100px" Height="18px"></asp:TextBox> 
                        <img src="../images/calendar.png" class="DatePicker" style="vertical-align:middle;" alt="" />
                    </td>
                    <td>Machine : </td>
                    <td> 
                        <asp:DropDownList ID="cmbMachine" runat="server" Width="210px" CssClass="labelL">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>Item Type :</td>
                    <td>
                        <asp:DropDownList ID="cmbItemType" runat="server" Width="210px" CssClass="labelL">
                        </asp:DropDownList>
                    </td>
                    <td>Search JO No.</td>
                    <td> 
                        <asp:TextBox ID="txtSearchBox" runat="server" CssClass="text-left" Width="100px" Height="24px"></asp:TextBox> 
                        <asp:Button ID="btnSearch" CssClass="btn btn-primary btn-xs" runat="server"  Text="Search" />    
                        <asp:DropDownList ID="cmbSearchBy" runat="server" Width="48px" CssClass="labelL" Visible="False" >
                        </asp:DropDownList> 

                        <asp:TextBox ID="txtMachList" runat="server" CssClass="labelL" Width="44px" Height="18px" Visible="false"></asp:TextBox> 
                        <asp:TextBox ID="txtSearch" runat="server" CssClass="labelL" Width="33px" Height="18px" Visible="False"></asp:TextBox> 
                    </td>
                    <td></td>
                </tr>
                <tr><td style="height:10px;" colspan="6"></td></tr>
                
            </table>
        </div>
        <div class="row" style="width:98%; margin:auto;">
            <table id="Table1" class="titleBarTop"  border="0" style="width:100%; text-align:center; border-collapse:collapse;">
                <tr style="border:solid 0px #cccccc; border-bottom:0px;">
                    <td style="width:320px; text-align:left;">
                        &nbsp;&nbsp;
                            <input type="button" runat="server" id="btnHeaderAdd" name="btnProAdd" value="Add"  class="tblButtonA" />  
                            <input type="button" runat="server" id="btnHeaderEdit" name="btnProEdit" value="Edit"  class="tblButtonE" />  
                            <input type="button" runat="server" id="btnHeaderDel" name="" value="Delete" class="tblButtonD" data-href="divPop" />
                              
                        <%-- Show :<asp:DropDownList ID="cmbShow" runat="server" Width="70px" AutoPostBack="True" CssClass="labelL">
                        </asp:DropDownList> --%>

                    </td>
                    <td style="text-align:left; padding-left :5px;"></td>
                
                    <td class="labelR" >
                        <b><asp:Label ID="lblTotalDocs" runat="server" CssClass="labelL" Text="Documents Retrieved : 0" ForeColor="#ffffff"></asp:Label></b>&nbsp;&nbsp;&nbsp;
                    </td>
                </tr>
            </table>
            <asp:GridView ID="tbl_JOList" runat="server" AllowPaging="True" BorderColor="#cccccc"
                AutoGenerateColumns="False" Width="100%" BorderStyle="Solid" BorderWidth="1px" 
                CssClass="table table-bordered">
                <Columns>
                    <asp:CommandField ButtonType="Button" ShowSelectButton="True" > 
                        <ItemStyle CssClass="labelC" Width="40px" />
                        <ControlStyle CssClass="btn btn-primary btn-xs" />
                    </asp:CommandField>
                    <asp:TemplateField HeaderText="#" HeaderStyle-Width="30px" >   
                        <ItemTemplate>
                            <%# Container.DataItemIndex + 1 %> 
                        </ItemTemplate> 
                        <HeaderStyle Width="30px"></HeaderStyle>
                    </asp:TemplateField>
                
                    <asp:BoundField DataField="BOM_Cd" HeaderText="BOM" >
                        <ItemStyle CssClass="labelC" Width="40px" />
                    </asp:BoundField>

                    <asp:BoundField DataField="BOMRev" HeaderText="Rev" >
                        <ItemStyle CssClass="labelC" Width="40px" />
                    </asp:BoundField>

                    <asp:BoundField DataField="JobOrderNo" HeaderText="JONO" >
                        <ItemStyle CssClass="labelC" Width="100px" />
                    </asp:BoundField>

                    <asp:BoundField DataField="SalesOrderNo" HeaderText="SONO" >
                        <ItemStyle CssClass="labelC" Width="100px" />
                    </asp:BoundField>

                    <asp:BoundField DataField="Alt_Cd" HeaderText="GCAS" >  
                        <ItemStyle CssClass="labelC" Width="100px" />
                    </asp:BoundField>

                    <asp:BoundField DataField="Item_Cd" HeaderText="Item Code" >  
                        <ItemStyle CssClass="labelC" Width="100px" />
                    </asp:BoundField>

                    <asp:BoundField DataField="vItemName" HeaderText="Item Description" >
                        <ItemStyle CssClass="labelL" />
                    </asp:BoundField> 
                            
                    <asp:BoundField DataField="OrderQty" HeaderText="JO QTY" >  
                        <ItemStyle CssClass="labelC" Width="80px" />
                    </asp:BoundField>

                
                    <asp:BoundField DataField="vUOM" HeaderText="UOM" >  
                        <ItemStyle CssClass="labelC" Width="80px" />
                    </asp:BoundField>
                
                    <asp:BoundField DataField="JO_Status" HeaderText="JO Status" >  
                        <ItemStyle CssClass="labelC" Width="80px" />
                    </asp:BoundField>

                    <asp:BoundField DataField="ReleaseDate" HeaderText="JO Release" >  
                        <ItemStyle CssClass="labelC" Width="90px" />
                    </asp:BoundField>

                    <asp:BoundField DataField="StartDate" HeaderText="JO StartDate" >  
                        <ItemStyle CssClass="labelC" Width="90px" />
                    </asp:BoundField>

                    <asp:BoundField DataField="DueDate" HeaderText="JO DueDate" >  
                        <ItemStyle CssClass="labelC" Width="90px" />
                    </asp:BoundField>
                                         
                    <asp:BoundField DataField="TranID" HeaderText="TranID" 
                        HeaderStyle-CssClass="hideGridColumn" 
                        ItemStyle-CssClass="hideGridColumn" ItemStyle-width="80px"/>
                 
                    <asp:TemplateField HeaderText="Process Completed" HeaderStyle-Width="30px" >   
                        <ItemTemplate>
                            <asp:Label ID="Label1" runat="server" Text='<%# GetProdStatus(Eval("JobOrderNo"), Eval("BOM_Cd"), Eval("BOMRev"))%>'></asp:Label>
                        </ItemTemplate> 
                        <HeaderStyle Width="30px"></HeaderStyle>
                    </asp:TemplateField>
                    
                </Columns> 
                <SelectedRowStyle CssClass="activeBar" />
                <PagerStyle Font-Size="8pt" />
                <HeaderStyle CssClass="titleBar" />
                <RowStyle CssClass="odd" />
                <AlternatingRowStyle CssClass="even" />
            </asp:GridView>
        </div><br />

        <div class="row" style="width:98%; margin:auto;">
            <table id="Table2" class="table table-bordered" style="width:100%;">
                <thead>
                    <tr>
                        <th colspan="7"></th>
                        <th class="text-center" colspan="2">Job Order</th>
                        <th class="text-center" colspan="3">Production</th>
                    </tr>
                    <tr class="text-center">
                        <th style="width:50px"></th>
                        <th style="width:40px">#</th>
                        <th style="width:50px">Oper Order</th>
                        <th style="width:90px">Section</th>
                        <th style="width:90px">Process</th>
                        <th style="width:120px">SFG Code</th>
                        <th>Item Description</th>
                        <th style="width:80px">Kilos (kgs)</th>
                        <th style="width:80px">Meter</th>
                        <th style="width:80px">Kilos (kgs)</th>
                        <th style="width:80px">Meter</th>
                        <th style="width:130px">Status</th> 
                    </tr>
                </thead>
                <%=vRecordData %>
            </table>  
        </div>
    </div>    

    <%--<div> 
        <asp:GridView ID="tbl_JOProcess" runat="server" AllowPaging="True" BorderColor="#cccccc"
            AutoGenerateColumns="False" Width="100%" BorderStyle="Solid" BorderWidth="1px" 
            CssClass="mainGridView" PageSize="100">
            <Columns>
                  
                <asp:TemplateField HeaderText="#" HeaderStyle-Width="30px" >   
                    <ItemTemplate>
                        <%# Container.DataItemIndex + 1 %> 
                    </ItemTemplate> 
                    <HeaderStyle Width="30px"></HeaderStyle>
                </asp:TemplateField>
                
                <asp:BoundField DataField="OperOrder" HeaderText="OperOrder" >
                    <ItemStyle CssClass="labelC" Width="70px" />
                </asp:BoundField>
                
                <asp:BoundField DataField="vSection" HeaderText="Section" >  
                    <ItemStyle CssClass="labelL" Width="100px" />
                </asp:BoundField>
                <asp:BoundField DataField="vProcess" HeaderText="Process" >  
                    <ItemStyle CssClass="labelL" Width="100px" />
                </asp:BoundField>

                <asp:BoundField DataField="SFG_Cd" HeaderText="SFG Code" >  
                    <ItemStyle CssClass="labelC" Width="100px" />
                </asp:BoundField>

                <asp:BoundField DataField="SFG_Descr" HeaderText="Item Description" >
                    <ItemStyle CssClass="labelL" />
                </asp:BoundField> 
                            
                <asp:BoundField DataField="vProdStatus" HeaderText="Status" >  
                    <ItemStyle CssClass="labelL" Width="110px" />
                </asp:BoundField>
                 
            </Columns> 
            <SelectedRowStyle CssClass="activeBar" />
            <PagerStyle Font-Size="8pt" /> 
            <HeaderStyle CssClass="titleBar" />
            <RowStyle CssClass="odd" />
            <AlternatingRowStyle CssClass="even" />
        </asp:GridView>
    </div>--%>

    </form>
</body>
</html>

<script src="../js/jquery-ui-1.10.4.custom.js"></script>
<script src="../js/jquery-ui-1.10.4.custom.min.js"></script>
<script src="../js/jquery-1.10.2.js"></script>
<script src="../js/jquery-ui.js"></script>
      
    <style>
        label {
            display: inline-block;
            width: 100px;
        }

        ._odd { /*light color*/
	        background-color: #f9f9f9; 
        }
       
        ._even { /*Dark color*/
	        background-color: #ffffff; 
        }
         
        .activeBar {
	        background-color: #e9e7e7; 
        }

        .GridButton {
            background: #f2f2f2;  height:22px;
            cursor:pointer; padding:3px; padding-left:7px; padding-right:7px; 
	        text-shadow: 1px 1px 1px #e5e5e5; outline: none; 
            font-weight:bold; color: #434343; background:#93c8ff;
        }
    </style>
    <script type="text/javascript"> 
        $(document).tooltip({
            content: function () {
                return (($(this).prop('title').replace('|', '<br />')));
            }
        });

        function view(pJO,pSFGCode) {
            //alert("lance test");

            var vProperties = "width=1100px, height=600px, top=50px, left=80px, scrollbars=yes";
            ViewComp = window.open("../inventory/joborder-completion.aspx?pJO=" + pJO + "&pSFG=" + pSFGCode, "ViewComp", vProperties);
            ViewComp.focus(); 

        }


        function Modify_JO(vID, vBOM, vBOMRev) {
            $("#h_TranId").val(vID);
            $("#h_BOM").val(vBOM);
            $("#h_BOMRev").val(vBOMRev);
             
            vlastSelected = $("#h_LastSelected").val(); 

            if (vlastSelected != vID) {
                document.getElementById("Tr_a" + vID).style.backgroundColor = "#d8def7";
                document.getElementById("Tr_b" + vID).style.backgroundColor = "#d8def7";
                document.getElementById("Tr_c" + vID).style.backgroundColor = "#d8def7";
                document.getElementById("Tr_d" + vID).style.backgroundColor = "#d8def7";
                document.getElementById("Tr_e" + vID).style.backgroundColor = "#d8def7";
                document.getElementById("Tr_f" + vID).style.backgroundColor = "#d8def7";
                document.getElementById("Tr_g" + vID).style.backgroundColor = "#d8def7";
                document.getElementById("Tr_h" + vID).style.backgroundColor = "#d8def7";
                document.getElementById("Tr_i" + vID).style.backgroundColor = "#d8def7";
                document.getElementById("Tr_j"+ vID).style.backgroundColor = "#d8def7";

                if (vlastSelected == "") {
                    document.getElementById("h_LastSelected").value = vID;
                } else { 
                    document.getElementById("Tr_a" + vlastSelected).style.backgroundColor = "white";
                    document.getElementById("Tr_b" + vlastSelected).style.backgroundColor = "white";
                    document.getElementById("Tr_c" + vlastSelected).style.backgroundColor = "white";
                    document.getElementById("Tr_d" + vlastSelected).style.backgroundColor = "white";
                    document.getElementById("Tr_e" + vlastSelected).style.backgroundColor = "white";
                    document.getElementById("Tr_f" + vlastSelected).style.backgroundColor = "white";
                    document.getElementById("Tr_g" + vlastSelected).style.backgroundColor = "white";
                    document.getElementById("Tr_h" + vlastSelected).style.backgroundColor = "white";
                    document.getElementById("Tr_i" + vlastSelected).style.backgroundColor = "white";
                    document.getElementById("Tr_j" + vlastSelected).style.backgroundColor = "white";
                    document.getElementById("h_LastSelected").value = vID;
                }

            }

        }
        function invoke() {
            <%=vScript %>
        }
         
        $(document).ready(function () {
            $(document).ready(function () {
                $("#txtDateFrom").datepicker();
                 
                //alert( + " -- lance add date");
                $("#txtDateTo").datepicker(); 
            });

            $('#btnHeaderEdit').click(function (event) { 
                //event.preventDefault();
                var vProperties = "width=1200px, height=700px, top=50px, left=80px, scrollbars=yes";
                winPopJO = window.open("../inventory/joborder.aspx?pBom=" + 
                    $("#h_BOM").val() + "&pBomRev=" +
                    $("#h_BOMRev").val() + "&pMode=edit&pTranId=" +
                    $("#h_TranId").val() + "", "popupWindow-JO", vProperties);
                winPopJO.focus(); 
            });

            $('#btnHeaderAdd').click(function (event) {
                if (this.id == "btnHeaderAdd") { 
                    window.open("../build_bom/bom.aspx?id=4000&pActiveBom=yes", "_self"); 
                }
                
            });
               
        });

        function ResetDate(pID, pHrs, pTtlHrs, pTranId) {
            //alert(pID + ' **** ' + pHrs + ' **** ' + pTtlHrs + ' **** ' + pTranId);

            var xmlhttp = new XMLHttpRequest();
            xmlhttp.open("GET", "joborderlist_xml.aspx?pDate=" + pID + "&pTranId=" + pTranId, true);
            xmlhttp.send();
        }

        
        
    </script>

<script type="text/javascript">
    $(document).ready(function () {
         
    });
     
</script>