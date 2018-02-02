Set WshShell = WScript.CreateObject("WScript.Shell")
Set WshArg = WScript.Arguments
If WshArg.Count = 3 Then
	' �������� ����������, ������� ���� �������� � ������
	imageName = WshArg(0)
	taskName = WshArg(1)
	needKillTask = WshArg(2)
	tmpFileName = "C:\" + taskName + ".txt"
	'Wscript.Echo "���������� ��������� : " & imageName & ", " & taskName & ", " & needKillTask
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
		' ������ � ���������� ������ �������
		Str = Replace(Str, """", "")
		'Wscript.Echo Str
		If Len(Str) > 0 Then
			' ������ �����-�� ���� - ����� �������������
			CurArr = Split(Str, ",")
			If StrComp(CurArr(0), imageName, 1) = 0 Then
				'Wscript.Echo "�������  " & taskName & " �������."
				' ���� ��� ������� ��� ��� ���������� ���������� ������� ��� ���������� ����� - ������� ���
				If needKillTask = 1 Then
					RetCode = WshShell.Run("cmd /c taskkill /pid " & CurArr(1) & " /f", 0, true)
					' ��������� ������ ������� �� ������������
					RetCode = WshShell.Run("schtasks.exe /Run /tn " + taskName, 0, true)
				End If
			Else
				' ��������� ������ ������� �� ������������
				RetCode = WshShell.Run("schtasks.exe /Run /tn " + taskName, 0, true)
			End If
		Else
				' ��������� ������ ������� �� ������������
				'Wscript.Echo "��������� ������ ������� �� ������������"
				RetCode = WshShell.Run("schtasks.exe /Run /tn " + taskName, 0, true)
'Wscript.Echo RetCode
		End If
	End If
End If