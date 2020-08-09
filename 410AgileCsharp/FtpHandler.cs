using System;
using System.IO;
using System.Net;
using System.Security;
using System.Text;
using ListFilesOnRemoteServer;

namespace _410AgileCsharp
{
	class FtpHandler
	{
		public FtpWebRequest mainRequest;
		public FtpWebResponse mainResponse;
		public SecureString securePwd;
		public string userName;
		public string url;

		public void LogOn()
		{
			bool loggedOn = false;
			RemoteLS listRemote = new RemoteLS();
			while (!loggedOn)
			{
				//Prompt for FTP URL, and add into FtpWebRequest.
				//Keep in mind that this needs to be a full URL, which should look something like this: ftp://HostName.com/
				//This also initializes FtpWebRequest, which is kinda major. Maybe we should find a way to move this? 
				//Maybe mainHandler constructor?
				Console.Write("Enter an FTP server URL: ");
				url = Console.ReadLine();

				//Allows a user to log on to an FTP server with username and password. 
				//Prompts user for username, domain, and password
				//Username
				Console.Write("Enter username: ");
				userName = Console.ReadLine();
				Console.WriteLine();

				//Password stuff
				//We need to use SecureStrings to be able to feed FtpWebRequest a password. It's probably much better that way!
				securePwd = new SecureString();
				ConsoleKeyInfo key;
				Console.Write("Enter password: ");
				do
				{
					key = Console.ReadKey(true);

					// Ignore any key out of range.
					if (((int)key.Key) >= 31 && ((int)key.Key <= 122) || (int)key.Key == 189)
					{
						// Append the character to the password.
						securePwd.AppendChar(key.KeyChar);
						Console.Write("*");
					}
					// Exit if Enter key is pressed.
				} while (key.Key != ConsoleKey.Enter);
				Console.WriteLine();

				mainRequest = (FtpWebRequest)WebRequest.Create(url);
				//HUGE: with EnableSsl = false, your username and password will be transmitted over the network in cleartext.
				//Please don't transmit your username and password over the network in cleartext :)
				mainRequest.EnableSsl = true;
				//Allows mainRequest to make multiple requests. Otherwise, connection will close after one request.
				mainRequest.KeepAlive = true;
				//Now, we feed all of these into a NetworkCredentials class, and feed that into our FtpWebRequest
				mainRequest.Credentials = new NetworkCredential(userName, securePwd);

				//Other parameters that are important to have
				mainRequest.KeepAlive = true;
				mainRequest.UseBinary = true;
				mainRequest.UsePassive = true;

				loggedOn = listRemote.ListRemote(this);
				if (loggedOn)
				{
					Console.WriteLine("logOn successfull\n");
				}
				else
                {
					Console.WriteLine("Failed to log in\n");
                }
			}


			//maybe we could prompt for this? I'm not sure if it matters. Can be tacked on to the end of a NetworkCredential constructor.
			/*
			//Domain
			Console.Write("Enter Domain: ");
			string domain = Console.ReadLine();
			*/

		
		}

		public bool LogOff()
		{
			//Logs user off of open FTP connection

			try
			{
				//mainResponse.Close();
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

		public void DeleteFile()
		{
			try
			{
				mainRequest.Method = WebRequestMethods.Ftp.DeleteFile;

				using (FtpWebResponse response = (FtpWebResponse)mainRequest.GetResponse())
				{
					Console.WriteLine(response.StatusDescription);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}

		public void RenameRemoteFile(string newFilename)
		{
			try
			{
				mainRequest.Method = WebRequestMethods.Ftp.Rename;
				mainRequest.RenameTo = newFilename;
				mainResponse = (FtpWebResponse)mainRequest.GetResponse();
				mainResponse.Close();
			}
			catch(Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}
	}
}
