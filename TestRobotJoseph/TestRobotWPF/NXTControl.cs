using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NKH.MindSqualls;
using System.Collections;

namespace TestRobotWPF
{
    public class NXTControl
    {
        //Déclaration
        //Block de gestion des moteurs
        private NxtBrick BlockMoteur;
        //Capteur de couleur
        private Nxt2ColorSensor CapteurCouleur;

        private static bool EndAction;
        //Degré d'ouverture de la pince
        private uint OuverturePince;
        //Etat d'ouverture de la pince
        private bool PlierState;
        //Couleur captée
        private string ColorChecked;

        //Indique le mouvement gauche/droite lorsque le robot suit une ligne
        private bool LeftRightDirection;
        //Mode automatique activé
        private bool AutomaticMode;
        //Ligne à suivre trouvée
        private bool BlackLineFound;
        //Premier démarrage du robot
        private bool HaveAlreadyTurn;
        //Couleur de la ligne à suivre
        private string FollowedColor;
        //Couleur du plateau
        private string BoardColor;
        //Couleur intersection boules
        private string BallIntersectColor;
        //Couleur intersection lettres
        private string LetterIntersectColor;
        //Action pour lettres/boules
        private string ActionLetterBall;
        //Enum de gestion de l'action en cours
        private enum EnumState {
            SuivreLigne = 0,
            IntersectionBoule = 1,
            AvancerLignes = 2,
            TournerGaucheLigne = 3,
            PositionnementLigne = 4,
            AttraperBalle = 5,
            RetournerLigneMilieu = 6,
            SuivreLigneVersBoule = 7,
            SuivreLigneVersLettre = 8,
            EnregistrerCouleur = 9,
            RetournerLigneMilieuEtDemiTour = 10,
            TurnAround = 11,
            DeposerBoule = 12,
            Final = 13
        };

        //Action en cours
        private int ToDoAction;
        private int LineBallToCatch;
        private int LineLetterToCatch;
        private int LineLetterAlreadyCross;
        private int LineBallAlreadyCross;
        private string ColorTransported;
        private Hashtable mapLetterColor;
        private int NumberBallMax;
        private bool ColorIsMapped;
        private bool HaveToReturn;
        private int CountCrossTheLine;
        private int LineLetterToReach;

        private bool TmpTest;

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
            OuverturePince = 60;
            PlierState = false;

            LeftRightDirection = false;
            ColorChecked = "Autre";
            EndAction = false;
            AutomaticMode = false;
            HaveAlreadyTurn = false;
            BlackLineFound = false;

            //Définition des couleurs standard du plateau
            BoardColor = "White";
            FollowedColor = "Red";
            BallIntersectColor = "Green";
            LetterIntersectColor = "Blue";
            NumberBallMax = 4;
            LineBallToCatch = 1;
            LineLetterToCatch = 1;
            mapLetterColor = new Hashtable();
            mapLetterColor.Add(1, "Unknown");
            mapLetterColor.Add(2, "Unknown");
            mapLetterColor.Add(3, "Unknown");
            mapLetterColor.Add(4, "Unknown");
            ToDoAction = (int)EnumState.SuivreLigne;
            LineLetterAlreadyCross = 0;
            LineBallAlreadyCross = 0;
            LineLetterToReach = 0;
            ColorIsMapped = false;
            ColorTransported = "Unknown";
            HaveToReturn = false;
            
            
            if(!PlierState)
                OpenPliers();

            TmpTest = true;
        }

        ~NXTControl()
        {
            if (PlierState)
                ClosePliers();
            StopRobot();


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
        public void GoRight()
        {
            BlockMoteur.MotorB.Run(-20, 0);
            BlockMoteur.MotorC.Run(20, 0);
        }

        /// <summary>
        /// Fait tourner le robot a droite tant qu'on ne l'arrête pas
        /// </summary>
        public void GoRightWhileStop()
        {
            BlockMoteur.MotorC.Run((sbyte)50, 0);
        }

        /// <summary>
        /// Fonction de mouvement du robot vers la droite en avançant
        /// </summary>
        public void GoFrontRight()
        {
            BlockMoteur.MotorB.Run(10, 0);
            BlockMoteur.MotorC.Run(40, 0);
        }

        public void GoFrontRightLine()
        {
            BlockMoteur.MotorB.Run(15, 0);
            BlockMoteur.MotorC.Run(25, 0);
        }

        public void GoFrontLeftLine()
        {
            BlockMoteur.MotorB.Run(25, 0);
            BlockMoteur.MotorC.Run(15, 0);
        }

        /// <summary>
        /// Fonction de mouvement du robot vers la gauche en avançant
        /// </summary>
        public void GoFrontLeft()
        {
            BlockMoteur.MotorB.Run(40, 0);
            BlockMoteur.MotorC.Run(10, 0);
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
            BlockMoteur.MotorA.Run(5, OuverturePince);
            PlierState = true;
            return "Ouverture Pince"; 
        }

        /// <summary>
        /// Fonction de fermeture de la pince
        /// </summary>
        public string ClosePliers()
        {
            BlockMoteur.MotorA.Run(-5, OuverturePince);
            PlierState = false;
            return "Fermeture Pince";
        }

        /// <summary>
        /// Fonction de détection de la couleur
        /// </summary>
        public void LookingForBlack(NxtPollable poll)
        {
            switch (CapteurCouleur.Color.Value.ToString())
            {
                case "Black":
                    ColorChecked = "Black";
                    break;
                case "White":
                    ColorChecked = "White";
                    break;
                case "Red":
                    ColorChecked = "Red";
                    break;
                case "Blue":
                    ColorChecked = "Blue";
                    break;
                case "Green":
                    ColorChecked = "Green";
                    break;
                case "Yellow":
                    ColorChecked = "Yellow";
                    break;
                default:
                    ColorChecked = "Autre";
                    break;
            }
        }

        /// <summary>
        /// Activation du mode de détection de couleur
        /// </summary>
        public void ModeDectection()
        {
            BlockMoteur.Sensor2.PollInterval = 100;
            BlockMoteur.Sensor2.OnPolled += new Polled(LookingForBlack);
        }

        /// <summary>
        /// Retourne la couleur détectée par le capteur
        /// </summary>
        /// <returns>Couleur détectée</returns>
        public string GetColorChecked()
        {
            return ColorChecked;
        }

        /// <summary>
        /// Retourne la couleur de la boule transportée
        /// </summary>
        /// <returns></returns>
        public string GetColorTransported()
        {
            return ColorTransported;
        }

        /// <summary>
        /// Fonction faisant faire un demi-tour au robot
        /// </summary>
        public void GoWrongWay(int leftRight)
        {
            if (CountCrossTheLine < 2)
            {
                //Demi tour vers la droite
                if (leftRight == 1)
                {
                    BlockMoteur.MotorB.Run(-50, 0);
                    BlockMoteur.MotorC.Run(50, 0);
                } //Demi tour vers la gauche
                else
                {
                    BlockMoteur.MotorC.Run(-50, 0);
                    BlockMoteur.MotorB.Run(50, 0);
                }

                if (!BlackLineFound && ColorChecked == FollowedColor)
                {
                    BlackLineFound = true;
                }
                else if (BlackLineFound && ColorChecked == BoardColor)
                {
                    BlackLineFound = false;
                    CountCrossTheLine++;
                }
                
            }
            else
            {
                StopRobot();
                System.Threading.Thread.Sleep(1000);
                BlockMoteur.MotorB.Run(-50, 200);
                BlockMoteur.MotorC.Run(50, 200);
                
                System.Threading.Thread.Sleep(1000);

                ToDoAction = (int)EnumState.SuivreLigne;
            }
        }

        /// <summary>
        /// Script de gestion automatique du robot,
        /// Comprend les appels des fonctions des actions enchainées du robot
        /// </summary>
        public void AutomateScript()
        {
            //Si une action est en cours, on attend qu'elle se finisse
            if (EndAction)
            {
                return;
            }
            switch (ToDoAction)
            {
                case (int)EnumState.SuivreLigne:
                    FolowTheLine();
                    break;
                case (int)EnumState.AvancerLignes:
                    GetPositionForLines();
                    break;
                case (int)EnumState.PositionnementLigne:
                    LeftRightDirection = false;
                    if (ActionLetterBall == LetterIntersectColor)
                        GetLetterLine();
                    else
                        GetBallLine();
                    break;
                case (int)EnumState.SuivreLigneVersBoule:
                    FolowTheBallLine();
                    break;
                case (int)EnumState.SuivreLigneVersLettre:
                    FolowTheLetterLine();
                    break;
                case (int)EnumState.EnregistrerCouleur:
                    SaveColor();
                    break;
                case (int)EnumState.AttraperBalle:
                    CatchTheBall();
                    break;
                case (int)EnumState.DeposerBoule:
                    DropTheBall();
                    break;
                case (int)EnumState.RetournerLigneMilieu:
                    GetTheMidleLine();
                    break;
                case (int)EnumState.RetournerLigneMilieuEtDemiTour:
                    GetTheMidleLineAndTurnBack();
                    break;
                case (int)EnumState.TurnAround:
                    GoWrongWay(0);
                    break;
                case (int)EnumState.Final:
                    FinalPlaySound();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Le robot dépose la boule sur la bonne couleur
        /// </summary>
        private void DropTheBall()
        {
            if (!EndAction)
            {
                ChangeEndAction();

                //Avancer
                BlockMoteur.MotorB.Run(50, 500);
                BlockMoteur.MotorC.Run(50, 500);
                System.Threading.Thread.Sleep(2000);

                //Ouverture de la pince
                OpenPliers();
                System.Threading.Thread.Sleep(1000);

                //Avancer
                BlockMoteur.MotorB.Run(-50, 500);
                BlockMoteur.MotorC.Run(-50, 500);

                System.Threading.Thread.Sleep(2000);

                //Final ou pas
                if (ColorTransported == "Blue")
                    ToDoAction = (int)EnumState.Final;
                else  //On revient vers les boules
                    ToDoAction = (int)EnumState.RetournerLigneMilieuEtDemiTour;

                ChangeEndAction();
            }
            
        }

        /// <summary>
        /// Fonction permettant au robot de revenir sur la ligne du milieu et de faire demi-tour
        /// </summary>
        private void GetTheMidleLineAndTurnBack()
        {
            if (!EndAction)
            {
                ChangeEndAction();
                int ActualLinePosition;
                //Cas après avoir déposé une boule
                if (ColorTransported != "Unknown")
                {
                    ActualLinePosition = LineLetterToReach;
                    ColorTransported = "Unknown";
                    LineLetterToReach = 0;
                }
                else //Autre cas
                    ActualLinePosition = LineLetterAlreadyCross - 1;
                //Rotation fonction de l'emplacement de la ligne
                switch (LineLetterAlreadyCross)
                {
                    case 1:
                        BlockMoteur.MotorC.Run(-50, 700);
                        BlockMoteur.MotorB.Run(50, 700);
                        System.Threading.Thread.Sleep(2000);

                        //On avance le robot de sorte qu'il se trouve sur la droite de la ligne, puis on change d'action
                        BlockMoteur.MotorC.Run(50, 800);
                        BlockMoteur.MotorB.Run(50, 800);
                        LeftRightDirection = false;

                        System.Threading.Thread.Sleep(2000);

                        break;
                    case 2:
                        BlockMoteur.MotorC.Run(-50, 850);
                        BlockMoteur.MotorB.Run(50, 850);
                        
                        System.Threading.Thread.Sleep(2000);

                        //On avance le robot de sorte qu'il se trouve sur la droite de la ligne, puis on change d'action
                        BlockMoteur.MotorC.Run(50, 2700);
                        BlockMoteur.MotorB.Run(50, 2700);
                        LeftRightDirection = false;

                        System.Threading.Thread.Sleep(2000);
                        break;
                    case 3:
                        BlockMoteur.MotorC.Run(-50, 900);
                        BlockMoteur.MotorB.Run(50, 900);
                        
                        System.Threading.Thread.Sleep(2000);

                        //On avance le robot de sorte qu'il se trouve sur la droite de la ligne, puis on change d'action
                        BlockMoteur.MotorC.Run(50, 900);
                        BlockMoteur.MotorB.Run(50, 900);
                        LeftRightDirection = false;

                        System.Threading.Thread.Sleep(2000);
                        break;
                    case 4:
                        BlockMoteur.MotorC.Run(-50, 1100);
                        BlockMoteur.MotorB.Run(50, 1100);
                        
                        System.Threading.Thread.Sleep(2000);

                        //On avance le robot de sorte qu'il se trouve sur la droite de la ligne, puis on change d'action
                        BlockMoteur.MotorC.Run(50, 450);
                        BlockMoteur.MotorB.Run(50, 450);
                        LeftRightDirection = false;

                        System.Threading.Thread.Sleep(2000);
                        break;
                    default:
                        StopRobot();
                        throw new NotImplementedException();
                }

                
                LineLetterAlreadyCross = 0;
                ChangeEndAction();
                ToDoAction = (int)EnumState.SuivreLigne;

            }
        }

        /// <summary>
        /// Enregistrement de la couleur associée à la ligne
        /// </summary>
        private void SaveColor()
        {
            //Enregistrement de la couleur
            if (mapLetterColor.ContainsKey(LineLetterAlreadyCross))
            {
                mapLetterColor[LineLetterAlreadyCross] = ColorChecked;

                //Si toutes les couleurs ont été détectée
                if (LineLetterAlreadyCross == mapLetterColor.Keys.Count)
                {
                    //On va chercher les boules
                    ColorIsMapped = true;
                    HaveToReturn = false;
                    ToDoAction = (int)EnumState.RetournerLigneMilieuEtDemiTour;
                }
                else
                {
                    //Sinon on va scanner la couleur suivante
                    HaveToReturn = true;
                    ToDoAction = (int)EnumState.RetournerLigneMilieuEtDemiTour;
                }
            }
            else
	        {
                StopRobot();
                throw new NotImplementedException();
	        }
        }

        /// <summary>
        /// Suis la ligne jusqu'à rencontrer une lettre de couleur
        /// </summary>
        private void FolowTheLetterLine()
        {
            //On vérifie qu'on est pas à une intersection
            if (ColorChecked == "Yellow" || ColorChecked == "Blue" || ColorChecked == "Green" || ColorChecked == "Black")
            {
                HaveAlreadyTurn = false;
                BlackLineFound = false;
                StopRobot();

                //Changement d'action en fonction de si le robot doit repérer la couleur ou déposer une boule
                if (ColorTransported != "Unknown")
                    ToDoAction = (int)EnumState.DeposerBoule;
                else
                    ToDoAction = (int)EnumState.EnregistrerCouleur;
                
                return;
            }
            //Premier lancement
            if (!HaveAlreadyTurn && !BlackLineFound && ColorChecked != FollowedColor)
            {
                GoDirectionLineAuto();
            }
            //Première fois sur la ligne de couleur noire
            else if (!HaveAlreadyTurn && !BlackLineFound && ColorChecked == FollowedColor)
            {
                BlackLineFound = true;
            }
            //Première sortie de la ligne de couleur noire
            else if (!HaveAlreadyTurn && BlackLineFound && ColorChecked == BoardColor)
            {
                BlackLineFound = false;
                HaveAlreadyTurn = true;
                ChangeDirection();
                GoDirectionLineAuto();
            }
            //Trouve ligne noire
            else if (HaveAlreadyTurn && !BlackLineFound && ColorChecked == FollowedColor)
            {
                BlackLineFound = true;
            }
            //Sort de la ligne
            else if (HaveAlreadyTurn && BlackLineFound && ColorChecked == BoardColor)
            {
                BlackLineFound = false;
                ChangeDirection();
                GoDirectionLineAuto();
            }
        }

        /// <summary>
        /// Suis la ligne jusqu'à rencontrer une boule de couleur
        /// </summary>
        private void FolowTheBallLine()
        {
            //On vérifie qu'on est pas à une intersection
            if (ColorChecked == "Yellow" || ColorChecked == "Blue" || ColorChecked == "Green" || ColorChecked == "Black")
            {
                //Changement d'action
                ToDoAction = (int)EnumState.AttraperBalle;
                HaveAlreadyTurn = false;
                BlackLineFound = false;
                StopRobot();
                return;
            }
            //Premier lancement
            if (!HaveAlreadyTurn && !BlackLineFound && ColorChecked != FollowedColor)
            {
                GoDirectionLineAuto();
            }
            //Première fois sur la ligne de couleur noire
            else if (!HaveAlreadyTurn && !BlackLineFound && ColorChecked == FollowedColor)
            {
                BlackLineFound = true;
            }
            //Première sortie de la ligne de couleur noire
            else if (!HaveAlreadyTurn && BlackLineFound && ColorChecked == BoardColor)
            {
                BlackLineFound = false;
                HaveAlreadyTurn = true;
                ChangeDirection();
                GoDirectionLineAuto();
            }
            //Trouve ligne noire
            else if (HaveAlreadyTurn && !BlackLineFound && ColorChecked == FollowedColor)
            {
                BlackLineFound = true;
            }
            //Sort de la ligne
            else if (HaveAlreadyTurn && BlackLineFound && ColorChecked == BoardColor)
            {
                BlackLineFound = false;
                ChangeDirection();
                GoDirectionLineAuto();
            }
        }

        /// <summary>
        /// Fonction permettant de retourner à la ligne centrale
        /// </summary>
        private void GetTheMidleLine()
        {
            if (!EndAction)
            {
                ChangeEndAction();

                int ActualLinePosition;
                //Cas après avoir déposé une boule
                if (LineLetterToReach != 0 && ColorTransported != "Unknown")
                {
                    ActualLinePosition = LineLetterToReach;
                    ColorTransported = "Unknown";
                    LineLetterToReach = 0;
                }
                else //Autre cas
                    ActualLinePosition = LineBallToCatch - 1;

                //Rotation fonction de l'emplacement de la ligne
                switch (ActualLinePosition)
                {
                    case 1:
                        BlockMoteur.MotorC.Run(-50, 800);
                        BlockMoteur.MotorB.Run(50, 800);
                        System.Threading.Thread.Sleep(2000);

                        //On avance le robot de sorte qu'il se trouve sur la droite de la ligne, puis on change d'action
                        BlockMoteur.MotorC.Run(50, 900);
                        BlockMoteur.MotorB.Run(50, 900);
                        LeftRightDirection = false;

                        System.Threading.Thread.Sleep(2000);

                        break;
                    case 2:
                        BlockMoteur.MotorC.Run(-50, 950);
                        BlockMoteur.MotorB.Run(50, 950);

                        System.Threading.Thread.Sleep(2000);

                        //On avance le robot de sorte qu'il se trouve sur la droite de la ligne, puis on change d'action
                        BlockMoteur.MotorC.Run(50, 900);
                        BlockMoteur.MotorB.Run(50, 900);
                        LeftRightDirection = false;

                        System.Threading.Thread.Sleep(2000);
                        break;
                    case 3:
                        BlockMoteur.MotorC.Run(-50, 1150);
                        BlockMoteur.MotorB.Run(50, 1150);

                        System.Threading.Thread.Sleep(2000);

                        //On avance le robot de sorte qu'il se trouve sur la droite de la ligne, puis on change d'action
                        BlockMoteur.MotorC.Run(50, 1000);
                        BlockMoteur.MotorB.Run(50, 1000);
                        LeftRightDirection = false;

                        System.Threading.Thread.Sleep(2000);
                        break;
                    case 4:
                        BlockMoteur.MotorC.Run(-50, 1100);
                        BlockMoteur.MotorB.Run(50, 1100);

                        System.Threading.Thread.Sleep(2000);

                        //On avance le robot de sorte qu'il se trouve sur la droite de la ligne, puis on change d'action
                        BlockMoteur.MotorC.Run(50, 700);
                        BlockMoteur.MotorB.Run(50, 700);
                        LeftRightDirection = false;

                        System.Threading.Thread.Sleep(2000);

                        BlockMoteur.MotorC.Run(-50, 300);
                        BlockMoteur.MotorB.Run(50, 300);

                        System.Threading.Thread.Sleep(1000);
                        break;
                    default:
                        StopRobot();
                        throw new NotImplementedException();
                    //case 1:
                    //    BlockMoteur.MotorC.Run(-50, 800);
                    //    BlockMoteur.MotorB.Run(50, 800);
                    //    break;
                    //case 2:
                    //    BlockMoteur.MotorC.Run(-50, 600);
                    //    BlockMoteur.MotorB.Run(50, 600);
                    //    break;
                    //case 3:
                    //    BlockMoteur.MotorC.Run(-50, 600);
                    //    BlockMoteur.MotorB.Run(50, 600);
                    //    break;
                    //case 4:
                    //    BlockMoteur.MotorC.Run(-50, 600);
                    //    BlockMoteur.MotorB.Run(50, 600);
                    //    break;
                    //default:
                    //    break;
                }

                System.Threading.Thread.Sleep(2000);

                //On avance le robot de sorte qu'il se trouve sur la droite de la ligne, puis on change d'action
                BlockMoteur.MotorC.Run(50, 600);
                BlockMoteur.MotorB.Run(50, 600);
                LeftRightDirection = false;
                
                LineLetterAlreadyCross = 0;
                ChangeEndAction();
                ToDoAction = (int)EnumState.SuivreLigne;
                
            }
        }

        /// <summary>
        /// Fonction permettant d'attraper la baballe
        /// </summary>
        private void CatchTheBall()
        {
            //Fermeture de la pince
            ClosePliers();

            //On enregistre la couleur de la balle
            ColorTransported = ColorChecked;

            //Pause pour attendre l'action
            System.Threading.Thread.Sleep(1000);

            //Action suivante
            ToDoAction = (int)EnumState.RetournerLigneMilieu;
        }

        /// <summary>
        /// Positionne le robot à droite de la ligne à suivre pour prendre la boule
        /// </summary>
        private void GetBallLine()
        {
            //On fait tourner le robot tant qu'il n'est pas à droite de la ligne à suivre
            
            //Premier démarrage
            if (!HaveAlreadyTurn)
            {
                //On fait tourner le robot vers la droite
                HaveAlreadyTurn = true;
                GoRight();
            }
            //En tournant, on check les lignes qui passent sous le capteur
            else if (HaveAlreadyTurn && !BlackLineFound && ColorChecked == FollowedColor)
            {
                //ligne trouvée
                BlackLineFound = true;
            }
            else if (HaveAlreadyTurn && BlackLineFound && ColorChecked == BoardColor)
            {
                //ligne noire dépassée
                BlackLineFound = false;
                //On compte la ligne
                LineBallAlreadyCross++;
                //Si c'est la ligne qu'on cherche
                if (LineBallAlreadyCross == LineBallToCatch)
                {
                    //On passe l'indice à la balle suivante
                    LineBallToCatch++;
                    //Et on va vers la zone de couleur
                    HaveAlreadyTurn = false;
                    BlockMoteur.MotorB.Run(20, 100);
                    BlockMoteur.MotorC.Run(-20, 100);

                    ToDoAction = (int)EnumState.SuivreLigneVersBoule;
                }

                //Sinon le script continu jusqu'à la bonne ligne
            }
        }

        /// <summary>
        /// Positionne le robot à droite de la ligne à suivre pour déposer la boule
        /// </summary>
        private void GetLetterLine()
        {
            //On fait tourner le robot tant qu'il n'est pas à droite de la ligne à suivre
            //Si les lignes ne sont pas encore analysées
            if (!ColorIsMapped)
            {
                if (TmpTest)
                {
                    mapLetterColor[1] = "Green";
                    mapLetterColor[2] = "Blue";
                    mapLetterColor[3] = "Yellow";
                }
                //Premier démarrage
                if (!HaveAlreadyTurn)
                {
                    //On fait tourner le robot vers la droite
                    HaveAlreadyTurn = true;
                    GoRight();
                }
                //En tournant, on check les lignes qui passent sous le capteur
                else if (HaveAlreadyTurn && !BlackLineFound && ColorChecked == FollowedColor)
                {
                    //ligne trouvée
                    BlackLineFound = true;
                }
                else if (HaveAlreadyTurn && BlackLineFound && ColorChecked == BoardColor)
                {
                    //ligne noire dépassée
                    BlackLineFound = false;

                    //On compte la ligne
                    LineLetterAlreadyCross++;
                   
                    if (mapLetterColor.ContainsKey(LineLetterAlreadyCross))
                    {
                        //Si la ligne n'est pas encore repérée
                        if (mapLetterColor[LineLetterAlreadyCross].ToString() == "Unknown")
                        {
                            //On va enregistrer la couleur
                            HaveAlreadyTurn = false;
                            BlockMoteur.MotorB.Run(20, 100);
                            BlockMoteur.MotorC.Run(-20, 100);

                            ToDoAction = (int)EnumState.SuivreLigneVersLettre;
                        }
                        
                    }

                    //Sinon le script continu vers la ligne suivante
                }
            
            }
            //Si les couleurs ont déjà été mappée
            else
            {
                //On repère la ligne à atteindre en fonction de la couleur transportée
                if (ColorTransported != "Unknown" && LineLetterToReach == 0)
                {
                    //Parcours des couleurs
                    foreach (int indexMap in mapLetterColor.Keys)
                    {
                        //Jusqu'à ce qu'on trouve la ligne à suivre
                        if (mapLetterColor[indexMap].ToString() == ColorTransported)
                        {
                            //Qu'on récupère
                            LineLetterToReach = indexMap;
                            break;  
                        }
                    }
                }//Si la ligne a déjà été repérée
                else if (ColorTransported != "Unknown" && LineLetterToReach != 0)
                {
                    //Premier démarrage
                    if (!HaveAlreadyTurn)
                    {
                        //On fait tourner le robot vers la droite
                        HaveAlreadyTurn = true;
                        GoRight();
                    }
                    //En tournant, on check les lignes qui passent sous le capteur
                    else if (HaveAlreadyTurn && !BlackLineFound && ColorChecked == FollowedColor)
                    {
                        //ligne trouvée
                        BlackLineFound = true;
                    }
                    else if (HaveAlreadyTurn && BlackLineFound && ColorChecked == BoardColor)
                    {
                        //ligne noire dépassée
                        BlackLineFound = false;

                        //On compte la ligne
                        LineLetterAlreadyCross++;
                        //Si la ligne correspond à notre couleur
                        if (LineLetterAlreadyCross == LineLetterToReach)
                        {
                            //On va déposer la boule
                            HaveAlreadyTurn = false;
                            BlockMoteur.MotorB.Run(20, 100);
                            BlockMoteur.MotorC.Run(-20, 100);

                            ToDoAction = (int)EnumState.SuivreLigneVersLettre;
                        }

                        //Sinon le script continu vers la ligne suivante
                    }
                }
                else
                {
                    StopRobot();
                    throw new NotImplementedException();
                }
            }
        }

        /// <summary>
        /// Fonction positionnant le robot à gauche des lignes afin de pouvoir les compter
        /// </summary>
        private void GetPositionForLines()
        {
            if (!EndAction)
            {
                ChangeEndAction();

                //On avance un peu le robot pour pouvoir scanner les lignes
                BlockMoteur.MotorC.Run(50, 450);
                BlockMoteur.MotorB.Run(50, 450);

                //Pause pour attendre l'action
                System.Threading.Thread.Sleep(3000);

                //Rotation du robot sur lui même vers la gauche
                BlockMoteur.MotorC.Run(-50, 550);
                BlockMoteur.MotorB.Run(50, 600);

                //Pause pour attendre l'action
                System.Threading.Thread.Sleep(3000);


                LineBallAlreadyCross = 0;

                ChangeEndAction();
                //Changement action
                ToDoAction = (int)EnumState.PositionnementLigne;
            }
            
        }

        /// <summary>
        /// Script qui suit une ligne automatiquement
        /// Fonctionne en faisant tourner de gauche à droite le robot en même temps
        /// qu'il avance
        /// </summary>
        public void FolowTheLine()
        {
            if (PlierState)
                ClosePliers();

            //On vérifie qu'on est pas à une intersection
            if (ColorChecked == BallIntersectColor || ColorChecked == LetterIntersectColor)
            {

                //Changement d'action
                HaveAlreadyTurn = false;
                BlackLineFound = false;
                if (ColorChecked == BallIntersectColor)
                {
                    OpenPliers();
                    ActionLetterBall = BallIntersectColor;
                }
                else
                    ActionLetterBall = LetterIntersectColor;
                StopRobot();
                ToDoAction = (int)EnumState.AvancerLignes;
                return;
            }
            //Premier lancement
            if (!HaveAlreadyTurn && !BlackLineFound && ColorChecked != FollowedColor)
            {
                GoDirectionAuto();
            }
            //Première fois sur la ligne de couleur noire
            else if (!HaveAlreadyTurn && !BlackLineFound && ColorChecked == FollowedColor)
            {
                BlackLineFound = true;

                //Si on est en phase de repérage, on fait demi tour
                if (HaveToReturn)
                {
                    HaveAlreadyTurn = false;
                    BlackLineFound = false;
                    HaveToReturn = false;
                    CountCrossTheLine = 0;
                    ToDoAction = (int)EnumState.TurnAround;

                }
            }
            //Première sortie de la ligne de couleur noire
            else if (!HaveAlreadyTurn && BlackLineFound && ColorChecked == BoardColor)
            {
                BlackLineFound = false;
                HaveAlreadyTurn = true;
                ChangeDirection();
                GoDirectionAuto();
            }
            //Trouve ligne noire
            else if (HaveAlreadyTurn && !BlackLineFound && ColorChecked == FollowedColor)
            {
                BlackLineFound = true;
            }
            //Sort de la ligne
            else if (HaveAlreadyTurn && BlackLineFound && ColorChecked == BoardColor)
            {
                BlackLineFound = false;
                ChangeDirection();
                GoDirectionAuto();
            }
        }

        /// <summary>
        /// Change la direction en cour du robot
        /// LeftRightDirection vrai : droite
        /// LeftRightDirection faux : gauche
        /// </summary>
        public void ChangeDirection()
        {
            //Droite
            if (LeftRightDirection)
                LeftRightDirection = false; //vers gauche
            else //Gauche 
                LeftRightDirection = true; //vers droite
        }

        /// <summary>
        /// Fait avancer le robot en fonction de la variable LeftRightDirection
        /// LeftRightDirection vrai : droite
        /// LeftRightDirection faux : gauche
        /// </summary>
        public void GoDirectionAuto()
        {
            //Droite
            if (LeftRightDirection)
                GoFrontRight();
            else //Gauche 
                GoFrontLeft(); 
        }

        public void FinalPlaySound()
        {
            StopRobot();
            BlockMoteur.PlaySoundfile("Togepi.rso");
        }

        public void GoDirectionLineAuto()
        {
            //Droite
            if (LeftRightDirection)
                GoFrontRightLine();
            else //Gauche 
                GoFrontLeftLine();
        }

        /// <summary>
        /// Activation/Désactivation du mode auto
        /// </summary>
        public void SetAutomaticMode()
        {
            if (AutomaticMode)
                AutomaticMode = false;
            else
                AutomaticMode = true;
        }

        /// <summary>
        /// Accesseur couleur transportée
        /// </summary>
        /// <param name="newColor">nouvelle couleur</param>
        public void SetColorTransported(string newColor)
        {
            ColorTransported = newColor;
        }
    }
}
