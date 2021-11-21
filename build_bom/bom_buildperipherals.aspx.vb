Imports denaro
Partial Class bom_buildperipherals
    Inherits System.Web.UI.Page
    Public vScript As String = ""
    Public vAlt_Perp As String = ""
  
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Session("uid") = Nothing Or Session("uid") = "" Then
            vScript = "alert('Login session has expired. Please re-login again.'); window.opener.document.form1.h_Mode.value='reload'; window.opener.document.form1.submit(); window.close();"
            Exit Sub
        End If
        If Not IsPostBack Then
             
            BuildCombo("select Item_Cd, Descr from item_master where itemType_Cd='PERP' and " & _
                       "Item_Cd not in (select Item_Cd from bom_peripherals " & _
                       "where BOM_Cd=" & Request.Item("pBom") & " and Revision=" & Request.Item("pBomRev") & " and Parent_Tranid='" & Request.Item("pProTranId") & "') " & _
                       "order by Descr", cmbPerp) ' and Mach_Cd='" & Request.Item("vMach") & "'
            cmbPerp.Items.Add("")
            cmbPerp.SelectedValue = ""

            Dim c As New SqlClient.SqlConnection
            Dim cm As New SqlClient.SqlCommand
            Dim rs As SqlClient.SqlDataReader

            c.ConnectionString = connStr
            c.Open()
            cm.Connection = c
             
            ' BOM HEADER INFORMATION 
            ' -----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            cm.CommandText = "select Item_Cd," & _
                    "(select Descr from item_master where item_master.Item_Cd=bom_header.Item_Cd) as Item_Name " & _
                    "from bom_header where BOM_Cd=" & Request.Item("pBom") & " and Revision=" & Request.Item("pBomRev") & ""
            Try
                rs = cm.ExecuteReader
                If rs.Read Then
                    txtBOM_Cd.Text = Request.Item("pBom")
                    txtBOM_Rev.Text = Request.Item("pBomRev")
                    txtBOM_ItemCd.Text = IIf(IsDBNull(rs("Item_Cd")), "", rs("Item_Cd"))
                    txtBOM_Item.Text = IIf(IsDBNull(rs("Item_Name")), " ", rs("Item_Name"))
                End If
                rs.Close()
            Catch ex As SqlClient.SqlException
                Response.Write("Error in SQL query Get BOM Header Details  " & ex.Message)
            End Try


            ' BOM PROCESS INFORMATION 
            ' -----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
             
            cm.CommandText = "select *," & _
               "(select Descr from ref_emp_section where ref_emp_section.Section_Cd=Sect_Cd) as vSectName," & _
               "(select Descr from ref_item_process where ref_item_process.Proc_Cd=bom_process.Proc_Cd ) as vProcName " & _
               "from bom_process where BOM_Cd=" & Request.Item("pBom") & " and TranId='" & Request.Item("pProTranId") & "'"
            'Response.Write(cm.CommandText)
            Try
                rs = cm.ExecuteReader
                If rs.Read Then
                    txtBOM_Process.Text = rs("vProcName")
                    txtBOM_Section.Text = rs("vSectName")
                End If
                rs.Close()
            Catch ex As SqlClient.SqlException
                Response.Write("Error in SQL query Get BOM Process Details:  " & ex.Message)
            End Try

             
            ' BOM MACHINE INFORMATION 
            ' -----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            cm.CommandText = "select *, (select Descr from ref_item_machine where ref_item_machine.Mach_Cd=bom_machine.Mach_Cd) as vDescr " & _
                "from Bom_machine where BOM_Cd=" & Request.Item("pBom") & " and Revision=" & Request.Item("pBomRev") & " and TranId='" & Request.Item("pMachTranId") & "'"
            Try
                rs = cm.ExecuteReader
                If rs.Read Then
                    txtMachCode.Text = rs("Mach_Cd")
                    txtMachine.Text = IIf(IsDBNull(rs("vDescr")), "", rs("vDescr"))
                End If
                rs.Close()
            Catch ex As SqlClient.SqlException
                Response.Write("Error in SQL query Get BOM Machine Details:  " & ex.Message)
            End Try
             
            c.Close()
            c.Dispose()
            cm.Dispose()

            If Request.Item("mode") = "Edit" Or Request.Item("mode") = "View" Then
                 

                BuildCombo("select Item_Cd, Descr from item_master where itemType_Cd='PERP' and " & _
                       "Item_Cd not in (select Item_Cd from bom_peripherals where BOM_Cd='" & Request.Item("pBom") & _
                       "' and Mach_Cd='" & Request.Item("vMach") & "' and Item_Cd<>'" & Request.Item("vPerp") & "') order by Descr", cmbPerp)
                cmbPerp.Items.Add("")
                cmbPerp.SelectedValue = ""

                txtPerp_Cd.Text = Request.Item("vPerp")
                cmbPerp.SelectedValue = Request.Item("vPerp")
                GetBOM_Peripherals_Details()
            End If
            GetAlt_Peripherals("Reload") 
        End If
        If Request.Item("mode") = "View" Then
            vScript = "$('#cmdSave').hide(); $('#cmdGetItem').hide(); "
        End If
    End Sub
    Private Sub GetBOM_Peripherals_Details()
        Dim c As New SqlClient.SqlConnection
        Dim cm As New SqlClient.SqlCommand
        Dim rs As SqlClient.SqlDataReader

        c.ConnectionString = connStr
        c.Open()
        cm.Connection = c
        cm.CommandText = "select Item_Cd,DrawnFrom, Alt_Perp,TranId from bom_peripherals " & _
            "where BOM_Cd=" & Request.Item("pBom") & " and Revision=" & Request.Item("pBomRev") & " and TranId='" & Request.Item("pPerpTranId") & "'"
        'Response.Write(cm.CommandText)
        Try
            rs = cm.ExecuteReader
            If rs.Read Then
                txtPerp_Cd.Text = rs("Item_Cd")
                cmbPerp.SelectedValue = rs("Item_Cd")
                rdoDrawnFrom.SelectedValue = rs("DrawnFrom")
                h_PerpList.Value = IIf(IsDBNull(rs("Alt_Perp")), " ", rs("Alt_Perp"))
            End If
            rs.Close()
        Catch ex As SqlClient.SqlException
            Response.Write("Error in SQL query Get BOM Peripherals Details:  " & ex.Message)
        End Try

        c.Close()
        c.Dispose()
        cm.Dispose()
    End Sub

    Protected Sub cmbPerp_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbPerp.SelectedIndexChanged
        txtPerp_Cd.Text = cmbPerp.SelectedValue
        GetAlt_Peripherals("Reload")
    End Sub

    Protected Sub cmdSave_Click(sender As Object, e As EventArgs) Handles cmdSave.Click
        Save()
        GetAlt_Peripherals("Save")
        vScript = "alert('Successfully saved.'); window.opener.document.form1.h_Mode.value='MachineDetails'; window.opener.document.form1.submit(); window.close();"
    End Sub

    Private Sub Save()
        Dim c As New SqlClient.SqlConnection
        Dim cm As New SqlClient.SqlCommand
        Dim rs As SqlClient.SqlDataReader
        Dim vSQL As String = ""
        Dim vPerpList As String = ""

        c.ConnectionString = connStr
        c.Open()
        cm.Connection = c

        If Request.Item("mode") = "Edit" Then
            vSQL = "update bom_peripherals set DrawnFrom='" & rdoDrawnFrom.SelectedValue & "',Item_Cd='" & _
                        cmbPerp.SelectedValue & "',CreatedBy='" & Session("uid") & "', DateCreated='" & Now & "' " & _
                        "where BOM_Cd='" & Request.Item("pBOM") & "' and Revision=" & Request.Item("pBomRev") & " and TranId='" & Request.Item("pPerpTranId") & "' "
        Else
            vSQL = "insert into bom_peripherals (BOM_Cd,Revision,Parent_TranId,Sect_Cd, Proc_Cd, Mach_Cd, Item_Cd, DrawnFrom, CreatedBy, DateCreated) values ( " & _
                        Request.Item("pBom") & "," & Request.Item("pBomRev") & ",'" & _
                        Request.Item("pMachTranId") & "', '" & Request.Item("pSect") & "','" & Request.Item("pProc") & "','" & txtMachCode.Text & "','" & _
                        cmbPerp.SelectedValue & "','" & rdoDrawnFrom.SelectedValue & "','" & Session("uid") & "', '" & Now & "')"
        End If

        cm.CommandText = vSQL
        'Response.Write(vSQL)
        Try
            cm.ExecuteNonQuery()
        Catch ex As SqlClient.SqlException
            Response.Write("Error in SQL query insert/update:  " & ex.Message)
        End Try



        vSQL = "select distinct(a.TranId) as vId from bom_peripherals a, bom_machine b where a.Bom_Cd=" & Request.Item("pBom") & " and a.Revision=" & Request.Item("pBomRev") & _
            " and a.Parent_TranId='" & Request.Item("pMachTranId") & "' and b.Parent_TranId=" & Request.Item("pProTranId")
        cm.CommandText = vSQL
        'Response.Write(vSQL)

        Try
            rs = cm.ExecuteReader
            Do While rs.Read
                If Not IsDBNull(rs("vId")) Then
                    vPerpList += rs("vId") & ","
                End If
            Loop 
            rs.Close()
        Catch ex As SqlClient.SqlException
            Response.Write("Error in SQL query Get Machine Peripherals :  " & ex.Message)
        End Try
        vPerpList = Mid(vPerpList, 1, Len(vPerpList) - 1)
         
        vSQL = "update bom_machine set Mach_Perp='" & vPerpList & "' where BOM_Cd='" & Request.Item("pBOM") & _
                    "' and Revision=" & Request.Item("pBomRev") & " and TranId='" & Request.Item("pMachTranId") & "' " 
        cm.CommandText = vSQL 
        Try
            cm.ExecuteNonQuery()
        Catch ex As SqlClient.SqlException
            Response.Write("Error in SQL query Update Machine Peripherals :  " & ex.Message)
        End Try
         
        c.Close()
        c.Dispose()
        cm.Dispose()


    End Sub

    Private Sub GetAlt_Peripherals(vOption As String)
        Dim c As New SqlClient.SqlConnection
        Dim cm As New SqlClient.SqlCommand
        Dim rs As SqlClient.SqlDataReader
        Dim iCtr As Integer = 1
        Dim vCheck As String = ""
        Dim vPerpList As String = ""
        Dim vActiveMachList() As String = h_PerpList.Value.ToString.Split(",")
        Dim vSQL As String = ""

        c.ConnectionString = connStr
        c.Open()
        cm.Connection = c

        vAlt_Perp = ""
        vPerpList = ""

        cm.CommandText = "select Item_Cd, Descr from item_master where itemType_Cd='PERP' and Item_Cd <> '" & txtPerp_Cd.Text & _
            "' and Item_Cd not in (select Item_Cd from bom_peripherals " & _
            "where BOM_Cd=" & Request.Item("pBOM") & " and Revision=" & Request.Item("pBomRev") & " and Mach_Cd='" & txtMachCode.Text & "') " & _
            " order by Descr"

        'Response.Write(cm.CommandText)
        Try
            rs = cm.ExecuteReader
            Do While rs.Read

                If Request.Item(rs("Item_Cd")) = "on" Then
                    vCheck = " checked='checked' "
                    vPerpList += rs("Item_Cd") & ","
                Else
                    For iLoop = 0 To UBound(vActiveMachList)
                        If vActiveMachList(iLoop) = rs("Item_Cd") Then
                            vCheck = " checked='checked' "
                        End If
                    Next
                End If

                vAlt_Perp += "<tr>"
                vAlt_Perp += "<td style='width:10px;'><input type='checkbox' id='' name='" & rs("Item_Cd") & "'" & vCheck & " /></td><td>" & rs("Descr") & "</td>"
                vAlt_Perp += "</tr>"
                iCtr += 1
                vCheck = ""
            Loop
            rs.Close()
        Catch ex As SqlClient.SqlException
            Response.Write("Error in SQL query Get All Alternate Peripherals:  " & ex.Message)
        End Try


        If vPerpList <> "" And vOption = "Save" Then
            vPerpList = Mid(vPerpList, 1, Len(vPerpList) - 1)
            vSQL = "update bom_peripherals set Alt_Perp='" & vPerpList & "' where BOM_Cd=" & Request.Item("pBom") & " and Revision=" & Request.Item("pBomRev") & "  "
            If Request.Item("mode") = "Edit" Then
                vSQL += "and TranId='" & Request.Item("pPerpTranId") & "'"
            Else
                vSQL += "and Sect_Cd='" & Request.Item("pSect") & "' and Proc_Cd='" & Request.Item("pProc") & "' and Mach_Cd='" & txtMachCode.Text & "' and Item_Cd='" & txtPerp_Cd.Text & "' "
            End If
            cm.CommandText = vSQL
            cm.ExecuteNonQuery()
        End If

        c.Close()
        c.Dispose()
        cm.Dispose()
    End Sub
End Class
