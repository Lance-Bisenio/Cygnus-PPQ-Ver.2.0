Imports denaro

Partial Class productioncost
	Inherits System.Web.UI.Page
	Public vScript As String = ""
	Public vHeader As String = ""
	Public vData As String = ""
	Public vMCostSummary As String = ""
	Dim vSQL As String = ""

	Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

		If Session("uid") = Nothing Or Session("uid") = "" Then
			vScript = "alert('Login session has expired. Please re-login again.'); window.close();"
			Exit Sub
		End If

		If Not IsPostBack Then
			vSQL = "select JobOrderNo+'~'+BOM_Cd+'~'+STR(BOMRev, 4, 0) as Keys,JobOrderNo from jo_header where JO_Status='RELEASE' "
			BuildCombo(vSQL, cmdJOList)

			cmdJOList.Items.Add("")
			cmdJOList.SelectedValue = ""

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
		Dim vSQL As String = ""

		Dim vTllCore As Decimal = 0
		Dim vTllNet As Decimal = 0
		Dim vTllGross As Decimal = 0
		Dim vTllMeter As Decimal = 0
		Dim vTllQty As Decimal = 0
		Dim vProdTllMeter As Decimal = 0
		Dim vProdTotalCost As Decimal = 0
		Dim vDateEdited As String = ""
		Dim vEditedBy As String = ""
		Dim vDateVoid As String = ""
		Dim vVoidBy As String = ""
		Dim vKeys() As String = cmdJOList.SelectedValue.Trim.Split("~")

		Dim c As New SqlClient.SqlConnection
		Dim cm As New SqlClient.SqlCommand
		Dim rs As SqlClient.SqlDataReader

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
		vSQL = "select TranId,Qty,JONO,BatchNo,PrevBatchNoA,PrevBatchNoB,ProdCost,ProdTotalCost," _
			& "CoreWeight,NetWeight,GrossWeight,TranType,Meter,CreatedBy,DateCreated, " _
			& "(select Emp_Fname + ' ' + Emp_Lname from emp_master where CreatedBy=Emp_Cd) as vOps, " _
			& "(select Emp_Fname + ' ' + Emp_Lname from emp_master where EditedBy=Emp_Cd) as vEditBy,EditedBy, DateEdited, " _
			& "(select Emp_Fname + ' ' + Emp_Lname from emp_master where VoidBy=Emp_Cd) as vVoidBy,VoidBy, DateVoid " _
			& "from prod_completion where " _
			& "JONO='" & vKeys(0) & "' and OperOrder=" & cmdProcessList.SelectedValue & " and " _
			& "TranType='" & pType & "' and DateVoid is null order by TranType,DateCreated"

		'Response.Write(vSQL)
		'& "JONO='" & vJO & "' and " _
		'& "Sect_Cd='" & vSect & "' and " _
		'& "Proc_Cd='" & vProc & "' and " _
		'& "OperOrder=" & vOperOrder & " " _
		'& "order by TranType,DateCreated"

		cm.CommandText = vSQL

		Try
			rs = cm.ExecuteReader
			Do While rs.Read

				vCore = IIf(IsDBNull(rs("CoreWeight")), 0, rs("CoreWeight"))
				vGross = IIf(IsDBNull(rs("GrossWeight")), 0, rs("GrossWeight"))

				vNet = vGross - vCore

				vData += "<tr >"

				vData += "<td>" & rs("BatchNo") & "</td>" _
					& "<td style='text-align:right;'>" & Format(vCore, "###,###,##0.0000") & "</td>" _
					& "<td style='text-align:right;'>" & Format(vNet, "###,###,##0.0000") & "</td>" _
					& "<td style='text-align:right;'>" & Format(vGross, "###,###,##0.0000") & "</td>" _
					& "<td style='text-align:right;'>" & Format(rs("Meter"), "###,###,##0.0000") & "</td>" _
					& "<td style='text-align:right;'>" & Format(rs("ProdCost"), "###,###,##0.0000") & "</td>"


				'vData += "<td>0.00</td><td>0.00</td>"
				vData += "" _
					& "<td style='text-align:right;'>" & Format(rs("ProdTotalCost"), "###,###,##0.0000") & "</td>"

				vData += "</tr>"

				If vDateVoid.ToString.Trim = "" Then
					vTllCore += vCore
					vTllNet += vNet
					vTllGross += vGross
					vTllMeter += rs("Meter")
					vTllQty += rs("Qty")
					vProdTotalCost += rs("ProdTotalCost")
				End If

				vDateEdited = ""
				vEditedBy = ""
				vVoidBy = ""
				vDateVoid = ""

			Loop
			rs.Close()

			vData += "<tr style='color: #434343; background:#93c8ff;'>" _
				& "<td class='text-right' ><b>TOTAL :</b></td>" _
				& "<td class='text-right'>" & Format(vTllCore, "###,###,##0.0000") & "</td>" _
				& "<td class='text-right'>" & Format(vTllNet, "###,###,##0.0000") & "</td>" _
				& "<td class='text-right'>" & Format(vTllGross, "###,###,##0.0000") & "</td>" _
				& "<td class='text-right'>" & Format(vTllMeter, "###,###,##0.0000") & "</td>" _
				& "<td class='text-right'></td>" _
				& "<td class='text-right'>" & Format(vProdTotalCost, "###,###,##0.0000") & "</td>" _
			& "</tr>"

		Catch ex As SqlClient.SqlException
			Response.Write("Error in SQL query Get Process Details function:  " & ex.Message)
		End Try

		c.Close()
		c.Dispose()
		cm.Dispose()

	End Sub

	Private Sub MaterialsCostSummary(pType As String)


		Dim vTllCore As Decimal = 0
		Dim vTllNet As Decimal = 0
		Dim vTllGross As Decimal = 0
		Dim vTllMeter As Decimal = 0
		Dim vTllQty As Decimal = 0
		Dim vProdTllMeter As Decimal = 0
		Dim vProdTotalCost As Decimal = 0
		Dim vKeys() As String = cmdJOList.SelectedValue.Trim.Split("~")

		Dim c As New SqlClient.SqlConnection
		Dim cm As New SqlClient.SqlCommand
		Dim rs As SqlClient.SqlDataReader

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
		vSQL = "select CostPerQty, TotalCost, RunningSFGCost, DateCreated " _
			& "from prod_qtycost where " _
			& "JONO='" & vKeys(0) & "' and OperOrder=" & cmdProcessList.SelectedValue

		'& "JONO='" & vJO & "' and " _
		'& "Sect_Cd='" & vSect & "' and " _
		'& "Proc_Cd='" & vProc & "' and " _
		'& "OperOrder=" & vOperOrder & " " _
		'& "order by TranType,DateCreated"

		cm.CommandText = vSQL

		Try
			rs = cm.ExecuteReader
			Do While rs.Read

				vMCostSummary += "<tr>"

				vMCostSummary += "<td style='text-align:right;'>" & rs("CostPerQty") & "</td>" _
					& "<td style='text-align:right;'>" & rs("TotalCost") & "</td>" _
					& "<td style='text-align:right;'>" & rs("RunningSFGCost") & "</td>" _
					& "<td style='text-align:right;'>" & rs("DateCreated") & "</td>"

				vMCostSummary += "</tr>"

			Loop
			rs.Close()



		Catch ex As SqlClient.SqlException
			Response.Write("Error in SQL query Get Process Details function:  " & ex.Message)
		End Try

		c.Close()
		c.Dispose()
		cm.Dispose()

	End Sub

	Private Sub btnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click
		vSQL = "update prod_completion set " _
			& "PrevBatchNoA='" & txtPrvBatch.Value.Trim & "', " _
			& "CoreWeight='" & txtCore.Value.Trim & "', " _
			& "Meter='" & txtMeter.Value.Trim & "', " _
			& "Qty='" & txtQty.Value.Trim & "', " _
			& "EditedBy='" & Session("uid") & "', " _
			& "DateEdited='" & Now & "' " _
			& "where TranId=" & h_TranId.Value
		'Response.Write(vSQL)

		CreateRecord(vSQL)
		vScript = "alert('Saved Successfully');"


	End Sub

	Protected Sub btnYes_Click(sender As Object, e As EventArgs) Handles btnYes.Click
		vSQL = "update prod_completion set " _
		   & "VoidBy='" & Session("uid") & "', " _
		   & "DateVoid='" & Now & "' " _
		   & "where TranId=" & h_TranId.Value
		'Response.Write(vSQL)

		CreateRecord(vSQL)
		vScript = "alert('Saved Successfully');"
	End Sub

	Protected Sub cmdJOList_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmdJOList.SelectedIndexChanged

		If cmdJOList.SelectedValue = "" Then
			Exit Sub
		End If

		Dim vKeys() As String = cmdJOList.SelectedValue.Trim.Split("~")

		vSQL = "Select JobOrderNo+'~'+BOM_Cd+'~'+STR(BOMRev, 4, 0) as Keys,JobOrderNo from jo_header where JO_Status='RELEASE' "

		vSQL = "select OperOrder, " _
			& "(select Descr from ref_item_process b where a.Proc_Cd=b.Proc_Cd) As Descr " _
			& "from bom_process a where BOM_Cd='" & vKeys(1) & "' and Revision=" & vKeys(2) & " order by OperOrder"
		BuildCombo(vSQL, cmdProcessList)
		cmdProcessList.Items.Add("All")
		cmdProcessList.SelectedValue = "All"


	End Sub
	Protected Sub cmdProcessList_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmdProcessList.SelectedIndexChanged
		CollectCompletion("COMPLETION")
		MaterialsCostSummary("COMPLETION")
	End Sub
End Class
