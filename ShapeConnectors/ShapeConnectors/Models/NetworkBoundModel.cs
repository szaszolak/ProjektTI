using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShapeConnectors
{
    class NetworkBoundModel
    {

        public string[] boundedNodes { get; private set; }
        public NetworkBoundModel(string firstMac,string secondMac)
        {
            boundedNodes = new string[]{firstMac,secondMac};
            packetCount = 1;
        }

        int packetCount;

        public int PacketCount
        {
            get { return packetCount; }
            set { packetCount = value; }
        }

    }
}
