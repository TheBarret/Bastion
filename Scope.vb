Public Class Scope
    Inherits Dictionary(Of String, TValue)
    Private disposedValue As Boolean

    Public Sub SetVariable(Name As String, value As TValue)
        If (Me.ContainsKey(Name.ToLower)) Then
            Me(Name.ToLower) = value
        Else
            Me.Add(Name.ToLower, value)
        End If
    End Sub

    Public Function GetVariable(Name As String) As TValue
        For Each var In Me
            If (var.Key.Equals(Name.ToLower)) Then
                Return var.Value
            End If
        Next
        Return TValue.Null
    End Function

    Public Function IsSet(Name As String) As Boolean
        Return Me.ContainsKey(Name.ToLower)
    End Function

    Public Sub DisposeVariables()
        For Each entry In Me
            entry.Value.Dispose()
        Next
        Me.Clear()
    End Sub
End Class
