Imports denaro

Partial Class item_invlogs
    Inherits System.Web.UI.Page
    Public vScript As String = ""
    Public vHeader As String = ""
    Public vData As String = ""
    Dim vSQL As String = ""

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Session("uid") = Nothing Or Session("uid") = "" Then
            vScript = "alert('Login session has expired. Please re-login again.'); window.close();"
            Exit Sub
        End If


        LblJONO.Text = Request.Item("pJO")
        If Not IsPostBack Then
            CollectCompletion("COMPLETION")
            CollectCompletion("WASTE")
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
        Dim vVoidBy As String = ""
        Dim vClass As String = ""

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
        vSQL = "select TranId,Qty,JONO,BatchNo,PrevBatchNoA,PrevBatchNoB,ProdCost," _
            & "CoreWeight,NetWeight,GrossWeight,TranType,Meter,CreatedBy,DateCreated,Batchgroup+''+BatchgroupLine as BatGroup,BatchCtr," _
            & "(select Emp_Fname + ' ' + Emp_Lname from emp_master where CreatedBy=Emp_Cd) as vOps, " _
            & "(select Emp_Fname + ' ' + Emp_Lname from emp_master where EditedBy=Emp_Cd) as vEditBy,EditedBy, DateEdited, " _
            & "(select Emp_Fname + ' ' + Emp_Lname from emp_master where VoidBy=Emp_Cd) as vVoidBy, VoidBy, DateVoid, " _
            & "TtlPcs, TtlPcsBox " _
            & "from prod_completion where " _
            & "JONO='" & vJO & "' and " _
            & "TranType='" & pType & "' and " _
            & "SFG_Cd='" & vSFG & "' order by TranType,DateCreated"

        '& "JONO='" & vJO & "' and " _
        '& "Sect_Cd='" & vSect & "' and " _
        '& "Proc_Cd='" & vProc & "' and " _
        '& "OperOrder=" & vOperOrder & " " _
        '& "order by TranType,DateCreated"
        'Response.Write(vSQL)
        cm.CommandText = vSQL

        Try
            rs = cm.ExecuteReader
            Do While rs.Read

                vCore = IIf(IsDBNull(rs("CoreWeight")), 0, rs("CoreWeight"))
                vGross = IIf(IsDBNull(rs("GrossWeight")), 0, rs("GrossWeight"))

                If Not IsDBNull(rs("DateVoid")) Then
                    If pType = "COMPLETION" Then
                        vTtlCompVoid += 1
                        lblTtlCompCntVoid.Text = vTtlCompVoid
                    End If
                    If pType = "WASTE" Then
                        vTtlCompVoid += 1
                        lblTtlWasteCntVoid.Text = vTtlCompVoid
                    End If
                    vClass = "style='background-color:#ccc;'"
                Else
                    vClass = ""
                End If

                vNet = vGross - vCore
                vTltComp += 1
                vCtr += 1

                vData += "<tr " & vClass & ">"

                vData += "<td>" & vCtr & "</td><td>" & rs("TranType") & "</td>" _
                    & "<td>" & rs("JONO") & "</td>" _
                    & "<td>"

                If Not IsDBNull(rs("BatGroup")) Then
                    vData += rs("BatGroup") & "-" & rs("BatchCtr")
                Else
                    vData += rs("BatchNo")
                End If


                vData += "</td>" _
                    & "<td>" _
                        & IIf(IsDBNull(rs("PrevBatchNoA")), "", rs("PrevBatchNoA")) & "</td>" _
                    & "<td style='text-align:right;'>" & vCore & "</td>" _
                    & "<td style='text-align:right;'>" & vNet & "</td>" _
                    & "<td style='text-align:right;'>" & vGross & "</td>" _
                    & "<td style='text-align:right;'>" & rs("Meter") & "</td>" _
                    & "<td style='text-align:right;'>" & rs("Qty") & "</td>" _
                    & "<td style='text-align:right;'>" & rs("TtlPcsBox") & "</td>"


                vData += "" _
                    & "<td style='text-align:left;'>" & rs("vOps") & "<br>" _
                        & Format(rs("DateCreated"), "MM/dd/yyyy HH:mm") & "</td>"


                If Not IsDBNull(rs("vEditBy")) Or rs("vEditBy").ToString = "" Then
                    vEditedBy = rs("EditedBy").ToString
                End If
                If Not IsDBNull(rs("DateEdited")) Then
                    vDateEdited = "<br>" & Format(rs("DateEdited"), "MM/dd/yyyy HH:mm")
                End If
                vData += "<td>" & vEditedBy & vDateEdited & "</td>"


                If Not IsDBNull(rs("vVoidBy")) Or rs("vVoidBy").ToString = "" Then
                    vVoidBy = rs("VoidBy").ToString
                End If
                If Not IsDBNull(rs("DateVoid")) Then
                    vDateVoid = "<br>" & Format(rs("DateVoid"), "MM/dd/yyyy HH:mm")
                Else
                    vDateVoid = ""
                End If

                vData += "<td>" & vVoidBy & vDateVoid & "</td>" _
                    & "<td>" _
                    & "<div class='btn-group btn-group-xs' role='group' aria-label='...'> " _
                    & "<Button type='button' class='btn btn-xs btn-success' data-toggle='modal' " _
                            & "onclick='ModalEdit(" & rs("TranId") & ",""" _
                                & rs("PrevBatchNoA") & """,""" _
                                & vCore & """,""" _
                                & rs("Meter") & """,""" _
                                & rs("Qty") & """)' " _
                            & "data-target='#ModalEdit'>Edit</button>"

                If Not IsDBNull(rs("DateVoid")) Then
                    vData += "<Button type='button' class='btn btn-xs btn-danger' data-toggle='modal' " _
                        & "onclick='ModalCancel(" & rs("TranId") & ",""" _
                            & rs("PrevBatchNoA") & """,""" _
                            & vCore & """,""" _
                            & rs("Meter") & """,""" _
                            & rs("Qty") & """)' " _
                    & "data-target='#ModelDelete'>Cancel</button>"

                Else
                    vData += "<Button type='button' class='btn btn-xs btn-danger' data-toggle='modal' " _
                        & "onclick='ModelDelete(" & rs("TranId") & ",""" _
                            & rs("PrevBatchNoA") & """,""" _
                            & vCore & """,""" _
                            & rs("Meter") & """,""" _
                            & rs("Qty") & """)' " _
                    & "data-target='#ModelDelete'>Void</button>"

                End If

                vData += "</div></td>" _
                & "</tr>"

                If vDateVoid.ToString.Trim = "" Then
                    vTllCore += vCore
                    vTllNet += vNet
                    vTllGross += vGross
                    vTllMeter += rs("Meter")
                    vTllQty += rs("Qty")
                    vTtlPcsBox += IIf(IsDBNull(rs("TtlPcsBox")), 0, rs("TtlPcsBox"))
                End If


                vDateEdited = ""
                vEditedBy = ""
                vVoidBy = ""
                vDateVoid = ""

            Loop

            If pType = "COMPLETION" Then
                lblTtlCompCnt.Text = vTltComp - vTtlCompVoid
            End If

            If pType = "WASTE" Then
                lblTtlWasteCnt.Text = vTltComp - vTtlCompVoid
            End If


            rs.Close()

            vData += "<tr style='font-weight:bold; color: #434343; background:#93c8ff;'>" _
                & "<td class='text-right' colspan='5'><b>TOTAL :</b></td>" _
                & "<td class='text-right'>" & vTllCore & "</td>" _
                & "<td class='text-right'>" & vTllNet & "</td>" _
                & "<td class='text-right'>" & vTllGross & "</td>" _
                & "<td class='text-right'>" & vTllMeter & "</td>" _
                & "<td class='text-right'>" & vTllQty & "</td>" _
                & "<td class='text-right'>" & vTtlPcsBox & "</td>" _
                & "<td colspan='4'></td>" _
            & "</tr><tr><td colspan='15'>&nbsp;</td></tr>"


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
           & "DateVoid='" & Format(CDate(Now), "yyyy-MM-dd HH:mm:ss") & "' " _
           & "where TranId=" & h_TranId.Value
        'Response.Write(vSQL)

        CreateRecord(vSQL)
        vScript = "alert('Saved Successfully');"

        CollectCompletion("COMPLETION")
        CollectCompletion("WASTE")
    End Sub

    Protected Sub btnVoidCancel_Click(sender As Object, e As EventArgs) Handles btnVoidCancel.Click
        vSQL = "update prod_completion set " _
           & "VoidBy=null, " _
           & "DateVoid=null " _
           & "where TranId=" & h_TranId.Value
        'Response.Write(vSQL)

        CreateRecord(vSQL)
        vScript = "alert('Saved Successfully');"

        CollectCompletion("COMPLETION")
        CollectCompletion("WASTE")
    End Sub

End Class
