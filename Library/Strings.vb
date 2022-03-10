Namespace Library
    Public Class Strings
        <ScriptFunction("lcase")>
        Public Shared Function StringToLower(rt As Runtime, str As String) As String
            Return str.ToLower
        End Function
        <ScriptFunction("ucase")>
        Public Shared Function StringToUpper(rt As Runtime, str As String) As String
            Return str.ToUpper
        End Function
        <ScriptFunction("strrev")>
        Public Shared Function StringReversed(rt As Runtime, str As String) As String
            Dim buffer As Char() = str.ToCharArray()
            Array.Reverse(buffer)
            Return New String(buffer)
        End Function
        <ScriptFunction("strlen")>
        Public Shared Function StringLength(rt As Runtime, str As String) As Integer
            Return str.Count
        End Function
        <ScriptFunction("replace")>
        Public Shared Function StringReplace(rt As Runtime, str As String, find As String, replaceWith As String) As String
            Return str.Replace(find, replaceWith)
        End Function
        <ScriptFunction("getstr")>
        Public Shared Function GetStringParts(rt As Runtime, str As String, index As Integer, length As Integer) As String
            Return str.Substring(index, length)
        End Function
        <ScriptFunction("contains")>
        Public Shared Function StringContains(rt As Runtime, str As String, find As String) As Boolean
            Return str.Contains(find)
        End Function
        <ScriptFunction("indexof")>
        Public Shared Function StringIndexOf(rt As Runtime, str As String, find As String) As Integer
            Return str.IndexOf(find)
        End Function
        <ScriptFunction("trim")>
        Public Shared Function TrimString(rt As Runtime, str As String, max As Integer) As String
            Return str.Trim
        End Function
        <ScriptFunction("truncate")>
        Public Shared Function TruncateString(rt As Runtime, str As String, max As Integer) As String
            Return str.Truncate(max)
        End Function
    End Class
End Namespace