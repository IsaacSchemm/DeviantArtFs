Public Class AccessToken
    Implements IDeviantArtAccessToken

    Public Sub New(token As String)
        AccessToken = token
    End Sub

    Public ReadOnly Property AccessToken As String Implements IDeviantArtAccessToken.AccessToken
End Class