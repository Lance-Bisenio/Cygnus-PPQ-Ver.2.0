<%@ Page Language="VB" AutoEventWireup="false" CodeFile="production_monitoring.aspx.vb" Inherits="production_monitoring" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    
    <title></title>
    <link href="../bootstrap/css/bootstrap.css" rel="stylesheet" />
    <link href="../css/jquery_datatable.css" rel="stylesheet" /> 
    <link href="../css/colorbox.css" rel="stylesheet" />
    <link href="../css/jquery-ui.css" rel="stylesheet" />  

    <style type="text/css">
    	div { margin-bottom:2px; border: solid 0px; }
    	.remove_pad {
    		padding-left :0px; margin-left:0px
		}

    	.rawwid {
			margin:auto;
    		width:99%;
		}


        /*.iDataFrame {
            width:99%; border: solid 0px #e2e2e2; height: 96%; margin:0px;
        }
        .GridButton {   
            background: #f2f2f2;
            cursor:pointer; padding:3px; padding-left:7px; padding-right:7px; 
	        text-shadow: 1px 1px 1px #e5e5e5; outline: none; 
            font-weight:bold; color: #434343; 
        }
        .pad {
            padding:0px;
        }*/
    </style>


</head>
<body onload="invoke();" style="font-family:Arial;font-size:12px;">
    <form id="form1" runat="server"> 
    
    <div class="container-fluid">
		<div class="row rawwid">
			<div class="col-sm-8"></div>
			<div class="col-sm-4">
				<div class="btn-group pull-right" id="divAction" runat="server" style="display:none;">
					<button type="button" class="btn btn-success disabled">
						<asp:Label ID="lblName" runat="server" Text="User"></asp:Label>
					</button>
					<button type="button" class="btn btn-success disabled">
						<asp:Label ID="lblSection" runat="server" Text="Section"></asp:Label>
					</button>
					<asp:Button ID="Button1" runat="server" Text="Log Out" CssClass="btn btn-danger" />
				</div> 
			</div>
		</div> 
		<br /> 
		<div class="row rawwid">
            <div class="col-lg-2">Year :</div>
            <div class="col-lg-3"> 
                <asp:DropDownList ID="CmdYear" runat="server" CssClass="form-control pad col-lg-3" AutoPostBack="True"></asp:DropDownList>
            </div>
			<div class="col-lg-1"> </div>
			<div class="col-lg-2">Prod Status :</div>
            <div class="col-lg-3">
                    <asp:DropDownList ID="cmdStatus" runat="server" CssClass="form-control pad" ></asp:DropDownList>
            </div>
			<div class="col-lg-1"> </div>
        </div>
		<div class="row rawwid">
            <div class="col-lg-2">Month :</div>
            <div class="col-lg-3">
                <asp:DropDownList ID="CmdMonths" runat="server" CssClass="form-control pad" AutoPostBack="True"></asp:DropDownList>
            </div>
			<div class="col-lg-1"> </div>
			<div class="col-lg-2">Machines :</div>
            <div class="col-lg-3">
                    <asp:DropDownList ID="cmdMac" runat="server" CssClass="form-control pad" ></asp:DropDownList>
            </div>
			<div class="col-lg-1"> </div>
        </div>
        <div class="row rawwid">
            <div class="col-lg-2">Job order list :</div>
            <div class="col-lg-3">
                <asp:DropDownList ID="cmdJOList" runat="server" CssClass="form-control pad"></asp:DropDownList>
            </div>
			<div class="col-lg-3"></div>
			<div class="col-lg-3"> </div>
        </div>
            
        <div class="row rawwid">
            <div class="col-lg-2"></div>
            <div class="col-lg-3 remove_pad">
				<div class="col-lg-9" style="padding-right:3px">
					<input type="text" id="TxtSearch" runat="server" class="form-control" placeholder="Search JONO Nunber" />
				</div>
				<div class="col-lg-3 remove_pad">
					<asp:Button ID="btnRefresh" CssClass="btn btn-md btn-info" runat="server" Text="Refresh" />
				</div> 
            </div>
        </div>			
		<div class="row rawwid">
            <div class="col-lg-2"></div>
            <div class="col-lg-4 remove_pad">
				<div class="col-lg-9"><h5>Total number of records retrieved : <%=vTotalRecords %> </h5></div>
				<div class="col-lg-3 remove_pad"> 
				</div> 
            </div>
        </div>		
   
		<div class="row rawwid">
            <div class="col-lg-12">
                <table class="table table-bordered">
                    <%=vHeader %>
                    <%=vData %>
                </table>
            </div>
        </div>  


		<div id="divBG_A" style="position:absolute; top:20px; left:32%; right:32%; border:solid 10px #000; 
			 background: #000; overflow:hidden; visibility:hidden; z-index:1;">
			 <%--<iframe id="frmContent" style="height:505px; width:100%; border: 0px #000; overflow:hidden;" ></iframe> --%>
			<label style="float:left; color:#fff; font-size:14px;">Production Properties</label>
			<input id="btnClose" type="button" class="btn btn-danger btn-sm" style="float:right;margin-bottom:10px;" value="Close" onclick="ClosePop();"/>
			<object id="frmContent" style="height:460px; width:100%; border: 0px #000; overflow:hidden; margin:0px" type="text/html"> 
			</object>
		</div>
		<div id="divBG_B" style="position:absolute; top:0px; left:0%; right:0%; bottom:0px;
					margin: 0; position:absolute; z-index:0; visibility:hidden;  
					padding: 0; background:#000; border: solid 0px; 
					filter: alpha(opacity=50);  
					-moz-opacity: 0.8;         
					-khtml-opacity: 0.8;      
					opacity: 0.8;">
		</div>
    
    </div>																				

	</form>
    
</body>
</html>


<script src="../js/jquery-1.10.2.js" type="text/javascript"></script>
<script src="../media/js/jquery.dataTables.js" type="text/javascript"> </script>
<script src="../js/jquery.colorbox-min.js" type="text/javascript"></script>


<script type="text/javascript">
    function invoke() {
        <%=vScript %>
    }

    $(document).ready(function () {
        $('.btn.btn-primary.btn-sm').on('click', function () {
             
            //document.getElementById("divBG_A").style.visibility = "visible";
            //document.getElementById("divBG_B").style.visibility = "visible";
            //plink = "production_menus.aspx?pTranId=" + this.id + "&pSFGCd=" + $('#lblScrId').val() + "&pCode=" + $('#txtvalue').val() + ""
            //document.getElementById("frmContent").data = plink;
            
            $('.btn.btn-primary.btn-sm').colorbox({
                'href': "production_menus.aspx?pTranId=" + this.id + "&pSFGCd=" + $('#lblScrId').val() + "&pCode=" + $('#txtvalue').val() + "",
                'width': '450px',
                'height': '630px;',
                'top': '20px',
                iframe: true,
                scrolling: false,
            });
        });
         
        //$('#btnClose').on('click', function () {
        //    document.getElementById("divBG_A").style.visibility = "hidden";
        //    document.getElementById("divBG_B").style.visibility = "hidden";
        //});

    });


        
</script>
