<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.TreeView1 = New System.Windows.Forms.TreeView()
        Me.SplitContainer3 = New System.Windows.Forms.SplitContainer()
        Me.PropertyGrid1 = New System.Windows.Forms.PropertyGrid()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.CheckBox2 = New System.Windows.Forms.CheckBox()
        Me.CheckBox1 = New System.Windows.Forms.CheckBox()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.SerializationToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SaveToXMLToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.LoadFromXMLToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.UserInfoToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.WhoamiToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.UserdataToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SpaceToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        CType(Me.SplitContainer3, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer3.Panel1.SuspendLayout()
        Me.SplitContainer3.Panel2.SuspendLayout()
        Me.SplitContainer3.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        Me.MenuStrip1.SuspendLayout()
        Me.SuspendLayout()
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
        Me.SplitContainer1.TabIndex = 0
        '
        'TreeView1
        '
        Me.TreeView1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TreeView1.Location = New System.Drawing.Point(0, 0)
        Me.TreeView1.Name = "TreeView1"
        Me.TreeView1.Size = New System.Drawing.Size(189, 461)
        Me.TreeView1.TabIndex = 0
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
        'PropertyGrid1
        '
        Me.PropertyGrid1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PropertyGrid1.Location = New System.Drawing.Point(0, 0)
        Me.PropertyGrid1.Name = "PropertyGrid1"
        Me.PropertyGrid1.Size = New System.Drawing.Size(284, 411)
        Me.PropertyGrid1.TabIndex = 1
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
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.CheckBox2)
        Me.Panel1.Controls.Add(Me.CheckBox1)
        Me.Panel1.Controls.Add(Me.Button2)
        Me.Panel1.Controls.Add(Me.Button1)
        Me.Panel1.Controls.Add(Me.MenuStrip1)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(591, 50)
        Me.Panel1.TabIndex = 0
        '
        'CheckBox2
        '
        Me.CheckBox2.AutoSize = True
        Me.CheckBox2.Location = New System.Drawing.Point(166, 30)
        Me.CheckBox2.Name = "CheckBox2"
        Me.CheckBox2.Size = New System.Drawing.Size(160, 17)
        Me.CheckBox2.TabIndex = 8
        Me.CheckBox2.Text = "Flatten all stacks (items only)"
        Me.CheckBox2.UseVisualStyleBackColor = True
        '
        'CheckBox1
        '
        Me.CheckBox1.AutoSize = True
        Me.CheckBox1.Checked = True
        Me.CheckBox1.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckBox1.Location = New System.Drawing.Point(3, 30)
        Me.CheckBox1.Name = "CheckBox1"
        Me.CheckBox1.Size = New System.Drawing.Size(157, 17)
        Me.CheckBox1.TabIndex = 4
        Me.CheckBox1.Text = "Flatten stacks with one item"
        Me.CheckBox1.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button2.Location = New System.Drawing.Point(513, 27)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(75, 20)
        Me.Button2.TabIndex = 6
        Me.Button2.Text = "Clear"
        Me.Button2.UseVisualStyleBackColor = True
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
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SerializationToolStripMenuItem, Me.UserInfoToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(591, 24)
        Me.MenuStrip1.TabIndex = 11
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'SerializationToolStripMenuItem
        '
        Me.SerializationToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SaveToXMLToolStripMenuItem, Me.LoadFromXMLToolStripMenuItem})
        Me.SerializationToolStripMenuItem.Name = "SerializationToolStripMenuItem"
        Me.SerializationToolStripMenuItem.Size = New System.Drawing.Size(76, 20)
        Me.SerializationToolStripMenuItem.Text = "&Serialization"
        '
        'SaveToXMLToolStripMenuItem
        '
        Me.SaveToXMLToolStripMenuItem.Name = "SaveToXMLToolStripMenuItem"
        Me.SaveToXMLToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.SaveToXMLToolStripMenuItem.Text = "&Save to XML"
        '
        'LoadFromXMLToolStripMenuItem
        '
        Me.LoadFromXMLToolStripMenuItem.Name = "LoadFromXMLToolStripMenuItem"
        Me.LoadFromXMLToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.LoadFromXMLToolStripMenuItem.Text = "&Load from XML"
        '
        'UserInfoToolStripMenuItem
        '
        Me.UserInfoToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.WhoamiToolStripMenuItem, Me.UserdataToolStripMenuItem, Me.SpaceToolStripMenuItem})
        Me.UserInfoToolStripMenuItem.Name = "UserInfoToolStripMenuItem"
        Me.UserInfoToolStripMenuItem.Size = New System.Drawing.Size(64, 20)
        Me.UserInfoToolStripMenuItem.Text = "&User Info"
        '
        'WhoamiToolStripMenuItem
        '
        Me.WhoamiToolStripMenuItem.Name = "WhoamiToolStripMenuItem"
        Me.WhoamiToolStripMenuItem.Size = New System.Drawing.Size(118, 22)
        Me.WhoamiToolStripMenuItem.Text = "&Whoami"
        '
        'UserdataToolStripMenuItem
        '
        Me.UserdataToolStripMenuItem.Name = "UserdataToolStripMenuItem"
        Me.UserdataToolStripMenuItem.Size = New System.Drawing.Size(118, 22)
        Me.UserdataToolStripMenuItem.Text = "&Userdata"
        '
        'SpaceToolStripMenuItem
        '
        Me.SpaceToolStripMenuItem.Name = "SpaceToolStripMenuItem"
        Me.SpaceToolStripMenuItem.Size = New System.Drawing.Size(118, 22)
        Me.SpaceToolStripMenuItem.Text = "&Space"
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(784, 461)
        Me.Controls.Add(Me.SplitContainer1)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "Form1"
        Me.Text = "Form1"
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        Me.SplitContainer3.Panel1.ResumeLayout(False)
        Me.SplitContainer3.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer3, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer3.ResumeLayout(False)
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents SplitContainer1 As SplitContainer
    Friend WithEvents TreeView1 As TreeView
    Friend WithEvents Button1 As Button
    Friend WithEvents PropertyGrid1 As PropertyGrid
    Friend WithEvents SplitContainer3 As SplitContainer
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents Panel1 As Panel
    Friend WithEvents CheckBox1 As CheckBox
    Friend WithEvents Button2 As Button
    Friend WithEvents CheckBox2 As CheckBox
    Friend WithEvents MenuStrip1 As MenuStrip
    Friend WithEvents SerializationToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents SaveToXMLToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents LoadFromXMLToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents UserInfoToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents WhoamiToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents UserdataToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents SpaceToolStripMenuItem As ToolStripMenuItem
End Class
