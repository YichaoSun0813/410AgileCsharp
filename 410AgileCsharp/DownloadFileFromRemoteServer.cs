using _410AgileCsharp;
using System;
using System.IO;
using System.Net;

namespace DownloadFileFromRemoteServer
{
	public class Download
	{
		public FtpWebRequest downloadRequest;

		//Resources https://docs.microsoft.com/en-us/dotnet/framework/network-programming/how-to-download-files-with-ftp
		// https://stackoverflow.com/questions/12519290/downloading-files-using-ftpwebrequest
		// example file: @"\\IPAddress or remote machine Name\ShareFolder\FileName.txt"
		// Right now it puts the file in the C:\Temp folder.
		public bool DownloadFromRemote(FtpHandler handler, string file)
		{
			try
			{
				downloadRequest = (FtpWebRequest)WebRequest.Create(handler.url + '/' + file);
				downloadRequest.Method = WebRequestMethods.Ftp.DownloadFile;
				downloadRequest.Credentials = new NetworkCredential(handler.userName, handler.securePwd);

				using (Stream ftpStream = downloadRequest.GetResponse().GetResponseStream())
				using (Stream fileStream = File.Create(@"C:\Temp\" + file))
				{
					byte[] buffer = new byte[10240];
					int read;
					while ((read = ftpStream.Read(buffer, 0, buffer.Length)) > 0)
					{
						fileStream.Write(buffer, 0, read);
						Console.WriteLine("Downloaded {0} bytes", fileStream.Position);
					}
				}

				return true;
			}
			catch (Exception fail)
			{
				Console.WriteLine("Downloading file Failed");
				Console.WriteLine(fail.Message.ToString());
				return false;
			}
		}
	}
}
