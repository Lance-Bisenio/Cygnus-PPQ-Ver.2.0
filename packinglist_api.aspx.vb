Imports denaro
Imports System
Imports item_details
Imports System.Runtime.ConstrainedExecution
Imports System.ServiceModel.PeerResolvers
Imports System.Net.NetworkInformation

Partial Class warehouse
    Inherits System.Web.UI.Page
    Dim vSQL As String = ""
    Dim c As New SqlClient.SqlConnection
    Public vScript As String = ""
    Public Complist As String = ""
    Dim CompTranId As Int64 = 0

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        'Response.Write(Now & Request.Item("batchno"))

        vSQL = "select TranId from prod_completion where " _
                & "BatchNo='" & Request.Item("batchno") & "' and " _
                & "JONO='" & Request.Item("jono") & "' and " _
                & "Sect_Cd='" & Request.Item("source") & "'"
        CompTranId = GetRef(vSQL, "")


        Select Case Request.Item("trantype")
            Case "Del"
                DeleteItem()
            Case "Add"
                AddItem()
            Case Else
                'Response.Write(Request.Item("jono"))
                CheckItem(Request.Item("batchno"), Request.Item("jono"), Request.Item("source"))
        End Select
    End Sub
    Private Sub DeleteItem()
        vSQL = "delete from prod_packinglist_details " _
                & "where CompletionTranId='" & CompTranId & "'"
        CreateRecord(vSQL)
        Response.Write(vSQL)
    End Sub
    Private Sub AddItem()
        vSQL = "insert into prod_packinglist_details (BatchNo, JONO, CompletionTranId, Customer, CreatedBy, DateCreated, Remarks) values " _
            & "('" & Session("PListID") & "', '" & Request.Item("jono") & "', " & CompTranId & ", '" & Session("CustomerId") & "', '" & Session("uid") & "', '" & Now & "', 'Added by: " & Session("uid") & "')"
        CreateRecord(vSQL)
        Response.Write(vSQL)
    End Sub

    Private Sub CheckItem(pBatchNo As String, pPONO As String, pSource As String)
        Dim APIStatus As String


        vSQL = "select count(BatchNo) from prod_completion where " _
            & "BatchNo='" & pBatchNo & "' and " _
            & "JONO='" & pPONO & "' and " _
            & "Sect_Cd='" & pSource & "'"
        APIStatus = GetRef(vSQL, "")

        Try
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
