
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PcapDotNet.Packets;

namespace snifer
{
    class CapturedPacketManager
    {
        public static CapturedPacketModel providePacket(Packet input)
        {
            CapturedPacketModel packet = new CapturedPacketModel();

            
            packet.SourceAddres = input.Ethernet.Source.ToString();
            packet.DestinationAddres = input.Ethernet.Destination.ToString();
         
            string tmp;
           // input.Ethernet.Trailer.ToHexadecimalString();
            if (input.IpV4.Payload != null)
                tmp = input.Timestamp.Ticks.ToString();
            else
                tmp = input.Timestamp.Ticks.ToString();

            packet.CheckSum = tmp;//( input.Ethernet.Trailer == null?tmp:chceckSumCalculator.CalculateMD5Hash(input.Ethernet.Trailer.ToHexadecimalString()));// tmp; //tmp);
            packet.Ttl = input.IpV4.Ttl;
            
            return packet;
        }
    }
}
