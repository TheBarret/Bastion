Imports System.Text.RegularExpressions

Namespace Parsers
    Public Class Lexer
        Implements IDisposable
        Private disposedValue As Boolean
        Private Property Length As Integer
        Private Property Flag As Boolean
        Private Property Line As Integer
        Private Property Index As Integer
        Private Property Parent As Runtime
        Private Property Stream As List(Of Token)

        Sub New(parent As Runtime)
            Me.Line = 1
            Me.Index = 0
            Me.Stream = New List(Of Token)
            Me.Parent = parent
        End Sub

        Public Function Analyze(context As String) As List(Of Token)
            Me.Parent.Session.Log("Beginning lexical analysis ...")
            If (Not String.IsNullOrEmpty(context)) Then
                Dim sw As New Stopwatch
                sw.Start()
                Me.Length = context.Length
                Using Definitions As New Syntax
                    Do
                        Me.Flag = False
                        For Each Rule As KeyValuePair(Of Tokens, String) In Definitions
                            Dim match As Match = Definitions.Match(Rule.Key, context)
                            If (match.Success) Then
                                Me.Flag = True
                                context = context.Remove(match.Index, match.Length)
                                If (Rule.Key = Tokens.T_Newline) Then
                                    Me.Line += 1
                                ElseIf (Rule.Key = Tokens.T_LineComment) Then
                                    Me.Line += Regex.Matches(match.Value, "\r\n").Count
                                ElseIf (Rule.Key = Tokens.T_BlockComment) Then
                                    Me.Line += Regex.Matches(match.Value, "\r\n").Count
                                ElseIf (Rule.Key = Tokens.T_Use) Then
                                    Me.Line += 1
                                End If
                                Me.Stream.Add(New Token(match.Value, Rule.Key, Me.Line, Me.Index, match.Length))
                                Me.Index += match.Length
                            End If
                            If (Me.Flag) Then Exit For
                        Next
                        If (Not Me.Flag) Then
                            Throw New ScriptError(String.Format("Undefined symbol '{0}' at index {1} line {2}", context(0), Me.Index, Me.Line))
                        End If
                    Loop Until Index = Me.Length
                End Using
                sw.Stop()
                Me.Parent.Session.Log(String.Format("Finished in {0}", sw.Elapsed.Duration))
                Me.Stream.Add(Token.Create(Tokens.T_EndOfFile, String.Empty, Me.Line, Me.Index))
                Return Lexer.Remove(Me.Stream, Tokens.T_Space, Tokens.T_Newline, Tokens.T_BlockComment, Tokens.T_LineComment)
            End If
            Throw New ScriptError("context is empty")
        End Function

        Public Shared ReadOnly Property Remove(input As List(Of Token), ParamArray Types() As Tokens) As List(Of Token)
            Get
                Return input.Where(Function(token) Not Types.Contains(token.Type)).ToList
            End Get
        End Property

        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not disposedValue Then
                If disposing Then
                    Me.Stream.Clear()
                End If
                disposedValue = True
            End If
        End Sub

        Public Sub Dispose() Implements IDisposable.Dispose
            Me.Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub
    End Class
End Namespace