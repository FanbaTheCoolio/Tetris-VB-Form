Public Class Board
    Private gridWidth As Integer
    Private gridHeight As Integer
    Private board(,) As TetrominoType


    Public Sub New(gridWidth As Integer, gridHeight As Integer)
        Me.gridWidth = gridWidth
        Me.gridHeight = gridHeight

        ReDim board(gridWidth, gridHeight)

        For y As Integer = 0 To gridHeight
            For x As Integer = 0 To gridWidth
                board(x, y) = TetrominoType.None
            Next
        Next
    End Sub
    Public Function GetCell(x As Integer, y As Integer) As TetrominoType
        Return board(x, y)
    End Function
    Public Function ShouldBeLocked(currentTetromino As Tetromino)
        For Each relativePosition In currentTetromino.GetBlockRelativePositions
            Dim brickXPosition = currentTetromino.GetXPosition + relativePosition.X
            Dim brickYPosition = currentTetromino.GetYPosition + relativePosition.Y
            If brickYPosition < gridHeight Then
                If board(brickXPosition, brickYPosition + 1) <> TetrominoType.None Then
                    Return True
                End If
            End If
            If brickYPosition >= (gridHeight) Then
                Return True
            End If
        Next
        Return False
    End Function
    Public Function IsGameOver(upcomingPiece As TetrominoType, spawnPosition As Point) As Boolean
        Dim upcomingTetromino As New Tetromino(upcomingPiece, spawnPosition)
        For Each relativePosition In upcomingTetromino.GetBlockRelativePositions
            Dim brickXPosition = upcomingTetromino.GetXPosition + relativePosition.X
            Dim brickYPosition = upcomingTetromino.GetYPosition + relativePosition.Y

            Dim yPosCondition As Boolean = (brickYPosition >= 0 And brickYPosition <= gridHeight)

            If yPosCondition Then
                If board(brickXPosition, brickYPosition) <> TetrominoType.None Then
                    Debug.WriteLine(brickXPosition & " " & brickYPosition)
                    Return True
                End If
            End If
        Next

        Return False
    End Function
    Public Sub LockPiece(currentTetromino As Tetromino)
        For Each relativePosition In currentTetromino.GetBlockRelativePositions
            Dim brickXPosition = currentTetromino.GetXPosition + relativePosition.X
            Dim brickYPosition = currentTetromino.GetYPosition + relativePosition.Y

            board(brickXPosition, brickYPosition) = currentTetromino.GetShapeType
        Next
    End Sub
    Public Function ClearLines()
        Dim linesCleared As Integer = 0
        For y As Integer = 0 To gridHeight
            Dim isLineClear As Boolean = True
            For x As Integer = 0 To gridWidth
                If board(x, y) = TetrominoType.None Then
                    isLineClear = False
                    Exit For
                End If
            Next
            If isLineClear Then
                linesCleared += 1
                For lineClearingX As Integer = 0 To gridWidth
                    board(lineClearingX, y) = TetrominoType.None

                Next
                For gravityY As Integer = (y - 1) To 0 Step -1
                    For gravityX As Integer = 0 To gridWidth
                        board(gravityX, gravityY + 1) = board(gravityX, gravityY)
                    Next
                Next
            End If
        Next
        Return linesCleared
    End Function

    Public Function IsValidPosition(newX As Integer, newY As Integer) As Boolean
        Dim xPosCondition As Boolean = (newX >= 0 And newX <= gridWidth)
        Dim yPosCondition As Boolean = (newY >= 0 And newY <= gridHeight)



        If xPosCondition And yPosCondition Then
            Dim isSpotClear As Boolean = (board(newX, newY) = TetrominoType.None)
            If isSpotClear Then
                Return True
            End If
        End If
        Return False
    End Function
End Class
