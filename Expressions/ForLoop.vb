Imports Bastion.Expressions.Types

Namespace Expressions
    Public NotInheritable Class ForLoop
        Inherits Expression
        Public Property Init As Expression
        Public Property Condition As Expression
        Public Property [Step] As Expression
        Public Property Body As List(Of Expression)
        Sub New(params As List(Of Expression))
            If (params.Count = 2) Then
                Me.Init = params(0)
                Me.Condition = params(1)
                Me.Step = New [Integer](1)
                Me.Body = New List(Of Expression)
            ElseIf (params.Count = 3) Then
                Me.Init = params(0)
                Me.Condition = params(1)
                Me.Step = params(2)
                Me.Body = New List(Of Expression)
            Else
                Throw New ScriptError("parameter mismatch 'for loop'")
            End If
        End Sub
        Public Overrides Function ToString() As String
            Return String.Format("For ({0};{1};{2})", Me.Init, Me.Condition, Me.Step)
        End Function
    End Class
End Namespace