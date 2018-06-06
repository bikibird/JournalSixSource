<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class CRTDiagram
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer
        Me.CustomCheckBox = New System.Windows.Forms.CheckBox
        Me.RowsNumericUpDown = New System.Windows.Forms.NumericUpDown
        Me.CRTPictureBox = New System.Windows.Forms.PictureBox
        Me.ClearButton = New System.Windows.Forms.Button
        Me.Label1 = New System.Windows.Forms.Label
        Me.CoordianteLabel = New System.Windows.Forms.Label
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        CType(Me.RowsNumericUpDown, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.CRTPictureBox, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'SplitContainer1
        '
        Me.SplitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.SplitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1
        Me.SplitContainer1.IsSplitterFixed = True
        Me.SplitContainer1.Location = New System.Drawing.Point(2, 2)
        Me.SplitContainer1.Margin = New System.Windows.Forms.Padding(0)
        Me.SplitContainer1.Name = "SplitContainer1"
        Me.SplitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.CustomCheckBox)
        Me.SplitContainer1.Panel1.Controls.Add(Me.RowsNumericUpDown)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.AutoScroll = True
        Me.SplitContainer1.Panel2.Controls.Add(Me.CRTPictureBox)
        Me.SplitContainer1.Size = New System.Drawing.Size(299, 135)
        Me.SplitContainer1.SplitterDistance = 32
        Me.SplitContainer1.SplitterWidth = 1
        Me.SplitContainer1.TabIndex = 4
        '
        'CustomCheckBox
        '
        Me.CustomCheckBox.AutoSize = True
        Me.CustomCheckBox.Font = New System.Drawing.Font("Times New Roman", 12.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Italic), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CustomCheckBox.ForeColor = System.Drawing.Color.SaddleBrown
        Me.CustomCheckBox.Location = New System.Drawing.Point(3, 4)
        Me.CustomCheckBox.Name = "CustomCheckBox"
        Me.CustomCheckBox.Size = New System.Drawing.Size(175, 23)
        Me.CustomCheckBox.TabIndex = 5
        Me.CustomCheckBox.Text = "Card Reader Diagram"
        Me.CustomCheckBox.UseVisualStyleBackColor = True
        '
        'RowsNumericUpDown
        '
        Me.RowsNumericUpDown.Font = New System.Drawing.Font("Gentium", 9.75!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RowsNumericUpDown.Location = New System.Drawing.Point(248, 4)
        Me.RowsNumericUpDown.Maximum = New Decimal(New Integer() {256, 0, 0, 0})
        Me.RowsNumericUpDown.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.RowsNumericUpDown.Name = "RowsNumericUpDown"
        Me.RowsNumericUpDown.Size = New System.Drawing.Size(46, 22)
        Me.RowsNumericUpDown.TabIndex = 4
        Me.RowsNumericUpDown.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'CRTPictureBox
        '
        Me.CRTPictureBox.BackColor = System.Drawing.Color.White
        Me.CRTPictureBox.Cursor = System.Windows.Forms.Cursors.Cross
        Me.CRTPictureBox.Location = New System.Drawing.Point(0, 0)
        Me.CRTPictureBox.Margin = New System.Windows.Forms.Padding(0)
        Me.CRTPictureBox.MaximumSize = New System.Drawing.Size(280, 1800)
        Me.CRTPictureBox.MinimumSize = New System.Drawing.Size(280, 8)
        Me.CRTPictureBox.Name = "CRTPictureBox"
        Me.CRTPictureBox.Size = New System.Drawing.Size(280, 140)
        Me.CRTPictureBox.TabIndex = 0
        Me.CRTPictureBox.TabStop = False
        '
        'ClearButton
        '
        Me.ClearButton.FlatAppearance.BorderColor = System.Drawing.Color.SaddleBrown
        Me.ClearButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.SaddleBrown
        Me.ClearButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Ivory
        Me.ClearButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ClearButton.Font = New System.Drawing.Font("Times New Roman", 8.25!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ClearButton.ForeColor = System.Drawing.Color.SaddleBrown
        Me.ClearButton.Location = New System.Drawing.Point(257, 141)
        Me.ClearButton.Name = "ClearButton"
        Me.ClearButton.Size = New System.Drawing.Size(44, 23)
        Me.ClearButton.TabIndex = 5
        Me.ClearButton.Text = "Clear"
        Me.ClearButton.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Times New Roman", 12.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Italic), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.SaddleBrown
        Me.Label1.Location = New System.Drawing.Point(3, 143)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(106, 19)
        Me.Label1.TabIndex = 6
        Me.Label1.Text = "Row, Column:"
        '
        'CoordianteLabel
        '
        Me.CoordianteLabel.AutoSize = True
        Me.CoordianteLabel.Font = New System.Drawing.Font("Times New Roman", 12.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Italic), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CoordianteLabel.ForeColor = System.Drawing.Color.SaddleBrown
        Me.CoordianteLabel.Location = New System.Drawing.Point(115, 143)
        Me.CoordianteLabel.Name = "CoordianteLabel"
        Me.CoordianteLabel.Size = New System.Drawing.Size(101, 19)
        Me.CoordianteLabel.TabIndex = 7
        Me.CoordianteLabel.Text = "Row, Column"
        '
        'CRTDiagram
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.BackColor = System.Drawing.Color.Ivory
        Me.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Controls.Add(Me.CoordianteLabel)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.ClearButton)
        Me.Controls.Add(Me.SplitContainer1)
        Me.MaximumSize = New System.Drawing.Size(400, 800)
        Me.MinimumSize = New System.Drawing.Size(300, 8)
        Me.Name = "CRTDiagram"
        Me.Size = New System.Drawing.Size(304, 169)
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel1.PerformLayout()
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        Me.SplitContainer1.ResumeLayout(False)
        CType(Me.RowsNumericUpDown, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.CRTPictureBox, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents CRTPictureBox As System.Windows.Forms.PictureBox
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents RowsNumericUpDown As System.Windows.Forms.NumericUpDown
    Friend WithEvents ClearButton As System.Windows.Forms.Button
    Friend WithEvents CustomCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents CoordianteLabel As System.Windows.Forms.Label

End Class
