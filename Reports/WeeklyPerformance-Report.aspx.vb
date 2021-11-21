Imports denaro

Partial Class Reports_WeeklyPerformanceReport
    Inherits System.Web.UI.Page
    Public vScript As String = ""
    Public vDataHeader As String = ""
    Public vDataDetails As String = ""
    Dim vSQL As String = ""

    Private Sub Reports_WeeklyPerformanceReport_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then

            vSQL = "select Section_Cd, Descr from ref_emp_section order by Descr "
            BuildCombo(vSQL, CmdSection)
        End If
    End Sub


    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click

        If txtDateFrom.Text.Trim = "" Or txtDateTo.Text.Trim = "" Then
            vScript = "alert('Invalid date parameters.');"
            Exit Sub
        End If

        Try
            txtDateFrom.Text = CDate(txtDateFrom.Text.Trim)
        Catch ex As Exception
            txtDateFrom.Focus()
            vScript = "alert('Invalid date parameters.');"
            Exit Sub
        End Try

        Try
            txtDateTo.Text = CDate(txtDateTo.Text.Trim)
        Catch ex As Exception
            txtDateTo.Focus()
            vScript = "alert('Invalid date parameters.');"
            Exit Sub
        End Try


        Dim vJOList As String = ""
        Dim vJOLength As Integer = 0
        Dim vDaysCtr As Long = DateDiff(DateInterval.Day, CDate(txtDateFrom.Text.Trim), CDate(txtDateTo.Text.Trim))



        'Response.Write("JO : " & vJOList)


        'Response.Write(vJOList)









        vDataHeader = "<tr>" _
                        & "<th style='width:10px'>#</th>" _
                        & "<th style='width:100px'>DATES</th>"

        For i As Integer = 0 To vDaysCtr
            vDataHeader += "<th colspan='2'>" & CDate(txtDateFrom.Text.Trim).AddDays(i) & "</th>"
        Next

        ' style='width:100px'
        vDataHeader += "<th style='width:100px' colspan='2'>WEEKLY AVERAGE</th>" _
                        & "<th style='width:100px'>MACHINE RESULTS</th>" _
                    & "</tr>"

        vDataHeader += "<tr>" _
                        & "<th></th>" _
                        & "<th></th>"

        For i As Integer = 0 To vDaysCtr
            vDataHeader += "<th>AM</th>"
            vDataHeader += "<th>PM</th>"
        Next

        vDataHeader += "<th>AM</th>"
        vDataHeader += "<th>PM</th>"
        vDataHeader += "<th></th></tr>"



        Dim c As New SqlClient.SqlConnection(connStr)
        Dim cm As New SqlClient.SqlCommand
        Dim rs As SqlClient.SqlDataReader

        Dim cmsub As New SqlClient.SqlCommand
        Dim rssub As SqlClient.SqlDataReader
        Dim vSubData As String = ""
        Dim vTtlAMMeter As Decimal = 0
        Dim vTtlPMMeter As Decimal = 0
        Dim vTempData As String = ""

        Try
            c.Open()
        Catch ex As SqlClient.SqlException
            vScript = "alert('Error occurred while trying to connect to database. Error is: " &
                ex.Message.Replace(vbCrLf, "").Replace("'", "") & "');"
            c.Dispose()
            cm.Dispose()
            Exit Sub
        End Try

        cm.Connection = c
        cmsub.Connection = c

        vSQL = "select Mach_Cd, Descr  from ref_item_machine where Sect_Cd='" & CmdSection.SelectedValue & "'"

        cm.CommandText = vSQL
        rs = cm.ExecuteReader
        Do While rs.Read

            'vSQL = "Select JobOrderNo, Mach_Cd, Proc_Cd, IsPrimary from jo_machine
            ''where IsPrimary ='YES' and 
            ''JobOrderNo in "

            'cmsub.CommandText = vSQL
            'rssub = cmsub.ExecuteReader
            'Do While rssub.Read

            'Loop
            'rssub.Close()


            vDataDetails += "<tr>" _
                & "<td></td>" _
                & "<td>" & rs(0) & "</td>"


            For i As Integer = 0 To vDaysCtr


                Session("sJOList") = ""
                Get_JOList(CDate(txtDateFrom.Text.Trim).AddDays(i))
                vJOLength = Session("sJOList").ToString.Length
                vJOList = Session("sJOList")
                vJOList = vJOList.ToString.Substring(0, vJOLength - 1)

                vSQL = "select sum(Meter) as vMeter from prod_completion " _
                    & "where DateCreated between '" _
                        & CDate(txtDateFrom.Text.Trim).AddDays(i) & " 00:00:00' and '" _
                        & CDate(txtDateFrom.Text.Trim).AddDays(i) & " 11:59:59' and  " _
                    & "TranType='COMPLETION' and " _
                    & "IsDeleted is null and  " _
                    & "Sect_Cd='" & CmdSection.SelectedValue & "' and " _
                    & "JONO in (select JobOrderNo from jo_machine where JobOrderNo in (" & vJOList & ") and IsPrimary='YES' and Mach_Cd='" & rs(0) & "')"



                vTempData = GetRef(vSQL, 0)

                If vTempData = "" Or vTempData = "null" Then
                    vDataDetails += "<td></td>"
                    vSubData += "<td></td>"
                Else
                    vTtlAMMeter = vTempData

                    vDataDetails += "<td>" & Format(vTtlAMMeter, "###,###.00") & "</td>"
                    vTtlAMMeter = vTtlAMMeter / (12 * 60)
                    vSubData += "<td>" & Format(vTtlPMMeter, "###,###.00") & "</td>"
                End If



                vSQL = "select sum(Meter) as vMeter from prod_completion " _
                    & "where DateCreated between '" _
                        & CDate(txtDateFrom.Text.Trim).AddDays(i) & " 12:00:00' and '" _
                        & CDate(txtDateFrom.Text.Trim).AddDays(i) & " 23:59:59' and  " _
                    & "TranType='COMPLETION' and " _
                    & "IsDeleted is null and  " _
                    & "Sect_Cd='" & CmdSection.SelectedValue & "' and " _
                    & "JONO in (select JobOrderNo from jo_machine where JobOrderNo in (" & vJOList & ") and IsPrimary='YES' and Mach_Cd='" & rs(0) & "')"

                vTempData = GetRef(vSQL, 0)

                If vTempData = "" Or vTempData = "null" Then
                    vDataDetails += "<td></td>"
                    vSubData += "<td></td>"
                Else
                    vTtlPMMeter = vTempData
                    vDataDetails += "<td>" & Format(vTtlPMMeter, "###,###.00") & "</td>"
                    vTtlPMMeter = vTtlPMMeter / (12 * 60)
                    vSubData += "<td>" & Format(vTtlPMMeter, "###,###.00") & "</td>"
                End If


            Next

            vDataDetails += "<td></td>"
            vDataDetails += "<td></td>"
            vDataDetails += "<td></td>"

            vDataDetails += "</tr>"
            vDataDetails += "<tr><td></td><td></td>"
            vDataDetails += vSubData
            vDataDetails += "</tr>"

            vSubData = ""
            vTtlPMMeter = 0
            vTtlAMMeter = 0
            vTempData = ""
        Loop
        rs.Close()

        c.Close()
        cm.Dispose()
        c.Dispose()

    End Sub


    Private Sub Get_JOList(pDate As String)

        Dim c As New SqlClient.SqlConnection(connStr)
        Dim cm As New SqlClient.SqlCommand
        Dim rs As SqlClient.SqlDataReader
        Dim vJOList As String = ""

        Dim iCtr As Integer = 0
        Try
            c.Open()
        Catch ex As SqlClient.SqlException
            vScript = "alert('Error occurred while trying to connect to database. Error is: " &
                ex.Message.Replace(vbCrLf, "").Replace("'", "") & "');"
            c.Dispose()
            cm.Dispose()
            Exit Sub
        End Try

        cm.Connection = c
        vSQL = "select distinct(JONO) " _
            & "from prod_completion " _
            & "where DateCreated between '" & CDate(pDate) & " 00:00:00' and '" & CDate(pDate) & " 23:59:59'"

        cm.CommandText = vSQL
        rs = cm.ExecuteReader
        Do While rs.Read
            vJOList += "'" & rs(0) & "',"
        Loop
        rs.Close()

        Session("sJOList") = vJOList

        c.Close()
        cm.Dispose()
        c.Dispose()
    End Sub

End Class
