Imports Bastion.Parsers
Imports Bastion.Expressions
Imports Bastion.Expressions.Types
Imports Bastion.Library
Imports Newtonsoft.Json.Linq

Namespace Helpers
    Public Class Parsing

        Public Shared Function GetTuples(parent As Ast, Open As Tokens, Close As Tokens) As List(Of Expression)
            Dim params As New List(Of Expression)
            parent.Match(Open)
            While parent.Current.Type <> Close
                params.Add(parent.ParseStatement(False))
                If (parent.Current.Type <> Tokens.T_Comma) Then
                    Exit While
                End If
                parent.Next()
            End While
            parent.Match(Close)
            Return params
        End Function

        Public Shared Function GetBraceBlock(parent As Ast, Optional ExpectEnd As Boolean = True) As List(Of Expression)
            Dim body As New List(Of Expression)
            parent.Match(Tokens.T_BraceOpen)
            Do While True
                If (parent.Current.Type = Tokens.T_BraceClose) Then
                    parent.Next()
                    Exit Do
                ElseIf (parent.Current.Type = Tokens.T_EndOfFile) Then
                    Throw New ScriptError(String.Format("unexpected EOF at line {0}", parent.Current.Line))
                Else
                    body.Add(parent.ParseStatement(ExpectEnd))
                End If
            Loop
            Return body
        End Function

        Public Shared Function ParseCondition(parent As Ast) As Expression
            parent.Next()
            Dim e As New Conditional(parent.ParseStatement(False)) With {.True = Parsing.GetBraceBlock(parent)}
            If (parent.Current.Type = Tokens.T_Else) Then
                parent.Next()
                e.False.AddRange(Parsing.GetBraceBlock(parent))
            End If
            Return e
        End Function

        Public Shared Function ParseForLoop(parent As Ast) As Expression
            Dim params, body As New List(Of Expression)
            parent.Next()
            parent.Match(Tokens.T_ParenthesisOpen)
            params.Add(parent.ParseStatement(True))
            params.Add(parent.ParseStatement(True))
            params.Add(parent.ParseStatement(True))
            parent.Match(Tokens.T_ParenthesisClose)
            If (parent.Current.Type = Tokens.T_BraceOpen) Then
                body.AddRange(Parsing.GetBraceBlock(parent, True))
            End If
            Return New ForLoop(params) With {.Body = body}
        End Function

        Public Shared Function GetLibrary(parent As Ast) As Expression
            parent.Next()
            Return New Expressions.Library(parent.ParseStatement(True))
        End Function

        Public Shared Function GetSignedParentheses(parent As Ast, op As Tokens) As Expression
            Return New Unary(parent.ParseStatement(False), op, True)
        End Function

        Public Shared Function GetSignedIdentifier(parent As Ast, op As Tokens) As Expression
            Try
                Return New Unary(New Identifier(parent.Current.Value), op, True)
            Finally
                parent.Next()
            End Try
        End Function

        Public Shared Function GetNull(parent As Ast) As Expression
            Try
                Return New Null
            Finally
                parent.Next()
            End Try
        End Function

        Public Shared Function GetBool(parent As Ast) As Expression
            Try
                Return New [Boolean](Boolean.Parse(parent.Current.Value))
            Finally
                parent.Next()
            End Try
        End Function

        Public Shared Function GetString(parent As Ast) As Expression
            Try
                Return New [String](parent.Current.Value.Substring(1, parent.Current.Value.Length - 2))
            Finally
                parent.Next()
            End Try
        End Function

        Public Shared Function GetHexadecimal(parent As Ast, signed As Boolean, Optional op As Tokens = Tokens.T_Null) As Expression
            Try
                Dim value As Integer = Convert.ToInt32(parent.Current.Value, 16)
                If (signed And op = Tokens.T_Minus) Then
                    Return New [Integer](value * -1)
                End If
                Return New [Integer](value * 1)
            Finally
                parent.Next()
            End Try
        End Function

        Public Shared Function GetIdentifier(parent As Ast) As Expression
            Try
                Return New Identifier(parent.Current.Value)
            Finally
                parent.Next()
            End Try
        End Function

        Public Shared Function GetFunction(parent As Ast) As Expression
            parent.Next()
            Return New [Function](New Identifier(String.Format("func_{0}", Uid.Generate(8))), Parsing.GetTuples(parent, Tokens.T_ParenthesisOpen, Tokens.T_ParenthesisClose), Parsing.GetBraceBlock(parent))
        End Function

        Public Shared Function GetReturn(parent As Ast) As Expression
            parent.Next()
            Return New [Return](parent.ParseStatement(True))
        End Function

        Public Shared Function GetArray(parent As Ast) As Expression
            Return New [Array](Parsing.GetTuples(parent, Tokens.T_BracketOpen, Tokens.T_BracketClose))
        End Function

        Public Shared Function GetParenthesis(parent As Ast) As Expression
            Try
                parent.Next()
                Return parent.ParseStatement(False)
            Finally
                parent.Match(Tokens.T_ParenthesisClose)
            End Try
        End Function

        Public Shared Function GetConditional(parent As Ast) As Expression
            parent.Next()
            Dim e As New Conditional(parent.ParseStatement(False)) With {.True = Parsing.GetBraceBlock(parent)}
            If (parent.Current.Type = Tokens.T_Else) Then
                parent.Next()
                e.False.AddRange(Parsing.GetBraceBlock(parent))
            End If
            Return e
        End Function

        Public Shared Function GetInteger(parent As Ast, signed As Boolean, Optional op As Tokens = Tokens.T_Null) As Expression
            Try
                If (signed And op = Tokens.T_Minus) Then
                    Return New [Integer](Integer.Parse(parent.Current.Value, Environment.Integers, Environment.Culture) * -1)
                End If
                Return New [Integer](Integer.Parse(parent.Current.Value, Environment.Integers, Environment.Culture) * 1)
            Finally
                parent.Next()
            End Try
        End Function

        Public Shared Function GetFloat(parent As Ast, signed As Boolean, Optional op As Tokens = Tokens.T_Null) As Expression
            Try
                If (signed And op = Tokens.T_Minus) Then
                    Return New Float(Double.Parse(parent.Current.Value, Environment.Floats, Environment.Culture) * -1.0F)
                End If
                Return New Float(Double.Parse(parent.Current.Value, Environment.Floats, Environment.Culture) * 1.0F)
            Finally
                parent.Next()
            End Try
        End Function

        Public Shared Function GetValue(e As Expression) As String
            Select Case e.GetType
                Case GetType([String])
                    Return CType(e, [String]).Value
                Case GetType(Identifier)
                    Return CType(e, Identifier).Value
                Case GetType([Integer])
                    Return CType(e, [Integer]).Value.ToString
                Case GetType(Float)
                    Return CType(e, Float).Value.ToString
                Case GetType([Boolean])
                    Return CType(e, [Boolean]).Value.ToString
                Case Else
                    Return e.ToString
            End Select
        End Function
    End Class
End Namespace