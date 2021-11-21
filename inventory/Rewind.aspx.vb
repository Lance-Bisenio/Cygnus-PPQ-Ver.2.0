Imports denaro

Partial Class Rewind
    Inherits System.Web.UI.Page
    Public vScript As String = ""
    Public vProcess As String = ""
    Public vReturnData As String = ""
	Public vData As String = ""
	Public vHeader As String = ""
	Public vMCostSummary As String = ""
    Dim vSQL As String = ""

    Dim vJOBOM As String = ""
    Dim vJOBOMRev As Integer

	Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

		If Session("uid") = Nothing Or Session("uid") = "" Then
			vScript = "alert('Login session has expired. Please re-login again.');"
			Response.Redirect("http://" & SERVERIP & "/" & SITENAME & "/")
			Exit Sub
		End If

		If Not CanRun(Session("caption"), Request.Item("id")) Then
			Session("denied") = "1"
			Server.Transfer("../main.aspx")
			Exit Sub
		End If


		If Not IsPostBack Then

			If Request.Item("pMode") = "reload" Then
				CollectCompletion("COMPLETION")
			End If

		End If

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

		Dim vTltComp As Decimal = 0
		Dim vTtlCompVoid As Decimal = 0
		Dim vCtr As Decimal = 0

		Dim vSQL As String = ""

		Dim vTllCore As Decimal = 0
		Dim vTllNet As Decimal = 0
		Dim vTllGross As Decimal = 0
		Dim vTllMeter As Decimal = 0
		Dim vTllQty As Decimal = 0
		Dim vTtlPcsBox As Decimal = 0
		Dim vDateEdited As String = ""
		Dim vEditedBy As String = ""
		Dim vDateVoid As String = ""
		Dim vFilter As String = ""
		Dim vClass As String = ""
		Dim vPrevBatchNum As String
		Dim c As New SqlClient.SqlConnection
		Dim cm As New SqlClient.SqlCommand
		Dim rs As SqlClient.SqlDataReader
		Dim vJOList As String = ""
		Dim vJOlen As Integer = 0

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

		If txtDateFrom.Text <> "" Then
			vFilter = "where DateCreated between '" & txtDateFrom.Text & "' and '" & txtDateTo.Text & "'"
		End If


		vSQL = "select CompletionTranId from prod_sfgrewind " & vFilter

		cm.CommandText = vSQL
		Try
			rs = cm.ExecuteReader
			Do While rs.Read
				vJOList += rs(0) & ","
			Loop
			rs.Close()
		Catch ex As Exception
			Response.Write("Error in SQL query:  " & ex.Message)
		End Try


		If vJOList.Length = 0 Then
			c.Close()
			c.Dispose()
			cm.Dispose()
			Exit Sub
		End If

		vJOlen = vJOList.Length

		vSQL = "select TranId, Qty,JONO,BatchNo, " _
			& "PrevBatchNoA, " _
			& "PrevBatchNoB, ProdCost," _
			& "CoreWeight,NetWeight,GrossWeight,TranType,Meter,CreatedBy,DateCreated,Batchgroup+''+BatchgroupLine as BatGroup,BatchCtr," _
			& "(select Emp_Fname + ' ' + Emp_Lname from emp_master where CreatedBy=Emp_Cd) as vOps, " _
			& "(select RewindBy from prod_sfgrewind a where a.CompletionTranId=prod_completion.TranId) as vRewindBy, " _
			& "(select DateRewind from prod_sfgrewind a where a.CompletionTranId=prod_completion.TranId) as vDateRewind, " _
			& "(select DateCreated from prod_sfgrewind a where a.CompletionTranId=prod_completion.TranId) as vLogDate, " _
			& "(select CreatedBy from prod_sfgrewind a where a.CompletionTranId=prod_completion.TranId) as vLogBy, " _
			& "TtlPcs, TtlPcsBox " _
			& "from prod_completion where " _
			& "TranId in (" & vJOList.ToString.Substring(0, vJOlen - 1) & ") order by TranType,DateCreated"
		'Response.Write(vSQL)

		'DataRefresh(vSQL)

		cm.CommandText = vSQL

		Try
			rs = cm.ExecuteReader
			Do While rs.Read

				vCore = IIf(IsDBNull(rs("CoreWeight")), 0, rs("CoreWeight"))
				vGross = IIf(IsDBNull(rs("GrossWeight")), 0, rs("GrossWeight"))


				vNet = vGross - vCore
				vTltComp += 1
				vCtr += 1

				vData += "<tr " & vClass & ">"
				vData += "<td>" & vCtr & "</td>" _
					& "<td>" & rs("JONO") & "</td>" _
					& "<td>"

				If Not IsDBNull(rs("BatGroup")) Then
					vData += rs("BatGroup") & "-" & rs("BatchCtr")
				Else
					vData += rs("BatchNo")
				End If


				vData += "</td>" _
					& "<td style='text-align:right;'>" & vCore & "</td>" _
					& "<td style='text-align:right;'>" & vNet & "</td>" _
					& "<td style='text-align:right;'>" & vGross & "</td>" _
					& "<td style='text-align:right;'>" & rs("Meter") & "</td>" _
					& "<td style='text-align:right;'>" & rs("Qty") & "</td>" _
					& "<td style='text-align:right;'>" & rs("vOps") & "<br>" & rs("DateCreated") & "</td>" _
					& "<td style='text-align:right;'>" & rs("vLogBy") & "<br>" & rs("vLogDate") & "</td>" _
					& "<td style='text-align:right;'><span id='" & rs("TranId") & "span'>" & rs("vRewindBy") & "<br>" & rs("vDateRewind") & "</span></td>"



				vData += "<td style='width: 40px'>"

				If IsDBNull(rs("vDateRewind")) Then
					vData += "<input type='button' id='" & rs("TranId") & "' name='" & rs("TranId") & "' class='btn btn-sm btn-primary' " _
									& "onclick='CompleteRewind(" & rs("TranId") & ",this.value, """ & rs("JONO") & """)' " _
									& " value='Complete'>"
				Else
					vData += "<input type='button' id='" & rs("TranId") & "' name='" & rs("TranId") & "' class='btn btn-sm btn-danger' " _
									& "onclick='CompleteRewind(" & rs("TranId") & ",this.value, """ & rs("JONO") & """)' " _
									& " value='Cancel'>"
				End If


				vData += "</td>" _
				& "</tr>"

				If vDateVoid.ToString.Trim = "" Then
					vTllCore += vCore
					vTllNet += vNet
					vTllGross += vGross
					vTllMeter += rs("Meter")
					vTllQty += rs("Qty")
					vTtlPcsBox += IIf(IsDBNull(rs("TtlPcsBox")), 0, rs("TtlPcsBox"))
				End If

			Loop

			rs.Close()


		Catch ex As SqlClient.SqlException
			Response.Write("Error in SQL query:  " & ex.Message)
		End Try

		c.Close()
		c.Dispose()
		cm.Dispose()

	End Sub

	'Private Sub DataRefresh(pSQL As String)
	'	Dim c As New SqlClient.SqlConnection
	'	Dim da As SqlClient.SqlDataAdapter
	'	Dim ds As New DataSet
	'	Dim vFilter As String = ""
	'	Dim vTableName As String = ""
	'	Dim vSQL As String = ""
	'	Dim vDateFilter As Integer = 0


	'	c.ConnectionString = connStr

	'	'If cmbStatus.SelectedValue = "All" Then
	'	'	vFilter = "and JO_Status in ('RELEASE','PLANNING','PRE-PLAN') "
	'	'Else
	'	'	vFilter = "and JO_Status='" & cmbStatus.SelectedValue & "'"
	'	'End If

	'	'If cmbCustomer.SelectedValue <> "All" Then
	'	'	vFilter += "and Cust_Cd='" & cmbCustomer.SelectedValue & "'"
	'	'End If

	'	'If cmbItemType.SelectedValue <> "All" Then
	'	'	vFilter += "and ItemType_Cd='" & cmbItemType.SelectedValue & "'"
	'	'End If

	'	'If txtSearchBox.text.trim <> "" Then
	'	'	vFilter += "and JobOrderNo like '%" & txtSearchBox.text.trim & "%'"
	'	'	txtSearchBox.text = txtSearchBox.text.trim
	'	'End If

	'	vDateFilter = txtDateFrom.Text.Trim.Length + txtDateTo.Text.Trim.Length

	'	If vDateFilter > 0 Then
	'		vFilter += " and DueDate between '" & txtDateFrom.Text.Trim & "' and '" & txtDateTo.Text.Trim & "'"
	'	End If

	'	'If cmbMachine.SelectedValue <> "All" Then

	'	'	If txtMachList.Text.Trim = "" Then
	'	'		vFilter += " and exists (select Mach_Cd from jo_machine where IsPrimary='YES' and jo_machine.JobOrderNo=jo_header.JobOrderNo) "
	'	'	Else
	'	'		vFilter += " and exists (select Mach_Cd from jo_machine where Mach_Cd in ('" & cmbMachine.SelectedValue & "') and IsPrimary='YES' and jo_machine.JobOrderNo=jo_header.JobOrderNo) "
	'	'	End If

	'	'Else
	'	'	If txtMachList.Text.Trim <> "" Then
	'	'		vFilter += " and exists (select Mach_Cd from jo_machine where Mach_Cd in (" & txtMachList.Text.Trim & ") and IsPrimary='YES' and jo_machine.JobOrderNo=jo_header.JobOrderNo) "
	'	'	End If
	'	'End If

	'	vSQL = pSQL

	'	'Response.Write(vSQL) 'Item Code|Item Name|JONO.|Qty Order|JO Status|Release Date|Start Date|Due Date

	'	da = New SqlClient.SqlDataAdapter(vSQL, c)

	'	da.Fill(ds, "ItemMaster")
	'	tbl_JOList.DataSource = ds.Tables("ItemMaster")
	'	tbl_JOList.DataBind()
	'	'lblTotalDocs.Text = "<b>BOM Header Retrieved : " & tbl_JOList.DataSource.Rows.Count & "</b>"

	'	da.Dispose()
	'	ds.Dispose()


	'End Sub

	Private Sub btnSearch_ServerClick(sender As Object, e As EventArgs) Handles btnSearch.ServerClick
		CollectCompletion("COMPLETION")
		'Server.Transfer("rewind.aspx?pMode=reload")
	End Sub
End Class
