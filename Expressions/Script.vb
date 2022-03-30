Namespace Expressions
    Public Class Script
        Inherits List(Of Expression)
        Sub New(e As List(Of Expression))
            Me.AddRange(e)
            Me.Name = "Untitled"
            Me.Description = "No description"
        End Sub
        Public Property Name As String
        Public Property Description As String
    End Class
End Namespace