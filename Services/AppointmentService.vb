Public Class AppointmentService
    Implements IDisposable

    Private _dbContext As AgendaContext

    Public Sub New()
        _dbContext = New AgendaContext()
    End Sub

    Public Function GetAppointments(userId As Integer) As OperationResult(Of List(Of AppointmentResult))
        Try
            Dim appointments = _dbContext.Appointments.
                Where(Function(a) a.UserId = userId).
                Select(Function(a) New AppointmentResult With {
                    .Id = a.Id,
                    .Title = a.Title,
                    .Description = a.Description,
                    .DateAndTime = a.DateTime,
                    .User = a.User.Name
                }).ToList()

            Return New OperationResult(Of List(Of AppointmentResult))(True, "Compromissos carregados com sucesso.", appointments)
        Catch ex As Exception
            Return New OperationResult(Of List(Of AppointmentResult))(False, $"Erro ao carregar compromissos: {ex.Message}")
        End Try
    End Function

    Public Function GetAppointmentById(appointmentId As Integer) As OperationResult(Of Appointment)
        Try
            Dim appointment = _dbContext.Appointments.
                FirstOrDefault(Function(a) a.Id = appointmentId)

            If appointment Is Nothing Then
                Return New OperationResult(Of Appointment)(False, $"Compromisso com ID {appointmentId} não encontrado.")
            End If

            Return New OperationResult(Of Appointment)(True, "Compromisso encontrado.", appointment)
        Catch ex As Exception
            Return New OperationResult(Of Appointment)(False, $"Erro ao buscar compromisso: {ex.Message}")
        End Try
    End Function

    Public Function AddAppointment(userId As Integer, title As String, description As String, dateTime As DateTime) As OperationResult(Of Integer)
        Try
            Dim validation = ValidateAppointmentInputs(title, description, dateTime)
            If Not validation.Success Then
                Return New OperationResult(Of Integer)(validation.Success, validation.Message)
            End If

            Dim newAppointment As New Appointment With {
                .UserId = userId,
                .Title = title,
                .Description = description,
                .DateTime = dateTime
            }

            _dbContext.Appointments.Add(newAppointment)
            _dbContext.SaveChanges()

            Return New OperationResult(Of Integer)(True, "Compromisso adicionado com sucesso.", newAppointment.Id)
        Catch ex As Exception
            Return New OperationResult(Of Integer)(False, $"Erro ao adicionar compromisso: {ex.Message}")
        End Try
    End Function

    Public Function EditAppointment(userId As Integer, id As Integer, title As String, description As String, dateTime As DateTime) As OperationResult
        Try
            Dim validation = ValidateAppointmentInputs(title, description, dateTime)
            If Not validation.Success Then
                Return validation
            End If

            Dim appointmentToEdit = _dbContext.Appointments.Find(id)
            If appointmentToEdit Is Nothing Then
                Return New OperationResult(False, $"Compromisso com ID {id} não encontrado.")
            End If

            appointmentToEdit.Title = title
            appointmentToEdit.Description = description
            appointmentToEdit.DateTime = dateTime
            appointmentToEdit.UserId = userId

            _dbContext.SaveChanges()
            Return New OperationResult(True, "Compromisso atualizado com sucesso.")
        Catch ex As Exception
            Return New OperationResult(False, $"Erro ao editar compromisso: {ex.Message}")
        End Try
    End Function

    Public Function DeleteAppointment(id As Integer) As OperationResult
        Try
            Dim appointment = _dbContext.Appointments.Find(id)
            If appointment Is Nothing Then
                Return New OperationResult(False, $"Compromisso com ID {id} não encontrado.")
            End If

            _dbContext.Appointments.Remove(appointment)
            _dbContext.SaveChanges()
            Return New OperationResult(True, "Compromisso excluído com sucesso.")
        Catch ex As Exception
            Return New OperationResult(False, $"Erro ao excluir compromisso: {ex.Message}")
        End Try
    End Function

    Private Function ValidateAppointmentInputs(title As String, description As String, dateTime As DateTime) As OperationResult
        If String.IsNullOrWhiteSpace(title) Then
            Return New OperationResult(False, "Título do compromisso não pode ser vazio.")
        End If

        If dateTime < DateTime.Now Then
            Return New OperationResult(False, "Data do compromisso não pode ser no passado.")
        End If

        Return New OperationResult(True)
    End Function

    Public Sub Dispose() Implements IDisposable.Dispose
        If _dbContext IsNot Nothing Then
            _dbContext.Dispose()
            _dbContext = Nothing
        End If
    End Sub
End Class