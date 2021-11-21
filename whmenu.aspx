<%@ Page Language="VB" AutoEventWireup="false" CodeFile="whmenu.aspx.vb" Inherits="whmenu" %>

<!DOCTYPE html>

<html>
<head>
    <title></title>
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <link href="content/bootstrap/bootstrap-5.0.2-dist/css/bootstrap.css" rel="stylesheet" />
    <script src="content/jquery/jquery/jquery.3.6.js"></script>

    <style>
        .frame {
            width: 100%;
            margin: auto;
        }

        * {
            box-sizing: border-box
        }

        body {
            font-family: "Lato", sans-serif;
        }

        /* Style the tab */
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

        /* Style the tab content */
        .tabcontent {
            float: left;
            padding: 0px 12px;
            border: 2px solid #ccc;
            width: 90%;
        }
    </style>

    <script>


        $(document).ready(function () {
            var hA = $(document).height();
            var hB = $(window).height();

            alert(width + ":" + height);

            $("WhPanel").css("height", hB); //= $(window).height();
        });


        function openCity(evt, cityName) {
        //    var i, tabcontent, tablinks;
        //    tabcontent = document.getElementsByClassName("tabcontent");
        //    for (i = 0; i < tabcontent.length; i++) {
        //        tabcontent[i].style.display = "none";
        //    }
        //    tablinks = document.getElementsByClassName("tablinks");
        //    for (i = 0; i < tablinks.length; i++) {
        //        tablinks[i].className = tablinks[i].className.replace(" active", "");
        //    }
        //    document.getElementById(cityName).style.display = "block";
        //    evt.currentTarget.className += " active";
        }

        // Get the element with id="defaultOpen" and click on it
        document.getElementById("defaultOpen").click();


        function LoadSide() {


            //var x = "Total Height: " + screen.height;
            //document.getElementById("FrmWarehouse").height = screen.height - 200;
            //document.getElementById("FrmProcessWarehouse").height = screen.height - 200;

        }
    </script>
</head>
<body onload="LoadSide()">

    <div class="row">
        <div class="col-sm-2">
            <div class="tab">
                <button onclick="openCity(event, 'London')" id="defaultOpen">Warehouse Management</button>
                <button onclick="openCity(event, 'Paris')">Transfer to Process Warehouse</button>
                <button onclick="openCity(event, 'inventory/curing.aspx')">Transfer to Production Order</button>
                <button onclick="openCity(event, 'Tokyo')">Transfer to Cabuyao Warehouse</button>
            </div>
        </div>
        <div class="col-sm-10">
            <div id="WhPanel" class="">
                <object id="FrmWarehouse" data="warehouse.aspx" class="frame" type="text/html">
                    Alternative Content
                </object>
            </div>
        </div>
    </div>



















    <%--    <div id="Paris" class="tabcontent">
       <object id="FrmProcessWarehouse" data="warehouse.aspx" class="frame" 
            type="text/html">
            Alternative Content
        </object>
    </div>

    <div id="Tokyo" class="tabcontent">
        <h3>Tokyo</h3>
        <p>Tokyo is the capital of Japan.</p>
    </div>--%>
     
</body>
</html>
