# DeviantArtFs

A .NET / F# library to interact with the [DeviantArt / Sta.sh API.](https://www.deviantart.com/developers/http/v1/20200519)

## Design

Each request that can be made to DeviantArt is represented by a function
in one of the modules (static classes) in the `DeviantArtFs.Api` namespace.
Each static method takes an `IDeviantArtAccessToken` as its first parameter.
Most methods have additional parameters, many of which are discriminated
unions; hopefully this makes it easy to see exactly what your code is doing
and ensures that parameters can't get mixed up.

In some cases, two methods are available for an API call. Functions whose
names begin with `Page` will return a single page of results, while the
corresponding `Get` function will return an asynchronous sequence which
begins at the offset you specify (see "Interoperability" below). Be careful
not to request too much data or you might hit API usage limits.

## Interoperability

In order to maximize ease of use from within F#, the response objects in this
library are .NET records using F# `option` types to represent missing fields.
You can interact with `option` types from C# / VB.Net using `FSharpOption<T>`
and `OptionModule`, but it may be easier to use extension methods (see below)
when working with these values.

The following types are used in response objects:

* `FSharpOption<T>`: Used to represent fields that may be missing or null on
  the response object. Extension methods (see below) allow C# and VB.NET users
  to extract these values by converting `None` to `null` or to an empty list.
* `FSharpList<T>`: An immutable linked list. Implements `IReadOnlyList<T>` and
  `IEnumerable<T>`, so other .NET languages can use `foreach`, LINQ, or access
  the list's properties directly.

The following extension methods are provided in the namespace `DeviantArtFs.Extensions`:

* `.OrNull()`: converts an option type to an equivalent nullable type
* `.IsTrue()`: checks whether a `bool option` type (which might be `true`, `false`, or `None`) is true
* `.IsFalse()`: checks whether a `bool option` type (which might be `true`, `false`, or `None`) is false
* `.OrEmpty()`: returns the items in the list, or an empty list if the field is `None`

### Optional parameters

Optional parameters for object expansion, `ext_params`, and mature content
filtering must be included through the token object. Use the interface
`IDeviantArtAccessTokenWithOptionalParameters` and include the optional
parameters in the `OptionalParameters` property; for example:

    member _.OptionalParameters = [
        OptionalParameter.Expansion [Expansion.UserProfile]
        OptionalParameter.ExtParam ExtParam.Collection
        OptionalParameter.MatureContent true
    ]

or:

    public IEnumerable<OptionalParameter> OptionalParameters => new OptionalParameter[] {
        OptionalParameter.NewExpansion(Expansion.UserProfile),
        OptionalParameter.NewExtParam(ExtParam.Gallery),
        OptionalParameter.NewMatureContent(true)
    }

### Deleted deviations and status updates

`Deviation` and `DeviantArtStatus` objects can represent a deviation or status
update that has been deleted; this is why most of the fields on those two
types are marked optional. Check the `is_deleted` field before attempting to
access any of the other fields.

## Known issues

* The profile_pic field in the user.profile expansion is not supported due to circular type definitions. Get it from the full profile object instead.
* The api_session return object is not supported.

## Examples

* **ExampleConsoleApp**: An F# console application that shows some data on the
  current user's recent (and not-so-recent) submissions, along with some of
  their Sta.sh info. Reads the access token interactively from standard input.
* **ExampleConsoleApp3**: An F# console application that submits a picture to
  Sta.sh and then publishes it to DeviantArt.
* **GalleryViewer**: A VB.NET app that lets you see the "All" view of
  someone's gallery and read the descriptions of individual submissions.
  Uses the Client Credentials grant and stores tokens in a file.
* **WebApp**: An ASP.NET Core app written in C# that lets you view
  someone's gallery folders and corresponding submission thumbnails.
  Uses the Client Credentials grant and stores tokens in a database.

## Authentication

See also: https://www.deviantart.com/developers/authentication

The DeviantArtAuth module provides methods to support the Authorization Code
grant type (getting tokens from an authorization code and refreshing tokens).

If you are writing a Windows desktop application, the package
DeviantArtFs.WinForms package uses Internet Explorer to provide a way to get a
code or token from the user.

If you need to store the access token somewhere (such as in a database or
file), you may want to create your own class that implements the
`IDeviantArtAccessToken` or `IDeviantArtRefreshableAccessToken` interface.
Using the latter will allow DeviantArtFs to automatically refresh the token
and store the new value when it recieves an HTTP 401 response.
