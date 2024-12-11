Imports denaro
Imports System
Imports item_details
Imports System.Runtime.ConstrainedExecution
Imports System.ServiceModel.PeerResolvers

Partial Class warehouse
    Inherits System.Web.UI.Page
    Dim vSQL As String = ""
    Dim c As New SqlClient.SqlConnection
    Public vScript As String = ""
    Public Complist As String = ""
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
            BuildCombo("select Customer_Cd, Descr from ref_item_customer " _
                       & "where Customer_Cd in (select distinct(Customer) as CustomerId from prod_completion where Sect_Cd in ('SLIT','BAGM') ) " _
                       & "order by Descr", DDLCustList)
            DDLCustList.Items.Add("All")
            DDLCustList.SelectedValue = "All"

            BtnSave.Visible = False
            BtnUpdate.Visible = False

            ddlSource.Items.Add(New ListItem("SLITTING", "SLIT"))
            ddlSource.Items.Add(New ListItem("BAG MAKING", "BAGM"))

        End If
        GetPackingList()

    End Sub

    Private Sub GetPackingList()
        Dim c As New SqlClient.SqlConnection
        Dim da As SqlClient.SqlDataAdapter
        Dim ds As New DataSet
        Dim vFilter As String = ""
        Dim vTableName As String = ""
        Dim vSQL As String = ""

        c.ConnectionString = connStr

        vSQL = "select TranId, BatchNo,JONO, CustomerId,JONO,Item_Cd CreatedBy, Remarks, CoreWeight, NetWeight, GrossWeight, " _
            & "(select Descr + ' ' + Descr1  from item_master a where a.Item_Cd=b.Item_Cd) as ItemName, " _
            & "(select Descr from ref_item_customer a where a.Customer_Cd=b.CustomerId) as CustName, " _
            & "PONO,FORMAT(PODate, 'MM/dd/yyyy') as PODate, " _
            & "FORMAT(DeliveryDate, 'MM/dd/yyyy') as DeliveryDate, " _
            & "FORMAT(ProdDate, 'MM/dd/yyyy') as ProdDate, " _
            & "FORMAT(DateCreated, 'MM/dd/yyyy') as DateCreated,PalletCnt,PalletItemCnt, " _
            & "CONVERT(varchar(10), PalletCnt)  + '/' +  " _
            & "Convert(varchar(10), PalletItemCnt) As Pallet, Source " _
            & "from prod_packinglist b " _
            & "where 1=1 " & vFilter & " order by ItemName"


        da = New SqlClient.SqlDataAdapter(vSQL, c)

        da.Fill(ds, "tblGetPackingList")
        tblGetPackingList.DataSource = ds.Tables("tblGetPackingList")
        tblGetPackingList.DataBind()

        da.Dispose()
        ds.Dispose()

    End Sub
    Private Sub tblGetPackingList_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles tblGetPackingList.PageIndexChanging
        tblGetPackingList.PageIndex = e.NewPageIndex
        GetPackingList()
        tblGetPackingList.SelectedIndex = -1
    End Sub
    Private Sub tblGetPackingList_SelectedIndexChanged(sender As Object, e As EventArgs) Handles tblGetPackingList.SelectedIndexChanged
        Session("PListID") = tblGetPackingList.SelectedRow.Cells(1).Text

        LblCustName.Text = tblGetPackingList.SelectedRow.Cells(3).Text
        LblItemName.Text = tblGetPackingList.SelectedRow.Cells(4).Text
        TxtJO.Text = tblGetPackingList.SelectedRow.Cells(2).Text

        TxtPO.Text = tblGetPackingList.SelectedRow.Cells(9).Text
        TxtPODate.Text = tblGetPackingList.SelectedRow.Cells(10).Text
        TxtDelDate.Text = tblGetPackingList.SelectedRow.Cells(11).Text
        TxtProdDate.Text = tblGetPackingList.SelectedRow.Cells(12).Text
        TxtRemarks.Text = tblGetPackingList.SelectedRow.Cells(13).Text
        TxtPalletCnt.Text = tblGetPackingList.SelectedRow.Cells(14).Text
        TxtPalletItemCnt.Text = tblGetPackingList.SelectedRow.Cells(15).Text

        btnPListEdit.Enabled = True
        btnPListDel.Enabled = True
        btnPListAddItem.Enabled = True
        btnPListView.Enabled = True

        BtnSave.Visible = False
        BtnUpdate.Visible = True
    End Sub
    Private Sub GetMasterItem(pMode As String)
        Dim c As New SqlClient.SqlConnection
        Dim da As SqlClient.SqlDataAdapter
        Dim ds As New DataSet
        Dim vFilter As String = ""
        Dim vTableName As String = ""
        Dim vSQL As String = ""

        If txtSearch.Text <> "" Then
            vFilter += "and JobOrderNo like '%" & txtSearch.Text & "%' "
        End If

        If DDLCustList.SelectedValue <> "All" Then
            vFilter += " and Cust_Cd='" & DDLCustList.SelectedValue & "' "
        End If


        c.ConnectionString = connStr
        vSQL = "Select JobOrderNo, Item_Cd, " _
            & "(select Descr + ' ' + Descr1  from item_master a where a.Item_Cd=b.Item_Cd) as ItemName, " _
            & "(select Descr from ref_item_customer where Customer_Cd=Cust_Cd) as CustName, Cust_Cd, " _
            & "SalesOrderNo " _
            & "From jo_header b " _
            & "where JobOrderNo in (select JONO from prod_completion where JobOrderNo=JONO and Sect_Cd in ('SLIT','BAGM') ) " & vFilter & " order by ItemName"

        da = New SqlClient.SqlDataAdapter(vSQL, c)

        da.Fill(ds, "ItemMaster")
        tblItemMaster.DataSource = ds.Tables("ItemMaster")
        tblItemMaster.DataBind()

        da.Dispose()
        ds.Dispose()

    End Sub
    Protected Sub tbl_ItemMaster_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles tblItemMaster.PageIndexChanging
        tblItemMaster.PageIndex = e.NewPageIndex
        GetMasterItem("")
        tblItemMaster.SelectedIndex = -1
    End Sub
    Protected Sub tbl_ItemMaster_SelectedIndexChanged(sender As Object, e As EventArgs) Handles tblItemMaster.SelectedIndexChanged
        GetItemOnhandDetails("")

        LblCustName.Text = tblItemMaster.SelectedRow.Cells(6).Text
        LblItemName.Text = tblItemMaster.SelectedRow.Cells(5).Text
        TxtJO.Text = tblItemMaster.SelectedRow.Cells(2).Text

        btnCreate.Disabled = False
        BtnSave.Visible = True
        BtnUpdate.Visible = False

        TxtDelDate.Text = ""
        TxtPO.Text = ""
        TxtPODate.Text = ""
        TxtProdDate.Text = ""
        TxtDelDate.Text = ""
        TxtRemarks.Text = ""
        TxtPalletCnt.Text = ""
        TxtPalletItemCnt.Text = ""

    End Sub
    Private Sub GetItemOnhandDetails(pFilter As String)
        Dim c As New SqlClient.SqlConnection
        Dim da As SqlClient.SqlDataAdapter
        Dim ds As New DataSet

        Dim vFilter As String = ""
        Dim vTableName As String = ""
        Dim vSQL As String = ""

        Dim vSlittingCtn As Int64 = 0
        Dim vBagMakingCtn As Int64 = 0

        vSQL = "select count(BatchNo) " _
            & "from prod_completion where JONO='" & tblItemMaster.SelectedRow.Cells(2).Text & "' and Sect_Cd='SLIT' "
        vSlittingCtn = GetRef(vSQL, 0)
        lblCtnComp1.InnerHtml = vSlittingCtn

        vSQL = "select count(BatchNo) " _
            & "from prod_completion where JONO='" & tblItemMaster.SelectedRow.Cells(2).Text & "' and Sect_Cd='BAGM' "
        vSlittingCtn = GetRef(vSQL, 0)
        lblCtnComp2.InnerHtml = vSlittingCtn

        c.ConnectionString = connStr

        vSQL = "select (select Descr from ref_item_process a where a.Proc_Cd=b.Proc_Cd) as ProcessDescr, " _
            & "GrossWeight, CoreWeight, NetWeight, BatchNo, DateCreated, QTY, TtlPCS " _
            & "from prod_completion b " _
            & "where JONO='" & tblItemMaster.SelectedRow.Cells(2).Text & "' "

        If pFilter = "SLIT" Then
            vSQL += "and Sect_Cd='SLIT'"
        ElseIf pFilter = "BAGM" Then
            vSQL += "and Sect_Cd='BAGM'"
        Else
            vSQL += "and Sect_Cd in ('SLIT','BAGM')"
        End If

        da = New SqlClient.SqlDataAdapter(vSQL, c)

        da.Fill(ds, "tblItemDetails")
        tblItemDetails.DataSource = ds.Tables("tblItemDetails")
        tblItemDetails.DataBind()

        da.Dispose()
        ds.Dispose()

    End Sub

    Private Sub tblItemDetails_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles tblItemDetails.PageIndexChanging
        tblItemDetails.PageIndex = e.NewPageIndex
        GetItemOnhandDetails("")
    End Sub

    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        tblItemMaster.SelectedIndex = -1
        tblGetPackingList.SelectedIndex = -1

        GetMasterItem("")
        btnCreate.Disabled = True

        BtnPListEdit.Enabled = False
        BtnPListDel.Enabled = False
        BtnPListAddItem.Enabled = False
        BtnPListView.Enabled = False
    End Sub

    Private Sub BtnSave_ServerClick(sender As Object, e As EventArgs) Handles BtnSave.Click

        Dim ItemCd As String = tblItemMaster.SelectedRow.Cells(2).Text
        Dim BatchNo As String = Format(Now, "yyyyMMddHHmm")

        vSQL = "insert into prod_packinglist " _
                & "(BatchNo,CustomerId,JONO,JODate,PONO,PODate,Item_Cd, " _
                & "DeliveryDate,ProdDate,PrepBy,DatePrep, " _
                & "CreatedBy,DateCreated,Remarks,PalletCnt,PalletItemCnt,Source) values ("

        vSQL += BatchNo & "," _
                & "'" & tblItemMaster.SelectedRow.Cells(7).Text & "'," _
                & "'" & TxtJO.Text.Trim & "'," _
                & "'" & TxtJODate.Text.Trim & "'," _
                & "'" & TxtPO.Text.Trim & "'," _
                & "'" & TxtPODate.Text.Trim & "'," _
                & "'" & tblItemMaster.SelectedRow.Cells(4).Text & "'," _
                & "'" & TxtDelDate.Text.Trim & "'," _
                & "'" & TxtProdDate.Text.Trim & "'," _
                & "'" & Session("uid") & "'," _
                & "'" & Format(Now, "MM/dd/yyyy HH:mm") & "'," _
                & "'" & Session("uid") & "'," _
                & "'" & Format(Now, "MM/dd/yyyy HH:mm") & "'," _
                & "'" & TxtRemarks.Text.Trim & "'," _
                & "'" & TxtPalletCnt.Text.Trim & "'," _
                & "'" & TxtPalletItemCnt.Text.Trim & "'," _
                & "'" & ddlSource.SelectedValue & "')"

        CreateRecord(vSQL)


        Dim Ctr As Int32 = 0
        Try
            Ctr = CInt(TxtPalletItemCnt.Text.Trim) * CInt(TxtPalletCnt.Text.Trim)
            vSQL = "select top " & Ctr & " TranId " _
                & "from prod_completion " _
                & "where JONO='" & TxtJO.Text.Trim & "' and TranType='COMPLETION' and Sect_Cd='" & ddlSource.SelectedValue & "' order by DateCreated"
            AutoAddItem(vSQL, BatchNo, "Add")

        Catch ex As Exception

        End Try

        GetPackingList()
        ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "alert('Successfully saved');", True)
    End Sub
    Private Sub BtnUpdate_Click(sender As Object, e As EventArgs) Handles BtnUpdate.Click

        vSQL = "update prod_packinglist set " _
                & "PONO='" & TxtPO.Text.Trim & "', " _
                & "JODate='" & TxtJODate.Text.Trim & "', " _
                & "PODate='" & TxtPODate.Text.Trim & "', " _
                & "DeliveryDate='" & TxtDelDate.Text.Trim & "', " _
                & "ProdDate='" & TxtProdDate.Text.Trim & "', " _
                & "Remarks='" & TxtRemarks.Text.Trim & "', " _
                & "PalletCnt='" & TxtPalletCnt.Text.Trim & "', " _
                & "PalletItemCnt='" & TxtPalletItemCnt.Text.Trim & "', " _
                & "Source='" & ddlSource.SelectedValue & "' " _
                & "where BatchNo='" & tblGetPackingList.SelectedRow.Cells(1).Text & "'"

        CreateRecord(vSQL)

        Dim Ctr As Int32 = 0
        Try
            Ctr = CInt(TxtPalletItemCnt.Text.Trim) * CInt(TxtPalletCnt.Text.Trim)

            vSQL = "delete from prod_packinglist_details " _
                & "where JONO='" & TxtJO.Text.Trim & "' and BatchNo='" & tblGetPackingList.SelectedRow.Cells(1).Text & "'"

            CreateRecord(vSQL)

            vSQL = "select top " & Ctr & " TranId " _
                & "from prod_completion " _
                & "where JONO='" & TxtJO.Text.Trim & "' and Sect_Cd='" & ddlSource.SelectedValue & "' and " _
                & "TranType='COMPLETION' and IsDeleted is null order by DateCreated"

            AutoAddItem(vSQL, tblGetPackingList.SelectedRow.Cells(1).Text, "Uodate")
        Catch ex As Exception

        End Try

        GetPackingList()

        ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "alert('Successfully saved');", True)

    End Sub
    Private Sub AutoAddItem(pSQL As String, BatchNo As Int64, pMode As String)

        Dim Ctr As Integer = 1
        Dim c As New SqlClient.SqlConnection
        Dim cm As New SqlClient.SqlCommand
        Dim rs As SqlClient.SqlDataReader
        Dim SQLStr As String = ""

        c.ConnectionString = connStr

        Try
            c.Open()
            cm.Connection = c
        Catch ex As SqlClient.SqlException
            ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "alert('Database connection error.');", True)
            Exit Sub
        End Try

        cm.CommandText = pSQL
        rs = cm.ExecuteReader

        SQLStr = "insert into prod_packinglist_details (BatchNo,JONO,CompletionTranId,Customer,CreatedBy,DateCreated,Remarks) values "


        Do While rs.Read
            If pMode = "Add" Then
                SQLStr += "(" & BatchNo & ",'" & TxtJO.Text.Trim & "'," & rs("TranId") & "," _
                    & "'" & tblItemMaster.SelectedRow.Cells(7).Text & "','" & Session("uid") & "'," _
                    & "'" & Format(Now, "MM/dd/yyyy HH:mm") & "','remarks'),"
            Else
                SQLStr += "(" & BatchNo & ",'" & TxtJO.Text.Trim & "'," & rs("TranId") & "," _
                    & "'" & tblGetPackingList.SelectedRow.Cells(16).Text & "','" & Session("uid") & "'," _
                    & "'" & Format(Now, "MM/dd/yyyy HH:mm") & "','remarks'),"
            End If

        Loop
        rs.Close()
        'Response.Write(SQLStr)

        SQLStr = SQLStr.Trim().Remove(SQLStr.Length - 1)
        CreateRecord(SQLStr)

        c.Close()
        c.Dispose()
        cm.Dispose()

    End Sub
    Private Sub tblGetPackingList_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles tblGetPackingList.RowEditing
        BtnSave.Visible = False
        BtnUpdate.Visible = True
    End Sub
    Private Sub BtnPListEdit_Click(sender As Object, e As EventArgs) Handles BtnPListEdit.Click
        ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "$('#myModal').modal('show');", True)
    End Sub
    Private Sub BtnPListDel_Click(sender As Object, e As EventArgs) Handles BtnPListDel.Click
        vSQL = "delete from prod_packinglist where BatchNo='" & tblGetPackingList.SelectedRow.Cells(1).Text & "'"

        CreateRecord(vSQL)
        GetPackingList()

        ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "alert('Successfully deleted');", True)
    End Sub
    Private Sub BtnPListAddItem_Click(sender As Object, e As EventArgs) Handles BtnPListAddItem.Click
        ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "$('#AddItemModal').modal('show');", True)
        GetCompletionDetails()
    End Sub
    Private Sub GetCompletionDetails()

        Dim Ctr As Integer = 1
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


        vSQL = "select TranId, (select Descr from ref_item_process a where a.Proc_Cd=b.Proc_Cd) as ProcessDescr, " _
            & "GrossWeight, CoreWeight, NetWeight, BatchNo, DateCreated, TtlPcs, TtlPcsBox, Qty," _
            & "(select CompletionTranId from prod_packinglist_details c " _
                    & "where c.CompletionTranId=b.TranId and c.JONO=b.JONO and " _
                    & "BatchNo='" & tblGetPackingList.SelectedRow.Cells(1).Text & "') as AddedItem " _
            & "from prod_completion b " _
            & "where JONO='" & tblGetPackingList.SelectedRow.Cells(2).Text & "' and Sect_Cd='" & tblGetPackingList.SelectedRow.Cells(18).Text & "' and " _
            & "TranType='COMPLETION' and IsDeleted is null " _
            & "order by DateCreated"

        'Response.Write(vSQL)
        cm.CommandText = vSQL
        rs = cm.ExecuteReader
        Do While rs.Read
            Complist += "<tr>" _
                & "<td>" & Ctr & "</td>" _
                & "<td>" & rs("BatchNo") & "</td>" _
                & "<td>" & rs("GrossWeight") & "</td>" _
                & "<td>" & rs("CoreWeight") & "</td>" _
                & "<td>" & rs("NetWeight") & "</td>" _
                & "<td>" & rs("Qty") & "</td>" _
                & "<td>" & rs("TtlPcs") & "</td>" _
                & "<td>" & rs("TtlPcsBox") & "</td>"

            If IsDBNull(rs("AddedItem")) Then
                Complist += "<td><input type='button' id='" & rs("TranId") & "' onclick='AddItem(" & rs("TranId") & ",this.value)' class='btn btn-info btn-sm' value='Add'></td>"
            Else
                Complist += "<td><input type='button' id='" & rs("TranId") & "' onclick='AddItem(" & rs("TranId") & ",this.value)' class='btn btn-danger btn-sm' value='Del'></td>"
            End If

            Complist += "</tr>"
            Ctr += 1
        Loop
        rs.Close()

        c.Close()
        c.Dispose()
        cm.Dispose()

    End Sub
    Private Sub BtnSaveSeleteditem_Click(sender As Object, e As EventArgs) Handles BtnSaveSeleteditem.Click
        GetCompletionDetails()
    End Sub

    Private Sub btnPListView_Click(sender As Object, e As EventArgs) Handles btnPListView.Click
        ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "ViewReport();", True)
    End Sub
    Private Sub btnSlit_ServerClick(sender As Object, e As EventArgs) Handles btnSlit.ServerClick
        GetItemOnhandDetails("SLIT")
        ddlSource.SelectedValue = "SLIT"
    End Sub
    Private Sub btnBag_ServerClick(sender As Object, e As EventArgs) Handles btnBag.ServerClick
        GetItemOnhandDetails("BAGM")
        ddlSource.SelectedValue = "BAGM"
    End Sub
End Class
