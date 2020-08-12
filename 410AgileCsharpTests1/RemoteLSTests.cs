using Microsoft.VisualStudio.TestTools.UnitTesting;
using _410AgileCsharp;
using ListFilesOnRemoteServer;
using System.Net;

namespace ListFilesOnRemoteServer.Tests
{
    [TestClass()]
    public class RemoteLSTests
    {
        [TestMethod()]
        public void ListFilesOnRemoteServer_RemoteLs_SuccesfulResult()
        {
            //Arrange
            var mainRequest = new FtpHandler();
            mainRequest.url = "ftp://13.59.40.218/";
            mainRequest.userName = "ftp_user";
            mainRequest.securePwd = new NetworkCredential("", "Kamono123").SecurePassword;
            var remoteLs = new RemoteLS();
            //Act
            var response = remoteLs.ListRemote(mainRequest);
            //Assert
            Assert.IsTrue(response);
        }

        [TestMethod()]
        public void ListFilesOnRemoteServer_RemoteLs_FailureResult()
        {
            var mainRequest = new FtpHandler();
            mainRequest.url = "ftp://13.59.40.218/";
            mainRequest.userName = "ftp_user";
            mainRequest.securePwd = new NetworkCredential("", "Kamono12345").SecurePassword;
            var remoteLs = new RemoteLS();
            //Act
            var response = remoteLs.ListRemote(mainRequest);
            //Assert
            Assert.IsFalse(response);
        }
    }
}