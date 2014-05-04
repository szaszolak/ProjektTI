using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace snifer
{
    class CapturedPacketModel
    {
        string sourceAddres;

        public string SourceAddres
        {
            get { return sourceAddres; }
            set { sourceAddres = value; }
        }
   

       
        string destinationAddres;

        public string DestinationAddres
        {
            get { return destinationAddres; }
            set { destinationAddres = value; }
        }
        string checkSum;

        public string CheckSum
        {
            get { return checkSum; }
            set { checkSum = value; }
        }

        byte ttl;
        public byte Ttl
        {
            get { return ttl; }
            set { ttl = value; }
        }

    }
}
