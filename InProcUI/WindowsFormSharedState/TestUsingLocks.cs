using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace WindowsFormSharedState
{
    public class TestUsingLocks : Form
    {
        delegate void StringParameterDelegate(string value);

        readonly Label statusIndicator;
        readonly Label counter;
        readonly Button button;

        /// <summary>
        /// Lock around target and currentCount
        /// </summary>
        readonly object stateLock = new object();
        int target;
        int currentCount;

        readonly Random rng = new Random();

        public TestUsingLocks()
        {
            Size = new Size(180, 120);
            Text = "Test";

            var lbl = new Label
                {
                    Text = "Status:", Size = new Size(50, 20), Location = new Point(10, 10)
                };
            Controls.Add(lbl);

            lbl = new Label {Text = "Count:", Size = new Size(50, 20), Location = new Point(10, 34)};
            Controls.Add(lbl);

            statusIndicator = new Label {Size = new Size(100, 20), Location = new Point(70, 10)};
            Controls.Add(statusIndicator);

            counter = new Label {Size = new Size(100, 20), Location = new Point(70, 34)};
            Controls.Add(counter);

            button = new Button {Text = "Go", Size = new Size(50, 20), Location = new Point(10, 58)};
            Controls.Add(button);
            button.Click += StartThread;
        }

        public override sealed string Text
        {
            get { return base.Text; }
            set { base.Text = value; }
        }

        void StartThread(object sender, EventArgs e)
        {
            button.Enabled = false;
            lock (stateLock)
            {
                target = rng.Next(100);
            }
            var t = new Thread(ThreadJob)
                {
                    IsBackground = true
                };
            t.Start();
        }

        void ThreadJob()
        {
            var updateCounterDelegate = new MethodInvoker(UpdateCount);
            int localTarget;
            lock (stateLock)
            {
                localTarget = target;
            }
            UpdateStatus("Starting");

            lock (stateLock)
            {
                currentCount = 0;
            }
            Invoke(updateCounterDelegate);
            // Pause before starting
            Thread.Sleep(500);
            UpdateStatus("Counting");
            for (int i = 0; i < localTarget; i++)
            {
                lock (stateLock)
                {
                    currentCount = i;
                }
                // Synchronously show the counter
                Invoke(updateCounterDelegate);
                Thread.Sleep(100);
            }
            UpdateStatus("Finished");
            Invoke(new MethodInvoker(EnableButton));
        }

        void UpdateStatus(string value)
        {
            if (InvokeRequired)
            {
                // We're not in the UI thread, so we need to call BeginInvoke
                BeginInvoke(new StringParameterDelegate(UpdateStatus), new object[] { value });
                return;
            }
            // Must be on the UI thread if we've got this far
            statusIndicator.Text = value;
        }

        void UpdateCount()
        {
            int tmpCount;
            lock (stateLock)
            {
                tmpCount = currentCount;
            }
            counter.Text = tmpCount.ToString();
        }

        void EnableButton()
        {
            button.Enabled = true;
        }
    }
}