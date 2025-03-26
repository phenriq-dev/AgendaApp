Public Class RegisterForm
    Private WithEvents txtName As New TextBox()
    Private WithEvents txtEmail As New TextBox()
    Private WithEvents txtPassword As New TextBox()
    Private WithEvents txtConfirmPassword As New TextBox()
    Private WithEvents btnRegister As New Button()
    Private WithEvents lblError As New Label()
    Private WithEvents lblName As New Label()
    Private WithEvents lblEmail As New Label()
    Private WithEvents lblPassword As New Label()
    Private WithEvents lblConfirmPassword As New Label()
    Private WithEvents lblTitle As New Label()

    Private ReadOnly _authService As New AuthService()

    Private Const MARGIN_HORIZONTAL As Integer = 50
    Private Const MARGIN_TOP As Integer = 20
    Private Const SPACING As Integer = 25
    Private Const CONTROL_WIDTH As Integer = 300
    Private Const LABEL_HEIGHT As Integer = 20
    Private Const TEXTBOX_HEIGHT As Integer = 30
    Private Const BUTTON_HEIGHT As Integer = 40

    Public Sub New()
        InitializeComponent()
    End Sub

    Private Sub RegisterForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SetupForm()
        SetupControls()
        PositionControls()
    End Sub

    Private Sub SetupForm()
        Me.Text = "Cadastro de Usuário"
        Me.Size = New Size(400, 500)
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.BackColor = Color.White
        Me.FormBorderStyle = FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
    End Sub

    Private Sub SetupControls()
        lblTitle.Text = "Cadastre-se"
        lblTitle.Font = New Font("Segoe UI", 18, FontStyle.Bold)
        lblTitle.ForeColor = Color.FromArgb(0, 122, 204)
        lblTitle.AutoSize = True

        Dim labelFont = New Font("Segoe UI", 10, FontStyle.Regular)
        lblName.Text = "Nome:"
        lblName.Font = labelFont
        lblEmail.Text = "Email:"
        lblEmail.Font = labelFont
        lblPassword.Text = "Senha:"
        lblPassword.Font = labelFont
        lblConfirmPassword.Text = "Confirmar Senha:"
        lblConfirmPassword.Font = labelFont

        Dim textBoxFont = New Font("Segoe UI", 10)
        txtName.Font = textBoxFont
        txtName.BorderStyle = BorderStyle.FixedSingle
        txtName.MaxLength = 100

        txtEmail.Font = textBoxFont
        txtEmail.BorderStyle = BorderStyle.FixedSingle
        txtEmail.MaxLength = 100

        txtPassword.Font = textBoxFont
        txtPassword.BorderStyle = BorderStyle.FixedSingle
        txtPassword.PasswordChar = "*"
        txtPassword.MaxLength = 50

        txtConfirmPassword.Font = textBoxFont
        txtConfirmPassword.BorderStyle = BorderStyle.FixedSingle
        txtConfirmPassword.PasswordChar = "*"
        txtConfirmPassword.MaxLength = 50

        btnRegister.Text = "Registrar"
        btnRegister.Font = New Font("Segoe UI", 12, FontStyle.Bold)
        btnRegister.BackColor = Color.FromArgb(0, 122, 204)
        btnRegister.ForeColor = Color.White
        btnRegister.FlatStyle = FlatStyle.Flat
        btnRegister.FlatAppearance.BorderSize = 0
        btnRegister.Cursor = Cursors.Hand

        lblError.Text = ""
        lblError.ForeColor = Color.Red
        lblError.Font = New Font("Segoe UI", 9, FontStyle.Regular)
        lblError.AutoSize = True
        lblError.Visible = False
    End Sub

    Private Sub PositionControls()
        Dim centerX As Integer = (Me.ClientSize.Width - CONTROL_WIDTH) \ 2
        Dim currentY As Integer = MARGIN_TOP

        lblTitle.Location = New Point((Me.ClientSize.Width - (lblTitle.Width * 1.5)) \ 2, currentY)
        Me.Controls.Add(lblTitle)
        currentY = lblTitle.Bottom + SPACING

        lblName.Location = New Point(centerX, currentY)
        lblName.Size = New Size(lblName.Size.Width, lblName.Size.Height - 5)
        Me.Controls.Add(lblName)

        txtName.Location = New Point(centerX, currentY + LABEL_HEIGHT)
        txtName.Size = New Size(CONTROL_WIDTH, TEXTBOX_HEIGHT)
        Me.Controls.Add(txtName)
        currentY = txtName.Bottom + SPACING

        lblEmail.Location = New Point(centerX, currentY)
        lblEmail.Size = New Size(lblEmail.Size.Width, lblEmail.Size.Height - 5)
        Me.Controls.Add(lblEmail)

        txtEmail.Location = New Point(centerX, currentY + LABEL_HEIGHT)
        txtEmail.Size = New Size(CONTROL_WIDTH, TEXTBOX_HEIGHT)
        Me.Controls.Add(txtEmail)
        currentY = txtEmail.Bottom + SPACING

        lblPassword.Location = New Point(centerX, currentY)
        lblPassword.Size = New Size(lblPassword.Size.Width, lblPassword.Size.Height - 5)
        Me.Controls.Add(lblPassword)

        txtPassword.Location = New Point(centerX, currentY + LABEL_HEIGHT)
        txtPassword.Size = New Size(CONTROL_WIDTH, TEXTBOX_HEIGHT)
        Me.Controls.Add(txtPassword)
        currentY = txtPassword.Bottom + SPACING

        lblConfirmPassword.Location = New Point(centerX, currentY)
        lblConfirmPassword.Size = New Size(lblConfirmPassword.Size.Width, lblConfirmPassword.Size.Height - 5)
        Me.Controls.Add(lblConfirmPassword)

        txtConfirmPassword.Location = New Point(centerX, currentY + LABEL_HEIGHT)
        txtConfirmPassword.Size = New Size(CONTROL_WIDTH, TEXTBOX_HEIGHT)
        Me.Controls.Add(txtConfirmPassword)
        currentY = txtConfirmPassword.Bottom + SPACING + 10

        btnRegister.Size = New Size(CONTROL_WIDTH, BUTTON_HEIGHT)
        btnRegister.Location = New Point(centerX, currentY)
        Me.Controls.Add(btnRegister)
        currentY = btnRegister.Bottom + SPACING - 10

        lblError.Location = New Point(centerX, currentY)
        Me.Controls.Add(lblError)

        AddHandler btnRegister.MouseEnter, AddressOf BtnRegister_MouseEnter
        AddHandler btnRegister.MouseLeave, AddressOf BtnRegister_MouseLeave
    End Sub

    Private Sub BtnRegister_MouseEnter(sender As Object, e As EventArgs)
        btnRegister.BackColor = Color.FromArgb(0, 102, 204)
    End Sub

    Private Sub BtnRegister_MouseLeave(sender As Object, e As EventArgs)
        btnRegister.BackColor = Color.FromArgb(0, 122, 204)
    End Sub

    Private Sub btnRegister_Click(sender As Object, e As EventArgs) Handles btnRegister.Click
        lblError.Visible = False

        If String.IsNullOrWhiteSpace(txtName.Text) Then
            ShowError("Por favor, informe seu nome.")
            Return
        End If

        If String.IsNullOrWhiteSpace(txtEmail.Text) OrElse Not IsValidEmail(txtEmail.Text) Then
            ShowError("Por favor, informe um email válido.")
            Return
        End If

        If String.IsNullOrWhiteSpace(txtPassword.Text) OrElse txtPassword.Text.Length < 6 Then
            ShowError("A senha deve ter pelo menos 6 caracteres.")
            Return
        End If

        If txtPassword.Text <> txtConfirmPassword.Text Then
            ShowError("As senhas não coincidem!")
            Return
        End If

        If _authService.CheckIfEmailExists(txtEmail.Text) Then
            ShowError("Este email já está cadastrado!")
            Return
        End If

        Dim user As New User() With {
            .Name = txtName.Text.Trim(),
            .Email = txtEmail.Text.Trim(),
            .PasswordHash = _authService.ComputeHash(txtPassword.Text.Trim())
        }
        Try
            Dim registrationResult = _authService.Register(user)

            If registrationResult.Success Then
                ShowSuccess("Cadastro realizado com sucesso!")
                NavigateToLoginScreen()
            Else
                ShowError(registrationResult.Message)
            End If
        Catch ex As Exception
            ShowError($"Erro no sistema: {ex.Message}")
        End Try
    End Sub

    Private Sub ShowSuccess(message As String)
        MessageBox.Show(message, "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub NavigateToLoginScreen()
        Dim loginForm As New LoginForm()
        loginForm.Show()
        Me.Close()
    End Sub

    Private Sub ShowError(message As String)
        lblError.Text = message
        lblError.Visible = True
    End Sub

    Private Function IsValidEmail(email As String) As Boolean
        Try
            Dim addr = New System.Net.Mail.MailAddress(email)
            Return addr.Address = email
        Catch
            Return False
        End Try
    End Function
End Class