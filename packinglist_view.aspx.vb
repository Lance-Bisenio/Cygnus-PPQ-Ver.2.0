Imports denaro
Imports System
Imports item_details
Imports System.Runtime.ConstrainedExecution

Partial Class packinglist_view
    Inherits System.Web.UI.Page
    Dim vSQL As String = ""
    Dim c As New SqlClient.SqlConnection
    Public vData As String = ""
    Dim Pallet As Integer = 0
    Dim PalletItem As Integer = 0

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        If Session("uid") = "" Then
            'Response.Redirect("http://" & SERVERIP & "/" & SITENAME & "/")
        End If

        If Not IsPostBack Then

        End If
        'Response.Write(Session("PListID"))
        GetPackingListDetails()
        GetPackingListItem()
    End Sub

    Private Sub GetPackingListDetails()

        Dim Ctr As Integer = 1
        Dim c As New SqlClient.SqlConnection
        Dim cm As New SqlClient.SqlCommand
        Dim rs As SqlClient.SqlDataReader
        Dim SQLStr As String = ""


        c.ConnectionString = connStr

        Try
            c.Open()
            cm.Connection = c
        Catch ex As SqlClient.SqlException
            ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "alert('Database connection error.');", True)
            Exit Sub
        End Try

        vSQL = "select PODate, PONO, DeliveryDate, Item_Cd,JONO,JODate, PalletCnt, PalletItemCnt," _
            & "(select SalesOrderNo from jo_header where JobOrderNo=z.JONO) as SONO, " _
            & "(select StartDate from jo_header where JobOrderNo=z.JONO) as StartDate, " _
            & "(select Descr + ' ' + Descr1  from item_master a where a.Item_Cd= z.Item_Cd) as ItemName,  " _
            & "(select Alt_Cd  from ref_item_catalog b where b.Item_Cd=z.Item_Cd and b.Acct_Cd=z.CustomerId) as ItemGCAS,  " _
            & "(select Descr from ref_item_customer where Customer_Cd=CustomerId) as CustName " _
            & "from prod_packinglist z " _
            & "where BatchNo='" & Session("PListID") & "'"

        'Response.Write(vSQL)

        cm.CommandText = vSQL
        rs = cm.ExecuteReader


        If rs.Read Then
            lblCust.InnerText = rs("CustName")
            lblProd.InnerText = rs("ItemName")
            lbProdCode.InnerText = rs("Item_Cd")
            lblGCAS.InnerText = rs("ItemGCAS")

            lblPO.InnerText = IIf(IsDBNull(rs("PONO")), "", rs("PONO"))
            lblJO.InnerText = rs("JONO")
            lblSO.InnerText = rs("SONO")

            lblJODate.InnerText = IIf(IsDBNull(rs("JODate")) Or rs("JODate") = "01/01/1900", "-", rs("JODate"))
            lblPODate.InnerText = IIf(IsDBNull(rs("PODate")), "", rs("PODate"))
            lblDelDate.InnerText = IIf(IsDBNull(rs("DeliveryDate")), "", rs("DeliveryDate"))

            Pallet = IIf(IsDBNull(rs("PalletCnt")), "", rs("PalletCnt"))
            PalletItem = IIf(IsDBNull(rs("PalletItemCnt")), "", rs("PalletItemCnt"))
        End If
        rs.Close()

        c.Close()
        c.Dispose()
        cm.Dispose()

    End Sub

    Private Sub GetPackingListItem()

        Dim Ctr As Integer = 1
        Dim c As New SqlClient.SqlConnection
        Dim cm As New SqlClient.SqlCommand
        Dim rs As SqlClient.SqlDataReader
        Dim SQLStr As String = ""
        Dim PalletVal As Integer = 1
        Dim CtrItem As Integer = 1

        c.ConnectionString = connStr

        Try
            c.Open()
            cm.Connection = c
        Catch ex As SqlClient.SqlException
            ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "alert('Database connection error.');", True)
            Exit Sub
        End Try

        vSQL = "select BatchNo, CoreWeight, NetWeight, GrossWeight, Qty, UOM, Meter, TtlPcs, TtlPcsBox " _
            & "from prod_completion where TranType='COMPLETION' and IsDeleted is null " _
            & "and TranId in " _
            & "(select CompletionTranId from prod_packinglist_details where BatchNo='" & Session("PListID") & "')"
        'Response.Write(vSQL)

        cm.CommandText = vSQL
        rs = cm.ExecuteReader

        Do While rs.Read
            vData += "<tr>" _
               & "<td style='width: 30px'>" & Ctr & "</td>" _
               & "<td>" & rs("BatchNo") & "</td>" _
               & "<td>" & rs("CoreWeight") & "</td>" _
               & "<td>" & rs("NetWeight") & "</td>" _
               & "<td>" & rs("GrossWeight") & "</td>" _
               & "<td>" & rs("TtlPcs") & "</td>" _
               & "<td>" & FormatNumber(rs("Qty"), "000") & "</td>" _
               & "<td>" & IIf(rs("UOM") = "", "KGS", rs("UOM")) & "</td>"

            If CtrItem > PalletItem Then
                PalletVal += 1
                CtrItem = 1
            End If

            If PalletItem = 0 Then
                vData += "<td style='width: 70px'>1</td>"
            Else
                vData += "<td style='width: 70px'>" & PalletVal & "</td>"
            End If

            CtrItem += 1

            vData += "</tr>"
            Ctr += 1
        Loop

        rs.Close()

        c.Close()
        c.Dispose()
        cm.Dispose()

    End Sub

End Class
