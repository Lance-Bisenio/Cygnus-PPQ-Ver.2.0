<%@ Page Language="VB" AutoEventWireup="false" CodeFile="WeeklyPerformance-Process.aspx.vb" Inherits="WeeklyPerformanceReportProcess" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../css/calendar/jquery-ui-1.10.4.custom.min.css" rel="stylesheet" /> 
    <link href="../bootstrap/bootstrap/css/bootstrap.css" rel="stylesheet" />
	<link href="../css/gridview.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="container-fluid">

            <%=vData %>
              
        </div>
    </form>
</body>
</html>
