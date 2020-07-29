using System;
using System.Net;
using System.IO;
using _410AgileCsharp;

namespace ListFilesOnRemoteServer
{
    class RemoteLS
    {
        public FtpWebRequest lsRequest;
        public FtpWebResponse lsResponse;

        public bool ListRemote(FtpHandler handler)
        {
            try
            {
                lsRequest = (FtpWebRequest)WebRequest.Create(handler.url);
                lsRequest.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                lsRequest.Credentials = new NetworkCredential(handler.savedUserName, handler.savedPassword);
                lsResponse = (FtpWebResponse)lsRequest.GetResponse();
                Stream responseStream = lsResponse.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream);
                Console.WriteLine(reader.ReadToEnd());
                reader.Close();
                lsResponse.Close();
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
