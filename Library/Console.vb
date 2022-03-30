Namespace Library
    Public Class Console
        <ScriptFunction("print")>
        Public Shared Sub ConsoleWrite(rt As Runtime, value As Object)
            System.Console.Write(value.ToString)
        End Sub
        <ScriptFunction("printl")>
        Public Shared Sub ConsoleWriteLine(rt As Runtime, value As Object)
            System.Console.WriteLine(value.ToString)
        End Sub
    End Class
End Namespace