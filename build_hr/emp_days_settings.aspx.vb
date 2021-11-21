Imports denaro
Partial Class emp_days_settings
    Inherits System.Web.UI.Page
    Public vScript As String = ""
    Public vData As String = "" 
    Dim vMonths1() As String = {"January ", "February ", "March ", "April", "May", "June"}
    Dim vMonths2() As String = {"July", "August ", "September ", "October ", "November ", "December "}
    Dim vMon_Id1() As String = {"Jan", "Feb", "Mar", "Apr", "May", "Jun"}
    Dim vMon_Id2() As String = {"Jul", "Aug", "Sep", "Oct", "Nov", "Dec"}


    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            GetDays()
        End If

    End Sub

    Private Sub GetDays()
        Dim vSQL As String = ""
        Dim c As New SqlClient.SqlConnection(connStr)
        Dim cm As New SqlClient.SqlCommand
        Dim rs As SqlClient.SqlDataReader

        Dim vValue0 As String = ""
        Dim vValue1 As String = ""
        Dim vValue2 As String = ""
        Dim vMonValue1() As String = {0, 0, 0, 0, 0, 0}
        Dim vMonValue2() As String = {0, 0, 0, 0, 0, 0}
        Dim vTtlDaysWork As Decimal = 0
        Try
            c.Open()
        Catch ex As SqlClient.SqlException
            vScript = "alert('Error occurred while trying to connect to database. Error code 101; Error is: " & ex.Message.Replace(vbCrLf, "").Replace("'", "") & "');"
            c.Dispose()
            cm.Dispose()
            Exit Sub
        End Try

        cm.Connection = c
         
        For iCtr = 0 To 5
            If Request.Form(vMon_Id1(iCtr)) <> "" Then
                vSQL = "update glsyscntrl set Day" & vMon_Id1(iCtr) & "='" & Request.Form(vMon_Id1(iCtr)) & "'"
                'Response.Write(vSQL & "<br>")
                cm.CommandText = vSQL
                cm.ExecuteNonQuery()
            End If

            cm.CommandText = "select * from glsyscntrl "
            rs = cm.ExecuteReader

            If rs.Read Then
                If Not IsDBNull(rs("Day" & vMon_Id1(iCtr))) Then
                    vValue1 = rs("Day" & vMon_Id1(iCtr))
                    vTtlDaysWork += vValue1
                Else
                    vValue1 = 0
                End If

                If Not IsDBNull(rs("Day" & vMon_Id2(iCtr))) Then
                    vValue2 = rs("Day" & vMon_Id2(iCtr))
                    vTtlDaysWork += vValue2
                Else
                    vValue2 = 0
                End If

            End If
            rs.Close()

            vData += "<tr><td class='Tbl_labelR' style='width:80px'>" & vMonths1(iCtr) & "</td>" & _
                "<td class='Tbl_labelL' style='width:50px'>" & _
                "<input type='text' class='txtEntreR' id='" & vMon_Id1(iCtr) & "' name='" & vMon_Id1(iCtr) & _
                "' value='" & IIf(Request.Form(vMon_Id1(iCtr)) = "", vValue1, Request.Form(vMon_Id1(iCtr))) & "'></td>" & _
                "<td class='Tbl_labelL'  style='width:40px'>days</td>"

            If Request.Form(vMon_Id2(iCtr)) <> "" Then
                vSQL = "update glsyscntrl set Day" & vMon_Id2(iCtr) & "='" & Request.Form(vMon_Id2(iCtr)) & "'"
                'Response.Write(vSQL & "<br>")
                cm.CommandText = vSQL
                cm.ExecuteNonQuery()
            End If


            vData += "<td class='Tbl_labelR' style='width:80px'>" & vMonths2(iCtr) & "</td>" & _
                            "<td class='Tbl_labelL' >" & _
                            "<input type='text' class='txtEntreR' id='" & vMon_Id2(iCtr) & "' name='" & vMon_Id2(iCtr) & _
                                "' value='" & IIf(Request.Form(vMon_Id2(iCtr)) = "", vValue2, Request.Form(vMon_Id2(iCtr))) & "'></td>" & _
                                "<td class='Tbl_labelL'  style='width:40px'>days</td></tr>"
        Next




        'cm.CommandText = "select * from glsyscntrl "
        'rs = cm.ExecuteReader

        'If rs.Read Then
        '    If Not IsDBNull(rs("DayYear")) Then
        '        vValue0 = rs("DayYear")
        '    Else
        '        vValue0 = 0
        '    End If

        'End If
        'rs.Close()


        If vTtlDaysWork <> 0 Then
            vSQL = "update glsyscntrl set DayYear='" & vTtlDaysWork & "'"
            cm.CommandText = vSQL
            cm.ExecuteNonQuery()
        End If


        vData += "</tr><tr><td class='Tbl_labelL' colspan='6'>&nbsp;</td></tr>" & _
            "<tr><td class='Tbl_labelL' style='width:80px' colspan='4'>Number of working day in year </td>" & _
             "<td class='Tbl_labelR' >" & _
             "<input type='text' class='txtEntreR' id='txtDayYr' name='txtDayYr' value='" & IIf(Request.Form("txtDayYr") = "", vTtlDaysWork, Request.Form("txtDayYr")) & "'  readonly='readonly'>" & _
             "</td><td class='Tbl_labelL'  style='width:40px'>days</td>"

        cm.Dispose()
        c.Dispose()
        c.Close()
    End Sub


    Protected Sub cmdCancel_Click(sender As Object, e As EventArgs) Handles cmdCancel.Click
        vScript = "window.close();"
    End Sub

    Protected Sub cmdSave_Click(sender As Object, e As EventArgs) Handles cmdSave.Click
        GetDays()
        vScript = "alert('Successfully Save'); opener.document.form1.submit(); window.close();"
    End Sub
End Class
