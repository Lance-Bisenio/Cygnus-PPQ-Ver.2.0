Imports denaro.fis
Partial Class subattachment
    Inherits System.Web.UI.Page
    Public mysubattachments As String = ""
    Public vscript As String = ""
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
       

        If Session("uid") = "" Then
            vscript = "alert('Login session has expired. Please re-login again.'); window.opener.document.form1.submit(); window.close();"
            Exit Sub
        End If

        If Not IsPostBack Then
            datarefresh()
        End If

        If Text1.Value <> "" Then
            Dim c As New SqlClient.SqlConnection(connStr)
            Dim cm As New SqlClient.SqlCommand
            Dim cmref As New SqlClient.SqlCommand
            Dim rs As SqlClient.SqlDataReader
            Dim vfiletoupload As String = ""
            Dim vFilename As String = ""
            Dim vattachtodelete() As String
            Dim i As Integer = 0

            cm.Connection = c
            cm.CommandText = "select ItemImages from item_master where Item_Cd='" & Request.Item("pCode") & "'"
            Try
                c.Open()
                rs = cm.ExecuteReader
                If rs.Read Then
                    If Not IsDBNull(rs("ItemImages")) Then
                        vattachtodelete = rs("ItemImages").ToString.Split("|")
                        For i = 0 To UBound(vattachtodelete) - 1
                            If i = Text1.Value Then
                                vfiletoupload = rs("ItemImages").ToString.Replace(vattachtodelete(i) & "|", "")

                                'Response.Write(vfiletoupload)
                                cmref.Connection = c
                                cmref.CommandText = "update item_master set ItemImages='" & vfiletoupload & _
                                                 "' where Item_Cd='" & Request.Item("pCode") & "'"

                                cmref.ExecuteNonQuery()
                                cmref.Dispose()
                                ''To delete the file in the folder
                                vscript = "window.opener.document.form1.submit();"
                                Exit For
                            End If
                        Next i
                    End If
                End If
                rs.Close()
            Catch ex As SqlClient.SqlException
                vscript = "alert('An error occurred while tryin to remove the attachment.  Error is " & _
                          ex.Message.Replace(vbCrLf, "").Replace("'", "") & "');"
            Finally
                Text1.Value = ""
                c.Close()
                c.Dispose()
                cm.Dispose()
            End Try
            datarefresh()
        End If
        
    End Sub
     

    Private Sub datarefresh()
        Dim c As New SqlClient.SqlConnection(connStr)
        Dim cm As New SqlClient.SqlCommand
        Dim rs As SqlClient.SqlDataReader
        Dim attachmentcol() As String
        Dim i As Integer = 0

        cm.Connection = c
        cm.CommandText = "select ItemImages from item_master where Item_Cd='" & Request.Item("pCode") & "'"
        Try
            c.Open()
            rs = cm.ExecuteReader
            If rs.Read Then
                If Not IsDBNull(rs("ItemImages")) Then
                    attachmentcol = rs("ItemImages").ToString.Split("|")
                    mysubattachments = "<table style='width:100%;' border='0'>"
                    For i = 0 To UBound(attachmentcol) - 1
                        mysubattachments += "<tr><td class='labelL' valign='top'>" & attachmentcol(i) & "</td>" & _
                                                "<td style='width:10%;' class='labelL'><a class='textLinks' href='javascript:remove(" & i & ");'>Remove</a></td></tr>"

                    Next i
                    mysubattachments += "</table>"
                End If
            End If
            rs.Close()
        Catch ex As SqlClient.SqlException
            vscript = "alert('An error occurred while trying to access the database.  Error is:" & _
                     ex.Message.Remove(vbCrLf, "").Replace("'", "") & "');"
        Finally
            c.Close()
            c.Dispose()
            cm.Dispose()
        End Try
    End Sub

    Protected Sub cmdClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdClose.Click
        vscript = "window.close();"
    End Sub

    Protected Sub cmdSave_Click(sender As Object, e As EventArgs) Handles cmdSave.Click
        Dim vfile As String = ""
        If fileuploader.FileName = "" Then
            vscript = "alert('No file to upload');"
            Exit Sub
        End If

        Dim c As New SqlClient.SqlConnection(connStr)
        Dim cm As New SqlClient.SqlCommand
        Dim rs As SqlClient.SqlDataReader
        Dim vTargetFilename As String = ""

        cm.Connection = c
        cm.CommandText = "select ItemImages from item_master where Item_Cd='" & Request.Item("pCode") & "'"
        Try
            c.Open()
            rs = cm.ExecuteReader
            If rs.Read Then
                vfile = IIf(IsDBNull(rs("ItemImages")), "", rs("ItemImages"))
            End If
            rs.Close()
        Catch ex As SqlClient.SqlException
            vscript = "alert('An error occurred while trying to access the database.  Error is " & _
                    ex.Message.Replace(vbCrLf, "").Replace("'", "") & "');"
            c.Close()
            c.Dispose()
            cm.Dispose()
            Exit Sub
        End Try

        cm.CommandText = "update item_master set ItemImages='" & _
                         IIf(vfile <> "", vfile & Request.Item("Doc_Id") & "_" & fileuploader.FileName, Request.Item("pCode") & "_" & fileuploader.FileName) & _
                         "|' where Item_Cd='" & Request.Item("pCode") & "'"
        Try
            cm.ExecuteNonQuery()

            vTargetFilename = Server.MapPath(".") & "\..\downloads\itemimages\" & Request.Item("pCode") & "_" & fileuploader.FileName
            fileuploader.SaveAs(vTargetFilename)

        Catch ex As SqlClient.SqlException
            vscript = "alert('An error occurred while trying to upload the attachment.  Error is " & _
                     ex.Message.Replace(vbCrLf, "").Replace("'", "") & "');"
        Finally
            c.Close()
            c.Dispose()
            cm.Dispose()
        End Try
        datarefresh()
        vscript = "alert('Successfully saved.'); window.opener.document.form1.h_Mode.value='reload'; window.opener.document.form1.submit(); window.close();"
        'vscript = "window.opener.document.form1.submit();"; 
    End Sub
End Class
