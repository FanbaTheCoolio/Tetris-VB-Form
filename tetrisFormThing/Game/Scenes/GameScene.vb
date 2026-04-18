Public Class GameScene
    Inherits BaseScene

#Region "Attributes"

#Region "Grid / Board"

    Private Const GRID_WIDTH As Integer = 8
    Private Const GRID_HEIGHT As Integer = 18
    Private board As New Board(GRID_WIDTH, GRID_HEIGHT)


#End Region


#Region "Tetromino System"

    Private currentTetromino As Tetromino
    Private tetrominoBag As New CyclicQueue(Of TetrominoType)(7)
    Private ReadOnly SPAWN_POSITION As New Point(5, 0)
    Private Const TETROMINO_UPDATE_INTERVAL As Integer = 30
    Private tetrominoDelayCounter As Integer = 0

#End Region


#Region "Input Handling"

    Private KeyRepeatInterval As Integer = 10
    Private KeyDelayCounter As Integer = 0

    Private softDropDebounce As Boolean = False
    Private softDropCounter As Integer = 0
    Private Const SOFT_DROP_INTERVAL As Integer = 5

#End Region


#Region "Game State"

    Private gameScore As Integer = 0
    Private heldPiece As TetrominoType = TetrominoType.None
    Private hasHeldThisTurn As Boolean = False

#End Region


#Region "Rendering / Layout"

    Private Const TILE_SIZE As Integer = 40
    Private shouldDrawButtons As Boolean = True

    Private gridStartPositionX As Integer
    Private Const GRID_START_POSITION_Y As Integer = 0

    Private scorePosition As Point

    Private rightOfGridAnchor As New Point
    Private previewAnchorX As Integer
    Private previewAnchorY As Integer

    Private Const PREVIEW_BOX_SIZE As Integer = 4


    Private Const SCORE_SPACING As Integer = TILE_SIZE \ 2
    Private Const PREVIEW_SPACING As Integer = TILE_SIZE \ 2


    Private ReadOnly BORDER_COLOUR As Color = Color.RebeccaPurple



#End Region



#End Region


    Public Sub New(screenWidth As Integer, screenHeight As Integer, sceneManager As SceneManager)
        MyBase.New(screenWidth, screenHeight, sceneManager)

        Randomize()
        RefillBag()
        KeyDelayCounter = 0

        currentTetromino = New Tetromino(tetrominoBag.Dequeue, SPAWN_POSITION)




        gridStartPositionX = (screenWidth - (TILE_SIZE * GRID_WIDTH) - TILE_SIZE) / 2
        previewAnchorX = ((gridStartPositionX + ((GRID_WIDTH)) * TILE_SIZE) + (TILE_SIZE * 2))
        previewAnchorY = TILE_SIZE * 2



        Dim pauseButtonWidth As Integer = GetRelativeX(0.2)
        Dim pauseButtonHeight As Integer = GetRelativeY(0.08)

        Dim pauseButtonCentreX As Integer = GetRelativeX(0.05)
        Dim pauseButtonCentreY As Integer = GetRelativeY(0.05)


        buttons.Add(New ImageButton(
                    pauseButtonCentreX,
                    pauseButtonCentreY,
                    pauseButtonWidth,
                    pauseButtonHeight,
                    My.Resources.Resource1.Pause,
                    Sub() manager.ChangeScene(New PauseScene(screenWidth, screenHeight, manager, Me))
                        ))



        scorePosition.X = pauseButtonCentreX
        scorePosition.Y = pauseButtonCentreY + (pauseButtonHeight + SCORE_SPACING)

    End Sub
    Public Sub ResetKeyDelay()
        KeyDelayCounter = KeyRepeatInterval
    End Sub

    Public Overrides Sub Update(keys As Dictionary(Of Integer, Boolean))
        HandleInput(keys)
        GameUpdate()
    End Sub
    Private Sub GameUpdate()
        tetrominoDelayCounter += 1

        If tetrominoDelayCounter >= TETROMINO_UPDATE_INTERVAL Or softDropDebounce Then
            softDropDebounce = False

            If board.ShouldBeLocked(currentTetromino) Then
                LockPiece()
                Return
            End If

            currentTetromino.Update()
            tetrominoDelayCounter = 0
        End If
    End Sub



#Region "Input & Movement"
    Private Sub HandleInput(keys As Dictionary(Of Integer, Boolean))
        If KeyDelayCounter >= KeyRepeatInterval Then

            If keys.ContainsKey(KeyCode.Q) AndAlso keys(KeyCode.Q) Then
                AttemptLeftRotation()
                KeyDelayCounter = 0
            End If
            If keys.ContainsKey(KeyCode.R) AndAlso keys(KeyCode.R) Then
                AttemptRightRotation()
                KeyDelayCounter = 0
            End If
            If (keys.ContainsKey(KeyCode.A) AndAlso keys(KeyCode.A)) Or (keys.ContainsKey(KeyCode.LeftArrow) AndAlso keys(KeyCode.LeftArrow)) Then
                AttemptLeftMovement()
                KeyDelayCounter = 0
            End If
            If (keys.ContainsKey(KeyCode.D) AndAlso keys(KeyCode.D)) Or (keys.ContainsKey(KeyCode.RightArrow) AndAlso keys(KeyCode.RightArrow)) Then
                AttemptRightMovement()
                KeyDelayCounter = 0
            End If
            If keys.ContainsKey(KeyCode.H) AndAlso keys(KeyCode.H) Then
                Hold()
                KeyDelayCounter = 0
            End If
            If keys.ContainsKey(KeyCode.Spacebar) AndAlso keys(KeyCode.Spacebar) Then
                HardDrop()
                KeyDelayCounter = 0
            End If
        Else
            KeyDelayCounter += 1
        End If

        If softDropCounter >= SOFT_DROP_INTERVAL Then
            If (keys.ContainsKey(KeyCode.S) AndAlso keys(KeyCode.S)) Then
                SoftDrop()
                softDropCounter = 0
            End If
        Else
            softDropCounter += 1
        End If
    End Sub
    Private Sub HardDrop()
        While Not board.ShouldBeLocked(currentTetromino)
            currentTetromino.Update()
        End While
    End Sub
    Private Sub AttemptLeftMovement()
        Dim canMoveLeft As Boolean = True
        For Each relativePosition In currentTetromino.GetBlockRelativePositions
            Dim brickXPosition = currentTetromino.GetXPosition + relativePosition.X
            Dim brickYPosition = currentTetromino.GetYPosition + relativePosition.Y

            If Not board.IsValidPosition(brickXPosition - 1, brickYPosition) Then
                canMoveLeft = False

            End If
        Next
        If canMoveLeft Then
            currentTetromino.MovePiece(DirectionType.Left)
        End If

    End Sub
    Private Sub AttemptRightMovement()
        Dim canMoveRight As Boolean = True
        For Each relativePosition In currentTetromino.GetBlockRelativePositions
            Dim brickXPosition = currentTetromino.GetXPosition + relativePosition.X
            Dim brickYPosition = currentTetromino.GetYPosition + relativePosition.Y

            If Not board.IsValidPosition(brickXPosition + 1, brickYPosition) Then
                canMoveRight = False

            End If
        Next
        If canMoveRight Then
            currentTetromino.MovePiece(DirectionType.Right)
        End If

    End Sub
    Private Sub SoftDrop()
        softDropDebounce = True
    End Sub
    Private Sub AttemptRightRotation()
        If currentTetromino.GetShapeType = TetrominoType.O_Piece Then Return

        Dim canRotateRight As Boolean = True

        For Each relativePosition In currentTetromino.GetPotentialRotation(DirectionType.Right)
            Dim brickXPosition = currentTetromino.GetXPosition + relativePosition.X
            Dim brickYPosition = currentTetromino.GetYPosition + relativePosition.Y

            If Not board.IsValidPosition(brickXPosition, brickYPosition) Then
                canRotateRight = False
            End If
        Next
        If canRotateRight Then
            currentTetromino.Rotate(DirectionType.Right)
        End If
    End Sub

    Private Sub AttemptLeftRotation()
        If currentTetromino.GetShapeType = TetrominoType.O_Piece Then Return

        Dim canRotateRight As Boolean = True

        For Each relativePosition In currentTetromino.GetPotentialRotation(DirectionType.Left)
            Dim brickXPosition = currentTetromino.GetXPosition + relativePosition.X
            Dim brickYPosition = currentTetromino.GetYPosition + relativePosition.Y

            If Not board.IsValidPosition(brickXPosition, brickYPosition) Then
                canRotateRight = False

            End If
        Next
        If canRotateRight Then
            currentTetromino.Rotate(DirectionType.Left)
        End If
    End Sub
    Public Overrides Sub HandleClick(mouseX As Integer, mouseY As Integer)
        For Each button In buttons
            button.HandleClick(mouseX, mouseY)
        Next
    End Sub


#End Region


#Region "Core Mechanics"
    Private Sub LockPiece()
        board.LockPiece(currentTetromino)
        Dim currentScore = board.ClearLines()
        addScore(currentScore)


        Dim upcomingPiece As TetrominoType = tetrominoBag.Peek
        If board.IsGameOver(upcomingPiece, SPAWN_POSITION) Then
            manager.ChangeScene(New GameOverScene(screenWidth, screenHeight, manager, gameScore))
        Else
            SpawnNextTetrominoPiece()
        End If



        hasHeldThisTurn = False
    End Sub

    Private Sub SpawnNextTetrominoPiece()
        currentTetromino = New Tetromino(tetrominoBag.Dequeue, SPAWN_POSITION)
        If tetrominoBag.IsEmpty Then
            RefillBag()
        End If
    End Sub
    Private Sub Hold()

        If hasHeldThisTurn Then Return


        If heldPiece = TetrominoType.None Then
            heldPiece = currentTetromino.GetShapeType

            SpawnNextTetrominoPiece()
        Else
            Dim temp As TetrominoType = currentTetromino.GetShapeType

            currentTetromino = New Tetromino(heldPiece, SPAWN_POSITION)
            heldPiece = temp
        End If

        hasHeldThisTurn = True
    End Sub
    Private Sub RefillBag()
        For Each piece As TetrominoType In [Enum].GetValues(GetType(TetrominoType))
            If piece <> TetrominoType.None Then
                tetrominoBag.Enqueue(piece)
            End If
        Next
        tetrominoBag.Randomise()
    End Sub

    Private Sub addScore(linesCleared As Integer)
        Select Case linesCleared
            Case 1 : gameScore += 100
            Case 2 : gameScore += 300
            Case 3 : gameScore += 500
            Case 4 : gameScore += 800
        End Select
    End Sub

#End Region


#Region "Rendering"

    Public Overrides Sub Draw(g As Graphics, mouseX As Integer, mouseY As Integer)
        g.Clear(Color.Black)
        DrawBorders(g)
        DrawLockedPieces(g)
        DrawCurrentPiece(g)
        DrawNextPiece(g)
        DrawScore(g)
        DrawHeldPiece(g)
        If shouldDrawButtons Then
            For Each button In buttons
                button.Draw(g, mouseX, mouseY)
            Next
        End If

    End Sub

    Public Sub SetShouldDrawButtons(shouldDrawButtons As Boolean)
        Me.shouldDrawButtons = shouldDrawButtons
    End Sub

    Private Sub DrawHeldPiece(g As Graphics)

    End Sub
    Private Sub DrawNextPiece(g As Graphics)

        Dim nextType = tetrominoBag.Peek()
        Dim previewTetromino = New Tetromino(nextType, SPAWN_POSITION)


        Dim minimumRelativeX As Integer = getMinimumRelativePositionX(previewTetromino)
        Dim pieceWidth = getMaximumRelativePositionX(previewTetromino) - minimumRelativeX + 1


        Dim offsetX = (PREVIEW_BOX_SIZE - pieceWidth) \ 2 - minimumRelativeX



        For Each block In previewTetromino.GetBlockRelativePositions
            Dim x = previewAnchorX + ((block.X + offsetX) * TILE_SIZE)
            Dim y = previewAnchorY + (block.Y * TILE_SIZE)

            g.FillRectangle(New SolidBrush(previewTetromino.GetTileColour), x, y, TILE_SIZE, TILE_SIZE)
            g.DrawRectangle(Pens.Black, x, y, TILE_SIZE, TILE_SIZE)
        Next


        Dim previewFont As Font = New Font("Consolas", 20, FontStyle.Bold)
        Dim labelString As String = "Preview"

        Dim previewLabelYPosition As Integer = previewAnchorY - (PREVIEW_SPACING * 2)
        g.DrawString(labelString, previewFont, New SolidBrush(Color.Black), previewAnchorX, previewLabelYPosition)
    End Sub

    Private Sub DrawScore(g As Graphics)
        Dim scoreString As String = "Score : " & gameScore
        Dim scoreFont As Font = New Font("Consolas", 20, FontStyle.Bold)

        g.DrawString(scoreString, scoreFont, New SolidBrush(Color.Black), scorePosition.X, scorePosition.Y)
    End Sub
    Private Sub DrawBorders(g As Graphics)
        g.FillRectangle(New SolidBrush(BORDER_COLOUR), 0, 0, gridStartPositionX, screenHeight)
        g.FillRectangle(New SolidBrush(BORDER_COLOUR), gridStartPositionX + (TILE_SIZE * GRID_WIDTH) + TILE_SIZE, 0, screenWidth, screenHeight)
    End Sub
    Private Sub DrawCurrentPiece(g As Graphics)

        For Each relativePosition In currentTetromino.GetBlockRelativePositions
            Dim currentTileXPosition = gridStartPositionX + ((currentTetromino.GetXPosition + relativePosition.X) * TILE_SIZE)
            Dim currentTileYPosition = GRID_START_POSITION_Y + ((currentTetromino.GetYPosition + relativePosition.Y) * TILE_SIZE)
            g.FillRectangle(New SolidBrush(currentTetromino.GetTileColour), currentTileXPosition, currentTileYPosition, TILE_SIZE, TILE_SIZE)
            g.DrawRectangle(New Pen(Color.Black), currentTileXPosition, currentTileYPosition, TILE_SIZE, TILE_SIZE)
        Next


    End Sub
    Private Sub DrawLockedPieces(g As Graphics)
        For columns = 0 To GRID_HEIGHT
            For rows = 0 To GRID_WIDTH
                Dim currentTileXPosition = gridStartPositionX + (rows * TILE_SIZE)
                Dim currentTileYPosition = GRID_START_POSITION_Y + (columns * TILE_SIZE)
                g.DrawRectangle(Pens.Gray, currentTileXPosition, currentTileYPosition, TILE_SIZE, TILE_SIZE)
                If board.GetCell(rows, columns) <> TetrominoType.None Then
                    g.FillRectangle(New SolidBrush(SceneManager.GetTetrominoColour(board.GetCell(rows, columns))), currentTileXPosition, currentTileYPosition, TILE_SIZE, TILE_SIZE)
                    g.DrawRectangle(New Pen(Color.Black), currentTileXPosition, currentTileYPosition, TILE_SIZE, TILE_SIZE)
                End If
            Next
        Next
    End Sub
#End Region



End Class

