using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DBSCAN_GUI
{
    public partial class Form1 : Form
    {

        Bitmap bmp;
        Size s;
        private Color boja_crtala;
        private Color ispuna;
        Point p1 = new Point();
        int r;
        int zastavica = 0;
        public List<Tocka> D = new List<Tocka>();

        int brTocaka;
        double epsilon;

        Graphics myGraphics;
        Graphics memoryGraphics;

        public void dodajTocku(int x, int y)
        {
            D.Add(new Tocka(x, y));
        }

        static public Bitmap Copy(Bitmap srcBitmap)
        {
            return new Bitmap(srcBitmap);
        }

        public Form1()
        {
            InitializeComponent();
        }
        
        public void startDBSCAN()
        {
            r = Convert.ToInt32(numericUpDown2.Value);
            epsilon = Convert.ToDouble(textBox1.Text);
            brTocaka = Convert.ToInt32(numericUpDown1.Value);
            foreach (Tocka p in D) p.C = 0;
            Algoritam alg = new Algoritam();

            alg.MinPts = brTocaka;
            alg.eps = epsilon;
            alg.pokreniAlgoritam(D);
            zastavica = 1;

            for (int i = 0; i < alg.klasteri.Count; i++)
            {
                
                switch (i)
                {
                    case 0: boja_crtala = Color.Red; break;
                    case 1: boja_crtala = Color.Cyan; break;
                    case 2: boja_crtala = Color.Green; break;
                    case 3: boja_crtala = Color.YellowGreen; break;
                    case 4: boja_crtala = Color.Orange; break;
                    case 5: boja_crtala = Color.Orchid; break;
                    case 6: boja_crtala = Color.DarkSlateBlue; break;
                    case 7: boja_crtala = Color.BlueViolet; break;
                    case 8: boja_crtala = Color.Brown; break;
                    case 9: boja_crtala = Color.DeepPink; return;

                }

                foreach (Tocka p in alg.klasteri[i]) // ***** BOJENJE KLASTERIRANIH TOČAKA
                {
                    myGraphics.FillEllipse(new SolidBrush(boja_crtala), p.X, p.Y, r, r);

                }
            }

            richTextBox1.Text = alg.text; // ****** ISPIS U TEXTBOX
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {

                Graphics n_gBmp = CreateGraphics();
                Bitmap n_bmp = new Bitmap(s.Width, s.Height);
                n_gBmp.FillRectangle(Brushes.White, 0, 0, bmp.Width, bmp.Height);
                n_gBmp.DrawImage(n_bmp, new Rectangle(0, 0, bmp.Width, bmp.Height));
                bmp = Copy(n_bmp);
                n_gBmp.Dispose();

                D.Clear();
                zastavica = 0;
                richTextBox1.Text = "";

                myGraphics.Dispose();
                memoryGraphics.Dispose();
            }

            catch (Exception exc)
            {

                MessageBox.Show("PLEASE ENTER POINTS!!\n" + "\n(" + exc.Message + ")");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            s = this.Size;
            bmp = new Bitmap(s.Width, s.Height);
            boja_crtala = Color.Black;
            ispuna = Color.Black;
            this.ActiveControl = textBox1;


        }

        private void label1_Click(object sender, EventArgs e)
        {
            // Ništa se ne dešava ....
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            label1.Text = "X = " + e.X.ToString() + " px";
            label2.Text = "Y = " + e.Y.ToString() + " px";

        }
       
        public void NacrtajTocku()
        {
            myGraphics = this.CreateGraphics();
            Bitmap membmp = new Bitmap(s.Width, s.Height, myGraphics);
            memoryGraphics = Graphics.FromImage(membmp);
            memoryGraphics.CopyFromScreen(this.Location.X, this.Location.Y, 0, 0, s);

            myGraphics.DrawEllipse(new Pen(Color.Black), p1.X, p1.Y, r, r);
            myGraphics.FillEllipse(new SolidBrush(ispuna), p1.X, p1.Y, r, r);
        }
        
        public void NacrtajTocku(Tocka p)
        {
            myGraphics = this.CreateGraphics();
            Bitmap membmp = new Bitmap(s.Width, s.Height, myGraphics);           
            myGraphics.DrawEllipse(new Pen(Color.Black), p.X, p.Y, r, r);
            myGraphics.FillEllipse(new SolidBrush(Color.Gray), p.X, p.Y, r, r);           
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {


            if (zastavica == 2)
            {
                MessageBox.Show("PLEASE CLEAR THE POINTS");

            }
            else
            {

                p1.X = e.X;
                p1.Y = e.Y;
                dodajTocku(p1.X, p1.Y);
                NacrtajTocku();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {                                
                foreach (Tocka p in D)
                {
                    NacrtajTocku(p);
                }
                startDBSCAN();
            }

            catch (Exception exc)
            {
                MessageBox.Show("PLEASE ENTER POINTS!!\n" + "\n(" + exc.Message + ")");
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            this.ActiveControl = textBox1;
            textBox1.Focus();
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            r = Convert.ToInt32(numericUpDown2.Value);
            epsilon = Convert.ToDouble(textBox1.Text);
            brTocaka = Convert.ToInt32(numericUpDown1.Value);
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            AboutBox1 ab = new AboutBox1();
            ab.Show();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {

                    if (zastavica == 1)
                    {
                        foreach (Tocka p in D)
                        {
                            NacrtajTocku(p);
                        }
                    }
                    startDBSCAN();
                }

                catch (Exception exc)
                {
                    MessageBox.Show("PLEASE ENTER POINTS!!\n" + "\n(" + exc.Message + ")");
                }
            }
        }

    }
}
