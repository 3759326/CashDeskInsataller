Imports Ionic.Zip

Public Class frmInstall
    Dim cashDeskNum
    Dim terminlPort
    Dim imgPath
    Dim destPath As String
    Dim paramINI As New Full_INI_Class
    Dim iniSections
    Dim iniPath = Application.StartupPath & "\settings.ini"
    Dim iniParameters()
    Dim installDir = Application.StartupPath & "\Support"


    Private Sub frmInstall_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        cashDeskNum = NumericUpDown1.Value
        terminlPort = NumericUpDown2.Value
    End Sub
    Public Sub Logining(eventName As String, eventStatus As String)
        Dim listit As New ListViewItem(eventName)
        listit.UseItemStyleForSubItems = False
        listit.SubItems.Add(eventStatus)
        Try
            Select Case eventStatus
                Case "Успех"
                    listit.SubItems(1).ForeColor = Color.Green
                Case "Внимание"

                    listit.SubItems(1).ForeColor = Color.Orange
                Case "Нетого ("
                    listit.SubItems(1).ForeColor = Color.Red
            End Select

        Catch
            listit.SubItems(1).ForeColor = Color.Black

        End Try





        ListView1.Items.Add(listit)
        ListView1.EnsureVisible(ListView1.Items.Count - 1)
    End Sub

    Private Sub frmInstall_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        frmStart.Show()

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        'OpenFileDialog1.FileName = archName
        OpenFileDialog1.Filter = "zip files (*.zip)|"
        OpenFileDialog1.InitialDirectory = Application.StartupPath & "\Archive"
        OpenFileDialog1.CheckPathExists = True
        OpenFileDialog1.ShowReadOnly = False
        OpenFileDialog1.ReadOnlyChecked = True
        OpenFileDialog1.CheckFileExists = False
        OpenFileDialog1.ValidateNames = False
        OpenFileDialog1.ShowDialog()
        imgPath = OpenFileDialog1.FileName
        'txtDest.Text = pathDest
        TextBox2.Text = imgPath
        MsgBox(imgPath)


    End Sub

    Private Sub TextBox2_ModifiedChanged(sender As Object, e As EventArgs) Handles TextBox2.ModifiedChanged

    End Sub

    Private Sub TextBox2_TextChanged(sender As Object, e As EventArgs) Handles TextBox2.TextChanged
        imgPath = TextBox2.Text
        MsgBox(imgPath)
    End Sub

    Private Sub NumericUpDown1_ValueChanged(sender As Object, e As EventArgs) Handles NumericUpDown1.ValueChanged
        cashDeskNum = NumericUpDown1.Value
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        FolderBrowserDialog1.SelectedPath = "c:\"
        FolderBrowserDialog1.ShowDialog()
        destPath = FolderBrowserDialog1.SelectedPath
        'If destPath.Substring(destPath.Length - 1) <> "\" Then
        '    destPath = destPath & "\"
        'Else
        'End If
        TextBox1.Text = destPath
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged

        destPath = TextBox1.Text

    End Sub
    Private Sub unZip(sources As String, dest As String)
        Dim options As New ReadOptions
        Logining("Распаковка архива начата", "Успех")
        options.Encoding = System.Text.Encoding.GetEncoding("CP866")
        Using zip1 As ZipFile = ZipFile.Read(sources, options)


            AddHandler zip1.ExtractProgress, AddressOf UnZipProgress
            zip1.ExtractAll(dest, ExtractExistingFileAction.OverwriteSilently)

        End Using
    End Sub
    Private Sub UnZipProgress(ByVal sender As Object, ByVal e As ExtractProgressEventArgs)
        Console.WriteLine(e.EventType)
        Select Case e.EventType
            Case ZipProgressEventType.Extracting_EntryBytesWritten
                'Console.WriteLine(e.EventType)


                'Console.WriteLine(e.BytesTransferred)

            Case ZipProgressEventType.Extracting_AfterExtractEntry

                Me.Invoke(Sub()
                              Label2.Text = "Распаковка архива: " & e.CurrentEntry.FileName
                              ProgressBar1.Maximum = e.EntriesTotal + 30
                              Label1.Text = Math.Round((e.EntriesExtracted * 100 / e.EntriesTotal)) - 30 & " %"
                              ProgressBar1.Value = e.EntriesExtracted




                          End Sub)
                Console.WriteLine(e.CurrentEntry)
                Console.WriteLine("Extracted: " & Math.Round((e.EntriesExtracted * 100 / e.EntriesTotal)) & " %")
            Case ZipProgressEventType.Extracting_AfterExtractAll

                Me.Invoke(Sub()
                              Label2.Text = "Распаковка завершена"
                              Logining("Распаковка завершена", "Успех")
                          End Sub)
        End Select


    End Sub

    Private Sub BackgroundWorker1_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork
        unZip(imgPath, destPath)
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        ProgressBar1.Value = 0
        'If TextBox1.Text = "" Then
        '    TextBox1.BackColor = Color.LightCoral
        '    MsgBox("Не задан путь для распаковки")
        '    Exit Sub

        'ElseIf TextBox2.Text = "" Then
        '    TextBox2.BackColor = Color.LightCoral
        '    MsgBox("Не задан путь к архиву")
        '    Exit Sub

        'ElseIf NumericUpDown1.Value = "0" Then
        '    NumericUpDown1.BackColor = Color.LightCoral
        '    MsgBox("Не задан номер кассы")
        '    Exit Sub
        'ElseIf NumericUpDown2.Value = "0" Then
        '    NumericUpDown2.BackColor = Color.LightCoral
        '    MsgBox("Не задан номер СOMпорта терминала")
        '    Exit Sub
        'End If

        Logining("Номер кассы установлен: " & cashDeskNum, "")
        Logining("СОМпорт для терминала Private установлен: " & terminlPort, "")

        ' Начинаем расспаковку архива
        ' BackgroundWorker1.WorkerReportsProgress = True
        'BackgroundWorker1.RunWorkerAsync()
        ' Устанавливаем софт

        install()


    End Sub
    Private Sub install()
        Dim installPath
        paramINI.File_Name = iniPath
        paramINI.LoadFile()

        iniParameters = paramINI.Get_ListOf_Parameters("INSTALL")
        For Each iniParametr In iniParameters
            installPath = Application.StartupPath & "\Support\" & iniParametr
            StartProcess(installPath, paramINI.Get_Value("INSTALL", iniParametr))
            Logining("Установка " & iniParametr & " завершена", "Успех")
            Threading.Thread.Sleep(100)
        Next

        MsgBox("Install Complite")
    End Sub

    Private Sub TextBox1_CausesValidationChanged(sender As Object, e As EventArgs) Handles TextBox1.CausesValidationChanged

    End Sub

    Private Sub TextBox1_GotFocus(sender As Object, e As EventArgs) Handles TextBox1.GotFocus
        TextBox1.BackColor = Color.LightCoral
    End Sub

    Private Sub TextBox2_GotFocus(sender As Object, e As EventArgs) Handles TextBox2.GotFocus

    End Sub

    Private Sub TextBox2_Validated(sender As Object, e As EventArgs) Handles TextBox2.Validated

    End Sub

    Private Sub NumericUpDown2_ValueChanged(sender As Object, e As EventArgs) Handles NumericUpDown2.ValueChanged
        terminlPort = NumericUpDown2.Value
        Console.WriteLine(terminlPort)
    End Sub
End Class