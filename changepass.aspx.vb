Namespace denaro
    Partial Class ChangePass
        Inherits System.Web.UI.Page
        Public vScript As String = ""
        Dim vPwd As String
        Dim c As New sqlclient.sqlconnection
#Region " Web Form Designer Generated Code "

        'This call is required by the Web Form Designer.
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

        End Sub


        Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
            'CODEGEN: This method call is required by the Web Form Designer
            'Do not modify it using the code editor.
            InitializeComponent()
        End Sub

#End Region

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load, Me.Load
            If Session("uid") = "" Then
                Session("returnaddr") = "changepass.aspx"
                Server.Transfer("index.aspx")
            End If
            If Not IsPostBack Then
                'lblScrName.Text = "Change Password"
                txtUserid.Text = Session("uid")
                cmdReturn.Enabled = False

            End If
        End Sub

        Private Sub vldCheck_ServerValidate(ByVal source As System.Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles vldCheck.ServerValidate

            If txtNew.Text <> txtConfirm.Text Then
                vldCheck.ErrorMessage = "Your New Password does not match with your confirmation password. " & _
                   "Please try again."
                vScript = "alert('Your new password does not match with your confirmation password. Please try again.');"
                args.IsValid = False
                Exit Sub
            End If

            Dim cm As New sqlclient.sqlcommand
            Dim rs As sqlclient.sqldatareader

            c.ConnectionString = connStr
            c.Open()
            cm.Connection = c

            'CHECK USER FROM USER_LIST TABLE
            cm.CommandText = "select UserPassword from user_list where User_Id='" & txtUserid.Text & "'"
            rs = cm.ExecuteReader
            If rs.Read Then
                vPwd = rs("UserPassword")
            Else
                vldCheck.ErrorMessage = "You are an invalid user. Please try again."
                vScript = "alert('You are an invalid user. Please try again.');"
                rs.Close()
                cm.Dispose()
                c.Close()
                args.IsValid = False
                Exit Sub
            End If
            rs.Close()
            If txtPwd.Text <> vPwd Then
                vldCheck.ErrorMessage = "Your current password is invalid. Please try again."
                vScript = "alert('Your current password is invalid. Please try again.');"
                cm.Dispose()
                c.Close()
                args.IsValid = False
                Exit Sub
            End If

            If txtNew.Text.Length < 6 Or txtConfirm.Text.Length < 6 Then
                vldCheck.ErrorMessage = "Password should be atleast 6 characters."
                vScript = "alert('Password should be atleast 6 characters.');"
                cm.Dispose()
                c.Close()
                args.IsValid = False
                Exit Sub
            End If

            cm.CommandText = "update User_List set UserPassword='" & txtNew.Text & "' where User_Id='" & txtUserid.Text & "'"
            cm.ExecuteNonQuery()
            cm.Dispose()
            c.Close()
            vScript = "alert('Your password was changed successfully.');"
            cmdReturn.Enabled = True
            args.IsValid = True
        End Sub

        Private Sub cmdReturn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdReturn.Click
            'Server.Transfer("main.aspx")
            Server.Transfer("index.aspx")
        End Sub

        Private Sub cmdChange_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdChange.Click
            If Page.IsValid Then
                cmdChange.Enabled = False
            End If
        End Sub
    End Class

End Namespace
