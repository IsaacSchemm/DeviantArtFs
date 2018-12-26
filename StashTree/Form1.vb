Imports DeviantArtFs
Imports DeviantArtFs.Stash.Marshal

Public Class Form1
    Private Token As IDeviantArtAccessToken = New AccessToken("00000000000000000000000000000000000000000000000000")
    Private User As UserResult = Nothing
    Private StashRoot As New StashRoot
    Private StashCursor As String = Nothing

    Private NodeToItem As New Dictionary(Of TreeNode, StashNode)

    Private Async Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If Token IsNot Nothing Then
            If Not Await Util.Placebo.IsValidAsync(Token) Then
                Token = Nothing
            End If
        End If

        If Token Is Nothing Then
            Dim clientIdStr = InputBox("Please enter the client ID (e.g. 1234)")
            Dim clientId = Integer.Parse(clientIdStr)
            Dim urlStr = InputBox("Please enter the redirect URL", DefaultResponse:="https://www.example.com")
            Dim url As New Uri(urlStr)

            Using form = New WinForms.DeviantArtImplicitGrantForm(clientId, url, {"stash"})
                If form.ShowDialog() = DialogResult.OK Then
                    Token = New AccessToken(form.AccessToken)
                    User = Nothing
                End If
            End Using
        End If

        If Token IsNot Nothing Then
            TextBox2.Text = Token.AccessToken

            If User Is Nothing Then
                User = Await DeviantArtFs.User.Whoami.ExecuteAsync(Token)
                TextBox1.Text = User.Username
                PictureBox2.ImageLocation = User.Usericon
            End If

            Dim delta = Await Stash.Delta.GetAllAsync(Token, New Stash.DeltaAllRequest With {.Cursor = StashCursor})

            StashCursor = delta.Cursor
            For Each entry In delta.Entries
                StashRoot.Apply(entry)
            Next
        End If

        TreeView1.Nodes.Clear()

        Dim rootNode = TreeView1.Nodes.Add("Root")
        AddNodes(rootNode, StashRoot.Children)
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        StashCursor = Nothing
        StashRoot.Clear()
        TreeView1.Nodes.Clear()
    End Sub

    Private Sub AddNodes(node As TreeNode, nodes As IEnumerable(Of StashNode))
        For Each n In nodes
            If TypeOf n Is StashStack And CheckBox1.Checked Then
                Dim s = CType(n, StashStack)
                If Not s.Children.Skip(1).Any() Then
                    AddNodes(node, s.Children)
                    Continue For
                End If
            End If

            Dim stackNode = node.Nodes.Add($"{n.Title} ({n.GetType().Name})")
            NodeToItem.Add(stackNode, n)
            If TypeOf n Is StashStack Then
                Dim s = CType(n, StashStack)
                AddNodes(stackNode, s.Children)
            End If
        Next
    End Sub

    Private Sub TreeView1_AfterSelect(sender As Object, e As TreeViewEventArgs) Handles TreeView1.AfterSelect
        Dim item = If(NodeToItem.ContainsKey(TreeView1.SelectedNode), NodeToItem(TreeView1.SelectedNode), Nothing)
        PropertyGrid1.SelectedObject = item
        If TypeOf item Is StashItem Then
            PictureBox1.ImageLocation = CType(item, StashItem).OriginalImageUrl
        Else
            PictureBox1.ImageLocation = Nothing
        End If
    End Sub
End Class

Public Class AccessToken
    Implements IDeviantArtAccessToken

    Public Sub New(token As String)
        AccessToken = token
    End Sub

    Public ReadOnly Property AccessToken As String Implements IDeviantArtAccessToken.AccessToken
End Class