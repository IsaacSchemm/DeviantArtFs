Public Class DeviationSubmittedControl
    Private ReadOnly deviationUrl As String

    Public Sub New(item As IBclDeviantArtFeedItem)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Dim deviation = item.Deviations.Single()
        deviationUrl = deviation.Url
        PictureBox1.ImageLocation = deviation.Thumbs.First().Src
        Label1.Text = deviation.Title
        Label2.Text = deviation.Author.Username

    End Sub

    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click
        Process.Start(deviationUrl)
    End Sub
End Class
