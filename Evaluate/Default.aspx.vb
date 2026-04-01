Partial Public Class _Default
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Request.Params("q") = Nothing Then

            ' Run only once a postback has occured
            If Page.IsPostBack Then

            End If
        Else
            Dim cmd As String = Request.QueryString("q")

            Select Case cmd
                Case "dummy"

                Case Else

            End Select

            Response.End()
        End If
    End Sub

End Class