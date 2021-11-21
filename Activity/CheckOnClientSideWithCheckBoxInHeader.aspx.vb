Imports System.IO
Imports System.Collections.Generic

Partial Class CheckOnClientSideWithCheckBoxInHeader
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            BindData()
        End If
    End Sub

    Private Sub BindData()
        Dim dirInfo As New DirectoryInfo(Request.PhysicalApplicationPath)

        FileList.DataSource = dirInfo.GetFiles()
        FileList.DataBind()
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

    Protected Sub FileList_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles FileList.PageIndexChanging
        FileList.PageIndex = e.NewPageIndex

        BindData()
    End Sub

  Protected Sub FileList_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles FileList.DataBound
      'Each time the data is bound to the grid we need to build up the CheckBoxIDs array

      'Get the header CheckBox
      Dim cbHeader As CheckBox = CType(FileList.HeaderRow.FindControl("HeaderLevelCheckBox"), CheckBox)

      'Run the ChangeCheckBoxState client-side function whenever the
      'header checkbox is checked/unchecked
      cbHeader.Attributes("onclick") = "ChangeAllCheckBoxStates(this.checked);"

      'Add the CheckBox's ID to the client-side CheckBoxIDs array
      Dim ArrayValues As New List(Of String)
      ArrayValues.Add(String.Concat("'", cbHeader.ClientID, "'"))

      For Each gvr As GridViewRow In FileList.Rows
          'Get a programmatic reference to the CheckBox control
          Dim cb As CheckBox = CType(gvr.FindControl("RowLevelCheckBox"), CheckBox)

          'If the checkbox is unchecked, ensure that the Header CheckBox is unchecked
          cb.Attributes("onclick") = "ChangeHeaderAsNeeded();"

          'Add the CheckBox's ID to the client-side CheckBoxIDs array
          ArrayValues.Add(String.Concat("'", cb.ClientID, "'"))
      Next

      'Output the array to the Literal control (CheckBoxIDsArray)
      CheckBoxIDsArray.Text = "<script type=""text/javascript"">" & vbCrLf & _
                              "<!--" & vbCrLf & _
                              String.Concat("var CheckBoxIDs =  new Array(", String.Join(",", ArrayValues.ToArray()), ");") & vbCrLf & _
                              "// -->" & vbCrLf & _
                              "</script>"
  End Sub
End Class
