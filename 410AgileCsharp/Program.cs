using System;
using ListFilesOnRemoteServer;

namespace _410AgileCsharp
{
    class Program
    {
        static void Main(string[] args)
        {
            RemoteLS remoteLs = new RemoteLS(); 
            Console.WriteLine("Hello World!");
            Console.WriteLine("What command?");
            var command = Console.ReadLine();
            if (command.ToLower() == "ls")
            {
                remoteLs.ListRemote();
            }
        }
    }
}
