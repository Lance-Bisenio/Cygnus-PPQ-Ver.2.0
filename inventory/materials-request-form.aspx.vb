Imports denaro
Imports item_details

Partial Class inventory_materials_request_form
    Inherits System.Web.UI.Page
    Public vScript As String = ""
  
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Session("uid") = "" Then
            vScript = "alert('Your login session has expired. Please login again.'); parent.jQuery.fn.colorbox.close();"
        End If
        If Not IsPostBack Then




            BuildCombo("select Item_Cd, " & _
               "(select (Descr + ' ' + Descr) as Descr from item_master where item_master.Item_Cd=bom_materials.Item_Cd) as ItemDescr " & _
               "from bom_materials where " & _
                    "BOM_Cd=" & Session("sBOM") & " and " & _
                    "Revision=" & Session("sBOMRev") & " and " & _
                    "Sect_Cd='" & Session("sSection") & "' and " & _
                    "Proc_Cd='" & Session("sProcess") & "' " & _
                    "order by ItemDescr ", cmbMaterials)

            cmbMaterials.Items.Add("Select Materials")
            cmbMaterials.SelectedValue = "Select Materials"


            If Request.Item("pMode") = "Edit" Then
                cmbMaterials.Enabled = False
                cmbMaterials.SelectedValue = GetRef("select Item_Cd from prod_request_materials where TranID=" & Session("sTranId"), "")
                txtItemCd.Text = GetRef("select Item_Cd from prod_request_materials where TranID=" & Session("sTranId"), "")
                txtQty.Text = GetRef("select Qty from prod_request_materials where TranID=" & Session("sTranId"), "")

            End If
            'Session("sTranId") = tbl_Materials.SelectedRow.Cells(1).Text


        End If
    End Sub

    Protected Sub cmbMaterials_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbMaterials.SelectedIndexChanged
        If cmbMaterials.SelectedValue = "Select Materials" Then
            txtItemCd.Text = ""
        Else
            txtItemCd.Text = cmbMaterials.SelectedValue
        End If
        vScript = "$('#txtQty').focus();"
    End Sub

    Protected Sub cmdSave_Click(sender As Object, e As EventArgs) Handles cmdSave.Click

        If Session("uid") = "" Then
            vScript = "alert('Login session has expired. Please re-login again.'); parent.jQuery.fn.colorbox.close();"
        End If

        If txtItemCd.Text.Trim = "" Then
            vScript = "alert('Please select Materials.'); " 'parent.jQuery.fn.colorbox.close();"
            Exit Sub
        End If

        If txtQty.Text.Trim = "" Then
            vScript = "alert('Please enter QTY.'); " 'parent.jQuery.fn.colorbox.close();"
            Exit Sub
        End If

        Dim vSQL As String = ""
        Dim vTranID As String = Session("sTranId")
        Dim vBOM As String = Session("sBOM")
        Dim vBOMRev As String = Session("sBOMRev")
        Dim vJONO As String = Session("sJONO")
        Dim vSection As String = Session("sSection")
        Dim vProcess As String = Session("sProcess")
        Dim vLotNo As String = Session("sSection")

        Dim vItemCd As String = txtItemCd.Text.Trim
        Dim vRefNo As String = "RQT" & GetRef("select COUNT(Item_Cd) from prod_request_materials", 0) + 1 & Format(Now(), "MMddyyyy")

        If Request.Item("pMode") = "Edit" Then

            vSQL = "update prod_request_materials set " & _
                        "Qty='" & txtQty.Text.Trim & "'," & _
                        "CreatedBy='" & Session("uid") & "'," & _
                        "DateCreated='" & Now & "' " & _
                        "where TranId=" & vTranID

        Else
            vSQL = "insert into prod_request_materials " & _
                        "(BOM,Revision,Sect_Cd,Proc_Cd,JONO,Item_Cd,Qty,CreatedBy,DateCreated,RequestRef) " & _
                        "values ('" & vBOM & "','" & vBOMRev & "','" & vSection & "','" & vProcess & "','" & vJONO & "','" & vItemCd & _
                        "','" & txtQty.Text.Trim & "','" & Session("uid") & "','" & Now & "','" & vRefNo & "') "

        End If
        





        Helper_ModifyRecords(vSQL)


        If Request.Item("pMode") = "Edit" Then
            vScript = "alert('Successfully saved.'); parent.jQuery.fn.colorbox.close();"
        Else
            vScript = "alert('Successfully saved.'); $('#txtRefBarcode').focus();"
        End If


        txtItemCd.Text = ""
        cmbMaterials.SelectedValue = "Select Materials"
        txtQty.Text = ""

    End Sub

    Protected Sub cmdCancel_Click(sender As Object, e As EventArgs) Handles cmdCancel.Click
        vScript = " parent.jQuery.fn.colorbox.close();"
    End Sub
End Class
