Public Class Card
    Inherits Printing.PrintDocument
    Private propMotif As String
    Private propMotifName As String
    Private propMotifWidth As Integer
    Private propMotifHeight As Integer
    Private propKnitTechnique As Boolean
    Private propContinuation As Boolean
    Private propFirstCard As Boolean
    Private propLastcard As Boolean
    Private propLastRow As Boolean
    
    Public Property Motif() As String
        Get
            Return propMotif
        End Get
        Set(ByVal value As String)
            propMotif = value
        End Set
    End Property
    Public Property MotifName() As String
        Get
            Return propMotifName
        End Get
        Set(ByVal value As String)
            propMotifName = value
        End Set
    End Property
    Public Property MotifWidth() As Integer
        Get
            Return propMotifWidth
        End Get
        Set(ByVal value As Integer)
            propMotifWidth = value
        End Set
    End Property
    Public Property MotifHeight() As Integer
        Get
            Return propMotifHeight
        End Get
        Set(ByVal value As Integer)
            propMotifHeight = value
        End Set
    End Property
    Public Property KnitTechnique() As Boolean
        Get
            Return propKnitTechnique
        End Get
        Set(ByVal value As Boolean)
            propKnitTechnique = value
        End Set
    End Property
    Public Property Continuation() As Boolean
        Get
            Return propContinuation
        End Get
        Set(ByVal value As Boolean)
            propContinuation = value
        End Set
    End Property
    Public Property FirstCard() As Boolean
        Get
            Return propFirstCard
        End Get
        Set(ByVal value As Boolean)
            propFirstCard = value
        End Set
    End Property
    Public Property LastCard() As Boolean
        Get
            Return propLastCard
        End Get
        Set(ByVal value As Boolean)
            propLastCard = value
        End Set
    End Property
    Public Property LastRow() As Boolean
        Get
            Return propLastRow
        End Get
        Set(ByVal value As Boolean)
            propLastRow = value
        End Set
    End Property


End Class
