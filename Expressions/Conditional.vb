Namespace Expressions
    Public NotInheritable Class Conditional
        Inherits Expression
        Public Property Condition As Expression
        Public Property [True] As List(Of Expression)
        Public Property [False] As List(Of Expression)
        Sub New(condition As Expression)
            Me.Condition = condition
            Me.True = New List(Of Expression)
            Me.False = New List(Of Expression)
        End Sub
        Public Overrides Function ToString() As String
            Return String.Format("if ({0})", Me.Condition)
        End Function
    End Class
End Namespace