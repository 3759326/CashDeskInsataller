Imports Ionic.Zip
Imports System.IO
Imports DTOLib2




Public Class frmInstall

    Dim cashDeskNum
    Dim terminlPort
    Dim imgPath
    Dim destPath As String
    Dim paramINI As New Full_INI_Class
    Dim iniSections
    Dim iniPath = Application.StartupPath & "\settings.ini"
    Dim iniParameters()
    Dim installDir = Application.StartupPath & "\Support\"
    Dim windir = Environment.GetEnvironmentVariable("WINDIR")
    Dim systemdir = Environment.GetFolderPath(Environment.SpecialFolder.System)
    Dim syswow64dir = Environment.GetEnvironmentVariable("WINDIR") & "\SysWOW64"
    Dim startupdir = Environment.GetFolderPath(Environment.SpecialFolder.Startup)
    Dim sysfontdir = Environment.GetEnvironmentVariable("WINDIR") & "\Fonts"
    Dim privatSettinsPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) & "\Bkc Corporation\JavaPosCATSevice\fourinone.properties"
    Dim splitPath() As String







    Private Sub frmInstall_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        cashDeskNum = NumericUpDown1.Value
        terminlPort = NumericUpDown2.Value
        destPath = TextBox1.Text
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
                Case "Нетого"
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
        Dim WOfflinePath As String
        Dim WOffExpPath As String
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
        ' BackgroundWorker1.RunWorkerAsync()
        'While (BackgroundWorker1.IsBusy)


        'Application.DoEvents()
        'End While
        ' Устанавливаем софт
        'install()
        'Копмруем и регистрим DLLки
        'DllReg()
        'fileCopy()
        'dirCopy()
        'fontCopy()
        'Прописывае в WOffline.ini номер кассы
        If destPath.LastIndexOf("\") Then
            WOfflinePath = "Fexpert\Fedata\Firms\Cash_Offline\WOffline.ini"
            WOffExpPath = "Fexpert\Fedata\Firms\Cash_Offline\WOffExp.ini"
        Else
            WOfflinePath = "\Fexpert\Fedata\Firms\Cash_Offline\WOffline.ini"
            WOffExpPath = "\Fexpert\Fedata\Firms\Cash_Offline\WOffExp.ini"
        End If
        'Прописывае в WOffline.ini номер кассы
        'cashDescIniSet(destPath & WOfflinePath, "Cashdesk_Setting", "curCashdeskCode", cashDeskNum)
        'Чистим секцию WOffline.ini Cashdesk_Export
        'cashDescIniClear(destPath & WOfflinePath, "Cashdesk_Export")
        'Чистим секцию  WOffExp.ini Cashdesk_Export
        'cashDescIniClear(destPath & WOffExpPath, "Cashdesk_Export")
        'Меняем порт терминала привата
        inLineReplace(privatSettinsPath, "ComPort", terminlPort)





    End Sub
    Private Sub install()

        paramINI.File_Name = iniPath
        paramINI.LoadFile()

        iniParameters = paramINI.Get_ListOf_Parameters("INSTALL")
        For Each iniParametr In iniParameters

            StartProcess(installDir & iniParametr, paramINI.Get_Value("INSTALL", iniParametr))
            Logining("Установка " & iniParametr & " завершена", "Успех")
            Threading.Thread.Sleep(100)
        Next

        'MsgBox("Install Complite")

    End Sub
    Private Sub DllReg()
        Dim dllname
        paramINI.File_Name = iniPath
        paramINI.LoadFile()

        iniParameters = paramINI.Get_ListOf_Parameters("DLL")
        For Each iniParametr In iniParameters
            dllname = paramINI.Get_Value("DLL", iniParametr)
            File.Copy(installDir & dllname, systemdir & "\" & dllname, True)
            Logining("DLL " & dllname & " скопирована", "Успех")
            If Directory.Exists(syswow64dir) Then
                File.Copy(installDir & dllname, syswow64dir & "\" & dllname, True)
                Logining("DLL " & dllname & " скопирована", "Успех")
                StartProcess(syswow64dir & "\regsvr32.exe", " /S " & dllname)
                Logining("DLL " & dllname & " зарегестрирована", "Успех")
            Else

                StartProcess("regsvr32.exe", " /S " & dllname)
                Logining("DLL " & dllname & " зарегестрирована", "Успех")
            End If

        Next

    End Sub
    Private Sub fileCopy()
        Dim stdPathres1 As String
        Dim stdPathres2 As String

        Dim fsources
        Dim fname
        Dim fdest
        Console.WriteLine(Environment.GetFolderPath(Environment.SpecialFolder.Programs))
        paramINI.File_Name = iniPath
        paramINI.LoadFile()

        iniParameters = paramINI.Get_ListOf_Parameters("FILESCOPY")
        For Each iniParametr In iniParameters
            If InStr(iniParametr, "#") Then
                stdPath(iniParametr)

                fname = Mid(splitPath(1), splitPath(1).LastIndexOf("\") + 1)
                MsgBox(fname)
                fsources = splitPath(0) & splitPath(1)
            Else
                fsources = iniParametr
            End If
            If InStr(paramINI.Get_Value("FILESCOPY", iniParametr), "#") Then
                stdPath(paramINI.Get_Value("FILESCOPY", iniParametr))

                fdest = splitPath(0) & splitPath(1)
            Else
                fdest = paramINI.Get_Value("FILESCOPY", iniParametr)
            End If
            Try
                File.Copy(fsources, fdest & fname, True)
                'MsgBox((stdPath(iniParametr).firstPart & stdPath(iniParametr).secondPart))
                'MsgBox(stdPath(iniParametr).secondPart)
                Logining("Файл " & fname & " скопирован в " & fdest, "Успех")
            Catch ex As Exception
                MsgBox(fsources & vbCrLf & fdest & fname & vbCrLf & ex.Message)
                Logining("Ошибка копирования " & fname & " в " & fdest, "Нетого")
            End Try



        Next

        splitPath = Nothing
    End Sub
    Private Sub dirCopy()
        Dim dirsourc As String
        Dim dirdest As String
        paramINI.File_Name = iniPath
        paramINI.LoadFile()

        iniParameters = paramINI.Get_ListOf_Parameters("DIRCOPY")
        For Each iniParametr In iniParameters
            If InStr(iniParametr, "#") Then
                stdPath(iniParametr)

                ' fname = Mid(splitPath(1), splitPath(1).LastIndexOf("\") + 1)
                'MsgBox(fname)
                dirsourc = splitPath(0) & splitPath(1)
            Else
                dirsourc = iniParametr
            End If
            If InStr(paramINI.Get_Value("DIRCOPY", iniParametr), "#") Then
                stdPath(paramINI.Get_Value("DIRCOPY", iniParametr))

                dirdest = splitPath(0) & splitPath(1)
            Else
                dirdest = paramINI.Get_Value("DIRCOPY", iniParametr)
            End If
            Console.WriteLine(dirsourc)
            Console.WriteLine(dirdest)
            Try
                My.Computer.FileSystem.CopyDirectory(dirsourc, dirdest, True)
                Logining("Директория " & splitPath(1) & " скопирована в " & dirdest, "Успех")
            Catch ex As Exception
                MsgBox(dirsourc & vbCrLf & dirdest & vbCrLf & ex.Message)
                Logining("Ошибка копирования " & dirsourc & " в " & dirdest, "Нетого")
            End Try
        Next

    End Sub
    Function stdPath(path As String)

        splitPath = Split(path, "#")
        Select Case LCase(splitPath(0))
            Case LCase("Windir")
                splitPath(0) = Environment.GetEnvironmentVariable("WINDIR")
            Case LCase("System32")
                splitPath(0) = Environment.GetFolderPath(Environment.SpecialFolder.System)

            Case LCase("SysWOW64")
                splitPath(0) = Environment.GetEnvironmentVariable("WINDIR") & "\SysWOW64"
            Case LCase("Desktop")
                splitPath(0) = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
            Case LCase("Program")
                splitPath(0) = Environment.GetFolderPath(Environment.SpecialFolder.Programs)
            Case LCase("ProgramFile")
                splitPath(0) = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles)
            Case LCase("Startup")
                splitPath(0) = Environment.GetFolderPath(Environment.SpecialFolder.Startup)
            Case LCase("Startmenu")
                splitPath(0) = Environment.GetFolderPath(Environment.SpecialFolder.StartMenu)
            Case LCase("Support")
                splitPath(0) = Application.StartupPath & "\Support"
        End Select
        ' tmp.secondPart = Strings.Mid(path, InStr(path, ":") + 1)

        'Return tmp

    End Function
    Private Sub fontCopy()
        Dim fontsources As String
        Dim fontname As String

        paramINI.File_Name = iniPath
        paramINI.LoadFile()

        iniParameters = paramINI.Get_ListOf_Parameters("FONTS")
        For Each iniParametr In iniParameters
            If InStr(iniParametr, "#") Then
                stdPath(iniParametr)

                ' fname = Mid(splitPath(1), splitPath(1).LastIndexOf("\") + 1)
                'MsgBox(fname)
                fontsources = splitPath(0) & splitPath(1)
            Else
                fontsources = iniParametr
            End If
            For Each fontfile In Directory.GetFiles(fontsources)
                fontname = Mid(fontfile, fontfile.LastIndexOf("\") + 1)
                Try
                    File.Copy(fontfile, sysfontdir & fontname, True)
                    Logining("Шрифт " & fontname & " скопирована в " & sysfontdir, "Успех")
                Catch ex As Exception
                    Logining("Ошибка копирования шрифта" & fontname, "Нетого")
                End Try

            Next
        Next

    End Sub
    Private Sub cashDescIniSet(inifile As String, inisect As String, iniparam As String, inivalue As String)
        paramINI.File_Name = inifile
        paramINI.LoadFile()
        Try
            paramINI.Set_Value(inisect, iniparam, inivalue)
            paramINI.SaveFile()

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try




    End Sub
    Private Sub cashDescIniClear(inifile As String, inisect As String)
        paramINI.File_Name = inifile
        paramINI.LoadFile()
        For Each param In paramINI.Get_ListOf_Parameters(inisect)
            paramINI.Delete_Parameter(inisect, param)
        Next
        paramINI.SaveFile()
    End Sub
    Private Sub inLineReplace(file As String, strfind As String, strReplace As String)
        Dim fileArr() As String = IO.File.ReadAllLines(file)
        Dim i As Integer



        For Each readstr In fileArr

            If InStr(readstr, strfind) Then

                fileArr(i) = strReplace
                IO.File.WriteAllLines(file, fileArr)
                Exit Sub
            End If
            i += 1
        Next


    End Sub
    'Function pathM(path As String)
    '    spPath = Split(path, ":")
    '    Return spPath


    'End Function

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

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        ' Dim pws As Object
        '  pws = CreateObject("DTO.DtoSession.1")
        Dim session As DtoSession
        Dim result As dtoResult
        Dim databas As DtoDatabase

        session = New DtoSession
        databas = New DtoDatabase
        result = session.Connect("Diempc")
        MsgBox(result.ToString)
        databas.Name = "SOCASHDESk"
        databas.DataPath = "C:\Fexpert\Fedata\Firms\Cash_Offline"
        databas.DdfPath = "C:\Fexpert\Fedata\Firms\Cash_Offline"
        databas.DBCodePage = 1251
        databas.Flags = 0
        MsgBox(session.Databases.Count)

        result = session.Databases.Add(databas)
        MsgBox(result)



        MsgBox(session.Databases.Count)
        For i = 1 To session.Databases.Count
            MsgBox(session.Databases(i).Name)

        Next
        '    MsgBox(datab)
        'Next
        MsgBox(result)
        MsgBox(session.Error(result))






        '        MsgBox(windir)
        '       MsgBox(systemdir)
        '      MsgBox(syswow64dir)
        '     MsgBox(startupdir)

    End Sub
End Class