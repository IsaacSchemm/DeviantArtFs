# DeviantArtFs

An unoffical F# library to interact with the [DeviantArt API.](www.deviantart.com/developers/http/v1/20240701)

## Design

Each request that can be made to DeviantArt is represented by a function
in one of the modules (static classes) in the `DeviantArtFs.Api` namespace.
Each static method takes an `IDeviantArtAccessToken` as its first parameter.
Most methods have additional parameters, many of which are discriminated
unions; hopefully this makes it easy to see exactly what your code is doing
and ensures that parameters can't get mixed up.

In some cases, two methods are available for an API call. Functions whose
names containing `Page` will return a single page of results, while the
corresponding `Get` function will return an asynchronous sequence which
begins at the offset you specify. (Keep API usage limits in mind!)

## Interoperability

The response objects in this library are F# records which use F# `option`
types for optional fields. You can interact with `option` types from C# or
VB.NET using `FSharpOption<T>` and/or `OptionModule`, for example:

    string title = OptionModule.ToObj(d.title);
    DateTimeOffset? time = OptionModule.ToNullable(d.published_time);

`FSharpList<T>`, an immutable linked list that is part of the F# core library,
implements `IReadOnlyList<T>` and `IEnumerable<T>`.

`IAsyncEnumerable<T>` is now the same interface between F# and other .NET
languages, and you can interact with it using `await foreach` or extension
methods like `ToListAsync` in .NET's `System.Linq.AsyncEnumerable` namespace.

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

* The `profile_pic` field in the `user.profile` expansion is not supported due to circular type definitions. Get it from the full profile object instead.
* The `tier.stats` field is not supported due to serialization issues on DeviantArt's end (the empty object `{}` can be rendered as `[]` by the server).
* The `api_session` return object is not supported.

## Authentication

See also: https://www.deviantart.com/developers/authentication

The DeviantArtAuth module provides methods to support the Authorization Code
grant type (getting tokens from an authorization code and refreshing tokens).

If you need to store the access token somewhere (such as in a database or
file), you may want to create your own class that implements the
`IDeviantArtAccessToken` or `IDeviantArtRefreshableAccessToken` interface.
Using the latter will allow DeviantArtFs to automatically refresh the token
and store the new value when it recieves an HTTP 401 response.
