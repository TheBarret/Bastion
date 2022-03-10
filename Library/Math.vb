Namespace Library
    Public Class Math
        <ScriptFunction("sqrt")>
        Public Shared Function Sqrt(rt As Runtime, value As Double) As Double
            Return System.Math.Sqrt(value)
        End Function

        <ScriptFunction("pow")>
        Public Shared Function Pow(rt As Runtime, x As Double, y As Double) As Double
            Return System.Math.Pow(x, y)
        End Function

        <ScriptFunction("abs")>
        Public Shared Function Abs(rt As Runtime, value As Double) As Double
            Return System.Math.Abs(value)
        End Function

        <ScriptFunction("sin")>
        Public Shared Function Sin(rt As Runtime, value As Double) As Double
            Return System.Math.Sin(value)
        End Function

        <ScriptFunction("cos")>
        Public Shared Function Cos(rt As Runtime, value As Double) As Double
            Return System.Math.Cos(value)
        End Function

        <ScriptFunction("tan")>
        Public Shared Function Tan(rt As Runtime, value As Double) As Double
            Return System.Math.Tan(value)
        End Function

        <ScriptFunction("sinh")>
        Public Shared Function Sinh(rt As Runtime, value As Double) As Double
            Return System.Math.Sinh(value)
        End Function

        <ScriptFunction("cosh")>
        Public Shared Function Cosh(rt As Runtime, value As Double) As Double
            Return System.Math.Cosh(value)
        End Function

        <ScriptFunction("tanh")>
        Public Shared Function Tanh(rt As Runtime, value As Double) As Double
            Return System.Math.Tanh(value)
        End Function

        <ScriptFunction("acos")>
        Public Shared Function Acos(rt As Runtime, value As Double) As Double
            Return System.Math.Acos(value)
        End Function

        <ScriptFunction("asin")>
        Public Shared Function Asin(rt As Runtime, value As Double) As Double
            Return System.Math.Asin(value)
        End Function

        <ScriptFunction("atan")>
        Public Shared Function Atan(rt As Runtime, value As Double) As Double
            Return System.Math.Atan(value)
        End Function

        <ScriptFunction("atan2")>
        Public Shared Function Atan2(rt As Runtime, x As Double, y As Double) As Double
            Return System.Math.Atan2(x, y)
        End Function

        <ScriptFunction("acosh")>
        Public Shared Function Acosh(rt As Runtime, value As Double) As Double
            Return System.Math.Acosh(value)
        End Function

        <ScriptFunction("asinh")>
        Public Shared Function Asinh(rt As Runtime, value As Double) As Double
            Return System.Math.Asinh(value)
        End Function

        <ScriptFunction("atanh")>
        Public Shared Function Atanh(rt As Runtime, value As Double) As Double
            Return System.Math.Atanh(value)
        End Function

        <ScriptFunction("pi")>
        Public Shared Function Pi(rt As Runtime) As Double
            Return System.Math.PI
        End Function

        <ScriptFunction("max")>
        Public Shared Function Max(rt As Runtime, x As Double, y As Double) As Double
            Return System.Math.Max(x, y)
        End Function

        <ScriptFunction("cbrt")>
        Public Shared Function Cbrt(rt As Runtime, value As Double) As Double
            Return System.Math.Cbrt(value)
        End Function

        <ScriptFunction("clamp")>
        Public Shared Function Clamp(rt As Runtime, x As Double, y As Double, z As Double) As Double
            Return System.Math.Clamp(x, y, z)
        End Function

        <ScriptFunction("ceiling")>
        Public Shared Function Ceiling(rt As Runtime, value As Double) As Double
            Return System.Math.Ceiling(value)
        End Function

        <ScriptFunction("floor")>
        Public Shared Function Floor(rt As Runtime, value As Double) As Double
            Return System.Math.Floor(value)
        End Function

        <ScriptFunction("e")>
        Public Shared Function E(rt As Runtime) As Double
            Return System.Math.E
        End Function

        <ScriptFunction("round")>
        Public Shared Function Round(rt As Runtime, x As Double, y As Integer) As Double
            Return System.Math.Round(x, y)
        End Function
    End Class
End Namespace