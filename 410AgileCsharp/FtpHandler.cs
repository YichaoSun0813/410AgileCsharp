using ListFilesOnRemoteServer;
using System;
using System.Net;
using System.Security;
using System.Timers;

namespace _410AgileCsharp
{
	public class FtpHandler
	{
		public FtpWebRequest mainRequest;
		public FtpWebResponse mainResponse;
		public SecureString securePwd;
		public string userName;
		public string url;
		private SavedConnectionHandler savedConnections;

		private static Timer timer;
		private static void OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e)
		{
			Console.WriteLine("Timeout！");
			System.Environment.Exit(0);
		}
		public bool LogOnInitial()
		{
			for (; ; )
			{
				Console.WriteLine("1) Log onto a server with url, username, and password, OR");
				Console.WriteLine("2) Log onto a previously saved server");
				Console.Write(": ");

				timer = new System.Timers.Timer();
				timer.Interval = 10000;
				timer.Elapsed += OnTimedEvent;
				timer.AutoReset = true;
				timer.Enabled = true;

				string userInput = Console.ReadLine();
				timer.Enabled = false;
				switch (userInput)
				{
					case "1":
						return LogOnUnsaved();
					case "2":
						if (LogOnSaved()) { return true; }
						else break;
					default:
						Console.WriteLine("Unrecognized Input");
						break;
				}
			}
		}

		private bool LogOnUnsaved()
		{
			try
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

					timer = new System.Timers.Timer();
					timer.Interval = 10000;
					timer.Elapsed += OnTimedEvent;
					timer.AutoReset = true;
					timer.Enabled = true;

					url = Console.ReadLine();
					timer.Enabled = false;



					//Allows a user to log on to an FTP server with username and password. 
					//Prompts user for username, domain, and password
					//Username
					Console.Write("Enter username: ");

					timer = new System.Timers.Timer();
					timer.Interval = 10000;
					timer.Elapsed += OnTimedEvent;
					timer.AutoReset = true;
					timer.Enabled = true;

					userName = Console.ReadLine();
					timer.Enabled = false;
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

				return true;
			}
			catch (Exception OhNo)
			{
				Console.WriteLine(OhNo.Message.ToString());
				return false;
			}
		}

		public bool LogOnSaved()
		{
			savedConnections = new SavedConnectionHandler();
			if (savedConnections.ReadAll())
			{
				return savedConnections.Connect(this);
			}
			else
			{
				return false;
			}
		}

		public void SaveInfo()
		{
			//saves current connection into savedConnections.
			if (savedConnections == null) { savedConnections = new SavedConnectionHandler(); }
			savedConnections.SaveConnection(this);
		}

		public bool LogOff()
		{
			//Logs user off of open FTP connection

			try
			{
				if (mainResponse != null) { mainResponse.Close(); }

				Console.WriteLine("Connection Closed!");
				return true;
			}
			catch (Exception OhJeeze)
			{
				Console.WriteLine(OhJeeze.Message.ToString());
				return false;
			}
		}
	}
}