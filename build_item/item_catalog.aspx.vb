Imports denaro
Partial Class build_item_item_catalog

    Inherits System.Web.UI.Page
    Public vScript As String = ""
    Public vItemImages As String = ""


    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Session("uid") = "" Then
            vScript = "alert('Login session has expired. Please re-login again.'); window.opener.document.form1.h_Mode.value='reload'; window.opener.document.form1.submit(); window.close();"
            Exit Sub
        End If

        If Not IsPostBack Then

            Dim vSuppQry As String = "select Supp_Cd, Descr from ref_item_supplier order by Descr"
            Dim vCustQry As String = "select Customer_Cd, Descr  from ref_item_customer order by Descr"

            txtItemCd.Text = Request.Item("pItemCd")
            txtDescr1.Text = Request.Item("pDescr1")
            txtDescr2.Text = Request.Item("pDescr2")

            BuildCombo(vCustQry, cmbCust1)
            cmbCust1.Items.Add("")
            cmbCust1.SelectedValue = ""

            BuildCombo(vCustQry, cmbCust2)
            cmbCust2.Items.Add("")
            cmbCust2.SelectedValue = ""

            BuildCombo(vCustQry, cmbCust3)
            cmbCust3.Items.Add("")
            cmbCust3.SelectedValue = ""

            BuildCombo(vCustQry, cmbCust4)
            cmbCust4.Items.Add("")
            cmbCust4.SelectedValue = ""

            BuildCombo(vCustQry, cmbCust5)
            cmbCust5.Items.Add("")
            cmbCust5.SelectedValue = ""



            BuildCombo(vSuppQry, cmbSupp1)
            cmbSupp1.Items.Add("")
            cmbSupp1.SelectedValue = ""

            BuildCombo(vSuppQry, cmbSupp2)
            cmbSupp2.Items.Add("")
            cmbSupp2.SelectedValue = ""

            BuildCombo(vSuppQry, cmbSupp3)
            cmbSupp3.Items.Add("")
            cmbSupp3.SelectedValue = ""

            BuildCombo(vSuppQry, cmbSupp4)
            cmbSupp4.Items.Add("")
            cmbSupp4.SelectedValue = ""

            BuildCombo(vSuppQry, cmbSupp5)
            cmbSupp5.Items.Add("")
            cmbSupp5.SelectedValue = ""
            GetInfo()

        End If

        GetImages()
         
    End Sub

    Protected Sub cmdSave_Click(sender As Object, e As EventArgs) Handles cmdSave.Click
        
        SaveCustomer()
        SaveSupplier()

        vScript = "alert('Successfully saved.'); window.opener.document.form1.h_Mode.value='reload'; window.opener.document.form1.submit(); window.close();"
    End Sub
    Private Sub GetImages()
        Dim c As New SqlClient.SqlConnection(connStr)
        Dim cmRef As New SqlClient.SqlCommand
        Dim rsref As SqlClient.SqlDataReader
        Dim attachments() As String

        cmRef.Connection = c
        c.Open()
        cmRef.CommandText = "select itemimages from item_master where Item_Cd='" & Request.Item("pItemCd") & "'"
        'Response.Write(cmRef.CommandText)
        rsref = cmRef.ExecuteReader
        If rsref.Read Then
            If Not IsDBNull(rsref("itemimages")) Then
                'txtSubAtt.Value = rsref("SubAttachment")
                attachments = rsref("itemimages").ToString.Split("|")
                For i = 0 To UBound(attachments) - 1
                    vItemImages += "<a class='textLinks' onclick='OpenSubAtt(""" & attachments(i) & """);'>" & attachments(i) & "</a><br>"
                Next i
            End If
        End If
        rsref.Close()
        c.Close()
        c.Dispose()
        cmRef.Dispose()
    End Sub
    Private Sub GetInfo()
         
        Dim vSQL As String = "select Acct_Cd from ref_item_catalog where Item_Cd='" & Request.Item("pItemCd") & "' and AcctType='CUSTOMER'"
        Dim vSubSQL As String = "select Alt_Cd from ref_item_catalog where Item_Cd='" & Request.Item("pItemCd") & "' and AcctType='CUSTOMER'"

        Dim vCust1 As String = GetRef(vSQL & " and iCtr=1", "No-record")
        Dim vCust2 As String = GetRef(vSQL & " and iCtr=2", "No-record")
        Dim vCust3 As String = GetRef(vSQL & " and iCtr=3", "No-record")
        Dim vCust4 As String = GetRef(vSQL & " and iCtr=4", "No-record")
        Dim vCust5 As String = GetRef(vSQL & " and iCtr=5", "No-record")
         
        If vCust1 <> "No-record" Then
            cmbCust1.SelectedValue = vCust1
            txtCust1.Text = GetRef(vSubSQL & " and iCtr=1", "No-record")
            tr_Cus2.Visible = True
        End If

        If vCust2 <> "No-record" Then
            cmbCust2.SelectedValue = vCust2
            txtCust2.Text = GetRef(vSubSQL & " and iCtr=2", "No-record")
            tr_Cus2.Visible = True
            tr_Cus3.Visible = True
        End If

        If vCust3 <> "No-record" Then
            cmbCust3.SelectedValue = vCust3
            txtCust3.Text = GetRef(vSubSQL & " and iCtr=3", "No-record")
            tr_Cus3.Visible = True
            tr_Cus4.Visible = True
        End If

        If vCust4 <> "No-record" Then
            cmbCust4.SelectedValue = vCust4
            txtCust4.Text = GetRef(vSubSQL & " and iCtr=4", "No-record")
            tr_Cus4.Visible = True
            tr_Cus5.Visible = True
        End If

        If vCust5 <> "No-record" Then
            cmbCust5.SelectedValue = vCust5
            txtCust5.Text = GetRef(vSubSQL & " and iCtr=5", "No-record")
            tr_Cus5.Visible = True
        End If

        vSQL = "select Acct_Cd from ref_item_catalog where Item_Cd='" & Request.Item("pItemCd") & "' and AcctType='SUPPLIER'"
        vSubSQL = "select Alt_Cd from ref_item_catalog where Item_Cd='" & Request.Item("pItemCd") & "' and AcctType='SUPPLIER'"

        Dim vSupp1 As String = GetRef(vSQL & " and iCtr=1", "No-record")
        Dim vSupp2 As String = GetRef(vSQL & " and iCtr=2", "No-record")
        Dim vSupp3 As String = GetRef(vSQL & " and iCtr=3", "No-record")
        Dim vSupp4 As String = GetRef(vSQL & " and iCtr=4", "No-record")
        Dim vSupp5 As String = GetRef(vSQL & " and iCtr=5", "No-record")

        If vSupp1 <> "No-record" Then
            cmbSupp1.SelectedValue = vSupp1
            txtSupp1.Text = GetRef(vSubSQL & " and iCtr=1", "No-record")
            tr_Supp2.Visible = True
        End If

        If vSupp2 <> "No-record" Then
            cmbSupp2.SelectedValue = vSupp2
            txtSupp2.Text = GetRef(vSubSQL & " and iCtr=2", "No-record")
            tr_Supp2.Visible = True
            tr_Supp3.Visible = True
        End If

        If vSupp3 <> "No-record" Then
            cmbSupp3.SelectedValue = vSupp3
            txtSupp3.Text = GetRef(vSubSQL & " and iCtr=3", "No-record")
            tr_Supp3.Visible = True
            tr_Supp4.Visible = True
        End If

        If vSupp4 <> "No-record" Then
            cmbSupp4.SelectedValue = vSupp4
            txtSupp4.Text = GetRef(vSubSQL & " and iCtr=4", "No-record") 
            tr_Supp4.Visible = True
            tr_Supp5.Visible = True
        End If

        If vSupp5 <> "No-record" Then
            cmbSupp5.SelectedValue = vSupp5
            txtSupp5.Text = GetRef(vSubSQL & " and iCtr=5", "No-record")
            tr_Supp5.Visible = True
        End If
 
    End Sub

    Private Sub SaveCustomer()
        Dim c As New SqlClient.SqlConnection(connStr)
        Dim cm As New SqlClient.SqlCommand
        Dim vSQL As String = ""

        cm.Connection = c
        c.Open()

        vSQL = "delete ref_item_catalog where Item_Cd='" & Request.Item("pItemCd") & "' and AcctType='CUSTOMER' "
        cm.CommandText = vSQL
        cm.ExecuteNonQuery()

        If cmbCust1.SelectedValue <> "" Then
            vSQL = "insert into ref_item_catalog (Acct_Cd,Item_Cd,Alt_Cd,AcctType,iCtr) values ('" & _
                        cmbCust1.SelectedValue & "','" & Request.Item("pItemCd") & "','" & IIf(txtCust1.Text.Trim = "", txtItemCd.Text, txtCust1.Text) & "','CUSTOMER',1) "
            cm.CommandText = vSQL
            cm.ExecuteNonQuery()
        End If

        If cmbCust2.SelectedValue <> "" And txtCust2.Text.Trim <> "" Then
            vSQL = "insert into ref_item_catalog (Acct_Cd,Item_Cd,Alt_Cd,AcctType,iCtr) values ('" & _
            cmbCust2.SelectedValue & "','" & Request.Item("pItemCd") & "','" & txtCust2.Text & "','CUSTOMER',2) "
            cm.CommandText = vSQL
            cm.ExecuteNonQuery()
        End If
        If cmbCust3.SelectedValue <> "" And txtCust3.Text.Trim <> "" Then
            vSQL = "insert into ref_item_catalog (Acct_Cd,Item_Cd,Alt_Cd,AcctType,iCtr) values ('" & _
            cmbCust3.SelectedValue & "','" & Request.Item("pItemCd") & "','" & txtCust3.Text & "','CUSTOMER',3) "
            cm.CommandText = vSQL
            cm.ExecuteNonQuery()
        End If
        If cmbCust4.SelectedValue <> "" And txtCust4.Text.Trim <> "" Then
            vSQL = "insert into ref_item_catalog (Acct_Cd,Item_Cd,Alt_Cd,AcctType,iCtr) values ('" & _
            cmbCust4.SelectedValue & "','" & Request.Item("pItemCd") & "','" & txtCust4.Text & "','CUSTOMER',4) "
            cm.CommandText = vSQL
            cm.ExecuteNonQuery()
        End If
        If cmbCust5.SelectedValue <> "" And txtCust5.Text.Trim <> "" Then
            vSQL = "insert into ref_item_catalog (Acct_Cd,Item_Cd,Alt_Cd,AcctType,iCtr) values ('" & _
            cmbCust5.SelectedValue & "','" & Request.Item("pItemCd") & "','" & txtCust5.Text & "','CUSTOMER',5) "
            cm.CommandText = vSQL
            cm.ExecuteNonQuery()
        End If

        'vSQL = " " 
        'Try
        '    'cm.ExecuteNonQuery() 
        '    'Server.Transfer("item_customer.aspx?pItemCd=" & txtItemCd.Text & "&pDescr1=" & txtDescr1.Text & "&pDescr2=" & txtDescr2.Text & "")
        'Catch ex As DataException
        '    vScript = "alert('An error occurred while trying to Save the new record.');"
        'End Try

        c.Close()
        c.Dispose()
        cm.Dispose()
    End Sub
    Private Sub SaveSupplier()
        Dim c As New SqlClient.SqlConnection(connStr)
        Dim cm As New SqlClient.SqlCommand
        Dim vSQL As String = ""

        cm.Connection = c
        c.Open()

        vSQL = "delete ref_item_catalog where Item_Cd='" & Request.Item("pItemCd") & "' and AcctType='SUPPLIER' "
        cm.CommandText = vSQL
        cm.ExecuteNonQuery()


        If cmbSupp1.SelectedValue <> "" Then
            vSQL = "insert into ref_item_catalog (Acct_Cd,Item_Cd,Alt_Cd,AcctType,iCtr) values ('" & _
                        cmbSupp1.SelectedValue & "','" & Request.Item("pItemCd") & "','" & IIf(txtSupp1.Text.Trim = "", txtItemCd.Text, txtSupp1.Text) & "','SUPPLIER',1) "
            cm.CommandText = vSQL
            cm.ExecuteNonQuery()
        End If


        If cmbSupp2.SelectedValue <> "" And txtSupp2.Text.Trim <> "" Then
            vSQL = "insert into ref_item_catalog (Acct_Cd,Item_Cd,Alt_Cd,AcctType,iCtr) values ('" & _
            cmbSupp2.SelectedValue & "','" & Request.Item("pItemCd") & "','" & txtSupp2.Text & "','SUPPLIER',2) "
            cm.CommandText = vSQL
            cm.ExecuteNonQuery()
        End If
        If cmbSupp3.SelectedValue <> "" And txtSupp3.Text.Trim <> "" Then
            vSQL = "insert into ref_item_catalog (Acct_Cd,Item_Cd,Alt_Cd,AcctType,iCtr) values ('" & _
            cmbSupp3.SelectedValue & "','" & Request.Item("pItemCd") & "','" & txtSupp3.Text & "','SUPPLIER',3) "
            cm.CommandText = vSQL
            cm.ExecuteNonQuery()
        End If
        If cmbSupp4.SelectedValue <> "" And txtSupp4.Text.Trim <> "" Then
            vSQL = "insert into ref_item_catalog (Acct_Cd,Item_Cd,Alt_Cd,AcctType,iCtr) values ('" & _
            cmbSupp4.SelectedValue & "','" & Request.Item("pItemCd") & "','" & txtSupp4.Text & "','SUPPLIER',4) "
            cm.CommandText = vSQL
            cm.ExecuteNonQuery()
        End If
        If cmbSupp5.SelectedValue <> "" And txtSupp5.Text.Trim <> "" Then
            vSQL = "insert into ref_item_catalog (Acct_Cd,Item_Cd,Alt_Cd,AcctType,iCtr) values ('" & _
            cmbSupp5.SelectedValue & "','" & Request.Item("pItemCd") & "','" & txtSupp5.Text & "','SUPPLIER',5) "
            cm.CommandText = vSQL
            cm.ExecuteNonQuery()
        End If

        'vSQL = " " 
        'Try
        '    'cm.ExecuteNonQuery() 
        '    'Server.Transfer("item_customer.aspx?pItemCd=" & txtItemCd.Text & "&pDescr1=" & txtDescr1.Text & "&pDescr2=" & txtDescr2.Text & "")
        'Catch ex As DataException
        '    vScript = "alert('An error occurred while trying to Save the new record.');"
        'End Try

        c.Close()
        c.Dispose()
        cm.Dispose()
    End Sub

    Protected Sub cmbCust1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbCust1.SelectedIndexChanged
        If cmbCust1.SelectedValue = "" Then
            tr_Cus2.Visible = False
            tr_Cus3.Visible = False
            tr_Cus4.Visible = False
            tr_Cus5.Visible = False

            cmbCust2.SelectedValue = ""
            cmbCust3.SelectedValue = ""
            cmbCust4.SelectedValue = ""
            cmbCust5.SelectedValue = ""
            txtCust1.Text = ""
            txtCust2.Text = ""
            txtCust3.Text = ""
            txtCust4.Text = ""
            txtCust5.Text = ""
        Else
            tr_Cus2.Visible = True
            txtCust1.Text = txtItemCd.Text
            BuildCombo("select Customer_Cd, Descr  from ref_item_customer where Customer_Cd not in ('" & _
                       cmbCust1.SelectedValue & "') order by Descr", cmbCust2)
            cmbCust2.Items.Add("")
            cmbCust2.SelectedValue = ""

        End If
    End Sub
    Protected Sub cmbCust2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbCust2.SelectedIndexChanged
        If cmbCust2.SelectedValue = "" Then
            tr_Cus3.Visible = False
            tr_Cus4.Visible = False
            tr_Cus5.Visible = False

            cmbCust3.SelectedValue = ""
            cmbCust4.SelectedValue = ""
            cmbCust5.SelectedValue = ""
            txtCust2.Text = ""
            txtCust3.Text = ""
            txtCust4.Text = ""
            txtCust5.Text = ""
        Else
            tr_Cus3.Visible = True
            txtCust2.Text = txtItemCd.Text
            BuildCombo("select Customer_Cd, Descr  from ref_item_customer where Customer_Cd not in ('" & _
                       cmbCust1.SelectedValue & "','" & cmbCust2.SelectedValue & "') order by Descr", cmbCust3)
            cmbCust3.Items.Add("")
            cmbCust3.SelectedValue = ""
        End If
    End Sub
    Protected Sub cmbCust3_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbCust3.SelectedIndexChanged
        If cmbCust3.SelectedValue = "" Then
            tr_Cus4.Visible = False
            tr_Cus5.Visible = False

            cmbCust4.SelectedValue = ""
            cmbCust5.SelectedValue = ""
            txtCust3.Text = ""
            txtCust4.Text = ""
            txtCust5.Text = ""
        Else
            tr_Cus4.Visible = True
            txtCust3.Text = txtItemCd.Text
            BuildCombo("select Customer_Cd, Descr  from ref_item_customer where Customer_Cd not in ('" & _
                       cmbCust1.SelectedValue & "','" & cmbCust2.SelectedValue & "','" & _
                       cmbCust3.SelectedValue & "') order by Descr", cmbCust4)
            cmbCust4.Items.Add("")
            cmbCust4.SelectedValue = ""
        End If
    End Sub
    Protected Sub cmbCust4_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbCust4.SelectedIndexChanged
        If cmbCust4.SelectedValue = "" Then
            tr_Cus5.Visible = False
            cmbCust5.SelectedValue = ""
            txtCust4.Text = ""
            txtCust5.Text = ""
        Else
            tr_Cus5.Visible = True
            txtCust4.Text = txtItemCd.Text
            BuildCombo("select Customer_Cd, Descr  from ref_item_customer where Customer_Cd not in ('" & _
                       cmbCust1.SelectedValue & "','" & cmbCust2.SelectedValue & "','" & _
                       cmbCust3.SelectedValue & "','" & cmbCust4.SelectedValue & "') order by Descr", cmbCust5)
            cmbCust5.Items.Add("")
            cmbCust5.SelectedValue = ""
        End If
    End Sub

    Protected Sub cmbSupp1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbSupp1.SelectedIndexChanged
        If cmbSupp1.SelectedValue = "" Then
            tr_Supp2.Visible = False
            tr_Supp3.Visible = False
            tr_Supp4.Visible = False
            tr_Supp5.Visible = False

            cmbSupp2.SelectedValue = ""
            cmbSupp3.SelectedValue = ""
            cmbSupp4.SelectedValue = ""
            cmbSupp5.SelectedValue = ""
            txtSupp1.Text = ""
            txtSupp2.Text = ""
            txtSupp3.Text = ""
            txtSupp4.Text = ""
            txtSupp5.Text = ""
        Else
            tr_Supp2.Visible = True
            txtSupp1.Text = txtItemCd.Text
            BuildCombo("select Supp_Cd, Descr  from ref_item_supplier where Supp_Cd not in ('" & _
                       cmbSupp1.SelectedValue & "') order by Descr", cmbSupp2)
            cmbSupp2.Items.Add("")
            cmbSupp2.SelectedValue = ""
        End If
    End Sub
    Protected Sub cmbSupp2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbSupp2.SelectedIndexChanged
        If cmbSupp2.SelectedValue = "" Then
            tr_Supp3.Visible = False
            tr_Supp4.Visible = False
            tr_Supp5.Visible = False

            cmbSupp3.SelectedValue = ""
            cmbSupp4.SelectedValue = ""
            cmbSupp5.SelectedValue = ""
            txtSupp2.Text = ""
            txtSupp3.Text = ""
            txtSupp4.Text = ""
            txtSupp5.Text = ""
        Else
            tr_Supp3.Visible = True
            txtSupp2.Text = txtItemCd.Text
            BuildCombo("select Supp_Cd, Descr  from ref_item_supplier where Supp_Cd not in ('" & _
                       cmbSupp1.SelectedValue & "','" & cmbSupp2.SelectedValue & "') order by Descr", cmbSupp3)
            cmbSupp3.Items.Add("")
            cmbSupp3.SelectedValue = ""
        End If
    End Sub
    Protected Sub cmbSupp3_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbSupp3.SelectedIndexChanged
        If cmbSupp3.SelectedValue = "" Then
            tr_Supp4.Visible = False
            tr_Supp5.Visible = False

            cmbSupp4.SelectedValue = ""
            cmbSupp5.SelectedValue = ""
            txtSupp3.Text = ""
            txtSupp4.Text = ""
            txtSupp5.Text = ""
        Else
            tr_Supp4.Visible = True
            txtSupp3.Text = txtItemCd.Text
            BuildCombo("select Supp_Cd, Descr  from ref_item_supplier where Supp_Cd not in ('" & _
                       cmbSupp1.SelectedValue & "','" & cmbSupp2.SelectedValue & "','" & _
                       cmbSupp3.SelectedValue & "') order by Descr", cmbSupp4)
            cmbSupp4.Items.Add("")
            cmbSupp4.SelectedValue = ""
        End If
    End Sub
    Protected Sub cmbSupp4_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbSupp4.SelectedIndexChanged
        If cmbSupp4.SelectedValue = "" Then
            tr_Supp5.Visible = False
            cmbSupp5.SelectedValue = ""
            txtSupp4.Text = ""
            txtSupp5.Text = ""
        Else
            tr_Supp5.Visible = True
            txtSupp4.Text = txtItemCd.Text
            BuildCombo("select Supp_Cd, Descr  from ref_item_supplier where Supp_Cd not in ('" & _
                       cmbSupp1.SelectedValue & "','" & cmbSupp2.SelectedValue & "','" & _
                       cmbSupp3.SelectedValue & "','" & cmbSupp4.SelectedValue & "') order by Descr", cmbSupp5)
            cmbSupp5.Items.Add("")
            cmbSupp5.SelectedValue = ""
        End If
    End Sub
  
    Protected Sub cmdClose_Click(sender As Object, e As EventArgs) Handles cmdClose.Click
        vScript = "window.close();"
    End Sub
End Class
