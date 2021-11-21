Imports denaro
Partial Class production_monitoring
    Inherits System.Web.UI.Page
    Public vScript As String = ""
    Public vEntityId As String = ""
    Public vColHeader As String = ""
    Public vEmpRecords As String = ""
    Public vHeader As String
    Public vData As String = ""
    Public vFilter As String = ""
	Public vTotalRecords As Decimal = 0

	Dim c As New SqlClient.SqlConnection
    Dim vColNames As String = ""
    Dim vColSource As String = ""
    Dim vTableSource As String = ""
    Dim vSQL As String = ""

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        If Session("uid") = "" Then
            Response.Redirect("http://" & SERVERIP & "/" & SITENAME & "/")
        End If

		'vSQL = ""
		'lblSection.Text = GetRef(vSQL, "")

		'If Not CanRun(Session("caption"), Request.Item("id")) Then
		'Session("denied") = "1"
		'Server.Transfer("../main.aspx") 
		'Exit Sub
		'End If


		'& "<td style='width:150px;' class='titleBar'>Section</td>" _
		vHeader += "<tr style='text-align:center;'>" _
			& "<td style='width:40px;' colspan='7'></td>" _
			& "<td style='width:100px;' colspan='2'>Job Order</td>" _
			& "<td style='width:120px;' colspan='2'>Production</td>" _
			& "<td style='width:120px;' colspan='3'></td>"

		vHeader += "<tr style=''><td style='width:40px;'></td><td style='width:40px;'>#</td>" _
		   & "<td style='width:50px;'>Order No.</td>" _
		   & "<td style='width:90px;'>JO No.</td>" _
		   & "<td style='width:150px;'>Process</td>" _
		   & "<td>SFG Name</td>" _
		   & "<td style='width:150px;'>Machine</td>" _
		   & "<td style='width:100px;'>Kilos (kgs)</td>" _
		   & "<td style='width:100px;'>Meter</td>" _
		   & "<td style='width:100px;'>Kilos (kgs)</td>" _
		   & "<td style='width:100px;'>Meter</td>" _
		   & "<td style='width:100px;'>Date<br />Release</td>" _
		   & "<td style='width:120px;'>Production<br />Status</td>"


		If Not IsPostBack Then

			Dim vMonth As String = Format(Now(), "MM")
			Dim vYear As String = Format(Now(), "yyyy") - 3

			For i As Integer = 1 To 4
				CmdYear.Items.Add(vYear + i)
			Next i
			CmdYear.SelectedValue = Format(Now, "yyyy")

			vSQL = "select Id,Descr from ref_month order by Id "
			BuildCombo(vSQL, CmdMonths)

            If vMonth < 10 Then
                CmdMonths.SelectedValue = vMonth.Replace("0", "")
            Else
                CmdMonths.SelectedValue = vMonth
            End If

			vSQL = "select JobOrderNo,JobOrderNo " _
				& "from jo_header where Month(ReleaseDate)='" & CmdMonths.SelectedValue & "' and Year(ReleaseDate)='" & CmdYear.SelectedValue & "' and " _
				& "JO_Status='RELEASE' and ProdStatus is null order by jo_header.JobOrderNo "
			BuildCombo(vSQL, cmdJOList)
			cmdJOList.Items.Add("All")
			cmdJOList.SelectedValue = "All"

			vSQL = "select Status_Cd, Descr from ref_item_status where GroupName='JO_PROD' order by Descr"
			BuildCombo(vSQL, cmdStatus)
			cmdStatus.Items.Add("All")
			cmdStatus.SelectedValue = "All"


			'Get_Released_JobOrders()
			Get_UserDetails()

		End If

	End Sub

    Private Sub Get_Released_JobOrders()
        vData = ""
        Dim c As New SqlClient.SqlConnection(connStr)
        Dim cm As New SqlClient.SqlCommand
        Dim rs As SqlClient.SqlDataReader

        Dim cmref As New SqlClient.SqlCommand
        Dim rsref As SqlClient.SqlDataReader
		Dim iCtr As Integer = 1
		Dim vProdStatus As String = "Pending"
        Dim vEmpSection As String = ""
        Dim vEmpProcess As String = ""

        Dim vItemUOM As String = "KGS"
		Dim vQTY As Decimal = 1
		Dim vBOMeters As Decimal = 0
        Dim vBOMKilos As Decimal = 0
        Dim vProdMeters As Decimal = 0
        Dim vProdKilos As Decimal = 0

		Dim vQTYComp As Decimal = 0
        Dim vlblQty As String = ""
        Dim vJOQTY As Decimal = 0
        Dim vFGQTY As Decimal = 0
        Dim vMachine As String = ""
        Dim vDateRel As String = ""

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
        cmref.Connection = c

        vEmpSection = GetRef("select SectionCd from emp_master where Emp_Cd='" & Session("uid") & "' and Pos_Cd ='6'", "")
        vEmpProcess = GetRef("select ProcessCd from emp_master where Emp_Cd='" & Session("uid") & "' and Pos_Cd ='6'", "")

        Session("Section") = vEmpSection

        If vEmpSection <> "" Then
            divAction.Style("display") = "normal"
        End If

        If cmdJOList.SelectedValue <> "All" Then
            vFilter = "and JobOrderNo='" & cmdJOList.SelectedValue & "' "
        End If

		If TxtSearch.Value.Trim <> "" Then
			vFilter = "and JobOrderNo like '%" & TxtSearch.Value.Trim & "%' "
			Try
				cmdJOList.SelectedValue = TxtSearch.Value.Trim
			Catch ex As Exception
				cmdJOList.SelectedValue = "All"
			End Try

		End If



		vSQL = "Select TranId, JobOrderNo, BOM_Cd, " _
			& "BOMRev, ProdStatus, ReleaseDate, OrderQty, " _
			& "(Select Descr from ref_item_uom where ref_item_uom.Uom_Cd=jo_header.Uom_Cd) As vItemUOM " _
			& "from jo_header where Month(ReleaseDate)='" & CmdMonths.SelectedValue & "' and Year(ReleaseDate)='" & CmdYear.SelectedValue & "' " _
				& " " & vFilter _
			& "Order by JobOrderNo"

		'Response.Write(vSQL)
		'Exit Sub 

		vFilter = ""
		cm.CommandText = vSQL
		Try
			rs = cm.ExecuteReader
			Do While rs.Read

				If vEmpSection <> "" Then
					vFilter = "Sect_Cd='" & vEmpSection & "' and Proc_Cd='" & vEmpProcess & "' and "
				End If

                vSQL = "Select TranId, OperOrder, SFG_Cd, SFG_Descr, Sect_Cd,Proc_Cd, Kilos, Meters," _
                    & "(Select Descr +' '+Descr1 from item_master where SFG_Cd=Item_Cd ) as SFGName, " _
                    & "(select OrderQty from jo_header where JobOrderNo='" & rs("JobOrderNo") & "') as vJOQty," _
                    & "(select NetWeight from bom_header where bom_header.BOM_Cd=bom_process.BOM_Cd and bom_header.Revision=bom_process.Revision) vFGQTY," _
                    & "(select Descr from ref_emp_section where Section_Cd=Sect_Cd) as vSection,"

                vSQL += "(select Descr from ref_item_process where ref_item_process.Proc_Cd=bom_process.Proc_Cd and " _
						& "ref_item_process.Sect_Cd=bom_process.Sect_Cd) as vProcess,"

				vSQL += "(select Remarks " _
						& "from prod_monitoring where SFG_Cd=SFG_Item_Cd and JobOrderNo='" & rs("JobOrderNo") & "') as vProdStatus, "

				vSQL += "(select Mach_Cd " _
						& "from jo_machine where JobOrderNo='" & rs("JobOrderNo") & "' and " _
						& "jo_machine.OperOrder=bom_process.OperOrder and IsPrimary='YES') as vMachCode, "

				vSQL += "(select (select Descr from ref_item_machine where ref_item_machine.Mach_Cd=jo_machine.Mach_Cd) " _
						& "from jo_machine where JobOrderNo='" & rs("JobOrderNo") & "' and " _
						& "jo_machine.OperOrder=bom_process.OperOrder and IsPrimary='YES') as vMach, "

				vSQL += "(select Kilos from jo_machine where JobOrderNo='" & rs("JobOrderNo") & "' and " _
						& "jo_machine.OperOrder=bom_process.OperOrder and IsPrimary='YES') as vJOKilos, "

				vSQL += "(select Meter from jo_machine where JobOrderNo='" & rs("JobOrderNo") & "' and " _
						& "jo_machine.OperOrder=bom_process.OperOrder and IsPrimary='YES') as vJOMeter, "

				vSQL += "(select sum(NetWeight) from prod_completion where bom_process.SFG_Cd=prod_completion.SFG_Cd and " _
						& "JONO='" & rs("JobOrderNo") & "' and TranType='COMPLETION' ) as vProdKilos, "

				vSQL += "(select sum(Meter) from prod_completion where bom_process.SFG_Cd=prod_completion.SFG_Cd and " _
						& "JONO='" & rs("JobOrderNo") & "' and TranType='COMPLETION') as vProdMeters "

				vSQL += "from bom_process where " & vFilter & "BOM_Cd=" & rs("BOM_Cd") & " and " _
					& "Revision=" & rs("BOMrev") & " Order by OperOrder"

				'Response.Write(vSQL & "<BR><BR>")

				cmref.CommandText = vSQL

				rsref = cmref.ExecuteReader
				Do While rsref.Read

					vMachine = IIf(IsDBNull(rsref("vMachCode")), "", rsref("vMachCode"))

					'Response.Write(cmdMac.SelectedValue & " - " & vMachine)

					If cmdMac.SelectedValue = vMachine Or cmdMac.SelectedValue = "All" Then

						vProdStatus = IIf(IsDBNull(rsref("vProdStatus")), "", rsref("vProdStatus"))
						vFGQTY = IIf(IsDBNull(rsref("vFGQTY")), 0, rsref("vFGQTY"))
						vJOQTY = IIf(IsDBNull(rsref("vJOQty")), 0, rsref("vJOQty"))

						If cmdStatus.SelectedItem.Value = "All" Or _
							cmdStatus.SelectedItem.Text = "COMPLETED" And vProdStatus = "COMPLETED" Or _
							cmdStatus.SelectedItem.Text = "START-UP RUN" And vProdStatus = "START-UP RUN" Or _
							cmdStatus.SelectedItem.Text = "INITIAL RUN" And vProdStatus = "INITIAL RUN" Or _
							cmdStatus.SelectedItem.Text = "PRODUCTION RUN" And vProdStatus = "PRODUCTION RUN" Then

							vData += "<tr><td>" _
								& "<input type='button' " _
								& "id='" & rs("TranId") _
									& "~" & rsref("OperOrder") _
									& "~" & rs("BOM_Cd") _
									& "~" & rs("BOMRev") _
									& "~" & rsref("Sect_Cd") _
									& "~" & rsref("Proc_Cd") _
									& "~" & rsref("SFG_Cd") & "' " _
								& "value='Select' class='btn btn-primary btn-sm'></td>" _
								& "<td class='labelC'>" & iCtr & "</td>" _
								& "<td class='labelC'>" & rsref("OperOrder") & "</td>" _
								& "<td class='labelC'>" & rs("JobOrderNo") & "</td>"

                            vData += "<td>" & rsref("vProcess") & "</td>" _
                                & "<td><b>" & rsref("SFG_Cd") & "</b><br>" & rsref("SFG_Descr") & "</td>"

                            vQTY = IIf(IsDBNull(rs("OrderQty")), 0, rs("OrderQty"))
							'vBOMeters = IIf(IsDBNull(rsref("Meters")), 0, rsref("Meters")) * vQTY
							'vBOMKilos = IIf(IsDBNull(rsref("Kilos")), 0, rsref("Kilos")) * vQTY

							vBOMKilos = IIf(IsDBNull(rsref("vJOKilos")), 0, rsref("vJOKilos"))
							vBOMeters = IIf(IsDBNull(rsref("vJOMeter")), 0, rsref("vJOMeter"))

							vProdMeters = IIf(IsDBNull(rsref("vProdMeters")), 0, rsref("vProdMeters"))
							vProdKilos = IIf(IsDBNull(rsref("vProdKilos")), 0, rsref("vProdKilos"))

							If Not IsDBNull(rs("ReleaseDate")) Then
								vDateRel = Format(CDate(rs("ReleaseDate")), "MM/dd/yyyy")
							End If


							vData += "<td Class='labelR'>" & rsref("vMach") & "</td>"
							'& "<td class='text-right'>" & vQTY & " " & vItemUOM & "</td>" _

							vData += "<td class='text-right'>" & Format(vBOMKilos, "###,###,##0.00") & "</td>" _
								& "<td class='text-right'>" & Format(vBOMeters, "###,###,##0.00") & "</td>" _
								& "<td class='text-right'>" & Format(vProdKilos, "###,###,##0.00") & "</td>" _
								& "<td class='text-right'>" & Format(vProdMeters, "###,###,##0.00") & "</td>" _
								& "<td class='text-center'>" & vDateRel & "</td>" _
								& "<td>" & vProdStatus & "</td>"

							iCtr += 1
							vProdStatus = ""
							vlblQty = ""
							vQTY = 0
							vQTYComp = 0
							vJOQTY = 0
							vFGQTY = 0
						End If
					End If
				Loop

				rsref.Close()
				vData += "</tr>"
			Loop
			vTotalRecords = iCtr - 1

			rs.Close()
		Catch ex As Exception
			Response.Write("Error in SQL Queries. . ")
		End Try


		c.Close()
        cm.Dispose()
        c.Dispose()
    End Sub

    Private Sub Get_UserDetails()
        Dim c As New SqlClient.SqlConnection(connStr)
        Dim cm As New SqlClient.SqlCommand
        Dim rs As SqlClient.SqlDataReader
        Dim vSection As String = ""

        Dim iCtr As Integer = 0
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
        vSQL = "select Emp_Cd, Emp_Fname, Emp_Lname, SectionCd, " _
                & "(select Descr from ref_emp_section where SectionCd=Section_Cd) as SecDescr " _
                & "from emp_master where Emp_Cd='" & Session("uid") & "'"

		cm.CommandText = vSQL
        rs = cm.ExecuteReader
        Do While rs.Read
            lblSection.Text = "Section : " & IIf(IsDBNull(rs("SecDescr")), "Unknown", rs("SecDescr"))
            lblName.Text = "Hi " & rs("Emp_Fname") & " " & rs("Emp_Lname")
            'Session("EmpSection") = rs("SectionCd")
            vSection = rs("SectionCd")
        Loop
        rs.Close()

		If vSection <> "" Then
			divAction.Style("display") = "normal"
		End If


		If vSection <> "" Then
            vFilter = "where Sect_Cd ='" & vSection & "'"
        End If

        vSQL = "select Mach_Cd, Descr from ref_item_machine " & vFilter & "  order by Descr"
        BuildCombo(vSQL, cmdMac)
        cmdMac.Items.Add("All")
		cmdMac.SelectedValue = "All"

		c.Close()
        cm.Dispose()
        c.Dispose()
    End Sub
    Private Sub GetTable_Properties()
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

        cm.CommandText = "select * from table_properties_hdr where ModuleCode='" & Request.Item("id") & "' and Published='YES' "
        rs = cm.ExecuteReader
        If rs.Read Then
            If Not IsDBNull(rs("DboTable")) Then
                vTableSource = rs("DboTable")
            End If
        End If
        rs.Close()
         
        cm.CommandText = "select * from table_properties_dtl where ModuleCode='" & Request.Item("id") & "' and Published='YES' and ColType='GRIDVIEW' order by ColCode"
        'Response.Write(cm.CommandText)
            Try
                rs = cm.ExecuteReader
                Do While rs.Read
                    vColHeader += "<th>" & rs("ColTitle") & "</th>"

                    vColNames += rs("ColReturnValue").ToString.Trim & ","
                    If Not IsDBNull(rs("ColSource")) Then
                        vColSource += rs("ColSource")
                    End If
                Loop

                vColNames = Mid(vColNames, 1, Len(vColNames) - 1)
                vColSource = Mid(vColSource, 1, Len(vColSource) - 1)

                rs.Close()
            Catch ex As SqlClient.SqlException
                vScript = "alert('Error occurred while trying to buil shift reference. Error is: " & _
                    ex.Message.Replace(vbCrLf, "").Replace("'", "") & "');"
            End Try  

        c.Close()
        cm.Dispose()
        c.Dispose()

    End Sub

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Session.RemoveAll()
        'Server.Transfer("../../Portallogin.aspx")
        Response.Redirect("http://" & SERVERIP & "/" & SITENAME & "/portallogin.aspx")
    End Sub
    Protected Sub cmdJOList_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmdJOList.SelectedIndexChanged
        Get_Released_JobOrders()
    End Sub

	'Protected Sub cmdMachine_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmdMachine.SelectedIndexChanged
	'    Get_Released_JobOrders()
	'End Sub
	'Protected Sub cmdStatus_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmdStatus.SelectedIndexChanged
	'    Get_Released_JobOrders()
	'End Sub
	Protected Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
		Get_Released_JobOrders()
		btnRefresh.Enabled = True
	End Sub

	Private Sub CmdMonths_TextChanged(sender As Object, e As EventArgs) Handles CmdMonths.TextChanged
		vSQL = "select JobOrderNo,JobOrderNo " _
				& "from jo_header where Month(ReleaseDate)='" & CmdMonths.SelectedValue & "' and Year(ReleaseDate)='" & CmdYear.SelectedValue & "' and " _
				& "JO_Status='RELEASE' and ProdStatus is null order by jo_header.JobOrderNo "
		BuildCombo(vSQL, cmdJOList)
		cmdJOList.Items.Add("All")
		cmdJOList.SelectedValue = "All"
	End Sub

	Private Sub CmdYear_TextChanged(sender As Object, e As EventArgs) Handles CmdYear.TextChanged
		vSQL = "select JobOrderNo,JobOrderNo " _
				& "from jo_header where Month(ReleaseDate)='" & CmdMonths.SelectedValue & "' and Year(ReleaseDate)='" & CmdYear.SelectedValue & "' and " _
				& "JO_Status='RELEASE' and ProdStatus is null order by jo_header.JobOrderNo "
		BuildCombo(vSQL, cmdJOList)
		cmdJOList.Items.Add("All")
		cmdJOList.SelectedValue = "All"
	End Sub
End Class
