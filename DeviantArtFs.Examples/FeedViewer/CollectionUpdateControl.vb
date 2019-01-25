Public Class CollectionUpdateControl
    Private ReadOnly collectionUrl As String

    Public Sub New(item As IBclDeviantArtFeedItem)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Dim collection = item.Collection
        collectionUrl = collection.Url
        Label1.Text = $"{item.ByUser.Username} added to collection {collection.Name}"

    End Sub

    Private Sub Label1_Click(sender As Object, e As EventArgs) Handles Label1.Click
        Process.Start(collectionUrl)
    End Sub
End Class
