Imports denaro
Partial Class inventory_materials_request
    Inherits System.Web.UI.Page

    Public vScript As String = ""
    Public vHeader As String = ""
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
        vTitle = "Production raw-material request"
    End Sub
    Private Sub Builder()
        vJO = Request.Item("pJO")
        vBOM = Request.Item("pBom")
        vBOMRev = Request.Item("pBomRev")

        ProductionMaterial_Receiving_Header()
        ProductionMaterial_Return()
        cmdSave.Visible = True
        cmdCancel.Visible = True

    End Sub

    Private Sub ProductionMaterial_Receiving_Header()

        vHeader = "<tr>" _
                & "<td style='width:100px;' class='titleBar'>Material Code</td>" _
                & "<td style='width:540px;' class='titleBar'>Description</td>" _
                & "<td style='width:80px;' class='titleBar'>UOM</td>"

        vHeader += "<td style='width:70px;' class='titleBar'>QTY</td></tr>"

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
            vSQL = "delete from prod_request_materials where " _
                        & "JONO='" & vJONO & "' and " _
                        & "OperOrder=" & vOperNo

            cm_sub2.CommandText = vSQL

            Try
                cm_sub2.ExecuteNonQuery()
            Catch ex As SqlClient.SqlException
                Response.Write("Error In SQL query Receiving raw-materials :  " & ex.Message)
            End Try
        End If


        If Session("ViewAll") = "YES" Then
            vFilter = ""
        Else
            vFilter = " and DateReceived is not null"
            Session.Remove("ViewAll")
        End If

        vSQL = "select distinct(Item_Cd), UOM, " _
                    & "(select (Descr + ' ' + Descr) as Descr from item_master " _
                        & "where item_master.Item_Cd=a.Item_Cd) as vDescr, " _
                    & "(select Qty from prod_request_materials b where " _
                        & "b.JONO=a.JONO and  " _
                        & "b.Item_Cd = a.Item_Cd) as vRQQty " _
                & "from prod_rawmaterial a where " _
                & "BOM='" & vBOM & "' and " _
                & "Revision='" & vBOMRev & "' and " _
                & "JONO='" & vJONO & "' and " _
                & "Sect_Cd='" & vSection & "' and " _
                & "Proc_Cd='" & vProcess & "'  " _
                & vFilter _
                & " Order by vDescr"

        '& "and Item_Cd in (" & Session("vProc_Matlist") & ")" 
        'Response.Write(vSQL)
        cm.CommandText = vSQL

        Try
            rs = cm.ExecuteReader
            Do While rs.Read
                iCtr += 1
                vID = iCtr & "_" & rs("Item_Cd")

                'vQty = IIf(IsDBNull(rs("RQty")), rs("Qty"), rs("RQty"))

                vData += "<tr style='height:24px;'>" & _
                    "<td class='labelC'>" & rs("Item_Cd") & "</td>" & _
                    "<td class='labelL'>" & rs("vDescr") & "</td>" & _
                    "<td class='labelC'>" & rs("UOM") & "</td>" & _
                    "<td class='labelR'>"

                If Not IsDBNull(rs("vRQQty")) Then
                    vQty = rs("vRQQty")
                Else
                    vQty = "0.00"
                End If

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
                        vSQL = "insert into prod_request_materials " _
                            & "(BOM, Revision, Sect_Cd, Proc_Cd, JONO, Item_Cd, Qty, UOM, " _
                            & "OperOrder, CreatedBy, DateCreated, RequestRef) " _
                            & "values ("

                        vSQL += vBOM & "," & vBOMRev & ",'" & vSection & "','" & vProcess & "','" & vJONO & "','" _
                            & rs("Item_Cd") & "','" & vValue & "','" & rs("UOM") & "','" _
                            & vOperNo & "','" & Session("uid") & "','" & Now() & "','" & vJONO & "-" & vOperNo & "')"

                        'Response.Write(vSQL & "<br>")
                        cm_sub2.CommandText = vSQL

                        Try
                            cm_sub2.ExecuteNonQuery()
                        Catch ex As SqlClient.SqlException
                            Response.Write("Error in SQL query Receiving raw-materials :  " & ex.Message)
                        End Try
                    End If

                End If
                vQty = "0"
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
            vScript = "alert('Successfully saved.'); window.close();" ' window.opener.document.form1.submit();
            h_Action.Value = ""
        End If

    End Sub

    Protected Sub cmdSave_Click(sender As Object, e As EventArgs) Handles cmdSave.Click
        h_Action.Value = "Save"
        Builder()
        h_Action.Value = ""
    End Sub



End Class

