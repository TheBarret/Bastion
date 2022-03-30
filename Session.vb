Imports Bastion.Expressions
Imports Bastion.Helpers
Imports System.Reflection

Public MustInherit Class Session
    Implements IDisposable
    Public Property Name As String
    Public Property Debug As Boolean
    Public Property Logger As Logger
    Public Property Scope As Scope
    Public Property Script As Script
    Public Property Timers As Dictionary(Of String, Stopwatch)

    Sub New(context As String)
        Me.Name = Uid.Generate(8)
        Me.Debug = True
        Me.Scope = New Scope
        Me.m_input = context
        Me.Timers = New Dictionary(Of String, Stopwatch)
        Me.Logger = New Logger(Me)
    End Sub

    Sub New(context As List(Of Expression))
        Me.Name = Uid.Generate(8)
        Me.Debug = True
        Me.Scope = New Scope
        Me.m_tree = context
        Me.Script = New Script(context)
        Me.Timers = New Dictionary(Of String, Stopwatch)
        Me.Logger = New Logger(Me)
    End Sub

    ''' <summary>
    ''' Logs a message to the logger.
    ''' </summary>
    ''' <param name="message"></param>
    Public Sub Log(message As String)
        If (Me.Debug) Then
            Dim mb As MethodBase = Me.GetStackFrame(2)
            Me.Logger.WriteLine(String.Format("[{0}] [{1}] [{2}.{3}] {4}", Operators.Repeat("*", Me.Scope.Level), Me.Name, mb.DeclaringType.Name, mb.Name, message))
        End If
    End Sub

    ''' <summary>
    ''' Gets the stackframe of defined level
    ''' </summary>
    ''' <param name="level"></param>

    Public Function GetStackFrame(level As Integer) As MethodBase
        Return New StackTrace().GetFrame(level).GetMethod()
    End Function

    ''' <summary>
    ''' Creates and starts a timer.
    ''' </summary>
    ''' <param name="name"></param>
    ''' <param name="startEnabled"></param>

    Public Function CreateTimer(name As String, Optional startEnabled As Boolean = False) As Stopwatch
        If (Not Me.Timers.ContainsKey(name)) Then
            Dim sw As New Stopwatch
            If (startEnabled) Then
                sw.Start()
            End If
            Me.Timers.Add(name, sw)
        End If
        Return Me.Timers(name)
    End Function

    ''' <summary>
    ''' Start and Restarts defined timer.
    ''' </summary>
    ''' <param name="name"></param>
    Public Sub StartTimer(name As String)
        If (Me.Timers.ContainsKey(name)) Then
            Me.Timers(name).Restart()
        Else
            Me.Timers.Add(name, New Stopwatch)
            Me.Timers(name).Start()
        End If
    End Sub

    ''' <summary>
    ''' Gets the defined timer
    ''' </summary>
    ''' <param name="name"></param>

    Public Function GetTimer(name As String) As Stopwatch
        Return Me.Timers(name)
    End Function

    ''' <summary>
    ''' Stops the defined timer
    ''' </summary>
    ''' <param name="name"></param>

    Public Function StopTimer(name As String) As Stopwatch
        If (Me.Timers.ContainsKey(name)) Then
            Me.Timers(name).Stop()
            Return Me.Timers(name)
        End If
        Throw New Exception(String.Format("no such timer defined '{0}'", name))
    End Function

    ''' <summary>
    ''' Destroys the defined timer
    ''' </summary>
    ''' <param name="name"></param>

    Public Function DestroyTimer(name As String) As Stopwatch
        If (Me.Timers.ContainsKey(name)) Then
            Try
                Me.Timers(name).Stop()
                Return Me.Timers(name)
            Finally
                Me.Timers.Remove(name)
            End Try
        End If
        Throw New Exception(String.Format("no such timer defined '{0}'", name))
    End Function

    ''' <summary>
    ''' Destroys all timers
    ''' </summary>
    Public Sub TerminateAllTimers()
        For Each t In Me.Timers
            t.Value.Stop()
        Next
    End Sub

    ''' <summary>
    ''' Gets the context of the session in AST format.
    ''' </summary>

    Private m_tree As List(Of Expression)
    Public ReadOnly Property Tree As List(Of Expression)
        Get
            Return Me.m_tree
        End Get
    End Property

    ''' <summary>
    ''' Returns true if context contains anything
    ''' </summary>

    Public ReadOnly Property HasTree As Boolean
        Get
            Return Me.Tree.Count > 0
        End Get
    End Property

    ''' <summary>
    ''' Gets the context of the session.
    ''' </summary>

    Private m_input As String
    Public ReadOnly Property Input As String
        Get
            Return Me.m_input
        End Get
    End Property

    ''' <summary>
    ''' Returns true if context contains anything
    ''' </summary>

    Public ReadOnly Property HasInput As Boolean
        Get
            Return Me.m_input.Length > 0
        End Get
    End Property

    ''' <summary>
    ''' IDisposable implementation.
    ''' </summary>
    Private disposedValue As Boolean
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not disposedValue Then
            If disposing Then
                Me.Logger.Dispose()
                Me.Scope.Dispose()
                Me.TerminateAllTimers()
                Me.Timers.Clear()
            End If
            Me.disposedValue = True
        End If
    End Sub
    Public Sub Dispose() Implements IDisposable.Dispose
        Me.Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
End Class
