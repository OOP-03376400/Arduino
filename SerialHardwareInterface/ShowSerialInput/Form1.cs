using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

namespace ShowSerialInput
{
    public partial class FormSerialDisplay : Form, IDataAddon
    {
        private SerialPort comPort = new SerialPort();
        ArduinoCircuit _arduinoCircuit = new ArduinoCircuit();

        public FormSerialDisplay()
        {
            InitializeComponent();

            comboBoxPort.DataSource = SerialPort.GetPortNames();
            comboBoxPort.SelectedIndex = comboBoxPort.Items.Count - 1;

            graphControl.AssignColor("A0", Color.Red);
            graphControl.AssignColor("A1", Color.Yellow);
            graphControl.AssignColor("A2", Color.Cyan);
            graphControl.AssignColor("A3", Color.Magenta);
            graphControl.AssignColor("A4", Color.White);
            graphControl.AssignColor("A5", Color.Lime);

            _arduinoCircuit.AddHandler(this);
        }

        protected override void WndProc(ref Message m)
        {
            // Abort screensaver and monitor power-down
            const int WM_SYSCOMMAND = 0x0112;
            const int SC_MONITOR_POWER = 0xF170;
            const int SC_SCREENSAVE = 0xF140;
            int WParam = (m.WParam.ToInt32() & 0xFFF0);

            if (m.Msg == WM_SYSCOMMAND &&
                (WParam == SC_MONITOR_POWER || WParam == SC_SCREENSAVE)) return;

            base.WndProc(ref m);
        }

        public void AddDataPackets(IList<DataPacket> packets)
        {
            graphControl.AddDataPackets(packets);
            RedrawGraph(graphControl);
        }

        [STAThread]
        private static void RedrawGraph(GraphControl graphControl)
        {
            if (!graphControl.IsHandleCreated) return;
            graphControl.Invoke(new EventHandler(
                delegate { graphControl.Invalidate(); }));
        }

        private void comboBoxPort_SelectedIndexChanged(object sender, EventArgs e)
        {
            _arduinoCircuit.SetPort(comboBoxPort.Text);
        }

        private void buttonLoad_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = "Select a file";
                dlg.Filter = "Library (*.dll) |*.dll";
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    Assembly assembly = Assembly.LoadFrom(dlg.FileName);
                    foreach (Type type in assembly.GetTypes())
                    {
                        IDataAddon addon = Activator.CreateInstance(type) as IDataAddon;
                        if (addon == null) continue;
                        _arduinoCircuit.AddHandler(addon);
                    }
                }
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
