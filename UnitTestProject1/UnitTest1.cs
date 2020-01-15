using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LvWpfLib;
using LvWpfLib.LvImageView;
using LvWpfLib.LvGeometry;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var result =GeometryCal.SegmentIntersect(new System.Windows.Point(10, 10), new System.Windows.Point(20, 20), new System.Windows.Point(20, 10),
                new System.Windows.Point(10, 20));
            Assert.IsTrue(result);
        }
        [TestMethod]
        public void TestMethod2()
        {
            var p = new System.Windows.Point(10, 10);
            Assert.IsTrue(true);
        }
    }
}
