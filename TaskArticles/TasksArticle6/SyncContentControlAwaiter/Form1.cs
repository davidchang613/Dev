using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Threading.Tasks;


namespace SyncContextControlAwaiter
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        



        private void BtnNoAwaiter_Click(object sender, EventArgs e)
        {
            ThreadPool.QueueUserWorkItem(delegate
            {
                try
                {
                    string text = "This should cause a problem, as we have spawned " +
                                    "background thread using ThreadPool" + 
                                    "Which is not the correct thread to change the UI control " +
                                    "so should cause a CrossThread Violation";
                    textBox1.Text = text;
                }
                catch (InvalidOperationException ioex)
                {
                    MessageBox.Show(ioex.Message);
                }
            });
        }



        private void BtnSyncContextAwaiter_Click(object sender, EventArgs e)
        {
            ThreadPool.QueueUserWorkItem(async delegate
            {
                string text = "This should work just fine thanks to our lovely \"ControlAwaiter\" " +
                                "which ensures correct thread marshalling";
                await textBox1;
                textBox1.Text = text;
            });

        }
    }



    


    

}
