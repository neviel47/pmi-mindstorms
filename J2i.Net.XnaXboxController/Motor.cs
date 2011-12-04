using System;
using System.Configuration;
//MindSqualls
using NKH.MindSqualls;

namespace MindstormsController
{
    public partial class XnaInputForm
    {
        #region NXT Attributes
        /// <summary>
        /// The NXT main brick
        /// </summary>
        private NxtBrick mainBrick;
        /// <summary>
        /// A motor pair to combine 2 motors
        /// </summary>
        private NxtMotorSync motorPair;
        /// <summary>
        /// The motor on the right side
        /// </summary>
        private NxtMotor motorRight;
        /// <summary>
        /// The motor on the left side
        /// </summary>
        private NxtMotor motorLeft;
        /// <summary>
        /// The motor on the front (Clip)
        /// </summary>
        private NxtMotor motorClip;
        /// <summary>
        /// The touch sensor on the right
        /// </summary>
        private NxtTouchSensor touchSensorRight = null;
        /// <summary>
        /// The touch sensor on the left
        /// </summary>
        private NxtTouchSensor touchSensorLeft = null;
        /// <summary>
        /// The color Sensor
        /// </summary>
        private Nxt2ColorSensor colorSensor = null;
        /// <summary>
        /// The UltraSonic Sensor
        /// </summary>
        private NxtUltrasonicSensor ultraSonicSensor = null;
        /// <summary>
        /// Power value from 1 to 10 (*10 for using)
        /// </summary>
        private int _initialPower = 0;
        public int initialPower
        {
            get
            {
                if (_initialPower <= 0) return 10;
                else if (_initialPower >= 100) return 100;
                else return _initialPower;
            }
            set { _initialPower = value; updatePowerBar(); }
        }
        /// <summary>
        /// Max degree for opening the clip
        /// </summary>
        private uint clipMaxDegrees = 40;
        /// <summary>
        /// When both motor run, the Turn Ratio is the rotation side power divisor
        /// </summary>
        private int turnRatio = 4;
        /// <summary>
        /// The name of the last used movement method
        /// </summary>
        private string lastUsedMethod = string.Empty;

        #endregion

        #region NXT

        /// <summary>
        /// Connect the robot and initialize motors
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Connect(object sender, EventArgs e)
        {
            try
            {
                byte comPort = 0;
                byte.TryParse(ConfigurationManager.AppSettings["PortCom"].ToString(), out comPort);

                //MOTOR
                mainBrick = new NxtBrick(NxtCommLinkType.USB, 5);
                mainBrick.MotorA = new NxtMotor();
                mainBrick.MotorB = new NxtMotor();
                mainBrick.MotorC = new NxtMotor();
                motorRight = mainBrick.MotorB;
                motorLeft = mainBrick.MotorC;
                motorClip = mainBrick.MotorA;
                motorPair = new NxtMotorSync(motorRight, motorLeft);

                //TOUCH SENSOR
                touchSensorRight = new NxtTouchSensor();
                mainBrick.Sensor1 = touchSensorRight;
                touchSensorLeft = new NxtTouchSensor();
                mainBrick.Sensor4 = touchSensorLeft;

                //COLOR SENSOR
                colorSensor = new Nxt2ColorSensor();
                mainBrick.Sensor2 = colorSensor;

                if (comPort != 0)
                {
                    //CONNECTION
                    mainBrick.Connect();
                    if (mainBrick.IsConnected)
                    {
                        mainBrick.PlaySoundfile("Hello.rso");
                        lblNotConnected.Text = "Connected";
                        updatePowerBar();

                        // SENSOR EVENTS
                        touchSensorRight.OnPressed += new NxtSensorEvent(TouchedOnRight);
                        touchSensorRight.PollInterval = 100;
                        touchSensorLeft.OnPressed += new NxtSensorEvent(TouchedOnLeft);
                        touchSensorLeft.PollInterval = 100;
                        colorSensor.SetColorDetectorMode();
                        colorSensor.PollInterval = 100;

                        cb = new System.Threading.TimerCallback(Tick);
                        //touchSensor.OnReleased += new NxtSensorEvent(Stop);
                        // colorSensor.OnInsideRange += new NxtSensorEvent(IdentifyColor);
                        /*
                        // Start the MotorControl program on the NXT and wait intill it is running.
                        MotorControlProxy.StartMotorControl(mainBrick.CommLink);
                        System.Threading.Thread.Sleep(500);

                        // Run the motor in port B with fill power forward for 3600 degrees.
                        MotorControlProxy.CONTROLLED_MOTORCMD(mainBrick.CommLink, MotorControlMotorPort.PortsBC, "100", "002000", '4');
                        McNxtBrick nx = new McNxtBrick(NxtCommLinkType.USB, comPort);
                         */
                    }
                    else
                    {
                        lblNotConnected.Text = "Please define a COM Port in app.config or Turn on the robot!";
                    }
                }
                else
                {
                    lblNotConnected.Text = "Please define a COM Port in app.config";
                }
            }
            catch (Exception ex)
            {
                lblNotConnected.Text = "Connection Error ! " + ex.Message;
            }
        }

        /// <summary>
        /// Disconnect the robot
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Disconnect(object sender, EventArgs e)
        {
            Idle(sender, e);

            if (mainBrick != null && mainBrick.IsConnected)
            {
                mainBrick.PlaySoundfile("Have A Nice Day.rso");
                motorPair = null;
                motorRight = null;
                motorLeft = null;
                motorClip = null;
                colorSensor = null;
                touchSensorLeft = null;
                touchSensorRight = null;
                mainBrick = null;
                lblNotConnected.Text = "Disconnected";
            }
        }

        /// <summary>
        /// Idle all motors
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Idle(object sender, EventArgs e)
        {
            if (mainBrick != null && mainBrick.IsConnected)
            {
                motorRight.Idle();
                motorLeft.Idle();
                motorClip.Idle();
            }
        }

        #endregion

        #region Mouvements Actions

        /// <summary>
        /// To make the robot move forward
        /// </summary>
        public void Run(int power, uint limit)
        {
            if (motorPair != null)
            {
                if (power != 0)
                {
                    if (lastUsedMethod != null && !lastUsedMethod.Equals("Run"))
                        motorPair.Brake();
                    motorLeft.Run((sbyte)initialPower, limit);
                    motorRight.Run((sbyte)initialPower, limit);
                    //MotorPair.Run((sbyte)initialPower, 0, 0);
                }
            }
            UpdateLabel("Run");
        }

        /// <summary>
        /// To make the robot move back
        /// </summary>
        public void Back(int power, uint limit)
        {
            if (motorPair != null)
            {
                if (power != 0)
                {
                    if (lastUsedMethod != null && !lastUsedMethod.Equals("Back"))
                        motorPair.Brake();
                    //MotorPair.Run((sbyte)-initialPower, 0, 0);
                    motorLeft.Run((sbyte)-initialPower, limit);
                    motorRight.Run((sbyte)-initialPower, limit);
                }
            }
            UpdateLabel("Back");
        }

        /// <summary>
        /// Move Forward and turn right too !
        /// </summary>
        public void RunRight(int power, uint limit)
        {
            if (motorPair != null)
            {
                if (power != 0)
                {
                    if (lastUsedMethod != null && !lastUsedMethod.Equals("RunRight"))
                        motorPair.Brake();
                    motorRight.Run((sbyte)Math.Round((double)power / turnRatio), limit);
                    motorLeft.Run((sbyte)power, limit);
                }
            }
            UpdateLabel("RunRight");
        }


        /// <summary>
        /// Move back and turn right too !
        /// </summary>
        public void BackRight(int power, uint limit)
        {
            if (motorPair != null)
            {
                if (power != 0)
                {
                    if (lastUsedMethod != null && !lastUsedMethod.Equals("BackRight"))
                        motorPair.Brake();
                    motorRight.Run((sbyte)-(Math.Round((double)power / turnRatio)), limit);
                    motorLeft.Run((sbyte)-power, limit);
                }
            }
            UpdateLabel("BackRight");
        }

        /// <summary>
        /// Move back and turn left too !
        /// </summary>
        public void BackLeft(int power, uint limit)
        {
            if (motorPair != null)
            {
                if (power != 0)
                {
                    if (lastUsedMethod != null && !lastUsedMethod.Equals("BackLeft"))
                        motorPair.Brake();
                    motorLeft.Run((sbyte)-(Math.Round((double)power / turnRatio)), limit);
                    motorRight.Run((sbyte)-power, limit);
                }
            }
            UpdateLabel("BackLeft");
        }

        /// <summary>
        /// Turn on the right side
        /// </summary>
        public void TurnRight()
        {
            if (motorPair != null)
            {
                if (initialPower != 0)
                {
                    if (lastUsedMethod != null && !lastUsedMethod.Equals("TurnRight"))
                        motorPair.Brake();
                    motorRight.Run((sbyte)-initialPower, 0);
                    motorLeft.Run((sbyte)initialPower, 0);
                }
            }
            UpdateLabel("TurnRight");

        }

        /// <summary>
        /// Move Forward and turn left too !
        /// </summary>
        public void RunLeft(int power, uint limit)
        {
            if (motorPair != null)
            {
                if (power != 0)
                {
                    if (lastUsedMethod != null && !lastUsedMethod.Equals("RunLeft"))
                        motorPair.Brake();
                    motorLeft.Run((sbyte)Math.Round((double)power / turnRatio), limit);
                    motorRight.Run((sbyte)power, limit);
                }
            }
            UpdateLabel("RunLeft");
        }

        /// <summary>
        ///  Turn on the left side
        /// </summary>
        public void TurnLeft()
        {
            if (motorPair != null)
            {
                if (initialPower != 0)
                {
                    if (lastUsedMethod != null && !lastUsedMethod.Equals("TurnLeft"))
                        motorPair.Brake();
                    motorLeft.Run((sbyte)-initialPower, 0);
                    motorRight.Run((sbyte)initialPower, 0);
                }
            }
            UpdateLabel("TurnLeft");
        }

        /// <summary>
        /// Stop movement motors
        /// </summary>
        /// 
        private void Stop()
        {
            lblNotConnected.Text = "Stopping.";

            if (motorPair == null)
            {
                motorLeft.Brake();
                motorRight.Brake();
            }
            else
            {
                motorPair.Brake();
                motorPair.ResetMotorPosition(true);
            }
            otherColorFounded = false;
            firstRun = false;
            colorSensor.OnPolled -= new Polled(IdentifyColorEvent);
        }

        /// <summary>
        /// Increase movement speed
        /// </summary>
        private void Turbo()
        {
            initialPower = initialPower + 10;
            if (initialPower > 100)
                initialPower = 100;
            updatePowerBar();
            if (motorPair != null)
            {
                InvokeMethod(lastUsedMethod);
            }
        }

        /// <summary>
        /// Decrease movement speed
        /// </summary>
        private void Brake()
        {
            initialPower = initialPower - 10;
            if (initialPower < 10)
                initialPower = 10;
            updatePowerBar();
            if (motorPair != null)
            {
                InvokeMethod(lastUsedMethod);
            }
        }

        private void updatePowerBar()
        {
            pbPower.Value = initialPower;
        }


        #endregion

        #region Clip Actions
        /// <summary>
        /// Open the clip
        /// </summary>
        private void OpenClip()
        {
            if (motorClip != null)
            {
                motorClip.Run(40, clipMaxDegrees);
            }
        }

        /// <summary>
        /// Close the clip
        /// </summary>
        private void CloseClip()
        {
            if (motorClip != null)
            {
                motorClip.Run(-40, clipMaxDegrees);
            }
        }
        #endregion

        #region Mouvements Events

        private void btnRun_Click(object sender, EventArgs e)
        {
            if (cb != null)
                timer = new System.Threading.Timer(cb, null, 0, 1000);
            Run(initialPower, 0);
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            Back(initialPower, 0);
        }

        private void btnTurnRight_Click(object sender, EventArgs e)
        {
            TurnRight();
        }

        private void btnTurnLeft_Click(object sender, EventArgs e)
        {
            TurnLeft();
        }

        private void btnTurbo_Click(object sender, EventArgs e)
        {
            Turbo();
        }

        private void btnBrake_Click(object sender, EventArgs e)
        {
            Brake();
        }

        private void btnRunRight_Click(object sender, EventArgs e)
        {
            RunRight(initialPower, 0);
        }

        private void btnRunLeft_Click(object sender, EventArgs e)
        {
            RunLeft(initialPower, 0);
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            Stop();
        }

        private void btnBackLeft_Click(object sender, EventArgs e)
        {
            BackLeft(-initialPower, 0);
        }

        private void btnBackRight_Click(object sender, EventArgs e)
        {
            BackRight(-initialPower, 0);
        }

        #endregion



        #region Clip Events
        private void btnOpenClip_Click(object sender, EventArgs e)
        {
            OpenClip();
        }

        private void btnCloseClip_Click(object sender, EventArgs e)
        {
            CloseClip();
        }
        #endregion

    }
}
