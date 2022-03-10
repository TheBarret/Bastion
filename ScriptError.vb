Public Class ScriptError
    Inherits Exception
    Sub New(message As String)
        MyBase.New(message)
    End Sub
End Class
