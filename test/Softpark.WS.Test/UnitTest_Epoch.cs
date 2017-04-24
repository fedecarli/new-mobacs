using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Softpark.Infrastructure.Extras;

namespace Softpark.WS.Test
{
    [TestClass]
    public class UnitTest_Epoch
    {
        [TestMethod]
        public void TestMethod_FromUnix()
        {
            var expected = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            var actual = (0L).FromUnix();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestMethod_ToUnix()
        {
            var expected = 0L;
            var actual = (new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)).ToUnix();
            Assert.AreEqual(expected, actual);
        }
    }
}
