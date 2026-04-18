Public MustInherit Class BaseButton
    Protected bounds As Rectangle
    Protected action As Action
    Protected currentScale As Double = 1.0
    Private Const INITIAL_SCALE As Double = 1.0
    Private Const HOVER_SCALE As Double = 1.1
    Private Const SCALE_SPEED As Double = 0.1


    Public Sub New(x As Integer, y As Integer, width As Integer, height As Integer, action As Action)
        bounds = New Rectangle(x, y, width, height)


        Me.action = action
    End Sub

    Public Sub HandleClick(mouseX As Integer, mouseY As Integer)
        If bounds.Contains(mouseX, mouseY) Then
            action.Invoke()
            'audioController.Play("ClickSound")
        End If
    End Sub

    Protected Sub UpdateScale(isHover As Boolean)
        Dim targetScale = If(isHover, HOVER_SCALE, INITIAL_SCALE)

        currentScale += (targetScale - currentScale) * SCALE_SPEED
    End Sub

    Protected Function GetScaledBounds() As Rectangle
        Dim newWidth = bounds.Width * currentScale
        Dim newHeight = bounds.Height * currentScale

        Dim offsetX = (newWidth - bounds.Width) / 2
        Dim offsetY = (newHeight - bounds.Height) / 2

        Return New Rectangle(
        bounds.X - offsetX,
        bounds.Y - offsetY,
        newWidth,
        newHeight
    )
    End Function
    Public MustOverride Sub Draw(g As Graphics, mouseX As Integer, mouseY As Integer)
End Class
