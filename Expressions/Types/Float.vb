Namespace Expressions.Types
    Public NotInheritable Class Float
        Inherits Expression
        Public Property Value As Double
        Sub New(Value As Double)
            Me.Value = Value
        End Sub
        Sub New(Value As Single)
            Me.Value = Convert.ToDouble(Value)
        End Sub
        Sub New(Value As Decimal)
            Me.Value = Convert.ToDouble(Value)
        End Sub
        Public Overrides Function ToString() As String
            Return String.Format("{0}", Me.Value)
        End Function
    End Class
End Namespace