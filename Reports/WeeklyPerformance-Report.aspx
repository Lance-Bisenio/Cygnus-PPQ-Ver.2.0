<%@ Page Language="VB" AutoEventWireup="false" CodeFile="WeeklyPerformance-Report.aspx.vb" Inherits="Reports_WeeklyPerformanceReport" %>

<!DOCTYPE html>


<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    
    <title></title> 
	
    <link href="../css/calendar/jquery-ui-1.10.4.custom.min.css" rel="stylesheet" /> 
	<link href="../bootstrap/bootstrap/css/bootstrap.css" rel="stylesheet" />
	
    <style type="text/css"> 

        .iDataFrame {
            width:99%; border: solid 0px #e2e2e2; height: 96%; margin:0px;
        }
    	div {
			border:0px solid #000; padding-top:1px;
    	}
    	body {
			font-family:Arial;
			font-size:12px;
    	}
    	.ZeroPadleft {
    		padding-left:0px; margin-left:0px;
		}
    	.Tbl {
    		border: solid 1px #CCC;
		}
    </style> 

</head>
<body onload="invoke();">
    <form id="form1" runat="server" autocomplete="off"> 

    <div class="container-fluid">
		<h3>Weekly Performance Report</h3>
		  
        <div class="row Mgin">
			<div class="col-md-2">Section :</div>
			<div class="col-md-3 ZeroPadleft">
				<div class="col-md-10 ZeroPadleft">
					<asp:DropDownList ID="CmdSection" runat="server" CssClass="form-control"></asp:DropDownList>
				</div> 
			</div> 
		</div>

		<div class="row Mgin">
			<div class="col-md-2">Curing Date From :</div>
			<div class="col-md-3 ZeroPadleft">
				<div class="col-md-10 ZeroPadleft">
					<asp:TextBox ID="txtDateFrom" runat="server" CssClass="form-control" placeholder="mm / dd / yyyy" ></asp:TextBox>  
				</div> 
				<img src="../images/calendar.png" class="DatePicker" style="vertical-align:middle;" alt="" />
			</div> 
		</div>

		<div class="row Mgin">
			<div class="col-md-2">Curing Date To :</div>
			<div class="col-md-3 ZeroPadleft">
				<div class="col-md-10 ZeroPadleft">
					<asp:TextBox ID="txtDateTo" runat="server" CssClass="form-control" placeholder="mm / dd / yyyy" ></asp:TextBox>  
				</div>  
				<img src="../images/calendar.png" class="DatePicker" style="vertical-align:middle;" alt="" />
			</div> 
		</div> 
	 
		  
		<div class="row Mgin">
			<div class="col-md-2 ZeroPadleft"></div>
			<div class="col-md-3 ZeroPadleft">
				<div class="btn-group"> 
					<input type="button" runat="server" id="btnScan" value="Create Transaction" class="btn btn-primary btn-sm" data-toggle="modal" data-target="#myModal" />  
					<asp:Button ID="btnSearch" CssClass="btn btn-primary btn-sm" runat="server"  Text="Search" />
				</div>
			</div>
		</div>

		<br /><br />
		<div class="row">
			
			<div class="col-md-12"> 
				<table class="table table-bordered" style="border:solid 1px #000;" border="1">
					<%--<tr>
						<th colspan="5"></th> 
						<th colspan="2">Curing IN</th>
						<th colspan="2">Curing OUT</th> 
						<th colspan="2"></th>
					</tr>--%>

                    <%=vDataHeader %>
                    <%=vDataDetails %>
					 
				</table><br />
			</div>			
			
		</div>
		  
		</div>
		  
    </form>
</body>
</html>
 
<script src="../js/jquery-1.10.2.js"></script>
<script src="../js/jquery-ui.js"></script> 
<script src="../bootstrap/bootstrap/js/bootstrap.js"></script>

<script>
	function invoke() {
		<%=vScript %>
	}
	
	$(document).ready(function () {
		$(document).ready(function () {
			$("#txtDateFrom").datepicker(); 
			$("#txtDateTo").datepicker();
		});  

		$('#btnScan').click(function (event) { 
			$("#txtSFGCode").focus();
		});
		 
	});
     

</script>