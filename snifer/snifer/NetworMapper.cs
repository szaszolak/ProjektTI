using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using snifer.Models;

namespace snifer
{
    class NetworMapper
    {
        List<CapturedPacketModel> capturedPackets;
        List<NetworkBoundModel> bounds;

        internal List<NetworkBoundModel> Bounds
        {
            get { return bounds; }
            set { bounds = value; }
        }

        public NetworMapper(List<CapturedPacketModel> capturedPackets)
        {
            bounds = new List<NetworkBoundModel>();
            this.capturedPackets = capturedPackets;
        }

        public void mapNetwork()
        {
            CapturedPacketModel packet;
            do
            {
                packet = capturedPackets.First();
                List<CapturedPacketModel> multipliedPackets = capturedPackets.FindAll(p => p.CheckSum == packet.CheckSum);
                capturedPackets.RemoveAll(p => p.CheckSum == packet.CheckSum);
                tracePassedBound(multipliedPackets);

            } while (capturedPackets.Count != 0);
           
        }


        public void tracePassedBound(List<CapturedPacketModel> packets) //mapowanie trasy pakietów, dla tego samego pakietu odebranego i wysłanego 
        {
           List<CapturedPacketModel> sortedPackets = packets.OrderByDescending(p => p.Ttl).ToList();
           for (int i = 0; i < sortedPackets.Count;i++ )
           {
               if (bounds.Exists(p => p.boundedNodes.Contains(sortedPackets[i].DestinationAddres) && p.boundedNodes.Contains(sortedPackets[i].SourceAddres)))
               {
                   bounds.First(p => p.boundedNodes.Contains(sortedPackets[i].DestinationAddres) && p.boundedNodes.Contains(sortedPackets[i].SourceAddres)).PacketCount += 1;
               }
               else
               {
                   bounds.Add(new NetworkBoundModel(sortedPackets[i].SourceAddres, sortedPackets[i].DestinationAddres));
               }

           }

           for (int i = 0; i < sortedPackets.Count-1; i++)
           {
               if (sortedPackets[i].DestinationAddres != sortedPackets[i + 1].SourceAddres)
               {
                   if(bounds.Exists(p => p.boundedNodes.Contains(sortedPackets[i].DestinationAddres)&& p.boundedNodes.Contains(sortedPackets[i+1].SourceAddres)))
                   {
                       bounds.First(p => p.boundedNodes.Contains(sortedPackets[i].DestinationAddres) && p.boundedNodes.Contains(sortedPackets[i+1].SourceAddres)).PacketCount += 1;
                   }
                   else
                   {
                       bounds.Add(new NetworkBoundModel(sortedPackets[i].DestinationAddres,sortedPackets[i+1].SourceAddres));
                   }
               }

           }
        }
    }
}
