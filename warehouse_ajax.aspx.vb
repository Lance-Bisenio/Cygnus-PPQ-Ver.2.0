Imports denaro
Partial Class warehouse_ajax
    Inherits System.Web.UI.Page
    Public Data As String = ""
    Dim vSQL As String = ""

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim PendingItemsList As String = Request.Item("warehouseType")

        'If PendingItemsList.Trim <> "" Then
        GetReleaseItemList(PendingItemsList)
        'End If

    End Sub

    Private Sub GetReleaseItemList(TranType As String)

        Dim c As New SqlClient.SqlConnection
        Dim cm As New SqlClient.SqlCommand
        Dim rs As SqlClient.SqlDataReader
        Dim Ctr As Integer = 0
        Dim TrIdName As String = ""
        c.ConnectionString = connStr

        Try
            c.Open()
            cm.Connection = c
        Catch ex As SqlClient.SqlException
            ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "alert('Database connection error.');", True)
            Exit Sub
        End Try


        vSQL = "select Item_Cd, " _
            & "(select Descr + ' ' + Descr1  from item_master b where a.Item_Cd=b.Item_Cd) as ItemDescr, " _
            & "LotNo, Qty, DateCreated  " _
            & "from item_transfer a where TranType='" & TranType & "'"
        Data = vSQL
        cm.CommandText = vSQL
        rs = cm.ExecuteReader
        Do While rs.Read
            Ctr += 1
            TrIdName = Ctr & "tr" & rs("Item_Cd")

            Data += "<tr id='" & TrIdName & "'>" _
                        & "<td>" & Ctr & "</td>" _
                        & "<td>" & rs("Item_Cd") & "</td>" _
                        & "<td>" & rs("ItemDescr") & "</td>" _
                        & "<td>" & rs("LotNo") & "</td>" _
                        & "<td class='text-right'>" & rs("Qty") & "</td>" _
                        & "<td>" & rs("DateCreated") & "</td>" _
                        & "<td class='text-center'><a href='#' onclick='test()' class='btn btn-danger btn-sm my-1 py-1'>Del</a></td>" _
                    & "</tr>"


        Loop

        rs.Close()


        c.Close()
        c.Dispose()
        cm.Dispose()

    End Sub

End Class
