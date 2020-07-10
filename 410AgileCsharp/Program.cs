using System;
using System.Net;

namespace _410AgileCsharp
{
    class Program
    {
        static void Main(string[] args)
        {
            FtpWebRequest mainRequest;
            FtpHandler mainHandler = new FtpHandler();

            Console.Write("Enter an FTP server URL: ");
            string url = Console.ReadLine();

            mainRequest = (FtpWebRequest)WebRequest.Create(url);

            mainHandler.logOn(mainRequest);

            Console.WriteLine("Hello World!");
        }
    }
}
