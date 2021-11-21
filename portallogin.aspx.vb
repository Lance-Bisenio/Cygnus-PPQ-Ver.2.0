Imports denaro

Partial Class Portallogin
    Inherits System.Web.UI.Page
    Dim vSQL As String = ""
    Dim c As New SqlClient.SqlConnection

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        lblError.Visible = False
        txtUsername.Focus()
    End Sub

    Protected Sub txtUsername_TextChanged(sender As Object, e As EventArgs) Handles txtUsername.TextChanged
        EmpLogin()
    End Sub

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        EmpLogin()
    End Sub

    Private Sub EmpLogin()
        Dim vUserId As String = System.Security.SecurityElement.Escape(txtUsername.Text.Trim)
        Dim vIsValid As String = ""

        vSQL = "select Emp_Cd from emp_master where Emp_Cd='" & vUserId & "' and Pos_Cd='6'"
        vIsValid = GetRef(vSQL, "")

        If vIsValid = "" Then
            lblError.Visible = True
            lblError.Text = "Invalid username, please try again."
        Else
            Session("uid") = vUserId
            Response.Redirect("build_operations/production_monitoring.aspx?id=220")
            lblError.Visible = False
        End If
    End Sub

    Private Sub CreateLogs()
        Dim cm As New SqlClient.SqlCommand
		'Dim vLineNum As Integer
		Dim vCol As String = ""
        Dim vData As String = ""

        c.ConnectionString = connStr
        c.Open()
        cm.Connection = c

        vSQL = "insert into user_list (" & vCol & ") values ('" & vData & "')"
        cm.CommandText = vSQL


        'Response.Write(cm.CommandText)
        cm.ExecuteNonQuery()

    End Sub
End Class
