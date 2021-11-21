Imports denaro
Partial Class upm
    Inherits System.Web.UI.Page
    Public vScript As String = ""
    Public vBuild As String = ""
    Dim c As New sqlclient.sqlconnection
    Dim vClass As String = "odd"
    Protected Sub cmdReturn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdReturn.Click
        c.Dispose()
        Session.Remove("user_id")
        Session.Remove("mode")
        Session.Remove("menuselect")
        Server.Transfer("main.aspx")
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("uid") = "" Then
            Server.Transfer("index.aspx")
            Exit Sub
        End If
        If Not IsPostBack Then
            If CanRun(Session("caption"), Request.Item("id")) Then
                DataRefresh()
                BuildCombo("select Position, Position from user_list group by Position", cmbPosition)
                cmbPosition.Items.Add("All")
                cmbPosition.SelectedValue = "All"

                'lblcaption.Text = "User Profiles"
            Else
                Session("denied") = "1"
                Server.Transfer("main.aspx")
            End If
        End If

        DataRefresh()
    End Sub
    Private Sub DataRefresh()
        Dim da As sqlclient.sqlDataAdapter
        Dim ds As New DataSet
        Dim vFilter As String = ""

        If txtSearch.Text <> "" Then
            vFilter = cmbFilter.SelectedValue & " '" & txtSearch.Text & "%'"
        End If

        If cmbPosition.SelectedValue <> "All" Then
            vFilter = " where Position='" & cmbPosition.SelectedValue & "'"
        End If


        c.ConnectionString = connStr
        da = New sqlclient.sqlDataAdapter("select * from user_list " & vFilter, c)
        da.Fill(ds, "users")
        tblUser.DataSource = ds.Tables("users")
        tblUser.DataBind()
        da.Dispose()
        ds.Dispose()
    End Sub

    Protected Sub tblUser_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles tblUser.PageIndexChanging
        tblUser.PageIndex = e.NewPageIndex
        DataRefresh()
        CheckUserLevel()
    End Sub

    Protected Sub cmdDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdDelete.Click
        If tblUser.SelectedIndex >= 0 Then
            Dim cm As New sqlclient.sqlcommand

            EventLog(Session("uid"), Request.ServerVariables("REMOTE_ADDR"), "DELETE", "", "", "User profile of " & tblUser.SelectedRow.Cells(0).Text, "User Profiles")

            c.ConnectionString = connStr
            c.Open()
            cm.Connection = c
            cm.CommandText = "delete from user_list where User_Id='" & _
                tblUser.SelectedRow.Cells(0).Text & "'"
            cm.ExecuteNonQuery()
            cm.Dispose()
            c.Close()
            vScript = "alert('Record was successfully deleted.');"
            DataRefresh()
            tblUser.SelectedIndex = -1
        Else
            vScript = "alert('You must first select a user.');"
        End If
    End Sub

    Protected Sub cmdDelete_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdDelete.Init
        cmdDelete.Attributes.Add("onclick", "return ask();")
    End Sub

    Protected Sub cmdEdit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdEdit.Click
        If tblUser.SelectedIndex >= 0 Then
            Session("mode") = "e"
            Session("user_id") = tblUser.SelectedRow.Cells(0).Text
            Server.Transfer("modifyuser.aspx")
        Else
            vScript = "alert('You must first select a record before you can use the Edit command.');"
        End If
    End Sub

    Protected Sub cmdAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAdd.Click
        Session("mode") = "a"
        Session("user_id") = ""
        Server.Transfer("modifyuser.aspx")
    End Sub

    Protected Sub cmdPwd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdPwd.Click
        If tblUser.SelectedIndex >= 0 Then
            Session("user_id") = tblUser.SelectedRow.Cells(0).Text
            vScript = "pwdwin=window.open('assignpwd.aspx','pwdwin','location=no,toolber=no,width=450,height=175,top=200,left=200');"
        Else
            vScript = "alert('You must first select a user before changing the password.');"
        End If
    End Sub

    Protected Sub cmdSelect_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSelect.Click
        If txtSeletedTab.Value = "Menus" Then
            SetStateTree(True)
        Else
            SetState(True)
        End If

    End Sub
    Private Sub SetStateTree(ByVal pState As Boolean)
        For iCtr As Integer = 0 To triMenu.Nodes.Count - 1
            For iChild As Integer = 0 To triMenu.Nodes(iCtr).ChildNodes.Count - 1
                triMenu.Nodes(iCtr).ChildNodes.Item(iChild).Checked = pState
            Next
        Next
    End Sub
    Private Sub SetState(ByVal pState As Boolean)
        Dim iCtr As Integer

        For iCtr = 0 To chkList.Items.Count - 1
            chkList.Items(iCtr).Selected = pState
        Next iCtr
    End Sub

    Protected Sub cmdDeselect_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdDeselect.Click
        If txtSeletedTab.Value = "Menus" Then
            SetStateTree(False)
        Else
            SetState(False)
        End If
    End Sub

    Protected Sub cmdMenus_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdMenus.Click
        If tblUser.SelectedIndex >= 0 Then
            chkList.Visible = False
            triMenu.Visible = True
            txtSeletedTab.Value = "Menus"

            Dim cm As New SqlClient.SqlCommand
            Dim dr As SqlClient.SqlDataReader
            Dim vList As String = ""
            c.ConnectionString = connStr
            c.Open()
            cm.Connection = c
            cm.CommandText = "select Caption from user_list where User_Id='" & _
                tblUser.SelectedRow.Cells(0).Text & "'"
            dr = cm.ExecuteReader
            If dr.Read Then
                vList = IIf(IsDBNull(dr("Caption")), "", dr("Caption"))
            End If
            dr.Close()
            cm.Dispose()
            c.Close()
            Session("oldval") = ""

            txtRList.Value = vList
            GetGroup()

            'Session("menuselect") = "menus"

            getColorState(cmdMenus.Text)
        Else
            vScript = "alert('You must first select a user before continuing.');"
        End If
    End Sub

    Private Sub GetGroup()
        ' ==============================================================================================================================
        ' CREATED BY : LANCE BISENIO 
        ' DATE CREATED : 09/19/2012
        ' ==============================================================================================================================
        ' NOTE : I USE THIS FUCNTION FOR MENU RIGHTS ONLY. 
        '   GET ALL GROUPNAME FROM MENU  RIGHTS TABLE
        ' ==============================================================================================================================

        Dim cm As New SqlClient.SqlCommand
        Dim rs As SqlClient.SqlDataReader
        Dim t As TreeNode

        c.ConnectionString = connStr
        c.Open()
        cm.Connection = c

		'cm.CommandText = "select DISTINCT GroupName from menu_rights where GroupName is not null and SystemName='PPQ' order by GroupName"
		cm.CommandText = "select distinct(SystemName) from evolvemenus"

		rs = cm.ExecuteReader
		triMenu.Nodes.Clear()

        Do While rs.Read
            t = New TreeNode
			't.Value = rs("GroupName")
			't.Text = rs("GroupName")

			t.Value = rs("SystemName")
			t.Text = rs("SystemName")
			t.SelectAction = TreeNodeSelectAction.Expand

			GetChild(t)
			triMenu.Nodes.Add(t)
        Loop

        rs.Close()
        cm.Dispose()
        c.Close()
    End Sub
    Private Sub GetChild(ByRef t As TreeNode)
        ' ==============================================================================================================================
        ' CREATED BY : LANCE BISENIO 
        ' DATE CREATED : 09/19/2012
        ' ==============================================================================================================================
        ' NOTE : I USE THIS FUCNTION FOR MENU RIGHTS ONLY. 
        '   GET LIST OF RIGHT BASE ON THE GROUPNAME (HRIS, PAYROLL, SHARED, SYSTEM SETTINGS, TIMEKEEPING)
        ' ==============================================================================================================================

        Dim cm As New SqlClient.SqlCommand
        Dim rs As SqlClient.SqlDataReader
        Dim tChild As TreeNode

        Dim vSplitRights As String() = txtRList.Value.Split(",")

        cm.Connection = c
		'cm.CommandText = "select * from menu_rights where GroupName='" & t.Value & "' and SystemName='PPQ' order by Label_Caption"
		cm.CommandText = "select Menu_Caption, Label_Caption from evolvemenus " _
			& "where SystemName='PPQ' and Params is not null order by Label_Caption"

		rs = cm.ExecuteReader
		Do While rs.Read
            tChild = New TreeNode

			tChild.Text = rs("Menu_Caption") & "=>" & rs("Label_Caption")
			tChild.Value = rs("Menu_Caption")


			tChild.ShowCheckBox = True
            tChild.SelectAction = TreeNodeSelectAction.None

            For i = 0 To UBound(vSplitRights)
                If vSplitRights(i).ToString = rs("Menu_Caption").ToString Then
                    tChild.Checked = True
                End If
            Next i
            t.ChildNodes.Add(tChild)

        Loop
        rs.Close()
        cm.Dispose()
    End Sub

    Private Sub SetList_For_Menus()
        ' ==============================================================================================================================
        ' CREATED BY : LANCE BISENIO 
        ' DATE CREATED : 09/19/2012
        ' ==============================================================================================================================
        ' NOTE : I USE THIS FUCNTION FOR MENU RIGHTS ONLY. 
        '   STEP 1 DELETE ALL RIGHTS FROM THE RIGHTS LIST TABLE
        '   STEP 2 INSERT NEW RIGHT BASE ON THE SELECTED CHECHBOX FROM THE TREE_MENU
        '   STEP 3 UPDATE CAPTION FROM USER_LIST TABLE 
        ' ==============================================================================================================================

        Dim vMenusForU As String = "" 'YOUR NEW SELECTED RIGHTS BUILD HERE

        If triMenu.CheckedNodes.Count > 0 Then
            Dim vTnode As TreeNode
            Dim cm As New SqlClient.SqlCommand
            Dim vMenu As String = cmdMenus.Text

            c.ConnectionString = connStr
            cm.Connection = c
            Try
                c.Open()
            Catch ex As SqlClient.SqlException
                vScript = "alert('Error occurred while trying to connect to Host Database.');"
                Exit Sub
            End Try

            cm.CommandText = "delete from rights_list where User_Id='" & _
                tblUser.SelectedRow.Cells(0).Text & "' and Property='menus'"
            Try
                cm.ExecuteNonQuery()
            Catch ex As SqlClient.SqlException
                vScript = "alert('Error occurred while trying to clean-up the rights list. Error is: " & _
                    ex.Message.Replace(vbCrLf, "\n").Replace("'", "") & "');"
                c.Close()
                c.Dispose()
                cm.Dispose()
                Exit Sub
            End Try


            For Each vTnode In triMenu.CheckedNodes
                vMenusForU += vTnode.Value & ","

                cm.CommandText = "insert into rights_list (User_Id,Property,Property_Value) values ('" & _
                        tblUser.SelectedRow.Cells(0).Text & "','menus','" & _
                        vTnode.Value & "')"
                Try
                    cm.ExecuteNonQuery()
                Catch ex As SqlClient.SqlException
                    vScript = "alert('Error occurred while trying to save the rights list. Error is: " & _
                        ex.Message.Replace(vbCrLf, "\n").Replace("'", "") & "');"
                    c.Close()
                    c.Dispose()
                    cm.Dispose()
                    Exit Sub
                End Try

            Next

            cm.CommandText = "update user_list set Caption='" & vMenusForU.Substring(0, vMenusForU.Length - 1) & "' where User_Id='" & _
                tblUser.SelectedRow.Cells(0).Text & "' "
            Try
                cm.ExecuteNonQuery()
            Catch ex As SqlClient.SqlException
                vScript = "alert('Error occurred while trying to clean-up the rights list. Error is: " & _
                    ex.Message.Replace(vbCrLf, "\n").Replace("'", "") & "');"
                c.Close()
                c.Dispose()
                cm.Dispose()
                Exit Sub
            End Try

            EventLog(Session("uid"), Request.ServerVariables("REMOTE_ADDR"), "ADD/EDIT (" & vMenu & _
                    ")", vMenu & "=" & txtRList.Value, vMenu & "=" & _
                    vMenusForU.Substring(0, vMenusForU.Length - 1), "User profile access rights setup for " & _
                    tblUser.SelectedRow.Cells(0).Text, "User Profiles")

            vScript = "alert('Access rights were successfully set.');"
        Else
            vScript = "alert('No selection was made.');"
        End If
    End Sub

    Private Sub SetList(ByVal pSQL As String, ByVal pChkField As String)
        Dim cm As New sqlclient.sqlcommand
        Dim dr As sqlclient.sqldatareader
        Dim iCtr As Integer
        Dim vList() As String
        Dim iLoop As Integer

        Session("oldval") = ""

        cm.Connection = c
        cm.CommandText = pSQL

        dr = cm.ExecuteReader
        chkList.Items.Clear()
        iCtr = 0
        vList = pChkField.Split(",")
        Do While dr.Read
            chkList.Items.Add(dr(0) & "=>" & dr(1))
            For iLoop = 0 To UBound(vList)
                'Response.Write(vList(iLoop) & " : " & dr(0) & "<br>")
                If vList(iLoop) <> "" Then
                    If vList(iLoop) = dr(0) Then
                        chkList.Items(iCtr).Selected = True
                        Session("oldval") += chkList.Items(iCtr).Text.Replace("=", "-") & "<br/>"
                        Exit For
                    End If
                End If

            Next
            iCtr += 1
        Loop
        If Session("oldval") <> "" Then
            Session("oldval") = Session("oldval").ToString.Substring(0, Session("oldval").ToString.Length - 5).Replace("'", "''")
        End If
        dr.Close()
        cm.Dispose()
    End Sub

    Protected Sub cmdRC_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAccount.Click
        If tblUser.SelectedIndex >= 0 Then
            chkList.Visible = True
            triMenu.Visible = False
            txtSeletedTab.Value = "Account"

            Dim cm As New SqlClient.SqlCommand
            Dim dr As SqlClient.SqlDataReader
            Dim vList As String = ""
            Dim vAdminList As String = ""

            c.ConnectionString = connStr
            c.Open()
            cm.Connection = c
            'cm.CommandText = "select Rc_Cd from user_list where User_Id='" & _
            '    tblUser.SelectedRow.Cells(0).Text & "'"

            cm.CommandText = "select AcctCd from user_list where User_Id='" & _
                tblUser.SelectedRow.Cells(0).Text & "'"

            dr = cm.ExecuteReader
            If dr.Read Then
                vList = IIf(IsDBNull(dr("AcctCd")), "", dr("AcctCd"))
            End If
            dr.Close()

            cm.CommandText = "select AcctCd from user_list where User_Id='" & Session("uid") & "'"
            dr = cm.ExecuteReader
            If dr.Read Then
                vAdminList = IIf(IsDBNull(dr("AcctCd")), "w", dr("AcctCd"))
            End If
            dr.Close()
            cm.Dispose()

            If Session("userlevel") = "0" Then
                SetList("select AcctCd,AcctName from coa where AcctCd in ('" & vAdminList.Replace(",", "','") & "') order by AcctName", vList)
            Else
                SetList("select AcctCd,AcctName from coa order by AcctName", vList)
            End If
            c.Close()
            Session("menuselect") = "Accounts"
            getColorState(cmdAccount.Text)
        Else
            vScript = "alert('You must first select a user before continuing.');"
        End If
    End Sub

    Protected Sub cmdCategory_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdRC.Click
        If tblUser.SelectedIndex >= 0 Then
            chkList.Visible = True
            triMenu.Visible = False
            txtSeletedTab.Value = "RC"

            Dim cm As New SqlClient.SqlCommand
            Dim dr As SqlClient.SqlDataReader
            Dim vList As String = ""
            Dim vAdminList As String = ""

            c.ConnectionString = connStr
            c.Open()
            cm.Connection = c
            cm.CommandText = "select Rc_Cd from user_list where User_Id='" & _
                tblUser.SelectedRow.Cells(0).Text & "'"
            dr = cm.ExecuteReader
            If dr.Read Then
                vList = IIf(IsDBNull(dr("Rc_Cd")), "", dr("Rc_Cd"))
            End If
            dr.Close()

            cm.CommandText = "select Rc_Cd from user_list where User_Id='" & Session("uid") & "'"
            dr = cm.ExecuteReader
            If dr.Read Then
                vAdminList = IIf(IsDBNull(dr("Rc_Cd")), "", dr("Rc_Cd"))
            End If
            dr.Close()
            cm.Dispose()
            If Session("userlevel") = "0" Then
                SetList("select Rc_Cd,Descr from rc where Rc_Cd in ('" & _
                    vAdminList.Replace(",", "','") & "') order by Descr", vList)
            Else
                SetList("select Rc_Cd,Descr from rc order by Descr", vList)
            End If
            c.Close()
            Session("menuselect") = "RC"
            getColorState(cmdRC.Text)
        Else
            vScript = "alert('You must first select a user before continuing.');"
        End If
    End Sub

    'Protected Sub cmdDept_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdDept.Click
    '    If tblUser.SelectedIndex >= 0 Then
    '        chkList.Visible = True
    '        triMenu.Visible = False
    '        txtSeletedTab.Value = "Supervisor"

    '        Dim cm As New SqlClient.SqlCommand
    '        Dim dr As SqlClient.SqlDataReader
    '        Dim vList As String = ""
    '        Dim vAdminList As String = ""

    '        c.ConnectionString = connStr
    '        c.Open()
    '        cm.Connection = c
    '        cm.CommandText = "select DeptCd from user_list where User_Id='" & _
    '            tblUser.SelectedRow.Cells(0).Text & "'"
    '        dr = cm.ExecuteReader
    '        If dr.Read Then
    '            vList = IIf(IsDBNull(dr("DeptCd")), "", dr("DeptCd"))
    '        End If
    '        dr.Close()

    '        cm.CommandText = "select DeptCd from user_list where User_Id='" & Session("uid") & "'"
    '        dr = cm.ExecuteReader
    '        If dr.Read Then
    '            vAdminList = IIf(IsDBNull(dr("DeptCd")), "", dr("DeptCd"))
    '        End If
    '        dr.Close()

    '        cm.Dispose()
    '        If Session("userlevel") = "0" Then
    '            SetList("select User_Id, FullName from User_list where User_Id <> '" & tblUser.SelectedRow.Cells(0).Text & "' and User_Id in ('" & _
    '                vAdminList.Replace(",", "','") & "')", vList)
    '        Else
    '            SetList("select User_Id,FullName from User_list where User_Id <> '" & tblUser.SelectedRow.Cells(0).Text & "' ", vList)
    '        End If
    '        c.Close()
    '        Session("menuselect") = "Supervisor"
    '        getColorState(cmdDept.Text)
    '    Else
    '        vScript = "alert('You must first select a user before continuing.');"
    '    End If
    'End Sub

    'Protected Sub cmdStatus_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdStatus.Click
    '    If tblUser.SelectedIndex >= 0 Then
    '        chkList.Visible = True
    '        triMenu.Visible = False
    '        txtSeletedTab.Value = "Status"

    '        Dim cm As New SqlClient.SqlCommand
    '        Dim dr As SqlClient.SqlDataReader
    '        Dim vList As String = ""
    '        Dim vAdminList As String = ""

    '        c.ConnectionString = connStr
    '        c.Open()
    '        cm.Connection = c
    '        cm.CommandText = "select StatusCd from user_list where User_Id='" & _
    '            tblUser.SelectedRow.Cells(0).Text & "'"
    '        dr = cm.ExecuteReader
    '        If dr.Read Then
    '            vList = IIf(IsDBNull(dr("StatusCd")), "", dr("StatusCd"))
    '        End If
    '        dr.Close()

    '        cm.CommandText = "select StatusCd from user_list where User_Id='" & Session("uid") & "'"
    '        dr = cm.ExecuteReader
    '        If dr.Read Then
    '            vAdminList = IIf(IsDBNull(dr("StatusCd")), "", dr("StatusCd"))
    '        End If
    '        dr.Close()
    '        cm.Dispose()

    '        If Session("userlevel") = "0" Then
    '            SetList("select Status_Cd,Descr from dm_document_status where Status_Cd in ('" & _
    '                vAdminList.Replace(",", "','") & "') order by Descr", vList)
    '        Else
    '            SetList("select Status_Cd,Descr from dm_document_status order by Descr", vList)
    '        End If

    '        c.Close()
    '        Session("menuselect") = "Status"
    '        getColorState(cmdStatus.Text)
    '    Else
    '        vScript = "alert('You must first select a user before continuing.');"
    '    End If
    'End Sub

    Protected Sub cmdSection_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSection.Click
        If tblUser.SelectedIndex >= 0 Then
            chkList.Visible = True
            triMenu.Visible = False
            txtSeletedTab.Value = "Section"

            Dim cm As New SqlClient.SqlCommand
            Dim dr As SqlClient.SqlDataReader
            Dim vList As String = ""
            Dim vAdminList As String = ""

            c.ConnectionString = connStr
            c.Open()
            cm.Connection = c
            cm.CommandText = "select SectionCd from user_list where User_Id='" & _
                tblUser.SelectedRow.Cells(0).Text & "'"
            dr = cm.ExecuteReader
            If dr.Read Then
                vList = IIf(IsDBNull(dr("SectionCd")), "", dr("SectionCd"))
            End If
            dr.Close()

            cm.CommandText = "select SectionCd from user_list where User_Id='" & Session("uid") & "'"
            dr = cm.ExecuteReader
            If dr.Read Then
                vAdminList = IIf(IsDBNull(dr("SectionCd")), "", dr("SectionCd"))
            End If
            dr.Close()
            cm.Dispose()
            If Session("userlevel") = "0" Then
                SetList("select Section_Cd,Descr from ref_emp_section where Section_Cd in ('" & _
                    vAdminList.Replace(",", "','") & "') order by Descr", vList)
            Else
                SetList("select Section_Cd,Descr from ref_emp_section order by Descr", vList)
            End If
            c.Close()
            Session("menuselect") = "Section"
            getColorState(cmdSection.Text)
        Else
            vScript = "alert('You must first select a user before continuing.');"
        End If
    End Sub

    'Protected Sub cndUnit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cndUnit.Click
    '    If tblUser.SelectedIndex >= 0 Then
    '        chkList.Visible = True
    '        triMenu.Visible = False
    '        txtSeletedTab.Value = "Unit"

    '        Dim cm As New SqlClient.SqlCommand
    '        Dim dr As SqlClient.SqlDataReader
    '        Dim vList As String = ""
    '        Dim vAdminList As String = ""

    '        c.ConnectionString = connStr
    '        c.Open()
    '        cm.Connection = c
    '        cm.CommandText = "select UnitCd from user_list where User_Id='" & _
    '            tblUser.SelectedRow.Cells(0).Text & "'"
    '        dr = cm.ExecuteReader
    '        If dr.Read Then
    '            vList = IIf(IsDBNull(dr("UnitCd")), "", dr("UnitCd"))
    '        End If
    '        dr.Close()

    '        cm.CommandText = "select UnitCd from user_list where User_Id='" & Session("uid") & "'"
    '        dr = cm.ExecuteReader
    '        If dr.Read Then
    '            vAdminList = IIf(IsDBNull(dr("UnitCd")), "", dr("UnitCd"))
    '        End If
    '        dr.Close()
    '        cm.Dispose()
    '        If Session("userlevel") = "0" Then
    '            SetList("select Unit_Cd,Descr from hr_unit_ref where Unit_Cd in ('" & _
    '                vAdminList.Replace(",", "','") & "') order by Descr", vList)
    '        Else
    '            SetList("select Unit_Cd,Descr from hr_unit_ref order by Descr", vList)
    '        End If
    '        c.Close()
    '        Session("menuselect") = "unit"
    '        getColorState(cndUnit.Text)
    '    Else
    '        vScript = "alert('You must first select a user before continuing.');"
    '    End If
    'End Sub

    'Protected Sub cmdSecuity_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSecuity.Click
    '    If tblUser.SelectedIndex >= 0 Then
    '        chkList.Visible = True
    '        triMenu.Visible = False
    '        txtSeletedTab.Value = "Security"

    '        Dim cm As New SqlClient.SqlCommand
    '        Dim dr As SqlClient.SqlDataReader
    '        Dim vList As String = ""
    '        Dim vAdminList As String = ""

    '        c.ConnectionString = connStr
    '        c.Open()
    '        cm.Connection = c
    '        cm.CommandText = "select EmploymentType from user_list where User_Id='" & _
    '            tblUser.SelectedRow.Cells(0).Text & "'"
    '        dr = cm.ExecuteReader
    '        If dr.Read Then
    '            vList = IIf(IsDBNull(dr("EmploymentType")), "", dr("EmploymentType"))
    '        End If
    '        dr.Close()

    '        cm.CommandText = "select EmploymentType from user_list where User_Id='" & Session("uid") & "'"
    '        dr = cm.ExecuteReader
    '        If dr.Read Then
    '            vAdminList = IIf(IsDBNull(dr("EmploymentType")), "", dr("EmploymentType"))
    '        End If
    '        dr.Close()
    '        cm.Dispose()
    '        If Session("userlevel") = "0" Then
    '            SetList("select EmploymentType,Descr from hr_employment_type where EmploymentType in ('" & _
    '                vAdminList.Replace(",", "','") & "') order by Descr", vList)
    '        Else
    '            SetList("select EmploymentType,Descr from hr_employment_type order by Descr", vList)
    '        End If
    '        c.Close()
    '        Session("menuselect") = "employmenttype"
    '        getColorState(cmdSecuity.Text)
    '    Else
    '        vScript = "alert('You must first select a user before continuing.');"
    '    End If
    'End Sub

    Protected Sub cmdSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSave.Click

        If txtSeletedTab.Value = "Menus" Then
            SetList_For_Menus()
            Exit Sub
        End If

        If tblUser.SelectedIndex >= 0 Then
            Dim cm As New SqlClient.SqlCommand
            Dim vSQL As String = "update user_list set "
            Dim vList As String = ""
            Dim iCtr As Integer
            Dim vData As String

            c.ConnectionString = connStr
            cm.Connection = c
            Try
                c.Open()
            Catch ex As SqlClient.SqlException
                vScript = "alert('Error occurred while trying to connect to Host Database.');"
                Exit Sub
            End Try

            'clean records first
            cm.CommandText = "delete from rights_list where User_Id='" & _
                tblUser.SelectedRow.Cells(0).Text & "' and Property='" & Session("menuselect") & "'"
            Try
                cm.ExecuteNonQuery()
            Catch ex As SqlClient.SqlException
                vScript = "alert('Error occurred while trying to clean-up the rights list. Error is: " & _
                    ex.Message.Replace(vbCrLf, "\n").Replace("'", "") & "');"
                c.Close()
                c.Dispose()
                cm.Dispose()
                Exit Sub
            End Try

            For iCtr = 0 To chkList.Items.Count - 1
                If chkList.Items(iCtr).Selected Then
                    vData = ExtractData(chkList.Items(iCtr).Text)
                    vList += vData & ","
                    Session("newval") += chkList.Items(iCtr).Text.Replace("=", "-") & "<br/>"

                    'now insert the new rights list
                    cm.CommandText = "insert into rights_list (User_Id,Property,Property_Value) values ('" & _
                        tblUser.SelectedRow.Cells(0).Text & "','" & Session("menuselect") & "','" & _
                        vData & "')"
                    Try
                        cm.ExecuteNonQuery()
                    Catch ex As SqlClient.SqlException
                        vScript = "alert('Error occurred while trying to save the rights list. Error is: " & _
                            ex.Message.Replace(vbCrLf, "\n").Replace("'", "") & "');"
                        c.Close()
                        c.Dispose()
                        cm.Dispose()
                        Exit Sub
                    End Try
                End If
            Next iCtr

            If Session("newval") <> "" Then
                Session("newval") = Session("newval").ToString.Substring(0, Session("newval").ToString.Length - 5).Replace("'", "''")
            End If


            Dim vMenu As String = ""
            If vList <> "" Then
                vList = vList.Substring(0, vList.Length - 1)
                Select Case Session("menuselect")
                    Case "menus"
                        vSQL += "Caption='" & vList & "' "
                        vMenu = "Menus"
                    Case "Accounts"
                        vSQL += "AcctCd='" & vList & "'"
                        vMenu = "Accounts"
                    Case "RC"
                        vSQL += "Rc_Cd='" & vList & "'"
                        vMenu = "Category"
                    Case "Status"
                        vSQL += "StatusCd='" & vList & "'"
                        vMenu = "Status"
                    Case "Section"
                        vSQL += "SectionCd='" & vList & "'"
                        vMenu = "Section"
                    Case "Supervisor"
                        vSQL += "DeptCd='" & vList & "'"
                        vMenu = "Supervisor"
                End Select
                vSQL += " where User_Id='" & tblUser.SelectedRow.Cells(0).Text & "'"

                cm.CommandText = vSQL
                'Response.Write(vSQL)
                Try

                    cm.ExecuteNonQuery()
                Catch ex As SqlClient.SqlException
                    vScript = "alert('Error occurred while trying to save the user list audit. Error is: " & _
                        ex.Message.Replace(vbCrLf, "\n").Replace("'", "") & "');"
                    c.Close()
                    c.Dispose()
                    cm.Dispose()
                    Exit Sub
                End Try
                c.Close()
                c.Dispose()
                cm.Dispose()

                'Modified by Rj Bautista
                'Adding of Event Audit Log
                'March 17,2009

                'Select Case Session("menuselect")
                '    Case "menus"
                '        vMenu = cmdMenus.Text
                '    Case "Agency"
                '        vMenu = cmdRC.Text
                '    Case "Category"
                '        vMenu = cmdCategory.Text
                '    Case "Status"
                '        vMenu = cmdStatus.Text
                '    Case "Supervisor"
                '        vMenu = cmdDept.Text

                'End Select

                EventLog(Session("uid"), Request.ServerVariables("REMOTE_ADDR"), "ADD/EDIT (" & vMenu & _
                    ")", vMenu & "=" & Session("oldval"), vMenu & "=" & _
                    Session("newval"), "User profile access rights setup for " & _
                    tblUser.SelectedRow.Cells(0).Text, "User Profiles")

                'Modified by Rj Bautista
                'Adding of Event Audit Log
                'March 17,2009

                vScript = "alert('Access rights were successfully set.');"
            Else
                vScript = "alert('No selection was made.');"
            End If
        Else
            vScript = "alert('You must first select a user before continuing.');"
        End If
    End Sub

    Protected Sub tblUser_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tblUser.SelectedIndexChanged
        CheckUserLevel()
        triMenu.Visible = False
        chkList.Visible = False
    End Sub

    Private Sub CheckUserLevel()
        If tblUser.SelectedIndex <> -1 And tblUser.SelectedIndex <= tblUser.Rows.Count And _
            tblUser.SelectedIndex > tblUser.Rows.Count Then
            Dim cm As New sqlclient.sqlcommand
            Dim dr As sqlclient.sqldatareader

            c.ConnectionString = connStr
            c.Open()
            cm.Connection = c
            cm.CommandText = "select UserLevel from user_list where User_Id='" & tblUser.SelectedRow.Cells(0).Text & "'"
            dr = cm.ExecuteReader

            If dr.Read Then
                cmdEdit.Enabled = Session("userlevel") <> 0 Or Session("userlevel") = dr("UserLevel")
                cmdDelete.Enabled = cmdEdit.Enabled
                cmdPwd.Enabled = cmdEdit.Enabled
                cmdSave.Enabled = cmdEdit.Enabled
            Else
                cmdEdit.Enabled = False
                cmdDelete.Enabled = False
                cmdPwd.Enabled = False
                cmdSave.Enabled = False
            End If
            dr.Close()
            cm.Dispose()
            c.Close()
        End If
    End Sub

    Protected Sub cmdSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        DataRefresh()
    End Sub

    Protected Sub cmdPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdPrint.Click
        Dim c As New SqlClient.SqlConnection(connStr)
        Dim cm As New SqlClient.SqlCommand
        Dim cmRef As New SqlClient.SqlCommand
        Dim rs As SqlClient.SqlDataReader
        Dim rsRef As SqlClient.SqlDataReader
        Dim vDump As New StringBuilder
        Dim vMenu As New StringBuilder
        Dim vRC As New StringBuilder
        Dim vAgency As New StringBuilder
        Dim vDiv As New StringBuilder
        Dim vDept As New StringBuilder
        Dim vSection As New StringBuilder
        Dim vUnit As New StringBuilder
        Dim vRank As New StringBuilder
        Dim vRight() As String
        Dim vClass As String = "odd"
        Dim iCtr As Integer

        Try
            c.Open()
        Catch ex As SqlClient.SqlException
            vScript = "alert('An error occurred while trying to connect to database. Error is: " & _
                ex.Message.Replace(vbCrLf, "").Replace("'", "") & "');"
            c.Dispose()
            cm.Dispose()
            cmRef.Dispose()
            Exit Sub
        End Try

        cm.Connection = c
        cmRef.Connection = c


        vDump.AppendLine("<html xmlns=""http://www.w3.org/1999/xhtml"" >")
        vDump.AppendLine("<head><title>User Profile Access Rights</title>" & _
            "<link href=""../redtheme/red.css"" rel=""stylesheet"" type=""text/css"" /></head><body><center>")
        vDump.AppendLine("<h4>User Rights Access Prooflist</h4><hr/>")
        vDump.AppendLine("<table border='1' style='width:100%; border-collapse:collapse;'>")
        vDump.AppendLine("<tr class='MenuLink'><th class='labelC'>User Id</th>" & _
            "<th class='labelC'>User Name</th>" & _
            "<th class='labelC'>Position</th>" & _
            "<th class='labelC'>User Level</th>" & _
            "<th class='labelC'>Menu Rights</th>" & _
            "<th class='labelC'>Cost Center Rights</th>" & _
            "<th class='labelC'>Office/Company/Branch Rigths</th>" & _
            "<th class='labelC'>Division Rights</th>" & _
            "<th class='labelC'>Department Rights</th>" & _
            "<th class='labelC'>Section Rights</th>" & _
            "<th class='labelC'>Unit Rights</th>" & _
            "<th class='labelC'>Rank Rights</th></tr>")

        cm.CommandText = "select * from user_list order by FullName"
        Try
            rs = cm.ExecuteReader
            Do While rs.Read
                'get menu rights 
                vMenu = New StringBuilder
                If Not IsDBNull(rs("Caption")) Then
                    vRight = rs("Caption").ToString.Split(",")
                    vMenu.AppendLine("<ul class='labelL'>")
                    For iCtr = 0 To UBound(vRight)
                        cmRef.CommandText = "select Label_Caption from menu_rights where SystemName='PPQ' and Menu_Caption='" & _
                            vRight(iCtr) & "'"
                        rsRef = cmRef.ExecuteReader
                        If rsRef.Read Then
                            vMenu.AppendLine("<li>" & rsRef("Label_Caption") & "</li>")
                        End If
                        rsRef.Close()
                    Next
                    vMenu.AppendLine("</ul>")
                Else
                    vMenu.AppendLine("&nbsp;")
                End If

                'get cost centers
                vRC = New StringBuilder
                If Not IsDBNull(rs("Rc_Cd")) Then
                    vRight = rs("Rc_Cd").ToString.Split(",")
                    vRC.AppendLine("<ul class='labelL'>")
                    For iCtr = 0 To UBound(vRight)
                        cmRef.CommandText = "select Descr from rc where Rc_Cd='" & vRight(iCtr) & "'"
                        rsRef = cmRef.ExecuteReader
                        If rsRef.Read Then
                            vRC.AppendLine("<li>" & rsRef("Descr") & "</li>")
                        End If
                        rsRef.Close()
                    Next
                    vRC.AppendLine("</ul>")
                Else
                    vRC.AppendLine("&nbsp;")
                End If

                'get agencies
                vAgency = New StringBuilder
                If Not IsDBNull(rs("AgencyCd")) Then
                    vRight = rs("AgencyCd").ToString.Split(",")
                    vAgency.AppendLine("<ul class='labelL'>")
                    For iCtr = 0 To UBound(vRight)
                        cmRef.CommandText = "select AgencyName from agency where AgencyCd='" & vRight(iCtr) & "'"
                        rsRef = cmRef.ExecuteReader
                        If rsRef.Read Then
                            vAgency.AppendLine("<li>" & rsRef("AgencyName") & "</li>")
                        End If
                        rsRef.Close()
                    Next
                    vAgency.AppendLine("</ul>")
                Else
                    vAgency.AppendLine("&nbsp;")
                End If

                'get division
                vDiv = New StringBuilder
                If Not IsDBNull(rs("DivCd")) Then
                    vRight = rs("DivCd").ToString.Split(",")
                    vDiv.AppendLine("<ul class='labelL'>")
                    For iCtr = 0 To UBound(vRight)
                        cmRef.CommandText = "select Descr from hr_div_ref where Div_Cd='" & vRight(iCtr) & "'"
                        rsRef = cmRef.ExecuteReader
                        If rsRef.Read Then
                            vDiv.AppendLine("<li>" & rsRef("Descr") & "</li>")
                        End If
                        rsRef.Close()
                    Next
                    vDiv.AppendLine("</ul>")
                Else
                    vDiv.AppendLine("&nbsp;")
                End If

                'get department
                vDept = New StringBuilder
                If Not IsDBNull(rs("DeptCd")) Then
                    vRight = rs("DeptCd").ToString.Split(",")
                    vDept.AppendLine("<ul class='labelL'>")
                    For iCtr = 0 To UBound(vRight)
                        cmRef.CommandText = "select Descr from hr_dept_ref where Dept_Cd='" & vRight(iCtr) & "'"
                        rsRef = cmRef.ExecuteReader
                        If rsRef.Read Then
                            vDept.AppendLine("<li>" & rsRef("Descr") & "</li>")
                        End If
                        rsRef.Close()
                    Next
                    vDept.AppendLine("</ul>")
                Else
                    vDept.AppendLine("&nbsp;")
                End If

                'get section
                vSection = New StringBuilder
                If Not IsDBNull(rs("SectionCd")) Then
                    vRight = rs("SectionCd").ToString.Split(",")
                    vSection.AppendLine("<ul class='labelL'>")
                    For iCtr = 0 To UBound(vRight)
                        cmRef.CommandText = "select Descr from ref_emp_section where Section_Cd='" & vRight(iCtr) & "'"
                        rsRef = cmRef.ExecuteReader
                        If rsRef.Read Then
                            vSection.AppendLine("<li>" & rsRef("Descr") & "</li>")
                        End If
                        rsRef.Close()
                    Next
                    vSection.AppendLine("</ul>")
                Else
                    vSection.AppendLine("&nbsp;")
                End If

                'get unit
                vUnit = New StringBuilder
                If Not IsDBNull(rs("UnitCd")) Then
                    vRight = rs("UnitCd").ToString.Split(",")
                    vUnit.AppendLine("<ul class='labelL'>")
                    For iCtr = 0 To UBound(vRight)
                        cmRef.CommandText = "select Descr from hr_unit_ref where Unit_Cd='" & vRight(iCtr) & "'"
                        rsRef = cmRef.ExecuteReader
                        If rsRef.Read Then
                            vUnit.AppendLine("<li>" & rsRef("Descr") & "</li>")
                        End If
                        rsRef.Close()
                    Next
                    vUnit.AppendLine("</ul>")
                Else
                    vUnit.AppendLine("&nbsp;")
                End If

                'get ranks
                vRank = New StringBuilder
                If Not IsDBNull(rs("EmploymentType")) Then
                    vRight = rs("EmploymentType").ToString.Split(",")
                    vRank.AppendLine("<ul class='labelL'>")
                    For iCtr = 0 To UBound(vRight)
                        cmRef.CommandText = "select Descr from hr_employment_type where EmploymentType='" & vRight(iCtr) & "'"
                        rsRef = cmRef.ExecuteReader
                        If rsRef.Read Then
                            vRank.AppendLine("<li>" & rsRef("Descr") & "</li>")
                        End If
                        rsRef.Close()
                    Next
                    vRank.AppendLine("</ul>")
                Else
                    vRank.AppendLine("&nbsp;")
                End If

                vDump.AppendLine("<tr class='" & vClass & "'>" & _
                    "<td class='labelL' valign='top'>" & rs("User_Id") & "</td>" & _
                    "<td class='labelL' valign='top'>" & rs("Fullname") & "</td>" & _
                    "<td class='labelL' valign='top'>" & rs("Position") & "</td>" & _
                    "<td class='labelL' valign='top'>" & IIf(rs("UserLevel") = 0, "Regular User", "Super User") & "</td>" & _
                    "<td class='labelC' valign='top'>" & vMenu.ToString & "</td>" & _
                    "<td class='labelC' valign='top'>" & vRC.ToString & "</td>" & _
                    "<td class='labelC' valign='top'>" & vAgency.ToString & "</td>" & _
                    "<td class='labelC' valign='top'>" & vDiv.ToString & "</td>" & _
                    "<td class='labelC' valign='top'>" & vDept.ToString & "</td>" & _
                    "<td class='labelC' valign='top'>" & vSection.ToString & "</td>" & _
                    "<td class='labelC' valign='top'>" & vUnit.ToString & "</td>" & _
                    "<td class='labelC' valign='top'>" & vRank.ToString & "</td></tr>")
                vClass = IIf(vClass = "odd", "even", "odd")
            Loop
            rs.Close()
            Dim vFilename As String = Server.MapPath(".") & "\downloads\" & Session.SessionID & "-user_rights.html"
            If IO.File.Exists(vFilename) Then
                Try
                    IO.File.Delete(vFilename)
                Catch ex As IO.IOException
                End Try
            End If
            IO.File.WriteAllText(vFilename, vDump.ToString)
            vScript = "winprint=window.open('downloads/" & Session.SessionID & _
                "-user_rights.html','winprint','top=0,left=50,width=1024,height=600,toolbars=no,resizable=yes,scrollbars=yes'); winprint.focus();"
        Catch ex As SqlClient.SqlException
            vScript = "alert('An error occurred while trying to retrieve user info. Error is: " & _
                ex.Message.Replace(vbCrLf, "").Replace("'", "") & "');"
        Finally
            c.Close()
            c.Dispose()
            cm.Dispose()
            cmRef.Dispose()
        End Try
        vDump.AppendLine("</table></center></body></html>")
    End Sub

    Private Sub getColorState(ByRef vMenu As String)

        Select Case vMenu
            Case "Menus"
                cmdMenus.CssClass = "MenuLink_selected"
                cmdAccount.CssClass = "MenuLink"
                cmdRC.CssClass = "MenuLink" 
                cmdSection.CssClass = "MenuLink"
                'cmdStatus.CssClass = "MenuLink"
                'cmdDept.CssClass = "MenuLink"
                'cndUnit.CssClass = "MenuLink"
                'cmdSecuity.CssClass = "MenuLink"
            Case "Accounts"
                cmdAccount.CssClass = "MenuLink_selected"
                cmdMenus.CssClass = "MenuLink"
                cmdRC.CssClass = "MenuLink"
                'cmdStatus.CssClass = "MenuLink"
                'cmdDept.CssClass = "MenuLink"
                'cmdSection.CssClass = "MenuLink"
                'cndUnit.CssClass = "MenuLink"
                'cmdSecuity.CssClass = "MenuLink"
            Case "Section"
                cmdRC.CssClass = "MenuLink"
                cmdMenus.CssClass = "MenuLink"
                cmdAccount.CssClass = "MenuLink"
                cmdSection.CssClass = "MenuLink_selected" 
                'cmdStatus.CssClass = "MenuLink"
                'cmdDept.CssClass = "MenuLink"
                'cndUnit.CssClass = "MenuLink"
                'cmdSecuity.CssClass = "MenuLink"
            Case "Cost Center"
                cmdRC.CssClass = "MenuLink_selected"
                cmdMenus.CssClass = "MenuLink"
                cmdAccount.CssClass = "MenuLink"
                'cmdStatus.CssClass = "MenuLink"
                'cmdDept.CssClass = "MenuLink"
                'cmdSection.CssClass = "MenuLink"
                'cndUnit.CssClass = "MenuLink"
                'cmdSecuity.CssClass = "MenuLink"
            Case "Status"

                cmdMenus.CssClass = "MenuLink"
                cmdAccount.CssClass = "MenuLink"
                cmdRC.CssClass = "MenuLink"
                'cmdStatus.CssClass = "MenuLink_selected"
                'cmdDept.CssClass = "MenuLink"
                'cmdSection.CssClass = "MenuLink"
                'cndUnit.CssClass = "MenuLink"
                'cmdSecuity.CssClass = "MenuLink"
            Case "Supervisor"

                cmdMenus.CssClass = "MenuLink"
                cmdAccount.CssClass = "MenuLink"
                cmdRC.CssClass = "MenuLink"
                'cmdDept.CssClass = "MenuLink_selected"
                'cmdStatus.CssClass = "MenuLink"
                'cmdSection.CssClass = "MenuLink"
                'cndUnit.CssClass = "MenuLink"
                'cmdSecuity.CssClass = "MenuLink"
                'Case "Section"
                '    'cmdSection.CssClass = "MenuLink_selected"
                '    cmdMenus.CssClass = "MenuLink"
                '    cmdRC.CssClass = "MenuLink"
                '    cmdCategory.CssClass = "MenuLink"
                '    cmdStatus.CssClass = "MenuLink"
                '    cmdDept.CssClass = "MenuLink"
                '    'cndUnit.CssClass = "MenuLink"
                '    'cmdSecuity.CssClass = "MenuLink"
                'Case "Unit"
                '    'cndUnit.CssClass = "MenuLink_selected"
                '    cmdMenus.CssClass = "MenuLink"
                '    cmdRC.CssClass = "MenuLink"
                '    cmdCategory.CssClass = "MenuLink"
                '    cmdStatus.CssClass = "MenuLink"
                '    cmdDept.CssClass = "MenuLink"
                '    'cmdSection.CssClass = "MenuLink"
                '    cmdSecuity.CssClass = "MenuLink"
                'Case "Rank"
                '    'cmdSecuity.CssClass = "MenuLink_selected"
                '    cmdMenus.CssClass = "MenuLink"
                '    cmdRC.CssClass = "MenuLink"
                '    cmdCategory.CssClass = "MenuLink"
                '    cmdStatus.CssClass = "MenuLink"
                '    cmdDept.CssClass = "MenuLink"
                '    'cmdSection.CssClass = "MenuLink"
                '    'cndUnit.CssClass = "MenuLink"
        End Select
    End Sub

   
End Class
