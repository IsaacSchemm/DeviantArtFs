using System;
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
            string n = t.Name.Replace("`1", "").Replace("`2", "").Replace("IJsonDocument", "???");
            if (n == "FSharpAsync") n = "Async";
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
            return n == "Nullable" ? $"{generics}?"
                : n == "FSharpOption" ? $"{generics} option"
                : $"{n}{(t.GenericTypeArguments.Any() ? $"<{generics}>" : "")}";
        }

        static void Main(string[] args)
        {
            using (var fs = new FileStream(Path.Combine(Environment.CurrentDirectory, "../../../..", "ENDPOINTS.md"), FileMode.Create, FileAccess.Write))
            using (var sw = new StreamWriter(fs))
            {
                sw.WriteLine(@"This is a list of functions in the DeviantArtFs library that call DeviantArt / Sta.sh API endpoints.

Methods that return an Async<T> or IAsyncEnumerable<T> are intended for use from F#, and methods that return a Task<T> can be used from async methods in C# and VB.NET.

""???"" indicates a type generated from a JSON sample by FSharp.Data's JsonProvider.

""long"" indicates a 64-bit integer, and a question mark (?) following a type name indicates a Nullable<T>, as in C#.

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
    new DeviantArtFs.PagingParams
    {
        Offset = 15,
        Limit = 5
    }
");

                var a = Assembly.GetAssembly(typeof(DeviantArtFs.Requests.Browse.CategoryTree));
                foreach (var t in a.GetTypes().OrderBy(x => x.FullName))
                {
                    if (t.Name.Contains("@")) continue;
                    var ae = t.GetMembers()
                        .Select(x => x as MethodInfo)
                        .Where(x => x != null)
                        .Where(x => x.ReturnType.Name.StartsWith("Task") || x.ReturnType.Name.StartsWith("FSharpAsync") || x.ReturnType.Name.StartsWith("IAsyncEnumerable"))
                        .OrderBy(x => x.Name == "ToListAsync")
                        .ThenBy(x => x.Name == "ToAsyncSeq");
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
                                if (p.ParameterType.FullName.StartsWith("DeviantArtFs.") && p.ParameterType.Name != "IDeviantArtAccessToken" && p.ParameterType.Name != "PagingParams")
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
