using CreateRemoteDirectory;
using DeleteFileFromRemoteServer;
using DownloadFileFromRemoteServer;
using ListFilesOnRemoteServer;
using RenameFileFromRemoteServer;
using System;
using System.IO;
using UploadFileToRemoteServer;
using System.Timers;

namespace _410AgileCsharp
{
	class Program
	{
		private static Timer timer;
		private static void OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e)
		{
			Console.WriteLine("Timeout！");
			System.Environment.Exit(0);
		}
		
		static void Main(string[] args)
		{
			FtpHandler mainHandler = new FtpHandler();
			RemoteLS listRemote = new RemoteLS();
			RemoteMkDir mkDirRemote = new RemoteMkDir();
			Upload upload = new Upload();
			Download download = new Download();
			Rename rename = new Rename();
			Delete delete = new Delete();

			mainHandler.LogOnInitial();

			try
			{
				bool loop = true;
				while (loop)
				{
					Console.WriteLine("\nEnter a command:\nls = List Directory Details\nupload = UpLoad File\nupload-m = Upload Multiple files\ndownload = DownLoad File\ndelete = Delete a File\nrr = Rename a Remote File\nmkdir = Make a Remote Directory\nsave = Save currenly connected server \nd = Disconnect\n");
					
					timer = new System.Timers.Timer();
					timer.Interval = 10000;
					timer.Elapsed += OnTimedEvent;
					timer.AutoReset = true;
					timer.Enabled = true;

					string command = Console.ReadLine();
					timer.Enabled = false;

					switch (command.ToLower())
					{
						case "upload":
							Console.WriteLine("Enter the filename to put on the ftp.\n");
							string upFile = Console.ReadLine();
							upload.UploadToRemote(mainHandler, upFile);
							break;
						case "upload-m":
							Console.WriteLine("Enter the filename to put on the ftp seperated by \";\"'s.\n");
							string upFiles = Console.ReadLine();
							upload.UploadMultipleToRemote(mainHandler, upFiles);
							break;
						case "download":
							Console.WriteLine("Enter filename to take from the ftp.\n");
							string downFile = Console.ReadLine();
							download.DownloadFromRemote(mainHandler, downFile);
							break;
						case "delete":
							Console.WriteLine("Enter filename to DELETE from the ftp.\n");
							string deleteFile = Console.ReadLine();
							delete.DeleteRemoteFile(mainHandler, deleteFile);
							break;
						case "ls":
							listRemote.ListRemote(mainHandler);
							break;
						case "rr":
							Console.WriteLine("Enter filename to Rename from the ftp.\n");
							string currnetFile = Console.ReadLine();
							Console.WriteLine("Enter the new name.\n");
							string newFileName = Console.ReadLine();
							rename.RenameRemoteFile(mainHandler, currnetFile, newFileName);
							break;
						case "d":
							loop = false;
							break;
						case "mkdir":
							Console.WriteLine("Enter directory name. \n");
							string dir = Console.ReadLine();
							mkDirRemote.MkDirRemote(mainHandler, dir);
							break;
						case "save":
							mainHandler.SaveInfo();
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
				File.WriteAllText("log.txt", ex.ToString());
			}
		}
	}
}
