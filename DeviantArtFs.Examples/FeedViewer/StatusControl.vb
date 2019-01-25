Public Class StatusControl
    Private ReadOnly statusUrl As String

    Public Sub New(item As IBclDeviantArtFeedItem)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Dim status = item.Status
        statusUrl = status.Url
        Label1.Text = status.Author.Username
        Label2.Text = status.Body

    End Sub

    Private Sub Label2_Click(sender As Object, e As EventArgs) Handles Label2.Click
        Process.Start(statusUrl)
    End Sub
End Class
