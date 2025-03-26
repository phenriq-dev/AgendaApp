Public Class OperationResult
    Public Property Success As Boolean
    Public Property Message As String

    Public Sub New(success As Boolean, Optional message As String = "")
        Me.Success = success
        Me.Message = message
    End Sub
End Class

Public Class OperationResult(Of T)
    Inherits OperationResult

    Public Property Data As T

    Public Sub New(success As Boolean, Optional message As String = "", Optional data As T = Nothing)
        MyBase.New(success, message)
        Me.Data = data
    End Sub
End Class