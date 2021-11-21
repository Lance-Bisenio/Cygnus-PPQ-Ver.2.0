<%@ Page Language="VB" AutoEventWireup="false" CodeFile="jomachine.aspx.vb" Inherits="inventory_jomachine" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../bootstrap/css/bootstrap.css" rel="stylesheet" />
    
    
    <script src="../js/jquery-ui-1.10.4.custom.min.js"></script>
    <script src="../js/jquery-1.10.2.js"></script>
    <script src="../js/jquery-ui.js"></script>
     
    <link href="../css/calendar/jquery-ui-1.10.4.custom.min.css" rel="stylesheet" />
    <link href="../css/calendar/jquery-ui-1.10.4.custom.css" rel="stylesheet" />


    <script type="text/javascript"> 
        $(document).ready(function () {
            $(document).ready(function () {
                $("#vDate").datepicker(); 
            }); 
        });

        function invoke() {
            <%=vScript %>
        }

        function Settings(pID, pName, pClass) {

            //alert(pID + " - " + pName + " - " + pClass);

            // CHECK BOX SETTINGS ------------------------------------------------------------------------------
            // CHECK ALL CHECKBOX AS DEFAULT SETTINGS
            $('.' + pClass  + '_Chk').each(function () {
                this.checked = true; 
            });
  
            // UN-CHECK THE SELECTED ROW BOX
            $('#' + pID + '_Chk').each(function () {
                this.checked = false;
            });
             
            // ENABLE ALL CHECKBOX AS DEFAULT SETTINGS
            $('.' + pClass + '_Chk').attr("disabled", false);

            // DISABLE THE SELECTED CHECKBOX 
            $("#" + pID + "_Chk").attr("disabled", true);
             
            // RADIO BOX SETTINGS ------------------------------------------------------------------------------
            // CHECK ALL RADIOBOX AS DEFAULT SETTINGS
            $('.' + pClass + '_Rdo').each(function () {
                this.checked = false;
            });

            // ENABLE THE SELECTED ROW BOX
            $('#' + pID + '_Rdo').each(function () {
                this.checked = true;
            });
            // -------------------------------------------------------------------------------------------------    
        }
    </script>

    <style type="text/css">
        .auto-style1 {
            height: 8px;
        }
    </style>

</head>
<body  onload="invoke();"> 
		<form id="Form1" method="post" runat="server">
            <%=vHeader %>
            <div>
                <table id="Table1" style="width:100%;" border="0">
                    <tr>
                        <td style="text-align:left; border-bottom: solid 1px #808080;" colspan="4">
                            <h3><asp:Label ID="Label1" CssClass="vModuleTitle" runat="server" Text="Process machine settings" ></asp:Label></h3></td>
                        
                    </tr>
                    <tr><td class="auto-style1"></td></tr> 
                </table> 

                <div style=" margin:auto; width:98%; height:520px; border-collapse:collapse; border: solid 1px #ccc; overflow:auto">
                    <table style="width:100%; border-collapse:collapse; border-color:#eeeded; border:solid 1px #eeeded;" border="1" >
                        <tr style="font-weight:bold;"> 
                            <td style="width:50px; text-align:center">Oper<br>Num</td> 
                            <td style=" text-align:center">SFG Code / Description</td>
                        
                            <td style="width:250px; text-align:center">Machine Code / Description</td>
                            <td style="width:55px; text-align:center">Primary</td>
                            <td style="width:73px; text-align:center">Alternative</td>
                            <td style="width:110px; text-align:center">Data Start</td>
                            
                            <td style="width:140px; text-align:center">UOM</td>
                        </tr>
                        <%= vData%> 
                    </table>
				</div>
                <br />
                <div class="col-lg-3">
                    <asp:button id="cmdSave" runat="server" Text="Save" CssClass="btn btn-primary btn-sm"></asp:button>
				    <asp:button id="cmdCancel" runat="server" CssClass="btn btn-primary btn-sm" Text="Cancel" OnClientClick="self.close()"></asp:button>
           
                    <asp:TextBox ID="txtMode" runat="server" ReadOnly="True" Visible="False" Width="30px"></asp:TextBox>
                    <asp:TextBox ID="txtDel_record" runat="server" ReadOnly="True" Visible="False" Width="30px"></asp:TextBox>
                </div>
		      

			</div>
		</form>
	</body>
</html>
