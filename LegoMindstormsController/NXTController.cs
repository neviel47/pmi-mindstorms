using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NKH.MindSqualls;
using System.Configuration;
using System.Reflection;
namespace LegoMindstormsController
{
    public class NXTController
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
            set { _initialPower = value; }
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
        /// Ultra sonic distance on connection
        /// </summary>
        byte? defaultDistance = null;
        bool otherColorFounded = false;
        bool firstRun = false;
        #endregion

        #region NXT Basics

        /// <summary>
        /// Connect the robot and initialize motors
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public string Connect()
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

                ConnectCaptor();

                if (comPort != 0)
                {
                    //CONNECTION
                    mainBrick.Connect();
                    if (mainBrick.IsConnected)
                    {
                        mainBrick.PlaySoundfile("Hello.rso");
                        addCaptorEvents();

                        //cb = new System.Threading.TimerCallback(Tick);
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
                        return "Connected";
                    }
                    else
                    {
                        return "Please define a COM Port in app.config or Turn on the robot!";
                    }
                }
                else
                {
                    return "Please define a COM Port in app.config";
                }
            }
            catch (Exception ex)
            {
                return "Connection Error ! " + ex.Message;
            }
        }
        /// <summary>
        /// Disconnect the robot
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private string Disconnect()
        {
            Idle();

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
            }
            return "Disconnected";
        }
        /// <summary>
        /// Idle all motors
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Idle()
        {
            if (mainBrick != null && mainBrick.IsConnected)
            {
                motorRight.Idle();
                motorLeft.Idle();
                motorClip.Idle();
            }
        }

        private void ConnectCaptor()
        {
            //TOUCH SENSOR
            touchSensorRight = new NxtTouchSensor();
            mainBrick.Sensor1 = touchSensorRight;
            touchSensorLeft = new NxtTouchSensor();
            mainBrick.Sensor4 = touchSensorLeft;

            //COLOR SENSOR
            colorSensor = new Nxt2ColorSensor();
            mainBrick.Sensor2 = colorSensor;

            //ULTRASONIC SENSOR
            ultraSonicSensor = new NxtUltrasonicSensor();
            mainBrick.Sensor3 = ultraSonicSensor;

        }

        private void addCaptorEvents()
        {
            // SENSOR EVENTS
            touchSensorRight.OnPressed += new NxtSensorEvent(TouchedOnRight);
            touchSensorRight.PollInterval = 10;
            touchSensorLeft.OnPressed += new NxtSensorEvent(TouchedOnLeft);
            touchSensorLeft.PollInterval = 10;

            colorSensor.SetColorDetectorMode();
            colorSensor.PollInterval = 100;
            colorSensor.OnPolled += new Polled(IdentifyColorEvent);

            ultraSonicSensor.PollInterval = 10;
            ultraSonicSensor.OnPolled += new Polled(OnDistance);

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
                   typeof(NXTController).GetMethod(methodName);
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
                    Console.WriteLine(e.Message);
                }
            }
        }
        #endregion

        #region Mouvements Actions

        /// <summary>
        /// To make the robot move forward, positive power or negative power (to go back)
        /// </summary>
        public void Run(int power, uint limit)
        {
            if (motorPair != null)
            {
                if (power != 0)
                {
                    if (lastUsedMethod != null && (!lastUsedMethod.Equals("Run") && !lastUsedMethod.Equals("Back")))
                        motorPair.Brake();
                    motorLeft.Run((sbyte)power, limit);
                    motorRight.Run((sbyte)power, limit);
                    if (power > 0)
                    {
                        lastUsedMethod = "Run";
                        Console.WriteLine("Run");
                    }
                    else
                    {
                        lastUsedMethod = "Back";
                        Console.WriteLine("Back");
                    }
                }
            }
        }
        /// <summary>
        /// Move Forward and turn right too, power can be + or -, it's change nothing
        /// </summary>
        public void RunRight(int power, uint limit)
        {
            if (motorPair != null)
            {
                if (power != 0)
                {
                    if (power < 0)
                        power = +power;
                    if (lastUsedMethod != null && !lastUsedMethod.Equals("RunRight"))
                        motorPair.Brake();
                    motorRight.Run((sbyte)Math.Round((double)power / turnRatio), limit);
                    motorLeft.Run((sbyte)power, limit);
                }
            }
            lastUsedMethod = "RunRight";
            Console.WriteLine("RunRight");
        }
        /// <summary>
        /// Move back and turn right too, power can be + or -, it's change nothing
        /// </summary>
        public void BackRight(int power, uint limit)
        {
            if (motorPair != null)
            {
                if (power != 0)
                {
                    if (power < 0)
                        power = -power;
                    if (lastUsedMethod != null && !lastUsedMethod.Equals("BackRight"))
                        motorPair.Brake();
                    motorRight.Run((sbyte)-(Math.Round((double)power / turnRatio)), limit);
                    motorLeft.Run((sbyte)-power, limit);
                }
            }
            lastUsedMethod = "BackRight";
            Console.WriteLine("BackRight");
        }
        /// <summary>
        /// Move back and turn left too, power can be + or -, it's change nothing
        /// </summary>
        public void BackLeft(int power, uint limit)
        {
            if (motorPair != null)
            {
                if (power != 0)
                {
                    if (power < 0)
                        power = -power;
                    if (lastUsedMethod != null && !lastUsedMethod.Equals("BackLeft"))
                        motorPair.Brake();
                    motorLeft.Run((sbyte)-(Math.Round((double)power / turnRatio)), limit);
                    motorRight.Run((sbyte)-power, limit);
                }
            }
            lastUsedMethod = "BackLeft";
            Console.WriteLine("BackLeft");
        }
        /// <summary>
        /// Move Forward and turn left too, power can be + or -, it's change nothing
        /// </summary>
        public void RunLeft(int power, uint limit)
        {
            if (motorPair != null)
            {
                if (power != 0)
                {
                    if (power < 0)
                        power = +power;
                    if (lastUsedMethod != null && !lastUsedMethod.Equals("RunLeft"))
                        motorPair.Brake();
                    motorLeft.Run((sbyte)Math.Round((double)power / turnRatio), limit);
                    motorRight.Run((sbyte)power, limit);
                }
            }
            lastUsedMethod = "RunLeft";
            Console.WriteLine("RunLeft");
        }
        /// <summary>
        /// Turn on the right side, power can be + or -, it's change nothing
        /// </summary>
        public void TurnRight(int power, uint limit)
        {
            if (motorPair != null)
            {
                if (power != 0)
                {
                    if (power < 0)
                        power = +power;
                    if (lastUsedMethod != null && !lastUsedMethod.Equals("TurnRight"))
                        motorPair.Brake();
                    motorRight.Run((sbyte)-power, limit);
                    motorLeft.Run((sbyte)power, limit);
                }
            }
            lastUsedMethod = "TurnRight";
            Console.WriteLine("TurnRight");
        }
        /// <summary>
        ///  Turn on the left side, power can be + or -, it's change nothing
        /// </summary>
        public void TurnLeft(int power, uint limit)
        {
            if (motorPair != null)
            {
                if (power != 0)
                {
                    if (power < 0)
                        power = +power;
                    if (lastUsedMethod != null && !lastUsedMethod.Equals("TurnLeft"))
                        motorPair.Brake();
                    motorLeft.Run((sbyte)power, limit);
                    motorRight.Run((sbyte)-power, limit);
                }
            }
            lastUsedMethod = "TurnLeft";
            Console.WriteLine("TurnLeft");
        }
        /// <summary>
        /// Stop movement motors
        /// </summary>
        /// 
        public void Stop()
        {
            
            Console.WriteLine("Stopping");

            if (motorLeft != null && motorLeft != null)
            {
                motorLeft.Brake();
                motorRight.Brake();
            }
            
            //otherColorFounded = false;
            //firstRun = false;
            //if (colorSensor != null)
            //    colorSensor.OnPolled -= new Polled(IdentifyColorEvent);
        }
        /// <summary>
        /// Increase movement speed and recall the lastused movement action
        /// </summary>
        private void Turbo()
        {
            initialPower = initialPower + 10;
            if (initialPower > 100)
                initialPower = 100;

            if (motorPair != null)
            {
                InvokeMethod(lastUsedMethod);
            }
        }
        /// <summary>
        /// Decrease movement speed and recall the lastused movement action
        /// </summary>
        private void Brake()
        {
            initialPower = initialPower - 10;
            if (initialPower < 10)
                initialPower = 10;

            if (motorPair != null)
            {
                InvokeMethod(lastUsedMethod);
            }
        }

        #endregion

        #region Mouvements Events

        private void btnRun_Click(object sender, EventArgs e)
        {
            Run(initialPower, 0);
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            Run(-initialPower, 0);
        }

        private void btnTurnRight_Click(object sender, EventArgs e)
        {
            TurnRight(initialPower, 0);
        }

        private void btnTurnLeft_Click(object sender, EventArgs e)
        {
            TurnLeft(initialPower, 0);
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

        #region Clip Actions
        /// <summary>
        /// Open the clip
        /// </summary>
        public void OpenClip()
        {
            if (motorClip != null)
            {
                motorClip.Run(40, clipMaxDegrees);
            }
        }

        /// <summary>
        /// Close the clip
        /// </summary>
        public void CloseClip()
        {
            if (motorClip != null)
            {
                motorClip.Run(-40, clipMaxDegrees);
            }
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

        #region Captor Actions
        /// <summary>
        /// Action when right touch sensor activated
        /// </summary>
        /// <param name="polledItem"></param>
        private void TouchedOnRight(NxtPollable polledItem)
        {
            Stop();
            mainBrick.PlaySoundfile("Ouch 02.rso");
        }

        /// <summary>
        /// Action when left touch sensor activated
        /// </summary>
        /// <param name="polledItem"></param>
        private void TouchedOnLeft(NxtPollable polledItem)
        {
            Stop();
            mainBrick.PlaySoundfile("R2D2_03.rso");
        }

        private void OnDistance(NxtPollable polledItem)
        {
            if (defaultDistance == null)
                defaultDistance = ultraSonicSensor.DistanceCm;
            else if (ultraSonicSensor.DistanceCm < defaultDistance) // - 1
            {
                mainBrick.PlaySoundfile("Woops.rso");
            }
        }
        #endregion

        #region Color Actions
        /// <summary>
        /// Identify the color
        /// </summary>
        private void IdentifyColor()
        {
            if (colorSensor != null)
            {
                string color;
                string intensity;
                string trigger;

                if (colorSensor.Color != null)
                {
                    color = colorSensor.Color.Value.ToString();
                }
                else color = "null";
                if (colorSensor.Intensity != null)
                    intensity = colorSensor.Intensity.Value.ToString();
                else intensity = "null";

                trigger = colorSensor.TriggerIntensity.ToString();
                /*lblColor.Text = color;
                lblIntensity.Text = intensity;
                lblTriggerIntensity.Text = trigger;*/
            }
        }
        #endregion

        #region Color Events
        /// <summary>
        /// Method to check the color
        /// </summary>
        /// <param name="np"></param>
        private void IdentifyColorEvent(NxtPollable np)
        {
            if (colorSensor != null)
            {
                string s = colorSensor.Color.ToString();

                if (s == "Black")
                {
                    if (firstRun != true)
                    {
                        RunRight(initialPower, 0);
                        firstRun = true;
                    }
                    otherColorFounded = false;
                }
                else
                {
                    if (otherColorFounded == false)
                    {
                        if (lastUsedMethod == "RunRight")
                        {
                            RunLeft(initialPower, 0);
                        }
                        else if (lastUsedMethod == "RunLeft")
                        {
                            RunRight(initialPower, 0);
                        }
                    }
                    otherColorFounded = true;
                }
            }
        }

        #endregion

    }
}
