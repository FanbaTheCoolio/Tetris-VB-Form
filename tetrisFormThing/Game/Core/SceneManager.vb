
Public Class SceneManager
    Private currentScene As Scene
    Private screenWidth, screenHeight As Integer
    Public Sub New(screenWidth As Integer, screenHeight As Integer)
        Me.screenHeight = screenHeight
        Me.screenWidth = screenWidth

        currentScene = New StartScene(screenWidth, screenHeight, Me)
    End Sub

    Public Sub ChangeScene(newScene As Scene)
        currentScene = newScene
    End Sub

    Public Sub Update(keys As Dictionary(Of Integer, Boolean))
        currentScene.Update(keys)
    End Sub
    Sub Draw(g As Graphics, mouseX As Integer, mouseY As Integer)
        currentScene.Draw(g, mouseX, mouseY)
    End Sub

    Public Sub HandleClick(mouseX As Integer, mouseY As Integer)
        currentScene.HandleClick(mouseX, mouseY)
    End Sub
    Public Sub ResetKeyDelay()
        If TypeOf currentScene Is GameScene Then
            CType(currentScene, GameScene).ResetKeyDelay()
        End If
    End Sub
    Public Shared Function GetTetrominoColour(value As TetrominoType) As Color
        Select Case value
            Case TetrominoType.I_Piece
                Return Color.Cyan
            Case TetrominoType.O_Piece
                Return Color.Yellow
            Case TetrominoType.T_Piece
                Return Color.Purple
            Case TetrominoType.L_Piece
                Return Color.Orange
            Case TetrominoType.J_Piece
                Return Color.Blue
            Case TetrominoType.S_Piece
                Return Color.Green
            Case TetrominoType.Z_Piece
                Return Color.Red
            Case TetrominoType.None
                Return Color.Black
            Case Else
                Return Color.Black
        End Select
    End Function
End Class

