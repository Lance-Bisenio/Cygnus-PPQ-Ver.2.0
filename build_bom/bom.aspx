<%@ Page Language="VB" AutoEventWireup="false" CodeFile="bom.aspx.vb" Inherits="bomheader" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server"> 
    <link href="../css/BasicContol.css" rel="stylesheet" type="text/css" /> 
    <link href="../css/jquery_datatable.css" rel="stylesheet" /> 
    <link href="../css/colorbox.css" rel="stylesheet" />
    <link href="../css/jquery-ui.css" rel="stylesheet" />  

   
   
    <title></title> 
    <style type="text/css">
        .h_Element {
            font-size:10px;
        }
        html, body, form {
            overflow: hidden;
            height: 100%;
            width: 100%;
            margin-left: auto;
            margin-right: auto;
            margin-top: auto;
        }

        .iDataFrame {
            width:99%; border: solid 0px #e2e2e2; height: 96%; margin:0px;
        }

       
    </style>
    

</head>
<body onload="invoke();">
    <form id="form1" runat="server">
        <div id="divPop" style="display:none; width:100%">
            <table style="border-collapse:collapse; visibility:visible; width:100%" border="0">
                <tr>
                    <td class="labelL"><h2><b>
                        <%--<input type="text" id="txtLabel" name="txtLabel" runat="server" 
                            style="width:100%; border:solid 0px #ffffff; font-weight:bold; 
                            font-family:'Century Gothic',Arial; font-size:16px;" readonly="readonly"/>--%></b></h2></td>
                </tr>
                 <tr>
                    <td class="labelL">
                        <asp:textbox id="txtReason"  runat="server" Width="98%" Height="100px"  CssClass="labelL" TextMode="MultiLine" ></asp:textbox>
                    </td>
                </tr>
                <tr>
                    <td class="labelL"> 
                        <input type="button" id="btnDelete" value="Save" class="Button"/>
                    </td>
                </tr>
            </table>
        </div>
  
        <table style="position:absolute; top:0px; right:5px; bottom:0px; width:70%; border-collapse:collapse; visibility:hidden;" border="1">
            <tr>
                <td class="labelR">bom header :         <input type="text" id="h_Bom" class="h_Element" name="h_Bom" runat="server"  style="width:45px"/></td> 
                <td class="labelR">bom revision :       <input type="text" id="h_BomRev" class="h_Element" name="h_ItemRev" runat="server" style="width:45px"/></td>
                <td class="labelR">bom item :           <input type="text" id="h_ItemCd" class="h_Element" name="h_ItemCd" runat="server" style="width:45px"/>
                                                        <input type="text" id="h_ItemDescr" class="h_Element" name="h_ItemDecsr" runat="server"  style="width:45px"/>
                </td>
                <td class="labelR">mode :               <input type="text" id="h_Mode" class="h_Element" name="h_Mode" runat="server" style="width:45px"/></td>
                <td class="labelR">bom is active.? :    <input type="text" id="h_BOM_Active" class="h_Element" name="h_BOM_Active" runat="server"  style="width:45px"/></td>
                <td class="labelR">bom selected :       <input type="text" id="h_Selected" class="h_Element" name="h_Selected" runat="server"  style="width:45px"/></td> 
            </tr>
            <tr>
                <td class="labelR">section :            <input type="text" id="h_Sect" class="h_Element" name="h_Sect" runat="server"  style="width:45px"/></td>
                <td class="labelR">process :            <input type="text" id="h_Proc" class="h_Element" name="h_Proc" runat="server"  style="width:45px"/></td>
                <td class="labelR">materials :          <input type="text" id="h_Mat" class="h_Element" name="h_Mat" runat="server"  style="width:45px"/></td>
                <td class="labelR">machine :            <input type="text" id="h_Mach" class="h_Element" name="h_Mach" runat="server"  style="width:45px"/></td>
                <td class="labelR">Perp :               <input type="text" id="h_Perp" class="h_Element" name="h_Perp" runat="server"  style="width:45px"/></td>
                <td class="labelR"></td> 
            </tr>
            <tr>
                <td class="labelR">delete from :        <input type="text" id="h_DeleteFrom" class="h_Element" name="h_DeleteFrom" runat="server"  style="width:45px"/></td>
                <td class="labelR">process tranid :     <input type="text" id="h_ProTranId" class="h_Element" name="h_ProTranId" runat="server"  style="width:45px"/></td>
                <td class="labelR">materials tranid :   <input type="text" id="h_MatTranId" class="h_Element" name="h_MatTranId" runat="server"  style="width:45px"/>  </td>
                <td class="labelR">machine tranid :     <input type="text" id="h_MachTranId" class="h_Element" name="h_MachTranId" runat="server"  style="width:45px"/>    </td>
                <td class="labelR">Perp tranid :        <input type="text" id="h_PerpTranId" class="h_Element" name="h_MachTranId" runat="server"  style="width:45px"/></td>
                <td class="labelR"></td> 
            </tr>
        </table> 
        
        <div style="position:absolute; top:0px; left:5px; right:5px; bottom:0px; overflow:auto; border:solid 0px #ff0000;">
        <%---------------------------------------------------------------------------------------------------------------------------------        
        BOM HEADER OR LIST OF BOM CODES START HERE 
            DATE CREATE : 01.21.2014
        -------------------------------------------------------------------------------------------------------------------------------------%>

        <div style="width:98%; margin:auto; overflow:auto; ">
            <table id="Standard_Tbl" border="0" style="width:100%;  border-collapse:collapse;"> 
                <tr><td style="height:10px;" colspan="6"></td></tr>
                <tr>
                    <td style="width:100px;">Item Type :</td>
                    <td style="width:300px;">
                        <asp:DropDownList ID="cmbItemType" runat="server" Width="210px" CssClass="labelL">
                            <asp:ListItem Value="0" Selected="True">Active</asp:ListItem>
                            <asp:ListItem Value="1">Inactive</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="width:100px;">Status :</td>
                    <td style="width:300px;">
                        <asp:DropDownList ID="cmbStatus" runat="server" Width="210px" CssClass="labelL">
                            <asp:ListItem Value="1" Selected="True">Active</asp:ListItem>
                            <asp:ListItem Value="2">Inactive</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="width:100px;"></td>
                    <td></td>
                </tr>
                <tr>
                    <td>Item UOM :</td>
                    <td>
                        <asp:DropDownList ID="cmbItemUOM" runat="server" Width="210px" CssClass="labelL">
                            <asp:ListItem Value="1" Selected="True">Active</asp:ListItem>
                            <asp:ListItem Value="2">Inactive</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                </tr>
                <tr><td style="height:10px;" colspan="6"></td></tr>
                <tr>
                <td>Quick Search :</td>
                <td>  
                    <asp:TextBox ID="txtSearch" runat="server" CssClass="labelL" Width="204px" Height="18px"></asp:TextBox> 
                </td>
                <td>Search By :</td>
                <td colspan="4">
                    <asp:DropDownList ID="cmbSearchBy" runat="server" Width="210px" CssClass="labelL" >
                    </asp:DropDownList> 
                    <asp:Button ID="btnSearch" CssClass="Button" runat="server"  Text="Search" />  
                    <asp:Button ID="btnViewDetails" CssClass="Button" runat="server"  Text="View BOM Details" />  
                    <input type="button" runat="server" id="btnDupBOM" name="btnDupBOM" value="Duplicate BOM" class="Button" data-href="divPop" visible="false" />
                    <input type="button" runat="server" id="btnJO" name="btnDupBOM0" value="Create Job Order" class="Button" visible="false" />
                </td>
            </tr> 
        </table> 

            <%--====================================================================================================================================================--%>
            <%--<table id="HeaderBom" class="display" style="border-right:solid 1px #ccc;">
                <thead>
                    <tr>   
                        <%=vColHeader %> 
                    </tr>
                </thead>
                <tbody> 
                    <%=vRecords %>
                </tbody>
            </table>--%>
            <%--====================================================================================================================================================--%>
              
        </div>   
        <br />
        <%---------------------------------------------------------------------------------------------------------------------------------        
        BOM PROCESS CODES START HERE 
            DATE CREATE : 01.21.2014
        -----------------------------------------------------------------------------------------------------------------------------------
        --%>

        <table style="width:100%; margin:auto; border: solid 0px; border-collapse:collapse;" border="0">
            <tr>
                <td>
                    <table id="Table1" class="titleBarTop"  border="0" style="width:100%;  border-collapse:collapse;">
                        <tr style="border:solid 0px #cccccc; border-bottom:0px;">
                            <td style="width:320px; text-align:left;">
                                &nbsp;&nbsp;
                                    <input type="button" runat="server" id="btnHeaderAdd" name="btnProAdd" value="Add"  class="tblButtonA" />  
                                    <input type="button" runat="server" id="btnHeaderEdit" name="btnProEdit" value="Edit"  class="tblButtonE" />  
                                    <input type="button" runat="server" id="btnHeaderDel" name="" value="Delete" class="tblButtonD" data-href="divPop" />
                                
                                Show :<asp:DropDownList ID="cmbShow" runat="server" Width="70px" AutoPostBack="True" CssClass="labelL">
                                </asp:DropDownList> 

                            </td>
                            <td style="text-align:left; padding-left :5px;"></td>
                            <td class="labelR" >
                                <b><asp:Label ID="lblTotalDocs" runat="server" CssClass="labelL" Text="Documents Retrieved : 0" ForeColor="#ffffff"></asp:Label></b>&nbsp;&nbsp;&nbsp;
                            </td>
                        </tr>
                    </table>
                    <asp:GridView ID="tbl_MasterBom" runat="server" AllowPaging="True" BorderColor="#cccccc"
                        AutoGenerateColumns="False" Width="100%" BorderStyle="Solid" BorderWidth="1px" 
                        CssClass="mainGridView" PageSize="15">
                        <Columns>
                            <asp:CommandField ButtonType="Button" ShowSelectButton="True" > 
                                <ItemStyle CssClass="labelC" Width="40px" />
                                <ControlStyle CssClass="button" />
                            </asp:CommandField>
                            <asp:TemplateField HeaderText="#" HeaderStyle-Width="30px" >   
                                <ItemTemplate>
                                    <%# Container.DataItemIndex + 1 %> 
                                </ItemTemplate> 
                                <HeaderStyle Width="30px"></HeaderStyle>
                            </asp:TemplateField>
                
                            <asp:BoundField DataField="BOM_Cd" HeaderText="BOM Code" >  
                                <ItemStyle CssClass="labelC" Width="50px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Revision" HeaderText="Rev" >
                                <ItemStyle CssClass="labelC" Width="40px" />
                            </asp:BoundField>
                            
                            <asp:TemplateField HeaderText="GCAS">
                                <ItemTemplate>
                                    <asp:Label ID="Label1" runat="server" Text='<%# GetGCAS(Eval("Item_Cd"))%>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="labelC" Width="90px" />
                            </asp:TemplateField>

                            <asp:BoundField DataField="Item_Cd" HeaderText="Item Code" >
                                <ItemStyle CssClass="labelC" Width="100px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Item_Name" HeaderText="Item Description" >
                                <ItemStyle CssClass="labelL" />
                            </asp:BoundField> 

                            <asp:BoundField DataField="ItemType" HeaderText="Item Type" >
                                <ItemStyle CssClass="labelC" Width="100px" />
                            </asp:BoundField> 
                                
                           <%--
                               
                            <asp:BoundField DataField="vGCAS" HeaderText="GCAS No." >
                                <ItemStyle CssClass="labelC" Width="80px" />
                            </asp:BoundField> 

                            <asp:BoundField DataField="vWeightUOM" HeaderText="Weight UOM" >
                                <ItemStyle CssClass="labelC" Width="80px" />
                            </asp:BoundField> --%>
                            <asp:BoundField DataField="StdQty" HeaderText="Min Order QTY" >
                                <ItemStyle CssClass="labelR" Width="70px" />
                            </asp:BoundField> 
                            <asp:BoundField DataField="vUOM" HeaderText="Item UOM" >
                                <ItemStyle CssClass="labelL" Width="80px" />
                            </asp:BoundField> 
                            
                            <asp:TemplateField HeaderText="Prod Hrs">
                                <ItemTemplate>
                                    <asp:Label ID="Label1" runat="server" Text='<%# GetProdHrs(Eval("BOM_Cd"), Eval("Revision"))%>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="labelC" Width="60px" />
                            </asp:TemplateField>

                            <%--<asp:BoundField DataField="vStdOrderUOM" HeaderText="Min Order UOM" >
                                <ItemStyle CssClass="labelL" Width="80px" />
                            </asp:BoundField> --%>
                            <%--<asp:BoundField DataField="StdProdRun" HeaderText="Prod Lead Day / Time" >
                                <ItemStyle CssClass="labelC" Width="70px" />
                            </asp:BoundField> 
  --%>
                            

                            <asp:BoundField DataField="Status_Cd" HeaderText="Status" >
                                <ItemStyle CssClass="labelL" Width="60px" />
                            </asp:BoundField> 
                           
                            <asp:BoundField DataField="CreatedBy" HeaderText="Modify By" >
                                <ItemStyle CssClass="labelL" Width="70px" />
                            </asp:BoundField> 

                            <asp:BoundField HeaderText="Last Date Modify" DataField="DateCreated" SortExpression="ClientDueDate" DataFormatString="{0:MM/dd/yyyy}" HtmlEncode="false" > 
                                <ItemStyle Width="5%" HorizontalAlign="Center" /> 
                                <HeaderStyle Wrap="True" /> 
                            </asp:BoundField>
                            <asp:BoundField DataField="Remarks" HeaderText="Remarks" >
                                <ItemStyle CssClass="labelL" />
                            </asp:BoundField> 

                            <%--<asp:TemplateField HeaderText="Last Date Modify">
                                <ItemTemplate>
                                    <asp:Label ID="Label1" runat="server" Text='<%# FormatDate(Eval("DateCreated"))%>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="labelC" Width="80px" />
                            </asp:TemplateField>  --%>
                        </Columns> 
                        <SelectedRowStyle CssClass="activeBar" />
                        <PagerStyle Font-Size="8pt" /> 
                        <HeaderStyle CssClass="titleBar" />
                        <RowStyle CssClass="odd" />
                        <AlternatingRowStyle CssClass="even" />
                    </asp:GridView><br />
                </td>
            </tr>
            <tr>
                <td style="vertical-align:top; text-align:left;">
                    <br />
                    <div id="divProcess" runat="server">
                        <table id="Table2" class="titleBarTop" border="0" style="width:100%;  border-collapse:collapse;">
                            <tr style="border:solid 0px #cccccc; border-bottom:0px;">
                                <td style="text-align:left;">&nbsp;&nbsp;
                                    <input type="button" runat="server" id="btnProcessAdd" name="btnProAdd" value="Add"  class="tblButtonA" /> 
                                    <input type="button" runat="server" id="btnProcessEdit" name="btnProEdit" value="Edit"  class="tblButtonE" />    
                                    <input type="button" runat="server" id="btnProcessDelete" name="btnProDelete" value="Delete" class="tblButtonD" data-href="divPop" />
                                </td>
                                <td style="width:150px; text-align:left; padding-left :5px;"> 
                                </td>
                                <td class="labelR" >
                                    <b><asp:Label ID="lblTotalProcess" runat="server" CssClass="labelL" Text="BOM Process Retrieved : 0" ForeColor="#ffffff"></asp:Label></b>&nbsp;&nbsp;&nbsp;
                                </td>
                            </tr>
                        </table>  
                        <asp:GridView ID="tbl_ProcessList" runat="server" AllowPaging="True" BorderColor="#cccccc"
                            AutoGenerateColumns="False" Width="100%" BorderStyle="Solid" BorderWidth="1px"
                            CssClass="mainGridView">  <%--PageSize="5"--%>

                            <Columns>
                             
                                <asp:CommandField ButtonType="Button" ShowSelectButton="True" > 
                                    <ItemStyle CssClass="labelC" Width="40px" />
                                    <ControlStyle CssClass="button" />
                                </asp:CommandField>

                                <asp:BoundField DataField="TranId" HeaderText="Tran ID" HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn" ItemStyle-width="80px"/>
                                <asp:BoundField DataField="Sect_Cd" HeaderText="Section" HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn" ItemStyle-width="80px"/>
                                <asp:BoundField DataField="Proc_Cd" HeaderText="Process" HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn" ItemStyle-width="80px"/>

                                <asp:BoundField DataField="OperOrder" HeaderText="Oper Order" >
                                    <ItemStyle CssClass="labelC" Width="80px" />
                                </asp:BoundField>  
                               <%-- <asp:BoundField DataField="Sect_Cd" HeaderText="Sect Code" >
                                    <ItemStyle CssClass="labelL" Width="80px" />
                                </asp:BoundField>--%>
                                <asp:BoundField DataField="Sect_Name" HeaderText="Section Descr" >
                                    <ItemStyle CssClass="labelL" Width="120px" />
                                </asp:BoundField>
                               <%-- <asp:BoundField DataField="Proc_Cd" HeaderText="Proc Code" >
                                    <ItemStyle CssClass="labelC" Width="80px" />
                                </asp:BoundField>--%>
                                 <asp:BoundField DataField="Proc_Name" HeaderText="Process Description" >
                                    <ItemStyle CssClass="labelL" Width="200px"  />
                                </asp:BoundField> 
                                <asp:BoundField DataField="SFG_Cd" HeaderText="SFG Code" >
                                    <ItemStyle CssClass="labelC" Width="100px"  />
                                </asp:BoundField>   
                                <asp:BoundField DataField="SFG_Descr" HeaderText="Semi Finish Good " >
                                    <ItemStyle CssClass="labelL"/>
                                </asp:BoundField>  
                                <asp:BoundField DataField="Meters" HeaderText="Meters"> <%--DataFormatString="{0:N0}"--%>
                                    <ItemStyle CssClass="labelR" Width="80px" />
                                </asp:BoundField>   
                                <asp:BoundField DataField="Kilos" HeaderText="Kilos">  <%--DataFormatString="{0:N0}"--%>
                                    <ItemStyle CssClass="labelR" Width="80px" />
                                </asp:BoundField>    
                                <%--<asp:BoundField DataField="QtyOut" HeaderText="Qty Output" DataFormatString="{0:N0}">
                                    <ItemStyle CssClass="labelR" Width="80px" />
                                </asp:BoundField>   --%>
                            </Columns>
            
                            <SelectedRowStyle CssClass="activeBar" />
                            <PagerStyle Font-Size="8pt" /> 
                            <HeaderStyle CssClass="titleBar" />
                            <RowStyle CssClass="odd" />
                            <AlternatingRowStyle CssClass="even" />
                        </asp:GridView>
                    </div> 
                </td>
            </tr>
        </table>
        <br />
        <table style="width:100%; margin:auto; border: solid 0px; border-collapse:collapse;" border="0">
            <tr><td style="height:10px;" colspan="2"></td></tr>
            <tr>
                <td style="vertical-align:top; width:50%; text-align:left;">
                    <div id="divMaterials" runat="server">
                        <table id="Table5" class="titleBarTop" border="0" style="width:99%;  border-collapse:collapse;">
                            <tr style="border:solid 0px #8B8B8A; border-bottom:0px;">
                                <td style="text-align:left;">&nbsp;&nbsp;
                                    <input type="button" runat="server" id="btnMatAdd" name="btnMatrAdd" value="Add" class="tblButtonA" />      
                                    <input type="button" runat="server" id="btnMatEdit" name="btnMatEdit" value="Edit" class="tblButtonE" />      
                                    <input type="button" runat="server" id="btnMatrDel" name="btnMatrDel" value="Delete" class="tblButtonD" data-href="divPop" />
                                </td>
                                <td style="width:150px; text-align:left; padding-left :5px;"> 
                                </td>
                                <td class="labelR" >
                                    <b><asp:Label ID="lblTotalMat" runat="server" CssClass="labelL" Text="BOM Materials Retrieved : 0" ForeColor="#ffffff"></asp:Label></b>&nbsp;&nbsp;&nbsp;
                                </td>
                            </tr>
                        </table> 
                        <asp:GridView ID="tbl_Materials" runat="server" AllowPaging="True" BorderColor="#cccccc"
                            AutoGenerateColumns="False" Width="99%" BorderStyle="Solid" BorderWidth="1px"
                            CssClass="mainGridView" PageSize="30"><%-- PageSize="5">--%>
                            <Columns>
                                <asp:CommandField ButtonType="Button" ShowSelectButton="True" > 
                                    <ItemStyle CssClass="labelC" Width="40px" />
                                    <ControlStyle CssClass="button" />
                                </asp:CommandField> 
                                <asp:BoundField DataField="TranId" HeaderText="TranId" HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn" ItemStyle-width="80px"/>
                                <asp:BoundField DataField="Item_Cd" HeaderText="Material Code" >
                                    <ItemStyle CssClass="labelL" Width="100px" />
                                </asp:BoundField>  
                                <asp:BoundField DataField="ItemName" HeaderText="Material Description" >
                                    <ItemStyle CssClass="labelL" />
                                </asp:BoundField>
                           
                                <asp:BoundField DataField="Grams" HeaderText="Grams" >
                                    <ItemStyle CssClass="labelR" Width="70px" />
                                </asp:BoundField> 

                                <asp:BoundField DataField="Percentage" HeaderText="Percentage" >
                                    <ItemStyle CssClass="labelR" Width="70px" />
                                </asp:BoundField> 

                                <asp:BoundField DataField="Qty" HeaderText="Qty" >
                                    <ItemStyle CssClass="labelR" Width="70px" />
                                </asp:BoundField> 
                                <asp:BoundField DataField="QtyUom" HeaderText="UOM" >
                                    <ItemStyle CssClass="labelC" Width="70px" />
                                </asp:BoundField> 
                                <asp:BoundField DataField="Parent_TranId" HeaderText="Parent" HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn" ItemStyle-width="80px"/>
                            </Columns>
            
                            <SelectedRowStyle CssClass="activeBar" />
                            <PagerStyle Font-Size="8pt" /> 
                            <HeaderStyle CssClass="titleBar" />
                            <RowStyle CssClass="odd" />
                            <AlternatingRowStyle CssClass="even" />
                        </asp:GridView>
                    </div> 
                </td>
                <td style="vertical-align:top; text-align:right;">
                    <div id="divMach" runat="server" style="visibility:hidden">
                        <table id="Table3" class="titleBarTop" border="0" style="width:100%;  border-collapse:collapse;">
                            <tr style="border:solid 0px #8B8B8A; border-bottom:0px;">
                                <td style="text-align:left;">&nbsp;&nbsp;
                                    <input type="button" runat="server" id="btnMachAdd" name="btnMacAdd" value="Add" class="tblButtonA" />  
                                    <input type="button" runat="server" id="btnMachEdit" name="btnMacEdit" value="Edit" class="tblButtonE" /> 
                                    <input type="button" runat="server" id="btnMachDel" name="btnMacDel" value="Delete" class="tblButtonD" data-href="divPop" />
                                </td>
                                <td style="width:150px; text-align:left; padding-left :5px;"></td>
                                <td class="labelR" >
                                    <b><asp:Label ID="lblTotalMachine" runat="server" CssClass="labelL" Text="Total BOM Machine Retrieved : 0" ForeColor="#ffffff"></asp:Label></b>&nbsp;&nbsp;&nbsp;
                                </td>
                            </tr>
                        </table> 
                        <asp:GridView ID="tbl_MachineList" runat="server" AllowPaging="True" BorderColor="#cccccc"
                            AutoGenerateColumns="False" Width="100%" BorderStyle="Solid" BorderWidth="1px"
                            CssClass="mainGridView"><%-- PageSize="5">--%>
                            <Columns>
                                <asp:CommandField ButtonType="Button" ShowSelectButton="True" > 
                                    <ItemStyle CssClass="labelC" Width="40px" />
                                    <ControlStyle CssClass="button" />
                                </asp:CommandField>
                                <asp:BoundField DataField="TranId" HeaderText="TranId" HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn" ItemStyle-width="80px"/>
                                <asp:BoundField DataField="Mach_Cd" HeaderText="Mach Code" >
                                    <ItemStyle CssClass="labelC" Width="80px" />
                                </asp:BoundField> 
                                <asp:BoundField DataField="MachDescr" HeaderText="Machine Description" >
                                    <ItemStyle CssClass="labelL" />
                                </asp:BoundField> 
                                <asp:BoundField DataField="CapUnit" HeaderText="Cap Unit" >
                                    <ItemStyle CssClass="labelR" Width="60px" />
                                </asp:BoundField> 
                                <asp:BoundField DataField="UomDescr" HeaderText="UOM" >
                                    <ItemStyle CssClass="labelL" Width="60px" />
                                </asp:BoundField> 
                                <%--<asp:BoundField DataField="Minutes" HeaderText="Minutes" >
                                    <ItemStyle CssClass="labelR" Width="60px" />
                                </asp:BoundField>  --%>
                                <asp:TemplateField HeaderText="Default">
                                    <ItemTemplate>
                                        <asp:Label ID="Label1" runat="server" Text='<%# IIf(Eval("Default ")=1,"Warehouse","Next Process")%>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle CssClass="labelC" Width="80px" />
                                </asp:TemplateField>   
                                 <asp:BoundField DataField="Parent_TranId" HeaderText="Parent" HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn" ItemStyle-width="80px"/>
                            </Columns>
            
                            <SelectedRowStyle CssClass="activeBar" />
                            <PagerStyle Font-Size="8pt" /> 
                            <HeaderStyle CssClass="titleBar" />
                            <RowStyle CssClass="odd" />
                            <AlternatingRowStyle CssClass="even" />
                        </asp:GridView>
                    </div> 
                </td>
            </tr>
        </table> 

        <table style="width:100%; margin:auto; border: solid 0px; border-collapse:collapse; visibility:hidden" border="0">
            <tr><td style="height:10px;" colspan="2"></td></tr>
            <tr>
                <td style="vertical-align:top; width:50%; text-align:left;">
                    <div id="divQA" runat="server">
                        <table id="Table7" class="titleBarTop" border="0" style="width:99%;  border-collapse:collapse;">
                            <tr style="border:solid 0px #8B8B8A; border-bottom:0px;">
                                <td style="text-align:left;">&nbsp;&nbsp;
                                    <%--<input type="button" runat="server" id="Button1" name="btnPerpAdd" value="Add" onclick="OpenMacPerp(id);" class="tblButtonA" />      
                                    <input type="button" runat="server" id="Button2" name="btnPerpEdit" value="Edit" onclick="OpenMacPerp(id);" class="tblButtonE" />      
                                    <input type="button" runat="server" id="Button3" name="btnPerpDel" value="Delete" onclick="OpenMacPerp();" class="tblButtonD" />--%>
                                </td>
                                <td style="width:150px; text-align:left; padding-left :5px;"></td>
                                <td class="labelR" >
                                    <b><asp:Label ID="lblTotalQA" runat="server" CssClass="labelL" Text="Process QA Retrieved : 0" ForeColor="#ffffff"></asp:Label></b>&nbsp;&nbsp;&nbsp;
                                </td>
                            </tr>
                        </table>
                        <asp:GridView ID="tbl_QAList" runat="server" AllowPaging="True" BorderColor="#cccccc"
                            AutoGenerateColumns="False" Width="99%" BorderStyle="Solid" BorderWidth="1px"
                            CssClass="mainGridView"><%-- PageSize="5">--%>
                            <Columns>
                                <%--<asp:CommandField ButtonType="Button" ShowSelectButton="True" > 
                                    <ItemStyle CssClass="labelC" Width="40px" />
                                    <ControlStyle CssClass="button" />
                                </asp:CommandField>--%>
                                <asp:BoundField DataField="vCode" HeaderText="QA Code" >
                                    <ItemStyle CssClass="labelC" Width="100px" />
                                </asp:BoundField>
                                    <asp:BoundField DataField="vDescr" HeaderText="QA Description" >
                                    <ItemStyle CssClass="labelL" />
                                </asp:BoundField>  
                            </Columns>
                            <SelectedRowStyle CssClass="activeBar" />
                            <PagerStyle Font-Size="8pt" /> 
                            <HeaderStyle CssClass="titleBar" />
                            <RowStyle CssClass="odd" />
                            <AlternatingRowStyle CssClass="even" />
                        </asp:GridView>
                    </div>
                </td>
                <td style="vertical-align:top; text-align:right;">
                    <div id="divPerp" runat="server">
                        <table id="Table4" class="titleBarTop" border="0" style="width:100%;  border-collapse:collapse;">
                            <tr style="border:solid 0px #8B8B8A; border-bottom:0px;">
                                <td style="text-align:left;">&nbsp;&nbsp;
                                    <input type="button" runat="server" id="btnPerpAdd" name="btnPerpAdd" value="Add" class="tblButtonA" />      
                                    <input type="button" runat="server" id="btnPerpEdit" name="btnPerpEdit" value="Edit" class="tblButtonE" />      
                                    <input type="button" runat="server" id="btnPerpDel" name="btnPerpDel" value="Delete" class="tblButtonD" data-href="divPop" />
                                </td>
                                <td style="width:150px; text-align:left; padding-left :5px;"></td>
                                <td class="labelR" >
                                    <b><asp:Label ID="lblTotalPerp" runat="server" CssClass="labelL" Text="Machine Peripherals Retrieved : 0" ForeColor="#ffffff"></asp:Label></b>&nbsp;&nbsp;&nbsp;
                                </td>
                            </tr>
                        </table>
                        <asp:GridView ID="tbl_PerpList" runat="server" AllowPaging="True" BorderColor="#cccccc"
                            AutoGenerateColumns="False" Width="100%" BorderStyle="Solid" BorderWidth="1px"
                            CssClass="mainGridView" PageSize="5" >
                            <Columns>
                                <asp:CommandField ButtonType="Button" ShowSelectButton="True" > 
                                    <ItemStyle CssClass="labelC" Width="40px" />
                                    <ControlStyle CssClass="button" />
                                </asp:CommandField>
                                <asp:BoundField DataField="TranId" HeaderText="TranId" HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn" ItemStyle-width="80px"/>
                                <asp:BoundField DataField="Item_Cd" HeaderText="Perip Code" >
                                    <ItemStyle CssClass="labelC" Width="100px" />
                                </asp:BoundField>
                                    <asp:BoundField DataField="MatDescr" HeaderText="Peripherals Description" >
                                    <ItemStyle CssClass="labelL" />
                                </asp:BoundField>  
                                <asp:BoundField DataField="Parent_TranId" HeaderText="Parent" HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn" ItemStyle-width="80px"/>
                            </Columns>
                            <SelectedRowStyle CssClass="activeBar" />
                            <PagerStyle Font-Size="8pt" /> 
                            <HeaderStyle CssClass="titleBar" />
                            <RowStyle CssClass="odd" />
                            <AlternatingRowStyle CssClass="even" />
                        </asp:GridView>
                    </div>
                </td>
            </tr>
        </table> 
             
        </div>
    </form>
</body>
</html>

<script src="../js/jquery-1.10.2.js" type="text/javascript"></script>
<script src="../media/js/jquery.dataTables.js" type="text/javascript"> </script>
<script src="../js/jquery.colorbox-min.js" type="text/javascript">  </script>

<script>
    $(document).ready(function () {
        var vProperties = "width=1200px, height=600px, top=50px, left=80px, scrollbars=yes";
        var vParam = "&id=4000&pBom=" + $('#h_Bom').val() +
                        "&pBomRev=" + $('#h_BomRev').val() +
                        "&pProTranId=" + $('#h_ProTranId').val() +
                        "&pSect=" + $('#h_Sect').val() +
                        "&pProc=" + $('#h_Proc').val() +
                        "&pMatTranId=" + $('#h_MatTranId').val() +
                        "&pMachTranId=" + $('#h_MachTranId').val() +
                        "&pPerpTranId=" + $('#h_PerpTranId').val() + "";

        $('#btnHeaderAdd').click(function (event) {
            event.preventDefault();
            winPop = window.open("bom_buildheader.aspx?mode=new" + vParam + "", "popupWindow", vProperties);
            winPop.focus();
        });

        $('#btnHeaderEdit').click(function (event) {
            event.preventDefault(); 
            winPop = window.open("bom_buildheader.aspx?mode=" + $('#btnHeaderEdit').val() + "" + vParam + "", "popupWindow", vProperties);
            winPop.focus();
        });

        // PROCESS =========================================================================================================================================================== 
        vProperties = "width=1200px, height=400px, top=50px, left=80px, scrollbars=yes";
        $('#btnProcessAdd').click(function (event) {
            event.preventDefault();
            winPop = window.open("bom_buildprocess.aspx?mode=new" + vParam + "", "popupWindow", vProperties);
            winPop.focus();
        });

        $('#btnProcessEdit').click(function (event) {
            event.preventDefault();
            winPop = window.open("bom_buildprocess.aspx?mode=" + $('#btnProcessEdit').val() + "" + vParam + "", "popupWindow", vProperties);
            winPop.focus();
        });
             
        // MATERIALS =========================================================================================================================================================
        vProperties = "width=1200px, height=630px, top=50px, left=80px, scrollbars=yes";
        $('#btnMatAdd').click(function (event) {
            event.preventDefault();
            winPop = window.open("bom_buildmaterials.aspx?mode=new" + vParam + "", "popupWindow", vProperties);
            winPop.focus();
        });

        $('#btnMatEdit').click(function (event) {
            event.preventDefault();
            winPop = window.open("bom_buildmaterials.aspx?mode=" + $('#btnMatEdit').val() + "" + vParam + "", "popupWindow", vProperties);
            winPop.focus();
        });

        // MACHINE ===========================================================================================================================================================
        $('#btnMachAdd').click(function (event) {
            event.preventDefault();
            winPop = window.open("bom_buildmachine.aspx?mode=new" + vParam + "", "popupWindow", vProperties);
            winPop.focus();
        });

        $('#btnMachEdit').click(function (event) {
            event.preventDefault();
            winPop = window.open("bom_buildmachine.aspx?mode=" + $('#btnMachEdit').val() + "" + vParam + "", "popupWindow", vProperties);
            winPop.focus();
        });

        // PERIPHERALS =======================================================================================================================================================
        $('#btnPerpAdd').click(function (event) {
            event.preventDefault();
            winPop = window.open("bom_buildperipherals.aspx?mode=new" + vParam + "", "popupWindow", vProperties);
            winPop.focus();
        });

        $('#btnPerpEdit').click(function (event) {
            event.preventDefault(); 
            winPop = window.open("bom_buildperipherals.aspx?mode=" + $('#btnPerpEdit').val() + "" + vParam + "", "popupWindow", vProperties);
            winPop.focus();
        });

        // CREATE JOB ORDER =================================================================================================================================================
        $('#btnJO').click(function (event) {
            event.preventDefault();
            winPop = window.open("../inventory/joborder.aspx?pBom=" + $('#h_Bom').val() + "&pBomRev=" + $('#h_BomRev').val() + "&pMode=new",
                "popupWindow", "width=1200px, height=700px, top=50px, left=80px, scrollbars=yes");
            winPop.focus();
        });
            
        // DELETE BOM/PROCESS/MATERIALS/MACHINE/PERIPHERALS ================================================================================================================= 
        $('.tblButtonD,#btnDupBOM').click(function (event) {
            var vPopMess = "";
            var vParam_win_settings = "width=450px, height=200px, top=50px, left=80px, scrollbars=yes";
            var vDeleteParam = "";

            if (this.id == "btnHeaderDel") { 
                vDeleteParam = "&pType=Delete_Bom&pWinType=confirm&pTitle=Delete message alert&pMess=BOM"
                vParam = "&id=4000&pBom=" + $('#h_Bom').val() +
                        "&pBomRev=" + $('#h_BomRev').val() + "";
                        

            } else if (this.id == "btnProcessDelete") { 
                vDeleteParam = "&pType=Delete_Bom&pWinType=confirm&pTitle=Delete message alert&pMess=Process"
                vParam = "&id=4000&pBom=" + $('#h_Bom').val() +
                        "&pBomRev=" + $('#h_BomRev').val() +
                        "&pTranId=" + $('#h_ProTranId').val() + "";
                         

            } else if (this.id == "btnMatrDel") { 
                vDeleteParam = "&pType=Delete_Bom&pWinType=confirm&pTitle=Delete message alert&pMess=Materials"
                vParam = "&id=4000&pBom=" + $('#h_Bom').val() +
                        "&pBomRev=" + $('#h_BomRev').val() + 
                        "&pTranId=" + $('#h_MatTranId').val() + "";
                          

            } else if (this.id == "btnMachDel") { 
                vDeleteParam = "&pType=Delete_Bom&pWinType=confirm&pTitle=Delete message alert&pMess=Machines"
                vParam = "&id=4000&pBom=" + $('#h_Bom').val() +
                        "&pBomRev=" + $('#h_BomRev').val() + 
                        "&pTranId=" + $('#h_MachTranId').val() + "";

            } else if (this.id == "btnPerpDel") { 
                vDeleteParam = "&pType=Delete_Bom&pWinType=confirm&pTitle=Delete message alert&pMess=Peripherals"
                vParam = "&id=4000&pBom=" + $('#h_Bom').val() +
                        "&pBomRev=" + $('#h_BomRev').val() + 
                        "&pTranId=" + $('#h_PerpTranId').val() + "";

            } else if (this.id == "btnDupBOM") { 
                vParam_win_settings = "width=600px, height=300px, top=50px, left=80px, scrollbars=yes";
                vDeleteParam = "&pType=Duplicate_Bom&pWinType=remarks&pTitle=Duplicate Bill of Materials&pMess=Duplicate"
                vParam = "&id=4000&pBom=" + $('#h_Bom').val() +
                        "&pBomRev=" + $('#h_BomRev').val() + "";
            }

            event.preventDefault();
            winPop = window.open("../global_forms/confirm.aspx?mode=Delete_BOM" + vParam + vDeleteParam + "",
                "SmallWindow", "width=600px, height=300px, top=50px, left=80px, scrollbars=yes");
            winPop.focus();
        });

              
    }); 

    function ShowTabValue(vLink) {
        document.getElementById("frmContent").src = vLink; 
    }
    function invoke() {
        <%=vScript %> 
    }
</script>