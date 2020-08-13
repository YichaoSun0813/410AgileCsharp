using _410AgileCsharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace _410AgileCsharpTests1
{
    [TestClass()]
    class ConnectionTest
    {
        [TestMethod()]
        public void TestLogOff()
        {
            var mainRequest = new FtpHandler();
            Assert.IsFalse(mainRequest.LogOff());
        }
    }
}
