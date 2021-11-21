Imports denaro
Partial Class inventory_joborder_helper
    Inherits System.Web.UI.Page
    Public vScript As String = ""
    Public vEmpList As String = ""
    Public vHelperList As String = ""

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        If Session("uid") = Nothing Or Session("uid") = "" Then
            vScript = "alert('Login session has expired. Please re-login again.'); window.opener.document.form1.submit(); window.close();"
            Exit Sub
        End If


        If Not IsPostBack Then
            BuildCombo("select Section_Cd, Descr from ref_emp_section order by Descr ", cmbSection)
            cmbSection.SelectedValue = Request.Item("pSection")

        End If

        GetAllEmployeeBySection()
        GetAllHelper()

    End Sub

    Private Sub GetAllEmployeeBySection()
        Dim c As New SqlClient.SqlConnection(connStr)
        Dim cm As New SqlClient.SqlCommand
        Dim rs As SqlClient.SqlDataReader
        Dim vTranId As String = Request.Item("pTranId")

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
        cm.CommandText = "select Emp_Cd, Emp_Lname, Emp_Fname, " & _
                "(select Descr from ref_emp_department where Dept_Cd=DeptCd) as vDept," & _
                "(select Descr from ref_emp_section where SectionCd=Section_Cd ) as vSect," & _
                "(select Descr from ref_emp_position where emp_master.Pos_Cd=ref_emp_position.Pos_Cd ) as vPos " & _
                "from emp_master " & _
                "where SectionCd = '" & cmbSection.SelectedValue & "' "

        'Response.Write(cm.CommandText)

        Try
            vEmpList = ""
            rs = cm.ExecuteReader
            Do While rs.Read
                vEmpList += "<tr class='trLink' onclick='SetupHelperList(""" & _
                                Request.Item("pJO") & """,""" & _
                                Request.Item("pSection") & """,""" & _
                                Request.Item("pProcess") & """,""" & _
                                Request.Item("pSFG") & """,""" & _
                                rs("Emp_Cd") & """)'>"

                vEmpList += "<td style='width:40px; border-bottom: solid 1px #efefef;padding-top: 10px; padding-bottom: 10px;'>" & _
                                "<img alt='' src='../images/menu/helper.png' style='margin-top:0px; padding:0px;'/></td>" & _
                                "<td style='border-bottom: solid 1px #efefef;padding-top: 10px; padding-bottom: 10px;'>" & _
                                    "" & rs("Emp_Lname") & ", " & rs("Emp_Fname") & "<br />" & _
                                    "" & rs("vDept") & "<br />" & _
                                    "" & rs("vPos") & "<br />" & _
                                "</td>" & _
                            "</tr>"
            Loop
            rs.Close() 

        Catch ex As SqlClient.SqlException
            vScript = "alert('Error occurred while trying to buil shift reference. Error is: " & _
                ex.Message.Replace(vbCrLf, "").Replace("'", "") & "');"
        End Try

        c.Close()
        cm.Dispose()
        c.Dispose()
          
    End Sub

    Private Sub GetAllHelper()
        Dim c As New SqlClient.SqlConnection(connStr)
        Dim cm As New SqlClient.SqlCommand
        Dim rs As SqlClient.SqlDataReader

        Dim cmref As New SqlClient.SqlCommand
        Dim rsref As SqlClient.SqlDataReader

        Dim vSect As String = Request.Item("pSection")
        Dim vProc As String = Request.Item("pProcess")
        Dim vJO As String = Request.Item("pJO")
        Dim vPos As String = ""

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
        cmref.Connection = c

        cm.CommandText = "select TranId, Helper_Emp_Cd, " & _
            "(select Descr from ref_emp_section where Sect_Cd=Section_Cd ) as vSect," & _
            "(select Descr from ref_item_process where ref_item_process.Proc_Cd=prod_helper.Proc_Cd and Sect_Cd='" & vSect & "') as vProc," & _
            "(select Emp_Lname + ', ' + Emp_FName as FullName from emp_master where Emp_Cd=Helper_Emp_Cd ) as vName " & _
            "from prod_helper where JobOrderNo='" & vJO & "' and Sect_Cd='" & vSect & "' and Proc_Cd='" & vProc & "' and DateRemove is null "

        'Response.Write(cm.CommandText)

        Try
            vHelperList = ""
            rs = cm.ExecuteReader
            Do While rs.Read

                cmref.CommandText = "select Emp_Cd, " & _
                    "(select Descr from ref_emp_department where Dept_Cd=DeptCd) as vDept," & _
                    "(select Descr from ref_emp_section where SectionCd=Section_Cd ) as vSect," & _
                    "(select Descr from ref_emp_position where emp_master.Pos_Cd=ref_emp_position.Pos_Cd ) as vPos " & _
                    "from emp_master where Emp_Cd='" & rs("Helper_Emp_Cd") & "'"

                rsref = cmref.ExecuteReader
                If rsref.Read Then

                    vSect = IIf(IsDBNull(rsref("vSect")), "", rsref("vSect"))
                    vPos = IIf(IsDBNull(rsref("vPos")), "", rsref("vPos"))
                End If
                rsref.Close()

                 
                vHelperList += "<tr>" & _
                        "<td style='width:50px; border-bottom: solid 1px #efefef; padding-top: 15px; padding-bottom: 15px; text-align:center;'>" & _
                                "<img alt='' src='../images/menu/helper.png' style='margin-top:0px; padding:0px;'/></td>" & _
                        "<td class='labelL' style='border-bottom: solid 1px #efefef;'>" & _
                            rs("Helper_Emp_Cd") & "<br>" & _
                            rs("vName") & "<br />" & _
                            vSect & "<br />" & _
                            vPos & "</td>" & _
                        "<td class='trLink' onclick='RemoveHelperList(""" & rs("TranId") & """)'" & _
                        "style='border-bottom: solid 1px #efefef; width:80px; text-align:center;'>Remove</td>" & _
                    "</tr>"

                 
            Loop
            rs.Close()

        Catch ex As SqlClient.SqlException
            vScript = "alert('Error occurred while trying to buil shift reference. Error is: " & _
                ex.Message.Replace(vbCrLf, "").Replace("'", "") & "');"
        End Try

        c.Close()
        cm.Dispose()
        c.Dispose()
    End Sub
    Protected Sub cmbSection_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbSection.SelectedIndexChanged
        
    End Sub
End Class
