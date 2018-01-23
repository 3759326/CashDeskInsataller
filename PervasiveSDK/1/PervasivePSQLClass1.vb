Imports System
Imports System.Data.Common
Imports System.Text
Imports Pervasive.Data.SqlClient

'
' Pervasive ADO.NET PSQL data provider template
'

Namespace $safeprojectname$

    ''' <summary>
    '''  Summary description for $safeitemrootname$.
    ''' </summary>
    Public Class $safeitemrootname$

        Public Shared Sub Main()

            'TODO: Insert your Connection String options
            Dim ConnectionString As String = ""
            $GeneratedPerfWizardOptions$
            Dim connection As PsqlConnection = New PsqlConnection()
            $ConnectionString$
            connection.ConnectionString = ConnectionString$Concatenate$

            'TODO: Insert your application code here

        End Sub

    End Class

End Namespace