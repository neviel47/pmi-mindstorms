using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NKH.MindSqualls;

namespace TestRobot
{
    public class NXTControl
    {
        private NxtBrick BlockMoteur;
        private Nxt2ColorSensor CapteurCouleur;
        private static bool EndAction;
        private uint OuverturePince;
        private bool PlierState;
        private string ColorChecked;

        public NXTControl ()
        {
            //Instanciation du block moteur
            BlockMoteur = new NxtBrick(NxtCommLinkType.USB, 5);
            
            //Moteur Pince
            BlockMoteur.MotorA = new NxtMotor();
            //Moteur Gauche
            BlockMoteur.MotorB = new NxtMotor();
            //Moteur Droit
            BlockMoteur.MotorC = new NxtMotor();

            //Capteur de couleur
            CapteurCouleur = new Nxt2ColorSensor();
            
            //Initialisation du capteur
            BlockMoteur.Sensor2 = CapteurCouleur;
            CapteurCouleur.SetColorDetectorMode();

            //Degré d'ouverture de la pince
            OuverturePince = 20;
            PlierState = false;

            ColorChecked = "White";
            EndAction = false;
        }

        ~NXTControl()
        {
            if (PlierState)
                ClosePliers();

        }

        /// <summary>
        /// Fonction de connection au robot
        /// </summary>
        /// <returns>Message comme quoi le robot est connecté ou message d'erreur</returns>
        public string ConnectRobot()
        {
            try
            {
                BlockMoteur.Connect();
                return "Connecté";
            }
            catch (Exception e)
            { return "Erreur de connection : " + e.Message; }
            
        }

        /// <summary>
        /// Fonction de mouvement du robot vers l'avant
        /// </summary>
        /// <param name="percentPower">Vitesse du moteur</param>
        /// <param name="numberDegre">Degré d'avancement de la roue</param>
        public void GoFront(int percentPower, uint numberDegre)
        {
            BlockMoteur.MotorB.Run((sbyte)percentPower, numberDegre);
            BlockMoteur.MotorC.Run((sbyte)percentPower, numberDegre);

        }

        /// <summary>
        /// Fait avancer le robot tant qu'on ne lui dit pas d'arrêter
        /// </summary>
        public void GoFrontWhileStop()
        {
            BlockMoteur.MotorB.Run((sbyte)20, 0);
            BlockMoteur.MotorC.Run((sbyte)20, 0);
        }

        /// <summary>
        /// Fonction de mouvement du robot vers la droite
        /// </summary>
        /// <param name="percentPower">Vitesse du moteur</param>
        /// <param name="numberDegre">Degré d'avancement de la roue</param>
        public void GoRight(int percentPower, uint numberDegre)
        {
            BlockMoteur.MotorC.Run((sbyte)percentPower, numberDegre);
        }

        /// <summary>
        /// Fait tourner le robot a droite tant qu'on ne l'arrête pas
        /// </summary>
        public void GoRightWhileStop()
        {
            BlockMoteur.MotorC.Run((sbyte)50, 0);
        }

        /// <summary>
        /// Fonction de mouvement du robot vers la gauche
        /// </summary>
        /// <param name="percentPower">Vitesse du moteur</param>
        /// <param name="numberDegre">Degré d'avancement de la roue</param>
        public void GoLeft(int percentPower, uint numberDegre)
        {
            BlockMoteur.MotorB.Run((sbyte)percentPower, numberDegre);
        }

        /// <summary>
        /// Fait tourner le robot a gauche tant qu'on ne l'arrête pas
        /// </summary>
        public void GoLeftWhileStop()
        {
            BlockMoteur.MotorB.Run((sbyte)50, 0);
        }

        /// <summary>
        /// Fonction de mouvement du robot vers l'arrière
        /// </summary>
        /// <param name="percentPower">Vitesse du moteur</param>
        /// <param name="numberDegre">Degré d'avancement de la roue</param>
        public void GoBack(int percentPower, uint numberDegre)
        {
            BlockMoteur.MotorB.Run((sbyte)-percentPower, numberDegre);
            BlockMoteur.MotorC.Run((sbyte)-percentPower, numberDegre);

        }

        /// <summary>
        /// Fait reculer le robot tant qu'on ne l'arrête pas
        /// </summary>
        public void GoBackWhileStop()
        {
            BlockMoteur.MotorB.Run((sbyte)-20, 0);
            BlockMoteur.MotorC.Run((sbyte)-20, 0);
        }

        /// <summary>
        /// Fonction de déconnection du robot
        /// </summary>
        /// <returns>Message comme quoi le robot est déconnecté ou message d'erreur</returns>
        public string DisconnectRobot()
        {
            try
            {
                BlockMoteur.Disconnect();
                return "Déconnecté";
            }
            catch (Exception e)
            { return "Erreur de déconnection : " + e.Message; }

        }

        /// <summary>
        /// Fonction de détection de la couleur présente sous le détecteur
        /// </summary>
        /// <returns>Retourne la couleur détectée</returns>
        public string DectectColor()
        {
            string colorDectected = "Aucune couleur détectée";

            try
            {
                CapteurCouleur.Poll();
                colorDectected = CapteurCouleur.Color.Value.ToString();
            }
            catch (Exception e)
            {
                colorDectected = "Erreur capteur : " + e.Message;
            }
            

            return colorDectected;
        }

        /// <summary>
        /// Fonction qui arrête les moteurs du robot
        /// </summary>
        public void StopRobot()
        {
            BlockMoteur.MotorB.Idle();
            BlockMoteur.MotorC.Idle();
        }

        /// <summary>
        /// Changement d'état de l'action en cours
        /// </summary>
        public void ChangeEndAction()
        {
            if (EndAction)
                EndAction = false;
            else
                EndAction = true;
        }

        /// <summary>
        /// Fonction d'ouverture de la pince
        /// </summary>
        public string OpenPliers()
        {
            BlockMoteur.MotorA.Run(30, OuverturePince);
            PlierState = true;
            return "Ouverture Pince"; 
        }

        /// <summary>
        /// Fonction de fermeture de la pince
        /// </summary>
        public string ClosePliers()
        {
            BlockMoteur.MotorA.Run(-30, OuverturePince);
            PlierState = false;
            return "Fermeture Pince";
        }

        /// <summary>
        /// Fonction de détection de la couleur noire
        /// </summary>
        public void LookingForBlack(NxtPollable poll)
        {
            if (CapteurCouleur.Color.Value.ToString() == "Black")
                ColorChecked = "Black";
            else
                ColorChecked = "Autre que noir";
        }

        public void ModeDectection()
        {
            BlockMoteur.Sensor2.PollInterval = 100;
            BlockMoteur.Sensor2.OnPolled += new Polled(LookingForBlack);
        }

        public string GetColorChecked()
        {
            return ColorChecked;
        }

        public void FolowTheLine()
        {

        }
    }
}
