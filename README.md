# DeviantArtFs

An F# library (.NET Standard 2.0) to interact with the [DeviantArt / Sta.sh API.](https://www.deviantart.com/developers/http/v1/20160316)
Uses [FSharp.Data](http://fsharp.github.io/FSharp.Data/) to parse JSON.

Much of the library is still untested - use at your own risk.

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

Some functions wrap response data in a type defined in the F# source code, but others use types that come from FSharp.Data's JSON type provider. These functions are marked with a dagger (�) below and may not be usuable from other .NET languages.

If you want more C# / VB.NET compatibility, you'll need to write new F# types (classes, interfaces, or records) to expose the response data; either make additions to this project (feel free to send pull requests) or write a wrapper library.

### Browse

* GET /browse/categorytree
* GET browse/dailydeviations�
* GET browse/hot�
* GET browse/morelikethis�
* GET browse/morelikethis/preview�
* GET browse/newest�
* GET browse/popular�
* GET browse/tags�
* GET browse/tags/search
* GET browse/undiscovered�
* GET browse/user/journals�

### Data

* GET /data/countries
* GET /data/privacy
* GET /data/submission
* GET /data/tos

### Deviation

* GET /deviation/{deviationid}�
* GET /deviation/content�
* GET /deviation/download/{deviationid}
* GET /deviation/embeddedcontent�
* GET /deviation/metadata�
* GET /deviation/whofaved

### Gallery

* GET /gallery/gallery/{folderid}
* GET /gallery/all�
* GET /gallery/folders
* POST /gallery/folders/create
* GET /gallery/folders/remove/{folderid}

### Stash

* GET /stash/{stackid}�
* GET /stash/{stackid}/contents�
* POST /stash/delete
* GET /stash/delta�
* GET /stash/item/{itemid}�
* POST /stash/move/{stackid}
* POST /stash/position/{stackid}
* POST /stash/publish
* GET /stash/publish/categorytree
* GET /stash/publish/userdata
* GET /stash/space
* POST /stash/submit
* POST /stash/update/{stackid}

> The DeviantArt.Stash.Marshal library wraps some of these endpoints and provides some additional functionality.

### User

* GET /user/damntoken
* GET /user/friends/{username}�
* GET /user/friends/search
* GET /user/friends/unwatch/{username}
* POST /user/friends/watch/{username}
* GET /user/friends/watching/{username}
* GET /user/profile/{username}�
* POST /user/profile/update
* GET /user/statuses�
* GET /user/statuses/{statusid}�
* POST /user/statuses/post
* GET /user/watchers/{username}�
* GET /user/whoami
* POST /user/whois

## Util

* GET /placebo

## Authentication

See also: https://www.deviantart.com/developers/authentication

Both Authorization Code (recommended) and Implicit grant types are supported.
If you are writing a Windows desktop application, you can use the forms in the DeviantArtFs.WinForms package to get a code or token from the user.

The DeviantArtAuth class provides methods to support the Authorization Code grant type (getting tokens from an authorization code and refreshing tokens).
