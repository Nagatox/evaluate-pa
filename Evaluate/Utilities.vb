Imports System.Web.Configuration
Imports System.Configuration
Imports System.Text
Imports System
Imports System.Data
Imports System.Data.OleDb
Imports System.IO
Imports System.Web.UI.HtmlControls.HtmlInputFile
Imports Microsoft.Office.Interop

Module Utilities
    '----  System Variables  ----
    Public gServerName As String
    Public gDatabaseName As String
    Public gUserName As String
    Public gUserPassword As String
    Public gConnectionString As String

    Public geBudgetingConnectionString As String

    Public rptPath As String
    Public vWorkDay As Integer = 6

    Public gVersion As String

    Public Class _eBudgeting_
        Public CampusID As String
        Public FacultyID As String
        Public DepartmentID As String

        Public PlanID As String
    End Class

    Public Sub InitializeSystemVariables()

        System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("th-TH")
        gVersion = "1.00"

        'Dim rootWebConfig1 As System.Configuration.Configuration
        'Dim CSServerName As System.Configuration.KeyValueConfigurationElement
        'Dim CSDatabaseName As System.Configuration.KeyValueConfigurationElement
        'Dim CSUserName As System.Configuration.KeyValueConfigurationElement
        'Dim CSPassword As System.Configuration.KeyValueConfigurationElement

        'rootWebConfig1 = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~/")
        'If (0 < rootWebConfig1.AppSettings.Settings.Count) Then
        '    CSServerName = rootWebConfig1.AppSettings.Settings("ServerName")
        '    CSDatabaseName = rootWebConfig1.AppSettings.Settings("DatabaseName")
        '    CSUserName = rootWebConfig1.AppSettings.Settings("UserName")
        '    CSPassword = rootWebConfig1.AppSettings.Settings("Password")

        '    gServerName = CSServerName.Value
        '    gDatabaseName = CSDatabaseName.Value
        '    gUserName = CSUserName.Value
        '    gUserPassword = CSPassword.Value

        '    CSServerName = rootWebConfig1.AppSettings.Settings("eBudgetingServerName")
        '    CSDatabaseName = rootWebConfig1.AppSettings.Settings("eBudgetingDatabaseName")
        '    CSUserName = rootWebConfig1.AppSettings.Settings("eBudgetingUserName")
        '    CSPassword = rootWebConfig1.AppSettings.Settings("eBudgetingPassword")

        'End If

        'gConnectionString = "Provider=SQLOLEDB;Data Source=" & gServerName & ";" & _
        '    "Initial Catalog=" & gDatabaseName & ";User ID=" & gUserName & ";" & _
        '    "Password=" & gUserPassword

    End Sub

    Public Function ConnectDB() As ADODB.Connection

        Dim DatabaseConnection As New ADODB.Connection

        Dim sServerName As String
        Dim sDatabaseName As String
        Dim sUserName As String
        Dim sUserPassword As String

        Try
            sServerName = System.Configuration.ConfigurationManager.AppSettings("ServerName")
            sDatabaseName = System.Configuration.ConfigurationManager.AppSettings("DatabaseName")
            sUserName = System.Configuration.ConfigurationManager.AppSettings("UserName")
            sUserPassword = System.Configuration.ConfigurationManager.AppSettings("Password")

            gConnectionString = "Provider=SQLOLEDB;Data Source=" & sServerName & ";" & _
                "Initial Catalog=" & sDatabaseName & ";User ID=" & sUserName & ";" & _
                "Password=" & sUserPassword

            DatabaseConnection.Open(gConnectionString)
        Catch ex As Exception
            Err.Raise(-1)
        End Try

        ConnectDB = DatabaseConnection
    End Function

    Public Function ConnecteBudgetingDB() As ADODB.Connection

        Dim DatabaseConnection As New ADODB.Connection

        Dim seBudgetingServerName As String
        Dim seBudgetingDatabaseName As String
        Dim seBudgetingUserName As String
        Dim seBudgetingUserPassword As String

        Try
            seBudgetingServerName = System.Configuration.ConfigurationManager.AppSettings("eBudgetingServerName")
            seBudgetingDatabaseName = System.Configuration.ConfigurationManager.AppSettings("eBudgetingDatabaseName")
            seBudgetingUserName = System.Configuration.ConfigurationManager.AppSettings("eBudgetingUserName")
            seBudgetingUserPassword = System.Configuration.ConfigurationManager.AppSettings("eBudgetingPassword")

            geBudgetingConnectionString = "Provider=SQLOLEDB;Data Source=" & seBudgetingServerName & ";" & _
                "Initial Catalog=" & seBudgetingDatabaseName & ";User ID=" & seBudgetingUserName & ";" & _
                "Password=" & seBudgetingUserPassword

            DatabaseConnection.Open(geBudgetingConnectionString)
        Catch ex As Exception
            Err.Raise(-1)
        End Try

        ConnecteBudgetingDB = DatabaseConnection
    End Function

    Public Sub CloseDB(ByRef Connection As ADODB.Connection)
        If Not (Connection Is Nothing) Then
            Try
                Connection.Close()
            Catch ex As Exception

            End Try

        End If
        Connection = Nothing
    End Sub


    Public Function OpenTable(ByVal Connection As ADODB.Connection, ByVal pRecordset As ADODB.Recordset, ByVal pQuery As String) As Long
        pRecordset.CursorLocation = ADODB.CursorLocationEnum.adUseClient
        pRecordset.Open(pQuery, Connection, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic)
        OpenTable = pRecordset.RecordCount
    End Function

    Public Function CheckDataTypeAndFormatField(ByVal pControl As TextBox, ByVal pFormat As String, ByVal pBorder As Boolean) As Boolean
        Dim vReturnValue As Boolean

        vReturnValue = True
        Select Case True
            Case UCase(pFormat) Like "NUMBER"
                If IsNumeric(SetNumberFormat(pControl.Text)) Then
                    'pControl.Text = SetDataFormat(pControl.Text, pFormat)
                Else
                    vReturnValue = False
                End If
            Case UCase(pFormat) Like "CURRENCY*" Or UCase(pFormat) Like "QUANTITY*"
                If IsNumeric(SetNumberFormat(pControl.Text)) Then
                    pControl.Text = SetDataFormat(pControl.Text, pFormat)
                Else
                    vReturnValue = False
                End If
            Case UCase(pFormat) Like "DATE*"
                If Trim(pControl.Text) <> "" Then
                    If IsDate(pControl.Text) Then
                        pControl.Text = SetDataFormat(pControl.Text, pFormat)
                    Else
                        vReturnValue = False
                    End If
                End If
            Case UCase(pFormat) Like "WEEK*"
                If IsNumeric(SetNumberFormat(pControl.Text)) Then
                    pControl.Text = SetDataFormat(pControl.Text, "QUANTITY0")
                    If (pControl.Text >= 54) Or (pControl.Text <= 0) Then
                        vReturnValue = False
                        pControl.Text = 1
                    End If
                Else
                    vReturnValue = False
                    pControl.Text = 1
                End If

            Case UCase(pFormat) Like "YEAR*"
                If IsNumeric(SetNumberFormat(pControl.Text)) Then
                    pControl.Text = SetDataFormat(pControl.Text, "YEAR0")
                    If (pControl.Text >= 2100) Or (pControl.Text <= 2000) Then
                        vReturnValue = False
                        pControl.Text = Date.Today.Year
                    End If
                Else
                    vReturnValue = False
                    pControl.Text = Date.Today.Year
                End If

        End Select
        Call SetControlError(vReturnValue, pControl, pBorder)
        CheckDataTypeAndFormatField = vReturnValue
    End Function

    'Public Function CheckKey(ByVal pControl As TextBox, ByVal pBorder As Boolean, ByVal pQuery As String, ByRef pValue() As String) As Boolean
    '    Dim vRecordset As New ADODB.Recordset
    '    Dim vReturnValue As Boolean
    '    Dim vNumOfRec As Long
    '    Dim vValue() As String
    '    Dim i As Integer
    '    Dim vStop As Integer

    '    vNumOfRec = OpenTable(vRecordset, pQuery)
    '    vStop = vRecordset.Fields.Count - 1
    '    ReDim vValue(vStop + 1)
    '    If vNumOfRec = 0 Then
    '        vReturnValue = False
    '        For i = 0 To vStop
    '            vValue(i) = ""
    '        Next
    '    Else
    '        vReturnValue = True
    '        For i = 0 To vStop
    '            vValue(i) = GetField(vRecordset, vRecordset.Fields(i).Name)
    '        Next
    '    End If
    '    If Trim(pControl.Text) = "" Then vReturnValue = True
    '    pValue = vValue
    '    Call SetControlError(vReturnValue, pControl, pBorder)
    '    CheckKey = vReturnValue
    'End Function

    Public Function SetControlError(ByVal pError As Boolean, ByVal pControl As TextBox, ByVal pBorder As Boolean) As Boolean
        If pError Then
            'If pBorder Then
            pControl.BorderStyle = BorderStyle.Inset
            pControl.BorderWidth = System.Web.UI.WebControls.Unit.Pixel(2)
            'Else
            '    pControl.BorderStyle = BorderStyle.None
            '    pControl.BorderWidth = System.Web.UI.WebControls.Unit.Pixel(0)
            'End If
            pControl.BorderColor = System.Drawing.Color.Empty
        Else
            pControl.BorderStyle = BorderStyle.Solid
            pControl.BorderWidth = System.Web.UI.WebControls.Unit.Pixel(1)
            pControl.BorderColor = System.Drawing.Color.Red
        End If
        SetControlError = pError
    End Function

    Public Function GetField(ByVal pRecordset As ADODB.Recordset, ByVal pFieldName As String) As String
        Dim vReturnValue As String
        Dim vTemp

        vTemp = pRecordset.Fields(pFieldName).Value
        Select Case pRecordset.Fields(pFieldName).Type
            Case 135 'Date Time
                If vTemp Is System.DBNull.Value Then
                    vReturnValue = ""
                Else
                    vReturnValue = vTemp
                End If
            Case 2, 3, 17, 20 'Integer
                If vTemp Is System.DBNull.Value Then
                    vReturnValue = ""
                Else
                    vReturnValue = vTemp
                End If
            Case 5, 6, 131, 204 'Numeric
                If vTemp Is System.DBNull.Value Then
                    vReturnValue = "0.00"
                Else
                    vReturnValue = vTemp
                End If
            Case 129, 130, 200, 201, 202, 203 'String
                If vTemp Is System.DBNull.Value Then
                    vReturnValue = ""
                Else
                    vReturnValue = Trim(vTemp)
                End If
            Case Else
                If vTemp Is System.DBNull.Value Then
                    vReturnValue = ""
                Else
                    vReturnValue = vTemp
                End If
        End Select
        GetField = vReturnValue
    End Function

    Public Function GetRunningNumber(ByRef tConnection As ADODB.Connection, ByVal pRunningName As String) As Long
        Dim vReturnValue As Long
        Dim vRecordset As New ADODB.Recordset
        Dim vQuery As String

        Try

            vQuery = "SELECT * FROM tblCFG_RunningNumber WHERE (RunningName = '" & pRunningName & "') "
            If OpenTable(tConnection, vRecordset, vQuery) = 0 Then
                vRecordset.AddNew()
                Call PutField(vRecordset, "RunningName", pRunningName)
                Call PutField(vRecordset, "LastNumber", 1)
                vReturnValue = 1
            Else
                vReturnValue = CLng(GetField(vRecordset, "LastNumber")) + 1
                Call PutField(vRecordset, "LastNumber", vReturnValue)
            End If
            vRecordset.Update()

        Catch ex As Exception
            Throw New ApplicationException(ex.Message)
        Finally
            If vRecordset.State > 0 Then
                vRecordset.Close()
            End If
        End Try

        GetRunningNumber = vReturnValue
    End Function

    Public Function GetRunningNumber(ByRef tConnection As ADODB.Connection, ByVal pRunningName As String, ByVal checkYear As String) As Long
        Dim vReturnValue As Long
        Dim vRecordset As New ADODB.Recordset
        Dim vQuery As String

        Try

            vQuery = "SELECT * FROM tblCFG_RunningNumber WHERE (RunningName = '" & pRunningName & "') And (sYear = '" & checkYear & "')"
            If OpenTable(tConnection, vRecordset, vQuery) = 0 Then
                vRecordset.AddNew()
                Call PutField(vRecordset, "RunningName", pRunningName)
                Call PutField(vRecordset, "sYear", checkYear)
                Call PutField(vRecordset, "LastNumber", 1)
                vReturnValue = 1
            Else
                vReturnValue = GetField(vRecordset, "LastNumber") + 1
                Call PutField(vRecordset, "LastNumber", vReturnValue)
            End If
            vRecordset.Update()
        Catch ex As Exception
            Throw New ApplicationException(ex.Message)
        Finally
            If vRecordset.State > 0 Then
                vRecordset.Close()
            End If

        End Try

        GetRunningNumber = vReturnValue
    End Function

    Public Function NextNumber(ByVal pStatus) As Integer
        Static vNumber As Integer

        If pStatus = 0 Then
            vNumber = 0
        Else
            vNumber = vNumber + 1
        End If
        NextNumber = vNumber
    End Function

    Public Sub PutField(ByVal pRecordset As ADODB.Recordset, ByVal pFieldName As String, ByVal pValue As String)
        Dim vReturnValue

        Select Case pRecordset.Fields(pFieldName).Type
            Case 135 'Date Time
                If IsDate(pValue) Then
                    vReturnValue = pValue
                Else
                    vReturnValue = System.DBNull.Value
                End If
            Case 2, 3, 17, 20, 5, 6, 131, 204 'Integer and Numeric
                vReturnValue = SetNumber(pValue)
            Case 129, 130, 200, 201, 202, 203 'String
                vReturnValue = Trim(pValue)
            Case Else
                vReturnValue = pValue
        End Select
        pRecordset.Fields(pFieldName).Value = vReturnValue
    End Sub

    Public Function SetDataFormat(ByVal pValue As String, ByVal pDataFormat As String) As String
        Dim vReturnValue As String = ""

        Select Case UCase(pDataFormat)
            Case "DATE"
                vReturnValue = Format(pValue, "Short Date")
            Case "CURRENCY2"
                vReturnValue = Format(SetNumber(pValue), "##,##0.00")
            Case "CURRENCY3"
                vReturnValue = Format(SetNumber(pValue), "##,##0.000")
            Case "CURRENCY4"
                vReturnValue = Format(SetNumber(pValue), "##,##0.0000")
            Case "QUANTITY0"
                vReturnValue = Format(SetNumber(pValue), "##,##0")
            Case "QUANTITY1"
                vReturnValue = Format(SetNumber(pValue), "##,##0.0")
            Case "QUANTITY2"
                vReturnValue = Format(SetNumber(pValue), "##,##0.00")
            Case "QUANTITY3"
                vReturnValue = Format(SetNumber(pValue), "##,##0.000")
            Case "QUANTITY4"
                vReturnValue = Format(SetNumber(pValue), "##,##0.0000")
            Case "YEAR0"
                vReturnValue = Format(SetNumber(pValue), "###0")
            Case "TRANSACTIONNUMBER"
                vReturnValue = Format(SetNumber(pValue), "###0")
        End Select
        SetDataFormat = vReturnValue
    End Function

    Public Function SetNumber(ByVal pNumber As String) As Integer
        Dim vTemp As String
        Dim vReturnValue As Integer

        vTemp = Replace(pNumber, ",", "")
        vTemp = Replace(vTemp, "$", "")
        vTemp = Replace(vTemp, "฿", "")
        If IsNumeric(vTemp) Then
            vReturnValue = Val(vTemp)
        Else
            vReturnValue = 0
        End If
        SetNumber = vReturnValue
    End Function

    Public Function SetNumberFormat(ByVal pNumber As String) As String
        Dim vTemp As String
        Dim vReturnValue As String

        vTemp = Replace(pNumber, ",", "")
        vTemp = Replace(vTemp, "$", "")
        vTemp = Replace(vTemp, "฿", "")
        If IsNumeric(vTemp) Then
            vReturnValue = Val(vTemp)
        Else
            If Trim(pNumber) = "" Then
                vReturnValue = 0
            Else
                vReturnValue = pNumber
            End If
        End If
        SetNumberFormat = vReturnValue

    End Function

    Public Function DateToWeekDefaultMon(ByVal inDate As Date) As Integer
        Const JAN As Integer = 1
        Const DEC As Integer = 12
        Const LASTDAYOFDEC As Integer = 31
        Const FIRSTDAYOFJAN As Integer = 1
        Const THURSDAY As Integer = 4
        Dim ThursdayFlag As Boolean = False
        Const MONDAY As Integer = 1
        Dim MondayFlag As Boolean = False
        ' Get the day number since the beginning of the year
        Dim DayOfYear As Integer = inDate.DayOfYear

        ' Get the numeric weekday of the first day of the
        ' year (using sunday as FirstDay)
        Dim StartWeekDayOfYear As Integer

        Dim tempYear As Long
        Dim tempDay As Date
        tempYear = inDate.Year

        tempDay = ("#01/01/" & tempYear & "#")

        StartWeekDayOfYear = tempDay.DayOfWeek

        'StartWeekDayOfYear = DateString(inDate.Year, JAN, FIRSTDAYOFJAN).DayOfWeek
        'StartWeekDayOfYear = DateTime(inDate.Year, JAN, FIRSTDAYOFJAN).DayOfWeek
        'DirectCast(New DateTime(inDate.Year, JAN, FIRSTDAYOFJAN).DayOfWeek, Integer)
        Dim EndWeekDayOfYear As Integer

        tempYear = inDate.Year
        tempDay = "31/12/" & tempYear

        EndWeekDayOfYear = tempDay.DayOfWeek

        'DirectCast(New FormatDateTime(inDate.Year, DEC, LASTDAYOFDEC).DayOfWeek, Integer)()

        ' Compensate for the fact that we are using monday
        ' as the first day of the week
        If StartWeekDayOfYear = 0 Then
            StartWeekDayOfYear = 7
        End If
        If EndWeekDayOfYear = 0 Then
            EndWeekDayOfYear = 7
        End If

        ' Calculate the number of days in the first and last week
        Dim DaysInFirstWeek As Integer = 8 - StartWeekDayOfYear
        Dim DaysInLastWeek As Integer = 8 - EndWeekDayOfYear

        '' If the year either starts or ends on a thursday it will have a 53rd week

        If StartWeekDayOfYear = MONDAY Then
            MondayFlag = True
        End If

        ' We begin by calculating the number of FULL weeks between the start of the year and
        ' our date. The number is rounded up, so the smallest possible value is 0.
        Dim FullWeeks As Integer = _
            CType(Math.Ceiling((DayOfYear - DaysInFirstWeek) / 7), Integer)

        Dim WeekNumber As Integer = FullWeeks

        ' If the first week of the year has at least four days, then the actual week number for our date
        ' can be incremented by one.

        If DaysInFirstWeek = 7 Then
            WeekNumber = WeekNumber + 1
        End If

        ' If week number is larger than week 52 (and the year doesn't either start or end on a thursday)
        ' then the correct week number is 1.
        If WeekNumber > 52 AndAlso Not MondayFlag Then
            WeekNumber = 1
        End If

        'If WeekNumber = 0 AndAlso Not MondayFlag Then
        '    WeekNumber = 1
        'End If

        'If week number is still 0, it means that we are trying to evaluate the week number for a
        'week that belongs in the previous year (since that week has 3 days or less in our date's year).
        'We therefore make a recursive call using the last day of the previous year.
        If WeekNumber = 0 Then
            WeekNumber = DateToWeekDefaultMon( _
                New DateTime(inDate.Year - 1, DEC, LASTDAYOFDEC))
        End If
        Return WeekNumber
    End Function

    Public Function WeekToDateDefaultMon(ByVal inYear As Integer, ByVal calweek As Integer, ByVal pCheck As Integer) As String
        Const JAN As Integer = 1
        Const DEC As Integer = 12
        Const LASTDAYOFDEC As Integer = 31
        Const FIRSTDAYOFJAN As Integer = 1
        Const THURSDAY As Integer = 4
        Dim ThursdayFlag As Boolean = False
        Const MONDAY As Integer = 1
        Dim MondayFlag As Boolean = False


        Dim tempYear As Long
        Dim tempDay As Date

        Dim EndWeekDayOfYear As Integer

        tempYear = inYear
        tempDay = "31/12/" & tempYear

        EndWeekDayOfYear = tempDay.DayOfWeek

        Dim StartWeekDayOfYear As Integer

        tempYear = inYear

        tempDay = "01/01/" & tempYear

        StartWeekDayOfYear = tempDay.DayOfWeek

        Dim DayOfYear As Integer = tempDay.DayOfYear

        ' Compensate for the fact that we are using monday
        ' as the first day of the week
        If StartWeekDayOfYear = 0 Then
            StartWeekDayOfYear = 7
        End If
        If EndWeekDayOfYear = 0 Then
            EndWeekDayOfYear = 7
        End If

        ' Calculate the number of days in the first and last week
        Dim DaysInFirstWeek As Integer = 8 - StartWeekDayOfYear
        Dim DaysInLastWeek As Integer = 8 - EndWeekDayOfYear

        If StartWeekDayOfYear = MONDAY Then
            MondayFlag = True
        End If

        'calweek
        Dim DaysForCalFromWeek As Integer
        Dim DateOfWeek As String

        If pCheck = 0 Then     '0 = วันจันทร์ 1 = วันอาทิตย์ 2 = วันเสาร์ ตอนนี้ โดน เปลี่ยน 1 และ 2 จะกลายเป็น อาทิตย์ หมด
            DaysForCalFromWeek = ((calweek - 1) * 7) + DaysInFirstWeek
        ElseIf pCheck = 2 Then
            DaysForCalFromWeek = ((calweek - 1) * 7) + DaysInFirstWeek + 6
        ElseIf pCheck = 1 Then
            DaysForCalFromWeek = ((calweek - 1) * 7) + DaysInFirstWeek + 6
        End If

        If MondayFlag Then
            DaysForCalFromWeek = DaysForCalFromWeek - 7
        End If

        DateOfWeek = FormatDateTime(DateAdd("d", DaysForCalFromWeek, tempDay), DateFormat.ShortDate)


        Return DateOfWeek
    End Function

    Public Function TransilateDateToSql(ByVal pdate As Date) As String
        Dim DateForSql As String
        DateForSql = pdate.Month & "/" & pdate.Day & "/" & pdate.Year
        Return DateForSql
    End Function

    Public Function TransilateDateToCrytal(ByVal pdate As Date) As String
        Dim DateForSql As String

        DateForSql = pdate.Year & "," & pdate.Month & "," & pdate.Day
        Return DateForSql
    End Function

    Public Function TransilateSaveToSql(ByVal pdate As Date) As String
        Dim DateForSql As String
        Dim tyear As Long

        'tyear = pdate.Year + 543

        tyear = pdate.Year

        'tyear = pdate.Year
        DateForSql = pdate.Day & "/" & pdate.Month & "/" & tyear
        Return DateForSql
    End Function

    Public Function FormatDate(ByVal pDate As String, ByVal pType As String) As String
        Dim sDate As String
        Dim vDate As Date
        If pDate <> "" And pDate <> "0" Then
            pDate = pDate.Substring(6, 2) + "/" + pDate.Substring(4, 2) + "/" + pDate.Substring(0, 4)
            vDate = CDate(pDate)
            If pType = "s" Then
                sDate = Format(vDate, "dd/MM/yy")
            Else
                sDate = Format(vDate, "dd/MM/yyyy")
            End If
        Else : sDate = ""
        End If
        Return sDate
    End Function

    Public Function FormatLongDate(ByVal pDate As String, ByVal pType As String) As String
        Dim sDate As String
        Dim vDate As Date
        If pDate <> "" Then
            vDate = CDate(pDate)
            If pType = "s" Then
                'sDate = vDate.Day & "/" & vDate.Month & "/" & vDate.Year
                sDate = Format(vDate, "dd/MM/yy")
            Else
                sDate = Format(vDate, "dd/MM/yyyy")
            End If
        Else : sDate = ""
        End If
        Return sDate
    End Function

    Public Function Date2SQL(ByVal pDate As String)
        Dim fDate As String
        Dim vDate As Date
        Dim lDate As Long
        If pDate <> "" And pDate <> "0" Then
            vDate = CDate(pDate)
            fDate = Format(vDate, "dd/MM/yyyy")
            lDate = CLng(fDate.Substring(6, 4) & fDate.Substring(3, 2) & fDate.Substring(0, 2))
        Else : lDate = "0"
        End If
        Return lDate

    End Function


    Public Function TransilateDateToSqlMovex(ByVal pdate As Date) As String
        Dim DateForSql, vMonth, vDay As String
        Dim vLen As Integer

        vMonth = pdate.Month
        vLen = Len(vMonth)
        If vLen = 1 Then vMonth = "0" + CStr(pdate.Month)
        vDay = pdate.Day
        vLen = Len(vDay)
        If vLen = 1 Then vDay = "0" + CStr(pdate.Day)
        DateForSql = CStr(pdate.Year) & vMonth & vDay
        Return DateForSql
    End Function


    Public Function CheckDateFormat(ByVal vDate As String) As String
        Dim vReturn As Integer
        Dim vRecordsetU As New ADODB.Recordset
        Dim vStandardBasicUnit As String
        Dim Digi1OfCode As String
        Dim tmpDate As Date

        'vDate = FormatDate(vDate, "")

        If vDate.Substring(1, 1) = "/" Then
            vDate = "0" & vDate
        End If

        If IsDate("31/12/" & Today.Year) Then
            CheckDateFormat = vDate.Substring(0, 2) & "/" & vDate.Substring(3, 2) & "/" & vDate.Substring(6, 4)
        Else
            CheckDateFormat = vDate.Substring(3, 2) & "/" & vDate.Substring(0, 2) & "/" & vDate.Substring(6, 4)
        End If

        Return CheckDateFormat
    End Function

    Public Function CheckYearFromWeek(ByVal vDate As Date, ByVal vWeek As Int16) As Int16
        Dim vMonth As Int16 = vDate.Month
        Dim vyear As Int16 = vDate.Year

        If vWeek > 50 And vMonth < 2 Then
            vyear = vyear - 1
        End If
        Return vyear

    End Function


    'Public Function SetAccessPage(ByVal pURL As String, ByVal pGID As String) As Boolean
    '    'Dim dt As New DataTable
    '    'Dim dbConn As New SqlConnection("Data Source=(local); initial catalog=dbSiamFood; user id=sf; password=")
    '    'Dim page_name As String = InStr(System.IO.Path.GetFileName(pURL), ".aspx")
    '    'page_name = Mid(System.IO.Path.GetFileName(pURL), Val(page_name))
    '    'page_name = Replace(System.IO.Path.GetFileName(pURL), page_name, "")
    '    'Dim dbdata As New SqlDataAdapter("Select * From tblGroup Where groupid ='" & pGID & "' And per_$" & page_name & " > '0'", dbConn)
    '    'dbdata.Fill(dt)
    '    'If dt.Rows.Count = 0 Then
    '    '    SetAccessPage = True
    '    'Else
    '    '    SetAccessPage = False
    '    'End If


    '    Dim vRecordset As New ADODB.Recordset
    '    Dim vConnection As ADODB.Connection = Nothing
    '    Dim vQuery As String

    '    Try
    '        vConnection = ConnectDB()

    '        Dim page_name As String = InStr(System.IO.Path.GetFileName(pURL), ".aspx")

    '        page_name = Mid(System.IO.Path.GetFileName(pURL), Val(page_name))
    '        page_name = Replace(System.IO.Path.GetFileName(pURL), page_name, "")

    '        vQuery = "Select * From tblGroup Where groupid ='" & pGID & "' And per_$" & page_name & " > '0' "

    '        If OpenTable(vConnection, vRecordset, vQuery) > 0 Then
    '            SetAccessPage = True
    '        Else
    '            SetAccessPage = False
    '        End If

    '    Catch ex As Exception

    '    Finally
    '        vRecordset.Close()
    '        vConnection.Close()
    '    End Try

    'End Function

    'Public Function SetMenuPermission(ByVal pN As ImageButton, ByVal pO As ImageButton, ByVal pC As ImageButton, ByVal pS As ImageButton, ByVal pD As ImageButton, ByVal pCenable As Boolean, ByVal pURL As String, ByVal pGID As String) As Boolean

    '    'Dim vModTemp As Integer
    '    'Dim per_R, per_S, per_E, per_D As Boolean
    '    'Dim j, y As Integer
    '    'Dim dt2 As New DataTable
    '    'Dim dbConn2 As New SqlConnection("Data Source=(local); initial catalog=dbSiamFood; user id=sa; password=")
    '    'Dim page_name2 As String = InStr(System.IO.Path.GetFileName(pURL), ".aspx")
    '    'page_name2 = Mid(System.IO.Path.GetFileName(pURL), Val(page_name2))
    '    'page_name2 = Replace(System.IO.Path.GetFileName(pURL), page_name2, "")
    '    'Dim dbdata2 As New SqlDataAdapter("Select per_$" & page_name2 & " From tblGroup Where groupid ='" & pGID & "' And per_$" & page_name2 & " > '0'", dbConn2)
    '    'dbdata2.Fill(dt2)

    '    Dim vModTemp As Integer
    '    Dim per_R, per_S, per_E, per_D As Boolean
    '    Dim vRecordset As New ADODB.Recordset
    '    Dim vQuery As String
    '    Dim page_name2 As String = InStr(System.IO.Path.GetFileName(pURL), ".aspx")

    '    page_name2 = Mid(System.IO.Path.GetFileName(pURL), Val(page_name2))
    '    page_name2 = Replace(System.IO.Path.GetFileName(pURL), page_name2, "")

    '    vQuery = "Select per_$" & page_name2 & " From tblGroup Where groupid ='" & pGID & "' And per_$" & page_name2 & " > '0' "

    '    Call OpenTable(vRecordset, vQuery)

    '    pN.Enabled = True
    '    pO.Enabled = True
    '    pC.Enabled = True
    '    pS.Enabled = True
    '    pD.Enabled = True

    '    per_R = False
    '    per_S = False
    '    per_E = False
    '    per_D = False

    '    vModTemp = GetField(vRecordset, "per_$" & page_name2)
    '    If vModTemp Mod 2 = 1 Then
    '        per_R = True
    '        vModTemp = (vModTemp / 2) - 0.9
    '    Else
    '        vModTemp = vModTemp / 2
    '    End If

    '    If vModTemp Mod 2 = 1 Then
    '        per_S = True
    '        vModTemp = (vModTemp / 2) - 0.9
    '    Else
    '        vModTemp = vModTemp / 2
    '    End If

    '    If vModTemp Mod 2 = 1 Then
    '        per_E = True
    '        vModTemp = (vModTemp / 2) - 0.9
    '    Else
    '        vModTemp = vModTemp / 2
    '    End If

    '    If vModTemp Mod 2 = 1 Then
    '        per_D = True
    '        vModTemp = (vModTemp / 2) - 0.9
    '    Else
    '        vModTemp = vModTemp / 2
    '    End If

    '    If per_R Then
    '        pN.Enabled = True
    '        pO.Enabled = per_E
    '        pC.Enabled = per_E
    '        pS.Enabled = per_S
    '        pD.Enabled = per_D
    '        If per_E Then pS.Enabled = True
    '        If per_D Then pO.Enabled = True
    '    Else
    '        pN.Enabled = False
    '        pO.Enabled = False
    '        pC.Enabled = False
    '        pS.Enabled = False
    '        pD.Enabled = False
    '    End If

    '    If pCenable Then
    '        pC.Enabled = True
    '    Else
    '        pC.Enabled = False
    '    End If
    '    SetMenuPermission = True

    '    vRecordset.Close()

    'End Function

    'Public Function ChanheNumberToMonth(ByVal pMonth As Integer) As String
    '    Dim pM As String

    '    Select Case pMonth
    '        Case 1
    '            pM = "Jan"
    '        Case 2
    '            pM = "Feb"
    '        Case 3
    '            pM = "Mar"
    '        Case 4
    '            pM = "Apl"
    '        Case 5
    '            pM = "May"
    '        Case 6
    '            pM = "Jun"
    '        Case 7
    '            pM = "Jul"
    '        Case 8
    '            pM = "Aug"
    '        Case 9
    '            pM = "Sep"
    '        Case 10
    '            pM = "Oct"
    '        Case 11
    '            pM = "Nov"
    '        Case 12
    '            pM = "Dec"
    '    End Select
    '    ChanheNumberToMonth = pM
    'End Function


    Public Function Convert2BCDate(ByVal ADDate As String) As String
        Convert2BCDate = ADDate.Substring(0, ADDate.Length - 4) & CStr(CInt(ADDate.Substring(ADDate.Length - 4)) - 543)
        'Convert2BCDate = ADDate
    End Function

    Public Function Convert2ADDate(ByVal BCDate As String) As String
        If BCDate = "" Then
            Convert2ADDate = ""
            Exit Function
        End If
        Convert2ADDate = BCDate.Substring(0, BCDate.Length - 4) & CStr(CInt(BCDate.Substring(BCDate.Length - 4)) + 543)
    End Function

    Public Function GetDropDownListIndex(ByVal drp As DropDownList, ByVal Value As String) As Integer

        drp.DataBind()
        Dim i As Integer
        For i = 0 To drp.Items.Count - 1
            If drp.Items(i).Text.Trim = Value.Trim Then
                GetDropDownListIndex = i
                Exit Function
            End If
        Next
        GetDropDownListIndex = 0
    End Function

    Public Function GetDropDownListIndexByValue(ByVal drp As DropDownList, ByVal Value As String) As Integer

        Try
            drp.DataBind()
            Dim i As Integer
            For i = 0 To drp.Items.Count - 1
                If drp.Items(i).Value.Trim = Value.Trim Then
                    GetDropDownListIndexByValue = i
                    Exit Function
                End If
            Next
            GetDropDownListIndexByValue = 0
        Catch ex As Exception
            GetDropDownListIndexByValue = 0
        End Try
    End Function

    Public Function GetRadioButtonListIndex(ByVal rbl As RadioButtonList, ByVal Value As String) As Integer

        rbl.DataBind()
        Dim i As Integer
        For i = 0 To rbl.Items.Count - 1
            If rbl.Items(i).Text.Trim = Value.Trim Then
                GetRadioButtonListIndex = i
                Exit Function
            End If
        Next
        GetRadioButtonListIndex = 0
    End Function

    'Public Function GetSheetCell(ByVal sheet As Excel.Worksheet, ByVal sCell As String) As String
    '    Dim range As Excel.Range

    '    Dim m_CurrentCulture = System.Threading.Thread.CurrentThread.CurrentCulture
    '    System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("en-US")

    '    range = sheet.Range(sCell, sCell)
    '    GetSheetCell = range.Cells.Value()
    '    If GetSheetCell Is Nothing Then
    '        GetSheetCell = ""
    '    End If

    '    System.Threading.Thread.CurrentThread.CurrentCulture = m_CurrentCulture

    'End Function

    'Public Sub PutSheetCell(ByVal sheet As Excel.Worksheet, ByVal sCell As String, ByVal value As String)
    '    Dim m_CurrentCulture = System.Threading.Thread.CurrentThread.CurrentCulture
    '    System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("en-US")

    '    sheet.Range(sCell).Item(1) = value

    '    System.Threading.Thread.CurrentThread.CurrentCulture = m_CurrentCulture
    'End Sub

    'Public Sub PutSheetCell2(ByVal sheet As Excel.Worksheet, ByVal sCell_1 As String, ByVal sCell_2 As String, ByVal value As String)
    '    'Dim m_CurrentCulture = System.Threading.Thread.CurrentThread.CurrentCulture
    '    'System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("en-US")

    '    sheet.Range(sCell_1, sCell_2).Value = value

    '    'System.Threading.Thread.CurrentThread.CurrentCulture = m_CurrentCulture
    'End Sub

    Public Function KillProcessByHwnd(ByVal hWnd As IntPtr) As Boolean
        Dim exproc As System.Diagnostics.Process
        Dim iRes, iProcID As Integer

        iRes = GetWindowThreadProcessId(hWnd, iProcID)
        exproc = System.Diagnostics.Process.GetProcessById(iProcID)

        Try
            exproc.Kill()
            KillProcessByHwnd = True
        Catch ex As Exception
            KillProcessByHwnd = False
        End Try
    End Function

    Public Function GetDateTime(ByVal d As Date) As String
        GetDateTime = d.Year & Format(d.Month, "00") & Format(d.Day, "00") & Format(d.Hour, "00") & Format(d.Minute, "00") & Format(d.Second, "00")
    End Function

    Public Function GetDate(ByVal d As Date) As String
        GetDate = d.Year & Format(d.Month, "00") & Format(d.Day, "00")
    End Function

    Public Function GetDateTimeforSQL(ByVal d As Date) As String
        GetDateTimeforSQL = d.Year & "-" & Format(d.Month, "00") & "-" & Format(d.Day, "00") & " " & Format(d.Hour, "00") & ":" & Format(d.Minute, "00") & ":" & Format(d.Second, "00")
        'GetDateTimeforSQL = Format(d.Month, "00") & "/" & Format(d.Day, "00") & "/" & d.Year & " " & Format(d.Hour, "00") & ":" & Format(d.Minute, "00") & ":" & Format(d.Second, "00")
    End Function

    Public Sub ShowMessageError(ByRef lbl As Label)
        lbl.Text = gErrorMessage
        lbl.CssClass = "message_error"
        gErrorMessage = ""
    End Sub

    Public Sub ShowMessage(ByRef lbl As Label, ByVal message As String)
        lbl.Text = message
        lbl.CssClass = "message"
    End Sub

    Public Sub ShowMessageWarning(ByRef lbl As Label, ByVal message As String)
        lbl.Text = message
        lbl.CssClass = "message_warning"
    End Sub

    Public Sub ShowMessageError(ByRef lbl As Label, ByVal message As String)
        lbl.Text = message
        lbl.CssClass = "message_error"
    End Sub

    Public Sub SetTableCell(ByRef TCell As TableCell, ByVal Width As Integer, ByVal Align As String, ByVal sClass As String, ByVal BorderColor As Drawing.Color, ByVal BorderWidth As Integer, ByVal bold As Boolean)
        TCell.Width = Width
        TCell.Style("text-align") = Align
        If bold Then
            TCell.Style("font-weight") = "bold"
        End If
        'TCell.CssClass = sClass
        'TCell.BorderColor = BorderColor
        'TCell.BorderWidth = BorderWidth
    End Sub

    Public Sub AddTable(ByRef TableProject As Table, ByVal Name As String, ByVal Value As String, ByVal Description As String, Optional ByVal RowColor As String = "")
        Dim TRow As New TableRow
        Dim TCellName As New TableCell
        Dim TCellValue As New TableCell
        Dim TCellDescription As New TableCell

        If Not RowColor.Equals("") Then
            TRow.BackColor = System.Drawing.ColorTranslator.FromHtml(RowColor)

            'TRow.BackColor = RowColor
            'TRow.BackColor = Drawing.Color.SlateBlue
            'TRow.BackColor = System.Drawing.Color.ReferenceEquals("Drawing.Color.SlateBlue")
        End If

        SetTableCell(TCellName, 0, "left", "", Drawing.Color.Black, 1, False)
        SetTableCell(TCellValue, 0, "left", "", Drawing.Color.Black, 1, False)
        SetTableCell(TCellDescription, 0, "left", "", Drawing.Color.Black, 1, False)

        TCellName.Text = "&nbsp;" & Name
        TCellValue.Text = "&nbsp;" & Value
        TCellDescription.Text = "&nbsp;" & Description

        TRow.Controls.Add(TCellName)
        TRow.Controls.Add(TCellValue)
        TRow.Controls.Add(TCellDescription)

        TableProject.Controls.Add(TRow)
    End Sub

    Public Function GetProperty(ByVal Name As String) As String
        Dim vQuery As String
        Dim pRecordSet As New ADODB.Recordset
        Dim pConnection As ADODB.Connection = Nothing
        Dim vReturn As String = ""

        Try
            pConnection = ConnectDB()

            vQuery = "SELECT Value FROM tblCFG_Property where Name = '" & Name & "' "
            If (OpenTable(pConnection, pRecordSet, vQuery) > 0) Then
                vReturn = GetField(pRecordSet, "Value")
            End If

        Catch ex As Exception

        Finally
            If pRecordSet.State > 0 Then
                pRecordSet.Close()
            End If

            CloseDB(pConnection)

        End Try

        GetProperty = vReturn
    End Function

    Public Function MuxString(ByVal str1 As String, ByVal str2 As String) As String
        Try
            MuxString = CDbl(str1) * CDbl(str2)
        Catch ex As Exception
            MuxString = "0"
        End Try
    End Function

    Public Function checkCitizenID(ByVal Id As String) As Boolean
        If (Id.Length <> 13) Or (Not IsNumeric(Id)) Then
            checkCitizenID = False
        Else
            checkCitizenID = True
            'Dim modVal As String
            'Dim sum As Integer

            'Dim i As Integer

            'For i = 1 To Id.Length - 1
            '    sum += CInt(Id.Substring(i, 1)) * (i + 1)
            'Next i

            'modVal = (sum Mod 11).ToString

            'If modVal.Substring(0, 1) = Id.Substring(0, 1) Then
            '    checkCitizenID = True
            'Else
            '    checkCitizenID = False
            'End If

        End If

    End Function

    Public Function numeric(ByVal Id As String, ByVal length As Integer) As Boolean
        Dim i As Integer

        numeric = True
        For i = 0 To length - 1
            If IsNumeric(Id.Substring(i, 1)) = False Then
                numeric = False
            End If
        Next i

    End Function

    Public Sub TextBoxDisable(ByRef txt As TextBox, ByVal value As Boolean)
        If value = True Then
            txt.ReadOnly = True
            txt.BackColor = Drawing.Color.Silver
        Else
            txt.ReadOnly = False
            txt.BackColor = Drawing.Color.White
        End If

    End Sub

    Public Function GetTodayADDate(Optional ByVal add As Integer = 0) As String

        Dim syear As Integer
        Dim today As Date
        today = Now.AddDays(add)
        If today.Year < 2100 Then
            syear = today.Year + 543
        Else
            syear = today.Year
        End If
        Return today.Day & "/" & today.Month & "/" & syear

    End Function

    Public Function Str2SQL(ByVal str As String) As String
        Str2SQL = str.Replace("'", "''")
    End Function

    Public Function HtmlErrorText(ByVal str As String) As String
        HtmlErrorText = "<b><font color='red'>" & str & "</font></b>"
    End Function

    Public Function CellIsTrue(ByVal str As String) As Boolean
        If str.ToUpper = "True".ToUpper Then
            Return True
        Else
            Return False
        End If

    End Function

    Public Function CellIsTrueDB(ByVal str As String) As Integer
        If CellIsTrue(str) Then
            Return 1
        Else
            Return 0
        End If

    End Function

    Public Function GetAxaptaProjectIDWithPlanID(ByVal AxaptaProjectName As String, ByVal PlanID As String) As String
        Dim vQuery As String
        Dim pRecordSet As New ADODB.Recordset
        Dim pConnection As ADODB.Connection = Nothing
        Dim vReturn As String = ""

        Try
            pConnection = ConnectDB()

            vQuery = "SELECT AxaptaProjectID " & _
                     " FROM vwMT_AxaptaProject_WithPlanID " & _
                     " WHERE AxaptaProjectName = '" & AxaptaProjectName & "' " & _
                     "   AND PlanID = '" & PlanID & "' "

            If (OpenTable(pConnection, pRecordSet, vQuery) > 0) Then
                vReturn = GetField(pRecordSet, "AxaptaProjectID")
            End If

        Catch ex As Exception

        Finally
            If pRecordSet.State > 0 Then
                pRecordSet.Close()
            End If

            CloseDB(pConnection)

        End Try

        GetAxaptaProjectIDWithPlanID = vReturn
    End Function

    Public Function GetAxaptaProjectIDWithType(ByVal AxaptaProjectName As String, ByVal Type As String) As String
        Dim vQuery As String
        Dim pRecordSet As New ADODB.Recordset
        Dim pConnection As ADODB.Connection = Nothing
        Dim vReturn As String = ""

        Try
            pConnection = ConnectDB()

            vQuery = "SELECT AxaptaProjectID " & _
                     " FROM vwMT_AxaptaProject_WithPlanID " & _
                     " WHERE AxaptaProjectName = '" & AxaptaProjectName & "' "

            If Type = "1" Then      ' งานประจำ
                vQuery = vQuery & " AND PlanID in ('1','2','3','4','5') "
            Else                    ' งานพัฒนา
                vQuery = vQuery & " AND PlanID = '6' "
            End If

            If (OpenTable(pConnection, pRecordSet, vQuery) > 0) Then
                vReturn = GetField(pRecordSet, "AxaptaProjectID")
            End If

        Catch ex As Exception

        Finally
            If pRecordSet.State > 0 Then
                pRecordSet.Close()
            End If

            CloseDB(pConnection)

        End Try

        GetAxaptaProjectIDWithType = vReturn
    End Function

    Public Function GetAxaptaProjectName(ByVal AxaptaProjectID As String) As String
        Dim vQuery As String
        Dim pRecordSet As New ADODB.Recordset
        Dim pConnection As ADODB.Connection = Nothing
        Dim vReturn As String = ""

        Try
            pConnection = ConnectDB()

            vQuery = "SELECT AxaptaProjectName FROM vwMT_AxaptaProject where AxaptaProjectID = '" & AxaptaProjectID & "' "
            If (OpenTable(pConnection, pRecordSet, vQuery) > 0) Then
                vReturn = GetField(pRecordSet, "AxaptaProjectName")
            End If

        Catch ex As Exception

        Finally
            If pRecordSet.State > 0 Then
                pRecordSet.Close()
            End If

            CloseDB(pConnection)

        End Try

        GetAxaptaProjectName = vReturn
    End Function

    Private Function CopyFile(ByVal source As String, ByVal dest As String) As String
        Dim result As String = "Copied file"
        Try
            ' Overwrites existing files
            File.Copy(source, dest, True)
        Catch ex As Exception
            result = ex.Message
        End Try
        Return result
    End Function

    Public Function XTrim(ByVal str As String) As String
        Dim ret As String = str.Replace(Chr(11), "")

        Return ret
    End Function

End Module



