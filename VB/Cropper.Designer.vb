<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Cropper
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel
        Me.SketchBox1 = New Journal6.SketchBox
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.ContinueLink = New System.Windows.Forms.LinkLabel
        Me.TableLayoutPanel1.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.BackColor = System.Drawing.Color.Ivory
        Me.TableLayoutPanel1.ColumnCount = 1
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle)
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.SketchBox1, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.Panel1, 0, 1)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(5, 5)
        Me.TableLayoutPanel1.Margin = New System.Windows.Forms.Padding(0)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 2
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 93.41637!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.58363!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(790, 590)
        Me.TableLayoutPanel1.TabIndex = 1
        '
        'SketchBox1
        '
        Me.SketchBox1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.SketchBox1.CenterPt = New System.Drawing.Point(0, 0)
        Me.SketchBox1.chartTemplate = Nothing
        Me.SketchBox1.Clip = False
        Me.SketchBox1.Color1 = -16
        Me.SketchBox1.Color2 = -16
        Me.SketchBox1.Color3 = -16
        Me.SketchBox1.Color4 = -16
        Me.SketchBox1.ColorCount = 4
        Me.SketchBox1.Dock = System.Windows.Forms.DockStyle.Left
        Me.SketchBox1.DrawFont = Nothing
        Me.SketchBox1.DrawText = Nothing
        Me.SketchBox1.LeftBrush = Nothing
        Me.SketchBox1.LeftPen = Nothing
        Me.SketchBox1.Location = New System.Drawing.Point(5, 5)
        Me.SketchBox1.Margin = New System.Windows.Forms.Padding(5)
        Me.SketchBox1.Motif = Nothing
        Me.SketchBox1.Name = "SketchBox1"
        Me.SketchBox1.RightBrush = Nothing
        Me.SketchBox1.RightPen = Nothing
        Me.SketchBox1.RowGauge = 0
        Me.SketchBox1.Rowz = 0
        Me.SketchBox1.Size = New System.Drawing.Size(780, 541)
        Me.SketchBox1.SketchBrush = Nothing
        Me.SketchBox1.SketchImage = Nothing
        Me.SketchBox1.SketchMode = Nothing
        Me.SketchBox1.SketchPen = Nothing
        Me.SketchBox1.SketchTool = Nothing
        Me.SketchBox1.StitchGauge = 0
        Me.SketchBox1.Symmetry = 0
        Me.SketchBox1.TabIndex = 0
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.Ivory
        Me.Panel1.Controls.Add(Me.ContinueLink)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(3, 554)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(794, 33)
        Me.Panel1.TabIndex = 1
        '
        'ContinueLink
        '
        Me.ContinueLink.ActiveLinkColor = System.Drawing.Color.SaddleBrown
        Me.ContinueLink.AutoSize = True
        Me.ContinueLink.BackColor = System.Drawing.Color.Ivory
        Me.ContinueLink.DisabledLinkColor = System.Drawing.Color.Peru
        Me.ContinueLink.Dock = System.Windows.Forms.DockStyle.Right
        Me.ContinueLink.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ContinueLink.LinkColor = System.Drawing.Color.SaddleBrown
        Me.ContinueLink.Location = New System.Drawing.Point(706, 0)
        Me.ContinueLink.Margin = New System.Windows.Forms.Padding(0)
        Me.ContinueLink.Name = "ContinueLink"
        Me.ContinueLink.Padding = New System.Windows.Forms.Padding(0, 0, 20, 0)
        Me.ContinueLink.Size = New System.Drawing.Size(88, 19)
        Me.ContinueLink.TabIndex = 67
        Me.ContinueLink.TabStop = True
        Me.ContinueLink.Text = "Continue"
        Me.ContinueLink.VisitedLinkColor = System.Drawing.Color.SaddleBrown
        '
        'Cropper
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.SaddleBrown
        Me.ClientSize = New System.Drawing.Size(800, 600)
        Me.ControlBox = False
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.ForeColor = System.Drawing.Color.SaddleBrown
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.MaximizeBox = False
        Me.Name = "Cropper"
        Me.Padding = New System.Windows.Forms.Padding(5)
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.Text = "Crop Image"
        Me.TopMost = True
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents SketchBox1 As Journal6.SketchBox
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents ContinueLink As System.Windows.Forms.LinkLabel
End Class
