Imports System.ComponentModel.DataAnnotations

Public Class User
    <Key>
    Public Property Id As Integer
    Public Property Name As String
    Public Property Email As String
    Public Property PasswordHash As String
End Class
