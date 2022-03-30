Imports System.Text

#Disable Warning BC42304

Public NotInheritable Class Operators
    ''' <summary>
    ''' Returns the sum of a + b    
    ''' </summary>
    ''' <param name="a"></param>
    ''' <param name="b"></param>

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

    ''' <summary>
    ''' Returns the sum of a - b    
    ''' </summary>
    ''' <param name="a"></param>
    ''' <param name="b"></param>

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

    ''' <summary>
    ''' ''' Returns the sum of a * b    
    ''' </summary>
    ''' <param name="a"></param>
    ''' <param name="b"></param>

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

    ''' <summary>
    ''' Returns the sum of a / b    
    ''' </summary>
    ''' <param name="a"></param>
    ''' <param name="b"></param>

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

    ''' <summary>
    ''' ''' Returns the sum of a % b    
    ''' </summary>
    ''' <param name="a"></param>
    ''' <param name="b"></param>

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

    ''' <summary>
    ''' Signs a value defined op token type (negative or positive)
    ''' </summary>
    ''' <param name="a"></param>
    ''' <param name="op"></param>

    Public Shared Function Sign(a As TValue, op As Tokens) As TValue
        If (op = Tokens.T_Plus) Then
            Return Operators.SignPositive(a)
        ElseIf (op = Tokens.T_Minus) Then
            Return Operators.SignNegative(a)
        Else
            Return TValue.Null
        End If
    End Function

    ''' <summary>
    ''' Signs a value positively
    ''' </summary>
    ''' <param name="a"></param>

    Public Shared Function SignPositive(a As TValue) As TValue
        If (a.GetObjectType = Tokens.T_Integer) Then
            Return New TValue(a.Cast(Of Integer)() * 1)
        ElseIf (a.GetObjectType = Tokens.T_Float) Then
            Return New TValue(a.Cast(Of Double)() * 1.0R)
        Else
            Return TValue.Null
        End If
    End Function

    ''' <summary>
    ''' Signs a value negatively
    ''' </summary>
    ''' <param name="a"></param>

    Public Shared Function SignNegative(a As TValue) As TValue
        If (a.GetObjectType = Tokens.T_Integer) Then
            Return New TValue(a.Cast(Of Integer)() * -1)
        ElseIf (a.GetObjectType = Tokens.T_Float) Then
            Return New TValue(a.Cast(Of Double)() * -1.0R)
        Else
            Return TValue.Null
        End If
    End Function

    ''' <summary>
    ''' Returns the logical AND result between A and B
    ''' </summary>
    ''' <param name="a"></param>
    ''' <param name="b"></param>

    Public Shared Function [And](a As TValue, b As TValue) As TValue
        If (a.GetObjectType = Tokens.T_Bool And b.GetObjectType = Tokens.T_Bool) Then
            Return New TValue(a.Cast(Of Boolean)() And b.Cast(Of Boolean)())
        ElseIf (a.GetObjectType = Tokens.T_Integer And b.GetObjectType = Tokens.T_Integer) Then
            Return New TValue(a.Cast(Of Integer)() And b.Cast(Of Integer)())
        Else
            Return TValue.Null
        End If
    End Function

    ''' <summary>
    ''' Returns the logical OR result between A and B
    ''' </summary>
    ''' <param name="a"></param>
    ''' <param name="b"></param>

    Public Shared Function [Or](a As TValue, b As TValue) As TValue
        If (a.GetObjectType = Tokens.T_Bool And b.GetObjectType = Tokens.T_Bool) Then
            Return New TValue(a.Cast(Of Boolean)() Or b.Cast(Of Boolean)())
        ElseIf (a.GetObjectType = Tokens.T_Integer And b.GetObjectType = Tokens.T_Integer) Then
            Return New TValue(a.Cast(Of Integer)() Or b.Cast(Of Integer)())
        Else
            Return TValue.Null
        End If
    End Function

    ''' <summary>
    ''' Returns the logical XOR result between A and B
    ''' </summary>
    ''' <param name="a"></param>
    ''' <param name="b"></param>

    Public Shared Function [Xor](a As TValue, b As TValue) As TValue
        If (a.GetObjectType = Tokens.T_Bool And b.GetObjectType = Tokens.T_Bool) Then
            Return New TValue(a.Cast(Of Boolean)() And b.Cast(Of Boolean)())
        ElseIf (a.GetObjectType = Tokens.T_Integer Xor b.GetObjectType = Tokens.T_Integer) Then
            Return New TValue(a.Cast(Of Integer)() Xor b.Cast(Of Integer)())
        Else
            Return TValue.Null
        End If
    End Function

    ''' <summary>
    ''' Returns the logical NOT result of A
    ''' </summary>
    ''' <param name="a"></param>

    Public Shared Function [Not](a As TValue) As TValue
        If (a.GetObjectType = Tokens.T_Integer) Then
            Return New TValue(Not a.Cast(Of Integer)())
        ElseIf (a.GetObjectType = Tokens.T_Bool) Then
            Return New TValue(Not a.Cast(Of Boolean)())
        Else
            Return TValue.Null
        End If
    End Function

    ''' <summary>
    ''' Shifts value A > B
    ''' </summary>
    ''' <param name="a"></param>
    ''' <param name="b"></param>

    Public Shared Function ShiftRight(a As TValue, b As TValue) As TValue
        If (a.GetObjectType = Tokens.T_Integer And b.GetObjectType = Tokens.T_Integer) Then
            Return New TValue(a.Cast(Of Integer)() >> b.Cast(Of Integer)())
        Else
            Return TValue.Null
        End If
    End Function

    ' XML documentation parse error
    ''' <summary>
    ''' Shifts value A < B
    ''' </summary>
    ''' <param name="a"></param>
    ''' <param name="b"></param>

    Public Shared Function ShiftLeft(a As TValue, b As TValue) As TValue
#Enable Warning BC42304 ' XML documentation parse error
        If (a.GetObjectType = Tokens.T_Integer And b.GetObjectType = Tokens.T_Integer) Then
            Return New TValue(a.Cast(Of Integer)() << b.Cast(Of Integer)())
        Else
            Return TValue.Null
        End If
    End Function

    ''' <summary>
    ''' Returns a TValue (boolean) if a is equal b
    ''' </summary>
    ''' <param name="a"></param>
    ''' <param name="b"></param>

    Public Shared Function IsEqual(a As TValue, b As TValue) As TValue
        Return New TValue(a.Value.Equals(b.Value))
    End Function

    ''' <summary>
    ''' Returns a TValue (boolean) if a not eqauals b
    ''' </summary>
    ''' <param name="a"></param>
    ''' <param name="b"></param>

    Public Shared Function IsNotEqual(a As TValue, b As TValue) As TValue
        Return New TValue(Not a.Value.Equals(b.Value))
    End Function

    ''' <summary>
    ''' Returns a TValue (boolean) if a is greater then b
    ''' </summary>
    ''' <param name="a"></param>
    ''' <param name="b"></param>

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

    ''' <summary>
    ''' Returns a TValue (boolean) if a is lesser then b
    ''' </summary>
    ''' <param name="a"></param>
    ''' <param name="b"></param>

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

    ''' <summary>
    ''' Returns a TValue (boolean) if a is equal or greater then b
    ''' </summary>
    ''' <param name="a"></param>
    ''' <param name="b"></param>

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

    ''' <summary>
    ''' Returns a TValue (boolean) if a is equal or lesser then b
    ''' </summary>
    ''' <param name="a"></param>
    ''' <param name="b"></param>

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

    ''' <summary>
    ''' Returns if a value is like b value using the Like operator
    ''' </summary>
    ''' <param name="a"></param>
    ''' <param name="b"></param>

    Public Shared Function IsLike(a As TValue, b As TValue) As TValue
        Return New TValue(a.Value.ToString Like b.Value.ToString)
    End Function

    ''' <summary>
    ''' Repeats a value of TValue defined by (c)ount.
    ''' </summary>
    ''' <param name="v"></param>
    ''' <param name="c"></param>

    Public Shared Function Repeat(v As TValue, c As TValue) As TValue
        Dim count As Integer = c.Cast(Of Integer)()
        If (count >= 0 AndAlso count <= Byte.MaxValue) Then
            Return New TValue(Operators.Repeat(v.Cast(Of String), count))
        End If
        Throw New Exception(String.Format("Count value out of range, max = {0}", Byte.MaxValue))
    End Function

    ''' <summary>
    ''' Repeats a string defined by (c)ount.
    ''' </summary>
    ''' <param name="v"></param>
    ''' <param name="c"></param>

    Public Shared Function Repeat(v As String, c As Integer) As String
        Return New StringBuilder(v.Length * c).Insert(0, v, c).ToString
    End Function

End Class
