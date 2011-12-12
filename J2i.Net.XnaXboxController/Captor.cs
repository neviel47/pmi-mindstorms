using System;
using NKH.MindSqualls;

namespace MindstormsController
{
    public partial class XnaInputForm
    {
        #region NXT attributes
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
        #endregion

        #region attributes

        byte? defaultDistance = null;

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
            mainBrick.PlaySoundfile("Woops.rso");
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
                lblColor.Text = color;
                lblIntensity.Text = intensity;
                lblTriggerIntensity.Text = trigger;
            }
        }
        #endregion

        #region Captor Events
        private void btnIdentifyColor_Click(object sender, EventArgs e)
        {
            IdentifyColor();
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
                UpdateColorLabel(s);
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
    }
}
