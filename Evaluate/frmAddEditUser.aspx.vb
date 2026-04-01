Imports System.Web.Script.Serialization
Imports System.Threading

Partial Public Class frmAddEditUser
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        InitializeSystemVariables()

        If Request.Params("q") = Nothing Then
            Dim _userInfoCookies As HttpCookie = Request.Cookies("EvaluateUserInfo")

            If _userInfoCookies Is Nothing Then
                Response.Redirect("~/frmLogin.aspx")
                Return
            End If

            Session("UserID") = _userInfoCookies("UserID")
            Session("BudgetYear") = _userInfoCookies("BudgetYear")

            If Session("UserID") Is Nothing Or Session("UserID") = "" Then
                Response.Redirect("~/frmLogin.aspx")
                Exit Sub

                'Session("UserID") = "6"
                'Session("BudgetYear") = "2555"
            End If

            Dim User As _USER_ = GetUserInfo(Session("UserID"))
            lblLoginName.Text = User.username
            lblGroupName.Text = GetGroupName(User.group_id)

            If User.group_id = "3" Then
                lblOrgName.Text = GetUserFacultyName(User)
            Else
                lblOrgName.Text = GetUserDepartmentName(User)
            End If

            lblBudgetYear.Text = Session("BudgetYear")

            ' Run only once a postback has occured
            If Page.IsPostBack Then

            End If
        Else
            Dim cmd As String = Request.QueryString("q")

            Select Case cmd
                Case "GetUserList"
                    GetUserList()

                Case "UpdateUser"
                    UpdateUser()
                Case "AddUser"
                    AddUser()
                Case "DeleteUser"
                    DeleteUser()
                Case Else

            End Select

            Response.End()
        End If


    End Sub

    Protected Sub UpdateUser()
        Dim employee_id As String = Request.Params("employee_id")
        Dim user_id As String = Request.Params("user_id")
        Dim username As String = Request.Params("username")
        Dim firstname As String = Request.Params("firstname")
        Dim lastname As String = Request.Params("lastname")

        Dim group_id As String = Request.Params("group")
        Dim position As String = GetPositionName(Request.Params("position"))
        Dim level As String = GetLevelName(Request.Params("level"))

        Dim campus_id As String = Request.Params("campus")
        Dim faculty_id As String = Request.Params("faculty")
        Dim department_id As String = Request.Params("department")

        Dim active As String = Request.Params("active")
        Dim status As String = Request.Params("status")

        Dim SQL As String
        Dim pConnection As ADODB.Connection = Nothing

        Try
            pConnection = ConnectDB()

            SQL = "UPDATE tblUser SET " & _
                  " employee_id = '" & employee_id & "' " & _
                  " , firstname = '" & firstname & "' " & _
                  " , lastname = '" & lastname & "' " & _
                  " , group_id = '" & group_id & "' " & _
                  " , position = '" & position & "' " & _
                  " , level = '" & level & "' " & _
                  " , campus_id = '" & campus_id & "' " & _
                  " , faculty_id = '" & faculty_id & "' " & _
                  " , department_id = '" & department_id & "' " & _
                  " , active = '" & active & "' " & _
                  " WHERE username = '" & username & "' " & _
                  ""

            pConnection.Execute(SQL)

        Catch tex As ThreadAbortException

        Catch ex As Exception
            Response.ClearHeaders()
            Response.ClearContent()
            Response.Status = "503 Service Unavailable"
            Response.StatusCode = 503
            Response.StatusDescription = ex.Message
            Response.Flush()
            Response.End()
        Finally
            CloseDB(pConnection)

            'Dim jobjsend As JavaScriptSerializer = New JavaScriptSerializer()
            'Response.Write(jobjsend.Serialize(objJSONHeader))
            'Response.Flush()
            'Response.End()
        End Try

    End Sub

    Protected Sub AddUser()
        Dim employee_id As String = Request.Params("employee_id")
        Dim user_id As String = Request.Params("user_id")
        Dim username As String = Request.Params("username")
        Dim firstname As String = Request.Params("firstname")
        Dim lastname As String = Request.Params("lastname")

        Dim group_id As String = Request.Params("group")
        Dim position As String = GetPositionName(Request.Params("position"))
        Dim level As String = GetLevelName(Request.Params("level"))

        Dim campus_id As String = Request.Params("campus")
        Dim faculty_id As String = Request.Params("faculty")
        Dim department_id As String = Request.Params("department")

        Dim active As String = Request.Params("active")
        Dim status As String = Request.Params("status")

        Dim SQL As String
        Dim pConnection As ADODB.Connection = Nothing

        Try
            pConnection = ConnectDB()
            Dim user As _USER_ = GetUserInfo(Session("UserID"))
            SQL = "INSERT INTO tblUser " & _
                   " (employee_id, username, inKMUTT, firstname, lastname " & _
                    " , group_id, campus_id, faculty_id, department_id " & _
                    " , position, level " & _
                    " , active, record_datetime, editor " & _
                    " ) VALUES ( " & _
                     " '" + employee_id + "' " & _
                     " ,'" + username + "' " & _
                     " ,'1' " & _
                     " ,'" + firstname + "' " & _
                     " ,'" + lastname + "' " & _
                     " ,'" + group_id + "' " & _
                     " ,'" + campus_id + "' " & _
                     " ,'" + faculty_id + "' " & _
                     " ,'" + department_id + "' " & _
                     " ,'" + position + "' " & _
                     " ,'" + level + "' " & _
                     " ,'" + active + "' " & _
                     " ,'" + GetDate(DateTime.Now) + "' " & _
                     " ,'" + user.username + "' " & _
                  ") "

            pConnection.Execute(SQL)

        Catch tex As ThreadAbortException

        Catch ex As Exception
            Response.ClearHeaders()
            Response.ClearContent()
            Response.Status = "503 Service Unavailable"
            Response.StatusCode = 503
            Response.StatusDescription = ex.Message
            Response.Flush()
            Response.End()
        Finally
            CloseDB(pConnection)

            'Dim jobjsend As JavaScriptSerializer = New JavaScriptSerializer()
            'Response.Write(jobjsend.Serialize(objJSONHeader))
            'Response.Flush()
            'Response.End()
        End Try

    End Sub

    Protected Sub DeleteUser()
        Dim username As String = Request.Params("username")

        Dim SQL As String
        Dim pConnection As ADODB.Connection = Nothing
        Try
            pConnection = ConnectDB()

            SQL = "DELETE FROM tblUser WHERE username = '" & username & "' "

            pConnection.Execute(Sql)

        Catch tex As ThreadAbortException

        Catch ex As Exception
            Response.ClearHeaders()
            Response.ClearContent()
            Response.Status = "503 Service Unavailable"
            Response.StatusCode = 503
            Response.StatusDescription = ex.Message
            Response.Flush()
            Response.End()
        Finally
            CloseDB(pConnection)

            'Dim jobjsend As JavaScriptSerializer = New JavaScriptSerializer()
            'Response.Write(jobjsend.Serialize(objJSONHeader))
            'Response.Flush()
            'Response.End()
        End Try
    End Sub

    Protected Sub GetUserList()

        Dim objJSONHeader As New JSONHeader
        objJSONHeader.id = "tblUser"
        objJSONHeader.isError = False
        objJSONHeader.ErrMessage = ""

        Dim SQL As String
        Dim pRecordSet As New ADODB.Recordset
        Dim pConnection As ADODB.Connection = Nothing
        Dim iRecordCount As Integer

        Try
            pConnection = ConnectDB()

            SQL = "SELECT U.employee_id, U.username, U.inKMUTT, U.group_id " & _
                  ", U.campus_id, U.faculty_id, U.department_id " & _
                  ", U.firstname, U.lastname, U.position, U.level " & _
                  ", U.active " & _
                  ", G.group_name, C.campus_name, F.faculty_name, D.department_name " & _
                  " FROM tblUser U LEFT JOIN tblMT_Group G ON (U.group_id = G.group_id) " & _
                  "   LEFT JOIN tblMT_Campus C ON (U.campus_id = C.campus_id ) " & _
                  "   LEFT JOIN tblMT_Faculty F ON (U.campus_id = F.campus_id AND U.faculty_id = F.faculty_id ) " & _
                  "   LEFT JOIN tblMT_Department D ON (U.campus_id = D.campus_id AND U.faculty_id = D.faculty_id AND U.department_id = D.department_id ) " & _
                  " "

            iRecordCount = OpenTable(pConnection, pRecordSet, SQL)
            objJSONHeader.recordCount = iRecordCount

            ReDim objJSONHeader.detail(iRecordCount - 1)

            Dim i As Integer = 0
            While Not pRecordSet.EOF
                Dim objUser As New _USER_
                objUser.id = i + 1
                objUser.UserID = GetField(pRecordSet, "username")
                objUser.employee_id = GetField(pRecordSet, "employee_id")
                objUser.username = GetField(pRecordSet, "username")
                objUser.inKMUTT = GetField(pRecordSet, "inKMUTT")
                objUser.campus_id = GetField(pRecordSet, "campus_id")
                objUser.faculty_id = GetField(pRecordSet, "faculty_id")
                objUser.department_id = GetField(pRecordSet, "department_id")
                objUser.group_id = GetField(pRecordSet, "group_id")
                objUser.firstname = GetField(pRecordSet, "firstname")
                objUser.lastname = GetField(pRecordSet, "lastname")
                objUser.position = GetField(pRecordSet, "position")
                objUser.level = GetField(pRecordSet, "level")
                objUser.active = GetField(pRecordSet, "active")

                objUser.campus = GetField(pRecordSet, "campus_name")
                objUser.faculty = GetField(pRecordSet, "faculty_name")
                objUser.department = GetField(pRecordSet, "department_name")
                objUser.group = GetField(pRecordSet, "group_name")


                objJSONHeader.detail(i) = objUser
                pRecordSet.MoveNext()
                i = i + 1
            End While

        Catch ex As Exception
            objJSONHeader.isError = True
            objJSONHeader.ErrMessage = ex.Message

        Finally
            If pRecordSet.State > 0 Then
                pRecordSet.Close()
            End If

            CloseDB(pConnection)

            Dim jobjsend As JavaScriptSerializer = New JavaScriptSerializer()
            Response.Write(jobjsend.Serialize(objJSONHeader))
            Response.Flush()
            Response.End()
        End Try
    End Sub

End Class