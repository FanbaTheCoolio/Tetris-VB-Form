
Public Class CyclicQueue(Of T)
    Private items() As T
    Private front As Integer = 0
    Private rear As Integer = -1
    Private count As Integer = 0
    Private capacity As Integer
    Private Shared rand As New Random()
    Public Sub New(size As Integer)
        Randomize()
        capacity = size
        ReDim items(size - 1)
    End Sub

    Public Function IsEmpty() As Boolean
        Return count = 0
    End Function

    Public Function IsFull()
        Return count = capacity
    End Function

    Public Sub Enqueue(item As T)
        If isFull() Then
            Throw New InvalidOperationException("Queue is full")
        End If

        rear = (rear + 1) Mod capacity
        items(rear) = item
        count += 1
    End Sub

    Public Function Dequeue() As T
        If IsEmpty() Then
            Throw New InvalidOperationException("Queue is empty")
        End If

        Dim value As T = items(front)
        front = (front + 1) Mod capacity
        count -= 1

        Return value
    End Function

    Public Function Peek() As T
        If IsEmpty() Then
            Throw New InvalidOperationException("Queue is empty")
        End If

        Return items(front)
    End Function
    Public Sub Randomise()
        For i = 0 To items.Count - 2
            Dim j = rand.Next(i, items.Count)
            Dim temp As T = items(i)
            items(i) = items(j)
            items(j) = temp
        Next
    End Sub
End Class
