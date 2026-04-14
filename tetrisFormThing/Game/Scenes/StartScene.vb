Public Class StartScene
    Inherits BaseScene
    Public Sub New(screenWidth As Integer, screenHeight As Integer, sceneManager As SceneManager)
        MyBase.New(screenWidth, screenHeight, sceneManager)

        Dim startButtonWidth As Integer = GetRelativeX(0.25)
        Dim startButtonHeight As Integer = GetRelativeY(0.1)

        Dim startButtonXPosition As Integer = GetHorizontalCenter(startButtonWidth)
        Dim startButtonYPosition As Integer = GetVerticalCenter(startButtonHeight)


        buttons.Add(New TextButton(startButtonXPosition, startButtonYPosition, startButtonWidth, startButtonHeight, "Start", Color.Black, Color.Blue, Color.DarkBlue, Sub() sceneManager.ChangeScene(New GameScene(screenWidth, screenHeight, sceneManager))))
    End Sub
    Public Overrides Sub Update(keys As Dictionary(Of Integer, Boolean))

    End Sub

    Public Overrides Sub Draw(g As Graphics, mouseX As Integer, mouseY As Integer)
        g.Clear(Color.Black)

        For Each button In buttons
            button.Draw(g, mouseX, mouseY)
        Next
    End Sub

    Public Overrides Sub HandleClick(mouseX As Integer, mouseY As Integer)
        For Each button In buttons
            button.HandleClick(mouseX, mouseY)
        Next
    End Sub
End Class
