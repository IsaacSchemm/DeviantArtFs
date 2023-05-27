Imports System.IO
Imports DeviantArtFs

Public Class AccessToken
    Implements IDeviantArtRefreshableAccessToken

    Private ReadOnly Property Path As String

    Public Property A As String Implements IDeviantArtAccessToken.AccessToken

    Public Property R As String

    Public Shared ReadOnly Property App As DeviantArtApp
        Get
            Return New DeviantArtApp(DeviantArtClientId, DeviantArtClientSecret)
        End Get
    End Property

    Public Sub New(path As String, Optional token As DeviantArtTokenResponse = Nothing)
        Me.Path = path
        If token IsNot Nothing Then
            A = token.access_token
            R = token.refresh_token
        End If
    End Sub

    Public Sub Write()
        Using fs As New FileStream(Path, FileMode.Create, FileAccess.Write)
            Using sw As New StreamWriter(fs)
                sw.WriteLine(A)
                sw.WriteLine(R)
            End Using
        End Using
    End Sub

    Public Shared Function ReadFrom(path As String) As AccessToken
        Using fs As New FileStream(path, FileMode.Open, FileAccess.Read)
            Using sr As New StreamReader(fs)
                Dim a = sr.ReadLine()
                Dim r = sr.ReadLine()
                Return New AccessToken(path) With {.A = a, .R = r}
            End Using
        End Using
    End Function

    Public Async Function RefreshAccessTokenAsync() As Task Implements IDeviantArtRefreshableAccessToken.RefreshAccessTokenAsync
        Dim newToken = Await DeviantArtAuth.RefreshAsync(App, R)
        A = newToken.access_token
        R = newToken.refresh_token
        Write()
    End Function
End Class