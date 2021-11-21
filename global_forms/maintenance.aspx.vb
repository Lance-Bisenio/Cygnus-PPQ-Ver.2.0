Imports denaro
Partial Class maintenance
    Inherits System.Web.UI.Page
    Public vScript As String = ""
    Dim vClass As String = "odd"

    Dim vRef As String
    Dim vSearchKey As String
    Dim vSearchType As String
    Dim vReturnVal As String
    Dim vSql As String
    Dim vTableName As String
    Dim vDataType As String
    Dim vSortBy As String
    Dim vColFormat As String
    Dim vColTitle As String
    Dim vColFields As String
    Dim vReportName As String
    Dim vFields() As String
    Dim vTitle() As String
    Dim vTypes() As String
    Dim vColor As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'If Session("uid") = "" Then
        '    Server.Transfer("index.aspx")
        '    Exit Sub
        'End If

        If Request.Item("pMode") = "NewWindow" Then
            If Session("uid") = "" Then
                vScript = "alert('Login session has expired. Please re-login again.'); window.opener.document.form1.submit(); window.close();"
                Exit Sub
            End If
        Else
            If Session("uid") = "" Or Session("uid") = Nothing Then
                vScript = "alert('Your login session has expired. Please re-login again.'); " 'window.close();"
                Response.Redirect("http://" & SERVERIP & "/" & SITENAME & "/")
                Exit Sub
            End If
        End If
         
        If Not CanRun(Session("caption"), Request.Item("id")) Then
            Session("denied") = "1"
            Server.Transfer("main.aspx")
            Exit Sub
        End If

        Dim c As New SqlClient.SqlConnection(connStr)
        c.Open()
        Dim vSQL As String = "select SubFile,SeqId,Label_Caption,SearchKey,SearchType,Return_Val,SqlCmd,SortBy," & _
                "ColFormat,ColTitle,ColFields,Dependencies,RunProg from menu_rights where SystemName='" & SYSTEMNAME & "' and Menu_Caption='" & Request.Item("id") & "'"

        Dim cm As New SqlClient.SqlCommand(vSQL, c)
        Dim vCoaType As String = ""

        'Response.Write("select SeqId,Label_Caption,SearchKey,SearchType,Return_Val,SqlCmd,SortBy," & _
        '        "ColFormat,ColTitle,ColFields,Dependencies,RunProg from menu_rights where SystemName='" & SYSTEMNAME & _
        '        "' and Menu_Caption='" & Request.Item("id") & "'")

        Dim rs As SqlClient.SqlDataReader
        rs = cm.ExecuteReader
        If Not rs.Read Then
            vScript = "alert('Module cannot be found. Please contact your administrator.');"
            rs.Close()
            GoTo skip
        End If
        vCoaType = rs("Label_Caption")
        'Response.Write(rs("Label_Caption"))
        'lblCaption.Text = "Codeset Maintenance for " & rs("Label_Caption")

        vSearchKey = IIf(IsDBNull(rs("SearchKey")), "", rs("SearchKey"))
        vSearchType = IIf(IsDBNull(rs("SearchType")), "", rs("SearchType"))
        vReturnVal = IIf(IsDBNull(rs("Return_Val")), "", rs("Return_Val"))
        vSQL = IIf(IsDBNull(rs("SqlCmd")), "", rs("SqlCmd"))
        vTableName = IIf(IsDBNull(rs("Dependencies")), "", rs("Dependencies"))
        vSortBy = IIf(IsDBNull(rs("SortBy")), "", rs("SortBy"))
        vColFormat = IIf(IsDBNull(rs("ColFormat")), "", rs("ColFormat"))
        vColTitle = IIf(IsDBNull(rs("ColTitle")), "", rs("ColTitle"))
        vColFields = IIf(IsDBNull(rs("ColFields")), "", rs("ColFields"))
        vDataType = IIf(IsDBNull(rs("RunProg")), "", rs("RunProg"))
        vReportName = IIf(IsDBNull(rs("SeqId")), "", rs("SeqId"))
        txtSubFile.Value = IIf(IsDBNull(rs("SubFile")), "", rs("SubFile"))


skip:
        c.Close()
        cm.Dispose()
        If Not IsPostBack Then

            If vCoaType.Trim = "Chart of Accounts Reference [Can View]" Then
                vTranType.Visible = True
            End If

            cmbShow.Items.Clear()
            For iCtr = 1 To 10
                cmbShow.Items.Add(10 * iCtr)
            Next iCtr

            BuildCombo("select CoaType_Cd, Descr from coa_type order by Descr", cmbAcctType)


            cmbAcctType.Items.Add("All")
            cmbAcctType.SelectedValue = "All"
            lblScrId.Value = Request.Item("id")
            DataRefresh()
        Else
            DataRefresh()
        End If
    End Sub
    Protected Sub cmdReturn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdReturn.Click
        If Request.Item("pMode") = "NewWindow" Then
            vScript = "window.opener.document.form1.submit(); window.close();"
        Else
            Server.Transfer("main.aspx")
        End If

    End Sub
    Private Sub DataRefresh()
        Dim c As New SqlClient.SqlConnection(connStr)
        Dim da As SqlClient.SqlDataAdapter
        Dim ds As New DataSet
        Dim vFilter As String = " "
        Dim vFields() As String = vColFields.Split("|")
        Dim vFieldName() As String = vColTitle.Split("|")
        Dim vSQLCmd As String = ""

        If txtSearch.Text.Trim() <> "" Then
            vFilter = " where " & vSearchKey & " like '" & _
                txtSearch.Text & "%' "
        End If

        If cmbAcctType.SelectedValue <> "All" Then
            If vFilter = " " Then
                vFilter += " where TranType='" & cmbAcctType.SelectedValue & "' "
            Else
                vFilter += " and TranType='" & cmbAcctType.SelectedValue & "' "
            End If
        End If

        For i As Integer = 0 To UBound(vFields)
            vSQLCmd += vFields(i) & " as [" & vFieldName(i) & "],"
        Next
        If vSQLCmd <> "" Then
            vSQLCmd = Mid(vSQLCmd, 1, Len(vSQLCmd) - 1)
            Try
                da = New SqlClient.SqlDataAdapter("select " & vSQLCmd & " from " & _
                    vTableName & vFilter & vSortBy, c)

                'Response.Write("select " & vSQLCmd & " from " & _
                '    vTableName & vFilter & vSortBy)

                da.Fill(ds, "maintenance")
                tblMaintenance.DataSource = ds.Tables("maintenance")
                tblMaintenance.DataBind()

                lblTotalDocs.Text = "<b>Total Records Retrieved : " & tblMaintenance.DataSource.Rows.Count & "</b>"

                da.Dispose()
                ds.Dispose()
                c.Dispose()
            Catch ex As SqlClient.SqlException
                vScript = "alert(""" & ex.Message.Replace(vbCrLf, "").Replace("'", "") & """);"
            End Try
        Else
            vScript = "alert(""Please check the database. No column list is defined. Seek Administrator's assistance."");"
        End If
    End Sub

    Protected Sub tblMaintenance_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles tblMaintenance.PageIndexChanging
        tblMaintenance.PageIndex = e.NewPageIndex
        DataRefresh()
    End Sub

    Protected Sub tblMaintenance_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles tblMaintenance.RowCreated
        'e.Row.CssClass = vClass
        'If vClass = "odd" Then
        '    vClass = "even"
        'Else
        '    vClass = "odd"
        'End If
    End Sub

    Protected Sub cmdDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdDelete.Click
        If tblMaintenance.SelectedIndex >= 0 Then

            Dim c As New sqlclient.sqlconnection(connStr)
            Dim cm As New sqlclient.sqlcommand

            cm.Connection = c
            cm.CommandText = "delete from " & vTableName & " where " & vReturnVal & "='" & _
                tblMaintenance.SelectedRow.Cells(2).Text & "'"
            Try
                c.Open()
                cm.ExecuteNonQuery()
                c.Close()

                If Request.Item("pMode") = "NewWindow" Then
                    vScript = "alert('Record successfully deleted.'); window.opener.location.reload(true);"
                Else
                    vScript = "alert('Record successfully deleted.');"
                End If

                DataRefresh()
            Catch ex As DataException
                vScript = "alert('An error occurred while trying to delete the selected record. ');"
            End Try
            c.Dispose()
            cm.Dispose()

            

        Else
            vScript = "alert('Please select item first.');"
        End If
        tblMaintenance.SelectedIndex = -1
    End Sub

    Protected Sub tblMaintenance_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tblMaintenance.SelectedIndexChanged
        txtvalue.Value = tblMaintenance.SelectedRow.Cells(2).Text.Replace("&amp;", "&")

        Dim c As New SqlClient.SqlConnection(connStr)
        Dim cm As New SqlClient.SqlCommand("select Frozen from " & vTableName & " where " & vReturnVal & "='" & txtvalue.Value & "'", c)
        Dim rs As SqlClient.SqlDataReader
        Dim vFrozen As Boolean = False

        Try
            c.Open()
            rs = cm.ExecuteReader
            If rs.Read Then
                vFrozen = rs("Frozen") = 1
            End If
            rs.Close()
        Catch ex As SqlClient.SqlException
        Finally
            c.Close()
            cm.Dispose()
            c.Dispose()
        End Try
        cmdDelete.Enabled = Not vFrozen
        'cmdEdit.Disabled = vFrozen
    End Sub

    Protected Sub cmdSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        DataRefresh()
    End Sub

    'Protected Sub cmdA_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdA.Click, _
    '    cmdB.Click, cmdC.Click, cmdD.Click, cmdE.Click, cmdF.Click, cmdG.Click, cmdH.Click, cmdI.Click, cmdJ.Click, _
    '    cmdK.Click, cmdL.Click, cmdM.Click, cmdN.Click, cmdO.Click, cmdP.Click, cmdQ.Click, cmdR.Click, cmdS.Click, _
    '    cmdT.Click, cmdU.Click, cmdV.Click, cmdW.Click, cmdX.Click, cmdY.Click, cmdZ.Click
    '    txtSearch.Text = CType(sender, WebControls.LinkButton).Text
    '    DataRefresh()
    'End Sub

  
    Protected Sub cmbShow_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbShow.SelectedIndexChanged
        tblMaintenance.PageSize = cmbShow.SelectedValue
        DataRefresh()
    End Sub
End Class
