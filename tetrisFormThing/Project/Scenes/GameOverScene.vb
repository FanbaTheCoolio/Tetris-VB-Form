Public Class GameOverScene
    Inherits Scene

    Private finalScore As Integer


    Public Sub New(screenWidth As Integer, screenHeight As Integer, manager As SceneManager, score As Integer, audioManager As AudioManager)
        MyBase.New(screenWidth, screenHeight, manager, audioManager)

        Me.finalScore = score

        Dim btnWidth = GetRelativeX(0.25)
        Dim btnHeight = GetRelativeY(0.1)

        Dim centerX = GetHorizontalCenter(btnWidth)


        buttons.Add(New TextButton(
            centerX,
            GetRelativeY(0.5),
            btnWidth,
            btnHeight,
            "Restart",
            Color.White,
            Color.DarkBlue,
            Color.Blue,
            Sub() manager.ChangeScene(New GameScene(screenWidth, screenHeight, manager, audioManager)),
            audioManager
        ))

        buttons.Add(New TextButton(
            centerX,
            GetRelativeY(0.65),
            btnWidth,
            btnHeight,
            "Menu",
            Color.White,
            Color.DarkRed,
            Color.Red,
            Sub() manager.ChangeScene(New StartScene(screenWidth, screenHeight, manager, audioManager)),
            audioManager
        ))
    End Sub
    Public Overrides Sub Update(keys As Dictionary(Of Integer, Boolean))


    End Sub

    Public Overrides Sub Draw(g As Graphics, mouseX As Integer, mouseY As Integer)
        g.Clear(Color.Black)

        Dim titleFont As New Font("Consolas", 40, FontStyle.Bold)
        Dim title = "GAME OVER"
        Dim titleSize = g.MeasureString(title, titleFont)

        g.DrawString(title, titleFont, Brushes.White,
        (screenWidth - titleSize.Width) / 2,
        GetRelativeY(0.2)
    )


        Dim scoreFont As New Font("Consolas", 20, FontStyle.Bold)
        Dim scoreText = "Score: " & finalScore

        Dim scoreSize = g.MeasureString(scoreText, scoreFont)

        g.DrawString(scoreText, scoreFont, Brushes.White,
        (screenWidth - scoreSize.Width) / 2,
        GetRelativeY(0.35)
    )

        For Each b In buttons
            b.Draw(g, mouseX, mouseY)
        Next

    End Sub

    Public Overrides Sub HandleClick(mouseX As Integer, mouseY As Integer)
        For Each b In buttons
            b.HandleClick(mouseX, mouseY)
        Next
    End Sub
End Class
