using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace _410AgileCsharp
{
    struct SavedConnection{
        public string url;
        public string userName;
    }
    class SavedConnectionHandler
    {
        public SavedConnection[] connectionList;
        public void ReadAll()
        {
            //read from text file, load into an array of SavedConnections. Display entire array at the end. 
            try
            {
                using(var sr = new StreamReader("SavedConnections.txt"))
                {
                    for(int i = 0; !sr.EndOfStream; i++)
                    {
                        connectionList[i].url = sr.ReadLine();
                        connectionList[i].userName = sr.ReadLine();
                    }
                }
            }
            catch(IOException e)
            {
                Console.WriteLine("File could not be opened.");
                Console.WriteLine(e.Message);
            }
            catch(Exception flip)
            {
                Console.WriteLine("Unexpected exception while reading saved connection file");
                Console.WriteLine(flip.Message);
            }
        }

        public bool Connect(FtpHandler toConnect)
        {
            //Allow user to select from array of SavedConnections, and feed information into appropriate FtpHandler fields.
            //prompt for password
            return true;
        }

        public bool SaveConnection(FtpHandler toSave)
        {
            //Allow user to save connection that current FtpHandler is connected to. Append saved connection info, and write to text file.
            return true;
        }
    }
}
