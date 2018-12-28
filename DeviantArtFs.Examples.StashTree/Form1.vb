Imports System.IO
Imports DeviantArtFs
Imports DeviantArtFs.Stash.Marshal

Public Class Form1
    Private Token As IDeviantArtAccessToken = Nothing
    Private User As UserResult = Nothing
    Private StashRoot As New StashRoot
    Private StashCursor As String = Nothing

    Private NodeToItem As New Dictionary(Of TreeNode, StashNode)

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If File.Exists("token.txt") Then
            Token = New AccessToken(File.ReadAllText("token.txt"))
        End If
    End Sub

    Private Async Function CheckToken() As Task
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
                    File.WriteAllText("token.txt", form.AccessToken)
                    Token = New AccessToken(form.AccessToken)
                    User = Nothing
                End If
            End Using
        End If
    End Function

    Private Async Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Button1.Enabled = False

        Await CheckToken()

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

    Private Async Sub LoadStackToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LoadStackToolStripMenuItem.Click
        Await CheckToken()

        Try
            Dim str = InputBox("Please enter the stack ID")
            If str = "" Then
                Return
            End If

            Dim stackId = Long.Parse(str)
            Dim stack = Await StashStack.GetStackAsync(Token, stackId)

            TreeView1.Nodes.Clear()
            Dim node = TreeView1.Nodes.Add(stack.Title)
            NodeToItem.Add(node, stack)
        Catch ex As DeviantArtException
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Async Sub LoadItemToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LoadItemToolStripMenuItem.Click
        Await CheckToken()

        Try
            Dim str = InputBox("Please enter the item ID")
            If str = "" Then
                Return
            End If

            Dim itemId = Long.Parse(str)
            Dim item = Await StashItem.GetItemAsync(Token, New Stash.ItemRequest(itemId))

            TreeView1.Nodes.Clear()
            Dim node = TreeView1.Nodes.Add(item.Title)
            NodeToItem.Add(node, item)
        Catch ex As DeviantArtException
            MsgBox(ex.Message)
        End Try
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
End Class

Public Class AccessToken
    Implements IDeviantArtAccessToken

    Public Sub New(token As String)
        AccessToken = token
    End Sub

    Public ReadOnly Property AccessToken As String Implements IDeviantArtAccessToken.AccessToken
End Class