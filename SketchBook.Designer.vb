<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SketchBook
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
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(SketchBook))
        Me.BrushImageList = New System.Windows.Forms.ImageList(Me.components)
        Me.DropperImageList = New System.Windows.Forms.ImageList(Me.components)
        Me.LayoutImageList = New System.Windows.Forms.ImageList(Me.components)
        Me.ToolOptionsBox = New System.Windows.Forms.FlowLayoutPanel
        Me.LeftMousePaint = New Journal6.DBPanel
        Me.RightMousePaint = New Journal6.DBPanel
        Me.BrushAngle = New System.Windows.Forms.NumericUpDown
        Me.SymmetryButton = New System.Windows.Forms.Button
        Me.SymmetryImageList = New System.Windows.Forms.ImageList(Me.components)
        Me.LayoutButton = New System.Windows.Forms.Button
        Me.DropperButton = New System.Windows.Forms.Button
        Me.ToolBox = New System.Windows.Forms.FlowLayoutPanel
        Me.TapeMeasureButton = New System.Windows.Forms.Button
        Me.MeasureImageList = New System.Windows.Forms.ImageList(Me.components)
        Me.BrushButton = New System.Windows.Forms.Button
        Me.TextButton = New System.Windows.Forms.Button
        Me.TextImageList = New System.Windows.Forms.ImageList(Me.components)
        Me.VerticalButton = New System.Windows.Forms.Button
        Me.VerticalLineImageList = New System.Windows.Forms.ImageList(Me.components)
        Me.DiagonalButton = New System.Windows.Forms.Button
        Me.DiagonalLineImageList = New System.Windows.Forms.ImageList(Me.components)
        Me.HorizontalButton = New System.Windows.Forms.Button
        Me.HorizontalLineImageList = New System.Windows.Forms.ImageList(Me.components)
        Me.stampButton = New System.Windows.Forms.Button
        Me.StampImageList = New System.Windows.Forms.ImageList(Me.components)
        Me.CircleButton = New System.Windows.Forms.Button
        Me.CircleImageList = New System.Windows.Forms.ImageList(Me.components)
        Me.FilledCircleButton = New System.Windows.Forms.Button
        Me.FilledCircleImageList = New System.Windows.Forms.ImageList(Me.components)
        Me.RectangleButton = New System.Windows.Forms.Button
        Me.RectangleImageList = New System.Windows.Forms.ImageList(Me.components)
        Me.FilledRectangleButton = New System.Windows.Forms.Button
        Me.FilledRectangleImageList = New System.Windows.Forms.ImageList(Me.components)
        Me.FillButton = New System.Windows.Forms.Button
        Me.FillImageList = New System.Windows.Forms.ImageList(Me.components)
        Me.ResizeButton = New System.Windows.Forms.Button
        Me.ResizeImageList = New System.Windows.Forms.ImageList(Me.components)
        Me.MirrorButton = New System.Windows.Forms.Button
        Me.MirrorImageList = New System.Windows.Forms.ImageList(Me.components)
        Me.RotateLeftButton = New System.Windows.Forms.Button
        Me.RotateLeftImageList = New System.Windows.Forms.ImageList(Me.components)
        Me.RotateRightButton = New System.Windows.Forms.Button
        Me.RotateRightImageList = New System.Windows.Forms.ImageList(Me.components)
        Me.FlipButton = New System.Windows.Forms.Button
        Me.FlipImageList = New System.Windows.Forms.ImageList(Me.components)
        Me.LineThickness = New System.Windows.Forms.NumericUpDown
        Me.SketchBookCloseLink = New System.Windows.Forms.LinkLabel
        Me.PaintBox = New System.Windows.Forms.Panel
        Me.FontDialog1 = New System.Windows.Forms.FontDialog
        Me.FileSystemWatcher1 = New System.IO.FileSystemWatcher
        Me.TextBox1 = New System.Windows.Forms.TextBox
        Me.FontButton = New System.Windows.Forms.Button
        Me.CancelLink = New System.Windows.Forms.LinkLabel
        Me.SwatchRowsLabel = New System.Windows.Forms.Label
        Me.SwatchRowsTextBox = New System.Windows.Forms.NumericUpDown
        Me.SwatchStitchesLabel = New System.Windows.Forms.Label
        Me.SwatchStitchesTextBox = New System.Windows.Forms.NumericUpDown
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.PageDrawSketch = New System.Windows.Forms.TableLayoutPanel
        Me.SketchBox1 = New Journal6.SketchBox
        Me.Rowz = New System.Windows.Forms.NumericUpDown
        Me.RowzLabel = New System.Windows.Forms.Label
        Me.PageChooseStitchPattern = New System.Windows.Forms.TableLayoutPanel
        Me.ViewStitchPatPictureBox = New Journal6.StitchPatternBox
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.StitchPatListFilterButton = New System.Windows.Forms.Button
        Me.StitchPatListFilterLink = New System.Windows.Forms.LinkLabel
        Me.StitchPatListFilterText = New System.Windows.Forms.TextBox
        Me.StitchPatList = New System.Windows.Forms.ListBox
        Me.ReturnToSketch = New System.Windows.Forms.LinkLabel
        Me.UpdateFillPatternLink = New System.Windows.Forms.LinkLabel
        Me.ToolOptionsBox.SuspendLayout()
        CType(Me.BrushAngle, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ToolBox.SuspendLayout()
        CType(Me.LineThickness, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.FileSystemWatcher1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SwatchRowsTextBox, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SwatchStitchesTextBox, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        Me.PageDrawSketch.SuspendLayout()
        CType(Me.Rowz, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PageChooseStitchPattern.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.SuspendLayout()
        '
        'BrushImageList
        '
        Me.BrushImageList.ImageStream = CType(resources.GetObject("BrushImageList.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.BrushImageList.TransparentColor = System.Drawing.Color.Transparent
        Me.BrushImageList.Images.SetKeyName(0, "Brush.bmp")
        Me.BrushImageList.Images.SetKeyName(1, "DimBrush.bmp")
        '
        'DropperImageList
        '
        Me.DropperImageList.ImageStream = CType(resources.GetObject("DropperImageList.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.DropperImageList.TransparentColor = System.Drawing.Color.Transparent
        Me.DropperImageList.Images.SetKeyName(0, "Dropper.bmp")
        Me.DropperImageList.Images.SetKeyName(1, "DimDropper.bmp")
        '
        'LayoutImageList
        '
        Me.LayoutImageList.ImageStream = CType(resources.GetObject("LayoutImageList.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.LayoutImageList.TransparentColor = System.Drawing.Color.Transparent
        Me.LayoutImageList.Images.SetKeyName(0, "GridRepeat.bmp")
        Me.LayoutImageList.Images.SetKeyName(1, "DimGridRepeat.bmp")
        Me.LayoutImageList.Images.SetKeyName(2, "")
        Me.LayoutImageList.Images.SetKeyName(3, "enlarge.bmp")
        '
        'ToolOptionsBox
        '
        Me.ToolOptionsBox.BackColor = System.Drawing.Color.Ivory
        Me.ToolOptionsBox.Controls.Add(Me.LeftMousePaint)
        Me.ToolOptionsBox.Controls.Add(Me.RightMousePaint)
        Me.ToolOptionsBox.Controls.Add(Me.BrushAngle)
        Me.ToolOptionsBox.Controls.Add(Me.SymmetryButton)
        Me.ToolOptionsBox.Controls.Add(Me.LayoutButton)
        Me.ToolOptionsBox.Controls.Add(Me.DropperButton)
        Me.ToolOptionsBox.Location = New System.Drawing.Point(112, 3)
        Me.ToolOptionsBox.Name = "ToolOptionsBox"
        Me.ToolOptionsBox.Size = New System.Drawing.Size(99, 208)
        Me.ToolOptionsBox.TabIndex = 25
        '
        'LeftMousePaint
        '
        Me.LeftMousePaint.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.LeftMousePaint.Location = New System.Drawing.Point(3, 0)
        Me.LeftMousePaint.Margin = New System.Windows.Forms.Padding(3, 0, 3, 3)
        Me.LeftMousePaint.Name = "LeftMousePaint"
        Me.LeftMousePaint.Size = New System.Drawing.Size(42, 42)
        Me.LeftMousePaint.TabIndex = 25
        Me.ToolTip1.SetToolTip(Me.LeftMousePaint, "Left Mouse Button Color/Fill Pattern")
        '
        'RightMousePaint
        '
        Me.RightMousePaint.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.RightMousePaint.Location = New System.Drawing.Point(51, 0)
        Me.RightMousePaint.Margin = New System.Windows.Forms.Padding(3, 0, 3, 3)
        Me.RightMousePaint.Name = "RightMousePaint"
        Me.RightMousePaint.Size = New System.Drawing.Size(42, 42)
        Me.RightMousePaint.TabIndex = 26
        Me.ToolTip1.SetToolTip(Me.RightMousePaint, "Right Mouse Button Color Fill Pattern")
        '
        'BrushAngle
        '
        Me.BrushAngle.Location = New System.Drawing.Point(3, 48)
        Me.BrushAngle.Maximum = New Decimal(New Integer() {359, 0, 0, 0})
        Me.BrushAngle.Name = "BrushAngle"
        Me.BrushAngle.Size = New System.Drawing.Size(90, 26)
        Me.BrushAngle.TabIndex = 28
        Me.ToolTip1.SetToolTip(Me.BrushAngle, "Fill Pattern Rotation")
        Me.BrushAngle.Visible = False
        '
        'SymmetryButton
        '
        Me.SymmetryButton.FlatAppearance.BorderColor = System.Drawing.Color.SaddleBrown
        Me.SymmetryButton.FlatAppearance.BorderSize = 0
        Me.SymmetryButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.SymmetryButton.ImageIndex = 0
        Me.SymmetryButton.ImageList = Me.SymmetryImageList
        Me.SymmetryButton.Location = New System.Drawing.Point(3, 80)
        Me.SymmetryButton.Name = "SymmetryButton"
        Me.SymmetryButton.Padding = New System.Windows.Forms.Padding(5)
        Me.SymmetryButton.Size = New System.Drawing.Size(90, 80)
        Me.SymmetryButton.TabIndex = 27
        Me.SymmetryButton.Tag = "Grid"
        Me.ToolTip1.SetToolTip(Me.SymmetryButton, "Symmetry Mode")
        Me.SymmetryButton.UseVisualStyleBackColor = True
        '
        'SymmetryImageList
        '
        Me.SymmetryImageList.ImageStream = CType(resources.GetObject("SymmetryImageList.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.SymmetryImageList.TransparentColor = System.Drawing.Color.Transparent
        Me.SymmetryImageList.Images.SetKeyName(0, "Kaleido1.bmp")
        Me.SymmetryImageList.Images.SetKeyName(1, "KaleidoV.bmp")
        Me.SymmetryImageList.Images.SetKeyName(2, "KaleidoH.bmp")
        Me.SymmetryImageList.Images.SetKeyName(3, "rightDiagonal.bmp")
        Me.SymmetryImageList.Images.SetKeyName(4, "leftDiagonal.bmp")
        Me.SymmetryImageList.Images.SetKeyName(5, "Kaleido4.bmp")
        Me.SymmetryImageList.Images.SetKeyName(6, "Kaleido8bmp.bmp")
        Me.SymmetryImageList.Images.SetKeyName(7, "PinwheelV.bmp")
        Me.SymmetryImageList.Images.SetKeyName(8, "Pinwheel3.bmp")
        Me.SymmetryImageList.Images.SetKeyName(9, "Pinwheel4.bmp")
        Me.SymmetryImageList.Images.SetKeyName(10, "Pinwheel5.bmp")
        Me.SymmetryImageList.Images.SetKeyName(11, "Pinwheel6.bmp")
        Me.SymmetryImageList.Images.SetKeyName(12, "Pinwheel8.bmp")
        '
        'LayoutButton
        '
        Me.LayoutButton.FlatAppearance.BorderColor = System.Drawing.Color.SaddleBrown
        Me.LayoutButton.FlatAppearance.BorderSize = 0
        Me.LayoutButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.LayoutButton.ImageIndex = 0
        Me.LayoutButton.ImageList = Me.LayoutImageList
        Me.LayoutButton.Location = New System.Drawing.Point(3, 166)
        Me.LayoutButton.Name = "LayoutButton"
        Me.LayoutButton.Padding = New System.Windows.Forms.Padding(5)
        Me.LayoutButton.Size = New System.Drawing.Size(42, 40)
        Me.LayoutButton.TabIndex = 24
        Me.LayoutButton.Tag = "Grid"
        Me.ToolTip1.SetToolTip(Me.LayoutButton, "Fill Mode Grid/Brick/Half Drop/Resize")
        Me.LayoutButton.UseVisualStyleBackColor = True
        '
        'DropperButton
        '
        Me.DropperButton.FlatAppearance.BorderColor = System.Drawing.Color.SaddleBrown
        Me.DropperButton.FlatAppearance.BorderSize = 0
        Me.DropperButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.DropperButton.ImageIndex = 1
        Me.DropperButton.ImageList = Me.DropperImageList
        Me.DropperButton.Location = New System.Drawing.Point(51, 166)
        Me.DropperButton.Name = "DropperButton"
        Me.DropperButton.Size = New System.Drawing.Size(42, 40)
        Me.DropperButton.TabIndex = 0
        Me.DropperButton.Tag = "Dropper"
        Me.ToolTip1.SetToolTip(Me.DropperButton, "Fill Pattern Selector")
        Me.DropperButton.UseVisualStyleBackColor = True
        '
        'ToolBox
        '
        Me.ToolBox.BackColor = System.Drawing.Color.Ivory
        Me.ToolBox.Controls.Add(Me.TapeMeasureButton)
        Me.ToolBox.Controls.Add(Me.BrushButton)
        Me.ToolBox.Controls.Add(Me.TextButton)
        Me.ToolBox.Controls.Add(Me.VerticalButton)
        Me.ToolBox.Controls.Add(Me.DiagonalButton)
        Me.ToolBox.Controls.Add(Me.HorizontalButton)
        Me.ToolBox.Controls.Add(Me.stampButton)
        Me.ToolBox.Controls.Add(Me.CircleButton)
        Me.ToolBox.Controls.Add(Me.FilledCircleButton)
        Me.ToolBox.Controls.Add(Me.RectangleButton)
        Me.ToolBox.Controls.Add(Me.FilledRectangleButton)
        Me.ToolBox.Controls.Add(Me.FillButton)
        Me.ToolBox.Controls.Add(Me.ResizeButton)
        Me.ToolBox.Controls.Add(Me.MirrorButton)
        Me.ToolBox.Controls.Add(Me.RotateLeftButton)
        Me.ToolBox.Controls.Add(Me.RotateRightButton)
        Me.ToolBox.Controls.Add(Me.FlipButton)
        Me.ToolBox.Location = New System.Drawing.Point(6, 152)
        Me.ToolBox.Name = "ToolBox"
        Me.ToolBox.Size = New System.Drawing.Size(100, 376)
        Me.ToolBox.TabIndex = 26
        '
        'TapeMeasureButton
        '
        Me.TapeMeasureButton.FlatAppearance.BorderColor = System.Drawing.Color.SaddleBrown
        Me.TapeMeasureButton.FlatAppearance.BorderSize = 0
        Me.TapeMeasureButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.TapeMeasureButton.ImageIndex = 1
        Me.TapeMeasureButton.ImageList = Me.MeasureImageList
        Me.TapeMeasureButton.Location = New System.Drawing.Point(3, 3)
        Me.TapeMeasureButton.Name = "TapeMeasureButton"
        Me.TapeMeasureButton.Size = New System.Drawing.Size(42, 40)
        Me.TapeMeasureButton.TabIndex = 67
        Me.TapeMeasureButton.Tag = "Measure"
        Me.ToolTip1.SetToolTip(Me.TapeMeasureButton, "Tape Measure")
        Me.TapeMeasureButton.UseVisualStyleBackColor = True
        Me.TapeMeasureButton.Visible = False
        '
        'MeasureImageList
        '
        Me.MeasureImageList.ImageStream = CType(resources.GetObject("MeasureImageList.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.MeasureImageList.TransparentColor = System.Drawing.Color.Transparent
        Me.MeasureImageList.Images.SetKeyName(0, "Measure.bmp")
        Me.MeasureImageList.Images.SetKeyName(1, "DimMeasure.bmp")
        Me.MeasureImageList.Images.SetKeyName(2, "chalk.bmp")
        '
        'BrushButton
        '
        Me.BrushButton.FlatAppearance.BorderColor = System.Drawing.Color.SaddleBrown
        Me.BrushButton.FlatAppearance.BorderSize = 2
        Me.BrushButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BrushButton.ImageIndex = 0
        Me.BrushButton.ImageList = Me.BrushImageList
        Me.BrushButton.Location = New System.Drawing.Point(51, 3)
        Me.BrushButton.Name = "BrushButton"
        Me.BrushButton.Size = New System.Drawing.Size(42, 40)
        Me.BrushButton.TabIndex = 3
        Me.BrushButton.Tag = "Brush"
        Me.ToolTip1.SetToolTip(Me.BrushButton, "Paintbrush")
        Me.BrushButton.UseVisualStyleBackColor = True
        '
        'TextButton
        '
        Me.TextButton.FlatAppearance.BorderColor = System.Drawing.Color.SaddleBrown
        Me.TextButton.FlatAppearance.BorderSize = 0
        Me.TextButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.TextButton.ImageIndex = 1
        Me.TextButton.ImageList = Me.TextImageList
        Me.TextButton.Location = New System.Drawing.Point(3, 49)
        Me.TextButton.Name = "TextButton"
        Me.TextButton.Size = New System.Drawing.Size(42, 40)
        Me.TextButton.TabIndex = 18
        Me.TextButton.Tag = "Text"
        Me.ToolTip1.SetToolTip(Me.TextButton, "Text Tool")
        Me.TextButton.UseVisualStyleBackColor = True
        '
        'TextImageList
        '
        Me.TextImageList.ImageStream = CType(resources.GetObject("TextImageList.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.TextImageList.TransparentColor = System.Drawing.Color.Transparent
        Me.TextImageList.Images.SetKeyName(0, "FilledText.bmp")
        Me.TextImageList.Images.SetKeyName(1, "DimFilledText.bmp")
        Me.TextImageList.Images.SetKeyName(2, "CurvedText.bmp")
        Me.TextImageList.Images.SetKeyName(3, "slantedText.bmp")
        '
        'VerticalButton
        '
        Me.VerticalButton.FlatAppearance.BorderColor = System.Drawing.Color.SaddleBrown
        Me.VerticalButton.FlatAppearance.BorderSize = 0
        Me.VerticalButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.VerticalButton.ImageIndex = 1
        Me.VerticalButton.ImageList = Me.VerticalLineImageList
        Me.VerticalButton.Location = New System.Drawing.Point(51, 49)
        Me.VerticalButton.Name = "VerticalButton"
        Me.VerticalButton.Size = New System.Drawing.Size(42, 40)
        Me.VerticalButton.TabIndex = 20
        Me.VerticalButton.Tag = "Vertical"
        Me.ToolTip1.SetToolTip(Me.VerticalButton, "Vertical Line")
        Me.VerticalButton.UseVisualStyleBackColor = True
        '
        'VerticalLineImageList
        '
        Me.VerticalLineImageList.ImageStream = CType(resources.GetObject("VerticalLineImageList.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.VerticalLineImageList.TransparentColor = System.Drawing.Color.Transparent
        Me.VerticalLineImageList.Images.SetKeyName(0, "VerticalLine.bmp")
        Me.VerticalLineImageList.Images.SetKeyName(1, "DimVerticalLine.bmp")
        '
        'DiagonalButton
        '
        Me.DiagonalButton.FlatAppearance.BorderColor = System.Drawing.Color.SaddleBrown
        Me.DiagonalButton.FlatAppearance.BorderSize = 0
        Me.DiagonalButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.DiagonalButton.ImageIndex = 1
        Me.DiagonalButton.ImageList = Me.DiagonalLineImageList
        Me.DiagonalButton.Location = New System.Drawing.Point(3, 95)
        Me.DiagonalButton.Name = "DiagonalButton"
        Me.DiagonalButton.Size = New System.Drawing.Size(42, 40)
        Me.DiagonalButton.TabIndex = 6
        Me.DiagonalButton.Tag = "Line"
        Me.ToolTip1.SetToolTip(Me.DiagonalButton, "Diagonal Line")
        Me.DiagonalButton.UseVisualStyleBackColor = True
        '
        'DiagonalLineImageList
        '
        Me.DiagonalLineImageList.ImageStream = CType(resources.GetObject("DiagonalLineImageList.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.DiagonalLineImageList.TransparentColor = System.Drawing.Color.Transparent
        Me.DiagonalLineImageList.Images.SetKeyName(0, "DiagonalLine.bmp")
        Me.DiagonalLineImageList.Images.SetKeyName(1, "DimDiagonalLine.bmp")
        '
        'HorizontalButton
        '
        Me.HorizontalButton.FlatAppearance.BorderColor = System.Drawing.Color.SaddleBrown
        Me.HorizontalButton.FlatAppearance.BorderSize = 0
        Me.HorizontalButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.HorizontalButton.ImageIndex = 1
        Me.HorizontalButton.ImageList = Me.HorizontalLineImageList
        Me.HorizontalButton.Location = New System.Drawing.Point(51, 95)
        Me.HorizontalButton.Name = "HorizontalButton"
        Me.HorizontalButton.Size = New System.Drawing.Size(42, 40)
        Me.HorizontalButton.TabIndex = 13
        Me.HorizontalButton.Tag = "Horizontal"
        Me.ToolTip1.SetToolTip(Me.HorizontalButton, "Horizontal Line")
        Me.HorizontalButton.UseVisualStyleBackColor = True
        '
        'HorizontalLineImageList
        '
        Me.HorizontalLineImageList.ImageStream = CType(resources.GetObject("HorizontalLineImageList.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.HorizontalLineImageList.TransparentColor = System.Drawing.Color.Transparent
        Me.HorizontalLineImageList.Images.SetKeyName(0, "HorizontalLine.bmp")
        Me.HorizontalLineImageList.Images.SetKeyName(1, "DimHorizontalLine.bmp")
        '
        'stampButton
        '
        Me.stampButton.FlatAppearance.BorderColor = System.Drawing.Color.SaddleBrown
        Me.stampButton.FlatAppearance.BorderSize = 0
        Me.stampButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.stampButton.ImageIndex = 1
        Me.stampButton.ImageList = Me.StampImageList
        Me.stampButton.Location = New System.Drawing.Point(3, 141)
        Me.stampButton.Name = "stampButton"
        Me.stampButton.Size = New System.Drawing.Size(42, 40)
        Me.stampButton.TabIndex = 21
        Me.stampButton.Tag = "Stamp"
        Me.ToolTip1.SetToolTip(Me.stampButton, "Stamp")
        Me.stampButton.UseVisualStyleBackColor = True
        '
        'StampImageList
        '
        Me.StampImageList.ImageStream = CType(resources.GetObject("StampImageList.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.StampImageList.TransparentColor = System.Drawing.Color.Transparent
        Me.StampImageList.Images.SetKeyName(0, "stamp.bmp")
        Me.StampImageList.Images.SetKeyName(1, "DimStamp.bmp")
        '
        'CircleButton
        '
        Me.CircleButton.FlatAppearance.BorderColor = System.Drawing.Color.SaddleBrown
        Me.CircleButton.FlatAppearance.BorderSize = 0
        Me.CircleButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.CircleButton.ImageIndex = 1
        Me.CircleButton.ImageList = Me.CircleImageList
        Me.CircleButton.Location = New System.Drawing.Point(51, 141)
        Me.CircleButton.Name = "CircleButton"
        Me.CircleButton.Size = New System.Drawing.Size(42, 40)
        Me.CircleButton.TabIndex = 5
        Me.CircleButton.Tag = "Ellipse"
        Me.ToolTip1.SetToolTip(Me.CircleButton, "Circle/Ellipse")
        Me.CircleButton.UseVisualStyleBackColor = True
        '
        'CircleImageList
        '
        Me.CircleImageList.ImageStream = CType(resources.GetObject("CircleImageList.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.CircleImageList.TransparentColor = System.Drawing.Color.Transparent
        Me.CircleImageList.Images.SetKeyName(0, "circle.bmp")
        Me.CircleImageList.Images.SetKeyName(1, "DimCircle.bmp")
        '
        'FilledCircleButton
        '
        Me.FilledCircleButton.FlatAppearance.BorderColor = System.Drawing.Color.SaddleBrown
        Me.FilledCircleButton.FlatAppearance.BorderSize = 0
        Me.FilledCircleButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.FilledCircleButton.ImageIndex = 1
        Me.FilledCircleButton.ImageList = Me.FilledCircleImageList
        Me.FilledCircleButton.Location = New System.Drawing.Point(3, 187)
        Me.FilledCircleButton.Name = "FilledCircleButton"
        Me.FilledCircleButton.Size = New System.Drawing.Size(42, 40)
        Me.FilledCircleButton.TabIndex = 8
        Me.FilledCircleButton.Tag = "FilledEllipse"
        Me.ToolTip1.SetToolTip(Me.FilledCircleButton, "Filled Circle/Ellipse")
        Me.FilledCircleButton.UseVisualStyleBackColor = True
        '
        'FilledCircleImageList
        '
        Me.FilledCircleImageList.ImageStream = CType(resources.GetObject("FilledCircleImageList.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.FilledCircleImageList.TransparentColor = System.Drawing.Color.Transparent
        Me.FilledCircleImageList.Images.SetKeyName(0, "FilledCircle.bmp")
        Me.FilledCircleImageList.Images.SetKeyName(1, "DimFilledCircle.bmp")
        '
        'RectangleButton
        '
        Me.RectangleButton.FlatAppearance.BorderColor = System.Drawing.Color.SaddleBrown
        Me.RectangleButton.FlatAppearance.BorderSize = 0
        Me.RectangleButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.RectangleButton.ImageIndex = 1
        Me.RectangleButton.ImageList = Me.RectangleImageList
        Me.RectangleButton.Location = New System.Drawing.Point(51, 187)
        Me.RectangleButton.Name = "RectangleButton"
        Me.RectangleButton.Size = New System.Drawing.Size(42, 40)
        Me.RectangleButton.TabIndex = 19
        Me.RectangleButton.Tag = "Rectangle"
        Me.ToolTip1.SetToolTip(Me.RectangleButton, "Square/Rectangle")
        Me.RectangleButton.UseVisualStyleBackColor = True
        '
        'RectangleImageList
        '
        Me.RectangleImageList.ImageStream = CType(resources.GetObject("RectangleImageList.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.RectangleImageList.TransparentColor = System.Drawing.Color.Transparent
        Me.RectangleImageList.Images.SetKeyName(0, "square.bmp")
        Me.RectangleImageList.Images.SetKeyName(1, "DimSquare.bmp")
        '
        'FilledRectangleButton
        '
        Me.FilledRectangleButton.FlatAppearance.BorderColor = System.Drawing.Color.SaddleBrown
        Me.FilledRectangleButton.FlatAppearance.BorderSize = 0
        Me.FilledRectangleButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.FilledRectangleButton.ImageIndex = 1
        Me.FilledRectangleButton.ImageList = Me.FilledRectangleImageList
        Me.FilledRectangleButton.Location = New System.Drawing.Point(3, 233)
        Me.FilledRectangleButton.Name = "FilledRectangleButton"
        Me.FilledRectangleButton.Size = New System.Drawing.Size(42, 40)
        Me.FilledRectangleButton.TabIndex = 9
        Me.FilledRectangleButton.Tag = "FilledRectangle"
        Me.ToolTip1.SetToolTip(Me.FilledRectangleButton, "Filled Square/Rectangle")
        Me.FilledRectangleButton.UseVisualStyleBackColor = True
        '
        'FilledRectangleImageList
        '
        Me.FilledRectangleImageList.ImageStream = CType(resources.GetObject("FilledRectangleImageList.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.FilledRectangleImageList.TransparentColor = System.Drawing.Color.Transparent
        Me.FilledRectangleImageList.Images.SetKeyName(0, "FilledSquare.bmp")
        Me.FilledRectangleImageList.Images.SetKeyName(1, "DimFilledSquare.bmp")
        '
        'FillButton
        '
        Me.FillButton.FlatAppearance.BorderColor = System.Drawing.Color.SaddleBrown
        Me.FillButton.FlatAppearance.BorderSize = 0
        Me.FillButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.FillButton.ImageIndex = 1
        Me.FillButton.ImageList = Me.FillImageList
        Me.FillButton.Location = New System.Drawing.Point(51, 233)
        Me.FillButton.Name = "FillButton"
        Me.FillButton.Size = New System.Drawing.Size(42, 40)
        Me.FillButton.TabIndex = 2
        Me.FillButton.Tag = "Flood"
        Me.ToolTip1.SetToolTip(Me.FillButton, "Flood Fill")
        Me.FillButton.UseVisualStyleBackColor = True
        '
        'FillImageList
        '
        Me.FillImageList.ImageStream = CType(resources.GetObject("FillImageList.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.FillImageList.TransparentColor = System.Drawing.Color.Transparent
        Me.FillImageList.Images.SetKeyName(0, "Fill.bmp")
        Me.FillImageList.Images.SetKeyName(1, "DimFill.bmp")
        '
        'ResizeButton
        '
        Me.ResizeButton.FlatAppearance.BorderColor = System.Drawing.Color.SaddleBrown
        Me.ResizeButton.FlatAppearance.BorderSize = 0
        Me.ResizeButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ResizeButton.ImageIndex = 1
        Me.ResizeButton.ImageList = Me.ResizeImageList
        Me.ResizeButton.Location = New System.Drawing.Point(3, 279)
        Me.ResizeButton.Name = "ResizeButton"
        Me.ResizeButton.Size = New System.Drawing.Size(42, 40)
        Me.ResizeButton.TabIndex = 68
        Me.ResizeButton.Tag = "Resize"
        Me.ToolTip1.SetToolTip(Me.ResizeButton, "Resize")
        Me.ResizeButton.UseVisualStyleBackColor = True
        Me.ResizeButton.Visible = False
        '
        'ResizeImageList
        '
        Me.ResizeImageList.ImageStream = CType(resources.GetObject("ResizeImageList.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ResizeImageList.TransparentColor = System.Drawing.Color.Transparent
        Me.ResizeImageList.Images.SetKeyName(0, "resize.bmp")
        Me.ResizeImageList.Images.SetKeyName(1, "dimResize.bmp")
        '
        'MirrorButton
        '
        Me.MirrorButton.FlatAppearance.BorderColor = System.Drawing.Color.SaddleBrown
        Me.MirrorButton.FlatAppearance.BorderSize = 0
        Me.MirrorButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.MirrorButton.ImageIndex = 1
        Me.MirrorButton.ImageList = Me.MirrorImageList
        Me.MirrorButton.Location = New System.Drawing.Point(51, 279)
        Me.MirrorButton.Name = "MirrorButton"
        Me.MirrorButton.Size = New System.Drawing.Size(42, 40)
        Me.MirrorButton.TabIndex = 14
        Me.MirrorButton.Tag = "Mirror"
        Me.ToolTip1.SetToolTip(Me.MirrorButton, "Mirror")
        Me.MirrorButton.UseVisualStyleBackColor = True
        '
        'MirrorImageList
        '
        Me.MirrorImageList.ImageStream = CType(resources.GetObject("MirrorImageList.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.MirrorImageList.TransparentColor = System.Drawing.Color.Transparent
        Me.MirrorImageList.Images.SetKeyName(0, "Mirror.bmp")
        Me.MirrorImageList.Images.SetKeyName(1, "DimMirror.bmp")
        '
        'RotateLeftButton
        '
        Me.RotateLeftButton.FlatAppearance.BorderColor = System.Drawing.Color.SaddleBrown
        Me.RotateLeftButton.FlatAppearance.BorderSize = 0
        Me.RotateLeftButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.RotateLeftButton.ImageIndex = 1
        Me.RotateLeftButton.ImageList = Me.RotateLeftImageList
        Me.RotateLeftButton.Location = New System.Drawing.Point(3, 325)
        Me.RotateLeftButton.Name = "RotateLeftButton"
        Me.RotateLeftButton.Size = New System.Drawing.Size(42, 40)
        Me.RotateLeftButton.TabIndex = 16
        Me.RotateLeftButton.Tag = "RotateLeft"
        Me.ToolTip1.SetToolTip(Me.RotateLeftButton, "Rotate Left")
        Me.RotateLeftButton.UseVisualStyleBackColor = True
        '
        'RotateLeftImageList
        '
        Me.RotateLeftImageList.ImageStream = CType(resources.GetObject("RotateLeftImageList.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.RotateLeftImageList.TransparentColor = System.Drawing.Color.Transparent
        Me.RotateLeftImageList.Images.SetKeyName(0, "RotateLeft.bmp")
        Me.RotateLeftImageList.Images.SetKeyName(1, "DimRotateLeft.bmp")
        '
        'RotateRightButton
        '
        Me.RotateRightButton.FlatAppearance.BorderColor = System.Drawing.Color.SaddleBrown
        Me.RotateRightButton.FlatAppearance.BorderSize = 0
        Me.RotateRightButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.RotateRightButton.ImageIndex = 1
        Me.RotateRightButton.ImageList = Me.RotateRightImageList
        Me.RotateRightButton.Location = New System.Drawing.Point(51, 325)
        Me.RotateRightButton.Name = "RotateRightButton"
        Me.RotateRightButton.Size = New System.Drawing.Size(42, 40)
        Me.RotateRightButton.TabIndex = 17
        Me.RotateRightButton.Tag = "RotateRight"
        Me.ToolTip1.SetToolTip(Me.RotateRightButton, "Rotate Right")
        Me.RotateRightButton.UseVisualStyleBackColor = True
        '
        'RotateRightImageList
        '
        Me.RotateRightImageList.ImageStream = CType(resources.GetObject("RotateRightImageList.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.RotateRightImageList.TransparentColor = System.Drawing.Color.Transparent
        Me.RotateRightImageList.Images.SetKeyName(0, "RotateRight.bmp")
        Me.RotateRightImageList.Images.SetKeyName(1, "DimRotateRight.bmp")
        '
        'FlipButton
        '
        Me.FlipButton.FlatAppearance.BorderColor = System.Drawing.Color.SaddleBrown
        Me.FlipButton.FlatAppearance.BorderSize = 0
        Me.FlipButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.FlipButton.ImageIndex = 1
        Me.FlipButton.ImageList = Me.FlipImageList
        Me.FlipButton.Location = New System.Drawing.Point(3, 371)
        Me.FlipButton.Name = "FlipButton"
        Me.FlipButton.Size = New System.Drawing.Size(42, 40)
        Me.FlipButton.TabIndex = 11
        Me.FlipButton.Tag = "Flip"
        Me.ToolTip1.SetToolTip(Me.FlipButton, "Flip")
        Me.FlipButton.UseVisualStyleBackColor = True
        '
        'FlipImageList
        '
        Me.FlipImageList.ImageStream = CType(resources.GetObject("FlipImageList.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.FlipImageList.TransparentColor = System.Drawing.Color.Transparent
        Me.FlipImageList.Images.SetKeyName(0, "Flip.bmp")
        Me.FlipImageList.Images.SetKeyName(1, "DimFlip.bmp")
        '
        'LineThickness
        '
        Me.LineThickness.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.LineThickness.Location = New System.Drawing.Point(6, 3)
        Me.LineThickness.Maximum = New Decimal(New Integer() {50, 0, 0, 0})
        Me.LineThickness.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.LineThickness.Name = "LineThickness"
        Me.LineThickness.Size = New System.Drawing.Size(100, 26)
        Me.LineThickness.TabIndex = 21
        Me.LineThickness.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.ToolTip1.SetToolTip(Me.LineThickness, "Line Thickness")
        Me.LineThickness.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'SketchBookCloseLink
        '
        Me.SketchBookCloseLink.ActiveLinkColor = System.Drawing.Color.SaddleBrown
        Me.SketchBookCloseLink.AutoSize = True
        Me.SketchBookCloseLink.BackColor = System.Drawing.Color.Ivory
        Me.SketchBookCloseLink.DisabledLinkColor = System.Drawing.Color.Peru
        Me.SketchBookCloseLink.Dock = System.Windows.Forms.DockStyle.Right
        Me.SketchBookCloseLink.LinkColor = System.Drawing.Color.SaddleBrown
        Me.SketchBookCloseLink.Location = New System.Drawing.Point(875, 559)
        Me.SketchBookCloseLink.Name = "SketchBookCloseLink"
        Me.SketchBookCloseLink.Size = New System.Drawing.Size(112, 21)
        Me.SketchBookCloseLink.TabIndex = 31
        Me.SketchBookCloseLink.TabStop = True
        Me.SketchBookCloseLink.Text = "Use This Sketch"
        Me.SketchBookCloseLink.VisitedLinkColor = System.Drawing.Color.SaddleBrown
        '
        'PaintBox
        '
        Me.PaintBox.BackColor = System.Drawing.Color.Ivory
        Me.PaintBox.Location = New System.Drawing.Point(111, 215)
        Me.PaintBox.Name = "PaintBox"
        Me.PaintBox.Size = New System.Drawing.Size(99, 313)
        Me.PaintBox.TabIndex = 34
        Me.ToolTip1.SetToolTip(Me.PaintBox, "Preset Colors/Fill Textures")
        '
        'FileSystemWatcher1
        '
        Me.FileSystemWatcher1.EnableRaisingEvents = True
        Me.FileSystemWatcher1.SynchronizingObject = Me
        '
        'TextBox1
        '
        Me.TextBox1.Font = New System.Drawing.Font("Times New Roman", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox1.Location = New System.Drawing.Point(6, 70)
        Me.TextBox1.Multiline = True
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(100, 76)
        Me.TextBox1.TabIndex = 35
        Me.TextBox1.Text = "Your Text" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Here"
        Me.TextBox1.WordWrap = False
        '
        'FontButton
        '
        Me.FontButton.BackColor = System.Drawing.Color.Ivory
        Me.FontButton.FlatAppearance.BorderColor = System.Drawing.Color.SaddleBrown
        Me.FontButton.FlatAppearance.BorderSize = 2
        Me.FontButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.FontButton.Font = New System.Drawing.Font("Times New Roman", 12.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Italic), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FontButton.ImageIndex = 0
        Me.FontButton.Location = New System.Drawing.Point(6, 35)
        Me.FontButton.Name = "FontButton"
        Me.FontButton.Size = New System.Drawing.Size(101, 30)
        Me.FontButton.TabIndex = 36
        Me.FontButton.Tag = "Brush"
        Me.FontButton.Text = "Font"
        Me.ToolTip1.SetToolTip(Me.FontButton, "Change Font Button")
        Me.FontButton.UseVisualStyleBackColor = False
        '
        'CancelLink
        '
        Me.CancelLink.ActiveLinkColor = System.Drawing.Color.SaddleBrown
        Me.CancelLink.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CancelLink.AutoSize = True
        Me.CancelLink.BackColor = System.Drawing.Color.Ivory
        Me.CancelLink.DisabledLinkColor = System.Drawing.Color.Peru
        Me.CancelLink.LinkColor = System.Drawing.Color.SaddleBrown
        Me.CancelLink.Location = New System.Drawing.Point(13, 559)
        Me.CancelLink.Name = "CancelLink"
        Me.CancelLink.Size = New System.Drawing.Size(199, 19)
        Me.CancelLink.TabIndex = 38
        Me.CancelLink.TabStop = True
        Me.CancelLink.Text = "Cancel and Return to Journal"
        Me.CancelLink.VisitedLinkColor = System.Drawing.Color.SaddleBrown
        '
        'SwatchRowsLabel
        '
        Me.SwatchRowsLabel.AutoSize = True
        Me.SwatchRowsLabel.BackColor = System.Drawing.Color.Ivory
        Me.SwatchRowsLabel.Font = New System.Drawing.Font("Times New Roman", 9.75!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Italic), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SwatchRowsLabel.Location = New System.Drawing.Point(3, 44)
        Me.SwatchRowsLabel.Name = "SwatchRowsLabel"
        Me.SwatchRowsLabel.Size = New System.Drawing.Size(86, 16)
        Me.SwatchRowsLabel.TabIndex = 66
        Me.SwatchRowsLabel.Text = "MM 40 Rows:"
        Me.SwatchRowsLabel.Visible = False
        '
        'SwatchRowsTextBox
        '
        Me.SwatchRowsTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.SwatchRowsTextBox.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SwatchRowsTextBox.Location = New System.Drawing.Point(6, 63)
        Me.SwatchRowsTextBox.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.SwatchRowsTextBox.Name = "SwatchRowsTextBox"
        Me.SwatchRowsTextBox.Size = New System.Drawing.Size(100, 26)
        Me.SwatchRowsTextBox.TabIndex = 63
        Me.SwatchRowsTextBox.Value = New Decimal(New Integer() {100, 0, 0, 0})
        Me.SwatchRowsTextBox.Visible = False
        '
        'SwatchStitchesLabel
        '
        Me.SwatchStitchesLabel.AutoSize = True
        Me.SwatchStitchesLabel.BackColor = System.Drawing.Color.Ivory
        Me.SwatchStitchesLabel.Font = New System.Drawing.Font("Times New Roman", 9.75!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Italic), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SwatchStitchesLabel.Location = New System.Drawing.Point(3, 92)
        Me.SwatchStitchesLabel.Name = "SwatchStitchesLabel"
        Me.SwatchStitchesLabel.Size = New System.Drawing.Size(72, 16)
        Me.SwatchStitchesLabel.TabIndex = 65
        Me.SwatchStitchesLabel.Text = "MM 40 Sts:"
        Me.SwatchStitchesLabel.Visible = False
        '
        'SwatchStitchesTextBox
        '
        Me.SwatchStitchesTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.SwatchStitchesTextBox.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SwatchStitchesTextBox.Location = New System.Drawing.Point(6, 111)
        Me.SwatchStitchesTextBox.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.SwatchStitchesTextBox.Name = "SwatchStitchesTextBox"
        Me.SwatchStitchesTextBox.Size = New System.Drawing.Size(100, 26)
        Me.SwatchStitchesTextBox.TabIndex = 64
        Me.SwatchStitchesTextBox.Value = New Decimal(New Integer() {100, 0, 0, 0})
        Me.SwatchStitchesTextBox.Visible = False
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.Transparent
        Me.Panel1.Controls.Add(Me.FontButton)
        Me.Panel1.Controls.Add(Me.SwatchRowsLabel)
        Me.Panel1.Controls.Add(Me.PaintBox)
        Me.Panel1.Controls.Add(Me.ToolBox)
        Me.Panel1.Controls.Add(Me.ToolOptionsBox)
        Me.Panel1.Controls.Add(Me.LineThickness)
        Me.Panel1.Controls.Add(Me.SwatchStitchesTextBox)
        Me.Panel1.Controls.Add(Me.SwatchStitchesLabel)
        Me.Panel1.Controls.Add(Me.SwatchRowsTextBox)
        Me.Panel1.Controls.Add(Me.TextBox1)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(3, 31)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(209, 525)
        Me.Panel1.TabIndex = 67
        '
        'PageDrawSketch
        '
        Me.PageDrawSketch.AutoSize = True
        Me.PageDrawSketch.BackColor = System.Drawing.Color.Transparent
        Me.PageDrawSketch.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.PageDrawSketch.ColumnCount = 5
        Me.PageDrawSketch.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle)
        Me.PageDrawSketch.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle)
        Me.PageDrawSketch.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle)
        Me.PageDrawSketch.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.PageDrawSketch.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10.0!))
        Me.PageDrawSketch.Controls.Add(Me.Panel1, 0, 1)
        Me.PageDrawSketch.Controls.Add(Me.SketchBox1, 1, 1)
        Me.PageDrawSketch.Controls.Add(Me.CancelLink, 0, 2)
        Me.PageDrawSketch.Controls.Add(Me.SketchBookCloseLink, 3, 2)
        Me.PageDrawSketch.Controls.Add(Me.Rowz, 2, 2)
        Me.PageDrawSketch.Controls.Add(Me.RowzLabel, 1, 2)
        Me.PageDrawSketch.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PageDrawSketch.Location = New System.Drawing.Point(0, 0)
        Me.PageDrawSketch.Margin = New System.Windows.Forms.Padding(0)
        Me.PageDrawSketch.Name = "PageDrawSketch"
        Me.PageDrawSketch.RowCount = 4
        Me.PageDrawSketch.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28.0!))
        Me.PageDrawSketch.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.PageDrawSketch.RowStyles.Add(New System.Windows.Forms.RowStyle)
        Me.PageDrawSketch.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.PageDrawSketch.Size = New System.Drawing.Size(1000, 600)
        Me.PageDrawSketch.TabIndex = 68
        '
        'SketchBox1
        '
        Me.SketchBox1.AutoSize = True
        Me.SketchBox1.CenterPt = New System.Drawing.Point(0, 0)
        Me.SketchBox1.chartTemplate = Nothing
        Me.SketchBox1.Clip = False
        Me.SketchBox1.Color1 = -16
        Me.SketchBox1.Color2 = -16
        Me.SketchBox1.Color3 = -16
        Me.SketchBox1.Color4 = -16
        Me.SketchBox1.ColorCount = 4
        Me.PageDrawSketch.SetColumnSpan(Me.SketchBox1, 3)
        Me.SketchBox1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SketchBox1.DrawFont = Nothing
        Me.SketchBox1.DrawText = Nothing
        Me.SketchBox1.LeftBrush = Nothing
        Me.SketchBox1.LeftPen = Nothing
        Me.SketchBox1.Location = New System.Drawing.Point(218, 31)
        Me.SketchBox1.Motif = Nothing
        Me.SketchBox1.Name = "SketchBox1"
        Me.SketchBox1.RightBrush = Nothing
        Me.SketchBox1.RightPen = Nothing
        Me.SketchBox1.RowGauge = 0.25
        Me.SketchBox1.Rowz = 0
        Me.SketchBox1.Size = New System.Drawing.Size(769, 525)
        Me.SketchBox1.SketchBrush = Nothing
        Me.SketchBox1.SketchImage = Nothing
        Me.SketchBox1.SketchMode = Nothing
        Me.SketchBox1.SketchPen = Nothing
        Me.SketchBox1.SketchTool = Nothing
        Me.SketchBox1.StitchGauge = 0.25
        Me.SketchBox1.Symmetry = 0
        Me.SketchBox1.TabIndex = 33
        '
        'Rowz
        '
        Me.Rowz.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Rowz.Dock = System.Windows.Forms.DockStyle.Left
        Me.Rowz.Font = New System.Drawing.Font("Times New Roman", 9.0!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Rowz.Increment = New Decimal(New Integer() {2, 0, 0, 0})
        Me.Rowz.Location = New System.Drawing.Point(411, 559)
        Me.Rowz.Margin = New System.Windows.Forms.Padding(0)
        Me.Rowz.Minimum = New Decimal(New Integer() {2, 0, 0, 0})
        Me.Rowz.Name = "Rowz"
        Me.Rowz.Size = New System.Drawing.Size(96, 21)
        Me.Rowz.TabIndex = 69
        Me.Rowz.Value = New Decimal(New Integer() {2, 0, 0, 0})
        Me.Rowz.Visible = False
        '
        'RowzLabel
        '
        Me.RowzLabel.AutoSize = True
        Me.RowzLabel.BackColor = System.Drawing.Color.Ivory
        Me.RowzLabel.Dock = System.Windows.Forms.DockStyle.Right
        Me.RowzLabel.Font = New System.Drawing.Font("Times New Roman", 9.75!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Italic), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RowzLabel.Location = New System.Drawing.Point(218, 559)
        Me.RowzLabel.Name = "RowzLabel"
        Me.RowzLabel.Size = New System.Drawing.Size(190, 21)
        Me.RowzLabel.TabIndex = 68
        Me.RowzLabel.Text = "Lock Passes Per Row of Squares:"
        Me.RowzLabel.TextAlign = System.Drawing.ContentAlignment.TopRight
        Me.RowzLabel.Visible = False
        '
        'PageChooseStitchPattern
        '
        Me.PageChooseStitchPattern.AutoSize = True
        Me.PageChooseStitchPattern.BackColor = System.Drawing.Color.Transparent
        Me.PageChooseStitchPattern.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.PageChooseStitchPattern.ColumnCount = 3
        Me.PageChooseStitchPattern.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle)
        Me.PageChooseStitchPattern.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.PageChooseStitchPattern.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10.0!))
        Me.PageChooseStitchPattern.Controls.Add(Me.ViewStitchPatPictureBox, 1, 1)
        Me.PageChooseStitchPattern.Controls.Add(Me.Panel2, 0, 1)
        Me.PageChooseStitchPattern.Controls.Add(Me.ReturnToSketch, 0, 2)
        Me.PageChooseStitchPattern.Controls.Add(Me.UpdateFillPatternLink, 1, 2)
        Me.PageChooseStitchPattern.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PageChooseStitchPattern.Location = New System.Drawing.Point(0, 0)
        Me.PageChooseStitchPattern.Margin = New System.Windows.Forms.Padding(0)
        Me.PageChooseStitchPattern.Name = "PageChooseStitchPattern"
        Me.PageChooseStitchPattern.RowCount = 4
        Me.PageChooseStitchPattern.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28.0!))
        Me.PageChooseStitchPattern.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.PageChooseStitchPattern.RowStyles.Add(New System.Windows.Forms.RowStyle)
        Me.PageChooseStitchPattern.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.PageChooseStitchPattern.Size = New System.Drawing.Size(1000, 600)
        Me.PageChooseStitchPattern.TabIndex = 70
        '
        'ViewStitchPatPictureBox
        '
        Me.ViewStitchPatPictureBox.BuiltIn = False
        Me.ViewStitchPatPictureBox.Color1 = -16
        Me.ViewStitchPatPictureBox.Color2 = -16
        Me.ViewStitchPatPictureBox.Color3 = -16
        Me.ViewStitchPatPictureBox.Color4 = -16
        Me.ViewStitchPatPictureBox.ColorCount = 4
        Me.ViewStitchPatPictureBox.Cursor = System.Windows.Forms.Cursors.Default
        Me.ViewStitchPatPictureBox.DisplayOnly = True
        Me.ViewStitchPatPictureBox.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ViewStitchPatPictureBox.Location = New System.Drawing.Point(326, 28)
        Me.ViewStitchPatPictureBox.Margin = New System.Windows.Forms.Padding(0)
        Me.ViewStitchPatPictureBox.Motif = ""
        Me.ViewStitchPatPictureBox.Name = "ViewStitchPatPictureBox"
        Me.ViewStitchPatPictureBox.Size = New System.Drawing.Size(664, 533)
        Me.ViewStitchPatPictureBox.TabIndex = 71
        '
        'Panel2
        '
        Me.Panel2.AutoSize = True
        Me.Panel2.BackColor = System.Drawing.Color.Ivory
        Me.Panel2.Controls.Add(Me.StitchPatListFilterButton)
        Me.Panel2.Controls.Add(Me.StitchPatListFilterLink)
        Me.Panel2.Controls.Add(Me.StitchPatListFilterText)
        Me.Panel2.Controls.Add(Me.StitchPatList)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Left
        Me.Panel2.Location = New System.Drawing.Point(3, 31)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(320, 527)
        Me.Panel2.TabIndex = 67
        '
        'StitchPatListFilterButton
        '
        Me.StitchPatListFilterButton.FlatAppearance.BorderColor = System.Drawing.Color.SaddleBrown
        Me.StitchPatListFilterButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.SaddleBrown
        Me.StitchPatListFilterButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Ivory
        Me.StitchPatListFilterButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.StitchPatListFilterButton.Font = New System.Drawing.Font("Times New Roman", 8.25!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.StitchPatListFilterButton.ForeColor = System.Drawing.Color.SaddleBrown
        Me.StitchPatListFilterButton.Location = New System.Drawing.Point(233, 3)
        Me.StitchPatListFilterButton.Name = "StitchPatListFilterButton"
        Me.StitchPatListFilterButton.Size = New System.Drawing.Size(42, 26)
        Me.StitchPatListFilterButton.TabIndex = 59
        Me.StitchPatListFilterButton.Text = "Show"
        Me.StitchPatListFilterButton.UseVisualStyleBackColor = True
        Me.StitchPatListFilterButton.Visible = False
        '
        'StitchPatListFilterLink
        '
        Me.StitchPatListFilterLink.ActiveLinkColor = System.Drawing.Color.SaddleBrown
        Me.StitchPatListFilterLink.AutoEllipsis = True
        Me.StitchPatListFilterLink.AutoSize = True
        Me.StitchPatListFilterLink.DisabledLinkColor = System.Drawing.Color.Peru
        Me.StitchPatListFilterLink.LinkColor = System.Drawing.Color.SaddleBrown
        Me.StitchPatListFilterLink.Location = New System.Drawing.Point(4, 10)
        Me.StitchPatListFilterLink.MaximumSize = New System.Drawing.Size(280, 19)
        Me.StitchPatListFilterLink.Name = "StitchPatListFilterLink"
        Me.StitchPatListFilterLink.Size = New System.Drawing.Size(132, 19)
        Me.StitchPatListFilterLink.TabIndex = 56
        Me.StitchPatListFilterLink.TabStop = True
        Me.StitchPatListFilterLink.Text = "Stitch Patterns: All"
        Me.StitchPatListFilterLink.VisitedLinkColor = System.Drawing.Color.SaddleBrown
        '
        'StitchPatListFilterText
        '
        Me.StitchPatListFilterText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.StitchPatListFilterText.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.StitchPatListFilterText.Location = New System.Drawing.Point(4, 4)
        Me.StitchPatListFilterText.Name = "StitchPatListFilterText"
        Me.StitchPatListFilterText.Size = New System.Drawing.Size(223, 26)
        Me.StitchPatListFilterText.TabIndex = 58
        Me.StitchPatListFilterText.Visible = False
        '
        'StitchPatList
        '
        Me.StitchPatList.BackColor = System.Drawing.Color.Ivory
        Me.StitchPatList.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.StitchPatList.Cursor = System.Windows.Forms.Cursors.Hand
        Me.StitchPatList.DisplayMember = "Name"
        Me.StitchPatList.ForeColor = System.Drawing.Color.SaddleBrown
        Me.StitchPatList.FormattingEnabled = True
        Me.StitchPatList.ItemHeight = 19
        Me.StitchPatList.Location = New System.Drawing.Point(28, 35)
        Me.StitchPatList.Name = "StitchPatList"
        Me.StitchPatList.Size = New System.Drawing.Size(289, 361)
        Me.StitchPatList.TabIndex = 57
        Me.StitchPatList.ValueMember = "Id"
        '
        'ReturnToSketch
        '
        Me.ReturnToSketch.ActiveLinkColor = System.Drawing.Color.SaddleBrown
        Me.ReturnToSketch.AutoSize = True
        Me.ReturnToSketch.DisabledLinkColor = System.Drawing.Color.Peru
        Me.ReturnToSketch.Dock = System.Windows.Forms.DockStyle.Left
        Me.ReturnToSketch.LinkColor = System.Drawing.Color.SaddleBrown
        Me.ReturnToSketch.Location = New System.Drawing.Point(3, 561)
        Me.ReturnToSketch.Name = "ReturnToSketch"
        Me.ReturnToSketch.Size = New System.Drawing.Size(193, 19)
        Me.ReturnToSketch.TabIndex = 70
        Me.ReturnToSketch.TabStop = True
        Me.ReturnToSketch.Text = "Cancel and Return to Sketch"
        Me.ReturnToSketch.VisitedLinkColor = System.Drawing.Color.SaddleBrown
        '
        'UpdateFillPatternLink
        '
        Me.UpdateFillPatternLink.ActiveLinkColor = System.Drawing.Color.SaddleBrown
        Me.UpdateFillPatternLink.AutoSize = True
        Me.UpdateFillPatternLink.DisabledLinkColor = System.Drawing.Color.Peru
        Me.UpdateFillPatternLink.Dock = System.Windows.Forms.DockStyle.Right
        Me.UpdateFillPatternLink.LinkColor = System.Drawing.Color.SaddleBrown
        Me.UpdateFillPatternLink.Location = New System.Drawing.Point(843, 561)
        Me.UpdateFillPatternLink.Name = "UpdateFillPatternLink"
        Me.UpdateFillPatternLink.Size = New System.Drawing.Size(144, 19)
        Me.UpdateFillPatternLink.TabIndex = 72
        Me.UpdateFillPatternLink.TabStop = True
        Me.UpdateFillPatternLink.Text = "Use This Fill Pattern"
        Me.UpdateFillPatternLink.VisitedLinkColor = System.Drawing.Color.SaddleBrown
        '
        'SketchBook
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.BackColor = System.Drawing.Color.Ivory
        Me.BackgroundImage = Global.Journal6.My.Resources.Resources.SketchCover
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.ClientSize = New System.Drawing.Size(1000, 600)
        Me.Controls.Add(Me.PageDrawSketch)
        Me.Controls.Add(Me.PageChooseStitchPattern)
        Me.DoubleBuffered = True
        Me.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ForeColor = System.Drawing.Color.SaddleBrown
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "SketchBook"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "SketchBook"
        Me.TransparencyKey = System.Drawing.Color.FromArgb(CType(CType(173, Byte), Integer), CType(CType(57, Byte), Integer), CType(CType(115, Byte), Integer))
        Me.ToolOptionsBox.ResumeLayout(False)
        CType(Me.BrushAngle, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ToolBox.ResumeLayout(False)
        CType(Me.LineThickness, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.FileSystemWatcher1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.SwatchRowsTextBox, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.SwatchStitchesTextBox, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.PageDrawSketch.ResumeLayout(False)
        Me.PageDrawSketch.PerformLayout()
        CType(Me.Rowz, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PageChooseStitchPattern.ResumeLayout(False)
        Me.PageChooseStitchPattern.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents DropperButton As System.Windows.Forms.Button
    Friend WithEvents FillButton As System.Windows.Forms.Button
    Friend WithEvents BrushButton As System.Windows.Forms.Button
    Friend WithEvents FilledCircleButton As System.Windows.Forms.Button
    Friend WithEvents DiagonalButton As System.Windows.Forms.Button
    Friend WithEvents CircleButton As System.Windows.Forms.Button
    Friend WithEvents RotateLeftButton As System.Windows.Forms.Button
    Friend WithEvents MirrorButton As System.Windows.Forms.Button
    Friend WithEvents HorizontalButton As System.Windows.Forms.Button
    Friend WithEvents FlipButton As System.Windows.Forms.Button
    Friend WithEvents FilledRectangleButton As System.Windows.Forms.Button
    Friend WithEvents VerticalButton As System.Windows.Forms.Button
    Friend WithEvents RectangleButton As System.Windows.Forms.Button
    Friend WithEvents TextButton As System.Windows.Forms.Button
    Friend WithEvents RotateRightButton As System.Windows.Forms.Button
    Friend WithEvents LayoutButton As System.Windows.Forms.Button
    Friend WithEvents LayoutImageList As System.Windows.Forms.ImageList
    Friend WithEvents BrushImageList As System.Windows.Forms.ImageList
    Friend WithEvents ToolOptionsBox As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents DropperImageList As System.Windows.Forms.ImageList
    Friend WithEvents ToolBox As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents DiagonalLineImageList As System.Windows.Forms.ImageList
    Friend WithEvents HorizontalLineImageList As System.Windows.Forms.ImageList
    Friend WithEvents VerticalLineImageList As System.Windows.Forms.ImageList
    Friend WithEvents CircleImageList As System.Windows.Forms.ImageList
    Friend WithEvents FilledCircleImageList As System.Windows.Forms.ImageList
    Friend WithEvents SketchBookCloseLink As System.Windows.Forms.LinkLabel
    Friend WithEvents LineThickness As System.Windows.Forms.NumericUpDown
    Friend WithEvents FontDialog1 As System.Windows.Forms.FontDialog
    Friend WithEvents RectangleImageList As System.Windows.Forms.ImageList
    Friend WithEvents FilledRectangleImageList As System.Windows.Forms.ImageList
    Friend WithEvents RotateLeftImageList As System.Windows.Forms.ImageList
    Friend WithEvents RotateRightImageList As System.Windows.Forms.ImageList
    Friend WithEvents FlipImageList As System.Windows.Forms.ImageList
    Friend WithEvents MirrorImageList As System.Windows.Forms.ImageList
    Friend WithEvents FillImageList As System.Windows.Forms.ImageList
    Friend WithEvents TextImageList As System.Windows.Forms.ImageList
    Friend WithEvents FileSystemWatcher1 As System.IO.FileSystemWatcher
    Friend WithEvents SketchBox1 As Journal6.SketchBox
    Friend WithEvents PaintBox As System.Windows.Forms.Panel
    Friend WithEvents LeftMousePaint As Journal6.DBPanel
    Friend WithEvents RightMousePaint As Journal6.DBPanel
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents FontButton As System.Windows.Forms.Button
    Friend WithEvents SymmetryButton As System.Windows.Forms.Button
    Friend WithEvents SymmetryImageList As System.Windows.Forms.ImageList
    Friend WithEvents stampButton As System.Windows.Forms.Button
    Friend WithEvents StampImageList As System.Windows.Forms.ImageList
    Friend WithEvents CancelLink As System.Windows.Forms.LinkLabel
    Friend WithEvents SwatchRowsTextBox As System.Windows.Forms.NumericUpDown
    Friend WithEvents SwatchStitchesTextBox As System.Windows.Forms.NumericUpDown
    Friend WithEvents SwatchRowsLabel As System.Windows.Forms.Label
    Friend WithEvents SwatchStitchesLabel As System.Windows.Forms.Label
    Friend WithEvents TapeMeasureButton As System.Windows.Forms.Button
    Friend WithEvents MeasureImageList As System.Windows.Forms.ImageList
    Friend WithEvents BrushAngle As System.Windows.Forms.NumericUpDown
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents PageDrawSketch As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents Rowz As System.Windows.Forms.NumericUpDown
    Friend WithEvents RowzLabel As System.Windows.Forms.Label
    Friend WithEvents ResizeButton As System.Windows.Forms.Button
    Friend WithEvents ResizeImageList As System.Windows.Forms.ImageList
    Friend WithEvents PageChooseStitchPattern As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents StitchPatListFilterButton As System.Windows.Forms.Button
    Friend WithEvents StitchPatListFilterLink As System.Windows.Forms.LinkLabel
    Friend WithEvents StitchPatListFilterText As System.Windows.Forms.TextBox
    Friend WithEvents StitchPatList As System.Windows.Forms.ListBox
    Friend WithEvents ReturnToSketch As System.Windows.Forms.LinkLabel
    Friend WithEvents ViewStitchPatPictureBox As Journal6.StitchPatternBox
    Friend WithEvents UpdateFillPatternLink As System.Windows.Forms.LinkLabel


End Class
