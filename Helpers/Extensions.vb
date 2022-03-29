Imports System.Reflection
Imports System.Runtime.CompilerServices
Imports Exp = System.Linq.Expressions.Expression

Public Module Extensions

    ''' <summary>
    ''' Returns true if type matches and has the correct attributes.
    ''' </summary>
    <Extension()>
    Public Function IsValid(type As Type, ref As String, domain As String) As Boolean
        Return type.IsClass AndAlso type.Namespace = domain AndAlso type.Name.Equals(ref, StringComparison.CurrentCultureIgnoreCase)
    End Function

    ''' <summary>
    ''' Returns a shortened string to a specified length and adds padding if the string is longer than the specified length.
    ''' </summary>
    <Extension>
    Public Function Truncate(value As String, max As Integer, Optional padding As String = "") As String
        Return If(value.Length <= max, value, String.Format("{0}{1}", value.Substring(0, max), padding))
    End Function

    ''' <summary>
    '''  Returns a delegate located by name in given type.
    ''' </summary>
    <Extension>
    Public Function CreateDelegate(Type As Type, Name As String) As [Delegate]
        Dim method As MethodInfo = Type.GetMethod(Name, BindingFlags.Public Or BindingFlags.Static Or BindingFlags.IgnoreCase)
        If (method IsNot Nothing) Then
            Return method.CreateDelegate(Exp.GetDelegateType((From p In method.GetParameters Select p.ParameterType).Concat(New Type() {method.ReturnType}).ToArray))
        End If
        Throw New ScriptError(String.Format("unable to import '{0}.{1}'", Type.Name, Name))
    End Function

    ''' <summary>
    ''' Returns the string representation of the given object.
    ''' </summary>
    <Extension>
    Public Function Name(token As Tokens) As String
        Select Case token
            Case Tokens.T_Null : Return "Null"
            Case Tokens.T_Stream : Return "Stream"
            Case Tokens.T_String : Return "String"
            Case Tokens.T_Integer : Return "Integer"
            Case Tokens.T_Float : Return "Float"
            Case Tokens.T_Bool : Return "Bool"
            Case Tokens.T_Identifier : Return "Identifier"
            Case Tokens.T_Function : Return "Function"
            Case Tokens.T_Array : Return "Array"
            Case Tokens.T_Plus : Return "+"
            Case Tokens.T_Minus : Return "-"
            Case Tokens.T_Mult : Return "*"
            Case Tokens.T_Div : Return "/"
            Case Tokens.T_Mod : Return "%"
            Case Tokens.T_Assign : Return "="
            Case Tokens.T_Equal : Return "=="
            Case Tokens.T_NotEqual : Return "!="
            Case Tokens.T_Greater : Return ">"
            Case Tokens.T_Lesser : Return "<"
            Case Tokens.T_EqualOrGreater : Return "=>"
            Case Tokens.T_EqualOrLesser : Return "=<"
            Case Tokens.T_If : Return "If"
            Case Tokens.T_Else : Return "Else"
            Case Tokens.T_Return : Return "Return"
            Case Tokens.T_For : Return "For"
            Case Tokens.T_Or : Return "|"
            Case Tokens.T_And : Return "&"
            Case Tokens.T_Xor : Return "^"
            Case Tokens.T_Colon : Return ":"
            Case Tokens.T_Comma : Return ","
            Case Tokens.T_Dot : Return "."
            Case Tokens.T_Negate : Return "!"
            Case Tokens.T_ParenthesisOpen : Return "("
            Case Tokens.T_ParenthesisClose : Return ")"
            Case Tokens.T_BraceOpen : Return "{"
            Case Tokens.T_BraceClose : Return "}"
            Case Tokens.T_BracketOpen : Return "["
            Case Tokens.T_BracketClose : Return "]"
            Case Tokens.T_Increment : Return "++"
            Case Tokens.T_Decrement : Return "--"
            Case Tokens.T_EndStatement : Return ";"
            Case Tokens.T_EndOfFile : Return "[EOF]"
            Case Else
                Return token.ToString
        End Select
    End Function
End Module
