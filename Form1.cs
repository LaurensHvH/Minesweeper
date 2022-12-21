using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Minesweeper
{
    public partial class Form1 : Form
    {
        List<Button> buttons = new List<Button>();
        int Spielfeldgroesse = 10;
        int counter = 0;
        Random rnd = new Random();

        public Form1()
        {
            InitializeComponent();
            Spielfeld(Spielfeldgroesse, Spielfeldgroesse);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
        public void Spielfeld(int height, int width)
        {
            if (buttons.Count != 0)
            {
                for (int i = 0; i == buttons.Count(); i++)
                {
                    Button buttonToRemove = buttons[i];
                    this.Controls.Remove(buttonToRemove);
                }
            }

            for (int i = 0;  i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    Button newButton = new Button();
                    this.Controls.Add(newButton);
                    newButton.Location = new Point(i * 25,100 + (j * 25));
                    newButton.Size= new Size(25, 25);
                    buttons.Add(newButton);

                }
            }
            label1.Text = buttons.Count.ToString();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            int breite = 0;
            int laenge = 0;

            try
            {
                breite = Convert.ToInt32(textBox1.Text);
                laenge = Convert.ToInt32(textBox2.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);   
            }

            Spielfeld(breite, laenge);
        }
    }
}
