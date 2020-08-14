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

        [Obsolete]
        static void Main(string[] args)
		{
			string [] logHistory = new string[101];
			FtpHandler mainHandler = new FtpHandler();
			RemoteLS listRemote = new RemoteLS();
			RemoteMkDir mkDirRemote = new RemoteMkDir();
			Upload upload = new Upload();
			Download download = new Download();
			Rename rename = new Rename();
			Delete delete = new Delete();


			string[] lines = System.IO.File.ReadAllLines(@".\log.txt");

			int t;


				string[] number = System.IO.File.ReadAllLines(@".\number.txt");
				Console.WriteLine(number);
				t = Int32.Parse(number[0]);
			
			
			int i = 0;
			foreach (string line in lines)
			{
				if (i < t)
                {
					logHistory[i] = String.Copy(line);
					i++;
				}
			}

			Console.WriteLine("Log history has been loaded.\n");

			mainHandler.LogOnInitial();
			logHistory[i] = DateTime.Now.ToString() + "  log in";	
			i++;
			t++;

			try
			{
				bool loop = true;
				while (loop)
				{
					Console.WriteLine("\nEnter a command:\nls = List Directory Details\nupload = UpLoad File\nupload-m = Upload Multiple files\ndownload = DownLoad File\ndelete = Delete a File\nrr = Rename a Remote File\nmkdir = Make a Remote Directory\nsave = Save currenly connected server \nlog = Log History\nd = Disconnect\n");
					
					timer = new System.Timers.Timer();
					timer.Interval = 30000;
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
							logHistory[i] = DateTime.Now.ToString() + "  attempted to upload " + upFile;
							i++;
							t++;
							break;
						case "upload-m":
							Console.WriteLine("Enter the filename to put on the ftp seperated by \";\"'s.\n");
							string upFiles = Console.ReadLine();
							upload.UploadMultipleToRemote(mainHandler, upFiles);
							logHistory[i] = DateTime.Now.ToString() + "  attempted to upload " + upFiles;
							i++;
							t++;
							break;
						case "download":
							Console.WriteLine("Enter filename to take from the ftp.\n");
							string downFile = Console.ReadLine();
							download.DownloadFromRemote(mainHandler, downFile);
							logHistory[i] = DateTime.Now.ToString() + "  attempted to download " + downFile;
							i++;
							t++;
							break;
						case "delete":
							Console.WriteLine("Enter filename to DELETE from the ftp.\n");
							string deleteFile = Console.ReadLine();
							delete.DeleteRemoteFile(mainHandler, deleteFile);
							logHistory[i] = DateTime.Now.ToString() + "  attempted to delete " + deleteFile;
							i++;
							t++;
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
							logHistory[i] = DateTime.Now.ToString() + "  attempted to rename " + currnetFile + " to " + newFileName;
							i++;
							t++;
							break;
						case "d":
							loop = false;
							break;
						case "mkdir":
							Console.WriteLine("Enter directory name. \n");
							string dir = Console.ReadLine();
							mkDirRemote.MkDirRemote(mainHandler, dir);
							logHistory[i] = DateTime.Now.ToString() + "  attempted to make a directory called " + dir;
							i++;
							t++;
							break;
						case "save":
							mainHandler.SaveInfo();
							break;
						case "log":
							for (int j = 0; j < i; j++)
                            {
								Console.WriteLine(logHistory[j]);
                            }
							break;
						default:
							Console.WriteLine("Wrong Command.\n");
							break;
					}
					command = string.Empty;
				}

				Console.WriteLine("Press enter to disconnect");
				Console.ReadLine();

				logHistory[i] = DateTime.Now.ToString() + "  log out";
				i++;
				t++;

				string [] tt = { t.ToString() };
				System.IO.File.WriteAllLines(@".\number.txt", tt);
				
				for (int j = 0; j < i; j++)
                {
					System.IO.File.WriteAllLines(@".\log.txt", logHistory);
				}
				Console.WriteLine("Log history has been saved.\n");

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




