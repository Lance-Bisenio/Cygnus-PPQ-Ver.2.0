<%@ Page Language="VB" AutoEventWireup="false" CodeFile="eventviewer.aspx.vb" Inherits="eventviewer" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
    <title>Event Viewer Page</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR" />
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE" />
    <meta content="JavaScript" name="vs_defaultClientScript" />
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />
    <link href="redtheme/red.css" rel="stylesheet" type="text/css" />

<script language="javascript" type="text/javascript">

        function invoke() {
            <%=vScript %>
        }
</script>
</head>
<body class="accsys" onload="invoke();">
<center>
    <form id="form1" runat="server">
        <center>
        <!--#include file="topborder.inc"-->
            <table width="100%" border="0">
                <tr>
                    <td class="labelR" style="width:150px;">Select Date Range From :</td>
                    <td class="labelL" style="width:200px;">
                        <asp:TextBox ID="txtFrom" runat="server" CssClass="label" Width="95%"></asp:TextBox>
                    </td>
                    <td class="labelR" style="width:100px;">Module :</td>
                    <td class="labelL">
                                        <asp:DropDownList ID="cmbModule" runat="server" Width="200px" 
                            CssClass="labelL">
                                        </asp:DropDownList>
                                    </td>
                </tr>
                <tr>
                    <td class="labelR">To :</td>
                    <td class="labelL">
                        <asp:TextBox ID="txtTo" runat="server" CssClass="label" Width="95%"></asp:TextBox>
                    </td>
                    <td class="labelR">Event :</td>
                    <td class="labelL">
                                        <asp:DropDownList ID="cmbEvent" runat="server" Width="200px" 
                            CssClass="labelL">
                                        </asp:DropDownList>
                                    </td>
                </tr>
                <tr>
                    <td class="labelR">Select User :</td>
                    <td class="labelL">
                        <asp:TextBox ID="txtUser" runat="server" CssClass="label" Width="95%"></asp:TextBox>
                    </td>
                    <td class="labelR">
                    
                    </td>
                    <td class="labelL">
                        <asp:Button ID="cmdRefresh" runat="server" Text="Refresh" CssClass="label" />
                        <asp:Button ID="cmdClose" runat="server" Text="Close this Screen" CssClass="label" />
                    </td>
                </tr>
                <tr><td colspan="4">&nbsp;</td></tr>
                <tr>
                    <td colspan="4">
                        <asp:GridView ID="tblLog" runat="server" AutoGenerateColumns="False" Width="100%" AllowPaging="True" PageSize="12" CssClass="label">
                            <Columns>
                                <asp:BoundField DataField="TranDate" HeaderText="Trans. Date" >
                                <ItemStyle CssClass="labelC" Width="130px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="MachineId" HeaderText="Source" >
                                <ItemStyle Width="100px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="User_Id" HeaderText="User" >
                                <ItemStyle Width="100px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Module" HeaderText="Module" >
                                <ItemStyle Width="150px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Event" HeaderText="Event" >
                                <ItemStyle Width="150px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Remarks" HeaderText="Remarks" />
                                <asp:CommandField ButtonType="Button" SelectText="View" ShowSelectButton="True">
                                    <ControlStyle CssClass="button" Width="50px" />
                                <ItemStyle Width="70px" />
                                </asp:CommandField>
                            </Columns>
                            <SelectedRowStyle ForeColor="White" CssClass="activeBar" />
                                <RowStyle CssClass="odd" />
                                <HeaderStyle CssClass="titleBar" />
                                <AlternatingRowStyle CssClass="even" />
                        </asp:GridView>
                    </td>
                </tr>
                <tr><td colspan="4">&nbsp;</td></tr>
                <tr>
                    <td colspan="4">
                        <div style="height:230px; overflow-y:scroll">
                            <table border="1" style="width:100%; border-collapse:collapse;" class="label">
        	                    <tr>
            	                    <th>Field Name</th>
                                    <th>Old Value</th>
                                    <th>New Value</th>
                                </tr>
                                <%=vDetail %>
                            </table>
                        </div>
                    </td>
                </tr>
            </table>
            
            
        
        
        <!--#include file="bottomborder.inc"-->
        </center>
    </form>
    </center>
</body>
</html>
