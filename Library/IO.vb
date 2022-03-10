Imports System.IO

Namespace Library
    Public Class IO
        <ScriptFunction("fopen")>
        Public Shared Function OpenFileStream(rt As Runtime, fn As String) As Object
            If (Environment.HasAccess(fn)) Then
                If (File.Exists(fn)) Then
                    Return IO.OpenFileStream(fn, False)
                Else
                    Return IO.OpenFileStream(fn, True)
                End If
            End If
            Return TValue.Null
        End Function

        Private Shared Function OpenFileStream(fn As String, createNew As Boolean) As FileStream
            Return If(createNew, File.Open(fn, FileMode.Create), File.Open(fn, FileMode.Open))
        End Function

        <ScriptFunction("fclose")>
        Public Shared Sub CloseFileStream(rt As Runtime, str As FileStream)
            If (str IsNot Nothing) Then str.Close()
        End Sub

        <ScriptFunction("fread")>
        Public Shared Function ReadFileStream(rt As Runtime, str As FileStream, offset As Integer, length As Integer) As String
            If (str IsNot Nothing) Then
                Dim buffer() As Byte = New Byte(CInt(str.Length - 1)) {}
                str.Read(buffer, offset, If(length = -1, buffer.Length, length))
                Return Environment.Encoder.GetString(buffer)
            End If
            Return String.Empty
        End Function

        <ScriptFunction("fwrite")>
        Public Shared Sub WriteFileStream(rt As Runtime, str As FileStream, input As Object)
            If (str IsNot Nothing) Then
                Dim buffer() As Byte = Environment.Encoder.GetBytes(input.ToString)
                str.Write(buffer, 0, buffer.Length)
            End If
        End Sub

        <ScriptFunction("fpos")>
        Public Shared Sub SetStreamPosition(rt As Runtime, str As FileStream, value As Integer)
            If (str IsNot Nothing) Then
                If (str.CanSeek) Then
                    str.Position = value
                End If
            End If
        End Sub

        <ScriptFunction("isfile")>
        Public Shared Function FileExist(rt As Runtime, fn As String) As Boolean
            Return File.Exists(fn)
        End Function

        <ScriptFunction("isdir")>
        Public Shared Function DirectoryExists(rt As Runtime, fn As String) As Boolean
            Return Directory.Exists(fn)
        End Function

        <ScriptFunction("getfolder")>
        Public Shared Function GetFoldername(rt As Runtime, fn As String) As String
            Return New FileInfo(fn).Directory.FullName
        End Function

        <ScriptFunction("delete")>
        Public Shared Function DeleteFilename(rt As Runtime, fn As String) As Boolean
            If (File.Exists(fn)) Then
                File.Delete(fn)
                Return True
            End If
            Return False
        End Function
    End Class
End Namespace