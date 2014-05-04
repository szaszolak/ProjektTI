using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PcapDotNet.Core;
using PcapDotNet.Packets;
using PcapDotNet.Packets.IpV4;
using PcapDotNet.Packets.Transport;
using PcapDotNet.Base;
using System.Windows.Forms;

namespace snifer
{
    class LiveCapturer
    {
        IList<LivePacketDevice> allDevices;
        DataGridView captured_packet_displayer;
        public LiveCapturer(DataGridView lBox)
        {
            captured_packet_displayer = lBox;
            allDevices = LivePacketDevice.AllLocalMachine;
        }

        public void start_capture(object device)
        {
            PacketDevice selectedDevice;
            if (device is PacketDevice)
            {
                selectedDevice = (PacketDevice)device;
            }
            else
                throw new Exception("Wrong argument given");

            using (PacketCommunicator communicator =
           selectedDevice.Open(65536,                                  // portion of the packet to capture
                // 65536 guarantees that the whole packet will be captured on all the link layers
                               PacketDeviceOpenAttributes.Promiscuous, // promiscuous mode
                               1000))                                  // read timeout
            {
                //  Console.WriteLine("Listening on " + selectedDevice.Description + "...");

                // Retrieve the packets
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
                            PacketHandler(packet);
                            break;
                        default:
                            throw new InvalidOperationException("The result " + result + " shoudl never be reached here");
                    }
                } while (true);
            }

        }
            void PacketHandler(Packet packet)
        {
            IpV4Datagram ip = packet.Ethernet.IpV4;
            UdpDatagram udp = ip.Udp;
            captured_packet_displayer.Invoke(new Action(() => captured_packet_displayer.Rows.Add(packet.Ethernet.Source.ToString(),packet.Ethernet.IpV4.CurrentDestination, packet.Ethernet.Destination )));
            captured_packet_displayer.Invoke(new Action(() => captured_packet_displayer.Update()));
        }
    
    }
}