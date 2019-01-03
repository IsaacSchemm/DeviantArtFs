<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form2
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.WhoamiToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.UserInfoToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.FriendsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.WatchersToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.JournalsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.GetToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.PropertyGrid1 = New System.Windows.Forms.PropertyGrid()
        Me.SplitContainer3 = New System.Windows.Forms.SplitContainer()
        Me.TreeView1 = New System.Windows.Forms.TreeView()
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.StatusesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.GetToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.Panel1.SuspendLayout()
        Me.MenuStrip1.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SplitContainer3, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer3.Panel1.SuspendLayout()
        Me.SplitContainer3.Panel2.SuspendLayout()
        Me.SplitContainer3.SuspendLayout()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.SuspendLayout()
        '
        'WhoamiToolStripMenuItem
        '
        Me.WhoamiToolStripMenuItem.Name = "WhoamiToolStripMenuItem"
        Me.WhoamiToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.WhoamiToolStripMenuItem.Text = "&Whoami"
        '
        'UserInfoToolStripMenuItem
        '
        Me.UserInfoToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.WhoamiToolStripMenuItem, Me.FriendsToolStripMenuItem, Me.WatchersToolStripMenuItem})
        Me.UserInfoToolStripMenuItem.Name = "UserInfoToolStripMenuItem"
        Me.UserInfoToolStripMenuItem.Size = New System.Drawing.Size(64, 20)
        Me.UserInfoToolStripMenuItem.Text = "&User Info"
        '
        'FriendsToolStripMenuItem
        '
        Me.FriendsToolStripMenuItem.Name = "FriendsToolStripMenuItem"
        Me.FriendsToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.FriendsToolStripMenuItem.Text = "&Friends"
        '
        'WatchersToolStripMenuItem
        '
        Me.WatchersToolStripMenuItem.Name = "WatchersToolStripMenuItem"
        Me.WatchersToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.WatchersToolStripMenuItem.Text = "&Watchers"
        '
        'Button2
        '
        Me.Button2.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button2.Location = New System.Drawing.Point(513, 27)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(75, 20)
        Me.Button2.TabIndex = 6
        Me.Button2.Text = "Next >"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.Button2)
        Me.Panel1.Controls.Add(Me.Button1)
        Me.Panel1.Controls.Add(Me.MenuStrip1)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(591, 50)
        Me.Panel1.TabIndex = 0
        '
        'Button1
        '
        Me.Button1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button1.Location = New System.Drawing.Point(432, 27)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(75, 20)
        Me.Button1.TabIndex = 5
        Me.Button1.Text = "Update"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.UserInfoToolStripMenuItem, Me.JournalsToolStripMenuItem, Me.StatusesToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(591, 24)
        Me.MenuStrip1.TabIndex = 11
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'JournalsToolStripMenuItem
        '
        Me.JournalsToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.GetToolStripMenuItem})
        Me.JournalsToolStripMenuItem.Name = "JournalsToolStripMenuItem"
        Me.JournalsToolStripMenuItem.Size = New System.Drawing.Size(59, 20)
        Me.JournalsToolStripMenuItem.Text = "&Journals"
        '
        'GetToolStripMenuItem
        '
        Me.GetToolStripMenuItem.Name = "GetToolStripMenuItem"
        Me.GetToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.GetToolStripMenuItem.Text = "&Get"
        '
        'PictureBox1
        '
        Me.PictureBox1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PictureBox1.Location = New System.Drawing.Point(0, 0)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(303, 411)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.PictureBox1.TabIndex = 0
        Me.PictureBox1.TabStop = False
        '
        'PropertyGrid1
        '
        Me.PropertyGrid1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PropertyGrid1.Location = New System.Drawing.Point(0, 0)
        Me.PropertyGrid1.Name = "PropertyGrid1"
        Me.PropertyGrid1.Size = New System.Drawing.Size(284, 411)
        Me.PropertyGrid1.TabIndex = 1
        '
        'SplitContainer3
        '
        Me.SplitContainer3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer3.Location = New System.Drawing.Point(0, 50)
        Me.SplitContainer3.Name = "SplitContainer3"
        '
        'SplitContainer3.Panel1
        '
        Me.SplitContainer3.Panel1.Controls.Add(Me.PropertyGrid1)
        '
        'SplitContainer3.Panel2
        '
        Me.SplitContainer3.Panel2.Controls.Add(Me.PictureBox1)
        Me.SplitContainer3.Size = New System.Drawing.Size(591, 411)
        Me.SplitContainer3.SplitterDistance = 284
        Me.SplitContainer3.TabIndex = 0
        '
        'TreeView1
        '
        Me.TreeView1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TreeView1.Location = New System.Drawing.Point(0, 0)
        Me.TreeView1.Name = "TreeView1"
        Me.TreeView1.Size = New System.Drawing.Size(189, 461)
        Me.TreeView1.TabIndex = 0
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer1.Name = "SplitContainer1"
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.TreeView1)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.SplitContainer3)
        Me.SplitContainer1.Panel2.Controls.Add(Me.Panel1)
        Me.SplitContainer1.Size = New System.Drawing.Size(784, 461)
        Me.SplitContainer1.SplitterDistance = 189
        Me.SplitContainer1.TabIndex = 1
        '
        'StatusesToolStripMenuItem
        '
        Me.StatusesToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.GetToolStripMenuItem1})
        Me.StatusesToolStripMenuItem.Name = "StatusesToolStripMenuItem"
        Me.StatusesToolStripMenuItem.Size = New System.Drawing.Size(61, 20)
        Me.StatusesToolStripMenuItem.Text = "&Statuses"
        '
        'GetToolStripMenuItem1
        '
        Me.GetToolStripMenuItem1.Name = "GetToolStripMenuItem1"
        Me.GetToolStripMenuItem1.Size = New System.Drawing.Size(180, 22)
        Me.GetToolStripMenuItem1.Text = "&Get"
        '
        'Form2
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(784, 461)
        Me.Controls.Add(Me.SplitContainer1)
        Me.Name = "Form2"
        Me.Text = "Form2"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer3.Panel1.ResumeLayout(False)
        Me.SplitContainer3.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer3, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer3.ResumeLayout(False)
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents WhoamiToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents UserInfoToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents Button2 As Button
    Friend WithEvents Panel1 As Panel
    Friend WithEvents MenuStrip1 As MenuStrip
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents PropertyGrid1 As PropertyGrid
    Friend WithEvents SplitContainer3 As SplitContainer
    Friend WithEvents TreeView1 As TreeView
    Friend WithEvents SplitContainer1 As SplitContainer
    Friend WithEvents Button1 As Button
    Friend WithEvents JournalsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents GetToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents FriendsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents WatchersToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents StatusesToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents GetToolStripMenuItem1 As ToolStripMenuItem
End Class
