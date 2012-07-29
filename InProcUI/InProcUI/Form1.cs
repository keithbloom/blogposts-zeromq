using System;
using System.Threading;
using System.Windows.Forms;

namespace InProcUI
{
    public partial class Form1 : Form
    {
        private bool _stopIt = false;
        private Thread _workerThread = null;
        private delegate void UpdateMessageDelegate(string messgae);

        private readonly UpdateMessageDelegate _updateMessageDelegate;

        public Form1()
        {
            InitializeComponent();
            _updateMessageDelegate = UpdateMessageBox;
        }

        private void bntGo_Click(object sender, EventArgs e)
        {
            LogMessage("Go clicked{0}",Environment.NewLine);
            _stopIt = false;
            _workerThread = new Thread(BigJob);
            _workerThread.Start();       
        }

        private void UpdateMessageBox(string message)
        {
            rtMessage.Text += message;
        }

        private void BigJob()
        {

            var i = 0;
            while (!_stopIt)
            {
                Invoke(_updateMessageDelegate, string.Format("{0}: Hello{1}",i,Environment.NewLine));
                Thread.Sleep(500);
                i++;
            }
            _workerThread.Abort();
                
            
        }

        private void LogMessage(string message, params object[] args)
        {
            rtLog.Text += string.Format(message, args);
        }

        private void btnStop_Click_1(object sender, EventArgs e)
        {
            LogMessage("Stop clicked{0}",Environment.NewLine);
            _stopIt = true;
        }
    }
}
