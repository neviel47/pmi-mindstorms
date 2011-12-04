using System;
using NKH.MindSqualls;

namespace MindstormsController
{
    public partial class XnaInputForm
    {
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
                    color = colorSensor.Color.Value.ToString();
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
