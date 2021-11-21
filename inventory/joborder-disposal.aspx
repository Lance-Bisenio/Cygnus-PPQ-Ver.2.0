<%@ Page Language="VB" AutoEventWireup="false" CodeFile="joborder-disposal.aspx.vb" Inherits="inventory_jocomplition" %>

<!DOCTYPE html>

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
        .RadioButtonList
        {
            font-size:1.4em;
            color:#243C7A;
            padding-left:6px;    
            width:100%;
            line-height:1.4em;  
            margin:2px 25px 0 0; 
        }
         #rdoTransferTo tbody tr td input {
            height:16px;
            vertical-align:text-bottom;
         }

         #rdoTransferTo tbody tr td label {
            margin-bottom :15px;
         }
         #txtWeight {
            padding-right:8px; font-size:16px;
         }

         #txtCoreWeight {
            padding-right:8px; font-size:16px;
         }

         .trLink {
            cursor:pointer;
            width:50px; border-bottom: solid 1px #efefef; padding-top: 15px; padding-bottom: 15px; text-align:center;
         }

         .trLink:hover {
            background: #f8f7f7;    
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
             $("#txtWeight").focus();
         });
         
         function ModifyCompletion(pTrand) {
            
             $("#h_TranId").val(pTrand);
             $("#h_Mode").val("Edit");
             form1.submit();
         }
         
         $(document).ready(function () {
             $("#txtWeight").bind('keyup change click', function () {

                 if ($.trim($(this).val()) != "") {
                     if ($(this).val().match(/^\d+$/)) {
                     } else {
                         //alert("Not a number");
                         $(this).val($(this).val().replace(/[^0.-9]+/gi, ""));
                     }
                 }
             });


             $("#txtCoreWeight").bind('keyup change click', function () {

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
    <input type="hidden" id="h_TranId" runat="server" name="h_TranId" />
    <input type="hidden" id="h_Mode" runat="server" name="h_Mode" />


    <div style="width:100%; margin:auto; border:0px solid #000;">
        <table id="Standard_Tbl" style="width:100%;" border="0">
             <tr>
                <td style="text-align:left; border-bottom: solid 1px #808080;" colspan="6">
                    <asp:Label ID="txtTitle" CssClass="vModuleTitle" runat="server" Text="Waste Management"></asp:Label> 
                </td>
            </tr>
            <tr><td style="height:15px;"></td></tr>
        </table>

        <table style="width:100%;" border="0">
            <tr>
                <td style="width:20%; vertical-align:top;"> 

                    <div style="width:99%; height:350px; overflow:auto; border: solid 1px #cccccc; margin:auto; vertical-align:top;">           
                        
                        <table style="width:95%; margin:auto; border-collapse:collapse; height: 32px; " border="0">
                            <tr>
                                <td style="text-align:left; border-bottom: solid 1px #808080;" >
                                    <asp:Label ID="Label2" CssClass="vModuleTitle" runat="server" Text="Waste Batch no."></asp:Label> 
                                </td> 
                            </tr> 
                        </table>    
                                     
                        <table style="width:95%; margin:auto; border-collapse:collapse; " border="0">
                            <tr><td style="height:3px;"></td></tr>
                            <%=vCompletionList %>
                        </table>    
                    </div> 

                </td>
                <td style="width:80%; vertical-align:top;">
                    <div style="width:99%; height:350px; overflow:auto; float:right; border: solid 1px #cccccc;">
                        <table style="width:97%; margin:auto; border-collapse:collapse; margin:auto; " border="0">
                            <tr><td style="height:10px;"></td></tr>
                            
                            <tr>
                                <td class="labelR" style="width:100px">Item Code :</td>
                                <td class="labelL">
                                    <asp:Label ID="lblFG" runat="server" Text="Label"></asp:Label>
                                </td>
                                <td class="labelR" style="width:10px"></td>
                                <td class="labelL" style="width:10px"></td>
                            </tr>
                            <tr>
                                <td class="labelR" style="vertical-align:top;">Description :</td>
                                <td class="labelL" style="vertical-align:top;">
                                    <asp:Label ID="lblFGDescr" runat="server" Text="Label"></asp:Label>
                                </td>
                                <td class="labelR" style="vertical-align:top;"></td>
                                <td class="labelL" style="vertical-align:top;"></td>
                            </tr>
                            <tr><td style="height:10px;" colspan="4"></td></tr>
                            <tr>
                                <td class="labelR">SFG Code :</td>
                                <td class="labelL">
                                    <asp:Label ID="lblSFGCd" runat="server" Text="Label"></asp:Label>
                                </td>
                                <td class="labelR"></td>
                                <td class="labelL"></td>
                            </tr>
                            <tr>
                                <td class="labelR" style="vertical-align:top;">Description :</td>
                                <td class="labelL" style="vertical-align:top;">
                                    <asp:Label ID="lblSFGDescr" runat="server" Text="Label"></asp:Label>
                                </td>
                                <td class="labelR" style="vertical-align:top;"></td>
                                <td class="labelL" style="vertical-align:top;"></td>
                            </tr>
                            <tr><td style="height:10px;" colspan="4"></td></tr>
                            <tr>
                                <td class="labelR">JO Number :</td>
                                <td class="labelL">
                                    <asp:Label ID="lblJONO" runat="server" Text="Label"></asp:Label>
                                </td>
                                <td class="labelR"></td>
                                <td class="labelL"></td>
                            </tr>
                            <tr>
                                <td class="labelR">Batch No :</td>
                                <td class="labelL">
                                    <asp:Label ID="lblBatch" runat="server" Text="Label"></asp:Label>
                                </td>
                                <td class="labelR"></td>
                                <td class="labelL"></td>
                            </tr>
                            <tr>
                                <td class="labelR">Production Status :</td>
                                <td class="labelL">
                                    <asp:Label ID="lblProdStatus" runat="server" Text="Label"></asp:Label>
                                </td>
                                <td class="labelR"></td>
                                <td class="labelL"></td>
                            </tr>
                            <%--<tr>
                                <td class="labelR">Completion Date :</td>
                                <td class="labelL">
                                    <asp:Label ID="lblTranDate" runat="server" Text="Label"></asp:Label>
                                </td>
                                <td class="labelR"></td>
                                <td class="labelL"></td>
                            </tr>--%>
                            <tr><td style="height:10px;" colspan="4"></td></tr>
                            <tr>
                                <td class="labelR">Weight :</td>
                                <td class="labelL">
                                    <asp:TextBox ID="txtWeight" CssClass="labelR" Height="30px" Width="165px" runat="server" AutoCompleteType="None"></asp:TextBox>&nbsp;
                                    <b>Kgs</b></td>
                                <td class="labelR"></td>
                                <td class="labelL"></td>
                            </tr>
                            
                            <tr><td style="height:20px;" colspan="4"></td></tr>
                            
                        </table>    
                         
                    </div>

                </td>
            </tr>
            <tr>
                <td></td>    
                <td class="labelL" style="padding-left:6px;">
                    <asp:Button id="cmdSave" runat="server" Text="Save" Height="30px" ></asp:Button>   
                    <asp:Button id="cmdClear" runat="server" CssClass="button" Text="Clear" Height="30px" Visible="false"></asp:Button>  
                    <asp:button id="cmdCancel" runat="server" CssClass="button" Text="Cancel" Height="30px" OnClientClick="self.close()"></asp:button> 
                </td>
            </tr>
        </table>
        

    </div>
    </form>
</body>
</html>
