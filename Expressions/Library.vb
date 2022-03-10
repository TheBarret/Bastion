Namespace Expressions
    Public NotInheritable Class Library
        Inherits Expression
        Public Property Name As Expression
        Sub New(Name As Expression)
            Me.Name = Name
        End Sub
        Public Overrides Function ToString() As String
            Return Me.Name.ToString
        End Function
    End Class
End Namespace