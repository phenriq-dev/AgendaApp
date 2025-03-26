Public Class LoginForm
    Private WithEvents txtEmail As New TextBox()
    Private WithEvents txtPassword As New TextBox()
    Private WithEvents btnLogin As New Button()
    Private WithEvents btnRegister As New Button()
    Private WithEvents lblError As New Label()

    Private ReadOnly _authService As New AuthService()

    Private Const FORM_WIDTH As Integer = 400
    Private Const FORM_HEIGHT As Integer = 400
    Private Const CONTROL_WIDTH As Integer = 200
    Private Const LABEL_HEIGHT As Integer = 30
    Private Const TEXTBOX_HEIGHT As Integer = 30
    Private Const BUTTON_WIDTH As Integer = 100
    Private Const BUTTON_HEIGHT As Integer = 40
    Private Const MARGIN_TOP As Integer = 50
    Private Const SPACING As Integer = 20

    Public Sub New()
        InitializeComponent()
    End Sub

    Private Sub LoginForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SetupForm()
        SetupControls()
        PositionControls()
    End Sub

    Private Sub SetupForm()
        Me.Text = "Login"
        Me.Size = New Size(FORM_WIDTH, FORM_HEIGHT)
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.BackColor = Color.FromArgb(250, 250, 250)
        Me.FormBorderStyle = FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
    End Sub

    Private Sub SetupControls()
        txtEmail.Font = New Font("Segoe UI", 10)
        txtEmail.BackColor = Color.White
        txtEmail.ForeColor = Color.Black
        txtEmail.BorderStyle = BorderStyle.FixedSingle

        txtPassword.Font = New Font("Segoe UI", 10)
        txtPassword.PasswordChar = "*"
        txtPassword.BackColor = Color.White
        txtPassword.ForeColor = Color.Black
        txtPassword.BorderStyle = BorderStyle.FixedSingle

        btnLogin.Text = "Entrar"
        btnLogin.BackColor = Color.FromArgb(0, 122, 204)
        btnLogin.ForeColor = Color.White
        btnLogin.FlatStyle = FlatStyle.Flat
        btnLogin.FlatAppearance.BorderSize = 0
        btnLogin.Font = New Font("Segoe UI", 12, FontStyle.Bold)
        btnLogin.Cursor = Cursors.Hand

        btnRegister.Text = "Registrar"
        btnRegister.BackColor = Color.FromArgb(34, 153, 84)
        btnRegister.ForeColor = Color.White
        btnRegister.FlatStyle = FlatStyle.Flat
        btnRegister.FlatAppearance.BorderSize = 0
        btnRegister.Font = New Font("Segoe UI", 12, FontStyle.Bold)
        btnRegister.Cursor = Cursors.Hand

        lblError.ForeColor = Color.Red
        lblError.Font = New Font("Segoe UI", 9)
        lblError.AutoSize = True

        AddHandler btnLogin.MouseEnter, AddressOf BtnLogin_MouseEnter
        AddHandler btnLogin.MouseLeave, AddressOf BtnLogin_MouseLeave
        AddHandler btnRegister.MouseEnter, AddressOf BtnRegister_MouseEnter
        AddHandler btnRegister.MouseLeave, AddressOf BtnRegister_MouseLeave
    End Sub

    Private Sub PositionControls()
        Dim centerX As Integer = (Me.ClientSize.Width - CONTROL_WIDTH) \ 2
        Dim currentY As Integer = MARGIN_TOP

        Dim lblEmail As New Label() With {
            .Text = "Email",
            .ForeColor = Color.FromArgb(0, 122, 204),
            .Font = New Font("Segoe UI", 12),
            .Size = New Size(CONTROL_WIDTH, LABEL_HEIGHT),
            .Location = New Point(centerX * 1.75, currentY)
        }
        Me.Controls.Add(lblEmail)
        currentY += lblEmail.Height + 5

        txtEmail.Location = New Point(centerX, currentY)
        txtEmail.Size = New Size(CONTROL_WIDTH, TEXTBOX_HEIGHT)
        Me.Controls.Add(txtEmail)
        currentY += txtEmail.Height + SPACING

        Dim lblPassword As New Label() With {
            .Text = "Senha",
            .ForeColor = Color.FromArgb(0, 122, 204),
            .Font = New Font("Segoe UI", 12),
            .Size = New Size(CONTROL_WIDTH, LABEL_HEIGHT),
            .Location = New Point(centerX * 1.75, currentY)
        }
        Me.Controls.Add(lblPassword)
        currentY += lblPassword.Height + 5

        txtPassword.Location = New Point(centerX, currentY)
        txtPassword.Size = New Size(CONTROL_WIDTH, TEXTBOX_HEIGHT)
        Me.Controls.Add(txtPassword)
        currentY += txtPassword.Height + SPACING + 10

        btnLogin.Size = New Size(BUTTON_WIDTH, BUTTON_HEIGHT)
        btnLogin.Location = New Point(centerX * 1.5, currentY)
        Me.Controls.Add(btnLogin)
        currentY += btnLogin.Height + SPACING

        btnRegister.Size = New Size(BUTTON_WIDTH, BUTTON_HEIGHT)
        btnRegister.Location = New Point(centerX * 1.5, currentY)
        Me.Controls.Add(btnRegister)
        currentY += btnRegister.Height + SPACING

        lblError.Location = New Point(centerX, currentY)
        Me.Controls.Add(lblError)
    End Sub

    Private Sub BtnLogin_MouseEnter(sender As Object, e As EventArgs)
        DirectCast(sender, Button).BackColor = Color.FromArgb(0, 102, 204)
    End Sub

    Private Sub BtnLogin_MouseLeave(sender As Object, e As EventArgs)
        DirectCast(sender, Button).BackColor = Color.FromArgb(0, 122, 204)
    End Sub

    Private Sub BtnRegister_MouseEnter(sender As Object, e As EventArgs)
        DirectCast(sender, Button).BackColor = Color.FromArgb(29, 133, 74)
    End Sub

    Private Sub BtnRegister_MouseLeave(sender As Object, e As EventArgs)
        DirectCast(sender, Button).BackColor = Color.FromArgb(34, 153, 84)
    End Sub

    Private Sub btnLogin_Click(sender As Object, e As EventArgs) Handles btnLogin.Click
        lblError.Visible = False
        txtEmail.BackColor = Color.White

        Dim email = txtEmail.Text.Trim()
        Dim password = txtPassword.Text.Trim()

        If String.IsNullOrEmpty(email) Then
            ShowError("Por favor, informe seu email.")
            Return
        End If

        If String.IsNullOrEmpty(password) Then
            ShowError("Por favor, informe sua senha.")
            Return
        End If

        If Not IsValidEmail(email) Then
            ShowError("Por favor, informe um email válido.")
            Return
        End If

        Try
            Dim result = _authService.Login(email, password)

            If result.Success Then
                MessageBox.Show("Login realizado com sucesso!", "Bem-vindo",
                          MessageBoxButtons.OK, MessageBoxIcon.Information)

                Dim mainForm As New MainForm(result.Data)
                mainForm.Show()
                Me.Hide()
            Else
                ShowError("Email ou senha incorretos. Tente novamente.")
                txtPassword.SelectAll()
                txtPassword.Focus()
            End If

        Catch ex As Exception
            ShowError("Ocorreu um erro ao tentar fazer login. Por favor, tente novamente.")
        End Try
    End Sub

    Private Sub btnRegister_Click(sender As Object, e As EventArgs) Handles btnRegister.Click
        Dim registerForm As New RegisterForm()
        registerForm.Show()
        Me.Hide()
    End Sub
    Private Function IsValidEmail(email As String) As Boolean
        Try
            Dim addr = New System.Net.Mail.MailAddress(email)
            Return addr.Address = email
        Catch
            Return False
        End Try
    End Function

    Private Sub ShowError(message As String)
        lblError.Text = message
        lblError.Visible = True
    End Sub
End Class