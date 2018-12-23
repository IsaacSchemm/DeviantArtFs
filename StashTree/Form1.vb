Imports DeviantArtFs
Imports DeviantArtFs.Stash.DeltaMarshal

Public Class Form1
    Private Token As IDeviantArtAccessToken = New AccessToken("ddc1e78588ebb25ee034851d15c7c972b8daa6602d29bc810e")
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
            Dim offset = 0
            Do
                Dim req As New Stash.DeltaRequest With {
                    .Cursor = StashCursor,
                    .Offset = offset
                }
                Dim resp = Await Stash.Delta.ExecuteAsync(Token, req)

                For Each entry In resp.Entries
                    Try
                        StashRoot.Apply(entry)
                    Catch ex As StashDeltaApplyException
                        StashRoot.Defer(entry)
                    End Try
                Next

                StashCursor = resp.Cursor
                offset = If(resp.NextOffset, offset)

                If Not resp.HasMore Then
                    Exit Do
                End If
            Loop
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