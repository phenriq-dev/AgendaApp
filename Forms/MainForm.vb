Public Class MainForm
    Private _loggedInUser As User
    Private dgvAppointments As DataGridView
    Private btnAdd, btnEdit, btnDelete, btnLogout As Button
    Private _appointmentService As New AppointmentService

    Public Sub New(user As User)
        InitializeComponent()
        _loggedInUser = user
    End Sub

    Private Sub MainForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Text = "Agenda de Compromissos"
        Me.Size = New Size(650, 500)
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.BackColor = Color.FromArgb(240, 240, 240)

        Dim lblUserName As New Label With {
            .Text = $"Bem-vindo, {_loggedInUser.Name}",
            .Font = New Font("Segoe UI", 12, FontStyle.Bold),
            .ForeColor = Color.FromArgb(50, 50, 50),
            .Location = New Point(20, 20),
            .AutoSize = True
        }
        Me.Controls.Add(lblUserName)

        dgvAppointments = New DataGridView With {
            .Location = New Point(20, 60),
            .Size = New Size(600, 200),
            .ReadOnly = True,
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            .BackgroundColor = Color.White
        }
        Me.Controls.Add(dgvAppointments)

        btnAdd = New Button With {
            .Text = "Adicionar",
            .Size = New Size(120, 40),
            .Location = New Point(20, 280),
            .BackColor = Color.FromArgb(0, 122, 204),
            .ForeColor = Color.White,
            .FlatStyle = FlatStyle.Flat
        }
        AddHandler btnAdd.Click, AddressOf BtnAdd_Click
        Me.Controls.Add(btnAdd)

        btnEdit = New Button With {
            .Text = "Editar",
            .Size = New Size(120, 40),
            .Location = New Point(160, 280),
            .BackColor = Color.FromArgb(255, 165, 0),
            .ForeColor = Color.White,
            .FlatStyle = FlatStyle.Flat
        }
        AddHandler btnEdit.Click, AddressOf BtnEdit_Click
        Me.Controls.Add(btnEdit)

        btnDelete = New Button With {
            .Text = "Excluir",
            .Size = New Size(120, 40),
            .Location = New Point(300, 280),
            .BackColor = Color.FromArgb(220, 20, 60),
            .ForeColor = Color.White,
            .FlatStyle = FlatStyle.Flat
        }
        AddHandler btnDelete.Click, AddressOf BtnDelete_Click
        Me.Controls.Add(btnDelete)

        btnLogout = New Button With {
            .Text = "Logout",
            .Size = New Size(120, 40),
            .Location = New Point(460, 280),
            .BackColor = Color.FromArgb(34, 153, 84),
            .ForeColor = Color.White,
            .FlatStyle = FlatStyle.Flat
        }
        AddHandler btnLogout.Click, AddressOf BtnLogout_Click
        Me.Controls.Add(btnLogout)

        LoadAppointments()
    End Sub
    Private Sub LoadAppointments()
        ClearDataGridView()

        Dim result = GetAppointmentsFromService()

        If result.Success Then
            ConfigureDataGridView()
            BindAppointmentsToGrid(result.Data)
        Else
            HandleLoadingError(result.Message)
        End If
    End Sub

    Private Sub ClearDataGridView()
        dgvAppointments.DataSource = Nothing
        dgvAppointments.Columns.Clear()
    End Sub

    Private Function GetAppointmentsFromService() As OperationResult(Of List(Of AppointmentResult))
        Try
            Using service As New AppointmentService()
                Return service.GetAppointments(_loggedInUser.Id)
            End Using
        Catch ex As Exception
            Return New OperationResult(Of List(Of AppointmentResult))(
            False, $"Erro inesperado: {ex.Message}")
        End Try
    End Function

    Private Sub ConfigureDataGridView()
        ConfigureDataGridViewColumns()
    End Sub

    Private Sub BindAppointmentsToGrid(appointments As List(Of AppointmentResult))
        dgvAppointments.DataSource = appointments
    End Sub

    Private Sub HandleLoadingError(errorMessage As String)
        MessageBox.Show(errorMessage, "Erro ao carregar compromissos",
                   MessageBoxButtons.OK, MessageBoxIcon.Error)
        ClearDataGridView()
    End Sub

    Private Sub ConfigureDataGridViewColumns()
        dgvAppointments.AutoGenerateColumns = False

        dgvAppointments.Columns.Add(New DataGridViewTextBoxColumn() With {
        .DataPropertyName = "Title",
        .HeaderText = "Nome",
        .Width = 150,
        .DefaultCellStyle = New DataGridViewCellStyle With {
            .Font = New Font("Segoe UI", 9),
            .ForeColor = Color.FromArgb(64, 64, 64)
        }
    })

        dgvAppointments.Columns.Add(New DataGridViewTextBoxColumn() With {
        .DataPropertyName = "Description",
        .HeaderText = "Descrição",
        .Width = 200,
        .DefaultCellStyle = New DataGridViewCellStyle With {
            .Font = New Font("Segoe UI", 9),
            .ForeColor = Color.FromArgb(64, 64, 64)
        }
    })

        dgvAppointments.Columns.Add(New DataGridViewTextBoxColumn() With {
        .DataPropertyName = "DateAndTime",
        .HeaderText = "Data/Hora",
        .Width = 120,
        .DefaultCellStyle = New DataGridViewCellStyle With {
            .Format = "g",  ' Formato de data/hora curto
            .Font = New Font("Segoe UI", 9),
            .ForeColor = Color.FromArgb(64, 64, 64)
        }
    })

        dgvAppointments.Columns.Add(New DataGridViewTextBoxColumn() With {
        .DataPropertyName = "User",
        .HeaderText = "Usuário",
        .Width = 130,
        .DefaultCellStyle = New DataGridViewCellStyle With {
            .Font = New Font("Segoe UI", 9),
            .ForeColor = Color.FromArgb(64, 64, 64)
        }
    })

        dgvAppointments.Columns.Add(New DataGridViewTextBoxColumn() With {
        .DataPropertyName = "Id",
        .HeaderText = "ID",
        .Visible = False
    })

        dgvAppointments.ColumnHeadersDefaultCellStyle = New DataGridViewCellStyle With {
        .BackColor = Color.FromArgb(0, 122, 204),
        .ForeColor = Color.White,
        .Font = New Font("Segoe UI", 10, FontStyle.Bold),
        .Alignment = DataGridViewContentAlignment.MiddleLeft
    }

        dgvAppointments.AlternatingRowsDefaultCellStyle = New DataGridViewCellStyle With {
        .BackColor = Color.FromArgb(240, 240, 240)
    }
    End Sub

    Private Sub BtnAdd_Click(sender As Object, e As EventArgs)
        Using addForm As New AddEditAppointmentForm(_loggedInUser.Id)
            If addForm.ShowDialog() = DialogResult.OK Then
                dgvAppointments.DataSource = Nothing
                LoadAppointments()
                dgvAppointments.Refresh()
            End If
        End Using
    End Sub

    Private Sub BtnEdit_Click(sender As Object, e As EventArgs)
        If dgvAppointments.SelectedRows.Count = 0 Then
            MessageBox.Show("Selecione um compromisso para editar.", "Atenção",
                       MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim selectedRow = dgvAppointments.SelectedRows(0)
        Dim appointment = TryCast(selectedRow.DataBoundItem, AppointmentResult)

        If appointment Is Nothing Then
            MessageBox.Show("Dados do compromisso inválidos.", "Erro",
                       MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        Using editForm As New AddEditAppointmentForm(_loggedInUser.Id, appointment.Id)
            If editForm.ShowDialog() = DialogResult.OK Then
                dgvAppointments.DataSource = Nothing
                LoadAppointments()
                dgvAppointments.Refresh()
            End If
        End Using
    End Sub


    Private Sub BtnDelete_Click(sender As Object, e As EventArgs)
        If dgvAppointments.SelectedRows.Count = 0 Then
            MessageBox.Show("Selecione um compromisso para excluir.", "Atenção",
                       MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim selectedRow = dgvAppointments.SelectedRows(0)
        Dim appointment = TryCast(selectedRow.DataBoundItem, AppointmentResult)

        If appointment Is Nothing Then
            MessageBox.Show("Dados do compromisso inválidos.", "Erro",
                       MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        Dim confirmResult = MessageBox.Show("Tem certeza que deseja excluir este compromisso?",
                                      "Confirmação",
                                      MessageBoxButtons.YesNo,
                                      MessageBoxIcon.Warning)

        If confirmResult <> DialogResult.Yes Then
            Return
        End If

        Dim result = _appointmentService.DeleteAppointment(appointment.Id)

        If result.Success Then
            MessageBox.Show(result.Message, "Sucesso",
                           MessageBoxButtons.OK, MessageBoxIcon.Information)
            dgvAppointments.DataSource = Nothing
            LoadAppointments()
            dgvAppointments.Refresh()
        Else
            MessageBox.Show(result.Message, "Erro",
                           MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub

    Private Sub BtnLogout_Click(sender As Object, e As EventArgs)
        _loggedInUser = Nothing
        NavigateToLoginScreen()
    End Sub

    Private Sub NavigateToLoginScreen()
        Dim loginForm = New LoginForm()
        loginForm.Show()

        Me.Close()
    End Sub
End Class
