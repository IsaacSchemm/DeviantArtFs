Imports DeviantArtFs.Stash.Marshal

Public Class Form1
    Public Token As IDeviantArtAccessToken

    Private StashRoot As New StashRoot
    Private StashCursor As String = Nothing

    Private NodeToItem As New Dictionary(Of TreeNode, StashNode)

    Private Async Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Button1.Enabled = False

        If Token IsNot Nothing Then
            Dim list As New List(Of ISerializedStashDeltaEntry)

            Dim paging = New PagingParams With {.Offset = 0, .Limit = 120}

            While True
                Dim resp = Await Requests.Stash.Delta.ExecuteAsync(Token, paging, New Requests.Stash.DeltaRequest With {.Cursor = StashCursor})
                list.AddRange(resp.Entries)
                If resp.HasMore Then
                    paging.Offset = If(resp.NextOffset, 0)
                Else
                    StashCursor = resp.Cursor
                    Exit While
                End If
            End While

            For Each entry In list
                StashRoot.Apply(entry)
            Next

            RebuildTree("Root", StashRoot)
        End If

        Button1.Enabled = True
    End Sub

    Private Sub RebuildTree(name As String, root As StashRoot)
        TreeView1.Nodes.Clear()

        Dim rootNode = TreeView1.Nodes.Add(name)
        If CheckBox2.Checked Then
            AddNodes(rootNode, root.AllItems)
        Else
            AddNodes(rootNode, root.Children)
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        StashCursor = Nothing
        StashRoot.Clear()
        TreeView1.Nodes.Clear()
    End Sub

    Private Sub AddNodes(node As TreeNode, nodes As IEnumerable(Of StashNode))
        For Each n In nodes
            If CheckBox1.Checked Then
                If n.Children.Count() = 1 Then
                    AddNodes(node, n.Children)
                    Continue For
                End If
            End If

            Dim stackNode = node.Nodes.Add($"{n.BclMetadata.Title} ({n.GetType().Name})")
            NodeToItem.Add(stackNode, n)
            AddNodes(stackNode, n.Children)
        Next
    End Sub

    Private Sub TreeView1_AfterSelect(sender As Object, e As TreeViewEventArgs) Handles TreeView1.AfterSelect
        Dim item = If(NodeToItem.ContainsKey(TreeView1.SelectedNode), NodeToItem(TreeView1.SelectedNode), Nothing)
        PropertyGrid1.SelectedObject = item
        PropertyGrid2.SelectedObject = item?.BclMetadata
        If item IsNot Nothing Then
            If item.BclMetadata.Thumb IsNot Nothing Then
                PictureBox1.ImageLocation = item.BclMetadata.Thumb.Src
            Else
                PictureBox1.ImageLocation = item.BclMetadata.Files.Where(Function(f) f.Width * f.Height > 0).OrderByDescending(Function(f) f.Width * f.Height).Select(Function(f) f.Src).FirstOrDefault()
            End If
        End If
    End Sub

    Private Sub SaveToXMLToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SaveToXMLToolStripMenuItem.Click
        SerializationExample.Save(StashRoot.Save())
    End Sub

    Private Sub LoadFromXMLToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LoadFromXMLToolStripMenuItem.Click
        Dim list = SerializationExample.Load()

        If list IsNot Nothing Then
            Dim root = New StashRoot()
            For Each entry In list
                root.Apply(entry)
            Next

            RebuildTree("Deserialized from XML", root)
        End If
    End Sub

    Private Async Sub WhoamiToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles WhoamiToolStripMenuItem.Click
        If Token IsNot Nothing Then
            Dim user = Await Requests.User.Whoami.ExecuteAsync(Token)
            MsgBox($"{user.Username} ({user.Type})")
        End If
    End Sub

    Private Async Sub UserdataToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles UserdataToolStripMenuItem.Click
        If Token IsNot Nothing Then
            Dim userdata = Await Requests.Stash.PublishUserdata.ExecuteAsync(Token)
            MsgBox($"Agreements: {String.Join(", ", userdata.Agreements)}{vbCrLf}Features: {String.Join(", ", userdata.Features)}")
        End If
    End Sub

    Private Async Sub SpaceToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SpaceToolStripMenuItem.Click
        If Token IsNot Nothing Then
            Dim space = Await Requests.Stash.Space.ExecuteAsync(Token)
            MsgBox($"Available (MiB): {space.AvailableSpace / 1048576.0}{vbCrLf}Total (MiB): {space.TotalSpace / 1048576.0}")
        End If
    End Sub
End Class