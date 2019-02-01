using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Haszowanie;
using System.Text;
using System.Collections.Generic;

namespace HaszowanieTesty
{
    [TestClass]
    public class HaszowanieTesty
    {
        const int valsCount = 50;
        const int arrayLen = 20;

        private bool InsertVals<T>(Hash<T> h, T[] vals)
        {
            try
            {
                foreach (var v in vals)
                    h.Insert(v);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        private bool CheckMembers<T>(Hash<T> h, T[] vals)
        {
            bool result = true;

            try
            {
                foreach (var v in vals)
                {
                    result &= h.Member(v);
                }
            }
            catch (Exception)
            {
                return false;
            }
            
            return result;
        }

        private bool CheckFalseMembers<T>(Hash<T> h, T[] vals)
        {
            bool result = false;

            try
            {
                foreach (var v in vals)
                {
                    result |= h.Member(v);
                }
            }
            catch (Exception)
            {
                return false;
            }

            return result == false;
        }

        private bool RemoveVals<T>(Hash<T> h, T[] vals)
        {
            try
            {
                foreach (var v in vals)
                    h.Delete(v);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        [TestMethod]
        public void TestHash()
        {
            Hash<int> hash = new Hash<int>(8);

            // Wartości
            int[] vals = new int[5];
            // specialne
            vals[0] = int.MaxValue - 1;
            vals[1] = int.MinValue;
            vals[2] = 0;
            vals[3] = 12345;
            vals[4] = -256;

            Assert.IsTrue(InsertVals(hash, vals), "Błąd dodawania wartości.");
            Assert.IsTrue(CheckMembers(hash, vals), "Błąd wyszukiwania wartości.");

            int[] otherVals = new int[3];
            // specialne
            otherVals[0] = int.MaxValue;
            otherVals[1] = 42;
            otherVals[2] = -1;

            Assert.IsTrue(CheckFalseMembers(hash, otherVals), "Błąd wyszukiwania fałszywych wartości.");

            Assert.IsTrue(RemoveVals(hash, vals), "Błąd usuwania wartości.");
            Assert.IsTrue(CheckFalseMembers(hash, vals), "Błąd wyszukiwania fałszywych (usuniętych) wartości.");
        }

        [TestMethod]
        public void TestInt()
        {
            Random r = new Random();

            Hash<int> hash = new Hash<int>(32);

            // Wartości
            int[] vals = new int[valsCount];
            // specialne
            vals[0] = int.MaxValue;
            vals[1] = int.MinValue;
            vals[2] = 0;
            // losowe
            for (int i = 3; i < valsCount; i++)
                vals[i] = r.Next();
            
            Assert.IsTrue(InsertVals(hash, vals), "Błąd dodawania wartości.");
            Assert.IsTrue(CheckMembers(hash, vals), "Błąd wyszukiwania wartości.");
            Assert.IsTrue(RemoveVals(hash, vals), "Błąd usuwania wartości.");
        }

        [TestMethod]
        public void TestDouble()
        {
            Random r = new Random();

            Hash<double> hash = new Hash<double>(32);

            // Wartości
            double[] vals = new double[valsCount];
            // specialne
            vals[0] = double.MaxValue;
            vals[1] = double.MinValue;
            vals[2] = double.Epsilon;
            vals[3] = double.NaN;
            vals[4] = double.NegativeInfinity;
            vals[5] = double.PositiveInfinity;
            vals[6] = 0;
            // losowe
            for (int i = 7; i < valsCount; i++)
                vals[i] = r.Next();

            Assert.IsTrue(InsertVals(hash, vals), "Błąd dodawania wartości.");
            Assert.IsTrue(CheckMembers(hash, vals), "Błąd wyszukiwania wartości.");
            Assert.IsTrue(RemoveVals(hash, vals), "Błąd usuwania wartości.");
        }

        [TestMethod]
        public void TestString()
        {
            Random r = new Random();
            
            Hash<string> hash = new Hash<string>(32);

            // Wartości
            string[] vals = new string[valsCount];
            // specialne
            vals[0] = "kot";
            vals[1] = "tok";
            // losowe
            byte[] b = new byte[arrayLen];
            for (int i = 2; i < valsCount; i++)
            {
                r.NextBytes(b);
                vals[i] = Encoding.ASCII.GetString(b);
            }

            Assert.IsTrue(InsertVals(hash, vals), "Błąd dodawania wartości.");
            Assert.IsTrue(CheckMembers(hash, vals), "Błąd wyszukiwania wartości.");
            Assert.IsTrue(RemoveVals(hash, vals), "Błąd usuwania wartości.");
        }

        [TestMethod]
        public void TestDoubleArray()
        {
            Random r = new Random();

            Hash<double[]> hash = new Hash<double[]>(32);

            // Wartości
            double[][] vals = new double[valsCount][];
            for (int i = 0; i < valsCount; i++)
                vals[i] = new double[arrayLen];
            // losowe
            for (int i = 0; i < valsCount; i++)
            {
                for (int j = 0; j < arrayLen; j++)
                    vals[i][j] = r.NextDouble();
            }

            Assert.IsTrue(InsertVals(hash, vals), "Błąd dodawania wartości.");
            Assert.IsTrue(CheckMembers(hash, vals), "Błąd wyszukiwania wartości.");
            Assert.IsTrue(RemoveVals(hash, vals), "Błąd usuwania wartości.");
        }

        [TestMethod]
        public void TestObject()
        {
            Hash<object> hash = new Hash<object>(8);

            // Wartości
            object[] vals = new object[10];
            // specialne
            vals[0] = null;
            vals[1] = true;
            vals[2] = 0;
            vals[3] = 0.0;
            vals[4] = new object();
            vals[5] = new object[] { };
            vals[6] = new object[] { null, null, null};
            vals[7] = new object[] { new object[] { new object[] { } } };
            vals[8] = new List<object>() { new object(), null};
            vals[9] = new SortedList<int, object> { { 1, null}, { int.MinValue, new object()} };

            Assert.IsTrue(InsertVals(hash, vals), "Błąd dodawania wartości.");
            Assert.IsTrue(CheckMembers(hash, vals), "Błąd wyszukiwania wartości.");
            Assert.IsTrue(RemoveVals(hash, vals), "Błąd usuwania wartości.");
        }
    }
}
