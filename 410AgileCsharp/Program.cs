using System;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace _410AgileCsharp
{
    class Program
    {
        static void Main(string[] args)
        {
            FtpHandler mainHandler = new FtpHandler();

            //Prompt for FTP URL, and add into FtpWebRequest.
            //Keep in mind that this needs to be a full URL, which should look something like this: ftp://HostName.com/
            //This also initializes FtpWebRequest, which is kinda major. Maybe we should find a way to move this? 
            //Maybe mainHandler constructor?
            Console.Write("Enter an FTP server URL: ");
            string url = Console.ReadLine();
            mainHandler.mainRequest = (FtpWebRequest)WebRequest.Create(url);

            //HUGE: with EnableSsl = false, your username and password will be transmitted over the network in cleartext.
            //Please don't transmit your username and password over the network in cleartext :)
            mainHandler.mainRequest.EnableSsl = true;
            //Allows mainRequest to make multiple requests. Otherwise, connection will close after one request.
            mainHandler.mainRequest.KeepAlive = false;

            if (mainHandler.LogOn())
            {
                Console.WriteLine("logOn successfull");
                Console.WriteLine("Press enter to disconnect");
                Console.ReadLine();
                mainHandler.LogOff();
            }
            else
            {
                Console.WriteLine("logOn unsuccessfull");
            }

            //Console.WriteLine("Hello World!");
        }
    }
}
