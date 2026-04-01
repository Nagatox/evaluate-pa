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
Imports ExcelHelper

Partial Public Class frmEvaluateService
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim cmd As String = Request.QueryString("q")

        Select Case cmd
            Case "LoadEvaluateMain"
                LoadEvaluateMain()

            Case "LoadEvaluateHeader"
                LoadEvaluateHeader()

            Case "LoadEvaluateDetail"
                LoadEvaluateDetail()
            Case "SaveEvaluate"
                SaveEvaluate(False)
            Case "SaveConfirmEvaluate"
                SaveEvaluate(True)

            Case "LoadSummaryData"
                LoadSummaryData()

            Case "LoadSummaryDatabyLoginName"
                LoadSummaryDatabyLoginName()

            Case "LoadSummaryDataPAbyLoginName"
                LoadSummaryDataPAbyLoginName()

            Case "ExportEvaluateForm"
                ExportEvaluateForm()

            Case "LoadPart0202"
                LoadPart0202()
            Case "SavePart0202"
                SavePart0202()

            Case "LoadPart03"
                LoadPart03()
            Case "SavePart03"
                SavePart03()

            Case "LoadPart04"
                LoadPart04()
            Case "SavePart04"
                SavePart04()

            Case "SaveConfirm"
                SaveConfirm()
            Case Else

        End Select

        Response.End()

    End Sub

    Protected Class _tblEvaluateMain_
        Public id As String
        Public username As String
        Public evaluate_year As String
        Public round As String

        Public department_name As String

        Public position As String
        Public level As String

        Public start_work_date As String
        Public leave_study_status As String
        Public leave_study_start_date As String
        Public leave_study_end_date As String

        Public disciplinary As String
        Public disciplinary_date As String

        Public promoted As String
        Public promoted_percent As String

        Public leave_sick As String
        Public leave_sick_guarantee As String

        Public leave_carer As String
        Public leave_vacation As String
        Public leave_maternity As String
        Public leave_ordination As String
        Public late As String
        Public lack_agenturer As String
        Public leave_sum As String

        Public part01status As String
        Public part0201status As String
        Public part0201pastatus As String
        Public part0202status As String
        Public part03status As String
        Public part04status As String

        Public confirmed As String
        Public confirm_datetime As String

        Public revision As String
        Public record_datetime As String
        Public editor As String

        Public employee_id As String
        Public firstname As String
        Public lastname As String

        Public recordCount As Integer
    End Class

    Protected Sub LoadEvaluateMain()

        Dim username = Request.QueryString("username").Trim
        Dim evaluate_year = Request.QueryString("evaluate_year").Trim
        Dim round = Request.QueryString("round").Trim
        Dim revision = Request.QueryString("revision").Trim

        Dim objJSONHeader As New JSONHeader
        objJSONHeader.id = "tblEvaluateMain"
        objJSONHeader.isError = False
        objJSONHeader.ErrMessage = ""

        Dim SQL As String
        Dim pRecordSet As New ADODB.Recordset
        Dim pConnection As ADODB.Connection = Nothing
        Dim iRecordCount As Integer

        Try
            pConnection = ConnectDB()

            SQL = "SELECT U.employee_id, U.firstname, U.lastname, U.position, U.level " & _
                  " , U.department_id, D.department_name " & _
                  " , UE.start_work_date, UE.leave_study_status, UE.leave_study_start_date, UE.leave_study_end_date " & _
                  " , UE.disciplinary, UE.disciplinary_date " & _
                  " , UE.promoted, UE.promoted_percent " & _
                  " , UE.leave_sick, UE.leave_sick_guarantee, UE.leave_carer, UE.leave_vacation " & _
                  " , UE.leave_maternity, UE.leave_ordination, UE.late, UE.lack_agenturer " & _
                  " , (ISNULL(UE.leave_sick,0) + ISNULL(UE.leave_sick_guarantee,0) + ISNULL(UE.leave_carer,0) + ISNULL(UE.leave_vacation,0) " & _
                  "  + ISNULL(UE.leave_maternity,0) + ISNULL(UE.leave_ordination,0) + (ISNULL(UE.late,0)*0.5) + ISNULL(UE.lack_agenturer,0)) as leave_sum " & _
                  " , '0' part01status, '0' part04status " & _
                  " , (SELECT COUNT(*)  FROM tblEvaluateHeader WHERE username = U.username AND evaluate_year = UE.evaluate_year AND round = '" & round & "' AND revision = '0') as part0201status " & _
                  " , (SELECT COUNT(*)  FROM tblPAHeader WHERE username = U.username AND evaluate_year = UE.evaluate_year AND round = '" & round & "') as part0201pastatus " & _
                  " , (SELECT COUNT(*)  FROM tblPart0202 WHERE username = U.username AND evaluate_year = UE.evaluate_year AND round = '" & round & "') as part0202status " & _
                  " , (SELECT COUNT(*)  FROM tblPart03 WHERE username = U.username AND evaluate_year = UE.evaluate_year AND round = '" & round & "') as part03status " & _
                  " , EH.confirmed, EH.confirm_datetime " & _
                  " FROM ((tblUser U LEFT JOIN tblUserExt UE ON (U.employee_id = UE.employee_id AND UE.evaluate_year = '" & evaluate_year & "') " & _
                  "    LEFT JOIN tblMT_Department D ON (U.department_id = D.department_id AND U.faculty_id = D.faculty_id AND U.campus_id = D.campus_id)) " & _
                  "    LEFT JOIN tblPAHeader EH ON (U.username = EH.username AND EH.evaluate_year = '" & evaluate_year & "' AND EH.round = '" & round & "' )) " & _
                  " WHERE U.username = '" & username & "' " & _
                  "    " & _
                  ""

            'SQL = SQL & "   AND round = '" & round & "' " & _

            'SQL = SQL & " AND revision = 0 "

            iRecordCount = OpenTable(pConnection, pRecordSet, SQL)
            objJSONHeader.recordCount = iRecordCount
            iRecordCount = 1

            ReDim objJSONHeader.detail(iRecordCount - 1)

            Dim i As Integer = 0
            'While Not pRecordSet.EOF
            Dim obj As New _tblEvaluateMain_
            If Not pRecordSet.EOF Then
                obj.id = i + 1
                obj.username = evaluate_year
                obj.evaluate_year = evaluate_year
                obj.round = round

                obj.employee_id = GetField(pRecordSet, "employee_id")
                obj.firstname = GetField(pRecordSet, "firstname")
                obj.lastname = GetField(pRecordSet, "lastname")

                obj.department_name = GetField(pRecordSet, "department_name")

                obj.position = GetField(pRecordSet, "position")
                obj.level = GetField(pRecordSet, "level")

                obj.start_work_date = GetField(pRecordSet, "start_work_date")
                obj.leave_study_status = GetField(pRecordSet, "leave_study_status")
                obj.leave_study_start_date = GetField(pRecordSet, "leave_study_start_date")
                obj.leave_study_end_date = GetField(pRecordSet, "leave_study_end_date")

                obj.disciplinary = GetField(pRecordSet, "disciplinary")
                obj.disciplinary_date = GetField(pRecordSet, "disciplinary_date")

                obj.promoted = GetField(pRecordSet, "promoted")
                obj.promoted_percent = GetField(pRecordSet, "promoted_percent")

                obj.leave_sick = GetField(pRecordSet, "leave_sick")
                obj.leave_sick_guarantee = GetField(pRecordSet, "leave_sick_guarantee")

                obj.leave_carer = GetField(pRecordSet, "leave_carer")
                obj.leave_vacation = GetField(pRecordSet, "leave_vacation")
                obj.leave_maternity = GetField(pRecordSet, "leave_maternity")
                obj.leave_ordination = GetField(pRecordSet, "leave_ordination")
                obj.late = GetField(pRecordSet, "late")
                obj.lack_agenturer = GetField(pRecordSet, "lack_agenturer")

                obj.leave_sum = GetField(pRecordSet, "leave_sum")

                obj.part01status = GetField(pRecordSet, "part01status")
                obj.part0201status = GetField(pRecordSet, "part0201status")
                obj.part0201pastatus = GetField(pRecordSet, "part0201pastatus")
                obj.part0202status = GetField(pRecordSet, "part0202status")
                obj.part03status = GetField(pRecordSet, "part03status")
                obj.part04status = GetField(pRecordSet, "part04status")

                obj.confirmed = GetField(pRecordSet, "confirmed")
                obj.confirm_datetime = GetField(pRecordSet, "confirm_datetime")

                objJSONHeader.detail(i) = obj
            Else
                obj.id = i + 1
                obj.username = evaluate_year
                obj.evaluate_year = evaluate_year
                obj.round = round


                objJSONHeader.detail(i) = obj
            End If

            'pRecordSet.MoveNext()
            'i = i + 1
            'End While

        Catch tex As ThreadAbortException

        Catch ex As Exception
            objJSONHeader.isError = True
            objJSONHeader.ErrMessage = ex.Message
        Finally
            If pRecordSet.State > 0 Then
                pRecordSet.Close()
            End If

            CloseDB(pConnection)

            Dim jobjsend As JavaScriptSerializer = New JavaScriptSerializer()
            Response.Write(jobjsend.Serialize(objJSONHeader))
            Response.Flush()
            Response.End()
        End Try

    End Sub

    Protected Class _tblEvaluateHeader_
        Public id As String
        Public username As String
        Public evaluate_year As String
        Public round As String

        Public department_name As String

        Public commander_recomment As String

        Public SumWeight As String
        Public SumPL As String
        Public SumSuccess As String
        Public FullSuccess As String
        Public PercentSuccess As String
        Public IFFull As String
        Public IFSuccess As String

        Public sum_excepted_PL1 As String
        Public sum_excepted_PL2 As String
        Public sum_excepted_PL3 As String
        Public sum_excepted_PL4 As String
        Public sum_excepted_PL5 As String

        Public sum_evaluate_PL1 As String
        Public sum_evaluate_PL2 As String
        Public sum_evaluate_PL3 As String
        Public sum_evaluate_PL4 As String
        Public sum_evaluate_PL5 As String

        Public revision As String
        Public record_datetime As String
        Public editor As String

        Public employee_id As String
        Public firstname As String
        Public lastname As String

        Public confirmed As String
        Public confirm_datetime As String

        Public recordCount As Integer
        Public detail As _tblEvaluateDetail_()
    End Class

    Protected Class _tblPAHeader_
        Public id As String
        Public username As String
        Public evaluate_year As String
        Public round As String

        Public department_name As String

        Public suggest As String

        Public sum_score As String
        Public sum_percent As String

        Public sum_score_100 As String
        Public total_score As String
        Public force_mean As String
        Public result As String

        Public revision As String
        Public record_datetime As String
        Public editor As String

        Public employee_id As String
        Public firstname As String
        Public lastname As String

        Public confirmed As String
        Public confirm_datetime As String

        Public result_confirmed As String
        Public result_confirm_datetime As String

        Public boss_confirmed As String
        Public boss_confirm_datetime As String

        Public boss_result_confirmed As String
        Public boss_result_confirm_datetime As String

        Public recordCount As Integer
        Public detail As _tblEvaluateDetail_()
    End Class

    Protected Sub LoadEvaluateHeader()

        Dim username = Request.QueryString("username").Trim
        Dim evaluate_year = Request.QueryString("evaluate_year").Trim
        Dim round = Request.QueryString("round").Trim
        Dim revision = Request.QueryString("revision").Trim

        Dim objJSONHeader As New JSONHeader
        objJSONHeader.id = "tblEvaluateHeader"
        objJSONHeader.isError = False
        objJSONHeader.ErrMessage = ""

        Dim SQL As String
        Dim pRecordSet As New ADODB.Recordset
        Dim pConnection As ADODB.Connection = Nothing
        Dim iRecordCount As Integer

        Try
            pConnection = ConnectDB()

            SQL = "SELECT commander_recomment, confirmed, confirm_datetime " & _
                  " , sum_excepted_PL1, sum_excepted_PL2, sum_excepted_PL3, sum_excepted_PL4, sum_excepted_PL5 " & _
                  " , sum_evaluate_PL1, sum_evaluate_PL2, sum_evaluate_PL3, sum_evaluate_PL4, sum_evaluate_PL5 " & _
                  " , SumWeight, SumPL, SumSuccess, FullSuccess, PercentSuccess, IFFull, IFSuccess " & _
                  " , record_datetime " & _
                  " FROM tblEvaluateHeader EH " & _
                  " WHERE username = '" & username & "' " & _
                  "   AND evaluate_year = '" & evaluate_year & "' " & _
                  "   AND round = '" & round & "' " & _
                  ""
            If revision = "0" Then
                SQL = SQL & " AND revision = '" & revision & "' "
            Else
                SQL = SQL & " AND revision = (" & _
                                  " SELECT MAX(revision) " & _
                                  " FROM tblEvaluateHeader " & _
                                  " WHERE username = '" & username & "' " & _
                                  "   AND evaluate_year = '" & evaluate_year & "' " & _
                                  "   AND round = '" & round & "' " & _
                                  ")"
            End If
            'SQL = SQL & " AND revision = 0 "

            iRecordCount = OpenTable(pConnection, pRecordSet, SQL)
            objJSONHeader.recordCount = iRecordCount
            iRecordCount = 1

            ReDim objJSONHeader.detail(iRecordCount - 1)

            Dim i As Integer = 0
            'While Not pRecordSet.EOF
            Dim objEvaluateHeader As New _tblEvaluateHeader_
            If Not pRecordSet.EOF Then
                objEvaluateHeader.id = i + 1
                objEvaluateHeader.username = evaluate_year
                objEvaluateHeader.evaluate_year = evaluate_year
                objEvaluateHeader.round = round

                objEvaluateHeader.SumWeight = GetField(pRecordSet, "SumWeight")
                objEvaluateHeader.SumPL = GetField(pRecordSet, "SumPL")
                objEvaluateHeader.SumSuccess = GetField(pRecordSet, "SumSuccess")
                objEvaluateHeader.FullSuccess = GetField(pRecordSet, "FullSuccess")
                objEvaluateHeader.PercentSuccess = GetField(pRecordSet, "PercentSuccess")
                objEvaluateHeader.IFFull = GetField(pRecordSet, "IFFull")
                objEvaluateHeader.IFSuccess = GetField(pRecordSet, "IFSuccess")

                objEvaluateHeader.sum_excepted_PL1 = GetField(pRecordSet, "sum_excepted_PL1")
                objEvaluateHeader.sum_excepted_PL2 = GetField(pRecordSet, "sum_excepted_PL2")
                objEvaluateHeader.sum_excepted_PL3 = GetField(pRecordSet, "sum_excepted_PL3")
                objEvaluateHeader.sum_excepted_PL4 = GetField(pRecordSet, "sum_excepted_PL4")
                objEvaluateHeader.sum_excepted_PL5 = GetField(pRecordSet, "sum_excepted_PL5")

                objEvaluateHeader.sum_evaluate_PL1 = GetField(pRecordSet, "sum_evaluate_PL1")
                objEvaluateHeader.sum_evaluate_PL2 = GetField(pRecordSet, "sum_evaluate_PL2")
                objEvaluateHeader.sum_evaluate_PL3 = GetField(pRecordSet, "sum_evaluate_PL3")
                objEvaluateHeader.sum_evaluate_PL4 = GetField(pRecordSet, "sum_evaluate_PL4")
                objEvaluateHeader.sum_evaluate_PL5 = GetField(pRecordSet, "sum_evaluate_PL5")

                'objEvaluateHeader.commander_recomment = GetField(pRecordSet, "commander_recomment")
                objEvaluateHeader.confirmed = GetField(pRecordSet, "confirmed")
                objEvaluateHeader.confirm_datetime = GetField(pRecordSet, "confirm_datetime")

                objEvaluateHeader.commander_recomment = GetCommentRecomment(pConnection, username, evaluate_year, round, revision)

                objEvaluateHeader.record_datetime = GetField(pRecordSet, "record_datetime")

                objJSONHeader.detail(i) = objEvaluateHeader
            Else
                objEvaluateHeader.id = i + 1
                objEvaluateHeader.username = evaluate_year
                objEvaluateHeader.evaluate_year = evaluate_year
                objEvaluateHeader.round = round

                objEvaluateHeader.commander_recomment = ""
                objEvaluateHeader.confirmed = "0"
                objEvaluateHeader.confirm_datetime = ""

                objJSONHeader.detail(i) = objEvaluateHeader
            End If

            'pRecordSet.MoveNext()
            'i = i + 1
            'End While

        Catch tex As ThreadAbortException

        Catch ex As Exception
            objJSONHeader.isError = True
            objJSONHeader.ErrMessage = ex.Message
        Finally
            If pRecordSet.State > 0 Then
                pRecordSet.Close()
            End If

            CloseDB(pConnection)

            Dim jobjsend As JavaScriptSerializer = New JavaScriptSerializer()
            Response.Write(jobjsend.Serialize(objJSONHeader))
            Response.Flush()
            Response.End()
        End Try

    End Sub

    Function GetCommentRecomment(ByRef tConnection As ADODB.Connection, ByVal username As String, ByVal evaluate_year As String, ByVal round As String, ByVal revision As String) As String
        Dim pRecordSet As New ADODB.Recordset
        'Dim pConnection As ADODB.Connection = Nothing

        Dim vReturnValue As String = ""
        Dim SQL As String

        Try
            'pConnection = ConnectDB()

            SQL = "SELECT commander_recomment " & _
                  " FROM tblEvaluateHeader EH " & _
                  " WHERE username = '" & username & "' " & _
                  "   AND evaluate_year = '" & evaluate_year & "' " & _
                  "   AND round = '" & round & "' " & _
                  ""
            If revision = "0" Then
                SQL = SQL & " AND revision = '" & revision & "' "
            Else
                SQL = SQL & " AND revision = (" & _
                                  " SELECT MAX(revision) " & _
                                  " FROM tblEvaluateHeader " & _
                                  " WHERE username = '" & username & "' " & _
                                  "   AND evaluate_year = '" & evaluate_year & "' " & _
                                  "   AND round = '" & round & "' " & _
                                  ")"
            End If

            If OpenTable(tConnection, pRecordSet, SQL) > 0 Then
                vReturnValue = GetField(pRecordSet, "commander_recomment")
            End If
        Catch ex As Exception
            Throw New ApplicationException(ex.Message)
        Finally
            If pRecordSet.State > 0 Then
                pRecordSet.Close()
            End If

            'CloseDB(pConnection)
        End Try

        GetCommentRecomment = vReturnValue
    End Function

    Function GetComfirmed(ByRef tConnection As ADODB.Connection, ByRef ConfirmDateTime As String, ByVal username As String, ByVal evaluate_year As String, ByVal round As String, ByVal revision As String) As String
        Dim pRecordSet As New ADODB.Recordset
        'Dim pConnection As ADODB.Connection = Nothing

        Dim vReturnValue As String = ""
        Dim SQL As String

        Try
            'pConnection = ConnectDB()

            SQL = "SELECT commander_recomment, confirmed, confirm_datetime " & _
                  " FROM tblEvaluateHeader EH " & _
                  " WHERE username = '" & username & "' " & _
                  "   AND evaluate_year = '" & evaluate_year & "' " & _
                  "   AND round = '" & round & "' " & _
                  ""
            'If revision = "0" Then
            '    SQL = SQL & " AND revision = '" & revision & "' "
            'Else
            '    SQL = SQL & " AND revision = (" & _
            '                      " SELECT MAX(revision) " & _
            '                      " FROM tblEvaluateHeader " & _
            '                      " WHERE username = '" & username & "' " & _
            '                      "   AND evaluate_year = '" & evaluate_year & "' " & _
            '                      "   AND round = '" & round & "' " & _
            '                      ")"
            'End If
            SQL = SQL & " AND revision = '0' "

            If OpenTable(tConnection, pRecordSet, SQL) > 0 Then
                vReturnValue = GetField(pRecordSet, "confirmed")
                ConfirmDateTime = GetField(pRecordSet, "confirm_datetime")
            End If
        Catch ex As Exception
            Throw New ApplicationException(ex.Message)
        Finally
            If pRecordSet.State > 0 Then
                pRecordSet.Close()
            End If

            'CloseDB(pConnection)
        End Try

        GetComfirmed = vReturnValue
    End Function

    Protected Class _tblEvaluateDetail_
        Public id As String
        Public username As String
        Public evaluate_year As String
        Public round As String

        Public activate_no As String
        Public activate_name As String
        Public activate_detail As String
        Public weight As String
        Public excepted_value As String
        Public evaluate_value As String
        Public pl_score As String
        Public success_level As String
        Public success_score As String
        Public total_score As String

        Public revision As String
        Public record_datetime As String
        Public editor As String

    End Class

    Protected Function LoadEvaluatePAToOpenXML(ByRef Document As SpreadsheetDocument, ByVal SheetName As String, ByVal rowStart As Integer, ByVal username As String, ByVal round As String, ByVal evaluate_year As String, ByVal revision As String, ByVal item_group_no As Integer) As Integer

        Dim SQL As String
        Dim pRecordSet As New ADODB.Recordset
        Dim pConnection As ADODB.Connection = Nothing
        Dim iRecordCount As Integer
        Const MAXROW = 30

        Try
            pConnection = ConnectDB()

            SQL = "SELECT D.username, D.year " & _
                  "   , D.item_group_no, D.item_no " & _
                  "   , D.detail, D.weight " & _
                  "   , DR.LOS, DR.LOSBoss, DR.comment2 " & _
                  "   , D.record_datetime, D.editor " & _
                  " FROM tblPADetail D LEFT JOIN  tblPADetailResult DR " & _
                  "   ON D.username = DR.username " & _
                  "   AND D.year = DR.year " & _
                  "   AND D.item_group_no = DR.item_group_no " & _
                  "   AND D.item_no = DR.item_no " & _
                  " WHERE D.username = '" & username & "' " & _
                  "   AND D.year = '" & evaluate_year & "' " & _
                  "   AND D.item_group_no = '" & item_group_no & "' " & _
                  " ORDER BY D.item_group_no DESC, D.item_no "

            iRecordCount = OpenTable(pConnection, pRecordSet, SQL)
            'InsertRow(Document, SheetName, rowStart, iRecordCount)
            HiddenRow(Document, SheetName, rowStart + iRecordCount, MAXROW - iRecordCount)

            Dim wbPart As WorkbookPart = Document.WorkbookPart

            Dim i As Integer = 0

            i = 0
            While Not pRecordSet.EOF
                Dim item_no As String = GetField(pRecordSet, "item_no")
                Dim detail As String = GetField(pRecordSet, "detail")
                UpdateValue(wbPart, SheetName, "A" & (rowStart + i).ToString, item_group_no & "." & item_no, 0, True)
                detail = XTrim(detail)
                UpdateValue(wbPart, SheetName, "B" & (rowStart + i).ToString, detail, 0, True)
                Dim weight As String = GetField(pRecordSet, "weight").Trim
                UpdateValue(wbPart, SheetName, "C" & (rowStart + i).ToString, weight, 0, True)

                Dim LOS As String = GetField(pRecordSet, "LOS").Trim
                Dim LOSBoss As String = GetField(pRecordSet, "LOSBoss").Trim
                If LOSBoss <> "" Then
                    LOS = LOSBoss
                End If
                UpdateValue(wbPart, SheetName, "D" & (rowStart + i).ToString, LOS, 0, True)
                'UpdateValue(wbPart, SheetName, "E" & (rowStart + i).ToString, (CDbl(LOS) * CDbl(weight)).ToString("0.00"), 0, True)
                'UpdateValue(wbPart, SheetName, "F" & (rowStart + i).ToString, ((CDbl(LOS) * CDbl(weight)) / 4).ToString("0.00"), 0, True)

                Dim comment2 As String = GetField(pRecordSet, "comment2").Trim
                UpdateValue(wbPart, SheetName, "G" & (rowStart + i).ToString, comment2, 0, True)

                pRecordSet.MoveNext()
                i = i + 1
            End While

        Catch ex As Exception
        Finally
            If pRecordSet.State > 0 Then
                pRecordSet.Close()
            End If

            CloseDB(pConnection)

        End Try

        Return iRecordCount
    End Function

    Protected Function LoadEvaluateDetailToOpenXML(ByRef Document As SpreadsheetDocument, ByVal SheetName As String, ByVal rowStart As Integer, ByVal username As String, ByVal round As String, ByVal evaluate_year As String, ByVal revision As String) As Integer

        Dim SQL As String
        Dim pRecordSet As New ADODB.Recordset
        Dim pConnection As ADODB.Connection = Nothing
        Dim iRecordCount As Integer

        Try
            pConnection = ConnectDB()

            SQL = "SELECT username, evaluate_year, round " & _
                  "   , activate_no, activate_name, activate_detail " & _
                  "   , weight, excepted_value, evaluate_value " & _
                  "   , pl_score, success_level, success_score, total_score " & _
                  "   , revision, record_datetime, editor " & _
                  " FROM tblEvaluateDetail ED " & _
                  " WHERE username = '" & username & "' " & _
                  "   AND evaluate_year = '" & evaluate_year & "' " & _
                  "   AND round = '" & round & "' " & _
                  ""

            If revision = "0" Then
                SQL = SQL & " AND revision = '" & revision & "' "
            Else
                SQL = SQL & " AND revision = (" & _
                                  " SELECT MAX(revision) " & _
                                  " FROM tblEvaluateHeader " & _
                                  " WHERE username = '" & username & "' " & _
                                  "   AND evaluate_year = '" & evaluate_year & "' " & _
                                  "   AND round = '" & round & "' " & _
                                  ")"
            End If

            '"   AND revision = '" & revision & "' " & _
            SQL = SQL & " ORDER BY activate_no "

            iRecordCount = OpenTable(pConnection, pRecordSet, SQL)
            InsertRow(Document, SheetName, rowStart, iRecordCount)

            Dim wbPart As WorkbookPart = Document.WorkbookPart

            Dim fSumWeight As Double = 0
            Dim fSumPlScore As Double = 0
            Dim fSumTotalScore As Double = 0

            Dim fExPL(5) As Double
            Dim fEvPL(5) As Double

            Dim i As Integer = 0

            For i = 0 To 4
                fExPL(i) = 0
                fEvPL(i) = 0
            Next
            i = 0
            While Not pRecordSet.EOF
                Dim activate_name As String = GetField(pRecordSet, "activate_name")
                activate_name = XTrim(activate_name)
                UpdateValue(wbPart, SheetName, "B" & (rowStart + i).ToString, activate_name, 0, True)
                Dim weight As String = GetField(pRecordSet, "weight").Trim
                If weight <> "" Then
                    If weight <> "0" Then
                        Dim fWeight = CDbl(weight)
                        fSumWeight = fSumWeight + fWeight
                        UpdateValue(wbPart, SheetName, "C" & (rowStart + i).ToString, weight, 0, True)
                        Dim iExceptedValue As Integer = CInt(GetField(pRecordSet, "excepted_value"))
                        UpdateValue(wbPart, SheetName, "D" & (rowStart + i).ToString, iExceptedValue.ToString, 0, True)
                        Dim iEvaluateValue As Integer = CInt(GetField(pRecordSet, "evaluate_value"))
                        UpdateValue(wbPart, SheetName, Chr(Asc("E") + iEvaluateValue - 1) & (rowStart + i).ToString, "X", 0, True)
                        Dim fPLScore As Double = (fWeight / iExceptedValue) * iEvaluateValue
                        'fSumPlScore = fSumPlScore + CDbl(GetField(pRecordSet, "pl_score"))
                        fSumPlScore = fSumPlScore + fPLScore
                        'UpdateValue(wbPart, SheetName, "J" & (rowStart + i).ToString, GetField(pRecordSet, "pl_score"), 0, True)
                        UpdateValue(wbPart, SheetName, "J" & (rowStart + i).ToString, fPLScore.ToString("0.00"), 0, True)
                        Dim iSuccessLevel As Integer = CInt(GetField(pRecordSet, "success_level"))
                        'iSuccessLevel = 0.5 * (iSuccessLevel + 1)
                        UpdateValue(wbPart, SheetName, Chr(Asc("K") + iSuccessLevel - 1) & (rowStart + i).ToString, "X", 0, True)

                        Dim fSuccessScore As Double = CDbl(GetField(pRecordSet, "success_score"))

                        'Dim fTotalScore As Double = CDbl(GetField(pRecordSet, "total_score"))
                        Dim fTotalScore As Double = fPLScore * fSuccessScore
                        fSumTotalScore = fSumTotalScore + fTotalScore

                        UpdateValue(wbPart, SheetName, "R" & (rowStart + i).ToString, fTotalScore.ToString("0.00"), 0, True)

                        If (iExceptedValue > 0) Then
                            fExPL(iExceptedValue - 1) = fExPL(iExceptedValue - 1) + fWeight
                        End If
                        If (iEvaluateValue > 0) Then
                            fEvPL(iEvaluateValue - 1) = fEvPL(iEvaluateValue - 1) + fWeight
                        End If
                    End If
                End If

                pRecordSet.MoveNext()
                i = i + 1
            End While

            UpdateValue(wbPart, SheetName, "C" & (rowStart + iRecordCount + 1).ToString, fSumWeight.ToString("0.00"), 0, True)
            UpdateValue(wbPart, SheetName, "J" & (rowStart + iRecordCount + 1).ToString, fSumPlScore.ToString("0.0"), 0, True)
            UpdateValue(wbPart, SheetName, "R" & (rowStart + iRecordCount + 1).ToString, fSumTotalScore.ToString("0.0"), 0, True)
            UpdateValue(wbPart, SheetName, "R" & (rowStart + iRecordCount + 2).ToString, (fSumPlScore * 4).ToString("0.0"), 0, True)
            UpdateValue(wbPart, SheetName, "R" & (rowStart + iRecordCount + 3).ToString, (fSumTotalScore / (fSumPlScore * 4) * 100).ToString("0.00"), 0, True)
            UpdateValue(wbPart, SheetName, "R" & (rowStart + iRecordCount + 4).ToString, ((fSumTotalScore / (fSumPlScore * 4) * 100) * 70 / 100).ToString("0.00"), 0, True)

            For i = 0 To 4
                UpdateValue(wbPart, SheetName, Chr(Asc("C") + i) & (rowStart + iRecordCount + 7).ToString, fExPL(i).ToString(), 0, True)
                UpdateValue(wbPart, SheetName, Chr(Asc("C") + i) & (rowStart + iRecordCount + 8).ToString, fEvPL(i).ToString(), 0, True)
            Next

        Catch ex As Exception
        Finally
            If pRecordSet.State > 0 Then
                pRecordSet.Close()
            End If

            CloseDB(pConnection)

        End Try

        Return iRecordCount
    End Function

    Protected Function LoadEvaluatePart0202ToOpenXML(ByRef Document As SpreadsheetDocument, ByVal SheetName As String, ByVal username As String, ByVal round As String, ByVal evaluate_year As String, ByVal revision As String, Optional ByVal levalmanager As String = "0") As Integer

        Dim SQL As String
        Dim pRecordSet As New ADODB.Recordset
        Dim pConnection As ADODB.Connection = Nothing
        Dim iRecordCount As Integer

        Try
            pConnection = ConnectDB()

            SQL = "SELECT * " & _
                  " FROM tblPart0202 ED " & _
                  " WHERE username = '" & username & "' " & _
                  "   AND evaluate_year = '" & evaluate_year & "' " & _
                  "   AND round = '" & round & "' " & _
                  ""

            iRecordCount = OpenTable(pConnection, pRecordSet, SQL)
            'InsertRow(Document, SheetName, rowStart, iRecordCount)

            Dim wbPart As WorkbookPart = Document.WorkbookPart

            While Not pRecordSet.EOF
                If (levalmanager <> "1" ) then
                    'UpdateValue(wbPart, SheetName, "X6", GetField(pRecordSet, "EmpScore01").ToString(), 0, True)
                    'UpdateValue(wbPart, SheetName, "X16", GetField(pRecordSet, "EmpScore02").ToString(), 0, True)
                    'UpdateValue(wbPart, SheetName, "X25", GetField(pRecordSet, "EmpScore03").ToString(), 0, True)
                    'UpdateValue(wbPart, SheetName, "X32", GetField(pRecordSet, "EmpScore04").ToString(), 0, True)
                    'UpdateValue(wbPart, SheetName, "X38", GetField(pRecordSet, "EmpScore05").ToString(), 0, True)
                    'UpdateValue(wbPart, SheetName, "X48", GetField(pRecordSet, "EmpScore06").ToString(), 0, True)
                    'UpdateValue(wbPart, SheetName, "X54", GetField(pRecordSet, "EmpScore07").ToString(), 0, True)
                    'UpdateValue(wbPart, SheetName, "X62", GetField(pRecordSet, "EmpScore08").ToString(), 0, True)

                    UpdateValue(wbPart, SheetName, "X6", GetField(pRecordSet, "Score01").ToString(), 0, True)
                    UpdateValue(wbPart, SheetName, "X16", GetField(pRecordSet, "Score02").ToString(), 0, True)
                    UpdateValue(wbPart, SheetName, "X25", GetField(pRecordSet, "Score03").ToString(), 0, True)
                    UpdateValue(wbPart, SheetName, "X32", GetField(pRecordSet, "Score04").ToString(), 0, True)
                    UpdateValue(wbPart, SheetName, "X38", GetField(pRecordSet, "Score05").ToString(), 0, True)
                    UpdateValue(wbPart, SheetName, "X48", GetField(pRecordSet, "Score06").ToString(), 0, True)
                    UpdateValue(wbPart, SheetName, "X54", GetField(pRecordSet, "Score07").ToString(), 0, True)
                    UpdateValue(wbPart, SheetName, "X62", GetField(pRecordSet, "Score08").ToString(), 0, True)

                    UpdateValue(wbPart, SheetName, "AA6", GetField(pRecordSet, "Suggestion01").ToString(), 0, True)
                    UpdateValue(wbPart, SheetName, "AA16", GetField(pRecordSet, "Suggestion02").ToString(), 0, True)
                    UpdateValue(wbPart, SheetName, "AA25", GetField(pRecordSet, "Suggestion03").ToString(), 0, True)
                    UpdateValue(wbPart, SheetName, "AA32", GetField(pRecordSet, "Suggestion04").ToString(), 0, True)
                    UpdateValue(wbPart, SheetName, "AA38", GetField(pRecordSet, "Suggestion05").ToString(), 0, True)
                    UpdateValue(wbPart, SheetName, "AA48", GetField(pRecordSet, "Suggestion06").ToString(), 0, True)
                    UpdateValue(wbPart, SheetName, "AA54", GetField(pRecordSet, "Suggestion07").ToString(), 0, True)
                    UpdateValue(wbPart, SheetName, "AA62", GetField(pRecordSet, "Suggestion08").ToString(), 0, True)
                Else
                    'UpdateValue(wbPart, SheetName, "X5", GetField(pRecordSet, "EmpScore01").ToString(), 0, True)
                    'UpdateValue(wbPart, SheetName, "X15", GetField(pRecordSet, "EmpScore02").ToString(), 0, True)
                    'UpdateValue(wbPart, SheetName, "X24", GetField(pRecordSet, "EmpScore03").ToString(), 0, True)
                    'UpdateValue(wbPart, SheetName, "X34", GetField(pRecordSet, "EmpScore04").ToString(), 0, True)
                    'UpdateValue(wbPart, SheetName, "X39", GetField(pRecordSet, "EmpScore05").ToString(), 0, True)
                    'UpdateValue(wbPart, SheetName, "X46", GetField(pRecordSet, "EmpScore06").ToString(), 0, True)
                    'UpdateValue(wbPart, SheetName, "X52", GetField(pRecordSet, "EmpScore07").ToString(), 0, True)
                    'UpdateValue(wbPart, SheetName, "X62", GetField(pRecordSet, "EmpScore08").ToString(), 0, True)
                    'UpdateValue(wbPart, SheetName, "X72", GetField(pRecordSet, "EmpScore09").ToString(), 0, True)
                    'UpdateValue(wbPart, SheetName, "X78", GetField(pRecordSet, "EmpScore10").ToString(), 0, True)
                    'UpdateValue(wbPart, SheetName, "X86", GetField(pRecordSet, "EmpScore11").ToString(), 0, True)

                    'UpdateValue(wbPart, SheetName, "AA5", GetField(pRecordSet, "Score01").ToString(), 0, True)
                    'UpdateValue(wbPart, SheetName, "AA15", GetField(pRecordSet, "Score02").ToString(), 0, True)
                    'UpdateValue(wbPart, SheetName, "AA24", GetField(pRecordSet, "Score03").ToString(), 0, True)
                    'UpdateValue(wbPart, SheetName, "AA34", GetField(pRecordSet, "Score04").ToString(), 0, True)
                    'UpdateValue(wbPart, SheetName, "AA39", GetField(pRecordSet, "Score05").ToString(), 0, True)
                    'UpdateValue(wbPart, SheetName, "AA46", GetField(pRecordSet, "Score06").ToString(), 0, True)
                    'UpdateValue(wbPart, SheetName, "AA52", GetField(pRecordSet, "Score07").ToString(), 0, True)
                    'UpdateValue(wbPart, SheetName, "AA62", GetField(pRecordSet, "Score08").ToString(), 0, True)
                    'UpdateValue(wbPart, SheetName, "AA72", GetField(pRecordSet, "Score09").ToString(), 0, True)
                    'UpdateValue(wbPart, SheetName, "AA78", GetField(pRecordSet, "Score10").ToString(), 0, True)
                    'UpdateValue(wbPart, SheetName, "AA86", GetField(pRecordSet, "Score11").ToString(), 0, True)

                    'UpdateValue(wbPart, SheetName, "AD5", GetField(pRecordSet, "Suggestion01").ToString(), 0, True)
                    'UpdateValue(wbPart, SheetName, "AD15", GetField(pRecordSet, "Suggestion02").ToString(), 0, True)
                    'UpdateValue(wbPart, SheetName, "AD24", GetField(pRecordSet, "Suggestion03").ToString(), 0, True)
                    'UpdateValue(wbPart, SheetName, "AD34", GetField(pRecordSet, "Suggestion04").ToString(), 0, True)
                    'UpdateValue(wbPart, SheetName, "AD39", GetField(pRecordSet, "Suggestion05").ToString(), 0, True)
                    'UpdateValue(wbPart, SheetName, "AD46", GetField(pRecordSet, "Suggestion06").ToString(), 0, True)
                    'UpdateValue(wbPart, SheetName, "AD52", GetField(pRecordSet, "Suggestion07").ToString(), 0, True)
                    'UpdateValue(wbPart, SheetName, "AD62", GetField(pRecordSet, "Suggestion08").ToString(), 0, True)
                    'UpdateValue(wbPart, SheetName, "AD72", GetField(pRecordSet, "Suggestion09").ToString(), 0, True)
                    'UpdateValue(wbPart, SheetName, "AD78", GetField(pRecordSet, "Suggestion10").ToString(), 0, True)
                    'UpdateValue(wbPart, SheetName, "AD86", GetField(pRecordSet, "Suggestion11").ToString(), 0, True)
                End If

                pRecordSet.MoveNext()
            End While


        Catch ex As Exception
        Finally
            If pRecordSet.State > 0 Then
                pRecordSet.Close()
            End If

            CloseDB(pConnection)

        End Try

        Return iRecordCount
    End Function


    Protected Function LoadEvaluatePart03ToOpenXML(ByRef Document As SpreadsheetDocument, ByVal SheetName As String, ByVal username As String, ByVal round As String, ByVal evaluate_year As String, ByVal revision As String) As Integer

        Dim SQL As String
        Dim pRecordSet As New ADODB.Recordset
        Dim pConnection As ADODB.Connection = Nothing
        Dim iRecordCount As Integer
        revision = 0
        Try
            pConnection = ConnectDB()

            SQL = "SELECT username, evaluate_year, round " & _
                  "   , type, no, line_item_no " & _
                  "   , detail " & _
                  "   , revision, record_datetime, editor " & _
                  " FROM tblPart03 P03 " & _
                  " WHERE username = '" & username & "' " & _
                  "   AND evaluate_year = '" & evaluate_year & "' " & _
                  "   AND round = '" & round & "' " & _
                  " AND revision = '" & revision & "' " & _
                  " AND RTRIM (detail) <> '' " & _
                  " ORDER BY type desc, no desc, line_item_no desc " & _
                  " "

            iRecordCount = OpenTable(pConnection, pRecordSet, SQL)

            Dim i As Integer = 0
            While Not pRecordSet.EOF
                Dim rowStart As Integer
                If GetField(pRecordSet, "type") = "1" Then
                    Select Case (Trim(GetField(pRecordSet, "no")))
                        Case "1"
                            rowStart = 5
                        Case "2"
                            rowStart = 7
                        Case "3"
                            rowStart = 9
                        Case "4"
                            rowStart = 11
                    End Select
                Else
                    Select Case (Trim(GetField(pRecordSet, "no")))
                        Case "1"
                            rowStart = 16
                        Case "2"
                            rowStart = 18
                    End Select
                End If

                InsertRow(Document, SheetName, rowStart, 1)

                Dim wbPart As WorkbookPart = Document.WorkbookPart
                UpdateValue(wbPart, SheetName, "C" & (rowStart).ToString, GetField(pRecordSet, "detail"), 0, True)

                pRecordSet.MoveNext()
                i = i + 1
            End While

        Catch ex As Exception
        Finally
            If pRecordSet.State > 0 Then
                pRecordSet.Close()
            End If

            CloseDB(pConnection)

        End Try

        Return iRecordCount
    End Function

    Protected Sub LoadEvaluateDetail()
        Dim username = Request.QueryString("username").Trim
        Dim evaluate_year = Request.QueryString("evaluate_year").Trim
        Dim round = Request.QueryString("round").Trim
        Dim revision = Request.QueryString("revision").Trim

        Dim objJSONHeader As New JSONHeader
        objJSONHeader.id = "tblEvaluateDetail"
        objJSONHeader.isError = False
        objJSONHeader.ErrMessage = ""

        Dim SQL As String
        Dim pRecordSet As New ADODB.Recordset
        Dim pConnection As ADODB.Connection = Nothing
        Dim iRecordCount As Integer

        Try
            pConnection = ConnectDB()

            SQL = "SELECT username, evaluate_year, round " & _
                  "   , activate_no, activate_name, activate_detail " & _
                  "   , weight, excepted_value, evaluate_value " & _
                  "   , pl_score, success_level, success_score, total_score " & _
                  "   , revision, record_datetime, editor " & _
                  " FROM tblEvaluateDetail ED " & _
                  " WHERE username = '" & username & "' " & _
                  "   AND evaluate_year = '" & evaluate_year & "' " & _
                  "   AND round = '" & round & "' " & _
                  " "
            If revision = "0" Then
                SQL = SQL & " AND revision = '" & revision & "' "
            Else
                SQL = SQL & " AND revision = (" & _
                                  " SELECT MAX(revision) " & _
                                  " FROM tblEvaluateDetail " & _
                                  " WHERE username = '" & username & "' " & _
                                  "   AND evaluate_year = '" & evaluate_year & "' " & _
                                  "   AND round = '" & round & "' " & _
                                  ")"
            End If

            SQL = SQL & _
                  " ORDER BY activate_no " & _
                  ""

            iRecordCount = OpenTable(pConnection, pRecordSet, SQL)
            objJSONHeader.recordCount = iRecordCount

            ReDim objJSONHeader.detail(iRecordCount - 1)

            Dim i As Integer = 0
            While Not pRecordSet.EOF
                Dim tblEvaluateDetail As New _tblEvaluateDetail_
                tblEvaluateDetail.id = i + 1
                tblEvaluateDetail.username = evaluate_year
                tblEvaluateDetail.evaluate_year = evaluate_year
                tblEvaluateDetail.round = round

                tblEvaluateDetail.activate_no = GetField(pRecordSet, "activate_no")
                tblEvaluateDetail.activate_name = GetField(pRecordSet, "activate_name")
                tblEvaluateDetail.activate_detail = GetField(pRecordSet, "activate_detail")
                tblEvaluateDetail.weight = GetField(pRecordSet, "weight")
                tblEvaluateDetail.excepted_value = GetField(pRecordSet, "excepted_value")
                tblEvaluateDetail.evaluate_value = GetField(pRecordSet, "evaluate_value")
                tblEvaluateDetail.pl_score = GetField(pRecordSet, "pl_score")
                tblEvaluateDetail.success_level = GetField(pRecordSet, "success_level")
                tblEvaluateDetail.success_score = GetField(pRecordSet, "success_score")
                tblEvaluateDetail.total_score = GetField(pRecordSet, "total_score")
                tblEvaluateDetail.revision = GetField(pRecordSet, "revision")
                tblEvaluateDetail.record_datetime = GetField(pRecordSet, "record_datetime")
                tblEvaluateDetail.editor = GetField(pRecordSet, "editor")

                objJSONHeader.detail(i) = tblEvaluateDetail
                pRecordSet.MoveNext()
                i = i + 1
            End While

        Catch tex As ThreadAbortException

        Catch ex As Exception
            objJSONHeader.isError = True
            objJSONHeader.ErrMessage = ex.Message
        Finally
            If pRecordSet.State > 0 Then
                pRecordSet.Close()
            End If

            CloseDB(pConnection)

            Dim jobjsend As JavaScriptSerializer = New JavaScriptSerializer()
            Response.Write(jobjsend.Serialize(objJSONHeader))
            Response.Flush()
            Response.End()
        End Try
    End Sub

    Protected Sub SaveEvaluate(Optional ByVal bConfirm As Boolean = False)
        Dim username As String = ""
        Dim evaluate_year As String = ""
        Dim round As String = ""
        Dim revision As String = ""

        Dim data As String = ""

        Dim objJSONHeader As New JSONHeader
        objJSONHeader.id = "tblEvaluate"
        objJSONHeader.isError = False
        objJSONHeader.ErrMessage = ""

        Dim objdata As _tblEvaluateHeader_ = Nothing

        Try
            username = Request.QueryString("username").Trim
            evaluate_year = Request.QueryString("evaluate_year").Trim
            round = Request.QueryString("round").Trim
            revision = Request.QueryString("revision").Trim

            'data = Request.Params("data").Trim
            Request.InputStream.Position = 0
            Dim inputStream As New StreamReader(Request.InputStream)
            data = inputStream.ReadToEnd()

            Dim jobj As JavaScriptSerializer = New JavaScriptSerializer()
            objdata = jobj.Deserialize(Of _tblEvaluateHeader_)(data)

        Catch ex As Exception
            objJSONHeader.isError = True
            objJSONHeader.ErrMessage = ex.Message

            Dim jobjsend As JavaScriptSerializer = New JavaScriptSerializer()
            Response.Write(jobjsend.Serialize(objJSONHeader))
            Response.Flush()
            Response.End()
        End Try

        Dim SQL As String
        Dim pConnection As ADODB.Connection = Nothing

        Try
            pConnection = ConnectDB()

            pConnection.BeginTrans()

            SQL = "DELETE FROM tblEvaluateDetail " & _
                  " WHERE username = '" & username & "' " & _
                  "   AND evaluate_year = '" & evaluate_year & "' " & _
                  "   AND round = '" & round & "' " & _
                  "   AND revision = '" & revision & "' " & _
                  ""
            pConnection.Execute(SQL)

            SQL = "DELETE FROM tblEvaluateHeader " & _
                  " WHERE username = '" & username & "' " & _
                  "   AND evaluate_year = '" & evaluate_year & "' " & _
                  "   AND round = '" & round & "' " & _
                  "   AND revision = '" & revision & "' " & _
                  ""
            pConnection.Execute(SQL)

            SQL = "INSERT INTO tblEvaluateHeader (username, evaluate_year, round " & _
                  "   , commander_recomment " & _
                  "   , sum_excepted_PL1 " & _
                  "   , sum_excepted_PL2 " & _
                  "   , sum_excepted_PL3 " & _
                  "   , sum_excepted_PL4 " & _
                  "   , sum_excepted_PL5 " & _
                  "   , sum_evaluate_PL1 " & _
                  "   , sum_evaluate_PL2 " & _
                  "   , sum_evaluate_PL3 " & _
                  "   , sum_evaluate_PL4 " & _
                  "   , sum_evaluate_PL5 " & _
                  "   , SumWeight " & _
                  "   , SumPL " & _
                  "   , SumSuccess " & _
                  "   , FullSuccess " & _
                  "   , PercentSuccess " & _
                  "   , IFFull " & _
                  "   , IFSuccess " & _
                  "   , revision, record_datetime, editor " & _
                  " ) VALUES ( " & _
                  "   '" & username & "' " & _
                  " , '" & evaluate_year & "' " & _
                  " , '" & round & "' " & _
                  " , '" & Str2SQL(objdata.commander_recomment) & "' " & _
                  " , '" & objdata.sum_excepted_PL1 & "' " & _
                  " , '" & objdata.sum_excepted_PL2 & "' " & _
                  " , '" & objdata.sum_excepted_PL3 & "' " & _
                  " , '" & objdata.sum_excepted_PL4 & "' " & _
                  " , '" & objdata.sum_excepted_PL5 & "' " & _
                  " , '" & objdata.sum_evaluate_PL1 & "' " & _
                  " , '" & objdata.sum_evaluate_PL2 & "' " & _
                  " , '" & objdata.sum_evaluate_PL3 & "' " & _
                  " , '" & objdata.sum_evaluate_PL4 & "' " & _
                  " , '" & objdata.sum_evaluate_PL5 & "' " & _
                  " , '" & objdata.SumWeight & "' " & _
                  " , '" & objdata.SumPL & "' " & _
                  " , '" & objdata.SumSuccess & "' " & _
                  " , '" & objdata.FullSuccess & "' " & _
                  " , '" & objdata.PercentSuccess & "' " & _
                  " , '" & objdata.IFFull & "' " & _
                  " , '" & objdata.IFSuccess & "' " & _
                  " , '" & objdata.revision & "' " & _
                  " , '" & GetDateTimeforSQL(Now) & "' " & _
                  " , '" & objdata.editor & "' " & _
                  " ) "
            pConnection.Execute(SQL)

            If (bConfirm) Then
                SQL = "UPDATE tblEvaluateHeader " & _
                      " SET confirmed = '1' " & _
                      " , confirm_datetime = '" & GetDateTimeforSQL(Now) & "' " & _
                      " WHERE username = '" & username & "' " & _
                      "   AND evaluate_year = '" & evaluate_year & "' " & _
                      "   AND round = '" & round & "' " & _
                      "   AND revision = '" & revision & "' " & _
                      ""
                pConnection.Execute(SQL)
            End If

            Dim i
            For i = 0 To objdata.recordCount - 1
                SQL = "INSERT INTO tblEvaluateDetail (username, evaluate_year, round " & _
                      "   , activate_no, activate_name, activate_detail " & _
                      "   , weight, excepted_value, evaluate_value " & _
                      "   , pl_score, success_level, success_score, total_score " & _
                      "   , revision, record_datetime, editor " & _
                      " ) VALUES ( " & _
                      "   '" & username & "' " & _
                      " , '" & evaluate_year & "' " & _
                      " , '" & round & "' " & _
                      " , '" & objdata.detail(i).activate_no & "' " & _
                      " , '" & Str2SQL(objdata.detail(i).activate_name) & "' " & _
                      " , '" & Str2SQL(objdata.detail(i).activate_detail) & "' " & _
                      " , '" & objdata.detail(i).weight & "' " & _
                      " , '" & objdata.detail(i).excepted_value & "' " & _
                      " , '" & objdata.detail(i).evaluate_value & "' " & _
                      " , '" & objdata.detail(i).pl_score & "' " & _
                      " , '" & objdata.detail(i).success_level & "' " & _
                      " , '" & objdata.detail(i).success_score & "' " & _
                      " , '" & objdata.detail(i).total_score & "' " & _
                      " , '" & objdata.detail(i).revision & "' " & _
                      " , '" & objdata.detail(i).record_datetime & "' " & _
                      " , '" & objdata.detail(i).editor & "' " & _
                      " ) " & _
                      ""
                pConnection.Execute(SQL)

            Next i


            pConnection.CommitTrans()

        Catch tex As ThreadAbortException

        Catch ex As Exception
            objJSONHeader.isError = True
            objJSONHeader.ErrMessage = ex.Message
            pConnection.RollbackTrans()
        Finally

            CloseDB(pConnection)

            Dim jobjsend As JavaScriptSerializer = New JavaScriptSerializer()
            Response.Write(jobjsend.Serialize(objJSONHeader))
            Response.Flush()
            Response.End()
        End Try
    End Sub

    Protected Sub LoadSummaryData()
        Dim campus_id = Request.QueryString("campus_id").Trim
        Dim faculty_id = Request.QueryString("faculty_id").Trim
        Dim department_id = Request.QueryString("department_id").Trim
        Dim round = Request.QueryString("round").Trim
        Dim evaluate_year = Request.QueryString("evaluate_year").Trim

        Dim objJSONHeader As New JSONHeader
        objJSONHeader.id = "tblEvaluateHeader"
        objJSONHeader.isError = False
        objJSONHeader.ErrMessage = ""

        Dim SQL As String
        Dim pRecordSet As New ADODB.Recordset
        Dim pConnection As ADODB.Connection = Nothing
        Dim iRecordCount As Integer

        Try
            pConnection = ConnectDB()

            SQL = "SELECT U.employee_id, U.firstname, U.lastname " & _
                  "   , U.username, EH.evaluate_year, EH.round " & _
                  "   , EH.commander_recomment " & _
                  "   , EH.revision " & _
                  "   , EH.sum_excepted_PL1, EH.sum_excepted_PL2, EH.sum_excepted_PL3, EH.sum_excepted_PL4, EH.sum_excepted_PL5 " & _
                  "   , EH.sum_evaluate_PL1, EH.sum_evaluate_PL2, EH.sum_evaluate_PL3, EH.sum_evaluate_PL4, EH.sum_evaluate_PL5 " & _
                  "   , EH.SumWeight, EH.SumPL, EH.SumSuccess, EH.FullSuccess, EH.PercentSuccess, EH.IFFull, EH.IFSuccess " & _
                  "   , EH.confirmed, EH.confirm_datetime " & _
                  "   , U.campus_id, U.faculty_id, U.department_id " & _
                  "   , D.department_name " & _
                  " FROM tblUser U LEFT JOIN tblEvaluateHeader EH ON (EH.username = U.username " & _
                  "   AND EH.evaluate_year = '" & evaluate_year & "' " & _
                  "   AND EH.round = '" & round & "' " & _
                  "   AND EH.revision = (" & _
                  " SELECT MAX(EH2.revision) FROM tblEvaluateHeader EH2  " & _
                  " WHERE EH2.username = EH.username " & _
                  "   AND EH2.evaluate_year = '" & evaluate_year & "' " & _
                  "   AND EH2.round = '" & round & "' " & _
                  "   ) " & _
                  " ), tblMT_Department D " & _
                  " WHERE 1 = 1 " & _
                  "   AND U.campus_id = D.campus_id " & _
                  "   AND U.faculty_id = D.faculty_id " & _
                  "   AND U.department_id = D.department_id " & _
                  "   AND U.campus_id = '" & campus_id & "' " & _
                  "   AND U.faculty_id = '" & faculty_id & "' " & _
                  ""

            If (department_id <> "0") Then
                SQL = SQL & _
                      "   AND U.department_id = '" & department_id & "' " & _
                      ""
            End If

            SQL = SQL & _
                  "   AND U.active = '1' " & _
                  " ORDER BY U.campus_id, U.faculty_id, U.department_id, U.employee_id " & _
                  ""

            iRecordCount = OpenTable(pConnection, pRecordSet, SQL)
            objJSONHeader.recordCount = iRecordCount

            ReDim objJSONHeader.detail(iRecordCount - 1)

            Dim i As Integer = 0
            While Not pRecordSet.EOF
                Dim tblEvaluateHeader As New _tblEvaluateHeader_
                tblEvaluateHeader.id = i + 1
                tblEvaluateHeader.evaluate_year = evaluate_year
                tblEvaluateHeader.round = round

                tblEvaluateHeader.employee_id = GetField(pRecordSet, "employee_id")
                tblEvaluateHeader.firstname = GetField(pRecordSet, "firstname")
                tblEvaluateHeader.lastname = GetField(pRecordSet, "lastname")
                tblEvaluateHeader.username = GetField(pRecordSet, "username")
                tblEvaluateHeader.evaluate_year = GetField(pRecordSet, "evaluate_year")
                tblEvaluateHeader.round = GetField(pRecordSet, "round")

                tblEvaluateHeader.department_name = GetField(pRecordSet, "department_name")

                tblEvaluateHeader.commander_recomment = GetField(pRecordSet, "commander_recomment")

                tblEvaluateHeader.sum_excepted_PL1 = GetField(pRecordSet, "sum_excepted_PL1")
                tblEvaluateHeader.sum_excepted_PL2 = GetField(pRecordSet, "sum_excepted_PL2")
                tblEvaluateHeader.sum_excepted_PL3 = GetField(pRecordSet, "sum_excepted_PL3")
                tblEvaluateHeader.sum_excepted_PL4 = GetField(pRecordSet, "sum_excepted_PL4")
                tblEvaluateHeader.sum_excepted_PL5 = GetField(pRecordSet, "sum_excepted_PL5")

                tblEvaluateHeader.sum_evaluate_PL1 = GetField(pRecordSet, "sum_evaluate_PL1")
                tblEvaluateHeader.sum_evaluate_PL2 = GetField(pRecordSet, "sum_evaluate_PL2")
                tblEvaluateHeader.sum_evaluate_PL3 = GetField(pRecordSet, "sum_evaluate_PL3")
                tblEvaluateHeader.sum_evaluate_PL4 = GetField(pRecordSet, "sum_evaluate_PL4")
                tblEvaluateHeader.sum_evaluate_PL5 = GetField(pRecordSet, "sum_evaluate_PL5")

                tblEvaluateHeader.SumWeight = GetField(pRecordSet, "SumWeight")
                tblEvaluateHeader.SumPL = GetField(pRecordSet, "SumPL")
                tblEvaluateHeader.SumSuccess = GetField(pRecordSet, "SumSuccess")
                tblEvaluateHeader.FullSuccess = GetField(pRecordSet, "FullSuccess")
                tblEvaluateHeader.PercentSuccess = GetField(pRecordSet, "PercentSuccess")
                tblEvaluateHeader.IFFull = GetField(pRecordSet, "IFFull")
                tblEvaluateHeader.IFSuccess = GetField(pRecordSet, "IFSuccess")

                tblEvaluateHeader.confirmed = GetComfirmed(pConnection, tblEvaluateHeader.confirm_datetime, tblEvaluateHeader.username, evaluate_year, round, GetField(pRecordSet, "revision"))
                'tblEvaluateHeader.confirmed = GetField(pRecordSet, "confirmed")
                'tblEvaluateHeader.confirm_datetime = GetField(pRecordSet, "confirm_datetime")

                objJSONHeader.detail(i) = tblEvaluateHeader
                pRecordSet.MoveNext()
                i = i + 1
            End While

        Catch tex As ThreadAbortException

        Catch ex As Exception
            objJSONHeader.isError = True
            objJSONHeader.ErrMessage = ex.Message
        Finally
            If pRecordSet.State > 0 Then
                pRecordSet.Close()
            End If

            CloseDB(pConnection)

            Dim jobjsend As JavaScriptSerializer = New JavaScriptSerializer()
            Response.Write(jobjsend.Serialize(objJSONHeader))
            Response.Flush()
            Response.End()
        End Try
    End Sub

    Protected Sub LoadSummaryDatabyLoginName()
        Dim loginname = Request.QueryString("loginname").Trim
        Dim round = Request.QueryString("round").Trim
        Dim evaluate_year = Request.QueryString("evaluate_year").Trim

        Dim objJSONHeader As New JSONHeader
        objJSONHeader.id = "tblEvaluateHeader"
        objJSONHeader.isError = False
        objJSONHeader.ErrMessage = ""

        Dim SQL As String
        Dim pRecordSet As New ADODB.Recordset
        Dim pConnection As ADODB.Connection = Nothing
        Dim iRecordCount As Integer

        Try
            pConnection = ConnectDB()

            SQL = " SELECT employee_id, firstname, lastname " & _
                  "   , username, evaluate_year, round " & _
                  "   , commander_recomment " & _
                  "   , revision " & _
                  "   , sum_excepted_PL1, sum_excepted_PL2, sum_excepted_PL3, sum_excepted_PL4, sum_excepted_PL5 " & _
                  "   , sum_evaluate_PL1, sum_evaluate_PL2, sum_evaluate_PL3, sum_evaluate_PL4, sum_evaluate_PL5 " & _
                  "   , SumWeight, SumPL, SumSuccess, FullSuccess, PercentSuccess, IFFull, IFSuccess " & _
                  "   , confirmed, confirm_datetime " & _
                  "   , campus_id, faculty_id, U.department_id " & _
                  "   , department_name " & _
                  " FROM (SELECT U.employee_id, U.firstname, U.lastname " & _
                  "   , U.username, EH.evaluate_year, EH.round " & _
                  "   , EH.commander_recomment " & _
                  "   , EH.revision " & _
                  "   , EH.sum_excepted_PL1, EH.sum_excepted_PL2, EH.sum_excepted_PL3, EH.sum_excepted_PL4, EH.sum_excepted_PL5 " & _
                  "   , EH.sum_evaluate_PL1, EH.sum_evaluate_PL2, EH.sum_evaluate_PL3, EH.sum_evaluate_PL4, EH.sum_evaluate_PL5 " & _
                  "   , EH.SumWeight, EH.SumPL, EH.SumSuccess, EH.FullSuccess, EH.PercentSuccess, EH.IFFull, EH.IFSuccess " & _
                  "   , EH.confirmed, EH.confirm_datetime " & _
                  "   , U.campus_id, U.faculty_id, U.department_id " & _
                  "   , D.department_name " & _
                  " FROM tblUser U LEFT JOIN tblEvaluateHeader EH ON (EH.username = U.username " & _
                  "   AND EH.evaluate_year = '" & evaluate_year & "' " & _
                  "   AND EH.round = '" & round & "' " & _
                  "   AND EH.revision = (" & _
                  " SELECT MAX(EH2.revision) FROM tblEvaluateHeader EH2  " & _
                  " WHERE EH2.username = EH.username " & _
                  "   AND EH2.evaluate_year = '" & evaluate_year & "' " & _
                  "   AND EH2.round = '" & round & "' " & _
                  "   ) " & _
                  " ), tblMT_Department D, tblUser U2 " & _
                  " WHERE 1 = 1 " & _
                  "   AND U.campus_id = D.campus_id " & _
                  "   AND U.faculty_id = D.faculty_id " & _
                  "   AND U.department_id = D.department_id " & _
                  "   AND U.campus_id = U2.campus_id " & _
                  "   AND U.faculty_id = U2.faculty_id " & _
                  "   AND U.department_id = U2.department_id " & _
                  "   AND U2.group_id = 2 " & _
                  "   AND U2.username = '" & loginname & "' " & _
                  "   AND U.active = '1' " & _
                  " UNION ALL " & _
                  " SELECT U.employee_id, U.firstname, U.lastname " & _
                  "   , U.username, EH.evaluate_year, EH.round " & _
                  "   , EH.commander_recomment " & _
                  "   , EH.revision " & _
                  "   , EH.sum_excepted_PL1, EH.sum_excepted_PL2, EH.sum_excepted_PL3, EH.sum_excepted_PL4, EH.sum_excepted_PL5 " & _
                  "   , EH.sum_evaluate_PL1, EH.sum_evaluate_PL2, EH.sum_evaluate_PL3, EH.sum_evaluate_PL4, EH.sum_evaluate_PL5 " & _
                  "   , EH.SumWeight, EH.SumPL, EH.SumSuccess, EH.FullSuccess, EH.PercentSuccess, EH.IFFull, EH.IFSuccess " & _
                  "   , EH.confirmed, EH.confirm_datetime " & _
                  "   , U.campus_id, U.faculty_id, U.department_id " & _
                  "   , D.department_name " & _
                  " FROM tblUser U LEFT JOIN tblEvaluateHeader EH ON (EH.username = U.username " & _
                  "   AND EH.evaluate_year = '" & evaluate_year & "' " & _
                  "   AND EH.round = '" & round & "' " & _
                  "   AND EH.revision = (" & _
                  " SELECT MAX(EH2.revision) FROM tblEvaluateHeader EH2  " & _
                  " WHERE EH2.username = EH.username " & _
                  "   AND EH2.evaluate_year = '" & evaluate_year & "' " & _
                  "   AND EH2.round = '" & round & "' " & _
                  "   ) " & _
                  " ), tblMT_Department D, tblUser U2 " & _
                  " WHERE 1 = 1 " & _
                  "   AND U.campus_id = D.campus_id " & _
                  "   AND U.faculty_id = D.faculty_id " & _
                  "   AND U.department_id = D.department_id " & _
                  "   AND U.campus_id = U2.campus_id " & _
                  "   AND U.faculty_id = U2.faculty_id " & _
                  "   AND U2.group_id = 3 " & _
                  "   AND U2.username = '" & loginname & "' " & _
                  "   AND U.active = '1' " & _
                  " ) U " & _
                  " ORDER BY U.campus_id, U.faculty_id, U.department_id, U.employee_id " & _
                  ""

            iRecordCount = OpenTable(pConnection, pRecordSet, SQL)
            objJSONHeader.recordCount = iRecordCount

            ReDim objJSONHeader.detail(iRecordCount - 1)

            Dim i As Integer = 0
            While Not pRecordSet.EOF
                Dim tblEvaluateHeader As New _tblEvaluateHeader_
                tblEvaluateHeader.id = i + 1
                tblEvaluateHeader.evaluate_year = evaluate_year
                tblEvaluateHeader.round = round

                tblEvaluateHeader.employee_id = GetField(pRecordSet, "employee_id")
                tblEvaluateHeader.firstname = GetField(pRecordSet, "firstname")
                tblEvaluateHeader.lastname = GetField(pRecordSet, "lastname")
                tblEvaluateHeader.username = GetField(pRecordSet, "username")
                tblEvaluateHeader.evaluate_year = GetField(pRecordSet, "evaluate_year")
                tblEvaluateHeader.round = GetField(pRecordSet, "round")

                tblEvaluateHeader.department_name = GetField(pRecordSet, "department_name")

                tblEvaluateHeader.commander_recomment = GetField(pRecordSet, "commander_recomment")

                tblEvaluateHeader.sum_excepted_PL1 = GetField(pRecordSet, "sum_excepted_PL1")
                tblEvaluateHeader.sum_excepted_PL2 = GetField(pRecordSet, "sum_excepted_PL2")
                tblEvaluateHeader.sum_excepted_PL3 = GetField(pRecordSet, "sum_excepted_PL3")
                tblEvaluateHeader.sum_excepted_PL4 = GetField(pRecordSet, "sum_excepted_PL4")
                tblEvaluateHeader.sum_excepted_PL5 = GetField(pRecordSet, "sum_excepted_PL5")

                tblEvaluateHeader.sum_evaluate_PL1 = GetField(pRecordSet, "sum_evaluate_PL1")
                tblEvaluateHeader.sum_evaluate_PL2 = GetField(pRecordSet, "sum_evaluate_PL2")
                tblEvaluateHeader.sum_evaluate_PL3 = GetField(pRecordSet, "sum_evaluate_PL3")
                tblEvaluateHeader.sum_evaluate_PL4 = GetField(pRecordSet, "sum_evaluate_PL4")
                tblEvaluateHeader.sum_evaluate_PL5 = GetField(pRecordSet, "sum_evaluate_PL5")

                tblEvaluateHeader.SumWeight = GetField(pRecordSet, "SumWeight")
                tblEvaluateHeader.SumPL = GetField(pRecordSet, "SumPL")
                tblEvaluateHeader.SumSuccess = GetField(pRecordSet, "SumSuccess")
                tblEvaluateHeader.FullSuccess = GetField(pRecordSet, "FullSuccess")
                tblEvaluateHeader.PercentSuccess = GetField(pRecordSet, "PercentSuccess")
                tblEvaluateHeader.IFFull = GetField(pRecordSet, "IFFull")
                tblEvaluateHeader.IFSuccess = GetField(pRecordSet, "IFSuccess")

                tblEvaluateHeader.confirmed = GetComfirmed(pConnection, tblEvaluateHeader.confirm_datetime, tblEvaluateHeader.username, evaluate_year, round, GetField(pRecordSet, "revision"))
                'tblEvaluateHeader.confirmed = GetField(pRecordSet, "confirmed")
                'tblEvaluateHeader.confirm_datetime = GetField(pRecordSet, "confirm_datetime")

                objJSONHeader.detail(i) = tblEvaluateHeader
                pRecordSet.MoveNext()
                i = i + 1
            End While

        Catch tex As ThreadAbortException

        Catch ex As Exception
            objJSONHeader.isError = True
            objJSONHeader.ErrMessage = ex.Message
        Finally
            If pRecordSet.State > 0 Then
                pRecordSet.Close()
            End If

            CloseDB(pConnection)

            Dim jobjsend As JavaScriptSerializer = New JavaScriptSerializer()
            Response.Write(jobjsend.Serialize(objJSONHeader))
            Response.Flush()
            Response.End()
        End Try
    End Sub

    Protected Sub LoadSummaryDataPAbyLoginName()
        Dim loginname = Request.QueryString("loginname").Trim
        Dim evaluate_year = Request.QueryString("evaluate_year").Trim
        Dim round = Request.QueryString("round").Trim

        Dim objJSONHeader As New JSONHeader
        objJSONHeader.id = "tblPAHeader"
        objJSONHeader.isError = False
        objJSONHeader.ErrMessage = ""

        Dim SQL As String
        Dim pRecordSet As New ADODB.Recordset
        Dim pConnection As ADODB.Connection = Nothing
        Dim iRecordCount As Integer

        Try
            pConnection = ConnectDB()

            SQL = " SELECT employee_id, firstname, lastname " & _
                  "   , U.username, U.evaluate_year, U.round " & _
                  "   , confirmed, confirm_datetime " & _
                  "   , result_confirmed, result_confirm_datetime " & _
                  "   , boss_confirmed, boss_confirm_datetime " & _
                  "   , boss_result_confirmed, boss_result_confirm_datetime " & _
                  "   , U.campus_id, U.faculty_id, U.department_id, U.department_name " & _
                  "   , suggest, sum_score, sum_percent, SumScore100, result " & _
                  " , (sum_percent * 70)/100 + (SumScore100*30)/100 AS total_score " & _
                  "  , UD.AVG_SCORE " & _
                  "  , ((sum_percent * 70)/100 + (SumScore100*30)/100) * 80 / UD.AVG_SCORE AS force_mean " & _
                  " FROM (SELECT U.employee_id, U.firstname, U.lastname " & _
                  "   , U.username, EH.evaluate_year, EH.round " & _
                  "   , EH.confirmed, EH.confirm_datetime " & _
                  "   , EH.result_confirmed, EH.result_confirm_datetime " & _
                  "   , EH.boss_confirmed, EH.boss_confirm_datetime " & _
                  "   , EH.boss_result_confirmed, EH.boss_result_confirm_datetime " & _
                  "   , U.campus_id, U.faculty_id, U.department_id, D.department_name " & _
                  "   , EH.suggest, EH.sum_score, EH.sum_percent " & _
                  "   , P02.SumScore100, P02.SumScore, P02.SumEmpScore100, P02.SumEmpScore, P04.Result " & _
                  " FROM tblUser U LEFT JOIN tblPAHeader EH ON (EH.username = U.username " & _
                  "   AND EH.evaluate_year = '" & evaluate_year & "' AND EH.round = '" & round & "' " & _
                  " ) LEFT JOIN tblPart0202 P02 ON (  " & _
                  "    P02.username = U.username And P02.evaluate_year = EH.evaluate_year And P02.round = EH.round " & _
                  " ) LEFT JOIN tblPart04 P04 ON ( " & _
                  "    P04.username = U.username And P04.evaluate_year = EH.evaluate_year And P04.round = EH.round " & _
                  " ), tblMT_Department D, tblUser U2 " & _
                  " WHERE 1 = 1 " & _
                  "   AND U.campus_id = D.campus_id " & _
                  "   AND U.faculty_id = D.faculty_id " & _
                  "   AND U.department_id = D.department_id " & _
                  "   AND U.campus_id = U2.campus_id " & _
                  "   AND U.faculty_id = U2.faculty_id " & _
                  "   AND U.department_id = U2.department_id " & _
                  "   AND U2.group_id = 2 " & _
                  "   AND U2.username = '" & loginname & "' " & _
                  "   AND U.active = '1' " & _
                  " UNION ALL " & _
                  " SELECT U.employee_id, U.firstname, U.lastname " & _
                  "   , U.username, EH.evaluate_year, EH.round " & _
                  "   , EH.confirmed, EH.confirm_datetime " & _
                  "   , EH.result_confirmed, EH.result_confirm_datetime " & _
                  "   , EH.boss_confirmed, EH.boss_confirm_datetime " & _
                  "   , EH.boss_result_confirmed, EH.boss_result_confirm_datetime " & _
                  "   , U.campus_id, U.faculty_id, U.department_id, D.department_name " & _
                  "   , EH.suggest, EH.sum_score, EH.sum_percent " & _
                  "   , P02.SumScore100, P02.SumScore, P02.SumEmpScore100, P02.SumEmpScore, P04.Result " & _
                  " FROM tblUser U LEFT JOIN tblPAHeader EH ON (EH.username = U.username " & _
                  "   AND EH.evaluate_year = '" & evaluate_year & "' AND EH.round = '" & round & "' " & _
                  " ) LEFT JOIN tblPart0202 P02 ON (  " & _
                  "    P02.username = U.username And P02.evaluate_year = EH.evaluate_year And P02.round = EH.round " & _
                  " ) LEFT JOIN tblPart04 P04 ON ( " & _
                  "    P04.username = U.username And P04.evaluate_year = EH.evaluate_year And P04.round = EH.round " & _
                  " ), tblMT_Department D, tblUser U2 " & _
                  " WHERE 1 = 1 " & _
                  "   AND U.campus_id = D.campus_id " & _
                  "   AND U.faculty_id = D.faculty_id " & _
                  "   AND U.department_id = D.department_id " & _
                  "   AND U.campus_id = U2.campus_id " & _
                  "   AND U.faculty_id = U2.faculty_id " & _
                  "   AND U2.group_id = 3 " & _
                  "   AND U2.username = '" & loginname & "' " & _
                  "   AND U.active = '1' " & _
                  " ) U LEFT JOIN ( " & _
                  " SELECT U.campus_id, U.faculty_id, U.department_id, PA.evaluate_year, PA.round " & _
                  " , Avg( (sum_percent * 70 /100) +  (SumEmpScore100 * 30 / 100)) AS AVG_SCORE " & _
                  " FROM tblUser U, tblPAHeader PA, tblPart0202 P02 " & _
                  " WHERE U.username = PA.username AND U.username = P02.username " & _
                  "   AND PA.evaluate_year = P02.evaluate_year AND PA.round = P02.round " & _
                  "   AND U.active = '1' AND PA.sum_percent > 0 AND P02.SumEmpScore100 > 0 " & _
                  " GROUP BY U.campus_id, U.faculty_id, U.department_id, PA.evaluate_year, PA.round " & _
                  " ) UD ON U.campus_id = UD.campus_id AND U.faculty_id = UD.faculty_id AND U.department_id = UD.department_id " & _
                  "   AND U.evaluate_year = UD.evaluate_year AND U.round = UD.round" & _
                  " ORDER BY U.campus_id, U.faculty_id, U.department_id, U.employee_id " & _
                  ""

            iRecordCount = OpenTable(pConnection, pRecordSet, SQL)
            objJSONHeader.recordCount = iRecordCount

            ReDim objJSONHeader.detail(iRecordCount - 1)

            Dim i As Integer = 0
            While Not pRecordSet.EOF
                Dim tblPAHeader As New _tblPAHeader_
                tblPAHeader.id = i + 1
                tblPAHeader.evaluate_year = evaluate_year
                tblPAHeader.round = round

                tblPAHeader.employee_id = GetField(pRecordSet, "employee_id")
                tblPAHeader.firstname = GetField(pRecordSet, "firstname")
                tblPAHeader.lastname = GetField(pRecordSet, "lastname")
                tblPAHeader.username = GetField(pRecordSet, "username")
                tblPAHeader.evaluate_year = GetField(pRecordSet, "evaluate_year")
                tblPAHeader.round = GetField(pRecordSet, "round")

                tblPAHeader.department_name = GetField(pRecordSet, "department_name")

                tblPAHeader.suggest = GetField(pRecordSet, "suggest")

                tblPAHeader.sum_score = GetField(pRecordSet, "sum_score")
                tblPAHeader.sum_percent = GetField(pRecordSet, "sum_percent")

                tblPAHeader.sum_score_100 = GetField(pRecordSet, "SumScore100")
                tblPAHeader.total_score = GetField(pRecordSet, "total_score")
                tblPAHeader.force_mean = GetField(pRecordSet, "force_mean")
                tblPAHeader.result = GetField(pRecordSet, "result")

                    'tblPAHeader.confirmed = GetComfirmed(pConnection, tblEvaluateHeader.confirm_datetime, tblEvaluateHeader.username, evaluate_year, round, GetField(pRecordSet, "revision"))
                tblPAHeader.confirmed = GetField(pRecordSet, "confirmed")
                tblPAHeader.confirm_datetime = GetField(pRecordSet, "confirm_datetime")

                tblPAHeader.result_confirmed = GetField(pRecordSet, "result_confirmed")
                tblPAHeader.result_confirm_datetime = GetField(pRecordSet, "result_confirm_datetime")

                tblPAHeader.boss_confirmed = GetField(pRecordSet, "boss_confirmed")
                tblPAHeader.boss_confirm_datetime = GetField(pRecordSet, "boss_confirm_datetime")

                tblPAHeader.boss_result_confirmed = GetField(pRecordSet, "boss_result_confirmed")
                tblPAHeader.boss_result_confirm_datetime = GetField(pRecordSet, "boss_result_confirm_datetime")

                objJSONHeader.detail(i) = tblPAHeader
                pRecordSet.MoveNext()
                i = i + 1
                End While

        Catch tex As ThreadAbortException

        Catch ex As Exception
            objJSONHeader.isError = True
            objJSONHeader.ErrMessage = ex.Message
        Finally
            If pRecordSet.State > 0 Then
                pRecordSet.Close()
            End If

            CloseDB(pConnection)

            Dim jobjsend As JavaScriptSerializer = New JavaScriptSerializer()
            Response.Write(jobjsend.Serialize(objJSONHeader))
            Response.Flush()
            Response.End()
        End Try
    End Sub

    Protected Class _tblPart0202_
        Public id As String
        Public username As String
        Public evaluate_year As String
        Public round As String

        Public Score01 As String
        Public Score02 As String
        Public Score03 As String
        Public Score04 As String
        Public Score05 As String
        Public Score06 As String
        Public Score07 As String
        Public Score08 As String
        Public Score09 As String
        Public Score10 As String
        Public Score11 As String
        Public EmpScore01 As String
        Public EmpScore02 As String
        Public EmpScore03 As String
        Public EmpScore04 As String
        Public EmpScore05 As String
        Public EmpScore06 As String
        Public EmpScore07 As String
        Public EmpScore08 As String
        Public EmpScore09 As String
        Public EmpScore10 As String
        Public EmpScore11 As String
        Public Suggestion01 As String
        Public Suggestion02 As String
        Public Suggestion03 As String
        Public Suggestion04 As String
        Public Suggestion05 As String
        Public Suggestion06 As String
        Public Suggestion07 As String
        Public Suggestion08 As String
        Public Suggestion09 As String
        Public Suggestion10 As String
        Public Suggestion11 As String

        Public SumScore As String
        Public SumScore25 As String
        Public SumScore100 As String

        Public SumEmpScore As String
        Public SumEmpScore25 As String
        Public SumEmpScore100 As String

        Public Result As String

        Public LevelManager As String

        Public confirmed As String
        Public confirm_datetime As String

        Public revision As String
        Public record_datetime As String
        Public editor As String

    End Class

    Protected Sub LoadPart0202()
        Dim username = Request.QueryString("username").Trim
        Dim evaluate_year = Request.QueryString("evaluate_year").Trim
        Dim round = Request.QueryString("round").Trim
        Dim revision = Request.QueryString("revision").Trim
        Dim sum_only = Request.QueryString("sum_only").Trim

        Dim objJSONHeader As New JSONHeader
        objJSONHeader.id = "tblPart0202"
        objJSONHeader.isError = False
        objJSONHeader.ErrMessage = ""

        Dim SQL As String
        Dim pRecordSet As New ADODB.Recordset
        Dim pConnection As ADODB.Connection = Nothing
        Dim iRecordCount As Integer

        Try
            pConnection = ConnectDB()

            SQL = "SELECT username, evaluate_year, round " & _
                  "   , Score01, Score02, Score03, Score04, Score05, Score06, Score07, Score08, Score09, Score10, Score11 " & _
                  "   , EmpScore01, EmpScore02, EmpScore03, EmpScore04, EmpScore05, EmpScore06, EmpScore07, EmpScore08, EmpScore09, EmpScore10, EmpScore11 " & _
                  "   , Suggestion01, Suggestion02, Suggestion03, Suggestion04, Suggestion05, Suggestion06, Suggestion07, Suggestion08, Suggestion09, Suggestion10, Suggestion11 " & _
                  "   , SumScore, SumScore25, SumScore100 " & _
                  "   , SumEmpScore, SumEmpScore25, SumEmpScore100 " & _
                  "   , Result, confirmed, confirm_datetime " & _
                  "   , revision, record_datetime, editor " & _
                  " FROM tblPart0202 P0202 " & _
                  " WHERE username = '" & username & "' " & _
                  "   AND evaluate_year = '" & evaluate_year & "' " & _
                  "   AND round = '" & round & "' " & _
                  " AND revision = '" & revision & "' " & _
                  " "

            iRecordCount = OpenTable(pConnection, pRecordSet, SQL)
            objJSONHeader.recordCount = iRecordCount

            ReDim objJSONHeader.detail(iRecordCount - 1)

            Dim i As Integer = 0
            While Not pRecordSet.EOF
                Dim objdata As New _tblPart0202_
                objdata.id = i + 1
                objdata.username = evaluate_year
                objdata.evaluate_year = evaluate_year
                objdata.round = round

                If Not sum_only = "1" Then
                    objdata.Score01 = GetField(pRecordSet, "Score01")
                    objdata.Score02 = GetField(pRecordSet, "Score02")
                    objdata.Score03 = GetField(pRecordSet, "Score03")
                    objdata.Score04 = GetField(pRecordSet, "Score04")
                    objdata.Score05 = GetField(pRecordSet, "Score05")
                    objdata.Score06 = GetField(pRecordSet, "Score06")
                    objdata.Score07 = GetField(pRecordSet, "Score07")
                    objdata.Score08 = GetField(pRecordSet, "Score08")
                    objdata.Score09 = GetField(pRecordSet, "Score09")
                    objdata.Score10 = GetField(pRecordSet, "Score10")
                    objdata.Score11 = GetField(pRecordSet, "Score11")
                    objdata.EmpScore01 = GetField(pRecordSet, "EmpScore01")
                    objdata.EmpScore02 = GetField(pRecordSet, "EmpScore02")
                    objdata.EmpScore03 = GetField(pRecordSet, "EmpScore03")
                    objdata.EmpScore04 = GetField(pRecordSet, "EmpScore04")
                    objdata.EmpScore05 = GetField(pRecordSet, "EmpScore05")
                    objdata.EmpScore06 = GetField(pRecordSet, "EmpScore06")
                    objdata.EmpScore07 = GetField(pRecordSet, "EmpScore07")
                    objdata.EmpScore08 = GetField(pRecordSet, "EmpScore08")
                    objdata.EmpScore09 = GetField(pRecordSet, "EmpScore09")
                    objdata.EmpScore10 = GetField(pRecordSet, "EmpScore10")
                    objdata.EmpScore11 = GetField(pRecordSet, "EmpScore11")
                    objdata.Suggestion01 = GetField(pRecordSet, "Suggestion01")
                    objdata.Suggestion02 = GetField(pRecordSet, "Suggestion02")
                    objdata.Suggestion03 = GetField(pRecordSet, "Suggestion03")
                    objdata.Suggestion04 = GetField(pRecordSet, "Suggestion04")
                    objdata.Suggestion05 = GetField(pRecordSet, "Suggestion05")
                    objdata.Suggestion06 = GetField(pRecordSet, "Suggestion06")
                    objdata.Suggestion07 = GetField(pRecordSet, "Suggestion07")
                    objdata.Suggestion08 = GetField(pRecordSet, "Suggestion08")
                    objdata.Suggestion09 = GetField(pRecordSet, "Suggestion09")
                    objdata.Suggestion10 = GetField(pRecordSet, "Suggestion10")
                    objdata.Suggestion11 = GetField(pRecordSet, "Suggestion11")
                End If

                objdata.SumScore = GetField(pRecordSet, "SumScore")
                objdata.SumScore25 = GetField(pRecordSet, "SumScore25")
                objdata.SumScore100 = GetField(pRecordSet, "SumScore100")

                objdata.SumEmpScore = GetField(pRecordSet, "SumEmpScore")
                objdata.SumEmpScore25 = GetField(pRecordSet, "SumEmpScore25")
                objdata.SumEmpScore100 = GetField(pRecordSet, "SumEmpScore100")

                objdata.Result = GetField(pRecordSet, "Result")

                objdata.confirmed = GetField(pRecordSet, "confirmed")
                objdata.confirm_datetime = GetField(pRecordSet, "confirm_datetime")

                objdata.revision = GetField(pRecordSet, "revision")
                objdata.record_datetime = GetField(pRecordSet, "record_datetime")
                objdata.editor = GetField(pRecordSet, "editor")

                objdata.LevelManager = GetLevelManagerByLoginName(username)

                objJSONHeader.detail(i) = objdata
                pRecordSet.MoveNext()
                i = i + 1
            End While

        Catch tex As ThreadAbortException

        Catch ex As Exception
            objJSONHeader.isError = True
            objJSONHeader.ErrMessage = ex.Message
        Finally
            If pRecordSet.State > 0 Then
                pRecordSet.Close()
            End If

            CloseDB(pConnection)

            Dim jobjsend As JavaScriptSerializer = New JavaScriptSerializer()
            Response.Write(jobjsend.Serialize(objJSONHeader))
            Response.Flush()
            Response.End()
        End Try
    End Sub

    Protected Sub SavePart0202(Optional ByVal bConfirm As Boolean = False)
        Dim username As String = ""
        Dim evaluate_year As String = ""
        Dim round As String = ""
        Dim revision As String = ""

        Dim data As String = ""

        Dim objJSONHeader As New JSONHeader
        objJSONHeader.id = "tblPart0202"
        objJSONHeader.isError = False
        objJSONHeader.ErrMessage = ""

        Dim objdata As _tblPart0202_ = Nothing

        Try
            username = Request.QueryString("username").Trim
            evaluate_year = Request.QueryString("evaluate_year").Trim
            round = Request.QueryString("round").Trim
            revision = Request.QueryString("revision").Trim

            'data = Request.Params("data").Trim
            Request.InputStream.Position = 0
            Dim inputStream As New StreamReader(Request.InputStream)
            data = inputStream.ReadToEnd()

            Dim jobj As JavaScriptSerializer = New JavaScriptSerializer()
            objdata = jobj.Deserialize(Of _tblPart0202_)(data)

        Catch ex As Exception
            objJSONHeader.isError = True
            objJSONHeader.ErrMessage = ex.Message

            Dim jobjsend As JavaScriptSerializer = New JavaScriptSerializer()
            Response.Write(jobjsend.Serialize(objJSONHeader))
            Response.Flush()
            Response.End()
        End Try

        Dim SQL As String
        Dim pConnection As ADODB.Connection = Nothing

        Try
            pConnection = ConnectDB()

            pConnection.BeginTrans()

            revision = "0"

            SQL = "DELETE FROM tblPart0202 " & _
                  " WHERE username = '" & username & "' " & _
                  "   AND evaluate_year = '" & evaluate_year & "' " & _
                  "   AND round = '" & round & "' " & _
                  "   AND revision = '" & revision & "' " & _
                  ""
            pConnection.Execute(SQL)

            SQL = "INSERT INTO tblPart0202 (username, evaluate_year, round " & _
                  "   , Score01 " & _
                  "   , Score02 " & _
                  "   , Score03 " & _
                  "   , Score04 " & _
                  "   , Score05 " & _
                  "   , Score06 " & _
                  "   , Score07 " & _
                  "   , Score08 " & _
                  "   , Score09 " & _
                  "   , Score10 " & _
                  "   , Score11 " & _
                  "   , EmpScore01 " & _
                  "   , EmpScore02 " & _
                  "   , EmpScore03 " & _
                  "   , EmpScore04 " & _
                  "   , EmpScore05 " & _
                  "   , EmpScore06 " & _
                  "   , EmpScore07 " & _
                  "   , EmpScore08 " & _
                  "   , EmpScore09 " & _
                  "   , EmpScore10 " & _
                  "   , EmpScore11 " & _
                  "   , Suggestion01 " & _
                  "   , Suggestion02 " & _
                  "   , Suggestion03 " & _
                  "   , Suggestion04 " & _
                  "   , Suggestion05 " & _
                  "   , Suggestion06 " & _
                  "   , Suggestion07 " & _
                  "   , Suggestion08 " & _
                  "   , Suggestion09 " & _
                  "   , Suggestion10 " & _
                  "   , Suggestion11 " & _
                  "   , SumScore " & _
                  "   , SumScore25 " & _
                  "   , SumScore100 " & _
                  "   , SumEmpScore " & _
                  "   , SumEmpScore25 " & _
                  "   , SumEmpScore100 " & _
                  "   , Result " & _
                  "   , confirmed " & _
                  "   , confirm_datetime " & _
                  "   , revision, record_datetime, editor " & _
                  " ) VALUES ( " & _
                  "   '" & username & "' " & _
                  " , '" & evaluate_year & "' " & _
                  " , '" & round & "' " & _
                  " , '" & objdata.Score01 & "' " & _
                  " , '" & objdata.Score02 & "' " & _
                  " , '" & objdata.Score03 & "' " & _
                  " , '" & objdata.Score04 & "' " & _
                  " , '" & objdata.Score05 & "' " & _
                  " , '" & objdata.Score06 & "' " & _
                  " , '" & objdata.Score07 & "' " & _
                  " , '" & objdata.Score08 & "' " & _
                  " , '" & objdata.Score09 & "' " & _
                  " , '" & objdata.Score10 & "' " & _
                  " , '" & objdata.Score11 & "' " & _
                  " , '" & objdata.EmpScore01 & "' " & _
                  " , '" & objdata.EmpScore02 & "' " & _
                  " , '" & objdata.EmpScore03 & "' " & _
                  " , '" & objdata.EmpScore04 & "' " & _
                  " , '" & objdata.EmpScore05 & "' " & _
                  " , '" & objdata.EmpScore06 & "' " & _
                  " , '" & objdata.EmpScore07 & "' " & _
                  " , '" & objdata.EmpScore08 & "' " & _
                  " , '" & objdata.EmpScore09 & "' " & _
                  " , '" & objdata.EmpScore10 & "' " & _
                  " , '" & objdata.EmpScore11 & "' " & _
                  " , '" & Str2SQL(objdata.Suggestion01) & "' " & _
                  " , '" & Str2SQL(objdata.Suggestion02) & "' " & _
                  " , '" & Str2SQL(objdata.Suggestion03) & "' " & _
                  " , '" & Str2SQL(objdata.Suggestion04) & "' " & _
                  " , '" & Str2SQL(objdata.Suggestion05) & "' " & _
                  " , '" & Str2SQL(objdata.Suggestion06) & "' " & _
                  " , '" & Str2SQL(objdata.Suggestion07) & "' " & _
                  " , '" & Str2SQL(objdata.Suggestion08) & "' " & _
                  " , '" & Str2SQL(objdata.Suggestion09) & "' " & _
                  " , '" & Str2SQL(objdata.Suggestion10) & "' " & _
                  " , '" & Str2SQL(objdata.Suggestion11) & "' " & _
                  " , '" & objdata.SumScore & "' " & _
                  " , '" & objdata.SumScore25 & "' " & _
                  " , '" & objdata.SumScore100 & "' " & _
                  " , '" & objdata.SumEmpScore & "' " & _
                  " , '" & objdata.SumEmpScore25 & "' " & _
                  " , '" & objdata.SumEmpScore100 & "' " & _
                  " , '" & objdata.Result & "' " & _
                  " , '" & objdata.confirmed & "' " & _
                  " , '" & objdata.confirm_datetime & "' " & _
                  " , '0' " & _
                  " , '" & GetDateTimeforSQL(Now) & "' " & _
                  " , '" & Session("UserID") & "' " & _
                  " ) "
            pConnection.Execute(SQL)

            'If (bConfirm) Then
            '    SQL = "UPDATE tblPart0202 " & _
            '          " SET confirmed = '1' " & _
            '          " , confirm_datetime = '" & GetDateTimeforSQL(Now) & "' " & _
            '          " WHERE username = '" & username & "' " & _
            '          "   AND evaluate_year = '" & evaluate_year & "' " & _
            '          "   AND round = '" & round & "' " & _
            '          "   AND revision = '" & revision & "' " & _
            '          ""
            '    pConnection.Execute(SQL)
            'End If


            pConnection.CommitTrans()

        Catch tex As ThreadAbortException

        Catch ex As Exception
            objJSONHeader.isError = True
            objJSONHeader.ErrMessage = ex.Message
            pConnection.RollbackTrans()
        Finally

            CloseDB(pConnection)

            Dim jobjsend As JavaScriptSerializer = New JavaScriptSerializer()
            Response.Write(jobjsend.Serialize(objJSONHeader))
            Response.Flush()
            Response.End()
        End Try
    End Sub

    Protected Class _tblPart03_
        Public id As String
        Public username As String
        Public evaluate_year As String
        Public round As String

        Public Employee01() As Object
        Public Employee02() As Object
        Public Employee03() As Object
        Public Employee04() As Object
        Public Commander01() As Object
        Public Commander02() As Object

        Public revision As String
        Public record_datetime As String
        Public editor As String

    End Class

    Protected Sub LoadPart03()
        Dim username = Request.QueryString("username").Trim
        Dim evaluate_year = Request.QueryString("evaluate_year").Trim
        Dim round = Request.QueryString("round").Trim
        Dim revision = Request.QueryString("revision").Trim

        Dim objJSONHeader As New JSONHeader
        objJSONHeader.id = "tblPart03"
        objJSONHeader.isError = False
        objJSONHeader.ErrMessage = ""

        Dim SQL As String
        Dim pRecordSet As New ADODB.Recordset
        Dim pConnection As ADODB.Connection = Nothing
        Dim iRecordCount As Integer

        Try
            pConnection = ConnectDB()

            SQL = "SELECT username, evaluate_year, round " & _
                  "   , type, no, line_item_no " & _
                  "   , detail " & _
                  "   , revision, record_datetime, editor " & _
                  " FROM tblPart03 P03 " & _
                  " WHERE username = '" & username & "' " & _
                  "   AND evaluate_year = '" & evaluate_year & "' " & _
                  "   AND round = '" & round & "' " & _
                  " AND revision = '" & revision & "' " & _
                  " "

            iRecordCount = OpenTable(pConnection, pRecordSet, SQL)
            objJSONHeader.recordCount = iRecordCount

            ReDim objJSONHeader.detail(iRecordCount - 1)

            Dim ALEmployee01 As New ArrayList
            Dim ALEmployee02 As New ArrayList
            Dim ALEmployee03 As New ArrayList
            Dim ALEmployee04 As New ArrayList

            Dim ALCommander01 As New ArrayList
            Dim ALCommander02 As New ArrayList

            Dim objdata As New _tblPart03_
            Dim i As Integer = 0
            While Not pRecordSet.EOF

                objdata.id = i + 1
                objdata.username = evaluate_year
                objdata.evaluate_year = evaluate_year
                objdata.round = round

                If GetField(pRecordSet, "type") = "1" Then
                    Select Case (Trim(GetField(pRecordSet, "no")))
                        Case "1"
                            ALEmployee01.Add(GetField(pRecordSet, "detail"))
                        Case "2"
                            ALEmployee02.Add(GetField(pRecordSet, "detail"))
                        Case "3"
                            ALEmployee03.Add(GetField(pRecordSet, "detail"))
                        Case "4"
                            ALEmployee04.Add(GetField(pRecordSet, "detail"))
                    End Select
                Else
                    Select Case (Trim(GetField(pRecordSet, "no")))
                        Case "1"
                            ALCommander01.Add(GetField(pRecordSet, "detail"))
                        Case "2"
                            ALCommander02.Add(GetField(pRecordSet, "detail"))
                    End Select
                End If

                objdata.revision = GetField(pRecordSet, "revision")
                objdata.record_datetime = GetField(pRecordSet, "record_datetime")
                objdata.editor = GetField(pRecordSet, "editor")

                objdata.Employee01 = ALEmployee01.ToArray
                objdata.Employee02 = ALEmployee02.ToArray
                objdata.Employee03 = ALEmployee03.ToArray
                objdata.Employee04 = ALEmployee04.ToArray


                objdata.Commander01 = ALCommander01.ToArray
                objdata.Commander02 = ALCommander02.ToArray

                objJSONHeader.detail(i) = objdata

                pRecordSet.MoveNext()
                i = i + 1
            End While

        Catch tex As ThreadAbortException

        Catch ex As Exception
            objJSONHeader.isError = True
            objJSONHeader.ErrMessage = ex.Message
        Finally
            If pRecordSet.State > 0 Then
                pRecordSet.Close()
            End If

            CloseDB(pConnection)

            Dim jobjsend As JavaScriptSerializer = New JavaScriptSerializer()
            Response.Write(jobjsend.Serialize(objJSONHeader))
            Response.Flush()
            Response.End()
        End Try
    End Sub

    Protected Sub SavePart03(Optional ByVal bConfirm As Boolean = False)
        Dim username As String = ""
        Dim evaluate_year As String = ""
        Dim round As String = ""
        Dim revision As String = ""

        Dim data As String = ""

        Dim objJSONHeader As New JSONHeader
        objJSONHeader.id = "tblPart03"
        objJSONHeader.isError = False
        objJSONHeader.ErrMessage = ""

        Dim objdata As _tblPart03_ = Nothing

        Try
            username = Request.QueryString("username").Trim
            evaluate_year = Request.QueryString("evaluate_year").Trim
            round = Request.QueryString("round").Trim
            revision = Request.QueryString("revision").Trim

            'data = Request.Params("data").Trim
            Request.InputStream.Position = 0
            Dim inputStream As New StreamReader(Request.InputStream)
            data = inputStream.ReadToEnd()

            Dim jobj As JavaScriptSerializer = New JavaScriptSerializer()
            objdata = jobj.Deserialize(Of _tblPart03_)(data)

        Catch ex As Exception
            objJSONHeader.isError = True
            objJSONHeader.ErrMessage = ex.Message

            Dim jobjsend As JavaScriptSerializer = New JavaScriptSerializer()
            Response.Write(jobjsend.Serialize(objJSONHeader))
            Response.Flush()
            Response.End()
        End Try

        Dim SQL As String
        Dim pConnection As ADODB.Connection = Nothing

        Try
            pConnection = ConnectDB()

            pConnection.BeginTrans()

            revision = "0"

            SQL = "DELETE FROM tblPart03 " & _
                  " WHERE username = '" & username & "' " & _
                  "   AND evaluate_year = '" & evaluate_year & "' " & _
                  "   AND round = '" & round & "' " & _
                  "   AND revision = '" & revision & "' " & _
                  ""
            pConnection.Execute(SQL)
            Dim i As Integer
            For i = 0 To objdata.Employee01.Length - 1
                SQL = "INSERT INTO tblPart03 (username, evaluate_year, round, type, no " & _
                      "   , line_item_no, detail " & _
                      "   , revision, record_datetime, editor " & _
                      " ) VALUES ( " & _
                      "   '" & username & "' " & _
                      " , '" & evaluate_year & "' " & _
                      " , '" & round & "' " & _
                      " , '1' " & _
                      " , '1' " & _
                      " , '" & (i + 1) & "' " & _
                      " , '" & Str2SQL(objdata.Employee01(i)) & "' " & _
                      " , '0' " & _
                      " , '" & GetDateTimeforSQL(Now) & "' " & _
                      " , '" & Session("UserID") & "' " & _
                      " ) "
                pConnection.Execute(SQL)
            Next i
            For i = 0 To objdata.Employee02.Length - 1
                SQL = "INSERT INTO tblPart03 (username, evaluate_year, round, type, no " & _
                      "   , line_item_no, detail " & _
                      "   , revision, record_datetime, editor " & _
                      " ) VALUES ( " & _
                      "   '" & username & "' " & _
                      " , '" & evaluate_year & "' " & _
                      " , '" & round & "' " & _
                      " , '1' " & _
                      " , '2' " & _
                      " , '" & (i + 1) & "' " & _
                      " , '" & Str2SQL(objdata.Employee02(i)) & "' " & _
                      " , '0' " & _
                      " , '" & GetDateTimeforSQL(Now) & "' " & _
                      " , '" & Session("UserID") & "' " & _
                      " ) "
                pConnection.Execute(SQL)
            Next i
            For i = 0 To objdata.Employee03.Length - 1
                SQL = "INSERT INTO tblPart03 (username, evaluate_year, round, type, no " & _
                      "   , line_item_no, detail " & _
                      "   , revision, record_datetime, editor " & _
                      " ) VALUES ( " & _
                      "   '" & username & "' " & _
                      " , '" & evaluate_year & "' " & _
                      " , '" & round & "' " & _
                      " , '1' " & _
                      " , '3' " & _
                      " , '" & (i + 1) & "' " & _
                      " , '" & Str2SQL(objdata.Employee03(i)) & "' " & _
                      " , '0' " & _
                      " , '" & GetDateTimeforSQL(Now) & "' " & _
                      " , '" & Session("UserID") & "' " & _
                      " ) "
                pConnection.Execute(SQL)
            Next i
            For i = 0 To objdata.Employee04.Length - 1
                SQL = "INSERT INTO tblPart03 (username, evaluate_year, round, type, no " & _
                      "   , line_item_no, detail " & _
                      "   , revision, record_datetime, editor " & _
                      " ) VALUES ( " & _
                      "   '" & username & "' " & _
                      " , '" & evaluate_year & "' " & _
                      " , '" & round & "' " & _
                      " , '1' " & _
                      " , '4' " & _
                      " , '" & (i + 1) & "' " & _
                      " , '" & Str2SQL(objdata.Employee04(i)) & "' " & _
                      " , '0' " & _
                      " , '" & GetDateTimeforSQL(Now) & "' " & _
                      " , '" & Session("UserID") & "' " & _
                      " ) "
                pConnection.Execute(SQL)
            Next i
            For i = 0 To objdata.Commander01.Length - 1
                SQL = "INSERT INTO tblPart03 (username, evaluate_year, round, type, no " & _
                      "   , line_item_no, detail " & _
                      "   , revision, record_datetime, editor " & _
                      " ) VALUES ( " & _
                      "   '" & username & "' " & _
                      " , '" & evaluate_year & "' " & _
                      " , '" & round & "' " & _
                      " , '2' " & _
                      " , '1' " & _
                      " , '" & (i + 1) & "' " & _
                      " , '" & Str2SQL(objdata.Commander01(i)) & "' " & _
                      " , '0' " & _
                      " , '" & GetDateTimeforSQL(Now) & "' " & _
                      " , '" & Session("UserID") & "' " & _
                      " ) "
                pConnection.Execute(SQL)
            Next i
            For i = 0 To objdata.Commander02.Length - 1
                SQL = "INSERT INTO tblPart03 (username, evaluate_year, round, type, no " & _
                      "   , line_item_no, detail " & _
                      "   , revision, record_datetime, editor " & _
                      " ) VALUES ( " & _
                      "   '" & username & "' " & _
                      " , '" & evaluate_year & "' " & _
                      " , '" & round & "' " & _
                      " , '2' " & _
                      " , '2' " & _
                      " , '" & (i + 1) & "' " & _
                      " , '" & Str2SQL(objdata.Commander02(i)) & "' " & _
                      " , '0' " & _
                      " , '" & GetDateTimeforSQL(Now) & "' " & _
                      " , '" & Session("UserID") & "' " & _
                      " ) "
                pConnection.Execute(SQL)
            Next i
            pConnection.CommitTrans()

        Catch tex As ThreadAbortException

        Catch ex As Exception
            objJSONHeader.isError = True
            objJSONHeader.ErrMessage = ex.Message
            pConnection.RollbackTrans()
        Finally

            CloseDB(pConnection)

            Dim jobjsend As JavaScriptSerializer = New JavaScriptSerializer()
            Response.Write(jobjsend.Serialize(objJSONHeader))
            Response.Flush()
            Response.End()
        End Try
    End Sub

    Protected Class _tblPart04_
        Public id As String
        Public username As String
        Public evaluate_year As String
        Public round As String

        Public Result As String
        Public UpPercent As String
        Public Level As String
        Public Quality As String

        Public confirmed As String
        Public confirm_datetime As String

        Public revision As String
        Public record_datetime As String
        Public editor As String

    End Class

    Protected Sub LoadPart04()
        Dim username = Request.QueryString("username").Trim
        Dim evaluate_year = Request.QueryString("evaluate_year").Trim
        Dim round = Request.QueryString("round").Trim
        Dim revision = Request.QueryString("revision").Trim

        Dim objJSONHeader As New JSONHeader
        objJSONHeader.id = "tblPart04"
        objJSONHeader.isError = False
        objJSONHeader.ErrMessage = ""

        Dim SQL As String
        Dim pRecordSet As New ADODB.Recordset
        Dim pConnection As ADODB.Connection = Nothing
        Dim iRecordCount As Integer

        Try
            pConnection = ConnectDB()

            SQL = "SELECT username, evaluate_year, round " & _
                  "   , Result, UpPercent, level, Quality " & _
                  "   , revision, record_datetime, editor " & _
                  " FROM tblPart04 P04 " & _
                  " WHERE username = '" & username & "' " & _
                  "   AND evaluate_year = '" & evaluate_year & "' " & _
                  "   AND round = '" & round & "' " & _
                  " "

            iRecordCount = OpenTable(pConnection, pRecordSet, SQL)
            objJSONHeader.recordCount = iRecordCount

            ReDim objJSONHeader.detail(iRecordCount - 1)

            Dim objdata As New _tblPart04_
            Dim i As Integer = 0
            While Not pRecordSet.EOF

                objdata.id = i + 1
                objdata.username = evaluate_year
                objdata.evaluate_year = evaluate_year
                objdata.round = round

                objdata.Result = GetField(pRecordSet, "Result")
                objdata.UpPercent = GetField(pRecordSet, "UpPercent")
                objdata.Level = GetField(pRecordSet, "Level")
                objdata.Quality = GetField(pRecordSet, "Quality")

                objdata.revision = GetField(pRecordSet, "revision")
                objdata.record_datetime = GetField(pRecordSet, "record_datetime")
                objdata.editor = GetField(pRecordSet, "editor")

                objJSONHeader.detail(i) = objdata

                pRecordSet.MoveNext()
                i = i + 1
            End While

        Catch tex As ThreadAbortException

        Catch ex As Exception
            objJSONHeader.isError = True
            objJSONHeader.ErrMessage = ex.Message
        Finally
            If pRecordSet.State > 0 Then
                pRecordSet.Close()
            End If

            CloseDB(pConnection)

            Dim jobjsend As JavaScriptSerializer = New JavaScriptSerializer()
            Response.Write(jobjsend.Serialize(objJSONHeader))
            Response.Flush()
            Response.End()
        End Try
    End Sub

    Protected Sub SavePart04()
        Dim username As String = ""
        Dim evaluate_year As String = ""
        Dim round As String = ""
        Dim revision As String = ""

        Dim data As String = ""

        Dim objJSONHeader As New JSONHeader
        objJSONHeader.id = "tblPart04"
        objJSONHeader.isError = False
        objJSONHeader.ErrMessage = ""

        Dim objdata As _tblPart04_ = Nothing

        Try
            username = Request.QueryString("username").Trim
            evaluate_year = Request.QueryString("evaluate_year").Trim
            round = Request.QueryString("round").Trim
            revision = Request.QueryString("revision").Trim

            'data = Request.Params("data").Trim
            Request.InputStream.Position = 0
            Dim inputStream As New StreamReader(Request.InputStream)
            data = inputStream.ReadToEnd()

            Dim jobj As JavaScriptSerializer = New JavaScriptSerializer()
            objdata = jobj.Deserialize(Of _tblPart04_)(data)

        Catch ex As Exception
            objJSONHeader.isError = True
            objJSONHeader.ErrMessage = ex.Message

            Dim jobjsend As JavaScriptSerializer = New JavaScriptSerializer()
            Response.Write(jobjsend.Serialize(objJSONHeader))
            Response.Flush()
            Response.End()
        End Try

        Dim SQL As String
        Dim pConnection As ADODB.Connection = Nothing

        Try
            pConnection = ConnectDB()

            pConnection.BeginTrans()

            SQL = "DELETE FROM tblPart04 " & _
                  " WHERE username = '" & username & "' " & _
                  "   AND evaluate_year = '" & evaluate_year & "' " & _
                  "   AND round = '" & round & "' " & _
                  "   AND revision = '" & revision & "' " & _
                  ""
            pConnection.Execute(SQL)

            SQL = "INSERT INTO tblPart04 (username, evaluate_year, round " & _
                      "   , Result, UpPercent, Level, Quality " & _
                      "   , revision, record_datetime, editor " & _
                      " ) VALUES ( " & _
                      "   '" & username & "' " & _
                      " , '" & evaluate_year & "' " & _
                      " , '" & round & "' " & _
                      " , '" & objdata.Result & "' " & _
                      " , '" & objdata.UpPercent & "' " & _
                      " , '" & objdata.Level & "' " & _
                      " , '" & objdata.Quality & "' " & _
                      " , '0' " & _
                      " , '" & GetDateTimeforSQL(Now) & "' " & _
                      " , '" & objdata.editor & "' " & _
                      " ) "
            pConnection.Execute(SQL)

            pConnection.CommitTrans()

        Catch tex As ThreadAbortException

        Catch ex As Exception
            objJSONHeader.isError = True
            objJSONHeader.ErrMessage = ex.Message
            pConnection.RollbackTrans()
        Finally

            CloseDB(pConnection)

            Dim jobjsend As JavaScriptSerializer = New JavaScriptSerializer()
            Response.Write(jobjsend.Serialize(objJSONHeader))
            Response.Flush()
            Response.End()
        End Try
    End Sub

    Protected Sub SaveConfirm()
        Dim username As String = ""
        Dim evaluate_year As String = ""
        Dim round As String = ""
        Dim revision As String = ""

        Dim data As String = ""

        Dim objJSONHeader As New JSONHeader
        objJSONHeader.id = "tblPart04"
        objJSONHeader.isError = False
        objJSONHeader.ErrMessage = ""

        Dim objdata As _tblPart04_ = Nothing

        Try
            username = Request.QueryString("username").Trim
            evaluate_year = Request.QueryString("evaluate_year").Trim
            round = Request.QueryString("round").Trim
            revision = Request.QueryString("revision").Trim

            'data = Request.Params("data").Trim
            Request.InputStream.Position = 0
            Dim inputStream As New StreamReader(Request.InputStream)
            data = inputStream.ReadToEnd()

            Dim jobj As JavaScriptSerializer = New JavaScriptSerializer()
            objdata = jobj.Deserialize(Of _tblPart04_)(data)

        Catch ex As Exception
            objJSONHeader.isError = True
            objJSONHeader.ErrMessage = ex.Message

            Dim jobjsend As JavaScriptSerializer = New JavaScriptSerializer()
            Response.Write(jobjsend.Serialize(objJSONHeader))
            Response.Flush()
            Response.End()
        End Try

        Dim SQL As String
        Dim pConnection As ADODB.Connection = Nothing

        Try
            pConnection = ConnectDB()

            pConnection.BeginTrans()

            'SQL = "SELECT COUNT(*) FROM tblEvaluateHeader " & _
            '      " WHERE username = '" & username & "' " & _
            '      "   AND evaluate_year = '" & evaluate_year & "' " & _
            '      "   AND round = '" & round & "' " & _
            '      "   AND revision = '" & revision & "' " & _
            '      ""
            'pConnection.Execute(SQL)

            objJSONHeader.RetMessage = GetDateTimeforSQL(Now)

            SQL = "UPDATE tblEvaluateHeader " & _
                      " SET confirmed = '" & objdata.confirmed & "' " & _
                      " , confirm_datetime = '" & objJSONHeader.RetMessage & "' " & _
                      " WHERE username = '" & username & "' " & _
                      "   AND evaluate_year = '" & evaluate_year & "' " & _
                      "   AND round = '" & round & "' " & _
                      "   AND revision = '" & revision & "' " & _
                      ""
            pConnection.Execute(SQL)

            pConnection.CommitTrans()

        Catch tex As ThreadAbortException

        Catch ex As Exception
            objJSONHeader.isError = True
            objJSONHeader.ErrMessage = ex.Message
            pConnection.RollbackTrans()
        Finally

            CloseDB(pConnection)

            Dim jobjsend As JavaScriptSerializer = New JavaScriptSerializer()
            Response.Write(jobjsend.Serialize(objJSONHeader))
            Response.Flush()
            Response.End()
        End Try
    End Sub

    Private Shared Function CreateRow(ByVal refRow As Row, ByVal sheetData As SheetData, ByVal refRow1 As Row) As Row
        Dim newRowIndex As UInteger = 0
        Dim newRow = New Row() With {.RowIndex = refRow.RowIndex.Value}

        ' Loop through all the rows in the worksheet with higher row
        ' index values than the one you just added. For each one,
        ' increment the existing row index.
        Dim rows As IEnumerable(Of Row) = sheetData.Descendants(Of Row)().Where(Function(r) r.RowIndex.Value > refRow.RowIndex.Value)
        For Each row As Row In rows
            newRowIndex = System.Convert.ToUInt32(row.RowIndex.Value + 1)

            For Each cell As Cell In row.Elements(Of Cell)()
                ' Update the references for reserved cells.
                Dim cellReference As String = cell.CellReference.Value
                cell.CellReference = New StringValue(cellReference.Replace(row.RowIndex.Value.ToString(), newRowIndex.ToString()))
            Next
            ' Update the row index.
            row.RowIndex = New UInt32Value(newRowIndex)
        Next

        'sheetData.InsertAt(newRow, 40);
        sheetData.InsertBefore(newRow, refRow)

        Return newRow
    End Function

    Public Sub ExportEvaluateForm()

        Dim username = Request.QueryString("username").Trim
        Dim evaluate_year = Request.QueryString("evaluate_year").Trim
        Dim round = Request.QueryString("round").Trim
        Dim revision = Request.QueryString("revision").Trim

        Dim LevelManager = GetLevelManagerByLoginName(username)

        Dim outputPath As String = MapPath("~/output/")
        Dim template As String = MapPath("~/template/template03.xlsx")
        If (LevelManager.Trim = "1") Then template = MapPath("~/template/template02.xlsx")
        'Dim UserEvaluate As _USER_ = GetUserInfo(username)
        'Dim filename As String = username & "_" & evaluate_year & "_" & round & "_" & UserEvaluate.firstname & "_" & UserEvaluate.lastname & ".xlsx"
        Dim filename As String = username & "_" & evaluate_year & "_" & round & "_" & ".xlsx"
        Dim newFile As String = outputPath & filename

        File.Copy(template, newFile, True)

        Dim stream As Stream = File.Open(newfile, FileMode.Open)
        OutputTo(stream, username, round, evaluate_year, revision)

        'Caller must close the stream.
        stream.Close()

        ' Display the document
        Response.Clear()
        Response.ContentEncoding = System.Text.Encoding.UTF8
        Response.ContentType = "application/vnd.openxmlformats"
        Response.AddHeader("content-disposition", "atachement; filename=" & filename)

        Dim byteArray As Byte() = File.ReadAllBytes(newFile)
        Dim mem As New MemoryStream()
        mem.Write(byteArray, 0, CInt(byteArray.Length))

        mem.Position = 0
        Dim writer As New BinaryWriter(Response.OutputStream)
        Dim reader As New BinaryReader(mem)
        Dim buffer As Byte() = mem.ToArray()

        Response.BinaryWrite(buffer)

    End Sub

    Public Function OutputTo(ByVal stream As Stream, ByVal username As String, ByVal round As String, ByVal year As String, ByVal revision As String) As Boolean
        ' Open a SpreadsheetDocument based on a stream.
        Dim UserEvaluate As _USER_ = GetUserInfo(username)

        Dim Document As SpreadsheetDocument = SpreadsheetDocument.Open(stream, True)

        Try

            Dim wbPart As WorkbookPart = Document.WorkbookPart

            Dim SheetName1 As String = "1"
            Dim SheetName As String = "2.1"
            Dim SheetName22 As String = "2.2 (ปฏิบัติการ)"
            Dim SheetName3 As String = "3"

            UpdateValue(wbPart, SheetName1, "E7", "..." & UserEvaluate.firstname & " " & UserEvaluate.lastname & "...", 0, True)
            UpdateValue(wbPart, SheetName1, "P7", "..." & UserEvaluate.position & "...", 0, True)
            UpdateValue(wbPart, SheetName1, "V7", Mid(UserEvaluate.employee_id, 5) & "/" & Mid(UserEvaluate.employee_id, 1, 4), 0, True)
            UpdateValue(wbPart, SheetName1, "AA7", "..." & GetUserDepartmentName(UserEvaluate) & "...", 0, True)
            UpdateValue(wbPart, SheetName1, "N18", round & "/" & year, 0, True)

            LoadEvaluatePAToOpenXML(Document, SheetName, 4, username, round, year, revision, 1)
            LoadEvaluatePAToOpenXML(Document, SheetName, 36, username, round, year, revision, 2)
            LoadEvaluatePAToOpenXML(Document, SheetName, 68, username, round, year, revision, 3)
            LoadEvaluatePAToOpenXML(Document, SheetName, 100, username, round, year, revision, 4)

            LoadEvaluatePart0202ToOpenXML(Document, SheetName22, username, round, year, revision, UserEvaluate.level_manager)

            'LoadEvaluatePart03ToOpenXML(Document, SheetName3, username, round, year, revision)

            'AddNewSheet(Document, "New Sheet")
            'Document.WorkbookPart.Workbook.CalculationProperties.FullCalculationOnLoad = True
            Document.WorkbookPart.Workbook.CalculationProperties.ForceFullCalculation = True
            Document.WorkbookPart.Workbook.Save()

            Return True
        Catch ex As Exception
            Return False
        Finally
            'Close the document handle.
            Document.Close()
        End Try

    End Function

    Public Function OutputTo_V1(ByVal stream As Stream, ByVal username As String, ByVal round As String, ByVal year As String, ByVal revision As String) As Boolean
        ' Open a SpreadsheetDocument based on a stream.
        Dim UserEvaluate As _USER_ = GetUserInfo(username)

        Dim Document As SpreadsheetDocument = SpreadsheetDocument.Open(stream, True)

        Try

            Dim wbPart As WorkbookPart = Document.WorkbookPart

            Dim SheetName As String = "2.1"
            Dim SheetName22 As String = "2.2ปฏิบัติการ"
            Dim SheetName3 As String = "3"

            UpdateValue(wbPart, SheetName, "B2", "หน่วยงาน..." & GetUserDepartmentName(UserEvaluate) & "... : ชื่อ ..." & UserEvaluate.firstname & " " & UserEvaluate.lastname & "...   ตำแหน่ง ... " & UserEvaluate.position & "...", 0, True)
            UpdateValue(wbPart, SheetName, "M2", UserEvaluate.level, 0, True)
            UpdateValue(wbPart, SheetName, "P2", round & "/" & year, 0, True)

            LoadEvaluateDetailToOpenXML(Document, SheetName, 7, username, round, year, revision)

            LoadEvaluatePart0202ToOpenXML(Document, SheetName22, username, round, year, revision, UserEvaluate.level_manager)

            LoadEvaluatePart03ToOpenXML(Document, SheetName3, username, round, year, revision)
            'InsertRow(Document, "Sheet1", 7, 10)

            'AddNewSheet(Document, "New Sheet")

            Document.WorkbookPart.Workbook.Save()

            Return True
        Catch ex As Exception
            Return False
        Finally
            'Close the document handle.
            Document.Close()
        End Try

    End Function

End Class