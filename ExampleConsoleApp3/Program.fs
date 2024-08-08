open System
open DeviantArtFs
open DeviantArtFs.ParameterTypes
open FSharp.Control

task {
    let token = { new IDeviantArtAccessToken with member _.AccessToken = Console.ReadLine().Trim() }

    let o = {
        Api.Stash.SubmissionParameters.Default with
            title = Api.Stash.SubmissionTitle "test 1"
            artist_comments = Api.Stash.ArtistComments "test comments"
            tags = Api.Stash.TagList ["tag1"; "tag2"]
    }

    let f = Api.Stash.FormFile.Create "test.png" "image/png" (System.IO.File.ReadAllBytes @"C:\Users\isaac\Pictures\ipod2.png")

    let! r = Api.Stash.SubmitAsync token (Api.Stash.SubmissionDestination.SubmitToStack (Api.Stash.Stack 8955911093015225L)) o f

    let! rr =
        Api.Stash.PublishAsync token [
            Api.Stash.SubmissionPolicyAgreement true
            Api.Stash.TermsOfServiceAgreement true
            Api.Stash.Featured false
            Api.Stash.Maturity NotMature
            Api.Stash.GalleryId (Guid.Parse "aed8cb5b-cd88-438e-a295-ba3abe618719")
            Api.Stash.GalleryId (Guid.Parse "fb8a1a56-a5f0-4949-8442-e3b0732174ff")
            Api.Stash.AllowComments true
            Api.Stash.RequestCritique false
            Api.Stash.License (CreativeCommons { commercial = CommericalNo; modify = ModifyShare })
            Api.Stash.AllowFreeDownload true
            Api.Stash.AddWatermark false
        ] (Api.Stash.Item r.itemid)

    printfn "%A" rr
}
|> Async.AwaitTask
|> Async.RunSynchronously