using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using System;
using System.Collections.Generic;
using System.Text;
using _410AgileCsharp;
using System.IO;

namespace _410AgileCsharpTests1
{
    [TestClass()]
    public class SaveConnectionInfoTests
    {
        [TestMethod()]
        public void TestSavingInfo()
        {
            var mainRequest = new FtpHandler();
            mainRequest.url = "ftp://13.59.40.218/";
            mainRequest.userName = "ftp_user";
            mainRequest.securePwd = new NetworkCredential("", "Kamono123").SecurePassword;
            mainRequest.SaveInfo();
            Assert.IsTrue(File.ReadAllText("SavedConnections.txt").Contains("ftp_user"));
        }
    }
}
