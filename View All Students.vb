Imports System.Drawing.Printing
Imports MySql.Data.MySqlClient
Public Class Manage_Students
    Dim WithEvents PD As New PrintDocument
    Dim PPD As New PrintPreviewDialog
    Dim dgvPrinter As DataGridView

    Private Sub btnedit_Click(sender As Object, e As EventArgs) Handles btnedit.Click
        Try
            Dim studentid As String = txt_search.Text.Trim()

            If studentid = "" Then
                MsgBox("Please Enter Student ID to Edit.", vbExclamation)
                Exit Sub
            End If

            dbconn()
            If acsconn.State = ConnectionState.Closed Then
                acsconn.Open()
            End If

            ' query jo id se match kare
            Dim searchbyid As String = "SELECT * FROM students_records WHERE student_id=@id"
            Dim cmdedit As New MySqlCommand(searchbyid, acsconn)
            cmdedit.Parameters.AddWithValue("@id", studentid)

            Dim readeredit As MySqlDataReader = cmdedit.ExecuteReader()

            If readeredit.Read() Then
                Dim frm As New Student_Information()

                ' Fill data in Student_Information form fields
                frm.txtstudentid.Text = readeredit("student_id").ToString()
                frm.txtstdname.Text = readeredit("name").ToString()
                Dim gender As String = readeredit("gender").ToString()
                If gender = "Male" Then
                    frm.gendermale.Checked = True
                ElseIf gender = "Female" Then
                    frm.genderfemale.Checked = True
                ElseIf gender = "Transgender" Then
                    frm.gendertransgender.Checked = True
                End If
                frm.txtstdfathername.Text = readeredit("father_name").ToString()
                frm.txtstdmothername.Text = readeredit("mother_name").ToString()
                frm.txtdateofbirth.Value = Convert.ToDateTime(readeredit("dob"))
                frm.txtaadharnumber.Text = readeredit("aadhar_no").ToString()
                frm.txtmobilenumber.Text = readeredit("mobile_no").ToString()
                frm.txtqualification.Text = readeredit("qualification").ToString()
                frm.txtcourse.Text = readeredit("course").ToString()
                frm.txtcourseduration.Text = readeredit("duration").ToString()
                frm.txtbatch.Text = readeredit("batch").ToString()
                frm.txtaddress.Text = readeredit("address").ToString()

                readeredit.Close()
                acsconn.Close()

                ' 👇 Ab form ko show karo edit ke liye
                Me.Hide()
                frm.ShowDialog()
                Me.Show()
            Else
                MsgBox("No record found with Student ID: " & studentid, vbExclamation)
                readeredit.Close()
            End If

        Catch ex As Exception
            MsgBox("Edit Error: " & ex.Message, vbCritical)
        Finally
            If acsconn.State = ConnectionState.Open Then
                acsconn.Close()
            End If
        End Try
    End Sub

    Private Sub Manage_Students_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadStudents()
    End Sub

    Private Sub LoadStudents()
        Try
            student_records_mantain.Rows.Clear()

            dbconn()
            If acsconn.State = ConnectionState.Closed Then
                acsconn.Open()
            End If

            Dim query As String = "SELECT student_id, name, gender, father_name, dob, aadhar_no, mobile_no, qualification, course, duration, batch,  created_at FROM students_records"
            Dim cmd As New MySqlCommand(query, acsconn)
            Dim reader As MySqlDataReader = cmd.ExecuteReader()

            While reader.Read()
                student_records_mantain.Rows.Add(
                    reader("student_id").ToString(),
                    reader("name").ToString(),
                    reader("gender").ToString(),
                    reader("father_name").ToString(),
                    Convert.ToDateTime(reader("dob")).ToString("yyyy-MM-dd"),
                    reader("aadhar_no").ToString(),
                    reader("mobile_no").ToString(),
                    reader("qualification").ToString(),
                    reader("course").ToString(),
                    reader("duration").ToString(),
                    reader("batch").ToString(),
                    Convert.ToDateTime(reader("created_at")).ToString("yyyy-MM-dd")
                )
            End While

            reader.Close()
        Catch ex As Exception
            MsgBox("Error loading data: " & ex.Message, vbCritical)
        Finally
            If acsconn.State = ConnectionState.Open Then
                acsconn.Close()
            End If
        End Try
    End Sub

    Private Sub txt_search_Click(sender As Object, e As EventArgs) Handles txt_search.Click
        txt_search.Text = ""
    End Sub

    Private Sub btn_add_Click(sender As Object, e As EventArgs) Handles btn_add.Click
        Me.Hide()
        Dim frm As New Student_Information()
        frm.ShowDialog()
        Me.Show()

        ' Refresh records after add
        LoadStudents()
    End Sub

    Private Sub btndelete_Click(sender As Object, e As EventArgs) Handles btndelete.Click
        Try
            Dim studentid As String = txt_search.Text.Trim()

            If studentid = "" Then
                MsgBox("Please Enter Student ID to Delete.", vbExclamation)
                Exit Sub
            End If

            If MsgBox("Are you sure you want to delete Student ID: " & studentid & " ?", vbYesNo + vbQuestion) = vbNo Then
                Exit Sub
            End If

            dbconn()
            If acsconn.State = ConnectionState.Closed Then
                acsconn.Open()
            End If

            Dim deleteQuery As String = "DELETE FROM students_records WHERE student_id=@id"
            Using cmd As New MySqlCommand(deleteQuery, acsconn)
                cmd.Parameters.AddWithValue("@id", studentid)

                Dim rowsAffected As Integer = cmd.ExecuteNonQuery()
                If rowsAffected > 0 Then
                    MsgBox("Student record deleted successfully.", vbInformation)
                Else
                    MsgBox("No record found with Student ID: " & studentid, vbExclamation)
                End If
            End Using

            ' Refresh after delete
            LoadStudents()

        Catch ex As Exception
            MsgBox("Delete Error: " & ex.Message, vbCritical)
        Finally
            If acsconn.State = ConnectionState.Open Then
                acsconn.Close()
            End If
        End Try
    End Sub
    Private Sub searchdata(keyword As String)
        Try
            Dim dtb As New DataTable()
            dbconn()
            If acsconn.State = ConnectionState.Closed Then acsconn.Open()

            Dim searchquery As String = "SELECT student_id, name, gender, father_name, dob, aadhar_no, mobile_no, qualification, course, duration, batch, created_at " &
                                    "FROM students_records " &
                                    "WHERE student_id LIKE @keyword"

            Using cmd As New MySqlCommand(searchquery, acsconn)
                cmd.Parameters.AddWithValue("@keyword", "%" & keyword.Trim() & "%")
                Using adapter As New MySqlDataAdapter(cmd)
                    adapter.Fill(dtb)
                    student_records_mantain.Rows.Clear()

                    For Each row As DataRow In dtb.Rows
                        student_records_mantain.Rows.Add(
                        row("student_id").ToString(),
                        row("name").ToString(),
                        row("gender").ToString(),
                        row("father_name").ToString(),
                        Convert.ToDateTime(row("dob")).ToString("yyyy-MM-dd"),
                        row("aadhar_no").ToString(),
                        row("mobile_no").ToString(),
                        row("qualification").ToString(),
                        row("course").ToString(),
                        row("duration").ToString(),
                        row("batch").ToString(),
                        Convert.ToDateTime(row("created_at")).ToString("yyyy-MM-dd")
                    )
                    Next
                End Using
            End Using

        Catch ex As Exception
            MsgBox("Search Error: " & ex.Message, vbCritical)
        Finally
            If acsconn.State = ConnectionState.Open Then acsconn.Close()
        End Try
    End Sub

    Private Sub txt_search_TextChanged(sender As Object, e As EventArgs) Handles txt_search.TextChanged
        If txt_search.Text.Trim() <> "" Then
            searchdata(txt_search.Text)
        Else
            LoadStudents()
        End If
    End Sub

    Private Sub btnprint_Click(sender As Object, e As EventArgs) Handles btnprint.Click
        dgvPrinter = student_records_mantain
        PPD.Document = PrintDocument1
        PPD.ShowDialog()
    End Sub
    Private Sub PrintDocument1_PrintPage(sender As Object, e As PrintPageEventArgs) Handles PrintDocument1.PrintPage
        Static rowIndex As Integer = 0
        Dim fontHeader As New Font("Arial", 12, FontStyle.Bold)
        Dim fontRow As New Font("Arial", 10, FontStyle.Regular)
        Dim lineHeight As Integer = fontRow.GetHeight(e.Graphics) + 5
        Dim recordsPerColumn As Integer = 12
        Dim recordsPerPrintPage As Integer = recordsPerColumn * 2
        Dim x As Integer = e.MarginBounds.Left
        Dim y As Integer = e.MarginBounds.Top
        Dim recordHeight As Integer = (dgvPrinter.Columns.Count + 2) * lineHeight

        Do While rowIndex < dgvPrinter.Rows.Count
            Dim row As DataGridViewRow = dgvPrinter.Rows(rowIndex)
            If row.IsNewRow Then
                rowIndex += 1
                Continue Do
            End If


            If y + recordHeight > e.MarginBounds.Bottom Then
                e.HasMorePages = True
                Exit Do
            End If


            e.Graphics.DrawString("Student Record", fontHeader, Brushes.Black, x, y)
            Dim currentY As Integer = y + lineHeight
            For Each col As DataGridViewColumn In dgvPrinter.Columns
                Dim header As String = col.HeaderText
                Dim value As String = If(row.Cells(col.Index).Value IsNot Nothing, row.Cells(col.Index).Value.ToString(), "")
                e.Graphics.DrawString(header & ": " & value, fontRow, Brushes.Black, x + 20, currentY)
                currentY += lineHeight
            Next
            y = currentY + lineHeight ' Extra spacing

            rowIndex += 1

            ' If a column is full, move to the next column
            If (rowIndex - (Math.Ceiling(rowIndex / recordsPerPrintPage) - 1) * recordsPerPrintPage) Mod recordsPerColumn = 0 And rowIndex Mod recordsPerPrintPage <> 0 Then
                x = e.MarginBounds.Left + (e.MarginBounds.Width / 2)
                y = e.MarginBounds.Top
            End If

            ' If the page is full, start a new page
            If y + recordHeight > e.MarginBounds.Bottom Then
                e.HasMorePages = True
                Exit Do
            End If

        Loop
        If rowIndex = dgvPrinter.Rows.Count Then
            e.HasMorePages = False
            rowIndex = 0
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Me.Close()
        Add_Attendence.Show()
    End Sub
End Class
