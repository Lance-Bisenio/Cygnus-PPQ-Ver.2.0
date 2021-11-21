Imports System.Data
Imports denaro.fis
Partial Class main
    Inherits System.Web.UI.Page
    Public vScript As String = ""
    Public vNotice As String = ""
    Public vReminders As New StringBuilder
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("uid") = "" Then
            Server.Transfer("index.aspx")
            Exit Sub
        End If

        'If Not CanRun(Session("caption"), Request.Item("id")) Then
        '    Session("denied") = "1"
        '    Server.Transfer("main.aspx")
        '    Exit Sub
        'End If

        If Session("denied") = "1" Then
            vScript = "alert('Sorry, but you are not allowed to access this menu.'); " 'window.location='queue.aspx';"
            Session.Remove("denied")
            Exit Sub
        End If



        'If Not IsPostBack Then
        '    'datarefresh()
        '    'reminders()
        '    If Session("norecord") <> "1" Then Server.Transfer("queue.aspx")
        'End If
    End Sub
    'Private Sub reminders()
    '    Dim c As New SqlClient.SqlConnection(connStr)
    '    Dim cm As New SqlClient.SqlCommand
    '    Dim cmRef As New SqlClient.SqlCommand
    '    Dim rs As SqlClient.SqlDataReader
    '    Dim rsRef As SqlClient.SqlDataReader
    '    Dim vClass As String = "linkstyle-a"
    '    Dim vStatus As String = ""
    '    Dim vCompany As String = ""
    '    Dim vCategory As String = ""
    '    Dim vFullname As String = ""
    '    Dim vProcessBy As String = ""
    '    Dim vElapsed As Single = 0
    '    Dim vCtr As Integer = 1
    '    c.Open()
    '    cm.Connection = c
    '    cmRef.Connection = c

    '    cm.CommandText = "select * from dm_document where Status_Cd <> '16' order by Date_Encoded "
    '    rs = cm.ExecuteReader

    '    Do While rs.Read
    '        cmRef.CommandText = "select Descr from dm_document_status where Status_Cd=" & rs("Status_Cd")
    '        vStatus = rs("Status_Cd")
    '        rsRef = cmRef.ExecuteReader
    '        If rsRef.Read Then
    '            vStatus = rsRef("Descr")
    '        End If
    '        rsRef.Close()

    '        cmRef.CommandText = "select SupplierName from supplier where supplierCd ='" & rs("Supplier_Cd") & "' "
    '        rsRef = cmRef.ExecuteReader
    '        If rsRef.Read Then
    '            vCompany = rsRef("SupplierName")
    '        End If
    '        rsRef.Close()

    '        cmRef.CommandText = "select Fullname from user_list where User_Id='" & rs("Emp_Cd") & "'"
    '        vProcessBy = ""
    '        rsRef = cmRef.ExecuteReader
    '        If rsRef.Read Then
    '            vProcessBy = rsRef("Fullname")
    '        End If
    '        rsRef.Close()

    '        cmRef.CommandText = "select Fullname from user_list where User_Id='" & _
    '            rs("Encoded_By") & "'"
    '        vFullname = rs("Encoded_By")
    '        rsRef = cmRef.ExecuteReader
    '        If rsRef.Read Then
    '            vFullname = rsRef("Fullname")
    '        End If
    '        rsRef.Close()

    '        cmRef.CommandText = "select Descr from dm_category where Category_Id=" & rs("Category_Id")
    '        rsRef = cmRef.ExecuteReader
    '        vCategory = rs("Category_Id")
    '        If rsRef.Read Then
    '            vCategory = rsRef("Descr")
    '        End If
    '        rsRef.Close()

    '        Select Case rs("Status_Cd")
    '            Case 1, 10, 12 'for processing, for review and posting
    '                vElapsed = Math.Round(DateDiff(DateInterval.Minute, CDate(rs("Date_Uploaded")), Now) / 60, 2)
    '                If Session("deptlist").ToString.ToLower.Contains(rs("Category_Id").ToString.ToLower) Then
    '                    vReminders.AppendLine("<tr class='" & vClass & "' onclick='openlink(" & rs("Doc_Id") & "," & rs("Status_Cd") & ")'>" & _
    '                         "" & _
    '                         "<td class='labelBC'><a href='javascript:openlink(" & rs("Doc_Id") & "," & rs("Status_Cd") & ");' class='textLinks'>" & _
    '                         rs("Doc_Id") & "</a></td>" & _
    '                         "<td class='labelBL'>&nbsp;" & rs("Doc_Name") & "</td>" & _
    '                        "<td class='labelBL'>&nbsp;" & vCompany & "</td>" & _
    '                        "<td class='labelBL'>&nbsp;" & vCategory & "</td>" & _
    '                         "<td class='labelBL'>&nbsp;" & vStatus & "</td>" & _
    '                         "<td class='labelBL'>&nbsp;" & vProcessBy & "</td>" & _
    '                        "<td class='labelBC'>" & rs("Date_Uploaded") & "&nbsp;</td>" & _
    '                        "<td class='labelBR'>" & vElapsed & "&nbsp;</td></tr>")

    '                End If

    '            Case 2, 9, 11, 14  'in=process, in-process (from exception), reviewed but with correction
    '                If Session("uid").ToString.ToLower = rs("Emp_Cd").ToString.ToLower Then
    '                    vElapsed = Math.Round(DateDiff(DateInterval.Minute, CDate(rs("Date_Assigned")), Now) / 60, 2)
    '                    vReminders.AppendLine("<tr class='" & vClass & "' onclick='openlink(" & rs("Doc_Id") & "," & rs("Status_Cd") & ")'>" & _
    '                             "<td class='labelBC'><a href='javascript:openlink(" & _
    '                             rs("Doc_Id") & "," & rs("Status_Cd") & ");'>" & _
    '                             rs("Doc_Id") & "</a></td>" & _
    '                             "<td class='labelBL'>&nbsp;" & rs("Doc_Name") & "</td>" & _
    '                            "<td class='labelBL'>&nbsp;" & vCompany & "</td>" & _
    '                            "<td class='labelBL'>&nbsp;" & vCategory & "</td>" & _
    '                             "<td class='labelBL'>&nbsp;" & vStatus & "</td>" & _
    '                             "<td class='labelBL'>&nbsp;" & vProcessBy & "</td>" & _
    '                            "<td class='labelBC'>" & rs("Date_Assigned") & "&nbsp;</td>" & _
    '                            "<td class='labelBR'>" & vElapsed & "&nbsp;</td></tr>")
    '                End If

    '            Case 4, 5, 6, 7          'with exceptions
    '                Dim vNewStatus As Integer = rs("Status_Cd")
    '                vElapsed = Math.Round(DateDiff(DateInterval.Minute, CDate(rs("Date_Assigned")), Now) / 60, 2)
    '                If vElapsed >= 24 And vElapsed < 48 Then
    '                    vStatus = 5
    '                    vNewStatus = vStatus
    '                ElseIf vElapsed >= 48 And vElapsed < 72 Then
    '                    vStatus = 6
    '                    vNewStatus = vStatus
    '                ElseIf vElapsed >= 72 Then
    '                    vStatus = 7
    '                    vNewStatus = vStatus
    '                End If

    '                cmRef.CommandText = "update dm_document set Status_Cd=" & vNewStatus & _
    '                    " where Doc_Id=" & rs("Doc_Id")
    '                cmRef.ExecuteNonQuery()

    '                cmRef.CommandText = "select Descr from dm_document_status where Status_Cd=" & vNewStatus
    '                vStatus = rs("Status_Cd")
    '                rsRef = cmRef.ExecuteReader
    '                If rsRef.Read Then
    '                    vStatus = rsRef("Descr")
    '                End If
    '                rsRef.Close()

    '                If Session("deptlist").ToString.ToLower.Contains(rs("Category_Id").ToString.ToLower) Then
    '                    vReminders.AppendLine("<tr class='" & vClass & "' onclick='openlink(" & rs("Doc_Id") & "," & rs("Status_Cd") & ")'>" & _
    '                         "" & _
    '                         "<td class='labelBC'><a href='javascript:openlink(" & rs("Doc_Id") & "," & vNewStatus & ");'>" & _
    '                         rs("Doc_Id") & "</a></td>" & _
    '                         "<td class='labelBL'>&nbsp;" & rs("Doc_Name") & "</td>" & _
    '                        "<td class='labelBL'>&nbsp;" & vCompany & "</td>" & _
    '                        "<td class='labelBL'>&nbsp;" & vCategory & "</td>" & _
    '                         "<td class='labelBL'>&nbsp;" & vStatus & "</td>" & _
    '                         "<td class='labelBL'>&nbsp;" & vProcessBy & "</td>" & _
    '                        "<td class='labelBC'>" & rs("Date_Assigned") & "&nbsp;</td>" & _
    '                        "<td class='labelBR'>" & vElapsed & "&nbsp;</td></tr>")
    '                End If

    '            Case 8          'in-process(re-assignment)
    '                vElapsed = Math.Round(DateDiff(DateInterval.Minute, CDate(rs("Date_Assigned")), Now) / 60, 2)
    '                If Session("deptlist").ToString.ToLower.Contains(rs("Category_Id").ToString.ToLower) Then
    '                    vReminders.AppendLine("<tr class='" & vClass & "' onclick='openlink(" & rs("Doc_Id") & "," & rs("Status_Cd") & ")'>" & _
    '                         "<td class='labelBC'><a href='javascript:openlink(" & rs("Doc_Id") & "," & rs("Status_Cd") & ");'>" & _
    '                         rs("Doc_Id") & "</a></td>" & _
    '                         "<td class='labelBL'>&nbsp;" & rs("Doc_Name") & "</td>" & _
    '                         "<td class='labelBL'>&nbsp;" & vCategory & "</td>" & _
    '                         "<td class='labelBL'>&nbsp;" & vStatus & "</td>" & _
    '                        "<td class='labelL'>&nbsp;" & vCompany & "</td>" & _
    '                        "<td class='labelBL'>&nbsp;" & vProcessBy & "</td>" & _
    '                         "<td class='labelBC'>" & rs("Date_Assigned") & "&nbsp;</td>" & _
    '                         "<td class='labelBR'>" & vElapsed & "&nbsp;</td></tr>")
    '                End If

    '        End Select
    '        vClass = IIf(vClass = "linkstyle-a", "linkstyle-b", "linkstyle-a")
    '    Loop

    '    rs.Close()
    '    cm.Dispose()
    '    c.Dispose()
    '    c.Close()

    'End Sub

    'Private Sub datarefresh()
    'Dim c As New sqlclient.sqlConnection(connStr)
    'Dim da As sqlclient.sqlDataAdapter
    'Dim ds As New DataSet

    'da = New SqlClient.SqlDataAdapter("select Doc_Id,(select Doc_Name from dm_document where Doc_Id=dm_document_dtl.Doc_Id) as Doc_Name," & _
    '                              "Value from dm_document_dtl where Alert_On=1 and str_to_date(trim([Value]),'%m/%d/%Y') " & _
    '                              "- interval alert_before_hrs day <= current_Date()", c)
    'da.Fill(ds, "Doc_for_Reminders")
    'grdReminders.DataSource = ds.Tables("Doc_for_Reminders")
    'grdReminders.DataBind()
    'da.Dispose()
    'ds.Dispose()
    'c.Dispose()
    'grdReminders.SelectedIndex = -1

    'c.Close()
    'da.Dispose()
    'c.Dispose()
    'vNotice += "</ul>"
    'End Sub
End Class
