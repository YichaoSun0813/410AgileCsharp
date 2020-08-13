using _410AgileCsharp;
using DeleteFileFromRemoteServer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using UploadFileToRemoteServer;

namespace DeleteFileFromRemoteServerTests.Tests
{
	[TestClass()]
    public class DeleteFileFromRemoteServerTests
    {
        [TestMethod()]
        public void DeleteFileFromRemoteServer_Delete_SuccesfulResult()
        {
            //Arrange
            var mainRequest = new FtpHandler();
            mainRequest.url = "ftp://13.59.40.218/";
            mainRequest.userName = "ftp_user";
            mainRequest.securePwd = new NetworkCredential("", "Kamono123").SecurePassword;


            var upload = new Upload();
            //upload a file to delete
            upload.UploadToRemote(mainRequest, "SavedConnections.txt");

            var delete = new Delete();
            //Act
            var response = delete.DeleteRemoteFile(mainRequest, "SavedConnections.txt");
            //Assert
            Assert.IsTrue(response);
        }

        [TestMethod()]
        public void DeleteFileFromRemoteServer_Delete_FailureResult()
        {
            var mainRequest = new FtpHandler();
            mainRequest.url = "ftp://13.59.40.218/";
            mainRequest.userName = "ftp_user";
            mainRequest.securePwd = new NetworkCredential("", "Kamono12345").SecurePassword;

            var delete = new Delete();
            //Act
            var response = delete.DeleteRemoteFile(mainRequest, "SavedConnections.txt");
            //Assert
            Assert.IsFalse(response);
        }
    }
}