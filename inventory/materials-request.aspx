<%@ Page Language="VB" AutoEventWireup="false" CodeFile="materials-request.aspx.vb" Inherits="inventory_materials_request" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="../js/jquery-ui-1.10.4.custom.js"></script>
    <script src="../js/jquery-ui-1.10.4.custom.min.js"></script>
    <script src="../js/jquery-1.10.2.js"></script>
    <script src="../js/jquery-ui.js"></script>
    
   <%-- <link href="../css/BasicContol.css" rel="stylesheet" type="text/css" />
    <link href="../css/calendar/jquery-ui-1.10.4.custom.min.css" rel="stylesheet" />
    <link href="../css/calendar/jquery-ui-1.10.4.custom.css" rel="stylesheet" />--%>
	<link href="../bootstrap/bootstrap/css/bootstrap.min.css" rel="stylesheet" />
     <script>
        function invoke() {
            <%=vScript %>
        } 
    </script>
	<style>
		body {
			font-family: Calibri, Arial, 'Trebuchet MS', sans-serif;
			font-size:12px;
		}
	</style>
</head>
<body onload="invoke();" >
    <form id="form1" runat="server">
	<input type="hidden" id="h_Action" runat="server"/>

	<div class="container-fluid" style="margin:10px;"> 
		<div class="row">
			<div class="col-sm-12">
				<h3>Production Raw Material Request</h3>
			</div>
		</div>
		<div class="row">
			<div class="col-sm-12">
				<div class="btn-group btn-group-sm"> 
					<asp:button id="cmdSave" runat="server" CssClass="btn btn-primary" Text="Save" Visible="false"></asp:button>  
                    <asp:button id="cmdCancel" runat="server" CssClass="btn btn-primary" Text="Cancel" OnClientClick="self.close()" Visible="false"></asp:button> 
				</div>
			</div> 
		</div>
		<br />
		<div class="row">
			<div class="col-sm-12">
				<table class="table table-bordered" > 
					<%=vHeader %>            
					<%=vData %>
				</table>
			</div>
		</div>
	</div>
		 
    </form>
</body>
</html>
