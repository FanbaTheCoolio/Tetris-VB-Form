' A Framework for creating 2D graphical applications
' such as games in VB.Net, by Neil Kendall
' Based on https://tinyurl.com/5b25pvv9
' Create a new VB.Net Forms application. Paste all of this code into Form1's code view.


Imports System.DirectoryServices.ActiveDirectory
Imports System.Drawing.Drawing2D
Imports System.Drawing.Text
Imports System.Numerics
Imports System.Reflection.Emit
Imports System.Runtime.CompilerServices
Imports System.Windows.Forms.AxHost
Imports System.Windows.Forms.Design


Public Class Form1
    Public ApplicationName As String = "2D Gfx Framework"
    Public Shared DisplaySize As Size = New Size(800, 800)
    Public FrameInterval As Integer = 16 ' Time in milliseconds between each frame render. 16 milliseconds is approximately 60 FPS (Frames Per Second)
    Public KeyRepeatInterval As Integer = 10 ' If a key is held down, this is the number of frames between each repeat. Set as 0 for no delay


#Region "Framework Variables"
    ' Framework Variables
    Public KeyPressed As Dictionary(Of Integer, Boolean) = New Dictionary(Of Integer, Boolean)
    Public KeyDelayCounter As Integer
    Public MouseLeftClicked As Boolean = False ' Has the left mouse button been clicked?
    Public MouseRightClicked As Boolean = False ' Has the right mouse button been clicked?
    Public MouseLeftDown As Boolean = False ' Is the left mouse button currently down?
    Public MouseRightDown As Boolean = False ' Is the right mouse button currently down?
    Public MouseX, MouseY As Integer ' These hold the current x and y positions of the mousepointer relative to the upper left corner of the form
    Public Shared rand As Random = New Random

#End Region


#Region "Application Variables"
    ' *********************************************
    ' ** Define your application variables below **
    ' *********************************************


    Dim currentSceneManager As SceneManager
    Enum PlayerAction
        LeftRotation
        RightRotation
        LeftMovement
        RightMovement
        SoftDrop
    End Enum
    Enum TetrominoType
        None
        I_Piece
        O_Piece
        T_Piece
        L_Piece
        J_Piece
        S_Piece
        Z_Piece
    End Enum
    Enum GameState
        StartMenu
        Ingame
        Pause
        GameOver
    End Enum
    Enum DirectionType
        Left
        Right
    End Enum
    '1   Cyan (I piece)
    '2   Yellow (O piece)
    '3   Purple (T piece)
    '4   Orange (L piece)
    '5   Blue (J piece)
    '6   Green (S piece)
    '7   Red (Z piece)

    ' *********************************************
    ' ** Define your application variables above **
    ' *********************************************
#End Region


#Region "Game Load"
    Private Sub GameLoad()
        KeyDelayCounter = 0
        ' **********************************************************************
        ' ** The code below will run once when the application loads / starts **
        ' **********************************************************************

        currentSceneManager = New SceneManager(ClientSize.Width, ClientSize.Height)

        ' **********************************************************************
        ' ** The code above will run once when the application loads / starts **
        ' **********************************************************************
    End Sub
#End Region


#Region "Game State Update"
    ' All game Updates should be put here e.g. reading keyboard/ mouse, updating sprite positions (but NOT drawing them), calculations etc.
    Private Sub GameUpdate()


        currentSceneManager.Update()


#Region "Check Keys"
        ' Up Arrow key = 38. Use 40 for down, 37 for left And 39 for right. 65-90 for A-Z. 48-57 for 0-9
        ' All key codes here https://learn.microsoft.com/en-us/dotnet/api/system.windows.forms.keys


        ' Up

        If currentSceneManager.IsSoftDropReady Then
            If KeyPressed.ContainsKey(32) Then
                If KeyPressed(32) Then
                    currentSceneManager.HandleAction(PlayerAction.SoftDrop)
                End If
            End If
        Else
            currentSceneManager.IncrementSoftDropCounter()
        End If

        If KeyDelayCounter = KeyRepeatInterval Then

            If KeyPressed.ContainsKey(81) Then
                If KeyPressed(81) Then
                    currentSceneManager.HandleAction(PlayerAction.LeftRotation)
                    KeyDelayCounter = 0
                End If
            End If

            If KeyPressed.ContainsKey(82) Then
                If KeyPressed(82) Then
                    currentSceneManager.HandleAction(PlayerAction.RightRotation)
                    KeyDelayCounter = 0
                End If
            End If
            If KeyPressed.ContainsKey(37) Then
                If KeyPressed(37) Then
                    ' *********************************************************
                    ' ** The code below runs when the LEFT (37) key is pressed **
                    ' *********************************************************

                    currentSceneManager.HandleAction(PlayerAction.LeftMovement)

                    ' *********************************************************
                    ' ** The code above runs when the LEFT (37) key is pressed **
                    ' *********************************************************
                    KeyDelayCounter = 0
                End If
            End If

            If KeyPressed.ContainsKey(65) Then
                If KeyPressed(65) Then
                    ' *********************************************************
                    ' ** The code below runs when the A (65) key is pressed **
                    ' *********************************************************

                    currentSceneManager.HandleAction(PlayerAction.LeftMovement)

                    ' *********************************************************
                    ' ** The code above runs when the A (65) key is pressed **
                    ' *********************************************************
                    KeyDelayCounter = 0
                End If
            End If
            If KeyPressed.ContainsKey(39) Then
                If KeyPressed(39) Then
                    ' *********************************************************
                    ' ** The code below runs when the RIGHT (39) key is pressed **
                    ' *********************************************************
                    currentSceneManager.HandleAction(PlayerAction.RightMovement)
                    ' *********************************************************
                    ' ** The code above runs when the RIGHT (39) key is pressed **
                    ' *********************************************************
                    KeyDelayCounter = 0
                End If
            End If


            If KeyPressed.ContainsKey(68) Then
                If KeyPressed(68) Then
                    ' *********************************************************
                    ' ** The code below runs when the D (68) key is pressed **
                    ' *********************************************************
                    currentSceneManager.HandleAction(PlayerAction.RightMovement)
                    ' *********************************************************
                    ' ** The code above runs when the D (68) key is pressed **
                    ' *********************************************************
                    KeyDelayCounter = 0
                End If
            End If

        Else

            KeyDelayCounter = KeyDelayCounter + 1
        End If
#End Region


#Region "Check Mouse Clicks"
        If MouseLeftClicked = True Then
            ' *****************************************************************
            ' ** The code below is run when the left mouse button is CLICKED **
            ' *****************************************************************


            currentSceneManager.HandleClicks(MouseX, MouseY)
            ' *****************************************************************
            ' ** The code above is run when the left mouse button is CLICKED **
            ' *****************************************************************
            MouseLeftClicked = False
        End If


        If MouseRightClicked = True Then
            ' *****************************************************************
            ' ** The code below is run when the right mouse button is CLICKED **
            ' *****************************************************************




            ' *****************************************************************
            ' ** The code above is run when the right mouse button is CLICKED **
            ' *****************************************************************


            MouseRightClicked = False
        End If
#End Region


    End Sub



#End Region


#Region "Game Draw"
    ' This sub draws the current frame. It is called automatically based on the setting FrameInterval which is measured in milliseconds
    ' A value of 16 equates to about 60 frames per second (FPS) [1000 / 60 = 16.67]
    Private Sub GameDraw(ByVal g As Graphics)


        ' ********************************************
        ' ** Put your code to draw each frame below **
        ' ********************************************

        currentSceneManager.Draw(g, MouseX, MouseY)

        ' ********************************************
        ' ** Put your code to draw each frame above **
        ' ********************************************
    End Sub
#End Region


#Region "User Defined Methods"
    ' ****************************************************************
    ' ** Put the subs and functions for your application below here **
    ' ****************************************************************


    ' ****************************************************************
    ' ** Put the subs and functions for your application above here **
    ' ****************************************************************
#End Region


#Region "User Defined Classes"
    Class SceneManager




        Private board(gridWidth, gridHeight) As TetrominoType

        Private gameScore As Integer = 0
        Private softDropDebounce = False
        Private currentTetromino As Tetromino
        Private tetrominoBag As New Queue(Of TetrominoType)
        Private tetrominoDelayCounter As Integer = 0
        Private currentGameState As GameState
        Private screenWidth, screenHeight As Integer
        Private borderColor As Color = Color.RebeccaPurple
        Private softDropCounter As Integer = 0
        Private startStateButtons As New List(Of Button)

        Private gridOffsetStartX As Integer '= ClientSize.Width * 0.85
        Private Const gridOffsetStartY As Integer = 0
        Private Const scoreOffsetX As Integer = 25
        Private Const scoreOffsetY As Integer = 100
        Private Const tileSize As Integer = 40
        Private Const gridHeight As Integer = 18
        Private Const gridWidth As Integer = 8
        Private Const tetrominoCounterIncrement As Integer = 1
        Private Const tetrominoUpdateInterval As Integer = 30
        Private Const softDropInterval As Integer = 5

        Public Sub New(screenWidth As Integer, screenHeight As Integer)
            Me.screenWidth = screenWidth
            Me.screenHeight = screenHeight
            gridOffsetStartX = (screenWidth - (tileSize * gridWidth) - tileSize) / 2

            startStateButtons.Add(New Button(300, 300, 200, 60, "Start", Color.Black, Color.Blue, Color.DarkBlue, Sub() ChangeGameState(GameState.Ingame)))

        End Sub

        Public Function IsSoftDropReady()
            Return softDropCounter >= softDropInterval

        End Function
        Public Sub IncrementSoftDropCounter()
            softDropCounter += 1
        End Sub
        Private Sub SoftDrop()
            softDropDebounce = True
            softDropCounter = 0
        End Sub

        Private Sub InitialiseGame()
            Randomize()

            RefillBag()
            currentTetromino = New Tetromino(tetrominoBag.Dequeue)



            For y As Integer = 0 To gridHeight
                For x As Integer = 0 To gridWidth
                    board(x, y) = TetrominoType.None
                Next
            Next
        End Sub

        Private Sub ChangeGameState(newGameState As GameState)
            Select Case newGameState
                Case GameState.StartMenu
                    currentGameState = newGameState
                Case GameState.Ingame
                    InitialiseGame()
                    currentGameState = newGameState
                Case GameState.GameOver
                    currentGameState = newGameState
                Case GameState.Pause
                    currentGameState = newGameState
            End Select
        End Sub

        Public Sub HandleClicks(mouseX As Integer, mouseY As Integer)
            Select Case currentGameState
                Case GameState.StartMenu
                    For Each button In startStateButtons
                        button.HandleClick(mouseX, mouseY)
                    Next
            End Select
        End Sub


        Public Sub Draw(g As Graphics, mouseX As Integer, mouseY As Integer)
            Select Case currentGameState
                Case GameState.Ingame
                    DrawIngameState(g)
                Case GameState.StartMenu
                    DrawStartState(g, mouseX, mouseY)
            End Select
        End Sub

        Private Sub DrawStartState(g As Graphics, mouseX As Integer, mouseY As Integer)
            g.Clear(Color.Black)

            For Each buttons In startStateButtons
                buttons.Draw(g, mouseX, mouseY)
            Next
        End Sub

        Public Sub Update()
            Select Case currentGameState
                Case GameState.Ingame
                    UpdateIngameState()
                Case GameState.StartMenu

            End Select
        End Sub
        Private Sub DrawIngameState(g As Graphics)
            g.Clear(Color.Black)
            DrawBorders(g)
            DrawLockedPieces(g)
            DrawCurrentPiece(g)
            DrawScore(g)
        End Sub

        Private Sub UpdateIngameState()
            tetrominoDelayCounter += 1

            If tetrominoDelayCounter >= tetrominoUpdateInterval Or softDropDebounce Then
                softDropDebounce = False

                If ShouldBeLocked() Then
                    LockPiece()
                End If

                currentTetromino.Update()

                'Debug.WriteLine("X : " & currentTetromino.GetXPosition)
                'Debug.WriteLine("Y : " & currentTetromino.GetYPosition)
                'Debug.WriteLine("Previous X : " & currentTetromino.GetPreviousXPosition)
                'Debug.WriteLine("Previous Y : " & currentTetromino.GetPreviousYPosition)
                'board(currentTetromino.GetXPosition, currentTetromino.GetYPosition) = currentTetromino.GetShapeType
                'board(currentTetromino.GetPreviousXPosition, currentTetromino.GetPreviousYPosition) = TetrominoType.None
                tetrominoDelayCounter = 0
            End If
        End Sub



        Private Sub DrawScore(g As Graphics)
            Dim scoreString As String = "Score : " & gameScore
            Dim scoreFont As Font = New Font("Consolas", 20, FontStyle.Bold)
            g.DrawString(scoreString, scoreFont, New SolidBrush(Color.Black), scoreOffsetX, scoreOffsetY)
        End Sub
        Private Sub DrawBorders(g As Graphics)
            g.FillRectangle(New SolidBrush(borderColor), 0, 0, gridOffsetStartX, screenHeight)
            g.FillRectangle(New SolidBrush(borderColor), gridOffsetStartX + (tileSize * gridWidth) + tileSize, 0, screenWidth, screenHeight)
        End Sub
        Private Sub DrawCurrentPiece(g As Graphics)

            For Each relativePosition In currentTetromino.getBlockRelativePositions
                Dim currentTileXPosition = gridOffsetStartX + ((currentTetromino.GetXPosition + relativePosition.X) * tileSize)
                Dim currentTileYPosition = gridOffsetStartY + ((currentTetromino.GetYPosition + relativePosition.Y) * tileSize)
                g.FillRectangle(New SolidBrush(currentTetromino.GetTileColour), currentTileXPosition, currentTileYPosition, tileSize, tileSize)
                g.DrawRectangle(New Pen(Color.Black), currentTileXPosition, currentTileYPosition, tileSize, tileSize)
            Next


        End Sub
        Private Sub DrawLockedPieces(g As Graphics)
            For columns = 0 To gridHeight
                For rows = 0 To gridWidth
                    Dim currentTileXPosition = gridOffsetStartX + (rows * tileSize)
                    Dim currentTileYPosition = gridOffsetStartY + (columns * tileSize)
                    g.DrawRectangle(Pens.Gray, currentTileXPosition, currentTileYPosition, tileSize, tileSize)
                    If board(rows, columns) <> TetrominoType.None Then
                        g.FillRectangle(New SolidBrush(GetTetrominoColour(board(rows, columns))), currentTileXPosition, currentTileYPosition, tileSize, tileSize)
                        g.DrawRectangle(New Pen(Color.Black), currentTileXPosition, currentTileYPosition, tileSize, tileSize)
                    End If
                Next
            Next
        End Sub
        Private Function ShouldBeLocked()


            For Each relativePosition In currentTetromino.getBlockRelativePositions
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

        Private Sub LockPiece()
            For Each relativePosition In currentTetromino.getBlockRelativePositions
                Dim brickXPosition = currentTetromino.GetXPosition + relativePosition.X
                Dim brickYPosition = currentTetromino.GetYPosition + relativePosition.Y

                board(brickXPosition, brickYPosition) = currentTetromino.GetShapeType
            Next
            ClearLines()

            If tetrominoBag.IsEmpty Then
                RefillBag()
            End If
            currentTetromino = New Tetromino(tetrominoBag.Dequeue)


        End Sub
        Private Sub ClearLines()
            'Dim positionOfLinesToBeCleared As New List(Of Integer)


            For y As Integer = 0 To gridHeight
                Dim isLineClear As Boolean = True
                For x As Integer = 0 To gridWidth
                    If board(x, y) = TetrominoType.None Then
                        isLineClear = False
                        Exit For
                    End If
                Next
                If isLineClear Then
                    Debug.WriteLine(y)
                    'positionOfLinesToBeCleared.Add(y)
                    gameScore += 50
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
        End Sub
        Private Sub RefillBag()
            For Each piece As TetrominoType In [Enum].GetValues(GetType(TetrominoType))
                If piece <> TetrominoType.None Then
                    tetrominoBag.Enqueue(piece)
                End If
            Next
            tetrominoBag.Randomise()
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

        Public Sub HandleAction(action As PlayerAction)


            If currentGameState <> GameState.Ingame Then Return

            Select Case action
                Case PlayerAction.LeftMovement
                    AttemptLeftMovement()
                Case PlayerAction.RightMovement
                    AttemptRightMovement()
                Case PlayerAction.LeftRotation
                    AttemptLeftRotation()
                Case PlayerAction.RightRotation
                    AttemptRightRotation()
                Case PlayerAction.SoftDrop
                    SoftDrop()
                Case Else
                    Throw New ArgumentOutOfRangeException(NameOf(action))
            End Select
        End Sub
        Private Sub AttemptLeftMovement()
            Dim canMoveLeft As Boolean = True
            For Each relativePosition In currentTetromino.getBlockRelativePositions
                Dim brickXPosition = currentTetromino.GetXPosition + relativePosition.X
                Dim brickYPosition = currentTetromino.GetYPosition + relativePosition.Y

                If Not TryTransformation(brickXPosition - 1, brickYPosition) Then
                    canMoveLeft = False

                End If
            Next
            If canMoveLeft Then
                currentTetromino.MovePiece(DirectionType.Left)
            End If

        End Sub
        Private Sub AttemptRightMovement()
            Dim canMoveRight As Boolean = True
            For Each relativePosition In currentTetromino.getBlockRelativePositions
                Dim brickXPosition = currentTetromino.GetXPosition + relativePosition.X
                Dim brickYPosition = currentTetromino.GetYPosition + relativePosition.Y

                If Not TryTransformation(brickXPosition + 1, brickYPosition) Then
                    canMoveRight = False

                End If
            Next
            If canMoveRight Then
                currentTetromino.MovePiece(DirectionType.Right)
            End If

        End Sub

        Private Sub AttemptRightRotation()
            If currentTetromino.GetShapeType = TetrominoType.O_Piece Then Return

            Dim canRotateRight As Boolean = True

            For Each relativePosition In currentTetromino.getPotentialRotation(DirectionType.Right)
                Dim brickXPosition = currentTetromino.GetXPosition + relativePosition.X
                Dim brickYPosition = currentTetromino.GetYPosition + relativePosition.Y

                If Not TryTransformation(brickXPosition, brickYPosition) Then
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

            For Each relativePosition In currentTetromino.getPotentialRotation(DirectionType.Left)
                Dim brickXPosition = currentTetromino.GetXPosition + relativePosition.X
                Dim brickYPosition = currentTetromino.GetYPosition + relativePosition.Y

                If Not TryTransformation(brickXPosition, brickYPosition) Then
                    canRotateRight = False

                End If
            Next
            If canRotateRight Then
                currentTetromino.Rotate(DirectionType.Left)
            End If
        End Sub
        Private Function TryTransformation(newX As Integer, newY As Integer) As Boolean
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
    Public Structure Block
        Public X As Integer
        Public Y As Integer

        Public Sub New(X As Integer, Y As Integer)
            Me.X = X
            Me.Y = Y
        End Sub
    End Structure
    Class Tetromino
        Private xPosition, yPosition As Integer
        Private shapeType As TetrominoType
        Private previousXPosition, previousYPosition As Integer
        Private blockRelativePositions() As Block
        Private centreOfRotation As Block

        ' Private shape  i'll do this later since its gonna be awkward.
        Sub New(shapeType As TetrominoType)

            xPosition = 5
            yPosition = 0
            Me.shapeType = shapeType

            selectPieceType()
        End Sub
        Function getPotentialRotation(direction As DirectionType)
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
        Sub Rotate(direction As DirectionType)
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
        Function getBlockRelativePositions()
            Return blockRelativePositions
        End Function
        Function getCentreOfRotation()
            Return centreOfRotation
        End Function
        Sub selectPieceType()

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
        Private Sub UpdatePreviousPosition()
            previousXPosition = xPosition
            previousYPosition = yPosition
        End Sub
        Public Sub MovePiece(direction As DirectionType)
            If direction = DirectionType.Left Then
                MoveLeft()
            ElseIf direction = DirectionType.Right Then
                MoveRight()
            End If
        End Sub

        Private Sub MoveLeft()
            xPosition -= 1
        End Sub
        Private Sub MoveRight()
            xPosition += 1
        End Sub
        Public Function GetShapeType() As TetrominoType
            Return shapeType
        End Function
        Public Function GetPreviousXPosition() As Integer
            Return previousXPosition
        End Function
        Public Function GetPreviousYPosition() As Integer
            Return previousYPosition
        End Function
        Public Function GetXPosition() As Integer
            Return xPosition
        End Function
        Public Function GetYPosition() As Integer
            Return yPosition
        End Function

        Public Sub Update()

            UpdatePreviousPosition()
            yPosition += 1
        End Sub
    End Class

    Class Button
        Private bounds As Rectangle
        Private action As Action
        Private text As String
        Private baseColour As Color
        Private hoverColour As Color
        Private textColour As Color
        Private font As Font

        Public Sub New(x As Integer, y As Integer, width As Integer, height As Integer, text As String, textColour As Color, baseColour As Color, hoverColour As Color, action As Action)
            bounds = New Rectangle(x, y, width, height)
            Me.text = text
            Me.baseColour = baseColour
            Me.hoverColour = hoverColour
            Me.textColour = textColour
            Me.action = action
            font = New Font("Consolas", 16)
        End Sub

        Public Sub New(x As Integer, y As Integer, width As Integer, height As Integer, text As String, textColour As Color, baseColour As Color, hoverColour As Color, font As Font)
            bounds = New Rectangle(x, y, width, height)
            Me.text = text
            Me.baseColour = baseColour
            Me.textColour = textColour
            Me.hoverColour = hoverColour
            Me.font = font
        End Sub
        Public Sub Draw(g As Graphics, mouseX As Integer, mouseY As Integer)
            Dim isHover = bounds.Contains(mouseX, mouseY)
            Dim colour = If(isHover, baseColour, hoverColour)

            g.FillRectangle(New SolidBrush(colour), bounds)

            Dim textSize As SizeF = g.MeasureString(text, font)
            Dim textXPosition As Single = bounds.X + (bounds.Width - textSize.Width) / 2
            Dim textYPosition As Single = bounds.Y + (bounds.Height - textSize.Height) / 2
            g.DrawString(text, font, New SolidBrush(textColour), textXPosition, textYPosition)
        End Sub

        Public Sub HandleClick(mouseX As Integer, mouseY As Integer)
            If bounds.Contains(mouseX, mouseY) Then
                action.Invoke()
            End If
        End Sub
    End Class

    ' needs to be cyclic cause its a fixed amount.
    Class Queue(Of T)
            Private itemsList As New List(Of T)
            Public Function IsEmpty()
                If itemsList.Count = 0 Then
                    Return True
                End If
                Return False
            End Function
            Public Sub Enqueue(item As T)
                itemsList.Add(item)
            End Sub
            Public Function Dequeue() As T
                If IsEmpty() Then
                    Throw New InvalidOperationException("Queue is empty")
                End If
                Dim queuedValue As T = itemsList(0)
                itemsList.RemoveAt(0)
                Return queuedValue
            End Function
            Public Sub Randomise()
                For i = 0 To itemsList.Count - 2
                    Dim j = rand.Next(i, itemsList.Count)
                    Dim temp As T = itemsList(i)
                    itemsList(i) = itemsList(j)
                    itemsList(j) = temp
                Next
            End Sub
        End Class
#End Region


#Region "These sections should not be modified"


#Region "GameWindowSetup"
        ' Set up the double buffer and Game Loop
        ' You should not need to modify any of this code
        Private backBuffer As Image
        Private bufferDisplay As Graphics
        Private graphicsDisplay As Graphics
        Private Shadows Sub Paint(ByVal g As Graphics)
            GameDraw(g)
        End Sub
        Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
            Me.Text = ApplicationName
            Me.SetStyle(ControlStyles.AllPaintingInWmPaint, True)
            Me.SetStyle(ControlStyles.OptimizedDoubleBuffer, True)
            Me.SetStyle(ControlStyles.UserPaint, True)
            Me.Size = DisplaySize
            Me.ClientSize = DisplaySize


            ' style and prevent resizing
            Me.FormBorderStyle = FormBorderStyle.FixedSingle
            Me.SetStyle(ControlStyles.FixedHeight, True)
            Me.SetStyle(ControlStyles.FixedWidth, True)
            Me.MinimumSize = DisplaySize
            Me.MaximumSize = DisplaySize
            Me.Update()


            ' setting up our buffers
            backBuffer = New Bitmap(Width, Height)
            bufferDisplay = Graphics.FromImage(backBuffer)
            graphicsDisplay = Me.CreateGraphics


            bufferDisplay.InterpolationMode = InterpolationMode.High
            bufferDisplay.SmoothingMode = SmoothingMode.AntiAlias
            bufferDisplay.TextRenderingHint = TextRenderingHint.AntiAlias


            Dim scrn = Screen.FromPoint(Cursor.Position)
            Me.Location = scrn.Bounds.Location
            Me.CenterToScreen()


            GameLoad()
            ' starting our game loop
            Dim refresh As Timer = New Timer
            refresh.Interval = FrameInterval
            refresh.Enabled = True
            AddHandler refresh.Tick, AddressOf GameLoop
        End Sub
        Private Sub GameLoop(ByVal sender As Object, ByVal e As EventArgs)
            GameUpdate()
            If ((Me.Disposing = False) And (Me.IsDisposed = False) And (Me.Visible = True)) Then
                Paint(bufferDisplay)
                graphicsDisplay.DrawImageUnscaled(backBuffer, New Point(0, 0))
            End If
        End Sub
#End Region


#Region "KeyboardEvents"
        ' You should not need to modify this section
        Private Sub Form1_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
            If KeyPressed.ContainsKey(e.KeyCode) Then
                KeyPressed(e.KeyCode) = True
            Else
                KeyPressed.Add(e.KeyCode, True)
            End If
        End Sub


        Private Sub Form1_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
            If KeyPressed.ContainsKey(e.KeyCode) Then
                KeyPressed(e.KeyCode) = False
                ' Cancel the key delay to allow an instant repress
                KeyDelayCounter = KeyRepeatInterval
            Else
                KeyPressed.Add(e.KeyCode, False)
            End If
        End Sub


#End Region


#Region "MouseEvents"
        Private Sub Form1_MouseClick(sender As Object, e As MouseEventArgs) Handles Me.MouseClick
            If e.Button = MouseButtons.Left Then
                MouseLeftClicked = True
            End If
            If e.Button = MouseButtons.Right Then
                MouseRightClicked = True
            End If
        End Sub


        Private Sub Form1_MouseMove(sender As Object, e As MouseEventArgs) Handles Me.MouseMove
            MouseX = e.Location.X
            MouseY = e.Location.Y
        End Sub


        Private Sub Form1_MouseDown(sender As Object, e As MouseEventArgs) Handles Me.MouseDown
            If e.Button = MouseButtons.Left Then
                MouseLeftDown = True
            End If
            If e.Button = MouseButtons.Right Then
                MouseRightDown = True
            End If
        End Sub


        Private Sub Form1_MouseUp(sender As Object, e As MouseEventArgs) Handles Me.MouseUp
            If e.Button = MouseButtons.Left Then
                MouseLeftDown = False
            End If
            If e.Button = MouseButtons.Right Then
                MouseRightDown = False
            End If
        End Sub


        'Private Sub Label1_Click(sender As Object, e As EventArgs) Handles Label1.Click

        'End Sub






#End Region


#End Region


    End Class
