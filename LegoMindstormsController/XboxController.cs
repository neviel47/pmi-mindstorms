using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Input = Microsoft.Xna.Framework.Input;
using System.Windows.Threading;

namespace LegoMindstormsController
{
    public partial class MainWindow
    {
        #region XNA Xbox Controller attributes
        /// <summary>
        /// To keep track of the current and previous state of the gamepad
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
        
        #endregion

        #region XNA Xbox Controller

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

        /// <summary>
        /// Stop all Xobx Controller Vibration
        /// </summary>
        private void StopAllVibration()
        {
            GamePad.SetVibration(PlayerIndex.One, 0.0f, 0.0f);
            GamePad.SetVibration(PlayerIndex.Two, 0.0f, 0.0f);
            GamePad.SetVibration(PlayerIndex.Three, 0.0f, 0.0f);
            GamePad.SetVibration(PlayerIndex.Four, 0.0f, 0.0f);
        }

        /// <summary>
        /// Make controller vibrate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnVibrate_Click(object sender, EventArgs e)
        {
            GamePad.SetVibration(playerIndex, (float)this.leftMotor.Value, (float)this.rightMotor.Value);
            vibrationCountdown = 30;
        }

        /// <summary>
        /// Check the vibration timeout
        /// </summary>
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

        /// <summary>
        /// Update the ControllerState (Timer)
        /// </summary>
        private void UpdateControllerState()
        {
            //Get the new gamepad state and save the old state.
            this.previousState = this.gamePadState;
            this.gamePadState = GamePad.GetState(this.playerIndex);
            //If the controller is not connected, let the user know
            this.lblInfo.IsEnabled = this.gamePadState.IsConnected;
            //I personally prefer to only update the buttons if their state has been changed. 
            if (!this.gamePadState.Buttons.Equals(this.previousState.Buttons))
            {
                this.btnA.IsChecked = (this.gamePadState.Buttons.A == Input.ButtonState.Pressed);
                if (this.gamePadState.Buttons.A == Input.ButtonState.Pressed)
                {
                    nc.CloseClip();
                }
                this.btnB.IsChecked = (this.gamePadState.Buttons.B == Input.ButtonState.Pressed);
                if (this.gamePadState.Buttons.B == Input.ButtonState.Pressed)
                {
                    nc.OpenClip();
                }
                this.btnX.IsChecked = (this.gamePadState.Buttons.X == Input.ButtonState.Pressed);
                this.btnY.IsChecked = (this.gamePadState.Buttons.Y == Input.ButtonState.Pressed);
                this.btnLeftShoulder.IsChecked = (this.gamePadState.Buttons.LeftShoulder == Input.ButtonState.Pressed);
                this.btnRightShoulder.IsChecked = (this.gamePadState.Buttons.RightShoulder == Input.ButtonState.Pressed);
                this.btnStart.IsChecked = (this.gamePadState.Buttons.Start == Input.ButtonState.Pressed);
                this.btnBack.IsChecked = (this.gamePadState.Buttons.Back == Input.ButtonState.Pressed);
                this.btnLeftStick.IsChecked = (this.gamePadState.Buttons.LeftStick == Input.ButtonState.Pressed);
                this.btnRightStick.IsChecked = (this.gamePadState.Buttons.RightStick == Input.ButtonState.Pressed);
            }
            if (!this.gamePadState.DPad.Equals(this.previousState.DPad))
            {
                this.btnDPadUp.IsChecked = (this.gamePadState.DPad.Up == Input.ButtonState.Pressed);
                this.btnDPadDown.IsChecked = (this.gamePadState.DPad.Down == Input.ButtonState.Pressed);
                this.btnDPadLeft.IsChecked = (this.gamePadState.DPad.Left == Input.ButtonState.Pressed);
                this.btnDPadRight.IsChecked = (this.gamePadState.DPad.Right == Input.ButtonState.Pressed);
            }

            //Update the position of the thumb sticks
            //since the thumbsticks can return a number between -1.0 and +1.0 I had to shift (add 1.0)
            //and scale (mutiplication by 100/2, or 50) to get the numbers to be in the range of 0-100
            //for the progress bar
            this.x1Position.Value = (int)((this.gamePadState.ThumbSticks.Left.X + 1.0f) * 100.0f / 2.0f);
            this.y1Position.Value = (int)((this.gamePadState.ThumbSticks.Left.Y + 1.0f) * 100.0f / 2.0f);

            #region Control Movement Left Thumbstick

            float valueY = this.gamePadState.ThumbSticks.Left.Y;
            float valueX = this.gamePadState.ThumbSticks.Left.X;
            if (valueY > 0)
            {
                if (valueX > 0)
                {
                    if (valueX > valueY)
                    {
                        nc.TurnRight((int)(valueX * 100), 0);
                    }
                    else
                    {
                        nc.RunRight((int)(valueY * 100), 0);
                    }
                }
                else if (valueX < 0)
                {
                    if (-valueX > valueY)
                    {
                        nc.TurnLeft((int)(valueX * 100), 0);
                    }
                    else
                    {
                        nc.RunLeft((int)(valueY * 100), 0);
                    }
                }
                else if (valueX == 0)
                {
                    nc.Run((int)(valueY * 100), 0);
                }
            }
            else if (valueY < 0)
            {
                if (valueX > 0)
                {
                    if (valueX > -valueY)
                    {
                        nc.TurnRight((int)(valueX * 100), 0);
                    }
                    else
                    {
                        nc.BackRight((int)(valueY * 100), 0);
                    }
                }
                else if (valueX < 0)
                {
                    if (-valueX > -valueY)
                    {
                        nc.TurnLeft((int)(valueX * 100), 0);
                    }
                    else
                    {
                        nc.BackLeft((int)(valueY * 100), 0);
                    }
                }
                else if (valueX == 0)
                {
                    nc.Run((int)(valueY * 100), 0);
                }
            }
            else if (valueX > 0)
            {
                nc.TurnRight((int)(valueX * 100), 0);
            }
            else if (valueX < 0)
            {
                nc.TurnLeft((int)(valueX * 100), 0);
            }
            else if ((valueY == 0) && (valueX == 0))
            {
                nc.Stop();
            }
            #endregion

            this.x2Position.Value = (int)((this.gamePadState.ThumbSticks.Right.X + 1.0f) * 100.0f / 2.0f);
            this.y2Position.Value = (int)((this.gamePadState.ThumbSticks.Right.Y + 1.0f) * 100.0f / 2.0f);

            //The triggers return a value between 0.0 and 1.0.  I only needed to scale these values for
            //the progress bar
            this.leftTriggerPosition.Value = (int)((this.gamePadState.Triggers.Left * 100));
            this.rightTriggerPosition.Value = (int)(this.gamePadState.Triggers.Right * 100);
        }

        /// <summary>
        /// I'm updating the controller display on a timed interval. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        private void controllerTimer_Tick(object sender, EventArgs e)
        {
            this.CheckVibrationTimeout();
            this.UpdateControllerState();
        }

        /// <summary>
        /// Form Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void XnaInputForm_Load(object sender, EventArgs e)
        {
            this.ddlController.SelectedIndex = 0;
            this.controllerTimer.Start();
        }


        #endregion
    }
}
