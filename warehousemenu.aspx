<%@ Page Language="VB" AutoEventWireup="false" CodeFile="warehousemenu.aspx.vb" Inherits="warehousemenu" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <title></title>
    <link href="content/bootstrap/bootstrap-5.0.2-dist/css/bootstrap.css" rel="stylesheet" />
    <script src="content/jquery/jquery/jquery.3.6.js"></script>
    <style>
        object {
            border: solid 1px #ccc;
        }

        .WhPanel {
            
        }

        .tab {
            /*float: left;*/
            background-color: #f1f1f1;
        }

            /* Style the buttons inside the tab */
            .tab button {
                border: none;
                border-bottom: 1px solid #ccc;
                display: block;
                background-color: inherit;
                color: black;
                padding: 10px 10px;
                width: 100%;
                outline: none;
                text-align: left;
                cursor: pointer;
                transition: 0.3s;
                font-size: 14px;
            }

                /* Change background color of buttons on hover */
                .tab button:hover {
                    background-color: #ddd;
                    color: #000;
                }

                /* Create an active/current "tab button" class */
                .tab button.active {
                    background-color: #007bff;
                    color: #f1f1f1;
                }
    </style>
    <script>
        


        $(document).ready(function () {
           
            var hA = $(window).height();
            var wHeight = $(document).height();
            var panelWidth = $("#panel").width();
             
            $("object").css({ "height": (wHeight - 20) + "px", "width": + panelWidth + "px" });
             
        }); 
         
    </script>
</head>
<body>

    <form id="form1" runat="server">

        <div class="container-fluid">
            <div class="row">
                <div class="col-sm-2">
                    <div class="tab">
                        <button onclick="openCity(event, 'London')" id="defaultOpen">Warehouse Management</button>
                        <button onclick="openCity(event, 'Paris')">Transfer to Process Warehouse</button>
                        <button onclick="openCity(event, 'inventory/curing.aspx')">Transfer to Production Order</button>
                        <button onclick="openCity(event, 'Tokyo')">Transfer to Cabuyao Warehouse</button>
                    </div>
                </div>
                <div id="panel" class="col-sm-10">
                    <object id="FrmWarehouse" data="warehouse.aspx" type="text/html">
                        Alternative Content
                    </object>
                </div>
            </div>

        </div>
    </form>
</body>
</html>




