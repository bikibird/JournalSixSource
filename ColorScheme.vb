Public Class ColorScheme

    Private PropColors(3) As Color
    Private PropCount As Byte = 2
    Private PropTransparent(3) As Boolean
    Public Property Colors() As Color()
        Get
            Return PropColors
        End Get
        Set(ByVal myColors As Color())
            PropColors = myColors
            Me.Invalidate()
        End Set
    End Property
    Public Property transparencies() As Boolean()
        Get
            Return PropTransparent
        End Get
        Set(ByVal values As Boolean())
            PropTransparent = values
            Me.Invalidate()
        End Set
    End Property
    Public Property Color1() As Integer
        Get
            Return PropColors(0).ToArgb
        End Get
        Set(ByVal myColor As Integer)
            PropColors(0) = Color.FromArgb(myColor)
            Me.Invalidate()
        End Set
    End Property
    Public Property Color2() As Integer
        Get
            Return PropColors(1).ToArgb
        End Get
        Set(ByVal myColor As Integer)
            PropColors(1) = Color.FromArgb(myColor)
            Me.Invalidate()
        End Set
    End Property
    Public Property Color3() As Integer
        Get
            Return PropColors(2).ToArgb
        End Get
        Set(ByVal myColor As Integer)
            PropColors(2) = Color.FromArgb(myColor)
            Me.Invalidate()
        End Set
    End Property
    Public Property Color4() As Integer
        Get
            Return PropColors(3).ToArgb
        End Get
        Set(ByVal myColor As Integer)
            PropColors(3) = Color.FromArgb(myColor)
            Me.Invalidate()
        End Set
    End Property
    Public Property Transparent1() As Boolean
        Get
            Return PropTransparent(0)
        End Get
        Set(ByVal value As Boolean)
            PropTransparent(0) = value
            Me.Invalidate()
        End Set
    End Property
    Public Property Transparent2() As Boolean
        Get
            Return PropTransparent(1)
        End Get
        Set(ByVal value As Boolean)
            PropTransparent(1) = value
            Me.Invalidate()
        End Set
    End Property
    Public Property Transparent3() As Boolean
        Get
            Return PropTransparent(2)
        End Get
        Set(ByVal value As Boolean)
            PropTransparent(2) = value
            Me.Invalidate()
        End Set
    End Property
    Public Property Transparent4() As Boolean
        Get
            Return PropTransparent(3)
        End Get
        Set(ByVal value As Boolean)
            PropTransparent(3) = value
            Me.Invalidate()
        End Set
    End Property
    Public Property Count() As Byte
        Get
            Return PropCount
        End Get
        Set(ByVal myCount As Byte)
            If myCount > 0 Or myCount < 5 Then
                PropCount = myCount
            End If
            Me.Invalidate()
        End Set
    End Property

    Private Sub ColorScheme_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles MyBase.Paint
        Dim skinnypen As Pen
        Dim myPaintBrush As Brush
        Dim colorRect As Rectangle
        Dim hBrush As Drawing2D.HatchBrush
        
        colorRect.Height = Me.Height - 1
        colorRect.Width = Me.Width / 4 - 4
        colorRect.X = 0
        colorRect.Y = 0
        skinnypen = New Pen(Me.ForeColor, 1)

        e.Graphics.Clear(Me.BackColor)
        If PropCount > 0 Then
            For i As Byte = 0 To PropCount - 1
                If PropTransparent(i) Then
                    hBrush = New Drawing2D.HatchBrush(Drawing2D.HatchStyle.SolidDiamond, Color.PaleGoldenrod, Color.Ivory)
                    e.Graphics.FillRectangle(hBrush, colorRect)
                    e.Graphics.DrawRectangle(skinnypen, colorRect)
                    colorRect.X = colorRect.X + colorRect.Width + 4
                    hBrush.Dispose()
                Else
                    myPaintBrush = New SolidBrush(PropColors(i))
                    e.Graphics.FillRectangle(myPaintBrush, colorRect)
                    e.Graphics.DrawRectangle(skinnypen, colorRect)
                    colorRect.X = colorRect.X + colorRect.Width + 4
                    myPaintBrush.Dispose()
                End If
            Next
        End If
    End Sub

End Class
