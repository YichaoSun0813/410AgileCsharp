using System;
using System.Net;
using System.IO;
namespace CreateRemoteDirectory
{
    class RemoteMkDir
    {
        public FtpWebRequest FtpWebRequest;
        public FtpWebResponse ftpWebResponse;

        public bool MkDirRemote()
        {
            try
            {

                FtpWebRequest = (FtpWebRequest)WebRequest.Create("ftp://test.rebex.net/");
                FtpWebRequest.Method = WebRequestMethods.Ftp.MakeDirectory;


            }
            catch
            {

            }
        }

    }
}