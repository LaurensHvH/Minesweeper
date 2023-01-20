using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;

namespace Minesweeper
{
    public partial class Form1 : Form
    {
        int[,] feld;
        int[,] oldvalues;
        Button[,] buttons;
        int size = 40;
        public Form1()
        {
            InitializeComponent();
            this.Text = "Minesweeper";
            Spielfeld(8,8,4);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }
        public void Spielfeld(int width, int height, int mines)
        {
            feld = new int[width, height];
            oldvalues = new int[width, height];
            buttons = new Button[width, height]; 
            Random rnd = new Random();

            for (int i = 0;  i < feld.GetLength(0); i++)
            {
                for (int j = 0; j < feld.GetLength(1); j++)
                {

                    Button newButton = new Button();
                    this.Controls.Add(newButton);
                    buttons[i, j] = newButton;
                    newButton.Left = i * size;
                    newButton.Top = j * size;
                    newButton.Size= new Size(size, size);
                    newButton.Click += NewButton_Click;
                    newButton.MouseUp += NewButton_MouseUp;
                    newButton.Font = new Font(" ", size/3);
                    
                }
            }
            while(mines > 0)
            {
                int x = rnd.Next(0, width);
                int y = rnd.Next(0, height);
                if (feld[x, y] != -1)
                {
                    feld[x, y] = -1;
                    for (int xneben = -1; xneben <= 1; xneben++)
                    {
                        for (int yneben = -1; yneben <= 1; yneben++)
                        {
                            if (x + xneben < 0 || x + xneben >= width || y + yneben < 0 || y + yneben >= height) continue;
                            if (feld[x + xneben, y + yneben] != -1)
                            {
                                feld[x + xneben, y + yneben]++;
                            }
                        }
                    }
                    mines--;
                }
            }
        }

        private void NewButton_MouseUp(object sender, EventArgs e)
        {
            Button newButton = (Button)sender;
            int x = newButton.Left / size;
            int y = newButton.Top / size;
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Right)
            {
                if (feld[x, y] != -2)
                {
                    oldvalues[x, y] = feld[x, y];
                    feld[x, y] = -2;
                    newButton.Text = "\U0001F6A9";
                }
                else
                {
                    newButton.Text = " ";
                    feld[x, y] = oldvalues[x, y];
                }
                check_win();
            }
        }

        private void check_win()
        {
            int counter = 0;
            for (int i = 0; i < feld.GetLength(0); i++)
            {
                for (int j = 0; j < feld.GetLength(1); j++)
                {
                    if (feld[i, j] == -1)
                    {
                        counter++;
                    }
                }
            }
            if (counter == 0)
            {
                MessageBox.Show("Gewonnen!");
                aufdecken();
            }

        }
        private void NewButton_Click(object sender, EventArgs e)
        {
            Button newButton = (Button)sender;
            int x = newButton.Left / size;
            int y = newButton.Top / size;
            MouseEventArgs me = (MouseEventArgs) e;
            if (me.Button == MouseButtons.Left)
            {
                if (feld[x, y] == -1)
                {
                    newButton.Text = "\U0001F4A3";
                    MessageBox.Show("Verloren");
                    aufdecken();

                }
                else if (feld[x, y] == 0)
                {
                    flutfill(x, y);
                }
                else if (feld[x, y] == -2)
                {

                }
                else 
                {
                    newButton.Text = " " + feld[x, y];
                    newButton.Enabled = false;
                }
            }
        }
        private void flutfill(int x, int y)
        {
            Stack<Point> stack = new Stack<Point>();
            stack.Push(new Point(x, y));
            while (stack.Count > 0)
            {
                Point p = stack.Pop();
                if (p.X < 0 || p.Y < 0) continue;
                if (p.X >= feld.GetLength(0) || p.Y >= feld.GetLength(1)) continue;

                if (!buttons[p.X, p.Y].Enabled) continue;
                    buttons[p.X, p.Y].Enabled = false;
                buttons[p.X, p.Y].Text = "0";
                if (feld[p.X, p.Y] != 0)
                {
                    buttons[p.X, p.Y].Text = "" + feld[p.X, p.Y];
                    continue;
                }
                stack.Push(new Point(p.X, p.Y + 1));
                stack.Push(new Point(p.X - 1, p.Y));
                stack.Push(new Point(p.X + 1, p.Y));
                stack.Push(new Point(p.X, p.Y - 1));
            }
        }
        private void aufdecken()
        {
            for (int i = 0; i < feld.GetLength(0); i++)
            {
                for (int j = 0; j < feld.GetLength(1); j++)
                {
                    buttons[i, j].Enabled = false;
                    if (feld[i,j] == -1) 
                    {
                        buttons[i, j].Text = "💣";
                    }
                    else if (feld[i, j] == -2)
                    {
                        buttons[i, j].Text = "🚩";
                    }
                    else
                        buttons[i, j].Text = feld[i, j].ToString();

                }
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            int height = 8;
            int width = 8;
            int mines = 10;
            
            try
            {
                height = Convert.ToInt32(textBox2.Text);
                width = Convert.ToInt32(textBox1.Text);
                mines = Convert.ToInt32(textBox3.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            if(height * width  < mines)
            {
                MessageBox.Show("Zu viele Mienen!");
            }
            else
            {
            deletebuttons();
            Spielfeld(width, height, mines);
            }

        } 
        public void deletebuttons()
        {
            for (int i = 0; i < feld.GetLength(0); i++)
            {
                for (int j = 0; j < feld.GetLength(1); j++)
                {
                    this.Controls.Remove(buttons[i, j]);
                }
            }
        }
        private void btn_reset_Click(object sender, EventArgs e)
        {
            deletebuttons();
            Spielfeld(8, 8, 10);
        }
    }
}
