Imports System
Imports System.Data.Common
Imports System.Text

'
' Pervasive ADO.NET data provider template
' ADO.NET 2.0 Common Programming Model

Namespace $safeprojectname$

    ''' <summary>
    '''  Summary description for $safeitemrootname$.
    ''' </summary>
    Public Class $safeitemrootname$

        Public Shared Sub Main()

            'TODO: Insert your Connection String options
            Dim ConnectionString As String = ""
            $GeneratedPerfWizardOptions$
            'TODO: Add provider invariant name            
            Dim factory As DbProviderFactory = DbProviderFactories.GetFactory("")
            Dim connection As DbConnection = factory.CreateConnection()
            $ConnectionString$
            connection.ConnectionString = ConnectionString$Concatenate$

            'TODO: Insert your application code here

        End Sub

    End Class

End Namespace