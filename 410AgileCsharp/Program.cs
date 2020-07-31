using ListFilesOnRemoteServer;
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
            mainHandler.url = url;


            //HUGE: with EnableSsl = false, your username and password will be transmitted over the network in cleartext.
            //Please don't transmit your username and password over the network in cleartext :)
            mainHandler.mainRequest.EnableSsl = true;
            //Allows mainRequest to make multiple requests. Otherwise, connection will close after one request.
            mainHandler.mainRequest.KeepAlive = false;
            var command = "l";
            var request = mainHandler.LogOn();
            var listRemote = new RemoteLS();

            if (request != null)
            {
                Console.WriteLine("logOn successfull");
                while (command != "LogOff")
                {
                    Console.WriteLine("Enter a command");
                    command = Console.ReadLine();
                    if (command == "ls")
                    {
                        listRemote.ListRemote(mainHandler);
                    }
                }
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
