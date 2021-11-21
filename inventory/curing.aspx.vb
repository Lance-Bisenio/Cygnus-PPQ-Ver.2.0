Imports denaro
Imports System

Partial Class inventory_curing
	Inherits System.Web.UI.Page
	Public vScript As String = ""
	Public vEntityId As String = ""
	Public vColHeader As String = ""
	Public vEmpRecords As String = ""
	Public vFilter As String = ""

	Dim c As New SqlClient.SqlConnection
	Dim vColNames As String = ""
	Dim vColSource As String = ""
	Dim vTableSource As String = ""
	Dim vSQL As String = ""
	Public vData As String = ""


	Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load


		If Session("uid") = "" Then
			Response.Redirect("http://" & SERVERIP & "/" & SITENAME & "/")
		End If

        'If Not CanRun(Session("caption"), Request.Item("id")) Then
        '	Session("denied") = "1"
        '	Server.Transfer("../main.aspx")
        '	Exit Sub
        'End If

        vEntityId = Request.Item("id").ToString.Trim

		If txtDateFrom.Text.Trim = "" Then
			txtDateFrom.Text = Format(Now, "MM/dd/yyyy")
		End If

		If txtDateTo.Text.Trim = "" Then
            txtDateTo.Text = Format(Now.AddDays(1), "MM/dd/yyyy")
        End If

        If Session("vDeleteRow") = "YES" Then
            'Response.Write("<script>document.getElementById('tblCuringReport').deleteRow(2);</script>")
            'Session("vDeleteRow") = "NO"
        End If

        Response.Write("Delete tag:" & Session("vDeleteRow"))


        If Not IsPostBack Then

            DDLCompStatus.Items.Add("Show All")
            DDLCompStatus.Items.Add("Show All Pending Transaction")
            DDLCompStatus.Items.Add("Show All Closed Transaction")
            DDLCompStatus.SelectedValue = "Show All Pending Transaction"

            'cmbShow.Items.Clear()
            'For iCtr = 1 To 10
            '	cmbShow.Items.Add(10 * iCtr)
            'Next iCtr

            'cmbShow.SelectedValue = 10

            'BuildCombo("select uom_Cd, Descr from ref_item_uom order by Descr", cmbUOMQ)
            'cmbUOMQ.Items.Add("All")
            'cmbUOMQ.SelectedValue = "All"

            'BuildCombo("select Type_Cd, Descr from ref_item_type order by Descr", cmbItemType)
            'cmbItemType.Items.Add("All")
            'cmbItemType.SelectedValue = "All"

            'BuildCombo("select Class_Cd, Descr from ref_item_class order by Descr", cmbTypeClass)
            'cmbTypeClass.Items.Add("All")
            'cmbTypeClass.SelectedValue = "All"

            'BuildCombo("select ColSource, ColTitle from table_properties_dtl where ModuleCode='203' and ColType='SEARCHBY'", cmbSearchBy)

            'cmbSource.Items.Add("All")
            'cmbSource.SelectedValue = "All"

            'If CanRun(Session("caption"), 204) = True Then
            '	cmbItemStatus.Items.Add("Deleted")
            'End If

            'GetTable_Properties()
            'DataRefresh("Search")
        End If

        'Response.Write(System.Security.Principal.WindowsIdentity.GetCurrent().Name & " test")

    End Sub

	Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click

		Dim c As New SqlClient.SqlConnection(connStr)
		Dim cm As New SqlClient.SqlCommand
		Dim rs As SqlClient.SqlDataReader
        Dim vIsJONull As String = ""
        Dim vIscomplete As Integer = 0

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

        vSQL = "select JONO, BOM, Revision, OperOrder, Sect_Cd, Proc_Cd, SFG_Cd, FG_Cd, BatchNo, " _
            & "CoreWeight, NetWeight, GrossWeight," _
            & "(select SFG_Descr from bom_process a where a.SFG_Cd=b.SFG_Cd) as SFGDescr, " _
            & "(select top 1 IsComplete from prod_curing c where c.BatchNo=b.BatchNo) as vIsComplete " _
            & "from prod_completion b where b.BatchNo='" & txtSFGCode.Text.Trim & "'"

        cm.CommandText = vSQL
		rs = cm.ExecuteReader
		If rs.Read Then

            vIsJONull = rs("JONO")

            If IsDBNull(rs("vIsComplete")) Then
                vIscomplete = 0
            Else
                vIscomplete = rs("vIsComplete")
            End If

            vSQL = "insert into prod_curing values ('" & txtSFGCode.Text.Trim & "','" & rs("JONO") & "','" & rs("BOM") & "','" & rs("Revision") & "'," _
                & "'" & rs("Sect_Cd") & "','" & rs("Proc_Cd") & "','" & rs("OperOrder") & "','" & rs("FG_Cd") & "'," _
                & "'" & rs("SFG_Cd") & "','" & TxtRemarks.Text.ToString.Replace("'", "*").Trim & "'," _
                & "'" & CDate(Now()) & "','" & Session("uid") & "','" & CDate(Now()) & "',0,NULL,NULL) "
        End If
		rs.Close()



        If vIsJONull <> "" Then
            Try
                If vIscomplete = 1 Then
                    Response.Write("<script>alert('This item is already completed.');</script>")
                Else
                    CreateRecord(vSQL)
                    Response.Write("<script>alert('Successfully Saved.');</script>")
                End If

            Catch ex As Exception
                Response.Write("Error in SQL query insert/update:  " & ex.Message)
			End Try
		Else
			Response.Write("<script>alert('SFG Barcode not found. Please check and try again.');</script>")
		End If

		txtSFGCode.Text = ""
		TxtRemarks.Text = ""

		c.Close()
		cm.Dispose()
		c.Dispose()

		Reload()

	End Sub

	Private Sub Reload()

		Dim c As New SqlClient.SqlConnection(connStr)
		Dim cm As New SqlClient.SqlCommand
		Dim rs As SqlClient.SqlDataReader

		Dim cmsub As New SqlClient.SqlCommand
		Dim rssub As SqlClient.SqlDataReader
		Dim vLastDate As Date = Now
		Dim vHrsDiff As String = ""
		Dim vCtr As Integer = 0
		Dim vOutCreatedBy As String = ""
		Dim vRemarksIn As String = ""
		Dim vRemarksOut As String = ""
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
		cmsub.Connection = c


		If txtJONO.Text.Trim <> "" Then
			vFilter = " and JONO like '%" & txtJONO.Text.Trim & "%' "
		End If

		If txtDateFrom.Text.Trim <> "" And txtDateTo.Text.Trim <> "" Then
			vFilter += " and DateCreated between '" _
				& txtDateFrom.Text.Trim & "' and '" _
				& txtDateTo.Text.Trim & "' "
		End If

        Select Case DDLCompStatus.SelectedValue
            Case "Show All"

            Case "Show All Pending Transaction"
                vFilter += " and IsComplete=0"
            Case "Show All Closed Transaction"
                vFilter += " and IsComplete=1"
        End Select

        vSQL = "Select distinct(BatchNo) As vBatchNo from prod_curing where BatchNo Is Not null " & vFilter

		cm.CommandText = vSQL
		rs = cm.ExecuteReader
		Do While rs.Read

			vData += "<Tr>"
            vSQL = "Select TranId, TranDate,CreatedBy,Remarks " _
                & "from prod_curing b where BatchNo='" & rs("vBatchNo") & "' order by TranDate desc"

            cmsub.CommandText = vSQL
			rssub = cmsub.ExecuteReader
			If rssub.Read Then
				vLastDate = rssub("TranDate")
				vOutCreatedBy = rssub("CreatedBy")
				vRemarksOut = rssub("Remarks")
			End If
			rssub.Close()

            vSQL = "Select JONO, BOM_Cd, BOMRev, Sect_Cd, Proc_Cd, OperOrder, SFG_Item_Cd, CreatedBy, DateCreated, TranDate, " _
                & "(Select SFG_Descr from bom_process a where a.SFG_Cd=b.SFG_Item_Cd) As SFGDescr, Remarks, IsComplete, " _
                & "(select Top 1 NetWeight from prod_completion c where c.BatchNo=b.BatchNo) as NetWeight " _
                & "from prod_curing b " _
                & "where BatchNo='" & rs("vBatchNo") & "' order by TranDate asc"

            cmsub.CommandText = vSQL
			rssub = cmsub.ExecuteReader
			If rssub.Read Then

				vHrsDiff = DateDiff(DateInterval.Hour, rssub("TranDate"), vLastDate)

                If vHrsDiff > 0 Then
					vCtr += 1

					vRemarksIn = rssub("Remarks")

                    vData += "<td>" & vCtr & "</td>"
                    vData += "<td>" & rssub("JONO") & "</td>"
                    vData += "<td>" & rs("vBatchNo") & "</td>"
					vData += "<td>" & rssub("DateCreated") & "</td>"
					vData += "<td>" & rssub("SFG_Item_Cd") & "</td>"
                    vData += "<td>" & rssub("SFGDescr") & "</td>"
                    vData += "<td>" & rssub("NetWeight") & "</b></td>"
                    vData += "<td style='background:#DDFFD1;'>" & rssub("TranDate") & "</td>"
                    vData += "<td style='background:#DDFFD1;'>" & rssub("CreatedBy").ToString.ToUpper & "</td>"
                    vData += "<td style='background:#D3D1FF;'>" & vLastDate & "</td>"
                    vData += "<td style='background:#D3D1FF;'>" & vOutCreatedBy.ToString.ToUpper & "</td>"
                    vData += "<td style='background-color:#6495ED; color:#FFF'>" & vHrsDiff & " Hour(s)</b></td>"
                    vData += "<td>IN: " & vRemarksIn & " <br />Out: " & vRemarksOut & "</td>"
                    vData += "<td>"

                    If rssub("IsComplete") = 1 Then
                        vData += "<input type='button' runat='server' id='Button1' value='Tas as Complete' " _
                            & "class='btn btn-primary btn-sm' data-toggle='modal' data-target='#ModalComplete' " _
                            & "onclick='SndParam(""" & rs("vBatchNo") & """);' disabled /></td>"
                    Else
                        vData += "<input type='button' runat='server' id='Button1' value='Tas as Complete' " _
                            & "class='btn btn-primary btn-sm' data-toggle='modal' data-target='#ModalComplete' " _
                            & "onclick='SndParam(""" & rs("vBatchNo") & """);'/></td>"
                    End If


                Else
					vCtr += 1
                    vData += "<td>" & vCtr & "</td>"
                    vData += "<td>" & rssub("JONO") & "</td>"
                    vData += "<td>" & rs("vBatchNo") & "</td>"
					vData += "<td>" & rssub("DateCreated") & "</td>"
					vData += "<td>" & rssub("SFG_Item_Cd") & "</td>"
                    vData += "<td>" & rssub("SFGDescr") & "</td>"
                    vData += "<td>" & rssub("NetWeight") & "</b></td>"
                    vData += "<td style='background:#DDFFD1;'>" & rssub("TranDate") & "</td>"
                    vData += "<td style='background:#DDFFD1;'>" & rssub("CreatedBy").ToString.ToUpper & "</td>"
                    vData += "<td style='background:#D3D1FF;'>" & vLastDate & "</td>"
                    vData += "<td style='background:#D3D1FF;'></td>"
                    vData += "<td style='background-color:#E3E2E2; color:#000'>" & vHrsDiff & " Hour(s)</b></td>"
                    vData += "<td>" & vRemarksIn & "</td>"
                    vData += "<td></td>"
                End If

				vData += "</div>" _
					& "</div>"
			End If
			rssub.Close()

			vData += "</Tr>"

			vRemarksIn = ""
			vRemarksOut = ""
		Loop
		rs.Close()
        Session("vData") = vData

        c.Close()
		cm.Dispose()
		c.Dispose()

	End Sub

	Protected Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
		Reload()
	End Sub

    Private Sub BtnComplete_Click(sender As Object, e As EventArgs) Handles BtnComplete.Click

        vSQL = "update prod_curing set Iscomplete=1, " _
            & "CompleteBy='" & Session("uid") & "', " _
            & "DateComplete='" & Now & "' " _
            & "where BatchNo='" & TxtItemKey.Value & "'"

        CreateRecord(vSQL)

        vData = Session("vData")

        Session("vDeleteRow") = "YES"

        Response.Write("<script>alert('Successfully Saved.');</script>")
        Response.Write("<script>TblDeleteRow();</script>")
    End Sub

End Class