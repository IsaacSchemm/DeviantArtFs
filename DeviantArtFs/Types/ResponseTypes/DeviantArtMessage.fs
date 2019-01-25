namespace DeviantArtFs

type IBclDeviantArtMessage =
    interface
    end

// https://www.deviantart.com/developers/http/v1/20160316/object/message
type DeviantArtMessage = {
    messageid: string
} with
    interface IBclDeviantArtMessage