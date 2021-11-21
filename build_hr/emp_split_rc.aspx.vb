Imports denaro.fis
Partial Class emp_split_rc
    Inherits System.Web.UI.Page
    Public vScript As String = ""
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("uid") = "" Then
            vScript = "alert('Your login session has expired. Please re-login again.'); window.close();"
            Exit Sub
        End If

        Dim c As New SqlClient.SqlConnection(connStr)
        Dim cm As New SqlClient.SqlCommand
        Dim rs As SqlClient.SqlDataReader
        Dim iLoop As Integer
        Dim RcList As String = ""
        Dim vList() As String

        Try
            c.Open()
        Catch ex As SqlClient.SqlException
            vScript = "alert('Error occurred while trying to connect to database. Error is: " & _
                ex.Message.Replace(vbCrLf, "").Replace("'", "") & "');"
            c.Dispose()
            cm.Dispose()
            Exit Sub
        End Try

        cm.Connection = c

        If Not IsPostBack Then

            cm.CommandText = "select Rc_Cd from emp_costcenter where Emp_Cd='" & Request.Item("vEmp") & "'"
            'Response.Write(cm.CommandText)
            Try
                rs = cm.ExecuteReader
                Do While rs.Read
                    RcList += "" & rs("Rc_Cd") & ","
                Loop
                rs.Close()

                vList = RcList.Split(",")
                 
                cm.CommandText = "select Rc_Cd,Descr from ref_emp_costcenter order by Descr"
                rs = cm.ExecuteReader
                chkCodes.Items.Clear()
                 
                Do While rs.Read
                    chkCodes.Items.Add(New ListItem(rs(1), rs(0))) 
                Loop
                rs.Close()

                For i = 0 To chkCodes.Items.Count - 1
                    For iLoop = 0 To UBound(vList) - 1
                        If vList(iLoop) = chkCodes.Items(i).Value Then
                            chkCodes.Items(i).Selected = True
                        End If
                    Next 
                Next
                 
            Catch ex As System.Exception
                vScript = "alert('Error occurred while trying to retrieve records. Error is 2 : " & _
                    ex.Message.Replace(vbCrLf, "").Replace("'", "") & "');"
            Finally
                c.Close()
                c.Dispose()
                cm.Dispose()
            End Try
        End If
    End Sub
     
    Protected Sub cmdSelect_Click(sender As Object, e As EventArgs) Handles cmdSelect.Click
        Dim i As Integer

        For i = 0 To chkCodes.Items.Count - 1
            chkCodes.Items(i).Selected = True
        Next
    End Sub

    Protected Sub cmdDeselect_Click(sender As Object, e As EventArgs) Handles cmdDeselect.Click
        Dim i As Integer

        For i = 0 To chkCodes.Items.Count - 1
            chkCodes.Items(i).Selected = False
        Next
    End Sub

    Protected Sub cmdSave_Click(sender As Object, e As EventArgs) Handles cmdSave.Click
         
        Dim i As Integer
        Dim vSelected As String = ""
        Dim c As New SqlClient.SqlConnection(connStr)
        Dim cm As New SqlClient.SqlCommand

        Try
            c.Open()
        Catch ex As SqlClient.SqlException
            vScript = "alert('Error occurred while trying to connect to database. Error is: " & _
                ex.Message.Replace(vbCrLf, "").Replace("'", "") & "');"
            c.Dispose()
            cm.Dispose()
            Exit Sub
        End Try

        cm.Connection = c
        cm.CommandText = "delete from emp_costcenter where Emp_Cd='" & Request.Item("vEmp") & "' "

        Try 
            cm.ExecuteNonQuery()

            For i = 0 To chkCodes.Items.Count - 1
                If chkCodes.Items(i).Selected Then
                    cm.CommandText = "insert into emp_costcenter (Emp_Cd,Rc_Cd,PercentShare,CreatedBy,DateCreated) values ('" & _
                        Request.Item("vEmp") & "', '" & chkCodes.Items(i).Value & "', 0, '" & Session("uid") & "','" & Now & "') "
                    cm.ExecuteNonQuery()
                End If
            Next 
        Catch ex As SqlClient.SqlException
            vScript = "alert('Error occurred while trying to retrieve info. Error is: " & _
                ex.Message.Replace(vbCrLf, "").Replace("'", "") & "');"
        Finally
            c.Close()
            c.Dispose()
            cm.Dispose()
        End Try 
        vScript = "alert('Successfully Save'); window.close();"
    End Sub
End Class
