using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;


namespace TestRobot
{
    public partial class Form1 : Form
    {
        private NXTControl ControlRobot;

        public Form1()
        {
            InitializeComponent();
            ControlRobot = new NXTControl();
            this.ActiveControl = null;
            this.KeyDown += new KeyEventHandler(Form1_KeyDown);

        }

        private void bt_connect_Click(object sender, EventArgs e)
        {
            string returnCon = ControlRobot.ConnectRobot();
            lb_con.Text = returnCon;
        }

        private void bt_disconnect_Click(object sender, EventArgs e)
        {
            string returnDisCon = ControlRobot.DisconnectRobot();
            lb_con.Text = returnDisCon;
        }

        private void bt_gofront_Click(object sender, EventArgs e)
        {
            lb_state.Text = "Move Front";
            ControlRobot.GoFront(75, 360);
            lb_state.Text = "Stop";
        }

        private void bt_goBack_Click(object sender, EventArgs e)
        {
            lb_state.Text = "Move Back";
            ControlRobot.GoBack(75, 360);
            lb_state.Text = "Stop";
        }

        private void bt_goRight_Click(object sender, EventArgs e)
        {
            lb_state.Text = "Move Right";
            ControlRobot.GoRight(75, 360);
            lb_state.Text = "Stop";
        }

        private void bt_goLeft_Click(object sender, EventArgs e)
        {
            lb_state.Text = "Move Left";
            ControlRobot.GoLeft(75, 360);
            lb_state.Text = "Stop";
        }

        private void bt_colorDectect_Click(object sender, EventArgs e)
        {
            lb_colorDetected.Text = ControlRobot.DectectColor();
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            //Arrêt du robot
            ControlRobot.StopRobot();
            lb_state.Text = "Stop";
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {

            switch (e.KeyCode)
            {
                case Keys.Z:
                    lb_state.Text = "Move Front";
                    ControlRobot.GoFrontWhileStop();
                    break;
                case Keys.S:
                    lb_state.Text = "Move Back";
                    ControlRobot.GoBack(75, 360);
                    break;
                case Keys.Q:
                    lb_state.Text = "Move Right";
                    ControlRobot.GoRight(75, 360);
                    break;
                case Keys.D:
                    lb_state.Text = "Move Left";
                    ControlRobot.GoLeft(75, 360);
                    break;
                case Keys.E:
                    lb_state.Text = ControlRobot.OpenPliers();
                    break;
                case Keys.R:
                    lb_state.Text = ControlRobot.ClosePliers();
                    break;
                case Keys.Space:
                    ControlRobot.ModeDectection();
                    //lb_colorDetected.Text = ControlRobot.GetColorChecked();
                    break;
                case Keys.Enter:
                    lb_colorDetected.Text = ControlRobot.DectectColor();
                    break;

            }
        }

        private void CheckColor()
        {
            lb_colorDetected.Text = ControlRobot.GetColorChecked();
        }
    }
}
