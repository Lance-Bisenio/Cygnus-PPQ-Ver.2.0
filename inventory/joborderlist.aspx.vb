Imports denaro
Partial Class joborderlist
    Inherits System.Web.UI.Page
    Dim vSQL As String
    Public vScript As String = "" 
    Public vData As String = ""
    Public vHeader As String = ""
    Public vDatesHeader As String = ""
    Public vRecordData As String = ""

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Session("uid") = "" Then
            Response.Redirect("http://" & SERVERIP & "/" & SITENAME & "/")
        End If

        If Not CanRun(Session("caption"), Request.Item("id")) Then
            Session("denied") = "1"
            Server.Transfer("../main.aspx")
            Exit Sub
        End If

        If Not IsPostBack Then
 
            Dim vLastDate As Date = Format(Now, "MM") & "/01/" & Format(Now, "yyyy")

            txtDateFrom.Text = Format(Now, "MM/01/yyyy")
            txtDateTo.Text = Format(vLastDate.AddMonths(1).AddDays(-1), "MM/dd/yyyy")

            'cmbYear
            'cmbYear.Items.Clear()
            'For iCtr = Year(Now) - 1 To Year(Now) + 3
            '    cmbYear.Items.Add(iCtr)
            'Next iCtr
            'cmbYear.SelectedValue = Year(Now)
            'cmbMonth.SelectedValue = Format(Now, "MM")

            BuildCombo("select Status_Cd, Descr from ref_item_status where GroupName='JO' order by Descr", cmbStatus)
            cmbStatus.Items.Add("All")
            cmbStatus.SelectedValue = "All"

            BuildCombo("select Customer_Cd, Descr from ref_item_customer order by Descr", cmbCustomer)
            cmbCustomer.Items.Add("All")
            cmbCustomer.SelectedValue = "All"

            BuildCombo("select Type_Cd, Descr from ref_item_type where Type_Cd in ('FG','SFG') order by Descr", cmbItemType)
            cmbItemType.Items.Add("All")
            cmbItemType.SelectedValue = "All"

            BuildCombo("select Section_Cd, Descr from ref_emp_section order by Descr", cmbSection)
            cmbSection.Items.Add("All")
            cmbSection.SelectedValue = "All"
             
            'BuildCombo("select Mach_Cd, Descr from ref_item_machine order by Descr", cmbMachine)
            cmbMachine.Items.Add("All")
            cmbMachine.SelectedValue = "All"

            DataRefresh()
        End If

    End Sub
    Function IsLastDay(ByVal myDate As Date) As Boolean
        Return myDate.Day = Date.DaysInMonth(myDate.Year, myDate.Month)
    End Function

    Private Sub DataRefresh()
        Dim c As New SqlClient.SqlConnection
        Dim da As SqlClient.SqlDataAdapter
        Dim ds As New DataSet
        Dim vFilter As String = ""
        Dim vTableName As String = ""
        Dim vSQL As String = ""
        Dim vDateFilter As Integer = 0


        c.ConnectionString = connStr

        If cmbStatus.SelectedValue = "All" Then
            vFilter = "and JO_Status in ('RELEASE','PLANNING','PRE-PLAN') "
        Else
            vFilter = "and JO_Status='" & cmbStatus.SelectedValue & "'"
        End If

        If cmbCustomer.SelectedValue <> "All" Then
            vFilter += "and Cust_Cd='" & cmbCustomer.SelectedValue & "'"
        End If

        If cmbItemType.SelectedValue <> "All" Then
            vFilter += "and ItemType_Cd='" & cmbItemType.SelectedValue & "'"
        End If

        If txtSearchBox.text.trim <> "" Then
            vFilter += "and JobOrderNo like '%" & txtSearchBox.text.trim & "%'"
            txtSearchBox.text = txtSearchBox.text.trim
        End If

        vDateFilter = txtDateFrom.Text.Trim.Length + txtDateTo.Text.Trim.Length

        If vDateFilter > 0 Then
            vFilter += " and DueDate between '" & txtDateFrom.Text.Trim & "' and '" & txtDateTo.Text.Trim & "'"
        End If

        If cmbMachine.SelectedValue <> "All" Then

            If txtMachList.Text.Trim = "" Then
                vFilter += " and exists (select Mach_Cd from jo_machine where IsPrimary='YES' and jo_machine.JobOrderNo=jo_header.JobOrderNo) "
            Else
                vFilter += " and exists (select Mach_Cd from jo_machine where Mach_Cd in ('" & cmbMachine.SelectedValue & "') and IsPrimary='YES' and jo_machine.JobOrderNo=jo_header.JobOrderNo) "
            End If

        Else
            If txtMachList.Text.Trim <> "" Then
                vFilter += " and exists (select Mach_Cd from jo_machine where Mach_Cd in (" & txtMachList.Text.Trim & ") and IsPrimary='YES' and jo_machine.JobOrderNo=jo_header.JobOrderNo) "
            End If
        End If

        vSQL = "select TranId,Alt_Cd,Item_Cd,JobOrderNo,SalesOrderNo,JO_Status," _
                & "BOM_Cd,BOMRev,DueDate,StartDate,ReleaseDate," _
                & "day(DueDate) as vDueDays, " _
                & "day(StartDate) as vStartDate, " _
                & "day(ReleaseDate) as vReleDate," _
                & "DATEPART(hour,StartDateTime) as vStartHrs," _
                & "(select Descr from ref_item_uom where ref_item_uom.Uom_Cd=jo_header.Uom_Cd) as vUOM," _
                & "(select Descr from item_master where item_master.Item_Cd=jo_header.item_Cd) as vItemName," _
                & "(select StdQty from bom_header where bom_header.BOM_Cd=jo_header.BOM_Cd and bom_header.Revision=jo_header.BOMRev) as vBomQty, OrderQty " _
                & "from jo_header where Item_Cd is not null " & vFilter & " " _
                & "order by StartDate, vStartHrs "

        'Response.Write(vSQL) 'Item Code|Item Name|JONO.|Qty Order|JO Status|Release Date|Start Date|Due Date

        da = New SqlClient.SqlDataAdapter(vSQL, c)

        da.Fill(ds, "ItemMaster")
        tbl_JOList.DataSource = ds.Tables("ItemMaster")
        tbl_JOList.DataBind()
        lblTotalDocs.Text = "<b>BOM Header Retrieved : " & tbl_JOList.DataSource.Rows.Count & "</b>"

        da.Dispose()
        ds.Dispose()


    End Sub
    Function GetProdStatus(pJO As String, pBOM As Integer, pRev As Integer) As String
        Dim vVal As String = ""

        vSQL = "select Count(BOM_Cd) from bom_process where BOM_Cd=" & pBOM & " and Revision=" & pRev
        vVal = GetRef(vSQL, 0)

        vSQL = "select Count(Remarks) from prod_monitoring where Remarks='COMPLETED' and JobOrderNo='" & pJO & "'"
        vVal += "/" & GetRef(vSQL, 0)

        Return vVal

    End Function
    Protected Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        tbl_JOList.SelectedIndex = -1
        DataRefresh()
        'tbl_JOProcess.DataSource = Nothing

    End Sub

    Protected Sub tbl_JOList_SelectedIndexChanged(sender As Object, e As EventArgs) Handles tbl_JOList.SelectedIndexChanged
        Get_JOProcess()
    End Sub

    Protected Sub tbl_JOList_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles tbl_JOList.PageIndexChanging
        tbl_JOList.PageIndex = e.NewPageIndex
        DataRefresh()
    End Sub

    Private Sub Get_JOProcess()

        Dim vFilter As String = ""
        Dim vJONO As String = tbl_JOList.SelectedRow.Cells(4).Text
        Dim vBOM As String = tbl_JOList.SelectedRow.Cells(2).Text
        Dim vBOMRev As String = tbl_JOList.SelectedRow.Cells(3).Text
        Dim vTranId As String = tbl_JOList.SelectedRow.Cells(15).Text

        h_BOM.Value = vBOM
        h_BOMRev.Value = vBOMRev
        h_TranId.Value = vTranId

        Dim c As New SqlClient.SqlConnection(connStr)
        Dim cm As New SqlClient.SqlCommand
        Dim rs As SqlClient.SqlDataReader

        Dim cmSub As New SqlClient.SqlCommand
        Dim rsSub As SqlClient.SqlDataReader

        Dim iCtr As Integer = 1
        Dim vMeter As String = "0.00"
        Dim vKilos As String = "0.00"

        Dim vPrdMeter As String = "0.00"
        Dim vPrdKilos As String = "0.00"
        Dim vColor As String = ""
        Dim vClass As String = "btn btn-primary btn-xs disabled"
        Try
            c.Open()
        Catch ex As SqlClient.SqlException
            vScript = "alert('Error occurred while trying to connect to database. Error code 101; Error is: " & ex.Message.Replace(vbCrLf, "").Replace("'", "") & "');"
            c.Dispose()
            cm.Dispose()
            Exit Sub
        End Try

        cm.Connection = c
        cmSub.Connection = c

        vSQL = "select TranId, OperOrder, SFG_Cd,SFG_Descr, Sect_Cd,Proc_Cd, " _
                & "(select Descr +' '+Descr1 from item_master where SFG_Cd=Item_Cd ) as SFGName, " _
                & "(select Descr from ref_emp_section where Section_Cd=Sect_Cd) as vSection," _
                & "(select Descr from ref_item_process where ref_item_process.Proc_Cd=bom_process.Proc_Cd and ref_item_process.Sect_Cd=bom_process.Sect_Cd) as vProcess," _
                & "(select Remarks from prod_monitoring where SFG_Cd=SFG_Item_Cd and JobOrderNo='" & vJONO & "') as vProdStatus " _
                & "from bom_process where BOM_Cd='" & vBOM & "' and Revision='" & vBOMRev & "' " _
                & "Order by OperOrder"

        '& "(select (select Descr from ref_item_status where Status_Cd=ProdStatus) as StatusDesc from prod_monitoring where SFG_Cd=SFG_Item_Cd and JobOrderNo='" & vJONO & "') as vProdStatus " _
        'Response.Write(vSQL)

        cm.CommandText = vSQL

        Try
            rs = cm.ExecuteReader
            Do While rs.Read
                ' ========================================================================
                ' JOB ORDER REQUIREMENTS KILOS AND METER
                ' ========================================================================
                vSQL = "select Meter, Kilos from jo_machine " _
                    & "where JobOrderNo='" & vJONO & "' and " _
                    & "SFG_Item_Cd='" & rs("SFG_Cd") & "' and " _
                    & "Sect_Cd='" & rs("Sect_Cd") & "' and " _
                    & "Proc_Cd ='" & rs("Proc_Cd") & "' and " _
                    & "IsPrimary='YES' "

                cmSub.CommandText = vSQL
                rsSub = cmSub.ExecuteReader
                Do While rsSub.Read
                    vMeter = IIf(IsDBNull(rsSub("Meter")), "0.00", rsSub("Meter"))
                    vKilos = IIf(IsDBNull(rsSub("Kilos")), "0.00", rsSub("Kilos"))
                Loop
                rsSub.Close()

                ' ========================================================================
                ' PRODUCTION COMPLETED KILOS AND METER
                ' ========================================================================
                vFilter = "where JONO='" & vJONO & "' and " _
                    & "SFG_Cd='" & rs("SFG_Cd") & "' and " _
                    & "Sect_Cd='" & rs("Sect_Cd") & "' and " _
                    & "Proc_Cd ='" & rs("Proc_Cd") & "' and TranType='COMPLETION' "

                vSQL = "select SUM(NetWeight) as PrdKilos from prod_completion " & vFilter
                cmSub.CommandText = vSQL
                rsSub = cmSub.ExecuteReader
                If rsSub.Read Then
                    vPrdKilos = IIf(IsDBNull(rsSub("PrdKilos")), "0.00", rsSub("PrdKilos"))
                End If
                rsSub.Close()

                vSQL = "select SUM(Meter) as PrdMeter from prod_completion " & vFilter
                cmSub.CommandText = vSQL
                rsSub = cmSub.ExecuteReader
                If rsSub.Read Then
                    vPrdMeter = IIf(IsDBNull(rsSub("PrdMeter")), "0.00", rsSub("PrdMeter"))
                End If
                rsSub.Close()
                ' ========================================================================
                vPrdKilos = IIf(vPrdKilos = "null", "0.00", vPrdKilos)
                vPrdMeter = IIf(vPrdMeter = "null", "0.00", vPrdMeter)

                If vPrdKilos = "0.00" And vPrdMeter = "0.00" Then
                    vClass = "btn btn-primary btn-xs disabled"
                Else
                    vClass = "btn btn-primary btn-xs"
                End If



                vRecordData += "<tr>" _
                    & "<td><input type='button' class='" & vClass & "' value='View' " _
                    & " onclick='view(""" & vJONO & """,""" & rs("SFG_Cd") & """)'></td>" _
                    & "<td>" & iCtr & "</td>" _
                    & "<td>" & rs("OperOrder") & "</td>" _
                    & "<td>" & rs("vSection") & "</td>" _
                    & "<td>" & rs("vProcess") & "</td>" _
                    & "<td>" & rs("SFG_Cd") & "</td>" _
                    & "<td>" & rs("SFG_Descr") & "</td>" _
                    & "<td class='text-right'>" & IIf(vKilos = "null", "0.00", vKilos) & "</td>" _
                    & "<td class='text-right'>" & IIf(vMeter = "null", "0.00", vMeter) & "</td>" _
                    & "<td class='text-right'>" & vPrdKilos & "</td>" _
                    & "<td class='text-right'>" & vPrdMeter & "</td>"


                If Not IsDBNull(rs("vProdStatus")) Then
                    Select Case rs("vProdStatus")
                        Case "COMPLETED"
                            vColor = "vColorG"
                        Case "PRODUCTION RUN"
                            vColor = "vColorB"
                    End Select
                End If

                vRecordData += "<td class='" & vColor & "'>" & rs("vProdStatus") & "</td>"
                vRecordData += "</tr>"

                vColor = ""
                vKilos = "0.00"
                vMeter = "0.00"
                vPrdKilos = "0.00"
                vPrdMeter = "0.00"
                iCtr += 1
                Loop
                rs.Close()

        Catch ex As SqlClient.SqlException
            vScript = "alert('Error occurred while trying to retrieve Job Order Info. " _
                & "Error code 102; Error Is: " _
                & ex.Message.Replace(vbCrLf, "").Replace("'", "") & "');"
        End Try

        c.Close()
        c.Dispose()
        cm.Dispose()
        cmSub.Dispose()
    End Sub


End Class

'Private Sub Get_JOProcess()
'    Dim c As New SqlClient.SqlConnection
'    Dim da As SqlClient.SqlDataAdapter
'    Dim ds As New DataSet
'    Dim vFilter As String = ""
'    Dim vJONO As String = tbl_JOList.SelectedRow.Cells(4).Text
'    Dim vBOM As String = tbl_JOList.SelectedRow.Cells(2).Text
'    Dim vBOMRev As String = tbl_JOList.SelectedRow.Cells(3).Text
'    Dim vTranId As String = tbl_JOList.SelectedRow.Cells(15).Text
'    Dim vSQL As String = ""

'    h_BOM.Value = vBOM
'    h_BOMRev.Value = vBOMRev
'    h_TranId.Value = vTranId

'    c.ConnectionString = connStr

'    vSQL = "select TranId, OperOrder, SFG_Cd,SFG_Descr, Sect_Cd,Proc_Cd, " _
'            & "(select Descr +' '+Descr1 from item_master where SFG_Cd=Item_Cd ) as SFGName, " _
'            & "(select Descr from ref_emp_section where Section_Cd=Sect_Cd) as vSection," _
'            & "(select Descr from ref_item_process where ref_item_process.Proc_Cd=bom_process.Proc_Cd and ref_item_process.Sect_Cd=bom_process.Sect_Cd) as vProcess," _
'            & "(select (select Descr from ref_item_status where Status_Cd=ProdStatus) as StatusDesc from prod_monitoring where SFG_Cd=SFG_Item_Cd and JobOrderNo='" & vJONO & "') as vProdStatus " _
'            & "from bom_process where BOM_Cd='" & vBOM & "' and Revision='" & vBOMRev & "'"

'    'Response.Write(vSQL)

'    da = New SqlClient.SqlDataAdapter(vSQL, c)

'    da.Fill(ds, "JOProcessList")
'    tbl_JOProcess.DataSource = ds.Tables("JOProcessList")
'    tbl_JOProcess.DataBind()
'    'lblTotalDocs.Text = "<b>BOM Header Retrieved : " & tbl_JOList.DataSource.Rows.Count & "</b>"

'    da.Dispose()
'    ds.Dispose()
'End Sub
'Protected Sub cmbSection_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbSection.SelectedIndexChanged
'    Dim vFilter As String = ""

'    If cmbSection.SelectedValue <> "All" Then
'        vFilter = "where Sect_Cd='" & cmbSection.SelectedValue & "'"
'    End If

'    BuildCombo("select Mach_Cd, Descr from ref_item_machine " & vFilter & " order by Descr", cmbMachine)
'    cmbMachine.Items.Add("All")
'    cmbMachine.SelectedValue = "All"

'    Dim c As New SqlClient.SqlConnection(connStr)
'    Dim cm As New SqlClient.SqlCommand
'    Dim rs As SqlClient.SqlDataReader
'    Dim vSQL As String = ""
'    c.ConnectionString = connStr
'    Try
'        c.Open()
'    Catch ex As SqlClient.SqlException
'        vScript = "alert('Error occurred while trying to connect to database. Error code 101; Error is: " & ex.Message.Replace(vbCrLf, "").Replace("'", "") & "');"
'        c.Dispose()
'        cm.Dispose()
'        Exit Sub
'    End Try


'    txtMachList.Text = ""

'    cm.Connection = c
'    vSQL = "Select Mach_Cd, Descr from ref_item_machine where Sect_Cd='" & cmbSection.SelectedValue & "'"
'    cm.CommandText = vSQL
'    Try
'        rs = cm.ExecuteReader
'        Do While rs.Read
'            txtMachList.Text += "'" & rs("Mach_Cd") & "',"
'        Loop


'        rs.Close()

'        If txtMachList.Text.Trim <> "" Then
'            txtMachList.Text = Mid(txtMachList.Text, 1, Len(txtMachList.Text) - 1)
'        End If



'    Catch ex As SqlClient.SqlException
'        vScript = "alert('Error occurred while trying to retrieve Budget Info. Error code 102; Error is: " & _
'                ex.Message.Replace(vbCrLf, "").Replace("'", "") & "');"
'        c.Close()
'        c.Dispose()
'        cm.Dispose()
'        Exit Sub
'    End Try
'    DataRefresh()

'End Sub

'Private Sub DataRefresh()
'    Dim c As New SqlClient.SqlConnection(connStr)
'    Dim cm As New SqlClient.SqlCommand
'    Dim rs As SqlClient.SqlDataReader

'    Dim cm_sub As New SqlClient.SqlCommand
'    Dim rs_sub As SqlClient.SqlDataReader

'    Dim vSQL As String = ""

'    Dim vStyle As String = ""
'    Dim vDueDate As String = ""
'    Dim vReleaseDate As String = ""
'    Dim vStartDate As String = ""

'    Dim vGenRelDate As String = ""
'    Dim vFilter As String = ""
'    Dim vLineCtr As Integer = 0
'    Dim vTooltip As String = ""
'    Dim vClass As String = "class='_odd'"
'    'Dim vTempDate As Date = CDate(cmbMonth.SelectedValue & "/01/" & cmbYear.SelectedValue)
'    'Dim vLastDate As String = vTempDate.AddMonths(1).AddDays(-1)
'    'Dim vDayInMonth As Integer = Format(CDate(vLastDate), "dd")
'    Dim iFrom As Date
'    Dim iTo As Date
'    Dim vDateInterval As Integer
'    Dim vWidth As Integer

'    Dim vBGWorkdays As String = "ffffff"
'    Dim vBGFreedays As String = "ffffff"

'    Dim vTtlHrs As Decimal = 0
'    Dim vHrs As Decimal = 0
'    Dim vMins As Decimal = 0

'    Dim vBOMQty As Decimal = 0
'    Dim vJOQty As Decimal = 100
'    Dim vWorkDay As Integer
'    Dim vDayCtr As Integer
'    Dim vTD_Id As String

'    Dim vTD_TranID As String = ""
'    Dim vTD_TranID_Data As String = ""


'    iFrom = Format(CDate(txtDateFrom.Text), "MM-dd-yyyy")
'    iTo = Format(CDate(txtDateTo.Text), "MM-dd-yyyy")
'    vDateInterval = DateDiff(DateInterval.Day, iFrom, iTo) + 1

'    'vWidth = (420 * vDateInterval) + 800

'    'Response.Write(vWidth)
'    vDatesHeader = "<div style='width:100%; overflow:auto; border-collapse:collapse;'>" & _
'        "<table style='width:" & vWidth & "px; border-collapse:collapse; border-color: #eeeded; border:solid 1px #eeeded;' border='1'  > "

'    Try
'        c.Open()
'    Catch ex As SqlClient.SqlException
'        vScript = "alert('Error occurred while trying to connect to database. Error code 101; Error is: " & ex.Message.Replace(vbCrLf, "").Replace("'", "") & "');"
'        c.Dispose()
'        cm.Dispose()
'        cm_sub.Dispose()
'        Exit Sub
'    End Try

'    cm.Connection = c
'    cm_sub.Connection = c

'    '' ===================================================================================================================================================
'    '' BUILD THE TABLE HEADERS MONTHS AND DAYS COLUMN 
'    '' ===================================================================================================================================================  
'    vDatesHeader += "<tr><td colspan='13'></td>"
'    'For iCtr = 1 To vDateInterval
'    '    vDatesHeader += "<td style='font-size:11px; text-align:Left; padding-left: 10px;' colspan='24'>" & Format(iFrom.AddDays(iCtr - 1), "MMM dd") & "</td>"
'    'Next iCtr
'    vDatesHeader += "</tr>"

'    vHeader = "<tr><td style='width:40px;'></td>" & _
'        "<td style='width:30px;'></td>" & _
'        "<td style='width:80px;' class='titleBar'>Item Code</td>" & _
'        "<td style='width:500px;' class='titleBar'>Item Name</td>" & _
'        "<td style='width:80px;' class='titleBar'>JONO.</td>" & _
'        "<td style='width:80px;' class='titleBar'>Qty Order</td>" & _
'        "<td style='width:60px;' class='titleBar'>JO Status</td>" & _
'        "<td style='width:80px;' class='titleBar'>Release Date</td>" & _
'        "<td style='width:80px;' class='titleBar'>Start Date</td>" & _
'        "<td style='width:80px;' class='titleBar'>Due Date</td>"

'    'vHeader += _
'    '    "<td style='width:80px;' class='titleBar'>Order No</td>" & _
'    '    "<td style='width:160px;' class='titleBar'>Section Desc</td>" & _
'    '    "<td style='width:300px;' class='titleBar'>Machine</td>"

'    'For iCtr = 1 To vDateInterval
'    '    For i = 0 To 23
'    '        vHeader += "<td style='width:15px; font-size:9px; text-align:center;' >" & Format(i, "00") & "</td>"
'    '    Next i
'    'Next
'    vHeader += "</tr>" 
'    '' ===================================================================================================================================================


'    '' ===================================================================================================================================================
'    '' FILTERS COLLECTION
'    '' =================================================================================================================================================== 
'    If cmbStatus.SelectedValue <> "All" Then
'        vFilter = "and JO_Status='" & cmbStatus.SelectedValue & "'"
'    End If

'    If cmbCustomer.SelectedValue <> "All" Then
'        vFilter += "and Cust_Cd='" & cmbCustomer.SelectedValue & "'"
'    End If

'    If cmbItemType.SelectedValue <> "All" Then
'        vFilter += "and ItemType_Cd='" & cmbItemType.SelectedValue & "'"
'    End If

'    If cmbStatus.SelectedValue <> "PRE-PLAN" Then
'        vFilter += " and StartDate between '" & txtDateFrom.Text.Trim & "' and '" & txtDateTo.Text.Trim & "'"
'    End If

'    If cmbMachine.SelectedValue <> "All" Then

'        If txtMachList.Text.Trim = "" Then
'            vFilter += " and exists (select Mach_Cd from jo_machine where IsPrimary='YES' and jo_machine.JobOrderNo=jo_header.JobOrderNo) "
'        Else
'            vFilter += " and exists (select Mach_Cd from jo_machine where Mach_Cd in ('" & cmbMachine.SelectedValue & "') and IsPrimary='YES' and jo_machine.JobOrderNo=jo_header.JobOrderNo) "
'        End If

'    Else
'        If txtMachList.Text.Trim <> "" Then
'            vFilter += " and exists (select Mach_Cd from jo_machine where Mach_Cd in (" & txtMachList.Text.Trim & ") and IsPrimary='YES' and jo_machine.JobOrderNo=jo_header.JobOrderNo) "
'        End If
'    End If



'    '' ===================================================================================================================================================
'    '' GET ALL JOB ORDER UNDER OJ_HEADER TABLE AND BUILD TO MASTER TABLE AS DATA
'    '' ===================================================================================================================================================
'    vSQL = "select TranId,Item_Cd,JobOrderNo,JO_Status,BOM_Cd,BOMrev,DueDate,StartDate,ReleaseDate," & _
'        "day(DueDate) as vDueDays, " & _
'        "day(StartDate) as vStartDate, " & _
'        "day(ReleaseDate) as vReleDate," & _
'        "DATEPART(hour,StartDateTime) as vStartHrs," & _
'        "(select Descr from item_master where item_master.Item_Cd=jo_header.item_Cd) as vItemName," & _
'        "(select StdQty from bom_header where bom_header.BOM_Cd=jo_header.BOM_Cd and bom_header.Revision=jo_header.BOMRev) as vBomQty, OrderQty " & _
'        "from jo_header where Item_Cd is not null " & vFilter & " " & _
'            "order by StartDate, vStartHrs  "

'    'Response.Write(vSQL)
'    cm.CommandText = vSQL

'    Try
'        rs = cm.ExecuteReader
'        Do While rs.Read

'            vLineCtr += 1

'            ' --------------------------------------------------------------------------------------------------------------------------------
'            ' TOOLTIP SETTING
'            ' --------------------------------------------------------------------------------------------------------------------------------
'            If Not IsDBNull(rs("ReleaseDate")) Then
'                vTooltip += "<b>Release Date : </b>" & Format(rs("ReleaseDate"), "MM/dd/yyyy") & "<br/>"
'            End If
'            If Not IsDBNull(rs("StartDate")) Then
'                vTooltip += "<b>Start Date : </b>" & Format(rs("StartDate"), "MM/dd/yyyy") & "<br/>"
'            End If
'            If Not IsDBNull(rs("DueDate")) Then
'                vTooltip += "<b>Due Date : </b>" & Format(rs("DueDate"), "MM/dd/yyyy") & "<br/>"
'            End If

'            ' --------------------------------------------------------------------------------------------------------------------------------
'            ' DUE DATE, START DATE AND REALESE DATE VALIDATION IF NULL OR NOT 
'            ' --------------------------------------------------------------------------------------------------------------------------------
'            If Not IsDBNull(rs("DueDate")) Then
'                vDueDate = Format(rs("DueDate"), "MM/dd/yyyy")
'            End If
'            If Not IsDBNull(rs("StartDate")) Then
'                vStartDate = Format(rs("StartDate"), "MM/dd/yyyy")
'            End If
'            If Not IsDBNull(rs("ReleaseDate")) Then
'                vReleaseDate = Format(rs("ReleaseDate"), "MM/dd/yyyy")
'            End If
'            ' --------------------------------------------------------------------------------------------------------------------------------

'            vJOQty = IIf(IsDBNull(rs("OrderQty")), 0, rs("OrderQty"))


'            vData += "<tr " & vClass & "><td id='Tr_a" & rs("TranId") & "'>" & _
'                "<input type='button' id='" & rs("TranId") & "' value='select' class='GridButton' onclick='Modify_JO(" & rs("TranId") & "," & rs("BOM_Cd") & "," & rs("BOMrev") & ")'></td>" & _
'                "<td id='Tr_b" & rs("TranId") & "' " & vClass & ">&nbsp;" & vLineCtr & "</td>" & _
'                "<td id='Tr_c" & rs("TranId") & "' " & vClass & ">" & rs("Item_Cd") & "</td>" & _
'                "<td id='Tr_d" & rs("TranId") & "' " & vClass & ">" & rs("vItemName") & "</td>" & _
'                "<td id='Tr_e" & rs("TranId") & "' " & vClass & ">" & rs("JobOrderNo") & "</td>" & _
'                "<td id='Tr_f" & rs("TranId") & "' " & vClass & " style='text-align:right'>" & rs("OrderQty") & "&nbsp;</td>" & _
'                "<td id='Tr_g" & rs("TranId") & "' " & vClass & ">" & rs("JO_Status") & "</td>" & _
'                "<td id='Tr_h" & rs("TranId") & "' " & vClass & " style='text-align:center'>" & vReleaseDate & "</td>" & _
'                "<td id='Tr_i" & rs("TranId") & "' " & vClass & " style='text-align:center'>" & vStartDate & "</td>" & _
'                "<td id='Tr_j" & rs("TranId") & "' " & vClass & " style='text-align:center'>" & vDueDate & "</td>"

'            vData += "</tr>"

'            vBGWorkdays = ""
'            vTooltip = ""
'            vFilter = ""

'            If cmbSection.SelectedValue <> "All" Then
'                vFilter += "Sect_Cd='" & cmbSection.SelectedValue & "' and "
'            End If

'            If cmbMachine.SelectedValue <> "All" Then
'                vFilter += "Mach_Cd in ('" & cmbMachine.SelectedValue & "') and"
'            Else
'                vFilter += ""
'            End If




'            '' --------------------------------------------------------------------------------------------------------------------------------
'            '' BUILD THE LIST OF PROCESS
'            '' --------------------------------------------------------------------------------------------------------------------------------
'            'vSQL = "select TranId,OperOrder,Sect_Cd,Proc_Cd,Mach_Cd,StartDate,DATEPART(hour,StartDate) as StartDate_Hrs," & _
'            '    "(select Descr from ref_emp_section where ref_emp_section.Section_Cd=jo_machine.Sect_Cd) vSectDesc," & _
'            '    "(select Descr from ref_item_process where ref_item_process.Proc_Cd=jo_machine.Proc_Cd) vProcDesc," & _
'            '    "(select Descr from ref_item_machine where ref_item_machine.Mach_Cd=jo_machine.Mach_Cd) vMachDesc," & _
'            '    "(select sum(ProdRun_Hrs) from bom_process where SFG_Cd=SFG_Item_Cd and bom_process.BOM_Cd=bom_process.BOM_Cd and bom_process.Revision=jo_machine.Revision) as vTtlHrs," & _
'            '    "(select sum(ProdRun_Mins) from bom_process where SFG_Cd=SFG_Item_Cd and bom_process.BOM_Cd=bom_process.BOM_Cd and bom_process.Revision=jo_machine.Revision) as vTtlMins " & _
'            '    "from jo_machine where " & vFilter & " IsPrimary='YES' and JobOrderNo='" & rs("JobOrderNo") & "' "

'            ''Response.Write(vSQL & "<br>")
'            'cm_sub.CommandText = vSQL

'            'rs_sub = cm_sub.ExecuteReader
'            'Do While rs_sub.Read
'            '    ' --------------------------------------------------------------------------------------------------------------------------------
'            '    ' WORK COMPUTATION : HOURS FROM BOM * QTY ORDER
'            '    ' --------------------------------------------------------------------------------------------------------------------------------
'            '    vHrs = IIf(IsDBNull(rs_sub("vTtlHrs")), 0, rs_sub("vTtlHrs"))
'            '    vMins = IIf(IsDBNull(rs_sub("vTtlMins")), 0, rs_sub("vTtlMins"))


'            '    vTtlHrs = (vMins / 60) + vHrs           ' TotalHours = Number of Minutes / 60 + Numbers of hours
'            '    vTtlHrs = (vTtlHrs / 100) * vJOQty      ' TotalHours = TotalHours / 100 * FG QTY 
'            '    ' --------------------------------------------------------------------------------------------------------------------------------



'            '    vData += "<tr>"
'            '    vData += "<td colspan='10'></td>"
'            '    vData += "<td class='labelC'>" & rs_sub("OperOrder") & "</td>"
'            '    vData += "<td class='labelL'>" & rs_sub("vSectDesc") & "</td>"
'            '    vData += "<td class='labelL'>" & rs_sub("vProcDesc") & "</td>"
'            '    'vData += "<td class='labelL'>" & rs_sub("vMachDesc") & " - " & vTtlHrs & "</td>"






'            '    'For iCtr = 1 To vDateInterval
'            '    '    For i = 0 To 23

'            '    '        'vWorkDay = Format(iFrom.AddDays(iCtr - 1), "dd")
'            '    '        'If Not IsDBNull(rs("vStartDate")) Then
'            '    '        '    If rs("vStartDate") <= vWorkDay And rs("vStartHrs") <= i Then
'            '    '        '        ' PRODUCTION DAYS TIME HEME
'            '    '        '        vBGWorkdays = "0ea462;"
'            '    '        '        vDayCtr += 1
'            '    '        '    Else
'            '    '        '        vBGWorkdays = ""
'            '    '        '        If vDayCtr > 0 Then
'            '    '        '            ' PRODUCTION DAYS TIME HEME
'            '    '        '            vBGWorkdays = "0ea462;"
'            '    '        '            
'            '    '        '        Else
'            '    '        '            vBGWorkdays = "ffffff;"
'            '    '        '        End If
'            '    '        '    End If

'            '    '        '    If vDayCtr > vTtlHrs Then
'            '    '        '        vBGWorkdays = "ffffff"
'            '    '        '    End If
'            '    '        'End If

'            '    '        'If cmbStatus.SelectedValue = "PRE-PLAN" Then
'            '    '        '    vBGWorkdays = "ffffff"
'            '    '        'End If


'            '    '        vTD_Id = Format(CDate(txtDateFrom.Text).AddDays(iCtr - 1), "MM/dd/yyyy")
'            '    '        vTD_TranID = vTD_Id & "_" & Format(i, "00")

'            '    '        If Not IsDBNull(rs_sub("StartDate")) Then
'            '    '            vTD_TranID_Data = Format(rs_sub("StartDate"), "MM/dd/yyyy") & "_" & Format(rs_sub("StartDate_Hrs"), "00")
'            '    '        End If



'            '    '        If vTD_TranID = vTD_TranID_Data Then
'            '    '            vBGWorkdays = "0ea462;"
'            '    '            vDayCtr = 1
'            '    '        Else
'            '    '            If vDayCtr > 1 And vDayCtr <= vTtlHrs Then
'            '    '                vBGWorkdays = "0ea462;"
'            '    '            Else
'            '    '                vBGWorkdays = "ffffff;"
'            '    '            End If 
'            '    '        End If




'            '    '        vTtlHrs = Mid(vTtlHrs, 1, Len(vTtlHrs) - 1)
'            '    '        vData += "<td id='" & vTD_TranID & "' onclick='ResetDate(""" & vTD_TranID & """,""" & i & """,""" & vTtlHrs & """,""" & rs_sub("TranId") & """);' " & _
'            '    '            "style='width:15px; font-size:9px; text-align:center; cursor:pointer;' title='" & vTD_Id & "'>" & _
'            '    '            "<hr style='width:90%; border:3px solid; color:#" & vBGWorkdays & ";' title='" & vTooltip & "'>"
'            '    '        vData += "</td>"

'            '    '        If vDayCtr >= 1 Then
'            '    '            vDayCtr += 1
'            '    '        End If

'            '    '        'vTooltip
'            '    '    Next i

'            '    'Next
'            '    vDayCtr = 0
'            '    vTtlHrs = 0

'            '    vData += "</tr>"
'            'Loop
'            'rs_sub.Close()


'            'If vClass = "class='_odd'" Then
'            '    vClass = "class='_even'"
'            'Else
'            vClass = "class='_odd'"
'            'End If
'            vDayCtr = 0
'        Loop
'        lblTotalDocs.Text = "Job Order Retrieved : " & vLineCtr
'        vScript = ""

'        rs.Close()
'    Catch ex As SqlClient.SqlException
'        vScript = "alert('Error occurred while trying to retrieve Job Order Info. Error code 102; Error is: " & _
'            ex.Message.Replace(vbCrLf, "").Replace("'", "") & "');"

'        c.Close()
'        c.Dispose()
'        cm.Dispose()
'        Exit Sub
'    Finally
'        c.Close()
'        c.Dispose()
'        cm.Dispose()
'    End Try

'    vData += "</table>"

'End Sub