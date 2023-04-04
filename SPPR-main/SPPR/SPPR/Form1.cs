using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SPPR
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

        }

        double function(double a, double b, double c, double d, double x)
        {
            double f = a * Math.Sin(b * x) + c * Math.Cos(d * x);
            return f;
        }
        (double a , int b) maxChar(List<double> mas)
        {
            double max = mas[0];
            int numI = 0;
            for (int i = 0; i < mas.Count; i++)
            {
                if (mas[i] > max)
                {
                    max = mas[i];
                    numI = i;
                }
            }
            return (max, numI);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            double Rcheck=0;
            int Tnum=0;
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            listBox3.Items.Clear();

            chart1.Series["Series1"].Points.Clear();
            chart1.Series["Series2"].Points.Clear();
            chart1.Series["Series3"].Points.Clear();
            double A, B, C, D;
            A = Convert.ToDouble(textBox1.Text);
            B = Convert.ToDouble(textBox2.Text);
            C = Convert.ToDouble(textBox3.Text);
            D = Convert.ToDouble(textBox4.Text);
            double rightBorder = Convert.ToDouble(textBox6.Text);
            double leftBorder = Convert.ToDouble(textBox5.Text);

            double x = leftBorder;
            double f = 0.0;
            while (x < rightBorder)
            {
                f = function(A, B, C, D, x);
                chart1.Series["Series1"].Points.AddXY(x, f);
                x = x + 0.1;
            }
            //  perebor
            if (radioButton1.Checked)
            {
                double eps = Convert.ToDouble(textBox8.Text);
                int maxIter = Convert.ToInt32(textBox7.Text);
                List<double> xValue = new List<double>();
                double ZleftBorder = function(A, B, C, D, leftBorder);
                double ZrightBorder = function(A, B, C, D, rightBorder);
                xValue.Add(leftBorder);
                xValue.Add(rightBorder);
                chart1.Series["Series2"].Points.AddXY(leftBorder, -6);
                chart1.Series["Series2"].Points.AddXY(rightBorder, -6);
                List<double> Zfunc = new List<double>();
                Zfunc.Add(ZleftBorder);
                Zfunc.Add(ZrightBorder);
                List<double> R = new List<double>();
                double min = ZleftBorder;
                double thisX = leftBorder;
                if (min > ZrightBorder)
                {
                    min = ZrightBorder;
                    thisX = rightBorder;
                }
                int k = 2;
                int t = 0;
                while ( k < maxIter)
                {
                    R.Clear();// Вот сюда воткнул 
                    Rcheck = 0;
                    listBox1.Items.Add(("___________Итерация_______", k));
                    for (int i = 0; i < k - 1; i++) { 
                        R.Add(xValue[i + 1] - xValue[i]);
                        if ((xValue[i + 1] - xValue[i]) >= Rcheck)
                        {
                            Rcheck = xValue[i + 1] - xValue[i];
                            Tnum = i;
                        }
                        listBox1.Items.Add(("xfirst=", xValue[i], " ", "xlast=", xValue[i + 1], "R", xValue[i + 1] - xValue[i]));
                    }
                    listBox3.Items.Add(("Rmax = ", Rcheck, "  ", "T=", Tnum, "  ", "X[t+1]=", xValue[Tnum + 1], "X[t]=", xValue[Tnum], "  ", "  ", "X[t+1] - X[t] = ", xValue[Tnum + 1] - xValue[Tnum]));
                   
                    if ((xValue[Tnum + 1] - xValue[Tnum]) <= eps) {
                        MessageBox.Show("ВЫХОД ПО ТОЧНОСТИ");
                        break; 
                    }

                    (double a, int b) Char = maxChar(R);
                    t = Char.b;
                    R.Clear();
                    double nextX = (xValue[t + 1] + xValue[t]) / 2;
                    chart1.Series["Series2"].Points.AddXY(nextX, -6);
                    double currentZ = function(A, B, C, D, nextX);
                    if (currentZ < min)
                    {
                        min = currentZ;
                        thisX = nextX;
                    }
                    xValue.Add(nextX);
                    Zfunc.Add(currentZ);
                    xValue.Sort();
                    for (int i = 0; i < xValue.Count(); i++)
                        Zfunc[i] = function(A, B, C, D, xValue[i]);
                    k++;
                    listBox2.Items.Add(("x=", nextX, "  ", "z=", currentZ));
                }
                chart1.Series["Series3"].Points.AddXY(thisX, min);
                label15.Text = Convert.ToString(min);
                label16.Text = Convert.ToString(thisX);
                label17.Text = Convert.ToString(k);
            }
            // Strongin
            if (radioButton2.Checked)
            {
                double r = Convert.ToDouble(textBox9.Text);
                double eps = Convert.ToDouble(textBox8.Text);
                int maxIter = Convert.ToInt32(textBox7.Text);
                double M = 0, m;
                List<double> xValue = new List<double>();
                double ZleftBorder = function(A, B, C, D, leftBorder);
                double ZrightBorder = function(A, B, C, D, rightBorder);
                xValue.Add(leftBorder);
                xValue.Add(rightBorder);
                chart1.Series["Series2"].Points.AddXY(leftBorder, -6);
                chart1.Series["Series2"].Points.AddXY(rightBorder, -6);
                List<double> Zfunc = new List<double>();
                Zfunc.Add(ZleftBorder);
                Zfunc.Add(ZrightBorder);
                List<double> R = new List<double>();
                int k = 2;
                int t = 0;
                while (k < maxIter)
                {
                    R.Clear();// Вот сюда воткнул 
                    Rcheck = 0;
                    listBox1.Items.Add(("___________Итерация_______",k));
                    for (int i=0;i<k-1;i++)
                    {
                        double tmpM = Math.Abs((Zfunc[i + 1] - Zfunc[i]) / (xValue[i + 1] - xValue[i]));
                        if (M < tmpM) M = tmpM;
                    }
                    if (M == 0) m = 1;
                    else m = r * M;
                    for (int i = 0; i < k - 1; i++)
                    {
                        R.Add(m * (xValue[i + 1] - xValue[i]) + (Zfunc[i + 1] - Zfunc[i]) * (Zfunc[i + 1] - Zfunc[i]) / (m * (xValue[i + 1] - xValue[i])) - 2 * (Zfunc[i + 1] + Zfunc[i]));
                        listBox1.Items.Add(("xfirst=", xValue[i], " ", "xlast=", xValue[i + 1], "R=", m * (xValue[i + 1] - xValue[i]) + (Zfunc[i + 1] - Zfunc[i]) * (Zfunc[i + 1] - Zfunc[i]) / (m * (xValue[i + 1] - xValue[i])) - 2 * (Zfunc[i + 1] + Zfunc[i])));
                        if ((m * (xValue[i + 1] - xValue[i]) + (Zfunc[i + 1] - Zfunc[i]) * (Zfunc[i + 1] - Zfunc[i]) / (m * (xValue[i + 1] - xValue[i])) - 2 * (Zfunc[i + 1] + Zfunc[i])) >= Rcheck)
                        {
                            Rcheck = m * (xValue[i + 1] - xValue[i]) + (Zfunc[i + 1] - Zfunc[i]) * (Zfunc[i + 1] - Zfunc[i]) / (m * (xValue[i + 1] - xValue[i])) - 2 * (Zfunc[i + 1] + Zfunc[i]);
                            Tnum = i;
                        }
                    
                    }
                    listBox3.Items.Add(("Rmax = " , Rcheck, "  ","T=",Tnum,"  ","X[t+1]=",xValue[Tnum+1], "X[t]=", xValue[Tnum], "  ", "  ","X[t+1] - X[t] = ", xValue[Tnum + 1] - xValue[Tnum]));
                    if ((xValue[Tnum+1] - xValue[Tnum]) <= eps)
                    {
                        MessageBox.Show("ВЫХОД ПО ТОЧНОСТИ");
                        break;
                    }

                    (double a, int b) Char = maxChar(R);
                    t = Char.b;
                    R.Clear();
                    double nextX = (xValue[t + 1] + xValue[t]) / 2 - (Zfunc[t + 1] - Zfunc[t]) / (2 * m);
                    chart1.Series["Series2"].Points.AddXY(nextX, -6);
                    double currentZ = function(A, B, C, D, nextX);
                    xValue.Add(nextX);
                    Zfunc.Add(currentZ);
                    xValue.Sort();
                    for (int i = 0; i < xValue.Count(); i++)
                        Zfunc[i] = function(A, B, C, D, xValue[i]);

                    listBox2.Items.Add(("x=", nextX, "  ", "z=", currentZ));
                    k++;
                }
                chart1.Series["Series3"].Points.AddXY(xValue[t], Zfunc[t]);
                label15.Text = Convert.ToString(Zfunc[t]);
                label16.Text = Convert.ToString(xValue[t]);
                label17.Text = Convert.ToString(k);
            }
            // Piyavskii
            if (radioButton3.Checked)
            {
                double r = Convert.ToDouble(textBox9.Text);
                double eps = Convert.ToDouble(textBox8.Text);
                int maxIter = Convert.ToInt32(textBox7.Text);
                double M = 0, m;
                List<double> xValue = new List<double>();
                double ZleftBorder = function(A, B, C, D, leftBorder);
                double ZrightBorder = function(A, B, C, D, rightBorder);
                xValue.Add(leftBorder);
                xValue.Add(rightBorder);
                chart1.Series["Series2"].Points.AddXY(leftBorder, -6);
                chart1.Series["Series2"].Points.AddXY(rightBorder, -6);
                List<double> Zfunc = new List<double>();
                Zfunc.Add(ZleftBorder);
                Zfunc.Add(ZrightBorder);
                List<double> R = new List<double>();
                int k = 2;
                int t = 0;
                while(k < maxIter)
                {
                    R.Clear();// Вот сюда воткнул 
                    Rcheck = 0;
                    listBox1.Items.Add(("___________Итерация_______", k));
                    for (int i = 0; i < k - 1; i++)
                    {
                        double tmpM = Math.Abs((Zfunc[i + 1] - Zfunc[i]) / (xValue[i + 1] - xValue[i]));
                        if (M < tmpM) M = tmpM;
                    }
                    if (M == 0) m = 1;
                    else m = r * M;
                    for (int i = 0; i < k - 1; i++)
                    {
                        R.Add(m * (xValue[i + 1] - xValue[i]) / 2.0 - (Zfunc[i + 1] + Zfunc[i]) / 2.0);
                        listBox1.Items.Add(("xfirst=", xValue[i], " ", "xlast=", xValue[i + 1], "R=", m * (xValue[i + 1] - xValue[i]) / 2.0 - (Zfunc[i + 1] + Zfunc[i]) / 2.0));
                        if ((m * (xValue[i + 1] - xValue[i]) / 2.0 - (Zfunc[i + 1] + Zfunc[i]) / 2.0) >= Rcheck)
                        {
                            Rcheck = (m * (xValue[i + 1] - xValue[i]) / 2.0 - (Zfunc[i + 1] + Zfunc[i]) / 2.0);
                            Tnum = i;
                        }

                    }
                    listBox3.Items.Add(("Rmax = ", Rcheck, "  ", "T=", Tnum, "  ", "X[t+1]=", xValue[Tnum + 1], "X[t]=", xValue[Tnum], "  ", "  ", "X[t+1] - X[t] = ", xValue[Tnum + 1] - xValue[Tnum]));
                    if ((xValue[Tnum + 1] - xValue[Tnum]) <= eps)
                    {
                        MessageBox.Show("ВЫХОД ПО ТОЧНОСТИ");
                        break;
                    }

                    (double a, int b) Char = maxChar(R);
                    t = Char.b;
                    R.Clear();
                    double nextX = (xValue[t + 1] + xValue[t]) / 2 - (Zfunc[t + 1] - Zfunc[t]) / (2 * m);
                    chart1.Series["Series2"].Points.AddXY(nextX, -6);
                    double currentZ = function(A, B, C, D, nextX);
                    xValue.Add(nextX);
                    Zfunc.Add(currentZ);
                    xValue.Sort();
                    for (int i = 0; i < xValue.Count(); i++)
                        Zfunc[i] = function(A, B, C, D, xValue[i]);
                    k++;
                    listBox2.Items.Add(("x=", nextX, "  ", "z=", currentZ));
                }
                chart1.Series["Series3"].Points.AddXY(xValue[t], Zfunc[t]);
                label15.Text = Convert.ToString(Zfunc[t]);
                label16.Text = Convert.ToString(xValue[t]);
                label17.Text = Convert.ToString(k);
            }

        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }
    }
}