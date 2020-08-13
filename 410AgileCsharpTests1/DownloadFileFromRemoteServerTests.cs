using _410AgileCsharp;
using DownloadFileFromRemoteServer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;

namespace DownloadFileFromRemoteServerTests.Tests
{
	[TestClass()]
    public class DownloadFileFromRemoteServerTests
    {
        [TestMethod()]
        public void DownloadFileFromRemoteServer_Download_SuccesfulResult()
        {
			//Arrange
			var mainRequest = new FtpHandler
			{
				url = "ftp://13.59.40.218/",
				userName = "ftp_user",
				securePwd = new NetworkCredential("", "Kamono123").SecurePassword
			};

            var download = new Download();
            //Act
            var response = download.DownloadFromRemote(mainRequest, "Test.txt");
            //Assert
            Assert.IsTrue(response);
        }

        [TestMethod()]
        public void DownloadFileFromRemoteServer_Download_FailureResult()
        {
			var mainRequest = new FtpHandler
			{
				url = "ftp://13.59.40.218/",
				userName = "ftp_user",
				securePwd = new NetworkCredential("", "Kamono12345").SecurePassword
			};

			var download = new Download();
            //Act
            var response = download.DownloadFromRemote(mainRequest, "Test.txt");
            //Assert
            Assert.IsFalse(response);
        }
    }
}