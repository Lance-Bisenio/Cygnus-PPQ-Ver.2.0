Imports denaro.fis
Partial Class eventviewer
    Inherits System.Web.UI.Page
    Public vDetail As String = ""
    Dim vClass As String = "odd"
    Public vScript As String = ""
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("uid") = "" Then
            Server.Transfer("index.aspx")
            Exit Sub
        End If
        If Not CanRun(Session("caption"), Request.Item("id")) Then
            Session("denied") = "1"
            Server.Transfer("main.aspx")
            Exit Sub
        End If
        If Not IsPostBack Then
            lblCaption.Text = "Event Viewer"

            BuildCombo("select DISTINCT Module as vLModule, Module from audit order by Module", cmbModule)
            BuildCombo("select DISTINCT Event as vLEvent, Event from audit order by Event", cmbEvent)

            cmbModule.Items.Add("All")
            cmbModule.SelectedValue = "All"

            cmbEvent.Items.Add("All")
            cmbEvent.SelectedValue = "All"

            txtTo.Text = Now
            txtFrom.Text = Now
        End If
    End Sub
    Private Sub DataRefresh()
        Dim vFilter As String = ""

        If txtUser.Text.Trim <> "" Then
            vFilter = " where User_Id='" & txtUser.Text & "' "
        End If

        If txtFrom.Text.Trim <> "" Then
            If vFilter.Trim = "" Then
                vFilter = " where TranDate between '" & Format(CDate(txtFrom.Text), "yyyy/MM/dd") & _
                    " 00:00:00' and '" & Format(CDate(txtTo.Text), "yyyy/MM/dd") & " 23:59:59'"
            Else
                vFilter += " and TranDate between '" & Format(CDate(txtFrom.Text), "yyyy/MM/dd") & _
                    " 00:00:00' and '" & Format(CDate(txtTo.Text), "yyyy/MM/dd") & " 23:59:59'"
            End If
        End If

        If cmbModule.SelectedValue <> "All" Then
            vFilter += " and Module='" & cmbModule.SelectedValue & "' "
        End If

        If cmbEvent.SelectedValue <> "All" Then
            vFilter += " and Event='" & cmbEvent.SelectedValue & "' "
        End If

        Dim c As New SqlClient.SqlConnection(connStr)
        Dim da As New SqlClient.SqlDataAdapter("select * from audit " & vFilter & " order by TranDate desc,User_Id", c)
        Dim ds As New DataSet

        Try
            da.Fill(ds, "audit")
            tblLog.DataSource = ds.Tables("audit")
            tblLog.DataBind()
            c.Dispose()
            da.Dispose()
            ds.Dispose()
        Catch ex As Exception
            Response.Write(da.SelectCommand.CommandText)
        End Try

    End Sub
    Protected Sub tblLog_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles tblLog.PageIndexChanging
        tblLog.PageIndex = e.NewPageIndex
        DataRefresh()
    End Sub


    Protected Sub tblLog_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tblLog.SelectedIndexChanged
        Dim c As New sqlclient.sqlconnection(connStr)
        Dim cm As New sqlclient.sqlcommand("select OldValues,NewValues from audit where TranDate='" & _
            tblLog.SelectedRow.Cells(0).Text & "' and User_Id='" & _
            tblLog.SelectedRow.Cells(2).Text & "' and MachineId='" & _
            tblLog.SelectedRow.Cells(1).Text & "'", c)
        Dim rs As sqlclient.sqldatareader
        Dim vOldVal As String = ""
        Dim vNewVal As String = ""
        Dim vFields() As String
        Dim vNewFields() As String
        Dim vContents() As String
        Dim vNewContents() As String
        Dim vNewValue As String
        Dim vColor As String
        Dim iCtr As Integer
        Dim iLoop As Integer

        vClass = "odd"
        c.Open()
        rs = cm.ExecuteReader
        If rs.Read Then
            vOldVal = rs("OldValues")
            vNewVal = rs("NewValues")
        End If
        rs.Close()
        vDetail = ""
        If vOldVal <> "" And vNewVal <> "" Then
            vFields = vOldVal.Split("|")
            vNewFields = vNewVal.Split("|")
            For iCtr = 0 To UBound(vFields)
                vContents = vFields(iCtr).Split("=")
                vNewValue = ""
                For iLoop = 0 To UBound(vNewFields)
                    vNewContents = vNewFields(iLoop).Split("=")
                    If vContents(0) = vNewContents(0) Then
                        vNewValue = vNewContents(1)
                        Exit For
                    End If
                Next iLoop
                vColor = "black"
                If vContents(1) <> vNewValue Then
                    vColor = "red"
                End If
                vDetail += "<tr style='color:" & vColor & ";' class='" & vClass & "'><td align='left' valign='top'>" & vContents(0) & _
                    "</td><td align='left' valign='top'>" & vContents(1) & "</td><td align='left' valign='top'>" & vNewValue & "</td></tr>"
                If vClass = "odd" Then
                    vClass = "even"
                Else
                    vClass = "odd"
                End If
            Next iCtr
        End If
    End Sub

    Protected Sub cmdClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdClose.Click
        Server.Transfer("main.aspx")
    End Sub

    Protected Sub cmdRefresh_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdRefresh.Click
        If txtFrom.Text.Trim <> "" And Not IsDate(txtFrom.Text) Then
            vScript = "alert('You must enter a valid Date in Date From field.');"
            Exit Sub
        End If
        If txtTo.Text.Trim <> "" And Not IsDate(txtTo.Text) Then
            vScript = "alert('You must enter a valid Date in Date To field.');"
            Exit Sub
        End If
        If (txtTo.Text.Trim <> "" And txtFrom.Text.Trim = "") Or _
            (txtTo.Text.Trim = "" And txtFrom.Text.Trim <> "") Then
            vScript = "alert('You must fill in both fields if you want to filter the records by date.');"
            Exit Sub
        End If
        DataRefresh()
    End Sub
End Class
