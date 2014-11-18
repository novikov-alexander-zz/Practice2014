using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FractalSetLibrary;
using System.Numerics;
using System.Diagnostics;

namespace FractalUnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            IFractalSet fractalSet = new SequintialSetSolver(new MandelbrotFormula(), -2, 2, -2, 2, 101, 101, 150, 10);
            uint[,] arr = fractalSet.getSet(101,101);

            Assert.AreEqual(arr[0, 0], 2);
            Assert.AreEqual(arr[0, 100], 2);
            Assert.AreEqual(arr[100, 0], 1);
            Assert.AreEqual(arr[100, 100], 1);
            Assert.AreEqual(arr[50, 50], -1);
        }

        [TestMethod]
        public void TestMethod2()
        {
            IFractalSet fractalSet = new ParallelSetSolver(new MandelbrotFormula(),-2, 2, -2, 2, 101, 101, 150, 10);
            uint[,] arr = fractalSet.getSet(101,101);

            Assert.AreEqual(arr[0, 0], 2);
            Assert.AreEqual(arr[0, 100], 2);
            Assert.AreEqual(arr[100, 0], 1);
            Assert.AreEqual(arr[100, 100], 1);
            Assert.AreEqual(arr[50, 50], -1);
        }
    }
}
