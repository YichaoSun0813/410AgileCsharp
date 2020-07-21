using System;
using System.Net;
using System.Security;

namespace _410AgileCsharp
{
	class Program
	{
		static void Main(string[] args)
		{
			FtpHandler mainHandler = new FtpHandler();

			//Prompt for FTP URL, and add into FtpWebRequest.
			//Keep in mind that this needs to be a full URL, which should look something like this: ftp://HostName.com/
			//This also initializes FtpWebRequest, which is kinda major. Maybe we should find a way to move this? 
			//Maybe mainHandler constructor?
			Console.Write("Enter an FTP server URL: ");
			string url = Console.ReadLine();

			//Allows a user to log on to an FTP server with username and password. 
			//Prompts user for username, domain, and password
			//Username
			Console.Write("Enter username: ");
			mainHandler.userName = Console.ReadLine();
			Console.WriteLine();

			//Password stuff
			//We need to use SecureStrings to be able to feed FtpWebRequest a password. It's probably much better that way!
			mainHandler.securePwd = new SecureString();
			ConsoleKeyInfo key;
			Console.Write("Enter password: ");
			do
			{
				key = Console.ReadKey(true);

				// Ignore any key out of range.
				if (((int)key.Key) >= 65 && ((int)key.Key <= 90))
				{
					// Append the character to the password.
					mainHandler.securePwd.AppendChar(key.KeyChar);
					Console.Write("*");
				}
				// Exit if Enter key is pressed.
			} while (key.Key != ConsoleKey.Enter);
			Console.WriteLine();

			mainHandler.mainRequest = (FtpWebRequest)WebRequest.Create(url);
			//HUGE: with EnableSsl = false, your username and password will be transmitted over the network in cleartext.
			//Please don't transmit your username and password over the network in cleartext :)
			mainHandler.mainRequest.EnableSsl = true;
			//Allows mainRequest to make multiple requests. Otherwise, connection will close after one request.
			mainHandler.mainRequest.KeepAlive = true;
			mainHandler.LogOn();
			mainHandler.ListDirectoryDetails();
			Console.WriteLine("logOn successfull\n");

			try
			{
				bool loop = true;
				while (loop)
				{
					Console.WriteLine("\nEnter a command:\nls = List Directory Details\nupload = UpLoad File\ndownload = DownLoad File\nd = Disconnect\n");
					string command = Console.ReadLine();

					switch (command.ToLower())
					{
						case "upload":
							Console.WriteLine("Enter the filename to put on the ftp.\n");
							string upFile = Console.ReadLine();
							mainHandler.mainRequest = (FtpWebRequest)WebRequest.Create(url + '/' + upFile);
							mainHandler.LogOn();
							mainHandler.UpLoad(upFile);
							break;
						case "download":
							Console.WriteLine("Enter filename to take from the ftp.\n");
							string downFile = Console.ReadLine();
							mainHandler.mainRequest = (FtpWebRequest)WebRequest.Create(url + '/' + downFile);
							mainHandler.LogOn();
							mainHandler.DownLoad();
							break;
						case "ls":
							mainHandler.mainRequest = (FtpWebRequest)WebRequest.Create(url);
							mainHandler.LogOn();
							mainHandler.ListDirectoryDetails();
							break;
						case "d":
							loop = false;
							break;
						default:
							Console.WriteLine("Wrong Command.\n");
							break;
					}
					command = string.Empty;
				}
				Console.WriteLine("Press enter to disconnect");
				Console.ReadLine();
				mainHandler.LogOff();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
			}
		}
	}
}
