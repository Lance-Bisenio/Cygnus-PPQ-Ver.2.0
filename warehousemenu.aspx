<%@ Page Language="VB" AutoEventWireup="false" CodeFile="warehousemenu.aspx.vb" Inherits="warehousemenu" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <title></title>
    <link href="content/bootstrap/bootstrap-4.6.0-dist/css/bootstrap.css" rel="stylesheet" />
    <script src="content/jquery/jquery/jquery.3.6.js"></script>
    <link href="https://fonts.googleapis.com/css2?family=Montserrat:wght@300&display=swap" rel="stylesheet" />
    <style>
        body {
            font-family: 'Montserrat', sans-serif;
        }
        /*object {
            border: solid 1px #ccc;
        }

        .WhPanel {
            
        }

        .tab {*/
        /*float: left;*/
        /*background-color: #f1f1f1;
        }*/

        /* Style the buttons inside the tab */
        /*.tab button {
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
            }*/

        /* Change background color of buttons on hover */
        /*.tab button:hover {
                    background-color: #ddd;
                    color: #000;
                }*/

        /* Create an active/current "tab button" class */
        /*.tab button.active {
                    background-color: #007bff;
                    color: #f1f1f1;
                }*/

        .sidebar {
            background-color: #F5F4F4;
            border: solid 0pc #000;
            box-shadow: 5px 7px 25px #999;
            height: 100vh;
            max-width: 300px
        }

        .bottom-border {
            border-bottom: 1px groove #eee;
        }

        .sidebar-link {
            color:#444;
            display: block;
            font-size:15px;
            padding: 0.5rem 1rem;
            transition: all 1s;
        }
            .sidebar-link:hover {
                background-color:#444;
                border-radius: 5px;
                color:#fff;
                text-decoration:unset;
            }
    </style>
    <script>



        $(document).ready(function () {

            var hA = $(window).height();
            var wHeight = $(document).height();
            var panelWidth = $("#panel").width();

            $("object").css({ "height": (wHeight - 20) + "px", "width": + panelWidth + "px" });


            $("button").click(function () {
                alert(this.id + " : Handler for .click() called.");
                $("#FrmWarehouse").attr("data", "warehouse.aspx");
            });


        });

        function OpenModule() {

        }

    </script>
</head>
<body>

    <form id="form1" runat="server">

        <nav class="navbar navbar-expand-md navbar-light">
            <div class="collapse navbar-collapse">
                <div class="container-fluid">
                    <div class="row">
                        <div class="col-lg-3 sidebar fixed-top mb-5">
                            <label class="nav-brand d-block mx-auto text-center py-3 mb-4 bottom-border">Warehouse Menu</label>


                            <ul class="navbar-nav flex-column mt-5">
                                <li class="nav-item">
                                    <a href="#" class="p-2 sidebar-link">Warehouse Management</a>
                                </li>
                                <li class="nav-item">
                                    <a href="#" class="p-2 sidebar-link">Transfer to Process Warehouse</a>
                                </li>
                                <li class="nav-item">
                                    <a href="#" class="p-2 sidebar-link">Transfer to Production Order</a>
                                </li>
                                <li class="nav-item">
                                    <a href="#" class="p-2 sidebar-link">Transfer to Cabuyao Warehouse</a>
                                </li>
                            </ul>
                        </div>
                         

                    </div>
                </div>
            </div>
        </nav>

        <section>
            <div class="container-fluid">
                <div class="row">
                    <div id="panel" class="col-lg-10 ml-auto">
                        <object id="FrmWarehouse" data="warehouse.aspx" type="text/html">
                            Alternative Content
                        </object>
                    </div>
                </div>
            </div>
        </section>












        <%--<div class="container-fluid">
            <div class="row">
                <div class="col-sm-2">
                    <div class="tab">       
                        <button id="defaultOpen">Warehouse Management</button>
                        <button id="TPW">Transfer to Process Warehouse</button>
                        <button id="TPO">Transfer to Production Order</button>
                        <button id="TCW">Transfer to Cabuyao Warehouse</button>
                    </div>
                </div>
                <div id="panel" class="col-sm-10">
                    <object id="FrmWarehouse" data="warehouse.aspx" type="text/html">
                        Alternative Content
                    </object>
                </div>
            </div>

        </div>--%>
    </form>
</body>
</html>




