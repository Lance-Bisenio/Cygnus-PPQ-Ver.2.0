Imports denaro
Imports item_details


Partial Class item_inv
	Inherits System.Web.UI.Page
	Public vScript As String = ""
	Public vEntityId As String = ""
	Public vItemDetails As String = ""
	Public vEmpRecords As String = ""
	Public vFilter As String = ""

	Dim c As New SqlClient.SqlConnection
	Dim vColNames As String = ""
	Dim vColSource As String = ""
	Dim vTableSource As String = ""
    Dim vSQL As String = ""
    Public vData As String = ""

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
            If txtDateFrom.Text.Trim = "" Then
                txtDateFrom.Text = Format(Now, "MM/dd/yyyy")
            End If

            If txtDateTo.Text.Trim = "" Then
                txtDateTo.Text = Format(Now, "MM/dd/yyyy")
            End If


            BuildCombo("select Customer_Cd, Descr from ref_item_customer order by Descr ", CmdCustomerList)
            CmdCustomerList.Items.Add(" ")
            CmdCustomerList.SelectedValue = " "

            BuildCombo("select Item_Cd, Descr from item_master " _
                       & "where Item_Cd is not null and ItemType_Cd in ('FG') order by Descr", CmdProductList)
            CmdProductList.Items.Add(" ")
            CmdProductList.SelectedValue = " "

            TxtDateCreated.Text = Format(Now, "MM/dd/yyyy")

            GetPackListHdr()
        End If


    End Sub
    Private Sub GetPackListHdr()
        Dim c As New SqlClient.SqlConnection
        Dim da As SqlClient.SqlDataAdapter
        Dim ds As New DataSet

        c.ConnectionString = connStr
        vSQL = "select BatchNo,Cust_Cd,Item_Cd,IONumber,PONO, JONO,format(DateCreated,'MM/dd/yyyy') as DateCreated, " _
            & "(select Descr from ref_item_customer where a.Cust_Cd=Customer_Cd) As CustomerName, " _
            & "(select Descr from item_master b where a.Item_Cd=b.Item_Cd) As ItemName, " _
            & "(select Emp_FName+ ' ' +Emp_LName from emp_master c where a.CreatedBy=c.Emp_Cd) As CreatedBy " _
            & "from item_packinglist_hdr a " _
            & "order by ItemName"
        'Response.Write(vSQL)
        da = New SqlClient.SqlDataAdapter(vSQL, c)
        da.Fill(ds, "TblPackingListHeader")
        TblPackingListHeader.DataSource = ds.Tables("TblPackingListHeader")
        TblPackingListHeader.DataBind()

        'lblTotal.Text = "<b>Total Item Retrieved : " & tbl_ItemMaster.DataSource.Rows.Count & "</b>"

        da.Dispose()
        ds.Dispose()
    End Sub
    Private Sub BuildItemMasterList()
        Dim c As New SqlClient.SqlConnection
        Dim da As SqlClient.SqlDataAdapter
        Dim ds As New DataSet
        Dim vFilter As String = ""
        Dim vTableName As String = ""


        'vFilter = "where IsActive=" & IIf(cmbItemStatus.SelectedValue = "Deleted", 3, cmbItemStatus.SelectedValue)

        If txtSearch.Text <> "" Then
            'If cmbSearchBy.SelectedValue = "Item_GCAS" Then

            '	If SeachItem_GCAS(txtSearch.Text.Trim).Trim <> "" Then
            '		vFilter += " and Item_Cd like '%" & SeachItem_GCAS(txtSearch.Text.Trim).Trim & "%' "
            '	End If

            'Else
            '	vFilter += " and " & cmbSearchBy.SelectedValue & " like '%" & txtSearch.Text & "%' "
            'End If
        End If

        'If cmbSource.SelectedValue <> "All" Then
        '	vFilter += " and Source='" & cmbSource.SelectedValue & "' "
        'End If

        'If cmbItemType.SelectedValue <> "All" Then
        '	vFilter += " and ItemType_Cd='" & cmbItemType.SelectedValue & "' "
        'End If

        'If cmbTypeClass.SelectedValue <> "All" Then
        '	vFilter += " and ItemClass_Cd='" & cmbTypeClass.SelectedValue & "' "
        'End If

        'If cmbUOMQ.SelectedValue <> "All" Then
        '	vFilter += " and QtyUOM_Cd='" & cmbUOMQ.SelectedValue & "' "
        'End If
        'vFilter += " Order By Item_Cd asc"

        c.ConnectionString = connStr
        vSQL = "select Distinct(Item_Cd) as ItemCode, " _
            & "(select Descr from item_master b where a.Item_Cd=b.Item_Cd) As ItemName, " _
            & "(select RollWidth from item_master d where a.Item_Cd=d.Item_Cd) As vRollWidth, " _
            & "(select RepeatLenght from item_master d where a.Item_Cd=d.Item_Cd) as vRepeatLenght, " _
            & "(select BagDimension from item_master d where a.Item_Cd=d.Item_Cd) as vBagDimension, " _
            & "(select MaterialSpecs from item_master d where a.Item_Cd=d.Item_Cd) as vMaterialSpecs " _
            & "from item_inv a " _
            & "where Item_Cd in (select Item_Cd from item_master c where a.Item_Cd=c.Item_Cd and Descr is not null) and " _
            & "Remarks='WRAPPING-COMPLETION' and " _
            & "DateCreated between '" & txtDateFrom.Text.Trim & "' and '" & txtDateTo.Text.Trim & "'" _
            & vFilter _
            & "order by ItemName"

        'lblMessageBox.Text = vSQL
        'Exit Sub

        da = New SqlClient.SqlDataAdapter(vSQL, c)
        da.Fill(ds, "ItemMaster")
        tblItemMaster.DataSource = ds.Tables("ItemMaster")
        tblItemMaster.DataBind()

        'lblTotal.Text = "<b>Total Item Retrieved : " & tbl_ItemMaster.DataSource.Rows.Count & "</b>"

        da.Dispose()
        ds.Dispose()
    End Sub

    Private Sub tblItemMaster_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles tblItemMaster.PageIndexChanging
        tblItemMaster.PageIndex = e.NewPageIndex
        tblItemMaster.SelectedIndex = -1
        BuildItemMasterList()
	End Sub

    Protected Sub txtSearch_1_Click(sender As Object, e As EventArgs) Handles BtnSearch.Click
        h_ItemCd.Value = ""
        h_Mode.Value = ""
        GetPackListHdr()
        'BtnPackingList.Enabled = False
        TblPackingListHeader.SelectedIndex = -1

        BtnAddItem.Disabled = True
        BtnEditItem.Disabled = True
    End Sub

    Protected Sub tblItemMaster_SelectedIndexChanged(sender As Object, e As EventArgs) Handles tblItemMaster.SelectedIndexChanged
        'BtnPackingList.Enabled = False
        BuildJOList()
    End Sub

    Private Sub BuildJOList()


        Dim c As New SqlClient.SqlConnection(connStr)
        Dim cm As New SqlClient.SqlCommand
        Dim rs As SqlClient.SqlDataReader

        Dim cmsub As New SqlClient.SqlCommand
        Dim rssub As SqlClient.SqlDataReader
        Dim vCtr As Integer = 1
        Dim vQTY As Decimal = 0
        Dim vUOM As String = "KGS"
        Dim vParamList As String = ""

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
        cmsub.Connection = c

        vSQL = "select distinct(JobOrderNo) as vJONO " _
                  & "from item_inv a " _
                  & "where a.JobOrderNo is not null and Item_Cd='" & tblItemMaster.SelectedRow.Cells(2).Text & "'"

        cm.CommandText = vSQL
        rs = cm.ExecuteReader
        Do While rs.Read

            'Dim vBOM As String = Request.Item("pBOM")
            'Dim vRev As String = Request.Item("pBOMRev")
            'Dim vJO As String = Request.Item("pJO")
            'Dim vSect As String = Request.Item("pSection")
            'Dim vProc As String = Request.Item("pProcess")
            'Dim vSFG As String = Request.Item("pSFG")
            'Dim vOperOrder As String = Request.Item("pOperNo")


            vParamList = "pBOM=''&pBOMRev=''&pJO='" & rs("vJONO") & "'&pSection=''&pProcess=''&pSFG=''&pOperN''"

            vData += "<Tr class='RowLink' onclick='ShowDetails(""" & rs("vJONO") & """)'>"

            vData += "<td>" & vCtr & "</td>"
            vData += "<td>" & rs("vJONO") & "</td>"


            vSQL = "select Alt_Cd from jo_header where JobOrderNo='" & rs("vJONO") & "'"
            cmsub.CommandText = vSQL
            rssub = cmsub.ExecuteReader
            If rssub.Read Then
                vData += "<td>" & rssub(0) & "</td>"
            End If
            rssub.Close()
            '============================================================================================================
            vSQL = "select Count(LotNo) from item_inv where JobOrderNo='" & rs("vJONO") & "' and Remarks='WRAPPING-COMPLETION'"
            cmsub.CommandText = vSQL
            rssub = cmsub.ExecuteReader
            If rssub.Read Then
                vData += "<td>" & rssub(0) & "</td>"
            End If
            rssub.Close()
            '============================================================================================================
            vSQL = "select sum(GrossWeight) from item_inv where JobOrderNo='" & rs("vJONO") & "' and Remarks='WRAPPING-COMPLETION'"
            cmsub.CommandText = vSQL
            rssub = cmsub.ExecuteReader
            If rssub.Read Then
                vData += "<td>" & rssub(0) & "</td>"
            End If
            rssub.Close()
            '============================================================================================================
            vSQL = "select sum(CoreWeight) from item_inv where JobOrderNo='" & rs("vJONO") & "' and Remarks='WRAPPING-COMPLETION'"
            cmsub.CommandText = vSQL
            rssub = cmsub.ExecuteReader
            If rssub.Read Then
                vData += "<td>" & rssub(0) & "</td>"
            End If
            rssub.Close()
            '============================================================================================================
            vSQL = "select sum(NetWeight) from item_inv where JobOrderNo='" & rs("vJONO") & "' and Remarks='WRAPPING-COMPLETION'"
            cmsub.CommandText = vSQL
            rssub = cmsub.ExecuteReader
            If rssub.Read Then
                vData += "<td>" & rssub(0) & "</td>"
            End If
            rssub.Close()
            '============================================================================================================
            vSQL = "select sum(QTY) from item_inv where JobOrderNo='" & rs("vJONO") & "' and Remarks='WRAPPING-COMPLETION'"
            cmsub.CommandText = vSQL
            rssub = cmsub.ExecuteReader
            If rssub.Read Then
                vQTY += rssub(0)
            End If
            rssub.Close()
            '============================================================================================================
            vSQL = "select distinct(UOM) from item_inv where JobOrderNo='" & rs("vJONO") & "' and Remarks='WRAPPING-COMPLETION'"
            cmsub.CommandText = vSQL
            rssub = cmsub.ExecuteReader
            If rssub.Read Then
                vUOM = IIf(rssub(0) = "", "KGS", rssub(0))
            End If
            rssub.Close()
            '============================================================================================================

            If vUOM = "BOX" Then
                vData += "<td>" & vQTY & "</td>"
            Else
                vData += "<td>0</td>"
            End If

            vData += "<td>" & vUOM & "</td>"

            vData += "</Tr>"
            vCtr += 1
            vQTY = 0
            vUOM = ""

        Loop
        rs.Close()


        c.Close()
        cm.Dispose()
        c.Dispose()

        ' ==================================================================================================================

    End Sub

    Private Sub BtnSubmit_ServerClick(sender As Object, e As EventArgs) Handles BtnSubmit.ServerClick
        Dim BatchNo As String = Format(Now, "MMddyyyyhhmmss")
        If CmdCustomerList.SelectedValue = "" Then

        End If

        If CmdProductList.SelectedValue = "" Then
            ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "alert('Please complete all required fields.'); $('#ModalCreateNew').modal();", True)
            Exit Sub
        End If

        If TxtPONO.Text.Trim = "" Then
            ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "alert('Enter PO Number.');", True)
            Exit Sub
        End If

        If TxtJONO.Text.Trim = "" Then
            ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "alert('Enter Job Order Number.');", True)
            Exit Sub
        End If

        If TxtIONUmber.Text.Trim = "" Then

        End If

        vSQL = "insert into item_packinglist_hdr (BatchNo,Cust_Cd,Item_Cd,IONumber,PONO,JONO,CreatedBy,DateCreated) values " _
            & "('" & BatchNo & "', '" & CmdCustomerList.SelectedValue & "','" & CmdProductList.SelectedValue & "', " _
            & "'" & TxtIONUmber.Text.Trim & "', '" & TxtPONO.Text.Trim & "','" & TxtJONO.Text.Trim & "','" & Session("uid") & "','" & Now & "')"
        Response.Write(vSQL)
        CreateRecord(vSQL)

        ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "alert('Successfully saved.');", True)
    End Sub

    Private Sub TblPackingListHeader_SelectedIndexChanged(sender As Object, e As EventArgs) Handles TblPackingListHeader.SelectedIndexChanged

        BtnAddItem.Disabled = False
        BtnEditItem.Disabled = False

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

        vSQL = "select distinct(JONO) as vJONO " _
                  & "from item_packinglist_hdr " _
                  & "where BatchNo='" & TblPackingListHeader.SelectedRow.Cells(2).Text & "'"
        'Response.Write(vSQL)
        cm.CommandText = vSQL
        rs = cm.ExecuteReader
        Do While rs.Read

        Loop
        rs.Close()


        c.Close()
        cm.Dispose()
        c.Dispose()


        'Dim c As New SqlClient.SqlConnection
        Dim da As SqlClient.SqlDataAdapter
        Dim ds As New DataSet

        c.ConnectionString = connStr
        vSQL = " "

        'lblMessageBox.Text = vSQL
        'Exit Sub

        da = New SqlClient.SqlDataAdapter(vSQL, c)
        da.Fill(ds, "ItemList")
        tblItemMaster.DataSource = ds.Tables("ItemList")
        tblItemMaster.DataBind()

        'lblTotal.Text = "<b>Total Item Retrieved : " & tbl_ItemMaster.DataSource.Rows.Count & "</b>"

        da.Dispose()
        ds.Dispose()
    End Sub


    'Private Sub tblJOList_SelectedIndexChanged(sender As Object, e As EventArgs) Handles tblJOList.SelectedIndexChanged
    '    BtnPackingList.Enabled = True
    'End Sub
End Class

