Imports denaro
Partial Class inventory_jocomplition
    Inherits System.Web.UI.Page
    Public vScript As String = ""
    Public vCompletionList As String = ""
    Public vSFGRawMatsList As String = ""

     
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Session("uid") = Nothing Or Session("uid") = "" Then
            vScript = "alert('Login session has expired. Please re-login again.'); " & _
                "window.close();"

            '"window.opener.document.form1.h_Mode.value='reload'; " & _
            '"window.opener.document.form1.submit(); " & _
            Exit Sub
        End If

        If Not IsPostBack Then

            Dim iCtr As Integer = 0
            Dim vBOM As String = Request.Item("pBOM")
            Dim vRev As String = Request.Item("pBOMRev")
            Dim vJO As String = Request.Item("pJO")
            Dim vSect As String = Request.Item("pSection")
            Dim vProc As String = Request.Item("pProcess")
            Dim vOperOrder As String = Request.Item("pOperNo")
            'GetRef("select OperOrder from bom_process where BOM_Cd=" & vBOM & " and Revision=" & vRev & " and Sect_Cd='" & vSect & "' and Proc_Cd='" & vProc & "'", "")

            lblJONO.Text = Request.Item("pJO")
            'lblTranDate.Text = Format(Now(), "MM/dd/yyyy")

            lblFG.Text = GetRef("select Item_Cd from jo_header where JobOrderNo='" & Request.Item("pJO") & "' ", "")
            lblFGDescr.Text = GetRef("select Descr +' '+ Descr1 as vDescr from item_master where Item_Cd='" & lblFG.Text & "' ", "")

            lblSFGCd.Text = Request.Item("pSFG")
            lblSFGDescr.Text = GetRef("select Descr +' '+ Descr1 as vDescr from item_master where Item_Cd='" & lblSFGCd.Text & "' ", "")

            iCtr = GetRef("select count(Item_Cd) as iCtr from prod_completion where " & _
                              "JONO='" & Request.Item("pJO") & "' and " & _
                              "Sect_Cd='" & Request.Item("pSection") & "' and " & _
                              "Proc_Cd='" & Request.Item("pProcess") & "' ", 0)

            lblBatch.Text = Request.Item("pJO") & Format(Now(), "MMddyyyy") & "-" & Format(iCtr + 1, "000")
             
            lblProdStatus.Text = GetRef("select (select Descr from ref_item_status where ProdStatus=Status_Cd) as vProdStatus from prod_monitoring where " & _
                                        "JobOrderNo='" & vJO & "' and " & _
                                        "OperOrder='" & vOperOrder & "' and " & _
                                        "SFG_Item_Cd='" & lblSFGCd.Text & "' ", 0)
             
        End If


        'getSFG_Rawmats()
        getSFG_CompletionList()
         
        'If h_Mode.Value = "Update" Then
        '    SaveAndUpdate_Process()
        'End If


        If Request.Item("h_TranId") <> "" And h_Mode.Value = "Edit" Then
            h_TranId.Value = Request.Item("h_TranId")
            h_Mode.Value = "Update"

            getPrev_Completion()
        End If

    End Sub

    'Private Sub getSFG_Rawmats()
    '    Dim vBOM As String = Request.Item("pBOM")
    '    Dim vRev As String = Request.Item("pBOMRev")
    '    Dim vJO As String = Request.Item("pJO")
    '    Dim vSect As String = Request.Item("pSection")
    '    Dim vProc As String = Request.Item("pProcess")
    '    Dim vSFG As String = Request.Item("pSFG")

    '    Dim vSQL As String = ""

    '    Dim c As New SqlClient.SqlConnection
    '    Dim cm As New SqlClient.SqlCommand
    '    Dim rs As SqlClient.SqlDataReader

    '    c.ConnectionString = connStr
    '    Try
    '        c.Open()
    '    Catch ex As SqlClient.SqlException
    '        vScript = "alert('Error occurred while trying to connect to database. Error is: " & _
    '            ex.Message.Replace(vbCrLf, "").Replace("'", "") & "');"
    '        c.Dispose()
    '        cm.Dispose()
    '        Exit Sub
    '    End Try

    '    cm.Connection = c
    '    vSQL = "select Item_Cd, " & _
    '        "(select Descr from item_master where item_master.Item_Cd=bom_materials.Item_Cd) as Descr , " & _
    '        "(select ItemType_Cd from item_master where item_master.Item_Cd=bom_materials.Item_Cd) as IsSFG  " & _
    '        "from bom_materials where " & _
    '            "BOM_Cd=" & vBOM & " and " & _
    '            "Revision=" & vRev & " and " & _
    '            "Sect_Cd='" & vSect & "' and " & _
    '            "Proc_Cd='" & vProc & "' "

    '    cm.CommandText = vSQL
    '    Try
    '        rs = cm.ExecuteReader
    '        Do While rs.Read


    '            If rs("IsSFG") = "SFG" Then
    '                vSFGRawMatsList = "<tr>" & _
    '                    "<td style='width:80px; text-align:left'>" & rs("Item_Cd") & "</td>" & _
    '                    "<td style='text-align:left'>" & rs("Descr") & "</td>" & _
    '                "</tr>"
    '            End If

    '        Loop
    '        rs.Close()

    '    Catch ex As SqlClient.SqlException
    '        Response.Write("Error in SQL query Get Process Details function:  " & ex.Message)
    '    End Try

    '    c.Close()
    '    c.Dispose()
    '    cm.Dispose()

    'End Sub

    Private Sub getSFG_CompletionList()
        Dim vBOM As String = Request.Item("pBOM")
        Dim vRev As String = Request.Item("pBOMRev")
        Dim vJO As String = Request.Item("pJO")
        Dim vSect As String = Request.Item("pSection")
        Dim vProc As String = Request.Item("pProcess")
        Dim vSFG As String = Request.Item("pSFG")

        Dim vSQL As String = ""

        Dim c As New SqlClient.SqlConnection
        Dim cm As New SqlClient.SqlCommand
        Dim rs As SqlClient.SqlDataReader

        c.ConnectionString = connStr
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
        vSQL = "SELECT TranId, BatchNo FROM prod_disposal where " & _
                "JONO='" & vJO & "' and " & _
                "Sect_Cd='" & vSect & "' and " & _
                "Proc_Cd='" & vProc & "' order by DateCreated  "

        cm.CommandText = vSQL
        Try
            rs = cm.ExecuteReader
            Do While rs.Read
                vCompletionList += "<tr onclick='ModifyCompletion(""" & rs("TranId") & """,""" & _
                                rs("TranId") & """)'>" & _
                    "<td class='trLink'>" & rs("BatchNo") & "</td>" & _
                "</tr>"

            Loop
            rs.Close()

        Catch ex As SqlClient.SqlException
            Response.Write("Error in SQL query Get Process Details function:  " & ex.Message)
        End Try

        c.Close()
        c.Dispose()
        cm.Dispose()

    End Sub

    Private Sub getPrev_Completion()

        Dim vTranId As String = Request.Item("h_TranId")
        cmdClear.Visible = True

        lblBatch.Text = GetRef("SELECT BatchNo FROM prod_disposal where " & _
                "TranId=" & vTranId & " ", "")

        txtWeight.Text = GetRef("SELECT ItemWeight FROM prod_disposal where " & _
                "TranId=" & vTranId & " ", "")
         
    End Sub

    Protected Sub cmdSave_Click(sender As Object, e As EventArgs) Handles cmdSave.Click
        SaveAndUpdate_Process() 
    End Sub

    Private Sub SaveAndUpdate_Process()
        Dim vBOM As String = Request.Item("pBOM")
        Dim vRev As String = Request.Item("pBOMRev")
        Dim vJO As String = Request.Item("pJO")
        Dim vSect As String = Request.Item("pSection")
        Dim vProc As String = Request.Item("pProcess")
        Dim vSFG As String = Request.Item("pSFG")
        Dim vOperOrder As String = Request.Item("pOperNo")
        Dim vSQL As String = ""


        If Session("uid") = "" Then
            vScript = "alert('Login session has expired. Please re-login again.');  window.close();"
            'window.opener.document.form1.h_Mode.value='reload'; window.opener.document.form1.submit();
            Exit Sub
        End If

        If txtWeight.Text.Trim = "" Then
            vScript = "alert('Please enter SFG Weight.');"
            Exit Sub
        End If

        Dim c As New SqlClient.SqlConnection
        Dim cm As New SqlClient.SqlCommand
        Dim vProdStatusCode As String = ""

        vProdStatusCode = GetRef("select ProdStatus from prod_disposal where " & _
                                       "JONO='" & vJO & "' and " & _
                                       "OperOrder='" & vOperOrder & "' and " & _
                                       "SFG_Cd='" & lblSFGCd.Text & "' ", 0)
         
        c.ConnectionString = connStr
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
         
        If Request.Item("h_TranId") <> "" Then
            vSQL = "update prod_disposal set " & _
                        "ItemWeight='" & txtWeight.Text.Trim & "'," & _
                        "CreatedBy='" & Session("uid") & "'," & _
                        "DateCreated='" & Now() & "' " & _
                        "where TranId= " & Request.Item("h_TranId")

        Else
            vSQL = "Insert into prod_disposal " & _
                        "(BOM,Revision,Sect_Cd,Proc_Cd,JONO,OperOrder,SFG_Cd,BatchNo,ItemWeight,Qty,Cost,Remarks,ProdStatus,CreatedBy,DateCreated) values (" & _
                        vBOM & "," & vRev & ",'" & vSect & "','" & vProc & "','" & vJO & "'," & vOperOrder & ",'" & vSFG & "','" & _
                        lblBatch.Text.Trim & "','" & txtWeight.Text.Trim & "',0,0,'PRODUCTION WASTE','" & vProdStatusCode & "', '" & _
                        Session("uid") & "','" & Now() & "') "
        End If


        cm.CommandText = vSQL
        'Response.Write(vSQL)
        Try
            cm.ExecuteNonQuery()
        Catch ex As SqlClient.SqlException
            Response.Write("Error in SQL query insert/update records:  " & ex.Message)
        End Try

        c.Close()
        c.Dispose()
        cm.Dispose()
         
        h_TranId.Value = ""
        h_Mode.Value = ""

        vScript = "alert('Successfully saved.'); window.close();"
         
    End Sub

    Protected Sub cmdClear_Click(sender As Object, e As EventArgs) Handles cmdClear.Click

        Dim ictr As Integer = 0
        h_TranId.Value = ""
        h_Mode.Value = ""

        ictr = GetRef("select count(SFG_Cd) as iCtr from prod_disposal where " & _
                              "JONO='" & Request.Item("pJO") & "' and " & _
                              "Sect_Cd='" & Request.Item("pSection") & "' and " & _
                              "Proc_Cd='" & Request.Item("pProcess") & "' ", 0)

        'Response.Write("select count(SFG_Cd) as iCtr from prod_disposal where " & _
        '                      "JONO='" & Request.Item("pJO") & "' and " & _
        '                      "Sect_Cd='" & Request.Item("pSection") & "' and " & _
        '                      "Proc_Cd='" & Request.Item("pProcess") & "' ")

        lblBatch.Text = Request.Item("pJO") & Format(Now(), "MMddyyyy") & "-" & Format(ictr + 1, "000")
        cmdClear.Visible = False
        txtWeight.Text = ""
    End Sub
End Class
