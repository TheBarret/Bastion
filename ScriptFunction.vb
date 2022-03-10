''' <summary>
''' This is a convenience property that provides a reference name for the method that is called
''' </summary>
Public Class ScriptFunction
    Inherits Attribute
    Public Property Reference As String
    Sub New(ref As String)
        Me.Reference = ref
    End Sub
End Class
