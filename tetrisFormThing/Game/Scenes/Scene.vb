Public MustInherit Class Scene


    Protected manager As SceneManager
    Protected buttons As New List(Of BaseButton)
    Protected screenWidth, screenHeight As Integer

    Public Sub New(screenWidth As Integer, screenHeight As Integer, manager As SceneManager)
        Me.screenWidth = screenWidth
        Me.screenHeight = screenHeight
        Me.manager = manager
    End Sub

    Public MustOverride Sub Update(keys As Dictionary(Of Integer, Boolean))

    Public MustOverride Sub Draw(g As Graphics, mouseX As Integer, mouseY As Integer)

    Public MustOverride Sub HandleClick(mouseX As Integer, mouseY As Integer)

    Protected Function GetHorizontalCenter(width As Integer) As Integer
        Return (screenWidth - width) \ 2
    End Function

    Protected Function GetVerticalCenter(height As Integer) As Integer
        Return (screenHeight - height) \ 2
    End Function

    Protected Function GetRelativeX(percent As Double) As Integer
        Return CInt(screenWidth * percent)
    End Function

    Protected Function GetRelativeY(percent As Double) As Integer
        Return CInt(screenHeight * percent)
    End Function


    Protected Function getMinimumRelativePositionX(previewTetromino As Tetromino) As Integer
        Dim minimumRelativePosition As Integer = 0
        For Each relativePosition In previewTetromino.GetBlockRelativePositions
            Dim x = relativePosition.X

            If x < minimumRelativePosition Then
                minimumRelativePosition = x
            End If

        Next
        Return minimumRelativePosition
    End Function

    Protected Function getMaximumRelativePositionX(previewTetromino As Tetromino) As Integer
        Dim maximumRelativePosition As Integer = 0
        For Each relativePosition In previewTetromino.GetBlockRelativePositions
            Dim x = relativePosition.X

            If x > maximumRelativePosition Then
                maximumRelativePosition = x
            End If


        Next
        Return maximumRelativePosition
    End Function
End Class
