# DeviantArtFs

A .NET / F# library to interact with the [DeviantArt / Sta.sh API.](https://www.deviantart.com/developers/http/v1/20200519)

## Design

Each request that can be made to DeviantArt is represented by a function
in one of the modules (static classes) in the `DeviantArtFs.Api` namespace.
Each static method takes one or more parameters:

* `token` (an object that implements the `IDeviantArtAccessToken` interface
  and provides the library with the API credentials)
* `expansion` (on some requests; allows user object expansion)
* A parameter specific to the request (if any)
* A range specifier (for endpoints that ask the user to request a particular
  range of results
    * `paging`: a `DeviantArtPagingParams` record, which specifies an offset
      and an optional limit / page size
        * DeviantArtFs is aware of the maximum limits for each API request; to
          request the maximum page size, use `int.MaxValue` as the limit
    * `cursor`: a string provided in the previous page's result (use `null` to
      start at the beginning)
    * `offset` / `limit`: used in `ToAsyncSeq` and `ToArrayAsync` wrapper
      methods in place of `paging` / `cursor`

In some cases, two methods are available for an API call. Functions whose
names begin with `Page` will return a single page of results, while the
corresponding `Get` function will return an asynchronous enumerable which
begins at the offset you specify (see "Interoperability" below). Be careful
not to request too much data or you might hit API usage limits.

## Interoperability

All return types from API calls are wrapped in either F# asynchronous
workflows (`FSharpAsync<T>`) or F# asynchronous sequences (`IAsyncEnumerable<T>` from the
package [FSharp.Control.AsyncSeq](https://www.nuget.org/packages/FSharp.Control.AsyncSeq)),
and the `FSharpOption<T>` type is used extensively in objct models. To help you work
with these types in C# or VB.NET, DeviantArtFs provides extension methods in
the namespace `DeviantArtFs.Extensions`:

* Option types
    * `.OrNull()`: converts any option type to an equivalent nullable type
    * `.IsTrue()`: checks whether a `bool option` type (which might be `true`, `false`, or `None`) is true
    * `.IsFalse()`: checks whether a `bool option` type (which might be `true`, `false`, or `None`) is false
    * `.OrEmpty()`: returns the items contained by a `list option` type, or an empty list if the field is `None`
* Asynchronous types
    * `.Take(int count)`: sets a limit on the amount of items returned by an F# asynchronous sequence
    * `.ThenToArray()`: converts an F# asynchronous sequence into an asynchronous workflow that collects all items in a single array
    * `.ThenToList()`: converts an F# asynchronous sequence into an asynchronous workflow that collects all items in a single immutable list
    * `.ToAsyncEnumerable()`: converts an F# asynchronous sequence into a .NET `IAsyncEnumerable<T>` (.NET 5.0+ only)
    * `.StartAsTask(TaskCreationOptions options = null, CancellationToken? token = null)`: executes a "cool" F# asynchronous workflow by creating a "hot" .NET task that can be awaited

The `FSharpList<T>` type also appears often, but this type implements
`IEnumerable<T>` and can be converted to a normal list or array using LINQ.

Any C# or VB.NET code that uses this library to call the DeviantArt API is
expected to use either `.ToAsyncEnumerable()` or `.StartAsTask()` on each API
call.

### Deleted deviations and status updates

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

## Known issues

* Mature content filtering is not supported (use the `is_mature` flag on the deviation instead).
* The profile_pic field in the user.profile expansion is not supported due to circular type definitions. Get it from the full profile object instead.
* The following fields in the deviation object are not supported:
  * challenge
  * challenge_entry
  * motion_book
  * premium_folder_data
  * text_content
  * suggested_reasons
* The api_session return object is not supported.

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
