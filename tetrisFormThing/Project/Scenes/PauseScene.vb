Public Class PauseScene
    Inherits Scene
    Private previousScene As GameScene
    Public Sub New(screenWidth As Integer, screenHeight As Integer, sceneManager As SceneManager, previousScene As GameScene, audioManager As AudioManager)
        MyBase.New(screenWidth, screenHeight, sceneManager, audioManager)

        Me.previousScene = previousScene

        Dim buttonWidth = GetRelativeX(0.25)
        Dim buttonHeight = GetRelativeY(0.1)

        Dim buttonX = GetHorizontalCenter(buttonWidth)
        Dim buttonY = GetVerticalCenter(buttonHeight)
        ' THIS PRACTISE IS DISGUSTING!!!!!! MAKE A UI MANAGER TO HANDLE GAP SPACING LAYOUTS PLEAAAASE
        Dim gapSpacing As Integer = GetRelativeY(0.15)
        buttons.Add(New TextButton(
            buttonX, buttonY,
            buttonWidth, buttonHeight,
            "Resume",
            Color.White,
            Color.DarkGreen,
            Color.Green,
            Sub() manager.ChangeScene(previousScene)
        ))

        buttons.Add(New TextButton(
                    buttonX, buttonY + gapSpacing,
            buttonWidth, buttonHeight,
            "Retry",
            Color.White,
            Color.MediumPurple,
            Color.Purple,
Sub() manager.ChangeScene(New GameScene(screenWidth, screenHeight, sceneManager, audioManager))))
    End Sub
    Public Overrides Sub Update(keys As Dictionary(Of Integer, Boolean))

    End Sub

    Public Overrides Sub Draw(g As Graphics, mouseX As Integer, mouseY As Integer)
        previousScene.SetShouldDrawButtons(False)
        previousScene.Draw(g, mouseX, mouseY)


        Dim overlay As New SolidBrush(Color.FromArgb(150, Color.Black))
        g.FillRectangle(overlay, 0, 0, screenWidth, screenHeight)


        Dim font As New Font("Consolas", 40, FontStyle.Bold)
        Dim text = "PAUSED"
        Dim size = g.MeasureString(text, font)

        g.DrawString(text, font, Brushes.White,
            (screenWidth - size.Width) / 2,
            GetRelativeY(0.2)
        )


        For Each button In buttons
            button.Draw(g, mouseX, mouseY)
        Next

        previousScene.SetShouldDrawButtons(True)
    End Sub

    Public Overrides Sub HandleClick(mouseX As Integer, mouseY As Integer)
        For Each button In buttons
            button.HandleClick(mouseX, mouseY)
        Next
    End Sub
End Class
