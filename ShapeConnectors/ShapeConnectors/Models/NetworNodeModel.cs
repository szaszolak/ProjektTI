using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShapeConnectors
{
    class NetworNodeModel
    {
        string ownMacAddress;
        Dictionary<string, int> communicationWith;

        public void addBoundedMac(string Mac)
        {
            communicationWith.Add(Mac, 1);
        }

        public void updateCommunicationDictionary(string Mac)
        {
            if (communicationWith.ContainsKey(Mac))
                communicationWith[Mac]++;
            else
                addBoundedMac(Mac);
        }
        public Dictionary<string, int> CommunicationWith
        {
            get { return communicationWith; }
            set { communicationWith = value; }
        }

        
    }
}
