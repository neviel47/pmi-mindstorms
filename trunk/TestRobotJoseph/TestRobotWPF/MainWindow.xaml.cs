using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using System.Windows.Threading;

namespace TestRobotWPF
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private NXTControl ControlRobot;
        private DispatcherTimer TimerColor;
        private bool EnterAutoMode;
        public MainWindow()
        {
            InitializeComponent();
            ControlRobot = new NXTControl();
            this.grid.Focus();
            EnterAutoMode = false;
            listTransportedColor.Items.Add("Blue");
            listTransportedColor.Items.Add("Black");
            listTransportedColor.Items.Add("Yellow");
            listTransportedColor.Items.Add("Green");
            listTransportedColor.Items.Add("Unknown");
        }

        private void bt_disconnect_Click(object sender, RoutedEventArgs e)
        {
            string returnDisCon = ControlRobot.DisconnectRobot();
            lb_con.Content = returnDisCon;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            string returnCon = ControlRobot.ConnectRobot();
            lb_con.Content = returnCon;
        }

        private void bt_goFront_Click(object sender, RoutedEventArgs e)
        {
            lb_state.Content = "Move Front";
            ControlRobot.GoFront(75, 360);
            lb_state.Content = "Stop";
        }

        private void bt_goBack_Click(object sender, RoutedEventArgs e)
        {
            lb_state.Content = "Move Back";
            ControlRobot.GoBack(75, 360);
            lb_state.Content = "Stop";
        }

        private void bt_goRight_Click(object sender, RoutedEventArgs e)
        {
            lb_state.Content = "Move Right";
            ControlRobot.GoFrontRight();
            lb_state.Content = "Stop";
        }

        private void bt_goLeft_Click(object sender, RoutedEventArgs e)
        {
            lb_state.Content = "Move Left";
            ControlRobot.GoFrontLeft();
            lb_state.Content = "Stop";
        }

        private void bt_colorDectect_Click(object sender, RoutedEventArgs e)
        {
            lb_colorDetected.Content = ControlRobot.DectectColor();
        }

        private void Grid_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Z:
                    lb_state.Content = "Move Front";
                    ControlRobot.GoFrontWhileStop();
                    break;
                case Key.S:
                    lb_state.Content = "Move Back";
                    ControlRobot.GoBack(75, 360);
                    break;
                case Key.D:
                    lb_state.Content = "Move Right";
                    ControlRobot.GoRight();
                    //ControlRobot.GoFrontRight();
                    break;
                case Key.Q:
                    lb_state.Content = "Move Left";
                    ControlRobot.GoLeft(75, 360);
                    break;
                case Key.E:
                    lb_state.Content = ControlRobot.OpenPliers();
                    break;
                case Key.R:
                    lb_state.Content = ControlRobot.ClosePliers();
                    break;
                case Key.Y:
                    lb_state.Content = "Follow The Line!";

                    if (EnterAutoMode)
                        EnterAutoMode = false;
                    else
                    {
                        EnterAutoMode = true;

                        ControlRobot.ModeDectection();
                        TimerColor = new DispatcherTimer();
                        TimerColor.Interval = new TimeSpan(0, 0, 0, 0, 50);
                        TimerColor.Tick += new EventHandler(AutomaticModeLine);
                        TimerColor.Start();

                        ControlRobot.SetAutomaticMode();
                    }
                        
                    break;
                case Key.T:
                    ControlRobot.ModeDectection();
                    TimerColor = new DispatcherTimer();
                    TimerColor.Interval = new TimeSpan(0, 0, 0, 0, 100);
                    TimerColor.Tick += new EventHandler(CheckColor);
                    TimerColor.Start();
                    //lb_colorDetected.Text = ControlRobot.GetColorChecked();
                    break;
            }
        }

        private void Grid_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Y)
            {
                //Arrêt du robot
                ControlRobot.StopRobot();
                lb_state.Content = "Stop";
            }
            else
            {
                if (!EnterAutoMode)
                {
                    ControlRobot.SetAutomaticMode();
                    //--------------TMP
                    ControlRobot.StopRobot();
                    //--------------
                    lb_state.Content = "Stop";
                    TimerColor.Stop();
                }
            }
        }

        private void CheckColor(object sender, EventArgs e)
        {
            lb_colorDetected.Content = ControlRobot.GetColorChecked();
            lb_colorTransported.Content = ControlRobot.GetColorTransported();
        }

        private void AutomaticModeLine(object sender, EventArgs e)
        {
            lb_colorDetected.Content = ControlRobot.GetColorChecked();
            lb_colorTransported.Content = ControlRobot.GetColorTransported();
            ControlRobot.AutomateScript();
        }

        private void bt_wrongWay_Click(object sender, RoutedEventArgs e)
        {
            ControlRobot.GoWrongWay(1);
            lb_state.Content = "Demi-tour droite";
        }

        private void bt_WrongWayLeft_Click(object sender, RoutedEventArgs e)
        {
            ControlRobot.GoWrongWay(0);
            lb_state.Content = "Demi-tour gauche";
        }

        private void bt_open_Click(object sender, RoutedEventArgs e)
        {
            ControlRobot.OpenPliers();
        }

        private void bt_close_Click(object sender, RoutedEventArgs e)
        {
            ControlRobot.ClosePliers();
        }

        private void bt_sound_Click(object sender, RoutedEventArgs e)
        {

        }

        private void bt_setColor_Click(object sender, RoutedEventArgs e)
        {
            if (listTransportedColor.SelectedItem != null)
            {
                ControlRobot.SetColorTransported(listTransportedColor.SelectedValue.ToString());
                lb_colorTransported.Content = listTransportedColor.SelectedValue.ToString();
            }
        }
    }
}
