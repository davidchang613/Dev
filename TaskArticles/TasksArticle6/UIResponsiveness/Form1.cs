using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace UIResponsiveness
{
    public partial class Form1 : Form
    {
        Random rand = new Random();
        List<string> suffixes = new List<string>() { 
            "a","b","c","d","e","f","g","h","i","j","k","l",
            "m","n","o","p","q","r","s","t","u","v","w","x","y","z"};


        public Form1()
        {
            InitializeComponent();


        }

        private async void button1_Click(object sender, EventArgs e)
        {
            RunTaskToGetText();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            RunTaskToGetText();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            RunTaskToGetText();
        }


        private async void RunTaskToGetText()
        {
            List<string> results = await GetSomeText(
                suffixes[rand.Next(0,suffixes.Count())], 500000);

            textBox1.Text += results[0];
            textBox1.Text += results[1];
            textBox1.Text += results[results.Count-2];
            textBox1.Text += results[results.Count-1];
        }



        public async Task<List<string>> GetSomeText(string prefix, int upperLimit)
        {
            List<string> values = new List<string>();
            values.Add("=============== New Task kicking off=================\r\n");
            for (int i = 0; i < upperLimit; i++)
            {
                values.Add(String.Format("Value_{0}_{1}\r\n", prefix, i.ToString()));
            }
            values.Add("=============== New Task Done=================\r\n");
            return values;
        }
    }
}
