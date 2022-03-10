Public MustInherit Class Scope
    Inherits Dictionary(Of String, TValue)

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

End Class
