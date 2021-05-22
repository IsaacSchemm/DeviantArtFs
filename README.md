# DeviantArtFs

A .NET / F# library to interact with the [DeviantArt / Sta.sh API.](https://www.deviantart.com/developers/http/v1/20200519)

## Design

Each request that can be made to DeviantArt is represented by a function
in one of the modules (static classes) in the `DeviantArtFs.Api` namespace.
Each static method takes an `IDeviantArtAccessToken` as its first parameter.
Most methods have additional parameters, many of which are discriminated
unions in the `DeviantArtFs.ParameterTypes` namespace; hopefully this makes
it easy to see exactly what your code is doing and ensures that parameters
can't get mixed up.

In some cases, two methods are available for an API call. Functions whose
names begin with `Page` will return a single page of results, while the
corresponding `Get` function will return an asynchronous sequence which
begins at the offset you specify (see "Interoperability" below). Be careful
not to request too much data or you might hit API usage limits.

## Interoperability

In order to maximize ease of use from within F#, the response objects in this
library are .NET records using F# `option` types to represent missing fields.
This means that you will need extension methods (see below) to extract a null
value or another placeholder value from these fields.

Since these extension methods are required to use the library outside F#, I've
also decided to reduce the amount of duplicate code in the library by exposing
`Async<T>` directly and relying on C# and VB.NET consumers to use another
extension method to create a `Task<T>`. This has the additional benefit of
(hopefully) allowing the consumer to pass a cancellation token to any method;
let me know if there are any bugs in this regard.

The following types are used in response objects:

* `FSharpAsync<T>`: An F# asynchronous workflow. An extension method (see
  below) allows C# or VB.NET users to create a `Task<T>` that can be awaited.
* `IAsyncEnumerable<T>`: A .NET asynchronous enumerable. F# users can use
  `FSharp.Control.AsyncSeq` and its `ofAsyncEnum` function to create an
  `AsyncSeq<T>`, while C# users can use the extension methods in the NuGet
  package `System.Linq.Async` or consume the enumerable directly with
  `await foreach`.
* `FSharpOption<T>`: Used to represent fields that may be missing or null on
  the response object. Extension methods (see below) allow C# and VB.NET users
  to extract these values by converting `None` to `null` or to an empty list.
* `FSharpList<T>`: An immutable linked list. Implements `IReadOnlyList<T>` and
  `IEnumerable<T>`, so other .NET languages can use `foreach`, LINQ, or access
  the list's properties directly.

The following extension methods are provided in the namespace `DeviantArtFs.Extensions`:

* Option types
    * `.OrNull()`: converts any option type to an equivalent nullable type
    * `.IsTrue()`: checks whether a `bool option` type (which might be `true`, `false`, or `None`) is true
    * `.IsFalse()`: checks whether a `bool option` type (which might be `true`, `false`, or `None`) is false
    * `.OrEmpty()`: returns the items in the list, or an empty list if the field is `None`
* Asynchronous types
    * `.StartAsTask(TaskCreationOptions options = null, CancellationToken? token = null)`: executes a "cool" F# asynchronous workflow by creating a "hot" .NET task that can be awaited

### Deleted deviations and status updates

`Deviation` and `DeviantArtStatus` objects can represent a deviation or status
update that has been deleted; this is why most of the fields on those two
types are marked optional. Check the `is_deleted` field (or `IsDeleted`
property) before attempting to access any of the other fields.

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
* **WebApp**: An ASP.NET Core app written in C# that lets you view
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
