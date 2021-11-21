Imports denaro
Partial Class inventory_joborderlist_xml
    Inherits System.Web.UI.Page
    Public vReturn As String = ""
    Public vScript As String = ""

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load


        If Session("uid") <> "" Then
            modify()
        End If

    End Sub

    Private Sub modify()
        Dim c As New SqlClient.SqlConnection(connStr)
        Dim cm As New SqlClient.SqlCommand
        Dim vSQL As String = ""

        Dim vLoadDate() As String = Request.Item("pDate").Split("_")
        Dim vStartDate As Date = Format(CDate(vLoadDate(0) & " " & vLoadDate(1) & ":00"), "MM/dd/yyyy HH:mm")

         
        Try
            c.Open()
        Catch ex As SqlClient.SqlException
            vScript = "alert('Error occurred while trying to connect to database. Error code 101; Error is: " & ex.Message.Replace(vbCrLf, "").Replace("'", "") & "');"
            c.Dispose()
            cm.Dispose()

            Exit Sub
        End Try

        cm.Connection = c 

        vSQL = "update jo_machine set StartDate='" & vStartDate & "', CreatedBy='" & Session("uid") & "', DateCreated='" & Now() & "' where TranId=" & Request.Item("pTranId")

        vReturn = vSQL

        cm.CommandText = vSQL

        Try
            cm.ExecuteNonQuery()

        Catch ex As DataException
            vScript = "alert('An error occurred while trying to Save the new record.');"
        End Try
        c.Close()
        c.Dispose()
        cm.Dispose()

    End Sub
End Class
