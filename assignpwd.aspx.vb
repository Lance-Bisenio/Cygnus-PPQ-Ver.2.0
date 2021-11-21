Imports denaro
Partial Class assignpwd
    Inherits System.Web.UI.Page
    Dim c As New sqlclient.sqlconnection
    Public vScript As String = ""
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("uid") = "" Then
            Server.Transfer("login.aspx")
            Exit Sub
        End If
        If Not IsPostBack Then
            txtTitle.Text = "Assign Password"
            Dim cm As New SqlClient.SqlCommand
            Dim dr As SqlClient.SqlDataReader

            c.ConnectionString = connStr
            c.Open()
            cm.Connection = c
            cm.CommandText = "select UserPassword from user_list where User_Id='" & Session("user_id") & "'"
            dr = cm.ExecuteReader
            If dr.Read Then
                txtPwd.Text = ""
                If Not IsDBNull(dr("UserPassword")) Then
                    txtPwd.Text = dr("UserPassword")
                End If
                txtRetype.Text = ""
            End If
            dr.Close()
            cm.Dispose()
            c.Close()
            'lblCaption.Text = "Assign password for " & Session("user_id")
        End If
    End Sub

    Protected Sub cmdReturn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdReturn.Click
        Session.Remove("user_id")
    End Sub

    Protected Sub cmdSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        If Page.IsValid Then
            Dim cm As New sqlclient.sqlcommand

            c.ConnectionString = connStr
            c.Open()
            cm.Connection = c
            cm.CommandText = "update user_list set UserPassword='" & txtPwd.Text & _
                "' where User_Id='" & Session("user_id") & "'"
            cm.ExecuteNonQuery()
            cm.Dispose()
            c.Close()

            vScript = "alert('New password has been set.');"
            EventLog(Session("uid"), Request.ServerVariables("REMOTE_ADDR"), "ASSIGN PASSWORD", "", "", "User Profile of " & Session("user_id"), "User Profiles")
        End If
    End Sub

    Protected Sub vldPwd_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles vldPwd.ServerValidate
        If txtPwd.Text <> txtRetype.Text Then
            vScript = "alert('Fields do not match!');"
            args.IsValid = False
        End If
    End Sub

    Protected Sub cmdReturn_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdReturn.Init
        cmdReturn.Attributes.Add("onclick", "window.close();")
    End Sub
End Class
