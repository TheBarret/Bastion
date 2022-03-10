Namespace Expressions.Types
    Public NotInheritable Class [String]
        Inherits Expression
        Public Property Value As String
        Sub New(Value As String)
            Me.Value = Value
        End Sub
        Sub New(Value As Char)
            Me.Value = Convert.ToString(Value)
        End Sub
        Public Overrides Function ToString() As String
            Return String.Format("{0}", Me.Value)
        End Function
    End Class
End Namespace