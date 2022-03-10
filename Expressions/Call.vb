Namespace Expressions
    Public NotInheritable Class [Call]
        Inherits Expression
        Public Property Name As Expression
        Public Property Parameters As List(Of Expression)
        Sub New(Name As Expression)
            Me.Name = Name
            Me.Parameters = New List(Of Expression)
        End Sub
        Sub New(Name As Expression, Parameters As List(Of Expression))
            Me.Name = Name
            Me.Parameters = Parameters
        End Sub
        Public Overrides Function ToString() As String
            Return String.Format("{0}({1})", Me.Name, String.Join(",", Me.Parameters.Select(Function(x) x)))
        End Function
    End Class
End Namespace