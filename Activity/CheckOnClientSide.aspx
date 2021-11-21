<%@ Page Language="VB" AutoEventWireup="false" CodeFile="CheckOnClientSide.aspx.vb" Inherits="CheckOnClientSide" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
    <script type="text/javascript">
        function ChangeCheckBoxState(id, checkState)
        {
            var cb = document.getElementById(id);
            if (cb != null)
               cb.checked = checkState;
        }
        
        function ChangeAllCheckBoxStates(checkState)
        {
            // Toggles through all of the checkboxes defined in the CheckBoxIDs array
            // and updates their value to the checkState input parameter
            if (CheckBoxIDs != null)
            {
                for (var i = 0; i < CheckBoxIDs.length; i++)
                   ChangeCheckBoxState(CheckBoxIDs[i], checkState);
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <h1>
            Check/Uncheck All CheckBoxes in a GridView Using Client-Side Script</h1>
        <p>
            The following demo illustrates how to, at the click of a Button or CheckBox, have
            all checkboxes checked (or unchecked). Clicking the Button (or CheckBox) causes
            client-side script to run, updating the check state of the checkboxes as needed.
            No postback occurs, thereby making the page seem snappier!</p>
        <p>
            <asp:Label ID="Summary" runat="server" EnableViewState="False" Font-Bold="False"
                Font-Italic="False" Font-Size="XX-Large" ForeColor="Red"></asp:Label>&nbsp;</p>
        <p>
            <input type="button" value="Check All" onclick="ChangeAllCheckBoxStates(true);" />&nbsp;
            <input type="button" value="Uncheck All" onclick="ChangeAllCheckBoxStates(false);" />
        </p>
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
