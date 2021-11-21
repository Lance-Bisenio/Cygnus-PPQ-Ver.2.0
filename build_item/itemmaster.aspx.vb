Imports denaro
Imports item_details


Partial Class itemmaster
    Inherits System.Web.UI.Page
    Public vScript As String = ""
    Public vEntityId As String = ""
    Public vColHeader As String = ""
    Public vEmpRecords As String = ""
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

		vEntityId = Request.Item("id").ToString.Trim

        If Not IsPostBack Then

            cmbShow.Items.Clear()
			For iCtr = 1 To 10
				cmbShow.Items.Add(10 * iCtr)
			Next iCtr

			cmbShow.SelectedValue = 10

			BuildCombo("select uom_Cd, Descr from ref_item_uom order by Descr", cmbUOMQ)
            cmbUOMQ.Items.Add("All")
            cmbUOMQ.SelectedValue = "All"

            BuildCombo("select Type_Cd, Descr from ref_item_type order by Descr", cmbItemType)
            cmbItemType.Items.Add("All")
            cmbItemType.SelectedValue = "All"

            BuildCombo("select Class_Cd, Descr from ref_item_class order by Descr", cmbTypeClass)
            cmbTypeClass.Items.Add("All")
            cmbTypeClass.SelectedValue = "All"

            BuildCombo("select ColSource, ColTitle from table_properties_dtl where ModuleCode='203' and ColType='SEARCHBY'", cmbSearchBy)

            cmbSource.Items.Add("All")
            cmbSource.SelectedValue = "All"

            If CanRun(Session("caption"), 204) = True Then
                cmbItemStatus.Items.Add("Deleted")
            End If

            GetTable_Properties()
            DataRefresh("Seach")
        End If

        Select Case h_Mode.Value
            Case "reload"
                GetTable_Properties()
                DataRefresh("reload")
                h_Mode.Value = ""
                tbl_ItemMaster.SelectedIndex = -1
        End Select



    End Sub
     
    Private Sub GetTable_Properties()
        Dim c As New SqlClient.SqlConnection(connStr)
        Dim cm As New SqlClient.SqlCommand
        Dim rs As SqlClient.SqlDataReader

        Try
            c.Open()
        Catch ex As SqlClient.SqlException
            vScript = "alert('Error occurred while trying to connect to database. Error is: " & _
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
                vScript = "alert('Error occurred while trying to buil shift reference. Error is: " & _
                    ex.Message.Replace(vbCrLf, "").Replace("'", "") & "');" 
            End Try
        End If
        h_Sql.Value = "select " & vColSource & " from " & vTableSource & " " & vFilter

        c.Close()
        cm.Dispose()
        c.Dispose()

    End Sub
     
    Protected Sub txtSearch_1_Click(sender As Object, e As EventArgs) Handles txtSearch_1.Click
        h_ItemCd.Value = ""
        h_Mode.Value = ""
          
        tbl_ItemMaster.SelectedIndex = -1
        GetTable_Properties()
        DataRefresh("Seach")
    End Sub

    Private Sub DataRefresh(pMode As String)
        Dim c As New SqlClient.SqlConnection
        Dim da As SqlClient.SqlDataAdapter
        Dim ds As New DataSet
        Dim vFilter As String = ""
        Dim vTableName As String = ""
        Dim vSQL As String = ""

        vFilter = "where IsActive=" & IIf(cmbItemStatus.SelectedValue = "Deleted", 3, cmbItemStatus.SelectedValue)

        If txtSearch.Text <> "" Then
			If cmbSearchBy.SelectedValue = "Item_GCAS" Then

				If SeachItem_GCAS(txtSearch.Text.Trim).Trim <> "" Then
					vFilter += " and Item_Cd like '%" & SeachItem_GCAS(txtSearch.Text.Trim).Trim & "%' "
				End If

			Else
				vFilter += " and " & cmbSearchBy.SelectedValue & " like '%" & txtSearch.Text & "%' "
			End If 
        End If

		If cmbSource.SelectedValue <> "All" Then
            vFilter += " and Source='" & cmbSource.SelectedValue & "' "
        End If

        If cmbItemType.SelectedValue <> "All" Then
            vFilter += " and ItemType_Cd='" & cmbItemType.SelectedValue & "' "
        End If
         
        If cmbTypeClass.SelectedValue <> "All" Then
            vFilter += " and ItemClass_Cd='" & cmbTypeClass.SelectedValue & "' "
        End If

        If cmbUOMQ.SelectedValue <> "All" Then
            vFilter += " and QtyUOM_Cd='" & cmbUOMQ.SelectedValue & "' "
        End If 
        vFilter += " Order By Item_Cd asc"
         
        c.ConnectionString = connStr 
        vSQL = h_Sql.Value & " " & vFilter
        'Response.Write(vSQL)
        da = New SqlClient.SqlDataAdapter(vSQL, c)
          
        da.Fill(ds, "ItemMaster")
        tbl_ItemMaster.DataSource = ds.Tables("ItemMaster")
        tbl_ItemMaster.DataBind()
        lblTotal.Text = "<b>Total Item Retrieved : " & tbl_ItemMaster.DataSource.Rows.Count & "</b>"

        da.Dispose()
        ds.Dispose()
    End Sub

    Protected Sub tbl_ItemMaster_SelectedIndexChanged(sender As Object, e As EventArgs) Handles tbl_ItemMaster.SelectedIndexChanged
        h_ItemCd.Value = tbl_ItemMaster.SelectedRow.Cells(3).Text
    End Sub
  
    Protected Sub tbl_ItemMaster_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles tbl_ItemMaster.PageIndexChanging
        tbl_ItemMaster.PageIndex = e.NewPageIndex 
        DataRefresh("Seach")
    End Sub

    Protected Sub cmbShow_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbShow.SelectedIndexChanged
        tbl_ItemMaster.PageSize = cmbShow.SelectedValue 
        DataRefresh("Seach")
    End Sub

    Public Function GetGCAS(ByVal pItemCd As String) As String
  
        Return GetGCAS_List(pItemCd)

    End Function
End Class
 