Imports Bastion.Expressions
Imports Bastion.Helpers
Imports System.Reflection

Public MustInherit Class Session
    Inherits Scope
    Public Property Name As String
    Public Property Debug As Boolean
    Public Property Logger As Logger
    Public Property Timers As Dictionary(Of String, Stopwatch)

    Sub New(context As String, Optional Openlog As Boolean = True)
        Me.Name = Uid.Generate(8)
        Me.Debug = True
        Me.m_script = context
        Me.Timers = New Dictionary(Of String, Stopwatch)
        If (Openlog) Then Me.Logger = New Logger
    End Sub

    Sub New(ast As List(Of Expression), Optional Openlog As Boolean = True)
        Me.Name = Uid.Generate(8)
        Me.Debug = True
        Me.m_ast = ast
        Me.Timers = New Dictionary(Of String, Stopwatch)
        If (Openlog) Then Me.Logger = New Logger
    End Sub

    ''' <summary>
    ''' Logs a message to the logger.
    ''' </summary>
    ''' <param name="message"></param>
    Public Sub Log(message As String)
        If (Me.Debug) Then
            Dim mb As MethodBase = Me.GetStackFrame(2)
            Me.Logger.WriteLine(String.Format("[{0}] [{1}] [{2}.{3}] {4}", Operators.Repeat("*", Me.Level), Me.Name, mb.DeclaringType.Name, mb.Name, message))
        End If
    End Sub

    ''' <summary>
    ''' Gets the stackframe of defined level
    ''' </summary>
    ''' <param name="level"></param>
    ''' <returns></returns>
    Public Function GetStackFrame(level As Integer) As MethodBase
        Return New StackTrace().GetFrame(level).GetMethod()
    End Function

    ''' <summary>
    ''' Creates and starts a timer.
    ''' </summary>
    ''' <param name="name"></param>
    ''' <param name="startEnabled"></param>
    ''' <returns></returns>
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
    ''' <returns></returns>
    Public Function GetTimer(name As String) As Stopwatch
        Return Me.Timers(name)
    End Function

    ''' <summary>
    ''' Stops the defined timer
    ''' </summary>
    ''' <param name="name"></param>
    ''' <returns></returns>
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
    ''' <returns></returns>
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
    ''' Gets the context of the session in AST format.
    ''' </summary>
    ''' <returns></returns>
    Private m_ast As List(Of Expression)
    Public ReadOnly Property Ast As List(Of Expression)
        Get
            Return Me.m_ast
        End Get
    End Property

    ''' <summary>
    ''' Returns true if context contains anything
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property HasAst As Boolean
        Get
            Return Me.Ast.Count > 0
        End Get
    End Property

    ''' <summary>
    ''' Gets the context of the session.
    ''' </summary>
    ''' <returns></returns>
    Private m_script As String
    Public ReadOnly Property Script As String
        Get
            Return Me.m_script
        End Get
    End Property

    ''' <summary>
    ''' Returns true if context contains anything
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property HasScript As Boolean
        Get
            Return Me.Script.Length > 0
        End Get
    End Property

End Class
