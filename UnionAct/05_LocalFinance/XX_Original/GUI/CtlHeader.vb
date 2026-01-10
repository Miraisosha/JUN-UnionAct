Imports UnionAct.Framework.Interface
Imports System
Imports System.ComponentModel
Imports System.Drawing
Imports System.Windows.Forms

Namespace GUI.Common.UnionForm
    Public Class CtlHeader
        Inherits UserControl
        ' Methods
        Public Sub New()
            Me.InitializeComponent()
        End Sub

        Private Sub CtlHeader_Load(ByVal sender As Object, ByVal e As EventArgs)
            MyBase.Tag = Me.lblHeader
        End Sub

        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If (disposing AndAlso (Not Me.components Is Nothing)) Then
                Me.components.Dispose
            End If
            MyBase.Dispose(disposing)
        End Sub

        Private Sub InitializeComponent()
            Me.lblHeader = New Label
            Me.pnlHeader = New Panel
            Me.pnlHeader.SuspendLayout
            MyBase.SuspendLayout
            Me.lblHeader.AutoSize = True
            Me.lblHeader.Font = New Font("MS UI Gothic", 21.75!, FontStyle.Regular, GraphicsUnit.Point, &H80)
            Me.lblHeader.Location = New Point(&H10D, 5)
            Me.lblHeader.Name = "lblHeader"
            Me.lblHeader.Size = New Size(&H53, &H1D)
            Me.lblHeader.TabIndex = 0
            Me.lblHeader.Tag = "lblHeader"
            Me.lblHeader.Text = "label1"
            Me.pnlHeader.BackColor = Color.LightCyan
            Me.pnlHeader.BorderStyle = BorderStyle.Fixed3D
            Me.pnlHeader.Controls.Add(Me.lblHeader)
            Me.pnlHeader.Location = New Point(&HC9, &H13)
            Me.pnlHeader.Name = "pnlHeader"
            Me.pnlHeader.Size = New Size(&H271, &H26)
            Me.pnlHeader.TabIndex = 1
            MyBase.AutoScaleDimensions = New SizeF(9!, 16!)
            MyBase.AutoScaleMode = AutoScaleMode.Font
            MyBase.Controls.Add(Me.pnlHeader)
            Me.Font = New Font("MS UI Gothic", 12!, FontStyle.Regular, GraphicsUnit.Point, &H80)
            MyBase.Margin = New Padding(4)
            MyBase.Name = "CtlHeader"
            MyBase.Size = New Size(&H402, 60)
            AddHandler MyBase.Load, New EventHandler(AddressOf Me.CtlHeader_Load)
            Me.pnlHeader.ResumeLayout(False)
            Me.pnlHeader.PerformLayout
            MyBase.ResumeLayout(False)
        End Sub


        ' Fields
        Private components As IContainer
        Private lblHeader As Label
        Private pnlHeader As Panel
    End Class
End Namespace
