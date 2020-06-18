Public Class NeedleDiagram
    Private PropRacking As Integer = 0
    Private PropBackPushers As String = Space(20)
    Private PropBackNeedles As String = Space(20)
    Private PropFrontNeedles As String = Space(20)
    Private PropFrontPushers As String = Space(20)
    Private PropDisplayOnly As Boolean = False

    Public Property Racking() As Integer
        Get
            Return PropRacking
        End Get
        Set(ByVal value As Integer)
            PropRacking = value
            RackingTrackBar.Value = value

        End Set
    End Property

    Public Property BackPushers() As String
        Get
            Return PropBackPushers
        End Get
        Set(ByVal value As String)
            PropBackPushers = value

        End Set
    End Property
    Public Property BackNeedles() As String
        Get
            Return PropBackNeedles
        End Get
        Set(ByVal value As String)
            PropBackNeedles = value

        End Set
    End Property
    Public Property FrontNeedles() As String
        Get
            Return PropFrontNeedles
        End Get
        Set(ByVal value As String)
            PropFrontNeedles = value

        End Set
    End Property
    Public Property FrontPushers() As String
        Get
            Return PropFrontPushers
        End Get
        Set(ByVal value As String)
            PropFrontPushers = value

        End Set
    End Property
    Public Property DisplayOnly() As String
        Get
            Return PropDisplayOnly
        End Get
        Set(ByVal value As String)
            PropDisplayOnly = value
            If PropDisplayOnly Then
                RackingTrackBar.Visible = False
            Else
                RackingTrackBar.Visible = True

            End If

        End Set
    End Property


    Private Sub DiagramPictureBox_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles DiagramPictureBox.Paint

        Dim WidePen, SkinnyPen, solidPen As Pen
        Dim blackBrush As New SolidBrush(Color.Black)
        Dim Instructions(3) As String
        Dim i, j As Byte
        Dim x(3) As Short

        If IsNothing(Racking) Then
            e.Graphics.Clear(Color.Silver)
        Else
            WidePen = New Pen(Color.Black)
            WidePen.EndCap = Drawing2D.LineCap.Round
            WidePen.StartCap = Drawing2D.LineCap.Round
            WidePen.Width = 5

            SkinnyPen = New Pen(Color.Black)
            SkinnyPen.Width = 1
            SkinnyPen.DashStyle = Drawing2D.DashStyle.Dot

            solidPen = New Pen(Color.Black)
            solidPen.Width = 5
            solidPen.StartCap = Drawing2D.LineCap.Flat
            solidPen.EndCap = Drawing2D.LineCap.Flat
            e.Graphics.Clear(Me.BackColor)

            'e.Graphics.DrawLine(solidPen, 1, 50, e.Graphics.ClipBounds.Width - 1, 50)

            Instructions(0) = "" : Instructions(1) = "" : Instructions(2) = "" : Instructions(3) = ""
            If Racking > 0 Then
                Instructions(0) = IIf(Not IsNothing(PropBackPushers), PropBackPushers, Space(20))
                Instructions(1) = IIf(Not IsNothing(PropBackNeedles), PropBackNeedles, Space(20))
                Instructions(2) = StrDup(Racking, "R") & IIf(Not IsNothing(PropFrontNeedles), PropFrontNeedles, Space(20))
                Instructions(3) = StrDup(Racking, "R") & IIf(Not IsNothing(PropFrontPushers), PropFrontPushers, Space(20))
            Else
                Instructions(0) = StrDup(Math.Abs(Racking), "R") & IIf(Not IsNothing(PropBackPushers), PropBackPushers, Space(20))
                Instructions(1) = StrDup(Math.Abs(Racking), "R") & IIf(Not IsNothing(PropBackNeedles), PropBackNeedles, Space(20))
                Instructions(2) = IIf(Not IsNothing(PropFrontNeedles), PropFrontNeedles, "                    ")
                Instructions(3) = IIf(Not IsNothing(PropFrontPushers), PropFrontPushers, "                    ")
            End If
            If PropDisplayOnly Then
                Instructions(0) = RTrim(Instructions(0))
                Instructions(1) = RTrim(Instructions(1))
                Instructions(2) = RTrim(Instructions(2))
                Instructions(3) = RTrim(Instructions(3))

            End If
            x(0) = 4 : x(1) = 3 : x(2) = 3 : x(3) = 3

            For j = 0 To 3
                If Len(Instructions(j)) > 0 Then
                    For i = 0 To Len(Instructions(j)) - 1

                        Select Case Instructions(j).Substring(i, 1)
                            Case "R"
                                x(j) = x(j) + 3
                            Case " "
                                Select Case j
                                    Case 0
                                        e.Graphics.DrawRectangle(SkinnyPen, x(j) - 3, 13, 4, 8)
                                    Case 1
                                        e.Graphics.DrawLine(SkinnyPen, x(j), 30, x(j), 50)
                                        e.Graphics.DrawLine(solidPen, x(j) - 6, 50, x(j) + 6, 50)
                                    Case 2
                                        e.Graphics.DrawLine(SkinnyPen, x(j), 50, x(j), 70)
                                        e.Graphics.DrawLine(solidPen, x(j) - 6, 50, x(j) + 6, 50)
                                    Case 3
                                        e.Graphics.DrawEllipse(SkinnyPen, x(j) - 3, 80, 6, 6)
                                End Select
                                x(j) = x(j) + 12
                            Case "N"
                                Select Case j
                                    Case 0
                                    Case 1
                                        e.Graphics.DrawLine(WidePen, x(j), 30, x(j), 50)
                                        e.Graphics.DrawLine(solidPen, x(j) - 6, 50, x(j) + 6, 50)
                                    Case 2
                                        e.Graphics.DrawLine(WidePen, x(j), 50, x(j), 70)
                                        e.Graphics.DrawLine(solidPen, x(j) - 6, 50, x(j) + 6, 50)
                                    Case 3
                                End Select
                                x(j) = x(j) + 12

                            Case "U"
                                Select Case j
                                    Case 0

                                        e.Graphics.FillRectangle(Brushes.Black, x(j) - 3, 13, 5, 8)
                                    Case 1
                                    Case 2
                                    Case 3
                                        e.Graphics.FillEllipse(Brushes.Black, x(j) - 3, 80, 7, 7)
                                End Select
                                x(j) = x(j) + 12
                            Case "D"
                                Select Case j
                                    Case 0

                                        e.Graphics.FillRectangle(Brushes.Black, x(j) - 3, 3, 5, 8)
                                        e.Graphics.DrawRectangle(SkinnyPen, x(j) - 3, 13, 4, 8)
                                    Case 1
                                    Case 2
                                    Case 3
                                        e.Graphics.FillEllipse(Brushes.Black, x(j) - 3, 90, 7, 7)
                                        e.Graphics.DrawEllipse(SkinnyPen, x(j) - 3, 80, 6, 6)
                                End Select
                                x(j) = x(j) + 12

                        End Select
                    Next i
                End If
            Next j
        End If
    End Sub

    Private Sub DiagramPictureBox_MouseClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles DiagramPictureBox.MouseClick
        Dim x As Integer
        Dim curSetting As String


        If Not PropDisplayOnly Then
            Select Case e.Y
                Case 0 To 20 'Back Pushers
                    If Racking < 0 Then
                        x = Int((e.X + 4 + 3 * Racking) / 12) + 1
                    Else
                        x = Int((e.X + 4) / 12) + 1
                    End If
                    curSetting = Mid(PropBackPushers, x, 1)

                    Select Case curSetting
                        Case "U"
                            If e.Button = Windows.Forms.MouseButtons.Left Then
                                If PropBackPushers.Length > x Then
                                    If x > 1 Then
                                        PropBackPushers = Mid(PropBackPushers, 1, x - 1) & "D" & Mid(PropBackPushers, x + 1)
                                    Else
                                        PropBackPushers = "D" & Mid(PropBackPushers, 2)
                                    End If
                                Else
                                    PropBackPushers = PropBackPushers & "D"
                                End If
                            Else
                                PropBackPushers = StrDup(PropBackPushers.Length, "D")
                            End If
                            Exit Select

                        Case "D"
                            If e.Button = Windows.Forms.MouseButtons.Left Then
                                If PropBackPushers.Length > x Then
                                    If x > 1 Then
                                        PropBackPushers = Mid(PropBackPushers, 1, x - 1) & " " & Mid(PropBackPushers, x + 1)
                                    Else
                                        PropBackPushers = " " & Mid(PropBackPushers, 2)
                                    End If
                                Else
                                    PropBackPushers = PropBackPushers & " "
                                End If

                            Else
                                PropBackPushers = StrDup(PropBackPushers.Length, " ")
                            End If
                            Exit Select
                        Case " "
                            If e.Button = Windows.Forms.MouseButtons.Left Then
                                If PropBackPushers.Length > x Then
                                    If x > 1 Then
                                        PropBackPushers = Mid(PropBackPushers, 1, x - 1) & "U" & Mid(PropBackPushers, x + 1)
                                    Else
                                        PropBackPushers = "U" & Mid(PropBackPushers, 2)
                                    End If
                                Else
                                    PropBackPushers = PropBackPushers & "U"
                                End If
                            Else
                                PropBackPushers = StrDup(PropBackPushers.Length, "U")
                            End If
                            Exit Select
                    End Select
                Case 30 To 50 'Back Needles
                    If Racking < 0 Then
                        x = Int((e.X + 4 + 3 * Racking) / 12) + 1
                    Else
                        x = Int((e.X + 4) / 12) + 1
                    End If
                    curSetting = Mid(PropBackNeedles, x, 1)
                    Select Case curSetting

                        Case "N"
                            If e.Button = Windows.Forms.MouseButtons.Left Then
                                If PropBackNeedles.Length > x Then
                                    If x > 1 Then
                                        PropBackNeedles = Mid(PropBackNeedles, 1, x - 1) & " " & Mid(PropBackNeedles, x + 1)
                                    Else
                                        PropBackNeedles = " " & Mid(PropBackNeedles, 2)
                                    End If
                                Else
                                    PropBackNeedles = PropBackNeedles & " "
                                End If
                            Else
                                PropBackNeedles = StrDup(PropBackNeedles.Length, " ")
                            End If
                            Exit Select
                        Case " "
                            If e.Button = Windows.Forms.MouseButtons.Left Then
                                If PropBackNeedles.Length > x Then
                                    If x > 1 Then
                                        PropBackNeedles = Mid(PropBackNeedles, 1, x - 1) & "N" & Mid(PropBackNeedles, x + 1)
                                    Else
                                        PropBackNeedles = "N" & Mid(PropBackNeedles, 2)
                                    End If
                                Else
                                    PropBackNeedles = PropBackNeedles & "N"
                                End If
                            Else
                                PropBackNeedles = StrDup(PropBackNeedles.Length, "N")
                            End If
                            Exit Select
                    End Select
                Case 50 To 70 'Front Needles
                    If Racking < 0 Then
                        x = Int((e.X + 4) / 12) + 1

                    Else
                        x = Int((e.X + 4 - 3 * Racking) / 12) + 1
                    End If
                    curSetting = Mid(PropFrontNeedles, x, 1)
                    Select Case curSetting
                        Case "N"
                            If e.Button = Windows.Forms.MouseButtons.Left Then
                                If PropFrontNeedles.Length > x Then
                                    If x > 1 Then
                                        PropFrontNeedles = Mid(PropFrontNeedles, 1, x - 1) & " " & Mid(PropFrontNeedles, x + 1)
                                    Else
                                        PropFrontNeedles = " " & Mid(PropFrontNeedles, 2)
                                    End If
                                Else
                                    PropFrontNeedles = PropFrontNeedles & " "
                                End If
                            Else
                                PropFrontNeedles = StrDup(PropFrontNeedles.Length, " ")
                            End If

                            Exit Select
                        Case " "
                            If e.Button = Windows.Forms.MouseButtons.Left Then
                                If PropFrontNeedles.Length > x Then
                                    If x > 1 Then
                                        PropFrontNeedles = Mid(PropFrontNeedles, 1, x - 1) & "N" & Mid(PropFrontNeedles, x + 1)
                                    Else
                                        PropFrontNeedles = "N" & Mid(PropFrontNeedles, 2)
                                    End If
                                Else
                                    PropFrontNeedles = PropFrontNeedles & "N"
                                End If
                            Else
                                PropFrontNeedles = StrDup(PropFrontNeedles.Length, "N")
                            End If
                            Exit Select
                    End Select


                Case 80 To 99 'Front Pushers
                    If Racking < 0 Then
                        x = Int((e.X + 4) / 12) + 1

                    Else
                        x = Int((e.X + 4 - 3 * Racking) / 12) + 1
                    End If
                    curSetting = Mid(PropFrontPushers, x, 1)
                    Select Case curSetting
                        Case "U"
                            If e.Button = Windows.Forms.MouseButtons.Left Then
                                If PropFrontPushers.Length > x Then
                                    If x > 1 Then
                                        PropFrontPushers = Mid(PropFrontPushers, 1, x - 1) & " " & Mid(PropFrontPushers, x + 1)
                                    Else
                                        PropFrontPushers = " " & Mid(PropFrontPushers, 2)
                                    End If
                                Else
                                    PropFrontPushers = PropFrontPushers & " "
                                End If
                            Else
                                PropFrontPushers = StrDup(PropFrontPushers.Length, " ")
                            End If

                            Exit Select


                        Case " "
                            If e.Button = Windows.Forms.MouseButtons.Left Then
                                If PropFrontPushers.Length > x Then
                                    If x > 1 Then
                                        PropFrontPushers = Mid(PropFrontPushers, 1, x - 1) & "U" & Mid(PropFrontPushers, x + 1)
                                    Else
                                        PropFrontPushers = "U" & Mid(PropFrontPushers, 2)
                                    End If
                                Else
                                    PropFrontPushers = PropFrontPushers & "U"
                                End If
                            Else
                                PropFrontPushers = StrDup(PropFrontPushers.Length, "U")
                            End If
                            Exit Select
                    End Select
            End Select
            DiagramPictureBox.Refresh()
        End If

    End Sub

    
    Private Sub RackingTrackBar_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RackingTrackBar.ValueChanged
        PropRacking = sender.value
        DiagramPictureBox.Refresh()
    End Sub


    Private Sub FlowLayoutPanel1_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FlowLayoutPanel1.Resize
        DiagramPictureBox.Width = sender.Size.Width
        RackingTrackBar.Width = sender.size.width
    End Sub
End Class

