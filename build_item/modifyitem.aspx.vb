Imports denaro.fis
Partial Class modifyitem
    Inherits System.Web.UI.Page
    Public vScript As String = ""
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("uid") = Nothing Or Session("uid") = "" Then
            vScript = "alert('Login session has expired. Please re-login again.');"
            Exit Sub
        End If

        If Not IsPostBack Then
            txtTitle.Text = "New / Modify Item"
            Dim c As New SqlClient.SqlConnection(connStr)

            Try
                c.Open()
            Catch ex As SqlClient.SqlException
                vScript = "alert('Error occurred while trying to connect to database. Error is: " & _
                    ex.Message.Replace(vbCrLf, "").Replace("'", "") & "');"
                c.Dispose()
                Exit Sub
            End Try

            If Request.Item("pid") <> "" Then
                txtParticularId.Text = Request.Item("pid")
            End If

            Try

                BuildCombo("select CoaType_Cd, Descr from coa_type order by Descr", cmbFilterAcct, c)
                BuildCombo("select AcctCd, AcctCd+' => '+AcctName from coa order by AcctName", cmbAcctCd, c)
                

                If txtParticularId.Text <> "Auto-generated" Then
                    BuildCombo("select distinct ParticularId,Particular from budget_dtl where BudgetId=" & Request.Item("id") & " and ParticularId<>" & txtParticularId.Text & " ", cmbParent, c)
                    'Response.Write("select distinct ParticularId,Particular from budget_dtl where ParticularId<>" & txtParticularId.Text)
                Else
                    BuildCombo("select distinct ParticularId,Particular from budget_dtl where BudgetId=" & Request.Item("id") & " order by Particular", cmbParent, c)
                    'Response.Write("lance test 3" & "select distinct ParticularId,Particular from budget_dtl where BudgetId=" & txtBudgId.Text)

                End If
            Catch ex As SqlClient.SqlException
                vScript = "alert('Error occurred while trying to retrieve reference tables. Error is: " & _
                    ex.Message.Replace(vbCrLf, "").Replace("'", "") & "');"
                c.Close()
                c.Dispose()
                Exit Sub
            End Try
            cmbFilterAcct.Items.Add("All")
            cmbAcctCd.Items.Add("N/A")
            cmbParent.Items.Add("N/A")

            cmbAcctCd.SelectedValue = "N/A"
            cmbParent.SelectedValue = "N/A"
            cmbFilterAcct.SelectedValue = "All"
            txtBudgId.Text = Request.Item("id")

            If Request.Item("pid") <> "" Then
                Dim cm As New SqlClient.SqlCommand
                Dim rs As SqlClient.SqlDataReader

                cm.Connection = c

                cm.CommandText = "select * from budget_dtl where BudgetId=" & txtBudgId.Text & _
                    " and ParticularId=" & txtParticularId.Text
                Try
                    rs = cm.ExecuteReader
                    If rs.Read Then
                        txtParticular.Text = rs("Particular")
                        ' txtLineNum.Text = rs("LineNum")
                        rdoViewBy.SelectedValue = rs("ViewBy")
                        rdoSubAcct.SelectedIndex = rs("SubAcct")

                        If IsDBNull(rs("AcctCd")) Then
                            cmbAcctCd.SelectedValue = "N/A"
                        Else
                            cmbAcctCd.SelectedValue = rs("AcctCd")
                        End If
                        If IsDBNull(rs("ParentId")) Then
                            cmbParent.SelectedValue = "N/A"
                        Else
                            cmbParent.SelectedValue = rs("ParentId")
                        End If
                    End If
                    rs.Close()
                Catch ex As SqlClient.SqlException
                    vScript = "alert('Error occurred while trying to retrieve record. Error is: " & _
                        ex.Message.Replace(vbCrLf, "").Replace("'", "") & "');"
                End Try
            End If
            c.Close()
            c.Dispose()
        End If
    End Sub

    Protected Sub cmdSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        If txtParticular.Text.Trim = "" Then
            vScript = "alert('Particulars field should not be empty.');"
            Exit Sub
        End If

        Dim c As New SqlClient.SqlConnection(connStr)
        Dim cm As New SqlClient.SqlCommand
        Dim rs As SqlClient.SqlDataReader
        Dim vLevel As Integer
        Dim vLastCtr As Integer

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

        ' GET THE LAST COUNT OF PARTICULAR ID ============================================================================================================= 
        cm.CommandText = "select MAX(ParticularId) as vPId from Budget_dtl where BudgetId=" & _
               txtBudgId.Text & " "
        rs = cm.ExecuteReader
        If rs.Read Then
            vLastCtr = rs("vPId") + 1
        End If
        rs.Close()

        vLevel = 1
        If cmbParent.SelectedValue <> "N/A" Then
            'find the level
            cm.CommandText = "select Level from budget_dtl where BudgetId=" & _
                txtBudgId.Text & " and ParticularId=" & cmbParent.SelectedValue
            rs = cm.ExecuteReader
            If rs.Read Then
                vLevel = rs("Level") + 1
            End If
            rs.Close()
        End If

        If txtParticularId.Text = "Auto-generated" Then
            ' Add mode ===================================================================================================================================
            cm.CommandText = "insert into budget_dtl (BudgetId,ParticularId,Particular,AcctCd,ParentId,Level,ViewBy,SubAcct,LineNum) values (" & _
                txtBudgId.Text & ",'" & vLastCtr & "','" & txtParticular.Text.Replace("'", "").Replace("""", "") & "'," & _
                IIf(cmbAcctCd.SelectedValue = "N/A", "null", "'" & cmbAcctCd.SelectedValue & "'") & "," & _
                IIf(cmbParent.SelectedValue = "N/A", "null", "'" & cmbParent.SelectedValue & "'") & _
                "," & vLevel & "," & rdoViewBy.SelectedValue & "," & rdoSubAcct.SelectedValue & ",0 )"
        Else
            'Edit mode ===================================================================================================================================
            cm.CommandText = "update budget_dtl set Particular='" & txtParticular.Text & _
                "',AcctCd=" & IIf(cmbAcctCd.SelectedValue = "N/A", "null", "'" & cmbAcctCd.SelectedValue & "'") & _
                ",ParentId=" & IIf(cmbParent.SelectedValue = "N/A", "null", "'" & cmbParent.SelectedValue & "'") & _
                ",Level=" & vLevel & ",ViewBy=" & rdoViewBy.SelectedValue & ", SubAcct=" & rdoSubAcct.SelectedValue & _
                " where BudgetId=" & txtBudgId.Text & " and ParticularId=" & txtParticularId.Text
        End If

        Try
            cm.ExecuteNonQuery()
            'Response.Write(cm.CommandText)
            vScript = "alert('Changes were successfully saved. Select the Budget again to reload the details.'); window.close();"
        Catch ex As SqlClient.SqlException
            vScript = "alert('Error occurred while trying to save record. Error is: " & _
                ex.Message.Replace(vbCrLf, "").Replace("'", "") & "');"
        Finally
            c.Close()
            c.Dispose()
            cm.Dispose()
        End Try
    End Sub

    Protected Sub cmbFilterAcct_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbFilterAcct.SelectedIndexChanged
        Dim c As New SqlClient.SqlConnection(connStr)
        Try
            c.Open()
        Catch ex As SqlClient.SqlException
            vScript = "alert('Error occurred while trying to connect to database. Error is: " & _
                ex.Message.Replace(vbCrLf, "").Replace("'", "") & "');"
            c.Dispose()
            Exit Sub
        End Try
        BuildCombo("select AcctCd, AcctCd+' => '+AcctName from coa where TranType=" & cmbFilterAcct.SelectedValue & " order by AcctName", cmbAcctCd, c)
        c.Close()
        c.Dispose()
    End Sub

    Protected Sub cmbAcctCd_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbAcctCd.SelectedIndexChanged

        Dim c As New SqlClient.SqlConnection(connStr)
        Dim cm As New SqlClient.SqlCommand
        Dim rs As SqlClient.SqlDataReader 
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
         
        cm.CommandText = "select AcctName from coa where AcctCd='" & cmbAcctCd.SelectedValue & "' "
            rs = cm.ExecuteReader
            If rs.Read Then
            txtParticular.Text = rs("AcctName")
            End If
            rs.Close()
       
        c.Close()
        c.Dispose()
        cm.Dispose()
    End Sub
End Class
