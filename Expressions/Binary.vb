Namespace Expressions
    Public NotInheritable Class Binary
        Inherits Expression
        Public Property Left As Expression
        Public Property Right As Expression
        Public Property Op As Tokens
        Sub New(Left As Expression, Op As Tokens, Right As Expression)
            Me.Left = Left
            Me.Right = Right
            Me.Op = Op
        End Sub
        Public Overrides Function ToString() As String
            Return String.Format("{0} {1} {2}", Me.Left.ToString, Me.Op.Name, Me.Right.ToString)
        End Function

    End Class
End Namespace