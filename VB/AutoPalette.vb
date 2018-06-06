
Imports System.Collections
Imports System.Drawing.Imaging

''' <summary>
''' Quantize using an Octree
''' </summary>
Public Class AutoPalette
    ' 
    '  THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF 
    '  ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO 
    '  THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A 
    '  PARTICULAR PURPOSE. 
    '  
    '    This is sample code and is freely distributable. 
    ' 



    'Public Class OctreeQuantizer
    Inherits Quantizer
    ''' <summary>
    ''' Construct the octree quantizer
    ''' </summary>
    ''' <remarks>
    ''' The Octree quantizer is a two pass algorithm. The initial pass sets up the octree,
    ''' the second pass quantizes a color based on the nodes in the tree
    ''' </remarks>
    ''' <param name="maxColors">The maximum number of colors to return</param>
    ''' <param name="maxColorBits">The number of significant bits</param>
    Public Sub New(ByVal maxColors As Integer, ByVal maxColorBits As Integer)
        MyBase.New(False)
        If maxColors > 255 Then
            Throw New ArgumentOutOfRangeException("maxColors", maxColors, "The number of colors should be less than 256")
        End If

        If (maxColorBits < 1) Or (maxColorBits > 8) Then
            Throw New ArgumentOutOfRangeException("maxColorBits", maxColorBits, "This should be between 1 and 8")
        End If

        ' Construct the octree
        _octree = New Octree(maxColorBits)
        _maxColors = maxColors
    End Sub

    ''' <summary>
    ''' Process the pixel in the first pass of the algorithm
    ''' </summary>
    ''' <param name="pixel">The pixel to quantize</param>
    ''' <remarks>
    ''' This function need only be overridden if your quantize algorithm needs two passes,
    ''' such as an Octree quantizer.
    ''' </remarks>
    Protected Overrides Sub InitialQuantizePixel(ByVal pixel As Color32)
        ' Add the color to the octree
        _octree.AddColor(pixel)
    End Sub

    ''' <summary>
    ''' Override this to process the pixel in the second pass of the algorithm
    ''' </summary>
    ''' <param name="pixel">The pixel to quantize</param>
    ''' <returns>The quantized value</returns>
    Protected Overrides Function QuantizePixel(ByVal pixel As Color32) As Byte
        Dim paletteIndex As Byte = CByte(_maxColors) ' The color at [_maxColors] is set to transparent



        ' Get the palette index if this non-transparent
        If pixel.Alpha > 0 Then
            paletteIndex = CByte(_octree.GetPaletteIndex(pixel))
        End If

        Return paletteIndex
    End Function

    ''' <summary>
    ''' Retrieve the palette for the quantized image
    ''' </summary>
    ''' <param name="original">Any old palette, this is overrwritten</param>
    ''' <returns>The new color palette</returns>
    Protected Overrides Function GetPalette(ByVal original As ColorPalette) As ColorPalette
        ' First off convert the octree to _maxColors colors
        Dim palette As ArrayList = _octree.Palletize(_maxColors - 1)

        ' Then convert the palette based on those colors
        For index As Integer = 0 To palette.Count - 1
            original.Entries(index) = CType(palette(index), Color)
        Next index

        ' Add the transparent color
        original.Entries(_maxColors) = Color.FromArgb(0, 0, 0, 0)

        Return original
    End Function

    ''' <summary>
    ''' Stores the tree
    ''' </summary>
    Private _octree As Octree

    ''' <summary>
    ''' Maximum allowed color depth
    ''' </summary>
    Private _maxColors As Integer

    ''' <summary>
    ''' Class which does the actual quantization
    ''' </summary>
    Private Class Octree
        ''' <summary>
        ''' Construct the octree
        ''' </summary>
        ''' <param name="maxColorBits">The maximum number of significant bits in the image</param>
        Public Sub New(ByVal maxColorBits As Integer)
            _maxColorBits = maxColorBits
            _leafCount = 0
            _reducibleNodes = New OctreeNode(8) {}
            _root = New OctreeNode(0, _maxColorBits, Me)
            _previousColor = 0
            _previousNode = Nothing
        End Sub

        ''' <summary>
        ''' Add a given color value to the octree
        ''' </summary>
        ''' <param name="pixel"></param>
        Public Sub AddColor(ByVal pixel As Color32)
            ' Check if this request is for the same color as the last
            If _previousColor = pixel.ARGB Then
                ' If so, check if I have a previous node setup. This will only ocurr if the first color in the image
                ' happens to be black, with an alpha component of zero.
                If Nothing Is _previousNode Then
                    _previousColor = pixel.ARGB
                    _root.AddColor(pixel, _maxColorBits, 0, Me)
                Else
                    ' Just update the previous node
                    _previousNode.Increment(pixel)
                End If
            Else
                _previousColor = pixel.ARGB
                _root.AddColor(pixel, _maxColorBits, 0, Me)
            End If
        End Sub

        ''' <summary>
        ''' Reduce the depth of the tree
        ''' </summary>
        Public Sub Reduce()
            Dim index As Integer

            ' Find the deepest level containing at least one reducible node
            index = _maxColorBits - 1
            Do While (index > 0) AndAlso (Nothing Is _reducibleNodes(index))

                index -= 1
            Loop

            ' Reduce the node most recently added to the list at level 'index'
            Dim node As OctreeNode = _reducibleNodes(index)
            _reducibleNodes(index) = node.NextReducible

            ' Decrement the leaf count after reducing the node
            _leafCount -= node.Reduce()


            ' And just in case I've reduced the last color to be added, and the next color to
            ' be added is the same, invalidate the previousNode...
            _previousNode = Nothing
        End Sub

        ''' <summary>
        ''' Get/Set the number of leaves in the tree
        ''' </summary>
        Public Property Leaves() As Integer
            Get
                Return _leafCount
            End Get
            Set(ByVal value As Integer)
                _leafCount = value
            End Set
        End Property

        ''' <summary>
        ''' Return the array of reducible nodes
        ''' </summary>
        Protected ReadOnly Property ReducibleNodes() As OctreeNode()
            Get
                Return _reducibleNodes
            End Get
        End Property

        ''' <summary>
        ''' Keep track of the previous node that was quantized
        ''' </summary>
        ''' <param name="node">The node last quantized</param>
        Protected Sub TrackPrevious(ByVal node As OctreeNode)
            _previousNode = node
        End Sub

        ''' <summary>
        ''' Convert the nodes in the octree to a palette with a maximum of colorCount colors
        ''' </summary>
        ''' <param name="colorCount">The maximum number of colors</param>
        ''' <returns>An arraylist with the palettized colors</returns>
        Public Function Palletize(ByVal colorCount As Integer) As ArrayList
            Do While Leaves > colorCount
                Reduce()
            Loop

            ' Now palettize the nodes
            Dim palette As New ArrayList(Leaves)
            Dim paletteIndex As Integer = 0
            _root.ConstructPalette(palette, paletteIndex)

            ' And return the palette
            Return palette
        End Function

        ''' <summary>
        ''' Get the palette index for the passed color
        ''' </summary>
        ''' <param name="pixel"></param>
        ''' <returns></returns>
        Public Function GetPaletteIndex(ByVal pixel As Color32) As Integer
            Return _root.GetPaletteIndex(pixel, 0)
        End Function

        ''' <summary>
        ''' Mask used when getting the appropriate pixels for a given node
        ''' </summary>
        Private Shared mask() As Integer = {&H80, &H40, &H20, &H10, &H8, &H4, &H2, &H1}

        ''' <summary>
        ''' The root of the octree
        ''' </summary>
        Private _root As OctreeNode

        ''' <summary>
        ''' Number of leaves in the tree
        ''' </summary>
        Private _leafCount As Integer

        ''' <summary>
        ''' Array of reducible nodes
        ''' </summary>
        Private _reducibleNodes() As OctreeNode

        ''' <summary>
        ''' Maximum number of significant bits in the image
        ''' </summary>
        Private _maxColorBits As Integer

        ''' <summary>
        ''' Store the last node quantized
        ''' </summary>
        Private _previousNode As OctreeNode

        ''' <summary>
        ''' Cache the previous color quantized
        ''' </summary>
        Private _previousColor As Integer

        ''' <summary>
        ''' Class which encapsulates each node in the tree
        ''' </summary>
        Protected Class OctreeNode
            ''' <summary>
            ''' Construct the node
            ''' </summary>
            ''' <param name="level">The level in the tree = 0 - 7</param>
            ''' <param name="colorBits">The number of significant color bits in the image</param>
            ''' <param name="octree_Renamed">The tree to which this node belongs</param>
            Public Sub New(ByVal level As Integer, ByVal colorBits As Integer, ByVal octree_Renamed As Octree)
                ' Construct the new node
                _leaf = (level = colorBits)

                _blue = 0
                _green = _blue
                _red = _green
                _pixelCount = 0

                ' If a leaf, increment the leaf count
                If _leaf Then
                    octree_Renamed.Leaves += 1
                    _nextReducible = Nothing
                    _children = Nothing
                Else
                    ' Otherwise add this to the reducible nodes
                    _nextReducible = octree_Renamed.ReducibleNodes(level)
                    octree_Renamed.ReducibleNodes(level) = Me
                    _children = New OctreeNode(7) {}
                End If
            End Sub

            ''' <summary>
            ''' Add a color into the tree
            ''' </summary>
            ''' <param name="pixel">The color</param>
            ''' <param name="colorBits">The number of significant color bits</param>
            ''' <param name="level">The level in the tree</param>
            ''' <param name="octree_Renamed">The tree to which this node belongs</param>
            Public Sub AddColor(ByVal pixel As Color32, ByVal colorBits As Integer, ByVal level As Integer, ByVal octree_Renamed As Octree)
                ' Update the color information if this is a leaf
                If _leaf Then
                    Increment(pixel)
                    ' Setup the previous node
                    octree_Renamed.TrackPrevious(Me)
                Else
                    ' Go to the next level down in the tree
                    Dim shift As Integer = 7 - level
                    Dim index As Integer = ((pixel.Red And mask(level)) >> (shift - 2)) Or ((pixel.Green And mask(level)) >> (shift - 1)) Or ((pixel.Blue And mask(level)) >> (shift))

                    Dim child As OctreeNode = _children(index)

                    If Nothing Is child Then
                        ' Create a new child node & store in the array
                        child = New OctreeNode(level + 1, colorBits, octree_Renamed)
                        _children(index) = child
                    End If

                    ' Add the color to the child node
                    child.AddColor(pixel, colorBits, level + 1, octree_Renamed)
                End If

            End Sub

            ''' <summary>
            ''' Get/Set the next reducible node
            ''' </summary>
            Public Property NextReducible() As OctreeNode
                Get
                    Return _nextReducible
                End Get
                Set(ByVal value As OctreeNode)
                    _nextReducible = value
                End Set
            End Property

            ''' <summary>
            ''' Return the child nodes
            ''' </summary>
            Public ReadOnly Property Children() As OctreeNode()
                Get
                    Return _children
                End Get
            End Property

            ''' <summary>
            ''' Reduce this node by removing all of its children
            ''' </summary>
            ''' <returns>The number of leaves removed</returns>
            Public Function Reduce() As Integer
                _blue = 0
                _green = _blue
                _red = _green
                Dim children As Integer = 0

                ' Loop through all children and add their information to this node
                For index As Integer = 0 To 7
                    If Nothing IsNot _children(index) Then
                        _red += _children(index)._red
                        _green += _children(index)._green
                        _blue += _children(index)._blue
                        _pixelCount += _children(index)._pixelCount
                        children += 1
                        _children(index) = Nothing
                    End If
                Next index

                ' Now change this to a leaf node
                _leaf = True

                ' Return the number of nodes to decrement the leaf count by
                Return (children - 1)
            End Function

            ''' <summary>
            ''' Traverse the tree, building up the color palette
            ''' </summary>
            ''' <param name="palette">The palette</param>
            ''' <param name="paletteIndex">The current palette index</param>
            Public Sub ConstructPalette(ByVal palette As ArrayList, ByRef paletteIndex As Integer)
                If _leaf Then
                    ' Consume the next palette index
                    _paletteIndex = paletteIndex
                    paletteIndex += 1

                    ' And set the color of the palette entry
                    palette.Add(Color.FromArgb(_red \ _pixelCount, _green \ _pixelCount, _blue \ _pixelCount))
                Else
                    ' Loop through children looking for leaves
                    For index As Integer = 0 To 7
                        If Nothing IsNot _children(index) Then
                            _children(index).ConstructPalette(palette, paletteIndex)
                        End If
                    Next index
                End If
            End Sub

            ''' <summary>
            ''' Return the palette index for the passed color
            ''' </summary>
            Public Function GetPaletteIndex(ByVal pixel As Color32, ByVal level As Integer) As Integer
                Dim paletteIndex As Integer = _paletteIndex

                If Not _leaf Then
                    Dim shift As Integer = 7 - level
                    Dim index As Integer = ((pixel.Red And mask(level)) >> (shift - 2)) Or ((pixel.Green And mask(level)) >> (shift - 1)) Or ((pixel.Blue And mask(level)) >> (shift))

                    If Nothing IsNot _children(index) Then
                        paletteIndex = _children(index).GetPaletteIndex(pixel, level + 1)
                    Else
                        Throw New Exception("Didn't expect this!")
                    End If
                End If

                Return paletteIndex
            End Function

            ''' <summary>
            ''' Increment the pixel count and add to the color information
            ''' </summary>
            Public Sub Increment(ByVal pixel As Color32)
                _pixelCount += 1
                _red += pixel.Red
                _green += pixel.Green
                _blue += pixel.Blue
            End Sub

            ''' <summary>
            ''' Flag indicating that this is a leaf node
            ''' </summary>
            Private _leaf As Boolean

            ''' <summary>
            ''' Number of pixels in this node
            ''' </summary>
            Private _pixelCount As Integer

            ''' <summary>
            ''' Red component
            ''' </summary>
            Private _red As Integer

            ''' <summary>
            ''' Green Component
            ''' </summary>
            Private _green As Integer

            ''' <summary>
            ''' Blue component
            ''' </summary>
            Private _blue As Integer

            ''' <summary>
            ''' Pointers to any child nodes
            ''' </summary>
            Private _children() As OctreeNode

            ''' <summary>
            ''' Pointer to next reducible node
            ''' </summary>
            Private _nextReducible As OctreeNode

            ''' <summary>
            ''' The index of this node in the palette
            ''' </summary>
            Private _paletteIndex As Integer

        End Class
    End Class
End Class



