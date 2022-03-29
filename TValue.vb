Imports System.IO
Imports Bastion.Expressions.Types

''' <summary>
''' Represents a value that is used by the script engine and can hold any value.
''' </summary>
Public Class TValue
    Public Property Value As Object

    Sub New()
        Me.Value = New Null
    End Sub

    Sub New(Value As Object)
        If (TypeOf Value Is TValue) Then
            Me.Value = CType(Value, TValue).Value
        Else
            Me.Value = If(Value Is Nothing, New Null, Value)
        End If
    End Sub

    ''' <summary>
    ''' Casts the value to the specified type.
    ''' </summary>
    ''' <typeparam name="T"></typeparam>
    ''' <returns></returns>
    Public Function Cast(Of T)() As T
        If (GetType(T) Is Me.Value.GetType) Then
            Return CType(Me.Value, T)
        End If
        Throw New ScriptError(String.Format("Cannot cast value '{0}' to '{1}'", Me.Value.GetType.Name, GetType(T).Name))
    End Function

    ''' <summary>
    ''' Returns the type of the value recognized by the engine.
    ''' </summary>
    ''' <returns></returns>
    Public Function GetObjectType() As Tokens
        If (TypeOf Me.Value Is String) Then
            Return Tokens.T_String
        ElseIf (TypeOf Me.Value Is Char) Then
            Return Tokens.T_String
        ElseIf (TypeOf Me.Value Is Integer) Then
            Return Tokens.T_Integer
        ElseIf (TypeOf Me.Value Is Double) Then
            Return Tokens.T_Float
        ElseIf (TypeOf Me.Value Is Single) Then
            Return Tokens.T_Float
        ElseIf (TypeOf Me.Value Is Decimal) Then
            Return Tokens.T_Float
        ElseIf (TypeOf Me.Value Is Boolean) Then
            Return Tokens.T_Bool
        ElseIf (TypeOf Me.Value Is [Function]) Then
            Return Tokens.T_Function
        ElseIf (TypeOf Me.Value Is [Delegate]) Then
            Return Tokens.T_Delegate
        ElseIf (TypeOf Me.Value Is Stream) Then
            Return Tokens.T_Stream
        ElseIf (TypeOf Me.Value Is Null) Then
            Return Tokens.T_Null
        Else
            Return Tokens.T_Undefined
        End If
    End Function

    ''' <summary>
    ''' Returns true if the value is a string
    ''' </summary>
    ''' <returns></returns>
    Public Function IsString() As Boolean
        Return Me.GetObjectType = Tokens.T_String
    End Function

    ''' <summary>
    ''' Returns true if the value is an boolean
    ''' </summary>
    ''' <returns></returns>
    Public Function IsBoolean() As Boolean
        Return Me.GetObjectType = Tokens.T_Bool
    End Function


    ''' <summary>
    ''' Returns true if the value is an integer
    ''' </summary>
    ''' <returns></returns>
    Public Function IsInteger() As Boolean
        Return Me.GetObjectType = Tokens.T_Integer
    End Function

    ''' <summary>
    ''' Returns true if the value is a float
    ''' </summary>
    ''' <returns></returns>
    Public Function IsFloat() As Boolean
        Return Me.GetObjectType = Tokens.T_Float
    End Function

    ''' <summary>
    ''' Returns true if the value is null
    ''' </summary>
    ''' <returns></returns>
    Public Function IsNull() As Boolean
        Return Me.GetObjectType = Tokens.T_Null
    End Function


    ''' <summary>
    ''' Returns true if the value is a delegate
    ''' </summary>
    ''' <returns></returns>
    Public Function IsDelegate() As Boolean
        Return Me.GetObjectType = Tokens.T_Delegate
    End Function

    ''' <summary>
    ''' Returns true if the value is a script function
    ''' </summary>
    ''' <returns></returns>
    Public Function IsScriptFunction() As Boolean
        Return Me.GetObjectType = Tokens.T_Function
    End Function

    ''' <summary>
    ''' Disposes the value, tries to call the dispose method if the value supports the interface.
    ''' </summary>
    Public Sub Dispose()
        If (GetType(IDisposable).IsAssignableFrom(Me.Value.GetType)) Then
            CType(Me.Value, IDisposable).Dispose()
        End If
    End Sub

    ''' <summary>
    ''' Wraps a object to TValue
    ''' </summary>
    ''' <returns></returns>
    Public Function Wrap() As TValue
        If (TypeOf Me.Value Is TValue) Then
            Return CType(Me.Value, TValue)
        End If
        Return New TValue(Me.Value)
    End Function

    ''' <summary>
    ''' Unwraps a TValue to object
    ''' </summary>
    ''' <returns></returns>
    Public Function Unwrap() As Object
        Dim result As Object = Me.Value
        Do
            If (TypeOf result Is TValue) Then
                result = CType(result, TValue).Value
            End If
        Loop While TypeOf result Is TValue
        Return result
    End Function

    ''' <summary>
    ''' Overrides the ToString() base method
    ''' </summary>
    ''' <returns></returns>
    Public Overrides Function ToString() As String
        Return Me.Value.ToString
    End Function

    ''' <summary>
    ''' Returns an empty (null) TValue
    ''' </summary>
    ''' <returns></returns>
    Public Shared Function Null() As TValue
        Return New TValue
    End Function
End Class
