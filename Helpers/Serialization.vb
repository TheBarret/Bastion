Imports System.IO
Imports Newtonsoft.Json

Public Class Serialization
    Public Function Pack(fn As String, target As Object) As Boolean
        Try
            Using fs As New BinaryWriter(File.OpenWrite(fn), Environment.Encoder)
                fs.Write(JsonConvert.SerializeObject(target, Formatting.Indented))
            End Using
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function
End Class
