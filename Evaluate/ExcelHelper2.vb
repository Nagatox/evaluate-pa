
Imports DocumentFormat.OpenXml
Imports DocumentFormat.OpenXml.Packaging
Imports DocumentFormat.OpenXml.Spreadsheet
Imports System.Linq

Module ExcelHelper2
    Public Sub InsertRow2(SpreadsheetDocument As SpreadsheetDocument, SheetName As String, rowIndex As UInteger)
        Dim workbookPart As WorkbookPart = SpreadsheetDocument.WorkbookPart
        Dim sheet As Sheet = workbookPart.Workbook.Descendants(Of Sheet)().Where(Function(s) s.Name = SheetName).FirstOrDefault()

        Dim worksheet As Worksheet = DirectCast(workbookPart.GetPartById(sheet.Id), WorksheetPart).Worksheet

        Dim sheetData As SheetData = worksheet.GetFirstChild(Of SheetData)()

        ' สร้างแถวใหม่
        Dim newRow As New Row() With {.RowIndex = rowIndex}

        ' เพิ่มเซลล์ (ตัวอย่าง: A1, B1, C1)
        newRow.Append(
            CreateCell("A", rowIndex, "Value1", CellValues.String),
            CreateCell("B", rowIndex, "Value2", CellValues.String),
            CreateCell("C", rowIndex, "100", CellValues.Number)
        )

        ' แทรกแถวใหม่ในตำแหน่งที่ถูกต้อง
        InsertRowx(sheetData, newRow)

        worksheet.Save()

    End Sub

    Private Function CreateCell(columnName As String, rowIndex As UInteger, value As String, dataType As CellValues) As Cell
        Return New Cell() With {
            .CellReference = columnName & rowIndex,
            .DataType = dataType,
            .CellValue = New CellValue(value)
        }
    End Function

    Private Sub InsertRowx(sheetData As SheetData, newRow As Row)
        ' หากชีตว่าง ให้เพิ่มเข้าไปเลย
        If Not sheetData.Elements(Of Row)().Any() Then
            sheetData.AppendChild(newRow)
            Return
        End If

        ' หาตำแหน่งที่ต้องการแทรก
        Dim refRow As Row = Nothing
        For Each row As Row In sheetData.Elements(Of Row)()
            If row.RowIndex.Value > newRow.RowIndex.Value Then
                refRow = row
                Exit For
            End If
        Next

        ' แทรกแถวใหม่
        If refRow IsNot Nothing Then
            sheetData.InsertBefore(newRow, refRow)
        Else
            sheetData.AppendChild(newRow)
        End If
    End Sub

End Module
