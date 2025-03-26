Imports System.Data.Entity

Public Class AgendaContext
    Inherits DbContext

    Public Sub New()
        MyBase.New("AgendaContext")
    End Sub

    Public Property Users As DbSet(Of User)
    Public Property Appointments As DbSet(Of Appointment)
End Class
