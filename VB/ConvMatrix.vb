Imports System.Text

Namespace ImageQuantization
	Public Class ConvMatrix
		Public TopLeft As Integer = 0, TopMid As Integer = 0, TopRight As Integer = 0
		Public MidLeft As Integer = 0, Pixel As Integer = 1, MidRight As Integer = 0
		Public BottomLeft As Integer = 0, BottomMid As Integer = 0, BottomRight As Integer = 0
		Public Factor As Integer = 1
		Public Offset As Integer = 0
		Public Sub SetAll(ByVal nVal As Integer)
			BottomRight = nVal
			BottomMid = BottomRight
			BottomLeft = BottomMid
			MidRight = BottomLeft
			Pixel = MidRight
			MidLeft = Pixel
			TopRight = MidLeft
			TopMid = TopRight
			TopLeft = TopMid
		End Sub
	End Class
End Namespace
