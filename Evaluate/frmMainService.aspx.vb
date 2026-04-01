Imports System.Threading
Imports System.Web.Script.Serialization
Imports System.Drawing
Imports System.IO
Partial Public Class frmMainService
    Inherits System.Web.UI.Page



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim cmd As String = Request.QueryString("q")

        Select Case cmd
            Case "GetGroupList2DRP"
                GetGroupList2DRP()

            Case "GetPositionList2DRP"
                GetPositionList2DRP()

            Case "GetLevelList2DRP"
                GetLevelList2DRP()

            Case "GetCampusList2DRP"
                GetCampusList2DRP()

            Case "GetFacultyList2DRP"
                Dim campus_id As String = Request.QueryString("campus_id")
                GetFacultyList2DRP(campus_id)

            Case "GetDepartmentList2DRP"
                Dim campus_id As String = Request.QueryString("campus_id")
                Dim faculty_id As String = Request.QueryString("faculty_id")
                GetDepartmentList2DRP(campus_id, faculty_id)

            Case "SystemCallPhantomJS"
                Dim page As String = Request.QueryString("page")
                Dim user_id As String = Request.QueryString("user_id")
                Dim budget_year As String = Request.QueryString("budget_year")
                Dim round_no As String = Request.QueryString("round_no")
                Dim result As String = Request.QueryString("result")
                Dim u As String = Request.QueryString("u")

                SystemCallPhantomJS(page, user_id, budget_year, round_no, result, u)

            Case Else


        End Select

        Response.End()
    End Sub

    Protected Sub SystemCallPhantomJS(ByVal page, ByVal user_id, ByVal budget_year, ByVal round_no, ByVal result, ByVal u)
        Dim objJSONHeader As New JSONHeader
        objJSONHeader.id = "SystemCallPhantomJS"
        objJSONHeader.isError = False
        objJSONHeader.ErrMessage = ""

        Try
            Dim strPathAndQuery = HttpContext.Current.Request.Url.PathAndQuery
            Dim strUrl = HttpContext.Current.Request.Url.AbsoluteUri.Replace(strPathAndQuery, "") & HttpContext.Current.Request.ApplicationPath
            strUrl = strUrl.TrimEnd(CChar("/"))
            strUrl = "https://evaluate-eng.kmutt.ac.th"
            Dim url = strUrl & "/frm" & page & ".aspx?UserID=" & user_id & "&BudgetYear=" & budget_year & "&RoundNo=" & round_no & "&result=" & result & "&u=" & u
            Dim fileout = user_id & "_" & budget_year & "_" & round_no & "_" & result & "_" & page

            Dim filepath As String = "C:\Work\Evaluate\PhantomJS\SaveImage\" & fileout & ".png"

            'If File.Exists(filepath) Then File.Delete(filepath)

            Dim process As New System.Diagnostics.Process()
            Dim startInfo As New System.Diagnostics.ProcessStartInfo()
            startInfo.RedirectStandardOutput = True
            startInfo.UseShellExecute = False
            'startInfo.CreateNoWindow = True
            startInfo.WorkingDirectory = "C:\Work\Evaluate\PhantomJS\"
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden
            startInfo.FileName = "C:\phantomjs-2.0.0-windows\bin\phantomjs.exe"
            'startInfo.Verb = "runAs"
            startInfo.Arguments = "screencapture.js " & url & " " & "C:\Work\Evaluate\PhantomJS\SaveImage\" & fileout
            process.StartInfo = startInfo
            process.Start()
            process.WaitForExit()
            'Response.Write(process.StandardOutput.ReadToEnd())
            objJSONHeader.recordCount = 1
            ReDim objJSONHeader.detail(1)
            objJSONHeader.detail(0) = strUrl & "/frmResponseFile.aspx?img=" & fileout

        Catch tex As ThreadAbortException

        Catch ex As Exception
            objJSONHeader.isError = True
            objJSONHeader.ErrMessage = ex.Message
        Finally

            Dim jobjsend As JavaScriptSerializer = New JavaScriptSerializer()
            Response.Write(jobjsend.Serialize(objJSONHeader))
            Response.Flush()
            Response.End()
        End Try
    End Sub

    Protected Sub GetGroupList2DRP()
        Dim SQL As String

        Try
            SQL = "SELECT group_id, group_name  " & _
                  " FROM tblMT_Group " & _
                  " WHERE active = '1' " & _
                  " ORDER BY group_id " & _
                  " "

            Response.Write(GetOptionList(SQL, "group_id", "group_name"))

        Catch ex As Exception
            Response.Write(gErrorMessage & "|" & ex.Message)
        Finally
            Response.Flush()
            Response.End()
        End Try
    End Sub

    Protected Sub GetPositionList2DRP()
        Dim SQL As String

        Try
            SQL = "SELECT position_id, position_name  " & _
                  " FROM tblMT_Position " & _
                  " ORDER BY position_id " & _
                  " "

            Response.Write(GetOptionList(SQL, "position_id", "position_name"))

        Catch ex As Exception
            Response.Write(gErrorMessage & "|" & ex.Message)
        Finally
            Response.Flush()
            Response.End()
        End Try
    End Sub

    Protected Sub GetLevelList2DRP()
        Dim SQL As String

        Try
            SQL = "SELECT level_id, level_name  " & _
                  " FROM tblMT_Level " & _
                  " ORDER BY level_id " & _
                  " "

            Response.Write(GetOptionList(SQL, "level_id", "level_name"))

        Catch ex As Exception
            Response.Write(gErrorMessage & "|" & ex.Message)
        Finally
            Response.Flush()
            Response.End()
        End Try
    End Sub

    Protected Sub GetCampusList2DRP(Optional ByVal CampusID As String = "", Optional ByVal bZero As Boolean = False)

        Dim SQL As String

        Try
            SQL = "SELECT C.campus_id, C.campus_name " & _
                  " FROM tblMT_Campus C " & _
                  " WHERE C.active = '1' " & _
                  " "

            If CampusID.Trim.Length > 0 Then
                SQL &= " AND campus_id = '" & CampusID & "' "
            End If

            SQL &= " ORDER BY campus_id "

            If (bZero) Then
                Response.Write(GetOptionListAddZero(SQL, "campus_id", "campus_name", "ทุกสถานที่"))
            Else
                Response.Write(GetOptionList(SQL, "campus_id", "campus_name"))
            End If


        Catch ex As Exception
            Response.Write(gErrorMessage & "|" & ex.Message)
        Finally
            Response.Flush()
            Response.End()
        End Try
    End Sub

    Protected Sub GetFacultyList2DRP(ByVal CampusID As String, Optional ByVal FacultyID As String = "", Optional ByVal bZero As Boolean = False)
        Dim SQL As String

        Try
            SQL = "SELECT F.faculty_id, F.faculty_name  " & _
                  " FROM tblMT_Faculty F " & _
                  " WHERE F.campus_id = '" & CampusID & "' " & _
                  "   AND F.active = '1' " & _
                  " "

            If FacultyID.Trim.Length > 0 Then
                SQL &= " AND faculty_id = '" & FacultyID & "' "
            End If

            SQL &= " ORDER BY faculty_id "

            If (bZero) Then
                Response.Write(GetOptionListAddZero(SQL, "faculty_id", "faculty_name", "ทุกคณะ/สำนัก"))
            Else
                Response.Write(GetOptionList(SQL, "faculty_id", "faculty_name"))
            End If


        Catch ex As Exception
            Response.Write(gErrorMessage & "|" & ex.Message)
        Finally
            Response.Flush()
            Response.End()
        End Try
    End Sub

    Protected Sub GetDepartmentList2DRP(ByVal CampusID As String, ByVal FacultyID As String, Optional ByVal DepartmentID As String = "", Optional ByVal bZero As Boolean = False)
        Dim SQL As String

        Try
            SQL = "SELECT D.department_id, D.department_name " & _
                  " FROM tblMT_Department D " & _
                  " WHERE D.campus_id = '" & CampusID & "' " & _
                  "   AND D.faculty_id = '" & FacultyID & "' " & _
                  "   AND D.active = '1' " & _
                  " "

            If DepartmentID.Trim.Length > 0 Then
                SQL &= " AND department_id = '" & DepartmentID & "' "
            End If

            SQL &= " ORDER BY department_id "

            If (bZero) Then
                Response.Write(GetOptionListAddZero(SQL, "department_id", "department_name", "ทุกภาควิชา/หน่วยงาน"))
            Else
                Response.Write(GetOptionList(SQL, "department_id", "department_name"))
            End If


        Catch ex As Exception
            Response.Write(gErrorMessage & "|" & ex.Message)
        Finally
            Response.Flush()
            Response.End()
        End Try
    End Sub

    Protected Sub GetYearList2DRP()
        Dim SQL As String

        Try
            SQL = "SELECT syear  " & _
                  " FROM tblCFG_Year " & _
                  " ORDER BY syear " & _
                  " "

            Response.Write(GetOptionSelectedList(SQL, "syear", "syear", GetProperty("DefaultBudgetYear")))

        Catch ex As Exception
            Response.Write(gErrorMessage & "|" & ex.Message)
        Finally
            Response.Flush()
            Response.End()
        End Try
    End Sub

    Protected Function GetOptionSelectedList(ByVal SQL As String, ByVal FieldValue As String, ByVal FieldName As String, ByVal SelectedValue As String)
        Dim pRecordset As New ADODB.Recordset
        Dim pConnection As ADODB.Connection = Nothing

        Dim sendToDropDown As String = ""

        Try
            pConnection = ConnectDB()

            If OpenTable(pConnection, pRecordset, SQL) > 0 Then
                While Not pRecordset.EOF

                    Dim value As String = GetField(pRecordset, FieldValue)
                    If value = SelectedValue Then
                        sendToDropDown = sendToDropDown & "<option value='" & GetField(pRecordset, FieldValue) & "' selected >" & GetField(pRecordset, FieldName) & "</option>"
                    Else
                        sendToDropDown = sendToDropDown & "<option value='" & GetField(pRecordset, FieldValue) & "'>" & GetField(pRecordset, FieldName) & "</option>"
                    End If


                    pRecordset.MoveNext()
                End While
            End If

            sendToDropDown = sendToDropDown
        Catch ex As Exception
            gErrorMessage = ex.Message
        Finally
            If pRecordset.State > 0 Then
                pRecordset.Close()
            End If

            CloseDB(pConnection)

        End Try
        GetOptionSelectedList = sendToDropDown
    End Function

    Protected Function GetOptionList(ByVal SQL As String, ByVal FieldValue As String, ByVal FieldName As String)
        Dim pRecordset As New ADODB.Recordset
        Dim pConnection As ADODB.Connection = Nothing

        Dim sendToDropDown As String = ""

        Try
            pConnection = ConnectDB()

            If OpenTable(pConnection, pRecordset, SQL) > 0 Then
                While Not pRecordset.EOF

                    sendToDropDown = sendToDropDown & "<option value='" & GetField(pRecordset, FieldValue) & "'>" & GetField(pRecordset, FieldName) & "</option>"

                    pRecordset.MoveNext()
                End While
            End If

            sendToDropDown = sendToDropDown
        Catch ex As Exception
            gErrorMessage = ex.Message
        Finally
            If pRecordset.State > 0 Then
                pRecordset.Close()
            End If

            CloseDB(pConnection)

        End Try
        GetOptionList = sendToDropDown
    End Function

    Protected Function GetOptionListAddZero(ByVal SQL As String, ByVal FieldValue As String, ByVal FieldName As String, ByVal Message As String)
        Dim pRecordset As New ADODB.Recordset
        Dim pConnection As ADODB.Connection = Nothing

        Dim sendToDropDown As String = ""

        Try
            sendToDropDown = sendToDropDown & "<option value='" & "0" & "'>" & Message & "</option>"

            pConnection = ConnectDB()

            If OpenTable(pConnection, pRecordset, SQL) > 0 Then
                While Not pRecordset.EOF

                    sendToDropDown = sendToDropDown & "<option value='" & GetField(pRecordset, FieldValue) & "'>" & GetField(pRecordset, FieldName) & "</option>"

                    pRecordset.MoveNext()
                End While
            End If

            sendToDropDown = sendToDropDown
        Catch ex As Exception
            gErrorMessage = ex.Message
        Finally
            If pRecordset.State > 0 Then
                pRecordset.Close()
            End If

            CloseDB(pConnection)

        End Try
        GetOptionListAddZero = sendToDropDown
    End Function

End Class

Public Class JSONHeader
    Public id As String
    Public recordCount As Integer
    Public detail() As Object
    Public isError As Boolean
    Public ErrMessage As String
    Public RetMessage As String
End Class

Public Class URL
    Public id As String
    Public url As String
End Class
