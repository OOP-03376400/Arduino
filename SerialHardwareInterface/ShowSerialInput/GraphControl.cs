using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace ShowSerialInput
{
    public partial class GraphControl : UserControl
    {
        int _max = 100;
        Dictionary<string, List<int>> _data = new Dictionary<string, List<int>>();
        Dictionary<string, Color> _assignedColors = new Dictionary<string, Color>();
        private Mutex _mut = new Mutex();

        public GraphControl()
        {
            InitializeComponent();
        }

        public int Max
        {
            get { return _max; }
            set { _max = value; }
        }

        public void AssignColor(string key, Color color)
        {
            _assignedColors.Add(key, color);
        }

        public void AddDataPackets(IList<DataPacket> packets)
        {
            _mut.WaitOne();
            foreach (DataPacket packet in packets)
            {
                List<int> dataPoints;
                if (!_data.TryGetValue(packet.Key, out dataPoints))
                {
                    dataPoints = new List<int>();
                    _data.Add(packet.Key, dataPoints);
                }
                dataPoints.Add(packet.Value);
            }
            _mut.ReleaseMutex();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            _mut.WaitOne();
            // Double-buffer to remove flicker
            Graphics gxOff; //Offscreen graphics

            using (Bitmap bmpOffscreen = new Bitmap(ClientSize.Width, ClientSize.Height)) //Bitmap for doublebuffering
            using (gxOff = Graphics.FromImage(bmpOffscreen))
            {
                gxOff.Clear(this.BackColor);

                foreach (KeyValuePair<string, List<int>> dataSet in _data)
                {
                    Color penColor;
                    if (!_assignedColors.TryGetValue(dataSet.Key, out penColor))
                    {
                        penColor = System.Drawing.SystemColors.HotTrack;
                    }
                    Pen pen = new Pen(penColor);

                    List<int> dataPoints = dataSet.Value;
                    if (dataPoints.Count > 1)
                    {
                        double scaleFactor = (double)Height / _max;
                        Point ptLast = new Point(Width - 1, Height - (int)(dataPoints[dataPoints.Count - 1] * scaleFactor));

                        for (int i = dataPoints.Count - 2; i >= 0; i--)
                        {
                            Point pt = new Point(ptLast.X - 1, Height - (int)(dataPoints[i] * scaleFactor));
                            if (pt.X < 0) break;
                            gxOff.DrawLine(pen, ptLast, pt, (scaleFactor > .5));
                            ptLast = pt;
                        }
                    }
                }

                //Draw from the memory bitmap
                e.Graphics.DrawImage(bmpOffscreen, 0, 0);
            }
            _mut.ReleaseMutex();
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            //Do nothing (we'll redraw the entire client area in memory in OnPaint)
        }

        private void GraphControl_Resize(object sender, EventArgs e)
        {
            // Invalidate the whole control
            Invalidate();
        }
    }

    static class GraphicsEx
    {
        public static void DrawLine(this Graphics gx, Pen pen, Point pt1, Point pt2, bool bold)
        {
            gx.DrawLine(pen, pt1, pt2);
            if (bold)
            {
                Point ptUp1 = new Point(pt1.X, pt1.Y + 1);
                Point ptUp2 = new Point(pt2.X, pt2.Y + 1);
                gx.DrawLine(pen, ptUp1, ptUp2);

                Point ptDown1 = new Point(pt1.X, pt1.Y - 1);
                Point ptDown2 = new Point(pt2.X, pt2.Y - 1);
                gx.DrawLine(pen, ptDown1, ptDown2);
            }
        }
    }
}
