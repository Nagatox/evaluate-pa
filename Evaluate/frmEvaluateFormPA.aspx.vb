Partial Public Class frmEvaluateFormPA
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        InitializeSystemVariables()

        If ((Not Request.QueryString("UserID") Is Nothing) And (Not Request.QueryString("BudgetYear") Is Nothing) And (Not Request.QueryString("RoundNo") Is Nothing)) Then
            Session("UserID") = Request.QueryString("UserID").ToString
            Session("BudgetYear") = Request.QueryString("BudgetYear").ToString
            Session("RoundNo") = Request.QueryString("RoundNo").ToString
        Else
            Dim _userInfoCookies As HttpCookie = Request.Cookies("EvaluateUserInfo")

            If _userInfoCookies Is Nothing Then
                Response.Redirect("~/frmLogin.aspx")
                Return
            End If

            Session("UserID") = _userInfoCookies("UserID")
            Session("BudgetYear") = _userInfoCookies("BudgetYear")
            Session("RoundNo") = _userInfoCookies("RoundNo")
        End If
        'Session("UserID") = "denjai.pho"

        If Session("UserID") Is Nothing Or Session("UserID") = "" Then
            Response.Redirect("~/frmLogin.aspx")
            Exit Sub

            'Session("UserID") = "99"
            'Session("BudgetYear") = "2557"
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

        If Request.QueryString("result") Is Nothing Then
            lblFormType.Text = " (ประเมินตนเอง)"
            txtResult.Text = ""
        Else
            lblFormType.Text = " (ผลการประเมินจากผู้บังคับบัญชา)"
            txtResult.Text = Request.QueryString("result")
        End If

        If Request.QueryString("u") Is Nothing Then
            txtUserName.Text = User.username
        Else
            txtUserName.Text = Request.QueryString("u").Trim
        End If

        Dim UserEvaluate As _USER_ = GetUserInfo(txtUserName.Text)
        lblNameUser.Text = UserEvaluate.firstname & " " & UserEvaluate.lastname

        lblEmployeeID.Text = FormatEmployeeID(UserEvaluate.employee_id)
        lblFullName.Text = UserEvaluate.firstname & " " & UserEvaluate.lastname
        lblLevelNo.Text = UserEvaluate.level
        lblPositionName.Text = UserEvaluate.position
        lblDepartmentName.Text = lblOrgName.Text
        'drpRoundNo.Text = Session("RoundNo")
        lblRoundNo.Text = Session("RoundNo")
        'lblRoundNo2.Text = Session("RoundNo")

        txtRoundNo.Text = Session("RoundNo") 'GetProperty("DefaultRoundNo")
        txtCampusID.Text = UserEvaluate.campus_id
        txtFacultyID.Text = UserEvaluate.faculty_id
        txtDepartmentID.Text = UserEvaluate.department_id        

        If Not IsPostBack Then
            'drpBudgetYear.SelectedIndex = CInt(GetProperty("DefaultBudgetYear")) - 2556
            drpBudgetYear.SelectedValue = GetProperty("DefaultBackRoundNo") & "/" & GetProperty("DefaultBackBudgetYear")
        End If
    End Sub

    Public Function FormatEmployeeID(ByVal employee_id As String) As String
        Dim ret As String = ""
        Try
            Dim emp_id As String = employee_id.Trim

            If emp_id.Length = 7 Then
                ret = Mid(emp_id, 5, 3) & "/" & Mid(emp_id, 1, 4)
            End If


        Catch ex As Exception

        End Try
        Return ret
    End Function

End Class