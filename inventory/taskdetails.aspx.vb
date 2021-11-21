Imports denaro
Partial Class inventory_taskdetails
    Inherits System.Web.UI.Page
    Public vScript As String = ""
    Public vHeader As String = ""
    Public vData As String = ""
    Public vDataPrintingLabel As String = ""
    Dim vSQL As String = ""
    Dim RetCnt As Integer = 0

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
		If Session("uid") = "" Then
			Response.Redirect("http://" & SERVERIP & "/" & SITENAME & "/")
		End If

        If Not IsPostBack Then

            GetJO_Details()
            Build_Header()
            Get_ProcessList()

            h_TranId.Value = Session("pTranId")

            If Request.Item("pMode") = "Print" Then
                btnBack.Visible = False
                btnRelease.Visible = False
                btnPrint.Visible = False
                'lblTotalRecords.Visible = False
                vScript = "window.print();"
            End If
        End If

        vSQL = "select count(TranId) as vCnt " _
                & "from prod_return_materials b " _
                & "where JONO='" & lblJO.Text & "' and DateReceived is null and TranStatus is null "
        RetCount.Text = GetRef(vSQL, 0)


        If h_Mode.Value = "Reload" Then
            GetJO_Details()
            Build_Header()
            Get_ProcessList()
            Change_JobOrder_Status()
        End If
	End Sub

	Public Sub Change_JobOrder_Status()
        'JOPROD2 = MATERIALS RELEASED COMPLETE
        Dim c As New SqlClient.SqlConnection(connStr)
        Dim cm As New SqlClient.SqlCommand 
        Dim vSQL As String = ""
        Dim vProdStatus As String = ""
         
        Try
            c.Open()
        Catch ex As SqlClient.SqlException
            vScript = "alert('Error occurred while trying to connect to database. Error is: " & _
                ex.Message.Replace(vbCrLf, "").Replace("'", "") & "');"
            c.Dispose()
            cm.Dispose()
            Exit Sub
        End Try

        cm.Connection = c

        vSQL = "insert into jo_header_log select * from jo_header where JobOrderNo='" & lblJO.Text & "'"
        cm.CommandText = vSQL
        Try
            'Response.Write(vSQL)
            cm.ExecuteNonQuery()
        Catch ex As SqlClient.SqlException
            Response.Write("Error in SQL query select insert new job order log :  " & vSQL & ex.Message)
        End Try 

        If h_TotalBal.Value = 0 Then
            vProdStatus = "JOPROD2"
        Else
            vProdStatus = "JOPROD1"
        End If

        vSQL = "update jo_header set ProdStatus='" & vProdStatus & "', ProdRemarks='', " _
            & "ProdDateCreated='" & Format(Now, "MM/dd/yyyy") & "', ProdCreatedBy='" & Session("uid") & "' " _
            & "where JobOrderNo='" & lblJO.Text & "'"
        cm.CommandText = vSQL
        'Response.Write(vSQL)
        Try
            cm.ExecuteNonQuery()
        Catch ex As SqlClient.SqlException
            Response.Write("Error in SQL query update production status :  " & ex.Message)
        End Try

        c.Close()
        cm.Dispose()
        c.Dispose()
    End Sub

    Private Sub GetJO_Details()
        Dim c As New SqlClient.SqlConnection(connStr)
        Dim cm As New SqlClient.SqlCommand
        Dim rs As SqlClient.SqlDataReader

        Try
            c.Open()
        Catch ex As SqlClient.SqlException
            vScript = "alert('Error occurred while trying to connect to database. Error is: " & _
                ex.Message.Replace(vbCrLf, "").Replace("'", "") & "');"
            c.Dispose()
            cm.Dispose()
            Exit Sub
        End Try

        cm.Connection = c
        cm.CommandText = "select * from jo_header where TranId=" & Session("pTranId")
        'Response.Write(cm.CommandText)
        Try
            rs = cm.ExecuteReader
            If rs.Read Then
                lblItemCode.Text = IIf(IsDBNull(rs("Item_Cd")), "", rs("Item_Cd"))
                lblCust_Cd.Text = IIf(IsDBNull(rs("Cust_Cd")), "", rs("Cust_Cd"))
                lblGCAS.Text = IIf(IsDBNull(rs("Alt_Cd")), "", rs("Alt_Cd"))
                lblJO.Text = IIf(IsDBNull(rs("JobOrderNo")), "", rs("JobOrderNo"))
                lblSO.Text = IIf(IsDBNull(rs("SalesOrderNo")), "", rs("SalesOrderNo"))
                lblPO.Text = IIf(IsDBNull(rs("PurchaseOrderNo")), "None", rs("PurchaseOrderNo"))
                lblBOM.Text = IIf(IsDBNull(rs("BOM_Cd")), "", rs("BOM_Cd"))
                lblBOMRev.Text = IIf(IsDBNull(rs("BOMRev")), "", rs("BOMRev"))
				lblProdDate.Text = IIf(IsDBNull(rs("StartDate")), "", Format(CDate(rs("StartDate")), "MMM dd, yyyy"))
				lblQtyOrder.Text = IIf(IsDBNull(rs("OrderQty")), "", rs("OrderQty"))
            End If 
            rs.Close()

            lblItemDescr.Text = GetRef("select Descr from item_master where Item_Cd='" & lblItemCode.Text & "'", "")
            lblCustDescr.Text = GetRef("select Descr from ref_item_customer where Customer_Cd='" & lblCust_Cd.Text & "'", "")



        Catch ex As SqlClient.SqlException
            vScript = "alert('Error occurred while trying to buil shift reference. Error is: " & _
                ex.Message.Replace(vbCrLf, "").Replace("'", "") & "');"
        End Try

        c.Close()
        cm.Dispose()
        c.Dispose()

    End Sub

    Private Sub Build_Header()
        vHeader = ""

        '"<td style='width:70px;' class='titleBar'>Operation Order</td>" &
        '"<td style='width:150px;' class='titleBar'>Process Description</td>" & 

        vHeader += "<tr class='bg-info text-white'>" &
            "<td class='titleBar'>Material Code</td>" &
            "<td class='titleBar'>Material Description</td>"

        vHeader += "<td style='width:70px;' class='titleBar'>Qty<br />Release</td>" &
            "<td style='width:70px;' class='titleBar'>Released<br />By</td>" &
            "<td style='width:70px;' class='titleBar'>Received<br />By</td>" &
            "<td style='width:80px;' class='titleBar'>UOM</td>" &
            "<td class='titleBar'>LOTNO</td>" &
            "<td class='titleBar'>Roll No</td>" &
            "<td style='width:80px;' class='titleBar'>Balance</td>" &
            "<td style='width:40px;' class='titleBar'>Edit</td>" _
            & "<td style='width:40px;' class='titleBar'>Print Label</td>"

        '"<td style='width:55px;' class='titleBar'></td>" & _'"<td style='width:70px;' class='titleBar'>Qty<br />Required</td>" & _
    End Sub

    Private Sub Get_ProcessList()
        Dim c As New SqlClient.SqlConnection(connStr)
        Dim cm As New SqlClient.SqlCommand
        Dim rs As SqlClient.SqlDataReader

        Dim cm_sub As New SqlClient.SqlCommand
        Dim rs_sub As SqlClient.SqlDataReader

        Dim cm_sub2 As New SqlClient.SqlCommand
        'Dim rs_sub2 As SqlClient.SqlDataReader
        Dim vTtlBal As Decimal = 0

        Try
            c.Open()
        Catch ex As SqlClient.SqlException
            vScript = "alert('Error occurred while trying to connect to database. Error is: " & _
                ex.Message.Replace(vbCrLf, "").Replace("'", "") & "');"
            c.Dispose()
            cm.Dispose()
            Exit Sub
        End Try

        cm.Connection = c
        cm_sub.Connection = c
        cm_sub2.Connection = c
        vData = ""

        'vMinQtyOrder = GetRef("select StdQty from bom_header where BOM_Cd='" _
        ' & lblBOM.Text.Trim & "' and Revision='" & lblBOMRev.Text.Trim & "' ", 0)

        vSQL = "select TranId, OperOrder, Sect_Cd, Proc_Cd, " _
            & "(select Descr from ref_item_process where ref_item_process.Proc_Cd=bom_process.Proc_Cd and " _
            & "ref_item_process.Sect_Cd = bom_process.Sect_Cd) as ProcName " _
            & "from bom_process where " _
                & "Proc_Cd<>'3002' and " _
                & "BOM_Cd=" & Session("pBOM") & " and " _
                & "Revision=" & Session("pBOMRev") & " order by OperOrder"



        cm.CommandText = vSQL


        Try
            rs = cm.ExecuteReader
            Do While rs.Read
                vData += "<tr style='height:24px; background-color:#e6e4e4; font-weight:bold;'>" &
                            "<td>Operation Order : " & rs("OperOrder") & "</td>" &
                            "<td class='labelL'>Process Description : " & rs("ProcName") & "</td>"

                vSQL = "select (select Descr from ref_item_machine a where a.Mach_Cd=e.Mach_Cd) as vMacDescr from jo_machine e where " _
                    & "JobOrderNo='" & lblJO.Text.Trim & "' and " _
                    & "BOM_Cd=" & Session("pBOM") & " and " _
                    & "OperOrder=" & rs("OperOrder") & " and " _
                    & "IsPrimary='YES'"



                cm_sub.CommandText = vSQL
                rs_sub = cm_sub.ExecuteReader
                If rs_sub.Read Then
                    vData += "<td colspan='8' class='labelL'>Machine : " & rs_sub("vMacDescr") & "</td><td></td></tr>"
                Else
                    vData += "<td colspan='8' class='labelL'></td><td></td></tr>"
                End If
                rs_sub.Close()

                GetAlternative_Materials(lblJO.Text, rs("OperOrder"))

            Loop

            rs.Close()

            h_TotalBal.Value = vTtlBal

        Catch ex As SqlClient.SqlException
            vScript = "alert('Error occurred while trying to buil list of materials based on the BOM reference. Error is: " &
                ex.Message.Replace(vbCrLf, "").Replace("'", "") & "');"
        End Try

        cm.Dispose()
        cm_sub.Dispose()
        cm_sub2.Dispose()
        c.Close()
        c.Dispose()

    End Sub

    Private Sub GetAlternative_Materials(pPONO As String, pOrderNo As Integer)
        Dim c As New SqlClient.SqlConnection(connStr)
        Dim cm As New SqlClient.SqlCommand
        Dim rs As SqlClient.SqlDataReader

        Dim cmSub As New SqlClient.SqlCommand
        'Dim rsSub As SqlClient.SqlDataReader

        Try
            c.Open()
        Catch ex As SqlClient.SqlException
            vScript = "alert('Error occurred while trying to connect to database. Error is: " &
                ex.Message.Replace(vbCrLf, "").Replace("'", "") & "');"
            c.Dispose()
            cm.Dispose()
            Exit Sub
        End Try



        ' GET ALL ALTERNATIVE MATERIALS
        cm.Connection = c
        cmSub.Connection = c


        vSQL = "select TranId, BatchNo, OperOrder, Item_Cd, Qty, ReleaseBy, DateRelease, UOM, LotNo, RollNo, ReceivedBy, DateReceived," _
            & "(select Descr from item_master b where b.Item_Cd=a.Item_Cd) As ItemName, IsAltItem, " _
            & "(select TOP 1 SFG_Descr from bom_process e where e.SFG_Cd=a.Item_Cd and e.BOM_Cd=" & Session("pBOM") & ") As SFGName, " _
            & "(Select Emp_Lname+', '+Emp_Fname  from emp_master where Emp_Cd=ReceivedBy) as vRec_EName,  " _
            & "(select FullName from user_list where user_id=ReceivedBy) as vRec_Admin, " _
            & "(select Emp_Lname+', '+Emp_Fname  from emp_master where Emp_Cd=ReleaseBy) as vRel_EName, " _
            & "(select FullName from user_list where user_id=ReleaseBy) as vRec_UName " _
            & "from prod_rawmaterial a " _
            & "where JONO='" & Session("pJO") & "' and OperOrder=" & pOrderNo

        'Response.Write(vSQL & "<br><br>") 'vRec_EName vRec_UName

        cm.CommandText = vSQL

        Try
            rs = cm.ExecuteReader
            Do While rs.Read

                If rs("IsAltItem") = 1 Then
                    vData += "<tr class='text-info'>"
                Else
                    vData += "<tr class='text-secondary'>"
                End If


                ' =========================================================================
                ' #Operation Order	
                ' #Process Description	
                ' #Material Code	
                ' #Material Description
                ' =========================================================================
                vData += "<td>" & rs("Item_Cd") & "</td>" _
                    & "<td>" & IIf(IsDBNull(rs("ItemName")), rs("SFGName"), rs("ItemName")) & "</td>"

                ' =========================================================================
                ' #Qty Release
                ' #Released By And Date
                ' =========================================================================
                vData += "<td>" & rs("Qty") & "</td>"

                If Not IsDBNull(rs("vRel_EName")) Then
                    vData += "<td>" & rs("vRel_EName").ToString.ToUpper & "<br>" & Format(CDate(rs("DateRelease")), "MM/dd/yyyy") & "</td>"
                Else
                    vData += "<td>" & rs("ReleaseBy").ToString.ToUpper & "<br>" & Format(CDate(rs("DateRelease")), "MM/dd/yyyy") & "</td>"
                End If


                ' =========================================================================
                ' #Received By And Date
                ' =========================================================================
                If Not IsDBNull(rs("vRec_EName")) Then
                    vData += "<td>" & rs("vRec_EName").ToString.ToUpper & "<br>"
                Else
                    vData += "<td>" & rs("vRec_Admin").ToString.ToUpper & "<br>"
                End If





                If Not IsDBNull(rs("DateReceived")) Then
                    vData += Format(CDate(rs("DateReceived")), "MM/dd/yyyy")
                End If
                vData += "</td>"



                ' =========================================================================
                ' #UOM
                ' #LOTNO
                ' #Balance
                ' =========================================================================
                vData += "<td>" & rs("UOM") & "</td>" _
                    & "<td>" & rs("LotNo") & "</td>" _
                    & "<td>" & rs("RollNo") & "</td>" _
                    & "<td></td>"
                vData += "<td>"


                ' =========================================================================
                ' #Edit
                ' =========================================================================
                If IsDBNull(rs("DateReceived")) Then
                    vData += "<input type='Button' id='' name='' class='btn btn-primary btn-sm' " _
                       & "onclick='EditItem(""" _
                            & rs("TranId") & """,""" _
                            & rs("OperOrder") & """,""" _
                            & rs("Item_Cd") & """,""" _
                            & rs("Qty") & """,""" _
                            & rs("LotNo") & """,""" _
                            & rs("RollNo") & """,""" _
                            & rs("BatchNo") & """,""" & rs("IsAltItem") & """);' value='Edit'>"
                End If

                vData += "</td>" _
                    & "<td><input type='Button' id='' name='' class='btn btn-warning btn-sm' value='Print' " _
                    & "onclick='PrintLabel(""AddToPrint"",""" _
                            & rs("TranId") & """,""" _
                            & pPONO & """,""" & rs("BatchNo") & """);'></td></tr>"



                'Loop
                'rs.Close()

            Loop
            rs.Close()

        Catch ex As SqlClient.SqlException
            vScript = "alert('Error occurred while trying to buil shift reference. Error is: " &
                ex.Message.Replace(vbCrLf, "").Replace("'", "") & "');"
        End Try

        cm.Dispose()
        c.Close()
        c.Dispose()

    End Sub

    Protected Sub btnBack_Click(sender As Object, e As EventArgs) Handles btnBack.Click
        Session("pMode") = "FromTaskDetails"
        Server.Transfer("tasklist.aspx?id=310&pDFrom=" & Session("RDateFrom") & "&pDTo=" & Session("RDateTo"))
    End Sub

    'Protected Sub btnReload_Click(sender As Object, e As EventArgs) Handles btnReload.Click
    '    GetJO_Details()
    '    Build_Header()
    '    Get_ProcessList()
    'End Sub

    Private Sub BtnTabRel_Click(sender As Object, e As EventArgs) Handles BtnTabRel.Click
        BtnTabRel.CssClass = "btn btn-success btn-sm"
        BtnTabReq.Attributes("class") = "btn btn-primary btn-sm"
        BtnTabRet.Attributes("class") = "btn btn-primary btn-sm"

        GetJO_Details()
        Build_Header()
        Get_ProcessList()
    End Sub

    Private Sub BtnTabReq_ServerClick(sender As Object, e As EventArgs) Handles BtnTabReq.ServerClick
        BtnTabRel.CssClass = "btn btn-primary btn-sm"
        BtnTabReq.Attributes("class") = "btn btn-success btn-sm"
        BtnTabRet.Attributes("class") = "btn btn-primary btn-sm"

        vHeader = ""
        vHeader += "<tr class='bg-info text-white'>" &
            "<td style='width:150px;' class='titleBar'>Process Description</td>" &
            "<td style='width:100px;' class='titleBar'>Material Code</td>" &
            "<td class='titleBar'>Material Description</td>" &
            "<td style='width:90px;' class='titleBar'>Qty Request</td>" &
            "<td style='width:90px;' class='titleBar'>Request By</td>" &
            "<td style='width:90px;' class='titleBar'>Date</td>" &
            "<td style='width:60px;' class='titleBar'>Action</td>"

        Session("vHeader") = vHeader
        GetRequestItem()

    End Sub

    Private Sub GetRequestItem()
        Dim c As New SqlClient.SqlConnection(connStr)
        Dim cm As New SqlClient.SqlCommand
        Dim rs As SqlClient.SqlDataReader

        Dim cm_sub As New SqlClient.SqlCommand
        Dim rs_sub As SqlClient.SqlDataReader

        Dim cm_sub2 As New SqlClient.SqlCommand

        Dim vMinQtyOrder As Decimal = 0

        Try
            c.Open()
        Catch ex As SqlClient.SqlException
            vScript = "alert('Error occurred while trying to connect to database. Error is: " &
                ex.Message.Replace(vbCrLf, "").Replace("'", "") & "');"
            c.Dispose()
            cm.Dispose()
            Exit Sub
        End Try

        cm.Connection = c
        cm_sub.Connection = c
        cm_sub2.Connection = c
        vData = ""

        vMinQtyOrder = GetRef("select StdQty from bom_header where BOM_Cd='" _
                              & lblBOM.Text.Trim & "' and Revision='" & lblBOMRev.Text.Trim & "' ", 0)

        cm.CommandText = "select TranId, OperOrder, Sect_Cd, Proc_Cd, " _
            & "(select Descr from ref_item_process where ref_item_process.Proc_Cd=bom_process.Proc_Cd and " _
            & "ref_item_process.Sect_Cd = bom_process.Sect_Cd) as ProcName " _
            & "from bom_process where " _
                & "Proc_Cd<>'3002' and " _
                & "BOM_Cd=" & Session("pBOM") & " and " _
                & "Revision=" & Session("pBOMRev") & " order by OperOrder"

        'Response.Write(cm.CommandText)
        Try
            rs = cm.ExecuteReader
            Do While rs.Read
                vData += "<tr style='height:24px; background-color:#e6e4e4; font-weight:bold;'>" &
                            "<td colspan='3' class='labelL'>" & rs("ProcName") & "</td>"

                vSQL = "select " _
                    & "(select Descr from ref_item_machine a where a.Mach_Cd=e.Mach_Cd) as vMacDescr " _
                    & "from jo_machine e where " _
                    & "JobOrderNo='" & lblJO.Text.Trim & "' and " _
                    & "BOM_Cd=" & Session("pBOM") & " and " _
                    & "OperOrder=" & rs("OperOrder") & " and " _
                    & "IsPrimary='YES'"


                'Response.Write(vSQL)
                cm_sub.CommandText = vSQL
                rs_sub = cm_sub.ExecuteReader
                If rs_sub.Read Then
                    vData += "<td colspan='7' class='labelL'>Machine : " & rs_sub("vMacDescr") & "</td></tr>"
                Else
                    vData += "<td colspan='7' class='labelL'></td></tr>"
                End If
                rs_sub.Close()

                GetJORequestItemDetails(lblJO.Text, rs("OperOrder"))

            Loop

            rs.Close()

        Catch ex As SqlClient.SqlException
            vScript = "alert('Error occurred while trying to buil list of materials base on the BOM reference. Error is: " &
                ex.Message.Replace(vbCrLf, "").Replace("'", "") & "');"
        End Try

        cm.Dispose()
        cm_sub.Dispose()
        cm_sub2.Dispose()
        c.Close()
        c.Dispose()

    End Sub

    Private Sub GetJORequestItemDetails(pPONO As String, pOrderNo As Integer)
        Dim c As New SqlClient.SqlConnection(connStr)
        Dim cm As New SqlClient.SqlCommand
        Dim rs As SqlClient.SqlDataReader

        Dim cmSub As New SqlClient.SqlCommand
        'Dim rsSub As SqlClient.SqlDataReader

        Dim VCtr As Integer = 0

        Try
            c.Open()
        Catch ex As SqlClient.SqlException
            vScript = "alert('Error occurred while trying to connect to database. Error is: " &
                ex.Message.Replace(vbCrLf, "").Replace("'", "") & "');"
            c.Dispose()
            cm.Dispose()
            Exit Sub
        End Try

        ' GET ALL ALTERNATIVE MATERIALS
        cm.Connection = c
        cmSub.Connection = c

        vSQL = "select TranId, Item_Cd, Qty, CreatedBy, DateCreated, TranStatus," _
            & "(select Descr from item_master b where b.Item_Cd=a.Item_Cd) As ItemName,  " _
            & "(select Emp_Fname +' '+ Emp_Lname from emp_master where CreatedBy=Emp_Cd) as vName, " _
            & "(select FullName from user_list where User_Id=CreatedBy) as vRecBy " _
            & "from prod_request_materials a " _
            & "where JONO='" & pPONO & "' and OperOrder=" & pOrderNo

        'Response.Write(vSQL & "<br><br>")

        cm.CommandText = vSQL

        Try
            rs = cm.ExecuteReader
            Do While rs.Read

                vData += "<tr class='text-info'>"
                vData += "<td></td>" _
                    & "<td>" & rs("Item_Cd") & "</td>" _
                    & "<td>" & rs("ItemName") & "</td>"

                vData += "<td class='labelL'>" & rs("Qty") & "</td>"
                vData += "<td class='labelL'>" & rs("CreatedBy") & "</td>"
                vData += "<td class='labelL'>" & Format(CDate(rs("DateCreated")), "MM/dd/yyyy") & "</td>"

                vData += "<td class='labelL'>"
                If IsDBNull(rs("TranStatus")) Then
                    vData += "<input type='Button' id='' name='' class='btn btn-primary btn-sm' " _
                            & "data-toggle='modal' data-target='#ModalTagAsCompete' " _
                            & "onclick='TagAsComplete(" & rs("TranId") & ",this.value);' value='Tag as complete'>"
                Else
                    vData += "<input type='Button' id='' name='' class='btn btn-danger btn-sm' " _
                            & "data-toggle='modal' data-target='#ModalTagAsCompete' " _
                            & "onclick='TagAsComplete(" & rs("TranId") & ",this.value);' value='Cancel'>"
                End If
                vData += "</td></tr>"

                VCtr += 1

            Loop
            rs.Close()

        Catch ex As SqlClient.SqlException
            vScript = "alert('Error occurred while trying to buil shift reference. Error is: " &
                ex.Message.Replace(vbCrLf, "").Replace("'", "") & "');"
        End Try

        cm.Dispose()
        c.Close()
        c.Dispose()
        'RetCount.Text = RetCnt

    End Sub

    Private Sub BtnTabRet_ServerClick(sender As Object, e As EventArgs) Handles BtnTabRet.ServerClick
        BtnTabRel.CssClass = "btn btn-primary btn-sm"
        BtnTabReq.Attributes("class") = "btn btn-primary btn-sm"
        BtnTabRet.Attributes("class") = "btn btn-success btn-sm"

        vHeader = ""
        vHeader += "<tr class='bg-info text-white'>" &
            "<td style='width:150px;' class='titleBar'>Process Description</td>" &
            "<td style='width:100px;' class='titleBar'>Material Code</td>" &
            "<td class='titleBar'>Material Description</td>" &
            "<td style='width:90px;' class='titleBar'>Qty Return</td>" &
            "<td style='width:90px;' class='titleBar'>LOTNO</td>" &
            "<td style='width:90px;' class='titleBar'>Return By</td>" &
            "<td style='width:90px;' class='titleBar'>Date</td>" &
            "<td style='width:90px;' class='titleBar'>Received By</td>" &
            "<td style='width:90px;' class='titleBar'>Date</td>" &
            "<td style='width:60px;' class='titleBar'>Action</td>"

        Session("vHeader") = vHeader
        GetReturnItem()
    End Sub

    Private Sub GetReturnItem()
        Dim c As New SqlClient.SqlConnection(connStr)
        Dim cm As New SqlClient.SqlCommand
        Dim rs As SqlClient.SqlDataReader

        Dim cm_sub As New SqlClient.SqlCommand
        Dim rs_sub As SqlClient.SqlDataReader

        Dim cm_sub2 As New SqlClient.SqlCommand

        Dim vMinQtyOrder As Decimal = 0

        Try
            c.Open()
        Catch ex As SqlClient.SqlException
            vScript = "alert('Error occurred while trying to connect to database. Error is: " &
                ex.Message.Replace(vbCrLf, "").Replace("'", "") & "');"
            c.Dispose()
            cm.Dispose()
            Exit Sub
        End Try

        cm.Connection = c
        cm_sub.Connection = c
        cm_sub2.Connection = c
        vData = ""

        vMinQtyOrder = GetRef("select StdQty from bom_header where BOM_Cd='" _
                              & lblBOM.Text.Trim & "' and Revision='" & lblBOMRev.Text.Trim & "' ", 0)

        cm.CommandText = "select TranId, OperOrder, Sect_Cd, Proc_Cd, " _
            & "(select Descr from ref_item_process where ref_item_process.Proc_Cd=bom_process.Proc_Cd and " _
            & "ref_item_process.Sect_Cd = bom_process.Sect_Cd) as ProcName " _
            & "from bom_process where " _
                & "Proc_Cd<>'3002' and " _
                & "BOM_Cd=" & Session("pBOM") & " and " _
                & "Revision=" & Session("pBOMRev") & " order by OperOrder"


        'Response.Write(cm.CommandText)
        Try
            rs = cm.ExecuteReader
            Do While rs.Read
                vData += "<tr style='height:24px; background-color:#e6e4e4; font-weight:bold;'>" &
                            "<td colspan='3' class='labelL'>" & rs("ProcName") & "</td>"

                vSQL = "select " _
                    & "(select Descr from ref_item_machine a where a.Mach_Cd=e.Mach_Cd) as vMacDescr " _
                    & "from jo_machine e where " _
                    & "JobOrderNo='" & lblJO.Text.Trim & "' and " _
                    & "BOM_Cd=" & Session("pBOM") & " and " _
                    & "OperOrder=" & rs("OperOrder") & " and " _
                    & "IsPrimary='YES'"


                'Response.Write(vSQL)
                cm_sub.CommandText = vSQL
                rs_sub = cm_sub.ExecuteReader
                If rs_sub.Read Then
                    vData += "<td colspan='7' class='labelL'>Machine : " & rs_sub("vMacDescr") & "</td></tr>"
                Else
                    vData += "<td colspan='7' class='labelL'></td></tr>"
                End If
                rs_sub.Close()

                GetJOReturnItem(lblJO.Text, rs("OperOrder"))

            Loop

            rs.Close()



        Catch ex As SqlClient.SqlException
            vScript = "alert('Error occurred while trying to buil list of materials base on the BOM reference. Error is: " &
                ex.Message.Replace(vbCrLf, "").Replace("'", "") & "');"
        End Try

        cm.Dispose()
        cm_sub.Dispose()
        cm_sub2.Dispose()
        c.Close()
        c.Dispose()

    End Sub

    Private Sub GetJOReturnItem(pPONO As String, pOrderNo As Integer)
        Dim c As New SqlClient.SqlConnection(connStr)
        Dim cm As New SqlClient.SqlCommand
        Dim rs As SqlClient.SqlDataReader

        Dim cmSub As New SqlClient.SqlCommand
        'Dim rsSub As SqlClient.SqlDataReader

        Try
            c.Open()
        Catch ex As SqlClient.SqlException
            vScript = "alert('Error occurred while trying to connect to database. Error is: " &
                ex.Message.Replace(vbCrLf, "").Replace("'", "") & "');"
            c.Dispose()
            cm.Dispose()
            Exit Sub
        End Try

        ' GET ALL ALTERNATIVE MATERIALS
        cm.Connection = c
        cmSub.Connection = c

        vSQL = "select TranId, Item_Cd, Createdby,DateCreated, Qty as QtyRet, LotNo," _
            & "(select Descr from item_master a where b.Item_Cd=a.Item_Cd) As ItemName, " _
            & "(select sum(Qty) from prod_return_materials a where a.JONO=b.JONO and a.LotNo=b.LotNo) as vQty," _
            & "(select Emp_Fname +' '+ Emp_Lname from emp_master where CreatedBy=Emp_Cd) as vCreatedOps, " _
            & "(select FullName from user_list where User_Id=CreatedBy) as vCreatedAdmin, " _
            & "(select Emp_Fname +' '+ Emp_Lname from emp_master where ReceivedBy=Emp_Cd) as vRecOps, " _
            & "(select FullName from user_list where User_Id=ReceivedBy) as vRecAdmin, DateReceived " _
            & "from prod_return_materials b " _
            & "where JONO='" & pPONO & "' " _
            & "and TranStatus is null " _
            & "and OperOrder=" & pOrderNo

        'Response.Write(vSQL & "<br><br>")
        cm.CommandText = vSQL

        Try
            rs = cm.ExecuteReader
            Do While rs.Read

                vData += "<td></td>" _
                        & "<td>" & rs("Item_Cd") & "</td>" _
                        & "<td>" & rs("ItemName") & "</td>"

                vData += "<td class='labelL'>" & rs("QtyRet") & "</td>"
                vData += "<td class='labelL'>" & rs("LotNo") & "</td>"
                vData += "<td class='labelL'>"

                If IsDBNull(rs("vCreatedOps")) Then
                    vData += rs("vCreatedAdmin")
                Else
                    vData += rs("vCreatedOps")
                End If

                vData += "</td><td class='labelL'>" & Format(CDate(rs("DateCreated")), "MM/dd/yyyy") & "</td>"

                vData += "<td class='labelL'>"

                If IsDBNull(rs("vRecOps")) Then
                    vData += rs("vRecAdmin")
                Else
                    vData += rs("vRecOps")
                End If

                vData += "</td>"

                If Not IsDBNull(rs("DateReceived")) Then
                    vData += "<td class='labelL'>" & Format(CDate(rs("DateReceived")), "MM/dd/yyyy") & "</td>"
                Else
                    vData += "<td class='labelL'></td>"
                        RetCnt += 1
                    End If

                vData += "<td class='labelL'>"

                If IsDBNull(rs("DateReceived")) Then
                    vData += "<input type='Button' id='' name='' class='btn btn-primary btn-sm' " _
                            & "data-toggle='modal' data-target='#myModal' " _
                            & "onclick='RecRawMats(" & rs("TranId") & ",this.value);' value='Receive'>"
                Else
                    vData += "<input type='Button' id='' name='' class='btn btn-danger btn-sm' " _
                            & "data-toggle='modal' data-target='#myModal' " _
                            & "onclick='RecRawMats(" & rs("TranId") & ",this.value);' value='Cancel'>"
                End If

                vData += "</td>"

                vData += "</tr>"
            Loop
            rs.Close()

        Catch ex As SqlClient.SqlException
            vScript = "alert('Error occurred while trying to buil shift reference. Error is: " &
                ex.Message.Replace(vbCrLf, "").Replace("'", "") & "');"
        End Try

        RetCount.Text = RetCnt

        cm.Dispose()
        c.Close()
        c.Dispose()
    End Sub

    Private Sub BtnAcceptRawmats_Click(sender As Object, e As EventArgs) Handles BtnAcceptRawmats.Click

        vHeader = Session("vHeader")

        If h_TrnIdRecRMType.Value = "Cancel" Then
            vSQL = "update prod_return_materials set " _
            & "ReceivedBy=null, " _
            & "DateReceived=null " _
            & "where TranId=" & h_TrnIdRecRM.Value
        Else
            vSQL = "update prod_return_materials set " _
            & "ReceivedBy='" & Session("uid") & "', " _
            & "DateReceived='" & Now & "' " _
            & "where TranId=" & h_TrnIdRecRM.Value
        End If

        CreateRecord(vSQL)

        GetReturnItem()
    End Sub

    Private Sub BtnSaveItemRequest_Click(sender As Object, e As EventArgs) Handles BtnSaveItemRequest.Click
        vHeader = Session("vHeader")

        If h_TrnReqType.Value = "Cancel" Then
            vSQL = "update prod_request_materials set " _
                & "TranStatus=null, " _
                & "Remarks='~" & Now & "|" & Session("uid") & "|CANCEL' + Remarks " _
                & "where TranId=" & h_TrnReqID.Value
        Else
            vSQL = "update prod_request_materials set " _
                & "TranStatus='COMPLETED', " _
                & "Remarks = CASE WHEN Remarks is null THEN '~" & Now & "|" & Session("uid") & "|COMPLETED' ELSE '~" & Now & "|" & Session("uid") & "|COMPLETED' + Remarks END " _
                & "where TranId=" & h_TrnReqID.Value
        End If

        'Response.Write(h_TrnReqType.Value & vSQL)

        h_TrnReqType.Value = ""

        CreateRecord(vSQL)

        GetRequestItem()
    End Sub

    Private Sub BtnPrintingLabel_Click(sender As Object, e As EventArgs) Handles BtnPrintingLabel.Click
        Dim c As New SqlClient.SqlConnection(connStr)
        Dim cm As New SqlClient.SqlCommand
        Dim rs As SqlClient.SqlDataReader

        Try
            c.Open()
        Catch ex As SqlClient.SqlException
            vScript = "alert('Error occurred while trying to connect to database. Error is: " &
                ex.Message.Replace(vbCrLf, "").Replace("'", "") & "');"
            c.Dispose()
            cm.Dispose()
            Exit Sub
        End Try

        cm.Connection = c

        vSQL = "select TranId, JONO, BatchNo, " _
            & "(select Item_Cd from prod_rawmaterial b where a.BatchNo=b.BatchNo) As ItemCode, " _
            & "(select Qty from prod_rawmaterial b where a.BatchNo=b.BatchNo) as Qty, " _
            & "(Select LotNo from prod_rawmaterial b where a.BatchNo=b.BatchNo) As Lotno, " _
            & "(select RollNo from prod_rawmaterial b where a.BatchNo=b.BatchNo) as RollNo " _
            & "from prod_printlabel a where JONO='" & Session("pJO") & "'"

        cm.CommandText = vSQL

        Try
            rs = cm.ExecuteReader
            Do While rs.Read

                vDataPrintingLabel += "<tr id='" & rs("TranId") & "x'>" _
                        & "<td>" & rs("TranId") & "</td>" _
                        & "<td>" & rs("ItemCode") & "</td>" _
                        & "<td>" & rs("Qty") & "</td>" _
                        & "<td>" & rs("Lotno") & "</td>" _
                        & "<td>" & rs("RollNo") & "</td>" _
                        & "<td><input type='Button' id='' name='' class='btn btn-primary btn-sm' " _
                            & "onclick='PrintLabel(""CancelPerItem"",""" _
                                & rs("TranId") & """,""" _
                                & rs("JONO") & """,""" _
                                & rs("BatchNo") & """);' value='Cancel'></td>"

                vDataPrintingLabel += "</tr>"
            Loop
            rs.Close()

        Catch ex As SqlClient.SqlException
            vScript = "alert('Error occurred while trying to buil shift reference. Error is: " &
                ex.Message.Replace(vbCrLf, "").Replace("'", "") & "');"
        End Try

        cm.Dispose()
        c.Close()
        c.Dispose()

        vScript = "$('#PrintingListModal').modal();"

    End Sub
End Class

