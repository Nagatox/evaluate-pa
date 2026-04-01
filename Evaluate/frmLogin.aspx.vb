Imports System.Web
Imports System.Net
Imports System.Xml
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports Microsoft.VisualBasic
Imports System.Security.Cryptography.X509Certificates
Imports System.DirectoryServices

Friend NotInheritable Class TestingCertificatePolicy
    Implements System.Net.ICertificatePolicy

    Public Function CheckValidationResult(ByVal srvPoint As System.Net.ServicePoint, ByVal certificate As System.Security.Cryptography.X509Certificates.X509Certificate, ByVal request As System.Net.WebRequest, ByVal certificateProblem As Integer) As Boolean Implements System.Net.ICertificatePolicy.CheckValidationResult
        Return True
    End Function
End Class

Partial Public Class frmLogin
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        InitializeSystemVariables()

        System.Net.ServicePointManager.CertificatePolicy = New TestingCertificatePolicy
        ServicePointManager.Expect100Continue = True
        ServicePointManager.DefaultConnectionLimit = 9999
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3

        If Not IsPostBack Then
            'drpBudgetYear.SelectedIndex = CInt(GetProperty("DefaultBudgetYear")) - 2556
            drpBudgetYear.SelectedValue = GetProperty("DefaultRoundNo") & "/" & GetProperty("DefaultBudgetYear")
        End If

        lblVersion.Text = String.Format("Version: {0} Last Update: {1}", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString(), System.IO.File.GetLastWriteTime(System.Reflection.Assembly.GetExecutingAssembly().Location).ToShortDateString())
    End Sub

    Protected Sub btnSubmit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
        lblMessage.Text = ""

        If txtUserName.Text.Trim.Length <= 0 Then
            lblMessage.Text = "กรุณากรอก ชื่อผู้ใช้"
            txtUserName.Focus()
            Exit Sub
        End If

        'If (txtUserName.Text.Trim.ToLower.Equals("eproject") And txtPassword.Text.Trim.Equals("eproject1234")) Then

        '    Session("BudgetYear") = drpBudgetYear.Text
        '    Session("UserID") = "0"

        '    Dim _userInfoCookies = New HttpCookie("eBudgetingUserInfo")
        '    _userInfoCookies("UserID") = Session("UserID")
        '    _userInfoCookies("BudgetYear") = Session("BudgetYear")

        '    _userInfoCookies.Expires = DateTime.Now.AddDays(1)
        '    Response.Cookies.Add(_userInfoCookies)

        '    Response.Redirect("~/frmMain.aspx")
        'End If

        If CheckUser(txtUserName.Text.Trim, txtPassword.Text.Trim) Then
            Dim round_year As String() = drpBudgetYear.Text.Split("/")
            Session("BudgetYear") = round_year(1)
            Session("RoundNo") = round_year(0)

            Dim _userInfoCookies = New HttpCookie("EvaluateUserInfo")
            _userInfoCookies("UserID") = Session("UserID")
            _userInfoCookies("BudgetYear") = Session("BudgetYear")
            _userInfoCookies("RoundNo") = Session("RoundNo")

            _userInfoCookies.Expires = DateTime.Now.AddDays(1)
            Response.Cookies.Add(_userInfoCookies)

            Response.Redirect("~/frmMain.aspx")
        End If

    End Sub

    Function CheckUserInKMUTT(ByVal username As String, ByVal password As String) As Boolean

        Dim de As New DirectoryEntry("LDAP://10.1.130.12/dc=kmutt,dc=ac,dc=th", "uid=" & username & ",ou=People,ou=staff,dc=kmutt,dc=ac,dc=th", password, AuthenticationTypes.None)
        Try
            'run a search using those credentials.  
            'If it returns anything, then you're authenticated
            Dim ds As DirectorySearcher = New DirectorySearcher(de)
            ds.FindOne()
            Return True
        Catch ex As Exception
            'otherwise, it will crash out so return false
            lblMessage.Text = ex.Message & " <BR>(กรุณาตรวจสอบกับสำนักคอมพิวเตอร์)"
            Return False
        End Try

    End Function

    Function CheckUserInKMUTT2(ByVal username As String, ByVal password As String) As Boolean
        Try
            Dim authen As New account.AuthenticationService
            Dim people As New account.person

            'people.credential = True
            people = authen.getAuthentication(username, password, "staff")

            Return people.credential

        Catch ex As Exception

        End Try

        Return False

    End Function

    Function CheckUser(ByVal username As String, ByVal password As String) As Boolean

        If CheckUserInDB(username, password) Then
            Return True
        End If

        Return False

    End Function

    Function CheckUserInDB(ByVal username As String, ByVal password As String) As Boolean
        Dim SQL As String
        Dim pRecordset As New ADODB.Recordset
        Dim pConnection As ADODB.Connection = Nothing
        Dim ret As Boolean = False

        Try
            pConnection = ConnectDB()

            SQL = "SELECT U.employee_id, U.username UserID, U.password, U.Firstname, U.Lastname, G.Group_ID, G.Group_Name, U.Campus_ID, U.Faculty_ID, U.Department_ID, U.inKMUTT " & _
                  "FROM tblUser AS U INNER JOIN tblMT_Group AS G ON U.Group_ID = G.Group_ID " & _
                  "WHERE (U.Active = '1') " & _
                  "  AND (RTRIM(U.username) = '" & username.Trim() & "')"

            OpenTable(pConnection, pRecordset, SQL)

            If (pRecordset.RecordCount > 0) Then

                Debug.Print("employee_id=" & GetField(pRecordset, "employee_id"))
                Debug.Print("UserID=" & GetField(pRecordset, "UserID"))
                Debug.Print("FIRSTNAME=" & GetField(pRecordset, "FIRSTNAME"))
                Debug.Print("LASTNAME=" & GetField(pRecordset, "LASTNAME"))
                Debug.Print("GROUP_ID" & GetField(pRecordset, "GROUP_ID"))
                Debug.Print("CAMPUS_ID=" & GetField(pRecordset, "CAMPUS_ID"))
                Debug.Print("FACULTY_ID=" & GetField(pRecordset, "FACULTY_ID"))
                Debug.Print("DEPARTMENT_ID=" & GetField(pRecordset, "DEPARTMENT_ID"))
                Debug.Print("inKMUTT=" & GetField(pRecordset, "inKMUTT"))

                Dim inKmutt As String = GetField(pRecordset, "inKMUTT").Trim()

                If inKmutt.Trim() = "1" Then
                    If Not CheckUserInKMUTT(username, password) Then
                        lblMessage.Text = "Username หรือ Password ไม่ถูกต้อง (กรุณาตรวจสอบกับสำนักคอมพิวเตอร์)"
                        Return False
                    End If
                Else
                    If GetField(pRecordset, "password").Trim() <> password.Trim() Then
                        Return False
                    End If
                End If

                Session("employee_id") = GetField(pRecordset, "employee_id")
                Session("UserID") = GetField(pRecordset, "UserID")
                'Session("firstname") = GetField(pRecordset, "FIRSTNAME")
                'Session("lastname") = GetField(pRecordset, "LASTNAME")
                'Session("group_id") = GetField(pRecordset, "GROUP_ID")
                'Session("campus_id") = GetField(pRecordset, "CAMPUS_ID")
                'Session("faculty_id") = GetField(pRecordset, "FACULTY_ID")
                'Session("department_id") = GetField(pRecordset, "DEPARTMENT_ID")

                ret = True
            End If

        Catch ex As Exception
            lblMessage.Text = "ผู้ใช้ยังไม่มีสิทธ์เข้าใช้ระบบนี้ <BR>(กรุณาติดต่อเจ้าหน้าที่สำนักงานคณบดี เพื่อลงทะเบียน)"
            ret = False
        Finally
            If pRecordset.State > 0 Then
                pRecordset.Close()
            End If

            CloseDB(pConnection)
        End Try

        Return ret
    End Function

    Function CheckUserPasswordInDB(ByVal username As String, ByVal password As String) As Boolean
        Dim SQL As String
        Dim pRecordset As New ADODB.Recordset
        Dim pConnection As ADODB.Connection = Nothing
        Dim ret As Boolean = False

        Try
            pConnection = ConnectDB()

            SQL = "SELECT U.employee_id, U.username UserID, U.Firstname, U.Lastname, G.Group_ID, G.Group_Name, U.Campus_ID, U.Faculty_ID, U.Department_ID, U.inKMUTT " & _
                  "FROM tblUser AS U INNER JOIN tblMT_Group AS G ON U.Group_ID = G.Group_ID " & _
                  "WHERE (U.Active = '1') " & _
                  "  AND (RTRIM(U.username) = '" & username.Trim() & "')"

            OpenTable(pConnection, pRecordset, SQL)

            If (pRecordset.RecordCount > 0) Then

                Debug.Print("employee_id=" & GetField(pRecordset, "employee_id"))
                Debug.Print("UserID=" & GetField(pRecordset, "UserID"))
                Debug.Print("FIRSTNAME=" & GetField(pRecordset, "FIRSTNAME"))
                Debug.Print("LASTNAME=" & GetField(pRecordset, "LASTNAME"))
                Debug.Print("GROUP_ID" & GetField(pRecordset, "GROUP_ID"))
                Debug.Print("CAMPUS_ID=" & GetField(pRecordset, "CAMPUS_ID"))
                Debug.Print("FACULTY_ID=" & GetField(pRecordset, "FACULTY_ID"))
                Debug.Print("DEPARTMENT_ID=" & GetField(pRecordset, "DEPARTMENT_ID"))
                Debug.Print("inKMUTT=" & GetField(pRecordset, "inKMUTT"))

                Dim inKmutt As String = GetField(pRecordset, "inKMUTT")

                If inKmutt.Trim() <> "1" Then


                End If

                Session("employee_id") = GetField(pRecordset, "employee_id")
                Session("UserID") = GetField(pRecordset, "UserID")
                'Session("firstname") = GetField(pRecordset, "FIRSTNAME")
                'Session("lastname") = GetField(pRecordset, "LASTNAME")
                'Session("group_id") = GetField(pRecordset, "GROUP_ID")
                'Session("campus_id") = GetField(pRecordset, "CAMPUS_ID")
                'Session("faculty_id") = GetField(pRecordset, "FACULTY_ID")
                'Session("department_id") = GetField(pRecordset, "DEPARTMENT_ID")

                ret = True
            End If

        Catch ex As Exception
            ret = False
        Finally
            If pRecordset.State > 0 Then
                pRecordset.Close()
            End If

            CloseDB(pConnection)
        End Try

        Return ret
    End Function
End Class