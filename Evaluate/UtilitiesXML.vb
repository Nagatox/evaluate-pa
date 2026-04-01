Imports System.IO
Imports System.IO.Packaging

Imports DocumentFormat.OpenXml
Imports DocumentFormat.OpenXml.Packaging
Imports DocumentFormat.OpenXml.Spreadsheet
Imports DocumentFormat.OpenXml.Drawing.Spreadsheet


Module UtilitiesXML
    Public Enum RowMode
        Insert
        Update
    End Enum

    Public Sub HiddenRow(ByRef Document As SpreadsheetDocument, ByVal SheetName As String, ByVal rowStart As Integer, ByVal rowLength As Integer)
        Dim workbookpart As WorkbookPart = Document.WorkbookPart

        Dim sheet As Sheet = workbookpart.Workbook.Descendants(Of Sheet)().Where(Function(s) s.Name = SheetName).FirstOrDefault()

        Dim worksheet As Worksheet = DirectCast(workbookpart.GetPartById(sheet.Id), WorksheetPart).Worksheet

        Dim sheetData As SheetData = worksheet.GetFirstChild(Of SheetData)()

        For i = 0 To rowLength - 1
            Dim rowi As Row = GetRow(sheetData, rowStart + i)
            rowi.Hidden = True
        Next i
    End Sub

    Public Sub RemoveRow(ByRef Document As SpreadsheetDocument, ByVal SheetName As String, ByVal rowStart As Integer, ByVal rowLength As Integer)
        Dim workbookpart As WorkbookPart = Document.WorkbookPart

        Dim sheet As Sheet = workbookpart.Workbook.Descendants(Of Sheet)().Where(Function(s) s.Name = SheetName).FirstOrDefault()

        Dim worksheet As Worksheet = DirectCast(workbookpart.GetPartById(sheet.Id), WorksheetPart).Worksheet

        Dim sheetData As SheetData = worksheet.GetFirstChild(Of SheetData)()

        For i = 0 To rowLength - 1
            Dim rowi As Row = GetRow(sheetData, rowStart + i)
            rowi.Remove()
        Next
    End Sub
    Public Sub InsertRow(ByRef Document As SpreadsheetDocument, ByVal SheetName As String, ByVal rowStart As Integer, ByVal rowLength As Integer)
        Dim workbookpart As WorkbookPart = Document.WorkbookPart

        Dim sheet As Sheet = workbookpart.Workbook.Descendants(Of Sheet)().Where(Function(s) s.Name = SheetName).FirstOrDefault()

        Dim worksheet As Worksheet = DirectCast(workbookpart.GetPartById(sheet.Id), WorksheetPart).Worksheet

        Dim sheetData As SheetData = worksheet.GetFirstChild(Of SheetData)()
#If 0 Then


        For i = 1 To sheetData.Count()
            Dim rowi As Row = GetRow(sheetData, i)
            For Each cell As Cell In rowi.Elements(Of Cell)()
                Dim cellformula As String = ""
                If Not cell.CellFormula Is Nothing Then
                    CellFormula = cell.CellFormula.Text
                    'cell.CellFormula = New CellFormula(CellFormula)
                End If
            Next
        Next
#End If
#If True Then
        For i = sheetData.Count() To 1 Step -1
            'For i = 1 To sheetData.Count()
            'Dim rowx = sheetData.Elements(Of Row)().Where(Function(r) r.RowIndex = 7).FirstOrDefault()
            Dim rowi As Row = GetRow(sheetData, i)

            If rowi.RowIndex.Value >= rowStart Then
                Dim oldRowIndex As Integer = rowi.RowIndex.Value
                Dim newRowIndex As Integer = rowi.RowIndex.Value + rowLength

                For Each cell As Cell In rowi.Elements(Of Cell)()
                    ' Update the references for reserved cells.
                    If Not cell.CellFormula Is Nothing Then
                        Dim cellformula As String = AdjustFormula(cell.CellFormula.Text, newRowIndex)
                        cell.CellFormula = New CellFormula(cellformula)
                    End If
                    Dim cellReference As String = cell.CellReference.Value
                    cell.CellReference = New DocumentFormat.OpenXml.StringValue(cellReference.Replace(rowi.RowIndex.Value.ToString(), newRowIndex.ToString()))
                Next

                rowi.RowIndex.Value = newRowIndex
            End If
        Next i
#End If
#If True Then
        Dim rowTemplate As Row = GetRow(worksheet.WorksheetPart, rowStart + rowLength) ' Row Template (ahead)

        Dim rowPosition As Row = rowTemplate
        Dim rowNew As Row
        For i = 1 To rowLength
            Dim newIndex As Integer = rowStart + rowLength - i
            rowNew = rowTemplate.CloneNode(True)
            For Each cell As Cell In rowNew.Elements(Of Cell)()
                ' Update the references for reserved cells.
                If Not cell.CellFormula Is Nothing Then
                    Dim cellformula As String = AdjustFormula(cell.CellFormula.Text, newIndex)
                    cell.CellFormula = New CellFormula(cellformula)
                End If

                Dim cellReference As String = cell.CellReference.Value
                cell.CellReference = New StringValue(cellReference.Replace(rowNew.RowIndex.Value.ToString(), newIndex))
            Next
            rowNew.RowIndex = newIndex

            sheetData.InsertBefore(rowNew, rowPosition)

            rowPosition = rowNew
        Next i

        AdjustMergedCells(worksheet.WorksheetPart, rowStart, rowLength)
        'Dim mcells As MergeCells = worksheet.GetFirstChild(Of MergeCells)()
        'mcells.Remove()
#End If

    End Sub

    Private Function AdjustFormula(originalFormula As String, newRowIndex As Integer) As String
        ' รูปแบบ regex สำหรับค้นหา cell references ในสูตร
        Dim pattern As String = "([A-Z]+)(\d+)"
        Dim regex As New Regex(pattern)

        Return regex.Replace(originalFormula, Function(match)
                                                  Dim column As String = match.Groups(1).Value
                                                  Dim row As Integer = Integer.Parse(match.Groups(2).Value)

                                                  ' ปรับแถวในสูตร (เพิ่ม 1 สำหรับทุก cell reference)
                                                  Dim adjustedRow As Integer = row + 1
                                                  adjustedRow = newRowIndex

                                                  ' ถ้าเป็น absolute reference ($) ไม่ต้องปรับ
                                                  If originalFormula.Contains("$" & match.Groups(2).Value) Then
                                                      Return match.Value
                                                  End If

                                                  Return column & adjustedRow
                                              End Function)
    End Function

    Public Sub InsertRow0(ByRef Document As SpreadsheetDocument, ByVal SheetName As String, ByVal rowStart As Integer, ByVal rowLength As Integer)

        Dim workbookpart As WorkbookPart = Document.WorkbookPart

        Dim sheet As Sheet = workbookpart.Workbook.Descendants(Of Sheet)().Where(Function(s) s.Name = SheetName).FirstOrDefault()

        Dim worksheet As Worksheet = DirectCast(workbookpart.GetPartById(sheet.Id), WorksheetPart).Worksheet

        Dim sheetData As SheetData = worksheet.GetFirstChild(Of SheetData)()

        For i = sheetData.Count() To 1 Step -1
            'For i = 1 To sheetData.Count()

            Dim rowi As Row = GetRow(sheetData, i)

            If rowi.RowIndex.Value >= rowStart Then
                Dim oldRowIndex As Integer = rowi.RowIndex.Value
                Dim newRowIndex As Integer = rowi.RowIndex.Value + rowLength

                For Each cell As Cell In rowi.Elements(Of Cell)()
                    ' Update the references for reserved cells.
                    Dim cellReference As String = cell.CellReference.Value
                    cell.CellReference = New StringValue(cellReference.Replace(rowi.RowIndex.Value.ToString(), newRowIndex.ToString()))
                Next

                rowi.RowIndex.Value = newRowIndex

            End If

        Next i

        Dim rowTemplate As Row = GetRow(worksheet.WorksheetPart, rowStart + rowLength) ' Row Template (ahead)

        Dim rowPosition As Row = rowTemplate
        Dim rowNew As Row
        For i = 1 To rowLength
            Dim newIndex As Integer = rowStart + rowLength - i
            rowNew = rowTemplate.CloneNode(True)
            For Each cell As Cell In rowNew.Elements(Of Cell)()
                ' Update the references for reserved cells.
                Dim cellReference As String = cell.CellReference.Value
                cell.CellReference = New StringValue(cellReference.Replace(rowNew.RowIndex.Value.ToString(), newIndex))
            Next
            rowNew.RowIndex = newIndex

            sheetData.InsertBefore(rowNew, rowPosition)

            rowPosition = rowNew
        Next i

        AdjustMergedCells(worksheet.WorksheetPart, rowStart, rowLength)
        'Dim mcells As MergeCells = worksheet.GetFirstChild(Of MergeCells)()
        'mcells.Remove()

    End Sub

    Private Sub AdjustMergedCells(ByVal workSheetPart As WorksheetPart, ByVal rowStart As Integer, ByVal rowLength As Integer)

        Dim MergeCells As List(Of MergeCell) = workSheetPart.Worksheet.Descendants(Of MergeCell)().Where(Function(m) ((GetRowIndex(m.Reference.Value.Split(":"c)(0)) > rowStart) OrElse (GetRowIndex(m.Reference.Value.Split(":"c)(1)) >= rowStart))).ToList()
        Dim strMergeCellStartRow As String, strMergeCellEndRow As String
        Dim iStartRowIndex As Integer = 0, iEndRowIndex As Integer = 0

        For Each mCell As MergeCell In MergeCells
            strMergeCellStartRow = mCell.Reference.Value.Split(":"c)(0)
            strMergeCellEndRow = mCell.Reference.Value.Split(":"c)(1)

            iStartRowIndex = GetRowIndex(strMergeCellStartRow)
            iEndRowIndex = GetRowIndex(strMergeCellEndRow)

            strMergeCellStartRow = strMergeCellStartRow.Replace(iStartRowIndex.ToString(), (iStartRowIndex + rowLength).ToString())

            strMergeCellEndRow = strMergeCellEndRow.Replace(iEndRowIndex.ToString(), (iEndRowIndex + rowLength).ToString())

            mCell.Reference.Value = strMergeCellStartRow & ":" & strMergeCellEndRow

            'If (iStartRowIndex = refRowIndex) AndAlso (iEndRowIndex = refRowIndex) Then
            '    mCell.Remove()
            'Else
            '    If iStartRowIndex > refRowIndex Then
            '        strMergeCellStartRow = strMergeCellStartRow.Replace(iStartRowIndex.ToString(), (iStartRowIndex + 1).ToString())
            '    End If

            '    If iEndRowIndex >= refRowIndex Then
            '        strMergeCellEndRow = strMergeCellEndRow.Replace(iEndRowIndex.ToString(), (iEndRowIndex + 1).ToString())
            '    End If

            '    mCell.Reference.Value = strMergeCellStartRow & ":" & strMergeCellEndRow
            'End If
        Next
    End Sub

    Public Sub AddNewSheet(ByRef Document As SpreadsheetDocument, ByVal SheetName As String)

        'Dim workbookpart As WorkbookPart = Document.AddWorkbookPart()
        'workbookpart.Workbook = New Workbook()

        Dim workbookpart As WorkbookPart = Document.WorkbookPart

        Dim worksheetPart As WorksheetPart = workbookpart.AddNewPart(Of WorksheetPart)()

        Dim sheets As Sheets = Document.WorkbookPart.Workbook.GetFirstChild(Of Sheets)()

        worksheetPart.Worksheet = New Worksheet(New SheetData())

        Dim sheet As New Sheet() With { _
         .Id = Document.WorkbookPart.GetIdOfPart(worksheetPart), _
         .SheetId = workbookpart.WorksheetParts.Count + 1, _
         .Name = SheetName _
        }
        sheets.Append(sheet)

        Dim sheetData As SheetData = worksheetPart.Worksheet.GetFirstChild(Of SheetData)()

        'Dim row As Row
        'row = New Row() With { _
        ' .RowIndex = 2 _
        '}
        'sheetData.Append(row)

        'Dim refCell As Cell = Nothing
        'Dim newCell As New Cell() With { _
        ' .CellReference = "B2" _
        '}
        'row.InsertBefore(newCell, refCell)

        'newCell.CellValue = New CellValue("123")
        'newCell.DataType = New EnumValue(Of CellValues)(CellValues.Number)

    End Sub

    Public Sub InsertLastRow(ByVal worksheetPart As WorksheetPart, ByVal LastRowofSection As Integer)
        Dim sheetData As SheetData = worksheetPart.Worksheet.GetFirstChild(Of SheetData)()
        Dim lastRow As Row = sheetData.Elements(Of Row)().LastOrDefault()
        sheetData.InsertAfter(New Row() With {.RowIndex = (LastRowofSection)}, lastRow)
    End Sub

    'Public Sub CopyRowRange(ByVal workbookPart As WorkbookPart, ByVal sheetIndex As Integer, ByVal srcRowFrom As Integer, ByVal srcRowTo As Integer, ByVal destRowFrom As Integer, ByVal copyTimes As Integer, ByVal srcColFrom As Integer, ByVal srcColTo As Integer)
    '    'only support copy down 
    '    If copyTimes <= 0 OrElse srcRowTo < srcRowFrom OrElse destRowFrom < srcRowFrom Then
    '        Return
    '    End If
    '    Dim destRowFromBase As Integer = destRowFrom

    '    'Get the source sheet to be copied
    '    Dim worksheetPart As WorksheetPart = GetWorksheetPart(workbookPart, sheetIndex)
    '    Dim sheetData As SheetData = worksheetPart.Worksheet.GetFirstChild(Of SheetData)()

    '    'get cells to be cloned according to the specified rows and columns
    '    Dim cells As IList(Of Cell) = sheetData.Descendants(Of Cell)().Where(Function(c) GetRowIndex(c.CellReference) >= srcRowFrom AndAlso GetRowIndex(c.CellReference) <= srcRowTo AndAlso GetCellColIndex(c.CellReference) >= srcColFrom AndAlso GetCellColIndex(c.CellReference) <= srcColTo).ToList(Of Cell)()

    '    'no cells to be cloned
    '    If cells.Count() = 0 Then
    '        Return
    '    End If


    '    'after cloned, the index of rows from destRowFrom should be changed
    '    'diff rows between srcColFrom and srcColTo
    '    Dim copiedRowCount As Integer = srcRowTo - srcRowFrom + 1

    '    'move row index between srcColFrom and srcColTo that the row index greater or equal to 
    '    'destRowFrom
    '    MoveRowIndex(workbookPart, sheetIndex, destRowFrom - 1, copiedRowCount * copyTimes, srcColFrom, srcColTo)

    '    'temporary array of changed row index
    '    Dim changeRowIndexs As IList(Of Integer) = New List(Of Integer)()

    '    'cloned cells, row index/cells
    '    Dim clonedCells As IDictionary(Of Integer, IList(Of Cell)) = Nothing

    '    'formula cells in cloned cells
    '    Dim formulaCells As IList(Of Cell) = New List(Of Cell)()

    '    'cloned related rows for speed
    '    Dim cloneRelatedRows As IList(Of Row) = New List(Of Row)()

    '    For i As Integer = 0 To copyTimes - 1
    '        'next copy col index
    '        destRowFrom = destRowFromBase + copiedRowCount * i
    '        ' changed column index
    '        Dim changedRows As Integer = destRowFrom - srcRowFrom

    '        'add change column index to list
    '        changeRowIndexs.Add(changedRows)
    '        'clear formula cells
    '        formulaCells.Clear()

    '        '1: cloned cells, row index/cell list 
    '        clonedCells = New Dictionary(Of Integer, IList(Of Cell))()

    '        For Each cell As Cell In cells
    '            Dim newCell As Cell = DirectCast(cell.CloneNode(True), Cell)
    '            Dim indexs As Integer() = GetCellIndex(cell.CellReference)
    '            'change row index of cloned cell
    '            Dim rowIndex As Integer = indexs(1) + changedRows
    '            newCell.CellReference = GetColumnName(cell.CellReference) & rowIndex.ToString()

    '            Dim rowCells As IList(Of Cell) = Nothing
    '            If clonedCells.ContainsKey(rowIndex) Then
    '                rowCells = clonedCells(rowIndex)
    '            Else
    '                rowCells = New List(Of Cell)()
    '                clonedCells.Add(rowIndex, rowCells)
    '            End If
    '            rowCells.Add(newCell)

    '            'if is formula cell
    '            If newCell.CellFormula IsNot Nothing AndAlso newCell.CellFormula.Text.Length > 0 Then
    '                formulaCells.Add(newCell)
    '            End If
    '        Next

    '        'add cloned cell into row
    '        For Each rowIndex As Integer In clonedCells.Keys
    '            Dim row As Row = sheetData.Elements(Of Row)().Where(Function(r) r.RowIndex = rowIndex).FirstOrDefault()
    '            'if no this row 
    '            If row Is Nothing Then
    'row = New Row() With { _
    '	Key .RowIndex = CUInt(rowIndex) _
    '}
    '                'find insert position
    '                Dim refRow As Row = sheetData.Elements(Of Row)().Where(Function(r) r.RowIndex > rowIndex).OrderBy(Function(r) r.RowIndex).FirstOrDefault()
    '                If refRow Is Nothing Then
    '                    sheetData.AppendChild(Of Row)(row)
    '                Else
    '                    sheetData.InsertBefore(Of Row)(row, refRow)
    '                End If
    '            End If
    '            row.Append(clonedCells(rowIndex).ToArray())

    '            'add into clone related rows
    '            cloneRelatedRows.Add(row)
    '        Next

    '        '2: processing copied range's formula 
    '        ChangeFormulaRowNumber(formulaCells, changedRows)

    '        '3: copy drawings in range
    '        CopyDrawingsInRange(worksheetPart, srcRowFrom, srcRowTo, srcColFrom, srcColTo, destRowFrom, _
    '         -1)
    '    Next

    '    'sort cells in clone-related rows, SORT IS MOST TIME-SPEND 
    '    For Each row As Row In cloneRelatedRows
    '        ' sort by column name
    '        Dim cs As IList(Of Cell) = row.Elements(Of Cell)().OrderBy(Function(c) GetCellColIndex(c.CellReference.Value)).ToList(Of Cell)()
    '        row.RemoveAllChildren()
    '        row.Append(cs.ToArray())
    '    Next

    '    '2: process merge cell in cloned rows
    '    Dim mcells As MergeCells = worksheetPart.Worksheet.GetFirstChild(Of MergeCells)()
    '    If mcells IsNot Nothing Then
    '        Dim newMergeCells As IList(Of MergeCell) = New List(Of MergeCell)()
    '        Dim clonedMergeCells As IEnumerable(Of MergeCell) = mcells.Elements(Of MergeCell)().Where(Function(m) MergeCellInRange(m, srcRowFrom, srcRowTo, srcColFrom, srcColTo)).ToList(Of MergeCell)()
    '        For Each cmCell As MergeCell In clonedMergeCells
    '            For Each changedRows As Integer In changeRowIndexs
    '                Dim newMergeCell As MergeCell = CreateChangedRowMergeCell(cmCell, changedRows)
    '                newMergeCells.Add(newMergeCell)
    '            Next
    '        Next
    '        Dim count As UInteger = mcells.Count.Value
    '        mcells.Count = New UInt32Value(count + CUInt(newMergeCells.Count))
    '        mcells.Append(newMergeCells.ToArray())
    '    End If

    '    '3: process datavalidate list
    '    Dim validates As IDictionary(Of String, DataValidation) = GetDataValidatesInRange(worksheetPart, srcRowFrom, srcRowTo, srcColFrom, srcColTo)
    '    For Each cellname As String In validates.Keys
    '        For Each changedRows As Integer In changeRowIndexs
    '            AddDataValidateRefItemOfChangedRow(cellname, changedRows, validates(cellname))
    '        Next
    '    Next
    'End Sub

    ''' <summary>
    ''' 獲取指定行或通過指定行COPY後插入一個新行
    ''' </summary>
    ''' <param name="rowIndex"></param>
    ''' <param name="wrksheetPartName"></param>
    ''' <param name="Mod"></param>
    ''' <returns></returns>
    Public Function GetRow(ByRef wrksheetPart As WorksheetPart, ByVal rowIndex As UInteger, ByVal wrksheetPartName As String, ByVal [Mod] As RowMode) As Row
        Dim worksheet As Worksheet = wrksheetPart.Worksheet
        Dim sheetData As SheetData = worksheet.GetFirstChild(Of SheetData)()
        ' If the worksheet does not contain a row with the specified row index, insert one.
        Dim row As Row = Nothing
        If sheetData.Elements(Of Row)().Where(Function(r) rowIndex = r.RowIndex.Value).Count() <> 0 Then
            Dim refRow As Row = sheetData.Elements(Of Row)().Where(Function(r) rowIndex = r.RowIndex.Value).First()
            If (refRow IsNot Nothing) AndAlso ([Mod] = RowMode.Insert) Then
                'Copy row from refRow and insert it
                row = CopyToLine(refRow, rowIndex, sheetData)
                'Update dataValidation (copy drop down list)
                Dim dvs As DataValidations = worksheet.GetFirstChild(Of DataValidations)()
                If dvs IsNot Nothing Then
                    For Each dv As DataValidation In dvs.Descendants(Of DataValidation)()
                        For Each sv As StringValue In dv.SequenceOfReferences.Items
                            sv.Value = sv.Value.Replace(row.RowIndex.ToString(), refRow.RowIndex.ToString())
                        Next
                    Next
                End If
            ElseIf (refRow IsNot Nothing) AndAlso ([Mod] = RowMode.Update) Then
                row = refRow
            Else
                row = New Row() With { _
                 .RowIndex = rowIndex _
                }
                sheetData.Append(row)
            End If
        Else
            row = New Row() With { _
             .RowIndex = rowIndex _
            }
            sheetData.Append(row)
        End If
        Return row
    End Function

    ''' <summary>
    ''' 拷貝源行的數據和格式後,在源行後新插入一行
    ''' </summary>
    ''' <param name="sourceRowIndex">源行行號,以1為開始</param>
    ''' <param name="workSheetName">要操作的workSheetName</param>
    ''' <returns></returns>
    Public Sub InsertRowByCopy(ByRef wrksheetPart As WorksheetPart, ByVal sourceRowIndex As UInteger, ByVal workSheetName As String)
        Dim worksheet As Worksheet = wrksheetPart.Worksheet
        Dim sheetData As SheetData = worksheet.GetFirstChild(Of SheetData)()
        ' If the worksheet does not contain a row with the specified row index, insert one.
        Dim row As Row = Nothing
        If sheetData.Elements(Of Row)().Where(Function(r) sourceRowIndex = r.RowIndex.Value).Count() <> 0 Then
            Dim refRow As Row = sheetData.Elements(Of Row)().Where(Function(r) sourceRowIndex = r.RowIndex.Value).First()
            If (refRow IsNot Nothing) Then
                'Copy row from refRow and insert it
                row = CopyToLine(refRow, sourceRowIndex, sheetData)
                'Update dataValidation (copy drop down list)
                Dim dvs As DataValidations = worksheet.GetFirstChild(Of DataValidations)()
                If dvs IsNot Nothing Then
                    For Each dv As DataValidation In dvs.Descendants(Of DataValidation)()
                        For Each sv As StringValue In dv.SequenceOfReferences.Items
                            sv.Value = sv.Value.Replace(row.RowIndex.ToString(), refRow.RowIndex.ToString())
                        Next
                    Next
                End If
            Else
                'else if ((refRow != null) && (Mod == RowMode.Update))
                '{
                '    row = refRow;
                '}
                row = New Row() With { _
                 .RowIndex = sourceRowIndex _
                }
                sheetData.Append(row)
            End If
        Else
            row = New Row() With { _
             .RowIndex = sourceRowIndex _
            }
            sheetData.Append(row)
        End If
        '儲存
        'excelDoc.WorkbookPart.Workbook.Save()
        'return row;
    End Sub

    ''' <summary>
    ''' Copy an existing row and insert it
    '''We don't need to copy styles of a refRow because a CloneNode() or Clone() methods do it for us
    ''' </summary>
    ''' <param name="refRow"></param>
    ''' <param name="rowIndex"></param>
    ''' <param name="sheetData"></param>
    ''' <returns></returns>
    Friend Function CopyToLine(ByVal refRow As Row, ByVal rowIndex As UInteger, ByVal sheetData As SheetData) As Row
        Dim newRowIndex As UInteger
        Dim newRow = DirectCast(refRow.CloneNode(True), Row)
        ' Loop through all the rows in the worksheet with higher row
        ' index values than the one you just added. For each one,
        ' increment the existing row index.
        Dim rows As IEnumerable(Of Row) = sheetData.Descendants(Of Row)().Where(Function(r) r.RowIndex.Value >= rowIndex)
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

        sheetData.InsertBefore(newRow, refRow)
        Return newRow
    End Function


    'Public Sub InsertRow(ByVal worksheetPart As WorksheetPart, ByVal LastRowofSection As Integer)
    '    Dim sheetData As SheetData = worksheetPart.Worksheet.GetFirstChild(Of SheetData)()
    '    Dim lastRow As Row = sheetData.Elements(Of Row)().FirstOrDefault()
    '    sheetData.InsertAfter(New Row() With {.RowIndex = (LastRowofSection)}, lastRow)
    'End Sub

    'Public Function GetRow(ByVal rowIndex As UInteger, ByVal wrksheetPart As WorksheetPart) As Row
    '    Dim worksheet As Worksheet = wrksheetPart.Worksheet
    '    Dim sheetData As SheetData = worksheet.GetFirstChild(Of SheetData)()
    '    ' If the worksheet does not contain a row with the specified row index, insert one.
    '    Dim row As Row = Nothing
    '    If sheetData.Elements(Of Row)().Where(Function(r) rowIndex = r.RowIndex.Value).Count() <> 0 Then
    '        Dim refRow As Row = sheetData.Elements(Of Row)().Where(Function(r) rowIndex = r.RowIndex.Value).First()
    '        If (refRow IsNot Nothing) Then
    '            'Copy row from refRow and insert it
    '            row = CopyToLine(refRow, rowIndex, sheetData)
    '            'Update dataValidation (copy drop down list)
    '            Dim dvs As DataValidations = worksheet.GetFirstChild(Of DataValidations)()
    '            For Each dv As DataValidation In dvs.Descendants(Of DataValidation)()
    '                For Each sv As StringValue In dv.SequenceOfReferences.Items
    '                    sv.Value = sv.Value.Replace(row.RowIndex.ToString(), refRow.RowIndex.ToString())
    '                Next
    '            Next
    '        ElseIf (refRow IsNot Nothing) Then
    '            row = refRow
    '        Else
    '            row = New Row() With { _
    '             .RowIndex = rowIndex _
    '            }
    '            sheetData.Append(row)
    '        End If
    '    Else
    '        row = New Row() With { _
    '         .RowIndex = rowIndex _
    '        }
    '        sheetData.Append(row)
    '    End If
    '    Return row
    'End Function

    ''Copy an existing row and insert it
    ''We don't need to copy styles of a refRow because a CloneNode() or Clone() methods do it for us
    'Friend Function CopyToLine(ByVal refRow As Row, ByVal rowIndex As UInteger, ByVal sheetData As SheetData) As Row
    '    Dim newRowIndex As UInteger
    '    Dim newRow = DirectCast(refRow.CloneNode(True), Row)
    '    ' Loop through all the rows in the worksheet with higher row 
    '    ' index values than the one you just added. For each one,
    '    ' increment the existing row index.
    '    Dim rows As IEnumerable(Of Row) = sheetData.Descendants(Of Row)().Where(Function(r) r.RowIndex.Value >= rowIndex)
    '    For Each row As Row In rows
    '        newRowIndex = System.Convert.ToUInt32(row.RowIndex.Value + 1)

    '        For Each cell As Cell In row.Elements(Of Cell)()
    '            ' Update the references for reserved cells.
    '            Dim cellReference As String = cell.CellReference.Value
    '            cell.CellReference = New StringValue(cellReference.Replace(row.RowIndex.Value.ToString(), newRowIndex.ToString()))
    '        Next
    '        ' Update the row index.
    '        row.RowIndex = New UInt32Value(newRowIndex)
    '    Next

    '    sheetData.InsertBefore(newRow, refRow)
    '    Return newRow
    'End Function

    'Friend Function CopyToLine(ByVal refRow As Row, ByVal rowIndex As UInteger, ByVal worksheetPart As WorksheetPart) As Row
    '    Dim worksheet As Worksheet = worksheetPart.Worksheet
    '    Dim sheetData As SheetData = worksheet.GetFirstChild(Of SheetData)()
    '    Dim newRowIndex As UInteger
    '    Dim newRow = DirectCast(refRow.CloneNode(True), Row)
    '    ' Loop through all the rows in the worksheet with higher row
    '    ' index values than the one you just added. For each one,
    '    ' increment the existing row index.
    '    Dim rows As IEnumerable(Of Row) = sheetData.Descendants(Of Row)().Where(Function(r) r.RowIndex.Value >= rowIndex)
    '    For Each row As Row In rows
    '        newRowIndex = System.Convert.ToUInt32(row.RowIndex.Value + 1)

    '        For Each cell As Cell In row.Elements(Of Cell)()
    '            ' Update the references for reserved cells.
    '            Dim cellReference As String = cell.CellReference.Value
    '            cell.CellReference = New StringValue(cellReference.Replace(row.RowIndex.Value.ToString(), newRowIndex.ToString()))
    '        Next
    '        ' Update the row index.
    '        row.RowIndex = New UInt32Value(newRowIndex)
    '    Next

    '    sheetData.InsertBefore(newRow, refRow)
    '    Return newRow
    'End Function

    Public Function UpdateValue(ByVal wbPart As WorkbookPart, ByVal sheetName As String, ByVal addressName As String, ByVal value As String, ByVal styleIndex As UInt32Value, ByVal isString As Boolean) As Boolean
        ' Assume failure.
        Dim updated As Boolean = False

        Dim sheet As Sheet = wbPart.Workbook.Descendants(Of Sheet)().Where(Function(s) s.Name = sheetName).FirstOrDefault()

        If sheet IsNot Nothing Then
            Dim ws As Worksheet = DirectCast(wbPart.GetPartById(sheet.Id), WorksheetPart).Worksheet
            Dim cell As Cell = InsertCellInWorksheet(ws, addressName)

            If isString Then
                ' Either retrieve the index of an existing string,
                ' or insert the string into the shared string table
                ' and get the index of the new item.
                Dim stringIndex As Integer = InsertSharedStringItem(wbPart, value)

                cell.CellValue = New CellValue(stringIndex.ToString())
                cell.DataType = New EnumValue(Of CellValues)(CellValues.SharedString)
            Else
                cell.CellValue = New CellValue(value)
                cell.DataType = New EnumValue(Of CellValues)(CellValues.Number)
            End If

            If styleIndex.Value > 0 Then
                cell.StyleIndex = styleIndex
            End If

            ' Save the worksheet.
            'ws.Save()
            updated = True
        End If

        Return updated
    End Function

    ' Given a Worksheet and an address (like "AZ254"), either return a 
    ' cell reference, or create the cell reference and return it.
    Public Function InsertCellInWorksheet(ByVal ws As Worksheet, ByVal addressName As String) As Cell
        Dim sheetData As SheetData = ws.GetFirstChild(Of SheetData)()
        Dim cell As Cell = Nothing

        Dim rowNumber As UInt32 = GetRowIndex(addressName)
        Dim row As Row = GetRow(sheetData, rowNumber)

        ' If the cell you need already exists, return it.
        ' If there is not a cell with the specified column name, insert one.  
        Dim refCell As Cell = row.Elements(Of Cell)().Where(Function(c) c.CellReference.Value = addressName).FirstOrDefault()
        If refCell IsNot Nothing Then
            cell = refCell
        Else
            cell = CreateCell(row, addressName)
        End If
        Return cell
    End Function

    ' Given the main workbook part, and a text value, insert the text into 
    ' the shared string table. Create the table if necessary. If the value 
    ' already exists, return its index. If it doesn't exist, insert it and 
    ' return its new index.
    Public Function InsertSharedStringItem(ByVal wbPart As WorkbookPart, ByVal value As String) As Integer
        Dim index As Integer = 0
        Dim found As Boolean = False
        Dim stringTablePart = wbPart.GetPartsOfType(Of SharedStringTablePart)().FirstOrDefault()

        ' If the shared string table is missing, something's wrong.
        ' Just return the index that you found in the cell.
        ' Otherwise, look up the correct text in the table.
        If stringTablePart Is Nothing Then
            ' Create it.
            stringTablePart = wbPart.AddNewPart(Of SharedStringTablePart)()
        End If

        Dim stringTable = stringTablePart.SharedStringTable
        If stringTable Is Nothing Then
            stringTable = New SharedStringTable()
        End If

        ' Iterate through all the items in the SharedStringTable. 
        ' If the text already exists, return its index.
        For Each item As SharedStringItem In stringTable.Elements(Of SharedStringItem)()
            If item.InnerText = value Then
                found = True
                Exit For
            End If
            index += 1
        Next

        If Not found Then
            stringTable.AppendChild(New SharedStringItem(New Text(value)))
            stringTable.Save()
        End If

        Return index
    End Function

    ' Add a cell with the specified address to a row.
    Public Function CreateCell(ByVal row As Row, ByVal address As [String]) As Cell
        Dim cellResult As Cell
        Dim refCell As Cell = Nothing

        ' Cells must be in sequential order according to CellReference. 
        ' Determine where to insert the new cell.
        For Each cell As Cell In row.Elements(Of Cell)()
            If String.Compare(cell.CellReference.Value, address, True) > 0 Then
                refCell = cell
                Exit For
            End If
        Next

        cellResult = New Cell()
        cellResult.CellReference = address

        row.InsertBefore(cellResult, refCell)
        Return cellResult
    End Function

    ' Return the row at the specified rowIndex located within
    ' the sheet data passed in via wsData. If the row does not
    ' exist, create it.
    Public Function GetRow(ByVal wsData As SheetData, ByVal rowIndex As UInt32) As Row
        Dim row = wsData.Elements(Of Row)().Where(Function(r) r.RowIndex.Value = rowIndex).FirstOrDefault()
        If row Is Nothing Then
            row = New Row()
            row.RowIndex = rowIndex
            wsData.Append(row)
        End If
        Return row
    End Function

    ' Given an Excel address such as E5 or AB128, GetRowIndex
    ' parses the address and returns the row index.
    Public Function GetRowIndex(ByVal address As String) As UInt32
        Dim rowPart As String
        Dim l As UInt32
        Dim result As UInt32 = 0

        For i As Integer = 0 To address.Length - 1
            If UInt32.TryParse(address.Substring(i, 1), l) Then
                rowPart = address.Substring(i, address.Length - i)
                If UInt32.TryParse(rowPart, l) Then
                    result = l
                    Exit For
                End If
            End If
        Next
        Return result
    End Function

    'Public Function UpdateValue(ByVal wbPart As WorkbookPart, ByVal sheetName As String, ByVal addressName As String, ByVal value As String, ByVal styleIndex As Integer, ByVal isString As Boolean) As Boolean
    '    ' Assume failure.
    '    Dim updated As Boolean = False

    '    Dim sheet As Sheet = wbPart.Workbook.Descendants(Of Sheet)().Where(Function(s) s.Name = SheetName).FirstOrDefault()

    '    If sheet IsNot Nothing Then
    '        Dim ws As Worksheet = DirectCast(wbPart.GetPartById(sheet.Id), WorksheetPart).Worksheet
    '        Dim cell As Cell = InsertCellInWorksheet(ws, addressName)

    '        If isString Then
    '            ' Either retrieve the index of an existing string,
    '            ' or insert the string into the shared string table
    '            ' and get the index of the new item.
    '            Dim stringIndex As Integer = InsertSharedStringItem(wbPart, value)

    '            cell.CellValue = New CellValue(stringIndex.ToString())
    '            cell.DataType = New EnumValue(Of CellValues)(CellValues.SharedString)
    '        Else
    '            cell.CellValue = New CellValue(value)
    '            cell.DataType = New EnumValue(Of CellValues)(CellValues.Number)
    '        End If

    '        'If styleIndex > 0 Then
    '        '    cell.StyleIndex = styleIndex
    '        'End If

    '        If styleIndex > 0 Then
    '            cell.StyleIndex = styleIndex
    '        End If

    '        ' Save the worksheet.
    '        ws.Save()
    '        updated = True
    '    End If

    '    Return updated
    'End Function

    '' Given the main workbook part, and a text value, insert the text into the shared
    '' string table. Create the table if necessary. If the value already exists, return
    '' its index. If it doesn't exist, insert it and return its new index.
    'Private Function InsertSharedStringItem(ByVal wbPart As WorkbookPart, ByVal value As String) As Integer
    '    Dim index As Integer = 0
    '    Dim found As Boolean = False
    '    Dim stringTablePart = wbPart.GetPartsOfType(Of SharedStringTablePart)().FirstOrDefault()

    '    ' If the shared string table is missing, something's wrong.
    '    ' Just return the index that you found in the cell.
    '    ' Otherwise, look up the correct text in the table.
    '    If stringTablePart Is Nothing Then
    '        ' Create it.
    '        stringTablePart = wbPart.AddNewPart(Of SharedStringTablePart)()
    '    End If

    '    Dim stringTable = stringTablePart.SharedStringTable
    '    If stringTable Is Nothing Then
    '        stringTable = New SharedStringTable()
    '    End If

    '    ' Iterate through all the items in the SharedStringTable. If the text already exists, return its index.
    '    For Each item As SharedStringItem In stringTable.Elements(Of SharedStringItem)()
    '        If item.InnerText = value Then
    '            found = True
    '            Exit For
    '        End If
    '        index += 1
    '    Next

    '    If Not found Then
    '        stringTable.AppendChild(New SharedStringItem(New Text(value)))
    '        stringTable.Save()
    '    End If

    '    Return index
    'End Function

    ' Used to force a recalc of cells containing formulas. The
    ' CellValue has a cached value of the evaluated formula. This
    ' will prevent Excel from recalculating the cell even if 
    ' calculation is set to automatic.
    Private Function RemoveCellValue(ByVal wbPart As WorkbookPart, ByVal sheetName As String, ByVal addressName As String) As Boolean
        Dim returnValue As Boolean = False

        Dim sheet As Sheet = wbPart.Workbook.Descendants(Of Sheet)().Where(Function(s) s.Name = sheetName).FirstOrDefault()
        If sheet IsNot Nothing Then
            Dim ws As Worksheet = DirectCast(wbPart.GetPartById(sheet.Id), WorksheetPart).Worksheet
            Dim cell As Cell = InsertCellInWorksheet(ws, addressName)

            ' If there is a cell value, remove it to force a recalc
            ' on this cell.
            If cell.CellValue IsNot Nothing Then
                cell.CellValue.Remove()
            End If

            ' Save the worksheet.
            'ws.Save()
            returnValue = True
        End If

        Return returnValue
    End Function

    'Public Function ReadCell(ByVal sheetName As String, ByVal cellCoordinates As String) As CellValue
    '    Using excelDoc As SpreadsheetDocument = SpreadsheetDocument.Open(_filePath, False)
    '        Dim cell As Cell = GetCell(excelDoc, sheetName, cellCoordinates)
    '        Return cell.CellValue
    '    End Using
    'End Function

    'Public Sub UpdateCell(ByVal sheetName As String, ByVal cellCoordinates As String, ByVal cellValue As Object)
    '    Using excelDoc As SpreadsheetDocument = SpreadsheetDocument.Open(_filePath, True)
    '        ' tell Excel to recalculate formulas next time it opens the doc
    '        excelDoc.WorkbookPart.Workbook.CalculationProperties.ForceFullCalculation = True
    '        excelDoc.WorkbookPart.Workbook.CalculationProperties.FullCalculationOnLoad = True

    '        Dim worksheetPart As WorksheetPart = GetWorksheetPart(excelDoc, sheetName)
    '        Dim cell As Cell = GetCell(worksheetPart, cellCoordinates)
    '        cell.CellValue = New CellValue(cellValue.ToString())
    '        worksheetPart.Worksheet.Save()
    '    End Using
    'End Sub

    Public Function GetWorksheetPart(ByVal excelDoc As SpreadsheetDocument, ByVal sheetName As String) As WorksheetPart
        Dim sheet As Sheet = excelDoc.WorkbookPart.Workbook.Descendants(Of Sheet)().SingleOrDefault(Function(s) s.Name = sheetName)
        If sheet Is Nothing Then
            Throw New ArgumentException([String].Format("No sheet named {0} found in spreadsheet", sheetName), "sheetName")
        End If
        Return DirectCast(excelDoc.WorkbookPart.GetPartById(sheet.Id), WorksheetPart)
    End Function

    Public Function GetCell(ByVal excelDoc As SpreadsheetDocument, ByVal sheetName As String, ByVal cellCoordinates As String) As Cell
        Dim worksheetPart As WorksheetPart = GetWorksheetPart(excelDoc, sheetName)
        Return GetCell(worksheetPart, cellCoordinates)
    End Function

    Public Function GetCell(ByVal worksheetPart As WorksheetPart, ByVal cellCoordinates As String) As Cell
        Dim rowIndex As Integer = Integer.Parse(cellCoordinates.Substring(1))
        Dim row As Row = GetRow(worksheetPart, rowIndex)

        Dim cell As Cell = row.Elements(Of Cell)().FirstOrDefault(Function(c) cellCoordinates.Equals(c.CellReference.Value))
        If cell Is Nothing Then
            Throw New ArgumentException([String].Format("Cell {0} not found in spreadsheet", cellCoordinates))
        End If
        Return cell
    End Function

    Public Function GetRow(ByVal worksheetPart As WorksheetPart, ByVal rowIndex As Integer) As Row
        Dim row As Row = worksheetPart.Worksheet.GetFirstChild(Of SheetData)().Elements(Of Row)().FirstOrDefault(Function(r) r.RowIndex.Value = rowIndex)
        If row Is Nothing Then
            Throw New ArgumentException([String].Format("No row with index {0} found in spreadsheet", rowIndex))
        End If
        Return row
    End Function

    Private Function InsertCellInWorksheet(ByVal columnName As String, ByVal rowIndex As UInteger, ByVal worksheetPart As WorksheetPart) As Cell
        Dim worksheet As Worksheet = worksheetPart.Worksheet
        Dim sheetData As SheetData = worksheet.GetFirstChild(Of SheetData)()
        Dim cellReference As String = (columnName + rowIndex.ToString())

        ' If the worksheet does not contain a row with the specified row index, insert one.
        Dim row As Row
        If (sheetData.Elements(Of Row).Where(Function(r) r.RowIndex.Value = rowIndex).Count() <> 0) Then
            row = sheetData.Elements(Of Row).Where(Function(r) r.RowIndex.Value = rowIndex).First()
        Else
            row = New Row()
            row.RowIndex = rowIndex
            sheetData.Append(row)
        End If

        ' If there is not a cell with the specified column name, insert one.  
        If (row.Elements(Of Cell).Where(Function(c) c.CellReference.Value = columnName + rowIndex.ToString()).Count() > 0) Then
            Return row.Elements(Of Cell).Where(Function(c) c.CellReference.Value = cellReference).First()
        Else
            ' Cells must be in sequential order according to CellReference. Determine where to insert the new cell.
            Dim refCell As Cell = Nothing
            For Each cell As Cell In row.Elements(Of Cell)()
                If (String.Compare(cell.CellReference.Value, cellReference, True) > 0) Then
                    refCell = cell
                    Exit For
                End If
            Next
            Dim newCell As Cell = New Cell
            newCell.CellReference = cellReference

            row.InsertBefore(newCell, refCell)
            worksheet.Save()
            Return newCell
        End If
    End Function
End Module
