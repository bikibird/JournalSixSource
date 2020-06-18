<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class StitchPatternBox
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
        Me.MotifPictureBox = New System.Windows.Forms.PictureBox
        Me.Magnification = New System.Windows.Forms.TrackBar
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer
        Me.Label1 = New System.Windows.Forms.Label
        Me.AspectRatio = New System.Windows.Forms.NumericUpDown
        Me.SizeLabel = New System.Windows.Forms.Label
        Me.ColorScheme = New Journal6.ColorScheme
        CType(Me.MotifPictureBox, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Magnification, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        CType(Me.AspectRatio, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'MotifPictureBox
        '
        Me.MotifPictureBox.BackColor = System.Drawing.Color.Ivory
        Me.MotifPictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.MotifPictureBox.Cursor = System.Windows.Forms.Cursors.Cross
        Me.MotifPictureBox.Location = New System.Drawing.Point(0, 0)
        Me.MotifPictureBox.Margin = New System.Windows.Forms.Padding(0)
        Me.MotifPictureBox.Name = "MotifPictureBox"
        Me.MotifPictureBox.Size = New System.Drawing.Size(600, 600)
        Me.MotifPictureBox.TabIndex = 40
        Me.MotifPictureBox.TabStop = False
        '
        'Magnification
        '
        Me.Magnification.BackColor = System.Drawing.Color.SaddleBrown
        Me.Magnification.Dock = System.Windows.Forms.DockStyle.Left
        Me.Magnification.LargeChange = 2
        Me.Magnification.Location = New System.Drawing.Point(106, 3)
        Me.Magnification.Maximum = 8
        Me.Magnification.Minimum = 1
        Me.Magnification.Name = "Magnification"
        Me.Magnification.Size = New System.Drawing.Size(81, 46)
        Me.Magnification.TabIndex = 52
        Me.Magnification.TickStyle = System.Windows.Forms.TickStyle.Both
        Me.Magnification.Value = 1
        '
        'SplitContainer1
        '
        Me.SplitContainer1.BackColor = System.Drawing.Color.Ivory
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer1.Name = "SplitContainer1"
        Me.SplitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.AutoScroll = True
        Me.SplitContainer1.Panel1.Controls.Add(Me.MotifPictureBox)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.Label1)
        Me.SplitContainer1.Panel2.Controls.Add(Me.AspectRatio)
        Me.SplitContainer1.Panel2.Controls.Add(Me.SizeLabel)
        Me.SplitContainer1.Panel2.Controls.Add(Me.Magnification)
        Me.SplitContainer1.Panel2.Controls.Add(Me.ColorScheme)
        Me.SplitContainer1.Panel2.Margin = New System.Windows.Forms.Padding(3)
        Me.SplitContainer1.Panel2.Padding = New System.Windows.Forms.Padding(3)
        Me.SplitContainer1.Size = New System.Drawing.Size(289, 254)
        Me.SplitContainer1.SplitterDistance = 198
        Me.SplitContainer1.TabIndex = 56
        '
        'Label1
        '
        Me.Label1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Times New Roman", 10.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Italic), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.SaddleBrown
        Me.Label1.Location = New System.Drawing.Point(187, 30)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(50, 17)
        Me.Label1.TabIndex = 57
        Me.Label1.Text = "Aspect:"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'AspectRatio
        '
        Me.AspectRatio.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.AspectRatio.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.AspectRatio.DecimalPlaces = 2
        Me.AspectRatio.Font = New System.Drawing.Font("Times New Roman", 8.25!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.AspectRatio.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.AspectRatio.Location = New System.Drawing.Point(237, 29)
        Me.AspectRatio.Minimum = New Decimal(New Integer() {1, 0, 0, 131072})
        Me.AspectRatio.Name = "AspectRatio"
        Me.AspectRatio.Size = New System.Drawing.Size(49, 20)
        Me.AspectRatio.TabIndex = 56
        Me.AspectRatio.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'SizeLabel
        '
        Me.SizeLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SizeLabel.AutoSize = True
        Me.SizeLabel.Font = New System.Drawing.Font("Times New Roman", 10.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Italic), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SizeLabel.ForeColor = System.Drawing.Color.SaddleBrown
        Me.SizeLabel.Location = New System.Drawing.Point(215, 3)
        Me.SizeLabel.Name = "SizeLabel"
        Me.SizeLabel.Size = New System.Drawing.Size(71, 17)
        Me.SizeLabel.TabIndex = 54
        Me.SizeLabel.Text = "nnn x nnn"
        Me.SizeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'ColorScheme
        '
        Me.ColorScheme.BackColor = System.Drawing.Color.Ivory
        Me.ColorScheme.Color1 = -16
        Me.ColorScheme.Color2 = -16
        Me.ColorScheme.Color3 = -16
        Me.ColorScheme.Color4 = -16
        Me.ColorScheme.Colors = New System.Drawing.Color() {System.Drawing.Color.Ivory, System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(240, Byte), Integer)), System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(240, Byte), Integer)), System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(240, Byte), Integer))}
        Me.ColorScheme.Count = CType(4, Byte)
        Me.ColorScheme.Dock = System.Windows.Forms.DockStyle.Left
        Me.ColorScheme.ForeColor = System.Drawing.Color.Black
        Me.ColorScheme.Location = New System.Drawing.Point(3, 3)
        Me.ColorScheme.Name = "ColorScheme"
        Me.ColorScheme.Padding = New System.Windows.Forms.Padding(3)
        Me.ColorScheme.Size = New System.Drawing.Size(103, 46)
        Me.ColorScheme.TabIndex = 53
        Me.ColorScheme.TabStop = False
        Me.ColorScheme.transparencies = New Boolean() {False, False, False, False}
        Me.ColorScheme.Transparent1 = False
        Me.ColorScheme.Transparent2 = False
        Me.ColorScheme.Transparent3 = False
        Me.ColorScheme.Transparent4 = False
        '
        'StitchPatternBox
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.Controls.Add(Me.SplitContainer1)
        Me.Margin = New System.Windows.Forms.Padding(0)
        Me.Name = "StitchPatternBox"
        Me.Size = New System.Drawing.Size(289, 254)
        CType(Me.MotifPictureBox, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Magnification, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        Me.SplitContainer1.Panel2.PerformLayout()
        Me.SplitContainer1.ResumeLayout(False)
        CType(Me.AspectRatio, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents MotifPictureBox As System.Windows.Forms.PictureBox
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents ColorScheme As Journal6.ColorScheme
    Friend WithEvents Magnification As System.Windows.Forms.TrackBar
    Friend WithEvents SizeLabel As System.Windows.Forms.Label
    Friend WithEvents AspectRatio As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label1 As System.Windows.Forms.Label

End Class
