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
