# DeviantArtFs

A .NET / F# library to interact with the [DeviantArt / Sta.sh API.](https://www.deviantart.com/developers/http/v1/20160316)

If you're using this library in a .NET Framework project and it doesn't run, make sure that the dependencies (FSharp.Core, FSharp.Data, FSharp.Control.AsyncSeq) are installed.

## Currently unsupported features

* The following groups of endpoints are not currently implemented:
  * Comments
  * Feed
  * Messages
  * Notes
* The "expand" parameter (user.details, user.geo, etc) is not currently supported.
* The "ext_preload" parameter (gallery/folders, collections/folders) is not currently supported.
* The "mature_content" parameter is not currently supported.

## Supported endpoints

### Browse

* GET /browse/categorytree
* GET /browse/dailydeviations
* GET /browse/hot
* GET /browse/morelikethis
* GET /browse/morelikethis/preview
* GET /browse/newest
* GET /browse/popular
* GET /browse/tags
* GET /browse/tags/search
* GET /browse/undiscovered
* GET /browse/user/journals

### Collections

* GET /collections/{folderid}
* GET /collections/folders
* POST /collections/fave
* POST /collections/unfave
* POST /collections/folders/create
* GET /collections/folders/remove/{folderid} 	

### Data

* GET /data/countries
* GET /data/privacy
* GET /data/submission
* GET /data/tos

### Deviation

* GET /deviation/{deviationid}
* GET /deviation/content
* GET /deviation/download/{deviationid}
* GET /deviation/embeddedcontent
* GET /deviation/metadata
* GET /deviation/whofaved

### Gallery

* GET /gallery/gallery/{folderid}
* GET /gallery/all
* GET /gallery/folders
* POST /gallery/folders/create
* GET /gallery/folders/remove/{folderid}

### Stash

* GET /stash/{stackid}
* GET /stash/{stackid}/contents
* POST /stash/delete
* GET /stash/delta
* GET /stash/item/{itemid}
* POST /stash/move/{stackid}
* POST /stash/position/{stackid}
* POST /stash/publish
* GET /stash/publish/categorytree
* GET /stash/publish/userdata
* GET /stash/space
* POST /stash/submit
* POST /stash/update/{stackid}

> The DeviantArt.Stash.Marshal library provides a StashRoot object that can process delta entries.

### User

* GET /user/damntoken
* GET /user/friends/{username}
* GET /user/friends/search
* GET /user/friends/unwatch/{username}
* POST /user/friends/watch/{username}
* GET /user/friends/watching/{username}
* GET /user/profile/{username}
* POST /user/profile/update
* GET /user/statuses
* GET /user/statuses/{statusid}
* POST /user/statuses/post
* GET /user/watchers/{username}
* GET /user/whoami
* POST /user/whois

### Util

* GET /placebo

## Usage

Each request you can make has a module (static class) in one of the
DeviantArtFs.Requests namespaces, with AsyncExecute and ExecuteAsync
methods. AsyncExecute returns an F# asynchronous workflow, while
ExecuteAsync returns a Task<T>.

Some modules also have the methods ToAsyncSeq (which takes a request and
offset and wraps the results using FSharp.Control.AsyncSeq) or ToArrayAsync
(which takes a request and optionally an offset and/or limit and compiles an
array.)

The methods sometimes vary in their response objects as well; AsyncExecute
and ToAsyncSeq will typically return a class or interface that uses option
types to represent fields that may or may not exist, while ExecuteAsync and
ToArrayAsync will return an interface that uses nullable reference types (or
Nullable<T>) for such fields. In many cases, the return object is actually the
same, even when the type is different. For example, the Deviation class has
a property named Excerpt of type `string option`, but it also implements the
interface IBclDeviation, which has a Excerpt property of type `string` that
can also be null. ("Bcl" stands for Base Class Library - these interfaces are
designed to be used from other .NET languages outside F#.)

Example (C#):

    var list = new List<IBclDeviation>();
    int offset = 0;
    while (true) {
        var req = new DeviantArtFs.Requests.Gallery.GalleryAllViewRequest();
        var paging = new PagingParams {
            Offset = offset,
            Limit = 24
        };
        IBclDeviantArtPagedResult<IBclDeviation> resp =
            await DeviantArtFs.Requests.Gallery.GalleryAllView.ExecuteAsync(token, paging, req);
        list.AddRange(resp.Results);
        offset = resp.NextOffset ?? 0;
        if (!resp.HasMore) break;
    }

Example (F#):

    let list = new ResizeArray<Deviation>()
    let mutable offset = 0
    let mutable more = true
    while more do
        let req = new DeviantArtFs.Requests.Gallery.GalleryAllViewRequest()
        let paging = new PagingParams(Offset = 0, Limit = Nullable 24)
        let! (resp: DeviantArtPagedResult<Deviation>) = DeviantArtFs.Requests.Gallery.GalleryAllView.AsyncExecute token paging req
        list.AddRange(resp.Results)
        offset <- resp.NextOffset |> Option.defaultValue 0
        more <- resp.HasMore

Note how the result from AsyncExecute is `DeviantArtPagedResult`, which has a NextOffset field of `int option`,
while the result from ExecuteAsync is `IBclDeviantArtPagedResult`, which has a NextOffset field of `int?`.

See ENDPOINTS.md for more information.

## Authentication

See also: https://www.deviantart.com/developers/authentication

Both Authorization Code (recommended) and Implicit grant types are supported.
If you are writing a Windows desktop application, you can use the forms in the DeviantArtFs.WinForms package to get a code or token from the user.

The DeviantArtAuth class provides methods to support the Authorization Code grant type (getting tokens from an authorization code and refreshing tokens).
