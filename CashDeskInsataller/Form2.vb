


Public Class frmArchive
    Dim pathSorces = "C:\Fexpert"
    Dim pathDest = Application.StartupPath & "\Archive\"
    Dim archName = "fexpert_" & Replace(DateAndTime.DateString, "-", "_") & ".zip"

    Private Sub frmArchive_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        txtSources.Text = pathSorces
        txtDest.Text = pathDest & archName
    End Sub

    Private Sub frmArchive_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        frmStart.Show()

    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Dim p() As Process
        Logining(SrvControl("Pervasive.SQL (relational)", "Stop"), logstatus)
        Logining(SrvControl("Pervasive.SQL (transactional)", "Stop"), logstatus)
        Logining(KillProc("TOTALCMD64"), logstatus)


        'Dim i = 0
        'Do While i <> 100
        '    Logining("kjsgfvsfagf", i)
        '    i += 1
        'Loop
        'Logining("kjsgfvsfagf", "Успех")





    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click

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
        MsgBox(OpenFileDialog1.FileName)
        pathDest = OpenFileDialog1.FileName
        txtDest.Text = pathDest



    End Sub
    Private Sub Logining(eventName As String, eventStatus As String)
        Dim listit As New ListViewItem(eventName)
        listit.UseItemStyleForSubItems = False
        listit.SubItems.Add(eventStatus)
        Try
            If eventStatus Mod 2 = 0 Then
                listit.SubItems(1).ForeColor = Color.Blue
            Else
                listit.SubItems(1).ForeColor = Color.Red

            End If
        Catch
            listit.SubItems(1).ForeColor = Color.Black

        End Try





        ListView1.Items.Add(listit)
        ListView1.EnsureVisible(ListView1.Items.Count - 1)
    End Sub

    Private Sub ListView1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListView1.SelectedIndexChanged

    End Sub
End Class