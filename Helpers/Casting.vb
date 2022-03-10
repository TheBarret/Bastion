Namespace Helpers
    Public Class Casting
        Public Shared Function Add(x As Integer, y As Integer) As Integer
            Return x + y
        End Function
        Public Shared Function Validate(rt As Runtime, e As [Delegate], params As List(Of Object)) As Boolean
            If (e.Method.GetParameters.Count <> params.Count) Then
                rt.Session.Log(String.Format("Parameter count mismatch for '{0}()'", e.Method.Name))
                Return False
            End If
            For i As Integer = 0 To e.Method.GetParameters.Count - 1
                If (e.Method.GetParameters(i).ParameterType = GetType(Object)) Then Continue For
                If (e.Method.GetParameters(i).ParameterType <> params(i).GetType) Then
                    If (Not Casting.TryConvert(rt, params(i), e.Method.GetParameters(i).ParameterType, params(i))) Then
                        rt.Session.Log(String.Format("Parameter type mismatch for '{0}()'", e.Method.Name))
                        Return False
                    End If
                End If
            Next
            Return True
        End Function
        Public Shared Function TryConvert(rt As Runtime, value As Object, targetType As Type, ByRef result As Object) As Boolean
            Try
                If (value.GetType = targetType) Then
                    result = value
                    Return True
                End If
                rt.Session.Log(String.Format("Attempting to change type '{0}' to '{1}'", value.GetType.Name, targetType.Name))
                result = Convert.ChangeType(value, targetType)
                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function
    End Class
End Namespace