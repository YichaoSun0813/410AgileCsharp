using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using _410AgileCsharp;
using CreateRemoteDirectory;
using System.Net;

namespace CreateRemoteDirectory.Test
{
    [TestClass()]
    public class CreateRemoteDirectoryTest
    {
        [TestMethod()]
        public void CreateRemoteDirectory_MkDirRemote_SuccessfulResult ()
        {
            //Arrange
            var mainRequest = new FtpHandler();
            mainRequest.url = "ftp://13.59.40.218/";
            mainRequest.userName = "ftp_user";
            mainRequest.securePwd = new NetworkCredential("", "Kamono123").SecurePassword;
            var mkDir = new RemoteMkDir();
            //Act
            var result = mkDir.MkDirRemote(mainRequest, "AutomantedTestDir1");
            //Assert
            Assert.IsTrue(result);
        }

        [TestMethod()]
        public void CreateRemoteDirectory_MkDirRemote_FailureResult()
        {
            //Arrange
            var mainRequest = new FtpHandler();
            mainRequest.url = "ftp://13.59.40.218/";
            mainRequest.userName = "ftp_user";
            mainRequest.securePwd = new NetworkCredential("", "Kamono12345").SecurePassword;
            var mkDir = new RemoteMkDir();
            //Act
            var result = mkDir.MkDirRemote(mainRequest, "AutomantedTestDir");
            //Assert
            Assert.IsFalse(result);
        }
    }
}
