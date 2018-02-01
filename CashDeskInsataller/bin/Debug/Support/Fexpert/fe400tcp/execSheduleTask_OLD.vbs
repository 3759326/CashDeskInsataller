Set WshShell = WScript.CreateObject("WScript.Shell")
Set WshArg = WScript.Arguments
If WshArg.Count = 3 Then
	' Зачитаем переменные, которые были переданы в скрипт
	imageName = WshArg(0)
	taskName = WshArg(1)
	needKillTask = WshArg(2)
	tmpFileName = "C:\" + taskName + ".txt"
	'Wscript.Echo "Полученные параметры : " & imageName & ", " & taskName & ", " & needKillTask
	RetCode = WshShell.Run("cmd /c tasklist /fo CSV /nh /fi ""Imagename eq " & imageName & """> " & tmpFileName, 0, true)
	Set FSO =  CreateObject("Scripting.FileSystemObject")
	If FSO.FileExists(tmpFileName) Then
		Str = vbNullString
		Set TextStream = FSO.OpenTextFile(tmpFileName, 1)
		Do While Not TextStream.AtEndOfStream
				Str = TextStream.ReadLine()
				'Wscript.Echo Str
				If (Len(Str) > 0) Then Exit Do
		Loop
		TextStream.Close
		' Уберем с полученной строки кавычки
		Str = Replace(Str, """", "")
		'Wscript.Echo Str
		If Len(Str) > 0 Then
			' Строка какая-то есть - будем анализировать
			CurArr = Split(Str, ",")
			If StrComp(CurArr(0), imageName, 1) = 0 Then
				'Wscript.Echo "Задание  " & taskName & " найдено."
				' Если нам сказали что при нахождении указанного задания его необходимо убить - сделаем это
				If needKillTask = 1 Then
					RetCode = WshShell.Run("cmd /c taskkill /pid " & CurArr(1) & " /f", 0, true)
					' Запускаем нужное задание из планировщика
					RetCode = WshShell.Run("schtasks.exe /Run /tn " + taskName, 0, true)
				End If
			Else
				' Запускаем нужное задание из планировщика
				RetCode = WshShell.Run("schtasks.exe /Run /tn " + taskName, 0, true)
			End If
		Else
				' Запускаем нужное задание из планировщика
				'Wscript.Echo "Запускаем нужное задание из планировщика"
				RetCode = WshShell.Run("schtasks.exe /Run /tn " + taskName, 0, true)
'Wscript.Echo RetCode
		End If
	End If
End If