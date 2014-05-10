
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PcapDotNet.Packets;

namespace ShapeConnectors
{
    class CapturedPacketManager
    {
        public static CapturedPacketModel providePacket(Packet input)
        {
            CapturedPacketModel packet = new CapturedPacketModel();

            
            packet.SourceAddres = input.Ethernet.Source.ToString();
            packet.DestinationAddres = input.Ethernet.Destination.ToString();
         
            string tmp;
           
            if(input.IpV4.Payload != null)
             tmp = input.IpV4.HeaderChecksum.ToString() + input.IpV4.Payload.ToHexadecimalString();
            else
                tmp = input.IpV4.HeaderChecksum.ToString();

            packet.CheckSum = chceckSumCalculator.CalculateMD5Hash(tmp);
            packet.Ttl = input.IpV4.Ttl;
            
            return packet;
        }
    }
}
