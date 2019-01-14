Imports DeviantArtFs

Public Class Form2
    Public Token As IDeviantArtAccessToken
    Private NextOffset As Integer = 0

    Private NodeToItem As New Dictionary(Of TreeNode, Object)

    Private Async Sub UpdateDeviations(offset As Integer)
        Button1.Enabled = False
        Button2.Enabled = False

        If Token IsNot Nothing Then
            Dim page = Await Requests.Gallery.GalleryAllView.ExecuteAsync(Token,
                                                                          New PagingParams With {.Offset = NextOffset},
                                                                          New Requests.Gallery.GalleryAllViewRequest())
            For Each deviation In page.Results
                Dim node = New TreeNode(If(deviation.Title, deviation.Deviationid.ToString()))
                NodeToItem.Add(node, deviation)
                TreeView1.Nodes.Add(node)
            Next

            NextOffset = page.NextOffset
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

        If TypeOf item Is IBclDeviation Then
            PictureBox1.ImageLocation = CType(item, IBclDeviation).Content?.Src
        ElseIf TypeOf item Is Requests.User.FriendRecord Then
            PictureBox1.ImageLocation = CType(item, Requests.User.FriendRecord).User.Usericon
        ElseIf TypeOf item Is Requests.User.WatcherRecord Then
            PictureBox1.ImageLocation = CType(item, Requests.User.WatcherRecord).User.Usericon
        ElseIf TypeOf item Is IBclDeviantArtStatus Then
            PictureBox1.ImageLocation = CType(item, IBclDeviantArtStatus).EmbeddedDeviations.Select(Function(d) d.Content?.Src).FirstOrDefault()
        Else
            PictureBox1.ImageLocation = Nothing
        End If
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
            Dim journals = Await Requests.Browse.UserJournals.ToListAsync(Token, New Requests.Browse.UserJournalsRequest(user.Username))

            For Each deviation In journals
                Dim node = New TreeNode(If(deviation.Title, deviation.Deviationid.ToString()))
                NodeToItem.Add(node, deviation)
                TreeView1.Nodes.Add(node)
            Next

            Button2.Enabled = False
        End If
    End Sub

    Private Async Sub FriendsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles FriendsToolStripMenuItem.Click
        If Token IsNot Nothing Then
            TreeView1.Nodes.Clear()
            NodeToItem.Clear()

            Dim user = Await Requests.User.Whoami.ExecuteAsync(Token)
            Dim friends = Await Requests.User.Friends.ToListAsync(Token, New Requests.User.FriendsRequest With {.Username = user.Username})

            For Each f In friends
                Dim node = New TreeNode(f.User.Username)
                NodeToItem.Add(node, f)
                TreeView1.Nodes.Add(node)
            Next

            Button2.Enabled = False
        End If
    End Sub

    Private Async Sub WatchersToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles WatchersToolStripMenuItem.Click
        If Token IsNot Nothing Then
            TreeView1.Nodes.Clear()
            NodeToItem.Clear()

            Dim user = Await Requests.User.Whoami.ExecuteAsync(Token)
            Dim watchers = Await Requests.User.Watchers.ToListAsync(Token, New Requests.User.WatchersRequest With {.Username = user.Username})

            For Each w In watchers
                Dim node = New TreeNode(w.User.Username)
                NodeToItem.Add(node, w)
                TreeView1.Nodes.Add(node)
            Next

            Button2.Enabled = False
        End If
    End Sub

    Private Async Sub GetToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles GetToolStripMenuItem1.Click
        If Token IsNot Nothing Then
            TreeView1.Nodes.Clear()
            NodeToItem.Clear()

            Dim user = Await Requests.User.Whoami.ExecuteAsync(Token)
            Dim statuses = Await Requests.User.StatusesList.ToListAsync(Token, user.Username)

            For Each w In statuses
                Dim node = New TreeNode(w.Body)
                NodeToItem.Add(node, w)
                TreeView1.Nodes.Add(node)
            Next

            Button2.Enabled = False
        End If
    End Sub
End Class