''' <summary>
''' These token types define what types of tokens are recognized by the parser.
''' </summary>
Public Enum Tokens As Byte
    T_Null = &H0
    T_String
    T_Integer
    T_Float
    T_Bool
    T_Identifier
    T_Stream
    T_Hexadecimal
    T_Function

    T_Plus
    T_Minus
    T_Mult
    T_Div
    T_Mod

    T_Assign

    T_Increment
    T_Decrement

    T_Equal
    T_NotEqual
    T_Greater
    T_Lesser
    T_EqualOrGreater
    T_EqualOrLesser

    T_If
    T_Else
    T_Use
    T_Return
    T_For

    T_Or
    T_And
    T_Xor

    T_Colon
    T_Comma
    T_Dot
    T_Negate

    T_ParenthesisOpen
    T_ParenthesisClose
    T_BraceOpen
    T_BraceClose
    T_BracketOpen
    T_BracketClose

    T_Undefined
    T_Delegate

    T_Space
    T_Newline
    T_BlockComment
    T_LineComment

    T_NOP
    T_EndStatement
    T_EndOfFile = &HFF
End Enum