Public Class AddEditAppointmentForm
    Private ReadOnly _userId As Integer
    Private ReadOnly _appointmentId As Integer?
    Private ReadOnly _appointmentService As New AppointmentService()

    Private WithEvents txtTitle As New TextBox()
    Private WithEvents txtDescription As New TextBox()
    Private WithEvents dtpDateTime As New DateTimePicker()
    Private WithEvents btnSave As New Button()
    Private WithEvents btnCancel As New Button()
    Private WithEvents lblTitle As New Label()
    Private WithEvents lblDescription As New Label()
    Private WithEvents lblDateTime As New Label()
    Private WithEvents lblHeader As New Label()

    Public Sub New(userId As Integer, Optional appointmentId As Integer? = Nothing)
        InitializeComponent()
        _userId = userId
        _appointmentId = appointmentId
    End Sub

    Private Sub AddEditAppointmentForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SetupForm()
        SetupControls()
        PositionControls()

        If _appointmentId.HasValue Then
            LoadAppointmentData()
        Else
            dtpDateTime.Value = DateTime.Now.AddHours(1) ' Valor padrão
        End If
    End Sub

    Private Sub SetupForm()
        Me.Text = If(_appointmentId.HasValue, "Editar Compromisso", "Novo Compromisso")
        Me.Size = New Size(450, 400)
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.BackColor = Color.White
        Me.FormBorderStyle = FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
    End Sub

    Private Sub SetupControls()
        lblHeader.Text = Me.Text
        lblHeader.Font = New Font("Segoe UI", 16, FontStyle.Bold)
        lblHeader.ForeColor = Color.FromArgb(0, 122, 204)
        lblHeader.AutoSize = True

        Dim labelFont = New Font("Segoe UI", 10, FontStyle.Regular)
        lblTitle.Text = "Título:"
        lblTitle.Font = labelFont
        lblDescription.Text = "Descrição:"
        lblDescription.Font = labelFont
        lblDateTime.Text = "Data/Hora:"
        lblDateTime.Font = labelFont

        txtTitle.Font = New Font("Segoe UI", 10)
        txtTitle.BorderStyle = BorderStyle.FixedSingle
        txtTitle.MaxLength = 100

        txtDescription.Font = New Font("Segoe UI", 10)
        txtDescription.BorderStyle = BorderStyle.FixedSingle
        txtDescription.Multiline = True
        txtDescription.ScrollBars = ScrollBars.Vertical
        txtDescription.MaxLength = 500

        dtpDateTime.Font = New Font("Segoe UI", 10)
        dtpDateTime.Format = DateTimePickerFormat.Custom
        dtpDateTime.CustomFormat = "dd/MM/yyyy HH:mm"
        dtpDateTime.ShowUpDown = True

        btnSave.Text = "Salvar"
        btnSave.Font = New Font("Segoe UI", 10, FontStyle.Bold)
        btnSave.BackColor = Color.FromArgb(0, 122, 204)
        btnSave.ForeColor = Color.White
        btnSave.FlatStyle = FlatStyle.Flat
        btnSave.FlatAppearance.BorderSize = 0
        btnSave.Cursor = Cursors.Hand

        btnCancel.Text = "Cancelar"
        btnCancel.Font = New Font("Segoe UI", 10)
        btnCancel.BackColor = Color.FromArgb(240, 240, 240)
        btnCancel.ForeColor = Color.FromArgb(64, 64, 64)
        btnCancel.FlatStyle = FlatStyle.Flat
        btnCancel.FlatAppearance.BorderSize = 1
        btnCancel.FlatAppearance.BorderColor = Color.FromArgb(200, 200, 200)
        btnCancel.Cursor = Cursors.Hand

        AddHandler btnSave.MouseEnter, Sub() btnSave.BackColor = Color.FromArgb(0, 102, 204)
        AddHandler btnSave.MouseLeave, Sub() btnSave.BackColor = Color.FromArgb(0, 122, 204)
        AddHandler btnCancel.MouseEnter, Sub() btnCancel.BackColor = Color.FromArgb(230, 230, 230)
        AddHandler btnCancel.MouseLeave, Sub() btnCancel.BackColor = Color.FromArgb(240, 240, 240)
    End Sub

    Private Sub PositionControls()
        Dim margin As Integer = 30
        Dim labelWidth As Integer = 80
        Dim controlWidth As Integer = Me.ClientSize.Width - (margin * 2) - labelWidth
        Dim verticalSpacing As Integer = 10

        lblHeader.Location = New Point(margin, margin)

        Dim currentY As Integer = lblHeader.Bottom + 20

        lblTitle.Location = New Point(margin, currentY)
        lblTitle.Size = New Size(lblTitle.Size.Width - 20, lblTitle.Size.Height)
        txtTitle.Location = New Point(margin + labelWidth, currentY)
        txtTitle.Size = New Size(controlWidth, 25)
        currentY += txtTitle.Height + verticalSpacing

        lblDescription.Location = New Point(margin, currentY)
        lblDescription.Size = New Size(lblDescription.Size.Width - 20, lblDescription.Size.Height)
        txtDescription.Location = New Point(margin + labelWidth, currentY)
        txtDescription.Size = New Size(controlWidth, 100)
        currentY += txtDescription.Height + verticalSpacing

        lblDateTime.Location = New Point(margin, currentY)
        lblDateTime.Size = New Size(lblDateTime.Size.Width - 20, lblDateTime.Size.Height)
        dtpDateTime.Location = New Point(margin + labelWidth, currentY)
        dtpDateTime.Size = New Size(controlWidth, 25)
        currentY += dtpDateTime.Height + 30

        Dim buttonWidth As Integer = 120
        Dim buttonHeight As Integer = 40
        Dim totalButtonWidth As Integer = (buttonWidth * 2) + 20

        btnSave.Location = New Point((Me.ClientSize.Width - totalButtonWidth) \ 2, currentY)
        btnSave.Size = New Size(buttonWidth, buttonHeight)

        btnCancel.Location = New Point(btnSave.Right + 20, currentY)
        btnCancel.Size = New Size(buttonWidth, buttonHeight)

        Me.Controls.AddRange({
            lblHeader, lblTitle, txtTitle, lblDescription, txtDescription,
            lblDateTime, dtpDateTime, btnSave, btnCancel
        })
    End Sub

    Private Sub LoadAppointmentData()
        Dim result = GetAppointmentFromService()

        If result.Success Then
            DisplayAppointmentData(result.Data)
        Else
            HandleLoadingError(result.Message)
        End If
    End Sub

    Private Function GetAppointmentFromService() As OperationResult(Of Appointment)
        Try
            Return _appointmentService.GetAppointmentById(_appointmentId.Value)
        Catch ex As Exception
            Return New OperationResult(Of Appointment)(False, $"Erro inesperado: {ex.Message}")
        End Try
    End Function

    Private Sub DisplayAppointmentData(appointment As Appointment)
        txtTitle.Text = appointment.Title
        txtDescription.Text = appointment.Description
        dtpDateTime.Value = appointment.DateTime
    End Sub

    Private Sub HandleLoadingError(errorMessage As String)
        MessageBox.Show(errorMessage, "Erro ao carregar compromisso",
                   MessageBoxButtons.OK, MessageBoxIcon.Error)
        Me.Close()
    End Sub

    Private Sub BtnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        If Not ValidateInputs() Then Return

        Try
            Dim result As OperationResult
            Dim title = txtTitle.Text.Trim()
            Dim description = txtDescription.Text.Trim()
            Dim dateTime = dtpDateTime.Value

            If _appointmentId.HasValue Then
                result = _appointmentService.EditAppointment(_userId, _appointmentId.Value, title, description, dateTime)
            Else
                result = _appointmentService.AddAppointment(_userId, title, description, dateTime)
            End If

            If result.Success Then
                MessageBox.Show(result.Message, "Sucesso",
                              MessageBoxButtons.OK, MessageBoxIcon.Information)
                Me.DialogResult = DialogResult.OK
                Me.Close()
            Else
                MessageBox.Show(result.Message, "Erro",
                              MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        Catch ex As Exception
            MessageBox.Show($"Erro inesperado: {ex.Message}", "Erro",
                          MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Function ValidateInputs() As Boolean
        If String.IsNullOrWhiteSpace(txtTitle.Text) Then
            MessageBox.Show("O título é obrigatório.", "Erro",
                          MessageBoxButtons.OK, MessageBoxIcon.Error)
            txtTitle.Focus()
            Return False
        End If

        If dtpDateTime.Value < DateTime.Now Then
            MessageBox.Show("A data/hora não pode ser no passado.", "Erro",
                          MessageBoxButtons.OK, MessageBoxIcon.Error)
            dtpDateTime.Focus()
            Return False
        End If

        Return True
    End Function

    Private Sub BtnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub
End Class