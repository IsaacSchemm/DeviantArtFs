Imports System.IO
Imports DeviantArtFs.WinForms

Public Class Form1
    Private token As IDeviantArtAccessToken
    Private feedCursor As String

    Private Async Sub btnLogIn_Click(sender As Object, e As EventArgs) Handles btnLogIn.Click
        If File.Exists("token.txt") Then
            Dim str = File.ReadAllText("token.txt")
            Dim t As New AccessToken(str)
            If Await Requests.Util.Placebo.IsValidAsync(t) Then
                token = t
            End If
        End If

        If token Is Nothing Then
            Dim clientId = Integer.Parse(InputBox("Enter the client ID (example: 1234)", Text))
            Dim redirectUrl = New Uri(InputBox("Enter the redirect URL", Text, "https://www.example.com"))

            Using f As New DeviantArtImplicitGrantForm(clientId, redirectUrl, {"feed", "user"})
                If f.ShowDialog() = DialogResult.OK Then
                    File.WriteAllText("token.txt", f.AccessToken)
                    token = New AccessToken(f.AccessToken)
                    feedCursor = Nothing
                    FlowLayoutPanel1.Controls.Clear()

                    Dim user = Await Requests.User.Whoami.ExecuteAsync(token)
                    PictureBox1.ImageLocation = user.Usericon
                    lblUsername.Text = user.Username
                End If
            End Using
        End If

        btnLogIn.Enabled = False
        btnLogOut.Enabled = True

        btnMore.PerformClick()
    End Sub

    Private Async Sub btnLogOut_Click(sender As Object, e As EventArgs) Handles btnLogOut.Click
        Await DeviantArtAuth.RevokeAsync(token.AccessToken, True)
        token = Nothing

        btnLogIn.Enabled = True
        btnLogOut.Enabled = False
    End Sub

    Private Async Sub btnMore_Click(sender As Object, e As EventArgs) Handles btnMore.Click
        If token IsNot Nothing Then
            Dim feed = Await Requests.Feed.FeedHome.ExecuteAsync(token, feedCursor)
            feedCursor = feed.Cursor
            For Each item In feed.Items
                If item.Type = "deviation_submitted" Then
                    FlowLayoutPanel1.Controls.Add(New DeviationSubmittedControl(item))
                ElseIf item.Type = "status" Then
                    FlowLayoutPanel1.Controls.Add(New StatusControl(item))
                ElseIf item.Type = "collection_update" Then
                    FlowLayoutPanel1.Controls.Add(New CollectionUpdateControl(item))
                ElseIf item.Type = "journal_submitted" Then
                    FlowLayoutPanel1.Controls.Add(New JournalSubmittedControl(item))
                Else
                    ' ignore
                End If
            Next
            FlowLayoutPanel1.Focus()
        End If
    End Sub
End Class
