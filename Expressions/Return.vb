Namespace Expressions
    Public NotInheritable Class [Return]
        Inherits Expression
        Public Property Operand As Expression
        Sub New(operand As Expression)
            Me.Operand = operand
        End Sub
        Public Overrides Function ToString() As String
            Return Me.Operand.ToString
        End Function
    End Class
End Namespace