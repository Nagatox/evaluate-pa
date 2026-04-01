Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Threading
Imports System.Web.Script.Serialization
Imports System.IO
Imports System.IO.Packaging

Imports DocumentFormat.OpenXml
Imports DocumentFormat.OpenXml.Packaging
Imports DocumentFormat.OpenXml.Spreadsheet

Public Class frmAutoGenForm
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        InitializeSystemVariables()

        Dim SQL As String
        Dim pRecordSet As New ADODB.Recordset
        Dim pConnection As ADODB.Connection = Nothing

        'Response.Write("1234")

        Try
            pConnection = ConnectDB()

            SQL = "SELECT username FROM tblUser WHERE active = '1' and group_id = 4"

            OpenTable(pConnection, pRecordSet, SQL)

            If pRecordSet.RecordCount > 0 Then
                While Not pRecordSet.EOF
                    Dim username = GetField(pRecordSet, "username")
                    Response.Write("<br>" & username)
                    Dim url = "frmEvaluateService.aspx?q=ExportEvaluateForm&username=" & username & "&evaluate_year=2561&round=1&revision=1"
                    Dim newWin As String = "window.open('" & url & "');"
                    ClientScript.RegisterStartupScript(Me.GetType(), username, newWin, True)

                    pRecordSet.MoveNext()

                End While

            Else

            End If

        Catch ex As Exception

            gErrorMessage = ex.Message
        Finally
            If pRecordSet.State > 0 Then
                pRecordSet.Close()
            End If

            CloseDB(pConnection)
        End Try
    End Sub

End Class