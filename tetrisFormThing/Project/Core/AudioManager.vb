Imports System.Runtime
Imports NAudio.Dmo
Imports NAudio.Wave

Public Class AudioManager
    Private sfxPaths As New Dictionary(Of SoundEffect, String)
    Private musicPaths As New Dictionary(Of MusicTrack, String)

    Private currentMusicOutput As WaveOutEvent
    Private currentMusicReader As AudioFileReader

    Private Sub LoadSFX(sfx As SoundEffect, fileLocation As String)
        sfxPaths(sfx) = fileLocation
    End Sub
    Private Sub LoadMusic(musicTrack As MusicTrack, fileLocation As String)
        musicPaths(musicTrack) = fileLocation
    End Sub

    Public Sub PlaySFX(sfx As SoundEffect)
        If Not sfxPaths.ContainsKey(sfx) Then Exit Sub

        Dim reader As New AudioFileReader(sfxPaths(sfx))
        reader.Volume = 1

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
        If Not musicPaths.ContainsKey(track) Then Exit Sub

        StopMusic()

        currentMusicReader = New AudioFileReader(musicPaths(track))
        currentMusicReader.Volume = 1

        currentMusicOutput = New WaveOutEvent()
        currentMusicOutput.Init(currentMusicReader)

        AddHandler currentMusicOutput.PlaybackStopped,
            Sub()

                PlayMusic(track)

            End Sub

        currentMusicOutput.Play()

    End Sub

    Public Sub StopMusic()

        If currentMusicOutput IsNot Nothing Then
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
