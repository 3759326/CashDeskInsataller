Public Class Full_INI_Class

#Region "Class Description"
	'*********************************************************************************************************
	'*									ПОЛНЫЙ КЛАСС ДЛЯ РАБОТЫ С INI-ФАЙЛАМИ								 *
	'*									   Автор: Комар Юрий Александрович									 *
	'*-------------------------------------------------------------------------------------------------------*
	'*  Dim INI As New Full_INI_Class - Присвоение класса переменной с именем "INI" (меняйте как вам удобно) *
	'*-------------------------------------------------------------------------------------------------------*
	'*  LoadFile()					- Boolean функция, грузит файл, формируя в памяти Dictionary для работы	 *
	'*  File_Name					- String переменная, хранящая имя текущего файла(оставил как Read\Write) *
	'*  AutoSaveFile				- Boolean переменная, хранящая инфо об авто-сохранении при обновлении    *
	'*  SectionBrackets				- String переменная, хранящая брэкеты секций для загрузки и сохранения	 *
	'*  ParameterSeparator			- String переменная, хранящая разделитель между Параметром и Значением	 *
	'*  Get_Value()					- String функция, получающая Значение нужного параметра					 *
	'*  Set_Value()					- Метод, создает или обновляем значение нужного параметра				 *
	'*  Get_ListOf_SectionKeys()	- Array функция, получающая список имён всех секций						 *
	'*  Get_ListOf_Parameters()		- Array функция, получающая список имён всех параметров в нужной секции	 *
	'*  SectionsCount()				- Integer функция, получающая количество всех секций					 *
	'*  ParametersCount()			- Integer функция, получающая количество параметров в секции или в файле *
	'*  Rename_Section()			- Метод, переименовывающий секцию (если она отсутвует, создает новую)	 *
	'*  Rename_Parameter()			- Метод, переименовывающий параметр (если она отсутвует, создает новый)	 *
	'*  Delete_Section()			- Метод, удаляющий всю секцию целиком									 *
	'*  Delete_Parameter()			- Метод, удаляющий параметр вместе с его значением из нужной секции		 *
	'*  Move_Parameter_FromTo()		- Метод, перемещающий параметр с его значением из одной секции в другую	 *
	'*  SaveFile()					- Boolean функция, сохраняющая сделанные изменения в файл формата "UTF8" *
	'*-------------------------------------------------------------------------------------------------------*
	'*											September 2015												 *
	'*							Copyright © 2015 Komar Yury <komar.yury@gmail.com>							 *
	'*********************************************************************************************************
#End Region

#Region "WinAPI - Держу здесь как раритет в музее! :)"
	'	Public Declare Function WritePrivateProfileSection Lib "kernel32.DLL" Alias "WritePrivateProfileSectionA" ( _
	'	 ByVal lpSectionName As String, _ 'Имя секции
	'	 ByVal lpString As String, _ 'Значение параметра
	'	 ByVal lpFileName As String) As Integer 'путь к ini-файлу

	'	Public Declare Function WritePrivateProfileString Lib "kernel32.DLL" Alias "WritePrivateProfileStringA" ( _
	'	 ByVal lpSectionName As String, _ 'Имя секции
	'	 <Runtime.InteropServices.MarshalAs(Runtime.InteropServices.UnmanagedType.AsAny)> ByVal lpKeyName As Object, _ 'Имя параметра
	'	 <Runtime.InteropServices.MarshalAs(Runtime.InteropServices.UnmanagedType.AsAny)> ByVal lpString As Object, _ 'Значение параметра
	'	 ByVal lpFileName As String) As Integer 'путь к ini-файлу

	'	Public Declare Function GetPrivateProfileSection Lib "kernel32.DLL" Alias "GetPrivateProfileSectionA" ( _
	'	 ByVal lpSectionName As String, _ 'Имя секции
	'	 ByVal lpReturnedString As String, _ 'Возвращаемое значение
	'	 ByVal nSize As Integer, _ 'Максимальный размер возвращаемого значения
	'	 ByVal lpFileName As String) As Integer 'путь к ini-файлу

	'	Public Declare Function GetPrivateProfileString Lib "kernel32.DLL" Alias "GetPrivateProfileStringA" ( _
	'	 ByVal lpSectionName As String, _ 'Имя секции
	'	 <Runtime.InteropServices.MarshalAs(Runtime.InteropServices.UnmanagedType.AsAny)> ByVal lpKeyName As Object, _ 'Имя параметра
	'	 ByVal lpDefault As String, _ 'Значение параметра по умолчанию
	'	 ByVal lpReturnedString As String, _ 'Возвращаемое значение
	'	 ByVal nSize As Integer, _ 'Максимальный размер возвращаемого значения
	'	 ByVal lpFileName As String) As Integer 'путь к ini-файлу

	'	Public Declare Function GetPrivateProfileSectionNames Lib "kernel32.DLL" Alias "GetPrivateProfileSectionNamesA" ( _
	'	 ByVal lpReturnedString As String, _ 'Возвращаемое значение
	'	 ByVal nSize As Integer, _ 'Максимальный размер возвращаемого значения
	'	 ByVal lpFileName As String) As Integer 'путь к ini-файлу
#End Region

#Region "Private Variables"
	Private L, R As String
	Const DefaultBrackets As String = "[]"
	Const DefaultSeparatop As String = "="
	Private MainINI As Dictionary(Of String, Dictionary(Of String, String)) = Nothing
#End Region

#Region "Public Variables"
	Public File_Name As String = Nothing
	Public AutoSaveFile As Boolean = False
	Public SectionBrackets As String = Nothing
	Public ParameterSeparator As String = Nothing
#End Region

#Region "Private Subs & Functions"

	Sub New(Optional ByVal [FileName] As String = Nothing, Optional ByVal [AutoSave] As Boolean = False, Optional ByVal [newBrackets] As String = DefaultBrackets, Optional ByVal [newSeparator] As String = DefaultSeparatop)
		If [newBrackets].Trim.Length <> 0 And [newBrackets].Trim.Length Mod 2 = 0 Then
			L = Strings.Left([newBrackets], [newBrackets].Length / 2)
			R = Strings.Right([newBrackets], [newBrackets].Length / 2)
			SectionBrackets = [newBrackets]
		Else
			MsgBox("Section Brackets must have an even number of chars.", MsgBoxStyle.Exclamation)
			Exit Sub
		End If

		If [newSeparator] IsNot Nothing Then
			ParameterSeparator = [newSeparator]
		Else
			ParameterSeparator = DefaultSeparatop
		End If

		File_Name = [FileName]
		AutoSaveFile = [AutoSave]
		MainINI = New Dictionary(Of String, Dictionary(Of String, String))
	End Sub

	Private Function CreateDictionary(ByVal _Filename As String, Optional ByVal [newBrackets] As String = DefaultBrackets, Optional ByVal [newSeparator] As String = DefaultSeparatop) As Dictionary(Of String, Dictionary(Of String, String))
		Dim TemDictiorary As New Dictionary(Of String, Dictionary(Of String, String))
		Dim OpenedFile As IO.StreamReader = Nothing
		Try
			OpenedFile = New IO.StreamReader(_Filename, True)
			Dim SectionKey As String = Nothing, ParamKey As String = Nothing, ParamValue As String = Nothing
			Dim FileLine As String = Nothing : Dim SepN As Integer = Nothing

			Application.DoEvents()
			Do While Not OpenedFile.EndOfStream
				FileLine = OpenedFile.ReadLine.ToString.Trim
				'Проверяем тип читаемой строки
				If Not FileLine.Trim Is Nothing And ( _
				Not FileLine.Trim.StartsWith([newSeparator]) Or _
				  Not FileLine.Trim.StartsWith(";") Or _
				  Not FileLine.Trim.StartsWith("\") Or _
				  Not FileLine.Trim.StartsWith("/")) Then

					'Проверяем строку на наличие в ней идентификаторов Секции
					If FileLine.Trim.StartsWith(L) And FileLine.Trim.EndsWith(R) Then

						'Получаем имя секции
						SectionKey = FileLine.Trim.Substring(L.Length, FileLine.Length - [newBrackets].Length).ToString

                        'Проверяем имя секции на совпадения в нашем словаре, and if missing - ADD NEW SECTION
                        TemDictiorary.ContainsKey(SectionKey)
                        If Not TemDictiorary.ContainsKey(SectionKey) Then
                            TemDictiorary.Add(SectionKey, New Dictionary(Of String, String))
                        End If

                        'Читаем Раздел секции
                    Else
						If SectionKey IsNot Nothing Then
							SepN = FileLine.IndexOf([newSeparator].ToString, 0)

							'Skip Lines with Wrong Format
							If SepN < 0 Then GoTo WrongLineFound_SKIP

							'Получаем имя параметра
							ParamKey = FileLine.Substring(0, SepN)

							'Получаем значение(содержимое) параметра
							ParamValue = FileLine.Substring(SepN + [newSeparator].Length)

                            'Продолжаем запись в открытую секцию

                            If Not TemDictiorary.Item(SectionKey).ContainsKey(ParamKey) Then
                                TemDictiorary.Item(SectionKey).Add(ParamKey, ParamValue)
                            End If
                        End If
					End If
				End If

WrongLineFound_SKIP:

				'Если конец файла, выходим из Do...Loop
			Loop 'While Not OpenedFile.EndOfStream


			'Закрываем файл
			OpenedFile.Close()
			OpenedFile.Dispose()
			GC.Collect()
			Return TemDictiorary


		Catch ex As Exception  'Информируем об ошибке
			MsgBox("Error in Reading file """ & IO.Path.GetFileName(_Filename) & Chr(34) & _
			 vbNewLine & "-----DESCRIPTION-----" & vbNewLine & ex.ToString, MsgBoxStyle.Critical)
			OpenedFile.Close()
			OpenedFile.Dispose()
		End Try
		GC.Collect()
		Return Nothing
	End Function
#End Region

#Region "Public Subs & Functions"
	Public Function LoadFile(Optional ByVal [New_FileName] As String = Nothing, Optional ByVal [NewFile_Question] As Boolean = False) As Boolean
		If [New_FileName] Is Nothing Then
			If File_Name IsNot Nothing Then
				[New_FileName] = File_Name
			Else
				MsgBox("File Name must not be Empty.", MsgBoxStyle.Exclamation)
				Return False
			End If
		Else
			If File_Name Is Nothing Then
				File_Name = [New_FileName]
			Else
				If [NewFile_Question] Then
					'Спросим пользователя, что делать с уже загруженным файлом?
					Select Case MsgBox("You are about to load a new file." & vbCrLf & "Not saved changes will be lost.!" _
					 & vbCrLf & vbCrLf & "Continue.?", MsgBoxStyle.Question + MsgBoxStyle.YesNo)
						Case MsgBoxResult.Yes
							File_Name = [New_FileName]
						Case MsgBoxResult.No
							Return False
					End Select
				Else
					File_Name = [New_FileName]
				End If
			End If
		End If

		If IO.File.Exists([New_FileName]) Then
			MainINI = CreateDictionary([New_FileName], SectionBrackets, ParameterSeparator)
		Else
			MsgBox("File """ & [New_FileName] & """ not found.", MsgBoxStyle.Exclamation)
			Return False
		End If
		Return True
	End Function

	Public Function Get_Value(ByVal SectionKey As String, ByVal ParameterKey As String, Optional ByVal [Default] As String = Nothing) As String
		If MainINI IsNot Nothing Then
            If MainINI.ContainsKey(SectionKey) Then
                If MainINI.Item(SectionKey).ContainsKey(ParameterKey) Then
                    Try
                        If Left(MainINI.Item(SectionKey).Item(ParameterKey).ToString, 1) = ";" Then
                            Return [Default]
                        End If
                        Return MainINI.Item(SectionKey).Item(ParameterKey).ToString
                    Catch
                        Return [Default]
                    End Try
                End If
            End If
        End If
		Return [Default]
	End Function

	Public Sub Set_Value(ByVal SectionKey As String, ByVal ParameterKey As String, ByVal Value As String)
		If MainINI IsNot Nothing Then
            If MainINI.ContainsKey(SectionKey) Then
                If MainINI.Item(SectionKey).ContainsKey(ParameterKey) Then
                    MainINI.Item(SectionKey).Item(ParameterKey) = Value
                    If AutoSaveFile Then SaveFile()
                Else
                    MainINI.Item(SectionKey).Add(ParameterKey, Value)
                    If AutoSaveFile Then SaveFile()
                End If
            Else
                MainINI.Add(SectionKey, New Dictionary(Of String, String) From {{ParameterKey, Value}})
                If AutoSaveFile Then SaveFile()
            End If
        End If
    End Sub

    Public Function Get_ListOf_SectionKeys(Optional ByVal [Sorted] As Boolean = False, Optional ByVal [Default] As Object = Nothing) As Array
        If MainINI IsNot Nothing Then
            Dim Result As New ArrayList
            Try
                For Each SectionKey In MainINI.Keys
                    Result.Add(SectionKey)
                Next
                If [Sorted] Then Array.Sort(Result.ToArray)
                Return Result.ToArray
            Catch
                Return [Default]
            End Try
        End If
        Return [Default]
    End Function

    Public Function Get_ListOf_Parameters(ByVal SectionKey As String, Optional ByVal [Sorted] As Boolean = False, Optional ByVal [Default] As Object = Nothing) As Array
        If MainINI IsNot Nothing Then
            If MainINI.ContainsKey(SectionKey) Then
                Dim Result As New ArrayList
                Try
                    For Each ParamKey In MainINI.Item(SectionKey).Keys
                        If Left(ParamKey, 1) = ";" Then
                            Continue For
                        End If
                        Result.Add(ParamKey)
                    Next
                    If [Sorted] Then Array.Sort(Result.ToArray)
                    Return Result.ToArray
                Catch
                    Return [Default]
                End Try
            Else
                Return [Default]
            End If
        End If
        Return [Default]
    End Function

    Public Function SectionsCount() As Integer
        If MainINI IsNot Nothing Then Return MainINI.Keys.Count
        Return Nothing
    End Function

    Public Function ParametersCount(Optional ByVal [SectionKey] As String = Nothing) As Integer
        Dim CountValue As Integer = Nothing
        If MainINI IsNot Nothing Then
            If [SectionKey] IsNot Nothing Then
                If MainINI.ContainsKey([SectionKey]) Then
                    CountValue = MainINI.Item([SectionKey]).Keys.Count
                End If
            Else
                For Each Section As String In MainINI.Keys
                    CountValue += MainINI.Item(Section).Keys.Count
                Next
            End If
        End If
        Return CountValue
    End Function

    Public Sub Rename_Section(ByVal Old_SectionName As String, ByVal New_SectionName As String)
        If MainINI IsNot Nothing Then
            If MainINI.ContainsKey(Old_SectionName) Then
                Try
                    Dim OldContent As Dictionary(Of String, String) = MainINI(Old_SectionName)
                    MainINI.Remove(Old_SectionName)
                    MainINI.Add(New_SectionName, OldContent)
                    If AutoSaveFile Then SaveFile()
                Catch : End Try
            Else
                MainINI.Add(New_SectionName, New Dictionary(Of String, String))
                If AutoSaveFile Then SaveFile()
            End If
        End If
    End Sub

    Public Sub Rename_Parameter(ByVal SectionKey As String, ByVal Old_ParameterName As String, ByVal New_ParameterName As String)
        If MainINI IsNot Nothing Then
            If MainINI.ContainsKey(SectionKey) Then
                If MainINI.Item(SectionKey).ContainsKey(Old_ParameterName) Then
                    Try
                        Dim OldContent As String = MainINI.Item(SectionKey).Item(Old_ParameterName)
                        MainINI.Item(SectionKey).Remove(Old_ParameterName)
                        MainINI.Item(SectionKey).Add(New_ParameterName, OldContent)
                        If AutoSaveFile Then SaveFile()
                    Catch : End Try
                Else
                    MainINI.Item(SectionKey).Add(New_ParameterName, Nothing)
                    If AutoSaveFile Then SaveFile()
                End If
            Else
                MainINI.Add(SectionKey, New Dictionary(Of String, String) From {{New_ParameterName, Nothing}})
                If AutoSaveFile Then SaveFile()
            End If
        End If
    End Sub

    Public Sub Delete_Section(ByVal SectionName As String)
        If MainINI IsNot Nothing Then
            If MainINI.ContainsKey(SectionName) Then
                Try
                    MainINI.Remove(SectionName)
                    If AutoSaveFile Then SaveFile()
                Catch : End Try
            End If
        End If
    End Sub

    Public Sub Delete_Parameter(ByVal SectionKey As String, ByVal ParameterKey As String)
        If MainINI IsNot Nothing Then
            If MainINI.ContainsKey(SectionKey) Then
                If MainINI.Item(SectionKey).ContainsKey(ParameterKey) Then
                    Try
                        MainINI.Item(SectionKey).Remove(ParameterKey)
                        If AutoSaveFile Then SaveFile()
                    Catch : End Try
                End If
            End If
        End If
    End Sub

    Public Sub Move_Parameter_FromTo(ByVal ParameterKey As String, ByVal SectionFROM As String, ByVal SectionTO As String)
        If MainINI IsNot Nothing Then
            If MainINI.ContainsKey(SectionFROM) Then
                If MainINI.Item(SectionFROM).ContainsKey(ParameterKey) Then
                    Try
                        Dim OldContent As String = MainINI.Item(SectionFROM).Item(ParameterKey)
                        MainINI.Item(SectionFROM).Remove(ParameterKey)
                        If MainINI.ContainsKey(SectionTO) Then
                            MainINI.Item(SectionTO).Add(ParameterKey, OldContent)
                        Else
                            MainINI.Add(SectionTO, New Dictionary(Of String, String) From {{ParameterKey, OldContent}})
                        End If
                        If AutoSaveFile Then SaveFile()
                    Catch
                    End Try
                End If
            End If
		End If
	End Sub

	Public Function SaveFile(Optional ByVal [New_File_Name] As String = Nothing, Optional ByVal [newBrackets] As String = Nothing, Optional ByVal [newSeparator] As String = Nothing) As Boolean

		If [New_File_Name] Is Nothing Then
			If File_Name IsNot Nothing Then
				[New_File_Name] = File_Name
			Else
				MsgBox("File Name must not be Empty.", MsgBoxStyle.Exclamation)
				Return False
			End If
		End If

		Dim TempLEFT As String
		Dim TempRIGHT As String

		'Определяем Брэкеты для секции [Секция]
		If [newBrackets] Is Nothing Then
			If SectionBrackets IsNot Nothing Then
				[newBrackets] = SectionBrackets
				TempLEFT = L : TempRIGHT = R
			Else
				[newBrackets] = DefaultBrackets
				If [newBrackets].Trim.Length <> 0 And [newBrackets].Trim.Length Mod 2 = 0 Then
					TempLEFT = Strings.Left([newBrackets], [newBrackets].Length / 2)
					TempRIGHT = Strings.Right([newBrackets], [newBrackets].Length / 2)
				Else
					MsgBox("Cannot save the file. The Default Section Brackets you set must have an even number of chars. ", MsgBoxStyle.Exclamation)
					Return False
				End If
			End If
		Else
			If [newBrackets].Trim.Length <> 0 And [newBrackets].Trim.Length Mod 2 = 0 Then
				TempLEFT = Strings.Left([newBrackets], [newBrackets].Length / 2)
				TempRIGHT = Strings.Right([newBrackets], [newBrackets].Length / 2)
			Else
				MsgBox("Cannot save the file. The Section Brackets you set must have an even number of chars. ", MsgBoxStyle.Exclamation)
				Return False
			End If
		End If

		'Определяем сепарацию между Параметром и Значением
		If [newSeparator] Is Nothing Then
			If ParameterSeparator IsNot Nothing Then
				[newSeparator] = ParameterSeparator
			Else
				[newSeparator] = DefaultSeparatop
			End If
		End If

		Dim sFile As IO.StreamWriter = Nothing

		Try	'Открытие файла для записи
			sFile = New IO.StreamWriter([New_File_Name], False, System.Text.Encoding.UTF8)
			sFile.AutoFlush = True
		Catch ex As Exception
			MsgBox("Error during processing of the file at following location:" _
			& vbNewLine & vbNewLine & [New_File_Name] & vbNewLine & vbNewLine & _
			ex.ToString, MsgBoxStyle.Critical)
			sFile.Close()
			Return False
		End Try

		Try	'Вывод данных в файл
			If MainINI.Keys.Count > 0 Then
				For Each SectionKey As String In MainINI.Keys
					sFile.WriteLine(TempLEFT & SectionKey.Replace(vbCrLf, " ") & TempRIGHT)
					If MainINI.Item(SectionKey).Keys.Count > 0 Then
						For Each ParamKey As String In MainINI.Item(SectionKey).Keys
							Dim Value As String = MainINI.Item(SectionKey).Item(ParamKey)
							sFile.WriteLine(ParamKey & [newSeparator] & Value.Replace(vbCrLf, " "))
						Next
					End If
				Next
			End If
		Catch ex As Exception
			MsgBox("Error during processing of the file at following location:" _
			& vbNewLine & vbNewLine & [New_File_Name] & vbNewLine & vbNewLine & _
			ex.ToString, MsgBoxStyle.Critical)
			sFile.Close()
			Return False
		End Try

		'Закрываем файл
		sFile.Close()
		Return True
	End Function
#End Region

End Class



