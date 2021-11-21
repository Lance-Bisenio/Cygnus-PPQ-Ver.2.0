Imports denaro
Partial Class empmaster
    Inherits System.Web.UI.Page
    Public vScript As String = ""
    Dim c As New SqlClient.SqlConnection
    Dim vTableSource As String = ""
    Dim vColNames As String = ""
    Dim vColSource As String = ""
    Public vColHeader As String = ""
    Public vRecords As String = ""


    Protected Sub Page_PreLoad(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreLoad
        If Session("uid") = "" Then
            Response.Redirect("http://" & SERVERIP & "/" & SITENAME & "/")
        End If

        If Not CanRun(Session("caption"), Request.Item("id")) Then
            Session("denied") = "1"
            Server.Transfer("../main.aspx")
            Exit Sub
        End If

        GetTable_Properties()
        If Not IsPostBack Then
            cmbShow.Items.Clear()
            For iCtr = 1 To 5
                cmbShow.Items.Add(15 * iCtr)
            Next iCtr 
            cmbShow.SelectedValue = 15

            BuildCombo("select Dept_Cd, Descr from ref_emp_department order by Descr ", cmbDept)
            cmbDept.Items.Add("All")
            cmbDept.SelectedValue = "All"

            BuildCombo("select section_Cd, Descr from ref_emp_section order by Descr", cmbSection)
            cmbSection.Items.Add("All")
            cmbSection.SelectedValue = "All"

            BuildCombo("select Pos_Cd, Descr from ref_emp_position  order by Descr", cmbPos)
            cmbPos.Items.Add("All")
            cmbPos.SelectedValue = "All"

            BuildCombo("select ColSource, ColTitle from table_properties_dtl where ModuleCode='206' and ColType in ('TEXT')", cmbSearchBy)
            
            DataRefresh("reload")
        End If
         
        Select Case h_Mode.Value
            Case "reload"
                DataRefresh("reload")
                h_Mode.Value = ""
                tbl_EmpMaster.SelectedIndex = -1
        End Select

    End Sub

    Protected Sub cmdSearch_Click(sender As Object, e As EventArgs) Handles cmdSearch.Click 
        DataRefresh("reload")
        tbl_EmpMaster.SelectedIndex = -1
        h_EmpCode.Value = ""
    End Sub
    Protected Sub tbl_ItemMaster_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles tbl_EmpMaster.PageIndexChanging
        tbl_EmpMaster.PageIndex = e.NewPageIndex
        DataRefresh("reload")
    End Sub
    Protected Sub cmbShow_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbShow.SelectedIndexChanged
        tbl_EmpMaster.PageSize = cmbShow.SelectedValue
        DataRefresh("Seach")
    End Sub

    Private Sub DataRefresh(pMode As String)
        Dim c As New SqlClient.SqlConnection
        Dim da As SqlClient.SqlDataAdapter
        Dim ds As New DataSet
        Dim vFilter As String = ""
        Dim vTableName As String = ""
        Dim vSQL As String = ""



        If cmbStatus.SelectedValue = 1 Then
            vFilter = "where Date_Resign is null "
        Else
            vFilter = "where Date_Resign is not null "
        End If

        If txtSearch.Text <> "" Then
            vFilter += " and " & cmbSearchBy.SelectedValue & " like '%" & txtSearch.Text & "%' "
        End If

        If cmbDept.SelectedValue <> "All" Then
            vFilter += " and DeptCd='" & cmbDept.SelectedValue & "' "
        End If

        If cmbSection.SelectedValue <> "All" Then
            vFilter += " and SectionCd='" & cmbSection.SelectedValue & "' "
        End If

        If cmbPos.SelectedValue <> "All" Then
            vFilter += " and Pos_Cd='" & cmbPos.SelectedValue & "' "
        End If
          
        vSQL = "select " & vColSource & " from " & vTableSource & " " & vFilter & " order by Emp_Lname asc "
        c.ConnectionString = connStr
        da = New SqlClient.SqlDataAdapter(vSQL, c)

        'Response.Write(vSQL)

        da.Fill(ds, "EmpMaster")
        tbl_EmpMaster.DataSource = ds.Tables("EmpMaster")
        tbl_EmpMaster.DataBind()
        lblTotal.Text = "<b>Total Employee Retrieved : " & tbl_EmpMaster.DataSource.Rows.Count & "</b>"

        da.Dispose()
        ds.Dispose()
        c.Dispose()

    End Sub

    Private Sub GetTable_Properties()
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

        cm.CommandText = "select * from table_properties_hdr where ModuleCode='" & Request.Item("id") & "' and Published='YES'"
        rs = cm.ExecuteReader
        If rs.Read Then
            If Not IsDBNull(rs("DboTable")) Then
                vTableSource = rs("DboTable")
            End If
        End If
        rs.Close()

        If vTableSource <> "" Then

            cm.CommandText = "select * from table_properties_dtl where ModuleCode='" & Request.Item("id") & "' and Published='YES' and ColType='GRIDVIEW' order by ColCode"
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
                vScript = "alert('Error occurred while trying to buil shift reference. Error is: " _
                    & ex.Message.Replace(vbCrLf, "").Replace("'", "") & "');"

            End Try
        End If

        c.Close()
        cm.Dispose()
        c.Dispose()

    End Sub

    Protected Sub tbl_EmpMaster_SelectedIndexChanged(sender As Object, e As EventArgs) Handles tbl_EmpMaster.SelectedIndexChanged
        h_EmpCode.Value = tbl_EmpMaster.SelectedRow.Cells(1).Text
    End Sub
End Class


