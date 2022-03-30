Imports Bastion.Expressions.Types

Namespace Expressions
    Public NotInheritable Class ForLoop
        Inherits Expression
        Public Property Init As Expression
        Public Property Limit As Expression
        Public Property [Step] As Expression
        Public Property Body As List(Of Expression)
        Sub New(params As List(Of Expression))
            If (params.Count = 2) Then
                Me.Init = params(0)
                Me.Limit = params(1)
                Me.Step = New [Integer](1)
                Me.Body = New List(Of Expression)
            ElseIf (params.Count = 3) Then
                Me.Init = params(0)
                Me.Limit = params(1)
                Me.Step = params(2)
                Me.Body = New List(Of Expression)
            Else
                Throw New ScriptError("parameter mismatch 'for loop'")
            End If
        End Sub
        Public Shared Function Valid(e As ForLoop) As Boolean
            Return (TypeOf e.Init Is Binary AndAlso CType(e.Init, Binary).Op = Tokens.T_Assign)
        End Function
        Public Overrides Function ToString() As String
            Return String.Format("For {0} to {1} step {2}", Me.Init, Me.Limit, Me.Step)
        End Function
    End Class
End Namespace