Imports denaro
Partial Class materials_return
    Inherits System.Web.UI.Page
    Public vScript As String = ""
    Public vHeader As String = ""
    Public vSubHeader As String = ""
    Public vData As String = ""
    Public vTitle As String = ""

    Dim vJO As String = ""
    Dim vBOM As String = ""
    Dim vBOMRev As String = ""
    Dim vSQL As String = ""
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Session("uid") = Nothing Or Session("uid") = "" Then
            vScript = "alert('Login session has expired. Please re-login again.'); window.close();"
            Exit Sub
        ElseIf Session("vProc_Matlist") = "" Then
            vScript = "alert('Login session has expired. Please re-login again.'); window.close();"
            Exit Sub
        End If

        If Not IsPostBack Then
            Session.Remove("ViewAll")
            Builder()
        End If

        vTitle = "Production raw-material return"
    End Sub
    Private Sub Builder()
        LblJONO.Text = Request.Item("pJO")
        vJO = Request.Item("pJO")
        vBOM = Request.Item("pBom")
        vBOMRev = Request.Item("pBomRev")

        LblProcess.Text = GetRef("select Descr from ref_item_process where Proc_Cd='" & Request.Item("pProcess") & "'", "")


        ProductionMaterial_Receiving_Header()
        ProductionMaterial_Return()
        cmdSave.Visible = True
        'cmdViewAll.Visible = True
        'cmdViewPending.Visible = True
        cmdCancel.Visible = True

        ProdReturnItemList()

    End Sub
    Private Sub ProductionMaterial_Receiving_Header()

        vHeader = "<tr class='bg-info text-white'>" _
                & "<td>Material Code</td>" _
                & "<td>Description</td>" _
                & "<td>LOTNO</td>" _
                & "<td>Roll No</td>" _
                & "<td>Qty<br>Received</td>" _
                & "<td>UOM</td>"

        vHeader += "<td>Qty<br />Return</td></tr>"

	End Sub

    Private Sub ProductionMaterial_Return()

        If Session("vProc_Matlist") = "" Then
            vScript = "alert('Login session has expired. Please re-login again.'); window.close();"
            Exit Sub
        End If

        Dim c As New SqlClient.SqlConnection(connStr)
        Dim cm As New SqlClient.SqlCommand
        Dim rs As SqlClient.SqlDataReader
        Dim cm_sub As New SqlClient.SqlCommand
		'Dim rs_sub As SqlClient.SqlDataReader
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

        Dim vQty As String = ""
        Dim vRetQty As String = ""

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


        If h_Action.Value = "Save" Then
            'vSQL = "delete from prod_return_materials where " _
            '            & "JONO='" & vJONO & "' and " _
            '            & "OperOrder=" & vOperNo

            'cm_sub2.CommandText = vSQL

            'Try
            '    cm_sub2.ExecuteNonQuery()
            'Catch ex As SqlClient.SqlException
            '    Response.Write("Error In SQL query Receiving raw-materials :  " & ex.Message)
            'End Try
        End If


		If Session("ViewAll") = "YES" Then
			vFilter = ""
		Else
			vFilter = " and DateReceived is not null"
			Session.Remove("ViewAll")
		End If

        vSQL = "select Item_Cd, LotNo, RollNo, UOM, RQty, " _
                    & "(select (Descr + ' ' + Descr) as Descr from item_master " _
                        & "where item_master.Item_Cd=a.Item_Cd) as vDescr, " _
                        & "(select top 1 SFG_Descr from bom_process e where e.SFG_Cd=Item_Cd and e.BOM_Cd='" & vBOM & "') As SFGName "

        vSQL += "from prod_rawmaterial a where " _
                & "BOM='" & vBOM & "' and " _
                & "Revision='" & vBOMRev & "' and " _
                & "JONO='" & vJONO & "' and " _
                & "Sect_Cd='" & vSection & "' and " _
                & "Proc_Cd='" & vProcess & "' and " _
                & "OperOrder=" & Request.Item("pOperNo") & "  " _
                & vFilter _
                & " Order by vDescr"

        '& "and Item_Cd in (" & Session("vProc_Matlist") & ")" 

        cm.CommandText = vSQL

        Try
            rs = cm.ExecuteReader
            Do While rs.Read
                iCtr += 1
                vID = iCtr & "_" & rs("Item_Cd")


                'vSQL = "select sum(Qty) from prod_rawmaterial where " _
                '    & "JONO='" & vJONO & "' and " _
                '    & "Sect_Cd='" & vSection & "' and " _
                '    & "Proc_Cd='" & vProcess & "' and " _
                '    & "Item_Cd='" & rs("Item_Cd") & "'"
                'vQty = GetRef(vSQL, 0)
                'vQty = IIf(IsDBNull(rs("RQty")), rs("Qty"), rs("RQty"))

                vData += "<tr style='height:24px;'>" &
                    "<td class='labelC'>" & rs("Item_Cd") & "</td>" &
                    "<td class='labelL'>" & rs("vDescr") & rs("SFGName") & "</td>" &
                    "<td class='labelL'>" & rs("LotNo") & "</td>" &
                    "<td class='labelL'>" & rs("RollNo") & "</td>" &
                    "<td class='labelR'>" & rs("RQty") & "</td>" &
                    "<td class='labelC'>" & rs("UOM") & "</td>" &
                    "<td class='labelR'>"

                'vSQL = "select sum(Qty) from prod_return_materials where " _
                '    & "JONO='" & vJONO & "' and " _
                '    & "Sect_Cd='" & vSection & "' and " _
                '    & "Proc_Cd='" & vProcess & "' and " _
                '    & "Item_Cd='" & rs("Item_Cd") & "' and " _
                '    & "LotNo='" & rs("LotNo") & "'"
                'vRetQty = GetRef(vSQL, 0)

                'If Not IsDBNull(rs("vRetQty")) Then
                'If vRetQty <> "null" Then
                '    vQty = vRetQty
                'Else
                '    vQty = "0.00"
                'End If

                vQty = "0.00"
                vData += "<input style='width:80px; height:21px; margen:auto; text-align:right; padding-right:5px' type='text' " _
                        & "name='" & vID & "' id='" & vID & "' value='" & vQty & "' />"

                vData += "</td></tr>"

                If h_Action.Value = "Save" Then

                    If Request.Item(vID).Trim = "" Then
                        vValue = 0
                    Else
                        vValue = Request.Item(vID)
                    End If

                    If vValue > 0 Then
                        vSQL = "insert into prod_return_materials " _
                            & "(BOM, Revision, Sect_Cd, Proc_Cd, JONO, Item_Cd, Qty, Weight, UOM, " _
                            & "OperOrder, LotNo, CreatedBy, DateCreated, RequestRef, RollNo) " _
                            & "values ("

                        vSQL += vBOM & "," & vBOMRev & ",'" & vSection & "','" & vProcess & "','" & vJONO & "','" _
                            & rs("Item_Cd") & "','" & vValue & "','" & vValue & "','" & rs("UOM") & "','" _
                            & vOperNo & "','" & rs("LotNo") & "','" & Session("uid") & "','" & Now() & "','" & vJONO & "-" & vOperNo & "','" & rs("RollNo") & "')"

                        'Response.Write(vSQL & "<br>")
                        cm_sub2.CommandText = vSQL

                        Try
                            cm_sub2.ExecuteNonQuery()
                        Catch ex As SqlClient.SqlException
                            Response.Write("Error in SQL query Receiving raw-materials :  " & ex.Message)
                        End Try
                    End If

                End If
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
            vScript = "alert('Successfully saved.'); " 'window.close();" ' window.opener.document.form1.submit();

            h_Action.Value = ""

            vHeader = ""
            vData = ""
            vSubHeader = ""

            Builder()

        End If

    End Sub

    Private Sub ProdReturnItemList()
        vSubHeader = ""

        If Session("vProc_Matlist") = "" Then
            vScript = "alert('Login session has expired. Please re-login again.'); window.close();"
            Exit Sub
        End If

        vSubHeader += "<tr class='bg-info text-white'>" _
                & "<td>Material Code</td>" _
                & "<td>Description</td>" _
                & "<td>LOTNO</td>" _
                & "<td>Roll No</td>"

        vSubHeader += "<td>Qty<br />Return</td>" _
                & "<td>Created By</td>" _
                & "<td>Received By</td>" _
                & "<td style='width:60px;'></td></tr>"


        Dim c As New SqlClient.SqlConnection(connStr)
        Dim cm As New SqlClient.SqlCommand
        Dim rs As SqlClient.SqlDataReader
        Dim cm_sub As New SqlClient.SqlCommand
        'Dim rs_sub As SqlClient.SqlDataReader
        Dim cm_sub2 As New SqlClient.SqlCommand

        Dim iCtr As Integer = 0
        Dim vFilter As String = ""
        Dim vID As String = ""
        Dim vBOM As String = Request.Item("pBOM")
        Dim vBOMRev As String = Request.Item("pBOMRev")
        Dim vJONO As String = Request.Item("pJO")
        Dim vSection As String = Request.Item("pSection")
        Dim vProcess As String = Request.Item("pProcess")
        Dim vOperNo As String = Request.Item("pOperNo")

        Dim vQty As String = ""

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

        If Session("ViewAll") = "YES" Then
            vFilter = ""
        Else
            vFilter = " and DateReceived is not null"
            Session.Remove("ViewAll")
        End If

        vSQL = "select TranId, Item_Cd, LotNo, RollNo, UOM, QTY, CreatedBy, DateCreated, DateReceived, " _
                    & "(Select Emp_Fname +' '+ Emp_Lname from emp_master where a.CreatedBy=Emp_Cd) as vOpsName, " _
                    & "(select FullName from user_list where a.CreatedBy=User_Id) as vAdminName, " _
                    & "(Select Emp_Fname +' '+ Emp_Lname from emp_master where a.ReceivedBy=Emp_Cd) as vRecOpsName, " _
                    & "(select FullName from user_list where a.ReceivedBy=User_Id) as vRecAdminName, " _
                    & "(select (Descr + ' ' + Descr) as Descr from item_master " _
                        & "where item_master.Item_Cd=a.Item_Cd) as vDescr "

        vSQL += "from prod_return_materials a where " _
                & "BOM='" & vBOM & "' and " _
                & "Revision='" & vBOMRev & "' and " _
                & "JONO='" & vJONO & "' and " _
                & "Sect_Cd='" & vSection & "' and " _
                & "Proc_Cd='" & vProcess & "' and " _
                & "OperOrder=" & Request.Item("pOperNo") & " and TranStatus is null " _
                & " Order by vDescr"

        '& "and Item_Cd in (" & Session("vProc_Matlist") & ")" 
        'Response.Write(vSQL)
        cm.CommandText = vSQL

        Try
            rs = cm.ExecuteReader
            Do While rs.Read
                iCtr += 1
                vID = iCtr & "_" & rs("Item_Cd")

                vSubHeader += "<tr style='height:24px;'>" &
                    "<td class='labelC'>" & rs("Item_Cd") & "</td>" &
                    "<td class='labelL'>" & rs("vDescr") & "</td>" &
                    "<td class='labelL'>" & rs("LotNo") & "</td>" &
                    "<td class='labelL'>" & rs("RollNo") & "</td>" &
                    "<td class='labelR'>" & rs("QTY") & "</td>" &
                    "<td class='labelR'>"

                If IsDBNull(rs("vOpsName")) Then
                    vSubHeader += rs("vAdminName")
                Else
                    vSubHeader += rs("vOpsName")
                End If


                vSubHeader += "<br>" & Format(CDate(rs("DateCreated")), "MM/dd/yyyy HH:mm") & "</td>"




                vSubHeader += "<td class='labelR'>"
                If Not IsDBNull(rs("DateReceived")) Then

                    If IsDBNull(rs("vRecOpsName")) Then
                        vSubHeader += rs("vRecAdminName")
                    Else
                        vSubHeader += rs("vRecOpsName")
                    End If


                    vSubHeader += "<br>" & Format(CDate(rs("DateReceived")), "MM/dd/yyyy HH:mm")
                End If
                vSubHeader += "</td>"

                vSubHeader += "<td class='labelR'>"



                If IsDBNull(rs("DateReceived")) Then
                    vSubHeader += "<input type='button' class='btn btn-danger btn-sm' value='Cancel' " _
                        & "name='" & vID & "' id='" & vID & "' value='" & vQty & "' onclick='CancelItem(" & rs("TranId") & ")' />"
                    'Else
                    '    vSubHeader += "Date Received: " & rs("DateReceived")
                End If

                vSubHeader += "</td></tr>"

            Loop
            rs.Close()

        Catch ex As SqlClient.SqlException
            vScript = "alert('Error occurred while trying to buil shift reference. Error is: " &
                ex.Message.Replace(vbCrLf, "").Replace("'", "") & "');"
        End Try

        c.Close()
        cm.Dispose()
        c.Dispose()

        If h_Action.Value = "Save" Then
            vScript = "alert('Successfully saved.'); window.close();" ' window.opener.document.form1.submit();
            h_Action.Value = ""
        End If

    End Sub

    Protected Sub cmdSave_Click(sender As Object, e As EventArgs) Handles cmdSave.Click
        h_Action.Value = "Save"
        Builder()
        h_Action.Value = ""
    End Sub

    Private Sub BtnCancelItemReturn_Click(sender As Object, e As EventArgs) Handles BtnCancelItemReturn.Click
        vSQL = "update prod_return_materials set TranStatus='1', " _
            & "Remarks ='Cancel by: " & Session("uid") & "; Date: " & Now & " ' where TranId=" & h_ReturnId.Value

        CreateRecord(vSQL)
        vScript = "alert('Successfully saved.');"

        h_ReturnId.Value = ""
        Builder()
    End Sub
End Class


