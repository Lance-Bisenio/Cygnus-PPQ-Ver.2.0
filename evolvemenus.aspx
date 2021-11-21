<%@ Page Language="VB" AutoEventWireup="false" CodeFile="evolvemenus.aspx.vb" Inherits="evolvemenus" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />

<link href="css/menu_v2.css" rel="stylesheet" type="text/css" media="screen" />

<title>(PPQ) Planning Production and Quality System</title>
 
 
		<link rel="stylesheet" type="text/css" href="css/menu/default.css" />
		<link rel="stylesheet" type="text/css" href="css/menu/component.css" /> 

        <%--<script  type="text/javascript" src="js/jquery-ui-1.10.4.custom.js"></script>--%>
        <%--<script  type="text/javascript" src="js/jquery-ui-1.10.4.custom.min.js"></script>
        <script  type="text/javascript" src="js/jquery-1.10.2.js"></script>--%>
        <%--<script  type="text/javascript" src="js/jquery-ui.js"></script>--%>

    <script src="js/jquery-ui-1.12.1/external/jquery/jquery.js"></script>
    <script src="js/jquery-ui-1.12.1/jquery-ui.min.js"></script>


        <script  type="text/javascript" src="js/menu/modernizr.custom.js"></script> 


        <script type="text/javascript">
            function showModule(vLink, vCaption) {
                document.getElementById("frmContent").src = vLink;
                var l = document.getElementById("lblModuleTitle");
                $('#lblTitle').val(vCaption);
                document.getElementById("lblTitle").text = "lance test";
                //l.innerHTML = vCaption;
            }

            $(document).ready(function () {
                $('ul.dl-menu').mouseleave(function () {
                    $('.dl-trigger').click();
                });
            });
        </script>
    <style type="text/css">
     html, body, form {
        overflow: hidden;
        margin: auto;
        height: 100%;
        width: 100%;
        background-color:#e0e0e0;
     }
        .MenuTitle {
            background:none; border: solid 0px;  padding-left:10px; font-weight:bold; width:200px;
            text-decoration:none;
	        text-shadow: 1px 1px 1px #000;
            color:#e0e0e0;
        }
  </style>
</head>
<body style="font-family:Arial;">
<form id="form1" runat="server">
    <div style="position:absolute; top:0px; right: 0px; left:0px; height:35px; border: solid 1px #000000; background-color:#555555;"></div>
    <div id="dl-menu" class="dl-menuwrapper">
		<button class="dl-trigger">Menu</button><input type="text" id="lblTitle" name="lblTitle" class="MenuTitle" readonly="readonly" />
		<ul class="dl-menu">
             
			<li> 
                <table class="table-menu" border="0" >
                    <thead>
                        <tr><td colspan="3" style="height:10px;">&nbsp;</td></tr>
                    </thead>
                    <tbody> 
                        <%=vMenus%>   
                    </tbody>

                    <tfoot>
                        
                    </tfoot>
                </table>
                <table class="table-menu-footer" style="width:100%" border="0" >
                    <thead>
                        <tr><td colspan="3">&nbsp;</td></tr>
                    </thead>
                    <tbody>  
                        <tr>
                            <td style="text-align:left;font-size:10px;color:#9e9e9e; padding-left:8px">Copyright © 2014-2014 SanPiox Solutions </td>
                            <td class="logout"><img alt="" src="images/menu/logout.png" style="margin-top:11px" /></td>
                        </tr>
                    </tbody> 
                </table>
            </li>     
		</ul>
	</div>

    <iframe id="frmContent" src="main.aspx" style="margin-top:5px; background-color:#ffffff" width="100%" height="93%" frameborder="0"></iframe>
    <div style="position:absolute; top:5px; right: 20px; border: solid 0px #000000;">
        <asp:LinkButton ID="SignOut" Width="100%" runat="server" CssClass="SignOut">Sign Out</asp:LinkButton>
    </div>

    
    <%--<script type="text/javascript" src="h t t p s ://ajax.googleapis.com/ajax/libs/jquery/1.9.1/jquery.min.js"></script>--%>
    <script type="text/javascript" src="js/menu/jquery.min.js"></script>
	<script type="text/javascript" src="js/menu/jquery.dlmenu.js"></script>
	<script type="text/javascript">
	    $(function () {
	        $('#dl-menu').dlmenu();
	    });
	</script>

</form>
</body>

</html>