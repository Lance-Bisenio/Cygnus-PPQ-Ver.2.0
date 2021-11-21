Imports denaro
Partial Class joborder
    Inherits System.Web.UI.Page
    Public vScript As String = ""
    Dim vStdQty As Decimal
    Dim vStdProdRun As Decimal
    Dim vReportTo As String = ""


    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Session("uid") = Nothing Or Session("uid") = "" Then
            vScript = "alert('Login session has expired. Please re-login again.');  window.close();" 'window.opener.document.form1.submit();
            Exit Sub
        End If

        If Not IsPostBack Then
            'txtTranDate.Text = Format(Now, "MM/dd/yyyy")
            cmbHrs.Items.Clear()
            For iCtr = 0 To 23
                cmbHrs.Items.Add(Format(iCtr, "00"))
            Next iCtr
            cmbHrs.SelectedValue = ""

            cmbMin.Items.Clear()
            For iCtr = 0 To 59
                cmbMin.Items.Add(Format(iCtr, "00"))
            Next iCtr
            cmbMin.SelectedValue = ""

            Dim c As New SqlClient.SqlConnection(connStr)
            Dim cm As New SqlClient.SqlCommand
            Dim rs As SqlClient.SqlDataReader

            Try
                c.Open()
            Catch ex As SqlClient.SqlException
                vScript = "alert('Error occurred while trying to connect to database. Error code 101; Error is: " & ex.Message.Replace(vbCrLf, "").Replace("'", "") & "');"
                c.Dispose()
                cm.Dispose()
                Exit Sub
            End Try
            cm.Connection = c

            '' ===================================================================================================================================================
            '' == 08.13.2014 == GET THE BOM DETAILS BASE ON BOM CODE AND BOM REVISION NUMBER
            '' ===================================================================================================================================================
            cm.CommandText = "select BOM_Cd,Revision,ActiveBy,DateActive,Item_Cd," _
                & "(select Descr from ref_item_status where ref_item_status.Status_Cd=bom_header.Status_Cd ) as Status_Cd," _
                & "(select Descr from item_master where item_master.Item_Cd=bom_header.Item_Cd) as Item_Name, " _
                & "(select Descr from ref_item_type where ref_item_type.Type_Cd=bom_header.ItemType_Cd) as ItemType, ItemType_Cd, " _
                & "(select Fullname from user_list where User_id=bom_header.CreatedBy) as CreatedBy," _
                & "(select Descr from ref_item_uom where ref_item_uom.UOM_Cd=bom_header.ItemUOM) as vUOM,ItemUOM," _
                & "(select MinOrderQty from item_master where item_master.Item_Cd=bom_header.Item_Cd) as StdQty," _
                & "(select Descr from ref_item_uom where ref_item_uom.UOM_Cd=bom_header.StdQtyUOM) as vStdOrderUOM," _
                & "(select Descr from ref_item_uom where ref_item_uom.UOM_Cd=bom_header.NetWeightUOM) as vWeightUOM, NetWeight," _
                & "cast(StdProdRunDay as varchar(100)) +' / '+ cast(StdProdRunTime as varchar(5)) as StdProdRun, ReportTo,DateCreated from " _
                & "bom_header Where Bom_Cd='" & Request.Item("pBOM") & "' and Revision='" & Request.Item("pBomRev") & "' Order by BOM_Cd, Revision"

            'Response.Write(cm.CommandText)
            Try
                rs = cm.ExecuteReader
                If rs.Read Then
                    txtItemCd.Text = rs("Item_Cd")
                    txtItemName.Text = rs("Item_Name")
                    txtBOMCd.Text = rs("BOM_Cd")
                    txtBOMRev.Text = rs("Revision")

                    txtItemType.Text = rs("ItemType")
                    txtItemUOM.Text = rs("vUOM")
                    txtMinOrder.Text = rs("StdQty")
                    txtQty.Text = rs("StdQty")

                    h_ItemType.value = rs("ItemType_Cd")
                    h_UOMCd.Value = rs("ItemUOM")

                End If
                rs.Close()
            Catch ex As SqlClient.SqlException
                vScript = "alert('Error occurred while trying to retrieve BOM details; Error is: " & _
                    ex.Message.Replace(vbCrLf, "").Replace("'", "") & "');"
                c.Close()
                c.Dispose()
                cm.Dispose()
                Exit Sub
            End Try

            BuildCombo("select Acct_Cd, (select Descr from ref_item_customer where Acct_Cd=Customer_Cd) as vDescr " & _
                                   "from ref_item_catalog where Item_Cd='" & txtItemCd.Text & "' order by vDescr", cmbCustomerName)
            cmbCustomerName.Items.Add("")
            cmbCustomerName.SelectedValue = ""

            BuildCombo("select Status_Cd, Descr from ref_item_status where GroupName='JO' order by Descr", cmbStatus)
            cmbStatus.SelectedValue = "PRE-PLAN"
            txtDueDate.Text = Format(Now, "MM/dd/yyyy")

            '' ===================================================================================================================================================
            '' == 08.13.2014 == IF THE JO IS ALREADY EXIST IN JO TABLE. THE PROCESS WILL READ THIS TO GET THE JOB ORDER DETAILS NOT IN BOM TABLE
            '' ===================================================================================================================================================
            If Request.Item("pTranId") <> "" Then
                cm.CommandText = "select StartDateTime,OrderQty,Cust_Cd,Alt_Cd,DueDate," _
                    & "JO_Status,StartDate,JobOrderNo,SalesOrderNo,PurchaseOrderNo," _
                    & "ReleaseDate,Remarks " _
                    & "from jo_header where TranId=" & Request.Item("pTranId")

                'Response.Write(cm.CommandText)
                Try
                    rs = cm.ExecuteReader
                    If rs.Read Then

                        txtCustomerCd.Text = rs("Cust_Cd")
                        cmbCustomerName.SelectedValue = rs("Cust_Cd")
                        txtItemCatalog.Text = rs("Alt_Cd")

                        cmbStatus.SelectedValue = rs("JO_Status")
                        OrderStatusFunction()

                        txtDueDate.Text = Format(rs("DueDate"), "MM/dd/yyyy")
                        h_DueDate.Value = Format(rs("DueDate"), "MM/dd/yyyy")

                        If Not IsDBNull(rs("StartDate")) Then
                            txtSDate.Text = Format(rs("StartDate"), "MM/dd/yyyy")
                            h_SDate.Value = Format(rs("StartDate"), "MM/dd/yyyy")
                        End If

                        If Not IsDBNull(rs("ReleaseDate")) Then
                            txtReleaseDate.Text = Format(rs("ReleaseDate"), "MM/dd/yyyy")
                            h_ReleaseDate.Value = Format(rs("ReleaseDate"), "MM/dd/yyyy")
                        End If

                        If Not IsDBNull(rs("StartDateTime")) Then
                            cmbHrs.SelectedValue = Format(rs("StartDateTime"), "HH")
                            cmbMin.SelectedValue = Format(rs("StartDateTime"), "mm")
                        End If

                        'txtSDate.Text = IIf(IsDBNull(rs("StartDate")), "", Format(rs("StartDate"), "MM/dd/yyyy"))
                        'txtReleaseDate.Text = IIf(IsDBNull(rs("ReleaseDate")), "", Format(rs("ReleaseDate"), "MM/dd/yyyy"))

                        txtJONo.Text = IIf(IsDBNull(rs("JobOrderNo")), "", rs("JobOrderNo"))
                        txtSONo.Text = IIf(IsDBNull(rs("SalesOrderNo")), "", rs("SalesOrderNo"))
                        txtPONo.Text = IIf(IsDBNull(rs("PurchaseOrderNo")), "", rs("PurchaseOrderNo"))
                        txtReason.Text = IIf(IsDBNull(rs("Remarks")), "", rs("Remarks"))
                        txtQty.Text = IIf(IsDBNull(rs("OrderQty")), "", rs("OrderQty"))

                        'txtMeter.Text = IIf(IsDBNull(rs("Kilos")), "", rs("Kilos"))
                        'txtKilos.Text = IIf(IsDBNull(rs("Meter")), "", rs("Meter"))

                    End If
                    rs.Close()
                Catch ex As SqlClient.SqlException
                    vScript = "alert('Error occurred while trying to retrieve Job Order Info.Error is: " & _
                        ex.Message.Replace(vbCrLf, "").Replace("'", "") & "');"
                    c.Close()
                    c.Dispose()
                    cm.Dispose()
                    Exit Sub
                End Try
            End If

            c.Close()
            c.Dispose()
            cm.Dispose()


        End If

    End Sub
      
    Protected Sub cmbCustomerName_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbCustomerName.SelectedIndexChanged
        txtItemCatalog.Text = GetRef("select Alt_Cd from ref_item_catalog where Acct_Cd='" & cmbCustomerName.SelectedValue & "' and Item_Cd='" & txtItemCd.Text & "' and AcctType='CUSTOMER'", "None")
        txtCustomerCd.Text = cmbCustomerName.SelectedValue 
    End Sub

    Protected Sub cmdSave_Click(sender As Object, e As EventArgs) Handles cmdSave.Click
        Dim vJONO As Integer = 0
        Dim vSONO As Integer = 0
        Dim vPONO As Integer = 0

        If cmbCustomerName.SelectedValue = "" Then
            vScript = "alert('Please select Customer Name.')"
            Exit Sub
        End If

        Select Case cmbStatus.SelectedValue
            Case "PRE-PLAN"
                txtSDate.Text = ""
                txtReleaseDate.Text = ""
                If txtDueDate.Text = "" Then
                    vScript = "alert('Please enter Due Date.')"
                    Exit Sub
                End If
            Case "PLANNING"
                txtReleaseDate.Text = ""
                If txtDueDate.Text = "" Then
                    vScript = "alert('Please enter Due Date.')"
                    Exit Sub
                End If
                If txtSDate.Text = "" Then
                    vScript = "alert('Please enter Start Date.')"
                    Exit Sub
                End If
                If txtSDate.Text.Trim <> "" Then
                    If txtJONo.Text.Trim = "" Then
                        vScript = "alert('Please enter Job Order No.')"
                        Exit Sub
                    End If
                End If
            Case "RELEASE"
                If txtDueDate.Text = "" Then
                    vScript = "alert('Please enter Due Date.')"
                    Exit Sub
                End If
                If txtSDate.Text = "" Then
                    vScript = "alert('Please enter Start Date.')"
                    Exit Sub
                End If
                If txtSDate.Text.Trim <> "" Then
                    If txtJONo.Text.Trim = "" Then
                        vScript = "alert('Please enter Job Order No.')"
                        Exit Sub
                    End If
                End If
                If txtReleaseDate.Text = "" Then
                    vScript = "alert('Please enter Release Date.')"
                    Exit Sub
                End If
                If txtReleaseDate.Text.Trim <> "" Then
                    If txtSDate.Text.Trim = "" Then
                        vScript = "alert('Please enter Release Date.')"
                        Exit Sub
                    End If
                End If
            Case "CANCEL"
                If txtReason.Text.Trim = "" Then
                    vScript = "alert('Please enter Reason');"
                    Exit Sub
                End If
            Case "HOLD"
                If txtReason.Text.Trim = "" Then
                    vScript = "alert('Please enter Reason');"
                    Exit Sub
                End If
        End Select

        If Request.Item("pTranId") = "" Then
            vJONO = GetRef("select count(JobOrderNo) as JONO from jo_header where JobOrderNo='" & txtJONo.Text & "' ", "")
            vPONO = GetRef("select count(SalesOrderNo) as SONO from jo_header where SalesOrderNo='" & txtPONo.Text & "' ", "")
            vSONO = GetRef("select count(PurchaseOrderNo) as PONO from jo_header where PurchaseOrderNo='" & txtSONo.Text & "' ", "")

            If vJONO > 0 Then
                vScript = "alert('Job Order Number already exists. \nPlease check again.');"
                Exit Sub
            End If

            If vPONO > 0 Then
                vScript = "alert('Purchase Order Number already exists. \nPlease check again.');"
                Exit Sub
            End If

            If vSONO > 0 Then
                vScript = "alert('Sales Order Number already exists. \nPlease check again.');"
                Exit Sub
            End If
        End If

        Save()

        'If Request.Item("pTranId") <> "" Then
        '    vScript = "alert('Successfully saved.'); window.opener.document.form1.submit(); window.close();   "
        'Else
        '    vScript = "alert('Successfully saved.'); window.opener.document.form1.h_Mode.value='Backto_Jo'; window.opener.document.form1.submit(); window.close(); "
        'End If

        If cmbStatus.SelectedValue = "PRE-PLAN" Or cmbStatus.SelectedValue = "JOCOMPLETED" Then
            vScript = "alert('Successfully saved.'); window.close();" ' window.opener.document.form1.submit(); 
        Else
            Dim vTranID As Integer = 0
            vTranID = GetRef("select TranId from jo_header where BOM_Cd='" & Request.Item("pBom") & "' and BOMRev='" & Request.Item("pBomRev") & "' and JobOrderNo='" & txtJONo.Text.Trim & "' ", 0)
            Server.Transfer("jomachine.aspx?pTranID=" & vTranID)
        End If

    End Sub

    Private Sub Save()
        Dim vSQL As String = ""
        Dim vJobOrderNo As String = IIf(txtJONo.Text = "", "NULL", "'" & txtJONo.Text & "'")
        Dim vSaleOrderNo As String = IIf(txtSONo.Text = "", "NULL", "'" & txtSONo.Text & "'")
        Dim vPurchaseOrderNo As String = IIf(txtPONo.Text = "", "NULL", "'" & txtPONo.Text & "'")
        Dim vStartDate As String = IIf(txtSDate.Text = "", "NULL", "'" & txtSDate.Text & "'")
        Dim vReleaseDate As String = IIf(txtReleaseDate.Text = "", "NULL", "'" & txtReleaseDate.Text & "'")
        Dim vStartTime As String = Format(Now(), "MM/dd/yyyy") & " " & cmbHrs.SelectedValue & ":" & cmbMin.SelectedValue
        Dim vJOStatus As String = ""

        'vJO = GetMaxCount("select max(JONo) as vJONo from jo_header ")

        Dim c As New SqlClient.SqlConnection(connStr)
        Dim cm As New SqlClient.SqlCommand
        cm.Connection = c
        c.Open()

        If cmbStatus.SelectedValue = "JOCOMPLETED" Then
            vJOStatus = ",ProdStatus='" & cmbStatus.SelectedValue & "',ProdCreatedBy='" & Session("uid") & "',ProdDateCreated='" & Now & "' "
        End If

        If Request.Item("pTranId") <> "" Then
			vSQL = "update jo_header Set Alt_Cd='" & txtItemCatalog.Text _
				& "',Cust_Cd='" & cmbCustomerName.SelectedValue & "',OrderQty='" & txtQty.Text _
				& "',JobOrderNo=" & vJobOrderNo & ",SalesOrderNo=" & vSaleOrderNo _
				& ",PurchaseOrderNo=" & vPurchaseOrderNo & ",JO_Status='" & cmbStatus.SelectedValue _
				& "',DueDate='" & txtDueDate.Text & "',StartDate=" & vStartDate & ",StartDateTime='" & vStartTime _
				& "',ReleaseDate=" & vReleaseDate & ",CreatedBy='" & Session("uid") _
				& "',DateCreated='" & Format(CDate(Now), "MM-dd-yyyy") & "',Remarks='" & txtReason.Text & "' " _
				& vJOStatus _
				& "where TranId=" & Request.Item("pTranId")
		Else
			vSQL = "insert jo_header (Item_Cd,Alt_Cd,Cust_Cd,ItemType_Cd,UOM_Cd,MinQty,OrderQty," _
				& "BOM_Cd,BOMRev,JobOrderNo,SalesOrderNo,PurchaseOrderNo,JO_Status," _
				& "DueDate,StartDate,StartDateTime,ReleaseDate,CreatedBy,DateCreated,Remarks) values ('" _
				& txtItemCd.Text.Trim & "','" & txtItemCatalog.Text.Trim & "','" & cmbCustomerName.SelectedValue & "','" & h_ItemType.Value & "','" & h_UOMCd.Value & "','" & txtMinOrder.Text.Trim & "','" & txtQty.Text.Trim & "','" _
				& txtBOMCd.Text.Trim & "','" & txtBOMRev.Text.Trim & "'," & vJobOrderNo & "," & vSaleOrderNo & "," & vPurchaseOrderNo & ",'" & cmbStatus.SelectedValue & "','" _
				& txtDueDate.Text.Trim & "'," & vStartDate.Trim & ",'" & vStartTime.Trim & "'," & vReleaseDate & ", '" & Session("uid") & "','" & Format(CDate(Now), "MM-dd-yyyy") & "','" & txtReason.Text.Trim & "'" _
				& ")"
		End If

        cm.CommandText = vSQL
        'Response.Write(vSQL)
        'Exit Sub
        Try
            cm.ExecuteNonQuery()
        Catch ex As DataException
            vScript = "alert('An error occurred while trying to Save the new record.');"
        End Try
        c.Close()
        c.Dispose()
        cm.Dispose()

    End Sub
 
    Public Sub OrderStatusFunction()
        Select Case cmbStatus.SelectedValue
            Case "PRE-PLAN"
                txtDueDate.Enabled = True
                txtSDate.Enabled = False
                txtReleaseDate.Enabled = False

				txtDueDate.Text = IIf(h_DueDate.Value <> "", h_DueDate.Value, Format(CDate(Now), "MM-dd-yyyy"))
				txtSDate.Text = ""
                txtReleaseDate.Text = ""
            Case "PLANNING"
                txtDueDate.Enabled = True
                txtSDate.Enabled = True
                txtReleaseDate.Enabled = False

				txtDueDate.Text = IIf(h_DueDate.Value <> "", h_DueDate.Value, Format(CDate(Now), "MM-dd-yyyy"))
				txtSDate.Text = IIf(h_SDate.Value <> "", h_SDate.Value, Format(CDate(Now), "MM-dd-yyyy"))
				txtReleaseDate.Text = ""
            Case "RELEASE"
                txtDueDate.Enabled = True
                txtSDate.Enabled = True
                txtReleaseDate.Enabled = True

				txtDueDate.Text = IIf(h_DueDate.Value <> "", h_DueDate.Value, Format(CDate(Now), "MM-dd-yyyy"))
				txtSDate.Text = IIf(h_SDate.Value <> "", h_SDate.Value, Format(CDate(Now), "MM-dd-yyyy"))
				txtReleaseDate.Text = IIf(h_ReleaseDate.Value <> "", h_ReleaseDate.Value, Format(CDate(Now), "MM-dd-yyyy"))
			Case "JOCOMPLETED"

            Case Else
                txtDueDate.Enabled = False
                txtSDate.Enabled = False
                txtReleaseDate.Enabled = False

                txtDueDate.Text = ""
                txtSDate.Text = ""
                txtReleaseDate.Text = ""
        End Select
    End Sub

    Protected Sub cmbStatus_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbStatus.SelectedIndexChanged
        OrderStatusFunction()
    End Sub
End Class

'Protected Sub txtSearch_Click(sender As Object, e As EventArgs) Handles txtSearch.Click
'    'Save()
'End Sub
