Public Class Cropper

    Private Sub ContinueLink_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles ContinueLink.LinkClicked
        Me.Hide()
    End Sub

    Private Sub Cropper_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Me.SketchBox1.GuidelinesButton.Visible = False
        Me.SketchBox1.LassoButton.Visible = False
        Me.SketchBox1.SketchTool = "Lasso"
        Me.SketchBox1.Clip = True
        Me.SketchBox1.Magnification.Value = 1
    End Sub
End Class