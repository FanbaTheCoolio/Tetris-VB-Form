' A Framework for creating 2D graphical applications
' such as games in VB.Net, by Neil Kendall
' Based on https://tinyurl.com/5b25pvv9
' Create a new VB.Net Forms application. Paste all of this code into Form1's code view.


Imports System.Drawing.Drawing2D
Imports System.Drawing.Text
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

    Dim gridOffsetStartX As Integer '= ClientSize.Width * 0.85
    Dim gridOffsetStartY As Integer = 0

    Dim tileSize As Integer = 40
    Dim gridHeight As Integer = 18
    Dim gridWidth As Integer = 8

    Dim board(gridWidth, gridHeight) As TetrominoType

    Dim borderColor As Color = Color.RebeccaPurple

    Dim currentTetromino As Tetromino
    Dim tetrominoBag As New Queue(Of TetrominoType)
    Dim tetrominoDelayCounter As Integer = 0
    Dim tetrominoUpdateInterval As Integer = 30 ' In miliseconds
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
        Randomize()

        RefillBag()
        currentTetromino = New Tetromino(tetrominoBag.Dequeue)

        gridOffsetStartX = (ClientSize.Width - (tileSize * gridWidth) - tileSize) / 2


        For y As Integer = 0 To gridHeight
            For x As Integer = 0 To gridWidth
                board(x, y) = TetrominoType.None
            Next
        Next


        'board(4, 5) = TetrominoType.S_Piece
        'board(7, 16) = TetrominoType.O_Piece
        'board(6, 8) = TetrominoType.T_Piece
        'board(2, 5) = TetrominoType.Z_Piece
        'board(5, 6) = TetrominoType.O_Piece
        'board(7, 3) = TetrominoType.L_Piece
        'board(3, 4) = TetrominoType.J_Piece
        ' **********************************************************************
        ' ** The code above will run once when the application loads / starts **
        ' **********************************************************************
    End Sub
#End Region


#Region "Game State Update"
    ' All game Updates should be put here e.g. reading keyboard/ mouse, updating sprite positions (but NOT drawing them), calculations etc.
    Private Sub GameUpdate()
        'TODO : ADD THE RANDOM BAG SYSTEM FOR TETROMINOS LATER

        tetrominoDelayCounter += 1

        If tetrominoDelayCounter >= tetrominoUpdateInterval Then
            Debug.WriteLine("Y : " & currentTetromino.GetYPosition)
            Debug.WriteLine(ShouldBeLocked)
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



#Region "Check Keys"
        ' Up Arrow key = 38. Use 40 for down, 37 for left And 39 for right. 65-90 for A-Z. 48-57 for 0-9
        ' All key codes here https://learn.microsoft.com/en-us/dotnet/api/system.windows.forms.keys


        ' Up
        If KeyDelayCounter = KeyRepeatInterval Then




            If KeyPressed.ContainsKey(37) Then
                If KeyPressed(37) Then
                    ' *********************************************************
                    ' ** The code below runs when the LEFT (37) key is pressed **
                    ' *********************************************************

                    AttemptMoveLeft()

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

                    AttemptMoveLeft()

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
                    AttemptMoveRight()
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
                    AttemptMoveRight()
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

        DrawTetris(g)

        ' ********************************************
        ' ** Put your code to draw each frame above **
        ' ********************************************
    End Sub
#End Region


#Region "User Defined Methods"
    ' ****************************************************************
    ' ** Put the subs and functions for your application below here **
    ' ****************************************************************
    Sub DrawTetris(g As Graphics)
        g.Clear(Color.Black)
        DrawBorders(g)
        DrawLockedPieces(g)
        DrawCurrentPiece(g)
    End Sub
    Sub DrawBorders(g As Graphics)
        g.FillRectangle(New SolidBrush(borderColor), 0, 0, gridOffsetStartX, ClientSize.Height)
        g.FillRectangle(New SolidBrush(borderColor), gridOffsetStartX + (tileSize * gridWidth) + tileSize, 0, ClientSize.Width, ClientSize.Height)
    End Sub
    Sub DrawCurrentPiece(g As Graphics)

        Dim currentTileXPosition = gridOffsetStartX + (currentTetromino.GetXPosition * tileSize)
        Dim currentTileYPosition = gridOffsetStartY + (currentTetromino.GetYPosition * tileSize)
        g.FillRectangle(New SolidBrush(currentTetromino.GetTileColour), currentTileXPosition, currentTileYPosition, tileSize, tileSize)


    End Sub
    Sub DrawLockedPieces(g As Graphics)
        For columns = 0 To gridHeight
            For rows = 0 To gridWidth
                Dim currentTileXPosition = gridOffsetStartX + (rows * tileSize)
                Dim currentTileYPosition = gridOffsetStartY + (columns * tileSize)
                g.DrawRectangle(Pens.Gray, currentTileXPosition, currentTileYPosition, tileSize, tileSize)
                If board(rows, columns) <> TetrominoType.None Then
                    g.FillRectangle(New SolidBrush(GetTetrominoColour(board(rows, columns))), currentTileXPosition, currentTileYPosition, tileSize, tileSize)
                End If
            Next
        Next
    End Sub
    Function ShouldBeLocked()
        If currentTetromino.GetYPosition < gridHeight Then
            If board(currentTetromino.GetXPosition, currentTetromino.GetYPosition + 1) <> TetrominoType.None Then
                Return True
            End If
        End If
        If currentTetromino.GetYPosition >= (gridHeight) Then
            Return True
        End If
        Return False
    End Function

    Sub LockPiece()
        board(currentTetromino.GetXPosition, currentTetromino.GetYPosition) = currentTetromino.GetShapeType
        If tetrominoBag.IsEmpty Then
            RefillBag()
        End If
        currentTetromino = New Tetromino(tetrominoBag.Dequeue)

    End Sub

    Sub RefillBag()
        For Each piece As TetrominoType In [Enum].GetValues(GetType(TetrominoType))
            If piece <> TetrominoType.None Then
                tetrominoBag.Enqueue(piece)
            End If
        Next
        tetrominoBag.Randomise()
    End Sub
    Function GetTetrominoColour(value As TetrominoType) As Color
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
    Sub AttemptMoveLeft()
        If TryTransformation(currentTetromino.GetXPosition - 1, currentTetromino.GetYPosition) Then
            currentTetromino.MovePiece(DirectionType.Left)
        End If
    End Sub
    Sub AttemptMoveRight()
        If TryTransformation(currentTetromino.GetXPosition + 1, currentTetromino.GetYPosition) Then
            currentTetromino.MovePiece(DirectionType.Right)
        End If
    End Sub
    Function TryTransformation(newX As Integer, newY As Integer) As Boolean
        Dim xPosCondition As Boolean = (newX >= 0 And newX <= gridWidth)
        Dim yPosCondition As Boolean = (newY >= 0 And newY <= gridHeight)


        Debug.WriteLine("X Pos : " & xPosCondition)
        Debug.WriteLine("Y Pos : " & yPosCondition)


        If xPosCondition And yPosCondition Then
            Dim isSpotClear As Boolean = (board(newX, newY) = TetrominoType.None)
            Debug.WriteLine("Is Spot Clear : " & isSpotClear)
            If isSpotClear Then
                Return True
            End If
        End If
        Return False
    End Function

    ' ****************************************************************
    ' ** Put the subs and functions for your application above here **
    ' ****************************************************************
#End Region


#Region "User Defined Classes"
    Public Structure Block
        Public X As Integer
        Public Y As Integer
    End Structure
    Class Tetromino
        Private xPosition, yPosition, boardWidth, boardHeight As Integer
        Private shapeType As TetrominoType
        Private previousXPosition, previousYPosition As Integer

        Private isActive As Boolean
        ' Private shape  i'll do this later since its gonna be awkward.
        Sub New(shapeType As TetrominoType)

            xPosition = 5
            yPosition = 0
            isActive = True
            Me.shapeType = shapeType
        End Sub
        Public Function GetTileColour() As Color
            Return Form1.GetTetrominoColour(shapeType)
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
            Debug.WriteLine("Left")
        End Sub
        Private Sub MoveRight()
            xPosition += 1
            Debug.WriteLine("Right")
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
