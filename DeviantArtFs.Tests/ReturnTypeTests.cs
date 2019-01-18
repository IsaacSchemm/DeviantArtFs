using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace DeviantArtFs.Tests
{
    [TestClass]
    public class ReturnTypeTests
    {
        private static readonly ISet<Type> TypesCheckedForFSharp = new HashSet<Type>();
        private static readonly ISet<Type> TypesCheckedForCSharp = new HashSet<Type>();

        private static void AssertOkForFSharp(Type t, string methodName, string typeName)
        {
            if (TypesCheckedForFSharp.Contains(t)) return;
            TypesCheckedForFSharp.Add(t);

            if (t.Namespace.StartsWith("DeviantArtFs") && t.IsInterface) Assert.Fail($"Found interface {t.Name} in result of {methodName} on {typeName}");
            if (t.Name.StartsWith("Nullable")) Assert.Fail($"Found Nullable<T> in result of {methodName} on {typeName}");
            if (t.Name.StartsWith("IBcl")) Assert.Fail($"Found one of the IBcl*** types in result of {methodName} on {typeName}");
            foreach (var a in t.GenericTypeArguments)
            {
                AssertOkForFSharp(a, methodName, typeName);
            }
            foreach (var p in t.GetProperties())
            {
                AssertOkForFSharp(p.PropertyType, methodName, typeName);
            }
        }
         
        private static void AssertOkForCSharp(Type t, string methodName, string typeName)
        {
            if (TypesCheckedForCSharp.Contains(t)) return;
            TypesCheckedForCSharp.Add(t);

            if (t.Namespace.StartsWith("DeviantArtFs") && t.IsInterface && !t.Name.StartsWith("IBcl")) Assert.Fail($"Found interface {t.Name} in result of {methodName} on {typeName} that does not conform to naming convention");
            if (t.Name.StartsWith("FSharpOption")) Assert.Fail($"Found FSharpOption<T> in result of {methodName} on {typeName}");
            foreach (var a in t.GenericTypeArguments)
            {
                AssertOkForCSharp(a, methodName, typeName);
            }
            foreach (var p in t.GetProperties())
            {
                AssertOkForCSharp(p.PropertyType, methodName, typeName);
            }
        }

        [TestMethod]
        public void TestAsyncExecute()
        {
            var a = Assembly.GetAssembly(typeof(Deviation));
            foreach (var t in a.GetTypes())
            {
                if (t.Name.Contains("@")) continue;
                var f = t.GetMethod("AsyncExecute");
                if (f != null)
                {
                    Assert.AreEqual("FSharpAsync`1", f.ReturnType.Name, $"Failure in type {t.Name}");
                    AssertOkForFSharp(f.ReturnType, f.Name, t.Name);

                    foreach (var p in f.GetParameters())
                    {
                        if (p.Name == "paging") Assert.AreEqual("PagingParams", p.ParameterType.Name, "Parameter \"paging\" is not of type PagingParams");
                        if (p.Name != "paging") Assert.AreNotEqual("PagingParams", p.ParameterType.Name, "Parameter of type PagingParams is not named \"paging\"");
                    }
                }
            }
        }

        [TestMethod]
        public void TestToAsyncSeq()
        {
            var a = Assembly.GetAssembly(typeof(Deviation));
            foreach (var t in a.GetTypes())
            {
                if (t.Name.Contains("@")) continue;
                var f = t.GetMethod("ToAsyncSeq");
                if (f != null)
                {
                    AssertOkForFSharp(f.ReturnType, f.Name, t.Name);
                }
            }
        }

        [TestMethod]
        public void TestExecuteAsync()
        {
            var a = Assembly.GetAssembly(typeof(Deviation));
            foreach (var t in a.GetTypes())
            {
                if (t.Name.Contains("@")) continue;
                var f = t.GetMethod("ExecuteAsync");
                if (f != null)
                {
                    Assert.IsTrue(f.ReturnType.FullName.StartsWith("System.Threading.Tasks.Task"), $"Failure in type {t.Name}");
                    AssertOkForCSharp(f.ReturnType, f.Name, t.Name);

                    foreach (var p in f.GetParameters())
                    {
                        if (p.Name == "paging") Assert.AreEqual("PagingParams", p.ParameterType.Name, $"Parameter {p.Name}, function {f.Name} on {t.Name}");
                        if (p.Name != "paging") Assert.AreNotEqual("PagingParams", p.ParameterType.Name, $"Parameter {p.Name}, function {f.Name} on {t.Name}");
                    }
                }
            }
        }

        [TestMethod]
        public void TestToArrayAsync()
        {
            var a = Assembly.GetAssembly(typeof(Deviation));
            foreach (var t in a.GetTypes())
            {
                if (t.Name.Contains("@")) continue;
                var f = t.GetMethod("ToArrayAsync");
                if (f != null)
                {
                    Assert.AreEqual("Task`1", f.ReturnType.Name, $"Failure in type {t.Name}");
                    Assert.AreEqual(1, f.ReturnType.GenericTypeArguments.Length);
                    Assert.IsTrue(f.ReturnType.GenericTypeArguments[0].IsArray, $"ToArrayAsync in type {t.Name} returns {f.ReturnType.GenericTypeArguments[0].FullName}");
                }
            }
        }
    }
}
