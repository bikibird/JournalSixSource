' 
'  THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF 
'  ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO 
'  THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A 
'  PARTICULAR PURPOSE. 
'  
'    This is sample code and is freely distributable. 
'

Imports System.Collections
Imports System.Drawing.Imaging

Namespace ImageQuantization
	''' <summary>
	''' Summary description for PaletteQuantizer.
	''' </summary>
	Public Class PaletteQuantizer
		Inherits Quantizer
		''' <summary>
		''' Construct the palette quantizer
		''' </summary>
		''' <param name="palette">The color palette to quantize to</param>
		''' <remarks>
		''' Palette quantization only requires a single quantization step
		''' </remarks>
		Public Sub New(ByVal palette As ArrayList)
			MyBase.New(True)
			_colorMap = New Hashtable ()

			_colors = New Color(palette.Count - 1){}
			palette.CopyTo (_colors)
		End Sub

		''' <summary>
		''' Override this to process the pixel in the second pass of the algorithm
		''' </summary>
		''' <param name="pixel">The pixel to quantize</param>
		''' <returns>The quantized value</returns>
		Protected Overrides Function QuantizePixel(ByVal pixel As Color32) As Byte
			Dim colorIndex As Byte = 0
			Dim colorHash As Integer = pixel.ARGB

			' Check if the color is in the lookup table
			If _colorMap.ContainsKey (colorHash) Then
				colorIndex = CByte(_colorMap(colorHash))
			Else
				' Not found - loop through the palette and find the nearest match.
				' Firstly check the alpha value - if 0, lookup the transparent color
				If 0 = pixel.Alpha Then
					' Transparent. Lookup the first color with an alpha value of 0
					For index As Integer = 0 To _colors.Length - 1
						If 0 = _colors(index).A Then
							colorIndex = CByte(index)
							Exit For
						End If
					Next index
				Else
					' Not transparent...
					Dim leastDistance As Integer = Integer.MaxValue
					Dim red As Integer = pixel.Red
					Dim green As Integer = pixel.Green
					Dim blue As Integer = pixel.Blue

					' Loop through the entire palette, looking for the closest color match
					For index As Integer = 0 To _colors.Length - 1
						Dim paletteColor As Color = _colors(index)

						Dim redDistance As Integer = paletteColor.R - red
						Dim greenDistance As Integer = paletteColor.G - green
						Dim blueDistance As Integer = paletteColor.B - blue

						Dim distance As Integer = (redDistance * redDistance) + (greenDistance * greenDistance) + (blueDistance * blueDistance)

						If distance < leastDistance Then
							colorIndex = CByte(index)
							leastDistance = distance

							' And if it's an exact match, exit the loop
							If 0 = distance Then
								Exit For
							End If
						End If
					Next index
				End If

				' Now I have the color, pop it into the hashtable for next time
				_colorMap.Add (colorHash, colorIndex)
			End If

			Return colorIndex
		End Function

		''' <summary>
		''' Retrieve the palette for the quantized image
		''' </summary>
		''' <param name="palette">Any old palette, this is overrwritten</param>
		''' <returns>The new color palette</returns>
		Protected Overrides Function GetPalette(ByVal palette As ColorPalette) As ColorPalette
			For index As Integer = 0 To _colors.Length - 1
				palette.Entries(index) = _colors(index)
			Next index

			Return palette
		End Function

		''' <summary>
		''' Lookup table for colors
		''' </summary>
		Private _colorMap As Hashtable

		''' <summary>
		''' List of all colors in the palette
		''' </summary>
		Protected _colors() As Color
	End Class
End Namespace
