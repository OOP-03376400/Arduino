using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows.Forms;

using ShowSerialInput;

namespace HighLightLevelDetector
{
    public class HighLightLevelDataAddon : IDataAddon
    {
        bool _alertThrown = false;

        #region IDataAddon Members

        public void AddDataPackets(IList<DataPacket> packets)
        {
            if (_alertThrown) return;
            foreach (DataPacket packet in packets)
            {
                if (packet.Key != "A1") continue;

                if (packet.Value > 900)
                {
                    _alertThrown = true;
                    // Start the message box -thread:
                    new Thread(new ThreadStart(delegate
                    {
                        MessageBox.Show
                        (
                          "MY EYES! THE GOGGLES DO NOTHING!",
                          "Light Threshold Exceeded",
                          MessageBoxButtons.OK,
                          MessageBoxIcon.Warning
                        );
                        _alertThrown = false;
                    })).Start();
                }
            }
        }

        #endregion
    }
}
