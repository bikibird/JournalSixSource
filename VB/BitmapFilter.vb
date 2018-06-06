Imports System.Drawing.Imaging
Imports System.Text

Namespace ImageQuantization
	Public Class BitmapFilter
		Public Shared Function Invert(ByVal b As Bitmap) As Boolean
			' GDI+ still lies to us - the return format is BGR, NOT RGB.
			Dim bmData As BitmapData = b.LockBits(New Rectangle(0, 0, b.Width, b.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb)

			Dim stride As Integer = bmData.Stride
			Dim Scan0 As IntPtr = bmData.Scan0

'INSTANT VB TODO TASK: There is no equivalent to an 'unsafe' block in VB
'			unsafe
				Byte* p = CByte(CType(Scan0, void))

				Dim nOffset As Integer = stride - b.Width * 3
				Dim nWidth As Integer = b.Width * 3

				For y As Integer = 0 To b.Height - 1
					For x As Integer = 0 To nWidth - 1
						p(0) = CByte(255 - p(0))
						p += 1
					Next x
					p += nOffset
				Next y
'INSTANT VB NOTE: End of the original C# 'unsafe' block

			b.UnlockBits(bmData)

			Return True
		End Function

		Public Shared Function GrayScale(ByVal b As Bitmap) As Boolean
			' GDI+ still lies to us - the return format is BGR, NOT RGB.
			Dim bmData As BitmapData = b.LockBits(New Rectangle(0, 0, b.Width, b.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb)

			Dim stride As Integer = bmData.Stride
			Dim Scan0 As IntPtr = bmData.Scan0

'INSTANT VB TODO TASK: There is no equivalent to an 'unsafe' block in VB
'			unsafe
				Byte* p = CByte(CType(Scan0, void))

				Dim nOffset As Integer = stride - b.Width * 3

				Dim red, green, blue As Byte

				For y As Integer = 0 To b.Height - 1
					For x As Integer = 0 To b.Width - 1
						blue = p(0)
						green = p(1)
						red = p(2)

						p(2) = CByte(.299 * red +.587 * green +.114 * blue)
						p(1) = p(2)
						p(0) = p(1)

						p += 3
					Next x
					p += nOffset
				Next y
'INSTANT VB NOTE: End of the original C# 'unsafe' block

			b.UnlockBits(bmData)

			Return True
		End Function

		Public Shared Function Brightness(ByVal b As Bitmap, ByVal nBrightness As Integer) As Boolean
			If nBrightness < -255 OrElse nBrightness > 255 Then
				Return False
			End If

			' GDI+ still lies to us - the return format is BGR, NOT RGB.
			Dim bmData As BitmapData = b.LockBits(New Rectangle(0, 0, b.Width, b.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb)

			Dim stride As Integer = bmData.Stride
			Dim Scan0 As IntPtr = bmData.Scan0

			Dim nVal As Integer = 0

'INSTANT VB TODO TASK: There is no equivalent to an 'unsafe' block in VB
'			unsafe
				Byte* p = CByte(CType(Scan0, void))

				Dim nOffset As Integer = stride - b.Width * 3
				Dim nWidth As Integer = b.Width * 3

				For y As Integer = 0 To b.Height - 1
					For x As Integer = 0 To nWidth - 1
						nVal = CInt(Fix(p(0) + nBrightness))

						If nVal < 0 Then
							nVal = 0
						End If
						If nVal > 255 Then
							nVal = 255
						End If

						p(0) = CByte(nVal)

						p += 1
					Next x
					p += nOffset
				Next y
'INSTANT VB NOTE: End of the original C# 'unsafe' block

			b.UnlockBits(bmData)

			Return True
		End Function

		Public Shared Function Contrast(ByVal b As Bitmap, ByVal nContrast As SByte) As Boolean
			If nContrast < -100 Then
				Return False
			End If
			If nContrast > 100 Then
				Return False
			End If

'INSTANT VB NOTE: The local variable contrast was renamed since Visual Basic will not allow local variables with the same name as their enclosing function or property:
            Dim pixel As Double = 0, contrast_Renamed As Double = (100.0 + nContrast) / 100.0

			contrast_Renamed *= contrast_Renamed

			Dim red, green, blue As Integer

			' GDI+ still lies to us - the return format is BGR, NOT RGB.
			Dim bmData As BitmapData = b.LockBits(New Rectangle(0, 0, b.Width, b.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb)

			Dim stride As Integer = bmData.Stride
			Dim Scan0 As IntPtr = bmData.Scan0

'INSTANT VB TODO TASK: There is no equivalent to an 'unsafe' block in VB
'			unsafe
				Byte* p = CByte(CType(Scan0, void))

				Dim nOffset As Integer = stride - b.Width * 3

				For y As Integer = 0 To b.Height - 1
					For x As Integer = 0 To b.Width - 1
						blue = p(0)
						green = p(1)
						red = p(2)

						pixel = red / 255.0
						pixel -= 0.5
						pixel *= contrast_Renamed
						pixel += 0.5
						pixel *= 255
						If pixel < 0 Then
							pixel = 0
						End If
						If pixel > 255 Then
							pixel = 255
						End If
						p(2) = CByte(pixel)

						pixel = green / 255.0
						pixel -= 0.5
						pixel *= contrast_Renamed
						pixel += 0.5
						pixel *= 255
						If pixel < 0 Then
							pixel = 0
						End If
						If pixel > 255 Then
							pixel = 255
						End If
						p(1) = CByte(pixel)

						pixel = blue / 255.0
						pixel -= 0.5
						pixel *= contrast_Renamed
						pixel += 0.5
						pixel *= 255
						If pixel < 0 Then
							pixel = 0
						End If
						If pixel > 255 Then
							pixel = 255
						End If
						p(0) = CByte(pixel)

						p += 3
					Next x
					p += nOffset
				Next y
'INSTANT VB NOTE: End of the original C# 'unsafe' block

			b.UnlockBits(bmData)

			Return True
		End Function

		Public Shared Function Gamma(ByVal b As Bitmap, ByVal red As Double, ByVal green As Double, ByVal blue As Double) As Boolean
			If red <.2 OrElse red > 5 Then
				Return False
			End If
			If green <.2 OrElse green > 5 Then
				Return False
			End If
			If blue <.2 OrElse blue > 5 Then
				Return False
			End If

			Dim redGamma(255) As Byte
			Dim greenGamma(255) As Byte
			Dim blueGamma(255) As Byte

			For i As Integer = 0 To 255
				redGamma(i) = CByte(Math.Min(255, CInt(Fix((255.0 * Math.Pow(i / 255.0, 1.0 / red)) + 0.5))))
				greenGamma(i) = CByte(Math.Min(255, CInt(Fix((255.0 * Math.Pow(i / 255.0, 1.0 / green)) + 0.5))))
				blueGamma(i) = CByte(Math.Min(255, CInt(Fix((255.0 * Math.Pow(i / 255.0, 1.0 / blue)) + 0.5))))
			Next i

			' GDI+ still lies to us - the return format is BGR, NOT RGB.
			Dim bmData As BitmapData = b.LockBits(New Rectangle(0, 0, b.Width, b.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb)

			Dim stride As Integer = bmData.Stride
			Dim Scan0 As IntPtr = bmData.Scan0

'INSTANT VB TODO TASK: There is no equivalent to an 'unsafe' block in VB
'			unsafe
				Byte* p = CByte(CType(Scan0,Byte))

				Dim nOffset As Integer = stride - b.Width * 3

				For y As Integer = 0 To b.Height - 1
					For x As Integer = 0 To b.Width - 1
						p(2) = redGamma(p(2))
						p(1) = greenGamma(p(1))
						p(0) = blueGamma(p(0))

						p += 3
					Next x
					p += nOffset
				Next y
'INSTANT VB NOTE: End of the original C# 'unsafe' block

			b.UnlockBits(bmData)

			Return True
		End Function

		Public Shared Function Color(ByVal b As Bitmap, ByVal red As Integer, ByVal green As Integer, ByVal blue As Integer) As Boolean
			If red < -255 OrElse red > 255 Then
				Return False
			End If
			If green < -255 OrElse green > 255 Then
				Return False
			End If
			If blue < -255 OrElse blue > 255 Then
				Return False
			End If

			' GDI+ still lies to us - the return format is BGR, NOT RGB.
			Dim bmData As BitmapData = b.LockBits(New Rectangle(0, 0, b.Width, b.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb)

			Dim stride As Integer = bmData.Stride
			Dim Scan0 As IntPtr = bmData.Scan0

'INSTANT VB TODO TASK: There is no equivalent to an 'unsafe' block in VB
'			unsafe
				Byte* p = CByte(CType(Scan0, void))

				Dim nOffset As Integer = stride - b.Width * 3
				Dim nPixel As Integer

				For y As Integer = 0 To b.Height - 1
					For x As Integer = 0 To b.Width - 1
						nPixel = p(2) + red
						nPixel = Math.Max(nPixel, 0)
						p(2) = CByte(Math.Min(255, nPixel))

						nPixel = p(1) + green
						nPixel = Math.Max(nPixel, 0)
						p(1) = CByte(Math.Min(255, nPixel))

						nPixel = p(0) + blue
						nPixel = Math.Max(nPixel, 0)
						p(0) = CByte(Math.Min(255, nPixel))

						p += 3
					Next x
					p += nOffset
				Next y
'INSTANT VB NOTE: End of the original C# 'unsafe' block

			b.UnlockBits(bmData)

			Return True
		End Function

		Public Shared Function Conv3x3(ByVal b As Bitmap, ByVal m As ConvMatrix) As Boolean
			' Avoid divide by zero errors
			If 0 = m.Factor Then
				Return False
			End If

			Dim bSrc As Bitmap = CType(b.Clone(), Bitmap)

			' GDI+ still lies to us - the return format is BGR, NOT RGB.
			Dim bmData As BitmapData = b.LockBits(New Rectangle(0, 0, b.Width, b.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb)
			Dim bmSrc As BitmapData = bSrc.LockBits(New Rectangle(0, 0, bSrc.Width, bSrc.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb)

			Dim stride As Integer = bmData.Stride
			Dim stride2 As Integer = stride * 2
			Dim Scan0 As IntPtr = bmData.Scan0
			Dim SrcScan0 As IntPtr = bmSrc.Scan0

'INSTANT VB TODO TASK: There is no equivalent to an 'unsafe' block in VB
'			unsafe
				Byte* p = CByte(CType(Scan0, void))
				Byte* pSrc = CByte(CType(SrcScan0, void))

				Dim nOffset As Integer = stride + 6 - b.Width * 3
				Dim nWidth As Integer = b.Width - 2
				Dim nHeight As Integer = b.Height - 2

				Dim nPixel As Integer

				For y As Integer = 0 To nHeight - 1
					For x As Integer = 0 To nWidth - 1
						nPixel = ((((pSrc(2) * m.TopLeft) + (pSrc(5) * m.TopMid) + (pSrc(8) * m.TopRight) + (pSrc(2 + stride) * m.MidLeft) + (pSrc(5 + stride) * m.Pixel) + (pSrc(8 + stride) * m.MidRight) + (pSrc(2 + stride2) * m.BottomLeft) + (pSrc(5 + stride2) * m.BottomMid) + (pSrc(8 + stride2) * m.BottomRight)) / m.Factor) + m.Offset)

						If nPixel < 0 Then
							nPixel = 0
						End If
						If nPixel > 255 Then
							nPixel = 255
						End If

						p(5 + stride) = CByte(nPixel)

						nPixel = ((((pSrc(1) * m.TopLeft) + (pSrc(4) * m.TopMid) + (pSrc(7) * m.TopRight) + (pSrc(1 + stride) * m.MidLeft) + (pSrc(4 + stride) * m.Pixel) + (pSrc(7 + stride) * m.MidRight) + (pSrc(1 + stride2) * m.BottomLeft) + (pSrc(4 + stride2) * m.BottomMid) + (pSrc(7 + stride2) * m.BottomRight)) / m.Factor) + m.Offset)

						If nPixel < 0 Then
							nPixel = 0
						End If
						If nPixel > 255 Then
							nPixel = 255
						End If

						p(4 + stride) = CByte(nPixel)

						nPixel = ((((pSrc(0) * m.TopLeft) + (pSrc(3) * m.TopMid) + (pSrc(6) * m.TopRight) + (pSrc(0 + stride) * m.MidLeft) + (pSrc(3 + stride) * m.Pixel) + (pSrc(6 + stride) * m.MidRight) + (pSrc(0 + stride2) * m.BottomLeft) + (pSrc(3 + stride2) * m.BottomMid) + (pSrc(6 + stride2) * m.BottomRight)) / m.Factor) + m.Offset)

						If nPixel < 0 Then
							nPixel = 0
						End If
						If nPixel > 255 Then
							nPixel = 255
						End If

						p(3 + stride) = CByte(nPixel)

						p += 3
						pSrc += 3
					Next x

					p += nOffset
					pSrc += nOffset
				Next y
'INSTANT VB NOTE: End of the original C# 'unsafe' block

			b.UnlockBits(bmData)
			bSrc.UnlockBits(bmSrc)

			Return True
		End Function
		Public Shared Function Smooth(ByVal b As Bitmap, ByVal nWeight As Integer) As Boolean ' default to 1
			Dim m As New ConvMatrix()
			m.SetAll(1)
			m.Pixel = nWeight
			m.Factor = nWeight + 8

			Return BitmapFilter.Conv3x3(b, m)
		End Function

		Public Shared Function GaussianBlur(ByVal b As Bitmap, ByVal nWeight As Integer) As Boolean ' default to 4
			Dim m As New ConvMatrix()
			m.SetAll(1)
			m.Pixel = nWeight
			m.BottomMid = 2
			m.MidRight = m.BottomMid
			m.MidLeft = m.MidRight
			m.TopMid = m.MidLeft
			m.Factor = nWeight + 12

			Return BitmapFilter.Conv3x3(b, m)
		End Function
		Public Shared Function MeanRemoval(ByVal b As Bitmap, ByVal nWeight As Integer) As Boolean ' default to 9
			Dim m As New ConvMatrix()
			m.SetAll(-1)
			m.Pixel = nWeight
			m.Factor = nWeight - 8

			Return BitmapFilter.Conv3x3(b, m)
		End Function
		Public Shared Function Sharpen(ByVal b As Bitmap, ByVal nWeight As Integer) As Boolean ' default to 11
			Dim m As New ConvMatrix()
			m.SetAll(0)
			m.Pixel = nWeight
			m.BottomMid = -2
			m.MidRight = m.BottomMid
			m.MidLeft = m.MidRight
			m.TopMid = m.MidLeft
			m.Factor = nWeight - 8

			Return BitmapFilter.Conv3x3(b, m)
		End Function
		Public Shared Function EmbossLaplacian(ByVal b As Bitmap) As Boolean
			Dim m As New ConvMatrix()
			m.SetAll(-1)
			m.BottomMid = 0
			m.MidRight = m.BottomMid
			m.MidLeft = m.MidRight
			m.TopMid = m.MidLeft
			m.Pixel = 4
			m.Offset = 127

			Return BitmapFilter.Conv3x3(b, m)
		End Function
		Public Shared Function EdgeDetectQuick(ByVal b As Bitmap) As Boolean
			Dim m As New ConvMatrix()
			m.TopRight = -1
			m.TopMid = m.TopRight
			m.TopLeft = m.TopMid
			m.MidRight = 0
			m.Pixel = m.MidRight
			m.MidLeft = m.Pixel
			m.BottomRight = 1
			m.BottomMid = m.BottomRight
			m.BottomLeft = m.BottomMid

			m.Offset = 127

			Return BitmapFilter.Conv3x3(b, m)
		End Function
	End Class
End Namespace
