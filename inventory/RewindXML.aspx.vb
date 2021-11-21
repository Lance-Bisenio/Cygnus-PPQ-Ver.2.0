Imports denaro

Partial Class RewindXML
    Inherits System.Web.UI.Page
    Public vScript As String = ""
    Public vProcess As String = ""
    Public vReturnData As String = ""
    Public vData As String = ""
    Public vMCostSummary As String = ""
    Dim vSQL As String = ""

    Dim vJOBOM As String = ""
    Dim vJOBOMRev As Integer

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Session("uid") = Nothing Or Session("uid") = "" Then
            vScript = "alert('Login session has expired. Please re-login again.'); window.close();"
            Exit Sub
        End If

        Select Case Request.Item("pModule")
            Case "Rewind"
                If Request.Item("pMode") = "Complete" Then
                    vSQL = "update prod_sfgrewind set RewindBy='" & Session("uid") & "', DateRewind='" & Now & "' " _
                        & " where CompletionTranId=" & Request.Item("pId")
                Else
                    vSQL = "update prod_sfgrewind set RewindBy=null, DateRewind=null " _
                        & "where CompletionTranId=" & Request.Item("pId")
                End If

                Dim valTemp As String = Session("uid") & "<br>" & Now

                CreateRecord(vSQL)
                Response.Write(valTemp)

            Case "Completion"
                If Request.Item("pMode") = "Add" Then
                    vSQL = " insert into prod_sfgrewind (JONO,CompletionTranId,Remarks,CreatedBy,DateCreated) " _
                        & "values ('" & Request.Item("pJO") & "'," & Request.Item("pId") & ", null,'" & Session("uid") & "','" & Now & "')"
                Else
                    vSQL = "delete from prod_sfgrewind " _
                        & "where CompletionTranId=" & Request.Item("pId")
                End If

                CreateRecord(vSQL)
        End Select

    End Sub

End Class
