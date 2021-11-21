Namespace denaro
    Partial Class empstep1
        Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

        'This call is required by the Web Form Designer.
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

        End Sub


        Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
            'CODEGEN: This method call is required by the Web Form Designer
            'Do not modify it using the code editor.
            InitializeComponent()
        End Sub

#End Region
        Dim c As New sqlclient.sqlconnection
        Public vScript As String = ""
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load, Me.Load
            If Session("uid") = "" Then
                vScript = "alert('Your login session has expired. Please login again.'); window.close();"
            End If
            If Not IsPostBack Then

                'If Not CanRun(Session("caption"), "41.1") Then
                '    'Server.Transfer("empstep2_1.aspx")
                '    Exit Sub
                'End If

                'lblCaption.Text = "Employee Master Information (1 of 6)"
                'BuildCombo("select Pos_Cd,Position from py_position_ref order by position", cmbPos)
                'BuildCombo("select Status_Code,Descr from py_employee_stat", cmbStatus)
                'BuildCombo("select EmploymentType,Descr from hr_employment_type where exists (select User_Id from rights_list where User_Id='" & _
                '    Session("uid") & "' and property='employmenttype' and Property_Value=EmploymentType) order by Descr", cmbSecurity)

                Dim cm As New SqlClient.SqlCommand
                Dim rs As SqlClient.SqlDataReader
                'BuildCombo("select Rc_Cd,Descr from rc where exists (select User_Id from rights_list where User_Id='" & _
                '    Session("uid") & "' and property='rc' and Property_Value=Rc_Cd) order by Descr", cmbRC)
                'BuildCombo("select AgencyCd,AgencyName from agency where exists (select User_Id from rights_list where User_Id='" & _
                '    Session("uid") & "' and property='agency' and Property_Value=AgencyCd) order by AgencyName", cmbAgency)
                'BuildCombo("select Div_Cd,Descr from hr_div_ref where exists (select User_Id from rights_list where User_Id='" & _
                '    Session("uid") & "' and property='division' and Property_Value=Div_Cd) order by Descr", cmbDiv)
                'BuildCombo("select Dept_Cd,Descr from hr_dept_ref where exists (select User_Id from rights_list where User_Id='" & _
                '    Session("uid") & "' and property='department' and Property_Value=Dept_Cd) order by Descr", cmbDept)
                'BuildCombo("select Section_Cd,Descr from hr_section_ref where exists (select User_Id from rights_list where User_Id='" & _
                '    Session("uid") & "' and property='section' and Property_Value=Section_Cd) order by Descr", cmbSection)
                'BuildCombo("select Unit_Cd,Descr from hr_unit_ref where exists (select User_Id from rights_list where User_Id='" & _
                '    Session("uid") & "' and property='unit' and Property_Value=Unit_Cd) order by Descr", cmbUnit)

                BuildCombo("select Rc_Cd,Descr from ref_emp_costcenter order by Descr", cmbRC)
                BuildCombo("select AgencyCd,AgencyName from ref_emp_agency order by AgencyName", cmbAgency)
                BuildCombo("select Div_Cd,Descr from ref_emp_division order by Descr", cmbDiv)
                BuildCombo("select Dept_Cd,Descr from ref_emp_department order by Descr", cmbDept)
                BuildCombo("select Section_Cd,Descr from ref_emp_section order by Descr", cmbSection)
                BuildCombo("select Unit_Cd,Descr from ref_emp_unit order by Descr", cmbUnit)

                If Session("empid") = "" Then
                    'c.ConnectionString = connStr
                    'c.Open()
                    'cm.Connection = c
                    'cm.CommandText = "SELECT Emp_Cd + 1 FROM py_emp_master WHERE substring(Emp_Cd,1,2) = '" & Format(Now, "yy") & _
                    '                 "' ORDER BY Emp_Cd DESC SET ROWCOUNT 1"
                    'rs = cm.ExecuteReader
                    'If rs.Read Then
                    '    Me.txtId.Text = Format(rs(0), "000000")
                    'Else
                    '    Me.txtId.Text = Format(Now, "yy0001")
                    'End If
                    'Me.txtId.Text = Me.txtId.Text
                    'cm.Dispose()
                    'c.Close()
                End If

                txtDateResign.Enabled = False
                If Session("empid") <> "" Then
                    'txtDateResign.Enabled = True

                    c.ConnectionString = connStr
                    c.Open()
                    cm.Connection = c
                    cm.CommandText = "select Rc_Cd,Agency_Cd,DivCd,DeptCd,SectionCd,UnitCd,Emp_Cd,BarcodeId,Rate_Day, Rate_Month, Rate_Year," & _
                        "Emp_Fname,Emp_Mname,Emp_Lname,NickName,Pos_Cd,EmploymentType,Emp_Status,Date_Resign,Rate_Hrs,Pay_Cd from emp_master where " & _
                        "Emp_Cd='" & Session("empid") & "'"

                    Try
                        rs = cm.ExecuteReader
                        If rs.Read Then
                            txtId.Text = IIf(IsDBNull(rs("Emp_Cd")), "", rs("Emp_Cd"))
                            txtId.Text = IIf(IsDBNull(rs("BarcodeId")), "", rs("BarcodeId"))
                            txtFirst.Text = IIf(IsDBNull(rs("Emp_Fname")), "", rs("Emp_Fname"))
                            txtMiddle.Text = IIf(IsDBNull(rs("Emp_Mname")), "", rs("Emp_Mname"))
                            txtLast.Text = IIf(IsDBNull(rs("Emp_Lname")), "", rs("Emp_Lname"))
                            'txtFirst.Text = IIf(IsDBNull(rs("NickName")), "", rs("NickName"))
                            txtDateResign.Text = IIf(IsDBNull(rs("Date_Resign")), "", rs("Date_Resign"))
                            'chkConfi.Checked = rs("Confidential")
                            rdoPayType.SelectedValue = rs("Pay_Cd")

                            txtHrRate.Text = IIf(IsDBNull(rs("Rate_Hrs")), "0.00", Format(rs("Rate_Hrs"), "#,###,##0.00")) ' Format(rs("Rate_Hrs"), "#,###,##0.00")
                            txtDayRate.Text = IIf(IsDBNull(rs("Rate_Hrs")), "0.00", Format(rs("Rate_Hrs") * 8, "#,###,##0.00"))
                            txtMoRate.Text = IIf(IsDBNull(rs("Rate_Month")), "0.00", Format(rs("Rate_Month"), "#,###,##0.00"))
                            txtYrRate.Text = IIf(IsDBNull(rs("Rate_Year")), "0.00", Format(rs("Rate_Year"), "#,###,##0.00"))

                            cmbRC.SelectedValue = PointData("select Rc_Cd,Descr from ref_emp_costcenter where Rc_Cd='" & rs("Rc_Cd") & "'")
                            cmbAgency.SelectedValue = PointData("select AgencyCd,AgencyName from ref_emp_agency where AgencyCd='" & rs("Agency_Cd") & "'")
                            cmbDiv.SelectedValue = PointData("select Div_Cd,Descr from ref_emp_division where Div_Cd='" & rs("DivCd") & "'")
                            cmbDept.SelectedValue = PointData("select Dept_Cd,Descr from ref_emp_department where Dept_Cd='" & rs("DeptCd") & "'")
                            cmbSection.SelectedValue = PointData("select Section_Cd,Descr from ref_emp_section where Section_Cd='" & rs("SectionCd") & "'")
                            cmbUnit.SelectedValue = PointData("select Unit_Cd,Descr from ref_emp_unit where Unit_Cd='" & rs("UnitCd") & "'")

                            Session("oldval") = "Emp Id=" & txtId.Text & _
                                "|Biometrics Id=" & txtId.Text & _
                                "|First Name=" & txtFirst.Text & _
                                "|Middle Name=" & txtMiddle.Text & _
                                "|Last Name=" & txtLast.Text & _
                                "|Nickname=" & txtFirst.Text & _
                                "|Cost Center=" & IIf(IsDBNull(rs("Rc_Cd")), "", rs("Rc_Cd")) & _
                                "|Agency=" & IIf(IsDBNull(rs("Agency_Cd")), "", rs("Agency_Cd")) & _
                                "|Division=" & IIf(IsDBNull(rs("DivCd")), "", rs("DivCd")) & _
                                "|Department=" & IIf(IsDBNull(rs("DeptCd")), "", rs("DeptCd")) & _
                                "|Section=" & IIf(IsDBNull(rs("SectionCd")), "", rs("SectionCd")) & _
                                "|Unit=" & IIf(IsDBNull(rs("UnitCd")), "", rs("UnitCd"))
                        End If
                        rs.Close()
                    Catch ex As SqlClient.SqlException
                        Response.Write("Error in retrieving data. " & ex.Message)
                    End Try
                    cm.Dispose()
                    c.Close()

                ElseIf Session("fromapplicant") = "1" Then
                    Dim vRC As String = ""
                    Dim vAgency As String = ""
                    Dim vDiv As String = ""
                    Dim vDept As String = ""
                    Dim vSection As String = ""
                    Dim vUnit As String = ""
                    Dim vPos As String = ""
                    Dim iCtr As Integer = 1
                    Dim vStatus As String = ""

                    c.ConnectionString = connStr
                    c.Open()
                    cm.Connection = c
                     
                    cmbRC.SelectedValue = PointData("select Rc_Cd,Descr from ref_emp_costcenter where Rc_Cd='" & vRC & "'")
                    cmbAgency.SelectedValue = PointData("select AgencyCd,AgencyName from ref_emp_agency where AgencyCd='" & vAgency & "'")
                    cmbDiv.SelectedValue = PointData("select Div_Cd,Descr from ref_emp_division where Div_Cd='" & vDiv & "'")
                    cmbDept.SelectedValue = PointData("select Dept_Cd,Descr from ref_emp_department where Dept_Cd='" & vDept & "'")
                    cmbSection.SelectedValue = PointData("select Section_Cd,Descr from ref_emp_section where Section_Cd='" & vSection & "'")
                    cmbUnit.SelectedValue = PointData("select Unit_Cd,Descr from ref_emp_unit where Unit_Cd='" & vUnit & "'")

                End If
            End If
        End Sub

        Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
            Session.Remove("oldval")
            Session.Remove("empid")
            vScript = "window.close();"
        End Sub

        Private Sub txtFirst_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtFirst.Init
            Dim txt As System.Web.UI.WebControls.TextBox
            txt = CType(sender, System.Web.UI.WebControls.TextBox)
            txt.Attributes.Add("onblur", "copyval();")
        End Sub

        Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
            If Page.IsValid Then


                Dim vSQL As String = ""
                Dim cm As New SqlClient.SqlCommand
                ' Dim rs As SqlClient.SqlDataReader

                Dim vDateResign As String = ""
                Dim vDateName As String = ""
                Dim vDateResign2 As String = ""

                c.ConnectionString = connStr
                c.Open()
                cm.Connection = c

                If txtDateResign.Text.Trim <> "" Then
                    If Not IsDate(txtDateResign.Text) Then
                        vScript = "alert('Invalid date format in Date Resign field')"
                        Exit Sub
                    End If
                    vDateName = ", Date_Resign"
                    vDateResign = ",'" & txtDateResign.Text & "'"
                    vDateResign2 = ", Date_Resign='" & txtDateResign.Text & "'"
                Else
                    vDateResign2 = ", Date_Resign=null"
                End If

                If Session("empid") = "" Then 'add mode
Again:
                    vSQL = "insert into emp_master (Rc_Cd,Agency_Cd,DivCd,DeptCd,SectionCd,UnitCd,Emp_Cd,BarcodeId," _
                        & "Emp_Fname,Emp_Mname,Emp_Lname,NickName,Pos_Cd,EmploymentType,Emp_Status,ImgPath," _
                        & "Rate_Hrs,Rate_Day, Rate_Month, Rate_Year, Pay_Cd) values ('" _
                        & cmbRC.SelectedValue & "','" _
                        & cmbAgency.SelectedValue & "','" _
                        & cmbDiv.SelectedValue & "','" _
                        & cmbDept.SelectedValue & "','" _
                        & cmbSection.SelectedValue & "','" _
                        & cmbUnit.SelectedValue & "','" _
                        & RTrim(txtId.Text.Trim) & "','" _
                        & RTrim(txtId.Text.Trim) & "','" _
                        & txtFirst.Text & "','" _
                        & txtMiddle.Text & "','" _
                        & txtLast.Text & "','" _
                        & txtFirst.Text & "','99','99','" _
                        & RTrim(txtId.Text.Trim) & "','../pics/" & txtId.Text & "','" & (txtDayRate.Text / 8) & "','" & txtDayRate.Text & "','" _
                        & txtMoRate.Text & "','" & txtYrRate.Text & "','" & rdoPayType.SelectedValue & "')"
                    Session("empid") = RTrim(txtId.Text.Trim)
                    cm.CommandText = vSQL
                    'Response.Write(vSQL)
                    Try
                        cm.ExecuteNonQuery()
                        Session("newval") = "Emp Id=" & txtId.Text _
                            & "|Biometrics Id=" & txtId.Text _
                            & "|First Name=" & txtFirst.Text _
                            & "|Middle Name=" & txtMiddle.Text _
                            & "|Last Name=" & txtLast.Text _
                            & "|Cost Center=" & cmbRC.SelectedValue _
                            & "|Agency=" & cmbAgency.SelectedValue _
                            & "|Division=" & cmbDiv.SelectedValue _
                            & "|Department=" & cmbDept.SelectedValue _
                            & "|Section=" & cmbSection.SelectedValue _
                            & "|Unit=" & cmbUnit.SelectedValue

                        EventLog(Session("uid"), Request.ServerVariables("REMOTE_ADDR"), "ADD", Session("oldval"), Session("newval"), "Employee ID: " & txtId.Text _
                            & " was added", "201 Profile", c)
                        Session.Remove("oldval")
                        Session.Remove("newval")

                    Catch ex As SqlClient.SqlException
                        txtId.Text = Format(Val(txtId.Text) + 1, "000000")
                        'txtBarcode.Text = txtId.Text
                        'GoTo Again
                    End Try
                    'vScript = "alert('Successfully saved.'); " & _
                    '"opener.document.getElementById('txtRefreshList').value='refresh'; " & _
                    '"opener.document.form1.submit(); window.close();"

                Else 'edit mode
                    'Rc_Cd='" & cmbRC.SelectedValue
                    vSQL = "update emp_master set " _
                        & "Agency_Cd='" & cmbAgency.SelectedValue _
                        & "',DivCd='" & cmbDiv.SelectedValue _
                        & "',DeptCd='" & cmbDept.SelectedValue _
                        & "',SectionCd='" & cmbSection.SelectedValue _
                        & "',UnitCd='" & cmbUnit.SelectedValue _
                        & "',Emp_Cd='" & RTrim(txtId.Text.Trim) _
                        & "',BarCodeId='" & RTrim(txtId.Text.Trim) _
                        & "',Emp_Fname='" & txtFirst.Text _
                        & "',Emp_Mname='" & txtMiddle.Text _
                        & "',Emp_Lname='" & txtLast.Text _
                        & "',NickName='" & txtFirst.Text _
                        & "',Pos_Cd='99',EmploymentType='99',Emp_Status='" & RTrim(txtId.Text.Trim) _
                        & "',Modified_Date='" & Format(Now, "yyy/MM/dd HH:mm:ss") _
                        & "',Modified_By='" & Session("uid") _
                        & "',User_id='" & txtId.Text _
                        & "',Rate_Hrs='" & txtHrRate.Text.Replace(",", "") _
                        & "',Rate_Day='" & txtDayRate.Text.Replace(",", "") _
                        & "',Rate_Month='" & txtMoRate.Text.Replace(",", "") _
                        & "',Rate_Year='" & txtYrRate.Text.Replace(",", "") _
                        & "',Pay_Cd='" & rdoPayType.SelectedValue _
                        & "' " & vDateResign2 & " where Emp_Cd='" & Session("empid") & "'"

                    cm.CommandText = vSQL
                    Try
                        cm.ExecuteNonQuery()
                        Session("empid") = RTrim(txtId.Text.Trim)
                        Session("newval") = "Emp Id=" & txtId.Text _
                            & "|Biometrics Id=" & txtId.Text _
                            & "|First Name=" & txtFirst.Text _
                            & "|Middle Name=" & txtMiddle.Text _
                            & "|Last Name=" & txtLast.Text _
                            & "|Cost Center=" & cmbRC.SelectedValue _
                            & "|Agency=" & cmbAgency.SelectedValue _
                            & "|Division=" & cmbDiv.SelectedValue _
                            & "|Department=" & cmbDept.SelectedValue _
                            & "|Section=" & cmbSection.SelectedValue _
                            & "|Unit=" & cmbUnit.SelectedValue

                        EventLog(Session("uid"), Request.ServerVariables("REMOTE_ADDR"), "EDIT", Session("oldval"), Session("newval"), "Employee ID: " & txtId.Text _
                            & " was added/edited", "201 Profile", c)
                        Session.Remove("oldval")
                        Session.Remove("newval")

                    Catch ex As SqlClient.SqlException
                        Response.Write("Error in SQL: 1" & ex.Message)
                    End Try
                    vScript = "alert('Successfully saved.'); " _
                        & "opener.document.getElementById('txtRefreshList').value='refresh'; " _
                        & "opener.document.form1.submit();" 'window.close();"

                End If
                'Server.Transfer("emp_split_rc.aspx?vEmp=" & txtId.Text.Trim)
            End If




        End Sub

        Private Sub vldEmpId_ServerValidate(ByVal source As System.Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles vldEmpId.ServerValidate
            If Session("empid") <> txtId.Text Then
                Dim cm As New sqlclient.sqlCommand
                Dim rs As sqlclient.sqlDataReader

                c.ConnectionString = connStr
                c.Open()
                cm.Connection = c
                cm.CommandText = "select Emp_Cd from emp_master where Emp_Cd='" & txtId.Text & "'"
                Try
                    rs = cm.ExecuteReader
                    rs.Read()
                    args.IsValid = Not rs.HasRows
                    cm.Dispose()
                    c.Close()
                    Exit Sub
                Catch ex As sqlclient.sqlException
                    Response.Write("Error executing SQL during employee id validattion.<Br>" & ex.Message)
                End Try
                cm.Dispose()
                c.Close()
                args.IsValid = False
            Else
                args.IsValid = True
            End If
        End Sub

        'Protected Sub vldEmpBio_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles vldEmpBio.ServerValidate
        '    If Session("empid") <> txtId.Text Then
        '        Dim cm As New SqlClient.SqlCommand
        '        Dim rs As SqlClient.SqlDataReader

        '        c.ConnectionString = connStr
        '        c.Open()
        '        cm.Connection = c
        '        cm.CommandText = "select Emp_Cd from py_emp_master where BarcodeId='" & txtBarcode.Text & "'"
        '        Try
        '            rs = cm.ExecuteReader
        '            rs.Read()
        '            args.IsValid = Not rs.HasRows
        '            cm.Dispose()
        '            c.Close()
        '            Exit Sub
        '        Catch ex As SqlClient.SqlException
        '            Response.Write("Error executing SQL during employee id validattion.<Br>" & ex.Message)
        '        End Try
        '        cm.Dispose()
        '        c.Close()
        '        args.IsValid = False
        '    Else
        '        args.IsValid = True
        '    End If
        'End Sub
    End Class

End Namespace
