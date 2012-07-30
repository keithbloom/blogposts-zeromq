using System;
using System.Threading;
using System.Windows.Forms;

namespace ZmqUI
{
    public partial class Form1 : Form
    {
        // Shared state
        private bool _stopIt = false;
        private Thread _workerThread = null;
        private delegate void UpdateMessageDelegate(string messgae);

        private readonly UpdateMessageDelegate _updateMessageDelegate;

        public Form1()
        {
            InitializeComponent();
            _updateMessageDelegate = UpdateMessageBox;
        }

        // App - starts worker thread
        private void btnGo_Click(object sender, EventArgs e)
        {
            LogMessage("Go clicked{0}", Environment.NewLine);
            _stopIt = false;
            _workerThread = new Thread(BigJob);
            _workerThread.Start();
        }


        // App - logging
        private void LogMessage(string message, params object[] args)
        {
            rtbOutput.Text += string.Format(message, args);
        }

        // App - stops worker thread
        private void btnStop_Click(object sender, EventArgs e)
        {
            LogMessage("Stop clicked{0}", Environment.NewLine);
            _stopIt = true;
        }

        // Worker process
        private void BigJob()
        {
            var i = 0;
            while (!_stopIt)
            {
                Invoke(_updateMessageDelegate, string.Format("{0}: Hello{1}", i, Environment.NewLine));
                Thread.Sleep(500);
                i++;
            }
            _workerThread.Abort();
        }

        // Called by worker thread
        private void UpdateMessageBox(string message)
        {
            rtbMessages.Text += message;
        }        
    }   

}


