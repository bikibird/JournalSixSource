<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class PassapMsg
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel
        Me.CancelLink = New System.Windows.Forms.LinkLabel
        Me.ContinueLink = New System.Windows.Forms.LinkLabel
        Me.ResponseText = New System.Windows.Forms.TextBox
        Me.Msg = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.TableLayoutPanel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.BackColor = System.Drawing.Color.Ivory
        Me.TableLayoutPanel1.ColumnCount = 2
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.CancelLink, 0, 3)
        Me.TableLayoutPanel1.Controls.Add(Me.ContinueLink, 1, 3)
        Me.TableLayoutPanel1.Controls.Add(Me.ResponseText, 0, 2)
        Me.TableLayoutPanel1.Controls.Add(Me.Msg, 0, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.Label1, 0, 0)
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(3, 3)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.Padding = New System.Windows.Forms.Padding(3)
        Me.TableLayoutPanel1.RowCount = 4
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 83.43558!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.56442!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(330, 200)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'CancelLink
        '
        Me.CancelLink.AutoSize = True
        Me.CancelLink.Dock = System.Windows.Forms.DockStyle.Left
        Me.CancelLink.Font = New System.Drawing.Font("Times New Roman", 9.75!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Italic), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CancelLink.LinkColor = System.Drawing.Color.SaddleBrown
        Me.CancelLink.Location = New System.Drawing.Point(8, 176)
        Me.CancelLink.Margin = New System.Windows.Forms.Padding(5)
        Me.CancelLink.Name = "CancelLink"
        Me.CancelLink.Size = New System.Drawing.Size(47, 16)
        Me.CancelLink.TabIndex = 8
        Me.CancelLink.TabStop = True
        Me.CancelLink.Text = "Cancel"
        Me.CancelLink.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ContinueLink
        '
        Me.ContinueLink.AutoSize = True
        Me.ContinueLink.Dock = System.Windows.Forms.DockStyle.Right
        Me.ContinueLink.Font = New System.Drawing.Font("Times New Roman", 9.75!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Italic), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ContinueLink.LinkColor = System.Drawing.Color.SaddleBrown
        Me.ContinueLink.Location = New System.Drawing.Point(263, 176)
        Me.ContinueLink.Margin = New System.Windows.Forms.Padding(5)
        Me.ContinueLink.Name = "ContinueLink"
        Me.ContinueLink.Size = New System.Drawing.Size(59, 16)
        Me.ContinueLink.TabIndex = 7
        Me.ContinueLink.TabStop = True
        Me.ContinueLink.Text = "Continue"
        Me.ContinueLink.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'ResponseText
        '
        Me.TableLayoutPanel1.SetColumnSpan(Me.ResponseText, 2)
        Me.ResponseText.Dock = System.Windows.Forms.DockStyle.Right
        Me.ResponseText.Font = New System.Drawing.Font("Times New Roman", 9.75!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ResponseText.Location = New System.Drawing.Point(8, 153)
        Me.ResponseText.Margin = New System.Windows.Forms.Padding(5)
        Me.ResponseText.Name = "ResponseText"
        Me.ResponseText.Size = New System.Drawing.Size(314, 22)
        Me.ResponseText.TabIndex = 5
        Me.ResponseText.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Msg
        '
        Me.Msg.AutoSize = True
        Me.TableLayoutPanel1.SetColumnSpan(Me.Msg, 2)
        Me.Msg.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Msg.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Msg.Location = New System.Drawing.Point(8, 35)
        Me.Msg.Margin = New System.Windows.Forms.Padding(5)
        Me.Msg.Name = "Msg"
        Me.Msg.Size = New System.Drawing.Size(314, 108)
        Me.Msg.TabIndex = 1
        Me.Msg.Text = "Label1"
        Me.Msg.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.TableLayoutPanel1.SetColumnSpan(Me.Label1, 2)
        Me.Label1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label1.Font = New System.Drawing.Font("Times New Roman", 12.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Italic), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(6, 3)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(318, 27)
        Me.Label1.TabIndex = 6
        Me.Label1.Text = "PASSAP Pal Message"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'PassapMsg
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.SaddleBrown
        Me.ClientSize = New System.Drawing.Size(336, 206)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ForeColor = System.Drawing.Color.SaddleBrown
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "PassapMsg"
        Me.Padding = New System.Windows.Forms.Padding(3)
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Passap Pal"
        Me.TopMost = True
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TableLayoutPanel1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents Msg As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents ContinueLink As System.Windows.Forms.LinkLabel
    Friend WithEvents ResponseText As System.Windows.Forms.TextBox
    Friend WithEvents CancelLink As System.Windows.Forms.LinkLabel
End Class
