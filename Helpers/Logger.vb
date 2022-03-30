Imports System.IO
Imports System.Text

Namespace Helpers
    Public Class Logger
        Inherits TextWriter
        Implements IDisposable
        Public Property Name As String
        Public Property Filename As String
        Public Property Encoder As Encoding

        Sub New(session As Session)
            Me.Name = session.Name
            Me.Encoder = Environment.Encoder
            Me.Filename = Environment.Log
            If (File.Exists(Me.Filename)) Then
                File.Delete(Me.Filename)
            End If
        End Sub

        Sub New(encoding As Encoding)
            Me.Encoder = encoding
        End Sub

        Public Overrides Sub Write(value As Object)
            Me.WriteDebug(value.ToString)
        End Sub

        Public Overrides Sub Write(value As String)
            Me.WriteDebug(value)
        End Sub

        Public Overrides Sub Write(value As Char)
            Me.WriteDebug(value)
        End Sub

        Public Overrides Sub Write(value As Char())
            Me.WriteDebug(value)
        End Sub

        Public Overrides Sub Write(value As Integer)
            Me.WriteDebug(value)
        End Sub

        Public Overrides Sub Write(value As Double)
            Me.WriteDebug(value)
        End Sub

        Public Overrides Sub Write(value As Boolean)
            Me.WriteDebug(value)
        End Sub

        Public Overrides Sub WriteLine(value As Object)
            Me.WriteDebug(value.ToString)
        End Sub

        Public Overrides Sub WriteLine(value As String)
            Me.WriteDebug(value)
        End Sub

        Public Overrides Sub WriteLine(value As Char)
            Me.WriteDebug(value)
        End Sub

        Public Overrides Sub WriteLine(value As Char())
            Me.WriteDebug(value)
        End Sub

        Public Overrides Sub WriteLine(value As Integer)
            Me.WriteDebug(value)
        End Sub

        Public Overrides Sub WriteLine(value As Double)
            Me.WriteDebug(value)
        End Sub

        Public Overrides Sub WriteLine(value As Boolean)
            Me.WriteDebug(value)
        End Sub

        Private Sub WriteDebug(message As String)
            Using sw As New StreamWriter(File.Open(Me.Filename, FileMode.Append, FileAccess.Write, FileShare.Read), Me.Encoder)
                sw.Write(String.Format("[{0}]", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")))
                sw.Write(message)
                sw.Write(ControlChars.CrLf)
            End Using
        End Sub

        Public Overrides ReadOnly Property Encoding As Encoding
            Get
                Return Me.Encoder
            End Get
        End Property
    End Class
End Namespace