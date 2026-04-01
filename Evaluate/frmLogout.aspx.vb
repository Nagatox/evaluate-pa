Public Partial Class frmLogout
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Session.Abandon()

        Dim _userInfoCookies = New HttpCookie("EvaluateUserInfo")
        _userInfoCookies("UserID") = ""
        _userInfoCookies("BudgetYear") = ""

        _userInfoCookies.Expires = DateTime.Now.AddDays(-30)
        Response.Cookies.Add(_userInfoCookies)

        Response.Redirect("~/frmLogin.aspx")
    End Sub

End Class