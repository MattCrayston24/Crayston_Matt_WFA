using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace T_Rex_Game
{
    public partial class Form1 : Form
    {
        // Variables pour gérer le jeu
        bool jumping = false; // Indique si le personnage est en train de sauter
        int jumpSpeed; // La vitesse du saut
        int force = 12; // La force du saut
        int score = 0; // Le score du joueur
        int obstacleSpeed = 10; // La vitesse des obstacles
        Random rand = new Random(); // Générateur de nombres aléatoires
        int position; // Position initiale des obstacles
        bool isGameOver = false; // Indique si le jeu est terminé
        bool isObstacleAerialEnabled = false; // Indique si les obstacles aériens sont activés
        bool ducking = false; // Indique si le personnage est en position de canard

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void txtscore_Click(object sender, EventArgs e)
        {

        }

        public Form1()
        {
            InitializeComponent();
            GameReset(); // Réinitialise le jeu
        }

        private void MainGameTimerEvent(object sender, EventArgs e)
        {
            trex.Top += jumpSpeed; // Déplace le personnage vers le haut ou vers le bas en fonction de la vitesse du saut

            txtscore.Text = "Score:" + score; // Met à jour le score affiché à l'écran

            // Changer la couleur de fond en noir clair (30, 30, 30) lorsque le score atteint ou dépasse 10
            if (score >= 10)
            {
                this.BackColor = Color.FromArgb(30, 30, 30);

                // Changer la couleur de fond des obstacles en noir clair (30, 30, 30) lorsque le score atteint ou dépasse 10
                foreach (Control x in this.Controls)
                {
                    if (x is PictureBox && ((string)x.Tag == "obstacle" || (string)x.Tag == "obstacleaerien"))
                    {
                        x.BackColor = Color.FromArgb(30, 30, 30);
                    }
                }
            }

            // Gestion du saut
            if (jumping == true && force < 0)
            {
                jumping = false; // Arrête le saut lorsque la force est épuisée
            }

            if (jumping == true)
            {
                jumpSpeed = -12; // Fait monter le personnage
                force -= 1; // Réduit la force du saut
            }
            else
            {
                jumpSpeed = 12; // Fait descendre le personnage
            }

            // Empêche le personnage de descendre en dessous du sol
            if (trex.Top > 289 && jumping == false)
            {
                force = 12;
                trex.Top = 290;
                jumpSpeed = 0;
            }

            // Gestion des collisions avec les obstacles
            foreach (Control x in this.Controls)
            {
                if (x is PictureBox)
                {
                    if ((string)x.Tag == "obstacle" || ((string)x.Tag == "obstacleaerien" && score > 5))
                    {
                        x.Left -= obstacleSpeed; // Déplace les obstacles vers la gauche

                        if (x.Left < -100) // Réinitialise la position des obstacles lorsqu'ils sortent de l'écran
                        {
                            x.Left = this.ClientSize.Width + rand.Next(200, 500) + (x.Width * 15);
                            if ((string)x.Tag == "obstacle")
                            {
                                score++; // Incrémente le score lorsque le joueur évite un obstacle terrestre
                            }
                        }

                        // Vérifie la collision avec le personnage
                        if (trex.Bounds.IntersectsWith(x.Bounds))
                        {
                            if ((string)x.Tag == "obstacleaerien")
                            {
                                if (ducking)
                                {
                                    // Évite la collision avec l'obstacle aérien lorsque "ducker1" est en position de canard
                                    continue;
                                }
                            }

                            gameTimer.Stop(); // Arrête le timer du jeu
                            trex.Image = Properties.Resources.dead; // Affiche l'image du personnage mort
                            txtscore.Text += " Press R to restart the game!"; // Affiche un message pour redémarrer le jeu
                            isGameOver = true; // Indique que le jeu est terminé
                        }
                    }
                }
            }

            // Active les obstacles aériens après avoir atteint un certain score
            if (score > 20 && !isObstacleAerialEnabled)
            {
                isObstacleAerialEnabled = true;
                PictureBox obstacleAerien = new PictureBox();
                obstacleAerien.Image = Properties.Resources.obstacleaerien;
                obstacleAerien.Size = new Size(40, 40);
                obstacleAerien.Tag = "obstacleaerien";
                obstacleAerien.Left = this.ClientSize.Width + rand.Next(200, 500) + (obstacleAerien.Width * 15);
                obstacleAerien.Top = 290 - obstacleAerien.Height;
                this.Controls.Add(obstacleAerien);
            }

            // Augmente la vitesse des obstacles à mesure que le score augmente
            if (score > 10)
            {
                obstacleSpeed = 15;
            }

            if (score > 30)
            {
                obstacleSpeed = 18;
            }

            if (score > 50)
            {
                obstacleSpeed = 20;
            }
        }

        private void keyisdown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space && jumping == false)
            {
                jumping = true; // Déclenche le saut lorsque la barre d'espace est pressée
            }

            if (e.KeyCode == Keys.Down && !jumping)
            {
                ducking = true; // Active la position de canard lorsque la touche bas est pressée
                trex.Top = 290;
                trex.Image = Properties.Resources.ducker1; // Utilise l'image "ducker1"
            }
        }

        private void keyisup(object sender, KeyEventArgs e)
        {
            if (jumping == true)
            {
                jumping = false; // Arrête le saut lorsque la touche est relâchée
            }

            if (e.KeyCode == Keys.R && isGameOver == true)
            {
                GameReset(); // Réinitialise le jeu lorsque la touche R est pressée après la fin du jeu
            }

            if (e.KeyCode == Keys.Down)
            {
                ducking = false; // Désactive la position de canard lorsque la touche bas est relâchée
                trex.Top = 290;
                trex.Image = Properties.Resources.running; // Utilise l'image de course
            }
        }

        private void GameReset()
        {
            force = 12; // Réinitialise la force du saut
            jumpSpeed = 0; // Réinitialise la vitesse du saut
            jumping = false; // Réinitialise l'état de saut
            score = 0; // Réinitialise le score
            obstacleSpeed = 10; // Réinitialise la vitesse des obstacles
            txtscore.Text = "Score: " + score; // Réinitialise l'affichage du score
            trex.Image = Properties.Resources.running; // Réinitialise l'image du personnage
            isGameOver = false; // Réinitialise l'état du jeu à "non terminé"
            trex.Top = 290; // Réinitialise la position du personnage

            // Réinitialise la position des obstacles
            foreach (Control x in this.Controls)
            {
                if (x is PictureBox)
                {
                    if ((string)x.Tag == "obstacle")
                    {
                        position = this.ClientSize.Width + rand.Next(500, 800) + (x.Width * 10);
                        x.Left = position;
                    }
                    else if ((string)x.Tag == "obstacleaerien")
                    {
                        x.Left = this.ClientSize.Width + 2000;
                    }
                }
            }

            gameTimer.Start(); // Redémarre le timer du jeu
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }
    }
}