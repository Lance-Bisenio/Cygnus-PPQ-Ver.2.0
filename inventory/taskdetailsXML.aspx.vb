Imports denaro
Partial Class inventory_taskdetailsXML
    Inherits System.Web.UI.Page
    Dim vSQL As String = ""

    Private Sub form1_Load(sender As Object, e As EventArgs) Handles Me.Load

        'pTranId=" + pTranId + "&pJO=" + pJO + "&pBatchNo=" + pBatchNo


        Select Case Request.Item("pType")
            Case "CancelPerItem"
                Response.Write("Successfully deleted")

                vSQL = "delete from prod_printlabel where TranId='" & Request.Item("pTranId") & "'"
                CreateRecord(vSQL)

            Case "AddToPrint"
                If Request.Item("pBatchNo").ToString.Trim <> "" Then
                    Response.Write("Successfully Saved")

                    vSQL = "insert into prod_printlabel values (" _
                        & "'" & Request.Item("pBatchNo") & "'," _
                        & "'" & Request.Item("pJO") & "'," _
                        & "0," _
                        & "'" & Session("uid") & "'," _
                        & "'" & Now() & "'" _
                        & ")"

                    CreateRecord(vSQL)
                End If
        End Select





    End Sub
End Class
