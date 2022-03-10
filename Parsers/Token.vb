Namespace Parsers
    Public NotInheritable Class Token
        Public Property Line As Integer
        Public Property Index As Integer
        Public Property Length As Integer
        Public Property Value As String
        Public Property Type As Tokens
        Sub New(Type As Tokens)
            Me.Type = Type
            Me.Value = String.Empty
        End Sub
        Sub New(Value As String, Type As Tokens)
            Me.Type = Type
            Me.Value = Value
        End Sub
        Sub New(Value As String, Type As Tokens, Line As Integer, Index As Integer, Length As Integer)
            Me.Line = Line
            Me.Type = Type
            Me.Value = Value
            Me.Index = Index
            Me.Length = Length
        End Sub
        Public Shared Function Create(Type As Tokens) As Token
            Return Token.Create(Type, String.Empty, 0)
        End Function
        Public Shared Function Create(Type As Tokens, value As String) As Token
            Return Token.Create(Type, value, 0)
        End Function
        Public Shared Function Create(Type As Tokens, value As String, Line As Integer) As Token
            Return New Token(value, Type, Line, 0, 0)
        End Function
        Public Shared Function Create(Type As Tokens, value As String, Line As Integer, index As Integer) As Token
            Return New Token(value, Type, Line, index, 0)
        End Function
        Public Shared Function Create(Type As Tokens, value As String, Line As Integer, index As Integer, Len As Integer) As Token
            Return New Token(value, Type, Line, index, Len)
        End Function
        Public Overrides Function ToString() As String
            Return String.Format("{0} [{1},{2}]", Me.Type, Me.Line, Me.Index)
        End Function
    End Class
End Namespace