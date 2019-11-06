using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;

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

            if (t.IsGenericParameter && t.DeclaringMethod != null) Assert.Fail($"On {typeName}, found type \"{t.Name}\" declared in method {t.DeclaringMethod.Name}");

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

            if (t.IsGenericParameter && t.DeclaringMethod != null) Assert.Fail($"On {typeName}, found type \"{t.Name}\" declared in method {t.DeclaringMethod.Name}");

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
                        if (p.Name == "paging") Assert.AreEqual("IDeviantArtPagingParams", p.ParameterType.Name, "Parameter \"paging\" is not of type IDeviantArt");
                        if (p.Name != "paging") Assert.AreNotEqual("IDeviantArtPagingParams", p.ParameterType.Name, "Parameter of type IDeviantArt is not named \"paging\"");
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
                    Assert.IsTrue(f.ReturnType.FullName?.StartsWith("System.Threading.Tasks.Task") == true, $"Failure in type {t.Name}");
                    AssertOkForCSharp(f.ReturnType, f.Name, t.Name);

                    foreach (var p in f.GetParameters())
                    {
                        if (p.Name == "paging") Assert.AreEqual("IDeviantArtPagingParams", p.ParameterType.Name, $"Parameter {p.Name}, function {f.Name} on {t.Name}");
                        if (p.Name != "paging") Assert.AreNotEqual("IDeviantArtPagingParams", p.ParameterType.Name, $"Parameter {p.Name}, function {f.Name} on {t.Name}");
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

        [TestMethod]
        public void TestTypesInDeviantArtFsNamespace()
        {
            // All visible type names in the DeviantArtFs root namespace should contain DeviantArt, Stash, or Deviation (with IBcl prefix for .NET-friendly interfaces)
            var a = Assembly.GetAssembly(typeof(Deviation));
            foreach (var t in a.GetTypes())
            {
                if (t.DeclaringType != null) continue;
                if (!t.IsVisible) continue;

                if (t.Namespace != "DeviantArtFs") continue;
                Assert.AreEqual(t.IsInterface, t.Name.StartsWith("I"), $"The name of type {t.Name} does not indicate whether it is a class/record or interface");

                if (t.Name.Contains("DeviantArt")) continue;
                if (t.Name.Contains("Stash")) continue;
                if (t.Name.Contains("Deviation")) continue;
                Assert.Fail($"Type {t.Name} in namespace {t.Namespace} does not have an appropriate name");
            }
        }

        [TestMethod]
        public void TestFlags() {
            Assert.IsFalse(DeviantArtObjectExpansion.None.HasFlag(DeviantArtObjectExpansion.UserDetails));
            Assert.IsFalse(DeviantArtObjectExpansion.None.HasFlag(DeviantArtObjectExpansion.UserGeo));
            Assert.IsFalse(DeviantArtObjectExpansion.None.HasFlag(DeviantArtObjectExpansion.UserProfile));
            Assert.IsFalse(DeviantArtObjectExpansion.None.HasFlag(DeviantArtObjectExpansion.UserStats));
            Assert.IsFalse(DeviantArtObjectExpansion.None.HasFlag(DeviantArtObjectExpansion.UserWatch));
            Assert.IsTrue(DeviantArtObjectExpansion.All.HasFlag(DeviantArtObjectExpansion.UserDetails));
            Assert.IsTrue(DeviantArtObjectExpansion.All.HasFlag(DeviantArtObjectExpansion.UserGeo));
            Assert.IsTrue(DeviantArtObjectExpansion.All.HasFlag(DeviantArtObjectExpansion.UserProfile));
            Assert.IsTrue(DeviantArtObjectExpansion.All.HasFlag(DeviantArtObjectExpansion.UserStats));
            Assert.IsTrue(DeviantArtObjectExpansion.All.HasFlag(DeviantArtObjectExpansion.UserWatch));
        }
    }
}
