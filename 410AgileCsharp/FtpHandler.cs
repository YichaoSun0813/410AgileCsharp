using System;
using System.IO;
using System.Net;
using System.Security;
using System.Text;

namespace _410AgileCsharp
{
	class FtpHandler
	{
		public FtpWebRequest mainRequest;
		public FtpWebResponse mainResponse;
		public SecureString securePwd;
		public string userName;

		public void LogOn()
		{
			//maybe we could prompt for this? I'm not sure if it matters. Can be tacked on to the end of a NetworkCredential constructor.
			/*
			//Domain
			Console.Write("Enter Domain: ");
			string domain = Console.ReadLine();
			*/

			//Now, we feed all of these into a NetworkCredentials class, and feed that into our FtpWebRequest
			mainRequest.Credentials = new NetworkCredential(userName, securePwd);

			//Other parameters that are important to have
			mainRequest.KeepAlive = true;
			mainRequest.UseBinary = true;
			mainRequest.UsePassive = true;

		}

		public void ListDirectoryDetails()
		{
			//List all files in directory, for testing, and to make sure we're connected
			mainRequest.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
			//Assign to mainResponse
			mainResponse = (FtpWebResponse)mainRequest.GetResponse();
			//Read the response stream
			Stream responseStream = mainResponse.GetResponseStream();
			StreamReader responseRead = new StreamReader(responseStream);
			//Print
			Console.WriteLine(responseRead.ReadToEnd());

			//gracefully exit reader and response
			responseRead.Close();
			mainResponse.Close();
		}

		public bool LogOff()
		{
			//Logs user off of open FTP connection

			try
			{
				mainResponse.Close();
				Console.WriteLine("Connection Closed!");
				return true;
			}
			catch (Exception OhJeeze)
			{
				Console.WriteLine(OhJeeze.Message.ToString());
				return false;
			}
		}

		//Resources https://docs.microsoft.com/en-us/dotnet/framework/network-programming/how-to-download-files-with-ftp
		// example file: @"\\IPAddress or remote machine Name\ShareFolder\FileName.txt"
		public void DownLoad()
		{
			try
			{
				mainRequest.Method = WebRequestMethods.Ftp.DownloadFile;

				mainResponse = (FtpWebResponse)mainRequest.GetResponse();

				Stream responseStream = mainResponse.GetResponseStream();
				StreamReader reader = new StreamReader(responseStream);
				Console.WriteLine(reader.ReadToEnd());

				Console.WriteLine($"Download Complete, status {mainResponse.StatusDescription}");

				reader.Close();
				mainResponse.Close();
			}
			catch (IOException ex)
			{
				Console.WriteLine("Loading file Failed");
				Console.WriteLine(ex.Message);
			}
		}

		//Resources https://docs.microsoft.com/en-us/dotnet/standard/io/how-to-read-text-from-a-file
		// https://docs.microsoft.com/en-us/dotnet/framework/network-programming/how-to-upload-files-with-ftp
		// example file: @"\\IPAddress or remote machine Name\ShareFolder\FileName.txt"
		public void UpLoad(string file)
		{
			try
			{
				mainRequest.Method = WebRequestMethods.Ftp.UploadFile;

				// Copy the contents of the file to the request stream.
				byte[] fileContents;
				using (StreamReader str = new StreamReader(file))
				{
					fileContents = Encoding.UTF8.GetBytes(str.ReadToEnd());
				}

				mainRequest.ContentLength = fileContents.Length;

				using (Stream requestStream = mainRequest.GetRequestStream())
				{
					requestStream.Write(fileContents, 0, fileContents.Length);
				}

				using (FtpWebResponse response = (FtpWebResponse)mainRequest.GetResponse())
				{
					Console.WriteLine($"Upload File Complete, status {response.StatusDescription}");
				}
			}
			catch (IOException ex)
			{
				Console.WriteLine("Loading file Failed");
				Console.WriteLine(ex.Message);
			}

			mainResponse.Close();
		}
	}
}
