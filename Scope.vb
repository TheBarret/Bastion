Public MustInherit Class Scope
    Inherits Dictionary(Of String, TValue)
    Private Property Parent As Session
    Public Property Level As Integer

    Sub New()
        Me.Level = 1
    End Sub

    ''' <summary>
    ''' Hacky way of assigning session owner to the scope, you cant do this in the constructor.
    ''' </summary>
    Public Sub SetScope(parent As Session)
        Me.Parent = parent
    End Sub

    ''' <summary>
    ''' Increment the level of the session.
    ''' </summary>
    Public Sub Enter()
        Me.Level += 1
    End Sub

    ''' <summary>
    ''' Decrement the level of the session.
    ''' </summary>
    Public Sub Leave()
        Me.Level -= 1
    End Sub

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
