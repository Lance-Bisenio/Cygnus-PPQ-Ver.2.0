Imports denaro
Partial Class entity_maintenance
    Inherits System.Web.UI.Page
    Public vScript As String = ""
    Public vEntityTitle As String = ""
    Public vEntity As String = ""

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            GetEntityProperties()
        End If
    End Sub

    Private Sub GetEntityProperties()
        Dim c As New SqlClient.SqlConnection(connStr)
        Dim cm As New SqlClient.SqlCommand
        Dim rs As SqlClient.SqlDataReader

        Try
            c.Open()
        Catch ex As SqlClient.SqlException
            vScript = "alert('Error occurred while trying to connect to database. Error code 101; Error is: " & ex.Message.Replace(vbCrLf, "").Replace("'", "") & "');"
            c.Dispose()
            cm.Dispose()
            Exit Sub
        End Try
        cm.Connection = c
        cm.CommandText = "select * from entity_properties_hdr where EntityCode='" & Request.Item("id") & "' and Published='YES'"
        Try
            rs = cm.ExecuteReader
            If rs.Read Then
                vEntityTitle = rs("EntityTitle")
                vScript = "window.resizeTo(" & rs("EntityWidth") & ", " & rs("EntityHieght") & "); window.focus();"
            End If
            rs.Close() 

            cm.CommandText = "select * from entity_properties_dtl where EntityCode='" & Request.Item("id") & "' and Published='YES'"
            rs = cm.ExecuteReader
            Do While rs.Read

                vEntity += "<tr><td style='Width: 120px;' class='labelR'>" & rs("FieldLabel") & " :</td><td class='labelL'>"
                Select Case rs("FieldType")
                    Case "TextField"
                        vEntity += "<input Type='Text' style='Width: " & rs("FieldWidth") & "px;' " & rs("ReadOnly") & " />"

                    Case "SelectList"
                        vEntity += "<select name='" & rs("FieldCode") & "' style='Width: " & rs("FieldWidth") & "px;'>" & _
                            "<option selected='selected'></option></select>"

                    Case "RadioButtons"
                        vEntity += "<input name='" & rs("FieldCode") & "' type='radio' name='q12_3' value=''>Yes" & _
                            "<input type='radio' name='" & rs("FieldCode") & "' name='q12_3' value=''>No"
                    Case "Image"
                        vEntity += "<input type='file' style='Width: " & rs("FieldWidth") & "px;' name='img'>"
                End Select

                vEntity += "</td>"
            Loop
            rs.Close()


        Catch ex As SqlClient.SqlException
            vScript = "alert('Error occurred while trying to retrieve Budget Info. Error code 102; Error is: " & _
                ex.Message.Replace(vbCrLf, "").Replace("'", "") & "');" 
        End Try
        c.Close()
        c.Dispose()
        cm.Dispose()
    End Sub
End Class
