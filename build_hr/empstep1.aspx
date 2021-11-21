<%@ Page Language="vb" AutoEventWireup="false" Inherits="denaro.empstep1" CodeFile="empstep1.aspx.vb" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html  xmlns="http://www.w3.org/1999/xhtml">
	<head>
		<title></title>
		<link href="../css/CalendarControl.css" type="text/css"  > 
		<link href="../css/BasicContol.css" rel="stylesheet" type="text/css" /> 
        <script src="../js/CalendarControl.js" type="text/javascript"></script>
		
        <script language="javascript" type="text/javascript">
			function copyval() {
				document.Form1.txtNick.value=document.Form1.txtFirst.value;
			}
			function resize() {
			    window.resizeTo(630,640);
			    window.focus();
			}
			function invoke() {
			    <%=vScript %>
			    resize();
			    
			}
		    function showSettings(vType) {
		        id = vType;
		        s = 0;
		        linkwin = window.open('maintenance.aspx?id=' + id + "&s=" + s,
               'viewprop', 'top=80,left=100,scrollbars=yes,resizable=yes,toolbars=no,width=850,height=550');
		        linkwin.focus();
		    }

		    function showsplit() {
		        rc = document.getElementById("cmbRC").value;

		        winsplit = window.open("splitrc.aspx?rc=" + rc, "winsplit", "top=100,left=100,width=640,height=480,resizable=yes,scrollbars=yes");
		        winsplit.focus();
		    }
		</script>
        
	</head>
	<body style="background-color:#e7e7e7" onload="invoke();">
		<form id="Form1" method="post" runat="server">
            <div class="SmallBox_Frame">
		        <table id="Standard_Tbl" style="width:100%;" border="0">
                    <tr>
                        <td style="text-align:left; border-bottom: solid 1px #808080;" colspan="2">
                            <asp:Label ID="txtTitle" CssClass="vModuleTitle" runat="server" Text="Employee Master Settings"></asp:Label></td>
                    </tr>
                    <tr><td>&nbsp;</td></tr>
                    <asp:dropdownlist id="cmbRC" runat="server" Width="280px" class="labelL" Visible="false"></asp:dropdownlist>
				    <%--<tr>
					    <td style="width: 70px;">Cost Center :</td>
					    <td>
                            <img src="images/settings.png" style="vertical-align:middle; cursor:pointer;" id="1010" alt="" onclick="showSettings(id);"/>
                            <input type="button" id="cmdAdd"  name="cmdAdd"  class="button" value="..." onclick="showsplit()" />
					    </td>
				    </tr>--%>
				    <tr>
					    <td>Company :</td>
					    <td><asp:dropdownlist id="cmbAgency" runat="server" Width="280px"  class="labelL"></asp:dropdownlist>
                            <%--<img src="images/settings.png" style="vertical-align:middle; cursor:pointer;"  id="0" alt="" onclick="showSettings(id);"/>--%>
					    </td>
				    </tr>
				    <tr>
					    <td>Division :</td>
					    <td><asp:dropdownlist id="cmbDiv" runat="server" Width="280px"  class="labelL"></asp:dropdownlist>
                            <img src="images/settings.png" style="vertical-align:middle; cursor:pointer;" id="1020" alt="" onclick="showSettings(id);"/>
					    </td>
				    </tr>
				    <tr>
					    <td>Department :</td>
					    <td><asp:dropdownlist id="cmbDept" runat="server" Width="280px"  class="labelL"></asp:dropdownlist>
                            <img src="images/settings.png" style="vertical-align:middle; cursor:pointer;" id="1030" alt="" onclick="showSettings(id);"/>
					    </td>
					    <td></td>
				    </tr>
				    <tr>
					    <td>Section :</td>
					    <td><asp:dropdownlist id="cmbSection" runat="server" Width="280px"  class="labelL"></asp:dropdownlist>
                            <img src="images/settings.png" style="vertical-align:middle; cursor:pointer;" id="1040" alt="" onclick="showSettings(id);"/>
					    </td>
				    </tr>
				    <tr>
					    <td>Unit :</td>
					    <td><asp:dropdownlist id="cmbUnit" runat="server" Width="280px"  class="labelL"></asp:dropdownlist>
                            <img src="images/settings.png" style="vertical-align:middle; cursor:pointer;" id="1050" alt="" onclick="showSettings(id);"/>
					    </td>
				    </tr>
				    <tr>
					    <td>&nbsp;</td>
					    <td>&nbsp;</td>
				    </tr>
				    <tr>
					    <td>Employee Id :</td>
					    <td><asp:textbox id="txtId" runat="server" Width="120px"></asp:textbox>
					
					        <asp:requiredfieldvalidator id="RequiredFieldValidator1" runat="server" ErrorMessage="Emp. Id required" CssClass="label" ControlToValidate="txtId"></asp:requiredfieldvalidator>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						    <asp:customvalidator id="vldEmpId" runat="server" ErrorMessage="Emp Id already exist" CssClass="label"></asp:customvalidator>
					    </td>
				    </tr>
				    <%--<tr>
					    <td>Biometics Id :</td>
					    <td><asp:textbox id="txtBarcode" runat="server" Width="120px"></asp:textbox>
					        <asp:requiredfieldvalidator id="RequiredFieldValidator2" runat="server" ErrorMessage="Barcode Id required" 
					        ControlToValidate="txtBarcode"></asp:requiredfieldvalidator>
						    <asp:customvalidator id="vldEmpBio" runat="server" 
                                ErrorMessage="Emp Biometrics Id already exist" CssClass="label"></asp:customvalidator>
					    </td>
				    </tr>
				    <tr><td>&nbsp;</td></tr>--%>
				    <tr>
					    <td>First Name :</td>
					    <td><asp:textbox id="txtFirst" runat="server" Width="280px"></asp:textbox>
					        <asp:requiredfieldvalidator id="RequiredFieldValidator3" runat="server" ErrorMessage="First Name required" 
					        ControlToValidate="txtFirst"></asp:requiredfieldvalidator></td>
				    </tr>
				    <tr>
					    <td>Middle Name :</td>
					    <td><asp:textbox id="txtMiddle" runat="server" Width="280px"></asp:textbox>
					        <asp:requiredfieldvalidator id="RequiredFieldValidator4" runat="server" ErrorMessage="Middle name required"
					        ControlToValidate="txtMiddle"></asp:requiredfieldvalidator></td>
				    </tr>
				    <tr>
					    <td>Last Name :</td>
					    <td><asp:textbox id="txtLast" runat="server" Width="280px"></asp:textbox>
					        <asp:requiredfieldvalidator id="RequiredFieldValidator5" runat="server" ErrorMessage="Last name required"
					        ControlToValidate="txtLast"></asp:requiredfieldvalidator></td>
				    </tr>
				    <tr>
				        <td>Date Resign :</td>
				        <td>
				            <asp:textbox id="txtDateResign" runat="server" Width="120px" ></asp:textbox>
                            <img src="images/calendar.png" style="vertical-align:middle;" alt="" onclick="showCalendarControl(txtDateResign);"/>
				            <%--<input type="text" runat="server" name="" id="txtDateResign" class="labelL" style="width: 120px;" />--%>
				            MM / DD / YYYY
				        </td>
				    </tr>
                    <tr><td>Pay Type :</td>
                        <td>
                            <asp:RadioButtonList ID="rdoPayType" runat="server" CssClass="labelL" RepeatDirection="Horizontal">
                                <asp:ListItem Selected="True" Value="M">Monthly</asp:ListItem>
                                <asp:ListItem Value="D">Daily</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
				        <td>Hours Rate :</td>
				        <td> 
				            <asp:textbox id="txtHrRate" runat="server" Width="120px" Text="0.00" CssClass="labelR"></asp:textbox> 
                        </td>
				    </tr>
                    <tr>
				        <td>Daily Rate :</td>
				        <td>
				            <asp:textbox id="txtDayRate" runat="server" Width="120px" Text="0.00" CssClass="labelR" ></asp:textbox> 
                        </td>
				    </tr>
                    <tr>
				        <td>Monthly Rate :</td>
				        <td>
				            <asp:textbox id="txtMoRate" runat="server" Width="120px" Text="0.00" CssClass="labelR" ></asp:textbox> 
                        </td>
				    </tr>
                    <tr>
				        <td>Yearly Rate :</td>
				        <td>
				            <asp:textbox id="txtYrRate" runat="server" Width="120px" Text="0.00" CssClass="labelR"></asp:textbox> 
                        </td>
				    </tr>
                    <tr><td>&nbsp;</td></tr>
                    <tr>
                        <td></td>
                        <td>
                            <asp:button id="cmdSave" runat="server" CssClass="button" Text="Save and Next"></asp:button>
						    <asp:button id="cmdCancel" runat="server" CssClass="button" Text="Cancel" OnClientClick="self.close()"></asp:button></td>
                        <td></td>
                    </tr>
			    </table>
			</div>
		</form>
	</body>
</html>
