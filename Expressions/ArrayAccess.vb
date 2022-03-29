
Namespace Expressions
    Public Class ArrayAccess
        Inherits Expression
        Public Property Name As Expression
        Public Property Index As Expression
        Sub New(name As Expression, index As Expression)
            Me.Name = name
            Me.Index = index
        End Sub
        Public Overrides Function ToString() As String
            Return String.Format("{0}[{1}]", Me.Name.ToString, Me.Index.ToString)
        End Function
    End Class
End Namespace