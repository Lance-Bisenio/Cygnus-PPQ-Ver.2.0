Imports denaro
Partial Class login
    Inherits System.Web.UI.Page
    Dim vSQL As String = ""
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Session.RemoveAll()
        End If

        Txtuser.Focus()

        If Txtuser.Value.Trim <> "" And Txtpwd.Value.Trim <> "" Then
            ValidateUser()
        End If

    End Sub

    Private Sub ValidateUser()
        Dim usrname As String = System.Security.SecurityElement.Escape(Txtuser.Value.Trim)
        Dim passwd As String = System.Security.SecurityElement.Escape(Txtpwd.Value.Trim)

        If usrname <> "" Or passwd <> "" Then
            'System.Security.SecurityElement.Escape(txtU.Value.Trim)

            Dim cn As New SqlClient.SqlConnection
            Dim cm As New SqlClient.SqlCommand
            Dim cmRef As New SqlClient.SqlCommand
            Dim rsRef As SqlClient.SqlDataReader
            Dim rs As SqlClient.SqlDataReader
            cn.ConnectionString = connStr

            Try
                cn.Open()

            Catch ex As SqlClient.SqlException

                cn.Close()
                cn.Dispose()
                cm.Dispose()

                lblError.Text = "System OFFLINE."
                dvError.Visible = True

                Exit Sub

            End Try

            cm.Connection = cn
            cmRef.Connection = cn

            vSQL = "select * from user_list where User_Id='" & usrname & "' and UserPassword='" & passwd & "'"
            cm.CommandText = vSQL

            rs = cm.ExecuteReader
            Try
                If rs.Read Then
                    If rs("UserPassword") = passwd Then

                        Session("caption") = IIf(IsDBNull(rs("Caption")), "", rs("Caption"))
                        Session("userlevel") = IIf(IsDBNull(rs("UserLevel")), 0, rs("UserLevel"))
                        Session("agencylist") = IIf(IsDBNull(rs("AgencyCd")), "", rs("AgencyCd"))
                        Session("uid") = usrname
                        Session("EmpPos") = IIf(IsDBNull(rs("Position")), "", "Position : " & rs("Position"))
                        Session("EmpFullName") = IIf(IsDBNull(rs("FullName")), "", rs("FullName"))
                        Session("sessionid") = Session.SessionID
                        Session("caption") = IIf(IsDBNull(rs("Caption")), "", rs("Caption"))
                        Session("userlevel") = IIf(IsDBNull(rs("UserLevel")), 0, rs("UserLevel"))
                        Session("agencylist") = IIf(IsDBNull(rs("AgencyCd")), "", rs("AgencyCd"))
                        Session("uid") = usrname
                        Session("EmpPos") = IIf(IsDBNull(rs("Position")), "", "Position : " & rs("Position"))
                        Session("EmpFullName") = IIf(IsDBNull(rs("FullName")), "", rs("FullName"))
                        Session("sessionid") = Session.SessionID

                        Session("sessionid") = Session("sessionid").ToString.Substring(Session("sessionid").ToString.IndexOf("=") + 1)

                        EventLog(Session("uid"), Request.ServerVariables("REMOTE_ADDR"), "LOGIN",
                                    "", "", "Successful LogIn on " & Format(Now, "yyyy/MM/dd HH:mm:ss"), "LOGIN")

                        Response.Redirect("evolvemenus.aspx")
                    Else
                        EventLog(usrname, Request.ServerVariables("REMOTE_ADDR"), "LOGIN",
                                    "", "", "Invalid LogIn Password Attempt on " & Format(Now, "yyyy/MM/dd HH:mm:ss"), "LOGIN")
                        lblError.Text = "Supply the correct Employee Code and Password to access your account."
                        dvError.Visible = True
                        Txtpwd.Value = ""
                        Txtuser.Value = ""
                    End If
                Else
                    EventLog(usrname, Request.ServerVariables("REMOTE_ADDR"), "LOGIN",
                                   "", "", "Invalid LogIn Password Attempt on " & Format(Now, "yyyy/MM/dd HH:mm:ss"), "LOGIN")
                    lblError.Text = "Supply the correct Employee Code and Password to access your account."
                    dvError.Visible = True
                    Txtpwd.Value = ""
                    Txtuser.Value = ""
                End If
                rs.Close()

            Catch ex As Exception

                lblError.Text = "Error is : " & ex.Message.Replace(vbCrLf, "").Replace("'", "") & " "
                dvError.Visible = True
            End Try

            cn.Close()
            cn.Dispose()
            cm.Dispose()

        Else
            lblError.Text = "Supply the correct Employee Code and Password to access your account."
            dvError.Visible = True

        End If

        Txtuser.Focus()
        Txtpwd.Value = ""
        Txtuser.Value = ""
    End Sub
    Private Sub Cmdlog_ServerClick(sender As Object, e As EventArgs) Handles Cmdlog.ServerClick
        ValidateUser()
    End Sub

End Class
