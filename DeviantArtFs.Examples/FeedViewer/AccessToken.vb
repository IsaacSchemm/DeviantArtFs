Public Class AccessToken
    Implements IDeviantArtAccessToken

    Public Sub New(accessToken As String)
        Me.AccessToken = accessToken
    End Sub

    Private ReadOnly Property AccessToken As String Implements IDeviantArtAccessToken.AccessToken
End Class
