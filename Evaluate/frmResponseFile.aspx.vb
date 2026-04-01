Imports System.Drawing
Imports System.IO

Partial Public Class frmResponseFile
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim img = Request.QueryString("img")
        Try

        
            Dim filepath As String = "C:\Work\Evaluate\PhantomJS\SaveImage\" & img & ".png"
            If File.Exists(filepath) Then
                Response.ContentType = "application/octet-stream"
                Response.AddHeader("Content-Disposition", "filename=" & img & ".png")

                'Dim MyFileStream As FileStream
                'Dim FileSize As Long

                'MyFileStream = New FileStream(filepath, FileMode.Open)

                'FileSize = MyFileStream.Length

                'Dim Buffer(CInt(FileSize)) As Byte
                'MyFileStream.Read(Buffer, 0, CInt(FileSize))
                'MyFileStream.Close()

                ''Response.ContentType = "application/pdf"
                'Response.OutputStream.Write(Buffer, 0, FileSize)
                'Response.Flush()
                'Response.Close()

                Using image As New Bitmap("C:\Work\Evaluate\PhantomJS\SaveImage\" & img & ".png")
                    Using ms As New MemoryStream()
                        image.Save(ms, System.Drawing.Imaging.ImageFormat.Png)
                        ms.WriteTo(Context.Response.OutputStream)
                    End Using
                End Using
                If File.Exists(filepath) Then File.Delete(filepath)
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

End Class