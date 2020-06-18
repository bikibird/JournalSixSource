Public Class PassapMsg
    Dim ContinueCancel As Boolean
    Private Sub ContinueLink_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles ContinueLink.LinkClicked
        ContinueCancel = True
        Me.Hide()
    End Sub

    Private Sub PassapMsg_Shown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Shown
        ' My.Computer.Audio.Play(My.Resources.DING, AudioPlayMode.Background)
        My.Computer.Audio.PlaySystemSound(Media.SystemSounds.Question)
    End Sub
    Public Function display(ByVal aMsg As String, ByVal aResponse As Boolean, ByVal aResponseText As String) As Boolean
        Me.Msg.Text = aMsg
        Me.ResponseText.Text = aResponseText
        Me.ResponseText.Visible = False
        Me.ShowDialog()
        display = ContinueCancel
    End Function

    Private Sub CancelLink_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles CancelLink.LinkClicked
        ContinueCancel = False
        Me.Hide()
    End Sub
End Class