using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Reflection;

namespace DeviantArtFs.Tests
{
    [TestClass]
    public class ReturnTypeTests
    {
        private static void AssertNoNullables(Type t, string methodName, string typeName)
        {
            if (t.Name.Contains("IJsonDocument")) Assert.Fail($"Found JsonProvider type in result of {methodName} on {typeName}");
            if (t.Name.StartsWith("Nullable")) Assert.Fail($"Found Nullable<T> in result of {methodName} on {typeName}");
            foreach (var p in t.GenericTypeArguments)
            {
                AssertNoNullables(p, methodName, typeName);
            }
        }

        private static void AssertNoOptions(Type t, string methodName, string typeName)
        {
            if (t.Name.Contains("IJsonDocument")) Assert.Fail($"Found JsonProvider type in result of {methodName} on {typeName}");
            if (t.Name.StartsWith("FSharpOption")) Assert.Fail($"Found FSharpOption<T> in result of {methodName} on {typeName}");
            foreach (var p in t.GenericTypeArguments)
            {
                AssertNoOptions(p, methodName, typeName);
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
                    AssertNoNullables(f.ReturnType, f.Name, t.Name);
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
                    AssertNoOptions(f.ReturnType, f.Name, t.Name);
                }
            }
        }

        [TestMethod]
        public void TestToListAsync()
        {
            var a = Assembly.GetAssembly(typeof(Deviation));
            foreach (var t in a.GetTypes())
            {
                if (t.Name.Contains("@")) continue;
                var f = t.GetMethod("ToListAsync");
                if (f != null)
                {
                    Assert.AreEqual("Task`1", f.ReturnType.Name, $"Failure in type {t.Name}");
                    Assert.AreEqual(1, f.ReturnType.GenericTypeArguments.Length);
                    Assert.IsTrue(f.ReturnType.GenericTypeArguments[0].FullName.StartsWith("System.Collections.Generic.List"), $"Failure in type {t.Name}: ToListAsync returns {f.ReturnType.GenericTypeArguments[0].FullName} instead of List<T>");
                    AssertNoOptions(f.ReturnType, f.Name, t.Name);
                }
            }
        }
    }
}
