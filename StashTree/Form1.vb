Imports DeviantArtFs
Imports DeviantArtFs.Stash.DeltaMarshal

Public Class Form1
    Private Token As IDeviantArtAccessToken = New AccessToken("cffc550be7f98fec6e5b44bad2a30a3bc355c974c40a889a00")
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
                End If
            End Using
        End If

        If Token IsNot Nothing Then
            Dim delta = Await Stash.Delta.GetAllAsync(Token, New Stash.DeltaAllRequest With {.Cursor = StashCursor})

            StashCursor = delta.Cursor
            For Each entry In delta.Entries
                StashRoot.ApplyOrDefer(entry)
            Next
        End If

        StashRoot.ApplyDeferred()

        TreeView1.Nodes.Clear()

        Dim rootNode = TreeView1.Nodes.Add("Root")
        AddNodes(rootNode, StashRoot.Stacks)
    End Sub

    Private Sub AddNodes(node As TreeNode, nodes As IEnumerable(Of StashNode))
        For Each n In nodes
            Dim stackNode = node.Nodes.Add($"{n.Title} ({n.GetType().Name})")
            NodeToItem.Add(stackNode, n)
            If TypeOf n Is StashStack Then
                Dim s = CType(n, StashStack)
                AddNodes(stackNode, s.Nodes)
            End If
        Next
    End Sub

    Private Sub TreeView1_AfterSelect(sender As Object, e As TreeViewEventArgs) Handles TreeView1.AfterSelect
        PropertyGrid1.SelectedObject = If(NodeToItem.ContainsKey(TreeView1.SelectedNode), NodeToItem(TreeView1.SelectedNode), nothing)
    End Sub
End Class

Public Class AccessToken
    Implements IDeviantArtAccessToken

    Public Sub New(token As String)
        AccessToken = token
    End Sub

    Public ReadOnly Property AccessToken As String Implements IDeviantArtAccessToken.AccessToken
End Class