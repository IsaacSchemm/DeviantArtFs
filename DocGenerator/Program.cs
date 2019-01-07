using System;
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
            if (n == "Int32") n = "int";
            if (n == "Int64") n = "long";
            if (n == "String") n = "string";
            return $"{n}{(t.GenericTypeArguments.Any() ? $"<{string.Join(", ", t.GenericTypeArguments.Select(x => PrintTypeName(x)))}>" : "")}";
        }

        static void Main(string[] args)
        {
            var a = Assembly.GetAssembly(typeof(DeviantArtFs.Requests.Browse.CategoryTree));
            foreach (var t in a.GetTypes().OrderBy(x => x.FullName))
            {
                if (t.Name.Contains("@")) continue;
                var ae = t.GetMembers()
                    .Select(x => x as MethodInfo)
                    .Where(x => x != null)
                    .Where(x => x.ReturnType.Name.StartsWith("Task") || x.ReturnType.Name.StartsWith("FSharpAsync"));
                if (ae.Any())
                {
                    Console.WriteLine(t.FullName);
                    foreach (var x in ae)
                    {
                        Console.Write($"* {x.Name}");
                        foreach (var p in x.GetParameters())
                        {
                            Console.Write($" ({PrintTypeName(p.ParameterType)})");
                        }
                        Console.WriteLine($" -> {PrintTypeName(x.ReturnType)}");
                    }
                    Console.WriteLine();
                }
            }
        }
    }
}
