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
        Select Case Request.Item("trantype")
            Case "Del"
                Response.Write("delete item")
            Case "Add"
                Response.Write("add item")
            Case Else
                'Response.Write(Request.Item("jono"))
                CheckItem(Request.Item("batchno"), Request.Item("jono"), Request.Item("source"))
        End Select
    End Sub

    Private Sub CheckItem(pBatchNo As String, pPONO As String, pSource As String)
        Dim APIStatus As String
        Dim CompTranId As Int64 = 0

        vSQL = "select count(BatchNo) from prod_completion where " _
            & "BatchNo='" & pBatchNo & "' and " _
            & "JONO='" & pPONO & "' and " _
            & "Sect_Cd='" & pSource & "'"
        APIStatus = GetRef(vSQL, "")


        Try
            vSQL = "select TranId from prod_completion where " _
                & "BatchNo='" & pBatchNo & "' and " _
                & "JONO='" & pPONO & "' and " _
                & "Sect_Cd='" & pSource & "'"
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
