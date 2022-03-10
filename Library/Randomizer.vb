Imports System.Security.Cryptography

Namespace Library
    Public Class Randomizer
        <ScriptFunction("rnd")>
        Public Shared Function RandomInteger(rt As Runtime, min As Integer, max As Integer) As Integer
            Static r As New Random(Randomizer.RandomNonZeroBytes(rt, 3))
            Return r.Next(min, max)
        End Function

        <ScriptFunction("rng")>
        Public Shared Function RandomNonZeroBytes(rt As Runtime, length As Integer) As Integer
            Static generator As RandomNumberGenerator = RandomNumberGenerator.Create()
            Dim buffer() As Byte = New Byte(length) {}
            generator.GetNonZeroBytes(buffer)
            Return BitConverter.ToInt32(buffer)
        End Function
    End Class
End Namespace
