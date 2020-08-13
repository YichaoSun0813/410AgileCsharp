using _410AgileCsharp;
using System;
using System.Net;

namespace RenameFileFromRemoteServer
{
	public class Rename
	{
		public FtpWebRequest renameRequest;
		public FtpWebResponse renameResponse;

		public bool RenameRemoteFile(FtpHandler handler, string file, string newName)
		{
			try
			{
				renameRequest = (FtpWebRequest)WebRequest.Create(handler.url + '/' + file);
				renameRequest.Method = WebRequestMethods.Ftp.UploadFile;
				renameRequest.Credentials = new NetworkCredential(handler.userName, handler.securePwd);

				renameRequest.Method = WebRequestMethods.Ftp.Rename;
				renameRequest.RenameTo = newName;
				renameResponse = (FtpWebResponse)renameRequest.GetResponse();
				renameResponse.Close();

				Console.WriteLine($"{file} Renamed to {newName}");
				return true;
			}
			catch (Exception fail)
			{
				Console.WriteLine("Renaming file Failed");
				Console.WriteLine(fail.Message.ToString());
				return false;
			}
		}
	}
}
