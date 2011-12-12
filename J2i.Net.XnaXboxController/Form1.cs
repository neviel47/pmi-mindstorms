using System;
using System.Reflection;
using System.Windows.Forms;
using NKH.MindSqualls;



namespace MindstormsController
{

    public partial class XnaInputForm : Form
    {

        /// <summary>
        /// Form Constructor
        /// </summary>
        public XnaInputForm()
        {
            InitializeComponent();
        }

        bool otherColorFounded = false;
        bool firstRun = false;
        System.Threading.Timer timer;
        System.Threading.TimerCallback cb;
        public delegate void Del();
        int timerCount = 0;

        private void UpdateLabel(string lastUsed)
        {
            this.Invoke((MethodInvoker)delegate
            {
                lblInfo.Text = lastUsedMethod = lastUsed;
            });
        }

        private void UpdateColorLabel(string color)
        {
            this.Invoke((MethodInvoker)delegate
            {
                lblColor.Text = color;
            });
        }
        private void Tick(object o)
        {
            timerCount++;
            this.Invoke((MethodInvoker)delegate
            {
                lblTick.Text = timerCount.ToString(); // runs on UI thread
            });
        }
        private void followBlack_Click(object sender, EventArgs e)
        {
            colorSensor.OnPolled += new Polled(IdentifyColorEvent);
        }
        private void btnLight_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Invoke a method by name
        /// </summary>
        /// <param name="methodName">The method name</param>
        private void InvokeMethod(string methodName)
        {
            if (!string.IsNullOrEmpty(methodName))
            {
                // Get the desired method by name: DisplayName
                MethodInfo methodInfo =
                   typeof(XnaInputForm).GetMethod(methodName);
                try
                {
                    object[] param = null;
                    if (lastUsedMethod.Equals("Run") || lastUsedMethod.Equals("Back"))
                    {
                        param = new object[] { initialPower, (uint)0 };
                    }
                    methodInfo.Invoke(this, param);
                }
                catch (Exception e)
                {
                    lblInfo.Text = e.Message;
                }
            }
        }

        /// <summary>
        /// Action to do when on the form closing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void XnaInputForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.StopAllVibration();
        }

        private void Stop(object sender, MouseEventArgs e)
        {
            Stop();
        }
    }
}