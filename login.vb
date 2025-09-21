# login page to can Admin login 


Imports MySql.Data.MySqlClient
Imports System.Windows.Forms
Imports System.Drawing

Public Class Login
    Private Sub Login_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        dbconn()
        acsconn.Open()


    End Sub

    Private Sub Login_Paint(sender As Object, e As PaintEventArgs) Handles Me.Paint

        Dim borderColor As Color = Color.Silver
        Dim borderWidth As Integer = 1


        ControlPaint.DrawBorder(e.Graphics, Me.ClientRectangle,
                                borderColor, borderWidth, ButtonBorderStyle.Solid,
                                borderColor, borderWidth, ButtonBorderStyle.Solid,
                                borderColor, borderWidth, ButtonBorderStyle.Solid,
                                borderColor, borderWidth, ButtonBorderStyle.Solid)
    End Sub

    Private Sub btn_close_Click(sender As Object, e As EventArgs)
        Me.Close()

    End Sub



    Private Sub hidepass_MouseHover(sender As Object, e As EventArgs) Handles hidepass.MouseHover
        hidepass.Cursor = Cursors.Hand
        txtpassword.PasswordChar = ""
    End Sub

    Private Sub hidepass_MouseLeave(sender As Object, e As EventArgs) Handles hidepass.MouseLeave
        hidepass.Cursor = Cursors.Default
        txtpassword.PasswordChar = "*"
    End Sub

    ''Private Sub Login_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
    ''    If e.KeyCode = Keys.Enter Then
    ''        ' Enter press hone par next control pe focus bhej do
    ''        Me.SelectNextControl(Me.ActiveControl, True, True, True, True)
    ''        e.SuppressKeyPress = True ' Enter ki default beep sound hatane ke liye
    ''    End If
    ''End Sub
    Protected Overrides Function ProcessCmdKey(ByRef msg As Message, keyData As Keys) As Boolean
        If keyData = Keys.Enter Then
            ' Enter press hone par next control select karo
            Me.SelectNextControl(Me.ActiveControl, True, True, True, True)
            Return True ' Enter ka default action block karna
        End If
        Return MyBase.ProcessCmdKey(msg, keyData)
    End Function

    Private Sub btn_login_Click(sender As Object, e As EventArgs) Handles btn_login.Click
        Try
            If acsconn.State = ConnectionState.Closed Then
                acsconn.Open()

            End If
            Dim cmd As New MySqlCommand("Select Count(*) From AdminLogin Where AdminId=@id AND AdminPassword=@pass", acsconn)

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@id", txtuserid.Text.Trim())
            cmd.Parameters.AddWithValue("@pass", txtpassword.Text.Trim())
            Dim count As Integer = Convert.ToInt32(cmd.ExecuteScalar())
            If count > 0 Then
                'MsgBox("Login Successful !", vbInformation)
                Me.Hide()

                Student_Information.ShowDialog()


            Else
                MsgBox("Invalid ID or Password", vbExclamation)
            End If
        Catch ex As Exception
            MsgBox("Error:" & ex.Message, vbCritical)
        Finally
            acsconn.Close()

        End Try
    End Sub

    Private Sub btnreset_Click(sender As Object, e As EventArgs) Handles btnreset.Click
        txtuserid.Text = ""
        txtpassword.Text = ""
    End Sub
End Class
