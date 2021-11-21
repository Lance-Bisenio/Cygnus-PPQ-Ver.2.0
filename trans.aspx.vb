Imports denaro.fis
Partial Class trans
    Inherits System.Web.UI.Page
    Public vScript As String = ""
    Public vBuildTransaction As String = ""

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("uid") = "" Or Session("uid") = Nothing Then
            vScript = "alert('Your login session has expired. Plase re-login again.'); window.close();"
            Exit Sub
        End If

        Select Case Request.Item("vTranType")
            Case 1
                GetAllTransaction_Accounts()
            Case 2
                GetAllTransaction_Customer()
        End Select

    End Sub

    Private Sub GetAllTransaction_Accounts()
        Dim vMonths() As String = {"Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"}
        Dim iCtr As Integer = 0

        vBuildTransaction += "<tr class='titleBar'>" & _
            "<th style='border: solid 1px #8B8B8A; width:90px;'>Tran Date</th>" & _
            "<th style='border: solid 1px #8B8B8A; width:90px;'>Tran ID</th>" & _
            "<th style='border: solid 1px #8B8B8A;'>Reference</th>" & _
            "<th style='border: solid 1px #8B8B8A; width:90px;'>Cost Center</th>" & _
            "<th style='border: solid 1px #8B8B8A; width:90px;'>Acct Code</th>" & _
            "<th style='border: solid 1px #8B8B8A; width:90px;'>Debit</th>" & _
            "<th style='border: solid 1px #8B8B8A; width:90px;'>Credit</th>" & _
            "<th style='border: solid 1px #8B8B8A;width:90px;'>Actual</th>"

        Dim c As New SqlClient.SqlConnection(connStr)
        Dim cm As New SqlClient.SqlCommand
        Dim rs As SqlClient.SqlDataReader

        Dim vTtlActual As Decimal
        Dim vActual As Decimal

        Try
            c.Open()
        Catch ex As SqlClient.SqlException
            vScript = "alert('Error occurred while trying to connect to database. Error is: " & ex.Message.Replace(vbCrLf, "").Replace("'", "") & "');"
            c.Dispose()
            cm.Dispose()
            Exit Sub
        End Try

        cm.Connection = c

        cm.CommandText = "select * from sap_OJDT1 where Account=" & Request.Item("vAcct") & " and Project='" & _
            Request.Item("vBudgID") & "' and Month(RefDate)='" & Request.Item("vMonth") + 1 & "' and Year(RefDate)='" & Request.Item("vYear") & "'"

        Try
            rs = cm.ExecuteReader
            Do While rs.Read
                vActual = IIf(IsDBNull(rs("Debit")), 0, rs("Debit")) - IIf(IsDBNull(rs("Credit")), 0, rs("Credit"))

                vBuildTransaction += "<tr><td class='Tbl_labelC'>" & Format(rs("RefDate"), "MM/dd/yyy") & "</td>" & _
                     "<td class='Tbl_labelC'>" & rs("TransId") & "</td>" & _
                     "<td class='Tbl_labelL'>" & rs("Ref2") & "</td>" & _
                     "<td class='Tbl_labelC'>" & rs("Project") & "</td>" & _
                    "<td class='Tbl_labelC'>" & rs("Account") & "</td>" & _
                    "<td class='Tbl_labelR'>" & Format(rs("Debit"), "#,###,###,##0.00") & "</td>" & _
                    "<td class='Tbl_labelR'>" & Format(rs("Credit"), "#,###,###,##0.00") & "</td>" & _
                     "<td class='Tbl_labelR'>" & Format(vActual, "#,###,###,##0.00") & "</td></tr>"

                'RefDate,TransId,Ref2,Project,Account,Debit,Credit
                ' "<td class='Tbl_labelC'>" & rs("Rc_Descr") & "</td>" & _
                ' "<td class='Tbl_labelC'>" & rs("Acct_Descr") & "</td>" & _
                vTtlActual += vActual
            Loop
            rs.Close()

            vBuildTransaction += "<tr><td class='Tbl_labelR' colspan='7'><b>Total Actual :</b></td>" & _
                "<td class='Tbl_labelR'><b>" & Format(vTtlActual, "#,###,###,##0.00") & "</b></td></tr>"


        Catch ex As SqlClient.SqlException
            vScript = "alert('Error occurred while trying to retrieve Budget Detail. Error is: " & _
                ex.Message.Replace(vbCrLf, "").Replace("'", "") & "');"
        Finally
            c.Close()
            c.Dispose()
            cm.Dispose()
        End Try
    End Sub

    Private Sub GetAllTransaction_Customer()

        Dim vMonths() As String = {"Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"}
        Dim iCtr As Integer = 0
        Dim vTtl_Amt As Decimal = 0
        Dim vGT_Amt As Decimal = 0
        Dim vTableRef_a As String = ""
        Dim vTableRef_b As String = ""

        Select Case Request.Item("vTranType")
            Case 2
                vTableRef_a = " sap_OINV1 a, sap_OINV b "
                vTableRef_b = " sap_ORIN1 a, sap_ORIN b "
            Case 3
                vTableRef_a = " sap_OPCH1 a, sap_OPCH b "
                vTableRef_b = " sap_ORPC1 a, sap_ORPC b "
        End Select

        vBuildTransaction += "<tr class='titleBar'>" & _
            "<th style='border: solid 1px #8B8B8A; width:80px;'>Tran Date</th>" & _
            "<th style='border: solid 1px #8B8B8A; width:60px;'>Tran ID</th>" & _
            "<th style='border: solid 1px #8B8B8A; width:90px;'>Card Code</th>" & _
            "<th style='border: solid 1px #8B8B8A; width:120px;'>Item Code</th>" & _
            "<th style='border: solid 1px #8B8B8A;'>Item Name</th>" & _
            "<th style='border: solid 1px #8B8B8A; width:80px;'>Quantity</th>" & _
            "<th style='border: solid 1px #8B8B8A; width:90px;'>Price</th>" & _
            "<th style='border: solid 1px #8B8B8A;width:90px;'>Ttl Price</th></tr>"

        Dim c As New SqlClient.SqlConnection(connStr)
        Dim cm As New SqlClient.SqlCommand
        Dim rs As SqlClient.SqlDataReader

        Dim vTtlActual As Decimal
        Dim vActual As Decimal

        Try
            c.Open()
        Catch ex As SqlClient.SqlException
            vScript = "alert('Error occurred while trying to connect to database. Error is: " & ex.Message.Replace(vbCrLf, "").Replace("'", "") & "');"
            c.Dispose()
            cm.Dispose()
            Exit Sub
        End Try

        cm.Connection = c
        cm.CommandText = "select a.DocEntry, a.DocDate, ItemCode, Quantity, Price, PriceAfVAT,CardCode, b.DocEntry, " & _
            "(select AcctName from coa where AcctCd=ItemCode) as vItemName " & _
            "from " & vTableRef_a & " where a.DocEntry=b.DocEntry and Month(a.DocDate)='" & Request.Item("vMonth") + 1 & "' " & _
            "and Year(a.DocDate)='" & Request.Item("vYear") & "' and ItemCode='" & Request.Item("vAcct") & "'"
        Try
            rs = cm.ExecuteReader
            Do While rs.Read
                vTtl_Amt = rs("Quantity") * rs("Price")
                vBuildTransaction += "<tr><td class='Tbl_labelC'>" & Format(rs("DocDate"), "MM/dd/yyy") & "</td>" & _
                     "<td class='Tbl_labelC'>" & rs("DocEntry") & "</td>" & _
                     "<td class='Tbl_labelC'>" & rs("CardCode") & "</td>" & _
                     "<td class='Tbl_labelC'>" & rs("ItemCode") & "</td>" & _
                    "<td class='Tbl_labelL'>" & rs("vItemName") & "</td>" & _
                    "<td class='Tbl_labelR'>" & Format(rs("Quantity"), "#,###,###,##0.00") & "</td>" & _
                    "<td class='Tbl_labelR'>" & Format(rs("Price"), "#,###,###,##0.00") & "</td>" & _
                    "<td class='Tbl_labelR'>" & Format(vTtl_Amt, "#,###,###,##0.00") & "</td></tr>"

                vTtlActual += rs("Quantity")
                vGT_Amt += vTtl_Amt
            Loop
            rs.Close()



            cm.CommandText = "select a.DocEntry, a.DocDate, ItemCode, Quantity, Price, PriceAfVAT,CardCode, b.DocEntry, " & _
           "(select AcctName from coa where AcctCd=ItemCode) as vItemName " & _
           "from " & vTableRef_b & " where a.DocEntry=b.DocEntry and Month(a.DocDate)='" & Request.Item("vMonth") + 1 & "' " & _
           "and Year(a.DocDate)='" & Request.Item("vYear") & "' and ItemCode='" & Request.Item("vAcct") & "'"
            'Response.Write(cm.CommandText)

            rs = cm.ExecuteReader
            Do While rs.Read
                vTtl_Amt = rs("Quantity") * rs("Price")
                vBuildTransaction += "<tr><td class='Tbl_labelC'>" & Format(rs("DocDate"), "MM/dd/yyy") & "</td>" & _
                    "<td class='Tbl_labelC'>" & rs("DocEntry") & "</td>" & _
                    "<td class='Tbl_labelC'>" & rs("CardCode") & "</td>" & _
                    "<td class='Tbl_labelC'>" & rs("ItemCode") & "</td>" & _
                    "<td class='Tbl_labelL'>" & rs("vItemName") & "</td>" & _
                    "<td class='Tbl_labelR' style='color:#ff0d25;'>(" & Format(rs("Quantity"), "#,###,###,##0.00") & ")</td>" & _
                    "<td class='Tbl_labelR'>" & Format(rs("Price"), "#,###,###,##0.00") & "</td>" & _
                    "<td class='Tbl_labelR' style='color:#ff0d25;'>(" & Format(vTtl_Amt, "#,###,###,##0.00") & ")</td></tr>"

                vTtlActual -= rs("Quantity")
                vGT_Amt -= vTtl_Amt
            Loop
            rs.Close()

            vBuildTransaction += "<tr><td class='Tbl_labelR' colspan='5'><b>Total Actual by Qty :</b></td>" & _
                "<td class='Tbl_labelR'><b>" & Format(vTtlActual, "#,###,###,##0.00") & "</b></td> " & _
                "<td class='Tbl_labelR'><b>by Amount :</b></td> " & _
                "<td class='Tbl_labelR'><b>" & Format(vGT_Amt, "#,###,###,##0.00") & "</b></td></tr>"

        Catch ex As SqlClient.SqlException
            vScript = "alert('Error occurred while trying to retrieve Budget Detail. Error is: " & _
                ex.Message.Replace(vbCrLf, "").Replace("'", "") & "');"
        Finally
            c.Close()
            c.Dispose()
            cm.Dispose()
        End Try
    End Sub
End Class
