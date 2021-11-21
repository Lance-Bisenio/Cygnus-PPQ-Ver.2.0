Imports denaro
Partial Class evolvemenus
    Inherits System.Web.UI.Page
    Public vMenus As String = ""
    Public vEmpInfo As String = ""
    Public vEmpPic As String = ""
    Public vActiveEmp As String = ""
    Public vScript As String = ""
    Public vPage As String = ""
    Dim vSQL As String = ""
    Dim c As New SqlClient.SqlConnection(connStr)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'If Not IsPostBack Then
        Dim cm As New SqlClient.SqlCommand
        Dim rs As SqlClient.SqlDataReader
        Dim vCtr As Integer = 1

        vEmpPic = Session("uid")
        vActiveEmp = Session("EmpFullName")
        vEmpInfo = Session("EmpPos") & "<br>" & Session("EmpEmail")

        cm.Connection = c
        Response.Write(vSQL)
        vSQL = "select * from evolvemenus where SystemName='PPQ' and Params is not null order by SeqId"
        cm.CommandText = vSQL


        c.Open()
        rs = cm.ExecuteReader
        vMenus = ""

        vMenus += "<tr>"
        Do While rs.Read
            vMenus += "<td style='color:#000; font-size:12px; font-family:Arial;' " _
                & "onclick='showModule(""" & rs("Dependencies") & """, """ & rs("Label_Caption") & """)'>" _
                & "<img alt='' src='" & rs("Params") & "' style='margin-top:11px; width:47px;' />" _
                & "<br />" & rs("Label_Caption") & "</td>"

            If vCtr = 3 Then
                vMenus += "</tr><tr>"
                vCtr = 1
            Else
                vCtr += 1
            End If
             
            'vMenus += "<div class='col_1'>" & _
            '    "<h5>" & rs("Label_Caption") & "</h5>"
            ''GetChild(rs("Menu_Caption"))
            'vMenus += "</div>"
        Loop

        rs.Close()
        cm.Dispose()
        c.Close()

        'End If
    End Sub
    Private Sub GetChild(ByRef vParent As String)
        Dim cm As New SqlClient.SqlCommand
        Dim rs As SqlClient.SqlDataReader

        cm.Connection = c
        vSQL = "select * from evolvemenus where SystemName='PPQ' and ParentId='" & vParent & "' order by SeqId"
        cm.CommandText = vSQL



        rs = cm.ExecuteReader

        vMenus += "<ul>"
        Do While rs.Read

            vMenus += "<li><a href='#' onclick='showModule(""" & rs("Dependencies") & """, """ & rs("Label_Caption") & """)'>" & rs("Label_Caption") & "</a></li>"

        Loop
        vMenus += "</ul>"
        rs.Close()
        cm.Dispose()
    End Sub

    Protected Sub SignOut_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SignOut.Click
        Session.RemoveAll()
        Server.Transfer("index.aspx")
    End Sub
End Class
