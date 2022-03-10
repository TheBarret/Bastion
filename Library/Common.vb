Namespace Library
    Public Class Common
        <ScriptFunction("isset")>
        Public Shared Function VariableExists(rt As Runtime, name As String) As Boolean
            Return rt.IsSet(name)
        End Function

        <ScriptFunction("typeof")>
        Public Shared Function [TypeOf](rt As Runtime, value As Object) As String
            Return New TValue(value).GetObjectType.ToString.Substring(2)
        End Function

        <ScriptFunction("tostr")>
        Public Shared Function ConvStr(rt As Runtime, value As Object) As String
            Return value.ToString
        End Function

        <ScriptFunction("tobool")>
        Public Shared Function ConvBool(rt As Runtime, value As Object) As Boolean
            Dim result As Boolean = False
            If (value.ToString = "1") Then
                Return True
            ElseIf (value.ToString = "0") Then
                Return False
            ElseIf (Boolean.TryParse(value.ToString, result)) Then
                Return result
            End If
            rt.Log(String.Format("[error] Unable to convert '{0}' to boolean (default: false)", value))
            Return result
        End Function

        <ScriptFunction("toint")>
        Public Shared Function ConvInt(rt As Runtime, value As Object) As Integer
            Dim result As Integer = Nothing
            If (Integer.TryParse(value.ToString, Environment.Integers, Environment.Culture, result)) Then
                Return result
            End If
            rt.Log(String.Format("[error] Unable to convert '{0}' to integer (default: 0)", value))
            Return 0
        End Function

        <ScriptFunction("tofloat")>
        Public Shared Function ConvFloat(rt As Runtime, value As Object) As Double
            Dim result As Double = Nothing
            If (Double.TryParse(value.ToString, Environment.Floats, Environment.Culture, result)) Then
                Return result
            End If
            rt.Log(String.Format("[error] Unable to convert '{0}' to double (default: 0)", value))
            Return 0
        End Function
    End Class
End Namespace