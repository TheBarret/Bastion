Imports Bastion.Helpers
Imports Bastion.Parsers
Imports Bastion.Expressions
Imports Bastion.Expressions.Types

Imports System.Reflection

Public Class Runtime
    Inherits Session
    Implements IDisposable

    ''' <summary>
    ''' Constructor of runtime.
    ''' </summary>
    ''' <param name="script"></param>
    Sub New(script As String)
        MyBase.New(script)
        Me.CreateTimer("execution_timer", True)
    End Sub

    ''' <summary>
    ''' Executes the script.            
    ''' </summary>
    ''' <returns></returns>
    Public Function Evaluate() As TValue
        Try
            If (Me.HasContext) Then
                Return Me.Resolve(New Ast(Me).Analyze(New Lexer(Me).Analyze(Me.Context)))
            End If
        Catch ex As Exception
            Me.Log(String.Format("[error] Execution stopped ({0})", ex.Message))
            Throw
        End Try
        Return TValue.Null
    End Function


    ''' <summary>
    ''' Scans type for exposed methods.
    ''' </summary>
    ''' <param name="obj"></param>
    Public Sub Scan(obj As Type)
        Me.Log(String.Format("Scanning {0}...", obj.FullName))
        For Each m As MethodInfo In obj.GetMethods(BindingFlags.Public Or BindingFlags.Static)
            For Each attr As ScriptFunction In m.GetCustomAttributes.OfType(Of ScriptFunction)()
                Me.Log(String.Format("<- {0}() ({1})", attr.Reference, m))
                Me.SetVariable(attr.Reference, New TValue(obj.CreateDelegate(m.Name)))
            Next
        Next
    End Sub

    ''' <summary>
    ''' Resolves an expression to a value.
    ''' </summary>
    ''' <param name="e"></param>
    ''' <returns></returns>
    Private Function Resolve(e As List(Of Expression)) As TValue
        Dim result As TValue = TValue.Null
        For Each expr In e
            result = Me.Resolve(expr)
        Next
        Return result
    End Function


    ''' <summary>
    ''' Resolves an expression to a value.
    ''' </summary>
    ''' <param name="e"></param>
    ''' <returns></returns>
    Private Function Resolve(e As Expression) As TValue
        Try
            Me.Enter()
            If (TypeOf e Is Null) Then
                Return TValue.Null
            ElseIf (TypeOf e Is [String]) Then
                Return New TValue(CType(e, [String]).Value)
            ElseIf (TypeOf e Is [Boolean]) Then
                Return New TValue(CType(e, [Boolean]).Value)
            ElseIf (TypeOf e Is [Integer]) Then
                Return New TValue(CType(e, [Integer]).Value)
            ElseIf (TypeOf e Is Float) Then
                Return New TValue(CType(e, Float).Value)
            ElseIf (TypeOf e Is Identifier) Then
                Return Me.Resolve(CType(e, Identifier))
            ElseIf (TypeOf e Is [Call]) Then
                Return Me.Resolve(CType(e, [Call]))
            ElseIf (TypeOf e Is Binary) Then
                Return Me.Resolve(CType(e, Binary))
            ElseIf (TypeOf e Is Unary) Then
                Return Me.Resolve(CType(e, Unary))
            ElseIf (TypeOf e Is Conditional) Then
                Return Me.Resolve(CType(e, Conditional))
            ElseIf (TypeOf e Is ForLoop) Then
                Return Me.Resolve(CType(e, ForLoop))
            ElseIf (TypeOf e Is Expressions.Library) Then
                Return Me.Resolve(CType(e, Expressions.Library))
            End If
            Throw New ScriptError(String.Format("undefined expression type '{0}'", e.GetType.Name))
        Finally
            Me.Leave()
        End Try
    End Function

    ''' <summary>
    ''' Resolves an expression to a value.
    ''' </summary>
    ''' <param name="e"></param>
    ''' <returns></returns>
    Private Function Resolve(e As ForLoop) As TValue
        Me.Log(String.Format("[Loop] -> {0}", e))
        Dim init As TValue = Me.Resolve(e.Init), condition As TValue
        Do
            condition = Me.Resolve(e.Condition)
            If (condition.IsBoolean) Then
                If (condition.Cast(Of Boolean)()) Then
                    Me.Resolve(e.Body)
                    Me.Resolve(e.Step)
                Else
                    Exit Do
                End If
            Else
                Throw New ScriptError(String.Format("for-loop expects a boolean for condition '{0}'", e))
            End If
        Loop While True
        Return TValue.Null
    End Function

    ''' <summary>
    ''' Resolves an expression to a value.
    ''' </summary>
    ''' <param name="e"></param>
    ''' <param name="domain"></param>
    ''' <returns></returns>
    Private Function Resolve(e As Expressions.Library, Optional domain As String = "Bastion.Library") As TValue
        Me.Log(String.Format("[Library] Looking for {0} ", e))
        Dim ref As String = Parsing.GetValue(e.Name)
        For Each t As Type In Assembly.GetExecutingAssembly.GetTypes
            If (t.IsClass AndAlso t.Namespace = domain AndAlso t.Name.Equals(ref, StringComparison.CurrentCultureIgnoreCase)) Then
                Me.Scan(t)
                Return New TValue(True)
            End If
        Next
        Return New TValue(False)
    End Function

    ''' <summary>
    ''' Resolves an expression to a value.
    ''' </summary>
    ''' <param name="e"></param>
    ''' <returns></returns>
    Private Function Resolve(e As Binary) As TValue
        Me.Log(String.Format("[binary] -> {0} ", e))
        If (e.Left IsNot Nothing AndAlso e.Right IsNot Nothing) Then
            If (e.Op = Tokens.T_Assign) Then
                Dim operand As TValue = Me.Resolve(e.Right)
                Me.SetVariable(Parsing.GetValue(e.Left), operand)
                Return operand
            Else
                Dim result As TValue = TValue.Null
                Dim left As TValue = Me.Resolve(e.Left)
                Dim right As TValue = Me.Resolve(e.Right)

                If (e.Op = Tokens.T_Plus) Then
                    result = Operators.Addition(left, right)
                ElseIf (e.Op = Tokens.T_Minus) Then
                    result = Operators.Subtraction(left, right)
                ElseIf (e.Op = Tokens.T_Mult) Then
                    result = Operators.Multiplication(left, right)
                ElseIf (e.Op = Tokens.T_Div) Then
                    result = Operators.Division(left, right)
                ElseIf (e.Op = Tokens.T_Mod) Then
                    result = Operators.Modulo(left, right)
                ElseIf (e.Op = Tokens.T_And) Then
                    result = Operators.And(left, right)
                ElseIf (e.Op = Tokens.T_Or) Then
                    result = Operators.Or(left, right)
                ElseIf (e.Op = Tokens.T_Xor) Then
                    result = Operators.Xor(left, right)
                ElseIf (e.Op = Tokens.T_Equal) Then
                    result = Operators.IsEqual(left, right)
                ElseIf (e.Op = Tokens.T_NotEqual) Then
                    result = Operators.IsNotEqual(left, right)
                ElseIf (e.Op = Tokens.T_Greater) Then
                    result = Operators.IsGreater(left, right)
                ElseIf (e.Op = Tokens.T_Lesser) Then
                    result = Operators.IsLesser(left, right)
                ElseIf (e.Op = Tokens.T_EqualOrGreater) Then
                    result = Operators.IsEqualOrGreater(left, right)
                ElseIf (e.Op = Tokens.T_EqualOrLesser) Then
                    result = Operators.IsEqualOrLesser(left, right)
                End If
                Me.Log(String.Format("[yield] <- ({0} {1} {2}) = {3}", left, e.Op.Name, right, result))
                Return result
            End If
            Throw New ScriptError(String.Format("undefined expression type '{0}'", e.GetType.Name))
        End If
        Throw New ScriptError("invalid expression")
    End Function

    ''' <summary>
    ''' Resolves an expression to a value.
    ''' </summary>
    ''' <param name="e"></param>
    ''' <returns></returns>
    Private Function Resolve(e As Unary) As TValue
        Me.Log(String.Format("[unary] -> {0}", e))
        If (e.Operand IsNot Nothing) Then
            If (e.Op = Tokens.T_Negate) Then
                Return Operators.Not(Me.Resolve(e.Operand))
            ElseIf (e.Op = Tokens.T_Plus) Then
                Return Operators.Sign(Me.Resolve(e.Operand), e.Op)
            ElseIf (e.Op = Tokens.T_Minus) Then
                Return Operators.Sign(Me.Resolve(e.Operand), e.Op)
            End If
            Throw New ScriptError(String.Format("undefined expression type '{0}'", e.GetType.Name))
        End If
        Throw New ScriptError("invalid expression")
    End Function

    ''' <summary>
    ''' Resolves an expression to a value.
    ''' </summary>
    ''' <param name="e"></param>
    ''' <returns></returns>
    Private Function Resolve(e As Conditional) As TValue
        Me.Log(String.Format("[conditional] -> {0}", e))
        Dim condition As TValue = Me.Resolve(e.Condition)
        If (condition.IsBoolean) Then
            If (condition.Cast(Of Boolean)()) Then
                Me.Resolve(e.True)
            Else
                Me.Resolve(e.False)
            End If
            Return condition
        Else
            Throw New ScriptError(String.Format("expecting boolean from '{0}'", e.ToString))
        End If
    End Function

    ''' <summary>
    ''' Resolves an expression to a value.
    ''' </summary>
    ''' <param name="e"></param>
    ''' <returns></returns>
    Private Function Resolve(e As Identifier) As TValue
        Me.Log(String.Format("[variable] -> {0}", e))
        Dim name As String = e.Value
        If (Me.IsSet(name)) Then Return Me.GetVariable(name)
        Throw New ScriptError(String.Format("undefined variable '{0}'", name))
    End Function

    ''' <summary>
    ''' Resolves an expression to a value.
    ''' </summary>
    ''' <param name="e"></param>
    ''' <returns></returns>
    Private Function Resolve(e As [Call]) As TValue
        Me.Log(String.Format("[call] -> {0}", e))
        If (Me.IsSet(Parsing.GetValue(e.Name))) Then
            Dim func As TValue = Me.Resolve(e.Name)
            If (func.IsDelegate) Then
                Return Me.Resolve(CType(func.Value, [Delegate]), Me.ResolveParameters(e.Parameters))
            End If
        End If
        Throw New ScriptError(String.Format("Undefined function '{0}()'", e.Name))
    End Function

    ''' <summary>
    ''' Resolves an expression to a value.
    ''' </summary>
    ''' <param name="e"></param>
    ''' <param name="params"></param>
    ''' <returns></returns>
    Private Function Resolve(e As [Delegate], params As List(Of TValue)) As TValue
        Me.Log(String.Format("[delegate] -> {0}", e.Method))
        Dim parameters As New List(Of Object) From {Me}
        parameters.AddRange(params.Select(Function(x) x.Unwrap).ToList)
        If (Casting.Validate(Me, e, parameters)) Then
            Dim result As TValue = New TValue(e.Method.Invoke(Nothing, parameters.ToArray))
            If (Not result.IsNull) Then
                Me.Log(String.Format("[yield] <- {0} ({1})", result.Value.ToString.Truncate(25, "..."), result.GetObjectType.Name))
            End If
            Return result
        End If
        Return TValue.Null
    End Function

    ''' <summary>
    ''' Resolves a list of expressions to a list of values.
    ''' </summary>
    ''' <param name="e"></param>
    ''' <returns></returns>
    Private Function ResolveParameters(e As List(Of Expression)) As List(Of TValue)
        Return e.Select(Function(exp) Me.Resolve(exp)).ToList
    End Function

    ''' <summary>
    ''' Creates unique ID for runtime instance
    ''' </summary>
    Private m_reference As String = Guid.NewGuid.ToString
    Public ReadOnly Property Reference As String
        Get
            Return Me.m_reference
        End Get
    End Property

    ''' <summary>
    ''' Disposes runtime instance
    ''' </summary>
    Private disposedValue As Boolean
    Protected Overloads Sub Dispose(disposing As Boolean)
        Me.Log(String.Format("Disposing [execution: {0}]", Me.DestroyTimer("execution_timer").Elapsed.Duration))
        If Not disposedValue Then
            If disposing Then
                For Each entry In Me
                    entry.Value.Dispose()
                Next
                Me.Clear()
                Me.Logger.Close()
            End If
            Me.disposedValue = True
        End If
    End Sub

    Public Overloads Sub Dispose() Implements IDisposable.Dispose
        Me.Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub


End Class
