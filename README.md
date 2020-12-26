# DeviantArtFs

A .NET / F# library to interact with the [DeviantArt / Sta.sh API.](https://www.deviantart.com/developers/http/v1/20160316)

If you're using this library in a .NET Framework project and it doesn't run, make sure that the dependencies (e.g. FSharp.Core, FSharp.Json, FSharp.Control.AsyncSeq) are installed via NuGet.

## Notes

Each request that can be made to DeviantArt is represented by a module
somewhere in the DeviantArtFs.Api namespace. These modules have static
methods that take one or more parameters:

* `token` (an object that implements the `IDeviantArtAccessToken` interface
  and provides the library with the API credentials)
* `common` (allows user object expansion and mature content filtering;
  `DeviantArtCommonParams.Default` will hide mature content and avoid
  all user object expansion)
* A parameter specific to the request (if any)
* `paging` / `cursor` / `offset` (for endpoints that ask the user to request a
  particular range of results; `paging` will be a `DeviantArtPagingParams`
  record, which contains an offset and an optional limit)
    * To request the maximum page size that DeviantArt allows for a particular request, use `int.MaxValue` as the limit

The main method is usually named `AsyncExecute` and returns an async workflow,
the result of which is an F# record type that lines up with the original JSON.
An `ExecuteAsync` method is also available that returns a .NET `Task` instead.

For endpoints that allow paging, `ToAsyncSeq` and `ToArrayAsync` methods will
be available as well; when using these, DeviantArtFs will perform multiple API
calls, asking for the maximum amount of results in each. Be careful not to
request too much data or you might hit API usage limits.

### JSON conversion notes

Many objects in the DeviantArt API have optional fields, which are difficult
to represent in languages such as F# that expect a fixed schema. DeviantArtFs
represents these optional fields with F# `option` types.

For requests that return an object with a single field that is either a string
or a list, DeviantArtFs will flatten the response to just the string or list
itself.

### Using the library from C# or VB.NET

The library provides
extension methods in the namespace `DeviantArtFs.Extensions` for dealing with
option types from outside F#:

    public string GetTitleCarefully(Deviation d) {
        return d.title.ToObj() ?? "Could not find title!";
	}

    public string GetTitleRecklessly(Deviation d) {
        return d.title.Value; // throws an exception if field is None
	}

    public IEnumerable<DeviationPreview> GetThumbnails(Deviation d) {
        return d.thumbs.OrEmpty();
	}

    public bool CheckIfFavorited(Deviation d) {
        return d.is_favourited.IsTrue();
	}

### Deleted Deviations and Status Updates

`Deviation` and `DeviantArtStatus` objects can represent a deviation or status
update that has been deleted; this is why most of the fields on those two
types are marked optional. Check the `is_deleted` field (or `IsDeleted`
property) before attempting to access any of the other fields.

## Partial updates

`Stash.Update` and `User.ProfileUpdate` allow you to choose which fields to
update on the object. DeviantArtFs uses discriminated unions to represent
these updates:

    await Requests.User.ProfileUpdate.ExecuteAsync(token, new[] {
        ProfileUpdateField.NewArtistLevel(ArtistLevel.Student),
        ProfileUpdateField.NewWebsite("https://www.example.com")
    });

    await Requests.Stash.Update.ExecuteAsync(token, 12345678L, new[] {
        UpdateField.NewTitle("new stack title"),
        UpdateField.ClearDescription
    });

Note that DeviantArt allows a null value for the "description" field on a
Sta.sh stack, and this is represented by its own union case.

## Currently unsupported features

* The following fields in the deviation object are not supported:
  * challenge
  * challenge_entry
  * motion_book
* The profile_pic field in the user.profile expansion is not supported due to circular type definitions. Get it from the full profile object instead.
* Some of the newer fields on the deviation object (like premium_folder_data or text_content) are not currently supported.
* The api_session return object is ignored.

## Examples

* **ExampleConsoleApp**: An F# console application that shows some data on the
  current user's recent (and not-so-recent) submissions, along with some of
  their Sta.sh info. Reads the access token interactively from standard input.
* **GalleryViewer**: A VB.NET app that lets you see the "All" view of
  someone's gallery and read the descriptions of individual submissions.
  Uses the Client Credentials grant and stores tokens in a file.
* **WebApp**: An ASP.NET Core 2.1 app written in C# that lets you view
  someone's gallery folders and corresponding submission thumbnails.
  Uses the Client Credentials grant and stores tokens in a database.

## Authentication

See also: https://www.deviantart.com/developers/authentication

Both Authorization Code (recommended) and Implicit grant types are supported.
The DeviantArtAuth module provides methods to support the Authorization Code
grant type (getting tokens from an authorization code and refreshing tokens).

If you are writing a Windows desktop application, the package
DeviantArtFs.WinForms package uses Internet Explorer to provide a way to get a
code or token from the user using either grant type.

If you need to store the access token somewhere (such as in a database or
file), you may want to create your own class that implements the
`IDeviantArtAccessToken`, `IDeviantArtRefreshToken`, or
`IDeviantArtAutomaticRefreshToken` interface. Using the latter will allow
DeviantArtFs to automatically refresh the token and store the new value when
it recieves an HTTP 401 response. (An InvalidRefreshTokenException is thrown
if the token cannot be refreshed.)
