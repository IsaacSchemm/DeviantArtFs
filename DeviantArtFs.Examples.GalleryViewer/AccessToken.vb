Imports System.IO

Public Class AccessToken
    Implements IDeviantArtRefreshToken

    Public Property A As String

    Public Property R As String

    Public Property E As DateTimeOffset

    Private ReadOnly Property IDeviantArtAccessToken_AccessToken As String Implements IDeviantArtAccessToken.AccessToken
        Get
            Return A
        End Get
    End Property

    Private ReadOnly Property IDeviantArtRefreshToken_RefreshToken As String Implements IDeviantArtRefreshToken.RefreshToken
        Get
            Return R
        End Get
    End Property

    Private ReadOnly Property IDeviantArtRefreshToken_ExpiresAt As DateTimeOffset Implements IDeviantArtRefreshToken.ExpiresAt
        Get
            Return E
        End Get
    End Property

    Public Sub New()

    End Sub

    Public Sub New(copyFrom As IDeviantArtRefreshToken)
        A = copyFrom.AccessToken
        R = copyFrom.RefreshToken
        E = copyFrom.ExpiresAt
    End Sub

    Public Shared Sub WriteTo(path As String, token As IDeviantArtRefreshToken)
        Using fs As New FileStream(path, FileMode.Create, FileAccess.Write)
            Using sw As New StreamWriter(fs)
                sw.WriteLine(token.AccessToken)
                sw.WriteLine(token.RefreshToken)
                sw.WriteLine(token.ExpiresAt.UtcTicks)
            End Using
        End Using
    End Sub

    Public Shared Function ReadFrom(path As String) As AccessToken
        Using fs As New FileStream(path, FileMode.Open, FileAccess.Read)
            Using sr As New StreamReader(fs)
                Dim a = sr.ReadLine()
                Dim r = sr.ReadLine()
                Dim e = New Date(Long.Parse(sr.ReadLine()), DateTimeKind.Utc)
                Return New AccessToken With {.A = a, .R = r, .E = e}
            End Using
        End Using
    End Function
End Class