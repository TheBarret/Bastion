Imports System.Text.RegularExpressions

Namespace Parsers
    Public Class Syntax
        Inherits Dictionary(Of Tokens, String)
        Implements IDisposable
        Sub New()

            Me.Add(Tokens.T_Return, "^\breturn\b")

            Me.Add(Tokens.T_Function, "^\bfunction\b")
            Me.Add(Tokens.T_Hexadecimal, "^(0x)[a-fA-F0-9]{1,8}")
            Me.Add(Tokens.T_AssignAddition, "^(\+\=)")
            Me.Add(Tokens.T_AssignSubtraction, "^(\-\=)")
            Me.Add(Tokens.T_AssignMultiplication, "^(\*\=)")
            Me.Add(Tokens.T_AssignDivision, "^(\/\=)")
            Me.Add(Tokens.T_AssignModulus, "^(\%\=)")
            Me.Add(Tokens.T_Increment, "^(\+\+)")
            Me.Add(Tokens.T_Decrement, "^(\-\-)")
            Me.Add(Tokens.T_Space, "^(\s)")
            Me.Add(Tokens.T_Newline, "^(\r\n)")
            Me.Add(Tokens.T_LineComment, "^(//(.*?)\r?\n)")
            Me.Add(Tokens.T_BlockComment, "^(/\*(.*?)\*/)")
            Me.Add(Tokens.T_EndStatement, "^(;)")
            Me.Add(Tokens.T_EqualOrGreater, "^(>=)")
            Me.Add(Tokens.T_EqualOrLesser, "^(<=)")
            Me.Add(Tokens.T_NotEqual, "^(!=)")
            Me.Add(Tokens.T_Equal, "^(==)")
            Me.Add(Tokens.T_Greater, "^(>)")
            Me.Add(Tokens.T_Lesser, "^(<)")
            Me.Add(Tokens.T_Plus, "^(\+)")
            Me.Add(Tokens.T_Minus, "^(-)")
            Me.Add(Tokens.T_Div, "^(/)")
            Me.Add(Tokens.T_Mult, "^(\*)")
            Me.Add(Tokens.T_Mod, "^(%)")
            Me.Add(Tokens.T_Assign, "^(=)")
            Me.Add(Tokens.T_Colon, "^(\:)")
            Me.Add(Tokens.T_Comma, "^(,)")
            Me.Add(Tokens.T_Negate, "^(\!)")
            Me.Add(Tokens.T_Or, "^(\|)")
            Me.Add(Tokens.T_And, "^(\&)")
            Me.Add(Tokens.T_Xor, "^(\^)")
            Me.Add(Tokens.T_If, "^\bif\b")
            Me.Add(Tokens.T_Else, "^\belse\b")
            Me.Add(Tokens.T_Use, "^\buse\b")
            Me.Add(Tokens.T_For, "^\bfor\b")
            Me.Add(Tokens.T_To, "^\bto\b")
            Me.Add(Tokens.T_Step, "^\bstep\b")
            Me.Add(Tokens.T_Null, "^\bnull\b")
            Me.Add(Tokens.T_BracketOpen, "^(\[)")
            Me.Add(Tokens.T_BracketClose, "^(\])")
            Me.Add(Tokens.T_BraceOpen, "^({)")
            Me.Add(Tokens.T_BraceClose, "^(})")
            Me.Add(Tokens.T_ParenthesisOpen, "^(\()")
            Me.Add(Tokens.T_ParenthesisClose, "^(\))")
            Me.Add(Tokens.T_Bool, "^\btrue\b|^\bfalse\b")
            Me.Add(Tokens.T_String, "^'(.*?)'|^""(.*?)""")
            Me.Add(Tokens.T_Float, "^[-+]?\d*\.\d+([eE][-+]?\d+)?")
            Me.Add(Tokens.T_Integer, "^[-+]?\d*([eE]?\d+)")
            Me.Add(Tokens.T_Identifier, "^(?:[a-z_\$][a-z0-9_]*)")
            Me.Add(Tokens.T_Dot, "^\.")
        End Sub
        Public Function Match(Type As Tokens, value As String) As Match
            Return New Regex(Me(Type), RegexOptions.Singleline Or RegexOptions.IgnoreCase).Match(value)
        End Function
        Private disposedValue As Boolean
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not disposedValue Then
                If disposing Then
                    Me.Clear()
                End If
                disposedValue = True
            End If
        End Sub
        Public Sub Dispose() Implements IDisposable.Dispose
            Me.Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub
    End Class
End Namespace