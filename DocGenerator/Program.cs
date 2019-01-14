﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace DocGenerator
{
    class Program
    {
        static string PrintTypeName(Type t)
        {
            string n = t.Name.Replace("`1", "").Replace("`2", "");
            if (n == "FSharpAsync") n = "Async";
            if (n == "IAsyncEnumerable") n = "AsyncSeq";
            if (n == "Boolean") n = "bool";
            if (n == "Byte") n = "byte";
            if (n == "Int16") n = "short";
            if (n == "Int32") n = "int";
            if (n == "Int64") n = "long";
            if (n == "Byte[]") n = "byte[]";
            if (n == "String") n = "string";
            if (n == "Unit") n = "unit";
            if (t.Namespace == "DeviantArtFs.Interop") n = $"Interop.{n}";
            var generics = string.Join(", ", t.GenericTypeArguments.Select(x => PrintTypeName(x)));
            if (n == "Nullable") return $"{generics}?";
            if (n == "FSharpOption") return $"{generics} option";
            return $"{n}{(t.GenericTypeArguments.Any() ? $"<{generics}>" : "")}";
        }

        static void Main(string[] args)
        {
            using (var fs = new FileStream(Path.Combine(Environment.CurrentDirectory, "../../../..", "ENDPOINTS.md"), FileMode.Create, FileAccess.Write))
            using (var sw = new StreamWriter(fs))
            {
                sw.WriteLine(@"This is a list of functions in the DeviantArtFs library that call DeviantArt / Sta.sh API endpoints.

Methods that return an Async<T> or AsyncSeq<T> are intended for use from F#, and their return values use option types to represent missing or null fields.

Methods that return a Task<T> can be used from async methods in C# and VB.NET, and their return values use null or Nullable<T> to represent missing or null fields.

""long"" indicates a 64-bit integer, and a question mark (?) following a type name indicates a Nullable<T>, as in C#.

**IDeviantArtAccessToken**:

An interface that provides an ""AccessToken"" string property. You can get one from DeviantArtFs.DeviantArtAuth or implement the interface yourself.

**ExtParams:**

The value of ""ExtParams"" determines what extra data (if any) is included with deviations and Sta.sh metadata.

    // C#
    ExtParams e1 = new ExtParams { ExtSubmission = true, ExtCamera = false, ExtStats = false };

**FieldChange:**

""FieldChange"" is a discriminated union used in update operations. FieldChange.NoChange means the parameter will not be included; for parameters you want to include, wrap it in FieldChange.UpdateToValue, like so:

    // C#
    new DeviantArtFs.Requests.Stash.UpdateRequest(4567890123456789L) {
        Title = FieldChange<string>.NewUpdateToValue(""new title""),
        Description = FieldChange<string>.NoChange
    }

> Note: Some fields can be null, and some cannot. For example, DeviantArt allows a null description for a Sta.sh stack, but not a null title.

**PagingParams:**

""PagingParams"" is used when the common ""offset"" and ""limit"" parameters are included in a request.

    // C#
    PagingParams x1 = new PagingParams { Offset = 0, Limit = 24 };
");

                var a = Assembly.GetAssembly(typeof(DeviantArtFs.Requests.Browse.CategoryTree));
                foreach (var t in a.GetTypes().OrderBy(x => x.FullName))
                {
                    if (t.Name.Contains("@")) continue;
                    var ae = t.GetMembers()
                        .Select(x => x as MethodInfo)
                        .Where(x => x != null)
                        .Where(x => x.ReturnType.Name.StartsWith("Task") || x.ReturnType.Name.StartsWith("FSharpAsync") || x.ReturnType.Name.StartsWith("IAsyncEnumerable"))
                        .OrderByDescending(x => x.Name.Contains("Execute"))
                        .ThenBy(x => x.Name.EndsWith("ListAsync"));
                    if (ae.Any())
                    {
                        sw.WriteLine($"### {t.FullName}");

                        ISet<Type> typesToDescribe = new HashSet<Type>();
                        foreach (var x in ae)
                        {
                            sw.Write($"* {x.Name}");
                            foreach (var p in x.GetParameters())
                            {
                                sw.Write($" ({PrintTypeName(p.ParameterType)})");
                                if (p.ParameterType.FullName.StartsWith("DeviantArtFs.") && !new[] { "IDeviantArtAccessToken", "ExtParams", "PagingParams" }.Contains(p.ParameterType.Name))
                                {
                                    typesToDescribe.Add(p.ParameterType);
                                }
                            }
                            sw.WriteLine($" -> `{PrintTypeName(x.ReturnType)}`");
                        }
                        sw.WriteLine();

                        foreach (var d in typesToDescribe)
                        {
                            sw.WriteLine($"**{PrintTypeName(d)}:**");
                            sw.WriteLine();
                            foreach (var p in d.GetProperties())
                            {
                                sw.Write($"* {p.Name}: `{PrintTypeName(p.PropertyType)}`");
                                if (p.PropertyType.IsEnum)
                                {
                                    sw.Write($" ({string.Join(", ", p.PropertyType.GetEnumNames())})");
                                }
                                sw.WriteLine();
                            }
                            sw.WriteLine();
                        }
                    }
                }
            }
        }
    }
}
