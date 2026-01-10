Imports Microsoft.Win32
Imports System
Imports System.ComponentModel
Imports System.Data
Imports System.Diagnostics
Imports System.Globalization
Imports System.IO
Imports System.Net.NetworkInformation
Imports System.Reflection
Imports System.Text
Imports System.Windows.Forms
Imports UnionAct.Framework.UnionException
Imports UnionAct.Framework.Interface
Imports UnionAct.Framework.Mapping
Imports UnionAct.NSMDInfo

Namespace Framework.Command
    Public Class PublicCommand
        ' Methods
        Public Shared Sub AddAppUnionExceptionData(ByRef appEx As AppUnionException, ByVal strErrorCode As String, ByVal ParamArray strErrorMessage As String())
            Try
                If (appEx Is Nothing) Then
                    appEx = New AppUnionException(MethodBase.GetCurrentMethod, New Exception, strErrorCode, strErrorMessage)
                Else
                    appEx.AddExceptionData(strErrorCode, strErrorMessage)
                End If
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "FE0001", New String(0 - 1) {})
            End Try
        End Sub

        Public Shared Function ConvertHanToZen(ByVal strRtn As String) As String
            Dim str As String
            Try
                Dim chArray As Char() = New Char() {"‚O", "‚P", "‚Q", "‚R", "‚S", "‚T", "‚U", "‚V", "‚W", "‚X", "|", "]", "[", "°", "•", "^", "E", "D"}
                Dim chArray2 As Char() = New Char() {"0"c, "1"c, "2"c, "3"c, "4"c, "5"c, "6"c, "7"c, "8"c, "9"c, "-"c, "-"c, "-"c, "-"c, "&"c, "/"c, "¥", "."c}
                Dim i As Integer
                For i = 0 To chArray2.Length - 1
                    strRtn = strRtn.Replace(chArray2(i), chArray(i))
                Next i
                str = strRtn
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "FE0001", New String(0 - 1) {})
            End Try
            Return str
        End Function

        Public Shared Function ConvertLogicalToPhysical(ByVal dTableIn As DataTable, ByVal map As EntityMap) As DataTable
            Dim table2 As DataTable
            Try
                Dim table As DataTable = map.CreateDataTablePhysName(dTableIn.TableName.ToString)
                Dim i As Integer
                For i = 0 To dTableIn.Rows.Count - 1
                    Dim row As DataRow = table.NewRow
                    Dim j As Integer
                    For j = 0 To dTableIn.Columns.Count - 1
                        Dim str As String = dTableIn.Columns.Item(j).ToString
                        Dim k As Integer
                        For k = 0 To table.Columns.Count - 1
                            Dim logicalName As String = map.GetLogicalName(k)
                            If str.Equals(logicalName) Then
                                row.Item(k) = dTableIn.Rows.Item(i).Item(str)
                                Exit For
                            End If
                        Next k
                    Next j
                    table.Rows.Add(row)
                Next i
                table2 = table
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "FE0001", New String(0 - 1) {})
            End Try
            Return table2
        End Function

        Public Shared Function ConvertNumberToAlphabet(ByVal iInNumber As Integer) As String
            Dim str As String
            Try
                str = New String(DirectCast(ChrW((iInNumber + &H41)), Char), 1)
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "FE0001", New String(0 - 1) {})
            End Try
            Return str
        End Function

        Public Shared Function ConvertPhysicalToLogical(ByVal dTableIn As DataTable, ByVal map As EntityMap) As DataTable
            Dim table2 As DataTable
            Try
                Dim table As DataTable = map.CreateDataTableLogiName(dTableIn.TableName.ToString)
                Dim i As Integer
                For i = 0 To dTableIn.Rows.Count - 1
                    Dim row As DataRow = table.NewRow
                    Dim j As Integer
                    For j = 0 To dTableIn.Columns.Count - 1
                        Dim str As String = dTableIn.Columns.Item(j).ToString
                        Dim k As Integer
                        For k = 0 To table.Columns.Count - 1
                            Dim physicalName As String = map.GetPhysicalName(k)
                            If str.Equals(physicalName) Then
                                row.Item(k) = dTableIn.Rows.Item(i).Item(str)
                                Exit For
                            End If
                        Next k
                    Next j
                    table.Rows.Add(row)
                Next i
                table2 = table
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "FE0001", New String(0 - 1) {})
            End Try
            Return table2
        End Function

        Public Shared Function CopyToS_ADataTable(ByVal dTable As DataTable, ByVal strTableName As String) As DataTable
            Dim table2 As DataTable
            Try
                Dim search As New CommitteeSearch
                Dim table As DataTable = search.CreateDataTableLogiName(strTableName)
                Dim i As Integer
                For i = 0 To dTable.Rows.Count - 1
                    Dim row As DataRow = table.NewRow
                    Dim j As Integer
                    For j = 0 To dTable.Columns.Count - 1
                        Dim str As String = dTable.Columns.Item(j).ToString
                        Dim k As Integer
                        For k = 0 To table.Columns.Count - 1
                            Dim logicalName As String = search.GetLogicalName(k)
                            If str.Equals(logicalName) Then
                                row.Item(k) = dTable.Rows.Item(i).Item(str)
                                Exit For
                            End If
                        Next k
                    Next j
                    table.Rows.Add(row)
                Next i
                table2 = table
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "BE0001", New String(0 - 1) {})
            End Try
            Return table2
        End Function

        Public Shared Sub CreateDirectory(ByVal strDirPath As String)
            Try
                Directory.CreateDirectory(strDirPath)
                Dim attributes As FileAttributes = File.GetAttributes(strDirPath)
                File.SetAttributes(strDirPath, (attributes And Not FileAttributes.ReadOnly))
            Catch exception As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception, "FE0001", New String(0 - 1) {})
            End Try
        End Sub

        Public Shared Function CurrencyToInt(ByVal strCurrency As String) As Integer
            Dim num2 As Integer
            Try
                Dim provider As New NumberFormatInfo With { _
                    .CurrencySymbol = "\", _
                    .CurrencyGroupSeparator = ",", _
                    .NumberGroupSeparator = "," _
                }
                provider.CurrencyGroupSizes(0) = 3
                provider.NumberGroupSizes(0) = 3
                provider.NegativeSign = "-"
                provider.CurrencyNegativePattern = 1
                num2 = Integer.Parse(strCurrency, (NumberStyles.AllowCurrencySymbol Or (NumberStyles.AllowThousands Or NumberStyles.AllowLeadingSign)), provider)
            Catch exception As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception, "FE0001", New String(0 - 1) {})
            End Try
            Return num2
        End Function

        Public Shared Function DateFromStr(ByVal strDate As String) As DateTime
            Dim time As DateTime
            Try
                Dim formats As String() = New String() {"yyyyMMdd", "yyyy/MM/dd"}
                time = DateTime.ParseExact(strDate, formats, Nothing, DateTimeStyles.None)
            Catch exception As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception, "FE0001", New String(0 - 1) {})
            End Try
            Return time
        End Function

        Private Shared Sub DeleteDirectory(ByVal hDirectoryInfo As DirectoryInfo)
            Try
                Dim info As FileInfo
                For Each info In hDirectoryInfo.GetFiles
                    If ((info.Attributes And FileAttributes.ReadOnly) = FileAttributes.ReadOnly) Then
                        info.Attributes = FileAttributes.Normal
                    End If
                Next
                Dim info2 As DirectoryInfo
                For Each info2 In hDirectoryInfo.GetDirectories
                    PublicCommand.DeleteDirectory(info2)
                Next
                If ((hDirectoryInfo.Attributes And FileAttributes.ReadOnly) = FileAttributes.ReadOnly) Then
                    hDirectoryInfo.Attributes = FileAttributes.Directory
                End If
                hDirectoryInfo.Delete(True)
            Catch exception As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception, "FE0001", New String(0 - 1) {})
            End Try
        End Sub

        Public Shared Sub DeleteDirectory(ByVal strDirPath As String)
            Try
                If Directory.Exists(strDirPath) Then
                    PublicCommand.DeleteDirectory(New DirectoryInfo(strDirPath))
                End If
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "FE0001", New String(0 - 1) {})
            End Try
        End Sub

        Public Shared Function GetByteLength(ByVal str As String) As Integer
            Dim byteCount As Integer
            Try
                byteCount = Encoding.GetEncoding("Shift_JIS").GetByteCount(str)
            Catch exception As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception, "FE0001", New String(0 - 1) {})
            End Try
            Return byteCount
        End Function

        Public Shared Function GetKsh() As String
            'TODO Return CommonDataClass.Instance.strKsh
            'Return "KSH"
            Return MDLoginInfo.Ksh
        End Function

        Public Shared Function GetMac() As String
            Dim str As String = ""
            Dim interface2 As NetworkInterface
            For Each interface2 In NetworkInterface.GetAllNetworkInterfaces
                str = interface2.GetPhysicalAddress.ToString
                If Not str.Equals("") Then
                    Return str
                End If
            Next
            Return str
        End Function

        Public Shared Function GetMonthEnd(ByVal strDate As String) As DateTime
            Dim monthEnd As DateTime
            Try
                monthEnd = PublicCommand.GetMonthEnd(strDate, 0)
            Catch exception As SysUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception2, "FE0001", New String(0 - 1) {})
            End Try
            Return monthEnd
        End Function

        Public Shared Function GetMonthEnd(ByVal strDate As String, ByVal iAddMonth As Integer) As DateTime
            Dim time4 As DateTime
            Try
                Dim time2 As DateTime = DateTime.ParseExact(strDate, "yyyyMMdd", Nothing).AddMonths(iAddMonth)
                Dim time3 As New DateTime(time2.Year, time2.Month, DateTime.DaysInMonth(time2.Year, time2.Month))
                time4 = time3
            Catch exception As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception, "FE0001", New String(0 - 1) {})
            End Try
            Return time4
        End Function

        Public Shared Function GetNow() As DateTime
            Return DateTime.Now
        End Function

        Public Shared Function GetStartupPath() As String
            Dim str2 As String
            Try
                Dim key As RegistryKey = Registry.CurrentUser.OpenSubKey("Environment")
                Dim obj2 As Object = key.GetValue("UnionActStartup")
                Dim str As String = If((Not obj2 Is Nothing), obj2.ToString, Nothing)
                key.Close()
                str2 = str
            Catch exception As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception, "FE0001", New String(0 - 1) {})
            End Try
            Return str2
        End Function

        Public Shared Function GetStrContainsCnt(ByVal str As String, ByVal ch As Char) As Integer
            Dim num As Integer = 0
            Dim i As Integer
            For i = 0 To str.Length - 1
                If (str.Chars(i) = ch) Then
                    num += 1
                End If
            Next i
            Return num
        End Function

        Public Shared Function GetSystemDate() As String
            Return DateTime.Today.ToString("yyyyMMdd")
        End Function

        Public Shared Function GetTemporaryPath() As String
            Dim str2 As String
            Try
                Dim key As RegistryKey = Registry.CurrentUser.OpenSubKey("Environment")
                Dim obj2 As Object = key.GetValue("UnionActTemp")
                Dim str As String = If((Not obj2 Is Nothing), obj2.ToString, Nothing)
                If (String.IsNullOrEmpty(str) OrElse Not Directory.Exists(str)) Then
                    obj2 = key.GetValue("TEMP")
                    str = If((Not obj2 Is Nothing), (obj2.ToString & "\"), Nothing)
                End If
                key.Close()
                str2 = str
            Catch exception As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception, "FE0001", New String(0 - 1) {})
            End Try
            Return str2
        End Function

        Public Shared Function GetToday() As DateTime
            Return DateTime.Today
        End Function

        Public Shared Sub OpenOtherProcessFile(ByVal strFileName As String)
            Try
                Dim item As Process = Process.Start(strFileName)
                item.WaitForInputIdle()
                item.EnableRaisingEvents = True
                AddHandler item.Exited, New EventHandler(AddressOf CommonDataClass.Instance.Process_Exited)
                Dim threads As ProcessThreadCollection = item.Threads
                CommonDataClass.Instance.lstProcess.Add(item)
            Catch exception As Win32Exception
                Throw New AppUnionException(MethodBase.GetCurrentMethod, exception, "FE0002", New String() {strFileName})
            Catch exception2 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception2, "FE0001", New String(0 - 1) {})
            End Try
        End Sub

        Public Shared Function PressedEnterKey(ByVal e As KeyPressEventArgs) As Boolean
            Dim flag As Boolean
            Try
                flag = e.KeyChar.Equals("")
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "FE0001", New String(0 - 1) {})
            End Try
            Return flag
        End Function

        Public Shared Function SortDataTable(ByVal dTable As DataTable, ByVal sortKey As String) As DataTable
            Dim table2 As DataTable
            Try
                Dim table As New DataTable
                table = dTable.Clone
                Dim rowArray As DataRow() = dTable.Copy.Select("", sortKey)
                Dim i As Integer
                For i = 0 To rowArray.Length - 1
                    table.ImportRow(rowArray(i))
                Next i
                table2 = table
            Catch exception As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception, "FE0001", New String(0 - 1) {})
            End Try
            Return table2
        End Function

        'Public Shared Function StrBinToByte(ByVal strBin As String) As Byte
        '    Dim num As Integer = 0
        '    If (strBin.Length > 8) Then
        '        Return 0
        '    End If
        '    Dim num2 As Integer = (strBin.Length - 1)
        '    Dim i As Integer = 0
        '    Do While (num2 >= 0)
        '        If ((strBin.Chars(num2) <> "0"c) AndAlso (strBin.Chars(num2) <> "1"c)) Then
        '            Throw New SysUnionException(MethodBase.GetCurrentMethod, New ArgumentException, "FE0001", New String(0 - 1) {})
        '        End If
        '        num = (num Or (((strBin.Chars(num2) - &H30) And 1) << i))
        '        num2 -= 1
        '        i += 1
        '    Loop
        '    Return CByte((num And &HFF))
        'End Function

        Public Shared Function StrnumToCurrency(ByVal strNum As String) As String
            Dim str2 As String
            Try
                Dim provider As New NumberFormatInfo With { _
                    .CurrencySymbol = "\", _
                    .CurrencyGroupSeparator = ",", _
                    .NumberGroupSeparator = "," _
                }
                provider.CurrencyGroupSizes(0) = 3
                provider.NumberGroupSizes(0) = 3
                provider.CurrencyDecimalDigits = 0
                provider.NegativeSign = "-"
                provider.CurrencyNegativePattern = 1
                str2 = Integer.Parse(strNum).ToString("C", provider)
            Catch exception As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception, "FE0001", New String(0 - 1) {})
            End Try
            Return str2
        End Function

        Public Shared Function StrnumToInt(ByVal str As String) As Integer
            If String.IsNullOrEmpty(str) Then
                Return 0
            End If
            Dim num As Integer = 0
            Dim num2 As Integer = 1
            Do While (num < str.Length)
                If (((str.Chars(num) >= "0"c) AndAlso (str.Chars(num) <= "9"c)) OrElse (str.Chars(num) = "-"c)) Then
                    Exit Do
                End If
                num += 1
            Loop
            If (num = str.Length) Then
                Return 0
            End If
            If (str.Chars(num) = "-"c) Then
                num2 = -1
                num += 1
            End If
            Dim num3 As Integer = 0
            Do While (num < str.Length)
                If ((str.Chars(num) < "0"c) OrElse (str.Chars(num) > "9"c)) Then
                    Exit Do
                End If
                Try
                    Dim ch As Char = str.Chars(num)
                    num3 = ((num3 * 10) + Integer.Parse(ch.ToString))
                Catch exception As OverflowException
                    Throw New AppUnionException(MethodBase.GetCurrentMethod, exception, "FE0001", New String(0 - 1) {})
                End Try
                num += 1
            Loop
            Return (num3 * num2)
        End Function

        Public Shared Function ToHalfAdjust(ByVal dValue As Double, ByVal iDigits As Integer) As Double
            Dim num2 As Double
            Try
                Dim num As Double = Math.Pow(10, CDbl((iDigits - 1)))
                num2 = If((dValue > 0), (Math.Floor(CDbl(((dValue * num) + 0.5))) / num), (Math.Ceiling(CDbl(((dValue * num) - 0.5))) / num))
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "FE0001", New String(0 - 1) {})
            End Try
            Return num2
        End Function

        Public Shared Function ToRoundDown(ByVal dValue As Double, ByVal iDigits As Integer) As Double
            Dim num2 As Double
            Try
                Dim num As Double = Math.Pow(10, CDbl((iDigits - 1)))
                num2 = If((dValue > 0), (Math.Floor(CDbl((dValue * num))) / num), (Math.Ceiling(CDbl((dValue * num))) / num))
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "FE0001", New String(0 - 1) {})
            End Try
            Return num2
        End Function

        Public Shared Function ToRoundUp(ByVal dValue As Double, ByVal iDigits As Integer) As Double
            Dim num2 As Double
            Try
                Dim num As Double = Math.Pow(10, CDbl((iDigits - 1)))
                num2 = If((dValue > 0), (Math.Ceiling(CDbl((dValue * num))) / num), (Math.Floor(CDbl((dValue * num))) / num))
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "FE0001", New String(0 - 1) {})
            End Try
            Return num2
        End Function

        Shared Function StrBinToByte(ByVal p1 As String) As Integer
            Throw New NotImplementedException
        End Function

    End Class
End Namespace
