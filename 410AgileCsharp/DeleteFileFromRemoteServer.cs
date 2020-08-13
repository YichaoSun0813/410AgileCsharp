using _410AgileCsharp;
using System;
using System.Net;

namespace DeleteFileFromRemoteServer
{
	public class Delete
	{
		public FtpWebRequest deleteRequest;

		public bool DeleteRemoteFile(FtpHandler handler, string file)
		{
			try
			{
				deleteRequest = (FtpWebRequest)WebRequest.Create(handler.url + '/' + file);
				deleteRequest.Method = WebRequestMethods.Ftp.DeleteFile;
				deleteRequest.Credentials = new NetworkCredential(handler.userName, handler.securePwd);

				using (FtpWebResponse response = (FtpWebResponse)deleteRequest.GetResponse())
				{
					Console.WriteLine(response.StatusDescription);
				}
				Console.WriteLine($"Deleting {file} Complete");
				return true;
			}
			catch (Exception fail)
			{
				Console.WriteLine("Deleting file Failed");
				Console.WriteLine(fail.Message.ToString());
				return false;
			}
		}
	}
}
