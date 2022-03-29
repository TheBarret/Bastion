Namespace Expressions.Types
    Public NotInheritable Class [Function]
        Inherits Expression
        Public Property Name As Identifier
        Public Property Body As List(Of Expression)
        Public Property Parameters As List(Of Expression)
        Sub New(Name As Identifier)
            Me.Name = Name
            Me.Parameters = New List(Of Expression)
            Me.Body = New List(Of Expression)
        End Sub
        Sub New(Name As Identifier, Parameters As List(Of Expression))
            Me.Name = Name
            Me.Parameters = Parameters
            Me.Body = New List(Of Expression)
        End Sub
        Sub New(Name As Identifier, Parameters As List(Of Expression), body As List(Of Expression))
            Me.Name = Name
            Me.Parameters = Parameters
            Me.Body = body
        End Sub
        Public Overrides Function ToString() As String
            Return String.Format("{0}({1})", Me.Name.ToString, String.Join(",", Me.Parameters.Select(Function(x) x)))
        End Function
    End Class
End Namespace