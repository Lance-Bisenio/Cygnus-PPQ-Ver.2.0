
Imports denaro.fis
Partial Class modifyuser
    Inherits System.Web.UI.Page
    Dim c As New sqlclient.sqlconnection
    Public vScript As String = "document.getElementById('txtUserId').focus();"
    Protected Sub cmdCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        Session.Remove("mode")
        Session.Remove("user_id")
        Server.Transfer("upm.aspx")
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("uid") = "" Then
            Server.Transfer("login.aspx")
            Exit Sub
        End If

        If Not IsPostBack Then

            BuildCombo("select Pos_Cd, Descr from ref_emp_position order by Descr ", cmbPosition)

            'cmbPosition.Items.Add(" ")
            'cmbPosition.SelectedValue = " "

            'lblCaption.Text = "Add/Modify User"
            If Session("mode") = "e" Then 'edit mode
                Dim cm As New SqlClient.SqlCommand
                Dim dr As SqlClient.SqlDataReader
                Dim vSQL As String = "select * from user_list where User_Id='" & Session("user_id") & "'"

                c.ConnectionString = connStr
                c.Open()
                cm.Connection = c

                'Response.Write(vSQL)
                cm.CommandText = vSQL
                dr = cm.ExecuteReader
                If dr.Read Then
                     
                    txtUserId.Text = dr("User_Id")
                    txtPwd.Attributes("value") = dr("UserPassword")
                    txtRetype.Attributes("value") = dr("UserPassword")

                    txtFullName.Text = dr("FullName")
                    cmbPosition.SelectedValue = dr("Position")

                    txtEmail.Text = IIf(IsDBNull(dr("Email")), "", dr("Email"))
                    cmbEmpStatus.SelectedValue = IIf(IsDBNull(dr("POSMenus")), "", dr("POSMenus"))

                    txtLineNum.Text = IIf(IsDBNull(dr("LineUp")), "", dr("LineUp"))

                    Session("oldval") = "User_Id=" & txtUserId.Text & _
                        "|FullName=" & txtFullName.Text & _
                        "|Position=" & cmbPosition.SelectedValue & _
                        "|POSMenus=" & cmbEmpStatus.SelectedValue
                End If
                dr.Close()
                cm.Dispose()
                c.Close()
            End If
        End If
    End Sub

    Protected Sub cmdSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Dim cm As New sqlclient.sqlcommand
        Dim vType As String = "ADD"
        Dim vLineNum As Integer

        c.ConnectionString = connStr
        c.Open()
        cm.Connection = c

        If txtLineNum.Text <> "" Then
            txtLineNum.Text.Replace(",", "")
            vLineNum = txtLineNum.Text
            If Not IsNumeric(txtLineNum.Text) Then
                vScript = "alert('Please enter valid for Line Number.')"
                txtLineNum.Text = ""
                Exit Sub
            End If
        Else
            vLineNum = IIf(txtLineNum.Text = "", 0, txtLineNum.Text)
        End If
        
        If Session("mode") = "a" Then        'add mode
        
            cm.CommandText = "insert into user_list (User_Id,FullName,Position,Email,POSMenus,LineUp,UserPassword) values ('" & _
                CleanVar(txtUserId.Text) & "','" & CleanVar(txtFullName.Text) & "','" & cmbPosition.SelectedValue & _
                "','" & CleanVar(txtEmail.Text) & "','" & cmbEmpStatus.SelectedValue & "'," & vLineNum & ",'" & txtPwd.Text & "')"

            Session("oldval") = ""
            Session("newval") = "User_Id=" & txtUserId.Text & _
                "|FullName=" & txtFullName.Text & _
                "|Position=" & cmbPosition.SelectedValue & _
                "|Email=" & txtEmail.Text & _
                "|POSMenus=" & cmbEmpStatus.SelectedValue
        Else 'edit mode
            vType = "EDIT"
            cm.CommandText = "update user_list set User_Id='" & CleanVar(txtUserId.Text) & _
                "',FullName='" & CleanVar(txtFullName.Text) & _
                "',Position='" & cmbPosition.SelectedValue & _
                "',POSMenus='" & cmbEmpStatus.SelectedValue & _
                "',Email='" & CleanVar(txtEmail.Text) & _
                "',LineUp=" & vLineNum & ",UserPassword='" & txtPwd.Text & "'" & _
                " where User_Id='" & Session("user_id") & "'"



            Session("newval") = "User_Id=" & CleanVar(txtUserId.Text) & _
                "|FullName=" & CleanVar(txtFullName.Text) & _
                "|Position=" & cmbPosition.SelectedValue & _
                "|Email=" & CleanVar(txtEmail.Text) & _
                "|POSMenus=" & cmbEmpStatus.SelectedValue
        End If
        'Response.Write(cm.CommandText)
        cm.ExecuteNonQuery()
        EventLog(Session("uid"), Request.ServerVariables("REMOTE_ADDR"), vType, Session("oldval"), Session("newval"), "User Profile of " & txtUserId.Text, "User Profiles")
        vScript = "alert('Record was successfully saved.');"
        Server.Transfer("upm.aspx")

    End Sub

    Protected Sub cmdSave_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSave.Init
        cmdSave.Attributes.Add("onclick", "return postvalidate();")
    End Sub
     

    Protected Sub cmbFind_Click(sender As Object, e As EventArgs) Handles cmbFind.Click
        Dim cm As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader


        Dim vSQL As String = "select * from emp_master where Emp_Cd='" & txtUserId.Text.Trim & "'"

        c.ConnectionString = connStr
        c.Open()
        cm.Connection = c

        'Response.Write(vSQL)

        cm.CommandText = vSQL
        dr = cm.ExecuteReader
        If dr.Read Then

            txtUserId.Text = dr("Emp_Cd")
            txtFullName.Text = dr("Emp_LName") & ", " & dr("Emp_FName") 
            cmbPosition.SelectedValue = dr("Pos_Cd") 

        End If
        dr.Close()
        cm.Dispose()
        c.Close()
    End Sub
End Class
