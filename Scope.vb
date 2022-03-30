Public Class Scope
    Inherits Dictionary(Of String, TValue)
    Implements IDisposable

    Public Property Level As Integer

    Sub New()
        Me.Level = 1
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

    ''' <summary>
    ''' Sets the value of the variable in the current scope.
    ''' </summary>
    Public Sub [Set](Name As String, value As TValue)
        If (Me.ContainsKey(Name.ToLower)) Then
            Me(Name.ToLower) = value
        Else
            Me.Add(Name.ToLower, value)
        End If
    End Sub

    ''' <summary>
    ''' Gets the value of the variable in the current scope.
    ''' </summary>
    ''' <param name="Name"></param>

    Public Function [Get](Name As String) As TValue
        For Each var In Me
            If (var.Key.Equals(Name.ToLower)) Then
                Return var.Value
            End If
        Next
        Return TValue.Null
    End Function

    ''' <summary>
    ''' Returns true if the variable exists in the current scope.
    ''' </summary>
    ''' <param name="Name"></param>

    Public Function Exists(Name As String) As Boolean
        Return Me.ContainsKey(Name.ToLower)
    End Function

    ''' <summary>
    ''' IDisposable implementation.
    ''' </summary>
    Private disposedValue As Boolean
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                For Each entry In Me
                    entry.Value.Dispose()
                Next
                Me.Clear()
            End If
            Me.disposedValue = True
        End If
    End Sub
    Public Sub Dispose() Implements IDisposable.Dispose
        Me.Dispose(disposing:=True)
        GC.SuppressFinalize(Me)
    End Sub
End Class
