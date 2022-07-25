Imports denaro
Imports System
Imports item_details
Partial Class processmaterials_receiving
    Inherits System.Web.UI.Page
    Dim vSQL As String = ""
    Dim c As New SqlClient.SqlConnection
    Public vScript As String = ""
    Public vPendingItem As String = ""
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        If Session("uid") = "" Then
            ' value = Response.Redirect("http://" & SERVERIP & "/" & SITENAME & "/")
        End If

        If Not IsPostBack Then
            BuildCombo("select Type_Cd, SubLabel from Item_warehouse_trantype order by Descr", DDLWarehouseList)

            DDLWarehouseList.Items.Add("Main Warehouse")
            DDLWarehouseList.SelectedValue = "Main Warehouse"

            'BuildCombo("select Type_Cd, Descr from ref_item_type order by Descr", cmbItemType)
            'cmbItemType.Items.Add("All")
            'cmbItemType.SelectedValue = "All"

            BuildCombo("select Type_Cd, Descr from Item_warehouse_trantype order by Descr", DDLTranType)

        End If
        GetAllReleaseIted()
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


        vSQL = "select Type_Cd, SubLabel, " _
            & "(select count(TranId) from item_transfer where Type_Cd=TranType and DatePosted is not null and DateReceived is null) as Ctr " _
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

    Private Sub btnAccept_ServerClick(sender As Object, e As EventArgs) Handles btnAccept.ServerClick

        vPendingItem = ""
        vSQL = "update item_transfer set ReceivedBy='" & Session("uid") & "', DateReceived='" & Now & "', " _
            & "PostRefNo='" & Session("PostRefNo") & "' " _
            & "where TranType='" & Session("TranType") & "' and DatePosted is not null"

        CreateRecord(vSQL)

        GetPostedItemList()
        GetAllReleaseIted()
    End Sub
    Protected Sub tbl_ItemMaster_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles tblItemMaster.PageIndexChanging
        tblItemMaster.PageIndex = e.NewPageIndex
        GetPostedItemList()
    End Sub
    Private Sub GetPostedItemList()
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


        vSQL = "select Item_Cd, PostRefNo, LotNo, Qty, Remarks, PostedBy, DatePosted, 0 as Unitcost,TranType, " _
            & "(select QtyUOM_Cd from item_master a where a.Item_Cd=b.Item_Cd) As ItemUOM " _
            & "from item_transfer b " _
            & "where DatePosted Is Not null and " _
            & "PostRefNo='" & Session("PostRefNo") & "' and " _
            & "TranType='" & Session("TranType") & "' and " _
            & "DateReceived is not null"

        cm.CommandText = vSQL
        rs = cm.ExecuteReader
        Do While rs.Read
            CreateItemInvRecord(
                rs("Item_Cd"), rs("PostRefNo"), rs("LotNo"),
                rs("Qty"), rs("Remarks"), rs("PostedBy"), rs("DatePosted"),
                rs("Unitcost"), rs("TranType"), rs("ItemUOM"))


        Loop
        rs.Close()

        c.Close()
        c.Dispose()
        cm.Dispose()

    End Sub
    Private Sub CreateItemInvRecord(
            pItemCode As String,
            pBatchNo As String,
            pLotno As String,
            pQty As Decimal,
            pRemarks As String,
            pCreatedBy As String,
            pDateCreated As Date,
            pUnitCost As Decimal,
            pWarehouseName As String,
            pItemUOM As String)

        vSQL = "insert into item_inv (" _
                & "Item_Cd,SupplierBarcode,RefBarcode,Barcode,LotNo," _
                & "Qty,UOM,Cost,TranType,Remarks,CreatedBy,DateCreated,JobOrderNo,WHName) values ('"

        vSQL += pItemCode.Trim & "','','" & pBatchNo & "','" & pItemCode.Trim & "','" _
                & pLotno & "','" & pQty & "', '" & pItemUOM.Trim & "','" & pUnitCost & "','" _
                & "Process Warehouse Receiving','Process Warehouse Receiving','" _
                & pCreatedBy & "','" & Format(pDateCreated, "MM/dd/yyyy HH:mm") & "','NONE','" & pWarehouseName & "')"


        CreateRecord(vSQL)
        ' ======================================================================================
        Page.ClientScript.RegisterStartupScript(Me.GetType(), "window-script", "alert('Successfully Saved.')", True)

    End Sub

    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        GetPostedItemReport()
    End Sub

    Private Sub GetPostedItemReport()
        Dim c As New SqlClient.SqlConnection
        Dim da As SqlClient.SqlDataAdapter
        Dim ds As New DataSet
        Dim vFilter As String = ""
        Dim vTableName As String = ""
        Dim vSQL As String = ""


        c.ConnectionString = connStr
        vSQL = "select distinct(PostRefNo) as PostedRef from item_transfer " _
            & "where TranType='" & DDLWarehouseList.SelectedValue & "' and DatePosted is not null order by PostRefNo"

        Response.Write(vSQL)
        da = New SqlClient.SqlDataAdapter(vSQL, c)

        da.Fill(ds, "ItemMaster")
        tblItemMaster.DataSource = ds.Tables("ItemMaster")
        tblItemMaster.DataBind()
        'lblTotal.Text = "Total Item Retrieved : " & tblItemMaster.DataSource.Rows.Count & ""

        da.Dispose()
        ds.Dispose()

        'If txtSearch.Text.Trim <> "" And TxtLotno.Text.Trim <> "" Then
        '    GetItemOnhandDetails(txtSearch.Text.Trim, TxtLotno.Text.Trim)
        'End If
    End Sub
End Class
