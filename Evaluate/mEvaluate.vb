

Imports System
Imports System.Data
Imports System.Data.OleDb
Imports System.IO
Imports System.Web.UI.HtmlControls.HtmlInputFile
Imports Microsoft.Office.Interop



Module mEvaluate

    Public gErrorMessage As String

    Declare Function EndTask Lib "user32.dll" (ByVal hWnd As IntPtr) As Integer
    Declare Function FindWindow Lib "user32.dll" Alias "FindWindowA" _
           (ByVal lpClassName As String, ByVal lpWindowName As String) As IntPtr
    Declare Function GetWindowThreadProcessId Lib "user32.dll" _
           (ByVal hWnd As IntPtr, ByRef lpdwProcessId As Integer) As Integer
    Declare Function SetLastError Lib "kernel32.dll" (ByVal dwErrCode As Integer) As IntPtr


    Public Function VersionCheck(ByVal VersionCode As String) As Boolean

        Dim pRecordset As New ADODB.Recordset
        Dim pConnection As ADODB.Connection = Nothing
        Dim SQL As String

        Try
            pConnection = ConnectDB()

            SQL = "SELECT COUNT(*) as CNT FROM tblCFG_VersionForm WHERE Status = 1 AND CodeVersion = '" & VersionCode & "'"

            OpenTable(pConnection, pRecordset, SQL)

            If CInt(GetField(pRecordset, "CNT")) > 0 Then
                VersionCheck = True
            Else
                VersionCheck = False
            End If
        Catch ex As Exception
            gErrorMessage = ex.Message
            VersionCheck = False
        Finally
            If pRecordset.State > 0 Then
                pRecordset.Close()
            End If

            CloseDB(pConnection)
        End Try

    End Function

    Public Function GetPlanName(ByVal PlanID As String) As String
        Dim pRecordset As New ADODB.Recordset
        Dim pConnection As ADODB.Connection = Nothing
        Dim SQL As String
        Dim pReturn As String = ""

        Try
            pConnection = ConnectDB()

            SQL = "SELECT PlanName FROM tblMT_Plan WHERE PlanID = '" & PlanID & "'"

            OpenTable(pConnection, pRecordset, SQL)

            pReturn = GetField(pRecordset, "PlanName")

        Catch ex As Exception
            gErrorMessage = ex.Message
        Finally
            If pRecordset.State > 0 Then
                pRecordset.Close()
            End If

            CloseDB(pConnection)
        End Try
        GetPlanName = pReturn
    End Function

    Public Function GetLocationName(ByVal LocationID As String) As String
        Dim pRecordset As New ADODB.Recordset
        Dim pConnection As ADODB.Connection = Nothing
        Dim SQL As String
        Dim pReturn As String = ""

        Try
            pConnection = ConnectDB()

            SQL = "SELECT LocationName FROM tblMT_Location WHERE LocationID = '" & LocationID & "'"

            OpenTable(pConnection, pRecordset, SQL)

            pReturn = GetField(pRecordset, "LocationName")

        Catch ex As Exception
            gErrorMessage = ex.Message
        Finally
            If pRecordset.State > 0 Then
                pRecordset.Close()
            End If

            CloseDB(pConnection)
        End Try
        GetLocationName = pReturn
    End Function

    Public Function GetFacultyName(ByVal FacultyID As String) As String
        Dim pRecordset As New ADODB.Recordset
        Dim pConnection As ADODB.Connection = Nothing
        Dim SQL As String
        Dim pReturn As String = ""

        Try
            pConnection = ConnectDB()

            SQL = "SELECT FacultyName FROM tblMT_Faculty WHERE FacultyID = '" & FacultyID & "'"

            OpenTable(pConnection, pRecordset, SQL)

            pReturn = GetField(pRecordset, "FacultyName")

        Catch ex As Exception
            gErrorMessage = ex.Message
        Finally
            If pRecordset.State > 0 Then
                pRecordset.Close()
            End If

            CloseDB(pConnection)
        End Try
        GetFacultyName = pReturn
    End Function

    Public Function GetDepartmentName(ByVal DepartmentID As String) As String
        Dim pRecordset As New ADODB.Recordset
        Dim pConnection As ADODB.Connection = Nothing
        Dim SQL As String
        Dim pReturn As String = ""

        Try
            pConnection = ConnectDB()

            SQL = "SELECT Department_Name FROM tblMT_Department WHERE Department_ID = '" & DepartmentID & "'"

            OpenTable(pConnection, pRecordset, SQL)

            pReturn = GetField(pRecordset, "Department_Name")

        Catch ex As Exception
            gErrorMessage = ex.Message
        Finally
            If pRecordset.State > 0 Then
                pRecordset.Close()
            End If

            CloseDB(pConnection)
        End Try
        GetDepartmentName = pReturn
    End Function

    Public Function GetLocationCode(ByVal LocationID As String) As String
        Dim pRecordset As New ADODB.Recordset
        Dim pConnection As ADODB.Connection = Nothing
        Dim SQL As String
        Dim pReturn As String = ""

        Try
            pConnection = ConnectDB()

            SQL = "SELECT LocationCode FROM tblMT_Location WHERE LocationID = '" & LocationID & "'"

            OpenTable(pConnection, pRecordset, SQL)

            pReturn = GetField(pRecordset, "LocationCode")

        Catch ex As Exception
            gErrorMessage = ex.Message
        Finally
            If pRecordset.State > 0 Then
                pRecordset.Close()
            End If

            CloseDB(pConnection)
        End Try
        GetLocationCode = pReturn
    End Function

    Public Function GetFacultyCode(ByVal FacultyID As String) As String
        Dim pRecordset As New ADODB.Recordset
        Dim pConnection As ADODB.Connection = Nothing
        Dim SQL As String
        Dim pReturn As String = ""

        Try
            pConnection = ConnectDB()

            SQL = "SELECT FacultyCode FROM tblMT_Faculty WHERE FacultyID = '" & FacultyID & "'"

            OpenTable(pConnection, pRecordset, SQL)

            pReturn = GetField(pRecordset, "FacultyCode")

        Catch ex As Exception
            gErrorMessage = ex.Message
        Finally
            If pRecordset.State > 0 Then
                pRecordset.Close()
            End If

            CloseDB(pConnection)
        End Try
        GetFacultyCode = pReturn
    End Function

    Public Function GetDepartmentCode(ByVal DepartmentID As String) As String
        Dim pRecordset As New ADODB.Recordset
        Dim pConnection As ADODB.Connection = Nothing
        Dim SQL As String
        Dim pReturn As String = ""

        Try
            pConnection = ConnectDB()

            SQL = "SELECT DepartmentCode FROM tblMT_Department WHERE DepartmentID = '" & DepartmentID & "'"

            OpenTable(pConnection, pRecordset, SQL)

            pReturn = GetField(pRecordset, "DepartmentCode")

        Catch ex As Exception
            gErrorMessage = ex.Message
        Finally
            If pRecordset.State > 0 Then
                pRecordset.Close()
            End If

            CloseDB(pConnection)
        End Try
        GetDepartmentCode = pReturn
    End Function

    Public Function GetLocationID(ByVal LocationCode As String) As String
        Dim pRecordset As New ADODB.Recordset
        Dim pConnection As ADODB.Connection = Nothing
        Dim SQL As String
        Dim pReturn As String = ""

        Try
            pConnection = ConnectDB()

            SQL = "SELECT L.LocationID " & _
                  " FROM tblMT_Location L " & _
                  " WHERE L.LocationCode = '" & LocationCode & "' "

            OpenTable(pConnection, pRecordset, SQL)

            pReturn = GetField(pRecordset, "LocationID")

        Catch ex As Exception
            gErrorMessage = ex.Message
        Finally
            If pRecordset.State > 0 Then
                pRecordset.Close()
            End If

            CloseDB(pConnection)
        End Try
        GetLocationID = pReturn
    End Function

    Public Function GetFacultyID(ByVal LocationCode As String, ByVal FacultyCode As String) As String
        Dim pRecordset As New ADODB.Recordset
        Dim pConnection As ADODB.Connection = Nothing
        Dim SQL As String
        Dim pReturn As String = ""

        Try
            pConnection = ConnectDB()

            SQL = "SELECT F.FacultyID " & _
                  " FROM tblMT_Location L, tblMT_Faculty F " & _
                  " WHERE L.LocationID = F.LocationID " & _
                  " AND L.LocationCode = '" & LocationCode & "' " & _
                  " AND F.FacultyCode = '" & FacultyCode & "' "

            OpenTable(pConnection, pRecordset, SQL)

            pReturn = GetField(pRecordset, "FacultyID")

        Catch ex As Exception
            gErrorMessage = ex.Message
        Finally
            If pRecordset.State > 0 Then
                pRecordset.Close()
            End If

            CloseDB(pConnection)
        End Try
        GetFacultyID = pReturn
    End Function

    Public Function GetDepartmentID(ByVal LocationCode As String, ByVal FacultyCode As String, ByVal DepartmentCode As String) As String
        Dim pRecordset As New ADODB.Recordset
        Dim pConnection As ADODB.Connection = Nothing
        Dim SQL As String
        Dim pReturn As String = ""

        Try
            pConnection = ConnectDB()

            SQL = "SELECT D.DepartmentID " & _
                  " FROM tblMT_Location L, tblMT_Faculty F, tblMT_Department D " & _
                  " WHERE L.LocationID = F.LocationID " & _
                  " AND D.FacultyID = F.FacultyID " & _
                  " AND L.LocationCode = '" & LocationCode & "' " & _
                  " AND F.FacultyCode = '" & FacultyCode & "' " & _
                  " AND D.DepartmentCode = '" & DepartmentCode & "'"

            OpenTable(pConnection, pRecordset, SQL)

            pReturn = GetField(pRecordset, "DepartmentID")

        Catch ex As Exception
            gErrorMessage = ex.Message
        Finally
            If pRecordset.State > 0 Then
                pRecordset.Close()
            End If

            CloseDB(pConnection)
        End Try
        GetDepartmentID = pReturn
    End Function


    'Public Function CheckLoginName(ByVal sLoginName As String) As Integer
    '    Dim vQuery As String
    '    Dim pRecordSet As New ADODB.Recordset
    '    Dim vReturn As Integer = -1

    '    vQuery = "SELECT Count(*) as CNT FROM tblUser where Upper(LoginName) = Upper('" & sLoginName & "') "
    '    If (OpenTable(pRecordSet, vQuery) > 0) Then
    '        vReturn = CInt(GetField(pRecordSet, "CNT"))
    '    End If

    '    CheckLoginName = vReturn
    'End Function



    'Public Function GetPrivilege(ByVal UserID As String, ByVal ProgramID As String, ByVal ControlType As String) As String
    '    Dim vQuery As String
    '    Dim pRecordSet As New ADODB.Recordset
    '    Dim vReturn As String = "0"

    '    vQuery = "SELECT P.ProgramID, URP.Privilege " & _
    '             " FROM tbluser U, tblUserRolePrivilege URP, tblUserRole UR, tblProgram P " & _
    '             " WHERE U.UserRole = UR.UserRole " & _
    '             " AND UR.UserRole = URP.UserRole " & _
    '             " AND P.ProgramID = URP.ProgramID " & _
    '             " AND P.ControlType = '" & ControlType & "' " & _
    '             " AND U.LoginName = '" & UserID & "' " & _
    '             " AND P.ProgramID = '" & ProgramID & "' "

    '    If (OpenTable(pRecordSet, vQuery) > 0) Then
    '        vReturn = GetField(pRecordSet, "Privilege")

    '        pRecordSet.Close()
    '    End If

    '    Return vReturn
    'End Function

    'Public Function GetFullName(ByVal LoginName As String) As String
    '    Dim SQL As String
    '    Dim pRecordset As New ADODB.Recordset
    '    Dim vReturn As String = ""

    '    SQL = "SELECT U.LoginName, U.Password, U.FullName, U.UserRole, U.OrganizationID, O.OrganizationTitle, O.OrderNo, U.Status " & _
    '          " FROM tblUser U, tblOrganization O " & _
    '          " WHERE U.OrganizationID = O.OrganizationID " & _
    '          "  AND U.LoginName = '" & LoginName & "' "

    '    If OpenTable(pRecordset, SQL) > 0 Then
    '        vReturn = GetField(pRecordset, "FullName")
    '    End If
    '    pRecordset.Close()

    '    GetFullName = vReturn
    'End Function

    'Public Function GetNextProcessNo(ByVal ProcessNo As String) As String
    '    Dim SQL As String
    '    Dim pRecordset As New ADODB.Recordset
    '    Dim vReturn As String = ""

    '    SQL = "SELECT O.NextProcessNo " & _
    '          " FROM tblOrganization O " & _
    '          " WHERE O.OrganizationID = '" & ProcessNo & "' "

    '    If OpenTable(pRecordset, SQL) > 0 Then
    '        vReturn = GetField(pRecordset, "NextProcessNo")
    '    End If
    '    pRecordset.Close()

    '    GetNextProcessNo = vReturn
    'End Function




End Module