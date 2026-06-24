Imports System.IO

Public Module Assets
    Private ReadOnly root As String = "C:\Users\faheem\source\repos\tetrisFormThing\tetrisFormThing\Resources\"
    Private ReadOnly audioRoot As String = Path.Combine(root, "Audio")
    Private ReadOnly spriteRoot As String = Path.Combine(root, "Sprites")
    Private ReadOnly musicRoot As String = Path.Combine(audioRoot, "Music")
    Private ReadOnly sfxRoot As String = Path.Combine(audioRoot, "SFX")

    Public ReadOnly spritePause As String = Path.Combine(spriteRoot, "Pause.png")
    Public ReadOnly spritePlay As String = Path.Combine(spriteRoot, "Play.png")

    Public ReadOnly sfxClick As String = Path.Combine(sfxRoot, "buttonClick.wav")
End Module
