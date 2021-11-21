Imports denaro
Partial Class item_sfg
    Inherits System.Web.UI.Page
    Public vScript As String = ""
    Public vEntityId As String = ""
    Public vColHeader As String = ""
    Public vEmpRecords As String = ""
    Public vFilter As String = ""
    Public vData As String = ""

    Dim c As New SqlClient.SqlConnection
    Dim vColNames As String = ""
    Dim vColSource As String = ""
    Dim vTableSource As String = ""


    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        'If Session("uid") = "" Then
        '    vScript = "alert('Your login session has expired. Please login again.'); "
        '    Server.Transfer("index.aspx")
        'End If

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
            For iCtr = 1 To 8
                cmbShow.Items.Add(15 * iCtr)
            Next iCtr

            cmbShow.SelectedValue = 15

            'BuildCombo("select uom_Cd, Descr from ref_item_uom order by Descr", cmbUOMQ)
            'cmbUOMQ.Items.Add("All")
            'cmbUOMQ.SelectedValue = "All"

            'BuildCombo("select Type_Cd, Descr from ref_item_type order by Descr", cmbItemType)
            'cmbItemType.Items.Add("All")
            'cmbItemType.SelectedValue = "All"

            BuildCombo("select ColSource, ColTitle from table_properties_dtl where ModuleCode='" & Request.Item("id") & "' and ColType in ('TEXT','TEXTDESCR')", cmbSearchBy)
            'cmbTypeClass.Items.Add("All")
            'cmbTypeClass.SelectedValue = "All"

            'cmbSource.Items.Add("All")
            'cmbSource.SelectedValue = "All"

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
        h_Sql.Value = "select " & vColSource & ",TranId from " & vTableSource & " " & vFilter

        c.Close()
        cm.Dispose()
        c.Dispose()

    End Sub

    Protected Sub txtSearch_1_Click(sender As Object, e As EventArgs) Handles txtSearch_1.Click


        ''Dim c As New SqlClient.SqlConnection(connStr)
        ''Dim cm As New SqlClient.SqlCommand
        ''Dim rs As SqlClient.SqlDataReader
        ''Dim vOld_sfg As String = ""
        ''Try
        ''    c.Open()
        ''Catch ex As SqlClient.SqlException
        ''    vScript = "alert('Error occurred while trying to connect to database. Error is: " & _
        ''        ex.Message.Replace(vbCrLf, "").Replace("'", "") & "');"
        ''    c.Dispose()
        ''    cm.Dispose()
        ''    Exit Sub
        ''End Try

        ''cm.Connection = c

        ''cm.CommandText = "select distinct(Field2) as sfg, Field3, Field4, Field5, Field6 from sfg where Field2 is not null "
        ' ''Response.Write(cm.CommandText)
        ''Try
        ''    rs = cm.ExecuteReader

        ''    Do While rs.Read
        ''        'If Not IsDBNull(rs("Field2")) Then
        ''        '    vOld_sfg = rs("Field2")
        ''        'End If


        ''        'insert into item_master (Item_Cd,Descr,Descr1,CurrCost," & _
        ''        '"MinOrderQty,MinOrderQtyUOM_Cd,ProdLeadDays,ProdLeadTime," & _
        ''        '"RlseLeadDays,RlseLeadTime," & _
        ''        '"ItemType_Cd,Source,ItemClass_Cd," & _
        ''        '"QtyUOM_Cd,IsActive,ModifyBy,DateModify)

        ''        'Response.Write("insert into item_master (Item_Cd,Descr,Descr1,CurrCost,ItemType_Cd,ItemClass_Cd,IsActive,ModifyBy,DateModify,Source) values " & _
        ''        '               "('" & rs("sfg") & "','" & rs("Field3") & " " & rs("Field4") & "','" & rs("Field6") & "','0','SFG','110','1','SystemUpload', '" & Format(Now, "yyyy-MM-dd") & "','Make') <br>")


        ''        'Response.Write("insert into item_sfg values ('" & IIf(IsDBNull(rs("Field2")), vOld_sfg, rs("Field2")) & "','" & _
        ''        '               rs("Field7") & "','" & rs("Field9") & "','" & rs("Field10") & "','" & Session("uid") & "', '" & Now & "','System Uploaded') <br>")

        ''        'vData += "<tr><td>" & rs("id") & "</td>" & _
        ''        '    "<td>" & IIf(IsDBNull(rs("Field2")), vOld_sfg, rs("Field2")) & "</td>" & _
        ''        '    "<td>" & rs("Field7") & "</td>" & _
        ''        '    "<td>" & rs("Field9") & "</td>" & _
        ''        '    "<td>" & rs("Field10") & "</td>" & _
        ''        '    "</tr>"
        ''    Loop

        ''    rs.Close()
        ''Catch ex As SqlClient.SqlException
        ''    vScript = "alert('Error occurred while trying to buil shift reference. Error is: " & _
        ''        ex.Message.Replace(vbCrLf, "").Replace("'", "") & "');"
        ''End Try 

        ''c.Close()
        ''cm.Dispose()
        ''c.Dispose()


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

        If cmbItemStatus.SelectedValue = "Deleted" Then
            vFilter = " where IsActive='3' "
        Else
            vFilter = " where IsActive='" & cmbItemStatus.SelectedValue & "' "
        End If

        If txtSearch.Text <> "" Then
            vFilter += " and " & cmbSearchBy.SelectedValue & " like '%" & txtSearch.Text & "%' "
        End If

        'If cmbSource.SelectedValue <> "All" Then
        '    vFilter += " and Source='" & cmbSource.SelectedValue & "' "
        'End If

        'If cmbItemType.SelectedValue <> "All" Then
        '    vFilter += " and ItemType_Cd='" & cmbItemType.SelectedValue & "' "
        'End If

        'If cmbTypeClass.SelectedValue <> "All" Then
        '    vFilter += " and ItemClass_Cd='" & cmbTypeClass.SelectedValue & "' "
        'End If

        'If cmbUOMQ.SelectedValue <> "All" Then
        '    vFilter += " and QtyUOM_cd='" & cmbUOMQ.SelectedValue & "' "
        'End If

        vFilter += " order by SFG_ItemCd"


        c.ConnectionString = connStr
        da = New SqlClient.SqlDataAdapter(h_Sql.Value & " " & vFilter, c)

        'Response.Write(da.SelectCommand.CommandText)

        da.Fill(ds, "ItemMaster")
        tbl_ItemMaster.DataSource = ds.Tables("ItemMaster")
        tbl_ItemMaster.DataBind()
        lblTotal.Text = "<b>Total Item Retrieved : " & tbl_ItemMaster.DataSource.Rows.Count & "</b>"

        da.Dispose()
        ds.Dispose()
    End Sub

    Protected Sub tbl_ItemMaster_SelectedIndexChanged(sender As Object, e As EventArgs) Handles tbl_ItemMaster.SelectedIndexChanged
        h_ItemCd.Value = tbl_ItemMaster.SelectedRow.Cells(2).Text
    End Sub

    Protected Sub tbl_ItemMaster_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles tbl_ItemMaster.PageIndexChanging
        tbl_ItemMaster.PageIndex = e.NewPageIndex
        DataRefresh("Seach")
    End Sub
    Protected Sub cmbShow_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbShow.SelectedIndexChanged
        tbl_ItemMaster.PageSize = cmbShow.SelectedValue
        DataRefresh("Seach")
    End Sub
End Class
 