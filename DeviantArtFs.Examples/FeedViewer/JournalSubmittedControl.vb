Public Class JournalSubmittedControl
    Private ReadOnly journalUrl As String

    Public Sub New(item As IBclDeviantArtFeedItem)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Dim journal = item.Deviations.Single()
        journalUrl = journal.Url
        Label1.Text = journal.Author.Username
        Label2.Text = journal.Excerpt

    End Sub

    Private Sub Label2_Click(sender As Object, e As EventArgs) Handles Label2.Click
        Process.Start(journalUrl)
    End Sub
End Class
