using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Example_BruteForce
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Menu());
        }

        public static void Log(System.Windows.Forms.RichTextBox richTextBox1, string text)
        {
            richTextBox1.AppendText("["+DateTime.Now.Hour+":"+DateTime.Now.Minute+"]: "+text+"\r\n");
        }
    }
}
