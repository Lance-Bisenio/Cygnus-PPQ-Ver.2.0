Imports denaro
Partial Class inventory_production_xml
    Inherits System.Web.UI.Page
    Public vReturn As String = ""
    Public vScript As String = ""

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        Select Case Request.Item("pMode")
            Case "Prod-HelperSetup"
                Production_helperSetup()
            Case "Prod-HelperRemove"
                Production_helperRemove()

        End Select

        If Session("uid") <> "" Then
            'modify()
        End If

    End Sub
    Private Sub Production_helperSetup()
        Dim c As New SqlClient.SqlConnection(connStr)
        Dim cm As New SqlClient.SqlCommand
        Dim vSQL As String = ""

        Dim vJO As String = Request.Item("pJO")
        Dim vSect As String = Request.Item("pSect")
        Dim vProc As String = Request.Item("pProc")
        Dim vEmp As String = Request.Item("pEmp")

        Dim vFG As String = ""
        Dim vSFG As String = Request.Item("pSFG")

        Try
            c.Open()
        Catch ex As SqlClient.SqlException
            vScript = "alert('Error occurred while trying to connect to database. Error code 101; Error is: " & ex.Message.Replace(vbCrLf, "").Replace("'", "") & "');"
            c.Dispose()
            cm.Dispose()

            Exit Sub
        End Try

        cm.Connection = c

        vFG = GetRef("select Item_Cd from jo_header where JobOrderNo='" & Request.Item("pJO") & "' ", "")

        vSQL = "delete from prod_helper where " & _
                    "JobOrderNo='" & vJO & "' and " & _
                    "Sect_Cd='" & vSect & "' and " & _
                    "Proc_Cd='" & vProc & "' and " & _
                    "SFG_Item_Cd='" & vSFG & "' and " & _
                    "Helper_Emp_Cd='" & vEmp & "'"
        cm.CommandText = vSQL


        Try
            cm.ExecuteNonQuery()

            vSQL = "insert into prod_helper (JobOrderNo,Sect_Cd,Proc_Cd,FG_Item_Cd,SFG_Item_Cd,Helper_Emp_Cd,CreatedBy,DateCreated)" & _
               "values ('" & vJO & "','" & vSect & "','" & vProc & "','" & vFG & "','" & vSFG & "','" & vEmp & "','" & Session("uid") & "','" & Now() & "')"
            cm.CommandText = vSQL
            cm.ExecuteNonQuery()
        Catch ex As DataException
            vScript = "alert('An error occurred while trying to Save the new record.');"
        End Try
        c.Close()
        c.Dispose()
        cm.Dispose()


    End Sub
  
    Private Sub Production_helperRemove()
        Dim c As New SqlClient.SqlConnection(connStr)
        Dim cm As New SqlClient.SqlCommand
        Dim vSQL As String = ""

        Dim vTranId As String = Request.Item("pTranId")

        Try
            c.Open()
        Catch ex As SqlClient.SqlException
            vScript = "alert('Error occurred while trying to connect to database. Error code 101; Error is: " & ex.Message.Replace(vbCrLf, "").Replace("'", "") & "');"
            c.Dispose()
            cm.Dispose()

            Exit Sub
        End Try

        cm.Connection = c


        vSQL = "update prod_helper set RemoveBy='" & Session("uid") & "', DateRemove='" & Now() & "' where TranId=" & vTranId & ""

        cm.CommandText = vSQL

        Try
            cm.ExecuteNonQuery()

        Catch ex As DataException
            vScript = "alert('An error occurred while trying to Save the new record.');"
        End Try
        c.Close()
        c.Dispose()
        cm.Dispose()


    End Sub

    Private Sub modify()
        Dim c As New SqlClient.SqlConnection(connStr)
        Dim cm As New SqlClient.SqlCommand
        Dim vSQL As String = ""

        Dim vLoadDate() As String = Request.Item("pDate").Split("_")
        Dim vStartDate As Date = Format(CDate(vLoadDate(0) & " " & vLoadDate(1) & ":00"), "MM/dd/yyyy HH:mm")


        Try
            c.Open()
        Catch ex As SqlClient.SqlException
            vScript = "alert('Error occurred while trying to connect to database. Error code 101; Error is: " & ex.Message.Replace(vbCrLf, "").Replace("'", "") & "');"
            c.Dispose()
            cm.Dispose()

            Exit Sub
        End Try

        cm.Connection = c

        vSQL = "update jo_machine set StartDate='" & vStartDate & "', CreatedBy='" & Session("uid") & "', DateCreated='" & Now() & "' where TranId=" & Request.Item("pTranId")

        vReturn = vSQL

        cm.CommandText = vSQL

        Try
            cm.ExecuteNonQuery()

        Catch ex As DataException
            vScript = "alert('An error occurred while trying to Save the new record.');"
        End Try
        c.Close()
        c.Dispose()
        cm.Dispose()

    End Sub

End Class
