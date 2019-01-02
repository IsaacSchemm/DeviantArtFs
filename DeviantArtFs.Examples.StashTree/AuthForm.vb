Imports System.IO

Public Class AuthForm
    Private Token As IDeviantArtAccessToken = Nothing

    Private Async Sub AuthForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If File.Exists("token.txt") Then
            Token = New AccessToken(File.ReadAllText("token.txt"))
            Await CheckToken()
        End If
    End Sub

    Private Async Function CheckToken() As Task
        If Token IsNot Nothing Then
            If Not Await Util.Placebo.IsValidAsync(Token) Then
                Token = Nothing
            End If
        End If

        If Token Is Nothing Then
            Dim clientIdStr = InputBox("Please enter the client ID (e.g. 1234)")
            Dim clientId = Integer.Parse(clientIdStr)
            Dim urlStr = InputBox("Please enter the redirect URL", DefaultResponse:="https://www.example.com")
            Dim url As New Uri(urlStr)

            Using form = New WinForms.DeviantArtImplicitGrantForm(clientId, url, {"stash"})
                If form.ShowDialog() = DialogResult.OK Then
                    File.WriteAllText("token.txt", form.AccessToken)
                    Token = New AccessToken(form.AccessToken)
                End If
            End Using
        End If

        If Token IsNot Nothing And TextBox1.Text = "" Then
            Dim user = Await DeviantArtFs.User.Whoami.ExecuteAsync(Token)
            TextBox1.Text = user.Username
            TextBox2.Text = Token.AccessToken
            PictureBox2.ImageLocation = user.Usericon
        End If
    End Function

    Private Async Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Await CheckToken()

        If Token IsNot Nothing Then
            Using f As New Form2
                f.Token = Token
                f.ShowDialog(Me)
            End Using
        End If
    End Sub

    Private Async Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Await CheckToken()

        If Token IsNot Nothing Then
            Using f As New Form1
                f.Token = Token
                f.ShowDialog(Me)
            End Using
        End If
    End Sub
End Class