Namespace denaro

    Partial Class edit
        Inherits System.Web.UI.Page
        Public vScript As String = ""
        Public vDetail As String = ""
        Dim vRef As String
        Dim vSearchKey As String
        Dim vSearchType As String
        Dim vReturnVal As String
        Dim vSQl As String
        Dim vTableName As String
        Dim vSortBy As String
        Dim vColFormat As String
        Dim vColTitle As String
        Dim vColFields As String
        Dim vDataType As String
        Dim vMode As String
        Dim vRec As String
        Dim vFields() As String
        Dim vTitle() As String
        Dim vTypes() As String

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

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            If Session("uid") = "" Then
                vScript = "alert('You login session has expired. Please login again.'); window.close();"
                Exit Sub
            End If

            Dim c As New SqlClient.SqlConnection(connStr)
            Dim rs As SqlClient.SqlDataReader
            Dim cm As New SqlClient.SqlCommand

            vRef = Request.Item("value")
            vMode = Request.Item("mode")
            vRec = Request.Item("id")

            c.Open()
            cm.Connection = c 

            cm.CommandText = "select Label_Caption,SearchKey,SearchType,Return_Val,SqlCmd,SortBy,Colformat," & _
                "ColTitle,Colfields,Dependencies,RunProg, ModuleTitle from menu_rights where SystemName='" & SYSTEMNAME & _
                "' and Menu_Caption='" & vRec & "'"
            'Response.Write(cm.CommandText)

            rs = cm.ExecuteReader
            If rs.Read Then
                txtTitle.Text = "Table Maintenance for " & rs("ModuleTitle")
                vSearchKey = IIf(IsDBNull(rs("SearchKey")), "", rs("SearchKey"))
                vSearchType = IIf(IsDBNull(rs("SearchType")), "", rs("SearchType"))
                vReturnVal = IIf(IsDBNull(rs("Return_Val")), "", rs("Return_Val"))
                vSQl = IIf(IsDBNull(rs("SqlCmd")), "", rs("SqlCmd"))
                vTableName = IIf(IsDBNull(rs("Dependencies")), "", rs("Dependencies"))
                vSortBy = IIf(IsDBNull(rs("SortBy")), "", rs("SortBy"))
                vColFormat = IIf(IsDBNull(rs("ColFormat")), "", rs("ColFormat"))
                vColTitle = IIf(IsDBNull(rs("ColTitle")), "", rs("ColTitle"))
                vColFields = IIf(IsDBNull(rs("ColFields")), "", rs("ColFields"))
                vDataType = IIf(IsDBNull(rs("RunProg")), "", rs("RunProg")) 

                If vColTitle <> "" Then
                    vTitle = vColTitle.Split("|")
                    vFields = vColFields.Split("|")
                    vTypes = vDataType.Split("|")
                End If
            End If
            rs.Close()
            cm.Dispose()
            c.Close()
            c.Dispose()

            SetupDataEntryFields()

        End Sub
        Private Sub SetupDataEntryFields()
            Dim iCtr As Integer
            Dim c As New SqlClient.SqlConnection
            Dim cm As New SqlClient.SqlCommand
            Dim cmRef As New sqlclient.sqlCommand
            Dim cmZoom As New sqlclient.sqlCommand
            Dim rs As sqlclient.sqlDataReader
            Dim rsRef As sqlclient.sqlDataReader
            Dim rsZoom As sqlclient.sqlDataReader
            Dim vValue As String
            Dim vControl As String
            Dim vOldValue As String = ""
            c.ConnectionString = connStr
            c.Open()
            cm.Connection = c
            cmRef.Connection = c
            cmZoom.Connection = c
             
            If vMode = "e" Then
                cm.CommandText = vSQl & " where " & vReturnVal & "='" & vRef & "'"
                'Response.Write(cm.CommandText)
                rs = cm.ExecuteReader
                rs.Read()

                'vOldValue
            End If

            Try
                

                vDetail = "<table border='0' class='' style='width:100%;'>"

                For iCtr = 0 To vTitle.Length - 1

                    Response.Write(rs(vFields(iCtr)))

                    If vFields(iCtr).Trim = "(select Descr from coa_type where CoaType_Cd=TranType)" Then
                        vFields(iCtr) = "TranType"
                    End If

                    If vMode = "e" Then

                        If IsDBNull(rs(vFields(iCtr))) Then
                            vValue = ""
                        Else
                            vValue = rs(vFields(iCtr))
                        End If
                    Else
                        vValue = ""
                    End If

                    cmZoom.CommandText = "select * from zoom_ref where TagName='" & vRec & "' and FieldName='" & _
                       vFields(iCtr) & "'"
                    'Response.Write(cmZoom.CommandText & "<br>")
                    rsZoom = cmZoom.ExecuteReader

                    If Not rsZoom.Read Then
                        vControl = "<input type='text' name='t" & vFields(iCtr) & "' value='" & vValue & "' style='width:100%;'>"
                    Else
                        vControl = "<select size='1' name='t" & vFields(iCtr) & "' style='width:100%;' class='labelL'>"
                        cmRef.CommandText = rsZoom("SqlCmd")
                        rsRef = cmRef.ExecuteReader
                        Do While rsRef.Read
                            vControl += "<option value='" & rsRef(rsZoom("ReturnField")) & "' " & _
                               IIf(rsRef(rsZoom("ReturnField")) = vValue, " SELECTED ", "") & ">" & _
                               rsRef(rsZoom("SearchKey")) & "</option>"
                        Loop
                        'vControl += "<option value=''></option>"
                        vControl += "</select>"
                        rsRef.Close()
                    End If
                    rsZoom.Close()

                    vDetail += "<tr><td class='labelR' style='width:120px;'>" & vTitle(iCtr) & " :</td>" & _
                       "<td class='labelL' style=''>" & vControl & "</td></tr>"
                Next iCtr

                vDetail += "</table>"
                If vMode = "e" Then rs.Close()

            Catch ex As Exception
                vScript = "alert('An error occurred while trying to Save the new record.');"
            End Try



            cmRef.Dispose()
            cmZoom.Dispose()
            cm.Dispose()
            c.Close()
            c.Dispose()
            'cRef.Close()
            'cZoom.Close()
        End Sub

        Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
            Dim iCtr As Integer
            Dim c As New sqlclient.sqlConnection
            Dim cm As New sqlclient.sqlCommand
            Dim vStr As String
            Dim vValue As String

            If vMode = "a" Then   'add mode
                vStr = "insert into " & vTableName & " ("
                For iCtr = 0 To vFields.Length - 1
                    vStr += vFields(iCtr) & ","
                Next iCtr
                vStr = vStr.Substring(0, vStr.Length - 1) & ") values ("
                For iCtr = 0 To vFields.Length - 1
                    Select Case vTypes(iCtr)
                        Case "9"
                            vStr += Val(Request.Form("T" & vFields(iCtr))) & ","
                        Case "d"
                            vValue = Request.Form("T" & vFields(iCtr))
                            If vValue = "" Or vValue = Nothing Then
                                vValue = "null"
                            Else
                                vValue = "'" & Format(CDate(Request.Form("T" & vFields(iCtr))), "yyyy/MM/dd") & "'"
                            End If
                            vStr += vValue & ","
                        Case "h"
                            vValue = Request.Form("T" & vFields(iCtr))
                            If vValue = "" Or vValue = Nothing Then
                                vValue = "null"
                            Else
                                vValue = "'" & Format(CDate(Request.Form("T" & vFields(iCtr))), "HH:mm:ss") & "'"
                            End If
                            vStr += vValue & ","
                        Case Else   '"x"
                            vStr += "'" & Request.Form("T" & vFields(iCtr)).Replace("'", "''") & "',"
                    End Select
                Next iCtr
                vStr = vStr.Substring(0, vStr.Length - 1) & ")"
            Else                  'edit mode
                vStr = "update " & vTableName & " set "
                For iCtr = 0 To vFields.Length - 1
                    Select Case vTypes(iCtr)
                        Case "9"
                            vStr += vFields(iCtr) & "=" & Val(Request.Form("T" & vFields(iCtr))) & ","
                        Case "d"
                            vValue = Request.Form("T" & vFields(iCtr))
                            If vValue = "" Or vValue = Nothing Then
                                vValue = "null"
                            Else
                                vValue = "'" & Format(CDate(Request.Form("T" & vFields(iCtr))), "yyyy/MM/dd") & "'"
                            End If
                            vStr += vFields(iCtr) & "=" & vValue & ","
                        Case "h"
                            vValue = Request.Form("T" & vFields(iCtr))
                            If vValue = "" Or vValue = Nothing Then
                                vValue = "null"
                            Else
                                vValue = "'" & Format(CDate(Request.Form("T" & vFields(iCtr))), "HH:mm:ss") & "'"
                            End If
                            vStr += vFields(iCtr) & "=" & vValue & ","
                        Case Else   '="x"
                            vStr += vFields(iCtr) & "='" & Request.Form("T" & vFields(iCtr)).Replace("'", "''") & "',"
                    End Select
                Next iCtr
                vStr = vStr.Substring(0, vStr.Length - 1) & " where " & vReturnVal & "='" & vRef & "'"
            End If
            Try
                c.ConnectionString = connStr
                c.Open()
                cm.Connection = c
                cm.CommandText = vStr

                cm.ExecuteNonQuery()
                c.Close()
                c.Dispose()
            Catch ex As sqlclient.sqlException
                vScript = "alert('An error occurred while try to execute your request. Error is: " & _
                    ex.Message.Replace("'", "") & "');"
                Exit Sub
            End Try

            If vMode = "e" Then
                vScript = "alert('Changes were successfully saved.'); window.opener.document.form1.submit(); window.close();"
            Else
                vScript = "alert('New record successfully saved.');window.opener.document.form1.submit();"
            End If
        End Sub

    End Class

End Namespace
