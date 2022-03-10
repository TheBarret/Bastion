Imports System.IO
Imports System.Reflection
Imports Bastion.Helpers

Public Class Sessions
    Implements IDisposable
    Public Property Parent As Runtime
    Public Property Level As Integer
    Public Property Debug As Boolean
    Public Property Logger As Logger
    Public Property Timer As Stopwatch
    Public Property Context As String

    Sub New(rt As Runtime, context As String)
        Me.Parent = rt
        Me.Level = 1
        Me.Context = context
        Me.Debug = True
        Me.Timer = New Stopwatch
        Me.Logger = New Logger
        Me.Timer.Start()
    End Sub

    Public Sub Log(message As String)
        If (Me.Debug) Then
            Dim origin As MethodBase = Me.GetCurrentExecutionMethod
            Me.Logger.WriteLine(String.Format("[{0}] [{1}.{2}] {3}", Me.Level, origin.DeclaringType.Name, origin.Name, message))
        End If
    End Sub

    Public Function GetCurrentExecutionMethod() As MethodBase
        Return New StackTrace().GetFrame(2).GetMethod()
    End Function

    Public Sub Enter()
        Me.Level += 1
    End Sub

    Public Sub Leave()
        Me.Level -= 1
    End Sub

    Public ReadOnly Property HasContext As Boolean
        Get
            Return Me.Context.Length > 0
        End Get
    End Property

    Private disposedValue As Boolean
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not disposedValue Then
            If disposing Then
                Me.Logger.Close()
                Me.Timer.Stop()
            End If
            Me.disposedValue = True
        End If
    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        Me.Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
End Class
