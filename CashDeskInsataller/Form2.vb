

Imports Ionic.Zip
Imports System.ComponentModel

Public Class frmArchive
    Dim pathSorces = "C:\Fexpert"
    Dim pathDest = Application.StartupPath & "\Archive\"
    Dim archName = "fexpert_" & Replace(DateAndTime.DateString, "-", "_") & ".zip"
    Dim paramINI As New Full_INI_Class
    Dim iniSections
    Dim iniPath = Application.StartupPath & "\settings.ini"
    Dim iniParameters

    Private Sub frmArchive_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        txtSources.Text = pathSorces
        pathDest = pathDest & archName
        txtDest.Text = pathDest
        paramINI.File_Name = iniPath



    End Sub

    Private Sub frmArchive_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        frmStart.Show()

    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click

        Dim curProc
        paramINI.LoadFile()
        'iniSections = paramINI.Get_ListOf_SectionKeys()
        iniParameters = paramINI.Get_ListOf_Parameters("SERVICES")
        For Each iniParametr In iniParameters
            Logining(SrvControl(paramINI.Get_Value("SERVICES", iniParametr), "Stop"), logstatus)

        Next
        iniParameters = paramINI.Get_ListOf_Parameters("PROCESSES")
        For Each iniParametr In iniParameters
            curProc = paramINI.Get_Value("PROCESSES", iniParametr)
            '  If ProcesStatus(curProc) Then
            Logining(KillProc(curProc), logstatus)
            ' Else
            'Logining("Процесс " & curProc & " не запущен", "Внимание")
            'End If
        Next

        curProc = Nothing
        BackgroundWorker1.WorkerReportsProgress = True
        BackgroundWorker1.RunWorkerAsync()
        While (BackgroundWorker1.IsBusy)

            'Threading.Thread.Sleep(1000)
            Application.DoEvents()
        End While
        MsgBox("tadama")

        'Dim i = 0
        'Do While i <> 100
        '    Logining("kjsgfvsfagf", i)
        '    i += 1
        'Loop
        'Logining("kjsgfvsfagf", "Успех")





    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click

    End Sub

    Private Sub Label2_Click(sender As Object, e As EventArgs) Handles Label2.Click

    End Sub

    Private Sub Label1_Click(sender As Object, e As EventArgs) Handles Label1.Click

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        FolderBrowserDialog1.ShowDialog()
        pathSorces = FolderBrowserDialog1.SelectedPath
        txtSources.Text = pathSorces
        '        MsgBox(pathSorces)
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        'SaveFileDialog1.FileName = archName
        'SaveFileDialog1.DefaultExt = ".zip"
        'SaveFileDialog1.Filter = "zip files (*.zip)|"
        'SaveFileDialog1.InitialDirectory = pathDest
        'SaveFileDialog1.AddExtension = True
        'SaveFileDialog1.RestoreDirectory = True
        'SaveFileDialog1.ShowDialog()
        'MsgBox(SaveFileDialog1.FileName)
        OpenFileDialog1.FileName = archName
        OpenFileDialog1.Filter = "zip files (*.zip)|"
        OpenFileDialog1.InitialDirectory = pathDest
        OpenFileDialog1.CheckPathExists = True
        OpenFileDialog1.ShowReadOnly = False
        OpenFileDialog1.ReadOnlyChecked = True
        OpenFileDialog1.CheckFileExists = False
        OpenFileDialog1.ValidateNames = False
        OpenFileDialog1.ShowDialog()
        MsgBox(OpenFileDialog1.FileName)
        pathDest = OpenFileDialog1.FileName
        txtDest.Text = pathDest
        MsgBox(pathDest)


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

    Private Sub ListView1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListView1.SelectedIndexChanged

    End Sub

    Private Sub BackgroundWorker1_DoWork(sender As Object, e As DoWorkEventArgs) Handles BackgroundWorker1.DoWork
        GoZip(pathSorces, pathDest)
    End Sub
    Public Sub GoZip(sources As String, dest As String)
        Using zip As ZipFile = New ZipFile(System.Text.Encoding.GetEncoding("CP866"))
            zip.AddDirectory(sources, sources)
            MsgBox(zip.EntryFileNames.Count - 1)

            AddHandler zip.SaveProgress, New EventHandler(Of SaveProgressEventArgs)(AddressOf Zip_SaveProgress)
            zip.Save(dest)
            MsgBox("Complite!")


        End Using


    End Sub
    Public Sub Zip_SaveProgress(ByVal sender As Object, ByVal e As SaveProgressEventArgs)
        Dim totalarch
        Dim curentarch

        Select Case e.EventType
            Case ZipProgressEventType.Saving_AfterWriteEntry
                'Me.StepArchiveProgress(e)

                Me.Invoke(Sub()

                              Label3.Text = "0"
                              Label3.Text = e.CurrentEntry.FileName

                              totalarch = e.EntriesTotal
                              curentarch = e.EntriesSaved
                              'MsgBox(totalarch - curentarch)

                              ProgressBar1.Maximum = totalarch
                              ProgressBar1.Value = curentarch

                              'ProgressBar1.CreateGraphics.DrawString(ProgressBar1.Maximum - ProgressBar1.Value, New Font("Arial", CSng(8.25), FontStyle.Regular), Brushes.Black, New PointF(ProgressBar1.Width / 2 - 10, ProgressBar1.Height / 2 - 7))
                              'Label4.Text = Math.Round((curentarch * 100) / totalarch) & " %"

                          End Sub)
                Exit Select
                'Case ZipProgressEventType.Saving_Completed
                '   Me.SaveCompleted()
                '  Exit Select
                'Case ZipProgressEventType.Saving_EntryBytesRead
                '   Me.StepEntryProgress(e)
                'Exit Select
        End Select
    End Sub
End Class