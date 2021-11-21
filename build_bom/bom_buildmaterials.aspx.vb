Imports denaro
Partial Class bom_buildmaterials
    Inherits System.Web.UI.Page
    Public vScript As String = ""
    Public vAlt_Materials As String = ""
    Dim vSQL As String = ""

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        If Session("uid") = Nothing Or Session("uid") = "" Then
            vScript = "alert('Login session has expired. Please re-login again.'); window.opener.document.form1.h_Mode.value='reload'; window.opener.document.form1.submit(); window.close();"
            Exit Sub
        End If

        If Request.Item("pMatTranId") = "" And Request.Item("mode") = "Edit" Then
            vScript = "alert('Please select item to edit.'); window.opener.document.form1.h_Mode.value='reload'; window.opener.document.form1.submit(); window.close();"
            Exit Sub
        End If

        If Not IsPostBack Then
            'BuildCombo("select Item_Cd,Descr from item_master where Item_Cd not in ( " & _
            '    "select Item_Cd from bom_materials where bom_materials.Item_Cd=item_master.Item_Cd and BOM_Cd=" & Request.Item("pBom") & " and Revision=" & Request.Item("pBomRev") & " and " & _
            '    "Proc_Cd='" & Request.Item("vProc") & "' and Sect_Cd='" & Request.Item("pSect") & "' and " & _
            '    "ItemType_Cd='RM' ) order by Descr", cmbMat)  'and Item_Cd <> '" & Request.Item("vMat") & "')

            vSQL = "select Item_Cd, Descr + ' - ' + ItemType_Cd as ItemDescr " _
                & "from item_master where IsActive=1 order by ItemType_Cd,Descr"
            ' and ItemType_Cd in ('RM',)
            BuildCombo(vSQL, cmbMat)

            cmbMat.Items.Add(" ")
            cmbMat.SelectedValue = " "
            BuildCombo("select Uom_Cd, Descr from ref_item_uom  order by Descr", cmbMatUOM)
            cmbMatUOM.Items.Add(" ")
            cmbMatUOM.SelectedValue = "22"

            Dim c As New SqlClient.SqlConnection
            Dim cm As New SqlClient.SqlCommand
            Dim rs As SqlClient.SqlDataReader
            Dim vMachList As String = ""

            c.ConnectionString = connStr
            c.Open()
            cm.Connection = c

            ' BOM HEADER INFORMATION 
            ' -----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            cm.CommandText = "Select Item_Cd," & _
                    "(Select Descr from item_master where item_master.Item_Cd=bom_header.Item_Cd) As Item_Name " & _
                    "from bom_header where BOM_Cd=" & Request.Item("pBom") & " And Revision=" & Request.Item("pBomRev") & ""
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
                Response.Write("Error In SQL query Get BOM Header Details:  " & ex.Message)
            End Try

            ' BOM PROCESS INFORMATION 
            ' -----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            cm.CommandText = "select *," & _
                "(select Descr from item_master where SFG_Cd=Item_Cd) as vSFGDesc," & _
                "(select Descr from ref_emp_section where ref_emp_section.Section_Cd=Sect_Cd) as vSectName," & _
                "(select Descr from ref_item_process where ref_item_process.Proc_Cd=bom_process.Proc_Cd ) as vProcName " & _
                "from bom_process where BOM_Cd=" & Request.Item("pBom") & " and Revision=" & Request.Item("pBomRev") & " and TranId='" & Request.Item("pProTranId") & "'"
            Try
                rs = cm.ExecuteReader
                If rs.Read Then
                    txtBOM_Process.Text = IIf(IsDBNull(rs("vProcName")), "", rs("vProcName"))
                    txtBOM_Section.Text = IIf(IsDBNull(rs("vSectName")), "", rs("vSectName"))
                    txtSFGCode.Text = IIf(IsDBNull(rs("SFG_Cd")), "", rs("SFG_Cd"))
                    txtSFGDescr.Text = IIf(IsDBNull(rs("SFG_Descr")), "", rs("SFG_Descr"))
                End If
                rs.Close()
            Catch ex As SqlClient.SqlException
                Response.Write("Error in SQL query Get BOM Process Details:  " & ex.Message)
            End Try
             
            c.Close()
            c.Dispose()
            cm.Dispose()

            If Request.Item("mode") = "Edit" Or Request.Item("mode") = "View" Then
                GetBOM_Materials_Details()
            End If
            'GetAlt_Materials("reload")
        End If

        If Request.Item("mode") = "View" Then
            vScript = "$('#cmdSave').hide(); $('#cmdGetItem').hide(); "
        End If

    End Sub

    Private Sub GetAlt_Materials(vOption As String)

        Dim c As New SqlClient.SqlConnection
        Dim cm As New SqlClient.SqlCommand
        Dim rs As SqlClient.SqlDataReader
        Dim iCtr As Integer = 1
        Dim vCheck As String = ""
        Dim vMachList As String = ""
        Dim vActiveMachList() As String = h_MatList.Value.ToString.Split(",")
        Dim vSQL As String = ""

        c.ConnectionString = connStr
        c.Open()
        cm.Connection = c

        vAlt_Materials = ""
        vMachList = ""

        If Request.Item("mode") = "new" Then
            cm.CommandText = "select QtyUOM_Cd from item_master where Item_Cd='" & txtMat_Cd.Text & "'"
            rs = cm.ExecuteReader
            If rs.Read Then
                cmbMatUOM.SelectedValue = IIf(IsDBNull(rs("QtyUOM_Cd")), " ", rs("QtyUOM_Cd"))
            End If
            rs.Close()
        End If

        cm.CommandText = "select Item_Cd," _
                & "(Descr + ' - ' + ItemType_Cd) as ItemDescr, ItemType_Cd " _
                & "from item_master where Item_Cd not in (" _
                    & "select Item_Cd from bom_materials where bom_materials.Item_Cd=item_master.Item_Cd and " _
                    & "BOM_Cd=" & Request.Item("pBom") & " and " _
                    & "Revision=" & Request.Item("pBomRev") & " and " _
                    & "Proc_Cd='" & Request.Item("vProc") & "' and Sect_Cd='" & Request.Item("pSect") & "' " _
                    & "and ItemType_Cd='RM') and IsActive=1 order by ItemType_Cd, ItemDescr "

        'Response.Write(cm.CommandText)
        Try
            rs = cm.ExecuteReader
            Do While rs.Read

                If Request.Item(rs("Item_Cd")) = "on" Then
                    vCheck = " checked='checked' "
                    vMachList += rs("Item_Cd") & ","
                Else
                    For iLoop = 0 To UBound(vActiveMachList)
                        If vActiveMachList(iLoop) = rs("Item_Cd") Then
                            vCheck = " checked='checked' "
                        End If
                    Next
                End If

                vAlt_Materials += "<tr>"
                vAlt_Materials += "<td style='width:10px; padding-left:5px; padding-right:5px;'>" _
                    & "<input type='checkbox' id='' name='" & rs("Item_Cd") & "'" & vCheck & " /></td>" _
                    & "<td style='font-size:10px;'>" & rs("ItemDescr") & "</td>"
                vAlt_Materials += "</tr>"
                iCtr += 1
                vCheck = ""
            Loop
            rs.Close()
        Catch ex As SqlClient.SqlException
            Response.Write("Error in SQL query Get All Alternative Materials:  " & ex.Message)
        End Try

        If vMachList <> "" And vOption = "Save" Then
            vMachList = Mid(vMachList, 1, Len(vMachList) - 1)
            vSQL = "update bom_materials set Alternative_Item_Cd='" & vMachList & "' where " _
                & "BOM_Cd=" & Request.Item("pBom") & " And " _
                & "Revision = " & Request.Item("pBomRev") & " And " _
                & "Parent_Tranid='" & Request.Item("pProTranId") & "' "

            If Request.Item("mode") = "Edit" Then
                vSQL += "and TranId='" & Request.Item("pMatTranId") & "'"
            Else
                vSQL += "and Sect_Cd='" & Request.Item("pSect") & "' and Proc_Cd='" & Request.Item("pProc") & "' and Item_Cd='" & txtMat_Cd.Text & "'"
            End If

            cm.CommandText = vSQL
            'Response.Write(cm.CommandText)
            Try 
                cm.ExecuteNonQuery()
            Catch ex As SqlClient.SqlException
                Response.Write("Error in SQL query Update Alternative Materials: " & ex.Message)
            End Try
        End If

        c.Close()
        c.Dispose()
        cm.Dispose()
    End Sub

    Private Sub GetBOM_Materials_Details()
        Dim c As New SqlClient.SqlConnection
        Dim cm As New SqlClient.SqlCommand
        Dim rs As SqlClient.SqlDataReader

        c.ConnectionString = connStr
        c.Open()
        cm.Connection = c

        cm.CommandText = "select * from bom_materials " & _
            "where BOM_Cd=" & Request.Item("pBom") & " and Revision=" & Request.Item("pBomRev") & " and TranId='" & Request.Item("pMatTranId") & "'"
        'Response.Write(cm.CommandText)
        Try
            rs = cm.ExecuteReader
            If rs.Read Then
                txtMat_Cd.Text = rs("Item_Cd")
                cmbMat.SelectedValue = rs("Item_Cd")

                txtOutput_Qty.Text = rs("Qty")
                cmbMatUOM.SelectedValue = IIf(IsDBNull(rs("ItemUOM")), " ", rs("ItemUOM"))
                rdoDrawnFrom.SelectedValue = rs("DrawnFrom")
                h_MatList.Value = IIf(IsDBNull(rs("Alternative_Item_Cd")), " ", rs("Alternative_Item_Cd"))

                txtGrams.Text = IIf(IsDBNull(rs("Grams")), "", rs("Grams"))
                txtPerc.Text = IIf(IsDBNull(rs("Percentage")), "", rs("Percentage"))
            End If
            rs.Close()

        Catch ex As SqlClient.SqlException
            Response.Write("Error in SQL query Get BOM Materials Details: " & ex.Message)
        End Try

        c.Close()
        c.Dispose()
        cm.Dispose()
    End Sub

    Protected Sub cmbMat_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbMat.SelectedIndexChanged
        txtMat_Cd.Text = cmbMat.SelectedValue
        txtGrams.Text = GetRef("select Grams from item_sfg where IsActive=1 and Mat_ItemCd='" & cmbMat.SelectedValue & "'", "")
        txtPerc.Text = GetRef("select Percentage from item_sfg where IsActive=1 and Mat_ItemCd='" & cmbMat.SelectedValue & "'", "")

        txtGrams.Text = IIf(txtGrams.Text.Trim = "", "0", txtGrams.Text)
        txtPerc.Text = IIf(txtPerc.Text.Trim = "", "0", txtPerc.Text)

        'GetAlt_Materials("reload")
    End Sub

    Protected Sub cmdSave_Click(sender As Object, e As EventArgs) Handles cmdSave.Click
        Save()
        'GetAlt_Materials("Save")
        vScript = "alert('Successfully saved.'); window.opener.document.getElementById('h_Mode').value='ProcessDetails'; window.opener.document.forms['form1'].submit(); window.close();"
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
			vSQL = "update bom_materials set Item_Cd='" & cmbMat.SelectedValue & "', " _
				& "SFG_Code='" & txtSFGCode.Text & "', ItemUOM='" & cmbMatUOM.SelectedValue & "', " _
				& "Qty='" & txtOutput_Qty.Text & "', DrawnFrom='" & rdoDrawnFrom.SelectedValue & "'," _
				& "DateCreated='" & Format(CDate(Now), "MM-dd-yyyy") & "', CreatedBy='" & Session("uid") & "', " _
				& "Grams='" & txtGrams.Text.Trim & "',Percentage='" & txtPerc.Text.Trim & "' " _
				& "where BOM_Cd=" & Request.Item("pBom") & " and Revision=" & Request.Item("pBomRev") & " and " _
				& "TranId='" & Request.Item("pMatTranId") & "'"
		Else
			vSQL = "insert into bom_materials (Parent_TranId,Item_Cd,ItemUOM,BOM_Cd,Revision,Sect_Cd,Proc_Cd,Qty,DrawnFrom," _
				& "CreatedBy, DateCreated,SFG_Code,Grams,Percentage) values ( " _
				& "'" & Request.Item("pProTranId") & "','" & cmbMat.SelectedValue & "','" & cmbMatUOM.SelectedValue & "'," _
				& Request.Item("pBom") & "," & Request.Item("pBomRev") & "," & "'" & Request.Item("pSect") & "','" & Request.Item("pProc") & "','" _
				& txtOutput_Qty.Text & "','" & rdoDrawnFrom.SelectedValue & "'," & "'" & Session("uid") & "', '" & Format(CDate(Now), "MM-dd-yyyy") & "','" _
				& txtSFGCode.Text & "','" & txtGrams.Text.Trim & "','" & txtPerc.Text.Trim & "')"
		End If
        'Response.Write(vSQL)
        cm.CommandText = vSQL 
        Try
            cm.ExecuteNonQuery()
        Catch ex As SqlClient.SqlException
            Response.Write("Error in SQL query insert/update:  " & ex.Message)
        End Try

        c.Close()
        c.Dispose()
        cm.Dispose()

    End Sub

    'Protected Sub cmdGetItem0_Click(sender As Object, e As EventArgs) Handles cmdGetItem0.Click
    '    If txtMat_Cd.Text.Trim = "" Then
    '        BuildCombo("select Item_Cd,Descr from item_master where ItemType_Cd='SFG' order by Descr", cmbMat)
    '    Else
    '        BuildCombo("select Item_Cd,Descr from item_master where ItemType_Cd='SFG' and Item_Cd like '%" & txtMat_Cd.Text.Trim & "%' order by Descr", cmbMat)
    '    End If 
    '    cmbMat.Items.Add(" ")
    '    cmbMat.SelectedValue = " "
    '    GetAlt_Materials("View")
    'End Sub
    Protected Sub cmdGetItem0_Click(sender As Object, e As EventArgs) Handles cmdGetItem0.Click
        Dim vCtr As Integer = 0

        vSQL = "select Item_Cd, Descr + ' - ' + ItemType_Cd as ItemDescr " _
               & "from item_master where IsActive=1 and " _
               & "Item_Cd like '%" & txtMat_Cd.Text.Trim.Replace("'", " ") & "%' order by ItemType_Cd,Descr"
        BuildCombo(vSQL, cmbMat)

        vSQL = "select count(Item_Cd) " _
               & "from item_master where IsActive=1 and " _
               & "Item_Cd like '%" & txtMat_Cd.Text.Trim.Replace("'", " ") & "%'"
        vCtr = GetRef(vSQL, "null")

        cmbMat.Items.Add(vCtr & " Item Found")
        cmbMat.SelectedValue = vCtr & " Item Found"

    End Sub
End Class
