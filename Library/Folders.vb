
Namespace Library
    Public Class Folders

        <ScriptFunction("myappdata")>
        Public Shared Function GetAppdata(rt As Runtime) As String
            Return System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData)
        End Function

        <ScriptFunction("mydocuments")>
        Public Shared Function GetMyDocuments(rt As Runtime) As String
            Return System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments)
        End Function

        <ScriptFunction("mydesktop")>
        Public Shared Function GetMyDesktop(rt As Runtime) As String
            Return System.Environment.GetFolderPath(System.Environment.SpecialFolder.DesktopDirectory)
        End Function

        <ScriptFunction("myfonts")>
        Public Shared Function GetMyFonts(rt As Runtime) As String
            Return System.Environment.GetFolderPath(System.Environment.SpecialFolder.Fonts)
        End Function

        <ScriptFunction("myprograms")>
        Public Shared Function GetMyPrograms(rt As Runtime) As String
            Return System.Environment.GetFolderPath(System.Environment.SpecialFolder.ProgramFiles)
        End Function

        <ScriptFunction("myprogramsX86")>
        Public Shared Function GetMyProgramsX86(rt As Runtime) As String
            Return System.Environment.GetFolderPath(System.Environment.SpecialFolder.ProgramFilesX86)
        End Function

        <ScriptFunction("mystartup")>
        Public Shared Function GetMyStartup(rt As Runtime) As String
            Return System.Environment.GetFolderPath(System.Environment.SpecialFolder.Startup)
        End Function

        <ScriptFunction("mysystem")>
        Public Shared Function GetMySystem(rt As Runtime) As String
            Return System.Environment.GetFolderPath(System.Environment.SpecialFolder.System)
        End Function

        <ScriptFunction("mysystemX86")>
        Public Shared Function GetMySystemX86(rt As Runtime) As String
            Return System.Environment.GetFolderPath(System.Environment.SpecialFolder.SystemX86)
        End Function

        <ScriptFunction("mywindows")>
        Public Shared Function GetMyWindows(rt As Runtime) As String
            Return System.Environment.GetFolderPath(System.Environment.SpecialFolder.Windows)
        End Function
    End Class
End Namespace
