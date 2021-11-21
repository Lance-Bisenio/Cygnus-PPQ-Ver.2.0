Imports denaro
Partial Class maintenance_ref
    Inherits System.Web.UI.Page
    Public vScript As String = ""
    Public vMenus As String = ""

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Session("uid") = "" Then
            Server.Transfer("index.aspx")
            Exit Sub
        End If

        If Not CanRun(Session("caption"), Request.Item("id")) Then
            Session("denied") = "1"
            Server.Transfer("main.aspx")
            Exit Sub
        End If

        Dim c As New SqlClient.SqlConnection(connStr)
        c.Open()
        Dim cm As New SqlClient.SqlCommand("select * from evolvemenus where ParentId='" & Request.Item("id") & "' order by SeqId", c)

        'Response.Write("select * from evolvemenus where ParentId='" & Request.Item("id") & "' order by SeqId")

        Dim rs As SqlClient.SqlDataReader

        Try
            rs = cm.ExecuteReader
            Do While rs.Read
                'vMenus += "<a href='#' onclick='showModule(""" & rs("Dependencies") & """, """ & rs("Label_Caption") & """)'>" & rs("Label_Caption") & "</li>"
                vMenus += "<input type='button' onclick='showModule(""" & rs("Dependencies") & """, """ & _
                    rs("Label_Caption") & """, """ & rs("Menu_Caption") & """)' id='" & rs("Menu_Caption") & "' name='' value='" & rs("Label_Caption") & "' class='RefList' />"
            Loop
        Catch ex As Exception
        Finally
            c.Close()
            cm.Dispose()
            c.Dispose()
        End Try


        'If Not rs.Read Then
        '    vScript = "alert('Module cannot be found. Please contact your administrator.');"
        '    rs.Close()
        '    GoTo skip
        'Else

        'End If

        
        'skip:
        'c.Close()
        'cm.Dispose()
        'If Not IsPostBack Then
        '    lblScrId.Value = Request.Item("id")
        '    DataRefresh()
        'Else
        '    DataRefresh()
        'End If
    End Sub
End Class
