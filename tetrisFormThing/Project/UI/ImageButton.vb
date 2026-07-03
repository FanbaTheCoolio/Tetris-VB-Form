Public Class ImageButton
    Inherits BaseButton

    Private image As Image

    Public Sub New(x As Integer, y As Integer, width As Integer, height As Integer,
           image As Image,
           action As Action,
                   audioManager As AudioManager)

        MyBase.New(x, y, width, height, action, audioManager)
        Me.image = image
    End Sub

    Public Overrides Sub Draw(g As Graphics, mouseX As Integer, mouseY As Integer)
        Dim isHover = bounds.Contains(mouseX, mouseY)
        UpdateScale(isHover)

        Dim drawBounds = GetScaledBounds()
        g.DrawImage(image, drawBounds)
    End Sub
End Class
