
Namespace item_details
    Public Module Item_Function
        Public ItemConnStr As String = System.Configuration.ConfigurationManager.AppSettings.Get("connstr")

        Public Function SeachItem_GCAS(ByVal pGCAS_Code As String) As String
            Dim cm As New SqlClient.SqlCommand
            Dim rs As SqlClient.SqlDataReader
            Dim c As New SqlClient.SqlConnection
            Dim vSQL As String = ""
            Dim vResult As String = ""

            c.ConnectionString = ItemConnStr
            c.Open()
            cm.Connection = c

            vSQL = "select Item_Cd from ref_item_catalog where Alt_Cd ='" & pGCAS_Code & "'"
            cm.CommandText = vSQL

            rs = cm.ExecuteReader
            If rs.Read Then
                vResult += IIf(IsDBNull(rs(0)), "null", rs(0))
            End If

            rs.Close()
            cm.Dispose()
            c.Close()
            c.Dispose()

            Return vResult

        End Function


        Public Function GetGCAS_List(ByVal pItemCode As String) As String
            Dim cm As New SqlClient.SqlCommand
            Dim rs As SqlClient.SqlDataReader
            Dim c As New SqlClient.SqlConnection
            Dim vSQL As String = ""
            Dim vResult As String = ""

            c.ConnectionString = ItemConnStr
            c.Open()
            cm.Connection = c

            vSQL = "select Alt_Cd from ref_item_catalog where  Item_Cd='" & pItemCode & "'"
            cm.CommandText = vSQL

            rs = cm.ExecuteReader
            'Do While rs.Read 
            '    vGAS += rs("ItemList") & "<br/>" 
            'Loop
            'rs.Close()


            If rs.Read Then
                vResult += IIf(IsDBNull(rs(0)), "null", rs(0)) & "<br/>"
            End If

            rs.Close()
            cm.Dispose()
            c.Close()
            c.Dispose()

            Return vResult

        End Function

        Public Sub Helper_ModifyRecords(ByVal pSQL As String)
            Dim vSQL = pSQL
            Dim c As New SqlClient.SqlConnection
            Dim cm As New SqlClient.SqlCommand
            Dim vResult As String = ""

            c.ConnectionString = ItemConnStr
            c.Open()
            cm.Connection = c

            cm.CommandText = vSQL 
            cm.ExecuteNonQuery()

            cm.Dispose()
            c.Close()
            c.Dispose()

        End Sub
    End Module
End Namespace