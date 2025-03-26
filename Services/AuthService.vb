Imports System.Linq
Imports System.Security.Cryptography
Imports System.Text

Public Class AuthService
    Implements IDisposable

    Private _dbContext As AgendaContext

    Public Sub New()
        _dbContext = New AgendaContext()
    End Sub

    Public Function Login(email As String, password As String) As OperationResult(Of User)
        Try
            If String.IsNullOrWhiteSpace(email) OrElse String.IsNullOrWhiteSpace(password) Then
                Return New OperationResult(Of User)(False, "Email e senha são obrigatórios.")
            End If

            Dim hashedPassword = ComputeHash(password)
            Dim user = _dbContext.Users.FirstOrDefault(Function(u) u.Email = email AndAlso u.PasswordHash = hashedPassword)

            If user Is Nothing Then
                Return New OperationResult(Of User)(False, "Credenciais inválidas.")
            End If

            Return New OperationResult(Of User)(True, "Login realizado com sucesso.", user)
        Catch ex As Exception
            Return New OperationResult(Of User)(False, $"Erro ao realizar login: {ex.Message}")
        End Try
    End Function

    Public Function Register(user As User) As OperationResult
        Try
            If user Is Nothing Then
                Return New OperationResult(False, "Dados do usuário inválidos.")
            End If

            If CheckIfEmailExists(user.Email) Then
                Return New OperationResult(False, "Este email já está cadastrado.")
            End If

            user.PasswordHash = user.PasswordHash
            _dbContext.Users.Add(user)
            _dbContext.SaveChanges()

            Return New OperationResult(True, "Usuário registrado com sucesso.")
        Catch ex As Exception
            Return New OperationResult(False, $"Erro ao registrar usuário: {ex.Message}")
        End Try
    End Function

    Public Function CheckIfEmailExists(email As String) As Boolean
        If String.IsNullOrWhiteSpace(email) Then Return False

        Return _dbContext.Users.Any(Function(u) u.Email.Equals(email, StringComparison.OrdinalIgnoreCase))
    End Function

    Public Function ComputeHash(input As String) As String
        If String.IsNullOrEmpty(input) Then Return String.Empty

        Using sha256 As SHA256 = SHA256.Create()
            Dim bytes = Encoding.UTF8.GetBytes(input)
            Dim hashBytes = sha256.ComputeHash(bytes)
            Return Convert.ToBase64String(hashBytes)
        End Using
    End Function

    Public Sub Dispose() Implements IDisposable.Dispose
        If _dbContext IsNot Nothing Then
            _dbContext.Dispose()
            _dbContext = Nothing
        End If
    End Sub
End Class