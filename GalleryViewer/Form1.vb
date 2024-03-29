﻿Imports System.IO
Imports DeviantArtFs
Imports DeviantArtFs.Extensions
Imports DeviantArtFs.ParameterTypes
Imports DeviantArtFs.ResponseTypes
Imports DeviantArtFs.WinForms

Public Class Form1
    Private Token As AccessToken = Nothing

    Private CurrentUsername As String = Nothing
    Private NextOffset As Integer? = Nothing

    Private Async Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If Token Is Nothing And File.Exists("refresh_token.txt") Then
            Dim t = AccessToken.ReadFrom("refresh_token.txt")

            Try
                Dim user = Await Api.User.WhoamiAsync(t)
                If MsgBox($"Log in as {user.username}?", MsgBoxStyle.OkCancel) = MsgBoxResult.Ok Then
                    Token = t
                End If
            Catch ex As InvalidRefreshTokenException
                MsgBox("Your credentials have expired.")
            End Try
        End If

        If Token Is Nothing Then
            Dim url As New Uri(DeviantArtRedirectUrl)

            Using form = New DeviantArtAuthorizationCodeForm(DeviantArtClientId, url, {"browse", "user", "stash"})
                If form.ShowDialog() = DialogResult.OK Then
                    Dim app = New DeviantArtApp(DeviantArtClientId, DeviantArtClientSecret)
                    Token = New AccessToken("refresh_token.txt", Await DeviantArtAuth.GetTokenAsync(app, form.Code, url))
                    Token.Write()
                End If
            End Using
        End If

        If Token IsNot Nothing Then
            Dim user = Await Api.User.WhoamiAsync(Token)
            ToolStripStatusLabel1.Text = $"Logged in as {user.username}"

            CurrentUsername = user.username
            NextOffset = 0
            Await LoadNextPage()
        End If
    End Sub

    Private Async Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Await LoadNextPage()
    End Sub

    Private Async Function LoadNextPage() As Task
        If Token Is Nothing Then
            Return
        End If

        TableLayoutPanel1.Controls.Clear()
        PictureBox1.ImageLocation = Nothing

        Dim page = Await Api.Gallery.PageAllViewAsync(
            Token,
            If(CurrentUsername Is Nothing, UserScope.ForCurrentUser, UserScope.NewForUser(CurrentUsername)),
            PagingLimit.NewPagingLimit(TableLayoutPanel1.ColumnCount * TableLayoutPanel1.RowCount),
            PagingOffset.NewPagingOffset(NextOffset))

        NextOffset = page.next_offset.OrNull()
        For Each r In page.results.OrEmpty()
            Dim thumbUrl = r.thumbs.OrEmpty().Select(Function(t) t.src).FirstOrDefault()
            Dim pic As New PictureBox With {.ImageLocation = thumbUrl, .SizeMode = PictureBoxSizeMode.Zoom, .Dock = DockStyle.Fill}
            AddHandler pic.Click, Sub(sender, e)
                                      ThumbnailClick(r)
                                  End Sub
            TableLayoutPanel1.Controls.Add(pic)
        Next
    End Function

    Private Async Sub ThumbnailClick(deviation As Deviation)
        PictureBox1.ImageLocation = Nothing

        If deviation.content.OrNull() IsNot Nothing Then
            PictureBox1.ImageLocation = deviation.content.OrNull().src
        Else
            Dim download = deviation.thumbs.OrEmpty().LastOrDefault()
            PictureBox1.ImageLocation = download.src
        End If

        WebBrowser1.Navigate("about:blank")
        Dim metadataResponse = Await Api.Deviation.GetMetadataAsync(Token, {deviation.deviationid})
        Dim s = metadataResponse.metadata.Single()
        WebBrowser1.Document.Write(s.description)
    End Sub
End Class
