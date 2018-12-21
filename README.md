# DeviantArtFs

An F# library (.NET Standard 2.0) to interact with the [DeviantArt / Sta.sh API.](https://www.deviantart.com/developers/http/v1/20160316)
Uses [FSharp.Data](http://fsharp.github.io/FSharp.Data/) to parse JSON.

Not all response objects are consumable from within languages other than F#.
If you want more C# / VB.NET compatibility, you'll need to make additions to this project (feel free to send pull requests) or write a wrapper library.

## Currently unsupported features

* Much of this library is not consumable from languages other than F#. If you
  want C# / VB.NET support, you'll probably need to add wrapper types and
  functions returning Task<...> (feel free to send pull requests), or write
  your own wrapper library.
* The "expand" parameter (user.details, user.geo, etc) is not currently supported.
* Much of the library is still untested - use at your own risk.

## Supported endpoints

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

* GET /gallery/all 	Get the "all" view of a users gallery
* GET /gallery/folders 	Fetch gallery folders

### Stash

* POST /stash/delete
* GET /stash/delta
* POST /stash/publish
* GET /stash/publish/categorytree
* POST /stash/submit

### User

* GET /user/profile/{username}
* GET /user/statuses
* GET /user/statuses/{statusid}
* POST /user/statuses/post
* GET /user/whoami

## Authentication

See also: https://www.deviantart.com/developers/authentication

Both Authorization Code (recommended) and Implicit grant types are supported.
If you are writing a Windows desktop application, you can use the forms in the DeviantArtFs.WinForms package to get a code or token from the user.

The DeviantArtAuth class provides methods to support the Authorization Code grant type (getting tokens from an authorization code and refreshing tokens).