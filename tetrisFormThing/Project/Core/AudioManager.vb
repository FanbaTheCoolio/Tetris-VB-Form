Imports System.Runtime
Imports NAudio.Dmo
Imports NAudio.Wave

Public Class AudioManager
    Private sfxPaths As New Dictionary(Of SoundEffect, String)
    Private musicPaths As New Dictionary(Of MusicTrack, String)

    Private currentMusicOutput As WaveOutEvent
    Private currentMusicReader As AudioFileReader

    Private sfxVolume As Double = 1.0F
    Private musicVolume As Double = 1.0F

    Private isStoppingMusic As Boolean = False

    Private musicStoppedHandler As EventHandler(Of StoppedEventArgs)

    Public Sub New()
        LoadSFX(SoundEffect.ButtonClick, Assets.sfxClick)
        LoadSFX(SoundEffect.GameOver, Assets.gameOver)
        LoadSFX(SoundEffect.RotatePiece, Assets.rotatePiece)
        LoadSFX(SoundEffect.MovePiece, Assets.movePiece)
        LoadSFX(SoundEffect.LineClear, Assets.lineClear)

        LoadMusic(MusicTrack.gameOverTrack, Assets.gameOverTrack)
        LoadMusic(MusicTrack.gameTrack, Assets.gameMusicTrack)
        LoadMusic(MusicTrack.titleTrack, Assets.titleScreenTrack)
    End Sub
    Private Sub LoadSFX(sfx As SoundEffect, fileLocation As String)
        sfxPaths(sfx) = fileLocation
    End Sub
    Private Sub LoadMusic(musicTrack As MusicTrack, fileLocation As String)
        musicPaths(musicTrack) = fileLocation
    End Sub

    Public Sub PlaySFX(sfx As SoundEffect)
        If Not sfxPaths.ContainsKey(sfx) Then Exit Sub

        Dim reader As New AudioFileReader(sfxPaths(sfx))
        reader.Volume = sfxVolume

        Dim output As New WaveOutEvent()
        output.Init(reader)

        AddHandler output.PlaybackStopped,
            Sub()
                output.Dispose()
                reader.Dispose()
            End Sub

        output.Play()
    End Sub

    Public Sub PlayMusic(track As MusicTrack)
        StopMusic()

        currentMusicReader = New AudioFileReader(musicPaths(track))
        Debug.WriteLine("Labubu" & musicPaths(track))
        currentMusicReader.Volume = musicVolume

        currentMusicOutput = New WaveOutEvent()
        currentMusicOutput.Init(currentMusicReader)


        musicStoppedHandler =
            Sub()
                PlayMusic(track)
            End Sub


        AddHandler currentMusicOutput.PlaybackStopped, musicStoppedHandler

        currentMusicOutput.Play()

    End Sub

    Public Sub StopMusic()
        If currentMusicOutput IsNot Nothing Then
            If musicStoppedHandler IsNot Nothing Then
                RemoveHandler currentMusicOutput.PlaybackStopped, musicStoppedHandler
                musicStoppedHandler = Nothing
            End If

            currentMusicOutput.Stop()
            currentMusicOutput.Dispose()
            currentMusicOutput = Nothing

        End If


        If currentMusicReader IsNot Nothing Then
            currentMusicReader.Dispose()
            currentMusicReader = Nothing
        End If
    End Sub


End Class
