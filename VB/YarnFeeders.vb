Public Class YarnFeeders
    Public Enum FeederStatus As Integer
        Full = 1
        Empty = 0
        OutOfUse = -1
    End Enum

    Private PropFeeders(3) As FeederStatus
    Private PropDisplayOnly As Boolean
  
    Public Property Feeder1() As Integer
        Get
            Return PropFeeders(0)
        End Get
        Set(ByVal myFeeder As Integer)
            PropFeeders(0) = myFeeder
        End Set
    End Property

    Public Property Feeder2() As Integer
        Get
            Return PropFeeders(1)
        End Get
        Set(ByVal myFeeder As Integer)
            PropFeeders(1) = myFeeder
            PictureBox1.Refresh()
        End Set
    End Property

    Public Property Feeder3() As Integer
        Get
            Return PropFeeders(2)
        End Get
        Set(ByVal myFeeder As Integer)
            PropFeeders(2) = myFeeder

        End Set
    End Property

    Public Property Feeder4() As Integer
        Get
            Return PropFeeders(3)
        End Get
        Set(ByVal myFeeder As Integer)
            PropFeeders(3) = myFeeder

        End Set
    End Property

    Public Property DisplayOnly() As Boolean
        Get
            Return PropDisplayOnly
        End Get
        Set(ByVal Value As Boolean)
            PropDisplayOnly = Value
        End Set

    End Property


    Private Sub YarnFeeders_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles PictureBox1.Paint
        Dim skinnypen As Pen
        Dim FeederRect As Rectangle
        Dim FeederCenter As Rectangle
        Dim YarnRect As Rectangle

        FeederRect.Height = Me.Height - 2
        FeederRect.Width = Me.Width / 4 - 4
        FeederRect.X = 1
        FeederRect.Y = 1

        YarnRect.Height = FeederRect.Height - 6
        YarnRect.Width = FeederRect.Width - 6
        YarnRect.X = 4
        YarnRect.Y = 4

        FeederCenter.Height = YarnRect.Height / 2
        FeederCenter.Width = YarnRect.Width / 2
        FeederCenter.X = 1 + FeederRect.Width / 2 - FeederCenter.Width / 2
        FeederCenter.Y = 1 + FeederRect.Height / 2 - FeederCenter.Height / 2

        e.Graphics.Clear(Me.BackColor)

        For i As Integer = 0 To 3
  
            Select Case PropFeeders(i)
                Case FeederStatus.Empty
                    skinnypen = New Pen(Me.ForeColor, 2)
                    e.Graphics.DrawEllipse(skinnypen, FeederRect)
                    e.Graphics.FillEllipse(New SolidBrush(Me.ForeColor), FeederCenter)
                Case FeederStatus.Full
                    skinnypen = New Pen(Me.ForeColor, 2)
                    e.Graphics.DrawEllipse(skinnypen, FeederRect)
                    e.Graphics.FillEllipse(New SolidBrush(Me.ForeColor), YarnRect)
                Case FeederStatus.OutOfUse
                    skinnypen = New Pen(Me.ForeColor, 2)
                    skinnypen.DashStyle = Drawing2D.DashStyle.Dot
                    e.Graphics.DrawEllipse(skinnypen, FeederRect)
            End Select

            FeederRect.X = FeederRect.X + FeederRect.Width + 4
            YarnRect.X = FeederRect.X + 3
            FeederCenter.X = FeederRect.X + FeederRect.Width / 2 - FeederCenter.Width / 2
            FeederCenter.Y = FeederRect.Y + FeederRect.Height / 2 - FeederCenter.Height / 2
        Next
    End Sub

    Private Sub PictureBox1_MouseClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles PictureBox1.MouseClick
        Dim i As Short
        If Not PropDisplayOnly Then
            i = Int(e.X / (Me.Width / 4))
            Select Case PropFeeders(i)
                Case FeederStatus.OutOfUse
                    PropFeeders(i) = FeederStatus.Full
                    Exit Select
                Case FeederStatus.Empty
                    PropFeeders(i) = FeederStatus.OutOfUse
                    Exit Select
                Case FeederStatus.Full
                    PropFeeders(i) = FeederStatus.Empty
                    Exit Select
            End Select
        
            PictureBox1.Refresh()

        End If
    End Sub
End Class
