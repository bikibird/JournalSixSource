Public Class CRTDiagram
    Inherits UserControl
    Private PropRows As Integer = 10
    Private PropTechBinary() As Byte
    Private PropCustom As Boolean
    Public Property Rows() As Integer
        Get
            Return PropRows
        End Get
        Set(ByVal value As Integer)
            If value < 1 Or value > 256 Then
                value = 1
            End If

            PropRows = value
            ReDim Preserve PropTechBinary(PropRows * 5 - 1)
            Me.RowsNumericUpDown.Value = value
            Me.CRTPictureBox.Height = PropRows * 7
            Me.SplitContainer1.Panel2.AutoScrollPosition = New Point(0, Me.CRTPictureBox.Height)
            Me.CRTPictureBox.Invalidate()
        End Set
    End Property
    Public Property TechBinary() As Byte()
        Get
            Return PropTechBinary
        End Get
        Set(ByVal value As Byte())
            PropTechBinary = value
        End Set
    End Property
    Public Property custom() As Boolean
        Get
            Return PropCustom
        End Get
        Set(ByVal value As Boolean)
            PropCustom = value

            Me.CustomCheckBox.Checked = value
            Me.RowsNumericUpDown.Enabled = value
            Me.ClearButton.Enabled = value
            Me.CRTPictureBox.Invalidate()
        End Set
    End Property

    Private Sub CRTPictureBox_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles CRTPictureBox.Paint
        Dim i, j, k As Short

        Dim skinnypen As Pen
        Dim paintBrush As Brush
        If PropCustom Then
            skinnypen = New Pen(Color.Black)
            paintBrush = Brushes.Black
        Else
            skinnypen = New Pen(Color.Silver)
            paintBrush = Brushes.Silver
        End If


        skinnypen.Width = 1
        skinnypen.DashStyle = Drawing2D.DashStyle.Dot
        'Vertical lines
        For i = -1 To 272 Step 7
            e.Graphics.DrawLine(skinnypen, i + 7, 0, i + 7, CRTPictureBox.ClientSize.Height - 1)
        Next
        'Horizontal lines

        For i = CRTPictureBox.ClientSize.Height - 1 To 0 Step -7
            e.Graphics.DrawLine(skinnypen, 0, i, 279, i)
        Next
        e.Graphics.DrawLine(skinnypen, 0, 0, 279, 0)
        If Not PropTechBinary Is Nothing Then
            If PropTechBinary.Length > 0 Then
                For i = 0 To PropTechBinary.GetUpperBound(0) Step 5 'rows
                    For j = 0 To 4 'bytes
                        For k = 0 To 7 'bits
                            If PropTechBinary(i + j) And (2 ^ (7 - k)) Then
                                e.Graphics.FillRectangle(paintBrush, (j * 8 + k) * 7, CInt(CRTPictureBox.ClientSize.Height - 7 - Int(i / 5) * 7), 6, 6)
                            Else
                                e.Graphics.FillRectangle(Brushes.White, (j * 8 + k) * 7, CInt(CRTPictureBox.ClientSize.Height - 7 - Int(i / 5) * 7), 6, 6)
                            End If
                        Next
                    Next
                Next
            End If
        End If
    End Sub

    Private Sub CRTPictureBox_MouseClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles CRTPictureBox.MouseClick
        If PropCustom Then
            Dim i, j, k As Short
            i = Int(e.X / 56) '7 pixels * 8 bits
            k = 7 - (Int(e.X / 7) Mod 8) '7 pixels mod 8 bits
            j = Int((CRTPictureBox.ClientSize.Height - e.Y) / 7) '7 pixels
            If j * 5 + i < PropTechBinary.Length Then
                PropTechBinary(j * 5 + i) = PropTechBinary(j * 5 + i) Xor (2 ^ k)
                CRTPictureBox.Invalidate()
            End If
        End If
    End Sub

    Private Sub SplitContainer1_Panel2_Scroll(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ScrollEventArgs) Handles SplitContainer1.Panel2.Scroll
        Me.CRTPictureBox.Invalidate()
    End Sub
    Private Sub RowsNumericUpDown_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RowsNumericUpDown.ValueChanged
        Me.Rows = RowsNumericUpDown.Value
    End Sub

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        ReDim Preserve PropTechBinary(PropRows * 5 - 1)
        Rows = PropRows
        Me.Invalidate()

    End Sub

    Private Sub ClearButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ClearButton.Click
        ReDim PropTechBinary(PropRows * 5 - 1)
        CRTPictureBox.Invalidate()
    End Sub

    Private Sub ClearButton_EnabledChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ClearButton.EnabledChanged
        If sender.enabled Then
            sender.forecolor = Color.SaddleBrown
        Else
            sender.forecolor = Color.Silver
        End If
    End Sub

    Private Sub CustomCheckBox_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CustomCheckBox.CheckedChanged
        Me.custom = CustomCheckBox.Checked
        Me.Invalidate()
    End Sub

    Private Sub CRTPictureBox_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles CRTPictureBox.MouseMove

        CoordianteLabel.Text = Trim(Str(PropRows - Int(e.Y / 7))) & ", " & Trim(Str(Int(e.X / 7 + 1)))

    End Sub
End Class
