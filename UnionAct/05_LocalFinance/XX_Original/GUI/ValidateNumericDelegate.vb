'Imports GUI.UnionComponent
Imports System
Imports System.Runtime.CompilerServices
Imports C1.Win.C1FlexGrid

Namespace GUI.Common
    Public Delegate Sub ValidateNumericDelegate(ByVal flxFirer As C1FlexGrid, ByVal lngValue As Long, ByVal iRow As Integer, ByVal iCol As Integer)
End Namespace
