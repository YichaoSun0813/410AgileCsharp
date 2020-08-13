using _410AgileCsharp;
using System;
using System.IO;
using System.Net;
using System.Text;

namespace UploadFileToRemoteServer
{
	public class Upload
	{
		public FtpWebRequest uploadRequest;

		//Resources https://docs.microsoft.com/en-us/dotnet/standard/io/how-to-read-text-from-a-file
		// https://docs.microsoft.com/en-us/dotnet/framework/network-programming/how-to-upload-files-with-ftp
		// example file: @"\\IPAddress or remote machine Name\ShareFolder\FileName.txt"
		// Right now it looks in the \bin\Debug\netcoreapp3.1\ folder for your file to upload
		public bool UploadToRemote(FtpHandler handler, string file)
		{
			try
			{
				uploadRequest = (FtpWebRequest)WebRequest.Create(handler.url + '/' + file);
				uploadRequest.Method = WebRequestMethods.Ftp.UploadFile;
				uploadRequest.Credentials = new NetworkCredential(handler.userName, handler.securePwd);

				byte[] fileContents;
				using (StreamReader str = new StreamReader(file))
				{
					fileContents = Encoding.UTF8.GetBytes(str.ReadToEnd());
				}
				uploadRequest.ContentLength = fileContents.Length;

				using (Stream requestStream = uploadRequest.GetRequestStream())
				{
					requestStream.Write(fileContents, 0, fileContents.Length);
				}

				using (FtpWebResponse uploadResponse = (FtpWebResponse)uploadRequest.GetResponse())
				{
					Console.WriteLine($"Upload File \"{file}\" Complete, status {uploadResponse.StatusDescription}");
				}
				return true;
			}
			catch (Exception fail)
			{
				Console.WriteLine($"Loading {file} Failed");
				Console.WriteLine(fail.Message.ToString());
				return false;
			}
		}

		public bool UploadMultipleToRemote(FtpHandler handler, string file)
		{
			try
			{
				string[] files = file.Split(";");
				foreach (var f in files)
				{
					UploadToRemote(handler, f);
				}
				return true;
			}
			catch (Exception fail)
			{
				Console.WriteLine($"Loading {file} Failed");
				Console.WriteLine(fail.Message.ToString());
				return false;
			}
		}
	}
}
