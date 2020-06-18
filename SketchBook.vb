
Imports System.Windows.Forms

Public Class SketchBook
    Private currentTool As String
    Private currentToolOption As String
    Private dragging As Boolean
    Private pointClicked, startPoint, endPoint, lastPoint As Point
    Private MouseButtonUsed As MouseButtons
    Private Paints(19) As Drawing.TextureBrush
    Private DropperCursor As Cursor
    Private leftBmap, rightBmap As Bitmap
    Public StitchPatBmap As Bitmap
    Public UpdateStitchPat As Boolean
    Public UpdateFormProgramme As Boolean
    Public ChartMode As Boolean
    Private ChalkMode As Boolean
    Public Chart As String
    Private currentFill As String

    Private fullscreenmode As Boolean
    Private SBP As Rectangle

  
    Private Sub ToolBoxButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BrushButton.Click, VerticalButton.Click, TextButton.Click, HorizontalButton.Click, FillButton.Click, DiagonalButton.Click, CircleButton.Click, RotateRightButton.Click, RotateLeftButton.Click, RectangleButton.Click, MirrorButton.Click, FlipButton.Click, FilledRectangleButton.Click, FilledCircleButton.Click, stampButton.Click, TapeMeasureButton.Click, ResizeButton.Click
        For Each T As Object In ToolBox.Controls
            If sender.Name <> T.Name Then
                T.FlatAppearance.BorderSize = 0
                T.ImageIndex = 1
            Else
                T.FlatAppearance.BorderSize = 2

                If T.tag = "Text" Then
                    Select Case SketchBox1.SketchTool
                        Case "Text"
                            T.ImageIndex = 2
                            SketchBox1.SketchTool = "CurvedText"
                        Case "CurvedText"
                            T.ImageIndex = 3
                            SketchBox1.SketchTool = "SlantedText"
                        Case Else
                            T.ImageIndex = 0
                            SketchBox1.SketchTool = "Text"
                    End Select

                Else
                    If T.tag = "Measure" Then
                        Select Case SketchBox1.SketchTool
                            Case "Measure"
                                T.ImageIndex = 2
                                SketchBox1.SketchTool = "Chalk"
                                ChalkMode = True
                                Exit Select
                            Case "Chalk"
                                T.ImageIndex = 0
                                SketchBox1.SketchTool = "Measure"
                                ChalkMode = False
                                Exit Select
                            Case Else
                                If ChalkMode Then
                                    T.ImageIndex = 2
                                    SketchBox1.SketchTool = "Chalk"
                                Else
                                    T.ImageIndex = 0
                                    SketchBox1.SketchTool = "Measure"
                                End If
                        End Select
                        If Not ChalkMode Then
                            SketchBox1.chalkMarks.Reset()
                            SketchBox1.SketchPanel.Invalidate()
                        End If
                    Else
                        T.ImageIndex = 0
                        SketchBox1.SketchTool = T.Tag
                    End If

                End If

            End If
        Next
        Select Case sender.Tag
            Case "Text"
                SketchBox1.DrawFont = TextBox1.Font
                SketchBox1.DrawText = TextBox1.Text
            Case "Measure", "Chalk"
                SketchBox1.RowGauge = SwatchRowsTextBox.Value / 400
                SketchBox1.StitchGauge = SwatchStitchesTextBox.Value / 400
                SketchBox1.Rowz = Rowz.Value
            Case "Resize"
                Me.Cursor = Cursors.WaitCursor
                CopyChartToLeftBrush()
                Me.Cursor = Cursors.Default
                SketchBox1.SketchTool = "FilledRectangle"
                Dim g3 As Graphics
                g3 = Graphics.FromImage(SketchBox1.SketchImage)
                g3.Clear(Color.White)
                SketchBox1.SketchPanel.Invalidate()
        End Select
        ' Private Sub TapeMeasureButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TapeMeasureButton.Click
        '    If sender.imageIndex = 0 Then
        'propSketchTool = OldTool
        'If OldTool = "Lasso" Then
        'LassoButton.ImageIndex = 0
        'End If
        'sender.ImageIndex = 1
        'Else
        'propSketchTool = "Measure"
        'sender.ImageIndex = 0
        'LassoButton.ImageIndex = 1
        'End If
        'sender.invalidate()
        'End Sub

        DropperButton.FlatAppearance.BorderSize = 0
        DropperButton.ImageIndex = 1
        ToolBox.Invalidate()
        SketchBox1.LassoButton.ImageIndex = 1
        SketchBox1.LassoButton.Invalidate()
    End Sub
    Private Sub DropperButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DropperButton.Click
        For Each T As Object In ToolBox.Controls
            T.FlatAppearance.BorderSize = 0
            T.ImageIndex = 1
        Next
        SketchBox1.SketchTool = "Dropper"
        DropperButton.FlatAppearance.BorderSize = 2
        DropperButton.ImageIndex = 0
        ToolBox.Invalidate()
    End Sub
    Private Sub SketchBook_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles MyBase.MouseDown, PageDrawSketch.MouseDown, PageChooseStitchPattern.MouseDown
        If e.Button = Windows.Forms.MouseButtons.Left Then
            dragging = True
            pointClicked = New Point(e.X, e.Y)
            Me.Cursor = Cursors.SizeAll
        End If
    End Sub
    Private Sub SketchBook_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles MyBase.MouseMove, PageDrawSketch.MouseMove, PageChooseStitchPattern.MouseMove
        If dragging Then
            Dim pointMoveTo As Point
            pointMoveTo = Me.PointToScreen(New Point(e.X, e.Y))
            pointMoveTo.Offset(-pointClicked.X, -pointClicked.Y)
            Me.Location = pointMoveTo
        End If
    End Sub
    Private Sub SketchBook_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles MyBase.MouseUp, PageDrawSketch.MouseUp, PageChooseStitchPattern.MouseUp
        dragging = False
        Me.Cursor = Cursors.Default
    End Sub
    Private Sub SketchBook_MouseHover(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.MouseHover, PageDrawSketch.MouseHover, PageChooseStitchPattern.MouseHover
        Me.Cursor = Cursors.SizeAll
    End Sub
    Private Sub SketchBook_MouseLeave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.MouseLeave, PageDrawSketch.MouseLeave, PageChooseStitchPattern.MouseLeave
        Me.Cursor = Cursors.Default
    End Sub
    Private Sub SketchBook_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        SketchBox1.SketchTool = "Brush"
        DropperCursor = New Cursor(My.Resources.paintChooser.Handle)
        If ChartMode Then
            ResizeButton.Visible = True
            LayoutButton.ImageIndex = 3
        Else
            ResizeButton.Visible = False
        End If
        fullscreenmode = False

        SBP.X = Me.Location.X
        SBP.Y = Me.Location.Y
        SBP.Width = Me.Width
        SBP.Height = Me.Height

        Me.Location = New Point(Int((My.Computer.Screen.WorkingArea.Size.Width - Me.Width) / 2), Int((My.Computer.Screen.WorkingArea.Size.Height - Me.Height) / 2))
        Me.TopMost = True
    End Sub



    Private Sub SketchBox1_MotifChanged(ByVal colors() As System.Drawing.Color) Handles SketchBox1.MotifChanged

        Dim abitmap As Bitmap
        abitmap = New Bitmap(2, 2)
        For Each b As TextureBrush In Paints
            If Not IsNothing(b) Then
                b.Dispose()
            End If
        Next
        Dim index, i, j, colorCount As Byte
        If ChartMode Then
            SketchBox1.SketchMode = "Chart"

            For i = 0 To 3
                abitmap.SetPixel(0, 0, SketchBox1.ColorScheme.Colors(i))
                abitmap.SetPixel(1, 0, SketchBox1.ColorScheme.Colors(i))
                abitmap.SetPixel(0, 1, SketchBox1.ColorScheme.Colors(i))
                abitmap.SetPixel(1, 1, SketchBox1.ColorScheme.Colors(i))
                Paints(i) = New TextureBrush(abitmap)
            Next
            index = 4
        Else
            colorCount = SketchBox1.ColorCount - 1
            ReDim Paints((colorCount + 1) ^ 2 * 4 - 1)
            index = 0
            For i = 0 To colorCount
                For j = 0 To colorCount
                    If i = j Then
                        abitmap.SetPixel(0, 0, SketchBox1.ColorScheme.Colors(i))
                        abitmap.SetPixel(1, 0, SketchBox1.ColorScheme.Colors(i))
                        abitmap.SetPixel(0, 1, SketchBox1.ColorScheme.Colors(i))
                        abitmap.SetPixel(1, 1, SketchBox1.ColorScheme.Colors(i))

                        Paints(index) = New TextureBrush(abitmap)
                        index += 1
                    End If
                    If i <> j Then
                        abitmap.SetPixel(0, 0, SketchBox1.ColorScheme.Colors(i))
                        abitmap.SetPixel(1, 0, SketchBox1.ColorScheme.Colors(j))
                        abitmap.SetPixel(0, 1, SketchBox1.ColorScheme.Colors(j))
                        abitmap.SetPixel(1, 1, SketchBox1.ColorScheme.Colors(i))

                        Paints(index) = New TextureBrush(abitmap)
                        index += 1

                        abitmap.SetPixel(0, 0, SketchBox1.ColorScheme.Colors(j))
                        abitmap.SetPixel(1, 0, SketchBox1.ColorScheme.Colors(i))
                        abitmap.SetPixel(0, 1, SketchBox1.ColorScheme.Colors(i))
                        abitmap.SetPixel(1, 1, SketchBox1.ColorScheme.Colors(i))

                        Paints(index) = New TextureBrush(abitmap)
                        index += 1

                    End If
                Next j
            Next i
        End If

        ReDim Preserve Paints(index - 1)
        If ChartMode Then
            leftBmap = Paints(1).Image
            rightBmap = Paints(0).Image
        Else
            leftBmap = Paints(0).Image
            rightBmap = Paints(1).Image
        End If

        setBrushPen(leftBmap, SketchBox1.LeftPen, SketchBox1.LeftBrush)
        setBrushPen(rightBmap, SketchBox1.RightPen, SketchBox1.RightBrush)

        PaintBox.Invalidate()
        LeftMousePaint.Invalidate()
        RightMousePaint.Invalidate()
        ' SketchBox1.CenterPt = New Point(20, 20)
    End Sub

    Private Sub PaintBox_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles PaintBox.Paint

        Dim x, y As Integer
        x = 1 : y = 1
        e.Graphics.PageScale = 2
        For Each b As TextureBrush In Paints
            e.Graphics.FillEllipse(b, x, y, 10, 15)
            e.Graphics.DrawEllipse(Pens.SaddleBrown, x, y, 10, 14)
            If x = 37 Then
                x = 1
                y = y + 16
            Else
                x = x + 12
            End If
        Next
    End Sub
    Private Sub PaintBox_MouseEnter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PaintBox.MouseEnter
        sender.Cursor = DropperCursor
    End Sub
    Private Sub PaintBox_MouseLeave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PaintBox.MouseLeave
        sender.cursor = Cursors.Default
    End Sub

    Private Sub LeftMousePaint_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles LeftMousePaint.Paint

        e.Graphics.SmoothingMode = Drawing2D.SmoothingMode.None
        e.Graphics.InterpolationMode = Drawing2D.InterpolationMode.NearestNeighbor
        e.Graphics.PixelOffsetMode = Drawing2D.PixelOffsetMode.Half
        e.Graphics.DrawImage(SketchBox1.LeftBrush.Image, e.ClipRectangle)

    End Sub

    Private Sub RightMousePaint_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles RightMousePaint.Paint
        e.Graphics.SmoothingMode = Drawing2D.SmoothingMode.None
        e.Graphics.InterpolationMode = Drawing2D.InterpolationMode.NearestNeighbor
        e.Graphics.PixelOffsetMode = Drawing2D.PixelOffsetMode.Half
        e.Graphics.DrawImage(SketchBox1.RightBrush.Image, e.ClipRectangle)
    End Sub
    Private Sub PaintBox_MouseClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles PaintBox.MouseClick
        Dim i As Integer

        i = (Int((e.Y - 2) / 32) * 4) + Int(((e.X - 2) / 24))
        If i <= Paints.GetUpperBound(0) Then
            If e.Button = Windows.Forms.MouseButtons.Left Then
                setBrushPen(Paints(i).Image, SketchBox1.LeftPen, SketchBox1.LeftBrush)
                leftBmap = Paints(i).Image
                LeftMousePaint.Invalidate()
            Else
                setBrushPen(Paints(i).Image, SketchBox1.RightPen, SketchBox1.RightBrush)
                rightBmap = Paints(i).Image
                RightMousePaint.Invalidate()
            End If
        End If

    End Sub

    Private Sub setBrushPen(ByVal img As Image, ByRef aPen As Pen, ByRef aBrush As TextureBrush)
        Dim g As Graphics
        Dim p As PointF
        Dim bmap
        Dim i, j, k As Integer

        Select Case LayoutButton.ImageIndex
            Case 0 'grid
                bmap = New Bitmap(img)
                SketchBox1.FillMode = "Grid"
            Case 1 'brick

                bmap = New Bitmap(img.Width, img.Height * 2)
                SketchBox1.FillMode = "Brick"
                g = Graphics.FromImage(bmap)
                g.SmoothingMode = Drawing2D.SmoothingMode.None
                g.InterpolationMode = Drawing2D.InterpolationMode.NearestNeighbor
                g.PixelOffsetMode = Drawing2D.PixelOffsetMode.Half
                g.DrawImage(img, 0, 0)
                p.X = 0 - img.Width / 2
                p.Y = 0 + img.Height
                g.DrawImage(img, p)
                p.X = img.Width / 2
                g.DrawImage(img, p)
                ' p.Y = img.Height
                ' p.X = 0
                ' g.DrawImage(img, p)

            Case 2 'half drop

                bmap = New Bitmap(img.Width * 2, img.Height)
                SketchBox1.FillMode = "HalfDrop"
                g = Graphics.FromImage(bmap)
                g.SmoothingMode = Drawing2D.SmoothingMode.None
                g.InterpolationMode = Drawing2D.InterpolationMode.NearestNeighbor
                g.PixelOffsetMode = Drawing2D.PixelOffsetMode.Half
                g.DrawImage(img, 0, 0)
                p.Y = 0 - img.Height / 2
                p.X = 0 + img.Width
                g.DrawImage(img, p)
                p.Y = img.Height / 2
                g.DrawImage(img, p)
                ' p.X = img.Width
                'p.Y = 0
                'g.DrawImage(img, p)

            Case 3 'Resize
                bmap = New Bitmap(img)
                SketchBox1.FillMode = "Resize"
            Case Else
                bmap = New Bitmap(img)
                SketchBox1.FillMode = "Grid"
        End Select


        For i = 0 To bmap.Width - 1
            For j = 0 To bmap.Height - 1
                For k = 0 To SketchBox1.ColorCount - 1
                    If Color.FromArgb(255, bmap.GetPixel(i, j)) = SketchBox1.ColorScheme.Colors(k) Then
                        If SketchBox1.ColorScheme.transparencies(k) Then
                            bmap.SetPixel(i, j, Color.FromArgb(0, bmap.GetPixel(i, j)))
                        Else
                            bmap.SetPixel(i, j, Color.FromArgb(255, bmap.GetPixel(i, j)))
                        End If
                    End If
                Next
            Next
        Next

        If Not IsNothing(aPen) Then
            aPen.Dispose()
        End If
        If Not IsNothing(aBrush) Then
            aBrush.Dispose()

        End If
        aBrush = New TextureBrush(bmap)
        'aBrush = New TextureBrush(rotateImage(bmap, BrushAngle.Value))
        aPen = New Pen(aBrush, LineThickness.Value)

    End Sub
    Private Sub LineThickness_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LineThickness.ValueChanged
        If Not IsNothing(SketchBox1.LeftPen) Then
            SketchBox1.LeftPen.Dispose()
        End If
        If Not IsNothing(SketchBox1.RightPen) Then
            SketchBox1.RightPen.Dispose()
        End If
        If Not IsNothing(SketchBox1.LeftBrush) Then
            SketchBox1.LeftPen = New Pen(SketchBox1.LeftBrush, LineThickness.Value)
            LeftMousePaint.Invalidate()
        End If
        If Not IsNothing(SketchBox1.RightBrush) Then
            SketchBox1.RightPen = New Pen(SketchBox1.RightBrush, LineThickness.Value)
            RightMousePaint.Invalidate()
        End If
    End Sub

    Private Sub LayoutButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LayoutButton.Click
        If LayoutButton.ImageIndex + 1 = LayoutButton.ImageList.Images.Count Then
            LayoutButton.ImageIndex = 0
        Else
            LayoutButton.ImageIndex = LayoutButton.ImageIndex + 1
        End If
        setBrushPen(rightBmap, SketchBox1.RightPen, SketchBox1.RightBrush)
        RightMousePaint.Invalidate()
        setBrushPen(leftBmap, SketchBox1.LeftPen, SketchBox1.LeftBrush)
        LeftMousePaint.Invalidate()

    End Sub

    Private Sub SketchBox1_selectionChanged(ByVal bmap As System.Drawing.Bitmap, ByVal b As MouseButtons) Handles SketchBox1.selectionChanged
        If b = Windows.Forms.MouseButtons.Left Then
            leftBmap = bmap
            setBrushPen(leftBmap, SketchBox1.LeftPen, SketchBox1.LeftBrush)
            LeftMousePaint.Invalidate()
        Else
            rightBmap = bmap
            setBrushPen(rightBmap, SketchBox1.RightPen, SketchBox1.RightBrush)
            RightMousePaint.Invalidate()
        End If
    End Sub


    Private Sub FontButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FontButton.Click
        FontDialog1.ShowDialog()
        TextBox1.Font = FontDialog1.Font
        SketchBox1.DrawFont = TextBox1.Font
        TextBox1.Invalidate()
    End Sub

    Private Sub TextBox1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox1.TextChanged
        SketchBox1.DrawText = TextBox1.Text
    End Sub

    Private Sub SymmetryButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SymmetryButton.Click
        If ChartMode Then
            If sender.imageindex <> 0 Then
                sender.imageindex = 0
            Else
                sender.imageindex = 2
            End If
        Else
            If sender.imageindex = 12 Then
                sender.imageindex = 0
            Else
                sender.ImageIndex = sender.ImageIndex + 1
            End If
        End If
        SketchBox1.Symmetry = sender.imageindex
    End Sub

    Private Sub SketchBox1_transparencyChanged(ByVal aColor As System.Drawing.Color, ByVal aTransparency As System.Boolean) Handles SketchBox1.transparencyChanged

        setBrushPen(leftBmap, SketchBox1.LeftPen, SketchBox1.LeftBrush)
        setBrushPen(rightBmap, SketchBox1.RightPen, SketchBox1.RightBrush)

        LeftMousePaint.Invalidate()
        RightMousePaint.Invalidate()
        


    End Sub



    Private Sub CancelLink_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles CancelLink.LinkClicked
        UpdateStitchPat = False
        Chart = ""
        Me.Hide()
    End Sub

    Private Sub SketchBookCloseLink_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles SketchBookCloseLink.LinkClicked
        If ChartMode Then
            Chart = getChart()
        Else
            UpdateStitchPat = True
            StitchPatBmap = getStitchPatBmap()
        End If
        
        Me.Hide()
    End Sub
    Sub CopyChartToLeftBrush()
        Dim bmap As Bitmap
        Dim g2 As Graphics
        Dim g3 As Graphics
        Dim i, j, x1, y1, x2, y2 As Integer
        Dim chartRect As Rectangle
        x1 = -1 : y1 = -1 : x2 = -1 : y2 = -1

        chartRect.X = 0
        chartRect.Width = 180
        chartRect.Y = 0 'SketchBox1.StitchPatRect.Y
        chartRect.Height = 999 ' SketchBox1.StitchPatRect.Height

        bmap = New Bitmap(180, 999) 'Bitmap has one 1 pixel white border at right edge, therefore 181 rather than 180
        g2 = Graphics.FromImage(bmap)
        g2.Clear(Color.White) 'Ensure white border at right edge
        g2.DrawImage(SketchBox1.SketchImage, 0, 0, chartRect, GraphicsUnit.Pixel)


        For j = 0 To bmap.Height - 1

            For i = 0 To 179

                If bmap.GetPixel(i, j).ToArgb = Color.Black.ToArgb Then
                    If x1 = -1 Then x1 = i
                    If i < x1 Then x1 = i
                    If x2 < i Then x2 = i
                    If y1 = -1 Then y1 = j
                    If j < y1 Then y1 = j
                    If y2 < j Then y2 = j
                End If

            Next
        Next
        chartRect = New Rectangle(x1, y1, x2 - x1 + 1, y2 - y1 + 1)
        leftBmap = New Bitmap(x2 - x1 + 1, y2 - y1 + 1)
        g3 = Graphics.FromImage(leftBmap)
        g3.DrawImage(bmap, 0, 0, chartRect, GraphicsUnit.Pixel)

        setBrushPen(leftBmap, SketchBox1.LeftPen, SketchBox1.LeftBrush)
        LeftMousePaint.Invalidate()
        g2.Dispose()
        g3.Dispose()
        bmap.Dispose()
    End Sub
    Function getChart() As String
        Dim bmap As Bitmap
        Dim g2 As Graphics
        Dim i, j As Integer
        Dim chart, chartline, oldChartline, lastchartline, chartLineText As String
        Dim currColor As Integer
        Dim chartRect As Rectangle
        Dim FirstRowFound, firstblackfound, Changed As Boolean
        Dim rowcount As Integer
        Dim rows() As String
        Dim rc, oldrc As String
        'Dim lastRowcount As Integer
        Dim offset, lastrow As Integer
        Dim refCount As Integer

        chartRect.X = 0
        chartRect.Width = 180
        chartRect.Y = 0 'SketchBox1.StitchPatRect.Y
        chartRect.Height = 999 ' SketchBox1.StitchPatRect.Height
        FirstRowFound = False
        chartline = ""
        lastchartline = ""
        chart = ""

        lastrow = 999
        offset = 999
        rowcount = 0
        bmap = New Bitmap(181, 999) 'Bitmap has one 1 pixel white border at right edge, therefore 181 rather than 180
        g2 = Graphics.FromImage(bmap)
        g2.Clear(Color.White) 'Ensure white border at right edge
        g2.DrawImage(SketchBox1.SketchImage, 0, 0, chartRect, GraphicsUnit.Pixel)

        'chart = "<chart>" & Chr(13) & Chr(10)
        oldChartline = ""
        For j = bmap.Height - 1 To 0 Step -1
            currColor = Color.White.ToArgb
            'rowcount = offset - j
            chartline = ""
            firstblackfound = False
            For i = 0 To 180 'Bitmap has one 1 pixel white border at right edge, therefore 180 rather than 179

                Select Case bmap.GetPixel(i, j).ToArgb
                    Case Color.Black.ToArgb
                        If currColor = Color.White.ToArgb Then
                            firstblackfound = True
                            If Not FirstRowFound Then
                                offset = j
                                rowcount = (offset - j) * 2
                                FirstRowFound = True
                            End If
                            chartline = chartline & " " & Format(i + 1, "000")
                        Else

                        End If
                        currColor = Color.Black.ToArgb

                    Case Color.White.ToArgb
                        If currColor = Color.Black.ToArgb Then
                            chartline = chartline & " " & Format(i, "000")
                        ElseIf currColor = Color.Gray.ToArgb Then
                            chartline = chartline & " " & Format(i, "000") & " <change pattern>"
                        ElseIf currColor = Color.LightCoral.ToArgb Then
                            refCount = refCount + 1
                            chartline = chartline & " " & Format(i, "000") & " <ref " & refCount.ToString & ">"
                        End If
                        currColor = Color.White.ToArgb

                    Case Color.Gray.ToArgb
                        If currColor = Color.White.ToArgb Then
                            firstblackfound = True
                            If Not FirstRowFound Then
                                offset = j
                                rowcount = (offset - j) * 2
                                FirstRowFound = True
                            End If
                            chartline = chartline & " " & Format(i + 1, "000")
                        Else

                        End If
                        currColor = Color.Gray.ToArgb
                    Case Color.LightCoral.ToArgb
                        If currColor = Color.White.ToArgb Then
                            firstblackfound = True
                            If Not FirstRowFound Then
                                offset = j
                                rowcount = (offset - j) * 2
                                FirstRowFound = True
                            End If
                            chartline = chartline & " " & Format(i + 1, "000")
                        Else

                        End If
                        currColor = Color.LightCoral.ToArgb

                End Select

            Next
            If Not firstblackfound And FirstRowFound Then
                Exit For
            End If

            If chartline <> "" Then
                chart = chart & "RC" & Format(rowcount, "0000") & chartline & Chr(13) & Chr(10)
            End If

            rowcount = rowcount + Rowz.Value
        Next

        rowcount = rowcount - Rowz.Value 'Overshot by 2 so adjust.

        rows = chart.Replace(Chr(10), "").Split(Chr(13))
        chart = rows(0) & Chr(13) & Chr(10)
        oldrc = rows(0).Substring(0, 6)
        If rows(0).Substring(6).Contains("change pattern") Then
            oldChartline = rows(0).Substring(6).Split(" <change pattern>")(0)
            chartline = rows(0).Substring(6).Split(" <change pattern>")(0)
        ElseIf rows(0).Substring(6).Contains("ref") Then
            oldChartline = rows(0).Substring(6).Split(" <ref " & refCount.ToString & ">")(0)
            chartline = rows(0).Substring(6).Split(" <ref " & refCount.ToString & ">")(0)

        Else
            oldChartline = rows(0).Substring(6)
            chartline = rows(0).Substring(6)
        End If

        chartLineText = rows(0)
        rc = rows(0).Substring(6)

        For i = 1 To rows.Length - 1
            If rows(i).Length > 6 Then
                Changed = False
                rc = rows(i).Substring(0, 6)
                If rows(i).Substring(6).Contains("change pattern") Then
                    chartline = rows(i).Substring(6).Split("<change pattern>")(0).TrimEnd
                ElseIf rows(i).Substring(6).Contains("ref") Then
                    chartline = rows(i).Substring(6).Split("<ref " & refCount.ToString & ">")(0).TrimEnd
                Else
                    chartline = rows(i).Substring(6)
                End If
                chartLineText = rows(i)
                If (chartline <> oldChartline) Or chartLineText.Contains(" <change pattern>") Then
                    chart = chart & chartLineText & Chr(13) & Chr(10)
                    Changed = True
                ElseIf (chartline <> oldChartline) Or chartLineText.Contains("ref") Then '(" <ref refer " & refCount.ToString & ">") Then
                    chart = chart & chartLineText & Chr(13) & Chr(10)
                    Changed = True
                End If
                oldChartline = chartline
            End If
            oldrc = rc
        Next i
        If Not Changed Then
            chart = chart & rc & chartline & Chr(13) & Chr(10)
        End If

        chart = "<chart>" & Chr(13) & Chr(10) & chart
        chart = chart & "RT" & Format(rowcount, "0000") & Chr(13) & Chr(10)
        chart = chart & "RZ" & Format(Rowz.Value, "0000") & Chr(13) & Chr(10)
        getChart = chart
        g2.Dispose()
    End Function
    Function getStitchPatBmap() As Bitmap
        Dim bmap As Bitmap
        Dim g2 As Graphics

        bmap = New Bitmap(SketchBox1.StitchPatRect.Width, SketchBox1.StitchPatRect.Height)
        g2 = Graphics.FromImage(bmap)
        g2.DrawImage(SketchBox1.SketchImage, 0, 0, SketchBox1.StitchPatRect, GraphicsUnit.Pixel)
        getStitchPatBmap = bmap
        g2.Dispose()
    End Function


    Private Sub UpdateLink_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs)

    End Sub

    Private Sub SwatchRowsTextBox_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SwatchRowsTextBox.ValueChanged
        SketchBox1.RowGauge = sender.value / 400
        SketchBox1.SizeLabel.Invalidate()
    End Sub

    Private Sub SwatchStitchesTextBox_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SwatchStitchesTextBox.ValueChanged
        SketchBox1.StitchGauge = sender.value / 400
        SketchBox1.SizeLabel.Invalidate()
    End Sub


    Private Sub BrushAngle_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BrushAngle.ValueChanged

        ' SketchBox1.LeftBrush.Dispose()
        ' SketchBox1.LeftBrush = New TextureBrush(rotateImage(leftBmap, BrushAngle.Value))
        ' SketchBox1.RightBrush.Dispose()
        ' SketchBox1.RightBrush = New TextureBrush(rotateImage(rightBmap, BrushAngle.Value))
        
    End Sub
    Private Function rotateImage(ByVal bmap As Bitmap, ByVal angle As Single) As Bitmap
        Dim g2 As Graphics
        g2 = Graphics.FromImage(bmap)
        g2.PixelOffsetMode = Drawing2D.PixelOffsetMode.Half
        g2.SmoothingMode = Drawing2D.SmoothingMode.None
        g2.InterpolationMode = Drawing2D.InterpolationMode.NearestNeighbor
        g2.RotateTransform(angle)
        g2.DrawImage(bmap, 0, 0)
        rotateImage = bmap
        g2.Dispose()
    End Function

    Private Sub SketchBook_MouseDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles MyBase.MouseDoubleClick, PageDrawSketch.MouseDoubleClick, PageChooseStitchPattern.MouseDoubleClick
        'Dim p As Point
        If fullscreenmode Then

            Me.Width = 1000
            Me.Height = 600
            ' Me.PageDrawSketch.Width = 980
            'Me.PageDrawSketch.Height = 563
            ' Me.PageSelectPattern.Width = 980
            'Me.PageSelectPattern.Height = 563

            ' p.X = 9 : p.Y = 25
            'Me.PageDrawSketch.Location = p
            'Me.PageSelectPattern.Location = p

            'p.X = (My.Computer.Screen.WorkingArea.Size.Width - 1000) / 2
            'p.Y = (My.Computer.Screen.WorkingArea.Size.Height - 600) / 2
            Me.Location = SBP.Location
            Me.Width = SBP.Width
            Me.Height = SBP.Height

            fullscreenmode = False
        Else
            SBP.Location = Me.Location
            SBP.Width = Me.Width
            SBP.Height = Me.Height

            Me.Location = My.Computer.Screen.WorkingArea.Location
            Me.Size = My.Computer.Screen.WorkingArea.Size

            'Me.PageDrawSketch.Width = My.Computer.Screen.WorkingArea.Size.Width * 0.98
            'Me.PageDrawSketch.Height = My.Computer.Screen.WorkingArea.Size.Height * 0.93
            'Me.PageSelectPattern.Width = My.Computer.Screen.WorkingArea.Size.Width * 0.98
            'Me.PageSelectPattern.Height = My.Computer.Screen.WorkingArea.Size.Height * 0.93

            ' p.X = 0.009 * Me.Size.Width : p.Y = 0.046 * Me.Size.Height
            ' Me.PageDrawSketch.Location = p
            'Me.PageSelectPattern.Location = p

            fullscreenmode = True
        End If
        Me.PerformLayout()
    End Sub


    Private Sub Rowz_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Rowz.ValueChanged
        SketchBox1.Rowz = sender.value
    End Sub

    Private Sub LeftMousePaint_MouseDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles LeftMousePaint.MouseDoubleClick
        currentFill = "Left"
        PageDrawSketch.Hide()
        PageChooseStitchPattern.Show()
    End Sub

    Private Sub RightMousePaint_MouseDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles RightMousePaint.MouseDoubleClick
        currentFill = "Right"
        PageDrawSketch.Hide()
        PageChooseStitchPattern.Show()
    End Sub

    Private Sub StitchPatList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StitchPatList.SelectedIndexChanged
        If Not IsNothing(StitchPatList.DataSource) Then
            If Not IsNothing(StitchPatList.DataSource.current) Then
                If Not IsDBNull(StitchPatList.DataSource.current("BuiltIn")) Then
                    ViewStitchPatPictureBox.BuiltIn = StitchPatList.DataSource.current("BuiltIn")
                    ViewStitchPatPictureBox.Motif = StitchPatList.DataSource.current("Motif")
                    ViewStitchPatPictureBox.ColorCount = Me.SketchBox1.ColorCount
                    ViewStitchPatPictureBox.Color1 = Me.SketchBox1.Color1
                    ViewStitchPatPictureBox.Color2 = Me.SketchBox1.Color2
                    If Me.SketchBox1.ColorCount < 3 Then
                        ViewStitchPatPictureBox.Color3 = Me.SketchBox1.Color2
                    Else
                        ViewStitchPatPictureBox.Color3 = Me.SketchBox1.Color3
                    End If

                    If Me.SketchBox1.ColorCount < 4 Then
                        ViewStitchPatPictureBox.Color4 = Me.SketchBox1.Color2
                    Else
                        ViewStitchPatPictureBox.Color4 = Me.SketchBox1.Color4
                    End If

                    Me.ViewStitchPatPictureBox.Refresh()
                End If
            End If
        End If
    End Sub

    Private Sub ReturnToSketch_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles ReturnToSketch.LinkClicked
        PageChooseStitchPattern.Hide()
        PageDrawSketch.Show()

    End Sub

    Private Sub UpdateFillPatternLink_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles UpdateFillPatternLink.LinkClicked
        Dim width, height, i, j As Integer
        Dim aMotif As String
        If ViewStitchPatPictureBox.Motif <> "" Then
            width = Val(Mid(ViewStitchPatPictureBox.Motif, 3, 3))
            height = Val(Mid(ViewStitchPatPictureBox.Motif, 7, 3))
            aMotif = Mid(ViewStitchPatPictureBox.Motif, 11)
            If currentFill = "Left" Then
                leftBmap.Dispose()
                leftBmap = New Bitmap(width, height)
                For j = 0 To height - 1
                    For i = 0 To width - 1
                        leftBmap.SetPixel(i, j, ViewStitchPatPictureBox.ColorScheme.Colors(Val(Mid(aMotif, j * width + i + 1, 1))))
                    Next i
                Next j
                setBrushPen(leftBmap, SketchBox1.LeftPen, SketchBox1.LeftBrush)
            Else
                rightBmap.Dispose()
                rightBmap = New Bitmap(width, height)
                For j = 0 To height - 1
                    For i = 0 To width - 1
                        rightBmap.SetPixel(i, j, ViewStitchPatPictureBox.ColorScheme.Colors(Val(Mid(aMotif, j * width + i + 1, 1))))
                    Next i
                Next j
                setBrushPen(rightBmap, SketchBox1.RightPen, SketchBox1.RightBrush)
            End If

        End If
        PageChooseStitchPattern.Hide()
        PageDrawSketch.Show()
    End Sub

    Private Sub StitchPatListFilterLink_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles StitchPatListFilterLink.LinkClicked
        StitchPatListFilterText.Visible = True
        StitchPatListFilterText.Focus()
        StitchPatListFilterButton.Visible = True
        StitchPatListFilterLink.Visible = False
    End Sub

    Private Sub StitchPatListFilterButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StitchPatListFilterButton.Click
        StitchPatList.DataSource.Filter = makeFilter(StitchPatListFilterText.Text)
        StitchPatListFilterText.Visible = False
        StitchPatListFilterButton.Visible = False
        StitchPatListFilterLink.Text = "Stitch Patterns: " & StitchPatListFilterText.Text
        StitchPatListFilterLink.Visible = True

        StitchPatList.DataSource.MoveFirst()
        If StitchPatList.DataSource.Position >= 0 Then
            StitchPatList.SetSelected(StitchPatList.DataSource.Position, True)
        End If
    End Sub
    Private Function makeFilter(ByVal keywordString As String) As String
        Dim keys() As String
        keys = Split(keywordString)
        If LCase(keywordString) = "all" Or LCase(keywordString) = "" Or LCase(keywordString) = "*" Then
            makeFilter = ""
        Else
            makeFilter = "FilterText like "
            'makeFilter = "Name like "
            For Each key As String In keys
                makeFilter = makeFilter & "'%" & key & "%' and FilterText like "
                ' makeFilter = makeFilter & "'%" & key & "%' and Name like "

            Next
            makeFilter = Mid(makeFilter, 1, Len(makeFilter) - 21) 'strip trailing and like


        End If
    End Function

 
End Class