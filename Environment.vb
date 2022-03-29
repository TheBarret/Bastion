Imports System.Text
Imports System.Globalization
Imports System.Security.Principal
Imports System.IO
Imports System.Security.AccessControl
Imports Bastion.Helpers

Public Class Environment

    Public Const Floats As NumberStyles = NumberStyles.Float Or NumberStyles.AllowExponent
    Public Const Integers As NumberStyles = NumberStyles.Integer Or NumberStyles.AllowExponent

    Public Shared Property CurrentUser As WindowsIdentity = WindowsIdentity.GetCurrent
    Public Shared Property CurrentPrincipal As WindowsPrincipal = New WindowsPrincipal(WindowsIdentity.GetCurrent)

    Public Shared Property Encoder As Encoding = Encoding.UTF8
    Public Shared Property Logfile As String = String.Format(".\log-{0}.log", DateTime.Now.ToString("MM-dd-yy"))

    Public Shared Function Culture() As CultureInfo
        Static ci As New CultureInfo("en-US")
        Return ci
    End Function

    ''' <summary>
    ''' Returns true if the current user is an administrator.
    ''' </summary>
    ''' <returns></returns>
    Public Shared Function IsAdministrator() As Boolean
        Return Environment.CurrentPrincipal.IsInRole(WindowsBuiltInRole.Administrator)
    End Function

    ''' <summary>
    ''' Returns true if a path is file
    ''' </summary>
    ''' <param name="path"></param>
    ''' <returns></returns>
    Public Shared Function IsFile(path As String) As Boolean
        Dim attr As FileAttributes = File.GetAttributes(path)
        Return Not (attr And FileAttributes.Directory) = FileAttributes.Directory
    End Function

    ''' <summary>
    ''' Returns true if current user is able to read and write to given path
    ''' </summary>
    ''' <param name="path"></param>
    ''' <returns></returns>
    Public Shared Function HasAccess(path As String) As Boolean
        If (File.Exists(path) Or Directory.Exists(path)) Then
            If (Environment.IsFile(path)) Then
                Return Environment.HasAccess(New FileInfo(path))
            Else
                Return Environment.HasAccess(New DirectoryInfo(path))
            End If
        End If
        Return False
    End Function

    ''' <summary>
    ''' Returns true if current user is able to read and write to given file
    ''' </summary>
    ''' <param name="path"></param>
    ''' <returns></returns>
    Public Shared Function HasAccess(path As FileInfo) As Boolean
        Dim accessControl As FileSecurity = path.GetAccessControl
        Dim accessRules As AuthorizationRuleCollection = accessControl.GetAccessRules(True, True, GetType(NTAccount))
        For Each rule As FileSystemAccessRule In accessRules
            If rule.IdentityReference.Value = Environment.CurrentUser.User.Value Then
                If rule.AccessControlType = AccessControlType.Allow Then
                    Return True
                End If
            End If
        Next
        Return False
    End Function

    ''' <summary>
    ''' Returns true if the current user is able to read and write to given directory
    ''' </summary>
    ''' <param name="path"></param>
    ''' <returns></returns>
    Public Shared Function HasAccess(path As DirectoryInfo) As Boolean
        Dim accessControl As DirectorySecurity = path.GetAccessControl()
        Dim accessRules As AuthorizationRuleCollection = accessControl.GetAccessRules(True, True, GetType(NTAccount))
        For Each rule As FileSystemAccessRule In accessRules
            If rule.IdentityReference.Value = Environment.CurrentUser.User.Value Then
                If rule.AccessControlType = AccessControlType.Allow Then
                    Return True
                End If
            End If
        Next
        Return False
    End Function

    ''' <summary>
    ''' Returns true if directory is writable by current user
    ''' </summary>
    ''' <param name="path"></param>
    ''' <param name="right"></param>
    ''' <returns></returns>
    Public Shared Function HasAccess(path As DirectoryInfo, right As FileSystemRights) As Boolean
        Return Environment.HasFileOrDirectoryAccess(right, path.GetAccessControl().GetAccessRules(True, True, GetType(SecurityIdentifier)))
    End Function

    ''' <summary>
    ''' Returns true if file is writable by current user
    ''' </summary>
    ''' <param name="file"></param>
    ''' <param name="right"></param>
    ''' <returns></returns>
    Public Shared Function HasAccess(file As FileInfo, right As FileSystemRights) As Boolean
        Return Environment.HasFileOrDirectoryAccess(right, file.GetAccessControl().GetAccessRules(True, True, GetType(SecurityIdentifier)))
    End Function

    ''' <summary>
    ''' Returns true if file or directory is writable by current user
    ''' </summary>
    ''' <param name="right"></param>
    ''' <param name="acl"></param>
    ''' <returns></returns>
    Private Shared Function HasFileOrDirectoryAccess(right As FileSystemRights, acl As AuthorizationRuleCollection) As Boolean
        Dim allow As Boolean = False
        Dim inheritedAllow As Boolean = False
        Dim inheritedDeny As Boolean = False

        For i As Integer = 0 To acl.Count - 1
            Dim currentRule = CType(acl(i), FileSystemAccessRule)

            If _CurrentUser.User.Equals(currentRule.IdentityReference) OrElse _CurrentPrincipal.IsInRole(CType(currentRule.IdentityReference, SecurityIdentifier)) Then
                If currentRule.AccessControlType.Equals(AccessControlType.Deny) Then
                    If (currentRule.FileSystemRights And right) = right Then
                        If currentRule.IsInherited Then
                            inheritedDeny = True
                        Else
                            Return False
                        End If
                    End If
                ElseIf currentRule.AccessControlType.Equals(AccessControlType.Allow) Then
                    If (currentRule.FileSystemRights And right) = right Then
                        If currentRule.IsInherited Then
                            inheritedAllow = True
                        Else
                            allow = True
                        End If
                    End If
                End If
            End If
        Next
        If allow Then Return True

        Return inheritedAllow AndAlso Not inheritedDeny
    End Function
    ''' <summary>
    ''' Validates the parameters of the delegate and tries to auto convert the parameters to the corresponding types
    ''' </summary>
    ''' <param name="rt"></param>
    ''' <param name="e"></param>
    ''' <param name="params"></param>
    ''' <returns></returns>
    Public Shared Function Validate(rt As Runtime, e As [Delegate], params As List(Of Object)) As Boolean
        If (e.Method.GetParameters.Count <> params.Count) Then
            rt.Log(String.Format("Parameter count mismatch for '{0}()'", e.Method.Name))
            Return False
        End If
        For i As Integer = 0 To e.Method.GetParameters.Count - 1
            If (e.Method.GetParameters(i).ParameterType = GetType(Object)) Then Continue For
            If (e.Method.GetParameters(i).ParameterType <> params(i).GetType) Then
                If Not Environment.ConvertType(rt, params(i), e.Method.GetParameters(i).ParameterType, params(i)) Then
                    rt.Log(String.Format("Parameter type mismatch for '{0}()'", e.Method.Name))
                    Return False
                End If
            End If
        Next
        Return True
    End Function

    ''' <summary>
    ''' Attempts to convert the given object to the given type and makes a log entry of the result.
    ''' </summary>
    ''' <typeparam name="T"></typeparam>
    ''' <param name="rt"></param>
    ''' <param name="obj"></param>
    ''' <param name="type"></param>
    ''' <param name="result"></param>
    ''' <returns></returns>
    Public Shared Function ConvertType(Of T)(rt As Runtime, obj As Object, type As Type, ByRef result As T) As Boolean
        Try
            If (obj.GetType = type) Then
                result = DirectCast(obj, T)
                Return True
            End If
            rt.Log(String.Format("Attempting to change type '{0}' to '{1}'", obj.GetType.Name, type.Name))
            result = DirectCast(Convert.ChangeType(obj, type), T)
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function
End Class