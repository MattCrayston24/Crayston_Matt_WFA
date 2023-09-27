using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace T_Rex_Game
{
    public partial class Form1 : Form
    {
        bool jumping = false;
        int jumpSpeed;
        int force = 12;
        int score = 0;
        int obstacleSpeed = 10;
        Random rand = new Random();
        int position;
        bool isGameOver = false;
        bool isObstacleAerialEnabled = false;
        bool ducking = false;

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void txtscore_Click(object sender, EventArgs e)
        {

        }

        public Form1()
        {
            InitializeComponent();
            GameReset();
        }

        private void MainGameTimerEvent(object sender, EventArgs e)
        {
            trex.Top += jumpSpeed;

            txtscore.Text = "Score:" + score;

            if (jumping == true && force < 0)
            {
                jumping = false;
            }

            if (jumping == true)
            {
                jumpSpeed = -12;
                force -= 1;
            }
            else
            {
                jumpSpeed = 12;
            }

            if (trex.Top > 289 && jumping == false)
            {
                force = 12;
                trex.Top = 290;
                jumpSpeed = 0;
            }

            foreach (Control x in this.Controls)
            {
                if (x is PictureBox)
                {
                    if ((string)x.Tag == "obstacle" || ((string)x.Tag == "obstacleaerien" && score > 5))
                    {
                        x.Left -= obstacleSpeed;

                        if (x.Left < -100)
                        {
                            x.Left = this.ClientSize.Width + rand.Next(200, 500) + (x.Width * 15);
                            if ((string)x.Tag == "obstacle")
                            {
                                score++;
                            }
                        }

                        if (trex.Bounds.IntersectsWith(x.Bounds))
                        {
                            gameTimer.Stop();
                            trex.Image = Properties.Resources.dead;
                            txtscore.Text += " Press R to restart the game!";
                            isGameOver = true;
                        }
                    }
                }
            }

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
                jumping = true;
            }

            if (e.KeyCode == Keys.Down && !jumping)
            {
                ducking = true;
                trex.Top = 250;
                trex.Image = Properties.Resources.duck2;
            }
        }

        private void keyisup(object sender, KeyEventArgs e)
        {
            if (jumping == true)
            {
                jumping = false;
            }

            if (e.KeyCode == Keys.R && isGameOver == true)
            {
                GameReset();
            }

            if (e.KeyCode == Keys.Down)
            {
                ducking = false;
                trex.Top = 290;
                trex.Image = Properties.Resources.running;
            }
        }

        private void GameReset()
        {
            force = 12;
            jumpSpeed = 0;
            jumping = false;
            score = 0;
            obstacleSpeed = 10;
            txtscore.Text = "Score: " + score;
            trex.Image = Properties.Resources.running;
            isGameOver = false;
            trex.Top = 290;

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

            gameTimer.Start();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }
    }
}