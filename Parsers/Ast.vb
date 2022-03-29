Imports Bastion.Expressions
Imports Bastion.Helpers

Namespace Parsers
    Public Class Ast
        Inherits List(Of Expression)
        Implements IDisposable
        Private disposedValue As Boolean
        Public Property Parent As Session
        Public Property Index As Integer
        Public Property Current As Token
        Public Property Stream As List(Of Token)

        Sub New(parent As Session)
            Me.Parent = parent
        End Sub

        Public Function Analyze(stream As List(Of Token)) As List(Of Expression)
            Me.Parent.Log("Building abstract syntax tree ...")
            Me.Parent.CreateTimer("ast_timer", True)
            If (stream.Count > 0) Then
                Me.Index = 0
                Me.Stream = stream
                Me.Current = stream.First
                Me.Parse()
            End If
            Me.Parent.Log(String.Format("Finished in {0}", Me.Parent.DestroyTimer("ast_timer").Elapsed.Duration))
            Return Me
        End Function

        Public Sub Parse()
            Do Until Me.Current.Type = Tokens.T_EndOfFile
                Dim result As Expression = Me.ParseStatement(True)
                If (result IsNot Nothing) Then Me.Add(result)
            Loop
        End Sub

        Public Function ParseStatement(ExpectEnd As Boolean) As Expression
            Dim e As Expression = Me.ParseAssignment
            If (Me.Current.Type = Tokens.T_If) Then
                e = Parsing.ParseCondition(Me)
            ElseIf (Me.Current.Type = Tokens.T_For) Then
                e = Parsing.ParseForLoop(Me)
            ElseIf (Me.Current.Type = Tokens.T_Use) Then
                e = Parsing.GetLibrary(Me)
            Else
                If (ExpectEnd) Then
                    If (Me.Current.Type = Tokens.T_EndStatement) Then
                        Me.Next()
                    ElseIf (Me.Current.Type = Tokens.T_EndOfFile) Then
                        Me.Next()
                    Else
                        Throw New ScriptError(String.Format("expecting end of statement at line {1} ", Me.Current.Type, Me.Current.Line))
                    End If
                ElseIf (e Is Nothing) Then
                    Throw New ScriptError(String.Format("enexpected '{0}' at line {1} ", Me.Current.Type, Me.Current.Line))
                End If
            End If
            Return e
        End Function
        Public Function ParseAssignment() As Expression
            Dim e As Expression = Me.ParseLogicalOr
            While (Me.Current.Type = Tokens.T_Assign)
                Me.Next()
                e = New Binary(e, Tokens.T_Assign, Me.ParseStatement(False))
            End While
            Return e
        End Function

        Public Function ParseLogicalOr() As Expression
            Dim e As Expression = Me.ParseLogicalAnd()
            While (Me.Current.Type = Tokens.T_Or)
                Dim op As Tokens = Me.Current.Type
                Me.Next()
                e = New Binary(e, op, Me.ParseLogicalAnd)
            End While
            Return e
        End Function

        Public Function ParseLogicalAnd() As Expression
            Dim e As Expression = Me.ParseRelational()
            While (Me.Current.Type = Tokens.T_And)
                Dim op As Tokens = Me.Current.Type
                Me.Next()
                e = New Binary(e, op, Me.ParseRelational)
            End While
            Return e
        End Function

        Public Function ParseRelational() As Expression
            Dim e As Expression = Me.ParsePostfixUnary()
            While (Me.Current.Type = Tokens.T_Equal) OrElse
                  (Me.Current.Type = Tokens.T_NotEqual) OrElse
                  (Me.Current.Type = Tokens.T_Greater) OrElse
                  (Me.Current.Type = Tokens.T_Lesser) OrElse
                  (Me.Current.Type = Tokens.T_EqualOrGreater) OrElse
                  (Me.Current.Type = Tokens.T_EqualOrLesser)
                Dim op As Tokens = Me.Current.Type
                Me.Next()
                e = New Binary(e, op, Me.ParsePostfixUnary)
            End While
            Return e
        End Function

        Private Function ParsePostfixUnary() As Expression
            Dim e As Expression = Me.ParseLogicalXor()
            While (Me.Current.Type = Tokens.T_Increment) OrElse
                  (Me.Current.Type = Tokens.T_Decrement)
                Dim op As Tokens = Me.Current.Type
                Me.Next()
                e = New Unary(e, op, False)
            End While
            Return e
        End Function

        Private Function ParseLogicalXor() As Expression
            Dim e As Expression = Me.ParseAdditionOrSubtraction()
            While (Me.Current.Type = Tokens.T_Xor)
                Me.Next()
                e = New Binary(e, Tokens.T_Xor, Me.ParseAdditionOrSubtraction())
            End While
            Return e
        End Function

        Private Function ParseAdditionOrSubtraction() As Expression
            Dim e As Expression = Me.ParseMultiplicationOrDivision()
            While (Me.Current.Type = Tokens.T_Plus) OrElse
                  (Me.Current.Type = Tokens.T_Minus)
                Dim Op As Tokens = Me.Current.Type
                Me.Next()
                e = New Binary(e, Op, Me.ParseMultiplicationOrDivision)
            End While
            Return e
        End Function

        Private Function ParseMultiplicationOrDivision() As Expression
            Dim e As Expression = Me.ParsePrefixUnary()
            While (Me.Current.Type = Tokens.T_Mult) OrElse
                  (Me.Current.Type = Tokens.T_Div) OrElse
                  (Me.Current.Type = Tokens.T_Mod)
                Dim Op As Tokens = Me.Current.Type
                Me.Next()
                e = New Binary(e, Op, Me.ParsePrefixUnary)
            End While
            Return e
        End Function

        Private Function ParsePrefixUnary() As Expression
            Dim e As Expression = Me.ParseCall()
            While (Me.Current.Type = Tokens.T_Negate) OrElse
                  (Me.Current.Type = Tokens.T_Return)
                Dim Op As Tokens = Me.Current.Type
                Me.Next()
                e = New Unary(Me.ParseStatement(False), Op, True)
            End While
            Return e
        End Function

        Public Function ParseCall() As Expression
            Dim e As Expression = Me.ParseValue
            While (Me.Current.Type = Tokens.T_ParenthesisOpen)
                e = New [Call](e, Parsing.GetTuples(Me, Tokens.T_ParenthesisOpen, Tokens.T_ParenthesisClose))
            End While
            Return e
        End Function

        Public Function ParseValue() As Expression
            Dim e As Expression = Nothing
            If (Me.Current.Type = Tokens.T_String) Then
                e = Parsing.GetString(Me)
            ElseIf (Me.Current.Type = Tokens.T_Identifier) Then
                e = Parsing.GetIdentifier(Me)
            ElseIf (Me.Current.Type = Tokens.T_Bool) Then
                e = Parsing.GetBool(Me)
            ElseIf (Me.Current.Type = Tokens.T_Integer) Then
                e = Parsing.GetInteger(Me, False)
            ElseIf (Me.Current.Type = Tokens.T_Float) Then
                e = Parsing.GetFloat(Me, False)
            ElseIf (Me.Current.Type = Tokens.T_Hexadecimal) Then
                e = Parsing.GetHexadecimal(Me, False)
            ElseIf (Me.Current.Type = Tokens.T_Function) Then
                e = Parsing.GetFunction(Me)
            ElseIf (Me.Current.Type = Tokens.T_Null) Then
                e = Parsing.GetNull(Me)
            ElseIf (Me.Current.Type = Tokens.T_ParenthesisOpen) Then
                e = Parsing.GetParenthesis(Me)
            ElseIf (Me.Current.Type = Tokens.T_Minus) Then
                Me.Next()
                If (Me.Current.Type = Tokens.T_Integer) Then
                    e = Parsing.GetInteger(Me, True, Tokens.T_Minus)
                ElseIf (Me.Current.Type = Tokens.T_Float) Then
                    e = Parsing.GetFloat(Me, True, Tokens.T_Minus)
                ElseIf (Me.Current.Type = Tokens.T_Hexadecimal) Then
                    e = Parsing.GetHexadecimal(Me, True, Tokens.T_Minus)
                ElseIf (Me.Current.Type = Tokens.T_Identifier) Then
                    e = Parsing.GetSignedIdentifier(Me, Tokens.T_Minus)
                ElseIf (Me.Current.Type = Tokens.T_ParenthesisOpen) Then
                    e = Parsing.GetSignedParentheses(Me, Tokens.T_Minus)
                End If
            ElseIf (Me.Current.Type = Tokens.T_Plus) Then
                Me.Next()
                If (Me.Current.Type = Tokens.T_Integer) Then
                    e = Parsing.GetInteger(Me, True, Tokens.T_Plus)
                ElseIf (Me.Current.Type = Tokens.T_Float) Then
                    e = Parsing.GetFloat(Me, True, Tokens.T_Plus)
                ElseIf (Me.Current.Type = Tokens.T_Hexadecimal) Then
                    e = Parsing.GetHexadecimal(Me, True, Tokens.T_Plus)
                ElseIf (Me.Current.Type = Tokens.T_Identifier) Then
                    e = Parsing.GetSignedIdentifier(Me, Tokens.T_Plus)
                ElseIf (Me.Current.Type = Tokens.T_ParenthesisOpen) Then
                    e = Parsing.GetSignedParentheses(Me, Tokens.T_Plus)
                End If
            End If
            Return e
        End Function

        Public Function [Next]() As Token
            If (Me.Index >= Me.Stream.Count - 1) Then
                Me.Current = Token.Create(Tokens.T_EndOfFile)
            Else
                Me.Index += 1
                Me.Current = Me.Stream(Me.Index)
            End If
            Return Me.Current
        End Function

        Public Function Peek() As Token
            If (Me.Index + 1 >= Me.Stream.Count - 1) Then
                Return Token.Create(Tokens.T_EndOfFile)
            Else
                Return Me.Stream(Me.Index + 1)
            End If
        End Function

        Public Function Match(Type As Tokens) As Token
            If (Not Me.Current.Type = Type) Then
                Throw New ScriptError(String.Format("Unexpected '{0}' at line {1}", Me.Current.Type, Me.Current.Line))
            End If
            If (Me.Index >= Me.Stream.Count - 1) Then
                Me.Current = Token.Create(Tokens.T_EndOfFile)
            Else
                Me.Index += 1
                Me.Current = Me.Stream(Me.Index)
            End If
            Return Me.Current
        End Function

        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not disposedValue Then
                If disposing Then
                    Me.Clear()
                End If
                disposedValue = True
            End If
        End Sub

        Public Sub Dispose() Implements IDisposable.Dispose
            Me.Dispose(disposing:=True)
            GC.SuppressFinalize(Me)
        End Sub
    End Class
End Namespace