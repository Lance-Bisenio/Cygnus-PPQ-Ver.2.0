Imports denaro
Partial Class inventory_taskmaterials
    Inherits System.Web.UI.Page
    Public vScript As String = ""
    Public vHeader As String = ""
    Public vData As String = ""
    Public vTitle As String = ""

	Dim vJO As String = ""
	Dim vBOM As String = ""
    Dim vBOMRev As String = ""
    Dim vSQL As String = ""
    Dim vFilter As String = ""


	Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

		If Session("uid") = Nothing Or Session("uid") = "" Then
			vScript = "alert('Login session has expired. Please re-login again.'); window.close();"
			Exit Sub
		End If



		If Not IsPostBack Then

			If Session("vProc_Matlist") = "" Then
				vScript = "alert('Please check BOM raw materials set-up.')"
				'Exit Sub
			End If

			Session.Remove("ViewAll")
			BuildCombo("select distinct(Proc_Cd)," _
				& "(select Descr from ref_item_process a where a.Proc_Cd=b.Proc_Cd) as vDescr " _
				& "from prod_completion b order by vDescr", cmdProcess)
			Builder()
		End If
		lblScanLabel.Text = ""

        'Select Case Request.Item("pMode")
        '	Case "WarehouseRelease"
        '		vTitle = "Warehouse raw-material releasing"

        '	Case "ReceiveMaterials"
        '		vTitle = "Production raw-material receiving"

        '	Case "RequestMaterials"
        '		vTitle = "Production raw-material request"
        'End Select

    End Sub

	Private Sub Builder()
        vJO = Request.Item("pJO")
        vBOM = Request.Item("pBom")
        vBOMRev = Request.Item("pBomRev")

        Select Case Request.Item("pMode")
   
            Case "ReceiveMaterials"
                ProductionMaterial_Receiving_Header()
                ProductionMaterial_Receiving()

                cmdSave.Visible = True
                cmdViewAll.Visible = True
                cmdViewPending.Visible = True
                cmdCancel.Visible = True

        End Select

    End Sub

	Private Sub ProductionMaterial_Cost(pTranIdList As String)

		Dim c As New SqlClient.SqlConnection(connStr)
		Dim cm As New SqlClient.SqlCommand
		Dim rs As SqlClient.SqlDataReader

		Dim vJONO As String = Request.Item("pJO")
		Dim vOperNo As String = Request.Item("pOperNo")

		Dim vJOQty As Integer = 0
		Dim vRMTotalCost As Decimal = 0
		Dim vCostPerQty As Decimal = 0
		Dim vRunningSFGCost As Decimal = 0
		Dim vRunningSFGQty As Decimal = 0

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

		' =============================================================================================================

		vSQL = "select OrderQty from jo_header where JobOrderNo='" & vJONO & "' "
		cm.CommandText = vSQL
		rs = cm.ExecuteReader
		If rs.Read Then
			vJOQty = rs("OrderQty")
		End If
		rs.Close()

		' =============================================================================================================

		vSQL = "select sum(TotalCost) as vItemCost from prod_rawmaterial " _
			& "where JONO='" & vJONO & "' and OperOrder='" & vOperNo & "' and " _
			& "DateReceived is not null "


		' & "Tranid in (" & pvTranIdList.TrimEnd(",") & ") and " _

		cm.CommandText = vSQL
		rs = cm.ExecuteReader
		If rs.Read Then
			vRMTotalCost = IIf(IsDBNull(rs("vItemCost")), 0, rs("vItemCost"))
		End If
		rs.Close()

		' =============================================================================================================

		vSQL = "select sum(ProdTotalCost) as vProdCost from prod_completion " _
			& "where JONO='" & vJONO & "' and OperOrder='" & vOperNo & "' and DateVoid is null "

		cm.CommandText = vSQL
		rs = cm.ExecuteReader
		If rs.Read Then
			vRunningSFGCost = IIf(Not IsDBNull(rs("vProdCost")), rs("vProdCost"), 0)
		End If
		rs.Close()

		' =============================================================================================================

		vSQL = "select sum(NetWeight) as vNetWeight from prod_completion " _
			& "where JONO='" & vJONO & "' and OperOrder='" & vOperNo & "'"
		cm.CommandText = vSQL
		rs = cm.ExecuteReader
		If rs.Read Then
			vRunningSFGQty = IIf(Not IsDBNull(rs("vNetWeight")), rs("vNetWeight"), 0)
		End If
		rs.Close()

		' =============================================================================================================

		vCostPerQty = (vRMTotalCost - vRunningSFGCost) / (vJOQty - vRunningSFGQty)

		'Response.Write(vCostPerQty & " (" & vRMTotalCost & "-" & vRunningSFGCost & ") / (" & vJOQty & "-" & vRunningSFGQty & ")")

		'Try 
		'Catch ex As SqlClient.SqlException
		'    vScript = "alert('Error occurred while trying to buil shift reference. Error is: " &
		'        ex.Message.Replace(vbCrLf, "").Replace("'", "") & "');"
		'End Try


		vSQL = "insert into prod_qtycost (JONO, OperOrder, Item_Cd, UOM, LotNo, JOQty, " _
			& "TotalCost, CostPerQty, RunningSFGQty, RunningSFGCost, CreatedBy, DateCreated) values ('" _
				& vJONO & "'," & vOperNo & ",'','','','" & vJOQty & "','" _
				& vRMTotalCost & "','" & vCostPerQty & "','" & vRunningSFGQty & "','" _
				& vRunningSFGCost & "','" & Session("uid") & "','" & Now & "') "

        cm.CommandText = vSQL

        Try
			cm.ExecuteNonQuery()
		Catch ex As SqlClient.SqlException
			Response.Write("Error in SQL query; Error code: 05142017 2014 " & ex.Message)
		End Try

		c.Close()
		cm.Dispose()
		c.Dispose()

	End Sub

	Private Sub ProductionMaterial_Receiving_Header()
        '" < td style='width:70px;' class='titleBar'>Operation Order</td>" & _
        '  "<td class='titleBar'>Process Description</td>" & _

        Select Case Request.Item("pMode")
            Case "ReceiveMaterials"
                vHeader = "<tr class='bg-info text-white'>" _
                    & "<td>Material Code</td>" _
                   & "<td>Description</td>" _
                   & "<td>Release By</td>" _
                   & "<td>Qty Release</td>" _
                   & "<td>LOTNO</td>" _
                   & "<td>Roll No</td>" _
                   & "<td>UOM</td>"

                vHeader += "<td>Qty<br />Received</td>"
                vHeader += "</tr>"

            Case "RequestMaterials"
                vHeader = "<tr class='bg-info text-white'>" &
                   "<td>Material Code</td>" &
                   "<td>Description</td>" &
                   "<td>UOM</td>"
                vHeader += "<td>Qty<br />Request</td></tr>"


            Case "ReturnMaterials"
                vHeader += "<td style='width:70px;' class='titleBar'>Qty<br />Return</td></tr>"
        End Select

    End Sub

    Private Sub ProductionMaterial_Receiving()

        If Session("vProc_Matlist") = "" Then
            'vScript = "alert('Login session has expired. Please re-login again.'); window.close();"
            Exit Sub
        End If

        Dim c As New SqlClient.SqlConnection(connStr)
        Dim cm As New SqlClient.SqlCommand
        Dim rs As SqlClient.SqlDataReader
        Dim cm_sub As New SqlClient.SqlCommand
		''Dim rs_sub As SqlClient.SqlDataReader
		Dim cm_sub2 As New SqlClient.SqlCommand

        Dim vSQL As String = ""
        Dim vID As String = ""
        Dim vValue As Decimal = 0
        Dim iCtr As Integer = 0
        Dim vFilter As String = ""

        Dim vBOM As String = Request.Item("pBOM")
        Dim vBOMRev As String = Request.Item("pBOMRev")
        Dim vJONO As String = Request.Item("pJO")
        Dim vSection As String = Request.Item("pSection")
        Dim vProcess As String = Request.Item("pProcess")
        Dim vOperNo As String = Request.Item("pOperNo")
		Dim vTranIdList As String = ""
        Dim vQty As String = ""
        Dim vQtyReceived As Decimal = 0

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


        'If Session("ViewAll") = "YES" Then
        '    vFilter = ""
        'Else
        '    'vFilter = " and DateReceived is null"
        '    Session.Remove("ViewAll")
        'End If

        If txtCode.Value.Trim <> "" Then
            vFilter += " and Item_Cd='" & txtCode.Value.Trim & "' "
        Else
            'vFilter += " and Item_Cd in (" & Session("vProc_Matlist") & ") "
        End If

        vSQL = "select TranId, Item_Cd, Qty, LotNo, RollNo, UOM, RQty, DateReceived, " _
                    & "(select (Descr + ' ' + Descr) as Descr from item_master " _
                        & "where item_master.Item_Cd=prod_rawmaterial.Item_Cd) as vDescr, " _
                    & "(select TOP 1 SFG_Descr from bom_process e where e.SFG_Cd=Item_Cd and e.BOM_Cd='" & vBOM & "') As SFGName, " _
                    & "CreatedBy, DateCreated, " _
                    & "(select FullName from user_list where user_id=CreatedBy) as vRec_Admin " _
                & "from prod_rawmaterial where TranType='RW' and " _
                & "BOM='" & vBOM & "' and " _
                & "Revision='" & vBOMRev & "' and " _
                & "JONO='" & vJONO & "' and " _
                & "Sect_Cd='" & vSection & "' and " _
                & "Proc_Cd='" & vProcess & "' and " _
                & "OperOrder=" & Request.Item("pOperNo") _
                & " " & vFilter _
                & " order by vDescr"

        'Response.Write(vSQL)

        cm.CommandText = vSQL

        Try
            rs = cm.ExecuteReader
            Do While rs.Read
                iCtr += 1
                vID = iCtr & "_" & rs("Item_Cd")

                vQty = IIf(IsDBNull(rs("RQty")), rs("Qty"), rs("RQty"))

                If Not IsDBNull(rs("RQty")) Then
                    vQtyReceived = rs("Qty") - rs("RQty")
                Else
                    vQtyReceived = rs("Qty")
                End If







                vData += "<tr style='height:24px;'>" &
                    "<td class='labelC'>" & rs("Item_Cd") & "</td>"


                If Not IsDBNull(rs("vDescr")) Then
                    vData += "<td Class='labelL'>" & rs("vDescr") & "</td>"
                Else
                    vData += "<td Class='labelL'>" & rs("SFGName") & "</td>"
                End If


                'CreatedBy, DateCreated
                vData += "<td>" & rs("vRec_Admin") & "<br>" & Format(CDate(rs("DateCreated")), "MM/dd/yyyy HH:mm") & "</td>"

                vData += "<td>" & rs("Qty") & "</td>" _
                    & "<td>" & rs("LotNo") & "</td>" _
                    & "<td>" & rs("RollNo") & "</td>" _
                    & "<td>" & rs("UOM") & "</td>" _
                    & "<td>"


                If vQtyReceived > 0 Then
                    vData += "<input style='width:80px; height:21px; margen:auto; text-align:right; padding-right:5px' type='text' " &
                        "name='" & vID & "' id='" & vID & "' value='" & vQtyReceived & "' />"


                    If h_Action.Value = "Save" Then
                        If Request.Item(vID).Trim = "" Then
                            vValue = 0
                        Else
                            vValue = Request.Item(vID)
                        End If

                        If vValue > 0 Then
                            vSQL = "update prod_rawmaterial set "

                            If IsDBNull(rs("RQty")) Then
                                vSQL += "RQty='" & vValue & "', "
                            Else
                                vSQL += "RQty='" & vValue & "' + RQty, "
                            End If

                            vSQL += "ReceivedBy='" & Session("uid") & "', " _
                                & "DateReceived='" & Now() & "' " _
                                & "where TranId=" & rs("TranId")

                            cm_sub2.CommandText = vSQL

                            'Response.Write(vSQL)

                            Try
                                cm_sub2.ExecuteNonQuery()
                                vTranIdList += rs("TranId") & ","
                            Catch ex As SqlClient.SqlException
                                Response.Write("Error in SQL query Receiving raw-materials :  " & ex.Message)
                            End Try
                        End If

                    End If
                Else
                    vData += vQty
                End If



                vData += "</td></tr>"


            Loop
            rs.Close()


            ' ====================================================================================================
            ' ====================================================================================================

            vData += "<tr style='height:30px;'><td colspan='8'><h4><b>List of SFG materials used from other process</b></h4></td></tr>"

            vSQL = "select TranId, Item_Cd, Qty, LotNo, UOM, RQty, DateReceived, " _
                    & "(select top 1 SFG_Descr from bom_process where SFG_Cd=Item_Cd) as vDescr, " _
                    & "(select Emp_Fname + ' ' + Emp_Lname from emp_master where ReleaseBy=Emp_Cd) as vCompletionBy, " _
                    & "DateRelease " _
                & "from prod_rawmaterial where " _
                & "BOM='" & vBOM & "' and " _
                & "Revision='" & vBOMRev & "' and " _
                & "JONO='" & vJONO & "' and " _
                & "Sect_Cd='" & vSection & "' and " _
                & "Proc_Cd='" & vProcess & "' and TranType='Raw-Mats from Completion' " _
                & " order by vDescr"

            'Response.Write(vSQL)
            cm.CommandText = vSQL
            rs = cm.ExecuteReader
            Do While rs.Read
                iCtr += 1
                vID = iCtr & "_" & rs("Item_Cd")

                vQty = IIf(IsDBNull(rs("RQty")), rs("Qty"), rs("RQty"))

                vData += "<tr style='height:24px;'>" &
                    "<td>" & rs("Item_Cd") & "</td>" &
                    "<td>" & rs("vDescr") & "</td>" &
                    "<td><b>Completion By:</b><br>" & rs("vCompletionBy") & "<br>" _
                        & Format(CDate(rs("DateRelease")), "MM/dd/yyyy HH:mm") & "</td>" &
                    "<td>" & rs("Qty") & "</td>" &
                    "<td>" & rs("LotNo").ToSting & "</td>" &
                    "<td>-</td>" &
                    "<td>" & rs("UOM") & "</td>" &
                    "<td>"

                If IsDBNull(rs("DateReceived")) Then
                    vData += "<input style='width:80px; height:21px; margen:auto; text-align:right; padding-right:5px' type='text' " &
                        "name='" & vID & "' id='" & vID & "' value='" & vQty & "' />"

                    If h_Action.Value = "Save" Then

                        Try
                            If Request.Item(vID).Trim = "" Then
                                vValue = 0
                            End If
                        Catch ex As Exception
                            vValue = Request.Item(vID)
                        End Try

                        'If Request.Item(vID).Trim = "" Then
                        '    vValue = 0
                        'Else
                        '    vValue = Request.Item(vID)
                        'End If

                    End If
                Else
                    vData += vQty
                End If
                vData += "</td></tr>"


            Loop
            rs.Close()

        Catch ex As SqlClient.SqlException
            vScript = "alert('Error occurred while trying to buil shift reference. Error is: " & _
                ex.Message.Replace(vbCrLf, "").Replace("'", "") & "');"
        End Try

        c.Close()
        cm.Dispose()
        c.Dispose()


        If h_Action.Value = "Save" Then
            ProductionMaterial_Cost(vTranIdList)
            vScript = "alert('Successfully saved.');  window.close();" '" + h_Action.Value + "' window.opener.document.form1.submit();
            h_Action.Value = ""
        End If


    End Sub

    Protected Sub cmdSave_Click(sender As Object, e As EventArgs) Handles cmdSave.Click
        h_Action.Value = "Save"
        Builder()
        h_Action.Value = "" 
    End Sub

    Protected Sub cmdView_Click(sender As Object, e As EventArgs) Handles cmdViewAll.Click
        Session("ViewAll") = "YES"
        Builder()

    End Sub
  
    Protected Sub cmdViewPending_Click(sender As Object, e As EventArgs) Handles cmdViewPending.Click
        Session("ViewAll") = "NO"
        Builder()
    End Sub

    Private Sub btnScan_ServerClick(sender As Object, e As EventArgs) Handles btnScan.ServerClick
        Scan()
        Builder()
    End Sub

    Private Sub Scan()

        Dim c As New SqlClient.SqlConnection(connStr)
        Dim cm As New SqlClient.SqlCommand
        Dim rs As SqlClient.SqlDataReader

        Dim vBOM As String = Request.Item("pBOM")
        Dim vBOMRev As String = Request.Item("pBOMRev")
        Dim vJONO As String = Request.Item("pJO")
        Dim vSection As String = Request.Item("pSection")
        Dim vProcess As String = Request.Item("pProcess")
        Dim vOperNo As String = Request.Item("pOperNo")

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

        vSQL = "select count(Item_Cd) as ItemCrt from prod_rawmaterial " _
            & "where JONO='" & vJONO & "' and  " _
            & "Item_Cd ='" & txtCode.Value.Trim & "' and  " _
            & "Sect_Cd ='" & vSection & "' and  " _
            & "Proc_Cd ='" & vProcess & "' and  " _
            & "OperOrder=" & vOperNo

        'Response.Write(vSQL)

        cm.CommandText = vSQL
        lblScanLabel.Text = "No records found."
        Try
            rs = cm.ExecuteReader
            If rs.Read Then
                If rs("ItemCrt") > 0 Then
                    lblScanLabel.Text = "Material found."
                    lblScanLabel.ForeColor = Color.Green
                Else
                    lblScanLabel.Text = "No records found."
                    lblScanLabel.ForeColor = Color.Red
                End If
            End If
            rs.Close()

        Catch ex As SqlClient.SqlException
            vScript = "alert('Error occurred while trying to buil shift reference. Error is: " & _
                ex.Message.Replace(vbCrLf, "").Replace("'", "") & "');"
        End Try

        c.Close()
        cm.Dispose()
        c.Dispose()

    End Sub

    Protected Sub cmdViewSFG_Click(sender As Object, e As EventArgs) Handles cmdViewSFG.Click
        CollectCompletion("COMPLETION")
    End Sub

    Private Sub CollectCompletion(pType As String)
        Dim vBOM As String = Request.Item("pBOM")
        Dim vRev As String = Request.Item("pBOMRev")
        Dim vJO As String = Request.Item("pJO")
        Dim vSect As String = Request.Item("pSection")
        Dim vProc As String = Request.Item("pProcess")
        Dim vSFG As String = Request.Item("pSFG")
        Dim vOperOrder As String = Request.Item("pOperNo")

        Dim vNet As Decimal = 0
        Dim vGross As Decimal = 0
        Dim vCore As Decimal = 0
        Dim vSQL As String = ""

        Dim vTllCore As Decimal = 0
        Dim vTllNet As Decimal = 0
        Dim vTllGross As Decimal = 0
        Dim vTllMeter As Decimal = 0
        Dim vTllQty As Decimal = 0
        Dim vDateEdited As String = ""
        Dim vEditedBy As String = ""
        Dim vDateVoid As String = ""
        Dim vVoidBy As String = ""
        Dim vFullName As String = ""
		Dim vGrabDate As String = ""

		Dim c As New SqlClient.SqlConnection
        Dim cm As New SqlClient.SqlCommand
        Dim rs As SqlClient.SqlDataReader

        Dim cmSub As New SqlClient.SqlCommand
        Dim rsSub As SqlClient.SqlDataReader

        c.ConnectionString = connStr
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
        cmSub.Connection = c

        vData = "<tr>" _
            & "<td>Oper No.</td>" _
            & "<td>JONO</td>" _
            & " <td>BTH NO</td>" _
            & "<td>Core Weight</td>" _
            & "<td>Net Weight</td>" _
            & "<td>Gross Weight</td>" _
            & "<td>Meter</td>" _
            & "<td>Qty</td>" _
            & "<td style='width:100px'>Created By</td>" _
            & "<td>Used By</td>" _
            & "<td style='width:88px'></td>" _
        & "</tr> "

        ''& "<td style='width:100px'>Void By</td>" _

        vSQL = "select TranId,Qty,JONO,BatchNo,PrevBatchNoA,PrevBatchNoB," _
            & "CoreWeight,NetWeight,GrossWeight,TranType,Meter,CreatedBy,DateCreated, " _
            & "(select Emp_Fname + ' ' + Emp_Lname from emp_master where CreatedBy=Emp_Cd) as vOps, " _
            & "(select Emp_Fname + ' ' + Emp_Lname from emp_master where EditedBy=Emp_Cd) as vEditBy, EditedBy, DateEdited, " _
            & "(select Emp_Fname + ' ' + Emp_Lname from emp_master where VoidBy=Emp_Cd) as vVoidBy,VoidBy, DateVoid, " _
            & "OperOrder, Sect_Cd," _
            & "(select CreatedBy from prod_rawmaterial c where c.BatchNo=a.BatchNo) as vUseBy," _
            & "(select DateCreated from prod_rawmaterial c where c.BatchNo=a.BatchNo) as vDateCreated," _
            & "(select Descr from ref_item_process b where b.Sect_Cd=a.Sect_Cd and b.Proc_Cd=a.Proc_Cd and b.Proc_Cd not in ('3002')) as vProcess " _
            & "from prod_completion a where " _
            & "JONO='" & vJO & "' and Proc_Cd='" & cmdProcess.SelectedValue & "' and TranType='COMPLETION' order by TranType,DateCreated"


        'Response.Write(vSQL)
        cm.CommandText = vSQL

        Try
            rs = cm.ExecuteReader
            Do While rs.Read

                vCore = IIf(IsDBNull(rs("CoreWeight")), 0, rs("CoreWeight"))
                vGross = IIf(IsDBNull(rs("GrossWeight")), 0, rs("GrossWeight"))

                vNet = vGross - vCore
                vData += "<tr>"

                vData += "<td>" & rs("OperOrder") & "</td>" _
                    & "<td>" & rs("Sect_Cd") & "/<br>" & rs("vProcess") & "</td>" _
                    & "<td>" & rs("BatchNo") & "</td>" _
                    & "<td style='text-align:right;'>" & vCore & "</td>" _
                    & "<td style='text-align:right;'>" & vNet & "</td>" _
                    & "<td style='text-align:right;'>" & vGross & "</td>" _
                    & "<td style='text-align:right;'>" & rs("Meter") & "</td>" _
                    & "<td style='text-align:right;'>" & rs("Qty") & "</td>" _
                    & "<td style='text-align:left;'>" & rs("vOps") & "<br>" _
                        & Format(rs("DateCreated"), "MM/dd/yyyy HH:mm") & "</td>"

                If Not IsDBNull(rs("vEditBy")) Or rs("vEditBy").ToString = "" Then
                    vEditedBy = rs("EditedBy").ToString
                End If
                If Not IsDBNull(rs("DateEdited")) Then
                    vDateEdited = "<br>" & Format(rs("DateEdited"), "MM/dd/yyyy HH:mm")
                End If


                '========================================================================================
                ' USED BY Column
                '========================================================================================

                vSQL = "select Emp_Fname+ ' ' + Emp_Lname as FullName, Emp_Cd from emp_master where Emp_Cd='" & rs("vUseBy") & "'"
                cmSub.CommandText = vSQL

                rsSub = cmSub.ExecuteReader
                If rsSub.Read Then
                    vFullName = rsSub("FullName")

                    If vFullName.Trim = "" Then
                        vFullName = rsSub("Emp_Cd")
                    End If

                End If
                rsSub.Close()
				vData += "<td>" & vFullName & "<br>" & rs("vDateCreated") & "</td>"

				vGrabDate = IIf(Not IsDBNull(rs("vDateCreated")), rs("vDateCreated"), "")
				'========================================================================================
				' 
				'========================================================================================

				'If Not IsDBNull(rs("vVoidBy")) Or rs("vVoidBy").ToString = "" Then
				'    vVoidBy = rs("VoidBy").ToString
				'End If
				'If Not IsDBNull(rs("DateVoid")) Then
				'    vDateVoid = "<br>" & Format(rs("DateVoid"), "MM/dd/yyyy HH:mm")
				'Else
				'    vDateVoid = ""
				'End If
				'vData += "<td>" & vVoidBy & vDateVoid & "</td>"

				vData += "<td>"

                If vGrabDate <> "" Then

                    If Session("uid") = rs("vUseBy") Then
                        vData += "<Button type='button' class='btn btn-sm btn-danger' data-toggle='modal' " _
                                    & "onclick='RemoveRawmats(""" & rs("BatchNo") & """)' " _
                                    & "data-target='#ModelDelete'>Delete Raw-Mats</button>"
                    End If

                Else
                    vData += "<Button type='button' class='btn btn-sm btn-success' data-toggle='modal' " _
                            & "onclick='UsedRawmats(" & rs("TranId") & ",""" _
                                & rs("PrevBatchNoA") & """,""" _
                                & vCore & """,""" _
                                & rs("Meter") & """,""" _
                                & rs("Qty") & """)' " _
                            & "data-target='#myModal'>Use Raw-Mats</button>"
                End If

                vFullName = ""

                vData += " </td></tr>"

                If vDateVoid.ToString.Trim = "" Then
                    vTllCore += vCore
                    vTllNet += vNet
                    vTllGross += vGross
                    vTllMeter += rs("Meter")
                    vTllQty += rs("Qty")
                End If

                vDateEdited = ""
                vEditedBy = ""
                vVoidBy = ""
                vDateVoid = ""

            Loop
            rs.Close()

            'vData += "<tr style='font-weight:bold; color: #434343; background:#93c8ff;'>" _
            '    & "<td class='text-right' colspan=4><b>TOTAL :</b></td>" _
            '    & "<td class='text-right'>" & vTllCore & "</td>" _
            '    & "<td class='text-right'>" & vTllNet & "</td>" _
            '    & "<td class='text-right'>" & vTllGross & "</td>" _
            '    & "<td class='text-right'>" & vTllMeter & "</td>" _
            '    & "<td class='text-right'>" & vTllQty & "</td>" _
            '    & "<td colspan='4'></td>" _
            '& "</tr>"

        Catch ex As SqlClient.SqlException
            Response.Write("Error in SQL query Get Process Details Error Code : 06142017-1727 " & ex.Message)
        End Try

        c.Close()
        c.Dispose()
        cm.Dispose()

    End Sub

    Private Sub cmdGrabRawMats_Click(sender As Object, e As EventArgs) Handles cmdGrabRawMats.Click

        Dim vSect As String = Request.Item("pSection")
		Dim vProc As String = Request.Item("pProcess")

		Dim vOperNo As String = Request.Item("pOperNo")

		vSQL = "insert into prod_rawmaterial " _
			& "(BatchNo, BOM, Revision, Sect_Cd, Proc_Cd, JONO, OperOrder, Item_Cd, UOM, LotNo, Qty, ItemCost, TotalCost," _
			& "RQty, Remarks, TranType, ReleaseBy, CreatedBy,DateRelease, DateCreated, ReceivedBy, DateReceived) "

		vSQL += "select BatchNo, BOM, Revision,'" & vSect & "','" & vProc & "', JONO, " & vOperNo & ", SFG_Cd, UOM, BatchNo," _
			& "NetWeight, ProdCost, ProdTotalCost, GrossWeight,'RMFC', 'Raw-Mats from Completion', CreatedBy,'" & Session("uid") & "',DateCreated," _
			& "'" & Now() & "','" & Session("uid") & "','" & Now() & "' " _
			& "from prod_completion where TranId=" & h_TranId.Value.Trim

		'Response.Write(vSQL)
		CreateRecord(vSQL)
		ProductionMaterial_Receiving()
		ProductionMaterial_Cost("x")
		'Response.Write(vSQL)
	End Sub

    Private Sub cmdDelRawMats_Click(sender As Object, e As EventArgs) Handles cmdDelRawMats.Click

        Dim vSect As String = Request.Item("pSection")
        Dim vProc As String = Request.Item("pProcess")

        vSQL = "Delete from prod_rawmaterial where BatchNo='" & h_TranId.Value.Trim & "'"

        'Response.Write(vSQL)

        CreateRecord(vSQL)
		ProductionMaterial_Receiving()
		ProductionMaterial_Cost("x")
		'Response.Write(vSQL)
	End Sub

End Class


