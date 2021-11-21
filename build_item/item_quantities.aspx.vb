Imports denaro
Partial Class item_quantities
    Inherits System.Web.UI.Page
    Public vScript As String = ""
    Public vErrorMsg As String = ""

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Session("uid") = "" Then
            vScript = "alert('Your login session has expired. Please re-login again.'); window.close();"
            Exit Sub
        End If

        If Not IsPostBack Then
            BuildCombo("select UomCd, UomDescr from uom_ref  order by UomDescr", cmbUom)
            cmbUom.SelectedValue = "99"

            


            If Request.Item("mode") = "view" Then
                GetInfo()
                GetConversionInfo()
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

                txtCeilling.Text = IIf(IsDBNull(rs("CeilingQty")), 0, rs("CeilingQty"))
                txtFloor.Text = IIf(IsDBNull(rs("FloorQty")), 0, rs("FloorQty"))
                txtCurrQty.Text = IIf(IsDBNull(rs("CurrQty")), 0, rs("CurrQty"))
                txtUnserved.Text = IIf(IsDBNull(rs("UnservedQty")), 0, rs("UnservedQty"))

                cmbUom.SelectedValue = IIf(IsDBNull(rs("UOMCd")), "99", rs("UOMCd"))

                txtCeilling.Enabled = False
                txtFloor.Enabled = False
                txtCurrQty.Enabled = False
                txtUnserved.Enabled = False
                cmbUom.Enabled = False

                Session("vOldRecord") += "Ceilling =" & IIf(IsDBNull(rs("CeilingQty")), 0, rs("CeilingQty")) & _
                "|Floor=" & IIf(IsDBNull(rs("FloorQty")), 0, rs("FloorQty")) & _
                "|Current Qty=" & IIf(IsDBNull(rs("CurrQty")), 0, rs("CurrQty")) & _
                "|Unserved Qty=" & IIf(IsDBNull(rs("UnservedQty")), 0, rs("UnservedQty")) & _
                "|Uom=" & IIf(IsDBNull(rs("UOMCd")), "", rs("UOMCd"))

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
            Case "Save"
                SaveModule()
            Case "Edit"

                txtCeilling.Enabled = True
                txtFloor.Enabled = True
                txtCurrQty.Enabled = True
                txtUnserved.Enabled = True
                cmbUom.Enabled = True

                txtSave.Text = "Save"
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

        If txtCeilling.Text.Trim = "" Then
            divErrorDis.Visible = True
            vErrorMsg += "Ceilling Field is requared"
            Exit Sub
        ElseIf txtFloor.Text.Trim = "" Then
            divErrorDis.Visible = True
            vErrorMsg = "Floor Field is requared"
            Exit Sub
        ElseIf txtCurrQty.Text.Trim = "" Then
            divErrorDis.Visible = True
            vErrorMsg = "Current Qty Field is requared"
            Exit Sub
        ElseIf txtUnserved.Text.Trim = "" Then
            divErrorDis.Visible = True
            vErrorMsg = "Unserved Qty Field is requared"
            Exit Sub
        End If

        cm.Connection = c
        c.Open()

        cm.CommandText = "Update item set CeilingQty='" & txtCeilling.Text.Trim & "', CurrQty='" & _
            txtCurrQty.Text.Trim & "', FloorQty='" & txtFloor.Text.Trim & "', UOMCd='" & cmbUom.SelectedValue & "', UnservedQty='" & _
            txtUnserved.Text.Trim & "' where ItemCd='" & Request.Item("ItemCd").Trim & "'"

        Try
            'Response.Write(cm.CommandText)
            cm.ExecuteNonQuery()

            '' ===============================================================================================================================
            '' CREATE RECORD OR AUDIT LOGS 
            '' ===============================================================================================================================
            vNewRecord = "Ceilling =" & txtCeilling.Text.Trim & _
                "|Floor=" & txtFloor.Text.Trim & _
                "|Current Qty=" & txtCurrQty.Text.Trim & _
                "|Unserved Qty=" & txtUnserved.Text.Trim & _
                "|Uom=" & cmbUom.SelectedValue

            EventLog(Session("uid"), Request.ServerVariables("REMOTE_ADDR"), IIf(Request.Item("mode") = "view", "EDIT", "ADD"), _
                Session("vOldRecord"), vNewRecord, "Item Code : " & Request.Item("itemCd") & _
                " was " & IIf(Request.Item("mode") = "view", "modify", "added"), "Item Master", c)
            Session.Remove("vOldRecord")

            divErrorDis.Visible = True
            divErrorDis.Style.Value = "border: solid 2px #46af0d; background:#d0fc9d;"
            vErrorMsg = "Record successfully Save."

            If txtSave.Text = "Save" Then
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

    Protected Sub txtNewConv_Click(sender As Object, e As EventArgs) Handles txtNewConv.Click
        Select Case txtNewConv.Text
            Case "Add Conversion"

                BuildCombo("select UomCd, UomDescr from uom_ref " & _
                    "where not EXISTS (select ToUOMCd from item_conversion where ItemCd='" & Request.Item("ItemCd")& "' and UomCd=ToUOMCd) " & _
                    "order by UomDescr", cmbConvUom)


                cmbConvUom.SelectedValue = "99"

                txtNewConv.Text = "Save"
                txtConvClose.Text = "Cancel"
                txtConvClose.Visible = True

                txtConvFac.Text = 0
                cmbConvUom.SelectedValue = 99
                txtEditConv.Visible = False
                txtDel.Visible = False
                divErrorDis.Visible = False

                txtConvFac.Enabled = True
                cmbConvUom.Enabled = True

            Case "Save"
                SaveConversion()
        End Select

    End Sub

    Protected Sub txtConvClose_Click(sender As Object, e As EventArgs) Handles txtConvClose.Click
        txtConvFac.Enabled = False
        cmbConvUom.Enabled = False
        txtNewConv.Text = "Add Conversion"
        txtEditConv.Text = "Edit"

        txtConvClose.Visible = False
        txtEditConv.Visible = True
        txtDel.Visible = True
    End Sub

    Private Sub SaveConversion()
        Dim c As New SqlClient.SqlConnection(connStr)
        Dim cm As New SqlClient.SqlCommand
        'Dim rs As SqlClient.SqlDataReader


        Dim vNewRecord As String = ""

        If txtConvFac.Text.Trim = "" Then
            divErrorDis.Visible = True
            vErrorMsg += "Convesion Factor Field is requared"
            Exit Sub
        End If

        cm.Connection = c
        c.Open()

        If txtEditConv.Text = "Save" Then

            Session("vOldRecord") = "Item Code Conversion=" & Request.Item("ItemCd") & _
                "|Conversion Factors=" & tblItemConv.SelectedRow.Cells(3).Text & _
                "|Conversion Unit of Measurement=" & tblItemConv.SelectedRow.Cells(1).Text.Trim

            cm.CommandText = "Update item_conversion set ToUOMCd='" & cmbConvUom.SelectedValue.Trim & "', Factor='" & txtConvFac.Text.Trim & _
                "' where ItemCd='" & Request.Item("ItemCd") & "' and ToUOMCd='" & tblItemConv.SelectedRow.Cells(1).Text.Trim & _
                "' and Factor='" & tblItemConv.SelectedRow.Cells(3).Text & "' "
        End If

        If txtNewConv.Text = "Save" Then
            cm.CommandText = "insert into item_conversion values ('" & Request.Item("ItemCd") & "','" & _
                cmbConvUom.SelectedValue.Trim & "','" & txtConvFac.Text.Trim & "')"
        End If

        Try
            ''Response.Write(cm.CommandText)
            cm.ExecuteNonQuery()

            '' ===============================================================================================================================
            '' CREATE RECORD OR AUDIT LOGS 
            '' ===============================================================================================================================
            vNewRecord = "Item Code Conversion=" & Request.Item("ItemCd") & _
                "|Conversion Factors=" & txtConvFac.Text.Trim & _
                "|Conversion Unit of Measurement=" & cmbConvUom.SelectedValue

            EventLog(Session("uid"), Request.ServerVariables("REMOTE_ADDR"), IIf(Request.Item("mode") = "view", "EDIT", "ADD"), _
                Session("vOldRecord"), vNewRecord, "Item Code : " & Request.Item("itemCd") & _
                " was " & IIf(Request.Item("mode") = "view", "modify", "added"), "Item Master", c)
            Session.Remove("vOldRecord")

            divErrorDis.Visible = True
            divErrorDis.Style.Value = "border: solid 2px #46af0d; background:#d0fc9d;"
            vErrorMsg = "Record successfully Save."

            GetConversionInfo()

        Catch ex As DataException
            vScript = "alert('An error occurred while trying to Save the new record.');"
        End Try

        c.Close()
        c.Dispose()
        cm.Dispose()
    End Sub

    Private Sub GetConversionInfo()
        Dim c As New SqlClient.SqlConnection(connStr)
        Dim da As SqlClient.SqlDataAdapter
        Dim ds As New DataSet

        c.ConnectionString = connStr
        da = New SqlClient.SqlDataAdapter("select *, (select UomDescr from uom_ref where UomCd=ToUomCd) as FactorName " & _
                                          "from item_conversion where ItemCd='" & Request.Item("ItemCd") & "'", c)

        da.Fill(ds, "ItemConversion")
        tblItemConv.DataSource = ds.Tables("ItemConversion")
        tblItemConv.DataBind()
        da.Dispose()
        ds.Dispose()

        txtConvFac.Enabled = False
        cmbConvUom.Enabled = False
        txtConvClose.Visible = False
        txtNewConv.Text = "Add Conversion"


    End Sub

    Protected Sub tblItemConv_SelectedIndexChanged(sender As Object, e As EventArgs) Handles tblItemConv.SelectedIndexChanged
        

        BuildCombo("select UomCd, UomDescr from uom_ref", cmbConvUom)
        txtConvFac.Text = tblItemConv.SelectedRow.Cells(3).Text
        cmbConvUom.SelectedValue = tblItemConv.SelectedRow.Cells(1).Text

        txtNewConv.Text = "Add Conversion"
        txtEditConv.Text = "Edit"
        txtConvFac.Enabled = False
        cmbConvUom.Enabled = False

        txtEditConv.Visible = True
        txtDel.Visible = True
        divErrorDis.Visible = False
    End Sub

    Protected Sub txtEditConv_Click(sender As Object, e As EventArgs) Handles txtEditConv.Click
        Select Case txtEditConv.Text
            Case "Edit"

                If tblItemConv.SelectedIndex >= 0 Then
                    txtEditConv.Text = "Save"
                    txtConvClose.Text = "Cancel"
                    txtConvClose.Visible = True

                    txtConvFac.Enabled = True
                    cmbConvUom.Enabled = True
                Else
                    divErrorDis.Visible = True

                    vErrorMsg = "No record selected."
                    Exit Sub

                End If

            Case "Save"
                SaveConversion()
        End Select
    End Sub
End Class
