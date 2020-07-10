using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security;
using System.Text;

namespace _410AgileCsharp
{
    class FtpHandler
    {
        public FtpWebRequest mainRequest;
        public FtpWebResponse mainResponse;

        public bool logOn() {
            //Allows a user to log on to an FTP server with username and password. 
            //Prompts user for username, domain, and password

            //Username
            Console.Write("Enter username: ");
            string userName = Console.ReadLine();
            Console.WriteLine();

            //Password stuff
            //We need to use SecureStrings to be able to feed FtpWebRequest a password. It's probably much better that way!
            SecureString securePwd = new SecureString();
            ConsoleKeyInfo key;

            Console.Write("Enter password: ");
            do
            {
                key = Console.ReadKey(true);

                // Ignore any key out of range.
                if (((int)key.Key) >= 65 && ((int)key.Key <= 90))
                {
                    // Append the character to the password.
                    securePwd.AppendChar(key.KeyChar);
                    Console.Write("*");
                }
                // Exit if Enter key is pressed.
            } while (key.Key != ConsoleKey.Enter);
            Console.WriteLine();

            //maybe we could prompt for this? 
            /*
            //Domain
            Console.Write("Enter Domain: ");
            string domain = Console.ReadLine();
            */
            
            //Now, we feed all of these into a NetworkCredentials class, and feed that into our FtpWebRequest
            mainRequest.Credentials = new NetworkCredential(userName, securePwd);

            return true;
        }

        
    }
}
