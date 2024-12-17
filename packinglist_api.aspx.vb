Imports denaro
Imports System
Imports item_details
Imports System.Runtime.ConstrainedExecution
Imports System.ServiceModel.PeerResolvers

Partial Class warehouse
    Inherits System.Web.UI.Page
    Dim vSQL As String = ""
    Dim c As New SqlClient.SqlConnection
    Public vScript As String = ""
    Public Complist As String = ""
    Public vPendingItem As String = ""

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        'Response.Write(Now & Request.Item("batchno"))

        CheckItem(Request.Item("batchno"))
    End Sub

    Private Sub CheckItem(pBatchNo As String)
        Dim APIStatus As String

        vSQL = "select count(BatchNo) from prod_completion where BatchNo='" & pBatchNo & "'"
        APIStatus = GetRef(vSQL, "")

        Response.Write(APIStatus)
    End Sub

End Class
