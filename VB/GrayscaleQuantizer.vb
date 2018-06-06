' 
'  THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF 
'  ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO 
'  THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A 
'  PARTICULAR PURPOSE. 
'  
'    This is sample code and is freely distributable. 
' 

Imports System.Collections

Namespace ImageQuantization
	''' <summary>
	''' Summary description for PaletteQuantizer.
	''' </summary>
	Public Class GrayscaleQuantizer
		Inherits PaletteQuantizer
		''' <summary>
		''' Construct the palette quantizer
		''' </summary>
		''' <remarks>
		''' Palette quantization only requires a single quantization step
		''' </remarks>
		Public Sub New()
			MyBase.New(New ArrayList())
			_colors = New Color(255){}

			Dim nColors As Integer = 256

			' Initialize a new color table with entries that are determined
			' by some optimal palette-finding algorithm; for demonstration 
			' purposes, use a grayscale.
			For i As UInteger = 0 To nColors - 1
				Dim Alpha As UInteger = &HFF ' Colors are opaque.
				Dim Intensity As UInteger = Convert.ToUInt32(i*&HFF/(nColors-1)) ' Even distribution.

				' The GIF encoder makes the first entry in the palette
				' that has a ZERO alpha the transparent color in the GIF.
				' Pick the first one arbitrarily, for demonstration purposes.

				' Create a gray scale for demonstration purposes.
				' Otherwise, use your favorite color reduction algorithm
				' and an optimum palette for that algorithm generated here.
				' For example, a color histogram, or a median cut palette.
				_colors(i) = Color.FromArgb(CInt(Fix(Alpha)), CInt(Fix(Intensity)), CInt(Fix(Intensity)), CInt(Fix(Intensity)))
			Next i
		End Sub

		''' <summary>
		''' Override this to process the pixel in the second pass of the algorithm
		''' </summary>
		''' <param name="pixel">The pixel to quantize</param>
		''' <returns>The quantized value</returns>
		Protected Overrides Function QuantizePixel(ByVal pixel As Color32) As Byte
			Dim colorIndex As Byte = 0

			Dim luminance As Double = (pixel.Red *0.299) + (pixel.Green *0.587) + (pixel.Blue *0.114)

			' Gray scale is an intensity map from black to white.
			' Compute the index to the grayscale entry that
			' approximates the luminance, and then round the index.
			' Also, constrain the index choices by the number of
			' colors to do, and then set that pixel's index to the 
			' byte value.
			colorIndex = CByte(luminance +0.5)

			Return colorIndex
		End Function

	End Class
End Namespace
