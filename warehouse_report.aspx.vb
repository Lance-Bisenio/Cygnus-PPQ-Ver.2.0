Imports denaro
Imports System
Imports item_details

Partial Class warehouse_report
    Inherits System.Web.UI.Page
    Dim vSQL As String = ""
    Dim c As New SqlClient.SqlConnection
    Public vScript As String = ""
    Public vPendingItem As String = ""

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        If Session("uid") = "" Then
            'Response.Redirect("http://" & SERVERIP & "/" & SITENAME & "/")
        End If

        'If Not CanRun(Session("caption"), Request.Item("id")) Then 
        '	Session("denied") = "1"
        '          Server.Transfer("main.aspx")
        '          Exit Sub
        'End If

        If Not IsPostBack Then

            BuildCombo("select Type_Cd, SubLabel from Item_warehouse_trantype order by Descr", DDLWarehouseList)
            DDLWarehouseList.Items.Add("All")
            DDLWarehouseList.SelectedValue = "All"

        End If
        GetAllReleaseIted()
    End Sub

    Private Sub GetMasterItem(pMode As String)
        Dim c As New SqlClient.SqlConnection
        Dim da As SqlClient.SqlDataAdapter
        Dim ds As New DataSet
        Dim vFilter As String = ""
        Dim vTableName As String = ""
        Dim vSQL As String = ""

        'If txtSearch.Text <> "" Then
        '    vFilter += "where Item_Cd like '%" & txtSearch.Text & "%' "
        'Else
        '    vFilter = "where Item_Cd is not null "
        'End If

        'If cmbItemType.SelectedValue <> "All" Then
        '    vFilter += " and ItemType_Cd='" & cmbItemType.SelectedValue & "' "
        'End If


        'If cmbTypeClass.SelectedValue <> "All" Then
        '    vFilter += " and ItemClass_Cd='" & cmbTypeClass.SelectedValue & "' "
        'End If

        'If cmbUOMQ.SelectedValue <> "All" Then
        '    vFilter += " and QtyUOM_Cd='" & cmbUOMQ.SelectedValue & "' "
        'End If
        vFilter += " Order By Item_Cd asc"

        c.ConnectionString = connStr
        vSQL = "select Item_Cd,Descr, " _
            & "(select Descr from ref_item_customer where ref_item_customer.Customer_Cd=item_master.Customer_Cd) As vCustomer, " _
            & "(select Descr from ref_item_supplier where ref_item_supplier.Supp_Cd=item_master.Supplier_Cd) as vSupplier,Source, " _
            & "(Select Descr from ref_item_type where ref_item_type.Type_Cd=item_master.ItemType_Cd) As vItemType, " _
            & "(select Descr from ref_item_uom where ref_item_uom.UOM_Cd=item_master.QtyUOM_cd) as vUomWeight,NetWeight, " _
            & "(Select Descr from ref_item_uom where ref_item_uom.UOM_Cd=item_master.WeightUOM_Cd) As vUomQty,CurrCost, " _
            & "(select Descr from ref_item_class where ref_item_class.Class_Cd=item_master.ItemClass_Cd) as vTypeClass  " _
            & "from item_master " _
            & vFilter

        'Response.Write(vSQL)

        da = New SqlClient.SqlDataAdapter(vSQL, c)

        da.Fill(ds, "ItemMaster")
        tblItemMaster.DataSource = ds.Tables("ItemMaster")
        tblItemMaster.DataBind()
        lblTotal.Text = "Total Item Retrieved : " & tblItemMaster.DataSource.Rows.Count & ""

        da.Dispose()
        ds.Dispose()

        'If txtSearch.Text.Trim <> "" And TxtLotno.Text.Trim <> "" Then
        '    GetItemOnhandDetails(txtSearch.Text.Trim, TxtLotno.Text.Trim)
        'End If
    End Sub
    Protected Sub tbl_ItemMaster_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles tblItemMaster.PageIndexChanging
        tblItemMaster.PageIndex = e.NewPageIndex
        GetMasterItem("")
    End Sub
    Protected Sub tbl_ItemMaster_SelectedIndexChanged(sender As Object, e As EventArgs) Handles tblItemMaster.SelectedIndexChanged
        GetItemOnhandDetails("", "")
    End Sub
    Private Sub GetItemOnhandDetails(ItemCode As String, ItemLotno As String)
        Dim c As New SqlClient.SqlConnection
        Dim da As SqlClient.SqlDataAdapter
        Dim ds As New DataSet

        Dim vFilter As String = ""
        Dim vTableName As String = ""
        Dim vSQL As String = ""


        c.ConnectionString = connStr

        If ItemCode = "" Then
            vFilter = "Item_Cd='" & tblItemMaster.SelectedRow.Cells(2).Text & "' "
        Else
            vFilter = "Item_Cd='" & ItemCode & "' "
        End If


        'If TxtLotno.Text.Trim <> "" Then
        '    vFilter += "and LotNo like '%" & TxtLotno.Text.Trim & "%' "
        'End If


        vSQL = "select distinct(LotNo) as LotNum, (select sum(Qty) from item_inv a where a.LotNo=b.LotNo And a.Item_Cd=b.Item_Cd) as QTY " _
            & "from item_inv b " _
            & "where " & vFilter & " " _
            & "order by LotNo "

        'Response.Write(vSQL)

        da = New SqlClient.SqlDataAdapter(vSQL, c)
        da.Fill(ds, "ItemOnhandDetails")
        tblItemOnhandDetails.DataSource = ds.Tables("ItemOnhandDetails")

        tblItemOnhandDetails.DataBind()
        'lblTotalLotnum.Text = "Total Item Retrieved : " & tblItemOnhandDetails.DataSource.Rows.Count & ""

        da.Dispose()
        ds.Dispose()

        vSQL = "select sum(Qty) " _
            & "from item_inv " _
            & "where " & vFilter & " "

        lblTotalPerItem.Text = "Total QTY per item code : " & GetRef(vSQL, 0)

    End Sub
    Protected Sub tblItemOnhandDetails_SelectedIndexChanged(sender As Object, e As EventArgs) Handles tblItemOnhandDetails.SelectedIndexChanged
        GetItemTransaction()
    End Sub
    Protected Sub tblItemOnhandDetails_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles tblItemOnhandDetails.PageIndexChanging
        tblItemOnhandDetails.PageIndex = e.NewPageIndex
        GetItemOnhandDetails("", "")
    End Sub

    Private Sub GetItemTransaction()
        Dim c As New SqlClient.SqlConnection
        Dim da As SqlClient.SqlDataAdapter
        Dim ds As New DataSet

        Dim vFilter As String = ""
        Dim vTableName As String = ""
        Dim vSQL As String = ""


        c.ConnectionString = connStr


        vSQL = "select TranId, LotNo, Qty, UOM, Remarks, convert(varchar, DateCreated, 101) + ' ' + convert(varchar, DateCreated, 24) as DateCreated " _
            & "from item_inv b " _
            & "where Item_Cd='" & tblItemMaster.SelectedRow.Cells(2).Text & "' and " _
            & "LotNo='" & tblItemOnhandDetails.SelectedRow.Cells(1).Text & "' " _
            & "Order by TranId desc "
        'Response.Write(vSQL)

        da = New SqlClient.SqlDataAdapter(vSQL, c)
        da.Fill(ds, "tblItemTransaction")
        tblItemTransaction.DataSource = ds.Tables("tblItemTransaction")

        tblItemTransaction.DataBind()
        'lblTotalLotnum.Text = "Total Item Retrieved : " & tblItemOnhandDetails.DataSource.Rows.Count & ""

        da.Dispose()
        ds.Dispose()

        'Response.Write("Qty: " & tblItemOnhandDetails.SelectedRow.Cells(2).Text)
        TxtItemQty.Text = tblItemOnhandDetails.SelectedRow.Cells(2).Text

        vSQL = "select sum(Qty) " _
            & "from item_inv " _
            & "where Item_Cd='" & tblItemMaster.SelectedRow.Cells(2).Text & "' and " _
            & "LotNo='" & tblItemOnhandDetails.SelectedRow.Cells(1).Text & "' "

        lblTotalPerLotnum.Text = "Total QTY per lotnumber : " & GetRef(vSQL, 0)

    End Sub


    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        tblItemMaster.SelectedIndex = -1
        tblItemOnhandDetails.SelectedIndex = -1
        tblItemTransaction.SelectedIndex = -1
        GetMasterItem("")

    End Sub
    Public Function GetItemOnhand(ByVal pItemCd As String) As String
        Dim Onhand As Decimal = 0.00
        Dim ResultOnhand As String = ""

        Try
            vSQL = "select sum(Qty) from item_inv where Item_Cd='" & pItemCd & "' and Qty is not null"
            Onhand = GetRef(vSQL, 0)

        Catch ex As Exception
            ResultOnhand = "0.00" & "*"
        End Try

        Return Format(Onhand, "#,###,##0.00")
    End Function
    Public Function GetLotnoOnhand(ByVal pItemCd As String, ByVal pLotno As String) As String
        Dim Onhand As Decimal = 0.00
        Dim ResultOnhand As String = ""

        Try
            vSQL = "select sum(Qty) from item_inv where Item_Cd='" & pItemCd & "' and Lotno='" & pLotno & "' and Qty is not null"
            Onhand = GetRef(vSQL, 0)

        Catch ex As Exception
            ResultOnhand = "0.00" & "*"
        End Try

        Return Format(Onhand, "#,###,##0.00")
    End Function
    Public Function GetGCAS(ByVal pItemCd As String) As String

        Return GetGCAS_List(pItemCd)

    End Function

    Private Sub tblItemTransaction_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles tblItemTransaction.PageIndexChanging
        tblItemTransaction.PageIndex = e.NewPageIndex
        GetItemTransaction()
    End Sub

    Private Sub tblItemTransaction_SelectedIndexChanging(sender As Object, e As GridViewSelectEventArgs) Handles tblItemTransaction.SelectedIndexChanging
        'ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "alert('test.'); $('#ModalQty').modal();", True)

    End Sub

    Private Sub BtnSave_ServerClick(sender As Object, e As EventArgs) Handles BtnSave.Click

        Dim ItemCd As String = tblItemMaster.SelectedRow.Cells(2).Text
        Dim LotNo As String = tblItemOnhandDetails.SelectedRow.Cells(1).Text
        Dim Barcode As String = GenerateRandomBarcode(32)

        vSQL = "insert into item_transfer (TranType,Item_Cd,LotNo,Qty,Remarks,CreatedBy,DateCreated,RefTranTags,SystemRemarks, Barcode) values " _
            & "('" & DDLTranType.SelectedValue & "','" & ItemCd & "','" & LotNo & "','" _
            & TxtItemQty.Text.Trim & "','" _
            & TxtRemarks.Text.Trim & "','" & Session("uid") & "','" & Now & "',1,'" _
            & DDLTranType.SelectedItem.Text & "','" _
            & Barcode & "')"
        CreateRecord(vSQL)

        vPendingItem = ""
        GetAllReleaseIted()
        ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "alert('Successfully saved');", True)
    End Sub

    Private Sub GetAllReleaseIted()

        Dim c As New SqlClient.SqlConnection
        Dim cm As New SqlClient.SqlCommand
        Dim rs As SqlClient.SqlDataReader
        c.ConnectionString = connStr

        Try
            c.Open()
            cm.Connection = c
        Catch ex As SqlClient.SqlException
            ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "alert('Database connection error.');", True)
            Exit Sub
        End Try


        vSQL = "select Type_Cd, SubLabel, (select count(TranId) from item_transfer where Type_Cd=TranType and DatePosted is null) as Ctr " _
            & "from Item_warehouse_trantype " _
            & "order by Descr"

        cm.CommandText = vSQL
        rs = cm.ExecuteReader
        Do While rs.Read

            If rs("Ctr") = 0 Then
                vPendingItem += "<li class='nav-item'>" _
                        & "<a href='#' class='p-2 sidebar-link'>" & rs("SubLabel") & "" _
                        & "" _
                    & "</a></li>"
            Else
                vPendingItem += "<li class='nav-item'>" _
                        & "<a href='#' id='" & rs("Type_Cd") & "' " _
                        & "class='p-2 sidebar-link' data-toggle='modal' data-target='#ItemLisModal'>" & rs("SubLabel") & "" _
                        & "<span class='badge badge-pill badge-danger ml-2' >" & rs("Ctr") & "</span>" _
                    & "</a></li>"
            End If


        Loop

        rs.Close()


        c.Close()
        c.Dispose()
        cm.Dispose()

        ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "ShowDetails();", True)
    End Sub

    Private Sub btnPost_ServerClick(sender As Object, e As EventArgs) Handles btnPost.ServerClick
        vPendingItem = ""
        Dim postRefNo = Format(Now, "MMddyyyyHHmmss")
        vSQL = "update item_transfer set PostedBy='" & Session("uid") & "', DatePosted='" & Now & "', " _
            & "PostRefNo=" & postRefNo & " " _
            & "where TranType='" & Session("TranType") & "'"
        CreateRecord(vSQL)
        GetAllReleaseIted()
    End Sub
End Class
