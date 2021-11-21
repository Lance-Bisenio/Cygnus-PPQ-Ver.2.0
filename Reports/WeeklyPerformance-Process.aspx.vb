
Partial Class WeeklyPerformanceReportProcess
    Inherits System.Web.UI.Page

    Public vData As String = ""

    Private Sub form1_Load(sender As Object, e As EventArgs) Handles form1.Load


        vData += "<tr><td>Lance </td><td>" & Now & "</td><td>" & Request.Form("fname") & "</td><td>" & Request.Form("lname") & "</td></tr>"


    End Sub
End Class
