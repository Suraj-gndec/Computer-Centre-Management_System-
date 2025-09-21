Imports MySql.Data.MySqlClient
Public Class Show_Attendance



    Private Sub Show_Attendance_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        dbconn()
        txtsearchstudent.Focus()
        txtsearchstudent.TabIndex = 1

    End Sub

    Private Sub btnShow_atds_Click(sender As Object, e As EventArgs) Handles btnShow_atds.Click
        Try
            Dim searchstudent As String = txtsearchstudent.Text.Trim()
            If searchstudent = "" Then
                MsgBox("Please Enter Student ID.", vbExclamation)
                Exit Sub
            End If
            'Dim searchbyid
            If acsconn.State = ConnectionState.Closed Then
                acsconn.Open()
            End If
            Dim searchbystid As String = "SELECT * FROM student_attendance WHERE st_id=@id"
            Dim cmdshow As New MySqlCommand(searchbystid, acsconn)
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

        End Try
    End Sub
    Private Sub loadstudent(ByVal studentId As String)
        Try
            showall_attendance.Rows.Clear()

            If acsconn.State = ConnectionState.Closed Then
                acsconn.Open()
            End If

            ' Sirf wahi student jiska ID match ho
            Dim showdata As String = "SELECT st_id, st_name, st_course,st_attendance_date,st_daily_attendance
                                  FROM student_attendance 
                                  WHERE st_id LIKE @id AND st_attendance_date BETWEEN @fromDate AND @toDate"
            Dim cmdshow As New MySqlCommand(showdata, acsconn)
            cmdshow.Parameters.AddWithValue("@id", studentId)
            cmdshow.Parameters.AddWithValue("@fromDate", st_startdate.Value.Date)
            cmdshow.Parameters.AddWithValue("@toDate", st_enddate.Value.Date)

            Dim dr As MySqlDataReader = cmdshow.ExecuteReader()

            While dr.Read()
                showall_attendance.Rows.Add(dr("st_id").ToString(),
                                         dr("st_name").ToString(),
                                         dr("st_course").ToString(),
                                        Convert.ToDateTime(dr("st_attendance_date")).ToString("yyyy-MM-dd"),
                                         dr("st_daily_attendance").ToString())

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
        st_startdate.Value = DateTime.Now
        st_enddate.Value = DateTime.Now
        showall_attendance.Rows.Clear()

    End Sub
End Class
