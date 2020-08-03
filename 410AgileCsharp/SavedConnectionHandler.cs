using System;
using System.Collections.Generic;
using System.Text;

namespace _410AgileCsharp
{
    struct SavedConnection{
        string url;
        string userName;
    }
    class SavedConnectionHandler
    {
        public SavedConnection[] connectionList;
        public void ReadAll()
        {
            //read from text file, load into an array of SavedConnections. Display entire array at the end. 

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
