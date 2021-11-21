<%@ Page Language="VB" AutoEventWireup="false" CodeFile="main.aspx.vb" Inherits="main" %>



<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Evolve Main Page</title>

<%--    <script type="text/javascript" src="js/jquery-1.10.2.js"></script> 
    <script type="text/javascript" src="js/jquery-1.10.1.min.js"></script>
    <script type="text/javascript" src="js/chart/exporting.js"></script>
    <script type="text/javascript" src="js/chart/highcharts.js"></script>--%>



    <script language="javascript" type="text/javascript">
        function invoke() {
            <%=vScript %>
        }

        //$(function () {
        //    $('#container').highcharts({
        //        chart: {
        //            type: 'bar'
        //        },
        //        title: {
        //            text: 'Historic World Population by Region'
        //        },
        //        subtitle: {
        //            text: 'Source: Wikipedia.org'
        //        },
        //        xAxis: {
        //            categories: ['Job Order No.', 'Job Order No.', 'Job Order No.', 'Job Order No.', 'Job Order No.'],
        //            title: {
        //                text: null
        //            }
        //        },
        //        yAxis: {
        //            min: 0,
        //            title: {
        //                text: 'Population (millions)',
        //                align: 'high'
        //            },
        //            labels: {
        //                overflow: 'justify'
        //            }
        //        },
        //        tooltip: {
        //            valueSuffix: ' millions'
        //        },
        //        plotOptions: {
        //            bar: {
        //                dataLabels: {
        //                    enabled: true
        //                }
        //            }
        //        },
        //        legend: {
        //            layout: 'vertical',
        //            align: 'right',
        //            verticalAlign: 'top',
        //            x: -40,
        //            y: 100,
        //            floating: true,
        //            borderWidth: 1,
        //            backgroundColor: (Highcharts.theme && Highcharts.theme.legendBackgroundColor || '#FFFFFF'),
        //            shadow: true
        //        },
        //        credits: {
        //            enabled: false
        //        },
        //        series: [{
        //            name: 'Year 1800',
        //            data: [22, 31, 33, 35, 66] 
        //        }, {
        //            name: 'Year 1900',
        //            data: [12, 45, 20, 98, 45]
        //        }, {
        //            name: 'Year 2008',
        //            data: [11, 43, 100, 76, 65]
        //        }]
        //    });
        //});
        // function openlink(id,s){
        //    linkwin=window.open('viewproperties.aspx?id='+id+"&s="+s,
        //        'viewprop','top=80,left=150,scrollbars=yes,resizable=yes,toolbars=no,width=700,height=550');
        //    linkwin.focus();
        //}
    </script>
    <%--<link href="redtheme/red.css" rel="stylesheet" type="text/css" />--%>
</head>
<body onload="invoke()" style=" background-position:center;">

    <form id="form1" runat="server">

        <%--
        <div id="container" style="min-width: 310px; max-width: 800px; height: 400px; margin: 0 auto; border:solid 0px; visibility:hidden"></div> 
        <div id="Div1" style="min-width: 310px; height: 400px; margin: 0 auto"></div>--%>


         <%--
        <div style="position:absolute; top: 30px; left: 50px; right: 50px; bottom: 5px; 
        	background:url(images/mainlogo.png) center no-repeat; border: solid 0px #000000;"></div>
        <div style="position:absolute; top: 30px; left: 10px; right: 10px; bottom: 5px; 
        	background:url(images/mainlogo.png) center no-repeat; border: solid 0px #cccccc; text-align:center;" >
    	    <table id="tblDoc" border="1" style="border-collapse:collapse; width:100%">
    	       <tr class="titleBar">
    	            <th class="labelC" style="width: 60px;">Doc Id</th>
    	            <th class="labelC">Doc Name</th>
    	            <th class="labelC" style="width: 200px;">Company</th>
    	            <th class="labelC" style="width: 130px;">Doc Category</th>
    	            <th class="labelC" style="width: 130px;">Doc. Status</th>
    	            <th class="labelC" style="width: 100px;">Process By</th>
    	            <th class="labelC" style="width: 130px;">Date Received</th>
    	            <th class="labelC" style="width: 70px;">Elapsed <br />Time (hrs)</th>
    	        </tr>
    	        < % =vReminders.ToString()%>
    	    </table>
        </div>--%>
    </form>
</body>
</html>
