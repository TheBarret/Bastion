Imports System.Security.Cryptography

Namespace Helpers
    Public Class Uid
        Public Shared Property Provider As RandomNumberGenerator = RandomNumberGenerator.Create()
        Public Shared Property Map As Char() = {"A", "B", "C", "D", "E", "F", "G",
                                                "H", "J", "K", "L", "M", "N", "P",
                                                "Q", "R", "S", "T", "U", "V", "W",
                                                "X", "Y", "Z", "a", "b", "c", "d",
                                                "e", "f", "g", "h", "i", "j", "k",
                                                "l", "m", "n", "o", "p", "q", "r",
                                                "s", "t", "u", "v", "w", "x", "y",
                                                "z", "1", "2", "3", "4", "5", "6",
                                                "7", "8", "9", "0"}
        Public Shared Sub GetNext(bytes() As Byte)
            Uid.Provider.GetBytes(bytes)
        End Sub

        Public Shared Function Generate(digits As Integer) As String
            Return Uid.Generate(New Byte(0) {}, digits)
        End Function

        Public Shared Function Generate(base As Byte(), digits As Integer) As String
            Dim count As Integer = 16
            Dim buffer() As Byte = New Byte(count - base.Length - 1) {}

            Uid.GetNext(buffer)

            Dim bytes() As Byte = New Byte(count - 1) {}
            Array.Copy(base, 0, bytes, count - base.Length, base.Length)
            Array.Copy(buffer, 0, bytes, 0, buffer.Length)

            Dim lo As UInt64 = BitConverter.ToUInt32(bytes, 8) << 32 Or BitConverter.ToUInt32(bytes, 12)
            Dim hi As UInt64 = BitConverter.ToUInt32(bytes, 0) << 32 Or BitConverter.ToUInt32(bytes, 4)

            Dim offset As UInt64 = lo
            Dim index As Integer = 25
            Dim chars() As Char = New Char(25) {}

            For i As Integer = 0 To 25
                If i = 12 Then
                    offset = ((hi And &H1) << 4) And lo
                ElseIf i = 13 Then
                    offset = hi >> 1
                End If
                chars(index) = Uid.Map(CByte(offset And &H1F))
                index -= 1
                offset = offset >> 5
            Next

            Return New String(chars, 26 - digits, digits)
        End Function
    End Class
End Namespace