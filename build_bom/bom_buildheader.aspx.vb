Imports denaro
Partial Class bom_buildheader
    Inherits System.Web.UI.Page
    Public vScript As String = ""
    Dim c As New SqlClient.SqlConnection

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Session("uid") = Nothing Or Session("uid") = "" Then
			vScript = "alert('Login session has expired. Please re-login again.'); window.opener.document.form1.h_Mode.value='reload'; window.opener.document.form1.submit(); window.close();"
			Exit Sub
		End If

		If Request.Item("pBom") = "" And Request.Item("Mode") = "Edit" Then
			vScript = "alert('No selected BOM');  window.close();"
			Exit Sub
		End If

		If Not IsPostBack Then
            BuildCombo("select Uom_Cd, Descr from ref_item_uom  order by Descr", cmbNetWUOM)
            BuildCombo("select Uom_Cd, Descr from ref_item_uom  order by Descr", cmbMOUOM)
            BuildCombo("select Uom_Cd, Descr from ref_item_uom  order by Descr", cmbItemUom)
            BuildCombo("select Status_Cd,Descr from ref_item_Status where GroupName='BOM' order by Descr", cmbStatus)
            BuildCombo("select Item_Cd,Descr from item_master where ItemType_Cd in ('FG','SFG') " & _
                       "order by Descr", cmbItem) 'where Item_Cd not in (select Item_Cd from bom_header) Source='Make' and ItemType_Cd in ('FG','SFG') and 
            cmbStatus.SelectedValue = 2

            cmbNetWUOM.Items.Add(" ")
            cmbNetWUOM.SelectedValue = " "
            cmbMOUOM.Items.Add(" ")
            cmbMOUOM.SelectedValue = " "

            cmbItem.Items.Add(" ")
            cmbItem.SelectedValue = " "
            'txtBOM_DateAvtive.Text = Format(Now, "MM/dd/yyyy")
            txtItemCd.Text = cmbItem.SelectedValue

            If Request.Item("mode") = "Edit" Or Request.Item("mode") = "View" Then

                If Request.Item("mode") = "View" Then
                    txtItemCd.Enabled = False
                    cmdGetItemDetails.Visible = False
                End If

                If Request.Item("mode") = "View" Then
                    vScript = "$('#cmdSave').hide();"
                End If

				'cmbItem.Enabled = False
				GetDetails()
            End If
        End If

    End Sub
    Protected Sub cmbItem_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbItem.SelectedIndexChanged
        txtItemCd.Text = cmbItem.SelectedValue
        GetDetails()
    End Sub

    Protected Sub cmdSave_Click(sender As Object, e As EventArgs) Handles cmdSave.Click
        txtBOM_DateAvtive.Text = Request.Item("txtBOM_DateAvtive")
        If cmbStatus.SelectedValue = "Active" Then
            vScript = "alert('Please enter Date Active');"
            Exit Sub
        End If

        If cmbItem.SelectedValue = " " Then
            vScript = "alert('Please select Item');"
            Exit Sub
        Else
            Save()
        End If
         
    End Sub

    Private Sub GetDetails()
        Dim vSQL As String = ""
        Dim cm As New SqlClient.SqlCommand
        Dim rs As SqlClient.SqlDataReader
        Dim vBOM_Code As String = ""
        Dim vNetW As String = ""
        Dim vMOQty As String = ""
        Dim vMOUOM As String = ""
        Dim vProLDays As String = ""
        Dim vProLTime As String = ""

        c.ConnectionString = connStr
        c.Open()
        cm.Connection = c 

        'If txtBOM_No.Text.Trim <> "Auto Generated" Then 
        'End If

        If Request.Item("mode") = "new" Then
            '=== CREATE NEW RECORD ======================================================================================================================================
            cm.CommandText = "select NetWeight,WeightUOM_Cd,QtyUOM_Cd,MinOrderQty,MinOrderQtyUOM_Cd,ProdLeadDays,ProdLeadTime, " & _
                                "(select Descr from ref_item_type where ref_item_type.Type_Cd=item_master.ItemType_Cd) as vItemType, ItemType_Cd " & _
                                "from item_master where Item_Cd='" & cmbItem.SelectedValue & "'"
            'Response.Write(cm.CommandText)
            'Exit Sub
            rs = cm.ExecuteReader
            If rs.Read Then
                'Response.Write("QtyUOM_Cd: " & rs("QtyUOM_Cd"))

                Try
                    cmbNetWUOM.SelectedValue = IIf(IsDBNull(rs("WeightUOM_Cd")), " ", rs("WeightUOM_Cd"))
                    cmbMOUOM.SelectedValue = IIf(IsDBNull(rs("MinOrderQtyUOM_Cd")), " ", rs("MinOrderQtyUOM_Cd"))
                    cmbItemUom.SelectedValue = IIf(IsDBNull(rs("QtyUOM_Cd")), "", rs("QtyUOM_Cd"))
                Catch ex As Exception

                End Try

                txtItemType_Cd.Text = IIf(IsDBNull(rs("ItemType_Cd")), " ", rs("ItemType_Cd"))
                txtItemType.Text = IIf(IsDBNull(rs("vItemType")), " ", rs("vItemType"))

                txtNetW.Text = IIf(IsDBNull(rs("NetWeight")), 0, rs("NetWeight"))
                txtMOQty.Text = IIf(IsDBNull(rs("MinOrderQty")), 0, rs("MinOrderQty"))
                txtSPRunDays.Text = IIf(IsDBNull(rs("ProdLeadDays")), 0, rs("ProdLeadDays"))
                txtSPRunTime.Text = IIf(IsDBNull(rs("ProdLeadTime")), "00:00", rs("ProdLeadTime").ToString)
            End If
            rs.Close()
        Else
			'=== MODIFY OLD RECORD ======================================================================================================================================
			cm.CommandText = "select *, " & _
				"(select QtyUOM_Cd from item_master where item_master.Item_Cd=bom_header.Item_Cd) as vItemUOM, " & _
				"(select MinOrderQtyUOM_Cd from item_master where item_master.Item_Cd=bom_header.Item_Cd) as vOrderQtyUOM, " & _
				"(select Descr from ref_item_type where ref_item_type.Type_Cd=bom_header.ItemType_Cd) as vItemType, ItemType_Cd " & _
				"from bom_header where BOM_Cd=" & Request.Item("pBOM") & " and Revision=" & Request.Item("pBOMRev")

			'Response.Write(cm.CommandText)
			rs = cm.ExecuteReader
            If rs.Read Then
                txtBOM_No.Text = Request.Item("pBOM")
                txtBOM_Rev.Text = IIf(IsDBNull(rs("Revision")), "Auto Geneted", rs("Revision"))
                txtItemCd.Text = IIf(IsDBNull(rs("Item_Cd")), "", rs("Item_Cd"))
                cmbStatus.SelectedValue = IIf(IsDBNull(rs("Status_Cd")), 0, rs("Status_Cd"))
				cmbItem.SelectedValue = IIf(IsDBNull(rs("Item_Cd")), "", rs("Item_Cd"))
				txtBOM_DateAvtive.Text = IIf(IsDBNull(rs("DateActive")), "", rs("DateActive"))
                rdoReportTo.SelectedValue = IIf(IsDBNull(rs("ReportTo")), 0, rs("ReportTo"))

                txtNetW.Text = IIf(IsDBNull(rs("NetWeight")), 0, rs("NetWeight"))
                cmbNetWUOM.SelectedValue = IIf(IsDBNull(rs("NetWeightUOM")), " ", rs("NetWeightUOM"))
                cmbItemUom.SelectedValue = IIf(IsDBNull(rs("ItemUOM")), "", rs("ItemUOM"))
                txtMOQty.Text = IIf(IsDBNull(rs("StdQty")), 0, rs("StdQty"))
                cmbMOUOM.SelectedValue = IIf(IsDBNull(rs("StdQtyUOM")), " ", rs("StdQtyUOM"))
                txtSPRunDays.Text = IIf(IsDBNull(rs("StdProdRunDay")), 0, rs("StdProdRunDay"))
                txtSPRunTime.Text = IIf(IsDBNull(rs("StdProdRunTime")), "00:00", rs("StdProdRunTime").ToString)

                txtItemType_Cd.Text = IIf(IsDBNull(rs("ItemType_Cd")), " ", rs("ItemType_Cd"))
                txtItemType.Text = IIf(IsDBNull(rs("vItemType")), " ", rs("vItemType"))
            End If
            rs.Close()
        End If


        c.Dispose()
        c.Close()
        cm.Dispose()

    End Sub

    Private Sub Save()
        Dim vSQL As String = ""
        Dim cm As New SqlClient.SqlCommand
        Dim rs As SqlClient.SqlDataReader
        Dim vBOM_Code As String = ""
        Dim vRevision As Integer = 1

        Dim vUpdate As String = ""

        c.ConnectionString = connStr
        c.Open()
        cm.Connection = c

        cm.CommandText = "select max(BOM_Cd) as Bom from bom_header "
        rs = cm.ExecuteReader
        If rs.Read Then
            vBOM_Code = IIf(IsDBNull(rs("Bom")), 0, rs("Bom")) + 1
        End If
        rs.Close()

        If Request.Item("mode") = "new" Then

			vSQL = "insert into bom_header (BOM_Cd,Revision,ActiveBy,Status_Cd,Item_Cd," & _
				"StdQty,StdQtyUOM,StdProdRunDay,StdProdRunTime,NetWeight,NetWeightUOM," & _
				"ReportTo,CreatedBy,DateCreated,ItemUOM,ItemType_Cd) values (" & _
				vBOM_Code & ",'" & vRevision & "','" & Session("uid") & "','" & cmbStatus.SelectedValue & "'," & "'" & cmbItem.SelectedValue & "','" & _
				txtMOQty.Text & "','" & cmbMOUOM.Text & "','" & txtSPRunDays.Text & "','" & txtSPRunTime.Text & "','" & txtNetW.Text & "','" & cmbNetWUOM.SelectedValue & "','" & _
				rdoReportTo.SelectedValue & "','" & Session("uid") & "','" & Format(CDate(Now), "MM/dd/yyyy") & "','" & cmbItemUom.SelectedValue & "','" & txtItemType_Cd.Text & "') "

		Else
			vSQL = "update bom_header set ActiveBy='" & Session("uid") & "'," & _
				"Status_Cd='" & cmbStatus.SelectedValue & "',Item_Cd='" & cmbItem.SelectedValue & "'," & _
				"StdQty='" & txtMOQty.Text & "',StdQtyUOM='" & cmbMOUOM.Text & "',StdProdRunDay='" & txtSPRunDays.Text & "',StdProdRunTime='" & txtSPRunTime.Text & "'," & _
				"NetWeight='" & txtNetW.Text & "',NetWeightUOM='" & cmbNetWUOM.SelectedValue & "',ReportTo='" & rdoReportTo.SelectedValue & "',CreatedBy='" & Session("uid") & "'," & _
				"DateCreated='" & Format(CDate(Now), "MM/dd/yyyy") & "',ItemUOM='" & cmbItemUom.SelectedValue & "', ItemType_Cd='" & txtItemType_Cd.Text & "' " & _
				"where BOM_Cd=" & Request.Item("pBOM") & " and Revision=" & Request.Item("pBOMRev")

		End If

        cm.CommandText = vSQL

        Try
            cm.ExecuteNonQuery()


            If txtBOM_DateAvtive.Text.Trim <> "" And cmbStatus.SelectedValue = 1 Then
                vSQL = "update bom_header set DateActive='" & Format(CDate(txtBOM_DateAvtive.Text), "MM/dd/yyyy") & "' " _
                               & "where BOM_Cd=" & Request.Item("pBOM") & " and Revision=" & Request.Item("pBOMRev")

                cm.CommandText = vSQL
                cm.ExecuteNonQuery()
            End If


        Catch ex As SqlClient.SqlException
			Response.Write("Error in SQL query insert/update: " & vSQL & ex.Message)
		End Try


        c.Dispose()
        c.Close()
        cm.Dispose()
		vScript = "alert('Successfully saved.'); window.close();"

	End Sub
     
    Protected Sub cmdGetItemDetails_Click(sender As Object, e As EventArgs) Handles cmdGetItemDetails.Click

        If txtItemCd.Text.Trim = "" Then
            BuildCombo("select Item_Cd, Descr from item_master where ItemType_Cd in ('FG','SFG') " & _
                      "order by Descr", cmbItem)
            cmbItem.Items.Add(" ")
            cmbItem.SelectedValue = " "

            vScript = "alert('Please enter Item Code'); $('#txtItemCd').focus();"
        Else
            BuildCombo("select Item_Cd, Descr from item_master where Item_Cd like '%" & txtItemCd.Text.Trim & "%' and ItemType_Cd in ('FG','SFG') " & _
                      "order by Descr", cmbItem)
            GetDetails()
        End If 
    End Sub

    Protected Sub cmbStatus_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbStatus.SelectedIndexChanged

        If cmbStatus.SelectedValue = 1 Then
			txtBOM_DateAvtive.Text = Format(CDate(Now), "MM/dd/yyyy")
		Else
            txtBOM_DateAvtive.Text = ""
        End If 
    End Sub
End Class
