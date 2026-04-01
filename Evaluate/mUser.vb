Imports System
Imports System.Data
Imports System.Data.OleDb
Imports System.IO

Public Class _USER_
    Public id As String
    Public UserID As String
    Public employee_id As String
    Public username As String
    Public inKMUTT As String
    Public campus_id As String
    Public faculty_id As String
    Public department_id As String
    Public group_id As String

    Public firstname As String
    Public lastname As String
    Public position As String
    Public level As String
    Public level_manager As String
    Public active As String

    Public campus As String
    Public faculty As String
    Public department As String
    Public group As String
End Class

Module mUser

    Public Function GetUserInfo(ByVal UserID As String) As _USER_
        Dim SQL As String
        Dim pRecordset As New ADODB.Recordset
        Dim pConnection As ADODB.Connection = Nothing
        Dim vReturn As New _USER_

        Try
            pConnection = ConnectDB()

            SQL = "SELECT U.employee_id, U.username, U.inKMUTT, U.group_id " & _
                  ", U.campus_id, U.faculty_id, U.department_id " & _
                  ", U.firstname, U.lastname, U.position, U.level, U.level_manager " & _
                  ", U.active " & _
                  ", G.group_name, C.campus_name, F.faculty_name, D.department_name " & _
                  " FROM tblUser U LEFT JOIN tblMT_Group G ON (U.group_id = G.group_id) " & _
                  "   LEFT JOIN tblMT_Campus C ON (U.campus_id = C.campus_id ) " & _
                  "   LEFT JOIN tblMT_Faculty F ON (U.campus_id = F.campus_id AND U.faculty_id = F.faculty_id ) " & _
                  "   LEFT JOIN tblMT_Department D ON (U.campus_id = D.campus_id AND U.faculty_id = D.faculty_id AND U.department_id = D.department_id ) " & _
                  " WHERE U.username = '" & UserID.Trim & "' "

            If OpenTable(pConnection, pRecordset, SQL) > 0 Then
                vReturn.employee_id = GetField(pRecordset, "employee_id")
                vReturn.username = GetField(pRecordset, "username")
                vReturn.inKMUTT = GetField(pRecordset, "inKMUTT")
                vReturn.group_id = GetField(pRecordset, "group_id")
                vReturn.campus_id = GetField(pRecordset, "campus_id")
                vReturn.faculty_id = GetField(pRecordset, "faculty_id")
                vReturn.department_id = GetField(pRecordset, "department_id")
                vReturn.firstname = GetField(pRecordset, "firstname")
                vReturn.lastname = GetField(pRecordset, "lastname")
                vReturn.position = GetField(pRecordset, "position")
                vReturn.level = GetField(pRecordset, "level")
                vReturn.level_manager = GetField(pRecordset, "level_manager")
                vReturn.active = GetField(pRecordset, "active")

                vReturn.campus = GetField(pRecordset, "campus_name")
                vReturn.faculty = GetField(pRecordset, "faculty_name")
                vReturn.department = GetField(pRecordset, "department_name")
                vReturn.group = GetField(pRecordset, "group_name")

                vReturn.UserID = vReturn.username
            Else
                vReturn.UserID = ""
            End If

        Catch ex As Exception
            gErrorMessage = ex.Message
        Finally
            If pRecordset.State > 0 Then
                pRecordset.Close()
            End If

            CloseDB(pConnection)
        End Try

        GetUserInfo = vReturn
    End Function

    Public Function GetLoginName(ByVal UserID As String) As String
        Dim SQL As String
        Dim pRecordSet As New ADODB.Recordset
        Dim pConnection As ADODB.Connection = Nothing

        Try
            pConnection = ConnectDB()

            SQL = "SELECT username FROM tblUser WHERE UserID = '" & UserID.Trim & "'"

            OpenTable(pConnection, pRecordSet, SQL)

            If pRecordSet.RecordCount > 0 Then
                GetLoginName = GetField(pRecordSet, "username")
            Else
                GetLoginName = ""
            End If

        Catch ex As Exception
            GetLoginName = ""
            gErrorMessage = ex.Message
        Finally
            If pRecordSet.State > 0 Then
                pRecordSet.Close()
            End If

            CloseDB(pConnection)
        End Try
    End Function

    Public Function GetLevelManagerByLoginName(ByVal LoginName As String) As String
        Dim SQL As String
        Dim pRecordSet As New ADODB.Recordset
        Dim pConnection As ADODB.Connection = Nothing

        Try
            pConnection = ConnectDB()

            SQL = "SELECT level_manager FROM tblUser WHERE username = '" & LoginName.Trim & "'"

            OpenTable(pConnection, pRecordSet, SQL)

            If pRecordSet.RecordCount > 0 Then
                GetLevelManagerByLoginName = GetField(pRecordSet, "level_manager")
            Else
                GetLevelManagerByLoginName = ""
            End If

        Catch ex As Exception
            GetLevelManagerByLoginName = ""
            gErrorMessage = ex.Message
        Finally
            If pRecordSet.State > 0 Then
                pRecordSet.Close()
            End If

            CloseDB(pConnection)
        End Try
    End Function

    Public Function GetGroupName(ByVal GroupID As String) As String
        Dim pRecordset As New ADODB.Recordset
        Dim pConnection As ADODB.Connection = Nothing
        Dim SQL As String
        Dim pReturn As String = ""

        Try
            pConnection = ConnectDB()

            SQL = "SELECT Group_Name FROM tblMT_Group WHERE Group_ID = '" & GroupID & "'"

            OpenTable(pConnection, pRecordset, SQL)

            pReturn = GetField(pRecordset, "Group_Name")

        Catch ex As Exception
            gErrorMessage = ex.Message
        Finally
            If pRecordset.State > 0 Then
                pRecordset.Close()
            End If

            CloseDB(pConnection)
        End Try
        GetGroupName = pReturn
    End Function

    Public Function GetPositionName(ByVal PositionID As String) As String
        Dim pRecordset As New ADODB.Recordset
        Dim pConnection As ADODB.Connection = Nothing
        Dim SQL As String
        Dim pReturn As String = ""

        Try
            pConnection = ConnectDB()

            SQL = "SELECT Position_Name FROM tblMT_Position WHERE Position_ID = '" & PositionID & "'"

            OpenTable(pConnection, pRecordset, SQL)

            pReturn = GetField(pRecordset, "Position_Name")

        Catch ex As Exception
            gErrorMessage = ex.Message
        Finally
            If pRecordset.State > 0 Then
                pRecordset.Close()
            End If

            CloseDB(pConnection)
        End Try
        GetPositionName = pReturn
    End Function

    Public Function GetLevelName(ByVal LevelID As String) As String
        Dim pRecordset As New ADODB.Recordset
        Dim pConnection As ADODB.Connection = Nothing
        Dim SQL As String
        Dim pReturn As String = ""

        Try
            pConnection = ConnectDB()

            SQL = "SELECT Level_Name FROM tblMT_Level WHERE Level_ID = '" & LevelID & "'"

            OpenTable(pConnection, pRecordset, SQL)

            pReturn = GetField(pRecordset, "Level_Name")

        Catch ex As Exception
            gErrorMessage = ex.Message
        Finally
            If pRecordset.State > 0 Then
                pRecordset.Close()
            End If

            CloseDB(pConnection)
        End Try
        GetLevelName = pReturn
    End Function

    Public Function GetUserFacultyName(ByVal User As _USER_) As String
        Dim pRecordset As New ADODB.Recordset
        Dim pConnection As ADODB.Connection = Nothing
        Dim SQL As String
        Dim pReturn As String = ""

        Try
            pConnection = ConnectDB()

            SQL = "SELECT F.faculty_name " & _
                  " FROM tblMT_Campus C, tblMT_Faculty F " & _
                  " WHERE C.campus_id = F.campus_id " & _
                  " AND C.campus_id = '" & User.campus_id & "' " & _
                  " AND F.faculty_id = '" & User.faculty_id & "' "

            OpenTable(pConnection, pRecordset, SQL)

            pReturn = GetField(pRecordset, "faculty_name")

        Catch ex As Exception
            gErrorMessage = ex.Message
        Finally
            If pRecordset.State > 0 Then
                pRecordset.Close()
            End If

            CloseDB(pConnection)
        End Try
        GetUserFacultyName = pReturn
    End Function

    Public Function GetUserDepartmentName(ByVal User As _USER_) As String
        Dim pRecordset As New ADODB.Recordset
        Dim pConnection As ADODB.Connection = Nothing
        Dim SQL As String
        Dim pReturn As String = ""

        Try
            pConnection = ConnectDB()

            SQL = "SELECT D.department_name " & _
                  " FROM tblMT_Campus C, tblMT_Faculty F, tblMT_Department D " & _
                  " WHERE C.campus_id = F.campus_id " & _
                  " AND C.campus_id = D.campus_id " & _
                  " AND D.faculty_id = F.faculty_id " & _
                  " AND C.campus_id = '" & User.campus_id & "' " & _
                  " AND F.faculty_id = '" & User.faculty_id & "' " & _
                  " AND D.department_id = '" & User.department_id & "'"

            OpenTable(pConnection, pRecordset, SQL)

            pReturn = GetField(pRecordset, "department_name")

        Catch ex As Exception
            gErrorMessage = ex.Message
        Finally
            If pRecordset.State > 0 Then
                pRecordset.Close()
            End If

            CloseDB(pConnection)
        End Try
        GetUserDepartmentName = pReturn
    End Function

    'Public Function GetUserLocationID(ByVal User As _USER_) As String
    '    Dim pRecordset As New ADODB.Recordset
    '    Dim pConnection As ADODB.Connection = Nothing
    '    Dim SQL As String
    '    Dim pReturn As String = ""

    '    Try
    '        pConnection = ConnectDB()

    '        SQL = "SELECT L.LocationID " & _
    '              " FROM tblMT_Campus C " & _
    '              " WHERE C.campus_id = '" & User.campus_id & "' "

    '        OpenTable(pConnection, pRecordset, SQL)

    '        pReturn = GetField(pRecordset, "LocationID")

    '    Catch ex As Exception
    '        gErrorMessage = ex.Message
    '    Finally
    '        If pRecordset.State > 0 Then
    '            pRecordset.Close()
    '        End If

    '        CloseDB(pConnection)
    '    End Try
    '    GetUserLocationID = pReturn
    'End Function

    'Public Function GetUserFacultyID(ByVal User As _USER_) As String
    '    Dim pRecordset As New ADODB.Recordset
    '    Dim pConnection As ADODB.Connection = Nothing
    '    Dim SQL As String
    '    Dim pReturn As String = ""

    '    Try
    '        pConnection = ConnectDB()

    '        SQL = "SELECT F.FacultyID " & _
    '              " FROM tblMT_Location L, tblMT_Faculty F " & _
    '              " WHERE L.LocationID = F.LocationID " & _
    '              " AND L.LocationCode = '" & User.LocationCode & "' " & _
    '              " AND F.FacultyCode = '" & User.FacultyCode & "' "

    '        OpenTable(pConnection, pRecordset, SQL)

    '        pReturn = GetField(pRecordset, "FacultyID")

    '    Catch ex As Exception
    '        gErrorMessage = ex.Message
    '    Finally
    '        If pRecordset.State > 0 Then
    '            pRecordset.Close()
    '        End If

    '        CloseDB(pConnection)
    '    End Try
    '    GetUserFacultyID = pReturn
    'End Function

    'Public Function GetUserDepartmentID(ByVal User As _USER_) As String
    '    Dim pRecordset As New ADODB.Recordset
    '    Dim pConnection As ADODB.Connection = Nothing
    '    Dim SQL As String
    '    Dim pReturn As String = ""

    '    Try
    '        pConnection = ConnectDB()

    '        SQL = "SELECT D.DepartmentID " & _
    '              " FROM tblMT_Location L, tblMT_Faculty F, tblMT_Department D " & _
    '              " WHERE L.LocationID = F.LocationID " & _
    '              " AND D.FacultyID = F.FacultyID " & _
    '              " AND L.LocationCode = '" & User.LocationCode & "' " & _
    '              " AND F.FacultyCode = '" & User.FacultyCode & "' " & _
    '              " AND D.DepartmentCode = '" & User.DepartmentCode & "'"

    '        OpenTable(pConnection, pRecordset, SQL)

    '        pReturn = GetField(pRecordset, "DepartmentID")

    '    Catch ex As Exception
    '        gErrorMessage = ex.Message
    '    Finally
    '        If pRecordset.State > 0 Then
    '            pRecordset.Close()
    '        End If

    '        CloseDB(pConnection)
    '    End Try
    '    GetUserDepartmentID = pReturn
    'End Function

    'Public Function GetUserGroupName(ByVal UserID As String) As String
    '    Dim SQL As String
    '    Dim pRecordSet As New ADODB.Recordset
    '    Dim pConnection As ADODB.Connection = Nothing

    '    Try
    '        pConnection = ConnectDB()

    '        SQL = "SELECT G.GroupName " & _
    '              " FROM tblMT_User U, tblMT_Group G " & _
    '              " WHERE U.GroupID = G.GroupID " & _
    '              "  AND U.UserID = '" & UserID.Trim & "'"

    '        OpenTable(pConnection, pRecordSet, SQL)

    '        If pRecordSet.RecordCount > 0 Then
    '            GetUserGroupName = GetField(pRecordSet, "GroupName")
    '        Else
    '            GetUserGroupName = ""
    '        End If

    '    Catch ex As Exception
    '        GetUserGroupName = ""
    '        gErrorMessage = ex.Message
    '    Finally
    '        If pRecordSet.State > 0 Then
    '            pRecordSet.Close()
    '        End If

    '        CloseDB(pConnection)
    '    End Try
    'End Function

    'Public Function GetUserGroupID(ByVal UserID As String) As String
    '    Dim SQL As String
    '    Dim pRecordSet As New ADODB.Recordset
    '    Dim pConnection As ADODB.Connection = Nothing

    '    Try
    '        pConnection = ConnectDB()

    '        SQL = "SELECT U.GroupID " & _
    '              " FROM tblMT_User U " & _
    '              " WHERE  U.UserID = '" & UserID.Trim & "'"

    '        OpenTable(pConnection, pRecordSet, SQL)

    '        If pRecordSet.RecordCount > 0 Then
    '            GetUserGroupID = GetField(pRecordSet, "GroupID")
    '        Else
    '            GetUserGroupID = ""
    '        End If

    '    Catch ex As Exception
    '        GetUserGroupID = ""
    '        gErrorMessage = ex.Message
    '    Finally
    '        If pRecordSet.State > 0 Then
    '            pRecordSet.Close()
    '        End If

    '        CloseDB(pConnection)
    '    End Try
    'End Function

    'Public Function GetUserDepartmentName(ByVal UserID As String) As String
    '    Dim SQL As String
    '    Dim pRecordSet As New ADODB.Recordset
    '    Dim pConnection As ADODB.Connection = Nothing

    '    Try
    '        pConnection = ConnectDB()

    '        SQL = "SELECT D.DepartmentName " & _
    '              " FROM tblMT_User U, tblMT_Department D " & _
    '              " WHERE D.DepartmentID = U.DepartmentID " & _
    '              "  AND U.UserID = '" & UserID.Trim & "'"

    '        OpenTable(pConnection, pRecordSet, SQL)

    '        If pRecordSet.RecordCount > 0 Then
    '            GetUserDepartmentName = GetField(pRecordSet, "DepartmentName")
    '        Else
    '            GetUserDepartmentName = ""
    '        End If

    '    Catch ex As Exception
    '        GetUserDepartmentName = ""
    '        gErrorMessage = ex.Message
    '    Finally
    '        If pRecordSet.State > 0 Then
    '            pRecordSet.Close()
    '        End If

    '        CloseDB(pConnection)
    '    End Try
    'End Function

    'Public Function GetUserFacultyName(ByVal UserID As String) As String
    '    Dim SQL As String
    '    Dim pRecordSet As New ADODB.Recordset
    '    Dim pConnection As ADODB.Connection = Nothing

    '    Try
    '        pConnection = ConnectDB()

    '        SQL = "SELECT F.FacultyName " & _
    '              " FROM tblMT_User U, tblMT_Faculty F " & _
    '              " WHERE F.FacultyID = U.FacultyID " & _
    '              "  AND U.UserID = '" & UserID.Trim & "'"

    '        OpenTable(pConnection, pRecordSet, SQL)

    '        If pRecordSet.RecordCount > 0 Then
    '            GetUserFacultyName = GetField(pRecordSet, "FacultyName")
    '        Else
    '            GetUserFacultyName = ""
    '        End If

    '    Catch ex As Exception
    '        GetUserFacultyName = ""
    '        gErrorMessage = ex.Message
    '    Finally
    '        If pRecordSet.State > 0 Then
    '            pRecordSet.Close()
    '        End If

    '        CloseDB(pConnection)
    '    End Try
    'End Function
End Module
