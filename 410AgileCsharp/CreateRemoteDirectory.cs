using System;
using System.Net;
using System.IO;
using _410AgileCsharp;

namespace CreateRemoteDirectory
{
    class RemoteMkDir
    {
        public FtpWebRequest ftpWebRequest;
        public FtpWebResponse ftpWebResponse;

        public bool MkDirRemote(FtpHandler handler, string name)
        {
            try
            {
                ftpWebRequest = (FtpWebRequest)WebRequest.Create(handler.url + name);
                ftpWebRequest.Method = WebRequestMethods.Ftp.MakeDirectory;
                ftpWebRequest.Credentials = new NetworkCredential(handler.savedUserName, handler.savedPassword);
                ftpWebResponse = (FtpWebResponse)ftpWebRequest.GetResponse();
                if (ftpWebResponse.StatusCode != FtpStatusCode.CommandOK)
                {
                    Console.WriteLine("directory failed top be created");
                    return false;
                }
                return true;
            }
            catch (Exception fail)
            {
                Console.WriteLine(fail.Message.ToString());
                return false;
            }
        }

    }
}