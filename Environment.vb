Imports System.Text
Imports System.IO
Imports System.Globalization

Public Class Environment

    Public Const Floats As NumberStyles = NumberStyles.Float Or NumberStyles.AllowExponent
    Public Const Integers As NumberStyles = NumberStyles.Integer Or NumberStyles.AllowExponent

    Public Shared Property Encoder As Encoding = Encoding.UTF8
    Public Shared Property Logfile As String = String.Format(".\log-{0}.log", DateTime.Now.ToString("MM-dd-yy"))

    Public Shared Function Culture() As CultureInfo
        Static ci As New CultureInfo("en-US")
        Return ci
    End Function

    Public Shared Function IsWritable(fn As String) As Boolean
        Try
            File.Create(Path.Combine(New FileInfo(fn).Directory.FullName, Path.GetRandomFileName), 1, FileOptions.DeleteOnClose).Close()
            Return True
        Catch
            Return False
        End Try
    End Function
End Class