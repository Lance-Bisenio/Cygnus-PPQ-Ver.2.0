Imports denaro
Partial Class joborderIncidentLog
    Inherits System.Web.UI.Page
    Public vScript As String = ""
    Public vCompletionList As String = ""
    Public vSQL As String = ""
    Dim vSection As String
    Dim vProcess As String
    Dim vJONO As String
    Dim vSFG As String
    Dim vOperOrderNo As Integer


    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        'If Session("uid") = Nothing Or Session("uid") = "" Then
        '    vScript = "alert('Login session has expired. Please re-login again.'); " &
        '        "window.close();"

        '    '"window.opener.document.form1.h_Mode.value='reload'; " & _
        '    '"window.opener.document.form1.submit(); " & _
        '    Exit Sub
        'End If

        If Not IsPostBack Then

            vSection = Request.Item("pSection")
            vProcess = Request.Item("pProcess")
            vOperOrderNo = Request.Item("pOperNo")
            vJONO = Request.Item("pJO")


            vSQL = "select Mach_Cd, Descr from ref_item_machine where Sect_Cd='" & vSection & "' order by Descr"
            BuildCombo(vSQL, DdlMachine)


            vSQL = "select Mach_Cd, Mach_Cd from jo_machine where IsPrimary='YES' and JobOrderNo='" & vJONO & "' and Sect_Cd='" & vSection & "' and Proc_Cd='" & vProcess & "'"
            DdlMachine.SelectedValue = GetRef(vSQL, 0)


            vSQL = "select TranId, Descr from ref_prod_incidentReason where Section_Cd='" & vSection & "' and Proc_Cd='" & vProcess & "' order by Descr"
            BuildCombo(vSQL, DdlReason)

            txtDateFrom.Text = Format(Now, "MM/dd/yyyy")
            TxtTimeStop.Text = Format(Now, "hh:mm")



        End If

        BuildIncidentList()
    End Sub

    Private Sub BtnSave_Click(sender As Object, e As EventArgs) Handles BtnSave.Click
        'Section_Cd,Proc_Cd,JONO,OperOrder,ReasonDescription,RootCause,TimeStop,TimeStart,TimeIntervalMinutes,CreatedBy,DateCreated,EditedBy,DateEdited

        vSection = Request.Item("pSection")
        vProcess = Request.Item("pProcess")
        vOperOrderNo = Request.Item("pOperNo")
        vJONO = Request.Item("pJO")


        Dim vDataStop As Date
        Dim vDataStart As Date

        Dim vDataStopTemp As String = ""
        Dim vDataStartTemp As String = ""

        If TxtTimeStop.Text.Trim = "" Then
            lblMessage.Text = "Invalid Date Stop details.<br>Kindly check your date and time values."
            lblMessage.ForeColor = Color.Red
            TxtTimeStop.Text = ""
            Exit Sub
        End If


        vDataStopTemp = txtDateFrom.Text.Trim & " " & TxtTimeStop.Text.Trim & ":00 " & IIf(RdoTimeStopAM.Checked = True, "AM", "PM")

        If Not IsDate(vDataStopTemp) Then
            lblMessage.Text = "Invalid Date Stop details.<br>Kindly check your date and time values."
            lblMessage.ForeColor = Color.Red
            Exit Sub
        End If

        vDataStop = vDataStopTemp

        If txtDateStart.Text.Trim <> "" And TxtTimeStart.Text.Trim <> "" Then
            vDataStartTemp = txtDateStart.Text.Trim & " " & TxtTimeStart.Text.Trim & ":00 " & IIf(RdoTimeStartAM.Checked = True, "AM", "PM")
            vDataStart = vDataStartTemp
        End If


        vSQL = "insert into prod_incidentLogs " _
            & "(Section_Cd,Proc_Cd,JONO,OperOrder,ReasonDescription,RootCause," _
            & "IncidentDate,TimeStop,TimeStart,TimeIntervalMinutes,CreatedBy,DateCreated) values (" _
            & "'" & vSection & "','" & vProcess & "','" & vJONO & "','" & vOperOrderNo & "','" & DdlReason.SelectedValue & "','" & TxtRootCause.Text.Replace("'", "") & "'," _
            & "'" & Format(Now, "MM/dd/yyyy HH:mm:ss") & "','" & vDataStop & "'," _
            & IIf(vDataStartTemp = "", "NULL", "'" & vDataStart & "'") _
            & ",null,'" & Session("uid") & "','" & Format(Now, "MM/dd/yyyy HH:mm:ss") & "')"


        'Response.Write(vSQL)

        CreateRecord(vSQL)
        BuildIncidentList()

        TxtRootCause.Text = ""

    End Sub

    Private Sub BuildIncidentList()
        Dim c As New SqlClient.SqlConnection
        Dim da As SqlClient.SqlDataAdapter
        Dim ds As New DataSet
        Dim vFilter As String = ""
        Dim vTableName As String = ""
        Dim vSQL As String = ""

        c.ConnectionString = connStr
        vSQL = "select ReasonDescription,RootCause, TimeStop, TimeStart, IncidentDate, CreatedBy, DateCreated, " _
            & "(select Descr from ref_prod_incidentReason where TranId=ReasonDescription) as Descr " _
            & " from prod_incidentLogs"

        'lblMessageBox.Text = vSQL
        'Exit Sub

        da = New SqlClient.SqlDataAdapter(vSQL, c)
        da.Fill(ds, "Incident")
        tblIncidentList.DataSource = ds.Tables("Incident")
        tblIncidentList.DataBind()

        'lblTotal.Text = "<b>Total Item Retrieved : " & tbl_ItemMaster.DataSource.Rows.Count & "</b>"

        da.Dispose()
        ds.Dispose()
    End Sub

    'Private Sub tblIncidentList_SelectedIndexChanging(sender As Object, e As GridViewSelectEventArgs) Handles tblIncidentList.SelectedIndexChanging

    '    Response.Write("lance test")
    'End Sub

    'Private Sub getSFG_CompletionList()
    '    Dim vBOM As String = Request.Item("pBOM")
    '    Dim vRev As String = Request.Item("pBOMRev")
    '    Dim vJO As String = Request.Item("pJO")
    '    Dim vSect As String = Request.Item("pSection")
    '    Dim vProc As String = Request.Item("pProcess")
    '    Dim vSFG As String = Request.Item("pSFG")

    '    Dim vSQL As String = ""

    '    Dim c As New SqlClient.SqlConnection
    '    Dim cm As New SqlClient.SqlCommand
    '    Dim rs As SqlClient.SqlDataReader

    '    c.ConnectionString = connStr
    '    Try
    '        c.Open()
    '    Catch ex As SqlClient.SqlException
    '        vScript = "alert('Error occurred while trying to connect to database. Error is: " &
    '            ex.Message.Replace(vbCrLf, "").Replace("'", "") & "');"
    '        c.Dispose()
    '        cm.Dispose()
    '        Exit Sub
    '    End Try


    '    cm.Connection = c
    '    vSQL = "SELECT TranId, BatchNo FROM prod_disposal where " &
    '            "JONO='" & vJO & "' and " &
    '            "Sect_Cd='" & vSect & "' and " &
    '            "Proc_Cd='" & vProc & "' order by DateCreated  "

    '    cm.CommandText = vSQL
    '    Try
    '        rs = cm.ExecuteReader
    '        Do While rs.Read
    '            vCompletionList += "<tr onclick='ModifyCompletion(""" & rs("TranId") & """,""" &
    '                            rs("TranId") & """)'>" &
    '                "<td class='trLink'>" & rs("BatchNo") & "</td>" &
    '            "</tr>"

    '        Loop
    '        rs.Close()

    '    Catch ex As SqlClient.SqlException
    '        Response.Write("Error in SQL query Get Process Details function:  " & ex.Message)
    '    End Try

    '    c.Close()
    '    c.Dispose()
    '    cm.Dispose()

    'End Sub


    'Private Sub SaveAndUpdate_Process()
    '    Dim vBOM As String = Request.Item("pBOM")
    '    Dim vRev As String = Request.Item("pBOMRev")
    '    Dim vJO As String = Request.Item("pJO")
    '    Dim vSect As String = Request.Item("pSection")
    '    Dim vProc As String = Request.Item("pProcess")
    '    Dim vSFG As String = Request.Item("pSFG")
    '    Dim vOperOrder As String = Request.Item("pOperNo")
    '    Dim vSQL As String = ""


    '    If Session("uid") = "" Then
    '        vScript = "alert('Login session has expired. Please re-login again.');  window.close();"
    '        'window.opener.document.form1.h_Mode.value='reload'; window.opener.document.form1.submit();
    '        Exit Sub
    '    End If

    '    If txtWeight.Text.Trim = "" Then
    '        vScript = "alert('Please enter SFG Weight.');"
    '        Exit Sub
    '    End If

    '    Dim c As New SqlClient.SqlConnection
    '    Dim cm As New SqlClient.SqlCommand
    '    Dim vProdStatusCode As String = ""

    '    vProdStatusCode = GetRef("select ProdStatus from prod_disposal where " &
    '                                   "JONO='" & vJO & "' and " &
    '                                   "OperOrder='" & vOperOrder & "' and " &
    '                                   "SFG_Cd='" & lblSFGCd.Text & "' ", 0)

    '    c.ConnectionString = connStr
    '    Try
    '        c.Open()
    '    Catch ex As SqlClient.SqlException
    '        vScript = "alert('Error occurred while trying to connect to database. Error is: " &
    '            ex.Message.Replace(vbCrLf, "").Replace("'", "") & "');"
    '        c.Dispose()
    '        cm.Dispose()
    '        Exit Sub
    '    End Try
    '    cm.Connection = c

    '    If Request.Item("h_TranId") <> "" Then
    '        vSQL = "update prod_disposal set " &
    '                    "ItemWeight='" & txtWeight.Text.Trim & "'," &
    '                    "CreatedBy='" & Session("uid") & "'," &
    '                    "DateCreated='" & Now() & "' " &
    '                    "where TranId= " & Request.Item("h_TranId")

    '    Else
    '        vSQL = "Insert into prod_disposal " &
    '                    "(BOM,Revision,Sect_Cd,Proc_Cd,JONO,OperOrder,SFG_Cd,BatchNo,ItemWeight,Qty,Cost,Remarks,ProdStatus,CreatedBy,DateCreated) values (" &
    '                    vBOM & "," & vRev & ",'" & vSect & "','" & vProc & "','" & vJO & "'," & vOperOrder & ",'" & vSFG & "','" &
    '                    lblBatch.Text.Trim & "','" & txtWeight.Text.Trim & "',0,0,'PRODUCTION WASTE','" & vProdStatusCode & "', '" &
    '                    Session("uid") & "','" & Now() & "') "
    '    End If


    '    cm.CommandText = vSQL
    '    'Response.Write(vSQL)
    '    Try
    '        cm.ExecuteNonQuery()
    '    Catch ex As SqlClient.SqlException
    '        Response.Write("Error in SQL query insert/update records:  " & ex.Message)
    '    End Try

    '    c.Close()
    '    c.Dispose()
    '    cm.Dispose()

    '    h_TranId.Value = ""
    '    h_Mode.Value = ""

    '    vScript = "alert('Successfully saved.'); window.close();"

    'End Sub


    Protected Sub tblIncidentList_SelectedIndexChanged(sender As Object, e As EventArgs) Handles tblIncidentList.SelectedIndexChanged

        Dim vDateTimeStop As String = ""
        Dim vDateTimeStart As String = ""



        txtDateFrom.Text = ""
        TxtTimeStop.Text = ""
        txtDateStart.Text = ""
        TxtTimeStart.Text = ""

        DdlReason.SelectedValue = tblIncidentList.SelectedRow.Cells(2).Text
        TxtRootCause.Text = tblIncidentList.SelectedRow.Cells(4).Text

        txtDateFrom.Text = Format(CDate(tblIncidentList.SelectedRow.Cells(5).Text), "MM/dd/yyyy")
        TxtTimeStop.Text = Format(CDate(tblIncidentList.SelectedRow.Cells(5).Text), "hh:mm")
        vDateTimeStop = Format(CDate(tblIncidentList.SelectedRow.Cells(5).Text), "tt")

        If vDateTimeStop = "AM" Then
            RdoTimeStopAM.Checked = True
            RdoTimeStopPM.Checked = False
        Else
            RdoTimeStopAM.Checked = False
            RdoTimeStopPM.Checked = True
        End If

        If tblIncidentList.SelectedRow.Cells(6).Text.Trim.ToString <> "&nbsp;" Then
            txtDateStart.Text = Format(CDate(tblIncidentList.SelectedRow.Cells(6).Text), "MM/dd/yyyy")
            TxtTimeStart.Text = Format(CDate(tblIncidentList.SelectedRow.Cells(6).Text), "hh:mm")
            vDateTimeStart = Format(CDate(tblIncidentList.SelectedRow.Cells(6).Text), "tt")

            If vDateTimeStart = "AM" Then
                RdoTimeStartAM.Checked = True
                RdoTimeStartPM.Checked = False
            Else
                RdoTimeStartAM.Checked = False
                RdoTimeStartPM.Checked = True
            End If
        End If


    End Sub
End Class
