# DeviantArtFs

A .NET / F# library to interact with the [DeviantArt / Sta.sh API.](https://www.deviantart.com/developers/http/v1/20160316)
Uses [FSharp.Data](http://fsharp.github.io/FSharp.Data/) to parse JSON.

## Currently unsupported features

* The following groups of endpoints are not currently implemented:
  * Collections
  * Comments
  * Feed
  * Messages
  * Notes
* The "expand" parameter (user.details, user.geo, etc) is not currently supported.
* The "ext_preload" parameter (gallery/folders) is not currently supported.
* The "mature_content" parameter is not currently supported.

## Supported endpoints

### Browse

* GET /browse/categorytree
* GET browse/dailydeviations
* GET browse/hot
* GET browse/morelikethis
* GET browse/morelikethis/preview
* GET browse/newest
* GET browse/popular
* GET browse/tags
* GET browse/tags/search
* GET browse/undiscovered
* GET browse/user/journals

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

* GET /stash/{stackid}†
* GET /stash/{stackid}/contents†
* POST /stash/delete
* GET /stash/delta†
* GET /stash/item/{itemid}†
* POST /stash/move/{stackid}
* POST /stash/position/{stackid}
* POST /stash/publish
* GET /stash/publish/categorytree
* GET /stash/publish/userdata
* GET /stash/space
* POST /stash/submit
* POST /stash/update/{stackid}

> † The DeviantArt.Stash.Marshal library provides the .NET-friendly StashItem and StackStack wrappers
>   and a StashRoot object that can process delta entries.

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

## Util

* GET /placebo

## Result objects

For requests that return relatively simple data, the resuult object will either be a standard .NET object (like IEnumerable<T>)
or an F# record type. Some F# records defined in this library use option types (FSharpOption<T>); to make interop easier,
these records also have functions that return the same result as a potentially null value.

Example (C#):

	var result = await DeviantArtFs.Requests.Gallery.All.ExecuteAsync(token, new DeviantArtFs.Requests.Gallery.AllRequest());
	Microsoft.FSharp.Core.FSharpOption<int> a = result.NextOffset;
	int? b = result.GetNextOffset();

Example (F#):

	let! result = new DeviantArtFs.Requests.Gallery.AllRequest() |> DeviantArtFs.Requests.Gallery.All.AsyncExecute token
	let a: int option = result.NextOffset
	let b: System.Nullable<int> = result.GetNextOffset()

More complex types (Deviation, Metadata, Status, Profile) have classes defined for them that provide a .NET-friendly wrapper
around the original JsonProvider<...> object, including the use of null and Nullable<T>. The original JsonProvider<...>
object is available via the "Original" property on these objects.

## Authentication

See also: https://www.deviantart.com/developers/authentication

Both Authorization Code (recommended) and Implicit grant types are supported.
If you are writing a Windows desktop application, you can use the forms in the DeviantArtFs.WinForms package to get a code or token from the user.

The DeviantArtAuth class provides methods to support the Authorization Code grant type (getting tokens from an authorization code and refreshing tokens).
