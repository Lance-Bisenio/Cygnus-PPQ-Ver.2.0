Imports denaro
Partial Class production_tranlog
    Inherits System.Web.UI.Page
    Public vScript As String = "" 
    

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Session("uid") = "" Then
            vScript = "alert('Login session has expired. Please re-login again.'); window.opener.document.form1.txtMode.value='reload'; window.opener.document.form1.submit(); window.close();"
            Exit Sub
        End If
        If Not IsPostBack Then

            BuildCombo("select uom_Cd, Descr from ref_item_uom order by Descr", cmbContainerType)
            cmbContainerType.Items.Add("None")
            cmbContainerType.SelectedValue = "None"
             
            txtSFGItemCd.Text = Request.Item("pSFG").Trim
            txtSFGDescr.Text = GetRef("select (Descr +' '+ Descr1) as Descr from item_master where Item_Cd='" & Request.Item("pSFG").Trim & "'", "None")

            txtOperOrder.Value = GetRef("select OperOrder from bom_process where BOM_Cd=" & Request.Item("pBOM") & " and " & _
                                "Revision=" & Request.Item("pBOMRev") & " and SFG_Cd='" & txtSFGItemCd.Text & "'", "None")

            If Request.Item("pMode") = "edit" Then
                'Get_TransactionDetails() 
            End If

        End If
    End Sub

    Protected Sub rdoReportTo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles rdoReportTo.SelectedIndexChanged

        If rdoReportTo.SelectedValue = "Warehouse" Then
            cmbOperationNo.Enabled = False
            cmbOperationNo.Items.Add(" ")
            cmbOperationNo.SelectedValue = " "
        Else
            cmbOperationNo.Enabled = True
            BuildCombo("select OperOrder, " & _
                "(select (SFG_Cd +' = '+ Descr +' '+ Descr1) as Descr from item_master where Item_Cd=SFG_Cd) as vDescr " & _
                "from bom_process where BOM_Cd=" & Request.Item("pBOM") & " and Revision=" & Request.Item("pBOMRev") & _
                " and OperOrder > " & txtOperOrder.Value & " order by OperOrder", cmbOperationNo)

            'Response.Write("select OperOrder, " & _
            '    "(select (SFG_Cd +' = '+ Descr +' '+ Descr1) as Descr from item_master where Item_Cd=SFG_Cd) as vDescr " & _
            '    "from bom_process where BOM_Cd=" & Request.Item("pBOM") & " and Revision=" & Request.Item("pBOMRev") & _
            '    " and OperOrder > " & txtOperOrder.Value & " order by OperOrder")


            If cmbOperationNo.SelectedValue = "" Then
                vScript = "alert('The system cant find any next process.');"
                rdoReportTo.SelectedValue = "Warehouse"
                cmbOperationNo.Enabled = False
            End If

        End If
    End Sub

    Protected Sub txtClose_Click(sender As Object, e As EventArgs) Handles txtClose.Click
        vScript = "window.close();"
    End Sub
    'Private Sub Get_TransactionDetails()
    '    Dim c As New SqlClient.SqlConnection(connStr)
    '    Dim cm As New SqlClient.SqlCommand
    '    Dim rs As SqlClient.SqlDataReader

    '    cm.Connection = c
    '    c.Open()
    '    cm.CommandText = "select * from item_sfg where Tranid='" & Request.Item("pTranId").Trim & "' "
    '    'Response.Write(cm.CommandText)
    '    Try
    '        rs = cm.ExecuteReader
    '        If rs.Read Then

    '            txtSFGItemCd.Text = IIf(IsDBNull(rs("SFG_ItemCd")), "0", rs("SFG_ItemCd"))
    '            txtMatItemCd.Text = IIf(IsDBNull(rs("Mat_ItemCd")), "0", rs("Mat_ItemCd"))
    '            cmbSFGDescr.SelectedValue = IIf(IsDBNull(rs("SFG_ItemCd")), " ", rs("SFG_ItemCd"))
    '            cmbMatDescr.SelectedValue = IIf(IsDBNull(rs("Mat_ItemCd")), " ", rs("Mat_ItemCd"))
    '            txtGrams.Text = IIf(IsDBNull(rs("Grams")), "0", Format(rs("Grams"), "###.00"))
    '            txtPercentage.Text = IIf(IsDBNull(rs("Percentage")), "0", Format(rs("Percentage"), "###.00"))

    '            rs.Close()
    '            c.Close()
    '            c.Dispose()
    '            cm.Dispose()

    '        End If
    '    Catch ex As SqlClient.SqlException
    '        Response.Write("Error in retrieving data. " & ex.Message)
    '    Finally
    '        c.Close()
    '        c.Dispose()
    '        cm.Dispose()
    '    End Try
    'End Sub

    'Protected Sub btnFindSFG_Click(sender As Object, e As EventArgs) Handles btnFindSFG.Click
    '    If txtSFGItemCd.Text.Trim <> "" Then
    '        BuildCombo("select Item_Cd, Descr +' '+ Descr1 as vDescr from item_master where Item_Cd='" & txtSFGItemCd.Text.Trim & "' and itemType_Cd in ('SFG') order by Descr", cmbSFGDescr)

    '    Else
    '        vScript = "alert('Please enter SFG Item Code');"
    '        BuildCombo("select Item_Cd,  Descr +' '+ Descr1 as vDescr  from item_master where itemType_Cd in ('SFG') order by Descr", cmbSFGDescr)
    '        cmbSFGDescr.Items.Add("")
    '        cmbSFGDescr.SelectedValue = ""
    '    End If

    'End Sub

    'Protected Sub btnFindMat_Click(sender As Object, e As EventArgs) Handles btnFindMat.Click
    '    If txtMatItemCd.Text.Trim <> "" Then
    '        BuildCombo("select Item_Cd, Descr from item_master where Item_Cd='" & txtMatItemCd.Text.Trim & "' and  itemType_Cd in ('SFG','RM') order by Descr", cmbMatDescr)

    '    Else
    '        vScript = "alert('Please enter Material Item Code');"
    '        BuildCombo("select Item_Cd, Descr  from item_master where itemType_Cd in ('SFG','RM') order by Descr", cmbMatDescr)
    '        cmbMatDescr.Items.Add("")
    '        cmbMatDescr.SelectedValue = ""
    '    End If

    'End Sub

    'Protected Sub cmdSave_Click(sender As Object, e As EventArgs) Handles cmdSave.Click
    '    SaveModule()
    'End Sub

    'Private Sub SaveModule()

    '    If txtSFGItemCd.Text.Trim = "" Or cmbSFGDescr.SelectedValue = "" Then
    '        vScript = "alert('SFG Item Code / Description is required.');"
    '        Exit Sub
    '    End If

    '    If txtSFGItemCd.Text.Trim = "" Or cmbMatDescr.SelectedValue = "" Then
    '        vScript = "alert('Material Item Code / Description is required.');"
    '        Exit Sub
    '    End If

    '    If txtGrams.Text.Trim = "" Then
    '        txtGrams.Text = 0
    '    End If

    '    If txtPercentage.Text.Trim = "" Then
    '        txtPercentage.Text = 0
    '    End If


    '    Dim c As New SqlClient.SqlConnection(connStr)
    '    Dim cm As New SqlClient.SqlCommand
    '    'Dim rs As SqlClient.SqlDataReader

    '    Dim vFields As String = ""
    '    Dim vFData As String = ""
    '    Dim vNewRecord As String = ""
    '    Dim vSQL As String = ""

    '    cm.Connection = c
    '    c.Open()

    '    If Request.Item("pMode") = "new" Then
    '        vSQL = "insert into item_sfg (SFG_ItemCd,Mat_ItemCd,Grams,Percentage,CreatedBy,DateCreated) values ('" & _
    '                    txtSFGItemCd.Text.Trim & "','" & txtMatItemCd.Text.Trim & "','" & txtGrams.Text.Trim & "','" & txtPercentage.Text.Trim & "','" & Session("uid") & "','" & Now & "')"
    '    Else
    '        vSQL = "Update item_sfg set " & _
    '                    "SFG_ItemCd='" & txtSFGItemCd.Text.Trim & "', Mat_ItemCd='" & txtMatItemCd.Text.Trim & "'," & _
    '                    "Grams='" & txtGrams.Text.Trim & "',Percentage='" & txtPercentage.Text.Trim & "', " & _
    '                    "CreatedBy='" & Session("uid") & "', DateCreated='" & Now & "' where TranId='" & Request.Item("pTranId").Trim & "'"
    '    End If

    '    cm.CommandText = vSQL
    '    'Response.Write(vSQL)
    '    Try
    '        cm.ExecuteNonQuery()
    '        vScript = "alert('Successfully saved.'); window.opener.document.form1.h_Mode.value='reload'; window.opener.document.form1.submit(); " 'window.close();"
    '        cmbMatDescr.SelectedValue = ""
    '        txtMatItemCd.Text = ""
    '        txtGrams.Text = ""
    '        txtPercentage.Text = ""

    '    Catch ex As DataException
    '        vScript = "alert('An error occurred while trying to Save the new record.');"
    '    Finally
    '        c.Close()
    '        c.Dispose()
    '        cm.Dispose()
    '    End Try

    'End Sub

    'Protected Sub cmbSFGDescr_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbSFGDescr.SelectedIndexChanged
    '    txtSFGItemCd.Text = cmbSFGDescr.SelectedValue
    'End Sub

    'Protected Sub cmbMatDescr_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbMatDescr.SelectedIndexChanged
    '    txtMatItemCd.Text = cmbMatDescr.SelectedValue
    'End Sub

    
End Class
