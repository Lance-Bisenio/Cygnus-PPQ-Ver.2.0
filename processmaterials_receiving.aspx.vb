Imports denaro
Imports System
Imports item_details
Partial Class processmaterials_receiving
    Inherits System.Web.UI.Page
    Dim vSQL As String = ""
    Dim c As New SqlClient.SqlConnection
    Public vScript As String = ""
    Public vPendingItem As String = ""
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        If Session("uid") = "" Then
            'Response.Redirect("http://" & SERVERIP & "/" & SITENAME & "/")
        End If

        'If Not CanRun(Session("caption"), Request.Item("id")) Then 
        '	Session("denied") = "1"
        '          Server.Transfer("main.aspx")
        '          Exit Sub
        'End If

        If Not IsPostBack Then

            BuildCombo("select Proc_Cd, Descr from ref_item_process order by Descr", DDLWarehouseList)
            DDLWarehouseList.Items.Add("Main Warehouse")
            DDLWarehouseList.SelectedValue = "Main Warehouse"

            BuildCombo("select Type_Cd, Descr from ref_item_type order by Descr", cmbItemType)
            cmbItemType.Items.Add("All")
            cmbItemType.SelectedValue = "All"

            BuildCombo("select Type_Cd, Descr from Item_warehouse_trantype order by Descr", DDLTranType)

        End If
        GetAllReleaseIted()
    End Sub
    Private Sub GetAllReleaseIted()

        Dim c As New SqlClient.SqlConnection
        Dim cm As New SqlClient.SqlCommand
        Dim rs As SqlClient.SqlDataReader
        c.ConnectionString = connStr

        Try
            c.Open()
            cm.Connection = c
        Catch ex As SqlClient.SqlException
            ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "alert('Database connection error.');", True)
            Exit Sub
        End Try


        vSQL = "select Type_Cd, SubLabel, (select count(TranId) from item_transfer where Type_Cd=TranType and DateReceived is null) as Ctr " _
            & "from Item_warehouse_trantype " _
            & "order by Descr"

        cm.CommandText = vSQL
        rs = cm.ExecuteReader
        Do While rs.Read

            If rs("Ctr") = 0 Then
                vPendingItem += "<li class='nav-item'>" _
                        & "<a href='#' class='p-2 sidebar-link'>" & rs("SubLabel") & "" _
                        & "" _
                    & "</a></li>"
            Else
                vPendingItem += "<li class='nav-item'>" _
                        & "<a href='#' id='" & rs("Type_Cd") & "' " _
                        & "class='p-2 sidebar-link' data-toggle='modal' data-target='#ItemLisModal'>" & rs("SubLabel") & "" _
                        & "<span class='badge badge-pill badge-danger ml-2' >" & rs("Ctr") & "</span>" _
                    & "</a></li>"
            End If


        Loop

        rs.Close()


        c.Close()
        c.Dispose()
        cm.Dispose()

        ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "ShowDetails();", True)
    End Sub

End Class
