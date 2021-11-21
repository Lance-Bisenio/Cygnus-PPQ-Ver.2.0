<%@ Page Language="VB" AutoEventWireup="false" CodeFile="upm.aspx.vb" Inherits="upm" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
    <title>User Profile Management Page</title>
    <link href="css/BasicContol.css" rel="stylesheet" type="text/css" />
    <script language="javascript" type="text/javascript">
        function invoke() {
            <%=vScript %>
        }
           
        function ask() 
        {
            if(confirm('Are you sure you want to delete the selected user?')) {
                return true;
            } else {
                return false;
            }
        }

    </script>
    
</head>
<body onload="invoke();">
    <form id="form1" runat="server">
        <center>
         
        
        
        <table style="width: 100%; margin:auto;" border="0" cellpadding="0" cellspacing="0" >
            <tr><td>&nbsp;</td></tr>
            <tr>
                <td style="width:100px;"  class="labelR">Filter By : </td>
                <td class="labelL">
                    <asp:DropDownList ID="cmbPosition" runat="server" CssClass="labelL" Width="285px">
                        <asp:ListItem Selected="True" Value="where FullName like">Fullname</asp:ListItem>
                        <asp:ListItem Value="where User_Id like">User Id</asp:ListItem>
                    </asp:DropDownList>
                </td></tr>
            <tr>
                <td class="labelR"> Quick Search : </td>
                <td class="labelL">
                    <asp:TextBox ID="txtSearch" runat="server" CssClass="labelL" Width="150px"></asp:TextBox> &nbsp;by : 
                    <asp:DropDownList ID="cmbFilter" runat="server" CssClass="labelL" Width="100px">
                        <asp:ListItem Selected="True" Value="where FullName like">Fullname</asp:ListItem>
                        <asp:ListItem Value="where User_Id like">User Id</asp:ListItem>
                    </asp:DropDownList>
                    <asp:Button ID="cmdSearch" runat="server" CssClass="button" Text="Search" />
                </td>
                <td></td>
            </tr>
            <tr><td>&nbsp;</td></tr>
            <tr><td>&nbsp;</td></tr>
            <tr><td>&nbsp;</td></tr>
             
            <tr>
                <td colspan="2" valign="top">
                    <table style="width: 100%;" border="0" cellpadding="0" cellspacing="0" align="center" >
                        <tr>
                            <td class="labelL" style="width:50%;" >
                                <asp:Button ID="cmdAdd" runat="server" CssClass="button" Text="Add user" />
                                <asp:Button ID="cmdEdit" runat="server" CssClass="button" Text="Edit user" />
                                <asp:Button ID="cmdDelete" runat="server" CssClass="button" Text="Delete user" />
                                <asp:Button ID="cmdPwd" runat="server" CssClass="button" Text="Assign Password" Visible="false" />
                                <asp:Button ID="cmdReturn" runat="server" CssClass="button" Text="Close this Screen" />
                                <asp:Button ID="cmdPrint" runat="server" CssClass="button" Text="Print" Visible="False" />
                            </td>
                            <td align="left" style="width:50%;" >
                                <asp:Button ID="cmdSelect" runat="server" CssClass="button" Text="Select All" />
                                <asp:Button ID="cmdDeselect" runat="server" CssClass="button" Text="Deselect All" />
                                <asp:Button ID="cmdSave" runat="server" CssClass="button" Text="Set Rights" />
                                <%--<asp:Button ID="cmdSave0" runat="server" CssClass="button" 
                                    Text="Set Rights v1" />--%>
                            </td>
                        </tr>
                        <tr> 
                            <td rowspan="2" valign="top">
                                <table id="Table1" class="titleBarTop"  border="0" style="width:99%;  border-collapse:collapse;">
                                    <tr style="border:solid 0px #cccccc; border-bottom:0px;">
                                        <td style="width:320px; text-align:left;">
                                            <%--&nbsp;&nbsp;
                                                <input type="button" runat="server" id="btnHeaderAdd" name="btnProAdd" value="Add"  class="tblButtonA" /> 
                                                <input type="button" runat="server" id="btnHeaderEdit" name="btnProEdit" value="Edit"  class="tblButtonE" /> 
                                                <input type="button" runat="server" id="btnHeaderDel" name="btnProDelete" value="Delete" onclick="DeleteItem();" class="tblButtonD" />--%>
                                
                                          <%--  Show :<asp:DropDownList ID="cmbShow" runat="server" Width="70px" AutoPostBack="True" CssClass="labelL">
                                            </asp:DropDownList> --%>

                                        </td>
                                        <td style="text-align:left; padding-left :5px;"></td>
                                        <td class="labelR" >
                                            <%--<b><asp:Label ID="lblTotalDocs" runat="server" CssClass="labelL" Text="Documents Retrieved : 0" ForeColor="#ffffff"></asp:Label></b>&nbsp;&nbsp;&nbsp;--%>
                                        </td>
                                    </tr>
                                </table>
                                <asp:GridView ID="tblUser" runat="server" AllowPaging="True"  BorderColor="#cccccc"
                                    AutoGenerateColumns="False" Width="99%" BorderStyle="Solid" BorderWidth="1px" 
                                    CssClass="mainGridView" PageSize="18" >
                                    <Columns>
                                        <asp:BoundField DataField="User_Id" HeaderText="User Id" >
                                            <ItemStyle CssClass="labelBC" />
                                        </asp:BoundField>
                            
                                        <asp:BoundField DataField="FullName" HeaderText="Fullname" >
                                        <ItemStyle CssClass="labelBL" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Position" HeaderText="Position" >
                                        <ItemStyle CssClass="labelBL" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="User Level">
                                            <ItemTemplate>
                                                <asp:Label ID="Label1" runat="server" Text='<%# IIf(Eval("UserLevel")=0,"Regular","Super") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="labelBL" />
                                        </asp:TemplateField>
                                        <%--<asp:BoundField DataField="Alt_Userid" HeaderText="Alt Userid" >
                                        <ItemStyle CssClass="labelBL" />
                                        </asp:BoundField>--%>
                                        <asp:CommandField ButtonType="Button" ShowSelectButton="True" >
                                            <ControlStyle CssClass="button" /> 
                                            <ItemStyle CssClass="labelC" Width="40px" />
                                        </asp:CommandField>
                                    </Columns>

                                    <SelectedRowStyle CssClass="activeBar" />
                                    <PagerStyle  HorizontalAlign="Left" />
                                    <RowStyle CssClass="odd" />
                                    <HeaderStyle CssClass="titleBar" />
                                    <AlternatingRowStyle CssClass="even" />
                                </asp:GridView>
                            </td>
                        </tr>
                        <tr> 
                            <td valign="top">
                                <table class="titleBarTop" id="Table2" border="0" style="width:100%;  border-collapse:collapse;">
                                    <tr style="border:solid 0px #cccccc; border-bottom:0px;">
                                        <td class="labelL">&nbsp;&nbsp;
                                            <asp:LinkButton ID="cmdMenus" runat="server" CssClass="MenuLink">Menus</asp:LinkButton>
                                            &nbsp;&nbsp;|&nbsp;&nbsp;&nbsp;<asp:LinkButton ID="cmdSection" runat="server" CssClass="MenuLink">Section</asp:LinkButton>
                                            &nbsp;&nbsp;<asp:LinkButton ID="cmdAccount" runat="server" CssClass="MenuLink" Visible="False">Accounts</asp:LinkButton>
                                            &nbsp;&nbsp;<%--|--%>&nbsp;&nbsp;&nbsp;<asp:LinkButton ID="cmdRC" runat="server" CssClass="MenuLink" Visible="false">Cost Center</asp:LinkButton>
                                            &nbsp;&nbsp;<%--|--%>&nbsp;&nbsp;&nbsp;<asp:LinkButton ID="cmdSecuity" runat="server" CssClass="MenuLink" Visible="False">Rank</asp:LinkButton>
                                            
                                        </td>
                                    </tr>
                                </table>
                                <table style="width: 100%; height: 100%" border="0" cellpadding="0" cellspacing="0"> 
                                    <tr>
                                        <td valign="top">
                                            <div style=" height: 448px; border: solid 1px #cccccc; overflow: auto;">
                                                <table style="width:100%">
                                                    <tr>
                                                        <td class="labelL">
                                                            <asp:TreeView ID="triMenu" runat="server"></asp:TreeView>
                                                            <input type="hidden" name="txtRList" id="txtRList" runat="server" />
                                                            <input type="hidden" name="txtSeletedTab" id="txtSeletedTab" runat="server" />
                                                            <asp:CheckBoxList ID="chkList" runat="server" CssClass="label"></asp:CheckBoxList>
                                                        </td>
                                                    </tr>
                                    
                                                </table>
                                            </div>
                                
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>

                </td>
            </tr>
            
            
                
        </table>
        
        </center>
    </form>
</body>
</html>
