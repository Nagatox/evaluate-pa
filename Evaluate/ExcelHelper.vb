Imports DocumentFormat.OpenXml
Imports DocumentFormat.OpenXml.Packaging
Imports DocumentFormat.OpenXml.Spreadsheet
Imports System.Linq
Imports System.Text.RegularExpressions

Module ExcelHelper
    Public Sub InsertRowWithFormulaAdjustment(spreadsheetDocument As SpreadsheetDocument, SheetName As String, insertAfterRowIndex As Integer)
        Dim workbookPart As WorkbookPart = spreadsheetDocument.WorkbookPart
        Dim sheet As Sheet = workbookPart.Workbook.Descendants(Of Sheet)().Where(Function(s) s.Name = SheetName).FirstOrDefault()

        Dim worksheet As Worksheet = DirectCast(workbookPart.GetPartById(sheet.Id), WorksheetPart).Worksheet

        Dim sheetData As SheetData = worksheet.GetFirstChild(Of SheetData)()

        ' ค้นหาแถวที่ต้องการแทรกหลังจาก
        Dim referenceRow As Row = sheetData.Elements(Of Row)().FirstOrDefault(Function(r) r.RowIndex.Value = insertAfterRowIndex)

        If referenceRow Is Nothing Then
            Throw New ArgumentException("ไม่พบแถวที่ระบุ")
        End If

        ' สร้างแถวใหม่
        Dim newRowIndex As Integer = insertAfterRowIndex + 1
        Dim newRow As New Row() With {.RowIndex = newRowIndex}

        ' คัดลอกเซลล์จากแถวอ้างอิงและปรับสูตร
        For Each refCell As Cell In referenceRow.Elements(Of Cell)()
            Dim newCell As Cell = CopyCellWithAdjustedFormula(refCell, newRowIndex)
            newRow.Append(newCell)
        Next

        ' ปรับดัชนีแถวทั้งหมดที่อยู่หลังแถวที่แทรก
        AdjustRowIndexes(sheetData, newRowIndex)

        ' แทรกแถวใหม่
        sheetData.InsertAfter(newRow, referenceRow)

        worksheet.Save()
    End Sub

    Private Function CopyCellWithAdjustedFormula(originalCell As Cell, newRowIndex As Integer) As Cell
        Dim newCell As New Cell()

        ' คัดลอก properties พื้นฐาน
        newCell.DataType = originalCell.DataType
        newCell.StyleIndex = originalCell.StyleIndex

        ' ปรับ cell reference
        Dim originalReference As String = originalCell.CellReference.Value
        Dim columnName As String = GetColumnName(originalReference)
        newCell.CellReference = columnName & newRowIndex

        ' ปรับสูตรหากมี
        If originalCell.CellFormula IsNot Nothing Then
            Dim originalFormula As String = originalCell.CellFormula.Text
            Dim adjustedFormula As String = AdjustFormula(originalFormula, newRowIndex)

            newCell.CellFormula = New CellFormula(adjustedFormula)

            ' คัดลอกค่า (อาจเป็นผลลัพธ์ของสูตร)
            If originalCell.CellValue IsNot Nothing Then
                newCell.CellValue = New CellValue(originalCell.CellValue.Text)
            End If
        Else
            ' คัดลอกค่าปกติ
            If originalCell.CellValue IsNot Nothing Then
                newCell.CellValue = New CellValue(originalCell.CellValue.Text)
            End If
        End If

        Return newCell
    End Function

    Private Function AdjustFormula(originalFormula As String, newRowIndex As Integer) As String
        ' รูปแบบ regex สำหรับค้นหา cell references ในสูตร
        Dim pattern As String = "([A-Z]+)(\d+)"
        Dim regex As New Regex(pattern)

        Return regex.Replace(originalFormula, Function(match)
                                                  Dim column As String = match.Groups(1).Value
                                                  Dim row As Integer = Integer.Parse(match.Groups(2).Value)

                                                  ' ปรับแถวในสูตร (เพิ่ม 1 สำหรับทุก cell reference)
                                                  Dim adjustedRow As Integer = row + 1

                                                  ' ถ้าเป็น absolute reference ($) ไม่ต้องปรับ
                                                  If originalFormula.Contains("$" & match.Groups(2).Value) Then
                                                      Return match.Value
                                                  End If

                                                  Return column & adjustedRow
                                              End Function)
    End Function

    Private Function GetColumnName(cellReference As String) As String
        Dim regex As New Regex("[A-Za-z]+")
        Return regex.Match(cellReference).Value
    End Function

    Private Sub AdjustRowIndexes(sheetData As SheetData, insertedRowIndex As Integer)
        For Each row As Row In sheetData.Elements(Of Row)()
            If row.RowIndex.Value >= insertedRowIndex Then
                ' ปรับดัชนีแถวทั้งหมดที่อยู่หลังแถวที่แทรก
                row.RowIndex.Value += 1

                ' ปรับ cell references ในแถว
                For Each cell As Cell In row.Elements(Of Cell)()
                    Dim columnName As String = GetColumnName(cell.CellReference.Value)
                    cell.CellReference = columnName '& row.RowIndex

                    ' ปรับสูตรในเซลล์
                    If cell.CellFormula IsNot Nothing Then
                        Dim originalFormula As String = cell.CellFormula.Text
                        Dim adjustedFormula As String = AdjustFormulaForRowChange(originalFormula, insertedRowIndex)
                        cell.CellFormula.Text = adjustedFormula
                    End If
                Next
            End If
        Next
    End Sub

    Private Function AdjustFormulaForRowChange(originalFormula As String, insertedRowIndex As UInteger) As String
        Dim pattern As String = "([A-Z]+)(\d+)"
        Dim regex As New Regex(pattern)

        Return regex.Replace(originalFormula, Function(match)
                                                  Dim column As String = match.Groups(1).Value
                                                  Dim row As Integer = Integer.Parse(match.Groups(2).Value)

                                                  ' ปรับแถวในสูตรหากอยู่หลังแถวที่แทรก
                                                  If row >= insertedRowIndex Then
                                                      Return column & (row + 1)
                                                  End If

                                                  Return match.Value
                                              End Function)
    End Function


#If 0 Then


    Public Sub InsertSingleRowWithFormula(filePath As String, sourceRowIndex As UInteger, targetRowIndex As UInteger)
        Using spreadsheetDocument As SpreadsheetDocument = SpreadsheetDocument.Open(filePath, True)
            Dim workbookPart As WorkbookPart = spreadsheetDocument.WorkbookPart
            Dim worksheetPart As WorksheetPart = workbookPart.WorksheetParts.First()
            Dim sheetData As SheetData = worksheetPart.Worksheet.GetFirstChild(Of SheetData)()

            ' ค้นหาแถวต้นแบบ
            Dim sourceRow As Row = sheetData.Elements(Of Row)().FirstOrDefault(Function(r) r.RowIndex = sourceRowIndex)

            If sourceRow Is Nothing Then
                Throw New ArgumentException("ไม่พบแถวต้นแบบ")
            End If

            ' สร้างแถวใหม่
            Dim newRow As New Row() With {.RowIndex = targetRowIndex}

            ' คัดลอกและปรับสูตร
            For Each sourceCell As Cell In sourceRow.Elements(Of Cell)()
                Dim newCell As Cell = CreateCellFromTemplate(sourceCell, targetRowIndex)
                newRow.Append(newCell)
            Next

            ' แทรกแถว
            InsertRowAtPosition(sheetData, newRow)

            worksheetPart.Worksheet.Save()
        End Using
    End Sub

    Private Function CreateCellFromTemplate(templateCell As Cell, newRowIndex As UInteger) As Cell
        Dim newCell As New Cell()

        ' คัดลอก properties
        newCell.DataType = templateCell.DataType
        newCell.StyleIndex = templateCell.StyleIndex

        ' ปรับ cell reference
        Dim columnName As String = Regex.Match(templateCell.CellReference.Value, "[A-Z]+").Value
        newCell.CellReference = columnName & newRowIndex

        ' ปรับสูตร
        If templateCell.CellFormula IsNot Nothing Then
            Dim originalFormula As String = templateCell.CellFormula.Text
            Dim adjustedFormula As String = originalFormula.Replace("=" & templateCell.CellReference.Value, "=" & newCell.CellReference.Value)

            ' ปรับสูตรอื่นๆ
            adjustedFormula = AdjustSingleFormula(adjustedFormula, newRowIndex)

            newCell.CellFormula = New CellFormula(adjustedFormula)
        End If

        ' คัดลอกค่า
        If templateCell.CellValue IsNot Nothing Then
            newCell.CellValue = New CellValue(templateCell.CellValue.Text)
        End If

        Return newCell
    End Function

    Private Function AdjustSingleFormula(formula As String, newRowIndex As UInteger) As String
        ' ปรับสูตรแบบง่ายๆ
        Return formula.Replace("=C" & (newRowIndex - 1), "=C" & newRowIndex)
        .Replace("=B" & (newRowIndex - 1), "=B" & newRowIndex)
        .Replace("=A" & (newRowIndex - 1), "=A" & newRowIndex)
    End Function
#End If
End Module