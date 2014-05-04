using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace snifer
{
    class FileManager
    {
        List<string> filePaths { get; set; }
        List<CapturedPacketModel> capturedPackets;

        internal List<CapturedPacketModel> CapturedPackets
        {
            get { return capturedPackets; }
            set { capturedPackets = value; }
        }
        List<Thread> workingThreads;
        public FileManager(List<string> filePaths)
        {
            workingThreads = new List<Thread>();
            this.filePaths  = filePaths;
            capturedPackets = new List<CapturedPacketModel>();
        }

        public void start_analizis()
        {
            Thread workThread;
            FlieAnalizer pcapFile;
            List<FlieAnalizer> analizedFiles = new List<FlieAnalizer>();
            foreach (string path in filePaths)
            {
                pcapFile = new FlieAnalizer();
                workThread = new Thread(new ParameterizedThreadStart(pcapFile.start_capture));
                workingThreads.Add(workThread);
                workThread.Start(path);
                analizedFiles.Add(pcapFile);

            }

            foreach (Thread th in workingThreads)
            {
                th.Join();
            }

            foreach (FlieAnalizer analizedFile in analizedFiles)
            {
                capturedPackets.AddRange(analizedFile.CapturedPackets);
            }
        }
    }
}
