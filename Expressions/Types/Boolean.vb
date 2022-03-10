Namespace Expressions.Types
    Public NotInheritable Class [Boolean]
        Inherits Expression
        Public Property Value As Boolean
        Sub New(Value As Boolean)
            Me.Value = Value
        End Sub
        Public Overrides Function ToString() As String
            Return String.Format("{0}", Me.Value)
        End Function
    End Class
End Namespace