Namespace Expressions.Types
    Public NotInheritable Class [Integer]
        Inherits Expression
        Public Property Value As Integer
        Sub New(Value As Integer)
            Me.Value = Value
        End Sub
        Public Overrides Function ToString() As String
            Return String.Format("{0}", Me.Value)
        End Function
    End Class
End Namespace