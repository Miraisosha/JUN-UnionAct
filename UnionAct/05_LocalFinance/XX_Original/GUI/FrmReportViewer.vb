Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Windows.Forms
Imports System
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Drawing.Printing
Imports System.Reflection
Imports System.Runtime.InteropServices
Imports System.Windows.Forms
Imports UnionAct.Framework.UnionException
Imports UnionAct.Framework.Interface
Imports UnionAct.Framework

Namespace Report
    Public Class FrmReportViewer
        Inherits Form
        ' Methods
        Private Sub New()
            Me.iPrintCount = 1
            Me.piPrintCount = 1
            Me.printDoc = New PrintDocument
            Me.paperSourceKind = PaperSourceKind.Custom
            Me.printStatus = New tagPrintStatus
            Me.nMaxPageCount = 1
        End Sub

        Public Sub New(ByVal dSet As DataSet, ByVal strReportName As String)
            Me.iPrintCount = 1
            Me.piPrintCount = 1
            Me.printDoc = New PrintDocument
            Me.paperSourceKind = PaperSourceKind.Custom
            Me.printStatus = New tagPrintStatus
            Me.nMaxPageCount = 1
            Try 
                Dim i As Integer
                For i = 0 To dSet.Tables.Count - 1
                    If (dSet.Tables.Item(i).Rows.Count = 0) Then
                        Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "RI0001", New String(0  - 1) {})
                    End If
                Next i
                Me.dSetReport = dSet

                Dim rptObj As ReportDocument = New ReportDocument()
                rptObj.Load(System.IO.Directory.GetCurrentDirectory() & "\" & strReportName & ".rpt")
                rptObj.SetDataSource(Me.dSetReport)
                Me.ShowDialog()
                'rptObj.PrintToPrinter(1, False, 0, 0)
                Return

                'Me.rptData = rptObj.DataDefinition.
                'Dim asm As [Assembly] = [Assembly].LoadFrom("Report.dll")
                'Dim masterType As Type = asm.GetType(strReportName)
                'Me.rptData = CType(Activator.CreateInstance(masterType), ReportClass)

                ' 上の3行は次のようにも記述可能
                'Me.rptData = CType(asm.CreateInstance(strReportName), ReportClass)

                'Dim obj2 As Object = Activator.CreateInstance(Type.GetType(strReportName))
                'Me.rptData = DirectCast(obj2, ReportClass)
                'Me.rptData.SetDataSource(Me.dSetReport)
                Me.InitializeComponent
                Me.nudPrintCnt.Value = 1
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "RE0001", New String(0  - 1) {})
            End Try
        End Sub

        Public Sub New(ByVal dSet As DataSet, ByVal nPageFrom As Integer, ByVal nPageTo As Integer)
            Me.iPrintCount = 1
            Me.piPrintCount = 1
            Me.printDoc = New PrintDocument
            Me.paperSourceKind = PaperSourceKind.Custom
            Me.printStatus = New tagPrintStatus
            Me.nMaxPageCount = 1
            Try 
                Dim i As Integer
                For i = 0 To dSet.Tables.Count - 1
                    If (dSet.Tables.Item(i).Rows.Count = 0) Then
                        Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "RI0001", New String(0  - 1) {})
                    End If
                Next i
                Me.dSetReport = dSet
                Dim obj2 As Object = Activator.CreateInstance(Type.GetType("Report.Search.RptSearch"), New Object() { dSet.Tables.Item("search_list_header") })
                Me.rptData = DirectCast(obj2, ReportClass)
                Me.rptData.SetDataSource(Me.dSetReport)
                Me.InitializeComponent
                Me.nudPrintCnt.Value = 1
                Me.nudPageFrom.Value = nPageFrom
                Me.nudPageTo.Value = nPageTo
                Me.grpPrintRange.Visible = True
                If Me.rbPrintRangeAll.Checked Then
                    Me.nudPageFrom.Enabled = Me.nudPageTo.Enabled = False
                End If
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "RE0001", New String(0  - 1) {})
            End Try
        End Sub

        Public Sub New(ByVal dSet As DataSet, ByVal strReportName As String, ByVal paperKind As PaperSourceKind)
            Me.iPrintCount = 1
            Me.piPrintCount = 1
            Me.printDoc = New PrintDocument
            Me.paperSourceKind = PaperSourceKind.Custom
            Me.printStatus = New tagPrintStatus
            Me.nMaxPageCount = 1
            Try 
                Dim i As Integer
                For i = 0 To dSet.Tables.Count - 1
                    If (dSet.Tables.Item(i).Rows.Count = 0) Then
                        Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "RI0001", New String(0  - 1) {})
                    End If
                Next i
                Me.dSetReport = dSet
                Dim obj2 As Object = Activator.CreateInstance(Type.GetType(strReportName))
                Me.rptData = DirectCast(obj2, ReportClass)
                Me.rptData.SetDataSource(Me.dSetReport)
                Me.paperSourceKind = paperKind
                Me.InitializeComponent
                Me.nudPrintCnt.Text = "1"
                Me.lblPrintCnt.Visible = False
                Me.nudPrintCnt.Visible = False
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "RE0001", New String(0  - 1) {})
            End Try
        End Sub

        Public Sub New(ByVal dSet As DataSet, ByVal strReportName As String, ByVal iPrint As Integer)
            Me.iPrintCount = 1
            Me.piPrintCount = 1
            Me.printDoc = New PrintDocument
            Me.paperSourceKind = PaperSourceKind.Custom
            Me.printStatus = New tagPrintStatus
            Me.nMaxPageCount = 1
            Try 
                Dim i As Integer
                For i = 0 To dSet.Tables.Count - 1
                    If (dSet.Tables.Item(i).Rows.Count = 0) Then
                        Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "RI0001", New String(0  - 1) {})
                    End If
                Next i
                Me.dSetReport = dSet
                Dim obj2 As Object = Activator.CreateInstance(Type.GetType(strReportName))
                Me.rptData = DirectCast(obj2, ReportClass)
                Me.rptData.SetDataSource(Me.dSetReport)
                Me.InitializeComponent
                Me.nudPrintCnt.Value = iPrint
                Me.lblPrintCnt.Visible = True
                Me.nudPrintCnt.Visible = True
                If (iPrint <= 0) Then
                    Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "RE0002", New String(0  - 1) {})
                End If
                Me.iPrintCount = iPrint
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "RE0001", New String(0  - 1) {})
            End Try
        End Sub

        Public Sub New(ByVal dSet As DataSet, ByVal strReportName As String, ByVal nPageFrom As Integer, ByVal nPageTo As Integer)
            Me.iPrintCount = 1
            Me.piPrintCount = 1
            Me.printDoc = New PrintDocument
            Me.paperSourceKind = PaperSourceKind.Custom
            Me.printStatus = New tagPrintStatus
            Me.nMaxPageCount = 1
            Try 
                Dim i As Integer
                For i = 0 To dSet.Tables.Count - 1
                    If (dSet.Tables.Item(i).Rows.Count = 0) Then
                        Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "RI0001", New String(0  - 1) {})
                    End If
                Next i
                Me.dSetReport = dSet
                Dim obj2 As Object = Activator.CreateInstance(Type.GetType(strReportName))
                Me.rptData = DirectCast(obj2, ReportClass)
                Me.rptData.SetDataSource(Me.dSetReport)
                Me.InitializeComponent
                Me.nudPrintCnt.Value = 1
                Me.nudPageFrom.Value = nPageFrom
                Me.nudPageTo.Value = nPageTo
                Me.grpPrintRange.Visible = True
                If Me.rbPrintRangeAll.Checked Then
                    Me.nudPageFrom.Enabled = Me.nudPageTo.Enabled = False
                End If
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "RE0001", New String(0  - 1) {})
            End Try
        End Sub

        Public Sub New(ByVal strReportName As String, ByVal dSet As DataSet, ByVal ViewerStyle As Integer, ByVal printStat As tagPrintStatus)
            Me.iPrintCount = 1
            Me.piPrintCount = 1
            Me.printDoc = New PrintDocument
            Me.paperSourceKind = PaperSourceKind.Custom
            Me.printStatus = New tagPrintStatus
            Me.nMaxPageCount = 1
            Try 
                Dim obj2 As Object
                Dim i As Integer
                For i = 0 To dSet.Tables.Count - 1
                    If (dSet.Tables.Item(i).Rows.Count = 0) Then
                        Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "RI0001", New String(0  - 1) {})
                    End If
                Next i
                Dim type As Type = Type.GetType(strReportName)
                If ((ViewerStyle And 1) <> 0) Then
                    obj2 = Activator.CreateInstance(type, New Object() { dSet.Tables.Item("search_list_header") })
                Else
                    obj2 = Activator.CreateInstance(type)
                End If
                Me.rptData = DirectCast(obj2, ReportClass)
                Me.dSetReport = dSet
                Me.rptData.SetDataSource(Me.dSetReport)
                Me.InitializeComponent
                Me.printStatus = printStat
                If ((ViewerStyle And 4) <> 0) Then
                    If (printStat.nPrintCount <= 0) Then
                        Me.nudPrintCnt.Text = ""
                    Else
                        Me.nudPrintCnt.Value = printStat.nPrintCount
                    End If
                    Me.lblPrintCnt.Visible = True
                    Me.nudPrintCnt.Visible = True
                End If
                If ((ViewerStyle And 8) <> 0) Then
                    If (printStat.nPageFrom <= 0) Then
                        Me.nudPageFrom.Text = ""
                    Else
                        Me.nudPageFrom.Value = printStat.nPageFrom
                    End If
                    If (printStat.nPageTo <= 0) Then
                        Me.nudPageTo.Text = ""
                    Else
                        Me.nudPageTo.Value = printStat.nPageTo
                    End If
                    If ((Me.nudPageFrom.Text.Length > 0) OrElse (Me.nudPageTo.Text.Length > 0)) Then
                        Me.rbPrintRangeAppoint.Checked = True
                    Else
                        Me.rbPrintRangeAll.Checked = True
                    End If
                    Me.grpPrintRange.Visible = True
                    If Me.rbPrintRangeAll.Checked Then
                        Me.nudPageFrom.Enabled = Me.nudPageTo.Enabled = False
                    End If
                End If
                If ((ViewerStyle And &H10) <> 0) Then
                    Me.paperSourceKind = printStat.pSourceKind
                End If
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "RE0001", New String(0  - 1) {})
            End Try
        End Sub

        Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As EventArgs)
            Me.iRenCode = 0
            MyBase.Close
        End Sub

        Private Sub btnSelect1_Click(ByVal sender As Object, ByVal e As EventArgs)
            Try 
                Dim message As New UnionMessage
                If Not Me.nudPrintCnt.Visible Then
                    Me.iPrintCount = 1
                Else
                    Try 
                        Me.iPrintCount = Convert.ToInt32(Me.nudPrintCnt.Text.ToString)
                        If (Me.iPrintCount <= 0) Then
                            message.ShowMessage("RE0002")
                            Me.iRenCode = 0
                            Return
                        End If
                    Catch exception1 As Exception
                        message.ShowMessage("RE0003")
                        Me.iRenCode = 0
                        Return
                    End Try
                End If
                Me.piPrintCount = Me.iPrintCount
                If Me.grpPrintRange.Visible Then
                    If Me.rbPrintRangeAll.Checked Then
                        Me.printStatus.nPageFrom = 0
                        Me.printStatus.nPageTo = 0
                    Else
                        Dim num As Integer = If((Me.nudPageFrom.Text.Length = 0), 1, CInt(Me.nudPageFrom.Value))
                        Dim num2 As Integer = If((Me.nudPageTo.Text.Length = 0), Me.nMaxPageCount, CInt(Me.nudPageTo.Value))
                        If (num > num2) Then
                            message.ShowMessage("RE0004")
                            Return
                        End If
                        Me.printStatus.nPageFrom = num
                        Me.printStatus.nPageTo = num2
                    End If
                Else
                    Me.printStatus.nPageFrom = 0
                    Me.printStatus.nPageTo = 0
                End If
                If (Me.iViewControl = 1) Then
                    Dim paperSource As CrystalDecisions.Shared.PaperSource = Me.rptData.PrintOptions.PaperSource
                    Me.GetDevMode(Me.GetPrinterName)
                    If (Me.paperSourceKind <> PaperSourceKind.Custom) Then
                        Me.rptData.PrintOptions.PaperSource = DirectCast(Me.paperSourceKind, CrystalDecisions.Shared.PaperSource)
                    End If
                    If Not CommonDataClass.Instance.IsTrainingMode Then
                        Me.rptData.PrintToPrinter(Me.iPrintCount, True, Me.printStatus.nPageFrom, Me.printStatus.nPageTo)
                    End If
                    Me.iRenCode = 1
                    Me.rptData.PrintOptions.PaperSource = paperSource
                Else
                    Me.iRenCode = 2
                End If
                MyBase.Close
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "RE0001", New String(0  - 1) {})
            End Try
        End Sub

        Private Sub btnSelect2_Click(ByVal sender As Object, ByVal e As EventArgs)
            Try 
                Me.iRenCode = 3
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "RE0001", New String(0  - 1) {})
            Finally
                MyBase.Close
            End Try
        End Sub

        <DllImport("winspool.drv", CharSet:=CharSet.Auto, SetLastError:=True)> _
        Private Shared Function ClosePrinter(ByVal hPrinter As IntPtr) As Boolean
        End Function

        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If (disposing AndAlso (Not Me.components Is Nothing)) Then
                Me.components.Dispose
            End If
            MyBase.Dispose(disposing)
        End Sub

        <DllImport("winspool.Drv", EntryPoint:="DocumentPropertiesA", CallingConvention:=CallingConvention.StdCall, SetLastError:=True, ExactSpelling:=True)> _
        Private Shared Function DocumentProperties(ByVal hwnd As IntPtr, ByVal hPrinter As IntPtr, <MarshalAs(UnmanagedType.LPStr)> ByVal pDeviceNameg As String, ByVal pDevModeOutput As IntPtr, ByRef pDevModeInput As IntPtr, ByVal fMode As Integer) As Integer
        End Function

        Private Sub FrmReportViewer_Load(ByVal sender As Object, ByVal e As EventArgs)
            Try 
                If Not Me.grpPrintRange.Visible Then
                    Me.lblPrintCnt.Location = New Point((Me.lblPrintCnt.Location.X - (Me.grpPrintRange.Size.Width - 30)), Me.lblPrintCnt.Location.Y)
                    Me.nudPrintCnt.Location = New Point((Me.nudPrintCnt.Location.X - (Me.grpPrintRange.Size.Width - 30)), Me.nudPrintCnt.Location.Y)
                    Me.btnEntryAndPrint.Location = New Point((Me.btnEntryAndPrint.Location.X - (Me.grpPrintRange.Size.Width - 30)), Me.btnEntryAndPrint.Location.Y)
                    Me.btnEntry.Location = New Point((Me.btnEntry.Location.X - (Me.grpPrintRange.Size.Width - 30)), Me.btnEntry.Location.Y)
                    Me.btnCancel.Location = New Point((Me.btnCancel.Location.X - (Me.grpPrintRange.Size.Width - 30)), Me.btnCancel.Location.Y)
                End If
                If (Me.iViewControl = 0) Then
                    Me.btnEntryAndPrint.Text = "登録＆印刷"
                    Me.btnEntry.Visible = True
                    Me.btnEntry.Text = "登録のみ"
                ElseIf (Me.iViewControl = 1) Then
                    Me.btnEntryAndPrint.Text = "印刷"
                    If Me.grpPrintRange.Visible Then
                        Me.btnEntryAndPrint.Location = New Point(Me.btnEntry.Location.X, Me.btnEntry.Location.Y)
                    End If
                    Me.btnEntry.Visible = False
                ElseIf (Me.iViewControl = 2) Then
                    Me.btnEntryAndPrint.Text = "登録（印刷）"
                    If Me.grpPrintRange.Visible Then
                        Me.btnEntryAndPrint.Location = New Point(Me.btnEntry.Location.X, Me.btnEntry.Location.Y)
                    End If
                    Me.btnEntry.Visible = False
                End If
                Me.crtViewer.ReportSource = Me.rptData
                Me.txtMessage.Visible = False
                If (CommonDataClass.Instance.IsTrainingMode AndAlso ((Me.iViewControl = 0) OrElse (Me.iViewControl = 1))) Then
                    Me.btnEntryAndPrint.Enabled = False
                End If
                Dim currentPageNumber As Integer = Me.crtViewer.GetCurrentPageNumber
                Me.crtViewer.ShowLastPage
                Me.nMaxPageCount = Me.crtViewer.GetCurrentPageNumber
                Me.crtViewer.ShowNthPage(currentPageNumber)
                Me.nudPageFrom.Maximum = Me.nMaxPageCount
                Me.nudPageTo.Maximum = Me.nMaxPageCount
                Me.crtViewer.Zoom(2)
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "RE0001", New String(0  - 1) {})
            End Try
        End Sub

        Public Function GetDevMode(ByVal printerName As String) As DEVMODE
            Dim ptr As IntPtr
            Dim devmode2 As DEVMODE
            If Not FrmReportViewer.OpenPrinter(printerName, ptr, IntPtr.Zero) Then
                Throw New Win32Exception(Marshal.GetLastWin32Error)
            End If
            Try 
                Dim zero As IntPtr = IntPtr.Zero
                Dim pDevModeOutput As IntPtr = Marshal.AllocCoTaskMem((FrmReportViewer.DocumentProperties(IntPtr.Zero, ptr, printerName, zero, zero, 0) + 100))
                FrmReportViewer.DocumentProperties(IntPtr.Zero, ptr, printerName, pDevModeOutput, zero, 2)
                Dim devmode As DEVMODE = DirectCast(Marshal.PtrToStructure(pDevModeOutput, GetType(DEVMODE)), DEVMODE)
                devmode2 = devmode
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "RE0001", New String(0  - 1) {})
            Finally
                FrmReportViewer.ClosePrinter(ptr)
            End Try
            Return devmode2
        End Function

        <DllImport("winspool.drv", CharSet:=CharSet.Auto, SetLastError:=True)> _
        Private Shared Function GetPrinter(ByVal hPrinter As IntPtr, ByVal dwLevel As Integer, ByVal pPrinter As IntPtr, ByVal cbBuf As Integer, <Out> ByRef pcbNeeded As Integer) As Boolean
        End Function

        Public Function GetPrinterInfo(ByVal printerName As String) As PRINTER_INFO_2
            Dim ptr As IntPtr
            Dim printer_info_2 As PRINTER_INFO_2
            If Not FrmReportViewer.OpenPrinter(printerName, ptr, IntPtr.Zero) Then
                Throw New Win32Exception(Marshal.GetLastWin32Error)
            End If
            Dim zero As IntPtr = IntPtr.Zero
            Try 
                Dim num As Integer
                Dim num2 As Integer
                FrmReportViewer.GetPrinter(ptr, 2, IntPtr.Zero, 0, num)
                If (num <= 0) Then
                    Throw New SysUnionException(MethodBase.GetCurrentMethod, New Exception, "BE0001", New String(0  - 1) {})
                End If
                zero = Marshal.AllocHGlobal(num)
                If Not FrmReportViewer.GetPrinter(ptr, 2, zero, num, num2) Then
                    Throw New Win32Exception(Marshal.GetLastWin32Error)
                End If
                Dim printer_info_ As PRINTER_INFO_2 = DirectCast(Marshal.PtrToStructure(zero, GetType(PRINTER_INFO_2)), PRINTER_INFO_2)
                printer_info_2 = printer_info_
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "BE0001", New String(0  - 1) {})
            Finally
                FrmReportViewer.ClosePrinter(ptr)
                Marshal.FreeHGlobal(zero)
            End Try
            Return printer_info_2
        End Function

        Public Function GetPrinterName() As String
            Dim printerName As String
            Try 
                printerName = Me.printDoc.PrinterSettings.PrinterName
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "BE0001", New String(0  - 1) {})
            End Try
            Return printerName
        End Function

        Private Sub InitializeComponent()
            Dim manager As New ComponentResourceManager(GetType(FrmReportViewer))
            Me.btnEntryAndPrint = New Button
            Me.btnCancel = New Button
            Me.crtViewer = New CrystalReportViewer
            Me.btnEntry = New Button
            Me.txtMessage = New TextBox
            Me.lblPrintCnt = New Label
            Me.nudPrintCnt = New NumericUpDown
            Me.grpPrintRange = New GroupBox
            Me.lblPageTo = New Label
            Me.lblPageFrom = New Label
            Me.nudPageTo = New NumericUpDown
            Me.nudPageFrom = New NumericUpDown
            Me.rbPrintRangeAppoint = New RadioButton
            Me.rbPrintRangeAll = New RadioButton
            Me.nudPrintCnt.BeginInit
            Me.grpPrintRange.SuspendLayout
            Me.nudPageTo.BeginInit
            Me.nudPageFrom.BeginInit
            MyBase.SuspendLayout
            Me.btnEntryAndPrint.Location = New Point(&H2B9, &H37E)
            Me.btnEntryAndPrint.Name = "btnEntryAndPrint"
            Me.btnEntryAndPrint.Size = New Size(&H74, &H20)
            Me.btnEntryAndPrint.TabIndex = 0
            Me.btnEntryAndPrint.Text = "登録＆印刷"
            Me.btnEntryAndPrint.UseVisualStyleBackColor = True
            AddHandler Me.btnEntryAndPrint.Click, New EventHandler(AddressOf Me.btnSelect1_Click)
            Me.btnCancel.Location = New Point(&H3E6, &H37E)
            Me.btnCancel.Name = "btnCancel"
            Me.btnCancel.Size = New Size(&H74, &H20)
            Me.btnCancel.TabIndex = 1
            Me.btnCancel.Text = "キャンセル"
            Me.btnCancel.UseVisualStyleBackColor = True
            AddHandler Me.btnCancel.Click, New EventHandler(AddressOf Me.btnCancel_Click)
            Me.crtViewer.ActiveViewIndex = -1
            Me.crtViewer.BorderStyle = BorderStyle.FixedSingle
            Me.crtViewer.DisplayGroupTree = False
            Me.crtViewer.Location = New Point(-2, -1)
            Me.crtViewer.Name = "crtViewer"
            Me.crtViewer.SelectionFormula = ""
            Me.crtViewer.ShowCloseButton = False
            Me.crtViewer.ShowExportButton = False
            Me.crtViewer.ShowGroupTreeButton = False
            Me.crtViewer.ShowPrintButton = False
            Me.crtViewer.ShowRefreshButton = False
            Me.crtViewer.ShowTextSearchButton = False
            Me.crtViewer.Size = New Size(&H4AF, &H35E)
            Me.crtViewer.TabIndex = 2
            Me.crtViewer.ViewTimeSelectionFormula = ""
            Me.btnEntry.Location = New Point(&H351, &H37E)
            Me.btnEntry.Name = "btnEntry"
            Me.btnEntry.Size = New Size(&H74, &H20)
            Me.btnEntry.TabIndex = 3
            Me.btnEntry.Text = "登録のみ"
            Me.btnEntry.UseVisualStyleBackColor = True
            AddHandler Me.btnEntry.Click, New EventHandler(AddressOf Me.btnSelect2_Click)
            Me.txtMessage.BackColor = SystemColors.MenuBar
            Me.txtMessage.BorderStyle = BorderStyle.None
            Me.txtMessage.Font = New Font("MS UI Gothic", 26.25!, FontStyle.Regular, GraphicsUnit.Point, &H80)
            Me.txtMessage.Location = New Point(&H176, &H17A)
            Me.txtMessage.Multiline = True
            Me.txtMessage.Name = "txtMessage"
            Me.txtMessage.Size = New Size(&H20B, &H54)
            Me.txtMessage.TabIndex = 5
            Me.txtMessage.Text = "印刷用のデータを読み込んでいます。しばらく、お待ちください。"
            Me.lblPrintCnt.AutoSize = True
            Me.lblPrintCnt.Location = New Point(&H1DD, &H386)
            Me.lblPrintCnt.Name = "lblPrintCnt"
            Me.lblPrintCnt.Size = New Size(80, &H10)
            Me.lblPrintCnt.TabIndex = 6
            Me.lblPrintCnt.Text = "印刷部数："
            Me.lblPrintCnt.Visible = False
            Me.nudPrintCnt.Location = New Point(&H233, 900)
            Me.nudPrintCnt.Name = "nudPrintCnt"
            Me.nudPrintCnt.Size = New Size(50, &H17)
            Me.nudPrintCnt.TabIndex = 8
            Me.nudPrintCnt.TextAlign = HorizontalAlignment.Right
            Dim bits As Integer() = New Integer(4  - 1) {}
            bits(0) = 1
            Me.nudPrintCnt.Value = New Decimal(bits)
            Me.nudPrintCnt.Visible = False
            Me.grpPrintRange.Controls.Add(Me.lblPageTo)
            Me.grpPrintRange.Controls.Add(Me.lblPageFrom)
            Me.grpPrintRange.Controls.Add(Me.nudPageTo)
            Me.grpPrintRange.Controls.Add(Me.nudPageFrom)
            Me.grpPrintRange.Controls.Add(Me.rbPrintRangeAppoint)
            Me.grpPrintRange.Controls.Add(Me.rbPrintRangeAll)
            Me.grpPrintRange.Location = New Point(&H5B, &H363)
            Me.grpPrintRange.Name = "grpPrintRange"
            Me.grpPrintRange.Size = New Size(340, 80)
            Me.grpPrintRange.TabIndex = 9
            Me.grpPrintRange.TabStop = False
            Me.grpPrintRange.Text = "印刷範囲"
            Me.grpPrintRange.Visible = False
            Me.lblPageTo.AutoSize = True
            Me.lblPageTo.Font = New Font("MS UI Gothic", 11.25!, FontStyle.Regular, GraphicsUnit.Point, &H80)
            Me.lblPageTo.Location = New Point(&H10F, &H34)
            Me.lblPageTo.Name = "lblPageTo"
            Me.lblPageTo.Size = New Size(&H42, 15)
            Me.lblPageTo.TabIndex = 5
            Me.lblPageTo.Text = "ページまで"
            Me.lblPageFrom.AutoSize = True
            Me.lblPageFrom.Font = New Font("MS UI Gothic", 11.25!, FontStyle.Regular, GraphicsUnit.Point, &H80)
            Me.lblPageFrom.Location = New Point(&H98, &H34)
            Me.lblPageFrom.Name = "lblPageFrom"
            Me.lblPageFrom.Size = New Size(&H41, 15)
            Me.lblPageFrom.TabIndex = 4
            Me.lblPageFrom.Text = "ページから"
            Me.nudPageTo.Location = New Point(&HDB, &H2F)
            Dim numArray2 As Integer() = New Integer(4  - 1) {}
            numArray2(0) = 1
            Me.nudPageTo.Minimum = New Decimal(numArray2)
            Me.nudPageTo.Name = "nudPageTo"
            Me.nudPageTo.Size = New Size(&H31, &H17)
            Me.nudPageTo.TabIndex = 3
            Dim numArray3 As Integer() = New Integer(4  - 1) {}
            numArray3(0) = 1
            Me.nudPageTo.Value = New Decimal(numArray3)
            Me.nudPageFrom.Location = New Point(&H66, &H2F)
            Dim numArray4 As Integer() = New Integer(4  - 1) {}
            numArray4(0) = 1
            Me.nudPageFrom.Minimum = New Decimal(numArray4)
            Me.nudPageFrom.Name = "nudPageFrom"
            Me.nudPageFrom.Size = New Size(&H31, &H17)
            Me.nudPageFrom.TabIndex = 2
            Dim numArray5 As Integer() = New Integer(4  - 1) {}
            numArray5(0) = 1
            Me.nudPageFrom.Value = New Decimal(numArray5)
            Me.rbPrintRangeAppoint.AutoSize = True
            Me.rbPrintRangeAppoint.Location = New Point(6, &H30)
            Me.rbPrintRangeAppoint.Name = "rbPrintRangeAppoint"
            Me.rbPrintRangeAppoint.Size = New Size(&H61, 20)
            Me.rbPrintRangeAppoint.TabIndex = 1
            Me.rbPrintRangeAppoint.Text = "ページ指定"
            Me.rbPrintRangeAppoint.UseVisualStyleBackColor = True
            AddHandler Me.rbPrintRangeAppoint.CheckedChanged, New EventHandler(AddressOf Me.rbPrintRange_CheckedChanged)
            Me.rbPrintRangeAll.AutoSize = True
            Me.rbPrintRangeAll.Checked = True
            Me.rbPrintRangeAll.Location = New Point(6, &H16)
            Me.rbPrintRangeAll.Name = "rbPrintRangeAll"
            Me.rbPrintRangeAll.Size = New Size(&H42, 20)
            Me.rbPrintRangeAll.TabIndex = 0
            Me.rbPrintRangeAll.TabStop = True
            Me.rbPrintRangeAll.Text = "すべて"
            Me.rbPrintRangeAll.UseVisualStyleBackColor = True
            AddHandler Me.rbPrintRangeAll.CheckedChanged, New EventHandler(AddressOf Me.rbPrintRange_CheckedChanged)
            MyBase.AutoScaleDimensions = New SizeF(9!, 16!)
            MyBase.AutoScaleMode = AutoScaleMode.Font
            MyBase.ClientSize = New Size(&H4AD, &H3BF)
            MyBase.ControlBox = False
            MyBase.Controls.Add(Me.grpPrintRange)
            MyBase.Controls.Add(Me.nudPrintCnt)
            MyBase.Controls.Add(Me.lblPrintCnt)
            MyBase.Controls.Add(Me.txtMessage)
            MyBase.Controls.Add(Me.btnEntry)
            MyBase.Controls.Add(Me.crtViewer)
            MyBase.Controls.Add(Me.btnCancel)
            MyBase.Controls.Add(Me.btnEntryAndPrint)
            Me.Font = New Font("MS UI Gothic", 12!)
            MyBase.FormBorderStyle = FormBorderStyle.FixedSingle
            'TODO MyBase.Icon = DirectCast(manager.GetObject("$this.Icon"), Icon)
            MyBase.Margin = New Padding(4)
            MyBase.MaximizeBox = False
            MyBase.MinimizeBox = False
            MyBase.Name = "FrmReportViewer"
            MyBase.StartPosition = FormStartPosition.CenterParent
            Me.Text = "印刷プレビュー"
            AddHandler MyBase.Load, New EventHandler(AddressOf Me.FrmReportViewer_Load)
            Me.nudPrintCnt.EndInit
            Me.grpPrintRange.ResumeLayout(False)
            Me.grpPrintRange.PerformLayout
            Me.nudPageTo.EndInit
            Me.nudPageFrom.EndInit
            MyBase.ResumeLayout(False)
            MyBase.PerformLayout
        End Sub

        <DllImport("winspool.drv", CharSet:=CharSet.Auto, SetLastError:=True)> _
        Private Shared Function OpenPrinter(ByVal pPrinterName As String, <Out> ByRef hPrinter As IntPtr, ByVal pDefault As IntPtr) As Boolean
        End Function

        Public Sub PrintToPrinter()
            Try 
                Me.GetDevMode(Me.GetPrinterName)
                Dim paperSource As CrystalDecisions.Shared.PaperSource = Me.rptData.PrintOptions.PaperSource
                If (Me.paperSourceKind <> PaperSourceKind.Custom) Then
                    Me.rptData.PrintOptions.PaperSource = DirectCast(Me.paperSourceKind, CrystalDecisions.Shared.PaperSource)
                End If
                If Not CommonDataClass.Instance.IsTrainingMode Then
                    Me.rptData.PrintToPrinter(Me.iPrintCount, True, Me.printStatus.nPageFrom, Me.printStatus.nPageTo)
                End If
                Me.rptData.PrintOptions.PaperSource = paperSource
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "RE0001", New String(0  - 1) {})
            End Try
        End Sub

        Private Sub rbPrintRange_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs)
            If Me.rbPrintRangeAll.Checked Then
                Me.nudPageFrom.Enabled = False
                Me.nudPageTo.Enabled = False
            Else
                Me.nudPageFrom.Enabled = True
                Me.nudPageTo.Enabled = True
            End If
        End Sub

        Public Sub RptDataDispose()
            Try 
                If (Not Me.rptData Is Nothing) Then
                    Me.rptData.Close
                    Me.rptData.Dispose
                End If
                If (Not Me.printDoc Is Nothing) Then
                    Me.printDoc.Dispose
                End If
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "RE0001", New String(0  - 1) {})
            End Try
        End Sub

        Public Sub SetDocumentName(ByVal strDocName As String)
            Try 
                Me.printDoc.DocumentName = strDocName
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "RE0001", New String(0  - 1) {})
            End Try
        End Sub


        ' Fields
        Private btnCancel As Button
        Private btnEntry As Button
        Private btnEntryAndPrint As Button
        Private components As IContainer
        Private crtViewer As CrystalReportViewer
        Private Const DM_OUT_BUFFER As Integer = 2
        Protected dSetReport As DataSet
        Private grpPrintRange As GroupBox
        Private iPrintCount As Integer
        Public iRenCode As Integer
        Public iViewControl As Integer
        Private lblPageFrom As Label
        Private lblPageTo As Label
        Private lblPrintCnt As Label
        Private nMaxPageCount As Integer
        Private nudPageFrom As NumericUpDown
        Private nudPageTo As NumericUpDown
        Private nudPrintCnt As NumericUpDown
        Private paperSourceKind As PaperSourceKind
        Public piPrintCount As Integer
        Private printDoc As PrintDocument
        Private printStatus As tagPrintStatus
        Private rbPrintRangeAll As RadioButton
        Private rbPrintRangeAppoint As RadioButton
        Private rptData As ReportClass
        Private txtMessage As TextBox

        ' Nested Types
        <StructLayout(LayoutKind.Sequential)> _
        Public Structure DEVMODE
            <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=&H20)> _
            Public dmDeviceName As String
            Public dmSpecVersion As Short
            Public dmDriverVersion As Short
            Public dmSize As Short
            Public dmDriverExtra As Short
            Public dmFields As Integer
            Public dmOrientation As Short
            Public dmPaperSize As Short
            Public dmPaperLength As Short
            Public dmPaperWidth As Short
            Public dmScale As Short
            Public dmCopies As Short
            Public dmDefaultSource As Short
            Public dmPrintQuality As Short
            Public dmColor As Short
            Public dmDuplex As Short
            Public dmYResolution As Short
            Public dmTTOption As Short
            Public dmCollate As Short
            <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=&H20)> _
            Public dmFormName As String
            Public dmLogPixels As Short
            Public dmBitsPerPel As Short
            Public dmPelsWidth As Integer
            Public dmPelsHeight As Integer
            Public dmDisplayFlags As Integer
            Public dmDisplayFrequency As Integer
            Public dmICMMethod As Integer
            Public dmICMIntent As Integer
            Public dmMediaType As Integer
            Public dmDitherType As Integer
            Public dmReserved1 As Integer
            Public dmReserved2 As Integer
            Public dmPanningWidth As Integer
            Public dmPanningHeight As Integer
        End Structure

        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Auto)> _
        Public Structure PRINTER_INFO_2
            Public pServerName As String
            Public pPrinterName As String
            Public pShareName As String
            Public pPortName As String
            Public pDriverName As String
            Public pComment As String
            Public pLocation As String
            Public pDevMode As IntPtr
            Public pSepFile As String
            Public pPrintProcessor As String
            Public pDatatype As String
            Public pParameters As String
            Public pSecurityDescriptor As IntPtr
            Public Attributes As UInt32
            Public Priority As UInt32
            Public DefaultPriority As UInt32
            Public StartTime As UInt32
            Public UntilTime As UInt32
            Public Status As UInt32
            Public cJobs As UInt32
            Public AveragePPM As UInt32
        End Structure
    End Class
End Namespace
