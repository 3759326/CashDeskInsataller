Imports Ionic.Zip
Imports System.IO
Imports DTOLib
Imports Pervasive.Data.SqlClient



'Imports DTOLib2








Public Class frmInstall

    Dim cashDeskNum As String
    Dim cashDeskPort As String
    Dim cashDeskSettings As String
    Dim terminlPort As String
    Dim imgPath As String
    Dim destPath As String
    Dim destPathSlash As String
    Dim paramINI As New Full_INI_Class
    Dim iniSections
    Dim iniPath As String = Application.StartupPath & "\settings.ini"
    Dim iniParameters()
    Dim installDir As String = Application.StartupPath & "\Support\"
    Dim windir As String = Environment.GetEnvironmentVariable("WINDIR")
    Dim systemdir As String = Environment.GetFolderPath(Environment.SpecialFolder.System)
    Dim syswow64dir As String = Environment.GetEnvironmentVariable("WINDIR") & "\SysWOW64"
    Dim startupdir As String = Environment.GetFolderPath(Environment.SpecialFolder.Startup)
    Dim sysfontdir As String = Environment.GetEnvironmentVariable("WINDIR") & "\Fonts"
    Dim privatSettinsPath As String = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) & "\Bkc Corporation\JavaPosCATSevice\fourinone.properties"
    Dim splitPath() As String
    Dim dirFESOL As String
    Dim logFile As String
    Dim logWriter As StreamWriter
    Dim progTxt As Double









    Private Sub frmInstall_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        cashDeskNum = NumericUpDown1.Value.ToString
        cashDeskPort = NumericUpDown3.Value.ToString
        cashDeskSettings = TextBox3.Text
        terminlPort = NumericUpDown2.Value.ToString
        dirFESOL = "c:\FESOL"
        destPath = TextBox1.Text
        If destPath.LastIndexOf("\") <> Len(destPath) - 1 Then

            destPathSlash = destPath & "\"
        Else
            destPathSlash = destPath

        End If

    End Sub
    Private Sub LogFileInit()
        logFile = Application.StartupPath & "\Log" & "\CashDesk" & cashDeskNum & "_install_" & Now.ToShortDateString & ".log"
        logWriter = New StreamWriter(logFile, append:=True)
        logWriter.AutoFlush = True
    End Sub
    Public Sub Logining(eventName As String, Optional eventStatus As String = Nothing)
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




        Label2.Text = eventName & " - " & eventStatus

        ListView1.Items.Add(listit)
        ListView1.EnsureVisible(ListView1.Items.Count - 1)

        Try
            logWriter.WriteLine(Now & " " & eventName & " - " & eventStatus)
            logWriter.Flush()
        Catch ex As Exception
            LogFileInit()
            logWriter.WriteLine(Now & " " & eventName & " - " & eventStatus)
            logWriter.Flush()
        End Try


    End Sub

    Private Sub frmInstall_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        frmStart.Show()

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        'OpenFileDialog1.FileName = archName
        Dim openRes
        OpenFileDialog1.Filter = "zip files (*.zip)|"
        OpenFileDialog1.InitialDirectory = Application.StartupPath & "\Archive"
        OpenFileDialog1.CheckPathExists = True
        OpenFileDialog1.ShowReadOnly = False
        OpenFileDialog1.ReadOnlyChecked = True
        OpenFileDialog1.CheckFileExists = False
        OpenFileDialog1.ValidateNames = False
        openRes = OpenFileDialog1.ShowDialog()
        Select Case openRes
            Case vbOK
                imgPath = OpenFileDialog1.FileName

                TextBox2.Text = imgPath
            Case vbCancel

                MsgBox("Путь не задан")

        End Select




    End Sub

    Private Sub TextBox2_ModifiedChanged(sender As Object, e As EventArgs) Handles TextBox2.ModifiedChanged

    End Sub

    Private Sub TextBox2_TextChanged(sender As Object, e As EventArgs) Handles TextBox2.TextChanged
        imgPath = TextBox2.Text
        TextBox2.BackColor = Color.White
        'MsgBox(imgPath)
    End Sub

    Private Sub NumericUpDown1_ValueChanged(sender As Object, e As EventArgs) Handles NumericUpDown1.ValueChanged
        cashDeskNum = NumericUpDown1.Value
        NumericUpDown1.BackColor = Color.White

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
        TextBox1.BackColor = Color.White
        destPath = TextBox1.Text

    End Sub
    Private Sub unZip(sources As String, dest As String)
        Dim options As New ReadOptions
        Me.Invoke(Sub()
                      Logining("Распаковка архива начата", "Успех")

                  End Sub)
        options.Encoding = System.Text.Encoding.GetEncoding("CP866")
        Using zip1 As ZipFile = ZipFile.Read(sources, options)


            AddHandler zip1.ExtractProgress, AddressOf UnZipProgress
            zip1.ExtractAll(dest, ExtractExistingFileAction.OverwriteSilently)

        End Using
    End Sub
    Private Sub progress(plus As Integer)
        ProgressBar1.Value = progTxt + plus
        Label1.Text = progTxt + plus & " %"

    End Sub
    Private Sub UnZipProgress(ByVal sender As Object, ByVal e As ExtractProgressEventArgs)

        Console.WriteLine(e.EventType)
        Select Case e.EventType
            Case ZipProgressEventType.Extracting_EntryBytesWritten
                'Console.WriteLine(e.EventType)


                'Console.WriteLine(e.BytesTransferred)
                Me.Invoke(Sub()
                              Label2.Text = "Распаковка архива: " & e.CurrentEntry.FileName


                          End Sub)
            Case ZipProgressEventType.Extracting_AfterExtractEntry
                Me.Invoke(Sub()

                              Label2.Text = "Распаковка архива: " & e.CurrentEntry.FileName
                              ProgressBar1.Maximum = 100
                              progTxt = Math.Round((e.EntriesExtracted * 50 / e.EntriesTotal))
                              ' If progTxt < 30 Then
                              'Label1.Text = "0" & " %"
                              'Else
                              Label1.Text = progTxt.ToString & " %"
                              'End If

                              ProgressBar1.Value = progTxt




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
        If destPath.LastIndexOf("\") <> Len(destPath) - 1 Then

            destPathSlash = destPath & "\"
        Else
            destPathSlash = destPath

        End If

        ' Проверяем заполненость параметров и путей

        If TextBox1.Text = "" Or Directory.Exists(destPath) = False Or destPath = "" Then
            TextBox1.BackColor = Color.LightCoral
            MsgBox("Не задан путь для распаковки")
            Exit Sub

        ElseIf TextBox2.Text = "" Or imgPath = "" Then
            TextBox2.BackColor = Color.LightCoral
            MsgBox("Не задан путь к архиву")
            Exit Sub

        ElseIf NumericUpDown1.Value = "0" Then
            NumericUpDown1.BackColor = Color.LightCoral
            MsgBox("Не задан номер кассы")
            Exit Sub
        ElseIf NumericUpDown2.Value = "0" Then
            NumericUpDown2.BackColor = Color.LightCoral
            MsgBox("Не задан номер СOMпорта терминала")
            Exit Sub
        End If


        LogFileInit()

        Logining("---------- Начнемс...---------")
        Logining("------------------------------")
        Button3.Enabled = False


        'Начинаем расспаковку архива
        BackgroundWorker1.WorkerReportsProgress = True
        BackgroundWorker1.RunWorkerAsync()

        While (BackgroundWorker1.IsBusy)


            Application.DoEvents()
        End While
        ' MsgBox("Async")
        ' Устанавливаем софт
        install()


        'Копмруем и регистрим DLLки
        '-------------------------------------------------
        DllReg()

        progress(15)

        'Копируем необходимые файлы
        fileCopy()
        progress(5)
        'Копируем необходимые диретории
        dirCopy()
        progress(5)
        'Копируем необходимые шрифты
        fontCopy()
        progress(5)

        'MsgBox(destPath)
        'MsgBox(destPath.LastIndexOf("\"),, Len(destPath) - 1)


        WOfflinePath = "Fexpert\Fedata\Firms\Cash_Offline\WOffline.ini"
        WOffExpPath = "Fexpert\Fedata\Firms\Cash_Offline\WOffExp.ini"

        'Прописывае в WOffline.ini номер кассы
        cashDescIniSet(destPathSlash & WOfflinePath, "Cashdesk_Setting", "curCashdeskCode", cashDeskNum)

        'Чистим секцию WOffline.ini Cashdesk_Export
        cashDescIniClear(destPathSlash & WOfflinePath, "Cashdesk_Export")

        'Чистим секцию  WOffExp.ini Cashdesk_Export
        cashDescIniClear(destPathSlash & WOffExpPath, "Cashdesk_Export")

        'Меняем порт терминала привата
        inLineReplace(privatSettinsPath, "ComPort", terminlPort)
        progress(5)
        'Прописываем номер кассы в Финэксперте ( в базе), делаем настройку Pervasive
        'SrvControl("Pervasive.SQL (transactional)", "Restart")
        'SrvControl("Pervasive.SQL (relational)", "Restart")
        dbChange()
        progress(5)
        'Чистим директории Shop, ShopOFFC, Log
        DirClear("Shop")
        DirClear("ShopOffC")
        DirClear("Log")
        progress(5)
        'помолясь, регистрим сервис VVFECTR.exe
        StartProcess(dirFESOL & "\VVFECTRLauncherService.exe", "-install")
        Logining("---------------------------------------------")
        Logining("---------   ФСЕ!   ---------")
        Logining("---------------------------------------------")
        ProgressBar1.Value = 100
        Label1.Text = "100 %"
        Console.WriteLine("Finish")
        logWriter.Close()
        logWriter.Dispose()
        Button3.Enabled = True

    End Sub
    Private Sub install()

        paramINI.File_Name = iniPath
        paramINI.LoadFile()

        iniParameters = paramINI.Get_ListOf_Parameters("INSTALL")
        For Each iniParametr In iniParameters
            Logining("Установка " & iniParametr & " начата")
            StartProcess(installDir & iniParametr, paramINI.Get_Value("INSTALL", iniParametr))

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
        logWriter.Close()
        logWriter.Dispose()

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
            Catch ex As IOException
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
                If splitPath(1) = "\FESOL" Then
                    dirFESOL = dirdest
                End If
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
                If Directory.Exists(Environment.GetEnvironmentVariable("WINDIR") & "\SysWOW64") Then
                    splitPath(0) = "\Program Files"
                Else
                    splitPath(0) = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles)
                End If
            Case LCase("Startup")
                splitPath(0) = Environment.GetFolderPath(Environment.SpecialFolder.Startup)
            Case LCase("Startmenu")
                splitPath(0) = Environment.GetFolderPath(Environment.SpecialFolder.StartMenu)
            Case LCase("Support")
                splitPath(0) = Application.StartupPath & "\Support"
            Case LCase("FEXPERT")
                splitPath(0) = destPath
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
        Dim fileArr() As String
        Dim i As Integer

        Try
            fileArr = IO.File.ReadAllLines(file)
            For Each readstr In fileArr

                If InStr(readstr, strfind) Then

                    fileArr(i) = strReplace
                    IO.File.WriteAllLines(file, fileArr)
                    Exit Sub
                End If
                i += 1
            Next
        Catch ex As Exception
            Logining(ex.Message, "Нетого")
        End Try



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
        NumericUpDown2.BackColor = Color.White
        'Console.WriteLine(terminlPort)
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click

        'DirClear("Shop")
        'DirClear("ShopOffC")
        'DirClear("Log")
        'Console.WriteLine(Math.Round(56954.515, 2))
        'SrvControl("Pervasive.SQL (transactional)", "Restart")
        'SrvControl("Pervasive.SQL (relational)", "Restart")
        'dbChange()
        For Each de As DictionaryEntry In Environment.GetEnvironmentVariables()
            Console.WriteLine("  {0} = {1}", de.Key, de.Value)
        Next





    End Sub
    Private Sub PervasConn()
        ' Dim dbtt As New Dbtool
        ' dbtt.dbt()


        'StartProcess(Application.StartupPath & "\dbupdate.exe", destPathSlash)

        Dim pdb As Object
        Dim session As DtoSession
        Dim result As dtoResult
        Dim databas As DtoDatabase
        'Dim dtoSettings As DtoSettings
        Dim dtoSetting As DtoSetting
        'Dim dtoCat As DtoCategories
        'Dim category As DtoCategory
        Dim sett As New DtoSelectionItems

        'pdb = CreateObject("DTOLib.dll.1")


        'Пробуем подключиться к Pervasive Server
        session = New DtoSession
        databas = New DtoDatabase

        result = session.Connect("localhost")

        Console.WriteLine(result.ToString)
        Console.WriteLine(session.Connected)
        If Not result = dtoResult.Dto_Success Then
            MsgBox("Connect pervasive " & session.Error(result))
            Exit Sub
        End If

        'dtoCat = session.Categories
        'For Each category In dtoCat
        '    Console.WriteLine(category.Name & " = " & category.CategoryID)
        '    ' dtoSettings = category.Settings


        'Next
        'dtoSettings = dtoCat(4).Settings
        'For Each dtoSetting In dtoSettings
        '    Console.WriteLine("Setting = " & dtoSetting.Name & "ID = " & dtoSetting.SettingID)
        '    'Console.WriteLine(dtoSetting.AllPossibleSelections)
        'Next

        'Console.WriteLine(dtoSetting.Value)
        'Dim selections As DtoSelectionItems
        'Dim selection As DtoSelectionItem
        'selections = dtoSetting.AllPossibleSelections

        '  For Each selection In selections
        ' Console.WriteLine(selection.String & " = " & selection.ItemID)
        'Next

        'Делаем настройку первасива (версия создаваемых файлов, раздел в Control Center - Compatibility -> Create File Version) устанавливаем 7.х
        Try
            dtoSetting = session.GetSetting(50)
            sett.Add(dtoSetting.AllPossibleSelections.GetByID(3))
            dtoSetting.Value = sett
            MsgBox("Настройка первасива Create File Version=7.х выполнена",, "Успех")
            'Logining("Настройка первасива Create File Version=7.х выполнена", "Успех")
        Catch ex As Exception
            MsgBox("Pervasive settnigs " & ex.Message)
            '  Logining("Не удалось установить настройку первасива Create File Version=7.х", "Нетого")
        End Try





        'Пробуем зарегестрировать базу настроек кассы в Pervasive
        databas.Name = "CashOffline"
        databas.DataPath = destPathSlash & "Fexpert\Fedata\Firms\Cash_Offline"
        databas.DdfPath = destPathSlash & "Fexpert\Fedata\Firms\Cash_Offline"
        databas.DBCodePage = dtoDbCodePage.dtoDbZeroCodePage

        databas.Flags = 0
        'MsgBox(session.ConnectionID)



        result = session.Databases.Add(databas)
        If Not result = dtoResult.Dto_Success Then

            If result = dtoResult.Dto_errDuplicateName Then
                MsgBox("База CashOffline уже зарегестрированна", , "Внимание")
                session.Disconnect()
            Else

                MsgBox("Ошибка регистрации базы данных," & vbCrLf & session.Error(result) & vbCrLf & " проверьте работу Pervasive Server")
                ' Logining("Ошибка регистрации базы данных," &  & " проверьте работу Pervasive Server", "Нетого")
                'Console.WriteLine(session.Error(result))
                session.Disconnect()
                Exit Sub
            End If
        Else
            MsgBox("База CashOffline подключена ", , "Успех")
            session.Disconnect()
        End If


    End Sub
    Private Sub dbChange()

        Dim session As DtoSession
        Dim result As dtoResult
        Dim databas As DtoDatabase
        'Dim dtoSettings As DtoSettings
        Dim dtoSetting As DtoSetting
        'Dim dtoCat As DtoCategories
        'Dim category As DtoCategory
        Dim sett As New DtoSelectionItems



        'Пробуем подключиться к Pervasive Server
        session = New DtoSession
        databas = New DtoDatabase

        result = session.Connect("localhost")

        Console.WriteLine(result.ToString)
        Console.WriteLine(session.Connected)
        If Not result = dtoResult.Dto_Success Then
            MsgBox("Connect pervasive " & session.Error(result))
            Exit Sub
        End If

        'dtoCat = session.Categories
        'For Each category In dtoCat
        '    Console.WriteLine(category.Name & " = " & category.CategoryID)
        '    ' dtoSettings = category.Settings


        'Next
        'dtoSettings = dtoCat(4).Settings
        'For Each dtoSetting In dtoSettings
        '    Console.WriteLine("Setting = " & dtoSetting.Name & "ID = " & dtoSetting.SettingID)
        '    'Console.WriteLine(dtoSetting.AllPossibleSelections)
        'Next

        'Console.WriteLine(dtoSetting.Value)
        'Dim selections As DtoSelectionItems
        'Dim selection As DtoSelectionItem
        'selections = dtoSetting.AllPossibleSelections

        '  For Each selection In selections
        ' Console.WriteLine(selection.String & " = " & selection.ItemID)
        'Next

        'Делаем настройку первасива (версия создаваемых файлов, раздел в Control Center - Compatibility -> Create File Version) устанавливаем 7.х
        Try
            dtoSetting = session.GetSetting(50)
            sett.Add(dtoSetting.AllPossibleSelections.GetByID(3))
            dtoSetting.Value = sett
            Logining("Настройка первасива Create File Version=7.х выполнена", "Успех")

        Catch ex As Exception
            MsgBox("Pervasive settnigs " & ex.Message)
            Logining("Не удалось установить настройку первасива Create File Version=7.х", "Нетого")
        End Try





        'Пробуем зарегестрировать базу настроек кассы в Pervasive
        databas.Name = "CashOffline"
        databas.DataPath = destPathSlash & "Fexpert\Fedata\Firms\Cash_Offline"
        databas.DdfPath = destPathSlash & "Fexpert\Fedata\Firms\Cash_Offline"
        databas.DBCodePage = dtoDbCodePage.dtoDbZeroCodePage

        databas.Flags = 0
        'MsgBox(session.ConnectionID)



        result = session.Databases.Add(databas)
        If Not result = dtoResult.Dto_Success Then

            If result = dtoResult.Dto_errDuplicateName Then
                Logining("База CashOffline уже зарегестрированна", "Внимание")
                session.Disconnect()
            Else

                MsgBox("Ошибка регистрации базы данных," & vbCrLf & result.ToString & vbCrLf & " проверьте работу Pervasive Server")
                Logining("Ошибка регистрации базы данных," & session.Error(result) & " проверьте работу Pervasive Server", "Нетого")
                'Console.WriteLine(session.Error(result))
                session.Disconnect()
                Exit Sub
            End If
        Else
            Logining("База CashOffline подключена ", "Успех")
            session.Disconnect()
        End If





        'MsgBox(session.Databases.Count)
        'For i = 1 To session.Databases.Count
        '    MsgBox(session.Databases(i).Name)

        'Next
        ''    MsgBox(datab)
        ''Next
        'MsgBox(result)
        'MsgBox(session.Error(result))

        'Пробуем подключиться к базе
        Dim Conn As New PsqlConnection("Host=localhost;Port=1583;Database=CashOffline; Encoding=1251")
        Try
            Conn.Open()
            Logining("Подключени к базе CashOffline", "Успех")
        Catch ex As Exception
            Logining("Ошибка подключения к базе CashOffline", "Нетого")
            Logining(ex.Message, "")
            Exit Sub
        End Try

        Dim doCmd As PsqlCommand
        Dim dataread As PsqlDataReader
        'Dim dataupdate As psql
        'dataread = doCmd.ExecuteReader
        'While dataread.Read()
        '    Console.WriteLine("ctCode=" & dataread("ctCode").ToString)
        '    Console.WriteLine("ctName=" & dataread("ctName").ToString)
        'End While
        'Пробуем писать в базу

        Dim cashDescName As String = "Касса " & cashDeskNum
        Try
            doCmd = New PsqlCommand("UPDATE SO_SHOPCASH Set ctCode='" & cashDeskNum & "' , ctName='" & cashDescName & "', cashCode='" & cashDeskNum & "', COMMport='" & cashDeskPort & "', COMMsetting='" & cashDeskSettings & "'", Conn)
            doCmd.ExecuteNonQuery()
            Logining("Параметры кассы обновленны успешно", "Успех")
        Catch ex As Exception
            Logining("Ошибка записи параметров кассы в БД", "Нетого")
            Logining(ex.Message, "")
            Conn.Close()
            Exit Sub
        End Try

        'попробуем прочитать записанное
        Try
            doCmd = New PsqlCommand("SELECT ctCode,ctName,cashCode, COMMport,COMMsetting FROM SO_SHOPCASH", Conn)
            dataread = doCmd.ExecuteReader
            While dataread.Read()
                Logining("ID кассы = " & dataread("ctCode").ToString)
                Logining("Имя кассы = " & dataread("ctName").ToString)
                Logining("Код кассы = " & dataread("cashCode").ToString)
                Logining("COM порт кассы установлен =" & dataread("COMMport").ToString)
                Logining("Настройки ком порта = " & dataread("COMMsetting").ToString)
            End While
        Catch ex As Exception
            Logining("Ошибка чтения параметров кассы в БД", "Нетого")
            Logining(ex.Message, "")
            Conn.Close()
            Exit Sub
        End Try

        Conn.Close()

    End Sub
    Private Sub DirClear(searchDir As String)
        Dim clearPath As String = destPathSlash & "Fexpert\Fedata\Firms\Cash_Offline\"
        Dim clearDirs() As String = Directory.GetDirectories(clearPath, searchDir, searchOption:=SearchOption.AllDirectories)
        Dim clearFiles() As String

        For Each dir As String In clearDirs
            If dir <> clearPath & "ShopOffC" Then
                clearFiles = Directory.GetFiles(dir)
                If clearFiles.Length = 0 Then
                    Logining("Директория  " & dir & " пуста", "Успех")
                End If
                For Each clsFile In clearFiles
                        Try
                            File.Delete(clsFile)
                        Logining("Директория  " & dir & " очищена", "Успех")
                    Catch ex As Exception
                            Logining("Ошибка удаления файла " & clsFile, "Нетого")
                        End Try

                    Next

                End If

        Next


    End Sub
    Private Sub NumericUpDown3_ValueChanged(sender As Object, e As EventArgs) Handles NumericUpDown3.ValueChanged
        cashDeskPort = NumericUpDown3.Value
    End Sub

    Private Sub TextBox3_TextChanged(sender As Object, e As EventArgs) Handles TextBox3.TextChanged
        cashDeskSettings = TextBox3.Text
    End Sub

    Private Sub BackgroundWorker2_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs)



    End Sub
End Class