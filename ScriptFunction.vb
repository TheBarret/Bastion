Public Class ScriptFunction
    Inherits Attribute
    Public Property Reference As String
    Sub New(ref As String)
        Me.Reference = ref
    End Sub
End Class
