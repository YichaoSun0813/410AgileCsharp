using _410AgileCsharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Linq;

namespace CopyRemote
{
    class CopyRemoteDirectory
    { 
        public FtpWebRequest lsRequest;
        public FtpWebResponse lsResponse;
        public bool Copy(FtpHandler handler)
        {
            var names = GetNames(handler);
            var directories = new List<string>();
            foreach (string name in names)
            {
                if (!name.Contains("."))
                { 
                    directories.Add(name);
                }
            }
            Console.WriteLine("Directories to copy:");
            foreach(string directory in directories)
            {
                Console.WriteLine(directory);
            }
            Console.WriteLine("Enter the directory to copy\n");
            var to_copy = Console.ReadLine();
            FtpHandler copyHandler = new FtpHandler();
            copyHandler.url = handler.url + to_copy;
            copyHandler.userName = handler.userName;
            copyHandler.securePwd = handler.securePwd;
            var currentpath = Directory.GetCurrentDirectory();
            string pathString = System.IO.Path.Combine(currentpath, to_copy);
            System.IO.Directory.CreateDirectory(pathString);
            FtpWebRequest request;
            request = (FtpWebRequest)WebRequest.Create(handler.url + to_copy);
            request.Method = WebRequestMethods.Ftp.DownloadFile;
            request.Credentials = new NetworkCredential(handler.userName, handler.securePwd);

            return true;
        }

        private bool DownloadAll(FtpHandler handler, string path)
        {
            var allNames = GetNames(handler);
            foreach (var name in allNames)
            {
                FtpWebRequest request;
                request = (FtpWebRequest)WebRequest.Create(handler.url + "/" + name);
                request.Method = WebRequestMethods.Ftp.DownloadFile;
                request.Credentials = new NetworkCredential(handler.userName, handler.securePwd);
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream);
                StreamWriter dest = new StreamWriter(path);
                dest.Write(reader.ReadToEnd());
                dest.Flush();
            }
            
            return true;
        }

        private string[] GetNames (FtpHandler handler)
        {
            lsRequest = (FtpWebRequest)WebRequest.Create(handler.url);
            lsRequest.Method = WebRequestMethods.Ftp.ListDirectory;
            lsRequest.Credentials = new NetworkCredential(handler.userName, handler.securePwd);
            lsResponse = (FtpWebResponse)lsRequest.GetResponse();
            Stream responseStream = lsResponse.GetResponseStream();
            StreamReader reader = new StreamReader(responseStream);
            var allNames = reader.ReadToEnd();
            string[] delimiters = { "\r\n" };
            var individualNames = allNames.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
            return individualNames;
        }

    }
}
