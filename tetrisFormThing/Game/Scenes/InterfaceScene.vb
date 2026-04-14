Public Interface InterfaceScene
    Sub Update(keys As Dictionary(Of Integer, Boolean))
    Sub Draw(g As Graphics, mouseX As Integer, mouseY As Integer)

    Sub HandleClick(mouseX As Integer, mouseY As Integer)
End Interface
