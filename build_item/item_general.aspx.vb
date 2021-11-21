Imports denaro
Partial Class Item_general
	Inherits System.Web.UI.Page
	Public vScript As String = ""
	Public vErrorMsg As String = ""

	Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

		'If Session("uid") = "" Then
		'    vScript = "alert('Login session has expired. Please re-login again.'); window.opener.document.form1.h_Mode.value='reload'; window.opener.document.form1.submit(); window.close();"
		'    Exit Sub
		'End If

		'If Request.Item("pItemCd") = "" And Request.Item("pMode") = "edit" Then
		'    vScript = "alert('No selected Item');  window.close();"
		'    Exit Sub
		'End If

		If Not IsPostBack Then

			BuildCombo("select Item_Cd, Descr  from item_master where itemType_Cd like 'FG' order by Descr", cmbParentItem)
			cmbParentItem.Items.Add("")
			cmbParentItem.SelectedValue = ""

			BuildCombo("select Uom_Cd, Descr from ref_item_uom  order by Descr", cmbMinOrderUom)
			cmbMinOrderUom.Items.Add("")
			cmbMinOrderUom.SelectedValue = ""

			BuildCombo("select Uom_Cd, Descr from ref_item_uom  order by Descr", cmbUomQty)
			cmbUomQty.SelectedValue = "99"

			BuildCombo("select Type_Cd, Descr from ref_item_Type  order by Descr", cmbItemType)
			cmbItemType.SelectedValue = "99"

			BuildCombo("select Class_Cd, Descr from ref_item_class  order by Descr", cmbTypeClass)
			cmbTypeClass.SelectedValue = "99"

			cmbSource.Items.Add("Buy")
			cmbSource.Items.Add("Make")
			cmbSource.SelectedValue = "Buy"

			If Request.Item("pMode") = "edit" Then
				GetInfo()
			End If
		End If

	End Sub

	Private Sub GetInfo()
		Dim c As New SqlClient.SqlConnection(connStr)
		Dim cm As New SqlClient.SqlCommand
		Dim rs As SqlClient.SqlDataReader

		cm.Connection = c
		c.Open()
		cm.CommandText = "select * from item_master where Item_Cd='" & Request.Item("pItemCd").Trim & "' "
		'Response.Write(cm.CommandText)
		Try
			rs = cm.ExecuteReader
			If rs.Read Then
				If IsDBNull(rs("CurrCost")) Then
					txtCost.Text = "0.00"
				Else
					txtCost.Text = Format(rs("CurrCost"), "#,###,##0.00")
				End If

				'BuildCombo("select Uom_Cd, Descr from ref_item_uom  order by Descr", cmbUomQty) 
				'BuildCombo("select Type_Cd, Descr from ref_item_Type  order by Descr", cmbItemType)
				cmbStatus.SelectedValue = rs("IsActive")

				If rs("IsActive") = 1 Then
					cmbStatus.Enabled = True
				End If

				txtItemCd.Text = IIf(IsDBNull(rs("Item_Cd")), "", rs("Item_Cd"))

				If Not IsDBNull(rs("Descr")) Then
					txtDescr1.Text = rs("Descr").Replace("""", "'")
				End If

				If Not IsDBNull(rs("Descr1")) Then
					txtDescr2.Text = rs("Descr1").Replace("""", "'")
				End If


				txtBarcode.Text = IIf(IsDBNull(rs("Barcode")), "", rs("Barcode"))
				txtSerialNo.Text = IIf(IsDBNull(rs("SerialNo")), "", rs("SerialNo"))
				txtCost.Text = IIf(IsDBNull(rs("CurrCost")), "0.00", rs("CurrCost"))
				txtCoreWeight.Text = IIf(IsDBNull(rs("CoreWeight")), "0", rs("CoreWeight"))

				txtMinOrderQty.Text = IIf(IsDBNull(rs("MinOrderQty")), "0", rs("MinOrderQty"))

				If Not IsDBNull(rs("ProdLeadTime")) Then
					txtProdLeadTimeHrs.Text = rs("ProdLeadTime").ToString
				Else
					txtProdLeadTimeHrs.Text = "00:00"
				End If
				If Not IsDBNull(rs("RlseLeadTime")) Then
					txtReleaseLeadTimeHrs.Text = rs("RlseLeadTime").ToString
				Else
					txtReleaseLeadTimeHrs.Text = "00:00"
				End If

				txtProdLeadTimeDays.Text = IIf(IsDBNull(rs("ProdLeadDays")), "0", rs("ProdLeadDays"))
				txtReleaseLeadTimeDay.Text = IIf(IsDBNull(rs("RlseLeadDays")), "0", rs("RlseLeadDays"))
				cmbItemType.SelectedValue = IIf(IsDBNull(rs("ItemType_Cd")), "99", rs("ItemType_Cd"))
				cmbSource.SelectedValue = IIf(IsDBNull(rs("Source")), "99", rs("Source"))
				cmbUomQty.SelectedValue = IIf(IsDBNull(rs("QtyUOM_Cd")), "99", rs("QtyUOM_Cd"))
				cmbMinOrderUom.SelectedValue = IIf(IsDBNull(rs("MinOrderQtyUOM_Cd")), "99", rs("MinOrderQtyUOM_Cd"))


				txtRepeatLenght.Text = IIf(IsDBNull(rs("RepeatLenght")), "", rs("RepeatLenght"))
				txtRollWidth.Text = IIf(IsDBNull(rs("RollWidth")), "", rs("RollWidth"))
				txtMatSpecs.Text = IIf(IsDBNull(rs("MaterialSpecs")), "", rs("MaterialSpecs"))
				txtBagDimension.Text = IIf(IsDBNull(rs("BagDimension")), "", rs("BagDimension"))

				'txtNetWeight.Text = IIf(IsDBNull(rs("NetWeight")), "", rs("NetWeight")) 
				'cmbCustomer.SelectedValue = IIf(IsDBNull(rs("Customer_Cd")), "99", rs("Customer_Cd"))
				'cmbSupplier.SelectedValue = IIf(IsDBNull(rs("Supplier_Cd")), "99", rs("Supplier_Cd")) 
				'cmbUomWeight.SelectedValue = IIf(IsDBNull(rs("WeightUOM_Cd")), "99", rs("WeightUOM_Cd")) 
				'Session("vOldRecord") += "ItemCd=" & rs("Item_Cd") & _
				'    "|Item Name=" & rs("ItemName") & _
				'    "|Asset=" & rs("AssetCd") & _
				'    "|Category=" & rs("CatgCd") & _
				'    "|Brand=" & rs("BrndCd") & _
				'    "|Status=" & rs("StatusCd") & _
				'    "|Varince=" & rs("VarnCd") & _
				'    "|Size=" & rs("SizeCd") & _
				'    "|Warehouse=" & rs("WhseCd") & _
				'    "|Location=" & rs("LocCd") & _
				'    "|Location Col=" & rs("Loc_Col") & _
				'    "|Location Row=" & rs("Loc_Row")

				rs.Close()
				c.Close()
				c.Dispose()
				cm.Dispose()

			End If
		Catch ex As SqlClient.SqlException
			Response.Write("Error in retrieving data. " & ex.Message)
		End Try

	End Sub

	Private Sub SaveModule()
		Dim c As New SqlClient.SqlConnection(connStr)
		Dim cm As New SqlClient.SqlCommand
		'Dim rs As SqlClient.SqlDataReader

		Dim vFields As String = ""
		Dim vFData As String = ""
		Dim vNewRecord As String = ""
		Dim vSQL As String = ""

		If txtItemCd.Text.Trim = "" Then
			lblErrorMsg.Visible = True
			lblErrorMsg.Text = "Item Code is required"
			Exit Sub
		ElseIf txtDescr1.Text.Trim = "" Then
			lblErrorMsg.Visible = True
			lblErrorMsg.Text = "Item Description is required"
			Exit Sub
		End If
		lblErrorMsg.Visible = False

		cm.Connection = c
		c.Open()

		If txtItemCd.Text.Trim <> Request.Item("pItemCd") Then
			vSQL = "select count(Item_Cd) as vTranId from item_master where Item_Cd='" & txtItemCd.Text.Trim & "'"

			If GetRef(vSQL, txtItemCd.Text.Trim) > 0 Then
				vScript = "alert('Item Code is already in use.');"
				c.Close()
				c.Dispose()
				cm.Dispose()
				Exit Sub
			End If
		End If

		If Request.Item("pMode") = "new" Then
			vSQL = "insert into item_master (Item_Cd,Descr,Descr1,Barcode,SerialNo,CurrCost," _
				& "MinOrderQty,MinOrderQtyUOM_Cd,ProdLeadDays,ProdLeadTime," _
				& "RlseLeadDays,RlseLeadTime," _
				& "ItemType_Cd,Source,ItemClass_Cd," _
				& "QtyUOM_Cd,IsActive,ModifyBy,DateModify,CoreWeight,MaterialSpecs,RollWidth,RepeatLenght,BagDimension) values ('" _
				& txtItemCd.Text.Trim & "','" & txtDescr1.Text.Trim.Replace("'", """") & "','" _
				& txtDescr2.Text.Trim.Replace("'", """") & "','" & txtBarcode.Text.Trim & "','" _
				& txtSerialNo.Text.Trim & "', '" & txtCost.Text.Trim & "','" & txtMinOrderQty.Text.Trim & "','" _
				& cmbMinOrderUom.SelectedValue & "','" & txtProdLeadTimeDays.Text.Trim & "','" _
				& txtProdLeadTimeHrs.Text.Trim & "','" & txtReleaseLeadTimeDay.Text.Trim & "','" _
				& txtReleaseLeadTimeHrs.Text.Trim & "','" & cmbItemType.SelectedValue & "','" _
				& cmbSource.SelectedValue & "','" & cmbTypeClass.SelectedValue & "','" _
				& cmbUomQty.SelectedValue & "'," & cmbStatus.SelectedValue & ",'" _
				& Session("uid") & "','" & Now() & "','" & txtCoreWeight.Text.Trim & "','" _
				& txtMatSpecs.Text.Trim & "','" & txtRollWidth.Text.Trim & "','" & txtRepeatLenght.Text.Trim & "','" & txtBagDimension.Text.Trim & "')"
		Else
			vSQL = "Update item_master Set " _
				& "Item_Cd='" & txtItemCd.Text.Trim & "', Descr='" & txtDescr1.Text.Trim.Replace("'", """") & "', " _
				& "Descr1='" & txtDescr2.Text.Trim.Replace("'", """") & "',Barcode='" & txtBarcode.Text.Trim _
				& "',SerialNo='" & txtSerialNo.Text.Trim & "', CurrCost='" & txtCost.Text.Trim & "',MinOrderQty='" & txtMinOrderQty.Text.Trim _
				& "',ProdLeadDays='" & txtProdLeadTimeDays.Text.Trim & "',ProdLeadTime='" & txtProdLeadTimeHrs.Text.Trim _
				& "',RlseLeadDays='" & txtReleaseLeadTimeDay.Text & "',RlseLeadTime='" & txtReleaseLeadTimeHrs.Text _
				& "',ItemType_Cd='" & cmbItemType.SelectedValue & "',Source='" & cmbSource.SelectedValue & "',MinOrderQtyUOM_Cd='" & cmbMinOrderUom.SelectedValue _
				& "',QtyUOM_Cd='" & cmbUomQty.SelectedValue & "',IsActive='" & cmbStatus.SelectedValue & "', CoreWeight='" & txtCoreWeight.Text.Trim _
				& "',ModifyBy='" & Session("uid") & "', DateModify='" & Now() & "', ItemClass_Cd='" & cmbTypeClass.SelectedValue _
				& "',MaterialSpecs='" & txtMatSpecs.Text.Trim & "', RollWidth='" & txtRollWidth.Text.Trim & "', RepeatLenght='" & txtRepeatLenght.Text.Trim _
				& "',BagDimension='" & txtBagDimension.Text.Trim & "' " _
				& "where Item_Cd='" & Request.Item("pItemCd").Trim & "'"
		End If

		cm.CommandText = vSQL
		'Response.Write(vSQL)
		Try
			cm.ExecuteNonQuery()
			vScript = "alert('Successfully saved.'); window.opener.document.form1.h_Mode.value='reload'; window.opener.document.form1.submit();" 'window.close();"
			Server.Transfer("item_catalog.aspx?pItemCd=" & txtItemCd.Text & "&pDescr1=" & txtDescr1.Text & "&pDescr2=" & txtDescr2.Text & "")
		Catch ex As DataException
			vScript = "alert('An error occurred while trying to Save the new record.');"
		End Try
		c.Close()
		c.Dispose()
		cm.Dispose()

		' ===============================================================================================================================
		' CREATE RECORD OR AUDIT LOGS 
		' ===============================================================================================================================
		'vNewRecord = "ItemCd=" & txtItemCd.Text.Trim & _
		'    "|Item Name=" & txtItemName.Text.Trim & _
		'    "|Category=" & cmbCat.SelectedValue

		'"|Brand=" & cmbBrand.SelectedValue & _
		'"|Status=" & cmbStatus.SelectedValue & _
		'"|Varince=" & cmbVar.SelectedValue & _
		'"|Size=" & cmbSize.SelectedValue & _
		'"|Warehouse=" & cmbWare.SelectedValue & _
		'"|Location=" & cmbLoc.SelectedValue & _
		'"|Location Col=" & txtLocCol.Text.Trim & _
		'"|Location Row=" & txtLocRow.Text.Trim
		'"|Asset=" & cmbAsset.SelectedValue & _

		'EventLog(Session("uid"), Request.ServerVariables("REMOTE_ADDR"), IIf(Request.Item("mode") = "view", "EDIT", "ADD"), _
		'    Session("vOldRecord"), vNewRecord, "Item Code : " & txtItemCd.Text.Trim & _
		'    " was " & IIf(Request.Item("mode") = "view", "modify", "added"), "Item Master", c)

		'divErrorDis.Visible = True
		'divErrorDis.Style.Value = "border: solid 2px #46af0d; background:#d0fc9d;"
		'vErrorMsg = "Record successfully Save."

		'If txtSave.Text = "Submit" Then
		'    GetInfo()
		'End If 
	End Sub

	Protected Sub txtClose_Click(sender As Object, e As EventArgs) Handles txtClose.Click

		Select Case txtClose.Text
			Case "Cancel"
				txtClose.Visible = False
				cmdSave.Text = "Edit"
				GetInfo()

			Case Else
				Session.Remove("vOldRecord")
				vScript = "window.close();"
		End Select

	End Sub

	Protected Sub cmdSave_Click(sender As Object, e As EventArgs) Handles cmdSave.Click
		SaveModule()
	End Sub
End Class
