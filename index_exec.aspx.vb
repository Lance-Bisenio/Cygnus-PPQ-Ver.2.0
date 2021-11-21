Imports denaro
Imports System.Data.SqlClient
Partial Class index2_exec
    Inherits System.Web.UI.Page
    Public msg As String = ""
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Request.HttpMethod = "POST" Then
            Check4Updates()
            If Request.Form("usrname") <> "" Or Request.Form("passwd") <> "" Then
                Dim usrname As String = Request.Form("usrname")
                Dim passwd As String = Request.Form("passwd")
                Dim cn As New SqlClient.SqlConnection
                Dim cm As New SqlClient.SqlCommand
                Dim cmRef As New SqlClient.SqlCommand
                Dim rsRef As SqlClient.SqlDataReader
                Dim rs As SqlClient.SqlDataReader

                'default to not licensed
                Dim vLicensed As Boolean = False
                Dim vExpiryDate As Date = Now
                Dim vCtr As Integer = 99
                Dim vMaxCount As Integer = 0

                cn.ConnectionString = connStr
                cn.Open()

                cm.Connection = cn
                cmRef.Connection = cn



                cm.CommandText = "select * from user_list where User_Id='" & usrname & "'"
                rs = cm.ExecuteReader
                Try
                    If rs.Read Then
                        If rs("UserPassword") = passwd Then
                            'If Now > CDate("2013/06/19") Then GoTo expired
                            'check if licensed or not
                            cmRef.CommandText = "select Licensed,Actr,ExpiryDate,MaxCount from glsyscntrl"
                            rsRef = cmRef.ExecuteReader
                            If rsRef.Read Then
                                vLicensed = IIf(IsDBNull(rsRef("Licensed")), False, Math.Abs(Val(rsRef("Licensed"))) = 1 Or rsRef("Licensed"))
                                vCtr = IIf(IsDBNull(rsRef("Actr")), 99, rsRef("Actr"))
                                vExpiryDate = IIf(IsDBNull(rsRef("ExpiryDate")), Now, rsRef("ExpiryDate"))
                                vMaxCount = IIf(IsDBNull(rsRef("MaxCount")), 0, rsRef("MaxCount"))
                            End If
                            rsRef.Close()

                            If Not vLicensed Then   'check other parameter limits
                                If Now > vExpiryDate Then
expired:
                                    msg = "Trial version had elapsed. " 'Please purchase a licensed version from Evolve Integrated Software Solutions at www.evolvesoftwaresolutions.com"
                                    Exit Sub
                                End If
                                If vCtr > vMaxCount Then
                                    GoTo expired
                                Else
                                    cmRef.CommandText = "update glsyscntrl set Actr=Actr + 1"
                                    cmRef.ExecuteNonQuery()
                                End If
                            End If

                            msg = ""
                            'UpdateSalaryHist(cn)
                            'Check4LeaveCredits(cn)

                            Session("caption") = IIf(IsDBNull(rs("Caption")), "", rs("Caption"))
                            Session("userlevel") = IIf(IsDBNull(rs("UserLevel")), 0, rs("UserLevel"))
                            Session("agencylist") = IIf(IsDBNull(rs("AgencyCd")), "", rs("AgencyCd"))
                            Session("uid") = usrname
                            Session("EmpPos") = IIf(IsDBNull(rs("Position")), "", "Position : " & rs("Position"))
                            Session("EmpFullName") = IIf(IsDBNull(rs("FullName")), "", rs("FullName"))
                            Session("sessionid") = Session.SessionID

                            'Session("Catglist") = IIf(IsDBNull(rs("CategoryCd")), "", rs("CategoryCd"))     'list of category
                            'Session("Statuslist") = IIf(IsDBNull(rs("StatusCd")), "", rs("StatusCd"))       'list of status
                            'Session("deptlist") = IIf(IsDBNull(rs("DeptCd")), "", rs("DeptCd"))             'list of supervisor

                            'Session("rclist") = IIf(IsDBNull(rs("Rc_Cd")), "", rs("Rc_Cd"))

                            'Session("sectionlist") = IIf(IsDBNull(rs("SectionCd")), "", rs("SectionCd"))
                            'Session("divlist") = IIf(IsDBNull(rs("DivCd")), "", rs("DivCd"))
                            'Session("unitlist") = IIf(IsDBNull(rs("UnitCd")), "", rs("UnitCd"))
                            'Session("typelist") = IIf(IsDBNull(rs("EmploymentType")), "", rs("EmploymentType"))
                            'Session("EmpEmail") = IIf(IsDBNull(rs("Email")), "", "Email : " & rs("Email"))

                            Session("sessionid") = Session("sessionid").ToString.Substring(Session("sessionid").ToString.IndexOf("=") + 1)

                            EventLog(Session("uid"), Request.ServerVariables("REMOTE_ADDR"), "LOGIN", _
                                "", "", "Successful LogIn on " & Format(Now, "yyyy/MM/dd HH:mm:ss"), "LOGIN")
                            If msg = "" Then
                                msg = "ok"
                            End If
                        Else
                            EventLog(usrname, Request.ServerVariables("REMOTE_ADDR"), "LOGIN", _
                                "", "", "Invalid LogIn Password Attempt on " & Format(Now, "yyyy/MM/dd HH:mm:ss"), "LOGIN")
                            msg = "nok"
                        End If
                    Else
                        EventLog(usrname, Request.ServerVariables("REMOTE_ADDR"), "LOGIN", _
                            "", "", "Invalid LogIn Userid Attempt on " & Format(Now, "yyyy/MM/dd HH:mm:ss"), "LOGIN")
                        msg = "nok"
                    End If
                    rs.Close()
                Catch ex As sqlclient.sqlexception
                    msg = ex.Message.ToString
                End Try
                cmRef.Dispose()
                cm.Dispose()

                cn.Close()
                cn.Dispose()
            End If
        Else
            Session.RemoveAll()
            Server.Transfer("index2.aspx")
        End If
    End Sub
    
    Private Sub Check4Updates()
        Dim c As New SqlClient.SqlConnection(connStr)
        Dim cm As New SqlClient.SqlCommand
        Dim a As IO.FileInfo
        Dim sr As IO.StreamReader
        Dim vFilename As String = Server.MapPath(".") & "\updates.dat"

        Try
            cm.Connection = c
            c.Open()
            If IO.File.Exists(vFilename) Then
                a = New IO.FileInfo(vFilename)
                sr = IO.File.OpenText(vFilename)
                Do While Not sr.EndOfStream
                    cm.CommandText = sr.ReadLine
                    cm.ExecuteNonQuery()
                Loop
                sr.Close()
                sr.Dispose()
                a.Delete()
            End If
            c.Close()
            c.Dispose()
            cm.Dispose()
            a = Nothing
        Catch ex As SqlClient.SqlException
            'Exit Sub
        End Try
    End Sub
    
End Class
