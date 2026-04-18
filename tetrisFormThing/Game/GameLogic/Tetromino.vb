Public Class Tetromino
    Private position As Point
    Private shapeType As TetrominoType
    Private blockRelativePositions() As Block
    Private centreOfRotation As Block

    ' Private shape  i'll do this later since its gonna be awkward.
    Sub New(shapeType As TetrominoType, position As Point)

        Me.position.X = position.X
        Me.position.Y = position.Y
        Me.shapeType = shapeType

        SelectPieceType()
    End Sub
    Public Function GetPotentialRotation(direction As DirectionType)
        Dim rotationRelativePosition(blockRelativePositions.Length) As Block
        Dim rightMultiplier As Integer = 1
        Dim leftMultiplier As Integer = 1

        If direction = DirectionType.Right Then
            rightMultiplier *= -1
        Else
            leftMultiplier *= -1
        End If

        For i As Integer = 0 To blockRelativePositions.Length - 1
            Dim currentblock = blockRelativePositions(i)
            Dim relativeX = currentblock.X - centreOfRotation.X
            Dim relativeY = currentblock.Y - centreOfRotation.Y

            Dim newX = relativeY * rightMultiplier
            Dim newY = relativeX * leftMultiplier

            rotationRelativePosition(i).X = newX + centreOfRotation.X
            rotationRelativePosition(i).Y = newY + centreOfRotation.Y
        Next

        Return rotationRelativePosition
    End Function
    Public Sub Rotate(direction As DirectionType)
        Dim rightMultiplier As Integer = 1
        Dim leftMultiplier As Integer = 1
        If direction = DirectionType.Right Then
            rightMultiplier *= -1
        Else
            leftMultiplier *= -1
        End If
        For i As Integer = 0 To blockRelativePositions.Length - 1
            Dim currentblock = blockRelativePositions(i)
            Dim relativeX = currentblock.X - centreOfRotation.X
            Dim relativeY = currentblock.Y - centreOfRotation.Y

            Dim newX = relativeY * rightMultiplier
            Dim newY = relativeX * leftMultiplier

            blockRelativePositions(i).X = newX + centreOfRotation.X
            blockRelativePositions(i).Y = newY + centreOfRotation.Y
        Next
    End Sub
    Public Function GetBlockRelativePositions()
        Return blockRelativePositions
    End Function
    Private Function GetCentreOfRotation()
        Return centreOfRotation
    End Function
    Private Sub SelectPieceType()

        Select Case shapeType
            Case TetrominoType.I_Piece
                blockRelativePositions = {New Block(-1, 0), New Block(0, 0), New Block(1, 0), New Block(2, 0)}
                centreOfRotation = New Block(0, 0)
            Case TetrominoType.O_Piece
                blockRelativePositions = {New Block(0, 0), New Block(1, 0), New Block(0, 1), New Block(1, 1)}
                centreOfRotation = New Block(0, 0)
            Case TetrominoType.T_Piece
                blockRelativePositions = {New Block(-1, 0), New Block(0, 0), New Block(1, 0), New Block(0, 1)}
                centreOfRotation = New Block(0, 0)
            Case TetrominoType.S_Piece
                blockRelativePositions = {New Block(0, 0), New Block(1, 0), New Block(-1, 1), New Block(0, 1)}
                centreOfRotation = New Block(0, 1)
            Case TetrominoType.Z_Piece
                blockRelativePositions = {New Block(-1, 0), New Block(0, 0), New Block(0, 1), New Block(1, 1)}
                centreOfRotation = New Block(0, 1)
            Case TetrominoType.J_Piece
                blockRelativePositions = {New Block(-1, 0), New Block(0, 0), New Block(1, 0), New Block(-1, 1)}
                centreOfRotation = New Block(0, 0)
            Case TetrominoType.L_Piece
                blockRelativePositions = {New Block(-1, 0), New Block(0, 0), New Block(1, 0), New Block(1, 1)}
                centreOfRotation = New Block(0, 0)
        End Select
    End Sub
    Public Function GetTileColour() As Color
        Return SceneManager.GetTetrominoColour(shapeType)
    End Function

    Public Sub MovePiece(direction As DirectionType)
        If direction = DirectionType.Left Then
            MoveLeft()
        ElseIf direction = DirectionType.Right Then
            MoveRight()
        End If
    End Sub

    Private Sub MoveLeft()
        position.X -= 1
    End Sub
    Private Sub MoveRight()
        position.X += 1
    End Sub
    Public Function GetShapeType() As TetrominoType
        Return shapeType
    End Function

    Public Function GetXPosition() As Integer
        Return position.X
    End Function
    Public Function GetYPosition() As Integer
        Return position.Y
    End Function

    Public Sub Update()
        position.Y += 1
    End Sub
End Class
