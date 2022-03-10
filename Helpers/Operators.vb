Imports System.Text

Public NotInheritable Class Operators
    Public Shared Function Addition(a As TValue, b As TValue) As TValue
        If (a.GetObjectType = Tokens.T_Integer And b.GetObjectType = Tokens.T_Integer) Then
            Return New TValue(a.Cast(Of Integer)() + b.Cast(Of Integer)())
        ElseIf (a.GetObjectType = Tokens.T_Float And b.GetObjectType = Tokens.T_Integer) Then
            Return New TValue(a.Cast(Of Double)() + b.Cast(Of Integer)())
        ElseIf (a.GetObjectType = Tokens.T_Integer And b.GetObjectType = Tokens.T_Float) Then
            Return New TValue(a.Cast(Of Integer)() + b.Cast(Of Double)())
        ElseIf (a.GetObjectType = Tokens.T_Float And b.GetObjectType = Tokens.T_Float) Then
            Return New TValue(a.Cast(Of Double)() + b.Cast(Of Double)())
        Else
            Return New TValue(a.Value.ToString & b.Value.ToString)
        End If
    End Function

    Public Shared Function Subtraction(a As TValue, b As TValue) As TValue
        If (a.GetObjectType = Tokens.T_Integer And b.GetObjectType = Tokens.T_Integer) Then
            Return New TValue(a.Cast(Of Integer)() - b.Cast(Of Integer)())
        ElseIf (a.GetObjectType = Tokens.T_Float And b.GetObjectType = Tokens.T_Integer) Then
            Return New TValue(a.Cast(Of Double)() - b.Cast(Of Integer)())
        ElseIf (a.GetObjectType = Tokens.T_Integer And b.GetObjectType = Tokens.T_Float) Then
            Return New TValue(a.Cast(Of Integer)() - b.Cast(Of Double)())
        ElseIf (a.GetObjectType = Tokens.T_Float And b.GetObjectType = Tokens.T_Float) Then
            Return New TValue(a.Cast(Of Double)() - b.Cast(Of Double)())
        Else
            Return TValue.Null
        End If
    End Function

    Public Shared Function Multiplication(a As TValue, b As TValue) As TValue
        If (a.GetObjectType = Tokens.T_Integer And b.GetObjectType = Tokens.T_Integer) Then
            Return New TValue(a.Cast(Of Integer)() * b.Cast(Of Integer)())
        ElseIf (a.GetObjectType = Tokens.T_Float And b.GetObjectType = Tokens.T_Integer) Then
            Return New TValue(a.Cast(Of Double)() * b.Cast(Of Integer)())
        ElseIf (a.GetObjectType = Tokens.T_Integer And b.GetObjectType = Tokens.T_Float) Then
            Return New TValue(a.Cast(Of Integer)() * b.Cast(Of Double)())
        ElseIf (a.GetObjectType = Tokens.T_Float And b.GetObjectType = Tokens.T_Float) Then
            Return New TValue(a.Cast(Of Double)() * b.Cast(Of Double)())
        ElseIf (a.GetObjectType = Tokens.T_String And b.GetObjectType = Tokens.T_Integer) Then
            Return Operators.Repeat(a, b)
        ElseIf (a.GetObjectType = Tokens.T_Integer And b.GetObjectType = Tokens.T_String) Then
            Return Operators.Repeat(b, a)
        Else
            Return TValue.Null
        End If
    End Function

    Public Shared Function Division(a As TValue, b As TValue) As TValue
        If (a.GetObjectType = Tokens.T_Integer And b.GetObjectType = Tokens.T_Integer) Then
            If (b.Cast(Of Integer) = 0) Then Throw New ScriptError("division by zero")
            Return New TValue(a.Cast(Of Integer)() \ b.Cast(Of Integer)())
        ElseIf (a.GetObjectType = Tokens.T_Float And b.GetObjectType = Tokens.T_Integer) Then
            If (b.Cast(Of Integer) = 0) Then Throw New ScriptError("division by zero")
            Return New TValue(a.Cast(Of Double)() / b.Cast(Of Integer)())
        ElseIf (a.GetObjectType = Tokens.T_Integer And b.GetObjectType = Tokens.T_Float) Then
            If (b.Cast(Of Double) = 0) Then Throw New ScriptError("division by zero")
            Return New TValue(a.Cast(Of Integer)() / b.Cast(Of Double)())
        ElseIf (a.GetObjectType = Tokens.T_Float And b.GetObjectType = Tokens.T_Float) Then
            If (b.Cast(Of Double) = 0) Then Throw New ScriptError("division by zero")
            Return New TValue(a.Cast(Of Double)() / b.Cast(Of Double)())
        Else
            Return TValue.Null
        End If
    End Function

    Public Shared Function Modulo(a As TValue, b As TValue) As TValue
        If (a.GetObjectType = Tokens.T_Integer And b.GetObjectType = Tokens.T_Integer) Then
            Return New TValue(a.Cast(Of Integer)() Mod b.Cast(Of Integer)())
        ElseIf (a.GetObjectType = Tokens.T_Float And b.GetObjectType = Tokens.T_Integer) Then
            Return New TValue(a.Cast(Of Double)() Mod b.Cast(Of Integer)())
        ElseIf (a.GetObjectType = Tokens.T_Integer And b.GetObjectType = Tokens.T_Float) Then
            Return New TValue(a.Cast(Of Integer)() Mod b.Cast(Of Double)())
        ElseIf (a.GetObjectType = Tokens.T_Float And b.GetObjectType = Tokens.T_Float) Then
            Return New TValue(a.Cast(Of Double)() Mod b.Cast(Of Double)())
        Else
            Return TValue.Null
        End If
    End Function

    Public Shared Function Sign(a As TValue, op As Tokens) As TValue
        If (op = Tokens.T_Plus) Then
            Return Operators.SignPositive(a)
        ElseIf (op = Tokens.T_Minus) Then
            Return Operators.SignNegative(a)
        Else
            Return TValue.Null
        End If
    End Function

    Public Shared Function SignPositive(a As TValue) As TValue
        If (a.GetObjectType = Tokens.T_Integer) Then
            Return New TValue(a.Cast(Of Integer)() * 1)
        ElseIf (a.GetObjectType = Tokens.T_Float) Then
            Return New TValue(a.Cast(Of Double)() * 1.0R)
        Else
            Return TValue.Null
        End If
    End Function

    Public Shared Function SignNegative(a As TValue) As TValue
        If (a.GetObjectType = Tokens.T_Integer) Then
            Return New TValue(a.Cast(Of Integer)() * -1)
        ElseIf (a.GetObjectType = Tokens.T_Float) Then
            Return New TValue(a.Cast(Of Double)() * -1.0R)
        Else
            Return TValue.Null
        End If
    End Function

    Public Shared Function [And](a As TValue, b As TValue) As TValue
        If (a.GetObjectType = Tokens.T_Bool And b.GetObjectType = Tokens.T_Bool) Then
            Return New TValue(a.Cast(Of Boolean)() And b.Cast(Of Boolean)())
        ElseIf (a.GetObjectType = Tokens.T_Integer And b.GetObjectType = Tokens.T_Integer) Then
            Return New TValue(a.Cast(Of Integer)() And b.Cast(Of Integer)())
        Else
            Return TValue.Null
        End If
    End Function

    Public Shared Function [Or](a As TValue, b As TValue) As TValue
        If (a.GetObjectType = Tokens.T_Bool And b.GetObjectType = Tokens.T_Bool) Then
            Return New TValue(a.Cast(Of Boolean)() Or b.Cast(Of Boolean)())
        ElseIf (a.GetObjectType = Tokens.T_Integer And b.GetObjectType = Tokens.T_Integer) Then
            Return New TValue(a.Cast(Of Integer)() Or b.Cast(Of Integer)())
        Else
            Return TValue.Null
        End If
    End Function

    Public Shared Function [Xor](a As TValue, b As TValue) As TValue
        If (a.GetObjectType = Tokens.T_Bool And b.GetObjectType = Tokens.T_Bool) Then
            Return New TValue(a.Cast(Of Boolean)() And b.Cast(Of Boolean)())
        ElseIf (a.GetObjectType = Tokens.T_Integer Xor b.GetObjectType = Tokens.T_Integer) Then
            Return New TValue(a.Cast(Of Integer)() Xor b.Cast(Of Integer)())
        Else
            Return TValue.Null
        End If
    End Function

    Public Shared Function [Not](a As TValue) As TValue
        If (a.GetObjectType = Tokens.T_Integer) Then
            Return New TValue(Not a.Cast(Of Integer)())
        ElseIf (a.GetObjectType = Tokens.T_Bool) Then
            Return New TValue(Not a.Cast(Of Boolean)())
        Else
            Return TValue.Null
        End If
    End Function

    Public Shared Function ShiftRight(a As TValue, b As TValue) As TValue
        If (a.GetObjectType = Tokens.T_Integer And b.GetObjectType = Tokens.T_Integer) Then
            Return New TValue(a.Cast(Of Integer)() >> b.Cast(Of Integer)())
        Else
            Return TValue.Null
        End If
    End Function

    Public Shared Function ShiftLeft(a As TValue, b As TValue) As TValue
        If (a.GetObjectType = Tokens.T_Integer And b.GetObjectType = Tokens.T_Integer) Then
            Return New TValue(a.Cast(Of Integer)() << b.Cast(Of Integer)())
        Else
            Return TValue.Null
        End If
    End Function

    Public Shared Function IsEqual(a As TValue, b As TValue) As TValue
        Return New TValue(a.Value.Equals(b.Value))
    End Function

    Public Shared Function IsNotEqual(a As TValue, b As TValue) As TValue
        Return New TValue(Not a.Value.Equals(b.Value))
    End Function

    Public Shared Function IsGreater(a As TValue, b As TValue) As TValue
        If (a.GetObjectType = Tokens.T_Integer And b.GetObjectType = Tokens.T_Integer) Then
            Return New TValue(a.Cast(Of Integer)() > b.Cast(Of Integer)())
        ElseIf (a.GetObjectType = Tokens.T_Float And b.GetObjectType = Tokens.T_Integer) Then
            Return New TValue(a.Cast(Of Double)() > b.Cast(Of Integer)())
        ElseIf (a.GetObjectType = Tokens.T_Integer And b.GetObjectType = Tokens.T_Float) Then
            Return New TValue(a.Cast(Of Integer)() > b.Cast(Of Double)())
        ElseIf (a.GetObjectType = Tokens.T_Float And b.GetObjectType = Tokens.T_Float) Then
            Return New TValue(a.Cast(Of Double)() > b.Cast(Of Double)())
        Else
            Return TValue.Null
        End If
    End Function

    Public Shared Function IsLesser(a As TValue, b As TValue) As TValue
        If (a.GetObjectType = Tokens.T_Integer And b.GetObjectType = Tokens.T_Integer) Then
            Return New TValue(a.Cast(Of Integer)() < b.Cast(Of Integer)())
        ElseIf (a.GetObjectType = Tokens.T_Float And b.GetObjectType = Tokens.T_Integer) Then
            Return New TValue(a.Cast(Of Double)() < b.Cast(Of Integer)())
        ElseIf (a.GetObjectType = Tokens.T_Integer And b.GetObjectType = Tokens.T_Float) Then
            Return New TValue(a.Cast(Of Integer)() < b.Cast(Of Double)())
        ElseIf (a.GetObjectType = Tokens.T_Float And b.GetObjectType = Tokens.T_Float) Then
            Return New TValue(a.Cast(Of Double)() < b.Cast(Of Double)())
        Else
            Return TValue.Null
        End If
    End Function

    Public Shared Function IsEqualOrGreater(a As TValue, b As TValue) As TValue
        If (a.GetObjectType = Tokens.T_Integer And b.GetObjectType = Tokens.T_Integer) Then
            Return New TValue(a.Cast(Of Integer)() >= b.Cast(Of Integer)())
        ElseIf (a.GetObjectType = Tokens.T_Float And b.GetObjectType = Tokens.T_Integer) Then
            Return New TValue(a.Cast(Of Double)() >= b.Cast(Of Integer)())
        ElseIf (a.GetObjectType = Tokens.T_Integer And b.GetObjectType = Tokens.T_Float) Then
            Return New TValue(a.Cast(Of Integer)() >= b.Cast(Of Double)())
        ElseIf (a.GetObjectType = Tokens.T_Float And b.GetObjectType = Tokens.T_Float) Then
            Return New TValue(a.Cast(Of Double)() >= b.Cast(Of Double)())
        Else
            Return TValue.Null
        End If
    End Function

    Public Shared Function IsEqualOrLesser(a As TValue, b As TValue) As TValue
        If (a.GetObjectType = Tokens.T_Integer And b.GetObjectType = Tokens.T_Integer) Then
            Return New TValue(a.Cast(Of Integer)() <= b.Cast(Of Integer)())
        ElseIf (a.GetObjectType = Tokens.T_Float And b.GetObjectType = Tokens.T_Integer) Then
            Return New TValue(a.Cast(Of Double)() <= b.Cast(Of Integer)())
        ElseIf (a.GetObjectType = Tokens.T_Integer And b.GetObjectType = Tokens.T_Float) Then
            Return New TValue(a.Cast(Of Integer)() <= b.Cast(Of Double)())
        ElseIf (a.GetObjectType = Tokens.T_Float And b.GetObjectType = Tokens.T_Float) Then
            Return New TValue(a.Cast(Of Double)() <= b.Cast(Of Double)())
        Else
            Return TValue.Null
        End If
    End Function

    Public Shared Function IsLike(a As TValue, b As TValue) As TValue
        Return New TValue(a.Value.ToString Like b.Value.ToString)
    End Function

    Public Shared Function Repeat(v As TValue, c As TValue) As TValue
        Dim count As Integer = c.Cast(Of Integer)()
        If (count >= 0 AndAlso count <= Byte.MaxValue) Then
            Return New TValue(Operators.Repeat(v.Cast(Of String), count))
        End If
        Throw New Exception(String.Format("Count value out of range, max = {0}", Byte.MaxValue))
    End Function

    Public Shared Function Repeat(v As String, c As Integer) As String
        Return New StringBuilder(v.Length * c).Insert(0, v, c).ToString
    End Function

End Class
