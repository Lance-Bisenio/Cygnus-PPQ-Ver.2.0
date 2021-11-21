Imports denaro
Partial Class bom_buildprocess
    Inherits System.Web.UI.Page
    Public vScript As String = ""
    Public vQA_List As String = ""
    Public vMac_List As String = ""
    Public vMat_List As String = ""


    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Session("uid") = Nothing Or Session("uid") = "" Then
            vScript = "alert('Login session has expired. Please re-login again.'); window.opener.document.form1.h_Mode.value='reload'; window.opener.document.form1.submit(); window.close();"
            Exit Sub
        End If
         
        If Not IsPostBack Then

            txtSFG.Focus()

            BuildCombo("select Section_Cd,Descr from ref_emp_section where Section_Cd<>'99' order by Descr", cmbSection)
            BuildCombo("select Proc_Cd,Descr from ref_item_process where Proc_Cd<>'99' and Sect_Cd='" & cmbSection.SelectedValue & "' order by Descr", cmbProcess)
            'BuildCombo("select Item_Cd,Descr from item_master where ItemType_Cd='SFG' order by Descr", cmbFSG)

            'BuildCombo("select Uom_Cd, Descr from ref_item_uom  order by Descr", cmbSFGUOM)
            BuildCombo("select Uom_Cd, Descr from ref_item_uom  order by Descr", cmbQtyOutUOM)
            BuildCombo("select Uom_Cd, Descr from ref_item_uom  order by Descr", cmbConUOM)

            cmbSection.Items.Add(" ")
            cmbSection.SelectedValue = " "
            cmbProcess.Items.Add(" ")
            cmbProcess.SelectedValue = " "

            cmbQtyOutUOM.Items.Add(" ")
            cmbQtyOutUOM.SelectedValue = "22"

            cmbConUOM.Items.Add(" ")
            cmbConUOM.SelectedValue = "22"

            'cmbFSG.Items.Add(" ")
            'cmbFSG.SelectedValue = " "

            Dim c As New SqlClient.SqlConnection
            Dim cm As New SqlClient.SqlCommand
            Dim rs As SqlClient.SqlDataReader
            Dim vOperNo As Integer = 10

            c.ConnectionString = connStr
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

            ' BOM HEADER INFORMATION 
            ' -----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            cm.CommandText = "select BOM_Cd,Revision,Item_Cd," & _
                    "(select Descr from item_master where item_master.Item_Cd=bom_header.Item_Cd) as Item_Name " & _
                    "from bom_header where BOM_Cd=" & Request.Item("pBom") & " and Revision=" & Request.Item("pBomRev")

            Try
                rs = cm.ExecuteReader
                If rs.Read Then
                    txtBOM_Rev.Text = IIf(IsDBNull(rs("Revision")), "Auto Geneted", rs("Revision"))
                    txtBOM_Item.Text = IIf(IsDBNull(rs("Item_Name")), " ", rs("Item_Name"))
                    txtBOM_Cd.Text = IIf(IsDBNull(rs("BOM_Cd")), " ", rs("BOM_Cd"))
                    txtItem_Cd.Text = IIf(IsDBNull(rs("Item_Cd")), " ", rs("Item_Cd"))
                End If
                rs.Close()

                cm.CommandText = "select MAX(OperOrder) as OperatioNo  from bom_process where BOM_Cd=" & Request.Item("pBom") & " and Revision=" & Request.Item("pBomRev")
                rs = cm.ExecuteReader
                If rs.Read Then
                    If Not IsDBNull(rs("OperatioNo")) Then
                        vOperNo = rs("OperatioNo") + 10
                    End If
                End If
                rs.Close()
                txtOperatingOrderNo.Text = vOperNo
            Catch ex As SqlClient.SqlException
                Response.Write("Error in SQL query Get BOM Header Details function:  " & ex.Message)
            End Try

            c.Close()
            c.Dispose()
            cm.Dispose()


            For i = 0 To 59
                cmbSRDMins.Items.Add(Format(i, "00"))
                cmbIRDMins.Items.Add(Format(i, "00"))
                cmbPRDMins.Items.Add(Format(i, "00"))
            Next
 

            If Request.Item("mode") = "Edit" Or Request.Item("mode") = "View" Then

                cmbSection.SelectedValue = Request.Item("pSect")
                BuildCombo("select Proc_Cd,Descr from ref_item_process where Proc_Cd<>'99' and Sect_Cd='" & cmbSection.SelectedValue & "' order by Descr", cmbProcess)
                cmbProcess.SelectedValue = Request.Item("vProc")
                GetProcess_Details()
            End If

            'getQA_List() 
            'GetAllMaterials()

        End If

        If Request.Item("mode") = "View" Then 
            vScript = "$('#cmdSave').hide(); $('#cmdGetItem').hide(); "
        End If

    End Sub

    Private Sub GetProcess_Details()  
        Dim c As New SqlClient.SqlConnection
        Dim cm As New SqlClient.SqlCommand
        Dim rs As SqlClient.SqlDataReader
         
        c.ConnectionString = connStr
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
        cm.CommandText = "select * from bom_process where " & _
            "BOM_Cd=" & Request.Item("pBom") & " and Revision=" & Request.Item("pBomRev") & " and TranId=" & Request.Item("pProTranId") & " "

        'Response.Write(cm.CommandText)
        Try
            rs = cm.ExecuteReader
            If rs.Read Then
                h_TranId.Value = rs("TranId")
                h_QA_list.Value += rs("QA_Cd")
                txtOperatingOrderNo.Text = rs("OperOrder")
                txtQty.Text = rs("QtyOut")
                cmbQtyOutUOM.SelectedValue = IIf(IsDBNull(rs("QtyOutUOM")), "", rs("QtyOutUOM"))
                 
                txtSDTime.Text = IIf(IsDBNull(rs("StartUpRun_Hrs")), "00", rs("StartUpRun_Hrs"))
                txtIRDTime.Text = IIf(IsDBNull(rs("InitialRun_Hrs")), "00", rs("InitialRun_Hrs"))
                txtPRDTime.Text = IIf(IsDBNull(rs("ProdRun_Hrs")), "00", rs("ProdRun_Hrs"))

                cmbSRDMins.SelectedValue = IIf(IsDBNull(rs("StartUpRun_Mins")), "00", rs("StartUpRun_Mins"))
                cmbIRDMins.SelectedValue = IIf(IsDBNull(rs("InitialRun_Mins")), "00", rs("InitialRun_Mins"))
                cmbPRDMins.SelectedValue = IIf(IsDBNull(rs("ProdRun_Mins")), "00", rs("ProdRun_Mins"))

                rdoWithContainer.SelectedValue = rs("WithContainer")
                'txtSFG.Text = IIf(IsDBNull(rs("SFG_Cd")), "", rs("SFG_Cd"))
                h_SFGCode.value = IIf(IsDBNull(rs("SFG_Cd")), "", rs("SFG_Cd"))
                'cmbFSG.SelectedValue = IIf(IsDBNull(rs("SFG_Cd")), "", rs("SFG_Cd"))
                cmbConUOM.SelectedValue = IIf(IsDBNull(rs("ContainerUOM")), "", rs("ContainerUOM"))

                txtKilo.Text = IIf(IsDBNull(rs("kilos")), "0", rs("kilos"))
                txtMeter.Text = IIf(IsDBNull(rs("Meters")), "0", rs("Meters"))

            End If
            rs.Close()

        Catch ex As SqlClient.SqlException
            Response.Write("Error in SQL query Get Process Details function:  " & ex.Message)
        End Try
         
        c.Close()
        c.Dispose()
        cm.Dispose()

    End Sub

    'Private Sub getQA_List()
    '    Dim c As New SqlClient.SqlConnection
    '    Dim cm As New SqlClient.SqlCommand
    '    Dim rs As SqlClient.SqlDataReader
    '    Dim iCtr As Integer = 1
    '    Dim vCheck As String = ""
    '    Dim vQACodeList As String = ""
    '    Dim vQAList() As String = h_QA_list.Value.ToString.Split(",")
    '    Dim vSQL As String = ""

    '    c.ConnectionString = connStr
    '    Try
    '        c.Open()
    '    Catch ex As SqlClient.SqlException
    '        vScript = "alert('Error occurred while trying to connect to database. Error is: " & _
    '            ex.Message.Replace(vbCrLf, "").Replace("'", "") & "');"
    '        c.Dispose()
    '        cm.Dispose()
    '        Exit Sub
    '    End Try
    '    cm.Connection = c

    '    vQA_List = ""
    '    vQACodeList = ""

    '    cm.CommandText = "select QA_Cd, Descr from ref_item_qualityassurance order by Descr"
    '    rs = cm.ExecuteReader
    '    Do While rs.Read

    '        If Request.Item(rs("QA_Cd") & "_" & iCtr) = "on" Then
    '            vCheck = " checked='checked' "
    '            vQACodeList += rs("QA_Cd") & ","
    '        Else
    '            For iLoop = 0 To UBound(vQAList)
    '                If vQAList(iLoop) = rs("QA_Cd") Then
    '                    vCheck = " checked='checked' "
    '                End If
    '            Next
    '        End If

    '        vQA_List += "<tr>"
    '        vQA_List += "<td style='width:10px;'><input type='checkbox' id='' name='" & rs("QA_Cd") & "_" & iCtr & "'" & vCheck & " /></td><td>" & rs("Descr") & "</td>"
    '        vQA_List += "</tr>"
    '        iCtr += 1
    '        vCheck = ""
    '    Loop
    '    rs.Close()

    '    If vQACodeList <> "" Then
    '        vQACodeList = Mid(vQACodeList, 1, Len(vQACodeList) - 1)
    '        vSQL = "update bom_process set QA_Cd='" & vQACodeList & "' "

    '        If Request.Item("mode") = "Edit" Then
    '            vSQL += "where BOM_Cd=" & Request.Item("pBom") & " and Revision=" & Request.Item("pBomRev") & " and TranId='" & Request.Item("pProTranId") & "' "
    '        Else
    '            vSQL += "where BOM_Cd=" & Request.Item("pBom") & " and Revision=" & Request.Item("pBomRev") & _
    '                " and Sect_Cd='" & cmbSection.SelectedValue & "' and Proc_Cd='" & cmbProcess.SelectedValue & "'"
    '        End If

    '        'Response.Write(vSQL)
    '        cm.CommandText = vSQL
    '        cm.ExecuteNonQuery()
    '    End If

    '    c.Close()
    '    c.Dispose()
    '    cm.Dispose()
    'End Sub

    Protected Sub cmdSave_Click(sender As Object, e As EventArgs) Handles cmdSave.Click
        Save() 'h_Mode
        vScript = "alert('Successfully saved.'); window.opener.document.getElementById('h_Mode').value='ViewDetails'; window.opener.document.forms['form1'].submit();  window.close();"

    End Sub

    Private Sub Save()
        Dim c As New SqlClient.SqlConnection
        Dim cm As New SqlClient.SqlCommand
        Dim rs As SqlClient.SqlDataReader
        Dim vSQL As String = ""
        Dim vSFGUom As String = ""
        Dim vParentId As Integer = 0

        Dim vStartUpRun_Hrs As String = ""
        Dim vInitialRun_Hrs As String = ""
        Dim vProdRun_Hrs As String = ""

        Dim vStartUpRun_Mins As String = ""
        Dim vInitialRun_Mins As String = ""
        Dim vProdRun_Mins As String = ""

        Dim vSFGCode As String = txtItem_Cd.Text.Trim & "-" & txtOperatingOrderNo.Text.Trim
        Dim vSFGDescr As String = txtBOM_Item.Text.Trim & "\" _
            & txtOperatingOrderNo.Text.Trim & "\" _
            & cmbSection.SelectedItem.Text & "\" _
            & cmbProcess.SelectedItem.Text

        If txtSFG.Text.Trim <> "" Then
            vSFGDescr += "\" & txtSFG.Text.Trim
        End If

        c.ConnectionString = connStr
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

        'cm.CommandText = "select QtyUOM_Cd from item_master where item_Cd='" & cmbFSG.SelectedValue & "'"
        cm.CommandText = "select QtyUOM_Cd from item_master where item_Cd='" & txtItem_Cd.Text.Trim & "'"
        rs = cm.ExecuteReader
        If rs.Read Then
            vSFGUom = IIf(IsDBNull(rs("QtyUOM_Cd")), "", rs("QtyUOM_Cd"))
        End If
        rs.Close()

        vStartUpRun_Hrs = IIf(txtSDTime.Text = "", "00", txtSDTime.Text.Trim)
        vInitialRun_Hrs = IIf(txtIRDTime.Text = "", "00", txtIRDTime.Text.Trim)
        vProdRun_Hrs = IIf(txtPRDTime.Text = "", "00", txtPRDTime.Text.Trim)

        vStartUpRun_Mins = cmbSRDMins.SelectedValue
        vInitialRun_Mins = cmbIRDMins.SelectedValue
        vProdRun_Mins = cmbPRDMins.SelectedValue

        If Request.Item("mode") = "Edit" Then
            vSQL = "update bom_process set " _
                & "Sect_Cd='" & cmbSection.SelectedValue & "',Proc_Cd='" & cmbProcess.SelectedValue & "'," _
                & "OperOrder='" & IIf(txtOperatingOrderNo.Text.Trim = "", 0, txtOperatingOrderNo.Text.Trim) & "'," _
                & "SFG_Cd='" & vSFGCode & "'," _
                & "SFG_Descr='" & vSFGDescr & "'," _
                & "SFGUOM='" & vSFGUom & "'," _
                & "QtyOut='" & IIf(txtQty.Text.Trim = "", 0, txtQty.Text.Trim) & "'," _
                & "QtyOutUOM ='" & cmbQtyOutUOM.SelectedValue & "',"

			vSQL += "StartUpRun_Hrs='" & vStartUpRun_Hrs & "'," _
				& "InitialRun_Hrs='" & vInitialRun_Hrs & "'," _
				& "ProdRun_Hrs='" & vProdRun_Hrs & "'," _
				& "StartUpRun_Mins='" & vStartUpRun_Mins & "'," _
				& "InitialRun_Mins='" & vInitialRun_Mins & "'," _
				& "ProdRun_Mins='" & vProdRun_Mins & "'," _
				& "WithContainer='" & rdoWithContainer.SelectedValue & "',ContainerUOM='" & cmbConUOM.SelectedValue & "'," _
				& "CreatedBy='" & Session("uid") & "',DateCreated='" & Format(CDate(Now), "MM-dd-yyyy") & "'," _
				& "Meters='" & txtMeter.Text.Trim & "'," _
				& "Kilos='" & txtKilo.Text.Trim & "' "

			vSQL += "where BOM_Cd=" & Request.Item("pBom") & " and Revision=" & Request.Item("pBomRev") & " and " _
                & "TranId='" & Request.Item("pProTranId") & "' "
        Else
            vSQL = "insert into bom_process (BOM_Cd,Revision,Sect_Cd,Proc_Cd,OperOrder, " _
                & "SFG_Cd,SFG_Descr,SFGUOM,QtyOut,QtyOutUOM," _
                & "StartUpRun_Hrs,InitialRun_Hrs,ProdRun_Hrs,StartUpRun_Mins,InitialRun_Mins,ProdRun_Mins," _
                & "WithContainer,ContainerUOM,CreatedBy,DateCreated,Meters,Kilos) values ("

            'StartUpSDurDays,StartUpSDurTime,InitRunSDurDays,InitRunSDurTime,ProdRunDurDays,ProdRunDurTime,

            vSQL += txtBOM_Cd.Text & "," & txtBOM_Rev.Text & ",'" & cmbSection.SelectedValue & "','" _
                & cmbProcess.SelectedValue & "','" _
                & IIf(txtOperatingOrderNo.Text.Trim = "", 0, txtOperatingOrderNo.Text.Trim) & "','"

            vSQL += vSFGCode & "','" & vSFGDescr & "', '" & vSFGUom & "','" _
                & IIf(txtQty.Text.Trim = "", 0, txtQty.Text.Trim) & "','" & cmbQtyOutUOM.SelectedValue & "','"

            vSQL += vStartUpRun_Hrs & "','" & vInitialRun_Hrs & "','" & vProdRun_Hrs & "','" _
                & vStartUpRun_Mins & "','" & vInitialRun_Mins & "','" & vProdRun_Mins & "','"

			vSQL += rdoWithContainer.SelectedValue & "','" & cmbConUOM.SelectedValue & "','" _
				& Session("uid") & "','" & Format(CDate(Now), "MM-dd-yyyy") & "','" & txtMeter.Text.Trim & "','" & txtKilo.Text.Trim & "') "

		End If

        cm.CommandText = vSQL
        'Response.Write(vSQL)
        Try
            cm.ExecuteNonQuery()

            'vParentId = GetRef("select TranId from bom_process " _
            '                   & "where Bom_Cd='" & txtBOM_Cd.Text & "' and " _
            '                   & "Revision='" & txtBOM_Rev.Text & "' and " _
            '                   & "OperOrder='" & txtOperatingOrderNo.Text.Trim & "' ", "")

            'vSQL = "delete from bom_materials where Bom_Cd='" & txtBOM_Cd.Text & "' and Revision='" & txtBOM_Rev.Text & "' and Parent_TranId=" & vParentId
            'cm.CommandText = vSQL
            'cm.ExecuteNonQuery()
            'Response.Write(vSQL)

            'vSQL = "insert into bom_materials (Parent_TranId,Item_Cd,ItemUOM,BOM_Cd,Revision,Sect_Cd,Proc_Cd,Qty,DrawnFrom,CreatedBy, DateCreated,SFG_Code) " & _
            '    "select " & vParentId & ", Mat_ItemCd, null, '" & txtBOM_Cd.Text & "','" & txtBOM_Rev.Text & "','" & cmbSection.SelectedValue & "','" & cmbProcess.SelectedValue & "',0,0,'" & _
            '    Session("uid") & "','" & Now & "','" & vSFGCode & "' from item_sfg where SFG_ItemCd='" & vSFGCode & "'"
            'cm.CommandText = vSQL
            'cm.ExecuteNonQuery()
            'Response.Write(vSQL)

            'If Request.Item("mode") = "new" Then
            '    'vParentId

            'Else
            '    'if the txtSFG Code <> to cmbSFG.selected
            '    '   Delete from bom_materials where BOM='' and BOMRev='' and SFG_Cd=''
            '    '   Insert into (Parent_TranId,Item_Cd,ItemUOM,BOM_Cd,Revision,Sect_Cd,Proc_Cd,Qty,DrawnFrom,CreatedBy, DateCreated,SFG_Code)
            '    'else
            '    '   Update
            'End If


        Catch ex As SqlClient.SqlException
            Response.Write("Error in SQL query insert/update records:  " & ex.Message)
        End Try

        c.Close()
        c.Dispose()
        cm.Dispose()
        'getQA_List()

    End Sub
      
    Protected Sub cmbSection_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbSection.SelectedIndexChanged 
        BuildCombo("select Proc_Cd,Descr from ref_item_process where Proc_Cd<>'99' and Sect_Cd='" & cmbSection.SelectedValue & "' order by Descr", cmbProcess)
        'getQA_List() 
    End Sub

    'Protected Sub cmbFSG_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbFSG.SelectedIndexChanged
    '    getQA_List()
    '    txtSFG.Text = cmbFSG.SelectedValue
    '    GetAllMaterials()
    'End Sub

    'Protected Sub cmdGetItem_Click(sender As Object, e As EventArgs) Handles cmdGetItem.Click

    '    If txtSFG.Text.Trim = "" Then
    '        BuildCombo("select Item_Cd,Descr from item_master where ItemType_Cd='SFG' order by Descr", cmbFSG)
    '    Else
    '        BuildCombo("select Item_Cd,Descr from item_master where ItemType_Cd='SFG' and Item_Cd like '%" & txtSFG.Text.Trim & "%' order by Descr", cmbFSG)
    '    End If

    '    cmbFSG.Items.Add(" ")
    '    cmbFSG.SelectedValue = " "
    '    getQA_List()

    'End Sub

    Private Sub GetAllMaterials()
        Dim c As New SqlClient.SqlConnection
        Dim cm As New SqlClient.SqlCommand
        Dim rs As SqlClient.SqlDataReader 
        c.ConnectionString = connStr
      

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
        cm.CommandText = "select distinct(Mat_ItemCd) as SFGItem, " _
            & "(select Descr from item_master where item_master.Item_Cd=Mat_ItemCd) as Descr, " _
            & "Grams, Percentage from item_sfg "

        'where " _
        '& "SFG_ItemCd='" & txtItem_Cd.Text.Trim & "-" & txtOperatingOrderNo.Text.Trim & "' "
        'Response.Write(cm.CommandText)

        rs = cm.ExecuteReader
        Do While rs.Read
              
            vMat_List += "<tr>"
            vMat_List += "<td style='text-align:center; padding:5px;'>" & rs("SFGItem") & "</td>" & _
                "<td style='text-align:left'>" & rs("Descr") & "</td>" & _
                "<td style='text-align:right'>" & Format(rs("Grams"), "###0.00") & "</td>" & _
                "<td style='text-align:right'>" & Format(rs("Percentage"), "###0.00") & "</td>"
            vMat_List += "</tr>"
            
        Loop
        rs.Close()

        c.Close()
        c.Dispose()
        cm.Dispose()
    End Sub

End Class

 