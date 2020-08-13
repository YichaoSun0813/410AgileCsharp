using _410AgileCsharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RenameFileFromRemoteServer;
using System.Net;
using UploadFileToRemoteServer;

namespace RenameFileFromRemoteServerTests.Tests
{
	[TestClass()]
    public class RenameFileFromRemoteServerTests
    {
        [TestMethod()]
        public void RenameFileFromRemoteServer_Rename_SuccesfulResult()
        {
            //Arrange
            var mainRequest = new FtpHandler();
            mainRequest.url = "ftp://13.59.40.218/";
            mainRequest.userName = "ftp_user";
            mainRequest.securePwd = new NetworkCredential("", "Kamono123").SecurePassword;


            var upload = new Upload();
            //upload a file to rename
            upload.UploadToRemote(mainRequest, "SavedConnections.txt");

            var rename = new Rename();
            //Act
            var response = rename.RenameRemoteFile(mainRequest, "SavedConnections.txt", "DidItWork.txt");
            //Assert
            Assert.IsTrue(response);
        }

        [TestMethod()]
        public void RenameFileFromRemoteServer_Rename_FailureResult()
        {
            var mainRequest = new FtpHandler();
            mainRequest.url = "ftp://13.59.40.218/";
            mainRequest.userName = "ftp_user";
            mainRequest.securePwd = new NetworkCredential("", "Kamono12345").SecurePassword;

            var rename = new Rename();
            //Act
            var response = rename.RenameRemoteFile(mainRequest, "SavedConnections.txt", "yummypie.txt");
            //Assert
            Assert.IsFalse(response);
        }
    }
}