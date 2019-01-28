# DeviantArtFs

A .NET / F# library to interact with the [DeviantArt / Sta.sh API.](https://www.deviantart.com/developers/http/v1/20160316)

If you're using this library in a .NET Framework project and it doesn't run, make sure that the dependencies (FSharp.Core, FSharp.Json, FSharp.Control.AsyncSeq) are installed via NuGet.

## Notes

Each request that can be made to DeviantArt is represented by a module
somewhere in the DeviantArtFs.Requests namespace. These modules have static
methods that take an IDeviantArtAccessToken (see "Authentication" below) and
usually at least one other parameter.

In most cases, these static methods exist in pairs - one method will use F#
async and use F# features such as records and option types, while the other
will return a Task<T> and use interfaces and null values for interoperability
with C# and VB.NET.

## Pagination

Some of the DeviantArt endpoints support [pagination](https://www.deviantart.com/developers/pagination).
For endpoints that use offset-based pagination, the AsyncExecute and
ExecuteAsync methods take a parameter of the type `IDeviantArtPagingParams`:

    public interface IDeviantArtPagingParams
    {
        int Offset { get; }
        int? Limit { get; }
    }

(The type `DeviantArtPagingParams` implements this interface.)

To request the maximum page size for a particular request, use int.MaxValue as
the Limit property. (The limits for each request are hardcoded into
DeviantArtFs, so it will never request more data than DeviantArt allows.	)

Methods that use cursor-based pagination will take a `string` or
`string option` parameter instead.

Modules for endpoints that support pagination also have ToAsyncSeq and
ToArrayAsync methods, which can be used to fetch an arbitary amount of data as
needed. (Keep in mind that some of the endpoints, like /browse/newest, might
return a theoretically unlimited amount of data!)

## Partial updates

`Stash.Update` and `User.ProfileUpdate` allow you to choose which fields to
update on the object. DeviantArtFs uses a discriminated union
(`DeviantArtFieldChange<T>`) to represent these updates:

    new DeviantArtFs.Requests.Stash.UpdateRequest(4567890123456789L) {
        Title = DeviantArtFieldChange<string>.NewUpdateToValue("new title"),
        Description = DeviantArtFieldChange<string>.NoChange
    }

Note that DeviantArt allows a null value for some fields, but not others.

## Currently unsupported features

* The following groups of endpoints are not currently implemented:
  * Notes
* The following fields in the deviation object are not supported:
  * challenge
  * challenge_entry
  * motion_book
* The profile_pic field in the user.profile expansion is not supported due to circular type definitions. Get it from the full profile object instead.

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

### Comments

* GET /comments/{commentid}/siblings
* GET /comments/deviation/{deviationid}
* POST /comments/post/deviation/{deviationid}
* POST /comments/post/profile/{username}
* POST /comments/post/status/{statusid}
* GET /comments/profile/{username}
* GET /comments/status/{statusid}

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

### Feed

* GET /feed/home
* GET /feed/home/{bucketid}
* GET /feed/notifications
* GET /feed/profile
* GET /feed/settings
* POST /feed/settings/update

### Gallery

* GET /gallery/gallery/{folderid}
* GET /gallery/all
* GET /gallery/folders
* POST /gallery/folders/create
* GET /gallery/folders/remove/{folderid}

### Messages

* POST /messages/delete
* GET /messages/feed
* GET /messages/feedback
* GET /messages/feedback/{stackid}
* GET /messages/mentions
* GET /messages/mentions/{stackid}

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

> The DeviantArtFs.Stash.Marshal library (https://github.com/libertyernie/DeviantArtFs.Stash.Marshal) provides a StashRoot object that can process delta entries.

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

Example (C#):

    int offset = 0;
    while (true) {
        var req = new DeviantArtFs.Requests.Gallery.GalleryAllViewRequest();
        var paging = new PagingParams {
            Offset = offset,
            Limit = 24
        };
        IBclDeviantArtPagedResult<IBclDeviation> resp =
            await DeviantArtFs.Requests.Gallery.GalleryAllView.ExecuteAsync(token, paging, req);
        foreach (var d in resp.Results) {
            Console.WriteLine($"{d.Author.Username}: ${d.Title}");
        }
        offset = resp.NextOffset ?? 0;
        if (!resp.HasMore) break;
    }

Example (F#):

    let mutable offset = 0
    let mutable more = true
    while more do
        let req = new DeviantArtFs.Requests.Gallery.GalleryAllViewRequest()
        let paging = new PagingParams(Offset = 0, Limit = Nullable 24)
        let! (resp: DeviantArtPagedResult<Deviation>) = DeviantArtFs.Requests.Gallery.GalleryAllView.AsyncExecute token paging req
        for d in resp.Results do
            printf "%s: %s" d.author.username d.title
        offset <- resp.next_offset |> Option.defaultValue 0
        more <- resp.has_more

See ENDPOINTS.md for more information.

## Common parameters

Several endpoints support common object expansion (e.g. user.details, user.geo) and/or mature content filtering.
To use these features of the DeviantArt API, wrap the token using DeviantArtCommonParameters.Wrap. For example:

    var commonParameters = new DeviantArtCommonParameters {
        Expand = DeviantArtObjectExpansion.UserDetails | DeviantArtObjectExpansion.UserGeo,
        MatureContent = true
    };
    var new_token = commonParameters.WrapToken(token);
    var me = await Requests.User.Whoami.ExecuteAsync(new_token);

## Examples

The Examples folder in the source code repository contains small applications
that use DeviantArtFs:

* **RecentSubmissions.CSharp**: A C# console application that shows the most
  recent submission, journal, and status for a user, along with any favorites
  or comments. (WinForms is needed for the login window, however.)
  Uses the Implicit grant and stores tokens in a file.
* **RecentSubmissions.FSharp**: As above, but in F#, to demonstrate how
  DeviantArtFs has both F#-style and .NET-style functions and types.
* **GalleryViewer**: A VB.NET app that lets you see the "All" view of
  someone's gallery and read the descriptions of individual submissions.
  Uses the Client Credentials grant and stores tokens in a file.
* **WebApp**: An ASP.NET Core 2.1 app written in C# that lets you view
  someone's gallery folders and corresponding submission thumbnails.
  Uses the Client Credentials grant and stores tokens in a database.

## Authentication

See also: https://www.deviantart.com/developers/authentication

Both Authorization Code (recommended) and Implicit grant types are supported.
If you are writing a Windows desktop application, you can use the forms in the DeviantArtFs.WinForms package to get a code or token from the user using either grant type.

The DeviantArtAuth class provides methods to support the Authorization Code grant type (getting tokens from an authorization code and refreshing tokens).

If you need to store the access token somewhere (such as in a database or file), create your own class that implements the IDeviantArtAccessToken or IDeviantArtRefreshToken interface.
