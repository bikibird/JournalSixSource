Public Class StitchPatternBox
    Inherits UserControl
    Private propMotif As String
    Private propDisplayOnly As Boolean
    Private propBuiltIn As Boolean
    Private Warning As Boolean
    
    Public Property Motif() As String
        Get
            Return propMotif
        End Get
        Set(ByVal value As String)
            propMotif = value
            SizeLabel.Text = IIf(IsNothing(value), "", Trim(Mid(value, 3, 3)) & " x " & Trim(Mid(value, 7, 3)))
            Dim motif As String
            Dim colorCount As Integer
            Dim colorMultiplier As Integer
            motif = Mid(value, 11)
            colorCount = Val(Mid(value, 1, 1))
            If colorCount > 2 Then
                colorMultiplier = 2
            Else
                colorMultiplier = 1
                colorCount = 0
            End If
            
            If Len(motif) * colorMultiplier > My.Settings.PatMax Then
                Warning = True
            Else
                Warning = False
            End If
        End Set
    End Property

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

    Public Property DisplayOnly() As Boolean
        Get
            Return propDisplayOnly
        End Get
        Set(ByVal value As Boolean)
            PropDisplayOnly = value
        End Set
    End Property
    Public Property BuiltIn() As Boolean
        Get
            Return propBuiltIn
        End Get
        Set(ByVal value As Boolean)
            propBuiltIn = value
        End Set
    End Property

    Private Sub MotifPictureBox_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles MotifPictureBox.Paint

        Dim width As Integer
        Dim height As Integer
        Dim aMotif As String
        Dim i, j As Integer
        Dim aBitmap As Bitmap
        Dim g As Graphics
        Dim hBrush As Drawing2D.HatchBrush
        Dim textBrush As SolidBrush
        Dim aFont, bFont As Font
        Dim sourceRect As Rectangle
        Dim destinationRect As Rectangle
        Dim s As SizeF
        Dim wBrush As New SolidBrush(Color.FromArgb(128, 255, 255, 255))

        g = e.Graphics
        hBrush = New Drawing2D.HatchBrush(Drawing2D.HatchStyle.SolidDiamond, Color.PaleGoldenrod, Color.Ivory)

        textBrush = New SolidBrush(Color.FromArgb(128, 139, 53, 19))
        aFont = New Font("Times New Roman", 54, FontStyle.Italic)
        bFont = New Font("Times New Roman", 16, FontStyle.Italic)

        If propMotif <> "" Then
            width = Val(Mid(propMotif, 3, 3))
            height = Val(Mid(propMotif, 7, 3))
            aMotif = Mid(propMotif, 11)

            aBitmap = New Bitmap(width, height)
            For j = 0 To height - 1
                For i = 0 To width - 1
                    aBitmap.SetPixel(i, j, Me.ColorScheme.Colors(Val(Mid(aMotif, j * width + i + 1, 1))))
                Next i
            Next j

            sourceRect = New Rectangle(0, 0, width, height)
            destinationRect = New Rectangle(0, 0, width * Magnification.Value, height * Magnification.Value / AspectRatio.Value)

            If width * Magnification.Value > 600 Then
                Me.MotifPictureBox.Width = width * Magnification.Value
            Else
                Me.MotifPictureBox.Width = 600
            End If

            If height * Magnification.Value > 600 Then
                Me.MotifPictureBox.Height = height * Magnification.Value / AspectRatio.Value
            Else
                Me.MotifPictureBox.Height = 600
            End If

            g.PixelOffsetMode = Drawing2D.PixelOffsetMode.Half
            g.SmoothingMode = Drawing2D.SmoothingMode.None
            g.InterpolationMode = Drawing2D.InterpolationMode.NearestNeighbor
            g.FillRectangle(hBrush, 0, 0, MotifPictureBox.Width, MotifPictureBox.Height)
            g.DrawImage(aBitmap, destinationRect, sourceRect, GraphicsUnit.Pixel)
            If propBuiltIn Then
                s = g.MeasureString("Console", aFont)
                g.FillRectangle(wBrush, 0, 0, s.Width, s.Height)
                g.DrawString("Console", aFont, textBrush, 0, 0)
            End If

            If Warning Then

                s = g.MeasureString("Motif will be split to fit console.", bFont)
                g.FillRectangle(wBrush, 0, 0, s.Width, s.Height)
                g.DrawString("Motif will be split to fit console.", bFont, textBrush, 0, 0)
            End If

        Else
            g.FillRegion(hBrush, g.Clip)
        End If

        hBrush.Dispose()
        aFont.Dispose()
        bFont.Dispose()
        textBrush.Dispose()
        wbrush.dispose()
    End Sub
    Private Sub MotifPictureBox_MouseClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles MotifPictureBox.MouseClick
        If Not propDisplayOnly Then
            Dim stitchColor As Integer
            Dim pt As Point
            Dim x As Integer
            Dim y As Integer
            Dim stitches, rows, motifPointer As Integer

            pt = New Point(e.X, e.Y)
            stitches = Val(Mid(propMotif, 3, 3))
            rows = Val(Mid(propMotif, 7, 3))
            x = Int(e.X / Me.Magnification.Value)
            y = Int(e.Y / Me.Magnification.Value * AspectRatio.Value)
            motifPointer = stitches * y + x + 11

            If motifPointer <= Len(Motif) And x < stitches Then 'if clicked inside pattern
                stitchColor = Val(Mid(Motif, motifPointer, 1))
                If stitchColor < ColorScheme.Count - 1 Then
                    propMotif = Mid(propMotif, 1, motifPointer - 1) & Trim(Str(stitchColor + 1)) & Mid(propMotif, motifPointer + 1)
                Else
                    propMotif = Mid(propMotif, 1, motifPointer - 1) & "0" & Mid(propMotif, motifPointer + 1)
                End If
            End If

            MotifPictureBox.Invalidate() 'repaint

        End If
    End Sub
    Private Sub Magnification_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Magnification.ValueChanged
        MotifPictureBox.Invalidate() 'repaint
    End Sub


    Private Sub AspectRatio_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AspectRatio.ValueChanged
        MotifPictureBox.Invalidate()
    End Sub
End Class
