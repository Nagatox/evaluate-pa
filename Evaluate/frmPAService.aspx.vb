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

Partial Public Class frmPAService
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim cmd As String = Request.QueryString("q")

        Select Case cmd

            Case "LoadPAHeader"
                LoadPAHeader()

            Case "LoadPADetail"
                LoadPADetail()

            Case "LoadPADetailResult"
                LoadPADetailResult()

            Case "SavePAForm"
                SavePAForm(False)

            Case "SaveConfirmPAForm"
                SavePAForm(True)

            Case "BossSave"
                BossSave()

            Case "BossUnlock"
                BossUnlock()

            Case "BossConfirm"
                BossConfirm()

            Case "SavePAResult"
                SavePAResult(False)

            Case "SaveConfirmPAResult"
                SavePAResult(True)

            Case "BossSaveResult"
                BossSaveResult()

            Case "BossUnlockResult"
                BossUnlockResult()

            Case "BossConfirmResult"
                BossConfirmResult()

            Case "ExportPAForm"
                ExportPAForm()

            Case Else

        End Select

        Response.End()

    End Sub


    Protected Class _tblPAHeader_
        Public id As String
        Public username As String
        Public evaluate_year As String
        Public round As String

        Public confirmed As String
        Public confirm_datetime As String
        Public result_confirmed As String
        Public result_confirm_datetime As String

        Public boss_confirmed As String
        Public boss_confirm_datetime As String
        Public boss_result_confirmed As String
        Public boss_result_confirm_datetime As String

        Public user_entrydate As String
        Public boss As String
        Public boss_entrydate As String
        Public boss2 As String
        Public boss2_entrydate As String

        Public suggest As String
        Public sum_score As String
        Public sum_percent As String

        Public recordCount As Integer
        Public detail As _tblPADetail_()
    End Class

    Protected Sub LoadPAHeader()

        Dim username = Request.QueryString("username").Trim
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

            SQL = "SELECT confirmed, confirm_datetime " & _
                  " , result_confirmed, result_confirm_datetime " & _
                  " , boss_confirmed, boss_confirm_datetime " & _
                  " , boss_result_confirmed, boss_result_confirm_datetime " & _
                  " , user_entrydate " & _
                  " , boss, boss_entrydate " & _
                  " , boss2, boss2_entrydate " & _
                  " , suggest " & _
                  " , sum_score, sum_percent " & _
                  " FROM tblPAHeader PAH " & _
                  " WHERE username = '" & username & "' " & _
                  "   AND evaluate_year = '" & evaluate_year & "' " & _
                  "   AND round = '" & round & "' " & _
                  ""

            iRecordCount = OpenTable(pConnection, pRecordSet, SQL)
            objJSONHeader.recordCount = iRecordCount
            iRecordCount = 1

            ReDim objJSONHeader.detail(iRecordCount - 1)

            Dim i As Integer = 0
            'While Not pRecordSet.EOF
            Dim objPAHeader As New _tblPAHeader_
            If Not pRecordSet.EOF Then
                objPAHeader.id = i + 1
                objPAHeader.username = username
                objPAHeader.evaluate_year = evaluate_year
                objPAHeader.round = round

                objPAHeader.confirmed = GetField(pRecordSet, "confirmed")
                objPAHeader.confirm_datetime = GetField(pRecordSet, "confirm_datetime")
                objPAHeader.result_confirmed = GetField(pRecordSet, "result_confirmed")
                objPAHeader.result_confirm_datetime = GetField(pRecordSet, "result_confirm_datetime")

                objPAHeader.boss_confirmed = GetField(pRecordSet, "boss_confirmed")
                objPAHeader.boss_confirm_datetime = GetField(pRecordSet, "boss_confirm_datetime")
                objPAHeader.boss_result_confirmed = GetField(pRecordSet, "boss_result_confirmed")
                objPAHeader.boss_result_confirm_datetime = GetField(pRecordSet, "boss_result_confirm_datetime")

                objPAHeader.user_entrydate = GetField(pRecordSet, "user_entrydate")
                objPAHeader.boss = GetField(pRecordSet, "boss")
                objPAHeader.boss_entrydate = GetField(pRecordSet, "boss_entrydate")
                objPAHeader.boss2 = GetField(pRecordSet, "boss2")
                objPAHeader.boss2_entrydate = GetField(pRecordSet, "boss2_entrydate")

                objPAHeader.suggest = GetField(pRecordSet, "suggest")
                objPAHeader.sum_score = GetField(pRecordSet, "sum_score")
                objPAHeader.sum_percent = GetField(pRecordSet, "sum_percent")

                objJSONHeader.detail(i) = objPAHeader
            Else
                objPAHeader.id = i + 1
                objPAHeader.username = username
                objPAHeader.evaluate_year = evaluate_year
                objPAHeader.round = round

                objJSONHeader.detail(i) = objPAHeader
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

    Protected Class _tblPADetail_
        Public id As String
        Public username As String
        Public evaluate_year As String
        Public round As String

        Public item_group_no As String
        Public item_no As String
        Public detail As String
        Public detail2 As String
        Public weight As String
        Public month_08 As String
        Public month_09 As String
        Public month_10 As String
        Public month_11 As String
        Public month_12 As String
        Public month_01 As String
        Public month_02 As String
        Public month_03 As String
        Public month_04 As String
        Public month_05 As String
        Public month_06 As String
        Public month_07 As String
        Public KPI As String
        Public STG As String
        Public comment As String

        Public record_datetime As String
        Public editor As String

    End Class

    Protected Class _tblPAHeaderResult_
        Public id As String
        Public username As String
        Public evaluate_year As String
        Public round As String

        Public user_entrydate As String
        Public boss As String
        Public boss_entrydate As String
        Public boss2 As String
        Public boss2_entrydate As String

        Public suggest As String
        Public sum_score As String
        Public sum_percent As String


        Public recordCount As Integer
        Public detail As _tblPADetailResult_()
    End Class

    Protected Class _tblPADetailResult_
        Public id As String
        Public username As String
        Public evaluate_year As String
        Public round As String

        Public item_group_no As String
        Public item_no As String
        Public LoS As String
        Public LoSBoss As String
        Public comment2 As String

        Public comment As String

        Public record_datetime As String
        Public editor As String

    End Class

    Protected Function LoadPADetailToOpenXML(ByRef Document As SpreadsheetDocument, ByVal SheetName As String, ByVal rowStart As Integer, ByVal username As String, ByVal evaluate_year As String, ByVal round As String, ByVal revision As String) As Integer

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

    Protected Sub LoadPADetail()
        Dim username = Request.QueryString("username").Trim
        Dim evaluate_year = Request.QueryString("evaluate_year").Trim
        Dim round = Request.QueryString("round").Trim

        Dim objJSONHeader As New JSONHeader
        objJSONHeader.id = "tblPADetail"
        objJSONHeader.isError = False
        objJSONHeader.ErrMessage = ""

        Dim SQL As String
        Dim pRecordSet As New ADODB.Recordset
        Dim pConnection As ADODB.Connection = Nothing
        Dim iRecordCount As Integer

        Try
            pConnection = ConnectDB()

            SQL = "SELECT username, evaluate_year, round " & _
                  "   , item_group_no, item_no, detail, detail2, weight " & _
                  "   , month_08, month_09, month_10 " & _
                  "   , month_11, month_12, month_01 " & _
                  "   , month_02, month_03, month_04 " & _
                  "   , month_05, month_06, month_07 " & _
                  "   , KPI, STG, comment " & _
                  "   , record_datetime, editor " & _
                  " FROM tblPADetail PAD " & _
                  " WHERE username = '" & username & "' " & _
                  "   AND evaluate_year = '" & evaluate_year & "' " & _
                  "   AND round = '" & round & "' " & _
                  " "

            SQL = SQL & _
                  " ORDER BY item_group_no, item_no " & _
                  ""

            iRecordCount = OpenTable(pConnection, pRecordSet, SQL)
            objJSONHeader.recordCount = iRecordCount

            ReDim objJSONHeader.detail(iRecordCount - 1)

            Dim i As Integer = 0
            While Not pRecordSet.EOF
                Dim tblEvaluateDetail As New _tblPADetail_
                tblEvaluateDetail.id = i + 1
                tblEvaluateDetail.username = username
                tblEvaluateDetail.evaluate_year = evaluate_year
                tblEvaluateDetail.round = round

                tblEvaluateDetail.item_group_no = GetField(pRecordSet, "item_group_no")
                tblEvaluateDetail.item_no = GetField(pRecordSet, "item_no")
                tblEvaluateDetail.detail = GetField(pRecordSet, "detail")
                tblEvaluateDetail.detail2 = GetField(pRecordSet, "detail2")
                tblEvaluateDetail.weight = GetField(pRecordSet, "weight")

                tblEvaluateDetail.month_08 = GetField(pRecordSet, "month_08")
                tblEvaluateDetail.month_09 = GetField(pRecordSet, "month_09")
                tblEvaluateDetail.month_10 = GetField(pRecordSet, "month_10")
                tblEvaluateDetail.month_11 = GetField(pRecordSet, "month_11")
                tblEvaluateDetail.month_12 = GetField(pRecordSet, "month_12")
                tblEvaluateDetail.month_01 = GetField(pRecordSet, "month_01")
                tblEvaluateDetail.month_02 = GetField(pRecordSet, "month_02")
                tblEvaluateDetail.month_03 = GetField(pRecordSet, "month_03")
                tblEvaluateDetail.month_04 = GetField(pRecordSet, "month_04")
                tblEvaluateDetail.month_05 = GetField(pRecordSet, "month_05")
                tblEvaluateDetail.month_06 = GetField(pRecordSet, "month_06")
                tblEvaluateDetail.month_07 = GetField(pRecordSet, "month_07")

                tblEvaluateDetail.KPI = GetField(pRecordSet, "KPI")
                tblEvaluateDetail.STG = GetField(pRecordSet, "STG")
                tblEvaluateDetail.comment = GetField(pRecordSet, "comment")
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

    Protected Sub LoadPADetailResult()
        Dim username = Request.QueryString("username").Trim
        Dim evaluate_year = Request.QueryString("evaluate_year").Trim
        Dim round = Request.QueryString("round").Trim

        Dim objJSONHeader As New JSONHeader
        objJSONHeader.id = "tblPADetailResult"
        objJSONHeader.isError = False
        objJSONHeader.ErrMessage = ""

        Dim SQL As String
        Dim pRecordSet As New ADODB.Recordset
        Dim pConnection As ADODB.Connection = Nothing
        Dim iRecordCount As Integer

        Try
            pConnection = ConnectDB()

            SQL = "SELECT username, evaluate_year, round " & _
                  "   , item_group_no, item_no, LoS, LoSBoss, comment2 " & _
                  "   , record_datetime, editor " & _
                  " FROM tblPADetailResult PADR " & _
                  " WHERE username = '" & username & "' " & _
                  "   AND evaluate_year = '" & evaluate_year & "' " & _
                  "   AND round = '" & round & "' " & _
                  " "

            SQL = SQL & _
                  " ORDER BY item_group_no, item_no " & _
                  ""

            iRecordCount = OpenTable(pConnection, pRecordSet, SQL)
            objJSONHeader.recordCount = iRecordCount

            ReDim objJSONHeader.detail(iRecordCount - 1)

            Dim i As Integer = 0
            While Not pRecordSet.EOF
                Dim tblEvaluateDetail As New _tblPADetailResult_
                tblEvaluateDetail.id = i + 1
                tblEvaluateDetail.username = username
                tblEvaluateDetail.evaluate_year = evaluate_year
                tblEvaluateDetail.round = round

                tblEvaluateDetail.item_group_no = GetField(pRecordSet, "item_group_no")
                tblEvaluateDetail.item_no = GetField(pRecordSet, "item_no")
                tblEvaluateDetail.LoS = GetField(pRecordSet, "LoS")
                tblEvaluateDetail.LoSBoss = GetField(pRecordSet, "LoSBoss")
                tblEvaluateDetail.comment2 = GetField(pRecordSet, "comment2")
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

    Protected Sub BossUnlock()
        Dim username As String = ""
        Dim evaluate_year As String = ""
        Dim round As String = ""

        Dim data As String = ""

        Dim objJSONHeader As New JSONHeader
        objJSONHeader.id = "tblPAForm"
        objJSONHeader.isError = False
        objJSONHeader.ErrMessage = ""

        Dim objdata As _tblPAHeader_ = Nothing

        Try
            username = Request.QueryString("username").Trim
            evaluate_year = Request.QueryString("evaluate_year").Trim
            round = Request.QueryString("round").Trim

            'data = Request.Params("data").Trim
            Request.InputStream.Position = 0
            Dim inputStream As New StreamReader(Request.InputStream)
            data = inputStream.ReadToEnd()

            Dim jobj As JavaScriptSerializer = New JavaScriptSerializer()
            objdata = jobj.Deserialize(Of _tblPAHeader_)(data)

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

            SQL = "UPDATE tblPAHeader SET " & _
                  " confirmed = 0, boss_confirmed = 0 " & _
                  " WHERE username = '" & username & "' " & _
                  "   AND evaluate_year = '" & evaluate_year & "' " & _
                  "   AND round = '" & round & "' " & _
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

    Protected Sub BossSave()
        Dim username As String = ""
        Dim evaluate_year As String = ""
        Dim round As String = ""

        Dim data As String = ""

        Dim objJSONHeader As New JSONHeader
        objJSONHeader.id = "tblPAForm"
        objJSONHeader.isError = False
        objJSONHeader.ErrMessage = ""

        Dim objdata As _tblPAHeader_ = Nothing

        Try
            username = Request.QueryString("username").Trim
            evaluate_year = Request.QueryString("evaluate_year").Trim
            round = Request.QueryString("round").Trim

            'data = Request.Params("data").Trim
            Request.InputStream.Position = 0
            Dim inputStream As New StreamReader(Request.InputStream)
            data = inputStream.ReadToEnd()

            Dim jobj As JavaScriptSerializer = New JavaScriptSerializer()
            objdata = jobj.Deserialize(Of _tblPAHeader_)(data)

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

            SQL = "UPDATE tblPAHeader " & _
                  " SET suggest = '" & Str2SQL(objdata.suggest) & "' " & _
                  " WHERE username = '" & username & "' " & _
                  "   AND evaluate_year = '" & evaluate_year & "' " & _
                  "   AND round = '" & round & "' " & _
                  ""
            pConnection.Execute(SQL)

            Dim i
            For i = 0 To objdata.recordCount - 1
                SQL = "UPDATE tblPADetail " & _
                      " SET comment = '" & Str2SQL(objdata.detail(i).comment) & "' " & _
                      " WHERE username = '" & username & "' " & _
                      "   AND evaluate_year = '" & evaluate_year & "' " & _
                      "   AND round = '" & round & "' " & _
                      "   AND item_group_no = '" & objdata.detail(i).item_group_no & "' " & _
                      "   AND item_no = '" & objdata.detail(i).item_no & "' " & _
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

    Protected Sub BossConfirm()
        Dim username As String = ""
        Dim evaluate_year As String = ""
        Dim round As String = ""

        Dim data As String = ""

        Dim objJSONHeader As New JSONHeader
        objJSONHeader.id = "tblPAForm"
        objJSONHeader.isError = False
        objJSONHeader.ErrMessage = ""

        Dim objdata As _tblPAHeader_ = Nothing

        Try
            username = Request.QueryString("username").Trim
            evaluate_year = Request.QueryString("evaluate_year").Trim
            round = Request.QueryString("round").Trim

            'data = Request.Params("data").Trim
            Request.InputStream.Position = 0
            Dim inputStream As New StreamReader(Request.InputStream)
            data = inputStream.ReadToEnd()

            Dim jobj As JavaScriptSerializer = New JavaScriptSerializer()
            objdata = jobj.Deserialize(Of _tblPAHeader_)(data)

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

            SQL = "UPDATE tblPAHeader SET " & _
                  " boss_confirmed = 1 " & _
                  " , boss_confirm_datetime = CONVERT(VARCHAR, GETDATE(),20) " & _
                  " WHERE username = '" & username & "' " & _
                  "   AND evaluate_year = '" & evaluate_year & "' " & _
                  "   AND round = '" & round & "' " & _
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

    Protected Sub SavePAForm(Optional ByVal bConfirm As Boolean = False)
        Dim username As String = ""
        Dim evaluate_year As String = ""
        Dim round As String = ""

        Dim data As String = ""

        Dim objJSONHeader As New JSONHeader
        objJSONHeader.id = "tblPAForm"
        objJSONHeader.isError = False
        objJSONHeader.ErrMessage = ""

        Dim objdata As _tblPAHeader_ = Nothing

        Try
            username = Request.QueryString("username").Trim
            evaluate_year = Request.QueryString("evaluate_year").Trim
            round = Request.QueryString("round").Trim

            'data = Request.Params("data").Trim
            Request.InputStream.Position = 0
            Dim inputStream As New StreamReader(Request.InputStream)
            data = inputStream.ReadToEnd()

            Dim jobj As JavaScriptSerializer = New JavaScriptSerializer()
            objdata = jobj.Deserialize(Of _tblPAHeader_)(data)

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

            ' Keep existing supervisor comments during user Save (avoid losing boss comment on subsequent user save)
            Dim commentMap As New System.Collections.Hashtable
            SQL = "SELECT item_group_no, item_no, comment FROM tblPADetail " & _
                  " WHERE username = '" & username & "' " & _
                  "   AND evaluate_year = '" & evaluate_year & "' " & _
                  "   AND round = '" & round & "' " & _
                  ""
            Dim rs As ADODB.Recordset = pConnection.Execute(SQL)
            While Not rs.EOF
                Dim key As String = rs.Fields("item_group_no").Value & "|" & rs.Fields("item_no").Value
                If Not commentMap.ContainsKey(key) Then
                    commentMap.Add(key, rs.Fields("comment").Value)
                End If
                rs.MoveNext()
            End While
            If rs.State > 0 Then rs.Close()

            SQL = "DELETE FROM tblPADetail " & _
                  " WHERE username = '" & username & "' " & _
                  "   AND evaluate_year = '" & evaluate_year & "' " & _
                  "   AND round = '" & round & "' " & _
                  ""
            pConnection.Execute(SQL)

            SQL = "DELETE FROM tblPAHeader " & _
                  " WHERE username = '" & username & "' " & _
                  "   AND evaluate_year = '" & evaluate_year & "' " & _
                  "   AND round = '" & round & "' " & _
                  ""
            pConnection.Execute(SQL)

            If Not bConfirm Then
                SQL = "INSERT INTO tblPAHeader (username, evaluate_year, round, user_entrydate " & _
                      "   , boss, boss_entrydate " & _
                      "   , boss2, boss2_entrydate " & _
                      "   , suggest " & _
                      " ) VALUES ( " & _
                      "   '" & username & "' " & _
                      " , '" & evaluate_year & "' " & _
                      " , '" & round & "' " & _
                      " , '" & objdata.user_entrydate & "' " & _
                      " , '" & objdata.boss & "' " & _
                      " , '" & objdata.boss_entrydate & "' " & _
                      " , '" & objdata.boss2 & "' " & _
                      " , '" & objdata.boss2_entrydate & "' " & _
                      " , '" & Str2SQL(objdata.suggest) & "' " & _
                      " ) "
            Else
                SQL = "INSERT INTO tblPAHeader (username, evaluate_year, round, confirmed, confirm_datetime, user_entrydate " & _
                      "   , boss, boss_entrydate " & _
                      "   , boss2, boss2_entrydate " & _
                      "   , suggest " & _
                      " ) VALUES ( " & _
                      "   '" & username & "' " & _
                      " , '" & evaluate_year & "' " & _
                      " , '" & round & "' " & _
                      " , '1' " & _
                      " , CONVERT(VARCHAR, GETDATE(),20) " & _
                      " , '" & objdata.user_entrydate & "' " & _
                      " , '" & objdata.boss & "' " & _
                      " , '" & objdata.boss_entrydate & "' " & _
                      " , '" & objdata.boss2 & "' " & _
                      " , '" & objdata.boss2_entrydate & "' " & _
                      " , '" & Str2SQL(objdata.suggest) & "' " & _
                      " ) "

            End If

            pConnection.Execute(SQL)

            Dim i
            For i = 0 To objdata.recordCount - 1
                Dim detailComment As String = ""
                Dim keyComment As String = objdata.detail(i).item_group_no & "|" & objdata.detail(i).item_no
                If Not String.IsNullOrEmpty(objdata.detail(i).comment) Then
                    detailComment = objdata.detail(i).comment
                ElseIf commentMap.ContainsKey(keyComment) Then
                    detailComment = commentMap(keyComment)
                End If

                SQL = "INSERT INTO tblPADetail (username, evaluate_year, round, item_group_no " & _
                      "   , item_no, detail, detail2, weight " & _
                      "   , month_08, month_09, month_10 " & _
                      "   , month_11, month_12, month_01 " & _
                      "   , month_02, month_03, month_04 " & _
                      "   , month_05, month_06, month_07 " & _
                      "   , KPI, STG, comment " & _
                      "   , record_datetime, editor " & _
                      " ) VALUES ( " & _
                      "   '" & username & "' " & _
                      " , '" & evaluate_year & "' " & _
                      " , '" & round & "' " & _
                      " , '" & objdata.detail(i).item_group_no & "' " & _
                      " , '" & objdata.detail(i).item_no & "' " & _
                      " , '" & Str2SQL(objdata.detail(i).detail) & "' " & _
                      " , '" & Str2SQL(objdata.detail(i).detail2) & "' " & _
                      " , '" & objdata.detail(i).weight & "' " & _
                      " , '" & objdata.detail(i).month_08 & "' " & _
                      " , '" & objdata.detail(i).month_09 & "' " & _
                      " , '" & objdata.detail(i).month_10 & "' " & _
                      " , '" & objdata.detail(i).month_11 & "' " & _
                      " , '" & objdata.detail(i).month_12 & "' " & _
                      " , '" & objdata.detail(i).month_01 & "' " & _
                      " , '" & objdata.detail(i).month_02 & "' " & _
                      " , '" & objdata.detail(i).month_03 & "' " & _
                      " , '" & objdata.detail(i).month_04 & "' " & _
                      " , '" & objdata.detail(i).month_05 & "' " & _
                      " , '" & objdata.detail(i).month_06 & "' " & _
                      " , '" & objdata.detail(i).month_07 & "' " & _
                      " , '" & Str2SQL(objdata.detail(i).KPI) & "' " & _
                      " , '" & objdata.detail(i).STG & "' " & _
                      " , '" & Str2SQL(detailComment) & "' " & _
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

    Protected Sub SavePAResult(Optional ByVal bConfirm As Boolean = False)
        Dim username As String = ""
        Dim evaluate_year As String = ""
        Dim round As String = ""

        Dim data As String = ""

        Dim objJSONHeader As New JSONHeader
        objJSONHeader.id = "tblPAForm"
        objJSONHeader.isError = False
        objJSONHeader.ErrMessage = ""

        Dim objdata As _tblPAHeaderResult_ = Nothing

        Try
            username = Request.QueryString("username").Trim
            evaluate_year = Request.QueryString("evaluate_year").Trim
            round = Request.QueryString("round").Trim

            'data = Request.Params("data").Trim
            Request.InputStream.Position = 0
            Dim inputStream As New StreamReader(Request.InputStream)
            data = inputStream.ReadToEnd()

            Dim jobj As JavaScriptSerializer = New JavaScriptSerializer()
            objdata = jobj.Deserialize(Of _tblPAHeaderResult_)(data)

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

            If Not bConfirm Then
                SQL = "UPDATE tblPAHeader set sum_score = '" & Str2SQL(objdata.sum_score) & "' " & _
                      " , sum_percent = '" & Str2SQL(objdata.sum_percent) & "' " & _
                      " WHERE username = '" & username & "' " & _
                      "   AND evaluate_year = '" & evaluate_year & "' " & _
                      "   AND round = '" & round & "' " & _
                      ""
                pConnection.Execute(SQL)
            Else
                SQL = "UPDATE tblPAHeader set result_confirmed = '1' " & _
                      " , result_confirm_datetime = CONVERT(VARCHAR, GETDATE(),20) " & _
                      " , sum_score = '" & Str2SQL(objdata.sum_score) & "' " & _
                      " , sum_percent = '" & Str2SQL(objdata.sum_percent) & "' " & _
                      " WHERE username = '" & username & "' " & _
                      "   AND evaluate_year = '" & evaluate_year & "' " & _
                      "   AND round = '" & round & "' " & _
                      ""
                pConnection.Execute(SQL)
            End If

            SQL = "DELETE FROM tblPADetailResult  " & _
                  " WHERE username = '" & username & "' " & _
                  "   AND evaluate_year = '" & evaluate_year & "' " & _
                  "   AND round = '" & round & "' " & _
                  ""
            pConnection.Execute(SQL)

            Dim i
            For i = 0 To objdata.recordCount - 1
                SQL = "INSERT INTO tblPADetailResult (username, evaluate_year, round, item_group_no " & _
                      "   , item_no, LoS, comment2 " & _
                      "   , record_datetime, editor " & _
                      " ) VALUES ( " & _
                      "   '" & username & "' " & _
                      " , '" & evaluate_year & "' " & _
                      " , '" & round & "' " & _
                      " , '" & objdata.detail(i).item_group_no & "' " & _
                      " , '" & objdata.detail(i).item_no & "' " & _
                      " , '" & objdata.detail(i).LoS & "' " & _
                      " , '" & Str2SQL(objdata.detail(i).comment2) & "' " & _
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

    Protected Sub BossUnlockResult()
        Dim username As String = ""
        Dim evaluate_year As String = ""
        Dim round As String = ""

        Dim data As String = ""

        Dim objJSONHeader As New JSONHeader
        objJSONHeader.id = "BossUnlockResult"
        objJSONHeader.isError = False
        objJSONHeader.ErrMessage = ""

        Dim objdata As _tblPAHeader_ = Nothing

        Try
            username = Request.QueryString("username").Trim
            evaluate_year = Request.QueryString("evaluate_year").Trim
            round = Request.QueryString("round").Trim

            'data = Request.Params("data").Trim
            Request.InputStream.Position = 0
            Dim inputStream As New StreamReader(Request.InputStream)
            data = inputStream.ReadToEnd()

            Dim jobj As JavaScriptSerializer = New JavaScriptSerializer()
            objdata = jobj.Deserialize(Of _tblPAHeader_)(data)

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

            SQL = "UPDATE tblPAHeader SET " & _
                  " result_confirmed = 0, boss_result_confirmed = 0 " & _
                  " WHERE username = '" & username & "' " & _
                  "   AND evaluate_year = '" & evaluate_year & "' " & _
                  "   AND round = '" & round & "' " & _
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

    Protected Sub BossSaveResult()
        Dim username As String = ""
        Dim evaluate_year As String = ""
        Dim round As String = ""

        Dim data As String = ""

        Dim objJSONHeader As New JSONHeader
        objJSONHeader.id = "BossSaveResult"
        objJSONHeader.isError = False
        objJSONHeader.ErrMessage = ""

        Dim objdata As _tblPAHeaderResult_ = Nothing

        Try
            username = Request.QueryString("username").Trim
            evaluate_year = Request.QueryString("evaluate_year").Trim
            round = Request.QueryString("round").Trim

            'data = Request.Params("data").Trim
            Request.InputStream.Position = 0
            Dim inputStream As New StreamReader(Request.InputStream)
            data = inputStream.ReadToEnd()

            Dim jobj As JavaScriptSerializer = New JavaScriptSerializer()
            objdata = jobj.Deserialize(Of _tblPAHeaderResult_)(data)

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

            SQL = "UPDATE tblPAHeader " & _
                  " SET suggest = '" & Str2SQL(objdata.suggest) & "' " & _
                  " , sum_score = '" & objdata.sum_score & "' " & _
                  " , sum_percent = '" & objdata.sum_percent & "' " & _
                  " WHERE username = '" & username & "' " & _
                  "   AND evaluate_year = '" & evaluate_year & "' " & _
                  "   AND round = '" & round & "' " & _
                  ""
            pConnection.Execute(SQL)

            Dim i
            For i = 0 To objdata.recordCount - 1
                SQL = "UPDATE tblPADetailResult " & _
                      " SET LoSBoss = '" & objdata.detail(i).LoSBoss & "' " & _
                      " WHERE username = '" & username & "' " & _
                      "   AND evaluate_year = '" & evaluate_year & "' " & _
                      "   AND round = '" & round & "' " & _
                      "   AND item_group_no = '" & objdata.detail(i).item_group_no & "' " & _
                      "   AND item_no = '" & objdata.detail(i).item_no & "' " & _
                      ""
                pConnection.Execute(SQL)

                If Not String.IsNullOrEmpty(objdata.detail(i).comment) Then
                    SQL = "UPDATE tblPADetail " & _
                          " SET comment = '" & Str2SQL(objdata.detail(i).comment) & "' " & _
                          " WHERE username = '" & username & "' " & _
                          "   AND evaluate_year = '" & evaluate_year & "' " & _
                          "   AND round = '" & round & "' " & _
                          "   AND item_group_no = '" & objdata.detail(i).item_group_no & "' " & _
                          "   AND item_no = '" & objdata.detail(i).item_no & "' " & _
                          ""
                    pConnection.Execute(SQL)
                End If
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

    Protected Sub BossConfirmResult()
        Dim username As String = ""
        Dim evaluate_year As String = ""
        Dim round As String = ""

        Dim data As String = ""

        Dim objJSONHeader As New JSONHeader
        objJSONHeader.id = "BossConfirmResult"
        objJSONHeader.isError = False
        objJSONHeader.ErrMessage = ""

        Dim objdata As _tblPAHeaderResult_ = Nothing

        Try
            username = Request.QueryString("username").Trim
            evaluate_year = Request.QueryString("evaluate_year").Trim
            round = Request.QueryString("round").Trim

            'data = Request.Params("data").Trim
            Request.InputStream.Position = 0
            Dim inputStream As New StreamReader(Request.InputStream)
            data = inputStream.ReadToEnd()

            Dim jobj As JavaScriptSerializer = New JavaScriptSerializer()
            objdata = jobj.Deserialize(Of _tblPAHeaderResult_)(data)

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

            SQL = "UPDATE tblPAHeader SET " & _
                  " boss_result_confirmed = 1 " & _
                  " , boss_result_confirm_datetime = CONVERT(VARCHAR, GETDATE(),20) " & _
                  " WHERE username = '" & username & "' " & _
                  "   AND evaluate_year = '" & evaluate_year & "' " & _
                  "   AND round = '" & round & "' " & _
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

    Public Sub ExportPAForm()

        Dim username = Request.QueryString("username").Trim
        Dim evaluate_year = Request.QueryString("evaluate_year").Trim
        Dim round = Request.QueryString("round").Trim
        Dim revision = Request.QueryString("revision").Trim

        Dim LevelManager = GetLevelManagerByLoginName(username)

        Dim outputPath As String = MapPath("~/output/")
        Dim template As String = MapPath("~/template/template01.xlsx")
        If (LevelManager.Trim = "1") Then template = MapPath("~/template/template02.xlsx")
        'Dim UserEvaluate As _USER_ = GetUserInfo(username)
        'Dim filename As String = username & "_" & evaluate_year & "_" & round & "_" & UserEvaluate.firstname & "_" & UserEvaluate.lastname & ".xlsx"
        Dim filename As String = username & "_" & evaluate_year & "_" & round & "_" & ".xlsx"
        Dim newFile As String = outputPath & filename

        File.Copy(template, newFile, True)

        Dim stream As Stream = File.Open(newFile, FileMode.Open)
        OutputTo(stream, username, evaluate_year, round, revision)

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

    Public Function OutputTo(ByVal stream As Stream, ByVal username As String, ByVal evaluate_year As String, ByVal round As String, ByVal revision As String) As Boolean
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
            UpdateValue(wbPart, SheetName, "P2", round & "/" & evaluate_year, 0, True)

            LoadPADetailToOpenXML(Document, SheetName, 7, username, evaluate_year, round, revision)

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