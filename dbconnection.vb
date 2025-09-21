# database module file to link database mysql
  
Imports MySql.Data.MySqlClient

Module dbconnection
    Public acsconn As New MySqlConnection()
    Public acsdr As MySqlDataReader
    Public acsda As MySqlDataAdapter
    Public acscmd As New MySqlCommand
    Public strsql As String
    Public acsds As DataSet

    Public connectionstring As String = "Server=localhost;port=3306;username=root;password=;database=logicalspotcenter"

    Public Function dbconn() As Boolean
        Dim result As Boolean
        Try
            If acsconn.State = ConnectionState.Closed Then
                acsconn.ConnectionString = connectionstring
                
            End If
            result = True
        Catch ex As Exception
            result = False
            MsgBox("Server Not Connected!! " & vbCrLf & ex.Message, vbExclamation)
        End Try
        Return result
    End Function
End Module
