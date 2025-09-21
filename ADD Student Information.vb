

Imports MySql.Data.MySqlClient

Public Class Student_Information
    Private Sub Student_Information_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        dbconn()
        'acsconn.Open()'

    End Sub

    Private Sub btnclear_Click(sender As Object, e As EventArgs) Handles btnclear.Click
        txtstdname.Text = ""
        txtstdfathername.Text = ""
        txtstdmothername.Text = ""
        txtaadharnumber.Text = ""
        txtmobilenumber.Text = ""
        txtqualification.Text = ""
        txtaddress.Text = ""
        txtstudentid.Text = ""
        txtcourse.SelectedIndex = -1
        txtcourse.Text = "Student Course"
        txtcourseduration.SelectedIndex = -1
        txtcourseduration.Text = " Course Duration"
        txtbatch.SelectedIndex = -1
        txtbatch.Text = "Student Batch"
        txtdateofbirth.Value = DateTime.Now
        gendermale.Checked = False
        genderfemale.Checked = False
        gendertransgender.Checked = False
    End Sub


    Private Sub btn_save_Click(sender As Object, e As EventArgs) Handles btn_save.Click


        If txtstdname.Text = "" Then
            MsgBox("Fill Student Name.")
            Exit Sub
        ElseIf txtstdfathername.Text = "" Then
            MsgBox("Fill Father Name.")
            Exit Sub
        ElseIf txtstdmothername.Text = "" Then
            MsgBox("Fill Mother Name.")
            Exit Sub
        ElseIf txtaadharnumber.Text = "" Then
            MsgBox("Fill Aadhar Number.")
            Exit Sub
        ElseIf txtaddress.Text = "" Then
            MsgBox("Fill Student Address.")
            Exit Sub
        ElseIf txtmobilenumber.Text = "" Then
            MsgBox("Enter Mobile Number.")
            Exit Sub
        ElseIf txtqualification.Text = "" Then
            MsgBox("Enter Student Qualification.")
            Exit Sub
        ElseIf txtstudentid.Text = "" Then
            MsgBox("Enter Student ID.")
            Exit Sub
        ElseIf txtdateofbirth.Value.Date = Date.Today Then
            MsgBox("Please select your Date of Birth.")
            Exit Sub
        ElseIf txtcourse.SelectedIndex = -1 Then
            MsgBox("Please select your Course.")
            Exit Sub
        ElseIf txtcourseduration.SelectedIndex = -1 Then
            MsgBox("Please select Course Duration.")
            Exit Sub
        ElseIf txtbatch.SelectedIndex = -1 Then
            MsgBox("Please select Batch Time.")
            Exit Sub
        ElseIf Not (gendermale.Checked Or genderfemale.Checked Or gendertransgender.Checked) Then
            MsgBox("Please select Gender.")
            Exit Sub
        End If

        ' Step 2: Determine Gender
        Dim gender As String = If(gendermale.Checked, "Male",
                            If(genderfemale.Checked, "Female", "Transgender"))

        ' Step 3: Save to Database
        Try
            If acsconn.State = ConnectionState.Closed Then
                acsconn.Open()
            End If


            ' 🔍 Database me check karo student exist karta hai ya nahi
            Dim existsQuery As String = "SELECT COUNT(*) FROM students_records WHERE student_id=@id"
            Dim cmdCheck As New MySqlCommand(existsQuery, acsconn)
            cmdCheck.Parameters.AddWithValue("@id", txtstudentid.Text.Trim())
            Dim recordExists As Integer = Convert.ToInt32(cmdCheck.ExecuteScalar())

            Dim query As String

            If recordExists > 0 Then
                ' ----------------update karne ke liye ----------------
                query = "UPDATE students_records SET 
                            name=@name, father_name=@father_name, mother_name=@mother_name, dob=@dob, 
                            gender=@gender, aadhar_no=@aadhar_no, mobile_no=@mobile_no, qualification=@qualification, 
                            course=@course, duration=@duration, batch=@batch, address=@address 
                         WHERE student_id=@student_id"
            Else
                ' ---------------- insert karne ke liye----------------
                query = "INSERT INTO students_records 
                        (student_id, name, father_name, mother_name, dob, gender, aadhar_no, mobile_no, qualification, 
                        course, duration, batch, address, created_at) 
                        VALUES 
                        (@student_id, @name, @father_name, @mother_name, @dob, @gender, @aadhar_no, @mobile_no, 
                        @qualification, @course, @duration, @batch, @address, NOW())"
            End If


            Using cmd As New MySqlCommand(query, acsconn)
                cmd.Parameters.AddWithValue("@student_id", txtstudentid.Text)
                cmd.Parameters.AddWithValue("@name", txtstdname.Text)
                cmd.Parameters.AddWithValue("@father_name", txtstdfathername.Text)
                cmd.Parameters.AddWithValue("@mother_name", txtstdmothername.Text)
                cmd.Parameters.AddWithValue("@dob", txtdateofbirth.Value.ToString("yyyy-MM-dd"))
                cmd.Parameters.AddWithValue("@gender", gender)
                cmd.Parameters.AddWithValue("@aadhar_no", txtaadharnumber.Text)
                cmd.Parameters.AddWithValue("@mobile_no", txtmobilenumber.Text)
                cmd.Parameters.AddWithValue("@qualification", txtqualification.Text)
                cmd.Parameters.AddWithValue("@course", txtcourse.Text)
                cmd.Parameters.AddWithValue("@duration", txtcourseduration.Text)
                cmd.Parameters.AddWithValue("@batch", txtbatch.Text)
                cmd.Parameters.AddWithValue("@address", txtaddress.Text)

                Dim count As Integer = cmd.ExecuteNonQuery()

                If count > 0 Then
                    MsgBox("Student added successfully!", vbInformation)
                    txtstdname.Text = ""
                    txtstdfathername.Text = ""
                    txtstdmothername.Text = ""
                    txtaadharnumber.Text = ""
                    txtmobilenumber.Text = ""
                    txtqualification.Text = ""
                    txtaddress.Text = ""
                    txtstudentid.Text = ""
                    txtcourse.SelectedIndex = -1
                    txtcourse.Text = "Student Course"
                    txtcourseduration.SelectedIndex = -1
                    txtcourseduration.Text = " Course Duration"
                    txtbatch.SelectedIndex = -1
                    txtbatch.Text = "Student Batch"
                    txtdateofbirth.Value = DateTime.Now
                    gendermale.Checked = False
                    genderfemale.Checked = False
                    gendertransgender.Checked = False
                Else
                    MsgBox("Failed to add student record.", vbExclamation)
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

    Private Sub btnview_Click(sender As Object, e As EventArgs) Handles btnview.Click
        Me.Hide()
        Dim frm As New Manage_Students()  ' 👈 yahan new object bana
        frm.ShowDialog()
        'Me.Show()
    End Sub

    Private Sub GroupBox1_Enter(sender As Object, e As EventArgs) Handles GroupBox1.Enter

    End Sub
End Class
