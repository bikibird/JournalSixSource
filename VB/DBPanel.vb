Public Class DBPanel
    Inherits System.Windows.Forms.Panel

    Public Sub New()

        MyBase.New()

        
        ' Me.SetStyle(ControlStyles.OptimizedDoubleBuffer Or ControlStyles.AllPaintingInWmPaint Or ControlStyles.Opaque Or ControlStyles.UserPaint, True)
        Me.DoubleBuffered = True

    End Sub
    
    'End Sub
End Class
