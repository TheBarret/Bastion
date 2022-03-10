Imports System.IO
Imports System.Text

Namespace Helpers
    Public Class Logger
        Inherits TextWriter
        Implements IDisposable
        Public Property Filename As String
        Public Property Encoder As Encoding
        Public Property Endpoint As FileStream

        Sub New()
            Me.Encoder = Environment.Encoder
            Me.Filename = Environment.Logfile
            Me.Validate()
            Me.Endpoint = File.OpenWrite(Me.Filename)
        End Sub

        Sub New(encoding As Encoding)
            Me.Encoder = encoding
        End Sub

        Public Overrides Sub Write(value As Object)
            Me.WriteDebug(value.ToString)
        End Sub

        Public Overrides Sub WriteLine(value As Object)
            Me.WriteDebug(value.ToString)
        End Sub

        Public Overrides Sub Write(value As String)
            Me.WriteDebug(value)
        End Sub

        Public Overrides Sub WriteLine(value As String)
            Me.WriteDebug(value)
        End Sub

        Private Sub Validate()
            If (File.Exists(Me.Filename)) Then File.Delete(Me.Filename)
        End Sub

        Private Sub WriteDebug(message As String)
            If (Me.Endpoint.CanWrite) Then
                message = String.Format("[{0}] {1}{2}", DateTime.Now.ToString("MM-dd-yy HH:mm:ss"), message, ControlChars.CrLf)
                Dim buffer() As Byte = Me.Encoding.GetBytes(message)
                Me.Endpoint.Write(buffer, 0, buffer.Length)
                Me.Endpoint.Flush()
            End If
        End Sub

        Private Property disposedValue As Boolean
        Protected Overloads Sub Dispose(disposing As Boolean)
            If Not disposedValue Then
                If disposing Then
                    Me.Endpoint.Close()
                End If
                Me.disposedValue = True
            End If
        End Sub

        Public Overrides ReadOnly Property Encoding As Encoding
            Get
                Return Me.Encoder
            End Get
        End Property
    End Class
End Namespace