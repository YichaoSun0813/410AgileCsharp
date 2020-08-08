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

			if (mainHandler.LogOnInitial())
			{
				try
				{
					bool loop = true;
					while (loop)
					{
						Console.WriteLine("\nEnter a command:\nls = List Directory Details\nupload = UpLoad File\ndownload = DownLoad File\nsave = Save Connection Information\nd = Disconnect\n");
						string command = Console.ReadLine();

						switch (command.ToLower())
						{
							case "upload":
								Console.WriteLine("Enter the filename to put on the ftp.\n");
								string upFile = Console.ReadLine();
								mainHandler.mainRequest = (FtpWebRequest)WebRequest.Create(mainHandler.url + '/' + upFile);
								mainHandler.LogOn();
								mainHandler.UpLoad(upFile);
								break;
							case "download":
								Console.WriteLine("Enter filename to take from the ftp.\n");
								string downFile = Console.ReadLine();
								mainHandler.mainRequest = (FtpWebRequest)WebRequest.Create(mainHandler.url + '/' + downFile);
								mainHandler.LogOn();
								mainHandler.DownLoad();
								break;
							case "delete":
								Console.WriteLine("Enter filename to DELETE from the ftp.\n");
								string deleteFile = Console.ReadLine();
								mainHandler.mainRequest = (FtpWebRequest)WebRequest.Create(mainHandler.url + '/' + deleteFile);
								mainHandler.LogOn();
								mainHandler.DeleteFile();
								break;
							case "ls":
								mainHandler.mainRequest = (FtpWebRequest)WebRequest.Create(mainHandler.url);
								mainHandler.LogOn();
								mainHandler.ListDirectoryDetails();
								break;
							case "save":
								mainHandler.SaveInfo();
								break;
							case "rr":
								Console.WriteLine("Enter filename to Rename from the ftp.\n");
								string currnetFile = Console.ReadLine();
								Console.WriteLine("Enter the new name.\n");
								string newFileName = Console.ReadLine();
								mainHandler.mainRequest = (FtpWebRequest)WebRequest.Create(mainHandler.url + '/' + currnetFile);
								mainHandler.LogOn();
								mainHandler.RenameRemoteFile(newFileName);
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
            else
            {
				Console.WriteLine("Log on unsuccessful. :(");
				return;
            }
		}
	}
}
