using System;
using System.Collections.Generic;
using System.IO;
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
            try
            {
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

                //maybe we could prompt for this? I'm not sure if it matters. Can be tacked on to the end of a NetworkCredential constructor.
                /*
                //Domain
                Console.Write("Enter Domain: ");
                string domain = Console.ReadLine();
                */

                //Now, we feed all of these into a NetworkCredentials class, and feed that into our FtpWebRequest
                mainRequest.Credentials = new NetworkCredential(userName, securePwd);

                //Other parameters that are important to have
                mainRequest.KeepAlive = false;
                mainRequest.UseBinary = true;
                mainRequest.UsePassive = true;

                //List all files in directory, for testing, and to make sure we're connected
                mainRequest.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                //Assign to mainResponse
                mainResponse = (FtpWebResponse)mainRequest.GetResponse();
                //Read the response stream
                Stream responseStream = mainResponse.GetResponseStream();
                StreamReader responseRead = new StreamReader(responseStream);
                //Print
                Console.WriteLine(responseRead.ReadToEnd());

                //gracefully exit reader and response
                responseRead.Close();
                mainResponse.Close();


                return true;
            }
            catch(Exception OhNo)
            {
                Console.WriteLine(OhNo.Message.ToString());
                return false;
            }

        }

        
    }
}
