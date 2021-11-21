Imports denaro
Partial Class bom_buildmachine
    Inherits System.Web.UI.Page
    Public vScript As String = ""
    Public vAlt_Machine As String = ""
    'Public vMachine_Perp As String = ""

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Session("uid") = Nothing Or Session("uid") = "" Then
            vScript = "alert('Login session has expired. Please re-login again.'); window.opener.document.form1.h_Mode.value='reload'; window.opener.document.form1.submit(); window.close();"
            Exit Sub
        End If
        If Not IsPostBack Then
            'BuildCombo("select Mach_Cd,Descr from ref_item_machine where  Mach_Cd not in ( " & _
            '    "select Mach_Cd from bom_machine where BOM_Cd='" & Request.Item("vBom") & "' and " & _
            '    "Proc_Cd='" & Request.Item("vProc") & "' and Mach_Cd <>'" & Request.Item("vMach") & "') and Sect_Cd='" & Request.Item("vSect") & "' and " & _
            '    "Mach_Cd<>'99' order by Descr", cmbMachine) 

            BuildCombo("select Uom_Cd,Descr from ref_item_uom where Uom_Cd<>'99' order by Descr", cmbUom)

            Dim c As New SqlClient.SqlConnection
            Dim cm As New SqlClient.SqlCommand
            Dim rs As SqlClient.SqlDataReader
            Dim vMachList As String = "" 

            c.ConnectionString = connStr
            c.Open()
            cm.Connection = c

            ' -----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            ' BOM HEADER INFORMATION 
            ' -----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            cm.CommandText = "select Item_Cd," & _
                    "(select Descr from item_master where item_master.Item_Cd=bom_header.Item_Cd) as Item_Name " & _
                    "from bom_header where BOM_Cd=" & Request.Item("pBom") & " and Revision=" & Request.Item("pBomRev") & ""
            'Response.Write(cm.CommandText)
            Try
                rs = cm.ExecuteReader
                If rs.Read Then
                    txtBOM_Cd.Text = Request.Item("pBom")
                    txtBOM_Rev.Text = Request.Item("pBomRev")
                    txtBOM_Item.Text = IIf(IsDBNull(rs("Item_Name")), " ", rs("Item_Name"))
                    txtBOM_ItemCd.Text = IIf(IsDBNull(rs("Item_Cd")), " ", rs("Item_Cd")) 
                End If
                rs.Close()
            Catch ex As SqlClient.SqlException
                Response.Write("Error in SQL query Get BOM Header Details:  " & ex.Message)
            End Try


            ' -----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            ' BOM PROCESS INFORMATION 
            ' -----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            cm.CommandText = "select *," & _
                "(select Descr from ref_emp_section where ref_emp_section.Section_Cd=Sect_Cd) as vSectName," & _
                "(select Descr from ref_item_process where ref_item_process.Proc_Cd=bom_process.Proc_Cd ) as vProcName " & _
                "from bom_process where BOM_Cd=" & Request.Item("pBom") & " and Revision=" & Request.Item("pBomRev") & " and TranId='" & Request.Item("pProTranId") & "'"

            Try
                rs = cm.ExecuteReader
                If rs.Read Then
                    txtBOM_Process.Text = rs("vProcName")
                    txtBOM_Section.Text = rs("vSectName")
                End If
                rs.Close()
            Catch ex As SqlClient.SqlException
                Response.Write("Error in SQL query Get BOM Process details:  " & ex.Message)
            End Try
             
            c.Close()
            c.Dispose()
            cm.Dispose()

            BuildCombo("select Mach_Cd,Descr from ref_item_machine where Sect_Cd='" & Request.Item("pSect") & "' and " & _
              "Mach_Cd<>'99' order by Descr", cmbMachine)
             
            cmbMachine.Items.Add(" ")
            cmbMachine.SelectedValue = " "

            If Request.Item("mode") = "Edit" Or Request.Item("mode") = "View" Then


                GetBOM_Machine_Details()
            Else
                GetMachine_Details()
            End If
            GetAlternative_Machine("Reload")
            'GetAllPeripheral()
        End If

        If Request.Item("mode") = "View" Then
            vScript = "$('#cmdSave').hide();"
        End If
    End Sub
  
    Protected Sub cmbMachine_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbMachine.SelectedIndexChanged
        GetMachine_Details()
        GetAlternative_Machine("Reload")
    End Sub
    
    Private Sub GetMachine_Details()
        Dim c As New SqlClient.SqlConnection
        Dim cm As New SqlClient.SqlCommand
        Dim rs As SqlClient.SqlDataReader

        c.ConnectionString = connStr
        c.Open()
        cm.Connection = c
        cm.CommandText = "select *," & _
            "(select Descr from ref_item_uom where ref_item_uom.Uom_Cd=ref_item_machine.Uom_Cd ) as UomDescr " & _
            "from ref_item_machine where " & _
            "Mach_Cd='" & cmbMachine.SelectedValue & "' "

        'Response.Write(cm.CommandText)
        Try
            rs = cm.ExecuteReader
            If rs.Read Then
                txtCapUnit.Text = IIf(IsDBNull(rs("CapUnit")), "", rs("CapUnit"))
                txtMat_Cd.Text = IIf(IsDBNull(rs("Mach_Cd")), "", rs("Mach_Cd"))
                cmbUom.SelectedValue = IIf(IsDBNull(rs("UOM_Cd")), "", rs("UOM_Cd"))
            End If
            rs.Close()
        Catch ex As SqlClient.SqlException
            Response.Write("Error in SQL query Get Machine Details from machine reference:  " & ex.Message)
        End Try
       
        c.Close()
        c.Dispose()
        cm.Dispose()
    End Sub

    Private Sub GetBOM_Machine_Details()
        Dim c As New SqlClient.SqlConnection
        Dim cm As New SqlClient.SqlCommand
        Dim rs As SqlClient.SqlDataReader

        c.ConnectionString = connStr
        c.Open()
        cm.Connection = c
        cm.CommandText = "select * from bom_machine " & _
            "where BOM_Cd=" & Request.Item("pBom") & " and Revision=" & Request.Item("pBomRev") & " and TranId='" & Request.Item("pMachTranId") & "'"
        'Response.Write(cm.CommandText)
        Try
            rs = cm.ExecuteReader
            If rs.Read Then
                txtMat_Cd.Text = rs("Mach_Cd")
                cmbMachine.SelectedValue = rs("Mach_Cd")

                txtCapUnit.Text = rs("CapUnit")
                cmbUom.SelectedValue = rs("UOM_Cd")
                h_MachList.Value = IIf(IsDBNull(rs("Alt_Mach")), " ", rs("Alt_Mach"))
            End If
            rs.Close()
        Catch ex As SqlClient.SqlException
            Response.Write("Error in SQL query Get BOM Machine Details:  " & ex.Message)
        End Try
       
        c.Close()
        c.Dispose()
        cm.Dispose()
    End Sub

    Protected Sub cmdSave_Click(sender As Object, e As EventArgs) Handles cmdSave.Click
        Save()
        GetAlternative_Machine("Save")
        vScript = "alert('Successfully saved.'); window.opener.document.form1.h_Mode.value='ProcessDetails'; window.opener.document.form1.submit(); window.close();"
    End Sub

    Private Sub Save() 
        Dim c As New SqlClient.SqlConnection
        Dim cm As New SqlClient.SqlCommand
        Dim rs As SqlClient.SqlDataReader
        Dim vSQL As String = ""

        c.ConnectionString = connStr
        c.Open()
        cm.Connection = c

        If Request.Item("mode") = "Edit" Then
            vSQL = "update bom_machine set Mach_Cd='" & cmbMachine.SelectedValue & "',CapUnit='" & txtCapUnit.Text & "',UOM_Cd='" & cmbUom.SelectedValue & "'," & _
                "StdLeadDay='" & txtLeadDays.Text & "',StdLeadTime='" & txtLeadTime.Text & "',[Default]='" & rdoDefault.SelectedValue & "',CreatedBy='" & Session("uid") & "',DateCreated='" & Now & "' " & _
                "where BOM_Cd=" & Request.Item("pBom") & " and Revision=" & Request.Item("pBomRev") & " and TranId='" & Request.Item("pMachTranId") & "'"
        Else
            vSQL = "insert into bom_machine (BOM_Cd,Parent_TranId,Revision,Sect_Cd,Proc_Cd,Mach_Cd,CapUnit,UOM_Cd,StdLeadDay,StdLeadTime,[Default],CreatedBy,DateCreated ) values ( " & _
                Request.Item("pBom") & ",'" & Request.Item("pProTranId") & "'," & Request.Item("pBomRev") & ",'" & Request.Item("pSect") & "','" & Request.Item("pProc") & "','" & _
                cmbMachine.SelectedValue & "','" & txtCapUnit.Text & "','" & cmbUom.SelectedValue & "','" & txtLeadDays.Text & "','" & txtLeadTime.Text & "','" & _
                rdoDefault.SelectedValue & "','" & Session("uid") & "', '" & Now & "')"
        End If

        cm.CommandText = vSQL
        'Response.Write(vSQL)
        Try
            cm.ExecuteNonQuery()
        Catch ex As SqlClient.SqlException
            Response.Write("Error in SQL query insert/update:  " & ex.Message)
        End Try

        c.Close()
        c.Dispose()
        cm.Dispose()
          
    End Sub

    Private Sub GetAlternative_Machine(vOption As String)
        Dim c As New SqlClient.SqlConnection
        Dim cm As New SqlClient.SqlCommand
        Dim rs As SqlClient.SqlDataReader
        Dim iCtr As Integer = 1
        Dim vCheck As String = ""
        Dim vMachList As String = ""
        Dim vActiveMachList() As String = h_MachList.Value.ToString.Split(",")
        Dim vSQL As String = ""

        c.ConnectionString = connStr
        c.Open()
        cm.Connection = c

        vAlt_Machine = ""
        vMachList = ""

        cm.CommandText = "select Mach_Cd, Descr from ref_item_machine where Mach_Cd not in ('99','" & cmbMachine.SelectedValue & "') order by Descr"
        '"select Mach_Cd, Descr from ref_item_machine where Mach_Cd not in ( " & _
        '"select Mach_Cd from bom_machine where BOM_Cd='" & Request.Item("vBom") & "' and " & _
        '"Proc_Cd='" & Request.Item("vProc") & "' ) and Mach_Cd<>99 and " & _
        '"Mach_Cd <> '" & cmbMachine.SelectedValue & "' and  " & _
        '"Sect_Cd='" & Request.Item("vSect") & "'  order by Descr "
        'Response.Write(cm.CommandText)
        rs = cm.ExecuteReader
        Do While rs.Read 
            If Request.Item(rs("Mach_Cd")) = "on" Then
                vCheck = " checked='checked' "
                vMachList += rs("Mach_Cd") & ","
            Else
                For iLoop = 0 To UBound(vActiveMachList)
                    If vActiveMachList(iLoop) = rs("Mach_Cd") Then
                        vCheck = " checked='checked' "
                    End If
                Next
            End If

            vAlt_Machine += "<tr>"
            vAlt_Machine += "<td style='width:10px;'><input type='checkbox' id='' name='" & rs("Mach_Cd") & "'" & vCheck & " /></td><td>" & rs("Descr") & "</td>"
            vAlt_Machine += "</tr>"
            iCtr += 1
            vCheck = ""
        Loop
        rs.Close()

        If vMachList <> "" And vOption = "Save" Then
            vMachList = Mid(vMachList, 1, Len(vMachList) - 1)
            vSQL = "update bom_machine set Alt_Mach='" & vMachList & "' " & _
                "where BOM_Cd=" & Request.Item("pBom") & " and Revision=" & Request.Item("pBomRev") & " and Parent_Tranid='" & Request.Item("pProTranId") & "' "

            If Request.Item("mode") = "Edit" Then
                vSQL += "and TranId='" & Request.Item("pMachTranId") & "'"
            Else
                vSQL += "and Sect_Cd='" & Request.Item("pSect") & "' and Proc_Cd='" & Request.Item("pProc") & "' and Mach_Cd='" & txtMat_Cd.Text & "'"
            End If
            cm.CommandText = vSQL
            'Response.Write(cm.CommandText)
            Try
                cm.ExecuteNonQuery()
            Catch ex As SqlClient.SqlException
                Response.Write("Error in SQL query update alternative machine: " & ex.Message)
            End Try
        End If

        c.Close()
        c.Dispose()
        cm.Dispose()
    End Sub
      
End Class
