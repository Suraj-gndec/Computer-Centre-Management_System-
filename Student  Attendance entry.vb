Imports MySql.Data.MySqlClient

Public Class Add_Attendence


    Private Sub Add_Attendence_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        dbconn()
    End Sub

    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        'Try
        '    Dim searchstudent As String = txtsearchstudent.Text.Trim()
        '    If searchstudent = " " Then
        '        MsgBox("Please Enter Student ID.", vbExclamation)
        '        Exit Sub
        '    End If


        '    If acsconn.State = ConnectionState.Closed Then
        '        acsconn.Open()
        '    End If

        '    ' query jo id se match kare
        '    Dim searchbyid As String = "SELECT * FROM students_records WHERE student_id=@id"
        '    Dim cmdshow As New MySqlCommand(searchbyid, acsconn)
        '    cmdshow.Parameters.AddWithValue("@id", searchstudent)
        '    Dim dr As MySqlDataReader = cmdshow.ExecuteReader()
        '    If dr.Read() Then
        '        dr.Close()
        '        loadstudent()

        '    Else
        '        MsgBox("No student found with this ID.", vbInformation)
        '    End If



        'Catch ex As Exception
        'Finally
        '    If acsconn.State = ConnectionState.Open Then
        '        acsconn.Close()
        '    End If
        'End Try

        Try
            Dim searchstudent As String = txtsearchstudent.Text.Trim()
            If searchstudent = "" Then
                MsgBox("Please Enter Student ID.", vbExclamation)
                Exit Sub
            End If


            If acsconn.State = ConnectionState.Closed Then
                acsconn.Open()
            End If

            Dim searchbyid As String = "SELECT * FROM students_records WHERE student_id=@id"
            Dim cmdshow As New MySqlCommand(searchbyid, acsconn)
            cmdshow.Parameters.AddWithValue("@id", searchstudent)
            Dim dr As MySqlDataReader = cmdshow.ExecuteReader()
            If dr.Read() Then
                dr.Close()
                ' sirf usi student ka data grid me dikhana
                loadstudent(searchstudent)
            Else
                MsgBox("No student found with this ID.", vbInformation)
            End If

        Catch ex As Exception
            MsgBox("Error: " & ex.Message, vbCritical)
        Finally
            If acsconn.State = ConnectionState.Open Then
                acsconn.Close()
            End If
        End Try
    End Sub
    'Private Sub loadstudent()
    '    Try
    '        search_student_show.Rows.Clear()
    '        ' dbconn()
    '        If acsconn.State = ConnectionState.Closed Then
    '            acsconn.Open()
    '        End If

    '        Dim showdata As String = "SELECT student_id, name, course,batch FROM students_records"
    '        Dim cmdshow As New MySqlCommand(showdata, acsconn)
    '        Dim dr As MySqlDataReader = cmdshow.ExecuteReader()

    '        While dr.Read()
    '            search_student_show.Rows.Add(dr("student_id").ToString(),
    '                                         dr("name").ToString(),
    '                                         dr("course").ToString(),
    '                                         dr("batch").ToString())
    '        End While
    '        dr.Close()

    '        Dim copyandadd_attendence As String = "INSERT INTO st_attendance(student_id,name,course,batch) SELECT student_id,name,course,batch FROM students_records;"
    '        Using cmd As New MySqlCommand(copyandadd_attendence, acsconn)

    '            Dim count As Integer = cmd.ExecuteNonQuery()
    '            'If count > 0 Then




    '            'Else
    '            'End If

    '        End Using

    '    Catch ex As Exception
    '        MsgBox("Error loading data: " & ex.Message, vbCritical)
    '    Finally
    '        If acsconn.State = ConnectionState.Open Then
    '            acsconn.Close()
    '        End If
    '    End Try
    'End Sub
    Private Sub loadstudent(ByVal studentId As String)
        Try
            search_student_show.Rows.Clear()
            dbconn()
            If acsconn.State = ConnectionState.Closed Then
                acsconn.Open()
            End If

            ' Sirf wahi student jiska ID match ho
            Dim showdata As String = "SELECT student_id, name, course, batch 
                                  FROM students_records 
                                  WHERE student_id=@id"
            Dim cmdshow As New MySqlCommand(showdata, acsconn)
            cmdshow.Parameters.AddWithValue("@id", studentId)
            Dim dr As MySqlDataReader = cmdshow.ExecuteReader()

            While dr.Read()
                search_student_show.Rows.Add(dr("student_id").ToString(),
                                         dr("name").ToString(),
                                         dr("course").ToString(),
                                         dr("batch").ToString())
            End While

        Catch ex As Exception
            MsgBox("Error loading data: " & ex.Message, vbCritical)
        Finally
            If acsconn.State = ConnectionState.Open Then
                acsconn.Close()
            End If
        End Try
    End Sub

    Private Sub txtsearchstudent_Click(sender As Object, e As EventArgs) Handles txtsearchstudent.Click
        txtsearchstudent.Text = ""
        search_student_show.Rows.Clear()

    End Sub

    Private Sub btn_save_attendence_Click(sender As Object, e As EventArgs) Handles btn_save_attendence.Click
        Try

            If acsconn.State = ConnectionState.Closed Then
                acsconn.Open()
            End If
            If attendence_add.Text = " " Then
                MsgBox("Please Enter Attendance Status.", vbExclamation)
                Exit Sub
            End If


            Dim add_atd As String = "INSERT INTO student_attendance (st_id,st_name, st_course, st_batch, st_attendance_date, st_daily_attendance)
                                 SELECT student_id, name, course, batch, @st_attendance_date, @st_daily_attendance
                                 FROM students_records
                                 WHERE student_id = @id;"

            Using cmd2 As New MySqlCommand(add_atd, acsconn)
                cmd2.Parameters.AddWithValue("@id", txtsearchstudent.Text.Trim())
                cmd2.Parameters.AddWithValue("@st_attendance_date", attendence_date.Value.ToString("yyyy-MM-dd"))
                cmd2.Parameters.AddWithValue("@st_daily_attendance", attendence_add.Text)
                Dim dbcount As Integer = cmd2.ExecuteNonQuery()
                If dbcount > 0 Then
                    attendence_add.Text = ""
                    MsgBox("Attendance added Successfully!", vbExclamation)
                Else
                    MsgBox("Failed to add attendence.", vbExclamation)

                End If
            End Using
        Catch ex As Exception
            MsgBox("Error: " & ex.Message, vbCritical)
        Finally
            If acsconn.State = ConnectionState.Open Then
                acsconn.Close()
            End If
        End Try
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Me.Hide()
        Show_Attendance.Show()

    End Sub
End Class
