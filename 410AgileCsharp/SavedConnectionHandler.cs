using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Security;
using System.Net;

namespace _410AgileCsharp
{
    struct SavedConnection{
        public string url;
        public string userName;
    }
    class SavedConnectionHandler
    {
        public List<SavedConnection> connectionList;
        public bool ReadAll()
        {
            connectionList = new List<SavedConnection>();
            //read from text file, load into an array of SavedConnections. Display entire array at the end. 
            try
            {
                //check if file exists, create it if it does not
                if (!File.Exists("SavedConnections.txt"))
                {
                    System.IO.File.Create("SavedConnections.txt");
                    Console.WriteLine("No saved connection file found. Creating an empty file and exiting.");
                    return false;
                }

                string[] allLines = System.IO.File.ReadAllLines("SavedConnections.txt");

                for(int i = 0; i < allLines.Length; i++, i++)
                {
                    connectionList.Add(new SavedConnection
                    {
                        url = allLines[i],
                        userName = allLines[i + 1]
                    });
                }

                return true;

                /*
                for (int i = 0; !sr.EndOfStream; i++)
                {
                    connectionList[i] = new SavedConnection
                    {
                        url = sr.ReadLine(),
                        userName = sr.ReadLine()
                    };
                }
                */
            }
            catch(IOException e)
            {
                Console.WriteLine("File could not be opened.");
                Console.WriteLine(e.Message);
                return false;
            }
            catch(Exception flip)
            {
                Console.WriteLine("Unexpected exception while reading saved connection file");
                Console.WriteLine(flip.Message);
                return false;
            }
        }

        public bool Connect(FtpHandler toConnect)
        {
            //Allow user to select from array of SavedConnections, and feed information into appropriate FtpHandler fields.
            //prompt for password
            
            try
            {
                if(connectionList == null)
                {
                    Console.WriteLine("No information in SavedConnections.txt");
                    return false;
                }
                //print out all possible connections
                for(int i = 0; i < connectionList.Count; i++)
                {
                    Console.WriteLine(i);
                    Console.WriteLine("url: " + connectionList[i].url);
                    Console.WriteLine("username: " + connectionList[i].userName);
                }

                //prompt user for a selection
                Console.Write("Make a selection: ");
                string userInput = Console.ReadLine();
                int userResult;
                for (; ; )
                {
                    try
                    {
                        userResult = Int32.Parse(userInput);
                        if(userResult < 0 || userResult > connectionList.Count)
                        {
                            throw new Exception();
                        }
                        break;
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("Cannot parse. Please try again!");
                        Console.WriteLine(": ");
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Input out of range. Try again!");
                        Console.WriteLine(": ");
                    }
                }

                //feed connectionList result into our FtpHandler
                toConnect.url = connectionList[userResult].url;
                toConnect.currentUser = connectionList[userResult].userName;

                //the rest is copy paste from logonunsaved

                toConnect.mainRequest = (FtpWebRequest)WebRequest.Create(toConnect.url);

                toConnect.currentPass = new SecureString();
                ConsoleKeyInfo key;

                Console.Write("Enter password: ");
                do
                {
                    key = Console.ReadKey(true);

                    // Ignore any key out of range.
                    if (((int)key.Key) >= 65 && ((int)key.Key <= 90))
                    {
                        // Append the character to the password.
                        toConnect.currentPass.AppendChar(key.KeyChar);
                        Console.Write("*");
                    }
                    // Exit if Enter key is pressed.
                } while (key.Key != ConsoleKey.Enter);
                Console.WriteLine();

                //Now, we feed all of these into a NetworkCredentials class, and feed that into our FtpWebRequest
                toConnect.mainRequest.Credentials = new NetworkCredential(toConnect.currentUser, toConnect.currentPass);

                //Other parameters that are important to have
                toConnect.mainRequest.KeepAlive = false;
                toConnect.mainRequest.UseBinary = true;
                toConnect.mainRequest.UsePassive = true;

                //List all files in directory, for testing, and to make sure we're connected
                toConnect.mainRequest.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                //Assign to mainResponse
                toConnect.mainResponse = (FtpWebResponse)toConnect.mainRequest.GetResponse();
                //Read the response stream
                Stream responseStream = toConnect.mainResponse.GetResponseStream();
                StreamReader responseRead = new StreamReader(responseStream);
                //Print
                Console.WriteLine(responseRead.ReadToEnd());

                //gracefully exit reader and response
                responseRead.Close();
                toConnect.mainResponse.Close();
                return true;
                
            }
            catch(Exception bruh)
            {
                Console.WriteLine(bruh.Message);
                return false;
            }
        }

        public bool SaveConnection(FtpHandler toSave)
        {
            //Allow user to save connection that current FtpHandler is connected to. Append saved connection info, and write to text file.
            if(connectionList == null)
            {
                if (!ReadAll()) {
                    Console.WriteLine("Unable to read file. Exiting connection saving.");
                    return false; 
                };
            }
            StreamWriter sw = File.AppendText("SavedConnections.txt");
            sw.WriteLine(toSave.url);
            sw.WriteLine(toSave.currentUser);

            return true;
        }
    }
}
