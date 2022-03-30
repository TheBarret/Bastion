Imports System.IO
Imports System.Text
Imports Newtonsoft.Json

Public Class Serialization
    Public Shared Function Pack(fn As String, target As Object) As Boolean
        Try
            Using sw As New StreamWriter(fn, False, New UTF8Encoding(False))
                Dim json = JsonConvert.SerializeObject(target, Formatting.Indented)
                sw.Write(json)
            End Using
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function
End Class
