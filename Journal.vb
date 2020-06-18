Imports System.Xml

#Const Version = "Full" 'Trial or Full

Public Class Journal
    Public Structure DPal
        Dim color As Color
        Dim ditherMatrix(,) As Byte 'array containing palette indices of 2x2 dither matrix, example: (0,1),(1,0)
    End Structure
    Dim PurePalette(3) As DPal 'color palette of stitch pattern
    Dim ColorDistanceTable(255, 255) As Integer
    Dim DitheredPalette(21) As DPal
    Dim CutPalette(255) As Color
    Dim DragColor As Color
    Dim im As Image
    Dim bmap As Bitmap
    Dim result As Bitmap
    Dim sourceBMap As Bitmap 'Reference Image bitmap on image page

    Dim motifResult As String
    Dim dragging As Boolean
    Dim pointClicked As Point
    Dim SilverKnit As String
    Dim fullscreenmode As Boolean
    Dim DBRestore As Boolean

    Private Sub Journal_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
       
        'TODO: This line of code loads data into the 'JournalSixDataSet1.PiecePattern' table. You can move, or remove it, as needed.
        '     Me.PiecePatternTableAdapter.Fill(Me.JournalSixDataSet1.PiecePattern)
        'TODO: This line of code loads data into the 'JournalSixDataSet.Piece' table. You can move, or remove it, as needed.
        '    Me.PieceTableAdapter.Fill(Me.JournalSixDataSet1.Piece)
        'TODO: This line of code loads data into the 'JournalSixDataSet.Pattern' table. You can move, or remove it, as needed.
        '   Me.PatternTableAdapter.Fill(Me.JournalSixDataSet1.Pattern)

        'Reset data directory to correct location for Vista and Windows 7
        Dim appPath As String
        Dim dataPath As String
        dataPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
        appPath = Application.StartupPath

#If Version = "Trial" Then
        DemoLabel.Visible = True
        PurchaseLink.Visible = True
        AppDomain.CurrentDomain.SetData("DataDirectory", dataPath & "\JournalSixV9Demo")
        If Not My.Computer.FileSystem.FileExists(dataPath & "\JournalSixV9Demo\JournalSix.sdf") Then
            My.Computer.FileSystem.CopyFile(appPath & "\JournalSix.sdf", dataPath & "\JournalSixV9Demo\JournalSix.sdf")
        End If
#Else
        DemoLabel.Visible = False
        PurchaseLink.Visible = False
        AppDomain.CurrentDomain.SetData("DataDirectory", dataPath & "\JournalSixV9")
        If Not My.Computer.FileSystem.FileExists(dataPath & "\JournalSixV9\JournalSix.sdf") Then
            My.Computer.FileSystem.CopyFile(appPath & "\JournalSix.sdf", dataPath & "\JournalSixV9\JournalSix.sdf")
        End If
#End If
       

        'New blank image for image page so that paint for sourcebox has something to works with.

        im = New Bitmap(40, 40)
        result = New Bitmap(40, 40)

        'initiallize color distance table for fast color math later
        Dim i As Integer
        Dim j As Integer
        For i = 0 To 255
            For j = 0 To 255
                ColorDistanceTable(i, j) = Math.Abs(i * i - j * j)
            Next j
        Next i


        'initialize dither matrices for 22 colors built out of four dithered colors
        ReDim DitheredPalette(21)

        CutPalette = loadPal()

        Me.ProjectTableAdapter.Fill(Me.JournalSixDataSet1.Project)
        Me.FormTableAdapter.Fill(Me.JournalSixDataSet1.Form)
        Me.PieceTableAdapter.Fill(Me.JournalSixDataSet1.Piece)
        Me.TechniqueTableAdapter.Fill(Me.JournalSixDataSet1.Technique)
        Me.PatternTableAdapter.Fill(Me.JournalSixDataSet1.Pattern)
        Me.PiecePatternTableAdapter.Fill(Me.JournalSixDataSet1.PiecePattern)

        SilverKnit = My.Computer.Registry.GetValue("HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\SkUtil.exe", "", "")




        For Each sp As String In My.Computer.Ports.SerialPortNames
            SettingComPortText.Items.Add(sp)
        Next

        Me.PageWelcome.BringToFront()
        setSettings()
        Me.Activate()
#If Version = "Trial" Then

        PassapMsg.display("You are using the demonstration version of Journal Six. This version of the software will not save any of your work once you close it.", False, "")


#End If
        fullscreenmode = False
    End Sub
    Private Sub setSettings()
        Me.E6000Port.BaudRate = 1200
        Me.E6000Port.Parity = IO.Ports.Parity.Even
        Me.E6000Port.DataBits = 8
        Me.E6000Port.StopBits = IO.Ports.StopBits.One
        Me.E6000Port.WriteTimeout = IO.Ports.SerialPort.InfiniteTimeout

        If My.Computer.Ports.SerialPortNames.Contains(My.Settings.ComPort) Then
            Try
                Me.E6000Port.PortName = My.Settings.ComPort
                Me.E6000Port.Close()
                Me.E6000Port.Open()
                Me.E6000Port.Close()
            Catch ex As Exception
                PassapMsg.display(ex.Message, False, "")
            End Try
        End If

        Me.SettingConsoleMemText.Text = My.Settings.ConsoleMax
        Me.SettingPatMemText.Text = My.Settings.PatMax
        Me.SettingComPortText.Text = My.Settings.ComPort
        Me.OutputMethod.SelectedText = My.Settings.OutputMethod
    End Sub
    Private Sub CloseLink_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles ProjectCloseLink.LinkClicked
        Me.Close()
    End Sub
    Private Sub MinimizeLink_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles ProjectMinimizeLink.LinkClicked, FormMinimizeLink.LinkClicked
        Me.WindowState = FormWindowState.Minimized

    End Sub
    Sub saveProject()

        Me.ProjectBindingSource.EndEdit()
        If Not IsNothing(ProjectBindingSource.Current) Then
            ProjectBindingSource.Current("FilterText") = ProjectBindingSource.Current("Name") & " " & ProjectBindingSource.Current("Notes")
        End If
        ProjectBindingSource.EndEdit()
#If Version = "Full" Then
        Try
            Me.ProjectTableAdapter.Update(Me.JournalSixDataSet1.Project)
        Catch ex As Exception
            PassapMsg.display(ex.Message, False, "")
        End Try
#Else
        'PassapMsg.display("This feature is available in the full version only.", False, "")
#End If

    End Sub
    Private Sub ImagePictureBox_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ImagePictureBox.Click
        'load graphics file
        Dim openFileDialog1 As New OpenFileDialog()

        openFileDialog1.InitialDirectory = "My Documents"
        openFileDialog1.Filter = "*.bmp|*.bmp|*.jpg|*.jpg|*.png|*.png|*.gif|*.gif|All files (*.*)|*.*"
        openFileDialog1.FilterIndex = 2

        If openFileDialog1.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
            ImagePictureBox.ImageLocation = openFileDialog1.FileName
        End If


    End Sub
    Private Sub AddNewProject_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles NewProject.LinkClicked
        Dim newProjectRow As System.Data.DataRowView
        newProjectRow = ProjectBindingSource.AddNew()
        newProjectRow("Id") = Guid.NewGuid
        newProjectRow("Name") = "My Project"
        newProjectRow("Notes") = "My Notes"
        newProjectRow("ImageFile") = ""
        newProjectRow("FilterText") = "My Project My Notes"
        Try
            saveProject()
            ProjectFilterLink.Text = "Projects: All"
            ProjectBindingSource.RemoveFilter()
            ProjectBindingSource.Position = ProjectBindingSource.Find("Id", newProjectRow("Id"))
        Catch ex As Exception
            PassapMsg.display(ex.Message, False, "")
        End Try

    End Sub
    Private Sub EraseProject_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles EraseProject.LinkClicked
        deleteProject()
    End Sub
    Sub deleteForm()
        If Not IsNothing(FormBindingSource.Current()) Then
            If PassapMsg.display("Erase the " & Chr(34) & FormBindingSource.Current("Name") & Chr(34) & " form programme?", False, "") Then
                FormBindingSource.RemoveCurrent()
                saveForm()
            End If
        Else
            PassapMsg.display("No form programmes left to erase.", False, "")
        End If
    End Sub
    Sub deleteProject()
        If Not IsNothing(ProjectBindingSource.Current()) Then
            If PassapMsg.display("Erase the " & Chr(34) & ProjectBindingSource.Current("Name") & Chr(34) & " project?", False, "") Then
                ProjectBindingSource.RemoveCurrent()
                saveProject()
            End If
        Else
            PassapMsg.display("No projects left to erase.", False, "")
        End If
    End Sub

    Sub deleteStitchPattern()

        If Not IsNothing(Me.PatternBindingSource.Current) Then
            If PassapMsg.display("Erase the " & Chr(34) & PatternBindingSource.Current("Name") & Chr(34) & " StitchPattern?", False, "") Then
                PatternBindingSource.RemoveCurrent()
                savePattern()
                StitchPatLibPictureBox.Refresh()
            End If
        Else
            PassapMsg.display("No stitch patterns left to erase.", False, "")
        End If
    End Sub

    Private Sub ProjectName_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ProjectName.Leave
        sender.text = Mid(sender.text, 1, 200)
        ProjectBindingSource.EndEdit()
        If Not IsNothing(ProjectBindingSource.Current) Then
            ProjectBindingSource.Current("FilterText") = ProjectBindingSource.Current("Name") & " " & ProjectBindingSource.Current("Notes")
        End If
        ProjectBindingSource.EndEdit()

    End Sub
    Sub savePattern()
        'Me.Validate()
        Me.PatternBindingSource.EndEdit()
#If Version = "Full" Then
        Try

            Me.PatternTableAdapter.Update(Me.JournalSixDataSet1.Pattern)
        Catch ex As Exception
            PassapMsg.display(ex.Message, False, "")
        End Try
#Else
#End If

    End Sub

    Sub savePiece()
        ' Me.Validate()
        Me.ProjectPieceBindingSource.EndEdit()
#If Version = "Full" Then
        Try

            Me.PieceTableAdapter.Update(Me.JournalSixDataSet1.Piece)
        Catch ex As Exception
            PassapMsg.display(ex.Message, False, "")
        End Try
#Else
#End If

    End Sub
    Sub saveForm()
        'Me.Validate()
        FormBindingSource.EndEdit()
        If Not IsNothing(FormBindingSource.Current) Then
            FormBindingSource.Current("FilterText") = FormBindingSource.Current("Name") & " " & FormBindingSource.Current("Notes")
        End If
        FormBindingSource.EndEdit()
#If Version = "Full" Then
        Try

            Me.FormTableAdapter.Update(Me.JournalSixDataSet1.Form)
        Catch ex As Exception
            PassapMsg.display(ex.Message, False, "")
        End Try
#Else
#End If

    End Sub
    Sub saveTechnique()
        Me.TechniqueBindingSource.EndEdit()
        If Not IsNothing(TechniqueBindingSource.Current) Then
            TechniqueBindingSource.Current("FilterText") = TechniqueBindingSource.Current("Name") & " " & TechniqueBindingSource.Current("Notes")
        End If
        TechniqueBindingSource.EndEdit()

#If Version = "Full" Then

        Try

            Me.TechniqueTableAdapter.Update(Me.JournalSixDataSet1.Technique)
        Catch ex As Exception
            PassapMsg.display(ex.Message, False, "")
        End Try
#Else
#End If

    End Sub
    Sub deleteTechnique()
        If Not IsNothing(Me.TechniqueBindingSource.Current) Then
            If PassapMsg.display("Erase the " & Chr(34) & TechniqueBindingSource.Current("Name") & Chr(34) & " Knit Technique?", False, "") Then
                TechniqueBindingSource.RemoveCurrent()
                saveTechnique()
            End If
        Else
            PassapMsg.display("No stitch patterns left to erase.", False, "")
        End If
    End Sub
    Private Sub ProjectsLink_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles ProjectsLink.LinkClicked
        Me.PageProjects.BringToFront()
    End Sub
    Private Sub PassapPalLink_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles PassapPalLink.LinkClicked, FormWelcomeLink.LinkClicked
        Me.PageWelcome.BringToFront()
    End Sub
    Private Sub WelcomeMinimizeLink_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles WelcomeMinimizeLink.LinkClicked
        MinimizeForm()
    End Sub
    Sub MinimizeForm()
        Me.WindowState = FormWindowState.Minimized
    End Sub
    Sub RestoreForm()
        Me.WindowState = FormWindowState.Normal
    End Sub
    Private Sub WelcomeCloseLink_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles WelcomeCloseLink.LinkClicked
        Me.Close()
    End Sub
    Sub saveAll()
        saveProject()
        savePiece()
        saveForm()
        savePiecePattern()
        saveTechnique()
        savePattern()
    End Sub

    Private Sub ProjectsPage_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PageProjects.Leave
        saveProject()
    End Sub
    Private Sub DetailsLink_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs)

        ProjectPieceBindingSource.MoveFirst()
        PagePieces.BringToFront()
    End Sub
    Private Sub PiecesWelcomeLink_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles PiecesWelcomeLink.LinkClicked
        PageWelcome.BringToFront()
    End Sub
    Private Sub PiecesMinimizeLink_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles PiecesMinimizeLink.LinkClicked
        Me.WindowState = FormWindowState.Minimized
    End Sub
    Private Sub PiecesCloseLink_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles PiecesCloseLink.LinkClicked
        Me.Close()
    End Sub
    Private Sub SummaryLink_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles SummaryLink.LinkClicked
        Me.PageProjects.BringToFront()
    End Sub
    Private Sub NewPieceLink_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles NewPieceLink.LinkClicked
        addNewPiece()

    End Sub
    Sub addNewPiece()
        Dim newPieceRow As System.Data.DataRowView
        newPieceRow = ProjectPieceBindingSource.AddNew()

        newPieceRow("Id") = Guid.NewGuid
        newPieceRow("Name") = "My Fabric Piece"
        newPieceRow("Notes") = "My Notes"

        savePiece()
    End Sub
    Private Sub ErasePieceLink_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles ErasePieceLink.LinkClicked
        If Not IsNothing(ProjectPieceBindingSource.Current()) Then
            If PassapMsg.display("Erase " & Chr(34) & ProjectPieceBindingSource.Current("Name") & Chr(34) & " project?", False, "") Then
                ProjectPieceBindingSource.RemoveCurrent()
                savePiece()
            Else
            End If
        Else
            PassapMsg.display("No fabric piece to erase.", False, "")
        End If
    End Sub
    Private Sub PieceNameText_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PieceNameText.Leave
        sender.text = Mid(sender.text, 1, 50)
        Me.ProjectPieceBindingSource.EndEdit()
    End Sub
    Private Sub Journal_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        If Not DBRestore Then
            saveAll()
        End If
        Try
            Me.E6000Port.Close()
        Catch ex As Exception
            PassapMsg.display(ex.Message, False, "")
        End Try

    End Sub
    Private Sub PiecesPage_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PagePieces.Leave
        savePiece()
    End Sub
    Private Sub SelectFormLink_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs)
        If IsNothing(ProjectPieceBindingSource.Current) Then
            PassapMsg.display("Please add fabric piece before selecting form programme", False, "")
        Else
            ViewFormPage()
        End If

    End Sub

    Private Sub ViewFormPage()
        FormBindingSource.MoveFirst()
        Me.PageForm.BringToFront()
    End Sub
    Private Sub FormWelcomeLink_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs)
        Me.PageWelcome.BringToFront()
    End Sub
    Private Sub FormMinimizeLink_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs)
        MinimizeForm()
    End Sub
    Private Sub FormPage_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs)
        saveForm()
    End Sub
    Private Sub UseFormProgrammeLink_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles UseFormProgrammeLink.LinkClicked

        If Not IsNothing(Me.ProjectPieceBindingSource.Current) Then
            Me.ProjectPieceBindingSource.Current("FormId") = Me.FormBindingSource.Current("Id")
            savePiece()
            PagePieces.BringToFront()
        Else
            PassapMsg.display("Please add or select fabric piece before selecting form programme.", False, "")
        End If
    End Sub
    Private Sub FormCloseLink_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles FormCloseLink.LinkClicked
        Me.Close()
    End Sub

    Private Sub PieceStitchPatternLink_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles PieceStitchPatternLink.LinkClicked
        If IsNothing(ProjectPieceBindingSource.Current) Then
            PassapMsg.display("Please add fabric piece before adding stitch pattern.", False, "")
        Else
            '            CurrentPiecePatternLabel.Text = ProjectPieceBindingSource.Current("Name")
            If PiecePatternBindingSource.Count = 0 Then



                addNewPiecePattern(PiecePatternBindingSource.Count)
            End If
            PiecePatternBindingSource.MoveFirst()
            PagePiecePattern.BringToFront()
        End If
    End Sub
    Sub addNewPiecePattern(ByVal patCount As Integer)
        Dim newPiecePatternRow As System.Data.DataRowView
        newPiecePatternRow = PiecePatternBindingSource.AddNew()
        newPiecePatternRow("Id") = Guid.NewGuid
        If patCount <= 26 And patCount > 0 Then
            newPiecePatternRow("Name") = "ST. PATT " & Mid("ABCDEFGHIJKLMNOPQRSTUVWXYZ", patCount, 1)
        ElseIf patCount = 0 Then
            newPiecePatternRow("Name") = "Cast On "

        Else
            newPiecePatternRow("Name") = "***"
        End If
        Try
            savePiecePattern()
        Catch ex As Exception
            PassapMsg.display(ex.Message, False, "")
        End Try
        PiecePatternList.Refresh()
        checkPatternCapacity()
    End Sub
    Private Sub checkPatternCapacity()
        Dim row As JournalSixDataSet.PiecePatternRow
        If PiecePatternPatternBindingSource.Count > 0 Then
            row = PiecePatternPatternBindingSource.Item(0)
            MsgBox(row.PatternRow.Motif)
        End If

        'MsgBox(row("Motif"))

    End Sub
    Private Sub NewPatternLink_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles NewPatternLink.LinkClicked
        addNewPiecePattern(PiecePatternBindingSource.Count)

    End Sub
    Private Sub PiecePatternPassapLink_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles PiecePatternPassapLink.LinkClicked
        PageWelcome.BringToFront()
    End Sub
    Private Sub PiecePatternLink_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles PiecePatternLink.LinkClicked
        MinimizeForm()
    End Sub
    Private Sub PiecePatternCloseLink_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles PiecePatternCloseLink.LinkClicked
        Me.Close()
    End Sub
    Private Sub PiecePatternPage_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PagePiecePattern.Leave
        savePiecePattern()
    End Sub
    Sub savePiecePattern()
        Me.PiecePatternBindingSource.EndEdit()
#If Version = "Full" Then
        Try

            Me.PiecePatternTableAdapter.Update(Me.JournalSixDataSet1.PiecePattern)
        Catch ex As Exception
            PassapMsg.display(ex.Message, False, "")
        End Try
#Else
#End If
    End Sub

    Private Sub SelectStitchPatternLink_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles SelectStitchPatternLink.LinkClicked
        Me.PagePatternLibrary.BringToFront()
        Application.DoEvents()
    End Sub
    Private Sub loadStitchPattern()

        Me.Cursor = Cursors.WaitCursor

        OpenFileDialog1.FileName = ""
        OpenFileDialog1.Filter = "Image Files|*.bmp;*.jpg;*.png;*.gif;*.cut|.bmp|*.bmp|*.jpg|*.jpg|*.png|*.png|*.gif|*.gif|*.cut|*.cut|All files (*.*)|*.*"
        OpenFileDialog1.FilterIndex = 1
        If OpenFileDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
            If Microsoft.VisualBasic.Right(OpenFileDialog1.FileName.ToLower, 3) = "cut" Then
                im = loadCut(OpenFileDialog1.FileName)
            Else
                im = Image.FromFile(OpenFileDialog1.FileName)
            End If
            Dim w, h As Integer
            w = im.Width
            h = im.Height
            If im.Width > 700 Then
                w = 700
                h = 700 * im.Height / im.Width
                If h > 450 Then
                    w = 450 * im.Width / im.Height
                    h = 450
                End If
            Else
                If h > 450 Then
                    w = 450 * im.Width / im.Height
                    h = 450
                End If
            End If

            Dim c As New Cropper
            c.SketchBox1.SketchImage = New Bitmap(im, w, h)
            c.SketchBox1.initializeSketch("")

            Me.WindowState = FormWindowState.Minimized
            c.TopMost = True
            c.ShowDialog(Me)
            Me.WindowState = FormWindowState.Normal

            im = c.SketchBox1.SketchImage

            Application.DoEvents()
            initializeImagePage()

            generateStitchPattern(0)

        End If
        Me.Cursor = Cursors.Default
    End Sub
    Sub initializeImagePage()
        Me.ColorProcessingComboBox.Text = "Match"
        Me.ForeColor1.Text = "1"
        Me.ForeColor2.Text = "2"
        Me.ForeColor3.Text = "3"
        Me.Forecolor4.Text = "4"
        Me.BackColor1.Text = "1"
        Me.BackColor2.Text = "1"
        Me.BackColor3.Text = "1"
        Me.BackColor4.Text = "1"

        Me.ColorSeparation.Text = "Overlay"
        Me.colorExpansion.Text = "Expand"

        Me.StitchesTextBox.Text = ""
        Me.RowsTextBox.Text = ""
        If My.Settings.OutputMethod = "Printer" Then
            autoReduceColors.Checked = False
        Else
            autoReduceColors.Checked = True
        End If
        Me.Refresh()

    End Sub
    Function loadCut(ByVal fileName As String) As Bitmap
        Dim width, height, runLength As Short
        Dim fs As IO.Stream
        Dim x, y, i, index As Integer
        fs = New System.IO.FileStream(fileName, IO.FileMode.Open, IO.FileAccess.Read)

        Dim r As New IO.BinaryReader(fs)

        width = r.ReadInt16()
        height = r.ReadInt16()
        loadCut = New Bitmap(width, height)
        r.ReadInt16() 'reserved word

        For y = 0 To height - 1
            x = 0
            r.ReadInt16() 'rowlength not used
            runLength = r.ReadByte
            While runLength <> 128 And runLength <> 0
                If runLength > 128 Then
                    index = r.ReadByte
                    For i = 1 To runLength - 128
                        loadCut.SetPixel(x, y, CutPalette(index))
                        x = x + 1
                    Next
                Else
                    For i = 1 To runLength
                        index = r.ReadByte
                        loadCut.SetPixel(x, y, CutPalette(index))
                        x = x + 1
                    Next
                End If
                runLength = r.ReadByte
            End While
        Next

        r.Close()
        fs.Close()

    End Function
    Function loadPal() As Array
        Dim pal(255) As Color
        Dim DrHalo() As Byte
        Dim count, i As Integer
        DrHalo = My.Resources.PAL1
        count = DrHalo(12)
        For i = 0 To count
            pal(i) = Color.FromArgb(DrHalo(i * 6 + 32), DrHalo(i * 6 + 34), DrHalo(i * 6 + 36))
        Next
        loadPal = pal
    End Function

    Sub generateStitchPattern(ByVal colorCount As Integer, Optional ByVal color1 As Integer = Nothing, Optional ByVal color2 As Integer = Nothing, Optional ByVal color3 As Integer = Nothing, Optional ByVal color4 As Integer = Nothing)
        Dim g As Graphics
        Dim i As Byte
        Dim j As UShort
        Dim intermediate As Bitmap
        Dim srcRect, destRect As Rectangle


        Me.Cursor = Cursors.WaitCursor

        If im.Width > 25 Then
            SourcePictureBox.Width = 200
            SourcePictureBox.Height = 200 * im.Height / im.Width
            If SourcePictureBox.Height > 200 Then
                SourcePictureBox.Width = 200 * im.Width / im.Height
                SourcePictureBox.Height = 200
            End If
        Else
            SourcePictureBox.Width = im.Width * 10
            SourcePictureBox.Height = im.Height * 10
            If SourcePictureBox.Height > 200 Then
                SourcePictureBox.Width = 200 * im.Width / im.Height
                SourcePictureBox.Height = 200
            End If
        End If

        SourcePictureBox.SizeMode = PictureBoxSizeMode.Normal

        If StitchesTextBox.Text = "" Or RowsTextBox.Text = "" Then
            Me.StitchesTextBox.Text = Int(im.Width)
            Me.RowsTextBox.Text = Int(im.Height)

            If Me.StitchesTextBox.Text >= 180 Then
                Me.StitchesTextBox.Text = 180
                Me.RowsTextBox.Text = Int(180 * im.Height / im.Width)

            End If
        End If

        bmap = New Bitmap(im, CInt(Me.StitchesTextBox.Text), CInt(Me.RowsTextBox.Text))
        If colorCount = 0 Then
            colorCount = 1
            For i = 0 To bmap.Width - 1
                For j = 0 To bmap.Height - 1

                    Select Case colorCount
                        Case 1
                            Color1Button.BackColor = bmap.GetPixel(i, j)
                            TargetStitchPatternBox.Color1 = Color1Button.BackColor.ToArgb
                            colorCount = colorCount + 1
                            Exit Select
                        Case 2
                            If bmap.GetPixel(i, j) <> Color1Button.BackColor Then
                                Color2Button.BackColor = bmap.GetPixel(i, j)
                                TargetStitchPatternBox.Color2 = Color2Button.BackColor.ToArgb
                                colorCount = colorCount + 1
                            End If
                            Exit Select
                        Case 3
                            If bmap.GetPixel(i, j) <> Color1Button.BackColor And bmap.GetPixel(i, j) <> Color2Button.BackColor Then
                                Color3Button.BackColor = bmap.GetPixel(i, j)
                                TargetStitchPatternBox.Color3 = Color3Button.BackColor.ToArgb
                                colorCount = colorCount + 1
                                Exit Select
                            End If
                        Case 4
                            If bmap.GetPixel(i, j) <> Color1Button.BackColor And bmap.GetPixel(i, j) <> Color2Button.BackColor And bmap.GetPixel(i, j) <> Color3Button.BackColor Then
                                Color4Button.BackColor = bmap.GetPixel(i, j)
                                TargetStitchPatternBox.Color4 = Color4Button.BackColor.ToArgb
                                colorCount = colorCount + 1
                                Exit Select
                            End If
                        Case 5
                            If bmap.GetPixel(i, j) <> Color1Button.BackColor And bmap.GetPixel(i, j) <> Color2Button.BackColor And bmap.GetPixel(i, j) <> Color4Button.BackColor Then
                                
                                colorCount = colorCount + 1
                                Exit Select
                            End If
                            Exit For
                    End Select

                Next j
               
            Next i
        Else
            Color1Button.BackColor = Color.FromArgb(color1)
            TargetStitchPatternBox.Color1 = color1

            Color2Button.BackColor = Color.FromArgb(color2)
            TargetStitchPatternBox.Color2 = color2

            Color3Button.BackColor = Color.FromArgb(color3)
            TargetStitchPatternBox.Color3 = color3

            Color4Button.BackColor = Color.FromArgb(color4)
            TargetStitchPatternBox.Color4 = color4
        End If

        Color3Button.Visible = False
        Color4Button.Visible = False
        ColorCountComboBox.Text = 2

        If colorCount > 3 Then
            ColorCountComboBox.Text = 3
            Color3Button.Visible = True
        End If

        If colorCount > 4 Then
            ColorCountComboBox.Text = 4
            Color4Button.Visible = True

        End If
        If colorCount > 5 Then
            AutoColor(Color1Button.BackColor, Color2Button.BackColor, Color3Button.BackColor, Color4Button.BackColor, ColorCountComboBox.Text, bmap)
        End If

        setColors(PurePalette, ColorCountComboBox.Text)
        initializePalette(PurePalette, PurePalette, False)
        initializePalette(DitheredPalette, PurePalette, True)

        intermediate = New Bitmap(CInt(Me.StitchesTextBox.Text), CInt(Me.RowsTextBox.Text), Imaging.PixelFormat.Format24bppRgb)

        resample(bmap, intermediate, PurePalette, PurePalette, False)

        destRect = New Rectangle(0, 0, SourcePictureBox.Width, SourcePictureBox.Height)
        srcRect = New Rectangle(0, 0, im.Width, im.Height)

        sourceBMap = New Bitmap(SourcePictureBox.Width, SourcePictureBox.Height)

        g = Graphics.FromImage(sourceBMap)
        g.PixelOffsetMode = Drawing2D.PixelOffsetMode.Half
        g.SmoothingMode = Drawing2D.SmoothingMode.None
        g.InterpolationMode = Drawing2D.InterpolationMode.NearestNeighbor
        g.DrawImage(im, destRect, srcRect, GraphicsUnit.Pixel)

        SourcePictureBox.Image = sourceBMap
        'If TargetStitchPatternBox.Motif = "" Then  For transparent overlays

        TargetStitchPatternBox.ColorCount = ColorCountComboBox.Text
        TargetStitchPatternBox.Color1 = Color1Button.BackColor.ToArgb
        TargetStitchPatternBox.Color2 = Color2Button.BackColor.ToArgb
        TargetStitchPatternBox.Color3 = Color3Button.BackColor.ToArgb
        TargetStitchPatternBox.Color4 = Color4Button.BackColor.ToArgb
        TargetStitchPatternBox.Motif = genStitchPatMotif(intermediate)
        TargetStitchPatternBox.Refresh()
        'End If


        Me.Cursor = Cursors.Default
    End Sub
    Sub AutoColor(ByRef c1 As Color, ByRef c2 As Color, ByRef c3 As Color, ByRef c4 As Color, ByVal counter As Short, ByRef m As Bitmap)
        If Not IsNothing(m) Then

            Dim r(3) As Integer
            Dim g(3) As Integer
            Dim b(3) As Integer
            Dim colors(3) As Color
            Dim c As Color
            Dim Divisor(3) As Integer
            Dim i, j, k As Integer
            Dim index As Integer
            Dim nearestCol, dist As Integer
            Dim tempColor As Color

            For i = 0 To 3
                Divisor(i) = 1
                c = m.GetPixel(Int(Rnd() * m.Width), Int(Rnd() * m.Height))
                r(i) = c.R : g(i) = c.G : b(i) = c.B
            Next

            k = Math.Min(m.Height * m.Width * 5, 40000)

            For i = 0 To k
                c = m.GetPixel(Int(Rnd() * m.Width), Int(Rnd() * m.Height))
                nearestCol = ColorDistanceTable(c.R, Int(r(0) / Divisor(0))) + ColorDistanceTable(c.G, Int(g(0) / Divisor(0))) + ColorDistanceTable(c.B, Int(b(0) / Divisor(0)))
                index = 0
                For j = 1 To counter - 1
                    dist = ColorDistanceTable(c.R, Int(r(j) / Divisor(j))) + ColorDistanceTable(c.G, Int(g(j) / Divisor(j))) + ColorDistanceTable(c.B, Int(b(j) / Divisor(j)))
                    If dist < nearestCol Then
                        index = j
                        nearestCol = dist
                    End If
                Next j
                r(index) = r(index) + c.R : g(index) = g(index) + c.G : b(index) = b(index) + c.B
                Divisor(index) = Divisor(index) + 1

            Next

            c1 = Color.FromArgb(255, Int(r(0) / Divisor(0)), Int(g(0) / Divisor(0)), Int(b(0) / Divisor(0)))
            c2 = Color.FromArgb(255, Int(r(1) / Divisor(1)), Int(g(1) / Divisor(1)), Int(b(1) / Divisor(1)))
            c3 = Color.FromArgb(255, Int(r(2) / Divisor(2)), Int(g(2) / Divisor(2)), Int(b(2) / Divisor(2)))
            c4 = Color.FromArgb(255, Int(r(3) / Divisor(3)), Int(g(3) / Divisor(3)), Int(b(3) / Divisor(3)))

            colors(0) = c1 : colors(1) = c2 : colors(2) = c3 : colors(3) = c4

            If counter = 2 Then
                If c2.GetBrightness > c1.GetBrightness Then
                    tempColor = c2
                    c2 = c1
                    c1 = tempColor
                End If
            End If
            If counter > 2 Then
                For i = 1 To counter - 1
                    tempColor = colors(i)
                    For j = i To 1 Step -1
                        If tempColor.GetBrightness > colors(j - 1).GetBrightness Then colors(j) = colors(j - 1) Else Exit For
                    Next j
                    colors(j) = tempColor
                Next i

            End If

            c1 = colors(0) : c2 = colors(1) : c3 = colors(2) : c4 = colors(3)
        End If
    End Sub
    Function genStitchPatMotif(ByRef intermediate As Bitmap, Optional ByVal twoColor As Boolean = False) As String
        Dim x, y, i, j, k As Short
        Dim aColor
        Dim foreColor, backColor As String
        Dim newWidth, newHeight As Short
        Dim heightMultiplier As Byte
        Dim widthMultiplier As Byte
        Dim expansionX, expansionY, colorCount As Byte

        Dim ForeExpansion() As Array
        Dim BackExpansion() As Array

        ReDim ForeExpansion(ColorCountComboBox.Text - 1)
        ReDim BackExpansion(ColorCountComboBox.Text - 1)
        Me.ForeColor1.Text = Me.ForeColor1.Text.Trim()
        Me.ForeColor2.Text = Me.ForeColor2.Text.Trim()
        Me.ForeColor3.Text = Me.ForeColor3.Text.Trim()
        Me.Forecolor4.Text = Me.Forecolor4.Text.Trim()
        Me.BackColor1.Text = Me.BackColor1.Text.Trim()
        Me.BackColor2.Text = Me.BackColor2.Text.Trim()
        Me.BackColor3.Text = Me.BackColor3.Text.Trim()
        Me.BackColor4.Text = Me.BackColor4.Text.Trim()

        genStitchPatMotif = ""

        Try
            If twoColor Then
                colorCount = 2
            Else
                colorCount = Me.ColorCountComboBox.Text
            End If

            Select Case Me.ColorCountComboBox.Text

                Case "2"
                    ForeExpansion(0) = Split(Me.ForeColor1.Text, Chr(13) & Chr(10))
                    ForeExpansion(1) = Split(Me.ForeColor2.Text, Chr(13) & Chr(10))

                    BackExpansion(0) = Split(Me.BackColor1.Text, Chr(13) & Chr(10))
                    BackExpansion(1) = Split(Me.BackColor2.Text, Chr(13) & Chr(10))
                Case "3"
                    ForeExpansion(0) = Split(Me.ForeColor1.Text, Chr(13) & Chr(10))
                    ForeExpansion(1) = Split(Me.ForeColor2.Text, Chr(13) & Chr(10))
                    ForeExpansion(2) = Split(Me.ForeColor3.Text, Chr(13) & Chr(10))

                    BackExpansion(0) = Split(Me.BackColor1.Text, Chr(13) & Chr(10))
                    BackExpansion(1) = Split(Me.BackColor2.Text, Chr(13) & Chr(10))
                    BackExpansion(2) = Split(Me.BackColor3.Text, Chr(13) & Chr(10))
                Case "4"
                    ForeExpansion(0) = Split(Me.ForeColor1.Text, Chr(13) & Chr(10))
                    ForeExpansion(1) = Split(Me.ForeColor2.Text, Chr(13) & Chr(10))
                    ForeExpansion(2) = Split(Me.ForeColor3.Text, Chr(13) & Chr(10))
                    ForeExpansion(3) = Split(Me.Forecolor4.Text, Chr(13) & Chr(10))

                    BackExpansion(0) = Split(Me.BackColor1.Text, Chr(13) & Chr(10))
                    BackExpansion(1) = Split(Me.BackColor2.Text, Chr(13) & Chr(10))
                    BackExpansion(2) = Split(Me.BackColor3.Text, Chr(13) & Chr(10))
                    BackExpansion(3) = Split(Me.BackColor4.Text, Chr(13) & Chr(10))
            End Select

            If ColorSeparation.Text = "Separate" Then
                If colorExpansion.Text = "Expand" Then
                    For k = ForeExpansion.GetLowerBound(0) To ForeExpansion.GetUpperBound(0)
                        heightMultiplier = heightMultiplier + ForeExpansion(k).GetLength(0)
                    Next
                    widthMultiplier = Len(ForeExpansion(0)(0))
                    newWidth = intermediate.Width * widthMultiplier
                    newHeight = intermediate.Height * heightMultiplier
                    If newHeight < 1000 Then
                        y = 0
                        While y < newHeight
                            For k = colorCount - 1 To 0 Step -1
                                expansionY = ForeExpansion(k).GetLength(0)
                                For j = 0 To expansionY - 1
                                    For x = 0 To intermediate.Width - 1
                                        aColor = intermediate.GetPixel(x, Int(y / heightMultiplier))
                                        expansionX = Len(ForeExpansion(k)(j))
                                        For i = 0 To expansionX - 1
                                            If aColor = PurePalette(k).color Then
                                                foreColor = Trim(Str(Val(Mid(ForeExpansion(k)(j), i + 1, 1) - 1)))
                                                genStitchPatMotif = genStitchPatMotif & foreColor
                                            Else
                                                backColor = Trim(Str(Val(Mid(BackExpansion(k)(j), i + 1, 1) - 1)))
                                                genStitchPatMotif = genStitchPatMotif & backColor
                                            End If
                                        Next i

                                    Next x
                                Next j
                                y = y + expansionY
                            Next k
                        End While
                    Else
                        PassapMsg.display("Unable to process stitch pattern greater than 999 rows.", False, "")

                    End If
                Else
                    'Separated, filtered

                    heightMultiplier = colorCount
                    newWidth = intermediate.Width
                    newHeight = intermediate.Height * heightMultiplier
                    If newHeight < 1000 Then
                        y = 0
                        While y < newHeight
                            For k = colorCount - 1 To 0 Step -1
                                For x = 0 To intermediate.Width - 1
                                    aColor = intermediate.GetPixel(x, Int(y / heightMultiplier))
                                    If aColor = PurePalette(k).color Then
                                        expansionY = Int(y / heightMultiplier) Mod ForeExpansion(k).GetLength(0)
                                        expansionX = x Mod Len(ForeExpansion(k)(expansionY))

                                        foreColor = Trim(Str(Val(Mid(ForeExpansion(k)(expansionY), expansionX + 1, 1) - 1)))
                                        genStitchPatMotif = genStitchPatMotif & foreColor
                                    Else
                                        expansionY = Int(y / heightMultiplier) Mod BackExpansion(k).GetLength(0)
                                        expansionX = x Mod Len(BackExpansion(k)(expansionY))

                                        backColor = Trim(Str(Val(Mid(BackExpansion(k)(expansionY), expansionX + 1, 1) - 1)))
                                        genStitchPatMotif = genStitchPatMotif & backColor
                                    End If
                                Next x
                                y = y + 1
                            Next k
                        End While
                    Else
                        PassapMsg.display("Unable to process stitch pattern greater than 999 rows.", False, "")
                    End If
                End If
            Else
                If colorExpansion.Text = "Expand" Then 'Overlay, expand
                    heightMultiplier = heightMultiplier + ForeExpansion(0).GetLength(0)
                    widthMultiplier = Len(ForeExpansion(0)(0))
                    newWidth = intermediate.Width * widthMultiplier
                    newHeight = intermediate.Height * heightMultiplier
                    If newHeight < 1000 Then
                        y = 0
                        While y < newHeight

                            expansionY = ForeExpansion(0).GetLength(0)
                            For j = 0 To expansionY - 1
                                For x = 0 To intermediate.Width - 1
                                    For k = Me.ColorCountComboBox.Text - 1 To 0 Step -1
                                        aColor = intermediate.GetPixel(x, Int(y / heightMultiplier))
                                        If aColor = PurePalette(k).color Then

                                            expansionX = Len(ForeExpansion(k)(j))
                                            For i = 0 To expansionX - 1
                                                foreColor = Trim(Str(Val(Mid(ForeExpansion(k)(j), i + 1, 1) - 1)))
                                                genStitchPatMotif = genStitchPatMotif & foreColor
                                            Next i

                                        End If
                                    Next k
                                Next x

                            Next j
                            y = y + expansionY
                        End While
                    Else
                        PassapMsg.display("Unable to process stitch pattern greater than 999 rows.", False, "")

                    End If
                Else
                    'overlay, filter
                    newWidth = intermediate.Width
                    newHeight = intermediate.Height
                    If newHeight < 1000 Then
                        y = 0
                        While y < newHeight
                            For x = 0 To intermediate.Width - 1
                                For k = Me.ColorCountComboBox.Text - 1 To 0 Step -1
                                    aColor = intermediate.GetPixel(x, y)
                                    If aColor = PurePalette(k).color Then
                                        expansionY = y Mod ForeExpansion(k).GetLength(0)
                                        expansionX = x Mod Len(ForeExpansion(k)(expansionY))
                                        foreColor = Trim(Str(Val(Mid(ForeExpansion(k)(expansionY), expansionX + 1, 1) - 1)))
                                        genStitchPatMotif = genStitchPatMotif & foreColor
                                    End If
                                Next k
                            Next x
                            y = y + 1
                        End While
                    Else
                        PassapMsg.display("Unable to process stitch pattern greater than 999 rows.", False, "")
                    End If
                End If
            End If
            If Me.autoReduceColors.Checked = True Then
                If genStitchPatMotif.Contains("3") Then
                    colorCount = 4
                ElseIf genStitchPatMotif.Contains("2") Then
                    colorCount = 3
                Else
                    colorCount = 2
                End If
            End If
            TargetStitchPatternBox.ColorCount = colorCount
                genStitchPatMotif = IIf(colorCount = 2, "0", Trim(Str(colorCount))) & " " & Space(3 - Len(Trim(Str(newWidth)))) & Trim(Str(newWidth)) & " " & Space(3 - Len(Trim(Str(newHeight)))) & Trim(Str(newHeight)) & " " & genStitchPatMotif

        Catch ex As Exception
            PassapMsg.display(ex.Message, False, "")
            PassapMsg.display("Could not correctly generate stitch pattern with settings provided.", False, "")
            genStitchPatMotif = ""
        End Try
    End Function

    Sub resample(ByVal sourceBmp As Bitmap, ByRef targetBmp As Bitmap, ByRef PurePalette() As DPal, ByRef DitheredPalette() As DPal, ByVal difussion As Boolean)
        'Match, Hatch or Stipple
        Dim i, j, paletteEntry As Integer
        Dim QuantErrorRed As Integer
        Dim QuantErrorGreen As Integer
        Dim QuantErrorBlue As Integer
        Dim sourceColor As Color


        For j = 0 To targetBmp.Height - 1
            For i = 0 To targetBmp.Width - 1
                sourceColor = sourceBmp.GetPixel(i, j)
                paletteEntry = nearestColor(sourceColor, DitheredPalette, QuantErrorRed, QuantErrorGreen, QuantErrorBlue)
                targetBmp.SetPixel(i, j, PurePalette(DitheredPalette(paletteEntry).ditherMatrix(i Mod 2, j Mod 2)).color)

                If difussion Then
                    If i < targetBmp.Width - 1 Then
                        sourceColor = sourceBmp.GetPixel(i + 1, j)
                        sourceBmp.SetPixel(i + 1, j, Color.FromArgb(Math.Min(Math.Max(sourceColor.R + QuantErrorRed * 7 / 16, 0), 255), _
                        Math.Min(Math.Max(sourceColor.G + QuantErrorGreen * 7 / 16, 0), 255), _
                        Math.Min(Math.Max(sourceColor.B + QuantErrorBlue * 7 / 16, 0), 255)))
                        If j < targetBmp.Height - 1 Then
                            sourceColor = sourceBmp.GetPixel(i + 1, j + 1)
                            sourceBmp.SetPixel(i + 1, j + 1, Color.FromArgb(Math.Min(Math.Max(sourceColor.R + QuantErrorRed * 1 / 16, 0), 255), _
                            Math.Min(Math.Max(sourceColor.G + QuantErrorGreen * 1 / 16, 0), 255), _
                            Math.Min(Math.Max(sourceColor.B + QuantErrorBlue * 1 / 16, 0), 255)))
                        End If
                    End If
                    If j < targetBmp.Height - 1 Then
                        sourceColor = sourceBmp.GetPixel(i, j + 1)
                        sourceBmp.SetPixel(i, j + 1, Color.FromArgb(Math.Min(Math.Max(sourceColor.R + QuantErrorRed * 5 / 16, 0), 255), _
                        Math.Min(Math.Max(sourceColor.G + QuantErrorGreen * 5 / 16, 0), 255), _
                        Math.Min(Math.Max(sourceColor.B + QuantErrorBlue * 5 / 16, 0), 255)))
                        If i > 0 Then
                            sourceColor = sourceBmp.GetPixel(i - 1, j + 1)
                            sourceBmp.SetPixel(i - 1, j + 1, Color.FromArgb(Math.Min(Math.Max(sourceColor.R + QuantErrorRed * 3 / 16, 0), 255), _
                            Math.Min(Math.Max(sourceColor.G + QuantErrorGreen * 3 / 16, 0), 255), _
                            Math.Min(Math.Max(sourceColor.B + QuantErrorBlue * 3 / 16, 0), 255)))
                        End If
                    End If
                End If
            Next i
        Next j

    End Sub
    Sub resample(ByVal sourceBmp As Bitmap, ByRef targetBmp As Bitmap, ByRef PurePalette() As DPal, ByVal outlineMode As Boolean)
        'For Sketche or outline mode
        Dim i, j, paletteEntry As Integer
        Dim leftPixel, rightPixel, upPixel As Integer
        Dim QuantErrorRed As Integer
        Dim QuantErrorGreen As Integer
        Dim QuantErrorBlue As Integer
        Dim sourceColor As Color

        upPixel = 0

        For j = 0 To targetBmp.Height - 1
            leftPixel = 0
            For i = 0 To targetBmp.Width - 1
                If i <> targetBmp.Width - 1 Then
                    rightPixel = nearestColor(sourceBmp.GetPixel(i + 1, j), PurePalette, QuantErrorRed, QuantErrorGreen, QuantErrorBlue)
                Else
                    rightPixel = 0
                End If

                sourceColor = sourceBmp.GetPixel(i, j)
                paletteEntry = nearestColor(sourceColor, PurePalette, QuantErrorRed, QuantErrorGreen, QuantErrorBlue)
                If paletteEntry <> leftPixel Or paletteEntry <> upPixel Or paletteEntry <> rightPixel Then
                    If outlineMode Then
                        targetBmp.SetPixel(i, j, PurePalette(1).color)
                    Else
                        If paletteEntry <> 0 Then
                            targetBmp.SetPixel(i, j, PurePalette(paletteEntry).color)
                        Else
                            targetBmp.SetPixel(i, j, PurePalette(upPixel).color)
                        End If
                    End If
                Else
                    targetBmp.SetPixel(i, j, PurePalette(0).color)
                End If
                leftPixel = paletteEntry
                If j > 0 Then
                    upPixel = nearestColor(sourceBmp.GetPixel(i, j - 1), PurePalette, QuantErrorRed, QuantErrorGreen, QuantErrorBlue)
                End If
            Next i
            upPixel = nearestColor(sourceBmp.GetPixel(0, j), PurePalette, QuantErrorRed, QuantErrorGreen, QuantErrorBlue)

        Next j

    End Sub
    Function nearestColor(ByVal aColor As Color, ByRef aPalette() As DPal, ByRef quantErrorRed As Integer, ByRef quantErrorGreen As Integer, ByRef quantErrorBlue As Integer) As Integer
        Dim i As Integer
        Dim NearestCol As Integer
        Dim Dist As Integer
        Dim index As Integer
        Dim colorCount As Integer

        colorCount = aPalette.GetUpperBound(0)
        index = 0

        NearestCol = ColorDistanceTable(aColor.R, aPalette(0).color.R) + _
            ColorDistanceTable(aColor.G, aPalette(0).color.G) + _
            ColorDistanceTable(aColor.B, aPalette(0).color.B)
        For i = 1 To colorCount
            Dist = ColorDistanceTable(aColor.R, aPalette(i).color.R) + _
                ColorDistanceTable(aColor.G, aPalette(i).color.G) + _
                ColorDistanceTable(aColor.B, aPalette(i).color.B)

            If Dist < NearestCol Then
                index = i
                NearestCol = Dist

            End If
        Next
        nearestColor = index
        quantErrorRed = CInt(aColor.R) - CInt(aPalette(nearestColor).color.R)
        quantErrorGreen = CInt(aColor.G) - CInt(aPalette(nearestColor).color.G)
        quantErrorBlue = CInt(aColor.B) - CInt(aPalette(nearestColor).color.B)
    End Function
    Sub setColors(ByRef colors() As DPal, ByVal colorCount As Integer)
        ReDim colors(1)
        colors(0).color = Me.Color1Button.BackColor

        colors(1).color = Me.Color2Button.BackColor
        If colorCount > 2 Then
            ReDim Preserve colors(2)
            colors(2).color = Me.Color3Button.BackColor
        End If

        If colorCount > 3 Then
            ReDim Preserve colors(3)
            colors(3).color = Me.Color4Button.BackColor
        End If

    End Sub
    Sub initializePalette(ByRef aPalette() As DPal, ByRef colors() As DPal, ByVal dithered As Boolean)
        Dim index, i, j, colorCount As Byte
        colorCount = colors.GetUpperBound(0)
        ReDim Preserve aPalette((colorCount + 1) ^ 2 * 4 - 1)
        index = 0
        For i = 0 To colorCount
            For j = 0 To colorCount
                If i = j Then
                    aPalette(index).color = colors(i).color
                    ReDim aPalette(index).ditherMatrix(1, 1)
                    aPalette(index).ditherMatrix(0, 0) = i
                    aPalette(index).ditherMatrix(1, 0) = i
                    aPalette(index).ditherMatrix(0, 1) = i
                    aPalette(index).ditherMatrix(1, 1) = i
                    index += 1
                End If
                If i < j And dithered Then
                    aPalette(index).color = Color.FromArgb(Int((colors(i).color.R * 3 + colors(j).color.R) / 4), _
                                            Int((colors(i).color.G * 3 + colors(j).color.G) / 4), _
                                            Int((colors(i).color.B * 3 + colors(j).color.B) / 4))
                    ReDim aPalette(index).ditherMatrix(1, 1)
                    aPalette(index).ditherMatrix(0, 0) = j
                    aPalette(index).ditherMatrix(1, 0) = i
                    aPalette(index).ditherMatrix(0, 1) = i
                    aPalette(index).ditherMatrix(1, 1) = i
                    index += 1

                    aPalette(index).color = Color.FromArgb(Int((colors(i).color.R * 2 + colors(j).color.R * 2) / 4), _
                                            Int((colors(i).color.G * 2 + colors(j).color.G * 2) / 4), _
                                            Int((colors(i).color.B * 2 + colors(j).color.B * 2) / 4))
                    ReDim aPalette(index).ditherMatrix(1, 1)
                    aPalette(index).ditherMatrix(0, 0) = i
                    aPalette(index).ditherMatrix(1, 0) = j
                    aPalette(index).ditherMatrix(0, 1) = j
                    aPalette(index).ditherMatrix(1, 1) = i
                    index += 1

                    aPalette(index).color = Color.FromArgb(Int((colors(i).color.R + colors(j).color.R * 3) / 4), _
                                            Int((colors(i).color.G + colors(j).color.G * 3) / 4), _
                                            Int((colors(i).color.B + colors(j).color.B * 3) / 4))
                    ReDim aPalette(index).ditherMatrix(1, 1)
                    aPalette(index).ditherMatrix(0, 0) = i
                    aPalette(index).ditherMatrix(1, 0) = j
                    aPalette(index).ditherMatrix(0, 1) = j
                    aPalette(index).ditherMatrix(1, 1) = j
                    index += 1
                End If
            Next j
        Next i
        ReDim Preserve aPalette(index - 1)
    End Sub
    Private Sub Color1Button_DoubleClick()
        Me.ColorDialog1.ShowDialog()
        Color1Button.BackColor = Color.FromArgb(255, ColorDialog1.Color)
    End Sub
    Private Sub Color2Button_DoubleClick()
        Me.ColorDialog1.ShowDialog()
        Color2Button.BackColor = Color.FromArgb(255, ColorDialog1.Color)
    End Sub
    Private Sub Color3Button_DoubleClick()
        Me.ColorDialog1.ShowDialog()
        Color3Button.BackColor = Color.FromArgb(255, ColorDialog1.Color)
    End Sub
    Private Sub Color4Button_DoubleClick()
        Me.ColorDialog1.ShowDialog()
        Color4Button.BackColor = Color.FromArgb(255, ColorDialog1.Color)
    End Sub
    Private Sub UpdateStitchPatternButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UpdateStitchPatternButton.Click
        Dim x, y As Short
        Dim i As Integer
        Dim bmap, intermediate As Bitmap
        Dim im2 As Image
        Dim motif, oldmotif As String

        motif = ""

        TargetStitchPatternBox.Color1 = Color1Button.BackColor.ToArgb
        TargetStitchPatternBox.Color2 = Color2Button.BackColor.ToArgb
        TargetStitchPatternBox.Color3 = Color3Button.BackColor.ToArgb
        TargetStitchPatternBox.Color4 = Color4Button.BackColor.ToArgb
        If Me.ColorProcessingComboBox.Text = "Outline" Then
            TargetStitchPatternBox.ColorCount = "2"
            TargetStitchPatternBox.Color1 = Color.White.ToArgb
            TargetStitchPatternBox.Color2 = Color.Black.ToArgb
        Else
            TargetStitchPatternBox.ColorCount = Me.ColorCountComboBox.Text
        End If

        If IsNothing(im) Then
            If Val(Me.StitchesTextBox.Text) > 0 And Val(Me.RowsTextBox.Text) > 0 Then
                bmap = New Bitmap(CInt(Me.StitchesTextBox.Text), CInt(Me.RowsTextBox.Text))
                For y = 0 To bmap.Height - 1
                    For x = 0 To bmap.Width - 1
                        bmap.SetPixel(x, y, Me.Color1Button.BackColor)
                        result = bmap
                    Next
                Next
            End If
        Else
            im2 = New Bitmap(im)
            If RightCheckBox.Checked Then
                im2.RotateFlip(RotateFlipType.Rotate90FlipNone)

            End If
            If LeftCheckBox.Checked Then
                im2.RotateFlip(RotateFlipType.Rotate270FlipNone)
            End If
            If FlipCheckBox.Checked Then
                im2.RotateFlip(RotateFlipType.RotateNoneFlipY)
            End If
            If MirrorCheckBox.Checked Then
                im2.RotateFlip(RotateFlipType.RotateNoneFlipX)
            End If


            If Val(Me.StitchesTextBox.Text) > 0 And Val(Me.RowsTextBox.Text) > 0 Then
                bmap = New Bitmap(im2, CInt(Me.StitchesTextBox.Text), CInt(Me.RowsTextBox.Text))
                intermediate = New Bitmap(CInt(Me.StitchesTextBox.Text), CInt(Me.RowsTextBox.Text))
                Me.Cursor = Cursors.WaitCursor
                setColors(PurePalette, ColorCountComboBox.Text)
                initializePalette(PurePalette, PurePalette, False)
                initializePalette(DitheredPalette, PurePalette, True)
                Select Me.ColorProcessingComboBox.Text
                    Case "Match"

                        ' If Me.ColorProcessingComboBox.Text = "Match" Then
                        resample(bmap, intermediate, PurePalette, PurePalette, False)
                        'TargetStitchPatternBox.Motif = genStitchPatMotif(intermediate)
                        Try
                            motif = genStitchPatMotif(intermediate)
                        Catch ex As Exception
                            PassapMsg.display(ex.Message, False, "")
                        End Try
                        'ElseIf Me.ColorProcessingComboBox.Text = "Hatch" Then
                    Case "Hatch"
                        resample(bmap, intermediate, PurePalette, DitheredPalette, False)
                        'TargetStitchPatternBox.Motif = genStitchPatMotif(intermediate)
                        Try
                            motif = genStitchPatMotif(intermediate)
                        Catch ex As Exception
                            PassapMsg.display(ex.Message, False, "")
                        End Try
                    Case "Stipple"
                        resample(bmap, intermediate, PurePalette, PurePalette, True) 'Stipple
                        'TargetStitchPatternBox.Motif = genStitchPatMotif(intermediate)
                        Try
                            motif = genStitchPatMotif(intermediate)
                        Catch ex As Exception
                            PassapMsg.display(ex.Message, False, "")
                        End Try
                    Case "Sketch"
                        resample(bmap, intermediate, PurePalette, False)
                        Try
                            motif = genStitchPatMotif(intermediate)
                        Catch ex As Exception
                            PassapMsg.display(ex.Message, False, "")
                        End Try
                    Case "Outline"
                        resample(bmap, intermediate, PurePalette, True)
                        Try
                            motif = genStitchPatMotif(intermediate, True)
                        Catch ex As Exception
                            PassapMsg.display(ex.Message, False, "")
                        End Try

                End Select
                If Len(TargetStitchPatternBox.Motif) = Len(motif) And TargetStitchPatternBox.ColorCount = Me.ColorCountComboBox.Text Then
                    ' TargetStitchPatternBox.Motif = Mid(motif, 1, 10) & Mid(TargetStitchPatternBox.Motif, 11)
                    oldmotif = TargetStitchPatternBox.Motif
                    TargetStitchPatternBox.Motif = Mid(motif, 1, 10)
                    For i = 11 To Len(motif)
                        If Mid(motif, i, 1) = "4" Then
                            TargetStitchPatternBox.Motif = TargetStitchPatternBox.Motif & Mid(oldmotif, i, 1)
                        Else
                            TargetStitchPatternBox.Motif = TargetStitchPatternBox.Motif & Mid(motif, i, 1)
                        End If
                    Next
                Else
                    TargetStitchPatternBox.Motif = motif
                End If
                Me.Cursor = Cursors.Default
            End If
        End If

        


        '  PageImage.BringToFront()
        TargetStitchPatternBox.Refresh()


    End Sub


    Private Sub ColorCountComboBox_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ColorCountComboBox.SelectedIndexChanged
        Select Case Me.ColorCountComboBox.Text
            Case "2"
                Me.Color1Button.Visible = True
                Me.Color2Button.Visible = True
                Me.Color3Button.Visible = False
                Me.Color4Button.Visible = False

                Me.ForeColor1.Visible = True
                Me.ForeColor2.Visible = True
                Me.ForeColor3.Visible = False
                Me.Forecolor4.Visible = False

                Me.BackColor1.Visible = True
                Me.BackColor2.Visible = True
                Me.BackColor3.Visible = False
                Me.BackColor4.Visible = False

            Case "3"
                Me.Color1Button.Visible = True
                Me.Color2Button.Visible = True
                Me.Color3Button.Visible = True
                Me.Color4Button.Visible = False

                Me.ForeColor1.Visible = True
                Me.ForeColor2.Visible = True
                Me.ForeColor3.Visible = True
                Me.Forecolor4.Visible = False

                Me.BackColor1.Visible = True
                Me.BackColor2.Visible = True
                Me.BackColor3.Visible = True
                Me.BackColor4.Visible = False
            Case "4"
                Me.Color1Button.Visible = True
                Me.Color2Button.Visible = True
                Me.Color3Button.Visible = True
                Me.Color4Button.Visible = True

                Me.ForeColor1.Visible = True
                Me.ForeColor2.Visible = True
                Me.ForeColor3.Visible = True
                Me.Forecolor4.Visible = True

                Me.BackColor1.Visible = True
                Me.BackColor2.Visible = True
                Me.BackColor3.Visible = True
                Me.BackColor4.Visible = True
        End Select
    End Sub
    Private Sub NewFormProgrammeLink_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles NewFormProgrammeLink.LinkClicked
        Dim newFormRow As DataRowView

        FormBindingSource.RemoveFilter()
        newFormRow = FormBindingSource.AddNew()
        newFormRow("Id") = Guid.NewGuid
        newFormRow("Name") = "My Form Programme"
        newFormRow("Notes") = "My Notes"
        newFormRow("FilterText") = "My Form Programme My Notes"
        Application.DoEvents()
        Try
            Me.Validate()
            Me.FormBindingSource.EndEdit()
#If Version = "Full" Then
            Me.FormTableAdapter.Update(Me.JournalSixDataSet1.Form)
#Else
#End If
            FormBindingSource.Position = FormBindingSource.Find("Id", newFormRow("Id"))
        Catch ex As Exception
            PassapMsg.display(ex.Message, False, "")
        End Try

    End Sub
    Private Sub EraseFormProgrammeLink_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles EraseFormProgrammeLink.LinkClicked
        deleteForm()
    End Sub
    Private Sub Color1Button_Clicked(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Color1Button.Click
        If sender.FlatAppearance.BorderSize = 1 Then
            Color1Button.FlatAppearance.BorderSize = 3
            Color2Button.FlatAppearance.BorderSize = 1
            Color3Button.FlatAppearance.BorderSize = 1
            Color4Button.FlatAppearance.BorderSize = 1
        Else
            Color1Button_DoubleClick()
            Color1Button.FlatAppearance.BorderSize = 1
        End If
    End Sub
    Private Sub Color2Button_Clicked(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Color2Button.Click
        If sender.FlatAppearance.BorderSize = 1 Then
            Color2Button.FlatAppearance.BorderSize = 3
            Color1Button.FlatAppearance.BorderSize = 1
            Color3Button.FlatAppearance.BorderSize = 1
            Color4Button.FlatAppearance.BorderSize = 1
        Else
            Color2Button_DoubleClick()
            Color2Button.FlatAppearance.BorderSize = 1
        End If
    End Sub
    Private Sub Color3Button_Clicked(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Color3Button.Click
        If sender.FlatAppearance.BorderSize = 1 Then
            Color3Button.FlatAppearance.BorderSize = 3
            Color1Button.FlatAppearance.BorderSize = 1
            Color2Button.FlatAppearance.BorderSize = 1
            Color4Button.FlatAppearance.BorderSize = 1
        Else
            Color3Button_DoubleClick()
            Color3Button.FlatAppearance.BorderSize = 1
        End If
    End Sub
    Private Sub Color4Button_Clicked(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Color4Button.Click
        If sender.FlatAppearance.BorderSize = 1 Then
            Color4Button.FlatAppearance.BorderSize = 3
            Color1Button.FlatAppearance.BorderSize = 1
            Color2Button.FlatAppearance.BorderSize = 1
            Color3Button.FlatAppearance.BorderSize = 1
        Else
            Color4Button_DoubleClick()
            Color4Button.FlatAppearance.BorderSize = 1
        End If
    End Sub

    Private Sub SourcePictureBox_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SourcePictureBox.Click
        Dim p As Point
        p = SourcePictureBox.PointToClient(sender.mouseposition)
        If Not IsNothing(sourceBMap) Then
            DragColor = sourceBMap.GetPixel(p.X, p.Y)
            If Color1Button.FlatAppearance.BorderSize = 3 Then
                Color1Button.BackColor = Color.FromArgb(255, DragColor.R, DragColor.G, DragColor.B)

            ElseIf Color2Button.FlatAppearance.BorderSize = 3 Then
                Color2Button.BackColor = Color.FromArgb(255, DragColor.R, DragColor.G, DragColor.B)

            ElseIf Color3Button.FlatAppearance.BorderSize = 3 Then
                Color3Button.BackColor = Color.FromArgb(255, DragColor.R, DragColor.G, DragColor.B)

            ElseIf Color4Button.FlatAppearance.BorderSize = 3 Then
                Color4Button.BackColor = Color.FromArgb(255, DragColor.R, DragColor.G, DragColor.B)

            End If
        End If
    End Sub
    Private Function MotifSize(ByVal StitchPat As String) As Integer
        Dim motif As String
        Dim colorCount As Integer
        Dim colorMultiplier As Integer
        motif = Mid(StitchPat, 11)
        colorCount = Val(Mid(StitchPat, 1, 1))
        If colorCount > 2 Then
            colorMultiplier = 2
        Else
            colorMultiplier = 1
            colorCount = 0
        End If
        MotifSize = Len(motif) * colorMultiplier
    End Function

    Private Sub AddStitchPatLibraryLink_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles AddStitchPatLibraryLink.LinkClicked
        Dim newPatternRow As System.Data.DataRowView

        Dim motifHeader As String
        Dim motif As String
        Dim colorMultiplier As Integer
        Dim cuts As Integer 'If pattern is too large, how may pieces will it need to be cut into to fit memory limitations?
        Dim cutSize As Integer 'Cuts must be between rows.  How many rows in each cut?
        Dim cubbySize As Integer 'How much memory is available per pattern slot ("cubby hole")
        Dim stitchCount As Integer 'How many stitches in a row.
        Dim curRowCount As Integer
        Dim i As Integer
        Dim colorCount As Integer
        Dim DbColorCount As Integer
        Dim cutSizeRows As Integer
        Dim perciseCuts As Double
        Dim Remainder As Integer
        Dim totalSize As Integer

        Me.Cursor = Cursors.WaitCursor

        motif = Mid(TargetStitchPatternBox.Motif, 11)

        colorCount = Val(Mid(TargetStitchPatternBox.Motif, 1, 1))
        DbColorCount = TargetStitchPatternBox.ColorCount

        If colorCount > 2 Then
            colorMultiplier = 2
        Else
            colorMultiplier = 1
            colorCount = 0
        End If

        stitchCount = Val(Mid(TargetStitchPatternBox.Motif, 3, 3))
        curRowCount = Val(Mid(TargetStitchPatternBox.Motif, 7, 3))

        cubbySize = My.Settings.PatMax
        cutSize = Len(motif) * colorMultiplier
        cutSizeRows = curRowCount
        cuts = 1
        Remainder = Len(motif)
        '  If OutputMethod.Text = "E6000" Then

        If Len(motif) * colorMultiplier > cubbySize Then
            totalSize = Len(motif)
            perciseCuts = Len(motif) * colorMultiplier / cubbySize
            cutSizeRows = Int(Len(motif) / perciseCuts / stitchCount)
            cutSize = cutSizeRows * stitchCount 'cut size must break on rows
            If Int(perciseCuts * stitchCount) = Int(perciseCuts) * stitchCount Then
                cuts = Int(perciseCuts)
                Remainder = 0
            Else
                cuts = Int(perciseCuts) + 1 'Round up-- last cut will be partial
                Remainder = Len(motif) - cutSize * Int(perciseCuts)
            End If

        End If

        ' End If

        For i = cuts To 1 Step -1
            motifHeader = Trim(Str(colorCount)) & " " & Space(3 - Len(Trim(Str(stitchCount)))) & Trim(Str(stitchCount)) & " "
            newPatternRow = PatternBindingSource.AddNew()
            If i > 1 Then  'full cut
                motifHeader = motifHeader & Space(3 - Len(Trim(Str(cutSizeRows)))) & Trim(Str(cutSizeRows)) & " "
                newPatternRow("Motif") = motifHeader & Mid(motif, totalSize - cutSize * (cuts - i + 1) + 1, cutSize)
                curRowCount = curRowCount - cutSizeRows

            Else 'partial cut

                motifHeader = motifHeader & Space(3 - Len(Trim(Str(curRowCount)))) & Trim(Str(curRowCount)) & " "
                newPatternRow("Motif") = motifHeader & Mid(motif, 1, Remainder)
            End If

            newPatternRow("Id") = Guid.NewGuid
            newPatternRow("Name") = "My Pattern - " & Mid("ABCDEFGHIJKLMNOPQRSTUVWXYZ", (cuts - i + 1), 1)
            newPatternRow("Notes") = "My Notes"
            newPatternRow("BuiltIn") = False
            newPatternRow("FilterText") = newPatternRow("Name") & "My Notes"

            newPatternRow("Color1") = TargetStitchPatternBox.Color1
            newPatternRow("Color2") = TargetStitchPatternBox.Color2
            newPatternRow("Color3") = TargetStitchPatternBox.Color3
            newPatternRow("Color4") = TargetStitchPatternBox.Color4
            newPatternRow("ColorCount") = DbColorCount
            Try
                Me.PatternBindingSource.EndEdit()
                PatternBindingSource.Position = PatternBindingSource.Find("Id", newPatternRow("Id"))

            Catch ex As Exception
                PassapMsg.display(ex.Message, False, "")
            End Try
        Next i

        Me.PagePatternLibrary.BringToFront()
        Me.StitchPatLibPictureBox.Refresh()
        Me.Cursor = Cursors.Default

    End Sub

    Private Function getMotif(ByRef result As Bitmap) As String

        Dim i, j, k As Short
        Dim aColor As Color

        getMotif = ""
        For j = 0 To result.Height - 1
            For i = 0 To result.Width - 1
                aColor = result.GetPixel(i, j)

                For k = 0 To ColorCountComboBox.Text - 1
                    If aColor = PurePalette(k).color Then
                        getMotif = getMotif & Trim(Str(k))
                        Exit For

                    End If
                Next
            Next i
        Next j

    End Function

    Private Sub PatternsLink_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles PatternsLink.LinkClicked
        Me.PagePatternLibrary.BringToFront()

    End Sub

    Private Sub LoadStitchPatLink_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs)

        '  Color1Button.BackColor = Color.FromArgb(255, 255, 255, 255)
        ' Color2Button.BackColor = Color.FromArgb(255, 0, 0, 0)
        'Color3Button.BackColor = Color.FromArgb(255, 128, 128, 128)
        'Color4Button.BackColor = Color.FromArgb(255, 64, 64, 64)

        'Me.ColorCountComboBox.Text = "4"

        Me.PageImage.BringToFront()
    End Sub

    Private Sub EraseStitchPatternLink_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs)
        deleteStitchPattern()
    End Sub

    Private Sub StitchPatName_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StitchPatName.Leave
        sender.text = Mid(sender.text, 1, 200)
        PatternBindingSource.EndEdit()
        If Not IsNothing(PatternBindingSource.Current) Then
            PatternBindingSource.Current("FilterText") = PatternBindingSource.Current("Name") & " " & PatternBindingSource.Current("Notes")
        End If
        PatternBindingSource.EndEdit()

    End Sub

    Private Sub PagePatternLibrary_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PagePatternLibrary.Leave
        SaveStitchPattern()
    End Sub
    Sub SaveStitchPattern()
        'Me.Validate()
        Me.PatternBindingSource.EndEdit()
        If Not IsNothing(PatternBindingSource.Current) Then
            PatternBindingSource.Current("FilterText") = PatternBindingSource.Current("Name") & " " & PatternBindingSource.Current("Notes")
        End If
        PatternBindingSource.EndEdit()
#If Version = "Full" Then
        Try

            Me.PatternTableAdapter.Update(Me.JournalSixDataSet1.Pattern)
        Catch ex As Exception
            PassapMsg.display(ex.Message, False, "")
        End Try
#Else
#End If
    End Sub


    Private Function motifToBitmap(ByVal motif As String, ByVal color1 As Color, ByVal color2 As Color, ByVal color3 As Color, ByVal color4 As Color) As Bitmap
        Dim width As Integer
        Dim height As Integer
        Dim colorCount As Byte
        Dim aMotif As String
        Dim i, j As Integer
        Dim aBitmap As Bitmap

        colorCount = Val(Mid(motif, 1, 1))
        width = Val(Mid(motif, 3, 3))
        height = Val(Mid(motif, 7, 3))
        aMotif = Mid(motif, 11)

        Select Case colorCount
            Case 0
                Me.ColorCountComboBox.Text = "2"
                colorCount = 2
                Me.Color1Button.BackColor = color1
                Me.Color2Button.BackColor = color2
                setColors(PurePalette, colorCount)
                initializePalette(PurePalette, PurePalette, False)
                initializePalette(DitheredPalette, PurePalette, True)
                Exit Select
            Case 3
                Me.ColorCountComboBox.Text = "3"
                colorCount = 3
                Me.Color1Button.BackColor = color1
                Me.Color2Button.BackColor = color2
                Me.Color3Button.BackColor = color3
                setColors(PurePalette, colorCount)
                initializePalette(PurePalette, PurePalette, False)
                initializePalette(DitheredPalette, PurePalette, True)
                Exit Select
            Case 4
                Me.ColorCountComboBox.Text = "4"
                colorCount = 4
                Me.Color1Button.BackColor = color1
                Me.Color2Button.BackColor = color2
                Me.Color3Button.BackColor = color3
                Me.Color4Button.BackColor = color4
                setColors(PurePalette, colorCount)
                initializePalette(PurePalette, PurePalette, False)
                initializePalette(DitheredPalette, PurePalette, True)
                Exit Select
        End Select

        aBitmap = New Bitmap(width, height)
        For j = 0 To height - 1
            For i = 0 To width - 1
                aBitmap.SetPixel(i, j, PurePalette(Val(Mid(aMotif, j * width + i + 1, 1))).color)
            Next i
        Next j

        motifToBitmap = aBitmap

    End Function


    Private Sub ColorSeparation_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ColorSeparation.SelectedIndexChanged
        If ColorSeparation.Text = "Separate" Then
            ForeColor1.Text = "2"
            ForeColor2.Text = "2"
            ForeColor3.Text = "2"
            Forecolor4.Text = "2"

            BackColor1.Text = "1"
            BackColor2.Text = "1"
            BackColor3.Text = "1"
            BackColor4.Text = "1"
        Else
            ForeColor1.Text = "1"
            ForeColor2.Text = "2"
            ForeColor3.Text = "3"
            Forecolor4.Text = "4"

            BackColor1.Text = ""
            BackColor2.Text = ""
            BackColor3.Text = ""
            BackColor4.Text = ""
        End If
    End Sub

    Private Sub AddStitchPatToPieceLink_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles AddStitchPatToPieceLink.LinkClicked
        If Not IsNothing(Me.PiecePatternBindingSource.Current) Then
            Me.PiecePatternBindingSource.Current("PatternId") = Me.PatternBindingSource.Current("Id")
            setPiecePatternName()
            savePiecePattern()
            updateConsoleWarning()
            Me.PagePiecePattern.BringToFront()

        Else
            PassapMsg.display("Unable to add stitch pattern at this time.  No fabric piece selected", False, "")
        End If
    End Sub
    Private Sub setPiecePatternName()
        If Not IsNothing(PiecePatternTechniqueBindingSource.Current) Then
            If PiecePatternTechniqueBindingSource.Current("CastOn") Then
                Me.PiecePatternBindingSource.Current("Name") = "Cast On " & PiecePatternTechniqueBindingSource.Current("Name")

            Else
                If Microsoft.VisualBasic.Left(Me.PiecePatternBindingSource.Current("Name"), 7) = "ST. PAT" Then
                    Me.PiecePatternBindingSource.Current("Name") = Microsoft.VisualBasic.Left(Me.PiecePatternBindingSource.Current("Name"), 10)
                    If Not IsNothing(Me.PiecePatternPatternBindingSource.Current) Then
                        Me.PiecePatternBindingSource.Current("Name") = Me.PiecePatternBindingSource.Current("Name") & ": " & Me.PiecePatternPatternBindingSource.Current("Name")
                    End If
                    If Not IsNothing(Me.PiecePatternTechniqueBindingSource.Current) Then
                        Me.PiecePatternBindingSource.Current("Name") = Me.PiecePatternBindingSource.Current("Name") & ": " & Me.PiecePatternTechniqueBindingSource.Current("Name")
                    End If
                Else
                    If Not IsNothing(Me.PiecePatternPatternBindingSource.Current) Then
                        Me.PiecePatternBindingSource.Current("Name") = Me.PiecePatternPatternBindingSource.Current("Name")
                    End If
                    If Not IsNothing(Me.PiecePatternTechniqueBindingSource.Current) Then
                        Me.PiecePatternBindingSource.Current("Name") = Me.PiecePatternBindingSource.Current("Name") & ": " & Me.PiecePatternTechniqueBindingSource.Current("Name")
                    End If
                End If
            End If
        Else
            If Microsoft.VisualBasic.Left(Me.PiecePatternBindingSource.Current("Name"), 7) = "ST. PAT" Then
                Me.PiecePatternBindingSource.Current("Name") = Microsoft.VisualBasic.Left(Me.PiecePatternBindingSource.Current("Name"), 10)
                If Not IsNothing(Me.PiecePatternPatternBindingSource.Current) Then
                    Me.PiecePatternBindingSource.Current("Name") = Me.PiecePatternBindingSource.Current("Name") & ": " & Me.PiecePatternPatternBindingSource.Current("Name")
                End If
            Else
                If Not IsNothing(Me.PiecePatternPatternBindingSource.Current) Then
                    Me.PiecePatternBindingSource.Current("Name") = Me.PiecePatternPatternBindingSource.Current("Name")
                End If
            End If
        End If
    End Sub
    Private Sub updatePiecePattern()
        If Not IsNothing(PiecePatternBindingSource.Current) Then
        Else
            Dim filt As String

            filt = PatternBindingSource.Filter
            PatternBindingSource.RemoveFilter()
            PatternBindingSource.Find("Id", PiecePatternBindingSource.Current("PatternID"))

            If IsDBNull(PiecePatternBindingSource.Current("TechniqueID")) Then
                PiecePatternYarnFeeders.Enabled = False
            Else
                filt = TechniqueBindingSource.Filter
                TechniqueBindingSource.RemoveFilter()
                TechniqueBindingSource.Find("Id", PiecePatternBindingSource.Current("TechniqueID"))

            End If
        End If
        PiecePatternYarnFeeders.Refresh()
        PieceKTNeedleDiagram.Refresh()
        PieceStitchPatternBox.Refresh()

    End Sub

    Private Sub ReturnToPiece_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles ReturnToPiece.LinkClicked
        If IsNothing(ProjectPieceBindingSource.Current) Then
            PassapMsg.display("No fabric piece to return to.", False, "")

        Else
            Me.PagePiecePattern.BringToFront()
        End If
    End Sub

    Private Sub PiecesPageLink_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles PiecesPageLink.LinkClicked, PiecePageLink.LinkClicked
        If IsNothing(ProjectBindingSource.Current) Then
            PassapMsg.display("Please add new project before working with fabric pieces.", False, "")
        Else

            Me.PagePieces.BringToFront()
        End If

    End Sub


    Private Sub ImagePassapLink_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles ImagePassapLink.LinkClicked
        PageWelcome.BringToFront()
    End Sub

    Private Sub ImageMinimizeLink_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles ImageMinimizeLink.LinkClicked
        MinimizeForm()
    End Sub

    Private Sub ImageCloseLink_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles ImageCloseLink.LinkClicked
        Me.Close()
    End Sub

    Private Sub StitchLibPassapLink_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles StitchLibPassapLink.LinkClicked
        PageWelcome.BringToFront()
    End Sub

    Private Sub PatLibMinimizelink_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles PatLibMinimizelink.LinkClicked
        MinimizeForm()
    End Sub

    Private Sub StitchPatLibCloseLink_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles StitchPatLibCloseLink.LinkClicked
        Me.Close()
    End Sub


    Private Sub ClearStitchPatternButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ClearStitchPatternButton.Click
        setColors(PurePalette, ColorCountComboBox.Text)
        initializePalette(PurePalette, PurePalette, False)
        If Val(Me.StitchesTextBox.Text) > 0 And Val(Me.RowsTextBox.Text) > 0 Then

            TargetStitchPatternBox.Color1 = Color1Button.BackColor.ToArgb
            TargetStitchPatternBox.Color2 = Color2Button.BackColor.ToArgb
            TargetStitchPatternBox.Color3 = Color3Button.BackColor.ToArgb
            TargetStitchPatternBox.Color4 = Color4Button.BackColor.ToArgb
            TargetStitchPatternBox.ColorCount = Me.ColorCountComboBox.Text
            TargetStitchPatternBox.Motif = IIf(ColorCountComboBox.Text = "2", "0", ColorCountComboBox.Text) & " " & Space(3 - Len(StitchesTextBox.Text)) & StitchesTextBox.Text & " " & Space(3 - Len(RowsTextBox.Text)) & RowsTextBox.Text & " " & StrDup(CInt(Val(StitchesTextBox.Text) * Val(RowsTextBox.Text)), "0")

            TargetStitchPatternBox.Refresh()
        End If
    End Sub

    Private Sub StitchPatternList_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StitchPatternList.DoubleClick
        If Not IsNothing(PatternBindingSource.Current()) Then
            Dim destRect, srcRect As Rectangle
            Dim g As Graphics

            im = motifToBitmap(Me.PatternBindingSource.Current("Motif"), Color.FromArgb(Me.PatternBindingSource.Current("Color1")), Color.FromArgb(Me.PatternBindingSource.Current("Color2")), Color.FromArgb(Me.PatternBindingSource.Current("Color3")), Color.FromArgb((Me.PatternBindingSource.Current("Color4"))))
            initializeImagePage()
            Color1Button.BackColor = Color.FromArgb(PatternBindingSource.Current("Color1"))
            Color2Button.BackColor = Color.FromArgb(PatternBindingSource.Current("Color2"))
            Color3Button.BackColor = Color.FromArgb(PatternBindingSource.Current("Color3"))
            Color4Button.BackColor = Color.FromArgb(PatternBindingSource.Current("Color4"))

            ColorCountComboBox.Text = PatternBindingSource.Current("ColorCount")
            StitchesTextBox.Value = Val(Mid(PatternBindingSource.Current("Motif"), 3, 3))
            RowsTextBox.Value = Val(Mid(PatternBindingSource.Current("Motif"), 7, 3))
            StitchesTextBox.Text = StitchesTextBox.Value.ToString
            RowsTextBox.Text = RowsTextBox.Value.ToString
            TargetStitchPatternBox.Motif = Me.PatternBindingSource.Current("Motif")
            TargetStitchPatternBox.Color1 = PatternBindingSource.Current("Color1")
            TargetStitchPatternBox.Color2 = PatternBindingSource.Current("Color2")
            TargetStitchPatternBox.Color3 = PatternBindingSource.Current("Color3")
            TargetStitchPatternBox.Color4 = PatternBindingSource.Current("Color4")
            TargetStitchPatternBox.ColorCount = PatternBindingSource.Current("ColorCount")
            TargetStitchPatternBox.Refresh()

            If im.Width > 25 Then
                SourcePictureBox.Width = 200
                SourcePictureBox.Height = 200 * im.Height / im.Width
                If SourcePictureBox.Height > 200 Then
                    SourcePictureBox.Width = 200 * im.Width / im.Height
                    SourcePictureBox.Height = 200
                End If
            Else
                SourcePictureBox.Width = im.Width * 10
                SourcePictureBox.Height = im.Height * 10
                If SourcePictureBox.Height > 200 Then
                    SourcePictureBox.Width = 200 * im.Width / im.Height
                    SourcePictureBox.Height = 200
                End If
            End If

            destRect = New Rectangle(0, 0, SourcePictureBox.Width, SourcePictureBox.Height)
            srcRect = New Rectangle(0, 0, im.Width, im.Height)

            sourceBMap = New Bitmap(SourcePictureBox.Width, SourcePictureBox.Height)

            g = Graphics.FromImage(sourceBMap)
            g.PixelOffsetMode = Drawing2D.PixelOffsetMode.Half
            g.SmoothingMode = Drawing2D.SmoothingMode.None
            g.InterpolationMode = Drawing2D.InterpolationMode.NearestNeighbor
            g.DrawImage(im, destRect, srcRect, GraphicsUnit.Pixel)

            SourcePictureBox.Image = sourceBMap

            Me.PageImage.BringToFront()
        End If
    End Sub

    Private Sub ReplaceStitchPatternLink_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles ReplaceStitchPatternLink.LinkClicked
        If Not IsNothing(PatternBindingSource.Current) Then

            Me.Cursor = Cursors.WaitCursor
            Try
                PatternBindingSource.Current("colorCount") = TargetStitchPatternBox.ColorCount
                PatternBindingSource.Current("Color1") = TargetStitchPatternBox.Color1
                PatternBindingSource.Current("Color2") = TargetStitchPatternBox.Color2
                PatternBindingSource.Current("Color3") = TargetStitchPatternBox.Color3
                PatternBindingSource.Current("Color4") = TargetStitchPatternBox.Color4
                PatternBindingSource.Current("Motif") = TargetStitchPatternBox.Motif

                Me.PatternBindingSource.EndEdit()

            Catch ex As Exception
                PassapMsg.display(ex.Message, False, "")
            End Try
        Else
            PassapMsg.display("No stitch pattern selected to replace.", False, "")
        End If

        Me.PagePatternLibrary.BringToFront()
        Me.StitchPatLibPictureBox.Refresh()
        Me.Cursor = Cursors.Default

    End Sub
    Private Sub CancelNewStitchPatLink_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles CancelNewStitchPatLink.LinkClicked
        Me.PagePatternLibrary.BringToFront()
    End Sub
    Private Sub PrintStitchPattern(ByVal Motif As String, ByVal MotifName As String)
        Dim result As DialogResult = PrintDialog1.ShowDialog()
        If (result = DialogResult.OK) Then
            Dim amotif As String
            Dim cardWidth, width, height As Integer
            Dim CardCounter As Integer
            Dim rowPointer, columnPointer
            Dim CardLetter As String

            width = Val(Mid(Motif, 3, 3))
            height = Val(Mid(Motif, 7, 3))
            amotif = Mid(Motif, 11)
            CardLetter = "A"

            CardDoc.PrinterSettings = PrintDialog1.PrinterSettings
            CardDoc.Motif = ""
            columnPointer = 0
            While columnPointer < width
                If width > 40 Then
                    If columnPointer + 39 <= width Then
                        cardWidth = 40
                    Else
                        cardWidth = width - columnPointer
                    End If
                Else
                    cardWidth = width
                End If
                rowPointer = height - 1
                CardCounter = 1
                While rowPointer >= 0
                    CardDoc.Motif = Mid(amotif, rowPointer * width + columnPointer + 1, cardWidth) & CardDoc.Motif

                    CardDoc.KnitTechnique = False
                    If rowPointer <> 0 Then
                        CardDoc.Continuation = True
                        CardDoc.LastCard = False
                    Else
                        CardDoc.Continuation = False
                        'CardDoc.LastRow = True
                        If cardWidth < 40 Or columnPointer + 39 = width Then
                            CardDoc.LastCard = True
                        End If
                    End If


                        If CardCounter = 63 Then
                            CardDoc.MotifWidth = cardWidth
                            CardDoc.MotifHeight = 63
                            CardDoc.DocumentName = MotifName & " Stitch Pattern " & CardLetter
                            CardDoc.Print()
                            CardDoc.Motif = ""
                            CardCounter = 1
                            CardLetter = Chr(Asc(CardLetter) + 1)
                        Else
                            CardCounter = CardCounter + 1
                        End If

                        rowPointer = rowPointer - 1


                End While

                If Len(CardDoc.Motif) > 0 Then
                    CardDoc.MotifWidth = cardWidth
                    CardDoc.MotifHeight = Len(CardDoc.Motif) / cardWidth
                    CardDoc.DocumentName = MotifName & " Stitch Pattern " & CardLetter
                    CardDoc.Print()
                    CardDoc.Motif = ""
                    CardLetter = Chr(Asc(CardLetter) + 1)
                End If
                columnPointer = columnPointer + cardWidth
            End While
        End If

    End Sub
    Private Sub PrintKnitTechnique(ByVal tech As Byte(), ByVal techName As String)
        Dim result As DialogResult = PrintDialog1.ShowDialog()
        If (result = DialogResult.OK) Then
            Dim amotif As String
            Dim cardWidth, width, height As Integer
            Dim CardCounter As Integer
            Dim rowPointer, columnPointer
            Dim CardLetter As String

            width = 40
            height = Int(tech.Length / 5)

            amotif = convertByteArrayToMotif(tech)
            CardLetter = "A"

            CardDoc.PrinterSettings = PrintDialog1.PrinterSettings
            CardDoc.KnitTechnique = True

            CardDoc.Motif = ""
            columnPointer = 0
            While columnPointer < width
                If width > 40 Then
                    If columnPointer + 39 <= width Then
                        cardWidth = 40
                    Else
                        cardWidth = width - columnPointer
                    End If
                Else
                    cardWidth = width
                End If
                rowPointer = height - 1
                CardCounter = 1

                While rowPointer >= 0
                    CardDoc.Motif = Mid(amotif, rowPointer * width + columnPointer + 1, cardWidth) & CardDoc.Motif

                    If CardCounter = 64 Then
                        CardDoc.MotifWidth = cardWidth
                        CardDoc.MotifHeight = 64
                        CardDoc.DocumentName = techName & " Technique " & CardLetter
                        If CardLetter = "A" Then
                            CardDoc.FirstCard = True
                        Else
                            CardDoc.FirstCard = False
                        End If
                        If rowPointer <> 0 Then
                            CardDoc.Continuation = True
                        Else
                            CardDoc.Continuation = False
                        End If
                        CardDoc.KnitTechnique = True
                        CardDoc.Print()
                        CardDoc.Motif = ""
                        CardCounter = 1
                        CardLetter = Chr(Asc(CardLetter) + 1)
                    Else
                        CardCounter = CardCounter + 1
                    End If

                    rowPointer = rowPointer - 1


                End While

                If Len(CardDoc.Motif) > 0 Then
                    CardDoc.MotifWidth = cardWidth
                    CardDoc.MotifHeight = Len(CardDoc.Motif) / cardWidth
                    CardDoc.DocumentName = techName & " Technique " & CardLetter
                    If CardLetter = "A" Then
                        CardDoc.FirstCard = True
                    Else
                        CardDoc.FirstCard = False
                    End If
                    CardDoc.LastCard = True
                    CardDoc.Continuation = False
                    CardDoc.KnitTechnique = True
                    CardDoc.Print()
                    CardDoc.Motif = ""
                    CardLetter = Chr(Asc(CardLetter) + 1)
                End If
                columnPointer = columnPointer + cardWidth
            End While
        End If

    End Sub
    Private Sub Printchart(ByVal chart As Byte(), ByVal chartName As String)

        Dim result As DialogResult = PrintDialog1.ShowDialog()
        If (result = DialogResult.OK) Then
            Dim amotif As String
            Dim cardWidth, width, height As Integer
            Dim CardCounter As Integer
            Dim rowPointer, columnPointer
            Dim CardLetter As String

            width = 40
            height = Int(chart.Length / 5)

            amotif = convertByteArrayToMotif(chart)
            CardLetter = "A"

            CardDoc.PrinterSettings = PrintDialog1.PrinterSettings
            CardDoc.KnitTechnique = True

            CardDoc.Motif = ""
            columnPointer = 0
            While columnPointer < width
                If width > 40 Then
                    If columnPointer + 39 <= width Then
                        cardWidth = 40
                    Else
                        cardWidth = width - columnPointer
                    End If
                Else
                    cardWidth = width
                End If
                rowPointer = height - 1
                CardCounter = 1

                While rowPointer >= 0
                    CardDoc.Motif = Mid(amotif, rowPointer * width + columnPointer + 1, cardWidth) & CardDoc.Motif

                    If CardCounter = 64 Then
                        CardDoc.MotifWidth = cardWidth
                        CardDoc.MotifHeight = 64
                        CardDoc.DocumentName = chartName & " chart " & CardLetter
                        If CardLetter = "A" Then
                            CardDoc.FirstCard = True
                        Else
                            CardDoc.FirstCard = False
                        End If
                        If rowPointer <> 0 Then
                            CardDoc.Continuation = True
                        Else
                            CardDoc.Continuation = False
                        End If
                        CardDoc.KnitTechnique = True
                        CardDoc.Print()
                        CardDoc.Motif = ""
                        CardCounter = 1
                        CardLetter = Chr(Asc(CardLetter) + 1)
                    Else
                        CardCounter = CardCounter + 1
                    End If

                    rowPointer = rowPointer - 1


                End While

                If Len(CardDoc.Motif) > 0 Then
                    CardDoc.MotifWidth = cardWidth
                    CardDoc.MotifHeight = Len(CardDoc.Motif) / cardWidth
                    CardDoc.DocumentName = chartName & " chart " & CardLetter
                    If CardLetter = "A" Then
                        CardDoc.FirstCard = True
                    Else
                        CardDoc.FirstCard = False
                    End If
                    CardDoc.LastCard = True
                    CardDoc.Continuation = False
                    CardDoc.KnitTechnique = True
                    CardDoc.Print()
                    CardDoc.Motif = ""
                    CardLetter = Chr(Asc(CardLetter) + 1)
                End If
                columnPointer = columnPointer + cardWidth
            End While
        End If

    End Sub
    Private Sub PCStartPatternButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PCStartPatternButton.Click

#If Version = "Trial" Then
        If sender.text = "PC START" Or My.Settings.OutputMethod = "Printer" Then
            PassapMsg.display("Demonstration version of software not capable of downloading or printing stitch patterns.  A demonstration stitch pattern will be used instead.  Please upgrade by clicking the Buy Now Button on the table of contents.", False, "")
        End If
        DownloadDemoPattern(sender)
#Else
        Dim SKBitmap As Bitmap
        Try
            Select Case sender.text
                Case "ST.PATT"
                    If My.Settings.OutputMethod = "E6000" Then
                        sender.Text = "PC START"
                        E6000Port.Close()
                        E6000Port.Open()
                    ElseIf My.Settings.OutputMethod = "Printer" Then
                        sender.Text = "PRINTER"
                        My.Application.DoEvents()
                        If Not IsNothing(Me.PiecePatternPatternBindingSource.Current()) Then

                            PrintStitchPattern(Me.PiecePatternPatternBindingSource.Current("Motif"), Me.PiecePatternPatternBindingSource.Current("Name"))
                        Else
                            PassapMsg.display("Cannot download stitch pattern.  No stitch pattern associated with this fabric piece.", False, "")
                        End If
                        sender.Text = "ST.PATT"
                        My.Application.DoEvents()
                    ElseIf My.Settings.OutputMethod = "SilverKnit" Then
                        If Not IsNothing(Me.PiecePatternPatternBindingSource.Current()) Then
                            SKBitmap = motifToBitmap(Me.PiecePatternPatternBindingSource.Current("Motif"), Color.FromArgb(Me.PiecePatternPatternBindingSource.Current("Color1")), Color.FromArgb(Me.PiecePatternPatternBindingSource.Current("Color2")), Color.FromArgb(Me.PiecePatternPatternBindingSource.Current("Color3")), Color.FromArgb((Me.PiecePatternPatternBindingSource.Current("Color4"))))
                            SKBitmap.Save(Application.LocalUserAppDataPath & "\sk.bmp")
                            Shell(SilverKnit & " " & Application.LocalUserAppDataPath & "\sk.bmp")
                        Else
                            PassapMsg.display("Cannot download stitch pattern.  No stitch pattern associated with this fabric piece.", False, "")
                        End If
                        sender.Text = "ST.PATT"
                    End If
                    Exit Select
                Case "PC START"
                    sender.Text = "WAIT"
                    Application.DoEvents()
                    If Not IsNothing(Me.PiecePatternPatternBindingSource.Current()) Then
                        Try
                            Dim pat As Byte()
                            pat = StringToBytes(Me.PiecePatternPatternBindingSource.Current("Motif"))
                            Me.E6000Port.Write(pat, 0, pat.Length)

                        Catch ex As Exception
                            PassapMsg.display(ex.Message, False, "")
                        End Try

                    Else
                        PassapMsg.display("Cannot download stitch pattern.  No stitch pattern associated with this fabric piece.", False, "")
                    End If
                    E6000Port.Close()
                    sender.Text = "ST.PATT"
                    Exit Select
                Case Else
                    sender.Text = "ST.PATT"
                    Exit Select
            End Select
        Catch ex As Exception
            PassapMsg.display(ex.Message, False, "")
        End Try
#End If
    End Sub
    Private Sub KnitTechsLink_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles KnitTechsLink.LinkClicked
        Me.PageTechnique.BringToFront()
    End Sub

    Private Sub NewTechniqueLink_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles NewTechniqueLink.LinkClicked

        Dim newTechniqueRow As JournalSixDataSet.TechniqueRow
        saveTechnique()
        newTechniqueRow = JournalSixDataSet1.Technique.NewTechniqueRow
        newTechniqueRow.Id = Guid.NewGuid
        newTechniqueRow.Name = "My Knit Technique"
        newTechniqueRow.Notes = "My Technique"
        newTechniqueRow.BackNeedles = Space(20)
        newTechniqueRow.BackPushers = Space(20)
        newTechniqueRow.FrontNeedles = Space(20)
        newTechniqueRow.FrontPushers = Space(20)
        newTechniqueRow.Custom = False
        newTechniqueRow.Racking = 0
        newTechniqueRow.Program = New Byte() {0, 0, 0, 0, 0}
        newTechniqueRow.Rows = 1
        newTechniqueRow.Feeder1 = 0
        newTechniqueRow.Feeder2 = 0
        newTechniqueRow.Feeder3 = 0
        newTechniqueRow.Feeder4 = 0
        newTechniqueRow.CastOn = False
        newTechniqueRow.CarriagePassesPerChartRow = 0

        JournalSixDataSet1.Technique.Rows.Add(newTechniqueRow)

        Application.DoEvents()
        Try
            Me.Validate()
            Me.TechniqueBindingSource.EndEdit()
#If Version = "Full" Then
            Me.TechniqueTableAdapter.Update(Me.JournalSixDataSet1.Technique)
#Else
#End If
        Catch ex As Exception
            PassapMsg.display(ex.Message, False, "")
        End Try

        TechniqueBindingSource.Position = TechniqueBindingSource.Find("Id", newTechniqueRow.Id)
        If TechniqueBindingSource.Position >= 0 Then
            TechniqueList.SetSelected(TechniqueBindingSource.Position, True)
        End If
    End Sub


    Private Sub TechniqueList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TechniqueList.SelectedIndexChanged
        TechLibCastOnCheckBox.Refresh()
        TechLibCRTDiagram.Refresh()
        TechLibNeedleDiagram.Refresh()
        TechniquesYarnFeeders.Refresh()
    End Sub

    Private Sub TechLibFilterLink_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles TechLibFilterLink.LinkClicked
        TechLibFilterText.Visible = True
        TechLibFilterText.Focus()
        TechLibFilterButton.Visible = True
        TechLibFilterLink.Visible = False
    End Sub

    Private Sub TechLibFilterButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TechLibFilterButton.Click
        TechniqueBindingSource.EndEdit()
        TechniqueBindingSource.Filter = makeFilter(TechLibFilterText.Text)
        TechLibFilterText.Visible = False
        TechLibFilterButton.Visible = False
        TechLibFilterLink.Text = "Knit Techniques: " & TechLibFilterText.Text
        TechLibFilterLink.Visible = True

        TechniqueBindingSource.MoveFirst()
        If TechniqueBindingSource.Position >= 0 Then
            TechniqueList.SetSelected(TechniqueBindingSource.Position, True)
        End If
    End Sub
    Private Function makeFilter(ByVal keywordString As String) As String
        Dim keys() As String
        keys = Split(keywordString)
        If LCase(keywordString) = "all" Or LCase(keywordString) = "" Or LCase(keywordString) = "*" Then
            makeFilter = ""
        Else
            makeFilter = "FilterText like "
            'makeFilter = "Name like "
            For Each key As String In keys
                makeFilter = makeFilter & "'%" & key & "%' and FilterText like "
                ' makeFilter = makeFilter & "'%" & key & "%' and Name like "

            Next
            makeFilter = Mid(makeFilter, 1, Len(makeFilter) - 21) 'strip trailing and like


        End If
    End Function

    Private Sub FormProgrammesLibLink_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles FormLibFilterLink.LinkClicked
        FormLibFilterText.Visible = True
        FormLibFilterText.Focus()
        FormLibFilterButton.Visible = True
        FormLibFilterLink.Visible = False
    End Sub

    Private Sub FormLibFilterButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FormLibFilterButton.Click
        FormBindingSource.EndEdit()
        FormBindingSource.Filter = makeFilter(FormLibFilterText.Text)
        FormLibFilterText.Visible = False
        FormLibFilterButton.Visible = False
        FormLibFilterLink.Text = "Form Programmes: " & FormLibFilterText.Text
        FormLibFilterLink.Visible = True

        FormBindingSource.MoveFirst()
        If FormBindingSource.Position >= 0 Then
            FormList.SetSelected(FormBindingSource.Position, True)
        End If
    End Sub

    Private Sub StitchPatFilterLink_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles StitchPatLibFilterLink.LinkClicked
        StitchPatLibFilterText.Visible = True
        StitchPatLibFilterText.Focus()
        StitchPatLibFilterButton.Visible = True
        StitchPatLibFilterLink.Visible = False
    End Sub

    Private Sub StitchPatLibFilterButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StitchPatLibFilterButton.Click
        PatternBindingSource.EndEdit()
        PatternBindingSource.Filter = makeFilter(StitchPatLibFilterText.Text)
        StitchPatLibFilterText.Visible = False
        StitchPatLibFilterButton.Visible = False
        StitchPatLibFilterLink.Text = "Stitch Patterns: " & StitchPatLibFilterText.Text
        StitchPatLibFilterLink.Visible = True

        PatternBindingSource.MoveFirst()
        If PatternBindingSource.Position >= 0 Then
            StitchPatternList.SetSelected(PatternBindingSource.Position, True)
        End If
    End Sub

    Private Sub ProjectFilterLink_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles ProjectFilterLink.LinkClicked
        ProjectFilterText.Visible = True
        ProjectFilterText.Focus()
        ProjectFilterButton.Visible = True
        ProjectFilterLink.Visible = False
        ProjectFilterText.Focus()
    End Sub

    Private Sub ProjectFilterButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ProjectFilterButton.Click
        ProjectBindingSource.EndEdit()
        ProjectBindingSource.Filter = makeFilter(ProjectFilterText.Text)
        ProjectFilterText.Visible = False
        ProjectFilterButton.Visible = False
        ProjectFilterLink.Text = "Projects: " & ProjectFilterText.Text
        ProjectFilterLink.Visible = True

        ProjectBindingSource.MoveFirst()
        If ProjectBindingSource.Position >= 0 Then
            ProjectList.SetSelected(ProjectBindingSource.Position, True)
        End If
    End Sub

    Private Sub TechLibCloseLink_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles TechLibCloseLink.LinkClicked
        Me.Close()
    End Sub

    Private Sub TechLibMinimizeLink_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles TechLibMinimizeLink.LinkClicked

        Me.MinimizeForm()
    End Sub

    Private Sub TechLibPassapPalLink_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles TechLibPassapPalLink.LinkClicked

        Me.PageWelcome.BringToFront()
    End Sub

    Private Sub FormProgramsLink_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles FormProgramsLink.LinkClicked

        Me.PageForm.BringToFront()
    End Sub

    Private Sub TechLibCurrentPieceLink_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles TechLibCurrentPieceLink.LinkClicked
        If Not IsNothing(PiecePatternBindingSource.Current) Then
            Me.PagePiecePattern.BringToFront()
        Else
            PassapMsg.display("No fabric piece to return to.", False, "")
        End If

    End Sub

    Private Sub AboutLink_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles AboutLink.LinkClicked
        Me.PageAbout.BringToFront()
    End Sub

    Private Sub AboutPassapPalLink_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles AboutPassapPalLink.LinkClicked
        Me.PageWelcome.BringToFront()
    End Sub

    Private Sub AboutMinimizeLink_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles AboutMinimizeLink.LinkClicked
        Me.MinimizeForm()
    End Sub

    Private Sub AboutCloseLink_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles AboutCloseLink.LinkClicked
        Me.Close()
    End Sub

    Private Sub PatternTechniqueLink_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles PatternTechniqueLink.LinkClicked
        Me.PageTechnique.BringToFront()
        Application.DoEvents()
    End Sub

    Private Sub UseKnitTechniqueLink_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles UseKnitTechniqueLink.LinkClicked
        saveTechnique()
        If Not IsNothing(PiecePatternBindingSource.Current()) Then
            Me.PiecePatternBindingSource.Current("TechniqueId") = Me.TechniqueBindingSource.Current("Id")
            setPiecePatternName()
            savePiecePattern()
            Me.PagePiecePattern.BringToFront()
        Else
            PassapMsg.display("Unable to add knit technique at this time.  No fabric piece selected.", False, "")
        End If
    End Sub
    Private Sub E6000Link_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles E6000Link.LinkClicked
        Me.PageSettings.BringToFront()
    End Sub
    Private Sub TechlibName_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TechlibName.Leave
        TechlibName.Text = Mid(TechlibName.Text, 1, 200)
        TechniqueBindingSource.EndEdit()
        If Not IsNothing(TechniqueBindingSource.Current) Then
            TechniqueBindingSource.Current("FilterText") = TechniqueBindingSource.Current("Name") & " " & TechniqueBindingSource.Current("Notes")
        End If

        TechniqueBindingSource.EndEdit()
    End Sub
    Private Sub FormName_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FormName.Leave
        FormName.Text = Mid(FormName.Text, 1, 200)
        FormBindingSource.EndEdit()

        If Not IsNothing(FormBindingSource.Current) Then
            FormBindingSource.Current("FilterText") = FormBindingSource.Current("Name") & " " & FormBindingSource.Current("Notes")
        End If

        FormBindingSource.EndEdit()

    End Sub
    Private Sub PageTechnique_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PageTechnique.Leave
        saveTechnique()
    End Sub
    Private Sub SettingsTOCLink_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles SettingsTOCLink.LinkClicked
        PageWelcome.BringToFront()
        Application.DoEvents()
    End Sub
    Private Sub GrayScaleButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GrayScaleButton.Click

        Dim avg As Integer

        avg = CInt(Color1Button.BackColor.R) + CInt(Color1Button.BackColor.G) + CInt(Color1Button.BackColor.B)
        avg = Int(avg / 3)
        Color1Button.BackColor = Color.FromArgb(255, avg, avg, avg)

        avg = CInt(Color2Button.BackColor.R) + CInt(Color2Button.BackColor.G) + CInt(Color2Button.BackColor.B)
        avg = Int(avg / 3)
        Color2Button.BackColor = Color.FromArgb(255, avg, avg, avg)

        avg = CInt(Color3Button.BackColor.R) + CInt(Color3Button.BackColor.G) + CInt(Color3Button.BackColor.B)
        avg = Int(avg / 3)
        Color3Button.BackColor = Color.FromArgb(255, avg, avg, avg)

        avg = CInt(Color4Button.BackColor.R) + CInt(Color4Button.BackColor.G) + CInt(Color4Button.BackColor.B)
        avg = Int(avg / 3)
        Color4Button.BackColor = Color.FromArgb(255, avg, avg, avg)

    End Sub

    Private Sub SettingConsoleMemText_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SettingConsoleMemText.SelectedIndexChanged
        My.Settings.ConsoleMax = sender.text()
    End Sub
    Private Sub SettingPatMemText_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SettingPatMemText.TextChanged
        My.Settings.PatMax = sender.Text()
    End Sub
    Private Sub SettingComPortText_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SettingComPortText.SelectedIndexChanged
        Try
            My.Settings.ComPort = sender.text
            E6000Port.Close()
            E6000Port.PortName = My.Settings.ComPort
            E6000Port.Open()
        Catch ex As Exception
            PassapMsg.display(ex.Message, False, "")
        End Try
    End Sub
    Private Sub RevertSettingsLink_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles RevertSettingsLink.LinkClicked
        Try
            SettingPatMemText.Text = 45900
            SettingConsoleMemText.Text = 47520
            OutputMethod.Text = "E6000"
            E6000Port.Close()
            E6000Port.Open()
        Catch ex As Exception
            PassapMsg.display(ex.Message, False, "")
        End Try
    End Sub
    Private Sub CoverPictureBox_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles CoverPictureBox.MouseDown
        If e.Button = Windows.Forms.MouseButtons.Left Then
            dragging = True
            pointClicked = New Point(e.X, e.Y)
            Me.Cursor = Cursors.SizeAll
        Else
            dragging = False
        End If
    End Sub
    Private Sub CoverPictureBox_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles CoverPictureBox.MouseMove

        If dragging Then
            Dim pointMoveTo As Point
            pointMoveTo = Me.PointToScreen(New Point(e.X, e.Y))
            pointMoveTo.Offset(-pointClicked.X, -pointClicked.Y)
            Me.Location = pointMoveTo
        End If
    End Sub

    Private Sub CoverPictureBox_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles CoverPictureBox.MouseUp
        dragging = False
        Me.Cursor = Cursors.Default
    End Sub
    Private Sub PageForm_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PageForm.Leave
        FormBindingSource.EndEdit()
#If Version = "Full" Then
        Me.FormTableAdapter.Update(Me.JournalSixDataSet1.Form)
#Else
#End If

    End Sub

    Private Sub LoadImageButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LoadImageButton.Click

        loadStitchPattern()
    End Sub

    Private Sub CoverPictureBox_MouseHover(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CoverPictureBox.MouseHover
        Me.Cursor = Cursors.SizeAll
    End Sub

    Private Sub CoverPictureBox_MouseLeave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CoverPictureBox.MouseLeave
        Me.Cursor = Cursors.Default
    End Sub

    Private Sub ReturnPiecePageLink_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles ReturnPiecePageLink.LinkClicked
        If Not IsNothing(ProjectPieceBindingSource.Current) Then
            PagePieces.BringToFront()
        Else
            PassapMsg.display("No fabric piece to return to.", False, "")
        End If

    End Sub


    Private Sub ProjectList_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ProjectList.DoubleClick
        Me.PagePieces.BringToFront()
    End Sub


    Private Sub StitchPatternList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StitchPatternList.SelectedIndexChanged
        StitchPatLibPictureBox.Refresh()
    End Sub

    Private Sub UpdateColorsButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UpdateColorsButton.Click
        TargetStitchPatternBox.Color1 = Color1Button.BackColor.ToArgb
        TargetStitchPatternBox.Color2 = Color2Button.BackColor.ToArgb
        TargetStitchPatternBox.Color3 = Color3Button.BackColor.ToArgb
        TargetStitchPatternBox.Color4 = Color4Button.BackColor.ToArgb
        TargetStitchPatternBox.Refresh()

    End Sub

    Private Sub PiecePatternList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PiecePatternList.SelectedIndexChanged
        PieceStitchPatternBox.Refresh()
        PieceKTNeedleDiagram.Refresh()
        PiecePatternYarnFeeders.Refresh()
    End Sub

    Private Sub PurchaseLink_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles PurchaseLink.LinkClicked
        System.Diagnostics.Process.Start(My.Settings.Website)
    End Sub

    Private Sub PCStartTechButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PCStartTechButton.Click
#If Version = "Trial" Then
        If sender.text = "PC START" Or My.Settings.OutputMethod = "Printer" Then
            PassapMsg.display("Demonstration version of software not capable of downloading or printing knit techniquess.  Please upgrade by clicking the Buy Now Button.  The demonstration knit technique will be used instead.", False, "")
        End If
        DownloadDemoTechnique(sender)
#Else
        Try
            Dim i As Integer
            Dim x, y As Integer
            Dim patBytes As Integer
            Dim TechProgram() As Byte

            Dim endtech As Boolean

            Select Case sender.text
                Case "KNIT.TECH"
                    If My.Settings.OutputMethod = "E6000" Then
                        sender.Text = "PC START"
                        E6000Port.Close()
                        E6000Port.Open()
                    ElseIf My.Settings.OutputMethod = "Printer" Then
                        sender.Text = "PRINTER"
                        My.Application.DoEvents()
                        If Not IsNothing(Me.PiecePatternTechniqueBindingSource.Current()) Then

                            PrintKnitTechnique(Me.PiecePatternTechniqueBindingSource.Current("Program"), Me.PiecePatternTechniqueBindingSource.Current("Name"))
                        Else
                            PassapMsg.display("Cannot print technique.  No knit technique associated with this stitch pattern.", False, "")
                        End If
                        sender.Text = "KNIT.TECH"
                        My.Application.DoEvents()
                    End If
                    Exit Select
                Case "PC START"
                    sender.Text = "WAIT"
                    Application.DoEvents()

                    If Not IsNothing(Me.PiecePatternTechniqueBindingSource.Current()) Then
                        TechProgram = Me.PiecePatternTechniqueBindingSource.Current("Program")

                        x = TechProgram(1)
                        y = TechProgram(2)
                        patBytes = Int((x * y) / 8 + 1) 'always round up
                        i = 0
                        endtech = False

                        While Not endtech 'Write out tech through ef and Pre row bytes bytes
                            If TechProgram(i + 1) = 239 Then
                                endtech = True
                                i = i + 4
                            Else
                                i = i + 1
                            End If
                        End While
                        '   Select Case (i) Mod 5 'check position of ef byte (i instead of i-1 because 0 based array
                        '      Case 0 'Done with technique.  Ready for pattern
                        ' Exit Select
                        '    Case 1 'Write four more bytes
                        'i = i + 4
                        'Exit Select
                        '    Case 2 'Write three more byte
                        'i = i + 3
                        'Exit Select
                        '   Case 3 'Write two more byte
                        'i = i + 2
                        'Exit Select
                        '    Case 4 'Write one more byte
                        'i = i + 1
                        'Exit Select
                        'End Select
                        i = i + patBytes
                        Try
                            Me.E6000Port.Write(TechProgram, 0, i)
                        Catch ex As Exception
                            PassapMsg.display(ex.Message, False, "")
                        End Try
                    Else
                        PassapMsg.display("Cannot download technique.  No technique associated with this pattern.", False, "")
                    End If

                    sender.Text = "KNIT.TECH"
                    E6000Port.Close()


                Case Else
                    sender.Text = "KNIT.TECH"
            End Select
        Catch ex As Exception
            PassapMsg.display(ex.Message, False, "")
        End Try
#End If

    End Sub

    Private Sub SettingsMinimizeLink_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles SettingsMinimizeLink.LinkClicked
        Me.WindowState = FormWindowState.Minimized
    End Sub

    Private Sub SettingsCloseLink_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles SettingsCloseLink.LinkClicked
        Me.Close()
    End Sub

    Private Sub EraseTechniqueLink_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles EraseTechniqueLink.LinkClicked
        deleteTechnique()
    End Sub


    Private Sub TestStitchPatButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TestStitchPatButton.Click
        DownloadDemoPattern(sender)
    End Sub

    Private Function StringToBytes(ByVal myString As String) As Byte()
        Dim myBytes(myString.Length - 1) As Byte
        For i As Integer = 1 To myString.Length
            myBytes(i - 1) = Asc(Mid(myString, i, 1))
        Next
        StringToBytes = myBytes
    End Function
    Private Sub DownloadDemoPattern(ByVal sender As System.Object)
        Dim SKBitmap As Bitmap
        Try
            Select Case sender.text
                Case "ST.PATT"
                    If My.Settings.OutputMethod = "E6000" Then

                        sender.Text = "PC START"
                        E6000Port.Close()
                        E6000Port.Open()
                    ElseIf My.Settings.OutputMethod = "Printer" Then
                        sender.Text = "PRINTER"
                        My.Application.DoEvents()
                        PrintStitchPattern("0  20   6 000000000000000000001110011101000100110010010100011011010010100101100101010100101001010001000101001011100111010001001100", "DEMO")
                        sender.Text = "ST.PATT"
                        My.Application.DoEvents()
                    ElseIf My.Settings.OutputMethod = "SilverKnit" Then
                        SKBitmap = motifToBitmap("0  20   6 000000000000000000001110011101000100110010010100011011010010100101100101010100101001010001000101001011100111010001001100", Color.White, Color.Black, Color.White, Color.White)
                        SKBitmap.Save(Application.LocalUserAppDataPath & "\sk.bmp")
                        Shell(SilverKnit & " " & Application.LocalUserAppDataPath & "\sk.bmp")
                        sender.Text = "ST.PATT"
                    End If
                    Exit Select
                Case "PC START"
                    sender.Text = "WAIT"
                    Application.DoEvents()

                    'DEMO stitch pat
                    Try
                        Dim pat As Byte()
                        pat = StringToBytes("0  20   6 000000000000000000001110011101000100110010010100011011010010100101100101010100101001010001000101001011100111010001001100")
                        Me.E6000Port.Write(pat, 0, pat.Length)
                        ' Me.E6000Port.Write("0   2   2 0110")
                    Catch ex As Exception
                        PassapMsg.display(ex.Message, False, "")
                    End Try
                    sender.Text = "ST.PATT"
                    E6000Port.Close()

                    Exit Select
                Case Else
                    sender.Text = "ST.PATT"
                    Exit Select
            End Select
        Catch ex As Exception
            PassapMsg.display(ex.Message, False, "")
        End Try

    End Sub

    Private Sub TestTechniqueButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TestTechniqueButton.Click
        DownloadDemoTechnique(sender)
    End Sub
    Private Sub DownloadDemoTechnique(ByVal sender As System.Object)
        Try

            Select Case sender.text
                Case "KNIT.TECH"
                    If My.Settings.OutputMethod = "E6000" Then
                        sender.Text = "PC START"
                        E6000Port.Close()
                        E6000Port.Open()
                    ElseIf My.Settings.OutputMethod = "Printer" Then
                        sender.Text = "PRINTER"
                        My.Application.DoEvents()
                        Dim Tech() As Byte = {1, 1, 1, 128, 242, 0, 0, 0, 34, 7, 4, 34, 7, 4, 233, 239, 128, 0, 128, 0}
                        PrintKnitTechnique(Tech, "DEMO")
                        sender.Text = "KNIT.TECH"
                        My.Application.DoEvents()
                    End If
                    Exit Select
                Case "PC START"

                    sender.Text = "WAIT"
                    Application.DoEvents()
                    Try
                        Dim Tech() As Byte = {1, 1, 1, 128, 242, 0, 0, 0, 34, 7, 4, 34, 7, 4, 233, 239, 128, 0, 128, 0}
                        Me.E6000Port.Write(Tech, 0, 19)

                    Catch ex As Exception
                        PassapMsg.display(ex.Message, False, "")
                    End Try
                    sender.Text = "KNIT.TECH"
                    E6000Port.Close()

                Case Else
                    sender.Text = "KNIT.TECH"
            End Select
        Catch ex As Exception
            PassapMsg.display(ex.Message, False, "")
        End Try
    End Sub

    Private Function convertByteArrayToMotif(ByVal bytes As Byte())
        convertByteArrayToMotif = ""
        If Not IsNothing(bytes) Then
            'If bytes.Length Mod 5 <> 0 Then
            'ReDim Preserve bytes(Int(bytes.Length / 5) * 5 + 5)
            'End If
            For i As Integer = Int(bytes.Length / 5) - 1 To 0 Step -1
                For j As Integer = 0 To 4
                    For k As Integer = 7 To 0 Step -1
                        If bytes(i * 5 + j) And 2 ^ k Then
                            convertByteArrayToMotif = convertByteArrayToMotif & "1"
                        Else
                            convertByteArrayToMotif = convertByteArrayToMotif & "0"
                        End If
                    Next k

                Next j
            Next i
        End If

    End Function


    Private Sub RemovePieceStitchPatLink_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles RemovePieceStitchPatLink.LinkClicked
        PiecePatternBindingSource.RemoveCurrent()
        PiecePatternBindingSource.EndEdit()
        PiecePatternList.Refresh()
        updateConsoleWarning()
    End Sub

    Private Sub PieceList_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PieceList.DoubleClick
        If IsNothing(ProjectPieceBindingSource.Current) Then
            PassapMsg.display("Please add fabric piece before adding stitch pattern.", False, "")
        Else
            '            CurrentPiecePatternLabel.Text = ProjectPieceBindingSource.Current("Name")
            If PiecePatternBindingSource.Count = 0 Then
                addNewPiecePattern(PiecePatternBindingSource.Count)
            End If
            PiecePatternBindingSource.MoveFirst()
            PagePiecePattern.BringToFront()
        End If
    End Sub
    Private Sub LinkLabel7_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel7.LinkClicked
        Try
            System.Diagnostics.Process.Start(Application.StartupPath & "\JournalSixUserManual.pdf")
        Catch ex As Exception
            PassapMsg.display(ex.Message, False, "")
        End Try
    End Sub

    Private Sub MSPaintLink_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles MSPaintLink.LinkClicked
        System.Diagnostics.Process.Start("mspaint.exe")
    End Sub
    Private Sub CardDoc_PrintPage(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles CardDoc.PrintPage
        Dim motifpointer As Integer

        If sender.Motif <> "" Then
            Dim cardDepth As Integer
            Dim x, y, w, h As Integer
            Dim cardx, cardwidth, cardy, cardheight
            Dim i, j As Integer

            Dim numFont As New Font(FontFamily.GenericSansSerif, 8)

            cardx = 0 : cardy = 0 : cardwidth = Math.Min(40, sender.motifwidth) : cardheight = sender.Motifheight

            cardDepth = e.Graphics.VisibleClipBounds.Height
            e.Graphics.PageUnit = GraphicsUnit.Millimeter
            e.Graphics.PageScale = 0.1

            e.Graphics.DrawString(sender.DocumentName, Me.Font, Brushes.SaddleBrown, 195, 120)

            For i = 64 To 1 Step -1
                x = 85 : y = 2783 - i * 37.5 : w = 45 : h = 37.5 'registration column
                e.Graphics.DrawRectangle(Pens.Gray, x, y, w, h)
                If i Mod 2 = 1 Then 'odd row print number in registrtion column
                    e.Graphics.DrawString(Trim(Str(i)), numFont, Brushes.Black, x + 3, y + 3)
                End If

                x = 195 : y = 2608 - i * 37.5 : w = 30 : h = 37.5 'height column
                e.Graphics.DrawRectangle(Pens.Gray, x, y, w, h)
                If i <= cardheight + 1 And (Not sender.KnitTechnique) Then
                    'Changed from: If i <= cardheight + 1 And ((Not sender.KnitTechnique) Or sender.lastCard) Then
                    e.Graphics.FillRectangle(Brushes.Black, x, y, w, h)
                End If
                If i <= cardheight And sender.KnitTechnique Then
                    e.Graphics.FillRectangle(Brushes.Black, x, y, w, h)
                End If

                x = 225 : y = 2608 - i * 37.5 : w = 37.5 : h = 37.5 'Continuation Column
                e.Graphics.DrawRectangle(Pens.Gray, x, y, w, h)
                If i = cardheight + 1 And sender.Continuation And Not (sender.KnitTechnique) Then
                    e.Graphics.FillRectangle(Brushes.Black, x, y, w, h)
                End If
                If i = cardheight And sender.Continuation And sender.KnitTechnique Then
                    e.Graphics.FillRectangle(Brushes.Black, x, y, w, h)
                End If

                If i = 1 And sender.KnitTechnique And sender.firstCard Then

                    e.Graphics.FillRectangle(Brushes.Black, x, y, w, h)
                End If

                For j = 0 To 39
                    x = 307.5 + j * 37.5 : y = 2608 - i * 37.5 : w = 37.5 : h = 37.5 'data column
                    e.Graphics.DrawRectangle(Pens.Gray, x, y, w, h)
                    If j < cardwidth And i <= cardheight Then
                        If Mid(sender.Motif, motifpointer + 1, 1) = "1" Then
                            e.Graphics.FillRectangle(Brushes.Black, x, y, w, h)
                        End If
                        motifpointer = motifpointer + 1
                    End If

                Next j

                If i = cardheight + 1 And Not (sender.Continuation) And Not (sender.KnitTechnique) And sender.lastcard Then
                    'Pattern width marker on last card
                    x = 307.5 + (cardwidth - 1) * 37.5 : y = 2608 - i * 37.5 : w = 37.5 : h = 37.5 'data column
                    e.Graphics.FillRectangle(Brushes.Black, x, y, w, h)
                End If

                x = 1815 : y = 2608 - i * 37.5 : w = 30 : h = 37.5 'height column
                e.Graphics.DrawRectangle(Pens.Gray, x, y, w, h)
                If i <= cardheight + 1 And (Not sender.KnitTechnique) Then
                    'Changed from: If i <= cardheight + 1 And ((Not sender.KnitTechnique) Or sender.lastcard) Then
                    e.Graphics.FillRectangle(Brushes.Black, x, y, w, h)
                End If
                If i <= cardheight And sender.KnitTechnique Then
                    e.Graphics.FillRectangle(Brushes.Black, x, y, w, h)
                End If

                x = 1910 : y = 2783 - i * 37.5 : w = 45 : h = 37.5 'registration column
                e.Graphics.DrawRectangle(Pens.Gray, x, y, w, h)
                If i Mod 2 = 0 Then 'even row print number
                    e.Graphics.DrawString(Trim(Str(i)), numFont, Brushes.Black, x + 3, y + 3)
                End If

            Next i

            e.Graphics.DrawEllipse(Pens.Aqua, 164, 20, 33, 33) 'Circles
            e.Graphics.DrawEllipse(Pens.Aqua, 1842, 20, 33, 33)
            e.Graphics.DrawEllipse(Pens.Aqua, 164, 2684, 33, 40)

            'Reg marks

            e.Graphics.DrawLine(Pens.Aqua, 0, 404, 37, 404)
            e.Graphics.DrawLine(Pens.Aqua, 1975, 404, 2025, 404)
            e.Graphics.DrawLine(Pens.Aqua, 0, 2765, 37, 2765)
            e.Graphics.DrawLine(Pens.Aqua, 1975, 2765, 2025, 2765)

            e.Graphics.DrawLine(Pens.Aqua, 1030, 0, 1030, 100)
            e.Graphics.DrawLine(Pens.Aqua, 1030, 2709, 1030, 2825)

            e.Graphics.DrawRectangle(Pens.Aqua, 0, 0, 2025, 2825)

            numFont.Dispose()

        End If
    End Sub

    Private Sub OutputMethod_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OutputMethod.SelectedIndexChanged
        My.Settings.OutputMethod = sender.text
        Me.E6000Port.Close()
        Me.E6000Port.Open()
    End Sub

    Private Sub FormNotes_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FormNotes.Leave
        FormBindingSource.EndEdit()
        If Not IsNothing(FormBindingSource.Current) Then
            FormBindingSource.Current("FilterText") = FormBindingSource.Current("Name") & " " & FormBindingSource.Current("Notes")
        End If
        FormBindingSource.EndEdit()
    End Sub

    Private Sub StitchPatNotes_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StitchPatNotes.Leave
        PatternBindingSource.EndEdit()
        If Not IsNothing(PatternBindingSource.Current) Then
            PatternBindingSource.Current("FilterText") = PatternBindingSource.Current("Name") & " " & PatternBindingSource.Current("Notes")
        End If
        PatternBindingSource.EndEdit()
    End Sub

    Private Sub NotesTextBox_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NotesTextBox.Leave
        ProjectBindingSource.EndEdit()
        If Not IsNothing(ProjectBindingSource.Current) Then
            ProjectBindingSource.Current("FilterText") = ProjectBindingSource.Current("Name") & " " & ProjectBindingSource.Current("Notes")
        End If
        ProjectBindingSource.EndEdit()

    End Sub

    Private Sub TechLibNotes_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TechLibNotes.Leave
        TechniqueBindingSource.EndEdit()
        If Not IsNothing(TechniqueBindingSource.Current) Then
            TechniqueBindingSource.Current("FilterText") = TechniqueBindingSource.Current("Name") & " " & TechniqueBindingSource.Current("Notes")
        End If
        TechniqueBindingSource.EndEdit()
    End Sub

    Private Sub RightCheckBox_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RightCheckBox.CheckedChanged
        If RightCheckBox.Checked Then
            LeftCheckBox.Checked = False
        End If
    End Sub

    Private Sub LeftCheckBox_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LeftCheckBox.CheckedChanged
        If LeftCheckBox.Checked Then
            RightCheckBox.Checked = False
        End If
    End Sub


    Private Sub PieceFormLabel_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PieceFormLabel.TextChanged

    End Sub


    Private Sub SettingConsoleMemText_SelectedValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SettingConsoleMemText.SelectedValueChanged

        If My.Settings.ConsoleMax <> sender.Text Then
            My.Settings.ConsoleMax = sender.Text
            If sender.text = "47520" Then
                SettingPatMemText.Text = "45900"
            Else
                SettingPatMemText.Text = "45900"
            End If
        End If
    End Sub

    Private Sub SketchBookLink_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles SketchBookLink.LinkClicked
        If TargetStitchPatternBox.Motif = "" Then
            setColors(PurePalette, ColorCountComboBox.Text)
            initializePalette(PurePalette, PurePalette, False)
            If Val(Me.StitchesTextBox.Text) > 0 And Val(Me.RowsTextBox.Text) > 0 Then

                TargetStitchPatternBox.Color1 = Color1Button.BackColor.ToArgb
                TargetStitchPatternBox.Color2 = Color2Button.BackColor.ToArgb
                TargetStitchPatternBox.Color3 = Color3Button.BackColor.ToArgb
                TargetStitchPatternBox.Color4 = Color4Button.BackColor.ToArgb
                TargetStitchPatternBox.ColorCount = Me.ColorCountComboBox.Text
                TargetStitchPatternBox.Motif = IIf(ColorCountComboBox.Text = "2", "0", ColorCountComboBox.Text) & " " & Space(3 - Len(StitchesTextBox.Text)) & StitchesTextBox.Text & " " & Space(3 - Len(RowsTextBox.Text)) & RowsTextBox.Text & " " & StrDup(CInt(Val(StitchesTextBox.Text) * Val(RowsTextBox.Text)), "0")

                TargetStitchPatternBox.Refresh()
            End If
        End If
        If TargetStitchPatternBox.Motif <> "" Then
            Dim sketch As New SketchBook

            sketch.SketchBox1.ColorScheme.Colors = TargetStitchPatternBox.ColorScheme.Colors
            sketch.SketchBox1.ColorCount = TargetStitchPatternBox.ColorCount
            sketch.SketchBox1.Motif = TargetStitchPatternBox.Motif
            If Me.TemplateCheckBox.CheckState = CheckState.Checked Then
                sketch.SketchBox1.chartTemplate = chartToBitmap(FormProgramme.Text)
            End If
            sketch.StitchPatList.DataSource = PatternBindingSource
            sketch.StitchPatList.DataSource.Filter = ""
            Me.WindowState = FormWindowState.Minimized
            sketch.TopMost = True
            sketch.ShowDialog(Me)
            Me.WindowState = FormWindowState.Normal
            If sketch.UpdateStitchPat Then
                im = sketch.StitchPatBmap
                StitchesTextBox.Value = im.Width
                RowsTextBox.Value = im.Height
                generateStitchPattern(sketch.SketchBox1.ColorCount + 1, sketch.SketchBox1.ColorScheme.Color1, sketch.SketchBox1.ColorScheme.Color2, sketch.SketchBox1.ColorScheme.Color3, sketch.SketchBox1.ColorScheme.Color4)

            End If

        End If

    End Sub
    Private Sub ExportButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExportButton.Click
        Dim abitmap As Bitmap
#If Version = "Trial" Then
        abitmap = motifToBitmap("0  20   6 000000000000000000001110011101000100110010010100011011010010100101100101010100101001010001000101001011100111010001001100", Color.White, Color.Black, Color.Black, Color.Black)
#Else
        abitmap = motifToBitmap(StitchPatLibPictureBox.Motif, Color.FromArgb(StitchPatLibPictureBox.Color1), Color.FromArgb(StitchPatLibPictureBox.Color2), Color.FromArgb(StitchPatLibPictureBox.Color3), Color.FromArgb(StitchPatLibPictureBox.Color4))
#End If

        SaveFileDialog1.Filter = "Bitmap Image|*.bmp"
        SaveFileDialog1.Title = "Save Stitch Pattern as Bitmap"
        SaveFileDialog1.ShowDialog()
        If SaveFileDialog1.FileName <> "" Then
            Dim fs As System.IO.FileStream = CType(SaveFileDialog1.OpenFile(), System.IO.FileStream)
            abitmap.Save(fs, System.Drawing.Imaging.ImageFormat.Bmp)
            fs.Close()
        End If
        abitmap.Dispose()
    End Sub

    Private Sub SketchChartLink_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles SketchChartLink.LinkClicked

        Dim sketch As New SketchBook
        Dim i As Integer
        sketch.SketchBox1.ColorScheme.Color1 = Color.White.ToArgb
        sketch.SketchBox1.ColorScheme.Color2 = Color.Black.ToArgb
        sketch.SketchBox1.ColorScheme.Color3 = Color.Gray.ToArgb
        sketch.SketchBox1.ColorScheme.Color4 = Color.LightCoral.ToArgb
        sketch.SketchBox1.ColorCount = 4
        sketch.ChartMode = True 'Chart mode must be set prior to setting motif.
        sketch.SketchBox1.SketchMode = "Chart"
        sketch.SketchBox1.Motif = FormProgramme.Text
        i = FormProgramme.Text.IndexOf("RZ")
        If i > -1 Then
            sketch.Rowz.Value = Val(FormProgramme.Text.Substring(i + 2, 4))
        Else
            sketch.Rowz.Value = 2
        End If

        sketch.FontButton.Visible = False
        sketch.TextBox1.Visible = False
        sketch.TextButton.Visible = False
        sketch.SketchBookCloseLink.Text = "Use This Chart"
        sketch.SketchBox1.TileViewLink.Visible = False
        sketch.SketchBox1.LassoButton.Visible = False
        'sketch.DropperButton.Visible = False
        sketch.LayoutButton.Visible = False
        'sketch.stampButton.Visible = False
        sketch.SwatchRowsTextBox.Visible = True
        sketch.SwatchStitchesTextBox.Visible = True
        sketch.SwatchRowsLabel.Visible = True
        sketch.SwatchStitchesLabel.Visible = True
        sketch.TapeMeasureButton.Visible = True
        sketch.RowzLabel.Visible = True
        sketch.Rowz.Visible = True
        'sketch.SketchBox1.SketchPanel.Location = New Point(sketch.SketchBox1.SketchPanel.Location.X, 0)
        sketch.StitchPatList.DataSource = PatternBindingSource
        sketch.StitchPatList.DataSource.Filter = ""
        Me.WindowState = FormWindowState.Minimized
        sketch.TopMost = True
        sketch.ShowDialog(Me)
        Me.WindowState = FormWindowState.Normal
        If sketch.Chart <> "" Then
            FormProgramme.Text = sketch.Chart
            saveForm()
        End If

    End Sub

    Private Sub PCStartFormButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PCStartFormButton.Click
#If Version = "Trial" Then
        If sender.text = "PC START" Then
            PassapMsg.display("Demonstration version of software not capable of downloading charts.  Please upgrade by clicking the Buy Now Button.  The demonstration chart will be used instead.", False, "")
        End If
        DownloadDemoChart(sender)
#Else
        Try
            Dim i As Integer
            Dim j As Integer
            Dim k As Integer
            Dim FormProgram(5000) As Byte
            Dim instructions() As String
            Dim instructionLine() As String
            Dim progCounter As Integer
            Dim divide As Boolean
            Dim leftNeedle As Byte
            Dim rightNeedle As Byte
            Dim oldLeftNeedle As Byte
            Dim oldRightNeedle As Byte
            Dim command As Byte
            Dim rightHalf As Boolean
            Dim rowOffset As Integer
            Dim rowBeginRightHalf As Integer
            Dim startDivide As Boolean
            Dim rightNeedleIndex As Integer
            Dim leftNeedleIndex As Integer
            Dim rc As Integer
            'Dim rz As Integer
            Dim refText As String
            Dim refCount As Integer

            rightHalf = False
            rowOffset = 0
            startDivide = True

            Select Case sender.text
                Case "FORM"
                    If My.Settings.OutputMethod = "E6000" Then
                        sender.Text = "PC START"
                        E6000Port.Close()
                        E6000Port.Open()
                    ElseIf My.Settings.OutputMethod = "Printer" Then
                        sender.Text = "PRINT"
                    End If
                    Exit Select
                Case "PC START", "PRINT"
                    sender.Text = "WAIT"
                    Application.DoEvents()
                    divide = False
                    If Me.PieceFormLabel.Text.StartsWith("<chart>", StringComparison.CurrentCultureIgnoreCase) Then
                        FormProgram(0) = 55 'No neckline divide.
                        FormProgram(1) = 32
                        instructions = Split(Me.PieceFormLabel.Text, Chr(13) & Chr(10))
                        instructionLine = Split(instructions(1)) 'Line after "<chart>" line-- Cast on
                        FormProgram(2) = 96
                        FormProgram(3) = 0
                        FormProgram(4) = Val(instructionLine(2)) ' right needle
                        FormProgram(5) = Val(instructionLine(1)) ' left needle
                        leftNeedle = Val(instructionLine(1))
                        rightNeedle = Val(instructionLine(2))
                        leftNeedleIndex = 1
                        rightNeedleIndex = 2
                        progCounter = 5

                        For i = 2 To instructions.Length - 1 'Start with third line of instructions-- line after cast on
                            If instructions(i).StartsWith("RZ") Then
                                Exit For
                            End If
                            instructionLine = Split(instructions(i))
                            rc = Val(instructionLine(0).Substring(2, 4)) + rowOffset

                            If instructions(i).ToLower.Contains("<change pattern>") Then

                                FormProgram(progCounter + 1) = 128 + Int(rc / 256)
                                FormProgram(progCounter + 2) = rc Mod 256 'Val(instructionLine(0).Substring(2, 4)) + rowOffset
                                FormProgram(progCounter + 3) = 63
                                progCounter = progCounter + 3
                            ElseIf instructions(i).ToLower.Contains("<ref") Then
                                FormProgram(progCounter + 1) = 128 + Int(rc / 256)
                                FormProgram(progCounter + 2) = rc Mod 256 'Val(instructionLine(0).Substring(2, 4)) + rowOffset
                                refText = Split(Split(instructions(i), "<ref ")(1), ">")(0)
                                refCount = refText.Length
                                FormProgram(progCounter + 3) = 100 + Val(refText) ' + refCount '160
                                ' For k = 0 To refCount - 1
                                'FormProgram(progCounter + 4 + k) = Asc(refText.Substring(k, 1).ToUpper)
                                'Next k

                                'FormProgram(progCounter + 4) = Asc("J")
                                'FormProgram(progCounter + 5) = Asc("E")
                                'FormProgram(progCounter + 6) = Asc("N")
                                'FormProgram(progCounter + 7) = Asc("N")
                                'FormProgram(progCounter + 8) = Asc("Y")
                                progCounter = progCounter + 3 ' + refCount '+4 +(refCount-1)

                            Else

                                If instructionLine.Length > 4 Then
                                    If divide = False Then
                                        divide = True
                                        rightHalf = True
                                        rowBeginRightHalf = rc 'Val(instructionLine(0).Substring(2, 4))
                                        j = i
                                        FormProgram(0) = 56
                                    End If

                                    If startDivide Then
                                        If rightHalf Then
                                            oldLeftNeedle = leftNeedle
                                            oldRightNeedle = Val(instructionLine(2)) 'LeftNeck
                                            If rightNeedle <> Val(instructionLine(4)) Then 'If Shoulder shaping...
                                                FormProgram(progCounter + 1) = 224 + Int(rc / 256)
                                                FormProgram(progCounter + 2) = rc Mod 256 'Val(instructionLine(0).Substring(2, 4)) + rowOffset
                                                FormProgram(progCounter + 3) = Val(instructionLine(4)) 'right shoulder
                                                rightNeedle = Val(instructionLine(4))
                                                FormProgram(progCounter + 4) = Val(instructionLine(2)) 'Left Neck
                                                progCounter = progCounter + 4
                                            Else 'If no shoulder shaping
                                                FormProgram(progCounter + 1) = 192 + Int(rc / 256)
                                                FormProgram(progCounter + 2) = rc Mod 256 'Val(instructionLine(0).Substring(2, 4)) + rowOffset
                                                FormProgram(progCounter + 3) = Val(instructionLine(2)) 'Left Neck
                                                progCounter = progCounter + 3
                                            End If
                                            FormProgram(progCounter + 1) = 152
                                            progCounter = progCounter + 1
                                            leftNeedleIndex = 3
                                            rightNeedleIndex = 4

                                        Else 'If left half...
                                            FormProgram(progCounter + 1) = 128 + Int(rc / 256)
                                            FormProgram(progCounter + 2) = rc Mod 256 'Val(instructionLine(0).Substring(2, 4)) + rowOffset
                                            FormProgram(progCounter + 3) = 184
                                            progCounter = progCounter + 3
                                            leftNeedle = oldLeftNeedle
                                            rightNeedle = oldRightNeedle
                                            leftNeedleIndex = 1
                                            rightNeedleIndex = 2
                                        End If 'if right half

                                        FormProgram(progCounter + 1) = Asc("D")
                                        FormProgram(progCounter + 2) = Asc("I")
                                        FormProgram(progCounter + 3) = Asc("V")
                                        FormProgram(progCounter + 4) = Asc("I")
                                        FormProgram(progCounter + 5) = Asc("D")
                                        FormProgram(progCounter + 6) = Asc("E")
                                        FormProgram(progCounter + 7) = Asc(" ")
                                        FormProgram(progCounter + 8) = Asc(" ")

                                        ' If rightHalf Then
                                        'FormProgram(progCounter + 13) = 64 + Int(rc / 256)
                                        'FormProgram(progCounter + 14) = rc Mod 256 'Val(instructionLine(0).Substring(2, 4)) + rowOffset
                                        'FormProgram(progCounter + 15) = Val(instructionLine(3))
                                        'progCounter = progCounter + 15
                                        'Else
                                        progCounter = progCounter + 8
                                        'End If ' right half
                                        startDivide = False

                                    End If

                                End If 'if start divide

                                command = 0
                                If rightNeedle <> Val(instructionLine(rightNeedleIndex)) And leftNeedle <> Val(instructionLine(leftNeedleIndex)) Then
                                    command = 96
                                ElseIf rightNeedle <> Val(instructionLine(rightNeedleIndex)) Then
                                    command = 32
                                ElseIf leftNeedle <> Val(instructionLine(leftNeedleIndex)) Then
                                    command = 64
                                End If
                                If command <> 0 Then
                                    FormProgram(progCounter + 1) = command + Int(rc / 256)
                                    FormProgram(progCounter + 2) = rc Mod 256 'Val(instructionLine(0).Substring(2, 4)) + rowOffset
                                    progCounter = progCounter + 2

                                    If command = 96 Or command = 32 Then
                                        FormProgram(progCounter + 1) = Val(instructionLine(rightNeedleIndex))
                                        progCounter = progCounter + 1
                                        rightNeedle = Val(instructionLine(rightNeedleIndex))
                                        leftNeedle = Val(instructionLine(leftNeedleIndex))
                                    End If

                                    If command = 96 Or command = 64 Then
                                        FormProgram(progCounter + 1) = Val(instructionLine(leftNeedleIndex))
                                        progCounter = progCounter + 1
                                        rightNeedle = Val(instructionLine(rightNeedleIndex))
                                        leftNeedle = Val(instructionLine(leftNeedleIndex))
                                    End If

                                End If
                            End If
                            If instructions(i + 1).StartsWith("RT") Then
                                '  rc = rc + 2 'In case we need an "END 1" or "END"
                                If rightHalf = False Then
                                    instructionLine = Split(instructions(i))
                                    If divide Then
                                        FormProgram(progCounter + 1) = 128 + Int((rc + 0) / 256)
                                        FormProgram(progCounter + 2) = (rc + 0) Mod 256 'Val(instructions(i + 1).Substring(2, 4)) + rowOffset
                                    Else
                                        FormProgram(progCounter + 1) = 128 + Int((rc + 0) / 256)
                                        FormProgram(progCounter + 2) = (rc + 0) Mod 256 'Val(instructions(i + 1).Substring(2, 4)) + rowOffset
                                    End If

                                    FormProgram(progCounter + 3) = 5
                                    FormProgram(progCounter + 4) = 0
                                    FormProgram(progCounter + 5) = 0
                                    progCounter = progCounter + 5
                                    Exit For
                                Else
                                    instructionLine = Split(instructions(i))
                                    FormProgram(progCounter + 1) = 128 + Int(rc / 256)
                                    FormProgram(progCounter + 2) = rc Mod 256 'Val(instructions(i + 1).Substring(2, 4)) + rowOffset 'Val(instructionLine(i + 1).Substring(2, 4)) + rowOffset
                                    FormProgram(progCounter + 3) = 166
                                    FormProgram(progCounter + 4) = Asc("E")
                                    FormProgram(progCounter + 5) = Asc("N")
                                    FormProgram(progCounter + 6) = Asc("D")
                                    FormProgram(progCounter + 7) = Asc(" ")
                                    FormProgram(progCounter + 8) = Asc(" ")
                                    FormProgram(progCounter + 9) = Asc("1")


                                    progCounter = progCounter + 9
                                    i = j - 1 'Go back and do the left half; subtract one from j because next i will add one.
                                    rightHalf = False
                                    startDivide = True
                                    rowOffset = Val(instructionLine(0).Substring(2, 4)) - rowBeginRightHalf + 2

                                End If
                            End If
                        Next i

                        If My.Settings.OutputMethod = "E6000" Then
                            Try
                                Me.E6000Port.Write(FormProgram, 0, progCounter + 1)
                            Catch ex As Exception
                                PassapMsg.display(ex.Message, False, "")
                            End Try
                            E6000Port.Close()
                        ElseIf My.Settings.OutputMethod = "Printer" Then
                            Array.Resize(FormProgram, progCounter + 1)
                            Printchart(FormProgram, Me.PieceFormBindingSource.Current("Name"))
                        End If

                    Else
                        PassapMsg.display("Cannot download chart.  No chart associated with this pattern.", False, "")
                    End If

                    sender.Text = "FORM"

                Case Else
                    sender.Text = "FORM"
            End Select
        Catch ex As Exception
            PassapMsg.display(ex.Message, False, "")
        End Try
#End If

    End Sub

    Private Sub TestFormButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TestFormButton.Click
        DownloadDemoChart(sender)
    End Sub
    Private Sub DownloadDemoChart(ByVal sender As System.Object)
        Try

            Select Case sender.text
                Case "FORM"
                    sender.Text = "PC START"
                    E6000Port.Close()
                    E6000Port.Open()
                    Exit Select
                Case "PC START"

                    sender.Text = "WAIT"
                    Application.DoEvents()
                    Try
                        Dim chart() As Byte = {&H38, &H20, &H60, &H0, &HA2, &H14, &H20, &H64, &H9E, &H60, &H66, &H9C, &H18, &H60, &H68, &H9A, &H1A, &H60, &H6C, &H98, &H1C, &HE0, &H6E, &H96, &H51, &H98, &H44, &H49, &H56, &H49, &H44, &H45, &H20, &H20, &H40, &H6E, &H55, &H60, &H70, &H95, &H56, &H60, &H72, &H93, &H58, &H60, &H74, &H92, &H5B, &H60, &H76, &H90, &H5D, &H60, &H78, &H8F, &H60, &H60, &H7A, &H8D, &H62, &H60, &H7C, &H8C, &H64, &H40, &H7E, &H66, &H80, &H80, &HA6, &H45, &H4E, &H44, &H20, &H20, &H31, &H80, &H82, &HB8, &H44, &H49, &H56, &H49, &H44, &H45, &H20, &H20, &H40, &H82, &H1E, &H40, &H84, &H20, &H60, &H86, &H4E, &H22, &H60, &H88, &H4D, &H24, &H60, &H8A, &H4C, &H25, &H60, &H8C, &H4B, &H27, &H60, &H8E, &H49, &H28, &H60, &H90, &H48, &H2A, &H80, &H94, &H5, &H0, &H0}
                        Me.E6000Port.Write(chart, 0, 123)

                    Catch ex As Exception
                        PassapMsg.display(ex.Message, False, "")
                    End Try
                    sender.Text = "FORM"
                    E6000Port.Close()

                Case Else
                    sender.Text = "FORM"
            End Select
        Catch ex As Exception
            PassapMsg.display(ex.Message, False, "")
        End Try
    End Sub

    Private Sub CoverPictureBox_MouseDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles CoverPictureBox.MouseDoubleClick

        If fullscreenmode Then

            Me.Width = 800
            Me.Height = 600
            fullscreenmode = False
        Else
            Me.Location = My.Computer.Screen.WorkingArea.Location
            Me.Size = My.Computer.Screen.WorkingArea.Size
            fullscreenmode = True
        End If


    End Sub

    Private Sub BackupLink_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles BackupLink.LinkClicked
#If Version = "Trial" Then
    PassapMsg.display( "Database back up not available in trial version.",False,"")
#Else
        saveAll()
        Me.ProjectTableAdapter.Connection.Close()
        Me.FormTableAdapter.Connection.Close()
        Me.PieceTableAdapter.Connection.Close()
        Me.TechniqueTableAdapter.Connection.Close()
        Me.PatternTableAdapter.Connection.Close()
        Me.PiecePatternTableAdapter.Connection.Close()

        SaveFileDialog1.Filter = "Database File|*.sdf"
        SaveFileDialog1.Title = "Save back up copy of Jounal Six Database"
        SaveFileDialog1.ShowDialog()
        If SaveFileDialog1.FileName <> "" Then
            My.Computer.FileSystem.CopyFile(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & "\JournalSixV9\JournalSix.sdf", SaveFileDialog1.FileName, True)
        End If
#End If
    End Sub

    Private Sub RestoreLink_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles RestoreLink.LinkClicked
        Dim openFileDialog1 As New OpenFileDialog()
#If Version = "Trial" Then
    PassapMsg.display( "Database back up not available in trial version.",False,"")
#Else
        saveAll()
        Me.ProjectTableAdapter.Connection.Close()
        Me.FormTableAdapter.Connection.Close()
        Me.PieceTableAdapter.Connection.Close()
        Me.TechniqueTableAdapter.Connection.Close()
        Me.PatternTableAdapter.Connection.Close()
        Me.PiecePatternTableAdapter.Connection.Close()

        openFileDialog1.InitialDirectory = "My Documents"
        openFileDialog1.Filter = "Database File |*.sdf"
        openFileDialog1.FileName = ""
        openFileDialog1.ShowDialog()
        If openFileDialog1.FileName <> "" Then
            My.Computer.FileSystem.CopyFile(openFileDialog1.FileName, Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & "\JournalSixV9\JournalSix.sdf", True)
            PassapMsg.display("Database has been restored.  Journal Six will close.  Please restart.", False, "")
            DBRestore = True
            Application.Exit()
        End If
#End If
    End Sub

    Private Function chartToBitmap(ByVal aMotif As String) As Bitmap

        Dim i, j, k, l As Integer
        Dim aBitmap As Bitmap
        Dim g2 As Graphics
        Dim rowz As Integer
        Dim offset As Integer
        Dim rows() As String
        Dim rowcount, rc As Integer
        Dim needles(4) As Integer
        Dim Row() As String
        Dim RW() As String
        Dim instruction As String
        Dim maxX As Integer

        If aMotif.StartsWith("<chart>") Then
            i = aMotif.IndexOf("RZ")
            If i > -1 Then
                rowz = Val(aMotif.Substring(i + 2, 4))
            Else
                rowz = 2
            End If
            i = aMotif.IndexOf("RT")
            If i > -1 Then
                rowcount = Val(aMotif.Substring(i + 2, 4)) / rowz
            Else
                chartToBitmap = Nothing
                Exit Function
            End If

            rows = aMotif.Replace(Chr(10), "").Split(Chr(13))
            RW = aMotif.Replace(Chr(10), "").Replace(Chr(13), " ").Split(" ")
            maxX = 0
            For i = 0 To RW.Length - 1
                If IsNumeric(RW(i)) Then
                    If Val(RW(i)) > maxX Then
                        maxX = Val(RW(i))
                    End If
                End If
            Next
            j = 1
            i = 0
            Row = rows(j).Split(" ")
            offset = Row(1)

            aBitmap = New Bitmap(CInt(Val(maxX + 2)), rowcount + 1)
            g2 = Graphics.FromImage(aBitmap)
            g2.Clear(Color.FromArgb(0, 0, 0, 0))
            g2.Dispose()

            If rows(j).Contains("change pattern") Then
                instruction = "change"
            Else
                instruction = "black"
                For k = 1 To Row.Length - 1
                    If IsNumeric(Row(k)) Then
                        needles(k - 1) = CInt(Row(k))
                    End If
                Next k
            End If

            Do Until i > rowcount
                Select Case instruction
                    Case "black"
                        For l = needles(0) To needles(1)
                            aBitmap.SetPixel(l - 1, rowcount - i, Color.FromArgb(64, 64, 128, 64))
                        Next l
                        If needles(2) <> 0 Or needles(3) <> 0 Then
                            For l = needles(2) To needles(3)
                                aBitmap.SetPixel(l - 1, rowcount - i, Color.FromArgb(64, 64, 128, 64))
                            Next l
                        End If
                    Case "change"
                        For l = needles(0) To needles(1)
                            aBitmap.SetPixel(l - 1, rowcount - i, Color.FromArgb(64, 128, 255, 128))
                        Next l
                        instruction = "black"
                End Select


                i = i + 1
                rc = rows(j + 1).Substring(2, 4) / rowz
                If rc = i Then
                    j = j + 1
                    Row = rows(j).Split(" ")
                    If rows(j).Contains("change pattern") Then
                        instruction = "change"
                    Else
                        instruction = "black"
                        For k = 1 To Row.Length - 1
                            If IsNumeric(Row(k)) Then
                                needles(k - 1) = CInt(Row(k))
                            End If
                        Next k
                    End If
                End If
            Loop
        End If

        chartToBitmap = aBitmap


    End Function

    Private Sub FormList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FormList.SelectedIndexChanged
        Me.TemplateCheckBox.CheckState = CheckState.Unchecked
    End Sub

    Private Sub updateConsoleWarning()
        Dim motif As String
        Dim colorCount As Integer
        Dim colorMultiplier As Integer
        Dim TotalPatterns As Integer
        Dim pieceID As Guid
        Dim pr As JournalSixDataSet.PieceRow
        Dim ppr() As JournalSixDataSet.PiecePatternRow

        TotalPatterns = 0
        If Not IsNothing(ProjectPieceBindingSource.Current) Then
            If Not IsDBNull(ProjectPieceBindingSource.Current("ID")) Then
                pieceID = ProjectPieceBindingSource.Current("Id")
                pr = JournalSixDataSet1.Piece.FindById(pieceID) 'Current piece row
                ppr = pr.GetChildRows("FK_Piece_PiecePattern") 'Current piecepattern rows
                For Each r1 As JournalSixDataSet.PiecePatternRow In ppr
                    For Each r2 As JournalSixDataSet.PatternRow In r1.GetChildRows("PiecePattern_Pattern")
                        If Not IsDBNull(r2.Motif) Then
                            motif = Mid(r2.Motif, 11)
                            colorCount = Val(Mid(r2.Motif, 1, 1))
                            If colorCount > 2 Then
                                colorMultiplier = 2
                            Else
                                colorMultiplier = 1
                                colorCount = 0
                            End If
                            TotalPatterns = TotalPatterns + Len(motif) * colorMultiplier
                        End If
                    Next
                Next
            End If
        End If
        If TotalPatterns > My.Settings.ConsoleMax Then
            WarningLabel.Visible = True
        Else
            WarningLabel.Visible = False
        End If

    End Sub

    Private Sub SettingConsoleMemText_ValueMemberChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SettingConsoleMemText.ValueMemberChanged
        My.Settings.ConsoleMax = sender.text()
        updateConsoleWarning()
    End Sub

    Private Sub SettingConsoleMemText_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SettingConsoleMemText.TextChanged
        My.Settings.ConsoleMax = sender.text()
        updateConsoleWarning()
    End Sub

    Private Sub ProjectPieceBindingSource_CurrentChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ProjectPieceBindingSource.CurrentChanged
        updateConsoleWarning()
    End Sub

    Private Sub ShareLink_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles ShareLink.LinkClicked
        Try
            If Not IsDBNull(ProjectBindingSource.Current()) Then
                Dim saveFileDialog1 As New SaveFileDialog()

                saveFileDialog1.InitialDirectory = "My Documents"
                saveFileDialog1.Filter = "*.xml|*.xml|All files (*.*)|*.*"
                saveFileDialog1.FilterIndex = 0

                If saveFileDialog1.ShowDialog() = System.Windows.Forms.DialogResult.OK Then

                    Dim writer As IO.StreamWriter = New IO.StreamWriter(saveFileDialog1.FileName)
                    Dim projectID As Guid
                    Dim pr As JournalSixDataSet.PieceRow
                    Dim ppr As JournalSixDataSet.PiecePatternRow
                    Dim sp As JournalSixDataSet.PatternRow
                    Dim f As JournalSixDataSet.FormRow
                    Dim t As JournalSixDataSet.TechniqueRow
                    Dim pj As System.Data.DataRowView

                    writer.WriteLine("<?xml version='1.0' standalone='yes'?>")
                    writer.WriteLine("<JournalSixDataSet xmlns='http://passappal.com/JournalSixDataSet.xsd'>")
                    writer.WriteLine("<Project>")

                    pj = ProjectBindingSource.Current()
                    projectID = pj("Id")

                    writer.WriteLine("<Name><![CDATA[" & pj("Name") & "]]></Name>")
                    writer.WriteLine("<Notes><![CDATA[" & pj("Notes") & "]]></Notes>")
                    writer.WriteLine("<ImageFile><![CDATA[" & pj("ImageFile") & "]]></ImageFile>")
                    writer.WriteLine("<Id>" & pj("Id").ToString & "</Id>")
                    writer.WriteLine("<FilterText><![CDATA[" & pj("FilterText") & "]]></FilterText>")
                    If Not IsNothing(JournalSixDataSet1.Project.FindById(ProjectBindingSource.Current("Id")).GetChildRows("FK_Project_Piece")) Then
                        For Each pr In JournalSixDataSet1.Project.FindById(ProjectBindingSource.Current("Id")).GetChildRows("FK_Project_Piece")

                            writer.WriteLine("<Piece>")

                            writer.WriteLine("<Name><![CDATA[" & pr("Name") & "]]></Name>")
                            writer.WriteLine("<Notes><![CDATA[" & pr("Notes") & "]]></Notes>")
                            writer.WriteLine("<SwatchWidth>" & pr("SwatchWidth") & "</SwatchWidth>")
                            writer.WriteLine("<SwatchLength>" & pr("SwatchLength") & "</SwatchLength>")
                            writer.WriteLine("<SwatchStitches>" & pr("SwatchStitches") & "</SwatchStitches>")
                            writer.WriteLine("<SwatchRows>" & pr("SwatchRows") & "</SwatchRows>")
                            writer.WriteLine("<ProjectId>" & pr("ProjectId").ToString & "</ProjectId>")
                            writer.WriteLine("<FormId>" & pr("FormId").ToString & "</FormId>")
                            writer.WriteLine("<Id>" & pr("Id").ToString & "</Id>")
                            If Not IsNothing(pr.GetChildRows("FK_Piece_PiecePattern")) Then

                                For Each ppr In pr.GetChildRows("FK_Piece_PiecePattern")
                                    writer.WriteLine("<PiecePattern>")
                                    writer.WriteLine("<Name><![CDATA[" & ppr("Name") & "]]></Name>")
                                    writer.WriteLine("<PieceId>" & ppr("PieceId").ToString & "</PieceId>")
                                    writer.WriteLine("<PatternId>" & ppr("PatternId").ToString & "</PatternId>")
                                    writer.WriteLine("<TechniqueId>" & ppr("TechniqueId").ToString & "</TechniqueId>")
                                    writer.WriteLine("<Id>" & ppr("Id").ToString & "</Id>")
                                    ' writer.WriteLine("<Motif><![CDATA[" & ppr("Motif").ToString & "]]></Motif>")
                                    writer.WriteLine("</PiecePattern>")
                                    If Not IsNothing(JournalSixDataSet1.PiecePattern.FindById(ppr("Id")).GetChildRows("PiecePattern_Pattern")) Then
                                        For Each sp In JournalSixDataSet1.PiecePattern.FindById(ppr("Id")).GetChildRows("PiecePattern_Pattern")
                                            writer.WriteLine("<Pattern>")
                                            writer.WriteLine("<Name><![CDATA[" & sp("Name") & "]]></Name>")
                                            writer.WriteLine("<BuiltIn>" & sp("BuiltIn") & "</BuiltIn>")
                                            writer.WriteLine("<Motif><![CDATA[" & sp("Motif") & "]]></Motif>")
                                            writer.WriteLine("<Color1>" & sp("Color1") & "</Color1>")
                                            writer.WriteLine("<Color2>" & sp("Color2") & "</Color2>")
                                            writer.WriteLine("<Color3>" & sp("Color3") & "</Color3>")
                                            writer.WriteLine("<Color4>" & sp("Color4") & "</Color4>")
                                            writer.WriteLine("<ColorCount>" & sp("ColorCount") & "</ColorCount>")
                                            writer.WriteLine("<Notes><![CDATA[" & sp("Notes") & "]]></Notes>")
                                            writer.WriteLine("<Id>" & sp("Id").ToString & "</Id>")
                                            writer.WriteLine("<FilterText><![CDATA[" & sp("FilterText") & "]]></FilterText>")
                                            writer.WriteLine("</Pattern>")
                                        Next
                                    End If
                                    If Not IsNothing(JournalSixDataSet1.PiecePattern.FindById(ppr("Id")).GetChildRows("PiecePattern_Technique")) Then
                                        For Each t In JournalSixDataSet1.PiecePattern.FindById(ppr("Id")).GetChildRows("PiecePattern_Technique")
                                            writer.WriteLine("<Technique>")
                                            writer.WriteLine("<Name><![CDATA[" & t("Name") & "]]></Name>")
                                            writer.WriteLine("<Notes><![CDATA[" & t("Notes") & "]]></Notes>")
                                            writer.WriteLine("<Custom>" & t("Custom") & "</Custom>")
                                            If Not IsDBNull(t("Program")) Then
                                                writer.WriteLine("<Program>" & Convert.ToBase64String(t("Program")) & "</Program>")
                                            End If
                                            writer.WriteLine("<BackNeedles>" & t("BackNeedles") & "</BackNeedles>")
                                            writer.WriteLine("<BackPushers>" & t("BackPushers") & "</BackPushers>")
                                            writer.WriteLine("<FrontNeedles>" & t("FrontNeedles") & "</FrontNeedles>")
                                            writer.WriteLine("<FrontPushers>" & t("FrontPushers") & "</FrontPushers>")
                                            writer.WriteLine("<Rows>" & t("Rows") & "</Rows>")
                                            writer.WriteLine("<Racking>" & t("Racking") & "</Racking>")
                                            writer.WriteLine("<Feeder1>" & t("Feeder1") & "</Feeder1>")
                                            writer.WriteLine("<Feeder2>" & t("Feeder2") & "</Feeder2>")
                                            writer.WriteLine("<Feeder3>" & t("Feeder3") & "</Feeder3>")
                                            writer.WriteLine("<Feeder4>" & t("Feeder4") & "</Feeder4>")
                                            writer.WriteLine("<CastOn>" & t("CastOn") & "</CastOn>")
                                            writer.WriteLine("<CarriagePassesPerChartRow>" & t("CarriagePassesPerChartRow") & "</CarriagePassesPerChartRow>")
                                            writer.WriteLine("<Id>" & t("Id").ToString & "</Id>")
                                            writer.WriteLine("<FilterText><![CDATA[" & t("FilterText") & "]]></FilterText>")
                                            writer.WriteLine("</Technique>")
                                        Next
                                    End If
                                    If Not IsNothing(pr.GetChildRows("Piece_Form")) Then
                                        For Each f In pr.GetChildRows("Piece_Form")
                                            writer.WriteLine("<Form>")
                                            writer.WriteLine("<Name><![CDATA[" & f("Name") & "]]></Name>")
                                            writer.WriteLine("<Notes><![CDATA[" & f("Notes") & "]]></Notes>")
                                            writer.WriteLine("<Programme><![CDATA[" & f("Programme") & "]]></Programme>")
                                            writer.WriteLine("<Id>" & f("Id").ToString & "</Id>")
                                            writer.WriteLine("<FilterText><![CDATA[" & f("FilterText") & "]]></FilterText>")
                                            writer.WriteLine("</Form>")
                                        Next
                                    End If
                                Next
                            End If

                            writer.WriteLine("</Piece>")
                        Next

                    End If

                    writer.WriteLine("</Project>")
                    writer.WriteLine("</JournalSixDataSet>")
                    writer.Close()
                Else
                    PassapMsg.display("Please select a project to export.", False, "")
                    Exit Sub
                End If

                PassapMsg.display("Your project has been exported.", False, "")
            End If
        Catch ex As Exception
            PassapMsg.display(ex.Message, False, "")
            Exit Try
        End Try

    End Sub
    Private Sub ImportLink_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles ImportLink.LinkClicked

        Dim openFileDialog1 As New OpenFileDialog()

        openFileDialog1.InitialDirectory = "My Documents"
        openFileDialog1.Filter = "*.xml|*.xml|All files (*.*)|*.*"
        openFileDialog1.FilterIndex = 0
        Try
            If openFileDialog1.ShowDialog() = System.Windows.Forms.DialogResult.OK Then

                Dim p As XmlDocument
                Dim nl As XmlNodeList
                Dim n As XmlNode

                Dim newRow As System.Data.DataRowView



                p = New XmlDocument()
                p.Load(openFileDialog1.FileName)
                Dim m As New XmlNamespaceManager(p.NameTable)
                m.AddNamespace("j", "http://passappal.com/JournalSixDataSet.xsd")



                nl = p.SelectNodes("//j:Pattern", m)

                For Each n In nl

                    If PatternBindingSource.Find("Id", New Guid(n.SelectSingleNode("j:Id", m).InnerText)) = -1 Then
                        newRow = PatternBindingSource.AddNew()
                        newRow("Id") = New Guid(n.SelectSingleNode("j:Id", m).InnerText)
                        newRow("Name") = n.SelectSingleNode("j:Name", m).InnerText
                        newRow("Notes") = n.SelectSingleNode("j:Notes", m).InnerText
                        If n.SelectSingleNode("j:BuiltIn", m).InnerText <> "" Then
                            newRow("BuiltIn") = Convert.ToBoolean((n.SelectSingleNode("j:BuiltIn", m).InnerText))
                        End If
                        newRow("Motif") = n.SelectSingleNode("j:Motif", m).InnerText
                        If n.SelectSingleNode("j:Color1", m).InnerText <> "" Then
                            newRow("Color1") = Convert.ToInt32(n.SelectSingleNode("j:Color1", m).InnerText)
                        End If
                        If n.SelectSingleNode("j:Color2", m).InnerText <> "" Then
                            newRow("Color2") = Convert.ToInt32(n.SelectSingleNode("j:Color2", m).InnerText)
                        End If
                        If n.SelectSingleNode("j:Color3", m).InnerText <> "" Then
                            newRow("Color3") = Convert.ToInt32(n.SelectSingleNode("j:Color3", m).InnerText)
                        End If
                        If n.SelectSingleNode("j:Color4", m).InnerText <> "" Then
                            newRow("Color4") = Convert.ToInt32(n.SelectSingleNode("j:Color4", m).InnerText)
                        End If
                        If n.SelectSingleNode("j:ColorCount", m).InnerText <> "" Then
                            newRow("ColorCount") = Convert.ToInt32(n.SelectSingleNode("j:ColorCount", m).InnerText)
                        End If
                        newRow("FilterText") = n.SelectSingleNode("j:FilterText", m).InnerText
                        savePattern()
                    End If
                Next

                nl = p.SelectNodes("//j:Technique", m)

                For Each n In nl

                    If TechniqueBindingSource.Find("Id", New Guid(n.SelectSingleNode("j:Id", m).InnerText)) = -1 Then
                        newRow = TechniqueBindingSource.AddNew()

                        newRow("Id") = New Guid(n.SelectSingleNode("j:Id", m).InnerText)
                        newRow("Name") = n.SelectSingleNode("j:Name", m).InnerText
                        newRow("Notes") = n.SelectSingleNode("j:Notes", m).InnerText
                        If n.SelectSingleNode("j:Custom", m).InnerText <> "" Then
                            newRow("Custom") = Convert.ToBoolean((n.SelectSingleNode("j:Custom", m).InnerText))
                        End If
                        If n.SelectSingleNode("j:Program", m).InnerText <> "" Then
                            newRow("Program") = Convert.FromBase64String(n.SelectSingleNode("j:Program", m).InnerText)
                        End If
                        newRow("BackNeedles") = n.SelectSingleNode("j:BackNeedles", m).InnerText
                        newRow("BackPushers") = n.SelectSingleNode("j:BackPushers", m).InnerText
                        newRow("FrontNeedles") = n.SelectSingleNode("j:FrontNeedles", m).InnerText
                        newRow("FrontPushers") = n.SelectSingleNode("j:FrontPushers", m).InnerText
                        If n.SelectSingleNode("j:Rows", m).InnerText <> "" Then
                            newRow("Rows") = Convert.ToInt32(n.SelectSingleNode("j:Rows", m).InnerText)
                        End If
                        If n.SelectSingleNode("j:Racking", m).InnerText <> "" Then
                            newRow("Racking") = Convert.ToInt32(n.SelectSingleNode("j:Racking", m).InnerText)
                        End If
                        If n.SelectSingleNode("j:Feeder1", m).InnerText <> "" Then
                            newRow("Feeder1") = Convert.ToInt32(n.SelectSingleNode("j:Feeder1", m).InnerText)
                        End If
                        If n.SelectSingleNode("j:Feeder2", m).InnerText <> "" Then
                            newRow("Feeder2") = Convert.ToInt32(n.SelectSingleNode("j:Feeder2", m).InnerText)
                        End If
                        If n.SelectSingleNode("j:Feeder3", m).InnerText <> "" Then
                            newRow("Feeder3") = Convert.ToInt32(n.SelectSingleNode("j:Feeder3", m).InnerText)
                        End If
                        If n.SelectSingleNode("j:Feeder4", m).InnerText <> "" Then
                            newRow("Feeder4") = Convert.ToInt32(n.SelectSingleNode("j:Feeder4", m).InnerText)
                        End If
                        If n.SelectSingleNode("j:CastOn", m).InnerText <> "" Then
                            newRow("CastOn") = Convert.ToBoolean((n.SelectSingleNode("j:CastOn", m).InnerText))
                        End If
                        If n.SelectSingleNode("j:CarriagePassesPerChartRow", m).InnerText <> "" Then
                            newRow("CarriagePassesPerChartRow") = Convert.ToInt32(n.SelectSingleNode("j:CarriagePassesPerChartRow", m).InnerText)
                        End If
                        newRow("FilterText") = n.SelectSingleNode("j:Filter", m).InnerText

                        saveTechnique()
                    End If
                Next

                nl = p.SelectNodes("//j:Form", m)

                For Each n In nl
                    If FormBindingSource.Find("Id", New Guid(n.SelectSingleNode("j:Id", m).InnerText)) = -1 Then
                        newRow = FormBindingSource.AddNew()
                        newRow("Id") = New Guid(n.SelectSingleNode("j:Id", m).InnerText)
                        newRow("Name") = n.SelectSingleNode("j:Name", m).InnerText
                        newRow("Notes") = n.SelectSingleNode("j:Notes", m).InnerText
                        newRow("Programme") = n.SelectSingleNode("j:Programme", m).InnerText
                        newRow("FilterText") = n.SelectSingleNode("j:FilterText", m).InnerText

                        saveForm()
                    End If
                Next

                nl = p.SelectNodes("//j:Project", m)

                For Each n In nl
                    If ProjectBindingSource.Find("Id", New Guid(n.SelectSingleNode("j:Id", m).InnerText)) = -1 Then
                        newRow = ProjectBindingSource.AddNew()
                        newRow("Id") = New Guid(n.SelectSingleNode("j:Id", m).InnerText)
                        newRow("Name") = n.SelectSingleNode("j:Name", m).InnerText
                        newRow("Notes") = n.SelectSingleNode("j:Notes", m).InnerText
                        newRow("ImageFile") = n.SelectSingleNode("j:ImageFile", m).InnerText
                        newRow("FilterText") = n.SelectSingleNode("j:FilterText", m).InnerText

                        saveProject()

                        ProjectFilterLink.Text = "Projects: All"
                        ProjectBindingSource.RemoveFilter()
                        ProjectBindingSource.Position = ProjectBindingSource.Find("Id", newRow("Id"))
                    End If
                Next

                nl = p.SelectNodes("//j:Piece", m)

                For Each n In nl
                    If PieceBindingSource.Find("Id", New Guid(n.SelectSingleNode("j:Id", m).InnerText)) = -1 Then
                        newRow = PieceBindingSource.AddNew()

                        newRow("Id") = New Guid(n.SelectSingleNode("j:Id", m).InnerText)
                        newRow("Name") = n.SelectSingleNode("j:Name", m).InnerText
                        newRow("Notes") = n.SelectSingleNode("j:Notes", m).InnerText
                        If n.SelectSingleNode("j:SwatchWidth", m).InnerText <> "" Then
                            newRow("SwatchWidth") = Convert.ToDouble(n.SelectSingleNode("j:SwatchWidth", m).InnerText)
                        End If
                        If n.SelectSingleNode("j:SwatchLength", m).InnerText <> "" Then
                            newRow("SwatchLength") = Convert.ToDouble(n.SelectSingleNode("j:SwatchLength", m).InnerText)
                        End If
                        If n.SelectSingleNode("j:SwatchStitches", m).InnerText <> "" Then
                            newRow("SwatchStitches") = Convert.ToInt32(n.SelectSingleNode("j:SwatchStitches", m).InnerText)
                        End If
                        If n.SelectSingleNode("j:SwatchRows", m).InnerText <> "" Then
                            newRow("SwatchRows") = Convert.ToInt32(n.SelectSingleNode("j:SwatchRows", m).InnerText)
                        End If
                        newRow("ProjectId") = New Guid(n.SelectSingleNode("j:ProjectId", m).InnerText)
                        If n.SelectSingleNode("j:FormId", m).InnerText <> "" Then
                            newRow("FormId") = New Guid(n.SelectSingleNode("j:FormId", m).InnerText)
                        End If

                        Me.PieceBindingSource.EndEdit()
#If Version = "Full" Then
                        Try

                            Me.PieceTableAdapter.Update(Me.JournalSixDataSet1.Piece)
                        Catch ex As Exception
                            PassapMsg.display(ex.Message, False, "")
                        End Try
#Else
#End If
                    End If
                Next

                nl = p.SelectNodes("//j:PiecePattern", m)

                For Each n In nl
                    If PPImportBindingSource.Find("Id", New Guid(n.SelectSingleNode("j:Id", m).InnerText)) Then
                        newRow = PPImportBindingSource.AddNew()
                        newRow("Id") = New Guid(n.SelectSingleNode("j:Id", m).InnerText)
                        newRow("Name") = n.SelectSingleNode("j:Name", m).InnerText
                        'newRow("Motif") = n.SelectSingleNode("j:Motif", m).InnerText
                        newRow("PieceId") = New Guid(n.SelectSingleNode("j:PieceId", m).InnerText)
                        If n.SelectSingleNode("j:PatternId", m).InnerText <> "" Then
                            newRow("PatternId") = New Guid(n.SelectSingleNode("j:PatternId", m).InnerText)
                        End If

                        If n.SelectSingleNode("j:TechniqueId", m).InnerText <> "" Then
                            newRow("TechniqueId") = New Guid(n.SelectSingleNode("j:TechniqueId", m).InnerText)
                        End If

                        Me.PPImportBindingSource.EndEdit()
#If Version = "Full" Then
                        Try

                            Me.PiecePatternTableAdapter.Update(Me.JournalSixDataSet1.PiecePattern)
                        Catch ex As Exception
                            PassapMsg.display(ex.Message, False, "")
                        End Try
#Else
#End If
                    End If
                Next

            End If
        Catch ex As Exception
            PassapMsg.display(ex.Message, False, "")
            Exit Try
        End Try
    End Sub

    Private Sub AutoColorButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AutoColorButton.Click
        Me.Cursor = Cursors.WaitCursor
        AutoColor(Color1Button.BackColor, Color2Button.BackColor, Color3Button.BackColor, Color4Button.BackColor, ColorCountComboBox.Text, bmap)
        Me.Cursor = Cursors.Default

    End Sub


    Private Sub TutorialLink_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles TutorialLink.LinkClicked
        System.Diagnostics.Process.Start(My.Settings.Website & "/tutorials")
    End Sub

 
End Class
