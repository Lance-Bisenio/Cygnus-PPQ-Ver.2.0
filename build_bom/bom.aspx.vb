Imports denaro
Imports item_details

Partial Class bomheader
    Inherits System.Web.UI.Page
    Public vScript As String = ""
    Public vColHeader As String = ""
    Public vRecords As String = ""
    Dim vColNames As String = ""
    Dim vColSource As String = ""
    Dim vTableSource As String = ""
      
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
         
        If Session("uid") = "" Then 
            Response.Redirect("http://" & SERVERIP & "/" & SITENAME & "/")
        End If
          
        If Not IsPostBack Then

            cmbShow.Items.Clear()
            For iCtr = 1 To 5
                cmbShow.Items.Add(15 * iCtr)
            Next iCtr
            cmbShow.SelectedValue = 15

            BuildCombo("select uom_Cd, Descr from ref_item_uom order by Descr", cmbItemUOM)
            cmbItemUOM.Items.Add("All")
            cmbItemUOM.SelectedValue = "All"
            BuildCombo("select Type_Cd, Descr from ref_item_type order by Descr", cmbItemType)
            cmbItemType.Items.Add("All")
            cmbItemType.SelectedValue = "All"

            cmbStatus.Items.Add("All")

            If Request.Item("pActiveBom") = "yes" Then
                cmbStatus.SelectedValue = "1"
            Else
                cmbStatus.SelectedValue = "All"
            End If


            BuildCombo("select ColSource, ColTitle from table_properties_dtl where ModuleCode='203' and ColType='SEARCHBY'", cmbSearchBy)
            'BuildCombo("select ColSource, ColTitle from table_properties_dtl where ModuleCode='" & Request.Item("id") & "' and ColType in ('TEXT','TEXTDESCR') and Published='YES'", cmbSearchBy)
            DataRefresh("Reload")
            BomSettings("PageLoad")
        End If

        Select Case h_Mode.Value
            Case "Search"
                BomSettings("Search")
                h_Mode.Value = ""
            Case "ViewDetails"
                BomSettings("ViewDetails")
                h_Mode.Value = ""
            Case "ProcessDetails"
                BomSettings("ProcessDetails")
                h_Mode.Value = ""
            Case "MachineDetails"
                BomSettings("MachineDetails")
                h_Mode.Value = ""
            Case "Backto_Jo"
                vScript = "window.open('../inventory/joborderlist.aspx?id=4060', '_self'); " 
        End Select 
    End Sub
      
    Protected Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        BomSettings("Search")
        vScript = "$('.tblButtonA').show(); $('.tblButtonE').show(); $('.tblButtonD').show(); $('#btnDupBOM').hide();"
    End Sub


    Public Function GetGCAS(ByVal pItemCd As String) As String

        Return GetGCAS_List(pItemCd)

    End Function

    Public Function GetProdHrs(ByVal pBOM As String, ByVal pRev As String) As String
        Dim vHrsProd As Decimal = 0
        Dim vCheckVal As String = ""
        Dim vHrs As Decimal = 0
        Dim vMins As Decimal = 0

        vCheckVal = GetRef("select sum(ProdRun_Hrs) from bom_process where BOM_Cd=" & pBOM & " and Revision=" & pRev, "")
        If vCheckVal <> "null" Then
            vHrs = Convert.ToDecimal(vCheckVal)
        End If
        vCheckVal = ""

        vCheckVal = GetRef("select sum(ProdRun_Mins) from bom_process where BOM_Cd=" & pBOM & " and Revision=" & pRev, "")
        If vCheckVal <> "null" Then
            vMins = Convert.ToDecimal(vCheckVal)
        End If
         
        vHrsProd = (vMins / 60) + vHrs

        Return Format(vHrsProd, "#####0.00")
    End Function
    'Public Function GetGCAS(ByVal pItemCd As String) As String

    '    Dim c As New SqlClient.SqlConnection(connStr)
    '    Dim cm As New SqlClient.SqlCommand
    '    Dim rs As SqlClient.SqlDataReader
    '    Dim vGAS As String = ""
    '    Try
    '        c.Open()
    '    Catch ex As SqlClient.SqlException
    '        vScript = "alert('Error occurred while trying to connect to database. Error is: " & _
    '            ex.Message.Replace(vbCrLf, "").Replace("'", "") & "');"
    '        c.Dispose()
    '        cm.Dispose()
    '    End Try

    '    cm.Connection = c

    '    cm.CommandText = "select distinct(Alt_Cd) as ItemList from ref_item_catalog where Item_Cd ='" & pItemCd & "'"
    '    'Response.Write(cm.CommandText)
    '    Try
    '        rs = cm.ExecuteReader
    '        Do While rs.Read

    '            vGAS += rs("ItemList") & "<br/>"

    '        Loop
    '        rs.Close()

    '    Catch ex As SqlClient.SqlException
    '        vScript = "alert('Error occurred while trying to buil shift reference. Error is: " & _
    '            ex.Message.Replace(vbCrLf, "").Replace("'", "") & "');"
    '    End Try

    '    c.Close()
    '    cm.Dispose()
    '    c.Dispose()


    '    Return vGAS

    'End Function

    Public Function FormatDate(ByVal pDate As String) As String
        If pDate = "" Or IsDBNull(pDate) Then
            Return "&nbsp;"
        Else
            Return Format(CDate(pDate), "MM/dd/yyyy")
        End If
    End Function

    Public Function ProdleadTime(ByVal pDate As String) As String
        If pDate = "0" Then
            Return "&nbsp;"
        Else
            Return Format(CDate(pDate), "MM/dd/yyyy")
        End If
    End Function

    Protected Sub tlbDocInfo_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles tbl_MasterBom.PageIndexChanging
        tbl_MasterBom.PageIndex = e.NewPageIndex
        DataRefresh("Reload")
        BomSettings("Search")
    End Sub

    Protected Sub tbl_ProcessList_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles tbl_ProcessList.PageIndexChanging
        tbl_ProcessList.PageIndex = e.NewPageIndex
        If h_ItemCd.Value <> "" And h_Selected.Value = "1" Then
            BomSettings("ViewDetails")
        Else
            BomSettings("MasterDetails")
        End If
    End Sub


    Protected Sub tbl_tbl_Materials_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles tbl_Materials.PageIndexChanging
        tbl_Materials.PageIndex = e.NewPageIndex
        If h_ItemCd.Value <> "" And h_Selected.Value = "1" Then
            BomSettings("ViewDetails")
        Else
            BomSettings("MasterDetails")
        End If
    End Sub

    Protected Sub cmbShow_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbShow.SelectedIndexChanged
        tbl_MasterBom.PageSize = cmbShow.SelectedValue
        DataRefresh("Reload")
        BomSettings("Search")
    End Sub

    Private Sub DataRefresh(pMode As String)
        Dim c As New SqlClient.SqlConnection
        Dim da As SqlClient.SqlDataAdapter
        Dim ds As New DataSet
        Dim vFilter As String = ""
        Dim vTableName As String = ""
        Dim vSQL As String = ""

        If pMode = "ViewDetails" Then 
            vFilter = "where Item_Cd like '%" & tbl_MasterBom.SelectedRow.Cells(5).Text & "%' and Revision=" & tbl_MasterBom.SelectedRow.Cells(3).Text
        Else
            vFilter = "where Item_Cd is not null " '" & cmbStatus.SelectedValue
        End If

        If cmbStatus.SelectedValue <> "All" Then
            vFilter += " and Status_Cd='" & cmbStatus.SelectedValue & "' "
        Else
            vFilter += " and Status_Cd<>3 "
        End If

        If cmbItemUOM.SelectedValue <> "All" Then
            vFilter += " and ItemUOM='" & cmbItemUOM.SelectedValue & "' "
        End If
        If cmbItemType.SelectedValue <> "All" Then
            vFilter += " and ItemType_Cd='" & cmbItemType.SelectedValue & "' "
        End If

        If txtSearch.Text.Trim <> "" Then
            If cmbSearchBy.SelectedValue = "Item_GCAS" Then 
                vFilter += "and Item_Cd like '%" & SeachItem_GCAS(txtSearch.Text.Trim).Trim & "%' "
            Else 
                vFilter += " and " & cmbSearchBy.SelectedValue & " like '%" & txtSearch.Text.Trim & "%' "
            End If
        End If

        c.ConnectionString = connStr
        vSQL = "select BOM_Cd,Revision,ActiveBy,DateActive,Item_Cd,Remarks," & _
            "(select Descr from ref_item_status where ref_item_status.Status_Cd=" & vTableName & "bom_header.Status_Cd ) as Status_Cd," & _
            "(select Descr from item_master where item_master.Item_Cd=" & vTableName & "bom_header.Item_Cd) as Item_Name, " & _
            "(select Descr from ref_item_type where ref_item_type.Type_Cd=" & vTableName & "bom_header.ItemType_Cd) as ItemType, " & _
            "(select Fullname from user_list where User_id=" & vTableName & "bom_header.CreatedBy) as CreatedBy," & _
            "(select Descr from ref_item_uom where ref_item_uom.UOM_Cd=bom_header.ItemUOM) as vUOM,StdQty," & _
            "(select Descr from ref_item_uom where ref_item_uom.UOM_Cd=bom_header.StdQtyUOM) as vStdOrderUOM," & _
            "(select Descr from ref_item_uom where ref_item_uom.UOM_Cd=bom_header.NetWeightUOM) as vWeightUOM, NetWeight," & _
            "cast(StdProdRunDay as varchar(100)) +' / '+ cast(StdProdRunTime as varchar(5)) as StdProdRun, ReportTo,DateCreated " & _
            "from " & vTableName & "bom_header " & vFilter & " Order by Item_Cd, BOM_Cd, Revision asc"
        'Response.Write(vSQL)


        da = New SqlClient.SqlDataAdapter(vSQL, c)

        'Response.Write(da.SelectCommand.CommandText) "() as vGCAS," & _

        da.Fill(ds, "ItemMaster")
        tbl_MasterBom.DataSource = ds.Tables("ItemMaster")
        tbl_MasterBom.DataBind()
        lblTotalDocs.Text = "<b>BOM Header Retrieved : " & tbl_MasterBom.DataSource.Rows.Count & "</b>"

        da.Dispose()
        ds.Dispose()
    End Sub

    Private Sub GetAllProcess()
        Dim c As New SqlClient.SqlConnection
        Dim da As SqlClient.SqlDataAdapter
        Dim ds As New DataSet
        Dim vFilter As String = ""
        Dim vTableName As String = "" 
        c.ConnectionString = connStr
        da = New SqlClient.SqlDataAdapter("select *," & _
            "(select Descr from ref_emp_section where ref_emp_section.Section_Cd=" & vTableName & "bom_process.Sect_Cd) as Sect_Name, " & _
            "(select Descr from ref_item_process where ref_item_process.Proc_Cd=" & vTableName & "bom_process.Proc_Cd) as Proc_Name, " & _
            "(select Descr + ' ' + Descr1 as vDescr from item_master where Item_Cd=SFG_Cd) SFG_Name," & _
            "(select Fullname from user_list where User_id=" & vTableName & "bom_process.CreatedBy) as CreatedBy " & _
            "from " & vTableName & "bom_process where BOM_Cd=" & h_Bom.Value.Trim & vFilter & " and Revision=" & h_BomRev.Value & " Order by OperOrder,Sect_Cd,Proc_Cd", c)

        'Response.Write(da.SelectCommand.CommandText)
        da.Fill(ds, "ItemProcess")
        tbl_ProcessList.DataSource = ds.Tables("ItemProcess")
        tbl_ProcessList.DataBind()
  
        lblTotalProcess.Text = "<b>Process Retrieved : " & tbl_ProcessList.DataSource.Rows.Count & "</b>"

        da.Dispose()
        ds.Dispose()
    End Sub

    Private Sub GetAllMaterials()
        Dim c As New SqlClient.SqlConnection
        Dim da As SqlClient.SqlDataAdapter
        Dim ds As New DataSet
        Dim vFilter As String = ""
        Dim vTableName As String = ""

        c.ConnectionString = connStr

        da = New SqlClient.SqlDataAdapter("select BOM_Cd, Parent_TranId,TranId, Revision, Proc_Cd, Sect_Cd, Item_Cd, Qty, DrawnFrom, " & _
                "(select Fullname from user_list where User_id=CreatedBy) as CreatedBy, DateCreated," & _
                "(select Descr  from item_master where item_master.Item_Cd=" & vTableName & "bom_materials.Item_Cd) as ItemName," & _
                "(select Descr from ref_item_uom where ref_item_uom.UOM_Cd=bom_materials.ItemUOM) as QtyUom,Grams,Percentage " & _
                "from " & vTableName & "bom_materials where BOM_Cd=" & h_Bom.Value.Trim & " and " & _
                "Parent_TranId='" & h_ProTranId.Value & "'" & vFilter, c)

        'Response.Write(da.SelectCommand.CommandText)

        da.Fill(ds, "MatList")
        tbl_Materials.DataSource = ds.Tables("MatList")
        tbl_Materials.DataBind()
        lblTotalMat.Text = "<b>Materials Retrieved : " & tbl_Materials.DataSource.Rows.Count & "</b>"

        da.Dispose()
        ds.Dispose()
    End Sub

    Private Sub GetAllMachine()
        Dim c As New SqlClient.SqlConnection
        Dim da As SqlClient.SqlDataAdapter
        Dim ds As New DataSet
        Dim vFilter As String = ""
        Dim vTableName As String = "" 
        c.ConnectionString = connStr
        da = New SqlClient.SqlDataAdapter("select Mach_Cd,TranId,Parent_TranId,CapUnit,UOM_Cd,StdLeadDay,StdLeadTime,[Default]," & _
            "(select Descr from ref_item_machine where ref_item_machine.Mach_Cd=" & vTableName & "bom_machine.Mach_Cd) as MachDescr, " & _
            "(select Descr from ref_item_uom where ref_item_uom.Uom_Cd=" & vTableName & "bom_machine.Uom_Cd) as UomDescr, " & _
            "(select Fullname from user_list where User_id=CreatedBy) as CreatedBy, " & _
            "DateCreated from " & vTableName & "bom_machine where BOM_Cd=" & h_Bom.Value.Trim & " and " & _
                "Parent_TranId='" & h_ProTranId.Value & "'" & vFilter, c)

        'Response.Write(da.SelectCommand.CommandText)

        da.Fill(ds, "MachineList")
        tbl_MachineList.DataSource = ds.Tables("MachineList")
        tbl_MachineList.DataBind()
        lblTotalMachine.Text = "<b>Machine Retrieved : " & tbl_MachineList.DataSource.Rows.Count & "</b>"

        da.Dispose()
        ds.Dispose()
    End Sub

    Private Sub GetAllQA()
        Dim c As New SqlClient.SqlConnection
        Dim da As SqlClient.SqlDataAdapter
        Dim ds As New DataSet
        Dim vFilter As String = ""
        Dim vTableName As String = ""
        c.ConnectionString = connStr

        Dim cm As New SqlClient.SqlCommand
        Dim rs As SqlClient.SqlDataReader
        Dim vQA As String = ""
        c.Open()
        cm.Connection = c

        cm.CommandText = "select QA_Cd from bom_process where TranId= " & tbl_ProcessList.SelectedRow.Cells(1).Text
        'Response.Write(cm.CommandText)
        rs = cm.ExecuteReader
        If rs.Read Then
            If Not IsDBNull(rs("QA_Cd")) Then
                vQA = rs("QA_Cd")
            Else
                divQA.Visible = False
                Exit Sub
            End If
        End If
        rs.Close()

        da = New SqlClient.SqlDataAdapter("select QA_Cd as vCode, Descr as vDescr from ref_item_qualityassurance where QA_Cd in (" & vQA & ") order by Descr ", c)
        'Response.Write(da.SelectCommand.CommandText)

        da.Fill(ds, "QAList")
        tbl_QAList.DataSource = ds.Tables("QAList")
        tbl_QAList.DataBind()
        lblTotalQA.Text = "<b>Process QA Retrieved : " & tbl_QAList.DataSource.Rows.Count & "</b>"

        da.Dispose()
        ds.Dispose()
    End Sub

    Private Sub GetAllPeripherals()
        Dim c As New SqlClient.SqlConnection
        Dim da As SqlClient.SqlDataAdapter
        Dim ds As New DataSet
        Dim vFilter As String = ""
        Dim vTableName As String = "" 
        c.ConnectionString = connStr
        da = New SqlClient.SqlDataAdapter("select BOM_Cd, Revision, Proc_Cd, Mach_Cd, Item_Cd, DrawnFrom, TranId,Parent_TranId," & _
                "(select Fullname from user_list where User_id=CreatedBy) as CreatedBy, DateCreated," & _
                "(select Descr from item_master where item_master.Item_Cd=" & vTableName & "bom_peripherals.Item_Cd) as MatDescr" & _
                " from " & vTableName & "bom_peripherals where BOM_Cd=" & h_Bom.Value.Trim & " and Mach_Cd='" & h_Mach.Value.Trim & _
                "' and Parent_TranId=" & h_MachTranId.Value & " and revision=" & h_BomRev.Value & " " & vFilter, c) 'and Proc_Cd='" & h_Proc.Value & "' 

        'Response.Write(da.SelectCommand.CommandText)
        da.Fill(ds, "PerpList")
        tbl_PerpList.DataSource = ds.Tables("PerpList")
        tbl_PerpList.DataBind()
        lblTotalPerp.Text = "<b>Machine Peripherals Retrieved : " & tbl_PerpList.DataSource.Rows.Count & "</b>"

        da.Dispose()
        ds.Dispose()
    End Sub

    Protected Sub btnViewDetails_Click(sender As Object, e As EventArgs) Handles btnViewDetails.Click 
        BomSettings("ViewDetails") 
    End Sub

    Protected Sub tbl_MasterBom_SelectedIndexChanged(sender As Object, e As EventArgs) Handles tbl_MasterBom.SelectedIndexChanged 
        If h_ItemCd.Value <> "" And h_Selected.Value = "1" Then
            BomSettings("ViewDetails") 
        Else
            BomSettings("MasterDetails") 
        End If 
    End Sub

    Protected Sub tbl_ProcessList_SelectedIndexChanged(sender As Object, e As EventArgs) Handles tbl_ProcessList.SelectedIndexChanged 
        BomSettings("ProcessDetails") 
    End Sub

    Protected Sub tbl_Materials_SelectedIndexChanged(sender As Object, e As EventArgs) Handles tbl_Materials.SelectedIndexChanged 
        BomSettings("MaterialDetails")
    End Sub

    Protected Sub tbl_MachineList_SelectedIndexChanged(sender As Object, e As EventArgs) Handles tbl_MachineList.SelectedIndexChanged 
        BomSettings("MachineDetails") 
    End Sub

    Protected Sub tbl_PerpList_SelectedIndexChanged(sender As Object, e As EventArgs) Handles tbl_PerpList.SelectedIndexChanged 
        BomSettings("PerpDetails")
    End Sub

    Private Sub BomSettings(eDiv As String)
        divProcess.Visible = False
        divMaterials.Visible = False
        divMach.Visible = False
        divPerp.Visible = False
        divQA.Visible = False
          
        Select Case eDiv
            Case "MasterDetails" 
                h_Bom.Value = tbl_MasterBom.SelectedRow.Cells(2).Text
                h_BomRev.Value = IIf(tbl_MasterBom.SelectedRow.Cells(3).Text = "&nbsp;", 0, tbl_MasterBom.SelectedRow.Cells(3).Text)
                h_ItemCd.Value = tbl_MasterBom.SelectedRow.Cells(5).Text
                h_ItemDescr.Value = tbl_MasterBom.SelectedRow.Cells(6).Text
                h_BOM_Active.Value = tbl_MasterBom.SelectedRow.Cells(11).Text

                If h_BOM_Active.Value = "Active" Then
                    btnDupBOM.Visible = True
                    btnJO.Visible = True
                Else
                    btnDupBOM.Visible = False
                    btnJO.Visible = False
                End If

            Case "ViewDetails"
                
                If h_Bom.Value.Trim = "" Then
                    vScript = "alert('No selected BOM');"
                    Exit Sub 
                End If

                h_ProTranId.Value = ""
                tbl_Materials.SelectedIndex = -1
                tbl_MachineList.SelectedIndex = -1
                tbl_PerpList.SelectedIndex = -1

                h_Selected.Value = 1
                h_Proc.Value = ""
                h_Mach.Value = ""
                h_Perp.Value = ""
                h_Mat.Value = ""
                h_Sect.Value = ""
                 
                h_MatTranId.Value = ""
                h_MachTranId.Value = ""
                h_PerpTranId.Value = ""

                h_Bom.Value = tbl_MasterBom.SelectedRow.Cells(2).Text
                h_BomRev.Value = IIf(tbl_MasterBom.SelectedRow.Cells(3).Text = "&nbsp;", 0, tbl_MasterBom.SelectedRow.Cells(3).Text)
                h_ItemCd.Value = tbl_MasterBom.SelectedRow.Cells(5).Text
                h_ItemDescr.Value = tbl_MasterBom.SelectedRow.Cells(6).Text
                h_BOM_Active.Value = tbl_MasterBom.SelectedRow.Cells(11).Text

                DataRefresh("ViewDetails")
                GetAllProcess()
                divProcess.Visible = True

                tbl_MasterBom.SelectedIndex = 0
                tbl_ProcessList.SelectedIndex = 0
                 
                '' ===================================================================================================================================================
                '' ===================================================================================================================================================
                Dim vSql As String = "select count(TranId) as vTranId from bom_process where bom_Cd=" & h_Bom.Value & " and Revision=" & h_BomRev.Value
 
                If GetRef(vSql, h_Bom.Value) > 0 Then
                    h_ProTranId.Value = tbl_ProcessList.SelectedRow.Cells(1).Text
                    BomSettings("ProcessDetails")
                End If
                '' ===================================================================================================================================================
                '' ===================================================================================================================================================

                h_Mode.Value = ""
                

            Case "ProcessDetails"
                h_ProTranId.Value = tbl_ProcessList.SelectedRow.Cells(1).Text
                h_Sect.Value = tbl_ProcessList.SelectedRow.Cells(2).Text
                h_Proc.Value = tbl_ProcessList.SelectedRow.Cells(3).Text
                GetAllMaterials()
                GetAllMachine()
                GetAllQA()

                divProcess.Visible = True
                divMaterials.Visible = True
                divMach.Visible = True
                divQA.Visible = True

                tbl_Materials.SelectedIndex = -1
                tbl_MachineList.SelectedIndex = -1
                tbl_PerpList.SelectedIndex = -1

            Case "MachineDetails", "MaterialDetails"
                If eDiv = "MaterialDetails" Then
                    h_MatTranId.Value = tbl_Materials.SelectedRow.Cells(1).Text
                    h_Mat.Value = tbl_Materials.SelectedRow.Cells(2).Text 
                Else
                    h_MachTranId.Value = tbl_MachineList.SelectedRow.Cells(1).Text
                    h_Mach.Value = tbl_MachineList.SelectedRow.Cells(2).Text
                    tbl_PerpList.SelectedIndex = -1
                    GetAllPeripherals()
                End If

                divProcess.Visible = True
                divMaterials.Visible = True
                divMach.Visible = True
                divPerp.Visible = True
                divQA.Visible = True

                tbl_PerpList.SelectedIndex = -1
            Case "PerpDetails"
                h_PerpTranId.Value = tbl_PerpList.SelectedRow.Cells(1).Text

                divProcess.Visible = True
                divMaterials.Visible = True
                divMach.Visible = True
                divPerp.Visible = True
                divQA.Visible = True
            Case "Search"
                DataRefresh("Reload")

                tbl_MasterBom.SelectedIndex = -1
                tbl_ProcessList.SelectedIndex = -1
                tbl_Materials.SelectedIndex = -1
                tbl_MachineList.SelectedIndex = -1
                tbl_PerpList.SelectedIndex = -1
                btnDupBOM.Visible = False

                vScript = "$('.h_Elements').click(function (event) { $(this.id.val("")).val() });"

                h_ItemCd.Value = ""
                h_Selected.Value = ""

                h_Bom.Value = ""
                h_Proc.Value = ""
                h_Sect.Value = ""
                h_Mat.Value = ""
                h_Mach.Value = ""
                h_Perp.Value = "" 
                h_BomRev.Value = ""

                h_Mode.Value = ""
                h_ProTranId.Value = ""
                h_MatTranId.Value = ""
                h_MachTranId.Value = ""
                h_PerpTranId.Value = ""

        End Select

        If h_BOM_Active.Value = "Active" Then
            vScript = "$('.tblButtonA').hide(); $('.tblButtonE').val('View'); $('.tblButtonD').hide(); $('#btnDupBOM').show(); "
            'vScript = "$('.tblButtonA').hide(); $('.tblButtonE').hide(); $('.tblButtonD').hide(); $('#btnDupBOM').show(); " 
        Else
            vScript = "$('.tblButtonA').show(); $('.tblButtonE').show(); $('.tblButtonD').show(); $('#btnDupBOM').hide();"
        End If
    End Sub

End Class
