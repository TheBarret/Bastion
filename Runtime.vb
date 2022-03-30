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
    Sub New(script As String)
        MyBase.New(script)
        Me.CreateTimer("execution_timer", True)
    End Sub

    ''' <summary>
    ''' Constructor of runtime.
    ''' </summary>
    Sub New(e As List(Of Expression))
        MyBase.New(e)
        Me.CreateTimer("execution_timer", True)
    End Sub

    ''' <summary>
    ''' Executes the script.            
    ''' </summary>
    Public Function Evaluate() As TValue
        'Try
        If (Me.HasInput) Then
                Me.Script = New Ast(Me).Analyze(New Lexer(Me).Analyze(Me.Input))
                Return Me.Resolve(Me.Script)
            ElseIf (Me.HasTree) Then
                Return Me.Resolve(Me.Tree)
            End If
        'Catch ex As Exception
        'Me.Log(String.Format("[error] Execution stopped ({0})", ex.Message))
        'Throw
        'End Try
        Return TValue.Null
    End Function

    ''' <summary>
    ''' Scans type for exposed methods.
    ''' </summary>
    Public Sub Scan(obj As Type)
        Me.Log(String.Format("Scanning {0}...", obj.FullName))
        For Each m As MethodInfo In obj.GetMethods(BindingFlags.Public Or BindingFlags.Static)
            For Each attr As ScriptFunction In m.GetCustomAttributes.OfType(Of ScriptFunction)()
                Me.Log(String.Format("<- {0}() ({1})", attr.Reference, m))
                Me.Scope.Set(attr.Reference, New TValue(obj.CreateDelegate(m.Name)))
            Next
        Next
    End Sub

    ''' <summary>
    ''' Resolves an expression to a value.
    ''' </summary>
    Private Function Resolve(e As List(Of Expression)) As TValue
        Try
            Dim result As TValue = TValue.Null
            For Each expr In e
                result = Me.Resolve(expr)
            Next
            Return result
        Finally
            Serialization.Pack(".\script.bas", Me.Script)
        End Try
    End Function

    ''' <summary>
    ''' Resolves an expression to a value.
    ''' </summary>
    Private Function Resolve(e As Expression) As TValue
        Try
            Me.Scope.Enter()
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
            ElseIf (TypeOf e Is [Function]) Then
                Return Me.Resolve(CType(e, [Function]))
            ElseIf (TypeOf e Is Expressions.Library) Then
                Return Me.Resolve(CType(e, Expressions.Library))
            End If
            Throw New ScriptError(String.Format("undefined expression type '{0}'", e.GetType.Name))
        Finally
            Me.Scope.Leave()
        End Try
    End Function

    ''' <summary>
    ''' Resolves for-loop
    ''' </summary>
    Private Function Resolve(e As ForLoop) As TValue
        Me.Log(String.Format("[Loop] -> {0}", e))
        Dim ref As String, init, limit, offset As TValue
        init = Me.Resolve(e.Init)
        limit = Me.Resolve(e.Limit)
        offset = Me.Resolve(e.Step)
        If (ForLoop.Valid(e)) Then
            ref = Parsing.GetValue(CType(e.Init, Binary).Left)
            If (init.IsNumber And limit.IsNumber And offset.IsNumber) Then
                For i As Integer = init.ToDouble To limit.ToDouble Step offset.ToDouble
                    Me.Scope.Set(ref, New TValue(i))
                    Me.Resolve(e.Body)
                Next
                Return TValue.Null
            End If
        End If
        Throw New ScriptError(String.Format("invalid for-loop '{0}'", e))
    End Function

    ''' <summary>
    ''' Resolves an expression to a value.
    ''' </summary>
    Private Function Resolve(e As Expressions.Library, Optional domain As String = "Bastion.Library") As TValue
        Me.Log(String.Format("[Import] <- {0} ", e))
        Dim ref As String = Parsing.GetValue(e.Name)
        For Each t As Type In Assembly.GetExecutingAssembly.GetTypes
            If (t.IsValid(ref, domain)) Then
                Me.Scan(t)
                Return New TValue(True)
            End If
        Next
        Return New TValue(False)
    End Function

    ''' <summary>
    ''' Resolves an expression to a value.
    ''' </summary>
    Private Function Resolve(e As Binary) As TValue
        Me.Log(String.Format("[binary] -> {0} ", e))
        If (e.Left IsNot Nothing AndAlso e.Right IsNot Nothing) Then
            If (e.Op = Tokens.T_Assign) Then
                Dim operand As TValue = Me.Resolve(e.Right)
                Me.Scope.Set(Parsing.GetValue(e.Left), operand)
                Return operand
            ElseIf (e.Op = Tokens.T_AssignAddition) Then
                Dim operand As TValue = Me.Resolve(e.Right)
                Dim value As TValue = Me.Scope.Get(Parsing.GetValue(e.Left))
                If (value.IsNumber And operand.IsNumber) Then
                    Dim result As TValue = Operators.Addition(value, operand)
                    Me.Scope.Set(Parsing.GetValue(e.Left), result)
                    Return result
                End If
            ElseIf (e.Op = Tokens.T_AssignSubtraction) Then
                Dim operand As TValue = Me.Resolve(e.Right)
                Dim value As TValue = Me.Scope.Get(Parsing.GetValue(e.Left))
                If (value.IsNumber And operand.IsNumber) Then
                    Dim result As TValue = Operators.Subtraction(value, operand)
                    Me.Scope.Set(Parsing.GetValue(e.Left), result)
                    Return result
                End If
            ElseIf (e.Op = Tokens.T_AssignMultiplication) Then
                Dim operand As TValue = Me.Resolve(e.Right)
                Dim value As TValue = Me.Scope.Get(Parsing.GetValue(e.Left))
                If (value.IsNumber And operand.IsNumber) Then
                    Dim result As TValue = Operators.Multiplication(value, operand)
                    Me.Scope.Set(Parsing.GetValue(e.Left), result)
                    Return result
                End If
            ElseIf (e.Op = Tokens.T_AssignDivision) Then
                Dim operand As TValue = Me.Resolve(e.Right)
                Dim value As TValue = Me.Scope.Get(Parsing.GetValue(e.Left))
                If (value.IsNumber And operand.IsNumber) Then
                    Dim result As TValue = Operators.Division(value, operand)
                    Me.Scope.Set(Parsing.GetValue(e.Left), result)
                    Return result
                End If
            ElseIf (e.Op = Tokens.T_AssignModulus) Then
                Dim operand As TValue = Me.Resolve(e.Right)
                Dim value As TValue = Me.Scope.Get(Parsing.GetValue(e.Left))
                If (value.IsNumber And operand.IsNumber) Then
                    Dim result As TValue = Operators.Modulo(value, operand)
                    Me.Scope.Set(Parsing.GetValue(e.Left), result)
                    Return result
                End If
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
    Private Function Resolve(e As Unary) As TValue
        Me.Log(String.Format("[unary] -> {0}", e))
        If (e.Operand IsNot Nothing) Then
            If (e.Op = Tokens.T_Return) Then
                Return New TValue(Me.Resolve(e.Operand))
            ElseIf (e.Op = Tokens.T_Negate) Then
                Return Operators.Not(Me.Resolve(e.Operand))
            ElseIf (e.Op = Tokens.T_Plus) Then
                Return Operators.Sign(Me.Resolve(e.Operand), e.Op)
            ElseIf (e.Op = Tokens.T_Minus) Then
                Return Operators.Sign(Me.Resolve(e.Operand), e.Op)
            ElseIf (e.Op = Tokens.T_Increment) Then
                Dim result As TValue = TValue.Null
                result = Operators.Addition(Me.Resolve(e.Operand), New TValue(1))
                Me.Scope.Set(Parsing.GetValue(e.Operand), result)
                Return result
            ElseIf (e.Op = Tokens.T_Decrement) Then
                Dim result As TValue = TValue.Null
                result = Operators.Subtraction(Me.Resolve(e.Operand), New TValue(1))
                Me.Scope.Set(Parsing.GetValue(e.Operand), result)
                Return result
            End If
        End If
        Throw New ScriptError(String.Format("invalid expression '{0}'", e.GetType.Name))
    End Function

    ''' <summary>
    ''' Resolves an expression to a value.
    ''' </summary>
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
            Throw New ScriptError(String.Format("expecting boolean '{0}'", e.ToString))
        End If
    End Function

    ''' <summary>
    ''' Resolves an expression to a value.
    ''' </summary>
    Private Function Resolve(e As Identifier) As TValue
        Me.Log(String.Format("[variable] -> {0}", e))
        Dim name As String = e.Value
        If (Me.Scope.Exists(name)) Then Return Me.Scope.Get(name)
        Throw New ScriptError(String.Format("undefined variable '{0}'", name))
    End Function

    ''' <summary>
    ''' Resolves an expression to a value.
    ''' </summary>
    Private Function Resolve(e As [Call]) As TValue
        Me.Log(String.Format("[call] -> {0}", e))
        If (Me.Scope.Exists(Parsing.GetValue(e.Name))) Then
            Dim func As TValue = Me.Resolve(e.Name)
            If (func.IsDelegate) Then
                Return Me.Resolve(CType(func.Value, [Delegate]), Me.ResolveAll(e.Parameters))
            ElseIf (func.IsFunction) Then
                Return Me.Resolve(CType(func.Value, [Function]), Me.ResolveAll(e.Parameters))
            End If
        End If
        Throw New ScriptError(String.Format("undefined function '{0}()'", e.Name))
    End Function

    ''' <summary>
    ''' Resolves an expression to a value.
    ''' </summary>
    Private Function Resolve(e As [Function], params As List(Of TValue)) As TValue
        Me.Log(String.Format("[function] -> {0}", e))
        Using rt As New Runtime(e.Body) With {.Logger = Me.Logger}
            If (e.Parameters.Count = params.Count) Then
                For i As Integer = 0 To e.Parameters.Count - 1
                    rt.Scope.Set(Parsing.GetValue(e.Parameters(i)), params(i))
                Next
                Return rt.Resolve(rt.Tree)
            End If
        End Using
        Throw New ScriptError(String.Format("parameter count mismatch '{0}'", e.ToString))
    End Function

    ''' <summary>
    ''' Assigns function into scope.
    ''' </summary>
    Private Function Resolve(e As [Function]) As TValue
        Me.Scope.Set(Parsing.GetValue(e.Name), New TValue(e))
        Return New TValue(e)
    End Function

    ''' <summary>
    ''' Resolves an expression to a value.
    ''' </summary>
    Private Function Resolve(e As [Delegate], params As List(Of TValue)) As TValue
        Me.Log(String.Format("[delegate] -> {0}", e.Method))
        Dim parameters As New List(Of Object) From {Me}
        parameters.AddRange(params.Select(Function(x) x.UnWrap).ToList)
        If (Environment.Validate(Me, e, parameters)) Then
            Dim result As Object = e.Method.Invoke(Me, parameters.ToArray)
            If (result IsNot Nothing) Then
                If (result.GetType.IsArray) Then
                    result = New TValue(CType(result, Object()).Select(Function(x) New TValue(x)).ToArray)
                Else
                    result = New TValue(result)
                End If
                If (Not result.IsNull) Then
                    Me.Log(String.Format("[yield] <- {0}", CType(result, TValue).GetObjectType.Name))
                End If
                Return result
            End If
        End If
        Return TValue.Null
    End Function

    ''' <summary>
    ''' Resolves a list of expressions.
    ''' </summary>
    Private Function ResolveAll(e As List(Of Expression)) As List(Of TValue)
        Return e.Select(Function(exp) Me.Resolve(exp)).ToList
    End Function

    ''' <summary>
    ''' Disposes runtime instance
    ''' </summary>
    Private disposedValue As Boolean
    Protected Overloads Sub Dispose(disposing As Boolean)
        Me.Log(String.Format("Disposing runtime [{0}]", Me.DestroyTimer("execution_timer").Elapsed.Duration))
        If Not disposedValue Then
            If disposing Then
                MyBase.Dispose()
            End If
            Me.disposedValue = True
        End If
    End Sub
    Public Overloads Sub Dispose() Implements IDisposable.Dispose
        Me.Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
End Class
