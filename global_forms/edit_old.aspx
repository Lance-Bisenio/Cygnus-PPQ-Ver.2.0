<%@ Page Language="vb" AutoEventWireup="false" Inherits="denaro.edit" CodeFile="edit_old.aspx.vb" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
	<head>
		<title>Add/Modify Codeset Record</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR" />
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>

		<script language="javascript" type="text/javascript">
		    function invoke() {
		        <%=vScript %>
		    }
		</script>

        <link href="../css/BasicContol.css" rel="stylesheet" />

	</head><%-- style="background-color:#e7e7e7"--%>
	<body onload="invoke();" >
		<form id="Form1" method="post" runat="server">
		<center>

            <div style="width: 100%;" ><%--class="SmallBox_Frame"--%>
			    <table id="Standard_Tbl" width="100%" border="0" align="center">
                    <tr>
                        <td style="text-align:left; border-bottom: solid 1px #808080;" colspan="2">
                            <asp:Label ID="txtTitle" CssClass="vModuleTitle" runat="server" Text="..."></asp:Label></td>
                    </tr>
			        <tr>
			            <td style="width:40%;">

                                <br>
                                <% =vDetail %>
                                <br>

                                <asp:Button id="cmdSave" runat="server" Text="Save Changes" CssClass="button"></asp:Button>
                                <%--<input id="cmdClose"  class="button" type="button" value="Cancel" onclick="window.close();" />--%>
    
			            </td>
                        <td></td>
                    </tr>
                    <tr>
                        <td style="margin-left:100px;">
                            
                        </td>
                    </tr>
                </table>
                <%--<table id="Table1" width="100%" border="1" align="center">
                    <tr>
                        <td style="text-align:left; border-bottom: solid 1px #808080;" >
                            <asp:Label ID="Label1" CssClass="vModuleTitle" runat="server" Text="..."></asp:Label></td>
                    </tr>
			        <tr>
			            <td>

                                <br>
                                <% =vDetail %>
                                <br>

                                <asp:Button id="Button1" runat="server" Text="Save Changes" CssClass="button"></asp:Button>
                                <input id="Button2"  class="button" type="button" value="Cancel" onclick="window.close();" />
    
			            </td>
                    </tr>
                </table>--%>
           </div>

		</center>
		</form>
	</body>
</html>
