Imports denaro
Partial Class bom_activation
    Inherits System.Web.UI.Page
    Public vScript As String = ""

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Session("uid") = Nothing Or Session("uid") = "" Then
            vScript = "alert('Login session has expired. Please re-login again.'); window.opener.document.form1.h_Mode.value='reload'; window.opener.document.form1.submit(); window.close();"
            Exit Sub
        End If

        If Not IsPostBack Then
 
            txtBOM_Cd.Text = Request.Item("vBom")
            txtBOM_Item.Text = Request.Item("vBomItemCd")
            txtBOM_ItemDescr.Text = Request.Item("vBomItemDescr")
            txtRev.Text = Request.Item("vBomRev")

            If Request.Item("mode") = "edit" Then
                 
            End If

        End If
    End Sub

    Protected Sub cmdSave_Click(sender As Object, e As EventArgs) Handles cmdSave.Click
        Save()
    End Sub

    Private Sub Save()
        ' 
        Dim c As New SqlClient.SqlConnection
        Dim cm As New SqlClient.SqlCommand
        Dim rs As SqlClient.SqlDataReader
        Dim vSQL As String = ""
        Dim vRevision As Integer = 0

        c.ConnectionString = connStr
        c.Open()
        cm.Connection = c

        cm.CommandText = "select Revision from bom_header  where BOM_Cd=" & Request.Item("vBom")
        
        Try
            rs = cm.ExecuteReader
            If rs.Read Then
                vRevision = IIf(IsDBNull(rs("Revision")), 0, rs("Revision")) + 1
            End If
            rs.Close()

            cm.CommandText = "update bom_header set Revision=" & vRevision & ", ActiveBy='" & Session("uid") & "', DateActive='" & Now & "'  where BOM_Cd=" & Request.Item("vBom")
            cm.ExecuteNonQuery()
            cm.CommandText = "update bom_process set Revision=" & vRevision & " where BOM_Cd=" & Request.Item("vBom")
            cm.ExecuteNonQuery()
            cm.CommandText = "update bom_machine set Revision=" & vRevision & " where BOM_Cd=" & Request.Item("vBom")
            cm.ExecuteNonQuery()
            cm.CommandText = "update bom_peripherals set Revision=" & vRevision & " where BOM_Cd=" & Request.Item("vBom")
            cm.ExecuteNonQuery()
            cm.CommandText = "update bom_materials set Revision=" & vRevision & " where BOM_Cd=" & Request.Item("vBom")
            cm.ExecuteNonQuery()

            cm.CommandText = "insert into active_bom_header (BOM_Cd,Revision,ActiveBy,DateActive,Status_Cd,Item_Cd," & _
                "StdQty,StdProdRun,ReportTo,CreatedBy,DateCreated,Remarks) " & _
                "select BOM_Cd,Revision,ActiveBy,DateActive,Status_Cd,Item_Cd,StdQty,StdProdRun,ReportTo,'" & _
                Session("uid") & "','" & Now & "','" & txtRemarks.Text.Trim & "' from bom_header where BOM_Cd=" & Request.Item("vBom")
            cm.ExecuteNonQuery()

            cm.CommandText = "insert into active_bom_process (BOM_Cd,Revision,Sect_Cd,Proc_Cd,OperOrder,SFG_Cd," & _
                "Qty, QA_Cd,StartUpSDur,InitRunSDur,WithContainer,CreatedBy,DateCreated,Remarks) " & _
                "select BOM_Cd, Revision, Sect_Cd,Proc_Cd, OperOrder, SFG_Cd, Qty, QA_Cd,StartUpSDur,InitRunSDur,WithContainer,'" & _
                Session("uid") & "','" & Now & "','" & txtRemarks.Text.Trim & "' from bom_process where BOM_Cd=" & Request.Item("vBom")
            cm.ExecuteNonQuery()

            cm.CommandText = "insert into active_bom_machine (BOM_Cd,Revision,Sect_Cd,Proc_Cd,Mach_Cd,CapUnit,UOM_Cd," & _
                "[Minutes],[Default],Alt_Mach,CreatedBy,DateCreated,Remarks) " & _
                "select BOM_Cd,Revision,Sect_Cd,Proc_Cd,Mach_Cd, CapUnit,UOM_Cd,[Minutes],[Default],Alt_Mach,'" & _
                Session("uid") & "','" & Now & "','" & txtRemarks.Text.Trim & "' from bom_machine where BOM_Cd=" & Request.Item("vBom")
            cm.ExecuteNonQuery()

            cm.CommandText = "insert into active_bom_peripherals (BOM_Cd,Revision,Proc_Cd,Mach_Cd,Item_Cd,DrawnFrom,Alt_Perp,CreatedBy,DateCreated,Remarks) " & _
                "select BOM_Cd,Revision,Proc_Cd,Mach_Cd,Item_Cd,DrawnFrom,Alt_Perp,'" & _
                Session("uid") & "','" & Now & "','" & txtRemarks.Text.Trim & "' from bom_peripherals where BOM_Cd=" & Request.Item("vBom")
            cm.ExecuteNonQuery()

            cm.CommandText = "insert into active_bom_materials (BOM_Cd,Revision,Sect_Cd,Proc_Cd,Item_Cd,Alternative_Item_Cd,Qty,DrawnFrom,CreatedBy,DateCreated,Remarks) " & _
                "select BOM_Cd,Revision,Sect_Cd,Proc_Cd,Item_Cd,Alternative_Item_Cd,Qty,DrawnFrom,'" & _
                Session("uid") & "','" & Now & "','" & txtRemarks.Text.Trim & "' from bom_materials where BOM_Cd=" & Request.Item("vBom")
            cm.ExecuteNonQuery()
            'Response.Write(cm.CommandText)

        Catch ex As SqlClient.SqlException
            Response.Write("Error in SQL:  " & ex.Message)
        End Try

        c.Close()
        c.Dispose()
        cm.Dispose()
         
        vScript = "alert('Successfully saved.'); window.opener.document.form1.h_Mode.value='reload'; window.opener.document.form1.submit(); window.close();"
    End Sub
End Class
