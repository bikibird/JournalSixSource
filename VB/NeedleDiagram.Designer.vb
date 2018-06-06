<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class NeedleDiagram
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
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
        Me.DiagramPictureBox = New System.Windows.Forms.PictureBox
        Me.RackingTrackBar = New System.Windows.Forms.TrackBar
        Me.FlowLayoutPanel1 = New System.Windows.Forms.FlowLayoutPanel
        CType(Me.DiagramPictureBox, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RackingTrackBar, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.FlowLayoutPanel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'DiagramPictureBox
        '
        Me.DiagramPictureBox.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DiagramPictureBox.BackColor = System.Drawing.Color.White
        Me.DiagramPictureBox.Cursor = System.Windows.Forms.Cursors.Default
        Me.DiagramPictureBox.Location = New System.Drawing.Point(0, 0)
        Me.DiagramPictureBox.Margin = New System.Windows.Forms.Padding(0)
        Me.DiagramPictureBox.MaximumSize = New System.Drawing.Size(400, 100)
        Me.DiagramPictureBox.MinimumSize = New System.Drawing.Size(100, 100)
        Me.DiagramPictureBox.Name = "DiagramPictureBox"
        Me.DiagramPictureBox.Size = New System.Drawing.Size(317, 100)
        Me.DiagramPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.DiagramPictureBox.TabIndex = 1
        Me.DiagramPictureBox.TabStop = False
        '
        'RackingTrackBar
        '
        Me.RackingTrackBar.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.RackingTrackBar.AutoSize = False
        Me.RackingTrackBar.BackColor = System.Drawing.Color.SaddleBrown
        Me.RackingTrackBar.LargeChange = 4
        Me.RackingTrackBar.Location = New System.Drawing.Point(0, 100)
        Me.RackingTrackBar.Margin = New System.Windows.Forms.Padding(0)
        Me.RackingTrackBar.Maximum = 24
        Me.RackingTrackBar.MaximumSize = New System.Drawing.Size(400, 42)
        Me.RackingTrackBar.Minimum = -24
        Me.RackingTrackBar.MinimumSize = New System.Drawing.Size(100, 42)
        Me.RackingTrackBar.Name = "RackingTrackBar"
        Me.RackingTrackBar.Size = New System.Drawing.Size(317, 42)
        Me.RackingTrackBar.TabIndex = 2
        Me.RackingTrackBar.TickFrequency = 4
        Me.RackingTrackBar.TickStyle = System.Windows.Forms.TickStyle.Both
        '
        'FlowLayoutPanel1
        '
        Me.FlowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.FlowLayoutPanel1.BackColor = System.Drawing.Color.SaddleBrown
        Me.FlowLayoutPanel1.Controls.Add(Me.DiagramPictureBox)
        Me.FlowLayoutPanel1.Controls.Add(Me.RackingTrackBar)
        Me.FlowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.FlowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        Me.FlowLayoutPanel1.Location = New System.Drawing.Point(0, 0)
        Me.FlowLayoutPanel1.Margin = New System.Windows.Forms.Padding(0)
        Me.FlowLayoutPanel1.MaximumSize = New System.Drawing.Size(317, 142)
        Me.FlowLayoutPanel1.MinimumSize = New System.Drawing.Size(100, 100)
        Me.FlowLayoutPanel1.Name = "FlowLayoutPanel1"
        Me.FlowLayoutPanel1.Size = New System.Drawing.Size(317, 142)
        Me.FlowLayoutPanel1.TabIndex = 3
        '
        'NeedleDiagram
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.Controls.Add(Me.FlowLayoutPanel1)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.MaximumSize = New System.Drawing.Size(400, 200)
        Me.MinimumSize = New System.Drawing.Size(100, 100)
        Me.Name = "NeedleDiagram"
        Me.Size = New System.Drawing.Size(319, 145)
        CType(Me.DiagramPictureBox, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RackingTrackBar, System.ComponentModel.ISupportInitialize).EndInit()
        Me.FlowLayoutPanel1.ResumeLayout(False)
        Me.FlowLayoutPanel1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents DiagramPictureBox As System.Windows.Forms.PictureBox
    Friend WithEvents RackingTrackBar As System.Windows.Forms.TrackBar
    Friend WithEvents FlowLayoutPanel1 As System.Windows.Forms.FlowLayoutPanel

End Class
