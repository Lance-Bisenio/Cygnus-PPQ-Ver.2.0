Imports denaro
Partial Class global_forms_confirm
    Inherits System.Web.UI.Page
    Public vScript As String = ""
    
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Session("uid") = Nothing Or Session("uid") = "" Then
            vScript = "alert('Login session has expired. Please re-login again.'); window.opener.document.form1.submit(); window.close();"
            Exit Sub
        End If

        txtTitle.Text = Request.Item("pTitle")
         
        If Not IsPostBack Then  
            Select Case Request.Item("pWinType")
                Case "confirm" 
                    If Request.Item("pMess") <> "BOM" Then
                        If Request.Item("pTranId") = "" Then
                            vScript = "alert('No selected " & Request.Item("pMess") & ".');  window.close();"
                            Exit Sub
                        End If
                    End If

                    txtReason.Visible = False
                    lblMess.Text = "Are you sure you want to delete this " & Request.Item("pMess") & "?"
                    vScript = " window.resizeTo(500, 250);  window.focus();"
                    cmdSave.Text = "YES"

                Case "remarks" 
                    txtReason.Visible = True
                    lblMess.Text = "Please enter remarks " ' & Request.Item("pMess")
                    vScript = " window.resizeTo(500, 360);  window.focus();"
            End Select
        End If

    End Sub

    Protected Sub cmdSave_Click(sender As Object, e As EventArgs) Handles cmdSave.Click
        Select Case Request.Item("pType")
            Case "Delete_Bom"
                Delete_Bom()
            Case "Duplicate_Bom"
                DuplicateBOM()
            Case "Delete_Item"
                Delete_MasterItem()
            Case "Delete_Emp"
                Delete_MasterEmployee()
            Case "Delete_Sfg"
                Delete_SFGMaterials()
        End Select
        'vScript = "alert('Successfully saved.'); window.opener.document.form1.h_Mode.value='Search'; window.opener.document.form1.submit(); window.close();"
    End Sub

    Private Sub Delete_SFGMaterials()
        Dim c As New SqlClient.SqlConnection
        Dim cm As New SqlClient.SqlCommand

        c.ConnectionString = connStr
        c.Open()
        cm.Connection = c

        cm.CommandText = "update item_sfg set IsActive=3 where TranId='" & Request.Item("pTranId") & "'"
        cm.ExecuteNonQuery()
        'Response.Write(cm.CommandText)
        vScript = "alert('Successfully Deleted.'); window.opener.document.form1.h_Mode.value='reload'; window.opener.document.form1.submit(); window.close();"

        c.Close()
        c.Dispose()
        cm.Dispose()
    End Sub

    Private Sub DuplicateBOM()
        Dim c As New SqlClient.SqlConnection
        Dim cm As New SqlClient.SqlCommand
        Dim cmRef_a As New SqlClient.SqlCommand
        Dim cmRef_b As New SqlClient.SqlCommand
        Dim rs As SqlClient.SqlDataReader
        Dim rs_a As SqlClient.SqlDataReader
        'Dim rs_b As SqlClient.SqlDataReader
        Dim vSQL As String = ""
        Dim vBomVer As Integer = CInt(Int(Request.Item("pBom")))
        Dim vMachList As String = ""

        c.ConnectionString = connStr
        c.Open()
        cm.Connection = c
        cmRef_a.Connection = c
        cmRef_b.Connection = c

        cm.CommandText = "select Revision from bom_header where Bom_Cd='" & Request.Item("pBom") & "' order by Revision desc"
        rs = cm.ExecuteReader
        If rs.Read Then
            vBomVer = IIf(IsDBNull(rs("Revision")), 1, rs("Revision"))
        End If
        rs.Close()

        ' ============================================================================================================================================================================
        ' == 07.22.2014 == DUPLICATE BOM =============================================================================================================================================
        ' ============================================================================================================================================================================

        vSQL = "insert into bom_header (BOM_Cd,Revision,Status_Cd,Item_Cd,Image1,Image2,Image3,ReportTo," & _
                    "CreatedBy,DateCreated,StdQty,StdQtyUOM,NetWeight,NetWeightUOM,StdProdRunDay,StdProdRunTime,ItemUOM,ItemType_Cd,Remarks) " & _
                    "select BOM_Cd," & vBomVer + 1 & ",2,Item_Cd,Image1,Image2,Image3,ReportTo,CreatedBy,'" & Format(Now, "MM/dd/yyyy") & "',StdQty," & _
                    "StdQtyUOM,NetWeight,NetWeightUOM,StdProdRunDay,StdProdRunTime,ItemUOM,ItemType_Cd,'" & txtReason.Text.Trim & "' from bom_header " & _
                    "where bom_Cd=" & Request.Item("pBom") & " and Revision=" & vBomVer 
        cm.CommandText = vSQL

        'Response.Write(Request.Item("txtReason") & " - " & Request.Form("txtReason") & " - " & txtReason.Text)
        Try
            cm.ExecuteNonQuery()
        Catch ex As SqlClient.SqlException
            Response.Write("Error in SQL Bom Header:  " & ex.Message)
        End Try

        ' ============================================================================================================================================================================
        ' == 07.22.2014 == DUPLICATE ALL PROCESS BY BOM CODE AND REVISION ============================================================================================================
        ' ============================================================================================================================================================================

        vSQL = "insert into bom_process (BOM_Cd,Revision,Sect_Cd,Proc_Cd,OperOrder,SFG_Cd,SFGUOM,QtyOut,QtyOutUOM,QA_Cd,StartUpSDurDays," & _
                    "StartUpSDurTime,InitRunSDurDays,InitRunSDurTime,ProdRunDurDays,ProdRunDurTime,WithContainer,ContainerUOM,CreatedBy,DateCreated)" & _
                    "select BOM_Cd," & vBomVer + 1 & ",Sect_Cd,Proc_Cd,OperOrder,SFG_Cd,SFGUOM,QtyOut,QtyOutUOM,QA_Cd,StartUpSDurDays," & _
                    "StartUpSDurTime,InitRunSDurDays,InitRunSDurTime,ProdRunDurDays,ProdRunDurTime,WithContainer,ContainerUOM,'" & Session("uid") & "','" & Format(Now, "MM/dd/yyyy") & "' " & _
                    "from bom_process where Bom_Cd=" & Request.Item("pBom") & " and Revision=" & vBomVer
        'Response.Write(vSQL)
        cm.CommandText = vSQL
        Try
            cm.ExecuteNonQuery()
        Catch ex As SqlClient.SqlException
            Response.Write("Error in SQL Process:  " & ex.Message)
        End Try


        ' ============================================================================================================================================================================
        ' == 07.22.2014 == DUPLICATE ALL MATERIALS AND MACHINE PER PROCESS ===========================================================================================================
        ' ============================================================================================================================================================================

        cmRef_a.CommandText = "select a.TranId, " & _
            "(select b.TranId from bom_process b where a.OperOrder=b.OperOrder and a.SFG_Cd=b.SFG_Cd and b.BOM_Cd=" & Request.Item("pBom") & " and b.Revision=" & vBomVer & ") as oldTranId " & _
            "from bom_process a where a.BOM_Cd=" & Request.Item("pBom") & " and a.Revision=" & vBomVer + 1 & ""
        'Response.Write(cmRef_a.CommandText)
        rs_a = cmRef_a.ExecuteReader
        Do While rs_a.Read

            vSQL = "insert into bom_materials (BOM_Cd,Parent_TranId,Revision,Sect_Cd,Proc_Cd,Item_Cd,ItemUOM,Alternative_Item_Cd,Qty,DrawnFrom,CreatedBy,DateCreated) " & _
                    "select BOM_Cd," & rs_a("TranId") & "," & vBomVer + 1 & ",Sect_Cd,Proc_Cd,Item_Cd,ItemUOM,Alternative_Item_Cd,Qty,DrawnFrom,'" & _
                        Session("uid") & "','" & Format(Now, "MM/dd/yyyy") & "' " & _
                    "from bom_materials where Bom_Cd=" & Request.Item("pBom") & " and Revision=" & vBomVer & " and Parent_TranId=" & rs_a("oldTranId")
            cm.CommandText = vSQL
            Try
                cm.ExecuteNonQuery()
            Catch ex As SqlClient.SqlException
                Response.Write("Error in SQL Materials:  " & ex.Message)
            End Try

            vSQL = "insert into bom_machine (BOM_Cd,Parent_TranId,Revision,Sect_Cd,Proc_Cd,Mach_Cd,CapUnit,UOM_Cd,StdLeadDay,StdLeadTime,[Default],CreatedBy,DateCreated,Alt_Mach,Mach_Perp) " & _
                    "select BOM_Cd," & rs_a("TranId") & "," & vBomVer + 1 & ",Sect_Cd,Proc_Cd,Mach_Cd,CapUnit,UOM_Cd,StdLeadDay,StdLeadTime,[Default],'" & _
                        Session("uid") & "','" & Format(Now, "MM/dd/yyyy") & "',Alt_Mach,Mach_Perp " & _
                    "from bom_machine where Bom_Cd=" & Request.Item("pBom") & " and Revision=" & vBomVer & " and Parent_TranId=" & rs_a("oldTranId")
            cm.CommandText = vSQL

            Try
                cm.ExecuteNonQuery()
            Catch ex As SqlClient.SqlException
                Response.Write("Error in SQL Machine :  " & ex.Message)
            End Try

            vSQL = ""

        Loop
        rs_a.Close()

        ' ============================================================================================================================================================================
        ' == 07.22.2014 == DUPLICATE ALL PERIPHERALS PER MACHINE =====================================================================================================================
        ' ============================================================================================================================================================================

        vSQL = "select TranId, Mach_Perp from bom_machine where Bom_Cd=" & Request.Item("pBom") & " and Revision=" & vBomVer + 1 & ""
        cmRef_a.CommandText = vSQL
        rs_a = cmRef_a.ExecuteReader
        Do While rs_a.Read

            If Not IsDBNull(rs_a("Mach_Perp")) Then
                vSQL = "insert into bom_peripherals (BOM_Cd,Parent_TranId,Revision,Sect_Cd,Proc_Cd,Mach_Cd,Item_Cd,DrawnFrom,CreatedBy,DateCreated,Alt_Perp) " & _
                            "select BOM_Cd," & rs_a("TranId") & "," & vBomVer + 1 & ",Sect_Cd,Proc_Cd,Mach_Cd,Item_Cd,DrawnFrom,'" & Session("uid") & "','" & Format(Now, "MM/dd/yyyy") & "',Alt_Perp " & _
                            "from bom_peripherals where Bom_Cd=" & Request.Item("pBom") & " and Revision=" & vBomVer & " and TranId in (" & rs_a("Mach_Perp") & ")"
                cm.CommandText = vSQL

                Try
                    cm.ExecuteNonQuery()
                Catch ex As SqlClient.SqlException
                    Response.Write("Error in SQL query Duplicate Peripherals per Machine :  " & ex.Message)
                End Try
            End If

        Loop
        rs_a.Close()

        c.Close()
        c.Dispose()
        cm.Dispose()
        cmRef_a.Dispose()
        cmRef_b.Dispose()


    End Sub

    Private Sub Delete_Bom()
        Dim c As New SqlClient.SqlConnection
        Dim cm As New SqlClient.SqlCommand
        Dim rs As SqlClient.SqlDataReader

        Dim vFilter As String = ""

        c.ConnectionString = connStr
        c.Open()
        cm.Connection = c

        vFilter = "where Bom_Cd=" & Request.Item("pBom") & " and Revision=" & Request.Item("pBomRev")

        Select Case Request.Item("pMess")
            Case "BOM"
                'cm.CommandText = "delete from bom_header " & vFilter
                'cm.ExecuteNonQuery()
                'cm.CommandText = "delete from bom_process " & vFilter
                'cm.ExecuteNonQuery()   
                'cm.CommandText = "delete from bom_materials " & vFilter
                'cm.ExecuteNonQuery()
                'cm.CommandText = "delete from bom_machine " & vFilter
                'cm.ExecuteNonQuery()
                'cm.CommandText = "delete from bom_peripherals " & vFilter
                'cm.ExecuteNonQuery()

                cm.CommandText = "Update bom_header set Status_Cd=3 " & vFilter
                cm.ExecuteNonQuery() 
                vScript = "alert('Successfully saved.'); window.opener.document.form1.h_Mode.value='Search'; window.opener.document.form1.submit(); window.close();"
                 
            Case "Process"
                Dim vParent As String = "" 
                cm.CommandText = "delete from bom_process " & vFilter & " and TranId=" & Request.Item("pTranId")
                cm.ExecuteNonQuery()
                cm.CommandText = "delete from bom_materials " & vFilter & "and Parent_TranId=" & Request.Item("pTranId")
                cm.ExecuteNonQuery()
                cm.CommandText = "delete from bom_machine " & vFilter & "and Parent_TranId=" & Request.Item("pTranId")
                cm.ExecuteNonQuery()
                 
                cm.CommandText = "select TranId from bom_machine " & vFilter & " and Parent_Id=" & Request.Item("pTranId")
                Try
                    rs = cm.ExecuteReader
                    If rs.Read Then
                        vParent = rs("TranId")
                    End If
                    rs.Close()
                Catch ex As SqlClient.SqlException
                    Response.Write("Error in SQL query get the pareherals parent from machine:  " & ex.Message)
                End Try

                If vParent <> "" Then
                    cm.CommandText = "delete from bom_peripherals " & vFilter & " and Parent_Id=" & vParent
                    cm.ExecuteNonQuery()
                End If 
                vScript = "alert('Successfully saved.');  window.opener.document.form1.h_Mode.value='ViewDetails'; window.opener.document.form1.submit(); window.close();"



            Case "Materials"
                cm.CommandText = "delete from bom_materials " & vFilter & "and TranId=" & Request.Item("pTranId")
                cm.ExecuteNonQuery()
                vScript = "alert('Successfully saved.'); window.opener.document.form1.h_Mode.value='ProcessDetails'; window.opener.document.form1.submit(); window.close();"



            Case "Machines"
                cm.CommandText = "delete from bom_machine " & vFilter & " and TranId=" & Request.Item("pTranId")
                cm.ExecuteNonQuery()
                cm.CommandText = "delete from bom_peripherals " & vFilter & " and Parent_TranId=" & Request.Item("pTranId")
                cm.ExecuteNonQuery()
                vScript = "alert('Successfully saved.'); window.opener.document.form1.h_Mode.value='ProcessDetails'; window.opener.document.form1.submit(); window.close();"



            Case "Peripherals"
                cm.CommandText = "delete from bom_peripherals " & vFilter & " and TranId=" & Request.Item("pTranId")
                cm.ExecuteNonQuery()
                vScript = "alert('Successfully saved.'); window.opener.document.form1.h_Mode.value='MachineDetails'; window.opener.document.form1.submit(); window.close();"
        End Select

        c.Close()
        c.Dispose()
        cm.Dispose()
    End Sub

    Private Sub Delete_MasterItem()
        Dim c As New SqlClient.SqlConnection
        Dim cm As New SqlClient.SqlCommand 
         
        c.ConnectionString = connStr
        c.Open()
        cm.Connection = c
         
        cm.CommandText = "update item_master set IsActive=3 where Item_Cd='" & Request.Item("pTranId") & "'"
        cm.ExecuteNonQuery()
        vScript = "alert('Successfully Deleted.'); window.opener.document.form1.h_Mode.value='reload'; window.opener.document.form1.submit(); window.close();"

        c.Close()
        c.Dispose()
        cm.Dispose()
    End Sub

    Private Sub Delete_MasterEmployee()
        Dim c As New SqlClient.SqlConnection
        Dim cm As New SqlClient.SqlCommand

        c.ConnectionString = connStr
        c.Open()
        cm.Connection = c

        cm.CommandText = "delete from emp_master where Emp_Cd='" & Request.Item("pTranId") & "'"
        cm.ExecuteNonQuery()
        vScript = "alert('Successfully Deleted.'); window.opener.document.form1.h_Mode.value='reload'; window.opener.document.form1.submit(); window.close();"

        c.Close()
        c.Dispose()
        cm.Dispose()
    End Sub
End Class