

Imports Ionic.Zip
Imports System.ComponentModel


Public Class frmArchive
    Public err As String
    Dim pathSorces = "C:\Fexpert"
    Dim pathDest = Application.StartupPath & "\Archive\"
    Dim archName = "fexpert_" & Replace(DateAndTime.DateString, "-", "_") & ".zip"
    Dim paramINI As New Full_INI_Class
    Dim iniSections
    Dim iniPath = Application.StartupPath & "\settings.ini"
    Dim iniParameters()
    Dim totalarch
    Dim canceleation As Boolean = False


    Private Sub frmArchive_Load(sender As Object, e As EventArgs) Handles MyBase.Load


        txtSources.Text = pathSorces
        pathDest = pathDest & archName
        txtDest.Text = pathDest
        paramINI.File_Name = iniPath
        err = ""


    End Sub

    Private Sub frmArchive_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        frmStart.Show()

    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click

        Dim curProc
        ProgressBar1.Value = 0
        Label3.Text = "0%"
        Label5.Text = "Архивирование "
        ListView1.Clear()
        ListView1.Columns.Add("Событие")
        ListView1.Columns(0).Width = 318
        ListView1.Columns.Add("Статус")
        ListView1.Columns(1).Width = 152



        Button3.Enabled = False
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
        If err.Length <> 0 Then

            Select Case MsgBox("Произошла ошибка при остановке " & vbCrLf & err & vbCrLf & "продолжить архивирование?", vbYesNo, "Винмание!")
                Case vbYes
                Case vbNo

                    iniParameters = paramINI.Get_ListOf_Parameters("SERVICES")
                    For Each iniParametr In iniParameters
                        Logining(SrvControl(paramINI.Get_Value("SERVICES", iniParametr), "Start"), logstatus)

                    Next
                    Logining("Процесс прерван", "Внимание")
                    Exit Sub
            End Select
        End If
        BackgroundWorker1.WorkerReportsProgress = True

        BackgroundWorker1.RunWorkerAsync()
        While (BackgroundWorker1.IsBusy)

            'Threading.Thread.Sleep(1000)
            Application.DoEvents()
        End While

        iniParameters = paramINI.Get_ListOf_Parameters("SERVICES")
        For Each iniParametr In iniParameters
            Logining(SrvControl(paramINI.Get_Value("SERVICES", iniParametr), "Start"), logstatus)

        Next
        If canceleation = True Then
            Logining("Архивирование прервано", "Внимание")
            MsgBox("Архивирование прерванно!", vbInformation, "Операция завершенна")
            ProgressBar1.Value = 0
            Label3.Text = "0%"
            Label5.Text = "Архивирование "
        Else
            MsgBox("Архивирование завершено!", vbInformation, "Операция завершенна")
        End If
        Button3.Enabled = True





    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        BackgroundWorker1.CancelAsync()
        BackgroundWorker1.Dispose()
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
        pathDest = OpenFileDialog1.FileName
        txtDest.Text = pathDest



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


        If BackgroundWorker1.CancellationPending = True Then
            e.Cancel = True

        End If


    End Sub
    Public Sub GoZip(sources As String, dest As String)
        Using zip As ZipFile = New ZipFile(System.Text.Encoding.GetEncoding("CP866"))
            Me.Invoke(Sub()
                          Logining("Архивирование начато", "Успех")
                      End Sub)
            zip.AddDirectory(sources, sources)
            zip.UpdateFile(Environment.GetEnvironmentVariable("WINDIR") & "\fe.ini", sources)
            totalarch = zip.EntryFileNames.Count


            AddHandler zip.SaveProgress, New EventHandler(Of SaveProgressEventArgs)(AddressOf Zip_SaveProgress)
            If canceleation = True Then

                Exit Sub
            Else

                zip.Save(dest)

            End If
            zip.Dispose()
        End Using


    End Sub
    Public Sub Zip_SaveProgress(ByVal sender As Object, ByVal e As SaveProgressEventArgs)

        Dim curentarch
        If BackgroundWorker1.CancellationPending = True Then
            canceleation = True

            e.Cancel = True



        End If
        Select Case e.EventType
                Case ZipProgressEventType.Saving_AfterWriteEntry
                    'Me.StepArchiveProgress(e)

                    Me.Invoke(Sub()

                                  Label3.Text = "0"
                                  Label5.Text = "Архивирование: " & e.CurrentEntry.FileName

                                  'totalarch = e.EntriesTotal
                                  curentarch = e.EntriesSaved
                                  'MsgBox(totalarch - curentarch)

                                  ProgressBar1.Maximum = totalarch
                                  ProgressBar1.Value = curentarch

                                  'ProgressBar1.CreateGraphics.DrawString(ProgressBar1.Maximum - ProgressBar1.Value, New Font("Arial", CSng(8.25), FontStyle.Regular), Brushes.Black, New PointF(ProgressBar1.Width / 2 - 10, ProgressBar1.Height / 2 - 7))
                                  Label3.Text = Math.Round((curentarch * 100) / totalarch) & " %"

                              End Sub)
                    Exit Select
                Case ZipProgressEventType.Saving_Completed
                    Me.Invoke(Sub()
                                  Logining("Архивирование завершенно", "Успех")
                                  Logining("Заархивировано " & totalarch - 1 & " файлов", "")
                              End Sub)
                    Exit Select
                'Case ZipProgressEventType.Saving_EntryBytesRead
                '   Me.StepEntryProgress(e)
                'Exit Select

        End Select

    End Sub

    Private Sub BackgroundWorker1_ProgressChanged(sender As Object, e As ProgressChangedEventArgs) Handles BackgroundWorker1.ProgressChanged

    End Sub

    Private Sub BackgroundWorker1_Disposed(sender As Object, e As EventArgs) Handles BackgroundWorker1.Disposed

    End Sub
End Class