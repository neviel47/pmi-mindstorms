using System; // to provide shorthand to clear up ambiguities
//XNA Xbox360 Controller
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Input = Microsoft.Xna.Framework.Input;
namespace MindstormsController
{
    public partial class XnaInputForm
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
            this.lblNotConnected.Visible = !this.gamePadState.IsConnected;
            //I personally prefer to only update the buttons if their state has been changed. 
            if (!this.gamePadState.Buttons.Equals(this.previousState.Buttons))
            {
                this.buttonA.Checked = (this.gamePadState.Buttons.A == Input.ButtonState.Pressed);
                if (this.gamePadState.Buttons.A == Input.ButtonState.Pressed)
                {
                    CloseClip();
                }
                this.buttonB.Checked = (this.gamePadState.Buttons.B == Input.ButtonState.Pressed);
                if (this.gamePadState.Buttons.B == Input.ButtonState.Pressed)
                {
                    OpenClip();
                }
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

            #region Control Movement Left Thumbstick

            float valueY = this.gamePadState.ThumbSticks.Left.Y;
            float valueX = this.gamePadState.ThumbSticks.Left.X;
            if (this.gamePadState.ThumbSticks.Left.Y > 0)
            {
                if (valueX > 0)
                {
                    if (valueX > valueY)
                    {
                        TurnRight();
                    }
                    else
                    {
                        RunRight((int)(valueY * 10), 0);
                    }
                }
                else if (valueX < 0)
                {
                    if (-valueX > valueY)
                    {
                        TurnLeft();
                    }
                    else
                    {
                        RunLeft((int)(valueY * 10), 0);
                    }
                }
                else if (valueX == 0)
                {
                    Run((int)(valueY * 10), 0);
                }
            }
            else if (this.gamePadState.ThumbSticks.Left.Y < 0)
            {
                if (valueX > 0)
                {
                    if (valueX > valueY)
                    {
                        TurnRight();
                    }
                    else
                    {
                        BackRight((int)(valueY * 10), 0);
                    }
                }
                else if (valueX < 0)
                {
                    if (-valueX > valueY)
                    {
                        TurnLeft();
                    }
                    else
                    {
                        BackLeft((int)(valueY * 10), 0);
                    }
                }
                else if (valueX == 0)
                {
                    Back((int)(valueY * 10), 0);
                }
            }
            else if (this.gamePadState.ThumbSticks.Left.Y == 0)
            {
                Stop();
            }
            #endregion

            this.x2position.Value = (int)((this.gamePadState.ThumbSticks.Right.X + 1.0f) * 100.0f / 2.0f);
            this.y2position.Value = (int)((this.gamePadState.ThumbSticks.Right.Y + 1.0f) * 100.0f / 2.0f);

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
