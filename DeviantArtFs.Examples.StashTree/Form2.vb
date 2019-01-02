Imports System.IO

Public Class Form2
    Public Token As IDeviantArtAccessToken
    Private NextOffset As Integer? = Nothing

    Private NodeToItem As New Dictionary(Of TreeNode, Deviation)

    Private Async Sub UpdateDeviations(offset As Integer)
        Button1.Enabled = False
        Button2.Enabled = False

        If Token IsNot Nothing Then
            Dim page = Await Requests.Gallery.All.ExecuteAsync(Token, New Requests.Gallery.AllRequest With {.Offset = offset})
            For Each d In page.Results
                Dim deviation = New Deviation(d)
                Dim node = New TreeNode(If(deviation.Title, deviation.Deviationid.ToString()))
                NodeToItem.Add(node, deviation)
                TreeView1.Nodes.Add(node)
            Next

            NextOffset = page.GetNextOffset()
            Button2.Enabled = page.HasMore
        End If

        Button1.Enabled = True
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        TreeView1.Nodes.Clear()
        NodeToItem.Clear()
        UpdateDeviations(0)
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        UpdateDeviations(NextOffset)
    End Sub

    Private Sub TreeView1_AfterSelect(sender As Object, e As TreeViewEventArgs) Handles TreeView1.AfterSelect
        Dim item = If(NodeToItem.ContainsKey(TreeView1.SelectedNode), NodeToItem(TreeView1.SelectedNode), Nothing)
        PropertyGrid1.SelectedObject = item
        PictureBox1.ImageLocation = item.Content?.Src
    End Sub

    Private Async Sub WhoamiToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles WhoamiToolStripMenuItem.Click
        If Token IsNot Nothing Then
            Dim user = Await Requests.User.Whoami.ExecuteAsync(Token)
            MsgBox($"{user.Username} ({user.Type})")
        End If
    End Sub

    Private Async Sub GetToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles GetToolStripMenuItem.Click
        If Token IsNot Nothing Then
            TreeView1.Nodes.Clear()
            NodeToItem.Clear()

            Dim user = Await Requests.User.Whoami.ExecuteAsync(Token)
            Dim journals = Await Requests.Browse.UserJournals.ExecuteAsync(Token, New Requests.Browse.UserJournalsRequest(user.Username))

            For Each d In journals.Results
                Dim deviation = New Deviation(d)
                Dim node = New TreeNode(If(deviation.Title, deviation.Deviationid.ToString()))
                NodeToItem.Add(node, deviation)
                TreeView1.Nodes.Add(node)
            Next
        End If
    End Sub
End Class