using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PcapDotNet.Core;
using System.Threading;
using snifer.Models;

namespace snifer
{
    public partial class Form1 : Form
    {
        IList<LivePacketDevice> networkDevices;
        List<RadioButton> rBtn_device_list; 
        LiveCapturer live_packet_capturer;
        Thread capture_th;
        public Form1()
        {
            rBtn_device_list = new List<RadioButton>();
           
            InitializeComponent();
            PrepareForm();
            live_packet_capturer = new LiveCapturer(this.dGVpackets);
        }

        void PrepareForm()
        {
            networkDevices = LivePacketDevice.AllLocalMachine;
            int i = 1;
            int positioner = 38;
            foreach (LivePacketDevice device in networkDevices)
            {
                // rBtn_Networ_Devices
            // 
            this.rBtn_Networ_Devices.AutoSize = true;
            this.rBtn_Networ_Devices.Location = new System.Drawing.Point(296,positioner);
            this.rBtn_Networ_Devices.Name = ""+i;
            this.rBtn_Networ_Devices.Size = new System.Drawing.Size(85, 17);
            this.rBtn_Networ_Devices.TabIndex = 2;
            this.rBtn_Networ_Devices.TabStop = true;
            this.rBtn_Networ_Devices.Text = device.Description;
            this.rBtn_Networ_Devices.UseVisualStyleBackColor = true;
            this.rBtn_Networ_Devices.Visible = true;
            rBtn_Networ_Devices.CreateControl();
            rBtn_device_list.Add(rBtn_Networ_Devices);
            positioner += 17;
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            var selected = rBtn_device_list.Where(device => device.Checked.Equals(true));
            int index = int.Parse(selected.First().Name);
            PacketDevice selectedDevice = networkDevices[index - 1];
            capture_th = new Thread(new ParameterizedThreadStart(live_packet_capturer.start_capture));
            capture_th.Start(selectedDevice);

        }

        private void button2_Click(object sender, EventArgs e)
        {
            capture_th.Abort();
        }

        private void button3_Click(object sender, EventArgs e)
        {
           // string file_path;
            List<string> filePaths = new List<string>();
            DialogResult result;
            DialogResult moreFileToLoad; 
            while (true)
            {
                result = openFileDialog1.ShowDialog();
                if (result == DialogResult.OK)
                {
                    var file_path = openFileDialog1.FileName;
                    if (file_path is string)
                        filePaths.Add(file_path);
                }
                moreFileToLoad = MessageBox.Show("Otworzyć kolejny plik", "Important Question", MessageBoxButtons.YesNo);
                if (moreFileToLoad == DialogResult.No)
                    break;
            }
                FileManager tmp = new FileManager(filePaths);
                capture_th = new Thread(new ThreadStart(tmp.start_analizis));
                capture_th.Start();
                capture_th.Join();
                NetworMapper test = new NetworMapper(tmp.CapturedPackets);
                test.mapNetwork();
                dGVpackets.Rows.Clear();
                foreach (NetworkBoundModel packet in test.Bounds)
                {
                    dGVpackets.Rows.Add(packet.boundedNodes[0].ToString(), packet.boundedNodes[1].ToString(), packet.PacketCount);
                }

        }
    }
}
