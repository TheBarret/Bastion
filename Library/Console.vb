Namespace Library
    Public Class Console
        <ScriptFunction("print")>
        Public Shared Sub ConsoleWrite(rt As Runtime, str As Object)
            System.Console.WriteLine(str)
        End Sub
    End Class
End Namespace