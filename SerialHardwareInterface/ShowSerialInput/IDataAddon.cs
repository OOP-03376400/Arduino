using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShowSerialInput
{
    public interface IDataAddon
    {
        void AddDataPackets(IList<DataPacket> packets);
    }
}
