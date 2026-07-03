Public Class TextButton
    Inherits BaseButton

    Private text As String
    Private baseColour As Color
    Private hoverColour As Color
    Private textColour As Color
    Private font As Font

    Public Sub New(x As Integer, y As Integer, width As Integer, height As Integer,
         text As String, textColour As Color,
         baseColour As Color, hoverColour As Color,
         action As Action,
                   audioManager As AudioManager)
        MyBase.New(x, y, width, height, action, audioManager)

        Me.textColour = textColour
        Me.baseColour = baseColour
        Me.hoverColour = hoverColour
        Me.text = text
        Me.font = New Font("Consolas", 16)

    End Sub

    Public Overrides Sub Draw(g As Graphics, mouseX As Integer, mouseY As Integer)
        Dim isHover = bounds.Contains(mouseX, mouseY)
        Dim colour = If(isHover, hoverColour, baseColour)
        Dim drawBounds = GetScaledBounds()
        UpdateScale(isHover)

        g.FillRectangle(New SolidBrush(colour), drawBounds)

        Dim textSize As SizeF = g.MeasureString(text, font)
        Dim textX = drawBounds.X + (drawBounds.Width - textSize.Width) / 2
        Dim textY = drawBounds.Y + (drawBounds.Height - textSize.Height) / 2

        g.DrawString(text, font, New SolidBrush(textColour), textX, textY)
    End Sub
End Class
