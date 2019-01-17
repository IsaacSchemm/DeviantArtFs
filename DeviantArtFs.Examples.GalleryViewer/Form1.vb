Imports System.IO
Imports DeviantArtFs.WinForms

Public Class Form1
    Private Token As IDeviantArtAccessToken = Nothing

    Private CurrentUsername As String = Nothing
    Private NextOffset As Integer? = Nothing

    Private Async Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If Token Is Nothing Then
            If File.Exists("token.txt") Then
                Token = New AccessToken(File.ReadAllText("token.txt"))
            End If
        End If

        Await CheckToken()

        If Token IsNot Nothing Then
            CurrentUsername = TextBox1.Text
            NextOffset = 0
            Await LoadNextPage()
        End If
    End Sub

    Private Async Function CheckToken() As Task
        If Token IsNot Nothing Then
            If Not Await Requests.Util.Placebo.IsValidAsync(Token) Then
                Token = Nothing
            End If
        End If

        If Token Is Nothing Then
            Dim clientIdStr = InputBox("Please enter the client ID (e.g. 1234)")
            Dim clientId = Integer.Parse(clientIdStr)
            Dim urlStr = InputBox("Please enter the redirect URL", DefaultResponse:="https://www.example.com")
            Dim url As New Uri(urlStr)

            Using form = New DeviantArtImplicitGrantForm(clientId, url, {"stash"})
                If form.ShowDialog() = DialogResult.OK Then
                    File.WriteAllText("token.txt", form.AccessToken)
                    Token = New AccessToken(form.AccessToken)
                End If
            End Using
        End If

        If Token IsNot Nothing Then
            Dim user = Await Requests.User.Whoami.ExecuteAsync(Token)
            ToolStripStatusLabel1.Text = $"Logged in as {user.Username}"
        End If
    End Function

    Private Async Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Await LoadNextPage()
    End Sub

    Private Shared Async Function GetImage(url As String) As Task(Of Image)
        Dim req = Net.WebRequest.CreateHttp(url)
        req.UserAgent = "DeviantArtFs/0.0 GalleryViewer"
        Using resp = Await req.GetResponseAsync()
            Using s = resp.GetResponseStream()
                Return Image.FromStream(s)
            End Using
        End Using
    End Function

    Private Async Function LoadNextPage() As Task
        If Token Is Nothing Then
            Return
        End If

        TableLayoutPanel1.Controls.Clear()
        PictureBox1.ImageLocation = Nothing

        Dim paging = New PagingParams With {.Offset = NextOffset, .Limit = TableLayoutPanel1.ColumnCount * TableLayoutPanel1.RowCount}
        Dim request As New Requests.Gallery.GalleryAllViewRequest With {.Username = CurrentUsername}
        Dim page = Await Requests.Gallery.GalleryAllView.ExecuteAsync(Token, paging, request)

        NextOffset = page.NextOffset
        For Each r In page.Results
            Dim thumbUrl = r.Thumbs.Select(Function(t) t.Src).FirstOrDefault()
            Dim pic As New PictureBox With {.ImageLocation = thumbUrl, .SizeMode = PictureBoxSizeMode.Zoom, .Dock = DockStyle.Fill}
            AddHandler pic.Click, Sub(sender, e)
                                      ThumbnailClick(r)
                                  End Sub
            TableLayoutPanel1.Controls.Add(pic)
        Next
    End Function

    Private Async Sub ThumbnailClick(deviation As IBclDeviation)
        PictureBox1.ImageLocation = Nothing
        Dim download = Await Requests.Deviation.Download.ExecuteAsync(Token, deviation.Deviationid)
        PictureBox1.ImageLocation = If(download.Width * download.Height > 0, download.Src, Nothing)

        WebBrowser1.Navigate("about:blank")
        Dim req = New Requests.Deviation.MetadataRequest({deviation.Deviationid})
        Dim metadata = Await Requests.Deviation.MetadataById.ExecuteAsync(Token, req)
        Dim s = metadata.Single()
        WebBrowser1.Document.Write(s.Description)
    End Sub
End Class
