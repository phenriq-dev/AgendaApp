Imports System.ComponentModel.DataAnnotations

Public Class Appointment
    <Key>
    Public Property Id As Integer
    Public Property UserId As Integer
    Public Property Title As String
    Public Property Description As String
    Public Property DateTime As DateTime
    Public Overridable Property User As User
End Class
