using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PcapDotNet.Core;
using PcapDotNet.Packets;
using PcapDotNet.Packets.IpV4;
using PcapDotNet.Packets.Transport;
using PcapDotNet.Base;
using System.Diagnostics;


namespace ShapeConnectors
{
    class FlieAnalizer
    {

        IList<LivePacketDevice> allDevices;
        List<CapturedPacketModel> capturedPackets;

        internal List<CapturedPacketModel> CapturedPackets
        {
            get { return capturedPackets; }
            set { capturedPackets = value; }
        }

        public FlieAnalizer()
        {
            allDevices = LivePacketDevice.AllLocalMachine;
            capturedPackets = new List<CapturedPacketModel>();
            
        }

        public void start_capture(object path_to_file)
        {

            if (!(path_to_file is string))
            {
                throw new Exception("bad type of input given");
            }

            OfflinePacketDevice selectedDevice = new OfflinePacketDevice((string)path_to_file);
            using (PacketCommunicator communicator =
                selectedDevice.Open(65536,                                  // portion of the packet to capture
                // 65536 guarantees that the whole packet will be captured on all the link layers
                                    PacketDeviceOpenAttributes.Promiscuous, // promiscuous mode
                                    1000))                          // read timeout
            {
                
                Packet packet;
                do
                {
                    PacketCommunicatorReceiveResult result = communicator.ReceivePacket(out packet);
                    switch (result)
                    {
                        case PacketCommunicatorReceiveResult.Timeout:
                            // Timeout elapsed
                            continue;
                        case PacketCommunicatorReceiveResult.Ok:
                            capturedPackets.Add(CapturedPacketManager.providePacket(packet));// PacketHandler(packet);
                            break;
                        case PacketCommunicatorReceiveResult.Eof:
                            return;
                        default:
                            throw new InvalidOperationException("The result " + result + " shoudl never be reached here");
                    }
                } while (true);
            }
               
            }

    


           
    }
}
