Imports denaro
Imports System.Diagnostics
Imports System.ComponentModel

Partial Class build_operations_production_menus
    Inherits System.Web.UI.Page
    Public vScript As String = ""
    Public vMenus As String = ""

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        If Session("uid") = Nothing Or Session("uid") = "" Then
            vScript = "alert('Login session has expired. Please re-login again.'); parent.jQuery.fn.colorbox.close();"
            Exit Sub
        End If

        Session("vProc_Matlist") = ""

        If Not IsPostBack Then
            Session("MonitoringId") = ""

            Dim vParamList() As String

            vParamList = Request.Item("pTranId").Split("~")
            txtJONO.Value = vParamList(0)
            txtOperNo.Value = vParamList(1)
            txtBOM.Value = vParamList(2)
            txtBomRev.Value = vParamList(3)
            txtSect.Value = vParamList(4)
            txtProcess.Value = vParamList(5)
            txtSFG.Value = vParamList(6)


            lblItemName.Text = GetRef("select SFG_Descr as vDescr from bom_process where SFG_Cd='" & txtSFG.Value & "'", "")
            lblSecDescr.Text = GetRef("select Descr from ref_emp_section where Section_Cd ='" & txtSect.Value & "'", "")
            lblProsDescr.Text = GetRef("select Descr from ref_item_process where Proc_Cd='" & txtProcess.Value & "' and Sect_Cd='" & txtSect.Value & "'", "")
            lblOperNo.Text = vParamList(1)

        End If

        Get_Details()

        'If Session("MonitoringId") <> "Empty" Then
        '    vScript = "alert('Login session has expired. Please re-login again.'); parent.jQuery.fn.colorbox.close();"
        '    Exit Sub
        'End If

        If txtMode.Value.Trim <> "" And txtMode.Value.Trim <> "reload" Then
            Prod_Monitoring()
        End If

    End Sub
    Private Sub Get_Details()

        Dim c As New SqlClient.SqlConnection(connStr)
        Dim cm As New SqlClient.SqlCommand
        Dim rs As SqlClient.SqlDataReader
        Dim vSQL As String = ""

        c.ConnectionString = connStr
        Try
            c.Open()
        Catch ex As SqlClient.SqlException
            vScript = "alert('Error occurred while trying to connect to database. Error code 101; Error is: " & ex.Message.Replace(vbCrLf, "").Replace("'", "") & "');"
            c.Dispose()
            cm.Dispose()
            Exit Sub
        End Try

        cm.Connection = c

        Dim vBOMParentItem As String = ""
        vBOMParentItem = GetRef("select TranId from bom_process where " _
            & "BOM_Cd='" & txtBOM.Value & "' and " _
            & "Revision='" & txtBomRev.Value & "' and " _
            & "OperOrder='" & txtOperNo.Value & "' ", "")

        If vBOMParentItem = "" Then
            vScript = "alert('No Item found.');"
            Exit Sub
        End If

        Dim vProcessMat As String = ""

        vSQL = "select Item_Cd from bom_materials where " & _
                    "Parent_TranId=" & vBOMParentItem & " and " & _
                    "BOM_Cd='" & txtBOM.Value & "' and " & _
                    "Revision='" & txtBomRev.Value & "'"

        cm.CommandText = vSQL

        Try
            rs = cm.ExecuteReader
            Do While rs.Read
                vProcessMat += "'" & rs("Item_Cd") & "',"
            Loop
            rs.Close()
        Catch ex As SqlClient.SqlException
            vScript = "alert('Error occurred while trying to collect all item under operation number or process. Error is: " & _
                ex.Message.Replace(vbCrLf, "").Replace("'", "") & "');"
        End Try


        If vProcessMat.Trim <> "" Then
            vProcessMat = Mid(vProcessMat, 1, Len(vProcessMat) - 1)
        End If

        Session("vProc_Matlist") = vProcessMat

        ' TABLE : jo_header =======================================================================================
        ' GET THE JOB ORDER DETAILS ===============================================================================

        If txtTranId.Value.Trim = "" Then
            vSQL = "select TranId,Alt_Cd,JobOrderNo,SalesOrderNo,PurchaseOrderNo,OrderQty " _
                & "from jo_header where TranId='" & txtJONO.Value & "'"
        Else
            vSQL = "select TranId,Alt_Cd,JobOrderNo,SalesOrderNo,PurchaseOrderNo,OrderQty " _
                & "from jo_header where TranId='" & txtTranId.Value & "'"
        End If

        cm.CommandText = vSQL
        'Response.Write(vSQL)
        Try
            rs = cm.ExecuteReader
            If rs.Read Then
                txtTranId.Value = IIf(IsDBNull(rs("TranId")), "", rs("TranId"))
                lblQtyOrder.Text = IIf(IsDBNull(rs("OrderQty")), "", rs("OrderQty"))
                txtJONO.Value = IIf(IsDBNull(rs("JobOrderNo")), "", rs("JobOrderNo"))
                txtSONO.Value = IIf(IsDBNull(rs("SalesOrderNo")), "", rs("SalesOrderNo"))
                txtPONO.Value = IIf(IsDBNull(rs("PurchaseOrderNo")), "", rs("PurchaseOrderNo"))

                lblJONO.Text = IIf(IsDBNull(rs("JobOrderNo")), "", rs("JobOrderNo"))
                lblGCAS.Text = IIf(IsDBNull(rs("Alt_Cd")), "", rs("Alt_Cd"))
            End If
            rs.Close()
        Catch ex As SqlClient.SqlException
            vScript = "alert('Error occurred while trying to retrieve Budget Info. Error code 1022; Error is: " _
                & ex.Message.Replace(vbCrLf, "").Replace("'", "") & "');"
            c.Close()
            c.Dispose()
            cm.Dispose()
            Exit Sub
        End Try

        vSQL = "select TranId, StartUpRun, InitialRun, ProdRun from prod_monitoring " _
            & "where OperOrder=" & lblOperNo.Text & " and " _
            & "JobOrderNo ='" & lblJONO.Text & "' and " _
            & "Proc_Cd='" & txtProcess.Value & "' and " _
            & "Sect_Cd='" & txtSect.Value & "'"


        cm.CommandText = vSQL

        Try
            rs = cm.ExecuteReader
            If rs.Read Then
                Session("MonitoringId") = rs("TranId")
                lblRun1.Text = IIf(IsDBNull(rs("StartUpRun")), "", rs("StartUpRun"))
                lblRun2.Text = IIf(IsDBNull(rs("InitialRun")), "", rs("InitialRun"))
                lblRun3.Text = IIf(IsDBNull(rs("ProdRun")), "", rs("ProdRun"))
            Else
                Session("MonitoringId") = "Empty"
            End If
            rs.Close()
        Catch ex As SqlClient.SqlException
            vScript = "alert('Error occurred while trying to retrieve PRODUCTION tabel Info. Error code 150; Error is: " & _
                ex.Message.Replace(vbCrLf, "").Replace("'", "") & "');"
            c.Close()
            c.Dispose()
            cm.Dispose()
            Exit Sub
        End Try

        Menus_Settings()

        c.Close()
        c.Dispose()
        cm.Dispose()
    End Sub

    Private Sub Menus_Settings()
        vMenus = ""
        txtStatus.Value = GetRef("select count(JobOrderNo) from prod_monitoring where JobOrderNo='" & txtJONO.Value & "' and SFG_Item_Cd='" & txtSFG.Value & "'", "Pending")

        'Response.Write("select count(JobOrderNo) from prod_monitoring where JobOrderNo='" & txtJONO.Value & "' and SFG_Item_Cd='" & txtSFG.Value & "'")

        If txtStatus.Value = 1 Then
            txtStartUpRun.Value = GetRef("select StartUpRun from prod_monitoring where JobOrderNo='" & _txtJONO.Value & "' and SFG_Item_Cd='" & txtSFG.Value & "'", "Pending")
            txtInitialRun.Value = GetRef("select InitialRun from prod_monitoring where JobOrderNo='" & txtJONO.Value & "' and SFG_Item_Cd='" & txtSFG.Value & "'", "Pending")
            txtProdRun.Value = GetRef("select ProdRun from prod_monitoring where JobOrderNo='" & txtJONO.Value & "' and SFG_Item_Cd='" & txtSFG.Value & "'", "Pending")

            If txtStartUpRun.Value = "" Then
                vMenus += "<tr><td id='tdRun1' onclick='RunProcess(this.id)'><img alt='' src='../images/menu/startup.png' style='margin-top:11px'/><br/>Startup Run</td>"
            Else
                vMenus += "<tr><td class='doneProcess'><img alt='' src='../images/menu/startup.png' style='margin-top:11px'/><br/>Startup Run</td>"
            End If

            If txtInitialRun.Value = "null" Then
                vMenus += "<td id='tdRun2' onclick='RunProcess(this.id)'><img alt='' src='../images/menu/initialrun.png' style='margin-top:11px'/><br/>Initial Run</td>"
                vMenus += "<td><img alt='' src='../images/menu/prodrun.png' style='margin-top:11px'/><br/>Prod Run</td></tr>"
            Else
                vMenus += "<td class='doneProcess'><img alt='' src='../images/menu/initialrun.png' style='margin-top:11px'/><br/>Initial Run</td>"

                If txtProdRun.Value = "null" Then
                    vMenus += "<td id='tdRun3' onclick='RunProcess(this.id)'><img alt='' src='../images/menu/prodrun.png' style='margin-top:11px'/><br/>Prod Run</td></tr>"
                Else
                    vMenus += "<td class='doneProcess'><img alt='' src='../images/menu/prodrun.png' style='margin-top:11px'/><br/>Prod Run</td></tr>"
                End If
            End If

        Else
            vMenus += "<tr><td id='tdRun1' onclick='RunProcess(this.id)'><img alt='' src='../images/menu/startup.png' style='margin-top:11px'/><br/>Startup Run</td>"
            vMenus += "<td id='tdRun2' onclick='RunProcess(this.id)'><img alt='' src='../images/menu/initialrun.png' style='margin-top:11px'/><br/>Initial Run</td>"
            vMenus += "<td id='tdRun3' onclick='RunProcess(this.id)'><img alt='' src='../images/menu/prodrun.png' style='margin-top:11px'/><br/>Prod Run</td></tr>"
        End If


        'vMenus += "<tr><td id='tdRun1' onclick='RunProcess(this.id)' colspan='3'><h4></h4></td></tr>"
    End Sub

    Private Sub Prod_Monitoring()
        Dim c As New SqlClient.SqlConnection
        Dim cm As New SqlClient.SqlCommand
		Dim rs As SqlClient.SqlDataReader

		Dim vSQL As String = ""
        Dim vSONO As String = ""
		Dim vPONO As String = ""

		Dim vQtyDispatch As Integer
		Dim vQtyReceived As Integer

		c.ConnectionString = connStr
        Try
            c.Open()
        Catch ex As SqlClient.SqlException
            vScript = "alert('Error occurred while trying to connect to database. Error code 101; Error is: " & ex.Message.Replace(vbCrLf, "").Replace("'", "") & "');"
            c.Dispose()
            cm.Dispose()
            Exit Sub
        End Try
        cm.Connection = c

		vSQL = "select sum(Qty) as vQtyDispatch, sum(RQty) as vQtyReceived " _
			& "FROM prod_rawmaterial " _
			& "where JONO='" & txtJONO.Value & "' and OperOrder=" & txtOperNo.Value

		cm.CommandText = vSQL

		Try
			rs = cm.ExecuteReader
			If rs.Read Then
				vQtyDispatch = IIf(IsDBNull(rs("vQtyDispatch")), 0, rs("vQtyDispatch"))
				vQtyReceived = IIf(IsDBNull(rs("vQtyReceived")), 0, rs("vQtyReceived"))
			End If
			rs.Close()
		Catch ex As SqlClient.SqlException
			vScript = "alert('Error occurred while trying to collect all item under operation number or process. Error is: " & _
				ex.Message.Replace(vbCrLf, "").Replace("'", "") & "');"
		End Try

		vSONO = IIf(txtSONO.Value.Trim = "", "NULL", txtSONO.Value.Trim)
		vPONO = IIf(txtPONO.Value.Trim = "", "NULL", txtPONO.Value.Trim)

		'Response.Write(vQtyReceived & " - " & (vQtyDispatch / 100) * 80 & ">=" & vQtyDispatch)
		Select Case txtMode.Value.Trim
			Case "tdRun1"
				vSQL = "insert into prod_monitoring (JobOrderNo,SalesOrderNo,PurchaseOrderNo,BOM_Cd,BOMRev, " & _
						"Sect_Cd,Proc_Cd,OperOrder,FG_Item_Cd,SFG_Item_Cd,QtyRequired,QtyComplete,ProdStatus, " & _
						"Remarks,StartUpRun,CreatedBy,DateCreated) values ('" & _
							txtJONO.Value & "','" & vSONO & "','" & vPONO & "'," & txtBOM.Value & "," & txtBomRev.Value & ",'" & _
							txtSect.Value & "','" & txtProcess.Value.Trim & "'," & txtOperNo.Value.Trim & ",'" & txtFG.Value & "','" & txtSFG.Value & "','" & txtQty.Value & "',0,'JOPROD5','START-UP RUN','" & _
							Now & "','" & Session("uid") & "','" & Now & "')"

			Case "tdRun2"
				vSQL = "update prod_monitoring set Remarks='INITIAL RUN', InitialRun='" & Now & "', ProdStatus='JOPROD6' ,CreatedBy='" & Session("uid") & "',DateCreated='" & Now & "' " _
						& "Where TranId=" & Session("MonitoringId")

			Case "tdRun3"
				vSQL = "update prod_monitoring set Remarks='PRODUCTION RUN', ProdRun='" & Now & "', ProdStatus='JOPROD7' ,CreatedBy='" & Session("uid") & "',DateCreated='" & Now & "' " _
						& "Where TranId=" & Session("MonitoringId")

		End Select

		Dim vIsValid As String = "N"

		Select Case txtProcess.Value
			Case "3002" 'FOR INK MIXING ONLY
				vIsValid = "Y"
			Case "6005" 'Slitting
				vIsValid = "Y"
			Case "7003" 'Bagmaking
				vIsValid = "Y"
			Case Else
				vIsValid = "N"
		End Select

		If vIsValid = "Y" Then
			cm.CommandText = vSQL
			Try
				cm.ExecuteNonQuery()

				vSQL = "insert into prod_monitoring_log select * from prod_monitoring where JobOrderNo='" & txtJONO.Value & "'"
				cm.CommandText = vSQL
				Try
					cm.ExecuteNonQuery()
				Catch ex As SqlClient.SqlException
					Response.Write("Error in SQL query creating monitoring logs :  " & ex.Message)
				End Try

			Catch ex As SqlClient.SqlException
				Response.Write("Error in SQL query insert/update:  " & ex.Message)
			End Try

			c.Close()
			c.Dispose()
			cm.Dispose()
			Menus_Settings()
			Exit Sub
		End If

		If vQtyReceived > (vQtyDispatch / 100) * 80 Then
			cm.CommandText = vSQL
			Try
				cm.ExecuteNonQuery()

				vSQL = "insert into prod_monitoring_log select * from prod_monitoring where JobOrderNo='" & txtJONO.Value & "'"
				cm.CommandText = vSQL
				Try
					cm.ExecuteNonQuery()
				Catch ex As SqlClient.SqlException
					Response.Write("Error in SQL query creating monitoring logs :  " & ex.Message)
				End Try

			Catch ex As SqlClient.SqlException
				Response.Write("Error in SQL query insert/update:  " & ex.Message)
			End Try

			c.Close()
			c.Dispose()
			cm.Dispose()
			Menus_Settings()
		Else
			vScript = "alert('The system recommends completing all raw materials to be received before starting the production process..');"
			Exit Sub
		End If

	End Sub

    Protected Sub btnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click
        Dim vSQL As String = ""
        Dim vIsValid As Integer = 0

        vSQL = "select count(Emp_Cd) as EmpCtr from emp_master " _
            & "where Emp_Cd='" & txtEmpCode.Text.Trim & "' and Pos_Cd=6"

        vIsValid = GetRef(vSQL, "")

        If vIsValid = 1 Then
            vSQL = "update prod_monitoring set " _
                & "CompletedBy='" & txtEmpCode.Text.Trim & "'," _
                & "CompleteRun='" & Now & "', " _
                & "Remarks='COMPLETED' " _
                & "where TranId=" & Session("MonitoringId")

            CreateRecord(vSQL)
            vScript = "alert('Successfully saved.'); parent.jQuery.fn.colorbox.close();"
        Else
            vScript = "alert('Invalid Employee Id.');"
        End If
    End Sub

End Class
