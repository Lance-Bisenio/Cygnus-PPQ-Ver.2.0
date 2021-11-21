Imports denaro
Partial Class inkmanagement
    Inherits System.Web.UI.Page

    Dim vSQL As String
    Public vScript As String = ""
    Public vColHeader As String = ""
    Public vFilter As String = ""

    Dim c As New SqlClient.SqlConnection
    Dim vColNames As String = ""
    Dim vColSource As String = ""
    Dim vTableSource As String = ""

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        If Session("uid") = "" Then
            Response.Redirect("http://" & SERVERIP & "/" & SITENAME & "/")
        End If

        If Not CanRun(Session("caption"), Request.Item("id")) Then
            Session("denied") = "1"
            Server.Transfer("../main.aspx")
            Exit Sub
        End If

        Dim vLastDate As Date = Format(Now.AddMonths(1), "MM/01/yyyy")
        If Not IsPostBack Then

            cmbSearchBy.Items.Add("Job Order")
            cmbSearchBy.Items.Add("GCAS")
            cmbSearchBy.Items.Add("Item Code")
            cmbSearchBy.Items.Add(" ")
            cmbSearchBy.SelectedValue = " "


            BuildCombo("select Status_Cd, Descr from ref_item_status where GroupName='JO' order by Descr", cmbStatus)
            cmbStatus.SelectedValue = "RELEASE"
            cmbStatus.Enabled = False

            BuildCombo("select Status_Cd, Descr from ref_item_status where GroupName='JO_PROD' order by Descr", cmbViewType)
            'cmbViewType.Items.Add("All")
            'cmbViewType.SelectedValue = "All"

            BuildCombo("select Customer_Cd, Descr from ref_item_customer order by Descr", cmbCustomer)
            cmbCustomer.Items.Add("All")
            cmbCustomer.SelectedValue = "All"

            BuildCombo("select Type_Cd, Descr from ref_item_type where Type_Cd in ('FG','SFG') order by Descr", cmbItemType)
            cmbItemType.Items.Add("All")
            cmbItemType.SelectedValue = "All"

            GetTable_Properties()
            DataRefresh("Reload")
        End If

        If h_Mode.value = "Reload" Then
            GetTable_Properties()
            DataRefresh("Reload")
        End If

        If Session("pMode") = "FromTaskDetails" Then
            'cmbMonth.SelectedValue = Session("pMonth")
            DataRefresh("Reload")
            Session.Remove("pMode")
        End If


        txtDateFrom.Text = Format(Now, "MM/01/yyyy")
        txtDateTo.Text = Format(Now, "MM/dd/yyyy")

        'If Session("RDateFrom") <> "" Then
        '    txtDateFrom.Text = hDFrom.Value
        '    txtDateTo.Text = hDTo.Value
        'End If

    End Sub
    Protected Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click

        Session.Remove("pMonth")
        DataRefresh("Reload")

        Session.Remove("RDateFrom")
        Session.Remove("RDateTo")

        'Session("pMonth") = cmbMonth.SelectedValue
        Session.Remove("pMode")

    End Sub

    Private Sub GetTable_Properties()
        Dim c As New SqlClient.SqlConnection(connStr)
        Dim cm As New SqlClient.SqlCommand
        Dim rs As SqlClient.SqlDataReader

        Try
            c.Open()
        Catch ex As SqlClient.SqlException
            vScript = "alert('Error occurred while trying to connect to database. Error is: " &
                ex.Message.Replace(vbCrLf, "").Replace("'", "") & "');"
            c.Dispose()
            cm.Dispose()
            Exit Sub
        End Try

        cm.Connection = c

        cm.CommandText = "select * from table_properties_hdr where ModuleCode='" & Request.Item("id") & "' and Published='YES' "
        rs = cm.ExecuteReader
        If rs.Read Then
            If Not IsDBNull(rs("DboTable")) Then
                vTableSource = rs("DboTable")
            End If
        End If
        rs.Close()

        If vTableSource <> "" Then
            cm.CommandText = "select * from table_properties_dtl where ModuleCode='" & Request.Item("id") & "' and Published='YES' and ColType='GRIDVIEW' order by ColCode"
            'Response.Write(cm.CommandText)
            Try
                rs = cm.ExecuteReader
                Do While rs.Read
                    vColHeader += "<th>" & rs("ColTitle") & "</th>"

                    vColNames += rs("ColReturnValue").ToString.Trim & ","
                    If Not IsDBNull(rs("ColSource")) Then
                        vColSource += rs("ColSource")
                    End If
                Loop

                vColNames = Mid(vColNames, 1, Len(vColNames) - 1)
                vColSource = Mid(vColSource, 1, Len(vColSource) - 1)

                rs.Close()
            Catch ex As SqlClient.SqlException
                vScript = "alert('Error occurred while trying to buil shift reference. Error is: " &
                    ex.Message.Replace(vbCrLf, "").Replace("'", "") & "');"
            End Try
        End If
        h_Sql.Value = "select " & vColSource & " from " & vTableSource & " " & vFilter

        c.Close()
        cm.Dispose()
        c.Dispose()

    End Sub

    Private Sub DataRefresh(pMode As String)
        Dim c As New SqlClient.SqlConnection
        Dim da As SqlClient.SqlDataAdapter
        Dim ds As New DataSet
        Dim vFilter As String = ""
        Dim vTableName As String = ""
        'Dim vTempDate As Date = CDate(cmbMonth.SelectedValue & "/01/" & cmbYear.SelectedValue)
        'Dim vLastDate As String = vTempDate.AddMonths(1).AddDays(-1)



        vFilter = ""

        vFilter += " and JO_Status='" & cmbStatus.SelectedValue & "' "

        If cmbCustomer.SelectedValue <> "All" Then
            vFilter += "and Cust_Cd='" & cmbCustomer.SelectedValue & "'"
        End If

        If cmbItemType.SelectedValue <> "All" Then
            vFilter += "and ItemType_Cd='" & cmbItemType.SelectedValue & "'"
        End If

        Select Case cmbSearchBy.SelectedValue
            Case "Job Order"
                vFilter += "and JobOrderNo='" & txtSearch.Text.Trim & "'"
            Case "GCAS"
                vFilter += "and Alt_Cd='" & txtSearch.Text.Trim & "'"
            Case "Item Code"
                vFilter += "and Item_Cd='" & txtSearch.Text.Trim & "'"
        End Select

        'vFilter += " and ReleaseDate between '" & cmbMonth.SelectedValue & "/01/" & cmbYear.SelectedValue & "' and '" & Format(CDate(vLastDate), "MM/dd/yyyy") & "'"
        'vFilter += " and ReleaseDate between '" _
        '    & Format(CDate(txtDateFrom.Text.Trim), "MM/dd/yyyy") & "' and '" _
        '    & Format(CDate(txtDateTo.Text.Trim), "MM/dd/yyyy") & "' "

        c.ConnectionString = connStr


        vSQL = "select Item_Cd, Descr, Descr1, IsActive " _
            & "from item_master where ItemClass_Cd='110' "

        'vSQL = h_Sql.Value & " " & vFilter

        da = New SqlClient.SqlDataAdapter(vSQL, c)

        'Response.Write(vSQL)

        da.Fill(ds, "ItemMaster")
        tbl_Tasklist.DataSource = ds.Tables("ItemMaster")
        tbl_Tasklist.DataBind()
        lblTotalRecords.Text = "<b>Total Records Retrieved : " & tbl_Tasklist.DataSource.Rows.Count & "</b>"

        da.Dispose()
        ds.Dispose()
    End Sub

    Protected Sub tbl_Tasklist_SelectedIndexChanged(sender As Object, e As EventArgs) Handles tbl_Tasklist.SelectedIndexChanged

        Server.Transfer("taskdetails.aspx?" _
            & "pTranId=" & tbl_Tasklist.SelectedRow.Cells(2).Text _
            & "&pBOM='" & tbl_Tasklist.SelectedRow.Cells(3).Text & "'" _
            & "&pBOMRev='" & tbl_Tasklist.SelectedRow.Cells(4).Text & "'" _
            & "&pJO='" & tbl_Tasklist.SelectedRow.Cells(5).Text & "'")
    End Sub

    Private Sub tbl_Tasklist_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles tbl_Tasklist.PageIndexChanging
        tbl_Tasklist.PageIndex = e.NewPageIndex
        DataRefresh("Reload")
        'Response.Write("test")
    End Sub
End Class
