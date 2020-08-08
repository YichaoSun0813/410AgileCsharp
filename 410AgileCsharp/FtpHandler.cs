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
        public string url;
        public string currentUser;
        public SecureString currentPass;
        private SavedConnectionHandler savedConnections;

        public bool LogOnInitial() {
            for (; ; )
            {
				Console.WriteLine("1) Log onto a server with url, username, and password, OR");
				Console.WriteLine("2) Log onto a previously saved server");
				Console.Write(": ");
                string userInput = Console.ReadLine();
                switch (userInput) {
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

		public void LogOn()
		{
			//maybe we could prompt for this? I'm not sure if it matters. Can be tacked on to the end of a NetworkCredential constructor.
			/*
			//Domain
			Console.Write("Enter Domain: ");
			string domain = Console.ReadLine();
			*/

			//Now, we feed all of these into a NetworkCredentials class, and feed that into our FtpWebRequest
			mainRequest.Credentials = new NetworkCredential(currentUser, currentPass);

			//Other parameters that are important to have
			mainRequest.KeepAlive = true;
			mainRequest.UseBinary = true;
			mainRequest.UsePassive = true;

		}

		private bool LogOnUnsaved()
        {
            try
            {
                //Allows a user to log on to an FTP server with username and password. 
                //Prompts user for username, domain, and password

                //Prompt for FTP URL, and add into FtpWebRequest.
                //Keep in mind that this needs to be a full URL, which should look something like this: ftp://HostName.com/
                //This also initializes FtpWebRequest, which is kinda major. Maybe we should find a way to move this? 
                //Maybe mainHandler constructor?
                Console.Write("Enter an FTP server URL: ");
                url = Console.ReadLine();
                mainRequest = (FtpWebRequest)WebRequest.Create(url);

                //HUGE: with EnableSsl = false, your username and password will be transmitted over the network in cleartext.
                //Please don't transmit your username and password over the network in cleartext :)
                mainRequest.EnableSsl = true;
                //Allows mainRequest to make multiple requests. Otherwise, connection will close after one request.
                mainRequest.KeepAlive = false;


                //Username
                Console.Write("Enter username: ");
                currentUser = Console.ReadLine();
                Console.WriteLine();

                //Password stuff
                //We need to use SecureStrings to be able to feed FtpWebRequest a password. It's probably much better that way!
                currentPass = new SecureString();
                ConsoleKeyInfo key;

                Console.Write("Enter password: ");
                do
                {
                    key = Console.ReadKey(true);

                    // Ignore any key out of range.
                    if (((int)key.Key) >= 65 && ((int)key.Key <= 90))
                    {
                        // Append the character to the password.
                        currentPass.AppendChar(key.KeyChar);
                        Console.Write("*");
                    }
                    // Exit if Enter key is pressed.
                } while (key.Key != ConsoleKey.Enter);
                Console.WriteLine();

                //maybe we could prompt for this? I'm not sure if it matters. Can be tacked on to the end of a NetworkCredential constructor.
                /*
                //Domain
                Console.Write("Enter Domain: ");
                string domain = Console.ReadLine();
                */

                //Now, we feed all of these into a NetworkCredentials class, and feed that into our FtpWebRequest
                mainRequest.Credentials = new NetworkCredential(currentUser, currentPass);

                //Other parameters that are important to have
                mainRequest.KeepAlive = true;
                mainRequest.UseBinary = true;
                mainRequest.UsePassive = true;

				return true;
			}
            catch (Exception OhNo)
            {
                Console.WriteLine(OhNo.Message.ToString());
                return false;
            }
        }

		private bool LogOnSaved()
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
			if(savedConnections == null) { savedConnections = new SavedConnectionHandler(); }
			savedConnections.SaveConnection(this);
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
				if(mainResponse != null) { mainResponse.Close(); }
				
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
