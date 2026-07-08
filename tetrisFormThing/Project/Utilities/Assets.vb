Imports System.IO

Public Module Assets
    Private ReadOnly root As String = Path.Combine(Application.StartupPath, "Resources")
    Private ReadOnly audioRoot As String = Path.Combine(root, "Audio")
    Private ReadOnly spriteRoot As String = Path.Combine(root, "Sprites")
    Private ReadOnly musicRoot As String = Path.Combine(audioRoot, "Music")
    Private ReadOnly sfxRoot As String = Path.Combine(audioRoot, "SFX")


#Region "Sprites"
    Public ReadOnly spritePause As String = Path.Combine(spriteRoot, "Pause.png")
    Public ReadOnly spritePlay As String = Path.Combine(spriteRoot, "Play.png")
#End Region



#Region "SFX"
    Public ReadOnly sfxClick As String = Path.Combine(sfxRoot, "buttonClick.wav")
    Public ReadOnly gameOver As String = Path.Combine(sfxRoot, "gameOver.mp3")
    Public ReadOnly lineClear As String = Path.Combine(sfxRoot, "lineClear.wav")
    Public ReadOnly rotatePiece As String = Path.Combine(sfxRoot, "rotatePiece.wav")
    Public ReadOnly movePiece As String = Path.Combine(sfxRoot, "movePiece.wav")
#End Region

#Region "Music"
    Public ReadOnly gameMusicTrack As String = Path.Combine(musicRoot, "gameTrack.mp3")
    Public ReadOnly gameOverTrack As String = Path.Combine(musicRoot, "gameOver.mp3")
    Public ReadOnly titleScreenTrack As String = Path.Combine(musicRoot, "titleScreen.mp3")
#End Region

End Module
