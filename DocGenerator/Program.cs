using Microsoft.FSharp.Reflection;
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
            string n = t.Name.Replace("`1", "").Replace("`2", "");
            if (t.Namespace == "FSharp.Control" && n == "IAsyncEnumerable") n = "AsyncSeq";
            if (n == "FSharpAsync") n = "AsyncSeq";
            if (t.Namespace == "DeviantArtFs.Interop") n = $"Interop.{n}";
            var generics = string.Join(", ", t.GenericTypeArguments.Select(x => PrintTypeName(x)));
            if (n == "Nullable") return $"{generics}?";
            return $"{n}{(t.GenericTypeArguments.Any() ? $"<{generics}>" : "")}";
        }

        static void Main()
        {
            using (var fs = new FileStream(Path.Combine(Environment.CurrentDirectory, "../../../..", "ENDPOINTS.md"), FileMode.Create, FileAccess.Write))
            using (var sw = new StreamWriter(fs))
            {
                sw.WriteLine(@"This is a list of functions in the DeviantArtFs library that call DeviantArt / Sta.sh API endpoints.

Methods that return an Async<T> or AsyncSeq<T> are intended for F# consumers. Methods that return a Task<T> can be used from async methods in C# and VB.NET.

A question mark (?) following a type name indicates a Nullable<T>, as in C#.
");

                var a = Assembly.GetAssembly(typeof(DeviantArtFs.Api.Browse.DailyDeviations));
                foreach (var t in a.GetTypes().OrderBy(x => x.FullName))
                {
                    if (t.Name.Contains("@")) continue;
                    if (t.IsInterface) continue;
                    var ae = t.GetMembers()
                        .Select(x => x as MethodInfo)
                        .Where(x => x != null)
                        .Where(x => x.ReturnType.Name.StartsWith("Task") || x.ReturnType.Name.StartsWith("FSharpAsync") || x.ReturnType.Name.StartsWith("IAsyncEnumerable"))
                        .OrderBy(x => x.Name.Contains("Execute") ? 0 : 1)
                        .ThenBy(x => x.Name.Contains("GetMax") ? 0 : 1)
                        .ThenBy(x => x.ReturnType.FullName);
                    if (ae.Any())
                    {
                        sw.WriteLine($"### {t.FullName}");
                        ISet<Type> typesToDescribe = new HashSet<Type>();
                        foreach (var x in ae)
                        {
                            sw.Write($"* {x.Name}");
                            foreach (var p in x.GetParameters())
                            {
                                sw.Write($" `{PrintTypeName(p.ParameterType)} {p.Name}`");
                                if (p.ParameterType.FullName.StartsWith("DeviantArtFs.") && !new[] { "IDeviantArtAccessToken", "IDeviantArtAccessToken", "DeviantArtCommonParams", "DeviantArtExtParams", "DeviantArtPagingParams" }.Contains(p.ParameterType.Name))
                                {
                                    typesToDescribe.Add(p.ParameterType);
                                }
                                foreach (var g in p.ParameterType.GenericTypeArguments)
                                {
                                    typesToDescribe.Add(g);
                                }
                            }
                            sw.WriteLine($" -> `{PrintTypeName(x.ReturnType)}`");
                        }
                        sw.WriteLine();

                        foreach (var d in typesToDescribe)
                        {
                            if (FSharpType.IsUnion(d, Microsoft.FSharp.Core.FSharpOption<BindingFlags>.None))
                            {
                                sw.WriteLine($"**{PrintTypeName(d)}** ({string.Join(" | ", FSharpType.GetUnionCases(d, Microsoft.FSharp.Core.FSharpOption<BindingFlags>.None).Select(x => x.Name))})");
                                sw.WriteLine();
                            }
                            else
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
                                    else if (FSharpType.IsUnion(p.PropertyType, Microsoft.FSharp.Core.FSharpOption<BindingFlags>.None))
                                    {
                                        sw.Write($" ({string.Join(" | ", FSharpType.GetUnionCases(p.PropertyType, Microsoft.FSharp.Core.FSharpOption<BindingFlags>.None).Select(x => x.Name))})");
                                    }
                                    sw.WriteLine();
                                }
                            }
                            sw.WriteLine();
                        }
                    }
                }
            }
        }
    }
}
