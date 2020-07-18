using System;
using System.Net;
using System.IO;
namespace ListFilesOnRemoteServer
{
    class RemoteLS
    {
        public FtpWebRequest lsRequest;
        public FtpWebResponse lsResponse;

        public bool ListRemote()
        {
            try
            {
                lsRequest = (FtpWebRequest)WebRequest.Create("ftp://test.rebex.net/");
                lsRequest.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                lsRequest.Credentials = new NetworkCredential("demo", "password");
                lsResponse = (FtpWebResponse)lsRequest.GetResponse();
                Stream responseStream = lsResponse.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream);
                Console.WriteLine(reader.ReadToEnd());
                Console.WriteLine("List complete");
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
