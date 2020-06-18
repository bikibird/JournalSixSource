' 
'  THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF 
'  ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO 
'  THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A 
'  PARTICULAR PURPOSE. 
'  
'    This is sample code and is freely distributable. 
' 

Imports System.Drawing.Imaging
Imports System.Runtime.InteropServices

Public MustInherit Class Quantizer

    ''' <summary>
    ''' Construct the quantizer
    ''' </summary>
    ''' <param name="singlePass">If true, the quantization only needs to loop through the source pixels once</param>
    ''' <remarks>
    ''' If you construct this class with a true value for singlePass, then the code will, when quantizing your image,
    ''' only call the 'QuantizeImage' function. If two passes are required, the code will call 'InitialQuantizeImage'
    ''' and then 'QuantizeImage'.
    ''' </remarks>
    Public Sub New(ByVal singlePass As Boolean)
        _singlePass = singlePass
        _pixelSize = Marshal.SizeOf(GetType(Color32))
    End Sub

    ''' <summary>
    ''' Quantize an image and return the resulting output bitmap
    ''' </summary>
    ''' <param name="source">The image to quantize</param>
    ''' <returns>A quantized version of the image</returns>
    Public Function Quantize(ByVal source As Image) As Bitmap
        ' Get the size of the source image
        Dim height As Integer = source.Height
        Dim width As Integer = source.Width

        ' And construct a rectangle from these dimensions
        Dim bounds As New Rectangle(0, 0, width, height)

        ' First off take a 32bpp copy of the image
        Dim copy As New Bitmap(width, height, PixelFormat.Format32bppArgb)

        ' And construct an 8bpp version
        Dim output As New Bitmap(width, height, PixelFormat.Format8bppIndexed)

        ' Now lock the bitmap into memory
        Using g As Graphics = Graphics.FromImage(copy)
            g.PageUnit = GraphicsUnit.Pixel

            ' Draw the source image onto the copy bitmap,
            ' which will effect a widening as appropriate.
            g.DrawImage(source, bounds)

        End Using

        ' Define a pointer to the bitmap data
        Dim sourceData As BitmapData = Nothing

        Try
            ' Get the source image bits and lock into memory
            sourceData = copy.LockBits(bounds, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb)



            ' Call the FirstPass function if not a single pass algorithm.
            ' For something like an octree quantizer, this will run through
            ' all image pixels, build a data structure, and create a palette.
            If Not _singlePass Then
                FirstPass(sourceData, width, height)
            End If

            ' Then set the color palette on the output bitmap. I'm passing in the current palette 
            ' as there's no way to construct a new, empty palette.
            output.Palette = GetPalette(output.Palette)


            ' Then call the second pass which actually does the conversion
            SecondPass(sourceData, output, width, height, bounds)
        Finally
            ' Ensure that the bits are unlocked
            copy.UnlockBits(sourceData)
        End Try

        ' Last but not least, return the output bitmap
        Return output
    End Function

    ''' <summary>
    ''' Execute the first pass through the pixels in the image
    ''' </summary>
    ''' <param name="sourceData">The source data</param>
    ''' <param name="width">The width in pixels of the image</param>
    ''' <param name="height">The height in pixels of the image</param>
    Protected Overridable Sub FirstPass(ByVal sourceData As BitmapData, ByVal width As Integer, ByVal height As Integer)
        ' Define the source data pointers. The source row is a byte to
        ' keep addition of the stride value easier (as this is in bytes)              
        Dim pSourceRow As IntPtr = sourceData.Scan0

        ' Loop through each row
        For row As Integer = 0 To height - 1
            ' Set the source pixel to the first pixel in this row
            Dim pSourcePixel As IntPtr = pSourceRow

            ' And loop through each column
            For col As Integer = 0 To width - 1
                InitialQuantizePixel(New Color32(pSourcePixel))
                pSourcePixel = CType(CInt(pSourcePixel) + _pixelSize, IntPtr)
            Next col ' Now I have the pixel, call the FirstPassQuantize function...

            ' Add the stride to the source row
            pSourceRow = CType(CLng(pSourceRow) + sourceData.Stride, IntPtr)
        Next row
    End Sub

    ''' <summary>
    ''' Execute a second pass through the bitmap
    ''' </summary>
    ''' <param name="sourceData">The source bitmap, locked into memory</param>
    ''' <param name="output">The output bitmap</param>
    ''' <param name="width">The width in pixels of the image</param>
    ''' <param name="height">The height in pixels of the image</param>
    ''' <param name="bounds">The bounding rectangle</param>
    Protected Overridable Sub SecondPass(ByVal sourceData As BitmapData, ByVal output As Bitmap, ByVal width As Integer, ByVal height As Integer, ByVal bounds As Rectangle)
        Dim outputData As BitmapData = Nothing

        Try
            ' Lock the output bitmap into memory
            outputData = output.LockBits(bounds, ImageLockMode.WriteOnly, PixelFormat.Format8bppIndexed)

            ' Define the source data pointers. The source row is a byte to
            ' keep addition of the stride value easier (as this is in bytes)
            Dim pSourceRow As IntPtr = sourceData.Scan0
            Dim pSourcePixel As IntPtr = pSourceRow
            Dim pPreviousPixel As IntPtr = pSourcePixel

            ' Now define the destination data pointers
            Dim pDestinationRow As IntPtr = outputData.Scan0
            Dim pDestinationPixel As IntPtr = pDestinationRow

            ' And convert the first pixel, so that I have values going into the loop

            Dim pixelValue As Byte = QuantizePixel(New Color32(pSourcePixel))

            ' Assign the value of the first pixel
            Marshal.WriteByte(pDestinationPixel, pixelValue)

            ' Loop through each row
            For row As Integer = 0 To height - 1
                ' Set the source pixel to the first pixel in this row
                pSourcePixel = pSourceRow

                ' And set the destination pixel pointer to the first pixel in the row
                pDestinationPixel = pDestinationRow

                ' Loop through each pixel on this scan line
                For col As Integer = 0 To width - 1
                    ' Check if this is the same as the last pixel. If so use that value
                    ' rather than calculating it again. This is an inexpensive optimisation.
                    If Marshal.ReadInt32(pPreviousPixel) <> Marshal.ReadInt32(pSourcePixel) Then
                        pixelValue = QuantizePixel(New Color32(pSourcePixel))

                        ' And setup the previous pointer
                        pPreviousPixel = pSourcePixel
                    End If

                    ' And set the pixel in the output
                    Marshal.WriteByte(pDestinationPixel, pixelValue)

                    pSourcePixel = CType(CLng(pSourcePixel) + _pixelSize, IntPtr)
                    pDestinationPixel = CType(CLng(pDestinationPixel) + 1, IntPtr)

                Next col

                ' Add the stride to the source row
                pSourceRow = CType(CLng(pSourceRow) + sourceData.Stride, IntPtr)

                ' And to the destination row
                pDestinationRow = CType(CLng(pDestinationRow) + outputData.Stride, IntPtr)
            Next row
        Finally
            ' Ensure that I unlock the output bits
            output.UnlockBits(outputData)
        End Try
    End Sub


    ''' <summary>
    ''' Override this to process the pixel in the first pass of the algorithm
    ''' </summary>
    ''' <param name="pixel">The pixel to quantize</param>
    ''' <remarks>
    ''' This function need only be overridden if your quantize algorithm needs two passes,
    ''' such as an Octree quantizer.
    ''' </remarks>
    Protected Overridable Sub InitialQuantizePixel(ByVal pixel As Color32)
    End Sub

    ''' <summary>
    ''' Override this to process the pixel in the second pass of the algorithm
    ''' </summary>
    ''' <param name="pixel">The pixel to quantize</param>
    ''' <returns>The quantized value</returns>
    Protected MustOverride Function QuantizePixel(ByVal pixel As Color32) As Byte

    ''' <summary>
    ''' Retrieve the palette for the quantized image
    ''' </summary>
    ''' <param name="original">Any old palette, this is overrwritten</param>
    ''' <returns>The new color palette</returns>
    Protected MustOverride Function GetPalette(ByVal original As ColorPalette) As ColorPalette

    ''' <summary>
    ''' Flag used to indicate whether a single pass or two passes are needed for quantization.
    ''' </summary>
    Private _singlePass As Boolean
    Private _pixelSize As Integer



    ''' <summary>
    ''' Struct that defines a 32 bpp colour
    ''' </summary>
    ''' <remarks>
    ''' This struct is used to read data from a 32 bits per pixel image
    ''' in memory, and is ordered in this manner as this is the way that
    ''' the data is layed out in memory
    ''' </remarks>
    <StructLayout(LayoutKind.Explicit)> _
    Public Structure Color32

        Public Sub New(ByVal pSourcePixel As IntPtr)
            Blue = CType(Marshal.PtrToStructure(pSourcePixel, GetType(Color32)), Color32).Blue
            Green = CType(Marshal.PtrToStructure(pSourcePixel, GetType(Color32)), Color32).Green
            Red = CType(Marshal.PtrToStructure(pSourcePixel, GetType(Color32)), Color32).Red
            Alpha = CType(Marshal.PtrToStructure(pSourcePixel, GetType(Color32)), Color32).Alpha
            ARGB = CType(Marshal.PtrToStructure(pSourcePixel, GetType(Color32)), Color32).ARGB
        End Sub

        ''' <summary>
        ''' Holds the blue component of the colour
        ''' </summary>
        <FieldOffset(0)> _
        Public Blue As Byte
        ''' <summary>
        ''' Holds the green component of the colour
        ''' </summary>
        <FieldOffset(1)> _
        Public Green As Byte
        ''' <summary>
        ''' Holds the red component of the colour
        ''' </summary>
        <FieldOffset(2)> _
        Public Red As Byte
        ''' <summary>
        ''' Holds the alpha component of the colour
        ''' </summary>
        <FieldOffset(3)> _
        Public Alpha As Byte

        ''' <summary>
        ''' Permits the color32 to be treated as an int32
        ''' </summary>
        <FieldOffset(0)> _
        Public ARGB As Integer

        ''' <summary>
        ''' Return the color for this Color32 object
        ''' </summary>
        Public ReadOnly Property Color() As Color
            Get
                Return Color.FromArgb(Alpha, Red, Green, Blue)
            End Get
        End Property
    End Structure
End Class

