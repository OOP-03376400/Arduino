using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

using ShowSerialInput;

namespace ImpactDetector
{
    class ImpactDetectorDataAddon : IDataAddon
    {
        bool _alertThrown = false;
        Queue<int> _last20DataPoints = new Queue<int>(20);

        #region IDataAddon Members

        public void AddDataPackets(IList<DataPacket> packets)
        {
            foreach (DataPacket packet in packets)
            {
                if (packet.Key != "A0") continue;

                while (_last20DataPoints.Count > 19)
                {
                    _last20DataPoints.Dequeue();
                }
                _last20DataPoints.Enqueue(packet.Value);
                int[] data = _last20DataPoints.ToArray();

                if (ImpactDetected(data))
                {
                    if (_alertThrown) return;
                    _alertThrown = true;
                    MessageBox.Show
                    (
                      "Impact detected! Call 911!!",
                      "Impact detected!",
                      MessageBoxButtons.OK,
                      MessageBoxIcon.Exclamation
                    );
                }
                else if (AllClearDetected(data))
                {
                    _alertThrown = false;
                }
            }
        }

        #endregion

        const int freefall = 200;
        const int impactThreshold = 700;
        const int still = 100;
        static bool ImpactDetected(int[] data)
        {
            if (data.Length < 20) return false;
            return (data[0] < freefall) &&
                (data[10] > impactThreshold) &&
                (data[19] < still);
        }

        static bool AllClearDetected(int[] data)
        {
            foreach (int pt in data)
            {
                if (pt > freefall) return false;
            }
            return true;
        }
    }
}
