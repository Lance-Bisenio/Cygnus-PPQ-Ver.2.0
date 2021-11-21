Imports denaro
Partial Class global_forms_edit
    Inherits System.Web.UI.Page
    Public BuildForm As String = ""
    Public vScript As String = ""

    Public vEntityId As String = ""
    Public vColHeader As String = ""
    Public vEmpRecords As String = ""
    Public vFilter As String = ""
    Dim vNewValue As String = ""

    Dim c As New SqlClient.SqlConnection
    Dim vColNames As String = ""
    Dim vColSource As String = ""
    Dim vTableSource As String = ""

    ' ==================================================================================================================================================================
    ' pMode = value (new or edit)
    ' pWinType = value (Popup or colorbox)
    ' id = value (module id) 
    ' ==================================================================================================================================================================

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Session("uid") = "" Then
            vScript = "alert('Login session has expired. Please re-login again.'); window.opener.document.form1.h_Mode.value='reload'; window.opener.document.form1.submit(); window.close();"
            Exit Sub
        End If

        If Request.Item("pCode") = "" And Request.Item("pMode") = "edit" Then
            vScript = "alert('No selected Item');  window.close();"
            Exit Sub
        End If

        If Not IsPostBack Then
            If Request.Item("pWinType") = "Popup" Then
                cmdCancel.Visible = True 
            End If
            GetTable_Properties()
        End If

        'Response.Write(Request.Item("pCode") & " - lance test2")
        'Response.Write(Request.Item("pMode") & " - lance test1")

    End Sub

    Private Sub GetTable_Properties()
        Dim c As New SqlClient.SqlConnection(connStr)
        Dim cm As New SqlClient.SqlCommand
        Dim cmsub As New SqlClient.SqlCommand
        Dim rs As SqlClient.SqlDataReader
        Dim vCurrVal As String = ""
        Dim vKeyTable As String = ""

        Dim vColName As String = ""
        Dim vSQL As String = ""

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
        cmsub.Connection = c

        ' ============================================================================================================================================================================
        ' == 08.07.2014 == GET THE MASTER TABLE PROPERTIES ===========================================================================================================================
        ' ============================================================================================================================================================================
        cm.CommandText = "select * from table_properties_hdr where ModuleCode='" & Request.Item("id") & "' and Published='YES' "
        'Response.Write(cm.CommandText)
        rs = cm.ExecuteReader
        If rs.Read Then
            If Not IsDBNull(rs("DboTable")) Then
                vTableSource = rs("DboTable")
            End If
        End If
        rs.Close()

        ' ============================================================================================================================================================================
        ' == 08.07.2014 == GET THE TABLE PROPERTIES AND DETAILS WHEN BUILD A WEBFORM =================================================================================================
        ' ============================================================================================================================================================================
        If vTableSource <> "" Then
            cm.CommandText = "select * from table_properties_dtl where ModuleCode='" & Request.Item("id") & "' and Published='YES' and ColType<>'GRIDVIEW' order by ColCode"
            'Response.Write(cm.CommandText)
            Try
                rs = cm.ExecuteReader
                Do While rs.Read
                     
                    If rs("ColType") = "SPACE" Then
                        BuildForm += "<tr><td></td><td>"
                        BuildForm += "&nbsp;"
                    Else
                        BuildForm += "<tr><td>" & rs("ColTitle") & " :</td><td>"
                        If Request.Item("pMode") = "edit" Then
                            vSQL = "select " & rs("ColSource") & " from " & vTableSource & " where " & rs("ColReturnValue") & "='" & Request.Item("pCode") & "'"
                            vCurrVal = GetRef(vSQL, "")
                            'Response.Write(vSQL)
                        End If
                        ' =================================================================================================================================================================
                        ' == 08.07.2014 == BUILD LABEL AND HTML TAGS ======================================================================================================================
                        ' =================================================================================================================================================================
                        Select Case rs("ColType")
                            Case "TEXT"
                                BuildForm += "<input type='text' name='" & rs("ColSource") & "' value='" & vCurrVal & "' style='width:150px;' class='labelL' />"
                            Case "TEXTDESCR"
                                BuildForm += "<input type='text' name='" & rs("ColSource") & "' value='" & vCurrVal & "' style='width:480px;' class='labelL' />"
                            Case "DROPLIST"
                                BuildSubListbox(Request.Item("id"), rs("ColReturnValue"), rs("ColSource"), vCurrVal)
                            Case "DATE"
                                vCurrVal = IIf(vCurrVal.ToString = "null", "", vCurrVal)
                                BuildForm += "<input type='text' id='" & rs("ColSource") & "' name='" & rs("ColSource") & "' value='" & vCurrVal & "' style='width:150px;' class='labelL' /> " & _
                                    "<img src='../images/calendar.png' style='vertical-align:middle;' alt='' /> "
                                vScript = "$('#" & rs("ColSource") & "').datepicker();"


                        End Select
                         
                        vKeyTable = rs("ColReturnValue")
                        vColName += rs("ColSource") & ","

                        If h_mode.Value = "save" Then
                            If Request.Item(rs("ColSource")).ToString = "" Then
                                vNewValue += "NULL,"
                            Else
                                vNewValue += "'" & Request.Item(rs("ColSource")) & "',"
                            End If

                        Else
                            vNewValue += "'" & vCurrVal & "',"
                        End If
                    End If

                    BuildForm += "</td></tr>"
                Loop
                rs.Close()
                 
                ' ============================================================================================================================================================================
                ' == 08.07.2014 == SAVE THE COLLECTED INFORMATION BASE ON TABLE PROPERTIES ===================================================================================================
                ' ============================================================================================================================================================================
                If h_mode.Value = "save" Then 
                    vColName = Mid(vColName, 1, Len(vColName) - 1)
                    vNewValue = Mid(vNewValue, 1, Len(vNewValue) - 1)

                    If Request.Item("pCode") <> "" Then
                        cmsub.CommandText = "delete from " & vTableSource & " where " & vKeyTable & "='" & Request.Item("pCode") & "'"
                        cmsub.ExecuteNonQuery()
                        'Response.Write(cmsub.CommandText)
                    End If
                     
                    cmsub.CommandText = "insert into " & vTableSource & " (" & vColName & ") values (" & vNewValue & ")"
                    cmsub.ExecuteNonQuery()
                    'Response.Write(cmsub.CommandText)

                    vScript = "alert('Successfully Save'); window.opener.document.form1.h_Mode.value='reload'; window.opener.location.reload(true); window.close();"
                    h_mode.Value = ""
                End If

            Catch ex As SqlClient.SqlException
                vScript = "alert('Error occurred while trying to save. Error is: " & _
                    ex.Message.Replace(vbCrLf, "").Replace("'", "") & "');"
            End Try
        End If
        'h_Sql.Value = "select " & vColSource & " from " & vTableSource & " " & vFilter

        c.Close()
        cm.Dispose()
        c.Dispose()

    End Sub
    Private Sub BuildSubListbox(ByVal pId As String, pKey As String, pRetVal As String, pCurrVal As String)
        Dim c As New SqlClient.SqlConnection(connStr)
        Dim cm As New SqlClient.SqlCommand
        Dim rs As SqlClient.SqlDataReader

        Dim cmSub As New SqlClient.SqlCommand
        'Dim rsSub As SqlClient.SqlDataReader

        Dim vSubKeyList As String = ""
        Dim vlblName As String = ""
        Dim vSql As String = ""
        Dim vSelected As String = ""

        c.Open()
        cm.Connection = c
        cmSub.Connection = c
        'vDumpSubKey = ""

        ' ============================================================================================================================================================================
        ' == 08.07.2014 == GET SQL QUERY FOR SELECT OPTION LIST ======================================================================================================================
        ' ============================================================================================================================================================================
        cm.CommandText = "select * from zoom_ref where TagName=" & pId & " and FieldName='" & pRetVal & "'"
        'Response.Write(cm.CommandText)
        Try
            rs = cm.ExecuteReader
            If rs.Read Then
                If Not IsDBNull(rs("SqlCmd")) Then
                    vSql = rs("SqlCmd")
                End If
            End If
            rs.Close()
        Catch ex As SqlClient.SqlException
            Response.Write("Error in SQL query Get SQL command in zoom_ref:  " & ex.Message)
            Exit Sub
        End Try
         
        BuildForm += "<select style='Width:486px' class='labelL' name='" & pRetVal & "' id='" & pRetVal & "'>"
        cm.CommandText = vSql
        Try
            rs = cm.ExecuteReader
            BuildForm += "<option value=''></option>"
            Do While rs.Read
                If Request.Item("pMode") = "new" And h_mode.Value = "" Then 
                    vSelected = IIf(rs(0).ToString.Trim = "99", "selected='selected'", "")
                Else
                    'Response.Write(pCurrVal.ToString.Trim & " = " & rs(0).ToString.Trim & "<br>")
                    vSelected = IIf(pCurrVal.ToString.Trim = rs(0).ToString.Trim, "selected='selected'", "")
                End If

                BuildForm += "<option value='" & rs(0) & "' " & vSelected & ">" & rs(1) & "</option>"
                vSelected = ""
            Loop
            rs.Close()

        Catch ex As DataException
            vScript = "alert('no record in column SqlCmd in Zoom_ref.');"

            c.Close()
            c.Dispose()
            cm.Dispose()
            Exit Sub
        Finally
            c.Close()
            c.Dispose()
            cm.Dispose()
        End Try
        BuildForm += "</select>&nbsp;<img src='../images/settings.png' style='vertical-align:middle; cursor:pointer;' id='2055' alt='' />"
    End Sub

    Protected Sub cmdSave_Click(sender As Object, e As EventArgs) Handles cmdSave.Click
        h_mode.Value = "save"
        GetTable_Properties() 
    End Sub
End Class
