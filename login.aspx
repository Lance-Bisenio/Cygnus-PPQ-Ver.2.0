<%@ Page Language="VB" AutoEventWireup="false" CodeFile="login.aspx.vb" Inherits="login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>PPQ Login Page</title>
    
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no" />
    <style type="text/css" > 
		body { 
             background-image: url(images/bg.jpg);
            background-size: cover; z-index:-9999; 
		}

        f7 { font-size:8px; }
	</style> 
    <link href="css/bootstrap4/css/bootstrap.min.css" rel="stylesheet" /> 
     
    <script type="text/javascript">
         
        function PortalLink(pVal) { 
            parent.window.location = "Portallogin.aspx";
        }
  </script>
</head>
<body>
	
    <form id="frmlogin" runat="server" autocomplete="off" >

        <div class="container">
          

		<div class="row">
            <div class="col-md-4"></div>
            <div class="col-md-4">
                <div class="panel panel-primary" style="width:100%; 
                    margin:auto; box-shadow: 2px 2px 10px #000; padding:20px; margin-top:50px; 
                    border: solid 0px #bb1100; background-color:#fff;"> 
					 
                    <div class="panel-body">
                        
                        <div class="row">
                            <div class="col-sm-12">
                                <h2>LOGIN</h2>
						        <h6>Planning Production and Quality System</h6>
                            </div>
                        </div>
                         
                        <br />
                        <br />
                        <div class="row">
                            <div class="col-sm-12">
                                
                                    <%--<div class="input-group-prepend">
                                        <span class="input-group-text" id="basic-addon1" style="padding: 0px; margin:0px">
                                            <img alt="" src="images/userid_icon.png" style="width:35px; padding:0px; margin:2px" />
                                        </span>
                                    </div>--%>

                                    <%--<h5><small>Username:</small></h5>--%>
                                    <input type="text" id="Txtuser" runat="server" class="form-control" value="" maxlength="16" onfocus="formrule(this)" placeholder="Username"
   			                            title="Enter a valid username consisting of alphanumeric characters without spaces." 
                                        aria-label="Username" aria-describedby="basic-addon1"  onkeypress="CheckKeys(event)" />
                                     
                                
                            </div> 
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-sm-12">
                                
                                   <%-- <div class="input-group-prepend">
                                        <span class="input-group-text" id="basic-addon2" style="padding: 0px; margin:0px">
                                            <img alt="" src="images/password_icon.png" style="width:30px; padding:0px; margin:5px" />
                                        </span>
                                    </div>--%>

                                    <%--<h5><small>Password:</small></h5>--%>
                                    <input type="password" id="Txtpwd" runat="server"  class="form-control" maxlength="16" style="" placeholder="Password" 
                                        aria-label="Username" aria-describedby="basic-addon2" onkeypress="CheckKeys(event)" />

                                    <small class="text-info">Never share your password with anyone else.</small>
                            </div> 
                        </div>
                        <br />
                        <div class="form-group form-group-sm">  
                            <input type="button" id="Cmdlog" runat="server" name="btnLogIn" value="LOG IN" class="btn btn-primary" 
                                style="width:100%"/>  

                             <%--<input type="button" id="Button1" runat="server" name="btnLogIn" value="Log In" class="btn btn-primary btn-lg"
            	                onclick="document.getElementById('divUpdates').style.visibility='visible'; loginexec();" 
                                style="width:100%"/>  --%>
                        </div>
         

                        <div class="form-group form-group-sm">  
                            <label id="lblPortalLink" class="btn-link" onclick="PortalLink(this.id);">Helper Portal</label>  
                        </div>
                         <div class="row">
                            <div class="col-sm-12">

                                <div class="alert alert-danger" id="dvError" visible="false" runat="server">
                                    <strong>Access Denied!</strong>&nbsp;:&nbsp;
                                    <asp:Label ID="lblError" runat="server" ForeColor="#FF3300" Font-Size="Small"></asp:Label>
                                </div>  



                                <%--<div class="input-group mb-3"> 
                                    <span id="errmsg" style="font-size: 11px; font-weight:bold; color: #e20404; font-family: Arial; text-decoration:double;"></span>
                                    <div id="divWait" style="visibility:hidden; margin:auto; width: 250px; border: 0px solid #cccccc; background-color:transparent;">
                                        <asp:Image ID="imgWait" runat="server" ImageUrl="images/clock.gif" />
        	                        </div>--%>

                                    
                                    <%--Access Denied!   
                                    Supply the correct Client Code, Employee Code and Password to access your ESS account.

                                    <div style="width: 256px; height: 24px; background-color: white; text-align: center; visibility:hidden; z-index: 102; text-decoration: blink; 
                                       font-size: 12px; border: 0px solid; font-family: Arial; color:#FFFFFF;" id="divUpdates">
                                        Please wait... Checking for updates...
                                    </div>--%>

                                </div> 
                            </div> 
                        </div>  
                        <br />
                        <div class="row">
                            <div class="form-group form-group-sm"> 
                                <div class="col-sm-12">
                                    <div class="input-group"> 
                                        <div style="height: 20px; font-family:Arial, Helvetica, sans-serif; font-size: 11px; color:#d4d4d4; padding-left: 10px;">
                                            Copyright © <%=Now.Year  %> <a href="http://www.**.com" style="font-weight: bold;
                                            font-size: 11px; color: #d4d4d4; font-family: Arial, Helvetica, sans-serif; text-decoration: none"
                                            target="_blank">SanPiox Solutions</a>
                                    </div>
                                </div>
                            </div> 
                        </div>
                         
                    </div>  
                </div>
            </div>
            <div class="col-md-4"></div>
		</div>
    </div>

            



   <%-- <center>
    
        <div style="width:250px; border: solid 0px #000000;">   
            <br /><br /><br /><br /><br /><br />
            <div class="LogIn_Box" >
                
            	<table class="Login_TitleBox" border="0" align="center">
                    <tr><td>&nbsp;</td></tr>
                    <tr>
                    	<td class="LogIn_Title">Planning Production and Quality System </td>
                    </tr>
                    <tr>
                    	<td class="LogIn_TitleSub">SanPiox Solutions</td>
                    </tr>
					<tr><td style="height:80px;"></td></tr>
                </table>
                	
                <table class="text_BoxPanel" border="0" align="center">
                    <tr style="border-bottom: solid 1px #CCCCCC;">
                        <td style="width:50px; height:40px; background:url(images/userid_icon.png) center no-repeat;"></td>
                        <td style="text-align:left;" valign="middle">
                            
                        </td>
                    </tr>
                    <tr>
                    	<td style="height:40px; background:url(images/password_icon.png) center no-repeat;"></td>
                        <td style="text-align:left;">
                           
                        </td>  
                    </tr>
                    <tr>
                        <td></td>
                        <td></td>
                    </tr>
                </table>
                
                <table border="0" style="width:210px; border-collapse:collapse;">
                    <tr><td style="height:10px;"></td></tr>
                    <tr>
                        <td align="left"><label id="lblPortalLink" class="MenuLink" onclick="PortalLink(this.id);">Helper Portal</label></td>
                    	<td align="right">
                        	
                        </td>
                    </tr>
					<tr><td style="height:50px;"></td></tr> 
                </table>
            </div>
            
            <div style="width:100%; border:solid 0px #ffffff;">
	            <table style="width:100%; border-collapse:collapse;" border="0">
                    <tr><td style="height:10px;"></td></tr>
                    <tr><td align="center" style="height: 20px; font-family:Arial, Helvetica, sans-serif; font-size: 11px; color:#d4d4d4; padding-left: 10px;">
                        Copyright © < %=now.Year  %> <a href="http://www.**.com" style="font-weight: bold;
                        font-size: 11px; color: #d4d4d4; font-family: Arial, Helvetica, sans-serif; text-decoration: none"
                        target="_blank">SanPiox Solutions</a>
                    </td></tr>
                    <tr>
                    	<td style="text-align:center; height:15px;">
                        	
                        </td>
                    </tr>
                    <tr>
                    	<td style="text-align:center;">
                        	<center>
                                <div style="width: 256px; height: 24px; background-color: white; text-align: center; visibility:hidden; z-index: 102; text-decoration: blink; 
                                   font-size: 12px; border: 0px solid; font-family: Arial; color:#FFFFFF;" id="divUpdates">
                                    Please wait... Checking for updates...
                                </div>
                            </center>
                        </td>
                    </tr>
                </table>
            </div>
	
        	<%--Loading images- -%>	
            

        </div>
        
        
    </cente r>
         --%>
</form>    
</body>
</html>


<script>
    function CheckKeys(p) {
        if (p.which == 13) {
            document.getElementById("frmlogin").submit()
        }
    }

    function MeLoad() {
        document.getElementById("frmlogin").submit()
    }
</script>