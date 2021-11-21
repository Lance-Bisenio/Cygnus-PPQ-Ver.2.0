Imports System.IO
Imports System.Collections.Generic

Partial Class CheckOnClientSide
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Dim dirInfo As New DirectoryInfo(Request.PhysicalApplicationPath)

            FileList.DataSource = dirInfo.GetFiles()
            FileList.DataBind()
        End If

        'On every page visit we need to build up the CheckBoxIDs array
        For Each gvr As GridViewRow In FileList.Rows
            'Get a programmatic reference to the CheckBox control
            Dim cb As CheckBox = CType(gvr.FindControl("RowLevelCheckBox"), CheckBox)

            ClientScript.RegisterArrayDeclaration("CheckBoxIDs", String.Concat("'", cb.ClientID, "'"))
        Next
    End Sub

    Protected Sub DeleteButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles DeleteButton.Click
        Summary.Text = "The following file would have been deleted:<ul>"

        Dim currentRowsFilePath As String

        'Enumerate the GridViewRows
        For index As Integer = 0 To FileList.Rows.Count - 1
            'Programmatically access the CheckBox from the TemplateField
            Dim cb As CheckBox = CType(FileList.Rows(index).FindControl("RowLevelCheckBox"), CheckBox)

            'If it's checked, delete it...
            If cb.Checked Then
                currentRowsFilePath = FileList.DataKeys(index).Value
                Summary.Text &= String.Concat("<li>", currentRowsFilePath, "</li>")
            End If
        Next

        Summary.Text &= "</ul>"
    End Sub
End Class
