<%@ Page Language="VB" AutoEventWireup="false" CodeFile="item_catalog.aspx.vb" Inherits="build_item_item_catalog" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../bootstrap/css/bootstrap.css" rel="stylesheet" />
     <style>
    	 
    	div .col-md-5, div .col-md-3 {
    		padding:1px;
		}
    	body {
			font-family:Arial; font-size:12px;
    	}
    </style>

    <script src="../js/jquery-1.10.2.js" type="text/javascript"></script>
    <script src="../media/js/jquery.dataTables.js" type="text/javascript"> </script>
    
</head>
<body onload="invoke();">  <%--style="background-color:#e7e7e7"--%>
    <form id="form1" runat="server">
     <div class="container">
		<h4>Item Catalog</h4>
		<hr /> 

		<div class="row"> 
			<div class="col-md-2 text-right">Item Code :</div>
			<div class="col-md-5"><asp:TextBox ID="txtItemCd" runat="server" Width="" CssClass="form-control" ></asp:TextBox></div> 
		</div>
		<div class="row"> 
			<div class="col-md-2 text-right">Description 1 :</div>
			<div class="col-md-5"><asp:TextBox ID="txtDescr1" runat="server" Width="" CssClass="form-control" ></asp:TextBox></div> 
		</div>
		<div class="row"> 
			<div class="col-md-2 text-right">Description 2 :</div>
			<div class="col-md-5"><asp:TextBox ID="txtDescr2" runat="server" Width="" CssClass="form-control" ></asp:TextBox></div> 
		</div>
		<div class="row"> 
			<div class="col-md-2 text-right">Item Images :</div>
			<div class="col-md-5">
				<input type="button" id="cmdUpload" value="Upload images" class="btn btn-primary btn-xs" /><br /><br />
                    <%=vItemImages %>
			</div> 
		</div>


		<h4>Customer</h4>
		<hr /> 
		<div class="row"> 
			<div class="col-md-2 text-right">Customer 1 :</div>
			<div class="col-md-5">
				<asp:DropDownList ID="cmbCust1" runat="server" Width="485px" CssClass="form-control" AutoPostBack="True"></asp:DropDownList> 
                    <%--<img src="../images/settings.png" style="vertical-align:middle; cursor:pointer;" id="2045" alt="" onclick="showSettings(id);"/>--%>
			</div>
			<div class="col-md-2 text-right">GCAS / Product Code :</div>
			<div class="col-md-3">
				<div class="col-md-8">
					<asp:TextBox ID="txtCust1" runat="server" Width="" CssClass="form-control" ></asp:TextBox>
				</div>
			</div>
		</div>
		<div class="row"> 
			<div class="col-md-2 text-right">Customer 2 :</div>
			<div class="col-md-5">
				<asp:DropDownList ID="cmbCust2" runat="server" Width="485px" CssClass="form-control" AutoPostBack="True"></asp:DropDownList> 
			</div> 
			<div class="col-md-2 text-right">GCAS / Product Code :</div>
			<div class="col-md-3">
				<div class="col-md-8">
					<asp:TextBox ID="txtCust2" runat="server" Width="" CssClass="form-control" ></asp:TextBox>
				</div>
			</div>
		</div>
<asp:Label ID="txtTitle" CssClass="vModuleTitle" runat="server" Text="Item Catalog"></asp:Label>
<asp:Label ID="Label1" CssClass="vModuleTitle" runat="server" Text="Customer"></asp:Label>

        <table id="Standard_Tbl" style="width:100%;" border="0"> 
            <tr>
                <td style="text-align:left; border-bottom: solid 1px #808080; padding-top:5px" colspan="4">
                    as</td>
            </tr>
            <tr><td>&nbsp;</td></tr>
            
            <tr><td style="vertical-align:top;">Item Images :</td><td>
                    </td></tr>
            <tr><td colspan="4"></td></tr>
            <tr>
                <td style="text-align:left; border-bottom: solid 1px #808080; padding-top:5px" colspan="4">
                    </td>
            </tr>
            <tr><td colspan="4">&nbsp;</td></tr>
            <tr>
                <td>Customer 1 :</td>
                <td >
                    </td>
                 <td>GCAS / Product Code :</td>
                <td>
                     
                </td>
            </tr>
            <tr id="tr_Cus2" runat="server" visible="false">
                <td>Customer 2 :</td>
                <td >
                    
                    </td>
                 <td>GCAS / Product Code :</td>
                <td>
                     
                </td>
            </tr>
            <tr id="tr_Cus3" runat="server" visible="false">
                <td>Customer 3 :</td>
                <td >
                    <asp:DropDownList ID="cmbCust3" runat="server" Width="485px" CssClass="labelL" AutoPostBack="True"></asp:DropDownList> 
                    </td>
                 <td>GCAS / Product Code :</td>
                <td>
                    <asp:TextBox ID="txtCust3" runat="server" Width="100px" CssClass="labelL" ForeColor="Red" ></asp:TextBox>
                </td>
            </tr>
            <tr id="tr_Cus4" runat="server" visible="false">
                <td>Customer 4 :</td>
                <td >
                    <asp:DropDownList ID="cmbCust4" runat="server" Width="485px" CssClass="labelL" AutoPostBack="True"></asp:DropDownList> 
                    </td>
                 <td>GCAS / Product Code :</td>
                <td>
                    <asp:TextBox ID="txtCust4" runat="server" Width="100px" CssClass="labelL" ForeColor="Red" ></asp:TextBox>
                </td>
            </tr>
            <tr id="tr_Cus5" runat="server" visible="false">
                <td>Customer 5 :</td>
                <td >
                    <asp:DropDownList ID="cmbCust5" runat="server" Width="485px" CssClass="labelL" AutoPostBack="True"></asp:DropDownList> 
                    </td>
                 <td>GCAS / Product Code :</td>
                <td>
                    <asp:TextBox ID="txtCust5" runat="server" Width="100px" CssClass="labelL" ForeColor="Red" ></asp:TextBox>
                </td>
            </tr>
            <tr><td>&nbsp;</td><td></td></tr>
            <tr>
                <td style="text-align:left; border-bottom: solid 1px #808080; padding-top:5px" colspan="4">
                    <asp:Label ID="Label2" CssClass="vModuleTitle" runat="server" Text="Supplier"></asp:Label></td>
            </tr>
            <tr><td colspan="4">&nbsp;</td></tr>
            <tr>
                <td>Supplier 1 :</td>
                <td>
                    <asp:DropDownList ID="cmbSupp1" runat="server" Width="485px" CssClass="labelL" AutoPostBack="True"></asp:DropDownList> 
                    <img src="../images/settings.png" style="vertical-align:middle; cursor:pointer;" id="2050" alt="" onclick="showSettings(id);"/></td>
                 <td>GCAS / Product Code :</td>
                <td>
                    <asp:TextBox ID="txtSupp1" runat="server" Width="100px" CssClass="labelL" ForeColor="Red" ></asp:TextBox>
                </td>
            </tr>
            <tr id="tr_Supp2" runat="server" visible="false">
                <td>Supplier 2 :</td>
                <td>
                    <asp:DropDownList ID="cmbSupp2" runat="server" Width="485px" CssClass="labelL" AutoPostBack="True"></asp:DropDownList> 
                    </td>
                 <td>GCAS / Product Code :</td>
                <td>
                    <asp:TextBox ID="txtSupp2" runat="server" Width="100px" CssClass="labelL" ForeColor="Red" ></asp:TextBox>
                </td>
            </tr>
            <tr id="tr_Supp3" runat="server" visible="false">
                <td>Supplier 3 :</td>
                <td>
                    <asp:DropDownList ID="cmbSupp3" runat="server" Width="485px" CssClass="labelL" AutoPostBack="True"></asp:DropDownList> 
                    </td>
                 <td>GCAS / Product Code :</td>
                <td>
                    <asp:TextBox ID="txtSupp3" runat="server" Width="100px" CssClass="labelL" ForeColor="Red" ></asp:TextBox>
                </td>
            </tr>
            <tr id="tr_Supp4" runat="server" visible="false">
                <td>Supplier 4 :</td>
                <td>
                    <asp:DropDownList ID="cmbSupp4" runat="server" Width="485px" CssClass="labelL" AutoPostBack="True"></asp:DropDownList> 
                    </td>
                 <td>GCAS / Product Code :</td>
                <td>
                    <asp:TextBox ID="txtSupp4" runat="server" Width="100px" CssClass="labelL" ForeColor="Red" ></asp:TextBox>
                </td>
            </tr>
            <tr id="tr_Supp5" runat="server" visible="false">
                <td>Supplier 5 :</td>
                <td>
                    <asp:DropDownList ID="cmbSupp5" runat="server" Width="485px" CssClass="labelL" AutoPostBack="True"></asp:DropDownList> 
                    </td>
                 <td>GCAS / Product Code :</td>
                <td>
                    <asp:TextBox ID="txtSupp5" runat="server" Width="100px" CssClass="labelL" ForeColor="Red" ></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td></td>
                <td>
                    <asp:Button ID="cmdSave" CssClass="Button" runat="server"  Text="Save" />
                    <asp:Button ID="cmdClose" CssClass="Button" runat="server"  Text="Close" />  
                    <input type="hidden"  id="h_Mode" class="h_Element" name="h_Mode" runat="server"  style="width:45px"/>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>


<script type="text/javascript">
    function invoke() {
        <%=vScript %>
    }

    function showSettings(vType) {
        id = vType;
        s = 0;
        linkwin = window.open('../global_forms/maintenance.aspx?id=' + id + "&s=" + s,
            'viewprop', 'top=80,left=100,scrollbars=yes,resizable=yes,toolbars=no,width=850,height=550');
        linkwin.focus();
    }

    $(document).ready(function () {
        $('#cmdUpload').click(function (event) { 
            event.preventDefault();
            winUploadPop = window.open("../global_forms/subattachment.aspx?pCode=" + $('#txtItemCd').val() + "",
                "winUploadPop", "width=600px, height=300px, top=50px, left=80px, scrollbars=yes");
            winUploadPop.focus();
        });
    });

    function OpenSubAtt(vMode) {
        window.open('../downloads/itemimages/' + vMode);
    }
</script>