﻿Imports System.IO
Imports DeviantArtFs.WinForms

Public Class Form1
    Private Token As AccessToken = Nothing

    Private CurrentUsername As String = Nothing
    Private NextOffset As Integer? = Nothing

    Private Async Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If Token Is Nothing And File.Exists("refresh_token.txt") Then
            Dim t = AccessToken.ReadFrom("refresh_token.txt")

            Try
                Dim user = Await Requests.User.Whoami.ExecuteAsync(t)
                If MsgBox($"Log in as {user.Username}?", MsgBoxStyle.OkCancel) = MsgBoxResult.Ok Then
                    Token = t
                End If
            Catch ex As InvalidRefreshTokenException
                MsgBox("Your credentials have expired.")
            End Try
        End If

        If Token Is Nothing Then
            Dim url As New Uri(DeviantArtRedirectUrl)

            Using form = New DeviantArtAuthorizationCodeForm(DeviantArtClientId, url, {"browse", "user"})
                If form.ShowDialog() = DialogResult.OK Then
                    Dim auth = New DeviantArtAuth(DeviantArtClientId, DeviantArtClientSecret)
                    Token = New AccessToken("refresh_token.txt", Await auth.GetTokenAsync(form.Code, url))
                    Token.Write()
                End If
            End Using
        End If

        If Token IsNot Nothing Then
            Dim user = Await Requests.User.Whoami.ExecuteAsync(Token)
            ToolStripStatusLabel1.Text = $"Logged in as {user.Username}"

            CurrentUsername = TextBox1.Text
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

        Dim paging = New DeviantArtPagingParams With {.Offset = NextOffset, .Limit = TableLayoutPanel1.ColumnCount * TableLayoutPanel1.RowCount}
        Dim request As New Requests.Gallery.GalleryAllViewRequest With {.Username = CurrentUsername}
        Dim page = Await Requests.Gallery.GalleryAllView.ExecuteAsync(Token, paging, request)

        NextOffset = page.GetNextOffset()
        For Each r In page.results.Where(Function(d) Not d.is_deleted)
            Dim thumbUrl = r.GetThumbs().Select(Function(t) t.src).FirstOrDefault()
            Dim pic As New PictureBox With {.ImageLocation = thumbUrl, .SizeMode = PictureBoxSizeMode.Zoom, .Dock = DockStyle.Fill}
            AddHandler pic.Click, Sub(sender, e)
                                      ThumbnailClick(r)
                                  End Sub
            TableLayoutPanel1.Controls.Add(pic)
        Next
    End Function

    Private Async Sub ThumbnailClick(deviation As Deviation)
        PictureBox1.ImageLocation = Nothing
        Dim download As IDeviationFile
        Try
            download = Await Requests.Deviation.Download.ExecuteAsync(Token, deviation.deviationid)
        Catch ex As DeviantArtException
            download = Enumerable.Empty(Of IDeviationFile).Concat(deviation.GetContent()).Concat(deviation.GetThumbs()).FirstOrDefault()
        End Try
        PictureBox1.ImageLocation = If(download IsNot Nothing And download.Width * download.Height > 0, download.Src, Nothing)

        WebBrowser1.Navigate("about:blank")
        Dim req = New Requests.Deviation.MetadataRequest({deviation.deviationid})
        Dim metadata = Await Requests.Deviation.MetadataById.ExecuteAsync(Token, req)
        Dim s = metadata.Single()
        WebBrowser1.Document.Write(s.description)
    End Sub
End Class
