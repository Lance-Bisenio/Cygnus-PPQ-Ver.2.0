<%@ Page Language="VB" AutoEventWireup="false" CodeFile="joborder-IncidentLog.aspx.vb" Inherits="joborderIncidentLog" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../css/calendar/jquery-ui-1.10.4.custom.min.css" rel="stylesheet" /> 
    <link href="../bootstrap/bootstrap/css/bootstrap.css" rel="stylesheet" />
	<link href="../css/gridview.css" rel="stylesheet" />
    <style>
        div {
            padding-bottom: 2px;
        } 
        .hideGridColumn
        {
            display:none;
        }
    </style>
</head>
<body onload="invoke();" >
    <form id="form1" runat="server" autocomplete="off">
         
        <div class="container-fluid">
            <div class="row">
                <div class="col-sm-12">
                    <h3>Incident Report</h3>
                    <hr />
                </div>
            </div>
            <div class="row">
                <div class="col-sm-6">
                    <div class="btn-group">
                        <button type="button" class="btn btn-primary btn-sm" data-toggle="modal" data-target="#myModal">Create New</button>
                        <button type="button" class="btn btn-primary btn-sm" data-toggle="modal" data-target="#myModal">Edit</button>
                        <button type="button" class="btn btn-primary btn-sm">Delete</button>
                    </div>
                </div>
                <div class="col-sm-6 text-right">
                    <asp:Label ID="lblMessage" runat="server" Text="..."></asp:Label>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-12">
                    <asp:GridView ID="tblIncidentList" runat="server" AllowPaging="True"  
					    AutoGenerateColumns="False" Width="100%"
					    CssClass="mainGridView" PageSize="10"> 
					    <Columns>
                            
						    <asp:CommandField ButtonType="Button" ShowSelectButton="True" > 
							    <ItemStyle CssClass="" Width="40px" />
							    <ControlStyle CssClass="btn btn-primary btn-xs" />
						    </asp:CommandField>

						    <asp:TemplateField HeaderText="#" HeaderStyle-Width="30px" >   
							    <ItemTemplate>
								    <%# Container.DataItemIndex + 1 %>   
							    </ItemTemplate>
						    <HeaderStyle Width="30px"></HeaderStyle>
						    </asp:TemplateField>

                            <asp:BoundField DataField="ReasonDescription" HeaderText="Reason ID" HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn" ItemStyle-width="80px"/>
                                
						    <asp:BoundField DataField="Descr" HeaderText="Description" >
							    <ItemStyle/>
						    </asp:BoundField>
                         
						    <asp:BoundField DataField="RootCause" HeaderText="Root Cause" >
							    <ItemStyle />
						    </asp:BoundField>
                         
						    <asp:BoundField DataField="TimeStop" HeaderText="Time Stop" >
							    <ItemStyle />
						    </asp:BoundField>

						    <asp:BoundField DataField="TimeStart" HeaderText="Time Start" >
							    <ItemStyle />
						    </asp:BoundField>

						  <%--  <asp:BoundField DataField="IncidentDate" HeaderText="Incident Date" >
							    <ItemStyle />
						    </asp:BoundField>--%>
	
						    <asp:BoundField DataField="CreatedBy" HeaderText="Created By" >
							    <ItemStyle />
						    </asp:BoundField>
                            <asp:BoundField DataField="DateCreated" HeaderText="DateCreated" >
							    <ItemStyle />
						    </asp:BoundField></Columns>
            
					    <SelectedRowStyle CssClass="activeBar" />
					    <PagerStyle Font-Size="8pt" /> 
					    <HeaderStyle CssClass="titleBar" />
					    <RowStyle CssClass="odd" />
					    <AlternatingRowStyle CssClass="even" />
				    </asp:GridView> 
                </div>
            </div>
            


            <div id="myModal" class="modal fade" role="dialog">
                <div class="modal-dialog">
                     
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                            <h4 class="modal-title">Incident Log</h4>
                        </div>
                        <div class="modal-body"> 

                            <div class="container-fluid">
                                <div class="row">
			                        <div class="col-sm-4">Machine:</div>
                                    <div class="col-sm-8">
                                        <asp:DropDownList ID="DdlMachine" CssClass="form-control" runat="server"></asp:DropDownList>
                                    </div>
                                </div>
		                        <div class="row">
			                        <div class="col-sm-4">Incident Reason:</div>
                                    <div class="col-sm-8">
                                        <asp:DropDownList ID="DdlReason" CssClass="form-control" runat="server"></asp:DropDownList>
                                    </div>
                                </div>
                                <div class="row">
			                        <div class="col-sm-4">Root Cause:</div>
                                    <div class="col-sm-8">
                                        <asp:TextBox ID="TxtRootCause" CssClass="form-control" runat="server" TextMode="MultiLine" Rows="2"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="row">
			                        <div class="col-sm-4">Date Stop:</div>
                                    <div class="col-sm-3">
                                        <asp:TextBox ID="txtDateFrom" CssClass="form-control" runat="server" placeholder="mm/dd/yyyy"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-2"><asp:TextBox ID="TxtTimeStop" CssClass="form-control" runat="server" placeholder="00:00"></asp:TextBox></div>
                                    <div class="col-sm-3">
                                        <label class="radio-inline"><input type="radio" id="RdoTimeStopAM" runat="server" name="optradioStop" checked="" />AM</label>
                                        <label class="radio-inline"><input type="radio" id="RdoTimeStopPM" runat="server" name="optradioStop" />PM</label>
                                    </div>
                                </div>
                                <div class="row">
			                        <div class="col-sm-4">Date Fixed:</div>
                                    <div class="col-sm-3">
                                        <asp:TextBox ID="txtDateStart" CssClass="form-control" runat="server" placeholder="mm/dd/yyyy"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-2"><asp:TextBox ID="TxtTimeStart" CssClass="form-control" runat="server" placeholder="00:00"></asp:TextBox></div>
                                    <div class="col-sm-3">
                                        <label class="radio-inline"><input type="radio" id="RdoTimeStartAM" runat="server" name="optradioStart" checked="" />AM</label>
                                        <label class="radio-inline"><input type="radio" id="RdoTimeStartPM" runat="server" name="optradioStart" />PM</label>
                                    </div>
                                </div>






                                <%--<div class="row">
			                        <div class="col-sm-4">Time Stop:</div>
                                    <div class="col-sm-2">
                                        
                                    </div>
                                    <div class="col-sm-2">hh:mm</div>
                                    <div class="col-sm-4">
                                        
                                    </div>
                                </div>
                                <div class="row">
			                        <div class="col-sm-4">Time Start:</div>
                                    <div class="col-sm-2">
                                        
                                    </div>
                                    <div class="col-sm-2">hh:mm</div>
                                    <div class="col-sm-4">
                                        
                                    </div>
                                </div>
                                <div class="row">
			                        <div class="col-sm-4"></div>
                                    <div class="col-sm-8"> 
                                    </div>
                                </div>--%>

                            </div>

                        </div>
                        <div class="modal-footer">
                            <asp:Button ID="BtnSave" CssClass="btn btn-primary btn-sm" runat="server" Text="Submit" />
                            <button type="button" class="btn btn-default btn-sm" data-dismiss="modal">Close</button>
                        </div>
                    </div>

                </div>
            </div>

        </div>
        
    </form>
</body>
</html>
 

<script src="../js/jquery-1.10.2.js"></script> 
<script src="../js/jquery-ui.js"></script>  
<script src="../bootstrap/bootstrap/js/bootstrap.js"></script>  
   

<script type="text/javascript">
		 
    function invoke() {
        <%=vScript %>
    }

    function ShowDetails() {
       
        $('#ModalItemDetails').modal('show');
    }

    $(document).ready(function () {
		$(document).ready(function () {
			$("#txtDateFrom").datepicker(); 
            $("#txtDateStart").datepicker();
            
        });  

        $('ItemHeader').click(function(){
            
            alert("Data: lance ");
          
        });
	});

                
</script>