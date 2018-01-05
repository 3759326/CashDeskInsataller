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
                    sc.Stop()
                    sc.Refresh()
                    SrvControl = "Остановка службы" & srvName
                    logstatus = "Успех"
                Catch ex As Exception
                    MsgBox(ex.Message)
                End Try


            Case "Start"
                Try
                    If sc.Status = 4 Then
                        SrvControl = "Служба " & srvName & " уже запущенна"
                        logstatus = "Внимание"
                        Exit Function
                    End If
                    sc.Start()
                    sc.Refresh()
                    SrvControl = "Запуск службы" & srvName
                    logstatus = "Успех"
                Catch ex As Exception
                    MsgBox(ex.Message)
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
            If p.Length > 0 Then
                ' Валим процесс 
                'MsgBox(p.Length)
                For i = 0 To p.Length - 1
                    Process.GetProcessesByName(procName)(0).Kill()
                    ' Пробуем еще раз на случай если он не один 

                Next
                KillProc(procName)
            Else
                logstatus = "Успех"
                Exit Function

            End If

        Catch ex As Exception
            'Logining(ex.Message, "Внимание")
        End Try

    End Function
End Module
