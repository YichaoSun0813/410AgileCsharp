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

        public bool ListRemote(FtpHandler handler, string url)
        {
            try
            {
                lsRequest = (FtpWebRequest)WebRequest.Create(url);
                lsRequest.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                lsRequest.Credentials = new NetworkCredential(handler.userName, handler.securePwd);
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
