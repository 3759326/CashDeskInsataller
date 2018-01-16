Public Class frmStart
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        frmArchive.Show()
        Me.Hide()




    End Sub

    Private Sub Button1_MouseHover(sender As Object, e As EventArgs) Handles Button1.MouseHover
        Label1.Text = "Создание мастер-образа (архива дирекотрии Fexpert) на полностью настроеной кассе"
    End Sub

    Private Sub Button1_MouseLeave(sender As Object, e As EventArgs) Handles Button1.MouseLeave
        Label1.Text = ""
    End Sub

    Private Sub Button2_MouseHover(sender As Object, e As EventArgs) Handles Button2.MouseHover
        Label1.Text = "Установка всех требуемых программ и утилит, разворачивание мастер-образа кассы с очисткой директорий SHOP, ShopOffC, Log и настройкой ini файлов"
    End Sub

    Private Sub Button2_MouseLeave(sender As Object, e As EventArgs) Handles Button2.MouseLeave
        Label1.Text = ""
    End Sub

    Private Sub Button3_MouseHover(sender As Object, e As EventArgs) Handles Button3.MouseHover
        Label1.Text = "Разворачивание мастер-образа кассы с очисткой директорий SHOP, ShopOffC, Log и настройкой ini файлов"
    End Sub

    Private Sub Button3_MouseLeave(sender As Object, e As EventArgs) Handles Button3.MouseLeave
        Label1.Text = ""
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        frmInstall.Show()
        Me.Hide()

    End Sub
End Class
