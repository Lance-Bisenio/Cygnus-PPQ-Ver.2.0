Imports denaro
Partial Class item_pricing
    Inherits System.Web.UI.Page
    Public vScript As String = ""
    Public vErrorMsg As String = ""

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Session("uid") = "" Then
            vScript = "alert('Your login session has expired. Please re-login again.'); window.close();"
            Exit Sub
        End If

        If Not IsPostBack Then
            BuildCombo("select UomCd, UomDescr from uom_ref  order by UomDescr", cmbWholeSale_Uom)
            cmbWholeSale_Uom.SelectedValue = "99"

            BuildCombo("select UomCd, UomDescr from uom_ref  order by UomDescr", cmbRetail_Uom)
            cmbRetail_Uom.SelectedValue = "99"

            If Request.Item("mode") = "view" Then
                GetInfo()
            End If
        End If
    End Sub
    Private Sub GetInfo()
        Dim c As New SqlClient.SqlConnection(connStr)
        Dim cm As New SqlClient.SqlCommand
        Dim rs As SqlClient.SqlDataReader

        txtClose.Visible = False
        txtSave.Text = "Edit"

        cm.Connection = c
        c.Open()
        cm.CommandText = "select * from item where ItemCd='" & Request.Item("itemCd").Trim & "' "
        Try
            rs = cm.ExecuteReader
            If rs.Read Then

                txtWSPrice.Text = IIf(IsDBNull(rs("WsPrice")), "0.00", rs("WsPrice"))
                txtWSDisFac.Text = IIf(IsDBNull(rs("WsDisc")), 0, rs("WsDisc"))

                txtRetailPrice.Text = IIf(IsDBNull(rs("RtlPrice")), "0.00", rs("RtlPrice"))
                txtRetailDisFac.Text = IIf(IsDBNull(rs("RtlDisc")), 0, rs("RtlDisc"))

                txtCurrCost.Text = IIf(IsDBNull(rs("CurrCost")), "0.00", rs("CurrCost"))
                txtAveCost.Text = IIf(IsDBNull(rs("AvgCost")), "0.00", rs("AvgCost"))

                cmbWholeSale_Uom.SelectedValue = IIf(IsDBNull(rs("WsUomCd")), "99", rs("WsUomCd"))
                cmbRetail_Uom.SelectedValue = IIf(IsDBNull(rs("RtlUomCd")), "99", rs("RtlUomCd"))

                txtWSPrice.Enabled = False
                txtWSDisFac.Enabled = False
                cmbWholeSale_Uom.Enabled = False

                txtRetailPrice.Enabled = False
                txtRetailDisFac.Enabled = False
                cmbRetail_Uom.Enabled = False

                txtCurrCost.Enabled = False
                txtAveCost.Enabled = False

                Session("vOldRecord") += "Wholesale Price=" & IIf(IsDBNull(rs("WsPrice")), 0, rs("WsPrice")) & _
                "|Discount Factor =" & IIf(IsDBNull(rs("WsDisc")), 0, rs("WsDisc")) & _
                "|Retail Price=" & IIf(IsDBNull(rs("RtlPrice")), 0, rs("RtlPrice")) & _
                "|Discount Factor=" & IIf(IsDBNull(rs("RtlDisc")), 0, rs("RtlDisc")) & _
                "|Current Cost=" & IIf(IsDBNull(rs("CurrCost")), 0, rs("CurrCost")) & _
                "|Average Cost=" & IIf(IsDBNull(rs("AvgCost")), 0, rs("AvgCost")) & _
                "|Wholesale Price UOM=" & IIf(IsDBNull(rs("WsUomCd")), 0, rs("WsUomCd")) & _
                "|Retail Price UOM=" & IIf(IsDBNull(rs("RtlUomCd")), "", rs("RtlUomCd"))

                rs.Close()
                c.Close()
                c.Dispose()
                cm.Dispose()

            End If
        Catch ex As SqlClient.SqlException
            Response.Write("Error in retrieving data. " & ex.Message)
        End Try

    End Sub

    Protected Sub txtSave_Click(sender As Object, e As EventArgs) Handles txtSave.Click
        Select Case txtSave.Text
            Case "Submit"
                SaveModule()
            Case "Edit"

                txtWSPrice.Enabled = True
                txtWSDisFac.Enabled = True
                cmbWholeSale_Uom.Enabled = True
                txtRetailPrice.Enabled = True
                txtRetailDisFac.Enabled = True
                cmbRetail_Uom.Enabled = True
                txtCurrCost.Enabled = True
                txtAveCost.Enabled = True

                txtSave.Text = "Submit"
                txtClose.Text = "Cancel"
                txtClose.Visible = True

                divErrorDis.Visible = False

        End Select

    End Sub
    Private Sub SaveModule()
        Dim c As New SqlClient.SqlConnection(connStr)
        Dim cm As New SqlClient.SqlCommand
        'Dim rs As SqlClient.SqlDataReader

        Dim vFields As String = ""
        Dim vFData As String = ""
        Dim vNewRecord As String = ""

        If txtWSPrice.Text.Trim = "" Then
            divErrorDis.Visible = True
            vErrorMsg += "Wholesale Price Field is requared"
            Exit Sub
        ElseIf txtWSDisFac.Text.Trim = "" Then
            divErrorDis.Visible = True
            vErrorMsg = "Discount Factor Field is requared"
            Exit Sub
        ElseIf txtRetailPrice.Text.Trim = "" Then
            divErrorDis.Visible = True
            vErrorMsg = "Retail Price Field is requared"
            Exit Sub
        ElseIf txtRetailDisFac.Text.Trim = "" Then
            divErrorDis.Visible = True
            vErrorMsg = "Discount Factor Field is requared"
            Exit Sub
        ElseIf txtCurrCost.Text.Trim = "" Then
            divErrorDis.Visible = True
            vErrorMsg = "Current Cost Field is requared"
            Exit Sub
        ElseIf txtAveCost.Text.Trim = "" Then
            divErrorDis.Visible = True
            vErrorMsg = "Average Cost Field is requared"
            Exit Sub
        End If

        cm.Connection = c
        c.Open()

        cm.CommandText = "Update item set WsPrice='" & txtWSPrice.Text.Trim & "', WsDisc='" & txtWSDisFac.Text.Trim & "', WsUomCd='" & cmbWholeSale_Uom.SelectedValue & _
            "', RtlPrice='" & txtRetailPrice.Text.Trim & "', RtlDisc='" & txtRetailDisFac.Text.Trim & "', RtlUomCd='" & cmbRetail_Uom.SelectedValue & _
            "', CurrCost='" & txtCurrCost.Text.Trim & "', AvgCost='" & txtAveCost.Text.Trim & "' where ItemCd='" & Request.Item("ItemCd").Trim & "'"

        Try
            'Response.Write(cm.CommandText)
            cm.ExecuteNonQuery()

            '' ===============================================================================================================================
            '' CREATE RECORD OR AUDIT LOGS 
            '' ===============================================================================================================================
            vNewRecord = "Wholesale Price=" & txtWSPrice.Text.Trim & _
                "|Wholesale Discount Factor =" & txtWSDisFac.Text.Trim & _
                "|Retail Price=" & txtRetailPrice.Text.Trim & _
                "|Retail Discount Factor=" & txtRetailDisFac.Text.Trim & _
                "|Current Cost=" & txtCurrCost.Text.Trim & _
                "|Average Cost=" & txtAveCost.Text.Trim & _
                "|Wholesale Price UOM=" & cmbWholeSale_Uom.SelectedValue & _
                "|Retail Price UOM=" & cmbRetail_Uom.SelectedValue

            EventLog(Session("uid"), Request.ServerVariables("REMOTE_ADDR"), IIf(Request.Item("mode") = "view", "EDIT", "ADD"), _
                Session("vOldRecord"), vNewRecord, "Item Code : " & Request.Item("itemCd") & _
                " was " & IIf(Request.Item("mode") = "view", "modify", "added"), "Item Master", c)
            Session.Remove("vOldRecord")

            divErrorDis.Visible = True
            divErrorDis.Style.Value = "border: solid 2px #46af0d; background:#d0fc9d;"
            vErrorMsg = "Record successfully Save."

            If txtSave.Text = "Submit" Then
                GetInfo()
            End If

        Catch ex As DataException
            vScript = "alert('An error occurred while trying to Save the new record.');"
        End Try

        c.Close()
        c.Dispose()
        cm.Dispose()
    End Sub

    Protected Sub txtClose_Click(sender As Object, e As EventArgs) Handles txtClose.Click
        txtClose.Visible = False
        txtSave.Text = "Edit"
        GetInfo()
    End Sub
End Class
