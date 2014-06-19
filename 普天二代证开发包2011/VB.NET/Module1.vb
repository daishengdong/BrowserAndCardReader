Imports System.Runtime.InteropServices
Imports System.Text
Module Module1
    Class Card2
        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode, Pack:=8)> _
        Public Structure PERSONINFOW
            <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=16)> _
            Public name As String
            <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=2)> _
            Public sex As String
            <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=10)> _
            Public nation As String
            <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=10)> _
            Public birthday As String
            <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=36)> _
            Public address As String
            <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=20)> _
            Public cardId As String
            <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=16)> _
            Public police As String
            <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=10)> _
            Public validStart As String
            <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=10)> _
            Public validEnd As String
            <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=2)> _
            Public sexCode As String
            <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=4)> _
            Public nationCode As String
            <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=36)> _
            Public appendMsg As String
        End Structure
        <DllImport("cardapi3.dll", EntryPoint:="OpenCardReader", _
            CallingConvention:=CallingConvention.StdCall, CharSet:=CharSet.Unicode)> _
        Public Shared Function OpenCardReader(ByVal lPort As Int32, ByVal ulFlag As UInt32, ByVal ulBaudRate As UInt32) As Int32
        End Function
        <DllImport("cardapi3.dll", EntryPoint:="GetPersonMsgW", _
            CallingConvention:=CallingConvention.StdCall, CharSet:=CharSet.Unicode)> _
        Public Shared Function GetPersonMsgW(ByRef pInfo As PERSONINFOW, ByVal pszImageFile As String) As Int32
        End Function
        <DllImport("cardapi3.dll", EntryPoint:="CloseCardReader", _
            CallingConvention:=CallingConvention.StdCall, CharSet:=CharSet.Unicode)> _
        Public Shared Function CloseCardReader() As Int32
        End Function
        <DllImport("cardapi3.dll", entrypoint:="GetErrorTextW", _
        CallingConvention:=CallingConvention.StdCall, CharSet:=CharSet.Unicode)> _
        Public Shared Sub GetErrorTextW(ByVal pszBuffer As StringBuilder, ByVal dwBufLen As UInt32)
        End Sub
    End Class
End Module
