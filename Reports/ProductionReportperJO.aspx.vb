Imports denaro

Partial Class ProductionReportperJO
    Inherits System.Web.UI.Page
    Public vScript As String = ""
    Public vProcess As String = ""
    Public vReturnData As String = ""
    Public vData As String = ""
    Public vMCostSummary As String = ""
    Dim vSQL As String = ""

    Dim vJOBOM As String = ""
    Dim vJOBOMRev As Integer

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        If Session("uid") = Nothing Or Session("uid") = "" Then
            vScript = "alert('Login session has expired. Please re-login again.'); window.close();"
            Exit Sub
        End If

        If Not IsPostBack Then

            txtDateFrom.Text = Format(Now.AddDays(-7), "MM/dd/yyyy")
            txtDateTo.Text = Format(Now, "MM/dd/yyyy")

            vSQL = "select JobOrderNo, JobOrderNo from jo_header " _
                & "where JO_Status='RELEASE' and ReleaseDate between '" & txtDateFrom.Text.Trim & "' and '" & txtDateTo.Text.Trim & "'"
            BuildCombo(vSQL, cmdJOList)

            cmdJOList.Items.Add("Select Job Order Number")
            cmdJOList.SelectedValue = "Select Job Order Number"

        End If

    End Sub

    Private Sub CollectAllProcess()


        Dim c As New SqlClient.SqlConnection(connStr)
        Dim cm As New SqlClient.SqlCommand
        Dim rs As SqlClient.SqlDataReader
        Dim vJONumber As String = ""
        Dim vJOBOM As String = ""
        Dim vJOBOMRev As Integer


        Try
            c.Open()
        Catch ex As SqlClient.SqlException
            vScript = "alert('Error occurred while trying to connect to database. Error is: " _
                & ex.Message.Replace(vbCrLf, "").Replace("'", "") & "');"
            c.Dispose()
            cm.Dispose()
            Exit Sub
        End Try

        cm.Connection = c


        If TxtJONO.Text.Trim = "" Then
            vJONumber = cmdJOList.SelectedValue
        Else
            vJONumber = TxtJONO.Text.Trim
        End If

        vSQL = "select TranId, OperOrder, SFG_Cd,SFG_Descr, Sect_Cd,Proc_Cd, " _
                & "(select Descr +' '+Descr1 from item_master where SFG_Cd=Item_Cd ) as SFGName, " _
                & "(select Descr from ref_emp_section where Section_Cd=Sect_Cd) as vSection," _
                & "(select Descr from ref_item_process where ref_item_process.Proc_Cd=bom_process.Proc_Cd and ref_item_process.Sect_Cd=bom_process.Sect_Cd) as vProcess," _
                & "(select Remarks from prod_monitoring where SFG_Cd=SFG_Item_Cd and JobOrderNo='" & vJONumber & "') as vProdStatus " _
                & "from bom_process where BOM_Cd='" & vJOBOM & "' and Revision='" & vJOBOMRev & "' " _
                & " "


        cm.CommandText = vSQL

        If Session("SelectedProcess") = "" Then
            cmdProcessList.Items.Clear()
            cmdProcessList.Items.Add("Select Process")
        End If



        Try
            rs = cm.ExecuteReader
            Do While rs.Read


                If rs("vProcess").ToString.Trim <> "INK MIXING" And cmdProcessList.SelectedValue = "Select Process" _
                    Or Session("SelectedProcess") = rs("vProcess").ToString.Trim Then


                    vProcess += "<h5>" & rs("vProcess") & " / " & GetMachineName(rs("OperOrder"), vJONumber) & "</h5>"

                    If Session("SelectedProcess") = "" Then
                        cmdProcessList.Items.Add(rs("vProcess"))
                    End If

                    vProcess += "<div class='row'><div class='col-7'><table class='table table-bordered' style='font-size: 11px;'>" _
                        & "<thead> " _
                            & "<tr><td colspan='9'><b>INPUT</b></td></tr>" _
                            & "<tr>" _
                                & "<td>Item Code</td>" _
                                & "<td>Description</td>" _
                                & "<td>LOTNO</td>" _
                                & "<td>Roll No</td>" _
                                & "<td>QTY Release</td>" _
                                & "<td>Release By</td>" _
                                & "<td>QTY Receive</td>" _
                                & "<td>Receive By</td>" _
                                & "<td>Shift</td>" _
                            & "</tr>" _
                        & "</thead>"

                    'GetReceivedMaterials(vJONumber, vJOBOM, rs("OperOrder"))
                    vProcess += "</table></div>"

                    vProcess += "<div class='col-5'><table class='table table-bordered' style='font-size: 11px;'>" _
                        & "<thead> " _
                            & "<tr><td colspan='9'><b>RETURN</b></td></tr>" _
                            & "<tr>" _
                                & "<td>Item Code</td>" _
                                & "<td>Description</td>" _
                                & "<td>LOTNO</td>" _
                                & "<td>Roll No</td>" _
                                & "<td>QTY Release</td>" _
                                & "<td>Release By</td>" _
                                & "<td>QTY Receive</td>" _
                                & "<td>Receive By</td>" _
                                & "<td>Shift</td>" _
                            & "</tr>" _
                        & "</thead>"

                    'CollectAllReturn(vJONumber, vJOBOM, rs("OperOrder"))
                    vProcess += "</table></div></div>"

                    vProcess += "<div class='row'><div class='col-7'><table class='table table-bordered' style='font-size: 11px;'>" _
                        & "<thead> " _
                            & "<tr><td colspan='8'><b>OUTPUT - COMPLETION</b></td></tr>" _
                            & "<tr>" _
                                & "<td>Roll No.</td>" _
                                & "<td>Core Wt.</td>" _
                                & "<td>Net Wt.</td>" _
                                & "<td>Gross Wt.</td>" _
                                & "<td>Meters</td>" _
                                & "<td>Created By</td>" _
                                & "<td>Date Created</td>" _
                                & "<td>Shift</td>" _
                            & "</tr>" _
                        & "</thead>"

                    'CollectCompletion(vJONumber, vJOBOM, rs("OperOrder"), "COMPLETION")
                    vProcess += "</table></div>"




                    vProcess += "<div class='col-5'><table class='table table-bordered' style='font-size: 11px;'>" _
                        & "<thead> " _
                            & "<tr><td colspan='8'><b>OUTPUT - WASTE</b></td></tr>" _
                            & "<tr>" _
                                & "<td>Roll No.</td>" _
                                & "<td>Core Wt.</td>" _
                                & "<td>Net Wt.</td>" _
                                & "<td>Gross Wt.</td>" _
                                & "<td>Created By</td>" _
                                & "<td>Date Created</td>" _
                                & "<td>Shift</td>" _
                            & "</tr>" _
                        & "</thead>"

                    'CollectCompletion(vJONumber, vJOBOM, rs("OperOrder"), "WASTE")
                    vProcess += "</table></div></div>"

                End If


            Loop
            rs.Close()


        Catch ex As SqlClient.SqlException
            Response.Write("Error in SQL query Get Process Details function:  " & ex.Message)
        End Try

        c.Close()
        c.Dispose()
        cm.Dispose()

    End Sub

    Private Sub cmdJOList_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmdJOList.SelectedIndexChanged

        Dim c As New SqlClient.SqlConnection(connStr)
        Dim cm As New SqlClient.SqlCommand
        Dim rs As SqlClient.SqlDataReader

        Try
            c.Open()
        Catch ex As SqlClient.SqlException
            vScript = "alert('Error occurred while trying to connect to database. Error is: " _
                & ex.Message.Replace(vbCrLf, "").Replace("'", "") & "');"
            c.Dispose()
            cm.Dispose()
            Exit Sub
        End Try

        cm.Connection = c

        vSQL = "select BOM_Cd, BOMRev from jo_header b " _
            & "where JO_Status='RELEASE' and JobOrderNo='" & cmdJOList.SelectedValue & "' "

        cm.CommandText = vSQL

        Try
            rs = cm.ExecuteReader
            If rs.Read Then
                vJOBOM = rs(0)
                vJOBOMRev = rs(1)
            End If
            rs.Close()

        Catch ex As SqlClient.SqlException
            Response.Write("Error in SQL query Get BOM Details:  " & ex.Message)
        End Try

        vSQL = "select OperOrder, " _
            & "(select Descr from ref_item_process b where b.Proc_Cd=a.Proc_Cd) As ProcessDescr  " _
            & "from bom_process a " _
            & "where BOM_Cd='" & vJOBOM & "' and Revision=" & vJOBOMRev & " and Proc_Cd<>3002 "

        BuildCombo(vSQL, cmdProcessList)

        c.Close()
        c.Dispose()
        cm.Dispose()

        CollectJODetails(cmdJOList.SelectedValue)

    End Sub

    Private Sub BtnQSearch_ServerClick(sender As Object, e As EventArgs) Handles BtnQSearch.ServerClick

        Dim c As New SqlClient.SqlConnection(connStr)
        Dim cm As New SqlClient.SqlCommand
        Dim rs As SqlClient.SqlDataReader

        Try
            c.Open()
        Catch ex As SqlClient.SqlException
            vScript = "alert('Error occurred while trying to connect to database. Error is: " _
                & ex.Message.Replace(vbCrLf, "").Replace("'", "") & "');"
            c.Dispose()
            cm.Dispose()
            Exit Sub
        End Try

        cm.Connection = c

        vSQL = "select BOM_Cd, BOMRev from jo_header b " _
            & "where JO_Status='RELEASE' and JobOrderNo='" & TxtJONO.Text.Trim & "' "

        cm.CommandText = vSQL

        Try
            rs = cm.ExecuteReader
            If rs.Read Then
                vJOBOM = rs(0)
                vJOBOMRev = rs(1)
            End If
            rs.Close()

        Catch ex As SqlClient.SqlException
            Response.Write("Error in SQL query Get BOM Details:  " & ex.Message)
        End Try

        vSQL = "select OperOrder, " _
            & "(select Descr from ref_item_process b where b.Proc_Cd=a.Proc_Cd) As ProcessDescr  " _
            & "from bom_process a " _
            & "where BOM_Cd='" & vJOBOM & "' and Revision=" & vJOBOMRev & " and Proc_Cd<>3002 "

        BuildCombo(vSQL, cmdProcessList)

        c.Close()
        c.Dispose()
        cm.Dispose()

        CollectJODetails(TxtJONO.Text.Trim)
    End Sub

    Private Sub CollectJODetails(pJONO As String)
        Dim vDateDiff As Integer = 0
        Dim c As New SqlClient.SqlConnection(connStr)
        Dim cm As New SqlClient.SqlCommand
        Dim rs As SqlClient.SqlDataReader



        Session("StarDate") = ""
        Session("EndDate") = ""

        vSQL = "select top 1 DateCreated from prod_rawmaterial " _
            & "where JONO='" & pJONO & "' order by DateCreated asc"
        Session("TblRawmats") = GetRef(vSQL, "")


        vSQL = "select top 1 DateCreated from prod_rawmaterial " _
            & "where JONO='" & pJONO & "' order by DateCreated Desc"

        vSQL = "select top 1 DateCreated from prod_completion " _
            & "where JONO='" & pJONO & "' order by DateCreated Desc"
        Session("TblCompletion") = GetRef(vSQL, "")

        vSQL = "select top 1 DateCreated from prod_return_materials " _
            & "where JONO='" & pJONO & "' order by DateCreated Desc"
        Session("TblReturnMats") = GetRef(vSQL, "")

        If Session("TblRawmats") <> "" Then
            Session("StarDate") = Format(CDate(Session("TblRawmats")), "MM/dd/yyyy")
        End If

        If Session("TblRawmatsEnd") <> "" Then
            Session("EndDate") = Format(CDate(Session("TblRawmatsEnd")), "MM/dd/yyyy")
        End If

        If Session("TblCompletion") <> "" Then
            Session("EndDate") = Format(CDate(Session("TblCompletion")), "MM/dd/yyyy")
        End If

        If Session("TblReturnMats") <> "" Then
            Session("EndDate") = Format(CDate(Session("TblReturnMats")), "MM/dd/yyyy")
        End If


        If Session("StarDate").ToString = "" Then
            Exit Sub
        End If

        'Response.Write(Session("StarDate") & "<br>" & Session("EndDate"))
        vDateDiff = DateDiff(DateInterval.Day, CDate(Session("StarDate")), CDate(Session("EndDate")))
        'Response.Write(vDateDiff)

        Try
            c.Open()
        Catch ex As SqlClient.SqlException
            vScript = "alert('Error occurred while trying to connect to database. Error is: " _
                & ex.Message.Replace(vbCrLf, "").Replace("'", "") & "');"
            c.Dispose()
            cm.Dispose()
            Exit Sub
        End Try

        cm.Connection = c

        vSQL = "select BOM_Cd, BOMRev,b.Item_Cd," _
            & "(select Descr + ' ' Descr2 from item_master a where a.Item_Cd=b.Item_Cd) as ItemDescr, " _
            & "(select Descr from ref_item_customer where Cust_Cd=Customer_Cd) as CustomerName, Cust_Cd, " _
            & "JobOrderNo, Alt_Cd  " _
            & "from jo_header b where JO_Status='RELEASE' and JobOrderNo='" & pJONO & "' "

        'Response.Write(vSQL)

        cm.CommandText = vSQL

        Try
            rs = cm.ExecuteReader
            If rs.Read Then
                vJOBOM = rs(0)
                vJOBOMRev = rs(1)

                lblJONO.Text = rs("JobOrderNo")
                lblItemCode.Text = rs("Item_Cd") & " / " & rs("Alt_Cd")
                lblItemDescr.Text = rs("ItemDescr")
                lblCustomerDescr.Text = rs("Cust_Cd")
                lblCustomer.Text = rs("CustomerName")
            End If

            rs.Close()

        Catch ex As SqlClient.SqlException
            Response.Write("Error in SQL query Get BOM Details:  " & ex.Message)

        End Try
        c.Close()
        c.Dispose()
        cm.Dispose()

        For x As Integer = 0 To vDateDiff

            vProcess += "<div class='row'>"
            BuildReportPerShift(pJONO, CDate(Session("StarDate")).AddDays(x), "NS", x)
            BuildReturnReportPerShift(pJONO, CDate(Session("StarDate")).AddDays(x), "NS", x)
            vProcess += "</div>"


            vProcess += "<div class='row'>"
            CollectCompletion(pJONO, CDate(Session("StarDate")).AddDays(x), "NS", x, "COMPLETION")
            CollectCompletion(pJONO, CDate(Session("StarDate")).AddDays(x), "NS", x, "WASTE")
            vProcess += "</div>"


            vProcess += "<div class='row'>"
            BuildReportPerShift(pJONO, CDate(Session("StarDate")).AddDays(x), "DS", x)
            BuildReturnReportPerShift(pJONO, CDate(Session("StarDate")).AddDays(x), "DS", x)
            vProcess += "</div>"

            vProcess += "<div class='row'>"
            CollectCompletion(pJONO, CDate(Session("StarDate")).AddDays(x), "DS", x, "COMPLETION")
            CollectCompletion(pJONO, CDate(Session("StarDate")).AddDays(x), "DS", x, "WASTE")
            vProcess += "</div>"
        Next

    End Sub

    Private Sub BuildReportPerShift(pJONO As String, pStartDate As Date, pShitfType As String, pLoop As Integer)

        Dim c As New SqlClient.SqlConnection(connStr)
        Dim cm As New SqlClient.SqlCommand
        Dim rs As SqlClient.SqlDataReader
        Dim vJONumber As String = pJONO

        Dim DayType As String = ""

        Dim ShiftLabel As String = ""
        Dim DateRangeLabel As String = ""
        Dim DateRange As String = ""
        Dim vClass As String = ""


        If pShitfType = "NS" And pLoop = 0 Then
            DateRange = "and DateCreated between '" & pStartDate & " 00:00:00' and '" & pStartDate & " 05:59:59'"
            ShiftLabel = "Night Shift : "
            DateRangeLabel = Format(pStartDate, "MM/dd/yyyy") & " 00:00:00 to " & pStartDate & " 05:59:59"
            DayType = "N"
        Else
            DateRange = "and DateCreated between '" & pStartDate.AddDays(-1) & " 18:00:00' and '" & pStartDate & " 05:59:59'"
            ShiftLabel = "Night Shift : "
            DateRangeLabel = Format(pStartDate.AddDays(-1), "MM/dd/yyyy") & " 18:00:00 to " & pStartDate & " 05:59:59"
            DayType = "N"
        End If


        If pShitfType = "DS" Then
            DateRange = "and DateCreated between '" & pStartDate & " 06:00:00' and '" & pStartDate & " 17:59:59'"
            ShiftLabel = "Day Shift : "
            DateRangeLabel = Format(pStartDate, "MM/dd/yyyy") & " 06:00:00 to " & pStartDate & " 17:59:59"
            DayType = "D"

        End If

        'Response.Write(DateRange)

        Try
            c.Open()
        Catch ex As SqlClient.SqlException
            vScript = "alert('Error occurred while trying to connect to database. Error is: " _
                & ex.Message.Replace(vbCrLf, "").Replace("'", "") & "');"
            c.Dispose()
            cm.Dispose()
            Exit Sub
        End Try

        cm.Connection = c

        vProcess += "<div class='col-7'>" _
                    & "<table class='table table-bordered' style='font-size: 11px;'>" _
                        & "<thead> " _
                            & "<tr class='bg-primary text-white'><td colspan='1'><b>INPUT</b></td>" _
                            & "<td colspan='6'><h6><small>" & ShiftLabel & DateRangeLabel & "</small></h6></td>" _
                            & "<td colspan='1'></td></tr>" _
                            & "<tr>" _
                                & "<td>Item Code</td>" _
                                & "<td>Description</td>" _
                                & "<td>LOTNO</td>" _
                                & "<td>Roll No</td>" _
                                & "<td>QTY Release</td>" _
                                & "<td>Release By</td>" _
                                & "<td>QTY Receive</td>" _
                                & "<td>Receive By</td>" _
                            & "</tr>" _
                        & "</thead>"

        vSQL = "select distinct Item_Cd, LotNo " _
                & "from prod_rawmaterial where TranType='RW' and " _
                & "JONO='" & pJONO & "' and " _
                & "OperOrder=" & cmdProcessList.SelectedValue & " " & DateRange

        cm.CommandText = vSQL
        'Response.Write(vSQL)
        'Exit Sub

        rs = cm.ExecuteReader
        Do While rs.Read

            vSQL = "select TranId, Item_Cd, Qty, LotNo, RollNo, UOM, RQty, " _
                    & "(select (Descr + ' ' + Descr) as Descr from item_master " _
                        & "where item_master.Item_Cd=prod_rawmaterial.Item_Cd) as vDescr, " _
                    & "(select SFG_Descr from bom_process e where e.SFG_Cd=Item_Cd and e.BOM_Cd='" & vJOBOM & "') As SFGName, " _
                    & "CreatedBy, DateCreated, " _
                    & "(select FullName from user_list where user_id=CreatedBy) as vRel_Admin, " _
                    & "(select Emp_Fname + ' ' + Emp_LName from emp_master where Emp_Cd=ReceivedBy) as vRec_Operation, " _
                    & "ReceivedBy, DateReceived, RQty " _
                & "from prod_rawmaterial where TranType='RW' and " _
                & "JONO='" & pJONO & "' and " _
                & "Item_Cd='" & rs("Item_Cd") & "' and LotNo='" & rs("LotNo") & "' and " _
                & "OperOrder=" & cmdProcessList.SelectedValue & DateRange _
                & " order by Item_Cd, LotNo, vDescr, DateReceived "

            If vClass = "" Then
                vClass = "bg-light text-dark"
            Else
                vClass = ""
            End If

            BuildReportPerShiftDetails(vSQL, pStartDate, vClass)

        Loop
        rs.Close()

        vProcess += "<tr class='text-primary bold'>" _
                        & "<td class='text-right' colspan='4'><b>Total material per shift : </b></td>" _
                        & "<td><b>" & Session("TtlShiftQTYRel") & "</b></td>" _
                        & "<td></td>" _
                        & "<td><b>" & Session("TtlShiftQTYRec") & "</b></td>" _
                        & "<td colspan='2'></td></tr>"


        vProcess += "</table></div>"

        Session("TtlShiftQTYRel") = 0
        Session("TtlShiftQTYRec") = 0

        c.Close()
        c.Dispose()
        cm.Dispose()

    End Sub

    Private Sub BuildReportPerShiftDetails(pSQL As String, pStartDate As Date, pClass As String)


        Dim c As New SqlClient.SqlConnection(connStr)
        Dim cm As New SqlClient.SqlCommand
        Dim rs As SqlClient.SqlDataReader

        Dim DayType As String = ""
        Dim iCtr As Integer = 0

        Dim TtlShiftQTYRel As Decimal
        Dim TtlShiftQTYRec As Decimal

        Try
            c.Open()
        Catch ex As SqlClient.SqlException
            vScript = "alert('Error occurred while trying to connect to database. Error is: " _
                & ex.Message.Replace(vbCrLf, "").Replace("'", "") & "');"
            c.Dispose()
            cm.Dispose()
            Exit Sub
        End Try


        'GET RAW MATERIALS RELEASE FROM WAREHOUSE
        cm.Connection = c
        cm.CommandText = pSQL

        'Response.Write(pSQL & "<br><br>")
        Try
            rs = cm.ExecuteReader
            Do While rs.Read
                iCtr += 1

                Session("LotNum") = rs("LotNo")

                vProcess += "<tr class='" & pClass & "'>" &
                    "<td>" & rs("Item_Cd") & "</td>"

                If Not IsDBNull(rs("vDescr")) Then
                    vProcess += "<td>" & rs("vDescr") & "</td>"
                Else
                    vProcess += "<td>" & rs("SFGName") & "</td>"
                End If

                'CreatedBy, DateCreated
                vProcess += "<td>" & rs("LotNo") & "</td>"
                vProcess += "<td>" & rs("RollNo").ToString.ToUpper & "</td>"
                vProcess += "<td>" & rs("Qty") & "</td>"
                vProcess += "<td>" & rs("vRel_Admin") & "<br>" & Format(CDate(rs("DateCreated")), "MM/dd/yyyy HH:mm tt") & "</td>"

                vProcess += "<td>" & rs("RQty") & "</td>" _
                    & "<td>" & rs("vRec_Operation") & "<br>"

                'vProcess += "</td><td>" & DayType & "</td>"
                vProcess += "</tr>"

                TtlShiftQTYRel += rs("Qty")
                TtlShiftQTYRec += rs("RQty")
                Session("DayShift") = DayType

            Loop

            vProcess += "<tr class='text-info bold'>" _
                        & "<td class='text-right' colspan='4'><b>Total material per Item and lot number : </b></td>" _
                        & "<td><b>" & TtlShiftQTYRel & "</b></td>" _
                        & "<td></td>" _
                        & "<td><b>" & TtlShiftQTYRec & "</b></td>" _
                        & "<td colspan='2'></td></tr>"

                Session("TtlShiftQTYRel") += TtlShiftQTYRel
                Session("TtlShiftQTYRec") += TtlShiftQTYRec

            rs.Close()
        Catch ex As SqlClient.SqlException
            vScript = "alert('Error occurred while trying to buil shift reference. Error is: " &
                ex.Message.Replace(vbCrLf, "").Replace("'", "") & "');"
        End Try

        c.Close()
        c.Dispose()
        cm.Dispose()


    End Sub

    Private Sub BuildReturnReportPerShift(pJONO As String, pStartDate As Date, pShitfType As String, pLoop As Integer)

        Dim c As New SqlClient.SqlConnection(connStr)
        Dim cm As New SqlClient.SqlCommand
        Dim rs As SqlClient.SqlDataReader
        Dim vJONumber As String = pJONO

        Dim DayType As String = ""

        Dim ShiftLabel As String = ""
        Dim DateRangeLabel As String = ""
        Dim DateRange As String = ""
        Dim vClass As String = ""


        If pShitfType = "NS" And pLoop = 0 Then
            DateRange = "and DateCreated between '" & pStartDate & " 00:00:00' and '" & pStartDate & " 05:59:59'"
            ShiftLabel = "Night Shift : "
            DateRangeLabel = Format(pStartDate, "MM/dd/yyyy") & " 00:00:00 to " & pStartDate & " 05:59:59"
            DayType = "N"
        Else
            DateRange = "and DateCreated between '" & pStartDate.AddDays(-1) & " 18:00:00' and '" & pStartDate & " 05:59:59'"
            ShiftLabel = "Night Shift : "
            DateRangeLabel = Format(pStartDate.AddDays(-1), "MM/dd/yyyy") & " 18:00:00 to " & pStartDate & " 05:59:59"
            DayType = "N"
        End If


        If pShitfType = "DS" Then
            DateRange = "and DateCreated between '" & pStartDate & " 06:00:00' and '" & pStartDate & " 17:59:59'"
            ShiftLabel = "Day Shift : "
            DateRangeLabel = Format(pStartDate, "MM/dd/yyyy") & " 06:00:00 to " & pStartDate & " 17:59:59"
            DayType = "D"

        End If

        'Response.Write(DateRange)

        Try
            c.Open()
        Catch ex As SqlClient.SqlException
            vScript = "alert('Error occurred while trying to connect to database. Error is: " _
                & ex.Message.Replace(vbCrLf, "").Replace("'", "") & "');"
            c.Dispose()
            cm.Dispose()
            Exit Sub
        End Try

        cm.Connection = c

        vProcess += "<div class='col-5'>" _
                    & "<table class='table table-bordered' style='font-size: 11px;'>" _
                        & "<thead> " _
                            & "<tr class='bg-primary text-white'><td colspan='1'><b>RETURN</b></td>" _
                            & "<td colspan='6'><h6><small>" & ShiftLabel & DateRangeLabel & "</small></h6></td>" _
                            & "<td colspan='1'></td></tr>" _
                            & "<tr>" _
                                & "<td>Item Code</td>" _
                                & "<td>Description</td>" _
                                & "<td>LOTNO</td>" _
                                & "<td>Roll No</td>" _
                                & "<td>QTY Return</td>" _
                                & "<td>Release By</td>" _
                                & "<td>QTY Receive</td>" _
                                & "<td>Receive By</td>" _
                            & "</tr>" _
                        & "</thead>"

        vSQL = "select distinct Item_Cd, LotNo " _
                & "from prod_return_materials where " _
                & "JONO='" & pJONO & "' and " _
                & "OperOrder=20 " & DateRange

        'Response.Write(vSQL & "<br>")
        cm.CommandText = vSQL
        rs = cm.ExecuteReader
        Do While rs.Read


            vSQL = "select TranId, Item_Cd, LotNo, RollNo, UOM, QTY, CreatedBy, DateCreated, " _
                    & "(Select Emp_Fname +' '+ Emp_Lname from emp_master where a.CreatedBy=Emp_Cd) as vOpsName, " _
                    & "(select FullName from user_list where a.CreatedBy=User_Id) as vAdminName, " _
                    & "(Select Emp_Fname +' '+ Emp_Lname from emp_master where a.ReceivedBy=Emp_Cd) as vRecOpsName, " _
                    & "(select FullName from user_list where a.ReceivedBy=User_Id) as vRecAdminName, " _
                    & "(select (Descr + ' ' + Descr) as Descr from item_master " _
                        & "where item_master.Item_Cd=a.Item_Cd) as vDescr, " _
                    & "QtyReceived, DateReceived, ReceivedBy, " _
                    & "(select FullName from user_list where a.ReceivedBy=User_Id) as vRecName "

            vSQL += "from prod_return_materials a where " _
                & "JONO='" & pJONO & "' and " _
                & "Item_Cd='" & rs("Item_Cd") & "' and LotNo='" & rs("LotNo") & "' and " _
                & "OperOrder=20 " & DateRange _
                & " order by Item_Cd, LotNo, vDescr, DateReceived "

            If vClass = "" Then
                vClass = "bg-light text-dark"
            Else
                vClass = ""
            End If


            BuildReturnReportPerShiftDetails(vSQL, pStartDate, vClass)
            'Response.Write(vSQL & "<br>")

        Loop
        rs.Close()

        vProcess += "<tr class='text-primary bold'>" _
                        & "<td class='text-right' colspan='4'><b>Total material return per shift : </b></td>" _
                        & "<td><b>" & Session("TtlShiftQTYReturn") & "</b></td>" _
                        & "<td></td>" _
                        & "<td><b>" & Session("TtlShiftQTYReturnRec") & "</b></td>" _
                        & "<td colspan='1'></td></tr>"


        vProcess += "</table></div>"

        Session("TtlShiftQTYReturn") = 0
        Session("TtlShiftQTYReturnRec") = 0

        c.Close()
        c.Dispose()
        cm.Dispose()

    End Sub

    Private Sub BuildReturnReportPerShiftDetails(pSQL As String, pStartDate As Date, pClass As String)


        Dim c As New SqlClient.SqlConnection(connStr)
        Dim cm As New SqlClient.SqlCommand
        Dim rs As SqlClient.SqlDataReader

        Dim DayType As String = ""
        Dim iCtr As Integer = 0

        Dim TtlShiftQTYReturn As Decimal
        Dim TtlShiftQTYReturnRec As Decimal

        Try
            c.Open()
        Catch ex As SqlClient.SqlException
            vScript = "alert('Error occurred while trying to connect to database. Error is: " _
                & ex.Message.Replace(vbCrLf, "").Replace("'", "") & "');"
            c.Dispose()
            cm.Dispose()
            Exit Sub
        End Try


        cm.Connection = c
        cm.CommandText = pSQL

        'Response.Write(pSQL & "<br><br>")
        Try
            rs = cm.ExecuteReader
            Do While rs.Read
                iCtr += 1


                vProcess += "<tr>" _
                    & "<td>" & rs("Item_Cd") & "</td>" _
                    & "<td>" & rs("vDescr") & "</td>" _
                    & "<td>" & rs("LotNo") & "</td>" _
                    & "<td>" & rs("RollNo") & "</td>" _
                    & "<td>" & rs("QTY") & "</td>" _
                    & "<td>"


                'TotalQTYReturn += rs("QTY")

                If IsDBNull(rs("vOpsName")) Then
                    vProcess += rs("vAdminName")
                Else
                    vProcess += rs("vOpsName")
                End If

                vProcess += "<br>" & Format(CDate(rs("DateCreated")), "MM/dd/yyyy HH:mm") & "</td>"




                If Not IsDBNull(rs("DateReceived")) Then
                    vProcess += "<td>" & rs("Qty") & "</td>"
                Else
                    vProcess += "<td></td>"
                End If
                'vProcess += "<td>" & rs("QtyReceived") & "</td>"


                vProcess += "<td>" & rs("vRecName") & "<br>"


                If Not IsDBNull(rs("DateReceived")) Then
                    vProcess += Format(CDate(rs("DateReceived")), "MM/dd/yyyy HH:mm")
                    If Format(CDate(rs("DateReceived")), "HH") >= 18 Or Format(CDate(rs("DateReceived")), "HH") <= 6 Then
                        DayType = "N"
                    Else
                        DayType = "D"
                    End If

                End If

                vProcess += "</td> "

                vProcess += "</tr>"

                TtlShiftQTYReturn += rs("Qty")

                If Not IsDBNull(rs("DateReceived")) Then
                    TtlShiftQTYReturnRec += rs("Qty")
                End If


            Loop

            vProcess += "<tr class='text-info bold'>" _
                        & "<td class='text-right' colspan='4'><b>Total material returb per Item and lot number : </b></td>" _
                        & "<td><b>" & TtlShiftQTYReturn & "</b></td>" _
                        & "<td></td>" _
                        & "<td><b>" & TtlShiftQTYReturnRec & "</b></td>" _
                        & "<td colspan='1'></td></tr>"

            Session("TtlShiftQTYReturn") += TtlShiftQTYReturn
            Session("TtlShiftQTYReturnRec") += TtlShiftQTYReturnRec


            TtlShiftQTYReturn = 0
            TtlShiftQTYReturnRec = 0

            rs.Close()
        Catch ex As SqlClient.SqlException
            vScript = "alert('Error occurred while trying to buil shift reference. Error is: " &
                ex.Message.Replace(vbCrLf, "").Replace("'", "") & "');"
        End Try

        c.Close()
        c.Dispose()
        cm.Dispose()


    End Sub

    Private Sub CollectCompletion(pJONO As String, pStartDate As Date, pShitfType As String, pLoop As Integer, pType As String)
        'Dim vBOM As String = pBOM
        Dim vJO As String = pJONO
        Dim vOperOrder As String = cmdProcessList.SelectedValue

        Dim vNet As Decimal = 0
        Dim vGross As Decimal = 0
        Dim vCore As Decimal = 0

        Dim vTltComp As Decimal = 0
        Dim vTtlCompVoid As Decimal = 0
        Dim vCtr As Decimal = 0

        Dim vSQL As String = ""

        Dim vTllCore As Decimal = 0
        Dim vTllNet As Decimal = 0
        Dim vTllGross As Decimal = 0
        Dim vTllMeter As Decimal = 0
        Dim vTllQty As Decimal = 0
        Dim vTtlPcsBox As Decimal = 0
        Dim vDateEdited As String = ""
        Dim vEditedBy As String = ""
        Dim vDateVoid As String = ""
        Dim vVoidBy As String = ""
        Dim DayType As String = ""

        Dim c As New SqlClient.SqlConnection
        Dim cm As New SqlClient.SqlCommand
        Dim rs As SqlClient.SqlDataReader

        'Dim DayType As String = ""

        Dim ShiftLabel As String = ""
        Dim DateRangeLabel As String = ""
        Dim DateRange As String = ""

        c.ConnectionString = connStr
        Try
            c.Open()
        Catch ex As SqlClient.SqlException
            vScript = "alert('Error occurred while trying to connect to database. Error is: " _
                & ex.Message.Replace(vbCrLf, "").Replace("'", "") & "');"
            c.Dispose()
            cm.Dispose()
            Exit Sub
        End Try

        cm.Connection = c


        If pShitfType = "NS" And pLoop = 0 Then
            DateRange = "and DateCreated between '" & pStartDate & " 00:00:00' and '" & pStartDate & " 05:59:59'"
            ShiftLabel = "Night Shift : "
            DateRangeLabel = Format(pStartDate, "MM/dd/yyyy") & " 00:00:00 to " & pStartDate & " 05:59:59"
            DayType = "N"
        Else
            DateRange = "and DateCreated between '" & pStartDate.AddDays(-1) & " 18:00:00' and '" & pStartDate & " 05:59:59'"
            ShiftLabel = "Night Shift : "
            DateRangeLabel = Format(pStartDate.AddDays(-1), "MM/dd/yyyy") & " 18:00:00 to " & pStartDate & " 05:59:59"
            DayType = "N"
        End If


        If pShitfType = "DS" Then
            DateRange = "and DateCreated between '" & pStartDate & " 06:00:00' and '" & pStartDate & " 17:59:59'"
            ShiftLabel = "Day Shift : "
            DateRangeLabel = Format(pStartDate, "MM/dd/yyyy") & " 06:00:00 to " & pStartDate & " 17:59:59"
            DayType = "D"

        End If


        If pType = "COMPLETION" Then
            vProcess += "<div class='col-7'><table class='table table-bordered' style='font-size: 11px;'>" _
                        & "<thead> " _
                            & "<tr><td colspan='8'><b>OUTPUT - COMPLETION</b></td></tr>" _
                            & "<tr>" _
                                & "<td>Roll No.</td>" _
                                & "<td>Core Wt.</td>" _
                                & "<td>Net Wt.</td>" _
                                & "<td>Gross Wt.</td>" _
                                & "<td>Meters</td>" _
                                & "<td>Created By</td>" _
                                & "<td>Date Created</td>" _
                            & "</tr>" _
                        & "</thead>"
        Else
            vProcess += "<div class='col-5'><table class='table table-bordered' style='font-size: 11px;'>" _
                        & "<thead> " _
                            & "<tr><td colspan='8'><b>OUTPUT - WASTE</b></td></tr>" _
                            & "<tr>" _
                                & "<td>Roll No.</td>" _
                                & "<td>Core Wt.</td>" _
                                & "<td>Net Wt.</td>" _
                                & "<td>Gross Wt.</td>" _
                                & "<td>Created By</td>" _
                                & "<td>Date Created</td>" _
                            & "</tr>" _
                        & "</thead>"
        End If


        vSQL = "select TranId,Qty,JONO,BatchNo,PrevBatchNoA,PrevBatchNoB,ProdCost," _
            & "CoreWeight,NetWeight,GrossWeight,TranType,Meter,CreatedBy,DateCreated,Batchgroup+''+BatchgroupLine as BatGroup,BatchCtr," _
            & "(select Emp_Fname + ' ' + Emp_Lname from emp_master where CreatedBy=Emp_Cd) as vOps, " _
            & "(select Emp_Fname + ' ' + Emp_Lname from emp_master where EditedBy=Emp_Cd) as vEditBy,EditedBy, DateEdited, " _
            & "(select Emp_Fname + ' ' + Emp_Lname from emp_master where VoidBy=Emp_Cd) as vVoidBy, VoidBy, DateVoid, " _
            & "TtlPcs, TtlPcsBox " _
            & "from prod_completion where " _
            & "JONO='" & vJO & "' and " _
            & "TranType='" & pType & "' and DateVoid is null and " _
            & "OperOrder=" & cmdProcessList.SelectedValue & DateRange _
            & "order by TranType, DateCreated"

        'Response.Write(vSQL & "<br><br>")
        cm.CommandText = vSQL

        Try
            rs = cm.ExecuteReader
            Do While rs.Read

                vCore = IIf(IsDBNull(rs("CoreWeight")), 0, rs("CoreWeight"))
                vGross = IIf(IsDBNull(rs("GrossWeight")), 0, rs("GrossWeight"))

                vNet = vGross - vCore
                vTltComp += 1
                vCtr += 1

                vProcess += "<tr>"

                vProcess += "<td>"

                If Not IsDBNull(rs("BatGroup")) Then
                    vProcess += rs("BatGroup") & "-" & rs("BatchCtr")
                Else
                    vProcess += rs("BatchNo")
                End If


                vProcess += "</td><td>" & vCore & "</td>" _
                    & "<td>" & vNet & "</td>" _
                    & "<td>" & vGross & "</td>"



                If pType = "COMPLETION" Then
                    vProcess += "<td>" & rs("Meter") & "</td>"
                End If



                vProcess += "<td>" & rs("vOps") & "</td>"
                vProcess += "<td>" & Format(rs("DateCreated"), "MM/dd/yyyy HH:mm") & "</td>"

                'vProcess += "<td>"

                'If Format(CDate(rs("DateCreated")), "HH") >= 18 Or Format(CDate(rs("DateCreated")), "HH") <= 6 Then
                '    DayType = "N"
                'Else
                '    DayType = "D"
                'End If


                vProcess += "</tr>"

                vTllCore += vCore
                vTllNet += vNet
                vTllGross += vGross
                vTllMeter += rs("Meter")

            Loop

            rs.Close()

            vProcess += "<tr class='text-primary'>" _
                & "<td class='text-right' ><b>TOTAL :</b></td>" _
                & "<td><b>" & vTllCore & "</b></td>" _
                & "<td><b>" & vTllNet & "</b></td>" _
                & "<td><b>" & vTllGross & "</b></td>"

            If pType = "COMPLETION" Then
                vProcess += "<td><b>" & vTllMeter & "</b></td>"
                Session("TotalNet") = vTllNet
            End If

            vProcess += "<td colspan='3' class='text-right'>"
            If pType = "WASTE" And Session("TotalNet") > 0 Then
                ' vProcess += "<b>WASTE PERCENTAGE : " & Format(vTllNet / Session("TotalNet"), "##,###,##0.0000") & "%</b></td></tr>"
                Session("TotalNet") = 0
            End If


            vProcess += "</tr></table></div>"


            vTllCore = 0
            vTllNet = 0
            vTllGross = 0
            vTllMeter = 0

        Catch ex As SqlClient.SqlException
            Response.Write("Error in SQL query Get Process Details function:  " & ex.Message)
        End Try

        c.Close()
        c.Dispose()
        cm.Dispose()

    End Sub

    Private Sub btnScan_ServerClick(sender As Object, e As EventArgs) Handles btnSearchJoList.ServerClick

        Dim vDateDiff As Integer = 0

        vSQL = "select JobOrderNo, JobOrderNo from jo_header " _
                & "where JO_Status='RELEASE' and ReleaseDate between '" _
                & txtDateFrom.Text.Trim & "' and '" & txtDateTo.Text.Trim & "'"
        BuildCombo(vSQL, cmdJOList)

        cmdJOList.Items.Add("")
        cmdJOList.SelectedValue = ""

        Session("StarDate") = ""
        Session("EndDate") = ""

        vSQL = "select top 1 DateCreated from prod_rawmaterial " _
           & "where JONO='" & cmdJOList.SelectedValue & "' order by DateCreated asc"
        Session("TblRawmats") = GetRef(vSQL, "")

        vSQL = "select top 1 DateCreated from prod_rawmaterial " _
            & "where JONO='" & cmdJOList.SelectedValue & "' order by DateCreated Desc"

        vSQL = "select top 1 DateCreated from prod_completion " _
            & "where JONO='" & cmdJOList.SelectedValue & "' order by DateCreated Desc"
        Session("TblCompletion") = GetRef(vSQL, "")

        vSQL = "select top 1 DateCreated from prod_return_materials " _
            & "where JONO='" & cmdJOList.SelectedValue & "' order by DateCreated Desc"
        Session("TblReturnMats") = GetRef(vSQL, "")

        If Session("TblRawmats") <> "" Then
            Session("StarDate") = Format(CDate(Session("TblRawmats")), "MM/dd/yyyy")
        End If

        If Session("TblRawmatsEnd") <> "" Then
            Session("EndDate") = Format(CDate(Session("TblRawmatsEnd")), "MM/dd/yyyy")
        End If

        If Session("TblCompletion") <> "" Then
            Session("EndDate") = Format(CDate(Session("TblCompletion")), "MM/dd/yyyy")
        End If

        If Session("TblReturnMats") <> "" Then
            Session("EndDate") = Format(CDate(Session("TblReturnMats")), "MM/dd/yyyy")
        End If

        'Response.Write(Session("StarDate") & "<br>" & Session("EndDate"))
        'vDateDiff = DateDiff(DateInterval.Day, CDate(Session("StarDate")), CDate(Session("EndDate")))
        'Response.Write(vDateDiff)

        'CollectAllProcess()
    End Sub

    Private Sub cmdProcessList_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmdProcessList.SelectedIndexChanged
        'Session("SelectedProcess") = cmdProcessList.SelectedValue
        'CollectAllProcess()
        CollectJODetails(cmdJOList.SelectedValue)
        'Session("SelectedProcess") = ""
    End Sub

    Function GetMachineName(pOperationOrder As Integer, pJONO As String) As String
        Dim vMachineName As String = ""

        Dim c As New SqlClient.SqlConnection(connStr)
        Dim cm As New SqlClient.SqlCommand
        Dim rs As SqlClient.SqlDataReader



        Try
            c.Open()
        Catch ex As SqlClient.SqlException
            vScript = "alert('Error occurred while trying to connect to database. Error is: " _
                & ex.Message.Replace(vbCrLf, "").Replace("'", "") & "');"
            c.Dispose()
            cm.Dispose()


            Exit Function

        End Try

        cm.Connection = c


        vSQL = "select " _
            & "(select Descr from ref_item_machine where ref_item_machine.Mach_Cd=jo_machine.Mach_Cd) as MachineName  " _
            & "from jo_machine where  " _
            & "OperOrder=" & pOperationOrder & " and IsPrimary='YES' and " _
            & "JobOrderNo='" & pJONO & "'"

        'Response.Write(vSQL)

        cm.CommandText = vSQL

        Try
            rs = cm.ExecuteReader
            If rs.Read Then
                vMachineName = rs(0)
            End If
            rs.Close()

        Catch ex As SqlClient.SqlException

            Response.Write("Error in SQL query Get BOM Details:  " & ex.Message)
        End Try

        c.Close()
        c.Dispose()
        cm.Dispose()

        Return vMachineName

    End Function

End Class
