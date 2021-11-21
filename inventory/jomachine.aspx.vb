Imports denaro
Partial Class inventory_jomachine
    Inherits System.Web.UI.Page
    Public vScript As String = ""
    Public vData As String = ""
    Public vHeader As String = ""


    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Session("uid") = Nothing Or Session("uid") = "" Then
            vScript = "alert('Login session has expired. Please re-login again.'); window.close();"
            Exit Sub
        End If

        If Not IsPostBack Then
            DataRefresh()
        End If 
    End Sub

    Private Sub DataRefresh()
        Dim c As New SqlClient.SqlConnection(connStr)
        Dim cm As New SqlClient.SqlCommand
        Dim rs As SqlClient.SqlDataReader

        Dim cm_sub As New SqlClient.SqlCommand
        Dim rs_sub As SqlClient.SqlDataReader

        Dim cm_modify As New SqlClient.SqlCommand
        'Dim rs_modify As SqlClient.SqlDataReader

        Dim iCtr As Integer = 1
        Dim iLineID As Integer = 1
        Dim iSubLineID As Integer = 1
        Dim R_IsSelected As String = ""
        Dim R_Selected As String = ""
        Dim C_Selected As String = ""
        Dim vSQL As String = ""
        Dim vMachineCtr As Integer = 0
        Dim vStartDate As String = ""
        Dim vSTime_HH As Integer = 0
        Dim vSTime_MM As String = ""
        Dim vSelected As String = ""
        Dim vRdoName As String = ""
        Dim vChkName As String = ""
		Dim vProdDate As String = ""
		Dim vProdDateTime As Date
		Dim vValue As String = ""

        Dim vKilos As String = ""
        Dim vMeter As String = ""

        Dim vJONO As String = GetRef("Select JobOrderNo from jo_header where TranId=" & Request.Item("pTranID"), 0)
        Dim vBOM As String = GetRef("Select BOM_Cd from jo_header where TranId=" & Request.Item("pTranID"), 0)
        Dim vBOM_Rev As String = GetRef("Select BOMRev from jo_header where TranId=" & Request.Item("pTranID"), 0)

        Dim IsPrimary As String = ""


        Try
            c.Open()
        Catch ex As SqlClient.SqlException
            vScript = "alert('Error occurred while trying to connect to database. Error code 101; Error is: " & ex.Message.Replace(vbCrLf, "").Replace("'", "") & "');"
            c.Dispose()
            cm.Dispose()
            cm_sub.Dispose()
            cm_modify.Dispose()
            Exit Sub
        End Try

        cm.Connection = c
        cm_sub.Connection = c
        cm_modify.Connection = c

        vSQL = "select OperOrder, SFG_Cd, SFG_Descr, Sect_Cd, Proc_Cd," & _
                    "(select Descr from ref_emp_section where Section_Cd=Sect_Cd) as Sec_Name," & _
                    "(select Descr from ref_item_process where bom_process.Proc_Cd=ref_item_process.Proc_Cd and bom_process.Sect_Cd=ref_item_process.Sect_Cd) as Proc_Name," & _
                    "(select Descr +' - '+ Descr1  from item_master where Item_Cd=SFG_Cd) as SFG_Name " & _
                    "from bom_process where BOM_Cd=" & vBOM & " and Revision=" & vBOM_Rev & " order by OperOrder"

        'Response.Write(vSQL)
        cm.CommandText = vSQL

        Try
            rs = cm.ExecuteReader
            Do While rs.Read
                vMachineCtr = GetRef("select count(*) from ref_item_machine where Sect_Cd ='" & rs("Sect_Cd") & "'", 0)

                vData += "<tr><td class='labelC' style='vertical-align:top; padding: 5px;' rowspan='" & vMachineCtr + 1 & "'>" & rs("OperOrder") & "</td>" & _
                    "<td class='labelL' style='vertical-align:top; padding: 5px;' rowspan='" & vMachineCtr + 1 & "'><b>" & rs("SFG_Cd") & "</b><br><br>" & rs("SFG_Descr") & "</td>" & _
                    "<td class='labelC' style='background: #00aeeb; padding:3px; color:#fff;' colspan='3'><b>" & rs("Sec_Name") & " / " & rs("Proc_Name") & "</b></td>" & _
                    "<td class='text-center' style='background: #00aeeb; padding:3px; color:#fff;'><b>MM/DD/YYYY</b></td>" & _
                    "<td class='text-center' style='background: #00aeeb; padding:3px; color:#fff;'><b></b></td>"

                vData += "</tr>"


                vSQL = "select Mach_Cd, Descr, " & _
                    "(select IsPrimary from jo_machine where JobOrderNo='" & vJONO & "' and OperOrder=" & rs("OperOrder") & " and jo_machine.Mach_Cd=ref_item_machine.Mach_Cd) as vSelected, " & _
                    "(select Startdate from jo_machine where JobOrderNo='" & vJONO & "' and OperOrder=" & rs("OperOrder") & " and jo_machine.Mach_Cd=ref_item_machine.Mach_Cd) as vProdDate, " & _
                    "(select DATEPART(HOUR, Startdate) from jo_machine where JobOrderNo='" & vJONO & "' and OperOrder=" & rs("OperOrder") & " and jo_machine.Mach_Cd=ref_item_machine.Mach_Cd) as vProdDate_HH, " & _
                    "(select DATEPART(MINUTE, Startdate) from jo_machine where JobOrderNo='" & vJONO & "' and OperOrder=" & rs("OperOrder") & " and jo_machine.Mach_Cd=ref_item_machine.Mach_Cd) as vProdDate_MM, " & _
                    "(select Kilos from jo_machine where JobOrderNo='" & vJONO & "' and OperOrder=" & rs("OperOrder") & " and jo_machine.Mach_Cd=ref_item_machine.Mach_Cd) as Kilos, " & _
                    "(select Meter from jo_machine where JobOrderNo='" & vJONO & "' and OperOrder=" & rs("OperOrder") & " and jo_machine.Mach_Cd=ref_item_machine.Mach_Cd) as Meter " & _
                    " from ref_item_machine where Sect_Cd ='" & rs("Sect_Cd") & "' Order by Descr"

                'Response.Write(vSQL)
                cm_sub.CommandText = vSQL 
                rs_sub = cm_sub.ExecuteReader

                Do While rs_sub.Read
                    iLineID += 1

                    'R_IsSelected = GetRef(, "")


                    ' -----------------------------------------------------------------------------------------------------------------------------------------------
                    ' RADIO AND CHECK-BOX CONFIGURATION
                    ' -----------------------------------------------------------------------------------------------------------------------------------------------
                    R_Selected = ""
                    C_Selected = ""

                    If Not IsDBNull(rs_sub("vSelected")) Then
                        Select Case rs_sub("vSelected")
                            Case "YES"
                                R_Selected = "checked='checked' "
                                C_Selected = "disabled='disabled' "
                            Case "NO"
                                R_Selected = ""
                                C_Selected = "checked='checked' "
                            Case "NOT-USE"
                                R_Selected = ""
                                C_Selected = ""
                        End Select
                    Else
                        Select Case iCtr
                            Case 1
                                R_Selected = "checked='checked' "
                                C_Selected = "disabled='disabled' "
                            Case 2
                                R_Selected = ""
                                C_Selected = "checked='checked' "
                        End Select
                    End If
                    ' -----------------------------------------------------------------------------------------------------------------------------------------------

                    vData += "<tr>" _
                    & "<td class='text-left'><b>" & rs_sub("Mach_Cd") _
                        & "</b><br>" & rs_sub("Descr") & "</td>"

                    ' -----------------------------------------------------------------------------------------------------------------------------------------------
                    ' BUILD THE RADIO BUTTON AS PRIMARY OPTION  
                    ' ----------------------------------------------------------------------------------------------------------------------------------------------- 
                    vRdoName = rs("OperOrder") & "_" & rs("OperOrder") & "_" & rs_sub("Mach_Cd")
                    vData += "<td class='text-center'>" & _
                                "<input type='radio' " & _
                                    "id='" & vRdoName & "_Rdo' " & _
                                    "name='" & vRdoName & "_Rdo' " & _
                                    "class='" & rs("OperOrder") & "_" & rs("Sect_Cd") & "_Rdo' " & R_Selected & " " & _
                                    "onClick='Settings(""" & vRdoName & """, this.name,""" & rs("OperOrder") & "_" & rs("Sect_Cd") & """);'></td>"



                    ' -----------------------------------------------------------------------------------------------------------------------------------------------
                    ' BUILD THE CHECK-BOX AS ALTERNATIVE OPTION  
                    ' -----------------------------------------------------------------------------------------------------------------------------------------------
                    vChkName = rs("OperOrder") & "_" & rs("OperOrder") & "_" & rs_sub("Mach_Cd")
                    vData += "<td class='text-center'>" & _
                                "<input type='checkbox'" & _
                                    "id='" & vChkName & "_Chk' " & _
                                    "name='" & vChkName & "_Chk' " & _
                                    "class='" & rs("OperOrder") & "_" & rs("Sect_Cd") & "_Chk' " & C_Selected & "></td>"


                    ' -----------------------------------------------------------------------------------------------------------------------------------------------
                    ' DEFAULT SETTINGS : PRODUCTION DATE AND PRODUCTION TIME
                    ' -----------------------------------------------------------------------------------------------------------------------------------------------
                    'If iLineID = 2 Then
                    vStartDate = GetRef("select StartDate from jo_header where TranId=" & Request.Item("pTranID"), 0)
                    If vStartDate.ToString <> "null" Then
                        vStartDate = Format(CDate(vStartDate), "MM/dd/yyyy")
                    Else
                        vStartDate = ""
                    End If

                    vSTime_HH = GetRef("select DATEPART(HOUR, StartDateTime) from jo_header where TranId=" & Request.Item("pTranID"), 0)
                    vSTime_MM = GetRef("select DATEPART(MINUTE, StartDateTime)  from jo_header where TranId=" & Request.Item("pTranID"), 0)

                    'Response.Write("select DATEPART(HOUR, StartDateTime) from jo_header where TranId=" & Request.Item("pTranID"))
                    'End If  



                    If iSubLineID = 1 Then ' ONETIME DISPLAY TO LINE 1 PER SECTION

                        ' -----------------------------------------------------------------------------------------------------------------------------------------------
                        ' BUILD THE PROCESS START DATE
                        ' ----------------------------------------------------------------------------------------------------------------------------------------------- 
                        If Not IsDBNull(rs_sub("vProdDate")) Then
                            vStartDate = rs_sub("vProdDate")
                            vStartDate = Format(CDate(vStartDate), "MM/dd/yyyy")
                        End If

                        If vStartDate = "" Then
							vStartDate = Format(CDate(Now()), "MM-dd-yyyy")
						End If

                        vData += "<td class='text-right' style='vertical-align:top;  padding: 5px;' rowspan='" & vMachineCtr & "'>"
                        vData += "<input type='text' class='vDate' style='width:100%; padding:4px' " & _
                                    "id='" & vRdoName & "_Date'" & _
                                    "name='" & vRdoName & "_Date'" & _
                                    "value='" & vStartDate & "' >"


                        ' -----------------------------------------------------------------------------------------------------------------------------------------------
                        ' BUILD THE DROPDOWN START TIME BY HOURS
                        ' -----------------------------------------------------------------------------------------------------------------------------------------------
                        'vData += "<td class='labelC' style='vertical-align:top; padding: 5px;' rowspan='" & vMachineCtr & "'>"
                        vData += "HH : <select " & _
                                    "id='" & vRdoName & "_HH'  " & _
                                    "name='" & vRdoName & "_HH' " & _
                                    "style='width:60px;padding:4px; margin-top:2px;'>"

                        For i = 0 To 23
                            If vSTime_HH = i And iLineID = 2 Then
                                vSelected = "selected='selected'"
                            End If

                            If Not IsDBNull(rs_sub("vProdDate_HH")) Then
                                If rs_sub("vProdDate_HH") = i Then
                                    vSelected = "selected='selected'"
                                End If
                            End If
                            vData += "<option value=" & Format(i, "00") & " " & vSelected & ">" & Format(i, "00") & "</option>"
                            vSelected = ""
                        Next
                        vData += "</select>"


                        ' -----------------------------------------------------------------------------------------------------------------------------------------------
                        ' BUILD THE DROPDOWN START TIME BY MINUTES
                        ' -----------------------------------------------------------------------------------------------------------------------------------------------
                        vData += "<br>MM : <select " & _
                                    "id='" & vRdoName & "_MM' " & _
                                    "name='" & vRdoName & "_MM' " & _
                                    "style='width:60px;padding:4px; margin-top:2px;'>"
                        For i = 0 To 59
                            If vSTime_MM = i And iLineID = 2 Then
                                vSelected = "selected='selected'"
                            End If

                            If Not IsDBNull(rs_sub("vProdDate_HH")) Then
                                If rs_sub("vProdDate_MM") = i Then
                                    vSelected = "selected='selected'"
                                End If
                            End If
                            vData += "<option value=" & Format(i, "00") & " " & vSelected & ">" & Format(i, "00") & "</option>"
                            vSelected = ""
                        Next
                        vData += "</select></td> "



                        ' -----------------------------------------------------------------------------------------------------------------------------------------------
                        If Not IsDBNull(rs_sub("Kilos")) Then
                            vKilos = rs_sub("Kilos")
                        End If

                        vData += "<td class='text-right' style='vertical-align:top; padding: 5px;' rowspan='" & vMachineCtr & "'>"
                        vData += "Kilos : <input type='text' class='vDate' style='width:80px; text-align:right;padding:4px;' " & _
                                    "id='" & vRdoName & "_Kilos'" & _
                                    "name='" & vRdoName & "_Kilos'" & _
                                    "value='" & vKilos & "' placeholder='Kilos'><br>"


                        If Not IsDBNull(rs_sub("Meter")) Then
                            vMeter = rs_sub("Meter")
                        End If

                        vData += "Meter : <input type='text' class='vDate' style='width:80px; text-align:right;padding:4px; margin-top:2px;' " & _
                                    "id='" & vRdoName & "_Meter'" & _
                                    "name='" & vRdoName & "_Meter'" & _
                                    "value='" & vMeter & "' placeholder='Meter' > </td>"

                        'vData += "<td class='' style='vertical-align:top; padding: 5px;' rowspan='" & vMachineCtr & "'>"
                        'vData += "<input type='text' class='vDate' style='width:100%; text-align:left;padding:4px;' " & _
                        '            "id='" & vRdoName & "_Meter'" & _
                        '            "name='" & vRdoName & "_Meter'" & _
                        '            "value='" & vMeter & "' placeholder='Meter' > </td>"

                        iCtr += 1
                    End If

                    vData += "</tr>"
                    iSubLineID += 1
                     

                    ' -----------------------------------------------------------------------------------------------------------------------------------------------
                    ' MACHINE CHECKING PRIMARY OR ALTERNATIVE
                    ' -----------------------------------------------------------------------------------------------------------------------------------------------
                    If Request.Item(vRdoName & "_Rdo") = "on" Then
                        IsPrimary = "YES"
                    ElseIf Request.Item(vChkName & "_Chk") = "on" Then
                        IsPrimary = "NO"
                    Else
                        IsPrimary = "NOT-USE"
                    End If

                    

                    ' -----------------------------------------------------------------------------------------------------------------------------------------------
                    ' BUILD THE PRODUCTION DATE [FORMAT : YYYY/DD/MM HH:MM]
                    ' -----------------------------------------------------------------------------------------------------------------------------------------------
                    If Request.Item(vRdoName & "_Date") <> "" Then
						'vProdDate = Format(CDate(Request.Item(vRdoName & "_Date")), "MM-dd-yyyy") & " " & Request.Item(vRdoName & "_HH") & ":" & Request.Item(vRdoName & "_MM")
						vProdDate = Request.Item(vRdoName & "_Date") & " " & Request.Item(vRdoName & "_HH") & ":" & Request.Item(vRdoName & "_MM")
						vProdDateTime = Format(CDate(vProdDate), "MM-dd-yyyy HH:mm")

					End If


                    If Request.Item(vRdoName & "_Kilos") <> "" Then
                        vKilos = "'" & Request.Item(vRdoName & "_Kilos") & "'"
                    Else
                        vKilos = "null"
                    End If

                    If Request.Item(vRdoName & "_Meter") <> "" Then
                        vMeter = "'" & Request.Item(vRdoName & "_Meter") & "'"
                    Else
                        vMeter = "null"
                    End If


                    ' -----------------------------------------------------------------------------------------------------------------------------------------------
                    ' CREATE RECORD IN JO_MACHINE TABLE
                    ' -----------------------------------------------------------------------------------------------------------------------------------------------
                    If txtMode.Text = "SAVE" Then

                        ' -----------------------------------------------------------------------------------------------------------------------------------------------
                        ' DELETE ALL MACHINE RECORD UNDER SELECTED JOB ORDER NUMBER : ONETIME RUN
                        ' -----------------------------------------------------------------------------------------------------------------------------------------------
                        If txtDel_record.Text.Trim = "" Then
                            vSQL = "delete from jo_machine where JObOrderNo='" & vJONO & "' "
                            cm_modify.CommandText = vSQL
                            cm_modify.ExecuteNonQuery()

                            txtDel_record.Text = "deleted"
                        End If


						' -----------------------------------------------------------------------------------------------------------------------------------------------
						' INSERT THE OFFICIAL RECORD FOR JOB ORDER MACHINE
						' -----------------------------------------------------------------------------------------------------------------------------------------------
						vSQL = "insert into jo_machine (BOM_Cd,Revision,OperOrder,Proc_Cd,JobOrderNo,Sect_Cd,FG_Item_Cd,SFG_Item_Cd,Mach_Cd,Remarks,IsPrimary,StartDate,CreatedBy,DateCreated,Kilos,Meter) " _
						   & "values ('" & vBOM & "','" & vBOM_Rev & "','" & rs("OperOrder") & "','" & rs("Proc_Cd") & "'," _
						   & "'" & vJONO & "','" & rs("Sect_Cd") & "',Null,'" & rs("SFG_Cd") & "','" & rs_sub("Mach_Cd") & "'," _
						   & "'System Created','" & IsPrimary & "','" & vProdDateTime & "','" & Session("uid") & "','" & Format(CDate(Now()), "MM-dd-yyyy") & "'," & vKilos & "," & vMeter & ")"

						'Response.Write(vSQL & "<br><br>")
						cm_modify.CommandText = vSQL

                        Try
                            cm_modify.ExecuteNonQuery()

                        Catch ex As DataException
                            vScript = "alert('An error occurred while trying to Save the new record.');"
                        End Try
                    End If

                    vStartDate = ""
                    
                Loop

                rs_sub.Close()

                vData += "<tr><td colspan='10' style='height:25px;'></td></tr>"
                 
                iCtr = 1
                iLineID += 1
                iSubLineID = 1
                vMeter = ""
                vKilos = ""
            Loop

            rs.Close()
            txtMode.Text = ""

        Catch ex As SqlClient.SqlException
            vScript = "alert('Error occurred while trying to retrieve Job Order Info. Error code 102; Error is: " & _
                ex.Message.Replace(vbCrLf, "").Replace("'", "") & "');"
            c.Close()
            c.Dispose()

            cm.Dispose()
            cm_sub.Dispose()
            cm_modify.Dispose()
            Exit Sub
        End Try
         

    End Sub

    Protected Sub cmdSave_Click(sender As Object, e As EventArgs) Handles cmdSave.Click
        txtMode.Text = "SAVE"
        DataRefresh()
        txtMode.Text = ""
        vScript = "alert('Successfully saved.'); window.close();" 'window.opener.document.form1.h_Mode.value='Backto_Jo'; window.opener.document.form1.submit();    "
    End Sub
End Class
