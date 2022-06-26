using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Actual_Mandlebrot
{
    public partial class Form1 : Form
    {
        const int Iterations = 1000;
        const int DigitsToRoundTo = 15;
        
        double MaxX = 2;
        double MaxY = 2;
        double MinX = -2;
        double MinY = -2;
        double RangeX = 4;
        double RangeY = 4;
        double OldMinX = -2;
        double OldMinY = -2;

        public Form1()
        {
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
            InitializeComponent();          
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            xLabel.Hide();
        }

        private void ConvertPointToArgonVersion(ref double x, ref double y)
        {
            
            //x * range / width of screen - minimum value
            x = (x * RangeX / this.Width) + OldMinX;
            y = (y * RangeY / this.Height) + OldMinY;

            //x = Math.Round(x, DigitsToRoundTo);
           // y = Math.Round(y, DigitsToRoundTo);
        }

        private void GenerateSet()
        {
            Graphics graphics = this.CreateGraphics();
            SolidBrush Bbrush = new SolidBrush(Color.Black);
            SolidBrush Pbrush = new SolidBrush(Color.Purple);
            SolidBrush Wbrush = new SolidBrush(Color.White);
            SolidBrush AWbrush = new SolidBrush(Color.AntiqueWhite);
            SolidBrush BAbrush = new SolidBrush(Color.BlanchedAlmond);

            double xPoint = 1;
            double yPoint = 1;

            for(double x = 0; x < Width; x++)
            {
                for (double y = 0; y < Height; y++)
                {
                    xPoint = x;
                    yPoint = y;
                    ConvertPointToArgonVersion(ref xPoint, ref yPoint);
                    int amountOfIterations = fz(xPoint, yPoint);

                    if (amountOfIterations == -1)
                    {
                        graphics.FillRectangle(Bbrush, Convert.ToInt32(x), Convert.ToInt32(y), 1, 1);
                    }
                    else if(amountOfIterations > Iterations / 5)
                    {
                        graphics.FillRectangle(Wbrush, Convert.ToInt32(x), Convert.ToInt32(y), 1, 1);
                    }
                    else if (amountOfIterations > Iterations / 10)
                    {
                        graphics.FillRectangle(AWbrush, Convert.ToInt32(x), Convert.ToInt32(y), 1, 1);
                    }
                    else if (amountOfIterations > Iterations / 20)
                    {
                        graphics.FillRectangle(AWbrush, Convert.ToInt32(x), Convert.ToInt32(y), 1, 1);
                    }
                    else if (amountOfIterations > Iterations / 40)
                    {
                        graphics.FillRectangle(Pbrush, Convert.ToInt32(x), Convert.ToInt32(y), 1, 1);
                    }
                    
                }   
            }

            
        }

        //cR is the real part of c, cI is the imaginery part of c, etc
        private int fz(double cR,double cI, double zR = 0, double zI = 0, int count = 0)
        {
           if(count > Iterations)
           {
                return -1;
           }
           //if( (cR + zR) * (cR + zR) - cI - zI > 2)
           if(zR > 2)
           {
                return count;
           }
           //                   real part of new z, imaginary part
            return fz(cR, cI, ((zR * zR) - (zI * zI)) + cR, (2 * zI * zR) + cI, count + 1);
        }

        private void START_Click(object sender, EventArgs e)
        {
            START.Hide();
            GenerateSet();
            xLabel.Show();
            MousePosTimer.Start();
        }

        private void MousePosTimer_Tick(object sender, EventArgs e)
        {
            
            double x = Cursor.Position.X;
            double y = Cursor.Position.Y;

            ConvertPointToArgonVersion(ref x, ref y);
            x =  Math.Round(x, DigitsToRoundTo);
            y =  Math.Round(y, DigitsToRoundTo);
            xLabel.Text = x.ToString() + "," + y.ToString();
        }

        private void Form1_MouseDown_1(object sender, MouseEventArgs e)
        {
            MaxX = Cursor.Position.X;
            MaxY = Cursor.Position.Y;
            ConvertPointToArgonVersion(ref MaxX, ref MaxY);
        }

        private void Form1_MouseUp_1(object sender, MouseEventArgs e)
        {
            MinX = Cursor.Position.X;
            MinY = Cursor.Position.Y;
            ConvertPointToArgonVersion(ref MinX, ref MinY);
            OldMinX = MinX;
            OldMinY = MinY;
            RangeX = MaxX - MinX;
            RangeY = MaxY - MinY;
            
            xLabel.Hide();
            Graphics graphics = CreateGraphics();
            graphics.Clear(Color.Blue);
            GenerateSet();
        }
    }
}
