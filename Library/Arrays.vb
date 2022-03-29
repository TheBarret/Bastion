
Namespace Library
    Public Class Arrays

        <ScriptFunction("str2array")>
        Public Shared Function StringToCharArray(rt As Runtime, value As String) As Object()
            Return value.ToCharArray.Select(Of Object)(Function(c) c).ToArray
        End Function

        <ScriptFunction("arev")>
        Public Shared Function ReverseArray(rt As Runtime, value As Object()) As Object()
            Array.Reverse(value)
            Return value
        End Function

        <ScriptFunction("aclr")>
        Public Shared Function ClearArray(rt As Runtime, value As Object(), index As Integer, length As Integer) As Object()
            If (index > 0 AndAlso index + length < value.Length) Then
                Array.Clear(value, index, length)
            End If
            Return value
        End Function

        <ScriptFunction("acpy")>
        Public Shared Function CopyArray(rt As Runtime, value As Object()) As Object()
            Dim result() As Object = New Object(value.Length - 1) {}
            Array.Copy(value, result, value.Length)
            Return result
        End Function

        <ScriptFunction("asort")>
        Public Shared Function SortArray(rt As Runtime, value As Object()) As Object()
            Array.Sort(value)
            Return value
        End Function

        <ScriptFunction("concat")>
        Public Shared Function ConcatenateArray(rt As Runtime, value As Object()) As String
            Return String.Concat(value)
        End Function


    End Class
End Namespace
