Imports denaro
Imports item_details
Partial Class inventory_materials_movement
    Inherits System.Web.UI.Page
    Public vScript As String = ""
    Dim vSQL As String = ""
    Dim vSecList As String = ""
    Dim vProcessCd As String = ""


    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        If Session("uid") = "" Then
            vScript = "alert('Login session has expired. Please re-login again.'); window.close();"
        End If

        If Not IsPostBack Then
            Dim vMode As String = Request.Item("pMode").Trim

            cmbMaterials.Items.Add("")
            cmbMaterials.SelectedValue = ""

            If vMode = "EditItem" Then
                'GetReceived_itemInfo()
                cmdDelete.Visible = True
                lblCurrQty.Visible = False
            End If

            GetSectionList() 'Proc_Cd
            vSQL = "select OperOrder," _
                & "CONVERT(varchar(10), OperOrder)+ ' = ' +(select Descr from ref_item_process where ref_item_process.Proc_Cd=bom_process.Proc_Cd) As Descr " _
                & "from  bom_process " _
                & "where BOM_Cd='" & Request.Item("pBOM") & "' and " _
                    & "Revision='" & Request.Item("pBOMRev") & "' and " _
                    & "Sect_Cd in (" & vSecList & ") and " _
                    & "Proc_Cd<>'3002' " _
                    & "order by OperOrder"

            'Response.Write(vSQL)
            BuildCombo(vSQL, cmbProcess)
            cmbProcess.Items.Add("Select Process")
            cmbProcess.SelectedValue = "Select Process"

            vSQL = "select Item_Cd, Descr from item_master where ItemType_Cd not in ('FG','SFG','99') order by Descr"
            BuildCombo(vSQL, cmbAltItem)
            cmbAltItem.Items.Add("Select Alternative Item")
            cmbAltItem.SelectedValue = "Select Alternative Item"

            If Request.Item("pMode") = "EditItem" Then
                cmbProcess.SelectedValue = Request.Item("pOrderNo")

                lblOrderNo.Text = cmbProcess.SelectedValue
                lblBOM.Text = Request.Item("pBOM") & " | " & Request.Item("pBOMRev")
                lblJONO.Text = Request.Item("pJO")

                GetMarialList()
                cmbMaterials.SelectedValue = Request.Item("pItemCode")
                lblRWCode.Text = Request.Item("pItemCode")


                If Request.Item("pIsAltItem") = 1 Then
                    cmbAltItem.SelectedValue = Request.Item("pItemCode")
                End If

                GetRawmats_details("")
                txtQty.Text = Request.Item("pQty")
                txtRollNuber.Text = Request.Item("pRollNo")

                cmbItemLotNo.SelectedValue = Request.Item("pLot")

                'vSQL = "select LotNo from prod_rawmaterial where TranId=" & Request.Item("pTranId")
                'GetRef(vSQL, "")
                ' GetQtyPer_Lotno()

                'vSQL = "select sum(Qty) as vQty from item_inv where " _
                '    & "item_cd='" & cmbMaterials.SelectedValue & "' and " _
                '    & "LotNo='" & cmbItemLotNo.SelectedValue & "' "
                'lblCurrQty2.Text = GetRef(vSQL, "")

                cmdSave.Visible = False
                cmdEdit.Visible = True
                cmdDelete.Visible = True
                'Response.Write(vSQL)
                'GetRawmatsLotno_Details()

            End If

        End If

    End Sub
    Private Sub GetQtyPer_Lotno()
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

        vSQL = "select sum(Qty) as vQty from item_inv where " _
                    & "item_cd='" & cmbMaterials.SelectedValue & "' and " _
                    & "LotNo='" & cmbItemLotNo.SelectedValue & "' "

        cm.CommandText = vSQL
        Try
            rs = cm.ExecuteReader
            If rs.Read Then

            End If
            rs.Close()

        Catch ex As SqlClient.SqlException
            vScript = "alert('Error occurred while trying to get reference. Error is: " _
                & ex.Message.Replace(vbCrLf, "").Replace("'", "") & "');"
        End Try

        c.Close()
        cm.Dispose()
        c.Dispose()

    End Sub
    Protected Sub cmbMaterials_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbMaterials.SelectedIndexChanged
        cmbAltItem.SelectedValue = "Select Alternative Item"
        GetRawmats_details("Default")
        Session("IsAltItem") = 0
    End Sub

    Protected Sub cmbcmbAltItem_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbAltItem.SelectedIndexChanged

        If cmbProcess.SelectedItem.Text = "Select Process" Then
            vScript = "alert('Please select process to dispatch materials.');"
            Exit Sub
        End If

        cmbMaterials.SelectedValue = "Select Material"
        GetRawmats_details("AltItem")
        Session("IsAltItem") = 1
    End Sub

    Private Sub GetRawmats_details(pItemType As String)
        Dim vTempCost As String = ""
        Dim vCost As Int64 = 0
        Dim vSelectedValue As String = ""

        If pItemType = "AltItem" Or Request.Item("pIsAltItem") = 1 And pItemType = "" Then
            vSelectedValue = cmbAltItem.SelectedValue
        Else
            vSelectedValue = cmbMaterials.SelectedValue
        End If

        Session("ItemType") = vSelectedValue
        txtItemCd.Text = vSelectedValue
        lblRWCode.Text = vSelectedValue

        vSQL = "select distinct(LotNo) as vLotNo, LotNo from item_inv " _
                & "where Item_Cd='" & vSelectedValue & "' and " _
                & "LotNo <>'' and LotNo<>'-' and LotNo is not null"

        BuildCombo(vSQL, cmbItemLotNo)

        cmbItemLotNo.Items.Add("Select Lot Number")
        cmbItemLotNo.SelectedValue = "Select Lot Number"

        vSQL = "Select Sum(Qty) from item_inv where item_Cd='" _
            & vSelectedValue & "' "
        vTempCost = GetRef(vSQL, 0)

        vCost = IIf(vTempCost = "null", 0, vTempCost)

        lblCurrQty.Text = Format(vCost, "###,###,##0.00")

        vSQL = "select TOP 1 UOM from item_inv where " _
            & "item_Cd ='" & vSelectedValue & "' "
        lblUOM.Text = GetRef(vSQL, "KGS")

        Select Case lblUOM.Text
            Case "KGS.", "KG(S)", "KG(S).", "null"
                lblUOM.Text = "KGS"
            Case "LITERS", "LITER(S)"
                lblUOM.Text = "LITERS"
        End Select

        vScript = "$('#txtQty').focus();"
    End Sub
    Protected Sub cmbMaterials0_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbItemLotNo.SelectedIndexChanged
        GetRawmatsLotno_Details()
    End Sub
    Private Sub GetRawmatsLotno_Details()
        If cmbItemLotNo.SelectedValue <> "Select Lot Number" Then
            Dim vCurrQty As String = 0
            vSQL = "select sum(Qty) as vQty from item_inv where " _
                                     & "item_cd='" & cmbMaterials.SelectedValue & "' and " _
                                     & "LotNo='" & cmbItemLotNo.SelectedValue & "' "
			'Response.Write(vSQL)
			vCurrQty = GetRef(vSQL, 0) 'IIf(IsDBNull(GetRef(vSQL, 0)), 0, GetRef(vSQL, 0))
			lblCurrQty.Text = vCurrQty

            vSQL = "select Cost from item_inv where item_Cd='" _
                & cmbMaterials.SelectedValue & "' and LotNo='" & cmbItemLotNo.SelectedValue & "' order by TranId desc"
            'SSResponse.Write(vSQL)
            lblCost.Text = GetRef(vSQL, "0.00")

        End If
        vScript = "$('#txtQty').focus();"
    End Sub
    Private Sub GetSectionList()
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
        vSQL = "select distinct(Sect_Cd) as vSection," _
                & "(select Descr from ref_emp_section where ref_emp_section.Section_Cd=bom_process.Sect_Cd) As Descr " _
                & "from  bom_process " _
                & "where BOM_Cd='" & Request.Item("pBOM") & "' and " _
                & "Revision='" & Request.Item("pBOMRev") & "' and " _
                & "Proc_Cd<>'3002' " _
                & "order by Descr"

        cm.CommandText = vSQL
        Try
            rs = cm.ExecuteReader
            Do While rs.Read
                vSecList += "'" & rs(0).ToString.Trim & "',"
            Loop
            rs.Close()
            vSecList = Mid(vSecList, 1, vSecList.Length - 1)

        Catch ex As SqlClient.SqlException
            vScript = "alert('Error occurred while trying to buil shift reference. Error is: " _
                & ex.Message.Replace(vbCrLf, "").Replace("'", "") & "');"
        End Try

        c.Close()
        cm.Dispose()
        c.Dispose()

    End Sub
    Protected Sub cmdSave_Click(sender As Object, e As EventArgs) Handles cmdSave.Click
        Save("New")
    End Sub
    Protected Sub cmdEdit_Click(sender As Object, e As EventArgs) Handles cmdEdit.Click
        Save("Edit")
    End Sub
    Protected Sub cmdDelete_Click(sender As Object, e As EventArgs) Handles cmdDelete.Click
        Save("Delete")
    End Sub
    Private Sub Save(pMode As String)


        If Session("uid") = "" Then
            vScript = "alert('Login session has expired. Please re-login again.'); window.close();"
        End If

        If cmbMaterials.SelectedItem.Text = "Select Material" And cmbAltItem.SelectedItem.Text = "Select Alternative Item" Then
            vScript = "alert('Please select Material Description to dispatch.');"
            Exit Sub
        End If

        If cmbItemLotNo.SelectedValue = "Select Lot Number" Then
            vScript = "alert('Please select Lot Number.');"
            Exit Sub
        End If

        If txtQty.Text.Trim = "" Then
            vScript = "alert('Please enter valid quantity.');"
            Exit Sub
        End If

        Dim vBOM As Integer = Request.Item("pBOM")
        Dim vBOMRev As Integer = Request.Item("pBOMRev")
        Dim vJO As String = Request.Item("pJO")
        Dim vOrderNo As Integer = lblOrderNo.Text
        Dim vSectCd As String = ""
        Dim vBatcNo As String = Format(Now, "MMddyyyyhhmmss")
        Dim vLotNum As String = cmbItemLotNo.SelectedValue
        Dim vTotalCost As Decimal = 0

        If cmbItemLotNo.SelectedValue = "Select Lot Number" Then
            vLotNum = "-"
        End If

        vProcessCd = txtProcessCode.Value

        vSQL = "select Sect_Cd from bom_process where " _
            & "BOM_Cd='" & Request.Item("pBOM") & "' and " _
            & "Revision='" & Request.Item("pBOMRev") & "' and " _
            & "Proc_Cd='" & vProcessCd & "' "
        vSectCd = GetRef(vSQL, "-")

        If pMode = "New" Then
            vSQL = "insert into item_inv (" _
                & "Item_Cd,SupplierBarcode,RefBarcode,Barcode,LotNo," _
                & "Qty,UOM,Cost,TranType,Remarks,CreatedBy,DateCreated,JobOrderNo) values ('"

            vSQL += txtItemCd.Text.Trim & "','','" & vBatcNo & "','" & txtItemCd.Text.Trim & "','" _
                & vLotNum & "','-" & txtQty.Text.Trim & "', '" & lblUOM.Text & "','" & lblCost.Text & "','" _
                & "WAREHOUSE to PRODUCTION','WAREHOUSE to PRODUCTION','" _
                & Session("uid") & "','" & Format(Now, "MM/dd/yyyy HH:mm") & "','" & vJO & "')"
        End If

        If pMode = "Delete" Then
            vSQL = "delete from item_inv where RefBarcode='" & Request.Item("pBatchNo") & "'"
        End If

        If pMode = "Edit" Then
            vSQL = "update item_inv set Qty='-" & txtQty.Text.Trim & "',LotNo='" & vLotNum & "' " _
                & "where RefBarcode='" & Request.Item("pBatchNo") & "'"
        End If

        CreateRecord(vSQL)

        If pMode = "New" Then

            vTotalCost = lblCost.Text.Trim * txtQty.Text.Trim

            vSQL = "insert into prod_rawmaterial (" _
                & "BatchNo,BOM,Revision,Sect_Cd,Proc_Cd,JONO,OperOrder," _
                & "Item_Cd,UOM,LotNo,Qty,Remarks,TranType,ItemCost,TotalCost," _
                & "ReleaseBy,DateRelease,CreatedBy,DateCreated,IsAltItem,RollNo) values (" _
                & "'" & vBatcNo & "','" & vBOM & "','" & vBOMRev & "','" & vSectCd & "'," _
                & "'" & vProcessCd & "','" & vJO & "','" & vOrderNo & "'," _
                & "'" & txtItemCd.Text.Trim & "','" & lblUOM.Text & "','" & vLotNum & "'," _
                & "'" & txtQty.Text.Trim & "'," _
                & "'RW WAREHOUSE to PRODUCTION','RW','" & lblCost.Text.Trim & "','" & Math.Floor(vTotalCost) & "'," _
                & "'" & Session("uid") & "','" & Format(Now, "MM/dd/yyyy HH:mm") & "'," _
                & "'" & Session("uid") & "','" & Format(Now, "MM/dd/yyyy HH:mm") & "'," & Session("IsAltItem") & "," _
                & "'" & txtRollNuber.Text.Trim & "')"
        End If

        If pMode = "Delete" Then
            vSQL = "delete from prod_rawmaterial where BatchNo='" & Request.Item("pBatchNo") & "'"
        End If







        ' ======================================================================================
        ' UPDATE RECORD 
        ' ======================================================================================
        If pMode = "Edit" Then
            vTotalCost = lblCost.Text.Trim * txtQty.Text.Trim

            If cmbAltItem.SelectedValue = "Select Alternative Item" Then
                Session("IsAltItem") = 0
            End If

            If cmbMaterials.SelectedValue = "Select Material" Then
                Session("IsAltItem") = 1
            End If

            vSQL = "update prod_rawmaterial set " _
                & "Item_Cd='" & txtItemCd.Text.Trim & "'," _
                & "Qty='" & txtQty.Text.Trim & "'," _
                & "LotNo='" & vLotNum & "'," _
                & "ItemCost='" & lblCost.Text.Trim & "'," _
                & "TotalCost='" & vTotalCost & "'," _
                & "IsAltItem='" & Session("IsAltItem") & "', " _
                & "RollNo='" & txtRollNuber.Text.Trim & "' " _
                & "where BatchNo='" & Request.Item("pBatchNo") & "'"
        End If

        CreateRecord(vSQL)
        ' ======================================================================================




        txtQty.Text = ""
        txtQty.Focus()



        Select Case pMode
            Case "New"
                vScript = "alert('Successfully Saved.'); window.opener.document.getElementById('h_Mode').value='Reload'; window.opener.document.forms['form1'].submit(); "
            Case "Delete"
                vScript = "alert('Successfully Deleted.'); window.opener.document.getElementById('h_Mode').value='Reload'; window.opener.document.forms['form1'].submit(); window.close();"
            Case "Edit"
                vScript = "alert('Successfully Saved.'); window.opener.document.getElementById('h_Mode').value='Reload'; window.opener.document.forms['form1'].submit(); window.close();"
        End Select

    End Sub
    Protected Sub cmbProcess_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbProcess.SelectedIndexChanged
        GetMarialList()
    End Sub
    Public Sub GetMarialList()
        If cmbProcess.SelectedValue = "Select Process" Then
            vScript = "alert('Please select process.');"
            cmbMaterials.Items.Clear()
            cmbMaterials.Items.Add("Select Material")
            cmbMaterials.SelectedValue = "Select Material"
            Exit Sub
        End If

        vSQL = "select Proc_Cd from bom_process " _
            & "where BOM_Cd=" & Request.Item("pBOM") & " and " _
            & "Revision=" & Request.Item("pBOMRev") & " and " _
            & "OperOrder=" & cmbProcess.SelectedValue
        txtProcessCode.Value = GetRef(vSQL, "-")
        vProcessCd = txtProcessCode.Value

        vSQL = "select TranId from bom_process " _
            & "where BOM_Cd=" & Request.Item("pBOM") & " and " _
            & "Revision=" & Request.Item("pBOMRev") & " and " _
            & "OperOrder=" & cmbProcess.SelectedValue
        Dim vBomMatParentId As String = GetRef(vSQL, 0)
		'Response.Write(vSQL)


		vSQL = "Select distinct(Item_Cd), " _
                    & "(Select Descr As Descr from item_master where item_master.Item_Cd=bom_materials.Item_Cd) As ItemDescr " _
                    & "from bom_materials where " _
                        & "BOM_Cd=" & Request.Item("pBOM") & " and " _
                        & "Revision=" & Request.Item("pBOMRev") & " and " _
                        & "Parent_TranId='" & vBomMatParentId & "' " _
                        & "order by ItemDescr "


		BuildCombo(vSQL, cmbMaterials)
        cmbMaterials.Items.Add("Select Material")
        cmbMaterials.SelectedValue = "Select Material"

        lblOrderNo.Text = cmbProcess.SelectedValue
        lblBOM.Text = Request.Item("pBOM") & " | " & Request.Item("pBOMRev")
        lblJONO.Text = Request.Item("pJO")
		lblRWCode.Text = "-"

		'GET MACHINE NAME BASED ON THE SELECTED PROCESS
		vSQL = "select " _
			& "(select Descr from ref_item_machine a where a.Mach_Cd = b.Mach_Cd) As MachName, Mach_Cd " _
			& "from jo_machine b " _
			& "where IsPrimary='YES' and JobOrderNo='" & Request.Item("pJO") & "' and OperOrder=" & cmbProcess.SelectedValue

		LblMachine.Text = GetRef(vSQL, "")

		vSQL = "select ItemCost from prod_rawmaterial where TranId=" & Request.Item("pTranId")
		lblCost.Text = GetRef(vSQL, 0)
	End Sub

End Class

