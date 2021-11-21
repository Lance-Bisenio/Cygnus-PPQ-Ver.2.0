<%@ Page Language="VB" AutoEventWireup="false" CodeFile="maintenance_ref.aspx.vb" Inherits="maintenance_ref" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        html, body, form {
            overflow: hidden;
            margin: auto;
            height: 100%;
            width: 100%;
        }

        .iDataFrame {
            width:98%; border: solid 10px #e2e2e2;
        }

        .RefList_x {
            width:245px; cursor:pointer;
            text-align: left; margin:0px;
            background-color: #ffffff;

            border: solid 0px #f00; 
            border-left: solid 0px #b7b6b6; 
            border-bottom: solid 1px #f0efef; 
            height:35px;

            display:inline-block;
            color:#777777;
            font-family:arial;
            font-size:11px;
            font-weight:bold;
	
            text-decoration:none;
            text-shadow:1px 1px 0px #ffffff;

        }
        .RefList {
            width:245px; cursor:pointer;
            text-align: left; margin:0px;
            background-color: #ffffff;

            border: solid 0px #f00; 
            border-left: solid 0px #b7b6b6; 
            border-bottom: solid 1px #f0efef; 
            height:35px;
             
            color:#777777;
            font-family:arial;
            font-size:11px;
            font-weight:bold;
	  
            text-shadow:1px 1px 0px #ffffff;
             outline: none; 
        }

        .RefList:hover {
            border-left: solid 5px #ea0016; 
            background-color: #f6f6f6;
            color: #ea0016;
             outline: none; 
        }

        .RefListActive {
            border: solid 0px #f00; 
            border-left: solid 5px #0f62ec; 

            background-color: #e2e2e2;
            color: #0f62ec;
            width:255px; cursor:pointer;
            text-align: left; margin:0px;
            height:35px;
            display:inline-block;
            font-family:arial;
            font-size:11px;
            font-weight:bold;
	
            text-decoration:none;
            text-shadow:1px 1px 0px #ffffff;
             outline: none; 
        }
    </style>

    <script type="text/javascript">

        function invoke() {
            <%=vScript %>
            var myWidth = 0, myHeight = 0;
            if (typeof (window.innerWidth) == 'number') {
                //Non-IE
                myWidth = window.innerWidth;
                myHeight = window.innerHeight;
            } else if (document.documentElement && (document.documentElement.clientWidth || document.documentElement.clientHeight)) {
                //IE 6+ in 'standards compliant mode'
                myWidth = document.documentElement.clientWidth;
                myHeight = document.documentElement.clientHeight;
            } else if (document.body && (document.body.clientWidth || document.body.clientHeight)) {
                //IE 4 compatible
                myWidth = document.body.clientWidth;
                myHeight = document.body.clientHeight;
            }
            //window.alert('Width = ' + myWidth);
            //window.alert('Height = ' + myHeight);


            document.getElementById("table1").style.height = myHeight + "px";
            document.getElementById("frmContent").style.height = myHeight - 30 + "px";

        }

        function showModule(vLink, vCaption, vId) {
            document.getElementById("frmContent").src = vLink;
            
            var vLastSeleted = document.getElementById("txtLastSelected").value;

            if (vLastSeleted != "") {
                document.getElementById(vLastSeleted).className = "RefList"
            }
            
            document.getElementById(vId).className = "RefListActive"
            document.getElementById("txtLastSelected").value = vId;

            var l = document.getElementById("lblModuleTitle");
            l.innerHTML = vCaption;

            
        }
    </script>
</head>
<body onload="invoke();">
    <form id="form1" runat="server">

    <div style="position:absolute; top:0px; left: 5px; width:250px;">
        <br /><br />
        <%=vMenus %>
        <input type="hidden" id="txtLastSelected" value="" />
    </div>

    <div style="vertical-align: top; ">
        
        <table id="table1" border="0" style="width: 100%; margin:0px; padding:0px; border-collapse:collapse;">
            <tr>
                <td style="width: 250px; vertical-align: top; ">
                    
                </td>
                <td style="vertical-align: top">
                    <iframe id="frmContent" class="iDataFrame" src="" style=""></iframe> <%--main.aspx--%>
                </td>
            </tr>
        </table>
        
    </div>
    </form>
</body>
</html>
