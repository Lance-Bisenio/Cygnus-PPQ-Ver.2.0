Imports denaro
Partial Class itemmaster_request
    Inherits System.Web.UI.Page
    Dim vColNames As String = ""
    Dim vColSource As String = ""
    Dim vTableSource As String = ""
    Public vColHeader As String = ""
    Public vEmpRecords As String = ""
    Public vScript As String = ""
    Public vCtr As Integer = 0
    Public vTop As Integer = 0
    Public vDraw As Integer = 0
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
         
        GetTable_Properties()
        GetAllItems()
    End Sub

    Private Sub GetTable_Properties()
        Dim c As New SqlClient.SqlConnection(connStr)
        Dim cm As New SqlClient.SqlCommand
        Dim rs As SqlClient.SqlDataReader
        'Dim DataTableData As New Array()


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

        cm.CommandText = "select * from table_properties_hdr where ModuleCode='" & Request.Item("id") & "' and Published='YES' "
        rs = cm.ExecuteReader
        If rs.Read Then
            If Not IsDBNull(rs("DboTable")) Then
                vTableSource = rs("DboTable")
            End If
        End If
        rs.Close()

        If vTableSource <> "" Then
            cm.CommandText = "select * from table_properties_dtl where ModuleCode='" & Request.Item("id") & "' and Published='YES' order by ColCode"
            Try
                rs = cm.ExecuteReader
                Do While rs.Read
                    vColHeader += "<td>" & rs("ColTitle") & "</td>"

                    vColNames += rs("ColReturnValue").ToString.Trim & ","

                    If Not IsDBNull(rs("ColSource")) Then
                        vColSource += rs("ColSource")
                    End If
                Loop

                vColNames = Mid(vColNames, 1, Len(vColNames) - 1)
                vColSource = Mid(vColSource, 1, Len(vColSource) - 1)

                rs.Close()
            Catch ex As SqlClient.SqlException
                vScript = "alert('Error occurred while trying to buil shift reference. Error is: " & _
                    ex.Message.Replace(vbCrLf, "").Replace("'", "") & "');"

            End Try
        End If

        c.Close()
        cm.Dispose()
        c.Dispose()

    End Sub
    Private Sub GetAllItems()
        'Dim da As SqlClient.SqlDataAdapter
        Dim ds As New DataSet
        Dim vFilter As String = ""
        Dim vSearchBy As String = Request.Item("search[value]")
        Dim vOrderByCol As Integer = Request.Item("order[0][column]") 
        Dim vOrderBy As String = "" 

        Dim c As New SqlClient.SqlConnection
        Dim cm As New SqlClient.SqlCommand
        Dim rs As SqlClient.SqlDataReader
        Dim vColNameList() As String = vColNames.Split(",")
        Dim vSQL As String = ""

        vFilter = "where IsActive=1 "

        If vSearchBy <> "" Then
            vFilter += " and Descr like '%" & vSearchBy & "%' "
        End If

         
        If vOrderByCol > 0 Then
            vOrderByCol = vOrderByCol - 1
        End If

        vOrderBy = " Order By " & vColNameList(vOrderByCol) & " " & Request.Item("order[0][dir]")

        vTop = Request.Item("draw") * Request.Item("length")
        vDraw = Request.Item("draw")

        c.ConnectionString = connStr
        c.Open()
        cm.Connection = c

        'vSQL = "select TOP " & vTop & " " & vColSource & " from " & vTableSource & " " & vFilter & vOrderBy


        vSQL = "SELECT " & vColSource & " FROM " & _
                    "(SELECT *, ROW_NUMBER() OVER (" & vOrderBy & ") as row FROM " & vTableSource & ") a " & _
               "WHERE a.row >= " & vDraw & " and a.row <= " & vTop & vOrderBy

        'Response.Write(vSQL)
        cm.CommandText = vSQL
        cm.ExecuteNonQuery()

        rs = cm.ExecuteReader
        Do While rs.Read
            vEmpRecords += "["""","
            For Each ColEmployee In vColNameList
                If Not IsDBNull(rs(ColEmployee)) Then
                    'vEmpRecords += """" & Server.UrlEncode(rs(ColEmployee)) & ""","
                    vEmpRecords += """" & rs(ColEmployee).ToString.Replace("""", "''") & ""","
                Else
                    vEmpRecords += """"","
                End If
                vCtr += 1
            Next
            vEmpRecords = Mid(vEmpRecords, 1, Len(vEmpRecords) - 1)
            vEmpRecords += "],"

        Loop
        If Len(vEmpRecords) Then
            vEmpRecords = Mid(vEmpRecords, 1, Len(vEmpRecords) - 1)
        End If

        rs.Close()
        cm.Dispose()
        c.Close()
        c.Dispose()

    End Sub
End Class
