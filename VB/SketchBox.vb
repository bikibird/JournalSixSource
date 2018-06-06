Public Class SketchBox
    Inherits UserControl
    Private propMotif As String
    Private propSketchBrush, propLeftBrush, propRightBrush As Drawing.TextureBrush
    Private propSketchPen, propLeftPen, propRightPen As Pen
    Private propSketchTool As String
    Private propSketchMode As String
    Private propSketch As Bitmap
    Private propTemplate As Bitmap
    Private dragging As Boolean
    Private startPoint, endPoint, lastPoint As Point
    Private eraseBitmap As Bitmap
    Private redoBitmap As Bitmap
    Private g As Graphics
    Private selectPen, gridPen, chalkPen As Pen
    Private propDrawText As String
    Private propDrawFont As Font
    Private chalkFont As Font
    Private propcenterPt As Point
    Public StitchPatRect As Rectangle
    Private StitchPatCenter As PointF
    Private OldTool As String
    Private PropSymmetry As Integer
    Private clipMinX As Integer
    Private clipMinY As Integer
    Private clipMaxX As Integer
    Private clipMaxY As Integer
    Private clipMinXOld As Integer
    Private clipMinYOld As Integer
    Private clipMaxXOld As Integer
    Private clipMaxYOld As Integer
    Private unDoStack As Collection
    Private reDoStack As Collection
    Private TileView As Boolean = False
    Private Tracing As Boolean = False
    Private propRowGauge As Double
    Private propStitchGauge As Double
    Private propRowz As Integer
    Public chalkMarks As New Drawing2D.GraphicsPath
    Public chalkBrush As SolidBrush
    Public chalkMsg As String
    Public FillMode As String
    Private oldMagnification As Integer = 4
    Private oldAspect As Double = 1
    Private propClipMode As Boolean = False
    Private PaperWidth As Integer = 180
    Private PaperLength As Integer = 999
   
    Public Event MotifChanged(ByVal colors As Color())
    Public Event selectionChanged(ByVal bmap As Bitmap, ByVal b As MouseButtons)
    Public Event transparencyChanged(ByVal aColor As Color, ByVal aTransparency As Boolean)
  
    Public Property Clip() As Boolean
        Get
            Return propClipMode
        End Get
        Set(ByVal value As Boolean)
            propClipMode = value
            If value Then
                Me.ColorScheme.Visible = False
                Me.FlowLayoutPanel4.Visible = False
                Me.SizeLabel.Visible = False
                Me.LocationLabel.Visible = False
                Me.TileViewLink.Visible = False
                PaperLength = propSketch.Height
                PaperWidth = propSketch.Width
                propSketchTool = "Lasso"
                Me.ColorScheme.Color1 = Color.White.ToArgb
                Me.ColorScheme.Color2 = Color.Black.ToArgb
                Me.ColorCount = 2
                propLeftPen = Pens.Black
                Me.ToolTip1.SetToolTip(SketchPanel, "Drag with mouse to crop image.")

            Else
                Me.ColorScheme.Visible = True
                Me.FlowLayoutPanel4.Visible = True
                Me.SizeLabel.Visible = True
                Me.LocationLabel.Visible = True
                Me.TileViewLink.Visible = True
                PaperWidth = 180
                PaperLength = 999
            End If
        End Set
    End Property
    Public Property RowGauge() As Double
        Get
            Return propRowGauge
        End Get
        Set(ByVal value As Double)
            propRowGauge = value
        End Set
    End Property
    Public Property StitchGauge() As Double
        Get
            Return propStitchGauge
        End Get
        Set(ByVal value As Double)
            propStitchGauge = value
        End Set
    End Property
    Public Property Rowz() As Integer
        Get
            Return propRowz
        End Get
        Set(ByVal value As Integer)
            propRowz = value
        End Set
    End Property
    Public Property chartTemplate() As Bitmap
        Get
            Return propTemplate
        End Get
        Set(ByVal value As Bitmap)
            propTemplate = value
        End Set
    End Property
    Public Property SketchImage() As Bitmap
        Get
            Return propSketch
        End Get
        Set(ByVal value As Bitmap)
            propSketch = value
            If propClipMode Then
                PaperLength = value.Height
                PaperWidth = value.Width
            End If
        End Set
    End Property
    Public Property SketchBrush() As Drawing.TextureBrush
        Get
            Return propSketchBrush
        End Get
        Set(ByVal value As Drawing.TextureBrush)
            propSketchBrush = value
        End Set
    End Property
    Public Property LeftBrush() As Drawing.TextureBrush
        Get
            Return propLeftBrush
        End Get
        Set(ByVal value As Drawing.TextureBrush)
            propLeftBrush = value
        End Set
    End Property
    Public Property RightBrush() As Drawing.TextureBrush
        Get
            Return propRightBrush
        End Get
        Set(ByVal value As Drawing.TextureBrush)
            propRightBrush = value
        End Set
    End Property
    Public Property SketchPen() As Pen
        Get
            Return propSketchPen
        End Get
        Set(ByVal value As Pen)
            propSketchPen = value
        End Set
    End Property
    Public Property LeftPen() As Pen
        Get
            Return propLeftPen
        End Get
        Set(ByVal value As Pen)
            propLeftPen = value

        End Set
    End Property
    Public Property RightPen() As Pen
        Get
            Return propRightPen
        End Get
        Set(ByVal value As Pen)
            propRightPen = value
        End Set
    End Property
    Public Property SketchTool() As String
        Get
            Return propSketchTool
        End Get
        Set(ByVal value As String)
            propSketchTool = value
        End Set
    End Property
    Public Property DrawText() As String
        Get
            Return propDrawText
        End Get
        Set(ByVal value As String)
            propDrawText = value
        End Set
    End Property

    Public Property DrawFont() As Font
        Get
            Return propDrawFont
        End Get
        Set(ByVal value As Font)
            propDrawFont = value
        End Set
    End Property
    Public Property SketchMode() As String
        Get
            Return propSketchMode
        End Get
        Set(ByVal value As String)
            propSketchMode = value
        End Set
    End Property
    Public Property CenterPt() As Point
        Get
            Return propCenterPt
        End Get
        Set(ByVal value As Point)
            propcenterPt = value
        End Set
    End Property
    Public Property Symmetry() As Integer
        Get
            Return PropSymmetry
        End Get
        Set(ByVal value As Integer)
            PropSymmetry = value
        End Set
    End Property
    Public Property Motif() As String
        Get
            Return propMotif
        End Get
        Set(ByVal value As String)

            propMotif = value
            If propSketchMode = "Chart" Then
                SizeLabel.Text = "CM"

            Else
                SizeLabel.Text = IIf(IsNothing(value), "", Trim(Mid(value, 3, 3)) & " x " & Trim(Mid(value, 7, 3)))
            End If


            propSketch = motifToBitmap(propMotif)
            initializeSketch(value)
            


            RaiseEvent MotifChanged(ColorScheme.Colors)
            unDoStack = New Collection
            reDoStack = New Collection
            TileView = False

        End Set
    End Property

    Sub initializeSketch(ByVal motif As String)
        If Not IsNothing(g) Then
            g.Dispose()
        End If
        g = Graphics.FromImage(propSketch)
        'g.PixelOffsetMode = Drawing2D.PixelOffsetMode.Half
        g.SmoothingMode = Drawing2D.SmoothingMode.None
        g.InterpolationMode = Drawing2D.InterpolationMode.NearestNeighbor

        If IsNothing(selectPen) Then
            selectPen = New Pen(Color.FromArgb(192, Color.Silver), 1)
            selectPen.DashStyle = Drawing2D.DashStyle.Dash

        End If

        If IsNothing(gridPen) Then
            gridPen = New Pen(Color.FromArgb(192, Color.Turquoise), 1)
            gridPen.DashStyle = Drawing2D.DashStyle.Dot

        End If
        If IsNothing(chalkPen) Then
            chalkPen = New Pen(Color.Turquoise, 1)
        End If
        If IsNothing(chalkFont) Then
            chalkFont = New Font(FontFamily.GenericMonospace, 10, FontStyle.Regular)
        End If
        If IsNothing(chalkBrush) Then
            chalkBrush = New SolidBrush(Color.Red)
        End If
        If propClipMode Then
            PaperWidth = propSketch.Width
            PaperLength = propSketch.Height
            If AspectRatio.Value <> 0 Then
                Me.SketchPanel.Height = PaperLength * (Magnification.Value / AspectRatio.Value)
            Else
                Me.SketchPanel.Height = PaperLength * Magnification.Value
            End If
            Me.SketchPanel.Width = PaperWidth * Magnification.Value
        End If
        If propSketchMode <> "Chart" And motif <> "" Then
            StitchPatRect.X = 0 : StitchPatRect.Y = 0
            StitchPatRect.Width = CInt(IIf(IsNothing(motif), "", Trim(Mid(motif, 3, 3))))
            StitchPatRect.Height = CInt(IIf(IsNothing(motif), "", Trim(Mid(motif, 7, 3))))
            StitchPatCenter.X = StitchPatRect.X + StitchPatRect.Width / 2
            StitchPatCenter.Y = StitchPatRect.Y + StitchPatRect.Height / 2
        Else

            StitchPatRect.X = 0 : StitchPatRect.Y = 0
            StitchPatRect.Width = PaperWidth
            StitchPatRect.Height = PaperLength
            StitchPatCenter.X = 89.5
            StitchPatCenter.Y = 499


        End If

        'End If

        Me.SketchPanel.Invalidate()
    End Sub

    Public Property Color1() As Integer
        Get
            Return ColorScheme.Color1
        End Get
        Set(ByVal value As Integer)
            ColorScheme.Color1 = value
        End Set
    End Property
    Public Property Color2() As Integer
        Get
            Return ColorScheme.Color2
        End Get
        Set(ByVal value As Integer)
            ColorScheme.Color2 = value
        End Set
    End Property
    Public Property Color3() As Integer
        Get
            Return ColorScheme.Color3
        End Get
        Set(ByVal value As Integer)
            ColorScheme.Color3 = value
        End Set
    End Property
    Public Property Color4() As Integer
        Get
            Return ColorScheme.Color4
        End Get
        Set(ByVal value As Integer)
            ColorScheme.Color4 = value
        End Set
    End Property
    Public Property ColorCount() As Integer
        Get
            Return ColorScheme.Count
        End Get
        Set(ByVal value As Integer)
            ColorScheme.Count = value
        End Set
    End Property


    Private Sub SketchPanel_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles SketchPanel.MouseDown
        If Not TileView Then
            If Not dragging Then
                setEraseBitmap()
                unDoStack.Add(eraseBitmap.Clone)
                If unDoStack.Count > 10 Then
                    unDoStack.Remove(1)
                End If

                startPoint = getCoordinates(e.X, e.Y)
                lastPoint = startPoint
                If e.Button = Windows.Forms.MouseButtons.Left Then
                    propSketchPen = LeftPen
                    propSketchBrush = LeftBrush
                Else
                    propSketchPen = RightPen
                    propSketchBrush = RightBrush
                End If

                dragging = True

                Select Case propSketchTool
                    Case "Brush"
                        drawWithSymmetry(lastPoint.X, lastPoint.Y, startPoint.X, startPoint.Y)
                    Case "Text"
                        Dim r As Rectangle
                        Dim s As SizeF
                        s = g.MeasureString(propDrawText, propDrawFont)
                        r = New Rectangle(lastPoint.X, lastPoint.Y, s.Width, s.Height)
                        eraseSketch(r)
                        DrawWithTool(propSketchTool, CInt(startPoint.X), CInt(startPoint.Y), lastPoint.X, lastPoint.Y)
                        SketchPanel.Invalidate(getLargestRectangle(startPoint.X, lastPoint.X, lastPoint.X + s.Width, startPoint.Y, lastPoint.Y, lastPoint.Y + s.Height))
                    Case "Stamp"
                        Dim r As Rectangle
                        r = New Rectangle(lastPoint.X, lastPoint.Y, propSketchBrush.Image.Width, propSketchBrush.Image.Height)
                        eraseSketch(r)
                        DrawWithTool(propSketchTool, CInt(startPoint.X), CInt(startPoint.Y), lastPoint.X, lastPoint.Y)
                        SketchPanel.Invalidate(getRectangle(lastPoint.X, lastPoint.Y, lastPoint.X + propSketchBrush.Image.Width + 1, lastPoint.Y + propSketchBrush.Image.Height + 1))
                End Select
            End If
        End If
    End Sub
    Private Sub SketchPanel_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles SketchPanel.MouseMove
        If Not TileView Then
            Dim dragPoint As Point
            Dim r As Rectangle
            dragPoint = getCoordinates(e.X, e.Y)
            If dragging Then

                Select Case propSketchTool
                    Case "Brush"
                        drawWithSymmetry(CInt(lastPoint.X), CInt(lastPoint.Y), dragPoint.X, dragPoint.Y)
                        'SketchPanel.Invalidate(getLargestRectangle(startPoint.X, lastPoint.X, dragPoint.X, startPoint.Y, lastPoint.Y, dragPoint.Y))
                        SketchPanel.Invalidate(getScaledRectangle(clipMinX, clipMinY, clipMaxX, clipMaxY))

                    Case "Dropper"
                        eraseSketch(getRectangle(startPoint.X, startPoint.Y, lastPoint.X, lastPoint.Y))
                        DrawWithTool(propSketchTool, CInt(startPoint.X), CInt(startPoint.Y), dragPoint.X, dragPoint.Y)
                        SketchPanel.Invalidate(getLargestRectangle(startPoint.X, lastPoint.X, dragPoint.X, startPoint.Y, lastPoint.Y, dragPoint.Y))
                    Case "Lasso"
                        eraseSketch(getRectangle(startPoint.X, startPoint.Y, lastPoint.X, lastPoint.Y))
                        DrawWithTool(propSketchTool, CInt(startPoint.X), CInt(startPoint.Y), dragPoint.X, dragPoint.Y)
                        SketchPanel.Invalidate(getLargestRectangle(startPoint.X, lastPoint.X, dragPoint.X, startPoint.Y, lastPoint.Y, dragPoint.Y))
                    Case "Measure", "Chalk"
                        eraseSketch(getRectangle(startPoint.X, startPoint.Y, lastPoint.X, lastPoint.Y))
                        DrawWithTool(propSketchTool, CInt(startPoint.X), CInt(startPoint.Y), dragPoint.X, dragPoint.Y)
                        SizeLabel.Text = getDiagonalMeasurement(startPoint.X, startPoint.Y, dragPoint.X, dragPoint.Y)
                        chalkMsg = getRowStitchesMeasurement(startPoint.X, startPoint.Y, dragPoint.X, dragPoint.Y)
                        SizeLabel.Invalidate()
                        chalkMsg = getRowStitchesMeasurement(startPoint.X, startPoint.Y, dragPoint.X, dragPoint.Y)
                        'r = getLargestRectangle(startPoint.X, lastPoint.X, dragPoint.X, startPoint.Y, lastPoint.Y, dragPoint.Y)
                        'SketchPanel.Invalidate(getLargestRectangle(0, r.X, r.X + r.Width, -Me.SplitContainer1.Panel1.AutoScrollPosition.Y, r.Y, r.Y + r.Height))
                        SketchPanel.Invalidate()
                    Case "Mirror"
                        eraseSketch(getRectangle(startPoint.X, startPoint.Y, lastPoint.X, lastPoint.Y))
                        DrawWithTool(propSketchTool, CInt(startPoint.X), CInt(startPoint.Y), dragPoint.X, dragPoint.Y)

                        SketchPanel.Invalidate(getLargestRectangle(startPoint.X, lastPoint.X, dragPoint.X, startPoint.Y, lastPoint.Y, dragPoint.Y))
                    Case "Flip"
                        eraseSketch(getRectangle(startPoint.X, startPoint.Y, lastPoint.X, lastPoint.Y))
                        DrawWithTool(propSketchTool, CInt(startPoint.X), CInt(startPoint.Y), dragPoint.X, dragPoint.Y)
                        SketchPanel.Invalidate(getLargestRectangle(startPoint.X, lastPoint.X, dragPoint.X, startPoint.Y, lastPoint.Y, dragPoint.Y))
                    Case "RotateLeft"
                        eraseSketch(getRectangle(startPoint.X, startPoint.Y, lastPoint.X, lastPoint.Y))
                        DrawWithTool(propSketchTool, CInt(startPoint.X), CInt(startPoint.Y), dragPoint.X, dragPoint.Y)
                        SketchPanel.Invalidate(getLargestRectangle(startPoint.X, lastPoint.X, dragPoint.X, startPoint.Y, lastPoint.Y, dragPoint.Y))
                    Case "RotateRight"
                        eraseSketch(getRectangle(startPoint.X, startPoint.Y, lastPoint.X, lastPoint.Y))
                        DrawWithTool(propSketchTool, CInt(startPoint.X), CInt(startPoint.Y), dragPoint.X, dragPoint.Y)
                        SketchPanel.Invalidate(getLargestRectangle(startPoint.X, lastPoint.X, dragPoint.X, startPoint.Y, lastPoint.Y, dragPoint.Y))
                    Case "Text"

                        Dim s As SizeF
                        s = g.MeasureString(propDrawText, propDrawFont)
                        r = New Rectangle(lastPoint.X, lastPoint.Y, s.Width, s.Height)
                        eraseSketch(r)
                        DrawWithTool(propSketchTool, CInt(startPoint.X), CInt(startPoint.Y), dragPoint.X, dragPoint.Y)
                        'SketchPanel.Invalidate(getLargestRectangle(startPoint.X, lastPoint.X, dragPoint.X + s.Width, startPoint.Y, lastPoint.Y, dragPoint.Y + s.Height))
                        SketchPanel.Invalidate(getLargestRectangle(lastPoint.X, lastPoint.X + s.Width, dragPoint.X + s.Width, lastPoint.Y, lastPoint.Y + s.Height, dragPoint.Y + s.Height))
                    Case "Stamp"

                        r = New Rectangle(lastPoint.X, lastPoint.Y, propSketchBrush.Image.Width, propSketchBrush.Image.Height)
                        eraseSketch(r)
                        DrawWithTool(propSketchTool, CInt(startPoint.X), CInt(startPoint.Y), dragPoint.X, dragPoint.Y)
                        SketchPanel.Invalidate(getLargestRectangle(lastPoint.X, lastPoint.X + propSketchBrush.Image.Width, dragPoint.X + propSketchBrush.Image.Width, lastPoint.Y, lastPoint.Y + propSketchBrush.Image.Height, dragPoint.Y + propSketchBrush.Image.Height))
                        'SketchPanel.Invalidate(getRectangle(dragPoint.X, dragPoint.Y, dragPoint.X + propSketchBrush.Image.Width, dragPoint.Y + propSketchBrush.Image.Height))
                    Case Else
                        'eraseSketch(getRectangle(startPoint.X, startPoint.Y, lastPoint.X, lastPoint.Y))
                        eraseSketch(getScaledRectangle(clipMinX, clipMinY, clipMaxX, clipMaxY))
                        clipMinXOld = clipMinX : clipMinYOld = clipMinY : clipMaxXOld = clipMaxX : clipMaxYOld = clipMaxY
                        drawWithSymmetry(CInt(startPoint.X), CInt(startPoint.Y), dragPoint.X, dragPoint.Y)
                        If propSketchMode = "Chart" Then
                            Select Case propSketchTool
                                Case "Vertical", "Horizontal", "Line"
                                    SizeLabel.Text = getDiagonalMeasurement(startPoint.X, startPoint.Y, dragPoint.X, dragPoint.Y)

                                Case Else
                                    SizeLabel.Text = getBoxMeasurement(startPoint.X, startPoint.Y, dragPoint.X, dragPoint.Y)
                            End Select
                            chalkMsg = getRowStitchesMeasurement(startPoint.X, startPoint.Y, dragPoint.X, dragPoint.Y)
                            SketchPanel.Invalidate()
                        Else
                            SketchPanel.Invalidate(getScaledRectangle(Math.Min(clipMinX, clipMinXOld), Math.Min(clipMinY, clipMinYOld), Math.Max(clipMaxX, clipMaxXOld), Math.Max(clipMaxY, clipMaxYOld)))
                        End If

                End Select
                lastPoint = dragPoint
            Else
            End If
            If propSketchMode = "Chart" Then
                If dragPoint.X < 90 Then
                    LocationLabel.Text = "L" & (90 - dragPoint.X).ToString
                Else
                    LocationLabel.Text = "R" & (dragPoint.X - 89).ToString
                End If


            Else
                LocationLabel.Text = Str(dragPoint.X) & " ," & Str(dragPoint.Y)
            End If

        End If
    End Sub
    Private Function getLargestRectangle(ByVal x1 As Integer, ByVal x2 As Integer, ByVal x3 As Integer, ByVal y1 As Integer, ByVal y2 As Integer, ByVal y3 As Integer) As Rectangle
        Dim minX, minY, maxX, maxY As Integer
        Dim w, h As Integer
        minX = Math.Min(Math.Min(x1, x2), x3)
        minY = Math.Min(Math.Min(y1, y2), y3)
        maxX = Math.Max(Math.Max(x1, x2), x3)
        maxY = Math.Max(Math.Max(y1, y2), y3)

        minX = minX - LeftPen.Width
        minY = minY - LeftPen.Width

        w = maxX - minX + 1 + LeftPen.Width
        h = maxY - minY + 1 + LeftPen.Width


        getLargestRectangle = New Rectangle(minX * Magnification.Value, minY * Magnification.Value / AspectRatio.Value, w * Magnification.Value, h * Magnification.Value / AspectRatio.Value)
    End Function
    Private Function getScaledRectangle(ByVal x1 As Integer, ByVal y1 As Integer, ByVal x2 As Integer, ByVal y2 As Integer) As Rectangle
        Dim minX, minY, maxX, maxY As Integer
        Dim w, h As Integer
        minX = Math.Min(x1, x2)
        minY = Math.Min(y1, y2)
        maxX = Math.Max(x1, x2)
        maxY = Math.Max(y1, y2)

        minX = minX - LeftPen.Width
        minY = minY - LeftPen.Width

        w = maxX - minX + 1 + LeftPen.Width
        h = maxY - minY + 1 + LeftPen.Width


        getScaledRectangle = New Rectangle(minX * Magnification.Value, minY * Magnification.Value / AspectRatio.Value, w * Magnification.Value, h * Magnification.Value / AspectRatio.Value)
    End Function
    Private Sub SketchPanel_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles SketchPanel.MouseUp
        If Not TileView Then

            If dragging Then
                'If propSketchTool <> "Brush" Then
                endPoint = getCoordinates(e.X, e.Y)
                dragging = False

                Select Case propSketchTool
                    Case "Brush"
                    Case "Dropper"
                        Dim srcRect As Rectangle
                        Dim bmap As Bitmap
                        Dim gbmap As Graphics
                        srcRect = getRectangle(startPoint.X, startPoint.Y, lastPoint.X, lastPoint.Y)
                        eraseSketch(srcRect)
                        srcRect.Width = srcRect.Width + 1
                        srcRect.Height = srcRect.Height + 1
                        bmap = New Bitmap(srcRect.Width, srcRect.Height)
                        gbmap = Graphics.FromImage(bmap)
                        gbmap.DrawImage(eraseBitmap, 0, 0, srcRect, GraphicsUnit.Pixel)
                        RaiseEvent selectionChanged(bmap, e.Button)
                    Case "Lasso"
                        If Not Clip Then
                            propSketchTool = OldTool


                            StitchPatRect = getRectangle(startPoint.X, startPoint.Y, lastPoint.X, lastPoint.Y)
                            StitchPatRect.Width = StitchPatRect.Width + 1
                            StitchPatRect.Height = StitchPatRect.Height + 1
                            eraseSketch(StitchPatRect)
                            StitchPatCenter.X = StitchPatRect.X + StitchPatRect.Width / 2
                            StitchPatCenter.Y = StitchPatRect.Y + StitchPatRect.Height / 2
                            LassoButton.ImageIndex = 1
                            LassoButton.Invalidate()
                            SizeLabel.Text = Str(StitchPatRect.Width) & " x" & Str(StitchPatRect.Height)
                        Else
                            StitchPatRect = getRectangle(startPoint.X, startPoint.Y, lastPoint.X, lastPoint.Y)
                            StitchPatRect.Width = StitchPatRect.Width + 1
                            StitchPatRect.Height = StitchPatRect.Height + 1
                            eraseSketch(StitchPatRect)
                            Dim gbmap As Graphics
                            Dim bmap As Bitmap
                            bmap = New Bitmap(StitchPatRect.Width, StitchPatRect.Height)
                            gbmap = Graphics.FromImage(bmap)
                            gbmap.DrawImage(eraseBitmap, 0, 0, StitchPatRect, GraphicsUnit.Pixel)
                            propSketch = bmap
                            
                            initializeSketch("")

                            SketchPanel.Invalidate()
                        End If
                    Case "Measure"
                        eraseSketch(StitchPatRect)
                    Case "Chalk"
                        eraseSketch(StitchPatRect)
                        chalkMarks.AddLine(CSng(startPoint.X - 0.5), CSng(startPoint.Y - 0.5), CSng(startPoint.X + 0.5), CSng(startPoint.Y + 0.5))
                        chalkMarks.CloseFigure()
                        chalkMarks.AddLine(CSng(startPoint.X + 0.5), CSng(startPoint.Y - 0.5), CSng(startPoint.X - 0.5), CSng(startPoint.Y + 0.5))
                        chalkMarks.CloseFigure()
                        chalkMarks.AddLine(startPoint, lastPoint)
                        chalkMarks.CloseFigure()
                        chalkMarks.AddLine(CSng(lastPoint.X - 0.5), CSng(lastPoint.Y - 0.5), CSng(lastPoint.X + 0.5), CSng(lastPoint.Y + 0.5))
                        chalkMarks.CloseFigure()
                        chalkMarks.AddLine(CSng(lastPoint.X + 0.5), CSng(lastPoint.Y - 0.5), CSng(lastPoint.X - 0.5), CSng(lastPoint.Y + 0.5))
                        chalkMarks.CloseFigure()
                    Case "Mirror"
                        eraseSketch(getRectangle(startPoint.X, startPoint.Y, lastPoint.X, lastPoint.Y))
                        DrawWithTool(propSketchTool, CInt(startPoint.X), CInt(startPoint.Y), lastPoint.X, lastPoint.Y)
                    Case "Flip"
                        eraseSketch(getRectangle(startPoint.X, startPoint.Y, lastPoint.X, lastPoint.Y))
                        DrawWithTool(propSketchTool, CInt(startPoint.X), CInt(startPoint.Y), lastPoint.X, lastPoint.Y)
                    Case "RotateLeft"
                        eraseSketch(getRectangle(startPoint.X, startPoint.Y, lastPoint.X, lastPoint.Y))
                        DrawWithTool(propSketchTool, CInt(startPoint.X), CInt(startPoint.Y), lastPoint.X, lastPoint.Y)
                    Case "RotateRight"
                        eraseSketch(getRectangle(startPoint.X, startPoint.Y, lastPoint.X, lastPoint.Y))
                        DrawWithTool(propSketchTool, CInt(startPoint.X), CInt(startPoint.Y), lastPoint.X, lastPoint.Y)
                    Case Else
                        'eraseSketch(getRectangle(startPoint.X, startPoint.Y, lastPoint.X, lastPoint.Y))
                        eraseSketch(getScaledRectangle(clipMinX, clipMinY, clipMaxX, clipMaxY))
                        drawWithSymmetry(startPoint.X, startPoint.Y, endPoint.X, endPoint.Y)
                End Select

                SketchPanel.Invalidate()
            End If
        End If
    End Sub
    Private Function getRectangle(ByVal X1 As Integer, ByVal Y1 As Integer, ByVal X2 As Integer, ByVal Y2 As Integer) As Rectangle
        Dim w, h, x, y As Integer
        x = Math.Min(X1, X2)
        y = Math.Min(Y1, Y2)
        w = Math.Abs((X2 - X1)) + 1
        h = Math.Abs((Y2 - Y1)) + 1
        getRectangle = New Rectangle(x, y, w, h)
    End Function
    Private Sub SketchPanel_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles SketchPanel.Paint
        If Not propClipMode Then
            If Not TileView Then
                ' Create a GraphicsPath object.


                Dim clipx, clipy As Integer          ', srcX, srcY, srcW, srcH, destX, destY, destW, destH As Integer


                clipx = Int(e.ClipRectangle.X / Magnification.Value) * Magnification.Value
                clipy = Int(e.ClipRectangle.Y / (Magnification.Value / AspectRatio.Value)) * (Magnification.Value / AspectRatio.Value)

                Dim w, h As Integer
                Dim sourceRect, destinationRect As Rectangle

                e.Graphics.PixelOffsetMode = Drawing2D.PixelOffsetMode.Half
                e.Graphics.SmoothingMode = Drawing2D.SmoothingMode.None
                e.Graphics.InterpolationMode = Drawing2D.InterpolationMode.NearestNeighbor



                If propMotif <> "" Or propSketchMode = "Chart" Then

                    w = propSketch.Width
                    h = propSketch.Height
                    sourceRect = New Rectangle(0, 0, w, h)
                    destinationRect = New Rectangle(0, 0, w * Magnification.Value, h * Magnification.Value / AspectRatio.Value)
                    e.Graphics.DrawImage(propSketch, destinationRect, sourceRect, GraphicsUnit.Pixel)

                    If Not IsNothing(propTemplate) Then
                        w = propTemplate.Width
                        h = propTemplate.Height
                        sourceRect = New Rectangle(0, 0, w, h)
                        destinationRect = New Rectangle(0, 0, w * Magnification.Value, h * Magnification.Value / AspectRatio.Value)
                        e.Graphics.DrawImage(propTemplate, destinationRect, sourceRect, GraphicsUnit.Pixel)
                        ' e.Graphics.DrawEllipse(Pens.Black, destinationRect)
                        'e.Graphics.DrawEllipse(Pens.Black, sourceRect)
                    End If

                    gridPen.DashStyle = Drawing2D.DashStyle.Solid

                    If GuidelinesButton.ImageIndex = 0 Then

                        For i As Integer = 0 To e.ClipRectangle.Width
                            e.Graphics.DrawLine(gridPen, Int(i * Magnification.Value + clipx), clipy, Int(i * Magnification.Value + clipx), Int(clipy + e.ClipRectangle.Height))
                        Next
                        For i As Integer = 0 To e.ClipRectangle.Height
                            e.Graphics.DrawLine(gridPen, clipx, Int(i * Magnification.Value / AspectRatio.Value + clipy), Int(clipx + e.ClipRectangle.Width), Int(i * Magnification.Value / AspectRatio.Value + clipy))
                        Next

                    End If



                    If propSketchMode = "Chart" Then
                        'draw midline
                        e.Graphics.DrawLine(Pens.Red, (90) * Magnification.Value, 0, 90 * Magnification.Value, PaperLength * Magnification.Value / AspectRatio.Value)
                        'draw chalk lines
                        Dim magChalkMarks As Drawing2D.GraphicsPath
                        Dim m As Drawing2D.Matrix
                        magChalkMarks = chalkMarks.Clone
                        m = New Drawing2D.Matrix(1, 0, 0, 1, 0, 0)
                        m.Translate(0.5, 0.5) '(Magnification.Value / 2, Magnification.Value / AspectRatio.Value / 2)
                        magChalkMarks.Transform(m)
                        m.Dispose()
                        m = New Drawing2D.Matrix(1, 0, 0, 1, 0, 0)
                        m.Scale(Magnification.Value, Magnification.Value / AspectRatio.Value)
                        magChalkMarks.Transform(m)
                        If propSketchTool = "Measure" Or propSketchTool = "Chalk" Then
                            e.Graphics.DrawString(chalkMsg, chalkFont, chalkBrush, 0, -Me.SplitContainer1.Panel1.AutoScrollPosition.Y)
                        End If

                        e.Graphics.DrawPath(chalkPen, magChalkMarks)
                    Else
                        'draw stitch pattern area
                        e.Graphics.DrawRectangle(gridPen, StitchPatRect.X * Magnification.Value, StitchPatRect.Y * Magnification.Value / AspectRatio.Value, StitchPatRect.Width * Magnification.Value, StitchPatRect.Height * Magnification.Value / AspectRatio.Value)
                    End If


                End If
            Else

                Dim tbrush As TextureBrush
                Dim bmap As Bitmap
                Dim g2 As Graphics

                Dim w, h As Integer
                Dim sourceRect, destinationRect As Rectangle
                w = StitchPatRect.Width
                h = StitchPatRect.Height
                sourceRect = New Rectangle(0, 0, w, h)
                destinationRect = New Rectangle(0, 0, w * Magnification.Value, h * Magnification.Value / AspectRatio.Value)
                bmap = New Bitmap(destinationRect.Width, destinationRect.Height)

                g2 = Graphics.FromImage(bmap)
                g2.PixelOffsetMode = Drawing2D.PixelOffsetMode.Half
                g2.SmoothingMode = Drawing2D.SmoothingMode.None
                g2.InterpolationMode = Drawing2D.InterpolationMode.NearestNeighbor
                g2.DrawImage(propSketch, destinationRect, StitchPatRect, GraphicsUnit.Pixel)

                e.Graphics.PixelOffsetMode = Drawing2D.PixelOffsetMode.Half
                e.Graphics.SmoothingMode = Drawing2D.SmoothingMode.None
                e.Graphics.InterpolationMode = Drawing2D.InterpolationMode.NearestNeighbor
                '            e.Graphics.ScaleTransform(Magnification.Value, Magnification.Value / AspectRatio.Value)

                tbrush = New TextureBrush(bmap, Drawing2D.WrapMode.Tile)
                e.Graphics.FillRectangle(tbrush, SketchPanel.ClientRectangle)

                tbrush.Dispose()
                bmap.Dispose()

            End If
        Else
            Dim w, h As Integer
            Dim sourceRect, destinationRect As Rectangle
            w = propSketch.Width
            h = propSketch.Height
            sourceRect = New Rectangle(0, 0, w, h)
            destinationRect = New Rectangle(0, 0, w * Magnification.Value, h * Magnification.Value / AspectRatio.Value)
            e.Graphics.DrawImage(propSketch, destinationRect, sourceRect, GraphicsUnit.Pixel)

            If GuidelinesButton.ImageIndex = 0 Then
                gridPen.DashStyle = Drawing2D.DashStyle.Solid
                Dim clipx, clipy As Integer
                clipx = Int(e.ClipRectangle.X / Magnification.Value) * Magnification.Value
                clipy = Int(e.ClipRectangle.Y / (Magnification.Value / AspectRatio.Value)) * (Magnification.Value / AspectRatio.Value)


                For i As Integer = 0 To e.ClipRectangle.Width
                    e.Graphics.DrawLine(gridPen, Int(i * Magnification.Value + clipx), clipy, Int(i * Magnification.Value + clipx), Int(clipy + e.ClipRectangle.Height))
                Next
                For i As Integer = 0 To e.ClipRectangle.Height
                    e.Graphics.DrawLine(gridPen, clipx, Int(i * Magnification.Value / AspectRatio.Value + clipy), Int(clipx + e.ClipRectangle.Width), Int(i * Magnification.Value / AspectRatio.Value + clipy))
                Next

            End If

        End If
    End Sub
    Private Sub DrawWithTool(ByVal tName As String, ByVal X1 As Integer, ByVal Y1 As Integer, ByVal X2 As Integer, ByVal Y2 As Integer)
        Dim rect As Rectangle
        Dim im2 As Bitmap
        Dim g2 As Graphics
        rect = getRectangle(X1, Y1, X2, Y2)
        Select Case tName
            Case "Brush"
                'rect = New Rectangle(X2 - 2, Y2 - 2, 4, 4)
                'g.DrawLine(propSketchPen, X1, Y1, X2, Y2) 'g.DrawRectangle(propSketchPen, rect)
                If propSketchPen.Width > 1 Then
                    g.FillEllipse(propSketchBrush, X1 - propSketchPen.Width / 2, Y1 - propSketchPen.Width / 2, propSketchPen.Width, propSketchPen.Width)
                Else
                    g.FillRectangle(propSketchBrush, X1 - propSketchPen.Width / 2, Y1 - propSketchPen.Width / 2, propSketchPen.Width, propSketchPen.Width)
                End If

            Case "Line"
                g.DrawLine(propSketchPen, X1, Y1, X2, Y2)
            Case "Vertical"
                g.DrawLine(propSketchPen, X1, Y1, X1, Y2)
            Case "Horizontal"
                g.DrawLine(propSketchPen, X1, Y1, X2, Y1)
            Case "Ellipse"
                g.DrawEllipse(propSketchPen, rect)

            Case "Rectangle"
                g.DrawRectangle(propSketchPen, rect)
                If FillMode = "Resize" Then

                End If
            Case "FilledEllipse"
                g.FillEllipse(propSketchBrush, rect)
            Case "FilledRectangle"
                If FillMode = "Resize" Then

                    rect.Width = rect.Width + 1
                    rect.Height = rect.Height + 1
                    im2 = New Bitmap(rect.Width, rect.Height)
                    g2 = Graphics.FromImage(im2)
                    g2.SmoothingMode = Drawing2D.SmoothingMode.None
                    g2.InterpolationMode = Drawing2D.InterpolationMode.NearestNeighbor
                    g2.DrawImage(propSketchBrush.Image, New Rectangle(0, 0, rect.Width, rect.Height))
                    g.DrawImage(im2, Math.Min(X1, X2), Math.Min(Y1, Y2))
                    g2.Dispose()
                    im2.Dispose()
                Else
                    rect.Width = rect.Width + 1
                    rect.Height = rect.Height + 1
                    g.FillRectangle(propSketchBrush, rect)
                End If
            Case "Stamp"
                g.DrawImage(propSketchBrush.Image, X2, Y2)
            Case "Text"
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit
                g.DrawString(propDrawText, propDrawFont, propSketchBrush, X2, Y2)

            Case "CurvedText"
                Dim gp As Drawing2D.GraphicsPath
                gp = getTextPath(propDrawText, propDrawFont, X1, Y1, X2, Y2, True)
                setClipAreaForPath(gp.PathData)
                g.DrawPath(propSketchPen, gp)
                g.FillPath(propSketchBrush, gp)
                gp.Dispose()
            Case "SlantedText"
                Dim gp As Drawing2D.GraphicsPath
                gp = getTextPath(propDrawText, propDrawFont, X1, Y1, X2, Y2, False)
                setClipAreaForPath(gp.PathData)
                g.DrawPath(propSketchPen, gp)
                g.FillPath(propSketchBrush, gp)
                gp.Dispose()
            Case "Dropper"
                g.DrawRectangle(selectPen, rect)
            Case "Lasso"
                g.DrawRectangle(selectPen, rect)
            Case "Measure", "Chalk"
                g.DrawLine(gridPen, X1, Y1, X2, Y2)

            Case "RotateLeft"
                If dragging Then
                    g.DrawRectangle(selectPen, rect)
                Else
                    rect.Width = rect.Width + 1
                    rect.Height = rect.Height + 1
                    im2 = New Bitmap(rect.Width, rect.Height)
                    g2 = Graphics.FromImage(im2)
                    'g2.PixelOffsetMode = Drawing2D.PixelOffsetMode.Half
                    g2.SmoothingMode = Drawing2D.SmoothingMode.None
                    g2.InterpolationMode = Drawing2D.InterpolationMode.NearestNeighbor
                    g2.DrawImage(propSketch, 0, 0, rect, GraphicsUnit.Pixel)
                    im2.RotateFlip(RotateFlipType.Rotate270FlipNone)
                    g.DrawImage(im2, X1, Y1)
                    If rect.Height > rect.Width Then
                        g.FillRectangle(SketchBrush, X1, Y1 + rect.Width, rect.Width, rect.Height - rect.Width)
                    End If
                    If rect.Width > rect.Height Then
                        g.FillRectangle(SketchBrush, X1 + rect.Height, Y1, rect.Width - rect.Height, rect.Height)

                    End If
                    g2.Dispose()
                    im2.Dispose()
                End If
            Case "RotateRight"
                If dragging Then
                    g.DrawRectangle(selectPen, rect)
                Else
                    rect.Width = rect.Width + 1
                    rect.Height = rect.Height + 1
                    im2 = New Bitmap(rect.Width, rect.Height)
                    g2 = Graphics.FromImage(im2)
                    'g2.PixelOffsetMode = Drawing2D.PixelOffsetMode.Half
                    g2.SmoothingMode = Drawing2D.SmoothingMode.None
                    g2.InterpolationMode = Drawing2D.InterpolationMode.NearestNeighbor
                    g2.DrawImage(propSketch, 0, 0, rect, GraphicsUnit.Pixel)
                    im2.RotateFlip(RotateFlipType.Rotate90FlipNone)
                    g.DrawImage(im2, X1, Y1)
                    g2.Dispose()
                    im2.Dispose()
                    If rect.Height > rect.Width Then
                        g.FillRectangle(SketchBrush, X1, Y1 + rect.Width, rect.Width, rect.Height - rect.Width)
                    End If
                    If rect.Width > rect.Height Then
                        g.FillRectangle(SketchBrush, X1 + rect.Height, Y1, rect.Width - rect.Height, rect.Height)

                    End If
                End If
            Case "Flip"
                If dragging Then
                    g.DrawRectangle(selectPen, rect)
                Else
                    rect.Width = rect.Width + 1
                    rect.Height = rect.Height + 1
                    im2 = New Bitmap(rect.Width, rect.Height)
                    g2 = Graphics.FromImage(im2)
                    'g2.PixelOffsetMode = Drawing2D.PixelOffsetMode.Half
                    g2.SmoothingMode = Drawing2D.SmoothingMode.None
                    g2.InterpolationMode = Drawing2D.InterpolationMode.NearestNeighbor
                    g2.DrawImage(propSketch, 0, 0, rect, GraphicsUnit.Pixel)
                    im2.RotateFlip(RotateFlipType.RotateNoneFlipY)
                    g.DrawImage(im2, X1, Y1)
                    g2.Dispose()
                    im2.Dispose()
                End If
            Case "Mirror"
                If dragging Then
                    g.DrawRectangle(selectPen, rect)
                Else
                    rect.Width = rect.Width + 1
                    rect.Height = rect.Height + 1
                    im2 = New Bitmap(rect.Width, rect.Height)
                    g2 = Graphics.FromImage(im2)
                    'g2.PixelOffsetMode = Drawing2D.PixelOffsetMode.Half
                    g2.SmoothingMode = Drawing2D.SmoothingMode.None
                    g2.InterpolationMode = Drawing2D.InterpolationMode.NearestNeighbor
                    g2.DrawImage(propSketch, 0, 0, rect, GraphicsUnit.Pixel)
                    im2.RotateFlip(RotateFlipType.RotateNoneFlipX)
                    g.DrawImage(im2, X1, Y1)
                    g2.Dispose()
                    im2.Dispose()
                End If
            Case "Flood"
                UnsafeFloodFill(X2, Y2)

        End Select


    End Sub
    Private Sub setEraseBitmap() 'ByVal X1 As Integer, ByVal Y1 As Integer, ByVal x2 As Integer, ByVal y2 As Integer)
        If Not IsNothing(eraseBitmap) Then
            eraseBitmap.Dispose()
        End If
        eraseBitmap = New Bitmap(propSketch)
    End Sub
    Private Sub eraseSketch(ByVal rect As Rectangle)

        g.DrawImage(eraseBitmap, 0, 0)
    End Sub
    Private Sub Magnification_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Magnification.ValueChanged
        Dim P As New Point
        Dim OldPosX As Double
        Dim OldPosY As Double

        P = Me.SplitContainer1.Panel1.AutoScrollPosition
        OldPosY = -P.Y / (Me.SketchPanel.Height - Me.SplitContainer1.Panel1.Height)
        OldPosX = -P.X / (Me.SketchPanel.Width - Me.SplitContainer1.Panel1.Width)
        Me.SketchPanel.Width = PaperWidth * Magnification.Value

        If AspectRatio.Value <> 0 Then
            Me.SketchPanel.Height = PaperLength * (Magnification.Value / AspectRatio.Value)
        Else
            Me.SketchPanel.Height = PaperLength * Magnification.Value
        End If

        Me.SplitContainer1.Panel1.AutoScrollPosition = New Point((Me.SketchPanel.Width - Me.SplitContainer1.Panel1.Width) * OldPosX, (Me.SketchPanel.Height - Me.SplitContainer1.Panel1.Height) * OldPosY)

        SketchPanel.Invalidate()
    End Sub
    Private Sub AspectRatio_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AspectRatio.ValueChanged
        Dim P As New Point
        Dim OldPosX As Double
        Dim OldPosY As Double

        P = Me.SplitContainer1.Panel1.AutoScrollPosition
        OldPosY = -P.Y / (Me.SketchPanel.Height - Me.SplitContainer1.Panel1.Height)
        OldPosX = -P.X / (Me.SketchPanel.Width - Me.SplitContainer1.Panel1.Width)

        Me.SketchPanel.Width = PaperWidth * Magnification.Value
        Me.SketchPanel.Height = PaperLength * Magnification.Value / AspectRatio.Value

        Me.SplitContainer1.Panel1.AutoScrollPosition = New Point((Me.SketchPanel.Width - Me.SplitContainer1.Panel1.Width) * OldPosX, (Me.SketchPanel.Height - Me.SplitContainer1.Panel1.Height) * OldPosY)

        SketchPanel.Invalidate()
    End Sub

    Private Function motifToBitmap(ByVal motif As String) As Bitmap
        Dim width As Integer
        Dim height As Integer
        Dim aMotif As String
        Dim i, j, k, l As Integer
        Dim aBitmap As Bitmap
        Dim g2 As Graphics
        Dim b1 As SolidBrush

        Dim rows() As String
        Dim rowcount, rc As Integer
        Dim needles(4) As Integer
        Dim Row() As String
        Dim instruction As String
        Dim verticalOffset As Integer

        b1 = New SolidBrush(ColorScheme.Colors(0))
        aBitmap = New Bitmap(PaperWidth, PaperLength)
        g2 = Graphics.FromImage(aBitmap)
        g2.FillRectangle(b1, 0, 0, PaperWidth, PaperLength)
        g2.Dispose()
        b1.Dispose()

        If propSketchMode = "Chart" Then
            If motif.StartsWith("<chart>") Then
                i = motif.IndexOf("RZ")
                If i > -1 Then
                    propRowz = Val(motif.Substring(i + 2, 4))
                Else
                    propRowz = 2
                End If
                i = motif.IndexOf("RT")
                If i > -1 Then
                    rowcount = Val(motif.Substring(i + 2, 4)) / propRowz
                    verticalOffset = Int(500 - rowcount / 2)

                Else
                    motifToBitmap = aBitmap
                    Exit Function
                End If

                rows = motif.Replace(Chr(10), "").Split(Chr(13))
                j = 1
                i = 0
                Row = rows(j).Split(" ")

                If rows(j).Contains("change pattern") Then
                    instruction = "change"
                ElseIf rows(j).Contains("ref") Then
                    instruction = "ref"
                Else
                    instruction = "black"
                    For k = 1 To Row.Length - 1
                        If IsNumeric(Row(k)) Then
                            needles(k - 1) = CInt(Row(k))
                        End If
                    Next k
                End If

                Do Until i > rowcount
                    Select Case instruction
                        Case "black"
                            For l = needles(0) To needles(1)
                                aBitmap.SetPixel(l - 1, verticalOffset + rowcount - i, Color.Black)
                            Next l
                            If needles(2) <> 0 Or needles(3) <> 0 Then
                                For l = needles(2) To needles(3)
                                    aBitmap.SetPixel(l - 1, verticalOffset + rowcount - i, Color.Black)
                                Next l
                            End If
                        Case "change"
                            For l = needles(0) To needles(1)
                                aBitmap.SetPixel(l - 1, verticalOffset + rowcount - i, Color.Gray)
                            Next l
                            instruction = "black"
                        Case "ref"
                            For l = needles(0) To needles(1)
                                aBitmap.SetPixel(l - 1, verticalOffset + rowcount - i, Color.LightCoral)
                            Next l
                            instruction = "black"
                    End Select


                    i = i + 1
                    rc = rows(j + 1).Substring(2, 4) / propRowz
                    If rc = i Then
                        j = j + 1
                        Row = rows(j).Split(" ")
                        If rows(j).Contains("change pattern") Then
                            instruction = "change"
                        ElseIf rows(j).Contains("ref") Then
                            instruction = "ref"
                        Else
                            instruction = "black"
                            For k = 1 To Row.Length - 1
                                If IsNumeric(Row(k)) Then
                                    needles(k - 1) = CInt(Row(k))
                                End If
                            Next k
                        End If
                    End If
                Loop
            End If

        Else

            width = Val(Mid(motif, 3, 3))
            height = Val(Mid(motif, 7, 3))
            aMotif = Mid(motif, 11)

            'aBitmap = New Bitmap(width, height)


            For j = 0 To height - 1
                For i = 0 To width - 1
                    aBitmap.SetPixel(i, j, ColorScheme.Colors((Val(Mid(aMotif, j * width + i + 1, 1)))))
                Next i
            Next j
        End If

        motifToBitmap = aBitmap

    End Function

    Private Function getCoordinates(ByVal x As Integer, ByVal y As Integer) As Point
        getCoordinates.X = Int(x / Me.Magnification.Value)
        getCoordinates.Y = Int(y / Me.Magnification.Value * AspectRatio.Value)
        'getCoordinates.X = Int((x + Me.Magnification.Value) / Me.Magnification.Value)
        'getCoordinates.Y = Int((y + Me.Magnification.Value / AspectRatio.Value) / Me.Magnification.Value * AspectRatio.Value)
    End Function

    Private Sub RotatePointAround(ByVal cx As Double, ByVal cy As Double, ByVal angle As Double, ByRef X As Double, ByRef Y As Double)
        Dim sin_angle As Double
        Dim cos_angle As Double
        Dim new_x As Double

        sin_angle = Math.Sin(angle)
        cos_angle = Math.Cos(angle)
        X = X - cx
        Y = Y - cy
        new_x = cx + X * cos_angle + Y * sin_angle
        Y = cy - X * sin_angle + Y * cos_angle
        X = new_x
    End Sub

    Private Sub LassoButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LassoButton.Click
        If Not propClipMode Then
            If propSketchTool <> "Lasso" Then
                OldTool = propSketchTool
                propSketchTool = "Lasso"
            End If

            LassoButton.ImageIndex = 0
        End If
    End Sub
    Private Sub drawWithSymmetry(ByVal x1 As Integer, ByVal y1 As Integer, ByVal x2 As Integer, ByVal y2 As Integer)
        Dim xa, xb, ya, yb As Double
        Dim alpha, beta, theta As Double

        clipMinX = Math.Min(x1, x2) : clipMinY = Math.Min(y1, y2) : clipMaxX = Math.Max(x1, x2) : clipMaxY = Math.Max(y1, y2)


        DrawWithTool(propSketchTool, x1, y1, x2, y2) 'case 0 and all cases


        Select Case PropSymmetry

            Case 1 'Vertical Kaleidoscope
                clipMinY = Math.Min(clipMinY, StitchPatCenter.Y * 2 - y1) : clipMinY = Math.Min(clipMinY, StitchPatCenter.Y * 2 - y2)
                clipMaxY = Math.Max(clipMaxY, StitchPatCenter.Y * 2 - y1) : clipMaxY = Math.Max(clipMaxY, StitchPatCenter.Y * 2 - y2)
                DrawWithTool(propSketchTool, x1, StitchPatCenter.Y * 2 - y1, x2, StitchPatCenter.Y * 2 - y2)

            Case 2 'Horizontal Kaleidoscope
                clipMinX = Math.Min(clipMinX, StitchPatCenter.X * 2 - x1) : clipMinX = Math.Min(clipMinX, StitchPatCenter.X * 2 - x2)
                clipMaxX = Math.Max(clipMaxX, StitchPatCenter.X * 2 - x1) : clipMaxX = Math.Max(clipMaxX, StitchPatCenter.X * 2 - x2)
                DrawWithTool(propSketchTool, StitchPatCenter.X * 2 - x1, y1, StitchPatCenter.X * 2 - x2, y2)

            Case 3 'Left Daigonal Kaleidoscope
                theta = Math.Atan2(StitchPatRect.Height, StitchPatRect.Width)
                alpha = Math.Atan2(y1 - StitchPatCenter.Y, x1 - StitchPatCenter.X)
                beta = Math.Atan2(y2 - StitchPatCenter.Y, x2 - StitchPatCenter.X)

                xa = x1 : xb = x2 : ya = StitchPatCenter.Y * 2 - y1 : yb = StitchPatCenter.Y * 2 - y2
                RotatePointAround(StitchPatCenter.X, StitchPatCenter.Y, Math.PI / 2, xa, ya)
                RotatePointAround(StitchPatCenter.X, StitchPatCenter.Y, Math.PI / 2, xb, yb)
                clipMinX = Math.Min(clipMinX, xa) : clipMinX = Math.Min(clipMinX, xb)
                clipMaxX = Math.Max(clipMaxX, xa) : clipMaxX = Math.Max(clipMaxX, xb)
                clipMinY = Math.Min(clipMinY, ya) : clipMinY = Math.Min(clipMinY, yb)
                clipMaxY = Math.Max(clipMaxY, ya) : clipMaxY = Math.Max(clipMaxY, yb)
                DrawWithTool(propSketchTool, xa, ya, xb, yb)

            Case 4 'Right Daigonal Kaleidoscope
                theta = Math.Atan2(StitchPatRect.Height, StitchPatRect.Width)
                alpha = Math.Atan2(y1 - StitchPatCenter.Y, x1 - StitchPatCenter.X)
                beta = Math.Atan2(y2 - StitchPatCenter.Y, x2 - StitchPatCenter.X)

                xa = StitchPatCenter.X * 2 - x1 : xb = StitchPatCenter.X * 2 - x2 : ya = y1 : yb = y2
                RotatePointAround(StitchPatCenter.X, StitchPatCenter.Y, Math.PI / 2, xa, ya)
                RotatePointAround(StitchPatCenter.X, StitchPatCenter.Y, Math.PI / 2, xb, yb)
                clipMinX = Math.Min(clipMinX, xa) : clipMinX = Math.Min(clipMinX, xb)
                clipMaxX = Math.Max(clipMaxX, xa) : clipMaxX = Math.Max(clipMaxX, xb)
                clipMinY = Math.Min(clipMinY, ya) : clipMinY = Math.Min(clipMinY, yb)
                clipMaxY = Math.Max(clipMaxY, ya) : clipMaxY = Math.Max(clipMaxY, yb)
                DrawWithTool(propSketchTool, xa, ya, xb, yb)

            Case 5 '4 Kaleidoscope (vert + horz)
                clipMinY = Math.Min(clipMinY, StitchPatCenter.Y * 2 - y1) : clipMinY = Math.Min(clipMinY, StitchPatCenter.Y * 2 - y2)
                clipMaxY = Math.Max(clipMaxY, StitchPatCenter.Y * 2 - y1) : clipMaxY = Math.Max(clipMaxY, StitchPatCenter.Y * 2 - y2)
                clipMinX = Math.Min(clipMinX, StitchPatCenter.X * 2 - x1) : clipMinX = Math.Min(clipMinX, StitchPatCenter.X * 2 - x2)
                clipMaxX = Math.Max(clipMaxX, StitchPatCenter.X * 2 - x1) : clipMaxX = Math.Max(clipMaxX, StitchPatCenter.X * 2 - x2)

                DrawWithTool(propSketchTool, x1, StitchPatCenter.Y * 2 - y1, x2, StitchPatCenter.Y * 2 - y2)
                DrawWithTool(propSketchTool, StitchPatCenter.X * 2 - x1, y1, StitchPatCenter.X * 2 - x2, y2)
                DrawWithTool(propSketchTool, StitchPatCenter.X * 2 - x1, StitchPatCenter.Y * 2 - y1, StitchPatCenter.X * 2 - x2, StitchPatCenter.Y * 2 - y2)


            Case 6 '8 Kaleidoscope
                theta = Math.Atan2(StitchPatRect.Height, StitchPatRect.Width)
                alpha = Math.Atan2(y1 - StitchPatCenter.Y, x1 - StitchPatCenter.X)
                beta = Math.Atan2(y2 - StitchPatCenter.Y, x2 - StitchPatCenter.X)

                clipMinY = Math.Min(clipMinY, StitchPatCenter.Y * 2 - y1) : clipMinY = Math.Min(clipMinY, StitchPatCenter.Y * 2 - y2)
                clipMaxY = Math.Max(clipMaxY, StitchPatCenter.Y * 2 - y1) : clipMaxY = Math.Max(clipMaxY, StitchPatCenter.Y * 2 - y2)
                clipMinX = Math.Min(clipMinX, StitchPatCenter.X * 2 - x1) : clipMinX = Math.Min(clipMinX, StitchPatCenter.X * 2 - x2)
                clipMaxX = Math.Max(clipMaxX, StitchPatCenter.X * 2 - x1) : clipMaxX = Math.Max(clipMaxX, StitchPatCenter.X * 2 - x2)

                xa = x1 : xb = x2 : ya = StitchPatCenter.Y * 2 - y1 : yb = StitchPatCenter.Y * 2 - y2
                RotatePointAround(StitchPatCenter.X, StitchPatCenter.Y, Math.PI / 2, xa, ya)
                RotatePointAround(StitchPatCenter.X, StitchPatCenter.Y, Math.PI / 2, xb, yb)
                clipMinX = Math.Min(clipMinX, xa) : clipMinX = Math.Min(clipMinX, xb)
                clipMaxX = Math.Max(clipMaxX, xa) : clipMaxX = Math.Max(clipMaxX, xb)
                clipMinY = Math.Min(clipMinY, ya) : clipMinY = Math.Min(clipMinY, yb)
                clipMaxY = Math.Max(clipMaxY, ya) : clipMaxY = Math.Max(clipMaxY, yb)
                DrawWithTool(propSketchTool, xa, ya, xb, yb)

                xa = x1 : xb = x2 : ya = StitchPatCenter.Y * 2 - y1 : yb = StitchPatCenter.Y * 2 - y2
                DrawWithTool(propSketchTool, xa, ya, xb, yb)
                RotatePointAround(StitchPatCenter.X, StitchPatCenter.Y, -Math.PI / 2, xa, ya)
                RotatePointAround(StitchPatCenter.X, StitchPatCenter.Y, -Math.PI / 2, xb, yb)
                clipMinX = Math.Min(clipMinX, xa) : clipMinX = Math.Min(clipMinX, xb)
                clipMaxX = Math.Max(clipMaxX, xa) : clipMaxX = Math.Max(clipMaxX, xb)
                clipMinY = Math.Min(clipMinY, ya) : clipMinY = Math.Min(clipMinY, yb)
                clipMaxY = Math.Max(clipMaxY, ya) : clipMaxY = Math.Max(clipMaxY, yb)
                DrawWithTool(propSketchTool, xa, ya, xb, yb)

                xa = StitchPatCenter.X * 2 - x1 : xb = StitchPatCenter.X * 2 - x2 : ya = y1 : yb = y2
                DrawWithTool(propSketchTool, xa, ya, xb, yb)


                xa = StitchPatCenter.X * 2 - x1 : xb = StitchPatCenter.X * 2 - x2 : ya = StitchPatCenter.Y * 2 - y1 : yb = StitchPatCenter.Y * 2 - y2
                DrawWithTool(propSketchTool, xa, ya, xb, yb)
                RotatePointAround(StitchPatCenter.X, StitchPatCenter.Y, Math.PI / 2, xa, ya)
                RotatePointAround(StitchPatCenter.X, StitchPatCenter.Y, Math.PI / 2, xb, yb)
                clipMinX = Math.Min(clipMinX, xa) : clipMinX = Math.Min(clipMinX, xb)
                clipMaxX = Math.Max(clipMaxX, xa) : clipMaxX = Math.Max(clipMaxX, xb)
                clipMinY = Math.Min(clipMinY, ya) : clipMinY = Math.Min(clipMinY, yb)
                clipMaxY = Math.Max(clipMaxY, ya) : clipMaxY = Math.Max(clipMaxY, yb)
                DrawWithTool(propSketchTool, xa, ya, xb, yb)

                xa = StitchPatCenter.X * 2 - x1 : xb = StitchPatCenter.X * 2 - x2 : ya = StitchPatCenter.Y * 2 - y1 : yb = StitchPatCenter.Y * 2 - y2
                RotatePointAround(StitchPatCenter.X, StitchPatCenter.Y, -Math.PI / 2, xa, ya)
                RotatePointAround(StitchPatCenter.X, StitchPatCenter.Y, -Math.PI / 2, xb, yb)
                clipMinX = Math.Min(clipMinX, xa) : clipMinX = Math.Min(clipMinX, xb)
                clipMaxX = Math.Max(clipMaxX, xa) : clipMaxX = Math.Max(clipMaxX, xb)
                clipMinY = Math.Min(clipMinY, ya) : clipMinY = Math.Min(clipMinY, yb)
                clipMaxY = Math.Max(clipMaxY, ya) : clipMaxY = Math.Max(clipMaxY, yb)
                DrawWithTool(propSketchTool, xa, ya, xb, yb)


            Case 7 '2 Pinwheel
                xa = x1 : xb = x2 : ya = y1 : yb = y2
                RotatePointAround(StitchPatCenter.X, StitchPatCenter.Y, Math.PI, xa, ya)
                RotatePointAround(StitchPatCenter.X, StitchPatCenter.Y, Math.PI, xb, yb)
                clipMinX = Math.Min(clipMinX, xa) : clipMinX = Math.Min(clipMinX, xb)
                clipMaxX = Math.Max(clipMaxX, xa) : clipMaxX = Math.Max(clipMaxX, xb)
                clipMinY = Math.Min(clipMinY, ya) : clipMinY = Math.Min(clipMinY, yb)
                clipMaxY = Math.Max(clipMaxY, ya) : clipMaxY = Math.Max(clipMaxY, yb)
                DrawWithTool(propSketchTool, xa, ya, xb, yb)
            Case 8 '3 Pinwheel
                xa = x1 : xb = x2 : ya = y1 : yb = y2
                RotatePointAround(StitchPatCenter.X, StitchPatCenter.Y, 2 * Math.PI / 3, xa, ya)
                RotatePointAround(StitchPatCenter.X, StitchPatCenter.Y, 2 * Math.PI / 3, xb, yb)
                clipMinX = Math.Min(clipMinX, xa) : clipMinX = Math.Min(clipMinX, xb)
                clipMaxX = Math.Max(clipMaxX, xa) : clipMaxX = Math.Max(clipMaxX, xb)
                clipMinY = Math.Min(clipMinY, ya) : clipMinY = Math.Min(clipMinY, yb)
                clipMaxY = Math.Max(clipMaxY, ya) : clipMaxY = Math.Max(clipMaxY, yb)
                DrawWithTool(propSketchTool, xa, ya, xb, yb)
                RotatePointAround(StitchPatCenter.X, StitchPatCenter.Y, 2 * Math.PI / 3, xa, ya)
                RotatePointAround(StitchPatCenter.X, StitchPatCenter.Y, 2 * Math.PI / 3, xb, yb)
                clipMinX = Math.Min(clipMinX, xa) : clipMinX = Math.Min(clipMinX, xb)
                clipMaxX = Math.Max(clipMaxX, xa) : clipMaxX = Math.Max(clipMaxX, xb)
                clipMinY = Math.Min(clipMinY, ya) : clipMinY = Math.Min(clipMinY, yb)
                clipMaxY = Math.Max(clipMaxY, ya) : clipMaxY = Math.Max(clipMaxY, yb)
                DrawWithTool(propSketchTool, xa, ya, xb, yb)
            Case 9 '4 Pinwheel
                xa = x1 : xb = x2 : ya = y1 : yb = y2
                RotatePointAround(StitchPatCenter.X, StitchPatCenter.Y, Math.PI / 2, xa, ya)
                RotatePointAround(StitchPatCenter.X, StitchPatCenter.Y, Math.PI / 2, xb, yb)
                clipMinX = Math.Min(clipMinX, xa) : clipMinX = Math.Min(clipMinX, xb)
                clipMaxX = Math.Max(clipMaxX, xa) : clipMaxX = Math.Max(clipMaxX, xb)
                clipMinY = Math.Min(clipMinY, ya) : clipMinY = Math.Min(clipMinY, yb)
                clipMaxY = Math.Max(clipMaxY, ya) : clipMaxY = Math.Max(clipMaxY, yb)
                DrawWithTool(propSketchTool, xa, ya, xb, yb)
                RotatePointAround(StitchPatCenter.X, StitchPatCenter.Y, Math.PI / 2, xa, ya)
                RotatePointAround(StitchPatCenter.X, StitchPatCenter.Y, Math.PI / 2, xb, yb)
                clipMinX = Math.Min(clipMinX, xa) : clipMinX = Math.Min(clipMinX, xb)
                clipMaxX = Math.Max(clipMaxX, xa) : clipMaxX = Math.Max(clipMaxX, xb)
                clipMinY = Math.Min(clipMinY, ya) : clipMinY = Math.Min(clipMinY, yb)
                clipMaxY = Math.Max(clipMaxY, ya) : clipMaxY = Math.Max(clipMaxY, yb)
                DrawWithTool(propSketchTool, xa, ya, xb, yb)
                RotatePointAround(StitchPatCenter.X, StitchPatCenter.Y, Math.PI / 2, xa, ya)
                RotatePointAround(StitchPatCenter.X, StitchPatCenter.Y, Math.PI / 2, xb, yb)
                clipMinX = Math.Min(clipMinX, xa) : clipMinX = Math.Min(clipMinX, xb)
                clipMaxX = Math.Max(clipMaxX, xa) : clipMaxX = Math.Max(clipMaxX, xb)
                clipMinY = Math.Min(clipMinY, ya) : clipMinY = Math.Min(clipMinY, yb)
                clipMaxY = Math.Max(clipMaxY, ya) : clipMaxY = Math.Max(clipMaxY, yb)
                DrawWithTool(propSketchTool, xa, ya, xb, yb)
            Case 10 '5 Pinwheel
                xa = x1 : xb = x2 : ya = y1 : yb = y2
                RotatePointAround(StitchPatCenter.X, StitchPatCenter.Y, 2 * Math.PI / 5, xa, ya)
                RotatePointAround(StitchPatCenter.X, StitchPatCenter.Y, 2 * Math.PI / 5, xb, yb)
                clipMinX = Math.Min(clipMinX, xa) : clipMinX = Math.Min(clipMinX, xb)
                clipMaxX = Math.Max(clipMaxX, xa) : clipMaxX = Math.Max(clipMaxX, xb)
                clipMinY = Math.Min(clipMinY, ya) : clipMinY = Math.Min(clipMinY, yb)
                clipMaxY = Math.Max(clipMaxY, ya) : clipMaxY = Math.Max(clipMaxY, yb)
                DrawWithTool(propSketchTool, xa, ya, xb, yb)
                RotatePointAround(StitchPatCenter.X, StitchPatCenter.Y, 2 * Math.PI / 5, xa, ya)
                RotatePointAround(StitchPatCenter.X, StitchPatCenter.Y, 2 * Math.PI / 5, xb, yb)
                clipMinX = Math.Min(clipMinX, xa) : clipMinX = Math.Min(clipMinX, xb)
                clipMaxX = Math.Max(clipMaxX, xa) : clipMaxX = Math.Max(clipMaxX, xb)
                clipMinY = Math.Min(clipMinY, ya) : clipMinY = Math.Min(clipMinY, yb)
                clipMaxY = Math.Max(clipMaxY, ya) : clipMaxY = Math.Max(clipMaxY, yb)
                DrawWithTool(propSketchTool, xa, ya, xb, yb)
                RotatePointAround(StitchPatCenter.X, StitchPatCenter.Y, 2 * Math.PI / 5, xa, ya)
                RotatePointAround(StitchPatCenter.X, StitchPatCenter.Y, 2 * Math.PI / 5, xb, yb)
                clipMinX = Math.Min(clipMinX, xa) : clipMinX = Math.Min(clipMinX, xb)
                clipMaxX = Math.Max(clipMaxX, xa) : clipMaxX = Math.Max(clipMaxX, xb)
                clipMinY = Math.Min(clipMinY, ya) : clipMinY = Math.Min(clipMinY, yb)
                clipMaxY = Math.Max(clipMaxY, ya) : clipMaxY = Math.Max(clipMaxY, yb)
                DrawWithTool(propSketchTool, xa, ya, xb, yb)
                RotatePointAround(StitchPatCenter.X, StitchPatCenter.Y, 2 * Math.PI / 5, xa, ya)
                RotatePointAround(StitchPatCenter.X, StitchPatCenter.Y, 2 * Math.PI / 5, xb, yb)
                clipMinX = Math.Min(clipMinX, xa) : clipMinX = Math.Min(clipMinX, xb)
                clipMaxX = Math.Max(clipMaxX, xa) : clipMaxX = Math.Max(clipMaxX, xb)
                clipMinY = Math.Min(clipMinY, ya) : clipMinY = Math.Min(clipMinY, yb)
                clipMaxY = Math.Max(clipMaxY, ya) : clipMaxY = Math.Max(clipMaxY, yb)
                DrawWithTool(propSketchTool, xa, ya, xb, yb)
            Case 11 '6 Pinwheel
                xa = x1 : xb = x2 : ya = y1 : yb = y2
                RotatePointAround(StitchPatCenter.X, StitchPatCenter.Y, Math.PI / 3, xa, ya)
                RotatePointAround(StitchPatCenter.X, StitchPatCenter.Y, Math.PI / 3, xb, yb)
                clipMinX = Math.Min(clipMinX, xa) : clipMinX = Math.Min(clipMinX, xb)
                clipMaxX = Math.Max(clipMaxX, xa) : clipMaxX = Math.Max(clipMaxX, xb)
                clipMinY = Math.Min(clipMinY, ya) : clipMinY = Math.Min(clipMinY, yb)
                clipMaxY = Math.Max(clipMaxY, ya) : clipMaxY = Math.Max(clipMaxY, yb)
                DrawWithTool(propSketchTool, xa, ya, xb, yb)
                RotatePointAround(StitchPatCenter.X, StitchPatCenter.Y, Math.PI / 3, xa, ya)
                RotatePointAround(StitchPatCenter.X, StitchPatCenter.Y, Math.PI / 3, xb, yb)
                clipMinX = Math.Min(clipMinX, xa) : clipMinX = Math.Min(clipMinX, xb)
                clipMaxX = Math.Max(clipMaxX, xa) : clipMaxX = Math.Max(clipMaxX, xb)
                clipMinY = Math.Min(clipMinY, ya) : clipMinY = Math.Min(clipMinY, yb)
                clipMaxY = Math.Max(clipMaxY, ya) : clipMaxY = Math.Max(clipMaxY, yb)
                DrawWithTool(propSketchTool, xa, ya, xb, yb)
                RotatePointAround(StitchPatCenter.X, StitchPatCenter.Y, Math.PI / 3, xa, ya)
                RotatePointAround(StitchPatCenter.X, StitchPatCenter.Y, Math.PI / 3, xb, yb)
                clipMinX = Math.Min(clipMinX, xa) : clipMinX = Math.Min(clipMinX, xb)
                clipMaxX = Math.Max(clipMaxX, xa) : clipMaxX = Math.Max(clipMaxX, xb)
                clipMinY = Math.Min(clipMinY, ya) : clipMinY = Math.Min(clipMinY, yb)
                clipMaxY = Math.Max(clipMaxY, ya) : clipMaxY = Math.Max(clipMaxY, yb)
                DrawWithTool(propSketchTool, xa, ya, xb, yb)
                RotatePointAround(StitchPatCenter.X, StitchPatCenter.Y, Math.PI / 3, xa, ya)
                RotatePointAround(StitchPatCenter.X, StitchPatCenter.Y, Math.PI / 3, xb, yb)
                clipMinX = Math.Min(clipMinX, xa) : clipMinX = Math.Min(clipMinX, xb)
                clipMaxX = Math.Max(clipMaxX, xa) : clipMaxX = Math.Max(clipMaxX, xb)
                clipMinY = Math.Min(clipMinY, ya) : clipMinY = Math.Min(clipMinY, yb)
                clipMaxY = Math.Max(clipMaxY, ya) : clipMaxY = Math.Max(clipMaxY, yb)
                DrawWithTool(propSketchTool, xa, ya, xb, yb)
                RotatePointAround(StitchPatCenter.X, StitchPatCenter.Y, Math.PI / 3, xa, ya)
                RotatePointAround(StitchPatCenter.X, StitchPatCenter.Y, Math.PI / 3, xb, yb)
                clipMinX = Math.Min(clipMinX, xa) : clipMinX = Math.Min(clipMinX, xb)
                clipMaxX = Math.Max(clipMaxX, xa) : clipMaxX = Math.Max(clipMaxX, xb)
                clipMinY = Math.Min(clipMinY, ya) : clipMinY = Math.Min(clipMinY, yb)
                clipMaxY = Math.Max(clipMaxY, ya) : clipMaxY = Math.Max(clipMaxY, yb)
                DrawWithTool(propSketchTool, xa, ya, xb, yb)
            Case 12 '8 Pinwheel
                xa = x1 : xb = x2 : ya = y1 : yb = y2
                RotatePointAround(StitchPatCenter.X, StitchPatCenter.Y, Math.PI / 4, xa, ya)
                RotatePointAround(StitchPatCenter.X, StitchPatCenter.Y, Math.PI / 4, xb, yb)
                clipMinX = Math.Min(clipMinX, xa) : clipMinX = Math.Min(clipMinX, xb)
                clipMaxX = Math.Max(clipMaxX, xa) : clipMaxX = Math.Max(clipMaxX, xb)
                clipMinY = Math.Min(clipMinY, ya) : clipMinY = Math.Min(clipMinY, yb)
                clipMaxY = Math.Max(clipMaxY, ya) : clipMaxY = Math.Max(clipMaxY, yb)
                DrawWithTool(propSketchTool, xa, ya, xb, yb)
                RotatePointAround(StitchPatCenter.X, StitchPatCenter.Y, Math.PI / 4, xa, ya)
                RotatePointAround(StitchPatCenter.X, StitchPatCenter.Y, Math.PI / 4, xb, yb)
                clipMinX = Math.Min(clipMinX, xa) : clipMinX = Math.Min(clipMinX, xb)
                clipMaxX = Math.Max(clipMaxX, xa) : clipMaxX = Math.Max(clipMaxX, xb)
                clipMinY = Math.Min(clipMinY, ya) : clipMinY = Math.Min(clipMinY, yb)
                clipMaxY = Math.Max(clipMaxY, ya) : clipMaxY = Math.Max(clipMaxY, yb)
                DrawWithTool(propSketchTool, xa, ya, xb, yb)
                RotatePointAround(StitchPatCenter.X, StitchPatCenter.Y, Math.PI / 4, xa, ya)
                RotatePointAround(StitchPatCenter.X, StitchPatCenter.Y, Math.PI / 4, xb, yb)
                clipMinX = Math.Min(clipMinX, xa) : clipMinX = Math.Min(clipMinX, xb)
                clipMaxX = Math.Max(clipMaxX, xa) : clipMaxX = Math.Max(clipMaxX, xb)
                clipMinY = Math.Min(clipMinY, ya) : clipMinY = Math.Min(clipMinY, yb)
                clipMaxY = Math.Max(clipMaxY, ya) : clipMaxY = Math.Max(clipMaxY, yb)
                DrawWithTool(propSketchTool, xa, ya, xb, yb)
                RotatePointAround(StitchPatCenter.X, StitchPatCenter.Y, Math.PI / 4, xa, ya)
                RotatePointAround(StitchPatCenter.X, StitchPatCenter.Y, Math.PI / 4, xb, yb)
                clipMinX = Math.Min(clipMinX, xa) : clipMinX = Math.Min(clipMinX, xb)
                clipMaxX = Math.Max(clipMaxX, xa) : clipMaxX = Math.Max(clipMaxX, xb)
                clipMinY = Math.Min(clipMinY, ya) : clipMinY = Math.Min(clipMinY, yb)
                clipMaxY = Math.Max(clipMaxY, ya) : clipMaxY = Math.Max(clipMaxY, yb)
                DrawWithTool(propSketchTool, xa, ya, xb, yb)
                RotatePointAround(StitchPatCenter.X, StitchPatCenter.Y, Math.PI / 4, xa, ya)
                RotatePointAround(StitchPatCenter.X, StitchPatCenter.Y, Math.PI / 4, xb, yb)
                clipMinX = Math.Min(clipMinX, xa) : clipMinX = Math.Min(clipMinX, xb)
                clipMaxX = Math.Max(clipMaxX, xa) : clipMaxX = Math.Max(clipMaxX, xb)
                clipMinY = Math.Min(clipMinY, ya) : clipMinY = Math.Min(clipMinY, yb)
                clipMaxY = Math.Max(clipMaxY, ya) : clipMaxY = Math.Max(clipMaxY, yb)
                DrawWithTool(propSketchTool, xa, ya, xb, yb)
                RotatePointAround(StitchPatCenter.X, StitchPatCenter.Y, Math.PI / 4, xa, ya)
                RotatePointAround(StitchPatCenter.X, StitchPatCenter.Y, Math.PI / 4, xb, yb)
                clipMinX = Math.Min(clipMinX, xa) : clipMinX = Math.Min(clipMinX, xb)
                clipMaxX = Math.Max(clipMaxX, xa) : clipMaxX = Math.Max(clipMaxX, xb)
                clipMinY = Math.Min(clipMinY, ya) : clipMinY = Math.Min(clipMinY, yb)
                clipMaxY = Math.Max(clipMaxY, ya) : clipMaxY = Math.Max(clipMaxY, yb)
                DrawWithTool(propSketchTool, xa, ya, xb, yb)
                RotatePointAround(StitchPatCenter.X, StitchPatCenter.Y, Math.PI / 4, xa, ya)
                RotatePointAround(StitchPatCenter.X, StitchPatCenter.Y, Math.PI / 4, xb, yb)
                clipMinX = Math.Min(clipMinX, xa) : clipMinX = Math.Min(clipMinX, xb)
                clipMaxX = Math.Max(clipMaxX, xa) : clipMaxX = Math.Max(clipMaxX, xb)
                clipMinY = Math.Min(clipMinY, ya) : clipMinY = Math.Min(clipMinY, yb)
                clipMaxY = Math.Max(clipMaxY, ya) : clipMaxY = Math.Max(clipMaxY, yb)
                DrawWithTool(propSketchTool, xa, ya, xb, yb)
        End Select
    End Sub
    Private Sub drawWithSymmetryRect(ByVal x1 As Integer, ByVal y1 As Integer, ByVal x2 As Integer, ByVal y2 As Integer)
        Dim xa, xb, ya, yb As Double
        Dim alpha, beta, theta As Double
        Dim rectW, rectL As Integer

        clipMinX = Math.Min(x1, x2) : clipMinY = Math.Min(y1, y2) : clipMaxX = Math.Max(x1, x2) : clipMaxY = Math.Max(y1, y2)
        rectW = clipMaxX - clipMinX
        rectL = clipMaxY - clipMinY

        DrawWithTool(propSketchTool, x1, y1, x1 + rectW, y1 + rectL) 'case 0 and all cases


        Select Case PropSymmetry

            Case 1 'Vertical Kaleidoscope
                clipMinY = Math.Min(clipMinY, StitchPatCenter.Y * 2 - y1) : clipMinY = Math.Min(clipMinY, StitchPatCenter.Y * 2 - y2)
                clipMaxY = Math.Max(clipMaxY, StitchPatCenter.Y * 2 - y1) : clipMaxY = Math.Max(clipMaxY, StitchPatCenter.Y * 2 - y2)
                'DrawWithTool(propSketchTool, x1, StitchPatCenter.Y * 2 - y1, x2, StitchPatCenter.Y * 2 - y2)
                DrawWithTool(propSketchTool, x1, StitchPatCenter.Y * 2 - y1, x1 + rectW, StitchPatCenter.Y * 2 - y1 + rectL)

            Case 2 'Horizontal Kaleidoscope
                clipMinX = Math.Min(clipMinX, StitchPatCenter.X * 2 - x1) : clipMinX = Math.Min(clipMinX, StitchPatCenter.X * 2 - x2)
                clipMaxX = Math.Max(clipMaxX, StitchPatCenter.X * 2 - x1) : clipMaxX = Math.Max(clipMaxX, StitchPatCenter.X * 2 - x2)
                DrawWithTool(propSketchTool, StitchPatCenter.X * 2 - x1, y1, StitchPatCenter.X * 2 - x1 + rectW, y1 + rectL)

            Case 3 'Left Daigonal Kaleidoscope
                theta = Math.Atan2(StitchPatRect.Height, StitchPatRect.Width)
                alpha = Math.Atan2(y1 - StitchPatCenter.Y, x1 - StitchPatCenter.X)
                beta = Math.Atan2(y2 - StitchPatCenter.Y, x2 - StitchPatCenter.X)

                xa = x1 : xb = x2 : ya = StitchPatCenter.Y * 2 - y1 : yb = StitchPatCenter.Y * 2 - y2
                RotatePointAround(StitchPatCenter.X, StitchPatCenter.Y, Math.PI / 2, xa, ya)
                RotatePointAround(StitchPatCenter.X, StitchPatCenter.Y, Math.PI / 2, xb, yb)
                clipMinX = Math.Min(clipMinX, xa) : clipMinX = Math.Min(clipMinX, xb)
                clipMaxX = Math.Max(clipMaxX, xa) : clipMaxX = Math.Max(clipMaxX, xb)
                clipMinY = Math.Min(clipMinY, ya) : clipMinY = Math.Min(clipMinY, yb)
                clipMaxY = Math.Max(clipMaxY, ya) : clipMaxY = Math.Max(clipMaxY, yb)
                DrawWithTool(propSketchTool, xa, ya, xa + rectW, ya + rectL)

            Case 4 'Right Daigonal Kaleidoscope
                theta = Math.Atan2(StitchPatRect.Height, StitchPatRect.Width)
                alpha = Math.Atan2(y1 - StitchPatCenter.Y, x1 - StitchPatCenter.X)
                beta = Math.Atan2(y2 - StitchPatCenter.Y, x2 - StitchPatCenter.X)

                xa = StitchPatCenter.X * 2 - x1 : xb = StitchPatCenter.X * 2 - x2 : ya = y1 : yb = y2
                RotatePointAround(StitchPatCenter.X, StitchPatCenter.Y, Math.PI / 2, xa, ya)
                RotatePointAround(StitchPatCenter.X, StitchPatCenter.Y, Math.PI / 2, xb, yb)
                clipMinX = Math.Min(clipMinX, xa) : clipMinX = Math.Min(clipMinX, xb)
                clipMaxX = Math.Max(clipMaxX, xa) : clipMaxX = Math.Max(clipMaxX, xb)
                clipMinY = Math.Min(clipMinY, ya) : clipMinY = Math.Min(clipMinY, yb)
                clipMaxY = Math.Max(clipMaxY, ya) : clipMaxY = Math.Max(clipMaxY, yb)
                DrawWithTool(propSketchTool, xa, ya, xa + rectW, ya + rectL)

            Case 5 '4 Kaleidoscope (vert + horz)
                clipMinY = Math.Min(clipMinY, StitchPatCenter.Y * 2 - y1) : clipMinY = Math.Min(clipMinY, StitchPatCenter.Y * 2 - y2)
                clipMaxY = Math.Max(clipMaxY, StitchPatCenter.Y * 2 - y1) : clipMaxY = Math.Max(clipMaxY, StitchPatCenter.Y * 2 - y2)
                clipMinX = Math.Min(clipMinX, StitchPatCenter.X * 2 - x1) : clipMinX = Math.Min(clipMinX, StitchPatCenter.X * 2 - x2)
                clipMaxX = Math.Max(clipMaxX, StitchPatCenter.X * 2 - x1) : clipMaxX = Math.Max(clipMaxX, StitchPatCenter.X * 2 - x2)

                DrawWithTool(propSketchTool, x1, StitchPatCenter.Y * 2 - y1, x1 + rectW, StitchPatCenter.Y * 2 - y1 + rectL)
                DrawWithTool(propSketchTool, StitchPatCenter.X * 2 - x1, y1, StitchPatCenter.X * 2 - x1 + rectW, y1)
                DrawWithTool(propSketchTool, StitchPatCenter.X * 2 - x1, StitchPatCenter.Y * 2 - y1, StitchPatCenter.X * 2 - x1 + rectW, StitchPatCenter.Y * 2 - y1 + rectL)


            Case 6 '8 Kaleidoscope
                theta = Math.Atan2(StitchPatRect.Height, StitchPatRect.Width)
                alpha = Math.Atan2(y1 - StitchPatCenter.Y, x1 - StitchPatCenter.X)
                beta = Math.Atan2(y2 - StitchPatCenter.Y, x2 - StitchPatCenter.X)

                clipMinY = Math.Min(clipMinY, StitchPatCenter.Y * 2 - y1) : clipMinY = Math.Min(clipMinY, StitchPatCenter.Y * 2 - y2)
                clipMaxY = Math.Max(clipMaxY, StitchPatCenter.Y * 2 - y1) : clipMaxY = Math.Max(clipMaxY, StitchPatCenter.Y * 2 - y2)
                clipMinX = Math.Min(clipMinX, StitchPatCenter.X * 2 - x1) : clipMinX = Math.Min(clipMinX, StitchPatCenter.X * 2 - x2)
                clipMaxX = Math.Max(clipMaxX, StitchPatCenter.X * 2 - x1) : clipMaxX = Math.Max(clipMaxX, StitchPatCenter.X * 2 - x2)

                xa = x1 : xb = x2 : ya = StitchPatCenter.Y * 2 - y1 : yb = StitchPatCenter.Y * 2 - y2
                RotatePointAround(StitchPatCenter.X, StitchPatCenter.Y, Math.PI / 2, xa, ya)
                RotatePointAround(StitchPatCenter.X, StitchPatCenter.Y, Math.PI / 2, xb, yb)
                clipMinX = Math.Min(clipMinX, xa) : clipMinX = Math.Min(clipMinX, xb)
                clipMaxX = Math.Max(clipMaxX, xa) : clipMaxX = Math.Max(clipMaxX, xb)
                clipMinY = Math.Min(clipMinY, ya) : clipMinY = Math.Min(clipMinY, yb)
                clipMaxY = Math.Max(clipMaxY, ya) : clipMaxY = Math.Max(clipMaxY, yb)
                DrawWithTool(propSketchTool, xa, ya, xb, yb)

                xa = x1 : xb = x2 : ya = StitchPatCenter.Y * 2 - y1 : yb = StitchPatCenter.Y * 2 - y2
                DrawWithTool(propSketchTool, xa, ya, xb, yb)
                RotatePointAround(StitchPatCenter.X, StitchPatCenter.Y, -Math.PI / 2, xa, ya)
                RotatePointAround(StitchPatCenter.X, StitchPatCenter.Y, -Math.PI / 2, xb, yb)
                clipMinX = Math.Min(clipMinX, xa) : clipMinX = Math.Min(clipMinX, xb)
                clipMaxX = Math.Max(clipMaxX, xa) : clipMaxX = Math.Max(clipMaxX, xb)
                clipMinY = Math.Min(clipMinY, ya) : clipMinY = Math.Min(clipMinY, yb)
                clipMaxY = Math.Max(clipMaxY, ya) : clipMaxY = Math.Max(clipMaxY, yb)
                DrawWithTool(propSketchTool, xa, ya, xb, yb)

                xa = StitchPatCenter.X * 2 - x1 : xb = StitchPatCenter.X * 2 - x2 : ya = y1 : yb = y2
                DrawWithTool(propSketchTool, xa, ya, xb, yb)


                xa = StitchPatCenter.X * 2 - x1 : xb = StitchPatCenter.X * 2 - x2 : ya = StitchPatCenter.Y * 2 - y1 : yb = StitchPatCenter.Y * 2 - y2
                DrawWithTool(propSketchTool, xa, ya, xb, yb)
                RotatePointAround(StitchPatCenter.X, StitchPatCenter.Y, Math.PI / 2, xa, ya)
                RotatePointAround(StitchPatCenter.X, StitchPatCenter.Y, Math.PI / 2, xb, yb)
                clipMinX = Math.Min(clipMinX, xa) : clipMinX = Math.Min(clipMinX, xb)
                clipMaxX = Math.Max(clipMaxX, xa) : clipMaxX = Math.Max(clipMaxX, xb)
                clipMinY = Math.Min(clipMinY, ya) : clipMinY = Math.Min(clipMinY, yb)
                clipMaxY = Math.Max(clipMaxY, ya) : clipMaxY = Math.Max(clipMaxY, yb)
                DrawWithTool(propSketchTool, xa, ya, xb, yb)

                xa = StitchPatCenter.X * 2 - x1 : xb = StitchPatCenter.X * 2 - x2 : ya = StitchPatCenter.Y * 2 - y1 : yb = StitchPatCenter.Y * 2 - y2
                RotatePointAround(StitchPatCenter.X, StitchPatCenter.Y, -Math.PI / 2, xa, ya)
                RotatePointAround(StitchPatCenter.X, StitchPatCenter.Y, -Math.PI / 2, xb, yb)
                clipMinX = Math.Min(clipMinX, xa) : clipMinX = Math.Min(clipMinX, xb)
                clipMaxX = Math.Max(clipMaxX, xa) : clipMaxX = Math.Max(clipMaxX, xb)
                clipMinY = Math.Min(clipMinY, ya) : clipMinY = Math.Min(clipMinY, yb)
                clipMaxY = Math.Max(clipMaxY, ya) : clipMaxY = Math.Max(clipMaxY, yb)
                DrawWithTool(propSketchTool, xa, ya, xb, yb)


            Case 7 '2 Pinwheel
                xa = x1 : xb = x2 : ya = y1 : yb = y2
                RotatePointAround(StitchPatCenter.X, StitchPatCenter.Y, Math.PI, xa, ya)
                RotatePointAround(StitchPatCenter.X, StitchPatCenter.Y, Math.PI, xb, yb)
                clipMinX = Math.Min(clipMinX, xa) : clipMinX = Math.Min(clipMinX, xb)
                clipMaxX = Math.Max(clipMaxX, xa) : clipMaxX = Math.Max(clipMaxX, xb)
                clipMinY = Math.Min(clipMinY, ya) : clipMinY = Math.Min(clipMinY, yb)
                clipMaxY = Math.Max(clipMaxY, ya) : clipMaxY = Math.Max(clipMaxY, yb)
                DrawWithTool(propSketchTool, xa, ya, xb, yb)
            Case 8 '3 Pinwheel
                xa = x1 : xb = x2 : ya = y1 : yb = y2
                RotatePointAround(StitchPatCenter.X, StitchPatCenter.Y, 2 * Math.PI / 3, xa, ya)
                RotatePointAround(StitchPatCenter.X, StitchPatCenter.Y, 2 * Math.PI / 3, xb, yb)
                clipMinX = Math.Min(clipMinX, xa) : clipMinX = Math.Min(clipMinX, xb)
                clipMaxX = Math.Max(clipMaxX, xa) : clipMaxX = Math.Max(clipMaxX, xb)
                clipMinY = Math.Min(clipMinY, ya) : clipMinY = Math.Min(clipMinY, yb)
                clipMaxY = Math.Max(clipMaxY, ya) : clipMaxY = Math.Max(clipMaxY, yb)
                DrawWithTool(propSketchTool, xa, ya, xb, yb)
                RotatePointAround(StitchPatCenter.X, StitchPatCenter.Y, 2 * Math.PI / 3, xa, ya)
                RotatePointAround(StitchPatCenter.X, StitchPatCenter.Y, 2 * Math.PI / 3, xb, yb)
                clipMinX = Math.Min(clipMinX, xa) : clipMinX = Math.Min(clipMinX, xb)
                clipMaxX = Math.Max(clipMaxX, xa) : clipMaxX = Math.Max(clipMaxX, xb)
                clipMinY = Math.Min(clipMinY, ya) : clipMinY = Math.Min(clipMinY, yb)
                clipMaxY = Math.Max(clipMaxY, ya) : clipMaxY = Math.Max(clipMaxY, yb)
                DrawWithTool(propSketchTool, xa, ya, xb, yb)
            Case 9 '4 Pinwheel
                xa = x1 : xb = x2 : ya = y1 : yb = y2
                RotatePointAround(StitchPatCenter.X, StitchPatCenter.Y, Math.PI / 2, xa, ya)
                RotatePointAround(StitchPatCenter.X, StitchPatCenter.Y, Math.PI / 2, xb, yb)
                clipMinX = Math.Min(clipMinX, xa) : clipMinX = Math.Min(clipMinX, xb)
                clipMaxX = Math.Max(clipMaxX, xa) : clipMaxX = Math.Max(clipMaxX, xb)
                clipMinY = Math.Min(clipMinY, ya) : clipMinY = Math.Min(clipMinY, yb)
                clipMaxY = Math.Max(clipMaxY, ya) : clipMaxY = Math.Max(clipMaxY, yb)
                DrawWithTool(propSketchTool, xa, ya, xb, yb)
                RotatePointAround(StitchPatCenter.X, StitchPatCenter.Y, Math.PI / 2, xa, ya)
                RotatePointAround(StitchPatCenter.X, StitchPatCenter.Y, Math.PI / 2, xb, yb)
                clipMinX = Math.Min(clipMinX, xa) : clipMinX = Math.Min(clipMinX, xb)
                clipMaxX = Math.Max(clipMaxX, xa) : clipMaxX = Math.Max(clipMaxX, xb)
                clipMinY = Math.Min(clipMinY, ya) : clipMinY = Math.Min(clipMinY, yb)
                clipMaxY = Math.Max(clipMaxY, ya) : clipMaxY = Math.Max(clipMaxY, yb)
                DrawWithTool(propSketchTool, xa, ya, xb, yb)
                RotatePointAround(StitchPatCenter.X, StitchPatCenter.Y, Math.PI / 2, xa, ya)
                RotatePointAround(StitchPatCenter.X, StitchPatCenter.Y, Math.PI / 2, xb, yb)
                clipMinX = Math.Min(clipMinX, xa) : clipMinX = Math.Min(clipMinX, xb)
                clipMaxX = Math.Max(clipMaxX, xa) : clipMaxX = Math.Max(clipMaxX, xb)
                clipMinY = Math.Min(clipMinY, ya) : clipMinY = Math.Min(clipMinY, yb)
                clipMaxY = Math.Max(clipMaxY, ya) : clipMaxY = Math.Max(clipMaxY, yb)
                DrawWithTool(propSketchTool, xa, ya, xb, yb)
            Case 10 '5 Pinwheel
                xa = x1 : xb = x2 : ya = y1 : yb = y2
                RotatePointAround(StitchPatCenter.X, StitchPatCenter.Y, 2 * Math.PI / 5, xa, ya)
                RotatePointAround(StitchPatCenter.X, StitchPatCenter.Y, 2 * Math.PI / 5, xb, yb)
                clipMinX = Math.Min(clipMinX, xa) : clipMinX = Math.Min(clipMinX, xb)
                clipMaxX = Math.Max(clipMaxX, xa) : clipMaxX = Math.Max(clipMaxX, xb)
                clipMinY = Math.Min(clipMinY, ya) : clipMinY = Math.Min(clipMinY, yb)
                clipMaxY = Math.Max(clipMaxY, ya) : clipMaxY = Math.Max(clipMaxY, yb)
                DrawWithTool(propSketchTool, xa, ya, xb, yb)
                RotatePointAround(StitchPatCenter.X, StitchPatCenter.Y, 2 * Math.PI / 5, xa, ya)
                RotatePointAround(StitchPatCenter.X, StitchPatCenter.Y, 2 * Math.PI / 5, xb, yb)
                clipMinX = Math.Min(clipMinX, xa) : clipMinX = Math.Min(clipMinX, xb)
                clipMaxX = Math.Max(clipMaxX, xa) : clipMaxX = Math.Max(clipMaxX, xb)
                clipMinY = Math.Min(clipMinY, ya) : clipMinY = Math.Min(clipMinY, yb)
                clipMaxY = Math.Max(clipMaxY, ya) : clipMaxY = Math.Max(clipMaxY, yb)
                DrawWithTool(propSketchTool, xa, ya, xb, yb)
                RotatePointAround(StitchPatCenter.X, StitchPatCenter.Y, 2 * Math.PI / 5, xa, ya)
                RotatePointAround(StitchPatCenter.X, StitchPatCenter.Y, 2 * Math.PI / 5, xb, yb)
                clipMinX = Math.Min(clipMinX, xa) : clipMinX = Math.Min(clipMinX, xb)
                clipMaxX = Math.Max(clipMaxX, xa) : clipMaxX = Math.Max(clipMaxX, xb)
                clipMinY = Math.Min(clipMinY, ya) : clipMinY = Math.Min(clipMinY, yb)
                clipMaxY = Math.Max(clipMaxY, ya) : clipMaxY = Math.Max(clipMaxY, yb)
                DrawWithTool(propSketchTool, xa, ya, xb, yb)
                RotatePointAround(StitchPatCenter.X, StitchPatCenter.Y, 2 * Math.PI / 5, xa, ya)
                RotatePointAround(StitchPatCenter.X, StitchPatCenter.Y, 2 * Math.PI / 5, xb, yb)
                clipMinX = Math.Min(clipMinX, xa) : clipMinX = Math.Min(clipMinX, xb)
                clipMaxX = Math.Max(clipMaxX, xa) : clipMaxX = Math.Max(clipMaxX, xb)
                clipMinY = Math.Min(clipMinY, ya) : clipMinY = Math.Min(clipMinY, yb)
                clipMaxY = Math.Max(clipMaxY, ya) : clipMaxY = Math.Max(clipMaxY, yb)
                DrawWithTool(propSketchTool, xa, ya, xb, yb)
            Case 11 '6 Pinwheel
                xa = x1 : xb = x2 : ya = y1 : yb = y2
                RotatePointAround(StitchPatCenter.X, StitchPatCenter.Y, Math.PI / 3, xa, ya)
                RotatePointAround(StitchPatCenter.X, StitchPatCenter.Y, Math.PI / 3, xb, yb)
                clipMinX = Math.Min(clipMinX, xa) : clipMinX = Math.Min(clipMinX, xb)
                clipMaxX = Math.Max(clipMaxX, xa) : clipMaxX = Math.Max(clipMaxX, xb)
                clipMinY = Math.Min(clipMinY, ya) : clipMinY = Math.Min(clipMinY, yb)
                clipMaxY = Math.Max(clipMaxY, ya) : clipMaxY = Math.Max(clipMaxY, yb)
                DrawWithTool(propSketchTool, xa, ya, xb, yb)
                RotatePointAround(StitchPatCenter.X, StitchPatCenter.Y, Math.PI / 3, xa, ya)
                RotatePointAround(StitchPatCenter.X, StitchPatCenter.Y, Math.PI / 3, xb, yb)
                clipMinX = Math.Min(clipMinX, xa) : clipMinX = Math.Min(clipMinX, xb)
                clipMaxX = Math.Max(clipMaxX, xa) : clipMaxX = Math.Max(clipMaxX, xb)
                clipMinY = Math.Min(clipMinY, ya) : clipMinY = Math.Min(clipMinY, yb)
                clipMaxY = Math.Max(clipMaxY, ya) : clipMaxY = Math.Max(clipMaxY, yb)
                DrawWithTool(propSketchTool, xa, ya, xb, yb)
                RotatePointAround(StitchPatCenter.X, StitchPatCenter.Y, Math.PI / 3, xa, ya)
                RotatePointAround(StitchPatCenter.X, StitchPatCenter.Y, Math.PI / 3, xb, yb)
                clipMinX = Math.Min(clipMinX, xa) : clipMinX = Math.Min(clipMinX, xb)
                clipMaxX = Math.Max(clipMaxX, xa) : clipMaxX = Math.Max(clipMaxX, xb)
                clipMinY = Math.Min(clipMinY, ya) : clipMinY = Math.Min(clipMinY, yb)
                clipMaxY = Math.Max(clipMaxY, ya) : clipMaxY = Math.Max(clipMaxY, yb)
                DrawWithTool(propSketchTool, xa, ya, xb, yb)
                RotatePointAround(StitchPatCenter.X, StitchPatCenter.Y, Math.PI / 3, xa, ya)
                RotatePointAround(StitchPatCenter.X, StitchPatCenter.Y, Math.PI / 3, xb, yb)
                clipMinX = Math.Min(clipMinX, xa) : clipMinX = Math.Min(clipMinX, xb)
                clipMaxX = Math.Max(clipMaxX, xa) : clipMaxX = Math.Max(clipMaxX, xb)
                clipMinY = Math.Min(clipMinY, ya) : clipMinY = Math.Min(clipMinY, yb)
                clipMaxY = Math.Max(clipMaxY, ya) : clipMaxY = Math.Max(clipMaxY, yb)
                DrawWithTool(propSketchTool, xa, ya, xb, yb)
                RotatePointAround(StitchPatCenter.X, StitchPatCenter.Y, Math.PI / 3, xa, ya)
                RotatePointAround(StitchPatCenter.X, StitchPatCenter.Y, Math.PI / 3, xb, yb)
                clipMinX = Math.Min(clipMinX, xa) : clipMinX = Math.Min(clipMinX, xb)
                clipMaxX = Math.Max(clipMaxX, xa) : clipMaxX = Math.Max(clipMaxX, xb)
                clipMinY = Math.Min(clipMinY, ya) : clipMinY = Math.Min(clipMinY, yb)
                clipMaxY = Math.Max(clipMaxY, ya) : clipMaxY = Math.Max(clipMaxY, yb)
                DrawWithTool(propSketchTool, xa, ya, xb, yb)
            Case 12 '8 Pinwheel
                xa = x1 : xb = x2 : ya = y1 : yb = y2
                RotatePointAround(StitchPatCenter.X, StitchPatCenter.Y, Math.PI / 4, xa, ya)
                RotatePointAround(StitchPatCenter.X, StitchPatCenter.Y, Math.PI / 4, xb, yb)
                clipMinX = Math.Min(clipMinX, xa) : clipMinX = Math.Min(clipMinX, xb)
                clipMaxX = Math.Max(clipMaxX, xa) : clipMaxX = Math.Max(clipMaxX, xb)
                clipMinY = Math.Min(clipMinY, ya) : clipMinY = Math.Min(clipMinY, yb)
                clipMaxY = Math.Max(clipMaxY, ya) : clipMaxY = Math.Max(clipMaxY, yb)
                DrawWithTool(propSketchTool, xa, ya, xb, yb)
                RotatePointAround(StitchPatCenter.X, StitchPatCenter.Y, Math.PI / 4, xa, ya)
                RotatePointAround(StitchPatCenter.X, StitchPatCenter.Y, Math.PI / 4, xb, yb)
                clipMinX = Math.Min(clipMinX, xa) : clipMinX = Math.Min(clipMinX, xb)
                clipMaxX = Math.Max(clipMaxX, xa) : clipMaxX = Math.Max(clipMaxX, xb)
                clipMinY = Math.Min(clipMinY, ya) : clipMinY = Math.Min(clipMinY, yb)
                clipMaxY = Math.Max(clipMaxY, ya) : clipMaxY = Math.Max(clipMaxY, yb)
                DrawWithTool(propSketchTool, xa, ya, xb, yb)
                RotatePointAround(StitchPatCenter.X, StitchPatCenter.Y, Math.PI / 4, xa, ya)
                RotatePointAround(StitchPatCenter.X, StitchPatCenter.Y, Math.PI / 4, xb, yb)
                clipMinX = Math.Min(clipMinX, xa) : clipMinX = Math.Min(clipMinX, xb)
                clipMaxX = Math.Max(clipMaxX, xa) : clipMaxX = Math.Max(clipMaxX, xb)
                clipMinY = Math.Min(clipMinY, ya) : clipMinY = Math.Min(clipMinY, yb)
                clipMaxY = Math.Max(clipMaxY, ya) : clipMaxY = Math.Max(clipMaxY, yb)
                DrawWithTool(propSketchTool, xa, ya, xb, yb)
                RotatePointAround(StitchPatCenter.X, StitchPatCenter.Y, Math.PI / 4, xa, ya)
                RotatePointAround(StitchPatCenter.X, StitchPatCenter.Y, Math.PI / 4, xb, yb)
                clipMinX = Math.Min(clipMinX, xa) : clipMinX = Math.Min(clipMinX, xb)
                clipMaxX = Math.Max(clipMaxX, xa) : clipMaxX = Math.Max(clipMaxX, xb)
                clipMinY = Math.Min(clipMinY, ya) : clipMinY = Math.Min(clipMinY, yb)
                clipMaxY = Math.Max(clipMaxY, ya) : clipMaxY = Math.Max(clipMaxY, yb)
                DrawWithTool(propSketchTool, xa, ya, xb, yb)
                RotatePointAround(StitchPatCenter.X, StitchPatCenter.Y, Math.PI / 4, xa, ya)
                RotatePointAround(StitchPatCenter.X, StitchPatCenter.Y, Math.PI / 4, xb, yb)
                clipMinX = Math.Min(clipMinX, xa) : clipMinX = Math.Min(clipMinX, xb)
                clipMaxX = Math.Max(clipMaxX, xa) : clipMaxX = Math.Max(clipMaxX, xb)
                clipMinY = Math.Min(clipMinY, ya) : clipMinY = Math.Min(clipMinY, yb)
                clipMaxY = Math.Max(clipMaxY, ya) : clipMaxY = Math.Max(clipMaxY, yb)
                DrawWithTool(propSketchTool, xa, ya, xb, yb)
                RotatePointAround(StitchPatCenter.X, StitchPatCenter.Y, Math.PI / 4, xa, ya)
                RotatePointAround(StitchPatCenter.X, StitchPatCenter.Y, Math.PI / 4, xb, yb)
                clipMinX = Math.Min(clipMinX, xa) : clipMinX = Math.Min(clipMinX, xb)
                clipMaxX = Math.Max(clipMaxX, xa) : clipMaxX = Math.Max(clipMaxX, xb)
                clipMinY = Math.Min(clipMinY, ya) : clipMinY = Math.Min(clipMinY, yb)
                clipMaxY = Math.Max(clipMaxY, ya) : clipMaxY = Math.Max(clipMaxY, yb)
                DrawWithTool(propSketchTool, xa, ya, xb, yb)
                RotatePointAround(StitchPatCenter.X, StitchPatCenter.Y, Math.PI / 4, xa, ya)
                RotatePointAround(StitchPatCenter.X, StitchPatCenter.Y, Math.PI / 4, xb, yb)
                clipMinX = Math.Min(clipMinX, xa) : clipMinX = Math.Min(clipMinX, xb)
                clipMaxX = Math.Max(clipMaxX, xa) : clipMaxX = Math.Max(clipMaxX, xb)
                clipMinY = Math.Min(clipMinY, ya) : clipMinY = Math.Min(clipMinY, yb)
                clipMaxY = Math.Max(clipMaxY, ya) : clipMaxY = Math.Max(clipMaxY, yb)
                DrawWithTool(propSketchTool, xa, ya, xb, yb)
        End Select
    End Sub

    Private Sub UndoButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UndoButton.Click
        If unDoStack.Count > 0 Then

            reDoStack.Add(propSketch.Clone)

            propSketch = unDoStack(unDoStack.Count)
            unDoStack.Remove(unDoStack.Count)
            If Not IsNothing(g) Then
                g.Dispose()
            End If
            g = Graphics.FromImage(propSketch)
            'g.PixelOffsetMode = Drawing2D.PixelOffsetMode.Half
            g.SmoothingMode = Drawing2D.SmoothingMode.None
            g.InterpolationMode = Drawing2D.InterpolationMode.NearestNeighbor
            If propClipMode Then
                initializeSketch("")
            End If
            SketchPanel.Invalidate()

        End If
    End Sub

    Private Sub RedoButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RedoButton.Click
        If reDoStack.Count > 0 Then
            unDoStack.Add(propSketch.Clone)
            propSketch = reDoStack(reDoStack.Count)
            reDoStack.Remove(reDoStack.Count)
            If Not IsNothing(g) Then
                g.Dispose()
            End If
            g = Graphics.FromImage(propSketch)
            'g.PixelOffsetMode = Drawing2D.PixelOffsetMode.Half
            g.SmoothingMode = Drawing2D.SmoothingMode.None
            g.InterpolationMode = Drawing2D.InterpolationMode.NearestNeighbor
            If propClipMode Then
                initializeSketch("")
            End If
            SketchPanel.Invalidate()
        End If

    End Sub

    Private Sub UndoRedoButton_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles UndoButton.MouseDown, RedoButton.MouseDown
        sender.FlatAppearance.BorderSize = 3
    End Sub

    Private Sub UndoRedoButton_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles UndoButton.MouseUp, RedoButton.MouseUp
        sender.FlatAppearance.BorderSize = 0
    End Sub
    Private Sub GuidelinesButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GuidelinesButton.Click
        If sender.ImageIndex = 0 Then
            sender.imageindex = 1
        Else
            sender.imageindex = 0
        End If
        SketchPanel.Invalidate()
    End Sub
    Private Sub ColorScheme_MouseClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles ColorScheme.MouseClick
        Dim k As Integer
        k = Int(e.X / (sender.Width / 4))
        sender.transparencies(k) = Not (sender.transparencies(k))
        sender.invalidate()
        RaiseEvent transparencyChanged(sender.colors(k), sender.transparencies(k))

    End Sub


    Private Sub TileViewLink_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles TileViewLink.LinkClicked
        TileView = Not TileView
        If TileView Then
            TileViewLink.Text = "Sketch Mode"
            SketchPanel.Invalidate()
        Else
            TileViewLink.Text = "Tile View"
            SketchPanel.Invalidate()
        End If
    End Sub

    ' Flood the area at this point.
    Public Sub UnsafeFloodFill(ByVal x As Integer, ByVal y As Integer)

        If x > 0 And x < propSketch.Width And y > 0 And y < propSketch.Height Then
            Dim underlay, overlay As Bitmap
            Dim g2 As Graphics
            Dim old_color As Color = propSketch.GetPixel(x, y)

            underlay = New Bitmap(propSketch.Width, propSketch.Height)
            g2 = Graphics.FromImage(underlay)
            'g2.PixelOffsetMode = Drawing2D.PixelOffsetMode.Half
            g2.SmoothingMode = Drawing2D.SmoothingMode.None
            g2.InterpolationMode = Drawing2D.InterpolationMode.NearestNeighbor
            g2.FillRectangle(SketchBrush, underlay.GetBounds(GraphicsUnit.Pixel)) 'create a flood fill dimensions of entire sketch.

            overlay = New Bitmap(propSketch)
            Dim bm_bytes As New BitmapBytesARGB32(overlay)

            ' Get the old and new colors' components.
            Dim old_a As Byte = old_color.A
            Dim old_r As Byte = old_color.R
            Dim old_g As Byte = old_color.G
            Dim old_b As Byte = old_color.B

            Dim new_r As Byte = old_r
            Dim new_g As Byte = old_g
            Dim new_b As Byte = old_b

            ' Start with the original point in the stack.
            Dim pts As New Stack(1000)
            pts.Push(New Point(x, y))
            overlay.SetPixel(x, y, Color.FromArgb(0, old_r, old_g, old_b))

            ' Make a BitmapBytesARGB32 object.


            ' Lock the bitmap.
            bm_bytes.LockBitmap()

            ' While the stack is not empty, process a point.

            Do While pts.Count > 0
                Dim pt As Point = DirectCast(pts.Pop(), Point)
                If pt.X > 0 Then UnsafeCheckPoint(bm_bytes, pts, pt.X - 1, pt.Y, old_r, old_g, old_b)
                If pt.Y > 0 Then UnsafeCheckPoint(bm_bytes, pts, pt.X, pt.Y - 1, old_r, old_g, old_b)
                If pt.X < overlay.Width - 1 Then UnsafeCheckPoint(bm_bytes, pts, pt.X + 1, pt.Y, old_r, old_g, old_b)
                If pt.Y < overlay.Height - 1 Then UnsafeCheckPoint(bm_bytes, pts, pt.X, pt.Y + 1, old_r, old_g, old_b)
            Loop

            ' Unlock the bitmap.
            bm_bytes.UnlockBitmap()

            g2.DrawImage(overlay, 0, 0) 'copy sketch with flood fill area "cut out" (transparent) onto underlay
            g.DrawImage(underlay, 0, 0) 'underlay now contains the completed image. Copy on top of sketch.
            g2.Dispose()
            underlay.Dispose()
            overlay.Dispose()
        End If
    End Sub

    ' See if this point should be added to the stack.
    Private Sub UnsafeCheckPoint(ByRef bm_bytes As  _
        BitmapBytesARGB32, ByVal pts As Stack, ByVal x As _
        Integer, ByVal y As Integer, ByVal old_r As Byte, ByVal _
        old_g As Byte, ByVal old_b As Byte)
        Dim pix As Integer = y * bm_bytes.RowSizeBytes + x * BitmapBytesARGB32.PixelSizeBytes

        Dim b As Byte = bm_bytes.ImageBytes(pix)
        Dim g As Byte = bm_bytes.ImageBytes(pix + 1)
        Dim r As Byte = bm_bytes.ImageBytes(pix + 2)
        Dim a As Byte = bm_bytes.ImageBytes(pix + 3)

        If (r = old_r) AndAlso (g = old_g) AndAlso (b = old_b) AndAlso (a = 255) Then
            pts.Push(New Point(x, y))
            bm_bytes.ImageBytes(pix + 3) = 0
        End If
    End Sub


    Private Function getDiagonalMeasurement(ByVal X1 As Double, ByVal Y1 As Double, ByVal X2 As Double, ByVal Y2 As Double) As String
        Dim CM As Double
        If X2 = X1 Then
            CM = Math.Abs((Y2 - Y1 + 1) * propRowz * propRowGauge)
        ElseIf Y2 = Y1 Then
            CM = Math.Abs((X2 - X1 + 1) * propStitchGauge)
        Else
            CM = Math.Sqrt(((X2 - X1 + 1) * propStitchGauge) ^ 2 + ((Y2 - Y1 + 1) * propRowz * propRowGauge) ^ 2)
        End If

        getDiagonalMeasurement = Format(CM, "###.#") & " CM"
    End Function
    Private Function getRowStitchesMeasurement(ByVal X1 As Double, ByVal Y1 As Double, ByVal X2 As Double, ByVal Y2 As Double) As String
        getRowStitchesMeasurement = ((Math.Abs(Y2 - Y1) + 1) * propRowz).ToString & " rows " & (Math.Abs(X2 - X1) + 1).ToString & " stitches"

    End Function

    Private Function getBoxMeasurement(ByVal X1 As Double, ByVal Y1 As Double, ByVal X2 As Double, ByVal Y2 As Double) As String

        getBoxMeasurement = Format((Math.Abs(X2 - X1) + 1) * propStitchGauge, "###.#") & " x " & Format((Math.Abs(Y2 - Y1) + 1) * propRowz * propRowGauge, "###.#") & " CM"
    End Function
    Private Function getTextPath(ByVal propDrawText As String, ByVal propDrawFont As Font, ByVal X1 As Integer, ByVal Y1 As Integer, ByVal X2 As Integer, ByVal Y2 As Integer, ByVal curved As Boolean) As Drawing2D.GraphicsPath
        Dim myPath As New Drawing2D.GraphicsPath
        Dim gp As New Drawing2D.GraphicsPath
        Dim pd As Drawing2D.PathData

        Dim p As Integer
        Dim y0 As Integer
        Dim theta As Double
        Dim alpha As Double
        Dim thetaPrime As Double
        Dim stringSize As SizeF
        ' Set up all the string parameters.
        Dim newX As Double
        Dim newY As Double
        Dim origin As New Point(0, 0)
        Dim format As StringFormat = StringFormat.GenericDefault

        ' Add the string to the path.
        myPath.AddString(propDrawText, propDrawFont.FontFamily, propDrawFont.Style, propDrawFont.Size, origin, format)

        pd = myPath.PathData
        stringSize.Width = 0
        stringSize.Height = 0
        For p = 0 To pd.Points.Length - 1
            If pd.Points(p).X > stringSize.Width Then
                stringSize.Width = pd.Points(p).X
            End If
            If pd.Points(p).Y > stringSize.Height Then
                stringSize.Height = pd.Points(p).Y
            End If
        Next p

        y0 = Math.Sqrt((Y2 - Y1) ^ 2 + (X2 - X1) ^ 2)
        If Int(X2) = Int(X1) Then
            theta = 3 * Math.PI / 2
        End If
        If X2 > X1 Then '
            theta = Math.Atan((Y2 - Y1) / (X2 - X1))
        End If

        If X2 < X1 Then
            theta = Math.Atan((Y2 - Y1) / (X2 - X1)) + Math.PI
        End If
        myPath.Flatten()
        If curved Then
            alpha = stringSize.Width / y0
            For p = 0 To pd.Points.Length - 1
                thetaPrime = theta + alpha * pd.Points(p).X / (stringSize.Width - 1)
                pd.Points(p).X = X1 + ((y0 + stringSize.Height - pd.Points(p).Y - 1) * Math.Cos(thetaPrime))
                pd.Points(p).Y = Y1 + ((y0 + stringSize.Height - pd.Points(p).Y - 1) * Math.Sin(thetaPrime))

            Next
            gp.FillMode = Drawing2D.FillMode.Winding
            getTextPath = New Drawing2D.GraphicsPath(pd.Points, pd.Types)
        Else 'Slant text


            For p = 0 To pd.Points.Length - 1
                newX = pd.Points(p).X
                newY = stringSize.Height - pd.Points(p).Y - 1
                pd.Points(p).X = X1 + newX * Math.Cos(theta) + newY * Math.Sin(theta)
                pd.Points(p).Y = Y1 + newX * Math.Sin(theta) - newY * Math.Cos(theta)

            Next
        End If
        getTextPath = New Drawing2D.GraphicsPath(pd.Points, pd.Types)
        myPath.Dispose()

    End Function
    Sub setClipAreaForPath(ByVal pd As Drawing2D.PathData)
        Dim p As Integer

        For p = 0 To pd.Points.Length - 1
            If pd.Points(p).X > clipMaxX Then
                clipMaxX = pd.Points(p).X
            End If
            If pd.Points(p).X < clipMinX Then
                clipMinX = pd.Points(p).X
            End If
            If pd.Points(p).Y > clipMaxY Then
                clipMaxY = pd.Points(p).Y
            End If
            If pd.Points(p).Y < clipMinY Then
                clipMinY = pd.Points(p).Y
            End If
        Next p
    End Sub

    Private Sub SketchPanel_MouseEnter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SketchPanel.MouseEnter
        Me.SizeLabel.Focus()

    End Sub


    Private Sub SketchBox_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If propSketchMode = "Chart" Then
            Me.SplitContainer1.Panel1.AutoScrollPosition = New Point(0, (Me.SketchPanel.Height - Me.SplitContainer1.Panel1.Height) / 2)
            oldMagnification = 4
            oldAspect = 1
        End If
    End Sub


    Private Sub SplitContainer1_Panel1_Scroll(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ScrollEventArgs) Handles SplitContainer1.Panel1.Scroll
        Me.SketchPanel.Invalidate()
    End Sub


    Private Sub TracingLink_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles TracingLink.LinkClicked
        Tracing = Not Tracing
        If Tracing Then
            TracingLink.Text = "Opaque Paper"
            Me.ParentForm.Opacity = 0.75
        Else
            TracingLink.Text = "Tracing Paper"
            Me.ParentForm.Opacity = 1
        End If
    End Sub
   
End Class


