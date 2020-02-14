﻿/*
 *  Snake Game
 * 
 * 
 * 
 *  2020-02-15 Coded by GwangSu Lee
 * 
 * */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snake
{
    public partial class Form1 : Form
    {
        Random rnd = new Random();

        Snake snake;
        Food food;

        int nScore = 0;

        int world_width;
        int world_height;

        Bitmap drawImage;
        public Form1()
        {
            InitializeComponent();

            drawImage = new Bitmap(picCanvas.Width, picCanvas.Height);
            Graphics.FromImage(drawImage).Clear(Color.White);
            picCanvas.CreateGraphics().DrawImageUnscaled(drawImage, 0, 0);

        }

        private void NewGame()
        {
            world_width = picCanvas.Width / Snake.SIZE - 1;
            world_height = picCanvas.Height / Snake.SIZE - 1;

            snake = new Snake(world_width / 2, world_height / 2, world_width, world_height);
            food = null;
            nScore = 0;
            this.Text = string.Format("Score : {0}", nScore);
            MakeNewFood();
        }

        private void MakeNewFood()
        {
            if (food != null && food.eat == false)
                return;

            int x = rnd.Next(1, world_width - 1);
            int y = rnd.Next(1, world_height - 1);

            while (snake.CheckCollide(x, y))
            {
                x = rnd.Next(0, world_width);
                y = rnd.Next(0, world_height);
            }

            food = new Food(x, y);
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            NewGame();
            tmrGame.Enabled = true;
        }

        private void tmrGame_Tick(object sender, EventArgs e)
        {
            Graphics g = Graphics.FromImage(drawImage);
            g.Clear(Color.White);

            snake.Forward();

            if (snake.Eat(food) == true)
            {
                nScore++;
                this.Text = string.Format("Score : {0}", nScore);
            }

            snake.Draw(g);

            if (food != null)
                food.Draw(g);

            MakeNewFood();

            if (snake.IsDead() == true)
            {
                //GameOver
                tmrGame.Enabled = false;
                this.Text = string.Format("Score : {0} [GAME OVER]", nScore);
            }

            picCanvas.CreateGraphics().DrawImageUnscaled(drawImage, 0, 0);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.NumPad4)
            {
                snake.Steering(Snake.DIRECTION.LEFT);
            }
            else if (e.KeyCode == Keys.NumPad6)
            {
                snake.Steering(Snake.DIRECTION.RIGHT);
            }
            else if (e.KeyCode == Keys.NumPad8)
            {
                snake.Steering(Snake.DIRECTION.UP);
            }
            else if (e.KeyCode == Keys.NumPad5)
            {
                snake.Steering(Snake.DIRECTION.DOWN);
            }
        }
    }
}
