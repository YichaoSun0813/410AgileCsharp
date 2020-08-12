using System;
using System.Net;
using System.IO;
using _410AgileCsharp;
using System.Linq;

namespace CreateRemoteDirectory
{
	public class RemoteMkDir
	{
		public FtpWebRequest ftpWebRequest;
		public FtpWebResponse ftpWebResponse;

		public bool MkDirRemote(FtpHandler handler, string name)
		{
			try
			{
				if (!handler.url.EndsWith('/'))
				{
					handler.url.Append('/');
				}
				ftpWebRequest = (FtpWebRequest)WebRequest.Create(handler.url + name);
				ftpWebRequest.Method = WebRequestMethods.Ftp.MakeDirectory;
				ftpWebRequest.Credentials = new NetworkCredential(handler.userName, handler.securePwd);
				ftpWebResponse = (FtpWebResponse)ftpWebRequest.GetResponse();
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