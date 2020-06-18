<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SketchBox
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
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(SketchBox))
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel
        Me.TracingLink = New System.Windows.Forms.LinkLabel
        Me.TileViewLink = New System.Windows.Forms.LinkLabel
        Me.SizeLabel = New System.Windows.Forms.Label
        Me.LocationLabel = New System.Windows.Forms.Label
        Me.FlowLayoutPanel1 = New System.Windows.Forms.FlowLayoutPanel
        Me.Magnification = New System.Windows.Forms.TrackBar
        Me.FlowLayoutPanel4 = New System.Windows.Forms.FlowLayoutPanel
        Me.Label1 = New System.Windows.Forms.Label
        Me.AspectRatio = New System.Windows.Forms.NumericUpDown
        Me.FlowLayoutPanel2 = New System.Windows.Forms.FlowLayoutPanel
        Me.UndoButton = New System.Windows.Forms.Button
        Me.UndoImageList = New System.Windows.Forms.ImageList(Me.components)
        Me.RedoButton = New System.Windows.Forms.Button
        Me.RedoImageList = New System.Windows.Forms.ImageList(Me.components)
        Me.GuidelinesButton = New System.Windows.Forms.Button
        Me.GuideLineImageList = New System.Windows.Forms.ImageList(Me.components)
        Me.LassoButton = New System.Windows.Forms.Button
        Me.LassoImageList = New System.Windows.Forms.ImageList(Me.components)
        Me.MeasureImageList = New System.Windows.Forms.ImageList(Me.components)
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.SketchPanel = New Journal6.DBPanel
        Me.ColorScheme = New Journal6.ColorScheme
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.FlowLayoutPanel1.SuspendLayout()
        CType(Me.Magnification, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.FlowLayoutPanel4.SuspendLayout()
        CType(Me.AspectRatio, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.FlowLayoutPanel2.SuspendLayout()
        Me.SuspendLayout()
        '
        'SplitContainer1
        '
        Me.SplitContainer1.BackColor = System.Drawing.Color.Transparent
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2
        Me.SplitContainer1.IsSplitterFixed = True
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer1.Name = "SplitContainer1"
        Me.SplitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.AutoScroll = True
        Me.SplitContainer1.Panel1.BackColor = System.Drawing.Color.Ivory
        Me.SplitContainer1.Panel1.Controls.Add(Me.SketchPanel)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.TableLayoutPanel1)
        Me.SplitContainer1.Panel2.Controls.Add(Me.FlowLayoutPanel1)
        Me.SplitContainer1.Panel2.Controls.Add(Me.FlowLayoutPanel2)
        Me.SplitContainer1.Panel2.Margin = New System.Windows.Forms.Padding(3)
        Me.SplitContainer1.Panel2.Padding = New System.Windows.Forms.Padding(3)
        Me.SplitContainer1.Size = New System.Drawing.Size(750, 550)
        Me.SplitContainer1.SplitterDistance = 494
        Me.SplitContainer1.TabIndex = 57
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.BackColor = System.Drawing.Color.Ivory
        Me.TableLayoutPanel1.ColumnCount = 2
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 47.5!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 52.5!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.TracingLink, 0, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.TileViewLink, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.SizeLabel, 1, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.LocationLabel, 1, 1)
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(511, 3)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 2
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(236, 46)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'TracingLink
        '
        Me.TracingLink.ActiveLinkColor = System.Drawing.Color.SaddleBrown
        Me.TracingLink.AutoSize = True
        Me.TracingLink.BackColor = System.Drawing.Color.Ivory
        Me.TracingLink.DisabledLinkColor = System.Drawing.Color.Peru
        Me.TracingLink.Dock = System.Windows.Forms.DockStyle.Left
        Me.TracingLink.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TracingLink.LinkColor = System.Drawing.Color.SaddleBrown
        Me.TracingLink.Location = New System.Drawing.Point(3, 23)
        Me.TracingLink.Name = "TracingLink"
        Me.TracingLink.Size = New System.Drawing.Size(101, 23)
        Me.TracingLink.TabIndex = 67
        Me.TracingLink.TabStop = True
        Me.TracingLink.Text = "Tracing Paper"
        Me.TracingLink.VisitedLinkColor = System.Drawing.Color.SaddleBrown
        '
        'TileViewLink
        '
        Me.TileViewLink.ActiveLinkColor = System.Drawing.Color.SaddleBrown
        Me.TileViewLink.AutoSize = True
        Me.TileViewLink.BackColor = System.Drawing.Color.Ivory
        Me.TileViewLink.DisabledLinkColor = System.Drawing.Color.Peru
        Me.TileViewLink.Dock = System.Windows.Forms.DockStyle.Left
        Me.TileViewLink.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TileViewLink.LinkColor = System.Drawing.Color.SaddleBrown
        Me.TileViewLink.Location = New System.Drawing.Point(3, 0)
        Me.TileViewLink.Name = "TileViewLink"
        Me.TileViewLink.Size = New System.Drawing.Size(67, 23)
        Me.TileViewLink.TabIndex = 66
        Me.TileViewLink.TabStop = True
        Me.TileViewLink.Text = "Tile View"
        Me.TileViewLink.VisitedLinkColor = System.Drawing.Color.SaddleBrown
        '
        'SizeLabel
        '
        Me.SizeLabel.AutoSize = True
        Me.SizeLabel.Dock = System.Windows.Forms.DockStyle.Right
        Me.SizeLabel.Font = New System.Drawing.Font("Times New Roman", 10.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Italic), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SizeLabel.ForeColor = System.Drawing.Color.SaddleBrown
        Me.SizeLabel.Location = New System.Drawing.Point(162, 0)
        Me.SizeLabel.Name = "SizeLabel"
        Me.SizeLabel.Size = New System.Drawing.Size(71, 23)
        Me.SizeLabel.TabIndex = 61
        Me.SizeLabel.Text = "nnn x nnn"
        Me.SizeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'LocationLabel
        '
        Me.LocationLabel.AutoSize = True
        Me.LocationLabel.Dock = System.Windows.Forms.DockStyle.Top
        Me.LocationLabel.Font = New System.Drawing.Font("Times New Roman", 10.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Italic), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LocationLabel.ForeColor = System.Drawing.Color.SaddleBrown
        Me.LocationLabel.Location = New System.Drawing.Point(115, 23)
        Me.LocationLabel.Name = "LocationLabel"
        Me.LocationLabel.Size = New System.Drawing.Size(118, 17)
        Me.LocationLabel.TabIndex = 64
        Me.LocationLabel.Text = "nnn , nnn"
        Me.LocationLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'FlowLayoutPanel1
        '
        Me.FlowLayoutPanel1.BackColor = System.Drawing.Color.Ivory
        Me.FlowLayoutPanel1.Controls.Add(Me.ColorScheme)
        Me.FlowLayoutPanel1.Controls.Add(Me.Magnification)
        Me.FlowLayoutPanel1.Controls.Add(Me.FlowLayoutPanel4)
        Me.FlowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Left
        Me.FlowLayoutPanel1.Location = New System.Drawing.Point(195, 3)
        Me.FlowLayoutPanel1.Margin = New System.Windows.Forms.Padding(0)
        Me.FlowLayoutPanel1.Name = "FlowLayoutPanel1"
        Me.FlowLayoutPanel1.Size = New System.Drawing.Size(313, 46)
        Me.FlowLayoutPanel1.TabIndex = 62
        '
        'Magnification
        '
        Me.Magnification.BackColor = System.Drawing.Color.SaddleBrown
        Me.Magnification.LargeChange = 2
        Me.Magnification.Location = New System.Drawing.Point(152, 3)
        Me.Magnification.Maximum = 8
        Me.Magnification.Minimum = 1
        Me.Magnification.Name = "Magnification"
        Me.Magnification.Size = New System.Drawing.Size(95, 45)
        Me.Magnification.TabIndex = 60
        Me.Magnification.TickStyle = System.Windows.Forms.TickStyle.Both
        Me.ToolTip1.SetToolTip(Me.Magnification, "Magnifier")
        Me.Magnification.Value = 4
        '
        'FlowLayoutPanel4
        '
        Me.FlowLayoutPanel4.Controls.Add(Me.Label1)
        Me.FlowLayoutPanel4.Controls.Add(Me.AspectRatio)
        Me.FlowLayoutPanel4.Location = New System.Drawing.Point(253, 3)
        Me.FlowLayoutPanel4.Name = "FlowLayoutPanel4"
        Me.FlowLayoutPanel4.Size = New System.Drawing.Size(57, 40)
        Me.FlowLayoutPanel4.TabIndex = 63
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Times New Roman", 10.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Italic), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.SaddleBrown
        Me.Label1.Location = New System.Drawing.Point(3, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(50, 17)
        Me.Label1.TabIndex = 62
        Me.Label1.Text = "Aspect:"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'AspectRatio
        '
        Me.AspectRatio.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.AspectRatio.DecimalPlaces = 2
        Me.AspectRatio.Font = New System.Drawing.Font("Times New Roman", 8.25!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.AspectRatio.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.AspectRatio.Location = New System.Drawing.Point(3, 20)
        Me.AspectRatio.Minimum = New Decimal(New Integer() {1, 0, 0, 131072})
        Me.AspectRatio.Name = "AspectRatio"
        Me.AspectRatio.Size = New System.Drawing.Size(49, 20)
        Me.AspectRatio.TabIndex = 59
        Me.AspectRatio.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'FlowLayoutPanel2
        '
        Me.FlowLayoutPanel2.BackColor = System.Drawing.Color.Ivory
        Me.FlowLayoutPanel2.Controls.Add(Me.UndoButton)
        Me.FlowLayoutPanel2.Controls.Add(Me.RedoButton)
        Me.FlowLayoutPanel2.Controls.Add(Me.GuidelinesButton)
        Me.FlowLayoutPanel2.Controls.Add(Me.LassoButton)
        Me.FlowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Left
        Me.FlowLayoutPanel2.Location = New System.Drawing.Point(3, 3)
        Me.FlowLayoutPanel2.Name = "FlowLayoutPanel2"
        Me.FlowLayoutPanel2.Size = New System.Drawing.Size(192, 46)
        Me.FlowLayoutPanel2.TabIndex = 63
        '
        'UndoButton
        '
        Me.UndoButton.FlatAppearance.BorderColor = System.Drawing.Color.SaddleBrown
        Me.UndoButton.FlatAppearance.BorderSize = 0
        Me.UndoButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.UndoButton.ImageIndex = 0
        Me.UndoButton.ImageList = Me.UndoImageList
        Me.UndoButton.Location = New System.Drawing.Point(3, 3)
        Me.UndoButton.Name = "UndoButton"
        Me.UndoButton.Size = New System.Drawing.Size(42, 40)
        Me.UndoButton.TabIndex = 22
        Me.ToolTip1.SetToolTip(Me.UndoButton, "Undo")
        Me.UndoButton.UseVisualStyleBackColor = True
        '
        'UndoImageList
        '
        Me.UndoImageList.ImageStream = CType(resources.GetObject("UndoImageList.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.UndoImageList.TransparentColor = System.Drawing.Color.Transparent
        Me.UndoImageList.Images.SetKeyName(0, "Undo.bmp")
        Me.UndoImageList.Images.SetKeyName(1, "DimUndo.bmp")
        '
        'RedoButton
        '
        Me.RedoButton.FlatAppearance.BorderColor = System.Drawing.Color.SaddleBrown
        Me.RedoButton.FlatAppearance.BorderSize = 0
        Me.RedoButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.RedoButton.ImageIndex = 0
        Me.RedoButton.ImageList = Me.RedoImageList
        Me.RedoButton.Location = New System.Drawing.Point(51, 3)
        Me.RedoButton.Name = "RedoButton"
        Me.RedoButton.Size = New System.Drawing.Size(42, 40)
        Me.RedoButton.TabIndex = 23
        Me.ToolTip1.SetToolTip(Me.RedoButton, "Redo")
        Me.RedoButton.UseVisualStyleBackColor = True
        '
        'RedoImageList
        '
        Me.RedoImageList.ImageStream = CType(resources.GetObject("RedoImageList.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.RedoImageList.TransparentColor = System.Drawing.Color.Transparent
        Me.RedoImageList.Images.SetKeyName(0, "Redo.bmp")
        Me.RedoImageList.Images.SetKeyName(1, "DimRedo.bmp")
        '
        'GuidelinesButton
        '
        Me.GuidelinesButton.FlatAppearance.BorderColor = System.Drawing.Color.SaddleBrown
        Me.GuidelinesButton.FlatAppearance.BorderSize = 0
        Me.GuidelinesButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.GuidelinesButton.ImageIndex = 1
        Me.GuidelinesButton.ImageList = Me.GuideLineImageList
        Me.GuidelinesButton.Location = New System.Drawing.Point(99, 3)
        Me.GuidelinesButton.Name = "GuidelinesButton"
        Me.GuidelinesButton.Size = New System.Drawing.Size(42, 40)
        Me.GuidelinesButton.TabIndex = 24
        Me.ToolTip1.SetToolTip(Me.GuidelinesButton, "Display Grid")
        Me.GuidelinesButton.UseVisualStyleBackColor = True
        '
        'GuideLineImageList
        '
        Me.GuideLineImageList.ImageStream = CType(resources.GetObject("GuideLineImageList.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.GuideLineImageList.TransparentColor = System.Drawing.Color.Transparent
        Me.GuideLineImageList.Images.SetKeyName(0, "GuideLines.bmp")
        Me.GuideLineImageList.Images.SetKeyName(1, "DimGuideLines.bmp")
        '
        'LassoButton
        '
        Me.LassoButton.FlatAppearance.BorderColor = System.Drawing.Color.SaddleBrown
        Me.LassoButton.FlatAppearance.BorderSize = 0
        Me.LassoButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.LassoButton.ImageIndex = 1
        Me.LassoButton.ImageList = Me.LassoImageList
        Me.LassoButton.Location = New System.Drawing.Point(147, 3)
        Me.LassoButton.Name = "LassoButton"
        Me.LassoButton.Size = New System.Drawing.Size(42, 40)
        Me.LassoButton.TabIndex = 21
        Me.ToolTip1.SetToolTip(Me.LassoButton, "Set Stitch Pattern Area")
        Me.LassoButton.UseVisualStyleBackColor = True
        '
        'LassoImageList
        '
        Me.LassoImageList.ImageStream = CType(resources.GetObject("LassoImageList.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.LassoImageList.TransparentColor = System.Drawing.Color.Transparent
        Me.LassoImageList.Images.SetKeyName(0, "lasso.bmp")
        Me.LassoImageList.Images.SetKeyName(1, "DimLasso.bmp")
        '
        'MeasureImageList
        '
        Me.MeasureImageList.ImageStream = CType(resources.GetObject("MeasureImageList.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.MeasureImageList.TransparentColor = System.Drawing.Color.Transparent
        Me.MeasureImageList.Images.SetKeyName(0, "Measure.bmp")
        Me.MeasureImageList.Images.SetKeyName(1, "DimMeasure.bmp")
        '
        'SketchPanel
        '
        Me.SketchPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.SketchPanel.Cursor = System.Windows.Forms.Cursors.Cross
        Me.SketchPanel.Location = New System.Drawing.Point(0, 0)
        Me.SketchPanel.Name = "SketchPanel"
        Me.SketchPanel.Size = New System.Drawing.Size(720, 3996)
        Me.SketchPanel.TabIndex = 0
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
        Me.ColorScheme.ForeColor = System.Drawing.Color.Black
        Me.ColorScheme.Location = New System.Drawing.Point(3, 3)
        Me.ColorScheme.Name = "ColorScheme"
        Me.ColorScheme.Padding = New System.Windows.Forms.Padding(3)
        Me.ColorScheme.Size = New System.Drawing.Size(143, 40)
        Me.ColorScheme.TabIndex = 59
        Me.ColorScheme.TabStop = False
        Me.ToolTip1.SetToolTip(Me.ColorScheme, "Toggle Transparent/Solid")
        Me.ColorScheme.transparencies = New Boolean() {False, False, False, False}
        Me.ColorScheme.Transparent1 = False
        Me.ColorScheme.Transparent2 = False
        Me.ColorScheme.Transparent3 = False
        Me.ColorScheme.Transparent4 = False
        '
        'SketchBox
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.Controls.Add(Me.SplitContainer1)
        Me.Name = "SketchBox"
        Me.Size = New System.Drawing.Size(750, 550)
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        Me.SplitContainer1.ResumeLayout(False)
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TableLayoutPanel1.PerformLayout()
        Me.FlowLayoutPanel1.ResumeLayout(False)
        Me.FlowLayoutPanel1.PerformLayout()
        CType(Me.Magnification, System.ComponentModel.ISupportInitialize).EndInit()
        Me.FlowLayoutPanel4.ResumeLayout(False)
        Me.FlowLayoutPanel4.PerformLayout()
        CType(Me.AspectRatio, System.ComponentModel.ISupportInitialize).EndInit()
        Me.FlowLayoutPanel2.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents Magnification As System.Windows.Forms.TrackBar
    Friend WithEvents ColorScheme As Journal6.ColorScheme
    Friend WithEvents FlowLayoutPanel1 As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents AspectRatio As System.Windows.Forms.NumericUpDown
    Friend WithEvents SizeLabel As System.Windows.Forms.Label
    Friend WithEvents SketchPanel As Journal6.DBPanel
    Friend WithEvents RedoImageList As System.Windows.Forms.ImageList
    Friend WithEvents UndoImageList As System.Windows.Forms.ImageList
    Friend WithEvents GuideLineImageList As System.Windows.Forms.ImageList
    Friend WithEvents LassoImageList As System.Windows.Forms.ImageList
    Friend WithEvents FlowLayoutPanel2 As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents UndoButton As System.Windows.Forms.Button
    Friend WithEvents RedoButton As System.Windows.Forms.Button
    Friend WithEvents GuidelinesButton As System.Windows.Forms.Button
    Friend WithEvents LassoButton As System.Windows.Forms.Button
    Friend WithEvents LocationLabel As System.Windows.Forms.Label
    Friend WithEvents TileViewLink As System.Windows.Forms.LinkLabel
    Friend WithEvents MeasureImageList As System.Windows.Forms.ImageList
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents FlowLayoutPanel4 As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents TracingLink As System.Windows.Forms.LinkLabel

End Class
