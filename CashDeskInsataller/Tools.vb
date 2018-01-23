Imports System.ServiceProcess



Module Tools
    Public logstatus
    Function SrvControl(srvName As String, srvAction As String) As String

        Dim sc As New ServiceController(srvName)
        Select Case srvAction

            Case "Stop"
                Try

                    If sc.Status = 1 Then
                        SrvControl = "Служба " & srvName & " уже остановленна"
                        logstatus = "Внимание"
                        Exit Function
                    End If
trypoint1:

                    sc.Stop()
                    Threading.Thread.Sleep(500)
                    sc.Refresh()
                    If sc.Status = 1 Then
                        SrvControl = "Остановка службы" & srvName
                        logstatus = "Успех"
                    Else
                        GoTo trypoint1

                    End If

                Catch ex As Exception
                    frmArchive.err = srvName
                    frmArchive.Logining(eventName:=ex.Message, eventStatus:="Внимание")
                End Try


            Case "Start"
                Try
                    ' Threading.Thread.Sleep(300)
                    If sc.Status = 4 Then
                        SrvControl = "Служба " & srvName & " уже запущенна"
                        logstatus = "Внимание"
                        Exit Function
                    End If
trypoint2:

                    sc.Start()
                    Threading.Thread.Sleep(500)
                    sc.Refresh()
                    If sc.Status = 4 Then
                        SrvControl = "Запуск службы" & srvName
                        logstatus = "Успех"
                    Else
                        GoTo trypoint2
                    End If

                Catch ex As Exception

                    'MsgBox(ex.Message)
                    frmArchive.Logining(ex.Message, "Внимание")
                End Try
            Case "Status"
                'SrvService = sc.Status

        End Select
    End Function

    Function SrvStatus(srvName) As String
        Dim sc As New ServiceController(srvName)
        SrvStatus = sc.Status


    End Function

    Function KillProc(procName As String) As String
        Dim p() As Process

        Try
            p = Process.GetProcessesByName(procName)

            If ProcesStatus(procName) = True Then
                ' Валим процесс 
trypoint1:
                For i = 0 To p.Length - 1
                    Process.GetProcessesByName(procName)(0).Kill()
                    Threading.Thread.Sleep(500)
                    ' Пробуем еще раз на случай если он не один 

                Next
                If ProcesStatus(procName) = False Then
                    KillProc = "Остановка процесса " & procName
                    logstatus = "Успех"
                Else
                    GoTo trypoint1
                End If

            Else
                KillProc = "Процесс " & procName & " не найден"
                logstatus = "Внимание"
                Exit Function

            End If

        Catch ex As Exception
            frmArchive.Logining(ex.Message, "Внимание")
        End Try

    End Function
    Function ProcesStatus(procName As String) As Boolean
        Dim p() As Process
        p = Process.GetProcessesByName(procName)
        If p.Length > 0 Then
            ProcesStatus = True
        Else
            ProcesStatus = False
        End If
    End Function
    Function StartProcess(procName As String, Optional argument As String = "")
        Dim pInfo As New ProcessStartInfo()

        pInfo.FileName = procName
        pInfo.Arguments = argument

        Dim p As Process = Process.Start(pInfo)
        p.WaitForExit()


    End Function

End Module
