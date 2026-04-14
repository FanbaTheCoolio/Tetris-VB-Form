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
Imports System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel
Imports NAudio.Wave
Imports tetrisFormThing.Form1


Public Class Form1
    Public ApplicationName As String = "2D Gfx Framework"
    Public Shared DisplaySize As Size = New Size(800, 800)
    Public FrameInterval As Integer = 16 ' Time in milliseconds between each frame render. 16 milliseconds is approximately 60 FPS (Frames Per Second)
    ' If a key is held down, this is the number of frames between each repeat. Set as 0 for no delay


#Region "Framework Variables"
    ' Framework Variables
    Public KeyPressed As Dictionary(Of Integer, Boolean) = New Dictionary(Of Integer, Boolean)

    Public MouseLeftClicked As Boolean = False ' Has the left mouse button been clicked?
    Public MouseRightClicked As Boolean = False ' Has the right mouse button been clicked?
    Public MouseLeftDown As Boolean = False ' Is the left mouse button currently down?
    Public MouseRightDown As Boolean = False ' Is the right mouse button currently down?
    Public MouseX, MouseY As Integer ' These hold the current x and y positions of the mousepointer relative to the upper left corner of the form
    'Public Shared audioController As New AudioManager
    Public Shared rand As Random = New Random

#End Region


#Region "Application Variables"
    ' *********************************************
    ' ** Define your application variables below **
    ' *********************************************


    Dim currentSceneManager As SceneManager


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

        ' **********************************************************************
        ' ** The code below will run once when the application loads / starts **
        ' **********************************************************************

        currentSceneManager = New SceneManager(ClientSize.Width, ClientSize.Height)
        'audioController.Load("ClickSound", My.Resources.Resource1.ButtonClickSound1)
        ' **********************************************************************
        ' ** The code above will run once when the application loads / starts **
        ' **********************************************************************
    End Sub
#End Region


#Region "Game State Update"
    ' All game Updates should be put here e.g. reading keyboard/ mouse, updating sprite positions (but NOT drawing them), calculations etc.
    Private Sub GameUpdate()


        currentSceneManager.Update(KeyPressed)


#Region "Check Keys"
        ' Up Arrow key = 38. Use 40 for down, 37 for left And 39 for right. 65-90 for A-Z. 48-57 for 0-9
        ' All key codes here https://learn.microsoft.com/en-us/dotnet/api/system.windows.forms.keys


        ' Up


#End Region


#Region "Check Mouse Clicks"
        If MouseLeftClicked = True Then
            ' *****************************************************************
            ' ** The code below is run when the left mouse button is CLICKED **
            ' *****************************************************************
            currentSceneManager.HandleClick(MouseX, MouseY)

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




#Region "Scenes"










#End Region




    'Class AudioManager
    '    Private sounds As New Dictionary(Of String, AudioFileReader)
    '    Private outputs As New Dictionary(Of String, WaveOutEvent)

    '    Public Sub Load(name As String, resourceStream As System.IO.UnmanagedMemoryStream)
    '        Dim player As New System.Media.SoundPlayer(resourceStream)
    '        player.Load()
    '        sounds(name) = player
    '    End Sub

    '    Public Sub Play(name As String)
    '        If Not sounds.ContainsKey(name) Then Return

    '        sounds(name).Play()
    '    End Sub
    'End Class





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
            ' Cancel the key delay to allow an instant repress (Should fix this)
            'TACKY FIX, MAKE IT BETTER LAAAATER
            currentSceneManager.ResetKeyDelay()
            'KeyDelayCounter = KeyRepeatInterval
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
