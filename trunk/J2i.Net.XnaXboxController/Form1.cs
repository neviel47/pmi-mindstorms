using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
//XNA Xbox360 Controller
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Input = Microsoft.Xna.Framework.Input; // to provide shorthand to clear up ambiguities
//MindSqualls
using NKH.MindSqualls;

namespace J2i.Net.XnaXboxController
{

    public partial class XnaInputForm : Form
    {
        //To keep track of the current and previous state of the gamepad
        /// <summary>
        /// The current state of the controller
        /// </summary>
        GamePadState gamePadState;
        /// <summary>
        /// The previous state of the controller
        /// </summary>
        GamePadState previousState;
        /// <summary>
        /// Keeps track of the current controller
        /// </summary>
        PlayerIndex playerIndex = PlayerIndex.One;
        /// <summary>
        /// Counter for limiting the time for which the vibration motors are on.
        /// </summary>
        int vibrationCountdown = 0;
        /// <summary>
        /// The NXT main brick
        /// </summary>
        private NxtBrick mainBrick;
        /// <summary>
        /// A motor pair to combine 2 motors
        /// </summary>
        private NxtMotorSync MotorPair;
        private NxtMotor MotorRight;
        private NxtMotor MotorLeft;
        private NxtMotor MotorClip;

        /// <summary>
        /// Power value from 1 to 10 (*10 for using)
        /// </summary>
        private int initialPower;

        /// <summary>
        /// 
        /// </summary>
        private uint clipMaxDegrees;

        /// <summary>
        /// 
        /// </summary>
        public XnaInputForm()
        {
            InitializeComponent();

        }

        #region NXT
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Connect(object sender, EventArgs e)
        {
            try
            {
                byte comPort = 0;
                byte.TryParse(ConfigurationManager.AppSettings["PortCom"].ToString(), out comPort);
                if (comPort != null || comPort == 0)
                {
                    //mainBrick = new NxtBrick(comPort);
                    mainBrick = new NxtBrick(NxtCommLinkType.USB, 5);
                    mainBrick.MotorA = new NxtMotor();
                    mainBrick.MotorB = new NxtMotor();
                    mainBrick.MotorC = new NxtMotor();
                    MotorRight = mainBrick.MotorB;
                    MotorLeft = mainBrick.MotorC;
                    MotorClip = mainBrick.MotorA;
                    MotorPair = new NxtMotorSync(MotorRight, MotorLeft);
                    mainBrick.Connect();
                    initialPower = 10;
                    clipMaxDegrees = 50;
                }
                else
                {
                    lblNotConnected.Text = "Please define a COM Port in app.config";
                }
            }
            catch (Exception ex)
            {
                //Disconnect();
                lblNotConnected.Text = "Connection Error ! Wrong COM Port or Any NXT!";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Disconnect()
        {

            Idle();

            if (mainBrick != null && mainBrick.IsConnected)
                mainBrick.Disconnect();

            mainBrick = null;
            MotorPair = null;
            MotorRight = null;
            MotorLeft = null;
            MotorClip = null;
            lblNotConnected.Text = "Disconnected";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Idle()
        {
            if (mainBrick != null && mainBrick.IsConnected)
            {
                MotorRight.Idle();
                MotorLeft.Idle();
                MotorClip.Idle();
            }
        }

        #endregion

        #region Actions

        /// <summary>
        /// To make the robot move forward
        /// </summary>
        private void Run()
        {
            if (MotorPair != null)
            {
                if (initialPower != null || initialPower != 0)
                {
                    MotorPair.Run((sbyte)initialPower, 0, 0);
                }
            }
        }


        private void Back()
        {
            if (MotorPair != null)
            {
                if (initialPower != null || initialPower != 0)
                {
                    MotorPair.Run((sbyte)-initialPower, 0, 0);
                }
            }
        }

        /// <summary>
        /// Turn on the right side
        /// </summary>
        private void TurnRight()
        {
            if (MotorPair != null)
            {
                if (initialPower != null || initialPower != 0)
                {
                    MotorRight.Run((sbyte)initialPower, 0);
                    MotorLeft.Run((sbyte)-initialPower, 0);
                }
            }
        }

        /// <summary>
        ///  Turn on the left side
        /// </summary>
        private void TurnLeft()
        {
            if (MotorPair != null)
            {
                if (initialPower != null || initialPower != 0)
                {
                    MotorLeft.Run((sbyte)initialPower, 0);
                    MotorRight.Run((sbyte)-initialPower, 0);
                }
            }
        }

        /// <summary>
        /// Stop movement motors
        /// </summary>
        private void Stop()
        {
            if (MotorPair != null)
            {
                MotorPair.Idle();
            }
        }

        /// <summary>
        /// Increase movement speed
        /// </summary>
        private void Turbo()
        {
            initialPower = initialPower + 10;
            if (initialPower > 100)
                initialPower = 100;
            if (MotorPair != null)
            {
                Run();
            }
        }

        /// <summary>
        /// Decrease movement speed
        /// </summary>
        private void Brake()
        {
            initialPower = initialPower - 10;
            if (initialPower < 0)
                initialPower = 0;
            if (MotorPair != null)
            {
                Run();
            }
        }

        /// <summary>
        /// Open the clip
        /// </summary>
        private void OpenClip()
        {
            if (MotorClip != null)
            {
                MotorClip.Run(40, clipMaxDegrees);
            }
        }

        /// <summary>
        /// Close the clip
        /// </summary>
        private void CloseClip()
        {
            if (MotorClip != null)
            {
                MotorClip.Run(-40, clipMaxDegrees);
            }
        }
        #endregion

        /// <summary>
        /// When a new controller is selected from the drop down
        /// update the player index and turn off all the vibration motors. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ddlController_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (this.ddlController.SelectedIndex)
            {
                case 0: playerIndex = PlayerIndex.One; break;
                case 1: playerIndex = PlayerIndex.Two; break;
                case 2: playerIndex = PlayerIndex.Three; break;
                case 3: playerIndex = PlayerIndex.Four; break;
                default: playerIndex = PlayerIndex.One; break;
            }
            this.StopAllVibration();

        }

        private void StopAllVibration()
        {
            GamePad.SetVibration(PlayerIndex.One, 0.0f, 0.0f);
            GamePad.SetVibration(PlayerIndex.Two, 0.0f, 0.0f);
            GamePad.SetVibration(PlayerIndex.Three, 0.0f, 0.0f);
            GamePad.SetVibration(PlayerIndex.Four, 0.0f, 0.0f);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GamePad.SetVibration(playerIndex, (float)this.leftMotor.Value, (float)this.rightMotor.Value);
            vibrationCountdown = 30;
        }

        private void CheckVibrationTimeout()
        {
            if (vibrationCountdown > 0)
            {
                --vibrationCountdown;
                if (vibrationCountdown == 0.0f)
                {
                    GamePad.SetVibration(playerIndex, 0.0f, 0.0f);
                }
            }
        }

        private void UpdateControllerState()
        {
            //Get the new gamepad state and save the old state.
            this.previousState = this.gamePadState;
            this.gamePadState = GamePad.GetState(this.playerIndex);
            //If the controller is not connected, let the user know
            this.lblNotConnected.Visible = !this.gamePadState.IsConnected;
            //I personally prefer to only update the buttons if their state has been changed. 
            if (!this.gamePadState.Buttons.Equals(this.previousState.Buttons))
            {
                this.buttonA.Checked = (this.gamePadState.Buttons.A == Input.ButtonState.Pressed);
                this.buttonB.Checked = (this.gamePadState.Buttons.B == Input.ButtonState.Pressed);
                this.buttonX.Checked = (this.gamePadState.Buttons.X == Input.ButtonState.Pressed);
                this.buttonY.Checked = (this.gamePadState.Buttons.Y == Input.ButtonState.Pressed);
                this.buttonLeftShoulder.Checked = (this.gamePadState.Buttons.LeftShoulder == Input.ButtonState.Pressed);
                this.buttonRightShoulder.Checked = (this.gamePadState.Buttons.RightShoulder == Input.ButtonState.Pressed);
                this.buttonStart.Checked = (this.gamePadState.Buttons.Start == Input.ButtonState.Pressed);
                this.buttonBack.Checked = (this.gamePadState.Buttons.Back == Input.ButtonState.Pressed);
                this.buttonLeftStick.Checked = (this.gamePadState.Buttons.LeftStick == Input.ButtonState.Pressed);
                this.buttonRightStick.Checked = (this.gamePadState.Buttons.RightStick == Input.ButtonState.Pressed);
            }
            if (!this.gamePadState.DPad.Equals(this.previousState.DPad))
            {
                this.buttonUp.Checked = (this.gamePadState.DPad.Up == Input.ButtonState.Pressed);
                this.buttonDown.Checked = (this.gamePadState.DPad.Down == Input.ButtonState.Pressed);
                this.buttonLeft.Checked = (this.gamePadState.DPad.Left == Input.ButtonState.Pressed);
                this.buttonRight.Checked = (this.gamePadState.DPad.Right == Input.ButtonState.Pressed);
            }

            //Update the position of the thumb sticks
            //since the thumbsticks can return a number between -1.0 and +1.0 I had to shift (add 1.0)
            //and scale (mutiplication by 100/2, or 50) to get the numbers to be in the range of 0-100
            //for the progress bar
            this.x1Position.Value = (int)((this.gamePadState.ThumbSticks.Left.X + 1.0f) * 100.0f / 2.0f);
            this.y1Position.Value = (int)((this.gamePadState.ThumbSticks.Left.Y + 1.0f) * 100.0f / 2.0f);
            this.x2position.Value = (int)((this.gamePadState.ThumbSticks.Right.X + 1.0f) * 100.0f / 2.0f);
            this.y2position.Value = (int)((this.gamePadState.ThumbSticks.Right.Y + 1.0f) * 100.0f / 2.0f);

            //The triggers return a value between 0.0 and 1.0.  I only needed to scale these values for
            //the progress bar
            this.leftTriggerPosition.Value = (int)((this.gamePadState.Triggers.Left * 100));
            this.rightTriggerPosition.Value = (int)(this.gamePadState.Triggers.Right * 100);
        }

        //I'm updating the controller display on a timed interval. 
        private void controllerTimer_Tick(object sender, EventArgs e)
        {
            this.CheckVibrationTimeout();
            this.UpdateControllerState();
        }

        private void XnaInputForm_Load(object sender, EventArgs e)
        {
            this.ddlController.SelectedIndex = 0;
            this.controllerTimer.Start();
        }

        private void XnaInputForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.StopAllVibration();
        }






        private void button6_Click(object sender, EventArgs e)
        {
            Run();
            label10.Text = initialPower.ToString();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Back();
            label10.Text = initialPower.ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            TurnRight();
            label10.Text = initialPower.ToString();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            TurnLeft();
            label10.Text = initialPower.ToString();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            OpenClip();
            label10.Text = initialPower.ToString();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            CloseClip();
            label10.Text = initialPower.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Turbo();
            label10.Text = initialPower.ToString();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Brake();
            label10.Text = initialPower.ToString();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            Disconnect();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            Idle();
        }



    }
}