Public Partial Class frmMain
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        InitializeSystemVariables()

        Dim _userInfoCookies As HttpCookie = Request.Cookies("EvaluateUserInfo")

        If _userInfoCookies Is Nothing Then
            Response.Redirect("~/frmLogin.aspx")
            Return
        End If

        Session("UserID") = _userInfoCookies("UserID")
        Session("BudgetYear") = _userInfoCookies("BudgetYear")
        Session("RoundNo") = _userInfoCookies("RoundNo")

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

        lblRoundNo.Text = Session("RoundNo")
        txtRoundNo.Text = Session("RoundNo") 'GetProperty("DefaultRoundNo")
        txtCampusID.Text = User.campus_id
        txtFacultyID.Text = User.faculty_id
        txtDepartmentID.Text = User.department_id
    End Sub

End Class