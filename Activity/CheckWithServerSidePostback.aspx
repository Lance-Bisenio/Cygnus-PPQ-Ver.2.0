<%@ Page Language="VB" AutoEventWireup="false" CodeFile="CheckWithServerSidePostback.aspx.vb" Inherits="CheckWithServerSidePostback" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <h1>
            Check/Uncheck All CheckBoxes in a GridView Using Server-Side Code</h1>
        <p>
            The following demo illustrates how to, at the click of a Button, have all checkboxes
            checked (or unchecked). Clicking the Button causes a postback, and server-side code
            iterates through the GridViewRows, either checking or unchecking each CheckBox.</p>
        <p>
            <asp:Label ID="Summary" runat="server" EnableViewState="False" Font-Bold="False"
                Font-Italic="False" Font-Size="XX-Large" ForeColor="Red"></asp:Label>&nbsp;</p>
        <p>
            <asp:Button ID="CheckAll" runat="server" Text="Check All" />&nbsp;
            <asp:Button ID="UncheckAll" runat="server" Text="Uncheck All" /></p>
        <p>
            <asp:GridView ID="FileList" runat="server" CellPadding="4"
                ForeColor="#333333" GridLines="None" AutoGenerateColumns="False" DataKeyNames="FullName">
                <FooterStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                <RowStyle BackColor="#FFFBD6" ForeColor="#333333" />
                <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />
                <PagerStyle BackColor="#FFCC66" ForeColor="#333333" HorizontalAlign="Center" />
                <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                <AlternatingRowStyle BackColor="White" />
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:CheckBox runat="server" ID="RowLevelCheckBox" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Name" HeaderText="Name" />
                    <asp:BoundField DataField="CreationTime" HeaderText="Created On">
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Length" DataFormatString="{0:N0}" HeaderText="File Size"
                        HtmlEncode="False">
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:BoundField>
                </Columns>
            </asp:GridView>
         </p>
         <p>
            <asp:Button runat="server" ID="DeleteButton" Text="Delete Checked Files" />
            (<i>go ahead and click this... it won't really delete the files</i>)
         </p>
    </div>

    </form>
</body>
</html>
