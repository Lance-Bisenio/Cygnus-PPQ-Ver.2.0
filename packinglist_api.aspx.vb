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
        Dim CompTranId As Int64 = 0

        vSQL = "select count(BatchNo) from prod_completion where BatchNo='" & pBatchNo & "'"
        APIStatus = GetRef(vSQL, "")

        Try
            vSQL = "select TranId from prod_completion where BatchNo='" & pBatchNo & "'"
            CompTranId = GetRef(vSQL, "")

            vSQL = "update prod_packinglist_details set " _
            & "PrepBy='" & Session("uid") & "', " _
            & "DatePrep='" & Now & "', " _
            & "IsAvailable=1 " _
            & "where CompletionTranId='" & CompTranId & "'"
            CreateRecord(vSQL)
        Catch ex As Exception

        End Try


        Response.Write(APIStatus)
    End Sub

End Class
